using System;

namespace PacketParser.PosToPc.Sadad.Classes.EventArguments;

internal class PCPOSEventArg : System.EventArgs
{
	public string Amount { get; set; }

	public string BillNo { get; set; }

	public string CardNo { get; set; }

	public string MerchantNo { get; set; }

	public string PosRes { get; set; }

	public string RefNo { get; set; }

	public string RequestId { get; set; }

	public string SwitchDate { get; set; }

	public string SwitchRes { get; set; }

	public string SwitchTime { get; set; }

	public string TerminalNo { get; set; }

	public string TraceNo { get; set; }

	public string TransCode { get; set; }
}
