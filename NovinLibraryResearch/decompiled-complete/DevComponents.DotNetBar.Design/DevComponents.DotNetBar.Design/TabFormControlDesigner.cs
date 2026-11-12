using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using Microsoft.Win32;

namespace DevComponents.DotNetBar.Design;

internal class TabFormControlDesigner : BarBaseControlDesigner
{
	private DesignerActionListCollection _ActionLists;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[4]
			{
				new DesignerVerb("Create Tab", CreateTab),
				new DesignerVerb("Create Button", CreateButton),
				new DesignerVerb("Create Combo Box", CreateComboBox),
				new DesignerVerb("Create Label", CreateLabel)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_ActionLists == null)
			{
				_ActionLists = new DesignerActionListCollection();
				_ActionLists.Add((DesignerActionList)(object)new TabFormControlActionList(this));
			}
			return _ActionLists;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.BaseAssociatedComponents);
			if (((ControlDesigner)this).Control is TabFormControl tabFormControl)
			{
				foreach (BaseItem item in tabFormControl.Items)
				{
					if (!item.SystemItem)
					{
						arrayList.Add(item);
					}
				}
				foreach (BaseItem captionItem in tabFormControl.CaptionItems)
				{
					if (!captionItem.SystemItem)
					{
						arrayList.Add(captionItem);
					}
				}
			}
			return arrayList;
		}
	}

	public TabFormControlDesigner()
	{
		EnableItemDragDrop = true;
		AcceptExternalControls = false;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode)
		{
			if (component is TabFormControl tabFormControl)
			{
				tabFormControl.SetDesignMode();
			}
			((ControlDesigner)this).EnableDragDrop(false);
		}
	}

	public override bool CanParent(Control control)
	{
		if (control is TabFormPanel && !((ControlDesigner)this).Control.Controls.Contains(control))
		{
			return true;
		}
		return false;
	}

	private BaseItem GetNewItemContainer()
	{
		TabFormControl tabFormControl = ((ControlDesigner)this).Control as TabFormControl;
		return tabFormControl.TabStrip.StripContainerItem;
	}

	public override ButtonItem CreateButton()
	{
		OnSubItemsChanging();
		ButtonItem result = CreateButton(GetNewItemContainer());
		OnSubItemsChanged();
		return result;
	}

	protected override void CreateButton(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateButton(GetNewItemContainer());
		}
	}

	protected override void CreateTextBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateTextBox(GetNewItemContainer());
		}
	}

	protected override void CreateComboBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateComboBox(GetNewItemContainer());
		}
	}

	protected override void CreateLabel(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateLabel(GetNewItemContainer());
		}
	}

	protected override void CreateColorPicker(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateColorPicker(GetNewItemContainer());
		}
	}

	protected override void CreateProgressBar(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateProgressBar(GetNewItemContainer());
		}
	}

	protected override void CreateRatingItem(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
		{
			CreateRatingItem(GetNewItemContainer());
		}
	}

	protected override void CreateSwitch(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabFormControl)
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
		if (((ControlDesigner)this).Control is TabFormControl tabFormControl)
		{
			AddSubItems(tabFormControl.TabStrip.StripContainerItem, arrayList);
			AddSubItems(tabFormControl.TabStrip.CaptionContainerItem, arrayList);
		}
		return arrayList;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		CreateTab("Document 1");
		CreateTab("Document 2");
		TabFormControl tabFormControl = ((ControlDesigner)this).Control as TabFormControl;
		((Control)tabFormControl).Dock = (DockStyle)5;
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is TabFormControl tabFormControl)
		{
			return tabFormControl.TabStrip.GetBaseItemContainer();
		}
		return null;
	}

	protected override Control GetItemContainerControl()
	{
		if (((ControlDesigner)this).Control is TabFormControl tabFormControl)
		{
			return (Control)(object)tabFormControl.TabStrip;
		}
		return null;
	}

	protected override void DesignTimeSelectionChanged(ISelectionService ss)
	{
		if (ss == null || ((ControlDesigner)this).Control == null || ((ControlDesigner)this).Control.IsDisposed)
		{
			return;
		}
		base.DesignTimeSelectionChanged(ss);
		if (!(((ControlDesigner)this).Control is TabFormControl tabFormControl))
		{
			return;
		}
		BaseItem stripContainerItem = tabFormControl.TabStrip.StripContainerItem;
		if (stripContainerItem != null && ss.PrimarySelection is TabFormItem)
		{
			TabFormItem tabFormItem = ss.PrimarySelection as TabFormItem;
			if (stripContainerItem.SubItems.Contains(tabFormItem))
			{
				TypeDescriptor.GetProperties(tabFormItem)["Checked"].SetValue(tabFormItem, true);
			}
		}
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
		if (((ControlDesigner)this).Control is TabFormControl tabFormControl)
		{
			BaseItem stripContainerItem = tabFormControl.TabStrip.StripContainerItem;
			if (e.Component is TabFormItem && stripContainerItem != null && stripContainerItem.SubItems != null && stripContainerItem.SubItems.Contains((TabFormItem)e.Component) && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
			{
				designerHost.DestroyComponent((IComponent)((TabFormItem)e.Component).Panel);
			}
		}
		base.OtherComponentRemoving(sender, e);
	}

	protected override void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		TabFormControl tabFormControl = ((ControlDesigner)this).Control as TabFormControl;
		if (e.Component is BaseItem && tabFormControl != null)
		{
			BaseItem baseItem = e.Component as BaseItem;
			if (tabFormControl.Items.Contains(baseItem))
			{
				tabFormControl.Items.Remove(baseItem);
			}
			else if (tabFormControl.CaptionItems.Contains(baseItem))
			{
				tabFormControl.CaptionItems.Remove(baseItem);
			}
			DestroySubItems(baseItem);
		}
	}

	protected override bool OnMouseDown(ref Message m, MouseButtons button)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Control itemContainerControl = GetItemContainerControl();
		if (!(itemContainerControl is TabFormStripControl tabFormStripControl))
		{
			return base.OnMouseDown(ref m, button);
		}
		Point pt = ((Control)tabFormStripControl).PointToClient(Control.MousePosition);
		if (!((Control)tabFormStripControl).ClientRectangle.Contains(pt))
		{
			return base.OnMouseDown(ref m, button);
		}
		if (true)
		{
			return base.OnMouseDown(ref m, button);
		}
		return true;
	}

	protected virtual void CreateTab(object sender, EventArgs e)
	{
		CreateTab(string.Empty);
	}

	internal void CreateTab(string text)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating Tab");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		TabFormControl tabFormControl = ((ControlDesigner)this).Control as TabFormControl;
		try
		{
			m_CreatingItem = true;
			OnSubItemsChanging();
			TabFormItem tabFormItem = designerHost.CreateComponent(typeof(TabFormItem)) as TabFormItem;
			if (string.IsNullOrEmpty(text))
			{
				TypeDescriptor.GetProperties(tabFormItem)["Text"].SetValue(tabFormItem, tabFormItem.Name);
			}
			else
			{
				TypeDescriptor.GetProperties(tabFormItem)["Text"].SetValue(tabFormItem, text);
			}
			TabFormPanel tabFormPanel = (TabFormPanel)designerHost.CreateComponent(typeof(TabFormPanel));
			TypeDescriptor.GetProperties(tabFormPanel)["Dock"].SetValue(tabFormPanel, (object)(DockStyle)5);
			tabFormControl.SetTabPanelStyle(tabFormPanel);
			componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
			((ControlDesigner)this).Control.Controls.Add((Control)(object)tabFormPanel);
			((Control)tabFormPanel).SendToBack();
			componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
			TypeDescriptor.GetProperties(tabFormItem)["Panel"].SetValue(tabFormItem, tabFormPanel);
			SimpleItemContainer stripContainerItem = tabFormControl.TabStrip.StripContainerItem;
			componentChangeService.OnComponentChanging(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
			stripContainerItem.SubItems.Add(tabFormItem);
			componentChangeService.OnComponentChanged(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
			if (stripContainerItem.SubItems.Count == 1)
			{
				TypeDescriptor.GetProperties(tabFormItem)["Checked"].SetValue(tabFormItem, true);
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

	protected override void OnItemDragAndDropped(BaseItem dragItem)
	{
		TabFormControl tabFormControl = (TabFormControl)((ComponentDesigner)this).Component;
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null && tabFormControl != null && tabFormControl.CaptionItems.Contains(dragItem))
		{
			componentChangeService.OnComponentChanged(tabFormControl, TypeDescriptor.GetProperties(typeof(BaseItem))["CaptionItems"], null, null);
		}
		else
		{
			base.OnItemDragAndDropped(dragItem);
		}
	}

	public void CreateCaptionButton()
	{
		ButtonItem buttonItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ButtonItem)) is ButtonItem buttonItem2)
			{
				TabFormControl tabFormControl = (TabFormControl)((ComponentDesigner)this).Component;
				TypeDescriptor.GetProperties(buttonItem2)["Text"].SetValue(buttonItem2, buttonItem2.Name);
				buttonItem2.ItemAlignment = eItemAlignment.Far;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(typeof(TabFormControl))["CaptionItems"]);
				tabFormControl.CaptionItems.Add(buttonItem2);
				RecalcLayout();
				componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(typeof(TabFormControl))["CaptionItems"], null, null);
				OnitemCreated(buttonItem2);
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
			m_CreatingItem = false;
		}
	}

	public void CreateCaptionLabel()
	{
		LabelItem labelItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(LabelItem)) is LabelItem labelItem2)
			{
				TabFormControl tabFormControl = (TabFormControl)((ComponentDesigner)this).Component;
				TypeDescriptor.GetProperties(labelItem2)["Text"].SetValue(labelItem2, labelItem2.Name);
				labelItem2.ItemAlignment = eItemAlignment.Far;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(typeof(TabFormControl))["CaptionItems"]);
				tabFormControl.CaptionItems.Add(labelItem2);
				RecalcLayout();
				componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(typeof(TabFormControl))["CaptionItems"], null, null);
				OnitemCreated(labelItem2);
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
			m_CreatingItem = false;
		}
	}

	internal void CreateLabel()
	{
		CreateLabel(this, EventArgs.Empty);
	}

	internal void CreateComboBox()
	{
		CreateComboBox(this, EventArgs.Empty);
	}

	internal void CreateTextBox()
	{
		CreateTextBox(this, EventArgs.Empty);
	}
}
