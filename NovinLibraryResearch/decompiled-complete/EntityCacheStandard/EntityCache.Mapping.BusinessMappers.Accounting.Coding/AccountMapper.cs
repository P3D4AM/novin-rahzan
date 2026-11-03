using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Coding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Coding;

public class AccountMapper : IItemAsyncMapper<AccountBusiness, WebAccount>, IItemAsyncMapper<AccountBusiness, AccountBusiness>, IItemAsyncMapper<WebAccount, AccountBusiness>
{
	private static SemaphoreSlim _getGardeshHesabSemaphore = new SemaphoreSlim(1, 5);

	public static AccountMapper Instance { get; private set; } = new AccountMapper();


	public async Task<WebAccount> MapAsync(string contextGuid, AccountBusiness source, WebAccount destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAccount();
			}
			destination.Guid = source.Guid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Account = source.Account;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> returnedSaveFuncInfoWithValue = null;
			GardeshTafsiliFilter filter = new GardeshTafsiliFilter
			{
				TafsilGuid = source.TafsilGuid,
				MoeinGuids = new List<Guid> { source.MoeinGuid }
			};
			NPSMS_WebServiceSettings nPSMS_WebServiceSettings = await LocalSettingBusiness.GetWebServiceSettingsAsync(contextGuid);
			filter.MaxCount = (nPSMS_WebServiceSettings.SendUnlimitedGardesh ? null : new int?(nPSMS_WebServiceSettings.NPACC_GardeshCount));
			await _getGardeshHesabSemaphore.WaitAsync();
			bool semaphoreHasBeenTaken = true;
			try
			{
				returnedSaveFuncInfoWithValue = await GardeshTafsilBusiness.GetAllAsync(contextGuid, filter, EnSystem.Mapper, tranName, token);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_getGardeshHesabSemaphore.Release();
				}
			}
			if (!returnedSaveFuncInfoWithValue.HasError && returnedSaveFuncInfoWithValue.value != null)
			{
				byte[] data = Json.ToBytesJson(await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, returnedSaveFuncInfoWithValue.value, new List<WebGardeshTafsiliCompact>(), tranName));
				WebAccount webAccount = destination;
				webAccount.GardeshHesab = (await GZip.Compress(data))?.ToList();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\AccountMapper.cs", 69);
			throw;
		}
		return destination;
	}

	public Task<AccountBusiness> MapAsync(string contextGuid, AccountBusiness source, AccountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AccountBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Account = source.Account;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\AccountMapper.cs", 101);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<AccountBusiness> MapAsync(string contextGuid, WebAccount source, AccountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AccountBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Account = source.Account;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\AccountMapper.cs", 132);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebAccount>> MapListAsync(string contextGuid, IEnumerable<AccountBusiness> source, List<WebAccount> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AccountBusiness>> MapListAsync(string contextGuid, IEnumerable<AccountBusiness> source, List<AccountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AccountBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAccount> source, List<AccountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
