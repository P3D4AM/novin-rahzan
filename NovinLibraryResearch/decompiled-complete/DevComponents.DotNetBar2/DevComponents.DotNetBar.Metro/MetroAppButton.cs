using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro;

[DefaultEvent("Click")]
[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.MetroAppButtonDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class MetroAppButton : ButtonItem
{
	private bool __20AF;

	private MetroTabItem __20B0;

	private bool __1BFA = true;

	private SuperTabControl __1BFB;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (base.Expanded != value && (value || !(ContainerControl is MetroTabStrip { _൮: not false })))
			{
				base.Expanded = value;
			}
		}
	}

	internal override bool _0C91
	{
		get
		{
			return base._0C91;
		}
		set
		{
			base._0C91 = value;
			if (base._0C91 && __20AF)
			{
				__20AF = false;
			}
		}
	}

	protected override bool IsRightHanded => false;

	protected override bool IsPulseEnabed
	{
		get
		{
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(EffectiveStyle)) && _0EFE.ষ)
			{
				return false;
			}
			return base.IsPulseEnabed;
		}
	}

	private MetroTabStrip _20AE => ContainerControl as MetroTabStrip;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether control set on BackstageTab property is used on application menu popup.")]
	public bool BackstageTabEnabled
	{
		get
		{
			return __1BFA;
		}
		set
		{
			Expanded = false;
			__1BFA = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates backstage tab that is displayed instead of popup menu.")]
	[DefaultValue(null)]
	public SuperTabControl BackstageTab
	{
		get
		{
			return __1BFB;
		}
		set
		{
			SuperTabControl ك = __1BFB;
			__1BFB = value;
			ᰅ(ك, value);
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
		if (__1BFB != null)
		{
			((Control)__1BFB).Leave -= ᰆ;
		}
		base.Dispose(disposing);
	}

	public override void RecalcSize()
	{
		_20B1._20B7(this, _20B9: true);
		m_NeedRecalcSize = false;
	}

	protected internal override void OnExpandChange()
	{
		__20AF = false;
		if (!DesignMode && Expanded && base.PopupLocation.IsEmpty && PopupSide == ePopupSide.Default && (Parent is ᱭ || Parent is ᐾ) && SubItems.Count > 0 && SubItems[0] is ItemContainer && ((ItemContainer)SubItems[0]).BackgroundStyle.Class == ElementStyleClassKeys.RibbonFileMenuContainerKey && ContainerControl is MetroTabStrip metroTabStrip)
		{
			if (Parent is ᐾ)
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
				base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, metroTabStrip._0D97().Y - 1);
			}
			__20AF = true;
		}
		if (__1BFB != null && __1BFA)
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
				ᰀ();
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
					__1BFB.TabStrip.ShowKeyTips = keyTipsControl.ShowKeyTips;
					keyTipsControl.ShowKeyTips = false;
				}
				OnPopupShowing(EventArgs.Empty);
				((Control)__1BFB).Visible = true;
				((Control)__1BFB).BringToFront();
				((Control)__1BFB).Focus();
				MetroTabStrip metroTabStrip2 = _20AE;
				if (metroTabStrip2 != null && metroTabStrip2.SelectedTab != null)
				{
					__20B0 = metroTabStrip2.SelectedTab;
					if (DesignMode)
					{
						__20B0.ᎇ = false;
					}
					else
					{
						__20B0.Checked = false;
					}
				}
			}
			else
			{
				OnPopupClose(EventArgs.Empty);
				if (__20B0 != null)
				{
					if (DesignMode)
					{
						__20B0.ᎇ = true;
					}
					else
					{
						__20B0.Checked = true;
					}
					__20B0 = null;
				}
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl2 = ContainerControl as IKeyTipsControl;
					keyTipsControl2.ShowKeyTips = __1BFB.TabStrip.ShowKeyTips;
					__1BFB.TabStrip.ShowKeyTips = false;
				}
				((Control)__1BFB).Visible = false;
				if (ContainerControl is MetroTabStrip metroTabStrip3)
				{
					metroTabStrip3.ඡ(__1BFB);
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
		if (!__1BFA || __1BFB == null || !DesignMode || !Expanded || !appLostFocus)
		{
			base.ContainerLostFocus(appLostFocus);
		}
	}

	internal void _1BFF(ItemPaintArgs _0BD2)
	{
		if (!__20AF)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (!(ContainerControl is MetroTabStrip metroTabStrip))
		{
			return;
		}
		if (Parent is ᐾ)
		{
			if (_0BD2.RightToLeft)
			{
				graphics.TranslateTransform((float)(-(LeftInternal - _0BD2.ContainerControl.Width + WidthInternal)), (float)(-TopInternal));
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
		else if (_0BD2.RightToLeft)
		{
			graphics.TranslateTransform((float)(-(LeftInternal - _0BD2.ContainerControl.Width + WidthInternal)), (float)(-(metroTabStrip._0D97().Y - 1)));
		}
		else
		{
			graphics.TranslateTransform((float)(-LeftInternal), (float)(-(metroTabStrip._0D97().Y - 1)));
		}
		graphics.ResetClip();
		Control containerControl = _0BD2.ContainerControl;
		_0BD2.ContainerControl = (Control)(object)metroTabStrip;
		_0BD2.IsOnMenu = false;
		base._๙ = true;
		bool glassEnabled = _0BD2.GlassEnabled;
		_0BD2.GlassEnabled = metroTabStrip.ষ;
		Paint(_0BD2);
		_0BD2.GlassEnabled = glassEnabled;
		base._๙ = false;
		_0BD2.ContainerControl = containerControl;
		_0BD2.IsOnMenu = true;
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

	private void ᰀ()
	{
		if (__1BFB == null)
		{
			return;
		}
		Control parent = ((Control)__1BFB).Parent;
		if (parent == null)
		{
			throw new InvalidOperationException("BackstageTab control does not have a parent");
		}
		Rectangle bounds = parent.ClientRectangle;
		MetroTabStrip metroTabStrip = _20AE;
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
		((Control)__1BFB).Bounds = bounds;
		((Control)__1BFB).Anchor = (AnchorStyles)15;
	}

	private void ᰅ(SuperTabControl ك, SuperTabControl ل)
	{
		if (ك != null)
		{
			((Control)ك).Leave -= ᰆ;
			if (ك.TabStrip != null)
			{
				ك.TabStrip.ApplicationButton = null;
			}
		}
		if (Expanded)
		{
			Expanded = false;
		}
		if (ل != null)
		{
			((Control)ل).Visible = false;
			((Control)ل).Leave += ᰆ;
			if (ل.TabStrip != null)
			{
				ل.TabStrip.ApplicationButton = this;
			}
		}
	}

	private void ᰆ(object _٠, EventArgs ؿ)
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

	internal void ᰇ(char ઈ)
	{
	}
}
