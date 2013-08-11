using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spacebuilder.Common;
using Tunynet.Common;

/* CREATE TABLE webim_histories (
 * 	`id` int(11) unsigned NOT NULL AUTO_INCREMENT,
 * 	`send` tinyint(1) DEFAULT NULL,
 * 	`type` varchar(20) DEFAULT NULL,
 * 	`to` varchar(50) NOT NULL,
 * 	`from` varchar(50) NOT NULL,
 * 	`nick` varchar(20) DEFAULT NULL COMMENT 'from nick',
 * 	`body` text,
 * 	`style` varchar(150) DEFAULT NULL,
 *	`timestamp` double DEFAULT NULL,
 *	`todel` tinyint(1) NOT NULL DEFAULT '0',
 *	`fromdel` tinyint(1) NOT NULL DEFAULT '0',
 *	`created_at` date DEFAULT NULL,
 *	`updated_at` date DEFAULT NULL,
 *	PRIMARY KEY (`id`),
 *	KEY `todel` (`todel`),
 *	KEY `fromdel` (`fromdel`),
 *	KEY `timestamp` (`timestamp`),
 *	KEY `to` (`to`),
 *	KEY `from` (`from`),
 *	KEY `send` (`send`)
 * ) ENGINE=MyISAM;
 */

namespace Spacebuilder.Webim
{

	[TableName("spb_Webim_Histories")]
	[PrimaryKey("id", autoIncrement = true)]
    public class HistoryEntity : IEntity
    {
		public static HistoryEntity new()
		{
			HistoryEntity entity = new HistoryEntity();
			entity.CreatedAt = DateTime.UtcNow;
			entity.UpdatedAt = DateTime.UtcNow;
			return entity;
		}

		#region 需要持久属性
		public long Id { get; set; }

		public int Send { get; set; }

		public string Type { get; set; }

		public string To { get; set; }

		public string From { get; set; }

		public string Nick { get; set; }
		
		public string Body { get; set; }

		public string Style { get; set; }

		public double Timestamp { get; set; }
	
		public int ToDel { get; set; }

		public int FromDel { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		#endregion

    }

}
