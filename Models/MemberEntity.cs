
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;

//SQL Server脚本
//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Members]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Members](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Room] [varchar](40) NULL,
//    [Nick] [varchar](40) NULL,
//    [Uid] [varchar](40) NULL,
//    [Joined] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Members] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END

namespace Spacebuilder.Webim
{

	[TableName("spb_Webim_Members")]
	[PrimaryKey("id", autoIncrement = true)]
    public class MemberEntity : IEntity
    {
        public static MemberEntity New() 
        {
            MemberEntity entity = new MemberEntity();
            entity.Joined = DateTime.Now;
            return entity;
        }

		#region 需要持久属性
		public long Id { get; set; }

        public string Room { get; set; }

        public string Nick { get; set; }

        public string Uid { get; set; }

		public DateTime Joined { get; set; }
		#endregion

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

    }

}
