using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;
using Microsoft.Win32;

namespace DevComponents.DotNetBar.Design;

public class RibbonControlDesigner : BarBaseControlDesigner
{
	private bool m_QuickAccessToolbarSelected;

	private eRibbonBackgroundStock _BackgroundImageStock;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[8]
			{
				new DesignerVerb("Create Ribbon Tab", CreateRibbonTab),
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
			if (((ControlDesigner)this).Control is RibbonControl ribbonControl)
			{
				foreach (BaseItem quickToolbarItem in ribbonControl.QuickToolbarItems)
				{
					if (!quickToolbarItem.SystemItem)
					{
						arrayList.Add(quickToolbarItem);
					}
				}
				foreach (BaseItem item in ribbonControl.Items)
				{
					if (!item.SystemItem)
					{
						arrayList.Add(item);
					}
				}
				foreach (RibbonTabItemGroup tabGroup in ribbonControl.TabGroups)
				{
					arrayList.Add(tabGroup);
				}
			}
			return arrayList;
		}
	}

	[Description("Specifies the visual style of the control.")]
	[Browsable(false)]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public eDotNetBarStyle Style
	{
		get
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			return ribbonControl.Style;
		}
		set
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			ribbonControl.Style = value;
			if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false } designerHost))
			{
				return;
			}
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Changing Ribbon Style");
			Form val = null;
			if (value == eDotNetBarStyle.Office2010 || value == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(value) || (value == eDotNetBarStyle.Office2007 && ribbonControl.Style == eDotNetBarStyle.Office2007))
			{
				val = ((Control)ribbonControl).FindForm();
			}
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, null);
				if (val != null)
				{
					componentChangeService.OnComponentChanging(val, null);
				}
			}
			if (val != null)
			{
				RibbonPredefinedColorSchemes.ChangeStyle(value, (Control)(object)val);
			}
			else
			{
				RibbonPredefinedColorSchemes.SetRibbonControlStyle(ribbonControl, value);
			}
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
				if (val != null)
				{
					componentChangeService.OnComponentChanged(val, null, null, null);
					((Control)val).Invalidate(true);
				}
			}
			designerTransaction?.Commit();
		}
	}

	[Browsable(true)]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
	[DefaultValue(false)]
	[Category("Appearance")]
	public bool CaptionVisible
	{
		get
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			return ribbonControl.CaptionVisible;
		}
		set
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			if (ribbonControl.CaptionVisible == value)
			{
				return;
			}
			ribbonControl.CaptionVisible = value;
			if (!ribbonControl.CaptionVisible || (ribbonControl.QuickToolbarItems.Count != 0 && (ribbonControl.QuickToolbarItems.Count != 1 || (!(ribbonControl.QuickToolbarItems[0] is SystemCaptionItem) && !(ribbonControl.QuickToolbarItems[0] is QatCustomizeItem))) && (ribbonControl.QuickToolbarItems.Count != 2 || !(ribbonControl.QuickToolbarItems[0] is QatCustomizeItem)) && ((ribbonControl.EffectiveStyle != eDotNetBarStyle.Windows7 && ribbonControl.EffectiveStyle != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(ribbonControl.EffectiveStyle)) || ribbonControl.QuickToolbarItems.Count != 4 || !(ribbonControl.QuickToolbarItems[0] is SystemCaptionItem) || !(ribbonControl.QuickToolbarItems[3] is SystemCaptionItem))))
			{
				return;
			}
			string imageName = "StartButtonImage.png";
			string imageName2 = "NewDocument.png";
			string imageName3 = "OpenDocument.png";
			string imageName4 = "Save.png";
			string imageName5 = "Share.png";
			string imageName6 = "Print.png";
			string imageName7 = "Close.png";
			string imageName8 = "Exit.png";
			string imageName9 = "Options.png";
			IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (designerHost == null || designerHost.Loading || componentChangeService == null || !(designerHost.TransactionDescription != "Paste components"))
			{
				return;
			}
			DesignerTransaction designerTransaction = designerHost.CreateTransaction();
			try
			{
				if (GlobalManager.Renderer is Office2007Renderer)
				{
					_ = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
				}
				m_CreatingItem = true;
				ApplicationButton applicationButton = designerHost.CreateComponent(typeof(ApplicationButton)) as ApplicationButton;
				applicationButton.HotTrackingStyle = eHotTrackingStyle.Image;
				applicationButton.Image = (Image)(object)LoadImage(imageName);
				applicationButton.ImagePaddingHorizontal = 2;
				applicationButton.ImagePaddingVertical = 2;
				applicationButton.ShowSubItems = false;
				applicationButton.CanCustomize = false;
				applicationButton.AutoExpandOnClick = true;
				applicationButton.Text = "&File";
				componentChangeService.OnComponentChanging(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"]);
				if (ribbonControl.EffectiveStyle == eDotNetBarStyle.Office2010 || ribbonControl.EffectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(ribbonControl.EffectiveStyle))
				{
					applicationButton.ImageFixedSize = new Size(16, 16);
					ribbonControl.Items.Insert(0, applicationButton);
				}
				else
				{
					ribbonControl.QuickToolbarItems.Add(applicationButton, 0);
				}
				componentChangeService.OnComponentChanged(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"], null, null);
				ButtonItem buttonItem = applicationButton;
				ButtonItem buttonItem2 = designerHost.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				buttonItem2.Text = buttonItem2.Name;
				componentChangeService.OnComponentChanging(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"]);
				ribbonControl.QuickToolbarItems.Add(buttonItem2, 1);
				componentChangeService.OnComponentChanged(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"], null, null);
				ItemContainer itemContainer = designerHost.CreateComponent(typeof(ItemContainer)) as ItemContainer;
				itemContainer.BackgroundStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
				itemContainer.LayoutOrientation = eOrientation.Vertical;
				buttonItem.SubItems.Add(itemContainer);
				ItemContainer itemContainer2 = designerHost.CreateComponent(typeof(ItemContainer)) as ItemContainer;
				itemContainer2.BackgroundStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
				itemContainer2.ItemSpacing = 0;
				itemContainer.SubItems.Add(itemContainer2);
				ItemContainer itemContainer3 = designerHost.CreateComponent(typeof(ItemContainer)) as ItemContainer;
				itemContainer3.BackgroundStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
				itemContainer3.LayoutOrientation = eOrientation.Vertical;
				itemContainer3.MinimumSize = new Size(120, 0);
				ButtonItem item = CreateFileButton(designerHost, "&New", (Image)(object)LoadImage(imageName2));
				itemContainer3.SubItems.Add(item);
				item = CreateFileButton(designerHost, "&Open...", (Image)(object)LoadImage(imageName3));
				itemContainer3.SubItems.Add(item);
				item = CreateFileButton(designerHost, "&Save...", (Image)(object)LoadImage(imageName4));
				itemContainer3.SubItems.Add(item);
				item = CreateFileButton(designerHost, "S&hare...", (Image)(object)LoadImage(imageName5));
				item.BeginGroup = true;
				itemContainer3.SubItems.Add(item);
				item = CreateFileButton(designerHost, "&Print...", (Image)(object)LoadImage(imageName6));
				itemContainer3.SubItems.Add(item);
				item = CreateFileButton(designerHost, "&Close", (Image)(object)LoadImage(imageName7));
				item.BeginGroup = true;
				itemContainer3.SubItems.Add(item);
				itemContainer2.SubItems.Add(itemContainer3);
				GalleryContainer galleryContainer = designerHost.CreateComponent(typeof(GalleryContainer)) as GalleryContainer;
				galleryContainer.BackgroundStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
				galleryContainer.LayoutOrientation = eOrientation.Vertical;
				galleryContainer.MinimumSize = new Size(180, 240);
				galleryContainer.MultiLine = false;
				galleryContainer.EnableGalleryPopup = false;
				galleryContainer.PopupUsesStandardScrollbars = false;
				LabelItem labelItem = designerHost.CreateComponent(typeof(LabelItem)) as LabelItem;
				labelItem.CanCustomize = false;
				labelItem.BackColor = Color.Empty;
				labelItem.BorderSide = eBorderSide.Bottom;
				labelItem.BorderType = eBorderType.Etched;
				labelItem.Font = null;
				labelItem.ForeColor = SystemColors.ControlText;
				labelItem.Name = "labelItem8";
				labelItem.PaddingBottom = 2;
				labelItem.PaddingTop = 2;
				labelItem.Stretch = true;
				labelItem.Text = "Recent Documents";
				galleryContainer.SubItems.Add(labelItem);
				galleryContainer.SubItems.Add(CreateMRUButton(designerHost, "&1. Short News 5-7.rtf"));
				galleryContainer.SubItems.Add(CreateMRUButton(designerHost, "&2. Prospect Email.rtf"));
				galleryContainer.SubItems.Add(CreateMRUButton(designerHost, "&3. Customer Email.rtf"));
				galleryContainer.SubItems.Add(CreateMRUButton(designerHost, "&4. example.rtf"));
				itemContainer2.SubItems.Add(galleryContainer);
				ItemContainer itemContainer4 = designerHost.CreateComponent(typeof(ItemContainer)) as ItemContainer;
				itemContainer4.BackgroundStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
				itemContainer4.HorizontalItemAlignment = eHorizontalItemsAlignment.Right;
				item = CreateFileButton(designerHost, "Opt&ions", (Image)(object)LoadImage(imageName9));
				item.ColorTable = eButtonColor.OrangeWithBackground;
				itemContainer4.SubItems.Add(item);
				item = CreateFileButton(designerHost, "E&xit", (Image)(object)LoadImage(imageName8));
				item.ColorTable = eButtonColor.OrangeWithBackground;
				itemContainer4.SubItems.Add(item);
				itemContainer.SubItems.Add(itemContainer4);
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

	[Description("Gets or sets whether control is expanded or not. When control is expanded both the tabs and the tab ribbons are visible.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Layout")]
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

	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	public bool CanCustomize
	{
		get
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			return ribbonControl.CanCustomize;
		}
		set
		{
			RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
			ribbonControl.CanCustomize = value;
			if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false } designerHost))
			{
				return;
			}
			if (value)
			{
				QatCustomizeItem qatCustomizeItem = GetQatCustomizeItem(ribbonControl);
				if (qatCustomizeItem != null)
				{
					return;
				}
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating the QAT");
				try
				{
					m_CreatingItem = true;
					qatCustomizeItem = designerHost.CreateComponent(typeof(QatCustomizeItem)) as QatCustomizeItem;
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					componentChangeService?.OnComponentChanging(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"]);
					ribbonControl.QuickToolbarItems.Add(qatCustomizeItem);
					componentChangeService?.OnComponentChanged(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"], null, null);
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
			QatCustomizeItem qatCustomizeItem2 = GetQatCustomizeItem(ribbonControl);
			if (qatCustomizeItem2 == null)
			{
				return;
			}
			DesignerTransaction designerTransaction2 = designerHost.CreateTransaction("Removing the QAT");
			try
			{
				IComponentChangeService componentChangeService2 = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService2?.OnComponentChanging(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"]);
				ribbonControl.QuickToolbarItems.Remove(qatCustomizeItem2);
				componentChangeService2?.OnComponentChanged(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["QuickToolbarItems"], null, null);
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

	public eRibbonBackgroundStock BackgroundImageStock
	{
		get
		{
			return _BackgroundImageStock;
		}
		set
		{
			if (_BackgroundImageStock != value)
			{
				_BackgroundImageStock = value;
				if (_BackgroundImageStock != 0 && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false } && ((ControlDesigner)this).Control is RibbonControl ribbonControl)
				{
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					componentChangeService?.OnComponentChanging(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["BackgroundImage"]);
					((Control)ribbonControl).BackgroundImage = (Image)(object)LoadImage(value.ToString() + ".png");
					componentChangeService?.OnComponentChanged(ribbonControl, TypeDescriptor.GetProperties(ribbonControl)["BackgroundImage"], null, null);
				}
			}
		}
	}

	public RibbonControlDesigner()
	{
		EnableItemDragDrop = true;
		AcceptExternalControls = false;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode)
		{
			if (component is RibbonControl ribbonControl)
			{
				ribbonControl.SetDesignMode();
				Expanded = ribbonControl.Expanded;
			}
			((ControlDesigner)this).EnableDragDrop(false);
		}
	}

	public override bool CanParent(Control control)
	{
		if (control is RibbonPanel && !((ControlDesigner)this).Control.Controls.Contains(control))
		{
			return true;
		}
		return false;
	}

	private BaseItem GetNewItemContainer()
	{
		RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
		if (m_QuickAccessToolbarSelected)
		{
			return ribbonControl.RibbonStrip.CaptionContainerItem;
		}
		return ribbonControl.RibbonStrip.StripContainerItem;
	}

	protected override void CreateButton(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateButton(GetNewItemContainer());
		}
	}

	protected override void CreateTextBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateTextBox(GetNewItemContainer());
		}
	}

	protected override void CreateComboBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateComboBox(GetNewItemContainer());
		}
	}

	protected override void CreateLabel(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateLabel(GetNewItemContainer());
		}
	}

	protected override void CreateColorPicker(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateColorPicker(GetNewItemContainer());
		}
	}

	protected override void CreateProgressBar(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateProgressBar(GetNewItemContainer());
		}
	}

	protected override void CreateRatingItem(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
		{
			CreateRatingItem(GetNewItemContainer());
		}
	}

	protected override void CreateSwitch(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is RibbonControl)
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
		if (((ControlDesigner)this).Control is RibbonControl ribbonControl)
		{
			AddSubItems(ribbonControl.RibbonStrip.StripContainerItem, arrayList);
			AddSubItems(ribbonControl.RibbonStrip.CaptionContainerItem, arrayList);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		CreateRibbonTab();
		CreateRibbonTab();
		StyleManager styleManager = CreateStyleManager();
		RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
		if (ribbonControl != null)
		{
			try
			{
				ribbonControl.KeyTipsFont = new Font("Tahoma", 7f);
			}
			catch
			{
			}
		}
		Style = eDotNetBarStyle.StyleManagerControlled;
		CanCustomize = true;
		((Control)ribbonControl).Size = new Size(200, 154);
		((Control)ribbonControl).Dock = (DockStyle)1;
		CreateRibbonBar();
		CaptionVisible = true;
		if (styleManager != null)
		{
			styleManager.ManagerStyle = eStyle.Office2010Blue;
		}
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is RibbonControl ribbonControl)
		{
			return ribbonControl.RibbonStrip.GetBaseItemContainer();
		}
		return null;
	}

	protected override Control GetItemContainerControl()
	{
		if (((ControlDesigner)this).Control is RibbonControl ribbonControl)
		{
			return (Control)(object)ribbonControl.RibbonStrip;
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
		if (!(((ControlDesigner)this).Control is RibbonControl ribbonControl))
		{
			return;
		}
		BaseItem stripContainerItem = ribbonControl.RibbonStrip.StripContainerItem;
		if (stripContainerItem != null && ss.PrimarySelection is RibbonTabItem)
		{
			RibbonTabItem ribbonTabItem = ss.PrimarySelection as RibbonTabItem;
			if (stripContainerItem.SubItems.Contains(ribbonTabItem))
			{
				TypeDescriptor.GetProperties(ribbonTabItem)["Checked"].SetValue(ribbonTabItem, true);
			}
		}
	}

	private bool IsApplicationButtonBackstageControl(object primarySelection)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		if (!(((ControlDesigner)this).Control is RibbonControl ribbonControl) || !(ribbonControl.GetApplicationButton() is ApplicationButton))
		{
			return false;
		}
		ApplicationButton applicationButton = ribbonControl.GetApplicationButton() as ApplicationButton;
		if (applicationButton.BackstageTab == null)
		{
			return false;
		}
		SuperTabControl backstageTab = applicationButton.BackstageTab;
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
		if (((ControlDesigner)this).Control is RibbonControl ribbonControl)
		{
			BaseItem stripContainerItem = ribbonControl.RibbonStrip.StripContainerItem;
			if (e.Component is RibbonTabItem && stripContainerItem != null && stripContainerItem.SubItems != null && stripContainerItem.SubItems.Contains((RibbonTabItem)e.Component) && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
			{
				designerHost.DestroyComponent((IComponent)((RibbonTabItem)e.Component).Panel);
			}
		}
		base.OtherComponentRemoving(sender, e);
	}

	protected override void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
		if (e.Component is BaseItem && ribbonControl != null)
		{
			BaseItem baseItem = e.Component as BaseItem;
			if (ribbonControl.Items.Contains(baseItem))
			{
				ribbonControl.Items.Remove(baseItem);
			}
			else if (ribbonControl.QuickToolbarItems.Contains(baseItem))
			{
				ribbonControl.QuickToolbarItems.Remove(baseItem);
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
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		m_QuickAccessToolbarSelected = false;
		Control itemContainerControl = GetItemContainerControl();
		if (!(itemContainerControl is RibbonStrip ribbonStrip))
		{
			return base.OnMouseDown(ref m, button);
		}
		Point pt = ((Control)ribbonStrip).PointToClient(Control.MousePosition);
		if (!((Control)ribbonStrip).ClientRectangle.Contains(pt))
		{
			return base.OnMouseDown(ref m, button);
		}
		if ((int)button == 2097152)
		{
			if (ribbonStrip.QuickToolbarBounds.Contains(pt))
			{
				m_QuickAccessToolbarSelected = true;
			}
			return base.OnMouseDown(ref m, button);
		}
		bool flag = true;
		foreach (RibbonTabItemGroup tabGroup in ribbonStrip.TabGroups)
		{
			foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
			{
				if (displayPosition.Contains(pt))
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(tabGroup);
					ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
					selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
					MouseDownSelectionPerformed = true;
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			return base.OnMouseDown(ref m, button);
		}
		return true;
	}

	protected virtual void CreateRibbonTab(object sender, EventArgs e)
	{
		CreateRibbonTab();
	}

	private void CreateRibbonTab()
	{
		m_QuickAccessToolbarSelected = false;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Create Ribbon Tab");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		RibbonControl ribbonControl = ((ControlDesigner)this).Control as RibbonControl;
		try
		{
			m_CreatingItem = true;
			OnSubItemsChanging();
			RibbonTabItem ribbonTabItem = designerHost.CreateComponent(typeof(RibbonTabItem)) as RibbonTabItem;
			TypeDescriptor.GetProperties(ribbonTabItem)["Text"].SetValue(ribbonTabItem, ribbonTabItem.Name);
			RibbonPanel ribbonPanel = designerHost.CreateComponent(typeof(RibbonPanel)) as RibbonPanel;
			TypeDescriptor.GetProperties(ribbonPanel)["Dock"].SetValue(ribbonPanel, (object)(DockStyle)5);
			TypeDescriptor.GetProperties(ribbonPanel)["ColorSchemeStyle"].SetValue(ribbonPanel, ribbonControl.Style);
			ribbonControl.SetRibbonPanelStyle(ribbonPanel);
			componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
			((ControlDesigner)this).Control.Controls.Add((Control)(object)ribbonPanel);
			((Control)ribbonPanel).SendToBack();
			componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
			TypeDescriptor.GetProperties(ribbonTabItem)["Panel"].SetValue(ribbonTabItem, ribbonPanel);
			GenericItemContainer stripContainerItem = ribbonControl.RibbonStrip.StripContainerItem;
			componentChangeService.OnComponentChanging(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
			stripContainerItem.SubItems.Add(ribbonTabItem);
			componentChangeService.OnComponentChanged(stripContainerItem, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
			if (stripContainerItem.SubItems.Count == 1)
			{
				TypeDescriptor.GetProperties(ribbonTabItem)["Checked"].SetValue(ribbonTabItem, true);
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

	private void CreateRibbonBar()
	{
		if (!(((ControlDesigner)this).Control is RibbonControl { SelectedRibbonTabItem: not null } ribbonControl) || ribbonControl.SelectedRibbonTabItem.Panel == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Create Default Ribbon Bar");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		try
		{
			RibbonBar ribbonBar = designerHost.CreateComponent(typeof(RibbonBar)) as RibbonBar;
			TypeDescriptor.GetProperties(ribbonBar)["Width"].SetValue(ribbonBar, 100);
			TypeDescriptor.GetProperties(ribbonBar)["Text"].SetValue(ribbonBar, ((Control)ribbonBar).Name);
			componentChangeService.OnComponentChanging(ribbonControl.SelectedRibbonTabItem.Panel, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
			((Control)ribbonControl.SelectedRibbonTabItem.Panel).Controls.Add((Control)(object)ribbonBar);
			((Control)ribbonBar).Dock = (DockStyle)3;
			componentChangeService.OnComponentChanged(ribbonControl.SelectedRibbonTabItem.Panel, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
			RecalcLayout();
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

	private QatCustomizeItem GetQatCustomizeItem(RibbonControl rc)
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
		properties["Expanded"] = TypeDescriptor.CreateProperty(typeof(RibbonControlDesigner), (PropertyDescriptor)properties["Expanded"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Layout"));
		properties["Style"] = TypeDescriptor.CreateProperty(typeof(RibbonControlDesigner), (PropertyDescriptor)properties["Style"], new DefaultValueAttribute(eDotNetBarStyle.Office2003), new BrowsableAttribute(browsable: false), new CategoryAttribute("Appearance"));
		properties["CaptionVisible"] = TypeDescriptor.CreateProperty(typeof(RibbonControlDesigner), (PropertyDescriptor)properties["CaptionVisible"], new DefaultValueAttribute(value: false), new BrowsableAttribute(browsable: true), new CategoryAttribute("Appearance"), new DescriptionAttribute("Indicates whether custom caption and quick access toolbar provided by the control is visible."));
		properties["CanCustomize"] = TypeDescriptor.CreateProperty(typeof(RibbonControlDesigner), (PropertyDescriptor)properties["CanCustomize"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Quick Access Toolbar"), new DescriptionAttribute("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled"));
		properties["BackgroundImageStock"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "BackgroundImageStock", typeof(eRibbonBackgroundStock), new BrowsableAttribute(browsable: true), new DesignOnlyAttribute(isDesignOnly: true), new DefaultValueAttribute(eRibbonBackgroundStock.PickOne), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden), new DescriptionAttribute("Specifies the stock ribbon background image loaded from DotNetBar Images folder"));
	}
}
