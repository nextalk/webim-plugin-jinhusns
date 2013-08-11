using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webim;
using Spacebuilder.Webim.Models;
using Spacebuilder.Group

namespace Spacebuilder.Webim
{
    public class WebimService
    {
        private IUserService userService = DIContainer.Resolve<IUserService>();

        private GroupService groupService = new GroupService();

        private FollowService followService = new FollowService();

		IHistoryRepository historyRepository = new HistoryRepository();

		ISettingRepository settingRepository = new SettingRepository();

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
            //IUser u = UserContext.CurrentUser;
			string uid = "1"; //u.UserId;
			string nick = "nick"; //u.NickName;
            WebimEndpoint ep =  new WebimEndpoint(uid, "uid:" + uid, nick);
            ep.Show = "available";
            ep.Status = "Online";
            return ep;
        }

		public IEnumerable<IUser> GetBuddies(long uid) 
		{
			//TODO: PERFORMANCE ISSUES
			PagingDataSet<long> ids = followService.GetFollowedIds(uid, null, null, 10000);
			List<IUser> buddies = new List<IUser>();
			for(long id in ids) 
			{
				if(followService.IsMutualFollowed(uid, id))
				{
					buddies.Add(userService.GetUser(id));
				}

			}
			return buddies;
		}

		public IEnumerable<GroupEntity> GetGroups(long uid) 
		{
			//TODO: fix later
		 	PagingDataSet<GroupEntity> groupService.GetMyJoinedGroups(long userId, 100, 0);
            return new List<GroupEntity>();
		}

		//Groups
		public GroupEntity GetGroup(string gid)
		{
            return new WebimGroup("group1", "group:1", "group1");
		}

		//Offline
		public IEnumerable<HistoryEntity> GetOfflineMessages(int uid)	
		{
			return historyRepository.GetOfflineMessages(uid);
		}

		public void OfflineMessageToHistory(int uid)
		{
			historyRepository.OfflineMessageToHistory(uid);
		}

        public void insertHistory(string from, string offline, WebimMessage msg)
		{
			HistoryEntity entity = HistoryEntity.new();
			entity.From = from;
			entity.Send = offline;
			entity.Nick = msg.Nick;
			entity.Type = msg.Type;
			entity.To = msg.To;
			entity.Body = msg.Body;
			entity.Style = msg.Style;
			entity.Timestamp = msg.Timestamp;
			historyRepository.insert(entity);
		}

		//Setting
		public string GetSetting(string uid) 
		{
			IUser user = UserContext.CurrentUser;
			return settingRepository.Get(user.UserId);
		}

		public void updateSetting(string data)
		{
			IUser user = UserContext.CurrentUser;
			settingRepository.Set(user.UserId, data);
		}

		//History
		public IEnumerable<WebimHistory> GetHistories(string with, string type = "unicast")
		{

			IUser user = UserContext.CurrentUser;
			return historyRepository.GetHistories(user.UserId, with, type);
		}
	
		public void ClearHistories(string with) 
		{
			IUser user = UserContext.CurrentUser;
			historyRepository.ClearHistories(user.UserId, with);
		}

		//Notifications
		public IEnumerable<WebimNotification> GetNotifications()
		{
			//TODO: unimplemented
			IUser user = UserContext.CurrentUser;
            return new List<WebimNotification>();
		}

		//Menu
		public IEnumerable<WebimMenu> GetMenuList()
		{
			//TODO: unimplemented
			IUser user = UserContext.CurrentUser;
            return new List<WebimMenu>();
		}

    }
}

