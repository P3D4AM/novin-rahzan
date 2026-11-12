using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class MetroAppButton : ButtonItem
{
	private bool _ThumbTucked;

	private MetroTabItem _LastSelectedTabItem;

	private bool _BackstageTabEnabled = true;

	private SuperTabControl _BackstageTab;

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (base.Expanded != value && (value || !(ContainerControl is MetroTabStrip { MouseDownOnCaption: not false })))
			{
				base.Expanded = value;
			}
		}
	}

	public override bool PopupPositionAdjusted
	{
		get
		{
			return base.PopupPositionAdjusted;
		}
		set
		{
			base.PopupPositionAdjusted = value;
			if (base.PopupPositionAdjusted && _ThumbTucked)
			{
				_ThumbTucked = false;
			}
		}
	}

	protected override bool IsRightHanded => false;

	protected override bool IsPulseEnabed
	{
		get
		{
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro) && WinApi.IsGlassEnabled)
			{
				return false;
			}
			return base.IsPulseEnabed;
		}
	}

	private MetroTabStrip MetroTabStrip => ContainerControl as MetroTabStrip;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control set on BackstageTab property is used on application menu popup.")]
	public bool BackstageTabEnabled
	{
		get
		{
			return _BackstageTabEnabled;
		}
		set
		{
			Expanded = false;
			_BackstageTabEnabled = value;
		}
	}

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates backstage tab that is displayed instead of popup menu.")]
	public SuperTabControl BackstageTab
	{
		get
		{
			return _BackstageTab;
		}
		set
		{
			SuperTabControl backstageTab = _BackstageTab;
			_BackstageTab = value;
			OnBackstageTabChanged(backstageTab, value);
		}
	}

	protected override bool CanShowPopup
	{
		get
		{
			if (ShowSubItems || ShouldAutoExpandOnClick)
			{
				if (SubItems.Count <= 0 && PopupType != ePopupType.Container)
				{
					if (BackstageTab != null)
					{
						return BackstageTabEnabled;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public MetroAppButton()
	{
		base.VerifyPopupScreenPosition = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (_BackstageTab != null)
		{
			((Control)_BackstageTab).Leave -= BackstageTabLeave;
		}
		base.Dispose(disposing);
	}

	public override void RecalcSize()
	{
		ButtonItemLayout.LayoutButton(this, startButtonType: true);
		m_NeedRecalcSize = false;
	}

	protected internal override void OnExpandChange()
	{
		_ThumbTucked = false;
		if (!DesignMode && Expanded && base.PopupLocation.IsEmpty && PopupSide == ePopupSide.Default && (Parent is CaptionItemContainer || Parent is MetroTabItemContainer) && SubItems.Count > 0 && SubItems[0] is ItemContainer && ((ItemContainer)SubItems[0]).BackgroundStyle.Class == ElementStyleClassKeys.RibbonFileMenuContainerKey && ContainerControl is MetroTabStrip metroTabStrip)
		{
			if (Parent is MetroTabItemContainer)
			{
				if (EffectiveStyle == eDotNetBarStyle.Windows7)
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal - 1);
				}
				else
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal);
				}
			}
			else
			{
				base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, metroTabStrip.GetItemContainerBounds().Y - 1);
			}
			_ThumbTucked = true;
		}
		if (_BackstageTab != null && _BackstageTabEnabled)
		{
			if (Expanded)
			{
				PopupOpenEventArgs popupOpenEventArgs = new PopupOpenEventArgs();
				OnPopupOpen(popupOpenEventArgs);
				if (popupOpenEventArgs.Cancel)
				{
					Expanded = false;
					return;
				}
				UpdateBackstageTabSize();
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
					_BackstageTab.TabStrip.ShowKeyTips = keyTipsControl.ShowKeyTips;
					keyTipsControl.ShowKeyTips = false;
				}
				OnPopupShowing(EventArgs.Empty);
				((Control)_BackstageTab).Visible = true;
				((Control)_BackstageTab).BringToFront();
				((Control)_BackstageTab).Focus();
				MetroTabStrip metroTabStrip2 = MetroTabStrip;
				if (metroTabStrip2 != null && metroTabStrip2.SelectedTab != null)
				{
					_LastSelectedTabItem = metroTabStrip2.SelectedTab;
					if (DesignMode)
					{
						_LastSelectedTabItem.RenderTabState = false;
					}
					else
					{
						_LastSelectedTabItem.Checked = false;
					}
				}
			}
			else
			{
				OnPopupClose(EventArgs.Empty);
				if (_LastSelectedTabItem != null)
				{
					if (DesignMode)
					{
						_LastSelectedTabItem.RenderTabState = true;
					}
					else
					{
						_LastSelectedTabItem.Checked = true;
					}
					_LastSelectedTabItem = null;
				}
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl2 = ContainerControl as IKeyTipsControl;
					keyTipsControl2.ShowKeyTips = _BackstageTab.TabStrip.ShowKeyTips;
					_BackstageTab.TabStrip.ShowKeyTips = false;
				}
				((Control)_BackstageTab).Visible = false;
				if (ContainerControl is MetroTabStrip metroTabStrip3)
				{
					metroTabStrip3.BackstageTabClosed(_BackstageTab);
				}
				OnPopupFinalized(EventArgs.Empty);
			}
			HideToolTip();
			RaiseExpandChange(EventArgs.Empty);
		}
		else
		{
			base.OnExpandChange();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		if (!(_BackstageTabEnabled && _BackstageTab != null && DesignMode && Expanded && appLostFocus))
		{
			base.ContainerLostFocus(appLostFocus);
		}
	}

	internal void OnMenuPaint(ItemPaintArgs pa)
	{
		if (!_ThumbTucked)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		if (!(ContainerControl is MetroTabStrip metroTabStrip))
		{
			return;
		}
		if (Parent is MetroTabItemContainer)
		{
			if (pa.RightToLeft)
			{
				graphics.TranslateTransform((float)(-(LeftInternal - pa.ContainerControl.Width + WidthInternal)), (float)(-TopInternal));
			}
			else if (EffectiveStyle == eDotNetBarStyle.Windows7)
			{
				graphics.TranslateTransform((float)(-LeftInternal), (float)(-(TopInternal - 1)));
			}
			else
			{
				graphics.TranslateTransform((float)(-LeftInternal), (float)(-TopInternal));
			}
		}
		else if (pa.RightToLeft)
		{
			graphics.TranslateTransform((float)(-(LeftInternal - pa.ContainerControl.Width + WidthInternal)), (float)(-(metroTabStrip.GetItemContainerBounds().Y - 1)));
		}
		else
		{
			graphics.TranslateTransform((float)(-LeftInternal), (float)(-(metroTabStrip.GetItemContainerBounds().Y - 1)));
		}
		graphics.ResetClip();
		Control containerControl = pa.ContainerControl;
		pa.ContainerControl = (Control)(object)metroTabStrip;
		pa.IsOnMenu = false;
		base.IgnoreAlpha = true;
		bool glassEnabled = pa.GlassEnabled;
		pa.GlassEnabled = metroTabStrip.IsGlassEnabled;
		Paint(pa);
		pa.GlassEnabled = glassEnabled;
		base.IgnoreAlpha = false;
		pa.ContainerControl = containerControl;
		pa.IsOnMenu = true;
		graphics.ResetTransform();
	}

	protected override void OnCommandChanged()
	{
	}

	protected override void OnStyleChanged()
	{
		ImagePaddingHorizontal = 0;
		ImagePaddingVertical = 0;
		base.OnStyleChanged();
	}

	private void UpdateBackstageTabSize()
	{
		if (_BackstageTab == null)
		{
			return;
		}
		Control parent = ((Control)_BackstageTab).Parent;
		if (parent == null)
		{
			throw new InvalidOperationException("BackstageTab control does not have a parent");
		}
		Rectangle bounds = parent.ClientRectangle;
		MetroTabStrip metroTabStrip = MetroTabStrip;
		if (metroTabStrip != null)
		{
			Point point = parent.PointToClient(((Control)metroTabStrip).PointToScreen(Bounds.Location));
			point.Y += Bounds.Height;
			bounds = new Rectangle(point.X, point.Y, parent.ClientRectangle.Width, parent.ClientRectangle.Height - point.Y);
			if (parent is MetroAppForm metroAppForm)
			{
				Thickness borderThickness = metroAppForm.GetBorderThickness();
				bounds.Width -= (int)borderThickness.Horizontal;
				bounds.Height -= (int)borderThickness.Bottom;
			}
		}
		((Control)_BackstageTab).Bounds = bounds;
		((Control)_BackstageTab).Anchor = (AnchorStyles)15;
	}

	private void OnBackstageTabChanged(SuperTabControl oldValue, SuperTabControl newValue)
	{
		if (oldValue != null)
		{
			((Control)oldValue).Leave -= BackstageTabLeave;
			if (oldValue.TabStrip != null)
			{
				oldValue.TabStrip.ApplicationButton = null;
			}
		}
		if (Expanded)
		{
			Expanded = false;
		}
		if (newValue != null)
		{
			((Control)newValue).Visible = false;
			((Control)newValue).Leave += BackstageTabLeave;
			if (newValue.TabStrip != null)
			{
				newValue.TabStrip.ApplicationButton = this;
			}
		}
	}

	private void BackstageTabLeave(object sender, EventArgs e)
	{
		if (Expanded)
		{
			Expanded = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ProcessEscapeKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		if ((int)keyData == 27 && Expanded)
		{
			Expanded = false;
			return true;
		}
		return false;
	}

	internal void BackstageMnemonicProcessed(char charCode)
	{
	}
}
