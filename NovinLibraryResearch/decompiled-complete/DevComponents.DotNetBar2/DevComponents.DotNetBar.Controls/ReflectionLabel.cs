using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(LabelX), "Controls.ReflectionLabel.ico")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.ReflectionLabelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ReflectionLabel : ControlWithBackgroundStyle
{
	private _0CB4 __1A56;

	private Bitmap __27B8;

	private float _ⱪ = 0.52f;

	private Size _ᢧ = Size.Empty;

	private MarkupLinkClickEventHandler Ӟ;

	private bool __27BA = true;

	protected override Size DefaultSize => new Size(175, 70);

	[Description("Indicates whether reflection effect is enabled.")]
	[DefaultValue(true)]
	public bool ReflectionEnabled
	{
		get
		{
			return __27BA;
		}
		set
		{
			if (__27BA != value)
			{
				__27BA = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue("")]
	[Description("Gets or sets the text displayed on label.")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		_27BB();
		((Control)this).Dispose(disposing);
	}

	protected override void PaintContent(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		Rectangle contentRectangle = GetContentRectangle();
		Point location = contentRectangle.Location;
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		if (__1A56 != null)
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
			if (!((Control)this).Enabled)
			{
				markupDrawContext.IgnoreFormattingColors = true;
				markupDrawContext.CurrentForeColor = SystemColors.ControlDark;
			}
			__1A56.Render(markupDrawContext);
			location.Y = __1A56.Bounds.Bottom;
			location.X = 0;
		}
		else
		{
			int num = (int)((float)_ᢧ.Height * (1f + _ⱪ));
			int num2 = contentRectangle.Y;
			int x = contentRectangle.X;
			if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Center)
			{
				num2 += (contentRectangle.Height - num) / 2;
			}
			else if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Far)
			{
				num2 += contentRectangle.Height - num;
			}
			TextDrawing.DrawString(graphics, ((Control)this).Text, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, new Rectangle(x, num2, contentRectangle.Width, contentRectangle.Height), ᗎ(backgroundStyle));
			location.Y = num2 + _ᢧ.Height;
		}
		Rectangle rectangle = new Rectangle(0, location.Y, ((Control)this).Width, ((Control)this).Height - location.Y);
		graphics.SetClip(rectangle, (CombineMode)0);
		int num3 = (int)Math.Floor(((Control)this).Font.Size * (float)((Control)this).Font.FontFamily.GetCellDescent(((Control)this).Font.Style) / (float)((Control)this).Font.FontFamily.GetEmHeight(((Control)this).Font.Style));
		location.Y -= num3;
		if (__27B8 != null && __27BA)
		{
			graphics.DrawImage((Image)(object)__27B8, location);
		}
		graphics.ResetClip();
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
		if (AntiAlias)
		{
			graphics.SmoothingMode = smoothingMode;
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		_22FD();
		((Control)this).OnTextChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_22FD();
		((Control)this).OnFontChanged(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		_22FD();
		((Control)this).OnForeColorChanged(e);
	}

	protected override void OnVisualPropertyChanged()
	{
		_22FD();
		base.OnVisualPropertyChanged();
	}

	protected override void OnResize(EventArgs e)
	{
		_22FD();
		((Control)this).OnResize(e);
	}

	private void _22FD()
	{
		string _06EA = ((Control)this).Text;
		if (__1A56 != null)
		{
			__1A56.ભ((Control)(object)this);
			__1A56.ವ -= ᑶ;
			__1A56 = null;
		}
		if (ሒ.መ(ref _06EA))
		{
			__1A56 = ሒ.ሕ(_06EA);
		}
		if (__1A56 != null)
		{
			__1A56.ವ += ᑶ;
		}
		Ⱬ();
		((Control)this).Invalidate();
	}

	private void Ⱬ()
	{
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Invalid comparison between Unknown and I4
		_27BB();
		if (((Control)this).Text.Length == 0 || ((Control)this).Width <= 0 || ((Control)this).Height <= 0 || ((Control)this).Font == null)
		{
			return;
		}
		Bitmap val = null;
		if (__1A56 == null)
		{
			Graphics val2 = ((Control)this).CreateGraphics();
			try
			{
				if (AntiAlias)
				{
					val2.SmoothingMode = (SmoothingMode)4;
					val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				Size ᢧ = TextDrawing.MeasureString(val2, ((Control)this).Text, ((Control)this).Font);
				val = new Bitmap(((Control)this).Width, ᢧ.Height, (PixelFormat)2498570);
				_ᢧ = ᢧ;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			val.MakeTransparent();
			Color color = Color.Empty;
			Graphics val3 = Graphics.FromImage((Image)(object)val);
			try
			{
				bool disposeStyle = false;
				ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
				if (AntiAlias)
				{
					val3.SmoothingMode = (SmoothingMode)4;
					val3.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
					color = (backgroundStyle.BackColor.IsEmpty ? ((Control)this).BackColor : backgroundStyle.BackColor);
					if (color.IsEmpty || color == Color.Transparent)
					{
						color = Color.WhiteSmoke;
					}
					DisplayHelp.FillRectangle(val3, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), color);
				}
				TextDrawing.DrawString(val3, ((Control)this).Text, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), ᗎ(backgroundStyle));
				if (disposeStyle)
				{
					backgroundStyle.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if (!color.IsEmpty)
			{
				val.MakeTransparent(color);
			}
		}
		else
		{
			ResizeMarkup();
			if (__1A56.Bounds.Height > 0)
			{
				val = new Bitmap(((Control)this).Width, __1A56.Bounds.Height, (PixelFormat)2498570);
				Color color2 = Color.Empty;
				Graphics val4 = Graphics.FromImage((Image)(object)val);
				try
				{
					bool disposeStyle2 = false;
					ElementStyle backgroundStyle2 = GetBackgroundStyle(out disposeStyle2);
					if (AntiAlias)
					{
						val4.SmoothingMode = (SmoothingMode)4;
						val4.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
						color2 = (backgroundStyle2.BackColor.IsEmpty ? ((Control)this).BackColor : backgroundStyle2.BackColor);
						if (color2.IsEmpty || color2 == Color.Transparent)
						{
							color2 = Color.WhiteSmoke;
						}
						DisplayHelp.FillRectangle(val4, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), color2);
					}
					if (__1A56.Bounds.Top > 0)
					{
						val4.TranslateTransform(0f, (float)(-(__1A56.Bounds.Top - GetContentRectangle().Y)));
					}
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val4, ((Control)this).Font, backgroundStyle2.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle2.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
					if (!((Control)this).Enabled)
					{
						markupDrawContext.IgnoreFormattingColors = true;
						markupDrawContext.CurrentForeColor = SystemColors.ControlDark;
					}
					__1A56.Render(markupDrawContext);
					if (disposeStyle2)
					{
						backgroundStyle2.Dispose();
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				if (!color2.IsEmpty)
				{
					val.MakeTransparent(color2);
				}
			}
		}
		if (val != null)
		{
			__27B8 = ᶣ.ᶥ((Image)(object)val);
			((Image)val).Dispose();
		}
	}

	private eTextFormat ᗎ(ElementStyle _0657)
	{
		if (_0657.TextAlignment == eStyleTextAlignment.Center)
		{
			return eTextFormat.HorizontalCenter;
		}
		if (_0657.TextAlignment == eStyleTextAlignment.Far)
		{
			return eTextFormat.Right;
		}
		return eTextFormat.Default;
	}

	private void _27BB()
	{
		if (__27B8 != null)
		{
			((Image)__27B8).Dispose();
			__27B8 = null;
		}
	}

	private void ᑶ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected virtual void ResizeMarkup()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Invalid comparison between Unknown and I4
		if (__1A56 == null)
		{
			return;
		}
		Rectangle contentRectangle = GetContentRectangle();
		if (contentRectangle.Width <= 0 || contentRectangle.Height <= 0)
		{
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			if (AntiAlias)
			{
				val.SmoothingMode = (SmoothingMode)4;
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			Size availableSize = new Size(10000, contentRectangle.Height);
			__1A56.Measure(availableSize, markupDrawContext);
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			int num = (int)((float)__1A56.Bounds.Height * (1f + _ⱪ));
			if (num < contentRectangle.Height)
			{
				if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Center)
				{
					contentRectangle.Y += (contentRectangle.Height - num) / 2;
					contentRectangle.Height = __1A56.Bounds.Height;
				}
				else if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Far)
				{
					contentRectangle.Y = contentRectangle.Bottom - num;
					contentRectangle.Height = __1A56.Bounds.Height;
				}
			}
			if (__1A56.Bounds.Width < contentRectangle.Width)
			{
				if (backgroundStyle.TextAlignment == eStyleTextAlignment.Center)
				{
					contentRectangle.X += (contentRectangle.Width - __1A56.Bounds.Width) / 2;
					contentRectangle.Width = __1A56.Bounds.Width;
				}
				else if ((backgroundStyle.TextAlignment == eStyleTextAlignment.Far && (int)((Control)this).RightToLeft == 0) || (backgroundStyle.TextAlignment == eStyleTextAlignment.Near && (int)((Control)this).RightToLeft == 1))
				{
					contentRectangle.X = contentRectangle.Right - __1A56.Bounds.Width;
					contentRectangle.Width = __1A56.Bounds.Width;
				}
			}
			__1A56._0CC6(contentRectangle, markupDrawContext);
			if (disposeStyle)
			{
				backgroundStyle.Dispose();
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		Ⱬ();
		((Control)this).Invalidate();
		((Control)this).OnEnabledChanged(e);
	}
}
