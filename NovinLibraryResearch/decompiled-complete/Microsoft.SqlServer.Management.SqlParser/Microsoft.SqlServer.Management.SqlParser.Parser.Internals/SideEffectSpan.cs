namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal class SideEffectSpan
{
	private readonly int beginIndex;

	private readonly int endIndex;

	public int BeginIndex => beginIndex;

	public int EndIndex => endIndex;

	public SideEffectSpan(int index)
	{
		beginIndex = index;
		endIndex = index + 1;
	}

	public SideEffectSpan(int beginIndex, int endIndex)
	{
		this.beginIndex = beginIndex;
		this.endIndex = endIndex;
	}

	public SideEffectSpan ShiftBy(int delta)
	{
		return new SideEffectSpan(beginIndex + delta, endIndex + delta);
	}
}
