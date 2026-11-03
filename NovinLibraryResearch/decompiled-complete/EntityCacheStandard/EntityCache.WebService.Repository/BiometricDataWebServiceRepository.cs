using System;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using PacketParser.Entities;

namespace EntityCache.WebService.Repository;

public class BiometricDataWebServiceRepository : WebServiceRepository<BiometricBusiness>, IDisposable
{
	public BiometricDataWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<BiometricBusiness>)cache.CacheRepository.BioMetricData, cache)
	{
		try
		{
			Cache = cache;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\BiometricDataWebServiceRepository.cs", 15);
		}
	}
}
