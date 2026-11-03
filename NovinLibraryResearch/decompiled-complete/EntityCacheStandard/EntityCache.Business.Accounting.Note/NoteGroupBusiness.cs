using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;

namespace EntityCache.Business.Accounting.Note;

public class NoteGroupBusiness : Serializable<NoteGroupBusiness>, INoteGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid ParentGuid { get; set; }

	public string Name { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public NoteGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<NoteGroupBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.GetAllAsync(token, tranName);
	}

	public static Task<List<NoteGroupBusiness>> GetAllAsync(string contextGuid, Guid parentGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.GetAllAsync(parentGuid, token, tranName);
	}

	public static Task<NoteGroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.GetAsync(guid, token, tranName);
	}

	public static Task<NoteGroupBusiness> CheckNameAsync(string contextGuid, Guid guid, string Name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.CheckNameAsync(guid, Name, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "NoteGroupBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 45);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 46), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 46);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 48);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.NoteGroupRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 50);
							goto end_IL_0121;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_0121:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 54);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 55);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 60), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 60);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, NoteGroupBusiness masterNote, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		if (masterNote == null)
		{
			throw new ArgumentNullException("masterNote");
		}
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "NoteGroupBusiness/RemoveAsync", 60000, token);
				if (dis.Token.IsCancellationRequested)
				{
					result = res.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 74);
					goto end_IL_0131;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 75), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 75);
				}
				transactionHasBeenCreated = true;
				List<NoteGroupBusiness> list = await GetAllAsync(contextGuid, masterNote.Guid, token, tranName);
				if (list != null && list.Any())
				{
					foreach (NoteGroupBusiness item in list)
					{
						if (item != null)
						{
							item.Status = false;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 85);
						}
						if (!res.HasError)
						{
							continue;
						}
						result = res;
						goto end_IL_0131;
					}
				}
				if (res.HasError)
				{
					result = res;
					goto end_IL_0131;
				}
				masterNote.Status = false;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.SaveAsync(masterNote, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 92);
				goto end_IL_0078;
				end_IL_0131:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 96);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 97);
				goto end_IL_0078;
			}
			return result;
			end_IL_0078:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 102), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 102);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeChildsStatusAsync(string contextGuid, EnSystem system, List<NoteGroupBusiness> lst, bool status, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (NoteGroupBusiness item in lst)
			{
				NoteGroupBusiness noteGroupBusiness = await GetAsync(contextGuid, item.Guid, token);
				if (noteGroupBusiness == null)
				{
					ret.AddError(system, "Object is Null", "obj", "ChangeChildsStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 116);
					return ret;
				}
				noteGroupBusiness.Status = status;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.SaveAsync(noteGroupBusiness, tryCount, token, tranName), ReturnedState.None, "ChangeChildsStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 120);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeChildsStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 125);
			ret.AddReturnedValue(ex, "", "ChangeChildsStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 126);
		}
		return ret;
	}

	public static Task<int> CountGroupChildsAsync(string contextGuid, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteGroupRepository.CountGroupChildsAsync(groupGuid, token, tranName);
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
			if (string.IsNullOrEmpty(Name))
			{
				await res.AddErrorAsync("عنوان گروه را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 138);
			}
			if (!string.IsNullOrEmpty(Name) && await CheckNameAsync(ContextGuid, Guid, Name, token, tranName) != null)
			{
				await res.AddErrorAsync("عنوان گروه تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 142);
			}
			if (Guid != Guid.Empty && Guid == ParentGuid)
			{
				await res.AddErrorAsync("گروه و زیرگروه نمی توانند یکسان باشند", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 144);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 148);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteGroupBusiness.cs", 149);
		}
		return res;
	}
}
