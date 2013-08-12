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
    public interface IHistoryRepository : IRepository<HistoryEntity>
    {
		IEnumerable<HistoryEntity> GetHistories(long uid, string with, string type="unicast", int limit = 30);
		void ClearHistories(long uid, string with);

		IEnumerable<HistoryEntity> GetOfflineMessages(long uid, int limit = 50);

		void OfflineMessageToHistory(long uid);

	}

}
