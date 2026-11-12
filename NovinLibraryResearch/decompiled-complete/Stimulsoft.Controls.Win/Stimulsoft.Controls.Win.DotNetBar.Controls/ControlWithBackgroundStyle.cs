using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
public class ControlWithBackgroundStyle : Control
{
	private ElementStyle _BackgroundStyle;

	private bool _AntiAlias = true;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public virtual bool AntiAlias
	{
		get
		{
			return _AntiAlias;
		}
		set
		{
			if (_AntiAlias != value)
			{
				_AntiAlias = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Style")]
	[Description("Indicates control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	public ControlWithBackgroundStyle()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)4096, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		_BackgroundStyle = new ElementStyle();
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		_BackgroundStyle = new ElementStyle();
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		((Control)this).Invalidate();
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent)
		{
			((Control)this).OnPaintBackground(e);
		}
		PaintBackground(e);
		PaintContent(e);
		((Control)this).OnPaint(e);
	}

	protected virtual void PaintContent(PaintEventArgs e)
	{
	}

	protected virtual Rectangle GetContentRectangle()
	{
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		Rectangle innerRect = ElementStyleLayout.GetInnerRect(backgroundStyle, ((Control)this).ClientRectangle);
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
		return innerRect;
	}

	protected virtual ElementStyle GetBackgroundStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
	}

	protected virtual void PaintBackground(PaintEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		if (!((Control)this).BackColor.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, ((Control)this).BackColor);
		}
		if (backgroundStyle.Custom)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, clientRectangle);
			ElementStyleDisplay.Paint(e2);
			if (AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
		}
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
	}
}
