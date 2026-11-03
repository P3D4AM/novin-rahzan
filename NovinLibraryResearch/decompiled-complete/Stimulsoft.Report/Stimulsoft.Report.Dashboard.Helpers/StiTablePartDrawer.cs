using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiTablePartDrawer
{
	private StiTableElementStyle style;

	private Color? headerForeColor;

	private Font headerFont;

	private int dragColumnIndex = -1;

	private bool IsMouseOverCell => CellBounds.Contains(((Control)Grid).PointToClient(Cursor.Position));

	private DataGridViewCellPaintingEventArgs CellArgs { get; }

	private Graphics Graphics => CellArgs.Graphics;

	private Rectangle CellBounds => CellArgs.CellBounds;

	public bool WordWrapHeader { get; set; }

	private DataGridView Grid { get; }

	private List<StiDataSortRule> Sorts { get; }

	private List<StiDataFilterRule> Filters { get; }

	private List<StiDataActionRule> Actions { get; }

	private int ColumnsCount { get; }

	private string ColumnKey { get; }

	private IStiTableElement Table { get; }

	public void DrawCell()
	{
		if (CellArgs.RowIndex != -1)
		{
			DrawDataCell();
		}
		else
		{
			DrawHeaderCell();
		}
	}

	private void DrawHeaderCell()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		Graphics graphics = CellArgs.Graphics;
		((HandledEventArgs)(object)CellArgs).Handled = true;
		CellArgs.Paint(CellBounds, (DataGridViewPaintParts)101);
		Point pt = ((Control)Grid).PointToClient(Cursor.Position);
		bool flag = CellBounds.Contains(pt);
		Color color = ((style == null) ? ColorTranslator.FromHtml(flag ? "#ffffff" : "#f0f0f0") : ((flag || (dragColumnIndex == CellArgs.ColumnIndex && CellArgs.RowIndex == -1)) ? style.HotHeaderBackColor : style.HeaderBackColor));
		SolidBrush val = new SolidBrush(color);
		try
		{
			graphics.FillRectangle((Brush)(object)val, CellBounds);
			PaintFrozenArea(graphics, CellBounds, CellArgs.ColumnIndex);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		DrawBorder();
		DrawHeaderText();
		IStiTableDashboardInteraction stiTableDashboardInteraction = ((Table is IStiElementInteraction stiElementInteraction) ? stiElementInteraction.DashboardInteraction : null) as IStiTableDashboardInteraction;
		if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserSorting)
		{
			DrawSortArrow();
		}
		if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserFiltering)
		{
			DrawFilterArrow();
		}
	}

	private void DrawDataCell()
	{
		CellArgs.Paint(CellBounds, (DataGridViewPaintParts)127);
		DrawBorder();
		((HandledEventArgs)(object)CellArgs).Handled = true;
	}

	private void PaintFrozenArea(Graphics g, Rectangle rect, int columnIndex)
	{
		IEnumerable<DataGridViewColumn> source = ((IEnumerable)Grid.Columns).Cast<DataGridViewColumn>();
		DataGridViewColumn val = source.LastOrDefault((Func<DataGridViewColumn, bool>)((DataGridViewColumn c) => ((DataGridViewBand)c).Frozen));
		if (val == null)
		{
			return;
		}
		int num = source.ToList().IndexOf(val) + 1;
		if (num != 0)
		{
			if (columnIndex == num - 1)
			{
				StiDrawing.FillRectangle(g, StiColor.Get("10FFFFFF"), new Rectangle(rect.Right - StiScale.I6, rect.Top, StiScale.I6, rect.Height));
			}
			else if (columnIndex == num)
			{
				StiDrawing.FillRectangle(g, StiColor.Get("10000000"), new Rectangle(rect.Left, rect.Top, StiScale.I6, rect.Height));
			}
		}
	}

	private void DrawBorder()
	{
		if (CellArgs.ColumnIndex != ((BaseCollection)Grid.Columns).Count - 1)
		{
			Color color = ((style == null) ? Table.BorderColor : style.LineColor);
			StiDrawing.DrawLine(CellArgs.Graphics, color, (double)(CellBounds.Right - 1), (double)CellBounds.Top, (double)(CellBounds.Right - 1), (double)CellBounds.Bottom);
		}
	}

	public void DrawHeaderText()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.Alignment = (StringAlignment)1;
			val.Trimming = (StringTrimming)3;
			val.FormatFlags = (StringFormatFlags)((!WordWrapHeader) ? 4096 : 0);
			string text = CellArgs.FormattedValue?.ToString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				StiDrawing.DrawString(Graphics, text, GetHeaderFont(), GetHeaderForeColorColor(), CellBounds, val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Font GetHeaderFont()
	{
		return headerFont ?? ((Control)Grid).Font;
	}

	private Color GetHeaderForeColorColor()
	{
		if (headerForeColor.HasValue && headerForeColor != Color.Transparent)
		{
			return headerForeColor.GetValueOrDefault();
		}
		if (style != null)
		{
			return style.HeaderForeColor;
		}
		return Color.Black;
	}

	public void DrawFilterArrow()
	{
		if (ColumnsCount > CellArgs.ColumnIndex)
		{
			bool flag = Filters != null && Filters.Any((StiDataFilterRule r) => r.Key == ColumnKey);
			bool flag2 = Actions != null && Actions.Any((StiDataActionRule a) => a.Key == ColumnKey);
			if (flag || flag2)
			{
				Image val = ((style != null) ? (IsMouseOverCell ? StiReportImages.Actions.DropDownFilterMouseOverWhite() : StiReportImages.Actions.DropDownFilterWhite()) : (IsMouseOverCell ? StiReportImages.Actions.DropDownFilterMouseOver() : StiReportImages.Actions.DropDownFilter()));
				Graphics.DrawImage(val, GetRightImageRect(val));
			}
			else if (IsMouseOverCell && dragColumnIndex == -1)
			{
				Image val2 = ((style == null) ? StiReportImages.Actions.DropDownArrowMouseOver() : StiReportImages.Actions.DropDownArrowMouseOverWhite());
				Graphics.DrawImage(val2, GetRightImageRect(val2));
			}
		}
	}

	private Rectangle GetRightImageRect(Image image)
	{
		int width = image.Width;
		int height = image.Height;
		return new Rectangle(CellBounds.Right - StiScale.I5 - width, CellBounds.Y + (CellBounds.Height - height) / 2, width, height);
	}

	public void DrawSortArrow()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (ColumnsCount > CellArgs.ColumnIndex)
		{
			StiDataSortDirection sortDirection = StiDataSortRuleHelper.GetSortDirection(Sorts, ColumnKey);
			if ((int)sortDirection != 3 || IsMouseOverCell)
			{
				Image sortImage = GetSortImage(sortDirection);
				Rectangle sortRectangle = GetSortRectangle(sortImage.Height, sortImage.Height);
				Graphics.DrawImage(sortImage, sortRectangle);
				sortRectangle.X += sortRectangle.Width - StiScale.I4;
				sortRectangle.Width = 100000;
				DrawSortIndex(sortRectangle);
			}
		}
	}

	public Rectangle GetSortRectangle(int imageWidth, int imageHeight)
	{
		return new Rectangle(CellBounds.X + StiScale.I4, CellBounds.Y + (CellBounds.Height - imageHeight) / 2, imageWidth, imageHeight);
	}

	public static Rectangle GetSortRectangle(Rectangle cellBounds)
	{
		Image val = StiReportImages.Actions.SortArrowAscMouseOver();
		return new Rectangle(cellBounds.X + StiScale.I4, cellBounds.Y + (cellBounds.Height - val.Height) / 2, val.Width, val.Height);
	}

	private Image GetSortImage(StiDataSortDirection direction)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (style == null)
		{
			if ((int)direction == 3 && IsMouseOverCell)
			{
				return StiReportImages.Actions.SortArrowAscMouseOver();
			}
			if ((int)direction == 1)
			{
				if (!IsMouseOverCell)
				{
					return StiReportImages.Actions.SortArrowAsc();
				}
				return StiReportImages.Actions.SortArrowAscMouseOver();
			}
			if (!IsMouseOverCell)
			{
				return StiReportImages.Actions.SortArrowDesc();
			}
			return StiReportImages.Actions.SortArrowDescMouseOver();
		}
		if ((int)direction == 3 && IsMouseOverCell)
		{
			return StiReportImages.Actions.SortArrowAscMouseOverDisable();
		}
		if ((int)direction == 1)
		{
			if (!IsMouseOverCell)
			{
				return StiReportImages.Actions.SortArrowAscWhite();
			}
			return StiReportImages.Actions.SortArrowAscMouseOverWhite();
		}
		if (!IsMouseOverCell)
		{
			return StiReportImages.Actions.SortArrowDescWhite();
		}
		return StiReportImages.Actions.SortArrowDescMouseOverWhite();
	}

	public void DrawSortIndex(Rectangle textRect)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		if (Sorts == null || Sorts.Count <= 1 || ColumnsCount <= CellArgs.ColumnIndex)
		{
			return;
		}
		StiDataSortRule val = ((IEnumerable<StiDataSortRule>)Sorts).FirstOrDefault((Func<StiDataSortRule, bool>)((StiDataSortRule r) => string.Equals(r.Key, ColumnKey, StringComparison.InvariantCultureIgnoreCase)));
		if (val == null)
		{
			return;
		}
		int num = Sorts.IndexOf(val) + 1;
		if (num == -1 || num > 9)
		{
			return;
		}
		Font val2 = new Font(((Control)Grid).Font.Name, 7f);
		try
		{
			StringFormat val3 = new StringFormat();
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				val3.Alignment = (StringAlignment)0;
				Color color = ((style != null) ? style.HeaderForeColor : (IsMouseOverCell ? ColorTranslator.FromHtml("#7a7a7a") : ColorTranslator.FromHtml("#aaaaaa")));
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					Graphics.DrawString(num.ToString(), val2, (Brush)(object)val4, (RectangleF)textRect, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public StiTablePartDrawer(DataGridViewCellPaintingEventArgs cellArgs, DataGridView grid, ObjectCollection items, List<StiDataSortRule> sorts, List<StiDataFilterRule> filters, List<StiDataActionRule> actions, IStiTableElement table = null)
	{
		CellArgs = cellArgs;
		Grid = grid;
		Sorts = sorts;
		Filters = filters;
		Actions = actions;
		Table = table;
		ColumnsCount = items.Count;
		int num = Math.Min(ColumnsCount - 1, cellArgs.ColumnIndex);
		ColumnKey = ((num == -1) ? null : (items[num] as StiDataColumn)?.Key);
	}

	public StiTablePartDrawer(DataGridViewCellPaintingEventArgs cellArgs, DataGridView grid, IStiTableElement element, StiTableElementStyle style = null, Color? headerForeColor = null, Font headerFont = null, int dragColumnIndex = -1, bool wordWrapHeader = false)
	{
		CellArgs = cellArgs;
		Grid = grid;
		Sorts = ((IStiUserSorts)element).UserSorts;
		Filters = ((IStiUserFilters)element).UserFilters;
		this.style = style;
		this.headerForeColor = headerForeColor;
		this.headerFont = headerFont;
		Table = element;
		WordWrapHeader = wordWrapHeader;
		List<IStiMeter> meters = element.GetMeters();
		ColumnsCount = meters.Count;
		int index = Math.Min(ColumnsCount - 1, cellArgs.ColumnIndex);
		IStiMeter obj = meters[index];
		ColumnKey = ((obj != null) ? obj.Key : null);
		this.dragColumnIndex = dragColumnIndex;
	}

	public StiTablePartDrawer(DataGridViewCellPaintingEventArgs cellArgs, DataGridView grid)
	{
		CellArgs = cellArgs;
		Grid = grid;
	}
}
