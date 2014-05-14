using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webim;

namespace Spacebuilder.Webim
{

    public class WebimPlugin
    {

        private IUserService userService;

        private GroupService groupService;

        private FollowService followService;

        public WebimPlugin () 
        {

            userService = DIContainer.Resolve<IUserService>();

            groupService = new GroupService();

            followService = new FollowService();

        }

       /**
        * API: current user
        *
        * 返回当前的Webim端点(用户)
        */
        public WebimEndpoint Endpoint()
        {
            IUser u = UserContext.CurrentUser;
            if(u == null) { return null; }
            return Mapping(u);
        }

        /**
         * API: Buddies of current user.
         *
         * Buddy:
         *
         *  id:         uid
         *  uid:        uid
         *  nick:       nick
         *  pic_url:    url of photo
         *  presence:   online | offline
         *  show:       available | unavailable | away | busy | hidden
         *  url:        url of home page of buddy
         *  status:     buddy status information
         *  group:      group of buddy

         * @param string current uid
         *
         * @return Buddy list
         *
         */
        public IEnumerable<WebimEndpoint> Buddies(string uid)
        {
            //TODO: PERFORMANCE ISSUES
            IEnumerable<long> ids = followService.GetTopFollowedUserIds(long.Parse(uid), 1000, FollowSpecifyGroupIds.Mutual);
            return (from id in ids
                    where followService.IsMutualFollowed(long.Parse(uid), id)
                    select Mapping(userService.GetUser(id)));
        }

        /**
        * API: buddies by ids
        *
        * @param buddy id array
        *
        * @return Buddy list
        *
        */

        public IEnumerable<WebimEndpoint> BuddiesByIds(string uid, IEnumerable<string> ids)
        {
            IEnumerable<long> uids = (from id in ids select long.Parse(id));
            IEnumerable<IUser> users = userService.GetUsers(uids);
            return (from user in users select Mapping(user));
        }

        /**
        * 根据roomId读取群组
        * 
        * @param roomId
        * @return WebimRoom
        */
        public WebimRoom findRoom(string roomId)
        {
            return Mapping(groupService.Get(long.Parse(roomId)));
        }

        /**
         * API：rooms of current user
         *
         * @param uid
         *
         * @return rooms
         *
         * Room:
         *
         *  id:         Room ID,
         *  nick:       Room Nick
         *  url:        Home page of room
         *  pic_url:    Pic of Room
         *  status:     Room status
         *  count:      count of online members
         *  all_count:  count of all members
         *  blocked:    true | false
         */
        public IEnumerable<WebimRoom> Rooms(string uid)
        {
            PagingDataSet<GroupEntity> groups = groupService.GetMyJoinedGroups(long.Parse(uid), 100, 0);
            return (from g in groups select Mapping(g));
        }

        /**
         * API: rooms by ids
         *
         * @param room id array
         *
         * @return rooms
         *
         * Room
         *
         */
        public IEnumerable<WebimRoom> RoomsByIds(string uid, IEnumerable<string> ids)
        {
            IEnumerable<long> gids = (from id in ids select long.Parse(id));
            return Mapping(groupService.GetGroups(gids);
        }

        /**
         * API: members of room
         *
         * @param roomId
         * @return member list
         */
        public IEnumerable<WebimMember> Members(string roomId)
        {
            //TODO: 读取群组成员
            return new List<WebimMember> { 
                new WebimMember("1", "user1"), 
                new WebimMember("2", "user2") 
            };
        }

        /**
         * API: notifications of current user
         *
         * @return notification list
         *
         * Notification:
         *
         *  text: text
         *  link: link
         */
        public IEnumerable<WebimNotification> Notifications(string uid)
        {
            //TODO: unimplemented
            IUser user = UserContext.CurrentUser;
            return new List<WebimNotification>();
        }


        /**
         * API: menu
         *
         * @return menu list
         *
         * Menu:
         *
         * icon
         * text
         * link
         */
        public IEnumerable<WebimMenu> Menu(string uid)
        {
            //TODO: unimplemented
            IUser user = UserContext.CurrentUser;
            return new List<WebimMenu>();
        }

        private WebimEndpoint Mapping(IUser u)
        {
            WebimEndpoint ep = new WebimEndpoint(
                u.UserId.ToString(),
                u.NickName);
            ep.PicUrl = SiteUrls.Instance().UserAvatarUrl(u, AvatarSizeType.Small);
            ep.Show = "available";
            ep.Url = SiteUrls.Instance().SpaceHome(u.UserId);
            ep.Status = "";
            return ep;
        }

        private WebimRoom Mapping(GroupEntity e)
        {
            string gid = e.GroupId.ToString();
            WebimRoom room = new WebimRoom(gid, e.GroupName);
            room.AllCount = e.MemberCount;
            room.PicUrl = SiteUrls.Instance().LogoUrl(e.Logo, TenantTypeIds.Instance().Group(), ImageSizeTypeKeys.Instance().Small());
            room.Url = SiteUrls.Instance().GroupHome(e.GroupId);
            return room;
        }

    }

}
