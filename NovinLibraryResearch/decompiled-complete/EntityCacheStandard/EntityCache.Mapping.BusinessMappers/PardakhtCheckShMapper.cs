using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PardakhtCheckShMapper : IItemAsyncMapper<PardakhtCheckShBusiness, WebPardakhtCheckSh>, IItemAsyncMapper<PardakhtCheckShBusiness, PardakhtCheckShBusiness>, IItemAsyncMapper<WebPardakhtCheckSh, PardakhtCheckShBusiness>
{
	public static PardakhtCheckShMapper Instance { get; private set; } = new PardakhtCheckShMapper();


	public Task<WebPardakhtCheckSh> MapAsync(string contextGuid, PardakhtCheckShBusiness source, WebPardakhtCheckSh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPardakhtCheckSh();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSaresid = source.DateSaresid;
			destination.Description = source.RowDesc;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.Status = (short)source.Status;
			destination.BankTafsilGuid = source.BankTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.PardakhtDate = source.PardakhtDate;
			destination.Aval2re = source.Aval2re;
			destination.A2re_TafsilGuid = source.A2re_TafsilGuid;
			destination.A2re_MoeinGuid = source.A2re_MoeinGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DebitA2reMoeinGuid = source.DebitA2reMoeinGuid;
			destination.DebitA2reTafsiGuid = source.DebitA2reTafsiGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName;
			destination.SayadiIdentity = source.SayadiIdentity;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.MetaData = source.MetaData;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.Ordinal = source.Ordinal;
			destination.DateUtc = source.DateUtc;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMapper.cs", 59);
			throw;
		}
	}

	public Task<List<WebPardakhtCheckSh>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckShBusiness> source, List<WebPardakhtCheckSh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtCheckShBusiness> MapAsync(string contextGuid, PardakhtCheckShBusiness source, PardakhtCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtCheckShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSaresid = source.DateSaresid;
			destination.RowDesc = source.RowDesc;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.Status = source.Status;
			destination.BankTafsilGuid = source.BankTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.PardakhtDate = source.PardakhtDate;
			destination.Aval2re = source.Aval2re;
			destination.A2re_TafsilGuid = source.A2re_TafsilGuid;
			destination.A2re_MoeinGuid = source.A2re_MoeinGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DebitA2reMoeinGuid = source.DebitA2reMoeinGuid;
			destination.DebitA2reTafsiGuid = source.DebitA2reTafsiGuid;
			destination.SayadiName = source.SayadiName;
			destination.SayadiIdentity = source.SayadiIdentity;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.MetaData = source.MetaData;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.DateUtc;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMapper.cs", 107);
			throw;
		}
	}

	public Task<List<PardakhtCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckShBusiness> source, List<PardakhtCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtCheckShBusiness> MapAsync(string contextGuid, WebPardakhtCheckSh source, PardakhtCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtCheckShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateSaresid = source.DateSaresid;
			destination.RowDesc = source.Description;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.Status = (ENCheckSHStatus)source.Status;
			destination.BankTafsilGuid = source.BankTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.PardakhtDate = source.PardakhtDate;
			destination.Aval2re = source.Aval2re;
			destination.A2re_TafsilGuid = source.A2re_TafsilGuid;
			destination.A2re_MoeinGuid = source.A2re_MoeinGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DebitA2reMoeinGuid = source.DebitA2reMoeinGuid;
			destination.DebitA2reTafsiGuid = source.DebitA2reTafsiGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName;
			destination.SayadiIdentity = source.SayadiIdentity;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.MetaData = source.MetaData;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.DateSaresid.ToUniversalTime();
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMapper.cs", 156);
			throw;
		}
	}

	public Task<List<PardakhtCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPardakhtCheckSh> source, List<PardakhtCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
