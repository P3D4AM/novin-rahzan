using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebMetaData_Base : IMetaData_Base
{
	[PersianNameAttribute.PersianName("کلید")]
	public string Key { get; set; }

	[PersianNameAttribute.PersianName("مقدار")]
	public string Value { get; set; }

	[PersianNameAttribute.PersianName("نوع داده")]
	public EnDataType DataType { get; set; }
}
