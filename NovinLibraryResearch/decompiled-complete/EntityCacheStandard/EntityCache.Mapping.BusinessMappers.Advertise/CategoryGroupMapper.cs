using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class CategoryGroupMapper : IItemAsyncMapper<CategoryGroupBusiness, CategoryGroupBusiness>
{
	public static CategoryGroupMapper Instance { get; private set; } = new CategoryGroupMapper();


	public Task<CategoryGroupBusiness> MapAsync(string contextGuid, CategoryGroupBusiness source, CategoryGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CategoryGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DivarCategoryList = source.DivarCategoryList;
			destination.SheypoorCategoryList = source.SheypoorCategoryList;
			destination.Name = source.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\CategoryGroupMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CategoryGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<CategoryGroupBusiness> source, List<CategoryGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
