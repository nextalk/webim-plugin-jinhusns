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
        //room actions
		join: path + "Join" + aspx,
		leave: path + "Leave" + aspx,
		block: path + "Block" + aspx,
		unblock: path + "Unblock" + aspx,
		members: path + "Members" + aspx,
		buddies: path + "Buddies" + aspx,
        //notifications
		notifications: path + "Notifications" + aspx,
        //upload files
		upload: path + "Upload" + aspx
	});
	webim.ui.emot.init({ "dir": "static/images/emot/default" });
	var soundUrls = {
		lib: "static/assets/sound.swf",
		msg: "static/assets/sound/msg.mp3"
	};
	var ui = new webim.ui(document.body, {
		imOptions: {
			jsonp: _IMC.jsonp
		},
		soundUrls: soundUrls,
		//layout: "layout.popup",
        layoutOptions: {
            unscalable: _IMC.is_visitor
        },
        soundUrls: soundUrls,
		buddyChatOptions: {
            downloadHistory: !_IMC.is_visitor,
			//simple: _IMC.is_visitor,
			upload: _IMC.upload && !_IMC.is_visitor
		},
		roomChatOptions: {
            downloadHistory: !_IMC.is_visitor,
			upload: _IMC.upload
		}
    }), im = ui.im;
    //全局化
    window.webimUI = ui;

	if( _IMC.user ) im.setUser( _IMC.user );
	if( _IMC.menu ) ui.addApp("menu", { "data": _IMC.menu } );
	if( _IMC.enable_shortcut ) ui.layout.addShortcut( _IMC.menu );

    ui.addApp("buddy", {
		showUnavailable: _IMC.show_unavailable,
		is_login: _IMC['is_login'],
		disable_login: true,
		collapse: false,
		//disable_user: _IMC.is_visitor,
        //simple: _IMC.is_visitor,
		loginOptions: _IMC['login_options']
    });
    if( _IMC.enable_room )ui.addApp("room", { discussion: (_IMC.discussion && !_IMC.is_visitor) });
	if( _IMC.enable_noti )ui.addApp("notification");
    if (_IMC.enable_chatlink) ui.addApp("chatlink", {
        space_href: [/mod=space&uid=(\d+)/i, /space\-uid\-(\d+)\.html$/i],
        space_class: /xl\sxl2\scl/,
        space_id: null,
        link_wrap: document.getElementById("ct")
    });
    ui.addApp("setting", {"data": webim.setting.defaults.data, "copyright": true});
    ui.render();
    _IMC['is_login'] && im.autoOnline() && im.online();
})(webim);

