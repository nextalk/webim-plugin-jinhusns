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
    /// 访客仓储接口
    /// </summary>
    public class VisitorRepository : Repository<VisitorEntity>, IVisitorRepository
    {
        public VisitorEntity Find(string vid) 
        {
            return null;
        }

        public IEnumerable<VisitorEntity> FindByIds(IEnumerable<string> vids)
        {
            return new List<VisitorEntity>();   
        }

    }
}


