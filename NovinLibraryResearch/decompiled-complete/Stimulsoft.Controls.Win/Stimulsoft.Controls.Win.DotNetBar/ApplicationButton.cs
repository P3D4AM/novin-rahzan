using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class ApplicationButton : ButtonItem
{
	private const string SysBackstageBackButtonName = "sys_backstage_back_button";

	private const string SysBackstagePanelName = "sys_backstage_panel";

	private bool m_ThumbTucked;

	private bool _WasMaximized;

	private RibbonTabItem _LastSelectedRibbonTabItem;

	private bool _BackstageTabEnabled = true;

	private SuperTabControl _BackstageTab;

	public bool Office2007StartButton { get; set; } = true;


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
			if (base.Expanded != value && (value || !(ContainerControl is RibbonStrip { MouseDownOnCaption: not false })))
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
			if (base.PopupPositionAdjusted && m_ThumbTucked)
			{
				m_ThumbTucked = false;
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

	private RibbonStrip RibbonStrip => ContainerControl as RibbonStrip;

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
		ButtonItemLayout.LayoutButton(this, Office2007StartButton);
		m_NeedRecalcSize = false;
	}

	protected internal override void OnExpandChange()
	{
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Invalid comparison between Unknown and I4
		m_ThumbTucked = false;
		if (!DesignMode && Expanded && base.PopupLocation.IsEmpty && PopupSide == ePopupSide.Default && (Parent is CaptionItemContainer || Parent is RibbonTabItemContainer) && SubItems.Count > 0 && SubItems[0] is ItemContainer && ((ItemContainer)SubItems[0]).BackgroundStyle.Class == ElementStyleClassKeys.RibbonFileMenuContainerKey && ContainerControl is RibbonStrip ribbonStrip)
		{
			if (Parent is RibbonTabItemContainer)
			{
				if (EffectiveStyle == eDotNetBarStyle.Windows7)
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal - 1);
				}
				else if (EffectiveStyle == eDotNetBarStyle.Metro)
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : (LeftInternal - 4), TopInternal - 1);
				}
				else
				{
					base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, TopInternal);
				}
			}
			else
			{
				base.PopupLocation = new Point(IsRightToLeft ? Bounds.Right : LeftInternal, ribbonStrip.GetItemContainerBounds().Y - 1);
			}
			m_ThumbTucked = true;
		}
		_WasMaximized = false;
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
				if (EffectiveStyle == eDotNetBarStyle.Metro)
				{
					CreateBackstageBackButton();
					if (((Control)_BackstageTab).Parent is Form && (int)((Form)((Control)_BackstageTab).Parent).WindowState == 2)
					{
						_WasMaximized = true;
					}
				}
				else
				{
					RemoveBackstageBackButton();
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
				if (_BackstageTab.SelectedPanel != null)
				{
					((Control)_BackstageTab.SelectedPanel).BringToFront();
				}
				RibbonStrip ribbonStrip2 = RibbonStrip;
				if (EffectiveStyle == eDotNetBarStyle.Metro && ribbonStrip2 != null)
				{
					((RibbonStripContainerItem)ribbonStrip2.GetBaseItemContainer()).MetroBackstageOpen = true;
					ribbonStrip2.TabGroupPaintSuspended = true;
				}
				if (ribbonStrip2 != null && ribbonStrip2.SelectedRibbonTabItem != null)
				{
					_LastSelectedRibbonTabItem = ribbonStrip2.SelectedRibbonTabItem;
					if (DesignMode)
					{
						_LastSelectedRibbonTabItem.RenderTabState = false;
					}
					else
					{
						_LastSelectedRibbonTabItem.Checked = false;
					}
				}
			}
			else
			{
				PopupOpenEventArgs popupOpenEventArgs2 = new PopupOpenEventArgs();
				OnPopupClose(popupOpenEventArgs2);
				if (popupOpenEventArgs2.Cancel)
				{
					Expanded = true;
					return;
				}
				if (_LastSelectedRibbonTabItem != null)
				{
					if (DesignMode)
					{
						_LastSelectedRibbonTabItem.RenderTabState = true;
					}
					else
					{
						_LastSelectedRibbonTabItem.Checked = true;
					}
					_LastSelectedRibbonTabItem = null;
				}
				if (ContainerControl is IKeyTipsControl)
				{
					IKeyTipsControl keyTipsControl2 = ContainerControl as IKeyTipsControl;
					keyTipsControl2.ShowKeyTips = _BackstageTab.TabStrip.ShowKeyTips;
					_BackstageTab.TabStrip.ShowKeyTips = false;
				}
				((Control)_BackstageTab).Visible = false;
				RibbonStrip ribbonStrip3 = ContainerControl as RibbonStrip;
				if (EffectiveStyle == eDotNetBarStyle.Metro)
				{
					((Control)_BackstageTab).Resize -= BackstageTabResized;
					if (ribbonStrip3 != null)
					{
						((RibbonStripContainerItem)ribbonStrip3.GetBaseItemContainer()).MetroBackstageOpen = false;
						ribbonStrip3.TabGroupPaintSuspended = false;
					}
				}
				if (ribbonStrip3 != null)
				{
					ribbonStrip3.BackstageTabClosed(_BackstageTab);
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

	private void CreateBackstageBackButton()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		if (!_BackstageTab.Tabs.Contains("sys_backstage_back_button"))
		{
			ButtonItem buttonItem = new ButtonItem("sys_backstage_back_button");
			buttonItem.ColorTable = eButtonColor.BlueOrb;
			buttonItem.Visible = false;
			buttonItem.Shape = new EllipticalArrowShapeDescriptor();
			buttonItem.Click += BackButtonClick;
			buttonItem.FixedSize = new Size(35, 35);
			buttonItem.Margin = new Padding(10, 0, 15, 15);
			buttonItem.SetSystemItem(b: true);
			_BackstageTab.Tabs.Insert(0, buttonItem);
			buttonItem.SetDesignMode(b: false);
			Panel val = new Panel();
			((Control)val).Dock = (DockStyle)1;
			((Control)val).Name = "sys_backstage_panel";
			((Control)_BackstageTab).Controls.Add((Control)(object)val);
			((Control)val).BringToFront();
		}
	}

	private void RemoveBackstageBackButton()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (_BackstageTab.Tabs.Contains("sys_backstage_back_button"))
		{
			_BackstageTab.Tabs.Remove("sys_backstage_back_button");
			Panel val = (Panel)((Control)_BackstageTab).Controls["sys_backstage_panel"];
			((Control)_BackstageTab).Controls.Remove((Control)(object)val);
			((Component)(object)val).Dispose();
		}
	}

	private void BackButtonClick(object sender, EventArgs e)
	{
		Expanded = false;
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
		if (!m_ThumbTucked)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		if (!(ContainerControl is RibbonStrip ribbonStrip))
		{
			return;
		}
		if (Parent is RibbonTabItemContainer)
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
			graphics.TranslateTransform((float)(-(LeftInternal - pa.ContainerControl.Width + WidthInternal)), (float)(-(ribbonStrip.GetItemContainerBounds().Y - 1)));
		}
		else
		{
			graphics.TranslateTransform((float)(-LeftInternal), (float)(-(ribbonStrip.GetItemContainerBounds().Y - 1)));
		}
		graphics.ResetClip();
		Control containerControl = pa.ContainerControl;
		pa.ContainerControl = (Control)(object)ribbonStrip;
		pa.IsOnMenu = false;
		base.IgnoreAlpha = true;
		bool glassEnabled = pa.GlassEnabled;
		pa.GlassEnabled = ribbonStrip.IsGlassEnabled;
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
		else if (EffectiveStyle == eDotNetBarStyle.Metro && Office2007StartButton)
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

	private void UpdateBackstageTabSize()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		if (_BackstageTab == null)
		{
			return;
		}
		Control val = (Control)(object)((Control)_BackstageTab).FindForm();
		if (val == null)
		{
			return;
		}
		Rectangle bounds = val.ClientRectangle;
		RibbonStrip ribbonStrip = RibbonStrip;
		if (ribbonStrip != null)
		{
			if (EffectiveStyle == eDotNetBarStyle.Metro)
			{
				bounds = new Rectangle(1, 1, val.Width - 2, val.Height - 2);
				Form val2 = (Form)(object)((val is Form) ? val : null);
				if (val2 != null && (int)val2.WindowState == 2)
				{
					if (val2 is RibbonForm)
					{
						NonClientInfo nonClientInfo = ((RibbonForm)(object)val2).GetNonClientInfo();
						bounds.Width -= nonClientInfo.LeftBorder;
						bounds.Height -= nonClientInfo.BottomBorder;
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
				Point point = val.PointToClient(((Control)ribbonStrip).PointToScreen(Bounds.Location));
				point.Y += Bounds.Height;
				bounds = new Rectangle(point.X, point.Y, val.ClientRectangle.Width, val.ClientRectangle.Height - point.Y);
				if (val is RibbonForm { IsGlassEnabled: false } ribbonForm)
				{
					bounds.Width -= ribbonForm.BorderSize * 2;
					bounds.Height -= ribbonForm.BorderSize;
				}
			}
		}
		((Control)_BackstageTab).Bounds = bounds;
		if (EffectiveStyle == eDotNetBarStyle.Metro)
		{
			((Control)_BackstageTab).Resize += BackstageTabResized;
			_BackstageTab.RecalcLayout();
			UpdateBackstageTabMetroRegion();
		}
		else if (((Control)_BackstageTab).Region != null)
		{
			((Control)_BackstageTab).Region = null;
		}
		((Control)_BackstageTab).Anchor = (AnchorStyles)15;
	}

	private void BackstageTabResized(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		if (EffectiveStyle == eDotNetBarStyle.Metro)
		{
			Control parent = ((Control)_BackstageTab).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			if (val != null && (int)val.WindowState == 2)
			{
				_WasMaximized = true;
			}
			else if (_WasMaximized)
			{
				_WasMaximized = false;
				UpdateBackstageTabSize();
				return;
			}
		}
		UpdateBackstageTabMetroRegion();
	}

	private void UpdateBackstageTabMetroRegion()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		int num = StiScale2.YYI(26.0);
		((Control)_BackstageTab).Controls["sys_backstage_panel"].Height = num;
		Rectangle clientRectangle = ((Control)_BackstageTab).ClientRectangle;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(0, 0, ((Control)_BackstageTab.TabStrip).Width, 0);
		val.AddLine(((Control)_BackstageTab.TabStrip).Width, 0, ((Control)_BackstageTab.TabStrip).Width, num);
		val.AddLine(((Control)_BackstageTab.TabStrip).Width, num, clientRectangle.Width, num);
		val.AddLine(clientRectangle.Width, clientRectangle.Height, 0, clientRectangle.Height);
		val.CloseAllFigures();
		Region region = new Region(val);
		((Control)_BackstageTab).Region = region;
		val.Dispose();
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

	public override void Refresh()
	{
		if (EffectiveStyle == eDotNetBarStyle.Metro)
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
}
