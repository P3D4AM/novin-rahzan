using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CustomerCacheRepository : DetailCacheRepository<CusBusiness, CusCompactBusiness>, ICustomerRepository, IDetailRepository<CusBusiness>, IEditableDelivery, IDisposable
{
	private new ICustomerRepository DbRepository;

	public override IItemAsyncMapper<CusBusiness, CusBusiness> MapperTtoT => CustomerMapper.Instance;

	public override IItemAsyncMapper<CusBusiness, CusCompactBusiness> MapperTtoTCompact => CustomerMapper.Instance;

	public CustomerCacheRepository(ICompactCacheRepository<CusCompactBusiness> compactRep, IDetailRepository<CusBusiness> dbRepository, int CachingTimeSeconds)
		: base(compactRep, dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = (ICustomerRepository)dbRepository;
	}

	public async Task<CusBusiness> GetByEmailAsync(string email, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				CusBusiness cusBusiness = base.Entities.GetAll().SingleOrDefault((CusBusiness x) => x.Email == email);
				line = 1;
				if (cusBusiness != null)
				{
					return cusBusiness;
				}
				line = 2;
				CusBusiness res = await DbRepository.GetByEmailAsync(email, token, tranName);
				line = 3;
				if (res != null)
				{
					line = 4;
					await base.Entities.UpdateTimeAsync(res.Guid, res, tranName);
				}
				line = 5;
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + string.Format("ClsCacheIsNullOrDisposed:{0}\r\n", ClsCache.GetCache(base.Context?.Guid ?? "")?.IsDisposed) + "Context:" + Json.ToStringJson(base.Context, indented: true), "GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 52);
				return null;
			}
		}
	}

	public async Task<CusBusiness> GetAsync(long code, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.Context.Guid, EnSystem.None, code, tranName, default(CancellationToken), "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs");
				if (tafsilBusiness == null)
				{
					return null;
				}
				if (base.Entities.TryGetValue(tafsilBusiness.Guid, out var ret))
				{
					return ret;
				}
				CusBusiness res = await DbRepository.GetAsync(tafsilBusiness.Guid, token, tranName);
				if (res != null)
				{
					await base.Entities.UpdateTimeAsync(res.Guid, res, tranName);
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 76);
				throw;
			}
		}
	}

	public Task<CusBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetFromDatabaseAsync(guid, token, tranName);
	}

	public async Task<string> GetNextCodeAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs"))
		{
			try
			{
				return "0";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 96);
				throw;
			}
		}
	}

	public async Task<CusBusiness> GetByNameAsync(string firstName, string lastName, Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		try
		{
			using (new JaegerMonitor("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs"))
			{
				CusBusiness cusBusiness = base.Entities.GetAll().FirstOrDefault((CusBusiness x) => x.Name == firstName && x.Family == lastName && x.Guid != guid);
				if (cusBusiness != null)
				{
					return cusBusiness;
				}
				CusBusiness res = await DbRepository.GetByNameAsync(firstName, lastName, guid, token, tranName);
				if (res != null)
				{
					await base.Entities.UpdateTimeAsync(res.Guid, res, tranName);
				}
				return res;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 126);
			throw;
		}
	}

	public async Task<bool> CheckUserNameAsync(Guid guid, string userName, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await DbRepository.CheckUserNameAsync(guid, userName, token, tranName);
	}

	public async Task<CusBusiness> GetByUserNameAsync(string userName, string pass, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				CusBusiness cusBusiness = base.Entities.GetAll().SingleOrDefault((CusBusiness x) => x.User == userName && x.Password == pass);
				if (cusBusiness != null)
				{
					return cusBusiness;
				}
				CusBusiness res = await DbRepository.GetByUserNameAsync(userName, pass, token, tranName);
				if (res != null)
				{
					await base.Entities.UpdateTimeAsync(res.Guid, res, tranName);
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 152);
				throw;
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateGroupAsync(IEnumerable<Guid> customerGuids, Guid groupGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.UpdateGroupAsync(customerGuids, groupGuid, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCacheRepository.cs", 173);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		base.Entities?.ClearAll();
	}

	public Task<ReturnedSaveFuncInfo> ChangeChequeBargashtiValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName)
	{
		return DbRepository.ChangeChequeBargashtiValidationAsync(lst, state, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeOrderTimeValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName)
	{
		return DbRepository.ChangeOrderTimeValidationAsync(lst, state, token, tranName);
	}

	public Task<CusBusiness> GetByNationalCodeAsync(string nationalCode, CancellationToken token, string tranName)
	{
		return DbRepository.GetByNationalCodeAsync(nationalCode, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateSendToServerDateAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(EnPriceType priceType, Guid roleGuid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateRoleGuidAsync(priceType, roleGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient serverStatus, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateServerStatusAsync(guid, serverStatus, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebCus> items, CancellationToken token, string tranName)
	{
		return DbRepository.SaveRangeAsync(items, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(List<Guid> customerList, Guid roleGuid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateRoleGuidAsync(customerList, roleGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdatePhoneNumbersAsync(CancellationToken token, string tranName)
	{
		return DbRepository.UpdatePhoneNumbersAsync(token, tranName);
	}

	public Task<List<Guid>> GetCustomerGuidListInRoleAsync(Guid roleGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetCustomerGuidListInRoleAsync(roleGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool value, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateSpecialAsync(groupGuid, value, token, tranName);
	}
}
