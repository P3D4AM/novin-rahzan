namespace PacketParser.Entities;

public class NumberContainer<T>
{
	public int RowNumber { get; set; }

	public T Item { get; set; }
}
