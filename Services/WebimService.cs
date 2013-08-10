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

        public WebimClient CurrentClient(string uid) 
        {
            WebimEndpoint user = CurrentUser(uid);
            return new WebimClient(user,
                WebimConfig.DOMAIN,
                WebimConfig.APIKEY,
                WebimConfig.HOST,
                WebimConfig.PORT);
        }

        public WebimEndpoint CurrentUser(string uid)
        {
            //TODO: SHOULD read from UserService
            WebimEndpoint user =  new WebimEndpoint(uid, "uid:" + uid, "user");
            user.Show = "available";
            user.Status = "Online";
            return user;
        }

		public Dictionary<string, WebimEndpoint> GetBuddies(string uid) 
		{
			return new Dictionary<string, WebimEndpoint>();
		}

		public Dictionary<string, WebimGroup> GetGroups(string uid) 
		{
			return new Dictionary<string, WebimGroup>();
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

		public void updateSetting(string uid, string data)
		{

		}

		//History
		public IEnumerable<WebimHistory> GetHistory(string uid, string with, string type = "unicast")
		{

		}
	
		//TODO: DELETE FROM DB
		public void ClearHistory(string uid, string with) 
		{
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

