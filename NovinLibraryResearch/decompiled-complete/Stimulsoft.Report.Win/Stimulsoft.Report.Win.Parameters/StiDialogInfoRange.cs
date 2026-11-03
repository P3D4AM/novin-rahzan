namespace Stimulsoft.Report.Win.Parameters;

internal class StiDialogInfoRange
{
	public object From { get; set; }

	public object To { get; set; }

	public string Value { get; set; }

	public override string ToString()
	{
		return $"{Value ?? string.Empty} [{From} - {To}]";
	}

	public StiDialogInfoRange(object from, object to, string value)
	{
		From = from;
		To = to;
		Value = value;
	}
}
