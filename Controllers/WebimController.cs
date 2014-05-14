using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Json;
using Spacebuilder.Webim;
using Tunynet.Common;
using Spacebuilder.Common;
using Spacebuilder.Group;
using Tunynet.Utilities;
using System.Configuration;
using Webim;

namespace Spacebuilder.Webim.Controllers
{
    public class WebimController : Controller
    {

        //TODO: There should be userService, groupService and FollowService

        private WebimService webimService = new WebimService();

        private WebimModel model = null;

        private WebimPlugin plugin = null;

        private WebimClient client = null;

        private WebimEndpoint endpoint = null;

        public WebimController()
        {
            this.model = new WebimModel();
            this.plugin = new WebimPlugin();
            this.endpoint = this.plugin.Endpoint();
        }

        private WebimEndpoint CurrentEndpoint()
        {
            IUser u = UserContext.CurrentUser;
            return webimService.Mapping(u);
        }

        private WebimClient CurrentClient(string ticket = "")
        {
            WebimClient c = new WebimClient(
                CurrentEndpoint(),
                WebimConfig.Instance().Domain,
                WebimConfig.Instance().APIkey,
                WebimConfig.Instance().Host,
                WebimConfig.Instance().Port);
            c.Ticket = ticket;
            return c;
        }

        // GET: /Webim/Boot
        [HttpGet]
        public ActionResult Boot()
        {
            int iisVersion = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            bool aspx = iisVersion < 7;
            IUser user = UserContext.CurrentUser;
            string setting = webimService.GetSetting(user.UserId);
            string body = string.Format(@"var _IMC = {{
	            production_name: 'jinhusns',
	            version: '1.0',
	            path: '{0}',
	            uiPath: '{1}',
	            is_login: true,
                is_visitor: false,
	            user: '',
	            setting: {2},
	            menu: '',
				enable_room: true,
				enable_noti: false,
	            enable_chatlink: '',
	            enable_shortcut: '',
	            enable_menu: 'false',
	            theme: 'base',
	            local: 'zh-CN',
				emot: 'default',
				opacity: 80,
                show_unavailable: false,
                aspx: {3},
	            min: """" //window.location.href.indexOf(""webim_debug"") != -1 ? """" : "".min""
            }};
            
            _IMC.script = window.webim ? '' : ('<link href=""' + _IMC.uiPath + 'static/webim'+ _IMC.min + '.css?' + _IMC.version + '"" media=""all"" type=""text/css"" rel=""stylesheet""/><link href=""' + _IMC.uiPath + 'static/themes/' + _IMC.theme + '/jquery.ui.theme.css?' + _IMC.version + '"" media=""all"" type=""text/css"" rel=""stylesheet""/><script src=""' + _IMC.uiPath + 'static/webim' + _IMC.min + '.js?' + _IMC.version + '"" type=""text/javascript""></script><script src=""' + _IMC.uiPath + 'static/i18n/webim-' + _IMC.local + '.js?' + _IMC.version + '"" type=""text/javascript""></script>');
            _IMC.script += '<script src=""' + _IMC.uiPath + 'webim.' + _IMC.production_name + '.js?' + _IMC.version + '"" type=""text/javascript""></script>';
            document.write( _IMC.script );

            ", WebUtility.ResolveUrl("~/Webim/"), WebUtility.ResolveUrl("~/Applications/Webim/UI/"), setting, aspx.ToString().ToLower());

            return Content(body, "text/javascript");
        }

        // POST: /Webim/Online
        [HttpPost]
        public ActionResult Online()
        {
            //当前用户登录
            IUser user = UserContext.CurrentUser;
            if (user == null)
                return Json(
                    new { success = false, error_msg = "尚未登录" },
                    JsonRequestBehavior.AllowGet
                );

            IEnumerable<WebimEndpoint> buddies = webimService.GetBuddies(user.UserId);
            IEnumerable<WebimGroup> groups = webimService.GetGroups(user.UserId);
            //Forward Online to IM Server
            WebimClient client = CurrentClient();
            var buddyIds = from b in buddies select b.Id;
            var groupIds = from g in groups select g.Id;
            try
            {
                JsonObject json = client.Online(buddyIds, groupIds);
                Debug.WriteLine(json.ToString());

                if (json.ContainsKey("status"))
                {
                    return Json(
                        new { success = false, error_msg = json["message"] },
                        JsonRequestBehavior.AllowGet
                    );
                }

                Dictionary<string, string> conn = new Dictionary<string, string>();
                conn.Add("ticket", (string)json["ticket"]);
                conn.Add("domain", client.Domain);
                conn.Add("jsonpd", (string)json["jsonpd"]);
                conn.Add("server", (string)json["jsonpd"]);
                conn.Add("websocket", (string)json["websocket"]);

                //Update Buddies 
                JsonObject presenceObj = json["buddies"].ToJsonObject();
                buddies = buddies.Select(b =>
                  {
                      if (presenceObj.ContainsKey(b.Id))
                      {
                          b.Presence = "online";
                          b.Show = (string)presenceObj[b.Id];
                      }
                      return b;
                  });

                //Groups with count
                JsonObject grpCountObj = json["groups"].ToJsonObject();
                groups = groups.Select(g =>
                {
                    if (grpCountObj.ContainsKey(g.Id))
                    {
                        g.Count = (int)grpCountObj[g.Id];
                    }
                    return g;
                });
                //{"success":true,
                // "connection":{
                // "ticket":"fcc493f7a7b17cfadbf4|admin",
                // "domain":"webim20.cn",
                // "server":"http:\/\/webim20.cn:8000\/packets"},
                // "buddies":[
                //           {"uid":"5","id":"demo","nick":"demo","group":"stranger","url":"home.php?mod=space&uid=5","pic_url":"picurl","status":"","presence":"online","show":"available"}],
                // "rooms":[],
                // "server_time":1370751451399.4,
                // "user":{"uid":"1","id":"admin","nick":"admin","pic_url":"pickurl","show":"available","url":"home.php?mod=space&uid=1","status":""},
                // "new_messages":[]}


                var buddyArray = (from b in buddies select b.Data()).ToArray();
                var groupArray = (from g in groups select g.Data()).ToArray();
                return Json(new
                {
                    success = true,
                    connection = conn,
                    buddies = buddyArray,
                    groups = groupArray,
                    rooms = groupArray,
                    server_time = Timestamp(),
                    user = client.Endpoint.Data()
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(
                    new { success = false, error_msg = e.ToString() },
                    JsonRequestBehavior.AllowGet
                );
            }

        }

        // POST: /Webim/Offline
        [HttpPost]
        public ActionResult Offline()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            c.Offline();
            return Json("ok");
        }

        //POST: /Webim/Message
        [HttpPost]
        public ActionResult Message()
        {
            IUser user = UserContext.CurrentUser;
            WebimClient c = CurrentClient(Request["ticket"]);
            string type = Request["type"];
            string offline = Request["offline"];
            string to = Request["to"];
            string body = Request["body"];
            string style = Request["style"];
            WebimMessage msg = new WebimMessage(type, to, user.NickName, body, style, Timestamp());
            c.Publish(msg);
            webimService.InsertHistory(user.UserId, offline, msg);
            return Json("ok");
        }

        //POST: /Webim/Presence
        [HttpPost]
        public ActionResult Presence()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string show = Request["show"];
            string status = Request["status"];
            c.Publish(new WebimPresence(show, status));
            return Json("ok");
        }

        //POST: /Webim/Status
        [HttpPost]
        public ActionResult Status()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string to = Request["to"];
            string show = Request["show"];
            string status = Request["status"];
            WebimStatus s = new WebimStatus(to, show, status);
            c.Publish(s);
            return Json("ok");
        }

        //POST: /Webim/Refresh
        [HttpPost]
        public ActionResult Refresh()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            c.Offline();
            return Json("ok");
        }

        //POST: /Webim/Setting
        [HttpPost]
        public ActionResult Setting()
        {
            IUser user = UserContext.CurrentUser;
            string data = Request["data"];
            webimService.updateSetting(user.UserId, data);
            return Json("ok");
        }

        //GET: /Webim/History
        [HttpGet]
        public ActionResult History()
        {
            IUser u = UserContext.CurrentUser;
            string id = Request["id"];
            string type = Request["type"];
            IEnumerable<HistoryEntity> histories = webimService.GetHistories(u.UserId, id, type);
            var list = from e in histories select webimService.mapping(e);
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //POST: /Webim/ClearHistory
        public ActionResult ClearHistory()
        {
            string id = Request["id"];
            webimService.ClearHistories(id);
            return Json("ok");

        }

        //GET: /Webim/DownloadHistory
        [HttpGet]
        public ActionResult DownloadHistory()
        {
            IUser u = UserContext.CurrentUser;
            string id = Request["id"];
            string type = Request["type"];
            IEnumerable<HistoryEntity> histories = webimService.GetHistories(u.UserId, id, type);
            return View(histories);
        }

        //GET: /Webim/Members
        [HttpGet]
        public ActionResult Members()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string gid = Request["id"];
            JsonArray members = c.Members(gid);
            return Json(members);
            /*
            List<Dictionary<string,string>> list = new List<Dictionary<string,string>>();
            foreach (JsonObject m in (JsonArray)obj[gid])
            { 
                Dictionary<string,string> data = new Dictionary<string,string>();
                data["id"] = (string)m["id"];
                data["nick"] = (string)m["nick"];
                list.Add(data);
            }
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
            */
        }

        //POST: /Webim/Join
        [HttpPost]
        public ActionResult Join()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string gid = Request["id"];
            JsonObject o = c.Join(gid);
            return Content(o.ToString(), "text/json");
        }

        //POST: /Webim/Leave
        [HttpPost]
        public ActionResult Leave()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            c.Leave(Request["id"]);
            return Json("ok");
        }

        //GET: /Webim/Buddies
        [HttpGet]
        public ActionResult Buddies()
        {
            IUser u = UserContext.CurrentUser;
            IEnumerable<long> ids = (from id in
                                         Request["ids"].Split(new char[1] { ',' })
                                     select long.Parse(id));
            IEnumerable<WebimEndpoint> buddies = webimService.GetBuddiesByIds(ids);
            var list = from buddy in buddies select buddy.Data();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //GET: /Webim/Notifications
        [HttpGet]
        public ActionResult Notifications()
        {
            return Json(webimService.GetNotifications(), JsonRequestBehavior.AllowGet);
        }

        //GET: /Webim/Menus
        [HttpGet]
        public ActionResult Menus()
        {
            return Json(webimService.GetMenuList(), JsonRequestBehavior.AllowGet);
        }

        private double Timestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }
    }
}
