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
    public class SettingRepository : Repository<SettingEntity>, ISettingRepository
    {

		public SettingRepository()
		{

		}

		public string Get(int uid)
		{
			//"select data from spb_Webim_Settings where uid = ?;"
			return "";
		}

		public void Set(int uid, string data)
		{
			//"update spb_Webim_Settings set data = ? where uid = ?;"
		}

	}

}
