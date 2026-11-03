using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill.Reports;
using EntityCache.Business.Accounting.Hotel;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Hotel;
using PacketParser.BusinessLayer.Models.Placement;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using PacketParser.Filters.Hotel;

namespace EntityCache.Business.Accounting.Bill;

public class PlacementBusiness : IPlacement, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<FacilitiesBusiness> _facilitiesList;

	private decimal _price;

	private int _number;

	public Guid Guid { get; set; }

	public Guid? MasterGuid { get; set; }

	public Guid? GroupGuid { get; set; }

	public string ContextGuid { get; set; }

	public bool Status { get; set; } = true;


	public DateTime Modified { get; set; } = DateTime.Now;


	public decimal Price
	{
		get
		{
			return _price;
		}
		set
		{
			_price = value;
			if (EventManager?.OnSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs");
			}
		}
	}

	public decimal ExtraPrice { get; set; }

	public int ItemCount { get; set; }

	public bool IsChecked { get; set; }

	public int NumberForDisplay => _number;

	public string Title { get; set; }

	public string Description { get; set; }

	public ICollection<FacilitiesBusiness> FacilitiesList => _facilitiesList?.Where((FacilitiesBusiness q) => q != null)?.ToList();

	public decimal SumPrice => Price + (FacilitiesList?.Sum((FacilitiesBusiness q) => q.Price) ?? 0m);

	public PlacementBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.PlacementBusiness;

	public event Func<object, EventArgs, Task> OnSumChanged;

	public PlacementBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task AddFacilitieslist(FacilitiesBusiness facilities)
	{
		try
		{
			if (facilities == null)
			{
				return Task.CompletedTask;
			}
			if (_facilitiesList == null)
			{
				_facilitiesList = new List<FacilitiesBusiness>();
			}
			_facilitiesList.Add(facilities);
			if (EventManager?.OnSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "AddFacilitieslist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddFacilitieslist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 66);
		}
		return Task.CompletedTask;
	}

	public Task AddRengeFacilities(IEnumerable<FacilitiesBusiness> facilites, bool clear = false)
	{
		try
		{
			if (facilites == null || !facilites.Any())
			{
				return Task.CompletedTask;
			}
			if (_facilitiesList == null)
			{
				_facilitiesList = new List<FacilitiesBusiness>();
			}
			if (clear)
			{
				_facilitiesList.Clear();
			}
			_facilitiesList.AddRange(facilites);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRengeFacilities", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 81);
			throw;
		}
	}

	public Task RemoveFacilitieslist(Guid guid)
	{
		try
		{
			FacilitiesBusiness facilitiesBusiness = FacilitiesList?.SingleOrDefault((FacilitiesBusiness q) => q.Guid == guid);
			if (facilitiesBusiness == null)
			{
				return Task.CompletedTask;
			}
			_facilitiesList.Remove(facilitiesBusiness);
			if (EventManager?.OnSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "RemoveFacilitieslist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveFacilitieslist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 96);
		}
		return Task.CompletedTask;
	}

	public static Task<IEnumerable<PlacementReportsBusiness>> GetAllAsync(string contextGuid, RoomFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetAllAsync(filter, token, tranName);
	}

	public static Task<IEnumerable<PlacementKardexViewModel>> GetAllKardexAsync(string contextGuid, PlacementKardexFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetAllKardexAsync(filter, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "PlacementBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 112);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 113), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 113);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 115);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							if ((MasterGuid ?? Guid.Empty) == Guid.Empty)
							{
								MasterGuid = null;
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Placement.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 118);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.PlacementFacilities.RemoveRangeAsync(Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 120);
								if (!res.HasError)
								{
									if (FacilitiesList?.Any() ?? false)
									{
										List<PlacementFacilitiesRelationBusiness> list = new List<PlacementFacilitiesRelationBusiness>();
										foreach (FacilitiesBusiness facilities in FacilitiesList)
										{
											list.Add(new PlacementFacilitiesRelationBusiness(ContextGuid)
											{
												ContextGuid = ContextGuid,
												FacilitiesGuid = facilities.Guid,
												PlacementGuid = Guid
											});
										}
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.PlacementFacilities.SaveRangeAsync(list, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 135);
									}
									goto end_IL_0124;
								}
								result = res;
							}
						}
					}
					goto end_IL_007d;
					end_IL_0124:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 138);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 141);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 142);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 147), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 147);
			}
		}
		return res;
	}

	public static Task<PlacementBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetAsync(guid, token, tranName);
	}

	public static Task<PlacementBusiness> GetAsync(string contextGuid, int number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetAsync(number, token, tranName);
	}

	public static Task<int> GetNextNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetNextNumber(token, tranName);
	}

	public async Task<int> GetNumberAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return _number;
			}
			if (_number <= 0)
			{
				_number = await GetNextNumberAsync(ContextGuid, token, tranName);
			}
			return _number;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 167);
			throw;
		}
	}

	public void SetNumber(int number)
	{
		_number = number;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (ItemCount <= 0)
			{
				await ret.AddErrorAsync("لطفا ظرفیت را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 177);
			}
			if (_number <= 0)
			{
				await ret.AddErrorAsync("شماره وارد شده معتبر نمی باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 178);
			}
			PlacementBusiness placementBusiness = await GetAsync(ContextGuid, _number, token, tranName);
			if (placementBusiness != null && placementBusiness.Guid != Guid)
			{
				await ret.AddErrorAsync("شماره وارد شده تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 180);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 184);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 185);
		}
		return ret;
	}

	public static Task<List<Guid>> GetTypePlacementListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetTypePlacementListAsync(token, tranName);
	}

	public static Task<List<RoomTypeModel>> GetTypeRoomCount(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Placement.GetTypeRoomCount(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, List<PlacementBusiness> lst, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (PlacementBusiness item in lst)
			{
				if (item == null)
				{
					throw new NullReferenceException("list contains null items!");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 200);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 201);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 206);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\PlacementBusiness.cs", 207);
		}
		return ret;
	}
}
