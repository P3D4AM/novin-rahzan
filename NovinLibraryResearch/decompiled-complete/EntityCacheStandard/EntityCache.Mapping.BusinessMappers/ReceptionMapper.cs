using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ReceptionMapper : IItemAsyncMapper<ReceptionBusiness, WebReception>, IItemAsyncMapper<ReceptionBusiness, ReceptionBusiness>, IItemAsyncMapper<WebReception, ReceptionBusiness>, IItemAsyncMapper<WebReception, ReceptionReportBusiness>, IItemAsyncMapper<WebReceptionReport, ReceptionReportBusiness>, IItemAsyncMapper<ReceptionReportBusiness, WebReceptionReport>, IItemAsyncMapper<WebReceptionCompactForDisplay, WebReception>, IItemAsyncMapper<WebReception, WebReceptionCompactForDisplay>
{
	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	public static ReceptionMapper Instance { get; private set; } = new ReceptionMapper();


	public async Task<WebReception> MapAsync(string contextGuid, ReceptionBusiness source, WebReception destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 6;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebReception();
			}
			destination.IsTemp = source.IsTemp;
			destination.IsChecked = source.IsChecked;
			destination.Source = source.Source;
			WebReception webReception = destination;
			webReception.Id2 = await source.GetId2Async(token, tranName);
			destination.Status = source.Status;
			destination.DependencyType = source.DependencyType;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.UserGuid = source.UserGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Ip = source.Ip;
			destination.LkSerial = source.LkSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.SanadNumber = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeliveryStatus = source.DeliveryStatus;
			webReception = destination;
			webReception.BeforeAccount = await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName);
			webReception = destination;
			webReception.AfterAccount = await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName);
			destination.PaymentMethod = source.PaymentMethod;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.DateMoaser = source.DateMoaser;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.OrderGuid = source.OrderGuid;
			destination.OrderType = source.OrderType;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.ListBank != null)
			{
				webReception = destination;
				webReception.ListBank = await ReceptionHavaleMapper.GetInstance(source).MapListAsync(contextGuid, source.ListBank, new List<WebReceptionHavale>(), tranName, token);
			}
			if (source.ListNaghd != null)
			{
				webReception = destination;
				webReception.ListNaghd = await ReceptionNaghdMapper.Instance.MapListAsync(contextGuid, source.ListNaghd, new List<WebReceptionNaghd>(), tranName);
			}
			if (source.ListCheck != null)
			{
				webReception = destination;
				webReception.ListCheck = await ReceptionCheckMapper.Instance.MapListAsync(contextGuid, source.ListCheck, new List<WebReceptionCheck>(), tranName, token);
			}
			if (source.ListEnteghal != null)
			{
				webReception = destination;
				webReception.ListEnteghal = await SanadDetMapper.Instance.MapListAsync(contextGuid, source.ListEnteghal, new List<WebSanad_Det>(), tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 90);
			throw;
		}
	}

	public Task<List<WebReception>> MapListAsync(string contextGuid, IEnumerable<ReceptionBusiness> source, List<WebReception> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ReceptionBusiness> MapAsync(string contextGuid, ReceptionBusiness source, ReceptionBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 5;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ReceptionBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.IsTemp = source.IsTemp;
			destination.IsChecked = source.IsChecked;
			destination.Source = source.Source;
			ReceptionBusiness receptionBusiness = destination;
			receptionBusiness.SetId2(await source.GetId2Async(token, tranName));
			destination.Status = source.Status;
			destination.DependencyType = source.DependencyType;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			destination.MoeinGuid = source.MoeinGuid;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
			destination.UserGuid = source.UserGuid;
			destination.Ip = source.Ip;
			destination.LkSerial = source.LkSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.PaymentMethod = source.PaymentMethod;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.DateMoaser = source.DateMoaser;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.OrderGuid = source.OrderGuid;
			destination.OrderType = source.OrderType;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.ListBank != null)
			{
				receptionBusiness = destination;
				receptionBusiness.AddItemsToDetList(await ReceptionHavaleMapper.GetInstance(source).MapListAsync(contextGuid, source.ListBank, new List<ReceptionHavaleBusiness>(), tranName, token), EnSystem.Mapper);
			}
			if (source.ListNaghd != null)
			{
				receptionBusiness = destination;
				receptionBusiness.AddItemsToDetList(await ReceptionNaghdMapper.Instance.MapListAsync(contextGuid, source.ListNaghd, new List<ReceptionNaghdBusiness>(), tranName), EnSystem.Mapper);
			}
			if (source.ListCheck != null)
			{
				receptionBusiness = destination;
				receptionBusiness.AddItemsToDetList(await ReceptionCheckMapper.Instance.MapListAsync(contextGuid, source.ListCheck, new List<ReceptionCheckBusiness>(), tranName, token), EnSystem.Mapper);
			}
			if (source.ListEnteghal != null)
			{
				receptionBusiness = destination;
				receptionBusiness.AddItemsToDetList(await SanadDetMapper.Instance.MapListAsync(contextGuid, source.ListEnteghal, new List<Sanad_DetBusiness>(), tranName), EnSystem.Mapper);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 154);
			throw;
		}
	}

	public Task<List<ReceptionBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionBusiness> source, List<ReceptionBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ReceptionBusiness> MapAsync(string contextGuid, WebReception source, ReceptionBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		await _semaphore.WaitAsync();
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (source.UserGuid == Guid.Empty)
			{
				throw new ArgumentException("نکن برادر من UserGuid رو پرکن".GetTranslate() + "\r\nsourceData:" + Json.ToStringJson(source));
			}
			if (destination == null)
			{
				destination = new ReceptionBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.IsTemp = source.IsTemp;
			destination.IsChecked = source.IsChecked;
			destination.Source = source.Source;
			destination.SetId2(source.Id2);
			destination.Status = source.Status;
			destination.DependencyType = source.DependencyType;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = (source.DateUtc.HasValue ? source.DateUtc.Value.ToLocalTime() : source.Date);
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			destination.MoeinGuid = source.MoeinGuid;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
			destination.UserGuid = source.UserGuid;
			destination.Ip = source.Ip;
			destination.LkSerial = source.LkSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.SanadNo = source.SanadNumber;
			destination.SanadSt = source.SanadSt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.SetBeforeAccount(source.BeforeAccount);
			destination.SetAfterAccount(source.AfterAccount);
			destination.PaymentMethod = source.PaymentMethod;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.DateMoaser = source.DateMoaser;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.OrderGuid = source.OrderGuid;
			destination.OrderType = source.OrderType;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.ListBank != null)
			{
				foreach (WebReceptionHavale item in source.ListBank)
				{
					ReceptionHavaleBusiness destination2 = new ReceptionHavaleBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					ReceptionBusiness receptionBusiness = destination;
					receptionBusiness.AddToDetList(await ReceptionHavaleMapper.GetInstance(source).MapAsync(contextGuid, item, destination2, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.ListCheck != null)
			{
				CashierBusiness cashier = await CashierBusiness.GetAsync(contextGuid, source.UserGuid, token, tranName);
				if (cashier == null)
				{
					source.UserGuid = (await CashierBusiness.GetAllAsync(contextGuid, true, token))?.FirstOrDefault()?.Guid ?? source.UserGuid;
					cashier = await CashierBusiness.GetAsync(contextGuid, source.UserGuid, token, tranName);
				}
				foreach (WebReceptionCheck item2 in source.ListCheck)
				{
					ReceptionCheckBusiness destination3 = new ReceptionCheckBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckMapper.Instance.MapAsync(contextGuid, item2, destination3, tranName, token);
					if ((receptionCheckBusiness?.VisitorGuid ?? Guid.Empty) == Guid.Empty)
					{
						receptionCheckBusiness.VisitorGuid = cashier.VisitorGuid;
					}
					if ((receptionCheckBusiness?.DebitTafsilGuid ?? Guid.Empty) == ParentDefaults.TafsilCoding.CLSTafsil29766 && (cashier?.SandoghGuid ?? Guid.Empty) != Guid.Empty)
					{
						receptionCheckBusiness.DebitTafsilGuid = cashier.SandoghGuid.Value;
					}
					destination.AddToDetList(receptionCheckBusiness, EnSystem.Mapper, token);
				}
			}
			if (source.ListNaghd != null)
			{
				foreach (WebReceptionNaghd item3 in source.ListNaghd)
				{
					ReceptionNaghdBusiness destination4 = new ReceptionNaghdBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					ReceptionBusiness receptionBusiness = destination;
					receptionBusiness.AddToDetList(await ReceptionNaghdMapper.Instance.MapAsync(contextGuid, item3, destination4, tranName, token), EnSystem.Mapper);
				}
			}
			if (source.ListEnteghal != null)
			{
				foreach (WebSanad_Det item4 in source.ListEnteghal)
				{
					Sanad_DetBusiness destination5 = new Sanad_DetBusiness(contextGuid, EnSystem.None, source.Guid, Document.Reception, destination.NextOrdinal(token))
					{
						MasterGuid = destination.Guid
					};
					destination.AddToDetList(await SanadDetMapper.Instance.MapAsync(contextGuid, item4, destination5, tranName, token), EnSystem.Mapper, token);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 262);
			throw;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public Task<List<ReceptionBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReception> source, List<ReceptionBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionReportBusiness> MapAsync(string contextGuid, WebReception source, ReceptionReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionReportBusiness(contextGuid);
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.CashierGuid = source.UserGuid;
			destination.ContextGuid = contextGuid;
			destination.AfterAccount = source.AfterAccount;
			destination.AndroidIme = source.AndroidIme;
			destination.BeforeAccount = source.BeforeAccount;
			destination.CpuSerial = source.CpuSerial;
			destination.DateM = (source.DateUtc.HasValue ? source.DateUtc.Value.ToLocalTime() : source.Date);
			destination.DateSabt = source.DateSabt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.DependencyType = source.DependencyType;
			destination.HddSerial = source.HddSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Id2 = source.Id2;
			destination.IdUser = source.IdUser;
			destination.Ip = source.Ip;
			destination.IsChecked = source.IsChecked;
			destination.IsTemp = source.IsTemp;
			destination.LkSerial = source.LkSerial;
			destination.Modified = source.Modified;
			destination.PaymentMethod = source.PaymentMethod;
			destination.TransferDate = source.TransferDate;
			destination.SanadSt = source.SanadSt;
			destination.Source = source.Source;
			destination.Status = source.Status;
			destination.SumHavale = source.SumBank;
			destination.SumChequeSh = source.SumCheck;
			destination.SumEnteghal = source.SumEnteghal;
			destination.SumNaghd = source.SumNaghd;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 328);
			throw;
		}
	}

	public Task<List<ReceptionReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReception> source, List<ReceptionReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebReceptionReport> MapAsync(string contextGuid, ReceptionReportBusiness source, WebReceptionReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceptionReport();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
			destination.AfterAccount = source.AfterAccount;
			destination.AndroidIme = source.AndroidIme;
			destination.BeforeAccount = source.BeforeAccount;
			destination.CpuSerial = source.CpuSerial;
			destination.DateSabt = source.DateSabt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.DependencyType = source.DependencyType;
			destination.HddSerial = source.HddSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Id2 = source.Id2;
			destination.IdUser = source.IdUser;
			destination.Ip = source.Ip;
			destination.IsChecked = source.IsChecked;
			destination.IsTemp = source.IsTemp;
			destination.LkSerial = source.LkSerial;
			destination.Modified = source.Modified;
			destination.TransferDate = source.TransferDate;
			destination.SanadSt = source.SanadSt;
			destination.Source = source.Source;
			destination.Status = source.Status;
			destination.SumEnteghal = source.SumEnteghal;
			destination.SumNaghd = source.SumNaghd;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 381);
			throw;
		}
	}

	public Task<List<WebReceptionReport>> MapListAsync(string contextGuid, IEnumerable<ReceptionReportBusiness> source, List<WebReceptionReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionReportBusiness> MapAsync(string contextGuid, WebReceptionReport source, ReceptionReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionReportBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
			destination.AfterAccount = source.AfterAccount;
			destination.AndroidIme = source.AndroidIme;
			destination.BeforeAccount = source.BeforeAccount;
			destination.CpuSerial = source.CpuSerial;
			destination.DateSabt = source.DateSabt;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.DependencyType = source.DependencyType;
			destination.HddSerial = source.HddSerial;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Id2 = source.Id2;
			destination.IdUser = source.IdUser;
			destination.Ip = source.Ip;
			destination.IsChecked = source.IsChecked;
			destination.IsTemp = source.IsTemp;
			destination.LkSerial = source.LkSerial;
			destination.Modified = source.Modified;
			destination.TransferDate = source.TransferDate;
			destination.SanadSt = source.SanadSt;
			destination.Source = source.Source;
			destination.Status = source.Status;
			destination.SumEnteghal = source.SumEnteghal;
			destination.SumNaghd = source.SumNaghd;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 435);
			throw;
		}
	}

	public Task<List<ReceptionReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReceptionReport> source, List<ReceptionReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebReception> MapAsync(string contextGuid, WebReceptionCompactForDisplay source, WebReception destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReception();
			}
			destination.IsChecked = source.IsChecked;
			destination.Source = source.Source;
			destination.Date = source.Date;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadNumber = source.SanadNo;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.FullName = source.CustomerFullName;
			destination.Guid = source.Guid;
			destination.UserGuid = source.CashierGuid;
			destination.Id = source.id;
			destination.Id2 = source.Id2;
			destination.Source = source.Source;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 474);
			throw;
		}
	}

	public Task<List<WebReception>> MapListAsync(string contextGuid, IEnumerable<WebReceptionCompactForDisplay> source, List<WebReception> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebReceptionCompactForDisplay> MapAsync(string contextGuid, WebReception source, WebReceptionCompactForDisplay destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceptionCompactForDisplay();
			}
			destination.IsChecked = source.IsChecked;
			destination.Source = source.Source;
			destination.Date = source.Date;
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.TafsilGuid = source.TafsilGuid;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.Guid = source.Guid;
			destination.Id2 = source.Id2;
			destination.Source = source.Source;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionMapper.cs", 508);
			throw;
		}
	}

	public Task<List<WebReceptionCompactForDisplay>> MapListAsync(string contextGuid, IEnumerable<WebReception> source, List<WebReceptionCompactForDisplay> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
