using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;

namespace EntityCache.Core.Repository.Accounting.Tolid;

public interface ITolidFormulaRepository : IRepository<TolidFormulaBusiness>, INoGuidRepository<TolidFormulaBusiness>, IDisposable
{
	Task<List<TolidFormulaBusiness>> GetAllByProductAsync(Guid prdGuid, CancellationToken token, string tranName);
}
