using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using PacketParser.BusinessLayer.Models;

namespace EntityCache.Core.Repository;

public interface ICusCardRepository : IRepository<CusCardBusiness>, INoGuidRepository<CusCardBusiness>, IDisposable
{
	Task<IEnumerable<CusCardBusiness>> GetAllByCusGuidAsync(Guid custGuid, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<CusCardBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);

	Task<CusCardBusiness> GetAsync(string cardNumber, CancellationToken token, string tranName);

	Task<IEnumerable<CustomerCardModel>> GetAllModelAsync(CancellationToken token, string tranName);
}
