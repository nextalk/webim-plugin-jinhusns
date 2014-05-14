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
    public class RoomRepository : Repository<RoomEntity>, IRoomRepository
    {
        public RoomEntity Find(string name)
        {
            long id = CreateDAO().FirstOrDefault<long>("select Id from spb_Webim_Rooms where Name = @0", name);
            //TODO:
            if (id > 0) { return base.Get(id); }
            return null;
        }

        public IEnumerable<RoomEntity> Rooms(string uid)
        {
            var sql = Sql.Builder;
            sql.Select("room").From("spb_Webim_Members").Where("uid = @0", uid);
            List<string> names = CreateDAO().Fetch<string>(sql);
            if (names != null && names.Count > 0) {
                sql = Sql.Builder;
                sql.Select("*").From("spb_Webim_Rooms").Where("names in (@0)", names);
                return CreateDAO().Fetch<RoomEntity>(sql);
            }
            return new List<RoomEntity>();

        }

        public IEnumerable<RoomEntity> Rooms(string uid, IEnumerable<string> ids)
        {
            if (ids.Count() > 0) {
                var sql = Sql.Builder;
                sql.Select("*").From("spb_Webim_Rooms").Where("names in (@0)", ids);
                return CreateDAO().Fetch<RoomEntity>(sql);
            }
            return new List<RoomEntity>();
        }

        public void Remove(string name)
        {
            CreateDAO().Execute("delete from spb_Webim_Rooms where name = @0", name);
        }

        public int MemberCount(string room)
        {
            var sql = Sql.Builder;
            sql.Select("count(id)").From("spb_Webim_Members").Where("room = @0", room);
            return CreateDAO().FirstOrDefault<int>(sql);
        }
    
    }

}


