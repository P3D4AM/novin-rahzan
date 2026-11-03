using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PacketParser.Settings.Accounting;

public class BarCodeSettings
{
	private List<TotalBarcodes> totalBarcodes;

	private List<OffLineBarCodes> offLineBarcodes;

	public List<TotalBarcodes> TotalBarcodes
	{
		get
		{
			return totalBarcodes?.ToList();
		}
		set
		{
			totalBarcodes = value;
		}
	}

	public List<OffLineBarCodes> OffLineBarcodes
	{
		get
		{
			return offLineBarcodes?.ToList();
		}
		set
		{
			offLineBarcodes = value;
		}
	}

	[Obsolete("حذف شده", false)]
	public bool BarcodeEnableNEW { get; set; }

	public bool BarcodeAlarmIfNotDefined { get; set; }

	[DefaultValue(false)]
	public bool BarcodeAlarmIfWithOutPrice { get; set; } = true;

}
