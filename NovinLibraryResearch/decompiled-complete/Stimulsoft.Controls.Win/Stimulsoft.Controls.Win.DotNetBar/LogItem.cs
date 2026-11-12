using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class LogItem : BaseItem
{
	private const int DefaultWidth = 96;

	private const int VerticalPadding = 3;

	private const int HorizontalPadding = 7;

	private const int TextLineSpacing = 4;

	private const int MinimumHeight = 32;

	private Size _CalloutSize = new Size(10, 10);

	private int _CornerSize = 5;

	private Color _TextColor = Color.Empty;

	private Color _HeaderTextColor = Color.Empty;

	private Image _Image;

	private Size _ImageSize = new Size(48, 48);

	private eLogItemImagePosition _ImagePosition;

	private string _SourceName = "";

	private DateTime _Created;

	private Color _BackColor = Color.White;

	[Category("Columns")]
	[Description("Indicates color of control text body.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of header text.")]
	public Color HeaderTextColor
	{
		get
		{
			return _HeaderTextColor;
		}
		set
		{
			_HeaderTextColor = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies image displayed on the log item.")]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (_Image == value)
			{
				return;
			}
			_Image = value;
			_ImageSize = new Size(48, 48);
			if (_Image != null)
			{
				try
				{
					_ImageSize = _Image.Size;
				}
				catch
				{
				}
			}
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[DefaultValue(eLogItemImagePosition.Left)]
	[Category("Appearance")]
	[Description("Specifies the image position in relation to text.")]
	public eLogItemImagePosition ImagePosition
	{
		get
		{
			return _ImagePosition;
		}
		set
		{
			if (_ImagePosition != value)
			{
				_ImagePosition = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Specifies the log item source name")]
	public string SourceName
	{
		get
		{
			return _SourceName;
		}
		set
		{
			_SourceName = value;
		}
	}

	public DateTime Created
	{
		get
		{
			return _Created;
		}
		set
		{
			_Created = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the item.")]
	public Color BackColor
	{
		get
		{
			return _BackColor;
		}
		set
		{
			if (value != _BackColor)
			{
				Color backColor = _BackColor;
				_BackColor = value;
				OnBackColorChanged(backColor, value);
			}
		}
	}

	protected override bool IsMarkupSupported => true;

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public LogItem()
		: this("", "")
	{
	}

	public LogItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public LogItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		SetIsContainer(b: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref _Image);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		LogItem logItem = new LogItem(m_Name);
		CopyToItem(logItem);
		return logItem;
	}

	protected override void CopyToItem(BaseItem instanceCopy)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		LogItem logItem = instanceCopy as LogItem;
		base.CopyToItem(instanceCopy);
		logItem.BackColor = BackColor;
		logItem.Created = Created;
		logItem.HeaderTextColor = HeaderTextColor;
		if (Image != null)
		{
			logItem.Image = (Image)Image.Clone();
		}
		logItem.ImagePosition = ImagePosition;
		logItem.SourceName = SourceName;
		logItem.TextColor = TextColor;
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (SubItems.Count > 0)
		{
			BaseItem baseItem = base.ItemAtLocation(x, y);
			if (baseItem != null)
			{
				return baseItem;
			}
		}
		if (!Bounds.Contains(x, y))
		{
			return null;
		}
		return this;
	}

	protected virtual int GetTextWidth(Rectangle bounds)
	{
		return Math.Max(0, bounds.Width - (14 + _CalloutSize.Width + ((_Image != null) ? _ImageSize.Width : 0)));
	}

	public override void RecalcSize()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		Rectangle displayRectangle = DisplayRectangle;
		Size imageSize = _ImageSize;
		if (displayRectangle.Width <= 0)
		{
			if (_Image != null)
			{
				displayRectangle.Width = imageSize.Width;
			}
			displayRectangle.Width += 96;
		}
		if (_Image != null && displayRectangle.Width - (imageSize.Width + 25) <= 1)
		{
			displayRectangle.Height = imageSize.Height;
		}
		else
		{
			int num = 6;
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					int textWidth = GetTextWidth(displayRectangle);
					num += TextDrawing.MeasureString(val2, string.IsNullOrEmpty(SourceName) ? "A" : SourceName, font).Height + 4;
					num += ButtonItemLayout.MeasureItemText(this, val2, textWidth, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1).Height + 4;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			displayRectangle.Height = Math.Max(num, imageSize.Height);
		}
		base.RecalcSize();
		m_Rect = displayRectangle;
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		if (_Image != null)
		{
			Rectangle imageRect = GetImageRect(displayRectangle);
			graphics.DrawImage(_Image, imageRect);
		}
		Rectangle contentBounds = GetContentBounds(displayRectangle);
		Rectangle contentBounds2 = contentBounds;
		contentBounds2.Inflate(1, 0);
		contentBounds2.Offset(0, 1);
		GraphicsPath val = CreateCalloutPath(contentBounds2);
		try
		{
			Pen val2 = new Pen(Color.FromArgb(32, Color.Black));
			try
			{
				graphics.DrawPath(val2, val);
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
		GraphicsPath val3 = CreateCalloutPath(contentBounds);
		try
		{
			Brush calloutBrush = GetCalloutBrush();
			if (calloutBrush != null)
			{
				graphics.FillPath(calloutBrush, val3);
				calloutBrush.Dispose();
			}
			Pen calloutPen = GetCalloutPen();
			if (calloutPen != null)
			{
				graphics.DrawPath(calloutPen, val3);
				calloutPen.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		Color headerColor = GetHeaderColor();
		Rectangle textBounds = GetTextBounds(displayRectangle);
		Rectangle bounds = textBounds;
		string sourceName = SourceName;
		if (!string.IsNullOrEmpty(sourceName))
		{
			Font val4 = new Font(p.Font, (FontStyle)1);
			try
			{
				TextDrawing.DrawString(graphics, sourceName, val4, headerColor, textBounds, eTextFormat.Default);
				bounds.Y += val4.Height + 4;
				bounds.Height -= val4.Height + 4;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (_Created != DateTime.MinValue && _Created != DateTime.MaxValue)
		{
			TimeSpan span = DateTime.Now.Subtract(_Created);
			string humanTimeSpan = GetHumanTimeSpan(span);
			Font val5 = new Font(p.Font, (FontStyle)2);
			try
			{
				TextDrawing.DrawString(graphics, humanTimeSpan, val5, headerColor, textBounds, eTextFormat.Right);
				textBounds.Y += val5.Height + 4;
				textBounds.Height -= val5.Height + 4;
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		string text = Text;
		Color textColor = GetTextColor();
		if (!string.IsNullOrEmpty(text))
		{
			Font font = p.Font;
			if (base.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, textColor, p.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = true;
				base.TextMarkupBody.Bounds = bounds;
				base.TextMarkupBody.Render(markupDrawContext);
			}
			else
			{
				TextDrawing.DrawString(graphics, text, font, textColor, bounds, eTextFormat.WordBreak | eTextFormat.WordEllipsis);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private Color GetTextColor()
	{
		if (!_TextColor.IsEmpty)
		{
			return _TextColor;
		}
		return Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHeaderTextColor()
	{
		return !_HeaderTextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHeaderTextColor()
	{
		HeaderTextColor = Color.Empty;
	}

	private Color GetHeaderColor()
	{
		if (!_HeaderTextColor.IsEmpty)
		{
			return _HeaderTextColor;
		}
		return ColorScheme.GetColor(6130891);
	}

	protected virtual Rectangle GetTextBounds(Rectangle bounds)
	{
		Rectangle contentBounds = GetContentBounds(bounds);
		if (_ImagePosition == eLogItemImagePosition.Left)
		{
			contentBounds.X += _CalloutSize.Width;
		}
		contentBounds.Width -= _CalloutSize.Width;
		contentBounds.Inflate(-7, -3);
		return contentBounds;
	}

	private Brush GetCalloutBrush()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (Brush)new SolidBrush(_BackColor);
	}

	private Pen GetCalloutPen()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		return new Pen(ColorScheme.GetColor(6130889), 1f);
	}

	private GraphicsPath CreateCalloutPath(Rectangle contentBounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_ImagePosition == eLogItemImagePosition.Left)
		{
			if (_CornerSize == 0)
			{
				Rectangle rectangle = contentBounds;
				rectangle.Width--;
				rectangle.Height--;
				int num = _CalloutSize.Height / 2;
				val.AddLine(rectangle.X + _CalloutSize.Width, rectangle.Y + _CalloutSize.Height + num, rectangle.X, rectangle.Y + num * 2);
				val.AddLine(rectangle.X, rectangle.Y + num * 2, rectangle.X + _CalloutSize.Width, rectangle.Y + num);
				val.AddLine(rectangle.X + _CalloutSize.Width, rectangle.Y, rectangle.Right, rectangle.Y);
				val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.X + _CalloutSize.Width, rectangle.Bottom);
				val.CloseAllFigures();
			}
			else
			{
				Rectangle bounds = contentBounds;
				bounds.Width--;
				bounds.Height--;
				int cornerSize = _CornerSize;
				int num2 = _CalloutSize.Height / 2;
				val.AddLine(bounds.X + _CalloutSize.Width, bounds.Y + cornerSize + _CalloutSize.Height + num2, bounds.X, bounds.Y + cornerSize + num2 * 2);
				val.AddLine(bounds.X, bounds.Y + cornerSize + num2 * 2, bounds.X + _CalloutSize.Width, bounds.Y + cornerSize + num2);
				bounds.X += _CalloutSize.Width;
				bounds.Width -= _CalloutSize.Width;
				ArcData cornerArc = ElementStyleDisplay.GetCornerArc(bounds, _CornerSize, eCornerArc.TopLeft);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = ElementStyleDisplay.GetCornerArc(bounds, _CornerSize, eCornerArc.TopRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = ElementStyleDisplay.GetCornerArc(bounds, _CornerSize, eCornerArc.BottomRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = ElementStyleDisplay.GetCornerArc(bounds, _CornerSize, eCornerArc.BottomLeft);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				val.CloseAllFigures();
			}
		}
		else if (_CornerSize == 0)
		{
			Rectangle rectangle2 = contentBounds;
			rectangle2.Width--;
			rectangle2.Height--;
			int num3 = _CalloutSize.Height / 2;
			val.AddLine(rectangle2.Right - _CalloutSize.Width, rectangle2.Y + _CalloutSize.Height + num3, rectangle2.Right, rectangle2.Y + num3 * 2);
			val.AddLine(rectangle2.Right, rectangle2.Y + num3 * 2, rectangle2.Right - _CalloutSize.Width, rectangle2.Y + num3);
			val.AddLine(rectangle2.Right - _CalloutSize.Width, rectangle2.Y, rectangle2.X, rectangle2.Y);
			val.AddLine(rectangle2.X, rectangle2.Bottom, rectangle2.Right - _CalloutSize.Width, rectangle2.Bottom);
			val.CloseAllFigures();
		}
		else
		{
			Rectangle bounds2 = contentBounds;
			bounds2.Width--;
			bounds2.Height--;
			int cornerSize2 = _CornerSize;
			int num4 = _CalloutSize.Height / 2;
			bounds2.Width -= _CalloutSize.Width;
			ArcData cornerArc2 = ElementStyleDisplay.GetCornerArc(bounds2, _CornerSize, eCornerArc.TopLeft);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			cornerArc2 = ElementStyleDisplay.GetCornerArc(bounds2, _CornerSize, eCornerArc.TopRight);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			bounds2.Width += _CalloutSize.Width;
			val.AddLine(bounds2.Right - _CalloutSize.Width, bounds2.Y + num4, bounds2.Right, bounds2.Y + num4 * 2);
			val.AddLine(bounds2.Right, bounds2.Y + num4 * 2, bounds2.Right - _CalloutSize.Width, bounds2.Y + _CalloutSize.Height + num4);
			bounds2.Width -= _CalloutSize.Width;
			cornerArc2 = ElementStyleDisplay.GetCornerArc(bounds2, _CornerSize, eCornerArc.BottomRight);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			cornerArc2 = ElementStyleDisplay.GetCornerArc(bounds2, _CornerSize, eCornerArc.BottomLeft);
			val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
			val.CloseAllFigures();
		}
		return val;
	}

	private Rectangle GetContentBounds(Rectangle bounds)
	{
		if (_Image != null)
		{
			if (_ImagePosition == eLogItemImagePosition.Left)
			{
				bounds.X += _ImageSize.Width;
			}
			bounds.Width -= _ImageSize.Width;
		}
		bounds.Height--;
		bounds.Width--;
		return bounds;
	}

	private Rectangle GetImageRect(Rectangle bounds)
	{
		if (_ImagePosition == eLogItemImagePosition.Left)
		{
			return new Rectangle(bounds.X, bounds.Y, _ImageSize.Width, _ImageSize.Height);
		}
		return new Rectangle(bounds.Right - _ImageSize.Width, bounds.Y, _ImageSize.Width, _ImageSize.Height);
	}

	private string GetHumanTimeSpan(TimeSpan span)
	{
		if (span.TotalMinutes <= 1.5)
		{
			return "now";
		}
		if (span.TotalMinutes < 2.0)
		{
			return "about a minute ago";
		}
		if (span.TotalMinutes < 60.0)
		{
			return span.TotalMinutes.ToString("N0") + " minutes ago";
		}
		if (span.TotalHours < 2.0)
		{
			return span.TotalHours.ToString("N0") + " hour ago";
		}
		if (span.TotalHours < 24.0)
		{
			return span.TotalHours.ToString("N0") + " hours ago";
		}
		if (span.TotalDays < 2.0)
		{
			return span.TotalDays.ToString("N0") + " day ago";
		}
		if (span.TotalDays < 30.0)
		{
			return span.TotalDays.ToString("N0") + " days ago";
		}
		return (span.TotalDays / 30.0).ToString("N0") + " months ago";
	}

	private void OnBackColorChanged(Color oldValue, Color newValue)
	{
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return _BackColor != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.White;
	}

	protected void SetBackColor(Color color)
	{
		_BackColor = color;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}
}
