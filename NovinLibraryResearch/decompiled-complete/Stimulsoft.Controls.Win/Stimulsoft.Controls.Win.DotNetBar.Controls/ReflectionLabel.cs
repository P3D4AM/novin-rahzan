using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(LabelX), "Controls.ReflectionLabel.ico")]
public class ReflectionLabel : ControlWithBackgroundStyle
{
	private BodyElement _TextMarkup;

	private Bitmap _ReflectionBitmap;

	private float _ReflectionFactor = 0.52f;

	private Size _TextSize = Size.Empty;

	private bool _ReflectionEnabled = true;

	protected override Size DefaultSize => new Size(175, 70);

	[DefaultValue(true)]
	[Description("Indicates whether reflection effect is enabled.")]
	public bool ReflectionEnabled
	{
		get
		{
			return _ReflectionEnabled;
		}
		set
		{
			if (_ReflectionEnabled != value)
			{
				_ReflectionEnabled = value;
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

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Gets or sets the text displayed on label.")]
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

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	protected override void Dispose(bool disposing)
	{
		DisposeReflectionImage();
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
		if (_TextMarkup != null)
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
			if (!((Control)this).Enabled)
			{
				markupDrawContext.IgnoreFormattingColors = true;
				markupDrawContext.CurrentForeColor = SystemColors.ControlDark;
			}
			_TextMarkup.Render(markupDrawContext);
			location.Y = _TextMarkup.Bounds.Bottom;
			location.X = 0;
		}
		else
		{
			int num = (int)((float)_TextSize.Height * (1f + _ReflectionFactor));
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
			TextDrawing.DrawString(graphics, ((Control)this).Text, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, new Rectangle(x, num2, contentRectangle.Width, contentRectangle.Height), GetTextFormat(backgroundStyle));
			location.Y = num2 + _TextSize.Height;
		}
		Rectangle rectangle = new Rectangle(0, location.Y, ((Control)this).Width, ((Control)this).Height - location.Y);
		graphics.SetClip(rectangle, (CombineMode)0);
		int num3 = (int)Math.Floor(((Control)this).Font.Size * (float)((Control)this).Font.FontFamily.GetCellDescent(((Control)this).Font.Style) / (float)((Control)this).Font.FontFamily.GetEmHeight(((Control)this).Font.Style));
		location.Y -= num3;
		if (_ReflectionBitmap != null && _ReflectionEnabled)
		{
			graphics.DrawImage((Image)(object)_ReflectionBitmap, location);
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
		UpdateDisplay();
		((Control)this).OnTextChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		UpdateDisplay();
		((Control)this).OnFontChanged(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		UpdateDisplay();
		((Control)this).OnForeColorChanged(e);
	}

	protected override void OnVisualPropertyChanged()
	{
		UpdateDisplay();
		base.OnVisualPropertyChanged();
	}

	protected override void OnResize(EventArgs e)
	{
		UpdateDisplay();
		((Control)this).OnResize(e);
	}

	private void UpdateDisplay()
	{
		string text = ((Control)this).Text;
		if (_TextMarkup != null)
		{
			_TextMarkup.MouseLeave((Control)(object)this);
			_TextMarkup.HyperLinkClick -= TextMarkupLinkClicked;
			_TextMarkup = null;
		}
		if (MarkupParser.IsMarkup(ref text))
		{
			_TextMarkup = MarkupParser.Parse(text);
		}
		if (_TextMarkup != null)
		{
			_TextMarkup.HyperLinkClick += TextMarkupLinkClicked;
		}
		CreateReflection();
		((Control)this).Invalidate();
	}

	private void CreateReflection()
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
		DisposeReflectionImage();
		if (((Control)this).Text.Length == 0 || ((Control)this).Width <= 0 || ((Control)this).Height <= 0 || ((Control)this).Font == null)
		{
			return;
		}
		Bitmap val = null;
		if (_TextMarkup == null)
		{
			Graphics val2 = ((Control)this).CreateGraphics();
			try
			{
				if (AntiAlias)
				{
					val2.SmoothingMode = (SmoothingMode)4;
					val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				Size textSize = TextDrawing.MeasureString(val2, ((Control)this).Text, ((Control)this).Font);
				val = new Bitmap(((Control)this).Width, textSize.Height, (PixelFormat)2498570);
				_TextSize = textSize;
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
				TextDrawing.DrawString(val3, ((Control)this).Text, ((Control)this).Font, backgroundStyle.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle.TextColor, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), GetTextFormat(backgroundStyle));
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
			if (_TextMarkup.Bounds.Height > 0)
			{
				val = new Bitmap(((Control)this).Width, _TextMarkup.Bounds.Height, (PixelFormat)2498570);
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
					if (_TextMarkup.Bounds.Top > 0)
					{
						val4.TranslateTransform(0f, (float)(-(_TextMarkup.Bounds.Top - GetContentRectangle().Y)));
					}
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val4, ((Control)this).Font, backgroundStyle2.TextColor.IsEmpty ? ((Control)this).ForeColor : backgroundStyle2.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
					if (!((Control)this).Enabled)
					{
						markupDrawContext.IgnoreFormattingColors = true;
						markupDrawContext.CurrentForeColor = SystemColors.ControlDark;
					}
					_TextMarkup.Render(markupDrawContext);
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
			_ReflectionBitmap = ImageHelper.CreateReflectionImage((Image)(object)val);
			((Image)val).Dispose();
		}
	}

	private eTextFormat GetTextFormat(ElementStyle style)
	{
		if (style.TextAlignment == eStyleTextAlignment.Center)
		{
			return eTextFormat.HorizontalCenter;
		}
		if (style.TextAlignment == eStyleTextAlignment.Far)
		{
			return eTextFormat.Right;
		}
		return eTextFormat.Default;
	}

	private void DisposeReflectionImage()
	{
		if (_ReflectionBitmap != null)
		{
			((Image)_ReflectionBitmap).Dispose();
			_ReflectionBitmap = null;
		}
	}

	private void TextMarkupLinkClicked(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}

	protected virtual void ResizeMarkup()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		if (_TextMarkup == null)
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
			MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			Size availableSize = new Size(10000, contentRectangle.Height);
			_TextMarkup.Measure(availableSize, d);
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			int num = (int)((float)_TextMarkup.Bounds.Height * (1f + _ReflectionFactor));
			if (num < contentRectangle.Height)
			{
				if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Center)
				{
					contentRectangle.Y += (contentRectangle.Height - num) / 2;
					contentRectangle.Height = _TextMarkup.Bounds.Height;
				}
				else if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Far)
				{
					contentRectangle.Y = contentRectangle.Bottom - num;
					contentRectangle.Height = _TextMarkup.Bounds.Height;
				}
			}
			if (_TextMarkup.Bounds.Width < contentRectangle.Width)
			{
				if (backgroundStyle.TextAlignment == eStyleTextAlignment.Center)
				{
					contentRectangle.X += (contentRectangle.Width - _TextMarkup.Bounds.Width) / 2;
					contentRectangle.Width = _TextMarkup.Bounds.Width;
				}
				else if ((backgroundStyle.TextAlignment == eStyleTextAlignment.Far && (int)((Control)this).RightToLeft == 0) || (backgroundStyle.TextAlignment == eStyleTextAlignment.Near && (int)((Control)this).RightToLeft == 1))
				{
					contentRectangle.X = contentRectangle.Right - _TextMarkup.Bounds.Width;
					contentRectangle.Width = _TextMarkup.Bounds.Width;
				}
			}
			_TextMarkup.Arrange(contentRectangle, d);
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
		CreateReflection();
		((Control)this).Invalidate();
		((Control)this).OnEnabledChanged(e);
	}
}
