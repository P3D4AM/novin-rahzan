using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class QatCustomizeItem : CustomizeItem
{
	private const string DefaultTooltipText = "Customize Quick Access Toolbar";

	private const int FIXED_SIZE = 14;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates whether Customize menu item is visible.")]
	[DefaultValue(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool CustomizeItemVisible
	{
		get
		{
			return base.CustomizeItemVisible;
		}
		set
		{
			base.CustomizeItemVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("Customize Quick Access Toolbar")]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Localizable(true)]
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

	public QatCustomizeItem()
	{
		Tooltip = "Customize Quick Access Toolbar";
	}

	public override BaseItem Copy()
	{
		QatCustomizeItem qatCustomizeItem = new QatCustomizeItem();
		CopyToItem(qatCustomizeItem);
		return qatCustomizeItem;
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			renderer.DrawQatCustomizeItem(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
			return;
		}
		QatCustomizeItemPainter qatCustomizeItemPainter = PainterFactory.CreateQatCustomizeItemPainter(this);
		if (qatCustomizeItemPainter != null)
		{
			qatCustomizeItemPainter.Paint(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
		}
		else
		{
			base.Paint(p);
		}
	}

	private bool QatContainsItem(RibbonControl rc, string itemName)
	{
		if (rc.QuickToolbarItems.Contains(itemName))
		{
			return true;
		}
		if (Parent is QatOverflowItem && Parent.SubItems.Contains(itemName))
		{
			return true;
		}
		return false;
	}

	private bool QatContainsItem(MetroShell tab, string itemName)
	{
		if (tab.QuickToolbarItems.Contains(itemName))
		{
			return true;
		}
		if (Parent is QatOverflowItem && Parent.SubItems.Contains(itemName))
		{
			return true;
		}
		return false;
	}

	protected override void SetupCustomizeItem()
	{
		if (GetRibbonControl() != null)
		{
			SetupRibbonCustomizeItem();
		}
		else if (GetMetroTab() != null)
		{
			SetupMetroTabCustomizeItem();
		}
	}

	private void SetupMetroTabCustomizeItem()
	{
		SubItems.Clear();
		string itemText = "&Customize Quick Access Toolbar...";
		MetroShell metroTab = GetMetroTab();
		if (metroTab != null)
		{
			itemText = metroTab.SystemText.QatCustomizeText;
			PopupFont = ((Control)metroTab).Font;
		}
		SubItemsCollection qatFrequentCommands = metroTab.QatFrequentCommands;
		string itemText2 = "<b>Customize Quick Access Toolbar</b>";
		if (metroTab != null)
		{
			itemText2 = metroTab.SystemText.QatCustomizeMenuLabel;
		}
		LabelItem labelItem = new LabelItem(RibbonControl.SysQatCustomizeLabelName, itemText2);
		labelItem.PaddingBottom = 2;
		labelItem.PaddingTop = 2;
		labelItem.PaddingLeft = 12;
		labelItem.BorderSide = eBorderSide.Bottom;
		labelItem.BorderType = eBorderType.SingleLine;
		labelItem.CanCustomize = false;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			labelItem.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelBackground;
			labelItem.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelText;
		}
		SubItems.Add(labelItem);
		bool beginGroup = false;
		if (qatFrequentCommands.Count > 0)
		{
			beginGroup = true;
			foreach (BaseItem item in qatFrequentCommands)
			{
				if (item.Text.Length > 0)
				{
					ButtonItem buttonItem = new ButtonItem(RibbonControl.SysFrequentlyQatNamePart + item.Name, item.Text);
					if (QatContainsItem(metroTab, item.Name))
					{
						buttonItem.Checked = true;
					}
					buttonItem.CanCustomize = false;
					SubItems.Add(buttonItem);
					buttonItem.Click += AddFrequentCommandToQat;
					buttonItem.Tag = item.Name;
				}
			}
		}
		ButtonItem buttonItem2 = new ButtonItem(RibbonControl.SysQatCustomizeItemName, itemText);
		buttonItem2.BeginGroup = beginGroup;
		buttonItem2.CanCustomize = false;
		SubItems.Add(buttonItem2);
		buttonItem2.Click += QatCustomizeItemClick;
	}

	private MetroShell GetMetroTab()
	{
		MetroTabStrip metroTabStrip = ContainerControl as MetroTabStrip;
		if (metroTabStrip == null)
		{
			BaseItem baseItem = this;
			while (baseItem != null && metroTabStrip == null)
			{
				baseItem = baseItem.Parent;
				if (baseItem != null)
				{
					metroTabStrip = baseItem.ContainerControl as MetroTabStrip;
				}
			}
		}
		MetroShell result = null;
		if (metroTabStrip != null)
		{
			result = ((Control)metroTabStrip).Parent as MetroShell;
		}
		return result;
	}

	private void SetupRibbonCustomizeItem()
	{
		SubItems.Clear();
		string itemText = "&Customize Quick Access Toolbar...";
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			itemText = ribbonControl.SystemText.QatCustomizeText;
		}
		SubItemsCollection qatFrequentCommands = ribbonControl.QatFrequentCommands;
		string itemText2 = "<b>Customize Quick Access Toolbar</b>";
		if (ribbonControl != null)
		{
			itemText2 = ribbonControl.SystemText.QatCustomizeMenuLabel;
		}
		LabelItem labelItem = new LabelItem(RibbonControl.SysQatCustomizeLabelName, itemText2);
		labelItem.PaddingBottom = 2;
		labelItem.PaddingTop = 2;
		labelItem.PaddingLeft = 12;
		labelItem.BorderSide = eBorderSide.Bottom;
		labelItem.BorderType = eBorderType.SingleLine;
		labelItem.CanCustomize = false;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			labelItem.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelBackground;
			labelItem.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelText;
		}
		SubItems.Add(labelItem);
		bool beginGroup = false;
		if (qatFrequentCommands.Count > 0)
		{
			beginGroup = true;
			foreach (BaseItem item in qatFrequentCommands)
			{
				if (item.Text.Length > 0)
				{
					ButtonItem buttonItem = new ButtonItem(RibbonControl.SysFrequentlyQatNamePart + item.Name, item.Text);
					if (QatContainsItem(ribbonControl, item.Name))
					{
						buttonItem.Checked = true;
					}
					buttonItem.CanCustomize = false;
					SubItems.Add(buttonItem);
					buttonItem.Click += AddFrequentCommandToQat;
					buttonItem.Tag = item.Name;
				}
			}
		}
		ButtonItem buttonItem2 = new ButtonItem(RibbonControl.SysQatCustomizeItemName, itemText);
		buttonItem2.BeginGroup = beginGroup;
		buttonItem2.CanCustomize = false;
		SubItems.Add(buttonItem2);
		buttonItem2.Click += QatCustomizeItemClick;
		if (ribbonControl != null && ribbonControl.EnableQatPlacement)
		{
			ButtonItem buttonItem3 = new ButtonItem(RibbonControl.SysQatPlaceItemName);
			buttonItem3.CanCustomize = false;
			if (ribbonControl.QatPositionedBelowRibbon)
			{
				buttonItem3.Text = ribbonControl.SystemText.QatPlaceAboveRibbonText;
			}
			else
			{
				buttonItem3.Text = ribbonControl.SystemText.QatPlaceBelowRibbonText;
			}
			buttonItem3.Click += QuickAccessToolbarChangePlacement;
			SubItems.Add(buttonItem3);
		}
		if (ribbonControl != null)
		{
			ButtonItem buttonItem4 = null;
			buttonItem4 = ((!ribbonControl.Expanded) ? new ButtonItem(RibbonControl.SysMaximizeRibbon, ribbonControl.SystemText.MaximizeRibbonText) : new ButtonItem(RibbonControl.SysMinimizeRibbon, ribbonControl.SystemText.MinimizeRibbonText));
			buttonItem4.CanCustomize = false;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += ToggleRibbonExpand;
			SubItems.Add(buttonItem4);
		}
	}

	private void AddFrequentCommandToQat(object sender, EventArgs e)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return;
		}
		BaseItem.CollapseAll(this);
		ribbonControl.RibbonStrip.ClosePopups();
		if (sender is ButtonItem buttonItem && buttonItem.Tag is string && buttonItem.Tag.ToString().Length > 0)
		{
			if (buttonItem.Checked)
			{
				ribbonControl.RemoveItemFromQuickAccessToolbar(ribbonControl.QuickToolbarItems[buttonItem.Tag.ToString()]);
			}
			else
			{
				ribbonControl.AddItemToQuickAccessToolbar(ribbonControl.QatFrequentCommands[buttonItem.Tag.ToString()]);
			}
		}
	}

	private void ToggleRibbonExpand(object sender, EventArgs e)
	{
		BaseItem.CollapseAll(this);
		if (!(sender is ButtonItem buttonItem))
		{
			return;
		}
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			ribbonControl.RibbonStrip.ClosePopups();
			if (buttonItem.Name == RibbonControl.SysMinimizeRibbon)
			{
				ribbonControl.Expanded = false;
			}
			else if (buttonItem.Name == RibbonControl.SysMaximizeRibbon)
			{
				ribbonControl.Expanded = true;
			}
		}
	}

	private void QuickAccessToolbarChangePlacement(object sender, EventArgs e)
	{
		BaseItem.CollapseAll(this);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			ribbonControl.RibbonStrip.ClosePopups();
			ribbonControl.QuickAccessToolbarChangePlacement();
		}
	}

	private void QatCustomizeItemClick(object sender, EventArgs e)
	{
		if (GetRibbonControl() != null)
		{
			QatCustomizeItemClickRibbon();
		}
		else if (GetMetroTab() != null)
		{
			QatCustomizeItemClickMetroTab();
		}
	}

	private void QatCustomizeItemClickMetroTab()
	{
		MetroShell metroTab = GetMetroTab();
		BaseItem.CollapseAll(this);
		if (metroTab != null)
		{
			metroTab.MetroTabStrip.ClosePopups();
			metroTab.ShowQatCustomizeDialog();
		}
	}

	private void QatCustomizeItemClickRibbon()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		BaseItem.CollapseAll(this);
		if (ribbonControl != null)
		{
			ribbonControl.RibbonStrip.ClosePopups();
			ribbonControl.ShowQatCustomizeDialog();
		}
	}

	private RibbonControl GetRibbonControl()
	{
		RibbonStrip ribbonStrip = ContainerControl as RibbonStrip;
		if (ribbonStrip == null)
		{
			BaseItem baseItem = this;
			while (baseItem != null && ribbonStrip == null)
			{
				baseItem = baseItem.Parent;
				if (baseItem == null)
				{
					continue;
				}
				if (baseItem.ContainerControl is QatToolbar)
				{
					QatToolbar qatToolbar = baseItem.ContainerControl as QatToolbar;
					if (((Control)qatToolbar).Parent is RibbonControl)
					{
						ribbonStrip = ((RibbonControl)(object)((Control)qatToolbar).Parent).RibbonStrip;
						break;
					}
				}
				else
				{
					ribbonStrip = baseItem.ContainerControl as RibbonStrip;
				}
			}
		}
		RibbonControl result = null;
		if (ribbonStrip != null)
		{
			result = ((Control)ribbonStrip).Parent as RibbonControl;
		}
		if (ContainerControl is QatToolbar qatToolbar2 && ((Control)qatToolbar2).Parent is RibbonControl)
		{
			return ((Control)qatToolbar2).Parent as RibbonControl;
		}
		return result;
	}

	protected override void ClearCustomizeItem()
	{
	}

	protected override void LoadResources()
	{
	}

	protected internal override void OnExpandChange()
	{
		PopupSide = ePopupSide.Bottom;
		base.OnExpandChange();
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Height = 14;
				m_Rect.Width = 22;
			}
			else
			{
				m_Rect.Width = 14;
				m_Rect.Height = 22;
			}
			SetCustomTooltip(GetTooltipText());
		}
	}

	protected override string GetTooltipText()
	{
		string text = "";
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			text = localizationManager.GetLocalizedString(LocalizationKeys.QatCustomizeTooltip);
		}
		if (text == "")
		{
			text = Tooltip;
		}
		return text;
	}

	protected override void MouseHoverCustomize()
	{
	}
}
