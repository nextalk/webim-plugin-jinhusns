//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;

namespace Spacebuilder.Webim
{
    /// <summary>
    /// 群组屏蔽仓储接口
    /// </summary>
    public class BlockedRepository : Repository<BlockedEntity>, IBlockedEntity
    {

        public BlockedRepository()
        {
        }

        public bool IsBlocked(string room, string uid)
        {
            long id = CreateDAO().FirstOrDefault<long>("select id from spb_Webim_Blocked where uid = @0 and name = @1", uid, room);
            if(id == null) return false;
            return true;
        }

        public void Remove(string room, string uid)
        {
            CreateDAO().Execute("delete from spb_Webim_Blocked where room = @0 and uid = @1", room, uid);
        } 

	}

}


