using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Global;

public interface IIntroductionMethodRepository : ISyncRepository<IntroductionMethodBusiness>, IEditableDelivery, IRepository<IntroductionMethodBusiness>, INoGuidRepository<IntroductionMethodBusiness>, IDisposable
{
	Task<IntroductionMethodBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
