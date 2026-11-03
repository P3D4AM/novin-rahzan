using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting.Khazane;

public class BaseKhazaneReportBusiness : Serializable<BaseKhazaneReportBusiness>, IBaseKhazaneReport, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private decimal sumHavale;

	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}:{DateM.Second}";

	public int Id2 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string TafsilName
	{
		get
		{
			string text = Tafsil?.Name ?? "";
			if (string.IsNullOrEmpty(text))
			{
				text = Moein?.Name ?? "";
			}
			return text;
		}
	}

	public string Description { get; set; }

	public decimal SumNaghd { get; set; }

	public decimal SumHavale
	{
		get
		{
			return sumHavale - SumBankPos;
		}
		set
		{
			sumHavale = value;
		}
	}

	public decimal SumChequeSh { get; set; }

	public decimal SumChequeM { get; set; }

	public decimal SumEnteghal { get; set; }

	public decimal Sum => SumHavale + SumEnteghal + SumNaghd + SumChequeM + SumChequeSh + SumBankPos;

	public bool IsChecked { get; set; }

	public int StatusId { get; set; }

	public ENSource Source { get; set; }

	public short SanadSt { get; set; }

	public byte[] SourceImage
	{
		get
		{
			if (Source == ENSource.Baje)
			{
				return ImageResourceManager.SourceBaje;
			}
			if (Source == ENSource.Android_HesabYar)
			{
				return ImageResourceManager.SourceHesabYar;
			}
			if (Source == ENSource.Android_SefareshGir)
			{
				return ImageResourceManager.SourceSefareshGir;
			}
			if (Source == ENSource.WebsiteWordPress)
			{
				return ImageResourceManager.SourceWordPress;
			}
			return ImageResourceManager.SourceAccounting;
		}
	}

	public byte[] ServerStatusImage
	{
		get
		{
			if (ServerStatus == ServerStatusForClient.Delivered)
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

	public byte[] SanadStatusImage
	{
		get
		{
			if (SanadSt == 2)
			{
				return ImageResourceManager.SanadStatusDaemi;
			}
			return ImageResourceManager.SanadStatusMovaghat;
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public string HostRefrenced { get; set; }

	public int SanadNo { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public bool IsTemp { get; set; }

	public int DeliveryId { get; set; }

	public decimal SumBankPos { get; set; }

	[JsonIgnore]
	public string SendToServerStatus
	{
		get
		{
			switch (ServerStatus)
			{
			case ServerStatusForClient.None:
				if (IsTemp)
				{
					return "موقت";
				}
				return "درحال ارسال...";
			case ServerStatusForClient.Sent:
				return ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
			case ServerStatusForClient.Delivered:
				return ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
			case ServerStatusForClient.SendError:
				return ServerStatus.GetDisplay();
			case ServerStatusForClient.DeliveryError:
				return ServerStatus.GetDisplay();
			case ServerStatusForClient.DirectDelivery:
				return $"{ServerStatus.GetDisplay()}({DeliveryId})";
			case ServerStatusForClient.LinkToOrder:
				return ServerStatus.GetDisplay();
			default:
				return $"خطا در تشخیص{ServerStatus}";
			}
		}
	}

	public EnKhazaneMaster Type { get; private set; }

	public string DeliveryShortMessage { get; set; }

	[JsonIgnore]
	public TafsilBusiness Tafsil => TafsilBusiness.Get(ContextGuid, TafsilGuid);

	[JsonIgnore]
	public MoeinBusiness Moein => MoeinBusiness.Get(ContextGuid, MoeinGuid);

	public BaseKhazaneReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<string> GetTafsilNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			string text = (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, TafsilGuid, tranName, token, "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\BaseKhazaneReportBusiness.cs"))?.Name ?? "";
			if (string.IsNullOrEmpty(text))
			{
				text = (await MoeinBusiness.GetAsync(ContextGuid, MoeinGuid, tranName, token))?.Name ?? "";
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\BaseKhazaneReportBusiness.cs", 130);
			throw;
		}
	}
}
