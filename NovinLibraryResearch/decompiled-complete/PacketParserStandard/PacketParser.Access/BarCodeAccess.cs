using System.ComponentModel;

namespace PacketParser.Access;

public class BarCodeAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دیدن قسمت بارکد")]
	public bool Barcode_See { get; set; } = true;

}
