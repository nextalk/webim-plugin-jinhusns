//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Configuration;
using System.Web.Mvc;
using Tunynet.Common;

namespace Spacebuilder.Webim
{
    /// <summary>
    /// 路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Webim"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            //商城在频道下的其它页面
            context.MapRoute(
                "Channel_Webim_Common", // Route name
                "Webim/{action}" + extensionForOldIIS, // URL with parame ters
                new { controller = "Webim", action = "Run" } // Parameter defaults
            );

        }
    }
}