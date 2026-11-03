using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using Microsoft.Win32;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxBitmap(typeof(Bar), "Bar.ico")]
[Designer("DevComponents.DotNetBar.Design.BarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[DefaultEvent("ItemClick")]
public class Bar : Control, IOwner, IDockInfo, IBarImageSize, IOwnerMenuSupport, IOwnerLocalize, IAccessibilitySupport, IRenderingSupport, ሎ, IBarDesignerServices, ICustomSerialization, ISupportInitialize
{
	public class BarAccessibleObject : ControlAccessibleObject
	{
		private Bar _0AC3;

		public override AccessibleRole Role
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
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
				if (_0AC3 != null && ((Control)_0AC3).Parent is DockSite)
				{
					return ((Control)_0AC3).Parent.AccessibilityObject;
				}
				return ((ControlAccessibleObject)this).Parent;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (_0AC3 != null && !((Control)_0AC3).IsDisposed && ((Control)_0AC3).Parent != null)
				{
					return ((Control)_0AC3).Parent.RectangleToScreen(((Control)_0AC3).Bounds);
				}
				return Rectangle.Empty;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				AccessibleStates val;
				if (_0AC3.GrabHandleStyle != 0 && _0AC3.GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
				{
					val = (AccessibleStates)262144;
					if (_0AC3.DockSide == eDockSide.None)
					{
						val = (AccessibleStates)(val | 0x1000);
					}
				}
				else
				{
					val = (AccessibleStates)0;
				}
				return val;
			}
		}

		public BarAccessibleObject(Bar owner)
			: base((Control)(object)owner)
		{
			_0AC3 = owner;
		}

		internal void _၈(BaseItem _٠, AccessibleEvents ؿ)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = _0AC3.Items.IndexOf(_٠);
			if (num >= 0 && _0AC3 != null && !((Control)_0AC3).IsDisposed)
			{
				((Control)_0AC3).AccessibilityNotifyClients(ؿ, num);
			}
		}

		public override int GetChildCount()
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed && _0AC3.Items != null)
			{
				return _0AC3.Items.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed && _0AC3.Items != null)
			{
				return _0AC3.Items[iIndex].AccessibleObject;
			}
			return null;
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Invalid comparison between Unknown and I4
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Invalid comparison between Unknown and I4
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Invalid comparison between Unknown and I4
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Invalid comparison between Unknown and I4
			if ((int)navdir == 7)
			{
				if (_0AC3.Items.Count > 0)
				{
					return _0AC3.Items[0].AccessibleObject;
				}
			}
			else if ((int)navdir == 8)
			{
				if (_0AC3.Items.Count > 0)
				{
					return _0AC3.Items[_0AC3.Items.Count - 1].AccessibleObject;
				}
			}
			else
			{
				if ((int)navdir == 5)
				{
					return null;
				}
				if ((int)navdir == 6)
				{
					return null;
				}
				if ((int)navdir == 2)
				{
					return null;
				}
				if ((int)navdir == 1)
				{
					return null;
				}
			}
			return ((AccessibleObject)this).Navigate(navdir);
		}
	}

	private class _1B5E
	{
		public Rectangle _1B73;

		public Rectangle _1B74;

		public Rectangle _1B75;

		public Rectangle _1B76;

		public Rectangle _1B77;

		public Size _1B78 = new Size(14, 14);

		private bool _1B79;

		private bool _1B7A;

		private bool _1B7B;

		private bool _1B7C;

		private bool _1B7D;

		private bool _1B7E;

		private bool _1B7F;

		private bool _1B80;

		private bool _1B81;

		private bool _1B82;

		public bool _1B69
		{
			get
			{
				return _1B79;
			}
			set
			{
				if (_1B79 != value)
				{
					_1B79 = value;
					if (this._1B5F != null)
					{
						this._1B5F(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6A
		{
			get
			{
				return _1B7A;
			}
			set
			{
				if (_1B7A != value)
				{
					_1B7A = value;
					if (this._1B60 != null)
					{
						this._1B60(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6B
		{
			get
			{
				return _1B81;
			}
			set
			{
				if (_1B81 != value)
				{
					_1B81 = value;
					if (this._1B67 != null)
					{
						this._1B67(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6C
		{
			get
			{
				return _1B7B;
			}
			set
			{
				if (_1B7B != value)
				{
					_1B7B = value;
					if (this._1B61 != null)
					{
						this._1B61(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6D
		{
			get
			{
				return _1B7C;
			}
			set
			{
				if (_1B7C != value)
				{
					_1B7C = value;
					if (this._1B63 != null)
					{
						this._1B63(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6E
		{
			get
			{
				return _1B7D;
			}
			set
			{
				if (_1B7D != value)
				{
					_1B7D = value;
					if (this._1B64 != null)
					{
						this._1B64(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B6F
		{
			get
			{
				return _1B82;
			}
			set
			{
				if (_1B82 != value)
				{
					_1B82 = value;
					if (this._1B68 != null)
					{
						this._1B68(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B70
		{
			get
			{
				return _1B7E;
			}
			set
			{
				if (_1B7E != value)
				{
					_1B7E = value;
					if (this._1B65 != null)
					{
						this._1B65(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B71
		{
			get
			{
				return _1B7F;
			}
			set
			{
				if (_1B7F != value)
				{
					_1B7F = value;
					if (this._1B62 != null)
					{
						this._1B62(this, new EventArgs());
					}
				}
			}
		}

		public bool _1B72
		{
			get
			{
				return _1B80;
			}
			set
			{
				if (_1B80 != value)
				{
					_1B80 = value;
					if (this._1B66 != null)
					{
						this._1B66(this, new EventArgs());
					}
				}
			}
		}

		public event EventHandler _1B5F;

		public event EventHandler _1B60;

		public event EventHandler _1B61;

		public event EventHandler _1B62;

		public event EventHandler _1B63;

		public event EventHandler _1B64;

		public event EventHandler _1B65;

		public event EventHandler _1B66;

		public event EventHandler _1B67;

		public event EventHandler _1B68;

		public _1B5E(Rectangle closerect, Rectangle custrect, bool mouseoverclose, bool mouseovercustomize, bool mousednclose, bool mousedncust)
		{
			_1B73 = closerect;
			_1B74 = custrect;
			_1B69 = mouseoverclose;
			_1B6C = mouseovercustomize;
			_1B6D = mousednclose;
			_1B70 = mousedncust;
			_1B71 = false;
			_1B72 = false;
			_1B75 = Rectangle.Empty;
			_1B78 = new Size(Dpi.Width14, Dpi.Height14);
		}
	}

	private struct ᮃ
	{
		public int _0749;

		public int _074A;

		public int ሪ;

		public int ራ;

		public bool ᮄ
		{
			get
			{
				if (_0749 == 0 && _074A == 0 && ሪ == 0)
				{
					return ራ == 0;
				}
				return false;
			}
		}
	}

	private const int ग = 33;

	private const int घ = 3;

	private const int ङ = 4;

	private const long च = 2147483648L;

	private const long छ = 67108864L;

	private const long ज = 33554432L;

	private const long झ = 8L;

	private const long ञ = 128L;

	private const long ଭ = 268435456L;

	private const int ମ = 70;

	private const int ଯ = 71;

	private const int ର = 1;

	private const int _0B31 = 2;

	private const int ଲ = 3;

	private const int ଳ = 4;

	private const int _0B34 = 5;

	private const int ଵ = 6;

	private const int ଶ = 7;

	private const int ଷ = 8;

	private const int ସ = 9;

	private const int ହ = 10;

	private const int _0B3A = 11;

	private const int _0B3B = 12;

	private const int _0B3C = 13;

	private const int ଽ = 14;

	private const int _0B3E = 15;

	private const int _0B3F = 3;

	private const int _0B40 = 7;

	private const int _0B41 = 7;

	private const int _0B42 = 17;

	private const int _0B43 = 23;

	private const int _0B44 = 20;

	private const int _0B45 = 25;

	private BarStateChangedEventHandler _0B46;

	private EventHandler _09BC;

	private EventHandler _0B47;

	private EventHandler _0B48;

	private EventHandler _09D3;

	private DotNetBarManager.DockTabChangeEventHandler _0B49;

	private EventHandler _0B4A;

	private EventHandler _0B4B;

	private DotNetBarManager.BarClosingEventHandler _0B4C;

	private DotNetBarManager.AutoHideDisplayEventHandler _0B4D;

	private EventHandler _09C1;

	private EventHandler _09BE;

	private EventHandler _09BF;

	private DotNetBarManager.PopupOpenEventHandler _09C0;

	private EventHandler _09C2;

	private EventHandler _0B4E;

	private EventHandler _0B4F;

	private DockTabClosingEventHandler _0B50;

	private DockTabClosingEventHandler _0B51;

	private SerializeItemEventHandler _0B52;

	private SerializeItemEventHandler _0B53;

	private EventHandler _0B54;

	private RenderBarEventHandler _0B55;

	private RenderBarEventHandler _0B56;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private BaseItem _0AF1;

	private Point _0B57;

	private object _0B58;

	private Rectangle _0B59;

	private SideBarImage _0B5A;

	private Rectangle _0B5B;

	private GenericItemContainer ଡ଼;

	private int ଢ଼;

	private eBarState _0B5E;

	private eGrabHandleStyle ୟ;

	private Rectangle ୠ;

	private int ୡ;

	private int _0B62;

	private object _0AC3;

	private Point _0AC2;

	private Size _0B63;

	private bool _0B64;

	private int _0B65 = -1;

	private Rectangle _୦;

	private Size _୧ = Size.Empty;

	private Size _୨ = Size.Empty;

	private int _୩;

	private DockSiteInfo _୪ = new DockSiteInfo();

	private Rectangle _୫;

	internal DockSiteInfo _୬ = new DockSiteInfo();

	private bool _୭;

	private bool _୮;

	private bool _୯;

	private bool _0B70;

	private ᆡ ୱ;

	private bool _0B72;

	private IntPtr _0A03;

	private bool _0B73 = true;

	private bool _0B74 = true;

	private bool _0B75 = true;

	private bool _0B76 = true;

	private bool _0B77;

	private bool _0B78 = true;

	private bool _0B79 = true;

	private bool _0B7A = true;

	private bool _0B7B;

	private bool _0B7C;

	private bool _0B7D;

	private bool _0B7E = true;

	private eBorderType _0B7F;

	private Color _0B80 = SystemColors.ControlDark;

	private bool _0B81;

	private _1B5E _0B82 = new _1B5E(Rectangle.Empty, Rectangle.Empty, mouseoverclose: false, mouseovercustomize: false, mousednclose: false, mousedncust: false);

	private ePopupAnimation ஃ = ePopupAnimation.ManagerControlled;

	private ഓ _0B84;

	private eBarImageSize ਉ;

	private Color அ = Color.Empty;

	private Color ஆ = Color.Empty;

	private PopupItem இ;

	private TabStrip ஈ;

	private bool உ;

	private bool ஊ = true;

	private bool _0B8B;

	private bool _0B8C;

	private bool _0B8D;

	private bool எ;

	private ColorScheme լ;

	internal bool ஏ;

	private bool _09F8;

	private ᮃ ஐ = default(ᮃ);

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7F _09F6;

	private int _0B91;

	private int ஒ;

	private Bar ஓ;

	private eTabStripAlignment ஔ = eTabStripAlignment.Bottom;

	private bool க = true;

	private Point _0B96 = Point.Empty;

	private bool _0B97;

	internal bool _0B98;

	private bool ங;

	private bool ச;

	private BaseItem ਈ;

	private int _0B9B = 100;

	private eBackgroundImagePosition ސ;

	private byte ޑ = byte.MaxValue;

	private bool _০ = true;

	private bool ஜ = true;

	protected ToolTip m_ToolTipWnd;

	private int _0B9D = -1;

	private bool ஞ;

	private bool ட = true;

	private bool _0BA0;

	private bool _৪;

	private bool _0A02;

	private int _0BA1 = -1;

	private bool _0A0C;

	private bool _0BA2 = true;

	private PopupItem ண;

	private bool த;

	private bool _0BA5 = true;

	private bool _0BA6;

	private IBarItemDesigner _09FD;

	private bool _0BA7;

	private bool ৼ;

	internal Hashtable ந = new Hashtable();

	private int ன = 3;

	private bool ਇ;

	private bool ե;

	private eBarType ப;

	private bool _0BAB = true;

	private bool _0BAC;

	private bool _0BAD;

	internal bool ம;

	private Color[] _ய;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private bool ர;

	private EventHandler ਞ;

	private int _ற;

	private bool ல;

	private Rectangle ள = Rectangle.Empty;

	private Form ழ;

	private bool _வ;

	private Rectangle _ஶ = Rectangle.Empty;

	private eDoubleClickBarBehavior _ஷ = eDoubleClickBarBehavior.FloatAndReDock;

	private bool ஸ = true;

	private bool ஹ;

	private bool _0BBA;

	private bool __0BBB = true;

	private bool _0BBC;

	private Component _0BBD;

	private CancelEventHandler _0BBE;

	private CancelEventHandler _0BBF;

	private bool __0BC0 = true;

	private int __0BC1 = 25;

	private bool _0BC2;

	private bool __0BC3 = true;

	private bool ਖ = true;

	private eDockSide __0BC4;

	private BaseItem _09D8;

	private BaseItem _09D9;

	private Hashtable _09DA = new Hashtable();

	private EventHandler _0BC5;

	private ImageList գ;

	private ImageList _09DB;

	private ImageList ড়;

	private bool __0BC6;

	private ArrayList ਏ = new ArrayList();

	private bool _২;

	private ᒺ _0A0E;

	private bool _৩;

	private bool _0BC7;

	private bool _0BC8;

	private bool _0BC9;

	private bool __0BCA = true;

	private bool _0BCB;

	private int _৬ = -1;

	private bool _৭;

	private IDesignTimeProvider _৫;

	private BaseItem ঢ়;

	private IDesignTimeProvider _0BCC;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(null)]
	[DevCoBrowsable(false)]
	public object Owner
	{
		get
		{
			if (_0AF1 is CustomizeItem)
			{
				return _0AF1.GetOwner();
			}
			if ((((Control)this).Parent != null && !(((Control)this).Parent is ᆡ) && !(((Control)this).Parent is DockSite) && !AutoHide && !_0BBA) || (((Control)this).Parent == null && _0AC3 == null))
			{
				return this;
			}
			return _0AC3;
		}
		set
		{
			_0AC3 = value;
			if (((Component)this).DesignMode && ((Component)(object)this).Site != null && ஈ != null)
			{
				ౘ(ౙ: false);
			}
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			if (ஏ)
			{
				return createParams;
			}
			if (_0B5E == eBarState.Popup && (_0AF1 == null || _0AF1.Site == null || !_0AF1.Site.DesignMode))
			{
				createParams.Style = -2046820352;
				createParams.ExStyle = 136;
			}
			createParams.Caption = "";
			return createParams;
		}
	}

	internal bool _0AF8 => _0B8B;

	internal bool ૹ
	{
		get
		{
			return ட;
		}
		set
		{
			if (ட != value && Visible)
			{
				ட = value;
				if (ட)
				{
					_089F.SendMessage(((Control)this).Handle, 11, 1, 0);
				}
				else
				{
					_089F.SendMessage(((Control)this).Handle, 11, 0, 0);
				}
			}
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Control LastFocusControl
	{
		get
		{
			if (((Control)this).Focused && _0A03 != IntPtr.Zero)
			{
				return Control.FromChildHandle(_0A03);
			}
			return null;
		}
	}

	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	[Description("Indicates the array of colors that when set are used to draw the border of the item.")]
	[DefaultValue(null)]
	internal Color[] _0AFA
	{
		get
		{
			return _ய;
		}
		set
		{
			if (_ய != value)
			{
				_ய = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool _0AFB
	{
		get
		{
			if (Style != eDotNetBarStyle.Office2003 && Style != eDotNetBarStyle.VS2005)
			{
				return BarFunctions.IsOffice2007Style(Style);
			}
			return true;
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BaseRenderer Renderer
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	private bool _0AFC => _0B56 != null;

	private bool _0AFD
	{
		get
		{
			if (Style == eDotNetBarStyle.VS2005 && LayoutType == eLayoutType.DockContainer)
			{
				return false;
			}
			return true;
		}
	}

	internal bool _0AFE
	{
		get
		{
			if (IsThemed && ଡ଼.m_BackgroundColor.IsEmpty)
			{
				return true;
			}
			return false;
		}
	}

	internal bool _0AFF
	{
		get
		{
			if (BarFunctions.IsOffice2007Style(Style) || !ThemeAware)
			{
				return false;
			}
			if (IsThemed || (_0B5E == eBarState.Floating && BarFunctions.ThemedOS && _0EEE._0EF0 && LayoutType == eLayoutType.DockContainer))
			{
				return true;
			}
			return false;
		}
	}

	internal BaseItem _0B00 => _0AF1;

	[Browsable(false)]
	public BaseItem ParentItem
	{
		get
		{
			if (_0AF1 != null)
			{
				return _0AF1;
			}
			return ଡ଼;
		}
		set
		{
			_௯();
			if (ଡ଼.SubItems.Count > 0)
			{
				ଡ଼.SubItems.Clear();
			}
			_0AF1 = value;
			if (_0AF1 == null || _0AF1.SubItems.Count == 0)
			{
				return;
			}
			ଡ଼.Style = _0AF1.Style;
			BaseItem baseItem = _0AF1.SubItems[0];
			if (baseItem != null)
			{
				_0B58 = baseItem.ContainerControl;
			}
			else
			{
				_0B58 = null;
			}
			ଡ଼.SubItems.AllowParentRemove = false;
			foreach (BaseItem subItem in _0AF1.SubItems)
			{
				subItem.ContainerControl = this;
				ଡ଼.SubItems.Add(subItem);
			}
			ଡ଼.SubItems.AllowParentRemove = true;
			if (_0AF1.Displayed)
			{
				object containerControl = _0AF1.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (BaseItem.IsHandleValid(val))
				{
					_0B57 = val.PointToScreen(new Point(_0AF1.LeftInternal, _0AF1.TopInternal));
					val = null;
				}
			}
		}
	}

	internal int _0B01
	{
		get
		{
			if (((Control)this).Parent != null && ஒ > 0)
			{
				int num = ((Control)this).Parent.Height * ஒ / 1000;
				Size size = _౮(MinimumDockSize(eOrientation.Horizontal));
				if (num < size.Height)
				{
					num = size.Height;
				}
				return num;
			}
			return 0;
		}
		set
		{
			if (((Control)this).Parent != null && value > 0)
			{
				if (value > ((Control)this).Parent.Height)
				{
					value = ((Control)this).Parent.Height;
				}
				ஒ = (int)((float)value / (float)((Control)this).Parent.Height * 1000f);
			}
			else
			{
				ஒ = 0;
			}
		}
	}

	internal int _0B02
	{
		get
		{
			if (((Control)this).Parent != null && _0B91 > 0)
			{
				return ((Control)this).Parent.Width * _0B91 / 1000;
			}
			return 0;
		}
		set
		{
			if (((Control)this).Parent != null && value > 0)
			{
				if (value > ((Control)this).Parent.Width)
				{
					value = ((Control)this).Parent.Width;
				}
				_0B91 = (int)((float)value / (float)((Control)this).Parent.Width * 1000f);
			}
			else
			{
				_0B91 = 0;
			}
		}
	}

	internal int _0B03
	{
		get
		{
			if (ଡ଼._0B03 > 0)
			{
				return ଡ଼._0B03 + _0BE3();
			}
			return 0;
		}
		set
		{
			if (ଡ଼ != null)
			{
				if (value > 0)
				{
					ଡ଼._0B03 = value - _0BE3();
				}
				else
				{
					ଡ଼._0B03 = 0;
				}
			}
		}
	}

	[DefaultValue(0)]
	[Description("Indicates docked bar caption height. Default value is 0 which means system predefined height is used.")]
	[Category("Appearance")]
	public int CaptionHeight
	{
		get
		{
			return _ற;
		}
		set
		{
			_ற = value;
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool LoadingHideFloating
	{
		get
		{
			return _0BA7;
		}
		set
		{
			_0BA7 = value;
		}
	}

	internal Bar _0B04 => ஓ;

	internal bool ଅ => _0B64;

	internal bool ଆ => _୩ != 0;

	[Browsable(false)]
	public bool IsMaximized
	{
		get
		{
			return _வ;
		}
		set
		{
			if (value != _வ)
			{
				if (!(((Control)this).Parent is ᆡ) || LayoutType != eLayoutType.DockContainer)
				{
					throw new InvalidOperationException("Bar is not dockable window or its not floating");
				}
				bool வ = _வ;
				_வ = value;
				OnIsMaximizedChanged(வ, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Specifies the bar behavior when its title is double-clicked")]
	[DefaultValue(eDoubleClickBarBehavior.FloatAndReDock)]
	public eDoubleClickBarBehavior DoubleClickBehavior
	{
		get
		{
			return _ஷ;
		}
		set
		{
			_ஷ = value;
		}
	}

	internal bool ଇ => ஸ;

	internal _0A79 _0A79
	{
		get
		{
			if (ৰ == null)
			{
				ৰ = new _0A79((Control)(object)this);
			}
			return ৰ;
		}
	}

	internal _0A7A _0A7A
	{
		get
		{
			if (ৱ == null)
			{
				ৱ = new _0A7A((Control)(object)this);
			}
			return ৱ;
		}
	}

	internal _0A7B _0A7B
	{
		get
		{
			if (_09F2 == null)
			{
				_09F2 = new _0A7B((Control)(object)this);
			}
			return _09F2;
		}
	}

	internal _0A7C _0A7C
	{
		get
		{
			if (_09F3 == null)
			{
				_09F3 = new _0A7C((Control)(object)this);
			}
			return _09F3;
		}
	}

	internal _0A7D _0A7D
	{
		get
		{
			if (_09F4 == null)
			{
				_09F4 = new _0A7D((Control)(object)this);
			}
			return _09F4;
		}
	}

	internal _0A7F _0A7F
	{
		get
		{
			if (_09F6 == null)
			{
				_09F6 = new _0A7F((Control)(object)this);
			}
			return _09F6;
		}
	}

	private int ଈ
	{
		get
		{
			int num = 0;
			if (_0AF1 != null && _0AF1.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				return 1;
			}
			return 3;
		}
	}

	private int ଉ
	{
		get
		{
			int num = 0;
			if (_0AF1 != null && _0AF1.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				return 2;
			}
			return 3;
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
			if (_0AF1 != null && _0AF1.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				if (_0B0D || !flag)
				{
					return 1;
				}
				return 3;
			}
			return 3;
		}
	}

	private int ଋ
	{
		get
		{
			bool flag = true;
			int num = 0;
			if (_0AC3 is IOwnerMenuSupport { ShowPopupShadow: false })
			{
				flag = false;
			}
			if (_0AF1 != null && _0AF1.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				if (_0B0D || !flag)
				{
					return 2;
				}
				return 4;
			}
			return 3;
		}
	}

	internal bool ଌ
	{
		get
		{
			if (ஏ)
			{
				return false;
			}
			if (_0AC3 is IOwnerMenuSupport ownerMenuSupport)
			{
				if (_0AF1 != null && _0AF1.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if (ownerMenuSupport.MenuDropShadow == eMenuDropShadow.Show)
					{
						return true;
					}
					return false;
				}
				return ownerMenuSupport.ShowPopupShadow;
			}
			if (_0AF1 != null && _0AF1.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return false;
			}
			return true;
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
			return _089F.ࢥ;
		}
	}

	internal bool _09A9
	{
		get
		{
			return _0A02;
		}
		set
		{
			if (_0A02 == value)
			{
				return;
			}
			_0A02 = value;
			if (_0A02)
			{
				ଡ଼.ௐ();
				if (_0AC3 is DotNetBarManager dotNetBarManager)
				{
					dotNetBarManager.FocusedBar = this;
				}
			}
			else
			{
				if (ଡ଼ != null)
				{
					ଡ଼.AutoExpand = false;
					ଡ଼.ඹ();
					ଡ଼.ContainerLostFocus(appLostFocus: false);
				}
				if (_0AC3 is DotNetBarManager dotNetBarManager2)
				{
					dotNetBarManager2.FocusedBar = null;
				}
			}
			if (MenuBar)
			{
				((Control)this).Refresh();
			}
		}
	}

	internal int _0B0E
	{
		get
		{
			return ଢ଼;
		}
		set
		{
			ଢ଼ = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
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

	bool ICustomSerialization.ଏ
	{
		get
		{
			bool flag = _0B52 != null;
			if (Owner != this && Owner is ICustomSerialization)
			{
				flag |= ((ICustomSerialization)Owner).HasSerializeItemHandlers;
			}
			return flag;
		}
	}

	bool ICustomSerialization.ଐ
	{
		get
		{
			bool flag = _0B53 != null;
			if (Owner != this && Owner is ICustomSerialization)
			{
				flag |= ((ICustomSerialization)Owner).HasDeserializeItemHandlers;
			}
			return flag;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string Definition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			ఠ(xmlDocument);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			ఢ(xmlDocument);
		}
	}

	private bool _0B11
	{
		get
		{
			eDockSide dockSide = DockSide;
			if (dockSide == eDockSide.None || dockSide == eDockSide.Left || dockSide == eDockSide.Right)
			{
				return true;
			}
			return false;
		}
	}

	private bool _0B12
	{
		get
		{
			eDockSide dockSide = DockSide;
			if (dockSide == eDockSide.None || dockSide == eDockSide.Top || dockSide == eDockSide.Bottom)
			{
				return true;
			}
			return false;
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether layout changes are saved for this bar")]
	public bool SaveLayoutChanges
	{
		get
		{
			return _0BA5;
		}
		set
		{
			_0BA5 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LayoutDefinition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			_0C29(xmlDocument);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			ప(xmlDocument);
		}
	}

	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Category("Appearance")]
	[Description("Specifies background image position when container is larger than image.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return ސ;
		}
		set
		{
			if (ސ != value)
			{
				ސ = value;
				((Control)this).Refresh();
			}
		}
	}

	[Description("Specifies the transparency of background image.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(byte.MaxValue)]
	[DevCoBrowsable(true)]
	public byte BackgroundImageAlpha
	{
		get
		{
			return ޑ;
		}
		set
		{
			if (ޑ != value)
			{
				ޑ = value;
				((Control)this).Refresh();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DevCoBrowsable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			if (((Control)this).Text != value)
			{
				((Control)this).Text = value;
				if (Visible && (_0B5E == eBarState.Floating || ((_0B5E == eBarState.Docked || _0B5E == eBarState.AutoHide) && (ୟ == eGrabHandleStyle.Caption || ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted))))
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
			if (((Control)this).Font != null)
			{
				_0B8D = ShouldSerializeFont();
			}
		}
	}

	protected internal bool IsThemed
	{
		get
		{
			if (_09F8 && Style != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && _0EEE._0EF0)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DefaultValue(false)]
	public virtual bool ThemeAware
	{
		get
		{
			return _09F8;
		}
		set
		{
			_09F8 = value;
			ଡ଼.ThemeAware = value;
		}
	}

	[Browsable(false)]
	public eBarState BarState => _0B5E;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items
	{
		get
		{
			if (ଡ଼ == null)
			{
				return null;
			}
			return ଡ଼.SubItems;
		}
	}

	[Browsable(false)]
	public GenericItemContainer ItemsContainer => ଡ଼;

	[Category("Behavior")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates whether the items that could not be displayed on the non-wrap Bar are displayed on popup menu or popup Bar.")]
	public bool DisplayMoreItemsOnMenu
	{
		get
		{
			return ଡ଼.MoreItemsOnMenu;
		}
		set
		{
			ଡ଼.MoreItemsOnMenu = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates the spacing in pixels between the sub-items.")]
	public int ItemSpacing
	{
		get
		{
			return ଡ଼.ItemSpacing;
		}
		set
		{
			if (ଡ଼.ItemSpacing != value)
			{
				ଡ଼.ItemSpacing = value;
				RecalcLayout();
			}
		}
	}

	[DefaultValue(1)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the top of the bar and the top of the item.")]
	public int PaddingTop
	{
		get
		{
			return ଡ଼.PaddingTop;
		}
		set
		{
			if (ଡ଼.PaddingTop != value)
			{
				ଡ଼.PaddingTop = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Description("Gets/Sets the padding in pixels. This represents the spacing between the bottom edge of the bar and the bottom of the item.")]
	[Category("Layout")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(1)]
	public int PaddingBottom
	{
		get
		{
			return ଡ଼.PaddingBottom;
		}
		set
		{
			if (ଡ଼.PaddingBottom != value)
			{
				ଡ଼.PaddingBottom = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(1)]
	[Browsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the left edge of the bar and the left position of the first item.")]
	public int PaddingLeft
	{
		get
		{
			return ଡ଼.PaddingLeft;
		}
		set
		{
			if (ଡ଼.PaddingLeft != value)
			{
				ଡ଼.PaddingLeft = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Description("Gets/Sets the padding in pixels. This represents the spacing between the right edge of the bar and the right side of the last item.")]
	[DefaultValue(1)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	public int PaddingRight
	{
		get
		{
			return ଡ଼.PaddingRight;
		}
		set
		{
			if (ଡ଼.PaddingRight != value)
			{
				ଡ଼.PaddingRight = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Bar layout type.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	public bool MenuBar
	{
		get
		{
			return _୯;
		}
		set
		{
			_୯ = value;
			_0BCD();
		}
	}

	[Browsable(true)]
	[Description("Indicates visual type of the bar. The type specified here is used to determine the appearance of the bar.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eBarType.Toolbar)]
	[Category("Appearance")]
	public eBarType BarType
	{
		get
		{
			return ப;
		}
		set
		{
			ப = value;
			if (ப == eBarType.StatusBar)
			{
				ଡ଼.ව = eContainerVerticalAlignment.Middle;
			}
			else
			{
				ଡ଼.ව = eContainerVerticalAlignment.Top;
			}
			if (BarType == eBarType.DockWindow)
			{
				ResetFont();
			}
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			լ = value;
			if (Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Description("Indicates the background color of the caption (Title bar).")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color CaptionBackColor
	{
		get
		{
			return அ;
		}
		set
		{
			if (அ != value)
			{
				அ = value;
				((Control)this).Refresh();
			}
		}
	}

	[Description("Indicates the caption (Title bar) text color.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(true)]
	public Color CaptionForeColor
	{
		get
		{
			return ஆ;
		}
		set
		{
			if (ஆ != value)
			{
				ஆ = value;
				((Control)this).Refresh();
			}
		}
	}

	[Description("Indicates whether toolbars with appropriate style appear with rounded corners.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	public bool RoundCorners
	{
		get
		{
			return _0BAB;
		}
		set
		{
			_0BAB = value;
			RecalcLayout();
		}
	}

	[Description("Specifies the visual style of the Bar.")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public eDotNetBarStyle Style
	{
		get
		{
			return ଡ଼.Style;
		}
		set
		{
			լ._0875(value);
			ଡ଼.Style = value;
			if ((ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2003 || ଡ଼.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(ଡ଼.EffectiveStyle)) && LayoutType == eLayoutType.Toolbar && GrabHandleStyle != 0 && GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
			{
				ୟ = eGrabHandleStyle.Office2003;
			}
			_0C54(ଡ଼.EffectiveStyle);
			if (AutoHide)
			{
				AutoHidePanel autoHidePanel = GetAutoHidePanel();
				if (autoHidePanel != null)
				{
					autoHidePanel.Style = value;
				}
			}
			else
			{
				((Control)this).Invalidate();
				RecalcLayout();
			}
		}
	}

	internal bool ଓ => _0B72;

	[Browsable(false)]
	public bool WrapItems => ଡ଼.WrapItems;

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are docked.")]
	public bool WrapItemsDock
	{
		get
		{
			return _୭;
		}
		set
		{
			_୭ = value;
		}
	}

	[Category("Layout")]
	[Browsable(false)]
	[Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are floating.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	public bool WrapItemsFloat
	{
		get
		{
			return _୮;
		}
		set
		{
			_୮ = value;
		}
	}

	[DefaultValue(eGrabHandleStyle.None)]
	[Description("Indicates the grab handle style of the docked Bars.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public eGrabHandleStyle GrabHandleStyle
	{
		get
		{
			return ୟ;
		}
		set
		{
			if (ୟ != value)
			{
				ୟ = value;
				_0BCD();
				if (Visible && !ஹ)
				{
					RecalcLayout();
				}
			}
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	public Rectangle GrabHandleRect => ୠ;

	private bool ଔ
	{
		get
		{
			DockContainerItem selectedDockContainerItem = SelectedDockContainerItem;
			if (selectedDockContainerItem == null || selectedDockContainerItem.CanClose == eDockContainerClose.Inherit)
			{
				return CanHide;
			}
			return selectedDockContainerItem.CanClose == eDockContainerClose.Yes;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Determines whether bar can be hidden.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool CanHide
	{
		get
		{
			return _0B7C;
		}
		set
		{
			if (_0B7C == value)
			{
				return;
			}
			_0B7C = value;
			if (_0B5E == eBarState.Floating || (_0B5E == eBarState.Docked && (ୟ == eGrabHandleStyle.Caption || ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted)))
			{
				((Control)this).Invalidate(ୠ);
			}
			_0C50();
			if (ஈ != null && BarFunctions.IsHandleValid((Control)(object)this))
			{
				ஈ.RecalcSize();
				if (((Component)this).DesignMode)
				{
					((Control)ஈ).Invalidate();
				}
			}
		}
	}

	private bool କ
	{
		get
		{
			if (__0BBB && LayoutType == eLayoutType.DockContainer)
			{
				return ((Control)this).Parent is ᆡ;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Maximize button is visible on floating dock windows.")]
	public bool CanMaximizeFloating
	{
		get
		{
			return __0BBB;
		}
		set
		{
			if (value != __0BBB)
			{
				bool oldValue = __0BBB;
				__0BBB = value;
				OnCanMaximizeFloatingChanged(oldValue, value);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eBorderType.None)]
	[Category("Appearance")]
	[Description("Indicates border style when Bar is docked.")]
	public eBorderType DockedBorderStyle
	{
		get
		{
			return _0B7F;
		}
		set
		{
			if (_0B7F != value)
			{
				_0B7F = value;
				if (_0B5E == eBarState.Docked && Visible)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Category("Docking")]
	[Description("Indicates whether floating bar is hidden when application loses focus.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool HideFloatingInactive
	{
		get
		{
			return க;
		}
		set
		{
			க = value;
		}
	}

	[Category("Docking")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether tab navigation buttons are shown for tabbed dockable bars.")]
	[DevCoBrowsable(true)]
	public bool TabNavigation
	{
		get
		{
			return _0B97;
		}
		set
		{
			if (_0B97 != value)
			{
				_0B97 = value;
				_0C50();
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates the border line color when docked border is a single line.")]
	public Color SingleLineColor
	{
		get
		{
			return _0B80;
		}
		set
		{
			if (_0B80 != value)
			{
				_0B80 = value;
				if (_0B5E == eBarState.Docked && Visible && _0B7F == eBorderType.SingleLine)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			if (_0B5E == eBarState.Floating && ୱ != null)
			{
				return ((Control)ୱ).Visible;
			}
			return ((Control)this).Visible;
		}
		set
		{
			ஸ = value;
			if (((Control)this).Visible == value)
			{
				if (!value)
				{
					((Control)this).Visible = false;
				}
			}
			else if (value)
			{
				_0C3A();
			}
			else
			{
				స();
			}
		}
	}

	[Browsable(false)]
	public int VisibleItemCount => ଡ଼.VisibleSubItems;

	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Determines whether end-user can drag and drop items on the Bar.")]
	[Category("Behavior")]
	[Browsable(false)]
	public bool AcceptDropItems
	{
		get
		{
			return _0B7E;
		}
		set
		{
			_0B7E = value;
		}
	}

	[Description("Gets or sets whether items on the Bar can be customized.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool CanCustomize
	{
		get
		{
			if (ଡ଼ != null)
			{
				return ଡ଼.CanCustomize;
			}
			return false;
		}
		set
		{
			if (!((Component)this).DesignMode && ଡ଼ != null)
			{
				ଡ଼.CanCustomize = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CustomBar
	{
		get
		{
			return _0B81;
		}
		set
		{
			_0B81 = value;
		}
	}

	[Browsable(false)]
	[Category("Design")]
	[DevCoBrowsable(true)]
	[Description("Bar name that can be used to identify Bar from code.")]
	public string Name
	{
		get
		{
			return ((Control)this).Name;
		}
		set
		{
			((Control)this).Name = value;
			if (((Component)(object)this).Site != null)
			{
				((Component)(object)this).Site.Name = value;
			}
		}
	}

	[Description("Specifies the Image size that will be used by items on this bar.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eBarImageSize.Default)]
	[Browsable(true)]
	[Category("Behavior")]
	public eBarImageSize ImageSize
	{
		get
		{
			return ਉ;
		}
		set
		{
			if (ਉ != value)
			{
				ਉ = value;
				ଡ଼.RefreshImageSize();
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Bar layout type.")]
	[DefaultValue(eLayoutType.Toolbar)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	public eLayoutType LayoutType
	{
		get
		{
			if (ଡ଼ == null)
			{
				return eLayoutType.Toolbar;
			}
			return ଡ଼.LayoutType;
		}
		set
		{
			if (ଡ଼.LayoutType != value)
			{
				ଡ଼.LayoutType = value;
				if (value == eLayoutType.DockContainer)
				{
					ResetFont();
				}
				_0BCD();
				RecalcLayout();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates that buttons will be automatically resized to match the size of the largest button on the bar.")]
	[Category("Behavior")]
	public bool EqualButtonSize
	{
		get
		{
			return ଡ଼.EqualButtonSize;
		}
		set
		{
			if (ଡ଼.EqualButtonSize != value)
			{
				ଡ଼.EqualButtonSize = value;
				RecalcLayout();
			}
		}
	}

	internal int ଖ
	{
		get
		{
			return ன;
		}
		set
		{
			ன = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether mouse over fade effect for buttons is enabled.")]
	[DefaultValue(false)]
	public bool FadeEffect
	{
		get
		{
			return ਇ;
		}
		set
		{
			ਇ = value;
		}
	}

	internal bool স
	{
		get
		{
			if (((Component)this).DesignMode || !BarFunctions.IsOffice2007Style(ଡ଼.EffectiveStyle) || (ਇ && _089F.ष()) || IsThemed || TextDrawing.UseTextRenderer)
			{
				return false;
			}
			return ਇ;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Specifies background color of the Bar.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public override Color BackColor
	{
		get
		{
			if (ଡ଼ != null)
			{
				return ଡ଼.BackColor;
			}
			return ((Control)this).BackColor;
		}
		set
		{
			if (ଡ଼ != null)
			{
				ଡ଼.BackColor = value;
			}
			else
			{
				((Control)this).BackColor = value;
			}
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates the Customize popup menu.")]
	[DefaultValue(null)]
	[Category("Behavior")]
	[Browsable(false)]
	public PopupItem CustomizeMenu
	{
		get
		{
			return இ;
		}
		set
		{
			இ = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates the auto-hide side of the parent form where bar is positioned.")]
	public eDockSide AutoHideSide
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			eDockSide result = eDockSide.None;
			if (உ)
			{
				DockStyle dockSide = _୬.DockSide;
				result = (dockSide - 1) switch
				{
					2 => eDockSide.Left, 
					3 => eDockSide.Right, 
					0 => eDockSide.Top, 
					_ => eDockSide.Bottom, 
				};
			}
			return result;
		}
	}

	[Category("Auto-Hide")]
	[Description("")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool AutoHideTabTextAlwaysVisible
	{
		get
		{
			return _0BAC;
		}
		set
		{
			_0BAC = value;
			if (AutoHide && GetAutoHidePanel() != null)
			{
				((Control)GetAutoHidePanel()).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Auto-Hide")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.")]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	public virtual bool AutoHide
	{
		get
		{
			return உ;
		}
		set
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			if (!((Component)this).DesignMode)
			{
				if (ஹ)
				{
					_0BBC = value;
				}
				else if (ஊ && LayoutType == eLayoutType.DockContainer && (((Control)this).Parent == null || (int)((Control)this).Parent.Dock != 5 || !value) && உ != value)
				{
					உ = value;
					_0C42();
					ఓ();
				}
			}
		}
	}

	[Category("Auto-Hide")]
	[Description("Gets or sets the visibility of the bar when bar is in auto-hide state.")]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool AutoHideVisible
	{
		get
		{
			return Visible;
		}
		set
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (AutoHide && Visible != value)
			{
				AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
				if (value && autoHidePanel != null && உ)
				{
					autoHidePanel._0C3A(this);
				}
				else if (!value && autoHidePanel != null && உ)
				{
					autoHidePanel.స(this);
				}
			}
		}
	}

	[Category("Auto-Hide")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether Bar can be auto hidden.")]
	public bool CanAutoHide
	{
		get
		{
			return ஊ;
		}
		set
		{
			ஊ = value;
			if (!ஊ)
			{
				_0B82._1B75 = Rectangle.Empty;
				_0B82._1B71 = false;
				_0B82._1B72 = false;
			}
			if (((Control)this).IsHandleCreated)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Component GlobalParentComponent
	{
		get
		{
			return _0BBD;
		}
		set
		{
			_0BBD = value;
		}
	}

	[Description("Specifies how long it takes to play the auto-hide animation, in milliseconds. Maximum value is 2000, 0 disables animation.")]
	[Category("Auto-Hide")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(100)]
	public virtual int AutoHideAnimationTime
	{
		get
		{
			return _0B9B;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value > 2000)
			{
				value = 2000;
			}
			_0B9B = value;
		}
	}

	private bool ଗ
	{
		get
		{
			if (ଡ଼.LayoutType == eLayoutType.DockContainer && (ୟ == eGrabHandleStyle.Caption || ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted) && இ != null)
			{
				return true;
			}
			if (ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				if (ଡ଼.HaveCustomizeItem)
				{
					return true;
				}
				return இ != null;
			}
			return false;
		}
	}

	private bool ଘ => m_ToolTipWnd != null;

	[Browsable(false)]
	[DefaultValue(true)]
	public bool DockTabVisible
	{
		get
		{
			return __0BC0;
		}
		set
		{
			__0BC0 = value;
			if (ஈ != null && !value)
			{
				((Control)ஈ).Visible = false;
			}
		}
	}

	[DefaultValue(25)]
	[Category("Appearance")]
	[Description("Indicates height of the docked bar tab strip which displays docked tabs.")]
	public int DockTabStripHeight
	{
		get
		{
			return __0BC1;
		}
		set
		{
			if (value != __0BC1)
			{
				int oldValue = __0BC1;
				__0BC1 = value;
				OnDockTabStripHeightChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public TabStrip DockTabControl => ஈ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockContainerItem SelectedDockContainerItem
	{
		get
		{
			if (ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				return null;
			}
			int selectedDockTab = SelectedDockTab;
			if (selectedDockTab >= 0)
			{
				return Items[selectedDockTab] as DockContainerItem;
			}
			if (VisibleItemCount > 0)
			{
				foreach (BaseItem item in Items)
				{
					if (item.Visible && item.Displayed)
					{
						return item as DockContainerItem;
					}
				}
			}
			return null;
		}
		set
		{
			if (ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				throw new InvalidOperationException("Bar type is not dockable window. LayoutType must be set to eLayoutType.DockContainer");
			}
			if (!Items.Contains(value))
			{
				throw new InvalidOperationException("Bar.Items collection does not contain the item");
			}
			SelectedDockTab = Items.IndexOf(value);
		}
	}

	[Category("Docking")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the tab (DockContainerItem) index for Bars with LayoutType set to eLayoutType.DockContainer.")]
	[DefaultValue(-1)]
	[Browsable(true)]
	public int SelectedDockTab
	{
		get
		{
			if (ଡ଼.LayoutType != eLayoutType.DockContainer || ஈ == null)
			{
				return -1;
			}
			if (ஈ.SelectedTab != null)
			{
				return ଡ଼.SubItems.IndexOf(ஈ.SelectedTab.AttachedItem);
			}
			return -1;
		}
		set
		{
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			if (ஈ == null || (ஈ != null && AutoHide))
			{
				if (value < Items.Count)
				{
					for (int i = 0; i < Items.Count; i++)
					{
						if (i == value && Items[i].Visible)
						{
							Items[i].Displayed = true;
						}
						else
						{
							Items[i].Displayed = false;
						}
					}
					if (ஈ == null)
					{
						if (Items[value].Displayed)
						{
							_౩(null, Items[value]);
						}
						ౚ();
					}
				}
				if (ஈ == null)
				{
					return;
				}
			}
			if (ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				throw new InvalidOperationException("SelectedDockTab property can be set only for LayoutType=eLayoutType.DockContainer.");
			}
			if (value < 0)
			{
				value = 0;
			}
			if (value >= ଡ଼.SubItems.Count)
			{
				throw new InvalidOperationException("Invalid tab index.");
			}
			BaseItem baseItem = ଡ଼.SubItems[value];
			foreach (TabItem tab in ஈ.Tabs)
			{
				if (tab.AttachedItem == baseItem)
				{
					if (ஈ.SelectedTab == tab && AutoHide)
					{
						_౬(baseItem);
					}
					ஈ.SelectedTab = tab;
					if (AutoHide)
					{
						AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
						autoHidePanel.ᆧ = baseItem as DockContainerItem;
					}
					break;
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return ((Control)this).Cursor;
		}
		set
		{
			((Control)this).Cursor = value;
		}
	}

	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates Bar background image.")]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RightToLeft = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override ContextMenu ContextMenu
	{
		get
		{
			return ((Control)this).ContextMenu;
		}
		set
		{
			((Control)this).ContextMenu = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).ImeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).ImeMode = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public int TabIndex
	{
		get
		{
			return ((Control)this).TabIndex;
		}
		set
		{
			((Control)this).TabIndex = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool CausesValidation
	{
		get
		{
			return ((Control)this).CausesValidation;
		}
		set
		{
			((Control)this).CausesValidation = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(true)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (DockSide != eDockSide.Document || (int)value == 0 || !(((Control)this).Parent is DockSite))
			{
				((Control)this).Dock = value;
			}
		}
	}

	[Browsable(false)]
	public Point Location
	{
		get
		{
			return ((Control)this).Location;
		}
		set
		{
			if (_0B5E == eBarState.Floating)
			{
				if (ୱ != null)
				{
					((Form)ୱ).Location = value;
				}
			}
			else
			{
				((Control)this).Location = value;
			}
		}
	}

	[Browsable(false)]
	public Size Size
	{
		get
		{
			return ((Control)this).Size;
		}
		set
		{
			((Control)this).Size = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool AllowDrop
	{
		get
		{
			return ((Control)this).AllowDrop;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether caption button drop-down menu is automatically created")]
	public virtual bool AutoCreateCaptionMenu
	{
		get
		{
			return _0BA2;
		}
		set
		{
			_0BA2 = value;
			if (Visible)
			{
				RecalcLayout();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether caption is automatically set to the active dock container item text.")]
	public virtual bool AutoSyncBarCaption
	{
		get
		{
			return த;
		}
		set
		{
			த = value;
			_౧();
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies whether Bar can be undocked.")]
	[Browsable(true)]
	[Category("Docking")]
	[DefaultValue(true)]
	public bool CanUndock
	{
		get
		{
			return _0B77;
		}
		set
		{
			if (_0B77 != value)
			{
				_0B77 = value;
			}
		}
	}

	[Category("Docking")]
	[Description("Specifes whether end-user can tear-off (deattach) the tabs on dockable window.")]
	[DefaultValue(true)]
	[Browsable(false)]
	[DevCoBrowsable(true)]
	public bool CanTearOffTabs
	{
		get
		{
			return _0B78;
		}
		set
		{
			if (_0B78 != value)
			{
				_0B78 = value;
			}
		}
	}

	[Description("Specifes whether end-user can reorder the tabs on dockable window.")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Docking")]
	public bool CanReorderTabs
	{
		get
		{
			return _0B79;
		}
		set
		{
			if (_0B79 != value)
			{
				_0B79 = value;
				if (ஈ != null)
				{
					ஈ.CanReorderTabs = value;
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates whether bar or DockContainerItem that is torn-off this bar can be docked as tab to another bar.")]
	[Category("Docking")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool CanDockTab
	{
		get
		{
			return _0B7A;
		}
		set
		{
			_0B7A = value;
		}
	}

	[Description("Indicates whether dock bar can be moved by dragging its caption using the mouse.")]
	[Category("Docking")]
	[DefaultValue(true)]
	public bool CanMove
	{
		get
		{
			return __0BC3;
		}
		set
		{
			__0BC3 = value;
		}
	}

	[Description("Determines whether bar can be docked to the top edge of container.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Docking")]
	[DevCoBrowsable(true)]
	public bool CanDockTop
	{
		get
		{
			return _0B75;
		}
		set
		{
			if (_0B75 != value)
			{
				_0B75 = value;
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Docking")]
	[Description("Determines whether bar can be docked to the bottom edge of container.")]
	public bool CanDockBottom
	{
		get
		{
			return _0B76;
		}
		set
		{
			if (_0B76 != value)
			{
				_0B76 = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Determines whether bar can be docked to the left edge of container.")]
	[Category("Docking")]
	public bool CanDockLeft
	{
		get
		{
			return _0B73;
		}
		set
		{
			if (_0B73 != value)
			{
				_0B73 = value;
			}
		}
	}

	[Description("Determines whether bar can be docked to the right edge of container.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Browsable(true)]
	public bool CanDockRight
	{
		get
		{
			return _0B74;
		}
		set
		{
			if (_0B74 != value)
			{
				_0B74 = value;
			}
		}
	}

	[Category("Docking")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Specifies whether Bar can be docked as document.")]
	public bool CanDockDocument
	{
		get
		{
			return _0B7B;
		}
		set
		{
			_0B7B = value;
		}
	}

	[Description("Specifies whether Bar will stretch to always fill the space in dock site.")]
	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Layout")]
	public bool Stretch
	{
		get
		{
			return _0B70;
		}
		set
		{
			if (_0B70 != value)
			{
				_0B70 = value;
				RecalcLayout();
			}
		}
	}

	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool DisabledImagesGrayScale
	{
		get
		{
			return ਖ;
		}
		set
		{
			ਖ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates the distance from the far left/top side of the docking site..")]
	[DefaultValue(0)]
	[Browsable(false)]
	public int DockOffset
	{
		get
		{
			return ୡ;
		}
		set
		{
			ୡ = value;
		}
	}

	[Category("Docking")]
	[Description("Indicates the docking line.")]
	[DefaultValue(0)]
	[Browsable(false)]
	[DevCoBrowsable(true)]
	public int DockLine
	{
		get
		{
			return _0B62;
		}
		set
		{
			_0B62 = value;
			if (_0B5E == eBarState.Docked && ((Control)this).Parent is DockSite && !ஹ)
			{
				((DockSite)(object)((Control)this).Parent)._1064((Control)(object)this);
			}
		}
	}

	[Category("Docking")]
	[Description("Gets or sets the dock tab alignment.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eTabStripAlignment.Bottom)]
	[Browsable(true)]
	public eTabStripAlignment DockTabAlignment
	{
		get
		{
			return ஔ;
		}
		set
		{
			ஔ = value;
			if (ஈ != null)
			{
				ஈ.TabAlignment = ஔ;
				RecalcLayout();
				_0C65();
			}
		}
	}

	[Category("Docking")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether selected dock tab is closed when Bar caption close button is pressed. Default value is false which indicates that whole bar will be hidden when bars close button is pressed.")]
	public bool CloseSingleTab
	{
		get
		{
			return _0B7D;
		}
		set
		{
			_0B7D = value;
		}
	}

	[Category("Docking")]
	[DefaultValue(false)]
	[Description("Indicates whether close button is displayed on each dock tab that allows closing of the tab.")]
	[Browsable(true)]
	public bool DockTabCloseButtonVisible
	{
		get
		{
			return _0BAD;
		}
		set
		{
			if (_0BAD != value)
			{
				_0BAD = value;
				if (ஈ != null)
				{
					ஈ.CloseButtonOnTabsVisible = _0BAD;
				}
			}
		}
	}

	[Category("Docking")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether tab that shows all dock containers on the bar is visible all the time.")]
	[DefaultValue(false)]
	public virtual bool AlwaysDisplayDockTab
	{
		get
		{
			return _0BA0;
		}
		set
		{
			if (_0BA0 != value)
			{
				_0BA0 = value;
				ౘ(ౙ: false);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates whether bar is locked to prevent docking below it. Applies to undockable bars only.")]
	[DefaultValue(false)]
	public bool LockDockPosition
	{
		get
		{
			return எ;
		}
		set
		{
			if (எ != value)
			{
				எ = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eOrientation.Horizontal)]
	[DevCoBrowsable(false)]
	public eOrientation DockOrientation
	{
		get
		{
			return ଡ଼.Orientation;
		}
		set
		{
			if (ଡ଼.Orientation != value)
			{
				ଡ଼.Orientation = value;
			}
			if (((Component)this).DesignMode && !(((Control)this).Parent is DockSite))
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	public bool Docked => _0B5E == eBarState.Docked;

	[Browsable(false)]
	public Control DockedSite => ((Control)this).Parent;

	internal eDockSide ଙ
	{
		get
		{
			return __0BC4;
		}
		set
		{
			__0BC4 = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates the dock side for the Bar.")]
	public eDockSide DockSide
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Invalid comparison between Unknown and I4
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			if (_0B5E != eBarState.Docked)
			{
				return eDockSide.None;
			}
			if (((Control)this).Parent == null)
			{
				return eDockSide.Top;
			}
			if ((int)((Control)this).Parent.Dock == 3)
			{
				return eDockSide.Left;
			}
			if ((int)((Control)this).Parent.Dock == 4)
			{
				return eDockSide.Right;
			}
			if ((int)((Control)this).Parent.Dock == 1)
			{
				return eDockSide.Top;
			}
			if ((int)((Control)this).Parent.Dock == 2)
			{
				return eDockSide.Bottom;
			}
			if ((int)((Control)this).Parent.Dock == 5)
			{
				return eDockSide.Document;
			}
			return eDockSide.None;
		}
		set
		{
			if (Owner == null || ஹ)
			{
				_0B9D = (int)value;
				return;
			}
			if (AutoHide)
			{
				_0C75(value);
				return;
			}
			DockSiteInfo dockSiteInfo = new DockSiteInfo();
			if (!(_0AC3 is IOwnerBarSupport ownerBarSupport))
			{
				_0B9D = (int)value;
				return;
			}
			dockSiteInfo.InsertPosition = -10;
			dockSiteInfo.DockLine = _0B62;
			dockSiteInfo.DockOffset = ୡ;
			if (!ங)
			{
				_0B01 = 0;
				_0B02 = 0;
			}
			switch (value)
			{
			case eDockSide.Left:
			{
				DockSite dockSite4 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite4 = ownerBarSupport.ToolbarLeftDockSite;
					if (dockSite4 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarLeftDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite4 = ownerBarSupport.LeftDockSite;
					if (dockSite4 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.LeftDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite4;
				break;
			}
			case eDockSide.Right:
			{
				DockSite dockSite3 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite3 = ownerBarSupport.ToolbarRightDockSite;
					if (dockSite3 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarRightDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite3 = ownerBarSupport.RightDockSite;
					if (dockSite3 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.RightDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite3;
				break;
			}
			case eDockSide.Top:
			{
				DockSite dockSite2 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite2 = ownerBarSupport.ToolbarTopDockSite;
					if (dockSite2 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarTopDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite2 = ownerBarSupport.TopDockSite;
					if (dockSite2 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.TopDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite2;
				break;
			}
			case eDockSide.Bottom:
			{
				DockSite dockSite = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite = ownerBarSupport.ToolbarBottomDockSite;
					if (dockSite == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarBottomDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite = ownerBarSupport.BottomDockSite;
					if (dockSite == null)
					{
						throw new InvalidOperationException("DotNetBarManager.BottomDockSite dock-site is not set.");
					}
				}
				if (dockSite == null)
				{
					throw new InvalidOperationException("Bottom dock-site is not set.");
				}
				dockSiteInfo.objDockSite = dockSite;
				break;
			}
			case eDockSide.Document:
				dockSiteInfo.objDockSite = ownerBarSupport.FillDockSite;
				break;
			default:
				dockSiteInfo.objDockSite = null;
				break;
			}
			DockingHandler(dockSiteInfo, _୦.Location);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates the inital floating location.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public Point InitalFloatLocation
	{
		get
		{
			return _୦.Location;
		}
		set
		{
			_୦.Location = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Run-time Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether Tooltips are shown on Bar and it's sub-items.")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool DesignerSelection
	{
		get
		{
			return ৼ;
		}
		set
		{
			if (ৼ != value)
			{
				ৼ = value;
				((Control)this).Refresh();
			}
		}
	}

	bool IOwner.জ => ((Component)this).DesignMode;

	[DefaultValue(false)]
	[DevCoBrowsable(false)]
	[Description("Indicates whether accelerator letters for menu or toolbar commands are underlined regardless of current Windows settings.")]
	[Browsable(true)]
	[Category("Run-time Behavior")]
	public bool AlwaysDisplayKeyAccelerators
	{
		get
		{
			return _0A0C;
		}
		set
		{
			if (_0A0C != value)
			{
				_0A0C = value;
				((Control)this).Refresh();
			}
		}
	}

	Form IOwner.ছ
	{
		get
		{
			return ((Control)this).FindForm();
		}
		set
		{
		}
	}

	Form IOwner.ঝ
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				return null;
			}
			if (val.IsMdiContainer)
			{
				return val.ActiveMdiChild;
			}
			return null;
		}
	}

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[DevCoBrowsable(false)]
	[Category("Data")]
	[DefaultValue(null)]
	[Browsable(true)]
	public ImageList Images
	{
		get
		{
			return գ;
		}
		set
		{
			if (գ != null)
			{
				((Component)(object)գ).Disposed -= ٶ;
			}
			գ = value;
			if (գ != null)
			{
				((Component)(object)գ).Disposed += ٶ;
			}
		}
	}

	[Category("Data")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("ImageList for medium-sized images used on Items.")]
	public ImageList ImagesMedium
	{
		get
		{
			return _09DB;
		}
		set
		{
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed -= ٶ;
			}
			_09DB = value;
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed += ٶ;
			}
		}
	}

	[Category("Data")]
	[Description("ImageList for large-sized images used on Items.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public ImageList ImagesLarge
	{
		get
		{
			return ড়;
		}
		set
		{
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed -= ٶ;
			}
			ড় = value;
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed += ٶ;
			}
		}
	}

	IBarItemDesigner IBarDesignerServices.হ
	{
		get
		{
			return _09FD;
		}
		set
		{
			_09FD = value;
		}
	}

	bool IOwner.ঞ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwner.ଚ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	BaseItem IOwner.ଛ => null;

	bool IOwner.ଜ => false;

	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	[Category("Behavior")]
	[Browsable(false)]
	[DefaultValue(false)]
	public bool UseHook
	{
		get
		{
			return __0BC6;
		}
		set
		{
			if (__0BC6 != value)
			{
				__0BC6 = value;
			}
		}
	}

	bool IOwnerMenuSupport.ঢ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ণ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ত
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.থ => true;

	eMenuDropShadow IOwnerMenuSupport.দ
	{
		get
		{
			return eMenuDropShadow.SystemDefault;
		}
		set
		{
		}
	}

	ePopupAnimation IOwnerMenuSupport.ধ
	{
		get
		{
			return ePopupAnimation.SystemDefault;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ন
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	internal bool ଝ
	{
		get
		{
			return _0BC7;
		}
		set
		{
			_0BC7 = value;
		}
	}

	internal bool ଞ
	{
		get
		{
			return _0BC8;
		}
		set
		{
			_0BC8 = value;
		}
	}

	bool ሎ.প
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return val.Modal;
			}
			return false;
		}
	}

	private bool ଟ
	{
		get
		{
			Form val = ((Control)this).FindForm();
			Form activeForm = Form.ActiveForm;
			if (val == null)
			{
				return false;
			}
			if (val.IsMdiChild && val.MdiParent != null)
			{
				if (val.MdiParent.ActiveMdiChild == val && (val == activeForm || val.MdiParent == activeForm))
				{
					return true;
				}
				return false;
			}
			return val == Form.ActiveForm;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether mnemonic keys, accelerator keys, which are set through item Text property used ampersand character are processed by control.")]
	[Category("Behavior")]
	public bool ProcessMnemonics
	{
		get
		{
			return __0BCA;
		}
		set
		{
			__0BCA = value;
		}
	}

	bool IDockInfo.ଠ => CanDockTop;

	bool IDockInfo.ଡ => CanDockBottom;

	bool IDockInfo.ଢ => CanDockLeft;

	bool IDockInfo.ଣ => CanDockRight;

	bool IDockInfo.ତ => CanDockDocument;

	bool IDockInfo.ଥ
	{
		get
		{
			return Stretch;
		}
		set
		{
			Stretch = value;
		}
	}

	int IDockInfo.ଦ
	{
		get
		{
			return DockOffset;
		}
		set
		{
			DockOffset = value;
		}
	}

	int IDockInfo.ଧ
	{
		get
		{
			return DockLine;
		}
		set
		{
			DockLine = value;
		}
	}

	eOrientation IDockInfo.ନ
	{
		get
		{
			return DockOrientation;
		}
		set
		{
			DockOrientation = value;
		}
	}

	bool IDockInfo._0B29 => Docked;

	Control IDockInfo.ପ => DockedSite;

	eDockSide IDockInfo.ଫ
	{
		get
		{
			return DockSide;
		}
		set
		{
			DockSide = value;
		}
	}

	bool IDockInfo.ବ
	{
		get
		{
			return LockDockPosition;
		}
		set
		{
			LockDockPosition = value;
		}
	}

	[Description("Occurs after bar state has changed, like selected dock tab has changed, bar has closed, bar has been docked or undocked etc.")]
	public event BarStateChangedEventHandler BarStateChanged
	{
		add
		{
			BarStateChangedEventHandler barStateChangedEventHandler = _0B46;
			BarStateChangedEventHandler barStateChangedEventHandler2;
			do
			{
				barStateChangedEventHandler2 = barStateChangedEventHandler;
				BarStateChangedEventHandler value2 = (BarStateChangedEventHandler)Delegate.Combine(barStateChangedEventHandler2, value);
				barStateChangedEventHandler = Interlocked.CompareExchange(ref _0B46, value2, barStateChangedEventHandler2);
			}
			while ((object)barStateChangedEventHandler != barStateChangedEventHandler2);
		}
		remove
		{
			BarStateChangedEventHandler barStateChangedEventHandler = _0B46;
			BarStateChangedEventHandler barStateChangedEventHandler2;
			do
			{
				barStateChangedEventHandler2 = barStateChangedEventHandler;
				BarStateChangedEventHandler value2 = (BarStateChangedEventHandler)Delegate.Remove(barStateChangedEventHandler2, value);
				barStateChangedEventHandler = Interlocked.CompareExchange(ref _0B46, value2, barStateChangedEventHandler2);
			}
			while ((object)barStateChangedEventHandler != barStateChangedEventHandler2);
		}
	}

	[Description("Occurs when Item is clicked.")]
	[Category("Item")]
	public event EventHandler ItemClick
	{
		add
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after Bar is docked.")]
	public event EventHandler BarDock
	{
		add
		{
			EventHandler eventHandler = _0B47;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B47, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B47;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B47, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after Bar is undocked.")]
	public event EventHandler BarUndock
	{
		add
		{
			EventHandler eventHandler = _0B48;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B48, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B48;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B48, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after Bar definition is loaded.")]
	public event EventHandler DefinitionLoaded
	{
		add
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when current Dock tab has changed.")]
	public event DotNetBarManager.DockTabChangeEventHandler DockTabChange
	{
		add
		{
			DotNetBarManager.DockTabChangeEventHandler dockTabChangeEventHandler = _0B49;
			DotNetBarManager.DockTabChangeEventHandler dockTabChangeEventHandler2;
			do
			{
				dockTabChangeEventHandler2 = dockTabChangeEventHandler;
				DotNetBarManager.DockTabChangeEventHandler value2 = (DotNetBarManager.DockTabChangeEventHandler)Delegate.Combine(dockTabChangeEventHandler2, value);
				dockTabChangeEventHandler = Interlocked.CompareExchange(ref _0B49, value2, dockTabChangeEventHandler2);
			}
			while ((object)dockTabChangeEventHandler != dockTabChangeEventHandler2);
		}
		remove
		{
			DotNetBarManager.DockTabChangeEventHandler dockTabChangeEventHandler = _0B49;
			DotNetBarManager.DockTabChangeEventHandler dockTabChangeEventHandler2;
			do
			{
				dockTabChangeEventHandler2 = dockTabChangeEventHandler;
				DotNetBarManager.DockTabChangeEventHandler value2 = (DotNetBarManager.DockTabChangeEventHandler)Delegate.Remove(dockTabChangeEventHandler2, value);
				dockTabChangeEventHandler = Interlocked.CompareExchange(ref _0B49, value2, dockTabChangeEventHandler2);
			}
			while ((object)dockTabChangeEventHandler != dockTabChangeEventHandler2);
		}
	}

	[Description("Occurs when bar visibility has changed as a result of user action.")]
	public event EventHandler UserVisibleChanged
	{
		add
		{
			EventHandler eventHandler = _0B4A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B4A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when bar auto hide state has changed.")]
	public event EventHandler AutoHideChanged
	{
		add
		{
			EventHandler eventHandler = _0B4B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B4B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.")]
	public event DotNetBarManager.BarClosingEventHandler Closing
	{
		add
		{
			DotNetBarManager.BarClosingEventHandler barClosingEventHandler = _0B4C;
			DotNetBarManager.BarClosingEventHandler barClosingEventHandler2;
			do
			{
				barClosingEventHandler2 = barClosingEventHandler;
				DotNetBarManager.BarClosingEventHandler value2 = (DotNetBarManager.BarClosingEventHandler)Delegate.Combine(barClosingEventHandler2, value);
				barClosingEventHandler = Interlocked.CompareExchange(ref _0B4C, value2, barClosingEventHandler2);
			}
			while ((object)barClosingEventHandler != barClosingEventHandler2);
		}
		remove
		{
			DotNetBarManager.BarClosingEventHandler barClosingEventHandler = _0B4C;
			DotNetBarManager.BarClosingEventHandler barClosingEventHandler2;
			do
			{
				barClosingEventHandler2 = barClosingEventHandler;
				DotNetBarManager.BarClosingEventHandler value2 = (DotNetBarManager.BarClosingEventHandler)Delegate.Remove(barClosingEventHandler2, value);
				barClosingEventHandler = Interlocked.CompareExchange(ref _0B4C, value2, barClosingEventHandler2);
			}
			while ((object)barClosingEventHandler != barClosingEventHandler2);
		}
	}

	[Description("Occurs when Bar in auto-hide state is about to be displayed.")]
	public event DotNetBarManager.AutoHideDisplayEventHandler AutoHideDisplay
	{
		add
		{
			DotNetBarManager.AutoHideDisplayEventHandler autoHideDisplayEventHandler = _0B4D;
			DotNetBarManager.AutoHideDisplayEventHandler autoHideDisplayEventHandler2;
			do
			{
				autoHideDisplayEventHandler2 = autoHideDisplayEventHandler;
				DotNetBarManager.AutoHideDisplayEventHandler value2 = (DotNetBarManager.AutoHideDisplayEventHandler)Delegate.Combine(autoHideDisplayEventHandler2, value);
				autoHideDisplayEventHandler = Interlocked.CompareExchange(ref _0B4D, value2, autoHideDisplayEventHandler2);
			}
			while ((object)autoHideDisplayEventHandler != autoHideDisplayEventHandler2);
		}
		remove
		{
			DotNetBarManager.AutoHideDisplayEventHandler autoHideDisplayEventHandler = _0B4D;
			DotNetBarManager.AutoHideDisplayEventHandler autoHideDisplayEventHandler2;
			do
			{
				autoHideDisplayEventHandler2 = autoHideDisplayEventHandler;
				DotNetBarManager.AutoHideDisplayEventHandler value2 = (DotNetBarManager.AutoHideDisplayEventHandler)Delegate.Remove(autoHideDisplayEventHandler2, value);
				autoHideDisplayEventHandler = Interlocked.CompareExchange(ref _0B4D, value2, autoHideDisplayEventHandler2);
			}
			while ((object)autoHideDisplayEventHandler != autoHideDisplayEventHandler2);
		}
	}

	[Description("Occurs when popup item is closing.")]
	[Category("Item")]
	public event EventHandler PopupClose
	{
		add
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when popup of type container is loading.")]
	public event EventHandler PopupContainerLoad
	{
		add
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when popup of type container is unloading.")]
	public event EventHandler PopupContainerUnload
	{
		add
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when popup item is about to open.")]
	public event DotNetBarManager.PopupOpenEventHandler PopupOpen
	{
		add
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _09C0;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Combine(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
		remove
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _09C0;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Remove(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
	}

	[Description("Occurs just before popup window is shown.")]
	[Category("Item")]
	public event EventHandler PopupShowing
	{
		add
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before dock tab is displayed.")]
	public event EventHandler BeforeDockTabDisplayed
	{
		add
		{
			EventHandler eventHandler = _0B4E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B4E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when caption button is clicked on bars with grab handle style task pane.")]
	public event EventHandler CaptionButtonClick
	{
		add
		{
			EventHandler eventHandler = _0B4F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B4F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
	public event DockTabClosingEventHandler DockTabClosing
	{
		add
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B50;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Combine(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B50, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
		remove
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B50;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Remove(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B50, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
	}

	[Description("Occurs on dockable bars after DockContainerItem is closed by end-user. This action cannot be cancelled.")]
	public event DockTabClosingEventHandler DockTabClosed
	{
		add
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B51;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Combine(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B51, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
		remove
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B51;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Remove(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B51, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
	}

	public event SerializeItemEventHandler SerializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
	}

	public event SerializeItemEventHandler DeserializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
	}

	public event EventHandler TabStripStyleChanged
	{
		add
		{
			EventHandler eventHandler = _0B54;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B54, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B54;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B54, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event RenderBarEventHandler PreRender
	{
		add
		{
			RenderBarEventHandler renderBarEventHandler = _0B55;
			RenderBarEventHandler renderBarEventHandler2;
			do
			{
				renderBarEventHandler2 = renderBarEventHandler;
				RenderBarEventHandler value2 = (RenderBarEventHandler)Delegate.Combine(renderBarEventHandler2, value);
				renderBarEventHandler = Interlocked.CompareExchange(ref _0B55, value2, renderBarEventHandler2);
			}
			while ((object)renderBarEventHandler != renderBarEventHandler2);
		}
		remove
		{
			RenderBarEventHandler renderBarEventHandler = _0B55;
			RenderBarEventHandler renderBarEventHandler2;
			do
			{
				renderBarEventHandler2 = renderBarEventHandler;
				RenderBarEventHandler value2 = (RenderBarEventHandler)Delegate.Remove(renderBarEventHandler2, value);
				renderBarEventHandler = Interlocked.CompareExchange(ref _0B55, value2, renderBarEventHandler2);
			}
			while ((object)renderBarEventHandler != renderBarEventHandler2);
		}
	}

	public event RenderBarEventHandler PostRender
	{
		add
		{
			RenderBarEventHandler renderBarEventHandler = _0B56;
			RenderBarEventHandler renderBarEventHandler2;
			do
			{
				renderBarEventHandler2 = renderBarEventHandler;
				RenderBarEventHandler value2 = (RenderBarEventHandler)Delegate.Combine(renderBarEventHandler2, value);
				renderBarEventHandler = Interlocked.CompareExchange(ref _0B56, value2, renderBarEventHandler2);
			}
			while ((object)renderBarEventHandler != renderBarEventHandler2);
		}
		remove
		{
			RenderBarEventHandler renderBarEventHandler = _0B56;
			RenderBarEventHandler renderBarEventHandler2;
			do
			{
				renderBarEventHandler2 = renderBarEventHandler;
				RenderBarEventHandler value2 = (RenderBarEventHandler)Delegate.Remove(renderBarEventHandler2, value);
				renderBarEventHandler = Interlocked.CompareExchange(ref _0B56, value2, renderBarEventHandler2);
			}
			while ((object)renderBarEventHandler != renderBarEventHandler2);
		}
	}

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	public event EventHandler ItemLayoutUpdated
	{
		add
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelEventHandler BeforeAutoHideDisplayed
	{
		add
		{
			CancelEventHandler cancelEventHandler = _0BBE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0BBE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _0BBE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0BBE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event CancelEventHandler BeforeAutoHideHidden
	{
		add
		{
			CancelEventHandler cancelEventHandler = _0BBF;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0BBF, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _0BBF;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0BBF, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler FocusItemChange
	{
		add
		{
			EventHandler eventHandler = _0BC5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0BC5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0BC5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0BC5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnBarStateChanged(BarStateChangedEventArgs e)
	{
		_0B46?.Invoke(this, e);
		if (Owner is DotNetBarManager dotNetBarManager)
		{
			dotNetBarManager.InvokeOnBarStateChanged(e);
		}
	}

	public Bar()
	{
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		լ = new ColorScheme();
		_0AF1 = null;
		_0B58 = null;
		_0B59 = Rectangle.Empty;
		_0B5B = Rectangle.Empty;
		_0B5A = default(SideBarImage);
		ଡ଼ = new GenericItemContainer();
		ଡ଼.GlobalItem = false;
		ଡ଼.ContainerControl = this;
		ଡ଼.WrapItems = true;
		ଡ଼.Stretch = false;
		ଡ଼.Displayed = true;
		ଡ଼.SystemContainer = true;
		ଢ଼ = 164;
		_0B5E = eBarState.Docked;
		ୡ = 0;
		_0B62 = 0;
		ୟ = eGrabHandleStyle.None;
		ୠ = Rectangle.Empty;
		_0AC3 = null;
		_0AC2 = Point.Empty;
		_0B63 = new Size(0, 0);
		_0B64 = false;
		_୦ = Rectangle.Empty;
		_୩ = 0;
		_୫ = Rectangle.Empty;
		_୭ = false;
		_୮ = true;
		_0B70 = false;
		_୯ = false;
		((Control)this).Text = "";
		ୱ = null;
		_0B72 = false;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		TabStop = false;
		((Control)this).Font = SystemFonts.MenuFont;
		_0B73 = true;
		_0B74 = true;
		_0B75 = true;
		_0B76 = true;
		_0B77 = true;
		SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(_0894);
		((Control)this).IsAccessible = true;
		_0B82._1B66 += _0C4A;
		_0B82._1B63 += _0C4A;
		_0B82._1B65 += _0C4A;
		_0B82._1B62 += _0C47;
		_0B82._1B61 += _0C48;
		_0B82._1B5F += _0C49;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (ଡ଼ != null && !((Control)this).IsDisposed)
		{
			if (Style == eDotNetBarStyle.StyleManagerControlled)
			{
				Style = eDotNetBarStyle.StyleManagerControlled;
			}
			((Control)this).Invalidate();
		}
	}

	private void _0894(object _٠, UserPreferenceChangedEventArgs ؿ)
	{
		if (!_0B8C && !_0B8D)
		{
			_0B8C = true;
			_089F.PostMessage(((Control)this).Handle, 1126, IntPtr.Zero, IntPtr.Zero);
		}
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		if (ଡ଼ != null)
		{
			ଡ଼.NotifyScaleItem(factor);
		}
		_0B82._1B78 = Dpi.Size(_0B82._1B78, factor);
		((Control)this).ScaleControl(factor, specified);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new BarAccessibleObject(this);
	}

	private void _0BCD()
	{
		if (((Control)this).Text != "")
		{
			((Control)this).AccessibleName = ((Control)this).Text;
		}
		else
		{
			((Control)this).AccessibleName = "DotNetBar Bar";
		}
		((Control)this).AccessibleDescription = ((Control)this).AccessibleName + " (" + Name + ")";
		if (MenuBar)
		{
			((Control)this).AccessibleRole = (AccessibleRole)2;
		}
		else if (LayoutType == eLayoutType.DockContainer)
		{
			((Control)this).AccessibleRole = (AccessibleRole)20;
		}
		else if (GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
		{
			((Control)this).AccessibleRole = (AccessibleRole)23;
		}
		else
		{
			((Control)this).AccessibleRole = (AccessibleRole)22;
		}
	}

	public Bar(string BarCaption)
		: this()
	{
		((Control)this).Text = BarCaption;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		StyleManager.Unregister((Control)(object)this);
		ம = true;
		if (ச)
		{
			DotNetBarManager.UnRegisterOwnerParentMsgHandler(this, null);
			ச = false;
		}
		if (ୱ != null && disposing)
		{
			try
			{
				if (((Control)ୱ).Controls.Contains((Control)(object)this))
				{
					((Control)ୱ).Controls.Remove((Control)(object)this);
				}
				ୱ.ᆤ();
				((Component)(object)ୱ).Dispose();
				ୱ = null;
			}
			catch (Exception)
			{
			}
		}
		if (_0AC3 is DotNetBarManager && ((DotNetBarManager)_0AC3)._0E38)
		{
			((Control)this).Controls.Clear();
		}
		if (((Control)this).Parent != null)
		{
			try
			{
				((Control)this).Parent.Controls.Remove((Control)(object)this);
			}
			catch (Exception)
			{
			}
		}
		SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(_0894);
		if (ஈ != null)
		{
			((Component)(object)ஈ).Dispose();
			ஈ = null;
		}
		if (_0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
		_௯();
		_0AC3 = null;
		_0AF1 = null;
		_0B58 = null;
		if (ଡ଼ != null)
		{
			ଡ଼.Dispose();
		}
		ଡ଼ = null;
		((Control)this).Dispose(disposing);
		ம = false;
	}

	internal void _0BCE(bool _0BCF)
	{
		_0B8B = _0BCF;
		if (DockSide == eDockSide.None && LayoutType == eLayoutType.DockContainer && Owner is DotNetBarManager)
		{
			DotNetBarManager dotNetBarManager = Owner as DotNetBarManager;
			if (_0BCF)
			{
				dotNetBarManager.InternalDockContainerActivated(SelectedDockContainerItem);
			}
			else
			{
				dotNetBarManager.InternalDockContainerDeactivated(SelectedDockContainerItem);
			}
		}
		((Control)this).Refresh();
	}

	protected override void OnLeave(EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnLeave(e);
		if (_0B8B)
		{
			_0B8B = false;
			if (ୟ == eGrabHandleStyle.Caption && (_0B5E == eBarState.Docked || _0B5E == eBarState.AutoHide))
			{
				((Control)this).Invalidate();
			}
			if (_0B5E == eBarState.AutoHide)
			{
				_0C3E(_୬.DockSide).ᆳ();
			}
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnEnter(e);
		if (_0B8B)
		{
			return;
		}
		_0B8B = true;
		if (ୟ == eGrabHandleStyle.Caption && (_0B5E == eBarState.Docked || _0B5E == eBarState.AutoHide))
		{
			((Control)this).Invalidate();
		}
		if (_0B5E == eBarState.AutoHide)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (((Control)this).ClientRectangle.Contains(pt))
			{
				_0C3E(_୬.DockSide).ᆲ();
			}
		}
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 37 || (int)keyData == 39 || (int)keyData == 38 || (int)keyData == 40 || (int)keyData == 13 || (int)keyData == 13 || (int)keyData == 9 || (int)keyData == 27)
		{
			return true;
		}
		return ((Control)this).IsInputKey(keyData);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (ଡ଼ != null)
		{
			ଡ଼.UpdateBindings();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && (_0B5E == eBarState.Popup || (_0B5E == eBarState.Floating && ଡ଼.LayoutType == eLayoutType.TaskList)))
		{
			((Message)(ref m)).Result = new IntPtr(3);
			return;
		}
		if (((Message)(ref m)).Msg == 1125)
		{
			if (!_0B72 && _0B5E == eBarState.Floating)
			{
				_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 19);
			}
		}
		else if (((Message)(ref m)).Msg == 1126)
		{
			if (Name == "StatusBar")
			{
				Name = Name;
			}
			if (LayoutType != eLayoutType.DockContainer)
			{
				((Control)this).Font = SystemFonts.MenuFont;
			}
			RecalcLayout();
			_0B8C = false;
		}
		else if (((Message)(ref m)).Msg == 1131)
		{
			if (ਈ != null)
			{
				ਈ.DoAccesibleDefaultAction();
				ਈ = null;
			}
		}
		else if (((Message)(ref m)).Msg == 7)
		{
			IntPtr wParam = ((Message)(ref m)).WParam;
			Control val = Control.FromChildHandle(wParam);
			if (val != null && !(val is Bar) && !(val is MenuPanel) && !(val is TextBoxX))
			{
				Form val2 = val.FindForm();
				if (val2 == ((Control)this).FindForm())
				{
					_0A03 = ((Message)(ref m)).WParam;
				}
				else if (val2 != null)
				{
					_0A03 = ((Message)(ref m)).WParam;
				}
			}
		}
		else if (((Message)(ref m)).Msg == 8)
		{
			bool flag = false;
			Control val3 = Control.FromChildHandle(((Message)(ref m)).WParam);
			if (val3 != null)
			{
				while (val3.Parent != null)
				{
					val3 = val3.Parent;
				}
				if (val3 != this && ଡ଼ != null && !ଡ଼.IsAnyOnHandle(val3.Handle))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && ଡ଼ != null)
			{
				ଡ଼.ContainerLostFocus(appLostFocus: false);
			}
		}
		else if (((Message)(ref m)).Msg == 794)
		{
			చ();
			ఙ();
			_0EEE._0EF6();
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		GetColorScheme().Refresh(null, bSystemColorEvent: true);
		((Control)this).Refresh();
	}

	public void ReleaseFocus()
	{
		if (!((Control)this).Focused || !(_0A03 != IntPtr.Zero))
		{
			return;
		}
		Control val = Control.FromChildHandle(_0A03);
		if (val != null)
		{
			val.Select();
			if (!val.Focused)
			{
				_089F.SetFocus(_0A03);
			}
		}
		else
		{
			_089F.SetFocus(_0A03);
		}
		_0A03 = IntPtr.Zero;
		ଡ଼.AutoExpand = false;
	}

	internal void ௐ()
	{
		if (!((Control)this).Focused)
		{
			ଡ଼.ௐ();
			((Control)this).Focus();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (ଡ଼ != null && !((Control)this).IsDisposed)
		{
			if (((Control)this).BackColor == Color.Transparent)
			{
				((Control)this).OnPaintBackground(e);
			}
			Graphics graphics = e.Graphics;
			graphics.PageUnit = (GraphicsUnit)2;
			if (_089F.फ())
			{
				graphics.Clear(Color.White);
				graphics.DrawString("Your DotNetBar Trial has expired :-(", ((Control)this).Font, SystemBrushes.ControlText, 0f, 0f);
			}
			else if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				PaintOffice(graphics);
			}
			else
			{
				PaintDotNet(e);
			}
			if (_ய != null && _ய.Length > 0)
			{
				graphics.ResetClip();
				DisplayHelp.DrawRoundedRectangle(graphics, ((Control)this).ClientRectangle, _ய, 0);
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (!ஞ && ((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			if (!(((Control)this).Parent is DockSite) && !(((Control)this).Parent is ᆡ) && _0B5E == eBarState.Docked && !ஹ)
			{
				RecalcLayout();
			}
			_0C65();
		}
	}

	private void _0982(Graphics _٥)
	{
		if (((Control)this).BackgroundImage != null)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			BarFunctions._0982(_٥, clientRectangle, ((Control)this).BackgroundImage, ސ, ޑ);
		}
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (ե)
		{
			val.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		return val;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null && BarFunctions.IsOffice2007Style(Style))
		{
			ਛ = new Office2007Renderer();
		}
		return ਜ;
	}

	internal ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(_0AC3 as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.DesignerSelection = ৼ;
		itemPaintArgs.ໝ = GetRenderer();
		if (_0B5E == eBarState.Popup && _0AF1 != null && _0AF1.DesignMode)
		{
			ISite site = ಌ();
			if (site != null && site.DesignMode)
			{
				itemPaintArgs.DesignerSelection = true;
			}
		}
		return itemPaintArgs;
	}

	protected void PaintDotNet(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_103e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Expected O, but got Unknown
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Expected O, but got Unknown
		//IL_119f: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a6: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		RenderBarEventArgs renderBarEventArgs = null;
		if (ե)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		_0B82._1B74 = Rectangle.Empty;
		_0B82._1B73 = Rectangle.Empty;
		_0B82._1B75 = Rectangle.Empty;
		_0B82._1B76 = Rectangle.Empty;
		_0B82._1B77 = Rectangle.Empty;
		GetColorScheme();
		ItemPaintArgs itemPaintArgs = _0A92(graphics);
		itemPaintArgs.ClipRectangle = e.ClipRectangle;
		Pen val = ((_0B5E != 0) ? new Pen(itemPaintArgs.Colors.BarFloatingBorder) : new Pen(itemPaintArgs.Colors.BarPopupBorder, 1f));
		if (ଌ && !_0B0D && _0AF1 != null && !BarFunctions.IsOffice2007Style(_0AF1.EffectiveStyle))
		{
			_0BDC();
		}
		if (_0B5E == eBarState.Popup)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (_0AF1 != null && BarFunctions.IsOffice2007Style(_0AF1.EffectiveStyle) && GetRenderer() != null)
				{
					ToolbarRendererEventArgs toolbarRendererEventArgs = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
					toolbarRendererEventArgs.က = itemPaintArgs;
					GetRenderer().DrawPopupToolbarBackground(toolbarRendererEventArgs);
				}
				else
				{
					SolidBrush val2 = new SolidBrush(itemPaintArgs.Colors.BarPopupBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					_0982(itemPaintArgs.Graphics);
					_0BDB(graphics);
					Rectangle rectangle = ((Control)this).ClientRectangle;
					if (ଌ && !_0B0D)
					{
						rectangle = new Rectangle(0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
					}
					if (_0AF1 != null && BarFunctions.IsOffice2007Style(_0AF1.EffectiveStyle))
					{
						DisplayHelp.DrawRoundedRectangle(graphics, val, rectangle, ன);
					}
					else
					{
						_089F.र(graphics, val, rectangle);
					}
					if (ଌ && !_0B0D)
					{
						val.Dispose();
						val = new Pen(SystemColors.ControlDark, 2f);
						Point[] array = new Point[3];
						array[0].X = 2;
						array[0].Y = ((Control)this).ClientSize.Height - 1;
						array[1].X = ((Control)this).ClientSize.Width - 1;
						array[1].Y = ((Control)this).ClientSize.Height - 1;
						array[2].X = ((Control)this).ClientSize.Width - 1;
						array[2].Y = 2;
						graphics.DrawLines(val, array);
					}
					if (_0AF1 != null && !BarFunctions.IsOffice2007Style(_0AF1.EffectiveStyle) && _0AF1 is ButtonItem && _0AF1.Displayed && _0B57.Y < Location.Y)
					{
						Point start = new Point(_0B57.X - Location.X + 1, 0);
						DisplayHelp.DrawLine(end: new Point(start.X + _0AF1.WidthInternal - 5, 0), g: graphics, start: start, color: itemPaintArgs.Colors.ItemExpandedBackground, penWidth: 1);
					}
				}
			}
		}
		else if (_0B5E == eBarState.Floating)
		{
			bool flag = true;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (BarFunctions.IsOffice2007Style(Style) && GetRenderer() != null)
				{
					ToolbarRendererEventArgs toolbarRendererEventArgs2 = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
					toolbarRendererEventArgs2.က = itemPaintArgs;
					GetRenderer().DrawToolbarBackground(toolbarRendererEventArgs2);
				}
				else
				{
					smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					if (MenuBar)
					{
						DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.MenuBarBackground, itemPaintArgs.Colors.MenuBarBackground2, itemPaintArgs.Colors.MenuBarBackgroundGradientAngle);
					}
					else
					{
						DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.BarBackground, itemPaintArgs.Colors.BarBackground2, itemPaintArgs.Colors.BarBackgroundGradientAngle);
					}
					_0982(itemPaintArgs.Graphics);
					graphics.SmoothingMode = smoothingMode;
				}
			}
			Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			_0A79 obj = null;
			_༣ _070B = _༣._0F2E;
			ཉ య = ཉ.ཎ;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, Rectangle.Empty);
			if (_0AFF)
			{
				renderBarEventArgs.Bounds = new Rectangle(0, 0, ((Control)this).Width, ஐ._074A + 1);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					obj = _0A79;
					if (LayoutType == eLayoutType.DockContainer && !_0B8B)
					{
						య = ཉ.ཏ;
					}
					obj._0EE5(graphics, _070B, య, new Rectangle(0, 0, ஐ._0749, ((Control)this).Height));
					_070B = _༣._0F2F;
					obj._0EE5(graphics, _070B, య, new Rectangle(((Control)this).Width - ஐ.ሪ, 0, ஐ._0749, ((Control)this).Height));
					_070B = _༣._0F30;
					obj._0EE5(graphics, _070B, య, new Rectangle(0, ((Control)this).Height - ஐ.ራ, ((Control)this).Width, ஐ.ራ));
					if (LayoutType == eLayoutType.DockContainer && !_0B8B)
					{
						య = ཉ.ཌ;
					}
					_070B = _༣._༦;
				}
				rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ஐ._074A + 1);
				obj._0EE5(graphics, _070B, య, rectangle2);
				rectangle2.Offset(0, 1);
			}
			else if (ୟ == eGrabHandleStyle.Caption && LayoutType == eLayoutType.DockContainer && Style == eDotNetBarStyle.Office2000)
			{
				Rectangle rectangle3 = (renderBarEventArgs.Bounds = new Rectangle(3, 3, ((Control)this).Width - 6, _0BF6()));
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					ControlPaint.DrawBorder3D(graphics, rectangle2, (Border3DStyle)5, (Border3DSide)2063);
					ऽ ऽ2 = ऽ._093F | ऽ._0941 | ऽ._0943;
					if (_0B8B)
					{
						ऽ2 |= ऽ._093E;
					}
					IntPtr hdc = graphics.GetHdc();
					_089F.RECT lprc = new _089F.RECT(rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Bottom);
					try
					{
						_089F.DrawCaption(((Control)this).Handle, hdc, ref lprc, ऽ2);
					}
					finally
					{
						graphics.ReleaseHdc(hdc);
					}
				}
				flag = false;
				rectangle2 = rectangle3;
			}
			else
			{
				Rectangle rectangle4 = (renderBarEventArgs.Bounds = new Rectangle(3, 3, ((Control)this).Width - 6, _0BF6()));
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					Pen val3 = new Pen(itemPaintArgs.Colors.BarFloatingBorder, 1f);
					_089F.र(graphics, val, rectangle2);
					rectangle2.Inflate(-1, -1);
					_089F.र(graphics, val, rectangle2);
					val3.Dispose();
					val3 = new Pen(itemPaintArgs.Colors.BarFloatingBorder, 1f);
					graphics.DrawLine(val3, 1, 2, 2, 2);
					graphics.DrawLine(val3, ((Control)this).Width - 3, 2, ((Control)this).Width - 2, 2);
					graphics.DrawLine(val3, 1, ((Control)this).Height - 3, 2, ((Control)this).Height - 3);
					graphics.DrawLine(val3, ((Control)this).Width - 3, ((Control)this).Height - 3, ((Control)this).Width - 2, ((Control)this).Height - 3);
					val3.Dispose();
					val3 = null;
					if (GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
					{
						smoothingMode = graphics.SmoothingMode;
						graphics.SmoothingMode = (SmoothingMode)0;
						if (அ.IsEmpty)
						{
							if (LayoutType == eLayoutType.Toolbar || _0B8B)
							{
								DisplayHelp.FillRectangle(graphics, rectangle4, itemPaintArgs.Colors.BarCaptionBackground, itemPaintArgs.Colors.BarCaptionBackground2, itemPaintArgs.Colors.BarCaptionBackgroundGradientAngle);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle4, itemPaintArgs.Colors.BarCaptionInactiveBackground, itemPaintArgs.Colors.BarCaptionInactiveBackground2, itemPaintArgs.Colors.ߖ);
							}
						}
						else
						{
							DisplayHelp.FillRectangle(graphics, new Rectangle(3, 3, ((Control)this).Width - 6, _0BF6()), அ, Color.Empty);
						}
						graphics.SmoothingMode = smoothingMode;
					}
				}
				rectangle2 = rectangle4;
			}
			rectangle2.Inflate(-1, -1);
			if (GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
			{
				if (ଔ)
				{
					_0B82._1B73 = new Rectangle(rectangle2.Right - (_0B82._1B78.Width + 3), rectangle2.Y + (rectangle2.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CloseButton, _0B82._1B73);
					OnPreRender(renderBarEventArgs);
					_0B82._1B73 = renderBarEventArgs.Bounds;
					rectangle2.Width -= _0B82._1B73.Width + 3;
					if (!renderBarEventArgs.Cancel)
					{
						_0BD7(itemPaintArgs);
					}
				}
				if (କ)
				{
					_0B82._1B77 = new Rectangle(rectangle2.Right - (_0B82._1B78.Width + 3), rectangle2.Y + (rectangle2.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.MaximizeButton, _0B82._1B77);
					OnPreRender(renderBarEventArgs);
					_0B82._1B77 = renderBarEventArgs.Bounds;
					rectangle2.Width -= _0B82._1B77.Width + 3;
					if (!renderBarEventArgs.Cancel)
					{
						_0BD6(itemPaintArgs);
					}
				}
				if (ଗ)
				{
					_0B82._1B74 = new Rectangle(rectangle2.Right - (_0B82._1B78.Width + 1), rectangle2.Y + (rectangle2.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CustomizeButton, _0B82._1B74);
					OnPreRender(renderBarEventArgs);
					_0B82._1B74 = renderBarEventArgs.Bounds;
					rectangle2.Width -= _0B82._1B74.Width + 2;
					if (!renderBarEventArgs.Cancel)
					{
						_0BD9(itemPaintArgs);
					}
				}
				rectangle2.X += 2;
				rectangle2.Width -= 2;
				if (rectangle2.Width > 0 && flag)
				{
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CaptionText, rectangle2);
					OnPreRender(renderBarEventArgs);
					if (!renderBarEventArgs.Cancel)
					{
						Font val4 = null;
						try
						{
							val4 = new Font(((Control)this).Font, (FontStyle)1);
						}
						catch
						{
							val4 = SystemFonts.MenuFont;
						}
						eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
						if (ஆ.IsEmpty)
						{
							if (_0AFF)
							{
								if (_0B8B)
								{
									య = ཉ.ཋ;
								}
								else
								{
									య = ཉ.ཌ;
								}
								rectangle2.Y++;
							}
							TextDrawing.DrawString(graphics, ((Control)this).Text, val4, (_0B8B || LayoutType == eLayoutType.Toolbar) ? itemPaintArgs.Colors.BarCaptionText : itemPaintArgs.Colors.BarCaptionInactiveText, rectangle2, format);
						}
						else
						{
							TextDrawing.DrawString(graphics, ((Control)this).Text, val4, ஆ, rectangle2, format);
						}
						val4.Dispose();
						val4 = null;
					}
				}
			}
			else
			{
				_0BD1(itemPaintArgs);
			}
		}
		else if (_0B5E == eBarState.Docked && BarFunctions.IsOffice2007Style(Style) && GetRenderer() != null && (!IsThemed || MenuBar))
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).DisplayRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel && ((Control)this).BackColor != Color.Transparent)
			{
				ToolbarRendererEventArgs toolbarRendererEventArgs3 = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
				toolbarRendererEventArgs3.က = itemPaintArgs;
				GetRenderer().DrawToolbarBackground(toolbarRendererEventArgs3);
			}
			else
			{
				_0BD1(itemPaintArgs);
			}
		}
		else
		{
			val.Dispose();
			val = null;
			bool flag2 = true;
			if (ଡ଼.m_BackgroundColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
			{
				if (IsThemed)
				{
					Rectangle ल = new Rectangle(-Location.X, -Location.Y, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
					_0A7A obj3 = _0A7A;
					obj3._0EE5(graphics, ཤ.ཥ, ཫ.ཊ, ल);
				}
				else
				{
					smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
					OnPreRender(renderBarEventArgs);
					if (!renderBarEventArgs.Cancel)
					{
						if (MenuBar)
						{
							if (!itemPaintArgs.Colors.MenuBarBackground2.IsEmpty && itemPaintArgs.Colors.MenuBarBackground2.A > 0)
							{
								LinearGradientBrush val5 = BarFunctions.ॴ(((Control)this).ClientRectangle, itemPaintArgs.Colors.MenuBarBackground, itemPaintArgs.Colors.MenuBarBackground2, itemPaintArgs.Colors.MenuBarBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val5, ((Control)this).ClientRectangle);
								((Brush)val5).Dispose();
							}
							else if (_0AFB && ((Control)this).Parent != null && !itemPaintArgs.Colors.DockSiteBackColor2.IsEmpty && !itemPaintArgs.Colors.DockSiteBackColor.IsEmpty)
							{
								LinearGradientBrush val6 = BarFunctions.ॴ(new Rectangle(-((Control)this).Left, -((Control)this).Top, ((Control)this).Parent.Width, ((Control)this).Parent.Height), itemPaintArgs.Colors.DockSiteBackColor, itemPaintArgs.Colors.DockSiteBackColor2, 0f);
								graphics.FillRectangle((Brush)(object)val6, -((Control)this).Left, -((Control)this).Top, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
								((Brush)val6).Dispose();
							}
							else
							{
								SolidBrush val7 = new SolidBrush(itemPaintArgs.Colors.MenuBarBackground);
								try
								{
									graphics.FillRectangle((Brush)(object)val7, ((Control)this).DisplayRectangle);
								}
								finally
								{
									((IDisposable)val7)?.Dispose();
								}
							}
						}
						else if (Style == eDotNetBarStyle.VS2005 && LayoutType == eLayoutType.DockContainer && !((Control)this).BackColor.IsEmpty)
						{
							DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor, Color.Empty);
						}
						else if (_0AFD)
						{
							DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.BarBackground, itemPaintArgs.Colors.BarBackground2, itemPaintArgs.Colors.BarBackgroundGradientAngle);
						}
						else
						{
							SolidBrush val8 = new SolidBrush(itemPaintArgs.Colors.BarBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val8, ((Control)this).ClientRectangle);
							}
							finally
							{
								((IDisposable)val8)?.Dispose();
							}
						}
					}
					else
					{
						flag2 = false;
					}
					graphics.SmoothingMode = smoothingMode;
				}
			}
			else if (!ଡ଼.BackColor.IsEmpty)
			{
				smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).DisplayRectangle);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					SolidBrush val9 = new SolidBrush(ଡ଼.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val9, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				else
				{
					flag2 = false;
				}
				graphics.SmoothingMode = smoothingMode;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent.BackgroundImage != null && ((Control)this).Parent is DockSite)
			{
				Rectangle rectangle5 = new Rectangle(-Location.X, -Location.Y, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
				DockSite dockSite = ((Control)this).Parent as DockSite;
				BarFunctions._0982(graphics, rectangle5, ((Control)dockSite).BackgroundImage, dockSite.BackgroundImagePosition, dockSite.BackgroundImageAlpha);
			}
			else
			{
				_0982(itemPaintArgs.Graphics);
			}
			if (flag2)
			{
				if (_0AFB && !IsThemed && LayoutType == eLayoutType.Toolbar && !MenuBar && BarType == eBarType.Toolbar && ((Control)this).BackColor != Color.Transparent)
				{
					if (val != null)
					{
						val.Dispose();
					}
					val = new Pen(itemPaintArgs.Colors.BarDockedBorder, 1f);
					graphics.DrawLine(val, 0, ((Control)this).Height - 1, ((Control)this).Width, ((Control)this).Height - 1);
					val.Dispose();
					val = null;
				}
				else
				{
					Rectangle clientRectangle = ((Control)this).ClientRectangle;
					clientRectangle.Inflate(-2, -2);
					BarFunctions._२(graphics, _0B7F, clientRectangle, _0B80);
				}
			}
			_0BD1(itemPaintArgs);
		}
		ଡ଼.Paint(itemPaintArgs);
		if (ப == eBarType.StatusBar && GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
		{
			_0BD1(itemPaintArgs);
		}
		if (val != null)
		{
			val.Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
		if (_0AFC)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.All, ((Control)this).ClientRectangle);
			OnPostRender(renderBarEventArgs);
		}
	}

	protected virtual void OnPreRender(RenderBarEventArgs e)
	{
		if (_0B55 != null)
		{
			_0B55(this, e);
		}
	}

	protected virtual void OnPostRender(RenderBarEventArgs e)
	{
		if (_0B56 != null)
		{
			_0B56(this, e);
		}
	}

	internal void _0BD1(ItemPaintArgs _0BD2)
	{
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_1096: Unknown result type (might be due to invalid IL or missing references)
		//IL_109c: Invalid comparison between Unknown and I4
		//IL_1064: Unknown result type (might be due to invalid IL or missing references)
		//IL_106a: Invalid comparison between Unknown and I4
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152f: Expected O, but got Unknown
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Expected O, but got Unknown
		//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Expected O, but got Unknown
		//IL_1564: Unknown result type (might be due to invalid IL or missing references)
		//IL_156b: Expected O, but got Unknown
		//IL_1554: Unknown result type (might be due to invalid IL or missing references)
		//IL_155b: Expected O, but got Unknown
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Expected O, but got Unknown
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_1383: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Expected O, but got Unknown
		//IL_0a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_1618: Unknown result type (might be due to invalid IL or missing references)
		//IL_161f: Expected O, but got Unknown
		//IL_0dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc4: Expected O, but got Unknown
		//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Expected O, but got Unknown
		RenderBarEventArgs renderBarEventArgs = null;
		Graphics graphics = _0BD2.Graphics;
		if (ୟ == eGrabHandleStyle.None)
		{
			return;
		}
		Rectangle empty = Rectangle.Empty;
		if (ୟ != eGrabHandleStyle.Caption && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted && IsThemed && ୟ != eGrabHandleStyle.ResizeHandle)
		{
			_0A7A obj = _0A7A;
			ཤ _070B = ཤ.ས;
			if (ଡ଼.Orientation == eOrientation.Vertical)
			{
				_070B = ཤ.ཧ;
				empty = new Rectangle(ୠ.X + 2, ୠ.Top + 2, ୠ.Width - 4, 5);
			}
			else
			{
				empty = new Rectangle(ୠ.X + 2, ୠ.Top + 2, 5, ୠ.Height - 4);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
			OnPreRender(renderBarEventArgs);
			empty = renderBarEventArgs.Bounds;
			if (!renderBarEventArgs.Cancel)
			{
				obj._0EE5(graphics, _070B, ཫ.ཊ, empty);
			}
			return;
		}
		Color color = _0BD2.Colors.BarBackground;
		if (!ଡ଼.m_BackgroundColor.IsEmpty)
		{
			color = ଡ଼.m_BackgroundColor;
		}
		switch (ୟ)
		{
		case eGrabHandleStyle.Single:
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(ୠ.X + 2, ୠ.Top + 2, 3, ୠ.Height - 4);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			else
			{
				empty = new Rectangle(ୠ.X + 2, ୠ.Top + 2, ୠ.Width - 4, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			break;
		case eGrabHandleStyle.Double:
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(ୠ.X + 1, ୠ.Top + 1, 3, ୠ.Height - 2);
				BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				empty.Offset(3, 0);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			else
			{
				empty = new Rectangle(ୠ.X + 1, ୠ.Top + 1, ୠ.Width - 2, 3);
				BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				empty.Offset(0, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			break;
		case eGrabHandleStyle.DoubleThin:
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(ୠ.X + 1, ୠ.Top + 2, 3, ୠ.Height - 4);
				BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)5, color);
				empty.Offset(3, 0);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)5, color);
				}
			}
			else
			{
				empty = new Rectangle(ୠ.X + 2, ୠ.Top + 1, ୠ.Width - 4, 2);
				BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)10, color);
				empty.Offset(0, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)10, color);
				}
			}
			break;
		case eGrabHandleStyle.DoubleFlat:
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				Pen val7 = new Pen(ControlPaint.Dark(color), 1f);
				if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
				{
					graphics.DrawLine(val7, ୠ.X + 2, ୠ.Top + 2, ୠ.X + 2, ୠ.Height - 4);
					graphics.DrawLine(val7, ୠ.X + 5, ୠ.Top + 2, ୠ.X + 5, ୠ.Height - 4);
				}
				else
				{
					graphics.DrawLine(val7, ୠ.X + 2, ୠ.Top + 2, ୠ.Width - 4, ୠ.Y + 2);
					graphics.DrawLine(val7, ୠ.X + 2, ୠ.Top + 5, ୠ.Width - 4, ୠ.Y + 5);
				}
				val7.Dispose();
				val7 = null;
			}
			break;
		case eGrabHandleStyle.Dotted:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			Brush val8 = (Brush)new SolidBrush(ControlPaint.Dark(color));
			Brush val9 = (Brush)new SolidBrush(ControlPaint.Light(color));
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				for (int n = 0; n < ୠ.Height - 4; n += 4)
				{
					graphics.FillRectangle(val8, ୠ.X + 1, ୠ.Top + 2 + n, 2, 2);
					graphics.FillRectangle(val9, ୠ.X + 1, ୠ.Top + 2 + n, 1, 1);
					graphics.FillRectangle(val8, ୠ.X + 5, ୠ.Top + 2 + n, 2, 2);
					graphics.FillRectangle(val9, ୠ.X + 5, ୠ.Top + 2 + n, 1, 1);
				}
			}
			else
			{
				for (int num5 = 0; num5 < ୠ.Width - 4; num5 += 4)
				{
					graphics.FillRectangle(SystemBrushes.ControlDark, ୠ.Left + 2 + num5, ୠ.Y + 1, 2, 2);
					graphics.FillRectangle(SystemBrushes.ControlLight, ୠ.Left + 2 + num5, ୠ.Y + 1, 1, 1);
					graphics.FillRectangle(SystemBrushes.ControlDark, ୠ.Left + 2 + num5, ୠ.Y + 5, 2, 2);
					graphics.FillRectangle(SystemBrushes.ControlLight, ୠ.Left + 2 + num5, ୠ.Y + 5, 1, 1);
				}
			}
			val8.Dispose();
			val9.Dispose();
			break;
		}
		case eGrabHandleStyle.Stripe:
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				for (int k = 0; k < ୠ.Height - 4; k += 3)
				{
					BarFunctions._८(graphics, ୠ.X + 2, ୠ.Top + k + 2, 5, 2, (Border3DStyle)4, (Border3DSide)10, color);
				}
			}
			else
			{
				for (int l = 0; l < ୠ.Width - 4; l += 3)
				{
					BarFunctions._८(graphics, ୠ.Left + l + 2, ୠ.Y + 2, 2, 5, (Border3DStyle)4, (Border3DSide)5, color);
				}
			}
			break;
		case eGrabHandleStyle.StripeFlat:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			Pen val16 = new Pen(_0BD2.Colors.BarStripeColor, 1f);
			if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
			{
				for (int num7 = 2; num7 < ୠ.Height - 6; num7 += 2)
				{
					graphics.DrawLine(val16, ୠ.X + 3, ୠ.Top + num7 + 3, ୠ.X + 5, ୠ.Top + num7 + 3);
				}
			}
			else
			{
				for (int num8 = 1; num8 < ୠ.Width - 6; num8 += 2)
				{
					graphics.DrawLine(val16, ୠ.Left + num8 + 3, ୠ.Y + 2, ୠ.Left + num8 + 3, ୠ.Y + 5);
				}
			}
			val16.Dispose();
			break;
		}
		case eGrabHandleStyle.CaptionTaskPane:
		case eGrabHandleStyle.CaptionDotted:
		{
			Rectangle _0983 = ୠ;
			Brush val4 = null;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, ୠ);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (_0B5E == eBarState.Floating)
				{
					DisplayHelp.FillRectangle(graphics, _0983, _0BD2.Colors.BarCaptionBackground, _0BD2.Colors.BarCaptionBackground2, _0BD2.Colors.BarCaptionBackgroundGradientAngle);
				}
				else
				{
					if (!CaptionBackColor.IsEmpty)
					{
						val4 = (Brush)new SolidBrush(CaptionBackColor);
					}
					val4 = (Brush)((!_0BD2.Colors.BarBackground2.IsEmpty) ? ((object)BarFunctions.ॴ(_0983, _0BD2.Colors.BarBackground, _0BD2.Colors.BarBackground2, _0BD2.Colors.BarBackgroundGradientAngle)) : ((object)((!_0B8B || ୟ != eGrabHandleStyle.CaptionDotted) ? new SolidBrush(_0BD2.Colors.BarBackground) : new SolidBrush(_0BD2.Colors.BarCaptionBackground))));
					graphics.FillRectangle(val4, _0983);
					val4.Dispose();
					val4 = null;
				}
				Brush val5 = null;
				Brush val6 = (Brush)new SolidBrush(ControlPaint.Light(color));
				if (!_0BD2.Colors.BarStripeColor.IsEmpty)
				{
					color = _0BD2.Colors.BarStripeColor;
					val5 = (Brush)new SolidBrush(color);
				}
				else
				{
					val5 = (Brush)new SolidBrush(ControlPaint.Dark(color));
				}
				int num3 = ୠ.Top + 4;
				int num4 = ୠ.Left + 4;
				for (int m = num3; m <= ୠ.Bottom - 4; m += 5)
				{
					graphics.FillRectangle(val6, num4 + 1, m + 1, 2, 2);
					graphics.FillRectangle(val5, num4, m, 2, 2);
				}
				if (val6 != null)
				{
					val6.Dispose();
				}
				if (val5 != null)
				{
					val5.Dispose();
				}
			}
			_0983.X += 8;
			_0983.Width -= 8;
			_0983.Inflate(-1, -1);
			_0BD5(_0BD2, ref _0983);
			if (ୟ == eGrabHandleStyle.CaptionDotted)
			{
				_0BD3(_0BD2, _0983);
			}
			break;
		}
		case eGrabHandleStyle.Caption:
		{
			Rectangle _09832 = ୠ;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, ୠ);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				_0A79 obj2 = null;
				_༣ _༦ = _༣._༦;
				ཉ య = ཉ.ཌ;
				if (_0AFF)
				{
					obj2 = _0A79;
				}
				if (_0B7F == eBorderType.None)
				{
					_09832.Inflate(-1, 0);
				}
				if (அ.IsEmpty)
				{
					if (Style == eDotNetBarStyle.Office2000)
					{
						if (_0B8B)
						{
							graphics.FillRectangle(SystemBrushes.ActiveCaption, _09832);
						}
						else
						{
							graphics.FillRectangle(SystemBrushes.InactiveCaption, _09832);
						}
					}
					else if (_0AFF)
					{
						if (_0B8B)
						{
							య = ཉ.ཋ;
						}
						obj2._0EE5(graphics, _༦, య, _09832);
					}
					else
					{
						Color color2;
						Color color3;
						if (_0B8B)
						{
							color2 = Color.FromArgb(128, _0BD2.Colors.BarCaptionText);
							color3 = _0BD2.Colors.BarCaptionBackground;
							if (_0BD2.Colors.BarCaptionBackground2.IsEmpty)
							{
								DisplayHelp.FillRectangle(graphics, _09832, _0BD2.Colors.BarCaptionBackground);
							}
							else
							{
								LinearGradientBrush val10 = BarFunctions.ॴ(_09832, _0BD2.Colors.BarCaptionBackground, _0BD2.Colors.BarCaptionBackground2, _0BD2.Colors.BarCaptionBackgroundGradientAngle);
								try
								{
									graphics.FillRectangle((Brush)(object)val10, _09832);
								}
								finally
								{
									((IDisposable)val10)?.Dispose();
								}
							}
						}
						else
						{
							color2 = Color.FromArgb(192, _0BD2.Colors.BarCaptionInactiveText);
							color3 = _0BD2.Colors.BarCaptionInactiveBackground;
							if (_0BD2.Colors.BarCaptionInactiveBackground2.IsEmpty)
							{
								DisplayHelp.FillRectangle(graphics, _09832, _0BD2.Colors.BarCaptionInactiveBackground);
							}
							else
							{
								LinearGradientBrush val11 = BarFunctions.ॴ(_09832, _0BD2.Colors.BarCaptionInactiveBackground, _0BD2.Colors.BarCaptionInactiveBackground2, _0BD2.Colors.ߖ);
								try
								{
									graphics.FillRectangle((Brush)(object)val11, _09832);
								}
								finally
								{
									((IDisposable)val11)?.Dispose();
								}
							}
							eDotNetBarStyle effectiveStyle = ଡ଼.EffectiveStyle;
							if (effectiveStyle != eDotNetBarStyle.VS2005 && effectiveStyle != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(effectiveStyle))
							{
								Pen val12 = new Pen(_0BD2.Colors.BarCaptionBackground, 1f);
								graphics.DrawLine(val12, _09832.X + 1, _09832.Y, _09832.Right - 2, _09832.Y);
								graphics.DrawLine(val12, _09832.X + 1, _09832.Bottom - 1, _09832.Right - 2, _09832.Bottom - 1);
								graphics.DrawLine(val12, _09832.X, _09832.Y + 1, _09832.X, _09832.Bottom - 2);
								graphics.DrawLine(val12, _09832.Right - 1, _09832.Y + 1, _09832.Right - 1, _09832.Bottom - 2);
								val12.Dispose();
							}
						}
						if (StyleManager.IsVisualStudio2012(StyleManager.Style))
						{
							Rectangle rectangle2 = new Rectangle(_09832.X, _09832.Y + (_09832.Height - 6) / 2, _09832.Width, 6);
							if (!string.IsNullOrEmpty(((Control)this).Text))
							{
								Size size = TextDrawing.MeasureString(graphics, ((Control)this).Text, ((Control)this).Font);
								size.Width += 8;
								if (size.Width < rectangle2.Width)
								{
									rectangle2.Width -= size.Width;
									rectangle2.X += size.Width;
									if (ଔ)
									{
										rectangle2.Width -= 16;
									}
									if (_0BD4())
									{
										rectangle2.Width -= 16;
									}
									if (ଗ)
									{
										rectangle2.Width -= 16;
									}
									if (rectangle2.Width <= 0)
									{
										rectangle2 = Rectangle.Empty;
									}
								}
								else
								{
									rectangle2 = Rectangle.Empty;
								}
							}
							if (!rectangle2.IsEmpty)
							{
								Region clip2 = graphics.Clip;
								graphics.SetClip(new Rectangle(rectangle2.X + 2, rectangle2.Y, rectangle2.Width - 2, rectangle2.Height), (CombineMode)1);
								HatchBrush val13 = new HatchBrush((HatchStyle)8, color2, color3);
								try
								{
									graphics.FillRectangle((Brush)(object)val13, rectangle2);
								}
								finally
								{
									((IDisposable)val13)?.Dispose();
								}
								graphics.Clip = clip2;
								if (clip2 != null)
								{
									clip2.Dispose();
								}
							}
						}
					}
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, _09832, அ);
				}
			}
			_09832.Inflate(-1, -1);
			_0BD5(_0BD2, ref _09832);
			_0BD3(_0BD2, _09832);
			break;
		}
		case eGrabHandleStyle.ResizeHandle:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.ResizeHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel || StyleManager.IsMetro(ଡ଼.EffectiveStyle))
			{
				break;
			}
			Form val14 = ((Control)this).FindForm();
			if (val14 == null || (int)val14.WindowState != 2)
			{
				int num6 = 1;
				Point point = new Point(((Control)this).ClientRectangle.Right, ((Control)this).ClientRectangle.Bottom);
				if ((int)((Control)this).RightToLeft == 1)
				{
					num6 = -1;
					point = new Point(0, ((Control)this).ClientRectangle.Bottom - 2);
				}
				ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(color);
				Color color4 = ColorFunctions.HLSToRGB(hLSColor.Hue, hLSColor.Lightness + 0.3, hLSColor.Saturation);
				Pen val15 = new Pen(color4, 1f);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 2, point.X - 3 * num6, point.Y - 2);
				graphics.DrawLine(val15, point.X - 6 * num6, point.Y - 2, point.X - 7 * num6, point.Y - 2);
				graphics.DrawLine(val15, point.X - 10 * num6, point.Y - 2, point.X - 11 * num6, point.Y - 2);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 2, point.X - 2 * num6, point.Y - 3);
				graphics.DrawLine(val15, point.X - 6 * num6, point.Y - 2, point.X - 6 * num6, point.Y - 3);
				graphics.DrawLine(val15, point.X - 10 * num6, point.Y - 2, point.X - 10 * num6, point.Y - 3);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 6, point.X - 3 * num6, point.Y - 6);
				graphics.DrawLine(val15, point.X - 6 * num6, point.Y - 6, point.X - 7 * num6, point.Y - 6);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 6, point.X - 2 * num6, point.Y - 7);
				graphics.DrawLine(val15, point.X - 6 * num6, point.Y - 6, point.X - 6 * num6, point.Y - 7);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 10, point.X - 3 * num6, point.Y - 10);
				graphics.DrawLine(val15, point.X - 2 * num6, point.Y - 10, point.X - 2 * num6, point.Y - 11);
				val15.Dispose();
				if (!StyleManager.IsMetro(ଡ଼.EffectiveStyle))
				{
					val15 = new Pen(_0BD2.Colors.BarStripeColor, 1f);
					graphics.DrawRectangle(val15, point.X - 4 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val15, point.X - 8 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val15, point.X - 12 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val15, point.X - 4 * num6, point.Y - 8, 1, 1);
					graphics.DrawRectangle(val15, point.X - 8 * num6, point.Y - 8, 1, 1);
					graphics.DrawRectangle(val15, point.X - 4 * num6, point.Y - 12, 1, 1);
					val15.Dispose();
					val15 = null;
				}
			}
			break;
		}
		case eGrabHandleStyle.Office2003:
		case eGrabHandleStyle.Office2003SingleDot:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, ୠ);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			int num = ୠ.Left + (ୠ.Width - 3) / 2;
			int num2 = ୠ.Top + 4;
			if (ୟ == eGrabHandleStyle.Office2003SingleDot)
			{
				num = ୠ.Left + (ୠ.Width - 3) / 2;
				num2 = ୠ.Top + (ୠ.Height - 3) / 2;
			}
			else if (ଡ଼.Orientation == eOrientation.Vertical)
			{
				num = ୠ.Left + 4;
				num2 = ୠ.Top + (ୠ.Height - 3) / 2;
			}
			Brush val = null;
			Brush val2 = (Brush)new SolidBrush(ControlPaint.Light(color));
			if (!_0BD2.Colors.BarStripeColor.IsEmpty)
			{
				color = _0BD2.Colors.BarStripeColor;
				val = (Brush)new SolidBrush(color);
			}
			else
			{
				val = (Brush)new SolidBrush(ControlPaint.Dark(color));
			}
			if (ୟ == eGrabHandleStyle.Office2003SingleDot)
			{
				graphics.FillRectangle(val2, num + 1, num2 + 1, 2, 2);
				graphics.FillRectangle(val, num, num2, 2, 2);
			}
			else if (StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				Rectangle rectangle = new Rectangle(ୠ.X, ୠ.Y + 2, 6, ୠ.Height - 4);
				Region clip = graphics.Clip;
				graphics.SetClip(new Rectangle(rectangle.X, rectangle.Y + 1, rectangle.Width, rectangle.Height - 2), (CombineMode)1);
				HatchBrush val3 = new HatchBrush((HatchStyle)8, color, _0BD2.Colors.BarBackground);
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
			else if (ଡ଼.Orientation == eOrientation.Vertical)
			{
				for (int i = num; i <= ୠ.Right - 4; i += 5)
				{
					graphics.FillRectangle(val2, i + 1, num2 + 1, 2, 2);
					graphics.FillRectangle(val, i, num2, 2, 2);
				}
			}
			else
			{
				for (int j = num2; j <= ୠ.Bottom - 4; j += 5)
				{
					graphics.FillRectangle(val2, num + 1, j + 1, 2, 2);
					graphics.FillRectangle(val, num, j, 2, 2);
				}
			}
			val.Dispose();
			val2.Dispose();
			break;
		}
		}
	}

	private void _0BD3(ItemPaintArgs _0BD2, Rectangle _0983)
	{
		Graphics graphics = _0BD2.Graphics;
		RenderBarEventArgs renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CaptionText, _0983);
		OnPreRender(renderBarEventArgs);
		if (renderBarEventArgs.Cancel)
		{
			return;
		}
		Font val = null;
		bool flag = false;
		try
		{
			object obj = ((Control)this).Font.Clone();
			val = (Font)((obj is Font) ? obj : null);
			flag = true;
		}
		catch
		{
			val = SystemFonts.MenuFont;
			flag = true;
		}
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (ஆ.IsEmpty)
		{
			if (IsThemed)
			{
				if (_0983.Height < val.Height)
				{
					_0983.Height = val.Height;
				}
				if (_0983.Width > 0)
				{
					if (!_0B8B)
					{
						TextDrawing.DrawString(graphics, ((Control)this).Text, val, SystemColors.WindowText, _0983, format);
					}
					else
					{
						TextDrawing.DrawString(graphics, ((Control)this).Text, val, SystemColors.ActiveCaptionText, _0983, format);
					}
				}
			}
			else
			{
				if (_0983.Height < val.Height)
				{
					_0983.Height = val.Height;
				}
				if (_0983.Width > 0)
				{
					Color color = _0BD2.Colors.BarCaptionText;
					if (ୟ == eGrabHandleStyle.CaptionDotted)
					{
						color = _0BD2.Colors.ItemText;
					}
					else if ((Style == eDotNetBarStyle.OfficeXP || Style == eDotNetBarStyle.Office2003 || Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(Style)) && !_0B8B)
					{
						color = _0BD2.Colors.BarCaptionInactiveText;
					}
					TextDrawing.DrawString(graphics, ((Control)this).Text, val, color, _0983, format);
				}
			}
		}
		else if (_0983.Width > 0)
		{
			TextDrawing.DrawString(graphics, ((Control)this).Text, val, ஆ, _0983, format);
		}
		if (flag)
		{
			val.Dispose();
		}
	}

	private bool _0BD4()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		bool result = false;
		if (ஊ && ଡ଼.LayoutType == eLayoutType.DockContainer && _0B5E != eBarState.Floating)
		{
			result = ((!(((Control)this).Parent is DockSite) || (int)((Control)this).Parent.Dock != 5) ? true : false);
		}
		return result;
	}

	private void _0BD5(ItemPaintArgs _0BD2, ref Rectangle _0983)
	{
		RenderBarEventArgs renderBarEventArgs = null;
		if (ଔ)
		{
			if (Style == eDotNetBarStyle.Office2000)
			{
				_0B82._1B73 = new Rectangle(_0983.Right - 11, _0983.Y + (_0983.Height - _0B82._1B78.Width) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
			}
			else
			{
				_0B82._1B73 = new Rectangle(_0983.Right - (_0B82._1B78.Width + 1), _0983.Y + (_0983.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, _0BD2.Graphics, eBarRenderPart.CloseButton, _0B82._1B73);
			OnPreRender(renderBarEventArgs);
			_0B82._1B73 = renderBarEventArgs.Bounds;
			_0983.Width -= _0B82._1B73.Width + 3;
			if (!renderBarEventArgs.Cancel)
			{
				_0BD7(_0BD2);
			}
		}
		if (_0BD4())
		{
			if (Style == eDotNetBarStyle.Office2000)
			{
				_0B82._1B75 = new Rectangle(_0983.Right - (_0B82._1B78.Width - 1), _0983.Y + (_0983.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
			}
			else
			{
				_0B82._1B75 = new Rectangle(_0983.Right - (_0B82._1B78.Width + 1), _0983.Y + (_0983.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, _0BD2.Graphics, eBarRenderPart.AutoHideButton, _0B82._1B75);
			OnPreRender(renderBarEventArgs);
			_0B82._1B75 = renderBarEventArgs.Bounds;
			_0983.Width -= _0B82._1B75.Width + 3;
			if (!renderBarEventArgs.Cancel)
			{
				_0BDA(_0BD2);
			}
		}
		if (ଗ)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, _0BD2.Graphics, eBarRenderPart.CustomizeButton, Rectangle.Empty);
			if (Style == eDotNetBarStyle.Office2000)
			{
				_0B82._1B74 = new Rectangle(_0983.X, _0983.Y + (_0983.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
				renderBarEventArgs.Bounds = _0B82._1B74;
				OnPreRender(renderBarEventArgs);
				_0B82._1B74 = renderBarEventArgs.Bounds;
				_0983.Width -= _0B82._1B74.Width + 2;
				_0983.X += _0B82._1B74.Width + 2;
			}
			else
			{
				_0B82._1B74 = new Rectangle(_0983.Right - (_0B82._1B78.Width + 1), _0983.Y + (_0983.Height - _0B82._1B78.Height) / 2, _0B82._1B78.Width, _0B82._1B78.Height);
				renderBarEventArgs.Bounds = _0B82._1B74;
				OnPreRender(renderBarEventArgs);
				_0B82._1B74 = renderBarEventArgs.Bounds;
				_0983.Width -= _0B82._1B74.Width + 2;
			}
			if (!renderBarEventArgs.Cancel)
			{
				_0BD9(_0BD2);
			}
		}
		if (GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
		{
			_0983.X += 2;
			_0983.Width -= 2;
			_0983.Inflate(0, -2);
			_0B82._1B76 = _0983;
			renderBarEventArgs = new RenderBarEventArgs(this, _0BD2.Graphics, eBarRenderPart.CaptionTaskPane, _0B82._1B76);
			OnPreRender(renderBarEventArgs);
			_0B82._1B76 = renderBarEventArgs.Bounds;
			if (!renderBarEventArgs.Cancel)
			{
				_0BD8(_0BD2);
			}
		}
		_0983.X += 2;
		_0983.Width -= 2;
	}

	protected void PaintOffice(Graphics g)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		_0B82._1B74 = Rectangle.Empty;
		_0B82._1B73 = Rectangle.Empty;
		if (ଡ଼.BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(SystemColors.Control);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(ଡ଼.BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		GetColorScheme();
		_0BDB(g);
		ItemPaintArgs itemPaintArgs = _0A92(g);
		if (ଡ଼.SubItems.Count <= 0)
		{
			return;
		}
		if (_0B5E == eBarState.Popup)
		{
			ControlPaint.DrawBorder3D(g, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
			_0BDB(g);
		}
		else if (_0B5E == eBarState.Floating)
		{
			ControlPaint.DrawBorder3D(g, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
			if (அ.IsEmpty)
			{
				g.FillRectangle(SystemBrushes.ActiveCaption, 4, 4, ((Control)this).ClientSize.Width - 8, 15);
			}
			else
			{
				SolidBrush val3 = new SolidBrush(அ);
				try
				{
					g.FillRectangle((Brush)(object)val3, 4, 4, ((Control)this).ClientSize.Width - 8, 15);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Font val4 = null;
			bool flag = false;
			try
			{
				val4 = new Font(((Control)this).Font, (FontStyle)1);
				flag = true;
			}
			catch
			{
				val4 = SystemFonts.MenuFont;
				flag = true;
			}
			Rectangle bounds = Rectangle.Empty;
			if (ଗ)
			{
				bounds = new Rectangle(18, 4, ((Control)this).ClientSize.Width - 22, 15);
				_0B82._1B74 = new Rectangle(5, 6, 15, 14);
				_0BD9(itemPaintArgs);
			}
			else
			{
				bounds = new Rectangle(4, 4, ((Control)this).ClientSize.Width - 8, 15);
			}
			if (ଔ)
			{
				_0B82._1B73 = new Rectangle(bounds.Right - _0B82._1B78.Width, 6, _0B82._1B78.Width, _0B82._1B78.Height);
				_0BD7(itemPaintArgs);
				bounds.Width -= _0B82._1B78.Width + 1;
			}
			if (ஆ.IsEmpty)
			{
				TextDrawing.DrawString(g, ((Control)this).Text, val4, SystemColors.ActiveCaptionText, bounds, eTextFormat.Default);
			}
			else
			{
				TextDrawing.DrawString(g, ((Control)this).Text, val4, ஆ, bounds, eTextFormat.Default);
			}
			if (flag)
			{
				val4.Dispose();
			}
		}
		else
		{
			BarFunctions._२(g, _0B7F, ((Control)this).ClientRectangle, _0B80);
			_0BD1(itemPaintArgs);
		}
		ଡ଼.Paint(itemPaintArgs);
	}

	private void _0BD6()
	{
		((Control)this).Invalidate(_0B82._1B77);
		((Control)this).Update();
	}

	private void _0BD7()
	{
		((Control)this).Invalidate(_0B82._1B73);
		((Control)this).Update();
	}

	private void _0BD7(ItemPaintArgs _0BD2)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		if (_0B82._1B73.IsEmpty)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (_0AFF)
		{
			_0A79 obj = _0A79;
			_༣ _0F36 = _༣._0F36;
			ཉ య = ཉ.ཐ;
			if (_0B82._1B6D)
			{
				య = ཉ.དྷ;
			}
			else if (_0B82._1B69)
			{
				య = ཉ.ད;
			}
			obj._0EE5(graphics, _0F36, య, _0B82._1B73);
			return;
		}
		if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			ButtonState val = (ButtonState)0;
			if (_0B82._1B6D)
			{
				val = (ButtonState)512;
			}
			ControlPaint.DrawCaptionButton(graphics, _0B82._1B73, (CaptionButton)0, val);
			return;
		}
		Pen val2 = null;
		if (_0B82._1B6D)
		{
			if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B73, _0BD2.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val3 = BarFunctions.ॴ(_0B82._1B73, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, _0B82._1B73);
				((Brush)val3).Dispose();
			}
			Pen val4 = new Pen(_0BD2.Colors.ItemPressedBorder);
			try
			{
				_089F.र(graphics, val4, _0B82._1B73);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		else if (_0B82._1B69)
		{
			val2 = new Pen(_0BD2.Colors.ItemHotText);
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val5 = BarFunctions.ॴ(_0B82._1B73, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val5, _0B82._1B73);
				((Brush)val5).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B73, _0BD2.Colors.ItemHotBackground);
			}
			val2.Dispose();
			val2 = null;
			Pen val6 = new Pen(_0BD2.Colors.ItemHotBorder);
			try
			{
				_089F.र(graphics, val6, _0B82._1B73);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		val2 = ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? new Pen((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText) : new Pen(_0BD2.Colors.ItemText, (float)Dpi.Width1));
		if (_0B82._1B69)
		{
			val2 = new Pen(_0BD2.Colors.ItemHotText, (float)Dpi.Width1);
		}
		else if (_0B82._1B6D)
		{
			val2 = new Pen(_0BD2.Colors.ItemPressedText, (float)Dpi.Width1);
		}
		else if (!ஆ.IsEmpty)
		{
			val2 = new Pen(ஆ, (float)Dpi.Width1);
		}
		else if (!_0B8B && _0B5E != eBarState.Floating && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted)
		{
			val2 = new Pen(_0BD2.Colors.BarCaptionInactiveText, (float)Dpi.Width1);
		}
		Point[] array = new Point[2];
		Rectangle rectangle = _0B82._1B73;
		rectangle.Inflate(-1, -1);
		array[0].X = rectangle.Left + Dpi.Width2;
		array[0].Y = rectangle.Top + Dpi.Height3;
		array[1].X = rectangle.Right - Dpi.Width4;
		array[1].Y = rectangle.Bottom - Dpi.Height4;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X++;
		array[1].X++;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X = rectangle.Right - Dpi.Width3;
		array[0].Y = rectangle.Top + Dpi.Height3;
		array[1].X = rectangle.Left + Dpi.Width3;
		array[1].Y = rectangle.Bottom - Dpi.Height4;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X--;
		array[1].X--;
		graphics.DrawLine(val2, array[0], array[1]);
		val2.Dispose();
	}

	private void _0BD6(ItemPaintArgs _0BD2)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		if (_0B82._1B77.IsEmpty)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (_0AFF)
		{
			_0A79 obj = _0A79;
			_༣ _0F2A = _༣._0F2A;
			ཉ య = ཉ.ཐ;
			if (_0B82._1B6E)
			{
				య = ཉ.དྷ;
			}
			else if (_0B82._1B6A)
			{
				య = ཉ.ད;
			}
			obj._0EE5(graphics, _0F2A, య, _0B82._1B77);
			return;
		}
		if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			ButtonState val = (ButtonState)0;
			if (_0B82._1B6E)
			{
				val = (ButtonState)512;
			}
			ControlPaint.DrawCaptionButton(graphics, _0B82._1B77, (CaptionButton)2, val);
			return;
		}
		Pen val2 = null;
		if (_0B82._1B6E)
		{
			if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B77, _0BD2.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val3 = BarFunctions.ॴ(_0B82._1B77, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, _0B82._1B73);
				((Brush)val3).Dispose();
			}
			Pen val4 = new Pen(_0BD2.Colors.ItemPressedBorder);
			try
			{
				_089F.र(graphics, val4, _0B82._1B77);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		else if (_0B82._1B6A)
		{
			val2 = new Pen(_0BD2.Colors.ItemHotText);
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val5 = BarFunctions.ॴ(_0B82._1B77, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val5, _0B82._1B77);
				((Brush)val5).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B77, _0BD2.Colors.ItemHotBackground);
			}
			val2.Dispose();
			val2 = null;
			Pen val6 = new Pen(_0BD2.Colors.ItemHotBorder);
			try
			{
				_089F.र(graphics, val6, _0B82._1B77);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		SolidBrush val7 = null;
		val7 = ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? new SolidBrush((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText) : new SolidBrush(_0BD2.Colors.ItemText));
		if (_0B82._1B6A)
		{
			val7 = new SolidBrush(_0BD2.Colors.ItemHotText);
		}
		else if (_0B82._1B6E)
		{
			val7 = new SolidBrush(_0BD2.Colors.ItemPressedText);
		}
		else if (!ஆ.IsEmpty)
		{
			val7 = new SolidBrush(ஆ);
		}
		else if (!_0B8B && _0B5E != eBarState.Floating && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted)
		{
			val7 = new SolidBrush(_0BD2.Colors.BarCaptionInactiveText);
		}
		Rectangle _1B77 = _0B82._1B77;
		_1B77.Inflate(-1, -1);
		Region clip = graphics.Clip;
		if (_வ)
		{
			Rectangle rectangle = new Rectangle(_1B77.X + (_1B77.Width - Dpi.Width10) / 2 + Dpi.Width3, _1B77.Y + (_1B77.Height - Dpi.Height10) / 2, Dpi.Width7, Dpi.Height7);
			graphics.SetClip(new Rectangle(rectangle.X + Dpi.Width1, rectangle.Y + Dpi.Height2, rectangle.Width - Dpi.Width2, rectangle.Height - Dpi.Height3), (CombineMode)4);
			rectangle.Offset(-Dpi.Width3, Dpi.Height3);
			graphics.SetClip(new Rectangle(rectangle.X + Dpi.Width1, rectangle.Y + Dpi.Height2, rectangle.Width - Dpi.Width2, rectangle.Height - Dpi.Height3), (CombineMode)4);
			rectangle.Offset(Dpi.Width3, -Dpi.Height3);
			graphics.FillRectangle((Brush)(object)val7, rectangle);
			graphics.Clip = clip;
			rectangle.Offset(-Dpi.Width3, Dpi.Height3);
			graphics.SetClip(new Rectangle(rectangle.X + Dpi.Width1, rectangle.Y + Dpi.Height2, rectangle.Width - Dpi.Width2, rectangle.Height - Dpi.Height3), (CombineMode)4);
			graphics.FillRectangle((Brush)(object)val7, rectangle);
		}
		else
		{
			Rectangle rectangle2 = new Rectangle(_1B77.X + (_1B77.Width - Dpi.Width9) / 2, _1B77.Y + (_1B77.Height - Dpi.Height9) / 2, Dpi.Width9, Dpi.Height9);
			graphics.SetClip(new Rectangle(rectangle2.X + Dpi.Width1, rectangle2.Y + Dpi.Height3, rectangle2.Width - Dpi.Width2, rectangle2.Height - Dpi.Height4), (CombineMode)4);
			graphics.FillRectangle((Brush)(object)val7, rectangle2);
		}
		graphics.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
		((Brush)val7).Dispose();
	}

	private void _0BD8()
	{
		((Control)this).Invalidate(_0B82._1B76);
		((Control)this).Update();
	}

	private void _0BD8(ItemPaintArgs _0BD2)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		if (_0B82._1B76.IsEmpty)
		{
			return;
		}
		Rectangle _1B76 = _0B82._1B76;
		Graphics graphics = _0BD2.Graphics;
		if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			ButtonState val = (ButtonState)0;
			if (_0B82._1B6F)
			{
				val = (ButtonState)512;
			}
			ControlPaint.DrawButton(graphics, _1B76, val);
		}
		else if (ண != null && ண.Expanded)
		{
			if (_0BD2.Colors.ItemExpandedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, _1B76, _0BD2.Colors.ItemExpandedBackground);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(_1B76, _0BD2.Colors.ItemExpandedBackground, _0BD2.Colors.ItemExpandedBackground2, _0BD2.Colors.ItemExpandedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, _1B76);
				((Brush)val2).Dispose();
			}
			Pen val3 = new Pen(_0BD2.Colors.ItemExpandedBorder);
			try
			{
				_089F.र(graphics, val3, _1B76);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		else if (_0B82._1B6F)
		{
			if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, _1B76, _0BD2.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val4 = BarFunctions.ॴ(_1B76, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, _1B76);
				((Brush)val4).Dispose();
			}
			Pen val5 = new Pen(_0BD2.Colors.ItemPressedBorder);
			try
			{
				_089F.र(graphics, val5, _1B76);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		else if (_0B82._1B6B)
		{
			new Pen(_0BD2.Colors.ItemHotText);
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val6 = BarFunctions.ॴ(_1B76, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val6, _1B76);
				((Brush)val6).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, _1B76, _0BD2.Colors.ItemHotBackground);
			}
			Pen val7 = new Pen(_0BD2.Colors.ItemHotBorder);
			try
			{
				_089F.र(graphics, val7, _1B76);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		SolidBrush val8 = null;
		Color itemText = _0BD2.Colors.ItemText;
		itemText = (_0B82._1B6B ? _0BD2.Colors.ItemHotText : (_0B82._1B6F ? _0BD2.Colors.ItemPressedText : (ஆ.IsEmpty ? _0BD2.Colors.ItemText : ஆ)));
		val8 = new SolidBrush(itemText);
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Rectangle bounds = _1B76;
		bounds.Width -= Dpi.Width12;
		bounds.X += Dpi.Width3;
		if (bounds.Width > Dpi.Width8)
		{
			TextDrawing.DrawString(graphics, ((Control)this).Text, ((Control)this).Font, itemText, bounds, format);
		}
		Point[] array = new Point[3];
		array[0].X = _1B76.Right - Dpi.Width9;
		array[0].Y = _1B76.Top + (_1B76.Height + Dpi.Height6) / 2;
		array[1].X = array[0].X - Dpi.Width4;
		array[1].Y = array[0].Y - Dpi.Height5;
		array[2].X = array[1].X + Dpi.Width9;
		array[2].Y = array[1].Y;
		graphics.FillPolygon((Brush)(object)val8, array);
		((Brush)val8).Dispose();
	}

	private void _0BD9()
	{
		((Control)this).Invalidate(_0B82._1B74);
		((Control)this).Update();
	}

	private void _0BD9(ItemPaintArgs _0BD2)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		if (_0B82._1B74.IsEmpty)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		Brush val = (Brush)new SolidBrush((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText);
		if (!_0AFF)
		{
			if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				Point[] array = new Point[3];
				array[0].X = _0B82._1B74.X + Dpi.Width4;
				array[0].Y = _0B82._1B74.Y + Dpi.Height8;
				array[1].X = array[0].X - Dpi.Width3;
				array[1].Y = array[0].Y - Dpi.Height4;
				array[2].X = array[1].X + Dpi.Width7;
				array[2].Y = array[1].Y;
				graphics.FillPolygon(SystemBrushes.ActiveCaptionText, array);
				val.Dispose();
				return;
			}
			if (!ஆ.IsEmpty)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(ஆ);
			}
			if (!_0B8B && _0B5E != eBarState.Floating)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(_0BD2.Colors.BarCaptionInactiveText);
			}
			if (_0B82._1B70)
			{
				if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
				{
					SolidBrush val2 = new SolidBrush(_0BD2.Colors.ItemPressedBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, _0B82._1B74);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val3 = BarFunctions.ॴ(_0B82._1B74, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val3, _0B82._1B74);
					((Brush)val3).Dispose();
				}
				Pen val4 = new Pen(_0BD2.Colors.ItemPressedBorder);
				try
				{
					_089F.र(graphics, val4, _0B82._1B74);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (_0B82._1B6C)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(_0BD2.Colors.BarCaptionText);
				if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
				{
					LinearGradientBrush val5 = BarFunctions.ॴ(_0B82._1B74, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val5, _0B82._1B74);
					((Brush)val5).Dispose();
				}
				else
				{
					SolidBrush val6 = new SolidBrush(_0BD2.Colors.ItemHotBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val6, _0B82._1B74);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				Pen val7 = new Pen(_0BD2.Colors.ItemHotBorder, (float)Dpi.Width1);
				try
				{
					_089F.र(graphics, val7, _0B82._1B74);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
		}
		if (ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(_0BD2.Colors.ItemText);
		}
		else
		{
			val.Dispose();
			val = (Brush)new SolidBrush((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText);
		}
		if (_0B82._1B6C)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(_0BD2.Colors.ItemHotText);
		}
		else if (_0B82._1B70)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(_0BD2.Colors.ItemPressedText);
		}
		else if (!ஆ.IsEmpty)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(ஆ);
		}
		else if (!_0B8B && _0B5E != eBarState.Floating && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(_0BD2.Colors.BarCaptionInactiveText);
		}
		Point[] array2 = new Point[3];
		array2[0].X = _0B82._1B74.Right - Dpi.Width7;
		array2[0].Y = _0B82._1B74.Top + Dpi.Height8;
		array2[1].X = array2[0].X - Dpi.Width3;
		array2[1].Y = array2[0].Y - Dpi.Height4;
		array2[2].X = array2[1].X + Dpi.Width7;
		array2[2].Y = array2[1].Y;
		graphics.FillPolygon(val, array2);
		val.Dispose();
	}

	private void _0BDA()
	{
		if (!_0B82._1B75.IsEmpty)
		{
			Rectangle _1B75 = _0B82._1B75;
			_1B75.Inflate(1, 1);
			((Control)this).Invalidate(_1B75);
			((Control)this).Update();
		}
	}

	private void _0BDA(ItemPaintArgs _0BD2)
	{
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		if (_0B82._1B75.IsEmpty)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (_0AFF)
		{
			Image val = null;
			if ((_0B82._1B72 && !AutoHide) || (AutoHide && !_0B82._1B72))
			{
				val = (Image)(object)((!_0B82._1B71) ? BarFunctions.ই("SystemImages.PinInactive.png") : BarFunctions.ই("SystemImages.Pin.png"));
				graphics.DrawImage(val, _0B82._1B75.X + (_0B82._1B75.Width - val.Width) / 2, _0B82._1B75.Y + (_0B82._1B75.Height - val.Height) / 2, val.Width, val.Height);
			}
			else
			{
				val = (Image)(object)((!_0B82._1B71) ? BarFunctions.ই("SystemImages.PinPushedInactive.png") : BarFunctions.ই("SystemImages.PinPushed.png"));
				graphics.DrawImage(val, _0B82._1B75.X + (_0B82._1B75.Width - val.Width) / 2, _0B82._1B75.Y + (_0B82._1B75.Height - val.Height) / 2, val.Width, val.Height);
			}
			return;
		}
		if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			Rectangle _1B75 = _0B82._1B75;
			_1B75.Inflate(1, 0);
			if (_0B82._1B72)
			{
				ControlPaint.DrawBorder3D(graphics, _1B75, (Border3DStyle)8, (Border3DSide)2063);
			}
			else
			{
				ControlPaint.DrawBorder3D(graphics, _1B75, (Border3DStyle)5, (Border3DSide)2063);
			}
		}
		else if (_0B82._1B72)
		{
			if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B75, _0BD2.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(_0B82._1B75, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, _0B82._1B75);
				((Brush)val2).Dispose();
			}
			Pen val3 = new Pen(_0BD2.Colors.ItemPressedBorder);
			try
			{
				_089F.र(graphics, val3, _0B82._1B75);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		else if (_0B82._1B71)
		{
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val4 = BarFunctions.ॴ(_0B82._1B75, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, _0B82._1B75);
				((Brush)val4).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, _0B82._1B75, _0BD2.Colors.ItemHotBackground);
			}
			Pen val5 = new Pen(_0BD2.Colors.ItemHotBorder);
			try
			{
				_089F.र(graphics, val5, _0B82._1B75);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if ((_0B82._1B72 && !AutoHide) || (AutoHide && !_0B82._1B72))
		{
			Rectangle rectangle = new Rectangle(_0B82._1B75.X + (_0B82._1B75.Width - Dpi.Width10) / 2, _0B82._1B75.Y + (_0B82._1B75.Height - Dpi.Height7) / 2, Dpi.Width10, Dpi.Height7);
			Pen val6 = null;
			val6 = ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? new Pen((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText) : new Pen(_0BD2.Colors.ItemText, (float)Dpi.Width1));
			if (_0B82._1B71)
			{
				val6 = new Pen(_0BD2.Colors.ItemHotText, (float)Dpi.Width1);
			}
			else if (_0B82._1B72)
			{
				val6 = new Pen(_0BD2.Colors.ItemPressedText, (float)Dpi.Width1);
			}
			else if (!ஆ.IsEmpty)
			{
				val6 = new Pen(ஆ, (float)Dpi.Width1);
			}
			else if (!_0B8B && _0B5E != eBarState.Floating && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted)
			{
				val6 = new Pen(_0BD2.Colors.BarCaptionInactiveText, (float)Dpi.Width1);
			}
			graphics.DrawRectangle(val6, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height1, Dpi.Width5, Dpi.Height4);
			graphics.DrawLine(val6, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height4, rectangle.X + Dpi.Width9, rectangle.Y + Dpi.Height4);
			graphics.DrawLine(val6, rectangle.X + Dpi.Width4, rectangle.Y, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height6);
			graphics.DrawLine(val6, rectangle.X, rectangle.Y + Dpi.Height3, rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height3);
		}
		else
		{
			Rectangle rectangle2 = new Rectangle(_0B82._1B75.X + (_0B82._1B75.Width - Dpi.Width7) / 2, _0B82._1B75.Y + (_0B82._1B75.Height - Dpi.Height9) / 2, Dpi.Width7, Dpi.Height9);
			Pen val7 = null;
			val7 = ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? new Pen((_0B8B || LayoutType == eLayoutType.Toolbar) ? _0BD2.Colors.BarCaptionText : _0BD2.Colors.BarCaptionInactiveText, (float)Dpi.Width1) : new Pen(_0BD2.Colors.ItemText, (float)Dpi.Width1));
			if (_0B82._1B71)
			{
				val7 = new Pen(_0BD2.Colors.ItemHotText, (float)Dpi.Width1);
			}
			else if (_0B82._1B72)
			{
				val7 = new Pen(_0BD2.Colors.ItemPressedText, (float)Dpi.Width1);
			}
			else if (!ஆ.IsEmpty)
			{
				val7 = new Pen(ஆ, (float)Dpi.Width1);
			}
			else if (!_0B8B && _0B5E != eBarState.Floating && ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted)
			{
				val7 = new Pen(_0BD2.Colors.BarCaptionInactiveText, (float)Dpi.Width1);
			}
			graphics.DrawRectangle(val7, rectangle2.X + Dpi.Width1, rectangle2.Y, Dpi.Width4, Dpi.Height5);
			graphics.DrawLine(val7, rectangle2.X + Dpi.Width4, rectangle2.Y, rectangle2.X + Dpi.Width4, rectangle2.Y + Dpi.Height5);
			graphics.DrawLine(val7, rectangle2.X, rectangle2.Y + Dpi.Height5, rectangle2.X + Dpi.Width6, rectangle2.Y + Dpi.Height5);
			graphics.DrawLine(val7, rectangle2.X + Dpi.Width3, rectangle2.Y + Dpi.Height5, rectangle2.X + Dpi.Width3, rectangle2.Y + Dpi.Height8);
		}
	}

	internal void _0BDB(Graphics _٥)
	{
		if (_0B5A.Picture != null && _0B5E == eBarState.Popup)
		{
			if (!_0B5A.GradientColor1.IsEmpty && !_0B5A.GradientColor2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.ॴ(_0B5B, _0B5A.GradientColor1, _0B5A.GradientColor2, _0B5A.GradientAngle);
				_٥.FillRectangle((Brush)(object)val, _0B5B);
			}
			else if (!_0B5A.BackColor.Equals((object?)Color.Empty))
			{
				DisplayHelp.FillRectangle(_٥, _0B5B, _0B5A.BackColor);
			}
			if (_0B5A.StretchPicture)
			{
				_٥.DrawImage(_0B5A.Picture, _0B5B);
			}
			else if (_0B5A.Alignment == eAlignment.Top)
			{
				_٥.DrawImage(_0B5A.Picture, _0B5B.X, _0B5B.Top, _0B5B.Width, _0B5A.Picture.Height);
			}
			else if (_0B5A.Alignment == eAlignment.Bottom)
			{
				_٥.DrawImage(_0B5A.Picture, _0B5B.Left, _0B5B.Bottom - _0B5A.Picture.Height, _0B5A.Picture.Width, _0B5A.Picture.Height);
			}
			else
			{
				_٥.DrawImage(_0B5A.Picture, _0B5B.Left, _0B5B.Top + (_0B5B.Height - _0B5A.Picture.Height) / 2, _0B5A.Picture.Width, _0B5A.Picture.Height);
			}
		}
	}

	private void _0BDC()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if (_0B5E == eBarState.Popup)
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

	public void RecalcLayout()
	{
		if (ஹ || ம)
		{
			return;
		}
		if (_0B5E == eBarState.Docked)
		{
			if (((Control)this).Parent != null && !ஏ)
			{
				if (ଡ଼.LayoutType != eLayoutType.DockContainer)
				{
					((Control)this).Invalidate(((Control)this).Region, true);
				}
				if (((Control)this).Parent is DockSite)
				{
					((DockSite)(object)((Control)this).Parent).RecalcLayout();
				}
				else
				{
					RecalcSize();
					((Control)this).Refresh();
				}
				_0C65();
			}
		}
		else
		{
			RecalcSize();
			_0C65();
			((Control)this).Refresh();
		}
	}

	internal void _0BDF(DockContainerItem _06EC, int _९)
	{
		if (!ஹ)
		{
			if (DockSide == eDockSide.Document || (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null))
			{
				((DockSite)(object)DockedSite).GetDocumentUIManager().SetBarWidth(this, _९ + 2);
			}
			else if (_0B5E == eBarState.AutoHide)
			{
				ଡ଼.ර = _९;
				_୬.DockedWidth = _९ + 2;
			}
		}
	}

	internal void _0BE0(DockContainerItem _06EC, int _0970)
	{
		if (!ஹ)
		{
			if (DockSide == eDockSide.Document || (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null))
			{
				((DockSite)(object)DockedSite).GetDocumentUIManager().SetBarHeight(this, _0970 + _0BE3());
			}
			else if (_0B5E == eBarState.AutoHide)
			{
				ଡ଼._0B03 = _0970;
				_୬.DockedHeight = _0970 + 2;
			}
		}
	}

	internal void _0BE1(DockContainerItem _06EC)
	{
		if (AutoHide)
		{
			_୬.DockedHeight = 0;
			_୬.DockedWidth = 0;
		}
	}

	private void _0BE2(Size ߒ)
	{
		if (((Control)this).Parent == null || ((ArrangedElementCollection)((Control)this).Parent.Controls).Count <= 1)
		{
			return;
		}
		Bar bar = null;
		Bar bar2 = null;
		Bar bar3 = _0BFF(this);
		Bar bar4 = _0C01(this);
		if (bar4 != null && bar4.LayoutType == eLayoutType.DockContainer && bar4.DockLine == DockLine)
		{
			bar = this;
			bar2 = bar4;
		}
		else if (bar3 != null && bar3.LayoutType == eLayoutType.DockContainer && bar3.DockLine == DockLine)
		{
			bar = bar3;
			bar2 = this;
			if (ߒ.Width > 0)
			{
				ߒ.Width = ((Control)bar).Width + (((Control)bar2).Width - ߒ.Width);
			}
			if (ߒ.Height > 0)
			{
				ߒ.Height = ((Control)bar).Height + (((Control)bar2).Height - ߒ.Height);
			}
		}
		if (DockSide == eDockSide.Left || DockSide == eDockSide.Right)
		{
			if (bar != null && bar2 != null && bar.DockLine == bar2.DockLine)
			{
				Size size = bar.MinimumDockSize(eOrientation.Horizontal);
				Size size2 = bar2.MinimumDockSize(eOrientation.Horizontal);
				int num = ߒ.Height - ((Control)bar).Height;
				if (ߒ.Height >= size.Height && ((Control)bar2).Height - num >= size2.Height)
				{
					Size size3 = bar.Size;
					Size size4 = bar2.Size;
					Size size5 = bar._௧(new Size(((Control)bar).Width, ߒ.Height));
					Size size6 = bar2._௧(new Size(((Control)bar2).Width, ((Control)bar2).Height - num));
					if (!size3.Equals((object?)size5) && !size4.Equals((object?)size6))
					{
						bar2._0B01 = 0;
						bar._0B01 = ߒ.Height;
					}
				}
			}
			else
			{
				_0B01 = 0;
				_0B02 = 0;
			}
		}
		else
		{
			if (DockSide != eDockSide.Top && DockSide != eDockSide.Bottom)
			{
				return;
			}
			if (bar != null && bar2 != null && bar.DockLine == bar2.DockLine)
			{
				Size size7 = bar.MinimumDockSize(eOrientation.Horizontal);
				Size size8 = bar2.MinimumDockSize(eOrientation.Horizontal);
				int num2 = ߒ.Width - ((Control)bar).Width;
				if (ߒ.Width >= size7.Width && ((Control)bar2).Width - num2 >= size8.Width)
				{
					Size size9 = bar.Size;
					Size size10 = bar2.Size;
					Size size11 = bar._௧(new Size(ߒ.Width, ((Control)bar).Height));
					Size size12 = bar2._௧(new Size(((Control)bar2).Width - num2, ((Control)bar2).Height));
					if (!size9.Equals((object?)size11) && !size10.Equals((object?)size12))
					{
						bar2._0B02 = 0;
						bar._0B02 = ߒ.Width;
					}
				}
			}
			else
			{
				_0B01 = 0;
				_0B02 = 0;
			}
		}
	}

	internal int _0BE3()
	{
		return _0BF6() + 2;
	}

	public void RecalcSize()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		ୠ = Rectangle.Empty;
		if (ர || ଡ଼ == null)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			return;
		}
		ர = true;
		if (LayoutType == eLayoutType.DockContainer && ஈ == null)
		{
			ౘ(ౙ: false);
		}
		try
		{
			if (_0B5E == eBarState.Docked)
			{
				ଡ଼.WrapItems = _୭;
				ଡ଼.Stretch = _0B70;
			}
			else if (_0B5E == eBarState.Floating || _0B5E == eBarState.AutoHide)
			{
				ଡ଼.WrapItems = _୮;
				if (ଡ଼.LayoutType == eLayoutType.DockContainer)
				{
					ଡ଼.Stretch = _0B70;
				}
				else
				{
					ଡ଼.Stretch = false;
				}
			}
			else
			{
				ଡ଼.WrapItems = true;
				ଡ଼.Stretch = false;
			}
			if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				if (_0B5E == eBarState.Docked && !(((Control)this).Parent is DockSite))
				{
					Size size = _௮();
					if (ଡ଼.Orientation == eOrientation.Horizontal)
					{
						((Control)this).Height = size.Height;
					}
					else
					{
						((Control)this).Width = size.Width;
					}
				}
				else
				{
					((Control)this).ClientSize = _௮();
				}
			}
			else if (_0B5E == eBarState.Docked && !(((Control)this).Parent is DockSite))
			{
				Size size2 = _௬();
				if (ଡ଼.Orientation == eOrientation.Horizontal)
				{
					((Control)this).Height = size2.Height;
				}
				else
				{
					((Control)this).Width = size2.Width;
				}
			}
			else
			{
				((Control)this).ClientSize = _௬();
			}
			if (_0B84 != null)
			{
				_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			}
			if (_0B5E == eBarState.Docked)
			{
				if (ଡ଼.Orientation == eOrientation.Horizontal)
				{
					_୧ = Size;
				}
				else
				{
					_୨ = Size;
				}
				_0BE4();
			}
			else if (((Control)this).Region != null)
			{
				((Control)this).Region = null;
			}
			if (_0B5E == eBarState.Floating && !ஏ)
			{
				ୱ.ᆥ(((Control)this).ClientSize);
				((Form)ୱ).ClientSize = ((Control)this).ClientSize;
			}
		}
		finally
		{
			ர = false;
		}
		if (LayoutType == eLayoutType.DockContainer && ஈ != null && ((Component)this).DesignMode && ((Component)(object)this).Site != null)
		{
			ౘ(ౙ: false);
		}
		OnItemLayoutUpdated(EventArgs.Empty);
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		ਞ?.Invoke(this, e);
	}

	private void _0BE4()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		if (_0BAB && _0AFB && !MenuBar && LayoutType == eLayoutType.Toolbar && BarType == eBarType.Toolbar && !IsThemed && GrabHandleStyle != eGrabHandleStyle.ResizeHandle && ((Control)this).BackColor != Color.Transparent)
		{
			GraphicsPath val = new GraphicsPath();
			val.AddLine(0, 2, 2, 0);
			val.AddLine(2, 0, ((Control)this).Width - 2, 0);
			val.AddLine(((Control)this).Width, 2, ((Control)this).Width, ((Control)this).Height - 3);
			val.AddLine(((Control)this).Width - 3, ((Control)this).Height, 3, ((Control)this).Height);
			val.AddLine(2, ((Control)this).Height, 0, ((Control)this).Height - 3);
			((Control)this).Region = new Region(val);
		}
		else if (((Control)this).Region != null)
		{
			((Control)this).Region = null;
		}
	}

	internal void _0BE5(Size _௦)
	{
		ୠ = Rectangle.Empty;
		if (ர || ଡ଼ == null)
		{
			return;
		}
		ர = true;
		try
		{
			if (_0B5E == eBarState.Docked)
			{
				ଡ଼.WrapItems = _୭;
				ଡ଼.Stretch = _0B70;
			}
			else if (_0B5E == eBarState.Floating || _0B5E == eBarState.AutoHide)
			{
				ଡ଼.WrapItems = _୮;
				if (ଡ଼.LayoutType == eLayoutType.DockContainer)
				{
					ଡ଼.Stretch = _0B70;
				}
				else
				{
					ଡ଼.Stretch = false;
				}
			}
			else
			{
				ଡ଼.WrapItems = true;
				ଡ଼.Stretch = false;
			}
			if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				((Control)this).ClientSize = _௮(_௭: false, _௦, ଡ଼.Orientation, _0B5E, ଡ଼.WrapItems);
			}
			else
			{
				((Control)this).ClientSize = _௬(_௭: false, _௦, ଡ଼.Orientation, _0B5E, ଡ଼.WrapItems);
			}
			if (_0B84 != null)
			{
				_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			}
			if (_0B5E == eBarState.Docked)
			{
				if (ଡ଼.Orientation == eOrientation.Horizontal)
				{
					_୧ = Size;
				}
				else
				{
					_୨ = Size;
				}
				_0BE4();
			}
			if (_0B5E == eBarState.Floating && !ஏ)
			{
				ୱ.ᆥ(((Control)this).ClientSize);
				((Form)ୱ).ClientSize = ((Control)this).ClientSize;
			}
		}
		finally
		{
			ர = false;
		}
	}

	private Size _௧(Size _௨)
	{
		return _௧(_௨, ଡ଼.Orientation, _0B5E, ଡ଼.WrapItems);
	}

	private Size _௧(Size _௨, eOrientation _௩, eBarState _௪, bool _௫)
	{
		if (ଡ଼.SubItems.Count > 0)
		{
			if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return _௮(_௭: true, _௨, _௩, _௪, _௫);
			}
			return _௬(_௭: true, _௨, _௩, _௪, _௫);
		}
		return Size.Empty;
	}

	private Size _௬()
	{
		return _௬(_௭: false, Size, ଡ଼.Orientation, _0B5E, ଡ଼.WrapItems);
	}

	private Size _௬(bool _௭, Size _௨, eOrientation _௩, eBarState _௪, bool _௫)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Invalid comparison between Unknown and I4
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Invalid comparison between Unknown and I4
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Invalid comparison between Unknown and I4
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Invalid comparison between Unknown and I4
		Size empty = Size.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int widthInternal = 0;
		int heightInternal = 0;
		eOrientation orientation = ଡ଼.Orientation;
		eBarState eBarState2 = _0B5E;
		bool stretch = ଡ଼.Stretch;
		bool wrapItems = ଡ଼.WrapItems;
		ଡ଼.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
		if (_௭)
		{
			ଡ଼.Orientation = _௩;
			eBarState2 = _௪;
			ଡ଼.WrapItems = _௫;
			if (_௪ == eBarState.Docked)
			{
				ଡ଼.Stretch = _0B70;
			}
			else if (ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				ଡ଼.Stretch = _0B70;
			}
			else
			{
				ଡ଼.Stretch = false;
			}
		}
		switch (eBarState2)
		{
		case eBarState.Docked:
			if (_0B7F != 0)
			{
				num = Dpi.Height3;
				num2 = Dpi.Height3;
				num3 = Dpi.Width3;
				num4 = Dpi.Width3;
			}
			break;
		case eBarState.Floating:
			if (IsThemed || _0AFF)
			{
				if (ஐ.ᮄ)
				{
					ఙ();
				}
				num = ஐ._074A;
				num3 = ஐ._0749;
				num4 = ஐ.ሪ;
				num2 = ஐ.ራ;
				if (GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
				{
					num = _0BF5() + ஐ.ራ;
				}
			}
			else
			{
				num = ((GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane) ? _0BE3() : (_0BF5() + Dpi.Height4));
				num3 = Dpi.Width2;
				num4 = Dpi.Width2;
				num2 = Dpi.Height3;
			}
			break;
		default:
			num = ଉ;
			num3 = ଈ;
			num4 = ଊ;
			num2 = ଋ;
			break;
		}
		int num6 = num;
		int num7 = num3;
		if (BarType == eBarType.StatusBar && BarFunctions.IsOffice2007Style(ଡ଼.EffectiveStyle))
		{
			num6 += Dpi.Height2;
		}
		if (ଡ଼.LayoutType == eLayoutType.DockContainer && !உ && _0C63() && (ଡ଼.VisibleSubItems > 1 || (_0BA0 && ଡ଼.VisibleSubItems == 1 && _0B5E != eBarState.Floating)))
		{
			int num8 = Dpi.Height(DockTabStripHeight);
			switch (ஔ)
			{
			case eTabStripAlignment.Top:
				num6 += num8;
				break;
			case eTabStripAlignment.Left:
				num7 += num8;
				break;
			case eTabStripAlignment.Right:
				num5 = num8;
				break;
			default:
				num2 += num8;
				break;
			}
		}
		if (eBarState2 == eBarState.Popup && _0B5A.Picture != null)
		{
			num7 += _0B5A.Picture.Width;
		}
		else if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && ୟ != 0)
		{
			if (ୟ == eGrabHandleStyle.ResizeHandle)
			{
				if ((int)((Control)this).RightToLeft == 1)
				{
					num7 += 17;
				}
				else
				{
					num4 += 17;
				}
			}
			else if (ଡ଼.Orientation != 0 || ଡ଼.LayoutType != 0)
			{
				num6 = ((ୟ == eGrabHandleStyle.Caption) ? (num6 + _0BF6()) : ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? (num6 + 7) : (num6 + _0BF5())));
			}
			else if (ୟ == eGrabHandleStyle.Caption)
			{
				num7 += _0BF6();
			}
			else if ((int)((Control)this).RightToLeft == 1)
			{
				num4 += 7;
			}
			else
			{
				num7 += 7;
			}
		}
		if (!_௭)
		{
			ଡ଼.TopInternal = num6;
			ଡ଼.LeftInternal = num7;
		}
		else
		{
			heightInternal = ଡ଼.HeightInternal;
			widthInternal = ଡ଼.WidthInternal;
		}
		ଡ଼.SuspendLayout = true;
		if (!_௭)
		{
			if (eBarState2 == eBarState.Popup)
			{
				ଡ଼.WidthInternal = ଢ଼;
			}
			else
			{
				ଡ଼.WidthInternal = _௨.Width - num7 - num4 - num5;
				ଡ଼.HeightInternal = _௨.Height - num6 - num2;
			}
		}
		else
		{
			ଡ଼.WidthInternal = _௨.Width - num7 - num4 - num5;
			ଡ଼.HeightInternal = _௨.Height - num6 - num2;
		}
		ଡ଼.SuspendLayout = false;
		if (ଡ଼.VisibleSubItems == 0 || (ଡ଼.SubItems.Count == 1 && ଡ଼.SubItems[0] is CustomizeItem && !ଡ଼.SubItems[0].Visible))
		{
			ଡ଼.RecalcSize();
			if (LayoutType != eLayoutType.DockContainer && DockSide != eDockSide.Document)
			{
				ଡ଼.WidthInternal = 36;
				ଡ଼.HeightInternal = 24;
			}
		}
		else
		{
			ଡ଼.RecalcSize();
		}
		num6 += ଡ଼.HeightInternal;
		if (_0AFB && LayoutType == eLayoutType.Toolbar && !MenuBar && !IsThemed && BarType != eBarType.StatusBar)
		{
			num6++;
		}
		if (!_௭)
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				if (LayoutType == eLayoutType.Toolbar && ଡ଼.WidthInternal < _௨.Width - num7 - num4 - num5 && Stretch)
				{
					ଡ଼.LeftInternal = _௨.Width - num4 - ଡ଼.WidthInternal;
					ଡ଼.RecalcSize();
				}
				_0B59 = new Rectangle(ଡ଼.LeftInternal, num, ଡ଼.WidthInternal, num6);
			}
			else
			{
				_0B59 = new Rectangle(num7, num, ଡ଼.WidthInternal, num6);
			}
		}
		if (!_௭)
		{
			if ((eBarState2 == eBarState.Docked || eBarState2 == eBarState.AutoHide) && ୟ != 0 && ୟ != eGrabHandleStyle.ResizeHandle)
			{
				if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType == eLayoutType.Toolbar)
				{
					if (ୟ == eGrabHandleStyle.Caption)
					{
						ୠ = new Rectangle(num3, num, _0BF6(), num6);
					}
					else if ((int)((Control)this).RightToLeft == 1)
					{
						ୠ = new Rectangle(ଡ଼.WidthInternal + num7 + num3, num, 10, num6);
					}
					else
					{
						ୠ = new Rectangle(num3, num, 10, num6);
					}
				}
				else if (ୟ == eGrabHandleStyle.Caption)
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, _0BF6());
				}
				else if (ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted)
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, _0BF5());
				}
				else
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, 10);
				}
			}
			else if (eBarState2 == eBarState.Floating)
			{
				if (ୟ == eGrabHandleStyle.CaptionTaskPane)
				{
					ୠ = new Rectangle(num3, (IsThemed || _0AFF) ? ஐ.ራ : 2, ଡ଼.WidthInternal + num7 - num3 + num5, _0BF5());
				}
				else
				{
					ୠ = new Rectangle(2, 2, ଡ଼.WidthInternal + num5, _0BF6());
					if (ଔ)
					{
						ୠ.Width -= 14;
					}
				}
			}
			if (eBarState2 == eBarState.Popup && _0B5A.Picture != null)
			{
				_0B5B = new Rectangle(num7 - _0B5A.Picture.Width, num, _0B5A.Picture.Width, num6 - num);
			}
			empty = new Size(num7 + ଡ଼.WidthInternal + num4 + num5, num6 + num2);
		}
		else
		{
			empty = new Size(num7 + ଡ଼.WidthInternal + num4 + num5, num6 + num2);
			ଡ଼.WidthInternal = widthInternal;
			ଡ଼.HeightInternal = heightInternal;
			ଡ଼.Orientation = orientation;
			ଡ଼.WrapItems = wrapItems;
			ଡ଼.Stretch = stretch;
			ଡ଼.RecalcSize();
		}
		return empty;
	}

	private Size _௮()
	{
		return _௮(_௭: false, Size, ଡ଼.Orientation, _0B5E, ଡ଼.WrapItems);
	}

	private Size _௮(bool _௭, Size _௨, eOrientation _௩, eBarState _௪, bool _௫)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		Size empty = Size.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int widthInternal = 0;
		int heightInternal = 0;
		eOrientation orientation = ଡ଼.Orientation;
		eBarState eBarState2 = _0B5E;
		bool stretch = ଡ଼.Stretch;
		bool wrapItems = ଡ଼.WrapItems;
		ଡ଼.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
		if (_௭)
		{
			ଡ଼.Orientation = _௩;
			eBarState2 = _௪;
			ଡ଼.WrapItems = _௫;
			if (_௪ == eBarState.Docked)
			{
				ଡ଼.Stretch = _0B70;
			}
			else if (ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				ଡ଼.Stretch = _0B70;
			}
			else
			{
				ଡ଼.Stretch = false;
			}
		}
		switch (eBarState2)
		{
		case eBarState.Docked:
			if (_0B7F != 0)
			{
				num = 2;
				num2 = 2;
				num3 = 2;
				num4 = 2;
			}
			break;
		case eBarState.Floating:
			num = 22;
			num3 = 4;
			num4 = 4;
			num2 = 4;
			break;
		default:
			num = ଉ;
			num3 = ଈ;
			num4 = ଊ;
			num2 = ଋ;
			break;
		}
		int num6 = num;
		int num7 = num3;
		if (ଡ଼.LayoutType == eLayoutType.DockContainer && !உ && _0C63() && (ଡ଼.VisibleSubItems > 1 || (_0BA0 && ଡ଼.VisibleSubItems == 1 && _0B5E != eBarState.Floating)))
		{
			int num8 = Dpi.Height(DockTabStripHeight);
			switch (ஔ)
			{
			case eTabStripAlignment.Top:
				num6 += num8;
				break;
			case eTabStripAlignment.Left:
				num7 += num8;
				break;
			case eTabStripAlignment.Right:
				num5 = num8;
				break;
			default:
				num2 += num8;
				break;
			}
		}
		if (eBarState2 == eBarState.Popup && _0B5A.Picture != null)
		{
			num7 += _0B5A.Picture.Width;
		}
		else if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && ୟ != 0)
		{
			if (ଡ଼.Orientation != 0 || ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				num6 = ((ୟ == eGrabHandleStyle.Caption) ? (num6 + _0BF6()) : ((ୟ != eGrabHandleStyle.CaptionTaskPane && ୟ != eGrabHandleStyle.CaptionDotted) ? (num6 + 7) : (num6 + _0BF5())));
			}
			else
			{
				num7 += 7;
			}
		}
		if (!_௭)
		{
			ଡ଼.TopInternal = num6;
			ଡ଼.LeftInternal = num7;
		}
		else
		{
			heightInternal = ଡ଼.HeightInternal;
			widthInternal = ଡ଼.WidthInternal;
		}
		if (!_௭)
		{
			if (eBarState2 == eBarState.Popup)
			{
				ଡ଼.WidthInternal = ଢ଼;
			}
			else
			{
				ଡ଼.WidthInternal = _௨.Width - num7 - num4 - num5;
				ଡ଼.HeightInternal = _௨.Height - num6 - num2;
			}
		}
		else
		{
			ଡ଼.WidthInternal = _௨.Width - num7 - num4 - num5;
			ଡ଼.HeightInternal = _௨.Height - num6 - num2;
		}
		if (ଡ଼.VisibleSubItems == 0)
		{
			ଡ଼.WidthInternal = 36;
			ଡ଼.HeightInternal = 24;
		}
		else
		{
			ଡ଼.RecalcSize();
		}
		num6 += ଡ଼.HeightInternal;
		if (!_௭)
		{
			_0B59 = new Rectangle(num7, num, ଡ଼.WidthInternal, num6);
		}
		if (!_௭)
		{
			if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && ୟ != 0)
			{
				if (ଡ଼.Orientation == eOrientation.Horizontal && ଡ଼.LayoutType != eLayoutType.DockContainer)
				{
					ୠ = new Rectangle(num3, num, 12, num6);
				}
				else if (ୟ == eGrabHandleStyle.Caption)
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, _0BF6());
				}
				else if (ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted)
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, _0BF5());
				}
				else
				{
					ୠ = new Rectangle(num3, num, ଡ଼.WidthInternal + num7 - num3 + num5, 12);
				}
			}
			else if (eBarState2 == eBarState.Floating)
			{
				if (ୟ == eGrabHandleStyle.CaptionTaskPane)
				{
					ୠ = new Rectangle(4, 4, ଡ଼.WidthInternal + num5, _0BF5());
				}
				else
				{
					ୠ = new Rectangle(4, 4, ଡ଼.WidthInternal + num5, 15);
				}
				if (ଔ)
				{
					ୠ.Width -= 14;
				}
			}
			if (eBarState2 == eBarState.Popup && _0B5A.Picture != null)
			{
				_0B5B = new Rectangle(num7 - _0B5A.Picture.Width, num, _0B5A.Picture.Width, num6 - num);
			}
			empty = new Size(num7 + ଡ଼.WidthInternal + num4 + num5, num6 + num2);
		}
		else
		{
			empty = new Size(num7 + ଡ଼.WidthInternal + num4 + num5, num6 + num2);
			ଡ଼.WidthInternal = widthInternal;
			ଡ଼.HeightInternal = heightInternal;
			ଡ଼.Orientation = orientation;
			ଡ଼.WrapItems = wrapItems;
			ଡ଼.Stretch = stretch;
			ଡ଼.RecalcSize();
		}
		return empty;
	}

	private void _௯()
	{
		if (_0AF1 == null)
		{
			return;
		}
		foreach (BaseItem subItem in _0AF1.SubItems)
		{
			subItem.Expanded = false;
			subItem.SetParent(_0AF1);
			subItem.ContainerControl = _0B58;
			subItem.Displayed = false;
			subItem.Orientation = _0AF1.Orientation;
		}
		ଡ଼.SubItems.__0AF5();
	}

	internal void _0BF0()
	{
		if (_0AC3 != null && _0AC3 is IOwner && ((IOwner)_0AC3).Images != null)
		{
			ଡ଼.RefreshImageSize();
		}
	}

	internal void _0BF1()
	{
		if (_0B9D < 0)
		{
			return;
		}
		if (_0B9D != 5 || DockSide != eDockSide.Document)
		{
			DockSide = (eDockSide)_0B9D;
		}
		_0B9D = -1;
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			subItem.OnProcessDelayedCommands();
		}
	}

	private bool _0BF2(Control _0BF3, Control _0BF4)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (_0BF3 == _0BF4)
		{
			return true;
		}
		foreach (Control item in (ArrangedElementCollection)_0BF3.Controls)
		{
			Control val = item;
			if (val == _0BF4)
			{
				return true;
			}
			if (_0BF2(val, _0BF4))
			{
				return true;
			}
		}
		return false;
	}

	private int _0BF5()
	{
		if (((Control)this).Font != null)
		{
			return Math.Max(Dpi.Height(23), ((Control)this).Font.Height + Dpi.Height2);
		}
		return Dpi.Height(23);
	}

	private int _0BF6()
	{
		if (_ற > 0)
		{
			return Dpi.Height(_ற);
		}
		if (((Control)this).Font != null)
		{
			return Math.Max(Dpi.Height(20), ((Control)this).Font.Height + 2);
		}
		return Dpi.Height(20);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DockDocumentManager(DockSiteInfo pDockInfo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		DocumentDockUIManager documentUIManager = pDockInfo.objDockSite.GetDocumentUIManager();
		if ((int)((Control)pDockInfo.objDockSite).Dock == 5)
		{
			documentUIManager.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
		}
		else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && ((int)((Control)pDockInfo.objDockSite).Dock == 3 || (int)((Control)pDockInfo.objDockSite).Dock == 4))
		{
			documentUIManager.Dock(null, this, (pDockInfo.DockLine == -1) ? eDockSide.Left : eDockSide.None);
		}
		else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && ((int)((Control)pDockInfo.objDockSite).Dock == 1 || (int)((Control)pDockInfo.objDockSite).Dock == 2))
		{
			documentUIManager.Dock(null, this, (pDockInfo.DockLine == -1) ? eDockSide.Top : eDockSide.None);
		}
		else
		{
			documentUIManager.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
		}
	}

	internal void _0BF7()
	{
		if (ୱ != null)
		{
			_୦ = new Rectangle(((Form)ୱ).Location, Size);
			((Control)ୱ).Controls.Remove((Control)(object)this);
			((Control)ୱ).Hide();
			((Component)(object)ୱ).Dispose();
			ୱ = null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DockingHandler(DockSiteInfo pDockInfo, Point p)
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected I4, but got Unknown
		IOwner owner = _0AC3 as IOwner;
		if (ஓ != null && pDockInfo.TabDockContainer == ஓ)
		{
			return;
		}
		if (pDockInfo.objDockSite == null && (_0B5E != eBarState.Floating || ୱ == null))
		{
			_0B72 = true;
			if (ஓ != null)
			{
				_0BFA();
				ஓ.RecalcLayout();
				ஓ = null;
			}
			DockSiteInfo dockSiteInfo = _୬;
			_୬ = new DockSiteInfo();
			_୬.LastRelativeDocumentId = dockSiteInfo.LastRelativeDocumentId;
			_୬.LastRelativeDockToBar = dockSiteInfo.LastRelativeDockToBar;
			_୬.DockedHeight = ((Control)this).Height;
			_୬.DockedWidth = ((Control)this).Width;
			_୬.DockLine = DockLine;
			_୬.DockOffset = DockOffset;
			if (((Control)this).Parent != null)
			{
				_୬.DockSide = ((Control)this).Parent.Dock;
			}
			else
			{
				_୬.DockSide = (DockStyle)3;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				_୬.InsertPosition = ((Control)(DockSite)(object)((Control)this).Parent).Controls.GetChildIndex((Control)(object)this);
				_୬.objDockSite = (DockSite)(object)DockedSite;
			}
			if (_୬.objDockSite == null && _0AC3 is IOwnerBarSupport ownerBarSupport)
			{
				DockStyle dockSide = _୬.DockSide;
				switch (dockSide - 1)
				{
				case 2:
					_୬.objDockSite = ownerBarSupport.LeftDockSite;
					break;
				case 3:
					_୬.objDockSite = ownerBarSupport.RightDockSite;
					break;
				case 0:
					_୬.objDockSite = ownerBarSupport.TopDockSite;
					break;
				case 1:
					_୬.objDockSite = ownerBarSupport.BottomDockSite;
					break;
				case 4:
					_୬.objDockSite = ownerBarSupport.FillDockSite;
					break;
				}
			}
			_0B5E = eBarState.Floating;
			if (ୱ == null)
			{
				ୱ = new ᆡ(this);
				((Control)ୱ).CreateControl();
			}
			if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			else if (owner.ParentForm != null && _0BF2((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				if (((DockSite)(object)((Control)this).Parent).ၓ || ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
				{
					((DockSite)(object)((Control)this).Parent).GetDocumentUIManager().UnDock(this);
				}
				else
				{
					((DockSite)(object)((Control)this).Parent).ၹ((Control)(object)this);
				}
			}
			((Control)this).Parent = null;
			((Control)ୱ).Controls.Add((Control)(object)this);
			if (!Visible)
			{
				((Control)this).Visible = true;
			}
			((Control)this).Location = new Point(0, 0);
			_୦ = new Rectangle(_୦.Location, _0BF9());
			Size = _୦.Size;
			DockOrientation = eOrientation.Horizontal;
			if (ଡ଼.LayoutType == eLayoutType.DockContainer && _0BA0)
			{
				ౘ(ౙ: true);
			}
			RecalcSize();
			_୦.Size = Size;
			((Form)ୱ).Location = p;
			if (ଡ଼.LayoutType != 0)
			{
				_0AC2 = new Point(((Control)this).Width / 2, 8);
			}
			else
			{
				_0AC2 = new Point(8, 8);
			}
			if (_0BA7)
			{
				((Control)ୱ).Visible = false;
			}
			else
			{
				((Control)ୱ).Show();
			}
			_୦.Location = ((Form)ୱ).Location;
			if (((Control)ୱ).Width != _୦.Width)
			{
				((Control)ୱ).Width = _୦.Width;
			}
			if (owner.ParentForm != null)
			{
				bool flag = true;
				if (ங && owner is DotNetBarManager && !((DotNetBarManager)owner).ActivateOnLayoutLoad)
				{
					flag = false;
				}
				if (flag)
				{
					owner.ParentForm.Activate();
				}
			}
			_0B72 = false;
			if (_0B48 != null)
			{
				_0B48(this, new EventArgs());
			}
			if (_0AC3 is IOwnerBarSupport ownerBarSupport2)
			{
				ownerBarSupport2.InvokeBarUndock(this, new EventArgs());
			}
			OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.BarUndocked));
			_0C65();
			return;
		}
		if ((pDockInfo.FullSizeDock || pDockInfo.PartialSizeDock) && pDockInfo.DockSiteZOrderIndex >= 0 && pDockInfo.objDockSite != null && ((Control)pDockInfo.objDockSite).Parent != null)
		{
			((Control)pDockInfo.objDockSite).Parent.Controls.SetChildIndex((Control)(object)pDockInfo.objDockSite, pDockInfo.DockSiteZOrderIndex);
		}
		if (_0AC3 != null)
		{
			if (pDockInfo.TabDockContainer != null)
			{
				if (ஓ != null && pDockInfo.TabDockContainer != ஓ)
				{
					_0BFA();
					ஓ.RecalcLayout();
					ஓ = null;
				}
				if (ஓ != pDockInfo.TabDockContainer)
				{
					_0B72 = true;
					if (_0B65 == -1)
					{
						if (ୱ != null && ((Control)ୱ).Visible)
						{
							if (owner.ParentForm != null)
							{
								owner.ParentForm.Activate();
							}
							_୦ = new Rectangle(((Form)ୱ).Location, Size);
							ୡ = pDockInfo.DockOffset;
							_0B62 = pDockInfo.DockLine;
							((Control)ୱ).Controls.Remove((Control)(object)this);
							((Control)ୱ).Hide();
							((Component)(object)ୱ).Dispose();
							ୱ = null;
						}
						else if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
						{
							((DockSite)(object)((Control)this).Parent).ၹ((Control)(object)this);
						}
						_0B5E = eBarState.Docked;
						if (!ங)
						{
							((Control)this).Visible = false;
						}
						foreach (BaseItem subItem in ଡ଼.SubItems)
						{
							if (subItem is DockContainerItem dockContainerItem)
							{
								DockContainerItem dockContainerItem2 = new DockContainerItem();
								dockContainerItem2.Displayed = false;
								dockContainerItem2.Text = subItem.Text;
								dockContainerItem2.Image = dockContainerItem.Image;
								dockContainerItem2.ImageIndex = dockContainerItem.ImageIndex;
								dockContainerItem2.Icon = dockContainerItem.Icon;
								dockContainerItem2.Tag = "systempdockitem";
								pDockInfo.TabDockContainer.Items.Add(dockContainerItem2);
							}
							ஓ = pDockInfo.TabDockContainer;
							ஓ.RecalcLayout();
							((Control)ஓ).Refresh();
						}
					}
					else
					{
						if (ଡ଼.SubItems[_0B65] is DockContainerItem dockContainerItem3)
						{
							DockContainerItem dockContainerItem4 = new DockContainerItem();
							dockContainerItem4.Displayed = false;
							dockContainerItem4.Text = dockContainerItem3.Text;
							dockContainerItem4.Image = dockContainerItem3.Image;
							dockContainerItem4.ImageIndex = dockContainerItem3.ImageIndex;
							dockContainerItem4.Icon = dockContainerItem3.Icon;
							dockContainerItem4.Tag = "systempdockitem";
							pDockInfo.TabDockContainer.Items.Add(dockContainerItem4);
						}
						ஓ = pDockInfo.TabDockContainer;
						ஓ.RecalcLayout();
						((Control)ஓ).Refresh();
					}
					_0B72 = false;
				}
			}
			else
			{
				if (ஓ != null)
				{
					if (pDockInfo.objDockSite == ((Control)ஓ).Parent)
					{
						return;
					}
					_0BFA();
					ஓ.RecalcLayout();
					ஓ = null;
				}
				if (!Visible && !ங)
				{
					Visible = true;
				}
				if (pDockInfo.LastRelativeDockToBar != null && pDockInfo.LastRelativeDockToBar.DockedSite != null && owner is DotNetBarManager)
				{
					DotNetBarManager dotNetBarManager = (DotNetBarManager)owner;
					if (pDockInfo.LastRelativeDockToBar.AutoHide)
					{
						if (pDockInfo.objDockSite != null)
						{
							pDockInfo.objDockSite.GetDocumentUIManager().Dock(this);
						}
						else
						{
							dotNetBarManager.Dock(this, eDockSide.Right);
						}
					}
					else if (!pDockInfo.LastRelativeDockToBar.Visible || pDockInfo.LastRelativeDockToBar.DockSide == eDockSide.None)
					{
						dotNetBarManager.Dock(this, _୬.LastDockSiteSide);
					}
					else if (pDockInfo.LastRelativeDockToBar.DockSide != 0 && _୬.LastDockSiteSide != pDockInfo.LastRelativeDockToBar.DockSide)
					{
						dotNetBarManager.Dock(this, _୬.LastDockSiteSide);
					}
					else
					{
						dotNetBarManager.Dock(this, pDockInfo.LastRelativeDockToBar, ଙ);
					}
					if (((Control)this).IsDisposed)
					{
						return;
					}
					_0B5E = eBarState.Docked;
					pDockInfo.LastRelativeDockToBar = null;
				}
				else if (pDockInfo.objDockSite != null && _0B5E != eBarState.Docked)
				{
					_0B72 = true;
					if (ୱ != null && _0B5E == eBarState.Floating)
					{
						if (owner.ParentForm != null)
						{
							owner.ParentForm.Activate();
						}
						_୦ = new Rectangle(((Form)ୱ).Location, Size);
						ୡ = pDockInfo.DockOffset;
						_0B62 = pDockInfo.DockLine;
						((Control)ୱ).Controls.Remove((Control)(object)this);
						((Control)ୱ).Hide();
						((Component)(object)ୱ).Dispose();
						ୱ = null;
					}
					_0B5E = eBarState.Docked;
					if (pDockInfo.objDockSite.ၓ || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						DockDocumentManager(pDockInfo);
					}
					else if (pDockInfo.InsertPosition == -10)
					{
						pDockInfo.objDockSite._105E((Control)(object)this);
					}
					else
					{
						pDockInfo.objDockSite._105E((Control)(object)this, pDockInfo.InsertPosition);
					}
					_0B72 = false;
				}
				else if (pDockInfo.objDockSite != null && pDockInfo.objDockSite != ((Control)this).Parent)
				{
					_0B72 = true;
					if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
					{
						((ContainerControl)owner.ParentForm).ActiveControl = null;
						((Control)this).Focus();
					}
					else if (owner.ParentForm != null && _0BF2((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
					{
						((ContainerControl)owner.ParentForm).ActiveControl = null;
						((Control)this).Focus();
					}
					if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
					{
						if (((DockSite)(object)((Control)this).Parent).ၓ || ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
						{
							((DockSite)(object)((Control)this).Parent).GetDocumentUIManager().UnDock(this);
						}
						else
						{
							((DockSite)(object)((Control)this).Parent).ၹ((Control)(object)this);
						}
					}
					if (!Visible && !ங)
					{
						Visible = true;
					}
					ୡ = pDockInfo.DockOffset;
					_0B62 = pDockInfo.DockLine;
					if (pDockInfo.objDockSite.ၓ || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
					}
					else if (pDockInfo.InsertPosition == -10)
					{
						pDockInfo.objDockSite._105E((Control)(object)this);
					}
					else
					{
						pDockInfo.objDockSite._105E((Control)(object)this, pDockInfo.InsertPosition);
					}
					_0B72 = false;
				}
				else if (((Control)this).Parent != null && pDockInfo.objDockSite == ((Control)this).Parent)
				{
					if (pDockInfo.objDockSite.ၓ || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
					}
					else if (_0B62 != pDockInfo.DockLine || (ୡ != pDockInfo.DockOffset && !Stretch) || ((Control)pDockInfo.objDockSite).Controls.GetChildIndex((Control)(object)this) != pDockInfo.InsertPosition || pDockInfo.NewLine)
					{
						_0B62 = pDockInfo.DockLine;
						ୡ = pDockInfo.DockOffset;
						if (!Visible && !ங)
						{
							Visible = true;
						}
						if (pDockInfo.NewLine)
						{
							pDockInfo.objDockSite.ၡ((Control)(object)this, pDockInfo.InsertPosition, _1063: true);
						}
						else if (pDockInfo.InsertPosition == -10)
						{
							pDockInfo.objDockSite._1064((Control)(object)this);
							pDockInfo.objDockSite.RecalcLayout();
						}
						else
						{
							pDockInfo.objDockSite.ၡ((Control)(object)this, pDockInfo.InsertPosition);
						}
					}
				}
				else
				{
					Point location = new Point(p.X - _0AC2.X, p.Y - _0AC2.Y);
					ঊ ঊ2 = BarFunctions.ॻ((Control)(object)ୱ);
					if (ঊ2 != null)
					{
						if (location.Y + 8 >= ঊ2.ঌ.Bottom)
						{
							location.Y = ঊ2.ঌ.Bottom - 8;
						}
						else if (location.Y <= ঊ2.ঌ.Y - 1 && କ && !IsMaximized && BarFunctions.ॹ(new Point(location.X, ঊ2.ঋ.Y - 32)) == null)
						{
							IsMaximized = true;
						}
						else if (IsMaximized && (Math.Abs(p.X - _0AC2.X) > 0 || Math.Abs(p.Y - _0AC2.Y) > 0) && location.Y > ঊ2.ঌ.Y + 8)
						{
							IsMaximized = false;
							_0AC2.X = ((Control)this).ClientSize.Width / 2;
							location = new Point(p.X - _0AC2.X, p.Y - _0AC2.Y);
						}
					}
					if (!IsMaximized)
					{
						((Form)ୱ).Location = location;
					}
					if (((IOwner)_0AC3).ParentForm != null)
					{
						((Control)((IOwner)_0AC3).ParentForm).Update();
					}
				}
			}
		}
		if (ଡ଼.LayoutType == eLayoutType.DockContainer && (ஈ == null || !((Control)ஈ).Visible))
		{
			int visibleSubItems = ଡ଼.VisibleSubItems;
			if ((_0B5E != eBarState.Floating || visibleSubItems > 1) && (_0BA0 || visibleSubItems > 0))
			{
				eBarState eBarState2 = _0B5E;
				_0B5E = eBarState.Docked;
				ౘ(ౙ: false);
				_0B5E = eBarState2;
			}
		}
		if (_0B5E != eBarState.Floating)
		{
			_0BF8();
		}
	}

	internal void _0BF8()
	{
		if (_0B47 != null)
		{
			_0B47(this, new EventArgs());
		}
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBarDock(this, new EventArgs());
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.BarDocked));
	}

	private Size _0BF9()
	{
		Size result = _୦.Size;
		if (result.IsEmpty)
		{
			if (ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				int selectedDockTab = SelectedDockTab;
				if (selectedDockTab >= 0 && ଡ଼.SubItems[selectedDockTab] is DockContainerItem)
				{
					DockContainerItem dockContainerItem = ଡ଼.SubItems[selectedDockTab] as DockContainerItem;
					result = Dpi.Size(dockContainerItem.DefaultFloatingSize);
				}
				else if (ଡ଼.SubItems.Count <= 0 || !(ଡ଼.SubItems[0] is DockContainerItem))
				{
					result = ((ଡ଼.Orientation != eOrientation.Vertical) ? new Size(ଡ଼.HeightInternal + 24, ଡ଼.HeightInternal + 24) : new Size(ଡ଼.WidthInternal + 16, ଡ଼.WidthInternal + 16));
				}
				else
				{
					DockContainerItem dockContainerItem2 = ଡ଼.SubItems[0] as DockContainerItem;
					result = Dpi.Size(dockContainerItem2.DefaultFloatingSize);
				}
			}
			else
			{
				result = Screen.FromControl((Control)(object)this).WorkingArea.Size;
			}
		}
		return result;
	}

	private void _0BFA()
	{
		if (ஓ == null)
		{
			return;
		}
		ArrayList arrayList = new ArrayList(ஓ.Items.Count);
		ஓ.Items.CopyTo(arrayList);
		foreach (BaseItem item in arrayList)
		{
			if (item.Tag != null && item.Tag.ToString() == "systempdockitem")
			{
				ஓ.Items.Remove(item);
			}
		}
	}

	internal void _0BFB()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		((Control)this).OnMouseMove(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
	}

	internal void _0BFC()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		((Control)this).OnMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
	}

	internal void _0BFD(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseMove(ؿ);
	}

	internal void _0BFE()
	{
		ఉ(ఊ: true, Point.Empty);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		ડ();
		((Control)this).OnFontChanged(e);
	}

	private void ડ()
	{
		if (ଡ଼ != null)
		{
			BarUtilities.ડ(ଡ଼.SubItems);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Invalid comparison between Unknown and I4
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Invalid comparison between Unknown and I4
		//IL_1fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff3: Invalid comparison between Unknown and I4
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Invalid comparison between Unknown and I4
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Invalid comparison between Unknown and I4
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Invalid comparison between Unknown and I4
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Invalid comparison between Unknown and I4
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Invalid comparison between Unknown and I4
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Invalid comparison between Unknown and I4
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Invalid comparison between Unknown and I4
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Invalid comparison between Unknown and I4
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Invalid comparison between Unknown and I4
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Invalid comparison between Unknown and I4
		//IL_1d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3c: Expected O, but got Unknown
		//IL_1f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f50: Expected O, but got Unknown
		if (ல)
		{
			return;
		}
		ல = true;
		if (_0B64 && _0B5E != 0 && (int)e.Button == 1048576 && DotNetBarManager.MouseDockingEnabled)
		{
			if (((Control)this).Cursor != Cursors.SizeAll)
			{
				((Control)this).Cursor = Cursors.SizeAll;
			}
			Point mousePosition = Control.MousePosition;
			Point point = ((Control)this).PointToClient(mousePosition);
			if (!(_0AC3 is IOwnerBarSupport ownerBarSupport) || (Math.Abs(point.X - _0AC2.X) <= 4 && Math.Abs(point.Y - _0AC2.Y) <= 4))
			{
				((Control)this).OnMouseMove(e);
				ல = false;
				return;
			}
			DockSiteInfo pDockInfo = ownerBarSupport.GetDockInfo(this, mousePosition.X, mousePosition.Y);
			if (pDockInfo.objDockSite == null && !_0B77)
			{
				((Control)this).OnMouseMove(e);
				ல = false;
				return;
			}
			bool flag = true;
			if (!ள.IsEmpty)
			{
				_089F.ब(ள, 3);
				ள = Rectangle.Empty;
			}
			if (pDockInfo.UseOutline && pDockInfo.objDockSite != null)
			{
				Rectangle barDockRectangle = pDockInfo.objDockSite.GetBarDockRectangle(this, ref pDockInfo);
				if (!barDockRectangle.IsEmpty)
				{
					flag = false;
					if (ழ == null)
					{
						ழ = CreateOutlineForm();
					}
					_089F.SetWindowPos(((Control)ழ).Handle, new IntPtr(0), barDockRectangle.X, barDockRectangle.Y, barDockRectangle.Width, barDockRectangle.Height, 80);
				}
			}
			if (flag && ழ != null)
			{
				((Control)ழ).Visible = false;
			}
			_୪ = pDockInfo;
			if (flag)
			{
				DockingHandler(pDockInfo, mousePosition);
			}
		}
		else if ((int)e.Button == 0)
		{
			if (_0B5E == eBarState.Floating)
			{
				if (ଡ଼.LayoutType == eLayoutType.DockContainer && ((e.X <= 2 && e.Y <= 2) || (e.X >= ((Control)this).Width - 4 && e.Y >= ((Control)this).Height - 4)))
				{
					((Control)this).Cursor = Cursors.SizeNWSE;
				}
				else if (ଡ଼.LayoutType == eLayoutType.DockContainer && ((e.X >= ((Control)this).Width - 2 && e.Y <= 2) || (e.X <= 4 && e.Y >= ((Control)this).Height - 4)))
				{
					((Control)this).Cursor = Cursors.SizeNESW;
				}
				else if ((e.X >= 0 && e.X <= 2) || (e.X >= ((Control)this).Width - 3 && e.X <= ((Control)this).Width))
				{
					((Control)this).Cursor = Cursors.SizeWE;
				}
				else if ((e.Y >= 0 && e.Y <= 2) || (e.Y >= ((Control)this).Height - 3 && e.Y <= ((Control)this).Height))
				{
					((Control)this).Cursor = Cursors.SizeNS;
				}
				else if (((Control)this).Cursor == Cursors.SizeWE || ((Control)this).Cursor == Cursors.SizeNS || ((Control)this).Cursor == Cursors.SizeNWSE || ((Control)this).Cursor == Cursors.SizeNESW)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			else if (_0B5E == eBarState.AutoHide && ଡ଼.LayoutType == eLayoutType.DockContainer && ଡ଼.Stretch)
			{
				if ((e.X <= 2 && (int)_୬.DockSide == 4) || (e.X >= ((Control)this).Width - 2 && (int)_୬.DockSide == 3))
				{
					((Control)this).Cursor = Cursors.VSplit;
				}
				else if ((e.Y <= 2 && (int)_୬.DockSide == 2) || (e.Y >= ((Control)this).Height - 2 && (int)_୬.DockSide == 1))
				{
					((Control)this).Cursor = Cursors.HSplit;
				}
				else if (e.X <= 2 && ((int)_୬.DockSide == 2 || (int)_୬.DockSide == 1) && ((Control)this).Left > 0)
				{
					((Control)this).Cursor = Cursors.VSplit;
				}
				else if (e.Y <= 2 && ((int)_୬.DockSide == 3 || (int)_୬.DockSide == 4) && ((Control)this).Top > 0)
				{
					((Control)this).Cursor = Cursors.HSplit;
				}
				else if (((Control)this).Cursor == Cursors.HSplit || ((Control)this).Cursor == Cursors.VSplit)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			else if (_0B5E == eBarState.Docked && ୟ == eGrabHandleStyle.ResizeHandle)
			{
				if ((e.X > Location.X + ((Control)this).Width - 17 && (int)((Control)this).RightToLeft == 0) || (e.X < Location.X + 17 && (int)((Control)this).RightToLeft == 1))
				{
					Form val = ((Control)this).FindForm();
					if (val != null && (int)val.WindowState == 2)
					{
						if (((Control)this).Cursor == Cursors.SizeNESW || ((Control)this).Cursor == Cursors.SizeNWSE)
						{
							((Control)this).Cursor = Cursors.Default;
						}
					}
					else if ((int)((Control)this).RightToLeft == 1)
					{
						((Control)this).Cursor = Cursors.SizeNESW;
					}
					else
					{
						((Control)this).Cursor = Cursors.SizeNWSE;
					}
				}
				else if (((Control)this).Cursor == Cursors.SizeNESW || ((Control)this).Cursor == Cursors.SizeNWSE)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			if (ଡ଼.EffectiveStyle == eDotNetBarStyle.OfficeXP || ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2003 || ଡ଼.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(ଡ଼.EffectiveStyle))
			{
				if (!_0B82._1B73.IsEmpty && _0B82._1B73.Contains(e.X, e.Y))
				{
					if (!_0B82._1B69)
					{
						_0B82._1B69 = true;
						_0BD7();
					}
				}
				else if (_0B82._1B69)
				{
					_0B82._1B69 = false;
					_0BD7();
				}
				if (!_0B82._1B77.IsEmpty && _0B82._1B77.Contains(e.X, e.Y))
				{
					if (!_0B82._1B6A)
					{
						_0B82._1B6A = true;
						_0BD6();
					}
				}
				else if (_0B82._1B6A)
				{
					_0B82._1B6A = false;
					_0BD6();
				}
				if (!_0B82._1B76.IsEmpty && _0B82._1B76.Contains(e.X, e.Y))
				{
					if (!_0B82._1B6B)
					{
						_0B82._1B6B = true;
						_0BD8();
					}
				}
				else if (_0B82._1B6B)
				{
					_0B82._1B6B = false;
					_0BD8();
				}
				if (!_0B82._1B74.IsEmpty && _0B82._1B74.Contains(e.X, e.Y))
				{
					if (!_0B82._1B6C)
					{
						_0B82._1B6C = true;
						_0BD9();
					}
				}
				else if (_0B82._1B6C)
				{
					_0B82._1B6C = false;
					_0BD9();
				}
				if (!_0B82._1B75.IsEmpty && _0B82._1B75.Contains(e.X, e.Y))
				{
					if (!_0B82._1B71)
					{
						_0B82._1B71 = true;
						_0BDA();
					}
				}
				else if (_0B82._1B71)
				{
					_0B82._1B71 = false;
					_0BDA();
				}
			}
		}
		else if ((int)e.Button == 1048576 && _୩ != 0 && _0B5E == eBarState.Floating)
		{
			Size clientSize = ((Control)this).ClientSize;
			Size empty = Size.Empty;
			Size size = MinimumDockSize(ଡ଼.Orientation);
			if (_୩ == 5 || _୩ == 6)
			{
				empty = ((_୩ != 6) ? _௧(new Size(((Control)this).Width - e.X, ((Control)this).Height - e.Y)) : _௧(new Size(e.X, e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					if (_୩ == 6)
					{
						((Control)this).ClientSize = empty;
					}
					else
					{
						((Form)ୱ).Location = new Point(((Control)ୱ).Left + (((Control)this).Width - empty.Width), ((Control)ୱ).Top + (((Control)this).Height - empty.Height));
						((Control)this).ClientSize = empty;
					}
					RecalcSize();
					((Control)this).Update();
					if (_0AC3 is IOwner { ParentForm: not null } owner)
					{
						((Control)owner.ParentForm).Update();
					}
				}
			}
			else if (_୩ == 7 || _୩ == 8)
			{
				empty = ((_୩ != 8) ? _௧(new Size(e.X, ((Control)this).Height - e.Y)) : _௧(new Size(((Control)this).Width - e.X, e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					if (_୩ == 8)
					{
						((Form)ୱ).Location = new Point(((Control)ୱ).Left + (((Control)this).Width - empty.Width), ((Control)ୱ).Top);
						((Control)this).ClientSize = empty;
					}
					else
					{
						((Form)ୱ).Location = new Point(((Control)ୱ).Left, ((Control)ୱ).Top + (((Control)this).Height - empty.Height));
						((Control)this).ClientSize = empty;
					}
					RecalcSize();
					((Control)this).Update();
					if (_0AC3 is IOwner { ParentForm: not null } owner2)
					{
						((Control)owner2.ParentForm).Update();
					}
				}
			}
			else if (_୩ == 2)
			{
				empty = _௧(new Size(e.X, ((Control)this).Height));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)this).Update();
					if (_0AC3 is IOwner { ParentForm: not null } owner3)
					{
						((Control)owner3.ParentForm).Update();
					}
				}
			}
			else if (_୩ == 1)
			{
				empty = _௧(new Size(((Control)this).Width - e.X, ((Control)this).Height));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					int right = ((Control)ୱ).Right;
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)ୱ).Left = ((Control)ୱ).Left + right - ((Control)ୱ).Right;
					((Control)this).Update();
					if (_0AC3 is IOwner { ParentForm: not null } owner4)
					{
						((Control)owner4.ParentForm).Update();
					}
				}
			}
			else if (_୩ == 4)
			{
				if (e.Y > 0)
				{
					empty = ((ଡ଼.LayoutType != eLayoutType.TaskList && ଡ଼.LayoutType != eLayoutType.DockContainer) ? _௧(new Size((int)((float)_0B63.Width * ((float)_0B63.Height / (float)e.Y)), ((Control)this).Height)) : _௧(new Size(((Control)this).Width, e.Y)));
					if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
					{
						((Control)this).ClientSize = empty;
						RecalcSize();
						((Control)this).Update();
						if (_0AC3 is IOwner { ParentForm: not null } owner5)
						{
							((Control)owner5.ParentForm).Update();
						}
					}
				}
			}
			else if (_୩ == 3 && e.Y != 0)
			{
				empty = ((ଡ଼.LayoutType != eLayoutType.TaskList && ଡ଼.LayoutType != eLayoutType.DockContainer) ? _௧(new Size((int)((float)_0B63.Width * ((float)_0B63.Height / (float)(((Control)this).Height - e.Y))), ((Control)this).Height)) : _௧(new Size(((Control)this).Width, ((Control)this).Height - e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					int bottom = ((Control)ୱ).Bottom;
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)ୱ).Top = ((Control)ୱ).Top + bottom - ((Control)ୱ).Bottom;
					((Control)this).Update();
					if (_0AC3 is IOwner { ParentForm: not null } owner6)
					{
						((Control)owner6.ParentForm).Update();
					}
				}
			}
			_୦ = new Rectangle(Location, Size);
		}
		else if (((int)e.Button == 1048576 && _୩ != 0 && _0B5E == eBarState.AutoHide) || (_୩ == 15 && _0B5E == eBarState.Docked))
		{
			Size clientSize2 = ((Control)this).ClientSize;
			Size empty2 = Size.Empty;
			if (_୩ == 15)
			{
				if ((int)((Control)this).RightToLeft == 0)
				{
					if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
					{
						Point point2 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
						if (((Control)this).Parent.Parent.Parent != null)
						{
							Point point3 = ((Control)this).Parent.Parent.Parent.PointToScreen(((Control)this).Parent.Parent.Location);
							((Control)this).Parent.Parent.Size = new Size(point2.X - point3.X + _0B96.X, point2.Y - point3.Y + _0B96.Y);
						}
						else
						{
							((Control)this).Parent.Parent.Size = new Size(point2.X - ((Control)this).Parent.Parent.Location.X + _0B96.X, point2.Y - ((Control)this).Parent.Parent.Location.Y + _0B96.Y);
						}
						if (_0AC3 is IOwner { ParentForm: not null } owner7)
						{
							((Control)owner7.ParentForm).Update();
						}
					}
					else if (_0AC3 == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is ᆡ))
					{
						Form val2 = ((Control)this).FindForm();
						if (val2 != null)
						{
							Point point4 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
							val2.Size = new Size(point4.X - val2.Location.X + _0B96.X, point4.Y - val2.Location.Y + _0B96.Y);
							((Control)val2).Update();
						}
					}
				}
				else if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					Point point5 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
					if (((Control)this).Parent.Parent.Parent != null)
					{
						Point point6 = ((Control)this).Parent.Parent.Parent.PointToScreen(((Control)this).Parent.Parent.Location);
						Rectangle bounds = ((Control)this).Parent.Parent.Bounds;
						bounds.X = point5.X - _0B96.X;
						bounds.Width += ((Control)this).Parent.Parent.Left - bounds.X;
						bounds.Height = point5.Y - point6.Y + _0B96.Y;
						((Control)this).Parent.Parent.Bounds = bounds;
					}
					else
					{
						Rectangle bounds2 = ((Control)this).Parent.Parent.Bounds;
						bounds2.X = point5.X - _0B96.X;
						bounds2.Width += ((Control)this).Parent.Parent.Left - bounds2.X;
						bounds2.Height = point5.Y - ((Control)this).Parent.Parent.Location.Y + _0B96.Y;
						((Control)this).Parent.Parent.Bounds = bounds2;
					}
					if (_0AC3 is IOwner { ParentForm: not null } owner8)
					{
						((Control)owner8.ParentForm).Update();
					}
				}
				else if (_0AC3 == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is ᆡ))
				{
					Form val3 = ((Control)this).FindForm();
					if (val3 != null)
					{
						Point point7 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
						Rectangle bounds3 = ((Control)val3).Bounds;
						bounds3.X = point7.X - _0B96.X;
						bounds3.Width += ((Control)val3).Left - bounds3.X;
						bounds3.Height = point7.Y - val3.Location.Y + _0B96.Y;
						((Control)val3).Bounds = bounds3;
						((Control)val3).Update();
					}
				}
			}
			else if (_୩ == 9)
			{
				int num = _0C02();
				int num2 = e.X;
				int num3 = 32;
				if (_0AC3 is DotNetBarManager)
				{
					num3 = ((DotNetBarManager)_0AC3).MinimumClientSize.Width;
				}
				if (num3 > 0 && num + num2 < num3)
				{
					num2 = num3 - num;
				}
				if (num + num2 >= num3 || num3 == 0)
				{
					int ර = ଡ଼.ර;
					ଡ଼.ර = 0;
					empty2 = _௧(new Size(((Control)this).Width - num2, ((Control)this).Height));
					ଡ଼.ර = ර;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (_0B5E == eBarState.AutoHide)
						{
							Rectangle bounds4 = ((Control)this).Bounds;
							ૹ = false;
							try
							{
								ଡ଼.ර = ଡ଼.WidthInternal - num2;
								((Control)this).Width = ((Control)this).Width - num2;
								RecalcSize();
								((Control)this).Left = ((Control)this).Left + (clientSize2.Width - Size.Width);
							}
							finally
							{
								ૹ = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds4, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int ර2 = ଡ଼.ර;
							ଡ଼.ර = ଡ଼.WidthInternal - num2;
							if (ଡ଼.ර < ර2)
							{
								_0C04();
							}
							RecalcLayout();
						}
						if (_0B5E == eBarState.AutoHide)
						{
							_୬.DockedWidth = ((Control)this).Width;
						}
					}
				}
			}
			else if (_୩ == 10)
			{
				int num4 = _0C02();
				int num5 = e.X;
				int num6 = 32;
				if (_0AC3 is DotNetBarManager)
				{
					num6 = ((DotNetBarManager)_0AC3).MinimumClientSize.Width;
				}
				if (num6 > 0 && num4 - (num5 - ((Control)this).Width) < num6)
				{
					num5 = num4 + ((Control)this).Width - num6;
				}
				if (num4 - (num5 - ((Control)this).Width) >= num6 || num6 == 0)
				{
					int ර3 = ଡ଼.ර;
					ଡ଼.ර = 0;
					empty2 = _௧(new Size(num5, ((Control)this).Height));
					ଡ଼.ර = ර3;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (_0B5E == eBarState.AutoHide)
						{
							Rectangle bounds5 = ((Control)this).Bounds;
							ૹ = false;
							try
							{
								ଡ଼.ර = ଡ଼.WidthInternal + (num5 - ((Control)this).Width);
								((Control)this).Width = ((Control)this).Width + (num5 - ((Control)this).Width);
								RecalcSize();
							}
							finally
							{
								ૹ = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds5, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int ර4 = ଡ଼.ර;
							ଡ଼.ර = ଡ଼.WidthInternal + (num5 - ((Control)this).Width);
							if (ଡ଼.ර < ර4)
							{
								_0C04();
							}
							RecalcLayout();
						}
						if (_0B5E == eBarState.AutoHide)
						{
							_୬.DockedWidth = ((Control)this).Width;
						}
					}
				}
			}
			else if (_୩ == 12)
			{
				int num7 = _0C03();
				int num8 = e.Y;
				int num9 = 32;
				if (_0AC3 is DotNetBarManager)
				{
					num9 = ((DotNetBarManager)_0AC3).MinimumClientSize.Height;
				}
				if (num9 > 0 && num7 + num8 < num9)
				{
					num8 = num9 - num7;
				}
				if (num7 + num8 >= num9 || num9 == 0)
				{
					int num10 = ଡ଼._0B03;
					ଡ଼._0B03 = 0;
					empty2 = _௧(new Size(((Control)this).Width, ((Control)this).Height - num8));
					ଡ଼._0B03 = num10;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (_0B5E == eBarState.AutoHide)
						{
							Rectangle bounds6 = ((Control)this).Bounds;
							ૹ = false;
							try
							{
								ଡ଼._0B03 = ଡ଼.HeightInternal - num8;
								((Control)this).Height = ((Control)this).Height - num8;
								RecalcSize();
								((Control)this).Top = ((Control)this).Top + (clientSize2.Height - ((Control)this).Height);
							}
							finally
							{
								ૹ = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds6, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int num11 = ଡ଼._0B03;
							Size size2 = MinimumDockSize(ଡ଼.Orientation);
							if (ଡ଼.HeightInternal - num8 >= size2.Height)
							{
								ଡ଼._0B03 = ଡ଼.HeightInternal - num8;
							}
							if (ଡ଼._0B03 < num11)
							{
								అ();
							}
							RecalcLayout();
						}
						if (_0B5E == eBarState.AutoHide)
						{
							_୬.DockedHeight = ((Control)this).Height;
						}
					}
				}
			}
			else if (_୩ == 11)
			{
				int num12 = _0C03();
				int num13 = e.Y;
				int num14 = 32;
				if (_0AC3 is DotNetBarManager)
				{
					num14 = ((DotNetBarManager)_0AC3).MinimumClientSize.Height;
				}
				if (num14 > 0 && num12 - (num13 - ((Control)this).Height) < num14)
				{
					num13 = num12 + ((Control)this).Height - num14;
				}
				if (num12 - (num13 - ((Control)this).Height) >= num14 || num14 == 0)
				{
					int num15 = ଡ଼._0B03;
					ଡ଼._0B03 = 0;
					empty2 = _௧(new Size(((Control)this).Width, num13));
					ଡ଼._0B03 = num15;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (_0B5E == eBarState.AutoHide)
						{
							Rectangle bounds7 = ((Control)this).Bounds;
							ૹ = false;
							try
							{
								ଡ଼._0B03 = ଡ଼.HeightInternal + (num13 - ((Control)this).Height);
								((Control)this).Height = num13;
								RecalcSize();
							}
							finally
							{
								ૹ = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds7, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int num16 = ଡ଼._0B03;
							ଡ଼._0B03 = ଡ଼.HeightInternal + (num13 - ((Control)this).Height);
							if (ଡ଼._0B03 < num16)
							{
								అ();
							}
							RecalcLayout();
						}
						if (_0B5E == eBarState.AutoHide)
						{
							_୬.DockedHeight = ((Control)this).Height;
						}
					}
				}
			}
			else if (_୩ == 13 && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) > 0)
			{
				Bar bar = _0BFF(this);
				if (bar != null && bar.DockLine == DockLine)
				{
					Size size3 = _౮(bar.MinimumDockSize(eOrientation.Horizontal));
					Size size4 = _౮(MinimumDockSize(eOrientation.Horizontal));
					int num17 = e.X;
					if (((Control)bar).Width + num17 < size3.Width)
					{
						num17 += size3.Width - (((Control)bar).Width + num17) - 1;
					}
					if (((Control)this).Width - num17 < size4.Width)
					{
						num17 -= size4.Width - (((Control)this).Width - num17) + 1;
					}
					if (((Control)bar).Width + num17 >= size3.Width && ((Control)this).Width - num17 >= size4.Width)
					{
						Size size5 = bar.Size;
						Size size6 = bar._௧(new Size(((Control)bar).Width + num17, ((Control)this).Height));
						empty2 = _௧(new Size(((Control)this).Width - num17, ((Control)this).Height));
						if (!size5.Equals((object?)size6) && !clientSize2.Equals((object?)empty2))
						{
							_0B02 = 0;
							bar._0B02 = ((Control)bar).Width + num17;
							foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
							{
								Control val4 = item;
								if (val4 != this && val4 != bar && val4.Visible && val4 is Bar)
								{
									Bar bar2 = val4 as Bar;
									if (bar2.DockLine == DockLine && bar2._0B01 == 0)
									{
										bar2._0B02 = ((Control)bar2).Width;
									}
								}
							}
							RecalcLayout();
						}
					}
				}
			}
			else if (_୩ == 14 && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) > 0)
			{
				Bar bar3 = _0BFF(this);
				if (bar3 != null && bar3.DockLine == DockLine)
				{
					Size size7 = _౮(bar3.MinimumDockSize(eOrientation.Horizontal));
					Size size8 = _౮(MinimumDockSize(eOrientation.Horizontal));
					int num18 = e.Y;
					if (((Control)bar3).Height + num18 < size7.Height)
					{
						num18 += size7.Height - (((Control)bar3).Height + num18) - 1;
					}
					if (((Control)this).Height - num18 < size8.Height)
					{
						num18 -= size8.Height - (((Control)this).Height - num18) + 1;
					}
					if (((Control)bar3).Height + num18 >= size7.Height && ((Control)this).Height - num18 >= size8.Height)
					{
						Size size9 = bar3.Size;
						Size size10 = bar3._௧(new Size(((Control)bar3).Width, ((Control)this).Height + num18));
						empty2 = _௧(new Size(((Control)this).Width, ((Control)this).Height - num18));
						if (!size9.Equals((object?)size10) && !clientSize2.Equals((object?)empty2))
						{
							_0B01 = 0;
							bar3._0B01 = ((Control)bar3).Height + num18;
							foreach (Control item2 in (ArrangedElementCollection)((Control)this).Parent.Controls)
							{
								Control val5 = item2;
								if (val5 != this && val5 != bar3 && val5.Visible && val5 is Bar)
								{
									Bar bar4 = val5 as Bar;
									if (bar4.DockLine == DockLine && bar4._0B01 == 0)
									{
										bar4._0B01 = ((Control)bar4).Height;
									}
								}
							}
							RecalcLayout();
						}
					}
				}
			}
		}
		if (_0B5E == eBarState.Popup && _0AF1 != null && _0AF1.DesignMode && (int)e.Button == 1048576 && (Math.Abs(e.X - _0AC2.X) >= 2 || Math.Abs(e.Y - _0AC2.Y) >= 2 || _0BCB))
		{
			BaseItem baseItem = _09D9;
			if (_0AC3 is IOwner)
			{
				baseItem = ((IOwner)_0AC3).GetFocusItem();
			}
			ISite site = ಌ();
			if (site != null && baseItem != null)
			{
				_0C8D(e);
			}
		}
		((Control)this).OnMouseMove(e);
		if (ଡ଼.SubItems.Count == 0)
		{
			ல = false;
			return;
		}
		if (!_0B64 && _୩ == 0)
		{
			ଡ଼.InternalMouseMove(e);
		}
		ல = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Form CreateOutlineForm()
	{
		return BarFunctions.CreateOutlineForm();
	}

	private Bar _0BFF(Bar _0C00)
	{
		if (((Control)this).Parent == null)
		{
			return null;
		}
		int num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this) - 1;
		if (num >= 0)
		{
			for (int num2 = num; num2 >= 0; num2--)
			{
				if (((Control)this).Parent.Controls[num2].Visible)
				{
					return ((Control)this).Parent.Controls[num2] as Bar;
				}
			}
		}
		return null;
	}

	private Bar _0C01(Bar _0C00)
	{
		if (((Control)this).Parent == null)
		{
			return null;
		}
		int num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this) + 1;
		if (num >= 0)
		{
			for (int i = num; i < ((ArrangedElementCollection)((Control)this).Parent.Controls).Count; i++)
			{
				if (((Control)this).Parent.Controls[i].Visible)
				{
					return ((Control)this).Parent.Controls[i] as Bar;
				}
			}
		}
		return null;
	}

	private int _0C02()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		if (!(_0AC3 is IOwner owner) || ((Control)this).Parent == null || (((Control)this).Parent.Parent == null && _0B5E != eBarState.AutoHide))
		{
			return 0;
		}
		Control val = ((Control)this).Parent.Parent;
		if (_0B5E == eBarState.AutoHide)
		{
			val = (Control)(object)owner.ParentForm;
		}
		if (val == null && owner.ParentForm == null && _0AC3 is DotNetBarManager)
		{
			val = ((DotNetBarManager)_0AC3).ParentUserControl;
		}
		if (val == null)
		{
			return 0;
		}
		int num = val.ClientSize.Width;
		foreach (Control item in (ArrangedElementCollection)val.Controls)
		{
			Control val2 = item;
			if (val2.Visible && ((int)val2.Dock == 3 || (int)val2.Dock == 4))
			{
				num -= val2.Width;
			}
		}
		if (_0B5E == eBarState.AutoHide && Visible)
		{
			num -= ((Control)this).Width;
		}
		return num;
	}

	private int _0C03()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Invalid comparison between Unknown and I4
		if (!(_0AC3 is IOwner owner) || ((Control)this).Parent == null || (((Control)this).Parent.Parent == null && _0B5E != eBarState.AutoHide))
		{
			return 0;
		}
		Control val = null;
		val = (Control)((_0B5E != eBarState.AutoHide) ? ((object)((Control)this).Parent.Parent) : ((object)owner.ParentForm));
		if (val == null && owner.ParentForm == null && _0AC3 is DotNetBarManager)
		{
			val = ((DotNetBarManager)_0AC3).ParentUserControl;
		}
		if (val == null)
		{
			return 0;
		}
		int num = val.ClientSize.Height;
		foreach (Control item in (ArrangedElementCollection)val.Controls)
		{
			Control val2 = item;
			if (val2.Visible && ((int)val2.Dock == 1 || (int)val2.Dock == 2))
			{
				num -= val2.Height;
			}
		}
		if (_0B5E == eBarState.AutoHide && Visible)
		{
			num -= ((Control)this).Height;
		}
		return num;
	}

	internal void _0C04()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is Bar bar && bar != this && bar.DockLine == _0B62)
			{
				bar.ItemsContainer.ර = ଡ଼.ර;
			}
		}
	}

	internal void అ()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is Bar bar && bar != this && bar.DockLine == _0B62)
			{
				bar.ItemsContainer._0B03 = ଡ଼._0B03;
			}
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		ఆ();
	}

	internal void ఆ()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (_0B64)
		{
			return;
		}
		if ((int)Control.MouseButtons == 1048576)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (!((Control)this).ClientRectangle.Contains(pt) && Owner is IOwnerMenuSupport ownerMenuSupport && ownerMenuSupport.RelayMouseHover())
			{
				return;
			}
		}
		ଡ଼.InternalMouseHover();
		if (_0B82._1B71)
		{
			using (LocalizationManager localizationManager = new LocalizationManager(_0AC3 as IOwnerLocalize))
			{
				string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.BarAutoHideButtonTooltip);
				if (localizedString != "")
				{
					_0C4E(localizedString);
				}
				return;
			}
		}
		if (_0B82._1B6C)
		{
			using (LocalizationManager localizationManager2 = new LocalizationManager(_0AC3 as IOwnerLocalize))
			{
				string localizedString2 = localizationManager2.GetLocalizedString(LocalizationKeys.BarCustomizeButtonTooltip);
				if (localizedString2 != "")
				{
					_0C4E(localizedString2);
				}
				return;
			}
		}
		if (_0B82._1B69)
		{
			using (LocalizationManager localizationManager3 = new LocalizationManager(_0AC3 as IOwnerLocalize))
			{
				string localizedString3 = localizationManager3.GetLocalizedString(LocalizationKeys.BarCloseButtonTooltip);
				if (localizedString3 != "")
				{
					_0C4E(localizedString3);
				}
				return;
			}
		}
		if (!_0B82._1B6A)
		{
			return;
		}
		using LocalizationManager localizationManager4 = new LocalizationManager(_0AC3 as IOwnerLocalize);
		string localizedString4 = localizationManager4.GetLocalizedString(LocalizationKeys.BarMaximizeButtonTooltip);
		if (!string.IsNullOrEmpty(localizedString4))
		{
			_0C4E(localizedString4);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		if (_0B82._1B69)
		{
			_0B82._1B69 = false;
			_0BD7();
		}
		if (_0B82._1B6A)
		{
			_0B82._1B6A = false;
			_0BD6();
		}
		if (_0B82._1B6B)
		{
			_0B82._1B6B = false;
			_0BD8();
		}
		if (_0B82._1B6C)
		{
			_0B82._1B6C = false;
			_0BD9();
		}
		if (_0B82._1B71)
		{
			_0B82._1B71 = false;
			_0BDA();
		}
		if (!_0B64)
		{
			ଡ଼.InternalMouseLeave();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Invalid comparison between Unknown and I4
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Invalid comparison between Unknown and I4
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Invalid comparison between Unknown and I4
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Invalid comparison between Unknown and I4
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Invalid comparison between Unknown and I4
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Invalid comparison between Unknown and I4
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Invalid comparison between Unknown and I4
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Invalid comparison between Unknown and I4
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Invalid comparison between Unknown and I4
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Invalid comparison between Unknown and I4
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Invalid comparison between Unknown and I4
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Invalid comparison between Unknown and I4
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Invalid comparison between Unknown and I4
		//IL_0857: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Invalid comparison between Unknown and I4
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Invalid comparison between Unknown and I4
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Invalid comparison between Unknown and I4
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Invalid comparison between Unknown and I4
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Invalid comparison between Unknown and I4
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Invalid comparison between Unknown and I4
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Invalid comparison between Unknown and I4
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Invalid comparison between Unknown and I4
		_0AC2 = new Point(e.X, e.Y);
		_0B63 = Size;
		_0C4B();
		if ((int)e.Button == 1048576 && !_0B82._1B74.IsEmpty && _0B82._1B74.Contains(_0AC2) && !((Component)this).DesignMode && !ଡ଼.DesignMode)
		{
			if (இ != null)
			{
				if (இ.GetOwner() == null)
				{
					இ.SetOwner(_0AC3);
				}
				Point point = ((ଡ଼.EffectiveStyle != eDotNetBarStyle.Office2000) ? new Point(_0B82._1B74.Left, _0B82._1B74.Bottom) : new Point(0, 16));
				if (point.X < 0)
				{
					point.X = 0;
				}
				point = ((Control)this).PointToScreen(point);
				இ.SetSourceControl((Control)(object)this);
				இ.Popup(point);
				((Control)this).OnMouseDown(e);
				return;
			}
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				if (subItem is CustomizeItem && !subItem.Expanded)
				{
					if (ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						((CustomizeItem)subItem).PopupLocation = new Point(((Control)this).Left - 128, ((Control)this).Top + 16);
					}
					else
					{
						((CustomizeItem)subItem).PopupLocation = new Point(_0B82._1B74.Left, _0B82._1B74.Bottom);
					}
					subItem.Expanded = true;
					((Control)this).OnMouseDown(e);
					return;
				}
			}
		}
		else
		{
			if ((int)e.Button == 1048576 && !_0B82._1B73.IsEmpty && _0B82._1B73.Contains(e.X, e.Y))
			{
				_0B82._1B6D = true;
				_0BD7();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && !_0B82._1B77.IsEmpty && _0B82._1B77.Contains(e.X, e.Y))
			{
				_0B82._1B6E = true;
				_0BD6();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && !_0B82._1B76.IsEmpty && _0B82._1B76.Contains(e.X, e.Y))
			{
				_0B82._1B6F = true;
				_0BD8();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && !_0B82._1B75.IsEmpty && _0B82._1B75.Contains(e.X, e.Y))
			{
				_0B82._1B72 = true;
				_0BDA();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && _0B5E == eBarState.Floating)
			{
				if (ଡ଼.LayoutType == eLayoutType.DockContainer && e.X <= 4 && e.Y <= 4)
				{
					_୩ = 5;
				}
				else if (ଡ଼.LayoutType == eLayoutType.DockContainer && e.X >= ((Control)this).Width - 4 && e.Y >= ((Control)this).Height - 4)
				{
					_୩ = 6;
				}
				else if (ଡ଼.LayoutType == eLayoutType.DockContainer && e.X >= ((Control)this).Width - 4 && e.Y <= 4)
				{
					_୩ = 7;
				}
				else if (ଡ଼.LayoutType == eLayoutType.DockContainer && e.X <= 4 && e.Y >= ((Control)this).Height - 4)
				{
					_୩ = 8;
				}
				else if (e.X >= 0 && e.X <= 2)
				{
					_୩ = 1;
				}
				else if (e.X >= ((Control)this).Width - 3 && e.X <= ((Control)this).Width)
				{
					_୩ = 2;
				}
				else if (e.Y >= 0 && e.Y <= 2)
				{
					_୩ = 3;
				}
				else if (e.Y >= ((Control)this).Height - 3 && e.Y <= ((Control)this).Height)
				{
					_୩ = 4;
				}
			}
			else if ((int)e.Button == 1048576 && _0B5E == eBarState.AutoHide && ଡ଼.LayoutType == eLayoutType.DockContainer && ଡ଼.Stretch)
			{
				if ((int)_୬.DockSide == 4 && e.X <= 2)
				{
					_୩ = 9;
				}
				else if ((int)_୬.DockSide == 3 && e.X >= ((Control)this).Width - 2)
				{
					_୩ = 10;
				}
				else if ((int)_୬.DockSide == 2 && e.Y <= 2)
				{
					_୩ = 12;
				}
				else if ((int)_୬.DockSide == 1 && e.Y >= ((Control)this).Height - 2)
				{
					_୩ = 11;
				}
				else if (((int)_୬.DockSide == 2 || (int)_୬.DockSide == 1) && e.X <= 2 && ((Control)this).Left > 0)
				{
					_୩ = 13;
				}
				else if (((int)_୬.DockSide == 3 || (int)_୬.DockSide == 4) && e.Y <= 2 && ((Control)this).Top > 0)
				{
					_୩ = 14;
				}
			}
			else if ((int)e.Button == 2097152 && CanCustomize && !ଡ଼.DesignMode && _0B5E != 0 && !_0B64 && _0AC3 is DotNetBarManager && _0AC3 is IOwnerBarSupport ownerBarSupport)
			{
				ownerBarSupport.BarContextMenu((Control)(object)this, e);
			}
		}
		if ((int)e.Button == 1048576 && _0B5E == eBarState.Docked && ୟ == eGrabHandleStyle.ResizeHandle && _0AC2.X > Location.X + ((Control)this).Width - 17 && (int)((Control)this).RightToLeft == 0)
		{
			Form val = ((Control)this).FindForm();
			if ((val != null && (int)val.WindowState != 2) || val == null)
			{
				((Control)this).Capture = true;
				((Control)this).Cursor = Cursors.SizeNWSE;
				_୩ = 15;
				Point point2 = ((Control)this).PointToScreen(_0AC2);
				if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					point2 = ((Control)this).Parent.Parent.PointToClient(point2);
					_0B96 = new Point(((Control)this).Parent.Parent.Width - point2.X, ((Control)this).Parent.Parent.ClientRectangle.Height - point2.Y);
				}
				else if (_0AC3 == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is ᆡ) && val != null)
				{
					point2 = ((Control)val).PointToClient(point2);
					_0B96 = new Point(((Control)val).Width - point2.X, ((Control)val).ClientRectangle.Height - point2.Y);
				}
			}
		}
		else if ((int)e.Button == 1048576 && _0B5E == eBarState.Docked && ୟ == eGrabHandleStyle.ResizeHandle && _0AC2.X < Location.X + 17 && (int)((Control)this).RightToLeft == 1)
		{
			Form val2 = ((Control)this).FindForm();
			if ((val2 != null && (int)val2.WindowState != 2) || val2 == null)
			{
				((Control)this).Capture = true;
				((Control)this).Cursor = Cursors.SizeNESW;
				_୩ = 15;
				Point point3 = _0AC2;
				if ((object)((object)val2).GetType().GetProperty("RightToLeftLayout") != null && (bool)TypeDescriptor.GetProperties(val2)["RightToLeftLayout"].GetValue(val2))
				{
					point3.X = ((Control)this).Width - point3.X;
				}
				Point point4 = ((Control)this).PointToScreen(point3);
				if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					point4 = ((Control)this).Parent.Parent.PointToClient(point4);
					_0B96 = new Point(point4.X, ((Control)this).Parent.Parent.ClientRectangle.Height - point4.Y);
				}
				else if (_0AC3 == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is ᆡ) && val2 != null)
				{
					point4 = ((Control)val2).PointToClient(point4);
					_0B96 = new Point(point4.X, ((Control)val2).ClientRectangle.Height - point4.Y);
				}
			}
		}
		if ((int)e.Button == 1048576 && __0BC3 && !ୠ.IsEmpty && ୠ.Contains(_0AC2) && _୩ == 0 && !உ && (இ == null || !இ.Expanded))
		{
			((Control)this).Cursor = Cursors.SizeAll;
			((Control)this).Capture = true;
			_0B64 = true;
		}
		if (_0B5E == eBarState.Popup && _0AF1 != null && _0AF1.DesignMode)
		{
			ಏ(e);
		}
		else
		{
			ଡ଼.InternalMouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	internal void ఇ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	internal void ఈ()
	{
		BarClosingEventArgs barClosingEventArgs = new BarClosingEventArgs();
		ఖ(barClosingEventArgs);
		if (!barClosingEventArgs.Cancel)
		{
			if (AutoHide)
			{
				AutoHide = false;
			}
			స();
			IOwner owner = Owner as IOwner;
			if (owner.ParentForm != null && !((Control)owner.ParentForm).ContainsFocus)
			{
				((Control)owner.ParentForm).Focus();
			}
			ఒ();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Invalid comparison between Unknown and I4
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Invalid comparison between Unknown and I4
		if (_0B64 || _୩ != 0)
		{
			((Control)this).Cursor = Cursors.Default;
		}
		if (_0BCB)
		{
			ಎ(e);
		}
		if (_0B64)
		{
			ఉ(ఊ: false, new Point(e.X, e.Y));
		}
		else if ((int)e.Button == 1048576 && !_0B82._1B73.IsEmpty && _0B82._1B73.Contains(_0AC2))
		{
			_0B82._1B6D = false;
			_0BD7();
			if (_0B82._1B73.Contains(e.X, e.Y))
			{
				if (_0B7D && SelectedDockContainerItem != null)
				{
					CloseDockTab(SelectedDockContainerItem);
				}
				else
				{
					ఈ();
				}
			}
		}
		else if ((int)e.Button == 1048576 && !_0B82._1B77.IsEmpty && _0B82._1B77.Contains(_0AC2))
		{
			_0B82._1B6E = false;
			if (_0B82._1B77.Contains(e.X, e.Y) && ((Control)this).Parent is ᆡ)
			{
				IsMaximized = !IsMaximized;
			}
			_0BD6();
		}
		else if ((int)e.Button == 1048576 && !_0B82._1B76.IsEmpty && _0B82._1B76.Contains(_0AC2))
		{
			_0B82._1B6F = false;
			_0BD8();
			if (_0B82._1B76.Contains(e.X, e.Y))
			{
				InvokeCaptionButtonClick();
				if (_0BA2)
				{
					_0C5E();
					_0BD8();
				}
			}
		}
		else if ((int)e.Button == 1048576 && !_0B82._1B75.IsEmpty && _0B82._1B75.Contains(_0AC2))
		{
			_0B82._1B72 = false;
			_0BDA();
			if (_0B82._1B75.Contains(e.X, e.Y))
			{
				ஜ = false;
				AutoHide = !AutoHide;
			}
		}
		_0B64 = false;
		_୩ = 0;
		if (ଡ଼ != null)
		{
			ଡ଼.InternalMouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected virtual void OnIsMaximizedChanged(bool oldValue, bool newValue)
	{
		_ = (ᆡ)(object)((Control)this).Parent;
		if (!newValue)
		{
			((Control)this).ClientSize = _ஶ.Size;
			Location = _ஶ.Location;
		}
		else
		{
			_ஶ = new Rectangle(((Form)ୱ).Location, ((Control)this).ClientSize);
			ঊ ঊ2 = BarFunctions.ॻ((Control)(object)ୱ);
			if (ঊ2 != null)
			{
				((Control)this).ClientSize = ঊ2.ঌ.Size;
				Location = ঊ2.ঌ.Location;
			}
		}
		RecalcSize();
		((Control)this).Update();
	}

	private void ఉ(bool ఊ, Point ఋ)
	{
		if (((Control)this).Capture)
		{
			((Control)this).Capture = false;
		}
		((Control)this).Cursor = Cursors.Default;
		if (Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.DockComplete();
		}
		if (_୪.IsEmpty())
		{
			_0B64 = false;
			return;
		}
		if (ஓ != null || _୪.TabDockContainer != null)
		{
			ఌ();
			Bar tabDockContainer = ஓ;
			if (tabDockContainer == null)
			{
				tabDockContainer = _୪.TabDockContainer;
			}
			_0BFA();
			if (_୪.TabDockContainer != this)
			{
				if (_0B65 == -1)
				{
					ArrayList arrayList = new ArrayList(ଡ଼.SubItems.Count);
					ଡ଼.SubItems.CopyTo(arrayList);
					DockContainerItem dockContainerItem = null;
					Form val = ((Control)tabDockContainer).FindForm();
					if (val != null)
					{
						((ContainerControl)val).ActiveControl = (Control)(object)tabDockContainer;
					}
					foreach (BaseItem item in arrayList)
					{
						if (item is DockContainerItem dockContainerItem2)
						{
							if (dockContainerItem == null)
							{
								dockContainerItem = dockContainerItem2;
							}
							dockContainerItem2.Displayed = false;
							if (dockContainerItem2.OriginalBarName == "")
							{
								dockContainerItem2.OriginalBarName = Name;
								dockContainerItem2.OriginalPosition = ଡ଼.SubItems.IndexOf(dockContainerItem2);
							}
							ଡ଼.SubItems.Remove(dockContainerItem2);
							tabDockContainer.Items.Add(dockContainerItem2);
						}
					}
					tabDockContainer.RecalcLayout();
					if (dockContainerItem != null)
					{
						tabDockContainer.SelectedDockContainerItem = dockContainerItem;
					}
					tabDockContainer._0BF8();
					if (_0AC3 is DotNetBarManager dotNetBarManager)
					{
						_0B64 = false;
						tabDockContainer = null;
						if (CustomBar)
						{
							dotNetBarManager.Bars.Remove(this);
							((Component)this).Dispose();
						}
						else
						{
							Visible = false;
						}
						if (dotNetBarManager.ParentForm != null)
						{
							dotNetBarManager.ParentForm.Activate();
						}
						return;
					}
				}
				else
				{
					Form val2 = ((Control)tabDockContainer).FindForm();
					if (val2 != null)
					{
						((ContainerControl)val2).ActiveControl = (Control)(object)tabDockContainer;
					}
					DockContainerItem dockContainerItem3 = ଡ଼.SubItems[_0B65] as DockContainerItem;
					dockContainerItem3.Displayed = false;
					ଡ଼.SubItems.Remove(dockContainerItem3);
					tabDockContainer.Items.Add(dockContainerItem3);
					tabDockContainer.SelectedDockContainerItem = dockContainerItem3;
					tabDockContainer.RecalcLayout();
					tabDockContainer._0BF8();
				}
			}
			ஓ = null;
			_0B65 = -1;
			_0B64 = false;
			_୪ = new DockSiteInfo();
			return;
		}
		if (_0B65 != -1)
		{
			ఌ();
			if (ఊ)
			{
				DockingHandler(_୬, ((Control)this).PointToScreen(new Point(ఋ.X, ఋ.Y)));
			}
			else
			{
				DockContainerItem dockContainerItem4 = (DockContainerItem)Items[_0B65];
				Bar bar = ఎ(dockContainerItem4, ఏ: false);
				bar.DockingHandler(_୪, ((Control)this).PointToScreen(new Point(ఋ.X, ఋ.Y)));
				Form val3 = ((Control)this).FindForm();
				if (val3 != null && dockContainerItem4 != null && dockContainerItem4.Control != null)
				{
					((ContainerControl)val3).ActiveControl = dockContainerItem4.Control;
				}
			}
			_୪ = new DockSiteInfo();
			_0B65 = -1;
		}
		else if (!ள.IsEmpty || ழ != null)
		{
			ఌ();
			if (ఊ)
			{
				if (LayoutType == eLayoutType.Toolbar)
				{
					DockingHandler(_୬, ((Control)this).PointToScreen(new Point(ఋ.X, ఋ.Y)));
				}
			}
			else
			{
				DockingHandler(_୪, ((Control)this).PointToScreen(new Point(ఋ.X, ఋ.Y)));
			}
			_୪ = new DockSiteInfo();
		}
		else if (ఊ)
		{
			DockingHandler(_୬, ((Control)this).PointToScreen(new Point(ఋ.X, ఋ.Y)));
			_୪ = new DockSiteInfo();
		}
		_0B64 = false;
	}

	private void ఌ()
	{
		if (!ள.IsEmpty || ழ != null)
		{
			if (!ள.IsEmpty)
			{
				_089F.ब(ள, 3);
				ள = Rectangle.Empty;
			}
			if (ழ != null)
			{
				((Control)ழ).Visible = false;
				((Component)(object)ழ).Dispose();
				ழ = null;
			}
		}
	}

	internal Bar _0C0D()
	{
		if (ஈ.SelectedTab == null)
		{
			return null;
		}
		if (!(ஈ.SelectedTab.AttachedItem is DockContainerItem dockContainerItem))
		{
			return null;
		}
		if (!(_0AC3 is DotNetBarManager))
		{
			return null;
		}
		if (VisibleItemCount == 1 && GrabHandleStyle == eGrabHandleStyle.None)
		{
			_0C11();
			return this;
		}
		if (!CanUndock)
		{
			_0B65 = Items.IndexOf(dockContainerItem);
			_0C11();
			return this;
		}
		Bar bar = ఎ(dockContainerItem, ఏ: true);
		bar.InitalFloatLocation = Point.Empty;
		bar._0C11();
		return bar;
	}

	internal Bar ఎ(DockContainerItem _06EC, bool ఏ)
	{
		return ఎ(_06EC, ఏ, new Point(Control.MousePosition.X - 32, Control.MousePosition.Y - 8));
	}

	internal Bar ఎ(DockContainerItem _06EC, bool ఏ, Point ఐ)
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			((ContainerControl)val).ActiveControl = (Control)(object)this;
		}
		DotNetBarManager dotNetBarManager = _0AC3 as DotNetBarManager;
		ଡ଼.SubItems.Remove(_06EC);
		RecalcLayout();
		Bar bar = BarFunctions.CreateDuplicateDockBar(this);
		((Control)bar).Text = _06EC.Text;
		bar.InitalFloatLocation = ఐ;
		if (dotNetBarManager.Bars.Contains(_06EC.Name))
		{
			string name = _06EC.Name;
			int num = 0;
			while (dotNetBarManager.Bars.Contains(name + num))
			{
				num++;
			}
			bar.Name = name + num;
		}
		else
		{
			bar.Name = _06EC.Name;
		}
		bar.Items.Add(_06EC);
		dotNetBarManager.Bars.Add(bar);
		if (ఏ)
		{
			bar.DockSide = eDockSide.None;
			bar.Location = ఐ;
		}
		bar.ଙ = DockSide;
		bar._୬.objDockSite = ((Control)this).Parent as DockSite;
		bar._୬.LastRelativeDockToBar = this;
		bar._୬.LastDockSiteSide = DockSide;
		bar.CustomBar = true;
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			if (ownerBarSupport.ApplyDocumentBarStyle && DockSide == eDockSide.Document)
			{
				BarFunctions.RestoreAutoDocumentBarStyle(bar);
			}
			ownerBarSupport.InvokeBarTearOff(bar, new EventArgs());
		}
		return bar;
	}

	internal void _0C11()
	{
		_0AC2 = ((Control)this).PointToClient(Control.MousePosition);
		_0B63 = Size;
		((Control)this).Cursor = Cursors.SizeAll;
		_0B64 = true;
	}

	internal void ఒ()
	{
		if (_0B4A != null)
		{
			_0B4A(this, new EventArgs());
		}
	}

	private void ఓ()
	{
		EventArgs e = new EventArgs();
		if (_0B4B != null)
		{
			_0B4B(this, e);
		}
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeAutoHideChanged(this, e);
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideChanged));
	}

	public void CloseDockTab(DockContainerItem dockTab)
	{
		CloseDockTab(dockTab, eEventSource.Code);
	}

	public void CloseDockTab(DockContainerItem dockTab, eEventSource source)
	{
		DockTabClosingEventArgs dockTabClosingEventArgs = new DockTabClosingEventArgs(dockTab, source);
		క(dockTabClosingEventArgs);
		if (dockTabClosingEventArgs.Cancel)
		{
			return;
		}
		if (VisibleItemCount > 1)
		{
			if (dockTabClosingEventArgs.RemoveDockTab)
			{
				Items.Remove(dockTab);
			}
			else
			{
				BarUtilities.SetDockContainerVisible(dockTab, visible: false);
			}
		}
		else
		{
			if (dockTabClosingEventArgs.RemoveDockTab)
			{
				Items.Remove(dockTab);
			}
			ఈ();
			if (!Visible)
			{
				dockTab.Visible = false;
			}
		}
		ఔ(dockTabClosingEventArgs);
	}

	private void ఔ(DockTabClosingEventArgs ؿ)
	{
		if (_0B51 != null)
		{
			_0B51(this, ؿ);
		}
		if (Owner is DotNetBarManager)
		{
			((DotNetBarManager)Owner).InvokeDockTabClosed(this, ؿ);
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.DockTabClosed, ؿ.DockContainerItem));
	}

	internal void క(DockTabClosingEventArgs ؿ)
	{
		if (_0B50 != null)
		{
			_0B50(this, ؿ);
		}
		if (Owner is DotNetBarManager)
		{
			((DotNetBarManager)Owner).InvokeDockTabClosing(this, ؿ);
		}
	}

	private void ఖ(BarClosingEventArgs ؿ)
	{
		if (_0B4C != null)
		{
			_0B4C(this, ؿ);
		}
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBarClosing(this, ؿ);
		}
	}

	protected virtual void OnItemClick(BaseItem item, EventArgs e)
	{
		if (_09BC != null)
		{
			_09BC(item, e);
		}
	}

	internal void గ(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemClick(_06EC, ؿ);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ଡ଼.InternalClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (_0B5E == eBarState.Popup)
		{
			ISite site = ಌ();
			if (site != null && site.DesignMode)
			{
				ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					((IDesignerHost)site.GetService(typeof(IDesignerHost)))?.GetDesigner(selectionService.PrimarySelection as IComponent)?.DoDefaultAction();
				}
			}
		}
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (ୠ.Contains(pt) && !_0B82._1B73.Contains(pt) && !_0B82._1B75.Contains(pt))
		{
			if (_0B5E == eBarState.Floating)
			{
				if (_ஷ == eDoubleClickBarBehavior.ReDock || _ஷ == eDoubleClickBarBehavior.FloatAndReDock)
				{
					ReDock();
				}
			}
			else if (_0B5E == eBarState.Docked && CanUndock && ((Control)this).Parent is DockSite)
			{
				Point empty = Point.Empty;
				empty = ((!_୦.IsEmpty) ? _୦.Location : Control.MousePosition);
				if (_ஷ == eDoubleClickBarBehavior.Float || _ஷ == eDoubleClickBarBehavior.FloatAndReDock)
				{
					DockingHandler(new DockSiteInfo(), empty);
				}
			}
		}
		if (!((Control)this).IsDisposed)
		{
			ଡ଼.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		}
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	internal void _0A81(KeyEventArgs ؿ)
	{
		ଡ଼.InternalKeyDown(ؿ);
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

	public override string ToString()
	{
		return ((Control)this).Text;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).IsHandleCreated)
		{
			ஸ = ((Control)this).Visible;
		}
		if (_0BA1 >= 0)
		{
			if (((Control)this).Parent is DockSite && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) != _0BA1)
			{
				((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, _0BA1);
			}
			_0BA1 = -1;
		}
		if (_0AC3 is IOwner owner)
		{
			if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			else if (owner.ParentForm != null && _0BF2((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
		}
		((Control)this).OnVisibleChanged(e);
		if (_0B5E == eBarState.Docked && !_0B72 && Visible)
		{
			if (LayoutType == eLayoutType.DockContainer && _0BA0)
			{
				ౘ(ౙ: false);
			}
			RecalcLayout();
		}
		if (!Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		if (!Visible && இ != null && இ.Expanded)
		{
			இ.Expanded = false;
		}
	}

	public void SuspendLayout()
	{
		ஹ = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout()
	{
		ResumeLayout(performLayout: true);
	}

	public void ResumeLayout(bool performLayout)
	{
		ஹ = false;
		((Control)this).ResumeLayout(true);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Control)this).Parent != null && !(((Control)this).Parent is ᆡ) && !(((Control)this).Parent is DockSite))
		{
			ଡ଼.SetOwner(this);
			if (!ச && ((Component)this).DesignMode && ((Control)this).FindForm() != null)
			{
				DotNetBarManager.RegisterOwnerParentMsgHandler(this, ((Control)this).FindForm());
				ச = true;
			}
			if (_09DA.Count == 0)
			{
				foreach (BaseItem subItem in ଡ଼.SubItems)
				{
					((IOwner)this).AddShortcutsFromItem(subItem);
				}
			}
			if (((Control)this).Parent != null && !(((Control)this).Parent is ᆡ) && !(((Control)this).Parent is DockSite) && !AutoHide && !_২)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
			if (!_0B70)
			{
				Stretch = true;
			}
		}
		else if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
		if (((Component)this).DesignMode)
		{
			ଡ଼.SetDesignMode(((Component)this).DesignMode);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (ஐ.ᮄ)
		{
			ఙ();
		}
		if (((Control)this).Parent != null && !(((Control)this).Parent is ᆡ) && !(((Control)this).Parent is DockSite) && !AutoHide && !_২)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
		RecalcSize();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		_0A75();
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
		((Control)this).OnHandleDestroyed(e);
	}

	private void ఙ()
	{
		if (!BarFunctions.ThemedOS)
		{
			return;
		}
		_0A79 obj = _0A79;
		Graphics val = CreateGraphics();
		try
		{
			Size size = obj.ThemeMinSize(val, _༣._0F2E, ཉ.ཎ);
			ஐ._0749 = size.Width;
			size = obj.ThemeMinSize(val, _༣._0F2F, ཉ.ཎ);
			ஐ.ሪ = size.Width;
			size = obj.ThemeMinSize(val, _༣._0F30, ཉ.ཎ);
			ஐ.ራ = size.Height;
			size = obj.ThemeTrueSize(val, _༣._༦, ཉ.ཎ);
			ஐ._074A = size.Height;
		}
		finally
		{
			val.Dispose();
		}
	}

	private void _0A78()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = null;
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = null;
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = null;
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = null;
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = null;
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = null;
		}
	}

	private void చ()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = new _0A79((Control)(object)this);
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = new _0A7A((Control)(object)this);
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = new _0A7B((Control)(object)this);
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = new _0A7C((Control)(object)this);
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = new _0A7D((Control)(object)this);
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = new _0A7F((Control)(object)this);
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (MenuBar)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (MenuBar)
		{
			((Control)this).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void SetDesignMode(bool b)
	{
		ଡ଼.SetDesignMode(b);
	}

	internal bool ఛ()
	{
		return ((Component)this).DesignMode;
	}

	private void జ(SerializeItemEventArgs ؿ)
	{
		if (_0B52 != null)
		{
			_0B52(this, ؿ);
		}
		if (Owner != this && Owner is ICustomSerialization)
		{
			((ICustomSerialization)Owner).InvokeSerializeItem(ؿ);
		}
	}

	void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in జ
		this.జ(ؿ);
	}

	private void ఝ(SerializeItemEventArgs ؿ)
	{
		if (_0B53 != null)
		{
			_0B53(this, ؿ);
		}
		if (Owner != this && Owner is ICustomSerialization)
		{
			((ICustomSerialization)Owner).InvokeDeserializeItem(ؿ);
		}
	}

	void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ఝ
		this.ఝ(ؿ);
	}

	public void SaveDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		ఠ(xmlDocument);
		xmlDocument.Save(FileName);
	}

	internal void ఠ(XmlDocument డ)
	{
		XmlElement xmlElement = డ.CreateElement("bar");
		డ.AppendChild(xmlElement);
		ణ(xmlElement);
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		ఢ(xmlDocument);
	}

	internal void ఢ(XmlDocument డ)
	{
		if (డ.FirstChild.Name != "bar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		ଡ଼.SubItems.Clear();
		ద(డ.FirstChild as XmlElement);
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.AddShortcutsFromBar(this);
		}
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
	}

	private void _0A37(object _٠, EventArgs ؿ)
	{
		if (_09D3 != null)
		{
			_09D3(_٠, ؿ);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਷
		this._0A37(_٠, ؿ);
	}

	internal void ణ(XmlElement త)
	{
		ణ(త, థ: false);
	}

	internal void ణ(XmlElement త, bool థ)
	{
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected I4, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected I4, but got Unknown
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		త.SetAttribute("name", Name);
		త.SetAttribute("candockleft", XmlConvert.ToString(_0B73));
		త.SetAttribute("candockright", XmlConvert.ToString(_0B74));
		త.SetAttribute("candocktop", XmlConvert.ToString(_0B75));
		త.SetAttribute("candockbottom", XmlConvert.ToString(_0B76));
		త.SetAttribute("candockdoc", XmlConvert.ToString(_0B7B));
		త.SetAttribute("candocktab", XmlConvert.ToString(_0B7A));
		త.SetAttribute("canmaximizefloating", XmlConvert.ToString(__0BBB));
		త.SetAttribute("text", ((Control)this).Text);
		త.SetAttribute("dockline", XmlConvert.ToString(_0B62));
		if (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom)
		{
			త.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Left));
		}
		else if (DockSide == eDockSide.Left || DockSide == eDockSide.Right)
		{
			త.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Top));
		}
		త.SetAttribute("grabhandle", XmlConvert.ToString((int)ୟ));
		త.SetAttribute("menubar", XmlConvert.ToString(_୯));
		త.SetAttribute("stretch", XmlConvert.ToString(_0B70));
		త.SetAttribute("style", XmlConvert.ToString((int)ଡ଼.Style));
		త.SetAttribute("wrapdock", XmlConvert.ToString(_୭));
		త.SetAttribute("wrapfloat", XmlConvert.ToString(_୮));
		if (எ)
		{
			త.SetAttribute("lockdockpos", XmlConvert.ToString(எ));
		}
		if (!_0B77)
		{
			త.SetAttribute("canundock", XmlConvert.ToString(_0B77));
		}
		if (_0B97)
		{
			త.SetAttribute("tabnav", XmlConvert.ToString(_0B97));
		}
		if (!_০)
		{
			త.SetAttribute("tooltips", XmlConvert.ToString(_০));
		}
		if (ଡ଼.MoreItemsOnMenu)
		{
			త.SetAttribute("overflowmenu", XmlConvert.ToString(ଡ଼.MoreItemsOnMenu));
		}
		if (உ)
		{
			త.SetAttribute("state", XmlConvert.ToString(2));
			eDockSide eDockSide2 = eDockSide.None;
			DockStyle dockSide = _୬.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				eDockSide2 = eDockSide.Left;
				break;
			case 3:
				eDockSide2 = eDockSide.Right;
				break;
			case 0:
				eDockSide2 = eDockSide.Top;
				break;
			case 1:
				eDockSide2 = eDockSide.Bottom;
				break;
			}
			త.SetAttribute("dockside", XmlConvert.ToString((int)eDockSide2));
		}
		else
		{
			త.SetAttribute("state", XmlConvert.ToString((int)_0B5E));
			త.SetAttribute("dockside", XmlConvert.ToString((int)DockSide));
			if (DockSide == eDockSide.None && ୱ != null)
			{
				త.SetAttribute("fpos", ((Form)ୱ).Location.X + "," + ((Form)ୱ).Location.Y + "," + ((Control)this).DisplayRectangle.Width + "," + ((Control)this).DisplayRectangle.Height);
			}
		}
		IOwnerBarSupport ownerBarSupport = _0AC3 as IOwnerBarSupport;
		if (_0B5E == eBarState.Floating && !Visible && ownerBarSupport != null && ownerBarSupport.WereVisible.Count > 0 && ownerBarSupport.WereVisible.Contains(this))
		{
			త.SetAttribute("visible", XmlConvert.ToString(value: true));
		}
		else if (உ)
		{
			త.SetAttribute("visible", XmlConvert.ToString(value: true));
			త.SetAttribute("autohide", XmlConvert.ToString(value: true));
		}
		else
		{
			త.SetAttribute("visible", XmlConvert.ToString(Visible));
		}
		త.SetAttribute("custom", XmlConvert.ToString(_0B81));
		త.SetAttribute("canhide", XmlConvert.ToString(_0B7C));
		త.SetAttribute("imagesize", XmlConvert.ToString((int)ਉ));
		త.SetAttribute("itemsp", XmlConvert.ToString(ଡ଼.ItemSpacing));
		త.SetAttribute("themes", XmlConvert.ToString(_09F8));
		if (!ଡ଼.CanCustomize)
		{
			త.SetAttribute("cancust", XmlConvert.ToString(ଡ଼.CanCustomize));
		}
		if (((Control)this).Font != null && _0B8D)
		{
			త.SetAttribute("fontname", ((Control)this).Font.Name);
			త.SetAttribute("fontemsize", XmlConvert.ToString(((Control)this).Font.Size));
			త.SetAttribute("fontstyle", XmlConvert.ToString((int)((Control)this).Font.Style));
		}
		if (!ଡ଼.m_BackgroundColor.IsEmpty)
		{
			త.SetAttribute("backcolor", BarFunctions.ColorToString(ଡ଼.BackColor));
		}
		త.SetAttribute("layout", XmlConvert.ToString((int)ଡ଼.LayoutType));
		త.SetAttribute("eqbutton", XmlConvert.ToString(ଡ଼.EqualButtonSize));
		if (_0B7F != 0)
		{
			త.SetAttribute("dborder", XmlConvert.ToString((int)_0B7F));
		}
		if (!_0B7E)
		{
			త.SetAttribute("acceptdrop", XmlConvert.ToString(_0B7E));
		}
		if (_0B80 != SystemColors.ControlDark)
		{
			త.SetAttribute("slcolor", BarFunctions.ColorToString(_0B80));
		}
		if (!அ.IsEmpty)
		{
			త.SetAttribute("captionbc", BarFunctions.ColorToString(அ));
		}
		if (!ஆ.IsEmpty)
		{
			త.SetAttribute("captionfc", BarFunctions.ColorToString(ஆ));
		}
		if (_0BA0)
		{
			త.SetAttribute("showtab", XmlConvert.ToString(_0BA0));
		}
		if (AutoHide)
		{
			if (ଡ଼.ර != 0)
			{
				త.SetAttribute("dockwidth", XmlConvert.ToString(ଡ଼.ර));
			}
			else if (_୬.DockedWidth != 0)
			{
				త.SetAttribute("dockwidth", XmlConvert.ToString(_୬.DockedWidth));
			}
			if (ଡ଼._0B03 != 0)
			{
				త.SetAttribute("dockheight", XmlConvert.ToString(ଡ଼._0B03));
			}
			else if (_୬.DockedHeight != 0)
			{
				త.SetAttribute("dockheight", XmlConvert.ToString(_୬.DockedHeight));
			}
		}
		else
		{
			if (ଡ଼.ර != 0 && _0B11)
			{
				త.SetAttribute("dockwidth", XmlConvert.ToString(ଡ଼.ර));
			}
			if (ଡ଼._0B03 != 0 && _0B12)
			{
				త.SetAttribute("dockheight", XmlConvert.ToString(ଡ଼._0B03));
			}
		}
		if (_0B91 > 0)
		{
			త.SetAttribute("splitwidthpercent", XmlConvert.ToString(_0B91));
		}
		if (ஒ > 0)
		{
			త.SetAttribute("splitheightpercent", XmlConvert.ToString(ஒ));
		}
		if (ଡ଼.PaddingBottom != 1)
		{
			త.SetAttribute("padbottom", XmlConvert.ToString(ଡ଼.PaddingBottom));
		}
		if (ଡ଼.PaddingLeft != 1)
		{
			త.SetAttribute("padleft", XmlConvert.ToString(ଡ଼.PaddingLeft));
		}
		if (ଡ଼.PaddingRight != 1)
		{
			త.SetAttribute("padright", XmlConvert.ToString(ଡ଼.PaddingRight));
		}
		if (ଡ଼.PaddingTop != 1)
		{
			త.SetAttribute("padtop", XmlConvert.ToString(ଡ଼.PaddingTop));
		}
		if (ଡ଼.LayoutType == eLayoutType.DockContainer && SelectedDockTab >= 0)
		{
			త.SetAttribute("seldocktab", XmlConvert.ToString(SelectedDockTab));
		}
		if (!ஊ)
		{
			త.SetAttribute("canautohide", XmlConvert.ToString(ஊ));
		}
		if (!_0B79)
		{
			త.SetAttribute("canreordertabs", XmlConvert.ToString(_0B79));
		}
		if (!_0B78)
		{
			త.SetAttribute("cantearoff", XmlConvert.ToString(_0B78));
		}
		if (!க)
		{
			త.SetAttribute("hidein", XmlConvert.ToString(க));
		}
		if (ஔ != eTabStripAlignment.Bottom)
		{
			త.SetAttribute("tabalign", XmlConvert.ToString((int)ஔ));
		}
		if (_0B9B != 100)
		{
			త.SetAttribute("ahanim", XmlConvert.ToString(_0B9B));
		}
		if (!_0BA2)
		{
			త.SetAttribute("autocaptionmenu", XmlConvert.ToString(_0BA2));
		}
		if (த)
		{
			త.SetAttribute("autocaptionsync", XmlConvert.ToString(த));
		}
		if (!_0BA5)
		{
			త.SetAttribute("savelayout", XmlConvert.ToString(_0BA5));
		}
		if (լ.SchemeChanged)
		{
			XmlElement xmlElement = త.OwnerDocument.CreateElement("colorscheme");
			լ.Serialize(xmlElement);
			త.AppendChild(xmlElement);
		}
		if (((Control)this).BackgroundImage != null)
		{
			XmlElement xmlElement2 = త.OwnerDocument.CreateElement("backimage");
			త.AppendChild(xmlElement2);
			BarFunctions.SerializeImage(((Control)this).BackgroundImage, xmlElement2);
			int num = (int)ސ;
			xmlElement2.SetAttribute("pos", num.ToString());
			xmlElement2.SetAttribute("alpha", ޑ.ToString());
		}
		if (థ)
		{
			return;
		}
		XmlElement xmlElement3 = త.OwnerDocument.CreateElement("items");
		త.AppendChild(xmlElement3);
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (subItem.ShouldSerialize)
			{
				XmlElement xmlElement4 = త.OwnerDocument.CreateElement("item");
				xmlElement3.AppendChild(xmlElement4);
				itemSerializationContext.ItemXmlElement = xmlElement4;
				subItem.Serialize(itemSerializationContext);
			}
		}
	}

	internal void ద(XmlElement త)
	{
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		Deserialize(త, itemSerializationContext);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deserialize(XmlElement xmlThisBar, ItemSerializationContext context)
	{
		ங = true;
		ஜ = true;
		try
		{
			ధ(xmlThisBar);
			((Control)this).BackgroundImage = null;
			foreach (XmlElement childNode in xmlThisBar.ChildNodes)
			{
				switch (childNode.Name)
				{
				case "items":
					foreach (XmlElement childNode2 in childNode.ChildNodes)
					{
						BaseItem baseItem = context.CreateItemFromXml(childNode2);
						ଡ଼.SubItems.Add(baseItem);
						context.ItemXmlElement = childNode2;
						baseItem.Deserialize(context);
					}
					break;
				case "colorscheme":
					լ.Deserialize(childNode);
					break;
				case "backimage":
					((Control)this).BackgroundImage = BarFunctions.DeserializeImage(childNode);
					ސ = (eBackgroundImagePosition)XmlConvert.ToInt32(childNode.GetAttribute("pos"));
					ޑ = XmlConvert.ToByte(childNode.GetAttribute("alpha"));
					break;
				}
			}
			if (XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside")) == 0)
			{
				if (xmlThisBar.HasAttribute("fpos"))
				{
					string attribute = xmlThisBar.GetAttribute("fpos");
					string[] array = attribute.Split(new char[1] { ',' });
					if (array.Length == 4)
					{
						Rectangle rectangle = new Rectangle(XmlConvert.ToInt32(array[0]), XmlConvert.ToInt32(array[1]), XmlConvert.ToInt32(array[2]), XmlConvert.ToInt32(array[3]));
						_୦ = rectangle;
					}
				}
				if (!XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible")) || (xmlThisBar.HasAttribute("autohide") && XmlConvert.ToBoolean(xmlThisBar.GetAttribute("autohide"))))
				{
					_0BA7 = true;
				}
			}
			if (xmlThisBar.HasAttribute("seldocktab"))
			{
				int num = XmlConvert.ToInt32(xmlThisBar.GetAttribute("seldocktab"));
				if (num >= 0 && num < ଡ଼.SubItems.Count)
				{
					foreach (BaseItem subItem in ଡ଼.SubItems)
					{
						subItem.Displayed = false;
					}
					ଡ଼.SubItems[num].Displayed = true;
				}
			}
			_0B62 = XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockline"));
			if (ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				ౘ(ౙ: true);
			}
			DockSide = (eDockSide)XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside"));
			if (_0BA7)
			{
				_0BA7 = false;
			}
			if (xmlThisBar.HasAttribute("autohide") && context._DesignerHost == null)
			{
				((Control)this).Visible = false;
				if (ଡ଼.ර > 0)
				{
					((Control)this).Width = ଡ଼.ර;
				}
				if (ଡ଼._0B03 > 0)
				{
					((Control)this).Height = ଡ଼._0B03;
				}
				AutoHide = true;
				_୬.DockedWidth = ଡ଼.ර;
				_୬.DockedHeight = ଡ଼._0B03;
			}
			else if (context._DesignerHost == null)
			{
				Visible = XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible"));
			}
			else
			{
				Visible = true;
			}
		}
		finally
		{
			ங = false;
		}
		_0BA6 = false;
		_0BCD();
	}

	private void ధ(XmlElement న)
	{
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		_0B5E = eBarState.Docked;
		Name = న.GetAttribute("name");
		if (న.HasAttribute("candockleft"))
		{
			_0B73 = XmlConvert.ToBoolean(న.GetAttribute("candockleft"));
		}
		if (న.HasAttribute("candockright"))
		{
			_0B74 = XmlConvert.ToBoolean(న.GetAttribute("candockright"));
		}
		if (న.HasAttribute("candocktop"))
		{
			_0B75 = XmlConvert.ToBoolean(న.GetAttribute("candocktop"));
		}
		if (న.HasAttribute("candockbottom"))
		{
			_0B76 = XmlConvert.ToBoolean(న.GetAttribute("candockbottom"));
		}
		if (న.HasAttribute("candockdoc"))
		{
			_0B7B = XmlConvert.ToBoolean(న.GetAttribute("candockdoc"));
		}
		if (న.HasAttribute("candocktab"))
		{
			_0B7A = XmlConvert.ToBoolean(న.GetAttribute("candocktab"));
		}
		if (న.HasAttribute("canmaximizefloating"))
		{
			__0BBB = XmlConvert.ToBoolean(న.GetAttribute("canmaximizefloating"));
		}
		if (న.HasAttribute("text"))
		{
			((Control)this).Text = న.GetAttribute("text");
		}
		if (న.HasAttribute("dockline"))
		{
			_0B62 = XmlConvert.ToInt32(న.GetAttribute("dockline"));
		}
		if (న.HasAttribute("dockoffset"))
		{
			ୡ = XmlConvert.ToInt32(న.GetAttribute("dockoffset"));
		}
		if (న.HasAttribute("grabhandle"))
		{
			ୟ = (eGrabHandleStyle)XmlConvert.ToInt32(న.GetAttribute("grabhandle"));
		}
		if (న.HasAttribute("menubar"))
		{
			_୯ = XmlConvert.ToBoolean(న.GetAttribute("menubar"));
		}
		if (న.HasAttribute("stretch"))
		{
			_0B70 = XmlConvert.ToBoolean(న.GetAttribute("stretch"));
		}
		if (న.HasAttribute("wrapdock"))
		{
			_୭ = XmlConvert.ToBoolean(న.GetAttribute("wrapdock"));
		}
		if (న.HasAttribute("wrapfloat"))
		{
			_୮ = XmlConvert.ToBoolean(న.GetAttribute("wrapfloat"));
		}
		if (న.HasAttribute("custom"))
		{
			_0B81 = XmlConvert.ToBoolean(న.GetAttribute("custom"));
		}
		if (న.HasAttribute("canhide"))
		{
			_0B7C = XmlConvert.ToBoolean(న.GetAttribute("canhide"));
		}
		if (న.HasAttribute("imagesize"))
		{
			ਉ = (eBarImageSize)XmlConvert.ToInt32(న.GetAttribute("imagesize"));
		}
		if (న.HasAttribute("itemsp"))
		{
			ଡ଼.ItemSpacing = XmlConvert.ToInt32(న.GetAttribute("itemsp"));
		}
		if (న.HasAttribute("backcolor"))
		{
			ଡ଼.BackColor = BarFunctions.ColorFromString(న.GetAttribute("backcolor"));
		}
		if (న.HasAttribute("layout"))
		{
			ଡ଼.LayoutType = (eLayoutType)XmlConvert.ToInt32(న.GetAttribute("layout"));
		}
		if (న.HasAttribute("eqbutton"))
		{
			ଡ଼.EqualButtonSize = XmlConvert.ToBoolean(న.GetAttribute("eqbutton"));
		}
		if (న.HasAttribute("dborder"))
		{
			_0B7F = (eBorderType)XmlConvert.ToInt32(న.GetAttribute("dborder"));
		}
		if (న.HasAttribute("acceptdrop"))
		{
			_0B7E = XmlConvert.ToBoolean(న.GetAttribute("acceptdrop"));
		}
		if (న.HasAttribute("slcolor"))
		{
			_0B80 = BarFunctions.ColorFromString(న.GetAttribute("slcolor"));
		}
		if (న.HasAttribute("captionbc"))
		{
			அ = BarFunctions.ColorFromString(న.GetAttribute("captionbc"));
		}
		if (న.HasAttribute("captionfc"))
		{
			ஆ = BarFunctions.ColorFromString(న.GetAttribute("captionfc"));
		}
		if (న.HasAttribute("dockwidth"))
		{
			ଡ଼.ර = XmlConvert.ToInt32(న.GetAttribute("dockwidth"));
		}
		if (న.HasAttribute("dockheight"))
		{
			ଡ଼._0B03 = XmlConvert.ToInt32(న.GetAttribute("dockheight"));
		}
		if (న.HasAttribute("splitwidthpercent"))
		{
			_0B91 = XmlConvert.ToInt32(న.GetAttribute("splitwidthpercent"));
		}
		else
		{
			_0B91 = 0;
		}
		if (న.HasAttribute("splitheightpercent"))
		{
			ஒ = XmlConvert.ToInt32(న.GetAttribute("splitheightpercent"));
		}
		if (న.HasAttribute("padbottom"))
		{
			ଡ଼.PaddingBottom = XmlConvert.ToInt32(న.GetAttribute("padbottom"));
		}
		if (న.HasAttribute("padleft"))
		{
			ଡ଼.PaddingLeft = XmlConvert.ToInt32(న.GetAttribute("padleft"));
		}
		if (న.HasAttribute("padright"))
		{
			ଡ଼.PaddingRight = XmlConvert.ToInt32(న.GetAttribute("padright"));
		}
		if (న.HasAttribute("padtop"))
		{
			ଡ଼.PaddingTop = XmlConvert.ToInt32(న.GetAttribute("padtop"));
		}
		if (న.HasAttribute("lockdockpos"))
		{
			எ = XmlConvert.ToBoolean(న.GetAttribute("lockdockpos"));
		}
		if (న.HasAttribute("canundock"))
		{
			_0B77 = XmlConvert.ToBoolean(న.GetAttribute("canundock"));
		}
		if (న.HasAttribute("canreordertabs"))
		{
			_0B79 = XmlConvert.ToBoolean(న.GetAttribute("canreordertabs"));
		}
		if (న.HasAttribute("cantearoff"))
		{
			_0B78 = XmlConvert.ToBoolean(న.GetAttribute("cantearoff"));
		}
		if (న.HasAttribute("canautohide"))
		{
			ஊ = XmlConvert.ToBoolean(న.GetAttribute("canautohide"));
		}
		if (న.HasAttribute("cancust"))
		{
			ଡ଼.CanCustomize = XmlConvert.ToBoolean(న.GetAttribute("cancust"));
		}
		if (న.HasAttribute("tabalign"))
		{
			ஔ = (eTabStripAlignment)XmlConvert.ToInt32(న.GetAttribute("tabalign"));
		}
		if (న.HasAttribute("showtab"))
		{
			_0BA0 = XmlConvert.ToBoolean(న.GetAttribute("showtab"));
		}
		if (న.HasAttribute("hidein"))
		{
			க = XmlConvert.ToBoolean(న.GetAttribute("hidein"));
		}
		if (న.HasAttribute("themes"))
		{
			_09F8 = XmlConvert.ToBoolean(న.GetAttribute("themes"));
			ଡ଼.ThemeAware = _09F8;
		}
		if (న.HasAttribute("tabnav"))
		{
			_0B97 = XmlConvert.ToBoolean(న.GetAttribute("tabnav"));
		}
		if (న.HasAttribute("tooltips"))
		{
			_০ = XmlConvert.ToBoolean(న.GetAttribute("tooltips"));
		}
		if (న.HasAttribute("overflowmenu"))
		{
			ଡ଼.MoreItemsOnMenu = XmlConvert.ToBoolean(న.GetAttribute("overflowmenu"));
		}
		if (న.HasAttribute("autocaptionmenu"))
		{
			_0BA2 = XmlConvert.ToBoolean(న.GetAttribute("autocaptionmenu"));
		}
		if (న.HasAttribute("autocaptionsync"))
		{
			AutoSyncBarCaption = XmlConvert.ToBoolean(న.GetAttribute("autocaptionsync"));
		}
		if (న.HasAttribute("style"))
		{
			string attribute = న.GetAttribute("style");
			Style = (eDotNetBarStyle)XmlConvert.ToInt32(attribute);
		}
		if (న.HasAttribute("fontname"))
		{
			string attribute2 = న.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(న.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(న.GetAttribute("fontstyle"));
			try
			{
				((Control)this).Font = new Font(attribute2, num, val);
			}
			catch (Exception)
			{
				((Control)this).Font = SystemFonts.MenuFont;
			}
			_0B8D = true;
		}
		if (న.HasAttribute("ahanim"))
		{
			_0B9B = XmlConvert.ToInt32(న.GetAttribute("ahanim"));
		}
		if (న.HasAttribute("savelayout"))
		{
			_0BA5 = XmlConvert.ToBoolean(న.GetAttribute("savelayout"));
		}
	}

	public void SaveLayout(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		_0C29(xmlDocument);
		xmlDocument.Save(FileName);
	}

	internal void _0C29(XmlDocument డ)
	{
		XmlElement xmlElement = డ.CreateElement("bar");
		డ.AppendChild(xmlElement);
		ఫ(xmlElement);
	}

	public void LoadLayout(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		ప(xmlDocument);
	}

	internal void ప(XmlDocument డ)
	{
		if (డ.FirstChild.Name != "bar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		బ(డ.FirstChild as XmlElement);
	}

	internal void ఫ(XmlElement త)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected I4, but got Unknown
		if (CustomBar)
		{
			ణ(త, థ: true);
		}
		else
		{
			త.SetAttribute("name", Name);
			త.SetAttribute("dockline", XmlConvert.ToString(_0B62));
			త.SetAttribute("layout", XmlConvert.ToString((int)LayoutType));
			if (ୡ == 0 && ((Control)this).Left > 0 && (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom))
			{
				త.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Left));
			}
			else if (ୡ == 0 && ((Control)this).Top > 0 && (DockSide == eDockSide.Left || DockSide == eDockSide.Right))
			{
				త.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Top));
			}
			else
			{
				త.SetAttribute("dockoffset", XmlConvert.ToString(ୡ));
			}
			if (உ)
			{
				త.SetAttribute("state", XmlConvert.ToString(2));
				eDockSide eDockSide2 = eDockSide.None;
				DockStyle dockSide = _୬.DockSide;
				switch (dockSide - 1)
				{
				case 2:
					eDockSide2 = eDockSide.Left;
					break;
				case 3:
					eDockSide2 = eDockSide.Right;
					break;
				case 0:
					eDockSide2 = eDockSide.Top;
					break;
				case 1:
					eDockSide2 = eDockSide.Bottom;
					break;
				}
				త.SetAttribute("dockside", XmlConvert.ToString((int)eDockSide2));
			}
			else
			{
				త.SetAttribute("state", XmlConvert.ToString((int)_0B5E));
				త.SetAttribute("dockside", XmlConvert.ToString((int)DockSide));
				if (DockSide == eDockSide.None && ୱ != null)
				{
					త.SetAttribute("fpos", ((Form)ୱ).Location.X + "," + ((Form)ୱ).Location.Y + "," + ((Control)this).DisplayRectangle.Width + "," + ((Control)this).DisplayRectangle.Height);
				}
				else if (!_୦.IsEmpty)
				{
					త.SetAttribute("fpos", _୦.X + "," + _୦.Y + "," + _୦.Width + "," + _୦.Height);
				}
			}
			IOwnerBarSupport ownerBarSupport = _0AC3 as IOwnerBarSupport;
			if (_0B5E == eBarState.Floating && !Visible && ownerBarSupport != null && ownerBarSupport.WereVisible.Count > 0 && ownerBarSupport.WereVisible.Contains(this))
			{
				త.SetAttribute("visible", XmlConvert.ToString(value: true));
			}
			else if (உ)
			{
				త.SetAttribute("visible", XmlConvert.ToString(value: true));
				త.SetAttribute("autohide", XmlConvert.ToString(value: true));
			}
			else
			{
				త.SetAttribute("visible", XmlConvert.ToString(Visible));
			}
			if (ଡ଼.ර != 0)
			{
				త.SetAttribute("dockwidth", XmlConvert.ToString(ଡ଼.ර));
			}
			if (ଡ଼._0B03 != 0)
			{
				త.SetAttribute("dockheight", XmlConvert.ToString(ଡ଼._0B03));
			}
			if (_0B91 > 0)
			{
				త.SetAttribute("splitwidthpercent", XmlConvert.ToString(_0B91));
			}
			if (ஒ > 0)
			{
				త.SetAttribute("splitheightpercent", XmlConvert.ToString(ஒ));
			}
			if (ଡ଼.LayoutType == eLayoutType.DockContainer && SelectedDockTab >= 0)
			{
				త.SetAttribute("seldocktab", XmlConvert.ToString(SelectedDockTab));
			}
		}
		XmlElement xmlElement = త.OwnerDocument.CreateElement("items");
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (subItem.OriginalBarName != "" || subItem.OriginalPosition >= 0 || subItem.UserCustomized || CustomBar || _0BA6 || LayoutType == eLayoutType.DockContainer)
			{
				XmlElement xmlElement2 = త.OwnerDocument.CreateElement("item");
				xmlElement2.SetAttribute("name", subItem.Name);
				xmlElement2.SetAttribute("origBar", subItem.OriginalBarName);
				xmlElement2.SetAttribute("origPos", XmlConvert.ToString(subItem.OriginalPosition));
				xmlElement2.SetAttribute("pos", XmlConvert.ToString(ଡ଼.SubItems.IndexOf(subItem)));
				if (!subItem.Visible)
				{
					xmlElement2.SetAttribute("visible", XmlConvert.ToString(subItem.Visible));
				}
				xmlElement.AppendChild(xmlElement2);
			}
		}
		if (xmlElement.ChildNodes.Count > 0)
		{
			త.AppendChild(xmlElement);
		}
	}

	internal void బ(XmlElement న)
	{
		ங = true;
		try
		{
			ஜ = true;
			ధ(న);
			if (న.ChildNodes.Count > 0)
			{
				foreach (XmlElement childNode in న.ChildNodes)
				{
					if (childNode.Name == "items")
					{
						భ(న);
						break;
					}
				}
			}
			if (న.HasAttribute("fpos"))
			{
				string attribute = న.GetAttribute("fpos");
				string[] array = attribute.Split(new char[1] { ',' });
				if (array.Length == 4)
				{
					Rectangle rectangle = new Rectangle(XmlConvert.ToInt32(array[0]), XmlConvert.ToInt32(array[1]), XmlConvert.ToInt32(array[2]), XmlConvert.ToInt32(array[3]));
					_୦ = rectangle;
				}
			}
			if (XmlConvert.ToInt32(న.GetAttribute("dockside")) == 0 && !XmlConvert.ToBoolean(న.GetAttribute("visible")))
			{
				_0BA7 = true;
			}
			if (న.HasAttribute("seldocktab"))
			{
				int num = XmlConvert.ToInt32(న.GetAttribute("seldocktab"));
				if (num >= 0 && num < ଡ଼.SubItems.Count)
				{
					foreach (BaseItem subItem in ଡ଼.SubItems)
					{
						subItem.Displayed = false;
					}
					ଡ଼.SubItems[num].Displayed = true;
					if (த)
					{
						((Control)this).Text = ଡ଼.SubItems[num].Text;
					}
				}
			}
			if (ଡ଼.LayoutType == eLayoutType.DockContainer)
			{
				ౘ(ౙ: true);
			}
			if (AutoHide)
			{
				if (న.HasAttribute("autohide"))
				{
					int ර = ଡ଼.ර;
					int num2 = ଡ଼._0B03;
					ଡ଼.ර = 0;
					ଡ଼._0B03 = 0;
					AutoHide = false;
					ଡ଼.ර = ර;
					ଡ଼._0B03 = num2;
				}
				else
				{
					AutoHide = false;
				}
			}
			DockSide = (eDockSide)XmlConvert.ToInt32(న.GetAttribute("dockside"));
			if (_0BA7)
			{
				_0BA7 = false;
			}
			if (న.HasAttribute("autohide"))
			{
				if (!AutoHide)
				{
					((Control)this).Visible = false;
					if (ଡ଼.ර > 0)
					{
						((Control)this).Width = ଡ଼.ර;
					}
					if (ଡ଼._0B03 > 0)
					{
						((Control)this).Height = ଡ଼._0B03;
					}
					AutoHide = true;
					_୬.DockedWidth = ଡ଼.ර;
					_୬.DockedHeight = ଡ଼._0B03;
				}
			}
			else
			{
				Visible = XmlConvert.ToBoolean(న.GetAttribute("visible"));
			}
		}
		finally
		{
			ங = false;
		}
	}

	private void భ(XmlElement న)
	{
		eLayoutType eLayoutType2 = eLayoutType.Toolbar;
		DotNetBarManager dotNetBarManager = null;
		if (_0AC3 != null && _0AC3 is DotNetBarManager)
		{
			dotNetBarManager = _0AC3 as DotNetBarManager;
		}
		if (న.HasAttribute("layout"))
		{
			eLayoutType2 = (eLayoutType)XmlConvert.ToInt32(న.GetAttribute("layout"));
		}
		if (eLayoutType2 == eLayoutType.DockContainer)
		{
			_0BA6 = true;
		}
		if (న.HasAttribute("custom"))
		{
			XmlConvert.ToBoolean(న.GetAttribute("custom"));
		}
		foreach (XmlElement childNode in న.ChildNodes)
		{
			string name;
			if ((name = childNode.Name) == null || !(name == "items"))
			{
				continue;
			}
			for (int i = 0; i < childNode.ChildNodes.Count; i++)
			{
				XmlElement xmlElement2 = childNode.ChildNodes[i] as XmlElement;
				string attribute = xmlElement2.GetAttribute("name");
				string attribute2 = xmlElement2.GetAttribute("origBar");
				int originalPosition = XmlConvert.ToInt32(xmlElement2.GetAttribute("origPos"));
				int num = XmlConvert.ToInt32(xmlElement2.GetAttribute("pos"));
				bool flag = true;
				if (xmlElement2.HasAttribute("visible"))
				{
					flag = XmlConvert.ToBoolean(xmlElement2.GetAttribute("visible"));
				}
				if (eLayoutType2 == eLayoutType.DockContainer)
				{
					if (Items.Contains(attribute))
					{
						BaseItem baseItem = Items[attribute];
						if (baseItem.Visible != flag)
						{
							baseItem.Visible = flag;
						}
						if (Items.IndexOf(Items[attribute]) != num)
						{
							Items.Remove(baseItem);
							if (Items.Count > num)
							{
								Items.Insert(num, baseItem);
							}
							else
							{
								Items.Add(baseItem);
							}
						}
					}
					else
					{
						if (dotNetBarManager == null)
						{
							continue;
						}
						BaseItem item = dotNetBarManager.GetItem(attribute);
						if (item == null || !(item is DockContainerItem))
						{
							continue;
						}
						Bar bar = item.ContainerControl as Bar;
						item.Parent.SubItems.Remove(item);
						if (item.Visible != flag)
						{
							item.Visible = flag;
						}
						if (Items.Count > num)
						{
							Items.Insert(num, item);
						}
						else
						{
							Items.Add(item);
						}
						if (bar != null && bar.Items.Count == 0)
						{
							if (bar.AutoHide)
							{
								bar.AutoHide = false;
							}
							bar.Visible = false;
						}
						else if (bar != null && bar.VisibleItemCount == 0)
						{
							bar.Visible = false;
						}
					}
					continue;
				}
				BaseItem baseItem2 = null;
				if (Items.Contains(attribute))
				{
					baseItem2 = Items[attribute];
					if (Items.IndexOf(attribute) != num)
					{
						Items.Remove(baseItem2);
						if (Items.Count > num)
						{
							Items.Insert(num, baseItem2);
						}
						else
						{
							Items.Add(baseItem2);
						}
					}
				}
				else
				{
					if (attribute2 != "" && dotNetBarManager != null && dotNetBarManager.Bars.Contains(attribute2) && dotNetBarManager.Bars[attribute2].Items.Contains(attribute))
					{
						baseItem2 = dotNetBarManager.Bars[attribute2].Items[attribute];
					}
					if (baseItem2 == null)
					{
						baseItem2 = dotNetBarManager.GetItem(attribute, FullSearch: true);
					}
					if (baseItem2 != null)
					{
						if (baseItem2.ContainerControl == null || (attribute2 == "" && !(baseItem2.ContainerControl is Bar)))
						{
							baseItem2 = baseItem2.Copy();
						}
						else
						{
							baseItem2.Parent.SubItems.Remove(baseItem2);
						}
						if (Items.Count > num)
						{
							Items.Insert(num, baseItem2);
						}
						else
						{
							Items.Add(baseItem2);
						}
					}
				}
				if (baseItem2 != null)
				{
					if (baseItem2.Visible != flag)
					{
						baseItem2.Visible = flag;
					}
					baseItem2.OriginalBarName = attribute2;
					baseItem2.OriginalPosition = originalPosition;
					baseItem2.UserCustomized = true;
				}
			}
		}
	}

	[Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
	public void SetSize(int width, int height)
	{
		if (_0B5E == eBarState.Docked)
		{
			ଡ଼._0B03 = height;
			ଡ଼.ර = width;
			_0C04();
			అ();
		}
		else
		{
			Size = new Size(width, height);
		}
		if (((Control)this).IsHandleCreated)
		{
			RecalcLayout();
		}
	}

	[Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
	public void SetSize(Size size)
	{
		SetSize(size.Width, size.Height);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackgroundImageAlpha()
	{
		return ޑ != byte.MaxValue;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFont()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Font != null)
		{
			if (((Control)this).Font.Name != SystemInformation.MenuFont.Name || ((Control)this).Font.Size != SystemInformation.MenuFont.Size || ((Control)this).Font.Style != SystemInformation.MenuFont.Style)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFont()
	{
		if (LayoutType == eLayoutType.DockContainer)
		{
			((Control)this).ResetFont();
			return;
		}
		_003F val = this;
		object obj = SystemInformation.MenuFont.Clone();
		((Control)val).Font = (Font)((obj is Font) ? obj : null);
	}

	internal void మ(eBarState య)
	{
		_0B5E = య;
		if (_0B5E != 0 && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
	}

	internal ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(Style) && GetRenderer() is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		if (_0AC3 is DotNetBarManager && ((DotNetBarManager)_0AC3).UseGlobalColorScheme)
		{
			return ((DotNetBarManager)_0AC3).ColorScheme;
		}
		return լ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		լ.Refresh();
		((Control)this).Invalidate();
	}

	internal void ర(bool ఱ)
	{
		if (ଡ଼ == null)
		{
			return;
		}
		try
		{
			if (ଡ଼.SubItems.Contains("dotnetbarsysiconitem"))
			{
				ଡ଼.SubItems.Remove("dotnetbarsysiconitem");
			}
			if (ଡ଼.SubItems.Contains("dotnetbarsysmenuitem"))
			{
				ଡ଼.SubItems.Remove("dotnetbarsysmenuitem");
			}
			if (ఱ)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ల(Form ళ, bool ఱ)
	{
		ర(ఱ);
		if (ళ != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysiconitem");
			mDISystemItem.Icon = ళ.Icon;
			if (!ళ.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!ళ.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			mDISystemItem.Click += ఴ;
			mDISystemItem.IsSystemIcon = true;
			ଡ଼.SubItems.Add(mDISystemItem, 0);
			mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!ళ.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!ళ.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += ఴ;
			ଡ଼.SubItems.Add(mDISystemItem);
			if (ఱ)
			{
				RecalcLayout();
			}
		}
	}

	private void ఴ(object _٠, EventArgs ؿ)
	{
		MDISystemItem mDISystemItem = _٠ as MDISystemItem;
		IOwner owner = _0AC3 as IOwner;
		Form val = null;
		if (owner != null)
		{
			val = owner.ActiveMdiChild;
		}
		if (val == null)
		{
			ర(ఱ: true);
		}
		else if (mDISystemItem._1C7E == _1886._1C8C)
		{
			_089F.PostMessage(((Control)val).Handle, 274, new IntPtr(61472), IntPtr.Zero);
		}
		else if (mDISystemItem._1C7E == _1886._1C8D)
		{
			_089F.PostMessage(((Control)val).Handle, 274, new IntPtr(61728), IntPtr.Zero);
		}
		else if (mDISystemItem._1C7E == _1886.ᆤ)
		{
			_089F.PostMessage(((Control)val).Handle, 274, new IntPtr(61536), IntPtr.Zero);
		}
		else if (mDISystemItem._1C7E == _1886.Ა)
		{
			_089F.PostMessage(((Control)val).Handle, 274, new IntPtr(61504), IntPtr.Zero);
		}
	}

	internal bool వ(int و, int ى)
	{
		Point pt = ((Control)this).PointToClient(new Point(و, ى));
		if (ୠ.Contains(pt) || (ஈ != null && ((Control)ஈ).Bounds.Contains(pt)))
		{
			return true;
		}
		return false;
	}

	internal void శ()
	{
		if (ଡ଼ != null)
		{
			ଡ଼.ContainerLostFocus(appLostFocus: true);
		}
		if (!((Control)this).IsDisposed)
		{
			((Control)this).AccessibilityNotifyClients((AccessibleEvents)32778, 0);
		}
	}

	internal void ష()
	{
		if (!((Control)this).IsDisposed)
		{
			((Control)this).AccessibilityNotifyClients((AccessibleEvents)32778, 0);
		}
	}

	internal void స()
	{
		if (_0BBA && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			_0BBA = false;
		}
		if (_0B5E == eBarState.Floating)
		{
			if (ୱ != null)
			{
				((Control)ୱ).Hide();
				((Control)this).OnVisibleChanged(new EventArgs());
			}
			return;
		}
		((Control)this).Hide();
		if (!_0B72 && !ஹ && ((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
		{
			((DockSite)(object)((Control)this).Parent).GetDocumentUIManager()._108A(this, _108C: true);
		}
		if (_0AC3 == null || !(_0AC3 is DotNetBarManager) || !((DotNetBarManager)_0AC3).IsDisposed)
		{
			RecalcLayout();
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
		Point location = val.PointToClient(Location);
		val.Controls.Add((Control)(object)this);
		Location = location;
		((Control)this).Show();
		((Control)this).Update();
		((Control)this).BringToFront();
		_0BBA = true;
		return true;
	}

	internal void _0C3A()
	{
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Invalid comparison between Unknown and I4
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Invalid comparison between Unknown and I4
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Invalid comparison between Unknown and I4
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Invalid comparison between Unknown and I4
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Invalid comparison between Unknown and I4
		if (ஏ)
		{
			((Control)this).Show();
			((Control)this).Update();
			return;
		}
		if (_0B5E == eBarState.Floating)
		{
			if (ୱ == null)
			{
				ୱ = new ᆡ(this);
				((Control)ୱ).CreateControl();
				if (((Control)this).Parent != null)
				{
					if (((Control)this).Parent is DockSite)
					{
						((DockSite)(object)((Control)this).Parent).ၹ((Control)(object)this);
					}
					else
					{
						((Control)this).Parent.Controls.Remove((Control)(object)this);
					}
				}
				((Control)this).Parent = null;
				((Control)ୱ).Controls.Add((Control)(object)this);
				((Control)this).Location = new Point(0, 0);
			}
			if (ୱ != null)
			{
				((Form)ୱ).TopLevel = true;
				((Control)ୱ).Visible = true;
				if (!((Control)this).Visible)
				{
					((Control)this).Visible = true;
				}
				((Control)ୱ).Refresh();
			}
		}
		else if (_0B5E == eBarState.Popup)
		{
			if (_0AF1 != null && ((_0AF1.Site != null && _0AF1.Site.DesignMode) || (_0AF1.Parent != null && _0AF1.Parent.Site != null && _0AF1.Parent.Site.DesignMode)) && హ())
			{
				return;
			}
			ePopupAnimation ePopupAnimation2 = ஃ;
			if (!BarFunctions.SupportsAnimation)
			{
				ePopupAnimation2 = ePopupAnimation.None;
			}
			else
			{
				IOwnerMenuSupport ownerMenuSupport = _0AC3 as IOwnerMenuSupport;
				if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
				{
					if (ownerMenuSupport != null)
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
			if (BarFunctions.IsOffice2007Style(Style) && BarType == eBarType.Toolbar)
			{
				_0BDD((Control)(object)this);
			}
			if (ePopupAnimation2 == ePopupAnimation.Fade && Environment.OSVersion.Version.Major >= 5)
			{
				_089F.AnimateWindow(((Control)this).Handle, 100, 524288);
			}
			else
			{
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.Slide:
					_089F.AnimateWindow(((Control)this).Handle, 100, 262149);
					break;
				case ePopupAnimation.Unfold:
					_089F.AnimateWindow(((Control)this).Handle, 100, 5);
					break;
				default:
					((Control)this).Show();
					break;
				}
			}
			if (ePopupAnimation2 != 0 && ((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				((Control)this).Refresh();
			}
			if (ଌ && _0B0D)
			{
				if (_0B84 != null)
				{
					((Control)_0B84).Hide();
					((Component)(object)_0B84).Dispose();
				}
				_0B84 = new ഓ(bAlphaShadow: true);
				((Control)_0B84).CreateControl();
				_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
				_0B84.ഖ();
			}
		}
		else
		{
			if (((Control)this).Parent is DockSite)
			{
				_0BA1 = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
			}
			else
			{
				_0BA1 = -1;
			}
			if (!_0B72 && !ஹ && ((Control)this).Parent is DockSite && (int)((Control)this).Parent.Dock != 5 && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
			{
				DockSite dockSite = ((Control)this).Parent as DockSite;
				ஹ = true;
				((Control)this).Show();
				ஹ = false;
				dockSite.GetDocumentUIManager()._108A(this, _108C: true);
				if (!ங)
				{
					DocumentBaseContainer documentFromBar = dockSite.GetDocumentUIManager().GetDocumentFromBar(this);
					if (documentFromBar != null && documentFromBar.Parent is DocumentDockContainer)
					{
						DocumentDockContainer documentDockContainer = documentFromBar.Parent as DocumentDockContainer;
						int visibleCount = documentDockContainer.Documents.VisibleCount;
						if (visibleCount > 1)
						{
							if (documentDockContainer.Orientation == eOrientation.Horizontal && ((int)((Control)dockSite).Dock == 1 || (int)((Control)dockSite).Dock == 2))
							{
								documentFromBar._10C9(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentDockContainer.LayoutBounds.Width / visibleCount, documentFromBar.LayoutBounds.Height));
							}
							else if (documentDockContainer.Orientation == eOrientation.Vertical && ((int)((Control)dockSite).Dock == 3 || (int)((Control)dockSite).Dock == 4))
							{
								documentFromBar._10C9(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentFromBar.LayoutBounds.Width, documentDockContainer.LayoutBounds.Height / visibleCount));
							}
						}
					}
				}
				if (Owner is DotNetBarManager { SuspendLayout: not false })
				{
					dockSite.NeedsLayout = true;
				}
			}
			else
			{
				((Control)this).Show();
			}
			_0BA1 = -1;
		}
		((Control)this).Update();
	}

	internal void _0C3B(DockContainerItem _06EC, eDockContainerClose ك, eDockContainerClose ل)
	{
		if (SelectedDockContainerItem == _06EC)
		{
			_0C3C();
		}
	}

	private void _0C3C()
	{
		bool closeVisible = ଔ;
		if (_0B5E == eBarState.Floating || (_0B5E == eBarState.Docked && (ୟ == eGrabHandleStyle.Caption || ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted)))
		{
			((Control)this).Invalidate(ୠ);
		}
		if (ஈ != null && ஈ._TabSystemBox != null)
		{
			ஈ._TabSystemBox.CloseVisible = closeVisible;
		}
	}

	protected virtual void OnCanMaximizeFloatingChanged(bool oldValue, bool newValue)
	{
		if (_0B5E == eBarState.Floating && (ୟ == eGrabHandleStyle.Caption || ୟ == eGrabHandleStyle.CaptionTaskPane || ୟ == eGrabHandleStyle.CaptionDotted))
		{
			((Control)this).Invalidate(ୠ);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return _0B80 != SystemColors.ControlDark;
	}

	private BaseItem ఽ()
	{
		foreach (BaseItem item in Items)
		{
			if (item.Visible)
			{
				return item;
			}
		}
		return null;
	}

	public void Show()
	{
		Visible = true;
	}

	public void Hide()
	{
		if (AutoHide)
		{
			AutoHide = false;
		}
		Visible = false;
		RecalcLayout();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void ResetBackColor()
	{
		if (ଡ଼ != null)
		{
			ଡ଼.BackColor = Color.Empty;
		}
		else
		{
			((Control)this).BackColor = SystemColors.Control;
		}
	}

	public bool ShouldSerializeBackColor()
	{
		if (ଡ଼ != null && !ଡ଼.m_BackgroundColor.IsEmpty)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSize()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (LayoutType == eLayoutType.DockContainer && ((Control)this).Parent is DockSite && (int)((Control)this).Parent.Dock == 5)
		{
			return false;
		}
		return true;
	}

	public BaseItem GetItem(string name)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(ଡ଼, name);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		if (_0BBD != null)
		{
			if (_0BBD is RibbonControl)
			{
				return ((RibbonControl)(object)_0BBD).RibbonStrip.GetItem(name);
			}
			if (_0BBD is DotNetBarManager)
			{
				return ((DotNetBarManager)_0BBD).GetItem(name);
			}
		}
		return null;
	}

	public ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		if (_0BBD != null)
		{
			if (_0BBD is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)_0BBD).RibbonStrip.GetItems(ItemName));
			}
			else if (_0BBD is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)_0BBD).GetItems(ItemName));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByName(ଡ଼, ItemName, arrayList);
		}
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		if (_0BBD != null)
		{
			if (_0BBD is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)_0BBD).RibbonStrip.GetItems(ItemName, itemType));
			}
			else if (_0BBD is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)_0BBD).GetItems(ItemName, itemType));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByNameAndType(ଡ଼, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		if (_0BBD != null)
		{
			if (_0BBD is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)_0BBD).RibbonStrip.GetItems(ItemName, itemType, useGlobalName));
			}
			else if (_0BBD is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)_0BBD).GetItems(ItemName, itemType, fullSearch: false, useGlobalName));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByNameAndType(ଡ଼, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public AutoHidePanel GetAutoHidePanel()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return null;
		}
		return _0C3E(_୬.DockSide);
	}

	private AutoHidePanel _0C3E(DockStyle _0C3F)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected I4, but got Unknown
		if (!(_0AC3 is IOwnerAutoHideSupport ownerAutoHideSupport))
		{
			return null;
		}
		AutoHidePanel autoHidePanel = null;
		DockStyle dockSide = _୬.DockSide;
		autoHidePanel = (dockSide - 1) switch
		{
			2 => ownerAutoHideSupport.LeftAutoHidePanel, 
			3 => ownerAutoHideSupport.RightAutoHidePanel, 
			0 => ownerAutoHideSupport.TopAutoHidePanel, 
			_ => ownerAutoHideSupport.BottomAutoHidePanel, 
		};
		if (AntiAlias)
		{
			autoHidePanel.AntiAlias = true;
		}
		autoHidePanel.Style = Style;
		return autoHidePanel;
	}

	private Size _0C40()
	{
		Size empty = Size.Empty;
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (subItem is DockContainerItem)
			{
				DockContainerItem dockContainerItem = subItem as DockContainerItem;
				if (dockContainerItem.MinimumSize.Width > empty.Width)
				{
					empty.Width = dockContainerItem.MinimumSize.Width;
				}
				if (dockContainerItem.MinimumSize.Height > empty.Height)
				{
					empty.Height = dockContainerItem.MinimumSize.Height;
				}
			}
		}
		return empty;
	}

	private Control _0C41()
	{
		Control result = null;
		IOwner owner = _0AC3 as IOwner;
		if (owner is DotNetBarManager && ((DotNetBarManager)owner).TopDockSite != null)
		{
			result = ((Control)((DotNetBarManager)owner).TopDockSite).Parent;
		}
		else if (owner != null)
		{
			result = (Control)(object)owner.ParentForm;
		}
		return result;
	}

	private void _0C42()
	{
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Invalid comparison between Unknown and I4
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Invalid comparison between Unknown and I4
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Invalid comparison between Unknown and I4
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Invalid comparison between Unknown and I4
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Invalid comparison between Unknown and I4
		if (!(_0AC3 is IOwner))
		{
			return;
		}
		Control val = _0C41();
		if (val == null)
		{
			return;
		}
		if (உ)
		{
			if (_0B5E == eBarState.AutoHide)
			{
				return;
			}
			if (ஈ != null)
			{
				((Control)ஈ).Visible = false;
			}
			DockSiteInfo dockSiteInfo = _୬;
			_୬ = new DockSiteInfo();
			_୬.LastRelativeDocumentId = dockSiteInfo.LastRelativeDocumentId;
			_୬.LastRelativeDockToBar = dockSiteInfo.LastRelativeDockToBar;
			if (!ங)
			{
				if (((Control)this).Height > 0)
				{
					_୬.DockedHeight = ((Control)this).Height;
				}
				else if (ଡ଼ != null && ଡ଼._0B03 > 0)
				{
					_୬.DockedHeight = ଡ଼._0B03 + 22;
				}
				if (((Control)this).Width > 0)
				{
					_୬.DockedWidth = ((Control)this).Width;
				}
				else if (ଡ଼ != null && ଡ଼.ර > 0)
				{
					_୬.DockedWidth = ଡ଼.ර + 22;
				}
				Size size = _0C40();
				if (size.Width > _୬.DockedWidth)
				{
					_୬.DockedWidth = 0;
				}
				if (size.Height > _୬.DockedHeight)
				{
					_୬.DockedHeight = 0;
				}
			}
			_୬.DockLine = DockLine;
			_୬.DockOffset = DockOffset;
			if (((Control)this).Parent != null && _0B5E == eBarState.Docked)
			{
				_୬.DockSide = ((Control)this).Parent.Dock;
			}
			else
			{
				_୬.DockSide = (DockStyle)3;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				_୬.InsertPosition = ((Control)(DockSite)(object)((Control)this).Parent).Controls.GetChildIndex((Control)(object)this);
				_୬.objDockSite = (DockSite)(object)DockedSite;
			}
			_0B5E = eBarState.AutoHide;
			_0C46();
			val.SuspendLayout();
			try
			{
				((Control)this).Visible = false;
				if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
				{
					DockSite dockSite = ((Control)this).Parent as DockSite;
					if (dockSite.DocumentDockContainer != null)
					{
						if ((int)((Control)dockSite).Dock != 5 && dockSite.GetDocumentUIManager().GetDocumentFromBar(this) is DocumentBarContainer documentBarContainer && documentBarContainer.Parent is DocumentDockContainer)
						{
							DocumentDockContainer documentDockContainer = documentBarContainer.Parent as DocumentDockContainer;
							if ((documentDockContainer.Orientation == eOrientation.Horizontal && ((int)((Control)dockSite).Dock == 1 || (int)((Control)dockSite).Dock == 2)) || (documentDockContainer.Orientation == eOrientation.Vertical && ((int)((Control)dockSite).Dock == 3 || (int)((Control)dockSite).Dock == 4) && documentDockContainer.Documents.Count > 1))
							{
								for (int i = 0; i < documentDockContainer.Documents.Count; i++)
								{
									if (documentDockContainer.Documents[i] is DocumentBarContainer && documentDockContainer.Documents[i].Visible && documentDockContainer.Documents[i] != documentBarContainer)
									{
										_୬.MouseOverBar = ((DocumentBarContainer)documentDockContainer.Documents[i]).Bar;
										if (i < documentDockContainer.Documents.IndexOf(documentBarContainer))
										{
											_୬.MouseOverDockSide = ((documentDockContainer.Orientation == eOrientation.Horizontal) ? eDockSide.Left : eDockSide.Bottom);
										}
										else
										{
											_୬.MouseOverDockSide = ((documentDockContainer.Orientation == eOrientation.Horizontal) ? eDockSide.Right : eDockSide.Top);
										}
										break;
									}
								}
							}
						}
						dockSite.GetDocumentUIManager().UnDock(this);
					}
					else
					{
						((DockSite)(object)((Control)this).Parent).ၹ((Control)(object)this);
					}
				}
				((Control)this).Parent = null;
				val.Controls.Add((Control)(object)this);
				val.Controls.SetChildIndex((Control)(object)this, 0);
				AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
				autoHidePanel.AddBar(this);
				if (!ங)
				{
					_0C43();
				}
				if (!ஜ && _0B9B != 0)
				{
					Visible = true;
				}
			}
			finally
			{
				val.ResumeLayout();
			}
			if (!ங)
			{
				((Control)this).Update();
				_0C45();
			}
			return;
		}
		AutoHidePanel autoHidePanel2 = _0C3E(_୬.DockSide);
		autoHidePanel2.RemoveBar(this);
		if (_୬.objDockSite != null)
		{
			if (_୬.MouseOverBar != null && (_୬.objDockSite.GetDocumentUIManager() == null || _୬.objDockSite.GetDocumentUIManager().GetDocumentFromBar(_୬.MouseOverBar) == null))
			{
				_୬.MouseOverBar = null;
				_୬.MouseOverDockSide = eDockSide.None;
				_୬.NewLine = true;
			}
		}
		else
		{
			_୬.MouseOverBar = null;
			_୬.MouseOverDockSide = eDockSide.None;
			_୬.NewLine = true;
		}
		DockingHandler(_୬, Point.Empty);
		if (!ଇ)
		{
			Visible = true;
		}
		ౘ(ౙ: true);
		_0C65();
		ଡ଼._0B03 = 0;
		ଡ଼.ර = 0;
	}

	internal void _0C43()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected I4, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected I4, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Invalid comparison between Unknown and I4
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Invalid comparison between Unknown and I4
		IOwner owner = _0AC3 as IOwner;
		IOwnerBarSupport ownerBarSupport = _0AC3 as IOwnerBarSupport;
		IOwnerAutoHideSupport ownerAutoHideSupport = _0AC3 as IOwnerAutoHideSupport;
		Control val = _0C41();
		if (owner == null || val == null || ownerBarSupport == null || ownerAutoHideSupport == null)
		{
			return;
		}
		int width = val.ClientSize.Width;
		int height = val.ClientSize.Height;
		int num = 0;
		int num2 = 0;
		DockStyle dockSide = _୬.DockSide;
		switch (dockSide - 1)
		{
		case 2:
		{
			width = ((Control)ownerBarSupport.LeftDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.LeftDockSite).Parent.Height;
			Point location4 = ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Location;
			num = location4.X + ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Width;
			num2 = location4.Y;
			break;
		}
		case 3:
		{
			width = ((Control)ownerBarSupport.RightDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.RightDockSite).Parent.Height;
			Point location3 = ((Control)ownerAutoHideSupport.RightAutoHidePanel).Location;
			num = location3.X;
			num2 = location3.Y;
			break;
		}
		case 0:
		{
			width = ((Control)ownerBarSupport.TopDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.TopDockSite).Parent.Height;
			Point location2 = ((Control)ownerAutoHideSupport.TopAutoHidePanel).Location;
			num = location2.X;
			num2 = location2.Y + ((Control)ownerAutoHideSupport.TopAutoHidePanel).Height;
			break;
		}
		default:
		{
			width = ((Control)ownerBarSupport.BottomDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.BottomDockSite).Parent.Height;
			Point location = ((Control)ownerAutoHideSupport.BottomAutoHidePanel).Location;
			num = location.X;
			num2 = location.Y;
			break;
		}
		}
		DockStyle dockSide2 = _୬.DockSide;
		switch (dockSide2 - 1)
		{
		case 2:
			height = ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Height;
			width = _୬.DockedWidth;
			if (width == 0)
			{
				int num6 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num6 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num6 = 0;
				}
				if (num6 >= 0)
				{
					width = Math.Max(((DockContainerItem)Items[num6]).MinimumSize.Width, ((DockContainerItem)Items[num6]).Width);
				}
				ૹ = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					ૹ = true;
				}
				width = ((Control)this).Width;
				_୬.DockedWidth = width;
			}
			break;
		case 3:
			height = ((Control)ownerAutoHideSupport.RightAutoHidePanel).Height;
			width = _୬.DockedWidth;
			if (width == 0)
			{
				int num4 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num4 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num4 = 0;
				}
				if (num4 >= 0)
				{
					width = Math.Max(((DockContainerItem)Items[num4]).MinimumSize.Width, ((DockContainerItem)Items[num4]).Width);
				}
				ૹ = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					ૹ = true;
				}
				width = ((Control)this).Width;
				_୬.DockedWidth = width;
			}
			num -= width;
			break;
		case 0:
			width = ((Control)ownerAutoHideSupport.TopAutoHidePanel).Width;
			height = _୬.DockedHeight;
			if (height == 0)
			{
				int num5 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num5 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num5 = 0;
				}
				if (num5 >= 0)
				{
					height = Math.Max(((DockContainerItem)Items[num5]).MinimumSize.Height, ((DockContainerItem)Items[num5]).Height);
				}
				ૹ = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					ૹ = true;
				}
				height = ((Control)this).Height;
				_୬.DockedHeight = height;
			}
			break;
		default:
			width = ((Control)ownerAutoHideSupport.BottomAutoHidePanel).Width;
			height = _୬.DockedHeight;
			if (height == 0)
			{
				int num3 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num3 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num3 = 0;
				}
				if (num3 >= 0)
				{
					height = Math.Max(((DockContainerItem)Items[num3]).MinimumSize.Height, ((DockContainerItem)Items[num3]).Height);
				}
				ૹ = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					ૹ = true;
				}
				height = ((Control)this).Height;
				_୬.DockedHeight = height;
				if (ஈ != null)
				{
					_୬.DockedHeight += ((Control)ஈ).Height;
				}
			}
			num2 -= height;
			break;
		}
		Rectangle rectangle = new Rectangle(num, num2, width, height);
		AutoHideDisplayEventArgs autoHideDisplayEventArgs = new AutoHideDisplayEventArgs(rectangle);
		if (_0B4D != null)
		{
			_0B4D(this, autoHideDisplayEventArgs);
			rectangle = autoHideDisplayEventArgs.DisplayRectangle;
		}
		if (ownerBarSupport != null)
		{
			ownerBarSupport.InvokeAutoHideDisplay(this, autoHideDisplayEventArgs);
			rectangle = autoHideDisplayEventArgs.DisplayRectangle;
		}
		((Control)this).Bounds = rectangle;
		RecalcSize();
		if ((int)_୬.DockSide == 2)
		{
			if (((Control)this).Height != height)
			{
				((Control)this).Top = ((Control)this).Top + (height - ((Control)this).Height);
			}
		}
		else if ((int)_୬.DockSide == 4 && ((Control)this).Width != width)
		{
			((Control)this).Left = ((Control)this).Left + (width - ((Control)this).Width);
		}
	}

	internal bool _0C44()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected I4, but got Unknown
		if (Visible)
		{
			return false;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeAutoHideDisplayed(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		_0C43();
		Rectangle rectangle = new Rectangle(Location, Size);
		Rectangle rectStart = rectangle;
		((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, 0);
		if (!ஜ && _0B9B > 0)
		{
			DockStyle dockSide = _୬.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				rectStart.Width = 1;
				break;
			case 3:
				rectStart.X = rectStart.Right - 1;
				rectStart.Width = 1;
				break;
			case 0:
				rectStart.Height = 1;
				break;
			default:
				rectStart.Y = rectStart.Bottom - 1;
				rectStart.Height = 1;
				break;
			}
		}
		try
		{
			ஞ = true;
			BarFunctions.AnimateControl((Control)(object)this, show: true, _0B9B, rectStart, rectangle);
		}
		finally
		{
			ஞ = false;
		}
		ஜ = false;
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideOpen));
		return true;
	}

	internal bool _0C45()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected I4, but got Unknown
		if (!Visible)
		{
			return true;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeAutoHideHidden(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, 0);
		Rectangle rectangle = new Rectangle(Location, Size);
		Rectangle rectEnd = rectangle;
		if (!ஜ && _0B9B > 0)
		{
			DockStyle dockSide = _୬.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				rectEnd.Width = 1;
				break;
			case 3:
				rectEnd.X = rectEnd.Right - 1;
				rectEnd.Width = 1;
				break;
			case 0:
				rectEnd.Height = 1;
				break;
			default:
				rectEnd.Y = rectEnd.Bottom - 1;
				rectEnd.Height = 1;
				break;
			}
			try
			{
				ஞ = true;
				BarFunctions.AnimateControl((Control)(object)this, show: false, _0B9B, rectangle, rectEnd);
			}
			finally
			{
				ஞ = false;
			}
		}
		else
		{
			Visible = false;
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideFolded));
		return true;
	}

	protected virtual void OnBeforeAutoHideDisplayed(CancelEventArgs e)
	{
		_0BBE?.Invoke(this, e);
	}

	protected virtual void OnBeforeAutoHideHidden(CancelEventArgs e)
	{
		_0BBF?.Invoke(this, e);
	}

	private void _0C46()
	{
		IOwner owner = _0AC3 as IOwner;
		if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
		{
			((ContainerControl)owner.ParentForm).ActiveControl = null;
			((Control)this).Focus();
		}
		else if (owner.ParentForm != null && _0BF2((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
		{
			((ContainerControl)owner.ParentForm).ActiveControl = null;
			((Control)this).Focus();
		}
	}

	private void _0C47(object _٠, EventArgs ؿ)
	{
		if (!_0B82._1B71)
		{
			_0C4B();
		}
		else
		{
			ఘ();
		}
	}

	private void _0C48(object _٠, EventArgs ؿ)
	{
		if (!_0B82._1B6C)
		{
			_0C4B();
		}
		else
		{
			ఘ();
		}
	}

	private void _0C49(object _٠, EventArgs ؿ)
	{
		if (!_0B82._1B69)
		{
			_0C4B();
		}
		else
		{
			ఘ();
		}
	}

	private void _0C4A(object _٠, EventArgs ؿ)
	{
		_0C4B();
	}

	private void _0C4B()
	{
		if (m_ToolTipWnd != null)
		{
			((Control)m_ToolTipWnd).Hide();
			((Component)(object)m_ToolTipWnd).Dispose();
			m_ToolTipWnd = null;
			ఘ();
		}
	}

	internal void _0C4C()
	{
		_0C4B();
		if (ଡ଼ == null)
		{
			return;
		}
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			_0C4D(subItem);
		}
	}

	private void _0C4D(BaseItem _06EC)
	{
		_06EC.HideToolTip();
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			_0C4D(subItem);
		}
	}

	private void _0C4E(string _0C4F)
	{
		if (!((Component)this).DesignMode && _০ && _0C4F != null && !(_0C4F == "") && Visible)
		{
			if (m_ToolTipWnd == null)
			{
				m_ToolTipWnd = new ToolTip();
			}
			m_ToolTipWnd.Text = _0C4F;
			if (Owner is IOwnerItemEvents ownerItemEvents)
			{
				ownerItemEvents.InvokeToolTipShowing(m_ToolTipWnd, new EventArgs());
			}
			m_ToolTipWnd.ShowToolTip();
		}
	}

	private void _0C50()
	{
		if (ஈ != null && BarFunctions.IsHandleValid((Control)(object)this))
		{
			if (ஈ.CanReorderTabs != _0B79)
			{
				ஈ.CanReorderTabs = _0B79;
			}
			if (ஈ.TabAlignment != ஔ)
			{
				ஈ.TabAlignment = ஔ;
			}
			if (_0B97)
			{
				ஈ.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
			}
			else
			{
				ஈ.TabLayoutType = eTabLayoutType.FitContainer;
			}
			ஈ.ShowFocusRectangle = false;
			if (ஈ._TabSystemBox != null)
			{
				ஈ._TabSystemBox.CloseVisible = ଔ;
			}
			if (உ)
			{
				((Control)ஈ).Visible = false;
			}
		}
	}

	internal void _0C51()
	{
		if (ஈ == null && BarFunctions.IsHandleValid((Control)(object)this))
		{
			ஈ = new TabStrip();
			_0C50();
			SuspendLayout();
			_0C65();
			ஈ.SelectedTabChanging += _౦;
			ஈ.SelectedTabChanged += _0C52;
			ஈ.BeforeTabDisplay += _0C55;
			ஈ.TabMoved += _౭;
			ஈ.TabItemClose += _0C53;
			ஈ.CloseButtonOnTabsVisible = _0BAD;
			((Control)this).Controls.Add((Control)(object)ஈ);
			_0C54(ଡ଼.EffectiveStyle);
			((Control)ஈ).Visible = _0C63();
			_0C64();
			((Control)this).Invalidate();
			ResumeLayout();
		}
	}

	private void _0C52(object _٠, TabStripTabChangedEventArgs ؿ)
	{
		_0C3C();
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.SelectedDockTabChanged, SelectedDockContainerItem));
	}

	private void _0C53(object _٠, TabStripActionEventArgs ؿ)
	{
		if (_٠ is TabItem { AttachedItem: DockContainerItem attachedItem })
		{
			CloseDockTab(attachedItem, eEventSource.Mouse);
		}
	}

	internal void _0C54(eDotNetBarStyle _0657)
	{
		if (ஈ == null)
		{
			return;
		}
		if (_0657 == eDotNetBarStyle.StyleManagerControlled)
		{
			_0657 = StyleManager.GetEffectiveStyle();
		}
		if (StyleManager.IsMetro(_0657))
		{
			ஈ.Style = eTabStripStyle.Metro;
		}
		else
		{
			switch (_0657)
			{
			case eDotNetBarStyle.Office2003:
			{
				IOwnerBarSupport ownerBarSupport3 = _0AC3 as IOwnerBarSupport;
				if (DockSide == eDockSide.Document && ownerBarSupport3 != null && ownerBarSupport3.ApplyDocumentBarStyle)
				{
					ஈ.Style = eTabStripStyle.OneNote;
				}
				else
				{
					ஈ.Style = eTabStripStyle.Office2003;
				}
				break;
			}
			case eDotNetBarStyle.VS2005:
			{
				IOwnerBarSupport ownerBarSupport2 = _0AC3 as IOwnerBarSupport;
				if (DockSide == eDockSide.Document && ownerBarSupport2 != null && ownerBarSupport2.ApplyDocumentBarStyle)
				{
					ஈ.Style = eTabStripStyle.VS2005Document;
				}
				else
				{
					ஈ.Style = eTabStripStyle.VS2005;
				}
				break;
			}
			default:
				if (BarFunctions.IsOffice2007Style(_0657))
				{
					IOwnerBarSupport ownerBarSupport = _0AC3 as IOwnerBarSupport;
					if (DockSide == eDockSide.Document && ownerBarSupport != null && ownerBarSupport.ApplyDocumentBarStyle)
					{
						ஈ.Style = eTabStripStyle.Office2007Document;
					}
					else
					{
						ஈ.Style = eTabStripStyle.Office2007Dock;
					}
				}
				else
				{
					ஈ.Style = eTabStripStyle.Flat;
				}
				break;
			}
		}
		ஈ.ColorScheme.TabBorder = Color.Empty;
		if (_0B54 != null)
		{
			_0B54(this, new EventArgs());
		}
	}

	private void _0C55(object _٠, EventArgs ؿ)
	{
		if (_٠ is TabItem)
		{
			_౬(((TabItem)_٠).AttachedItem);
		}
	}

	private void _0C56()
	{
		if (ஈ != null)
		{
			if (ஈ.ᓦ)
			{
				((Control)ஈ).Visible = false;
				ஈ.Tabs.Clear();
			}
			else
			{
				((Control)this).Controls.Remove((Control)(object)ஈ);
				((Component)(object)ஈ).Dispose();
				ஈ = null;
			}
		}
	}

	internal void _0C57()
	{
		if (ஈ != null)
		{
			((Control)ஈ).Refresh();
		}
	}

	internal void ౘ(bool ౙ)
	{
		if (ଡ଼ == null || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		_౧();
		int visibleSubItems = ଡ଼.VisibleSubItems;
		if (visibleSubItems > 1 || (_0BA0 && _0B5E != eBarState.Floating))
		{
			if (ஈ == null)
			{
				_0C51();
				return;
			}
			if (!((Control)ஈ).Visible && _0B5E != eBarState.AutoHide)
			{
				((Control)ஈ).Visible = _0C63();
			}
			BaseItem baseItem = null;
			if (ౙ && ஈ != null && ஈ.SelectedTab != null)
			{
				baseItem = ஈ.SelectedTab.AttachedItem;
			}
			_0C64();
			if (ౙ && ஈ != null && ஈ.SelectedTab != null && baseItem != ஈ.SelectedTab.AttachedItem)
			{
				_౩(baseItem, ஈ.SelectedTab.AttachedItem);
			}
			return;
		}
		if (visibleSubItems == 1 && ஈ != null)
		{
			DockContainerItem dockContainerItem = null;
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				if (subItem.Visible && subItem is DockContainerItem)
				{
					dockContainerItem = subItem as DockContainerItem;
					break;
				}
			}
			if (dockContainerItem != null && ౙ)
			{
				_౩(null, dockContainerItem);
			}
		}
		if (((Component)this).DesignMode)
		{
			if (ஈ != null)
			{
				((Control)ஈ).Visible = false;
			}
		}
		else
		{
			_0C56();
		}
	}

	internal void ౚ()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return;
		}
		AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
		if (autoHidePanel != null)
		{
			autoHidePanel.RefreshBar(this);
			if (autoHidePanel.ᆧ != null)
			{
				_౩(null, autoHidePanel.ᆧ);
			}
		}
	}

	internal void _0C5B(DockContainerItem _0C5C)
	{
		if (!AutoHide || _0C5C.Visible)
		{
			return;
		}
		bool flag = true;
		foreach (BaseItem item in Items)
		{
			if (item.Visible && flag)
			{
				item.Displayed = true;
				flag = false;
			}
			else
			{
				item.Displayed = false;
			}
		}
	}

	public void ReDock()
	{
		if (_0B5E == eBarState.Floating && (CanDockBottom || CanDockTop || CanDockLeft || CanDockRight || CanDockDocument))
		{
			DockingHandler(_୬, Point.Empty);
		}
	}

	internal void _0C5D(BaseItem ਤ)
	{
		if (ਤ is DockContainerItem && LayoutType == eLayoutType.DockContainer)
		{
			if (ਤ.Displayed)
			{
				bool flag = false;
				foreach (BaseItem item in Items)
				{
					if (item.Displayed)
					{
						flag = true;
					}
				}
				if (!flag && Items.Count > 0)
				{
					Items[0].Displayed = true;
				}
			}
			ౘ(ౙ: true);
			if (AutoHide)
			{
				GetAutoHidePanel()?.RefreshBar(this);
			}
		}
		_౧();
	}

	protected void InvokeCaptionButtonClick()
	{
		if (_0B4F != null)
		{
			_0B4F(this, new EventArgs());
		}
	}

	private void _0C5E()
	{
		if (ண != null)
		{
			bool expanded = ண.Expanded;
			if (expanded)
			{
				ண.ClosePopup();
			}
			ண.Dispose();
			ண = null;
			if (expanded)
			{
				return;
			}
		}
		if (Items.Count <= 0)
		{
			return;
		}
		ண = new ButtonItem("sysCaptionButtonMenuParent");
		ண.Style = Style;
		ண.PopupShowing += ౠ;
		ண.PopupFinalized += ౡ;
		foreach (BaseItem item in Items)
		{
			ButtonItem buttonItem = new ButtonItem("sysCaption_" + item.Name);
			buttonItem.Tag = item;
			buttonItem.OptionGroup = "sysCaptionMenu";
			buttonItem.Text = item.Text;
			buttonItem.BeginGroup = item.BeginGroup;
			if (item is DockContainerItem)
			{
				DockContainerItem dockContainerItem = item as DockContainerItem;
				buttonItem.Checked = item.Displayed;
				if (dockContainerItem.Image != null)
				{
					object obj = dockContainerItem.Image.Clone();
					buttonItem.Image = (Image)((obj is Image) ? obj : null);
				}
				else if (dockContainerItem.Icon != null)
				{
					object obj2 = dockContainerItem.Icon.Clone();
					buttonItem.Icon = (Icon)((obj2 is Icon) ? obj2 : null);
				}
				else if (dockContainerItem.ImageIndex >= 0 && dockContainerItem.ImageList != null)
				{
					object obj3 = dockContainerItem.ImageList.Images[dockContainerItem.ImageIndex].Clone();
					buttonItem.Image = (Image)((obj3 is Image) ? obj3 : null);
				}
			}
			else
			{
				buttonItem.Checked = item.Visible;
			}
			buttonItem.Click += _0C5F;
			ண.SubItems.Add(buttonItem);
		}
		if (ண.GetOwner() == null)
		{
			ண.SetOwner(_0AC3);
		}
		Size popupSize = ண.PopupSize;
		Point point = new Point(_0B82._1B76.Right - popupSize.Width, _0B82._1B76.Bottom);
		if (point.X < 0)
		{
			point.X = 0;
		}
		point = ((Control)this).PointToScreen(point);
		ண.Popup(point);
	}

	private void _0C5F(object _٠, EventArgs ؿ)
	{
		if (!(_٠ is ButtonItem buttonItem))
		{
			return;
		}
		if (LayoutType == eLayoutType.DockContainer)
		{
			SelectedDockTab = Items.IndexOf(buttonItem.Tag as BaseItem);
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item == buttonItem.Tag)
			{
				item.Visible = true;
			}
			else
			{
				item.Visible = false;
			}
		}
		RecalcLayout();
		ண.Dispose();
		ண = null;
	}

	private void ౠ(object _٠, EventArgs ؿ)
	{
		if (ண != null)
		{
			((MenuPanel)(object)ண.PopupControl).ColorScheme = GetColorScheme();
		}
	}

	private void ౡ(object _٠, EventArgs ؿ)
	{
		_0BD8();
	}

	internal void _0C62(DockContainerItem _06EC)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		if (ଡ଼ == null || ஈ == null)
		{
			_౧();
			return;
		}
		foreach (TabItem tab in ஈ.Tabs)
		{
			if (tab.AttachedItem == _06EC)
			{
				tab.Text = _06EC.Text;
				tab.AttachedItem = _06EC;
				tab.Tooltip = _06EC.Tooltip;
				tab.Name = _06EC.Name;
				if (_06EC.Icon != null)
				{
					object obj = _06EC.Icon.Clone();
					tab.Icon = (Icon)((obj is Icon) ? obj : null);
				}
				else
				{
					tab.Image = _06EC._10CE;
					tab.Icon = null;
				}
				break;
			}
		}
		((Control)ஈ).Refresh();
		if (AutoHide)
		{
			AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
			if (autoHidePanel != null)
			{
				((Control)autoHidePanel).Refresh();
			}
		}
		_౧();
	}

	private bool _0C63()
	{
		if ((ୟ == eGrabHandleStyle.CaptionTaskPane && _0BA2) || !__0BC0)
		{
			return false;
		}
		return true;
	}

	protected virtual void OnDockTabStripHeightChanged(int oldValue, int newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			RecalcLayout();
		}
	}

	private void _0C64()
	{
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		if (_0BC2)
		{
			return;
		}
		if (!((Control)ஈ).Visible && _0B5E != eBarState.AutoHide)
		{
			((Control)ஈ).Visible = _0C63();
			int num = Dpi.Height(DockTabStripHeight);
			if (IsThemed && _0B5E == eBarState.Floating)
			{
				((Control)ஈ).Size = new Size(((Control)this).Width - (ஐ._0749 + ஐ.ሪ), num);
				((Control)ஈ).Location = new Point(ஐ._0749, ((Control)this).Height - num - ஐ.ራ);
			}
			else
			{
				((Control)ஈ).Size = new Size(((Control)this).Width - Dpi.Width4, num);
				((Control)ஈ).Location = new Point(Dpi.Width2, ((Control)this).Height - num - Dpi.Height2);
			}
		}
		_0BC2 = true;
		try
		{
			ஈ.Tabs.Clear();
			int num2 = 0;
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				if (!subItem.Visible)
				{
					continue;
				}
				num2++;
				TabItem tabItem = new TabItem();
				tabItem.Text = subItem.Text;
				tabItem.Tooltip = subItem.Tooltip;
				tabItem.Name = subItem.Name;
				if (subItem is DockContainerItem)
				{
					DockContainerItem dockContainerItem = subItem as DockContainerItem;
					if (dockContainerItem.Icon != null)
					{
						object obj = dockContainerItem.Icon.Clone();
						tabItem.Icon = (Icon)((obj is Icon) ? obj : null);
					}
					else
					{
						tabItem.Image = dockContainerItem._10CE;
					}
					tabItem.PredefinedColor = dockContainerItem.PredefinedTabColor;
				}
				ஈ.Tabs.Add(tabItem);
				tabItem.AttachedItem = subItem;
				if (subItem.Displayed)
				{
					ஈ._ᔩ = true;
					ஈ.SelectedTab = tabItem;
					ஈ._ᔩ = false;
				}
			}
			if (num2 > 1 || (num2 == 1 && _0BA0 && _0B5E != eBarState.Floating))
			{
				if (!((Control)ஈ).Visible && _0B5E != eBarState.AutoHide)
				{
					((Control)ஈ).Visible = _0C63();
				}
				if (ஈ.SelectedTab == null)
				{
					ஈ.SelectedTab = ஈ.Tabs[0];
				}
				else if (ஈ.SelectedTab.AttachedItem != null && !ஈ.SelectedTab.AttachedItem.Displayed)
				{
					ஈ.SelectedTab.AttachedItem.Displayed = true;
				}
				((Control)ஈ).Refresh();
			}
			else if (((Control)ஈ).Visible)
			{
				((Control)ஈ).Visible = false;
			}
		}
		finally
		{
			_0BC2 = false;
		}
		if (_0B5E == eBarState.AutoHide)
		{
			_0C3E(_୬.DockSide)?.RefreshBar(this);
		}
	}

	private void _0C65()
	{
		if (ஈ == null || !((Control)ஈ).Visible)
		{
			return;
		}
		Point point = Point.Empty;
		Size size = Size.Empty;
		int num = Dpi.Height(DockTabStripHeight);
		switch (ஔ)
		{
		case eTabStripAlignment.Top:
			if (ୟ != 0 && !ୠ.IsEmpty)
			{
				point = new Point(ୠ.X, ୠ.Bottom);
				size = new Size(((Control)this).Width - ୠ.Left * 2, num);
			}
			else if (_0B5E == eBarState.Docked)
			{
				point = new Point(Dpi.Width1, Dpi.Width1);
				size = new Size(((Control)this).Width - Dpi.Width2, num);
			}
			else if (_0B5E == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(ஐ._0749, ஐ._074A);
					size = new Size(((Control)this).Width - (ஐ._0749 + ஐ.ሪ), num);
				}
				else
				{
					point = new Point(Dpi.Width3, Dpi.Height3);
					size = new Size(((Control)this).Width - Dpi.Width6, num);
				}
			}
			else
			{
				((Control)ஈ).Visible = false;
			}
			break;
		case eTabStripAlignment.Left:
			if (ୟ != 0 && !ୠ.IsEmpty)
			{
				point = new Point(ୠ.X + 1, ୠ.Bottom + Dpi.Height1);
				size = new Size(num, ଡ଼.HeightInternal - Dpi.Height1);
			}
			else if (_0B5E == eBarState.Docked)
			{
				point = new Point(Dpi.Width2, Dpi.Height1);
				size = new Size(num, ((Control)this).Height - Dpi.Height3);
			}
			else if (_0B5E == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(ஐ._0749, ஐ._074A);
					size = new Size(((Control)this).Width - (ஐ._0749 + ஐ.ሪ), num);
				}
				else
				{
					point = new Point(Dpi.Width3, Dpi.Height3);
					size = new Size(((Control)this).Width - Dpi.Width6, num);
				}
			}
			else
			{
				((Control)ஈ).Visible = false;
			}
			break;
		case eTabStripAlignment.Right:
			if (ୟ != 0 && !ୠ.IsEmpty && _0B5E == eBarState.Docked)
			{
				point = new Point(((Control)this).ClientRectangle.Right - num, ୠ.Bottom + Dpi.Height1);
				size = new Size(num - Dpi.Width2, ଡ଼.HeightInternal);
			}
			else if (_0B5E == eBarState.Docked)
			{
				point = new Point(((Control)this).Width - num, Dpi.Height1);
				size = new Size(num, ((Control)this).Height - Dpi.Height2);
			}
			else if (_0B5E == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(((Control)this).Width - ஐ.ሪ - num, ஐ._074A);
					size = new Size(num, ((Control)this).Height - (ஐ._074A + ஐ.ራ));
				}
				else
				{
					point = new Point(((Control)this).Width - Dpi.Width3 - num, ଡ଼.TopInternal);
					size = new Size(num, ଡ଼.HeightInternal);
				}
			}
			else
			{
				((Control)ஈ).Visible = false;
			}
			break;
		default:
			point = new Point(0, ((Control)this).Height - num);
			size = new Size(((Control)this).Width, num);
			if (DockSide == eDockSide.Top && _0B5E == eBarState.Docked)
			{
				point = new Point(0, ((Control)this).Height - num - Dpi.Height1);
				size = new Size(((Control)this).Width, num);
			}
			if (_0B5E == eBarState.Docked)
			{
				if (_0B7F != 0)
				{
					point = new Point(Dpi.Width2, ((Control)this).Height - num - Dpi.Height3);
					size = new Size(((Control)this).Width - Dpi.Width4, num);
				}
			}
			else if (_0B5E == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(ஐ._0749, ((Control)this).Height - num - ஐ.ሪ);
					size = new Size(((Control)this).Width - (ஐ._0749 + ஐ.ሪ), num);
				}
				else
				{
					point = new Point(3, ((Control)this).Height - num - Dpi.Height3);
					size = new Size(((Control)this).Width - Dpi.Width6, num);
				}
			}
			else
			{
				((Control)ஈ).Visible = false;
			}
			break;
		}
		((Control)ஈ).SetBounds(point.X, point.Y, size.Width, size.Height);
	}

	private void _౦(object _٠, TabStripTabChangingEventArgs ؿ)
	{
		if (!_0BC2)
		{
			ؿ.Cancel = _౩((ؿ.OldTab == null) ? null : ؿ.OldTab.AttachedItem, (ؿ.NewTab == null) ? null : ؿ.NewTab.AttachedItem);
		}
	}

	private void _౧(BaseItem _౨)
	{
		if (!த || LayoutType != eLayoutType.DockContainer)
		{
			return;
		}
		DockContainerItem selectedDockContainerItem = SelectedDockContainerItem;
		if ((selectedDockContainerItem != null && selectedDockContainerItem.Visible) || _౨ != null)
		{
			if (_౨ == null)
			{
				_౨ = selectedDockContainerItem;
			}
			((Control)this).Text = _౨.Text;
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item.Visible && item.Displayed)
			{
				((Control)this).Text = item.Text;
				break;
			}
		}
	}

	internal void _౧()
	{
		_౧(null);
	}

	private bool _౩(BaseItem _౪, BaseItem _౫)
	{
		DockTabChangeEventArgs dockTabChangeEventArgs = null;
		IOwnerBarSupport ownerBarSupport = _0AC3 as IOwnerBarSupport;
		bool flag = false;
		if (_0B49 != null)
		{
			dockTabChangeEventArgs = new DockTabChangeEventArgs(_౪, _౫);
			_0B49(this, dockTabChangeEventArgs);
			flag = dockTabChangeEventArgs.Cancel;
			if (flag)
			{
				return flag;
			}
		}
		if (_0AC3 != null)
		{
			if (dockTabChangeEventArgs == null)
			{
				dockTabChangeEventArgs = new DockTabChangeEventArgs(_౪, _౫);
			}
			ownerBarSupport?.InvokeDockTabChange(this, dockTabChangeEventArgs);
			flag = dockTabChangeEventArgs.Cancel;
		}
		if (!flag)
		{
			_౧(_౫);
			_0C3C();
		}
		if (!flag)
		{
			OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.SelectedDockTabChanging, _౫));
		}
		return flag;
	}

	internal void _౬(BaseItem _06EC)
	{
		if (_0B4E != null)
		{
			_0B4E(_06EC, new EventArgs());
		}
		if (_0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBeforeDockTabDisplay(_06EC, new EventArgs());
		}
	}

	private void _౭(object _٠, TabStripTabMovedEventArgs ؿ)
	{
		List<BaseItem> list = new List<BaseItem>();
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (!subItem.Visible)
			{
				list.Add(subItem);
			}
		}
		foreach (BaseItem item in list)
		{
			ଡ଼.SubItems._Remove(item);
		}
		ଡ଼.SubItems._Remove((DockContainerItem)ؿ.Tab.AttachedItem);
		ଡ଼.SubItems.__0AF3((DockContainerItem)ؿ.Tab.AttachedItem, ؿ.NewIndex);
		foreach (BaseItem item2 in list)
		{
			ଡ଼.SubItems.__0AF3(item2);
		}
		_0BA6 = true;
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.DockTabMoved, ؿ.Tab.AttachedItem));
	}

	public Size MinimumDockSize(eOrientation dockOrientation)
	{
		if (ଡ଼ == null)
		{
			return new Size(32, 32);
		}
		if (ଡ଼.LayoutType == eLayoutType.DockContainer && (ଡ଼.Stretch || _0B70))
		{
			if (ଡ଼.SubItems.Count > 0 && ଡ଼.SubItems[0] is DockContainerItem)
			{
				return ((DockContainerItem)ଡ଼.SubItems[0]).MinimumSize;
			}
			return new Size(32, 32);
		}
		if (ଡ଼.Stretch || _0B70)
		{
			return PreferredDockSize(dockOrientation);
		}
		if (ଡ଼.SubItems.Count > 0)
		{
			BaseItem baseItem = ଡ଼.SubItems[0];
			if (baseItem != null)
			{
				return new Size(baseItem.WidthInternal + _0B59.Left * 2, baseItem.HeightInternal);
			}
		}
		return new Size(0, 0);
	}

	internal Size _౮(Size ߒ)
	{
		Size result = ߒ;
		if (ஈ != null && ((Control)ஈ).Visible && (DockTabAlignment == eTabStripAlignment.Top || DockTabAlignment == eTabStripAlignment.Bottom))
		{
			result.Height += ((Control)ஈ).Height;
		}
		result.Height += 6 + SystemInformation.ToolWindowCaptionHeight;
		result.Width += 6;
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int GetBarDockedSize(eOrientation o)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		int width = Dpi.Width6;
		if (ଡ଼ == null)
		{
			return 0;
		}
		if (o == eOrientation.Horizontal && _୬.DockedHeight > 0 && ((int)_୬.DockSide == 1 || (int)_୬.DockSide == 2))
		{
			return _୬.DockedHeight;
		}
		if (o == eOrientation.Vertical && _୬.DockedWidth > 0 && ((int)_୬.DockSide == 3 || (int)_୬.DockSide == 4))
		{
			return _୬.DockedWidth;
		}
		if (o == eOrientation.Horizontal && ଡ଼._0B03 > 0 && ଡ଼._0B03 + SystemInformation.ToolWindowCaptionHeight < _0C03())
		{
			return ଡ଼._0B03 + SystemInformation.ToolWindowCaptionHeight;
		}
		if (o == eOrientation.Vertical && ଡ଼.ර > 0 && ଡ଼.ර + SystemInformation.ToolWindowCaptionHeight < _0C02())
		{
			return ଡ଼.ර + SystemInformation.ToolWindowCaptionHeight;
		}
		if (SelectedDockTab >= 0 || (VisibleItemCount > 0 && LayoutType == eLayoutType.DockContainer))
		{
			DockContainerItem dockContainerItem = null;
			dockContainerItem = ((SelectedDockTab < 0) ? (ఽ() as DockContainerItem) : (Items[SelectedDockTab] as DockContainerItem));
			if (dockContainerItem != null)
			{
				if (o == eOrientation.Horizontal)
				{
					if (dockContainerItem.MinimumSize.Height > 0)
					{
						int num = ((Control)this).Height;
						if ((((Control)this).Parent != null && (DockSide == eDockSide.Left || DockSide == eDockSide.Right)) || (((Control)this).Parent == null && (ଙ == eDockSide.Left || ଙ == eDockSide.Right)))
						{
							num = ((Control)this).Width;
						}
						if (num > dockContainerItem.MinimumSize.Height)
						{
							return num;
						}
						return dockContainerItem.MinimumSize.Height * 2 + Dpi.Height(DockTabStripHeight) + width;
					}
				}
				else if (dockContainerItem.MinimumSize.Width > 0)
				{
					int num2 = ((Control)this).Width;
					if ((((Control)this).Parent != null && (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom)) || (((Control)this).Parent == null && (ଙ == eDockSide.Top || ଙ == eDockSide.Bottom)))
					{
						num2 = ((Control)this).Height;
					}
					if (num2 > dockContainerItem.MinimumSize.Width)
					{
						return num2;
					}
					return dockContainerItem.MinimumSize.Width * 2 + width;
				}
			}
		}
		if (o == eOrientation.Horizontal)
		{
			return ((Control)this).Height;
		}
		return ((Control)this).Width;
	}

	public Size PreferredDockSize(eOrientation dockOrientation)
	{
		IOwner owner = _0AC3 as IOwner;
		if (dockOrientation == eOrientation.Horizontal)
		{
			if (_୧.IsEmpty)
			{
				if (owner != null && owner.ParentForm != null)
				{
					_୧ = _௧(owner.ParentForm.Size, eOrientation.Horizontal, eBarState.Docked, _୭);
				}
				else
				{
					_୧ = _௧(Screen.FromControl((Control)(object)this).WorkingArea.Size, eOrientation.Horizontal, eBarState.Docked, _୭);
				}
			}
			return _୧;
		}
		if (_୨.IsEmpty)
		{
			Size empty = Size.Empty;
			empty = ((owner == null || owner.ParentForm == null) ? _௧(Screen.FromControl((Control)(object)this).WorkingArea.Size, eOrientation.Horizontal, eBarState.Docked, _୭) : _௧(owner.ParentForm.Size, eOrientation.Horizontal, eBarState.Docked, _୭));
			_୨ = new Size(empty.Height, empty.Width);
		}
		return _୨;
	}

	protected override void OnDockChanged(EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnDockChanged(e);
		if ((int)((Control)this).Dock != 0)
		{
			_0B70 = true;
		}
		else
		{
			_0B70 = false;
		}
		RecalcLayout();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDockLine(int iLine)
	{
		_0B62 = iLine;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		DotNetBarManager dotNetBarManager = Owner as DotNetBarManager;
		bool flag = false;
		if (dotNetBarManager != null)
		{
			flag = dotNetBarManager.EnableDockCtrlTabKey;
		}
		if (flag && LayoutType == eLayoutType.DockContainer && VisibleItemCount > 1 && (keyData & 0x20000) == 131072 && (keyData & 9) == 9 && _0EFE._0F0D(_0EFE.GetKeyState(9)) != 0)
		{
			if ((keyData & 0x10000) == 65536)
			{
				_౯(_0C70: true);
			}
			else
			{
				_0C71(_0C70: true);
			}
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	internal bool _౯(bool _0C70)
	{
		int num = -1;
		if (SelectedDockTab <= 0)
		{
			if (!_0C70)
			{
				return false;
			}
			num = _0C72(Items.Count, -1);
		}
		else
		{
			num = _0C72(SelectedDockTab, -1);
			if (num < 0 && _0C70)
			{
				num = _0C72(Items.Count, -1);
			}
		}
		if (num < 0)
		{
			return false;
		}
		SelectedDockTab = num;
		return true;
	}

	internal bool _0C71(bool _0C70)
	{
		int num = -1;
		if (SelectedDockTab >= Items.Count - 1)
		{
			if (!_0C70)
			{
				return false;
			}
			num = _0C72(-1, 1);
		}
		else
		{
			num = _0C72(SelectedDockTab, 1);
			if (num < 0 && _0C70)
			{
				num = _0C72(-1, 1);
			}
		}
		if (num < 0)
		{
			return false;
		}
		SelectedDockTab = num;
		return true;
	}

	private int _0C72(int _0C73, int _0C74)
	{
		int num = _0C73;
		int num2 = Items.Count - 1;
		int num3 = 1;
		if (_0C74 < 0)
		{
			num2 = 0;
			num3 = -1;
			if (_0C73 <= 0)
			{
				return -1;
			}
		}
		else if (_0C73 >= num2)
		{
			return -1;
		}
		while (num2 != num)
		{
			num += num3;
			if (Items[num] is DockContainerItem { Visible: not false })
			{
				return num;
			}
		}
		return -1;
	}

	public bool ShouldSerializeDockSide()
	{
		if (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
		{
			return false;
		}
		return DockSide != eDockSide.None;
	}

	private void _0C75(eDockSide _५)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected I4, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return;
		}
		DockStyle val = (DockStyle)3;
		switch (_५)
		{
		case eDockSide.Bottom:
			val = (DockStyle)2;
			break;
		case eDockSide.Right:
			val = (DockStyle)4;
			break;
		case eDockSide.Top:
			val = (DockStyle)1;
			break;
		}
		if (_୬.DockSide == val || _५ == eDockSide.None)
		{
			return;
		}
		AutoHidePanel autoHidePanel = _0C3E(_୬.DockSide);
		if (autoHidePanel != null && _0AC3 is IOwnerBarSupport ownerBarSupport)
		{
			_0C45();
			autoHidePanel.RemoveBar(this);
			_୬.DockSide = val;
			DockStyle val2 = val;
			switch (val2 - 1)
			{
			case 2:
				_୬.objDockSite = ownerBarSupport.LeftDockSite;
				break;
			case 3:
				_୬.objDockSite = ownerBarSupport.RightDockSite;
				break;
			case 0:
				_୬.objDockSite = ownerBarSupport.TopDockSite;
				break;
			case 1:
				_୬.objDockSite = ownerBarSupport.BottomDockSite;
				break;
			}
			autoHidePanel = _0C3E(val);
			autoHidePanel.AddBar(this);
		}
	}

	private void ਣ(BaseItem ਤ)
	{
		if (ਤ != null && ਤ.Parent is PopupItem)
		{
			return;
		}
		if (_09D8 != null)
		{
			if (_09D8.Expanded)
			{
				_09D8.Expanded = false;
			}
			_09D8 = null;
		}
		_09D8 = ਤ;
	}

	void IOwner.SetExpandedItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਣ
		this.ਣ(ਤ);
	}

	private BaseItem ਥ()
	{
		return _09D8;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਥ
		return this.ਥ();
	}

	private void ਦ(BaseItem ਧ)
	{
		if (DockSide == eDockSide.Document && _0AC3 != null)
		{
			((IOwner)_0AC3).SetFocusItem(ਧ);
			return;
		}
		if (_09D9 != null && _09D9 != ਧ)
		{
			_09D9.OnLostFocus();
		}
		_09D9 = ਧ;
		if (_09D9 != null)
		{
			_09D9.OnGotFocus();
		}
		if (_0BC5 != null)
		{
			_0BC5(this, new EventArgs());
		}
	}

	void IOwner.SetFocusItem(BaseItem ਧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਦ
		this.ਦ(ਧ);
	}

	private BaseItem ਨ()
	{
		if (DockSide == eDockSide.Document && _0AC3 != null && _0AC3 is IOwner)
		{
			return ((IOwner)_0AC3).GetFocusItem();
		}
		return _09D9;
	}

	BaseItem IOwner.GetFocusItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਨ
		return this.ਨ();
	}

	private void _0A29(BaseItem ਤ)
	{
	}

	void IOwner.DesignTimeContextMenu(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਩
		this._0A29(ਤ);
	}

	private void ਪ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (!_09DA.ContainsKey(shortcut))
				{
					continue;
				}
				ᯣ2 = (ᯣ)_09DA[shortcut];
				try
				{
					ᯣ2.ᯥ.Remove(ਤ.Id);
					if (ᯣ2.ᯥ.Count == 0)
					{
						_09DA.Remove(ᯣ2.ᯤ);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).RemoveShortcutsFromItem(subItem);
		}
	}

	void IOwner.RemoveShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਪ
		this.ਪ(ਤ);
	}

	private void ਫ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (_09DA.ContainsKey(shortcut))
				{
					ᯣ2 = (ᯣ)_09DA[ਤ.Shortcuts[0]];
				}
				else
				{
					ᯣ2 = new ᯣ(shortcut);
					_09DA.Add(ᯣ2.ᯤ, ᯣ2);
				}
				try
				{
					ᯣ2.ᯥ.Add(ਤ.Id, ਤ);
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).AddShortcutsFromItem(subItem);
		}
	}

	void IOwner.AddShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਫ
		this.ਫ(ਤ);
	}

	private MdiClient ਵ(Form ਸ਼)
	{
		return BarFunctions.GetMdiClient(ਸ਼);
	}

	MdiClient IOwner.GetMdiClient(Form ਸ਼)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਵ
		return this.ਵ(ਸ਼);
	}

	private void ਭ()
	{
	}

	void IOwner.Customize()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਭ
		this.ਭ();
	}

	private void ٶ(object _٠, EventArgs ؿ)
	{
		if (_٠ == գ)
		{
			Images = null;
		}
		else if (_٠ == ড়)
		{
			ImagesLarge = null;
		}
		else if (_٠ == _09DB)
		{
			ImagesMedium = null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void BeginInit()
	{
		ஹ = true;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndInit()
	{
		ஹ = false;
		if (DockSide == eDockSide.Document && ((Control)this).Parent is DockSite)
		{
			((DockSite)(object)((Control)this).Parent).RecalcLayout();
		}
		else
		{
			RecalcSize();
		}
		if (AutoHide != _0BBC)
		{
			AutoHide = _0BBC;
		}
	}

	private void ਮ(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwner.InvokeResetDefinition(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਮ
		this.ਮ(_06EC, ؿ);
	}

	private void ਲ਼(object _٠, EventArgs ؿ)
	{
	}

	void IOwner.InvokeUserCustomize(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ਼
		this.ਲ਼(_٠, ؿ);
	}

	private void _0A34(object _٠, EndUserCustomizeEventArgs ؿ)
	{
	}

	void IOwner.InvokeEndUserCustomize(object _٠, EndUserCustomizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਴
		this._0A34(_٠, ؿ);
	}

	private void _0C77(BaseItem _06EC)
	{
		if (((Component)this).DesignMode && _09FD != null)
		{
			_09FD.StartExternalDrag(_06EC);
		}
	}

	void IOwner.StartItemDrag(BaseItem _06EC)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ౷
		this._0C77(_06EC);
	}

	private void ਰ()
	{
	}

	void IOwner.OnApplicationActivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਰ
		this.ਰ();
	}

	private void _0A31()
	{
		ClosePopups();
	}

	void IOwner.OnApplicationDeactivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਱
		this._0A31();
	}

	private void ਲ()
	{
	}

	void IOwner.OnParentPositionChanging()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ
		this.ਲ();
	}

	private void ੴ()
	{
		if (_৪)
		{
			return;
		}
		_৪ = true;
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			_৪ = false;
			return;
		}
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		((Control)val).Resize += _0A76;
		val.Deactivate += _0A77;
		DotNetBarManager.RegisterParentMsgHandler(this, val);
	}

	private void _0A75()
	{
		if (!_৪)
		{
			return;
		}
		_৪ = false;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if (val.IsMdiChild && val.MdiParent != null)
			{
				val = val.MdiParent;
			}
			DotNetBarManager.UnRegisterParentMsgHandler(this, val);
			((Control)val).Resize -= _0A76;
			val.Deactivate -= _0A77;
		}
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void _0A77(object _٠, EventArgs ؿ)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void _0A3C(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			return;
		}
		if (!((Component)this).DesignMode && !__0BC6)
		{
			if (!_২)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
			if (!_৪)
			{
				ੴ();
			}
		}
		else if (_0A0E == null)
		{
			_0A0E = new ᒺ(this);
		}
		if (ਏ.Count == 0)
		{
			PopupManager.RegisterPopup(this);
		}
		ਏ.Add(_0A3D);
		if (_0A3D.GetOwner() != this)
		{
			_0A3D.SetOwner(this);
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਼
		this._0A3C(_0A3D);
	}

	private void _0A3E(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			ਏ.Remove(_0A3D);
		}
		if (ਏ.Count == 0)
		{
			_0A75();
			if (_0A0E != null)
			{
				_0A0E.Dispose();
				_0A0E = null;
			}
			PopupManager.UnregisterPopup(this);
		}
	}

	void IOwnerMenuSupport.UnregisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਾ
		this._0A3E(_0A3D);
	}

	private bool _0A3F()
	{
		foreach (PopupItem item in ਏ)
		{
			Control popupControl = item.PopupControl;
			if (popupControl != null && popupControl.DisplayRectangle.Contains(Control.MousePosition))
			{
				if (popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).ఆ();
				}
				else if (popupControl is Bar)
				{
					((Bar)(object)popupControl).ఆ();
				}
				return true;
			}
		}
		return false;
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਿ
		return this._0A3F();
	}

	private void _0A49()
	{
		ClosePopups();
	}

	void IOwnerMenuSupport.ClosePopups()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੉
		this._0A49();
	}

	internal void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			if (item.ತ(eEventSource.Mouse))
			{
				item.ClosePopup();
			}
		}
	}

	private void _0A40(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C1 != null)
		{
			_09C1(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupClose(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੀ
		this._0A40(_06EC, ؿ);
	}

	private void _0A41(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BE != null)
		{
			_09BE(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੁ
		this._0A41(_06EC, ؿ);
	}

	private void _0A42(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BF != null)
		{
			_09BF(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੂ
		this._0A42(_06EC, ؿ);
	}

	private void _0A43(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		if (_09C0 != null)
		{
			_09C0(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupOpen(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੃
		this._0A43(_06EC, ؿ);
	}

	private void _0A44(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C2 != null)
		{
			_09C2(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੄
		this._0A44(_06EC, ؿ);
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected I4, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		bool designMode = ((Component)this).DesignMode;
		int num = _0EFE._0F11(_੦);
		if (ਏ.Count > 0)
		{
			BaseItem baseItem = (BaseItem)ਏ[ਏ.Count - 1];
			Keys val = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
			if ((int)val == 0)
			{
				val = (Keys)num;
			}
			if (baseItem.Parent == null || IsContextPopup(baseItem) || (int)val == 27 || (int)val == 13 || (int)val == 37 || (int)val == 39 || (int)val == 40 || (int)val == 38)
			{
				PopupItem popupItem = (PopupItem)ਏ[ਏ.Count - 1];
				Control popupControl = popupItem.PopupControl;
				Control val2 = Control.FromChildHandle(_0A65);
				if (val2 != null)
				{
					while (val2.Parent != null)
					{
						val2 = val2.Parent;
					}
				}
				bool flag = false;
				if (val2 != null && popupItem != null)
				{
					flag = popupItem.IsAnyOnHandle(val2.Handle);
				}
				bool flag2 = (popupControl != null && val2 != null && popupControl.Handle == val2.Handle) || flag;
				if (!flag)
				{
					popupItem.InternalKeyDown((KeyEventArgs)(object)new KeyEventArgsEx(val, _0EFE.ༀ((uint)(int)_੦)));
				}
				if (flag2)
				{
					return false;
				}
				return !designMode;
			}
		}
		Form val3 = ((Control)this).FindForm();
		if (val3 == null || (val3 != Form.ActiveForm && val3.MdiParent == null) || (val3.MdiParent != null && val3.MdiParent.ActiveMdiChild != val3))
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (_0EFE._0F11(_੧) & 0x1000000000L) != 0 || num == 46 || num == 45)
		{
			int _੬ = Control.ModifierKeys | num;
			if (_੫((eShortcut)_੬))
			{
				return !designMode;
			}
			return false;
		}
		return false;
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	protected virtual bool IsContextPopup(BaseItem popup)
	{
		return false;
	}

	private bool _੫(eShortcut _੬)
	{
		if (!ଟ)
		{
			return false;
		}
		bool result = BarFunctions.ProcessItemsShortcuts(_੬, _09DA);
		if (!_৩)
		{
			return result;
		}
		return false;
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (ਏ.Count == 0 || ((Component)this).DesignMode)
		{
			return false;
		}
		for (int num = ਏ.Count - 1; num >= 0; num--)
		{
			PopupItem popupItem = ਏ[num] as PopupItem;
			object containerControl = popupItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			bool flag = popupItem.IsAnyOnHandle(_0A65);
			if (!flag)
			{
				Control val2 = Control.FromChildHandle(_0A65);
				if (val2 != null)
				{
					if (val2 is MenuPanel)
					{
						flag = true;
					}
					else
					{
						while (val2.Parent != null)
						{
							val2 = val2.Parent;
							if (((object)val2).GetType().FullName.IndexOf("DropDownHolder") >= 0 || val2 is MenuPanel || val2 is PopupContainerControl)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						flag = popupItem.IsAnyOnHandle(val2.Handle);
					}
				}
				else
				{
					string text = _089F.स(_0A65);
					text = text.ToLower();
					if (text.IndexOf("combolbox") >= 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Control val3 = popupItem.PopupControl;
				if (val3 != null)
				{
					while (val3.Parent != null)
					{
						val3 = val3.Parent;
					}
				}
				if (val3 != null && val3.Bounds.Contains(Control.MousePosition))
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			if (val != null && _0A65 != val.Handle && !flag)
			{
				if (popupItem.Expanded && popupItem.ತ(eEventSource.Mouse))
				{
					popupItem.Expanded = !popupItem.Expanded;
					if (!popupItem.Expanded && popupItem.Parent != null)
					{
						popupItem.Parent.AutoExpand = false;
					}
				}
			}
			else if (val == null && !flag && popupItem.ತ(eEventSource.Mouse))
			{
				popupItem.ClosePopup();
			}
			if (ਏ.Count == 0)
			{
				break;
			}
		}
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (ਏ.Count > 0)
		{
			foreach (BaseItem item in ਏ)
			{
				if (item.Parent == null)
				{
					Control popupControl = ((PopupItem)item).PopupControl;
					if (popupControl != null && popupControl.Handle != _0A65 && !item.IsAnyOnHandle(_0A65) && (popupControl.Parent == null || !(popupControl.Parent.Handle != _0A65)))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (!MenuBar && __0BCA)
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible && item.GetEnabled() && item.Displayed && item.AccessKeyEnabled && item.AccessKey == charCode)
				{
					item.RaiseClick(eEventSource.Keyboard);
					return true;
				}
			}
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected virtual bool InternalSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Invalid comparison between Unknown and I4
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected I4, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected I4, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		int num = _0EFE._0F11(wParam);
		if (num == 18 && (int)Control.ModifierKeys == 262144)
		{
			ClosePopups();
		}
		Form val = ((Control)this).FindForm();
		if (val == null || !BarFunctions.IsFormActive(val))
		{
			return false;
		}
		Bar bar = null;
		if (MenuBar)
		{
			bar = this;
		}
		if (bar != null && bar.ItemsContainer != null && !bar.ItemsContainer.DesignMode)
		{
			GenericItemContainer itemsContainer = bar.ItemsContainer;
			if (itemsContainer == null)
			{
				return false;
			}
			if (num == 18 || (num == 121 && !_0BC9 && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 262144)))
			{
				if (itemsContainer.ExpandedItem() != null && ((Control)bar).Focused)
				{
					bar.ReleaseFocus();
					_0BC7 = true;
					return true;
				}
			}
			else
			{
				if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
				{
					int _੬ = Control.ModifierKeys | num;
					if (_੫((eShortcut)_੬))
					{
						return true;
					}
				}
				_0BC7 = true;
				if (num >= 27 && num <= 111)
				{
					int num2 = (int)_089F.MapVirtualKey((uint)(int)wParam, 2u);
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 > 0 && itemsContainer._෫(num2))
					{
						return true;
					}
				}
			}
		}
		else if (bar == null && !((Component)this).DesignMode && ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123)))
		{
			int _੬2 = Control.ModifierKeys | num;
			if (_੫((eShortcut)_੬2))
			{
				return true;
			}
		}
		if ((int)Control.ModifierKeys == 262144 && num > 27)
		{
			_0BC7 = true;
			int num3 = (int)_089F.MapVirtualKey((uint)(int)wParam, 2u);
			if (num3 != 0 && !((Component)this).DesignMode && ଡ଼._෫(num3))
			{
				return true;
			}
		}
		return false;
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return InternalSysKeyDown(_0A65, _੦, _੧);
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		Form val = ((Control)this).FindForm();
		if (val == null || ((Component)this).DesignMode || !BarFunctions.IsFormActive(val))
		{
			return false;
		}
		int num = _0EFE._0F11(_੦);
		if (num == 18 || num == 121)
		{
			if (_0BC7)
			{
				_0BC7 = false;
				return false;
			}
			if (_0BC8)
			{
				_0BC8 = false;
				return true;
			}
			if (num == 18 || (num == 121 && !_0BC9))
			{
				Bar bar = null;
				if (MenuBar)
				{
					bar = this;
				}
				if (bar != null && !bar.ItemsContainer.DesignMode)
				{
					if (((Control)bar).Focused)
					{
						bar.ReleaseFocus();
					}
					else
					{
						bar.ௐ();
					}
					return true;
				}
			}
		}
		return false;
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	private Size _0C7F(eOrientation ಀ)
	{
		return MinimumDockSize(ಀ);
	}

	Size IDockInfo.MinimumDockSize(eOrientation ಀ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ౿
		return this._0C7F(ಀ);
	}

	private Size _0C81(eOrientation ಀ)
	{
		return PreferredDockSize(ಀ);
	}

	Size IDockInfo.PreferredDockSize(eOrientation ಀ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ಁ
		return this._0C81(ಀ);
	}

	private void ಉ(int ಊ)
	{
		SetDockLine(ಊ);
	}

	void IDockInfo.SetDockLine(int ಊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ಉ
		this.ಉ(ಊ);
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
		if (_0AF1 != null && _0AF1.ContainerControl is Control && (val == null || (val != null && ((Component)(object)val).Site == null)))
		{
			object containerControl = _0AF1.ContainerControl;
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
		if (site == null && _0AF1 != null)
		{
			BaseItem parent = _0AF1;
			while (site == null && parent != null)
			{
				if (parent.Site != null && parent.Site.DesignMode)
				{
					site = parent.Site;
				}
				else
				{
					parent = parent.Parent;
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
		if (_0AC3 is IOwner)
		{
			ঢ় = ((IOwner)_0AC3).GetFocusItem();
		}
		if (ঢ় != null)
		{
			BaseItem parent = _0AF1;
			while (parent.Parent is IDesignTimeProvider)
			{
				parent = parent.Parent;
			}
			_0BCC = (IDesignTimeProvider)parent;
			_0BCB = true;
			((Control)this).Capture = true;
		}
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
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		IOwner owner = Owner as IOwner;
		BaseItem baseItem = ଡ଼.ItemAtLocation(ؿ.X, ؿ.Y);
		if (baseItem != null && baseItem.SystemItem)
		{
			baseItem = null;
		}
		if (baseItem == null)
		{
			return;
		}
		if (owner != null)
		{
			owner.SetFocusItem(baseItem);
			ISite site = ಌ();
			if (site != null)
			{
				ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(baseItem);
					selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
				}
				if ((int)ؿ.Button == 2097152)
				{
					((IMenuCommandService)site.GetService(typeof(IMenuCommandService)))?.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 1280), Control.MousePosition.X, Control.MousePosition.Y);
				}
			}
		}
		owner = null;
		baseItem?.InternalMouseDown(ؿ);
	}

	private void ઌ(LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ઌ
		this.ઌ(ؿ);
	}
}
