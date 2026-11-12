namespace Stimulsoft.Base.Excel;

internal class XlsbWorksheet
{
	private XlsbDimension _dimension;

	private string _Name;

	private int _id;

	public XlsbDimension Dimension
	{
		get
		{
			return _dimension;
		}
		set
		{
			_dimension = value;
		}
	}

	public int ColumnsCount
	{
		get
		{
			if (_dimension != null)
			{
				return _dimension.LastCol - _dimension.FirstCol + 1;
			}
			return -1;
		}
	}

	public int RowsCount
	{
		get
		{
			if (_dimension != null)
			{
				return _dimension.LastRow - _dimension.FirstRow + 1;
			}
			return -1;
		}
	}

	public string Name => _Name;

	public int Id => _id;

	public XlsbWorksheet(string name, int id)
	{
		_Name = name;
		_id = id;
	}
}
