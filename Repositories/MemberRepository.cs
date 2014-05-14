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
            var sql = Sql.Builder;
            sql.Select("*").From("spb_Webim_Members").Where("room = @0", room);
            return CreateDAO().Fetch<MemberEntity>(sql);
        }

        public void JoinRoom(string room, string uid, string nick)
        {
            MemberEntity e = MemberEntity.New();
            e.Room = room;
            e.Uid = uid;
            e.Nick = nick;
            base.Insert(e);
        }

        public void LeaveRoom(string room, string uid)
        {
            CreateDAO().Execute("delete from spb_Webim_Members where room = @0 and uid = @1", room, uid);
        }
    
    }

}


