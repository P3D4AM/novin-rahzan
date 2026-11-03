using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Filters;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Models.Accounting.Prd;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Serilog;

namespace EntityCache.Business;

public class PrdGroupBusiness : BaseBusiness<PrdGroupBusiness>, IPrdGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<PrdGroupBusiness>, IDeliverableEntity, IEditableDelivery, IHasChangeStatus
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _description = "";

	private string _htmlFull;

	private string _htmlQuick;

	private string _name;

	private string _pic;

	private string _techInfoData;

	private PrdGroupTechnicalInformations _techInfo;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public string HtmlFull
	{
		get
		{
			return _htmlFull;
		}
		set
		{
			_htmlFull = value?.FixString() ?? "";
		}
	}

	public string HtmlQuick
	{
		get
		{
			return _htmlQuick;
		}
		set
		{
			_htmlQuick = value?.FixString() ?? "";
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value?.FixString() ?? "";
		}
	}

	public string ShortName { get; set; }

	public string Pic
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(_pic))
				{
					return "noImage.jpg";
				}
				return _pic;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Pic", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 55);
				return null;
			}
		}
		set
		{
			_pic = value.FixString();
		}
	}

	public bool Special { get; set; }

	public bool Status { get; set; }

	public string TechInfoData
	{
		get
		{
			return _techInfoData;
		}
		set
		{
			_techInfoData = value.FixString();
			_techInfo = null;
		}
	}

	public Guid? ParentGuid { get; set; }

	[JsonIgnore]
	public PrdGroupBusiness Parent
	{
		get
		{
			if (ParentGuid.HasValue)
			{
				return Get(ContextGuid, ParentGuid.Value);
			}
			return null;
		}
	}

	public int Level { get; set; }

	public string Slug { get; set; }

	public long Code { get; set; }

	public long FullCode { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	public EnProductType Type { get; set; }

	[JsonIgnore]
	public string FullGroupParentName { get; private set; }

	[JsonIgnore]
	public PrdGroupTechnicalInformations TechInfo
	{
		get
		{
			if (string.IsNullOrEmpty(TechInfoData))
			{
				return null;
			}
			return _techInfo ?? (_techInfo = TechInfoData.FromJson<PrdGroupTechnicalInformations>("TechInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 95));
		}
	}

	[JsonIgnore]
	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public PrdGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, short autoFixValidationErrorsCount, bool send2Server, Priority priority = Priority.Low, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", List<FileInfoBusiness> fileInfoList = null, bool awaitOnSend = false)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 2;
		Stopwatch.StartNew();
		try
		{
			_ = 6;
			ReturnedSaveFuncInfo result;
			try
			{
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdGroupBusiness/SaveAsync", 60000, token))
				{
					line = 3;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 136);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 137), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 137);
						}
						transactionHasBeenCreated = true;
						line = 4;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(autoFixValidationErrorsCount, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 140);
						if (system == EnSystem.SignalR)
						{
							ret.throwExceptionIfError();
						}
						line = 5;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (Guid == Guid.Empty)
							{
								Guid = Guid.NewGuid();
							}
							if (!(fileInfoList?.Any() ?? false))
							{
								goto IL_0567;
							}
							foreach (FileInfoBusiness fileInfo in fileInfoList)
							{
								fileInfo.EntityGuid = Guid;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await fileInfo.SaveAsync(system, autoFixValidationErrorsCount, send2Server, priority, awaitOnSemaphore: false, tryCount, "", awair4send: false, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 150);
							}
							if (!ret.HasError)
							{
								goto IL_0567;
							}
							result = ret;
						}
					}
					goto end_IL_0109;
					IL_0567:
					line = 6;
					if (string.IsNullOrEmpty(ShortName))
					{
						ShortName = Name;
					}
					ChangeStatus = EntityChangeSt.Edited;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductGroup.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 157);
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await UpdateSubGroupAsync(Guid, Special, TechInfoData, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 160);
						if (!ret.HasError && system == EnSystem.UserInterface && Special && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateSpecialAsync(ContextGuid, Guid, Special, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 162);
							goto end_IL_01df;
						}
						result = ret;
					}
					goto end_IL_0109;
					end_IL_01df:;
				}
				goto end_IL_00f5;
				end_IL_0109:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Line iS error Has Line :{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 166);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 167);
				goto end_IL_00f5;
			}
			return result;
			end_IL_00f5:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 172), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 172);
			}
			if (!ret.HasError && send2Server)
			{
				if (awaitOnSend)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 175);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebPrdGroup webEntity = await PrdGroupMapper.Instance.MapAsync(ContextGuid, this, new WebPrdGroup());
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.ProductGroup;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = $"خطا در ذخیره گروه کالا ({Name}) - {Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 212);
		}
	}

	public async Task<string> GetPicUrlPathAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			FileInfoBusiness fileInfoBusiness = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicPrdGroup, token))?.SingleOrDefault();
			if (fileInfoBusiness != null && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				if (fileInfoBusiness.ServerStatus == ServerStatusForClient.None)
				{
					return fileInfoBusiness.GetLocalFilePath();
				}
				return fileInfoBusiness.Url;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPicUrlPathAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 224);
		}
		return "";
	}

	public async Task<string> GetPicFullPathAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			FileInfoBusiness fileInfoBusiness = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicPrdGroup, token))?.SingleOrDefault();
			if (fileInfoBusiness != null && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				if (fileInfoBusiness.ServerStatus == ServerStatusForClient.None)
				{
					return fileInfoBusiness.GetLocalFilePath();
				}
				return fileInfoBusiness.Url;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPicFullPathAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 237);
		}
		return "";
	}

	public Task<List<FileInfoBusiness>> GetPicAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicPrdGroup, cancellationToken, "", justFromCache: false, deCompressed: true);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAndBajeAsync(string contextGuid, EnSystem system, List<PrdGroupBusiness> lst, CancellationToken token, bool awaitOnSemaphore)
	{
		string tranName = "";
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 2;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdGroupBusiness/SaveAsync", 60000, token))
				{
					line = 3;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 257);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 258), ReturnedState.None, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 258);
						}
						transactionHasBeenCreated = true;
						line = 4;
						if (!ret.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.SaveRangeAsync(lst, 10, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 262);
							goto end_IL_0150;
						}
						result = ret;
					}
					goto end_IL_00b4;
					end_IL_0150:;
				}
				goto end_IL_00a3;
				end_IL_00b4:;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 264);
				goto end_IL_00a3;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 267);
				ret.AddReturnedValue(ex2, "", "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 268);
				goto end_IL_00a3;
			}
			return result;
			end_IL_00a3:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 273), ReturnedState.None, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 273);
			}
			if (ret.HasError && lst.Count() == 1)
			{
				ret = await lst.Single().SaveAsync(system, token, 10, send2Server: false, Priority.Low, awaitOnSemaphore: true, 10);
				if (ret.HasError)
				{
					WebErrorLog.Instance.StartLog("یک دسته گروه کالا درست ام رسید - اینقدر شکوندمشون تا بالاخره فهمیدم این گروه کالا به خطا میخوره دری ثبت اش\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(lst), "", "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 278);
				}
			}
			else if (ret.HasError && lst.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<PrdGroupBusiness> list2 = new List<PrdGroupBusiness>();
				PrdGroupBusiness[] array = lst.ToArray();
				for (int i = 0; i < lst.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAndBajeAsync(contextGuid, system, list2, token, awaitOnSemaphore: true));
				list.Add(SaveRangeForHesabyarAndBajeAsync(contextGuid, system, lst.Except(list2).ToList(), token, awaitOnSemaphore: true));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAndBajeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 291);
			}
			Console.WriteLine($"SaveProductGroup finishes Result:{!ret.HasError} Time:{stopwatch.ElapsedMilliseconds}(ms)");
		}
		return ret;
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetPrdGroupBusinessesAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			return await ClsCache.GetCache(ContextGuid).CacheRepository.IProductGroup.GetChildrenAsync(Guid, token, "");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdGroupBusinessesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 303);
		}
		return null;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guid, bool status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			PrdGroupBusiness obj = await GetAsync(contextGuid, guid, token, tranName);
			if (obj == null)
			{
				return res;
			}
			if (obj.Status == status)
			{
				return res.AddWarning("وضعیت قدیم و جدید هر دو یکسال هست بنابراین تغییری ایجاد نمیشود.", "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 313);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckChildAndParentAsync(contextGuid, guid, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 314);
			if (res.HasError)
			{
				return res;
			}
			obj.Status = status;
			obj.ServerStatus = ServerStatusForClient.None;
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, token, 0, send2Server: true, Priority.Low, awaitOnSemaphore: true, 10), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 318);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 322);
			res.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 323);
		}
		return res;
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetAllPrdGroupBusinessesAndSelfAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		List<PrdGroupBusiness> list = null;
		try
		{
			list = (await GetPrdGroupBusinessesAsync(token))?.ToList();
			if (list == null || list.Count() == 0)
			{
				list = new List<PrdGroupBusiness>();
			}
			else
			{
				for (int count = 0; count != list.Count(); count = checked(count + 1))
				{
					if (await list[count].HasChildrenAsync(token))
					{
						List<PrdGroupBusiness> list2 = list;
						list2.AddRange(await list[count].GetPrdGroupBusinessesAsync(token));
					}
				}
			}
			list.Add(this);
			list = list.Where((PrdGroupBusiness w) => w.Status)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPrdGroupBusinessesAndSelfAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 350);
		}
		return await ApplyAccessAsync(ContextGuid, list, system, token);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, List<PrdGroupBusiness> lst, short autoFixValidationErrorsCount, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), bool awaitOnSend = true, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			ENSource source = ClsCache.GetCache(contextGuid).Source;
			if (source == ENSource.Android_HesabYar || source == ENSource.Baje)
			{
				return await SaveRangeForHesabyarAndBajeAsync(contextGuid, system, lst, token, awaitOnSemaphore: true);
			}
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			int current = 0;
			foreach (PrdGroupBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 368);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, autoFixValidationErrorsCount, send2Server, priority, awaitOnSemaphore: true, tryCount, tranName, null, awaitOnSend), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 369);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 381);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 382);
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

	public async Task<bool> HasChildrenAsync(CancellationToken token = default(CancellationToken))
	{
		return (await GetPrdGroupBusinessesAsync(token))?.Any() ?? false;
	}

	public async Task<string> WebImageAsync()
	{
		try
		{
			return Path.Combine((await UserInformationBusiness.GetAsync(ContextGuid)).RelativePath, FileManagement_Parts.PicPrdGroup.GetRemoteFileAttribute());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 409);
			throw;
		}
	}

	public static async Task<bool> CheckGuidAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAsync(guid, token, tranName) != null;
	}

	public static async Task<ProductGroupModel> GetAllAsync(string contextGuid, ProductGroupFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new ArgumentException("contextGuid is null or empty!");
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			_ = 2;
			try
			{
				List<PrdGroupBusiness> x = new List<PrdGroupBusiness>();
				if (filter?.LoadParentWithGuidEmpty ?? false)
				{
					x.Add(new PrdGroupBusiness(contextGuid)
					{
						Name = "[" + "کلیه گروه ها".GetTranslate() + "]",
						Description = "همه کالاها",
						Status = true,
						ParentGuid = Guid.Empty,
						Special = false,
						ChangeStatus = EntityChangeSt.Edited
					});
				}
				ProductGroupModel result = new ProductGroupModel();
				IEnumerable<PrdGroupBusiness> enumerable = await (ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductGroup?.GetAllAsync(filter?.Status, token, tranName));
				if ((filter?.Type).GetValueOrDefault(EnProductType.None) != EnProductType.None)
				{
					enumerable = enumerable?.Where((PrdGroupBusiness q) => q.Type == filter.Type);
				}
				result.AllDbCount = enumerable?.Count() ?? 0;
				if (filter?.LoadJustParents ?? false)
				{
					enumerable = enumerable?.Where((PrdGroupBusiness w) => (w.ParentGuid ?? Guid.Empty) == Guid.Empty && w.Status == (filter?.Status).GetValueOrDefault());
				}
				enumerable = enumerable?.OrderBy((PrdGroupBusiness o) => o.Name);
				if (enumerable != null)
				{
					x.AddRange(enumerable);
				}
				if (filter?.FillFullGroupName ?? false)
				{
					await Task.WhenAll(x.Select((PrdGroupBusiness prdGroup) => prdGroup.GetFullGroupParentName(token)));
				}
				ProductGroupModel productGroupModel = result;
				productGroupModel.List = await ApplyAccessAsync(contextGuid, x, filter?.System ?? EnSystem.None, token);
				return result;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 447);
				throw;
			}
		}
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 460), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 460);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 461);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 465);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 466);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 471), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 471);
			}
		}
		return ret;
	}

	public static Task<PrdGroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAsync(guid, token, tranName);
	}

	internal static async Task<List<Guid>> GroupAndSubGroupsGuidsAsync(string contextGuid, EnSystem system, Guid Guid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GroupAndSubGroupsGuidsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			_ = 2;
			try
			{
				List<Guid> ret = new List<Guid> { Guid };
				ProductGroupFilter filter = new ProductGroupFilter
				{
					System = system
				};
				List<PrdGroupBusiness> list = (await GetAllAsync(contextGuid, filter, token))?.List?.Where((PrdGroupBusiness p) => p.ParentGuid == Guid)?.ToList();
				if (list != null && list.Count > 0)
				{
					foreach (PrdGroupBusiness item in list)
					{
						_ = item;
						PrdGroupBusiness prdGroupBusiness = await GetAsync(contextGuid, Guid, token);
						if (prdGroupBusiness == null)
						{
							continue;
						}
						IEnumerable<PrdGroupBusiness> enumerable = await prdGroupBusiness.GetAllPrdGroupBusinessesAndSelfAsync(system, token);
						if (enumerable != null && enumerable.Count() > 0)
						{
							ret.AddRange(enumerable.Select((PrdGroupBusiness s) => s.Guid));
						}
					}
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GroupAndSubGroupsGuidsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 500);
				throw;
			}
		}
	}

	private async Task GetFullGroupParentName(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!string.IsNullOrEmpty(FullGroupParentName))
			{
				return;
			}
			FullGroupParentName = Name;
			if (!((ParentGuid ?? Guid.Empty) != Guid.Empty))
			{
				return;
			}
			PrdGroupBusiness prdGroupBusiness = await GetAsync(ContextGuid, ParentGuid.Value, token);
			while (prdGroupBusiness != null)
			{
				FullGroupParentName = prdGroupBusiness.Name + "/" + FullGroupParentName;
				while (prdGroupBusiness != null)
				{
					if ((prdGroupBusiness.ParentGuid ?? Guid.Empty) == Guid.Empty)
					{
						prdGroupBusiness = null;
						continue;
					}
					prdGroupBusiness = await GetAsync(ContextGuid, prdGroupBusiness.ParentGuid.Value, token);
					if (prdGroupBusiness != null)
					{
						FullGroupParentName = prdGroupBusiness.Name + "/" + FullGroupParentName;
					}
					prdGroupBusiness = null;
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetFullGroupParentName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 531);
		}
	}

	public static async Task<bool> CheckChildAsync(string contextGuid, EnSystem system, Guid guid, CancellationToken token = default(CancellationToken))
	{
		List<Guid> obj = await GroupAndSubGroupsGuidsAsync(contextGuid, system, guid, token);
		return obj != null && obj.Count > 0;
	}

	public static async Task<ReturnedSaveFuncInfo> CheckChildAndParentAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			List<PrdGroupBusiness> list = await (await GetAsync(contextGuid, guid, token, tranName)).GetGroupAndChildsAsync(EnSystem.None, token);
			if (list != null && list.Any())
			{
				foreach (PrdGroupBusiness item in list)
				{
					if ((item?.Guid ?? Guid.Empty) != Guid.Empty && item.Guid != guid && item.Status)
					{
						ret.AddError("گروه دارای زیر گروه فعال میباشد.\r\n" + item.Name, "", "CheckChildAndParentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 550);
					}
				}
			}
			ProductCompactFilter filter = new ProductCompactFilter
			{
				GroupGuids = new List<Guid> { guid },
				Sort = (cashier?.Settings?.Products?.ProductSort).GetValueOrDefault()
			};
			IEnumerable<ProductCompactBusiness> enumerable = await ProductCompactBusiness.GetAllAsync(contextGuid, EnSystem.None, filter, token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				ret.AddError($"تعداد {enumerable.Count()} کالای فعال در این گروه وجود دارد.", "", "CheckChildAndParentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 560);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckChildAndParentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 564);
			ret.AddReturnedValue(ex, "", "CheckChildAndParentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 565);
		}
		return ret;
	}

	public async Task<List<PrdGroupBusiness>> GetGroupAndChildsAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool isLoadParent = true, string tranName = "")
	{
		using (new JaegerMonitor("GetGroupAndChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			List<PrdGroupBusiness> ret = new List<PrdGroupBusiness>();
			try
			{
				if (isLoadParent)
				{
					ret.Add(this);
				}
				ProductGroupFilter filter = new ProductGroupFilter
				{
					System = system,
					LoadJustParents = false,
					LoadParentWithGuidEmpty = false
				};
				List<PrdGroupBusiness> list = (await GetAllAsync(ContextGuid, filter, token, tranName))?.List?.Where((PrdGroupBusiness p) => p.ParentGuid == Guid)?.ToList();
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (PrdGroupBusiness childGroup in list)
				{
					List<PrdGroupBusiness> list2 = await childGroup.GetGroupAndChildsAsync(system, token, isLoadParent, tranName);
					if (list2 != null && list2.Any())
					{
						ret.AddRange(list2);
					}
					else
					{
						ret.Add(childGroup);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetGroupAndChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 594);
				throw;
			}
			return ret;
		}
	}

	public static Task<IEnumerable<PrdGroupBusiness>> GetListByGrpNameAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetListByGrpNameAsync(name, token, "");
	}

	public static Task<PrdGroupBusiness> GetAsync(string contextGuid, long fullcode, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAsync(fullcode, token, "");
	}

	public async Task<List<ProductCompactBusiness>> GetProductsAsync(string contextGuid, Document orderBuy, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			return await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllAsync(null, null, orderBuy, token, new ProductCompactFilter
			{
				GroupGuids = new List<Guid> { Guid },
				Sort = (cashierBusiness?.Settings?.Products?.ProductSort).GetValueOrDefault()
			}, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 616);
		}
		return null;
	}

	public static Task<long> GetNextCodeAsync(string contextGuid, Guid parent, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetNextCodeAsync(parent, token, tranName);
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.DbCountAsync(status, token, tranName);
	}

	public static PrdGroupBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.Get(guid, token, tranName);
		}
		return null;
	}

	public static Task<PrdGroupBusiness> GetAsync(string contextGuid, short id, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAsync(id, token, "");
	}

	internal static async Task<ReturnedSaveFuncInfoWithValue<bool>> HasImageAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
			int line = 0;
			try
			{
				line = 10;
				if (ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductGroup == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup");
				}
				line = 110;
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = await ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.HasImageAsync(name, token, "");
				line = 111;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 643);
				line = 112;
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Format("Line:{0}//Name:{1}//ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup is null:{2}", line, name ?? "NULL!!", ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductGroup == null), "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 649);
				ret.AddReturnedValue(ex, "", "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 650);
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IProductGroup;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 663), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 663);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductGroup.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 665);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 668);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 671);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 672);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 677), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 677);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<PrdGroupBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<PrdGroupBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.ProductGroup;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال گروه کالاها";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Products;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 700);
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 702);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 704);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IProductGroup is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 711);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation($"گروه های کالا سینک هستند تعداد گروه کالای سینک شده:{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 712);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 719);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 721);
						}
						PrdGroupBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "گروه کالای جاری:" + obj?.Name;
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: PrdGroup \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 730);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 736);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 741);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 745);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 746);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 748);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 751);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 752);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "PrdGroupBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 782);
				}
				List<WebPrdGroup> list = new List<WebPrdGroup>();
				List<WebPrdGroup> list2 = list;
				list2.Add(await PrdGroupMapper.Instance.MapAsync(ContextGuid, this, new WebPrdGroup(), "", token));
				List<Packet> paks = WebPrdGroup.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 784);
				}
				using PrdGroupWaiter waiter = new PrdGroupWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("prdGroupBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 788);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 790);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 792);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 795);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 796);
			}
			return ret;
		}
	}

	public static Task<PrdGroupBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductGroup.GetAsync(name, token, tranName);
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(short autoFixValidationErrorsCount, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		checked
		{
			try
			{
				if (system == EnSystem.SignalR)
				{
					return ret;
				}
				Slug = Slug?.WordPressFixSlugOrSku();
				line = 1;
				ProductGroupFilter filter = new ProductGroupFilter
				{
					System = EnSystem.None,
					LoadJustParents = false,
					Status = null,
					LoadParentWithGuidEmpty = false
				};
				ProductGroupModel allCategories = await GetAllAsync(ContextGuid, filter, token, tranName);
				line = 2;
				if (string.IsNullOrEmpty(Name?.Trim()))
				{
					if (system == EnSystem.UserInterface)
					{
						ret.AddError(system, "وارد کردن عنوان گروه الزامی است", "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 822);
					}
					else
					{
						line = 10;
						int num = 1;
						Name = "نام گروه";
						while ((allCategories?.List?.Any((PrdGroupBusiness p) => p.Name.ToLower() == Name.ToLower())).GetValueOrDefault())
						{
							Name = $"نام گروه_{++num}";
						}
					}
				}
				line = 15;
				if (Code == 0L || FullCode == 0L)
				{
					if (autoFixValidationErrorsCount > 0 && system != EnSystem.UserInterface)
					{
						line = 20;
						Code = await GetNextCodeAsync(ContextGuid, ParentGuid ?? Guid.Empty, token);
						if (FullCode == 0L)
						{
							FullCode = Code;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						PrdGroupBusiness prdGroupBusiness = this;
						short num2 = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num2;
						returnedSaveFuncInfo.AddReturnedValue(await prdGroupBusiness.CheckValidationAsync(num2, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 839);
						return ret;
					}
					ret.AddError(system, "وارد کردن کد گروه الزامی است", "Code", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 842);
					return ret;
				}
				line = 25;
				Guid guid = Guid;
				Guid? parentGuid = ParentGuid;
				if (guid == parentGuid)
				{
					line = 26;
					if (autoFixValidationErrorsCount > 0)
					{
						line = 27;
						ParentGuid = null;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						PrdGroupBusiness prdGroupBusiness2 = this;
						short num2 = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num2;
						returnedSaveFuncInfo.AddReturnedValue(await prdGroupBusiness2.CheckValidationAsync(num2, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 853);
						return ret;
					}
					ret.AddError(system, "یک گروه نمیتواند والد خود باشد. لطفا والد را اصلاح کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 856);
				}
				line = 30;
				PrdGroupBusiness prdGroupBusiness3 = await GetAsync(ContextGuid, FullCode, token);
				if (prdGroupBusiness3 != null && prdGroupBusiness3.Guid != Guid.Empty && prdGroupBusiness3.Guid != Guid && prdGroupBusiness3.Status)
				{
					line = 35;
					if (autoFixValidationErrorsCount > 0)
					{
						line = 36;
						long num4 = (FullCode = await GetNextCodeAsync(ContextGuid, ParentGuid ?? Guid.Empty, token));
						if (FullCode == num4)
						{
							FullCode += 10 - autoFixValidationErrorsCount;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						PrdGroupBusiness prdGroupBusiness4 = this;
						short num2 = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num2;
						returnedSaveFuncInfo.AddReturnedValue(await prdGroupBusiness4.CheckValidationAsync(num2, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 870);
						return ret;
					}
					ret.AddError(system, "کد کامل وارد شده تکراری است لطفا کد و یا گروه فرعی را اصلاح نمایید\r\n" + $"GuidGroup:{Guid}\r\n" + "Name:" + Name, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 873);
				}
				line = 40;
				PrdGroupBusiness prdGroupBusiness5 = await GetAsync(ContextGuid, Name, token);
				line = 41;
				if (prdGroupBusiness5 != null && prdGroupBusiness5.Guid != Guid)
				{
					line = 41;
					if (autoFixValidationErrorsCount > 0)
					{
						line = 42;
						Name += $"_{autoFixValidationErrorsCount}";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						PrdGroupBusiness prdGroupBusiness6 = this;
						short num2 = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num2;
						returnedSaveFuncInfo.AddReturnedValue(await prdGroupBusiness6.CheckValidationAsync(num2, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 887);
						return ret;
					}
					ret.AddError(system, "گروه " + Name + " تکراری می باشد", "dup", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 890);
				}
				line = 50;
				if (string.IsNullOrEmpty(Slug))
				{
					line = 51;
					if (autoFixValidationErrorsCount > 0 && system != EnSystem.UserInterface)
					{
						line = 52;
						Slug = Name.WordPressFixSlugOrSku();
						if (string.IsNullOrEmpty(Slug))
						{
							Name = "گروه_کالا_" + Name;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						PrdGroupBusiness prdGroupBusiness7 = this;
						short num2 = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num2;
						returnedSaveFuncInfo.AddReturnedValue(await prdGroupBusiness7.CheckValidationAsync(num2, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 903);
						return ret;
					}
					line = 55;
					ret.AddError(system, "نامک نمی تواند خالی باشد\r\n" + $"GroupGuid:{Guid}\r\n" + "Name:" + Name, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 907);
				}
				else
				{
					line = 60;
					int num5 = 0;
					if (system != EnSystem.UserInterface)
					{
						while ((allCategories?.List?.Any((PrdGroupBusiness p) => p.Guid != Guid && (p?.Slug?.ToLower() ?? "") == Slug.ToLower())).GetValueOrDefault())
						{
							line = 65;
							if (num5 > 50)
							{
								throw new StackOverflowException($"fixe slug algoritm exceeded count of ({num5}) for Category name:({Name})");
							}
							Slug = Name.WordPressFixSlugOrSku(num5 += 1);
						}
					}
					else if ((allCategories?.List?.Any((PrdGroupBusiness p) => p.Guid != Guid && p.Slug.ToLower() == Slug.ToLower())).GetValueOrDefault())
					{
						line = 70;
						PrdGroupBusiness prdGroupBusiness8 = allCategories.List.First((PrdGroupBusiness p) => p.Slug.ToLower() == Slug.ToLower());
						ret.AddError(system, "گروه کالای دیگری(" + prdGroupBusiness8.Name + ") با همین Slug(نامک)-(" + Slug + ") وجود دارد. لطفا نامک دیگری برای کالا تعیین کنید.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 927);
					}
				}
				if (system == EnSystem.UserInterface && (ParentGuid ?? Guid.Empty) != Guid.Empty)
				{
					PrdGroupBusiness prdGroupBusiness9 = await GetAsync(ContextGuid, ParentGuid.Value, token, tranName);
					if ((prdGroupBusiness9?.ParentGuid ?? Guid.Empty) == Guid)
					{
						await ret.AddErrorAsync("گروه والد (" + prdGroupBusiness9.Name + ") خود به عنوان زیرمجموعه ای از گروه جاری تعریف شده و نمی تواند والد این گروه باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 935);
					}
				}
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex, $"#Line_{line} cache is null or disposed!");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 943);
				}
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 944);
			}
			finally
			{
				if (ret.HasError && system != EnSystem.UserInterface)
				{
					WebErrorLog.Instance.StartLog("#OnFinally!!\r\n" + $"#Line_{line}" + "ProductGroup Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 949);
				}
			}
			return ret;
		}
	}

	public static async Task<IEnumerable<PrdGroupBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<PrdGroupBusiness> items, EnSystem system, CancellationToken token)
	{
		int line = 0;
		try
		{
			if (system == EnSystem.SignalR || system == EnSystem.None || system == EnSystem.Transfer)
			{
				return items;
			}
			line = 1;
			UserAccBusiness userAccess = await ((await CashierBusiness.GetCurrentCashierAsync(contextGuid, token)) ?? throw new Exception("Current Cashier is null")).GetUserAsync(token);
			line = 2;
			if (!(userAccess?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault())
			{
				items = items?.Where((PrdGroupBusiness p) => !p.Special);
			}
			line = 3;
			if ((userAccess?.UserAccess?.PrdGroupListGuid?.Any()).GetValueOrDefault())
			{
				items = items?.Where((PrdGroupBusiness p) => p.Guid == Guid.Empty || userAccess.UserAccess.PrdGroupListGuid.Contains(p.Guid));
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			line = 4;
			items = items?.ToList();
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Error Line IS :{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 977);
			throw;
		}
	}

	public static async Task<string> GetGroupNameAsync(string contextGuid, List<Guid> guidList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(guidList?.Any() ?? false))
			{
				return "";
			}
			string groupName = "";
			foreach (Guid guid in guidList)
			{
				PrdGroupBusiness prdGroupBusiness = await GetAsync(contextGuid, guid, token, tranName);
				if (prdGroupBusiness != null)
				{
					groupName = groupName + " " + prdGroupBusiness.Name + " -";
				}
			}
			if (groupName.EndsWith("-"))
			{
				groupName = groupName.Substring(0, checked(groupName.Length - 1));
			}
			return groupName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGroupNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 997);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> UpdateSubGroupAsync(Guid masterGuid, bool special, string technicalInfo, bool awaitOnSemaphore, CancellationToken token, string tranName)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdGroupBusiness/UpdateSubGroupSpecialAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1011), ReturnedState.None, "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1011);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductGroup.UpdateSubGroupSpecialAsync(masterGuid, special, technicalInfo, token, tranName), ReturnedState.None, "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1013);
						goto end_IL_011b;
					}
					result = res.AddErrorTokenIsCancelled("", "", "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1010);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1017);
				res.AddReturnedValue(ex, "", "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1018);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1023), ReturnedState.None, "UpdateSubGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdGroupBusiness.cs", 1023);
			}
		}
		return res;
	}
}
