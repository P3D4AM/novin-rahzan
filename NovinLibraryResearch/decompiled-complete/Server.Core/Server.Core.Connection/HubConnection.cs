using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Connection;
using PacketParser.LogPacket;
using PacketParser.TaxService;
using Serilog;
using Server.Core.Assistance;
using Server.Core.Connection.Dal;
using Server.Core.Connection.Dal.Cheque;
using Server.Core.Connection.Dal.Department;
using Server.Core.Connection.Dal.TcpConnection.Attendance;
using SharedServer.Core.Connection.Dal;
using SharedServer.Core.Connection.Dal.AttPersonLeave;

namespace Server.Core.Connection;

public class HubConnection
{
	private static bool _initialized;

	public static ITaxConnectionManager TaxConnectionManager { get; private set; }

	private static void HandleEvents(IEnumerable<WebContextHolder> contexts)
	{
		try
		{
			if (contexts != null)
			{
				foreach (WebContextHolder context in contexts)
				{
					clsTaxService @object = new clsTaxService(TaxConnectionManager);
					ClsOrder object2 = new ClsOrder(TaxConnectionManager);
					if (context != null)
					{
						context.LoginExtention.OnLoginRequest -= Instance_OnLogin;
						context.LoginExtention.OnLoginRequest += Instance_OnLogin;
						context.WebLkInfo.OnSaveRequest -= ClsLkInfo.Instance.WebLkInfo_OnSaveRequest;
						context.WebLkInfo.OnSaveRequest += ClsLkInfo.Instance.WebLkInfo_OnSaveRequest;
						context.WebCusTitle.OnSaveRequest -= ClsCusTitle.CusTitleInstance.CusTitleInstance_OnSaveRequest;
						context.WebCusTitle.OnSaveRequest += ClsCusTitle.CusTitleInstance.CusTitleInstance_OnSaveRequest;
						context.CustomerRole.OnSaveRequest -= ClsCustomerRole.CustomerRoleInstance.CustomerRoleInstance_OnSaveRequest;
						context.CustomerRole.OnSaveRequest += ClsCustomerRole.CustomerRoleInstance.CustomerRoleInstance_OnSaveRequest;
						context.AghsatMGroup.OnSaveRequest -= clsAghsatMGroup.Instance.AghsatMGroupInstance_OnSaveRequestAsync;
						context.AghsatMGroup.OnSaveRequest += clsAghsatMGroup.Instance.AghsatMGroupInstance_OnSaveRequestAsync;
						context.WebAghsatAshkhas.OnSaveRequest -= ClsAghsatAshkhas.AghsatAshkhasInstance.AghsatAshkhasInstance_OnSaveRequest;
						context.WebAghsatAshkhas.OnSaveRequest += ClsAghsatAshkhas.AghsatAshkhasInstance.AghsatAshkhasInstance_OnSaveRequest;
						context.WebAghsatShakhsi.OnSaveRequest -= ClsAghsatShakhsi.AghsatShakhsiInstance.AghsatShakhsiInstance_OnSaveRequest;
						context.WebAghsatShakhsi.OnSaveRequest += ClsAghsatShakhsi.AghsatShakhsiInstance.AghsatShakhsiInstance_OnSaveRequest;
						context.WebApplicationSetting.OnSaveRequest -= ClsAppicationSettings.AppicationSettingsInstance.AppSettingInstance_OnSaveRequest;
						context.WebApplicationSetting.OnSaveRequest += ClsAppicationSettings.AppicationSettingsInstance.AppSettingInstance_OnSaveRequest;
						context.WebCallingLog.OnSaveRequest -= ClsCallingLog.CallingLogInstance.CallingLogInstance_OnSaveRequest;
						context.WebCallingLog.OnSaveRequest += ClsCallingLog.CallingLogInstance.CallingLogInstance_OnSaveRequest;
						context.WebContactUs.OnSaveRequest -= ClsContactsUs.ContactsUsInstance.ContactUsInstance_OnSaveRequestAsync;
						context.WebContactUs.OnSaveRequest += ClsContactsUs.ContactsUsInstance.ContactUsInstance_OnSaveRequestAsync;
						context.WebCus.OnTempSaveRequest -= ClsCustomer.CustomerInstance_OnTempSaveRequest;
						context.WebCus.OnTempSaveRequest += ClsCustomer.CustomerInstance_OnTempSaveRequest;
						context.WebCus.OnTempCommitRequest -= ClsCustomer.CustomerInstance_OnTempCommitRequest;
						context.WebCus.OnTempCommitRequest += ClsCustomer.CustomerInstance_OnTempCommitRequest;
						context.WebCus.OnPasswordRecoveryRequest -= ClsCustomer.CustomerInstance_OnPasswordRecoveryRequest;
						context.WebCus.OnPasswordRecoveryRequest += ClsCustomer.CustomerInstance_OnPasswordRecoveryRequest;
						context.WebCus.OnSaveRequest -= ClsCustomer.CusInstance.CustomerInstance_OnSaveRequest;
						context.WebCus.OnSaveRequest += ClsCustomer.CusInstance.CustomerInstance_OnSaveRequest;
						context.WebCus.OnReadResponse -= ClsCustomer.CusInstance.WebCus_OnReadResponse;
						context.WebCus.OnReadResponse += ClsCustomer.CusInstance.WebCus_OnReadResponse;
						context.WebCusGroup.OnSaveRequest -= ClsCusGroup.CusGroupInstance.CusGroupInstance_OnSaveRequest;
						context.WebCusGroup.OnSaveRequest += ClsCusGroup.CusGroupInstance.CusGroupInstance_OnSaveRequest;
						context.WebDelivery.OnReadRequest -= ClsDeliveryPartitioned.DeliveriInstance.Delivery_OnReadRequest;
						context.WebDelivery.OnReadRequest += ClsDeliveryPartitioned.DeliveriInstance.Delivery_OnReadRequest;
						context.WebDelivery.OnReadResponse -= ClsDeliveryPartitioned.DeliveriInstance.ClsDeliveryPartitioned_OnReadResponse;
						context.WebDelivery.OnReadResponse += ClsDeliveryPartitioned.DeliveriInstance.ClsDeliveryPartitioned_OnReadResponse;
						context.WebDepot.OnSaveRequest -= ClsDepot.DepotInstance.DepotInstance_OnSaveRequest;
						context.WebDepot.OnSaveRequest += ClsDepot.DepotInstance.DepotInstance_OnSaveRequest;
						context.WebDesc.OnSaveRequest -= ClsDesc.DescInstance.DescInstance_OnSaveRequest;
						context.WebDesc.OnSaveRequest += ClsDesc.DescInstance.DescInstance_OnSaveRequest;
						context.WebIntroductionMethod.OnSaveRequest -= ClsIntroductionMethod.IntroductionMethodInstance.DescInstance_OnSaveRequest;
						context.WebIntroductionMethod.OnSaveRequest += ClsIntroductionMethod.IntroductionMethodInstance.DescInstance_OnSaveRequest;
						context.WebEntitySync.OnSaveRequest -= ClsSyncEntity.Instance.SyncInstance_OnSaveRequest;
						context.WebEntitySync.OnSaveRequest += ClsSyncEntity.Instance.SyncInstance_OnSaveRequest;
						context.WebErrorLogSum.OnReadRequest -= ErrorLogInstance_OnReadRequest;
						context.WebErrorLogSum.OnReadRequest += ErrorLogInstance_OnReadRequest;
						context.WebFileInfo.OnWriteRequest -= ClsFileInfo.FileInfoInstance.FileInstance_OnWriteRequest;
						context.WebFileInfo.OnWriteRequest += ClsFileInfo.FileInfoInstance.FileInstance_OnWriteRequest;
						context.WebFileInfo.OnSaveRequest -= ClsFileInfo.FileInfoInstance.WebFileInfo_OnSaveRequest;
						context.WebFileInfo.OnSaveRequest += ClsFileInfo.FileInfoInstance.WebFileInfo_OnSaveRequest;
						context.WebFileInfo.OnDeleteRequest -= ClsFileInfo.FileInfoInstance.FileInstance_OnDeleteRequest;
						context.WebFileInfo.OnDeleteRequest += ClsFileInfo.FileInfoInstance.FileInstance_OnDeleteRequest;
						context.WebFileInfo.OnCommitRequest -= ClsFileInfo.FileInfoInstance.FileInstance_OnCommitRequest;
						context.WebFileInfo.OnCommitRequest += ClsFileInfo.FileInfoInstance.FileInstance_OnCommitRequest;
						context.WebGPS.OnSaveRequest -= ClsGps.GpsInstance.GpsInstance_OnSaveRequest;
						context.WebGPS.OnSaveRequest += ClsGps.GpsInstance.GpsInstance_OnSaveRequest;
						context.WebProduct.OnSaveRequest -= ClsProduct.ProductInstance.ProductInstance_OnSaveRequest;
						context.WebProduct.OnSaveRequest += ClsProduct.ProductInstance.ProductInstance_OnSaveRequest;
						context.WebProduct.OnReadRequest -= ClsProduct.ProductInstance.ProductInstance_OnReadRequest;
						context.WebProduct.OnReadRequest += ClsProduct.ProductInstance.ProductInstance_OnReadRequest;
						context.WebProduct.OnReadResponse -= ClsProduct.ProductInstance.ProductInstance_OnReadResponse;
						context.WebProduct.OnReadResponse += ClsProduct.ProductInstance.ProductInstance_OnReadResponse;
						context.WebPrdGroup.OnSaveRequest -= ClsProductGroup.ProductGroupInstance.PrdGroupInstance_OnSaveRequest;
						context.WebPrdGroup.OnSaveRequest += ClsProductGroup.ProductGroupInstance.PrdGroupInstance_OnSaveRequest;
						context.WebPrdVahed.OnSaveRequest -= ClsProductVahed.ProductVahedInstance.PrdVahedInstance_OnSaveRequest;
						context.WebPrdVahed.OnSaveRequest += ClsProductVahed.ProductVahedInstance.PrdVahedInstance_OnSaveRequest;
						context.WebNews.OnReadRequest -= ClsNews.NewsInstance.NewsInstance_OnReadRequest;
						context.WebNews.OnReadRequest += ClsNews.NewsInstance.NewsInstance_OnReadRequest;
						context.WebNews.OnSaveRequest -= ClsNews.NewsInstance.NewsInstance_OnSaveRequest;
						context.WebNews.OnSaveRequest += ClsNews.NewsInstance.NewsInstance_OnSaveRequest;
						context.WebHyperLink.OnReadRequest -= ClsHyperLink.HyperLinkInstance.HyperLinkInstance_OnReadRequest;
						context.WebHyperLink.OnReadRequest += ClsHyperLink.HyperLinkInstance.HyperLinkInstance_OnReadRequest;
						context.WebHyperLink.OnSaveRequest -= ClsHyperLink.HyperLinkInstance.HyperLinkInstance_OnSaveRequest;
						context.WebHyperLink.OnSaveRequest += ClsHyperLink.HyperLinkInstance.HyperLinkInstance_OnSaveRequest;
						context.WebUserAcc.OnSaveRequest -= ClsUserAcc.UserAccInstance.UserAccInstance_OnSaveRequest;
						context.WebUserAcc.OnSaveRequest += ClsUserAcc.UserAccInstance.UserAccInstance_OnSaveRequest;
						context.WebCustomerAddress.OnSaveRequest -= ClsCustomerAddress.CustomerAddressInstance_OnSaveRequest;
						context.WebCustomerAddress.OnSaveRequest += ClsCustomerAddress.CustomerAddressInstance_OnSaveRequest;
						context.WebOrder.OnReadRequest -= object2.FactorInstance_OnReadRequest;
						context.WebOrder.OnReadRequest += object2.FactorInstance_OnReadRequest;
						context.WebOrder.OnSaveRequest -= object2.FactorInstance_OnSaveRequest;
						context.WebOrder.OnSaveRequest += object2.FactorInstance_OnSaveRequest;
						context.WebOrder.OnSaveTempRequest -= object2.FactorInstance_OnSaveTempRequest;
						context.WebOrder.OnSaveTempRequest += object2.FactorInstance_OnSaveTempRequest;
						context.WebReception.OnReadRequest -= ClsReception.ReceptionInstance.ReceptionInstance_OnReadRequest;
						context.WebReception.OnReadRequest += ClsReception.ReceptionInstance.ReceptionInstance_OnReadRequest;
						context.WebReception.OnSaveRequest -= ClsReception.ReceptionInstance.ReceptionInstance_OnSaveRequest;
						context.WebReception.OnSaveRequest += ClsReception.ReceptionInstance.ReceptionInstance_OnSaveRequest;
						context.WebPardakht.OnSaveRequest -= ClsPardakht.PardakhtInstance.PardakhtInstance_OnSaveRequest;
						context.WebPardakht.OnSaveRequest += ClsPardakht.PardakhtInstance.PardakhtInstance_OnSaveRequest;
						context.WebPardakht.OnReadRequest -= ClsPardakht.PardakhtInstance.PardakhtInstance_OnReadRequest;
						context.WebPardakht.OnReadRequest += ClsPardakht.PardakhtInstance.PardakhtInstance_OnReadRequest;
						context.WebHesabTree.OnSaveRequest -= ClsHesabTree.HesabTreeInstance.HesabTreeInstance_OnSaveRequest;
						context.WebHesabTree.OnSaveRequest += ClsHesabTree.HesabTreeInstance.HesabTreeInstance_OnSaveRequest;
						context.WebVisit.OnSaveRequest -= ClsVisit.VisitInstance.VisitInstance_OnSaveRequest;
						context.WebVisit.OnSaveRequest += ClsVisit.VisitInstance.VisitInstance_OnSaveRequest;
						context.WebTafsilGroup.OnSaveRequest -= ClsTafsilGroup.TafsilGroupInstance.TafsilLevelInstance_OnSaveRequest;
						context.WebTafsilGroup.OnSaveRequest += ClsTafsilGroup.TafsilGroupInstance.TafsilLevelInstance_OnSaveRequest;
						context.WebViewCheckM.OnSaveRequest -= ClsViewCheckM.ViewCheckMInstance.ViewCheckMInstance_OnSaveRequest;
						context.WebViewCheckM.OnSaveRequest += ClsViewCheckM.ViewCheckMInstance.ViewCheckMInstance_OnSaveRequest;
						context.WebViewCheckSh.OnSaveRequest -= ClsViewCheckSh.ViewCheckShInstance.ViewCheckShInstance_OnSaveRequest;
						context.WebViewCheckSh.OnSaveRequest += ClsViewCheckSh.ViewCheckShInstance.ViewCheckShInstance_OnSaveRequest;
						context.WebKol.OnSaveRequest -= ClsKol.KolInstance.KolInstance_OnSaveRequest;
						context.WebKol.OnSaveRequest += ClsKol.KolInstance.KolInstance_OnSaveRequest;
						context.WebMoein.OnSaveRequest -= ClsMoein.MoeinInstance.MoeinInstance_OnSaveRequest;
						context.WebMoein.OnSaveRequest += ClsMoein.MoeinInstance.MoeinInstance_OnSaveRequest;
						context.WebTafsil.OnSaveRequest -= ClsTafsil.TafsilInstance.TafsilInstance_OnSaveRequest;
						context.WebTafsil.OnSaveRequest += ClsTafsil.TafsilInstance.TafsilInstance_OnSaveRequest;
						context.Packet.OnResponse -= PacketInstance_OnResponse;
						context.Packet.OnResponse += PacketInstance_OnResponse;
						context.WebLastPrice.OnSaveRequest -= ClsLastPrice.Instance.LastPriceInstance_OnSaveRequest;
						context.WebLastPrice.OnSaveRequest += ClsLastPrice.Instance.LastPriceInstance_OnSaveRequest;
						context.WebUserLog.OnSaveRequest -= ClsUserLog.UserLogInstance.UserLogInstance_OnSaveRequest;
						context.WebUserLog.OnSaveRequest += ClsUserLog.UserLogInstance.UserLogInstance_OnSaveRequest;
						context.WebExecuteQuery.OnExecuteQueryRequest -= ClsExecuteQuery.QueryInstance.WebExecuteQuery_OnExecuteQueryRequest;
						context.WebExecuteQuery.OnExecuteQueryRequest += ClsExecuteQuery.QueryInstance.WebExecuteQuery_OnExecuteQueryRequest;
						context.WebChequeMOperation.OnSaveRequest -= ChequeMOperation.ChequeMOperationInstance.WebChequeMOperation_OnSaveRequest;
						context.WebChequeMOperation.OnSaveRequest += ChequeMOperation.ChequeMOperationInstance.WebChequeMOperation_OnSaveRequest;
						context.WebChequeShOperation.OnSaveRequest -= ChequeShOperation.ChequeShOperationInstance.WebChequeShOperation_OnSaveRequest;
						context.WebChequeShOperation.OnSaveRequest += ChequeShOperation.ChequeShOperationInstance.WebChequeShOperation_OnSaveRequest;
						context.WebSanad.OnSaveRequest -= ClsSanad.SanadInstance.SanadInstance_OnSaveRequest;
						context.WebSanad.OnSaveRequest += ClsSanad.SanadInstance.SanadInstance_OnSaveRequest;
						context.WebPrdAnbarGardani.OnSaveRequest -= ClsPrdAnbarGardani.PrdAnbarGardaniInstance.PrdAnbarGardaniInstance_OnSaveRequest;
						context.WebPrdAnbarGardani.OnSaveRequest += ClsPrdAnbarGardani.PrdAnbarGardaniInstance.PrdAnbarGardaniInstance_OnSaveRequest;
						context.WebPrdSerial.OnSaveRequest -= clsPrdSerial.Instance.Instance_OnSaveRequest;
						context.WebPrdSerial.OnSaveRequest += clsPrdSerial.Instance.Instance_OnSaveRequest;
						context.WebAccount.OnSaveRequest -= clsAccount.Instance.AccountInstance_OnSaveRequest;
						context.WebAccount.OnSaveRequest += clsAccount.Instance.AccountInstance_OnSaveRequest;
						context.Note.OnSaveRequest -= ClsNote.Instance.NoteInstance_OnSaveRequest;
						context.Note.OnSaveRequest += ClsNote.Instance.NoteInstance_OnSaveRequest;
						context.TaxLogin.OnLoginRequest -= @object.TaxInstance_OnLoginRequestAsync;
						context.TaxLogin.OnLoginRequest += @object.TaxInstance_OnLoginRequestAsync;
						context.WebOrder.OnSendToTaxServiceRequest -= object2.OrderInstance_OnSendToTaxServiceRequestAsync;
						context.WebOrder.OnSendToTaxServiceRequest += object2.OrderInstance_OnSendToTaxServiceRequestAsync;
						context.WebOrder.OnTaxEconomicCodeStateRequest -= object2.OrderInstance_OnTaxEconomicCodeStateRequestAsync;
						context.WebOrder.OnTaxEconomicCodeStateRequest += object2.OrderInstance_OnTaxEconomicCodeStateRequestAsync;
						context.WebOrder.OnTaxServiceTrackRequest -= object2.OrderInstance_OnTaxServiceTracykingRequestAsync;
						context.WebOrder.OnTaxServiceTrackRequest += object2.OrderInstance_OnTaxServiceTracykingRequestAsync;
						context.AttPersonLeave.OnSaveRequest -= ClsAttPersonLeave.Instance.AttPersonLeaveInstance_OnSaveRequestAsync;
						context.AttPersonLeave.OnSaveRequest += ClsAttPersonLeave.Instance.AttPersonLeaveInstance_OnSaveRequestAsync;
						context.DiscoupntCoupon.OnSaveRequest -= clsDiscountCoupon.Instance.Instance_OnSaveRequest;
						context.DiscoupntCoupon.OnSaveRequest += clsDiscountCoupon.Instance.Instance_OnSaveRequest;
						context.AttDepartment.OnSaveRequest -= ClsAttDepartment.Instance.Instance_OnSaveRequest;
						context.AttDepartment.OnSaveRequest += ClsAttDepartment.Instance.Instance_OnSaveRequest;
						context.Tax.OnSaveRequest -= clsTaxClass.Instance.Instance_OnSaveRequest;
						context.Tax.OnSaveRequest += clsTaxClass.Instance.Instance_OnSaveRequest;
						context.TaxOrderLog.OnSaveRequest -= clsTaxOrderLog.Instance.InstanceOnSaveRequestAsync;
						context.TaxOrderLog.OnSaveRequest += clsTaxOrderLog.Instance.InstanceOnSaveRequestAsync;
						context.ProductPriceLevel.OnSaveRequest -= clsProductPriceLevel.Instance.OnSaveRequestAsync;
						context.ProductPriceLevel.OnSaveRequest += clsProductPriceLevel.Instance.OnSaveRequestAsync;
						context.SanadStatusReport.OnSaveRequest -= clsSanadStatusReport.Instance.OnSaveRequestAsync;
						context.SanadStatusReport.OnSaveRequest += clsSanadStatusReport.Instance.OnSaveRequestAsync;
						context.DeviceLogOnPerson.OnSaveRequest -= clsDeviceCommunicationService.Instance.Instance_OnSaveRequest;
						context.DeviceLogOnPerson.OnSaveRequest += clsDeviceCommunicationService.Instance.Instance_OnSaveRequest;
						context.SanadPrint.OnSaveRequest -= clsSanadPrint.Instance.Instance_OnSaveRequest;
						context.SanadPrint.OnSaveRequest += clsSanadPrint.Instance.Instance_OnSaveRequest;
					}
				}
			}
			ServerFactory.Instance.DataReceived -= Server_DataReceived;
			ServerFactory.Instance.DataReceived += Server_DataReceived;
			ServerFactory.Instance.Disconnected -= Server_OnDisconnected;
			ServerFactory.Instance.Disconnected += Server_OnDisconnected;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleEvents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 281);
		}
	}

	public static void Init(IEnumerable<WebContextHolder> contexts, short countOfReceiveDataFromWp, ITaxConnectionManager taxManager)
	{
		try
		{
			if (!_initialized)
			{
				_initialized = true;
				TaxConnectionManager = taxManager;
				HandleEvents(contexts);
				if (Server.Core.Assistance.Initializer.Dal.FileInfo != null)
				{
					Task.Run((Func<Task<ReturnedSaveFuncInfo>?>)Server.Core.Assistance.Initializer.Dal.FileInfo.DeleteTempsAsync);
					Task.Run((Func<Task?>)Server.Core.Assistance.Initializer.Dal.FileInfo.UpdateAllFilesCrcAsync);
					Task.Run((Func<Task?>)Server.Core.Assistance.Initializer.Dal.FileInfo.FillUrlAsync);
				}
				if (Server.Core.Assistance.Initializer.Dal.Product != null)
				{
					Task.Run((Func<Task?>)Server.Core.Assistance.Initializer.Dal.Product.UpdateBarcodeRelationsAsync);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 302);
		}
	}

	private static async Task PacketInstance_OnResponse(object sender, BaseEventArgs e)
	{
		using (new JaegerMonitor("PacketInstance_OnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Packet, 1, isReceivedPacket: true);
				if (e != null)
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.PacketDeliveredAsync(e.PacketReferenceId, e.Part);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PacketInstance_OnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 318);
			}
		}
	}

	private static async Task Server_OnSendRequst(object sender, LoggerSendEventArgs e)
	{
		using (new JaegerMonitor("Server_OnSendRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			try
			{
				await Task.Run(() => Server.Core.Assistance.Initializer.Dal.SignalRLogger.SlogSendReq_InsertAsync(e.pack, e.Source, e.DepId, e.UserId, e.Connections));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Server_OnSendRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 327);
			}
		}
	}

	private static async Task Server_OnSent(object sender, LoggerSendEventArgs e)
	{
		using (new JaegerMonitor("Server_OnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			try
			{
				await Task.Run(() => Server.Core.Assistance.Initializer.Dal.SignalRLogger.SlogSent_InsertAsync(e.pack, e.Connections));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Server_OnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 337);
			}
		}
	}

	private static async Task ErrorLogInstance_OnReadRequest(object sender, ErrorLogSumEventArgs e)
	{
		using (new JaegerMonitor("ErrorLogInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			_ = 1;
			try
			{
				throw new Exception("please fill action for ExpiredPacketLogger");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ErrorLogInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 356);
			}
		}
	}

	private static async Task Server_DataReceived(object sender, ConnectionDataEventArgs e)
	{
		_ = 1;
		try
		{
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
			if (connection == null)
			{
				await Packet.ParseLoginPacket(e.context, e.Data, e.ConnectionUniqueId, connection, e.Sb);
			}
			else
			{
				await connection.RecievePacketAndParse(Packet.GetPacket(e.Data, e.Sb), e.context, e.ConnectionUniqueId, e.Ip, e.Sb);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Server_DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 372);
		}
	}

	private static Task Server_OnDisconnected(object sender, ConnectionDataEventArgs e)
	{
		using (new JaegerMonitor("Server_OnDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			try
			{
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				connection?.Dispose("HubConnection.Server_OnDisconnected");
				if (connection != null)
				{
					Task.Run(() => ConnectionManager.Instance.UnHandleConnection(connection, "Server_OnDisconnected", connection.Source));
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Server_OnDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 387);
			}
			return Task.CompletedTask;
		}
	}

	private static async Task Instance_OnLogin(object sender, LoginRequestEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs"))
		{
			_ = 3;
			try
			{
				if (!string.IsNullOrEmpty(e.ConnectionData.Username) && e.ConnectionData.Username.ToLower() == "clientacc")
				{
					return;
				}
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Login, 1, isReceivedPacket: true);
				ClientLoginHistory loginHistory = new ClientLoginHistory
				{
					LoginDateM = DateTime.Now,
					ConnectionUniqueId = e.ConnectionUniqueId
				};
				LoginExtention res = await ConnectionManager.Instance.ProcessLoginAsync(e, default(CancellationToken));
				if (e.ConnectionData.ClientTime.HasValue && Math.Abs((e.ConnectionData.ClientTime.Value - DateTime.Now).TotalMinutes) >= 5.0)
				{
					Serilog.Log.Debug($"Client Time is wrong.DepId:{e.ConnectionData?.UserInfo?.DepId}.User:{e.ConnectionData.Username},ClientTime:{e.ConnectionData.ClientTime}.ServerTime:{DateTime.Now}");
				}
				ClsConnectionData connectionData = null;
				loginHistory.LoadFromLoginExtention(e.ConnectionData);
				loginHistory.Ip = e?.Ip ?? "";
				if (res.Status == EnLoginResult.Success)
				{
					connectionData = await ConnectionManager.Instance.HandleConnection(e.Context, e.ConnectionUniqueId, res.UserInfo.DepId, e.ConnectionData, res.UserInfo, e.Ip, TaxConnectionManager);
					if ((res.UserInfo?.SyncInfo?.Token ?? Guid.Empty) == Guid.Empty)
					{
						WebUserInformation userInfo = res.UserInfo;
						userInfo.SyncInfo = (await Server.Core.Assistance.Initializer.Dal.Sync.LoadDataAsync(connectionData.HashId, default(CancellationToken)))?.value;
					}
					loginHistory.HashId = connectionData?.HashId ?? 0;
				}
				else
				{
					loginHistory.DisconnectDateM = DateTime.Now;
					loginHistory.DisconnectDescription = res.LoginResultMessage;
				}
				if (Server.Core.Assistance.Initializer.Dal.ClientLoginHistory != null)
				{
					Task.Run(() => Server.Core.Assistance.Initializer.Dal.ClientLoginHistory.InsertLogAsync(loginHistory, 10));
				}
				await ServerFactory.Instance.SendToClient(res.GenerateLoginStatusPacket(e.Context), e.Context, e.ConnectionUniqueId, e.ConnectionData.CompressedMode, "instance_onLogin!");
				if ((e?.ConnectionData?.SyncRequested).GetValueOrDefault() && connectionData != null)
				{
					Task.Run(() => connectionData.SyncAsync(e.ConnectionData.DepartmentEntitySummery));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\HubConnection.cs", 428);
			}
		}
	}
}
