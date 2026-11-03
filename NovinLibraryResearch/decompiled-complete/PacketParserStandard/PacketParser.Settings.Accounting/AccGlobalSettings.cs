using System;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Issabel;
using PacketParser.Settings.Accounting.Lock;
using PacketParser.Settings.Accounting.Orders;
using PacketParser.Settings.Sefareshgir;

namespace PacketParser.Settings.Accounting;

public class AccGlobalSettings : INpDisposable, IDisposable
{
	private bool _isDisposed;

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed => _isDisposed;

	public PrintSettings printSettings { get; set; } = new PrintSettings();


	public CompanySettings CompanyInfo { get; set; } = new CompanySettings();


	public ProductSettings Products { get; set; } = new ProductSettings();


	public SanadSettings Sanads { get; set; } = new SanadSettings();


	public CustomerSettings Customers { get; set; } = new CustomerSettings();


	public FactorSettings Factors { get; set; } = new FactorSettings();


	public SellSettings Sells { get; set; } = new SellSettings();


	public BuySettings Buy { get; set; } = new BuySettings();


	public BackSellSettings BackSell { get; set; } = new BackSellSettings();


	public BackBuySettings BackBuy { get; set; } = new BackBuySettings();


	public AmaniVorodSettings AmaniVorod { get; set; } = new AmaniVorodSettings();


	public AmaniKhorojSettings AmaniKhoroj { get; set; } = new AmaniKhorojSettings();


	public ZayeatSettings Zayeat { get; set; } = new ZayeatSettings();


	public KardexSettings Kardex { get; set; } = new KardexSettings();


	public OtherSettings Others { get; set; } = new OtherSettings();


	public AccCodeSetting CodeSetting { get; set; } = new AccCodeSetting();


	public PayamakSettings Payamak { get; set; } = new PayamakSettings();


	public AccAutoDescriptionSettings AutoDescription { get; set; } = new AccAutoDescriptionSettings();


	public CrmSettings CrmSetting { get; set; } = new CrmSettings();


	public Desc ReadyDes { get; set; } = new Desc();


	public DepotSettings DepotSetting { get; set; } = new DepotSettings();


	public MaliYearSettings MaliYearSettings { get; set; } = new MaliYearSettings();


	public IsabelleClientSettings Isabelle { get; set; } = new IsabelleClientSettings();


	public TaxServiceSettings TaxService { get; set; } = new TaxServiceSettings();


	public PcPossSettings PcPoss { get; set; } = new PcPossSettings();


	public InstallmentSetting Installments { get; set; } = new InstallmentSetting();


	public ReservationSetting Reservation { get; set; } = new ReservationSetting();


	public BranchSetting Branches { get; set; } = new BranchSetting();


	public DateAndTimePriodSettings DateAndTimePriod { get; set; } = new DateAndTimePriodSettings();


	public VamSettings Vam { get; set; } = new VamSettings();


	public MultiLanguageSettings MultiLanguage { get; set; } = new MultiLanguageSettings();


	public TolidSettings Tolid { get; set; } = new TolidSettings();


	public int Version { get; set; }

	public DateTime Date_Modified { get; set; } = DateTime.Now;


	public DateTime? Date_Synced { get; set; }

	public EnPriceType SellType { get; set; }

	public SefareshgirSettings SefareshgirSettings { get; set; } = new SefareshgirSettings();


	public string DescriptionMaker { get; set; } = "";


	[JsonIgnore]
	[XmlIgnore]
	private DateTime Create { get; } = DateTime.Now;


	[JsonIgnore]
	[XmlIgnore]
	public bool IsExpired => DateTime.Now.Subtract(Create).TotalSeconds > 60000.0;

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs", 73);
				throw;
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs", 84);
		}
	}

	public static AccGlobalSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs"))
		{
			try
			{
				return data.FromJson<AccGlobalSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs", 91);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AccGlobalSettings.cs", 95);
				throw;
			}
		}
	}

	public static string GetKeyName(string contextGuid)
	{
		return contextGuid + "AccGlobalSettings";
	}
}
