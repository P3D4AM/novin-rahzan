using System;
using System.Collections.Generic;

namespace PacketParser.Entities.SerializeData;

public class BankData
{
	public string HesabType { get; set; }

	public bool IsPos { get; set; }

	public string Shobe { get; set; }

	public string ShobeCode { get; set; }

	public string HesabNo { get; set; }

	public string Tel1 { get; set; }

	public string Tel2 { get; set; }

	public string Fax { get; set; }

	[Obsolete("Use NegativeAmount", false)]
	public bool BankManfi { get; set; }

	public ReturnedState? NegativeAmount { get; set; }

	public string DarandeName { get; set; }

	public string DarandeTel { get; set; }

	public string DarandeADD { get; set; }

	public string VName { get; set; }

	public string VFName { get; set; }

	public string VShenas { get; set; }

	public string VMeli { get; set; }

	public string VPosti { get; set; }

	public string VTel { get; set; }

	public string VAdd { get; set; }

	public string ParentFullCode { get; set; }

	public string PortName { get; set; } = "";


	public EnumConnectionType ConnectionType { get; set; }

	public long PortSpeed { get; set; }

	public double PosMultiplier { get; set; }

	public string ShabaNumber { get; set; }

	public string CardNumber { get; set; }

	public List<string> BarcodeFormatList { get; set; }
}
