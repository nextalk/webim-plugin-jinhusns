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
using Webim;

namespace Spacebuilder.Webim
{
    /// <summary>
    /// 访客仓储接口
    /// </summary>
    public interface IVisitorRepository : IRepository<VisitorEntity>
    {
        VisitorEntity Find(string vid);

        IEnumerable<VisitorEntity> FindByIds(IEnumerable<string> vids);

    }

}


