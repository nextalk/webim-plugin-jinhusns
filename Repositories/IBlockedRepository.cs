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
using Webim;

namespace Spacebuilder.Webim
{
    /// <summary>
    /// 群组屏蔽仓储接口
    /// </summary>
    public interface IBlockedRepository : IRepository<BlockedEntity>
    {

        bool IsBlocked(string room, string uid);

        void Remove(string room, string uid); 

	}

}

