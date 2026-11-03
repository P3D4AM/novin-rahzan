using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Models.Accounting.Prd;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.EventArgs.Sanad;
using PacketParser.Filters;

namespace EntityCache.Business.Global;

public class MenuBusiness : IMenu, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<MenuItemBusiness> _detList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public int RowCount { get; set; }

	public int ColumnCount { get; set; }

	public string Title { get; set; } = "";


	public string Description { get; set; } = "";


	public string ContextGuid { get; set; }

	public EnMenuType Type { get; set; }

	public Guid? MasterGuid { get; set; }

	public IReadOnlyList<MenuItemBusiness> DetailList => _detList;

	public event Func<object, EventArgs, Task> OnItemChanged;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public MenuBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<MenuBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Menu.GetAllAsync(status, token, tranName);
	}

	public static Task<MenuBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Menu.GetAsync(guid, token, tranName);
	}

	public async Task AddRangeToListSanadAsync(List<MenuItemBusiness> dets, bool appendCurrentItems = false)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<MenuItemBusiness>();
			}
			if (!appendCurrentItems)
			{
				ClearList();
			}
			foreach (MenuItemBusiness det in dets)
			{
				await AddToListAsync(det);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 54);
			throw;
		}
	}

	public void ClearList()
	{
		try
		{
			List<MenuItemBusiness> detList = _detList;
			if (detList != null && detList.Any())
			{
				foreach (MenuItemBusiness item in _detList.ToList())
				{
					item?.Dispose();
				}
			}
			_detList = new List<MenuItemBusiness>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 67);
		}
	}

	public Task<MenuItemBusiness> AddToListAsync(MenuItemBusiness det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (det == null)
			{
				return null;
			}
			if (_detList == null)
			{
				_detList = new List<MenuItemBusiness>();
			}
			det.MasterGuid = Guid;
			det.OnDisposed += Det_OnDisposed;
			det.OnOrdinalChanged += Det_OnOrdinalChanged;
			_ = det.TypeGuid;
			_detList.Add(det);
			if (this.OnItemChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnItemChanged, this, new EventArgs(), token, 60, "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 85);
		}
		return Task.FromResult(det);
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		checked
		{
			try
			{
				if ((_detList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (MenuItemBusiness item in _detList?.ToList()?.Where((MenuItemBusiness p) => p == null || p == sender)?.ToList() ?? new List<MenuItemBusiness>())
					{
						item?.Dispose();
					}
				}
				_detList?.RemoveAll((MenuItemBusiness p) => p == null || p == sender);
				for (int i = 0; i < _detList.Count(); i++)
				{
					_detList[i].Ordinal = i + 1;
				}
				if (this.OnItemChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnItemChanged, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs");
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 110);
				throw;
			}
		}
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
			_ = 2;
			try
			{
				ret.AddReturnedValue(CheckValidation(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 122);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 123), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 123);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Menu.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 125);
				if (ret.HasError)
				{
					return ret;
				}
				List<Guid> list = DetailList?.Where((MenuItemBusiness q) => q.Type == EnMenuItemType.Menu && q.TypeGuid != Guid.Empty)?.Select((MenuItemBusiness q) => q.TypeGuid)?.ToList();
				if (list != null && list.Any())
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Menu.UpdateAsync(Guid, list, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 129);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 133);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 134);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 139), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 139);
			}
		}
		return ret;
	}

	private ReturnedSaveFuncInfo CheckValidation()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (RowCount > 8 || ColumnCount > 3)
			{
				returnedSaveFuncInfo.AddError("کاربر گرامی، حداکثر تعداد سطرها 8 و حداکثر تعداد ستون ها 3 ستون می باشد".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 149);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 153);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 154);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<MenuBusiness> GenerateWithDefualtsAsync(string contextGuid, EnMenuType type = EnMenuType.Static, Guid? guid = null, int rowCount = 0, int columnCount = 0, CancellationToken token = default(CancellationToken))
	{
		try
		{
			MenuBusiness item = new MenuBusiness(contextGuid);
			if (guid.HasValue && guid != Guid.Empty)
			{
				item.Guid = guid.Value;
			}
			item.RowCount = rowCount;
			item.ColumnCount = columnCount;
			item.Type = type;
			MenuItemBusiness det = new MenuItemBusiness(contextGuid, item)
			{
				Guid = Guid.Empty,
				Modified = DateTime.Now,
				Ordinal = 1,
				Status = true
			};
			await item.AddToListAsync(det, token);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateWithDefualtsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 180);
			throw;
		}
	}

	public static async Task<MenuBusiness> GenerateGroupDefualtsAsync(string contextGuid, Guid defProductGroupGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			ProductGroupFilter filter = new ProductGroupFilter
			{
				System = EnSystem.UserInterface,
				LoadParentWithGuidEmpty = false
			};
			ProductGroupModel productGroupModel = await PrdGroupBusiness.GetAllAsync(contextGuid, filter, token);
			if (productGroupModel.List.Count() == 1 && productGroupModel.List.SingleOrDefault().Guid == defProductGroupGuid)
			{
				return await GenerateProductDefualtsAsync(contextGuid, (await ProductCompactBusiness.GetAllAsync(contextGuid, EnSystem.UserInterface, null, token)).ToList(), token);
			}
			MenuBusiness item = new MenuBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				RowCount = 5,
				ColumnCount = 2,
				Type = EnMenuType.Static
			};
			List<PrdGroupBusiness> list = productGroupModel?.List?.Where((PrdGroupBusiness q) => q.Description.Contains("1") || q.Description.Contains("2") || q.Description.Contains("3") || q.Description.Contains("4") || q.Description.Contains("5") || q.Description.Contains("6") || q.Description.Contains("7") || q.Description.Contains("8") || q.Description.Contains("9"))?.OrderBy((PrdGroupBusiness q) => q.Description)?.ThenBy((PrdGroupBusiness q) => q.Name)?.ToList();
			if (list != null && list.Count <= 0)
			{
				list = productGroupModel?.List?.Take(10)?.ToList();
			}
			if (list == null || !list.Any())
			{
				return item;
			}
			int ordinal = 1;
			foreach (PrdGroupBusiness item2 in list)
			{
				foreach (PrdGroupBusiness item3 in await item2.GetGroupAndChildsAsync(EnSystem.UserInterface, token))
				{
					await item.AddToListAsync(new MenuItemBusiness(contextGuid, item)
					{
						Guid = Guid.NewGuid(),
						MasterGuid = item.Guid,
						Modified = DateTime.Now,
						Ordinal = ordinal,
						Status = true,
						Type = EnMenuItemType.Card,
						TypeGuid = item3.Guid
					}, token);
					ordinal = checked(ordinal + 1);
				}
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateGroupDefualtsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 249);
			throw;
		}
	}

	public static async Task<MenuBusiness> GenerateProductDefualtsAsync(string contextGuid, List<ProductCompactBusiness> prdList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			MenuBusiness item = new MenuBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				RowCount = 6,
				ColumnCount = 3,
				Type = EnMenuType.Static
			};
			if (!(prdList?.Any() ?? false))
			{
				return item;
			}
			int ordinal = 1;
			foreach (ProductCompactBusiness prd in prdList)
			{
				await item.AddToListAsync(new MenuItemBusiness(contextGuid, item)
				{
					Guid = Guid.NewGuid(),
					MasterGuid = item.Guid,
					Modified = DateTime.Now,
					Ordinal = ordinal,
					Status = true,
					Type = EnMenuItemType.Card,
					TypeGuid = prd.Guid
				}, token);
				ordinal = checked(ordinal + 1);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateProductDefualtsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 286);
			throw;
		}
	}

	private Task Det_OnOrdinalChanged(object sender, OrdinalChangedEventArgs e)
	{
		try
		{
			if (e.OldOrdinal == e.NewOrdinal || e.OldOrdinal < 0)
			{
				return Task.CompletedTask;
			}
			(DetailList?.FirstOrDefault((MenuItemBusiness p) => p != null && p.Ordinal == e.NewOrdinal && p.Guid != e.DetailGuid))?.SetOrdinalChange(e.System, e.OldOrdinal, sender);
			if (this.OnOrdinalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, sender, e, default(CancellationToken), 60, "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuBusiness.cs", 305);
		}
		return Task.CompletedTask;
	}
}
