//custom
(function (webim) {
    var path = _IMC.path;
    var aspx = _IMC.aspx ? ".aspx" : "";
    webim.extend(webim.setting.defaults.data, _IMC.setting);
    var webim = window.webim;
    webim.defaults.urls = {
        online: path + "Online" + aspx,
        offline: path + "Offline" + aspx,
        message: path + "Message" + aspx,
        presence: path + "Presence" + aspx,
        refresh: path + "Refresh" + aspx,
        status: path + "Status" + aspx
    };
    webim.setting.defaults.url = path + "Setting" + aspx;
    webim.history.defaults.urls = {
        load: path + "History" + aspx,
        clear: path + "ClearHistory" + aspx,
        download: path + "DownloadHistory" + aspx
    };
    webim.room.defaults.urls = {
        member: path + "Members" + aspx,
        join: path + "Join" + aspx,
        leave: path + "Leave" + aspx
    };
    webim.buddy.defaults.url = path + "Buddies" + aspx;
    webim.notification.defaults.url = path + "Notifications" + aspx;

    webim.ui.emot.init({ "dir": _IMC.uiPath + "static/images/emot/default" });
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

    if (_IMC.user) im.user(_IMC.user);
    //if( _IMC.menu ) ui.addApp("menu", { "data": _IMC.menu } );
    if (_IMC.enable_shortcut) ui.layout.addShortcut(_IMC.menu);

    ui.addApp("buddy", {
        is_login: _IMC['is_login'],
        loginOptions: _IMC['login_options']
    });
    ui.addApp("room");
    ui.addApp("notification");
    ui.addApp("setting", { "data": webim.setting.defaults.data });
    if (!_IMC.disable_chatlink) ui.addApp("chatlink", {
        space_href: [/mod=space&uid=(\d+)/i, /space\-uid\-(\d+)\.html$/i],
        space_class: /xl\sxl2\scl/,
        space_id: null,
        link_wrap: document.getElementById("ct")
    });
    ui.render();
    _IMC['is_login'] && im.autoOnline() && im.online();
})(webim);
