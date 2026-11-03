using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection.ProductGroup;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Selection.ProductGroup;

public class PrdGroupSelectedDetListMapper : IItemAsyncMapper<PrdGroupSelectedDetListBusiness, PrdGroupSelectedDetListBusiness>
{
	public static PrdGroupSelectedDetListMapper Instance { get; private set; } = new PrdGroupSelectedDetListMapper();


	public async Task<PrdGroupSelectedDetListBusiness> MapAsync(string contextGuid, PrdGroupSelectedDetListBusiness source, PrdGroupSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new PrdGroupSelectedDetListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetGroupGuid(source.GetGroupGuid());
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListMapper.cs", 35);
			throw;
		}
	}

	public Task<List<PrdGroupSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdGroupSelectedDetListBusiness> source, List<PrdGroupSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
