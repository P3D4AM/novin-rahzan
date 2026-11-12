using System.Collections.Generic;

namespace Stimulsoft.Base.Excel;

internal class XlsbStyles
{
	private List<XlsbXf> _cellXfs;

	private List<XlsbNumFmt> _NumFmts;

	public List<XlsbXf> CellXfs
	{
		get
		{
			return _cellXfs;
		}
		set
		{
			_cellXfs = value;
		}
	}

	public List<XlsbNumFmt> NumFmts
	{
		get
		{
			return _NumFmts;
		}
		set
		{
			_NumFmts = value;
		}
	}

	public XlsbStyles()
	{
		_cellXfs = new List<XlsbXf>();
		_NumFmts = new List<XlsbNumFmt>();
	}
}
