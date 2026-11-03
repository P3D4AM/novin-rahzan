using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Report;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Global;

namespace EntityCache.Business.Tickets;

public class TicketBusiness : Serializable<TicketBusiness>, ITicket, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IFileAttachment
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private List<FileInfoBusiness> _addedFileList = new List<FileInfoBusiness>();

	private List<FileInfoBusiness> _removedFileList = new List<FileInfoBusiness>();

	public Guid Guid { get; set; }

	public int Number { get; set; }

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	[JsonIgnore]
	public string DateM2Time => DateM.ToString("HH:mm");

	public Guid SourceGuid { get; set; }

	public EnTargetType SourceType { get; set; }

	public string Title { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSh => DateM.MiladiToCalture();

	public string Description { get; set; }

	public EnTargetType TargetType { get; set; }

	public Guid TargetGuid { get; set; }

	public short StatusId { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public Priority Priority { get; set; }

	public FileManagement_Parts Part => FileManagement_Parts.Ticket;

	public IReadOnlyCollection<FileInfoBusiness> AddedFiles => _addedFileList;

	public IReadOnlyCollection<FileInfoBusiness> RemovedFiles => _removedFileList;

	public event Func<object, List<TicketBusiness>, Task> OnTicketChanged;

	public TicketBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task TicketChanged(List<TicketBusiness> detBusiness, CancellationToken token = default(CancellationToken))
	{
		return PacketParser.Utilities.RaiseEvent(this.OnTicketChanged, null, detBusiness, token, 60, "TicketChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs");
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = ClsCache.GetCache(ContextGuid).Source;
				if (SourceType == EnTargetType.None)
				{
					res.AddError("لطفا جنس ارسال کننده را مشخص نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 62);
				}
				if (string.IsNullOrEmpty(Title))
				{
					res.AddError("لطفا عنوان را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 63);
				}
				if (TargetGuid == Guid.Empty)
				{
					res.AddError("لطفا بخش را مشخص نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 64);
				}
				if (string.IsNullOrEmpty(Description))
				{
					res.AddError("لطفا شرح درخواست را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 65);
				}
				if (SourceGuid == Guid.Empty)
				{
					res.AddError("لطفا فرستنده را مشخص نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 66);
				}
				TicketBusiness ticketBusiness = await GetByNumberAsync(ContextGuid, Number, token, tranName);
				if (ticketBusiness != null && ticketBusiness.Guid != Guid)
				{
					res.AddError("شماره وارد شده تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 68);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 72);
				res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 73);
			}
			return res;
		}
	}

	public static Task<int> NextNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ticket.NextNumberAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = 4;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TicketBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 89);
						}
						else
						{
							Modified = DateTime.Now;
							if (Number == 0)
							{
								Number = await NextNumberAsync(ContextGuid, token, tranName);
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 93), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 93);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 95);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Ticket.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 97);
								goto end_IL_0144;
							}
							result = res;
						}
						goto end_IL_009c;
						end_IL_0144:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 101);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 102);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 107), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 107);
				}
			}
			return res;
		}
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusAsync(short statusId, short oldStatusId, string extraDes, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ChangeStatusAsync(ContextGuid, Guid, statusId, oldStatusId, extraDes, null, awaitOnSemaphore, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, Guid guid, short statusId, short oldStatusId, string extraDes, Guid? tafsilGuid = null, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = 6;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TicketBusiness/ChangeStatusAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 123);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 124), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 124);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Ticket.ChangeStatusAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 126);
							if (!res.HasError && (oldStatusId > 0 || !string.IsNullOrEmpty(extraDes)))
							{
								IEnumerable<SanadStatusReportBusiness> source = await SanadStatusReportBusiness.GetAllAsync(contextGuid, Document.Ticket, ENAction.See, true);
								SanadStatusReportBusiness oldState = source.FirstOrDefault((SanadStatusReportBusiness q) => q.Id == oldStatusId);
								SanadStatusReportBusiness newState = source.FirstOrDefault((SanadStatusReportBusiness q) => q.Id == statusId);
								string description = "";
								CashierBusiness cashier = null;
								if ((tafsilGuid ?? Guid.Empty) == Guid.Empty)
								{
									cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
									if (cashier == null)
									{
										throw new ArgumentNullException("cashier");
									}
									description = $"این تیکت در تاریخ {DateTime.Now.MiladiToCalture()} ساعت {DateTime.Now.Hour}:{DateTime.Now.Minute} توسط {cashier.Name} از وضعیت {oldState.Name} به وضعیت {newState.Name} تغییر کرد";
								}
								else
								{
									TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, tafsilGuid.Value, "", token, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs");
									if (tafsilBusiness != null)
									{
										description = $"این تیکت در تاریخ {DateTime.Now.MiladiToCalture()} ساعت {DateTime.Now.Hour}:{DateTime.Now.Minute} توسط {tafsilBusiness.Name} از وضعیت {oldState.Name} به وضعیت {newState.Name} تغییر کرد";
									}
								}
								TicketDetBusiness ticketDetBusiness = new TicketDetBusiness(contextGuid)
								{
									Guid = Guid.NewGuid(),
									MasterGuid = guid,
									DateM = DateTime.Now,
									SourceGuid = cashier.Guid,
									Description = description,
									SourceType = EnTargetType.Cashier
								};
								if (!string.IsNullOrEmpty(extraDes))
								{
									ticketDetBusiness.Description = ticketDetBusiness.Description + "\r\n " + extraDes;
								}
								if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
								{
									ticketDetBusiness.SourceType = EnTargetType.Tafsil;
									ticketDetBusiness.SourceGuid = tafsilGuid.Value;
								}
								else if (cashier != null)
								{
									ticketDetBusiness.SourceGuid = cashier.Guid;
									ticketDetBusiness.SourceType = EnTargetType.Cashier;
								}
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ticketDetBusiness.SaveAsync(EnSystem.UserInterface, awaitOnSemaphore: true, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 170);
								goto end_IL_0172;
							}
							result = res;
						}
						goto end_IL_00c2;
						end_IL_0172:;
					}
					goto end_IL_00b1;
					end_IL_00c2:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 174);
					res.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 175);
					goto end_IL_00b1;
				}
				return result;
				end_IL_00b1:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 180), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 180);
				}
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveDetailAsync(string contextGuid, TicketDetailSaveConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (config == null)
		{
			throw new ArgumentException("config");
		}
		using (new JaegerMonitor("SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = 9;
				ReturnedSaveFuncInfo result;
				try
				{
					config.SendToServer = config.SendToServer && ClsCache.GetCache(contextGuid).BufferEnable;
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(config.AwaitOnSemaphore, "TicketBusiness/SaveDetailAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 196);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 197), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 197);
							}
							transactionHasBeenCreated = true;
							if (config.TicketDetail.MasterGuid != Guid.Empty && config.MasterTicket == null)
							{
								TicketDetailSaveConfig ticketDetailSaveConfig = config;
								ticketDetailSaveConfig.MasterTicket = await GetAsync(contextGuid, config.TicketDetail.MasterGuid, token, tranName);
							}
							if (config.MasterTicket == null)
							{
								throw new ArgumentException("MasterTicket");
							}
							if (config.MasterTicket.StatusId <= 0)
							{
								AccessLevel accessLevel = await (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token)).GetUserAccessAsync(token);
								config.MasterTicket.StatusId = checked((short)accessLevel.TicketAccess.DefaultSanadStatusAccess.Start);
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ((TicketDetBusiness)config.TicketDetail).CheckValidationAsync(token, tranName), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 211);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ((TicketBusiness)config.MasterTicket).SaveAsync(config.System, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 214);
								if (!res.HasError)
								{
									if (config.AddedFiles?.Any() ?? false)
									{
										foreach (IFileInfo addedFile in config.AddedFiles)
										{
											await ((TicketDetBusiness)config.TicketDetail).AddFileAsync((FileInfoBusiness)addedFile);
										}
									}
									if (config.RemovedFiles?.Any() ?? false)
									{
										foreach (IFileInfo removedFile in config.RemovedFiles)
										{
											await ((TicketDetBusiness)config.TicketDetail).RemoveFileAsync((FileInfoBusiness)removedFile);
										}
									}
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await ((TicketDetBusiness)config.TicketDetail).SaveAsync(config.System, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 225);
									goto end_IL_0208;
								}
								result = res;
							}
						}
						goto end_IL_010a;
						end_IL_0208:;
					}
					goto end_IL_00f5;
					end_IL_010a:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 229);
					res.AddReturnedValue(ex, "", "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 230);
					goto end_IL_00f5;
				}
				return result;
				end_IL_00f5:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 234), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 234);
				}
				if (!res.HasError && config.SendToServer && ClsCache.GetCache(contextGuid).BufferLoginState.Status == EnLoginResult.Success)
				{
					if (config.AwaitForSendToServer)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ((TicketBusiness)config.MasterTicket).Send2ServerAsync(config.CustomerUserName, config.CustomerPassword, config.Timeout, token, config.PriorityOfSendToServer), ReturnedState.None, "SaveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 238);
					}
					else
					{
						Task.Run(() => ((TicketBusiness)config.MasterTicket).Send2ServerAsync(config.CustomerUserName, config.CustomerPassword, config.Timeout, token, config.PriorityOfSendToServer));
					}
				}
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string customerUserName, string customerPassword, int timeout, CancellationToken token, Priority priority)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			return new ReturnedSaveFuncInfo();
		}
	}

	public Task<WebTicket> ToWebItemAsync(CancellationToken token)
	{
		return TicketMapper.Instance.MapAsync(ContextGuid, this, new WebTicket(), "", token);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<TicketBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.Ticket.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<TicketBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			ISendData2Server sendData2Server = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Ticket;
			try
			{
			}
			finally
			{
				if (sendData2Server != null)
				{
					if (returnedSaveFuncInfo.HasError)
					{
						sendData2Server.ShortMessage = "با شکست مواجه شد".GetTranslate();
						sendData2Server.Status = SyncStatus.SyncFailed;
					}
					else
					{
						sendData2Server.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						sendData2Server.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return returnedSaveFuncInfo;
		}
	}

	public static Task<TicketBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ticket.GetAsync(guid, token, tranName);
	}

	public static Task<TicketBusiness> GetByNumberAsync(string contextGuid, int number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ticket.GetByNumberAsync(number, token, tranName);
	}

	public static TicketBusiness GenerateDefault(string contextGuid, Guid sourceGuid)
	{
		try
		{
			return new TicketBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Priority = Priority.Medium,
				TargetType = EnTargetType.Tafsil,
				SourceGuid = sourceGuid,
				Status = true
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 399);
			throw;
		}
	}

	public async Task<string> GetSourceNameAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			return SourceType switch
			{
				EnTargetType.Cashier => (await CashierBusiness.GetAsync(ContextGuid, SourceGuid, token, tranName))?.Name, 
				EnTargetType.Tafsil => (await TafsilBusiness.GetAsync(ContextGuid, system, SourceGuid, tranName, token, "GetSourceNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs"))?.Name, 
				EnTargetType.Department => (await TicketPartBusiness.GetAsync(ContextGuid, SourceGuid, token))?.Name, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSourceNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 423);
			throw;
		}
	}

	public async Task<TicketPartBusiness> GetPartAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (TargetGuid == Guid.Empty)
			{
				return null;
			}
			return await TicketPartBusiness.GetAsync(ContextGuid, TargetGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 436);
			throw;
		}
	}

	public Task AddFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.AddFileAsync(file, _addedFileList);
	}

	public Task RemoveFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.RemoveFileAsync(file, _removedFileList);
	}

	public static async Task<IEnumerable<TicketReportBusiness>> GetAllAsync(string contextGuid, TicketFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<TicketReportBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.Ticket.GetAllAsync(filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return await ApplyAccessAsync(contextGuid, enumerable, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 453);
			throw;
		}
	}

	public static Task<List<TicketTitleSuggest>> GetSuggestTitelsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ticket.GetSuggestTitelsAsync(token, tranName);
	}

	private static async Task<IEnumerable<TicketReportBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<TicketReportBusiness> list, CancellationToken token, string tranName)
	{
		try
		{
			IEnumerable<TicketPartBusiness> enumerable = await TicketPartBusiness.GetAllAsync(contextGuid, new TicketPartFilter
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			}, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return list;
			}
			List<Guid> validPartGuids = enumerable.Select((TicketPartBusiness q) => q.Guid).ToList();
			list = list?.Where((TicketReportBusiness q) => (q.SourceType != EnTargetType.Department || (q.SourceType == EnTargetType.Department && validPartGuids.Contains(q.SourceGuid))) && (q.TargetType != EnTargetType.Department || (q.TargetType == EnTargetType.Department && validPartGuids.Contains(q.TargetGuid))));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketBusiness.cs", 472);
			throw;
		}
	}
}
