namespace Microsoft.Data.Tools.Sql.BatchParser;

internal struct PositionStruct
{
	private int _line;

	private int _column;

	private int _offset;

	private string _filename;

	public int Line => _line;

	public int Column => _column;

	public int Offset => _offset;

	public string Filename => _filename;

	public PositionStruct(int line, int column, int offset, string filename)
	{
		_line = line;
		_column = column;
		_offset = offset;
		_filename = filename;
	}
}
