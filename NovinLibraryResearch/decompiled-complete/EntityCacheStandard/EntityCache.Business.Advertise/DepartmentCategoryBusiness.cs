using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class DepartmentCategoryBusiness : Serializable<DepartmentCategoryBusiness>, IDepartmentCategory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid DepartmentGuid { get; set; }

	public string FirstLevel { get; set; }

	public string SecondLevel { get; set; }

	public string ThirdLevel { get; set; }

	public string ContextGuid { get; set; }

	public DepartmentCategoryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<DepartmentCategoryBusiness> list, Guid depGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (DepartmentCategoryBusiness item in list)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IDepartmentCategory.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\DepartmentCategoryBusiness.cs", 36);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\DepartmentCategoryBusiness.cs", 40);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\DepartmentCategoryBusiness.cs", 41);
		}
		return ret;
	}

	public static Task<IEnumerable<DepartmentCategoryBusiness>> GetAllAsync(string contextGuid, Guid depGuid, bool? status = true, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepartmentCategory.GetAllAsync(depGuid, status, tranName);
	}

	public static async Task<DepartmentCategoryBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IDepartmentCategory.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<DepartmentCategoryBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepartmentCategory.GetAllAsync(status, token, tranName);
	}
}
