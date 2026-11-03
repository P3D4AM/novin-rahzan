using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Account;
using PacketParser.Connection.LowLevel.Accounting.Entities.CashierLog;
using PacketParser.Connection.LowLevel.Accounting.Entities.Crm;
using PacketParser.Connection.LowLevel.Accounting.Entities.Customer;
using PacketParser.Connection.LowLevel.Accounting.Entities.Hesab;
using PacketParser.Connection.LowLevel.Accounting.Entities.Kol;
using PacketParser.Connection.LowLevel.Accounting.Entities.LocalSetting;
using PacketParser.Connection.LowLevel.Accounting.Entities.MetaData;
using PacketParser.Connection.LowLevel.Accounting.Entities.Moein;
using PacketParser.Connection.LowLevel.Accounting.Entities.MoeinTafsilGroupRelations;
using PacketParser.Connection.LowLevel.Accounting.Entities.Note;
using PacketParser.Connection.LowLevel.Accounting.Entities.NoteGroup;
using PacketParser.Connection.LowLevel.Accounting.Entities.Order;
using PacketParser.Connection.LowLevel.Accounting.Entities.Reception;
using PacketParser.Connection.LowLevel.Accounting.Entities.VisitorArea;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace ClientHub.AccountingHubs;

public class AccountingClientHubInstances : INpAsyncDisposable, IAsyncDisposable
{
	private static Dictionary<string, AccountingClientHubInstances> _dic = new Dictionary<string, AccountingClientHubInstances>();

	private bool _isDisposed;

	public ITafsilSignalRClient Tafsil { get; set; }

	public IUserInfoSignalRClient UserInformation { get; set; }

	public ITicketPartSignalRClient TicketPart { get; set; }

	public ISanadStatusReportSignalRClient SanadStatusReport { get; set; }

	public IFileInfoSignalRClient FileInfo { get; set; }

	public ITafsilGroupSignalRClient TafsilGroup { get; set; }

	public ICashierSignalRClient Cashier { get; set; }

	public IUserAccSignalRClient UserAcc { get; set; }

	public ITicketSignalRClient Ticket { get; set; }

	public ITicketDetailSignalRClient TicketDetail { get; set; }

	public IContextHolderSignalRClient ContextHolder { get; set; }

	public IGardeshTafsilSignalRClient GardeshTafsil { get; set; }

	public ICheckedSaveSignalRClient CheckedSave { get; set; }

	public ISanadPrintSignalRClient SanadPrint { get; set; }

	public ICustomerCompactSignalRClient CustomerCompact { get; set; }

	public ITafsilNumberSignalRClient TafsilNumber { get; set; }

	public ICustomerSignalRClient Customer { get; set; }

	public IDescSignalRClient Desc { get; set; }

	public ICallingLogDetailSignalRClient CallingLogDetail { get; set; }

	public ILocalSettingSignalRClient LocalSetting { get; set; }

	public IOrderSignalRClient Order { get; set; }

	public IMetaDataSignalRClient MetaData { get; set; }

	public ICustomerRoleSignalRClient CustomerRole { get; set; }

	public IVisitorAreaSignalRClient VisitorArea { get; set; }

	public ICustomerGroupSignalRClient CustomerGroup { get; set; }

	public ICustomerTitelSignalRClient CustomerTitel { get; set; }

	public IMoeinSignalRClient Moein { get; set; }

	public IKolSignalRClient Kol { get; set; }

	public IAccountSignalRClient Account { get; set; }

	public IMoeinTafsilGroupRelationsSignalRClient MoeinTafsilGroupRelationsSignalRClient { get; set; }

	public INoteSignalRClient Note { get; set; }

	public INoteGroupSignalRClient NoteGroup { get; set; }

	public ICashierLogSignalRClient CashierLog { get; set; }

	public ICallingLogSignalRClient CallingLog { get; set; }

	public IReceptionSignalRClient Reception { get; set; }

	public bool IsDisposed => _isDisposed;

	public static AccountingClientHubInstances GetInstance(string contextGuid)
	{
		_dic.TryGetValue(contextGuid, out var value);
		return value;
	}

	public static void SetInstance(string contextGuid, AccountingClientHubInstances instance)
	{
		try
		{
			_dic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				_dic.Add(contextGuid, instance);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\AccountingClientHubInstances.cs", 82);
		}
	}

	public async ValueTask DisposeAsync()
	{
		_ = 34;
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (Tafsil != null)
				{
					await Tafsil.DisposeAsync();
					Tafsil = null;
				}
				if (UserInformation != null)
				{
					await UserInformation.DisposeAsync();
					UserInformation = null;
				}
				if (TicketPart != null)
				{
					await TicketPart.DisposeAsync();
					TicketPart = null;
				}
				if (SanadStatusReport != null)
				{
					await SanadStatusReport.DisposeAsync();
					SanadStatusReport = null;
				}
				if (FileInfo != null)
				{
					await FileInfo.DisposeAsync();
					FileInfo = null;
				}
				if (TafsilGroup != null)
				{
					await TafsilGroup.DisposeAsync();
					TafsilGroup = null;
				}
				if (Cashier != null)
				{
					await Cashier.DisposeAsync();
					Cashier = null;
				}
				if (UserAcc != null)
				{
					await UserAcc.DisposeAsync();
					UserAcc = null;
				}
				if (Ticket != null)
				{
					await Ticket.DisposeAsync();
					Ticket = null;
				}
				if (TicketDetail != null)
				{
					await TicketDetail.DisposeAsync();
					TicketDetail = null;
				}
				if (ContextHolder != null)
				{
					await ContextHolder.DisposeAsync();
					ContextHolder = null;
				}
				if (GardeshTafsil != null)
				{
					await GardeshTafsil.DisposeAsync();
					GardeshTafsil = null;
				}
				if (CheckedSave != null)
				{
					await CheckedSave.DisposeAsync();
					CheckedSave = null;
				}
				if (SanadPrint != null)
				{
					await SanadPrint.DisposeAsync();
					SanadPrint = null;
				}
				if (CustomerCompact != null)
				{
					await CustomerCompact.DisposeAsync();
					CustomerCompact = null;
				}
				if (TafsilNumber != null)
				{
					await TafsilNumber.DisposeAsync();
					TafsilNumber = null;
				}
				if (Customer != null)
				{
					await Customer.DisposeAsync();
					Customer = null;
				}
				if (CheckedSave != null)
				{
					await CheckedSave.DisposeAsync();
					CheckedSave = null;
				}
				if (Desc != null)
				{
					await Desc.DisposeAsync();
					Desc = null;
				}
				if (CallingLogDetail != null)
				{
					await CallingLogDetail.DisposeAsync();
					CallingLogDetail = null;
				}
				if (Order != null)
				{
					await Order.DisposeAsync();
					Order = null;
				}
				if (MetaData != null)
				{
					await MetaData.DisposeAsync();
					MetaData = null;
				}
				if (CustomerRole != null)
				{
					await CustomerRole.DisposeAsync();
					CustomerRole = null;
				}
				if (VisitorArea != null)
				{
					await VisitorArea.DisposeAsync();
					VisitorArea = null;
				}
				if (CustomerGroup != null)
				{
					await CustomerGroup.DisposeAsync();
					CustomerGroup = null;
				}
				if (CustomerTitel != null)
				{
					await CustomerTitel.DisposeAsync();
					CustomerTitel = null;
				}
				if (Moein != null)
				{
					await Moein.DisposeAsync();
					Moein = null;
				}
				if (Kol != null)
				{
					await Kol.DisposeAsync();
					Kol = null;
				}
				if (Account != null)
				{
					await Account.DisposeAsync();
					Account = null;
				}
				if (MoeinTafsilGroupRelationsSignalRClient != null)
				{
					await MoeinTafsilGroupRelationsSignalRClient.DisposeAsync();
					MoeinTafsilGroupRelationsSignalRClient = null;
				}
				if (Note != null)
				{
					await Note.DisposeAsync();
					Note = null;
				}
				if (NoteGroup != null)
				{
					await NoteGroup.DisposeAsync();
					NoteGroup = null;
				}
				if (CashierLog != null)
				{
					await CashierLog.DisposeAsync();
					CashierLog = null;
				}
				if (CallingLog != null)
				{
					await CallingLog.DisposeAsync();
					CallingLog = null;
				}
				if (Reception != null)
				{
					await Reception.DisposeAsync();
					Reception = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\AccountingClientHubInstances.cs", 267);
		}
	}
}
