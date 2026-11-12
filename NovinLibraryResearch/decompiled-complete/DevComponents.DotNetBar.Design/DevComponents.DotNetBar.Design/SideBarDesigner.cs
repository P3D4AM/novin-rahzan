using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class SideBarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[3]
			{
				new DesignerVerb("Create Panel", OnAddGroup),
				new DesignerVerb("Create Button", OnAddButton),
				new DesignerVerb("Choose Color Scheme", OnPickColorScheme)
			};
			return new DesignerVerbCollection(value);
		}
	}

	[Description("Indicates visual appearance for the control.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(eSideBarAppearance.Traditional)]
	public eSideBarAppearance Appearance
	{
		get
		{
			return ((SideBar)(object)((ControlDesigner)this).Control).Appearance;
		}
		set
		{
			SideBar sideBar = (SideBar)(object)((ControlDesigner)this).Control;
			if (sideBar.Appearance == value)
			{
				return;
			}
			sideBar.Appearance = value;
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost != null && !designerHost.Loading)
			{
				switch (value)
				{
				case eSideBarAppearance.Flat:
					SetColorScheme(sideBar.PredefinedColorScheme);
					break;
				case eSideBarAppearance.Traditional:
					sideBar.ResetTraditional();
					RaiseStyleChanged();
					break;
				}
				if (designerHost != null && !designerHost.Loading && value == eSideBarAppearance.Flat && Helpers.IsOffice2007Style(sideBar.Style))
				{
					sideBar.Style = eDotNetBarStyle.Office2003;
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the side bar.")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[DevCoBrowsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return ((SideBar)(object)((ControlDesigner)this).Control).Style;
		}
		set
		{
			SideBar sideBar = (SideBar)(object)((ControlDesigner)this).Control;
			if (sideBar.Style == value)
			{
				return;
			}
			sideBar.Style = value;
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost == null || designerHost.Loading)
			{
				return;
			}
			bool fontBold = false;
			if (Helpers.IsOffice2007Style(value))
			{
				sideBar.BorderStyle = eBorderType.None;
				fontBold = true;
			}
			else
			{
				sideBar.BorderStyle = eBorderType.Sunken;
			}
			foreach (SideBarPanelItem panel in sideBar.Panels)
			{
				panel.FontBold = fontBold;
			}
		}
	}

	public SideBarDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode && ((ControlDesigner)this).Control is SideBar sideBar)
		{
			sideBar.SetDesignMode();
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is SideBar)
		{
			Style = eDotNetBarStyle.StyleManagerControlled;
		}
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is SideBar sideBar)
		{
			return sideBar.ItemsContainer;
		}
		return null;
	}

	protected override void RecalcLayout()
	{
		if (GetItemContainerControl() is SideBar sideBar)
		{
			sideBar.RecalcLayout();
		}
	}

	protected override void OnSubItemsChanging()
	{
		base.OnSubItemsChanging();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			SideBar component = GetItemContainerControl() as SideBar;
			componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Panels", ignoreCase: true));
		}
	}

	protected override void OnSubItemsChanged()
	{
		base.OnSubItemsChanged();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			SideBar component = GetItemContainerControl() as SideBar;
			componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Panels", ignoreCase: true), null, null);
		}
	}

	private void OnAddGroup(object sender, EventArgs e)
	{
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		DesignerTransaction designerTransaction = null;
		if (designerHost != null)
		{
			designerTransaction = designerHost.CreateTransaction("New SideBarPanelItem");
		}
		try
		{
			CreateNewPanel();
		}
		finally
		{
			if (designerTransaction != null && !designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
		RecalcLayout();
	}

	private SideBarPanelItem CreateNewPanel()
	{
		SideBarPanelItem sideBarPanelItem = null;
		SideBar sideBar = ((ComponentDesigner)this).Component as SideBar;
		IDesignerHost iDesignerHost = GetIDesignerHost();
		if (sideBar != null && iDesignerHost != null)
		{
			try
			{
				m_CreatingItem = true;
				OnSubItemsChanging();
				sideBarPanelItem = iDesignerHost.CreateComponent(typeof(SideBarPanelItem)) as SideBarPanelItem;
				if (sideBarPanelItem == null)
				{
					return null;
				}
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(sideBarPanelItem, null);
				sideBarPanelItem.Text = sideBarPanelItem.Name;
				sideBar.Panels.Add(sideBarPanelItem);
				sideBarPanelItem.Appearance = sideBar.Appearance;
				if (sideBarPanelItem.Appearance == eSideBarAppearance.Flat)
				{
					SideBar.ApplyColorScheme(sideBarPanelItem, sideBar.PredefinedColorScheme);
				}
				if (Helpers.IsOffice2007Style(sideBar.Style))
				{
					sideBarPanelItem.FontBold = true;
				}
				componentChangeService?.OnComponentChanged(sideBarPanelItem, null, null, null);
				OnSubItemsChanged();
			}
			finally
			{
				m_CreatingItem = false;
			}
		}
		return sideBarPanelItem;
	}

	private void OnAddButton(object sender, EventArgs e)
	{
		SideBar sideBar = ((ComponentDesigner)this).Component as SideBar;
		IDesignerHost iDesignerHost = GetIDesignerHost();
		if (sideBar == null || iDesignerHost == null)
		{
			return;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService == null)
		{
			return;
		}
		SideBarPanelItem sideBarPanelItem = null;
		sideBarPanelItem = ((selectionService.PrimarySelection is SideBarPanelItem) ? (selectionService.PrimarySelection as SideBarPanelItem) : ((sideBar.ExpandedPanel != null) ? sideBar.ExpandedPanel : CreateNewPanel()));
		if (sideBarPanelItem == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = iDesignerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBarPanelItem).Find("SubItems", ignoreCase: true));
			try
			{
				m_CreatingItem = true;
				if (!(iDesignerHost.CreateComponent(typeof(ButtonItem)) is ButtonItem buttonItem))
				{
					return;
				}
				buttonItem.Text = "New Button";
				if (sideBar.Appearance == eSideBarAppearance.Flat)
				{
					buttonItem.ImagePosition = eImagePosition.Left;
				}
				else
				{
					buttonItem.ImagePosition = eImagePosition.Top;
				}
				buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
				sideBarPanelItem.SubItems.Add(buttonItem);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBarPanelItem).Find("SubItems", ignoreCase: true), null, null);
			}
			finally
			{
				m_CreatingItem = false;
			}
			RecalcLayout();
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

	private void OnPickColorScheme(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		SideBarColorSchemePicker sideBarColorSchemePicker = new SideBarColorSchemePicker();
		try
		{
			((Form)sideBarColorSchemePicker).ShowDialog();
			if ((int)((Form)sideBarColorSchemePicker).DialogResult == 1)
			{
				SetColorScheme(sideBarColorSchemePicker.SelectedColorScheme);
			}
		}
		finally
		{
			((IDisposable)sideBarColorSchemePicker)?.Dispose();
		}
	}

	private void SetColorScheme(eSideBarColorScheme scheme)
	{
		if (((ControlDesigner)this).Control is SideBar sideBar)
		{
			if (sideBar.Appearance != eSideBarAppearance.Flat)
			{
				TypeDescriptor.GetProperties(sideBar)["Appearance"].SetValue(sideBar, eSideBarAppearance.Flat);
			}
			TypeDescriptor.GetProperties(sideBar)["PredefinedColorScheme"].SetValue(sideBar, scheme);
			sideBar.ApplyPredefinedColorScheme(scheme);
			RaiseStyleChanged();
		}
	}

	private void RaiseStyleChanged()
	{
		IDesignerHost iDesignerHost = GetIDesignerHost();
		DesignerTransaction designerTransaction = null;
		if (iDesignerHost != null)
		{
			designerTransaction = iDesignerHost.CreateTransaction();
		}
		SideBar sideBar = ((ControlDesigner)this).Control as SideBar;
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, null);
			componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
			componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBar).Find("ColorScheme", ignoreCase: true));
			componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBar).Find("ColorScheme", ignoreCase: true), null, null);
			componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBar).Find("Panels", ignoreCase: true));
			componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(sideBar).Find("Panels", ignoreCase: true), null, null);
			foreach (SideBarPanelItem panel in sideBar.Panels)
			{
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderStyle"], null, null);
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderHotStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderHotStyle"], null, null);
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderMouseDownStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderMouseDownStyle"], null, null);
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideHotStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideHotStyle"], null, null);
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideMouseDownStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideMouseDownStyle"], null, null);
				componentChangeService.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideStyle"]);
				componentChangeService.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideStyle"], null, null);
			}
		}
		designerTransaction?.Commit();
	}

	private IDesignerHost GetIDesignerHost()
	{
		try
		{
			return (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		}
		catch
		{
		}
		return null;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["Appearance"] = TypeDescriptor.CreateProperty(typeof(SideBarDesigner), (PropertyDescriptor)properties["Appearance"]);
		properties["Style"] = TypeDescriptor.CreateProperty(typeof(SideBarDesigner), (PropertyDescriptor)properties["Style"]);
	}

	protected override bool CanDragItem(BaseItem item)
	{
		if (item is SideBarPanelItem)
		{
			return false;
		}
		return base.CanDragItem(item);
	}
}
