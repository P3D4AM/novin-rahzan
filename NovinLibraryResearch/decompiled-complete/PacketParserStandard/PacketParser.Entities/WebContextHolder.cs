using System;
using System.Collections.Concurrent;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.CRM;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebContextHolder : INpDisposable, IDisposable
{
	private static ConcurrentDictionary<string, WebContextHolder> _contexts = new ConcurrentDictionary<string, WebContextHolder>();

	public string Guid { get; private set; }

	[JsonIgnore]
	[XmlIgnore]
	public WebCus WebCus { get; set; } = new WebCus();


	[JsonIgnore]
	[XmlIgnore]
	public WebCashier WebCashier { get; set; } = new WebCashier();


	[JsonIgnore]
	[XmlIgnore]
	public WebCusTitle WebCusTitle { get; set; } = new WebCusTitle();


	[JsonIgnore]
	[XmlIgnore]
	public LoginExtention LoginExtention { get; set; } = new LoginExtention();


	[JsonIgnore]
	[XmlIgnore]
	public DateTimeManager DateTimeManager { get; set; } = new DateTimeManager();


	[JsonIgnore]
	[XmlIgnore]
	public WebAghsatAshkhas WebAghsatAshkhas { get; set; } = new WebAghsatAshkhas();


	[JsonIgnore]
	[XmlIgnore]
	public WebAghsatShakhsi WebAghsatShakhsi { get; set; } = new WebAghsatShakhsi();


	[JsonIgnore]
	[XmlIgnore]
	public WebApplicationSetting WebApplicationSetting { get; set; } = new WebApplicationSetting();


	[JsonIgnore]
	[XmlIgnore]
	public WebCallingLog WebCallingLog { get; set; } = new WebCallingLog();


	[JsonIgnore]
	[XmlIgnore]
	public WebContactUs WebContactUs { get; set; } = new WebContactUs();


	[JsonIgnore]
	[XmlIgnore]
	public WebCusGroup WebCusGroup { get; set; } = new WebCusGroup();


	[JsonIgnore]
	[XmlIgnore]
	public WebDelivery WebDelivery { get; set; } = new WebDelivery();


	[JsonIgnore]
	[XmlIgnore]
	public WebDepot WebDepot { get; set; } = new WebDepot();


	[JsonIgnore]
	[XmlIgnore]
	public WebDesc WebDesc { get; set; } = new WebDesc();


	[JsonIgnore]
	[XmlIgnore]
	public WebReception WebReception { get; set; } = new WebReception();


	[JsonIgnore]
	[XmlIgnore]
	public WebPardakht WebPardakht { get; set; } = new WebPardakht();


	[JsonIgnore]
	[XmlIgnore]
	public WebOrder WebOrder { get; set; } = new WebOrder();


	[JsonIgnore]
	[XmlIgnore]
	public WebProduct WebProduct { get; set; } = new WebProduct();


	[JsonIgnore]
	[XmlIgnore]
	public WebPrdGroup WebPrdGroup { get; set; } = new WebPrdGroup();


	[JsonIgnore]
	[XmlIgnore]
	public WebErrorLogSum WebErrorLogSum { get; set; } = new WebErrorLogSum();


	[JsonIgnore]
	[XmlIgnore]
	public WebFileInfo WebFileInfo { get; set; } = new WebFileInfo();


	[JsonIgnore]
	[XmlIgnore]
	public WebHesabTree WebHesabTree { get; set; } = new WebHesabTree();


	[JsonIgnore]
	[XmlIgnore]
	public WebGPS WebGPS { get; set; } = new WebGPS();


	[JsonIgnore]
	[XmlIgnore]
	public WebPrdVahed WebPrdVahed { get; set; } = new WebPrdVahed();


	[JsonIgnore]
	[XmlIgnore]
	public WebNews WebNews { get; set; } = new WebNews();


	[JsonIgnore]
	[XmlIgnore]
	public WebUserAcc WebUserAcc { get; set; } = new WebUserAcc();


	[JsonIgnore]
	[XmlIgnore]
	public WebHyperLink WebHyperLink { get; set; } = new WebHyperLink();


	[JsonIgnore]
	[XmlIgnore]
	public WebTelegramMessage WebTelegramMessage { get; set; } = new WebTelegramMessage();


	[JsonIgnore]
	[XmlIgnore]
	public Packet Packet { get; set; } = new Packet();


	[JsonIgnore]
	[XmlIgnore]
	public WebExecuteQuery WebExecuteQuery { get; set; } = new WebExecuteQuery();


	[JsonIgnore]
	[XmlIgnore]
	public WebTicketPart WebTicketPart { get; set; } = new WebTicketPart();


	[JsonIgnore]
	[XmlIgnore]
	public WebTicketDet WebTicketDet { get; set; } = new WebTicketDet();


	[JsonIgnore]
	[XmlIgnore]
	public WebTicket WebTicket { get; set; } = new WebTicket();


	[JsonIgnore]
	[XmlIgnore]
	public WebVisit WebVisit { get; set; } = new WebVisit();


	[JsonIgnore]
	[XmlIgnore]
	public WebViewCheckM WebViewCheckM { get; set; } = new WebViewCheckM();


	[JsonIgnore]
	[XmlIgnore]
	public WebViewCheckSh WebViewCheckSh { get; set; } = new WebViewCheckSh();


	[JsonIgnore]
	[XmlIgnore]
	public WebTafsilGroup WebTafsilGroup { get; set; } = new WebTafsilGroup();


	[JsonIgnore]
	[XmlIgnore]
	public WebModem WebModem { get; set; } = new WebModem();


	[JsonIgnore]
	[XmlIgnore]
	public WebCustomerAddress WebCustomerAddress { get; set; } = new WebCustomerAddress();


	[JsonIgnore]
	[XmlIgnore]
	public WebLastPrice WebLastPrice { get; set; } = new WebLastPrice();


	[JsonIgnore]
	[XmlIgnore]
	public WebAttendanceLogOnPerson WebAttendanceLogOnPerson { get; set; } = new WebAttendanceLogOnPerson();


	[JsonIgnore]
	[XmlIgnore]
	public WebTafsil WebTafsil { get; set; } = new WebTafsil();


	[JsonIgnore]
	[XmlIgnore]
	public WebMoein WebMoein { get; set; } = new WebMoein();


	[JsonIgnore]
	[XmlIgnore]
	public WebKol WebKol { get; set; } = new WebKol();


	[JsonIgnore]
	[XmlIgnore]
	public WebCashierLog WebUserLog { get; set; } = new WebCashierLog();


	[JsonIgnore]
	[XmlIgnore]
	public WebLkInfo WebLkInfo { get; set; } = new WebLkInfo();


	[JsonIgnore]
	[XmlIgnore]
	public WebChequeMOperation WebChequeMOperation { get; set; } = new WebChequeMOperation();


	[JsonIgnore]
	[XmlIgnore]
	public WebChequeShOperation WebChequeShOperation { get; set; } = new WebChequeShOperation();


	[JsonIgnore]
	[XmlIgnore]
	public WebSanad WebSanad { get; set; } = new WebSanad();


	[JsonIgnore]
	[XmlIgnore]
	public WebPrdAnbarGardani WebPrdAnbarGardani { get; set; } = new WebPrdAnbarGardani();


	[JsonIgnore]
	[XmlIgnore]
	public WebPrdSerial WebPrdSerial { get; set; } = new WebPrdSerial();


	[JsonIgnore]
	[XmlIgnore]
	public WebIntroductionMethod WebIntroductionMethod { get; set; } = new WebIntroductionMethod();


	[JsonIgnore]
	[XmlIgnore]
	public WebEntitySync WebEntitySync { get; set; } = new WebEntitySync();


	[JsonIgnore]
	[XmlIgnore]
	public WebAccount WebAccount { get; set; } = new WebAccount();


	[JsonIgnore]
	[XmlIgnore]
	public WebCustomerRole CustomerRole { get; set; } = new WebCustomerRole();


	[JsonIgnore]
	[XmlIgnore]
	public WebNote Note { get; set; } = new WebNote();


	[JsonIgnore]
	[XmlIgnore]
	public WebTaxLogin TaxLogin { get; set; } = new WebTaxLogin();


	[JsonIgnore]
	[XmlIgnore]
	public WebAttPersonLeave AttPersonLeave { get; set; } = new WebAttPersonLeave();


	[JsonIgnore]
	[XmlIgnore]
	public WebDiscountCoupon DiscoupntCoupon { get; set; } = new WebDiscountCoupon();


	[JsonIgnore]
	[XmlIgnore]
	public WebAttDepartment AttDepartment { get; set; } = new WebAttDepartment();


	[JsonIgnore]
	[XmlIgnore]
	public WebTax Tax { get; set; } = new WebTax();


	[JsonIgnore]
	[XmlIgnore]
	public WebTaxOrderLog TaxOrderLog { get; set; } = new WebTaxOrderLog();


	[JsonIgnore]
	[XmlIgnore]
	public WebAghsatMGroup AghsatMGroup { get; set; } = new WebAghsatMGroup();


	[JsonIgnore]
	[XmlIgnore]
	public WebProductPriceLevel ProductPriceLevel { get; set; } = new WebProductPriceLevel();


	[JsonIgnore]
	[XmlIgnore]
	public WebSanadStatusReport SanadStatusReport { get; set; } = new WebSanadStatusReport();


	[JsonIgnore]
	[XmlIgnore]
	public WebDeviceLogOnPerson DeviceLogOnPerson { get; set; } = new WebDeviceLogOnPerson();


	[JsonIgnore]
	[XmlIgnore]
	public WebSanadPrint SanadPrint { get; set; } = new WebSanadPrint();


	[JsonIgnore]
	[XmlIgnore]
	public WebBioWriteDevice BioMetricWriteDevice { get; set; } = new WebBioWriteDevice();


	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public Sync Sync { get; internal set; } = new Sync();


	public WebContextHolder()
	{
	}

	private WebContextHolder(string guid)
	{
		Guid = guid;
	}

	public static WebContextHolder GetWebContextHolder(string guid = null)
	{
		WebContextHolder value = null;
		try
		{
			if (string.IsNullOrEmpty(guid))
			{
				guid = System.Guid.NewGuid().ToString();
			}
			if (_contexts.TryGetValue(guid, out value) && value != null)
			{
				return value;
			}
			value = new WebContextHolder(guid);
			_contexts.TryAdd(guid, value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetWebContextHolder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContextHolder.cs", 41);
		}
		return value;
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				WebCus?.Dispose();
				WebCusTitle?.Dispose();
				LoginExtention?.Dispose();
				DateTimeManager?.Dispose();
				WebAghsatAshkhas?.Dispose();
				WebAghsatShakhsi?.Dispose();
				WebApplicationSetting?.Dispose();
				WebCallingLog?.Dispose();
				WebContactUs?.Dispose();
				WebCusGroup?.Dispose();
				WebDelivery?.Dispose();
				WebDepot?.Dispose();
				WebDesc?.Dispose();
				WebReception?.Dispose();
				WebPardakht?.Dispose();
				WebOrder?.Dispose();
				WebProduct?.Dispose();
				WebPrdGroup?.Dispose();
				WebErrorLogSum?.Dispose();
				WebFileInfo?.Dispose();
				WebHesabTree?.Dispose();
				WebGPS?.Dispose();
				WebPrdVahed?.Dispose();
				WebNews?.Dispose();
				WebUserAcc?.Dispose();
				WebHyperLink?.Dispose();
				WebTelegramMessage?.Dispose();
				Packet?.Dispose();
				WebExecuteQuery?.Dispose();
				WebTicketPart?.Dispose();
				WebTicketDet?.Dispose();
				WebTicket?.Dispose();
				WebVisit?.Dispose();
				WebViewCheckM?.Dispose();
				WebLastPrice?.Dispose();
				WebViewCheckSh?.Dispose();
				WebTafsilGroup?.Dispose();
				WebModem?.Dispose();
				WebCustomerAddress?.Dispose();
				WebAttendanceLogOnPerson.Dispose();
				WebKol?.Dispose();
				WebMoein?.Dispose();
				WebTafsil?.Dispose();
				WebAccount?.Dispose();
				CustomerRole?.Dispose();
				Note?.Dispose();
				TaxLogin?.Dispose();
				Sync?.Dispose();
				AttPersonLeave?.Dispose();
				DiscoupntCoupon?.Dispose();
				SanadPrint?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContextHolder.cs", 181);
		}
	}
}
