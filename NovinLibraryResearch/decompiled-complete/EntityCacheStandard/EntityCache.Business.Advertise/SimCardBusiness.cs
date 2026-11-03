using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class SimCardBusiness : ISimCard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public DateTime DivarModified { get; set; }

	public DateTime SheypoorModified { get; set; }

	public DateTime NextUseDivar { get; set; }

	public DateTime NextUseSheypoor { get; set; }

	public bool isSheypoorBlocked { get; set; }

	public bool isSendDivarAdv { get; set; }

	public bool isSendSheypoorAdv { get; set; }

	public long Number { get; set; }

	public Guid OwnerGuid { get; set; }

	public int UseCount { get; set; }

	public bool Status { get; set; }

	public string Operator { get; set; }

	public string UserName { get; set; }

	public string OwnerName { get; set; }

	public bool IsChecked { get; set; }

	public bool HasDivarToken { get; set; }

	public bool HasDivarChatToken { get; set; }

	public bool HasSheypoorToken { get; set; }

	public string ContextGuid { get; set; }

	public static Task<int> DeleteCountAsync(string contextGuid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.DeleteCountAsync(tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 51), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 51);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISimCard.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 53);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 57);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 58);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 63), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 63);
			}
		}
		return ret;
	}

	public SimCardBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<SimCardBusiness> lst, short tryCount = 10, string tranName = null)
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 80), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 80);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISimCard.SaveAsync(lst, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 82);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 86);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 87);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 92), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 92);
			}
		}
		return ret;
	}

	public static Task<bool> CheckNumberAsync(string contextGuid, long number, Guid guid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.CheckNumberAsync(number, guid, tranName);
	}

	public static Task<bool> CheckUserNameAsync(string contextGuid, string username, Guid guid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.CheckUserNameAsync(username, guid, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guid, bool status, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 111), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 111);
				}
				transactionHasBeenCreated = true;
				SimCardBusiness simCardBusiness = await GetAsync(contextGuid, guid, token);
				if (simCardBusiness == null)
				{
					ret.AddError(system, "Object is Null", "guid", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 116);
					return ret;
				}
				simCardBusiness.Status = status;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.SaveAsync(simCardBusiness, tryCount, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 121);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 125);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 126);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 131), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 131);
			}
		}
		return ret;
	}

	public static Task<List<SimCardBusiness>> GetSpecialAsync(string contextGuid, Expression<Func<ISimCard, bool>> where, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetAllSpecialAsync(where, tranName);
	}

	public static Task<long> GetNextSimCardNumberAsync(string contextGuid, short advType, short maxDailyCountDivar, short maxDailyCountSheypoor, string IP, short maxMountlyCountDivar, short maxMountlyCountSheypoor, int maxAdvCountInIpInDayDivar, int maxAdvCountInIpInDaySheypoor, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetNextNumberAsync(advType, maxDailyCountDivar, maxDailyCountSheypoor, IP, maxMountlyCountDivar, maxMountlyCountSheypoor, maxAdvCountInIpInDayDivar, maxAdvCountInIpInDaySheypoor, tranName);
	}

	public static Task<List<long>> GetSimCardsForSendAdvAsync(string contextGuid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetSimCardsForSendAdvAsync(tranName);
	}

	public static Task<string> GetOwnerNameAsync(string contextGuid, long simCardNumber, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetOwnerNameAsync(simCardNumber, tranName);
	}

	public static Task<IEnumerable<SimCardBusiness>> GetAllAsync(string contextGuid, Guid? ownerGuid = null, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetAllAsync(ownerGuid, tranName);
	}

	public static Task<SimCardBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetAsync(guid, token, tranName);
	}

	public static Task<SimCardBusiness> GetAsync(string contextGuid, long number, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.GetAsync(number, tranName);
	}

	public static async Task<List<SimCardBusiness>> GetAllAsync(string contextGuid, string search, Guid ownerGuid, CancellationToken token, short divarToken = 2, short divarChatToken = 2, short sheypoorToken = 2, bool status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs"))
		{
			IEnumerable<SimCardBusiness> enumerable2;
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				IEnumerable<SimCardBusiness> enumerable = ((!(ownerGuid == Guid.Empty)) ? (await GetAllAsync(contextGuid, ownerGuid)) : (await GetAllAsync(contextGuid)));
				enumerable2 = enumerable;
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (divarToken != 2)
				{
					switch (divarToken)
					{
					case 0:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => q.HasDivarToken);
						break;
					case 1:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => !q.HasDivarToken);
						break;
					}
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (divarChatToken != 2)
				{
					switch (divarChatToken)
					{
					case 0:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => q.HasDivarChatToken);
						break;
					case 1:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => !q.HasDivarChatToken);
						break;
					}
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (sheypoorToken != 2)
				{
					switch (sheypoorToken)
					{
					case 0:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => q.HasSheypoorToken);
						break;
					case 1:
						if (token.IsCancellationRequested)
						{
							return null;
						}
						enumerable2 = enumerable2?.Where((SimCardBusiness q) => !q.HasSheypoorToken);
						break;
					}
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item))
						{
							if (token.IsCancellationRequested)
							{
								return null;
							}
							enumerable2 = enumerable2?.Where((SimCardBusiness x) => ("0" + x.Number).Contains(item) || x.OwnerName.Contains(item) || x.Operator.Contains(item));
						}
					}
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				enumerable2 = (enumerable2?.Where((SimCardBusiness q) => q.Status == status))?.OrderBy((SimCardBusiness o) => o.OwnerName)?.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 243);
				throw;
			}
			return enumerable2?.ToList();
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAllSheypoorBlocks(string contextGuid, bool state, short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 261), ReturnedState.None, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 261);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISimCard.UpdateAllSheypoorBlocks(state, tryCount, tranName), ReturnedState.None, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 263);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 267);
				ret.AddReturnedValue(ex, "", "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 268);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 273), ReturnedState.None, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\SimCardBusiness.cs", 273);
			}
		}
		return ret;
	}
}
