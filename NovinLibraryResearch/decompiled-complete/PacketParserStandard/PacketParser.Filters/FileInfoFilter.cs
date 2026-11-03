using PacketParser.Entities;

namespace PacketParser.Filters;

public class FileInfoFilter
{
	public bool? Status { get; set; }

	public FileManagement_Parts? Part { get; set; }
}
