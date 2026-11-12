using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design;

[ToolboxItem(false)]
public class StiReportPropertyGrid : StiPropertyGrid
{
	private ButtonItem contextMenu;

	private ButtonItem menuItemLocalization;

	private ButtonItem menuItemShowDescription;

	private ButtonItem menuItemBasicConfiguration;

	private ButtonItem menuItemStandardConfiguration;

	private ButtonItem menuItemProfessionalConfiguration;

	private ButtonItem menuItemCopyPropertName;

	private bool firstPaint = true;

	internal ContextMenuBar ContextMenuBar { get; set; }

	public bool ShowEventsTag { get; set; } = true;


	public override bool HelpVisible
	{
		get
		{
			return StiOptions.Designer.PropertyGrid.ShowDescription;
		}
		set
		{
			base.HelpVisible = StiOptions.Designer.PropertyGrid.ShowDescription;
		}
	}

	public void RemovePropertyPages()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		ToolStrip val = null;
		foreach (Control item in (ArrangedElementCollection)((PropertyGrid)this).Controls)
		{
			Control val2 = item;
			val = (ToolStrip)(object)((val2 is ToolStrip) ? val2 : null);
			if (val != null)
			{
				break;
			}
		}
		if (val != null && ((ArrangedElementCollection)val.Items).Count > 1 && val.Items[((ArrangedElementCollection)val.Items).Count - 1] is ToolStripButton && val.Items[((ArrangedElementCollection)val.Items).Count - 2] is ToolStripSeparator)
		{
			val.Items.RemoveAt(((ArrangedElementCollection)val.Items).Count - 1);
			val.Items.RemoveAt(((ArrangedElementCollection)val.Items).Count - 1);
		}
	}

	public override void SetStandardPropertyTabs()
	{
		base.SetStandardPropertyTabs();
		if (ShowEventsTag && StiOptions.Designer.IsShowEventsTab)
		{
			((PropertyGrid)this).PropertyTabs.AddTabType(typeof(StiEventsTab), PropertyTabScope.Document);
		}
		RemovePropertyPages();
	}

	private void RefreshPropertyGrid()
	{
		object[] selectedObjects = ((PropertyGrid)this).SelectedObjects;
		((PropertyGrid)this).SelectedObject = null;
		((PropertyGrid)this).SelectedObjects = selectedObjects;
	}

	public override void Localize()
	{
		base.Localize();
		menuItemLocalization.Text = StiLocalization.Get("FormDesigner", "LocalizePropertyGrid");
		menuItemShowDescription.Text = StiLocalization.Get("FormDesigner", "ShowDescription");
		menuItemCopyPropertName.Text = "Copy Property Name";
		if (menuItemBasicConfiguration != null)
		{
			menuItemBasicConfiguration.Text = StiLocalization.Get("Report", "Basic");
		}
		if (menuItemStandardConfiguration != null)
		{
			menuItemStandardConfiguration.Text = StiLocalization.Get("Report", "Standard");
		}
		if (menuItemProfessionalConfiguration != null)
		{
			menuItemProfessionalConfiguration.Text = StiLocalization.Get("Report", "Professional");
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		((PropertyGrid)this).OnPaint(e);
		if (firstPaint)
		{
			StiSettings.Load();
			((PropertyGrid)this).PropertySort = (PropertySort)StiSettings.Get("StiDesigner", "PropertyGridSort", (object)(PropertySort)3);
			firstPaint = false;
		}
	}

	private void ContextMenu_Popup(object sender, PopupOpenEventArgs e)
	{
		Localize();
		menuItemShowDescription.Checked = StiOptions.Designer.PropertyGrid.ShowDescription;
		menuItemLocalization.Checked = StiOptions.Designer.PropertyGrid.Localizable;
		if (menuItemBasicConfiguration != null)
		{
			menuItemBasicConfiguration.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Basic;
		}
		if (menuItemStandardConfiguration != null)
		{
			menuItemStandardConfiguration.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Standard;
		}
		if (menuItemProfessionalConfiguration != null)
		{
			menuItemProfessionalConfiguration.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Professional;
		}
	}

	internal void ItemShowDescription_Click(object sender, EventArgs e)
	{
		StiOptions.Designer.PropertyGrid.ShowDescription = !StiOptions.Designer.PropertyGrid.ShowDescription;
		((PropertyGrid)this).HelpVisible = StiOptions.Designer.PropertyGrid.ShowDescription;
	}

	internal void ItemLocalization_Click(object sender, EventArgs e)
	{
		StiOptions.Designer.PropertyGrid.Localizable = !StiOptions.Designer.PropertyGrid.Localizable;
		RefreshPropertyGrid();
	}

	private void ItemConfiguration_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount?.User != null)
		{
			if (sender == menuItemBasicConfiguration)
			{
				StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.Beginner;
			}
			else if (sender == menuItemStandardConfiguration)
			{
				StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.BICreator;
			}
			else if (sender == menuItemProfessionalConfiguration)
			{
				StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.Developer;
			}
			StiAccountSettings.CurrentAccount.User.SaveToFile();
		}
		else if (sender == menuItemBasicConfiguration)
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Basic;
		}
		else if (sender == menuItemStandardConfiguration)
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Standard;
		}
		else if (sender == menuItemProfessionalConfiguration)
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Professional;
		}
		RefreshPropertyGrid();
	}

	private void ItemCopyPropertyName_Click(object sender, EventArgs e)
	{
		string text = string.Empty;
		try
		{
			text = ((object)((PropertyGrid)this).SelectedGridItem).GetType().GetProperty("PropertyName").GetValue(((PropertyGrid)this).SelectedGridItem, null) as string;
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(text))
		{
			Clipboard.SetText(text);
		}
	}

	protected override void Dispose(bool disposing)
	{
		((PropertyGrid)this).Dispose(disposing);
		try
		{
			((Component)(object)ContextMenuBar)?.Dispose();
			contextMenu?.Dispose();
			menuItemLocalization?.Dispose();
			menuItemShowDescription?.Dispose();
			menuItemBasicConfiguration?.Dispose();
			menuItemStandardConfiguration?.Dispose();
			menuItemProfessionalConfiguration?.Dispose();
		}
		catch
		{
		}
	}

	public StiReportPropertyGrid()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		ContextMenuBar = new ContextMenuBar
		{
			Style = eDotNetBarStyle.StyleManagerControlled
		};
		contextMenu = new ButtonItem
		{
			AutoExpandOnClick = true
		};
		ContextMenuBar.Items.Add(contextMenu);
		StiSettings.Load();
		((PropertyGrid)this).PropertySort = (PropertySort)StiSettings.Get("StiDesigner", "PropertyGridSort", (object)(PropertySort)3);
		StiPropertyGridOptions.PropertyLevel = StiOptions.Designer.PropertyGrid.PropertyLevel;
		base.HelpVisible = StiOptions.Designer.PropertyGrid.ShowDescription;
		menuItemLocalization = new ButtonItem(StiLocalization.Get("FormDesigner", "LocalizePropertyGrid"));
		menuItemLocalization.Click += ItemLocalization_Click;
		contextMenu.SubItems.Add(menuItemLocalization);
		menuItemShowDescription = new ButtonItem(StiLocalization.Get("FormDesigner", "ShowDescription"));
		menuItemShowDescription.Click += ItemShowDescription_Click;
		contextMenu.SubItems.Add(menuItemShowDescription);
		if (StiOptions.Designer.PropertyGrid.AllowShowConfiguration)
		{
			menuItemBasicConfiguration = new ButtonItem(StiLocalization.Get("Report", "Basic"));
			menuItemBasicConfiguration.Click += ItemConfiguration_Click;
			menuItemBasicConfiguration.BeginGroup = true;
			contextMenu.SubItems.Add(menuItemBasicConfiguration);
			menuItemStandardConfiguration = new ButtonItem(StiLocalization.Get("Report", "Standard"));
			menuItemStandardConfiguration.Click += ItemConfiguration_Click;
			contextMenu.SubItems.Add(menuItemStandardConfiguration);
			menuItemProfessionalConfiguration = new ButtonItem(StiLocalization.Get("Report", "Professional"));
			menuItemProfessionalConfiguration.Click += ItemConfiguration_Click;
			contextMenu.SubItems.Add(menuItemProfessionalConfiguration);
		}
		menuItemCopyPropertName = new ButtonItem("Copy Property Name");
		menuItemCopyPropertName.BeginGroup = true;
		menuItemCopyPropertName.Click += ItemCopyPropertyName_Click;
		contextMenu.SubItems.Add(menuItemCopyPropertName);
		contextMenu.PopupOpen += ContextMenu_Popup;
		ContextMenuBar.SetContextMenuEx((Control)(object)this, contextMenu);
	}
}
