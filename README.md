webim-plugin-jinhusns
=========================

webim plugin for jinhusns

Install
=======

1.将Web下的文件覆盖到网站根目录下；
2.在Web/Themes下找到Footer文件，根据不同的IIS版本选择需要追加的代码：
     IIS7： @Html.LinkScript("~/Webim/Run") 
     IIS6： @Html.LinkScript("~/Webim/Run.aspx")
3.在SqlServer中，执行SQL脚本文件：Web\Applications\Webim\Setup\Install\SqlServer\01_Install_Webim_Schema.sql

注意：请联系NextIM，索取apiKey，并配置Web\Applications\Webim\Application.Config。
