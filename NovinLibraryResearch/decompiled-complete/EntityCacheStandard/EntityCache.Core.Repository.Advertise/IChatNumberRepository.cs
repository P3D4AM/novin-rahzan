using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IChatNumberRepository : IRepository<ChatNumberBusiness>, INoGuidRepository<ChatNumberBusiness>, IDisposable
{
	Task<bool> CheckNumber(string number, string tranName);

	Task<List<string>> GetAllCities(string tranName);

	Task<List<string>> GetAllCategories(string tranName);
}
