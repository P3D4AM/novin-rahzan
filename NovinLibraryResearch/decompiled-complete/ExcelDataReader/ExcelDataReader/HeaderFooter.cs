namespace ExcelDataReader;

public sealed class HeaderFooter
{
	public bool HasDifferentFirst { get; }

	public bool HasDifferentOddEven { get; }

	public string FirstHeader { get; internal set; }

	public string FirstFooter { get; internal set; }

	public string OddHeader { get; internal set; }

	public string OddFooter { get; internal set; }

	public string EvenHeader { get; internal set; }

	public string EvenFooter { get; internal set; }

	internal HeaderFooter(bool hasDifferentFirst, bool hasDifferentOddEven)
	{
		HasDifferentFirst = hasDifferentFirst;
		HasDifferentOddEven = hasDifferentOddEven;
	}
}
