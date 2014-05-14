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
using PetaPoco;

namespace Spacebuilder.Webim
{
    public class MemberRepository : Repository<MemberEntity>, IMemberRepository 
    {

        public IEnumerable<MemberEntity> AllInRoom(string room)
        {
            //TODO:
            return new List<MemberEntity>();
        }

        public IEnumerable<RoomEntity> Rooms(string uid) 
        {
            //TODO:
            return new List<MemberEntity>();
        }

        public void JoinRoom(string room, string uid, string nick)
        {
            //TODO:
        }

        public void LeaveRoom(string room, string uid)
        {
            //TODO:
        }
    
    }

}


