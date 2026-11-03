using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Reception;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Khazane.Reception;

public class ReceptionChequeEntezamiMapper : IItemAsyncMapper<ReceptionChequeEntezamiBusiness, ReceptionChequeEntezamiBusiness>
{
	public static ReceptionChequeEntezamiMapper Instance { get; private set; } = new ReceptionChequeEntezamiMapper();


	public Task<ReceptionChequeEntezamiBusiness> MapAsync(string contextGuid, ReceptionChequeEntezamiBusiness source, ReceptionChequeEntezamiBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionChequeEntezamiBusiness(contextGuid);
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
			destination.ReceptionDateM = source.ReceptionDateM;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiMapper.cs", 40);
			throw;
		}
	}

	public Task<List<ReceptionChequeEntezamiBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionChequeEntezamiBusiness> source, List<ReceptionChequeEntezamiBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
