using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Bill;

internal class BillMasterMapper : IItemAsyncMapper<BillMasterBusiness, BillMasterBusiness>
{
	public static BillMasterMapper Instance { get; private set; } = new BillMasterMapper();


	public Task<BillMasterBusiness> MapAsync(string contextGuid, BillMasterBusiness source, BillMasterBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BillMasterBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
			destination.Date = source.Date;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.Guid = source.Guid;
			destination.SanadNumber = source.SanadNumber;
			destination.SanadSt = source.SanadSt;
			destination.Sum = source.Sum;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\BillMasterMapper.cs", 39);
			throw;
		}
	}

	public Task<List<BillMasterBusiness>> MapListAsync(string contextGuid, IEnumerable<BillMasterBusiness> source, List<BillMasterBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
