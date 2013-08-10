var _IMC = {
	production_name: 'dotnet',
	version: '1.0',
	path: 'Webim/',
	uiPath: 'WebimUI/',
	is_login: true,
	user: '',
	setting: '',
	menu: '',
	disable_chatlink: '',
	enable_shortcut: '',
	disable_menu: '',
	theme: 'base',
	local: 'zh-CN',
	min: "" //window.location.href.indexOf("webim_debug") != -1 ? "" : ".min"
};

_IMC.script = window.webim ? '' : ('<link href="' + _IMC.uiPath + 'static/webim.'+ _IMC.production_name + _IMC.min + '.css?' + _IMC.version + '" media="all" type="text/css" rel="stylesheet"/><link href="' + _IMC.uiPath + 'static/themes/' + _IMC.theme + '/jquery.ui.theme.css?' + _IMC.version + '" media="all" type="text/css" rel="stylesheet"/><script src="' + _IMC.uiPath + 'static/webim.' + _IMC.production_name + _IMC.min + '.js?' + _IMC.version + '" type="text/javascript"></script><script src="' + _IMC.uiPath + 'static/i18n/webim-' + _IMC.local + '.js?' + _IMC.version + '" type="text/javascript"></script>');
_IMC.script += '<script src="' + _IMC.uiPath + 'webim.js?' + _IMC.version + '" type="text/javascript"></script>';
document.write( _IMC.script );
