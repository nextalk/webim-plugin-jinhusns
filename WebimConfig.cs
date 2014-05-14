using Autofac;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Tunynet.Common;

namespace Spacebuilder.Webim
{
    //TODO: Should be in DB.
    public class WebimConfig : ApplicationConfig
    {

        private static int applicationId = 9001;
        //public static string VERSION = "1.0";
        //public static string DOMAIN = "spacebuilder.cn";//"localhost";
        //public static string APIKEY = "2e9d639da76de5a9";//"public";
        //public static string HOST = "webim20.cn";//"192.168.0.145";
        //public static int PORT = 8888;
        /// <summary>
        /// 获取WebimConfig实例
        /// </summary>
        public static WebimConfig Instance()
        {
            return ApplicationConfig.GetConfig(applicationId) as WebimConfig;
        }


        public WebimConfig(XElement xElement)
            : base(xElement)
        {
            XAttribute att = xElement.Attribute("version");
            if (att != null)
                this.version = att.Value;
            att = xElement.Attribute("isopen");
            if (att != null)
                bool.TryParse(att.Value, out this.isopen);
            att = xElement.Attribute("domain");
            if (att != null)
                this.domain = att.Value;
            att = xElement.Attribute("apikey");
            if (att != null)
                this.apikey = att.Value;
            att = xElement.Attribute("host");
            if (att != null)
                this.host = att.Value;
            att = xElement.Attribute("port");
            if (att != null)
                int.TryParse(att.Value, out this.port);
            att = xElement.Attribute("show_unavailable");
            if (att != null)
                this.show_unavailable = bool.Parse(att.Value);
        }

        private bool isopen;
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return isopen; }
        }


        private string version;
        /// <summary>
        /// Webim版本
        /// </summary>
        public string Version
        {
            get { return version; }
        }

        private string domain;
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain
        {
            get { return domain; }
        }

        private string apikey;
        /// <summary>
        /// APIKey
        /// </summary>
        public string APIkey
        {
            get { return apikey; }
        }

        private string host;
        /// <summary>
        /// HOST
        /// </summary>
        public string Host
        {
            get { return host; }
        }

        private int port;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return port; }
        }

        private bool show_unavailable;
        /// <summary>
        /// 是否显示离线
        /// </summary>
        public bool ShowUnavailable
        {
            get { return show_unavailable; }
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return applicationId; }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return "Webim"; }
        }

        /// <summary>
        /// 获取Webim实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return null; }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {

        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

    }

}
