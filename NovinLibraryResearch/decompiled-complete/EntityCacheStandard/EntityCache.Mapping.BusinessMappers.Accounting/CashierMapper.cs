using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class CashierMapper : IItemAsyncMapper<CashierBusiness, WebCashier>, IItemAsyncMapper<CashierBusiness, CashierBusiness>, IItemAsyncMapper<WebCashier, CashierBusiness>, IItemAsyncMapper<CashierTime, CashierTime>
{
	public static CashierMapper Instance { get; private set; } = new CashierMapper();


	public async Task<WebCashier> MapAsync(string contextGuid, CashierBusiness source, WebCashier destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebCashier();
			}
			destination.Guid = source.Guid;
			destination.UserGuid = source.UserGuid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.UserName = source.UserName;
			destination.HashedPassword = source?.HashedPassword?.ToList()?.ToArray();
			destination.Description = source.Description;
			destination.Status = source.Status;
			WebCashier webCashier = destination;
			webCashier.CashierTime = await MapListAsync(contextGuid, source?.CashierTime, new List<CashierTime>(), tranName, token);
			destination.DateM = source.DateM;
			destination.Settings = source.Settings;
			destination.BankGuid1 = (((source.BankGuid1 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid1);
			destination.BankGuid2 = (((source.BankGuid2 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid2);
			destination.BankGuid3 = (((source.BankGuid3 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid3);
			destination.BankGuid4 = (((source.BankGuid4 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid4);
			webCashier = destination;
			Guid? cusGuid = ((!(((await source.GetCusGuidAsync(EnSystem.Mapper)) ?? Guid.Empty) == Guid.Empty)) ? (await source.GetCusGuidAsync(EnSystem.Mapper)) : null);
			webCashier.CusGuid = cusGuid;
			destination.SandoghGuid = (((source.SandoghGuid ?? Guid.Empty) == Guid.Empty) ? null : source.SandoghGuid);
			destination.DepotGuid = (((source.DepotGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DepotGuid);
			destination.AllVisitorAccess = source.AllVisitorAccess;
			destination.LoginAccess = source.LoginAccess;
			destination.AllSandoghsAccess = source.AllSandoghsAccess;
			destination.AllDepotsAccess = source.AllDepotsAccess;
			destination.AllBanksAccess = source.AllBanksAccess;
			destination.VisitorGuid = (((source.VisitorGuid ?? Guid.Empty) == Guid.Empty) ? null : source.VisitorGuid);
			destination.DefaultCusGroup = (((source.DefaultCusGroup ?? Guid.Empty) == Guid.Empty) ? null : source.DefaultCusGroup);
			destination.ReceptionSmsCustomerGuid = source.ReceptionSmsCustomerGuid;
			destination.PardakhtSmsCustomerGuid = source.PardakhtSmsCustomerGuid;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			destination.FareTafsilGuid = (((source.FareTafsilGuid ?? Guid.Empty) == Guid.Empty) ? null : source.FareTafsilGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierMapper.cs", 62);
			throw;
		}
		return destination;
	}

	public async Task<CashierBusiness> MapAsync(string contextGuid, CashierBusiness source, CashierBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new CashierBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.UserGuid = source.UserGuid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.UserName = source.UserName;
			destination.HashedPassword = source?.HashedPassword?.ToList()?.ToArray();
			destination.Description = source.Description;
			destination.Status = source.Status;
			CashierBusiness cashierBusiness;
			if ((source?.CashierTime?.Any()).GetValueOrDefault())
			{
				cashierBusiness = destination;
				cashierBusiness.CashierTime = await MapListAsync(contextGuid, source.CashierTime, new List<CashierTime>(), tranName, token);
			}
			else
			{
				destination.CashierTime = null;
			}
			destination.DateM = source.DateM;
			destination.SetSetting(source.Settings);
			destination.BankGuid1 = (((source.BankGuid1 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid1);
			destination.BankGuid2 = (((source.BankGuid2 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid2);
			destination.BankGuid3 = (((source.BankGuid3 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid3);
			destination.BankGuid4 = (((source.BankGuid4 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid4);
			cashierBusiness = destination;
			Guid? cusGuid = ((!(((await source.GetCusGuidAsync(EnSystem.Mapper)) ?? Guid.Empty) == Guid.Empty)) ? (await source.GetCusGuidAsync(EnSystem.Mapper)) : null);
			cashierBusiness.SetCusGuid(cusGuid);
			destination.SandoghGuid = (((source.SandoghGuid ?? Guid.Empty) == Guid.Empty) ? null : source.SandoghGuid);
			destination.DepotGuid = (((source.DepotGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DepotGuid);
			destination.AllVisitorAccess = source.AllVisitorAccess;
			destination.LoginAccess = source.LoginAccess;
			destination.AllSandoghsAccess = source.AllSandoghsAccess;
			destination.AllDepotsAccess = source.AllDepotsAccess;
			destination.AllBanksAccess = source.AllBanksAccess;
			destination.VisitorGuid = (((source.VisitorGuid ?? Guid.Empty) == Guid.Empty) ? null : source.VisitorGuid);
			destination.DefaultCusGroup = (((source.DefaultCusGroup ?? Guid.Empty) == Guid.Empty) ? null : source.DefaultCusGroup);
			destination.ReceptionSmsCustomerGuid = source.ReceptionSmsCustomerGuid;
			destination.PardakhtSmsCustomerGuid = source.PardakhtSmsCustomerGuid;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			destination.FareTafsilGuid = (((source.FareTafsilGuid ?? Guid.Empty) == Guid.Empty) ? null : source.FareTafsilGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierMapper.cs", 114);
			throw;
		}
		return destination;
	}

	public async Task<CashierBusiness> MapAsync(string contextGuid, WebCashier source, CashierBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CashierBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.UserGuid = source.UserGuid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.UserName = source.UserName;
			destination.HashedPassword = source.HashedPassword;
			destination.Description = source.Description;
			destination.Status = source.Status;
			CashierBusiness cashierBusiness = destination;
			cashierBusiness.CashierTime = await MapListAsync(contextGuid, source?.CashierTime, new List<CashierTime>(), tranName, token);
			destination.DateM = source.DateM;
			destination.BankGuid1 = (((source.BankGuid1 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid1);
			destination.BankGuid2 = (((source.BankGuid2 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid2);
			destination.BankGuid3 = (((source.BankGuid3 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid3);
			destination.BankGuid4 = (((source.BankGuid4 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid4);
			destination.SetCusGuid(((source.CusGuid ?? Guid.Empty) == Guid.Empty) ? null : source.CusGuid);
			destination.SandoghGuid = (((source.SandoghGuid ?? Guid.Empty) == Guid.Empty) ? null : source.SandoghGuid);
			destination.DepotGuid = (((source.DepotGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DepotGuid);
			destination.AllVisitorAccess = source.AllVisitorAccess;
			destination.LoginAccess = source.LoginAccess;
			destination.AllSandoghsAccess = source.AllSandoghsAccess;
			destination.AllDepotsAccess = source.AllDepotsAccess;
			destination.AllBanksAccess = source.AllBanksAccess;
			destination.VisitorGuid = (((source.VisitorGuid ?? Guid.Empty) == Guid.Empty) ? null : source.VisitorGuid);
			destination.DefaultCusGroup = (((source.DefaultCusGroup ?? Guid.Empty) == Guid.Empty) ? null : source.DefaultCusGroup);
			destination.SetSetting(source.Settings);
			destination.ReceptionSmsCustomerGuid = source.ReceptionSmsCustomerGuid;
			destination.PardakhtSmsCustomerGuid = source.PardakhtSmsCustomerGuid;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			destination.FareTafsilGuid = (((source.FareTafsilGuid ?? Guid.Empty) == Guid.Empty) ? null : source.FareTafsilGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierMapper.cs", 162);
			throw;
		}
		return destination;
	}

	public Task<CashierTime> MapAsync(string contextGuid, CashierTime source, CashierTime destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CashierTime();
			}
			destination.DTime1 = source.DTime1;
			destination.DTime2 = source.DTime2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierMapper.cs", 182);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CashierBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCashier> source, List<CashierBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CashierBusiness>> MapListAsync(string contextGuid, IEnumerable<CashierBusiness> source, List<CashierBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCashier>> MapListAsync(string contextGuid, IEnumerable<CashierBusiness> source, List<WebCashier> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CashierTime>> MapListAsync(string contextGuid, IEnumerable<CashierTime> source, List<CashierTime> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
