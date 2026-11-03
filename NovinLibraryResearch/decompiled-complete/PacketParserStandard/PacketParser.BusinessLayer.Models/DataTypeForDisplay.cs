using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class DataTypeForDisplay
{
	public string Value { get; set; }

	public EnDataType DataType { get; set; }

	public string TypeForDisplay => DataType.GetDisplay();

	public DataTypeForDisplay(EnDataType _type)
	{
		DataType = _type;
	}
}
