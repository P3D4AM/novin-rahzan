using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Cheque;

public class ChequeShOperationMapper : IItemAsyncMapper<ChequeShOperationBusiness, WebChequeShOperation>, IItemAsyncMapper<ChequeShOperationBusiness, ChequeShOperationBusiness>, IItemAsyncMapper<WebChequeShOperation, ChequeShOperationBusiness>
{
	public static ChequeShOperationMapper Instance { get; private set; } = new ChequeShOperationMapper();


	public Task<WebChequeShOperation> MapAsync(string contextGuid, ChequeShOperationBusiness source, WebChequeShOperation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebChequeShOperation();
			}
			destination.DateM = source.DateM;
			destination.DateMUtc = source.DateM.ToUniversalTime();
			destination.CashierGuid = source.CashierGuid;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.SanadNo = source.SanadNo;
			destination.ChequeGuid = source.ChequeGuid;
			destination.StatusId = source.StatusId;
			destination.DateUtc = source.DateUtc;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeShOperationMapper.cs", 50);
			throw;
		}
	}

	public Task<ChequeShOperationBusiness> MapAsync(string contextGuid, ChequeShOperationBusiness source, ChequeShOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeShOperationBusiness(contextGuid);
			}
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.SanadNo = source.SanadNo;
			destination.ChequeGuid = source.ChequeGuid;
			destination.StatusId = source.StatusId;
			destination.ContextGuid = contextGuid;
			destination.DateUtc = source.DateUtc;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeShOperationMapper.cs", 86);
			throw;
		}
	}

	public Task<ChequeShOperationBusiness> MapAsync(string contextGuid, WebChequeShOperation source, ChequeShOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeShOperationBusiness(contextGuid);
			}
			destination.DateM = (source.DateMUtc.HasValue ? source.DateMUtc.Value.ToLocalTime() : source.DateM);
			destination.CashierGuid = source.CashierGuid;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.SanadNo = source.SanadNo;
			destination.ChequeGuid = source.ChequeGuid;
			destination.StatusId = source.StatusId;
			destination.ContextGuid = contextGuid;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeShOperationMapper.cs", 122);
			throw;
		}
	}

	public Task<List<WebChequeShOperation>> MapListAsync(string contextGuid, IEnumerable<ChequeShOperationBusiness> source, List<WebChequeShOperation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeShOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeShOperationBusiness> source, List<ChequeShOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeShOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebChequeShOperation> source, List<ChequeShOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
