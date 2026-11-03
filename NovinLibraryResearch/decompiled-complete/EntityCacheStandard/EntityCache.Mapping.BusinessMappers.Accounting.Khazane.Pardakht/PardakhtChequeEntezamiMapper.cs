using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Pardakht;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Khazane.Pardakht;

public class PardakhtChequeEntezamiMapper : IItemAsyncMapper<PardakhtChequeEntezamiBusiness, PardakhtChequeEntezamiBusiness>
{
	public static PardakhtChequeEntezamiMapper Instance { get; private set; } = new PardakhtChequeEntezamiMapper();


	public Task<PardakhtChequeEntezamiBusiness> MapAsync(string contextGuid, PardakhtChequeEntezamiBusiness source, PardakhtChequeEntezamiBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtChequeEntezamiBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Modified = source.Modified;
			destination.ChequeGuid = source.ChequeGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.PardakhtDateM = source.PardakhtDateM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PardakhtChequeEntezamiBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtChequeEntezamiBusiness> source, List<PardakhtChequeEntezamiBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
