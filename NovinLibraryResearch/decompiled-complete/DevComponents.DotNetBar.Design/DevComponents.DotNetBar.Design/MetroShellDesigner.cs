using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;
using Microsoft.Win32;

namespace DevComponents.DotNetBar.Design;

public class MetroShellDesigner : BarBaseControlDesigner
{
	private bool m_QuickAccessToolbarSelected;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[8]
			{
				new DesignerVerb("Create Tab", CreateMetroTab),
				new DesignerVerb("Create Button", CreateButton),
				new DesignerVerb("Create Text Box", CreateTextBox),
				new DesignerVerb("Create Combo Box", CreateComboBox),
				new DesignerVerb("Create Label", CreateLabel),
				new DesignerVerb("Create Micro-Chart", CreateMicroChart),
				new DesignerVerb("Create Switch Button", CreateSwitch),
				new DesignerVerb("Create Rating Item", CreateRatingItem)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.BaseAssociatedComponents);
			if (((ControlDesigner)this).Control is MetroShell metroShell)
			{
				foreach (BaseItem quickToolbarItem in metroShell.QuickToolbarItems)
				{
					if (!quickToolbarItem.SystemItem)
					{
						arrayList.Add(quickToolbarItem);
					}
				}
				foreach (BaseItem item in metroShell.Items)
				{
					if (!item.SystemItem)
					{
						arrayList.Add(item);
					}
				}
			}
			return arrayList;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
	[Browsable(true)]
	public bool CaptionVisible
	{
		get
		{
			MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
			return metroShell.CaptionVisible;
		}
		set
		{
			MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
			if (metroShell.CaptionVisible == value)
			{
				return;
			}
			metroShell.CaptionVisible = value;
			if (!metroShell.CaptionVisible || metroShell.QuickToolbarItems.Count != 6 || !(metroShell.QuickToolbarItems[0] is SystemCaptionItem) || !(metroShell.QuickToolbarItems[5] is SystemCaptionItem))
			{
				return;
			}
			IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (designerHost == null || designerHost.Loading || componentChangeService == null || !(designerHost.TransactionDescription != "Paste components"))
			{
				return;
			}
			DesignerTransaction designerTransaction = designerHost.CreateTransaction();
			try
			{
				m_CreatingItem = true;
				MetroAppButton metroAppButton = designerHost.CreateComponent(typeof(MetroAppButton)) as MetroAppButton;
				metroAppButton.Text = "&File";
				metroAppButton.ShowSubItems = false;
				metroAppButton.CanCustomize = false;
				metroAppButton.AutoExpandOnClick = true;
				componentChangeService.OnComponentChanging(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"]);
				metroAppButton.ImageFixedSize = new Size(16, 16);
				metroShell.Items.Insert(0, metroAppButton);
				componentChangeService.OnComponentChanged(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"], null, null);
				ButtonItem buttonItem = designerHost.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				buttonItem.Text = buttonItem.Name;
				componentChangeService.OnComponentChanging(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"]);
				metroShell.QuickToolbarItems.Add(buttonItem, 2);
				componentChangeService.OnComponentChanged(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"], null, null);
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
				m_CreatingItem = false;
			}
		}
	}

	[Category("Quick Access Toolbar")]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool CanCustomize
	{
		get
		{
			MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
			return metroShell.CanCustomize;
		}
		set
		{
			MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
			metroShell.CanCustomize = value;
			if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false } designerHost))
			{
				return;
			}
			if (value)
			{
				QatCustomizeItem qatCustomizeItem = GetQatCustomizeItem(metroShell);
				if (qatCustomizeItem != null)
				{
					return;
				}
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating the QAT");
				try
				{
					m_CreatingItem = true;
					qatCustomizeItem = designerHost.CreateComponent(typeof(QatCustomizeItem)) as QatCustomizeItem;
					qatCustomizeItem.BeginGroup = true;
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					componentChangeService?.OnComponentChanging(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"]);
					metroShell.QuickToolbarItems.Add(qatCustomizeItem);
					componentChangeService?.OnComponentChanged(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"], null, null);
					m_CreatingItem = false;
					RecalcLayout();
					return;
				}
				catch
				{
					designerTransaction.Cancel();
					return;
				}
				finally
				{
					if (!designerTransaction.Canceled)
					{
						designerTransaction.Commit();
					}
				}
			}
			QatCustomizeItem qatCustomizeItem2 = GetQatCustomizeItem(metroShell);
			if (qatCustomizeItem2 == null)
			{
				return;
			}
			DesignerTransaction designerTransaction2 = designerHost.CreateTransaction("Removing the QAT");
			try
			{
				IComponentChangeService componentChangeService2 = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService2?.OnComponentChanging(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"]);
				metroShell.QuickToolbarItems.Remove(qatCustomizeItem2);
				componentChangeService2?.OnComponentChanged(metroShell, TypeDescriptor.GetProperties(metroShell)["QuickToolbarItems"], null, null);
				designerHost.DestroyComponent(qatCustomizeItem2);
			}
			catch
			{
				designerTransaction2.Cancel();
			}
			finally
			{
				if (!designerTransaction2.Canceled)
				{
					designerTransaction2.Commit();
				}
			}
		}
	}

	public MetroShellDesigner()
	{
		EnableItemDragDrop = true;
		AcceptExternalControls = false;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode)
		{
			if (component is MetroShell metroShell)
			{
				metroShell.SetDesignMode();
			}
			((ControlDesigner)this).EnableDragDrop(false);
		}
	}

	public override bool CanParent(Control control)
	{
		if (control is MetroTabPanel && !((ControlDesigner)this).Control.Controls.Contains(control))
		{
			return true;
		}
		return false;
	}

	private BaseItem GetNewItemContainer()
	{
		MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
		if (m_QuickAccessToolbarSelected)
		{
			return metroShell.MetroTabStrip.CaptionContainerItem;
		}
		return metroShell.MetroTabStrip.StripContainerItem;
	}

	protected override void CreateButton(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateButton(GetNewItemContainer());
		}
	}

	protected override void CreateTextBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateTextBox(GetNewItemContainer());
		}
	}

	protected override void CreateComboBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateComboBox(GetNewItemContainer());
		}
	}

	protected override void CreateLabel(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateLabel(GetNewItemContainer());
		}
	}

	protected override void CreateColorPicker(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateColorPicker(GetNewItemContainer());
		}
	}

	protected override void CreateProgressBar(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateProgressBar(GetNewItemContainer());
		}
	}

	protected override void CreateRatingItem(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateRatingItem(GetNewItemContainer());
		}
	}

	protected override void CreateSwitch(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell)
		{
			CreateSwitch(GetNewItemContainer());
		}
	}

	protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (m_AddingItem)
		{
			m_AddingItem = false;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ControlDesigner)this).Control, null);
			GetNewItemContainer().SubItems.Add(e.Component as BaseItem);
			componentChangeService?.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
			m_InsertItemTransaction.Commit();
			m_InsertItemTransaction = null;
			RecalcLayout();
		}
	}

	protected override ArrayList GetAllAssociatedComponents()
	{
		ArrayList arrayList = new ArrayList(base.AssociatedComponents);
		if (((ControlDesigner)this).Control is MetroShell metroShell)
		{
			AddSubItems(metroShell.MetroTabStrip.StripContainerItem, arrayList);
			AddSubItems(metroShell.MetroTabStrip.CaptionContainerItem, arrayList);
		}
		return arrayList;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private StyleManager CreateStyleManager()
	{
		StyleManager result = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Create Style Manager");
			try
			{
				result = designerHost.CreateComponent(typeof(StyleManager)) as StyleManager;
			}
			finally
			{
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
			}
		}
		return result;
	}

	private void SetDesignTimeDefaults()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		CreateMetroTab("&HOME");
		CreateMetroTab("&VIEW");
		StyleManager styleManager = CreateStyleManager();
		MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
		if (metroShell != null)
		{
			try
			{
				metroShell.KeyTipsFont = new Font("Tahoma", 7f);
				metroShell.TabStripFont = new Font("Segoe UI", 10.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			}
			catch
			{
			}
		}
		CanCustomize = true;
		((Control)metroShell).Size = new Size(200, 154);
		((Control)metroShell).Dock = (DockStyle)1;
		CaptionVisible = true;
		if (styleManager != null)
		{
			styleManager.ManagerStyle = eStyle.Metro;
		}
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is MetroShell metroShell)
		{
			return metroShell.MetroTabStrip.GetBaseItemContainer();
		}
		return null;
	}

	protected override Control GetItemContainerControl()
	{
		if (((ControlDesigner)this).Control is MetroShell metroShell)
		{
			return (Control)(object)metroShell.MetroTabStrip;
		}
		return null;
	}

	protected override void DesignTimeSelectionChanged(ISelectionService ss)
	{
		if (ss == null || ((ControlDesigner)this).Control == null || ((ControlDesigner)this).Control.IsDisposed || IsApplicationButtonBackstageControl(ss.PrimarySelection))
		{
			return;
		}
		base.DesignTimeSelectionChanged(ss);
		if (!(((ControlDesigner)this).Control is MetroShell metroShell))
		{
			return;
		}
		BaseItem stripContainerItem = metroShell.MetroTabStrip.StripContainerItem;
		if (stripContainerItem != null && ss.PrimarySelection is MetroTabItem)
		{
			MetroTabItem metroTabItem = ss.PrimarySelection as MetroTabItem;
			if (stripContainerItem.SubItems.Contains(metroTabItem))
			{
				TypeDescriptor.GetProperties(metroTabItem)["Checked"].SetValue(metroTabItem, true);
			}
		}
	}

	private bool IsApplicationButtonBackstageControl(object primarySelection)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		if (!(((ControlDesigner)this).Control is MetroShell metroShell) || !(metroShell.GetApplicationButton() is MetroAppButton))
		{
			return false;
		}
		MetroAppButton metroAppButton = metroShell.GetApplicationButton() as MetroAppButton;
		if (metroAppButton.BackstageTab == null)
		{
			return false;
		}
		SuperTabControl backstageTab = metroAppButton.BackstageTab;
		if (primarySelection is BaseItem)
		{
			BaseItem baseItem = (BaseItem)primarySelection;
			if (backstageTab.Tabs.Contains(baseItem))
			{
				return true;
			}
			while (baseItem.Parent != null)
			{
				baseItem = baseItem.Parent;
			}
			object containerControl = baseItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null)
			{
				return false;
			}
			return ContainsControl((Control)(object)backstageTab, val);
		}
		if (primarySelection is Control)
		{
			return ContainsControl((Control)(object)backstageTab, (Control)primarySelection);
		}
		return false;
	}

	private bool ContainsControl(Control container, Control childControl)
	{
		for (Control val = childControl; val != null; val = val.Parent)
		{
			if (val == container)
			{
				return true;
			}
		}
		return false;
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (((ControlDesigner)this).Control is MetroShell metroShell)
		{
			BaseItem stripContainerItem = metroShell.MetroTabStrip.StripContainerItem;
			if (e.Component is MetroTabItem && stripContainerItem != null && stripContainerItem.SubItems != null && stripContainerItem.SubItems.Contains((MetroTabItem)e.Component) && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
			{
				designerHost.DestroyComponent((IComponent)((MetroTabItem)e.Component).Panel);
			}
		}
		base.OtherComponentRemoving(sender, e);
	}

	protected override void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
		if (e.Component is BaseItem && metroShell != null)
		{
			BaseItem baseItem = e.Component as BaseItem;
			if (metroShell.Items.Contains(baseItem))
			{
				metroShell.Items.Remove(baseItem);
			}
			else if (metroShell.QuickToolbarItems.Contains(baseItem))
			{
				metroShell.QuickToolbarItems.Remove(baseItem);
			}
			DestroySubItems(baseItem);
		}
	}

	protected override bool OnMouseDown(ref Message m, MouseButtons button)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		m_QuickAccessToolbarSelected = false;
		Control itemContainerControl = GetItemContainerControl();
		if (!(itemContainerControl is MetroTabStrip metroTabStrip))
		{
			return base.OnMouseDown(ref m, button);
		}
		Point pt = ((Control)metroTabStrip).PointToClient(Control.MousePosition);
		if (!((Control)metroTabStrip).ClientRectangle.Contains(pt))
		{
			return base.OnMouseDown(ref m, button);
		}
		if ((int)button == 2097152)
		{
			if (metroTabStrip.QuickToolbarBounds.Contains(pt))
			{
				m_QuickAccessToolbarSelected = true;
			}
			return base.OnMouseDown(ref m, button);
		}
		if (true)
		{
			return base.OnMouseDown(ref m, button);
		}
		return true;
	}

	protected virtual void CreateMetroTab(object sender, EventArgs e)
	{
		CreateMetroTab(string.Empty);
	}

	private void CreateMetroTab(string text)
	{
		m_QuickAccessToolbarSelected = false;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating Metro Tab");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		MetroShell metroShell = ((ControlDesigner)this).Control as MetroShell;
		try
		{
			m_CreatingItem = true;
			OnSubItemsChanging();
			MetroTabItem metroTabItem = designerHost.CreateComponent(typeof(MetroTabItem)) as MetroTabItem;
			if (string.IsNullOrEmpty(text))
			{
				TypeDescriptor.GetProperties(metroTabItem)["Text"].SetValue(metroTabItem, metroTabItem.Name);
			}
			else
			{
				TypeDescriptor.GetProperties(metroTabItem)["Text"].SetValue(metroTabItem, text);
			}
			MetroTabPanel metroTabPanel = designerHost.CreateComponent(typeof(MetroTabPanel)) as MetroTabPanel;
			TypeDescriptor.GetProperties(metroTabPanel)["Dock"].SetValue(metroTabPanel, (object)(DockStyle)5);
			metroShell.SetTabPanelStyle(metroTabPanel);
			componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
			((ControlDesigner)this).Control.Controls.Add((Control)(object)metroTabPanel);
			((Control)metroTabPanel).SendToBack();
			componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
			TypeDescriptor.GetProperties(metroTabItem)["Panel"].SetValue(metroTabItem, metroTabPanel);
			SimpleItemContainer stripContainerItem = metroShell.MetroTabStrip.StripContainerItem;
			componentChangeService.OnComponentChanging(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
			stripContainerItem.SubItems.Add(metroTabItem);
			componentChangeService.OnComponentChanged(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
			if (stripContainerItem.SubItems.Count == 1)
			{
				TypeDescriptor.GetProperties(metroTabItem)["Checked"].SetValue(metroTabItem, true);
			}
			RecalcLayout();
			OnSubItemsChanged();
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
			m_CreatingItem = false;
		}
	}

	private ButtonItem CreateFileButton(IDesignerHost dh, string text, Image image)
	{
		ButtonItem buttonItem = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem.Image = image;
		buttonItem.SubItemsExpandWidth = 24;
		buttonItem.Text = text;
		return buttonItem;
	}

	private ButtonItem CreateMRUButton(IDesignerHost dh, string text)
	{
		ButtonItem buttonItem = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
		buttonItem.Text = text;
		return buttonItem;
	}

	internal static Bitmap LoadImage(string imageName)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		string imagesFolder = GetImagesFolder();
		if (imagesFolder != "" && File.Exists(imagesFolder + imageName))
		{
			return new Bitmap(imagesFolder + imageName);
		}
		return null;
	}

	private static string GetImagesFolder()
	{
		try
		{
			RegistryKey val = Registry.LocalMachine;
			string text = "";
			try
			{
				if (val != null)
				{
					val = val.OpenSubKey("Software\\DevComponents\\DotNetBar");
				}
				if (val != null)
				{
					text = val.GetValue("InstallationFolder", (object)"").ToString();
				}
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
			}
			if (text != "")
			{
				if (text.Substring(text.Length - 1, 1) != "\\")
				{
					text += "\\";
				}
				return text + "Images\\";
			}
		}
		catch (Exception)
		{
		}
		return "";
	}

	private QatCustomizeItem GetQatCustomizeItem(MetroShell rc)
	{
		QatCustomizeItem result = null;
		foreach (BaseItem quickToolbarItem in rc.QuickToolbarItems)
		{
			if (quickToolbarItem is QatCustomizeItem)
			{
				result = quickToolbarItem as QatCustomizeItem;
				break;
			}
		}
		return result;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["CaptionVisible"] = TypeDescriptor.CreateProperty(typeof(MetroShellDesigner), (PropertyDescriptor)properties["CaptionVisible"], new DefaultValueAttribute(value: false), new BrowsableAttribute(browsable: true), new CategoryAttribute("Appearance"), new DescriptionAttribute("Indicates whether custom caption and quick access toolbar provided by the control is visible."));
		properties["CanCustomize"] = TypeDescriptor.CreateProperty(typeof(MetroShellDesigner), (PropertyDescriptor)properties["CanCustomize"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Quick Access Toolbar"), new DescriptionAttribute("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled"));
	}
}
