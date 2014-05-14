//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;

namespace Spacebuilder.Webim
{
    /// <summary>
    /// 群组屏蔽仓储接口
    /// </summary>
    public class BlockedRepository : Repository<BlockedEntity>, IBlockedRepository
    {

        public BlockedRepository()
        {
        }
        /// <summary>
        /// 判断群组是否屏蔽
        /// </summary>
        /// <param name="room">组名</param>
        /// <param name="uid">用户id</param>
        public bool IsBlocked(string room, string uid)
        {
            int id = CreateDAO().FirstOrDefault<int>("select id from spb_Webim_Blocked where uid = @0 and name = @1", uid, room);
            if(id == 0) return false;
            return true;
        }

        /// <summary>
        /// 解除群组屏蔽
        /// </summary>
        /// <param name="room">组名</param>
        /// <param name="uid">用户id</param>
        public void Remove(string room, string uid)
        {
            CreateDAO().Execute("delete from spb_Webim_Blocked where room = @0 and uid = @1", room, uid);
        } 

	}

}


