namespace Stimulsoft.Base;

public class StiGisOptions : StiFileDataOptions
{
	public string Separator;

	public StiGisOptions(byte[] content, string separator)
		: base(content)
	{
		Separator = separator;
	}
}
