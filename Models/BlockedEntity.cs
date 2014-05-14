
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;

//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Blocked]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Blocked](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Uid] [varchar](40) NOT NULL,
//    [Room] [varchar](40) NOT NULL,
//    [Blocked] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Rooms] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END

namespace Spacebuilder.Webim
{
	[TableName("spb_Webim_Blocked")]
	[PrimaryKey("id", autoIncrement = true)]
    public class BlockedEntity : IEntity
    {
        public static BlockedEntity New() 
        {
            BlockedEntity entity = new BlockedEntity();
            entity.blocked = DateTime.Now;
            return entity;
        }

		#region 需要持久属性
		public long Id { get; set; }

		public long Uid { get; set; }

		public long Room { get; set; }

		public DateTime Blocked { get; set; }
		#endregion

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

    }

}


