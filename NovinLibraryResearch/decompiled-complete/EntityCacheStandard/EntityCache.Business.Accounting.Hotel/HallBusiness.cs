using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Business.Accounting.Bill.Reports;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using PacketParser.Filters.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class HallBusiness : IHall, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public string Title { get; set; }

	[JsonIgnore]
	public string FullName { get; set; }

	public Guid? MasterGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public HallBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<HallBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Hall.GetAsync(guid, token, tranName);
	}

	public static Task<HallBusiness> GetAsync(string contextGuid, string title, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Hall.GetAsync(title, token, tranName);
	}

	public static async Task<IEnumerable<HallBusiness>> GetAllAsync(string contextGuid, HallFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<HallBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.Hall.GetAllAsync(filter, token, tranName);
			if (!(list?.Any() ?? false))
			{
				return list;
			}
			await Task.WhenAll(list.Select((HallBusiness q) => q.GetFullNameAsync(token)));
			if (filter?.IsLoadGuidEmpty ?? false)
			{
				List<HallBusiness> list2 = list?.ToList();
				if (list2 == null)
				{
					list2 = new List<HallBusiness>();
				}
				list2.Add(new HallBusiness(contextGuid)
				{
					ContextGuid = contextGuid,
					Guid = Guid.Empty,
					Status = true,
					Title = "[انتخاب نشده]"
				});
				list = list2;
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 61);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "HallBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 74);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 75), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 75);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 77);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Hall.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 79);
							goto end_IL_011c;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_011c:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 81);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 84);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 85);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 90), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 90);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Title))
			{
				await ret.AddErrorAsync("عنوان وارد شده معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 99);
			}
			HallBusiness hallBusiness = await GetAsync(ContextGuid, Title, token, tranName);
			if (hallBusiness != null && hallBusiness.Title == Title && hallBusiness.Guid != Guid)
			{
				await ret.AddErrorAsync("عنوان".GetTranslate() + " " + (Title ?? "").GetTranslate() + " " + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 102);
			}
			if (!Status)
			{
				IEnumerable<HallBusiness> enumerable = await GetAllAsync(ContextGuid, new HallFilter
				{
					MasterGuid = Guid
				}, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					await ret.AddErrorAsync("سالن".GetTranslate() + " " + (Title ?? "").GetTranslate() + " " + " به علت داشتن".GetTranslate() + " " + $"{enumerable?.Count() ?? 0}" + " " + " سالن زیرمجموعه فعال، قادر به حذف نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 107);
				}
				RoomFilter filter = new RoomFilter
				{
					MasterGuid = Guid,
					Status = true
				};
				IEnumerable<PlacementReportsBusiness> enumerable2 = await PlacementBusiness.GetAllAsync(ContextGuid, filter, token, tranName);
				if (enumerable2 != null && enumerable2.Any())
				{
					await ret.AddErrorAsync("سالن".GetTranslate() + " " + (Title ?? "").GetTranslate() + " " + "به علت داشتن".GetTranslate() + " " + $" {enumerable2?.Count() ?? 0}" + " " + " محل پذیرایی فعال، قادر به حذف نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 116);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 121);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 122);
		}
		return ret;
	}

	private async Task GetFullNameAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!string.IsNullOrEmpty(FullName))
			{
				return;
			}
			FullName = Title;
			if (!((MasterGuid ?? Guid.Empty) != Guid.Empty))
			{
				return;
			}
			HallBusiness hallBusiness = await GetAsync(ContextGuid, MasterGuid.Value, token);
			while (hallBusiness != null)
			{
				FullName = hallBusiness.Title + " / " + FullName;
				while (hallBusiness != null)
				{
					if ((hallBusiness.MasterGuid ?? Guid.Empty) == Guid.Empty)
					{
						hallBusiness = null;
						continue;
					}
					hallBusiness = await GetAsync(ContextGuid, hallBusiness.MasterGuid.Value, token);
					if (hallBusiness != null)
					{
						FullName = (hallBusiness.Title ?? "").GetTranslate() + "/" + (" " + FullName).GetTranslate();
					}
					hallBusiness = null;
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Bill\\HallBusiness.cs", 153);
		}
	}
}
