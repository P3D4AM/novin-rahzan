using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;
using Stimulsoft.Controls.Win.DotNetBar.Touch;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[Description("Page Slider Control with Touch Support")]
[DefaultEvent("SelectedPageChanged")]
[DefaultProperty("SelectedPageIndex")]
[ToolboxBitmap(typeof(StepIndicator), "Controls.PageSlider.ico")]
public class PageSlider : Control
{
	private StepIndicator _StepIndicator;

	private TouchHandler _TouchHandler;

	private HScrollBarAdv _HorizontalScrollBar;

	private VScrollBarAdv _VerticalScrollBar;

	private bool _IndicatorVisible = true;

	private bool _IsScrolling;

	private int _InitialScrollValue;

	private Point _PageBoundsOffset = Point.Empty;

	private List<PageSliderPage> _PageTable = new List<PageSliderPage>();

	private PageSliderPage _SelectedPage;

	private int _AnimationTime = 250;

	private int _PageCount;

	private Padding _PagePadding = new Padding(4);

	private const int DefaultPageSpacing = 48;

	private int _PageSpacing = 48;

	private const int DefaultNextPageVisibleMargin = 50;

	private int _NextPageVisibleMargin = 50;

	private eOrientation _Orientation;

	private bool _MouseDrag;

	private eScrollBarVisibility _ScrollBarVisibility = eScrollBarVisibility.AutoVisible;

	private int _AutoScrollBarVisibleTime = 3000;

	private Timer _ScrollBarTimer;

	private Point _LastMousePosition = Point.Empty;

	private bool _TouchDrag;

	private Point _TouchStartLocation = Point.Empty;

	protected override Size DefaultSize => new Size(300, 200);

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether current page indicator is visible.")]
	public bool IndicatorVisible
	{
		get
		{
			return _IndicatorVisible;
		}
		set
		{
			if (value != _IndicatorVisible)
			{
				bool indicatorVisible = _IndicatorVisible;
				_IndicatorVisible = value;
				OnIndicatorVisibleChanged(indicatorVisible, value);
			}
		}
	}

	private Point PageBoundsOffset
	{
		get
		{
			return _PageBoundsOffset;
		}
		set
		{
			if (_PageBoundsOffset != value)
			{
				_PageBoundsOffset = value;
				UpdateBoundsForAllPages(updateControl: true, syncPageIndicator: true);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates selected page.")]
	public PageSliderPage SelectedPage
	{
		get
		{
			return _SelectedPage;
		}
		set
		{
			if (value != _SelectedPage)
			{
				PageSliderPage selectedPage = _SelectedPage;
				_SelectedPage = value;
				OnSelectedPageChanged(selectedPage, value);
			}
		}
	}

	public int AnimationTime
	{
		get
		{
			return _AnimationTime;
		}
		set
		{
			if (value != _AnimationTime)
			{
				int animationTime = _AnimationTime;
				_AnimationTime = value;
				OnAnimationTimeChanged(animationTime, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedPageIndex
	{
		get
		{
			if (_SelectedPage == null)
			{
				return -1;
			}
			return _PageTable.IndexOf(_SelectedPage);
		}
		set
		{
			if (value < 0 || value >= _PageTable.Count)
			{
				throw new ArgumentOutOfRangeException("SelectedPageIndex must be greater or equal than 0 and less or equal than number of pages added to control.");
			}
			SelectedPage = _PageTable[value];
		}
	}

	[Browsable(false)]
	public int PageCount => _PageTable.Count;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Padding PagePadding
	{
		get
		{
			return _PagePadding;
		}
		set
		{
			if (value != _PagePadding)
			{
				Padding pagePadding = _PagePadding;
				_PagePadding = value;
				OnPagePaddingChanged(pagePadding, value);
			}
		}
	}

	[DefaultValue(48)]
	[Category("Appearance")]
	[Description("Indicates spacing in pixels between pages.")]
	public int PageSpacing
	{
		get
		{
			return _PageSpacing;
		}
		set
		{
			if (value != _PageSpacing)
			{
				int pageSpacing = _PageSpacing;
				_PageSpacing = value;
				OnPageSpacingChanged(pageSpacing, value);
			}
		}
	}

	[DefaultValue(50)]
	public int NextPageVisibleMargin
	{
		get
		{
			return _NextPageVisibleMargin;
		}
		set
		{
			if (value != _NextPageVisibleMargin)
			{
				int nextPageVisibleMargin = _NextPageVisibleMargin;
				_NextPageVisibleMargin = value;
				OnNextPageVisibleMarginChanged(nextPageVisibleMargin, value);
			}
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates page layout orientation.")]
	public eOrientation Orientation
	{
		get
		{
			return _Orientation;
		}
		set
		{
			if (value != _Orientation)
			{
				eOrientation orientation = _Orientation;
				_Orientation = value;
				OnOrientationChanged(orientation, value);
			}
		}
	}

	private int MaximumReversePageOffset => Math.Min(32, ((Control)this).Width / 6);

	private int TriggerPageChangeOffset => Math.Max(32, ((Control)this).Width / 5);

	[DefaultValue(eScrollBarVisibility.AutoVisible)]
	[Category("Behavior")]
	[Description("Indicates scrollbar visibility.")]
	public eScrollBarVisibility ScrollBarVisibility
	{
		get
		{
			return _ScrollBarVisibility;
		}
		set
		{
			if (value != _ScrollBarVisibility)
			{
				eScrollBarVisibility scrollBarVisibility = _ScrollBarVisibility;
				_ScrollBarVisibility = value;
				OnScrollBarVisibilityChanged(scrollBarVisibility, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int AutoScrollBarVisibleTime
	{
		get
		{
			return _AutoScrollBarVisibleTime;
		}
		set
		{
			_AutoScrollBarVisibleTime = value;
		}
	}

	[Description("Occurs when selected page has changed.")]
	public event EventHandler SelectedPageChanged;

	public PageSlider(int pageCount)
	{
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		_PageCount = pageCount;
		_StepIndicator = new StepIndicator();
		((Control)_StepIndicator).Dock = (DockStyle)1;
		((Control)this).Controls.Add((Control)(object)_StepIndicator);
		if (BarFunctions.IsWindows7 && TouchHandler.IsTouchEnabled)
		{
			_TouchHandler = new TouchHandler((Control)(object)this);
			_TouchHandler.PanBegin += TouchHandlerPanBegin;
			_TouchHandler.Pan += TouchHandlerPan;
			_TouchHandler.PanEnd += TouchHandlerPanEnd;
		}
		_HorizontalScrollBar = new HScrollBarAdv();
		((Control)_HorizontalScrollBar).Dock = (DockStyle)2;
		((Control)_HorizontalScrollBar).Height = 12;
		_HorizontalScrollBar.Scroll += new ScrollEventHandler(ScrollBarScroll);
		((Control)_HorizontalScrollBar).Visible = false;
		((Control)this).Controls.Add((Control)(object)_HorizontalScrollBar);
		_VerticalScrollBar = new VScrollBarAdv();
		((Control)_VerticalScrollBar).Dock = (DockStyle)4;
		((Control)_VerticalScrollBar).Width = 12;
		_VerticalScrollBar.Scroll += new ScrollEventHandler(ScrollBarScroll);
		((Control)_VerticalScrollBar).Visible = false;
		((Control)this).Controls.Add((Control)(object)_VerticalScrollBar);
	}

	protected override void Dispose(bool disposing)
	{
		StopScrollBarTimer();
		((Control)this).Dispose(disposing);
	}

	protected virtual void OnSelectedPageChanged(EventArgs e)
	{
		this.SelectedPageChanged?.Invoke(this, e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		UpdateBoundsForAllPages(updateControl: false);
		((Control)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		StopScrollBarTimer();
		((Control)this).OnHandleDestroyed(e);
	}

	protected virtual void OnIndicatorVisibleChanged(bool oldValue, bool newValue)
	{
		((Control)_StepIndicator).Visible = newValue;
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (e.Control is PageSliderPage)
		{
			PageSliderPage pageSliderPage = (PageSliderPage)(object)e.Control;
			UpdatePageTable();
			((Control)pageSliderPage).Bounds = GetPageBounds(pageSliderPage);
			UpdateScrollBar();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control is PageSliderPage)
		{
			PageSliderPage pageSliderPage = (PageSliderPage)(object)e.Control;
			int num = _PageTable.IndexOf(pageSliderPage);
			_PageTable.Remove(pageSliderPage);
			UpdatePageTable();
			if (_SelectedPage == pageSliderPage)
			{
				if (_PageTable.Count > 0)
				{
					SelectedPage = _PageTable[(num > 0) ? num-- : 0];
				}
				else
				{
					SelectedPage = null;
				}
			}
			UpdateScrollBar();
		}
		((Control)this).OnControlRemoved(e);
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		((Control)this).OnLayout(levent);
		if (levent.AffectedProperty == "ChildIndex")
		{
			UpdatePageTable();
			UpdateBoundsForAllPages(updateControl: true, syncPageIndicator: true);
		}
	}

	private void UpdateBoundsForAllPages(bool updateControl)
	{
		UpdateBoundsForAllPages(updateControl, syncPageIndicator: false);
	}

	private void UpdateBoundsForAllPages(bool updateControl, bool syncPageIndicator)
	{
		Rectangle selectedPageBounds = GetSelectedPageBounds();
		int num = -1;
		for (int i = 0; i < _PageTable.Count; i++)
		{
			PageSliderPage pageSliderPage = _PageTable[i];
			((Control)pageSliderPage).Bounds = GetPageBounds(pageSliderPage);
			if (updateControl && ((Control)pageSliderPage).Bounds.IntersectsWith(((Control)this).ClientRectangle))
			{
				((Control)this).Update();
			}
			if (syncPageIndicator && !_PageBoundsOffset.IsEmpty && ((Control)pageSliderPage).Bounds.IntersectsWith(selectedPageBounds) && num == -1)
			{
				num = i + 1;
			}
		}
		if (syncPageIndicator && _PageBoundsOffset.IsEmpty)
		{
			num = SelectedPageIndex + 1;
		}
		if (syncPageIndicator && num > 0)
		{
			_StepIndicator.CurrentStep = num;
		}
		UpdateScrollBar();
	}

	private void UpdateScrollBar()
	{
		int selectedPageIndex = SelectedPageIndex;
		Rectangle selectedPageBounds = GetSelectedPageBounds();
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = Rectangle.Empty;
		if (_PageTable.Count > 0)
		{
			rectangle = ((Control)_PageTable[0]).Bounds;
			rectangle2 = ((Control)_PageTable[_PageTable.Count - 1]).Bounds;
		}
		if (_Orientation == eOrientation.Horizontal)
		{
			_HorizontalScrollBar.Minimum = 0;
			_HorizontalScrollBar.Maximum = Math.Max(1, rectangle2.Right - rectangle.X + ((selectedPageIndex == -1) ? rectangle.X : 0));
			_HorizontalScrollBar.LargeChange = Math.Max(1, selectedPageBounds.Width);
			_HorizontalScrollBar.SmallChange = 32;
			if (!_IsScrolling && selectedPageIndex != -1)
			{
				_HorizontalScrollBar.Value = Math.Min(Math.Max(_HorizontalScrollBar.Minimum, Math.Abs(rectangle.X)), _HorizontalScrollBar.Maximum);
			}
		}
		else
		{
			_VerticalScrollBar.Minimum = 0;
			_VerticalScrollBar.Maximum = Math.Max(1, rectangle2.Bottom - rectangle.Y + ((selectedPageIndex == -1) ? rectangle.Y : 0));
			_VerticalScrollBar.LargeChange = Math.Max(1, selectedPageBounds.Height);
			_VerticalScrollBar.SmallChange = 32;
			if (!_IsScrolling && selectedPageIndex != -1)
			{
				_VerticalScrollBar.Value = Math.Min(Math.Max(_VerticalScrollBar.Minimum, Math.Abs(rectangle.Y)), _VerticalScrollBar.Maximum);
			}
		}
	}

	private void ScrollBarScroll(object sender, ScrollEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Type == 8)
		{
			_IsScrolling = false;
			Rectangle selectedPageBounds = GetSelectedPageBounds();
			PageSliderPage pageSliderPage = SelectedPage;
			if (e.NewValue > _InitialScrollValue)
			{
				for (int num = _PageTable.Count - 1; num >= 0; num--)
				{
					PageSliderPage pageSliderPage2 = _PageTable[num];
					if (((Control)pageSliderPage2).Bounds.IntersectsWith(selectedPageBounds) && selectedPageBounds.Right - ((Control)pageSliderPage2).Left >= TriggerPageChangeOffset)
					{
						pageSliderPage = pageSliderPage2;
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < _PageTable.Count; i++)
				{
					PageSliderPage pageSliderPage3 = _PageTable[i];
					if (((Control)pageSliderPage3).Bounds.IntersectsWith(selectedPageBounds) && ((Control)pageSliderPage3).Right - selectedPageBounds.X >= TriggerPageChangeOffset)
					{
						pageSliderPage = pageSliderPage3;
						break;
					}
				}
			}
			if (SelectedPage != pageSliderPage)
			{
				SelectedPage = pageSliderPage;
				if (_Orientation == eOrientation.Horizontal)
				{
					_HorizontalScrollBar.Value = Math.Abs(((Control)_PageTable[0]).Left);
				}
				else
				{
					_VerticalScrollBar.Value = Math.Abs(((Control)_PageTable[0]).Top);
				}
			}
			else
			{
				PageBoundsOffset = Point.Empty;
			}
			return;
		}
		if (!_IsScrolling)
		{
			if (_Orientation == eOrientation.Horizontal)
			{
				_InitialScrollValue = _HorizontalScrollBar.Value;
			}
			else
			{
				_InitialScrollValue = _VerticalScrollBar.Value;
			}
			_IsScrolling = true;
		}
		if ((int)e.ScrollOrientation == 0)
		{
			PageBoundsOffset = new Point(_InitialScrollValue - e.NewValue, 0);
		}
		else
		{
			PageBoundsOffset = new Point(0, _InitialScrollValue - e.NewValue);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		UpdateBoundsForAllPages(updateControl: false);
		((Control)this).OnResize(e);
	}

	private Rectangle GetSelectedPageBounds()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Invalid comparison between Unknown and I4
		Rectangle result = DeflateRectangle(((Control)this).ClientRectangle, _PagePadding);
		if (_Orientation == eOrientation.Horizontal)
		{
			result.Width -= _PageSpacing + _NextPageVisibleMargin;
		}
		else
		{
			result.Height -= _PageSpacing + _NextPageVisibleMargin;
		}
		if (_IndicatorVisible)
		{
			if ((int)((Control)_StepIndicator).Dock == 1)
			{
				result.Y += ((Control)_StepIndicator).Height;
				result.Height -= ((Control)_StepIndicator).Height;
			}
			else if ((int)((Control)_StepIndicator).Dock == 2)
			{
				result.Height -= ((Control)_StepIndicator).Height;
			}
			else if ((int)((Control)_StepIndicator).Dock == 3)
			{
				result.X += ((Control)_StepIndicator).Width;
				result.Width -= ((Control)_StepIndicator).Width;
			}
			else if ((int)((Control)_StepIndicator).Dock == 4)
			{
				result.Width -= ((Control)_StepIndicator).Width;
			}
		}
		if (_ScrollBarVisibility == eScrollBarVisibility.AlwaysVisible)
		{
			if (_Orientation == eOrientation.Horizontal)
			{
				result.Height -= ((Control)_HorizontalScrollBar).Height;
			}
			else
			{
				result.Width -= ((Control)_HorizontalScrollBar).Width;
			}
		}
		return result;
	}

	private Rectangle GetPageBounds(PageSliderPage page)
	{
		int num = _PageTable.IndexOf(page);
		int selectedPageIndex = SelectedPageIndex;
		Rectangle empty = Rectangle.Empty;
		Rectangle selectedPageBounds = GetSelectedPageBounds();
		if (num == selectedPageIndex)
		{
			empty = selectedPageBounds;
		}
		else if (num > selectedPageIndex)
		{
			empty = selectedPageBounds;
			if (_Orientation == eOrientation.Horizontal)
			{
				empty.X += (_PageSpacing + empty.Width) * (num - selectedPageIndex);
			}
			else
			{
				empty.Y += (_PageSpacing + empty.Height) * (num - selectedPageIndex);
			}
		}
		else
		{
			empty = selectedPageBounds;
			if (_Orientation == eOrientation.Horizontal)
			{
				empty.X -= (_PageSpacing + empty.Width) * (selectedPageIndex - num);
			}
			else
			{
				empty.Y -= (_PageSpacing + empty.Height) * (selectedPageIndex - num);
			}
		}
		empty.Offset(_PageBoundsOffset);
		return empty;
	}

	private Rectangle DeflateRectangle(Rectangle rect, Padding padding)
	{
		rect.X += padding.Left;
		rect.Y += padding.Top;
		rect.Width -= padding.Horizontal;
		rect.Height -= padding.Vertical;
		return rect;
	}

	private void UpdatePageTable()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		_PageTable.Clear();
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is PageSliderPage pageSliderPage))
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < _PageTable.Count; i++)
			{
				if (_PageTable[i].PageNumber > pageSliderPage.PageNumber)
				{
					_PageTable.Insert(i, pageSliderPage);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				_PageTable.Add(pageSliderPage);
			}
		}
		_StepIndicator.StepCount = _PageTable.Count;
		_StepIndicator.CurrentStep = SelectedPageIndex + 1;
	}

	protected virtual void OnSelectedPageChanged(PageSliderPage oldValue, PageSliderPage newValue)
	{
		if (_AnimationTime > 0)
		{
			AnimatePageTransition(oldValue, newValue);
		}
		else
		{
			UpdateBoundsForAllPages(updateControl: false);
		}
		UpdateScrollBar();
		_StepIndicator.CurrentStep = SelectedPageIndex + 1;
		OnSelectedPageChanged(EventArgs.Empty);
	}

	private void AnimatePageTransition(PageSliderPage oldValue, PageSliderPage newValue)
	{
		int num = -1;
		if (oldValue != null)
		{
			num = _PageTable.IndexOf(oldValue);
		}
		int num2 = -1;
		if (newValue != null)
		{
			num2 = _PageTable.IndexOf(newValue);
		}
		if (num == num2)
		{
			UpdateBoundsForAllPages(updateControl: false);
			_PageBoundsOffset = Point.Empty;
			return;
		}
		int num3 = 0;
		Rectangle selectedPageBounds = GetSelectedPageBounds();
		num3 = ((_Orientation != 0) ? ((num - num2) * (selectedPageBounds.Height + _PageSpacing) - _PageBoundsOffset.Y) : ((num - num2) * (selectedPageBounds.Width + _PageSpacing) - _PageBoundsOffset.X));
		_PageBoundsOffset = Point.Empty;
		DateTime now = DateTime.Now;
		int num4 = 1;
		int num5 = Math.Abs(num3);
		int num6 = Math.Sign(num3);
		int num7 = _AnimationTime;
		if ((_Orientation == eOrientation.Horizontal && (double)num5 < (double)selectedPageBounds.Width * 0.8) || (_Orientation == eOrientation.Vertical && (double)num5 < (double)selectedPageBounds.Height * 0.8))
		{
			num7 = ((_Orientation != 0) ? (num7 * (num5 / selectedPageBounds.Height)) : (num7 * (num5 / selectedPageBounds.Width)));
			num7 = Math.Max(num7, 50);
		}
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, num7);
		while (num5 > 0)
		{
			DateTime now2 = DateTime.Now;
			foreach (PageSliderPage item in _PageTable)
			{
				Rectangle bounds = ((Control)item).Bounds;
				if (_Orientation == eOrientation.Horizontal)
				{
					bounds.Offset(num4 * num6, 0);
				}
				else
				{
					bounds.Offset(0, num4 * num6);
				}
				((Control)item).Bounds = bounds;
				if (bounds.IntersectsWith(((Control)this).ClientRectangle))
				{
					((Control)this).Update();
				}
			}
			num5 -= num4;
			TimeSpan timeSpan2 = DateTime.Now - now2;
			TimeSpan timeSpan3 = DateTime.Now - now;
			num4 = (((timeSpan - timeSpan3).TotalMilliseconds <= 0.0) ? num5 : (((int)(timeSpan - timeSpan3).TotalMilliseconds == 0) ? 1 : (num5 * (int)timeSpan2.TotalMilliseconds / Math.Max(1, (int)(timeSpan - timeSpan3).TotalMilliseconds))));
			if (num4 > num5)
			{
				num4 = num5;
			}
		}
	}

	protected virtual void OnAnimationTimeChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnPagePaddingChanged(Padding oldValue, Padding newValue)
	{
		UpdateBoundsForAllPages(updateControl: false);
	}

	protected virtual void OnPageSpacingChanged(int oldValue, int newValue)
	{
		UpdateBoundsForAllPages(updateControl: false);
	}

	protected virtual void OnNextPageVisibleMarginChanged(int oldValue, int newValue)
	{
		UpdateBoundsForAllPages(updateControl: false);
	}

	protected virtual void OnOrientationChanged(eOrientation oldValue, eOrientation newValue)
	{
		UpdateBoundsForAllPages(updateControl: false);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 793)
		{
			switch (WinApi.GetAppCommandLParam(((Message)(ref m)).LParam))
			{
			case 1:
				if (SelectedPageIndex > 0)
				{
					SelectedPageIndex--;
				}
				((Message)(ref m)).Result = new IntPtr(1);
				break;
			case 2:
				if (SelectedPageIndex < _PageTable.Count - 1)
				{
					SelectedPageIndex++;
				}
				((Message)(ref m)).Result = new IntPtr(1);
				break;
			}
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && _TouchHandler == null && _PageTable.Count > 0)
		{
			_MouseDrag = true;
			_TouchStartLocation = e.Location;
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_ScrollBarVisibility == eScrollBarVisibility.AutoVisible && !((Control)_HorizontalScrollBar).Visible && !((Control)_VerticalScrollBar).Visible)
		{
			StartScrollBarTimer();
		}
		if (_MouseDrag)
		{
			Rectangle selectedPageBounds = GetSelectedPageBounds();
			if (_Orientation == eOrientation.Horizontal)
			{
				int num = e.Location.X - _TouchStartLocation.X;
				int num2 = num - _PageBoundsOffset.X;
				Rectangle pageBounds = GetPageBounds(_PageTable[0]);
				if (((Control)_PageTable[0]).Left + num2 > selectedPageBounds.X + MaximumReversePageOffset && SelectedPageIndex >= 0)
				{
					num -= ((Control)_PageTable[0]).Left + num2 - (selectedPageBounds.X + MaximumReversePageOffset);
				}
				else if (((Control)_PageTable[_PageTable.Count - 1]).Left + num2 < selectedPageBounds.X - MaximumReversePageOffset)
				{
					num += selectedPageBounds.X - MaximumReversePageOffset - (((Control)_PageTable[_PageTable.Count - 1]).Left + num2);
				}
				PageBoundsOffset = new Point(num, 0);
			}
			else
			{
				int num3 = e.Location.Y - _TouchStartLocation.Y;
				int num4 = num3 - _PageBoundsOffset.Y;
				Rectangle pageBounds2 = GetPageBounds(_PageTable[0]);
				if (((Control)_PageTable[0]).Top + num4 > selectedPageBounds.Y + MaximumReversePageOffset && SelectedPageIndex >= 0)
				{
					num3 -= ((Control)_PageTable[0]).Top + num4 - (selectedPageBounds.Y + MaximumReversePageOffset);
				}
				else if (((Control)_PageTable[_PageTable.Count - 1]).Top + num4 < selectedPageBounds.Y - MaximumReversePageOffset)
				{
					num3 += selectedPageBounds.Y - MaximumReversePageOffset - (((Control)_PageTable[_PageTable.Count - 1]).Top + num4);
				}
				PageBoundsOffset = new Point(0, num3);
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_MouseDrag)
		{
			_MouseDrag = false;
			int num = ((_Orientation == eOrientation.Horizontal) ? _PageBoundsOffset.X : _PageBoundsOffset.Y);
			if (Math.Abs(num) >= TriggerPageChangeOffset)
			{
				Rectangle selectedPageBounds = GetSelectedPageBounds();
				if (num < 0)
				{
					if (SelectedPageIndex < _PageTable.Count - 1)
					{
						PageSliderPage pageSliderPage = _PageTable[SelectedPageIndex + 1];
						for (int num2 = _PageTable.Count - 1; num2 >= 0; num2--)
						{
							PageSliderPage pageSliderPage2 = _PageTable[num2];
							if (((Control)pageSliderPage2).Bounds.IntersectsWith(selectedPageBounds) && selectedPageBounds.Right - ((Control)pageSliderPage2).Left >= TriggerPageChangeOffset)
							{
								pageSliderPage = pageSliderPage2;
								break;
							}
						}
						if (SelectedPage != pageSliderPage)
						{
							SelectedPage = pageSliderPage;
						}
						else
						{
							PageBoundsOffset = Point.Empty;
						}
					}
					else
					{
						PageBoundsOffset = Point.Empty;
					}
				}
				else if (SelectedPageIndex > 0)
				{
					PageSliderPage pageSliderPage3 = _PageTable[SelectedPageIndex - 1];
					for (int i = 0; i < _PageTable.Count; i++)
					{
						PageSliderPage pageSliderPage4 = _PageTable[i];
						if (((Control)pageSliderPage4).Bounds.IntersectsWith(selectedPageBounds) && ((Control)pageSliderPage4).Right - selectedPageBounds.X >= TriggerPageChangeOffset)
						{
							pageSliderPage3 = pageSliderPage4;
							break;
						}
					}
					if (SelectedPage != pageSliderPage3)
					{
						SelectedPage = pageSliderPage3;
					}
					else
					{
						PageBoundsOffset = Point.Empty;
					}
				}
				else
				{
					PageBoundsOffset = Point.Empty;
				}
			}
			else
			{
				PageBoundsOffset = Point.Empty;
			}
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (_ScrollBarVisibility == eScrollBarVisibility.AutoVisible && ((Control)this).Visible)
		{
			StartScrollBarTimer();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected virtual void OnScrollBarVisibilityChanged(eScrollBarVisibility oldValue, eScrollBarVisibility newValue)
	{
		UpdateScrollBarVisibility();
	}

	private void UpdateScrollBarVisibility()
	{
		if (_ScrollBarVisibility == eScrollBarVisibility.Hidden)
		{
			StopScrollBarTimer();
			((Control)_HorizontalScrollBar).Visible = false;
			((Control)_VerticalScrollBar).Visible = false;
			UpdateBoundsForAllPages(updateControl: false);
		}
		else if (_ScrollBarVisibility == eScrollBarVisibility.AlwaysVisible)
		{
			StopScrollBarTimer();
			if (_Orientation == eOrientation.Horizontal)
			{
				if (!((Control)_HorizontalScrollBar).Visible)
				{
					((Control)_HorizontalScrollBar).Visible = true;
					UpdateBoundsForAllPages(updateControl: false);
				}
				((Control)_VerticalScrollBar).Visible = false;
			}
			else
			{
				((Control)_HorizontalScrollBar).Visible = false;
				if (!((Control)_VerticalScrollBar).Visible)
				{
					((Control)_VerticalScrollBar).Visible = true;
					UpdateBoundsForAllPages(updateControl: false);
				}
			}
		}
		else
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (_ScrollBarTimer == null && ((Control)this).ClientRectangle.Contains(pt))
			{
				StartScrollBarTimer();
			}
		}
	}

	internal void MouseEnterInternal()
	{
		if (_ScrollBarVisibility == eScrollBarVisibility.AutoVisible)
		{
			StartScrollBarTimer();
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		MouseEnterInternal();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		MouseLeaveInternal();
		((Control)this).OnMouseLeave(e);
	}

	internal void MouseLeaveInternal()
	{
		if (_ScrollBarVisibility == eScrollBarVisibility.AutoVisible)
		{
			StartScrollBarTimer();
		}
	}

	private void StartScrollBarTimer()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		if (_Orientation == eOrientation.Horizontal)
		{
			((Control)_HorizontalScrollBar).Visible = true;
			((Control)_VerticalScrollBar).Visible = false;
		}
		else
		{
			((Control)_HorizontalScrollBar).Visible = false;
			((Control)_VerticalScrollBar).Visible = true;
		}
		if (_ScrollBarTimer == null)
		{
			_ScrollBarTimer = new Timer();
			_ScrollBarTimer.Tick += ScrollBarTimerTick;
			_ScrollBarTimer.Interval = _AutoScrollBarVisibleTime;
		}
		_ScrollBarTimer.Stop();
		_ScrollBarTimer.Start();
	}

	private void ScrollBarTimerTick(object sender, EventArgs e)
	{
		bool flag = false;
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(point))
		{
			if (!_LastMousePosition.IsEmpty && _LastMousePosition == point && ((!((Control)_HorizontalScrollBar).Bounds.Contains(point) && ((Control)_HorizontalScrollBar).Visible) || (!((Control)_VerticalScrollBar).Bounds.Contains(point) && ((Control)_VerticalScrollBar).Visible)))
			{
				flag = true;
			}
		}
		else if (!_LastMousePosition.IsEmpty)
		{
			flag = true;
		}
		_LastMousePosition = point;
		if (flag)
		{
			StopScrollBarTimer();
			((Control)_HorizontalScrollBar).Visible = false;
			((Control)_VerticalScrollBar).Visible = false;
			_LastMousePosition = Point.Empty;
		}
	}

	private void StopScrollBarTimer()
	{
		Timer scrollBarTimer = _ScrollBarTimer;
		_ScrollBarTimer = null;
		if (scrollBarTimer != null)
		{
			scrollBarTimer.Tick -= ScrollBarTimerTick;
			scrollBarTimer.Stop();
			((Component)(object)scrollBarTimer).Dispose();
		}
	}

	private void TouchHandlerPanBegin(object sender, GestureEventArgs e)
	{
		_TouchDrag = true;
		_TouchStartLocation = e.Location;
		e.Handled = true;
	}

	private void TouchHandlerPanEnd(object sender, GestureEventArgs e)
	{
		if (_TouchDrag)
		{
			EndTouchPan();
			e.Handled = true;
		}
	}

	private void EndTouchPan()
	{
		_TouchDrag = false;
		int num = ((_Orientation == eOrientation.Horizontal) ? _PageBoundsOffset.X : _PageBoundsOffset.Y);
		if (Math.Abs(num) >= TriggerPageChangeOffset)
		{
			Rectangle selectedPageBounds = GetSelectedPageBounds();
			if (num < 0)
			{
				if (SelectedPageIndex < _PageTable.Count - 1)
				{
					PageSliderPage pageSliderPage = _PageTable[SelectedPageIndex + 1];
					for (int num2 = _PageTable.Count - 1; num2 >= 0; num2--)
					{
						PageSliderPage pageSliderPage2 = _PageTable[num2];
						if (((Control)pageSliderPage2).Bounds.IntersectsWith(selectedPageBounds) && selectedPageBounds.Right - ((Control)pageSliderPage2).Left >= TriggerPageChangeOffset)
						{
							pageSliderPage = pageSliderPage2;
							break;
						}
					}
					if (SelectedPage != pageSliderPage)
					{
						SelectedPage = pageSliderPage;
					}
					else
					{
						PageBoundsOffset = Point.Empty;
					}
				}
				else
				{
					PageBoundsOffset = Point.Empty;
				}
			}
			else if (SelectedPageIndex > 0)
			{
				PageSliderPage pageSliderPage3 = _PageTable[SelectedPageIndex - 1];
				for (int i = 0; i < _PageTable.Count; i++)
				{
					PageSliderPage pageSliderPage4 = _PageTable[i];
					if (((Control)pageSliderPage4).Bounds.IntersectsWith(selectedPageBounds) && ((Control)pageSliderPage4).Right - selectedPageBounds.X >= TriggerPageChangeOffset)
					{
						pageSliderPage3 = pageSliderPage4;
						break;
					}
				}
				if (SelectedPage != pageSliderPage3)
				{
					SelectedPage = pageSliderPage3;
				}
				else
				{
					PageBoundsOffset = Point.Empty;
				}
			}
			else
			{
				PageBoundsOffset = Point.Empty;
			}
		}
		else
		{
			PageBoundsOffset = Point.Empty;
		}
	}

	private void TouchHandlerPan(object sender, GestureEventArgs e)
	{
		if (!_TouchDrag)
		{
			return;
		}
		Rectangle selectedPageBounds = GetSelectedPageBounds();
		if (_Orientation == eOrientation.Horizontal)
		{
			int num = e.Location.X - _TouchStartLocation.X;
			int num2 = num - _PageBoundsOffset.X;
			Rectangle pageBounds = GetPageBounds(_PageTable[0]);
			bool flag = false;
			if (((Control)_PageTable[0]).Left + num2 > selectedPageBounds.X + MaximumReversePageOffset && SelectedPageIndex >= 0)
			{
				num -= ((Control)_PageTable[0]).Left + num2 - (selectedPageBounds.X + MaximumReversePageOffset);
				flag = true;
			}
			else if (((Control)_PageTable[_PageTable.Count - 1]).Left + num2 < selectedPageBounds.X - MaximumReversePageOffset)
			{
				num += selectedPageBounds.X - MaximumReversePageOffset - (((Control)_PageTable[_PageTable.Count - 1]).Left + num2);
				flag = true;
			}
			PageBoundsOffset = new Point(num, 0);
			if (flag && e.IsInertia)
			{
				EndTouchPan();
			}
		}
		else
		{
			int num3 = e.Location.Y - _TouchStartLocation.Y;
			int num4 = num3 - _PageBoundsOffset.Y;
			Rectangle pageBounds2 = GetPageBounds(_PageTable[0]);
			bool flag2 = false;
			if (((Control)_PageTable[0]).Top + num4 > selectedPageBounds.Y + MaximumReversePageOffset && SelectedPageIndex >= 0)
			{
				num3 -= ((Control)_PageTable[0]).Top + num4 - (selectedPageBounds.Y + MaximumReversePageOffset);
				flag2 = true;
			}
			else if (((Control)_PageTable[_PageTable.Count - 1]).Top + num4 < selectedPageBounds.Y - MaximumReversePageOffset)
			{
				num3 += selectedPageBounds.Y - MaximumReversePageOffset - (((Control)_PageTable[_PageTable.Count - 1]).Top + num4);
				flag2 = true;
			}
			PageBoundsOffset = new Point(0, num3);
			if (flag2 && e.IsInertia)
			{
				EndTouchPan();
			}
		}
		e.Handled = true;
	}
}
