using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webim;

namespace Spacebuilder.Webim.Services
{
    public class WebimService
    {
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

    }
}