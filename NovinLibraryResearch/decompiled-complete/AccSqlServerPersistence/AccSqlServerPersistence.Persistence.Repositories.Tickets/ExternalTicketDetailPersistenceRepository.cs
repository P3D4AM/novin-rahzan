using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Tickets;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.Ticket;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

internal class ExternalTicketDetailPersistenceRepository : ITicketDetRepository, IRepository<TicketDetBusiness>, INoGuidRepository<TicketDetBusiness>, IDisposable
{
	private ContextHolder _dbContext;

	public WebContextHolder Context => _dbContext.Context;

	public string ContextGuid => _dbContext.Context.Guid;

	public ExternalTicketDetailPersistenceRepository(ContextHolder dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfoWithValue<List<WebTicketReport>> returnedSaveFuncInfoWithValue = await TicketDetailClient.GetAllAsync(filter);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			return await TicketMapper.Instance.MapListAsync(ContextGuid, returnedSaveFuncInfoWithValue.value, new List<TicketReportBusiness>(), tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketDetailPersistenceRepository.cs", 35);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TicketDetBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			WebTicketDet ticket = await TicketDetMapper.Instance.MapAsync(ContextGuid, item, new WebTicketDet(), tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await TicketDetailClient.SaveAsync(ticket), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketDetailPersistenceRepository.cs", 45);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketDetailPersistenceRepository.cs", 49);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketDetailPersistenceRepository.cs", 50);
		}
		return res;
	}

	public Task<IEnumerable<TicketDetBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<TicketDetBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public TicketDetBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<TicketDetBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<TicketDetBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TicketDetBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<TicketDetBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}
