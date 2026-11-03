using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class DepartmentCategoryMapper : IItemAsyncMapper<DepartmentCategoryBusiness, DepartmentCategoryBusiness>
{
	public static DepartmentCategoryMapper Instance { get; private set; } = new DepartmentCategoryMapper();


	public Task<DepartmentCategoryBusiness> MapAsync(string contextGuid, DepartmentCategoryBusiness source, DepartmentCategoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new DepartmentCategoryBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DepartmentGuid = source.DepartmentGuid;
			destination.FirstLevel = source.FirstLevel;
			destination.SecondLevel = source.SecondLevel;
			destination.ThirdLevel = source.ThirdLevel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\DepartmentCategoryMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<DepartmentCategoryBusiness>> MapListAsync(string contextGuid, IEnumerable<DepartmentCategoryBusiness> source, List<DepartmentCategoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
