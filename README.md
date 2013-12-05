
简介
====

NexTalk为jinhusns.com定制开发的Webim应用，支持JinhuSNS社区网站访客、用户与管理员间的即时聊天。

依赖(Depends)
=============

	packages/
		System.Json.4.0.20118.13260/
		System.Net.Http.2.0.20118.13260/
		System.Net.Http.Formatting.4.0.20118.13260/

下载(Download)
==============

下载Webim-for-JinhuSNS4-20131205.zip安装包并解压。

下载地址:

	http://nextalk.im/static/plugins/jinhusns/Webim-for-JinhuSNS4-20131205.zip

	https://github.com/webim/webim-plugin-jinhusns/blob/master/Webim-for-JinhuSNS4-20131205.zip

安装(Install)
=============

1. 请务必做好网站程序的备份，特别是_Footer.cshtml文件；

2. 安装Webim应用，将Web目录下的文件覆盖到网站根目录下。

备注：
	（1）如果您修改过Web.config中以下配置：<add key="IISVersion" value="7" />， 则需要在Web/Themes下找到所有的_Footer.cshtml，然后找到@Html.LinkScript("~/Webim/Run.aspx")代码行，改为 @Html.LinkScript("~/Webim/Run")，这是因为IIS7版本允许我们忽略后缀名aspx；

    （2）如果您修改过_Footer.cshtml文件，请自己追加@Html.LinkScript("~/Webim/Run.aspx")代码行。

3. 初始化数据库: 在SqlServer中，执行SQL脚本文件Web\Applications\Webim\Setup\Install\SqlServer\01_Install_Webim_Schema.sql

配置(Configure)
===============

配置文件Web\Applications\Webim\Application.Config

	/*是否启动Webim应用*/
	isopen="true"

	version="1.0"

	/*网站域名，在NexTalk.IM注册*/
	domain="spacebuilder.cn"

	/*消息服务器通信APIKEY，在NexTalk.IM注册申请*/
	apikey="2e9d639da76de5a9"

	/*消息服务器地址，nextalk.im提供免费测试服务器*/
	host="nextalk.im"

	/*消息服务器地址*/
	port="8000"

版权(License)
=============

Webim应用版权归NexTalk.IM、JinhuSNS.com共有.


联系(Contact)
=============

http://nextalk.im

http://jinhusns.com
