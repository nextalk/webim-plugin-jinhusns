DROP TABLE IF EXISTS spb_Webim_Settings;
CREATE TABLE spb_Webim_Settings(
	`Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
	`Uid` varchar(40) NOT NULL,
	`Data` text,
	`Created` DATETIME DEFAULT NULL,
	`Updated` DATETIME DEFAULT NULL,
	PRIMARY KEY (`Id`) 
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Histories;
CREATE TABLE spb_Webim_Histories (
	`Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
	`Send` tinyint(1) DEFAULT NULL,
	`Type` varchar(20) DEFAULT NULL,
	`ToUser` varchar(50) NOT NULL,
	`FromUser` varchar(50) NOT NULL,
	`Nick` varchar(20) DEFAULT NULL COMMENT 'from nick',
	`Body` text,
	`Style` varchar(150) DEFAULT NULL,
	`Timestamp` bigint DEFAULT NULL,
	`ToDel` tinyint(1) NOT NULL DEFAULT '0',
	`FromDel` tinyint(1) NOT NULL DEFAULT '0',
	`Created` date DEFAULT NULL,
	`Updated` date DEFAULT NULL,
	PRIMARY KEY (`Id`),
	KEY `timestamp` (`Timestamp`),
	KEY `to` (`ToUser`),
	KEY `from` (`fromUser`),
	KEY `send` (`Send`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Rooms;
CREATE TABLE spb_Webim_Rooms (
      `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
      `Owner` varchar(40) NOT NULL,
      `Name` varchar(40) NOT NULL,
      `Nick` varchar(60) NOT NULL DEFAULT '',
      `Topic` varchar(60) DEFAULT NULL,
      `Url` varchar(100) DEFAULT '#',
      `Created` datetime DEFAULT NULL,
      `Updated` datetime DEFAULT NULL,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `webim_room_name` (`Name`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Members;
CREATE TABLE spb_Webim_Members (
      `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
      `Room` varchar(60) NOT NULL,
      `Uid` varchar(40) NOT NULL,
      `Nick` varchar(60) NOT NULL,
      `Joined` datetime DEFAULT NULL,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `webim_member_room_uid` (`Room`,`Uid`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Blocked;
CREATE TABLE spb_Webim_Blocked (
      `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
      `Room` varchar(60) NOT NULL,
      `Uid` varchar(40) NOT NULL,
      `Blocked` datetime DEFAULT NULL,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `webim_blocked_room_uid` (`Uid`,`Room`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Visitors;
CREATE TABLE spb_Webim_Visitors (
      `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
      `Name` varchar(60) DEFAULT NULL,
      `Ipaddr` varchar(60) DEFAULT NULL,
      `Url` varchar(100) DEFAULT NULL,
      `Referer` varchar(100) DEFAULT NULL,
      `Location` varchar(100) DEFAULT NULL,
      `Created` datetime DEFAULT NULL,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `Webim_visitor_name` (`Name`)
)ENGINE=MyISAM AUTO_INCREMENT=10000 DEFAULT CHARSET=utf8;

