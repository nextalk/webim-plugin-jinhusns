//custom
(function (webim) {
    var path = _IMC.path;
    var aspx = _IMC.aspx ? ".aspx" : "";
    webim.extend(webim.setting.defaults.data, _IMC.setting);
	webim.route({
        online: path + "Online" + aspx,
        offline: path + "Offline" + aspx,
        message: path + "Message" + aspx,
        presence: path + "Presence" + aspx,
        deactivate: path + "Refresh" + aspx,
        status: path + "Status" + aspx,
		setting: path + "Setting" + aspx,
		history: path + "History" + aspx,
        clear: path + "ClearHistory" + aspx,
        download: path + "DownloadHistory" + aspx,
        members: path + "Members" + aspx,
        join: path + "Join" + aspx,
        leave: path + "Leave" + aspx,
		buddies: path + "Buddies" + aspx,
		notifications: path + "Notifications" + aspx
	});

    webim.ui.emot.init({ "dir": _IMC.uiPath + "static/images/emot/default" });
    var soundUrls = {
        lib: _IMC.uiPath + "static/assets/sound.swf",
        msg: _IMC.uiPath + "static/assets/sound/msg.mp3"
    };
    var ui = new webim.ui(document.body, {
        imOptions: {
            jsonp: _IMC.jsonp
        },
        soundUrls: soundUrls,
		buddyChatOptions: {
			upload: _IMC.upload
		},
		roomChatOptions: {
			upload: _IMC.upload
		}
    }), im = ui.im;

	if( _IMC.user ) im.setUser( _IMC.user );
    //if( _IMC.menu ) ui.addApp("menu", { "data": _IMC.menu } );
    if (_IMC.enable_shortcut) ui.layout.addShortcut(_IMC.menu);

    ui.addApp("buddy", {
		showUnavailable: _IMC.show_unavailable,
        is_login: _IMC['is_login'],
        collapse: false,
		disable_login: true,
        loginOptions: _IMC['login_options']
    });
	if( _IMC.enable_room )ui.addApp("room", { discussion: false});
	if( _IMC.enable_noti )ui.addApp("notification");
    ui.addApp("setting", { "data": webim.setting.defaults.data });
    if (_IMC.enable_chatlink) ui.addApp("chatlink", {
        space_href: [/mod=space&uid=(\d+)/i, /space\-uid\-(\d+)\.html$/i],
        space_class: /xl\sxl2\scl/,
        space_id: null,
        link_wrap: document.getElementById("ct")
    });
    ui.render();
    _IMC['is_login'] && im.autoOnline() && im.online();
})(webim);

