
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
	`CreatedAt` date DEFAULT NULL,
	`UpdatedAt` date DEFAULT NULL,
	PRIMARY KEY (`Id`),
	KEY `timestamp` (`Timestamp`),
	KEY `to` (`ToUser`),
	KEY `from` (`fromUser`),
	KEY `send` (`Send`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS spb_Webim_Settings;
CREATE TABLE spb_Webim_Settings(
	`Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
	`Uid` int(11) unsigned NOT NULL,
	`Data` text,
	`Created_at` DATETIME DEFAULT NULL,
	`Updated_at` DATETIME DEFAULT NULL,
	PRIMARY KEY (`Id`) 
)ENGINE=MyISAM DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS spb_Webim_Visitors;
CREATE TABLE spb_Webim_Visitors (
      `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
      `Name` varchar(60) DEFAULT NULL,
      `Ipaddr` varchar(60) DEFAULT NULL,
      `Url` varchar(100) DEFAULT NULL,
      `Referer` varchar(100) DEFAULT NULL,
      `Location` varchar(100) DEFAULT NULL,
      `Created_at` datetime DEFAULT NULL,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `Webim_visitor_name` (`Name`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

