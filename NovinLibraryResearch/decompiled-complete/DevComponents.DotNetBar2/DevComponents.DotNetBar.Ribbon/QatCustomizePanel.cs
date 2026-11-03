using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Ribbon;

public class QatCustomizePanel : UserControl
{
	public ItemPanel itemPanelCommands;

	public ItemPanel itemPanelQat;

	public ButtonX buttonAddToQat;

	public ButtonX buttonRemoveFromQat;

	public ComboBoxEx comboCategories;

	public Label labelCategories;

	public CheckBoxX checkQatBelowRibbon;

	private Hashtable _23DD = new Hashtable();

	private bool _23DE;

	private IContainer Պ;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool DataChanged
	{
		get
		{
			return _23DE;
		}
		set
		{
			_23DE = value;
		}
	}

	public QatCustomizePanel()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		ـ();
	}

	private void ـ()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		itemPanelCommands = new ItemPanel();
		itemPanelQat = new ItemPanel();
		buttonAddToQat = new ButtonX();
		buttonRemoveFromQat = new ButtonX();
		comboCategories = new ComboBoxEx();
		labelCategories = new Label();
		checkQatBelowRibbon = new CheckBoxX();
		((Control)this).SuspendLayout();
		((Control)itemPanelCommands).Anchor = (AnchorStyles)15;
		itemPanelCommands.AutoScroll = true;
		itemPanelCommands.BackgroundStyle.Class = ElementStyleClassKeys.ItemPanelKey;
		itemPanelCommands.Style = eDotNetBarStyle.StyleManagerControlled;
		itemPanelCommands.FadeEffect = false;
		itemPanelCommands.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanelCommands).Location = new Point(9, 46);
		((Control)itemPanelCommands).Name = "itemPanelCommands";
		((Control)itemPanelCommands).Size = new Size(173, 257);
		((Control)itemPanelCommands).TabIndex = 2;
		((Control)itemPanelCommands).Text = "itemPanelCommands";
		((Control)itemPanelCommands).KeyUp += new KeyEventHandler(_23DF);
		itemPanelCommands.ItemDoubleClick += new MouseEventHandler(_23EC);
		((Control)itemPanelQat).Anchor = (AnchorStyles)11;
		itemPanelQat.AutoScroll = true;
		itemPanelQat.BackgroundStyle.Class = ElementStyleClassKeys.ItemPanelKey;
		itemPanelQat.Style = eDotNetBarStyle.StyleManagerControlled;
		itemPanelQat.EnableDragDrop = true;
		itemPanelQat.FadeEffect = false;
		itemPanelQat.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanelQat).Location = new Point(266, 46);
		((Control)itemPanelQat).Name = "itemPanelQat";
		((Control)itemPanelQat).Size = new Size(173, 257);
		((Control)itemPanelQat).TabIndex = 5;
		((Control)itemPanelQat).Text = "itemPanelCommands";
		((Control)itemPanelQat).DragDrop += new DragEventHandler(_23F2);
		itemPanelQat.UserCustomize += _23F3;
		((Control)itemPanelQat).KeyUp += new KeyEventHandler(_23DF);
		itemPanelQat.ItemDoubleClick += new MouseEventHandler(_23F1);
		((Control)buttonAddToQat).Anchor = (AnchorStyles)9;
		buttonAddToQat.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonAddToQat).Location = new Point(188, 126);
		((Control)buttonAddToQat).Name = "buttonAddToQat";
		((Control)buttonAddToQat).Size = new Size(73, 21);
		((Control)buttonAddToQat).TabIndex = 3;
		((Control)buttonAddToQat).Text = "&Add >>";
		((Control)buttonAddToQat).Click += _23EE;
		((Control)buttonRemoveFromQat).Anchor = (AnchorStyles)9;
		buttonRemoveFromQat.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRemoveFromQat).Location = new Point(188, 153);
		((Control)buttonRemoveFromQat).Name = "buttonRemoveFromQat";
		((Control)buttonRemoveFromQat).Size = new Size(73, 21);
		((Control)buttonRemoveFromQat).TabIndex = 4;
		((Control)buttonRemoveFromQat).Text = "&Remove";
		((Control)buttonRemoveFromQat).Click += _23F0;
		((Control)comboCategories).Anchor = (AnchorStyles)13;
		((ComboBox)comboCategories).DropDownStyle = (ComboBoxStyle)2;
		((Control)comboCategories).Location = new Point(9, 19);
		((Control)comboCategories).Name = "comboCategories";
		((Control)comboCategories).Size = new Size(173, 21);
		((ComboBox)comboCategories).Sorted = true;
		((Control)comboCategories).TabIndex = 1;
		((ComboBox)comboCategories).SelectedIndexChanged += _23EB;
		comboCategories.Style = eDotNetBarStyle.Office2007;
		((ComboBox)comboCategories).DrawMode = (DrawMode)1;
		comboCategories.ThemeAware = false;
		((Control)labelCategories).AutoSize = true;
		((Control)labelCategories).BackColor = Color.Transparent;
		((Control)labelCategories).Location = new Point(6, 3);
		((Control)labelCategories).Name = "labelCategories";
		((Control)labelCategories).Size = new Size(123, 13);
		((Control)labelCategories).TabIndex = 0;
		((Control)labelCategories).Text = "&Choose commands from:";
		((Control)checkQatBelowRibbon).Anchor = (AnchorStyles)6;
		((Control)checkQatBelowRibbon).BackColor = Color.Transparent;
		((Control)checkQatBelowRibbon).Location = new Point(9, 311);
		((Control)checkQatBelowRibbon).Name = "checkQatBelowRibbon";
		((Control)checkQatBelowRibbon).Size = new Size(420, 17);
		((Control)checkQatBelowRibbon).TabIndex = 6;
		((Control)checkQatBelowRibbon).Text = "&Place Quick Access Toolbar below the Ribbon";
		checkQatBelowRibbon.CheckedChangedEx += _23F4;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)comboCategories);
		((Control)this).Controls.Add((Control)(object)checkQatBelowRibbon);
		((Control)this).Controls.Add((Control)(object)labelCategories);
		((Control)this).Controls.Add((Control)(object)buttonRemoveFromQat);
		((Control)this).Controls.Add((Control)(object)buttonAddToQat);
		((Control)this).Controls.Add((Control)(object)itemPanelQat);
		((Control)this).Controls.Add((Control)(object)itemPanelCommands);
		((Control)this).Name = "QatCustomizePanel";
		((Control)this).Size = new Size(444, 334);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void _23DF(object _٠, KeyEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if ((int)ؿ.KeyCode != 38 && (int)ؿ.KeyCode != 40 && (int)ؿ.KeyCode != 37 && (int)ؿ.KeyCode != 39)
		{
			return;
		}
		ItemPanel itemPanel = _٠ as ItemPanel;
		foreach (BaseItem item in itemPanel.Items)
		{
			if (item is ButtonItem { IsMouseOver: not false, Checked: false } buttonItem)
			{
				buttonItem.Checked = true;
				break;
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackColor.A < byte.MaxValue)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		if (!(((Control)this).BackColor == Color.Transparent))
		{
			DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor);
		}
	}

	public void LoadItems(RibbonControl rc)
	{
		_23DD.Clear();
		itemPanelCommands.Items.Clear();
		itemPanelQat.Items.Clear();
		_23E8(rc);
		if (rc.CategorizeMode == eCategorizeMode.Categories)
		{
			_23E0(rc);
		}
		else
		{
			_23E2(rc);
		}
		_23EA();
		checkQatBelowRibbon.Checked = rc.QatPositionedBelowRibbon;
		((Control)checkQatBelowRibbon).Visible = rc.EnableQatPlacement;
		_23DE = false;
	}

	public void LoadItems(MetroShell tab)
	{
		_23DD.Clear();
		itemPanelCommands.Items.Clear();
		itemPanelQat.Items.Clear();
		_23E8(tab);
		if (tab.CategorizeMode == eMetroCategorizeMode.Categories)
		{
			_23E0(tab);
		}
		else
		{
			_23E3(tab);
		}
		_23EA();
		((Control)checkQatBelowRibbon).Visible = false;
		_23DE = false;
	}

	private void _23E0(RibbonControl ᑠ)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)ᑠ).Controls)
		{
			Control ꮔ = item;
			_23E0(ꮔ);
		}
		BaseItem applicationButton = ᑠ.GetApplicationButton();
		if (applicationButton != null)
		{
			_23E0(applicationButton);
		}
	}

	private void _23E0(MetroShell ᆰ)
	{
		List<IOwner> ꮯ = ᆰ.Ꮯ;
		foreach (IOwner item in ꮯ)
		{
			if (item is MetroToolbar)
			{
				_23E0((MetroToolbar)item);
			}
		}
		BaseItem applicationButton = ᆰ.GetApplicationButton();
		if (applicationButton != null)
		{
			_23E0(applicationButton);
		}
	}

	private void _23E0(MetroToolbar ᐘ)
	{
		foreach (BaseItem item in ᐘ.Items)
		{
			_23E0(item);
		}
		foreach (BaseItem extraItem in ᐘ.ExtraItems)
		{
			_23E0(extraItem);
		}
	}

	private void _23E0(Control Ꮔ)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (Ꮔ is RibbonBar)
		{
			_23E0(Ꮔ as RibbonBar);
		}
		foreach (Control item in (ArrangedElementCollection)Ꮔ.Controls)
		{
			Control ꮔ = item;
			_23E0(ꮔ);
		}
	}

	private void _23E0(RibbonBar ච)
	{
		foreach (BaseItem item in ච.Items)
		{
			_23E0(item);
		}
	}

	private void _23E0(BaseItem _06EC)
	{
		if (ᐌ(_06EC))
		{
			string key = _23E1(_06EC);
			ArrayList arrayList = _23DD[key] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				_23DD.Add(key, arrayList);
			}
			arrayList.Add(_23E9(_06EC));
		}
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			_23E0(subItem);
		}
		if (!(_06EC is GalleryContainer))
		{
			return;
		}
		GalleryContainer galleryContainer = _06EC as GalleryContainer;
		foreach (BaseItem popupGalleryItem in galleryContainer.PopupGalleryItems)
		{
			_23E0(popupGalleryItem);
		}
	}

	private string _23E1(BaseItem _06EC)
	{
		if (_06EC.Category != "")
		{
			return _06EC.Category;
		}
		return "Unassigned";
	}

	private void _23E2(RibbonControl ᑠ)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)ᑠ).Controls)
		{
			Control ꮔ = item;
			_23E2(ꮔ);
		}
		BaseItem applicationButton = ᑠ.GetApplicationButton();
		if (applicationButton != null)
		{
			_23E2(applicationButton, _23E4(applicationButton.Text));
		}
	}

	private void _23E3(MetroShell ᑠ)
	{
		List<IOwner> ꮯ = ᑠ.Ꮯ;
		foreach (IOwner item in ꮯ)
		{
			if (item is MetroToolbar)
			{
				_23E3((MetroToolbar)item);
			}
		}
		BaseItem applicationButton = ᑠ.GetApplicationButton();
		if (applicationButton != null)
		{
			_23E2(applicationButton, _23E6(applicationButton.Text));
		}
	}

	private void _23E3(MetroToolbar ච)
	{
		foreach (BaseItem item in ච.Items)
		{
			_23E2(item, _23E6(((Control)ච).Text));
		}
		foreach (BaseItem extraItem in ච.ExtraItems)
		{
			_23E2(extraItem, _23E6(((Control)ච).Text));
		}
	}

	private void _23E2(Control Ꮔ)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (Ꮔ is RibbonBar)
		{
			_23E2(Ꮔ as RibbonBar);
		}
		foreach (Control item in (ArrangedElementCollection)Ꮔ.Controls)
		{
			Control ꮔ = item;
			_23E2(ꮔ);
		}
	}

	private void _23E2(RibbonBar ච)
	{
		foreach (BaseItem item in ච.Items)
		{
			_23E2(item, _23E4(((Control)ච).Text));
		}
	}

	private string _23E4(string _23E5)
	{
		StringBuilder stringBuilder = new StringBuilder(_23E5.Length);
		for (int i = 0; i < _23E5.Length; i++)
		{
			if (_23E5[i] == '&')
			{
				if (i + 1 < _23E5.Length && _23E5[i + 1] == '&')
				{
					stringBuilder.Append(_23E5[i]);
				}
			}
			else
			{
				stringBuilder.Append(_23E5[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private string _23E6(string _23E5)
	{
		StringBuilder stringBuilder = new StringBuilder(_23E5.Length);
		for (int i = 0; i < _23E5.Length; i++)
		{
			if (_23E5[i] == '&')
			{
				if (i + 1 < _23E5.Length && _23E5[i + 1] == '&')
				{
					stringBuilder.Append(_23E5[i]);
				}
			}
			else
			{
				stringBuilder.Append(_23E5[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private void _23E2(BaseItem _06EC, string _23E7)
	{
		if (ᐌ(_06EC))
		{
			ArrayList arrayList = _23DD[_23E7] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				_23DD.Add(_23E7, arrayList);
			}
			arrayList.Add(_23E9(_06EC));
		}
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			_23E2(subItem, _23E7);
		}
		if (!(_06EC is GalleryContainer))
		{
			return;
		}
		GalleryContainer galleryContainer = _06EC as GalleryContainer;
		foreach (BaseItem popupGalleryItem in galleryContainer.PopupGalleryItems)
		{
			_23E2(popupGalleryItem, _23E7);
		}
	}

	private void _23E8(RibbonControl ᑠ)
	{
		int count = ᑠ.QuickToolbarItems.Count;
		int num = 0;
		BaseItem applicationButton = ᑠ.GetApplicationButton();
		if (applicationButton != null)
		{
			num = ᑠ.QuickToolbarItems.IndexOf(applicationButton) + 1;
		}
		for (int i = num; i < count; i++)
		{
			BaseItem baseItem = ᑠ.QuickToolbarItems[i];
			if (!ᐒ(baseItem))
			{
				BaseItem baseItem2 = _23E9(baseItem);
				baseItem2.Tag = null;
				if (!baseItem.CanCustomize)
				{
					baseItem2.Visible = false;
				}
				itemPanelQat.Items.Add(baseItem2);
			}
		}
	}

	private void _23E8(MetroShell ᆰ)
	{
		int count = ᆰ.QuickToolbarItems.Count;
		int num = 0;
		BaseItem applicationButton = ᆰ.GetApplicationButton();
		if (applicationButton != null)
		{
			num = ᆰ.QuickToolbarItems.IndexOf(applicationButton) + 1;
		}
		for (int i = num; i < count; i++)
		{
			BaseItem baseItem = ᆰ.QuickToolbarItems[i];
			if (!ᐒ(baseItem))
			{
				BaseItem baseItem2 = _23E9(baseItem);
				baseItem2.Tag = null;
				if (!baseItem.CanCustomize)
				{
					baseItem2.Visible = false;
				}
				itemPanelQat.Items.Add(baseItem2);
			}
		}
	}

	private bool ᐒ(BaseItem _06EC)
	{
		if (_06EC.SystemItem || _06EC is ItemContainer || _06EC is CustomizeItem || _06EC is SystemCaptionItem)
		{
			return true;
		}
		return false;
	}

	private ButtonItem _23E9(BaseItem _06EC)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		ButtonItem buttonItem = new ButtonItem(_06EC.Name);
		buttonItem.Text = _06EC.Text;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem.ImagePaddingVertical = 4;
		buttonItem.OptionGroup = "sys";
		buttonItem.GlobalItem = false;
		if (buttonItem.Text == "")
		{
			buttonItem.Text = "Unassigned";
		}
		buttonItem.Tag = _06EC;
		if (_06EC is ButtonItem)
		{
			ButtonItem buttonItem2 = _06EC as ButtonItem;
			if (buttonItem2.ImageSmall != null)
			{
				buttonItem.Image = buttonItem2.ImageSmall;
				if (buttonItem2.ImageSmall.Width != 16 || buttonItem2.ImageSmall.Height != 16)
				{
					buttonItem.ImageFixedSize = new Size(16, 16);
				}
			}
			else
			{
				ᇎ ᇎ = buttonItem2._0EC5(_1937.ሬ, Color.Black);
				if (ᇎ != null)
				{
					if (ᇎ.ᇏ)
					{
						buttonItem.Icon = (Icon)ᇎ.ᆻ.Clone();
					}
					else
					{
						buttonItem.Image = ᇎ.ᇑ;
					}
					buttonItem.ImageFixedSize = new Size(16, 16);
				}
			}
		}
		else if (_06EC is ComboBoxItem || _06EC is TextBoxItem || _06EC is ControlContainerItem)
		{
			buttonItem.ImagePosition = eImagePosition.Right;
			buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.QatCustomizeItemCombo.png");
		}
		return buttonItem;
	}

	private bool ᐌ(BaseItem _06EC)
	{
		if (_06EC == null)
		{
			return false;
		}
		if (!_06EC.CanCustomize || _06EC.SystemItem || _06EC.Name == "tempColorPickerItem")
		{
			return false;
		}
		if (_06EC is ItemContainer || _06EC is GenericItemContainer)
		{
			return false;
		}
		return true;
	}

	private void _23EA()
	{
		comboCategories.Items.Clear();
		foreach (string key in _23DD.Keys)
		{
			comboCategories.Items.Add((object)key);
		}
		if (comboCategories.Items.Count > 0)
		{
			((ListControl)comboCategories).SelectedIndex = 0;
		}
	}

	private void _23EB(object _٠, EventArgs ؿ)
	{
		if (((ListControl)comboCategories).SelectedIndex < 0)
		{
			itemPanelCommands.Items.Clear();
			itemPanelCommands.RecalcLayout();
			return;
		}
		string key = comboCategories.Items[((ListControl)comboCategories).SelectedIndex].ToString();
		ArrayList arrayList = _23DD[key] as ArrayList;
		itemPanelCommands.Items.Clear();
		foreach (BaseItem item in arrayList)
		{
			itemPanelCommands.Items.Add(item);
		}
		itemPanelCommands.RecalcLayout();
	}

	private void _23EC(object _٠, MouseEventArgs ؿ)
	{
		_23ED();
	}

	private void _23ED()
	{
		ButtonItem @checked = itemPanelCommands.GetChecked();
		if (@checked != null && !itemPanelQat.Items.Contains(@checked.Name))
		{
			ButtonItem buttonItem = @checked.Copy() as ButtonItem;
			buttonItem.Checked = false;
			itemPanelQat.Items.Add(buttonItem);
			buttonItem.Checked = true;
			itemPanelQat.RecalcLayout();
			_23DE = true;
			int num = itemPanelCommands.Items.IndexOf(@checked) + 1;
			if (num != itemPanelCommands.Items.Count && itemPanelCommands.Items[num] is ButtonItem buttonItem2)
			{
				buttonItem2.Checked = true;
				itemPanelCommands.EnsureVisible(buttonItem2);
			}
		}
	}

	private void _23EE(object _٠, EventArgs ؿ)
	{
		_23ED();
	}

	private void _23EF()
	{
		ButtonItem @checked = itemPanelQat.GetChecked();
		if (@checked == null)
		{
			return;
		}
		int num = itemPanelQat.Items.IndexOf(@checked);
		itemPanelQat.Items.Remove(@checked);
		if (num >= 0 && itemPanelQat.Items.Count > 0)
		{
			if (num + 1 >= itemPanelQat.Items.Count)
			{
				num = itemPanelQat.Items.Count - 1;
			}
			if (itemPanelQat.Items[num] is ButtonItem buttonItem)
			{
				buttonItem.Checked = true;
				itemPanelQat.EnsureVisible(buttonItem);
			}
		}
		itemPanelQat.RecalcLayout();
		_23DE = true;
	}

	private void _23F0(object _٠, EventArgs ؿ)
	{
		_23EF();
	}

	private void _23F1(object _٠, MouseEventArgs ؿ)
	{
		_23EF();
	}

	private void _23F2(object _٠, DragEventArgs ؿ)
	{
		_23DE = true;
	}

	private void _23F3(object _٠, EventArgs ؿ)
	{
		_23DE = true;
	}

	private void _23F4(object _٠, CheckBoxXChangeEventArgs ؿ)
	{
		_23DE = true;
	}
}
