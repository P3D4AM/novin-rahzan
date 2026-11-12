using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

[ToolboxItem(false)]
public class StiTableFooterControl : UserControl
{
	private StiTableViewControl tableViewControl;

	private StiTableElement tableElement;

	private Hashtable hashValues = new Hashtable();

	private IContainer components;

	internal StiTableElementStyle Style { get; set; }

	internal bool BlockColumnWidthChanged { get; set; }

	public StiTableFooterControl(StiTableViewControl tableViewControl, StiTableElement tableElement, StiTableElementStyle style)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		InitializeComponent();
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		this.tableViewControl = tableViewControl;
		this.tableElement = tableElement;
		Style = style;
		((DataGridView)this.tableViewControl).Scroll += new ScrollEventHandler(TableViewControl_Scroll);
		((DataGridView)this.tableViewControl).ColumnWidthChanged += new DataGridViewColumnEventHandler(TableViewControl_ColumnWidthChanged);
	}

	public void UpdateData()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		StiDataTable dataTable = tableViewControl.DataTable;
		hashValues.Clear();
		double num = 28.0 * StiScale.Factor;
		int num2 = 0;
		foreach (DataGridViewColumn item in (BaseCollection)((DataGridView)tableViewControl).Columns)
		{
			DataGridViewColumn val = item;
			StiTableColumn stiTableColumn = (StiTableColumn)tableViewControl.GridColumnToTableColumn[val];
			if (stiTableColumn.ShowTotalSummary)
			{
				decimal num3 = StiTableElementHelper.CalculateTotal(stiTableColumn, num2, dataTable);
				string text = ((stiTableColumn.SummaryType == StiSummaryColumnType.Count) ? ((int)num3).ToString() : StiTextFormatHelper.FormatBasedOnColumnType(tableViewControl.Element, stiTableColumn, num3));
				hashValues.Add(val, text);
				StringFormat val2 = new StringFormat();
				try
				{
					if (stiTableColumn.SummaryAlignment == StiHorAlignment.Left)
					{
						val2.Alignment = (StringAlignment)0;
					}
					else if (stiTableColumn.SummaryAlignment == StiHorAlignment.Center)
					{
						val2.Alignment = (StringAlignment)1;
					}
					else if (stiTableColumn.SummaryAlignment == StiHorAlignment.Right)
					{
						val2.Alignment = (StringAlignment)2;
					}
					val2.LineAlignment = (StringAlignment)1;
					int num4 = (int)StiReport.GlobalMeasureGraphics.MeasureString(text, ((Control)this).Font).Width + 4;
					if (val.MinimumWidth < num4)
					{
						val.MinimumWidth = num4;
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			num2++;
		}
		((Control)this).Height = (int)num;
		((Control)this).Paint -= new PaintEventHandler(This_Paint);
		((Control)this).Paint += new PaintEventHandler(This_Paint);
		((Control)this).Refresh();
	}

	private void TableViewControl_Scroll(object sender, ScrollEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.ScrollOrientation == 0)
		{
			((Control)this).Refresh();
		}
	}

	private void TableViewControl_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
	{
		if (!BlockColumnWidthChanged)
		{
			((Control)this).Refresh();
		}
	}

	private Color GetForeColor()
	{
		if (((Control)this).ForeColor != Color.Transparent)
		{
			return ((Control)this).ForeColor;
		}
		return Style.FooterForeColor;
	}

	private void This_Paint(object sender, PaintEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		int num = 0;
		int num2 = -((DataGridView)tableViewControl).HorizontalScrollingOffset;
		SolidBrush val = new SolidBrush(Style.FooterBackColor);
		try
		{
			SolidBrush val2 = new SolidBrush(Style.LineColor);
			try
			{
				SolidBrush val3 = new SolidBrush(GetForeColor());
				try
				{
					foreach (DataGridViewColumn item in (BaseCollection)((DataGridView)tableViewControl).Columns)
					{
						DataGridViewColumn val4 = item;
						if (tableElement.FrozenColumns == 0 || num > tableElement.FrozenColumns)
						{
							RectangleF rectangleF = new RectangleF(num2, 0f, val4.Width, ((Control)this).Height);
							graphics.FillRectangle((Brush)(object)val, rectangleF);
							if (num > 0)
							{
								StiDrawing.DrawLine(graphics, Style.LineColor, rectangleF.Left, rectangleF.Top, rectangleF.Left, rectangleF.Bottom);
							}
							if (hashValues.ContainsKey(val4))
							{
								StiTableColumn stiTableColumn = (StiTableColumn)tableViewControl.GridColumnToTableColumn[val4];
								string text = (string)hashValues[val4];
								if (!string.IsNullOrEmpty(text))
								{
									StringFormat val5 = new StringFormat();
									try
									{
										if (stiTableColumn.SummaryAlignment == StiHorAlignment.Left)
										{
											val5.Alignment = (StringAlignment)0;
										}
										else if (stiTableColumn.SummaryAlignment == StiHorAlignment.Center)
										{
											val5.Alignment = (StringAlignment)1;
										}
										else if (stiTableColumn.SummaryAlignment == StiHorAlignment.Right)
										{
											val5.Alignment = (StringAlignment)2;
										}
										val5.LineAlignment = (StringAlignment)1;
										graphics.DrawString(text, ((Control)this).Font, (Brush)(object)val3, rectangleF, val5);
									}
									finally
									{
										((IDisposable)val5)?.Dispose();
									}
								}
							}
						}
						num++;
						num2 += val4.Width;
					}
					if (tableElement.FrozenColumns <= 0)
					{
						return;
					}
					num = 0;
					num2 = 0;
					foreach (DataGridViewColumn item2 in (BaseCollection)((DataGridView)tableViewControl).Columns)
					{
						DataGridViewColumn val6 = item2;
						if (num > tableElement.FrozenColumns)
						{
							break;
						}
						RectangleF rectangleF2 = new RectangleF(num2, 0f, val6.Width, ((Control)this).Height);
						graphics.FillRectangle((Brush)(object)val, rectangleF2);
						if (num > 0)
						{
							StiDrawing.DrawLine(graphics, Style.LineColor, rectangleF2.Left, rectangleF2.Top, rectangleF2.Left, rectangleF2.Bottom);
						}
						if (hashValues.ContainsKey(val6))
						{
							StiTableColumn textObj = (StiTableColumn)tableViewControl.GridColumnToTableColumn[val6];
							string text2 = (string)hashValues[val6];
							if (!string.IsNullOrEmpty(text2))
							{
								StringFormat val7 = new StringFormat();
								try
								{
									val7.Alignment = StiTextCellPainter.ToAlignment(textObj);
									val7.LineAlignment = (StringAlignment)1;
									graphics.DrawString(text2, ((Control)this).Font, (Brush)(object)val3, rectangleF2, val7);
								}
								finally
								{
									((IDisposable)val7)?.Dispose();
								}
							}
						}
						num++;
						num2 += val6.Width;
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
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
	}
}
