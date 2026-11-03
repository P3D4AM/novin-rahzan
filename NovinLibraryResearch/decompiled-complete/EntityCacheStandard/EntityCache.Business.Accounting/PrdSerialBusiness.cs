using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Core;
using EntityCache.Business.Products.ProductSerial;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Records;
using PacketParser.TransferLayer.Waiters.Accounting;

namespace EntityCache.Business.Accounting;

public class PrdSerialBusiness : BaseBusiness<PrdSerialBusiness>, IPrdSerial, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, INotifyPropertyChanged, ISyncFromClient2Server<PrdSerialBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _productName = "";

	private string _productCode = "";

	private List<PrdSerialDetailBusiness> _listDetails;

	private bool _isDisposed;

	private Guid _prdGuid = Guid.Empty;

	private CancellationTokenSource _prdToken = new CancellationTokenSource();

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	[Obsolete("for v5 please remove it", true)]
	public int RefrencedId { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid ProductGuid
	{
		get
		{
			return _prdGuid;
		}
		set
		{
			if (!(_prdGuid == value))
			{
				_prdGuid = value;
				_prdToken?.Cancel();
				_prdToken = new CancellationTokenSource(60000);
				CancellationToken token = _prdToken.Token;
				Task.Run(() => UpdateProductInfoAsync(token));
			}
		}
	}

	public string Serial { get; set; } = "";


	public string GarrantyCode { get; set; } = "";


	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	[JsonIgnore]
	public string PrdName => _productName;

	[JsonIgnore]
	public string PrdCode => _productCode;

	[JsonIgnore]
	public string StartDateSh => StartDate.MiladiToCalture();

	[JsonIgnore]
	public string EndDateSh => EndDate.MiladiToCalture();

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public bool IsMojod { get; set; }

	[JsonIgnore]
	public string StatusForDisplay
	{
		get
		{
			if (!IsMojod)
			{
				return "ناموجود";
			}
			return "موجود";
		}
	}

	public override string ContextGuid { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public IReadOnlyList<PrdSerialDetailBusiness> DetList
	{
		get
		{
			if (_listDetails == null)
			{
				_listDetails = new List<PrdSerialDetailBusiness>();
			}
			_listDetails.RemoveAll((PrdSerialDetailBusiness p) => p?.IsDisposed ?? true);
			return _listDetails;
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; } = "";


	public string DeliveryShortMessage { get; set; } = "";


	public string HostRefrenced { get; set; } = "";


	public bool IsTransfer { get; set; }

	public string TechnicalInfo1 { get; set; }

	public string TechnicalInfo2 { get; set; }

	public string TechnicalInfo3 { get; set; }

	public DateTime InsertDate { get; set; }

	[JsonIgnore]
	public byte[] TechnicalInfoImage => ImageResourceManager.Information;

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public event PropertyChangedEventHandler PropertyChanged;

	public PrdSerialBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		InsertDate = DateTime.Now;
	}

	private async Task UpdateProductInfoAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, EnSystem.None, null, "", token, true);
			_productName = productCompactBusiness?.Name;
			_productCode = productCompactBusiness?.IdProduct;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrdCode"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrdName"));
				}, "UpdateProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 120);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> CheckValidationAsync(string contextGuid, List<PrdSerialBusiness> lst, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return res;
			}
			int num = checked(lst.Count - lst.Select((PrdSerialBusiness e) => e.Guid).ToList().Distinct()
				.Count());
			if (num > 0)
			{
				res.AddError("تعداد ".GetTranslate() + " " + $"{num} " + " " + "عدد از سریال های وارد شده تکراری میباشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 130);
			}
			foreach (PrdSerialBusiness item in lst)
			{
				ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(contextGuid, item.Serial, system, true, tranName, null, token);
				if (productCompactBusiness != null)
				{
					res.AddError("سریال".GetTranslate() + item.Serial + "با کد کالای".GetTranslate() + " " + (" " + productCompactBusiness.Name + " تداخل دارد").GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 135);
				}
				BarcodeBusiness barcodeBusiness = await BarcodeBusiness.GetAsync(contextGuid, item.Serial, token, tranName);
				if (barcodeBusiness != null)
				{
					res.AddError("سریال".GetTranslate() + (item.Serial ?? "").GetTranslate() + "با بارکد کالا".GetTranslate() + " " + (barcodeBusiness.Name ?? "").GetTranslate() + " " + " تداخل دارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 138);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 143);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 144);
		}
		return res;
	}

	public void AddDet(PrdSerialDetailBusiness det)
	{
		try
		{
			if (det == null)
			{
				throw new ArgumentNullException("Detail is Null");
			}
			if (_listDetails == null)
			{
				_listDetails = new List<PrdSerialDetailBusiness>();
			}
			_listDetails.Add(det);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 159);
			throw;
		}
	}

	public void AddDetList(List<PrdSerialDetailBusiness> lstDetails, bool clearDets)
	{
		try
		{
			if (_listDetails == null)
			{
				_listDetails = new List<PrdSerialDetailBusiness>();
			}
			if (clearDets)
			{
				ClearDetList();
			}
			if (lstDetails == null)
			{
				lstDetails = new List<PrdSerialDetailBusiness>();
			}
			if (!lstDetails.Any())
			{
				return;
			}
			foreach (PrdSerialDetailBusiness lstDetail in lstDetails)
			{
				if (lstDetail != null)
				{
					AddDet(lstDetail);
				}
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 178);
			throw;
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			foreach (PrdSerialDetailBusiness item in _listDetails?.ToList() ?? new List<PrdSerialDetailBusiness>())
			{
				item?.Dispose();
			}
			_listDetails = null;
			Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 195);
		}
	}

	public void ClearDetList()
	{
		_listDetails?.Clear();
	}

	public static Task<PrdSerialBusiness> GetAsync(string contextGuid, string serialNumber, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAsync(serialNumber, token, tranName);
	}

	public static Task<PrdSerialBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAsync(guid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 210), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 210);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 212);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 216);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 217);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 222), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 222);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveByPrdGuidAsync(string contextGuid, Guid prdGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 235), ReturnedState.None, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 235);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.RemoveByPrdGuidAsync(prdGuid, tryCount, token, tranName), ReturnedState.None, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 237);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 241);
				ret.AddReturnedValue(ex, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 242);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 247), ReturnedState.None, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 247);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<PrdSerialBusiness>> GetAllAsync(string contextGuid, ProductSerialFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllAsync(filter, token, tranName);
	}

	public static Task<IEnumerable<PrdSerialDetailReportBusiness>> GetAllAsync(string contextGuid, string serial, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllAsync(serial, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer = true, bool await4send = false, Priority priority = Priority.Medium, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdSerialBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 264);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 265), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 265);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(ContextGuid, new List<PrdSerialBusiness> { this }, system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 267);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdSerial.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 269);
							goto end_IL_0188;
						}
						result = ret;
					}
					goto end_IL_00df;
					end_IL_0188:;
				}
				goto end_IL_00ce;
				end_IL_00df:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 271);
				goto end_IL_00ce;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 274);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 275);
				goto end_IL_00ce;
			}
			return result;
			end_IL_00ce:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 280), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 280);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 283);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public void RemoveFromDetList(IEnumerable<PrdSerialDetailBusiness> x)
	{
		using (new JaegerMonitor("RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs"))
		{
			try
			{
				if (_listDetails == null)
				{
					_listDetails = new List<PrdSerialDetailBusiness>();
				}
				if (x == null || !x.Any())
				{
					return;
				}
				foreach (PrdSerialDetailBusiness item in x)
				{
					_listDetails.FirstOrDefault((PrdSerialDetailBusiness p) => p.Guid == item.Guid)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 304);
				throw;
			}
		}
	}

	public static Task<PrdSerialBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<PrdSerialBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 319), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 319);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdSerial.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 321);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 323);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 326);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 327);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 332), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 332);
			}
		}
		return ret;
	}

	Task<ReturnedSaveFuncInfo> ISyncFromClient2Server<PrdSerialBusiness>.ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 5)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	internal static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
			{
				return ret;
			}
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Serial;
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال سریال کالاها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 359);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 361);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 363);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.Serial is null");
				}
				line = 52;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 53;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (list == null || !list.Any())
				{
					return ret.AddInformation("سریال کالاها سینک هستند. تعداد سریال سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 373);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 374);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 381);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 383);
						}
						PrdSerialBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "سریال جاری".GetTranslate() + ":" + obj?.Serial;
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Serial \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Serial, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 392);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 398);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 403);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 407);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 409);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"CacheIsNullOrdisposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}\r\n" + "ContextxGuid:" + contextGuid, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 412);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 415);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_resendToServerSemaphore.Release();
				}
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs"))
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache == null || !cache.BufferEnable || cache.BufferLoginState.Status != EnLoginResult.Success)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 443);
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "PrdSerialBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 448);
				}
				List<WebPrdSerial> list = new List<WebPrdSerial>();
				List<WebPrdSerial> list2 = list;
				list2.Add(await PrdSerialMapper.Instance.MapAsync(ContextGuid, this, new WebPrdSerial(), "", token));
				List<Packet> paks = WebPrdSerial.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 450);
				}
				using SerialWaiter waiter = new SerialWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("PrdSerialBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 454);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 456);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 458);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 461);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 462);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<PrdSerialBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllWithoutDetailAsync(token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (PrdSerialBusiness item in enumerable)
			{
				if (string.IsNullOrEmpty(item?.Serial))
				{
					continue;
				}
				IEnumerable<OrderDetailSerialModel> enumerable2 = await OrderBusiness.GetAllDetailSerialAsync(contextGuid, item.Serial, token, tranName);
				if (enumerable2 == null || !enumerable2.Any())
				{
					continue;
				}
				enumerable2 = enumerable2?.OrderBy((OrderDetailSerialModel q) => q.OrderDate);
				foreach (OrderDetailSerialModel item2 in enumerable2)
				{
					PrdSerialDetailBusiness prdSerialDetailBusiness = new PrdSerialDetailBusiness(contextGuid)
					{
						AsType = item2.OrderType,
						ContextGuid = contextGuid,
						DetailGuid = item2.OrderDetailGuid,
						Guid = Guid.NewGuid(),
						MasterGuid = item.Guid,
						Modified = DateTime.Now,
						Ordinal = 0,
						SanadDate = item2.OrderDate
					};
					if (item2.OrderType == Document.OrderBuy || item2.OrderType == Document.OrderBackSell || item2.OrderType == Document.Tolid_Mahsul)
					{
						prdSerialDetailBusiness.IsVorod = true;
					}
					else if (item2.OrderType == Document.OrderSell || item2.OrderType == Document.OrderBackBuy || item2.OrderType == Document.Tolid_Mavad)
					{
						prdSerialDetailBusiness.IsVorod = false;
					}
					item.AddDet(prdSerialDetailBusiness);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(EnSystem.None, sendToServer: true, await4send: false, Priority.Medium, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 498);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 503);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 504);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem System, CancellationToken token, List<PrdSerialBusiness> items, bool awaitOnSemaphore = true, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (PrdSerialBusiness serial in items)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await serial.SaveAsync(System, sendToServer: true, await4send: false, Priority.Medium, token, awaitOnSemaphore, 10, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 519);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(items.Count(), current, serial.Serial, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 531);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialBusiness.cs", 532);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(items?.Count() ?? 0, items?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[طرف حساب]"),
			new SmsTemplateRecord("[سریال]"),
			new SmsTemplateRecord("[کد گارانتی]"),
			new SmsTemplateRecord("[تاریخ اتمام گارانتی]")
		};
	}

	public static Task<IEnumerable<ProductSerialTechnicalInfoSuggestModel>> GetAllTechnicalSuggestionAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllTechnicalSuggestionAsync(token, tranName);
	}

	public static Task<IEnumerable<PrdSerialReportBusiness>> GetAllModelAsync(string contextGuid, ProductSerialFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSerial.GetAllModelAsync(filter, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IPrdSerial;
	}
}
