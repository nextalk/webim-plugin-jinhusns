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
    /// 群组仓储接口
    /// </summary>
    public class HistoryRepository : Repository<HistoryEntity>, IHistoryRepository
    {

		public HistoryRepository()
		{
		}
		
		public void insert(HistoryEntity entity)
		{

		}
		
		public IEnumerable<HistoryEntity> GetHistories(int uid, string with, string type="unicast", int limit = 30)
		{
			if(type == "unicast" ){
				/*
				"SELECT * FROM spb_Webim_Histories WHERE `type` = 'unicast' 
					AND ((`to`=%s AND `from`=%s AND `fromdel` != 1) 
					OR (`send` = 1 AND `from`=%s AND `to`=%s AND `todel` != 1))  
					ORDER BY timestamp DESC LIMIT %d", $with, $uid, $with, $uid, $limit );
				*/
			} else {
				/*
				"SELECT * FROM  spb_Webim_Histories 
					WHERE `to`=%s AND `type`='multicast' AND send = 1 
					ORDER BY timestamp DESC LIMIT %d", , $with, $limit);
				*/
			}

			return new List<HistoryEntity>();
		}

		public void ClearnHistories(int uid, string with)
		{
			//"UPDATE spb_Webim_Histories SET fromdel = 1 Where from = ? and to = ? and type = 'unicast';", uid, with

			//"UPDATE spb_Webim_Histories SET todel = 1 Where to = ? and from = ? and type = 'unicast';", uid, with

			//"DELETE FROM spb_Webim_Histories WHERE fromdel = 1 AND todel = 1;"
		}

		public	IEnumerable<HistoryEntity> GetOfflineMessages(int uid, int limit = 50)
		{
			//"SELECT * FROM  spb_Webim_Histories WHERE `to` = ? and send != 1 ORDER BY timestamp DESC LIMIT %d", limit;
			return new List<HistoryEntity>();	
		}

		public void OfflineMessageToHistory(int uid)
		{
			//"UPDATE spb_Webim_Histories SET send = 1 where to = ? and send = 0"); uid
		}


	}

}
