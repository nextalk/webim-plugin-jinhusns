using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webim;
using Spacebuilder.Group;
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Webim
{
    public class WebimService
    {
        private IUserService userService = DIContainer.Resolve<IUserService>();

        private GroupService groupService = new GroupService();

        private FollowService followService = new FollowService();

        IHistoryRepository historyRepository = new HistoryRepository();

        ISettingRepository settingRepository = new SettingRepository();

        public WebimService()
        { }

        public WebimEndpoint Mapping(IUser u)
        {
            WebimEndpoint ep = new WebimEndpoint(
                u.UserId.ToString(),
                "uid:" + u.UserId,
                u.NickName);
            ep.PicUrl = SiteUrls.Instance().UserAvatarUrl(u, AvatarSizeType.Small);
            ep.Show = "available";
            ep.Url = SiteUrls.Instance().SpaceHome(u.UserId);
            //TODO:
            ep.Status = "";
            return ep;
        }

        public WebimGroup Mapping(GroupEntity e)
        {
            string gid = e.GroupId.ToString();
            WebimGroup g = new WebimGroup(gid, "gid:" + gid, e.GroupName);
            g.AllCount = e.MemberCount;
            g.PicUrl = SiteUrls.Instance().LogoUrl(e.Logo, TenantTypeIds.Instance().Group(), ImageSizeTypeKeys.Instance().Small());
            g.Url = SiteUrls.Instance().GroupHome(e.GroupId);
            return g;
        }

        //TODO: FIXME Later
        public Dictionary<string, string> mapping(HistoryEntity e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["type"] = e.Type;
            data["send"] = e.Send == 1 ? "true" : "false";
            data["to"] = e.To;
            data["from"] = e.From;
            data["nick"] = e.Nick;
            data["body"] = e.Body;
            data["style"] = e.Style;
            data["timestamp"] = e.Timestamp.ToString();
            return data;
        }
        public IEnumerable<WebimEndpoint> GetBuddies(long uid)
        {
            //TODO: PERFORMANCE ISSUES
            IEnumerable<long> ids = followService.GetTopFollowedUserIds(uid, 1000, FollowSpecifyGroupIds.Mutual);
            return (from id in ids
                    where followService.IsMutualFollowed(uid, id)
                    select Mapping(userService.GetUser(id)));
        }

        public IEnumerable<WebimGroup> GetGroups(long uid)
        {
            PagingDataSet<GroupEntity> groups = groupService.GetMyJoinedGroups(uid, 10, 0);
            return (from g in groups select Mapping(g));
        }

        //Groups
        public WebimGroup GetGroup(long gid)
        {
            return Mapping(groupService.Get(gid));
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

        public void InsertHistory(long uid, string offline, WebimMessage msg)
        {
            HistoryEntity entity = HistoryEntity.New();
            entity.From = uid.ToString();
            entity.Send = (offline == "true" ? 0 : 1);
            entity.Nick = msg.Nick;
            entity.Type = msg.Type;
            entity.To = msg.To;
            entity.Body = msg.Body;
            entity.Style = msg.Style;
            entity.Timestamp = msg.Timestamp;
            historyRepository.Insert(entity);
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
        public IEnumerable<HistoryEntity> GetHistories(long uid, string with, string type = "unicast")
        {
            return historyRepository.GetHistories(uid, with, type);
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

        public IEnumerable<WebimEndpoint> GetBuddiesByIds(IEnumerable<long> ids)
        {
            IEnumerable<IUser> users = userService.GetUsers(ids);
            return (from user in users select Mapping(user));
        }
    }
}

