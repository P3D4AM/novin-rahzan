namespace DevComponents.AdvTree;

public class ColumnMovedEventArgs : ColumnEventArgs
{
	public readonly int OldColumnDisplayIndex;

	public readonly int NewColumnDisplayIndex;

	public ColumnMovedEventArgs(ColumnHeader column, int oldColumnDisplayIndex, int newColumnDisplayIndex)
		: base(column)
	{
		OldColumnDisplayIndex = oldColumnDisplayIndex;
		NewColumnDisplayIndex = newColumnDisplayIndex;
	}
}
