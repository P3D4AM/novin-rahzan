using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class TableRelationShipBusiness : Serializable<TableRelationShipBusiness>, ITableRelationShip, IHasJustGuid, IHasContext, IHasStatus, INotifyPropertyChanged
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _itemSemaphore = new SemaphoreSlim(1, 1);

	private List<TableRelationShipBusiness> _childList;

	private OrderReportBusiness _orderItem;

	private ReceptionReportBusiness _receptionItem;

	private PardakhtReportBusiness _pardakhtItem;

	private string _dateSh;

	private string _desc;

	private CancellationTokenSource _itemToken;

	private decimal _totalPrice;

	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public bool Status { get; set; }

	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }

	public decimal Price { get; set; }

	public bool IsInRelationSanad { get; set; }

	public string DateSh
	{
		get
		{
			if (!string.IsNullOrEmpty(_dateSh))
			{
				return _dateSh;
			}
			_itemToken?.Cancel();
			_itemToken = new CancellationTokenSource(60000);
			CancellationToken token = _itemToken.Token;
			Task.Run(() => GetItemAsync(token));
			return _dateSh;
		}
	}

	public decimal Sum
	{
		get
		{
			if (_totalPrice != 0m)
			{
				return _totalPrice;
			}
			_itemToken?.Cancel();
			_itemToken = new CancellationTokenSource(60000);
			CancellationToken token = _itemToken.Token;
			Task.Run(() => GetItemAsync(token));
			return _totalPrice;
		}
	}

	public string Description
	{
		get
		{
			if (!string.IsNullOrEmpty(_desc))
			{
				return _desc;
			}
			_itemToken?.Cancel();
			_itemToken = new CancellationTokenSource(60000);
			CancellationToken token = _itemToken.Token;
			Task.Run(() => GetItemAsync(token));
			return _desc;
		}
	}

	public byte[] MoreImage => ImageResourceManager.ShowMoreDetail2;

	public IReadOnlyList<TableRelationShipBusiness> ChildList
	{
		get
		{
			try
			{
				if (_childList == null)
				{
					_childList = new List<TableRelationShipBusiness>();
				}
				_childList.RemoveAll((TableRelationShipBusiness p) => p?.IsDisposed ?? true);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChildList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 84);
			}
			return _childList;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public TableRelationShipBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TableRelationShipBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ClsCache cache = ClsCache.GetCache(ContextGuid);
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 103), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 103);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await cache.CacheRepository.TableRelationShip.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 105);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 100);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 107);
				goto end_IL_006c;
			}
			catch (ThreadAbortException exception2)
			{
				ret.AddReturnedValue(exception2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 108);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 111);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 112);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 117), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 117);
			}
		}
		return ret;
	}

	public Task AddChildListAsync(List<TableRelationShipBusiness> lstObjects)
	{
		try
		{
			if (lstObjects == null || !lstObjects.Any())
			{
				return Task.CompletedTask;
			}
			foreach (TableRelationShipBusiness obj in lstObjects)
			{
				if (obj != null)
				{
					if (_childList == null)
					{
						_childList = new List<TableRelationShipBusiness>();
					}
					TableRelationShipBusiness tableRelationShipBusiness = _childList.FirstOrDefault((TableRelationShipBusiness q) => q.Guid == obj.Guid);
					if (tableRelationShipBusiness != null)
					{
						_childList.Remove(tableRelationShipBusiness);
					}
					_childList.Add(obj);
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddChildListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 139);
			throw;
		}
		return Task.CompletedTask;
	}

	public void RemoveFromDets(TableRelationShipBusiness det)
	{
		using (new JaegerMonitor("RemoveFromDets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs"))
		{
			try
			{
				if (_childList == null)
				{
					_childList = new List<TableRelationShipBusiness>();
				}
				if (det != null)
				{
					_childList.Remove(det);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 152);
			}
		}
	}

	public void ClearDets()
	{
		_childList?.Clear();
	}

	public IEnumerable<TableRelationShipBusiness> GetRelatedChilds()
	{
		try
		{
			if (_childList == null)
			{
				_childList = new List<TableRelationShipBusiness>();
			}
			_childList.RemoveAll((TableRelationShipBusiness p) => p?.IsInRelationSanad ?? true);
			return _childList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRelatedChilds", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 165);
			throw;
		}
	}

	public static Task<IEnumerable<TableRelationShipBusiness>> GetAllRelatedSanadAsync(string contextGuid, Guid sanadGuid, Document sanadType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.TableRelationShip.GetAllRelatedSanadAsync(sanadGuid, sanadType, token, tranName);
		}
		return Task.FromResult<IEnumerable<TableRelationShipBusiness>>(null);
	}

	public static Task<IEnumerable<TableRelationShipBusiness>> GetAllInRelationSanadAsync(string contextGuid, Guid sanadGuid, Document sanadType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.TableRelationShip.GetAllInRelationSanadAsync(sanadGuid, sanadType, token, tranName);
		}
		return Task.FromResult<IEnumerable<TableRelationShipBusiness>>(null);
	}

	private async Task GetItemAsync(CancellationToken token, string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _itemSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (token.IsCancellationRequested)
			{
				return;
			}
			switch (SanadType)
			{
			case Document.Reception:
				if (_receptionItem == null)
				{
					_receptionItem = await ReceptionReportBusiness.GetAsync(ContextGuid, SanadGuid, token, tranName);
					_dateSh = _receptionItem?.DateSh;
					_totalPrice = _receptionItem?.Sum ?? 0m;
					if (_receptionItem == null)
					{
						_desc = "";
						return;
					}
					_desc = "دریافت".GetTranslate() + "(" + $"{_receptionItem.Id2}" + ") **" + (_receptionItem.Description ?? "").GetTranslate();
				}
				return;
			case Document.Pardakht:
				if (_pardakhtItem == null)
				{
					_pardakhtItem = await PardakhtReportBusiness.GetAsync(ContextGuid, SanadGuid, token, tranName);
					_dateSh = _pardakhtItem?.DateSh;
					_totalPrice = _pardakhtItem?.Sum ?? 0m;
					if (_pardakhtItem == null)
					{
						_desc = null;
						return;
					}
					_desc = "پرداخت".GetTranslate() + "(" + $"{_pardakhtItem.Id2}" + ") ** " + (_pardakhtItem.Description ?? "").GetTranslate();
				}
				return;
			}
			if (_orderItem == null)
			{
				_orderItem = await OrderBusiness.GetReportAsync(ContextGuid, SanadGuid, token, tranName);
				_dateSh = _orderItem?.DateSh;
				_totalPrice = _orderItem?.TotalSum ?? 0m;
				if (_orderItem == null)
				{
					_desc = "";
					return;
				}
				_desc = _orderItem.FactorType.GetDisplay().GetTranslate() + " " + $"({_orderItem.Id2}) **" + " " + (" " + _orderItem.Description).GetTranslate();
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 215);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_itemSemaphore.Release();
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null && !base.IsDisposed)
			{
				if (!string.IsNullOrEmpty(_dateSh))
				{
					UiInvokerHolder.Instance.RunOnUiThread(delegate
					{
						try
						{
							this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateSh"));
						}
						catch (Exception ex6)
						{
							WebErrorLog.Instance.StartLog(ex6, "", "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 222);
						}
					}, "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs");
				}
				if (!string.IsNullOrEmpty(_desc))
				{
					UiInvokerHolder.Instance.RunOnUiThread(delegate
					{
						try
						{
							this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
						}
						catch (Exception ex5)
						{
							WebErrorLog.Instance.StartLog(ex5, "", "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 224);
						}
					}, "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs");
				}
				if (_totalPrice != 0m)
				{
					UiInvokerHolder.Instance.RunOnUiThread(delegate
					{
						try
						{
							this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sum"));
						}
						catch (Exception ex4)
						{
							WebErrorLog.Instance.StartLog(ex4, "", "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs", 226);
						}
					}, "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Attachment\\TableRelationShipBusiness.cs");
				}
			}
		}
	}
}
