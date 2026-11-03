using System;
using System.Collections.Generic;

namespace PacketParser.LkService;

public class LockInformation
{
	private string _clientSet;

	public DateTime? X12Mx { get; set; }

	public string SoftwareSerialHozor { get; set; } = "";


	public string SoftwareSerialAccounting { get; set; } = "";


	public string SoftwareSerialScale { get; set; } = "";


	public int X125 { get; set; }

	public long LkSerial { get; set; }

	public string AccountingActivationCode { get; set; } = "";


	public string TolidActivationCode { get; set; } = "";


	public string BarcodeActivationCode { get; set; } = "";


	public string ScaleActivationCode { get; set; } = "";


	public string CallerIdActivationCode { get; set; } = "";


	public string SpecialActivationCode { get; set; } = "";


	public string HoghoghActivationCode { get; set; } = "";


	public string ProductDepot_PishraftehActivationCode { get; set; } = "";


	public string ProductSerialActivationCode { get; set; } = "";


	public string TaxActivationCode { get; set; } = "";


	public string AghsatActivationCode { get; set; } = "";


	public string Cheque_KhazaneActivationCode { get; set; } = "";


	public string MultiCurrencyActivationCode { get; set; } = "";


	public string VoiceCallActivationCode { get; set; } = "";


	public string ServiceActivationCode { get; set; } = "";


	public string CustomerClubActivationCode { get; set; } = "";


	public string SoftwareType { get; set; } = "00";


	public string ScaleNo { get; set; } = "01";


	public string NumberOfdays { get; set; } = "";


	public string ClientNetNumber { get; set; } = "00";


	public bool IsDemo { get; set; }

	public string ClientSet
	{
		get
		{
			return _clientSet;
		}
		set
		{
			_clientSet = value;
		}
	}

	public int NumberOfDemoRun { get; set; }

	public DateTime ActiveDate { get; set; } = DateTime.Now.AddYears(-10);


	public string GuidClient { get; set; }

	public List<string> ListOfClient { get; set; } = new List<string>();

}
