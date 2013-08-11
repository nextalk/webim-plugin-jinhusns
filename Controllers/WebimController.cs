using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Spacebuilder.Webim.Controllers
{
    public class WebimController : Controller
    {

		//TODO: There should be userService, groupService and FollowService

		private WebimService webimService = new WebimService();

        // GET: /Webim/
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Webim/Online
		[HttpPost]
        public ActionResult Online()
        {
		    //当前用户登录
            WebimClient client = webimService.CurrentClient();
			Dictionary<string, WebimEndpoint> buddies = webimService.GetBuddeis();
			Dictionary<string, WebimGroup> groups = webimService.GetGroups();
            //Forward Online to IM Server
            JsonObject respObj = client.Online(new List<string>(buddies.Keys), 
											   new List<string>(groups.Keys));
            Debug.WriteLine(respObj.ToString());

            bool success = (bool)respobj["success"];
            if (!isSuccess)
			{
                return Json(
					new {success = false, error_msg = "IM Server is not found" },
                    JsonRequestBehavior.AllowGet
				);
			}
			JsonObject conn = (JsonObject)obj["conn"];
			client.Ticket = (string)conn["ticket"];

			List<WebimEndpoint> onlineBuddies = new List<WebimEndpoint>();
			foreach (JsonValue v in conn["buddies"])
			{
				JsonObject o = (JsonObject)v;
				onlineBuddies.Add(buddies[(string)o["name"]]);
			}

			List<WebimGroup> onlineGroups = new List<WebimGroup>();
			foreach(JsonValue v in conn["groups"])
			{
				JsonObject o = (JsonObject)v;
				WebimGroup group = groups[(string)o["name"]];
			}

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
			return Json(new
                {success = true,
                 server_time = 1370751451399,//FIXME:
                 connection = conn,
                 buddies = onlineBuddies.ToArray(),
                 groups = onlineGroups.ToArray(),
                 rooms = onlineGroups.ToArray(),
                 user = respObj["user"] 
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: /Webim/Offline
		[HttpPost]
		public ActionResult Offline() 
		{
			WebimClient c = webimService.CurrentClient(Request["ticket"]);
			c.Offline();
			return Json("ok");
		}
			
        //POST: /Webim/Message
		public ActionResult Message()
		{
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            string type = Request["type"];
            string offline = Request["offline"];
            string to = Request["to"];
            string body = Request["body"];
            string style = Request["style"];
            //FIXME: unix timestamp
            int timestamp = 1928282;
            //TODO: Insert into database
            WebimMessage msg = new WebimMessage(to, body, style, timestamp);
            c.Publish(msg);
			webimService.insertHistory(msg);
            return Json("ok");
		}

        //POST: /Webim/Presence
		[HttpPost]
        public ActionResult Presence()
        {
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            string show = Request["show"];
            string status = Request["status"];
            c.Publish(new WebimPresence(show, status));
            return Json("ok");
        }

        //POST: /Webim/Status
		[HttpPost]
        public ActionResult Status()
        {
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
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
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            c.Offline();
            return Json("ok");
        }

        //POST: /Webim/Setting
		[HttpPost]
        public ActionResult Setting()
        {
            string data = Request["data"];
			webimService.updateSetting(data);
            return Json("ok");
        }

        //GET: /Webim/History
		[HttpGet]
        public ActionResult History()
        {
            string id = Request["id"];
            string type = Request["type"];
			IEnumerable<WebimHistory> histories = webimService.GetHistory(id, type);
            return Json(histories, JsonRequestBehavior.AllowGet);
        }

        //POST: /Webim/ClearHistory
        public ActionResult ClearHistory()
        {
            string id = Request["id"];
			webimService.clearHistory(id);
            return Json("ok");

        }

        //GET: /Webim/DownloadHistory
		[HttpGet]
        public ActionResult DownloadHistory()
        {
			string id = Request["id"];
			string type = Request["type"];
			IEnumerable<WebimHistory> histories = webimService.GetHistory(id, type);
            return View(histories);
        }


        //GET: /Webim/Members
		[HttpGet]
        public ActionResult Members()
        {
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            string gid = Request["id"];
            JsonObject obj = c.Members(gid);
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        //POST: /Webim/Join
		[HttpPost]
        public ActionResult Join()
        {
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            string gid = Request["id"];
            JsonObject o = c.Join(gid);
            return Json(o);
        }

        //POST: /Webim/Leave
		[HttpPost]
        public ActionResult Leave()
        {
            WebimClient c = webimService.CurrentClient(Request["ticket"]);
            c.Leave(Request["id"]);
            return Json("ok");
        }

        //GET: /Webim/Buddies
		[HttpGet]
        public ActionResult Buddies()
        {
			Dictionary<string, WebimEndpoint> buddies = webimService.GetBuddeis();
            return Json(buddies.Values.ToArray(), JsonRequestBehavior.AllowGet);
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

    }
}
