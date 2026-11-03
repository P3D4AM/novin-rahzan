using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(true)]
[DefaultEvent("SelectedItemChanged")]
[Designer("DevComponents.DotNetBar.Design.CrumbBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(CrumbBar), "CrumbBar.CrumbBar.ico")]
public class CrumbBar : ItemControl
{
	private CrumbBarItemsCollection _ᡨ;

	private _21E3 __21CE;

	private Office2007Renderer __21CF;

	private CrumBarSelectionEventHandler _21D0;

	private CrumBarSelectionEventHandler _20DD;

	private CrumbBarItem __21D1;

	private eCrumbBarStyle _ᎌ = eCrumbBarStyle.Vista;

	private Size __21D2 = Size.Empty;

	private string __21D3 = Path.PathSeparator.ToString();

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates currently selected item")]
	public CrumbBarItem SelectedItem
	{
		get
		{
			return __21D1;
		}
		set
		{
			SetSelectedItem(value, eEventSource.Code);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("DevComponents.DotNetBar.Design.CrumbBarItemCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public CrumbBarItemsCollection Items => _ᡨ;

	[Description("Indicates visual style of the control.")]
	[Category("Appearance")]
	[DefaultValue(eCrumbBarStyle.Vista)]
	public eCrumbBarStyle Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			if (value != _ᎌ)
			{
				eCrumbBarStyle ᎌ = _ᎌ;
				_ᎌ = value;
				OnStyleChanged(ᎌ, value);
			}
		}
	}

	[Browsable(false)]
	public Office2007ColorTable VistaColorTable => __21CF.ColorTable;

	protected override Size DefaultSize => new Size(200, 22);

	[Browsable(false)]
	[Localizable(true)]
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

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return base.AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				base.AutoSize = value;
				_21DE();
				_21DF();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DefaultValue("\\")]
	[Description("Indicates the delimiter string that the tree node path uses.")]
	[Browsable(false)]
	public string PathSeparator
	{
		get
		{
			return __21D3;
		}
		set
		{
			__21D3 = value;
		}
	}

	[Browsable(false)]
	public override eBarImageSize ImageSize
	{
		get
		{
			return base.ImageSize;
		}
		set
		{
			base.ImageSize = value;
		}
	}

	[Browsable(false)]
	public override ImageList ImagesLarge
	{
		get
		{
			return base.ImagesLarge;
		}
		set
		{
			base.ImagesLarge = value;
		}
	}

	[Browsable(false)]
	public override ImageList ImagesMedium
	{
		get
		{
			return base.ImagesMedium;
		}
		set
		{
			base.ImagesMedium = value;
		}
	}

	[Browsable(false)]
	public override Font KeyTipsFont
	{
		get
		{
			return base.KeyTipsFont;
		}
		set
		{
			base.KeyTipsFont = value;
		}
	}

	[Browsable(false)]
	public override bool ShowShortcutKeysInToolTips
	{
		get
		{
			return base.ShowShortcutKeysInToolTips;
		}
		set
		{
			base.ShowShortcutKeysInToolTips = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Description("Occurs before SelectedItem has changed and provides opportunity to cancel the change.")]
	public event CrumBarSelectionEventHandler SelectedItemChanging
	{
		add
		{
			CrumBarSelectionEventHandler crumBarSelectionEventHandler = _21D0;
			CrumBarSelectionEventHandler crumBarSelectionEventHandler2;
			do
			{
				crumBarSelectionEventHandler2 = crumBarSelectionEventHandler;
				CrumBarSelectionEventHandler value2 = (CrumBarSelectionEventHandler)Delegate.Combine(crumBarSelectionEventHandler2, value);
				crumBarSelectionEventHandler = Interlocked.CompareExchange(ref _21D0, value2, crumBarSelectionEventHandler2);
			}
			while ((object)crumBarSelectionEventHandler != crumBarSelectionEventHandler2);
		}
		remove
		{
			CrumBarSelectionEventHandler crumBarSelectionEventHandler = _21D0;
			CrumBarSelectionEventHandler crumBarSelectionEventHandler2;
			do
			{
				crumBarSelectionEventHandler2 = crumBarSelectionEventHandler;
				CrumBarSelectionEventHandler value2 = (CrumBarSelectionEventHandler)Delegate.Remove(crumBarSelectionEventHandler2, value);
				crumBarSelectionEventHandler = Interlocked.CompareExchange(ref _21D0, value2, crumBarSelectionEventHandler2);
			}
			while ((object)crumBarSelectionEventHandler != crumBarSelectionEventHandler2);
		}
	}

	public event CrumBarSelectionEventHandler SelectedItemChanged
	{
		add
		{
			CrumBarSelectionEventHandler crumBarSelectionEventHandler = _20DD;
			CrumBarSelectionEventHandler crumBarSelectionEventHandler2;
			do
			{
				crumBarSelectionEventHandler2 = crumBarSelectionEventHandler;
				CrumBarSelectionEventHandler value2 = (CrumBarSelectionEventHandler)Delegate.Combine(crumBarSelectionEventHandler2, value);
				crumBarSelectionEventHandler = Interlocked.CompareExchange(ref _20DD, value2, crumBarSelectionEventHandler2);
			}
			while ((object)crumBarSelectionEventHandler != crumBarSelectionEventHandler2);
		}
		remove
		{
			CrumBarSelectionEventHandler crumBarSelectionEventHandler = _20DD;
			CrumBarSelectionEventHandler crumBarSelectionEventHandler2;
			do
			{
				crumBarSelectionEventHandler2 = crumBarSelectionEventHandler;
				CrumBarSelectionEventHandler value2 = (CrumBarSelectionEventHandler)Delegate.Remove(crumBarSelectionEventHandler2, value);
				crumBarSelectionEventHandler = Interlocked.CompareExchange(ref _20DD, value2, crumBarSelectionEventHandler2);
			}
			while ((object)crumBarSelectionEventHandler != crumBarSelectionEventHandler2);
		}
	}

	public CrumbBar()
	{
		_ᡨ = new CrumbBarItemsCollection(this);
		__21CE = new _21E3();
		__21CE.GlobalItem = false;
		__21CE.ContainerControl = this;
		__21CE.Displayed = true;
		__21CE.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		__21CE.SetOwner(this);
		_21DC();
		SetBaseItemContainer(__21CE);
	}

	private CrumbBarItem _21D4(IList _21D5, string _0956)
	{
		foreach (CrumbBarItem item in _21D5)
		{
			if (item.Name == _0956)
			{
				return item;
			}
			CrumbBarItem crumbBarItem2 = _21D4(item.SubItems, _0956);
			if (crumbBarItem2 != null)
			{
				return crumbBarItem2;
			}
		}
		return null;
	}

	public CrumbBarItem FindByName(string name)
	{
		return _21D4(Items, name);
	}

	internal void _21D6()
	{
		SelectedItem = null;
	}

	public void SetSelectedItem(CrumbBarItem selection, eEventSource source)
	{
		bool flag = selection != __21D1;
		if (flag)
		{
			CrumbBarSelectionEventArgs crumbBarSelectionEventArgs = new CrumbBarSelectionEventArgs(selection);
			OnSelectedItemChanging(crumbBarSelectionEventArgs);
			if (crumbBarSelectionEventArgs.Cancel)
			{
				return;
			}
			selection = crumbBarSelectionEventArgs.NewSelectedItem;
		}
		if (__21D1 != null)
		{
			__21D1.IsSelected = false;
		}
		ArrayList arrayList = new ArrayList();
		if (selection == null)
		{
			selection = GetFirstVisibleItem();
		}
		__21CE.Expanded = false;
		__21CE._21E9();
		if (selection != null)
		{
			for (CrumbBarItem crumbBarItem = selection; crumbBarItem != null; crumbBarItem = crumbBarItem.Parent as CrumbBarItem)
			{
				arrayList.Insert(0, _21DA(crumbBarItem, _21DB: true));
			}
			_21D8(selection);
		}
		else
		{
			_21D8(null);
		}
		__21CE._21EA();
		if (selection != null)
		{
			__21CE.SubItems.AddRange((BaseItem[])arrayList.ToArray(typeof(BaseItem)));
		}
		__21CE.NeedRecalcSize = true;
		__21D1 = selection;
		if (__21D1 != null)
		{
			__21D1.IsSelected = true;
		}
		RecalcLayout();
		if (flag)
		{
			CrumbBarSelectionEventArgs e = new CrumbBarSelectionEventArgs(selection);
			OnSelectedItemChanged(e);
		}
	}

	internal void _21D7()
	{
		_21D8(__21D1);
	}

	private void _21D8(CrumbBarItem _21D9)
	{
		if (_21D9 == null)
		{
			__21CE._21E5.Visible = false;
			return;
		}
		ᇎ ᇎ2 = _21D9._0EC5();
		if (ᇎ2 != null)
		{
			__21CE._21E5.Visible = true;
			if (ᇎ2.ᇏ)
			{
				__21CE._21E5.Icon = ᇎ2.ᆻ;
			}
			else
			{
				__21CE._21E5.Image = ᇎ2.ᇑ;
			}
		}
		else
		{
			__21CE._21E5.Visible = false;
		}
	}

	public bool ShowSelectedItemPopupMenu()
	{
		CrumbBarItem selectedItem = SelectedItem;
		if (selectedItem == null)
		{
			throw new NullReferenceException("SelectedItem is null");
		}
		if (_21DA(selectedItem, _21DB: false) is CrumbBarItemView { Expanded: false, AttachedItem: not null } crumbBarItemView && crumbBarItemView.AttachedItem.SubItems.Count > 0)
		{
			crumbBarItemView.Expanded = true;
			return true;
		}
		return false;
	}

	private object _21DA(CrumbBarItem _06E7, bool _21DB)
	{
		for (int i = 2; i < __21CE.SubItems.Count; i++)
		{
			if (__21CE.SubItems[i] is CrumbBarItemView crumbBarItemView && crumbBarItemView.AttachedItem == _06E7)
			{
				return crumbBarItemView;
			}
		}
		if (_21DB)
		{
			return CrumbBarItemView._223F(_06E7);
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public CrumbBarItem GetFirstVisibleItem()
	{
		foreach (CrumbBarItem item in Items)
		{
			if (item.Visible)
			{
				return item;
			}
		}
		return null;
	}

	public bool GetIsInSelectedPath(CrumbBarItem item)
	{
		for (int i = 2; i < __21CE.SubItems.Count; i++)
		{
			if (__21CE.SubItems[i] is CrumbBarItemView crumbBarItemView && crumbBarItemView.AttachedItem == item)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void OnStyleChanged(eCrumbBarStyle oldValue, eCrumbBarStyle newValue)
	{
		((Control)this).Invalidate();
	}

	public override BaseRenderer GetRenderer()
	{
		if (base.RenderMode == eRenderMode.Global && _ᎌ == eCrumbBarStyle.Vista)
		{
			return __21CF;
		}
		return base.GetRenderer();
	}

	private void _21DC()
	{
		__21CF = new Office2007Renderer();
		Office2007ColorTable colorTable = __21CF.ColorTable;
		colorTable.CrumbBarItemView = _21DD();
		colorTable.Menu.Background = new LinearGradientColorTable(ColorScheme.GetColor("FFF0F0F0"));
		colorTable.Menu.Border = new LinearGradientColorTable(ColorScheme.GetColor("FF646464"));
		colorTable.Menu.Side = new LinearGradientColorTable(ColorScheme.GetColor("FFF1F1F1"));
		colorTable.Menu.SideBorder = new LinearGradientColorTable(ColorScheme.GetColor("FFE2E3E3"));
		colorTable.Menu.SideBorderLight = new LinearGradientColorTable(ColorScheme.GetColor("FFFFFFFF"));
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[0];
		office2007ButtonItemColorTable.Default.Text = Color.Black;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ColorScheme.GetColor("34C5EBFF"), ColorScheme.GetColor("7081D8FF"), 90);
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ColorScheme.GetColor("FF96DBFA"));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ColorScheme.GetColor("A0FFFFFF"));
		office2007ButtonItemColorTable.MouseOver.Text = Color.Black;
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemColorTable.MouseOver;
	}

	private CrumbBarItemViewColorTable _21DD()
	{
		return _21DD(new ColorFactory());
	}

	internal static CrumbBarItemViewColorTable _21DD(ColorFactory ቖ)
	{
		CrumbBarItemViewColorTable crumbBarItemViewColorTable = new CrumbBarItemViewColorTable();
		(crumbBarItemViewColorTable.Default = new CrumbBarItemViewStateColorTable()).Foreground = Color.Black;
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = (crumbBarItemViewColorTable.MouseOver = new CrumbBarItemViewStateColorTable());
		crumbBarItemViewStateColorTable.Foreground = Color.Black;
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("EAF6FD")), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("D7EFFC")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("BDE6FD")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("A6D9F4")), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ColorScheme.GetColor("3C7FB1"));
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor(ColorScheme.GetColor("E0FFFFFF"));
		crumbBarItemViewStateColorTable = (crumbBarItemViewColorTable.MouseOverInactive = new CrumbBarItemViewStateColorTable());
		crumbBarItemViewStateColorTable.Foreground = Color.Black;
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFF2F2F2")), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFEAEAEA")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFDCDCDC")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFCFCFCF")), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ColorScheme.GetColor("FF8E8F8F"));
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor(ColorScheme.GetColor("E0FFFFFF"));
		crumbBarItemViewStateColorTable = (crumbBarItemViewColorTable.Pressed = new CrumbBarItemViewStateColorTable());
		crumbBarItemViewStateColorTable.Foreground = Color.Black;
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFC2E4F6")), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFC2E4F6")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FFA9D9F2")), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(ColorScheme.GetColor("FF90CBEB")), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ColorScheme.GetColor("FF6E8D9F"));
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor(ColorScheme.GetColor("906E8D9F"));
		return crumbBarItemViewColorTable;
	}

	protected virtual void OnSelectedItemChanging(CrumbBarSelectionEventArgs e)
	{
		_21D0?.Invoke(this, e);
	}

	protected virtual void OnSelectedItemChanged(CrumbBarSelectionEventArgs e)
	{
		_20DD?.Invoke(this, e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!__21D2.IsEmpty)
		{
			return __21D2;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (Items.Count == 0 || !BarFunctions.IsHandleValid((Control)(object)this) || __21CE.SubItems.Count == 0)
		{
			return new Size(((Control)this).GetPreferredSize(proposedSize).Width, 22);
		}
		int num = ᝈ.ᝏ(GetBackgroundStyle());
		num += ((__21CE._21E4 > 0) ? __21CE._21E4 : 20);
		__21D2 = new Size(proposedSize.Width, num);
		return __21D2;
	}

	private void _21DE()
	{
		__21D2 = Size.Empty;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0)
			{
				width = preferredSize.Width;
			}
			if (preferredSize.Height > 0)
			{
				height = preferredSize.Height;
			}
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0 && preferredSize.Height > 0)
			{
				((Control)this).Size = ((Control)this).PreferredSize;
			}
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
	}

	internal static string _175E(CrumbBarItem _06EC, string _175F)
	{
		if (_06EC == null)
		{
			throw new ArgumentNullException("node");
		}
		StringBuilder stringBuilder = new StringBuilder(_06EC.Text);
		for (_06EC = _06EC.Parent as CrumbBarItem; _06EC != null; _06EC = _06EC.Parent as CrumbBarItem)
		{
			stringBuilder.Insert(0, _06EC.Text + _175F);
		}
		return stringBuilder.ToString();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		if (_089F.फ() || !_089F.थ)
		{
			e.Graphics.Clear(SystemColors.Control);
		}
	}
}
