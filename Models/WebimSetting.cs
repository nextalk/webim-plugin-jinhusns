using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spacebuilder.Common;
using Tunynet.Common;

/*
 * CREATE TABLE webim_settings(
 *   `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
 *   `uid` mediumint(8) unsigned NOT NULL,
 *   `data` text,
 *   `created_at` date DEFAULT NULL,
 *   `updated_at` date DEFAULT NULL,
 *   PRIMARY KEY (`id`)
 * )ENGINE=MyISAM;
 */
namespace Spacebuilder.Webim
{
	[TableName("spb_Webim_Settings")]
	[PrimaryKey("id", autoIncrement = true)]
    public class SettingEntity : IEntity
    {

		public static SettingEntity New()
		{
			SettingEntity e = new SettingEntity();
			e.data = "";
			e.CreatedAt = DateTime.UtcNow;
			e.UpdatedAt = DateTime.UtcNow;
			return e;
		}

		public long Id { get; set; }

		public int Uid { get; set; }

		public string Data { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

    }
}
