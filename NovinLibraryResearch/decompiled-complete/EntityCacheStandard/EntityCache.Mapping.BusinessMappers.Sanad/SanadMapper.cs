using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Sanad;

public class SanadMapper : IItemAsyncMapper<SanadBusiness, WebSanad>, IItemAsyncMapper<SanadBusiness, SanadBusiness>, IItemAsyncMapper<WebSanad, SanadBusiness>, IItemAsyncMapper<SanadBusiness, SanadReportBusiness>
{
	public static SanadMapper Instance { get; private set; } = new SanadMapper();


	public async Task<WebSanad> MapAsync(string contextGuid, SanadBusiness source, WebSanad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebSanad();
			}
			destination.Guid = source.Guid;
			destination.Description = source.Description;
			destination.Status = true;
			destination.DateM = source.DateM;
			destination.SanadSt = source.SanadSt;
			destination.Modified = DateTime.Now;
			WebSanad webSanad = destination;
			webSanad.SetSanadNo(await source.GetSanadNoAsync(token, tranName));
			destination.UserGuid = source.UserGuid;
			destination.ChangeStatus = source.ChangeStatus;
			destination.SanadType = source.SanadType;
			webSanad = destination;
			webSanad.Lst = await SanadDetMapper.Instance.MapListAsync(contextGuid, source.ListSanad_Det, new List<WebSanad_Det>(), tranName, token);
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.StatusId = source.StatusId;
			destination.IsTemp = source.IsTemp;
			destination.MetaData = source.MetaData;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadMapper.cs", 59);
			throw;
		}
	}

	public Task<List<WebSanad>> MapListAsync(string contextGuid, IEnumerable<SanadBusiness> source, List<WebSanad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<SanadBusiness> MapAsync(string contextGuid, SanadBusiness source, SanadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new SanadBusiness(contextGuid, source.OldSanadNumber);
			}
			destination.ContextGuid = contextGuid;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.UserGuid = source.UserGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id = source.Id;
			destination.ChangeStatus = source.ChangeStatus;
			destination.SanadType = source.SanadType;
			if ((source?.ListSanad_Det?.Any()).GetValueOrDefault())
			{
				await destination.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(contextGuid, source.ListSanad_Det, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.Mapper, appendCurrentItems: false, token);
			}
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.StatusId = source.StatusId;
			destination.IsTemp = source.IsTemp;
			destination.MetaData = source.MetaData;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadMapper.cs", 104);
			throw;
		}
	}

	public Task<List<SanadBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadBusiness> source, List<SanadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<SanadBusiness> MapAsync(string contextGuid, WebSanad source, SanadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new SanadBusiness(contextGuid, 0);
			}
			destination.ContextGuid = contextGuid;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.UserGuid = source.UserGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ChangeStatus = source.ChangeStatus;
			destination.SanadType = source.SanadType;
			SanadBusiness sanadBusiness = destination;
			await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(contextGuid, source.Lst, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.Mapper);
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.StatusId = source.StatusId;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadMapper.cs", 142);
			throw;
		}
	}

	public Task<List<SanadBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanad> source, List<SanadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<SanadReportBusiness> MapAsync(string contextGuid, SanadBusiness source, SanadReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadReportBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.DateM = source.DateM;
			destination.DateSh = source.DateM.MiladiToCalture();
			destination.Description = source.Description;
			destination.SanadNo = source.SanadNo;
			destination.SanadId = source.Id;
			destination.SanadSt = source.SanadSt;
			destination.SanadType = source.SanadType;
			destination.UserGuid = source.UserGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Sum = (source?.ListSanad_Det?.Sum((Sanad_DetBusiness q) => q.Debit)).GetValueOrDefault();
			destination.StatusId = source.StatusId;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadMapper.cs", 175);
			throw;
		}
	}

	public Task<List<SanadReportBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadBusiness> source, List<SanadReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
