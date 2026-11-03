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
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MenuPanel : Control, IDesignTimeProvider, IAccessibilitySupport, IKeyTipsControl, _103D
{
	private struct _၅
	{
		public bool ਸ;

		public bool _၆;

		public Rectangle _၇;

		public bool ज़;

		public _၅(bool personalizedallvisible, bool showexpandbutton, Rectangle rExpandButton, bool mouseover)
		{
			ਸ = personalizedallvisible;
			_၆ = showexpandbutton;
			_၇ = rExpandButton;
			ज़ = mouseover;
		}
	}

	public class PopupMenuAccessibleObject : ControlAccessibleObject
	{
		private MenuPanel _0AC3;

		public override AccessibleRole Role
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null)
				{
					return (AccessibleRole)0;
				}
				return ((Control)_0AC3).AccessibleRole;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (_0AC3 == null || _0AC3.ParentItem == null)
				{
					return null;
				}
				return _0AC3.ParentItem.AccessibleObject;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (_0AC3 == null)
				{
					return Rectangle.Empty;
				}
				return ((Control)_0AC3).DisplayRectangle;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				if (_0AC3 != null && !((Control)_0AC3).IsDisposed)
				{
					return (AccessibleStates)4096;
				}
				return (AccessibleStates)1;
			}
		}

		public PopupMenuAccessibleObject(MenuPanel owner)
			: base((Control)(object)owner)
		{
			_0AC3 = owner;
		}

		internal void _၈(BaseItem _٠, AccessibleEvents ؿ)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 != null && _0AC3 != null && _0AC3.m_ParentItem != null)
			{
				int num = _0AC3.m_ParentItem.SubItems.IndexOf(_٠);
				if (num >= 0)
				{
					((Control)_0AC3).AccessibilityNotifyClients(ؿ, num);
				}
			}
		}

		internal void _၈(AccessibleEvents ؿ)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 != null)
			{
				((Control)_0AC3).AccessibilityNotifyClients(ؿ, -1);
			}
		}

		public override int GetChildCount()
		{
			if (_0AC3 != null && _0AC3.m_ParentItem != null)
			{
				return _0AC3.m_ParentItem.SubItems.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (_0AC3 != null && _0AC3.m_ParentItem != null)
			{
				return _0AC3.m_ParentItem.SubItems[iIndex].AccessibleObject;
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

	private class _2919
	{
		public LinearGradientColorTable ཥ = new LinearGradientColorTable();

		public LinearGradientColorTable _291A = new LinearGradientColorTable();

		public LinearGradientColorTable _291B = new LinearGradientColorTable();

		public LinearGradientColorTable ᙴ = new LinearGradientColorTable();

		public LinearGradientColorTable _291C = new LinearGradientColorTable();

		public LinearGradientColorTable _291D = new LinearGradientColorTable();
	}

	private const int ग = 33;

	private const int घ = 3;

	private const int ङ = 4;

	private const int ဓ = 16;

	private const int န = 3;

	private const int ပ = 9;

	private const int ဖ = 14;

	private const int ဗ = 18;

	protected BaseItem m_ParentItem;

	protected BaseItem m_HotSubItem;

	protected Point m_ParentItemScreenPos;

	private bool ဘ;

	private bool _0DC9;

	private bool _0DCB;

	private bool _0DCC;

	private int _0DCA;

	private int မ;

	private int ယ;

	private Rectangle _0B59;

	private SideBarImage _0B5A;

	private Rectangle ရ;

	private object _0AC3;

	private bool လ;

	private Point _0AC2;

	private ePersonalizedMenus ಛ;

	private int ဝ = 14;

	private _၅ သ = new _၅(personalizedallvisible: false, showexpandbutton: false, Rectangle.Empty, mouseover: false);

	private Timer ဟ;

	private ePopupAnimation ஃ = ePopupAnimation.ManagerControlled;

	private ഓ _0B84;

	private ColorScheme լ;

	private bool _0DCF;

	private MouseEventArgs _0DD0;

	internal bool _0B98;

	private bool ಔ = true;

	private bool _০ = true;

	private BaseItem ဠ;

	private bool အ = true;

	private bool ե;

	private int ன = 3;

	private BaseItem ਈ;

	internal bool ဢ;

	private bool _09FE;

	private string _0A00 = "";

	private Font _09FF;

	private _103E _0A01;

	private bool _0BCB;

	private int _৬ = -1;

	private bool _৭;

	private IDesignTimeProvider _৫;

	private BaseItem ঢ়;

	private IDesignTimeProvider _0BCC;

	private bool _0BBA;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool ShowKeyTips
	{
		get
		{
			return _09FE;
		}
		set
		{
			if (_09FE != value)
			{
				_09FE = value;
				OnShowKeyTipsChanged();
			}
		}
	}

	string IKeyTipsControl._09B5
	{
		get
		{
			return _0A00;
		}
		set
		{
			_0A00 = value;
			if (_0A01 != null)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	[DefaultValue(null)]
	public virtual Font KeyTipsFont
	{
		get
		{
			return _09FF;
		}
		set
		{
			_09FF = value;
		}
	}

	BaseItem IAccessibilitySupport._09B3
	{
		get
		{
			return ਈ;
		}
		set
		{
			ਈ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool PopupMenu
	{
		get
		{
			return ಔ;
		}
		set
		{
			ಔ = value;
		}
	}

	private bool ဏ
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

	private bool _0AFB
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
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			_௯();
			m_ParentItem = value;
			if (m_ParentItem != null && m_ParentItem.SubItems.Count != 0)
			{
				if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					ဝ = 18;
				}
				else
				{
					ဝ = 14;
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
				if (val != null)
				{
					val.MouseWheel += new MouseEventHandler(_102C);
				}
				if (m_ParentItem.Displayed && BaseItem.IsHandleValid(val))
				{
					m_ParentItemScreenPos = val.PointToScreen(new Point(m_ParentItem.LeftInternal, m_ParentItem.TopInternal));
					val = null;
				}
				if (val is ItemControl)
				{
					ե = ((ItemControl)(object)val).AntiAlias;
				}
			}
		}
	}

	private bool တ
	{
		get
		{
			if (m_ParentItem != null)
			{
				if ((m_ParentItem is ApplicationButton && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Windows7) || StyleManager.IsMetro(m_ParentItem.EffectiveStyle))
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

	private bool ထ
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

	private int ଈ
	{
		get
		{
			int num = 0;
			if (m_ParentItem != null && _0AFB)
			{
				num = 1;
				if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					num++;
				}
				if (ဏ)
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

	private int ଉ
	{
		get
		{
			int num = 0;
			num = ((m_ParentItem == null || !_0AFB) ? 3 : 2);
			if (ဏ)
			{
				num--;
			}
			return num;
		}
	}

	private int ଊ
	{
		get
		{
			bool flag = true;
			int num = 0;
			if (_0AC3 is IOwnerMenuSupport { ShowPopupShadow: false })
			{
				flag = false;
			}
			if (m_ParentItem != null && _0AFB)
			{
				num = ((_0B0D || !flag) ? 1 : 3);
				if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					num++;
				}
				if (ဏ)
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

	private int ଋ
	{
		get
		{
			IOwnerMenuSupport ownerMenuSupport = _0AC3 as IOwnerMenuSupport;
			bool flag = true;
			int num = 0;
			if (ownerMenuSupport != null && !ownerMenuSupport.ShowPopupShadow)
			{
				flag = false;
			}
			if (m_ParentItem != null && _0AFB)
			{
				num = ((!_0B0D && flag) ? 4 : 2);
				if (ဏ)
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

	internal bool ଌ
	{
		get
		{
			if (!ಔ || (m_ParentItem != null && m_ParentItem.Site != null && m_ParentItem.Site.DesignMode))
			{
				return false;
			}
			if (_0AC3 is IOwnerMenuSupport ownerMenuSupport)
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
			return ե;
		}
		set
		{
			ե = value;
		}
	}

	internal bool _0B0D
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				return false;
			}
			if (_0AC3 is IOwnerMenuSupport { AlphaBlendShadow: false })
			{
				return false;
			}
			return true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether Tooltips are shown on Bar and it's sub-items.")]
	[DefaultValue(true)]
	public bool ShowToolTips
	{
		get
		{
			return _০;
		}
		set
		{
			_০ = value;
		}
	}

	public SideBarImage SideBar
	{
		get
		{
			return _0B5A;
		}
		set
		{
			_0B5A = value;
		}
	}

	public object Owner
	{
		get
		{
			return _0AC3;
		}
		set
		{
			_0AC3 = value;
		}
	}

	internal bool ဒ
	{
		get
		{
			return လ;
		}
		set
		{
			လ = value;
			if (m_ParentItem == null)
			{
				return;
			}
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				subItem.SetIsOnCustomizeMenu(လ);
			}
		}
	}

	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[Browsable(true)]
	[Category("Appearance")]
	public ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return ಛ;
		}
		set
		{
			ಛ = value;
		}
	}

	public bool PersonalizedAllVisible
	{
		get
		{
			return သ.ਸ;
		}
		set
		{
			သ.ਸ = value;
		}
	}

	public ePopupAnimation PopupAnimation
	{
		get
		{
			return ஃ;
		}
		set
		{
			ஃ = value;
		}
	}

	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			լ = value;
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
			if (ParentItem.SubItems.Contains(value) || _103C(value))
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
		ဘ = false;
		_0DC9 = false;
		_0DCB = false;
		_0DCC = false;
		_0DCA = 0;
		မ = 0;
		ယ = 0;
		_0B59 = default(Rectangle);
		ရ = default(Rectangle);
		_0B5A = default(SideBarImage);
		လ = false;
		_0AC3 = null;
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
		if (_0B98 && ((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			popupMenuAccessibleObject._၈((AccessibleEvents)32769);
		}
		if (_0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		((Control)this).Font = null;
		_௯();
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
		if (((Message)(ref m)).Msg == 1131 && ਈ != null)
		{
			ਈ.DoAccesibleDefaultAction();
			ਈ = null;
		}
		((Control)this).WndProc(ref m);
	}

	private BaseRenderer ဣ()
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
		_0A00 = "";
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
		if (_0A01 != null)
		{
			((Control)_0A01).BringToFront();
			return;
		}
		_0A01 = new _103E(this);
		((Control)_0A01).Bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		((Control)_0A01).Visible = true;
		((Control)this).Controls.Add((Control)(object)_0A01);
		((Control)_0A01).BringToFront();
	}

	protected virtual void DestroyKeyTipCanvas()
	{
		if (_0A01 != null)
		{
			((Control)_0A01).Visible = false;
			((Control)this).Controls.Remove((Control)(object)_0A01);
			((Component)(object)_0A01).Dispose();
			_0A01 = null;
		}
	}

	private void ઠ(Graphics _٥)
	{
		છ(_٥);
	}

	void _103D.છ(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ઠ
		this.ઠ(_٥);
	}

	protected virtual void છ(Graphics _٥)
	{
		if (_09FE && m_ParentItem != null)
		{
			KeyTipsRendererEventArgs ؿ = new KeyTipsRendererEventArgs(_٥, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer ઝ = ဣ();
			જ(m_ParentItem, ઝ, ؿ);
		}
	}

	protected virtual Font GetKeyTipFont()
	{
		Font result = ((Control)this).Font;
		if (_09FF != null)
		{
			result = _09FF;
		}
		return result;
	}

	internal virtual void જ(BaseItem ઇ, BaseRenderer ઝ, KeyTipsRendererEventArgs ؿ)
	{
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem.IsContainer)
			{
				જ(subItem, ઝ, ؿ);
			}
			if ((subItem.AccessKey != 0 || !(subItem.KeyTips == "")) && (!(_0A00 != "") || subItem.KeyTips.StartsWith(_0A00)) && (!(subItem.KeyTips == "") || !(_0A00 != "")))
			{
				if (subItem.KeyTips != "")
				{
					ؿ.KeyTip = subItem.KeyTips;
				}
				else
				{
					ؿ.KeyTip = subItem.AccessKey.ToString().ToUpper();
				}
				ؿ.Bounds = GetKeyTipRectangle(ؿ.Graphics, subItem, ؿ.Font, ؿ.KeyTip);
				ؿ.ReferenceObject = subItem;
				ઝ.DrawKeyTips(ؿ);
			}
		}
	}

	protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size _၂ = _၁._၂;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += _၂.Width;
		size.Height += _၂.Height;
		Rectangle displayRectangle = item.DisplayRectangle;
		return new Rectangle(displayRectangle.X + 16, displayRectangle.Bottom - size.Height, size.Width, size.Height);
	}

	public virtual bool ProcessMnemonicEx(char charCode)
	{
		if (_1034(m_ParentItem, charCode))
		{
			return true;
		}
		return false;
	}

	public void Close()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		if (_0B98 && ((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			if (m_ParentItem != null && m_ParentItem.IsOnMenuBar)
			{
				popupMenuAccessibleObject._၈((AccessibleEvents)5);
			}
			popupMenuAccessibleObject._၈((AccessibleEvents)7);
		}
		if (m_ParentItem != null)
		{
			object containerControl = m_ParentItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.MouseWheel -= new MouseEventHandler(_102C);
			}
		}
		_102E(null);
		Hide();
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		_0B98 = true;
		return (AccessibleObject)(object)new PopupMenuAccessibleObject(this);
	}

	private void _0BCD()
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

	internal ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = ((լ != null) ? new ItemPaintArgs(_0AC3 as IOwner, (Control)(object)this, _٥, լ) : new ItemPaintArgs(_0AC3 as IOwner, (Control)(object)this, _٥, new ColorScheme(_٥)));
		itemPaintArgs.ໝ = ဣ();
		if (m_ParentItem.DesignMode)
		{
			ISite site = ಌ();
			if (site != null && site.DesignMode)
			{
				itemPaintArgs.DesignerSelection = true;
			}
		}
		return itemPaintArgs;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		if (m_ParentItem != null)
		{
			ItemPaintArgs itemPaintArgs = _0A92(e.Graphics);
			itemPaintArgs.PaintEventArgs = e;
			itemPaintArgs.ClipRectangle = e.ClipRectangle;
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
			if (ե)
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
				((ApplicationButton)m_ParentItem)._1BFF(itemPaintArgs);
			}
			else if (m_ParentItem is MetroAppButton)
			{
				((MetroAppButton)m_ParentItem)._1BFF(itemPaintArgs);
			}
			e.Graphics.SmoothingMode = smoothingMode;
			e.Graphics.TextRenderingHint = textRenderingHint;
		}
	}

	private _2919 ဤ(ItemPaintArgs _0BD2)
	{
		_2919 _2919 = new _2919();
		if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && _0BD2.ໝ is Office2007Renderer)
		{
			Office2007MenuColorTable menu = ((Office2007Renderer)_0BD2.ໝ).ColorTable.Menu;
			_2919.ཥ = menu.Background;
			_2919.ᙴ = menu.Border;
			_2919._291A = menu.Side;
			_2919._291C = menu.SideBorder;
			_2919._291D = menu.SideBorderLight;
			_2919._291B = menu.SideUnused;
		}
		else
		{
			_2919.ཥ.Start = _0BD2.Colors.MenuBackground;
			_2919.ཥ.End = _0BD2.Colors.MenuBackground2;
			_2919.ཥ.GradientAngle = _0BD2.Colors.MenuBackgroundGradientAngle;
			_2919._291A.Start = _0BD2.Colors.MenuSide;
			_2919._291A.End = _0BD2.Colors.MenuSide2;
			_2919._291A.GradientAngle = _0BD2.Colors.MenuSideGradientAngle;
			_2919._291B.Start = _0BD2.Colors.MenuUnusedSide;
			_2919._291B.End = _0BD2.Colors.MenuUnusedSide2;
			_2919._291B.GradientAngle = _0BD2.Colors.MenuUnusedSideGradientAngle;
			_2919.ᙴ.Start = _0BD2.Colors.MenuBorder;
			_2919._291C.Start = _0BD2.Colors.ItemSeparator;
			_2919._291D.Start = _0BD2.Colors.ItemSeparatorShade;
		}
		return _2919;
	}

	protected void PaintDotNet(ItemPaintArgs pa)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (ଌ && !_0B0D)
		{
			_0BDC();
		}
		_2919 _2919 = ဤ(pa);
		DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, _2919.ཥ);
		_0BDB(pa);
		if (m_ParentItem != null)
		{
			Rectangle rectangle = ((Control)this).ClientRectangle;
			if (ଌ && !_0B0D)
			{
				rectangle = new Rectangle(0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
			}
			eDotNetBarStyle effectiveStyle = m_ParentItem.EffectiveStyle;
			if (BarFunctions.IsOffice2007Style(effectiveStyle) && (!StyleManager.IsMetro(effectiveStyle) || StyleManager.IsVisualStudio2012(StyleManager.Style)))
			{
				if (((Control)this).Region != null)
				{
					DisplayHelp.DrawRoundedRectangle(graphics, _2919.ᙴ.Start, rectangle, ன);
					Rectangle rectangle2 = rectangle;
					rectangle2.Inflate(-1, -1);
					Color color = ControlPaint.LightLight(_2919.ཥ.Start);
					if (ဏ)
					{
						color = Color.Empty;
						GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(rectangle2, ன);
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
							DisplayHelp.DrawRoundedRectangle(graphics, val2, rectangle2, ன);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
				else
				{
					DisplayHelp.DrawRectangle(graphics, _2919.ᙴ.Start, rectangle);
				}
			}
			else
			{
				DisplayHelp.DrawGradientRectangle(graphics, rectangle, _2919.ᙴ, 1);
			}
			if (ଌ && !_0B0D && m_ParentItem != null && !BarFunctions.IsOffice2007Style(effectiveStyle))
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
			if ((!StyleManager.IsMetro(effectiveStyle) || StyleManager.IsVisualStudio2012(StyleManager.Style)) && (!BarFunctions.IsOffice2007Style(effectiveStyle) || effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro) && m_ParentItem is ButtonItem && m_ParentItem.Displayed && m_ParentItem.Visible && m_ParentItem.Orientation == eOrientation.Horizontal && !(m_ParentItem.ContainerControl is ContextMenuBar) && ಔ && ((Control)this).Parent != null && m_ParentItemScreenPos.Y < ((Control)this).Parent.Location.Y)
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
			if (_0DC9)
			{
				_ = m_ParentItem.SubItems[0];
				if (_0DCB)
				{
					if (m_ParentItem is ImageItem imageItem)
					{
						Rectangle rectangle3 = new Rectangle(_0B59.Left, _0B59.Top, imageItem.SubItemsImageSize.Width + 7, မ);
						DisplayHelp.FillRectangle(graphics, rectangle3, _2919._291A);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							ဨ(pa, rectangle3, _2919);
						}
					}
					else
					{
						Rectangle rectangle4 = new Rectangle(_0B59.Left, _0B59.Top, 23, မ);
						DisplayHelp.FillRectangle(graphics, rectangle4, _2919._291A);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							ဨ(pa, rectangle4, _2919);
						}
					}
				}
				if (_0DCC)
				{
					if (m_ParentItem is ImageItem imageItem2)
					{
						Rectangle rectangle5 = new Rectangle(_0B59.Left, _0B59.Bottom - ယ - 1, imageItem2.SubItemsImageSize.Width + 7, ယ);
						DisplayHelp.FillRectangle(graphics, rectangle5, _2919._291A);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							ဨ(pa, rectangle5, _2919);
						}
					}
					else
					{
						Rectangle rectangle6 = new Rectangle(_0B59.Left, _0B59.Bottom - ယ - 1, 23, ယ);
						DisplayHelp.FillRectangle(graphics, rectangle6, _2919._291A);
						if (BarFunctions.IsOffice2007Style(effectiveStyle))
						{
							ဨ(pa, rectangle6, _2919);
						}
					}
				}
			}
			ဦ(pa, _2919);
		}
		else
		{
			DisplayHelp.DrawGradientRectangle(graphics, ((Control)this).ClientRectangle, _2919.ᙴ, 1);
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
		_0BDB(pa);
		ControlPaint.DrawBorder3D(pa.Graphics, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
		if (m_ParentItem != null)
		{
			ဦ(pa, ဤ(pa));
		}
	}

	private bool ဥ(BaseItem _06EC)
	{
		if (_06EC is LabelItem || _06EC is ItemContainer)
		{
			return false;
		}
		return true;
	}

	private void ဦ(ItemPaintArgs _0BD2, _2919 ဧ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = _0BD2.Graphics;
		bool flag = (int)((Control)this).RightToLeft == 1;
		if (m_ParentItem == null || m_ParentItem == null)
		{
			return;
		}
		Rectangle rectangle = new Rectangle((int)graphics.ClipBounds.X, (int)graphics.ClipBounds.Y, (int)graphics.ClipBounds.Width, (int)graphics.ClipBounds.Height);
		if (_0DC9)
		{
			if (_0DCB)
			{
				Point[] array = new Point[3];
				array[0].X = _0B59.Left + (_0B59.Width - 8) / 2;
				array[0].Y = _0B59.Top + 8;
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
			for (int i = _0DCA; i < m_ParentItem.SubItems.Count; i++)
			{
				BaseItem baseItem = m_ParentItem.SubItems[i];
				if (!rectangle.IntersectsWith(baseItem.DisplayRectangle) || (!baseItem.Visible && !လ) || !baseItem.Displayed)
				{
					continue;
				}
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				if (baseItem.BeginGroup && baseItem.TopInternal > ଉ)
				{
					if (baseItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if (လ)
						{
							ControlPaint.DrawBorder3D(graphics, baseItem.HeightInternal + Dpi.Height2 + baseItem.LeftInternal + Dpi.Width13, baseItem.TopInternal - Dpi.Height5, baseItem.WidthInternal - Dpi.Width26 - baseItem.HeightInternal - Dpi.Height2, Dpi.Height2, (Border3DStyle)6, (Border3DSide)2);
						}
						else
						{
							ControlPaint.DrawBorder3D(graphics, baseItem.LeftInternal + Dpi.Width13, baseItem.TopInternal - Dpi.Height5, baseItem.WidthInternal - Dpi.Width26, Dpi.Width2, (Border3DStyle)6, (Border3DSide)2);
						}
					}
					else if (လ)
					{
						Rectangle rectangle2 = new Rectangle(baseItem.LeftInternal, baseItem.TopInternal - Dpi.Height3, baseItem.HeightInternal + Dpi.Height2 + num + Dpi.Width7, Dpi.Height3);
						rectangle2.Inflate(0, Dpi.Height1);
						DisplayHelp.FillRectangle(graphics, rectangle2, ဧ._291A.Start, ဧ._291A.End, ဧ._291A.GradientAngle);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							ဨ(_0BD2, rectangle2, ဧ);
						}
						DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + num + Dpi.Width8 + Dpi.Width7 + baseItem.HeightInternal + Dpi.Width2, baseItem.TopInternal - Dpi.Height2, baseItem.DisplayRectangle.Right - Dpi.Width1, baseItem.TopInternal - Dpi.Height2, ဧ._291C.Start, Dpi.Width1);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + num + Dpi.Width8 + Dpi.Width7 + baseItem.HeightInternal + Dpi.Width2, baseItem.TopInternal - Dpi.Height1, baseItem.DisplayRectangle.Right - Dpi.Width1, baseItem.TopInternal - Dpi.Height1, ဧ._291D.Start, Dpi.Width1);
						}
					}
					else
					{
						bool flag3 = ဥ(baseItem);
						bool flag4 = flag3;
						if (flag3)
						{
							Rectangle rectangle3 = new Rectangle(baseItem.LeftInternal, baseItem.TopInternal - Dpi.Width3, num + Dpi.Width7, Dpi.Height3);
							rectangle3.Inflate(0, Dpi.Height1);
							if (baseItem is IPersonalizedMenuItem && ((IPersonalizedMenuItem)baseItem).MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !((IPersonalizedMenuItem)baseItem).RecentlyUsed)
							{
								DisplayHelp.FillRectangle(graphics, rectangle3, ဧ._291B);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle3, ဧ._291A);
							}
							if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
							{
								ဨ(_0BD2, rectangle3, ဧ);
							}
						}
						if (flag4 && flag2)
						{
							flag4 = false;
						}
						DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + (flag4 ? (num + Dpi.Width8 + Dpi.Width7) : 0), baseItem.TopInternal - Dpi.Height2, baseItem.DisplayRectangle.Right - Dpi.Width1, baseItem.TopInternal - Dpi.Height2, ဧ._291C.Start, Dpi.Width1);
						if (BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle))
						{
							DisplayHelp.DrawLine(graphics, baseItem.LeftInternal + (flag4 ? (num + Dpi.Width8 + Dpi.Width7) : 0), baseItem.TopInternal - Dpi.Height1, baseItem.DisplayRectangle.Right - Dpi.Width1, baseItem.TopInternal - Dpi.Height1, ဧ._291D.Start, Dpi.Width1);
						}
					}
				}
				graphics.SmoothingMode = smoothingMode;
				baseItem.Paint(_0BD2);
				flag2 = baseItem.IsContainer;
			}
			if (_0DCC)
			{
				Point[] array = new Point[3];
				array[0].X = _0B59.Left + (_0B59.Width - 8) / 2;
				array[0].Y = _0B59.Bottom - 8;
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
			int num2 = Dpi.Width(16);
			if (imageItem2 != null)
			{
				num2 = imageItem2.SubItemsImageSize.Width;
			}
			imageItem2 = null;
			bool flag5 = true;
			bool flag6 = false;
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if (!rectangle.IntersectsWith(subItem.DisplayRectangle) || ((!subItem.Visible || !subItem.Displayed) && !လ))
				{
					continue;
				}
				SmoothingMode smoothingMode2 = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				if (subItem.BeginGroup && !flag5)
				{
					if (subItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if (လ)
						{
							ControlPaint.DrawBorder3D(graphics, subItem.HeightInternal + 2 + subItem.LeftInternal + 13, subItem.TopInternal - 5, subItem.WidthInternal - 26 - subItem.HeightInternal - 2, 2, (Border3DStyle)6, (Border3DSide)2);
						}
						else
						{
							ControlPaint.DrawBorder3D(graphics, subItem.LeftInternal + 13, subItem.TopInternal - 5, subItem.WidthInternal - 26, 2, (Border3DStyle)6, (Border3DSide)2);
						}
					}
					else if (လ)
					{
						Rectangle rectangle4 = new Rectangle(subItem.LeftInternal, subItem.TopInternal - 3, subItem.HeightInternal + 2 + num2 + 7, 3);
						rectangle4.Inflate(0, 1);
						if (flag)
						{
							rectangle4.Width = num2 + 7;
							rectangle4.X = subItem.DisplayRectangle.Right - rectangle4.Width;
						}
						DisplayHelp.FillRectangle(graphics, rectangle4, ဧ._291A);
						if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
						{
							ဨ(_0BD2, rectangle4, ဧ);
						}
						if (flag)
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - (num2 + 8 + 7 + subItem.HeightInternal + 2 + 1), subItem.TopInternal - 2, ဧ._291C.Start, 1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + 1, subItem.TopInternal - 1, subItem.DisplayRectangle.Right - (num2 + 8 + 7 + subItem.HeightInternal + 2 + 1), subItem.TopInternal - 1, ဧ._291D.Start, 1);
							}
						}
						else
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + num2 + 8 + 7 + subItem.HeightInternal + 2, subItem.TopInternal - 2, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 2, ဧ._291C.Start, 1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + num2 + 8 + 7 + subItem.HeightInternal + 2, subItem.TopInternal - 1, subItem.DisplayRectangle.Right - 1, subItem.TopInternal - 1, ဧ._291D.Start, 1);
							}
						}
					}
					else
					{
						Rectangle rectangle5 = new Rectangle(subItem.LeftInternal, subItem.TopInternal - Dpi.Height2, num2 + Dpi.Width7, Dpi.Height3);
						rectangle5.Inflate(0, Dpi.Height2);
						if (flag)
						{
							rectangle5.X = subItem.DisplayRectangle.Right - rectangle5.Width - Dpi.Width1;
						}
						bool flag7 = ဥ(subItem);
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
								DisplayHelp.FillRectangle(graphics, rectangle5, ဧ._291B);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle5, ဧ._291A);
							}
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								ဨ(_0BD2, rectangle5, ဧ);
							}
						}
						if (flag)
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + Dpi.Width1, subItem.TopInternal - Dpi.Height2, subItem.DisplayRectangle.Right - (flag8 ? (num2 + Dpi.Width8 + Dpi.Width7 + Dpi.Width1) : 0), subItem.TopInternal - Dpi.Height2, ဧ._291C.Start, Dpi.Width1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle))
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + Dpi.Width1, subItem.TopInternal - Dpi.Height1, subItem.DisplayRectangle.Right - (flag8 ? (num2 + Dpi.Width8 + Dpi.Width7 + Dpi.Width1) : 0), subItem.TopInternal - Dpi.Height1, ဧ._291D.Start, Dpi.Width1);
							}
						}
						else
						{
							DisplayHelp.DrawLine(graphics, subItem.LeftInternal + (flag8 ? (num2 + Dpi.Width8 + Dpi.Width7) : 0), subItem.TopInternal - Dpi.Height2, subItem.DisplayRectangle.Right - Dpi.Width1, subItem.TopInternal - Dpi.Height2, _0BD2.Colors.ItemSeparator, Dpi.Width1);
							if (BarFunctions.IsOffice2007Style(subItem.EffectiveStyle) && !_0BD2.Colors.ItemSeparatorShade.IsEmpty)
							{
								DisplayHelp.DrawLine(graphics, subItem.LeftInternal + (flag8 ? (num2 + Dpi.Width8 + Dpi.Width7) : 0), subItem.TopInternal - Dpi.Height1, subItem.DisplayRectangle.Right - Dpi.Width1, subItem.TopInternal - Dpi.Height1, _0BD2.Colors.ItemSeparatorShade, Dpi.Width1);
							}
						}
					}
				}
				graphics.SmoothingMode = smoothingMode2;
				flag5 = false;
				subItem.Paint(_0BD2);
				subItem.Displayed = true;
				flag6 = subItem.IsContainer;
			}
		}
		_102B(_0BD2, ဧ);
	}

	private void ဨ(ItemPaintArgs _0BD2, Rectangle ဩ, _2919 ဧ)
	{
		if (!(_0BD2.Owner is DotNetBarManager))
		{
			Graphics graphics = _0BD2.Graphics;
			if (_0BD2.RightToLeft)
			{
				Point point = new Point(ဩ.X, ဩ.Y);
				DisplayHelp.DrawLine(graphics, point.X, point.Y, point.X, point.Y + ဩ.Height, ဧ._291D.Start, 1);
				point.X++;
				DisplayHelp.DrawLine(graphics, point.X, point.Y, point.X, point.Y + ဩ.Height, ဧ._291C.Start, 1);
			}
			else
			{
				Point point2 = new Point(ဩ.Right - 2, ဩ.Y);
				DisplayHelp.DrawLine(graphics, point2.X, point2.Y, point2.X, point2.Y + ဩ.Height, ဧ._291C.Start, 1);
				point2.X++;
				DisplayHelp.DrawLine(graphics, point2.X, point2.Y, point2.X, point2.Y + ဩ.Height, ဧ._291D.Start, 1);
			}
		}
	}

	private void ဪ()
	{
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			ItemPaintArgs itemPaintArgs = ((լ != null) ? new ItemPaintArgs(_0AC3 as IOwner, (Control)(object)this, val, լ) : new ItemPaintArgs(_0AC3 as IOwner, (Control)(object)this, val, new ColorScheme(val)));
			_102B(itemPaintArgs, ဤ(itemPaintArgs));
		}
		finally
		{
			val.Dispose();
		}
	}

	private void _102B(ItemPaintArgs _0BD2, _2919 ဧ)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Invalid comparison between Unknown and I4
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		if (!သ._၆ || m_ParentItem == null)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		ImageItem imageItem = m_ParentItem as ImageItem;
		int num = 16;
		if (imageItem != null && imageItem.SubItemsImageSize.Width > num)
		{
			num = imageItem.SubItemsImageSize.Width;
		}
		num += 7;
		imageItem = null;
		if (_0AFB)
		{
			if (သ.ज़)
			{
				Rectangle _၇ = သ._၇;
				DisplayHelp.FillRectangle(graphics, _၇, _0BD2.Colors.MenuBackground, Color.Empty);
				_၇.Inflate(-1, 0);
				if ((int)Control.MouseButtons == 1048576)
				{
					DisplayHelp.FillRectangle(graphics, _၇, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, _၇, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				}
				_089F.र(graphics, SystemPens.Highlight, _၇);
			}
			else
			{
				Rectangle bounds = new Rectangle(သ._၇.Left + num, သ._၇.Top, သ._၇.Width - num, သ._၇.Height);
				DisplayHelp.FillRectangle(graphics, bounds, _0BD2.Colors.MenuBackground, Color.Empty);
				Rectangle rectangle = new Rectangle(သ._၇.Left, သ._၇.Top, num, သ._၇.Height);
				DisplayHelp.FillRectangle(graphics, rectangle, _0BD2.Colors.MenuSide, _0BD2.Colors.MenuSide2, _0BD2.Colors.MenuSideGradientAngle);
				if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					ဨ(_0BD2, rectangle, ဧ);
				}
			}
			if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
			{
				GraphicsPath val = new GraphicsPath();
				Point point = new Point(သ._၇.X + (သ._၇.Width - 16) / 2, သ._၇.Y + (သ._၇.Height - 16) / 2);
				val.AddEllipse(point.X, point.Y, 16, 16);
				PathGradientBrush val2 = new PathGradientBrush(val);
				val2.CenterColor = _0BD2.Colors.MenuSide;
				val2.SurroundColors = new Color[1] { _0BD2.Colors.MenuSide2 };
				val2.CenterPoint = new Point(point.X + 3, point.Y + 3);
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.FillEllipse((Brush)(object)val2, point.X, point.Y, 16, 16);
				graphics.SmoothingMode = smoothingMode;
			}
		}
		else if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			if (သ.ज़)
			{
				graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()), သ._၇);
				if ((int)Control.MouseButtons == 1048576)
				{
					ControlPaint.DrawBorder(graphics, သ._၇, SystemColors.Control, (ButtonBorderStyle)4);
				}
				else
				{
					ControlPaint.DrawBorder3D(graphics, သ._၇, (Border3DStyle)4, (Border3DSide)15);
				}
			}
			else
			{
				graphics.FillRectangle(SystemBrushes.Control, သ._၇);
			}
		}
		Point[] array = new Point[3];
		array[0].X = သ._၇.Left + (သ._၇.Width - 4) / 2;
		if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
		{
			array[0].Y = သ._၇.Top + (သ._၇.Height - 7) / 2;
		}
		else
		{
			array[0].Y = သ._၇.Top + 3;
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

	private void _0BDB(ItemPaintArgs _0BD2)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		if (_0B5A.Picture != null)
		{
			if (!_0B5A.GradientColor1.IsEmpty && !_0B5A.GradientColor2.IsEmpty)
			{
				PointF[] array = new PointF[2];
				array[0].X = ရ.Left;
				array[0].Y = ရ.Top;
				array[1].X = ရ.Left;
				array[1].Y = ရ.Bottom;
				LinearGradientBrush val = BarFunctions.ॴ(ရ, _0B5A.GradientColor1, _0B5A.GradientColor2, _0B5A.GradientAngle);
				graphics.FillRectangle((Brush)(object)val, ရ);
			}
			else if (!_0B5A.BackColor.Equals((object?)Color.Empty))
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0B5A.BackColor), ရ);
			}
			if (_0B5A.StretchPicture)
			{
				graphics.DrawImage(_0B5A.Picture, ရ);
			}
			else if (_0B5A.Alignment == eAlignment.Top)
			{
				graphics.DrawImage(_0B5A.Picture, ရ.X, ရ.Top, ရ.Width, _0B5A.Picture.Height);
			}
			else if (_0B5A.Alignment == eAlignment.Bottom)
			{
				graphics.DrawImage(_0B5A.Picture, ရ.Left, ရ.Bottom - _0B5A.Picture.Height, _0B5A.Picture.Width, _0B5A.Picture.Height);
			}
			else
			{
				graphics.DrawImage(_0B5A.Picture, ရ.Left, ရ.Top + (ရ.Height - _0B5A.Picture.Height) / 2, _0B5A.Picture.Width, _0B5A.Picture.Height);
			}
		}
	}

	private void _102C(object _٠, MouseEventArgs ؿ)
	{
		if (!_0DC9)
		{
			return;
		}
		if (ؿ.Delta > 0)
		{
			if (_0DCB)
			{
				_1039();
			}
		}
		else if (_0DCC)
		{
			_103A();
		}
	}

	private void _0BDC()
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
				_௮();
			}
			else
			{
				_௬();
			}
			if (((Control)this).Visible && ಔ && ((Control)this).Parent != null && ((Control)this).Parent is PopupContainer)
			{
				ঊ ঊ2 = BarFunctions.ॻ(((Control)this).Parent);
				if (ঊ2 != null)
				{
					Rectangle rectangle = ঊ2.ঌ;
					if (ဢ)
					{
						rectangle = ঊ2.ঋ;
					}
					if (((Control)this).Parent.Location.Y + ((Control)this).Size.Height > rectangle.Bottom)
					{
						_0B59.Height -= ((Control)this).Parent.Location.Y + ((Control)this).Size.Height - rectangle.Bottom;
						((Control)this).Height = ((Control)this).Height - (((Control)this).Parent.Location.Y + ((Control)this).Size.Height - rectangle.Bottom);
						if (!_0DC9)
						{
							_0DC9 = true;
							_0DCA = 0;
						}
						_0DDE();
					}
					else
					{
						_0DC9 = false;
					}
				}
			}
		}
		if (!((Control)this).Visible)
		{
			return;
		}
		if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && တ)
		{
			if (((Control)this).Parent != null)
			{
				_0BDD(((Control)this).Parent);
			}
			_0BDD((Control)(object)this);
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

	private void _௬()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		သ._၆ = false;
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			((Control)this).Height = 24;
			((Control)this).Width = m_ParentItem.WidthInternal + 16;
			return;
		}
		int num = 0;
		int num2 = ଉ;
		int num3 = ଈ;
		int num4 = 0;
		bool flag = false;
		_ = ((Control)this).RightToLeft;
		if (_0B5A.Picture != null)
		{
			num3 += _0B5A.Picture.Width;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.RecalcSize();
			if (subItem.Visible || လ)
			{
				if (!m_ParentItem.DesignMode && ಛ != 0 && !သ.ਸ && !လ && subItem is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed, RecentlyUsed: false })
				{
					subItem.Displayed = false;
					သ._၆ = true;
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
				if (subItem.BeginGroup && num2 > ଉ)
				{
					num2 += Dpi.Height(3);
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
			num2 = ଉ;
			foreach (BaseItem subItem2 in m_ParentItem.SubItems)
			{
				subItem2.WidthInternal = num;
				if (subItem2.Displayed)
				{
					if (subItem2.BeginGroup && num2 > ଉ)
					{
						num2 += Dpi.Height(3);
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
		if (သ._၆)
		{
			သ._၇ = new Rectangle(num3, num2, num, ဝ);
			num2 += ဝ;
		}
		_0B59 = new Rectangle(num3, ଉ, num, num2 - ଉ);
		if (_0B5A.Picture != null)
		{
			ရ = new Rectangle(num3 - _0B5A.Picture.Width, ଉ, _0B5A.Picture.Width, num2 - ଉ);
		}
		((Control)this).Size = new Size(num3 + num + ଊ, num2 + ଋ);
		if (((Control)this).Height == 92)
		{
			((Control)this).Height = 92;
		}
	}

	private void _௮()
	{
		သ._၆ = false;
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			((Control)this).Height = 24;
			((Control)this).Width = m_ParentItem.WidthInternal + 16;
			return;
		}
		int num = 0;
		int num2 = ଉ;
		int num3 = ଈ;
		int num4 = 0;
		bool flag = false;
		if (_0B5A.Picture != null)
		{
			num3 += _0B5A.Picture.Width;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.RecalcSize();
			if (subItem.Visible || လ)
			{
				if (!m_ParentItem.DesignMode && ಛ != 0 && !သ.ਸ && !လ && subItem is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed, RecentlyUsed: false })
				{
					subItem.Displayed = false;
					သ._၆ = true;
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
				if (subItem.BeginGroup && num2 > ଉ)
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
					if (subItem2.BeginGroup && num2 > ଉ)
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
		if (သ._၆)
		{
			သ._၇ = new Rectangle(num3, num2, num, ဝ);
			num2 += ဝ;
		}
		_0B59 = new Rectangle(num3, ଉ, num, num2 - ଉ);
		if (_0B5A.Picture != null)
		{
			ရ = new Rectangle(num3 - _0B5A.Picture.Width, ଉ, _0B5A.Picture.Width, num2);
		}
		((Control)this).Size = new Size(num3 + num + ଊ, num2 + ଋ);
	}

	private void _௯()
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

	private BaseItem _102D()
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

	internal void _102E(BaseItem _102F)
	{
		if (m_ParentItem != null && m_ParentItem.Parent == null)
		{
			BaseItem baseItem = FocusedItem();
			if (baseItem != _102F)
			{
				baseItem?.OnLostFocus();
				_102F?.OnGotFocus();
			}
		}
	}

	internal void _0BFD(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseMove(ؿ);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Invalid comparison between Unknown and I4
		((Control)this).OnMouseMove(e);
		if (_0DCF)
		{
			if (_0DD0 == null)
			{
				_0DD0 = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
				return;
			}
			if (_0DD0.X == e.X && _0DD0.Y == e.Y && _0DD0.Button == e.Button)
			{
				return;
			}
			_0DCF = false;
			_0DD0 = null;
		}
		if (m_ParentItem != null && m_ParentItem.DesignMode && (int)e.Button == 1048576 && (Math.Abs(e.X - _0AC2.X) >= 2 || Math.Abs(e.Y - _0AC2.Y) >= 2 || _0BCB))
		{
			BaseItem baseItem = FocusedItem();
			IOwner owner = _0AC3 as IOwner;
			ISite site = ಌ();
			if (site != null && baseItem != null)
			{
				_0C8D(e);
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
		BaseItem baseItem3 = _102D();
		if (baseItem3 != null && baseItem3 is PopupItem)
		{
			Control popupControl = ((PopupItem)baseItem3).PopupControl;
			if (popupControl != null)
			{
				Point pt = popupControl.PointToClient(((Control)this).PointToScreen(new Point(e.X, e.Y)));
				if (popupControl.ClientRectangle.Contains(pt) && popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl)._0BFD(new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));
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
				_1031(baseItem3);
			}
		}
		if (သ._၆)
		{
			if (baseItem2 == null && သ._၇.Contains(e.X, e.Y) && !သ.ज़)
			{
				သ.ज़ = true;
				ဪ();
			}
			else if (baseItem2 != null && သ.ज़)
			{
				သ.ज़ = false;
				ဪ();
			}
		}
		if (baseItem2 == null && m_HotSubItem is GalleryContainer)
		{
			GalleryContainer galleryContainer = m_HotSubItem as GalleryContainer;
			if (!galleryContainer.ᥳ && galleryContainer.PopupUsesStandardScrollbars && galleryContainer.ᥥ != null && galleryContainer.ᥥ.IsMouseDown && (int)e.Button == 1048576)
			{
				baseItem2 = m_HotSubItem;
			}
		}
		if (baseItem2 != m_HotSubItem)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (ဘ)
				{
					ఘ();
					ဘ = false;
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

	private ISite ಌ()
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

	private void _0C8D(MouseEventArgs ؿ)
	{
		if (_0BCB)
		{
			try
			{
				if (_৫ != null)
				{
					_৫.DrawReversibleMarker(_৬, _৭);
					_৫ = null;
				}
				InsertPosition insertPosition = _0BCC.GetInsertPosition(Control.MousePosition, ঢ়);
				if (insertPosition != null)
				{
					if (insertPosition.TargetProvider == null)
					{
						Cursor.Current = Cursors.No;
					}
					else
					{
						insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
						_৬ = insertPosition.Position;
						_৭ = insertPosition.Before;
						_৫ = insertPosition.TargetProvider;
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
				_৫ = null;
				return;
			}
		}
		BaseItem baseItem = m_ParentItem;
		while (baseItem.Parent is IDesignTimeProvider)
		{
			baseItem = baseItem.Parent;
		}
		ISite site = ಌ();
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
		_0BCC = (IDesignTimeProvider)baseItem;
		ঢ় = FocusedItem();
		_0BCB = true;
		((Control)this).Capture = true;
	}

	private void ಎ(MouseEventArgs ؿ)
	{
		ISite site = ಌ();
		if (site == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (_৫ != null)
		{
			_৫.DrawReversibleMarker(_৬, _৭);
			BaseItem parent = ঢ়.Parent;
			if (parent != null)
			{
				if (parent == (BaseItem)_৫ && _৬ > 0 && parent.SubItems.IndexOf(ঢ়) < _৬)
				{
					_৬--;
				}
				componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
				parent.SubItems.Remove(ঢ়);
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
			_৫.InsertItemAt(ঢ়, _৬, _৭);
			_৫ = null;
		}
		_0BCC = null;
		ঢ় = null;
		_0BCB = false;
		((Control)this).Capture = false;
	}

	private void ಏ(MouseEventArgs ؿ)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		IOwner owner = Owner as IOwner;
		BaseItem baseItem = ItemAtLocation(ؿ.X, ؿ.Y);
		if (baseItem != null && baseItem.Parent is GalleryContainer)
		{
			GalleryContainer galleryContainer = baseItem.Parent as GalleryContainer;
			if (galleryContainer.ᦐ(ؿ.X, ؿ.Y))
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
			if ((int)ؿ.Button != 2097152)
			{
				return;
			}
			ISite site = ಌ();
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
		BaseItem baseItem2 = _102D();
		if (baseItem2 != null && m_HotSubItem != baseItem2)
		{
			baseItem2.Expanded = false;
		}
		if (owner != null)
		{
			owner.SetFocusItem(baseItem);
			ISite site2 = ಌ();
			if (site2 != null)
			{
				ISelectionService selectionService2 = (ISelectionService)site2.GetService(typeof(ISelectionService));
				if (selectionService2 != null)
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(baseItem);
					selectionService2.SetSelectedComponents(arrayList, SelectionTypes.Click);
				}
				if ((int)ؿ.Button == 2097152)
				{
					((IMenuCommandService)site2.GetService(typeof(IMenuCommandService)))?.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 1280), Control.MousePosition.X, Control.MousePosition.Y);
				}
			}
		}
		owner = null;
		baseItem?.InternalMouseDown(ؿ);
	}

	internal void _1030()
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
			if (subItem.Displayed && subItem.GetEnabled() && (!(subItem is ItemContainer) || ((ItemContainer)subItem)._1030()))
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
			if (ဘ)
			{
				ఘ();
				ဘ = false;
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

	private void _1031(BaseItem _06EC)
	{
		ఘ();
		if (ဠ != null && ဠ != _06EC)
		{
			ဠ.Expanded = false;
		}
		if (!အ)
		{
			_06EC.Expanded = false;
			ဠ = null;
			အ = true;
		}
		else
		{
			ဠ = _06EC;
			ဠ.InternalMouseLeave();
		}
	}

	internal void ఆ()
	{
		if ((m_ParentItem != null && m_ParentItem.DesignMode) || m_ParentItem == null)
		{
			return;
		}
		if (ဠ != null)
		{
			ဠ.Expanded = false;
			ဠ = null;
		}
		if (m_HotSubItem != null)
		{
			bool flag = false;
			flag = m_HotSubItem.Expanded;
			m_HotSubItem.InternalMouseHover();
			ဘ = true;
			if (!flag && m_HotSubItem != null && m_HotSubItem.Expanded)
			{
				အ = true;
			}
			else
			{
				အ = true;
			}
		}
		else if (သ._၆ && သ.ज़)
		{
			if (ಛ != ePersonalizedMenus.DisplayOnClick)
			{
				သ.ਸ = true;
				if (_0AC3 is IOwnerMenuSupport ownerMenuSupport)
				{
					ownerMenuSupport.PersonalizedAllVisible = true;
				}
				RecalcSize();
				ఘ();
			}
		}
		else if (_0DC9 && ဟ == null)
		{
			_103B();
			ఘ();
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		ఆ();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
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
		else if (သ._၆ && သ.ज़)
		{
			သ.ज़ = false;
			ဪ();
		}
		if (m_HotSubItem == null)
		{
			BaseItem baseItem = _102D();
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
		_0AC2 = new Point(e.X, e.Y);
		if (m_ParentItem.DesignMode)
		{
			ಏ(e);
		}
		if (!((Component)this).DesignMode && owner != null && owner.GetFocusItem() != null)
		{
			BaseItem baseItem = ItemAtLocation(e.X, e.Y);
			if (baseItem != owner.GetFocusItem())
			{
				owner.GetFocusItem().ReleaseFocus();
			}
		}
		if ((int)e.Button == 2097152 && m_HotSubItem != null && !ဒ && owner is IRibbonCustomize && m_ParentItem != null && m_ParentItem.Name != "syscustomizepopupmenu")
		{
			((IRibbonCustomize)owner).ItemRightClick(m_HotSubItem);
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseDown(e);
		}
		else if (သ._၆ && သ.ज़)
		{
			ဪ();
		}
		((Control)this).OnMouseDown(e);
	}

	internal void ఇ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_0BCB)
		{
			ಎ(e);
		}
		else if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseUp(e);
		}
		else if (သ._၆ && သ.ज़)
		{
			if (ಛ != ePersonalizedMenus.DisplayOnHover && သ._၇.Contains(e.X, e.Y))
			{
				_1032();
			}
			else
			{
				ဪ();
			}
		}
		((Control)this).OnMouseUp(e);
	}

	internal void _1032()
	{
		if (ಛ != ePersonalizedMenus.DisplayOnHover)
		{
			သ.ਸ = true;
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
			else if (_0DC9)
			{
				_103B();
			}
			((Control)this).OnClick(e);
		}
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		ISite site = ಌ();
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
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	private bool _1033(BaseItem _06EC)
	{
		if (_06EC is LabelItem)
		{
			return false;
		}
		return true;
	}

	internal void _0A81(KeyEventArgs ؿ)
	{
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Invalid comparison between Unknown and I4
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Invalid comparison between Unknown and I4
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Invalid comparison between Unknown and I4
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Invalid comparison between Unknown and I4
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Invalid comparison between Unknown and I4
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Invalid comparison between Unknown and I4
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Invalid comparison between Unknown and I4
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Invalid comparison between Unknown and I4
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0 || m_ParentItem.DesignMode)
		{
			return;
		}
		if (m_HotSubItem == null || (m_HotSubItem != null && !m_HotSubItem.Expanded))
		{
			if ((int)ؿ.KeyCode == 40)
			{
				if (m_ParentItem is ItemContainer)
				{
					m_ParentItem.InternalKeyDown(ؿ);
				}
				else
				{
					if (m_HotSubItem is ItemContainer)
					{
						m_HotSubItem.InternalKeyDown(ؿ);
					}
					if (!ؿ.Handled)
					{
						int num = 0;
						if (m_HotSubItem != null)
						{
							m_HotSubItem.InternalMouseLeave();
							num = m_ParentItem.SubItems.IndexOf(m_HotSubItem) + 1;
							if (num == m_ParentItem.SubItems.Count && သ._၆)
							{
								သ.ज़ = true;
								ဪ();
							}
							else if (num < 0 || num == m_ParentItem.SubItems.Count)
							{
								num = 0;
							}
							m_HotSubItem = null;
						}
						else if (သ.ज़)
						{
							သ.ज़ = false;
							ဪ();
							num = 0;
						}
						for (int i = num; i < m_ParentItem.SubItems.Count; i++)
						{
							BaseItem baseItem = m_ParentItem.SubItems[i];
							if (baseItem.Displayed && baseItem.Visible && _1033(baseItem))
							{
								m_HotSubItem = baseItem;
								m_HotSubItem.InternalMouseEnter();
								m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
								if (m_HotSubItem.IsContainer)
								{
									m_HotSubItem.InternalKeyDown(ؿ);
								}
								break;
							}
						}
						if (m_HotSubItem == null && သ._၆ && !သ.ज़)
						{
							သ.ज़ = true;
							ဪ();
						}
					}
				}
				ؿ.Handled = true;
				_0DCF = true;
			}
			else if ((int)ؿ.KeyCode == 38)
			{
				if (m_ParentItem is ItemContainer)
				{
					m_ParentItem.InternalKeyDown(ؿ);
				}
				else
				{
					if (m_HotSubItem is ItemContainer)
					{
						m_HotSubItem.InternalKeyDown(ؿ);
					}
					if (!ؿ.Handled)
					{
						int num2 = 0;
						if (m_HotSubItem != null)
						{
							m_HotSubItem.InternalMouseLeave();
							num2 = m_ParentItem.SubItems.IndexOf(m_HotSubItem) - 1;
							if (num2 < 0 && သ._၆)
							{
								သ.ज़ = true;
								ဪ();
							}
							else if (num2 < 0)
							{
								num2 = m_ParentItem.SubItems.Count - 1;
							}
							m_HotSubItem = null;
						}
						else if (သ.ज़)
						{
							သ.ज़ = false;
							ဪ();
							num2 = m_ParentItem.SubItems.Count - 1;
						}
						else if (သ._၆)
						{
							သ.ज़ = true;
							ဪ();
						}
						else
						{
							num2 = m_ParentItem.SubItems.Count - 1;
						}
						for (int num3 = num2; num3 >= 0; num3--)
						{
							BaseItem baseItem2 = m_ParentItem.SubItems[num3];
							if (baseItem2.Displayed && baseItem2.Visible && _1033(baseItem2))
							{
								m_HotSubItem = baseItem2;
								m_HotSubItem.InternalMouseEnter();
								m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
								if (m_HotSubItem.IsContainer)
								{
									m_HotSubItem.InternalKeyDown(ؿ);
								}
								break;
							}
						}
						if (m_HotSubItem == null && သ._၆ && !သ.ज़)
						{
							သ.ज़ = true;
							ဪ();
						}
					}
				}
				ؿ.Handled = true;
				_0DCF = true;
			}
			else if ((int)ؿ.KeyCode == 39)
			{
				_0DCF = true;
				if (m_HotSubItem != null && m_HotSubItem.GetEnabled() && m_HotSubItem is ButtonItem buttonItem && buttonItem.SubItems.Count > 0 && buttonItem.ShowSubItems && !buttonItem.Expanded)
				{
					buttonItem.Expanded = true;
					if (buttonItem.PopupControl is MenuPanel)
					{
						((MenuPanel)(object)buttonItem.PopupControl)._1030();
					}
					ؿ.Handled = true;
				}
				if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
				{
					m_HotSubItem.InternalKeyDown(ؿ);
				}
				if (!ؿ.Handled && m_ParentItem != null)
				{
					foreach (BaseItem subItem in m_ParentItem.SubItems)
					{
						if (subItem is ItemContainer)
						{
							subItem.InternalKeyDown(ؿ);
							if (ؿ.Handled)
							{
								break;
							}
						}
					}
				}
			}
			else if ((int)ؿ.KeyCode == 37)
			{
				if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
				{
					m_HotSubItem.InternalKeyDown(ؿ);
				}
				if (!ؿ.Handled)
				{
					_0DCF = true;
					if (BaseItem.IsOnPopup(m_ParentItem))
					{
						m_ParentItem.Expanded = false;
						ؿ.Handled = true;
					}
				}
			}
			else if ((int)ؿ.KeyCode == 27)
			{
				BaseItem parentItem = m_ParentItem;
				parentItem.Expanded = false;
				if (parentItem.Parent != null && parentItem.Parent is GenericItemContainer && parentItem.Parent.AutoExpand)
				{
					parentItem.Parent.AutoExpand = false;
				}
				ؿ.Handled = true;
			}
			else if ((int)ؿ.KeyCode == 13 && m_HotSubItem != null && m_HotSubItem.IsContainer)
			{
				m_HotSubItem.InternalKeyDown(ؿ);
			}
			else if ((int)ؿ.KeyCode == 13 && m_HotSubItem != null && m_HotSubItem.SubItems.Count > 0 && m_HotSubItem.ShowSubItems && !m_HotSubItem.Expanded && m_HotSubItem.GetEnabled())
			{
				m_HotSubItem.Expanded = true;
				if (m_HotSubItem is PopupItem && ((PopupItem)m_HotSubItem).PopupControl is MenuPanel)
				{
					((MenuPanel)(object)((PopupItem)m_HotSubItem).PopupControl)._1030();
				}
				ؿ.Handled = true;
			}
			else if (((int)ؿ.KeyCode == 13 || (int)ؿ.KeyCode == 32) && သ._၆ && သ.ज़)
			{
				_1032();
			}
			else
			{
				int num4 = 0;
				if (ؿ.Shift)
				{
					try
					{
						byte[] array = new byte[256];
						if (_089F.GetKeyboardState(array))
						{
							byte[] array2 = new byte[2];
							if (_089F.ToAscii((uint)ؿ.KeyValue, 0u, array, array2, 0u) != 0)
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
				if (num4 == 0 && !BarFunctions.IsSystemKey(ؿ.KeyCode))
				{
					num4 = (int)_089F.MapVirtualKey((uint)ؿ.KeyValue, 2u);
					if (num4 == 0)
					{
						num4 = ؿ.KeyValue;
					}
				}
				if (ؿ is KeyEventArgsEx && ((KeyEventArgsEx)(object)ؿ).KeyChar != 0 && _1034(m_ParentItem, ((KeyEventArgsEx)(object)ؿ).KeyChar))
				{
					ؿ.Handled = true;
				}
				if (!ؿ.Handled && num4 > 0)
				{
					char[] array3 = new char[1];
					byte[] bytes = new byte[1] { Convert.ToByte(num4) };
					Encoding.Default.GetDecoder().GetChars(bytes, 0, 1, array3, 0);
					string text = array3[0].ToString();
					array3[0] = text.ToLower()[0];
					if ((Control.ModifierKeys & 0x20000) == 0 && _1034(m_ParentItem, array3[0]))
					{
						ؿ.Handled = true;
					}
				}
			}
		}
		if (ؿ.Handled || m_HotSubItem == null)
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
			m_HotSubItem.InternalKeyDown(ؿ);
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
			_0A81(val);
			if (val.Handled)
			{
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private bool _1034(BaseItem ઇ, char _੬)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		List<BaseItem> list = _1035(ઇ, _੬);
		if (list.Count > 1)
		{
			BaseItem baseItem = null;
			if (m_HotSubItem == null)
			{
				baseItem = list[0];
			}
			else
			{
				int num = list.IndexOf(m_HotSubItem);
				if (num == -1)
				{
					baseItem = list[0];
				}
				else
				{
					num = ((num != list.Count - 1) ? (num + 1) : 0);
					baseItem = list[num];
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
		BaseItem itemForMnemonic = GetItemForMnemonic(ઇ, _੬, deepScan: true);
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
					((MenuPanel)(object)((PopupItem)itemForMnemonic).PopupControl)._1030();
				}
				_0DCF = true;
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

	private List<BaseItem> _1035(BaseItem ઇ, char ઈ)
	{
		List<BaseItem> list = new List<BaseItem>();
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			if (Control.IsMnemonic(ઈ, subItem.Text) && subItem.Visible && subItem.GetEnabled())
			{
				list.Add(subItem);
			}
		}
		return list;
	}

	protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan)
	{
		string text = _0A00 + charCode;
		text = text.ToUpper();
		bool flag = false;
		foreach (BaseItem subItem in container.SubItems)
		{
			if (subItem.KeyTips != "" || _0A00 != "")
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
			_0A00 += charCode.ToString().ToUpper();
		}
		return null;
	}

	protected BaseItem ItemAtLocation(int x, int y)
	{
		if (m_ParentItem != null && m_ParentItem.SubItems.Count != 0)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				if ((!subItem.Visible && !လ) || !subItem.Displayed || x < subItem.LeftInternal || x > subItem.LeftInternal + subItem.WidthInternal || y < subItem.TopInternal || y > subItem.TopInternal + subItem.HeightInternal)
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
		_1036();
		((Control)this).OnHandleDestroyed(e);
	}

	internal void _1036()
	{
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			m_HotSubItem = null;
		}
	}

	private void ఘ()
	{
		_089F.TRACKMOUSEEVENT tme = default(_089F.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf((object)tme);
		_089F.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		_089F.TrackMouseEvent(ref tme);
	}

	private void _1037()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		ဟ = new Timer();
		ဟ.Interval = 100;
		ဟ.Tick += _1038;
		ဟ.Start();
	}

	private void _1038(object _٠, EventArgs ؿ)
	{
		_103B();
	}

	private void _1039()
	{
		if (_0DCA > 0)
		{
			_0DCA--;
		}
		_0DDE();
		((Control)this).Refresh();
	}

	private void _103A()
	{
		_0DCA++;
		_0DDE();
		((Control)this).Refresh();
	}

	private bool _103B()
	{
		if (!_0DC9)
		{
			return false;
		}
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (_0DCB && point.Y <= မ + _0B59.Top)
		{
			_1039();
			if (ဟ == null)
			{
				_1037();
			}
			return true;
		}
		if (_0DCC && point.Y >= ((Control)this).Height - ယ)
		{
			_103A();
			if (ဟ == null)
			{
				_1037();
			}
			return true;
		}
		if (ဟ != null)
		{
			ဟ.Stop();
			((Component)(object)ဟ).Dispose();
			ဟ = null;
		}
		return false;
	}

	private void _0DDE()
	{
		_0DCB = false;
		_0DCC = false;
		if (!_0DC9 || m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
		{
			return;
		}
		int num = _0B59.Top;
		int num2 = 0;
		if (_0DCA > 0)
		{
			မ = 16;
			num += မ;
			_0DCB = true;
		}
		bool flag = false;
		for (int i = 0; i < m_ParentItem.SubItems.Count; i++)
		{
			BaseItem baseItem = m_ParentItem.SubItems[i];
			if (flag || i < _0DCA || (!baseItem.Visible && !လ))
			{
				baseItem.Displayed = false;
				continue;
			}
			num2 = baseItem.HeightInternal;
			if (baseItem.BeginGroup)
			{
				num2 = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num2 + Dpi.Height(3)) : (num2 + Dpi.Height(9)));
			}
			if (num2 + num > _0B59.Bottom || (_0B59.Bottom - (num2 + num) < 15 && i + 1 < m_ParentItem.SubItems.Count))
			{
				baseItem.Displayed = false;
				flag = true;
				_0DCC = true;
				ယ = _0B59.Bottom - num;
				continue;
			}
			if (i == m_ParentItem.SubItems.Count - 1)
			{
				baseItem.Displayed = true;
				num = _0B59.Bottom;
				for (int num3 = i; num3 >= _0DCA; num3--)
				{
					baseItem = m_ParentItem.SubItems[num3];
					if (baseItem.Visible || လ)
					{
						num = (baseItem.TopInternal = num - baseItem.HeightInternal);
						if (baseItem.BeginGroup)
						{
							num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num - Dpi.Height(3)) : (num - Dpi.Height(9)));
						}
					}
				}
				if (_0DCA > 0)
				{
					baseItem = m_ParentItem.SubItems[_0DCA - 1];
					if (num - baseItem.HeightInternal - 15 > _0B59.Top)
					{
						for (int num5 = _0DCA - 1; num5 >= 0; num5--)
						{
							baseItem = m_ParentItem.SubItems[num5];
							if (baseItem.Visible || လ)
							{
								if (num - baseItem.HeightInternal - 15 <= _0B59.Top)
								{
									break;
								}
								num = (baseItem.TopInternal = num - baseItem.HeightInternal);
								baseItem.Displayed = true;
								if (baseItem.BeginGroup)
								{
									num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num - Dpi.Height(3)) : (num - Dpi.Height(9)));
								}
								_0DCA = num5;
							}
						}
					}
				}
				မ = num - _0B59.Top;
				_0DCC = false;
				break;
			}
			if (baseItem.BeginGroup)
			{
				num = ((m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000) ? (num + Dpi.Height(3)) : (num + Dpi.Height(9)));
			}
			baseItem.TopInternal = num;
			baseItem.Displayed = true;
			num += baseItem.HeightInternal;
		}
	}

	private void _0BDD(Control _0BDE)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Rectangle clientRectangle = _0BDE.ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(clientRectangle, ன);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(roundedRectanglePath);
		roundedRectanglePath.Widen(SystemPens.ControlText);
		val.Union(roundedRectanglePath);
		_0BDE.Region = val;
	}

	public void Hide()
	{
		((Control)this).Hide();
		if (_0BBA && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			_0BBA = false;
		}
	}

	private bool హ()
	{
		ISite site = ಌ();
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
		_0BBA = true;
		return true;
	}

	public void Show()
	{
		if (!ಔ)
		{
			((Control)this).Visible = true;
			((Control)this).Update();
		}
		else
		{
			if (m_ParentItem != null && ((m_ParentItem.Site != null && m_ParentItem.Site.DesignMode) || m_ParentItem.DesignMode || (m_ParentItem.Parent != null && m_ParentItem.Parent.Site != null && m_ParentItem.Parent.Site.DesignMode)) && హ())
			{
				return;
			}
			PopupContainer popupContainer = new PopupContainer();
			popupContainer.ShowDropShadow = ଌ && _0B0D;
			ঊ ঊ2 = BarFunctions.ॻ((Control)(object)this);
			if (ঊ2 != null)
			{
				Rectangle rectangle = ঊ2.ঌ;
				if (ထ || ဢ)
				{
					rectangle = ঊ2.ঋ;
				}
				if (((Control)this).Location.Y + ((Control)this).Size.Height > rectangle.Bottom)
				{
					_0B59.Height -= ((Control)this).Location.Y + ((Control)this).Size.Height - rectangle.Bottom;
					((Control)this).Height = ((Control)this).Height - (((Control)this).Location.Y + ((Control)this).Size.Height - rectangle.Bottom);
					_0DC9 = true;
					_0DCA = 0;
					_0DDE();
				}
			}
			((Control)popupContainer).Location = ((Control)this).Location;
			((Control)popupContainer).Controls.Add((Control)(object)this);
			((Control)popupContainer).Size = ((Control)this).Size;
			((Control)this).Location = new Point(0, 0);
			((Control)popupContainer).CreateControl();
			if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && တ)
			{
				_0BDD((Control)(object)this);
				_0BDD((Control)(object)popupContainer);
			}
			ePopupAnimation ePopupAnimation2 = ஃ;
			if (!BarFunctions.SupportsAnimation)
			{
				ePopupAnimation2 = ePopupAnimation.None;
			}
			else
			{
				if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
				{
					if (_0AC3 is IOwnerMenuSupport ownerMenuSupport)
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
					ePopupAnimation2 = _089F.ࢡ;
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
				_089F.AnimateWindow(((Control)popupContainer).Handle, 100, 524288);
				((Control)popupContainer).Visible = true;
			}
			else
			{
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.Slide:
					_089F.AnimateWindow(((Control)popupContainer).Handle, 100, 262149);
					((Control)popupContainer).Visible = true;
					break;
				case ePopupAnimation.Unfold:
					_089F.AnimateWindow(((Control)popupContainer).Handle, 100, 5);
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
				_0DCF = true;
			}
			((Control)this).Update();
			if (((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject)
			{
				if (m_ParentItem != null && m_ParentItem.IsOnMenuBar)
				{
					popupMenuAccessibleObject._၈((AccessibleEvents)4);
				}
				popupMenuAccessibleObject._၈((AccessibleEvents)6);
			}
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (!((Control)this).Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		if (!((Control)this).Visible && ((Control)this).Parent != null && ((Control)this).Parent is PopupContainer)
		{
			((Control)this).Parent.Visible = false;
		}
		if (!((Control)this).Visible && ဟ != null)
		{
			ဟ.Stop();
			((Component)(object)ဟ).Dispose();
			ဟ = null;
		}
		if (!_0B98 || !(((Control)this).AccessibilityObject is PopupMenuAccessibleObject popupMenuAccessibleObject))
		{
			return;
		}
		if (m_ParentItem != null)
		{
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				popupMenuAccessibleObject._၈(subItem, (AccessibleEvents)32769);
			}
		}
		if (!((Control)this).Visible)
		{
			popupMenuAccessibleObject._၈((AccessibleEvents)7);
			popupMenuAccessibleObject._၈((AccessibleEvents)32778);
		}
	}

	private bool _103C(BaseItem _06EC)
	{
		if (_06EC == null)
		{
			return false;
		}
		while (_06EC != null)
		{
			if (ParentItem == _06EC)
			{
				return true;
			}
			_06EC = _06EC.Parent;
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
				baseItem.DesignInsertMarker = ᥡ.ᘶ;
			}
			else if (Before)
			{
				baseItem.DesignInsertMarker = ᥡ.ᆍ;
			}
			else
			{
				baseItem.DesignInsertMarker = ᥡ.ᥢ;
			}
		}
		else
		{
			new Rectangle(((Control)this).ClientRectangle.Left + 2, ((Control)this).ClientRectangle.Top + 2, ((Control)this).ClientRectangle.Width - 4, 1);
			new Rectangle(((Control)this).ClientRectangle.Left + 1, ((Control)this).ClientRectangle.Top, 1, 5);
			new Rectangle(((Control)this).ClientRectangle.Right - 2, ((Control)this).ClientRectangle.Top, 1, 5);
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
		_1036();
		RecalcSize();
		ISite site = ಌ();
		if (site != null && m_ParentItem != null && ((Component)(object)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanged(m_ParentItem, TypeDescriptor.GetProperties(m_ParentItem)["SubItems"], null, null);
		}
	}
}
