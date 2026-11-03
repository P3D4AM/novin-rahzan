using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_SellDetailMapper : IItemAsyncMapper<Scale_SellDetailBusiness, Scale_SellDetailBusiness>
{
	public static Scale_SellDetailMapper Instance { get; private set; } = new Scale_SellDetailMapper();


	public Task<Scale_SellDetailBusiness> MapAsync(string contextGuid, Scale_SellDetailBusiness source, Scale_SellDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_SellDetailBusiness(contextGuid);
			}
			destination.PortNumber = source.PortNumber;
			destination.ScaleGuid = source.ScaleGuid;
			destination.FactorNumber = source.FactorNumber;
			destination.MemoryNumber = source.MemoryNumber;
			destination.Quantity = source.Quantity;
			destination.UnitPrice = source.UnitPrice;
			destination.Type_Weight = source.Type_Weight;
			destination.DateM = source.DateM;
			destination.DateSH_H = source.DateSH_H;
			destination.DateUpdate = source.DateUpdate;
			destination.ComputerName = source.ComputerName;
			destination.IsRead = source.IsRead;
			destination.Ghorfe = source.Ghorfe;
			destination.RowSum = source.RowSum;
			destination.RInt1 = source.RInt1;
			destination.RInt2 = source.RInt2;
			destination.RText1 = source.RText1;
			destination.RText2 = source.RText2;
			destination.RDbl1 = source.RDbl1;
			destination.RDbl2 = source.RDbl2;
			destination.IP = source.IP;
			destination.SC_Guid = source.SC_Guid;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_SellDetailMapper.cs", 52);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_SellDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SellDetailBusiness> source, List<Scale_SellDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
