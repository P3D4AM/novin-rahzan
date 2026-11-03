using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.ScrollBar;
using DevComponents.DotNetBar.Touch;

namespace DevComponents.DotNetBar.Controls;

[DefaultProperty("SelectedPageIndex")]
[ToolboxBitmap(typeof(StepIndicator), "Controls.PageSlider.ico")]
[Description("Page Slider Control with Touch Support")]
[DefaultEvent("SelectedPageChanged")]
[Designer("DevComponents.DotNetBar.Design.PageSliderDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class PageSlider : Control
{
	private const int _286A = 48;

	private const int _286B = 50;

	private StepIndicator __286C;

	private TouchHandler _ظ;

	private HScrollBarAdv __286D;

	private VScrollBarAdv __286E;

	private EventHandler _286F;

	private bool __2870 = true;

	private bool __2871;

	private int __2872;

	private Point __2873 = Point.Empty;

	private List<PageSliderPage> __2874 = new List<PageSliderPage>();

	private PageSliderPage __2875;

	private int __20D7 = 250;

	private int __2876;

	private Padding __2877 = new Padding(4);

	private int __2878 = 48;

	private int __2879 = 50;

	private eOrientation _ᡪ;

	private bool __287A;

	private bool __287B = true;

	private bool __287C = true;

	private eScrollBarVisibility __287D = eScrollBarVisibility.AutoVisible;

	private bool __287E;

	private int __287F = 3000;

	private Timer __2880;

	private Point __2881 = Point.Empty;

	private bool _غ;

	private Point _ػ = Point.Empty;

	private eTouchHandling _ع;

	protected override Size DefaultSize => new Size(300, 200);

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether current page indicator is visible.")]
	public bool IndicatorVisible
	{
		get
		{
			return __2870;
		}
		set
		{
			if (value != __2870)
			{
				bool oldValue = __2870;
				__2870 = value;
				OnIndicatorVisibleChanged(oldValue, value);
			}
		}
	}

	private Point _2869
	{
		get
		{
			return __2873;
		}
		set
		{
			if (__2873 != value)
			{
				__2873 = value;
				_2882(_2883: true, _2884: true);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates selected page.")]
	public PageSliderPage SelectedPage
	{
		get
		{
			return __2875;
		}
		set
		{
			if (value != __2875)
			{
				PageSliderPage oldValue = __2875;
				__2875 = value;
				OnSelectedPageChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the animation time in milliseconds for operations that perform visual animation of transition. Set to zero to disable animation.")]
	[Category("Behavior")]
	[DefaultValue(250)]
	public int AnimationTime
	{
		get
		{
			return __20D7;
		}
		set
		{
			if (value != __20D7)
			{
				int oldValue = __20D7;
				__20D7 = value;
				OnAnimationTimeChanged(oldValue, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectedPageIndex
	{
		get
		{
			if (__2875 == null)
			{
				return -1;
			}
			return __2874.IndexOf(__2875);
		}
		set
		{
			if (value < 0 || value >= __2874.Count)
			{
				throw new ArgumentOutOfRangeException("SelectedPageIndex must be greater or equal than 0 and less or equal than number of pages added to control.");
			}
			SelectedPage = __2874[value];
		}
	}

	[Browsable(false)]
	public int PageCount => __2874.Count;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Padding PagePadding
	{
		get
		{
			return __2877;
		}
		set
		{
			if (value != __2877)
			{
				Padding oldValue = __2877;
				__2877 = value;
				OnPagePaddingChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates spacing in pixels between pages.")]
	[Category("Appearance")]
	[DefaultValue(48)]
	public int PageSpacing
	{
		get
		{
			return __2878;
		}
		set
		{
			if (value != __2878)
			{
				int oldValue = __2878;
				__2878 = value;
				OnPageSpacingChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(50)]
	public int NextPageVisibleMargin
	{
		get
		{
			return __2879;
		}
		set
		{
			if (value != __2879)
			{
				int oldValue = __2879;
				__2879 = value;
				OnNextPageVisibleMarginChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates page layout orientation.")]
	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	public eOrientation Orientation
	{
		get
		{
			return _ᡪ;
		}
		set
		{
			if (value != _ᡪ)
			{
				eOrientation ᡪ = _ᡪ;
				_ᡪ = value;
				OnOrientationChanged(ᡪ, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether page can be dragged using mouse to change currently selected page")]
	public bool PageMouseDragEnabled
	{
		get
		{
			return __287B;
		}
		set
		{
			__287B = value;
		}
	}

	[Description("Indicates whether selected page can be changed by using mouse drag on PageSlider client area which is not covered by pages.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool MouseDragEnabled
	{
		get
		{
			return __287C;
		}
		set
		{
			__287C = value;
		}
	}

	private int Ն => Math.Min(32, ((Control)this).Width / 6);

	private int Յ => Math.Max(32, ((Control)this).Width / 5);

	[Category("Behavior")]
	[Description("Indicates scrollbar visibility.")]
	[DefaultValue(eScrollBarVisibility.AutoVisible)]
	public eScrollBarVisibility ScrollBarVisibility
	{
		get
		{
			return __287D;
		}
		set
		{
			if (value != __287D)
			{
				eScrollBarVisibility oldValue = __287D;
				__287D = value;
				OnScrollBarVisibilityChanged(oldValue, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int AutoScrollBarVisibleTime
	{
		get
		{
			return __287F;
		}
		set
		{
			__287F = value;
		}
	}

	[Description("Indicates whether native touch support in control is enabled if available on target system.")]
	[DefaultValue(eTouchHandling.Yes)]
	[Category("Behavior")]
	public eTouchHandling TouchEnabled
	{
		get
		{
			return _ع;
		}
		set
		{
			if (value != _ع)
			{
				eTouchHandling ع = _ع;
				_ع = value;
				OnTouchEnabledChanged(ع, value);
			}
		}
	}

	[Description("Occurs when selected page has changed.")]
	public event EventHandler SelectedPageChanged
	{
		add
		{
			EventHandler eventHandler = _286F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _286F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _286F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _286F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public PageSlider()
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		__286C = new StepIndicator();
		((Control)__286C).Dock = (DockStyle)1;
		((Control)this).Controls.Add((Control)(object)__286C);
		if (BarFunctions.IsWindows7 && TouchHandler.IsTouchEnabled)
		{
			_ظ = new TouchHandler((Control)(object)this);
			_ظ.PanBegin += _۸;
			_ظ.Pan += ۻ;
			_ظ.PanEnd += _۹;
		}
		__286D = new HScrollBarAdv();
		((Control)__286D).Dock = (DockStyle)2;
		((Control)__286D).Height = 12;
		__286D.Scroll += new ScrollEventHandler(_2886);
		((Control)__286D).Visible = false;
		((Control)this).Controls.Add((Control)(object)__286D);
		__286E = new VScrollBarAdv();
		((Control)__286E).Dock = (DockStyle)4;
		((Control)__286E).Width = 12;
		__286E.Scroll += new ScrollEventHandler(_2886);
		((Control)__286E).Visible = false;
		((Control)this).Controls.Add((Control)(object)__286E);
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		_2894();
		__286D.Scroll -= new ScrollEventHandler(_2886);
		__286E.Scroll -= new ScrollEventHandler(_2886);
		((Component)(object)__286D).Dispose();
		((Component)(object)__286E).Dispose();
		__286E = null;
		__286D = null;
		if (_ظ != null)
		{
			_ظ.PanBegin -= _۸;
			_ظ.Pan -= ۻ;
			_ظ.PanEnd -= _۹;
			_ظ.Release();
		}
		((Control)this).Dispose(disposing);
	}

	protected virtual void OnSelectedPageChanged(EventArgs e)
	{
		_286F?.Invoke(this, e);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((Control)this).ScaleControl(factor, specified);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (__287E)
		{
			__287E = false;
			_288F();
		}
		_2882(_2883: false);
		((Control)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_2894();
		((Control)this).OnHandleDestroyed(e);
	}

	protected virtual void OnIndicatorVisibleChanged(bool oldValue, bool newValue)
	{
		((Control)__286C).Visible = newValue;
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (e.Control is PageSliderPage)
		{
			PageSliderPage pageSliderPage = (PageSliderPage)(object)e.Control;
			_288C();
			((Control)pageSliderPage).Bounds = _2888(pageSliderPage);
			_2885();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control is PageSliderPage)
		{
			PageSliderPage pageSliderPage = (PageSliderPage)(object)e.Control;
			int num = __2874.IndexOf(pageSliderPage);
			__2874.Remove(pageSliderPage);
			_288C();
			if (__2875 == pageSliderPage)
			{
				if (__2874.Count > 0)
				{
					SelectedPage = __2874[(num > 0) ? num-- : 0];
				}
				else
				{
					SelectedPage = null;
				}
			}
			_2885();
			_2882(_2883: false, _2884: true);
		}
		((Control)this).OnControlRemoved(e);
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		((Control)this).OnLayout(levent);
		if (levent.AffectedProperty == "ChildIndex")
		{
			_288C();
			_2882(_2883: true, _2884: true);
		}
	}

	private void _2882(bool _2883)
	{
		_2882(_2883, _2884: false);
	}

	private void _2882(bool _2883, bool _2884)
	{
		Rectangle rect = _2887();
		int num = -1;
		for (int i = 0; i < __2874.Count; i++)
		{
			PageSliderPage pageSliderPage = __2874[i];
			((Control)pageSliderPage).Bounds = _2888(pageSliderPage);
			if (_2883 && ((Control)pageSliderPage).Bounds.IntersectsWith(((Control)this).ClientRectangle))
			{
				((Control)this).Update();
			}
			if (_2884 && !__2873.IsEmpty && ((Control)pageSliderPage).Bounds.IntersectsWith(rect) && num == -1)
			{
				num = i + 1;
			}
		}
		if (_2884 && __2873.IsEmpty)
		{
			num = SelectedPageIndex + 1;
		}
		if (_2884 && num > 0)
		{
			__286C.CurrentStep = num;
		}
		_2885();
	}

	private void _2885()
	{
		int selectedPageIndex = SelectedPageIndex;
		Rectangle rectangle = _2887();
		Rectangle rectangle2 = Rectangle.Empty;
		Rectangle rectangle3 = Rectangle.Empty;
		if (__2874.Count > 0)
		{
			rectangle2 = ((Control)__2874[0]).Bounds;
			rectangle3 = ((Control)__2874[__2874.Count - 1]).Bounds;
		}
		if (_ᡪ == eOrientation.Horizontal)
		{
			__286D.Minimum = 0;
			__286D.Maximum = Math.Max(1, rectangle3.Right - rectangle2.X + ((selectedPageIndex == -1) ? rectangle2.X : 0));
			__286D.LargeChange = Math.Max(1, rectangle.Width);
			__286D.SmallChange = 32;
			if (!__2871 && selectedPageIndex != -1)
			{
				__286D.Value = Math.Min(Math.Max(__286D.Minimum, Math.Abs(rectangle2.X)), __286D.Maximum);
			}
		}
		else
		{
			__286E.Minimum = 0;
			__286E.Maximum = Math.Max(1, rectangle3.Bottom - rectangle2.Y + ((selectedPageIndex == -1) ? rectangle2.Y : 0));
			__286E.LargeChange = Math.Max(1, rectangle.Height);
			__286E.SmallChange = 32;
			if (!__2871 && selectedPageIndex != -1)
			{
				__286E.Value = Math.Min(Math.Max(__286E.Minimum, Math.Abs(rectangle2.Y)), __286E.Maximum);
			}
		}
	}

	private void _2886(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)ؿ.Type == 8)
		{
			__2871 = false;
			Rectangle rect = _2887();
			PageSliderPage pageSliderPage = SelectedPage;
			if (ؿ.NewValue > __2872)
			{
				for (int num = __2874.Count - 1; num >= 0; num--)
				{
					PageSliderPage pageSliderPage2 = __2874[num];
					if (((Control)pageSliderPage2).Bounds.IntersectsWith(rect) && rect.Right - ((Control)pageSliderPage2).Left >= Յ)
					{
						pageSliderPage = pageSliderPage2;
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < __2874.Count; i++)
				{
					PageSliderPage pageSliderPage3 = __2874[i];
					if (((Control)pageSliderPage3).Bounds.IntersectsWith(rect) && ((Control)pageSliderPage3).Right - rect.X >= Յ)
					{
						pageSliderPage = pageSliderPage3;
						break;
					}
				}
			}
			if (SelectedPage != pageSliderPage)
			{
				SelectedPage = pageSliderPage;
				if (_ᡪ == eOrientation.Horizontal)
				{
					__286D.Value = Math.Abs(((Control)__2874[0]).Left);
				}
				else
				{
					__286E.Value = Math.Abs(((Control)__2874[0]).Top);
				}
			}
			else
			{
				_2869 = Point.Empty;
			}
			return;
		}
		if (!__2871)
		{
			if (_ᡪ == eOrientation.Horizontal)
			{
				__2872 = __286D.Value;
			}
			else
			{
				__2872 = __286E.Value;
			}
			__2871 = true;
		}
		if ((int)ؿ.ScrollOrientation == 0)
		{
			_2869 = new Point(__2872 - ؿ.NewValue, 0);
		}
		else
		{
			_2869 = new Point(0, __2872 - ؿ.NewValue);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		_2882(_2883: false);
		((Control)this).OnResize(e);
	}

	private Rectangle _2887()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Invalid comparison between Unknown and I4
		Rectangle result = _288A(((Control)this).ClientRectangle, __2877);
		if (_ᡪ == eOrientation.Horizontal)
		{
			result.Width -= __2878 + __2879;
		}
		else
		{
			result.Height -= __2878 + __2879;
		}
		if (__2870)
		{
			if ((int)((Control)__286C).Dock == 1)
			{
				result.Y += ((Control)__286C).Height;
				result.Height -= ((Control)__286C).Height;
			}
			else if ((int)((Control)__286C).Dock == 2)
			{
				result.Height -= ((Control)__286C).Height;
			}
			else if ((int)((Control)__286C).Dock == 3)
			{
				result.X += ((Control)__286C).Width;
				result.Width -= ((Control)__286C).Width;
			}
			else if ((int)((Control)__286C).Dock == 4)
			{
				result.Width -= ((Control)__286C).Width;
			}
		}
		if (__287D == eScrollBarVisibility.AlwaysVisible)
		{
			if (_ᡪ == eOrientation.Horizontal)
			{
				result.Height -= ((Control)__286D).Height;
			}
			else
			{
				result.Width -= ((Control)__286D).Width;
			}
		}
		return result;
	}

	private Rectangle _2888(PageSliderPage _2889)
	{
		int num = __2874.IndexOf(_2889);
		int selectedPageIndex = SelectedPageIndex;
		Rectangle empty = Rectangle.Empty;
		Rectangle rectangle = _2887();
		if (num == selectedPageIndex)
		{
			empty = rectangle;
		}
		else if (num > selectedPageIndex)
		{
			empty = rectangle;
			if (_ᡪ == eOrientation.Horizontal)
			{
				empty.X += (__2878 + empty.Width) * (num - selectedPageIndex);
			}
			else
			{
				empty.Y += (__2878 + empty.Height) * (num - selectedPageIndex);
			}
		}
		else
		{
			empty = rectangle;
			if (_ᡪ == eOrientation.Horizontal)
			{
				empty.X -= (__2878 + empty.Width) * (selectedPageIndex - num);
			}
			else
			{
				empty.Y -= (__2878 + empty.Height) * (selectedPageIndex - num);
			}
		}
		empty.Offset(__2873);
		return empty;
	}

	private Rectangle _288A(Rectangle भ, Padding _288B)
	{
		भ.X += _288B.Left;
		भ.Y += _288B.Top;
		भ.Width -= _288B.Horizontal;
		भ.Height -= _288B.Vertical;
		return भ;
	}

	private void _288C()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		__2874.Clear();
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is PageSliderPage pageSliderPage))
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < __2874.Count; i++)
			{
				if (__2874[i].PageNumber > pageSliderPage.PageNumber)
				{
					__2874.Insert(i, pageSliderPage);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				__2874.Add(pageSliderPage);
			}
		}
		__286C.StepCount = __2874.Count;
		__286C.CurrentStep = SelectedPageIndex + 1;
	}

	public PageSliderPage[] GetAllPages()
	{
		return __2874.ToArray();
	}

	public void RemoveAllPages()
	{
		foreach (PageSliderPage item in __2874)
		{
			((Control)item).Visible = false;
		}
		while (__2874.Count > 0)
		{
			PageSliderPage pageSliderPage = __2874[0];
			((Control)this).Controls.Remove((Control)(object)pageSliderPage);
			((Component)(object)pageSliderPage).Dispose();
		}
	}

	protected virtual void OnSelectedPageChanged(PageSliderPage oldValue, PageSliderPage newValue)
	{
		if (__20D7 > 0)
		{
			_288D(oldValue, newValue);
		}
		else
		{
			_2882(_2883: false);
		}
		_2885();
		__286C.CurrentStep = SelectedPageIndex + 1;
		OnSelectedPageChanged(EventArgs.Empty);
	}

	private void _288D(PageSliderPage ك, PageSliderPage ل)
	{
		int num = -1;
		if (ك != null)
		{
			num = __2874.IndexOf(ك);
		}
		int num2 = -1;
		if (ل != null)
		{
			num2 = __2874.IndexOf(ل);
		}
		if (num == num2)
		{
			_2882(_2883: false);
			__2873 = Point.Empty;
			return;
		}
		int num3 = 0;
		Rectangle rectangle = _2887();
		num3 = ((_ᡪ != 0) ? ((num - num2) * (rectangle.Height + __2878) - __2873.Y) : ((num - num2) * (rectangle.Width + __2878) - __2873.X));
		__2873 = Point.Empty;
		DateTime now = DateTime.Now;
		int num4 = 1;
		int num5 = Math.Abs(num3);
		int num6 = Math.Sign(num3);
		int num7 = __20D7;
		if ((_ᡪ == eOrientation.Horizontal && (double)num5 < (double)rectangle.Width * 0.8) || (_ᡪ == eOrientation.Vertical && (double)num5 < (double)rectangle.Height * 0.8))
		{
			num7 = ((_ᡪ != 0) ? (num7 * (num5 / rectangle.Height)) : (num7 * (num5 / rectangle.Width)));
			num7 = Math.Max(num7, 50);
		}
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, num7);
		while (num5 > 0)
		{
			DateTime now2 = DateTime.Now;
			foreach (PageSliderPage item in __2874)
			{
				Rectangle bounds = ((Control)item).Bounds;
				if (_ᡪ == eOrientation.Horizontal)
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
		_2882(_2883: false);
	}

	protected virtual void OnPageSpacingChanged(int oldValue, int newValue)
	{
		_2882(_2883: false);
	}

	protected virtual void OnNextPageVisibleMarginChanged(int oldValue, int newValue)
	{
		_2882(_2883: false);
	}

	protected virtual void OnOrientationChanged(eOrientation oldValue, eOrientation newValue)
	{
		_2882(_2883: false);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 793)
		{
			switch (_0EFE._0F0A(((Message)(ref m)).LParam))
			{
			case 1:
				if (SelectedPageIndex > 0)
				{
					SelectedPageIndex--;
				}
				((Message)(ref m)).Result = new IntPtr(1);
				break;
			case 2:
				if (SelectedPageIndex < __2874.Count - 1)
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
		if ((int)e.Button == 1048576 && _ظ == null && __2874.Count > 0 && __287C)
		{
			__287A = true;
			_ػ = e.Location;
		}
		((Control)this).OnMouseDown(e);
	}

	internal void _288E()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if (__287B && !__287A && (int)Control.MouseButtons == 1048576 && _ظ == null && __2874.Count > 0)
		{
			((Control)this).Capture = true;
			__287A = true;
			_ػ = ((Control)this).PointToClient(Control.MousePosition);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (__287D == eScrollBarVisibility.AutoVisible && !((Control)__286D).Visible && !((Control)__286E).Visible)
		{
			_2892();
		}
		if (__287A)
		{
			Rectangle rectangle = _2887();
			if (_ᡪ == eOrientation.Horizontal)
			{
				int num = e.Location.X - _ػ.X;
				int num2 = num - __2873.X;
				_2888(__2874[0]);
				if (((Control)__2874[0]).Left + num2 > rectangle.X + Ն && SelectedPageIndex >= 0)
				{
					num -= ((Control)__2874[0]).Left + num2 - (rectangle.X + Ն);
				}
				else if (((Control)__2874[__2874.Count - 1]).Left + num2 < rectangle.X - Ն)
				{
					num += rectangle.X - Ն - (((Control)__2874[__2874.Count - 1]).Left + num2);
				}
				_2869 = new Point(num, 0);
			}
			else
			{
				int num3 = e.Location.Y - _ػ.Y;
				int num4 = num3 - __2873.Y;
				_2888(__2874[0]);
				if (((Control)__2874[0]).Top + num4 > rectangle.Y + Ն && SelectedPageIndex >= 0)
				{
					num3 -= ((Control)__2874[0]).Top + num4 - (rectangle.Y + Ն);
				}
				else if (((Control)__2874[__2874.Count - 1]).Top + num4 < rectangle.Y - Ն)
				{
					num3 += rectangle.Y - Ն - (((Control)__2874[__2874.Count - 1]).Top + num4);
				}
				_2869 = new Point(0, num3);
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (__287A)
		{
			__287A = false;
			int num = ((_ᡪ == eOrientation.Horizontal) ? __2873.X : __2873.Y);
			if (Math.Abs(num) >= Յ)
			{
				Rectangle rect = _2887();
				if (num < 0)
				{
					if (SelectedPageIndex < __2874.Count - 1)
					{
						PageSliderPage pageSliderPage = __2874[SelectedPageIndex + 1];
						for (int num2 = __2874.Count - 1; num2 >= 0; num2--)
						{
							PageSliderPage pageSliderPage2 = __2874[num2];
							if (((Control)pageSliderPage2).Bounds.IntersectsWith(rect) && rect.Right - ((Control)pageSliderPage2).Left >= Յ)
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
							_2869 = Point.Empty;
						}
					}
					else
					{
						_2869 = Point.Empty;
					}
				}
				else if (SelectedPageIndex > 0)
				{
					PageSliderPage pageSliderPage3 = __2874[SelectedPageIndex - 1];
					for (int i = 0; i < __2874.Count; i++)
					{
						PageSliderPage pageSliderPage4 = __2874[i];
						if (((Control)pageSliderPage4).Bounds.IntersectsWith(rect) && ((Control)pageSliderPage4).Right - rect.X >= Յ)
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
						_2869 = Point.Empty;
					}
				}
				else
				{
					_2869 = Point.Empty;
				}
			}
			else
			{
				_2869 = Point.Empty;
			}
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (__287D == eScrollBarVisibility.AutoVisible && ((Control)this).Visible)
		{
			_2892();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected virtual void OnScrollBarVisibilityChanged(eScrollBarVisibility oldValue, eScrollBarVisibility newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			_288F();
		}
		else
		{
			__287E = true;
		}
	}

	private void _288F()
	{
		if (__287D == eScrollBarVisibility.Hidden)
		{
			_2894();
			((Control)__286D).Visible = false;
			((Control)__286E).Visible = false;
			_2882(_2883: false);
		}
		else if (__287D == eScrollBarVisibility.AlwaysVisible)
		{
			_2894();
			if (_ᡪ == eOrientation.Horizontal)
			{
				if (!((Control)__286D).Visible)
				{
					((Control)__286D).Visible = true;
					if (!((Control)this).IsHandleCreated && ((Control)__286D).Width == 0)
					{
						((Control)__286D).Bounds = new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - ((Control)__286D).Height, ((Control)this).ClientRectangle.Width, ((Control)__286D).Height);
					}
					_2882(_2883: false);
				}
				((Control)__286E).Visible = false;
			}
			else
			{
				((Control)__286D).Visible = false;
				if (!((Control)__286E).Visible)
				{
					((Control)__286E).Visible = true;
					_2882(_2883: false);
				}
			}
		}
		else
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (__2880 == null && ((Control)this).ClientRectangle.Contains(pt))
			{
				_2892();
			}
		}
	}

	internal void _2890()
	{
		if (__287D == eScrollBarVisibility.AutoVisible)
		{
			_2892();
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_2890();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_2891();
		((Control)this).OnMouseLeave(e);
	}

	internal void _2891()
	{
		if (__287D == eScrollBarVisibility.AutoVisible)
		{
			_2892();
		}
	}

	private void _2892()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		if (_ᡪ == eOrientation.Horizontal)
		{
			((Control)__286D).Visible = true;
			((Control)__286E).Visible = false;
		}
		else
		{
			((Control)__286D).Visible = false;
			((Control)__286E).Visible = true;
		}
		if (__2880 == null)
		{
			__2880 = new Timer();
			__2880.Tick += _2893;
			__2880.Interval = __287F;
		}
		__2880.Stop();
		__2880.Start();
	}

	private void _2893(object _٠, EventArgs ؿ)
	{
		bool flag = false;
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(point))
		{
			if (!__2881.IsEmpty && __2881 == point && ((!((Control)__286D).Bounds.Contains(point) && ((Control)__286D).Visible) || (!((Control)__286E).Bounds.Contains(point) && ((Control)__286E).Visible)))
			{
				flag = true;
			}
		}
		else if (!__2881.IsEmpty)
		{
			flag = true;
		}
		__2881 = point;
		if (flag)
		{
			_2894();
			((Control)__286D).Visible = false;
			((Control)__286E).Visible = false;
			__2881 = Point.Empty;
		}
	}

	private void _2894()
	{
		Timer val = __2880;
		__2880 = null;
		if (val != null)
		{
			val.Tick -= _2893;
			val.Stop();
			((Component)(object)val).Dispose();
		}
	}

	private bool _2895(GestureEventArgs ؿ)
	{
		if (_ع == eTouchHandling.No)
		{
			return false;
		}
		if (_ع == eTouchHandling.ClientContentOnly)
		{
			_089F.POINT p = new _089F.POINT(ؿ.Location);
			IntPtr intPtr = _089F.ChildWindowFromPoint(((Control)this).Handle, p);
			if (intPtr == IntPtr.Zero)
			{
				return false;
			}
			Control val = Control.FromChildHandle(intPtr);
			if (val == null)
			{
				return false;
			}
			if (val is PageSliderPage)
			{
				intPtr = _089F.ChildWindowFromPoint(val.Handle, p);
				if (intPtr == IntPtr.Zero)
				{
					return false;
				}
				val = Control.FromChildHandle(intPtr);
				if (val == null || val is PageSliderPage || val is StepIndicator || val is Label || val is LabelX)
				{
					return true;
				}
			}
			if (val == this || val is StepIndicator)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private void _۸(object _٠, GestureEventArgs ؿ)
	{
		if (_2895(ؿ))
		{
			_غ = true;
			_ػ = ؿ.Location;
			ؿ.Handled = true;
		}
	}

	private void _۹(object _٠, GestureEventArgs ؿ)
	{
		if (_غ)
		{
			ۺ();
			ؿ.Handled = true;
		}
	}

	private void ۺ()
	{
		_غ = false;
		int num = ((_ᡪ == eOrientation.Horizontal) ? __2873.X : __2873.Y);
		if (Math.Abs(num) >= Յ)
		{
			Rectangle rect = _2887();
			if (num < 0)
			{
				if (SelectedPageIndex < __2874.Count - 1)
				{
					PageSliderPage pageSliderPage = __2874[SelectedPageIndex + 1];
					for (int num2 = __2874.Count - 1; num2 >= 0; num2--)
					{
						PageSliderPage pageSliderPage2 = __2874[num2];
						if (((Control)pageSliderPage2).Bounds.IntersectsWith(rect) && rect.Right - ((Control)pageSliderPage2).Left >= Յ)
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
						_2869 = Point.Empty;
					}
				}
				else
				{
					_2869 = Point.Empty;
				}
			}
			else if (SelectedPageIndex > 0)
			{
				PageSliderPage pageSliderPage3 = __2874[SelectedPageIndex - 1];
				for (int i = 0; i < __2874.Count; i++)
				{
					PageSliderPage pageSliderPage4 = __2874[i];
					if (((Control)pageSliderPage4).Bounds.IntersectsWith(rect) && ((Control)pageSliderPage4).Right - rect.X >= Յ)
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
					_2869 = Point.Empty;
				}
			}
			else
			{
				_2869 = Point.Empty;
			}
		}
		else
		{
			_2869 = Point.Empty;
		}
	}

	private void ۻ(object _٠, GestureEventArgs ؿ)
	{
		if (!_غ)
		{
			return;
		}
		Rectangle rectangle = _2887();
		if (_ᡪ == eOrientation.Horizontal)
		{
			int num = ؿ.Location.X - _ػ.X;
			int num2 = num - __2873.X;
			_2888(__2874[0]);
			bool flag = false;
			if (((Control)__2874[0]).Left + num2 > rectangle.X + Ն && SelectedPageIndex >= 0)
			{
				num -= ((Control)__2874[0]).Left + num2 - (rectangle.X + Ն);
				flag = true;
			}
			else if (((Control)__2874[__2874.Count - 1]).Left + num2 < rectangle.X - Ն)
			{
				num += rectangle.X - Ն - (((Control)__2874[__2874.Count - 1]).Left + num2);
				flag = true;
			}
			_2869 = new Point(num, 0);
			if (flag && ؿ.IsInertia)
			{
				ۺ();
			}
		}
		else
		{
			int num3 = ؿ.Location.Y - _ػ.Y;
			int num4 = num3 - __2873.Y;
			_2888(__2874[0]);
			bool flag2 = false;
			if (((Control)__2874[0]).Top + num4 > rectangle.Y + Ն && SelectedPageIndex >= 0)
			{
				num3 -= ((Control)__2874[0]).Top + num4 - (rectangle.Y + Ն);
				flag2 = true;
			}
			else if (((Control)__2874[__2874.Count - 1]).Top + num4 < rectangle.Y - Ն)
			{
				num3 += rectangle.Y - Ն - (((Control)__2874[__2874.Count - 1]).Top + num4);
				flag2 = true;
			}
			_2869 = new Point(0, num3);
			if (flag2 && ؿ.IsInertia)
			{
				ۺ();
			}
		}
		ؿ.Handled = true;
	}

	protected virtual void OnTouchEnabledChanged(eTouchHandling oldValue, eTouchHandling newValue)
	{
	}
}
