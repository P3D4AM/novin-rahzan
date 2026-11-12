using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class Scrolling : IScrolling
{
	private RichTextBoxScrollBars scrollBars = (RichTextBoxScrollBars)3;

	private int defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;

	private int windowOriginX;

	private int windowOriginY;

	private int scrollUpdateCount;

	private ScrollingOptions options = EditConsts.DefaultScrollingOptions;

	private ScrollBar hScrollBar;

	private ScrollBar vScrollBar;

	private IScrollingButtons horzButtons;

	private IScrollingButtons vertButtons;

	private IScrollingButton horzSplitButton;

	private IScrollingButton vertSplitButton;

	private int updateCount;

	private bool flatBars;

	private ISyntaxEdit owner;

	public ISyntaxEdit Edit => owner;

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the type of scroll bars displayed in the control.")]
	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return scrollBars;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (scrollBars != value)
			{
				scrollBars = value;
				UpdateScroll(updateSize: true);
			}
		}
	}

	[Description("Gets or sets a default size of horizontal scrollbar.")]
	public int DefaultHorzScrollSize
	{
		get
		{
			return defaultHorzScrollSize;
		}
		set
		{
			if (defaultHorzScrollSize != value)
			{
				defaultHorzScrollSize = value;
				UpdateScroll(updateSize: false);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int WindowOriginY
	{
		get
		{
			return windowOriginY;
		}
		set
		{
			value = Math.Max(value, 0);
			if (owner == null)
			{
				return;
			}
			if (owner.Pages.PageType == PageType.PageLayout)
			{
				int num = owner.Pages.Height - owner.ClientRect.Height;
				if (value >= num && num >= 0)
				{
					value = num;
				}
			}
			else if ((NavigateOptions.BeyondEof & owner.NavigateOptions) == 0)
			{
				int num2 = owner.DisplayLines.GetCount() - owner.LinesInHeight() + 1;
				if (value >= num2)
				{
					value = Math.Max(num2, 0);
				}
			}
			if (windowOriginY != value)
			{
				int line = windowOriginY;
				windowOriginY = value;
				UpdateScrollPosition();
				((SyntaxEdit)owner).WindowOriginChanged(line, windowOriginX);
				OnVerticalScroll();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int WindowOriginX
	{
		get
		{
			return windowOriginX;
		}
		set
		{
			value = Math.Max(value, 0);
			if (owner != null)
			{
				if (owner.Pages.PageType == PageType.PageLayout)
				{
					int num = owner.Pages.Width - owner.ClientRect.Width;
					if (value >= num && num >= 0)
					{
						value = num;
					}
				}
				else if ((NavigateOptions.BeyondEol & owner.NavigateOptions) == 0 && owner.Painter.IsMonoSpaced && owner.Painter.FontWidth != 0 && ((DisplayStrings)owner.DisplayLines).NeedMaxLine())
				{
					int num2 = owner.DisplayLines.MaxLineWidth / owner.Painter.FontWidth - owner.CharsInWidth();
					if (value >= num2)
					{
						value = Math.Max(num2, 0);
					}
				}
			}
			if (windowOriginX != value)
			{
				int ch = windowOriginX;
				windowOriginX = value;
				UpdateScrollPosition();
				((SyntaxEdit)owner).WindowOriginChanged(windowOriginY, ch);
				OnHorizontalScroll();
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets a \"ScrollingOptions\" that determine scrolling behaviour.")]
	public ScrollingOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			if (options == value)
			{
				return;
			}
			bool flag = ((options ^ value) & ScrollingOptions.SystemScrollbars) != 0;
			if (flag)
			{
				if ((value & ScrollingOptions.SystemScrollbars) != 0)
				{
					if ((value & ScrollingOptions.AllowSplitHorz) != 0)
					{
						value &= ~ScrollingOptions.AllowSplitHorz;
					}
					if ((value & ScrollingOptions.AllowSplitVert) != 0)
					{
						value &= ~ScrollingOptions.AllowSplitVert;
					}
					if ((value & ScrollingOptions.VertButtons) != 0)
					{
						value &= ~ScrollingOptions.VertButtons;
					}
					if ((value & ScrollingOptions.HorzButtons) != 0)
					{
						value &= ~ScrollingOptions.HorzButtons;
					}
					RemoveScrollbars(system: false);
				}
				else
				{
					if ((value & ScrollingOptions.FlatScrollbars) != 0)
					{
						value &= ~ScrollingOptions.FlatScrollbars;
					}
					RemoveScrollbars(system: true);
				}
			}
			bool flag2 = ((options ^ value) & ScrollingOptions.FlatScrollbars) != 0;
			options = value;
			if (flag)
			{
				RemoveScrollbars((options & ScrollingOptions.SystemScrollbars) == 0);
			}
			if (flag2)
			{
				SetFlatBars((options & ScrollingOptions.FlatScrollbars) != 0);
			}
			UpdateScroll(updateSize: true);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ScrollBar VScrollBar
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			if (vScrollBar == null)
			{
				vScrollBar = (ScrollBar)new VScrollBar();
				((Control)vScrollBar).Visible = false;
				((Control)vScrollBar).Height = 0;
				((Control)vScrollBar).Cursor = Cursors.Default;
				((Control)vScrollBar).Parent = (Control)owner;
				vScrollBar.Scroll += new ScrollEventHandler(vScrollBar_Scroll);
				((Control)vScrollBar).BringToFront();
			}
			return vScrollBar;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ScrollBar HScrollBar
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			if (hScrollBar == null)
			{
				hScrollBar = (ScrollBar)new HScrollBar();
				((Control)hScrollBar).Visible = false;
				((Control)hScrollBar).Width = 0;
				((Control)hScrollBar).Cursor = Cursors.Default;
				((Control)hScrollBar).Parent = (Control)owner;
				hScrollBar.Scroll += new ScrollEventHandler(hScrollBar_Scroll);
				((Control)hScrollBar).BringToFront();
			}
			return hScrollBar;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HasHScrollBar
	{
		get
		{
			if (hScrollBar != null)
			{
				return ((Control)hScrollBar).Visible;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HasVScrollBar
	{
		get
		{
			if (vScrollBar != null)
			{
				return ((Control)vScrollBar).Visible;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool VertScrollbarVisible
	{
		get
		{
			if ((options & ScrollingOptions.SystemScrollbars) != 0)
			{
				if (owner.IsHandleCreated)
				{
					return OSUtils.GetScrollSize(owner.Handle, flatBars, isVertical: true) > 0;
				}
				return false;
			}
			return HasVScrollBar;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HorzScrollbarVisible
	{
		get
		{
			if ((options & ScrollingOptions.SystemScrollbars) != 0)
			{
				if (owner.IsHandleCreated)
				{
					return OSUtils.GetScrollSize(owner.Handle, flatBars, isVertical: false) > 0;
				}
				return false;
			}
			return HasHScrollBar;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IScrollingButtons HorzButtons => horzButtons;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IScrollingButtons VertButtons => vertButtons;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
	[Category("Scrolling")]
	[Description("Represents a collection of additional buttons displayed at the horizontal scroll bar area.")]
	public ScrollingButtons HorizontalButtons => (ScrollingButtons)horzButtons;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
	[Category("Scrolling")]
	[Description("Represents a collection of additional buttons displayed at the vertical scroll bar area.")]
	public ScrollingButtons VerticalButtons => (ScrollingButtons)vertButtons;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IScrollingButton VertSplitButton
	{
		get
		{
			if (vertSplitButton == null && CanSplitVert())
			{
				vertSplitButton = CreateSplitterButton(vert: true);
			}
			return vertSplitButton;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IScrollingButton HorzSplitButton
	{
		get
		{
			if (horzSplitButton == null && CanSplitHorz())
			{
				horzSplitButton = CreateSplitterButton(vert: false);
			}
			return horzSplitButton;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlScrollingInfo(this);
		}
		set
		{
			((XmlScrollingInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event EventHandler VerticalScroll;

	[Browsable(false)]
	public event EventHandler HorizontalScroll;

	[Browsable(false)]
	public event EventHandler ScrollButtonClick;

	public Scrolling(ISyntaxEdit owner)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		horzButtons = new ScrollingButtons(this);
		vertButtons = new ScrollingButtons(this);
	}

	~Scrolling()
	{
		horzButtons.Clear();
		vertButtons.Clear();
	}

	private int ScrollHeight()
	{
		if (owner != null)
		{
			if (owner.Pages.PageType == PageType.PageLayout)
			{
				return owner.Pages.Height;
			}
			return owner.DisplayLines.GetCount();
		}
		return 0;
	}

	private int ScrollWidth()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		if (owner == null)
		{
			return 0;
		}
		switch (owner.Pages.PageType)
		{
		case PageType.PageBreaks:
			return owner.CharsInWidth(owner.Pages.Width);
		case PageType.PageLayout:
			return owner.Pages.Width;
		default:
		{
			if (((int)scrollBars == 19 || (int)scrollBars == 17) && defaultHorzScrollSize > 0)
			{
				return defaultHorzScrollSize;
			}
			if (owner.WordWrap)
			{
				if (owner.WrapAtMargin)
				{
					return owner.EditMargin.Position;
				}
				return owner.CharsInWidth();
			}
			int maxLineWidth = owner.DisplayLines.MaxLineWidth;
			if (owner.Painter.FontWidth != 0)
			{
				int num = maxLineWidth / owner.Painter.FontWidth;
				if (maxLineWidth % owner.Painter.FontWidth != 0)
				{
					num++;
				}
				return num;
			}
			return defaultHorzScrollSize;
		}
		}
	}

	private void GetScrollCodes(out bool vert, out bool horz, out bool forced)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected I4, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected I4, but got Unknown
		vert = false;
		horz = false;
		forced = false;
		RichTextBoxScrollBars val = scrollBars;
		switch (val - 1)
		{
		case 2:
			vert = true;
			horz = true;
			return;
		case 0:
			horz = true;
			return;
		case 1:
			vert = true;
			return;
		}
		switch (val - 17)
		{
		case 2:
			horz = true;
			vert = true;
			forced = true;
			break;
		case 0:
			horz = true;
			forced = true;
			break;
		case 1:
			vert = true;
			forced = true;
			break;
		}
	}

	private void SetScrollBar(int size, int pageSize, bool vert)
	{
		if ((options & ScrollingOptions.SystemScrollbars) != 0)
		{
			OSUtils.SetScrollBar(owner.Handle, flatBars, Math.Max(size - 1, 0), pageSize, vert);
			return;
		}
		ScrollBar val = (vert ? vScrollBar : hScrollBar);
		if (pageSize < 0)
		{
			if (val != null)
			{
				((Control)val).Visible = false;
				if (vert)
				{
					((Control)val).Height = 0;
				}
				else
				{
					((Control)val).Width = 0;
				}
			}
		}
		else
		{
			val = (vert ? VScrollBar : HScrollBar);
			((Control)val).Visible = true;
			val.Maximum = size;
			val.SmallChange = 1;
			val.LargeChange = pageSize;
			((Control)val).Enabled = size >= pageSize;
		}
	}

	private void SetScrollPos(bool vert, int value)
	{
		if ((options & ScrollingOptions.SystemScrollbars) != 0)
		{
			OSUtils.SetScrollPos(owner.Handle, flatBars, value, vert);
			return;
		}
		ScrollBar val = (vert ? vScrollBar : hScrollBar);
		if (val != null)
		{
			val.Value = Math.Min(value, val.Maximum);
		}
	}

	protected bool CanSplitVert()
	{
		if ((options & ScrollingOptions.AllowSplitVert) != 0)
		{
			return owner.CanSplitVert();
		}
		return false;
	}

	protected bool CanSplitHorz()
	{
		if ((options & ScrollingOptions.AllowSplitHorz) != 0)
		{
			return owner.CanSplitHorz();
		}
		return false;
	}

	protected void RepositionScrollBars()
	{
		Rectangle r = owner.ClientRectangle;
		if (HasVScrollBar)
		{
			r.Width -= ((Control)VScrollBar).Width;
		}
		if (HasHScrollBar)
		{
			r.Height -= ((Control)HScrollBar).Height;
		}
		Rectangle rectangle = r;
		UpdateScrollButtons(vertButtons, vert: true, ref r);
		if (HasVScrollBar)
		{
			((Control)VScrollBar).Bounds = new Rectangle(rectangle.Right, r.Top, ((Control)VScrollBar).Width, r.Height);
		}
		r = rectangle;
		UpdateScrollButtons(horzButtons, vert: false, ref r);
		if (HasHScrollBar)
		{
			((Control)HScrollBar).Bounds = new Rectangle(r.Left, rectangle.Bottom, r.Width, ((Control)HScrollBar).Height);
		}
	}

	protected void UpdateScrollSize(bool updateSize)
	{
		if (owner == null || !owner.IsHandleCreated)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		GetScrollCodes(out var vert, out var horz, out var forced);
		bool hasVScrollBar = HasVScrollBar;
		bool hasHScrollBar = HasHScrollBar;
		if (vert)
		{
			num = ScrollHeight();
			num2 = ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Height : owner.LinesInHeight());
			if (!forced && num <= num2)
			{
				num2 = -1;
			}
			SetScrollBar(num, num2, vert: true);
		}
		else
		{
			SetScrollBar(0, -1, vert: true);
		}
		if (horz)
		{
			num = ScrollWidth();
			num2 = ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Width : owner.CharsInWidth());
			if (!forced && num <= num2)
			{
				num2 = -1;
			}
			SetScrollBar(num, num2, vert: false);
		}
		else
		{
			SetScrollBar(0, -1, vert: false);
		}
		if ((options & ScrollingOptions.SystemScrollbars) == 0 && (updateSize || hasVScrollBar != HasVScrollBar || hasHScrollBar != HasHScrollBar))
		{
			RepositionScrollBars();
		}
	}

	protected void UpdateScrollPosition()
	{
		if (owner == null || !owner.IsHandleCreated)
		{
			return;
		}
		scrollUpdateCount++;
		try
		{
			GetScrollCodes(out var vert, out var horz, out var _);
			if (vert)
			{
				SetScrollPos(vert: true, windowOriginY);
			}
			if (horz)
			{
				SetScrollPos(vert: false, windowOriginX);
			}
		}
		finally
		{
			scrollUpdateCount--;
		}
	}

	protected void SplitterMouseDown(object sender, MouseEventArgs e)
	{
		Splitter val = null;
		if (vertSplitButton != null && sender == ((ScrollingButton)vertSplitButton).Button)
		{
			owner.SplitViewVert();
			val = owner.VertSplitter;
		}
		else if (horzSplitButton != null && sender == ((ScrollingButton)horzSplitButton).Button)
		{
			owner.SplitViewHorz();
			val = owner.HorzSplitter;
		}
		if (val == null || !((Control)val).IsHandleCreated)
		{
			return;
		}
		if (owner.IsHandleCreated)
		{
			owner.Update();
			if (vScrollBar != null)
			{
				((Control)vScrollBar).Update();
			}
			if (hScrollBar != null)
			{
				((Control)hScrollBar).Update();
			}
		}
		OSUtils.SendMessage(((Control)val).Handle, 513, IntPtr.Zero, IntPtr.Zero);
	}

	protected internal void ButtonClick(object sender, EventArgs e)
	{
		if (this.ScrollButtonClick == null)
		{
			return;
		}
		foreach (ScrollingButton vertButton in vertButtons)
		{
			if (sender == vertButton.Button)
			{
				this.ScrollButtonClick(vertButton, e);
				return;
			}
		}
		foreach (ScrollingButton horzButton in horzButtons)
		{
			if (sender == horzButton.Button)
			{
				this.ScrollButtonClick(horzButton, e);
				break;
			}
		}
	}

	protected virtual IScrollingButton CreateSplitterButton(bool vert)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		ScrollSplitter scrollSplitter = new ScrollSplitter();
		scrollSplitter.Visible = false;
		SpeedButton button = scrollSplitter.Button;
		if (vert)
		{
			((Control)button).Cursor = Cursors.VSplit;
			((Control)button).Width = EditConsts.DefaltScrollSplitterSize;
		}
		else
		{
			((Control)button).Cursor = Cursors.HSplit;
			((Control)button).Height = EditConsts.DefaltScrollSplitterSize;
		}
		((Control)button).Parent = (Control)owner;
		((Control)button).BringToFront();
		((Control)button).Dock = (DockStyle)0;
		((Control)button).MouseDown += new MouseEventHandler(SplitterMouseDown);
		return scrollSplitter;
	}

	protected virtual void UpdateScrollSplitters(bool vert, ref Rectangle r)
	{
		ScrollBar val = (vert ? vScrollBar : hScrollBar);
		vert = !vert;
		if (val == null || !((Control)val).Visible || (vert && !CanSplitVert()) || (!vert && !CanSplitHorz()))
		{
			IScrollingButton scrollingButton = (vert ? vertSplitButton : horzSplitButton);
			if (scrollingButton != null)
			{
				scrollingButton.Visible = false;
			}
			return;
		}
		if (vert && CanSplitVert())
		{
			IScrollingButton scrollingButton = VertSplitButton;
			if (scrollingButton != null)
			{
				SpeedButton button = ((ScrollingButton)scrollingButton).Button;
				((Control)button).Height = ((Control)val).Height;
				((Control)button).Location = new Point(0, r.Bottom);
				r.X += ((Control)button).Width;
				r.Width -= ((Control)button).Width;
				((Control)button).Visible = true;
			}
		}
		if (!vert && CanSplitHorz())
		{
			IScrollingButton scrollingButton = HorzSplitButton;
			if (scrollingButton != null)
			{
				SpeedButton button2 = ((ScrollingButton)scrollingButton).Button;
				((Control)button2).Width = ((Control)val).Width;
				((Control)button2).Location = new Point(r.Right, 0);
				r.Y += ((Control)button2).Height;
				r.Height -= ((Control)button2).Height;
				((Control)button2).Visible = true;
			}
		}
	}

	protected virtual void UpdateScrollButtons(IScrollingButtons buttons, bool vert, ref Rectangle r)
	{
		UpdateScrollSplitters(vert, ref r);
		ScrollBar val = (vert ? vScrollBar : hScrollBar);
		foreach (ScrollingButton button2 in buttons)
		{
			SpeedButton button = button2.Button;
			bool flag = val != null && ((Control)val).Visible && ((vert && (options & ScrollingOptions.VertButtons) != 0) || (!vert && (options & ScrollingOptions.HorzButtons) != 0));
			if (flag)
			{
				if (vert)
				{
					((Control)button).Width = ((Control)val).Width;
					((Control)button).Height = ((Control)val).Width;
					((Control)button).Location = new Point(r.Right, r.Bottom - ((Control)button).Height);
					r.Height -= ((Control)button).Height;
				}
				else
				{
					((Control)button).Height = ((Control)val).Height;
					((Control)button).Width = ((Control)val).Height;
					((Control)button).Location = new Point(r.X, r.Bottom);
					r.X += ((Control)button).Width;
					r.Width -= ((Control)button).Width;
				}
			}
			button2.Visible = flag;
		}
	}

	protected internal void UpdateScroll()
	{
		UpdateScroll(updateSize: false);
	}

	protected internal void UpdateScroll(bool updateSize)
	{
		if (updateCount <= 0)
		{
			WindowOriginY = windowOriginY;
			WindowOriginX = windowOriginX;
			UpdateScrollSize(updateSize);
			UpdateScrollPosition();
			if (owner != null)
			{
				((SyntaxEdit)owner).CheckResizeRedraw();
			}
		}
	}

	protected internal void SafeSetWindowLine(int line)
	{
		windowOriginY = line;
	}

	protected internal void SafeSetWindowChar(int ch)
	{
		windowOriginX = ch;
	}

	protected void OnVerticalScroll()
	{
		if (this.VerticalScroll != null)
		{
			this.VerticalScroll(owner, EventArgs.Empty);
		}
	}

	protected void OnHorizontalScroll()
	{
		if (this.HorizontalScroll != null)
		{
			this.HorizontalScroll(owner, EventArgs.Empty);
		}
	}

	protected internal void DoSystemScroll(int code, bool vert)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (vert)
		{
			DoVerticalScroll(OSUtils.GetScrollType(code), OSUtils.GetScrollPos(owner.Handle, flatBars, isVertical: true));
		}
		else
		{
			DoHorizontalScroll(OSUtils.GetScrollType(code), OSUtils.GetScrollPos(owner.Handle, flatBars, isVertical: false));
		}
	}

	protected internal void DoVerticalScroll(int pos)
	{
		DoVerticalScroll((ScrollEventType)4, pos);
	}

	protected void vScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		DoVerticalScroll(e.Type, e.NewValue);
	}

	protected void hScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		DoHorizontalScroll(e.Type, e.NewValue);
	}

	protected internal void DoVerticalScroll(ScrollEventType code, int pos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected I4, but got Unknown
		if (scrollUpdateCount > 0 || owner == null)
		{
			return;
		}
		switch ((int)code)
		{
		case 8:
			if (owner.WordWrap)
			{
				UpdateScroll();
			}
			if ((options & ScrollingOptions.ShowScrollHint) != 0)
			{
				owner.HideScrollHint();
			}
			break;
		case 6:
			WindowOriginY = 0;
			break;
		case 2:
			WindowOriginY -= ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Height : owner.LinesInHeight());
			break;
		case 3:
			WindowOriginY += ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Height : owner.LinesInHeight());
			break;
		case 7:
			WindowOriginY = ScrollHeight();
			break;
		case 0:
			WindowOriginY -= ((owner.Pages.PageType != PageType.PageLayout) ? 1 : owner.Painter.FontHeight);
			break;
		case 1:
			WindowOriginY += ((owner.Pages.PageType != PageType.PageLayout) ? 1 : owner.Painter.FontHeight);
			break;
		case 4:
			WindowOriginY = pos;
			break;
		case 5:
			if ((options & ScrollingOptions.SmoothScroll) != 0)
			{
				WindowOriginY = pos;
			}
			if ((options & ScrollingOptions.ShowScrollHint) != 0)
			{
				pos = (((options & ScrollingOptions.SmoothScroll) == 0) ? VScrollBar.Value : WindowOriginY);
				owner.ShowScrollHint(pos);
			}
			break;
		}
	}

	protected internal void DoHorizontalScroll(int pos)
	{
		DoHorizontalScroll((ScrollEventType)4, pos);
	}

	protected internal void DoHorizontalScroll(ScrollEventType code, int pos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected I4, but got Unknown
		if (scrollUpdateCount > 0 || owner == null)
		{
			return;
		}
		switch ((int)code)
		{
		case 8:
			break;
		case 6:
			WindowOriginX = 0;
			break;
		case 2:
			WindowOriginX -= ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Width : owner.CharsInWidth());
			break;
		case 3:
			WindowOriginX += ((owner.Pages.PageType == PageType.PageLayout) ? owner.ClientRect.Width : owner.CharsInWidth());
			break;
		case 7:
			WindowOriginX = ScrollWidth();
			break;
		case 0:
			WindowOriginX -= ((owner.Pages.PageType != PageType.PageLayout) ? 1 : owner.Painter.FontWidth);
			break;
		case 1:
			WindowOriginX += ((owner.Pages.PageType != PageType.PageLayout) ? 1 : owner.Painter.FontWidth);
			break;
		case 4:
			WindowOriginX = pos;
			break;
		case 5:
			if ((options & ScrollingOptions.SmoothScroll) != 0)
			{
				WindowOriginX = pos;
			}
			break;
		}
	}

	protected internal void RemoveScrollbars(bool system)
	{
		if (system)
		{
			if (owner.IsHandleCreated)
			{
				OSUtils.SetScrollBar(owner.Handle, flatBars, 0, -1, isVertical: false);
				OSUtils.SetScrollBar(owner.Handle, flatBars, 0, -1, isVertical: true);
			}
			return;
		}
		if (vScrollBar != null)
		{
			((Component)(object)vScrollBar).Dispose();
			vScrollBar = null;
		}
		if (hScrollBar != null)
		{
			((Component)(object)hScrollBar).Dispose();
			hScrollBar = null;
		}
		Rectangle r = Rectangle.Empty;
		UpdateScrollButtons(vertButtons, vert: true, ref r);
		UpdateScrollButtons(horzButtons, vert: false, ref r);
	}

	protected internal void SetFlatBars(bool value)
	{
		if (owner.IsHandleCreated)
		{
			if (value)
			{
				flatBars = OSUtils.InitializeFlatSB(owner.Handle);
			}
			else if (flatBars)
			{
				OSUtils.UninitializeFlatSB(owner.Handle);
				flatBars = false;
			}
		}
	}

	public int BeginUpdate()
	{
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
		return updateCount;
	}

	public void Update()
	{
		UpdateScroll(updateSize: true);
	}

	public void Assign(IScrolling source)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		BeginUpdate();
		try
		{
			ScrollBars = source.ScrollBars;
			DefaultHorzScrollSize = source.DefaultHorzScrollSize;
			WindowOriginX = source.WindowOriginX;
			WindowOriginY = source.WindowOriginY;
			Options = source.Options;
			horzButtons.Assign(source.HorzButtons);
			vertButtons.Assign(source.VertButtons);
		}
		finally
		{
			EndUpdate();
		}
	}

	public virtual void ResetDefaultHorzScrollSize()
	{
		DefaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
	}

	public virtual void ResetScrollBars()
	{
		ScrollBars = (RichTextBoxScrollBars)3;
	}

	public virtual void ResetOptions()
	{
		Options = EditConsts.DefaultScrollingOptions;
	}

	public bool ShouldSerializeOptions()
	{
		return options != EditConsts.DefaultScrollingOptions;
	}

	public bool ShouldSerializeDefaultHorzScrollSize()
	{
		return defaultHorzScrollSize != EditConsts.DefaultHorzScrollSize;
	}
}
