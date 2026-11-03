using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

public class LogItem : BaseItem
{
	private const int _2EE0 = 96;

	private const int _0E62 = 3;

	private const int _0E63 = 7;

	private const int _2EE1 = 4;

	private const int _2EE2 = 32;

	private Size __2EE3 = new Size(10, 10);

	private int __2EE4 = 5;

	private MarkupLinkClickEventHandler Ӟ;

	private Color __0CD1 = Color.Empty;

	private Color __2EE5 = Color.Empty;

	private Image _ነ;

	private Size __2EE6 = new Size(48, 48);

	private eLogItemImagePosition __2EE7;

	private string __2EE8 = "";

	private DateTime __2EE9;

	private Color __0CD2 = Color.White;

	[Category("Columns")]
	[Description("Indicates color of control text body.")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
		}
	}

	[Description("Indicates color of header text.")]
	[Category("Columns")]
	public Color HeaderTextColor
	{
		get
		{
			return __2EE5;
		}
		set
		{
			__2EE5 = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies image displayed on the log item.")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (_ነ == value)
			{
				return;
			}
			_ነ = value;
			__2EE6 = new Size(48, 48);
			if (_ነ != null)
			{
				try
				{
					__2EE6 = _ነ.Size;
				}
				catch
				{
				}
			}
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Specifies the image position in relation to text.")]
	[DefaultValue(eLogItemImagePosition.Left)]
	[Category("Appearance")]
	public eLogItemImagePosition ImagePosition
	{
		get
		{
			return __2EE7;
		}
		set
		{
			if (__2EE7 != value)
			{
				__2EE7 = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Description("Specifies the log item source name")]
	[Category("Appearance")]
	[DefaultValue("")]
	public string SourceName
	{
		get
		{
			return __2EE8;
		}
		set
		{
			__2EE8 = value;
		}
	}

	public DateTime Created
	{
		get
		{
			return __2EE9;
		}
		set
		{
			__2EE9 = value;
		}
	}

	[Description("Indicates background color of the item.")]
	[Category("Appearance")]
	public Color BackColor
	{
		get
		{
			return __0CD2;
		}
		set
		{
			if (value != __0CD2)
			{
				Color ك = __0CD2;
				__0CD2 = value;
				_2EF1(ك, value);
			}
		}
	}

	protected override bool IsMarkupSupported => true;

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
			BarUtilities.ᆟ(ref _ነ);
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
		return Math.Max(0, bounds.Width - (14 + __2EE3.Width + ((_ነ != null) ? __2EE6.Width : 0)));
	}

	public override void RecalcSize()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		Rectangle displayRectangle = DisplayRectangle;
		Size size = __2EE6;
		if (displayRectangle.Width <= 0)
		{
			if (_ነ != null)
			{
				displayRectangle.Width = size.Width;
			}
			displayRectangle.Width += 96;
		}
		if (_ነ != null && displayRectangle.Width - (size.Width + 25) <= 1)
		{
			displayRectangle.Height = size.Height;
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
					num += _20B1._20B2(this, val2, textWidth, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1).Height + 4;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			displayRectangle.Height = Math.Max(num, size.Height);
		}
		base.RecalcSize();
		m_Rect = displayRectangle;
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		if (_ነ != null)
		{
			Rectangle rectangle = _2EEF(displayRectangle);
			graphics.DrawImage(_ነ, rectangle);
		}
		Rectangle rectangle2 = _2B08(displayRectangle);
		Rectangle _2EEE = rectangle2;
		_2EEE.Inflate(1, 0);
		_2EEE.Offset(0, 1);
		GraphicsPath val = _2EED(_2EEE);
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
		GraphicsPath val3 = _2EED(rectangle2);
		try
		{
			Brush val4 = _2EEB();
			if (val4 != null)
			{
				graphics.FillPath(val4, val3);
				val4.Dispose();
			}
			Pen val5 = _2EEC();
			if (val5 != null)
			{
				graphics.DrawPath(val5, val3);
				val5.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		Color color = _2EEA();
		Rectangle textBounds = GetTextBounds(displayRectangle);
		Rectangle bounds = textBounds;
		string sourceName = SourceName;
		if (!string.IsNullOrEmpty(sourceName))
		{
			Font val6 = new Font(p.Font, (FontStyle)1);
			try
			{
				TextDrawing.DrawString(graphics, sourceName, val6, color, textBounds, eTextFormat.Default);
				bounds.Y += val6.Height + 4;
				bounds.Height -= val6.Height + 4;
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (__2EE9 != DateTime.MinValue && __2EE9 != DateTime.MaxValue)
		{
			TimeSpan _2042 = DateTime.Now.Subtract(__2EE9);
			string text = _2EF0(_2042);
			Font val7 = new Font(p.Font, (FontStyle)2);
			try
			{
				TextDrawing.DrawString(graphics, text, val7, color, textBounds, eTextFormat.Right);
				textBounds.Y += val7.Height + 4;
				textBounds.Height -= val7.Height + 4;
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		string text2 = Text;
		Color color2 = _1933();
		if (!string.IsNullOrEmpty(text2))
		{
			Font font = p.Font;
			if (base.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color2, p.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = true;
				base.TextMarkupBody.Bounds = bounds;
				base.TextMarkupBody.Render(markupDrawContext);
			}
			else
			{
				TextDrawing.DrawString(graphics, text2, font, color2, bounds, eTextFormat.WordBreak | eTextFormat.WordEllipsis);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private Color _1933()
	{
		if (!__0CD1.IsEmpty)
		{
			return __0CD1;
		}
		return Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHeaderTextColor()
	{
		return !__2EE5.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHeaderTextColor()
	{
		HeaderTextColor = Color.Empty;
	}

	private Color _2EEA()
	{
		if (!__2EE5.IsEmpty)
		{
			return __2EE5;
		}
		return ColorScheme.GetColor(6130891);
	}

	protected virtual Rectangle GetTextBounds(Rectangle bounds)
	{
		Rectangle result = _2B08(bounds);
		if (__2EE7 == eLogItemImagePosition.Left)
		{
			result.X += __2EE3.Width;
		}
		result.Width -= __2EE3.Width;
		result.Inflate(-7, -3);
		return result;
	}

	private Brush _2EEB()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (Brush)new SolidBrush(__0CD2);
	}

	private Pen _2EEC()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		return new Pen(ColorScheme.GetColor(6130889), 1f);
	}

	private GraphicsPath _2EED(Rectangle _2EEE)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (__2EE7 == eLogItemImagePosition.Left)
		{
			if (__2EE4 == 0)
			{
				Rectangle rectangle = _2EEE;
				rectangle.Width--;
				rectangle.Height--;
				int num = __2EE3.Height / 2;
				val.AddLine(rectangle.X + __2EE3.Width, rectangle.Y + __2EE3.Height + num, rectangle.X, rectangle.Y + num * 2);
				val.AddLine(rectangle.X, rectangle.Y + num * 2, rectangle.X + __2EE3.Width, rectangle.Y + num);
				val.AddLine(rectangle.X + __2EE3.Width, rectangle.Y, rectangle.Right, rectangle.Y);
				val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.X + __2EE3.Width, rectangle.Bottom);
				val.CloseAllFigures();
			}
			else
			{
				Rectangle ശ = _2EEE;
				ശ.Width--;
				ശ.Height--;
				int num2 = __2EE4;
				int num3 = __2EE3.Height / 2;
				val.AddLine(ശ.X + __2EE3.Width, ശ.Y + num2 + __2EE3.Height + num3, ശ.X, ശ.Y + num2 + num3 * 2);
				val.AddLine(ശ.X, ശ.Y + num2 + num3 * 2, ശ.X + __2EE3.Width, ശ.Y + num2 + num3);
				ശ.X += __2EE3.Width;
				ശ.Width -= __2EE3.Width;
				ᒇ ᒇ2 = ElementStyleDisplay.സ(ശ, __2EE4, eCornerArc.TopLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = ElementStyleDisplay.സ(ശ, __2EE4, eCornerArc.TopRight);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = ElementStyleDisplay.സ(ശ, __2EE4, eCornerArc.BottomRight);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = ElementStyleDisplay.സ(ശ, __2EE4, eCornerArc.BottomLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				val.CloseAllFigures();
			}
		}
		else if (__2EE4 == 0)
		{
			Rectangle rectangle2 = _2EEE;
			rectangle2.Width--;
			rectangle2.Height--;
			int num4 = __2EE3.Height / 2;
			val.AddLine(rectangle2.Right - __2EE3.Width, rectangle2.Y + __2EE3.Height + num4, rectangle2.Right, rectangle2.Y + num4 * 2);
			val.AddLine(rectangle2.Right, rectangle2.Y + num4 * 2, rectangle2.Right - __2EE3.Width, rectangle2.Y + num4);
			val.AddLine(rectangle2.Right - __2EE3.Width, rectangle2.Y, rectangle2.X, rectangle2.Y);
			val.AddLine(rectangle2.X, rectangle2.Bottom, rectangle2.Right - __2EE3.Width, rectangle2.Bottom);
			val.CloseAllFigures();
		}
		else
		{
			Rectangle ശ2 = _2EEE;
			ശ2.Width--;
			ശ2.Height--;
			int num5 = __2EE3.Height / 2;
			ശ2.Width -= __2EE3.Width;
			ᒇ ᒇ3 = ElementStyleDisplay.സ(ശ2, __2EE4, eCornerArc.TopLeft);
			val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
			ᒇ3 = ElementStyleDisplay.സ(ശ2, __2EE4, eCornerArc.TopRight);
			val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
			ശ2.Width += __2EE3.Width;
			val.AddLine(ശ2.Right - __2EE3.Width, ശ2.Y + num5, ശ2.Right, ശ2.Y + num5 * 2);
			val.AddLine(ശ2.Right, ശ2.Y + num5 * 2, ശ2.Right - __2EE3.Width, ശ2.Y + __2EE3.Height + num5);
			ശ2.Width -= __2EE3.Width;
			ᒇ3 = ElementStyleDisplay.സ(ശ2, __2EE4, eCornerArc.BottomRight);
			val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
			ᒇ3 = ElementStyleDisplay.സ(ശ2, __2EE4, eCornerArc.BottomLeft);
			val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
			val.CloseAllFigures();
		}
		return val;
	}

	private Rectangle _2B08(Rectangle ശ)
	{
		if (_ነ != null)
		{
			if (__2EE7 == eLogItemImagePosition.Left)
			{
				ശ.X += __2EE6.Width;
			}
			ശ.Width -= __2EE6.Width;
		}
		ശ.Height--;
		ശ.Width--;
		return ശ;
	}

	private Rectangle _2EEF(Rectangle ശ)
	{
		if (__2EE7 == eLogItemImagePosition.Left)
		{
			return new Rectangle(ശ.X, ശ.Y, __2EE6.Width, __2EE6.Height);
		}
		return new Rectangle(ശ.Right - __2EE6.Width, ശ.Y, __2EE6.Width, __2EE6.Height);
	}

	private string _2EF0(TimeSpan _2042)
	{
		if (_2042.TotalMinutes <= 1.5)
		{
			return "now";
		}
		if (_2042.TotalMinutes < 2.0)
		{
			return "about a minute ago";
		}
		if (_2042.TotalMinutes < 60.0)
		{
			return _2042.TotalMinutes.ToString("N0") + " minutes ago";
		}
		if (_2042.TotalHours < 2.0)
		{
			return _2042.TotalHours.ToString("N0") + " hour ago";
		}
		if (_2042.TotalHours < 24.0)
		{
			return _2042.TotalHours.ToString("N0") + " hours ago";
		}
		if (_2042.TotalDays < 2.0)
		{
			return _2042.TotalDays.ToString("N0") + " day ago";
		}
		if (_2042.TotalDays < 30.0)
		{
			return _2042.TotalDays.ToString("N0") + " days ago";
		}
		return (_2042.TotalDays / 30.0).ToString("N0") + " months ago";
	}

	private void _2EF1(Color ك, Color ل)
	{
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return __0CD2 != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.White;
	}

	protected void SetBackColor(Color color)
	{
		__0CD2 = color;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}
}
