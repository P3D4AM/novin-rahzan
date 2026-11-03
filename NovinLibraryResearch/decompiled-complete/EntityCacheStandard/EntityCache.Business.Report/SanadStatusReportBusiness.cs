using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Sanad;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Report;

public class SanadStatusReportBusiness : BaseBusiness<SanadStatusReportBusiness>, IHasContext, ISanadStatusReport, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<SanadStatusReportBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _name;

	public Guid Guid { get; set; }

	public int Id { get; set; }

	public string Name
	{
		get
		{
			return _name?.TrimStart(Array.Empty<char>());
		}
		set
		{
			_name = value.FixString();
		}
	}

	public Document AsType { get; set; }

	public string AsTypeName => AsType.GetDisplay().GetTranslate();

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public override string ContextGuid { get; set; }

	public SanadStatusReportMetaData MetaData { get; set; }

	public int WarningMinutes { get; set; }

	public int ErrorMinutes { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public long? Id2From { get; set; }

	public long? Id2To { get; set; }

	public int InactivityDays { get; set; }

	public int TargetStatusId { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public bool IsChecked { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public static int NotDeterminedId => 9999;

	public SanadStatusReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		Guid = Guid.NewGuid();
		ChangeStatus = EntityChangeSt.New;
	}

	public static Task<SanadStatusReportBusiness> GetAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAsync(id, token, tranName);
	}

	public static Task<SanadStatusReportBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAsync(guid, token, tranName);
	}

	public static Task<int> GetNextIdAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetNextIdAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, FileInfoBusiness fileInfo = null, bool await4send = false, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 7;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "SanadStatusReportBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 81);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 82), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 82);
						}
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 83), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 83);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 85);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (Id <= 0 && Status)
							{
								Id = await GetNextIdAsync(ContextGuid, token, tranName);
							}
							if (fileInfo == null)
							{
								goto IL_05ac;
							}
							fileInfo.ServerStatus = ServerStatusForClient.None;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await fileInfo.SaveAsync(system, 10, sendToServer, Priority.Low, awaitOnSemaphore: true, tryCount, tranName, awair4send: false, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 91);
							if (!ret.HasError)
							{
								goto IL_05ac;
							}
							result = ret;
						}
					}
					goto end_IL_00e2;
					IL_05ac:
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.SanadStatusReport.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 95);
						if (!ret.HasError && system == EnSystem.UserInterface)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await UpdateAccessAsync(sendToServer, system, await4send, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 97);
							goto end_IL_0196;
						}
						result = ret;
					}
					goto end_IL_00e2;
					end_IL_0196:;
				}
				goto end_IL_00d1;
				end_IL_00e2:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 99);
				goto end_IL_00d1;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 102);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 103);
				goto end_IL_00d1;
			}
			return result;
			end_IL_00d1:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 109), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 109);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 110), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 110);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 114);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system));
				}
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> UpdateAccessAsync(bool sendToServer, EnSystem system, bool await4Send, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<UserAccBusiness> enumerable = await UserAccBusiness.GetAllAsync(ContextGuid, true, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			bool dataHasChanged = false;
			foreach (UserAccBusiness item in enumerable)
			{
				switch (AsType)
				{
				case Document.Reception:
					if (item.UserAccess.Reception.SanadStatusAccess?.FirstOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Reception.SanadStatusAccess.Add(new ReceptionSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.Pardakht:
					if (item.UserAccess.Pardakht.SanadStatusAccess?.FirstOrDefault((PardakhtSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Pardakht.SanadStatusAccess.Add(new PardakhtSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.OrderSell:
					if (item.UserAccess.Sell.SanadStatusAccess?.FirstOrDefault((OrderSellSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Sell.SanadStatusAccess.Add(new OrderSellSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.OrderBuy:
					if (item.UserAccess.Buy.SanadStatusAccess?.FirstOrDefault((OrderBuySanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Buy.SanadStatusAccess.Add(new OrderBuySanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.OrderBackBuy:
					if (item.UserAccess.BackBuy.SanadStatusAccess?.FirstOrDefault((OrderBackBuySanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.BackBuy.SanadStatusAccess.Add(new OrderBackBuySanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.OrderBackSell:
					if (item.UserAccess.BackSell.SanadStatusAccess?.FirstOrDefault((OrderBackSellSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.BackSell.SanadStatusAccess.Add(new OrderBackSellSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.PishFactor:
					if (item.UserAccess.PishFacotr.SanadStatusAccess?.FirstOrDefault((OrderPreSellSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.PishFacotr.SanadStatusAccess.Add(new OrderPreSellSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.OrderPreBuy:
					if (item.UserAccess.PreBuy.SanadStatusAccess?.FirstOrDefault((OrderPreBuySanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.PreBuy.SanadStatusAccess.Add(new OrderPreBuySanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.Zayeat:
					if (item.UserAccess.Zayeat.SanadStatusAccess?.FirstOrDefault((OrderZayeatSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Zayeat.SanadStatusAccess.Add(new OrderZayeatSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.AnbarGardani:
					if (item.UserAccess.AnbarGardani.SanadStatusAccess?.FirstOrDefault((AnbarGardaniSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.AnbarGardani.SanadStatusAccess.Add(new AnbarGardaniSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.AmaniVorod:
					if (item.UserAccess.AmaniVarede.SanadStatusAccess?.FirstOrDefault((OrderAmaniVorodSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.AmaniVarede.SanadStatusAccess.Add(new OrderAmaniVorodSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.AmaniKhoroj:
					if (item.UserAccess.AmaniSadere.SanadStatusAccess?.FirstOrDefault((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.AmaniSadere.SanadStatusAccess.Add(new OrderAmaniKhorojSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.Sanad:
					if (item.UserAccess.ManualSanad.SanadStatusAccess?.FirstOrDefault((ManualSanadSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.ManualSanad.SanadStatusAccess.Add(new ManualSanadSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.CheckSanad:
					if (item.UserAccess.Check.SanadStatusAccess?.FirstOrDefault((ChequeSanadSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Check.SanadStatusAccess.Add(new ChequeSanadSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.Resid:
					if (item.UserAccess.Resid.SanadStatusAccess?.FirstOrDefault((ResidSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.Resid.SanadStatusAccess.Add(new ResidSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				case Document.SarNakh:
					if (item.UserAccess.SarNakh.SanadStatusAccess?.FirstOrDefault((SarNakhSanadStatusAccess q) => q.StatusId == Id) == null)
					{
						item.UserAccess.SarNakh.SanadStatusAccess.Add(new SarNakhSanadStatusAccess
						{
							StatusId = Id
						});
						dataHasChanged = true;
					}
					break;
				}
				if (dataHasChanged)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(sendToServer, system, Priority.Low, awaitOnSemaphore: true, await4Send, 10, tranName, token, 0), ReturnedState.None, "UpdateAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 264);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 269);
			res.AddReturnedValue(ex, "", "UpdateAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 270);
		}
		return res;
	}

	public static async Task<IEnumerable<SanadStatusReportBusiness>> GetAllAsync(string contextGuid, Document? document, ENAction? action, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			IEnumerable<SanadStatusReportBusiness> _sanadList = await ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAllAsync(document, status, token, tranName);
			if (token.IsCancellationRequested || !(_sanadList?.Any() ?? false))
			{
				return null;
			}
			if (document.GetValueOrDefault() != 0)
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				if (cashierBusiness == null)
				{
					return _sanadList;
				}
				UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token, tranName);
				if (userAccBusiness == null)
				{
					throw new ArgumentNullException("user");
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				IEnumerable<int> accessStatus = null;
				switch (document)
				{
				case Document.Reception:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Reception.SanadStatusAccess?.Where((ReceptionSanadStatusAccess q) => q.Add)?.Select((ReceptionSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Reception.SanadStatusAccess?.Where((ReceptionSanadStatusAccess q) => q.Edit)?.Select((ReceptionSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Reception.SanadStatusAccess?.Where((ReceptionSanadStatusAccess q) => q.Show)?.Select((ReceptionSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.Pardakht:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Pardakht.SanadStatusAccess?.Where((PardakhtSanadStatusAccess q) => q.Add)?.Select((PardakhtSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Pardakht.SanadStatusAccess?.Where((PardakhtSanadStatusAccess q) => q.Edit)?.Select((PardakhtSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Pardakht.SanadStatusAccess?.Where((PardakhtSanadStatusAccess q) => q.Show)?.Select((PardakhtSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.OrderSell:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Sell.SanadStatusAccess?.Where((OrderSellSanadStatusAccess q) => q.Add)?.Select((OrderSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Sell.SanadStatusAccess?.Where((OrderSellSanadStatusAccess q) => q.Edit)?.Select((OrderSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Sell.SanadStatusAccess?.Where((OrderSellSanadStatusAccess q) => q.Show)?.Select((OrderSellSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.OrderBuy:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Buy.SanadStatusAccess?.Where((OrderBuySanadStatusAccess q) => q.Add)?.Select((OrderBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Buy.SanadStatusAccess?.Where((OrderBuySanadStatusAccess q) => q.Edit)?.Select((OrderBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Buy.SanadStatusAccess?.Where((OrderBuySanadStatusAccess q) => q.Show)?.Select((OrderBuySanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.OrderBackBuy:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess?.Where((OrderBackBuySanadStatusAccess q) => q.Add)?.Select((OrderBackBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess?.Where((OrderBackBuySanadStatusAccess q) => q.Edit)?.Select((OrderBackBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess?.Where((OrderBackBuySanadStatusAccess q) => q.Show)?.Select((OrderBackBuySanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.OrderBackSell:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.BackSell.SanadStatusAccess?.Where((OrderBackSellSanadStatusAccess q) => q.Add)?.Select((OrderBackSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.BackSell.SanadStatusAccess?.Where((OrderBackSellSanadStatusAccess q) => q.Edit)?.Select((OrderBackSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.BackSell.SanadStatusAccess?.Where((OrderBackSellSanadStatusAccess q) => q.Show)?.Select((OrderBackSellSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.PishFactor:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess?.Where((OrderPreSellSanadStatusAccess q) => q.Add)?.Select((OrderPreSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess?.Where((OrderPreSellSanadStatusAccess q) => q.Edit)?.Select((OrderPreSellSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess?.Where((OrderPreSellSanadStatusAccess q) => q.Show)?.Select((OrderPreSellSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.OrderPreBuy:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess?.Where((OrderPreBuySanadStatusAccess q) => q.Add)?.Select((OrderPreBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess?.Where((OrderPreBuySanadStatusAccess q) => q.Edit)?.Select((OrderPreBuySanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess?.Where((OrderPreBuySanadStatusAccess q) => q.Show)?.Select((OrderPreBuySanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.Zayeat:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess?.Where((OrderZayeatSanadStatusAccess q) => q.Add)?.Select((OrderZayeatSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess?.Where((OrderZayeatSanadStatusAccess q) => q.Edit)?.Select((OrderZayeatSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess?.Where((OrderZayeatSanadStatusAccess q) => q.Show)?.Select((OrderZayeatSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.AnbarGardani:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess?.Where((AnbarGardaniSanadStatusAccess q) => q.Add)?.Select((AnbarGardaniSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess?.Where((AnbarGardaniSanadStatusAccess q) => q.Edit)?.Select((AnbarGardaniSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess?.Where((AnbarGardaniSanadStatusAccess q) => q.Show)?.Select((AnbarGardaniSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.AmaniVorod:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess?.Where((OrderAmaniVorodSanadStatusAccess q) => q.Add)?.Select((OrderAmaniVorodSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess?.Where((OrderAmaniVorodSanadStatusAccess q) => q.Edit)?.Select((OrderAmaniVorodSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess?.Where((OrderAmaniVorodSanadStatusAccess q) => q.Show)?.Select((OrderAmaniVorodSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.AmaniKhoroj:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess?.Where((OrderAmaniKhorojSanadStatusAccess q) => q.Add)?.Select((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess?.Where((OrderAmaniKhorojSanadStatusAccess q) => q.Edit)?.Select((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess?.Where((OrderAmaniKhorojSanadStatusAccess q) => q.Show)?.Select((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.Sanad:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.ManualSanad.SanadStatusAccess?.Where((ManualSanadSanadStatusAccess q) => q.Add)?.Select((ManualSanadSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.ManualSanad.SanadStatusAccess?.Where((ManualSanadSanadStatusAccess q) => q.Edit)?.Select((ManualSanadSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.ManualSanad.SanadStatusAccess?.Where((ManualSanadSanadStatusAccess q) => q.Show)?.Select((ManualSanadSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.CheckSanad:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Check.SanadStatusAccess?.Where((ChequeSanadSanadStatusAccess q) => q.Add)?.Select((ChequeSanadSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.Resid:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.Resid.SanadStatusAccess?.Where((ResidSanadStatusAccess q) => q.Add)?.Select((ResidSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.Resid.SanadStatusAccess?.Where((ResidSanadStatusAccess q) => q.Edit)?.Select((ResidSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.Resid.SanadStatusAccess?.Where((ResidSanadStatusAccess q) => q.Show)?.Select((ResidSanadStatusAccess q) => q.StatusId);
					}
					break;
				case Document.SarNakh:
					if (action == ENAction.Add)
					{
						accessStatus = userAccBusiness.UserAccess.SarNakh.SanadStatusAccess?.Where((SarNakhSanadStatusAccess q) => q.Add)?.Select((SarNakhSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.Edit)
					{
						accessStatus = userAccBusiness.UserAccess.SarNakh.SanadStatusAccess?.Where((SarNakhSanadStatusAccess q) => q.Edit)?.Select((SarNakhSanadStatusAccess q) => q.StatusId);
					}
					else if (action == ENAction.See)
					{
						accessStatus = userAccBusiness.UserAccess.SarNakh.SanadStatusAccess?.Where((SarNakhSanadStatusAccess q) => q.Show)?.Select((SarNakhSanadStatusAccess q) => q.StatusId);
					}
					break;
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (accessStatus?.Any() ?? false)
				{
					_sanadList = _sanadList?.Where((SanadStatusReportBusiness q) => accessStatus.Contains(q.Id));
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
			}
			return _sanadList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 433);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
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
			_ = 2;
			try
			{
				Modified = DateTime.Now;
				using (await _databaseSemaphore.GaurdAsync(true, "SanadStatusReportBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 447), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 447);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.SanadStatusReport.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 449);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 451);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 454);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 455);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 460), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 460);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static Task<SanadStatusReportBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<SanadStatusReportBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Desc;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال وضعیت اسناد".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Others;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 482);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 484);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 485);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport;
					throw new NullReferenceException("repo");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 495);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("وضعیت اسناد سینک هستند تعداد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 496);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 497);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 504);
						}
						current = checked(current + 1);
						SanadStatusReportBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "داده جاری".GetTranslate() + ":" + (obj.Name ?? "").GetTranslate();
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 512);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: SanadStatusReport \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 516);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 522);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 527);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 531);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 532);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 536);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 537);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.SanadStatusReport;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs"))
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (!cache.BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "SanadStatusReportBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 572);
				}
				WebSanadStatusReport item = await SanadStatusReportMapper.Instance.MapAsync(ContextGuid, this, new WebSanadStatusReport(), "", token);
				List<Packet> paks = WebSanadStatusReport.GenerateSaveRequest(new List<WebSanadStatusReport> { item }, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 575);
				}
				using SanadStatusReportWaiter waiter = new SanadStatusReportWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => cache.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("SanadStatusReportWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 579);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 581);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 583);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 586);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 587);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name))
			{
				res.AddError("لطفا عنوان وضعیت را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 596);
			}
			else if (Status)
			{
				SanadStatusReportBusiness sanadStatusReportBusiness = await GetAsync(ContextGuid, Name, AsType, token, tranName);
				if (sanadStatusReportBusiness != null && sanadStatusReportBusiness.Guid != Guid)
				{
					if (system == EnSystem.Transfer)
					{
						IEnumerable<SanadStatusReportBusiness> enumerable = await GetAllAsync(ContextGuid, AsType, null, true, token, tranName);
						int num = 1;
						while (enumerable != null && enumerable.Any((SanadStatusReportBusiness p) => p.Name.ToLower() == Name.ToLower()))
						{
							Name = $"{Name}{num = checked(num + 1)}";
						}
					}
					else
					{
						res.AddError("نام وضعیت".GetTranslate() + "  (" + (Name ?? "").GetTranslate() + ") " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 610);
					}
				}
			}
			if (Status && ((!Id2From.HasValue && Id2To.HasValue) || (Id2From.HasValue && !Id2To.HasValue)))
			{
				res.AddError(system, "بازه کدینگ نامعتبر است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 614);
			}
			if (Status && Id2From.HasValue && Id2To.HasValue)
			{
				if (Status && Id2To.Value < Id2From.Value)
				{
					res.AddError("بازه کدینگ نامعتبر است".GetTranslate() + " " + $" ({Id2From.Value}-{Id2To.Value})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 617);
				}
				if (Status && Id2To.Value == Id2From.Value)
				{
					res.AddError("بازه کدینگ نامعتبر است".GetTranslate() + " " + $" ({Id2From.Value}-{Id2To.Value})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 618);
				}
				if (Status && Id2From.Value <= 0)
				{
					res.AddError("بازه کدینگ نامعتبر است".GetTranslate() + " " + $" ({Id2From.Value}-{Id2To.Value})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 619);
				}
				List<SanadStatusReportBusiness> list = await IsDuplicateCodeAsync(ContextGuid, Id2From.Value, Id2To.Value, AsType, Guid, token, tranName);
				if (list != null && list.Any((SanadStatusReportBusiness p) => p.Guid != Guid))
				{
					res.AddError(system, "کد شروع بازه یا پایان بازه وضعیت".GetTranslate() + " " + (" " + Name).GetTranslate() + " " + "با وضعیت دیگری همپوشانی دارد ".GetTranslate() + "\r\n" + "کد شروع بازه".GetTranslate() + ":" + $"{Id2From.Value}".GetTranslate() + "\r\n" + "کد پایان بازه".GetTranslate() + ":" + $"{Id2To.Value}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 624);
					foreach (SanadStatusReportBusiness item in list)
					{
						if (item != null)
						{
							res.AddError("تداخل با وضعیت".GetTranslate() + "(" + (item.Name ?? "").GetTranslate() + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 629);
						}
					}
				}
			}
			if (!Status && system == EnSystem.UserInterface)
			{
				int num2 = await CheckCountAsync(ContextGuid, system, Id, AsType, token, tranName);
				if (num2 > 0)
				{
					await res.AddErrorAsync("با توجه به اینکه تعداد".GetTranslate() + $"{num2}".GetTranslate() + " سند در وضعیت ".GetTranslate() + " " + (Name + " ").GetTranslate() + "قرار دارد، شما مجاز به حذف این وضعیت نمی باشید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 636);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 641);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 642);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> CheckValidationAsync(string contextGuid, EnSystem system, int oldStatusId, int newStatusId, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (oldStatusId == newStatusId || (oldStatusId <= 0 && newStatusId <= 0))
			{
				return res;
			}
			SanadStatusReportBusiness oldStatus = null;
			SanadStatusReportBusiness newStatus = null;
			if (oldStatusId > 0)
			{
				oldStatus = await GetAsync(contextGuid, oldStatusId, token, tranName);
			}
			if (newStatusId > 0)
			{
				newStatus = await GetAsync(contextGuid, newStatusId, token, tranName);
			}
			if ((oldStatus?.MetaData?.NextNodes?.Any()).GetValueOrDefault() && !oldStatus.MetaData.NextNodes.Contains(newStatusId))
			{
				await res.AddErrorAsync("سلسله مراتب وضعیت سند رعایت نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 660);
			}
			if (res.HasError)
			{
				return res;
			}
			if (newStatus?.MetaData != null && newStatus.MetaData.StartNode != 0 && oldStatusId != 0 && newStatus.MetaData.StartNode != oldStatusId)
			{
				await res.AddErrorAsync("سلسله مراتب وضعیت سند رعایت نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 668);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 673);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 674);
		}
		return res;
	}

	public static async Task<IEnumerable<SanadStatusReportErrorTimeModel>> GetAllErrorTimeAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			List<SanadStatusReportErrorTimeModel> accessList = new List<SanadStatusReportErrorTimeModel>();
			IEnumerable<SanadStatusReportErrorTimeModel> list = await ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAllErrorTimeAsync(token, tranName);
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			UserAccBusiness user = await (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName)).GetUserAsync(token, tranName);
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.Reception && (from q in user.UserAccess.Reception.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.Pardakht && (from q in user.UserAccess.Pardakht.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.OrderSell && (from q in user.UserAccess.Sell.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.OrderBackSell && (from q in user.UserAccess.BackSell.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.OrderBuy && (from q in user.UserAccess.Buy.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.OrderBackBuy && (from q in user.UserAccess.BackBuy.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.PishFactor && (from q in user.UserAccess.PishFacotr.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.OrderPreBuy && (from q in user.UserAccess.PreBuy.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.Zayeat && (from q in user.UserAccess.Zayeat.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.AmaniKhoroj && (from q in user.UserAccess.AmaniSadere.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.AmaniVorod && (from q in user.UserAccess.AmaniVarede.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.Sanad && (from q in user.UserAccess.ManualSanad.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.CheckSanad && (from q in user.UserAccess.Check.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.AnbarGardani && (from q in user.UserAccess.AnbarGardani.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			accessList.AddRange(list.Where((SanadStatusReportErrorTimeModel q) => q.AsType == Document.Resid && (from q in user.UserAccess.Resid.SanadStatusAccess
				where q.Show
				select q.StatusId).Contains(q.StatusId)));
			return accessList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 708);
			throw;
		}
	}

	internal static async Task ApplySanadStatusIdAsync(string contextGuid, IHasSanadStatusReport data, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", EnTaxResult? taxResult = null)
	{
		_ = 1;
		try
		{
			if (data == null)
			{
				return;
			}
			short num = await GetSanadStatusIdAsync(contextGuid, data.CashierGuid, data.AsType, data.StatusId, data.ChangeStatus, system, token, tranName, taxResult);
			if (num > 0 && data.StatusId != num)
			{
				data.StatusId = num;
				if (data.AsType == Document.Pardakht && num != 0)
				{
					int value = await ((PardakhtBusiness)data).GetId2Async(token);
					((PardakhtBusiness)data).SetId2(value, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplySanadStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 726);
		}
	}

	public static async Task<short> GetSanadStatusIdAsync(string contextGuid, Guid cashierGuid, Document asType, short oldStatusId, EntityChangeSt changeStatus, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", EnTaxResult? taxResult = null)
	{
		try
		{
			if (cashierGuid == Guid.Empty)
			{
				return 0;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetAsync(contextGuid, cashierGuid, token, tranName);
			if (cashierBusiness == null)
			{
				return 0;
			}
			UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token, tranName);
			if (userAccBusiness == null)
			{
				return 0;
			}
			ENSource source = ClsCache.GetCache(contextGuid).Source;
			int num = 0;
			bool flag = false;
			switch (asType)
			{
			case Document.Reception:
				flag = userAccBusiness.UserAccess.Reception.SanadStatusAccess.SingleOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				if (system == EnSystem.SmsParser)
				{
					num = userAccBusiness.UserAccess.Reception.SanadStatusDefaultAccess.StatusFromSmsParser;
					break;
				}
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.Reception.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.Reception.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.Reception.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.Pardakht:
				flag = userAccBusiness.UserAccess.Pardakht.SanadStatusAccess.SingleOrDefault((PardakhtSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				if (system == EnSystem.SmsParser)
				{
					num = userAccBusiness.UserAccess.Pardakht.SanadStatusDefaultAccess.StatusFromSmsParser;
					break;
				}
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.Pardakht.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.Pardakht.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.Pardakht.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.OrderSell:
				flag = userAccBusiness.UserAccess.Sell.SanadStatusAccess.SingleOrDefault((OrderSellSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				if (taxResult.GetValueOrDefault() != 0)
				{
					switch (taxResult.Value)
					{
					case EnTaxResult.Sent:
						num = userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.TaxStatusAfterSend;
						break;
					case EnTaxResult.Accepted:
						num = userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.TaxStatusAfterSuccess;
						break;
					case EnTaxResult.Failed:
						num = userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.TaxStatusAfterFailed;
						break;
					}
				}
				else
				{
					switch (changeStatus)
					{
					case EntityChangeSt.New:
						num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.InsertFromAccounting);
						break;
					case EntityChangeSt.Edited:
						num = userAccBusiness.UserAccess.Sell.SanadStatusDefaultAccess.StatusAfterEdit;
						break;
					}
				}
				break;
			case Document.OrderBuy:
				flag = userAccBusiness.UserAccess.Buy.SanadStatusAccess.SingleOrDefault((OrderBuySanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.Buy.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.Buy.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.Buy.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.OrderBackBuy:
				flag = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess.SingleOrDefault((OrderBackBuySanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.BackBuy.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.BackBuy.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.BackBuy.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.OrderBackSell:
				flag = userAccBusiness.UserAccess.BackSell.SanadStatusAccess.SingleOrDefault((OrderBackSellSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.BackSell.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.BackSell.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.BackSell.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.PishFactor:
				flag = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess.SingleOrDefault((OrderPreSellSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.PishFacotr.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.PishFacotr.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.PishFacotr.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.Zayeat:
				flag = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess.SingleOrDefault((OrderZayeatSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.Zayeat.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.Zayeat.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.Zayeat.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.AnbarGardani:
				flag = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess.SingleOrDefault((AnbarGardaniSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = userAccBusiness.UserAccess.AnbarGardani.SanadStatusDefaultAccess.InsertFromAccounting;
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.AnbarGardani.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.AmaniVorod:
				flag = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess.SingleOrDefault((OrderAmaniVorodSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.AmaniVarede.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.AmaniVarede.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.AmaniVarede.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.AmaniKhoroj:
				flag = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess.SingleOrDefault((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.AmaniSadere.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.AmaniSadere.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.AmaniSadere.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.Sanad:
				flag = userAccBusiness.UserAccess.ManualSanad.SanadStatusAccess.SingleOrDefault((ManualSanadSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = userAccBusiness.UserAccess.ManualSanad.SanadStatusDefaultAccess.InsertFromAccounting;
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.ManualSanad.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.CheckSanad:
				flag = userAccBusiness.UserAccess.Check.SanadStatusAccess.SingleOrDefault((ChequeSanadSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				if (changeStatus == EntityChangeSt.New)
				{
					num = userAccBusiness.UserAccess.Check.SanadStatusDefaultAccess.InsertFromAccounting;
				}
				break;
			case Document.Resid:
				flag = userAccBusiness.UserAccess.Resid.SanadStatusAccess.SingleOrDefault((ResidSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = userAccBusiness.UserAccess.Resid.SanadStatusDefaultAccess.InsertFromAccounting;
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.Resid.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.OrderPreBuy:
				flag = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess.SingleOrDefault((OrderPreBuySanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = ((source != ENSource.Accounting) ? userAccBusiness.UserAccess.PreBuy.SanadStatusDefaultAccess.InsertFromClients : userAccBusiness.UserAccess.PreBuy.SanadStatusDefaultAccess.InsertFromAccounting);
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.PreBuy.SanadStatusDefaultAccess.StatusAfterEdit;
					break;
				}
				break;
			case Document.SarNakh:
				flag = userAccBusiness.UserAccess.SarNakh.SanadStatusAccess.SingleOrDefault((SarNakhSanadStatusAccess q) => q.StatusId == oldStatusId)?.ChangeStatus ?? true;
				switch (changeStatus)
				{
				case EntityChangeSt.New:
					num = userAccBusiness.UserAccess.SarNakh.SanadStatusDefaultAccess.Add;
					break;
				case EntityChangeSt.Edited:
					num = userAccBusiness.UserAccess.SarNakh.SanadStatusDefaultAccess.Edit;
					break;
				}
				break;
			default:
				throw new NotImplementedException();
			case Document.Tolid:
			case Document.Tolid_Hazine:
			case Document.Tolid_Mahsul:
			case Document.Tolid_Mavad:
			case Document.TolidFormula:
				break;
			}
			if (!flag)
			{
				return oldStatusId;
			}
			return checked((short)num);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 938);
			throw;
		}
	}

	public static Task<int> GetMinErrorTimeAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetMinErrorTimeAsync(token, tranName);
	}

	public static Task<SanadStatusReportBusiness> GenerateNotDeterminedAsync(string contextGuid)
	{
		try
		{
			return Task.FromResult(new SanadStatusReportBusiness(contextGuid)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = contextGuid,
				Guid = Guid.NewGuid(),
				Id = NotDeterminedId,
				Name = "تعیین وضعیت نشده (پیش فرض)",
				Status = true
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNotDeterminedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 960);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<SanadStatusReportBusiness> lst, bool send2Server, Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (SanadStatusReportBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 975);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, send2Server, null, await4send: false, token, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 976);
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 988);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 989);
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

	public static Task<SanadStatusReportBusiness> GetAsync(string contextGuid, string name, Document asType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.GetAsync(name, asType, token, tranName);
	}

	public static Task<List<SanadStatusReportBusiness>> IsDuplicateCodeAsync(string contextGuid, long minCode, long maxCode, Document asType, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SanadStatusReport.IsDuplicateCodeAsync(minCode, maxCode, asType, guid, token, tranName);
	}

	public static async Task<bool> IsId2InRangeAsync(string contextGuid, short? statusId, int id2, CancellationToken token, string tranName = "")
	{
		try
		{
			if (statusId.GetValueOrDefault() <= 0)
			{
				return true;
			}
			SanadStatusReportBusiness sanadStatusReportBusiness = await GetAsync(contextGuid, statusId.Value, token, tranName);
			if (sanadStatusReportBusiness == null || !sanadStatusReportBusiness.Id2From.HasValue || sanadStatusReportBusiness == null || !sanadStatusReportBusiness.Id2To.HasValue)
			{
				return true;
			}
			return id2 >= sanadStatusReportBusiness.Id2From.Value && id2 <= sanadStatusReportBusiness.Id2To.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsId2InRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1018);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixId2Async(string contextGuid, Guid asGuid, Document asType, short statusId, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (statusId <= 0)
			{
				return res;
			}
			switch (asType)
			{
			case Document.OrderSell:
			case Document.OrderBuy:
			case Document.OrderBackBuy:
			case Document.OrderBackSell:
			case Document.PishFactor:
			case Document.Zayeat:
			case Document.AmaniVorod:
			case Document.AmaniKhoroj:
			case Document.OrderPreBuy:
			{
				OrderBusiness order = await OrderBusiness.GetAsync(contextGuid, asGuid, asType, fromDatabase: true);
				if (order != null)
				{
					order.SetId2(await order.NextId2Async(statusId, token), token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await order.SaveAsync(system, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10), ReturnedState.None, "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1045);
				}
				break;
			}
			case Document.Reception:
			{
				ReceptionBusiness reception = await ReceptionBusiness.GetAsync(contextGuid, asGuid, system, null, fromDatabase: true);
				if (reception != null)
				{
					reception.SetId2(await reception.NextId2Async(statusId, token), token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await reception.SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10), ReturnedState.None, "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1054);
				}
				break;
			}
			case Document.Pardakht:
			{
				PardakhtBusiness pardakht = await PardakhtBusiness.GetAsync(contextGuid, asGuid, fromDatabase: true);
				if (pardakht != null)
				{
					pardakht.SetId2(await pardakht.NextId2Async(statusId, token), token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await pardakht.SaveAsync(system, sendToServer: false, Priority.Low, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1063);
				}
				break;
			}
			case Document.AnbarGardani:
			{
				PrdAnbarGardaniBusiness anbarGardani = await PrdAnbarGardaniBusiness.GetAsync(contextGuid, asGuid, token);
				if (anbarGardani != null)
				{
					anbarGardani.SetId2(await anbarGardani.NextId2Async(statusId, token), token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await anbarGardani.SaveAsync(system, 0, isGenerateSanad: true, isTemp: false, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1072);
				}
				break;
			}
			case Document.Resid:
			{
				ResidBusiness resid = await ResidBusiness.GetAsync(contextGuid, asGuid, token);
				if (resid != null)
				{
					resid.SetId2(await resid.NextId2Async(statusId, token), token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await resid.SaveAsync(0, null, default(CancellationToken), EnSystem.UserInterface, awaitOnSemaphore: true, 10), ReturnedState.None, "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1081);
				}
				break;
			}
			default:
				throw new NotImplementedException($"AsType({asType})");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1089);
			res.AddReturnedValue(ex, "", "FixId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1090);
		}
		return res;
	}

	internal static async Task<int> CheckCountAsync(string contextGuid, EnSystem system, int id, Document asType, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			switch (asType)
			{
			case Document.Reception:
			{
				KhazaneFilter filters = new KhazaneFilter
				{
					StatusId = id
				};
				return (await ReceptionBusiness.GetAllAsync(contextGuid, system, filters, token, tranName))?.Count() ?? 0;
			}
			case Document.Pardakht:
			{
				KhazaneFilter filter2 = new KhazaneFilter
				{
					StatusId = id
				};
				return (await PardakhtBusiness.GetAllAsync(contextGuid, system, filter2, token, tranName))?.Count() ?? 0;
			}
			case Document.OrderSell:
			case Document.OrderBuy:
			case Document.OrderBackBuy:
			case Document.OrderBackSell:
			case Document.PishFactor:
			case Document.Zayeat:
			case Document.AmaniVorod:
			case Document.AmaniKhoroj:
			case Document.OrderPreBuy:
			{
				OrderFilter filter7 = new OrderFilter
				{
					StatusId = id,
					Status = true,
					FactorType = asType
				};
				return (await OrderBusiness.GetAllAsync(contextGuid, system, filter7, token, tranName))?.Count() ?? 0;
			}
			case Document.AnbarGardani:
			{
				AnbarGardaniFilter filter3 = new AnbarGardaniFilter
				{
					Status = true,
					StatusId = id
				};
				return (await PrdAnbarGardaniBusiness.GetAllAsync(contextGuid, filter3, token, tranName))?.Count() ?? 0;
			}
			case Document.Sanad:
			{
				SanadFilter filter6 = new SanadFilter
				{
					Status = true,
					StatusId = id,
					DateFrom = new DateTime(1900, 1, 1),
					DateTo = new DateTime(2999, 1, 1)
				};
				return (await SanadBusiness.GetAllReportAsync(contextGuid, filter6, token, tranName))?.Count() ?? 0;
			}
			case Document.Resid:
			{
				ResidFilter filter4 = new ResidFilter
				{
					SelectedStatus = new List<int> { id }
				};
				return (await ResidBusiness.GetAllAsync(contextGuid, filter4, token, tranName))?.Count() ?? 0;
			}
			case Document.ChequeMOperation:
			{
				ChequeMOperationFilter filter5 = new ChequeMOperationFilter
				{
					StatusId = id
				};
				return (await ChequeMOperationBusiness.GetAllAsync(contextGuid, filter5, token, tranName))?.Count() ?? 0;
			}
			case Document.ChequeShOperation:
			{
				ChequeShOperationFilter filter = new ChequeShOperationFilter
				{
					StatusId = id
				};
				return (await ChequeShOperationBusiness.GetAllAsync(contextGuid, filter, token, tranName))?.Count() ?? 0;
			}
			default:
				return 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1156);
			throw;
		}
	}

	public async Task ForceUpdateStatusAsync(CancellationToken token)
	{
		if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
		{
			return;
		}
		await Task.Delay(120000);
		if (base.IsDisposed || token.IsCancellationRequested)
		{
			return;
		}
		while (!base.IsDisposed && !token.IsCancellationRequested)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 7;
				try
				{
					DateTime dateMoasser = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
					ClsCache cache = ClsCache.GetCache(ContextGuid);
					if (cache != null)
					{
						IEnumerable<SanadStatusReportForceUpdateModel> enumerable = await cache.CacheRepository.SanadStatusReport.GetAllForceUpdateAsync(dateMoasser, token, "");
						if (enumerable == null || !enumerable.Any())
						{
							await Task.Delay(1800000);
							continue;
						}
						foreach (SanadStatusReportForceUpdateModel item in enumerable)
						{
							if (item != null && item.SanadType != 0)
							{
								switch (item.SanadType)
								{
								case Document.Reception:
									await ReceptionBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								case Document.Pardakht:
									await PardakhtBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								case Document.OrderSell:
								case Document.OrderBuy:
								case Document.OrderBackBuy:
								case Document.OrderBackSell:
								case Document.PishFactor:
								case Document.Zayeat:
								case Document.AmaniVorod:
								case Document.AmaniKhoroj:
								case Document.OrderPreBuy:
									await OrderBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								case Document.AnbarGardani:
									await PrdAnbarGardaniBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								case Document.Sanad:
									await SanadBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								case Document.Resid:
									await ResidBusiness.ChangeStatusIdAsync(ContextGuid, item.SanadGuid, item.TargetStatusId, token, "");
									break;
								default:
									WebErrorLog.Instance.StartLog($"Type:{item.SanadType} For ForceStatusId in NotImplemented!!!!!!", "", "ForceUpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1201);
									break;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadStatusReportBusiness.cs", 1206);
				}
			}
			finally
			{
				await Task.Delay(1800000);
			}
			int num2;
			if (num2 == 2)
			{
				break;
			}
		}
	}
}
