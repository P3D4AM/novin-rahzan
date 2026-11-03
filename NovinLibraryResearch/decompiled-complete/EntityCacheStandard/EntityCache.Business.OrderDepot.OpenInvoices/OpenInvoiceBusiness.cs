using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.OrderDepot.OpenInvoices;

public class OpenInvoiceBusiness : BaseBusiness<OpenInvoiceBusiness>, IOpenInvoice, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IDisposable
{
	private CashierBusiness _cashierItem;

	private bool _isDisposed;

	private List<OpenInvoicePlacementRelationBusiness> placementRelationList;

	private List<OpenInvoiceOrderRelationBusiness> orderRelationList;

	private ReceptionBusiness _reception;

	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public int Number { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public int PeopleCount { get; set; }

	public int SanadNo { get; set; }

	public decimal Sum { get; set; }

	public DateTime? SettlementDate { get; set; }

	public override string ContextGuid { get; set; }

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

	public IReadOnlyCollection<OpenInvoicePlacementRelationBusiness> PlacementRelationList
	{
		get
		{
			if (placementRelationList == null)
			{
				placementRelationList = new List<OpenInvoicePlacementRelationBusiness>();
			}
			placementRelationList.RemoveAll((OpenInvoicePlacementRelationBusiness q) => q?.IsDisposed ?? true);
			return placementRelationList;
		}
	}

	public IReadOnlyCollection<OpenInvoiceOrderRelationBusiness> OrderRelationList
	{
		get
		{
			if (orderRelationList == null)
			{
				orderRelationList = new List<OpenInvoiceOrderRelationBusiness>();
			}
			orderRelationList.RemoveAll((OpenInvoiceOrderRelationBusiness q) => q?.IsDisposed ?? true);
			return orderRelationList;
		}
	}

	public List<PlacementOrderRelationBusiness> PlacementOrderRelationList
	{
		get
		{
			try
			{
				List<PlacementOrderRelationBusiness> list = new List<PlacementOrderRelationBusiness>();
				IReadOnlyCollection<OpenInvoicePlacementRelationBusiness> readOnlyCollection = PlacementRelationList;
				if (readOnlyCollection != null && readOnlyCollection.Any())
				{
					IReadOnlyCollection<OpenInvoiceOrderRelationBusiness> readOnlyCollection2 = OrderRelationList;
					if (readOnlyCollection2 != null && readOnlyCollection2.Any())
					{
						foreach (OpenInvoiceOrderRelationBusiness orderRelation in OrderRelationList)
						{
							foreach (OpenInvoicePlacementRelationBusiness placementRelation in PlacementRelationList)
							{
								list.Add(new PlacementOrderRelationBusiness(ContextGuid)
								{
									ContextGuid = ContextGuid,
									OrderGuid = orderRelation.OrderGuid,
									PlacementGuid = placementRelation.PlacementGuid
								});
							}
						}
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PlacementOrderRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 87);
			}
			return null;
		}
	}

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public OpenInvoiceBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.OpenInvoice;

	public event Func<object, Guid, Task> OnPlacementRelationAdd;

	public event Func<object, Guid, Task> OnOrderRelationAdd;

	public event Func<object, EventArgs, Task> OnPlacementRelationDisposed;

	public event Func<object, EventArgs, Task> OnOrderRelationDisposed;

	public OpenInvoiceBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (OpenInvoicePlacementRelationBusiness item in placementRelationList?.ToList() ?? new List<OpenInvoicePlacementRelationBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 109);
			}
		}
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (TafsilGuid == Guid.Empty)
			{
				return null;
			}
			return await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 120);
			throw;
		}
	}

	public async Task<CashierBusiness> GetCashierAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_cashierItem != null)
			{
				return _cashierItem;
			}
			_cashierItem = await CashierBusiness.GetAsync(ContextGuid, CashierGuid, token, tranName);
			return _cashierItem;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 134);
			throw;
		}
	}

	public static Task<int> GetNextNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.OpenInvoice.GetNextNumberAsync(token, tranName);
	}

	public static async Task<OpenInvoiceBusiness> GenerateNewAsync(string contextGuid, DateTime? dateM = null, Guid? tafsilGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			OpenInvoiceBusiness openInvoiceBusiness = new OpenInvoiceBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				ContextGuid = contextGuid,
				Modified = DateTime.Now,
				Status = true
			};
			OpenInvoiceBusiness openInvoiceBusiness2 = openInvoiceBusiness;
			openInvoiceBusiness2.Number = await GetNextNumberAsync(contextGuid, token, tranName);
			openInvoiceBusiness.Guid = Guid.NewGuid();
			OpenInvoiceBusiness item = openInvoiceBusiness;
			if (!dateM.HasValue)
			{
				item.DateM = DateTime.Now;
			}
			else
			{
				item.DateM = dateM.Value;
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				item.TafsilGuid = tafsilGuid.Value;
			}
			else
			{
				tafsilGuid = await cashier.GetCusGuidAsync(EnSystem.UserInterface);
				if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					item.TafsilGuid = tafsilGuid.Value;
				}
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 168);
			throw;
		}
	}

	public static Task<Guid> Id2ToGuidAsync(string contextGuid, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.OpenInvoice.Id2ToGuidAsync(id2, token, tranName);
	}

	public Task AddPlacementRelationAsync(Guid placementGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (placementGuid == Guid.Empty)
			{
				return Task.CompletedTask;
			}
			if (placementRelationList == null)
			{
				placementRelationList = new List<OpenInvoicePlacementRelationBusiness>();
			}
			if (placementRelationList?.SingleOrDefault((OpenInvoicePlacementRelationBusiness q) => q.PlacementGuid == placementGuid) != null)
			{
				return Task.CompletedTask;
			}
			OpenInvoicePlacementRelationBusiness obj = new OpenInvoicePlacementRelationBusiness(ContextGuid)
			{
				ContextGuid = ContextGuid,
				InvoiceGuid = Guid
			};
			List<OpenInvoicePlacementRelationBusiness> list = placementRelationList;
			obj.Ordinal = checked((short)((list == null || !list.Any()) ? 1 : ((short)((placementRelationList?.Max((OpenInvoicePlacementRelationBusiness p) => p.Ordinal) ?? (-1)) + 1))));
			obj.PlacementGuid = placementGuid;
			OpenInvoicePlacementRelationBusiness openInvoicePlacementRelationBusiness = obj;
			openInvoicePlacementRelationBusiness.OnDisposed += Placement_OnDisposed;
			placementRelationList.Add(openInvoicePlacementRelationBusiness);
			if (EventManager.OnPlacementRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnPlacementRelationAdd, this, openInvoicePlacementRelationBusiness.PlacementGuid, token, 60, "AddPlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddPlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 194);
		}
		return Task.CompletedTask;
	}

	public Task AddPlacementRelationAsync(OpenInvoicePlacementRelationBusiness data, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (data == null)
			{
				return Task.CompletedTask;
			}
			if (placementRelationList == null)
			{
				placementRelationList = new List<OpenInvoicePlacementRelationBusiness>();
			}
			data.InvoiceGuid = Guid;
			if (data.Ordinal <= 0)
			{
				data.Ordinal = checked((short)((placementRelationList?.Max((OpenInvoicePlacementRelationBusiness p) => p.Ordinal) ?? (-1)) + 1));
			}
			data.OnDisposed += Placement_OnDisposed;
			placementRelationList.Add(data);
			if (EventManager.OnPlacementRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnPlacementRelationAdd, this, data.PlacementGuid, token, 60, "AddPlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddPlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 210);
		}
		return Task.CompletedTask;
	}

	public async Task AddPlacementRelationListAsync(List<OpenInvoicePlacementRelationBusiness> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (OpenInvoicePlacementRelationBusiness item in list)
			{
				await AddPlacementRelationAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddPlacementRelationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 220);
		}
	}

	private Task Placement_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Placement_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs"))
		{
			try
			{
				if ((placementRelationList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (OpenInvoicePlacementRelationBusiness item in placementRelationList?.ToList()?.Where((OpenInvoicePlacementRelationBusiness p) => p == null || p == sender)?.ToList() ?? new List<OpenInvoicePlacementRelationBusiness>())
					{
						item?.Dispose();
					}
				}
				placementRelationList?.RemoveAll((OpenInvoicePlacementRelationBusiness p) => p == null || p == sender);
				if (EventManager.OnPlacementRelationDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnPlacementRelationDisposed, this, new EventArgs(), default(CancellationToken), 60, "Placement_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Placement_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 240);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemovePlacementRelationAsync(Guid placementGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			OpenInvoicePlacementRelationBusiness openInvoicePlacementRelationBusiness = placementRelationList?.FirstOrDefault((OpenInvoicePlacementRelationBusiness q) => q.PlacementGuid == placementGuid);
			if (openInvoicePlacementRelationBusiness == null)
			{
				return Task.CompletedTask;
			}
			placementRelationList.Remove(openInvoicePlacementRelationBusiness);
			if (EventManager.OnPlacementRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnPlacementRelationAdd, this, placementGuid, token, 60, "RemovePlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemovePlacementRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 255);
		}
		return Task.CompletedTask;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.OpenInvoice;
	}

	public Task AddOrderRelationAsync(Guid orderGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (orderGuid == Guid.Empty)
			{
				return Task.CompletedTask;
			}
			if (orderRelationList == null)
			{
				orderRelationList = new List<OpenInvoiceOrderRelationBusiness>();
			}
			if (orderRelationList?.SingleOrDefault((OpenInvoiceOrderRelationBusiness q) => q.OrderGuid == orderGuid) != null)
			{
				return Task.CompletedTask;
			}
			OpenInvoiceOrderRelationBusiness obj = new OpenInvoiceOrderRelationBusiness(ContextGuid)
			{
				ContextGuid = ContextGuid,
				InvoiceGuid = Guid
			};
			List<OpenInvoiceOrderRelationBusiness> list = orderRelationList;
			obj.Ordinal = checked((short)((list == null || !list.Any()) ? 1 : ((short)((orderRelationList?.Max((OpenInvoiceOrderRelationBusiness p) => p.Ordinal) ?? (-1)) + 1))));
			obj.OrderGuid = orderGuid;
			OpenInvoiceOrderRelationBusiness openInvoiceOrderRelationBusiness = obj;
			openInvoiceOrderRelationBusiness.OnDisposed += Order_OnDisposed;
			orderRelationList.Add(openInvoiceOrderRelationBusiness);
			if (EventManager.OnOrderRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnOrderRelationAdd, this, openInvoiceOrderRelationBusiness.OrderGuid, token, 60, "AddOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 279);
		}
		return Task.CompletedTask;
	}

	public Task AddOrderRelationAsync(OpenInvoiceOrderRelationBusiness data, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (data == null)
			{
				return Task.CompletedTask;
			}
			if (orderRelationList == null)
			{
				orderRelationList = new List<OpenInvoiceOrderRelationBusiness>();
			}
			data.InvoiceGuid = Guid;
			if (data.Ordinal <= 0)
			{
				data.Ordinal = checked((short)((orderRelationList?.Max((OpenInvoiceOrderRelationBusiness p) => p.Ordinal) ?? (-1)) + 1));
			}
			data.OnDisposed += Order_OnDisposed;
			orderRelationList.Add(data);
			if (EventManager.OnOrderRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnOrderRelationAdd, this, data.OrderGuid, token, 60, "AddOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 295);
		}
		return Task.CompletedTask;
	}

	public async Task AddOrderRelationListAsync(List<OpenInvoiceOrderRelationBusiness> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (OpenInvoiceOrderRelationBusiness item in list)
			{
				await AddOrderRelationAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddOrderRelationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 305);
		}
	}

	private Task Order_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Order_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs"))
		{
			try
			{
				if ((orderRelationList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (OpenInvoiceOrderRelationBusiness item in orderRelationList?.ToList()?.Where((OpenInvoiceOrderRelationBusiness p) => p == null || p == sender)?.ToList() ?? new List<OpenInvoiceOrderRelationBusiness>())
					{
						item?.Dispose();
					}
				}
				orderRelationList?.RemoveAll((OpenInvoiceOrderRelationBusiness p) => p == null || p == sender);
				if (EventManager.OnOrderRelationDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnOrderRelationDisposed, this, new EventArgs(), default(CancellationToken), 60, "Order_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Order_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 325);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveOrderRelationAsync(Guid orderGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			OpenInvoiceOrderRelationBusiness openInvoiceOrderRelationBusiness = orderRelationList?.FirstOrDefault((OpenInvoiceOrderRelationBusiness q) => q.OrderGuid == orderGuid);
			if (openInvoiceOrderRelationBusiness == null)
			{
				return Task.CompletedTask;
			}
			orderRelationList.Remove(openInvoiceOrderRelationBusiness);
			if (EventManager.OnOrderRelationAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnOrderRelationAdd, this, orderGuid, token, 60, "RemoveOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 340);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, Guid orderGuid, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "OpenInvoiceBusiness/SaveAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 352);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autotran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 353), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 353);
					}
					trantionBeenCreate = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 355);
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.OpenInvoice.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 357);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await PlacementOrderRelationBusiness.RemoveRangeAsync(ContextGuid, orderGuid, awaitOnSemaphore, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 359);
							if (!res.HasError && (PlacementOrderRelationList?.Any() ?? false))
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await PlacementOrderRelationBusiness.SaveRangeAsync(ContextGuid, PlacementOrderRelationList, awaitOnSemaphore, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 361);
								goto end_IL_0129;
							}
							result = res;
						}
					}
					goto end_IL_007d;
					end_IL_0129:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 365);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 366);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 371), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 371);
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!Status)
			{
				return res;
			}
			if (CashierGuid == Guid.Empty)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
			}
			if (TafsilGuid == Guid.Empty)
			{
				await res.AddErrorAsync("طرف حساب صورتحساب را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 382);
			}
			if (!(PlacementRelationList?.Any() ?? false))
			{
				await res.AddErrorAsync("محل پذیرایی را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 383);
			}
			if (!(OrderRelationList?.Any() ?? false))
			{
				await res.AddErrorAsync("صورتحساب را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 384);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 388);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 389);
		}
		return res;
	}

	public static async Task<OpenInvoiceBusiness> GetLastRecordAsync(string contextGuid, Guid? placementGuid, Guid? orderGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (placementGuid == Guid.Empty)
			{
				return null;
			}
			Guid? guid = (await PlacementOrderRelationBusiness.GetAllAsync(contextGuid, placementGuid, orderGuid, token, tranName))?.Select((PlacementOrderRelationReportBusiness q) => q.OrderGuid)?.FirstOrDefault();
			if ((guid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.OpenInvoice.GetByOrderGuidAsync(guid.Value, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 406);
			throw;
		}
	}

	public async Task<List<OrderReportBusiness>> GetOrderListAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(OrderRelationList?.Any() ?? false))
			{
				return null;
			}
			List<OrderReportBusiness> list = new List<OrderReportBusiness>();
			foreach (OpenInvoiceOrderRelationBusiness orderRelation in OrderRelationList)
			{
				if (!((orderRelation?.OrderGuid ?? Guid.Empty) == Guid.Empty))
				{
					OrderReportBusiness orderReportBusiness = await OrderBusiness.GetReportAsync(ContextGuid, orderRelation.OrderGuid, token, tranName);
					if (orderReportBusiness != null)
					{
						list.Add(orderReportBusiness);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 426);
			throw;
		}
	}

	public async Task<decimal> GetOrderSumAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetOrderListAsync(token, tranName))?.Sum((OrderReportBusiness q) => q.TotalSum) ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 439);
			throw;
		}
	}

	public async Task<List<OrderDetailBusiness>> GetOrderDetailListAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		checked
		{
			try
			{
				if (!(OrderRelationList?.Any() ?? false))
				{
					return null;
				}
				List<OrderDetailBusiness> list = new List<OrderDetailBusiness>();
				foreach (OpenInvoiceOrderRelationBusiness orderRelation in OrderRelationList)
				{
					if ((orderRelation?.OrderGuid ?? Guid.Empty) == Guid.Empty)
					{
						continue;
					}
					OrderBusiness orderBusiness = await OrderBusiness.GetAsync(ContextGuid, orderRelation.OrderGuid, Document.OrderSell, fromDatabase: true, token, tranName);
					if (!(orderBusiness?.ListKala?.Any()).GetValueOrDefault())
					{
						continue;
					}
					foreach (OrderDetailBusiness product in orderBusiness.ListKala)
					{
						if (product != null)
						{
							OrderDetailBusiness orderDetailBusiness = list.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == product.ProductGuid);
							if (orderDetailBusiness == null)
							{
								list.Add(product);
							}
							else
							{
								await orderDetailBusiness.SetQuantityAsync(system, orderDetailBusiness.Quantity + product.Quantity, token, tranName);
							}
						}
					}
				}
				if (!(list?.Any() ?? false))
				{
					return null;
				}
				for (int i = 0; i < list.Count(); i++)
				{
					list[i].Ordinal = i + 1;
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetOrderDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 469);
				throw;
			}
		}
	}

	public async Task<ReceptionBusiness> GetReceptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_reception == null)
			{
				_reception = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid ?? Guid.Empty, system, null, fromDatabase: true, token, tranName);
			}
			if (_reception == null)
			{
				_reception = new ReceptionBusiness(ContextGuid)
				{
					UserGuid = CashierGuid,
					Date = DateM.AddSeconds(1.0),
					Status = Status,
					ChangeStatus = EntityChangeSt.New,
					Description = "بابت تسویه صورتحساب شماره".GetTranslate() + $" {Number}"
				};
				await _reception.SetTafsilGuidAsync(TafsilGuid, system, token, tranName);
				ReceptionGuid = _reception.Guid;
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 494);
		}
		return _reception;
	}

	public async Task<ReturnedSaveFuncInfo> SettlementAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 12;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(true, "OpenInvoiceBusiness/SettlementAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 506);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autotran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 507), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 507);
					}
					trantionBeenCreate = true;
					Sum = await GetOrderSumAsync(token, tranName);
					SettlementDate = DateTime.Now;
					ReceptionBusiness reception = await GetReceptionAsync(system, token, tranName);
					decimal receptionSum = reception.Sum;
					foreach (OpenInvoiceOrderRelationBusiness orderRelation in OrderRelationList)
					{
						if (receptionSum <= 0m)
						{
							break;
						}
						if ((orderRelation?.OrderGuid ?? Guid.Empty) == Guid.Empty)
						{
							continue;
						}
						OrderBusiness order = await OrderBusiness.GetAsync(ContextGuid, orderRelation.OrderGuid, Document.OrderSell, fromDatabase: true);
						if (order != null)
						{
							if (receptionSum >= order.TotalSum)
							{
								await reception.AddToLinkedSanadsAsync(order.Guid, order.FactorType, order.TotalSum);
								receptionSum -= order.TotalSum;
							}
							else if (receptionSum < order.TotalSum)
							{
								await reception.AddToLinkedSanadsAsync(order.Guid, order.FactorType, receptionSum);
								receptionSum = default(decimal);
							}
						}
					}
					if (!((reception?.Sum ?? 0m) > 0m))
					{
						ReceptionGuid = null;
						goto IL_07dc;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await reception.SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 534);
					if (!res.HasError)
					{
						goto IL_07b4;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await reception.RemoveAsync(system, awaitOnSemaphore: false, 1, token, tranName), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 537);
					if (!res.HasError)
					{
						goto IL_07b4;
					}
					result = res;
					goto end_IL_0081;
					IL_07b4:
					ReceptionGuid = reception.Guid;
					goto IL_07dc;
					IL_07dc:
					if (OrderRelationList?.Any() ?? false)
					{
						AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
						if ((setting?.Reservation?.StatusId).GetValueOrDefault() != 0)
						{
							foreach (OpenInvoiceOrderRelationBusiness orderRelation2 in OrderRelationList)
							{
								if (!((orderRelation2?.OrderGuid ?? Guid.Empty) == Guid.Empty))
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await OrderBusiness.ChangeStatusIdAsync(ContextGuid, orderRelation2.OrderGuid, setting.Reservation.StatusId, token, tranName), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 552);
								}
							}
						}
					}
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.OpenInvoice.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 558);
						if (!res.HasError)
						{
							if (PlacementOrderRelationList?.Any() ?? false)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await PlacementOrderRelationBusiness.RemoveRangeAsync(ContextGuid, PlacementOrderRelationList, awaitOnSemaphore: true, token, tranName), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 561);
							}
							goto end_IL_0144;
						}
						result = res;
					}
					goto end_IL_0081;
					end_IL_0144:;
				}
				goto end_IL_006e;
				end_IL_0081:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 565);
				res.AddReturnedValue(ex, "", "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 566);
				goto end_IL_006e;
			}
			return result;
			end_IL_006e:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 571), ReturnedState.None, "SettlementAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceBusiness.cs", 571);
			}
		}
		return res;
	}

	public void ClearPlacementRelation()
	{
		placementRelationList.Clear();
	}
}
