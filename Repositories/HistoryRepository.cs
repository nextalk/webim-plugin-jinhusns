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
    /// <summary>
    /// 群组仓储接口
    /// </summary>
    public class HistoryRepository : Repository<HistoryEntity>, IHistoryRepository
    {

        public IEnumerable<HistoryEntity> GetHistories(long uid, string with, string type = "unicast", int limit = 30)
        {
            Sql sql = Sql.Builder;
            if (type == "unicast")
            {
                /*
                "SELECT * FROM spb_Webim_Histories WHERE `type` = 'unicast' 
                AND ((`to`=%s AND `from`=%s AND `fromdel` != 1) 
                OR (`send` = 1 AND `from`=%s AND `to`=%s AND `todel` != 1))  
                ORDER BY timestamp DESC LIMIT %d", $with, $uid, $with, $uid, $limit );
                */
                sql.Select("*")
                    .From("spb_Webim_Histories")
                    .Where("type='unicast'")
                    .Where("(to=@0 and from=@1 and fromdel!=1) or (send=1 and from=@0 and to=@1 and todel!=1)", with, uid.ToString());
            }
            else
            {
                /*
                "SELECT * FROM  spb_Webim_Histories 
                    WHERE `to`=%s AND `type`='multicast' AND send = 1 
                    ORDER BY timestamp DESC LIMIT %d", , $with, $limit);
                */
                sql.Select("*")
                    .From("spb_Webim_Histories")
                    .Where("to=@0", with)
                    .Where("type = 'multicast'")
                    .Where("send = 1");
            }
            sql.OrderBy("timestamp DESC");
            var list = CreateDAO().FetchTopPrimaryKeys<HistoryEntity>(limit, sql).Cast<long>();
            return PopulateEntitiesByEntityIds(list);
        }

        public void ClearHistories(long uid, string with)
        {
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET fromdel = 1 Where from = @0 and to = @1 and type = 'unicast'", uid.ToString(), with);
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET todel = 1 Where to = @0 and from = @1 and type = 'unicast'", uid.ToString(), with);
            CreateDAO().Execute("DELETE FROM spb_Webim_Histories WHERE fromdel = 1 AND todel = 1");
        }

        public IEnumerable<HistoryEntity> GetOfflineMessages(long uid, int limit = 50)
        {
            //"SELECT * FROM  spb_Webim_Histories WHERE `to` = ? and send != 1 ORDER BY timestamp DESC LIMIT %d", limit;
            
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("spb_Webim_Histories")
                .Where("to = @0", uid.ToString())
                .Where("send != 1")
                .OrderBy("timestamp DESC");
            var list = CreateDAO().FetchTopPrimaryKeys<HistoryEntity>(limit, sql).Cast<long>();
            return PopulateEntitiesByEntityIds(list);
        }

        public void OfflineMessageToHistory(long uid)
        {
            //"UPDATE spb_Webim_Histories SET send = 1 where to = ? and send = 0"); uid
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET send = 1 where to = @0 and send = 0", uid.ToString());
        }

    }

}
