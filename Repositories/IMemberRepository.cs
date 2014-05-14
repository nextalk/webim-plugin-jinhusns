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
    /// 群组成员仓储接口
    /// </summary>
    public interface IMemberRepository : IRepository<MemberEntity>
    {

        IEnumerable<MemberEntity> AllInRoom(string room);

        IEnumerable<RoomEntity> Rooms(string uid);

        void JoinRoom(string room, string uid, string nick);

        void LeaveRoom(string room, string uid);

	}

}


