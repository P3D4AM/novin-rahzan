namespace Stimulsoft.Base;

public class StiDBaseOptions : StiFileDataOptions
{
	public int MaxDataRows { get; set; }

	public string Path { get; }

	public string TableName { get; }

	public int CodePage { get; }

	public StiDBaseOptions(byte[] content, int codePage = 0)
		: this(content, null, codePage)
	{
	}

	public StiDBaseOptions(string path, int codePage = 0)
		: this(path, null, codePage)
	{
	}

	public StiDBaseOptions(byte[] content, string tableName, int codePage = 0)
		: base(content)
	{
		TableName = tableName;
		CodePage = codePage;
	}

	public StiDBaseOptions(string path, string tableName, int codePage = 0)
		: base(null)
	{
		Path = path;
		TableName = tableName;
		CodePage = codePage;
	}
}
