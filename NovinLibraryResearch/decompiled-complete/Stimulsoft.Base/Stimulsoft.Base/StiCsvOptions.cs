namespace Stimulsoft.Base;

public class StiCsvOptions : StiFileDataOptions
{
	public int MaxDataRows { get; set; }

	public string Path { get; }

	public string TableName { get; }

	public int CodePage { get; }

	public string Separator { get; }

	public StiCsvOptions(byte[] content, int codePage = 0, string separator = ";")
		: this(content, null, codePage, separator)
	{
	}

	public StiCsvOptions(string path, int codePage = 0, string separator = ";")
		: this(path, null, codePage, separator)
	{
	}

	public StiCsvOptions(byte[] content, string tableName, int codePage = 0, string separator = ";")
		: base(content)
	{
		TableName = tableName;
		CodePage = codePage;
		Separator = separator;
	}

	public StiCsvOptions(string path, string tableName, int codePage = 0, string separator = ";")
		: base(null)
	{
		Path = path;
		TableName = tableName;
		CodePage = codePage;
		Separator = separator;
	}
}
