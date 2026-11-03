using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class UserInfoCacheRepository : CacheRepositoryNoGuiId<UserInformationBusiness, Credential>, IUserInformationRepository, INoGuidRepository<UserInformationBusiness>, IDisposable
{
	private bool _isDisposed;

	private UserInformationBusiness _UserInfo;

	private IUserInformationRepository dbRepository;

	private DateTime lastCacheRefresh;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	protected override TimingCache<Credential, UserInformationBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<UserInformationBusiness, UserInformationBusiness> Mapper => UserInformationMapper.Instance;

	public UserInfoCacheRepository(IUserInformationRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<UserInformationBusiness>)dbRepository, CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs"))
		{
			try
			{
				this.dbRepository = dbRepository ?? throw new ArgumentNullException("dbRepository");
				lastCacheRefresh = DateTime.Now.Subtract(new TimeSpan(10, 0, 0));
				TimingCache = new TimingCache<Credential, UserInformationBusiness>(base.ContextGuid, Mapper, CachingTime);
				GetAsync(default(CancellationToken), "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 45);
				throw;
			}
		}
	}

	public async Task<UserInformationBusiness> GetAsync(CancellationToken token, string traName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (_UserInfo != null)
		{
			return _UserInfo;
		}
		await Locks.DbReadUserInfoSemaphore.WaitAsync();
		try
		{
			Credential cred = new Credential
			{
				Password = "",
				Username = ""
			};
			TimingCache?.TryGetValue(cred, out _UserInfo);
			if (_UserInfo == null)
			{
				_UserInfo = await dbRepository.GetAsync(token, traName);
				if (_UserInfo == null)
				{
					_UserInfo = new UserInformationBusiness(base.ContextGuid);
				}
				if (TimingCache != null)
				{
					await TimingCache.UpdateTimeAsync(cred, _UserInfo, traName);
				}
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 69);
		}
		finally
		{
			Locks.DbReadUserInfoSemaphore.Release();
		}
		return _UserInfo;
	}

	public async Task<UserInformationBusiness> GetByCredentialAsync(string username, string password, CancellationToken token, string traName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				Credential cred = new Credential
				{
					Password = username,
					Username = password
				};
				if (DateTime.Now.Subtract(lastCacheRefresh).TotalMinutes > 3.0 || !TimingCache.TryGetValue(cred, out var ret))
				{
					TimingCache.TryRemove(cred, out var _);
					UserInformationBusiness res = await dbRepository.GetByCredentialAsync(username, password, token, traName);
					if (res == null)
					{
						return new UserInformationBusiness(base.ContextGuid);
					}
					await TimingCache.UpdateTimeAsync(cred, res, traName);
					lastCacheRefresh = DateTime.Now;
					return res;
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 95);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAsync(UserInformationBusiness item, string username, string password, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs"))
		{
			int line = 1;
			if (item.Source != ENSource.Android && item.Source != ENSource.Android_HesabYar && item.Source != ENSource.Android_SefareshGir && item.Source != ENSource.Baje && item.Source != ENSource.Android_SefareshGir)
			{
				_ = item.Source;
				_ = 19;
			}
			try
			{
				line = 2;
				if (username == null)
				{
					username = "";
				}
				line = 5;
				if (password == null)
				{
					password = "";
				}
				line = 10;
				if (string.IsNullOrEmpty(username))
				{
					_UserInfo = item;
				}
				line = 15;
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				Credential key = new Credential
				{
					Username = username,
					Password = password
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.UpdateAsync(item, username, password, tryCount, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 137);
				ret.throwExceptionIfError();
				line = 17;
				await TimingCache.UpdateTimeAsync(key, item, tranName);
				line = 16;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//IsDisposed:{IsDisposed}//DbRepository is null:{dbRepository == null}", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 145);
				ret.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 146);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(UserInformationBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		return SaveAsync(item, token, tryCount, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(UserInformationBusiness item, CancellationToken token, short tryCount, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 1;
		try
		{
			line = 2;
			_UserInfo = item;
			Credential credential = default(Credential);
			credential.Password = "";
			credential.Username = "";
			Credential key = credential;
			line = 3;
			await TimingCache.UpdateTimeAsync(key, item, tranName);
			line = 4;
			await dbRepository.SaveAsync(_UserInfo, token, tryCount, tranName);
			line = 6;
		}
		catch (OperationCanceledException ex)
		{
			WebErrorLog.Instance.StartLog(ex, string.Format("Line:{0} item:{1}", line, item?.ToString() ?? "Null"), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 174);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0} item:{1},ClsCache is disposed:{2}", line, item?.ToString() ?? "Null", ClsCache.GetCache(base.ContextGuid).IsDisposed), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 178);
			ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 179);
		}
		return ret;
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_UserInfo = null;
				dbRepository = null;
				lastCacheRefresh = DateTime.MinValue;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\UserInfoCacheRepository.cs", 197);
		}
	}
}
