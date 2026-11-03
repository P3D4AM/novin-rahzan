using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Cache.Repository.Accounting.Details;

public class ReceptionCheckCacheRepository : MasterCacheRepository<ReceptionCheckBusiness>, IReceptionCheckRepository, IPersistenceMasterRepository<ReceptionCheckBusiness>, IDisposable
{
	private IReceptionCheckRepository DbRepository;

	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public ReceptionCheckCacheRepository(IReceptionCheckRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<ReceptionCheckBusiness>)dbRepository, CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
	}

	public Task<string> NextPoshtNomreAsync(int checkCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.NextPoshtNomreAsync(checkCount, token, tranName);
	}

	public Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(ReceptionChequeFilter filter, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAsync(guid, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Details\\ReceptionCheckCacheRepository.cs", 48);
		}
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllReportAsync(filter, token, tranName);
	}

	public Task<ReceptionCheckBusiness> GetAsync(string number, CancellationToken token, ENUMCheck? status, string tranName)
	{
		return DbRepository.GetAsync(number, token, status, tranName);
	}
}
