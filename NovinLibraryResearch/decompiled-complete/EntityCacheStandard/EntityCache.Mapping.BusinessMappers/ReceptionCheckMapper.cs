using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ReceptionCheckMapper : IItemAsyncMapper<ReceptionCheckBusiness, WebReceptionCheck>, IItemAsyncMapper<ReceptionCheckBusiness, ReceptionCheckBusiness>, IItemAsyncMapper<WebReceptionCheck, ReceptionCheckBusiness>, IItemAsyncMapper<ReceptionCheckReportBusiness, ReceptionCheckBusiness>
{
	public static ReceptionCheckMapper Instance { get; private set; } = new ReceptionCheckMapper();


	public Task<WebReceptionCheck> MapAsync(string contextGuid, ReceptionCheckBusiness source, WebReceptionCheck destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceptionCheck();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.BankName = source.BankName;
			destination.Date = source.Date;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
			destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
			destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
			destination.Aval2re = source.Aval2re;
			destination.Description = source.Description;
			destination.HesabNum = source.HesabNum;
			destination.Number = source.Number;
			destination.Personal = source.Personal;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.Price = source.Price;
			destination.ServerStatus = source.ServerStatus;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
			destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName ?? "";
			destination.SayadiIdentity = source.SayadiIdentity ?? "";
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.MetaData = source.MetaData;
			destination.Ordinal = source.Ordinal;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionCheckMapper.cs", 62);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebReceptionCheck>> MapListAsync(string contextGuid, IEnumerable<ReceptionCheckBusiness> source, List<WebReceptionCheck> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionCheckBusiness> MapAsync(string contextGuid, ReceptionCheckBusiness source, ReceptionCheckBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionCheckBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.BankName = source.BankName;
			destination.Date = source.Date;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
			destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
			destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
			destination.Aval2re = source.Aval2re;
			destination.Description = source.Description;
			destination.HesabNum = source.HesabNum;
			destination.Number = source.Number;
			destination.Personal = source.Personal;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.ServerStatus = source.ServerStatus;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
			destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName ?? "";
			destination.SayadiIdentity = source.SayadiIdentity ?? "";
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.MetaData = source.MetaData;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionCheckMapper.cs", 114);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ReceptionCheckBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionCheckBusiness> source, List<ReceptionCheckBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionCheckBusiness> MapAsync(string contextGuid, WebReceptionCheck source, ReceptionCheckBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionCheckBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.BankName = source.BankName;
			destination.Date = source.Date;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
			destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
			destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
			destination.Aval2re = source.Aval2re;
			destination.Description = source.Description;
			destination.HesabNum = source.HesabNum;
			destination.Number = source.Number;
			destination.Personal = source.Personal;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.ServerStatus = source.ServerStatus;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
			destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName ?? "";
			destination.SayadiIdentity = source.SayadiIdentity ?? "";
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.MetaData = source.MetaData;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.Date.ToUniversalTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionCheckMapper.cs", 166);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ReceptionCheckBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReceptionCheck> source, List<ReceptionCheckBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionCheckBusiness> MapAsync(string contextGuid, ReceptionCheckReportBusiness source, ReceptionCheckBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionCheckBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.BankName = source.BankName;
			destination.Date = source.Date;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.UserGuid = source.UserGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
			destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
			destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
			destination.Aval2re = source.Aval2re;
			destination.Description = source.Description;
			destination.HesabNum = source.HesabNum;
			destination.Number = source.Number;
			destination.Personal = source.Personal;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
			destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.SayadiName = source.SayadiName ?? "";
			destination.SayadiIdentity = source.SayadiIdentity ?? "";
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.MetaData = source.MetaData;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionCheckMapper.cs", 216);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ReceptionCheckBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionCheckReportBusiness> source, List<ReceptionCheckBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
