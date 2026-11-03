using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_ProductMapper : IItemAsyncMapper<Scale_ProductBusiness, Scale_ProductBusiness>
{
	public static Scale_ProductMapper Instance { get; private set; } = new Scale_ProductMapper();


	public Task<Scale_ProductBusiness> MapAsync(string contextGuid, Scale_ProductBusiness source, Scale_ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_ProductBusiness(contextGuid);
			}
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.GroupGuid = source.GroupGuid;
			destination.VahedGuid = source.VahedGuid;
			destination.IsWeight = source.IsWeight;
			destination.Price = source.Price;
			destination.Barcode = source.Barcode;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Guid = source.Guid;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_ProductMapper.cs", 41);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_ProductBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ProductBusiness> source, List<Scale_ProductBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
