using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Click")]
[DesignTimeVisible(false)]
public class BubbleButton : Component, IBlock
{
	private string _27BD = "";

	private int դ = -1;

	private int _27BE = -1;

	private bool ݕ = true;

	private Image _27BF;

	private Image _27C0;

	private Image _0E66;

	private Image _27C1;

	private Rectangle ᕮ = Rectangle.Empty;

	private Rectangle _27C2 = Rectangle.Empty;

	private bool ݡ;

	private bool ݠ;

	private BubbleButtonCollection _27C3;

	private string Ӿ = "";

	private bool Ӻ = true;

	private object ӧ;

	private bool _27C4;

	private eShortcut _27C5;

	private ClickEventHandler દ;

	private EventHandler બ;

	private EventHandler ભ;

	private Cursor _נ;

	private Cursor __27C6;

	private Padding __0AE5 = new Padding(0);

	[Description("Indicates shortcut key to expand/collapse splitter.")]
	[DefaultValue(eShortcut.None)]
	[Browsable(true)]
	[Category("Expand")]
	public eShortcut Shortcut
	{
		get
		{
			return _27C5;
		}
		set
		{
			_27C5 = value;
			if (_27C5 != 0 && GetBubbleBar() != null)
			{
				GetBubbleBar()._2803();
			}
		}
	}

	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates tooltip for the button.")]
	public string TooltipText
	{
		get
		{
			return _27BD;
		}
		set
		{
			_27BD = value;
		}
	}

	[Category("Images")]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates default image used on the button.")]
	[Browsable(true)]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			դ = value;
			_27BF = null;
			OnImageIndexChanged();
			OnAppearanceChanged();
		}
	}

	internal Image ImageCached => _27BF;

	internal Image ImageLargeCached => _27C0;

	[DefaultValue(null)]
	[Description("The image that will be displayed on the face of the button.")]
	[Category("Images")]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_0E66 = value;
			OnImageChanged();
		}
	}

	[Description("Indicates enlarged image that will be displayed on the face of the button.")]
	[Category("Images")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Image ImageLarge
	{
		get
		{
			return _27C1;
		}
		set
		{
			_27C1 = value;
			OnImageChanged();
		}
	}

	[Browsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	[Category("Images")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates image index of the enlarged image for the button.")]
	public int ImageIndexLarge
	{
		get
		{
			return _27BE;
		}
		set
		{
			_27BE = value;
			_27C0 = null;
			OnImageIndexChanged();
			OnAppearanceChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList => GetBubbleBar()?.Images;

	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether button is enabled.")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ݕ;
		}
		set
		{
			if (ݕ != value)
			{
				ݕ = value;
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	public Rectangle DisplayRectangle => ᕮ;

	[Browsable(false)]
	public Rectangle MagnifiedDisplayRectangle => _27C2;

	[Browsable(false)]
	public bool MouseOver => ݡ;

	[Browsable(false)]
	public bool MouseDown => ݠ;

	[Browsable(false)]
	[Category("Design")]
	[Description("Indicates the name used to identify button.")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				Ӿ = Site.Name;
			}
			return Ӿ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Determines whether the button is visible or hidden.")]
	[Category("Behavior")]
	public virtual bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			if (Ӻ != value)
			{
				Ӻ = value;
				OnVisibleChanged();
			}
		}
	}

	[Category("Data")]
	[Description("Indicates text that contains data about the cell.")]
	[DefaultValue(null)]
	[Browsable(false)]
	public object Tag
	{
		get
		{
			return ӧ;
		}
		set
		{
			ӧ = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates text that contains data about the cell.")]
	[Category("Data")]
	public string TagString
	{
		get
		{
			if (ӧ == null)
			{
				return "";
			}
			return ӧ.ToString();
		}
		set
		{
			ӧ = value;
		}
	}

	[Browsable(false)]
	public BubbleBarTab Parent
	{
		get
		{
			if (_27C3 != null)
			{
				return _27C3.ކ;
			}
			return null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool Focus
	{
		get
		{
			return _27C4;
		}
		set
		{
			_27C4 = value;
		}
	}

	[Category("Appearance")]
	[Description("Indciates mouse cursor that is displayed when mouse is over the button.")]
	[DefaultValue(null)]
	public Cursor Cursor
	{
		get
		{
			return __27C6;
		}
		set
		{
			__27C6 = value;
		}
	}

	Rectangle IBlock.Bounds
	{
		get
		{
			return ᕮ;
		}
		set
		{
			ᕮ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Padding IBlock.Margin
	{
		get
		{
			return __0AE5;
		}
		set
		{
			__0AE5 = value;
		}
	}

	public event ClickEventHandler Click
	{
		add
		{
			ClickEventHandler clickEventHandler = દ;
			ClickEventHandler clickEventHandler2;
			do
			{
				clickEventHandler2 = clickEventHandler;
				ClickEventHandler value2 = (ClickEventHandler)Delegate.Combine(clickEventHandler2, value);
				clickEventHandler = Interlocked.CompareExchange(ref દ, value2, clickEventHandler2);
			}
			while ((object)clickEventHandler != clickEventHandler2);
		}
		remove
		{
			ClickEventHandler clickEventHandler = દ;
			ClickEventHandler clickEventHandler2;
			do
			{
				clickEventHandler2 = clickEventHandler;
				ClickEventHandler value2 = (ClickEventHandler)Delegate.Remove(clickEventHandler2, value);
				clickEventHandler = Interlocked.CompareExchange(ref દ, value2, clickEventHandler2);
			}
			while ((object)clickEventHandler != clickEventHandler2);
		}
	}

	public event EventHandler MouseEnter
	{
		add
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler MouseLeave
	{
		add
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref _27C1);
			BarUtilities.ᆟ(ref _27BF);
			BarUtilities.ᆟ(ref _27C0);
		}
		base.Dispose(disposing);
	}

	internal void SetDisplayRectangle(Rectangle r)
	{
		ᕮ = r;
	}

	internal void SetMagnifiedDisplayRectangle(Rectangle r)
	{
		_27C2 = r;
	}

	internal void SetMouseOver(bool value)
	{
		bool flag = ݡ != value;
		ݡ = value;
		UpdateCursor();
		if (flag)
		{
			if (value)
			{
				OnMouseEnter();
			}
			else
			{
				OnMouseLeave();
			}
		}
	}

	protected virtual void OnMouseEnter()
	{
		બ?.Invoke(this, new EventArgs());
	}

	protected virtual void OnMouseLeave()
	{
		ભ?.Invoke(this, new EventArgs());
	}

	private void UpdateCursor()
	{
		BubbleBar bubbleBar = GetBubbleBar();
		if (bubbleBar == null || __27C6 == (Cursor)null)
		{
			return;
		}
		if (ݡ)
		{
			if (_נ == (Cursor)null)
			{
				_נ = Cursor.Current;
			}
			((Control)bubbleBar).Cursor = __27C6;
		}
		else if (_נ != (Cursor)null)
		{
			((Control)bubbleBar).Cursor = _נ;
			_נ = null;
		}
	}

	internal void SetMouseDown(bool value)
	{
		ݠ = value;
	}

	internal void SetParentCollection(BubbleButtonCollection value)
	{
		_27C3 = value;
	}

	private void OnAppearanceChanged()
	{
		BubbleBar bubbleBar = GetBubbleBar();
		if (bubbleBar != null)
		{
			((Control)bubbleBar).Refresh();
		}
	}

	private void OnImageIndexChanged()
	{
		BubbleBar bubbleBar = GetBubbleBar();
		if (bubbleBar != null)
		{
			if (դ >= 0 && _27BF == null && bubbleBar.Images != null && դ < bubbleBar.Images.Images.Count)
			{
				_27BF = bubbleBar.Images.Images[դ];
			}
			if (_27BE >= 0 && _27C0 == null && bubbleBar.ImagesLarge != null && _27BE < bubbleBar.ImagesLarge.Images.Count)
			{
				_27C0 = bubbleBar.ImagesLarge.Images[դ];
			}
		}
	}

	private void OnImageChanged()
	{
		BubbleBar bubbleBar = GetBubbleBar();
		if (bubbleBar != null && Site != null && Site.DesignMode)
		{
			((Control)bubbleBar).Refresh();
		}
	}

	public BubbleBar GetBubbleBar()
	{
		if (_27C3 != null && _27C3.ކ != null && _27C3.ކ.Parent != null)
		{
			return _27C3.ކ.Parent;
		}
		return null;
	}

	private void OnVisibleChanged()
	{
		GetBubbleBar()?._2805(this);
	}

	public void InvokeClick(eEventSource source, MouseButtons mouseButton)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		OnClick(new ClickEventArgs(source, mouseButton));
	}

	protected virtual void OnClick(ClickEventArgs e)
	{
		if (દ != null)
		{
			દ(this, e);
		}
		GetBubbleBar()?._27F0(this, e);
	}
}
