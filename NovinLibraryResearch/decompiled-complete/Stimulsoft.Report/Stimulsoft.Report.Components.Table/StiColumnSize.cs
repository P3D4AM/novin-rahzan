using System;
using System.Linq;

namespace Stimulsoft.Report.Components.Table;

public class StiColumnSize
{
	private double[] widths;

	private bool[] fixedColumns;

	public int Length => widths.Length;

	public void SetFixedColumn(int indexCol, double width)
	{
		fixedColumns[indexCol] = true;
		if (widths[indexCol] < width)
		{
			widths[indexCol] = width;
		}
	}

	public void Add(int indexCol, double width)
	{
		if (!fixedColumns[indexCol])
		{
			widths[indexCol] += width;
		}
	}

	public void AddLastNotFixed(double width)
	{
		for (int num = fixedColumns.Length - 1; num != 0; num--)
		{
			if (!fixedColumns[num])
			{
				widths[num] += width;
				break;
			}
		}
	}

	public void Subtract(int indexCol, double width)
	{
		if (!fixedColumns[indexCol])
		{
			widths[indexCol] -= width;
		}
	}

	public void SetWidth(int indexCol, double width)
	{
		if (!fixedColumns[indexCol])
		{
			widths[indexCol] = width;
		}
	}

	public bool GetFixed(int index)
	{
		return fixedColumns[index];
	}

	public int GetCountNotFixedColumn()
	{
		return fixedColumns.Count((bool column) => !column);
	}

	public double GetWidth(int indexCol)
	{
		return widths[indexCol];
	}

	public void Normalize()
	{
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		for (int i = 0; i < widths.Length; i++)
		{
			num += (decimal)widths[i];
			decimal num3 = Math.Round(num - num2, 2);
			widths[i] = (double)num3;
			num2 += num3;
		}
	}

	public StiColumnSize(int size)
	{
		widths = new double[size];
		fixedColumns = new bool[size];
		for (int i = 0; i < size; i++)
		{
			widths[i] = 0.0;
			fixedColumns[i] = false;
		}
	}
}
