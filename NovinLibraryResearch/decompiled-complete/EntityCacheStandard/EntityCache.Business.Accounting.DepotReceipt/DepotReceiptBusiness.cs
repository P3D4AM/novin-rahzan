using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Business.OrderDepot.OrderComparer;
using EntityCache.Core;
using EntityCache.Entities;
using EntityCache.Models.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting.DepotReceipt;

public class DepotReceiptBusiness : BaseBusiness<DepotReceiptBusiness>, IDepotReceipt, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IChangeIntegrity
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private CashierBusiness _cashierItem;

	private List<DepotReceiptDetailBusiness> _detList;

	private bool _isDisposed;

	private DepotReceiptBusiness _oldItem;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public Guid? TafsilGuid { get; set; }

	public string Description { get; set; }

	public bool IsTemp { get; set; }

	public Guid CashierGuid { get; set; }

	public string CarNumber { get; set; }

	public string DriverName { get; set; }

	public EnDepotReceiptType Type { get; set; }

	public string DriverPhone { get; set; }

	public string BarnameNo { get; set; }

	public string RefrencedNumber { get; set; }

	public Guid? OrderGuid { get; set; }

	[JsonIgnore]
	public Document Document
	{
		get
		{
			if (Type != EnDepotReceiptType.Receipt)
			{
				return Document.DepotHavale;
			}
			return Document.DepotReceipt;
		}
	}

	[JsonIgnore]
	public string ShortDescription
	{
		get
		{
			if (Type != EnDepotReceiptType.Receipt)
			{
				return $"حواله خروج از انبار شماره {Id2}";
			}
			return $"رسید ورود به انبار شماره {Id2}";
		}
	}

	public override string ContextGuid { get; set; }

	[JsonIgnore]
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

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	[JsonIgnore]
	public DepotReceiptBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.DepotReceipt;

	public IReadOnlyCollection<DepotReceiptDetailBusiness> DetailList
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<DepotReceiptDetailBusiness>();
			}
			_detList.RemoveAll((DepotReceiptDetailBusiness q) => q?.IsDisposed ?? true);
			return _detList;
		}
	}

	public event Func<object, Guid, Task> OnDetailChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public DepotReceiptBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public bool IsDuplicate(DepotReceiptDetailBusiness str)
	{
		return (_detList?.Where((DepotReceiptDetailBusiness p) => p != null && p.Guid != str.Guid && p.ProductGuid == str.ProductGuid)?.Any()).GetValueOrDefault();
	}

	public override IEditableDelivery GetRepository()
	{
		throw new NotImplementedException();
	}

	public static async Task<DepotReceiptBusiness> GenerateNewDepotReceiptAsync(string contextGuid, EnDepotReceiptType _type)
	{
		_ = 3;
		try
		{
			DepotReceiptBusiness depotReceiptBusiness = new DepotReceiptBusiness(contextGuid)
			{
				Modified = DateTime.Now,
				Type = _type,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				ContextGuid = contextGuid,
				Status = true,
				DateM = DateTime.Now
			};
			DepotReceiptBusiness depotReceiptBusiness2 = depotReceiptBusiness;
			depotReceiptBusiness2.Id2 = await GetNextNumberAsync(contextGuid, _type);
			DepotReceiptBusiness item = depotReceiptBusiness;
			if ((await LocalSettingBusiness.GetSettingAsync(contextGuid)).Factors.ViewPRDForm)
			{
				DepotReceiptDetailBusiness depotReceiptDetailBusiness = await DepotReceiptDetailBusiness.GetFactoryAsync(contextGuid, item);
				depotReceiptDetailBusiness.Guid = Guid.NewGuid();
				depotReceiptDetailBusiness.Ordinal = 1;
				await item.AddToDetailListAsync(depotReceiptDetailBusiness);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewDepotReceiptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 100);
			throw;
		}
	}

	public static Task<int> GetNextNumberAsync(string contextGuid, EnDepotReceiptType type, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.GetNextNumberAsync(type, token, tranName);
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if ((TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			return await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid.Value, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 114);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 128);
			throw;
		}
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (DepotReceiptDetailBusiness item in _detList?.ToList() ?? new List<DepotReceiptDetailBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 142);
			}
		}
	}

	public Task AddToDetailListAsync(DepotReceiptDetailBusiness _det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_det == null)
			{
				return Task.CompletedTask;
			}
			if (_detList == null)
			{
				_detList = new List<DepotReceiptDetailBusiness>();
			}
			_det.OnDisposed += Det_OnDisposed;
			_detList.Add(_det);
			if (EventManager.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailChanged, this, _det.Guid, token, 60, "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 155);
		}
		return Task.CompletedTask;
	}

	public async Task<DepotReceiptDetailBusiness> AddNewDetailAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 5;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token, tranName);
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
			DepotReceiptDetailBusiness detail = await DepotReceiptDetailBusiness.GetFactoryAsync(ContextGuid, this, token, tranName);
			detail.Guid = Guid.NewGuid();
			detail.Ordinal = NextOrdinal();
			await detail.SetDepotGuidAsync(cashier?.DepotGuid ?? Guid.Empty, token, tranName);
			await detail.SetQuantityAsync(system, setting.Sells.OrderSellDefaultQT, token, tranName);
			await AddToDetailListAsync(detail, token);
			return detail;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddNewDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 174);
			throw;
		}
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		checked
		{
			using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs"))
			{
				try
				{
					if ((_detList?.ToList()?.Any()).GetValueOrDefault())
					{
						foreach (DepotReceiptDetailBusiness item in _detList?.ToList()?.Where((DepotReceiptDetailBusiness p) => p == null || p == sender)?.ToList() ?? new List<DepotReceiptDetailBusiness>())
						{
							item?.Dispose();
						}
					}
					_detList?.RemoveAll((DepotReceiptDetailBusiness p) => p == null || p == sender);
					List<DepotReceiptDetailBusiness> list = (from p in DetailList
						where p.Guid != Guid
						orderby p.Ordinal
						select p).ToList();
					for (int i = 0; i < list.Count(); i++)
					{
						list[i].Ordinal = i + 1;
					}
					if (EventManager.OnItemDisposed != null)
					{
						PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs");
					}
					return Task.CompletedTask;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 202);
					throw;
				}
			}
		}
	}

	public Task RemoveDetailAsync(Guid detGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			DepotReceiptDetailBusiness depotReceiptDetailBusiness = _detList?.FirstOrDefault((DepotReceiptDetailBusiness q) => q.Guid == detGuid);
			if (depotReceiptDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			_detList.Remove(depotReceiptDetailBusiness);
			if (EventManager.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailChanged, this, detGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 216);
		}
		return Task.CompletedTask;
	}

	private int NextOrdinal()
	{
		try
		{
			List<DepotReceiptDetailBusiness> detList = _detList;
			if (detList != null && detList.Any())
			{
				return checked((_detList?.Max((DepotReceiptDetailBusiness p) => p.Ordinal) ?? (-1)) + 1);
			}
			return 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 229);
			throw;
		}
	}

	public static Task<Guid> GetGuidById2Async(string contextGuid, EnDepotReceiptType type, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.GetGuidById2Async(type, id2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (Guid == Guid.Empty)
			{
				Guid = Guid.NewGuid();
			}
			if (TafsilGuid == Guid.Empty)
			{
				TafsilGuid = null;
			}
			if (DateM < new DateTime(2000, 1, 1) || DateM > new DateTime(2099, 1, 1))
			{
				res.AddError("تاریخ وارد شده معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 242);
			}
			if (!isDeleteAction)
			{
				Guid guid = await GetGuidById2Async(ContextGuid, Type, Id2, token, tranName);
				if (guid != Guid.Empty && guid != Guid)
				{
					res.AddError("شماره".GetTranslate() + $"{Id2} " + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 246);
				}
			}
			if (!(DetailList?.Any() ?? false) && !isDeleteAction)
			{
				res.AddError("هیچ کالایی انتخاب نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 249);
			}
			else
			{
				foreach (DepotReceiptDetailBusiness detail in DetailList)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await detail.CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 252);
				}
			}
			if (!isDeleteAction && (OrderGuid ?? Guid.Empty) != Guid.Empty)
			{
				AccessLevel accessLevel = await ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName)) ?? throw new ArgumentNullException("cashier")).GetUserAccessAsync(token, tranName);
				if (accessLevel == null)
				{
					throw new ArgumentNullException("access");
				}
				switch (Type)
				{
				case EnDepotReceiptType.Receipt:
					if (accessLevel.DepotReceipt.AllowSaveWithoutInvoiceMatch)
					{
						return res;
					}
					break;
				case EnDepotReceiptType.Havale:
					if (accessLevel.DepotHavale.AllowSaveWithoutInvoiceMatch)
					{
						return res;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				OrderReportBusiness orderReportBusiness = await OrderBusiness.GetReportAsync(ContextGuid, OrderGuid.Value, token, tranName);
				if (orderReportBusiness == null)
				{
					return res.AddError("فاکتور پیوست شده یافت نشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 274);
				}
				OrderBusiness order = await OrderBusiness.GetAsync(ContextGuid, orderReportBusiness.Guid, orderReportBusiness.FactorType, fromDatabase: true, token, tranName);
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await (await OrderComparerWrapper.GetFactoryAsync(ContextGuid, order)).CompareAsync(this);
				if (returnedSaveFuncInfo2.HasError)
				{
					return res.AddError("عملیات ثبت " + Type.GetDisplay() + " به دلیل عدم تطبیق اقلام با فاکتور مرجع و نداشتن مجوز لازم امکان\u200cپذیر نیست \r\n " + returnedSaveFuncInfo2.ErrorMessage, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 280);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 285);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 286);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 6;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DepotReceiptBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 300);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 301), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 301);
							}
							transactionHasBeenCreated = true;
							if (DetailList?.Any() ?? false)
							{
								foreach (DepotReceiptDetailBusiness item in DetailList.ToList())
								{
									if (item.Quantity <= 0m)
									{
										item.Dispose();
									}
								}
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 309);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: false, this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 311);
								if (res.HasError)
								{
									result = res;
								}
								else
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.DepotReceipt.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 313);
									if (res.HasError)
									{
										result = res;
									}
									else
									{
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 315), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 315);
										if (!res.HasError)
										{
											returnedSaveFuncInfo = res;
											returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document, DateM, 1, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 317);
											goto end_IL_014c;
										}
										result = res;
									}
								}
							}
						}
						goto end_IL_009c;
						end_IL_014c:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 321);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 322);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 327), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 327);
				}
				if (!res.HasError)
				{
					ChangeStatus = EntityChangeSt.Edited;
				}
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 5;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 340), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 340);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 342);
				if (res.HasError)
				{
					result = res;
					goto IL_06c4;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: true, this, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 344);
				if (res.HasError)
				{
					result = res;
					goto IL_06c4;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.DepotReceipt.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 346);
				if (res.HasError)
				{
					result = res;
					goto IL_06c4;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 348), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 348);
				if (res.HasError)
				{
					result = res;
					goto IL_06c4;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document, DateM, 10, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 350);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 354);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 355);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 360), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 360);
			}
		}
		return res;
		IL_06c4:
		return result;
	}

	public static Task<IEnumerable<DepotReceiptReportBusiness>> GetAllAsync(string contextGuid, DepotReceiptFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.GetAllAsync(filter, token, tranName);
	}

	public static Task<DepotReceiptBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.GetAsync(guid, token, tranName);
	}

	private async Task<DepotReceiptBusiness> GetOldReceiptAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_oldItem == null)
			{
				_oldItem = await ClsCache.GetCache(ContextGuid).CacheRepository.DepotReceipt.GetFromDatabaseAsync(Guid, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldReceiptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 374);
		}
		return _oldItem;
	}

	public Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		using (new JaegerMonitor("GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs"))
		{
			List<ProductsChanges> ret = new List<ProductsChanges>();
			try
			{
				DepotReceiptBusiness depotReceiptBusiness = await GetOldReceiptAsync(token, tranName);
				if (depotReceiptBusiness != null && !depotReceiptBusiness.IsTemp)
				{
					foreach (DepotReceiptDetailBusiness item in depotReceiptBusiness?.DetailList ?? new List<DepotReceiptDetailBusiness>())
					{
						CalcChanges.ComputeDepotReceiptChange(ContextGuid, item, ret, isNew: false, isDeleteAction);
					}
				}
				if (!IsTemp && !isDeleteAction)
				{
					foreach (DepotReceiptDetailBusiness item2 in DetailList ?? new List<DepotReceiptDetailBusiness>())
					{
						CalcChanges.ComputeDepotReceiptChange(ContextGuid, item2, ret, isNew: true, isDeleteAction: false);
					}
				}
				return ret?.Where((ProductsChanges q) => q.qt != 0m)?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 397);
				throw;
			}
		}
	}

	public Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> CanReportAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.Report || (accessLevel.DepotReceipt.ReportSelf && CashierGuid == cashier.Guid), 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.Report || (accessLevel.DepotHavale.ReportSelf && CashierGuid == cashier.Guid), 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 421);
			return false;
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.Insert, 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.Insert, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 440);
			return false;
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.Update || (accessLevel.DepotReceipt.Update_Self && CashierGuid == cashier.Guid), 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.Update || (accessLevel.DepotHavale.Update_Self && CashierGuid == cashier.Guid), 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 461);
			return false;
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.Delete || (accessLevel.DepotReceipt.Delete_Self && CashierGuid == cashier.Guid), 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.Delete || (accessLevel.DepotHavale.Delete_Self && CashierGuid == cashier.Guid), 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 482);
			return false;
		}
	}

	public async Task<bool> CanPrintAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.Print, 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.Print, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 501);
			return false;
		}
	}

	public async Task<bool> CanChangeToOrderAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.ChangeToOrder, 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.ChangeToOrder, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanChangeToOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 520);
			return false;
		}
	}

	public async Task<bool> CanChangeToBackOrderAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.ChangeToOrderBackSell, 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.ChangeToOrderBackBuy, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanChangeToBackOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 539);
			return false;
		}
	}

	public async Task<bool> CanChangeToAmaniOrderAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token);
			return Type switch
			{
				EnDepotReceiptType.Receipt => accessLevel.DepotReceipt.ChangeToOrderAmaniVorod, 
				EnDepotReceiptType.Havale => accessLevel.DepotHavale.ChangeToOrderAmaniKhoroj, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanChangeToAmaniOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 558);
			return false;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateRefrenceGuidAsync(string contextGuid, List<SanadAttchmentModel> model, bool awaitOnSemaphore, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					if (!(model?.Any() ?? false))
					{
						result = res;
						goto end_IL_0098;
					}
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DepotReceiptDetailBusiness/UpdateRefrenceGuidAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 574), ReturnedState.None, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 574);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.UpdateRefrenceGuidAsync(model, token, tranName), ReturnedState.None, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 576);
							goto end_IL_0159;
						}
						result = res.AddErrorTokenIsCancelled("", "", "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 573);
						goto end_IL_0098;
						end_IL_0159:;
					}
					goto end_IL_0084;
					end_IL_0098:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 580);
					res.AddReturnedValue(ex, "", "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 581);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 586), ReturnedState.None, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptBusiness.cs", 586);
				}
			}
			return res;
		}
	}

	public static Task<List<Guid>> GetAllByOrderGuidAsync(string contextGuid, Guid orderGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DepotReceipt.GetAllByOrderGuidAsync(orderGuid, token, tranName);
	}
}
