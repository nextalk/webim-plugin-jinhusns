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
    /// 历史消息仓储
    /// </summary>
    public class HistoryRepository : Repository<HistoryEntity>, IHistoryRepository
    {

        public IEnumerable<HistoryEntity> GetHistories(long uid, string with, string type = "chat", int limit = 30)
        {
            Sql sql = Sql.Builder;
            if (type == "chat")
            {
                /*
                "SELECT * FROM spb_Webim_Histories WHERE `type` = 'chat' 
                AND ((`to`=%s AND `from`=%s AND `fromdel` != 1) 
                OR (`send` = 1 AND `from`=%s AND `to`=%s AND `todel` != 1))  
                ORDER BY timestamp DESC LIMIT %d", $with, $uid, $with, $uid, $limit );
                */
                sql.Where("type='chat'")
                    .Where("(touser=@0 and fromuser=@1 and fromdel!=1) or (send=1 and fromuser=@0 and touser=@1 and todel!=1)", with, uid.ToString());
            }
            else
            {
                /*
                "SELECT * FROM  spb_Webim_Histories 
                    WHERE `to`=%s AND `type`='grpchat' AND send = 1 
                    ORDER BY timestamp DESC LIMIT %d", , $with, $limit);
                */
                sql.Where("touser=@0", with)
                    .Where("type = 'grpchat'")
                    .Where("send = 1");
            }
            sql.OrderBy("timestamp DESC");
            var list = CreateDAO().FetchTopPrimaryKeys<HistoryEntity>(limit, sql).Select(n => Convert.ToInt64(n)).Reverse();
            return PopulateEntitiesByEntityIds(list);
        }

        public void ClearHistories(long uid, string with)
        {
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET fromdel = 1 Where fromuser = @0 and touser = @1 and type = 'chat'", uid.ToString(), with);
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET todel = 1 Where touser = @0 and fromuser = @1 and type = 'chat'", uid.ToString(), with);
            CreateDAO().Execute("DELETE FROM spb_Webim_Histories WHERE fromdel = 1 AND todel = 1");
        }

        public IEnumerable<HistoryEntity> GetOfflineMessages(long uid, int limit = 50)
        {
            //"SELECT * FROM  spb_Webim_Histories WHERE `to` = ? and send != 1 ORDER BY timestamp DESC LIMIT %d", limit;

            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("spb_Webim_Histories")
                .Where("touser = @0", uid.ToString())
                .Where("send != 1")
                .OrderBy("timestamp DESC");
            var list = CreateDAO().FetchTopPrimaryKeys<HistoryEntity>(limit, sql).Select(n => Convert.ToInt64(n)).Reverse();
            return PopulateEntitiesByEntityIds(list);
        }

        public void OfflineMessageToHistory(long uid)
        {
            //"UPDATE spb_Webim_Histories SET send = 1 where touser = ? and send = 0"); uid
            CreateDAO().Execute("UPDATE spb_Webim_Histories SET send = 1 where touser = @0 and send = 0", uid.ToString());
        }

    }

}
