using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Design.Ribbon.Styles;

namespace Stimulsoft.Report.Design.Ribbon.Localizations;

[ToolboxItem(false)]
internal sealed class StiLocButton : Control
{
	private bool isMouseOver;

	public bool IsChecked { get; set; }

	public event EventHandler ButtonClick;

	public StiLocButton(string text, string tag)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).Text = text;
		((Control)this).Tag = tag;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (isMouseOver)
		{
			SolidBrush val = new SolidBrush(StiCollectionNameStyleButton.MouseOverColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (IsChecked)
		{
			Pen val2 = new Pen(StiCollectionNameStyleButton.SelectedColor);
			try
			{
				e.Graphics.DrawRectangle(val2, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		clientRectangle.X += StiScale.XXI(3.0);
		clientRectangle.Y += StiScale.YYI(3.0);
		clientRectangle.Width -= StiScale.XXI(6.0);
		clientRectangle.Height -= StiScale.YYI(6.0);
		e.Graphics.FillRectangle(Brushes.White, clientRectangle);
		Pen val3 = new Pen(StiCollectionNameStyleButton.StrokeColor);
		try
		{
			e.Graphics.DrawRectangle(val3, clientRectangle);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		Font val4 = new Font("Arial", 8f);
		try
		{
			StringFormat val5 = new StringFormat();
			try
			{
				val5.Alignment = (StringAlignment)1;
				val5.LineAlignment = (StringAlignment)1;
				val5.Trimming = (StringTrimming)0;
				e.Graphics.DrawString(((Control)this).Text, val4, Brushes.Black, (RectangleF)clientRectangle, val5);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		this.ButtonClick?.Invoke(this, EventArgs.Empty);
	}
}
