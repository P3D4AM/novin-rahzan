using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.DepartmentApiModel.Ticket;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

internal class ExternalTicketPersistenceRepository : ITicketRepository, IRepository<TicketBusiness>, INoGuidRepository<TicketBusiness>, IDisposable
{
	private ContextHolder _dbContext;

	public WebContextHolder Context => _dbContext.Context;

	public string ContextGuid => _dbContext.Context.Guid;

	public ExternalTicketPersistenceRepository(ContextHolder dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await TicketClient.ChangeStatusAsync(guid, statusId), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 31);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 35);
			res.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 36);
		}
		return res;
	}

	public async Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfoWithValue<List<WebTicketReport>> returnedSaveFuncInfoWithValue = await TicketClient.GetAllAsync(filter);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			return await TicketMapper.Instance.MapListAsync(ContextGuid, returnedSaveFuncInfoWithValue.value, new List<TicketReportBusiness>(), tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 50);
			throw;
		}
	}

	public async Task<TicketBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfoWithValue<WebTicket> returnedSaveFuncInfoWithValue = await TicketClient.GetAsync(guid);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			return await TicketMapper.Instance.MapAsync(ContextGuid, returnedSaveFuncInfoWithValue.value, new TicketBusiness(ContextGuid), tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 64);
			throw;
		}
	}

	public async Task<TicketBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfoWithValue<WebTicket> returnedSaveFuncInfoWithValue = await TicketClient.GetByNumberAsync(number);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			return await TicketMapper.Instance.MapAsync(ContextGuid, returnedSaveFuncInfoWithValue.value, new TicketBusiness(ContextGuid), tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 78);
			throw;
		}
	}

	public Task<TicketBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<List<TicketTitleSuggest>> GetSuggestTitelsAsync(CancellationToken token, string tranName)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<List<TicketTitleSuggest>> obj = await TicketClient.GetAllSuggestTitelsAsync();
			obj.throwExceptionIfError();
			return obj.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 93);
			throw;
		}
	}

	public async Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<int> obj = await TicketClient.GetNextNumberAsync();
			obj.throwExceptionIfError();
			return obj.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 107);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TicketBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			WebTicket ticket = await TicketMapper.Instance.MapAsync(ContextGuid, item, new WebTicket(), tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await TicketClient.SaveAsync(ticket), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 117);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 121);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Tickets\\ExternalTicketPersistenceRepository.cs", 122);
		}
		return res;
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public TicketBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<TicketBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<TicketBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TicketBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
