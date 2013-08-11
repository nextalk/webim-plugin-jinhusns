﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Json;
using Webim;
using Spacebuilder.Webim.Models;
using Spacebuilder.Webim.Services;

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
			Dictionary<string, WebimEndpoint> buddies = webimService.GetBuddies();
			Dictionary<string, WebimGroup> groups = webimService.GetGroups();
            //Forward Online to IM Server
            JsonObject json = client.Online(new List<string>(buddies.Keys), 
											   new List<string>(groups.Keys));
            Debug.WriteLine(json.ToString());

            bool success = (bool)json["success"];
            if (!success)
			{
                return Json(
					new {success = false, error_msg = "IM Server is not found" },
                    JsonRequestBehavior.AllowGet
				);
			}

            Dictionary<string, string> conn = new Dictionary<string, string>();
            
            conn.Add("ticket", (string)json["ticket"]);
            conn.Add("domain", client.Domain);
            conn.Add("jsonpd", (string)json["jsonpd"]);
            conn.Add("server", (string)json["jsonpd"]);
            conn.Add("websocket", (string)json["websocket"]);

            List<Dictionary<string, string>> onlineBuddies = new List<Dictionary<string, string>>();
            foreach (JsonValue v in (JsonArray)json["buddies"])
			{
				JsonObject o = (JsonObject)v;
                string uid = (string)o["name"];
				onlineBuddies.Add(buddies[uid].Data());
			}

            List<Dictionary<string, string>> onlineGroups = new List<Dictionary<string, string>>();
            foreach (JsonValue v in (JsonArray)json["groups"])
			{
				JsonObject o = (JsonObject)v;
                string gid = (string)o["name"];
				WebimGroup group = groups[gid];
                group.Count = (int)o["total"];
                onlineGroups.Add(group.Data());
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
                 service_time = 0,
                 user = client.Endpoint.Data()
                }, JsonRequestBehavior.AllowGet);
          
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
			webimService.ClearHistory(id);
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
			Dictionary<string, WebimEndpoint> buddies = webimService.GetBuddies();
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
