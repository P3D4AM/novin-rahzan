using System.Collections.Generic;
using System.Threading;

namespace PacketParser.Entities.Interfaces;

public interface ISwipeable<T>
{
	List<T> GetItems();

	void SetItems(List<T> items);
}
public interface ISwipeable
{
	void SwapedItemToRemove(int position, CancellationToken token);
}
