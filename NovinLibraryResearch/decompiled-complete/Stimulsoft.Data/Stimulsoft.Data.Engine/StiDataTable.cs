using System.Collections.Generic;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Data.Engine;

public class StiDataTable
{
	public static StiDataTable NullTable = new StiDataTable();

	public List<IStiMeter> Meters { get; set; }

	public List<object[]> Rows { get; set; }

	public bool IsNull => this == NullTable;

	public bool IsEmpty
	{
		get
		{
			if (!IsNull && Meters != null && Meters.Count != 0 && Rows != null)
			{
				return Rows.Count == 0;
			}
			return true;
		}
	}

	public StiDataTable()
		: this(new List<IStiMeter>(), new List<object[]>())
	{
	}

	public StiDataTable(List<IStiMeter> meters, List<object[]> rows)
	{
		Meters = meters;
		Rows = rows;
	}
}
