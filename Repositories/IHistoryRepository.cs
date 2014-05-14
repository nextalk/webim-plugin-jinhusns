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
    /// 历史消息仓储接口
    /// </summary>
    public interface IHistoryRepository : IRepository<HistoryEntity>
    {
		IEnumerable<HistoryEntity> GetHistories(string uid, string with, string type="chat", int limit = 50);

		void ClearHistories(string uid, string with);

		IEnumerable<HistoryEntity> GetOfflineMessages(string uid, int limit = 50);

		void OfflineMessageToHistory(string uid);

	}

}
