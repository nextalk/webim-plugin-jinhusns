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

        public string Get(long uid)
        {
            string s = CreateDAO().FirstOrDefault<string>("select data from spb_Webim_Settings where uid = @0", uid);
            if (s == null) s = "{}";

            return s;
        }

        public void Set(long uid, string data)
        {
            if (Get(uid) == "{}")
            {
                CreateDAO().Execute("INSERT INTO spb_Webim_Settings(uid, data) values(@0, @1)", uid, data);
                //SettingEntity setting = SettingEntity.New();
                //setting.Uid = uid;
                //setting.Data = data;
                //Insert(setting);
            }
            else { 
                CreateDAO().Execute("update spb_Webim_Settings set data =@0  where uid = @1", data, uid);
            }
        }

    }

}
