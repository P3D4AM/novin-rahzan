using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface ISanadPrint : ISyncRepository<SanadPrintBusiness>, IEditableDelivery, IRepository<SanadPrintBusiness>, INoGuidRepository<SanadPrintBusiness>, IDisposable
{
	Task<SanadPrintBusiness> GetAsync(Guid asGuid, Document asType, CancellationToken token, string tranName);

	Task<SanadPrintBusiness> GetAsync(long number, CancellationToken token, string tranName);
}
