using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection.ProductGroup;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Selection.ProductGroup;

public class PrdGroupSelectedMapper : IItemAsyncMapper<PrdGroupSelectedListBusiness, PrdGroupSelectedListBusiness>
{
	public static PrdGroupSelectedMapper Instance { get; private set; } = new PrdGroupSelectedMapper();


	public async Task<PrdGroupSelectedListBusiness> MapAsync(string contextGuid, PrdGroupSelectedListBusiness source, PrdGroupSelectedListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdGroupSelectedListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			if (source.DetList?.Any() ?? false)
			{
				destination.AddRangeToDetList(await PrdGroupSelectedDetListMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<PrdGroupSelectedDetListBusiness>(), tranName, token));
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedMapper.cs", 42);
			throw;
		}
	}

	public Task<List<PrdGroupSelectedListBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdGroupSelectedListBusiness> source, List<PrdGroupSelectedListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
