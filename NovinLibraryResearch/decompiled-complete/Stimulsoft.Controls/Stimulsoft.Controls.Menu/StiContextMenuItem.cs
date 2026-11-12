using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls.Menu;

[ToolboxItem(false)]
public class StiContextMenuItem : ToolStripMenuItem
{
	public bool IsDarkMode
	{
		get
		{
			if (((ToolStripItem)this).Parent is StiContextMenu)
			{
				return (((ToolStripItem)this).Parent as StiContextMenu).IsDarkMode;
			}
			if (((ToolStripItem)this).OwnerItem is StiContextMenuItem)
			{
				return (((ToolStripItem)this).OwnerItem as StiContextMenuItem).IsDarkMode;
			}
			return false;
		}
	}

	protected override Padding DefaultPadding
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			int num = StiScaleUI.I1 * StiScaleUI.I1;
			return new Padding(StiScaleUI.I1, num, StiScaleUI.I1, num);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle contentRectangle = ((ToolStripItem)this).ContentRectangle;
		contentRectangle.Inflate(1, 1);
		contentRectangle.X++;
		contentRectangle.Width--;
		PaintBackground(graphics, contentRectangle);
		PaintText(graphics, contentRectangle);
		PaintCheck(graphics, contentRectangle);
		PaintNestedIndicator(graphics, contentRectangle);
	}

	private void PaintBackground(Graphics g, Rectangle rect)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		Color color = ((((ToolStripItem)this).Selected && ((ToolStripItem)this).Enabled) ? StiColor.Get("e2e2e2") : StiColor.Get("ffffff"));
		if (IsDarkMode)
		{
			color = ((((ToolStripItem)this).Selected && ((ToolStripItem)this).Enabled) ? StiColor.Get("414141") : StiColor.Get("2b2b2b"));
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.FillRectangle((Brush)(object)val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintText(Graphics g, Rectangle rect)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(((ToolStripItem)this).Text))
		{
			return;
		}
		GraphicsState val = g.Save();
		int num = StiScaleUI.XXI(20.0);
		Rectangle rectangle = new Rectangle(rect.X + num, rect.Y, rect.Width - num - StiScaleUI.I2, rect.Height - 1);
		try
		{
			g.SetClip(rectangle);
			Color textColor = GetTextColor();
			TextFormatFlags val2 = (TextFormatFlags)4;
			TextRenderer.DrawText((IDeviceContext)(object)g, ((ToolStripItem)this).Text, ((ToolStripItem)this).Font, rectangle, textColor, val2);
		}
		finally
		{
			g.Restore(val);
		}
	}

	private void PaintNestedIndicator(Graphics g, Rectangle rect)
	{
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		if (((ArrangedElementCollection)((ToolStripDropDownItem)this).DropDownItems).Count == 0)
		{
			return;
		}
		rect = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
		rect = new Rectangle(rect.X + (rect.Width - StiScaleUI.I8) / 2, rect.Y + (rect.Height - StiScaleUI.I8) / 2, StiScaleUI.I8, StiScaleUI.I8);
		Point point = new Point(rect.X, rect.Y);
		Point point2 = new Point(rect.X, rect.Bottom);
		Point point3 = new Point(rect.Right - rect.Width / 3, rect.Y + rect.Height / 2);
		Color color = (IsDarkMode ? StiColor.Get("f8f8f8") : StiColor.Get("333333"));
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			SolidBrush val = new SolidBrush(color);
			try
			{
				g.SmoothingMode = (SmoothingMode)4;
				g.FillPolygon((Brush)(object)val, new Point[3] { point, point2, point3 });
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private void PaintCheck(Graphics g, Rectangle rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		if ((int)((ToolStripMenuItem)this).CheckState == 1)
		{
			PaintCheckState(g, rect);
		}
		else if ((int)((ToolStripMenuItem)this).CheckState == 2)
		{
			PaintIndeterminateState(g, rect);
		}
	}

	private void PaintCheckState(Graphics g, Rectangle rect)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		rect = new Rectangle(rect.X + StiScaleUI.I3, rect.Y + (rect.Height - StiScaleUI.I10) / 2, StiScaleUI.I12, StiScaleUI.I10);
		Point point = new Point(rect.X, rect.Y + rect.Height / 2);
		Point point2 = new Point(rect.X + (int)((float)rect.Width / 2.4f), rect.Bottom);
		Point point3 = new Point(rect.Right, rect.Y);
		Color color = (IsDarkMode ? StiColor.Get("f8f8f8") : StiColor.Get("333333"));
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			Pen val = new Pen(color);
			try
			{
				g.SmoothingMode = (SmoothingMode)4;
				val.Width = 1.8f;
				val.Alignment = (PenAlignment)2;
				val.DashCap = (DashCap)3;
				g.DrawLines(val, new Point[3] { point, point2, point3 });
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private void PaintIndeterminateState(Graphics g, Rectangle rect)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		rect = new Rectangle(rect.X + StiScaleUI.I3, rect.Y + (rect.Height - StiScaleUI.I10) / 2, StiScaleUI.I10, StiScaleUI.I10);
		Rectangle rectangle = rect;
		rectangle.Inflate(-StiScaleUI.I2, -StiScaleUI.I2);
		Point point = new Point(rectangle.X, rectangle.Y + rectangle.Height / 2);
		Point point2 = new Point(rectangle.Right, rectangle.Y + rectangle.Height / 2);
		Color color = (IsDarkMode ? StiColor.Get("f8f8f8") : StiColor.Get("333333"));
		Pen val = new Pen(color);
		try
		{
			g.DrawLine(val, point, point2);
			g.DrawRectangle(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Color GetTextColor()
	{
		if (IsDarkMode)
		{
			if (((ToolStripItem)this).Enabled)
			{
				return StiColor.Get("f8f8f8");
			}
			return StiColor.Get("a8a8a8");
		}
		if (((ToolStripItem)this).Enabled)
		{
			return StiColor.Get("333333");
		}
		return StiColor.Get("777777");
	}

	public StiContextMenuItem()
	{
	}

	public StiContextMenuItem(string text)
	{
		((ToolStripItem)this).Text = text;
	}
}
