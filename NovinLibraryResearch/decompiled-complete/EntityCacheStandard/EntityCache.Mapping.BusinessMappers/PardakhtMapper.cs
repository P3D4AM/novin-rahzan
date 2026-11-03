using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PardakhtMapper : IItemAsyncMapper<PardakhtBusiness, WebPardakht>, IItemAsyncMapper<PardakhtBusiness, PardakhtBusiness>, IItemAsyncMapper<WebPardakht, PardakhtBusiness>, IItemAsyncMapper<WebPaymentCompactForDisplay, WebPaymentCompact>, IItemAsyncMapper<WebPaymentCompact, WebPaymentCompactForDisplay>
{
	public static PardakhtMapper Instance { get; private set; } = new PardakhtMapper();


	public async Task<WebPardakht> MapAsync(string contextGuid, PardakhtBusiness source, WebPardakht destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebPardakht();
			}
			WebPardakht webPardakht = destination;
			webPardakht.AfterAccount = await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName);
			webPardakht = destination;
			webPardakht.BeforeAccount = await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName);
			destination.IsTemp = source.IsTemp;
			webPardakht = destination;
			webPardakht.Id2 = await source.GetId2Async(token, tranName);
			destination.MoeinGuid = source.MoeinGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.UserGuid = source.UserGuid;
			destination.Ip = source.Ip;
			destination.IsRead = source.IsRead;
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
			destination.ServerStatus = source.ServerStatus;
			destination.CashierGuid = source.UserGuid;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.DateMoaser = source.DateMoaser;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.Naghd != null)
			{
				webPardakht = destination;
				webPardakht.Naghd = await PardakhtNaghdMapper.Instance.MapListAsync(contextGuid, source.Naghd, new List<WebPardakhtNaghd>());
			}
			if (source.CheckM != null)
			{
				webPardakht = destination;
				webPardakht.CheckM = await PardakhtCheckMMapper.Instance.MapListAsync(contextGuid, source.CheckM, new List<WebPardakhtCheckM>(), tranName, token);
			}
			if (source.CheckSh != null)
			{
				webPardakht = destination;
				webPardakht.CheckSh = await PardakhtCheckShMapper.Instance.MapListAsync(contextGuid, source.CheckSh, new List<WebPardakhtCheckSh>(), tranName, token);
				foreach (WebPardakhtCheckSh item in destination.CheckSh)
				{
					item.DebitMoeinGuid = source.MoeinGuid;
					item.DebitTafsilGuid = source.TafsilGuid;
				}
			}
			if (source.Bank != null)
			{
				webPardakht = destination;
				webPardakht.Bank = await PardakhtBankMapper.Instance.MapListAsync(contextGuid, source.Bank, new List<WebPardakhtBank>(), tranName, token);
			}
			if (source.Enteghal != null)
			{
				webPardakht = destination;
				webPardakht.Enteghal = await SanadDetMapper.Instance.MapListAsync(contextGuid, source.Enteghal, new List<WebSanad_Det>(), tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtMapper.cs", 86);
			throw;
		}
	}

	public async Task<PardakhtBusiness> MapAsync(string contextGuid, PardakhtBusiness source, PardakhtBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.IsTemp = source.IsTemp;
			PardakhtBusiness pardakhtBusiness = destination;
			pardakhtBusiness.SetId2(await source.GetId2Async(token, tranName), token);
			destination.MoeinGuid = source.MoeinGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
			destination.UserGuid = source.UserGuid;
			destination.Ip = source.Ip;
			destination.IsRead = source.IsRead;
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
			destination.ServerStatus = source.ServerStatus;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.DateMoaser = source.DateMoaser;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.Bank != null)
			{
				foreach (PardakhtBankBusiness item in source.Bank)
				{
					PardakhtBankBusiness destination2 = new PardakhtBankBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtBankMapper.Instance.MapAsync(contextGuid, item, destination2, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.CheckM != null)
			{
				foreach (PardakhtCheckMBusiness item2 in source.CheckM)
				{
					PardakhtCheckMBusiness destination3 = new PardakhtCheckMBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtCheckMMapper.Instance.MapAsync(contextGuid, item2, destination3, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.CheckSh != null)
			{
				foreach (PardakhtCheckShBusiness item3 in source.CheckSh)
				{
					PardakhtCheckShBusiness destination4 = new PardakhtCheckShBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtCheckShMapper.Instance.MapAsync(contextGuid, item3, destination4, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.Naghd != null)
			{
				foreach (PardakhtNaghdBusiness item4 in source.Naghd)
				{
					PardakhtNaghdBusiness destination5 = new PardakhtNaghdBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtNaghdMapper.Instance.MapAsync(contextGuid, item4, destination5, tranName), EnSystem.Mapper, token);
				}
			}
			if (source.Enteghal != null)
			{
				foreach (Sanad_DetBusiness item5 in source.Enteghal)
				{
					Sanad_DetBusiness destination6 = new Sanad_DetBusiness(contextGuid, EnSystem.None, item5.AsGuid, Document.Pardakht, destination.NextOrdinal(token))
					{
						MasterGuid = destination.Guid
					};
					destination.AddToDetList(await SanadDetMapper.Instance.MapAsync(contextGuid, item5, destination6, tranName), EnSystem.Mapper, token);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtMapper.cs", 180);
			throw;
		}
	}

	public async Task<PardakhtBusiness> MapAsync(string contextGuid, WebPardakht source, PardakhtBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.SetAfterAccount(source.AfterAccount);
			destination.SetBeforeAccount(source.BeforeAccount);
			destination.IsTemp = source.IsTemp;
			destination.SetId2(source.Id2, token);
			destination.MoeinGuid = source.MoeinGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = (source.DateUtc.HasValue ? source.DateUtc.Value.ToLocalTime() : source.Date);
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.HddSerial = source.HddSerial;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
			destination.UserGuid = source.CashierGuid;
			destination.Ip = source.Ip;
			destination.IsRead = source.IsRead;
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
			destination.ServerStatus = source.ServerStatus;
			destination.TransferDate = source.TransferDate;
			destination.StatusId = source.StatusId;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.DateMoaser = source.DateMoaser;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			if (source.Bank != null)
			{
				foreach (WebPardakhtBank item in source.Bank)
				{
					PardakhtBankBusiness destination2 = new PardakhtBankBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					PardakhtBusiness pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtBankMapper.Instance.MapAsync(contextGuid, item, destination2, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.CheckM != null)
			{
				foreach (WebPardakhtCheckM item2 in source.CheckM)
				{
					PardakhtCheckMBusiness destination3 = new PardakhtCheckMBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					PardakhtBusiness pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtCheckMMapper.Instance.MapAsync(contextGuid, item2, destination3, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.CheckSh != null)
			{
				foreach (WebPardakhtCheckSh item3 in source.CheckSh)
				{
					PardakhtCheckShBusiness destination4 = new PardakhtCheckShBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					PardakhtBusiness pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtCheckShMapper.Instance.MapAsync(contextGuid, item3, destination4, tranName, token), EnSystem.Mapper, token);
				}
			}
			if (source.Naghd != null)
			{
				foreach (WebPardakhtNaghd item4 in source.Naghd)
				{
					PardakhtNaghdBusiness destination5 = new PardakhtNaghdBusiness(contextGuid)
					{
						MasterGuid = destination.Guid
					};
					PardakhtBusiness pardakhtBusiness = destination;
					pardakhtBusiness.AddToDetList(await PardakhtNaghdMapper.Instance.MapAsync(contextGuid, item4, destination5, tranName), EnSystem.Mapper, token);
				}
			}
			if (source.Enteghal != null)
			{
				foreach (WebSanad_Det item5 in source.Enteghal)
				{
					Sanad_DetBusiness destination6 = new Sanad_DetBusiness(contextGuid, EnSystem.None, item5.AsGuid, Document.Pardakht, destination.NextOrdinal(token))
					{
						MasterGuid = destination.Guid
					};
					destination.AddToDetList(await SanadDetMapper.Instance.MapAsync(contextGuid, item5, destination6, tranName), EnSystem.Mapper, token);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtMapper.cs", 276);
			throw;
		}
	}

	public Task<WebPaymentCompact> MapAsync(string contextGuid, WebPaymentCompactForDisplay source, WebPaymentCompact destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPaymentCompact();
			}
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.Date = source.Date;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.CashierGuid = source.CashierGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.Id2 = source.Id2;
			destination.CashierFullName = source.CashierFullName;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Description = source.Description;
			destination.id = source.id;
			destination.Source = source.Source;
			destination.Sum = source.Sum;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.ServerStatus = source.ServerStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtMapper.cs", 313);
			throw;
		}
	}

	public Task<WebPaymentCompactForDisplay> MapAsync(string contextGuid, WebPaymentCompact source, WebPaymentCompactForDisplay destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPaymentCompactForDisplay();
			}
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.Date = source.Date;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Guid = source.Guid;
			destination.CashierGuid = source.CashierGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadNo = source.SanadNo;
			destination.Id2 = source.Id2;
			destination.CashierFullName = source.CashierFullName;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.Description = source.Description;
			destination.id = source.id;
			destination.Source = source.Source;
			destination.Sum = source.Sum;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.ServerStatus = source.ServerStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtMapper.cs", 350);
			throw;
		}
	}

	public Task<List<WebPardakht>> MapListAsync(string contextGuid, IEnumerable<PardakhtBusiness> source, List<WebPardakht> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PardakhtBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtBusiness> source, List<PardakhtBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PardakhtBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPardakht> source, List<PardakhtBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebPaymentCompact>> MapListAsync(string contextGuid, IEnumerable<WebPaymentCompactForDisplay> source, List<WebPaymentCompact> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebPaymentCompactForDisplay>> MapListAsync(string contextGuid, IEnumerable<WebPaymentCompact> source, List<WebPaymentCompactForDisplay> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
