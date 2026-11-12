using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
public class SlidePanel : UserControl
{
	internal const int DefaultAnimationTime = 800;

	private eSlideSide _SlideSide;

	private bool _IsOpen = true;

	private bool _UsesBlockingAnimation;

	private Rectangle _OpenBounds = Rectangle.Empty;

	private bool _IsWaiting;

	private Stimulsoft.Controls.Win.DotNetBar.Animation.Animation _CurrentAnimation;

	private bool _IsAnimating;

	private SliderButton _SlideOutButton;

	private int _AnimationTime = 800;

	private bool _SlideOutButtonVisible = true;

	private Size _SlideOutButtonSize = SliderButton.DefaultSliderSize;

	private Size _SlideOutActiveButtonSize = SliderButton.DefaultActiveSliderSize;

	private ElementStyle _SlideOutButtonStyle;

	private bool _CenterContent;

	[DefaultValue(eSlideSide.Left)]
	[Category("Behavior")]
	[Description("Indicates side panel slides into.")]
	public eSlideSide SlideSide
	{
		get
		{
			return _SlideSide;
		}
		set
		{
			if (value != _SlideSide)
			{
				eSlideSide slideSide = _SlideSide;
				_SlideSide = value;
				OnSlideSideChanged(slideSide, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether panel is open. When this property is changed panel will slide in or out of the view.")]
	public bool IsOpen
	{
		get
		{
			return _IsOpen;
		}
		set
		{
			if ((!_UsesBlockingAnimation || _CurrentAnimation == null) && !_IsWaiting && value != _IsOpen)
			{
				bool isOpen = _IsOpen;
				_IsOpen = value;
				OnIsOpenChanged(isOpen, value);
			}
		}
	}

	[Browsable(false)]
	public bool UsesBlockingAnimation
	{
		get
		{
			return _UsesBlockingAnimation;
		}
		set
		{
			_UsesBlockingAnimation = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle OpenBounds
	{
		get
		{
			return _OpenBounds;
		}
		set
		{
			_OpenBounds = value;
			if (!((Control)this).Visible && ((Control)this).Bounds.Size != _OpenBounds.Size)
			{
				((Control)this).Bounds = GetSlideOutBounds(((Control)this).Parent, _OpenBounds);
			}
		}
	}

	internal bool IsAnimating => _IsAnimating;

	[DefaultValue(800)]
	[Category("Behavior")]
	[Description("Indicates animation duration time in milliseconds. Setting this property to 0 will disable slide animation.")]
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

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether slide out button is shown when panel is out of the view and which allows panel to be shown.")]
	public bool SlideOutButtonVisible
	{
		get
		{
			return _SlideOutButtonVisible;
		}
		set
		{
			if (value != _SlideOutButtonVisible)
			{
				bool slideOutButtonVisible = _SlideOutButtonVisible;
				_SlideOutButtonVisible = value;
				OnSlideOutButtonVisibleChanged(slideOutButtonVisible, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slide-out buttton size in default state.")]
	public Size SlideOutButtonSize
	{
		get
		{
			return _SlideOutButtonSize;
		}
		set
		{
			if (value != _SlideOutButtonSize)
			{
				Size slideOutButtonSize = _SlideOutButtonSize;
				_SlideOutButtonSize = value;
				OnSlideOutButtonSizeChanged(slideOutButtonSize, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates active (mouse over) slide out button size.")]
	public Size SlideOutActiveButtonSize
	{
		get
		{
			return _SlideOutActiveButtonSize;
		}
		set
		{
			if (value != _SlideOutActiveButtonSize)
			{
				Size slideOutActiveButtonSize = _SlideOutActiveButtonSize;
				_SlideOutActiveButtonSize = value;
				OnSlideOutActiveButtonSizeChanged(slideOutActiveButtonSize, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates slide-out button style.")]
	public ElementStyle SlideOutButtonStyle
	{
		get
		{
			return _SlideOutButtonStyle;
		}
		set
		{
			if (value != _SlideOutButtonStyle)
			{
				ElementStyle slideOutButtonStyle = _SlideOutButtonStyle;
				_SlideOutButtonStyle = value;
				OnSlideOutButtonStyleChanged(slideOutButtonStyle, value);
			}
		}
	}

	internal bool CenterContent
	{
		get
		{
			return _CenterContent;
		}
		set
		{
			if (value != _CenterContent)
			{
				bool centerContent = _CenterContent;
				_CenterContent = value;
				OnCenterContentChanged(centerContent, value);
			}
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get
		{
			return ((UserControl)this).Text;
		}
		set
		{
			((UserControl)this).Text = value;
		}
	}

	[Description("Occurs when IsOpen property value has changed, i.e. slide-panel is shown or hidden.")]
	public event EventHandler IsOpenChanged;

	protected virtual void OnIsOpenChanged(EventArgs e)
	{
		this.IsOpenChanged?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		DisposeSlideOutButton();
		((ContainerControl)this).Dispose(disposing);
	}

	protected virtual void OnSlideSideChanged(eSlideSide oldValue, eSlideSide newValue)
	{
		if (!IsOpen)
		{
			WaitForCurrentAnimationFinish();
			((Control)this).Bounds = GetSlideOutBounds(((Control)this).Parent, _OpenBounds);
			if (_SlideOutButtonVisible)
			{
				CreateSlideOutButton();
			}
		}
	}

	protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			SlideOutOfView();
		}
		else
		{
			SlideIntoView();
		}
		OnIsOpenChanged(EventArgs.Empty);
	}

	private void SlideIntoView()
	{
		Stimulsoft.Controls.Win.DotNetBar.Animation.Animation currentAnimation = _CurrentAnimation;
		if (currentAnimation != null)
		{
			currentAnimation.Stop();
			WaitForCurrentAnimationFinish();
		}
		Rectangle bounds = ((Control)this).Bounds;
		Rectangle openBounds = _OpenBounds;
		if (_SlideOutButton != null)
		{
			((Control)_SlideOutButton).Visible = false;
		}
		if (_AnimationTime > 0)
		{
			_IsAnimating = true;
			((Control)this).Visible = true;
			AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(this, "Bounds", bounds, openBounds), AnimationEasing.EaseOutExpo, _AnimationTime);
			animationRectangle.AnimationCompleted += AnimationCompleted;
			animationRectangle.Start();
			_CurrentAnimation = animationRectangle;
		}
		else
		{
			((Control)this).Bounds = openBounds;
		}
		if (_SlideOutButtonVisible && _SlideOutButton != null)
		{
			DisposeSlideOutButton();
		}
	}

	public void WaitForCurrentAnimationFinish()
	{
		if (_IsWaiting)
		{
			return;
		}
		_IsWaiting = true;
		try
		{
			if (_CurrentAnimation != null)
			{
				while (_CurrentAnimation != null)
				{
					Application.DoEvents();
				}
			}
		}
		finally
		{
			_IsWaiting = false;
		}
	}

	private void DisposeSlideOutButton()
	{
		if (_SlideOutButton != null)
		{
			Control parent = ((Control)_SlideOutButton).Parent;
			if (parent != null)
			{
				parent.Resize -= ParentResize;
				parent.Controls.Remove((Control)(object)_SlideOutButton);
			}
			((Component)(object)_SlideOutButton).Dispose();
			_SlideOutButton = null;
		}
	}

	private void SlideOutOfView()
	{
		Stimulsoft.Controls.Win.DotNetBar.Animation.Animation currentAnimation = _CurrentAnimation;
		if (currentAnimation != null)
		{
			currentAnimation.Stop();
			WaitForCurrentAnimationFinish();
		}
		Rectangle bounds = ((Control)this).Bounds;
		Rectangle slideOutBounds = GetSlideOutBounds();
		if (_AnimationTime > 0 && ((Control)this).Visible)
		{
			_IsAnimating = true;
			AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(this, "Bounds", bounds, slideOutBounds), AnimationEasing.EaseOutExpo, _AnimationTime);
			animationRectangle.AnimationCompleted += AnimationCompleted;
			animationRectangle.Start();
			_CurrentAnimation = animationRectangle;
		}
		else
		{
			((Control)this).Bounds = slideOutBounds;
		}
		_OpenBounds = bounds;
		if (_SlideOutButtonVisible)
		{
			CreateSlideOutButton();
		}
	}

	private void AnimationCompleted(object sender, EventArgs e)
	{
		Stimulsoft.Controls.Win.DotNetBar.Animation.Animation currentAnimation = _CurrentAnimation;
		_CurrentAnimation = null;
		currentAnimation?.Dispose();
		if (!_IsOpen)
		{
			((Control)this).Visible = false;
		}
		_IsAnimating = false;
	}

	internal void SlideOutOfViewSilent(Control parent)
	{
		_OpenBounds = ((Control)this).Bounds;
		((Control)this).Bounds = GetSlideOutBounds(parent);
		_IsOpen = false;
	}

	private void CreateSlideOutButton()
	{
		DisposeSlideOutButton();
		Control parent = ((Control)this).Parent;
		if (parent != null)
		{
			_SlideOutButton = new SliderButton(this);
			if (_SlideOutButtonStyle != null)
			{
				_SlideOutButton.Style = _SlideOutButtonStyle;
			}
			else
			{
				_SlideOutButton.Style = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
			}
			Size activeSliderSize = _SlideOutActiveButtonSize;
			Size sliderSize = _SlideOutButtonSize;
			if (_SlideSide == eSlideSide.Top || _SlideSide == eSlideSide.Bottom)
			{
				activeSliderSize = new Size(activeSliderSize.Height, activeSliderSize.Width);
				sliderSize = new Size(sliderSize.Height, sliderSize.Width);
			}
			_SlideOutButton.ActiveSliderSize = activeSliderSize;
			_SlideOutButton.SliderSize = sliderSize;
			parent.Controls.Add((Control)(object)_SlideOutButton);
			parent.Controls.SetChildIndex((Control)(object)_SlideOutButton, 0);
			parent.Resize += ParentResize;
		}
	}

	private void ParentResize(object sender, EventArgs e)
	{
		if (_SlideOutButton != null)
		{
			_SlideOutButton.UpdatePosition();
		}
	}

	private Rectangle GetSlideOutBounds()
	{
		return GetSlideOutBounds(((Control)this).Parent);
	}

	private Rectangle GetSlideOutBounds(Control parent)
	{
		return GetSlideOutBounds(parent, ((Control)this).Bounds);
	}

	private Rectangle GetSlideOutBounds(Control parent, Rectangle panelBounds)
	{
		if (parent == null)
		{
			return panelBounds;
		}
		Rectangle result = panelBounds;
		if (_SlideSide == eSlideSide.Left)
		{
			result.X = -result.Width;
		}
		else if (_SlideSide == eSlideSide.Right)
		{
			result.X = parent.Width;
		}
		else if (_SlideSide == eSlideSide.Top)
		{
			result.Y = -result.Height;
		}
		else if (_SlideSide == eSlideSide.Bottom)
		{
			result.Y = parent.Height;
		}
		return result;
	}

	protected virtual void OnAnimationTimeChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnSlideOutButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (!_IsOpen)
		{
			if (newValue)
			{
				CreateSlideOutButton();
			}
			else
			{
				DisposeSlideOutButton();
			}
		}
	}

	protected virtual void OnSlideOutButtonSizeChanged(Size oldValue, Size newValue)
	{
		if (_SlideOutButton != null)
		{
			_SlideOutButton.SliderSize = newValue;
			_SlideOutButton.UpdatePosition();
		}
	}

	public bool ShouldSerializeSlideOutButtonSize()
	{
		return _SlideOutButtonSize != SliderButton.DefaultSliderSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSlideOutButtonSize()
	{
		SlideOutButtonSize = SliderButton.DefaultSliderSize;
	}

	protected virtual void OnSlideOutActiveButtonSizeChanged(Size oldValue, Size newValue)
	{
		if (_SlideOutButton != null)
		{
			_SlideOutButton.ActiveSliderSize = newValue;
			_SlideOutButton.UpdatePosition();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSlideOutActiveButtonSize()
	{
		return _SlideOutActiveButtonSize != SliderButton.DefaultActiveSliderSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSlideOutActiveButtonSize()
	{
		SlideOutActiveButtonSize = SliderButton.DefaultActiveSliderSize;
	}

	protected virtual void OnSlideOutButtonStyleChanged(ElementStyle oldValue, ElementStyle newValue)
	{
		if (newValue != null && _SlideOutButton != null)
		{
			_SlideOutButton.Style = newValue;
		}
	}

	protected virtual void OnCenterContentChanged(bool oldValue, bool newValue)
	{
		if (newValue)
		{
			CenterControls();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		if (_CenterContent)
		{
			CenterControls();
		}
		((UserControl)this).OnResize(e);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (_CenterContent)
		{
			CenterControl(e.Control);
		}
		((Control)this).OnControlAdded(e);
	}

	private void CenterControl(Control item)
	{
		Point point = new Point(Math.Max(0, (((Control)this).Width - item.Width) / 2), Math.Max(0, (((Control)this).Height - item.Height) / 2));
		if (item.Location != point)
		{
			item.Location = point;
		}
	}

	private void CenterControls()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control item = item2;
			CenterControl(item);
		}
	}
}
