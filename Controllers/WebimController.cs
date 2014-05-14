using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Json;
using Spacebuilder.Webim;
using System.Configuration;
using Tunynet.Utilities;
using Tunynet.Common;
using Webim;

namespace Spacebuilder.Webim.Controllers
{
    /// <summary>
    /// Webim控制器
    /// </summary>
    public class WebimController : Controller
    {

        private WebimModel model = null;

        private WebimPlugin plugin = null;

        private WebimClient client = null;

        private WebimEndpoint endpoint = null;

        /// <summary>
        /// Webim控制器
        /// </summary>
        public WebimController()
        {
            this.model = new WebimModel();
            this.plugin = new WebimPlugin();
            this.endpoint = this.plugin.Endpoint();
        }

        private string CurrentUid()
        {
            return this.endpoint.Id;
        }


        private WebimClient CurrentClient(string ticket = "")
        {
            if (this.client == null)
            {
                this.client = new WebimClient(this.endpoint,
                    WebimConfig.Instance().Domain,
                    WebimConfig.Instance().APIkey,
                    WebimConfig.Instance().Host,
                    WebimConfig.Instance().Port);
                this.client.Ticket = ticket;
            }
            return this.client;

        }

        // GET: /Webim/Index
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Webim/Boot
        [HttpGet]
        public ActionResult Boot()
        {
            int iisVersion = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            bool aspx = iisVersion < 7;
            string setting = model.GetSetting(CurrentUid());
            string body = string.Format(@"var _IMC = {{
	            product: 'jinhusns',
	            version: '5.4.2',
	            path: '{0}',
	            uiPath: '{1}',
	            is_login: true,
                is_visitor: false,
	            user: '',
	            setting: {2},
	            menu: '',
                discussion: false,
				enable_room: true,
				enable_noti: true,
	            enable_chatlink: true,
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
            
            _IMC.script = window.webim ? '' : ('<link href=""' + _IMC.uiPath + 'webim'+ _IMC.min + '.css?' + _IMC.version + '"" media=""all"" type=""text/css"" rel=""stylesheet""/><link href=""' + _IMC.uiPath + 'themes/' + _IMC.theme + '/jquery.ui.theme.css?' + _IMC.version + '"" media=""all"" type=""text/css"" rel=""stylesheet""/><script src=""' + _IMC.uiPath + 'webim' + _IMC.min + '.js?' + _IMC.version + '"" type=""text/javascript""></script><script src=""' + _IMC.uiPath + 'i18n/webim-' + _IMC.local + '.js?' + _IMC.version + '"" type=""text/javascript""></script>');
            _IMC.script += '<script src=""' + _IMC.uiPath + 'webim.' + _IMC.product + '.js?' + _IMC.version + '"" type=""text/javascript""></script>';
            document.write( _IMC.script );

            ", WebUtility.ResolveUrl("~/Webim/"), WebUtility.ResolveUrl("~/Applications/Webim/static/"), setting, aspx.ToString().ToLower());

            return Content(body, "text/javascript");
        }

        // POST: /Webim/Online
        [HttpPost]
        public ActionResult Online()
        {
            //当前用户登录
            if (this.endpoint == null)
                return Json(
                    new { success = false, error = "尚未登录" },
                    JsonRequestBehavior.AllowGet
                );

            string uid = CurrentUid();
            if (Request["show"] != null)
            {
                this.endpoint.Show = Request["show"];
            }
            IEnumerable<WebimEndpoint> buddies = plugin.Buddies(uid);
            IEnumerable<WebimRoom> rooms = plugin.Rooms(uid);
            //Forward Online to IM Server
            WebimClient client = CurrentClient();
            var buddyIds = from b in buddies select b.Id;
            var roomIds = from g in rooms select g.Id;
            try
            {
                Dictionary<string, object> data = client.Online(buddyIds, roomIds);

                //Update Buddies 
                JsonObject presences = (JsonObject)data["presences"];
                foreach (WebimEndpoint buddy in buddies) {
                    if (presences.ContainsKey(buddy.Id))
                    {
                        buddy.Presence = "online";
                        buddy.Show = (string)presences[buddy.Id];
                    }
                    else {
                        buddy.Presence = "offline";
                        buddy.Show = "unavailable";
                    }
                 
                }
                if (!WebimConfig.Instance().ShowUnavailable)
                {
                    buddies = buddies.Where(buddy => buddy.Presence.Equals("online") && !buddy.Show.Equals("invisible"));
                }
                var buddyArray = (from b in buddies select b.Data()).ToArray();
                var roomArray = (from g in rooms select g.Data()).ToArray();
                data["success"] = true;
                data["buddies"] = buddyArray;
                data["rooms"] = roomArray;
                data["server_time"] = Timestamp();
                data["user"] = this.endpoint.Data();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(
                    new { success = false, error= e.ToString() },
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
            string uid = CurrentUid();
            WebimClient c = CurrentClient(Request["ticket"]);
            string type = Request["type"];
            string offline = Request["offline"];
            string to = Request["to"];
            string body = Request["body"];
            string style = Request["style"];
            if (style == null) { style = ""; }
            WebimMessage msg = new WebimMessage(type, to, c.Endpoint.Nick, body, style, Timestamp());
            msg.Offline = offline.Equals("true") ? true : false;
            c.Publish(msg);
            if (body != null && !body.StartsWith("webim-event:")) {
                model.InsertHistory(uid, msg);
            }
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
            if (status == null) { status = ""; }
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
            string data = Request["data"];
            model.SaveSetting(CurrentUid(), data);
            return Json("ok");
        }

        //GET: /Webim/History
        [HttpGet]
        public ActionResult History()
        {
            string uid = CurrentUid();
            string id = Request["id"];
            string type = Request["type"];
            IEnumerable<WebimHistory> histories = model.Histories(uid, id, type);
            var list = from h in histories select h.Data();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //POST: /Webim/ClearHistory
        public ActionResult ClearHistory()
        {
            string uid = CurrentUid();
            string id = Request["id"];
            model.ClearHistories(uid, id);
            return Json("ok");
        }

        //GET: /Webim/DownloadHistory
        [HttpGet]
        public ActionResult DownloadHistory()
        {
            string uid = CurrentUid();
            string id = Request["id"];
            string type = Request["type"];
            IEnumerable<WebimHistory> histories = model.Histories(uid, id, type);
            return View(histories);
        }

        //GET: /Webim/Members
        [HttpGet]
        public ActionResult Members()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string roomId = Request["id"];
            WebimRoom room = this.plugin.FindRoom(roomId);
            IEnumerable<WebimMember> members = null;
            if (room != null)
            {
                members = plugin.Members(roomId);
            }
            else
            {
                room = model.FindRoom(roomId);
                if (room != null)
                {
                    members = model.Members(roomId);
                }
            }
            if (room == null)
            {
                return null;
            }
            JsonObject presences = c.Members(roomId);
            foreach (WebimMember member in members) {
                if (presences.ContainsKey(member.Id))
                {
                    member.Presence = "online";
                    member.Show = (string)presences[member.Id];
                }
                else {
                    member.Presence = "offline";
                    member.Show = "unavailable";
                }
            }
            var data = from m in members select m.Data();
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //POST: /Webim/Join
        [HttpPost]
        public ActionResult Join()
        {
            WebimClient c = CurrentClient(Request["ticket"]);
            string id = Request["id"];
            WebimRoom room = plugin.FindRoom(id);
            if(room == null) {
                room = model.FindRoom(id);
            }
            if(room != null) { 
                c.Join(id);
                return Json(room.Data(), JsonRequestBehavior.AllowGet);
            }
            return null;
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
        //TODO: SECURITY BUGS!!!
        [HttpGet]
        public ActionResult Buddies()
        {
            string[] ids = Request["ids"].Split(new char[1] { ',' });                 
            IEnumerable<WebimEndpoint> buddies = plugin.BuddiesByIds(CurrentUid(), ids);
            var list = from buddy in buddies select buddy.Data();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //GET: /Webim/Notifications
        [HttpGet]
        public ActionResult Notifications()
        {
            var list = from n in plugin.Notifications(CurrentUid()) select n.Data();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //GET: /Webim/Menu
        [HttpGet]
        public ActionResult Menu()
        {
            var list = from m in plugin.Menu(CurrentUid()) select m.Data();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        private double Timestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }
    }
}
