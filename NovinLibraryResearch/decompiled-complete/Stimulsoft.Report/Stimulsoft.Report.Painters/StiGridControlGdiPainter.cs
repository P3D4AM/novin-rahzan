using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiGridControlGdiPainter : StiReportControlGdiPainter
{
	private void DrawColumn(StiGridControl grid, Graphics g, Rectangle rect, string text)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		g.DrawLine(SystemPens.ControlDark, rect.X, rect.Y + 2, rect.X, rect.Bottom - 4);
		g.DrawLine(SystemPens.ControlLightLight, rect.X + 1, rect.Y + 2, rect.X + 1, rect.Bottom - 4);
		SolidBrush val = new SolidBrush(grid.HeaderForeColor);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.Alignment = (StringAlignment)0;
				val2.LineAlignment = (StringAlignment)1;
				val2.FormatFlags = (StringFormatFlags)4096;
				val2.Trimming = (StringTrimming)1;
				g.DrawString(text, grid.HeaderFont, (Brush)(object)val, (RectangleF)rect, val2);
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

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiGridControl stiGridControl = component as StiGridControl;
		stiGridControl.InvokePainting(stiGridControl, e);
		if (!e.Cancel && (stiGridControl.Enabled || stiGridControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD val = stiGridControl.GetPaintRectangle();
			Rectangle rectangle = ((RectangleD)(ref val)).ToRectangle();
			if (rectangle.Width > 0 && rectangle.Height > 0)
			{
				val = e.ClipRectangle;
				if (!((RectangleD)(ref val)).IsEmpty)
				{
					val = e.ClipRectangle;
					if (!rectangle.IntersectsWith(((RectangleD)(ref val)).ToRectangle()))
					{
						goto IL_02fd;
					}
				}
				Rectangle rectangle2 = rectangle;
				SolidBrush val2 = new SolidBrush(stiGridControl.BackgroundColor);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				Rectangle rectangle3 = rectangle;
				ControlPaint.DrawBorder3D(graphics, rectangle3, (Border3DStyle)10);
				rectangle.X += 2;
				rectangle.Y += 2;
				rectangle.Width -= 4;
				rectangle.Height -= 4;
				graphics.SetClip(rectangle);
				if (stiGridControl.ColumnHeadersVisible && stiGridControl.Columns.Count > 0)
				{
					int num = (stiGridControl.RowHeadersVisible ? (stiGridControl.RowHeaderWidth + rectangle.X) : rectangle.X);
					int y = rectangle.Y;
					Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 20);
					SolidBrush val3 = new SolidBrush(stiGridControl.HeaderBackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, rectangle4);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					graphics.DrawLine(SystemPens.ControlLightLight, rectangle4.X, rectangle4.Y, rectangle4.Right, rectangle4.Y);
					graphics.DrawLine(SystemPens.ControlLightLight, rectangle4.X, rectangle4.Y, rectangle4.X, rectangle4.Bottom);
					graphics.DrawLine(SystemPens.ControlDarkDark, rectangle4.X, rectangle4.Bottom, rectangle4.Right, rectangle4.Bottom);
					graphics.DrawLine(SystemPens.ControlDarkDark, rectangle4.Right, rectangle4.Y, rectangle4.Right, rectangle4.Bottom);
					foreach (StiGridColumn column in stiGridControl.Columns)
					{
						int num2 = ((column.Width > 0) ? column.Width : stiGridControl.PreferredColumnWidth);
						Rectangle rect = new Rectangle(num, y, num2, 20);
						DrawColumn(stiGridControl, graphics, rect, (!string.IsNullOrEmpty(column.HeaderText)) ? column.HeaderText : column.DataTextField);
						num += num2;
					}
				}
				graphics.ResetClip();
				PaintEvents(stiGridControl, e.Graphics, RectangleD.CreateFromRectangle(rectangle));
				PaintConditions(stiGridControl, e.Graphics, RectangleD.CreateFromRectangle(rectangle));
			}
		}
		goto IL_02fd;
		IL_02fd:
		e.Cancel = false;
		stiGridControl.InvokePainted(stiGridControl, e);
		PaintComponents(stiGridControl, e);
	}
}
