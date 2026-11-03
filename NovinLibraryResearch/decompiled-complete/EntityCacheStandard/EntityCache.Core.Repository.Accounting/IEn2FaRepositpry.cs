using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IEn2FaRepositpry : IRepository<En2FaBusiness>, INoGuidRepository<En2FaBusiness>, IDisposable
{
	Task<string> ConvertFa(string en, CancellationToken token, string tranName);
}
