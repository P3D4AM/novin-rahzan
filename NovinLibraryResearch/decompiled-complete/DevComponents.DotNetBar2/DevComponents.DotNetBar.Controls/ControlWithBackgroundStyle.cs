using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class ControlWithBackgroundStyle : Control
{
	private ElementStyle _ᠩ;

	private bool _᮰ = true;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public virtual bool AntiAlias
	{
		get
		{
			return _᮰;
		}
		set
		{
			if (_᮰ != value)
			{
				_᮰ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates control background style.")]
	public ElementStyle BackgroundStyle => _ᠩ;

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
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_ᠩ.StyleChanged -= ઐ;
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	private void ઐ(object _٠, EventArgs ؿ)
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
		Rectangle result = ᝈ._1756(backgroundStyle, ((Control)this).ClientRectangle);
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
		return result;
	}

	protected virtual ElementStyle GetBackgroundStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		return ElementStyleDisplay.റ(_ᠩ, out disposeStyle);
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
