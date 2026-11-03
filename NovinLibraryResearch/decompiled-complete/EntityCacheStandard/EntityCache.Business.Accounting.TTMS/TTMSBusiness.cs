using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TTMS;

namespace EntityCache.Business.Accounting.TTMS;

public class TTMSBusiness : ITTMS, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<TTMSDetBusiness> _detList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string FilePath { get; set; }

	public DateTime DateM { get; set; }

	public EnSeason Priod { get; set; }

	public short Year { get; set; }

	public short State { get; set; }

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public IReadOnlyList<TTMSDetBusiness> DetailList => _detList;

	public TTMSBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public Task<TTMSDetBusiness> AddToListAsync(TTMSDetBusiness det)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TTMSDetBusiness>();
			}
			det.MasterGuid = Guid;
			_detList.Add(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 45);
		}
		return Task.FromResult(det);
	}

	public async Task<TTMSDetBusiness> AddToListAsync(Guid asGuid, Document asType)
	{
		try
		{
			if (asGuid == Guid.Empty)
			{
				return null;
			}
			TTMSDetBusiness tTMSDetBusiness = _detList?.FirstOrDefault((TTMSDetBusiness q) => q.AsGuid == asGuid && q.AsType == asType);
			if (tTMSDetBusiness != null)
			{
				return tTMSDetBusiness;
			}
			TTMSDetBusiness det = new TTMSDetBusiness(ContextGuid)
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
			WebErrorLog.Instance.StartLog(ex, "", "AddToListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 69);
			throw;
		}
	}

	public Task RemoveFromListAsync(Guid asGuid)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TTMSDetBusiness>();
			}
			TTMSDetBusiness item = _detList.FirstOrDefault((TTMSDetBusiness p) => p.AsGuid == asGuid);
			_detList.Remove(item);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 81);
		}
		return Task.CompletedTask;
	}

	public void ClearListSanad()
	{
		List<TTMSDetBusiness> detList = _detList;
		if (detList != null && detList.Any())
		{
			foreach (TTMSDetBusiness item in _detList.ToList())
			{
				item?.Dispose();
			}
		}
		_detList = new List<TTMSDetBusiness>();
	}

	public async Task AddRangeToListSanadAsync(List<TTMSDetBusiness> dets, bool appendCurrentItems = false)
	{
		try
		{
			if (_detList == null)
			{
				_detList = new List<TTMSDetBusiness>();
			}
			if (!appendCurrentItems)
			{
				ClearListSanad();
			}
			foreach (TTMSDetBusiness det in dets)
			{
				await AddToListAsync(det);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 101);
			throw;
		}
	}

	public static Task<IEnumerable<TTMSReportBusiness>> GetAllReportAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TTMS.GetAllReportAsync(token, tranName);
	}

	public static Task<TTMSBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TTMS.GetAsync(guid, token, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 115), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 115);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TTMS.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 116);
				transactionHasBeenCreated = true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 121);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 122);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 127), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 127);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs"))
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
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 141), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 141);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TTMS.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 143);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 147);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 148);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 153), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 153);
				}
			}
			return ret;
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
			IEnumerable<TTMSDetBusiness> enumerable = _detList.Where((TTMSDetBusiness q) => q.AsType == Document.OrderSell);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TTMSDetBusiness item in enumerable)
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
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderSellsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 176);
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
			IEnumerable<TTMSDetBusiness> enumerable = _detList.Where((TTMSDetBusiness q) => q.AsType == Document.OrderBackSell);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TTMSDetBusiness item in enumerable)
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
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderBackSellsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 199);
			throw;
		}
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetOrderBuyAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TTMSDetBusiness> enumerable = _detList.Where((TTMSDetBusiness q) => q.AsType == Document.OrderBuy);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TTMSDetBusiness item in enumerable)
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
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 222);
			throw;
		}
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetOrderBackBuyAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TTMSDetBusiness> enumerable = _detList.Where((TTMSDetBusiness q) => q.AsType == Document.OrderBackBuy);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderReportBusiness> lstOrder = new List<OrderReportBusiness>();
			foreach (TTMSDetBusiness item in enumerable)
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
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderBackBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 245);
			throw;
		}
	}

	public async Task<IEnumerable<OrderTtmsReport>> GetAllOrderAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_detList?.Any() ?? false))
			{
				return null;
			}
			return await OrderBusiness.GetOrderTtmsReportAsync(ContextGuid, _detList.Select((TTMSDetBusiness q) => q.AsGuid).ToList(), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSBusiness.cs", 259);
			throw;
		}
	}
}
