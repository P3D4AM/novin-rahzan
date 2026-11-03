using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TozieList;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.TozieList;

public class TozieListMapper : IItemAsyncMapper<TozieListBusiness, WebTozieList>, IItemAsyncMapper<TozieListBusiness, TozieListBusiness>, IItemAsyncMapper<WebTozieList, TozieListBusiness>
{
	public static TozieListMapper Instance { get; private set; } = new TozieListMapper();


	public Task<WebTozieList> MapAsync(string contextGuid, TozieListBusiness source, WebTozieList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTozieList();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Number = source.Number;
			destination.DateM = source.DateM;
			destination.DriverName = source.DriverName;
			destination.DriverMobile1 = source.DriverMobile1;
			destination.DriverMobile2 = source.DriverMobile2;
			destination.CarNumber = source.CarNumber;
			destination.Description = source.Description;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListMapper.cs", 42);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<TozieListBusiness> MapAsync(string contextGuid, TozieListBusiness source, TozieListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TozieListBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Number = source.Number;
			destination.DateM = source.DateM;
			destination.DriverName = source.DriverName;
			destination.DriverMobile1 = source.DriverMobile1;
			destination.DriverMobile2 = source.DriverMobile2;
			destination.CarNumber = source.CarNumber;
			destination.Description = source.Description;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
			if (source.DetailList?.Any() ?? false)
			{
				await destination.AddRangeToListSanadAsync(await TozieListDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<TozieListDetailBusiness>(), tranName, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListMapper.cs", 78);
			throw;
		}
		return destination;
	}

	public Task<TozieListBusiness> MapAsync(string contextGuid, WebTozieList source, TozieListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TozieListBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Number = source.Number;
			destination.DateM = source.DateM;
			destination.DriverName = source.DriverName;
			destination.DriverMobile1 = source.DriverMobile1;
			destination.DriverMobile2 = source.DriverMobile2;
			destination.CarNumber = source.CarNumber;
			destination.Description = source.Description;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListMapper.cs", 108);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebTozieList>> MapListAsync(string contextGuid, IEnumerable<TozieListBusiness> source, List<WebTozieList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TozieListBusiness>> MapListAsync(string contextGuid, IEnumerable<TozieListBusiness> source, List<TozieListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TozieListBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTozieList> source, List<TozieListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
