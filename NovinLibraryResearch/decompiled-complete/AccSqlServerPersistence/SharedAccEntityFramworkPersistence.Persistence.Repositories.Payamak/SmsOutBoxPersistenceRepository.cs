using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using ClientHub.AccountingHubs;
using EntityCache.Business.Accounting.Payamak;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.BusinessMappers.Payamak;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Payamak;

internal class SmsOutBoxPersistenceRepository : ISmsOutBoxRepository, IRepository<SmsOutBoxBusiness>, INoGuidRepository<SmsOutBoxBusiness>, IDisposable
{
	protected ContextHolder dbContext;

	private bool _isDisposed;

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public SmsOutBoxPersistenceRepository(ContextHolder context)
	{
		dbContext = context;
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
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 40);
		}
	}

	public SmsOutBoxBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<SmsOutBoxBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<SmsOutBoxBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<SmsOutBoxBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<SmsOutBoxBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
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

	public async Task<ReturnedSaveFuncInfo> SaveAsync(SmsOutBoxBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((item?.AccountId ?? 0) <= 0)
			{
				return await res.AddErrorAsync("Sms Account Not Found", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 56);
			}
			PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			if (instance == null)
			{
				return await res.AddErrorAsync("Sms Instance is Null", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 58);
			}
			BaseSmsNetAccount smsNetAccount = (await instance.SmsNetAccount.GetByIdAsync(item.AccountId))?.value;
			if (smsNetAccount == null)
			{
				throw new NullReferenceException("smsNetAccount");
			}
			BaseSmsOutBox baseSmsOutBox = await SmsOutBoxMapper.Instance.MapAsync(ContextGuid, item, new BaseSmsOutBox(), tranName, token);
			baseSmsOutBox.SenderNumber = smsNetAccount.SenderNumber;
			await instance.SmsOutBox.SaveAsync(baseSmsOutBox, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 70);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 71);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<SmsOutBoxBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (SmsOutBoxBusiness item in items)
			{
				if (item != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 84);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 89);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Payamak\\SmsOutBoxPersistenceRepository.cs", 90);
		}
		return res;
	}
}
