using PacketParser.Entities;

namespace PacketParser.Records.Products;

public class ProductSerialSettingRecord
{
	public string Title { get; set; }

	public EnDataType DataType { get; set; }

	public string DefaultValue { get; set; }
}
