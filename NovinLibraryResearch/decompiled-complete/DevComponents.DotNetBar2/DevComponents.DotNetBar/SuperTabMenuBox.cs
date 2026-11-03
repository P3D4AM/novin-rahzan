using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class SuperTabMenuBox : PopupItem
{
	private SuperTabControlBox __2075;

	private Size _ᡍ = new Size(16, 16);

	private bool _ᆿ;

	private bool __171A;

	private bool __2076;

	private bool __2077 = true;

	private bool __2078 = true;

	private bool __2079;

	[Browsable(false)]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Browsable(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Browsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[Browsable(false)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
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

	[Browsable(false)]
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	[Browsable(false)]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Browsable(false)]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Browsable(false)]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public override string GlobalName
	{
		get
		{
			return base.GlobalName;
		}
		set
		{
			base.GlobalName = value;
		}
	}

	[Browsable(false)]
	public override Command Command
	{
		get
		{
			return base.Command;
		}
		set
		{
			base.Command = value;
		}
	}

	[Browsable(false)]
	public override object CommandParameter
	{
		get
		{
			return base.CommandParameter;
		}
		set
		{
			base.CommandParameter = value;
		}
	}

	[Description("Indicates whether the MenuBox is automatically hidden when the tab items size does not exceed the size of the control.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool AutoHide
	{
		get
		{
			return __2076;
		}
		set
		{
			if (__2076 != value)
			{
				__2076 = value;
				Visible = !value || !_2015();
				ᾦ();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the MenuBox raises a ClickEvent when selected.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool RaiseClickOnSelection
	{
		get
		{
			return __2079;
		}
		set
		{
			__2079 = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether MenuBox shows only Tabs entries.")]
	public bool ShowTabsOnly
	{
		get
		{
			return __2077;
		}
		set
		{
			__2077 = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether the MenuBox displays each menu entry with its associated Image/Icon.")]
	public bool ShowImages
	{
		get
		{
			return __2078;
		}
		set
		{
			__2078 = value;
		}
	}

	[Description("Indicates MenuBox Visible state.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (base.Visible != value)
			{
				base.Visible = value;
				ᾦ();
			}
		}
	}

	internal bool _206D => __171A;

	internal bool ᆼ => _ᆿ;

	public SuperTabMenuBox(SuperTabControlBox controlBox)
	{
		__2075 = controlBox;
		Style = eDotNetBarStyle.Office2007;
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		WidthInternal = Dpi.Width(_ᡍ.Width);
		HeightInternal = Dpi.Height(_ᡍ.Height);
	}

	private void ᾦ()
	{
		if (__2075.έ != null)
		{
			SuperTabStripItem superTabStripItem = __2075.έ._1FD4;
			if (superTabStripItem != null)
			{
				__2075.NeedRecalcSize = true;
				__2075.RecalcSize();
				superTabStripItem.NeedRecalcSize = true;
				superTabStripItem.Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		using SuperTabColorTable superTabColorTable = __2075.έ.ጧ();
		Color image = superTabColorTable.ControlBoxDefault.Image;
		if (ᆼ)
		{
			Rectangle bounds = Bounds;
			bounds.Width--;
			bounds.Height--;
			image = superTabColorTable.ControlBoxMouseOver.Image;
			Brush val = (Brush)new SolidBrush(superTabColorTable.ControlBoxMouseOver.Background);
			try
			{
				graphics.FillRectangle(val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Pen val2 = new Pen(superTabColorTable.ControlBoxMouseOver.Border);
			try
			{
				graphics.DrawRectangle(val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (_2015())
		{
			graphics.DrawImageUnscaled((Image)(object)_2029(graphics, image), Bounds);
		}
		else
		{
			graphics.DrawImageUnscaled((Image)(object)_202B(graphics, image), Bounds);
		}
	}

	private bool _2015()
	{
		SubItemsCollection ᇃ = __2075.έ.ᇃ;
		for (int i = 0; i < ᇃ.Count; i++)
		{
			if (ᇃ[i].Visible && !ᇃ[i].Displayed)
			{
				return false;
			}
		}
		return true;
	}

	private Bitmap _2029(Graphics _٥, Color _065B)
	{
		return __2075.έ._2029(_٥, _065B);
	}

	private Bitmap _202B(Graphics _٥, Color _065B)
	{
		return __2075.έ._202B(_٥, _065B);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_ᆿ = true;
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_ᆿ = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		__171A = true;
		if (!DesignMode && (int)objArg.Button == 1048576)
		{
			_207B();
			Point p = _207A();
			Popup(p);
		}
	}

	private Point _207A()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		Point point = new Point(Bounds.X, Bounds.Bottom);
		Control val = (Control)__2075.GetContainerControl(checkIsContainer: true);
		if (val != null)
		{
			return val.PointToScreen(point);
		}
		return point;
	}

	private void _207B()
	{
		SubItems.Clear();
		SubItemsCollection ᇃ = __2075.έ.ᇃ;
		if (__2077)
		{
			_207C(ᇃ);
		}
		else
		{
			_207E(ᇃ);
		}
	}

	private void _207C(SubItemsCollection _207D)
	{
		for (int i = 0; i < _207D.Count; i++)
		{
			if (_207D[i] is SuperTabItem { Visible: not false } superTabItem)
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.Tag = superTabItem;
				buttonItem.Text = superTabItem.Text;
				if (superTabItem.Enabled)
				{
					buttonItem.Click += _2081;
				}
				else
				{
					buttonItem.Enabled = false;
				}
				SubItems.Add(buttonItem);
				if (__2078)
				{
					ⁿ(buttonItem, superTabItem);
				}
				if (superTabItem.IsSelected)
				{
					buttonItem.Checked = true;
				}
			}
		}
	}

	private void _207E(SubItemsCollection _207D)
	{
		ButtonItem[] array = new ButtonItem[_207D.Count];
		for (int i = 0; i < _207D.Count; i++)
		{
			if (!_207D[i].Visible)
			{
				continue;
			}
			array[i] = new ButtonItem();
			array[i].Tag = _207D[i];
			array[i].Text = _207D[i].Text;
			if (_207D[i].Enabled)
			{
				array[i].Click += _2081;
			}
			else
			{
				array[i].Enabled = false;
			}
			if (__2078)
			{
				if (_207D[i] is SuperTabItem)
				{
					ⁿ(array[i], ((SuperTabItem)_207D[i]).ᕂ());
				}
				else if (_207D[i] is ButtonItem)
				{
					ⁿ(array[i], ((ButtonItem)_207D[i])._0EC5());
				}
			}
		}
		int selectedTabIndex = __2075.έ._1FD4.SelectedTabIndex;
		if (selectedTabIndex >= 0 && selectedTabIndex < _207D.Count)
		{
			array[selectedTabIndex].Checked = true;
		}
		SubItems.AddRange(array);
	}

	private void ⁿ(ButtonItem _2080, SuperTabItem ᆰ)
	{
		if (!string.IsNullOrEmpty(ᆰ.SymbolRealized))
		{
			_2080.Symbol = ᆰ.Symbol;
			_2080.SymbolSize = ᆰ.SymbolSize;
			_2080.SymbolColor = ᆰ.Ὀ.Ύ();
		}
		else
		{
			ⁿ(_2080, ᆰ.ᕂ());
		}
	}

	private void ⁿ(ButtonItem _2080, ᇎ ແ)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		if (ແ == null)
		{
			return;
		}
		if (ແ.ᆻ != null)
		{
			_2080.Icon = ແ.ᆻ;
		}
		else if (ແ.ᇑ != null)
		{
			if (ImageAnimator.CanAnimate(ແ.ᇑ))
			{
				FrameDimension val = new FrameDimension(ແ.ᇑ.FrameDimensionsList[0]);
				ແ.ᇑ.SelectActiveFrame(val, 0);
				_2080.Image = (Image)new Bitmap(ແ.ᇑ);
				_2080.NeedRecalcSize = true;
			}
			else
			{
				_2080.Image = ແ.ᇑ;
			}
		}
	}

	private void _2081(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem { Tag: BaseItem tag } buttonItem)
		{
			if (buttonItem.Tag is SuperTabItem selectedTab)
			{
				__2075.έ._1FD4.SelectedTab = selectedTab;
			}
			else
			{
				__2075.έ._1FD4.ή = tag;
			}
			if (__2079)
			{
				tag.RaiseClick(eEventSource.Code);
			}
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		__171A = false;
		Refresh();
	}

	public override BaseItem Copy()
	{
		SuperTabMenuBox superTabMenuBox = new SuperTabMenuBox(__2075);
		CopyToItem(superTabMenuBox);
		return superTabMenuBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabMenuBox copy2 = copy as SuperTabMenuBox;
		base.CopyToItem((BaseItem)copy2);
	}
}
