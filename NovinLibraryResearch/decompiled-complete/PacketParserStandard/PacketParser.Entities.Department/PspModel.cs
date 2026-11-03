using Newtonsoft.Json;

namespace PacketParser.Entities.Department;

public class PspModel
{
	public EnumConnectionType ConnectionType { get; set; }

	[JsonIgnore]
	public string ConnectionTypeForDisplay => ConnectionType.GetDisplay();

	public int Count { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }
}
