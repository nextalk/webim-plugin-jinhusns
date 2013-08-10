//custom
(function(webim){
	var path = _IMC.path;
	webim.extend(webim.setting.defaults.data, _IMC.setting );
	var webim = window.webim;
	webim.defaults.urls = {
		online:path + "Online",
		offline:path + "Offline",
		message:path + "Message",
		presence:path + "Presence",
		refresh:path + "Refresh",
		status:path + "Status"
	};
	webim.setting.defaults.url = path + "Setting";
	webim.history.defaults.urls = {
		load: path + "History",
		clear: path + "ClearHistory",
		download: path + "DownloadHistory"
	};
	webim.room.defaults.urls = {
		member: path + "Members",
		join: path + "Join",
		leave: path + "Leave"
	};
	webim.buddy.defaults.url = path + "Buddies";
	webim.notification.defaults.url = path + "Notifications";

	webim.ui.emot.init({"dir": _IMC.uiPath + "static/images/emot/default"});
	var soundUrls = {
	    lib: _IMC.uiPath + "static/assets/sound.swf",
	    msg: _IMC.uiPath + "static/assets/sound/msg.mp3"
	};
	var ui = new webim.ui(document.body, {
		imOptions: {
			jsonp: _IMC.jsonp
		},
		soundUrls: soundUrls
	}), im = ui.im;

	if( _IMC.user ) im.user( _IMC.user );
	//if( _IMC.menu ) ui.addApp("menu", { "data": _IMC.menu } );
	if( _IMC.enable_shortcut ) ui.layout.addShortcut( _IMC.menu );

	ui.addApp("buddy", {
		is_login: _IMC['is_login'],
		loginOptions: _IMC['login_options']
	} );
	ui.addApp("room");
	ui.addApp("notification");
	ui.addApp("setting", {"data": webim.setting.defaults.data});
	if( !_IMC.disable_chatlink )ui.addApp("chatlink", {
		space_href: [/mod=space&uid=(\d+)/i, /space\-uid\-(\d+)\.html$/i],
		space_class: /xl\sxl2\scl/,
		space_id: null,
		link_wrap: document.getElementById("ct")
	});
	ui.render();
	_IMC['is_login'] && im.autoOnline() && im.online();
})(webim);
