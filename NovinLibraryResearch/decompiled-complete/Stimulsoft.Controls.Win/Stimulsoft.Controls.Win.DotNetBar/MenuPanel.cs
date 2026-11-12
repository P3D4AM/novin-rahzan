using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MenuPanel : Control, IDesignTimeProvider, IAccessibilitySupport, IKeyTipsControl, IKeyTipsRenderer
{
	private struct ExpandButtonData
	{
		public bool PersonalizedAllVisible;

		public bool ShowExpandButton;

		public Rectangle Rect;

		public bool MouseOver;

		public ExpandButtonData(bool personalizedallvisible, bool showexpandbutton, Rectangle rExpandButton, bool mouseover)
		{
			PersonalizedAllVisible = personalizedallvisible;
			ShowExpandButton = showexpandbutton;
			Rect = rExpandButton;
			MouseOver = mouseover;
		}
	}

	private class InternalMenuColors
	{
		public LinearGradientColorTable Background = new LinearGradientColorTable();

		public LinearGradientColorTable Side = new LinearGradientColorTable();

		public LinearGradientColorTable SideUnused = new LinearGradientColorTable();

		public LinearGradientColorTable Border = new LinearGradientColorTable();

		public LinearGradientColorTable SideBorder = new LinearGradientColorTable();

		public LinearGradientColorTable SideBorderLight = new LinearGradientColorTable();
	}

	public class PopupMenuAccessibleObject : ControlAccessibleObject
	{
		private MenuPanel m_Owner;

		public override AccessibleRole Role
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_Owner == null)
				{
					return (AccessibleRole)0;
				}
				return ((Control)m_Owner).AccessibleRole;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (m_Owner == null || m_Owner.ParentItem == null)
				{
					return null;
				}
				return m_Owner.ParentItem.AccessibleObject;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (m_Owner == null)
				{
					return Rectangle.Empty;
				}
				return ((Control)m_Owner).DisplayRectangle;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				if (m_Owner != null && !((Control)m_Owner).IsDisposed)
				{
					return (AccessibleStates)4096;
				}
				return (AccessibleStates)1;
			}
		}

		public PopupMenuAccessibleObject(MenuPanel owner)
			: base((Control)(object)owner)
		{
			m_Owner = owner;
		}

		internal void GenerateEvent(BaseItem sender, AccessibleEvents e)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (m_Owner != null && m_Owner != null && m_Owner.m_ParentItem != null)
			{
				int num = m_Owner.m_ParentItem.SubItems.IndexOf(sender);
				if (num >= 0)
				{
					((Control)m_Owner).AccessibilityNotifyClients(e, num);
				}
			}
		}

		internal void GenerateEvent(AccessibleEvents e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_Owner != null)
			{
				((Control)m_Owner).AccessibilityNotifyClients(e, -1);
			}
		}

		public override int GetChildCount()
		{
			if (m_Owner != null && m_Owner.m_ParentItem != null)
			{
				return m_Owner.m_ParentItem.SubItems.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (m_Owner != null && m_Owner.m_ParentItem != null)
			{
				return m_Owner.m_ParentItem.SubItems[iIndex].AccessibleObject;
			}
			return null;
		}

		public override void Select(AccessibleSelection flags)
		{
		}

		public override void DoDefaultAction()
		{
		}
	}

	private const int WM_MOUSEACTIVATE = 33;

	private const int MA_NOACTIVATE = 3;

	private const int MA_NOACTIVATEANDEAT = 4;

	private const int DEFAULT_SIDE_WIDTH = 16;

	private const int GROUP_SPACINGDOTNET = 3;

	private const int GROUP_SPACINGOFFICE = 9;

	protected BaseItem m_ParentItem;

	protected BaseItem m_HotSubItem;

	protected Point m_ParentItemScreenPos;

	private bool m_Hover;

	private bool m_Scroll;

	private bool m_TopScroll;

	private bool m_BottomScroll;

	private int m_ScrollTopPosition;

	private int m_TopScrollHeight;

	private int m_BottomScrollHeight;

	private Rectangle m_ClientRect;

	private SideBarImage m_SideBarImage;

	private Rectangle m_SideRect;

	private object m_Owner;

	private bool m_IsCustomizeMenu;

	private Point m_MouseDownPt;

	private ePersonalizedMenus m_PersonalizedMenus;

	private const int DEFAULT_EXPAND_BUTTON_HEIGHT = 14;

	private const int OFFICE2003_EXPAND_BUTTON_HEIGHT = 18;

	private int EXPAND_BUTTON_HEIGHT = 14;

	private ExpandButtonData m_ExpandButton = new ExpandButtonData(personalizedallvisible: false, showexpandbutton: false, Rectangle.Empty, mouseover: false);

	private Timer m_ScrollTimer;

	private ePopupAnimation m_PopupAnimation = ePopupAnimation.ManagerControlled;

	private PopupShadow m_DropShadow;

	private ColorScheme m_ColorScheme;

	private bool m_IgnoreDuplicateMouseMove;

	private MouseEventArgs m_LastMouseMoveEvent;

	internal bool m_AccessibleObjectCreated;

	private bool m_PopupMenu = true;

	private bool m_ShowToolTips = true;

	private BaseItem m_DelayedCollapseItem;

	private bool m_LastExpandedOnHover = true;

	private bool m_AntiAlias;

	private int m_CornerSize = 3;

	private BaseItem m_DoDefaultActionItem;

	internal bool UseWholeScreenForSizeChecking;

	private bool m_ShowKeyTips;

	private string m_KeyTipsKeysStack = "";

	private Font m_KeyTipsFont;

	private KeyTipsCanvasControl m_KeyTipsCanvas;

	private bool m_DragDropInProgress;

	private int m_InsertPosition = -1;

	private bool m_InsertBefore;

	private IDesignTimeProvider m_DesignTimeProvider;

	private BaseItem m_DragItem;

	private IDesignTimeProvider m_DragDropDesignTimeProvider;

	private bool m_DesignerParent;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool ShowKeyTips
	{
		get
		{
			return m_ShowKeyTips;
		}
		set
		{
			if (m_ShowKeyTips != value)
			{
				m_ShowKeyTips = value;
				OnShowKeyTipsChanged();
			}
		}
	}

	string IKeyTipsControl.KeyTipsKeysStack
	{
		get
		{
			return m_KeyTipsKeysStack;
		}
		set
		{
			m_KeyTipsKeysStack = value;
			if (m_KeyTipsCanvas != null)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	public virtual Font KeyTipsFont
	{
		get
		{
			return m_KeyTipsFont;
		}
		set
		{
			m_KeyTipsFont = value;
		}
	}

	BaseItem IAccessibilitySupport.DoDefaultActionItem
	{
		get
		{
			return m_DoDefaultActionItem;
		}
		set
		{
			m_DoDefaultActionItem = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool PopupMenu
	{
		get
		{
			return m_PopupMenu;
		}
		set
		{
			m_PopupMenu = value;
		}
	}

	private bool IsContainerMenu
	{
		get
		{
			if (m_ParentItem != null && m_ParentItem.SubItems.Count == 1 && m_ParentItem.SubItems[0] is ItemContainer)
			{
				return true;
			}
			return false;
		}
	}

	private bool IsGradientStyle
	{
		get
		{
			if (m_ParentItem != null)
			{
				if (m_ParentItem.EffectiveStyle != 0 && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2003 && m_ParentItem.EffectiveStyle != eDotNetBarStyle.VS2005)
				{
					return BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle);
				}
				return true;
			}
			return true;
		}
	}

	public BaseItem ParentItem
	{
		get
		{
			return m_ParentItem;
		}
		set
		{
			RestoreContainer();
			m_ParentItem = value;
			if (m_ParentItem != null && m_ParentItem.SubItems.Count != 0)
			{
				if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					EXPAND_BUTTON_HEIGHT = 18;
				}
				else
				{
					EXPAND_BUTTON_HEIGHT = 14;
				}
				if (m_ParentItem.SubItems.Count > 0)
				{
					m_ParentItem.SubItems[0].ContainerControl = this;
				}
				for (int i = 1; i < m_ParentItem.SubItems.Count; i++)
				{
					m_ParentItem.SubItems[i].ContainerControl = this;
				}
				object containerControl = m_ParentItem.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (m_ParentItem.Displayed && BaseItem.IsHandleValid(val))
				{
					m_ParentItemScreenPos = val.PointToScreen(new Point(m_ParentItem.LeftInternal, m_ParentItem.TopInternal));
					val = null;
				}
				if (val is ItemControl)
				{
					m_AntiAlias = ((ItemControl)(object)val).AntiAlias;
				}
			}
		}
	}

	private bool IsRoundRegion
	{
		get
		{
			if (m_ParentItem != null)
			{
				if ((m_ParentItem is ApplicationButton && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Windows7) || m_ParentItem.EffectiveStyle == eDotNetBarStyle.Metro)
				{
					return false;
				}
				BaseItem baseItem = m_ParentItem;
				while (baseItem.Parent != null)
				{
					baseItem = baseItem.Parent;
				}
				object containerControl = baseItem.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is RibbonStrip || val is RibbonBar || (val is ContextMenuBar && BarFunctions.IsOffice2007Style(((ContextMenuBar)(object)val).Style)))
				{
					return true;
				}
			}
			return false;
		}
	}

	private bool IsContextMenu
	{
		get
		{
			if (m_ParentItem == null)
			{
				return false;
			}
			if (m_ParentItem.Parent == null || m_ParentItem.ContainerControl is ContextMenuBar)
			{
				return true;
			}
			BaseItem parent = m_ParentItem.Parent;
			while (parent.Parent != null)
			{
				parent = parent.Parent;
			}
			if (parent is PopupItem)
			{
				return true;
			}
			return false;
		}
	}

	private int ClientMarginLeft
	{
		get
		{
			int num = 0;
			if (m_ParentItem != null && IsGradientStyle)
			{
				num = 1;
				if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					num++;
				}
				if (IsContainerMenu)
				{
					num--;
				}
			}
			else
			{
				num = 3;
			}
			return num;
		}
	}

	private int ClientMarginTop
	{
		get
		{
			int num = 0;
			num = ((m_ParentItem == null || !IsGradientStyle) ? 3 : 2);
			if (IsContainerMenu)
			{
				num--;
			}
			return num;
		}
	}

	private int ClientMarginRight
	{
		get
		{
			bool flag = true;
			int num = 0;
			if (m_Owner is IOwnerMenuSupport { ShowPopupShadow: false })
			{
				flag = false;
			}
			if (m_ParentItem != null && IsGradientStyle)
			{
				num = ((AlphaShadow || !flag) ? 1 : 3);
				if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					num++;
				}
				if (IsContainerMenu)
				{
					num--;
				}
			}
			else
			{
				num = 3;
			}
			return num;
		}
	}

	private int ClientMarginBottom
	{
		get
		{
			IOwnerMenuSupport ownerMenuSupport = m_Owner as IOwnerMenuSupport;
			bool flag = true;
			int num = 0;
			if (ownerMenuSupport != null && !ownerMenuSupport.ShowPopupShadow)
			{
				flag = false;
			}
			if (m_ParentItem != null && IsGradientStyle)
			{
				num = ((!AlphaShadow && flag) ? 4 : 2);
				if (IsContainerMenu)
				{
					num--;
				}
			}
			else
			{
				num = 3;
			}
			return num;
		}
	}

	internal bool DisplayShadow
	{
		get
		{
			if (!m_PopupMenu || (m_ParentItem != null && m_ParentItem.Site != null && m_ParentItem.Site.DesignMode))
			{
				return false;
			}
			if (m_Owner is IOwnerMenuSupport ownerMenuSupport)
			{
				if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if (ownerMenuSupport.MenuDropShadow == eMenuDropShadow.Show)
					{
						return true;
					}
					return false;
				}
				return ownerMenuSupport.ShowPopupShadow;
			}
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return false;
			}
			return true;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			m_AntiAlias = value;
		}
	}

	internal bool AlphaShadow
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				return false;
			}
			if (m_Owner is IOwnerMenuSupport { AlphaBlendShadow: false })
			{
				return false;
			}
			return true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether Tooltips are shown on Bar and it's sub-items.")]
	public bool ShowToolTips
	{
		get
		{
			return m_ShowToolTips;
		}
		set
		{
			m_ShowToolTips = value;
		}
	}

	public SideBarImage SideBar
	{
		get
		{
			return m_SideBarImage;
		}
		set
		{
			m_SideBarImage = value;
		}
	}

	public object Owner
	{
		get
		{
			return m_Owner;
		}
		set
		{
			m_Owner = value;
		}
	}

	internal bool IsCustomizeMenu
	{
		get
		{
			return m_IsCustomizeMenu;
		}
		set
		{
			m_IsCustomizeMenu = value;
			if (m_ParentItem == null)
			{
				return;
			}
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				subItem.SetIsOnCustomizeMenu(m_IsCustomizeMenu);
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	public ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return m_PersonalizedMenus;
		}
		set
		{
			m_PersonalizedMenus = value;
		}
	}

	public bool PersonalizedAllVisible
	{
		get
		{
			return m_ExpandButton.PersonalizedAllVisible;
		}
		set
		{
			m_ExpandButton.PersonalizedAllVisible = value;
		}
	}

	public ePopupAnimation PopupAnimation
	{
		get
		{
			return m_PopupAnimation;
		}
		set
		{
			m_PopupAnimation = value;
		}
	}

	public ColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			m_ColorScheme = value;
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	public BaseItem HotSubItem
	{
		get
		{
			return m_HotSubItem;
		}
		set
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
			}
			m_HotSubItem = null;
			if (ParentItem.SubItems.Contains(value) || IsChildItem(value))
			{
				m_HotSubItem = value;
				if (m_HotSubItem != null)
				{
					m_HotSubItem.InternalMouseEnter();
					m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 1, m_HotSubItem.TopInternal + 1, 0));
				}
			}
		}
	}

	public MenuPanel()
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		m_ParentItem = null;
		m_HotSubItem = null;
		m_Hover = false;
		m_Scroll = false;
		m_TopScroll = false;
		m_BottomScroll = false;
		m_ScrollTopPosition = 0;
		m_TopScrollHeight = 0;
		m_BottomScrollHeight = 0;
		m_ClientRect = default(Rectangle);
		m_SideRect = default(Rectangle);
		m_SideBarImage = default(SideBarImage);
		m_IsCustomizeMenu = false;
		m_Owner = null;
		((Control)this).Font = SystemInformation.MenuFont;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).IsAccessible = true;
		((Control)this).AccessibleRole = (AccessibleRole)11;
	}

	protected override void Dispose(bool disposing)
	{
		if (((Control)this).Parent != null && ((Control)this).Parent is PopupContainer)
		{
			Control parent = ((Control)this).Parent;
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			((Component)(object)parent).Dispose();
			parent = null;
		}
		if (m_AccessibleObjectCreated && ((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)32769);
		}
		if (m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		((Control)this).Font = null;
		RestoreContainer();
		m_ParentItem = null;
		((Control)this).Dispose(disposing);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
			return;
		}
		if (((Message)(ref m)).Msg == 1131 && m_DoDefaultActionItem != null)
		{
			m_DoDefaultActionItem.DoAccesibleDefaultAction();
			m_DoDefaultActionItem = null;
		}
		((Control)this).WndProc(ref m);
	}

	private BaseRenderer GetRenderer()
	{
		BaseRenderer result = GlobalManager.Renderer;
		if (m_ParentItem != null && m_ParentItem.GetContainerControl(checkIsContainer: false) is IRenderingSupport)
		{
			BaseRenderer renderer = ((IRenderingSupport)m_ParentItem.GetContainerControl(checkIsContainer: false)).GetRenderer();
			if (renderer != null)
			{
				result = renderer;
			}
		}
		return result;
	}

	protected virtual void OnShowKeyTipsChanged()
	{
		m_KeyTipsKeysStack = "";
		if (ShowKeyTips)
		{
			CreateKeyTipCanvas();
		}
		else
		{
			DestroyKeyTipCanvas();
		}
	}

	protected virtual void CreateKeyTipCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).BringToFront();
			return;
		}
		m_KeyTipsCanvas = new KeyTipsCanvasControl(this);
		((Control)m_KeyTipsCanvas).Bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		((Control)m_KeyTipsCanvas).Visible = true;
		((Control)this).Controls.Add((Control)(object)m_KeyTipsCanvas);
		((Control)m_KeyTipsCanvas).BringToFront();
	}

	protected virtual void DestroyKeyTipCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).Visible = false;
			((Control)this).Controls.Remove((Control)(object)m_KeyTipsCanvas);
			((Component)(object)m_KeyTipsCanvas).Dispose();
			m_KeyTipsCanvas = null;
		}
	}

	void IKeyTipsRenderer.PaintKeyTips(Graphics g)
	{
		PaintKeyTips(g);
	}

	protected virtual void PaintKeyTips(Graphics g)
	{
		if (m_ShowKeyTips && m_ParentItem != null)
		{
			KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			PaintContainerKeyTips(m_ParentItem, renderer, e);
		}
	}

	protected virtual Font GetKeyTipFont()
	{
		Font result = ((Control)this).Font;
		if (m_KeyTipsFont != null)
		{
			result = m_KeyTipsFont;
		}
		return result;
	}

	internal virtual void PaintContainerKeyTips(BaseItem container, BaseRenderer renderer, KeyTipsRendererEventArgs e)
	{
		foreach (BaseItem subItem in container.SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem.IsContainer)
			{
				PaintContainerKeyTips(subItem, renderer, e);
			}
			if ((subItem.AccessKey != 0 || !(subItem.KeyTips == "")) && (!(m_KeyTipsKeysStack != "") || subItem.KeyTips.StartsWith(m_KeyTipsKeysStack)) && (!(subItem.KeyTips == "") || !(m_KeyTipsKeysStack != "")))
			{
				if (subItem.KeyTips != "")
				{
					e.KeyTip = subItem.KeyTips;
				}
				else
				{
					e.KeyTip = subItem.AccessKey.ToString().ToUpper();
				}
				e.Bounds = GetKeyTipRectangle(e.Graphics, subItem, e.Font, e.KeyTip);
				e.ReferenceObject = subItem;
				renderer.DrawKeyTips(e);
			}
		}
	}

	protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size keyTipsPadding = KeyTipsPainter.KeyTipsPadding;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += keyTipsPadding.Width;
		size.Height += keyTipsPadding.Height;
		Rectangle displayRectangle = item.DisplayRectangle;
		return new Rectangle(displayRectangle.X + 16, displayRectangle.Bottom - size.Height, size.Width, size.Height);
	}

	public virtual bool ProcessMnemonicEx(char charCode)
	{
		if (ProcessContainerAccessKey(m_ParentItem, charCode))
		{
			return true;
		}
		return false;
	}

	public void Close()
	{
		if (m_AccessibleObjectCreated && ((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			if (m_ParentItem != null && m_ParentItem.IsOnMenuBar)
			{
				popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)5);
			}
			popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)7);
		}
		SetFocusItem(null);
		Hide();
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		m_AccessibleObjectCreated = true;
		return (AccessibleObject)(object)new PopupMenuAccessibleObject(this);
	}

	private void SetupAccessibility()
	{
		if (m_ParentItem != null && m_ParentItem.Text != "")
		{
			((Control)this).AccessibleName = m_ParentItem.Text;
		}
		else
		{
			((Control)this).AccessibleName = "DotNetBar Popup Menu";
		}
		((Control)this).AccessibleDescription = "";
		((Control)this).AccessibleRole = (AccessibleRole)11;
	}

	internal ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = ((m_ColorScheme != null) ? new ItemPaintArgs(m_Owner as IOwner, (Control)(object)this, g, m_ColorScheme) : new ItemPaintArgs(m_Owner as IOwner, (Control)(object)this, g, new ColorScheme(g)));
		itemPaintArgs.Renderer = GetRenderer();
		if (m_ParentItem.DesignMode)
		{
			ISite site = GetSite();
			if (site != null && site.DesignMode)
			{
				itemPaintArgs.DesignerSelection = true;
			}
		}
		return itemPaintArgs;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (m_ParentItem != null)
		{
			ItemPaintArgs itemPaintArgs = GetItemPaintArgs(e.Graphics);
			itemPaintArgs.ClipRectangle = e.ClipRectangle;
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
			if (m_AntiAlias)
			{
				e.Graphics.SmoothingMode = (SmoothingMode)4;
				e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				PaintOffice(itemPaintArgs);
			}
			else
			{
				PaintDotNet(itemPaintArgs);
			}
			if (m_ParentItem is ApplicationButton)
			{
				((ApplicationButton)m_ParentItem).OnMenuPaint(itemPaintArgs);
			}
			else if (m_ParentItem is MetroAppButton)
			{
				((MetroAppButton)m_ParentItem).OnMenuPaint(itemPaintArgs);
			}
			e.Graphics.SmoothingMode = smoothingMode;
			e.Graphics.TextRenderingHint = textRenderingHint;
		}
	}

	private InternalMenuColors GetMenuColors(ItemPaintArgs pa)
	{
		InternalMenuColors internalMenuColors = new InternalMenuColors();
		if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && pa.Renderer is Office2007Renderer)
		{
			Office2007MenuColorTable menu = ((Office2007Renderer)pa.Renderer).ColorTable.Menu;
			internalMenuColors.Background = menu.Background;
			internalMenuColors.Border = menu.Border;
			internalMenuColors.Side = menu.Side;
			internalMenuColors.SideBorder = menu.SideBorder;
			internalMenuColors.SideBorderLight = menu.SideBorderLight;
			internalMenuColors.SideUnused = menu.SideUnused;
		}
		else
		{
			internalMenuColors.Background.Start = pa.Colors.MenuBackground;
			internalMenuColors.Background.End = pa.Colors.MenuBackground2;
			internalMenuColors.Background.GradientAngle = pa.Colors.MenuBackgroundGradientAngle;
			internalMenuColors.Side.Start = pa.Colors.MenuSide;
			internalMenuColors.Side.End = pa.Colors.MenuSide2;
			internalMenuColors.Side.GradientAngle = pa.Colors.MenuSideGradientAngle;
			internalMenuColors.SideUnused.Start = pa.Colors.MenuUnusedSide;
			internalMenuColors.SideUnused.End = pa.Colors.MenuUnusedSide2;
			internalMenuColors.SideUnused.GradientAngle = pa.Colors.MenuUnusedSideGradientAngle;
			internalMenuColors.Border.Start = pa.Colors.MenuBorder;
			internalMenuColors.SideBorder.Start = pa.Colors.ItemSeparator;
			internalMenuColors.SideBorderLight.Start = pa.Colors.ItemSeparatorShade;
		}
		return internalMenuColors;
	}

	protected void PaintDotNet(ItemPaintArgs pa)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (DisplayShadow && !AlphaShadow)
		{
			SetupRegion();
		}
		InternalMenuColors menuColors = GetMenuColors(pa);
		DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, menuColors.Background);
		PaintSideBar(pa);
		if (m_ParentItem != null)
		{
			Rectangle rectangle = ((Control)this).ClientRectangle;
			if (DisplayShadow && !AlphaShadow)
			{
				rectangle = new Rectangle(0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
			}
			eDotNetBarStyle effectiveStyle = m_ParentItem.EffectiveStyle;
			if (BarFunctions.IsOffice2007Style(effectiveStyle) && effectiveStyle != eDotNetBarStyle.Metro)
			{
				if (((Control)this).Region != null)
				{
					DisplayHelp.DrawRoundedRectangle(graphics, menuColors.Border.Start, rectangle, m_CornerSize);
					Rectangle rectangle2 = rectangle;
					rectangle2.Inflate(-1, -1);
					Color color = ControlPaint.LightLight(menuColors.Background.Start);
					if (IsContainerMenu)
					{
						color = Color.Empty;
						GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(rectangle2, m_CornerSize);
						Region val = new Region(roundedRectanglePath);
						roundedRectanglePath.Widen(SystemPens.Control);
						val.Union(roundedRectanglePath);
						roundedRectanglePath.Dispose();
						roundedRectanglePath = null;
						graphics.Clip = val;
					}
					if (!color.IsEmpty)
					{
						Pen val2 = new Pen(color);
						try
						{
							DisplayHelp.DrawRoundedRectangle(graphics, val2, rectangle2, m_CornerSize);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
				else
				{
					DisplayHelp.DrawRectangle(graphics, menuColors.Border.Start, rectangle);
				}
			}
			else
			{
				DisplayHelp.DrawGradientRectangle(graphics, rectangle, menuColors.Border, 1);
			}
			if (DisplayShadow && !AlphaShadow && m_ParentItem != null && !BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				Pen val3 = new Pen(SystemColors.ControlDark, 2f);
				try
				{
					Point[] array = new Point[3];
					array[0].X = 2;
					array[0].Y = ((Control)this).ClientSize.Height - 1;
					array[1].X = ((Control)this).ClientSize.Width - 1;
					array[1].Y = ((Control)this).ClientSize.Height - 1;
					array[2].X = ((Control)this).ClientSize.Width - 1;
					array[2].Y = 2;
					graphics.DrawLines(val3, array);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if (effectiveStyle != eDotNetBarStyle.Metro && (!BarFunctions.IsOffice2007Style(effectiveStyle) || effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro) && m_ParentItem is ButtonItem && m_ParentItem.Displayed && m_ParentItem.Visible && m_ParentItem.Orientation == eOrientation.Horizontal && !(m_ParentItem.ContainerControl is ContextMenuBar) && m_PopupMenu && ((Control)this).Parent != null && m_ParentItemScreenPos.Y < ((Control)this).Parent.Location.Y)
			{
				Point point = new Point(m_ParentItemScreenPos.X - ((Control)this).Parent.Location.X + 1, 0);
				Point point2 = new Point(point.X + m_ParentItem.WidthInternal - (pa.Colors.ItemExpandedShadow.IsEmpty ? 3 : 5), 0);
				Pen val4 = new Pen(pa.Colors.ItemExpandedBackground, 1f);
				try
				{
					graphics.DrawLine(val4, point, point2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			if (m_Scroll)
			{
				BaseItem baseItem = m_ParentItem.SubItems[0];
				if (m_TopScroll)
				{
					if (m_ParentItem is ImageItem imageItem)
					{
						Rectangle rectangle3 = new Rectangle(m_ClientRect.Left, m_ClientRect.Top, imageItem.SubItemsImageSize.Width + 7, m_TopScrollHeight);
						DisplayHelp.FillRectangle(graphics, rectangle3, menuColors.Side);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle3, menuColors);
						}
					}
					else
					{
						Rectangle rectangle4 = new Rectangle(m_ClientRect.Left, m_ClientRect.Top, 23, m_TopScrollHeight);
						DisplayHelp.FillRectangle(graphics, rectangle4, menuColors.Side);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle4, menuColors);
						}
					}
				}
				if (m_BottomScroll)
				{
					if (m_ParentItem is ImageItem imageItem2)
					{
						Rectangle rectangle5 = new Rectangle(m_ClientRect.Left, m_ClientRect.Bottom - m_BottomScrollHeight - 1, imageItem2.SubItemsImageSize.Width + 7, m_BottomScrollHeight);
						DisplayHelp.FillRectangle(graphics, rectangle5, menuColors.Side);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle5, menuColors);
						}
					}
					else
					{
						Rectangle rectangle6 = new Rectangle(m_ClientRect.Left, m_ClientRect.Bottom - m_BottomScrollHeight - 1, 23, m_BottomScrollHeight);
						DisplayHelp.FillRectangle(graphics, rectangle6, menuColors.Side);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle6, menuColors);
						}
					}
				}
				baseItem = null;
			}
			PaintItems(pa, menuColors);
		}
		else
		{
			DisplayHelp.DrawGradientRectangle(graphics, ((Control)this).ClientRectangle, menuColors.Border, 1);
		}
	}

	protected void PaintOffice(ItemPaintArgs pa)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(SystemColors.Control);
		try
		{
			pa.Graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		PaintSideBar(pa);
		ControlPaint.DrawBorder3D(pa.Graphics, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
		if (m_ParentItem != null)
		{
			PaintItems(pa, GetMenuColors(pa));
		}
	}

	private bool DoPaintSide(BaseItem item)
	{
		if (item is LabelItem || item is ItemContainer)
		{
			return false;
		}
		return true;
	}

	private void PaintItems(ItemPaintArgs pa, InternalMenuColors menuColors)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = pa.Graphics;
		bool flag = (int)((Control)this).RightToLeft == 1;
		if (m_ParentItem == null || m_ParentItem == null)
		{
			return;
		}
		Rectangle rectangle = new Rectangle((int)graphics.ClipBounds.X, (int)graphics.ClipBounds.Y, (int)graphics.ClipBounds.Width, (int)graphics.ClipBounds.Height);
		if (m_Scroll)
		{
			if (m_TopScroll)
			{
				Point[] array = new Point[3];
				array[0].X = m_ClientRect.Left + (m_ClientRect.Width - 8) / 2;
				array[0].Y = m_ClientRect.Top + 8;
				array[1].X = array[0].X + 8;
				array[1].Y = array[0].Y;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y - 5;
				graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
			ImageItem imageItem = m_ParentItem as ImageItem;
			int num = 16;
			if (imageItem != null)
			{
				num = imageItem.SubItemsImageSize.Width;
			}
			imageItem = null;
			bool flag2 = false;
			for (int i = m_ScrollTopPosition; i < m_ParentItem.SubItems.Count; i++)
			{
				BaseItem baseItem = m_ParentItem.SubItems[i];
				if (!rectangle.IntersectsWith(baseItem.DisplayRectangle) || (!baseItem.Visible && !m_IsCustomizeMenu) || !baseItem.Displayed)
				{
					continue;
				}
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				if (baseItem.BeginGroup && baseItem.TopInternal > ClientMarginTop)
				{
					if (baseItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if (m_IsCustomizeMenu)
						{
							ControlPaint.DrawBorder3D(graphics, baseItem.HeightInternal + 2 + baseItem.LeftInternal + 13, baseItem.TopInternal - 5, baseItem.WidthInternal - 26 - baseItem.HeightInternal - 2, 2, (Border3DStyle)6, (Border3DSide)2);
						}
						else
						{
							ControlPaint.DrawBorder3D(graphics, baseItem.LeftInternal + 13, baseItem.TopInternal - 5, baseItem.WidthInternal - 26, 2, (Border3DStyle)6, (Border3DSide)2);
						}
					}
					else if (m_IsCustomizeMenu)
					{
						Rectangle rectangle2 = new Rectangle(baseItem.LeftInternal, baseItem.TopInternal - 3, baseItem.HeightInternal + 2 + num + 7, 3);
						rectangle2.Inflate(0, 1);
						DisplayHelp.FillRectangle(graphics, rectangle2, menuColors.Side.Start, menuColors.Side.End, menuColors.Side.GradientAngle);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle2, menuColors);
						}
						DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + num + 8 + 7 + baseItem.HeightInternal + 2, baseItem.TopInternal - 2, baseItem.DisplayRectangle.Right - 1, baseItem.TopInternal - 2, menuColors.SideBorder.Start, 1);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + num + 8 + 7 + baseItem.HeightInternal + 2, baseItem.TopInternal - 1, baseItem.DisplayRectangle.Right - 1, baseItem.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
						}
					}
					else
					{
						bool flag3 = DoPaintSide(baseItem);
						bool flag4 = flag3;
						if (flag3)
						{
							Rectangle rectangle3 = new Rectangle(baseItem.LeftInternal, baseItem.TopInternal - 3, num + 7, 3);
							rectangle3.Inflate(0, 1);
							if (baseItem is IPersonalizedMenuItem && ((IPersonalizedMenuItem)baseItem).MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !((IPersonalizedMenuItem)baseItem).RecentlyUsed)
							{
								DisplayHelp.FillRectangle(graphics, rectangle3, menuColors.SideUnused);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle3, menuColors.Side);
							}
							if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
							{
								PaintMenuItemSide(pa, rectangle3, menuColors);
							}
						}
						if (flag4 && flag2)
						{
							flag4 = false;
						}
						DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + (flag4 ? (num + 8 + 7) : 0), baseItem.TopInternal - 2, baseItem.DisplayRectangle.Right - 1, baseItem.TopInternal - 2, menuColors.SideBorder.Start, 1);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + (flag4 ? (num + 8 + 7) : 0), baseItem.TopInternal - 1, baseItem.DisplayRectangle.Right - 1, baseItem.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
						}
					}
				}
				graphics.SmoothingMode = smoothingMode;
				baseItem.Paint(pa);
				flag2 = baseItem.IsContainer;
			}
			if (m_BottomScroll)
			{
				Point[] array = new Point[3];
				array[0].X = m_ClientRect.Left + (m_ClientRect.Width - 8) / 2;
				array[0].Y = m_ClientRect.Bottom - 8;
				array[1].X = array[0].X + 8;
				array[1].Y = array[0].Y;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y + 4;
				graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
		}
		else
		{
			ImageItem imageItem2 = m_ParentItem as ImageItem;
			int num2 = 16;
			if (imageItem2 != null)
			{
				num2 = imageItem2.SubItemsImageSize.Width;
			}
			imageItem2 = null;
			bool flag5 = true;
			bool flag6 = false;
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if (!rectangle.IntersectsWith(subItem.DisplayRectangle) || ((!subItem.Visible || !subItem.Displayed) && !m_IsCustomizeMenu))
				{
					continue;
				}
				SmoothingMode smoothingMode2 = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				if (subItem.BeginGroup && !flag5)
				{
					if (subItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if (m_IsCustomizeMenu)
						{
							ControlPaint.DrawBorder3D(graphics, subItem.HeightInternal + 2 + subItem.LeftInternal + 13, subItem.TopInternal - 5, subItem.WidthInternal - 26 - subItem.HeightInternal - 2, 2, (Border3DStyle)6, (Border3DSide)2);
						}
						else
						{
							ControlPaint.DrawBorder3D(graphics, subItem.LeftInternal + 13, subItem.TopInternal - 5, subItem.WidthInternal - 26, 2, (Border3DStyle)6, (Border3DSide)2);
						}
					}
					else if (m_IsCustomizeMenu)
					{
						Rectangle rectangle4 = new Rectangle(subItem.LeftInternal, subItem.TopInternal - 3, subItem.HeightInternal + 2 + num2 + 7, 3);
						rectangle4.Inflate(0, 1);
						if (flag)
						{
							rectangle4.Width = num2 + 7;
							rectangle4.X = subItem.DisplayRectangle.Right - rectangle4.Width;
						}
						DisplayHelp.FillRectangle(graphics, rectangle4, menuColors.Side);
						if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
						{
							PaintMenuItemSide(pa, rectangle4, menuColors);
						}
						if (flag)
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - (num2 + 8 + 7 + subItem.HeightInternal + 2 + 1), subItem.TopInternal - 2, menuColors.SideBorder.Start, 1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 1, subItem.DisplayRectangle.Right - (num2 + 8 + 7 + subItem.HeightInternal + 2 + 1), subItem.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
							}
						}
						else
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + num2 + 8 + 7 + subItem.HeightInternal + 2, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 2, menuColors.SideBorder.Start, 1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + num2 + 8 + 7 + subItem.HeightInternal + 2, subItem.TopInternal - 1, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
							}
						}
					}
					else
					{
						Rectangle rectangle5 = new Rectangle(subItem.LeftInternal, subItem.TopInternal - 2, num2 + 7, 3);
						rectangle5.Inflate(0, 1);
						if (flag)
						{
							rectangle5.X = subItem.DisplayRectangle.Right - rectangle5.Width - 1;
							rectangle5.Width++;
						}
						bool flag7 = DoPaintSide(subItem);
						bool flag8 = flag7;
						if (flag8 && flag6)
						{
							flag7 = false;
							flag8 = false;
						}
						if (flag7)
						{
							if (subItem is IPersonalizedMenuItem && ((IPersonalizedMenuItem)subItem).MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !((IPersonalizedMenuItem)subItem).RecentlyUsed)
							{
								DisplayHelp.FillRectangle(graphics, rectangle5, menuColors.SideUnused);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle5, menuColors.Side);
							}
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								PaintMenuItemSide(pa, rectangle5, menuColors);
							}
						}
						if (flag)
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - (flag8 ? (num2 + 8 + 7 + 1) : 0), subItem.TopInternal - 2, menuColors.SideBorder.Start, 1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 1, subItem.DisplayRectangle.Right - (flag8 ? (num2 + 8 + 7 + 1) : 0), subItem.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
							}
						}
						else
						{
							if (subItem.WideBeginGroup)
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 2, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - 3, subItem.TopInternal - 2, pa.Colors.ItemSeparator, 1);
							}
							else
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + (flag8 ? (num2 + 8 + 7) : 0), subItem.TopInternal - 2, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 2, pa.Colors.ItemSeparator, 1);
							}
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle) && !pa.Colors.ItemSeparatorShade.IsEmpty)
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + (flag8 ? (num2 + 8 + 7) : 0), subItem.TopInternal - 1, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 1, pa.Colors.ItemSeparatorShade, 1);
							}
						}
					}
				}
				graphics.SmoothingMode = smoothingMode2;
				flag5 = false;
				subItem.Paint(pa);
				subItem.Displayed = true;
				flag6 = subItem.IsContainer;
			}
		}
		PaintExpandButton(pa, menuColors);
	}

	private void PaintMenuItemSide(ItemPaintArgs pa, Rectangle sideRect, InternalMenuColors menuColors)
	{
		if (!(pa.Owner is DotNetBarManager))
		{
			Graphics graphics = pa.Graphics;
			if (pa.RightToLeft)
			{
				Point point = new Point(sideRect.X, sideRect.Y);
				DisplayHelp.DrawLine(graphics, point.X, point.Y, point.X, point.Y + sideRect.Height, menuColors.SideBorderLight.Start, 1);
				point.X++;
				DisplayHelp.DrawLine(graphics, point.X, point.Y, point.X, point.Y + sideRect.Height, menuColors.SideBorder.Start, 1);
			}
			else
			{
				Point point2 = new Point(sideRect.Right - 2, sideRect.Y);
				DisplayHelp.DrawLine(graphics, point2.X, point2.Y, point2.X, point2.Y + sideRect.Height, menuColors.SideBorder.Start, 1);
				point2.X++;
				DisplayHelp.DrawLine(graphics, point2.X, point2.Y, point2.X, point2.Y + sideRect.Height, menuColors.SideBorderLight.Start, 1);
			}
		}
	}

	private void RefreshExpandButton()
	{
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			ItemPaintArgs pa = ((m_ColorScheme != null) ? new ItemPaintArgs(m_Owner as IOwner, (Control)(object)this, val, m_ColorScheme) : new ItemPaintArgs(m_Owner as IOwner, (Control)(object)this, val, new ColorScheme(val)));
			PaintExpandButton(pa, GetMenuColors(pa));
		}
		finally
		{
			val.Dispose();
		}
	}

	private void PaintExpandButton(ItemPaintArgs pa, InternalMenuColors menuColors)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Invalid comparison between Unknown and I4
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		if (!m_ExpandButton.ShowExpandButton || m_ParentItem == null)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		ImageItem imageItem = m_ParentItem as ImageItem;
		int num = 16;
		if (imageItem != null && imageItem.SubItemsImageSize.Width > num)
		{
			num = imageItem.SubItemsImageSize.Width;
		}
		num += 7;
		imageItem = null;
		if (IsGradientStyle)
		{
			if (m_ExpandButton.MouseOver)
			{
				Rectangle rect = m_ExpandButton.Rect;
				DisplayHelp.FillRectangle(graphics, rect, pa.Colors.MenuBackground, Color.Empty);
				rect.Inflate(-1, 0);
				if ((int)Control.MouseButtons == 1048576)
				{
					DisplayHelp.FillRectangle(graphics, rect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, rect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				}
				NativeFunctions.DrawRectangle(graphics, SystemPens.Highlight, rect);
			}
			else
			{
				Rectangle bounds = new Rectangle(m_ExpandButton.Rect.Left + num, m_ExpandButton.Rect.Top, m_ExpandButton.Rect.Width - num, m_ExpandButton.Rect.Height);
				DisplayHelp.FillRectangle(graphics, bounds, pa.Colors.MenuBackground, Color.Empty);
				Rectangle rectangle = new Rectangle(m_ExpandButton.Rect.Left, m_ExpandButton.Rect.Top, num, m_ExpandButton.Rect.Height);
				DisplayHelp.FillRectangle(graphics, rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
				if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					PaintMenuItemSide(pa, rectangle, menuColors);
				}
			}
			if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
			{
				GraphicsPath val = new GraphicsPath();
				Point point = new Point(m_ExpandButton.Rect.X + (m_ExpandButton.Rect.Width - 16) / 2, m_ExpandButton.Rect.Y + (m_ExpandButton.Rect.Height - 16) / 2);
				val.AddEllipse(point.X, point.Y, 16, 16);
				PathGradientBrush val2 = new PathGradientBrush(val);
				val2.CenterColor = pa.Colors.MenuSide;
				val2.SurroundColors = new Color[1] { pa.Colors.MenuSide2 };
				val2.CenterPoint = new Point(point.X + 3, point.Y + 3);
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.FillEllipse((Brush)(object)val2, point.X, point.Y, 16, 16);
				graphics.SmoothingMode = smoothingMode;
			}
		}
		else if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			if (m_ExpandButton.MouseOver)
			{
				graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()), m_ExpandButton.Rect);
				if ((int)Control.MouseButtons == 1048576)
				{
					ControlPaint.DrawBorder(graphics, m_ExpandButton.Rect, SystemColors.Control, (ButtonBorderStyle)4);
				}
				else
				{
					ControlPaint.DrawBorder3D(graphics, m_ExpandButton.Rect, (Border3DStyle)4, (Border3DSide)15);
				}
			}
			else
			{
				graphics.FillRectangle(SystemBrushes.Control, m_ExpandButton.Rect);
			}
		}
		Point[] array = new Point[3];
		array[0].X = m_ExpandButton.Rect.Left + (m_ExpandButton.Rect.Width - 4) / 2;
		if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
		{
			array[0].Y = m_ExpandButton.Rect.Top + (m_ExpandButton.Rect.Height - 7) / 2;
		}
		else
		{
			array[0].Y = m_ExpandButton.Rect.Top + 3;
		}
		array[1].X = array[0].X + 2;
		array[1].Y = array[0].Y + 2;
		array[2].X = array[0].X + 4;
		array[2].Y = array[0].Y;
		graphics.DrawLines(SystemPens.ControlText, array);
		array[0].Y++;
		array[1].Y++;
		array[2].Y++;
		graphics.DrawLines(SystemPens.ControlText, array);
		array[0].Y += 3;
		array[1].Y += 3;
		array[2].Y += 3;
		graphics.DrawLines(SystemPens.ControlText, array);
		array[0].Y++;
		array[1].Y++;
		array[2].Y++;
		graphics.DrawLines(SystemPens.ControlText, array);
	}

	private void PaintSideBar(ItemPaintArgs pa)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (m_SideBarImage.Picture != null)
		{
			if (!m_SideBarImage.GradientColor1.IsEmpty && !m_SideBarImage.GradientColor2.IsEmpty)
			{
				PointF[] array = new PointF[2];
				array[0].X = m_SideRect.Left;
				array[0].Y = m_SideRect.Top;
				array[1].X = m_SideRect.Left;
				array[1].Y = m_SideRect.Bottom;
				LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(m_SideRect, m_SideBarImage.GradientColor1, m_SideBarImage.GradientColor2, m_SideBarImage.GradientAngle);
				graphics.FillRectangle((Brush)(object)val, m_SideRect);
			}
			else if (!m_SideBarImage.BackColor.Equals((object?)Color.Empty))
			{
				graphics.FillRectangle((Brush)new SolidBrush(m_SideBarImage.BackColor), m_SideRect);
			}
			if (m_SideBarImage.StretchPicture)
			{
				graphics.DrawImage(m_SideBarImage.Picture, m_SideRect);
			}
			else if (m_SideBarImage.Alignment == eAlignment.Top)
			{
				graphics.DrawImage(m_SideBarImage.Picture, m_SideRect.X, m_SideRect.Top, m_SideRect.Width, m_SideBarImage.Picture.Height);
			}
			else if (m_SideBarImage.Alignment == eAlignment.Bottom)
			{
				graphics.DrawImage(m_SideBarImage.Picture, m_SideRect.Left, m_SideRect.Bottom - m_SideBarImage.Picture.Height, m_SideBarImage.Picture.Width, m_SideBarImage.Picture.Height);
			}
			else
			{
				graphics.DrawImage(m_SideBarImage.Picture, m_SideRect.Left, m_SideRect.Top + (m_SideRect.Height - m_SideBarImage.Picture.Height) / 2, m_SideBarImage.Picture.Width, m_SideBarImage.Picture.Height);
			}
		}
	}

	private void SetupRegion()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (m_ParentItem != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			Region val = new Region(rectangle);
			rectangle.X = ((Control)this).Width - 2;
			rectangle.Y = 0;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			rectangle.X = 0;
			rectangle.Y = ((Control)this).Height - 2;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			((Control)this).Region = val;
		}
	}

	public void RecalcLayout()
	{
		RecalcSize();
		((Control)this).Invalidate(true);
	}

	public void RecalcSize()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		if (m_ParentItem != null)
		{
			m_ParentItem.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				RecalcSizeOffice();
			}
			else
			{
				RecalcSizeDotNet();
			}
			if (((Control)this).Visible && m_PopupMenu && ((Control)this).Parent != null && ((Control)this).Parent is PopupContainer)
			{
				ScreenInformation screenInformation = BarFunctions.ScreenFromControl(((Control)this).Parent);
				if (screenInformation != null)
				{
					Rectangle rectangle = screenInformation.WorkingArea;
					if (UseWholeScreenForSizeChecking)
					{
						rectangle = screenInformation.Bounds;
					}
					if (((Control)this).Parent.Location.Y + ((Control)this).Size.Height > rectangle.Bottom)
					{
						m_ClientRect.Height -= ((Control)this).Parent.Location.Y + ((Control)this).Size.Height - rectangle.Bottom;
						((Control)this).Height = ((Control)this).Height - (((Control)this).Parent.Location.Y + ((Control)this).Size.Height - rectangle.Bottom);
						if (!m_Scroll)
						{
							m_Scroll = true;
							m_ScrollTopPosition = 0;
						}
						RepositionItems();
					}
					else
					{
						m_Scroll = false;
					}
				}
			}
		}
		if (!((Control)this).Visible)
		{
			return;
		}
		if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && IsRoundRegion)
		{
			if (((Control)this).Parent != null)
			{
				SetRoundRegion(((Control)this).Parent);
			}
			SetRoundRegion((Control)(object)this);
		}
		((Control)this).Refresh();
	}

	protected override void OnResize(EventArgs e)
	{
		if (((Control)this).Parent is PopupContainer)
		{
			((Control)this).Parent.Size = ((Control)this).Size;
		}
		((Control)this).OnResize(e);
	}

	private void RecalcSizeDotNet()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		m_ExpandButton.ShowExpandButton = false;
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			((Control)this).Height = 24;
			((Control)this).Width = m_ParentItem.WidthInternal + 16;
			return;
		}
		int num = 0;
		int num2 = ClientMarginTop;
		int num3 = ClientMarginLeft;
		int num4 = 0;
		bool flag = false;
		bool flag2 = (int)((Control)this).RightToLeft == 1;
		if (m_SideBarImage.Picture != null)
		{
			num3 += m_SideBarImage.Picture.Width;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.RecalcSize();
			if (subItem.Visible || m_IsCustomizeMenu)
			{
				if (!m_ParentItem.DesignMode && m_PersonalizedMenus != 0 && !m_ExpandButton.PersonalizedAllVisible && !m_IsCustomizeMenu && subItem is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed, RecentlyUsed: false })
				{
					subItem.Displayed = false;
					m_ExpandButton.ShowExpandButton = true;
					continue;
				}
				if (subItem.HeightInternal != num4 && subItem is ImageItem)
				{
					if (num4 > 0)
					{
						flag = true;
					}
					if (subItem.HeightInternal > num4)
					{
						num4 = subItem.HeightInternal;
					}
				}
				if (subItem.BeginGroup && num2 > ClientMarginTop)
				{
					num2 += 3;
				}
				subItem.TopInternal = num2;
				subItem.LeftInternal = num3;
				num2 += subItem.HeightInternal;
				if (subItem.WidthInternal > num)
				{
					num = subItem.WidthInternal;
				}
				subItem.Displayed = true;
			}
			else
			{
				subItem.Displayed = false;
			}
		}
		if (num == 0)
		{
			num = 120;
		}
		if (flag)
		{
			num2 = ClientMarginTop;
			foreach (BaseItem subItem2 in m_ParentItem.SubItems)
			{
				subItem2.WidthInternal = num;
				if (subItem2.Displayed)
				{
					if (subItem2.BeginGroup && num2 > ClientMarginTop)
					{
						num2 += 3;
					}
					subItem2.TopInternal = num2;
					num2 += subItem2.HeightInternal;
				}
			}
		}
		else
		{
			foreach (BaseItem subItem3 in m_ParentItem.SubItems)
			{
				subItem3.WidthInternal = num;
			}
		}
		if (m_ExpandButton.ShowExpandButton)
		{
			m_ExpandButton.Rect = new Rectangle(num3, num2, num, EXPAND_BUTTON_HEIGHT);
			num2 += EXPAND_BUTTON_HEIGHT;
		}
		m_ClientRect = new Rectangle(num3, ClientMarginTop, num, num2 - ClientMarginTop);
		if (m_SideBarImage.Picture != null)
		{
			m_SideRect = new Rectangle(num3 - m_SideBarImage.Picture.Width, ClientMarginTop, m_SideBarImage.Picture.Width, num2 - ClientMarginTop);
		}
		((Control)this).Size = new Size(num3 + num + ClientMarginRight, num2 + ClientMarginBottom);
		if (((Control)this).Height == 92)
		{
			((Control)this).Height = 92;
		}
	}

	private void RecalcSizeOffice()
	{
		m_ExpandButton.ShowExpandButton = false;
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			((Control)this).Height = 24;
			((Control)this).Width = m_ParentItem.WidthInternal + 16;
			return;
		}
		int num = 0;
		int num2 = ClientMarginTop;
		int num3 = ClientMarginLeft;
		int num4 = 0;
		bool flag = false;
		if (m_SideBarImage.Picture != null)
		{
			num3 += m_SideBarImage.Picture.Width;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.RecalcSize();
			if (subItem.Visible || m_IsCustomizeMenu)
			{
				if (!m_ParentItem.DesignMode && m_PersonalizedMenus != 0 && !m_ExpandButton.PersonalizedAllVisible && !m_IsCustomizeMenu && subItem is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed, RecentlyUsed: false })
				{
					subItem.Displayed = false;
					m_ExpandButton.ShowExpandButton = true;
					continue;
				}
				if (subItem.HeightInternal != num4 && subItem is ImageItem)
				{
					if (num4 > 0)
					{
						flag = true;
					}
					if (subItem.HeightInternal > num4)
					{
						num4 = subItem.HeightInternal;
					}
				}
				if (subItem.BeginGroup && num2 > ClientMarginTop)
				{
					num2 += 9;
				}
				subItem.TopInternal = num2;
				subItem.LeftInternal = num3;
				num2 += subItem.HeightInternal;
				if (subItem.WidthInternal > num)
				{
					num = subItem.WidthInternal;
				}
				subItem.Displayed = true;
			}
			else
			{
				subItem.Displayed = false;
			}
		}
		if (num == 0)
		{
			num = 120;
		}
		if (flag)
		{
			num2 = 3;
			foreach (BaseItem subItem2 in m_ParentItem.SubItems)
			{
				subItem2.WidthInternal = num;
				if (subItem2.Displayed)
				{
					if (subItem2.BeginGroup && num2 > ClientMarginTop)
					{
						num2 += 9;
					}
					subItem2.TopInternal = num2;
					num2 += subItem2.HeightInternal;
				}
			}
		}
		else
		{
			foreach (BaseItem subItem3 in m_ParentItem.SubItems)
			{
				subItem3.WidthInternal = num;
			}
		}
		if (m_ExpandButton.ShowExpandButton)
		{
			m_ExpandButton.Rect = new Rectangle(num3, num2, num, EXPAND_BUTTON_HEIGHT);
			num2 += EXPAND_BUTTON_HEIGHT;
		}
		m_ClientRect = new Rectangle(num3, ClientMarginTop, num, num2 - ClientMarginTop);
		if (m_SideBarImage.Picture != null)
		{
			m_SideRect = new Rectangle(num3 - m_SideBarImage.Picture.Width, ClientMarginTop, m_SideBarImage.Picture.Width, num2);
		}
		((Control)this).Size = new Size(num3 + num + ClientMarginRight, num2 + ClientMarginBottom);
	}

	private void RestoreContainer()
	{
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			return;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.ContainerControl = null;
		}
		int num = 0;
		while (m_ParentItem != null && num < m_ParentItem.SubItems.Count)
		{
			m_ParentItem.SubItems[num].Expanded = false;
			num++;
		}
		num = 0;
		while (m_ParentItem != null && num < m_ParentItem.SubItems.Count)
		{
			m_ParentItem.SubItems[num].Displayed = false;
			num++;
		}
	}

	private BaseItem ExpandedItem()
	{
		if (m_ParentItem != null && m_ParentItem.SubItems.Count > 0)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if (subItem.Expanded && subItem is PopupItem)
				{
					return subItem;
				}
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ExpandedItem();
					if (baseItem2 is PopupItem)
					{
						return baseItem2;
					}
				}
			}
		}
		return null;
	}

	protected internal BaseItem FocusedItem()
	{
		if (m_ParentItem != null && m_ParentItem.SubItems.Count > 0)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if (subItem.Focused)
				{
					return subItem;
				}
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.FocusedItem();
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
			}
		}
		return null;
	}

	internal void SetFocusItem(BaseItem objFocus)
	{
		if (m_ParentItem != null && m_ParentItem.Parent == null)
		{
			BaseItem baseItem = FocusedItem();
			if (baseItem != objFocus)
			{
				baseItem?.OnLostFocus();
				objFocus?.OnGotFocus();
			}
		}
	}

	internal void InternalMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Invalid comparison between Unknown and I4
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Invalid comparison between Unknown and I4
		((Control)this).OnMouseMove(e);
		if (m_IgnoreDuplicateMouseMove)
		{
			if (m_LastMouseMoveEvent == null)
			{
				m_LastMouseMoveEvent = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
				return;
			}
			if (m_LastMouseMoveEvent.X == e.X && m_LastMouseMoveEvent.Y == e.Y && m_LastMouseMoveEvent.Button == e.Button)
			{
				return;
			}
			m_IgnoreDuplicateMouseMove = false;
			m_LastMouseMoveEvent = null;
		}
		if (m_ParentItem != null && m_ParentItem.DesignMode && (int)e.Button == 1048576 && (Math.Abs(e.X - m_MouseDownPt.X) >= 2 || Math.Abs(e.Y - m_MouseDownPt.Y) >= 2 || m_DragDropInProgress))
		{
			BaseItem baseItem = FocusedItem();
			IOwner owner = m_Owner as IOwner;
			ISite site = GetSite();
			if (site != null && baseItem != null)
			{
				DesignTimeMouseMove(e);
			}
			else if (baseItem != null && owner != null && baseItem.CanCustomize)
			{
				owner.StartItemDrag(baseItem);
			}
		}
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0 || m_ParentItem.DesignMode)
		{
			return;
		}
		BaseItem baseItem2 = ItemAtLocation(e.X, e.Y);
		BaseItem baseItem3 = ExpandedItem();
		if (baseItem3 != null && baseItem3 is PopupItem)
		{
			Control popupControl = ((PopupItem)baseItem3).PopupControl;
			if (popupControl != null)
			{
				Point pt = popupControl.PointToClient(((Control)this).PointToScreen(new Point(e.X, e.Y)));
				if (popupControl.ClientRectangle.Contains(pt) && popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).InternalMouseMove(new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));
					return;
				}
			}
		}
		if (baseItem3 != null && baseItem2 != baseItem3)
		{
			if ((baseItem3 is PopupItem && ((PopupItem)baseItem3).PopupType != 0) || (baseItem3 != null && m_ParentItem != null && m_ParentItem.SubItems.Count > 0 && m_ParentItem.SubItems[0] is ItemContainer && !(m_ParentItem is ApplicationButton) && !(m_ParentItem is MetroAppButton)))
			{
				baseItem3.Expanded = false;
			}
			else
			{
				CollapseItemDelayed(baseItem3);
			}
		}
		if (m_ExpandButton.ShowExpandButton)
		{
			if (baseItem2 == null && m_ExpandButton.Rect.Contains(e.X, e.Y) && !m_ExpandButton.MouseOver)
			{
				m_ExpandButton.MouseOver = true;
				RefreshExpandButton();
			}
			else if (baseItem2 != null && m_ExpandButton.MouseOver)
			{
				m_ExpandButton.MouseOver = false;
				RefreshExpandButton();
			}
		}
		if (baseItem2 == null && m_HotSubItem is GalleryContainer)
		{
			GalleryContainer galleryContainer = m_HotSubItem as GalleryContainer;
			if (!galleryContainer.SystemGallery && galleryContainer.PopupUsesStandardScrollbars && galleryContainer.ScrollBarControl != null && galleryContainer.ScrollBarControl.IsMouseDown && (int)e.Button == 1048576)
			{
				baseItem2 = m_HotSubItem;
			}
		}
		if (baseItem2 != m_HotSubItem)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (m_Hover)
				{
					ResetHover();
					m_Hover = false;
				}
			}
			if (baseItem2 != null)
			{
				baseItem2.InternalMouseEnter();
				baseItem2.InternalMouseMove(e);
				m_HotSubItem = baseItem2;
			}
			else
			{
				m_HotSubItem = null;
			}
		}
		else if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseMove(e);
		}
		if (((Control)this).Capture && !((Control)this).DisplayRectangle.Contains(e.X, e.Y) && m_ParentItem != null && m_ParentItem.SubItems.Count > 0 && m_ParentItem.SubItems[0] is GalleryContainer && m_ParentItem.SubItems[0].Visible && m_ParentItem.SubItems[0].Displayed)
		{
			m_ParentItem.SubItems[0].InternalMouseMove(e);
		}
	}

	private ISite GetSite()
	{
		ISite site = null;
		IOwner owner = Owner as IOwner;
		Control val = null;
		if (owner is Control)
		{
			val = (Control)((owner is Control) ? owner : null);
		}
		else if (m_ParentItem != null && m_ParentItem.ContainerControl is Control)
		{
			object containerControl = m_ParentItem.ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
		}
		if (val != null)
		{
			while (site == null && val != null)
			{
				if (((Component)(object)val).Site != null && ((Component)(object)val).Site.DesignMode)
				{
					site = ((Component)(object)val).Site;
				}
				else
				{
					val = val.Parent;
				}
			}
		}
		if (site == null && m_ParentItem != null)
		{
			BaseItem baseItem = m_ParentItem;
			while (site == null && baseItem != null)
			{
				if (baseItem.Site != null && baseItem.Site.DesignMode)
				{
					site = baseItem.Site;
				}
				else
				{
					baseItem = baseItem.Parent;
				}
			}
		}
		return site;
	}

	private void DesignTimeMouseMove(MouseEventArgs e)
	{
		if (m_DragDropInProgress)
		{
			try
			{
				if (m_DesignTimeProvider != null)
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
					m_DesignTimeProvider = null;
				}
				InsertPosition insertPosition = m_DragDropDesignTimeProvider.GetInsertPosition(Control.MousePosition, m_DragItem);
				if (insertPosition != null)
				{
					if (insertPosition.TargetProvider == null)
					{
						Cursor.Current = Cursors.No;
					}
					else
					{
						insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
						m_InsertPosition = insertPosition.Position;
						m_InsertBefore = insertPosition.Before;
						m_DesignTimeProvider = insertPosition.TargetProvider;
						Cursor.Current = Cursors.Hand;
					}
				}
				else
				{
					Cursor.Current = Cursors.No;
				}
				return;
			}
			catch
			{
				m_DesignTimeProvider = null;
				return;
			}
		}
		BaseItem baseItem = m_ParentItem;
		while (baseItem.Parent is IDesignTimeProvider)
		{
			baseItem = baseItem.Parent;
		}
		ISite site = GetSite();
		if (site != null && baseItem.ContainerControl != null && site.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			object containerControl = baseItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is RibbonStrip && ((Component)(object)val).Site == null && val.Parent is RibbonControl)
			{
				val = val.Parent;
			}
			if (designerHost.GetDesigner((IComponent)val) is IBarItemDesigner barItemDesigner)
			{
				barItemDesigner.StartExternalDrag(FocusedItem());
				return;
			}
		}
		m_DragDropDesignTimeProvider = (IDesignTimeProvider)baseItem;
		m_DragItem = FocusedItem();
		m_DragDropInProgress = true;
		((Control)this).Capture = true;
	}

	private void DesignTimeMouseUp(MouseEventArgs e)
	{
		ISite site = GetSite();
		if (site == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			BaseItem parent = m_DragItem.Parent;
			if (parent != null)
			{
				if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0 && parent.SubItems.IndexOf(m_DragItem) < m_InsertPosition)
				{
					m_InsertPosition--;
				}
				componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
				parent.SubItems.Remove(m_DragItem);
				componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
				object containerControl = parent.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is Bar)
				{
					((Bar)(object)val).RecalcLayout();
				}
				else if (val is MenuPanel)
				{
					((MenuPanel)(object)val).RecalcSize();
				}
			}
			m_DesignTimeProvider.InsertItemAt(m_DragItem, m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
		}
		m_DragDropDesignTimeProvider = null;
		m_DragItem = null;
		m_DragDropInProgress = false;
		((Control)this).Capture = false;
	}

	private void DesignTimeMouseDown(MouseEventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		IOwner owner = Owner as IOwner;
		BaseItem baseItem = ItemAtLocation(e.X, e.Y);
		if (baseItem != null && baseItem.Parent is GalleryContainer)
		{
			GalleryContainer galleryContainer = baseItem.Parent as GalleryContainer;
			if (galleryContainer.ScrollHitTest(e.X, e.Y))
			{
				baseItem = galleryContainer;
			}
		}
		if (baseItem != null && baseItem.SystemItem)
		{
			baseItem = null;
		}
		if (baseItem == null)
		{
			if ((int)e.Button != 2097152)
			{
				return;
			}
			ISite site = GetSite();
			if (site != null)
			{
				ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
				if (selectionService.PrimarySelection == m_ParentItem)
				{
					((IMenuCommandService)site.GetService(typeof(IMenuCommandService)))?.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 1280), Control.MousePosition.X, Control.MousePosition.Y);
				}
			}
			return;
		}
		BaseItem baseItem2 = ExpandedItem();
		if (baseItem2 != null && m_HotSubItem != baseItem2)
		{
			baseItem2.Expanded = false;
		}
		if (owner != null)
		{
			owner.SetFocusItem(baseItem);
			ISite site2 = GetSite();
			if (site2 != null)
			{
				ISelectionService selectionService2 = (ISelectionService)site2.GetService(typeof(ISelectionService));
				if (selectionService2 != null)
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(baseItem);
					selectionService2.SetSelectedComponents(arrayList, SelectionTypes.Click);
				}
				if ((int)e.Button == 2097152)
				{
					((IMenuCommandService)site2.GetService(typeof(IMenuCommandService)))?.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 1280), Control.MousePosition.X, Control.MousePosition.Y);
				}
			}
		}
		owner = null;
		baseItem?.InternalMouseDown(e);
	}

	internal void SelectFirstItem()
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		if (m_ParentItem == null)
		{
			return;
		}
		BaseItem baseItem = null;
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			if (subItem.Displayed && subItem.GetEnabled() && (!(subItem is ItemContainer) || ((ItemContainer)subItem).SelectFirstItem()))
			{
				baseItem = subItem;
				break;
			}
		}
		if (baseItem == null || m_HotSubItem == baseItem)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			if (m_Hover)
			{
				ResetHover();
				m_Hover = false;
			}
		}
		if (baseItem != null)
		{
			if (!(baseItem is ItemContainer))
			{
				baseItem.InternalMouseEnter();
				baseItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, baseItem.LeftInternal + 1, baseItem.TopInternal + 1, 0));
			}
			m_HotSubItem = baseItem;
		}
	}

	private void CollapseItemDelayed(BaseItem item)
	{
		ResetHover();
		if (m_DelayedCollapseItem != null && m_DelayedCollapseItem != item)
		{
			m_DelayedCollapseItem.Expanded = false;
		}
		if (!m_LastExpandedOnHover)
		{
			item.Expanded = false;
			m_DelayedCollapseItem = null;
			m_LastExpandedOnHover = true;
		}
		else
		{
			m_DelayedCollapseItem = item;
			m_DelayedCollapseItem.InternalMouseLeave();
		}
	}

	internal void InternalMouseHover()
	{
		if ((m_ParentItem != null && m_ParentItem.DesignMode) || m_ParentItem == null)
		{
			return;
		}
		if (m_DelayedCollapseItem != null)
		{
			m_DelayedCollapseItem.Expanded = false;
			m_DelayedCollapseItem = null;
		}
		if (m_HotSubItem != null)
		{
			bool flag = false;
			flag = m_HotSubItem.Expanded;
			m_HotSubItem.InternalMouseHover();
			m_Hover = true;
			if (!flag && m_HotSubItem != null && m_HotSubItem.Expanded)
			{
				m_LastExpandedOnHover = true;
			}
			else
			{
				m_LastExpandedOnHover = true;
			}
		}
		else if (m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
		{
			if (m_PersonalizedMenus != ePersonalizedMenus.DisplayOnClick)
			{
				m_ExpandButton.PersonalizedAllVisible = true;
				if (m_Owner is IOwnerMenuSupport ownerMenuSupport)
				{
					ownerMenuSupport.PersonalizedAllVisible = true;
				}
				RecalcSize();
				ResetHover();
			}
		}
		else if (m_Scroll && m_ScrollTimer == null)
		{
			CheckScrolling();
			ResetHover();
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		if ((m_ParentItem != null && m_ParentItem.DesignMode) || m_ParentItem == null)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (!m_HotSubItem.DisplayRectangle.Contains(pt) && (!(m_HotSubItem is PopupItem) || !m_HotSubItem.Expanded))
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem = null;
			}
		}
		else if (m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
		{
			m_ExpandButton.MouseOver = false;
			RefreshExpandButton();
		}
		if (m_HotSubItem == null)
		{
			BaseItem baseItem = ExpandedItem();
			if (baseItem != null && baseItem is PopupItem)
			{
				m_HotSubItem = baseItem;
				m_HotSubItem.InternalMouseEnter();
				m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.DisplayRectangle.X + 4, m_HotSubItem.DisplayRectangle.Y + 4, 0));
			}
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		if (m_ParentItem == null)
		{
			return;
		}
		ShowKeyTips = false;
		IOwner owner = Owner as IOwner;
		m_MouseDownPt = new Point(e.X, e.Y);
		if (m_ParentItem.DesignMode)
		{
			DesignTimeMouseDown(e);
		}
		if (!((Component)this).DesignMode && owner != null && owner.GetFocusItem() != null)
		{
			BaseItem baseItem = ItemAtLocation(e.X, e.Y);
			if (baseItem != owner.GetFocusItem())
			{
				owner.GetFocusItem().ReleaseFocus();
			}
		}
		if ((int)e.Button == 2097152 && m_HotSubItem != null && !IsCustomizeMenu && owner is IRibbonCustomize && m_ParentItem != null && m_ParentItem.Name != "syscustomizepopupmenu")
		{
			((IRibbonCustomize)owner).ItemRightClick(m_HotSubItem);
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseDown(e);
		}
		else if (m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
		{
			RefreshExpandButton();
		}
		((Control)this).OnMouseDown(e);
	}

	internal void InternalMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (m_DragDropInProgress)
		{
			DesignTimeMouseUp(e);
		}
		else if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseUp(e);
		}
		else if (m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
		{
			if (m_PersonalizedMenus != ePersonalizedMenus.DisplayOnHover && m_ExpandButton.Rect.Contains(e.X, e.Y))
			{
				ExpandRecentlyUsed();
			}
			else
			{
				RefreshExpandButton();
			}
		}
		((Control)this).OnMouseUp(e);
	}

	internal void ExpandRecentlyUsed()
	{
		if (m_PersonalizedMenus != ePersonalizedMenus.DisplayOnHover)
		{
			m_ExpandButton.PersonalizedAllVisible = true;
			if (Owner is IOwnerMenuSupport ownerMenuSupport)
			{
				ownerMenuSupport.PersonalizedAllVisible = true;
			}
			RecalcSize();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if ((m_ParentItem == null || !m_ParentItem.DesignMode) && m_ParentItem != null)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalClick(Control.MouseButtons, Control.MousePosition);
			}
			else if (m_Scroll)
			{
				CheckScrolling();
			}
			((Control)this).OnClick(e);
		}
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		ISite site = GetSite();
		if (site != null && site.DesignMode)
		{
			ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				((IDesignerHost)site.GetService(typeof(IDesignerHost)))?.GetDesigner(selectionService.PrimarySelection as IComponent)?.DoDefaultAction();
			}
		}
		if ((m_ParentItem == null || !m_ParentItem.DesignMode) && m_ParentItem != null)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
			}
			((Control)this).OnDoubleClick(e);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			((Control)this).OnKeyDown(e);
			return;
		}
		ExKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	private bool CanGetMouseFocus(BaseItem item)
	{
		if (item is LabelItem)
		{
			return false;
		}
		return true;
	}

	internal void ExKeyDown(KeyEventArgs e)
	{
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Invalid comparison between Unknown and I4
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Invalid comparison between Unknown and I4
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Invalid comparison between Unknown and I4
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Invalid comparison between Unknown and I4
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Invalid comparison between Unknown and I4
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Invalid comparison between Unknown and I4
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Expected O, but got Unknown
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Invalid comparison between Unknown and I4
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Invalid comparison between Unknown and I4
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0 || m_ParentItem.DesignMode)
		{
			return;
		}
		if (m_HotSubItem == null || (m_HotSubItem != null && !m_HotSubItem.Expanded))
		{
			if ((int)e.KeyCode == 40)
			{
				if (m_ParentItem is ItemContainer)
				{
					m_ParentItem.InternalKeyDown(e);
				}
				else
				{
					if (m_HotSubItem is ItemContainer)
					{
						m_HotSubItem.InternalKeyDown(e);
					}
					if (!e.Handled)
					{
						int num = 0;
						if (m_HotSubItem != null)
						{
							m_HotSubItem.InternalMouseLeave();
							num = m_ParentItem.SubItems.IndexOf(m_HotSubItem) + 1;
							if (num == m_ParentItem.SubItems.Count && m_ExpandButton.ShowExpandButton)
							{
								m_ExpandButton.MouseOver = true;
								RefreshExpandButton();
							}
							else if (num < 0 || num == m_ParentItem.SubItems.Count)
							{
								num = 0;
							}
							m_HotSubItem = null;
						}
						else if (m_ExpandButton.MouseOver)
						{
							m_ExpandButton.MouseOver = false;
							RefreshExpandButton();
							num = 0;
						}
						for (int i = num; i < m_ParentItem.SubItems.Count; i++)
						{
							BaseItem baseItem = m_ParentItem.SubItems[i];
							if (baseItem.Displayed && baseItem.Visible && CanGetMouseFocus(baseItem))
							{
								m_HotSubItem = baseItem;
								m_HotSubItem.InternalMouseEnter();
								m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
								if (m_HotSubItem.IsContainer)
								{
									m_HotSubItem.InternalKeyDown(e);
								}
								break;
							}
						}
						if (m_HotSubItem == null && m_ExpandButton.ShowExpandButton && !m_ExpandButton.MouseOver)
						{
							m_ExpandButton.MouseOver = true;
							RefreshExpandButton();
						}
					}
				}
				e.Handled = true;
				m_IgnoreDuplicateMouseMove = true;
			}
			else if ((int)e.KeyCode == 38)
			{
				if (m_ParentItem is ItemContainer)
				{
					m_ParentItem.InternalKeyDown(e);
				}
				else
				{
					if (m_HotSubItem is ItemContainer)
					{
						m_HotSubItem.InternalKeyDown(e);
					}
					if (!e.Handled)
					{
						int num2 = 0;
						if (m_HotSubItem != null)
						{
							m_HotSubItem.InternalMouseLeave();
							num2 = m_ParentItem.SubItems.IndexOf(m_HotSubItem) - 1;
							if (num2 < 0 && m_ExpandButton.ShowExpandButton)
							{
								m_ExpandButton.MouseOver = true;
								RefreshExpandButton();
							}
							else if (num2 < 0)
							{
								num2 = m_ParentItem.SubItems.Count - 1;
							}
							m_HotSubItem = null;
						}
						else if (m_ExpandButton.MouseOver)
						{
							m_ExpandButton.MouseOver = false;
							RefreshExpandButton();
							num2 = m_ParentItem.SubItems.Count - 1;
						}
						else if (m_ExpandButton.ShowExpandButton)
						{
							m_ExpandButton.MouseOver = true;
							RefreshExpandButton();
						}
						else
						{
							num2 = m_ParentItem.SubItems.Count - 1;
						}
						for (int num3 = num2; num3 >= 0; num3--)
						{
							BaseItem baseItem2 = m_ParentItem.SubItems[num3];
							if (baseItem2.Displayed && baseItem2.Visible && CanGetMouseFocus(baseItem2))
							{
								m_HotSubItem = baseItem2;
								m_HotSubItem.InternalMouseEnter();
								m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
								if (m_HotSubItem.IsContainer)
								{
									m_HotSubItem.InternalKeyDown(e);
								}
								break;
							}
						}
						if (m_HotSubItem == null && m_ExpandButton.ShowExpandButton && !m_ExpandButton.MouseOver)
						{
							m_ExpandButton.MouseOver = true;
							RefreshExpandButton();
						}
					}
				}
				e.Handled = true;
				m_IgnoreDuplicateMouseMove = true;
			}
			else if ((int)e.KeyCode == 39)
			{
				m_IgnoreDuplicateMouseMove = true;
				if (m_HotSubItem != null && m_HotSubItem.GetEnabled() && m_HotSubItem is ButtonItem buttonItem && buttonItem.SubItems.Count > 0 && buttonItem.ShowSubItems && !buttonItem.Expanded)
				{
					buttonItem.Expanded = true;
					if (buttonItem.PopupControl is MenuPanel)
					{
						((MenuPanel)(object)buttonItem.PopupControl).SelectFirstItem();
					}
					e.Handled = true;
				}
				if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
				{
					m_HotSubItem.InternalKeyDown(e);
				}
				if (!e.Handled && m_ParentItem != null)
				{
					foreach (BaseItem subItem in m_ParentItem.SubItems)
					{
						if (subItem is ItemContainer)
						{
							subItem.InternalKeyDown(e);
							if (e.Handled)
							{
								break;
							}
						}
					}
				}
			}
			else if ((int)e.KeyCode == 37)
			{
				if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
				{
					m_HotSubItem.InternalKeyDown(e);
				}
				if (!e.Handled)
				{
					m_IgnoreDuplicateMouseMove = true;
					if (BaseItem.IsOnPopup(m_ParentItem))
					{
						m_ParentItem.Expanded = false;
						e.Handled = true;
					}
				}
			}
			else if ((int)e.KeyCode == 27)
			{
				BaseItem parentItem = m_ParentItem;
				parentItem.Expanded = false;
				if (parentItem.Parent != null && parentItem.Parent is GenericItemContainer && parentItem.Parent.AutoExpand)
				{
					parentItem.Parent.AutoExpand = false;
				}
				e.Handled = true;
			}
			else if ((int)e.KeyCode == 13 && m_HotSubItem != null && m_HotSubItem.IsContainer)
			{
				m_HotSubItem.InternalKeyDown(e);
			}
			else if ((int)e.KeyCode == 13 && m_HotSubItem != null && m_HotSubItem.SubItems.Count > 0 && m_HotSubItem.ShowSubItems && !m_HotSubItem.Expanded && m_HotSubItem.GetEnabled())
			{
				m_HotSubItem.Expanded = true;
				if (m_HotSubItem is PopupItem && ((PopupItem)m_HotSubItem).PopupControl is MenuPanel)
				{
					((MenuPanel)(object)((PopupItem)m_HotSubItem).PopupControl).SelectFirstItem();
				}
				e.Handled = true;
			}
			else if (((int)e.KeyCode == 13 || (int)e.KeyCode == 32) && m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
			{
				ExpandRecentlyUsed();
			}
			else
			{
				int num4 = 0;
				if (e.Shift)
				{
					try
					{
						byte[] array = new byte[256];
						if (NativeFunctions.GetKeyboardState(array))
						{
							byte[] array2 = new byte[2];
							if (NativeFunctions.ToAscii((uint)e.KeyValue, 0u, array, array2, 0u) != 0)
							{
								num4 = array2[0];
							}
						}
					}
					catch (Exception)
					{
						num4 = 0;
					}
				}
				if (num4 == 0 && !BarFunctions.IsSystemKey(e.KeyCode))
				{
					num4 = (int)NativeFunctions.MapVirtualKey((uint)e.KeyValue, 2u);
					if (num4 == 0)
					{
						num4 = e.KeyValue;
					}
				}
				if (e is KeyEventArgsEx && ((KeyEventArgsEx)(object)e).KeyChar != 0 && ProcessContainerAccessKey(m_ParentItem, ((KeyEventArgsEx)(object)e).KeyChar))
				{
					e.Handled = true;
				}
				if (!e.Handled && num4 > 0)
				{
					char[] array3 = new char[1];
					byte[] bytes = new byte[1] { Convert.ToByte(num4) };
					Encoding.Default.GetDecoder().GetChars(bytes, 0, 1, array3, 0);
					string text = array3[0].ToString();
					array3[0] = text.ToLower()[0];
					if (ProcessContainerAccessKey(m_ParentItem, array3[0]))
					{
						e.Handled = true;
					}
				}
			}
		}
		if (e.Handled || m_HotSubItem == null)
		{
			return;
		}
		bool flag = true;
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val.Focused)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			m_HotSubItem.InternalKeyDown(e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27 || (int)keyData == 13)
		{
			KeyEventArgs val = new KeyEventArgs(keyData);
			ExKeyDown(val);
			if (val.Handled)
			{
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private bool ProcessContainerAccessKey(BaseItem container, char key)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		List<BaseItem> itemsForMnemonic = GetItemsForMnemonic(container, key);
		if (itemsForMnemonic.Count > 1)
		{
			BaseItem baseItem = null;
			if (m_HotSubItem == null)
			{
				baseItem = itemsForMnemonic[0];
			}
			else
			{
				int num = itemsForMnemonic.IndexOf(m_HotSubItem);
				if (num == -1)
				{
					baseItem = itemsForMnemonic[0];
				}
				else
				{
					num = ((num != itemsForMnemonic.Count - 1) ? (num + 1) : 0);
					baseItem = itemsForMnemonic[num];
				}
			}
			if (baseItem != null)
			{
				if (m_HotSubItem != null)
				{
					m_HotSubItem.InternalMouseLeave();
				}
				m_HotSubItem = baseItem;
				m_HotSubItem.InternalMouseEnter();
				m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
			}
			return true;
		}
		BaseItem itemForMnemonic = GetItemForMnemonic(container, key, deepScan: true);
		if (itemForMnemonic != null && itemForMnemonic.GetEnabled() && itemForMnemonic.Visible)
		{
			if (itemForMnemonic is ItemContainer)
			{
				ShowKeyTips = false;
				HotSubItem = itemForMnemonic;
			}
			else if (itemForMnemonic.SubItems.Count > 0 && itemForMnemonic.ShowSubItems && !itemForMnemonic.Expanded)
			{
				if (m_HotSubItem != itemForMnemonic)
				{
					if (m_HotSubItem != null)
					{
						m_HotSubItem.InternalMouseLeave();
					}
					m_HotSubItem = itemForMnemonic;
					m_HotSubItem.InternalMouseEnter();
					m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
				}
				itemForMnemonic.Expanded = true;
				if (itemForMnemonic is PopupItem && ((PopupItem)itemForMnemonic).PopupControl is MenuPanel)
				{
					((MenuPanel)(object)((PopupItem)itemForMnemonic).PopupControl).SelectFirstItem();
				}
				m_IgnoreDuplicateMouseMove = true;
			}
			else
			{
				ShowKeyTips = false;
				if (itemForMnemonic is ComboBoxItem)
				{
					((Control)((ComboBoxItem)itemForMnemonic).ComboBoxEx).Focus();
					((ComboBox)((ComboBoxItem)itemForMnemonic).ComboBoxEx).DroppedDown = true;
				}
				else if (itemForMnemonic is TextBoxItem)
				{
					((Control)((TextBoxItem)itemForMnemonic).TextBox).Focus();
				}
				else if (itemForMnemonic is ControlContainerItem && ((ControlContainerItem)itemForMnemonic).Control != null)
				{
					((ControlContainerItem)itemForMnemonic).Control.Focus();
				}
				else
				{
					itemForMnemonic.RaiseClick();
				}
			}
			return true;
		}
		return false;
	}

	private List<BaseItem> GetItemsForMnemonic(BaseItem container, char charCode)
	{
		List<BaseItem> list = new List<BaseItem>();
		foreach (BaseItem subItem in container.SubItems)
		{
			if (Control.IsMnemonic(charCode, subItem.Text) && subItem.Visible && subItem.GetEnabled())
			{
				list.Add(subItem);
			}
		}
		return list;
	}

	protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan)
	{
		string text = m_KeyTipsKeysStack + charCode;
		text = text.ToUpper();
		bool flag = false;
		foreach (BaseItem subItem in container.SubItems)
		{
			if (subItem.KeyTips != "" || m_KeyTipsKeysStack != "")
			{
				if (subItem.KeyTips != "")
				{
					if (subItem.KeyTips == text)
					{
						if (subItem.Visible && subItem.GetEnabled())
						{
							return subItem;
						}
					}
					else if (subItem.KeyTips.StartsWith(text))
					{
						flag = true;
					}
				}
			}
			else if (Control.IsMnemonic(charCode, subItem.Text) && subItem.Visible && subItem.GetEnabled())
			{
				return subItem;
			}
			if (deepScan && subItem.IsContainer)
			{
				BaseItem itemForMnemonic = GetItemForMnemonic(subItem, charCode, deepScan);
				if (itemForMnemonic != null)
				{
					return itemForMnemonic;
				}
			}
			else if (deepScan && subItem is ControlContainerItem && ((ControlContainerItem)subItem).Control is RibbonBar)
			{
				RibbonBar ribbonBar = ((ControlContainerItem)subItem).Control as RibbonBar;
				BaseItem itemForMnemonic2 = GetItemForMnemonic(ribbonBar.GetBaseItemContainer(), charCode, deepScan);
				if (itemForMnemonic2 != null)
				{
					return itemForMnemonic2;
				}
			}
		}
		if (flag)
		{
			m_KeyTipsKeysStack += charCode.ToString().ToUpper();
		}
		return null;
	}

	protected BaseItem ItemAtLocation(int x, int y)
	{
		if (m_ParentItem != null && m_ParentItem.SubItems.Count != 0)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if ((!subItem.Visible && !m_IsCustomizeMenu) || !subItem.Displayed || x < subItem.LeftInternal || x > subItem.LeftInternal + subItem.WidthInternal || y < subItem.TopInternal || y > subItem.TopInternal + subItem.HeightInternal)
				{
					continue;
				}
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ItemAtLocation(x, y);
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				return subItem;
			}
		}
		return null;
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		ClearHotSubItem();
		((Control)this).OnHandleDestroyed(e);
	}

	internal void ClearHotSubItem()
	{
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			m_HotSubItem = null;
		}
	}

	private void ResetHover()
	{
		NativeFunctions.TRACKMOUSEEVENT tme = default(NativeFunctions.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf(tme);
		NativeFunctions.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		NativeFunctions.TrackMouseEvent(ref tme);
	}

	private void SetupScrollTimer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		m_ScrollTimer = new Timer();
		m_ScrollTimer.Interval = 100;
		m_ScrollTimer.Tick += ScrollTimerTick;
		m_ScrollTimer.Start();
	}

	private void ScrollTimerTick(object sender, EventArgs e)
	{
		CheckScrolling();
	}

	private bool CheckScrolling()
	{
		if (!m_Scroll)
		{
			return false;
		}
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (m_TopScroll && point.Y <= m_TopScrollHeight + m_ClientRect.Top)
		{
			if (m_ScrollTopPosition > 0)
			{
				m_ScrollTopPosition--;
			}
			RepositionItems();
			((Control)this).Refresh();
			if (m_ScrollTimer == null)
			{
				SetupScrollTimer();
			}
			return true;
		}
		if (m_BottomScroll && point.Y >= ((Control)this).Height - m_BottomScrollHeight)
		{
			m_ScrollTopPosition++;
			RepositionItems();
			((Control)this).Refresh();
			if (m_ScrollTimer == null)
			{
				SetupScrollTimer();
			}
			return true;
		}
		if (m_ScrollTimer != null)
		{
			m_ScrollTimer.Stop();
			((Component)(object)m_ScrollTimer).Dispose();
			m_ScrollTimer = null;
		}
		return false;
	}

	private void RepositionItems()
	{
		m_TopScroll = false;
		m_BottomScroll = false;
		if (!m_Scroll || m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			return;
		}
		int num = m_ClientRect.Top;
		int num2 = 0;
		if (m_ScrollTopPosition > 0)
		{
			m_TopScrollHeight = 16;
			num += m_TopScrollHeight;
			m_TopScroll = true;
		}
		bool flag = false;
		for (int i = 0; i < m_ParentItem.SubItems.Count; i++)
		{
			BaseItem baseItem = m_ParentItem.SubItems[i];
			if (flag || i < m_ScrollTopPosition || (!baseItem.Visible && !m_IsCustomizeMenu))
			{
				baseItem.Displayed = false;
				continue;
			}
			num2 = baseItem.HeightInternal;
			if (baseItem.BeginGroup)
			{
				num2 = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num2 + 3) : (num2 + 9));
			}
			if (num2 + num > m_ClientRect.Bottom || (m_ClientRect.Bottom - (num2 + num) < 15 && i + 1 < m_ParentItem.SubItems.Count))
			{
				baseItem.Displayed = false;
				flag = true;
				m_BottomScroll = true;
				m_BottomScrollHeight = m_ClientRect.Bottom - num;
				continue;
			}
			if (i == m_ParentItem.SubItems.Count - 1)
			{
				baseItem.Displayed = true;
				num = m_ClientRect.Bottom;
				for (int num3 = i; num3 >= m_ScrollTopPosition; num3--)
				{
					baseItem = m_ParentItem.SubItems[num3];
					if (baseItem.Visible || m_IsCustomizeMenu)
					{
						num = (baseItem.TopInternal = num - baseItem.HeightInternal);
						if (baseItem.BeginGroup)
						{
							num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num - 3) : (num - 9));
						}
					}
				}
				if (m_ScrollTopPosition > 0)
				{
					baseItem = m_ParentItem.SubItems[m_ScrollTopPosition - 1];
					if (num - baseItem.HeightInternal - 15 > m_ClientRect.Top)
					{
						for (int num5 = m_ScrollTopPosition - 1; num5 >= 0; num5--)
						{
							baseItem = m_ParentItem.SubItems[num5];
							if (baseItem.Visible || m_IsCustomizeMenu)
							{
								if (num - baseItem.HeightInternal - 15 <= m_ClientRect.Top)
								{
									break;
								}
								num = (baseItem.TopInternal = num - baseItem.HeightInternal);
								baseItem.Displayed = true;
								if (baseItem.BeginGroup)
								{
									num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num - 3) : (num - 9));
								}
								m_ScrollTopPosition = num5;
							}
						}
					}
				}
				m_TopScrollHeight = num - m_ClientRect.Top;
				m_BottomScroll = false;
				break;
			}
			if (baseItem.BeginGroup)
			{
				num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num + 3) : (num + 9));
			}
			baseItem.TopInternal = num;
			baseItem.Displayed = true;
			num += baseItem.HeightInternal;
		}
	}

	private void SetRoundRegion(Control c)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		Rectangle clientRectangle = c.ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(clientRectangle, m_CornerSize);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(roundedRectanglePath);
		roundedRectanglePath.Widen(SystemPens.ControlText);
		val.Union(roundedRectanglePath);
		c.Region = val;
	}

	public void Hide()
	{
		((Control)this).Hide();
		if (m_DesignerParent && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			m_DesignerParent = false;
		}
	}

	private bool AddtoDesignTimeContainer()
	{
		ISite site = GetSite();
		if (site == null)
		{
			return false;
		}
		if (!(site.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			return false;
		}
		IComponent rootComponent = designerHost.RootComponent;
		Control val = (Control)((rootComponent is Control) ? rootComponent : null);
		while (val != null)
		{
			val = val.Parent;
			if (val != null && ((object)val).GetType().Name.IndexOf("DesignerFrame") >= 0)
			{
				break;
			}
		}
		if (val == null || val.Parent == null)
		{
			return false;
		}
		Point location = val.PointToClient(((Control)this).Location);
		val.Controls.Add((Control)(object)this);
		((Control)this).Location = location;
		((Control)this).Visible = true;
		((Control)this).Update();
		((Control)this).BringToFront();
		m_DesignerParent = true;
		return true;
	}

	public void Show()
	{
		if (!m_PopupMenu)
		{
			((Control)this).Visible = true;
			((Control)this).Update();
		}
		else
		{
			if (m_ParentItem != null && ((m_ParentItem.Site != null && m_ParentItem.Site.DesignMode) || m_ParentItem.DesignMode || (m_ParentItem.Parent != null && m_ParentItem.Parent.Site != null && m_ParentItem.Parent.Site.DesignMode)) && AddtoDesignTimeContainer())
			{
				return;
			}
			PopupContainer popupContainer = new PopupContainer();
			popupContainer.ShowDropShadow = DisplayShadow && AlphaShadow;
			ScreenInformation screenInformation = BarFunctions.ScreenFromControl((Control)(object)this);
			if (screenInformation != null)
			{
				Rectangle rectangle = screenInformation.WorkingArea;
				if (IsContextMenu || UseWholeScreenForSizeChecking)
				{
					rectangle = screenInformation.Bounds;
				}
				if (((Control)this).Location.Y + ((Control)this).Size.Height > rectangle.Bottom)
				{
					m_ClientRect.Height -= ((Control)this).Location.Y + ((Control)this).Size.Height - rectangle.Bottom;
					((Control)this).Height = ((Control)this).Height - (((Control)this).Location.Y + ((Control)this).Size.Height - rectangle.Bottom);
					m_Scroll = true;
					m_ScrollTopPosition = 0;
					RepositionItems();
				}
			}
			((Control)popupContainer).Location = ((Control)this).Location;
			((Control)popupContainer).Controls.Add((Control)(object)this);
			((Control)popupContainer).Size = ((Control)this).Size;
			((Control)this).Location = new Point(0, 0);
			((Control)popupContainer).CreateControl();
			if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && IsRoundRegion)
			{
				SetRoundRegion((Control)(object)this);
				SetRoundRegion((Control)(object)popupContainer);
			}
			ePopupAnimation ePopupAnimation2 = m_PopupAnimation;
			if (!BarFunctions.SupportsAnimation)
			{
				ePopupAnimation2 = ePopupAnimation.None;
			}
			else
			{
				if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
				{
					if (m_Owner is IOwnerMenuSupport ownerMenuSupport)
					{
						ePopupAnimation2 = ownerMenuSupport.PopupAnimation;
					}
					if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
					{
						ePopupAnimation2 = ePopupAnimation.SystemDefault;
					}
				}
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.SystemDefault:
					ePopupAnimation2 = NativeFunctions.SystemMenuAnimation;
					break;
				case ePopupAnimation.Random:
				{
					Random random = new Random();
					int num = random.Next(2);
					ePopupAnimation2 = ePopupAnimation.Fade;
					switch (num)
					{
					case 1:
						ePopupAnimation2 = ePopupAnimation.Slide;
						break;
					case 2:
						ePopupAnimation2 = ePopupAnimation.Unfold;
						break;
					}
					break;
				}
				}
			}
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				ePopupAnimation2 = ePopupAnimation.None;
			}
			if (ePopupAnimation2 == ePopupAnimation.Fade && Environment.OSVersion.Version.Major >= 5)
			{
				NativeFunctions.AnimateWindow(((Control)popupContainer).Handle, 100, 524288);
				((Control)popupContainer).Visible = true;
			}
			else
			{
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.Slide:
					NativeFunctions.AnimateWindow(((Control)popupContainer).Handle, 100, 262149);
					((Control)popupContainer).Visible = true;
					break;
				case ePopupAnimation.Unfold:
					NativeFunctions.AnimateWindow(((Control)popupContainer).Handle, 100, 5);
					((Control)popupContainer).Visible = true;
					break;
				default:
					((Control)this).Visible = true;
					((Control)popupContainer).Visible = true;
					break;
				}
			}
			popupContainer.ShowShadow();
			if (ePopupAnimation2 != 0 && ((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				((Control)this).Refresh();
			}
			if (new Rectangle(((Control)popupContainer).Location, ((Control)popupContainer).Size).Contains(Control.MousePosition))
			{
				m_IgnoreDuplicateMouseMove = true;
			}
			((Control)this).Update();
			if (((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
			{
				if (m_ParentItem != null && m_ParentItem.IsOnMenuBar)
				{
					popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)4);
				}
				popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)6);
			}
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (!((Control)this).Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		if (!((Control)this).Visible && ((Control)this).Parent != null && ((Control)this).Parent is PopupContainer)
		{
			((Control)this).Parent.Visible = false;
		}
		if (!((Control)this).Visible && m_ScrollTimer != null)
		{
			m_ScrollTimer.Stop();
			((Component)(object)m_ScrollTimer).Dispose();
			m_ScrollTimer = null;
		}
		if (!m_AccessibleObjectCreated || !(((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject))
		{
			return;
		}
		if (m_ParentItem != null)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				popupMenuAccessibleObject.GenerateEvent(subItem, (AccessibleEvents)32769);
			}
		}
		if (!((Control)this).Visible)
		{
			popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)7);
			popupMenuAccessibleObject.GenerateEvent((AccessibleEvents)32778);
		}
	}

	private bool IsChildItem(BaseItem item)
	{
		if (item == null)
		{
			return false;
		}
		while (item != null)
		{
			if (ParentItem == item)
			{
				return true;
			}
			item = item.Parent;
		}
		return false;
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		if (m_ParentItem == null)
		{
			return null;
		}
		return DesignTimeProviderContainer.GetInsertPosition(m_ParentItem, pScreen, DragItem);
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		if (iPos >= 0)
		{
			BaseItem baseItem = m_ParentItem.SubItems[iPos];
			if (baseItem.DesignInsertMarker != 0)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.None;
			}
			else if (Before)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.Before;
			}
			else
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.After;
			}
		}
		else
		{
			Rectangle rectangle = new Rectangle(((Control)this).ClientRectangle.Left + 2, ((Control)this).ClientRectangle.Top + 2, ((Control)this).ClientRectangle.Width - 4, 1);
			Rectangle rectangle2 = new Rectangle(((Control)this).ClientRectangle.Left + 1, ((Control)this).ClientRectangle.Top, 1, 5);
			Rectangle rectangle3 = new Rectangle(((Control)this).ClientRectangle.Right - 2, ((Control)this).ClientRectangle.Top, 1, 5);
		}
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		if (!Before)
		{
			if (iPos + 1 >= m_ParentItem.SubItems.Count)
			{
				m_ParentItem.SubItems.Add(objItem);
			}
			else
			{
				m_ParentItem.SubItems.Add(objItem, iPos + 1);
			}
		}
		else if (iPos >= m_ParentItem.SubItems.Count)
		{
			m_ParentItem.SubItems.Add(objItem);
		}
		else
		{
			m_ParentItem.SubItems.Add(objItem, iPos);
		}
		objItem.ContainerControl = this;
		ClearHotSubItem();
		RecalcSize();
		ISite site = GetSite();
		if (site != null && m_ParentItem != null && ((Component)(object)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanged(m_ParentItem, TypeDescriptor.GetProperties(m_ParentItem)["SubItems"], null, null);
		}
	}
}
