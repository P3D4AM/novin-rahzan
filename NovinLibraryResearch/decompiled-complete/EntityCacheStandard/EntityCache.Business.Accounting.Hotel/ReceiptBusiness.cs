using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel.ReportsT;
using EntityCache.Business.Core;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using PacketParser.Entities.MetaData.Hotel;
using PacketParser.Filters;
using PacketParser.Filters.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class ReceiptBusiness : BaseBusiness<ReceiptBusiness>, IReceipt, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public decimal _discount;

	public decimal _amountPaid;

	private List<CustomerReceiptRelationBusiness> _customerList;

	private OrderBusiness _order;

	private static SemaphoreSlim _databaseSemaphor = new SemaphoreSlim(1, 1);

	private List<ReceiptDetailBusiness> _receiptDetailList;

	private bool _isDisposed;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public DateTime DateM { get; set; } = DateTime.Now;


	public int Number { get; set; }

	public string Description { get; set; }

	public Guid CustomerGuid { get; set; }

	public Guid? PresenterGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public int PepolCount { get; set; }

	public bool Status { get; set; } = true;


	public ReceiptMetaData MetaData { get; set; }

	public decimal TotalPrice => ReceiptDetailList?.Sum((ReceiptDetailBusiness q) => q.TotalPrice) ?? 0m;

	public decimal DetailDiscount => ReceiptDetailList?.Sum((ReceiptDetailBusiness q) => q.Discount) ?? 0m;

	public decimal Discount
	{
		get
		{
			return _discount;
		}
		set
		{
			_discount = value;
			if (EventManager.OnChengePrice != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnChengePrice, this, new EventArgs(), default(CancellationToken), 60, "Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
		}
	}

	public decimal Penalty => ReceiptDetailList?.Sum((ReceiptDetailBusiness q) => q.Penalty) ?? 0m;

	public decimal AmountPaid
	{
		get
		{
			return TotalPrice - Discount;
		}
		set
		{
			_amountPaid = value;
			if (EventManager.OnChengePrice != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnChengePrice, this, new EventArgs(), default(CancellationToken), 60, "AmountPaid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
		}
	}

	public decimal SumServes => ReceiptDetailList?.Sum((ReceiptDetailBusiness q) => q.SumServes) ?? 0m;

	public override string ContextGuid { get; set; }

	public new bool IsDisposed => _isDisposed;

	public Guid CashierGuid => _order?.UserAccGuid ?? Guid.Empty;

	public ReceiptBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.ReceiptBusiness;

	public IReadOnlyCollection<CustomerReceiptRelationBusiness> CustomerList
	{
		get
		{
			if (_customerList == null)
			{
				_customerList = new List<CustomerReceiptRelationBusiness>();
			}
			return _customerList;
		}
	}

	public IReadOnlyCollection<ReceiptDetailBusiness> ReceiptDetailList
	{
		get
		{
			if (_receiptDetailList == null)
			{
				_receiptDetailList = new List<ReceiptDetailBusiness>();
			}
			_receiptDetailList.RemoveAll((ReceiptDetailBusiness q) => q.IsDisposed || q == null);
			return _receiptDetailList;
		}
	}

	public override IDatabaseManager DatabaseManager
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public event Func<object, EventArgs, Task> OnCustomerChanged;

	public event Func<object, Guid, Task> OnReceiptDetailAdd;

	public event Func<object, Guid, Task> OnReceiptDetailRemove;

	public event Func<object, EventArgs, Task> OnServiceChanged;

	public event Func<object, EventArgs, Task> OnRowSumChanged;

	public event Func<object, EventArgs, Task> OnChengePrice;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public ReceiptBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		Guid = Guid.NewGuid();
		ChangeStatus = EntityChangeSt.New;
	}

	public static Task<ReceiptBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Receipt.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<ReceiptReportBusiness>> GetAllAsync(string contextGuid, ReceiptFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Receipt.GetAllAsync(filter, token, tranName);
	}

	public Task AddRangeToCustomerListAsync(IEnumerable<CustomerReceiptRelationBusiness> _list, bool clearList = false, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_list == null || !_list.Any())
			{
				return Task.CompletedTask;
			}
			if (_customerList == null)
			{
				_customerList = new List<CustomerReceiptRelationBusiness>();
			}
			if (clearList)
			{
				_customerList.Clear();
			}
			_customerList.AddRange(_list);
			if (EventManager?.OnCustomerChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnCustomerChanged, this, new EventArgs(), token, 60, "AddRangeToCustomerListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToCustomerListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 123);
			throw;
		}
	}

	public Task AddRangeToDetailAsync(IEnumerable<ReceiptDetailBusiness> list, bool clearlist = false)
	{
		try
		{
			if (list == null || !list.Any())
			{
				return Task.CompletedTask;
			}
			if (_receiptDetailList == null)
			{
				_receiptDetailList = new List<ReceiptDetailBusiness>();
			}
			if (clearlist)
			{
				_receiptDetailList.Clear();
			}
			_receiptDetailList.AddRange(list);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 139);
			throw;
		}
	}

	public Task AddCoustomerAsync(CustomerReceiptRelationBusiness customerRecipt, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (customerRecipt == null)
			{
				return Task.CompletedTask;
			}
			if (_customerList == null)
			{
				_customerList = new List<CustomerReceiptRelationBusiness>();
			}
			_customerList.Add(customerRecipt);
			if (EventManager?.OnCustomerChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnCustomerChanged, this, new EventArgs(), token, 60, "AddCoustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddCoustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 156);
			throw;
		}
	}

	public Task AddReciptDetalAsync(ReceiptDetailBusiness receiptDetail, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (receiptDetail == null)
			{
				return Task.CompletedTask;
			}
			if (_receiptDetailList == null)
			{
				_receiptDetailList = new List<ReceiptDetailBusiness>();
			}
			receiptDetail.OnRowSumChanged += ReceiptDetail_OnRowSumChanged;
			receiptDetail.OnServiceChanged += ReceiptDetail_OnServesChanged1;
			receiptDetail.OnDisposed += ReceiptDetail_OnDisposed;
			_receiptDetailList.Add(receiptDetail);
			if (receiptDetail.RoomGuid != Guid.Empty && EventManager?.OnReceiptDetailAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnReceiptDetailAdd, this, receiptDetail.Guid, token, 60, "AddReciptDetalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			if (EventManager?.OnChengePrice != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnChengePrice, this, new EventArgs(), token, 60, "AddReciptDetalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddReciptDetalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 182);
			throw;
		}
	}

	private Task ReceiptDetail_OnServesChanged1(object sender, EventArgs e)
	{
		try
		{
			if (EventManager?.OnServiceChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnServiceChanged, this, e, default(CancellationToken), 60, "ReceiptDetail_OnServesChanged1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReceiptDetail_OnServesChanged1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 193);
		}
		return Task.CompletedTask;
	}

	private Task ReceiptDetail_OnRowSumChanged(object sender, EventArgs e)
	{
		try
		{
			if (EventManager?.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnRowSumChanged, this, e, default(CancellationToken), 60, "ReceiptDetail_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReceiptDetail_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 203);
		}
		return Task.CompletedTask;
	}

	private Task ReceiptDetail_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("ReceiptDetail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs"))
		{
			try
			{
				if ((ReceiptDetailList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (ReceiptDetailBusiness item in ReceiptDetailList?.ToList()?.Where((ReceiptDetailBusiness p) => p == null || p.Master == sender || p == sender)?.ToList() ?? new List<ReceiptDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				_receiptDetailList?.RemoveAll((ReceiptDetailBusiness p) => p == null || p.Master == sender || p == sender);
				if (EventManager.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "ReceiptDetail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReceiptDetail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 224);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	private Task ReceiptDetail_OnServesChanged(object sender, EventArgs e)
	{
		try
		{
			if (EventManager?.OnServiceChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnServiceChanged, this, e, default(CancellationToken), 60, "ReceiptDetail_OnServesChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReceiptDetail_OnServesChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 240);
			throw;
		}
	}

	public Task RemoveCustomerAsync(Guid customerGuid)
	{
		try
		{
			CustomerReceiptRelationBusiness customerReceiptRelationBusiness = _customerList?.FirstOrDefault((CustomerReceiptRelationBusiness q) => q.CustomerGuid == customerGuid);
			if (customerReceiptRelationBusiness == null)
			{
				return Task.CompletedTask;
			}
			_customerList.Remove(customerReceiptRelationBusiness);
			if (EventManager?.OnCustomerChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnCustomerChanged, this, new EventArgs(), default(CancellationToken), 60, "RemoveCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 258);
			throw;
		}
	}

	public Task RemoveReceiptDetail(Guid receiptDetailGuid)
	{
		try
		{
			ReceiptDetailBusiness receiptDetailBusiness = _receiptDetailList?.FirstOrDefault((ReceiptDetailBusiness q) => q.Guid == receiptDetailGuid);
			if (receiptDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			receiptDetailBusiness.OnServiceChanged -= ReceiptDetail_OnServesChanged;
			_receiptDetailList.Remove(receiptDetailBusiness);
			if (EventManager?.OnReceiptDetailRemove != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnReceiptDetailRemove, this, receiptDetailGuid, default(CancellationToken), 60, "RemoveReceiptDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			if (EventManager.OnChengePrice != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnChengePrice, this, new EventArgs(), default(CancellationToken), 60, "RemoveReceiptDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveReceiptDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 280);
			throw;
		}
	}

	public static Task<int> GetNextNumberReciptAsync(string contextGuid, CancellationToken token = default(CancellationToken), string teanName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Receipt.GetNextNumberRecipt(token, teanName);
	}

	private async Task<OrderBusiness> GenerateOrderAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 8;
		try
		{
			if (OrderGuid == Guid.Empty)
			{
				OrderGuid = Guid.NewGuid();
			}
			_order = await OrderBusiness.GetAsync(ContextGuid, OrderGuid, Document.OrderSell, fromDatabase: true, token, tranName);
			string description = "فاکتور متصل به پذیرش شماره ".GetTranslate() + $"{Number}";
			if (_order == null)
			{
				_order = await OrderBusiness.GenerateNewWithDefaultsAsync(ContextGuid, system, Document.OrderSell, ENSource.Hotel, CustomerGuid, description, OrderGuid, tranName, token);
			}
			_order.SetId2(Number);
			await _order.SetDateAsync(DateM, system, token, tranName);
			_order.ClearListKala();
			foreach (ReceiptDetailBusiness item in ReceiptDetailList)
			{
				if (!((item?.Guid ?? Guid.Empty) == Guid.Empty))
				{
					OrderDetailBusiness det = await OrderDetailBusiness.GetOrderDetailBusinessAsync(ContextGuid, _order, token, tranName);
					await det.SetProductGuidAsync(system, item.RoomGuid, isLoadOriginalPrice: true, tranName, token);
					await det.SetDepotGuidAsync(item.RoomGuid, token);
					await det.SetQuantityAsync(system, 1m, token, tranName);
					await det.SetPriceAsync(system, item.TotalPrice, token, tranName);
					await _order.AddDetAsync(det, system, awaitOnSemaphor: true, token);
				}
			}
			return _order;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 312);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transationHasBeenCreate = false;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphor.GaurdAsync(true, "ReceiptBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 325);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 326), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 326);
					}
					transationHasBeenCreate = true;
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						await GenerateOrderAsync(system, token, tranName);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 330);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await _order.SaveAsync(system, awaitOnSemaphore: true, sendToServer: true, isTemp: false, 10, "", "", "", 60000, await4sent: false, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 332);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Receipt.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 334);
								goto end_IL_0124;
							}
							result = res;
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
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 336);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 339);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 340);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autotran && transationHasBeenCreate)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 345), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 345);
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transationHasBeenCreate = false;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphor.GaurdAsync(true, "ReceiptBusiness/RemoveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 358);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 359), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 359);
					}
					transationHasBeenCreate = true;
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						_order = await OrderBusiness.GetAsync(ContextGuid, OrderGuid, Document.OrderSell, fromDatabase: true, token, tranName);
						if (_order != null)
						{
							OrderRemoveFilter filter = new OrderRemoveFilter
							{
								IsDeleteAghsat = true,
								IsDeletePardakht = true,
								IsDeleteReception = true,
								IsDeleteTolid = true
							};
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await _order.RemoveAsync(system, filter, 0, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 372);
						}
						if (!res.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Receipt.RemoveAsync(Guid, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 375);
							goto end_IL_0120;
						}
						result = res;
					}
					goto end_IL_007d;
					end_IL_0120:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 377);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 380);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 381);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autotran && transationHasBeenCreate)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 386), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 386);
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(List<Guid> guids, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			foreach (Guid guid in guids)
			{
				_ = guid;
				await RemoveAsync(system, token, tranName);
			}
			if (res.HasError)
			{
				return res;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 401);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 402);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (CustomerGuid == Guid.Empty)
				{
					res.AddError("لطفا مشخصات مهمان را وارد کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 411);
				}
				if (!(ReceiptDetailList?.Any() ?? false))
				{
					res.AddError("لطفا اتاق مورد نظر را انتخاب کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 412);
				}
				if (Guid == Guid.Empty && Guid != Guid)
				{
					res.AddError("شماره پذیرش تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 413);
				}
				if (Number <= 0)
				{
					res.AddError("شماره پذیرش صحیح نیست".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 414);
				}
				if (TotalPrice < Discount || TotalPrice <= 0m)
				{
					res.AddError("مبلغ وارد شده مصحیح نیست ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 415);
				}
				int? num = ReceiptDetailList?.Sum((ReceiptDetailBusiness q) => q.CountCustomer);
				IReadOnlyCollection<CustomerReceiptRelationBusiness> customerList = CustomerList;
				if (num != ((customerList != null) ? new int?(customerList.Count() + 1) : null))
				{
					res.AddError("لیست اشخاص رو به درستی وارد کنید ".GetTranslate() + " DetailCount:" + $"{ReceiptDetailList.Sum((ReceiptDetailBusiness q) => q.CountCustomer)} \r\n " + "CusCount: " + $"{CustomerList.Count() + 1}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 417);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (ReceiptDetailBusiness receiptDetail in ReceiptDetailList)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await receiptDetail.CheckValidationAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 419);
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _order.CheckValidationAsync(system, isDeleteAction: false, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 420);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 424);
				res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 425);
			}
			return res;
		}
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (ReceiptDetailBusiness item in ReceiptDetailList?.ToList() ?? new List<ReceiptDetailBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose(disposing);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptBusiness.cs", 442);
			}
		}
	}

	public override IEditableDelivery GetRepository()
	{
		throw new NotImplementedException();
	}
}
