using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
public class SliderButton : Control
{
	private SlidePanel _SlidePanel;

	private Point _MouseOffset = Point.Empty;

	private Point _MouseDownPoint = Point.Empty;

	private Point _OriginalSlidePanelLocation = Point.Empty;

	private bool _IsSlidingActive;

	private int MaximizedOffset = SystemInformation.FrameBorderSize.Width * 2 - 2;

	private static readonly Point SlideOffsetStart = new Point(4, 4);

	private static readonly Point SlideOutSnapOffset = new Point(96, 96);

	private Timer _Timer;

	private bool _AutoTopMostEnabled = true;

	internal static readonly Size DefaultSliderSize = new Size(16, 36);

	private Size _SliderSize = DefaultSliderSize;

	internal static readonly Size DefaultActiveSliderSize = new Size(24, 36);

	private Size _ActiveSliderSize = DefaultActiveSliderSize;

	private bool _IsActive;

	private bool _IsMouseDown;

	private ElementStyle _Style;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether slider button automatically checks whether its top-most control on the form, i.e. visible at all times on top of other controls")]
	public bool AutoTopMostEnabled
	{
		get
		{
			return _AutoTopMostEnabled;
		}
		set
		{
			if (value != _AutoTopMostEnabled)
			{
				bool autoTopMostEnabled = _AutoTopMostEnabled;
				_AutoTopMostEnabled = value;
				OnAutoTopMostEnabledChanged(autoTopMostEnabled, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slider size in default state. Notice that size specified here applies to Left and Right SlidePanel positions. For Top and Bottom positions the Width and Height are interchaged.")]
	public Size SliderSize
	{
		get
		{
			return _SliderSize;
		}
		set
		{
			if (value != _SliderSize)
			{
				Size sliderSize = _SliderSize;
				_SliderSize = value;
				OnSliderSizeChanged(sliderSize, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slider size in active state. Notice that size specified here applies to Left and Right SlidePanel positions. For Top and Bottom positions the Width and Height are interchaged.")]
	public Size ActiveSliderSize
	{
		get
		{
			return _ActiveSliderSize;
		}
		set
		{
			if (value != _ActiveSliderSize)
			{
				Size activeSliderSize = _ActiveSliderSize;
				_ActiveSliderSize = value;
				OnActiveSliderSizeChanged(activeSliderSize, value);
			}
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates  whether slider button is in active state.")]
	public bool IsActive
	{
		get
		{
			return _IsActive;
		}
		set
		{
			if (value != _IsActive)
			{
				bool isActive = _IsActive;
				_IsActive = value;
				OnIsActiveChanged(isActive, value);
			}
		}
	}

	protected override Size DefaultSize => GetSliderSize();

	internal bool IsMouseDown
	{
		get
		{
			return _IsMouseDown;
		}
		set
		{
			if (value != _IsMouseDown)
			{
				bool isMouseDown = _IsMouseDown;
				_IsMouseDown = value;
				OnIsMouseDownChanged(isMouseDown, value);
			}
		}
	}

	public ElementStyle Style
	{
		get
		{
			return _Style;
		}
		set
		{
			if (_Style != null)
			{
				_Style.StyleChanged -= ElementStyleChanged;
			}
			_Style = value;
			if (_Style != null)
			{
				_Style.StyleChanged += ElementStyleChanged;
			}
			((Control)this).Invalidate();
		}
	}

	public SliderButton(SlidePanel slidePanel)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		_SlidePanel = slidePanel;
		((Control)this).Margin = default(Padding);
	}

	protected override void Dispose(bool disposing)
	{
		if (_Style != null)
		{
			_Style.StyleChanged -= ElementStyleChanged;
		}
		DestroyTopmostCheckTimer();
		((Control)this).Dispose(disposing);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (((Control)this).Parent != null)
		{
			UpdatePosition();
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (!_IsSlidingActive)
		{
			_SlidePanel.IsOpen = true;
		}
		((Control)this).OnClick(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			_MouseDownPoint = Control.MousePosition;
			IsMouseDown = true;
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (IsMouseDown && !_IsSlidingActive)
		{
			eSlideSide slideSide = _SlidePanel.SlideSide;
			if (slideSide == eSlideSide.Left)
			{
				int num = Control.MousePosition.X - _MouseDownPoint.X;
				Point slideOffsetStart = SlideOffsetStart;
				if (num > slideOffsetStart.X)
				{
					_IsSlidingActive = true;
					goto IL_00f6;
				}
			}
			if (slideSide == eSlideSide.Right)
			{
				int num2 = _MouseDownPoint.X - Control.MousePosition.X;
				Point slideOffsetStart = SlideOffsetStart;
				if (num2 > slideOffsetStart.X)
				{
					_IsSlidingActive = true;
					goto IL_00f6;
				}
			}
			if (slideSide == eSlideSide.Top)
			{
				int num3 = Control.MousePosition.Y - _MouseDownPoint.Y;
				Point slideOffsetStart = SlideOffsetStart;
				if (num3 > slideOffsetStart.Y)
				{
					_IsSlidingActive = true;
					goto IL_00f6;
				}
			}
			if (slideSide == eSlideSide.Bottom)
			{
				int num4 = _MouseDownPoint.Y - Control.MousePosition.Y;
				Point slideOffsetStart = SlideOffsetStart;
				if (num4 > slideOffsetStart.Y)
				{
					_IsSlidingActive = true;
				}
			}
		}
		goto IL_00f6;
		IL_00f6:
		if (IsMouseDown && _IsSlidingActive)
		{
			if (!((Control)this).Capture)
			{
				((Control)this).Capture = true;
			}
			Point mousePosition = Control.MousePosition;
			eSlideSide slideSide2 = _SlidePanel.SlideSide;
			switch (slideSide2)
			{
			case eSlideSide.Left:
				_MouseOffset = new Point(Math.Max(0, mousePosition.X - _MouseDownPoint.X), 0);
				break;
			case eSlideSide.Right:
				_MouseOffset = new Point(Math.Max(0, _MouseDownPoint.X - mousePosition.X), 0);
				break;
			case eSlideSide.Top:
				_MouseOffset = new Point(0, Math.Max(0, mousePosition.Y - _MouseDownPoint.Y));
				break;
			case eSlideSide.Bottom:
				_MouseOffset = new Point(0, Math.Max(0, _MouseDownPoint.Y - mousePosition.Y));
				break;
			}
			if (!_MouseOffset.IsEmpty)
			{
				UpdatePosition();
				if (!((Control)_SlidePanel).Visible)
				{
					((Control)_SlidePanel).Visible = true;
					_OriginalSlidePanelLocation = ((Control)_SlidePanel).Location;
				}
				switch (slideSide2)
				{
				case eSlideSide.Left:
					((Control)_SlidePanel).Left = _OriginalSlidePanelLocation.X + _MouseOffset.X;
					break;
				case eSlideSide.Right:
					((Control)_SlidePanel).Left = _OriginalSlidePanelLocation.X - _MouseOffset.X;
					break;
				case eSlideSide.Top:
					((Control)_SlidePanel).Top = _OriginalSlidePanelLocation.Y + _MouseOffset.Y;
					break;
				case eSlideSide.Bottom:
					((Control)_SlidePanel).Top = _OriginalSlidePanelLocation.Y - _MouseOffset.Y;
					break;
				}
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (IsMouseDown)
		{
			Point mousePosition = Control.MousePosition;
			if (((Control)this).Capture)
			{
				((Control)this).Capture = false;
			}
			eSlideSide slideSide = _SlidePanel.SlideSide;
			if (slideSide == eSlideSide.Left)
			{
				int num = mousePosition.X - _MouseDownPoint.X;
				Point slideOutSnapOffset = SlideOutSnapOffset;
				if (num > slideOutSnapOffset.X)
				{
					_SlidePanel.IsOpen = true;
					goto IL_0126;
				}
			}
			if (slideSide == eSlideSide.Right)
			{
				int num2 = _MouseDownPoint.X - mousePosition.X;
				Point slideOutSnapOffset = SlideOutSnapOffset;
				if (num2 > slideOutSnapOffset.X)
				{
					_SlidePanel.IsOpen = true;
					goto IL_0126;
				}
			}
			if (slideSide == eSlideSide.Top)
			{
				int num3 = mousePosition.Y - _MouseDownPoint.Y;
				Point slideOutSnapOffset = SlideOutSnapOffset;
				if (num3 > slideOutSnapOffset.Y)
				{
					_SlidePanel.IsOpen = true;
					goto IL_0126;
				}
			}
			if (slideSide == eSlideSide.Bottom)
			{
				int num4 = _MouseDownPoint.Y - mousePosition.Y;
				Point slideOutSnapOffset = SlideOutSnapOffset;
				if (num4 > slideOutSnapOffset.Y)
				{
					_SlidePanel.IsOpen = true;
					goto IL_0126;
				}
			}
			if (_IsSlidingActive)
			{
				((Control)_SlidePanel).Location = _OriginalSlidePanelLocation;
				((Control)_SlidePanel).Visible = false;
			}
			goto IL_0126;
		}
		goto IL_0133;
		IL_0133:
		((Control)this).OnMouseUp(e);
		return;
		IL_0126:
		IsMouseDown = false;
		UpdatePosition();
		goto IL_0133;
	}

	protected override void OnMouseCaptureChanged(EventArgs e)
	{
		if (IsMouseDown)
		{
			_ = ((Control)this).Capture;
		}
		((Control)this).OnMouseCaptureChanged(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		Size activeSliderSize = ActiveSliderSize;
		if (activeSliderSize != ((Control)this).Size)
		{
			((Control)this).Size = activeSliderSize;
			UpdatePosition();
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		Size sliderSize = SliderSize;
		if (sliderSize != ((Control)this).Size)
		{
			((Control)this).Size = sliderSize;
			UpdatePosition();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
	}

	internal void UpdatePosition()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		Control parent = ((Control)this).Parent;
		if (parent != null)
		{
			eSlideSide slideSide = _SlidePanel.SlideSide;
			Point location = default(Point);
			Padding margin;
			switch (slideSide)
			{
			case eSlideSide.Left:
			{
				margin = ((Control)this).Margin;
				location.X = ((Padding)(ref margin)).Left + _MouseOffset.X;
				int num4 = (parent.ClientRectangle.Height - ((Control)this).Height) / 2;
				margin = ((Control)this).Margin;
				location.Y = num4 + ((Padding)(ref margin)).Top;
				break;
			}
			case eSlideSide.Right:
			{
				int width = parent.ClientRectangle.Width;
				margin = ((Control)this).Margin;
				location.X = width - ((Padding)(ref margin)).Right - ((Control)this).Width - _MouseOffset.X;
				int num3 = (parent.ClientRectangle.Height - ((Control)this).Height) / 2;
				margin = ((Control)this).Margin;
				location.Y = num3 + ((Padding)(ref margin)).Top;
				break;
			}
			case eSlideSide.Top:
			{
				int num2 = (parent.ClientRectangle.Width - ((Control)this).Width) / 2;
				margin = ((Control)this).Margin;
				location.X = num2 + ((Padding)(ref margin)).Top;
				margin = ((Control)this).Margin;
				location.Y = ((Padding)(ref margin)).Top + _MouseOffset.Y;
				break;
			}
			case eSlideSide.Bottom:
			{
				int num = (parent.ClientRectangle.Width - ((Control)this).Width) / 2;
				margin = ((Control)this).Margin;
				location.X = num + ((Padding)(ref margin)).Top;
				int height = parent.ClientRectangle.Height;
				margin = ((Control)this).Margin;
				location.Y = height - ((Padding)(ref margin)).Bottom - ((Control)this).Height - _MouseOffset.Y;
				break;
			}
			}
			((Control)this).Location = location;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		if (_Style != null)
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(_Style, graphics, ((Control)this).ClientRectangle);
			ElementStyleDisplay.Paint(e2);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		SetupTopmostCheckTimer();
		((Control)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DestroyTopmostCheckTimer();
		((Control)this).OnHandleDestroyed(e);
	}

	private void SetupTopmostCheckTimer()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (_Timer == null && _AutoTopMostEnabled)
		{
			_Timer = new Timer();
			_Timer.Interval = 800;
			_Timer.Tick += CheckAutoTopMostTimerTick;
			_Timer.Start();
		}
	}

	private void CheckAutoTopMostTimerTick(object sender, EventArgs e)
	{
		if (((Control)this).Visible && ((Control)this).Parent != null)
		{
			Control parent = ((Control)this).Parent;
			if (parent.Controls.IndexOf((Control)(object)this) != 0)
			{
				parent.Controls.SetChildIndex((Control)(object)this, 0);
			}
		}
	}

	private void DestroyTopmostCheckTimer()
	{
		Timer timer = _Timer;
		_Timer = null;
		if (timer != null)
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
		}
	}

	protected virtual void OnAutoTopMostEnabledChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			DestroyTopmostCheckTimer();
		}
		else if (((Control)this).IsHandleCreated)
		{
			SetupTopmostCheckTimer();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSliderSize()
	{
		return _SliderSize != DefaultSliderSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSliderSize()
	{
		SliderSize = DefaultSliderSize;
	}

	protected virtual void OnSliderSizeChanged(Size oldValue, Size newValue)
	{
		((Control)this).Size = newValue;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeActiveSliderSize()
	{
		return _ActiveSliderSize != DefaultActiveSliderSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetActiveSliderSize()
	{
		ActiveSliderSize = DefaultActiveSliderSize;
	}

	protected virtual void OnActiveSliderSizeChanged(Size oldValue, Size newValue)
	{
	}

	protected virtual void OnIsActiveChanged(bool oldValue, bool newValue)
	{
	}

	private Size GetSliderSize()
	{
		Size result = (IsActive ? ActiveSliderSize : SliderSize);
		if (_SlidePanel != null)
		{
			eSlideSide slideSide = _SlidePanel.SlideSide;
			if (slideSide == eSlideSide.Left || slideSide == eSlideSide.Right)
			{
				return result;
			}
			return new Size(result.Height, result.Width);
		}
		return result;
	}

	protected virtual void OnIsMouseDownChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			_IsSlidingActive = false;
			_MouseOffset = Point.Empty;
		}
		((Control)this).Invalidate();
	}

	private void ElementStyleChanged(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}
}
