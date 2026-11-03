using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.OrderDepot;

public class OrderAbadBusiness : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<AbadDetBusiness> _detList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid RefrencedGuid { get; set; }

	public Document OrderType { get; set; }

	public Guid DetailGuid { get; set; }

	public ShapeInfo ShapeInfo { get; set; }

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	[Obsolete]
	public int RefrencedID { get; set; }

	public IReadOnlyList<AbadDetBusiness> ListAbad
	{
		get
		{
			_detList = _detList?.Where((AbadDetBusiness p) => p != null)?.ToList() ?? new List<AbadDetBusiness>();
			return _detList;
		}
	}

	public OrderAbadBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public string GetDescription()
	{
		using (new JaegerMonitor("GetDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs"))
		{
			try
			{
				string text = "";
				IReadOnlyList<AbadDetBusiness> listAbad = ListAbad;
				if (listAbad == null || !listAbad.Any())
				{
					return text;
				}
				foreach (AbadDetBusiness item in ListAbad)
				{
					if (item != null)
					{
						text = text + item.GetDescription() + "\r\n";
					}
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 52);
				throw;
			}
		}
	}

	public decimal GetQuantity()
	{
		using (new JaegerMonitor("GetQuantity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs"))
		{
			try
			{
				decimal result = default(decimal);
				IReadOnlyList<AbadDetBusiness> listAbad = ListAbad;
				if (listAbad == null || !listAbad.Any())
				{
					return result;
				}
				foreach (AbadDetBusiness item in ListAbad)
				{
					if (item != null)
					{
						switch (ShapeInfo)
						{
						case ShapeInfo.Mohit:
							result += item.TotalMohit;
							break;
						case ShapeInfo.Mohit_Masahat:
						case ShapeInfo.Masahat:
							result += item.TotalMasahat;
							break;
						}
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetQuantity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 79);
				throw;
			}
		}
	}

	public static Task<OrderAbadBusiness> GetAsync(string contextGuid, Guid DetailGuid, Document OrderType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrderAbad.GetAsync(DetailGuid, OrderType, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid OrderGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 93), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 93);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrderAbad.RemoveAsync(OrderGuid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 95);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 99);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 100);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 105), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 105);
			}
		}
		return ret;
	}

	public void AddRangeToDetList(List<AbadDetBusiness> detList, bool clearDets)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs"))
		{
			try
			{
				if (detList == null || !detList.Any())
				{
					return;
				}
				if (_detList == null)
				{
					_detList = new List<AbadDetBusiness>();
				}
				if (clearDets)
				{
					_detList?.Clear();
				}
				foreach (AbadDetBusiness det in detList)
				{
					det.MasterGuid = Guid;
				}
				_detList.AddRange(detList);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderAbadBusiness.cs", 124);
				throw;
			}
		}
	}
}
