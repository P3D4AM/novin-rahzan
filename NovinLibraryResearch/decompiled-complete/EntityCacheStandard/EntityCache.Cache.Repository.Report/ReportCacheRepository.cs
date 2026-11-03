using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Report;
using EntityCache.Core.Repository.Report;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Filters;
using PacketParser.Records;
using PacketParser.Records.Kardex;
using PacketParser.Records.OccasionCalendar;

namespace EntityCache.Cache.Repository.Report;

public class ReportCacheRepository : IReportRepository, IDisposable
{
	private bool _isDisposed;

	private IReportRepository _dbRepository;

	public bool IsDisposed => _isDisposed;

	public ReportCacheRepository(IReportRepository dbRepository, int CachingTimeSeconds)
	{
		_dbRepository = dbRepository;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_dbRepository?.Dispose();
				_dbRepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Report\\ReportCacheRepository.cs", 41);
		}
	}

	public Task<IEnumerable<BedehiTalabModel>> GetBedehiTalabReportAsync(EnBedehiTalabType type, DateTime? date1, DateTime? date2, short status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetBedehiTalabReportAsync(type, date1, date2, status, token, tranName);
	}

	public Task<List<TarazHesabBusiness>> GetAllTarazHesabAsync(CancellationToken token, TarazHesabFilter filter, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllTarazHesabAsync(token, filter, tranName);
	}

	public Task<IEnumerable<AccountTreeModel>> GetAllAccountTreeAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAccountTreeAsync(token, tranName);
	}

	public Task<PayamakManage> GetPayamakManageAsync(DateTime d1, DateTime d2, CancellationToken token, string tranName)
	{
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			return _dbRepository.GetPayamakManageAsync(d1, d2, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPayamakManageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Report\\ReportCacheRepository.cs", 67);
			throw;
		}
	}

	public Task<IEnumerable<SumKolBusiness>> GetSumKolListAsync(SumKolFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetSumKolListAsync(filter, token, tranName);
	}

	public Task<WebDepartmentEntitySummery> GetDepartmentEntitySummeryAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetDepartmentEntitySummeryAsync(token, tranName);
	}

	public Task<List<OccasionCalendarRecord>> GetOccasionCalendarListAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetOccasionCalendarListAsync(token, tranName);
	}

	public Task<List<ServerStatusReportModel>> GetServerStatusAsync(ENSource source, List<ServerStatusForClient> serverStatuses, CancellationToken token, string tranName)
	{
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			return _dbRepository.GetServerStatusAsync(source, serverStatuses, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Report\\ReportCacheRepository.cs", 83);
			throw;
		}
	}

	public Task<IEnumerable<KardexAndAccountConflictRecord>> GetAllKardexAndAccountConflictAsync(Guid moeinGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllKardexAndAccountConflictAsync(moeinGuid, token, tranName);
	}

	public Task<IEnumerable<DocumentRoundMismatchRecord>> GetAllDocumentRoundMismatchAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllDocumentRoundMismatchAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<ServerStatusReportModel> list, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateServerStatusAsync(list, token, tranName);
	}
}
