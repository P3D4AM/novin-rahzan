using System;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.Cache;

public static class FormExtentions
{
	public static void Init(this DataGridView dgrid, Font dgirFont, bool isFullScreen = true, bool readOnly = true)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			DataGridViewCellStyle val = new DataGridViewCellStyle();
			DataGridViewCellStyle val2 = new DataGridViewCellStyle();
			DataGridViewCellStyle val3 = new DataGridViewCellStyle();
			dgrid.AllowUserToAddRows = false;
			dgrid.AllowUserToDeleteRows = false;
			dgrid.AllowUserToResizeRows = false;
			dgrid.ReadOnly = readOnly;
			val.Alignment = (DataGridViewContentAlignment)16;
			val.BackColor = Color.FromArgb(192, 255, 255);
			val.ForeColor = SystemColors.WindowText;
			val.SelectionBackColor = Color.DodgerBlue;
			val.SelectionForeColor = SystemColors.HighlightText;
			val.WrapMode = (DataGridViewTriState)1;
			dgrid.ColumnHeadersDefaultCellStyle = val;
			dgrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
			val2.Alignment = (DataGridViewContentAlignment)16;
			val2.BackColor = Color.FromArgb(192, 255, 255);
			val2.ForeColor = SystemColors.ControlText;
			val2.SelectionBackColor = SystemColors.Highlight;
			val2.SelectionForeColor = SystemColors.ControlText;
			val2.WrapMode = (DataGridViewTriState)2;
			dgrid.DefaultCellStyle = val2;
			if (isFullScreen)
			{
				((Control)dgrid).Anchor = (AnchorStyles)15;
			}
			dgrid.AutoGenerateColumns = false;
			dgrid.CellBorderStyle = (DataGridViewCellBorderStyle)4;
			dgrid.GridColor = Color.FromArgb(208, 215, 229);
			((Control)dgrid).RightToLeft = (RightToLeft)1;
			dgrid.SelectionMode = (DataGridViewSelectionMode)1;
			val3.Alignment = (DataGridViewContentAlignment)16;
			val3.BackColor = Color.FromArgb(192, 255, 255);
			val3.ForeColor = SystemColors.WindowText;
			val3.SelectionBackColor = Color.DodgerBlue;
			val3.SelectionForeColor = SystemColors.HighlightText;
			val3.WrapMode = (DataGridViewTriState)1;
			dgrid.RowHeadersDefaultCellStyle = val3;
			dgrid.RowHeadersVisible = false;
			dgrid.BackgroundColor = Color.Gainsboro;
			dgrid.RowsDefaultCellStyle.BackColor = Color.Gainsboro;
			dgrid.RowsDefaultCellStyle.SelectionBackColor = Color.SkyBlue;
			dgrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
			dgrid.AlternatingRowsDefaultCellStyle = null;
			((Control)dgrid).Enter += delegate
			{
				dgrid.BackgroundColor = Color.LightYellow;
				dgrid.RowsDefaultCellStyle.BackColor = Color.LightYellow;
				dgrid.RowsDefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
				dgrid.RowsDefaultCellStyle.SelectionForeColor = Color.White;
			};
			((Control)dgrid).Leave += delegate
			{
				dgrid.BackgroundColor = Color.Gainsboro;
				dgrid.RowsDefaultCellStyle.BackColor = Color.Gainsboro;
				dgrid.RowsDefaultCellStyle.SelectionBackColor = Color.SkyBlue;
				dgrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
			};
			if (dgirFont != null)
			{
				((Control)dgrid).Font = dgirFont;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 197);
		}
	}

	public static void Initialize(this DataGridView DGrid, Font dgirFont, bool readOnly = true)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			DataGridViewCellStyle val = new DataGridViewCellStyle();
			DGrid.AllowUserToAddRows = false;
			DGrid.AllowUserToDeleteRows = false;
			DGrid.AllowUserToResizeRows = false;
			val.BackColor = Color.FromArgb(235, 237, 255);
			DGrid.AlternatingRowsDefaultCellStyle = val;
			DGrid.AutoGenerateColumns = false;
			DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
			DGrid.MultiSelect = false;
			((Control)DGrid).RightToLeft = (RightToLeft)1;
			DGrid.RowHeadersVisible = false;
			DGrid.ReadOnly = readOnly;
			DGrid.BackgroundColor = Color.White;
			DGrid.RowsDefaultCellStyle.BackColor = Color.White;
			DGrid.RowsDefaultCellStyle.SelectionBackColor = Color.SkyBlue;
			DGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
			DGrid.SelectionMode = (DataGridViewSelectionMode)1;
			DGrid.CellBorderStyle = (DataGridViewCellBorderStyle)1;
			DGrid.RowTemplate.Height = 30;
			((Control)DGrid).Enter += delegate
			{
				DGrid.RowsDefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
				DGrid.RowsDefaultCellStyle.SelectionForeColor = Color.White;
			};
			((Control)DGrid).Leave += delegate
			{
				DGrid.RowsDefaultCellStyle.SelectionBackColor = Color.SkyBlue;
				DGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
			};
			if (dgirFont != null)
			{
				((Control)DGrid).Font = dgirFont;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 236);
		}
	}
}
