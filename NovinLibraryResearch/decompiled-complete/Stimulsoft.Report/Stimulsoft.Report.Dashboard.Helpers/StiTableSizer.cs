using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiTableSizer
{
	public static void MakeAutoSize(DataGridView grid, bool skipLimit = false, bool isDashboardShowing = false)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (((Control)grid).Disposing || ((Control)grid).IsDisposed)
			{
				return;
			}
			DataTable dataTable = grid.DataSource as DataTable;
			if (dataTable == null || dataTable.Rows.Count < 5000 || skipLimit)
			{
				grid.AutoResizeColumns((DataGridViewAutoSizeColumnsMode)6);
			}
			else
			{
				grid.AutoResizeColumns((DataGridViewAutoSizeColumnsMode)10);
			}
			Bitmap val = new Bitmap(1, 1);
			try
			{
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				try
				{
					foreach (DataGridViewColumn item in (BaseCollection)grid.Columns)
					{
						DataGridViewColumn val3 = item;
						val3.SortMode = (DataGridViewColumnSortMode)0;
						if (!isDashboardShowing)
						{
							val3.Width = Math.Min(StiScale.XXI(300.0), val3.Width);
						}
						DataColumn dataColumn = dataTable.Columns[val3.Name];
						if (dataColumn != null)
						{
							((DataGridViewBand)val3).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
							if (TypeExt.IsDateType(dataTable.Columns[val3.Name].DataType) || dataTable.Columns[val3.Name].DataType == typeof(bool))
							{
								((DataGridViewBand)val3).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
							}
							if (dataColumn?.DataType != null && TypeExt.IsNumericType(dataColumn.DataType))
							{
								((DataGridViewBand)val3).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)64;
							}
						}
						if (!isDashboardShowing || GetDefaultColumnWidth((IStiTableColumn)/*isinst with value type is only supported in some contexts*/) == 0)
						{
							val3.MinimumWidth = (int)val2.MeasureString(val3.HeaderText, ((Control)grid).Font).Width + StiScale.XXI(44.0);
						}
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
		}
	}

	private static int GetDefaultColumnWidth(IStiTableColumn column)
	{
		IStiTableColumnSize stiTableColumnSize = column as IStiTableColumnSize;
		if (stiTableColumnSize?.Size == null)
		{
			return 0;
		}
		return stiTableColumnSize.Size.Width;
	}
}
