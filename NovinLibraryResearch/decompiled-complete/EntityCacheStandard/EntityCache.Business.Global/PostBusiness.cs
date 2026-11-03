using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Mapping.Global;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class PostBusiness : IPost, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public byte[] Image { get; set; }

	public string Link { get; set; }

	public DateTime Startdate { get; set; }

	public DateTime EndDate { get; set; }

	public string Title { get; set; }

	public EnTypePost Type { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public short Ordinal { get; set; }

	public PostBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<PostBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Post.GetAllAsync(token, tranName);
	}

	public static Task<PostBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Post.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PostBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 46);
					}
					else
					{
						ClsCache cache = ClsCache.GetCache(ContextGuid);
						if (cache == null)
						{
							result = ret;
						}
						else
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await cache.GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 49), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 49);
							}
							transactionHasBeenCreated = true;
							if (!ret.HasError)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await cache.CacheRepository.Post.SaveAsync(this, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 52);
								goto end_IL_011b;
							}
							result = ret;
						}
					}
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 56);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 57);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 62), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 62);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PostBusiness> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (PostBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 74);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 75);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 80);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 81);
		}
		return ret;
	}

	public static async Task PourDataToLocalAsync(string contextGuid, ENSource source, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<WebPost> source2 = (await WebPost.GetAllBySourceAsync(source))?.Where((WebPost q) => q.EndDate >= DateTime.Now)?.ToList();
			List<PostBusiness> lst = await PostMapper.Instance.MapListAsync(contextGuid, source2, null, "", token);
			if (ClsCache.GetCache(contextGuid) != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveRangeAsync(lst, token), ReturnedState.None, "PourDataToLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 95);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourDataToLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 99);
			res.AddReturnedValue(ex, "", "PourDataToLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PostBusiness.cs", 100);
		}
	}
}
