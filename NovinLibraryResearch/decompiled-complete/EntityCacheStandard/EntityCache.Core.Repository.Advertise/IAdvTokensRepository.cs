using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvTokensRepository : IRepository<AdvTokensBusiness>, INoGuidRepository<AdvTokensBusiness>, IDisposable
{
	Task<AdvTokensBusiness> GetBySimCardAsync(long simCardNumber, AdvertiseType type, string tranName);

	Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, bool? status, string tranName);

	Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, AdvertiseType type, bool? status, string tranName);

	Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(AdvertiseType type, bool? status, string tranName);
}
