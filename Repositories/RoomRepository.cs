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
            //TODO:
            RoomEntity entity = CreateDAO().FirstOrDefault<RoomEntity>("select * from spb_Webim_Rooms where name = @0", name);
            return entity;
        }

        public IEnumerable<RoomEntity> Rooms(string uid)
        {
            //TODO:
            return new List<RoomEntity>();

        }

        public IEnumerable<RoomEntity> Rooms(string uid, IEnumerable<string> ids)
        {
            //TODO:
            return new List<RoomEntity>();
        }

        public void Remove(string name)
        {
        }
    
    }

}


