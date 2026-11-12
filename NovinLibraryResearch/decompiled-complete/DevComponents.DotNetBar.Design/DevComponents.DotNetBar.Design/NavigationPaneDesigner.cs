using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class NavigationPaneDesigner : NavigationBarDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[1]
			{
				new DesignerVerb("Create New Pane", CreateNewPane)
			};
			return new DesignerVerbCollection(value);
		}
	}

	[Description("Indicates whether navigation pane can be collapsed.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Title")]
	public bool Expanded
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Expanded"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Expanded"] = value;
		}
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode && ((ControlDesigner)this).Control is NavigationPane navigationPane)
		{
			navigationPane.SetDesignMode();
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		CreateNewPane();
		NavigationPane navigationPane = ((ControlDesigner)this).Control as NavigationPane;
		navigationPane.Style = eDotNetBarStyle.StyleManagerControlled;
	}

	private void CreateNewPane(object sender, EventArgs e)
	{
		CreateNewPane();
	}

	private void CreateNewPane()
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((ControlDesigner)this).Control is NavigationPane navigationPane) || designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			ButtonItem buttonItem = null;
			try
			{
				m_CreatingItem = true;
				buttonItem = designerHost.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				buttonItem.Text = buttonItem.Name;
				buttonItem.OptionGroup = "navBar";
				buttonItem.Image = (Image)(object)Helpers.LoadBitmap("SystemImages.DefaultNavBarImage.png");
			}
			finally
			{
				m_CreatingItem = false;
			}
			NavigationPanePanel navigationPanePanel = designerHost.CreateComponent(typeof(NavigationPanePanel)) as NavigationPanePanel;
			navigationPanePanel.ParentItem = buttonItem;
			navigationPane.Items.Add(buttonItem);
			((Control)navigationPane).Controls.Add((Control)(object)navigationPanePanel);
			((Control)navigationPanePanel).Dock = (DockStyle)5;
			((Control)navigationPanePanel).SendToBack();
			navigationPanePanel.ApplyPanelStyle();
			navigationPanePanel.ColorSchemeStyle = navigationPane.Style;
			if (Helpers.IsOffice2007Style(navigationPane.Style))
			{
				navigationPanePanel.ColorScheme = navigationPane.NavigationBar.GetColorScheme();
			}
			navigationPanePanel.Style.BackColor2.ColorSchemePart = eColorSchemePart.None;
			navigationPanePanel.Style.Border = eBorderType.None;
			navigationPanePanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			if (navigationPane.Items.Count == 1)
			{
				buttonItem.Checked = true;
			}
			navigationPane.RecalcLayout();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(navigationPanePanel, null);
				componentChangeService.OnComponentChanged(navigationPanePanel, null, null, null);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	public override bool CanParent(Control c)
	{
		if (c is NavigationPanePanel)
		{
			return true;
		}
		return false;
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is NavigationPane navigationPane)
		{
			return navigationPane.NavigationBar.GetBaseItemContainer();
		}
		return null;
	}

	protected override Control GetItemContainerControl()
	{
		if (((ControlDesigner)this).Control is NavigationPane navigationPane)
		{
			return (Control)(object)navigationPane.NavigationBar;
		}
		return null;
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		base.OtherComponentRemoving(sender, e);
		if (!(((ComponentDesigner)this).Component is NavigationPane navigationPane) || ((Control)navigationPane).IsDisposed)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (!m_InternalRemoving && e.Component is BaseItem && designerHost.TransactionDescription.StartsWith("Delete "))
		{
			if (e.Component is ButtonItem item)
			{
				NavigationPanePanel panel = navigationPane.GetPanel(item);
				if (panel != null && designerHost != null)
				{
					DesignerTransaction designerTransaction = designerHost.CreateTransaction("Removing associated NavigationPanePanel");
					panel.ParentItem = null;
					componentChangeService?.OnComponentChanging(navigationPane, TypeDescriptor.GetProperties(navigationPane)["Controls"]);
					((Control)navigationPane).Controls.Remove((Control)(object)panel);
					componentChangeService?.OnComponentChanged(navigationPane, TypeDescriptor.GetProperties(navigationPane)["Controls"], null, null);
					designerHost.DestroyComponent((IComponent)panel);
					designerTransaction.Commit();
					navigationPane.RecalcLayout();
				}
			}
		}
		else if (!m_InternalRemoving && e.Component is NavigationPanePanel && ((Control)navigationPane).Controls.Contains((Control)(object)(e.Component as NavigationPanePanel)))
		{
			NavigationPanePanel navigationPanePanel = e.Component as NavigationPanePanel;
			if (navigationPanePanel.ParentItem != null)
			{
				BaseItem parentItem = navigationPanePanel.ParentItem;
				navigationPanePanel.ParentItem = null;
				componentChangeService?.OnComponentChanging(navigationPane, TypeDescriptor.GetProperties(navigationPane)["Items"]);
				navigationPane.Items.Remove(parentItem);
				componentChangeService?.OnComponentChanged(navigationPane, TypeDescriptor.GetProperties(navigationPane)["Items"], null, null);
				designerHost.DestroyComponent(parentItem);
			}
			navigationPane.RecalcLayout();
		}
		if (navigationPane.CheckedButton == null && navigationPane.Items.Count > 0 && navigationPane.Items[0] is ButtonItem)
		{
			ButtonItem buttonItem = (ButtonItem)navigationPane.Items[0];
			buttonItem.Checked = true;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["Expanded"] = TypeDescriptor.CreateProperty(typeof(NavigationPaneDesigner), (PropertyDescriptor)properties["Expanded"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Title"), new DescriptionAttribute("Indicates whether navigation pane can be collapsed."));
	}
}
