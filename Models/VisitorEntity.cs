
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;

//SqlServer脚本
//
//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Visitors]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Visitors](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Name] [varchar](60) NOT NULL,
//    [Ipaddr] [varchar](60) NOT NULL,
//    [Url] [varchar](60) NOT NULL,
//    [Referer] [varchar](60) NOT NULL,
//    [Location] [varchar](60) NOT NULL,
//    [Created] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Visitors] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END
namespace Spacebuilder.Webim
{

	[TableName("spb_Webim_Visitors")]
	[PrimaryKey("id", autoIncrement = true)]
    public class VisitorEntity : IEntity
    {
		#region 需要持久属性
		public long Id { get; set; }
		public string Name { get; set; }
		public string Ipaddr { get; set; }
		public string Url { get; set; }
		public string Referer { get; set; }
		public string Location { get; set; }
		public DateTime Created { get; set; }

		#endregion

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }
    }

}
