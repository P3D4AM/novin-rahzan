using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.ApplicationButtonDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("Click")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class ApplicationButton : ButtonItem
{
	private const string _1BF4 = "sys_backstage_back_button";

	private const string _1BF5 = "sys_backstage_panel";

	private bool _1BF6;

	private bool __1BF7;

	private bool __1BF8;

	private RibbonTabItem __1BF9;

	private bool __1BFA = true;

	private SuperTabControl __1BFB;

	[DefaultValue(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (base.Expanded != value && (value || !(ContainerControl is RibbonStrip { _൮: not false })))
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
			if (base._0C91 && _1BF6)
			{
				_1BF6 = false;
			}
		}
	}

	protected override bool IsRightHanded => false;

	internal bool _1BF2 => __1BF7;

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

	private RibbonStrip _1BF3 => ContainerControl as RibbonStrip;

	[Description("Indicates whether control set on BackstageTab property is used on application menu popup.")]
	[DefaultValue(true)]
	[Category("Behavior")]
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

	[DefaultValue(null)]
	[Description("Indicates backstage tab that is displayed instead of popup menu.")]
	[Category("Behavior")]
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
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Invalid comparison between Unknown and I4
		_1BF6 = false;
		if (!DesignMode && Expanded && base.PopupLocation.IsEmpty && PopupSide == ePopupSide.Default && (Parent is ᱭ || Parent is ය) && SubItems.Count > 0 && SubItems[0] is ItemContainer && ((ItemContainer)SubItems[0]).BackgroundStyle.Class == ElementStyleClassKeys.RibbonFileMenuContainerKey && ContainerControl is RibbonStrip ribbonStrip)
		{
			if (Parent is ය)
			{
				if (EffectiveStyle == eDotNetBarStyle.Windows7)
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal - 1);
				}
				else if (StyleManager.IsMetro(EffectiveStyle))
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : (LeftInternal - 4), TopInternal);
				}
				else
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal);
				}
			}
			else
			{
				base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, ribbonStrip._0D97().Y - 1);
			}
			base.VerifyPopupScreenPosition = false;
			_1BF6 = true;
		}
		__1BF8 = false;
		if (__1BFB != null && __1BFA)
		{
			PopupManager.CloseAllPopups();
			if (Expanded)
			{
				PopupOpenEventArgs popupOpenEventArgs = new PopupOpenEventArgs();
				OnPopupOpen(popupOpenEventArgs);
				if (popupOpenEventArgs.Cancel)
				{
					Expanded = false;
					return;
				}
				if (StyleManager.IsMetro(EffectiveStyle))
				{
					_1BFC();
					if (((Control)__1BFB).Parent is Form && (int)((Form)((Control)__1BFB).Parent).WindowState == 2)
					{
						__1BF8 = true;
					}
				}
				else
				{
					_1BFD();
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
				if (__1BFB.SelectedPanel != null)
				{
					((Control)__1BFB.SelectedPanel).BringToFront();
				}
				RibbonStrip ribbonStrip2 = _1BF3;
				if (StyleManager.IsMetro(EffectiveStyle) && ribbonStrip2 != null)
				{
					((RibbonStripContainerItem)ribbonStrip2.GetBaseItemContainer()).MetroBackstageOpen = true;
					ribbonStrip2._൪ = true;
				}
				if (ribbonStrip2 != null && ribbonStrip2.SelectedRibbonTabItem != null)
				{
					__1BF9 = ribbonStrip2.SelectedRibbonTabItem;
					if (DesignMode)
					{
						__1BF9.ᎇ = false;
					}
					else
					{
						__1BF9.Checked = false;
					}
				}
			}
			else
			{
				OnPopupClose(EventArgs.Empty);
				if (__1BF9 != null)
				{
					if (DesignMode)
					{
						__1BF9.ᎇ = true;
					}
					else
					{
						__1BF9.Checked = true;
					}
					__1BF9 = null;
				}
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl2 = ContainerControl as IKeyTipsControl;
					keyTipsControl2.ShowKeyTips = __1BFB.TabStrip.ShowKeyTips;
					__1BFB.TabStrip.ShowKeyTips = false;
				}
				((Control)__1BFB).Visible = false;
				RibbonStrip ribbonStrip3 = ContainerControl as RibbonStrip;
				if (StyleManager.IsMetro(EffectiveStyle))
				{
					((Control)__1BFB).Resize -= ᰃ;
					((Control)__1BFB.TabStrip).Resize -= ᰃ;
					if (ribbonStrip3 != null)
					{
						((RibbonStripContainerItem)ribbonStrip3.GetBaseItemContainer()).MetroBackstageOpen = false;
						ribbonStrip3._൪ = false;
					}
				}
				if (ribbonStrip3 != null)
				{
					ribbonStrip3.ඡ(__1BFB);
					((Control)ribbonStrip3).Invalidate();
				}
				OnPopupFinalized(EventArgs.Empty);
			}
			RaiseExpandChange(EventArgs.Empty);
		}
		else
		{
			base.OnExpandChange();
		}
	}

	private void _1BFC()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		if (!__1BFB.Tabs.Contains("sys_backstage_back_button"))
		{
			ButtonItem buttonItem = new ButtonItem("sys_backstage_back_button");
			buttonItem.ColorTable = eButtonColor.BlueOrb;
			buttonItem.Symbol = "\uf060";
			buttonItem.Shape = new EllipticalShapeDescriptor();
			buttonItem.Click += _1BFE;
			buttonItem.FixedSize = new Size(35, 35);
			buttonItem.Margin = new Padding(10, 0, 15, 15);
			buttonItem.SetSystemItem(b: true);
			__1BFB.Tabs.Insert(0, buttonItem);
			buttonItem.SetDesignMode(b: false);
			Panel val = new Panel();
			((Control)val).Dock = (DockStyle)1;
			((Control)val).Name = "sys_backstage_panel";
			((Control)__1BFB).Controls.Add((Control)(object)val);
			((Control)val).BringToFront();
		}
	}

	private void _1BFD()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (__1BFB.Tabs.Contains("sys_backstage_back_button"))
		{
			__1BFB.Tabs.Remove("sys_backstage_back_button");
			Panel val = (Panel)((Control)__1BFB).Controls["sys_backstage_panel"];
			((Control)__1BFB).Controls.Remove((Control)(object)val);
			((Component)(object)val).Dispose();
		}
	}

	private void _1BFE(object _٠, EventArgs ؿ)
	{
		Expanded = false;
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
		if (!_1BF6)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (!(ContainerControl is RibbonStrip ribbonStrip))
		{
			return;
		}
		if (Parent is ය)
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
			graphics.TranslateTransform((float)(-(LeftInternal - _0BD2.ContainerControl.Width + WidthInternal)), (float)(-(ribbonStrip._0D97().Y - 1)));
		}
		else
		{
			graphics.TranslateTransform((float)(-LeftInternal), (float)(-(ribbonStrip._0D97().Y - 1)));
		}
		graphics.ResetClip();
		Control containerControl = _0BD2.ContainerControl;
		_0BD2.ContainerControl = (Control)(object)ribbonStrip;
		_0BD2.IsOnMenu = false;
		base._๙ = true;
		bool glassEnabled = _0BD2.GlassEnabled;
		_0BD2.GlassEnabled = ribbonStrip.ষ;
		__1BF7 = true;
		Paint(_0BD2);
		__1BF7 = false;
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
		if (EffectiveStyle == eDotNetBarStyle.Office2010)
		{
			ImagePaddingHorizontal = 0;
			ImagePaddingVertical = 0;
		}
		else if (EffectiveStyle == eDotNetBarStyle.Windows7)
		{
			ImagePaddingHorizontal = 0;
			ImagePaddingVertical = 0;
		}
		else if (StyleManager.IsMetro(EffectiveStyle))
		{
			ImagePaddingHorizontal = 0;
			ImagePaddingVertical = 1;
		}
		else
		{
			if (ImagePaddingVertical == 0)
			{
				ImagePaddingVertical = 2;
			}
			if (ImagePaddingHorizontal == 0)
			{
				ImagePaddingHorizontal = 2;
			}
		}
		base.OnStyleChanged();
	}

	private void ᰀ()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
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
		RibbonStrip ribbonStrip = _1BF3;
		if (ribbonStrip != null)
		{
			if (StyleManager.IsMetro(EffectiveStyle))
			{
				bounds = new Rectangle(1, 1, parent.Width - 2, parent.Height - 2);
				Form val = (Form)(object)((parent is Form) ? parent : null);
				if (val != null && (int)val.WindowState == 2)
				{
					if (val is RibbonForm)
					{
						Ἇ ἇ = ((RibbonForm)(object)val).Ꮛ();
						bounds.Width -= ἇ.ἒ;
						bounds.Height -= ἇ.ἑ;
					}
					else
					{
						bounds.Width -= SystemInformation.FrameBorderSize.Width;
						bounds.Height -= SystemInformation.FrameBorderSize.Height;
					}
				}
			}
			else
			{
				Point point = parent.PointToClient(((Control)ribbonStrip).PointToScreen(Bounds.Location));
				point.Y += Bounds.Height;
				bounds = new Rectangle(point.X, point.Y, parent.ClientRectangle.Width, parent.ClientRectangle.Height - point.Y);
				if (parent is RibbonForm { IsGlassEnabled: false } ribbonForm)
				{
					bounds.Width -= ribbonForm.ủ * 2;
					bounds.Height -= ribbonForm.ủ;
				}
			}
		}
		((Control)__1BFB).Bounds = bounds;
		if (StyleManager.IsMetro(EffectiveStyle))
		{
			((Control)__1BFB).Resize += ᰃ;
			((Control)__1BFB.TabStrip).Resize += ᰃ;
			if (((Control)__1BFB).IsHandleCreated)
			{
				ᰁ();
			}
			else
			{
				((Control)__1BFB).HandleCreated += ᰂ;
			}
		}
		else if (((Control)__1BFB).Region != null)
		{
			((Control)__1BFB).Region = null;
		}
		((Control)__1BFB).Anchor = (AnchorStyles)15;
	}

	private void ᰁ()
	{
		__1BFB.RecalcLayout();
		ᰄ();
	}

	private void ᰂ(object _٠, EventArgs ؿ)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		((Control)__1BFB).HandleCreated -= ᰂ;
		BarUtilities.InvokeDelayed((MethodInvoker)delegate
		{
			ᰁ();
		}, 50);
	}

	private void ᰃ(object _٠, EventArgs ؿ)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (StyleManager.IsMetro(EffectiveStyle))
		{
			Control parent = ((Control)__1BFB).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			if (val != null && (int)val.WindowState == 2)
			{
				__1BF8 = true;
			}
			else if (__1BF8)
			{
				__1BF8 = false;
				ᰀ();
				return;
			}
		}
		ᰄ();
	}

	private void ᰄ()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		int num = 28;
		RibbonStrip ribbonStrip = _1BF3;
		if (ribbonStrip != null)
		{
			num = ribbonStrip.ඛ();
		}
		((Control)__1BFB).Controls["sys_backstage_panel"].Height = num;
		Rectangle clientRectangle = ((Control)__1BFB).ClientRectangle;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(0, 0, ((Control)__1BFB.TabStrip).Width, 0);
		val.AddLine(((Control)__1BFB.TabStrip).Width, 0, ((Control)__1BFB.TabStrip).Width, num);
		val.AddLine(((Control)__1BFB.TabStrip).Width, num, clientRectangle.Width, num);
		val.AddLine(clientRectangle.Width, clientRectangle.Height, 0, clientRectangle.Height);
		val.CloseAllFigures();
		Region region = new Region(val);
		((Control)__1BFB).Region = region;
		val.Dispose();
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

	public override void Refresh()
	{
		if (StyleManager.IsMetro(EffectiveStyle))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && val.FindForm() is RibbonForm ribbonForm)
			{
				Rectangle bounds = Bounds;
				bounds.Height++;
				bounds.Width++;
				bounds.Inflate(1, 1);
				((Control)ribbonForm).Invalidate(bounds, true);
			}
		}
		base.Refresh();
	}

	[CompilerGenerated]
	private void ᰈ()
	{
		ᰁ();
	}
}
