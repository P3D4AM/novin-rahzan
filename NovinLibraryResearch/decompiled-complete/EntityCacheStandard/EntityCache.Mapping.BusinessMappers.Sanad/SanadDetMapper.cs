using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Sanad;

public class SanadDetMapper : IItemAsyncMapper<Sanad_DetBusiness, WebSanad_Det>, IItemAsyncMapper<Sanad_DetBusiness, Sanad_DetBusiness>, IItemAsyncMapper<WebSanad_Det, Sanad_DetBusiness>, IItemAsyncMapper<WebSanad_Det, GardeshTafsilBusiness>
{
	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	public static SanadDetMapper Instance { get; private set; } = new SanadDetMapper();


	public Task<WebSanad_Det> MapAsync(string contextGuid, Sanad_DetBusiness source, WebSanad_Det destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSanad_Det();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Qt = source.Qt;
			destination.RowDesc = source.RowDesc ?? "";
			destination.Serial = source.Serial;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.DateSarresid = source.DateSarresid;
			destination.Ordinal = source.Ordinal;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			destination.TafsilGuid4 = source.TafsilGuid4;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadDetMapper.cs", 52);
			throw;
		}
	}

	public Task<List<WebSanad_Det>> MapListAsync(string contextGuid, IEnumerable<Sanad_DetBusiness> source, List<WebSanad_Det> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<Sanad_DetBusiness> MapAsync(string contextGuid, Sanad_DetBusiness source, Sanad_DetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new Sanad_DetBusiness(contextGuid, EnSystem.Mapper, source.AsGuid, source.AsType, source.Ordinal);
			}
			destination.ContextGuid = contextGuid;
			destination.MasterSanadItem = source.MasterSanadItem;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetMoeinGuidAsync(source.MoeinGuid, token, tranName);
			await destination.SetTafsilGuidAsync(source.TafsilGuid, tranName, token);
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Qt = source.Qt;
			destination.RowDesc = source.RowDesc ?? "";
			destination.MasterGuid = source.MasterGuid;
			destination.Serial = source.Serial;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.DebitString = source.DebitString;
			destination.CreditString = source.CreditString;
			destination.AsType = source.AsType;
			destination.DateSarresid = source.DateSarresid;
			destination.SetOrdinalChange(EnSystem.Mapper, source.Ordinal, throwOnOrdinalChanged: false);
			await destination.SetTafsil2GuidAsync(source.TafsilGuid2, tranName, token);
			await destination.SetTafsil3GuidAsync(source.TafsilGuid3, tranName, token);
			await destination.SetTafsil4GuidAsync(source.TafsilGuid4, tranName, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadDetMapper.cs", 95);
			throw;
		}
	}

	public Task<List<Sanad_DetBusiness>> MapListAsync(string contextGuid, IEnumerable<Sanad_DetBusiness> source, List<Sanad_DetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<Sanad_DetBusiness> MapAsync(string contextGuid, WebSanad_Det source, Sanad_DetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		await _semaphore.WaitAsync();
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new Sanad_DetBusiness(contextGuid, EnSystem.Mapper, source.AsGuid, source.AsType, source.Ordinal);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetMoeinGuidAsync(source.MoeinGuid, token, tranName);
			await destination.SetTafsilGuidAsync(source.TafsilGuid, tranName, token);
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Qt = source.Qt;
			destination.RowDesc = source.RowDesc ?? "";
			destination.Serial = source.Serial;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.DateSarresid = source.DateSarresid;
			destination.SetOrdinalChange(EnSystem.Mapper, source.Ordinal, throwOnOrdinalChanged: false);
			await destination.SetTafsil2GuidAsync(source.TafsilGuid2, tranName, token);
			await destination.SetTafsil3GuidAsync(source.TafsilGuid3, tranName, token);
			await destination.SetTafsil4GuidAsync(source.TafsilGuid4, tranName, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadDetMapper.cs", 135);
			throw;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public Task<List<Sanad_DetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanad_Det> source, List<Sanad_DetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<GardeshTafsilBusiness> MapAsync(string contextGuid, WebSanad_Det source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshTafsilBusiness(contextGuid);
			}
			destination.AsType = (short)source.AsType;
			destination.Date = source.DateM;
			destination.Description = source.RowDesc ?? "";
			destination.AsGuid = source.AsGuid;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNo;
			destination.SanadSt = (int)source.SanadST;
			destination.Serial = source.Serial;
			destination.QT = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.UGuid = source.CashierGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadDetMapper.cs", 177);
			throw;
		}
	}

	public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanad_Det> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
