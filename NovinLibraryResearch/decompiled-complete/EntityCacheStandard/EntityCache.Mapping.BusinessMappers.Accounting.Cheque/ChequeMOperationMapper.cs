using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Cheque;

public class ChequeMOperationMapper : IItemAsyncMapper<ChequeMOperationBusiness, WebChequeMOperation>, IItemAsyncMapper<ChequeMOperationBusiness, ChequeMOperationBusiness>, IItemAsyncMapper<WebChequeMOperation, ChequeMOperationBusiness>
{
	public static ChequeMOperationMapper Instance { get; private set; } = new ChequeMOperationMapper();


	public Task<WebChequeMOperation> MapAsync(string contextGuid, ChequeMOperationBusiness source, WebChequeMOperation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebChequeMOperation();
			}
			destination.GuidCheque = source.GuidCheque;
			destination.DateM = source.DateM;
			destination.DateMUtc = source.DateM.ToUniversalTime();
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSabt = source.DateSabt;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.Avale2re_Transfer = source.Avale2re_Transfer;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.IsCreditTafsilAffective = source.IsCreditTafsilAffective;
			destination.IsDebitTafsilAffective = source.IsDebitTafsilAffective;
			destination.Description = source.Description;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.IsSanadAffective = source.IsSanadAffective;
			destination.StatusId = source.StatusId;
			destination.DateUtc = source.DateUtc;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeMOperationMapper.cs", 58);
			throw;
		}
	}

	public Task<ChequeMOperationBusiness> MapAsync(string contextGuid, ChequeMOperationBusiness source, ChequeMOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeMOperationBusiness(contextGuid);
			}
			destination.GuidCheque = source.GuidCheque;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSabt = source.DateSabt;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.Avale2re_Transfer = source.Avale2re_Transfer;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.IsCreditTafsilAffective = source.IsCreditTafsilAffective;
			destination.IsDebitTafsilAffective = source.IsDebitTafsilAffective;
			destination.Description = source.Description;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.IsSanadAffective = source.IsSanadAffective;
			destination.StatusId = source.StatusId;
			destination.ContextGuid = contextGuid;
			destination.DateUtc = source.DateUtc;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeMOperationMapper.cs", 102);
			throw;
		}
	}

	public Task<ChequeMOperationBusiness> MapAsync(string contextGuid, WebChequeMOperation source, ChequeMOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeMOperationBusiness(contextGuid);
			}
			destination.GuidCheque = source.GuidCheque;
			destination.DateM = (source.DateMUtc.HasValue ? source.DateMUtc.Value.ToLocalTime() : source.DateM);
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSabt = source.DateSabt;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.Avale2re_Transfer = source.Avale2re_Transfer;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Ordinal = source.Ordinal;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.IsCreditTafsilAffective = source.IsCreditTafsilAffective;
			destination.IsDebitTafsilAffective = source.IsDebitTafsilAffective;
			destination.Description = source.Description;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.IsSanadAffective = source.IsSanadAffective;
			destination.StatusId = source.StatusId;
			destination.ContextGuid = contextGuid;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeMOperationMapper.cs", 146);
			throw;
		}
	}

	public Task<List<WebChequeMOperation>> MapListAsync(string contextGuid, IEnumerable<ChequeMOperationBusiness> source, List<WebChequeMOperation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeMOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeMOperationBusiness> source, List<ChequeMOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeMOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebChequeMOperation> source, List<ChequeMOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
