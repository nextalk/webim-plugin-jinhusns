
Webim for JinhuSNS
=========================

NexTalk为jinhusns.com定制开发的WebIM应用，支持JinhuSNS社区网站访客、用户与管理员间的即时聊天。


安装(Install)
=============

1. 安装WebIM应用: 将Web下的文件覆盖到网站根目录下。

2. 初始化数据库: 在SqlServer中，执行SQL脚本文件Web\Applications\Webim\Setup\Install\SqlServer\01_Install_Webim_Schema.sql

3. 嵌入WebIM页面， 在Web/Themes下找到Footer文件，根据不同的IIS版本选择需要追加的代码：
     IIS7： @Html.LinkScript("~/Webim/Run") 
     IIS6： @Html.LinkScript("~/Webim/Run.aspx")



配置(Configure)
===============

配置文件Web\Applications\Webim\Application.Config

	#是否启动Webim应用
	isopen="true"

	version="1.0"

	#网站域名，在NexTalk.IM注册
	domain="spacebuilder.cn"

	#消息服务器通信APIKEY，在NexTalk.IM注册申请
	apikey="2e9d639da76de5a9"

	#消息服务器地址，nextalk.im提供免费测试服务器
	host="nextalk.im"

	#消息服务器地址
	port="8000"

版权(License)
=============

Webim应用版权为NexTalk.IM、JinhuSNS.com共有.


联系(Contact)
=============

http://nextalk.im

