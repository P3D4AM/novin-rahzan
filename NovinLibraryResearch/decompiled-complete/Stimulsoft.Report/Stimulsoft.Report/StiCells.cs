using System.Collections;

namespace Stimulsoft.Report;

public class StiCells
{
	private Hashtable rows = new Hashtable();

	public decimal this[int x, int y]
	{
		get
		{
			Hashtable row = GetRow(y + DistY);
			if (row.ContainsKey(x + DistX))
			{
				return (decimal)row[x + DistX];
			}
			return 0m;
		}
	}

	public int DistX { get; set; }

	public int DistY { get; set; }

	public void Clear()
	{
		rows.Clear();
	}

	private Hashtable GetRow(int y)
	{
		Hashtable hashtable = rows[y] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			rows[y] = hashtable;
		}
		return hashtable;
	}

	public void SetCell(int x, int y, decimal value)
	{
		Hashtable row = GetRow(y);
		row[x] = value;
	}
}
