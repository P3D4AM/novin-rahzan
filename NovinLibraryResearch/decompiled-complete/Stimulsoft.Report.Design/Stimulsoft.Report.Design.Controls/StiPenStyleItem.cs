using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

public class StiPenStyleItem : StiCustomItem
{
	private StiPenStyle selectedStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPenStyle SelectedStyle
	{
		get
		{
			return selectedStyle;
		}
		set
		{
			if (selectedStyle != value)
			{
				selectedStyle = value;
				InvokeSelectedIndexChanged(this, EventArgs.Empty);
				Invalidate();
			}
		}
	}

	public event EventHandler SelectedIndexChanged;

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(sender, e);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		e.Graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle bounds = Bounds;
		if (!e.IsOnMenu)
		{
			bounds.Width -= (int)Math.Ceiling(10.0 * StiScale.X);
		}
		bounds.Width--;
		bounds.Height--;
		bounds.Inflate(-1, -1);
		Rectangle rectangle = new Rectangle(bounds.X + 3, bounds.Top + 3, bounds.Width - 7, bounds.Height - 7);
		base.Paint(e);
		if (e.IsOnMenu)
		{
			rectangle.Width = (int)Math.Ceiling(30.0 * StiScale.X);
		}
		Rectangle rectangle2 = new Rectangle(rectangle.Right + 4, bounds.Y, bounds.Width - rectangle.Width - 10, bounds.Height);
		if (Enabled)
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, rectangle);
			e.Graphics.DrawRectangle(Pens.Black, rectangle);
		}
		else
		{
			e.Graphics.FillRectangle(SystemBrushes.Control, rectangle);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
		}
		Pen val = new Pen(Color.Black, 2f);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(SelectedStyle);
			if (Enabled)
			{
				int num = rectangle.Y + rectangle.Height / 2 + 1;
				if (SelectedStyle == StiPenStyle.Double)
				{
					val.Width = 1f;
					e.Graphics.DrawLine(val, rectangle.X + 1, num - 1, rectangle.Right, num - 1);
					e.Graphics.DrawLine(val, rectangle.X + 1, num + 1, rectangle.Right, num + 1);
				}
				else if (SelectedStyle != StiPenStyle.None)
				{
					e.Graphics.DrawLine(val, rectangle.X + 1, num, rectangle.Right, num);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!e.IsOnMenu)
		{
			return;
		}
		StringFormat val2 = new StringFormat();
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				val2.Alignment = (StringAlignment)0;
				val2.LineAlignment = (StringAlignment)1;
				val2.Trimming = (StringTrimming)3;
				val2.FormatFlags = (StringFormatFlags)4096;
				e.Graphics.DrawString(Text, val3, Brushes.Black, (RectangleF)rectangle2, val2);
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
}
