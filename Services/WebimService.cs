using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webim;

namespace Spacebuilder.Webim.Services
{
    public class WebimService
    {
		/*
        private IUserService userService = DIContainer.Resolve<IUserService>();

        private GroupService groupService = new GroupService();

        private FollowService followService = new FollowService();
		*/

        public WebimClient CurrentClient(string ticket="") 
        {
            WebimClient c = new WebimClient(
				ThisEndpoint(),
				WebimConfig.DOMAIN,
				WebimConfig.APIKEY,
				WebimConfig.HOST,
				WebimConfig.PORT);
			c.Ticket = ticket;
			return c;
        }

        public WebimEndpoint ThisEndpoint()
        {
            //TODO: SHOULD read from UserService
            IUser u = UserContext.CurrentUser;
			string uid = "1"; //u.UserId;
			string nick = "nick"; //u.NickName;
            WebimEndpoint ep =  new WebimEndpoint(uid, "uid:" + uid, nick);
            ep.Show = "available";
            ep.Status = "Online";
            return ep;
        }

		public Dictionary<string, WebimEndpoint> GetBuddies() 
		{
			//stub
			Dictionary<string, WebimEndpoint> data = new Dictionary<string, WebimEndpoint>();
			WebimEndpoint ep = ThisEndpoint();
			data[ep.Id] = ep;
			return data;
		}

		public Dictionary<string, WebimGroup> GetGroups() 
		{
			
			//stub
			Dictionary<string, WebimGroup> data = new Dictionary<string, WebimGroup>();
			data["group1"] = new WebimGroup("group1", "group1");
			return data;
		}

		//Groups
		public WebimGroup GetGroup(string uid, string gid)
		{

		}

		public IEnumerable<WebimGroup> GetGroups(string uid) 
		{

		}

		//Offline
		public IEnumerable<WebimMessage> GetOfflineMessages(string uid)	
		{

		}

		public void OfflineMessageToHistory(string uid)
		{
			
		}

		//Setting
		public string GetSetting(string uid) 
		{

		}

		
		public void insertHistory(WebimMessage msg)
		{
			/*
			$row = array(
				"from" => $uid,
				"nick" => $nick,
				"send" => $send,
				"type" => $type,
				"to" => $to,
				"body" => $body,
				"style" => $style,
				"timestamp" => $timestamp,
				"created_at" => date( 'Y-m-d H:i:s' ),
			);
			$this->_loadDao(self::HISTORY_DAO)->add($row);
			*/
		}

		//Setting
		public string GetSetting(string uid) 
		{

		}

		public void updateSetting(string data)
		{
			//IUser user = UserContext.CurrentUser;
			//store in db	
		}

		//History
		public IEnumerable<WebimHistory> GetHistory(string with, string type = "unicast")
		{
			//IUser user = UserContext.CurrentUser;
			string uid = "1"; 
		}
	
		//TODO: DELETE FROM DB
		public void ClearHistory(string with) 
		{
			//IUser user = UserContext.CurrentUser;
			string uid = "1";
			return;

		}

		//Notifications
		public IEnumerable<WebimNotification> GetNotifications(string uid)
		{

		}

		//Menu
		public IEnumerable<WebimMenu> GetMenuList(string uid)
		{

		}

    }
}

