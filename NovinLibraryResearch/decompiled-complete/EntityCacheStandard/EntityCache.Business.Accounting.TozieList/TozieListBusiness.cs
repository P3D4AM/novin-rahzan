using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace EntityCache.Business.Accounting.TozieList;

public class TozieListBusiness : ITozieList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<TozieListDetailBusiness> _detList;

	private Guid guid = Guid.NewGuid();

	public Guid Guid
	{
		get
		{
			return guid;
		}
		set
		{
			guid = value;
			List<TozieListDetailBusiness> detList = _detList;
			if (detList == null || !detList.Any())
			{
				return;
			}
			foreach (TozieListDetailBusiness det in _detList)
			{
				det.AsGuid = guid;
			}
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public int Number { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => DateM.ToShortTimeString();

	public string DriverName { get; set; }

	public string DriverMobile1 { get; set; }

	public string DriverMobile2 { get; set; }

	public string CarNumber { get; set; }

	public string Description { get; set; }

	public Guid CashierGuid { get; set; }

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public IReadOnlyList<TozieListDetailBusiness> DetailList => _detList;

	public EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	public TozieListBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<TozieListDetailBusiness> AddToListAsync(TozieListDetailBusiness det)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TozieListDetailBusiness>();
			}
			det.MasterGuid = Guid;
			_detList.Add(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 57);
		}
		return Task.FromResult(det);
	}

	public async Task<TozieListDetailBusiness> AddToListAsync(Guid asGuid, Document asType)
	{
		try
		{
			if (asGuid == Guid.Empty)
			{
				return null;
			}
			TozieListDetailBusiness tozieListDetailBusiness = _detList?.FirstOrDefault((TozieListDetailBusiness q) => q.AsGuid == asGuid && q.AsType == asType);
			if (tozieListDetailBusiness != null)
			{
				return tozieListDetailBusiness;
			}
			TozieListDetailBusiness det = new TozieListDetailBusiness(ContextGuid)
			{
				AsGuid = asGuid,
				AsType = asType,
				Guid = Guid.NewGuid(),
				Status = true,
				MasterGuid = Guid,
				Modified = DateTime.Now
			};
			return await AddToListAsync(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 81);
			throw;
		}
	}

	public Task RemoveFromListAsync(Guid asGuid)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TozieListDetailBusiness>();
			}
			TozieListDetailBusiness item = _detList.FirstOrDefault((TozieListDetailBusiness p) => p.AsGuid == asGuid);
			_detList.Remove(item);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 93);
		}
		return Task.CompletedTask;
	}

	public void ClearListSanad()
	{
		List<TozieListDetailBusiness> detList = _detList;
		if (detList != null && detList.Any())
		{
			foreach (TozieListDetailBusiness item in _detList.ToList())
			{
				item?.Dispose();
			}
		}
		_detList = new List<TozieListDetailBusiness>();
	}

	public async Task AddRangeToListSanadAsync(List<TozieListDetailBusiness> dets, bool appendCurrentItems = false)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TozieListDetailBusiness>();
			}
			if (!appendCurrentItems)
			{
				ClearListSanad();
			}
			foreach (TozieListDetailBusiness det in dets)
			{
				await AddToListAsync(det);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 113);
			throw;
		}
	}

	public static Task<IEnumerable<TozieListRepostBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TozieList.GetAllReportAsync(token, tranName);
	}

	public static Task<TozieListBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TozieList.GetAsync(guid, token, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 127), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 127);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TozieList.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 128);
				transactionHasBeenCreated = true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 133);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 134);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 139), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 139);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 153), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 153);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TozieList.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 155);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 159);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 160);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 165), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 165);
				}
			}
			return ret;
		}
	}

	public static Task<int> GetNextNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TozieList.GetNextNumberAsync(token, tranName);
	}

	public static async Task<TozieListBusiness> GenerateWithDefualtsAsync(string contextGuid, DateTime? date = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			TozieListBusiness item = new TozieListBusiness(contextGuid);
			if (!date.HasValue)
			{
				date = DateManagment.DateM;
			}
			item.DateM = date.Value;
			TozieListBusiness tozieListBusiness = item;
			tozieListBusiness.Number = await GetNextNumberAsync(contextGuid, token, tranName);
			item.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateWithDefualtsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 186);
			throw;
		}
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetOrderSellsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TozieListDetailBusiness> enumerable = _detList.Where((TozieListDetailBusiness q) => q.AsType == Document.OrderSell);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TozieListDetailBusiness item in enumerable)
			{
				if (item != null && !(item.AsGuid == Guid.Empty))
				{
					OrderReportBusiness orderReportBusiness = await OrderBusiness.GetReportAsync(ContextGuid, item.AsGuid, token, tranName);
					if (orderReportBusiness != null)
					{
						lstOrder.Add(orderReportBusiness);
					}
				}
			}
			return lstOrder;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderSellsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 209);
			throw;
		}
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetOrderBackSellsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TozieListDetailBusiness> enumerable = _detList.Where((TozieListDetailBusiness q) => q.AsType == Document.OrderBackSell);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TozieListDetailBusiness item in enumerable)
			{
				if (item != null && !(item.AsGuid == Guid.Empty))
				{
					OrderReportBusiness orderReportBusiness = await OrderBusiness.GetReportAsync(ContextGuid, item.AsGuid, token, tranName);
					if (orderReportBusiness != null)
					{
						lstOrder.Add(orderReportBusiness);
					}
				}
			}
			return lstOrder;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderBackSellsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 232);
			throw;
		}
	}

	public async Task<IEnumerable<ReceptionReportBusiness>> GetReceptionsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TozieListDetailBusiness> enumerable = _detList.Where((TozieListDetailBusiness q) => q.AsType == Document.Reception);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<ReceptionReportBusiness> lstReception = new List<ReceptionReportBusiness>();
			foreach (TozieListDetailBusiness item in enumerable)
			{
				if (item != null && !(item.AsGuid == Guid.Empty))
				{
					ReceptionReportBusiness receptionReportBusiness = await ReceptionBusiness.GetReportAsync(ContextGuid, item.AsGuid, token, tranName);
					if (receptionReportBusiness != null)
					{
						lstReception.Add(receptionReportBusiness);
					}
				}
			}
			return lstReception;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListBusiness.cs", 255);
			throw;
		}
	}
}
