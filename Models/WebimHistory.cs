using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;

//SqlServer脚本
//
//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Histories](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Send] [tinyint] NULL,
//    [Type] [varchar](20) NULL,
//    [ToUser] [varchar](50) NOT NULL,
//    [FromUser] [varchar](50) NOT NULL,
//    [Nick] [varchar](20) NULL,
//    [Body] [text] NULL,
//    [Style] [varchar](150) NULL,
//    [Timestamp] [bigint] NULL,
//    [ToDel] [tinyint] NOT NULL,
//    [FromDel] [tinyint] NOT NULL,
//    [CreatedAt] [date] NULL,
//    [UpdatedAt] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Histories] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END

namespace Spacebuilder.Webim
{

	[TableName("spb_Webim_Histories")]
	[PrimaryKey("id", autoIncrement = true)]
    public class HistoryEntity : IEntity
    {
		public static HistoryEntity New()
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

		public string ToUser { get; set; }

		public string FromUser { get; set; }

		public string Nick { get; set; }
		
		public string Body { get; set; }

		public string Style { get; set; }

		public double Timestamp { get; set; }
	
		public int ToDel { get; set; }

		public int FromDel { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		#endregion

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

    }

}
