namespace PacketParser.Entities.Interfaces.Accounting;

public interface IMetaData_Base
{
	string Key { get; set; }

	string Value { get; set; }

	EnDataType DataType { get; set; }
}
