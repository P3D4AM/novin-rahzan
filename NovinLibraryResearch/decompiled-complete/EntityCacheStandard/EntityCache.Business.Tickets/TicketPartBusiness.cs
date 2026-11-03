using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global.Tickets;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket.TicketPart;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData.Ticket;
using PacketParser.Filters.Global;

namespace EntityCache.Business.Tickets;

public class TicketPartBusiness : Serializable<TicketPartBusiness>, ITicketPart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<TicketPartDetailBusiness> _detList;

	private bool _isDisposed;

	public Guid Guid { get; set; }

	public bool Status { get; set; } = true;


	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	public string Description { get; set; }

	public short Ordinal { get; set; }

	public string ContextGuid { get; set; }

	public TicketPartMetaData MetaData { get; set; }

	[JsonIgnore]
	public string FullName { get; private set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public TicketPartBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.TicketPartBusiness;

	public IReadOnlyCollection<TicketPartDetailBusiness> DetailList
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<TicketPartDetailBusiness>();
			}
			_detList.RemoveAll((TicketPartDetailBusiness q) => q?.IsDisposed ?? true);
			return _detList?.OrderBy((TicketPartDetailBusiness q) => q.TargetName)?.ToList();
		}
	}

	public event Func<object, Guid, Task> OnDetailChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public TicketPartBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					res.AddError("نام بخش را باید مشخص نمائید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 59);
				}
				if (!Status)
				{
					IEnumerable<TicketPartBusiness> enumerable = await GetSubItemsAsync(token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						res.AddError("بخش".GetTranslate() + " (" + Name + ")" + " به علت داشتن ".GetTranslate() + $"({enumerable.Count()})" + " زیرمجموعه، امکان حذف شدن ندارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 63);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 68);
				res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 69);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short autoFixValidationCount, bool sendToServer, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 4;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "TicketPartBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 83);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 84), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 84);
							}
							transactionHasBeenCreated = true;
							if (Ordinal <= 0)
							{
								Ordinal = await GetNextOrdinalAsync(token, tranName);
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 87);
							if (!ret.HasError)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TicketPart.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 89);
								goto end_IL_013f;
							}
							result = ret;
						}
						goto end_IL_009c;
						end_IL_013f:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 93);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 94);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 99), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 99);
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, short autoFixValidationCount, IEnumerable<TicketPartBusiness> list, bool send2Server, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return ret;
			}
			foreach (TicketPartBusiness item in list)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 111);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, autoFixValidationCount, send2Server, await4Send: false, 10, "", token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 112);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 117);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 118);
		}
		return ret;
	}

	public static Task<TicketPartBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.TicketPart.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<TicketPartBusiness>(null);
	}

	public static Task<TicketPartBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TicketPart.GetAsync(guid, token, tranName);
	}

	public static Task<TicketPartBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TicketPart.GetAsync(name, token, tranName);
	}

	public Task<IEnumerable<TicketPartBusiness>> GetSubItemsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.TicketPart.GetSubItemsAsync(Guid, token, tranName);
	}

	public static async Task<IEnumerable<TicketPartBusiness>> GetAllAsync(string contextGuid, TicketPartFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new ArgumentException("contextGuid is null or empty!");
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs"))
		{
			_ = 3;
			try
			{
				IEnumerable<TicketPartBusiness> enumerable = await (ClsCache.GetCache(contextGuid)?.CacheRepository?.TicketPart?.GetAllAsync(filter?.Status, token, tranName));
				if (enumerable == null || !enumerable.Any())
				{
					return Enumerable.Empty<TicketPartBusiness>();
				}
				List<TicketPartBusiness> tempList = enumerable.OrderBy((TicketPartBusiness o) => o.Name).ToList();
				if (filter?.IsLoadFullName ?? false)
				{
					await Task.WhenAll(tempList.Select((TicketPartBusiness q) => q.GetFullNameAsync(token)));
				}
				if ((filter?.CashierGuid ?? Guid.Empty) == Guid.Empty)
				{
					return tempList.OrderBy((TicketPartBusiness q) => q.Name);
				}
				List<Guid> list = await GetAllRelatedPartsAsync(contextGuid, filter.CashierGuid.Value, token, tranName);
				if (list == null || !list.Any())
				{
					return tempList.OrderBy((TicketPartBusiness q) => q.Name);
				}
				HashSet<Guid> filteredSet = new HashSet<Guid>();
				List<TicketPartBusiness> filteredList = new List<TicketPartBusiness>();
				foreach (Guid item in list.Where((Guid x) => x != Guid.Empty))
				{
					TicketPartBusiness part = await GetAsync(contextGuid, item, token, tranName);
					if (part == null)
					{
						continue;
					}
					if (!(part?.MetaData?.ValidPartGuidList?.Any()).GetValueOrDefault())
					{
						foreach (TicketPartBusiness item2 in tempList.Where((TicketPartBusiness x) => x != null && !filteredSet.Contains(x.Guid)))
						{
							filteredSet.Add(item2.Guid);
							filteredList.Add(item2);
						}
						continue;
					}
					foreach (TicketPartBusiness item3 in tempList.Where((TicketPartBusiness q) => q != null && part.MetaData.ValidPartGuidList.Contains(q.Guid) && !filteredSet.Contains(q.Guid)))
					{
						filteredSet.Add(item3.Guid);
						filteredList.Add(item3);
					}
				}
				return filteredList.OrderBy((TicketPartBusiness q) => q.Name);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 182);
				throw;
			}
		}
	}

	private Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.TicketPart.GetNextOrdinalAsync(token, tranName);
	}

	private async Task GetFullNameAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!string.IsNullOrEmpty(FullName))
			{
				return;
			}
			FullName = Name;
			if (!((ParentGuid ?? Guid.Empty) != Guid.Empty))
			{
				return;
			}
			TicketPartBusiness ticketPartBusiness = await GetAsync(ContextGuid, ParentGuid.Value, token);
			while (ticketPartBusiness != null)
			{
				FullName = ticketPartBusiness.Name + " / " + FullName;
				while (ticketPartBusiness != null)
				{
					if ((ticketPartBusiness.ParentGuid ?? Guid.Empty) == Guid.Empty)
					{
						ticketPartBusiness = null;
						continue;
					}
					ticketPartBusiness = await GetAsync(ContextGuid, ticketPartBusiness.ParentGuid.Value, token);
					if (ticketPartBusiness != null)
					{
						FullName = ticketPartBusiness.Name + "/" + FullName;
					}
					ticketPartBusiness = null;
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 214);
		}
	}

	public Task AddToDetailListAsync(TicketPartDetailBusiness _det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_det == null)
			{
				return Task.CompletedTask;
			}
			if (_detList == null)
			{
				_detList = new List<TicketPartDetailBusiness>();
			}
			if (_detList?.SingleOrDefault((TicketPartDetailBusiness q) => q.TargetGuid == _det.TargetGuid && q.TargetType == _det.TargetType) != null)
			{
				return Task.CompletedTask;
			}
			_det.OnDisposed += Det_OnDisposed;
			_det.MasterGuid = Guid;
			_detList.Add(_det);
			if (EventManager.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailChanged, this, _det.TargetGuid, token, 60, "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 233);
		}
		return Task.CompletedTask;
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs"))
		{
			try
			{
				if ((_detList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (TicketPartDetailBusiness item in _detList?.ToList()?.Where((TicketPartDetailBusiness p) => p == null || p == sender)?.ToList() ?? new List<TicketPartDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				_detList?.RemoveAll((TicketPartDetailBusiness p) => p == null || p == sender);
				if (EventManager.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs");
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 256);
				throw;
			}
		}
	}

	public Task RemoveDetailAsync(Guid targetGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			TicketPartDetailBusiness ticketPartDetailBusiness = _detList?.FirstOrDefault((TicketPartDetailBusiness q) => q.TargetGuid == targetGuid);
			if (ticketPartDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			_detList.Remove(ticketPartDetailBusiness);
			if (EventManager.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailChanged, this, targetGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 272);
		}
		return Task.CompletedTask;
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (TicketPartDetailBusiness item in _detList?.ToList() ?? new List<TicketPartDetailBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 285);
			}
		}
	}

	private static Task<List<Guid>> GetAllRelatedPartsAsync(string contextGuid, Guid targetGuid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TicketPart.GetAllRelatedPartsAsync(targetGuid, token, tranName);
	}

	public static async Task<IEnumerable<AccessibleDepartmentAndUser>> GetAllAccessibleDepartmentAndUserAsync(string contextGuid, Guid userGuid, CancellationToken token, string tranName = "")
	{
		List<AccessibleDepartmentAndUser> res = new List<AccessibleDepartmentAndUser>();
		try
		{
			IEnumerable<TicketPartBusiness> TicketPartList = await GetAllAsync(contextGuid, new TicketPartFilter
			{
				IsLoadFullName = false,
				Status = true,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			}, token);
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			IEnumerable<CashierBusiness> enumerable = (await CashierBusiness.GetAllAsync(contextGuid, userGuid, true, token)).Where((CashierBusiness x) => x.Guid != cashier.Guid);
			List<AccessibleDepartmentAndUser> collection = TicketPartList?.Select((TicketPartBusiness x) => new AccessibleDepartmentAndUser
			{
				Name = x.Name,
				Guid = x.Guid,
				TargetType = EnTargetType.Department
			}).ToList();
			res.AddRange(collection);
			List<AccessibleDepartmentAndUser> collection2 = enumerable?.Select((CashierBusiness x) => new AccessibleDepartmentAndUser
			{
				Name = x.Name,
				Guid = x.Guid,
				TargetType = EnTargetType.Cashier
			}).ToList();
			res.AddRange(collection2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAccessibleDepartmentAndUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartBusiness.cs", 312);
		}
		return res;
	}
}
