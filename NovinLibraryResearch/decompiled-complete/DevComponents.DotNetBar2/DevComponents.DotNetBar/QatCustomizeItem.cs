using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar;

public class QatCustomizeItem : CustomizeItem
{
	private const string ᮽ = "Customize Quick Access Toolbar";

	private const int _෯ = 14;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Category("Behavior")]
	[Description("Indicates whether Customize menu item is visible.")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[Localizable(true)]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("Customize Quick Access Toolbar")]
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
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			ໝ.DrawQatCustomizeItem(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
			return;
		}
		ᯈ ᯈ = ᣓ.ᤝ(this);
		if (ᯈ != null)
		{
			ᯈ.Paint(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
		}
		else
		{
			base.Paint(p);
		}
	}

	private bool ᮾ(RibbonControl ᑠ, string _0963)
	{
		if (ᑠ.QuickToolbarItems.Contains(_0963))
		{
			return true;
		}
		if (Parent is QatOverflowItem && Parent.SubItems.Contains(_0963))
		{
			return true;
		}
		return false;
	}

	private bool ᮾ(MetroShell ᆰ, string _0963)
	{
		if (ᆰ.QuickToolbarItems.Contains(_0963))
		{
			return true;
		}
		if (Parent is QatOverflowItem && Parent.SubItems.Contains(_0963))
		{
			return true;
		}
		return false;
	}

	protected override void SetupCustomizeItem()
	{
		if (ᯇ() != null)
		{
			ᯀ();
		}
		else if (ᐩ() != null)
		{
			ᮿ();
		}
	}

	private void ᮿ()
	{
		SubItems.Clear();
		string itemText = "&Customize Quick Access Toolbar...";
		MetroShell metroShell = ᐩ();
		if (metroShell != null)
		{
			itemText = metroShell.SystemText.QatCustomizeText;
			PopupFont = ((Control)metroShell).Font;
		}
		SubItemsCollection qatFrequentCommands = metroShell.QatFrequentCommands;
		string itemText2 = "<b>Customize Quick Access Toolbar</b>";
		if (metroShell != null)
		{
			itemText2 = metroShell.SystemText.QatCustomizeMenuLabel;
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
					if (ᮾ(metroShell, item.Name))
					{
						buttonItem.Checked = true;
					}
					buttonItem.CanCustomize = false;
					SubItems.Add(buttonItem);
					buttonItem.Click += ᯁ;
					buttonItem.Tag = item.Name;
				}
			}
		}
		ButtonItem buttonItem2 = new ButtonItem(RibbonControl.SysQatCustomizeItemName, itemText);
		buttonItem2.BeginGroup = beginGroup;
		buttonItem2.CanCustomize = false;
		SubItems.Add(buttonItem2);
		buttonItem2.Click += ᯄ;
	}

	private MetroShell ᐩ()
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

	private void ᯀ()
	{
		SubItems.Clear();
		string itemText = "&Customize Quick Access Toolbar...";
		RibbonControl ribbonControl = ᯇ();
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
					if (ᮾ(ribbonControl, item.Name))
					{
						buttonItem.Checked = true;
					}
					buttonItem.CanCustomize = false;
					SubItems.Add(buttonItem);
					buttonItem.Click += ᯁ;
					buttonItem.Tag = item.Name;
				}
			}
		}
		ButtonItem buttonItem2 = new ButtonItem(RibbonControl.SysQatCustomizeItemName, itemText);
		buttonItem2.BeginGroup = beginGroup;
		buttonItem2.CanCustomize = false;
		SubItems.Add(buttonItem2);
		buttonItem2.Click += ᯄ;
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
			buttonItem3.Click += ᯃ;
			SubItems.Add(buttonItem3);
		}
		if (ribbonControl != null)
		{
			ButtonItem buttonItem4 = null;
			buttonItem4 = ((!ribbonControl.Expanded) ? new ButtonItem(RibbonControl.SysMaximizeRibbon, ribbonControl.SystemText.MaximizeRibbonText) : new ButtonItem(RibbonControl.SysMinimizeRibbon, ribbonControl.SystemText.MinimizeRibbonText));
			buttonItem4.CanCustomize = false;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += ᯂ;
			SubItems.Add(buttonItem4);
		}
	}

	private void ᯁ(object _٠, EventArgs ؿ)
	{
		RibbonControl ribbonControl = ᯇ();
		if (ribbonControl == null)
		{
			return;
		}
		BaseItem.CollapseAll(this);
		ribbonControl.RibbonStrip.ClosePopups();
		if (_٠ is ButtonItem buttonItem && buttonItem.Tag is string && buttonItem.Tag.ToString().Length > 0)
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

	private void ᯂ(object _٠, EventArgs ؿ)
	{
		BaseItem.CollapseAll(this);
		if (!(_٠ is ButtonItem buttonItem))
		{
			return;
		}
		RibbonControl ribbonControl = ᯇ();
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

	private void ᯃ(object _٠, EventArgs ؿ)
	{
		BaseItem.CollapseAll(this);
		RibbonControl ribbonControl = ᯇ();
		if (ribbonControl != null)
		{
			ribbonControl.RibbonStrip.ClosePopups();
			ribbonControl.ᯃ();
		}
	}

	private void ᯄ(object _٠, EventArgs ؿ)
	{
		if (ᯇ() != null)
		{
			ᯆ();
		}
		else if (ᐩ() != null)
		{
			ᯅ();
		}
	}

	private void ᯅ()
	{
		MetroShell metroShell = ᐩ();
		BaseItem.CollapseAll(this);
		if (metroShell != null)
		{
			metroShell.MetroTabStrip.ClosePopups();
			metroShell.ShowQatCustomizeDialog();
		}
	}

	private void ᯆ()
	{
		RibbonControl ribbonControl = ᯇ();
		BaseItem.CollapseAll(this);
		if (ribbonControl != null)
		{
			ribbonControl.RibbonStrip.ClosePopups();
			ribbonControl.ShowQatCustomizeDialog();
		}
	}

	private RibbonControl ᯇ()
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
				if (baseItem.ContainerControl is Ꮮ)
				{
					Ꮮ ꮮ = baseItem.ContainerControl as Ꮮ;
					if (((Control)ꮮ).Parent is RibbonControl)
					{
						ribbonStrip = ((RibbonControl)(object)((Control)ꮮ).Parent).RibbonStrip;
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
		if (ContainerControl is Ꮮ ꮮ2 && ((Control)ꮮ2).Parent is RibbonControl)
		{
			return ((Control)ꮮ2).Parent as RibbonControl;
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
				m_Rect.Height = Dpi.Height(14);
				m_Rect.Width = Dpi.Width22;
			}
			else
			{
				m_Rect.Width = Dpi.Width(14);
				m_Rect.Height = Dpi.Height22;
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
