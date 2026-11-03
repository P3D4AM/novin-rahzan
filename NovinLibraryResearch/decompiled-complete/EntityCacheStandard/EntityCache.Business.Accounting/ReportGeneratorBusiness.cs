using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class ReportGeneratorBusiness : Serializable<ReportGeneratorBusiness>, IReportGenerator, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<ReportGeneratorDetBusiness> _detList;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string Title { get; set; }

	public string Description { get; set; } = "";


	public string Rpt { get; set; } = "";


	public int Version { get; set; } = 100;


	public DateTime LastUpdate { get; set; } = DateTime.Now;


	public StiType? Type { get; set; }

	public Document? Document { get; set; }

	public List<ReportGeneratorDetBusiness> DetList
	{
		get
		{
			return _detList;
		}
		set
		{
			_detList = value;
		}
	}

	public string ContextGuid { get; set; }

	public bool IsChecked { get; set; }

	public MaliYearSanadType MaliYearSanadType { get; set; }

	[JsonIgnore]
	public byte[] ReportBytes => Encoding.UTF8.GetBytes(Rpt);

	public ReportGeneratorBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReportGeneratorBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ReportGenerator.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 49);
			returnedSaveFuncInfo = null;
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ReportGeneratorBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							returnedSaveFuncInfo = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 56);
						}
						else
						{
							if (autoCommit)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
								returnedSaveFuncInfo2.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 57), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 57);
							}
							transactionHasBeenCreated = true;
							if (!ret.HasError)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
								returnedSaveFuncInfo2.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ReportGenerator.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 60);
								goto end_IL_01d9;
							}
							returnedSaveFuncInfo = ret;
						}
						goto end_IL_013b;
						end_IL_01d9:;
					}
					goto end_IL_0127;
					end_IL_013b:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 64);
					ret.AddReturnedValue(new ReturnedSaveFuncInfo(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 65), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 65);
					goto end_IL_0127;
				}
				return returnedSaveFuncInfo;
				end_IL_0127:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
					returnedSaveFuncInfo2.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 70), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 70);
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, IEnumerable<ReportGeneratorBusiness> lst, bool send2Server, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (ReportGeneratorBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 85);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 86);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, "", token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 98);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 99);
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
					percentCompleted(new PercenteCompletedEventArgs(lst?.Count() ?? 0, lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<ReportGeneratorBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ReportGenerator.GetAllAsync(status, token, tranName);
	}

	public static Task<List<ReportGeneratorBusiness>> GetAllAsync(string contextGuid, ReportGeneratorFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ReportGenerator.GetAllAsync(filter, token, tranName);
	}

	public static ReportGeneratorBusiness Deserialize(string contextGuid, string data)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(new ReportGeneratorBusiness(contextGuid).GetType());
			StringReader textReader = new StringReader(data);
			return (ReportGeneratorBusiness)xmlSerializer.Deserialize(textReader);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Deserialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 129);
			throw;
		}
	}

	public static async Task<List<ReportGeneratorBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<ReportGeneratorBusiness> enumerable = await GetAllAsync(contextGuid, status, token);
				if (status.HasValue)
				{
					enumerable = enumerable?.Where((ReportGeneratorBusiness q) => q.Status == status.Value);
				}
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((ReportGeneratorBusiness x) => x.Title.StringComparisons(item) || x.Description.StringComparisons(item) || x.Rpt.StringComparisons(item));
						}
					}
				}
				enumerable = enumerable?.OrderBy((ReportGeneratorBusiness o) => o.Title)?.ToList();
				return token.IsCancellationRequested ? null : enumerable?.ToList();
			}
			catch (OperationCanceledException ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 155);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 160);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 173), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 173);
					}
					transactionHasBeenCreated = true;
					if (ret.HasError)
					{
						return ret;
					}
					Status = false;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 177);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 181);
					ret.AddReturnedValue(new ReturnedSaveFuncInfo(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 182), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 182);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 187), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 187);
				}
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if ((this?.Guid ?? Guid.Empty) == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("شناسه تهی", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 196);
			}
			if (string.IsNullOrEmpty(this?.Title ?? ""))
			{
				returnedSaveFuncInfo.AddError("عنوان گزارش را مشخص کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 197);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 201);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ReportGeneratorBusiness.cs", 202);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}
}
