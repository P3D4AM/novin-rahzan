namespace Stimulsoft.Report.Engine;

internal class StiIndex
{
	public int Index { get; set; }

	public int IndexInColumnContainer { get; set; }

	public StiIndex(int index)
		: this(index, -1)
	{
	}

	public StiIndex(int index, int indexInColumnContainer)
	{
		Index = index;
		IndexInColumnContainer = indexInColumnContainer;
	}
}
