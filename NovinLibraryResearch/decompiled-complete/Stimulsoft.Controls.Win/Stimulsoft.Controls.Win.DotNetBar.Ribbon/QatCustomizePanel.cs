using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Metro;

namespace Stimulsoft.Controls.Win.DotNetBar.Ribbon;

[ToolboxItem(false)]
public class QatCustomizePanel : UserControl
{
	public ItemPanel itemPanelCommands;

	public ItemPanel itemPanelQat;

	public ButtonX buttonAddToQat;

	public ButtonX buttonRemoveFromQat;

	public ComboBoxEx comboCategories;

	public Label labelCategories;

	public CheckBoxX checkQatBelowRibbon;

	private Hashtable m_Categories = new Hashtable();

	private bool m_DataChanged;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool DataChanged
	{
		get
		{
			return m_DataChanged;
		}
		set
		{
			m_DataChanged = value;
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
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		labelCategories = new Label();
		comboCategories = new ComboBoxEx();
		checkQatBelowRibbon = new CheckBoxX();
		buttonRemoveFromQat = new ButtonX();
		buttonAddToQat = new ButtonX();
		itemPanelQat = new ItemPanel();
		itemPanelCommands = new ItemPanel();
		((Control)this).SuspendLayout();
		((Control)labelCategories).AutoSize = true;
		((Control)labelCategories).BackColor = Color.Transparent;
		((Control)labelCategories).Location = new Point(12, 12);
		((Control)labelCategories).Name = "labelCategories";
		((Control)labelCategories).Size = new Size(123, 13);
		((Control)labelCategories).TabIndex = 0;
		((Control)labelCategories).Text = "&Choose commands from:";
		((Control)comboCategories).Anchor = (AnchorStyles)13;
		((ComboBox)comboCategories).DrawMode = (DrawMode)1;
		((ComboBox)comboCategories).DropDownStyle = (ComboBoxStyle)2;
		((Control)comboCategories).Location = new Point(15, 28);
		((Control)comboCategories).Name = "comboCategories";
		((Control)comboCategories).Size = new Size(173, 21);
		((ComboBox)comboCategories).Sorted = true;
		((Control)comboCategories).TabIndex = 1;
		((ComboBox)comboCategories).SelectedIndexChanged += comboCategories_SelectedIndexChanged;
		((Control)checkQatBelowRibbon).Anchor = (AnchorStyles)6;
		((Control)checkQatBelowRibbon).BackColor = Color.Transparent;
		checkQatBelowRibbon.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)checkQatBelowRibbon).Location = new Point(15, 311);
		((Control)checkQatBelowRibbon).Name = "checkQatBelowRibbon";
		((Control)checkQatBelowRibbon).Size = new Size(430, 17);
		((Control)checkQatBelowRibbon).TabIndex = 6;
		((Control)checkQatBelowRibbon).Text = "&Place Quick Access Toolbar below the Ribbon";
		checkQatBelowRibbon.CheckedChangedEx += checkQatBelowRibbon_CheckedChanged;
		((Control)buttonRemoveFromQat).AccessibleRole = (AccessibleRole)43;
		((Control)buttonRemoveFromQat).Anchor = (AnchorStyles)9;
		buttonRemoveFromQat.ColorTable = eButtonColor.OrangeWithBackground;
		buttonRemoveFromQat.Is20Button = false;
		((Control)buttonRemoveFromQat).Location = new Point(194, 162);
		((Control)buttonRemoveFromQat).Name = "buttonRemoveFromQat";
		((Control)buttonRemoveFromQat).Size = new Size(73, 21);
		((Control)buttonRemoveFromQat).TabIndex = 4;
		((Control)buttonRemoveFromQat).Text = "&Remove";
		((Control)buttonRemoveFromQat).Click += buttonRemoveFromQat_Click;
		((Control)buttonAddToQat).AccessibleRole = (AccessibleRole)43;
		((Control)buttonAddToQat).Anchor = (AnchorStyles)9;
		buttonAddToQat.ColorTable = eButtonColor.OrangeWithBackground;
		buttonAddToQat.Is20Button = false;
		((Control)buttonAddToQat).Location = new Point(194, 135);
		((Control)buttonAddToQat).Name = "buttonAddToQat";
		((Control)buttonAddToQat).Size = new Size(73, 21);
		((Control)buttonAddToQat).TabIndex = 3;
		((Control)buttonAddToQat).Text = "&Add >>";
		((Control)buttonAddToQat).Click += buttonAddToQat_Click;
		((Control)itemPanelQat).Anchor = (AnchorStyles)11;
		itemPanelQat.AutoScroll = true;
		itemPanelQat.BackgroundStyle.Class = "ItemPanel";
		itemPanelQat.BackgroundStyle.CornerType = eCornerType.Square;
		itemPanelQat.ContainerControlProcessDialogKey = true;
		itemPanelQat.EnableDragDrop = true;
		itemPanelQat.FadeEffect = false;
		itemPanelQat.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanelQat).Location = new Point(272, 55);
		((Control)itemPanelQat).Name = "itemPanelQat";
		((Control)itemPanelQat).Size = new Size(167, 242);
		itemPanelQat.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)itemPanelQat).TabIndex = 5;
		((Control)itemPanelQat).Text = "itemPanelCommands";
		itemPanelQat.ItemDoubleClick += new MouseEventHandler(ItemPanelQatDoubleClick);
		itemPanelQat.UserCustomize += itemPanelQat_UserCustomize;
		((Control)itemPanelQat).DragDrop += new DragEventHandler(itemPanelQat_DragDrop);
		((Control)itemPanelQat).KeyUp += new KeyEventHandler(ItemPanelKeyUp);
		((Control)itemPanelCommands).Anchor = (AnchorStyles)15;
		itemPanelCommands.AutoScroll = true;
		itemPanelCommands.BackgroundStyle.Class = "ItemPanel";
		itemPanelCommands.BackgroundStyle.CornerType = eCornerType.Square;
		itemPanelCommands.ContainerControlProcessDialogKey = true;
		itemPanelCommands.FadeEffect = false;
		itemPanelCommands.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanelCommands).Location = new Point(15, 55);
		((Control)itemPanelCommands).Name = "itemPanelCommands";
		((Control)itemPanelCommands).Size = new Size(173, 242);
		itemPanelCommands.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)itemPanelCommands).TabIndex = 2;
		((Control)itemPanelCommands).Text = "itemPanelCommands";
		itemPanelCommands.ItemDoubleClick += new MouseEventHandler(ItemPanelCommandsDoubleClick);
		((Control)itemPanelCommands).KeyUp += new KeyEventHandler(ItemPanelKeyUp);
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)comboCategories);
		((Control)this).Controls.Add((Control)(object)checkQatBelowRibbon);
		((Control)this).Controls.Add((Control)(object)labelCategories);
		((Control)this).Controls.Add((Control)(object)buttonRemoveFromQat);
		((Control)this).Controls.Add((Control)(object)buttonAddToQat);
		((Control)this).Controls.Add((Control)(object)itemPanelQat);
		((Control)this).Controls.Add((Control)(object)itemPanelCommands);
		((Control)this).Name = "QatCustomizePanel";
		((Control)this).Size = new Size(450, 336);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void ItemPanelKeyUp(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode != 38 && (int)e.KeyCode != 40 && (int)e.KeyCode != 37 && (int)e.KeyCode != 39)
		{
			return;
		}
		ItemPanel itemPanel = sender as ItemPanel;
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
		m_Categories.Clear();
		itemPanelCommands.Items.Clear();
		itemPanelQat.Items.Clear();
		LoadQatItems(rc);
		if (rc.CategorizeMode == eCategorizeMode.Categories)
		{
			LoadByCategory(rc);
		}
		else
		{
			LoadByRibbonBar(rc);
		}
		LoadCategories();
		checkQatBelowRibbon.Checked = rc.QatPositionedBelowRibbon;
		((Control)checkQatBelowRibbon).Visible = rc.EnableQatPlacement;
		m_DataChanged = false;
	}

	public void LoadItems(MetroShell tab)
	{
		m_Categories.Clear();
		itemPanelCommands.Items.Clear();
		itemPanelQat.Items.Clear();
		LoadQatItems(tab);
		if (tab.CategorizeMode == eMetroCategorizeMode.Categories)
		{
			LoadByCategory(tab);
		}
		else
		{
			LoadByMetroToolbar(tab);
		}
		LoadCategories();
		((Control)checkQatBelowRibbon).Visible = false;
		m_DataChanged = false;
	}

	private void LoadByCategory(RibbonControl rc)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)rc).Controls)
		{
			Control panel = item;
			LoadByCategory(panel);
		}
		BaseItem applicationButton = rc.GetApplicationButton();
		if (applicationButton != null)
		{
			LoadByCategory(applicationButton);
		}
	}

	private void LoadByCategory(MetroShell tab)
	{
		List<IOwner> registeredToolbars = tab.RegisteredToolbars;
		foreach (IOwner item in registeredToolbars)
		{
			if (item is MetroToolbar)
			{
				LoadByCategory((MetroToolbar)item);
			}
		}
		BaseItem applicationButton = tab.GetApplicationButton();
		if (applicationButton != null)
		{
			LoadByCategory(applicationButton);
		}
	}

	private void LoadByCategory(MetroToolbar toolbar)
	{
		foreach (BaseItem item3 in toolbar.Items)
		{
			LoadByCategory(item3);
		}
		foreach (BaseItem extraItem in toolbar.ExtraItems)
		{
			LoadByCategory(extraItem);
		}
	}

	private void LoadByCategory(Control panel)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (panel is RibbonBar)
		{
			LoadByCategory(panel as RibbonBar);
		}
		foreach (Control item in (ArrangedElementCollection)panel.Controls)
		{
			Control panel2 = item;
			LoadByCategory(panel2);
		}
	}

	private void LoadByCategory(RibbonBar bar)
	{
		foreach (BaseItem item in bar.Items)
		{
			LoadByCategory(item);
		}
	}

	private void LoadByCategory(BaseItem item)
	{
		if (CanCustomizeItem(item))
		{
			string category = GetCategory(item);
			ArrayList arrayList = m_Categories[category] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				m_Categories.Add(category, arrayList);
			}
			arrayList.Add(GetCustomizeRepresentation(item));
		}
		foreach (BaseItem subItem in item.SubItems)
		{
			LoadByCategory(subItem);
		}
		if (!(item is GalleryContainer))
		{
			return;
		}
		GalleryContainer galleryContainer = item as GalleryContainer;
		foreach (BaseItem popupGalleryItem in galleryContainer.PopupGalleryItems)
		{
			LoadByCategory(popupGalleryItem);
		}
	}

	private string GetCategory(BaseItem item)
	{
		if (item.Category != "")
		{
			return item.Category;
		}
		return "Unassigned";
	}

	private void LoadByRibbonBar(RibbonControl rc)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)rc).Controls)
		{
			Control panel = item;
			LoadByRibbonBar(panel);
		}
		BaseItem applicationButton = rc.GetApplicationButton();
		if (applicationButton != null)
		{
			LoadByRibbonBar(applicationButton, GetRibbonBarCategory(applicationButton.Text));
		}
	}

	private void LoadByMetroToolbar(MetroShell rc)
	{
		List<IOwner> registeredToolbars = rc.RegisteredToolbars;
		foreach (IOwner item in registeredToolbars)
		{
			if (item is MetroToolbar)
			{
				LoadByMetroToolbar((MetroToolbar)item);
			}
		}
		BaseItem applicationButton = rc.GetApplicationButton();
		if (applicationButton != null)
		{
			LoadByRibbonBar(applicationButton, GetMetroToolbarCategory(applicationButton.Text));
		}
	}

	private void LoadByMetroToolbar(MetroToolbar bar)
	{
		foreach (BaseItem item3 in bar.Items)
		{
			LoadByRibbonBar(item3, GetMetroToolbarCategory(((Control)bar).Text));
		}
		foreach (BaseItem extraItem in bar.ExtraItems)
		{
			LoadByRibbonBar(extraItem, GetMetroToolbarCategory(((Control)bar).Text));
		}
	}

	private void LoadByRibbonBar(Control panel)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (panel is RibbonBar)
		{
			LoadByRibbonBar(panel as RibbonBar);
		}
		foreach (Control item in (ArrangedElementCollection)panel.Controls)
		{
			Control panel2 = item;
			LoadByRibbonBar(panel2);
		}
	}

	private void LoadByRibbonBar(RibbonBar bar)
	{
		foreach (BaseItem item in bar.Items)
		{
			LoadByRibbonBar(item, GetRibbonBarCategory(((Control)bar).Text));
		}
	}

	private string GetRibbonBarCategory(string barText)
	{
		StringBuilder stringBuilder = new StringBuilder(barText.Length);
		for (int i = 0; i < barText.Length; i++)
		{
			if (barText[i] == '&')
			{
				if (i + 1 < barText.Length && barText[i + 1] == '&')
				{
					stringBuilder.Append(barText[i]);
				}
			}
			else
			{
				stringBuilder.Append(barText[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private string GetMetroToolbarCategory(string barText)
	{
		StringBuilder stringBuilder = new StringBuilder(barText.Length);
		for (int i = 0; i < barText.Length; i++)
		{
			if (barText[i] == '&')
			{
				if (i + 1 < barText.Length && barText[i + 1] == '&')
				{
					stringBuilder.Append(barText[i]);
				}
			}
			else
			{
				stringBuilder.Append(barText[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private void LoadByRibbonBar(BaseItem item, string category)
	{
		if (CanCustomizeItem(item))
		{
			ArrayList arrayList = m_Categories[category] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				m_Categories.Add(category, arrayList);
			}
			arrayList.Add(GetCustomizeRepresentation(item));
		}
		foreach (BaseItem subItem in item.SubItems)
		{
			LoadByRibbonBar(subItem, category);
		}
		if (!(item is GalleryContainer))
		{
			return;
		}
		GalleryContainer galleryContainer = item as GalleryContainer;
		foreach (BaseItem popupGalleryItem in galleryContainer.PopupGalleryItems)
		{
			LoadByRibbonBar(popupGalleryItem, category);
		}
	}

	private void LoadQatItems(RibbonControl rc)
	{
		int count = rc.QuickToolbarItems.Count;
		int num = 0;
		BaseItem applicationButton = rc.GetApplicationButton();
		if (applicationButton != null)
		{
			num = rc.QuickToolbarItems.IndexOf(applicationButton) + 1;
		}
		for (int i = num; i < count; i++)
		{
			BaseItem baseItem = rc.QuickToolbarItems[i];
			if (!IsSystemItem(baseItem))
			{
				BaseItem customizeRepresentation = GetCustomizeRepresentation(baseItem);
				customizeRepresentation.Tag = null;
				if (!baseItem.CanCustomize)
				{
					customizeRepresentation.Visible = false;
				}
				itemPanelQat.Items.Add(customizeRepresentation);
			}
		}
	}

	private void LoadQatItems(MetroShell tab)
	{
		int count = tab.QuickToolbarItems.Count;
		int num = 0;
		BaseItem applicationButton = tab.GetApplicationButton();
		if (applicationButton != null)
		{
			num = tab.QuickToolbarItems.IndexOf(applicationButton) + 1;
		}
		for (int i = num; i < count; i++)
		{
			BaseItem baseItem = tab.QuickToolbarItems[i];
			if (!IsSystemItem(baseItem))
			{
				BaseItem customizeRepresentation = GetCustomizeRepresentation(baseItem);
				customizeRepresentation.Tag = null;
				if (!baseItem.CanCustomize)
				{
					customizeRepresentation.Visible = false;
				}
				itemPanelQat.Items.Add(customizeRepresentation);
			}
		}
	}

	private bool IsSystemItem(BaseItem item)
	{
		if (item.SystemItem || item is ItemContainer || item is CustomizeItem || item is SystemCaptionItem)
		{
			return true;
		}
		return false;
	}

	private ButtonItem GetCustomizeRepresentation(BaseItem item)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		ButtonItem buttonItem = new ButtonItem(item.Name);
		buttonItem.Text = item.Text;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem.ImagePaddingVertical = 4;
		buttonItem.OptionGroup = "sys";
		buttonItem.GlobalItem = false;
		if (buttonItem.Text == "")
		{
			buttonItem.Text = "Unassigned";
		}
		buttonItem.Tag = item;
		if (item is ButtonItem)
		{
			ButtonItem buttonItem2 = item as ButtonItem;
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
				CompositeImage image = buttonItem2.GetImage(ImageState.Default, Color.Black);
				if (image != null)
				{
					if (image.IsIcon)
					{
						buttonItem.Icon = (Icon)image.Icon.Clone();
					}
					else
					{
						buttonItem.Image = image.Image;
					}
					buttonItem.ImageFixedSize = new Size(16, 16);
				}
			}
		}
		else if (item is ComboBoxItem || item is TextBoxItem || item is ControlContainerItem)
		{
			buttonItem.ImagePosition = eImagePosition.Right;
			buttonItem.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.QatCustomizeItemCombo.png");
		}
		return buttonItem;
	}

	private bool CanCustomizeItem(BaseItem item)
	{
		if (item == null)
		{
			return false;
		}
		if (!item.CanCustomize || item.SystemItem || item.Name == "tempColorPickerItem")
		{
			return false;
		}
		if (item is ItemContainer || item is GenericItemContainer)
		{
			return false;
		}
		return true;
	}

	private void LoadCategories()
	{
		comboCategories.Items.Clear();
		foreach (string key in m_Categories.Keys)
		{
			comboCategories.Items.Add((object)key);
		}
		if (comboCategories.Items.Count > 0)
		{
			((ListControl)comboCategories).SelectedIndex = 0;
		}
	}

	private void comboCategories_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboCategories).SelectedIndex < 0)
		{
			itemPanelCommands.Items.Clear();
			itemPanelCommands.RecalcLayout();
			return;
		}
		string key = comboCategories.Items[((ListControl)comboCategories).SelectedIndex].ToString();
		ArrayList arrayList = m_Categories[key] as ArrayList;
		itemPanelCommands.Items.Clear();
		foreach (BaseItem item in arrayList)
		{
			itemPanelCommands.Items.Add(item);
		}
		itemPanelCommands.RecalcLayout();
	}

	private void ItemPanelCommandsDoubleClick(object sender, MouseEventArgs e)
	{
		AddCheckedButtonToQat();
	}

	private void AddCheckedButtonToQat()
	{
		ButtonItem @checked = itemPanelCommands.GetChecked();
		if (@checked != null && !itemPanelQat.Items.Contains(@checked.Name))
		{
			ButtonItem buttonItem = @checked.Copy() as ButtonItem;
			buttonItem.Checked = false;
			itemPanelQat.Items.Add(buttonItem);
			buttonItem.Checked = true;
			itemPanelQat.RecalcLayout();
			m_DataChanged = true;
			int num = itemPanelCommands.Items.IndexOf(@checked) + 1;
			if (num != itemPanelCommands.Items.Count && itemPanelCommands.Items[num] is ButtonItem buttonItem2)
			{
				buttonItem2.Checked = true;
				itemPanelCommands.EnsureVisible(buttonItem2);
			}
		}
	}

	private void buttonAddToQat_Click(object sender, EventArgs e)
	{
		AddCheckedButtonToQat();
	}

	private void RemoveCheckedButtonFromQat()
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
		m_DataChanged = true;
	}

	private void buttonRemoveFromQat_Click(object sender, EventArgs e)
	{
		RemoveCheckedButtonFromQat();
	}

	private void ItemPanelQatDoubleClick(object sender, MouseEventArgs e)
	{
		RemoveCheckedButtonFromQat();
	}

	private void itemPanelQat_DragDrop(object sender, DragEventArgs e)
	{
		m_DataChanged = true;
	}

	private void itemPanelQat_UserCustomize(object sender, EventArgs e)
	{
		m_DataChanged = true;
	}

	private void checkQatBelowRibbon_CheckedChanged(object sender, CheckBoxXChangeEventArgs e)
	{
		m_DataChanged = true;
	}
}
