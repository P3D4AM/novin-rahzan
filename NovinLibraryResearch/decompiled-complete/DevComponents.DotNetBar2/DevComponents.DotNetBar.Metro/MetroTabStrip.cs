using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Metro;

[ToolboxItem(false)]
[ComVisible(false)]
public class MetroTabStrip : ItemControl
{
	private ᐬ _ᐚ;

	private int _ற;

	private bool _ᐛ;

	private Rectangle _ᐜ = Rectangle.Empty;

	private Rectangle _ᐝ = Rectangle.Empty;

	private Rectangle _ᐞ = Rectangle.Empty;

	private Font _ᐟ;

	private bool _ᐠ = true;

	private ElementStyle _ᐡ = new ElementStyle();

	private bool _ᐢ = true;

	private string _ᅶ = "";

	private MarkupLinkClickEventHandler _0D83;

	private _0CB4 _ᐣ;

	internal Rectangle අ = Rectangle.Empty;

	private bool _ᐤ;

	private IKeyTipsControl _ᐥ;

	private bool _ᐦ = true;

	private bool _ᐧ;

	private Point _ᐨ = Point.Empty;

	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	[Category("Appearance")]
	[Browsable(true)]
	public string TitleText
	{
		get
		{
			return _ᅶ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ᅶ = value;
			_ᐣ = null;
			if (ሒ.መ(ref _ᅶ))
			{
				_ᐣ = ሒ.ሕ(_ᅶ);
				if (_ᐣ != null)
				{
					_ᐣ.ವ += ඌ;
				}
				අ = Rectangle.Empty;
				((Control)this).Invalidate();
			}
		}
	}

	internal _0CB4 _0D62 => _ᐣ;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return _ᐢ;
		}
		set
		{
			_ᐢ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	[Category("Customization")]
	[Browsable(true)]
	public bool CanCustomize
	{
		get
		{
			return _ᐠ;
		}
		set
		{
			_ᐠ = value;
		}
	}

	[Description("Indicates explicit height of the caption provided by control")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(0)]
	public int CaptionHeight
	{
		get
		{
			return _ற;
		}
		set
		{
			_ற = value;
			_ᐚ.NeedRecalcSize = true;
		}
	}

	internal bool _0D63
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal Rectangle _0D65
	{
		get
		{
			return _ᐜ;
		}
		set
		{
			_ᐜ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Rectangle QuickToolbarBounds
	{
		get
		{
			return _ᐝ;
		}
		set
		{
			_ᐝ = value;
		}
	}

	internal Rectangle _൦
	{
		get
		{
			return _ᐞ;
		}
		set
		{
			_ᐞ = value;
		}
	}

	internal SystemCaptionItem _൧ => _ᐚ._൧;

	[DefaultValue(false)]
	[Description("Indicates whether custom caption line provided by the control is visible.")]
	[Browsable(true)]
	[Category("Appearance")]
	public bool CaptionVisible
	{
		get
		{
			return _ᐛ;
		}
		set
		{
			_ᐛ = value;
			ඍ();
		}
	}

	[DefaultValue(null)]
	[Browsable(true)]
	[Description("")]
	[Category("Appearance")]
	public Font CaptionFont
	{
		get
		{
			return _ᐟ;
		}
		set
		{
			_ᐟ = value;
			_ᐚ.NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	internal eDotNetBarStyle _൨ => _ᐚ.EffectiveStyle;

	[Description("Specifies the visual style of the control.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(eDotNetBarStyle.Metro)]
	[Browsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return _ᐚ.Style;
		}
		set
		{
			base.ColorScheme.ߕ = value;
			_ᐚ.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _ᐚ.ᐭ.SubItems;

	[Browsable(false)]
	public MetroTabItem SelectedTab
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is MetroTabItem && ((MetroTabItem)item).Checked)
				{
					return (MetroTabItem)item;
				}
			}
			return null;
		}
	}

	internal bool _൩
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return (int)val.WindowState == 2;
			}
			return false;
		}
	}

	internal override bool ষ => false;

	[Browsable(false)]
	public bool IsInKeyTipsMode => _ᐤ | ShowKeyTips;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Form.Icon is shown in top-left corner.")]
	public bool ShowIcon
	{
		get
		{
			return _ᐦ;
		}
		set
		{
			if (_ᐦ != value)
			{
				_ᐦ = value;
				_ᐚ.NeedRecalcSize = true;
				RecalcLayout();
			}
		}
	}

	internal bool _൮ => _ᐧ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => _ᐚ.ᐮ.SubItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public GenericItemContainer CaptionContainerItem => _ᐚ.ᐮ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SimpleItemContainer StripContainerItem => _ᐚ.ᐭ;

	internal ᐬ ᐙ => _ᐚ;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public MetroTabStrip()
	{
		((Control)this).SetStyle((ControlStyles)4096, true);
		_ᐚ = new ᐬ(this);
		_ᐚ.GlobalItem = false;
		_ᐚ.ContainerControl = this;
		_ᐚ.Displayed = true;
		_ᐚ.SetOwner(this);
		_ᐚ.Style = eDotNetBarStyle.Metro;
		SetBaseItemContainer(_ᐚ);
		base.ColorScheme.ߕ = eDotNetBarStyle.Office2003;
		((Control)this).AutoSize = true;
		_ᐚ._൧.Click += ණ;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		Style = StyleManager.GetEffectiveStyle();
	}

	private void ඌ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj && _0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	private void ඍ()
	{
		_ᐚ.NeedRecalcSize = true;
		RecalcLayout();
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (ষ && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - BarFunctions.CaptionButtonSize.Width * 3, 0, BarFunctions.CaptionButtonSize.Width * 3, BarFunctions.CaptionButtonSize.Height + 6);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (_ᐜ.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (CaptionVisible && !_൩ && new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (BarFunctions.IsWindows7 && _൩ && _0D65.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}

	protected override ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return base.GetColorScheme();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		base.PaintControlBackground(pa);
		_ᐝ = Rectangle.Empty;
		_ᐜ = Rectangle.Empty;
		_ᐞ = Rectangle.Empty;
		MetroRender.Paint(this, pa);
		if (_089F.फ())
		{
			pa.Graphics.Clear(Color.White);
			TextDrawing.DrawString(pa.Graphics, "Your DotNetBar trial has expired :-(", ((Control)this).Font, Color.FromArgb(128, Color.Black), ((Control)this).ClientRectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return _ᐡ;
	}

	internal void ඕ()
	{
		_ᐡ = MetroRender.GetColorTable().MetroTab.TabStrip.BackgroundStyle;
	}

	internal ElementStyle ඖ()
	{
		return GetBackgroundStyle();
	}

	internal Rectangle _0D97()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (_ᐛ)
		{
			itemContainerRectangle.Y += ක();
		}
		return itemContainerRectangle;
	}

	internal Rectangle _0D98()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ষ)
		{
			itemContainerRectangle.Y += 3;
		}
		return new Rectangle(itemContainerRectangle.X, itemContainerRectangle.Y, itemContainerRectangle.Width, ක());
	}

	public int GetCaptionHeight()
	{
		if (_ற == 0)
		{
			if (StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
			{
				return Dpi.Height32;
			}
			return Dpi.Height25;
		}
		return Dpi.Height(_ற);
	}

	internal int ක()
	{
		return GetCaptionHeight();
	}

	internal int ඛ()
	{
		return GetCaptionHeight();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_ᐚ.NeedRecalcSize)
		{
			RecalcSize();
		}
		ඕ();
		base.OnPaint(e);
	}

	protected override void RecalcSize()
	{
		_ᐜ = Rectangle.Empty;
		_ᐞ = Rectangle.Empty;
		_ᐝ = Rectangle.Empty;
		ඕ();
		base.RecalcSize();
	}

	public override int GetAutoSizeHeight()
	{
		return base.GetAutoSizeHeight();
	}

	protected override bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Location = ((Control)this).PointToScreen(displayRectangle.Location);
		MetroShell metroShell = ((Control)this).Parent as MetroShell;
		if (((Control)this).Parent is MetroShell)
		{
			displayRectangle = ((Control)this).Parent.DisplayRectangle;
			displayRectangle.Location = ((Control)this).Parent.PointToScreen(displayRectangle.Location);
			if (metroShell.SelectedTab != null && metroShell.SelectedTab.Panel != null && ((IKeyTipsControl)metroShell.SelectedTab.Panel).ShowKeyTips)
			{
				return false;
			}
		}
		if (metroShell != null && !metroShell.MouseWheelTabScrollEnabled)
		{
			return false;
		}
		Point mousePosition = Control.MousePosition;
		bool flag = true;
		Form val = ((Control)this).FindForm();
		if (val != null && !BarFunctions.IsFormActive(val))
		{
			flag = false;
		}
		if (flag && displayRectangle.Contains(mousePosition) && !ShowKeyTips && Items.Count > 0)
		{
			IntPtr intPtr = _089F.WindowFromPoint(new _089F.POINT(mousePosition));
			Control val2 = Control.FromChildHandle(intPtr);
			if (val2 == null)
			{
				val2 = Control.FromHandle(intPtr);
			}
			if (val2 is MetroTabStrip || val2 is MetroShell || val2 is MetroTabPanel)
			{
				MetroTabItem selectedTab = SelectedTab;
				int num = 0;
				int num2 = Items.Count - 1;
				int num3 = 1;
				if ((_੦.ToInt64() & 0x80000000u) > 0)
				{
					num3 = -1;
					num2 = 0;
				}
				if (selectedTab != null)
				{
					num = Items.IndexOf(selectedTab) + num3;
					if (num3 < 0 && num < 0)
					{
						return false;
					}
					if (num == Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = Items.Count - 1;
					}
				}
				int num4 = num - num3;
				do
				{
					num4 += num3;
					if (num4 < 0 || num4 > Items.Count - 1)
					{
						break;
					}
					if (Items[num4] is MetroTabItem && Items[num4].Visible)
					{
						((MetroTabItem)Items[num4]).Checked = true;
						return true;
					}
				}
				while (num4 != num2);
			}
		}
		return false;
	}

	public override ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, arrayList);
		MetroShell metroShell = ᐩ();
		if (metroShell != null && metroShell.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByName(metroShell.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType);
		MetroShell metroShell = ᐩ();
		if (metroShell != null && metroShell.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(metroShell.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		MetroShell metroShell = ᐩ();
		if (metroShell != null && metroShell.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(metroShell.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public override BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		MetroShell metroShell = ᐩ();
		if (metroShell != null && metroShell.GlobalContextMenuBar != null)
		{
			return BarFunctions.GetSubItemByName(metroShell.GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	internal void ඟ(RibbonBar ච)
	{
		if (!_ᐤ)
		{
			_ᐤ = true;
		}
		if (_ᐥ != ((Control)ච).Parent && ((Control)ච).Parent is RibbonPanel)
		{
			if (_ᐥ != null)
			{
				_ᐥ.ShowKeyTips = false;
			}
			_ᐥ = ((Control)ච).Parent as IKeyTipsControl;
			if (_ᐥ != null)
			{
				_ᐥ.ShowKeyTips = true;
			}
		}
	}

	protected override void OnShowKeyTipsChanged()
	{
		ᐩ();
		base.OnShowKeyTipsChanged();
		if (ShowKeyTips && !_ᐤ)
		{
			ඥ();
		}
	}

	internal void ඡ(SuperTabControl ජ)
	{
		if (_ᐥ == ජ || _ᐥ == ජ.TabStrip)
		{
			_ᐥ = null;
		}
	}

	protected override bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected I4, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		if (_ᐤ)
		{
			if (base.HasRegisteredPopups)
			{
				base._੪(_0A65, _੦, _੧);
				if (!base.HasRegisteredPopups)
				{
					ඦ();
				}
			}
			Keys val = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
			if ((int)val == 0)
			{
				int num = _0EFE._0F11(_੦);
				val = (Keys)num;
			}
			if ((int)val == 27)
			{
				bool flag = false;
				if (!ShowKeyTips && _ᐥ != null && _ᐥ is RibbonPanel)
				{
					flag = true;
				}
				ඦ();
				if (flag)
				{
					ඥ();
				}
				return base._੪(_0A65, _੦, _੧);
			}
			char c = ඣ((int)val);
			if (c == '\0')
			{
				ඦ();
				return true;
			}
			if ((int)val == 32 || (int)val == 40 || (int)val == 39 || (int)val == 9)
			{
				ඦ();
				return false;
			}
			if (((Control)this).ProcessMnemonic(c))
			{
				return true;
			}
			return true;
		}
		return base._੪(_0A65, _੦, _੧);
	}

	private char ඣ(int _੬)
	{
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(_੬);
			Encoding.Default.GetDecoder().GetChars(array2, 0, 1, array, 0);
		}
		catch (Exception)
		{
			return '\0';
		}
		return array[0];
	}

	protected override bool ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		int num = _0EFE._0F11(_੦);
		if (num == 18 || num == 121)
		{
			bool flag = true;
			if (_ᐤ)
			{
				if (num == 18 || num == 121)
				{
					flag = false;
				}
				ඦ();
			}
			else
			{
				Form val = ((Control)this).FindForm();
				if (val == Form.ActiveForm)
				{
					flag = false;
					ඥ();
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return base.ੳ(_0A65, _੦, _੧);
	}

	private void ඥ()
	{
		if (_ᐢ)
		{
			_ᐤ = true;
			ShowKeyTips = true;
			SetupActiveWindowTimer();
		}
	}

	private void ඦ()
	{
		if (_ᐤ)
		{
			ReleaseActiveWindowTimer();
			if (!ShowKeyTips && _ᐥ != null)
			{
				_ᐥ.ShowKeyTips = false;
				_ᐥ = null;
			}
			ShowKeyTips = false;
		}
		_ᐤ = false;
	}

	public void ExitKeyTipsMode()
	{
		ඦ();
	}

	internal void ට(BaseItem _06EC)
	{
		if (ShowKeyTips || (_ᐥ != null && _ᐥ.ShowKeyTips))
		{
			ShowKeyTips = false;
			ඦ();
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ShowKeyTips || (_ᐥ != null && _ᐥ.ShowKeyTips))
		{
			ShowKeyTips = false;
			ඦ();
		}
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (_ᐥ != null)
		{
			if (_ᐥ.ProcessMnemonicEx(charCode))
			{
				ඦ();
				return true;
			}
			return false;
		}
		if (!_ᐤ && (Control.ModifierKeys & 0x40000) != 262144)
		{
			return false;
		}
		return base.ProcessMnemonic(charCode);
	}

	public override bool ProcessMnemonicEx(char charCode)
	{
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		BaseItem baseItem = null;
		if (_ᐛ)
		{
			baseItem = GetItemForMnemonic(_ᐚ.ᐮ, charCode, deepScan: false, stackKeys: true);
			if (baseItem != null && ProcessItemMnemonicKey(baseItem))
			{
				ඦ();
				return true;
			}
		}
		ᐩ();
		if (baseItem == null)
		{
			baseItem = GetItemForMnemonic(GetBaseItemContainer(), charCode, deepScan: true, stackKeys: true);
		}
		if (baseItem is MetroTabItem && baseItem.Visible)
		{
			if (!_ᐤ)
			{
				ඥ();
			}
			ShowKeyTips = false;
			if (baseItem != SelectedTab)
			{
				baseItem.RaiseClick();
			}
			if (SelectedTab != null && _ᐢ)
			{
				if (_ᐥ != null)
				{
					_ᐥ.ShowKeyTips = false;
				}
				_ᐥ = SelectedTab.Panel;
				if (_ᐥ != null)
				{
					_ᐥ.ShowKeyTips = true;
				}
			}
			return true;
		}
		if (baseItem != null && ProcessItemMnemonicKey(baseItem))
		{
			ShowKeyTips = false;
			if (baseItem is MetroAppButton)
			{
				MetroAppButton metroAppButton = (MetroAppButton)baseItem;
				if (metroAppButton.BackstageTab != null)
				{
					if (_ᐥ != null)
					{
						_ᐥ.ShowKeyTips = false;
					}
					_ᐥ = metroAppButton.BackstageTab.TabStrip;
					if (!_ᐥ.ShowKeyTips)
					{
						_ᐥ.ShowKeyTips = true;
					}
				}
			}
			return true;
		}
		if (_ᐤ && SelectedTab != null && SelectedTab.Panel != null)
		{
			Control panel = (Control)(object)SelectedTab.Panel;
			foreach (Control item in (ArrangedElementCollection)panel.Controls)
			{
				Control val = item;
				if (val is RibbonBar && val.Visible && Control.IsMnemonic(charCode, val.Text))
				{
					ShowKeyTips = false;
					((RibbonBar)(object)val).ShowKeyTips = true;
					if (_ᐥ != null)
					{
						_ᐥ.ShowKeyTips = false;
					}
					_ᐥ = (IKeyTipsControl)val;
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnActiveWindowChanged()
	{
		ඦ();
		base.OnActiveWindowChanged();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return ((ContainerControl)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return base.ProcessDialogKey(keyData);
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		ᐩ()?.ᐆ(item, ᐇ: false);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_ᐣ != null)
		{
			_ᐣ.ભ((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	private MetroShell ᐩ()
	{
		return ((Control)this).Parent as MetroShell;
	}

	internal void ᐪ(Point _065C)
	{
		Form val = ((Control)this).FindForm();
		if (val is MetroAppForm)
		{
			((MetroAppForm)(object)val).Ꮨ(_065C);
			return;
		}
		byte[] bytes = BitConverter.GetBytes(_065C.X);
		byte[] bytes2 = BitConverter.GetBytes(_065C.Y);
		byte[] value = new byte[4]
		{
			bytes[0],
			bytes[1],
			bytes2[0],
			bytes2[1]
		};
		int lParam = BitConverter.ToInt32(value, 0);
		((Control)this).Capture = false;
		_089F.SendMessage(((Control)val).Handle, 274, _089F.TrackPopupMenu(_089F.GetSystemMenu(((Control)val).Handle, bRevert: false), 256u, _065C.X, _065C.Y, 0, ((Control)val).Handle, IntPtr.Zero), lParam);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		_ᐧ = false;
		_ᐨ = e.Location;
		if (_ᐛ)
		{
			_ᐧ = HitTestCaption(new Point(e.X, e.Y));
			if ((int)e.Button == 2097152 && _ᐧ)
			{
				ᐪ(Control.MousePosition);
				return;
			}
			if (_ᐣ != null)
			{
				_ᐣ.ग़((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		if ((int)e.Button == 2097152 && QuickToolbarBounds.Contains(e.X, e.Y))
		{
			ᐩ()?.ᐄ(this, e.X, e.Y);
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ᐛ)
		{
			if (_ᐣ != null)
			{
				_ᐣ.ફ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		_ᐧ = false;
		base.OnMouseUp(e);
	}

	internal BaseItem ඩ()
	{
		if (!CaptionVisible)
		{
			return null;
		}
		BaseItem captionContainerItem = CaptionContainerItem;
		if (_൨ == eDotNetBarStyle.Office2010 && Items.Count > 0 && Items[0] is MetroAppButton)
		{
			return Items[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is MetroAppButton)
		{
			return captionContainerItem.SubItems[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ButtonItem && ((ButtonItem)captionContainerItem.SubItems[0]).HotTrackingStyle == eHotTrackingStyle.Image)
		{
			return captionContainerItem.SubItems[0];
		}
		return null;
	}

	public void StartFormMove()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 0)
		{
			if (ඩ() is PopupItem { Expanded: not false } popupItem)
			{
				popupItem.Expanded = false;
			}
			Point mousePosition = Control.MousePosition;
			byte[] bytes = BitConverter.GetBytes(mousePosition.X);
			byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
			byte[] value = new byte[4]
			{
				bytes[0],
				bytes[1],
				bytes2[0],
				bytes2[1]
			};
			int lParam = BitConverter.ToInt32(value, 0);
			((Control)this).Capture = false;
			_089F.SendMessage(((Control)val).Handle, 274, 61458, lParam);
			_ᐧ = false;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (_ᐛ)
		{
			if ((int)e.Button == 1048576 && ((_ᐧ && HitTestCaption(new Point(e.X, e.Y))) || HitTest(_ᐨ.X, _ᐨ.Y) == null))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && (int)val.WindowState == 0)
				{
					StartFormMove();
					return;
				}
			}
			if (_ᐣ != null)
			{
				_ᐣ.મ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		base.OnMouseMove(e);
	}

	protected override void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (_ᐛ)
		{
			MouseEventArgs ؿ = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
			ؿ = ඪ(ؿ);
			mousePos = new Point(ؿ.X, ؿ.Y);
		}
		base.InternalOnClick(mb, mousePos);
	}

	private MouseEventArgs ඪ(MouseEventArgs ؿ)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		if (ؿ.Y <= 6)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 2 && val is RibbonForm)
			{
				if (ؿ.X > 4)
				{
					ؿ = ((ؿ.X < ((Control)this).Width - 6) ? new MouseEventArgs(ؿ.Button, ؿ.Clicks, ؿ.X, CaptionContainerItem.TopInternal + 5, ؿ.Delta) : new MouseEventArgs(ؿ.Button, ؿ.Clicks, _൧.DisplayRectangle.Right - 4, _൧.DisplayRectangle.Top + 4, ؿ.Delta));
				}
				else
				{
					BaseItem baseItem = ඩ();
					if (baseItem != null)
					{
						ؿ = new MouseEventArgs(ؿ.Button, ؿ.Clicks, baseItem.LeftInternal + 1, baseItem.TopInternal + 1, ؿ.Delta);
					}
				}
			}
		}
		return ؿ;
	}

	protected override void OnClick(EventArgs e)
	{
		if (_ᐛ && _ᐣ != null)
		{
			_ᐣ.દ((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		if (_ᐛ)
		{
			Point p = ((Control)this).PointToClient(Control.MousePosition);
			if (HitTestCaption(p))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.MaximizeBox && ((int)val.FormBorderStyle == 4 || (int)val.FormBorderStyle == 6))
				{
					if ((int)val.WindowState == 0)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
					}
					else if ((int)val.WindowState == 2)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}

	protected bool HitTestCaption(Point p)
	{
		return _ᐜ.Contains(p);
	}

	private void ණ(object _٠, EventArgs ؿ)
	{
		SystemCaptionItem systemCaptionItem = _٠ as SystemCaptionItem;
		Form val = ((Control)this).FindForm();
		if (val != null && systemCaptionItem._1C7E == systemCaptionItem.ᲀ)
		{
			if (systemCaptionItem._1C7E == _1886._1C8C)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8F)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8D)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
			}
			else if (systemCaptionItem._1C7E == _1886.ᆤ)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8E)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61824, 0);
			}
		}
	}

	internal void ᐫ()
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		base.OnResize(e);
		if (!_ᐛ)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if ((int)val.WindowState == 2 || (int)val.WindowState == 1)
			{
				_ᐚ._൧.RestoreEnabled = true;
			}
			else
			{
				_ᐚ._൧.RestoreEnabled = false;
			}
		}
	}

	protected override void છ(Graphics _٥)
	{
		if (ShowKeyTips)
		{
			KeyTipsRendererEventArgs ؿ = new KeyTipsRendererEventArgs(_٥, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			જ(_ᐚ.ᐭ, renderer, ؿ);
			if (_ᐛ)
			{
				જ(_ᐚ.ᐮ, renderer, ؿ);
			}
		}
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Rectangle keyTipRectangle = base.GetKeyTipRectangle(g, item, font, keyTip);
		if (QuickToolbarItems.Contains(item))
		{
			keyTipRectangle.Y += 4;
		}
		return keyTipRectangle;
	}

	internal void ర(bool ఱ)
	{
		if (_ᐚ.ᐭ == null)
		{
			return;
		}
		bool flag = false;
		try
		{
			if (Items.Contains("dotnetbarsysiconitem"))
			{
				Items.Remove("dotnetbarsysiconitem");
				flag = true;
			}
			if (Items.Contains("dotnetbarsysmenuitem"))
			{
				Items.Remove("dotnetbarsysmenuitem");
				flag = true;
			}
			if (ఱ && flag)
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
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!ళ.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!ళ.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			if (!ళ.MaximizeBox)
			{
				mDISystemItem.RestoreEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += ఴ;
			Items.Add(mDISystemItem);
			if (ఱ)
			{
				RecalcLayout();
			}
		}
	}

	private void ఴ(object _٠, EventArgs ؿ)
	{
		MDISystemItem mDISystemItem = _٠ as MDISystemItem;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val = val.ActiveMdiChild;
		}
		if (val == null)
		{
			ర(ఱ: true);
		}
		else if (mDISystemItem._1C7E == _1886._1C8C)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
		}
		else if (mDISystemItem._1C7E == _1886._1C8D)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
		}
		else if (mDISystemItem._1C7E == _1886.ᆤ)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
		else if (mDISystemItem._1C7E == _1886.Ა)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61504, 0);
		}
	}

	internal void ඬ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseDown(ؿ);
	}

	internal void ත(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseMove(ؿ);
	}

	internal void ථ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	internal void ද(EventArgs ؿ)
	{
		((Control)this).OnClick(ؿ);
	}
}
