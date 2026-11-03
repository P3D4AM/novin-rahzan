using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class RadialMenuContainer : BaseItem
{
	private class ᨓ
	{
		public GraphicsPath ᙳ;

		public GraphicsPath ᨔ;

		public int ᨕ;

		public int ᨖ;

		public ᨓ(GraphicsPath path, GraphicsPath signPath, int itemAngle, int itemPieAngle)
		{
			ᙳ = path;
			ᨔ = signPath;
			ᨕ = itemAngle;
			ᨖ = itemPieAngle;
		}

		public void _1A17()
		{
			if (ᙳ != null)
			{
				ᙳ.Dispose();
			}
			if (ᨔ != null)
			{
				ᨔ.Dispose();
			}
			ᙳ = null;
		}
	}

	private class _2462 : PopupItem
	{
		internal RadialMenuContainer __2463;

		private bool __223A;

		public override bool Expanded
		{
			get
			{
				return __223A;
			}
			set
			{
				if (__223A != value)
				{
					__223A = value;
					if (!__223A && __2463.Expanded)
					{
						__2463.Expanded = false;
					}
				}
			}
		}

		public _2462(RadialMenuContainer parentRadialMenu)
		{
			__2463 = parentRadialMenu;
		}

		protected internal override bool IsAnyOnHandle(IntPtr iHandle)
		{
			if (__2463.__19ED != null && ((Control)__2463.__19ED).Handle == iHandle)
			{
				return true;
			}
			return false;
		}

		public override void Paint(ItemPaintArgs p)
		{
		}

		public override BaseItem Copy()
		{
			return new _2462(__2463);
		}
	}

	private const int _19E3 = 18;

	private EventHandler _19E4;

	private EventHandler _19E5;

	private SizeF __19E6 = new SizeF(1f, 1f);

	private ᨓ[] __19E7;

	private bool __19E8;

	private Rectangle __19E9 = Rectangle.Empty;

	private int __19EA;

	private int __19EB = 100;

	private _1A18 __19EC;

	private RadialMenuPopup __19ED;

	private bool __19EE;

	private _2462 __19EF;

	private int __19F0;

	private int __19F1;

	private int __19F2 = 180;

	private int __19F3 = 32;

	private int __19F4 = 18;

	private int __19F5 = 3;

	private Point __19F6 = Point.Empty;

	private RadialMenu __19F7;

	private _2488 __19F8;

	private int __19F9 = -1;

	private int __19FA;

	private int __19FB = 18;

	private string __19FC = "\uf060";

	private eSymbolSet __19FD;

	private int __19FE = 90;

	private RadialMenuColorTable _ቿ;

	private eRadialMenuType __19FF;

	private Size _ᨀ = Size.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ;

	private Image _ነ;

	private Font _ݐ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SizeF ScaleFactor
	{
		get
		{
			return __19E6;
		}
		set
		{
			__19E6 = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int OpenState
	{
		get
		{
			return __19EA;
		}
		set
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			MethodInvoker val = null;
			RadialMenuPopup radialMenuPopup = __19ED;
			if (base.IsDisposed || radialMenuPopup == null || (radialMenuPopup != null && ((Control)radialMenuPopup).IsDisposed))
			{
				__19EA = value;
				return;
			}
			if (radialMenuPopup != null && ((Control)radialMenuPopup).InvokeRequired && !((Control)radialMenuPopup).IsDisposed)
			{
				try
				{
					if (val == null)
					{
						val = (MethodInvoker)delegate
						{
							OpenState = value;
						};
					}
					((Control)radialMenuPopup).Invoke((Delegate)(object)val);
					return;
				}
				catch (ObjectDisposedException)
				{
					return;
				}
			}
			if (value < 0)
			{
				value = 0;
			}
			else if (value > 100)
			{
				value = 100;
			}
			if (value != __19EA)
			{
				int oldValue = __19EA;
				__19EA = value;
				OnOpenStateChanged(oldValue, value);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int InnerScale
	{
		get
		{
			return __19EB;
		}
		set
		{
			if (value > 100)
			{
				value = 100;
			}
			else if (value < 0)
			{
				value = 0;
			}
			if (__19EB != value)
			{
				__19EB = value;
				Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[Browsable(false)]
	public RadialMenuPopup Popup => __19ED;

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates maximum radial angle single item in menu can consume. Max value is 180.")]
	public int MaxItemRadialAngle
	{
		get
		{
			return __19F1;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			else if (value > 180)
			{
				value = 180;
			}
			if (value != __19F1)
			{
				int oldValue = __19F1;
				__19F1 = value;
				OnMaxItemRadialAngleChanged(oldValue, value);
			}
		}
	}

	private int _19DF => (int)((float)__19F2 * __19E6.Height);

	[Description("Radial menu diameter.")]
	[DefaultValue(180)]
	[Category("Appearance")]
	public int Diameter
	{
		get
		{
			return __19F2;
		}
		set
		{
			if (value < 64)
			{
				value = 64;
			}
			if (value != __19F2)
			{
				int oldValue = __19F2;
				__19F2 = value;
				OnDiameterChanged(oldValue, value);
			}
		}
	}

	private int _19E0 => (int)((float)__19F3 * __19E6.Height);

	[Description("Indicates diameter of center button of radial menu.")]
	[DefaultValue(32)]
	[Category("Appearance")]
	public int CenterButtonDiameter
	{
		get
		{
			return __19F3;
		}
		set
		{
			if (value != __19F3)
			{
				int oldValue = __19F3;
				__19F3 = value;
				OnCenterButtonDiameterChanged(oldValue, value);
			}
		}
	}

	[Description("Specifies the width of the sub-menu edge around the radial menu.")]
	[Category("Appearance")]
	[DefaultValue(18)]
	public int SubMenuEdgeWidth
	{
		get
		{
			return __19F4;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value != __19F4)
			{
				int oldValue = __19F4;
				__19F4 = value;
				OnSubMenuEdgeWidthChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates spacing between sub-menu marking edge and the item.")]
	[DefaultValue(3)]
	[Category("Appearance")]
	public int SubMenuEdgeItemSpacing
	{
		get
		{
			return __19F5;
		}
		set
		{
			if (value != __19F5)
			{
				int oldValue = __19F5;
				__19F5 = value;
				OnSubMenuEdgeItemSpacingChanged(oldValue, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates the position of top-left corner of radial menu when shown in screen coordinates")]
	public Point MenuLocation
	{
		get
		{
			return __19F6;
		}
		set
		{
			if (value != __19F6)
			{
				Point oldValue = __19F6;
				__19F6 = value;
				OnMenuLocationChanged(oldValue, value);
			}
		}
	}

	internal RadialMenu _19E1
	{
		get
		{
			return __19F7;
		}
		set
		{
			if (value != __19F7)
			{
				RadialMenu oldValue = __19F7;
				__19F7 = value;
				OnRadialMenuChanged(oldValue, value);
			}
		}
	}

	private int _19E2
	{
		get
		{
			return __19F9;
		}
		set
		{
			if (__19F9 != value)
			{
				__19F9 = value;
				Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SubMenuEdgeWidthAnimated
	{
		get
		{
			return __19FA;
		}
		set
		{
			if (__19FA != value)
			{
				__19FA = value;
				Refresh();
			}
		}
	}

	[DefaultValue(18)]
	[Browsable(false)]
	[Description("Indicates the size of the back symbol that is displayed on center of radial menu")]
	public int BackButtonSymbolSize
	{
		get
		{
			return __19FB;
		}
		set
		{
			if (value != __19FB)
			{
				int oldValue = __19FB;
				__19FB = value;
				OnBackSymbolSizeChanged(oldValue, value);
			}
		}
	}

	[DefaultValue("\uf060")]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Specifies the back button symbol.")]
	public string BackButtonSymbol
	{
		get
		{
			return __19FC;
		}
		set
		{
			if (value != __19FC)
			{
				string oldValue = __19FC;
				__19FC = value;
				OnBackSymbolChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet BackButtonSymbolSet
	{
		get
		{
			return __19FD;
		}
		set
		{
			if (__19FD != value)
			{
				eSymbolSet oldValue = __19FD;
				__19FD = value;
				OnBackButtonSymbolSetChanged(oldValue, value);
			}
		}
	}

	[Description("Specifies the maximum pie part angle an item will occupy. Maximum is 180, minimum is 1 degree.")]
	[DefaultValue(90)]
	[Category("Appearance")]
	public int MaxItemPieAngle
	{
		get
		{
			return __19FE;
		}
		set
		{
			if (value < 0)
			{
				value = 90;
			}
			if (value > 180)
			{
				value = 180;
			}
			if (value != __19FE)
			{
				int oldValue = __19FE;
				__19FE = value;
				OnMaxItemPieAngleChanged(oldValue, value);
			}
		}
	}

	[Description("Gets reference to colors used by the radial menu.")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RadialMenuColorTable Colors => _ቿ;

	[Description("Indicates the radial menu type.")]
	[Category("Appearance")]
	[DefaultValue(eRadialMenuType.Segment)]
	public eRadialMenuType MenuType
	{
		get
		{
			return __19FF;
		}
		set
		{
			if (value != __19FF)
			{
				eRadialMenuType oldValue = __19FF;
				__19FF = value;
				OnMenuTypeChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0f)]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates image displayed on the item.")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[Description("Specifies font for menu items.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Font Font
	{
		get
		{
			return _ݐ;
		}
		set
		{
			if (value != _ݐ)
			{
				Font ݐ = _ݐ;
				_ݐ = value;
				OnFontChanged(ݐ, value);
			}
		}
	}

	[Description("Occurs after radial menu is opened.")]
	public event EventHandler Opened
	{
		add
		{
			EventHandler eventHandler = _19E4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _19E4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _19E4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _19E4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after radial menu is closed.")]
	public event EventHandler Closed
	{
		add
		{
			EventHandler eventHandler = _19E5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _19E5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _19E5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _19E5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public RadialMenuContainer()
	{
		_ቿ = new RadialMenuColorTable();
		_ቿ.PropertyChanged += ᨁ;
		m_IsContainer = true;
		AutoCollapseOnClick = false;
		AccessibleRole = (AccessibleRole)20;
	}

	private void ᨁ(object _٠, PropertyChangedEventArgs ؿ)
	{
		Invalidate();
	}

	public override BaseItem Copy()
	{
		RadialMenuContainer radialMenuContainer = new RadialMenuContainer();
		CopyToItem(radialMenuContainer);
		return radialMenuContainer;
	}

	internal void _0EBA(RadialMenuContainer _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		RadialMenuContainer radialMenuContainer = copy as RadialMenuContainer;
		radialMenuContainer.BackButtonSymbol = __19FC;
		radialMenuContainer.BackButtonSymbolSize = __19FB;
		radialMenuContainer.BackButtonSymbolSet = __19FD;
		radialMenuContainer.CenterButtonDiameter = __19F3;
		radialMenuContainer.Colors.Apply(_ቿ);
		radialMenuContainer.Diameter = __19F2;
		radialMenuContainer.MenuLocation = __19F6;
		radialMenuContainer.MenuType = __19FF;
		radialMenuContainer.SubMenuEdgeItemSpacing = __19F5;
		radialMenuContainer.SubMenuEdgeWidth = __19F4;
	}

	protected override void Dispose(bool disposing)
	{
		if (__19ED != null)
		{
			((Component)(object)__19ED).Dispose();
			__19ED = null;
		}
		if (__19EF != null)
		{
			__19EF.__2463 = null;
			__19EF.Dispose();
			__19EF = null;
		}
		if (__19EC != null)
		{
			__19EC.Stop();
		}
		base.Dispose(disposing);
	}

	protected virtual void OnOpened(EventArgs e)
	{
		_19E4?.Invoke(this, e);
	}

	protected virtual void OnClosed(EventArgs e)
	{
		_19E5?.Invoke(this, e);
	}

	public override void RecalcSize()
	{
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		if (__19E8)
		{
			return;
		}
		__19E8 = true;
		try
		{
			ᨇ();
			int num = _19DF;
			m_Rect = new Rectangle(0, 0, num, num);
			Rectangle rectangle = new Rectangle(LeftInternal, TopInternal, num, num);
			rectangle.Width--;
			rectangle.Height--;
			Rectangle rectangle2 = rectangle;
			Rectangle rectangle3 = rectangle;
			int num2 = _19E0;
			rectangle3.Inflate(-((num - num2) / 2 + 1), -((num - num2) / 2 + 1));
			int num3 = Dpi.Width(__19F5);
			if (__19FF == eRadialMenuType.Segment)
			{
				int num4 = Dpi.Width(__19F4);
				rectangle.Inflate(-num4, -num4);
				rectangle.Inflate(-num3, -num3);
			}
			SubItemsCollection subItemsCollection = ᨂ();
			int num5 = ᨅ(subItemsCollection);
			if (num5 > 0)
			{
				int num6 = __19F1;
				if (num6 == 0)
				{
					num6 = 180;
				}
				int num7 = Math.Max(18, Math.Min(num6, 360 / num5));
				if (num7 > __19FE)
				{
					num7 = __19FE;
				}
				int num8 = -90 - num7 / 2;
				__19E7 = new ᨓ[subItemsCollection.Count];
				for (int i = 0; i < subItemsCollection.Count; i++)
				{
					BaseItem baseItem = subItemsCollection[i];
					if (!baseItem.Visible)
					{
						continue;
					}
					if (baseItem is RadialMenuItem radialMenuItem)
					{
						radialMenuItem.RecalcSize();
						GraphicsPath val = new GraphicsPath();
						val.AddArc(rectangle, (float)num8, (float)num7);
						Rectangle rectangle4 = rectangle3;
						rectangle4.Inflate(4, 4);
						val.AddArc(rectangle4, (float)(num8 + num7), (float)(-num7));
						val.CloseAllFigures();
						radialMenuItem.DisplayPath = val;
						radialMenuItem._248B = rectangle;
						radialMenuItem._248C = rectangle3;
						radialMenuItem.ᨕ = num8;
						radialMenuItem.ᨖ = num7;
						radialMenuItem.Bounds = Rectangle.Round(val.GetBounds());
					}
					else
					{
						baseItem.RecalcSize();
						Rectangle rectangle5 = rectangle;
						rectangle5.Width -= baseItem.WidthInternal;
						rectangle5.Height -= baseItem.HeightInternal;
						if (rectangle5.Width < rectangle3.Width)
						{
							rectangle5.Inflate((rectangle3.Width - rectangle5.Width) / 2, 0);
						}
						if (rectangle5.Height < rectangle3.Height)
						{
							rectangle5.Inflate(0, (rectangle3.Height - rectangle5.Height) / 2);
						}
						Point point = new Point(rectangle5.X + (int)((double)(rectangle5.Width / 2) * Math.Cos((double)(num8 + num7 / 2) * (Math.PI / 180.0))), rectangle5.Y + (int)((double)(rectangle5.Height / 2) * Math.Sin((double)(num8 + num7 / 2) * (Math.PI / 180.0))));
						point.Offset(rectangle5.Width / 2, rectangle5.Height / 2);
						baseItem.TopInternal = point.Y;
						baseItem.LeftInternal = point.X;
					}
					if (baseItem != null && ᨄ(baseItem))
					{
						Rectangle rectangle6 = rectangle;
						rectangle6.Inflate(num3, num3);
						GraphicsPath val2 = new GraphicsPath();
						int num9 = num8 + 1;
						int num10 = num7 - 1;
						val2.AddArc(rectangle6, (float)num9, (float)num10);
						val2.AddArc(rectangle2, (float)(num9 + num10), (float)(-num10));
						val2.CloseAllFigures();
						Rectangle rectangle7 = rectangle2;
						rectangle7.Inflate(-4, -4);
						Point point2 = new Point(rectangle7.X + (int)((double)(rectangle7.Width / 2) * Math.Cos((double)(num8 + num7 / 2) * (Math.PI / 180.0))), rectangle7.Y + (int)((double)(rectangle7.Height / 2) * Math.Sin((double)(num8 + num7 / 2) * (Math.PI / 180.0))));
						point2.Offset(rectangle7.Width / 2, rectangle7.Height / 2);
						GraphicsPath val3 = ឱ.ឲ(new Point(-6, -6), 12, _17B6.ሪ);
						Matrix val4 = new Matrix();
						if (num8 + num7 / 2 != 0)
						{
							val4.Rotate((float)(num8 + num7 / 2));
							val4.Translate((float)point2.X, (float)point2.Y, (MatrixOrder)1);
						}
						else
						{
							val4.Translate((float)point2.X, (float)point2.Y);
						}
						val3.Transform(val4);
						__19E7[i] = new ᨓ(val2, val3, num8, num7);
					}
					num8 += num7;
					baseItem.Displayed = true;
				}
			}
		}
		finally
		{
			__19E8 = false;
		}
		base.RecalcSize();
	}

	private SubItemsCollection ᨂ()
	{
		SubItemsCollection subItems = SubItems;
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null)
		{
			subItems = baseItem.SubItems;
		}
		return subItems;
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		p.ContextData = _ቿ;
		p.ContextData2 = __19FF;
		if (_089F.फ())
		{
			StringFormat val = new StringFormat(StringFormat.GenericDefault);
			val.Alignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags & ~(val.FormatFlags & 0x1000));
			p.Graphics.DrawString("Trial period is over.", p.Font, SystemBrushes.Highlight, (RectangleF)Bounds, val);
			val.Dispose();
			return;
		}
		if (__19FF == eRadialMenuType.Circular)
		{
			ᨃ(p);
			return;
		}
		Graphics graphics = p.Graphics;
		if (__19EA < 100)
		{
			int num = Math.Max(5, __19EA);
			graphics.ScaleTransform((float)num / 100f, (float)num / 100f, (MatrixOrder)1);
			graphics.TranslateTransform((float)(WidthInternal / 2) * (float)(100 - num) / 100f, (float)(HeightInternal / 2) * (float)(100 - num) / 100f, (MatrixOrder)1);
		}
		bool flag = ExpandedItem() != null;
		RadialMenuColorTable radialMenuColorTable = ጧ();
		RadialMenuColorTable ቿ = _ቿ;
		Color color = ColorScheme.GetColor(2840474);
		Color color2 = Color.FromArgb(128, color);
		Color color3 = Color.White;
		Color color4 = Color.FromArgb(200, color);
		Color color5 = Color.White;
		if (!ቿ.RadialMenuBorder.IsEmpty)
		{
			color = ቿ.RadialMenuBorder;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuBorder.IsEmpty)
		{
			color = radialMenuColorTable.RadialMenuBorder;
		}
		if (!ቿ.RadialMenuBackground.IsEmpty)
		{
			color5 = ቿ.RadialMenuBackground;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuBackground.IsEmpty)
		{
			color5 = radialMenuColorTable.RadialMenuBackground;
		}
		if (!ቿ.RadialMenuInactiveBorder.IsEmpty)
		{
			color2 = ቿ.RadialMenuInactiveBorder;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuInactiveBorder.IsEmpty)
		{
			color2 = radialMenuColorTable.RadialMenuInactiveBorder;
		}
		if (!ቿ.RadialMenuExpandForeground.IsEmpty)
		{
			color3 = ቿ.RadialMenuExpandForeground;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuExpandForeground.IsEmpty)
		{
			color3 = radialMenuColorTable.RadialMenuExpandForeground;
		}
		if (!ቿ.RadialMenuMouseOverBorder.IsEmpty)
		{
			color4 = ቿ.RadialMenuMouseOverBorder;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuMouseOverBorder.IsEmpty)
		{
			color4 = radialMenuColorTable.RadialMenuMouseOverBorder;
		}
		Brush val2 = (Brush)new SolidBrush(color3);
		Brush val3 = (Brush)new SolidBrush(color4);
		Brush val4 = (Brush)new SolidBrush(color);
		Rectangle bounds = Bounds;
		bounds.Width--;
		bounds.Height--;
		GraphicsPath val5 = new GraphicsPath();
		try
		{
			Rectangle rectangle = bounds;
			val5.AddEllipse(rectangle);
			Rectangle rectangle2 = rectangle;
			int num2 = _19DF;
			int num3 = _19E0;
			rectangle2.Inflate(-(num2 - num3) / 2, -(num2 - num3) / 2);
			__19E9 = rectangle2;
			int num4 = Dpi.Width(__19F4);
			rectangle.Inflate(-num4, -num4);
			GraphicsPath val6 = new GraphicsPath();
			try
			{
				if (!flag && Parent == null && string.IsNullOrEmpty(__0EA6) && _ነ == null)
				{
					val6.AddEllipse(rectangle2);
					graphics.SetClip(val6, (CombineMode)4);
				}
				SolidBrush val7 = new SolidBrush(color5);
				try
				{
					p.Graphics.FillPath((Brush)(object)val7, val5);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				if (!flag)
				{
					graphics.ResetClip();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			int num5 = Dpi.Width(__19FA);
			rectangle.Inflate(-num5, -num5);
			val5.AddEllipse(rectangle);
			Brush val8 = (Brush)new SolidBrush(color2);
			try
			{
				graphics.FillPath(val8, val5);
				Pen val9 = new Pen(color, 2f);
				try
				{
					graphics.DrawEllipse(val9, rectangle2);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			if (flag)
			{
				Font font = Symbols.GetFont(__19FB, __19FD);
				Size size = TextDrawing.MeasureString(graphics, __19FC, font);
				int num6 = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
				size.Height -= num6;
				TextDrawing.DrawStringLegacy(graphics, __19FC, font, color, new Rectangle(rectangle2.X + (rectangle2.Width - size.Width) / 2, rectangle2.Y + (rectangle2.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
			}
			else if (!string.IsNullOrEmpty(__0EA6))
			{
				Font font2 = Symbols.GetFont(_ຨ, _ວ);
				if (_ᨀ.IsEmpty)
				{
					_ᨀ = TextDrawing.MeasureString(graphics, __0EA6, font2);
					int num7 = (int)Math.Ceiling((float)font2.FontFamily.GetCellDescent(font2.Style) * font2.Size / (float)font2.FontFamily.GetEmHeight(font2.Style));
					_ᨀ.Height -= num7;
				}
				TextDrawing.DrawStringLegacy(graphics, __0EA6, font2, color, new Rectangle(rectangle2.X + (rectangle2.Width - _ᨀ.Width) / 2 + 1, rectangle2.Y + (rectangle2.Height - _ᨀ.Height) / 2 + 1, 0, 0), eTextFormat.Default);
			}
			else if (_ነ != null)
			{
				graphics.DrawImage(_ነ, rectangle2.X + (rectangle2.Width - _ነ.Width) / 2, rectangle2.Y + (rectangle2.Height - _ነ.Height) / 2);
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		if (__19EA < 100)
		{
			graphics.ResetTransform();
			graphics.RotateTransform(-16f * (float)(100 - __19EA) / 100f);
			graphics.ScaleTransform((float)Math.Max(1, __19EA) / 100f, (float)Math.Max(1, __19EA) / 100f, (MatrixOrder)1);
			graphics.TranslateTransform((float)(WidthInternal / 2) * (float)(100 - __19EA) / 100f, (float)(HeightInternal / 2) * (float)(100 - __19EA) / 100f, (MatrixOrder)1);
		}
		else if (__19EB < 100)
		{
			int num8 = __19EB;
			graphics.ScaleTransform((float)num8 / 100f, (float)num8 / 100f, (MatrixOrder)1);
			graphics.TranslateTransform((float)(WidthInternal / 2) * (float)(100 - num8) / 100f, (float)(HeightInternal / 2) * (float)(100 - num8) / 100f, (MatrixOrder)1);
		}
		SubItemsCollection subItemsCollection = ᨂ();
		for (int i = 0; i < subItemsCollection.Count; i++)
		{
			BaseItem baseItem = subItemsCollection[i];
			if (!baseItem.Visible || !baseItem.Displayed)
			{
				continue;
			}
			if (p.ClipRectangle.IsEmpty || p.ClipRectangle.IntersectsWith(baseItem.DisplayRectangle))
			{
				Region clip = graphics.Clip;
				graphics.SetClip(baseItem.DisplayRectangle, (CombineMode)1);
				if (!graphics.IsClipEmpty)
				{
					baseItem.Paint(p);
				}
				graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
			if (__19E7 != null && __19E7[i] != null)
			{
				if (__19F9 == i)
				{
					graphics.FillPath(val3, __19E7[i].ᙳ);
					graphics.FillPath(val2, __19E7[i].ᨔ);
				}
				else
				{
					graphics.FillPath(val4, __19E7[i].ᙳ);
					graphics.FillPath(val2, __19E7[i].ᨔ);
				}
			}
		}
		val2.Dispose();
		val2 = null;
		val3.Dispose();
		val3 = null;
		val4.Dispose();
		val4 = null;
		graphics.ResetTransform();
	}

	private void ᨃ(ItemPaintArgs _065C)
	{
		Graphics graphics = _065C.Graphics;
		Rectangle bounds = Bounds;
		bounds.Width--;
		bounds.Height--;
		if (__19EA < 100)
		{
			graphics.ResetTransform();
			graphics.RotateTransform(-16f * (float)(100 - __19EA) / 100f);
			graphics.ScaleTransform((float)Math.Max(1, __19EA) / 100f, (float)Math.Max(1, __19EA) / 100f, (MatrixOrder)1);
			graphics.TranslateTransform((float)(WidthInternal / 2) * (float)(100 - __19EA) / 100f, (float)(HeightInternal / 2) * (float)(100 - __19EA) / 100f, (MatrixOrder)1);
		}
		else if (__19EB < 100)
		{
			int num = __19EB;
			graphics.ScaleTransform((float)num / 100f, (float)num / 100f, (MatrixOrder)1);
			graphics.TranslateTransform((float)(WidthInternal / 2) * (float)(100 - num) / 100f, (float)(HeightInternal / 2) * (float)(100 - num) / 100f, (MatrixOrder)1);
		}
		SubItemsCollection subItemsCollection = ᨂ();
		for (int i = 0; i < subItemsCollection.Count; i++)
		{
			BaseItem baseItem = subItemsCollection[i];
			if (baseItem.Visible && baseItem.Displayed && (_065C.ClipRectangle.IsEmpty || _065C.ClipRectangle.IntersectsWith(baseItem.DisplayRectangle)))
			{
				Region clip = graphics.Clip;
				graphics.SetClip(baseItem.DisplayRectangle, (CombineMode)1);
				if (!graphics.IsClipEmpty)
				{
					baseItem.Paint(_065C);
				}
				graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
		graphics.ResetTransform();
	}

	private bool ᨄ(BaseItem _06EC)
	{
		if (!_06EC.ShowSubItems)
		{
			return false;
		}
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			if (subItem.Visible)
			{
				return true;
			}
		}
		return false;
	}

	private int ᨅ(SubItemsCollection ᨆ)
	{
		int num = 0;
		foreach (BaseItem item in ᨆ)
		{
			if (item.Visible)
			{
				num++;
			}
		}
		return num;
	}

	private void ᨇ()
	{
		if (__19E7 != null)
		{
			for (int i = 0; i < __19E7.Length; i++)
			{
				if (__19E7[i] != null)
				{
					__19E7[i]._1A17();
				}
			}
		}
		__19E7 = null;
	}

	protected internal override void OnExpandChange()
	{
		if (Expanded)
		{
			ᨉ();
		}
		else
		{
			if (base.HotSubItem != null)
			{
				base.HotSubItem.InternalMouseLeave();
			}
			ᆤ();
			ᨈ(this);
		}
		base.OnExpandChange();
	}

	private void ᨈ(BaseItem _06EC)
	{
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			subItem.Expanded = false;
			ᨈ(subItem);
		}
	}

	protected virtual void OnOpenStateChanged(int oldValue, int newValue)
	{
		Refresh();
		switch (newValue)
		{
		case 0:
		{
			RadialMenuPopup radialMenuPopup = __19ED;
			__19ED = null;
			if (radialMenuPopup != null)
			{
				((Control)radialMenuPopup).Hide();
				((Component)(object)radialMenuPopup).Dispose();
			}
			OnClosed(EventArgs.Empty);
			break;
		}
		case 100:
			OnOpened(EventArgs.Empty);
			break;
		}
	}

	internal void ᨉ()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		MethodInvoker val = null;
		if (__19EE)
		{
			return;
		}
		if (__19EA > 0 && __19F0 < 2)
		{
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					ᨉ();
				};
			}
			BarUtilities.InvokeDelayed(val, 300);
			return;
		}
		try
		{
			__19F0 = 0;
			__19EE = true;
			Displayed = true;
			ᨊ();
			if (Parent != null)
			{
				IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
				if (iOwnerMenuSupport != null)
				{
					if (__19EF == null)
					{
						__19EF = new _2462(this);
					}
					__19EF.Expanded = true;
					iOwnerMenuSupport.RegisterPopup(__19EF);
				}
			}
			RadialMenuPopup radialMenuPopup = __19ED;
			if (radialMenuPopup == null)
			{
				radialMenuPopup = new RadialMenuPopup();
				radialMenuPopup.DisplayItem = this;
				if (_ݐ != null)
				{
					((Control)radialMenuPopup).Font = _ݐ;
				}
				else if (__19F7 != null)
				{
					((Control)radialMenuPopup).Font = ((Control)__19F7).Font;
				}
				((Control)radialMenuPopup).CreateControl();
			}
			if (__19F6.IsEmpty)
			{
				int num = _19DF;
				if (__19F7 != null)
				{
					Point location = ((Control)__19F7).PointToScreen(new Point(((Control)__19F7).Width / 2, ((Control)__19F7).Height / 2));
					location.X -= num / 2;
					location.Y -= num / 2;
					((Control)radialMenuPopup).Location = location;
				}
				else
				{
					Point mousePosition = Control.MousePosition;
					mousePosition.X -= num / 2;
					mousePosition.Y -= num / 2;
					((Control)radialMenuPopup).Location = mousePosition;
				}
			}
			else
			{
				((Control)radialMenuPopup).Location = __19F6;
			}
			((Control)radialMenuPopup).Visible = true;
			((Control)radialMenuPopup).Refresh();
			__19ED = radialMenuPopup;
			_1A6A obj = new _1A6A(new ခ(this, "OpenState", 0, 100), AnimationEasing.EaseOutCubic, 300);
			obj.AutoDispose = true;
			__19EC = obj;
			obj.Start();
		}
		finally
		{
			__19EE = false;
		}
	}

	private void ᨊ()
	{
		if (__19EC == null)
		{
			return;
		}
		DateTime now = DateTime.Now;
		while (!__19EC.IsDisposed)
		{
			Application.DoEvents();
			if (DateTime.Now.Subtract(now).TotalMilliseconds > 1000.0)
			{
				ᨋ();
				break;
			}
		}
		__19EC = null;
	}

	private void ᨋ()
	{
		_1A18 obj = __19EC;
		__19EC = null;
		if (obj != null)
		{
			obj.Stop();
			obj.Dispose();
		}
	}

	protected override bool ShouldCollapseParentItem()
	{
		if (base.HotSubItem != null)
		{
			return base.HotSubItem.AutoCollapseOnClick;
		}
		return base.ShouldCollapseParentItem();
	}

	internal void ᆤ()
	{
		ᨊ();
		_1A6A obj = new _1A6A(new ခ(this, "OpenState", 100, 0), AnimationEasing.EaseOutCubic, 300);
		obj.AutoDispose = true;
		obj.Start();
		_2462 _2462 = __19EF;
		if (_2462 != null)
		{
			IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
			if (iOwnerMenuSupport != null)
			{
				iOwnerMenuSupport.UnregisterPopup(_2462);
				_2462.Expanded = false;
			}
		}
	}

	protected internal IOwnerMenuSupport GetIOwnerMenuSupport()
	{
		return GetOwner() as IOwnerMenuSupport;
	}

	protected virtual void OnMaxItemRadialAngleChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	protected virtual void OnDiameterChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	protected virtual void OnCenterButtonDiameterChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnSubMenuEdgeWidthChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	protected virtual void OnSubMenuEdgeItemSpacingChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		SubItemsCollection subItemsCollection = ᨂ();
		foreach (BaseItem item in subItemsCollection)
		{
			if (!item.Visible || !item.Displayed)
			{
				continue;
			}
			if (item is RadialMenuItem radialMenuItem)
			{
				if (radialMenuItem.DisplayPath.IsVisible(x, y))
				{
					return radialMenuItem;
				}
			}
			else if ((item.Visible || base.IsOnCustomizeMenu) && item.Displayed && item.DisplayRectangle.Contains(x, y) && (_0ABB.IsEmpty || _0ABB.Contains(x, y)))
			{
				return item;
			}
		}
		return null;
	}

	protected override void Invalidate(Control containerControl)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		if (DesignMode)
		{
			if (containerControl.InvokeRequired)
			{
				Control obj = containerControl;
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						containerControl.Invalidate();
					};
				}
				obj.BeginInvoke((Delegate)(object)val);
			}
			else
			{
				containerControl.Invalidate();
			}
		}
		else if (containerControl.InvokeRequired)
		{
			Control obj2 = containerControl;
			if (val2 == null)
			{
				val2 = (MethodInvoker)delegate
				{
					containerControl.Invalidate(m_Rect, true);
				};
			}
			obj2.BeginInvoke((Delegate)(object)val2);
		}
		else
		{
			containerControl.Invalidate(m_Rect, true);
		}
	}

	protected virtual void OnMenuLocationChanged(Point oldValue, Point newValue)
	{
		if (__19ED != null)
		{
			((Control)__19ED).Location = newValue;
		}
	}

	protected virtual void OnRadialMenuChanged(RadialMenu oldValue, RadialMenu newValue)
	{
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		ᨌ(objArg);
	}

	private void ᨌ(MouseEventArgs ᨍ)
	{
		if (base.HotSubItem == null && __19E7 != null)
		{
			bool flag = false;
			for (int i = 0; i < __19E7.Length; i++)
			{
				ᨓ ᨓ = __19E7[i];
				if (ᨓ != null && ᨓ.ᙳ.IsVisible(ᨍ.X, ᨍ.Y))
				{
					_19E2 = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_19E2 = -1;
			}
		}
		else
		{
			_19E2 = -1;
		}
	}

	public override void InternalMouseLeave()
	{
		_19E2 = -1;
		base.InternalMouseLeave();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		ᨌ(objArg);
		int num = _19E2;
		if (num >= 0)
		{
			SubItemsCollection subItemsCollection = ᨂ();
			if (num < subItemsCollection.Count)
			{
				BaseItem baseItem = subItemsCollection[num];
				if (baseItem.ShowSubItems && baseItem.VisibleSubItems > 0)
				{
					baseItem.Expanded = true;
					return;
				}
			}
		}
		else if (ExpandedItem() == null && __19E9.Contains(objArg.Location))
		{
			Expanded = false;
		}
		base.InternalMouseDown(objArg);
	}

	private void ᨎ()
	{
		_2488 _2488 = new _2488();
		_2488.AutoDispose = true;
		_1A6A obj = new _1A6A(new ခ(this, "InnerScale", 100, 85), AnimationEasing.EaseInCubic, 250);
		obj.Animations.Add(new ခ(this, "SubMenuEdgeWidthAnimated", 0, 12));
		obj.AutoDispose = true;
		_2488.Animations.Add(obj);
		obj = new _1A6A(new ခ(this, "InnerScale", 85, 100), AnimationEasing.EaseOutCubic, 250);
		obj.Animations.Add(new ခ(this, "SubMenuEdgeWidthAnimated", 12, 0));
		obj.AutoDispose = true;
		_2488.Animations.Add(obj);
		__19F8 = _2488;
		_2488.Run();
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		NeedRecalcSize = true;
		if (Expanded)
		{
			if (__19F8 == null || __19F8.IsDisposed)
			{
				ᨊ();
				ᨎ();
			}
			else
			{
				Invalidate();
			}
		}
		_19E2 = -1;
		base.OnSubItemExpandChange(item);
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		object containerControl = ContainerControl;
		ᨏ(this, containerControl);
	}

	private void ᨏ(BaseItem _06EC, object ᨐ)
	{
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			subItem.ContainerControl = ᨐ;
			ᨏ(subItem, ᨐ);
		}
	}

	protected internal override BaseItem ExpandedItem()
	{
		return ᨑ(this);
	}

	private BaseItem ᨑ(BaseItem _06EC)
	{
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			if (subItem.Expanded)
			{
				BaseItem baseItem2 = ᨑ(subItem);
				if (baseItem2 != null)
				{
					return baseItem2;
				}
				return subItem;
			}
		}
		return null;
	}

	protected virtual void OnBackSymbolSizeChanged(int oldValue, int newValue)
	{
		Invalidate();
	}

	protected virtual void OnBackSymbolChanged(string oldValue, string newValue)
	{
		Invalidate();
	}

	protected virtual void OnBackButtonSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnMaxItemPieAngleChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	internal static RadialMenuColorTable ጧ()
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.RadialMenu;
		}
		return null;
	}

	protected virtual void OnMenuTypeChanged(eRadialMenuType oldValue, eRadialMenuType newValue)
	{
		Refresh();
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		_ᨀ = Size.Empty;
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		_ᨀ = Size.Empty;
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnFontChanged(Font oldValue, Font newValue)
	{
		if (__19ED != null && Expanded)
		{
			((Control)__19ED).Invalidate();
		}
	}

	[CompilerGenerated]
	private void ᨒ()
	{
		ᨉ();
	}
}
