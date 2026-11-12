using System;
using System.Collections.Generic;

namespace ExcelDataReader.Core;

internal class Row
{
	public int RowIndex { get; set; }

	public double Height { get; set; }

	public List<Cell> Cells { get; set; }

	public bool IsEmpty => Cells.Count == 0;

	public int GetMaxColumnIndex()
	{
		int num = int.MinValue;
		foreach (Cell cell in Cells)
		{
			num = Math.Max(cell.ColumnIndex, num);
		}
		return num;
	}
}
