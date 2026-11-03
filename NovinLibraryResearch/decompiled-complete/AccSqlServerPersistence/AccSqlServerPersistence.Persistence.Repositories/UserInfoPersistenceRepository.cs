using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class UserInfoPersistenceRepository : PersistenceNoGuidRepository<UserInformationBusiness, UserInformation>, IUserInformationRepository, INoGuidRepository<UserInformationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<UserInformationBusiness, UserInformation>, IItemAsyncMapper<UserInformation, UserInformationBusiness>
	{
		public Task<UserInformation> MapAsync(string contextGuid, UserInformationBusiness source, UserInformation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
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
						destination = new UserInformation();
					}
					destination.Username = source.UserName;
					destination.Password = "";
					destination.UserData = "";
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 214);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<UserInformationBusiness> MapAsync(string contextGuid, UserInformation source, UserInformationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
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
						destination = new UserInformationBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.UserName = source.Username;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 238);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<UserInformation>> MapListAsync(string contextGuid, IEnumerable<UserInformationBusiness> source, List<UserInformation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<UserInformationBusiness>> MapListAsync(string contextGuid, IEnumerable<UserInformation> source, List<UserInformationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<UserInformation, UserInformationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<UserInformationBusiness, UserInformation> MapToDatabase => Mapper;

	public UserInfoPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public async Task<UserInformationBusiness> GetAsync(CancellationToken token, string tranName)
	{
		int line = 0;
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 27));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			UserInformationBusiness result;
			try
			{
				_ = 1;
				try
				{
					UserInformation userInformation = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserInformation>((IQueryable<UserInformation>)c.UserInformation, token);
					line = 1;
					if (userInformation != null)
					{
						UserInformationBusiness userInformationBusiness = (userInformation?.UserData ?? "").FromJson<UserInformationBusiness>("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 35);
						line = 2;
						if (userInformationBusiness == null)
						{
							userInformationBusiness = new UserInformationBusiness(base.ContextGuid);
						}
						line = 3;
						userInformationBusiness.ContextGuid = base.ContextGuid;
						result = userInformationBusiness;
					}
					else
					{
						result = null;
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, $"The Error Line Is :{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<UserInformationBusiness> GetByCredentialAsync(string username, string password, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 61));
			UserInformationBusiness result;
			try
			{
				_ = 1;
				try
				{
					UserInformation userInformation = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserInformation>((IQueryable<UserInformation>)c.UserInformation, (Expression<Func<UserInformation, bool>>)((UserInformation x) => x.Username == username && x.Password == password), token);
					result = ((userInformation != null) ? userInformation.UserData.FromJson<UserInformationBusiness>("GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 66) : new UserInformationBusiness(base.ContextGuid));
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 73);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByCredentialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAsync(UserInformationBusiness item, string username, string password, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			try
			{
				Global_ModelContext obj = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 91));
				UserInformation userInformation = new UserInformation
				{
					UserData = item.ToString(),
					Username = username,
					Password = password
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.AddOrUpdateAsync(userInformation, userInformation.Username), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 98);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 102);
				ret.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 103);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(UserInformationBusiness item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 118);
				token.ThrowIfCancellationRequested();
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 121);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 124);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 125);
			}
			return ret;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 141);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(UserInformationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 152));
				UserInformation userInformation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(userInformation, userInformation.Username), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 154);
				return ret;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 160);
				throw;
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<UserInformationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs"))
		{
			try
			{
				foreach (UserInformationBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 177);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 181);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\UserInfoPersistenceRepository.cs", 182);
			}
			return ret;
		}
	}
}
