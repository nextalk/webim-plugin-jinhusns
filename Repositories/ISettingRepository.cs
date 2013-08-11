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
    public interface ISettingRepository : IRepository<SettingEntity>
    {

		string Get(int uid);

		void Set(int uid, string data);

	}

}
