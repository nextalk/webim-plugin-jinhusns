﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;


//SQL Server脚本 
//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Settings]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Settings](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Uid] [bigint] NOT NULL,
//    [Data] [text] NULL,
//    [CreatedAt] [date] NULL,
//    [UpdatedAt] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Settings] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END


namespace Spacebuilder.Webim
{
	[TableName("spb_Webim_Settings")]
	[PrimaryKey("id", autoIncrement = true)]
    public class SettingEntity : IEntity
    {

		public static SettingEntity New()
		{
			SettingEntity e = new SettingEntity();
			e.Data = "";
			e.CreatedAt = DateTime.UtcNow;
			e.UpdatedAt = DateTime.UtcNow;
			return e;
		}

		public long Id { get; set; }

		public long Uid { get; set; }

		public string Data { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

    }
}
