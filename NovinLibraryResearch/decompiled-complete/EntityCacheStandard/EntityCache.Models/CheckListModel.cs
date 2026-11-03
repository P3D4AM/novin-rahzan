using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Models;

public class CheckListModel : ICheckList
{
	public string Title { get; set; }

	public bool IsChecked { get; set; }

	public int Ordinal { get; set; }
}
