
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using PetaPoco;
using Tunynet;

//SQL Server脚本
//IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Rooms]') AND type in (N'U'))
//BEGIN
//CREATE TABLE [dbo].[spb_Webim_Rooms](
//    [Id] [int] IDENTITY(1,1) NOT NULL,
//    [Owner] [varchar](40) NOT NULL,
//    [Name] [varchar](40) NOT NULL,
//    [Nick] [varchar](60) NOT NULL,
//    [Topic] [varchar](60) NULL,
//    [Created] [date] NULL,
//    [Updated] [date] NULL,
// CONSTRAINT [PK_spb_Webim_Rooms] PRIMARY KEY CLUSTERED 
//(
//    [Id] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//END

namespace Spacebuilder.Webim
{

	[TableName("spb_Webim_Rooms")]
	[PrimaryKey("id", autoIncrement = true)]
    public class RoomEntity : IEntity
    {
        public static RoomEntity New() 
        {
            RoomEntity entity = new RoomEntity();
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            return entity;
        }

		#region 需要持久属性

		public long Id { get; set; }

        public string Owner { get; set; }

        public string Name { get; set; }

        public string Nick { get; set; }

        public string Topic { get; set; }

		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }

		#endregion

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

    }

}

