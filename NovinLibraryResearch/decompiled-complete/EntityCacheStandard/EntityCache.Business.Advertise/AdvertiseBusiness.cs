using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Settings.Advertise;

namespace EntityCache.Business.Advertise;

public class AdvertiseBusiness : IAdvAdvertise, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _sheypoorCategoryListData;

	private string _divarCategoryListData;

	private string _imageListData;

	private string _titleListData;

	private List<SheypoorCategory> _sheypoorCategoryListInfo;

	private List<DivarCategory> _divarCategoryListInfo;

	private List<string> _imageListInfo;

	private List<string> _titleListInfo;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string Name { get; set; }

	public Guid GroupGuid { get; set; }

	public decimal Price { get; set; }

	public bool IsPishNevis { get; set; }

	public string ImageList
	{
		get
		{
			return _imageListData;
		}
		set
		{
			_imageListData = value.FixString();
		}
	}

	public string TitleList
	{
		get
		{
			return _titleListData;
		}
		set
		{
			_titleListData = value.FixString();
		}
	}

	public string ContentList { get; set; }

	public string SheypoorCategoryList
	{
		get
		{
			return _sheypoorCategoryListData;
		}
		set
		{
			_sheypoorCategoryListData = value.FixString();
		}
	}

	public string DivatCategoryList
	{
		get
		{
			return _divarCategoryListData;
		}
		set
		{
			_divarCategoryListData = value.FixString();
		}
	}

	public bool IsChecked { get; set; }

	public List<DivarCategory> DivarCategory
	{
		get
		{
			if (string.IsNullOrEmpty(DivatCategoryList))
			{
				return null;
			}
			return _divarCategoryListInfo ?? (_divarCategoryListInfo = DivatCategoryList.FromJson<List<DivarCategory>>("DivarCategory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 64));
		}
		set
		{
			DivatCategoryList = Json.ToStringJson(value);
		}
	}

	public List<SheypoorCategory> SheypoorCategory
	{
		get
		{
			if (string.IsNullOrEmpty(SheypoorCategoryList))
			{
				return null;
			}
			return _sheypoorCategoryListInfo ?? (_sheypoorCategoryListInfo = SheypoorCategoryList.FromJson<List<SheypoorCategory>>("SheypoorCategory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 74));
		}
		set
		{
			SheypoorCategoryList = Json.ToStringJson(value);
		}
	}

	public List<string> Images
	{
		get
		{
			if (string.IsNullOrEmpty(ImageList))
			{
				return null;
			}
			if (_imageListInfo != null && _imageListInfo.Count > 0)
			{
				return _imageListInfo;
			}
			return _imageListInfo = ImageList.FromJson<List<string>>("Images", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 84);
		}
		set
		{
			ImageList = Json.ToStringJson(value);
		}
	}

	public List<string> Titles
	{
		get
		{
			if (string.IsNullOrEmpty(TitleList))
			{
				return null;
			}
			if (_titleListInfo != null && _titleListInfo.Count > 0)
			{
				return _titleListInfo;
			}
			return _titleListInfo = TitleList.FromJson<List<string>>("Titles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 94);
		}
		set
		{
			TitleList = Json.ToStringJson(value);
		}
	}

	public string ContextGuid { get; set; }

	public AdvertiseBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<AdvertiseBusiness> list, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (AdvertiseBusiness item in list)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 111);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 115);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 116);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 129), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 129);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AdvertiseRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 131);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 137);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 138);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 143), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 143);
			}
		}
		return ret;
	}

	public static Task<AdvertiseBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AdvertiseRepository.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<AdvertiseBusiness>> GetAllAsync(string contextGuid, Guid groupGuid, bool? status = true, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AdvertiseRepository.GetAllAsync(groupGuid, status, tranName);
	}

	public static async Task<List<AdvertiseBusiness>> GetAllAsync(string contextGuid, string search, Guid groupGuid, int type = 0, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<AdvertiseBusiness> enumerable = await GetAllAsync(contextGuid, groupGuid, status);
				switch (type)
				{
				case 1:
					enumerable = enumerable.Where((AdvertiseBusiness q) => !q.IsPishNevis);
					break;
				case 2:
					enumerable = enumerable.Where((AdvertiseBusiness q) => q.IsPishNevis);
					break;
				}
				List<string> list = search.SplitString()?.ToList();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((AdvertiseBusiness x) => x.TitleList.StringComparisons(item) || x.ContentList.StringComparisons(item));
						}
					}
				}
				return (enumerable?.OrderByDescending((AdvertiseBusiness o) => o.Modified))?.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 193);
				return new List<AdvertiseBusiness>();
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 206), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 206);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AdvertiseRepository.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 208);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 214);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 215);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 220), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 220);
			}
		}
		return ret;
	}

	public static async Task<AdvertiseBusiness> GetAsync(string contextGuid, Guid visitorGuid, bool alaki, CancellationToken token = default(CancellationToken))
	{
		AdvertiseBusiness adv = new AdvertiseBusiness(contextGuid);
		try
		{
			IEnumerable<VisitorAdvBusiness> lst = await VisitorAdvBusiness.GetAllAsync(contextGuid, visitorGuid);
			if (lst == null || !lst.Any())
			{
				return null;
			}
			while (!(adv?.IsPishNevis ?? false))
			{
				int num = new Random().Next(0, lst.ToList().Count);
				if (num == 0 && !lst.Any())
				{
					return null;
				}
				adv = await GetAsync(contextGuid, lst.ToList()[num].AdvGuid, token);
				AdvertiseBusiness advertiseBusiness = adv;
				if (advertiseBusiness != null && !advertiseBusiness.IsPishNevis)
				{
					break;
				}
			}
			return adv;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseBusiness.cs", 243);
			return null;
		}
	}

	public static Task<IEnumerable<AdvertiseBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AdvertiseRepository.GetAllAsync(status, token, tranName);
	}
}
