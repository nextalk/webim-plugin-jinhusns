
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
    /// 临时讨论组仓储接口
    /// </summary>
    public interface IRoomRepository : IRepository<RoomEntity>
    {

        RoomEntity Find(string name);

        IEnumerable<RoomEntity> Rooms(string uid);

        IEnumerable<RoomEntity> Rooms(string uid, IEnumerable<string> ids);

        void Remove(string name);
        
	}


}
