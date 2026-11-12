using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class BarDesigner : BarBaseControlDesigner
{
	private int m_MouseDownSelectedTabIndex = -1;

	private bool m_IsDocking;

	private Form m_OutlineForm;

	private DockSiteInfo m_DockInfo;

	private bool m_DragDrop;

	private Timer m_TimerDelayRemove;

	private Bar m_DelayDockBar;

	private int m_DelayDockTabIndex = -1;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = ((!IsDockableWindow) ? new DesignerVerb[18]
			{
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Container", CreateContainer),
				new DesignerVerb("Add Progress-Bar", CreateProgressBar),
				new DesignerVerb("Add Circular Progress", CreateCircularProgressItem),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart),
				new DesignerVerb("Add Switch Button", CreateSwitch),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Check Box", CreateCheckBox),
				new DesignerVerb("Add Slider", CreateSliderItem),
				new DesignerVerb("Add Range Slider", CreateRangeSliderItem),
				new DesignerVerb("Add Customize Item", CreateCustomizeItem),
				new DesignerVerb("Add MDI Window List Item", CreateMdiWindowList),
				new DesignerVerb("Add Rating Item", CreateRatingItem),
				new DesignerVerb("Add Control Container", CreateControlContainer)
			} : new DesignerVerb[1]
			{
				new DesignerVerb("Create Dock Tab", CreateDocument)
			});
			return new DesignerVerbCollection(array);
		}
	}

	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[Description("Specifies the visual style of the Bar.")]
	[Browsable(true)]
	[Category("Appearance")]
	public eDotNetBarStyle Style
	{
		get
		{
			Bar bar = ((ControlDesigner)this).Control as Bar;
			return bar.Style;
		}
		set
		{
			Bar bar = ((ControlDesigner)this).Control as Bar;
			bool flag = bar.Style != value;
			bar.Style = value;
			if (flag && bar.Owner is DotNetBarManager && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false })
			{
				DotNetBarManager component = bar.Owner as DotNetBarManager;
				TypeDescriptor.GetProperties(component)["Style"].SetValue(component, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Gets or sets whether items on the Bar can be customized.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool CanCustomize
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["CanCustomize"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["CanCustomize"] = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.")]
	public bool AutoHide
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["AutoHide"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["AutoHide"] = value;
		}
	}

	public override SelectionRules SelectionRules
	{
		get
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (IsDockableWindow || ((ControlDesigner)this).Control.Parent is DockSite)
			{
				return (SelectionRules)int.MinValue;
			}
			return ((ControlDesigner)this).SelectionRules;
		}
	}

	protected override bool IsDockableWindow
	{
		get
		{
			if (((ControlDesigner)this).Control is Bar { LayoutType: eLayoutType.DockContainer })
			{
				return true;
			}
			return false;
		}
	}

	private bool IsDockableToolbar
	{
		get
		{
			if (((ControlDesigner)this).Control is Bar { LayoutType: eLayoutType.Toolbar } bar && ((Control)bar).Parent is DockSite)
			{
				return true;
			}
			return false;
		}
	}

	private bool IsDocumentDock
	{
		get
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			if (!(((ControlDesigner)this).Control is Bar { LayoutType: eLayoutType.DockContainer } bar))
			{
				return false;
			}
			if (((Control)bar).Parent is DockSite && (int)((Control)bar).Parent.Dock == 5)
			{
				return true;
			}
			return false;
		}
	}

	protected override eDotNetBarStyle InternalStyle
	{
		get
		{
			if (((ControlDesigner)this).Control is Bar bar)
			{
				return bar.Style;
			}
			return base.InternalStyle;
		}
	}

	public BarDesigner()
	{
		m_DockInfo = new DockSiteInfo();
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component is Bar bar)
		{
			bar.SetDesignMode(b: true);
		}
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete += DesignerLoadComplete;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["AutoHide"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "AutoHide", typeof(bool), new BrowsableAttribute(browsable: true), new DefaultValueAttribute(value: false), new CategoryAttribute("Auto-Hide"), new DescriptionAttribute("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only."));
		properties["CanCustomize"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "CanCustomize", typeof(bool), new BrowsableAttribute(browsable: true), new DefaultValueAttribute(value: true), new CategoryAttribute("Behavior"), new DescriptionAttribute("Gets or sets whether items on the Bar can be customized."));
		properties["Style"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "Style", typeof(eDotNetBarStyle), new BrowsableAttribute(browsable: true), new DefaultValueAttribute(eDotNetBarStyle.OfficeXP), new CategoryAttribute("Appearance"), new DescriptionAttribute("Gets or sets the control style."));
	}

	private void DesignerLoadComplete(object sender, EventArgs e)
	{
		if (IsDockableWindow)
		{
			Bar bar = ((ControlDesigner)this).Control as Bar;
			DockContainerItem selectedDockContainerItem = bar.SelectedDockContainerItem;
			if (selectedDockContainerItem != null && selectedDockContainerItem.Control != null && ((Control)bar).Controls.IndexOf(selectedDockContainerItem.Control) > 0)
			{
				((Control)bar).Controls.SetChildIndex(selectedDockContainerItem.Control, 0);
			}
		}
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete -= DesignerLoadComplete;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	protected virtual void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component is Bar bar)
		{
			bar.Style = eDotNetBarStyle.StyleManagerControlled;
			bar.AntiAlias = true;
		}
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is Bar bar)
		{
			return bar.ItemsContainer;
		}
		return null;
	}

	protected override void RecalcLayout()
	{
		if (GetItemContainerControl() is Bar bar)
		{
			bar.RecalcLayout();
		}
	}

	protected override void OnSubItemsChanging()
	{
		base.OnSubItemsChanging();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			Bar component = GetItemContainerControl() as Bar;
			componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Items", ignoreCase: true));
		}
	}

	protected override void OnSubItemsChanged()
	{
		base.OnSubItemsChanged();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			Bar component = GetItemContainerControl() as Bar;
			componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Items", ignoreCase: true), null, null);
		}
	}

	public override bool CanParent(Control control)
	{
		if (control.Contains(((ControlDesigner)this).Control) || control is Bar)
		{
			return false;
		}
		if (IsDockableWindow && !(control is PanelDockContainer))
		{
			return false;
		}
		return base.CanParent(control);
	}

	private void SelectDockTab(int index)
	{
		if (!(((ControlDesigner)this).Control is Bar bar))
		{
			return;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		componentChangeService?.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
		bar.SelectedDockTab = index;
		componentChangeService?.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"], null, null);
		if (bar.SelectedDockTab >= 0)
		{
			SelectComponent(bar.Items[bar.SelectedDockTab], SelectionTypes.Click);
			if (bar.Items[bar.SelectedDockTab] is DockContainerItem { Control: not null } dockContainerItem)
			{
				componentChangeService?.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				dockContainerItem.Control.BringToFront();
				componentChangeService?.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
			}
		}
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		Bar bar = ((ControlDesigner)this).Control as Bar;
		if (e.Component is Control)
		{
			IComponent? component = e.Component;
			BaseItem controlItem = GetControlItem((Control)((component is Control) ? component : null));
			if (controlItem != null && controlItem.Parent != null && controlItem.Parent.SubItems.Contains(controlItem))
			{
				if (controlItem is DockContainerItem)
				{
					((DockContainerItem)controlItem).Control = null;
				}
				else if (controlItem is ControlContainerItem)
				{
					((ControlContainerItem)controlItem).Control = null;
				}
				if (controlItem.Parent != null)
				{
					controlItem.Parent.SubItems.Remove(controlItem);
				}
				DestroySubItems(controlItem);
				RecalcLayout();
				if (bar != null && bar.Items.Count > 0)
				{
					SelectDockTab(0);
				}
			}
		}
		else if (!m_InternalRemoving && bar != null && e.Component is DockContainerItem)
		{
			bar.Items.Contains((BaseItem)e.Component);
		}
		base.OtherComponentRemoving(sender, e);
	}

	protected override bool OnMouseDown(ref Message m, MouseButtons mb)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Bar bar = ((ControlDesigner)this).Control as Bar;
		if (!IsDockableWindow || bar == null)
		{
			return base.OnMouseDown(ref m, mb);
		}
		Point mousePosition = Control.MousePosition;
		int tabAt = GetTabAt(mousePosition.X, mousePosition.Y);
		if (tabAt >= 0 && ((Message)(ref m)).Msg == 516)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null && selectionService.PrimarySelection != bar.Items[tabAt])
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(bar.Items[tabAt]);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
				SelectDockTab(tabAt);
				((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
				return true;
			}
		}
		return base.OnMouseDown(ref m, mb);
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		Bar bar = ((ControlDesigner)this).Control as Bar;
		m_DragDrop = false;
		if (bar != null && IsDockableWindow)
		{
			m_MouseDownSelectedTabIndex = GetTabAt(x, y);
			if (m_MouseDownSelectedTabIndex != -1)
			{
				if (bar.SelectedDockTab != m_MouseDownSelectedTabIndex)
				{
					SelectDockTab(m_MouseDownSelectedTabIndex);
				}
				m_DragDrop = true;
			}
			else if (IsInTabSystemBox(x, y))
			{
				MouseDownTabSystemBox(x, y);
			}
			else if (IsCaptionGrabHandle(bar))
			{
				Point pt = ((Control)bar).PointToClient(new Point(x, y));
				if (bar.GrabHandleRect.Contains(pt))
				{
					m_DragDrop = true;
				}
			}
		}
		else if (bar != null && IsDockableToolbar && bar.GrabHandleStyle != 0)
		{
			Point pt2 = ((Control)bar).PointToClient(new Point(x, y));
			if (bar.GrabHandleRect.Contains(pt2))
			{
				m_DragDrop = true;
			}
		}
		base.OnMouseDragBegin(x, y);
		if (m_MouseDownSelectedTabIndex != -1)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null && selectionService.PrimarySelection != bar.Items[m_MouseDownSelectedTabIndex])
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(bar.Items[m_MouseDownSelectedTabIndex]);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
			}
		}
		if (m_DragDrop)
		{
			((Control)bar).Capture = true;
			if (IsDocumentDock)
			{
				WinApi.RECT r = new WinApi.RECT(0, 0, 0, 0);
				WinApi.GetWindowRect(((Control)bar).Parent.Handle, ref r);
				Rectangle clip = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
				Cursor.Clip = clip;
			}
		}
	}

	private bool IsCaptionGrabHandle(Bar bar)
	{
		if (bar.GrabHandleStyle != eGrabHandleStyle.Caption)
		{
			return bar.GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane;
		}
		return true;
	}

	protected override void OnMouseDragMove(int x, int y)
	{
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		if ((!IsDockableWindow && !IsDockableToolbar) || !m_DragDrop)
		{
			base.OnMouseDragMove(x, y);
			return;
		}
		Point point = new Point(x, y);
		if (!(((ControlDesigner)this).Control is Bar bar))
		{
			return;
		}
		if (IsDockableToolbar)
		{
			IOwnerBarSupport ownerBarSupport = bar.Owner as IOwnerBarSupport;
			m_DockInfo = ownerBarSupport.GetDockInfo(bar, point.X, point.Y);
			if (m_DockInfo.objDockSite != null)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				Control val = null;
				bool flag = false;
				if (m_DockInfo.objDockSite != ((Control)bar).Parent)
				{
					val = ((Control)bar).Parent;
					componentChangeService.OnComponentChanging(((Control)bar).Parent, null);
					componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["DockSide"]);
				}
				componentChangeService.OnComponentChanging(m_DockInfo.objDockSite, null);
				if (m_DockInfo.DockOffset != bar.DockOffset)
				{
					flag = true;
					componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["DockOffset"]);
					componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["Location"]);
				}
				bar.DockingHandler(m_DockInfo, point);
				if (val != null)
				{
					componentChangeService.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["DockSide"], null, null);
					componentChangeService.OnComponentChanged(val, null, null, null);
				}
				componentChangeService.OnComponentChanged(m_DockInfo.objDockSite, null, null, null);
				if (flag)
				{
					componentChangeService.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["DockOffset"], null, null);
					componentChangeService.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["Location"], null, null);
				}
			}
			((Control)bar).Refresh();
			return;
		}
		Point pt = Point.Empty;
		if (bar.DockTabControl != null)
		{
			pt = ((Control)bar.DockTabControl).PointToClient(point);
		}
		if (bar.DockTabControl != null && ((Control)bar.DockTabControl).ClientRectangle.Contains(pt))
		{
			if (m_IsDocking)
			{
				EndBarOwnerDocking(bar);
				m_IsDocking = false;
				m_DockInfo = new DockSiteInfo();
			}
			MouseEventArgs e = new MouseEventArgs((MouseButtons)1048576, 0, pt.X, pt.Y, 0);
			bar.DockTabControl.InternalOnMouseMove(e);
			return;
		}
		m_IsDocking = true;
		IOwnerBarSupport ownerBarSupport2 = bar.Owner as IOwnerBarSupport;
		m_DockInfo = ownerBarSupport2.GetDockInfo(bar, point.X, point.Y);
		if (m_DockInfo.objDockSite == null)
		{
			if (m_OutlineForm != null)
			{
				((Control)m_OutlineForm).Hide();
			}
			return;
		}
		Rectangle barDockRectangle = m_DockInfo.objDockSite.GetBarDockRectangle(bar, ref m_DockInfo);
		if (!barDockRectangle.IsEmpty)
		{
			if (m_OutlineForm == null)
			{
				m_OutlineForm = bar.CreateOutlineForm();
			}
			WinApi.SetWindowPos(((Control)m_OutlineForm).Handle, 0, barDockRectangle.X, barDockRectangle.Y, barDockRectangle.Width, barDockRectangle.Height, 80);
		}
		else if (m_OutlineForm != null)
		{
			((Control)m_OutlineForm).Hide();
		}
	}

	protected override void OnMouseDragEnd(bool cancel)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Invalid comparison between Unknown and I4
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Invalid comparison between Unknown and I4
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Invalid comparison between Unknown and I4
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Invalid comparison between Unknown and I4
		if (!IsDockableWindow)
		{
			base.OnMouseDragEnd(cancel);
			return;
		}
		((ControlDesigner)this).Control.Capture = false;
		Cursor.Clip = Rectangle.Empty;
		Bar bar = ((ControlDesigner)this).Control as Bar;
		if (cancel || bar == null || !m_DragDrop)
		{
			base.OnMouseDragEnd(cancel);
			return;
		}
		m_DragDrop = false;
		if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			EndBarOwnerDocking(bar);
			m_IsDocking = false;
			m_MouseDownSelectedTabIndex = -1;
			base.OnMouseDragEnd(cancel);
			return;
		}
		if (m_IsDocking)
		{
			EndBarOwnerDocking(bar);
			Bar mouseOverBar = m_DockInfo.MouseOverBar;
			if ((m_DockInfo.MouseOverDockSide != 0 && m_DockInfo.MouseOverDockSide != eDockSide.Document && (mouseOverBar != bar || (m_MouseDownSelectedTabIndex != -1 && bar.VisibleItemCount > 1))) || ((int)m_DockInfo.DockSide != 0 && m_DockInfo.MouseOverDockSide != eDockSide.Document))
			{
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("DotNetBar Docking");
				IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
				try
				{
					Bar bar2 = null;
					DockSite objDockSite = m_DockInfo.objDockSite;
					DockSite dockSite = ((Control)bar).Parent as DockSite;
					if (m_MouseDownSelectedTabIndex != -1 && bar.VisibleItemCount > 1)
					{
						Bar bar3 = designerHost.CreateComponent(typeof(Bar)) as Bar;
						bar2 = BarFunctions.CreateDuplicateDockBar(bar, bar3);
					}
					else
					{
						bar2 = bar;
					}
					componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
					if (m_MouseDownSelectedTabIndex != -1 && bar.VisibleItemCount > 1)
					{
						DockContainerItem item = bar.Items[bar.SelectedDockTab] as DockContainerItem;
						componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
						componentChangeService.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
						bar.Items.Remove(item);
						componentChangeService.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Items"], null, null);
						componentChangeService.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
						componentChangeService.OnComponentChanging(bar2, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
						componentChangeService.OnComponentChanging(bar2, TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
						bar2.Items.Add(item);
						componentChangeService.OnComponentChanged(bar2, TypeDescriptor.GetProperties(typeof(Bar))["Items"], null, null);
						componentChangeService.OnComponentChanged(bar2, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
					}
					componentChangeService.OnComponentChanging(objDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
					componentChangeService.OnComponentChanging(bar2, null);
					if (mouseOverBar != null)
					{
						componentChangeService.OnComponentChanging(mouseOverBar, null);
					}
					componentChangeService.OnComponentChanging(objDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
					if (objDockSite != dockSite && dockSite != null)
					{
						componentChangeService.OnComponentChanging(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
						componentChangeService.OnComponentChanging(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
					}
					m_DockInfo.MouseOverBar = mouseOverBar;
					bar2.DockDocumentManager(m_DockInfo);
					if (((Control)objDockSite).Width == 0 && ((int)((Control)objDockSite).Dock == 3 || (int)((Control)objDockSite).Dock == 4))
					{
						((Control)objDockSite).Width = bar2.GetBarDockedSize(eOrientation.Vertical);
					}
					else if (((Control)objDockSite).Height == 0 && ((int)((Control)objDockSite).Dock == 1 || (int)((Control)objDockSite).Dock == 2))
					{
						((Control)objDockSite).Height = bar2.GetBarDockedSize(eOrientation.Horizontal);
					}
					if (objDockSite != dockSite && dockSite != null)
					{
						componentChangeService.OnComponentChanged(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"], null, null);
						componentChangeService.OnComponentChanged(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"], null, null);
					}
					componentChangeService.OnComponentChanged(objDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"], null, null);
					if (mouseOverBar != null)
					{
						componentChangeService.OnComponentChanged(mouseOverBar, null, null, null);
					}
					componentChangeService.OnComponentChanged(bar2, null, null, null);
					componentChangeService.OnComponentChanged(objDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"], null, null);
					Form val = ((Control)bar2).FindForm();
					if (val != null)
					{
						((Control)val).Refresh();
					}
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
			else if (m_DockInfo.MouseOverDockSide == eDockSide.Document && bar != mouseOverBar && designerHost.GetDesigner((IComponent)mouseOverBar) is BarDesigner barDesigner)
			{
				barDesigner.DelayedDockTabs(bar, m_MouseDownSelectedTabIndex);
			}
			m_DockInfo = new DockSiteInfo();
		}
		else if (m_MouseDownSelectedTabIndex != -1 && m_MouseDownSelectedTabIndex != bar.SelectedDockTab)
		{
			IComponentChangeService componentChangeService2 = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService2 != null)
			{
				componentChangeService2.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Items"], null, null);
				componentChangeService2.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"], null, null);
				componentChangeService2.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
			}
		}
		m_IsDocking = false;
		m_MouseDownSelectedTabIndex = -1;
		base.OnMouseDragEnd(cancel);
	}

	internal void DelayedDockTabs(Bar bar, int tabIndex)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (m_TimerDelayRemove == null)
		{
			m_DelayDockBar = bar;
			m_DelayDockTabIndex = tabIndex;
			m_TimerDelayRemove = new Timer();
			m_TimerDelayRemove.Tick += TimerTickDelayRemove;
			m_TimerDelayRemove.Interval = 200;
			m_TimerDelayRemove.Enabled = true;
			m_TimerDelayRemove.Start();
		}
	}

	private void TimerTickDelayRemove(object sender, EventArgs e)
	{
		m_TimerDelayRemove.Stop();
		m_TimerDelayRemove.Enabled = false;
		((Component)(object)m_TimerDelayRemove).Dispose();
		m_TimerDelayRemove = null;
		DockTabs(m_DelayDockBar, m_DelayDockTabIndex, ((ControlDesigner)this).Control as Bar);
		m_DelayDockBar = null;
		m_DelayDockTabIndex = -1;
	}

	private void DockTabs(Bar sourceBar, int selectedTabIndex, Bar targetBar)
	{
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("DotNetBar Docking");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		try
		{
			DockContainerItem[] array = null;
			if (selectedTabIndex != -1)
			{
				array = new DockContainerItem[1] { sourceBar.Items[sourceBar.SelectedDockTab] as DockContainerItem };
			}
			else
			{
				array = new DockContainerItem[sourceBar.Items.Count];
				sourceBar.Items.CopyTo(array, 0);
			}
			componentChangeService.OnComponentChanging(sourceBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
			componentChangeService.OnComponentChanging(sourceBar, TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
			sourceBar.Items.RemoveRange(array);
			componentChangeService.OnComponentChanged(sourceBar, TypeDescriptor.GetProperties(typeof(Bar))["Items"], null, null);
			componentChangeService.OnComponentChanged(sourceBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
			componentChangeService.OnComponentChanging(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
			componentChangeService.OnComponentChanging(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
			targetBar.Items.AddRange(array);
			componentChangeService.OnComponentChanged(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Items"], null, null);
			componentChangeService.OnComponentChanged(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
			if (sourceBar.Items.Count == 0)
			{
				DockSite dockSite = ((Control)sourceBar).Parent as DockSite;
				componentChangeService.OnComponentChanging(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
				componentChangeService.OnComponentChanging(sourceBar, null);
				dockSite.GetDocumentUIManager().UnDock(sourceBar, removeFromContainer: false);
				componentChangeService.OnComponentChanged(sourceBar, null, null, null);
				componentChangeService.OnComponentChanged(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"], null, null);
				designerHost.DestroyComponent((IComponent)sourceBar);
			}
			if (targetBar != null && targetBar.SelectedDockTab >= 0 && targetBar.Items[targetBar.SelectedDockTab] is DockContainerItem { Control: not null } dockContainerItem)
			{
				componentChangeService.OnComponentChanged(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
				dockContainerItem.Control.BringToFront();
				componentChangeService.OnComponentChanged(targetBar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
			}
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

	private void EndBarOwnerDocking(Bar bar)
	{
		if (bar.Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.DockComplete();
		}
		if (m_OutlineForm != null)
		{
			((Control)m_OutlineForm).Hide();
			((Component)(object)m_OutlineForm).Dispose();
			m_OutlineForm = null;
		}
	}

	protected override IOwner GetIOwner()
	{
		Bar bar = ((ControlDesigner)this).Control as Bar;
		if (bar.Owner is IOwner)
		{
			return bar.Owner as IOwner;
		}
		return base.GetIOwner();
	}

	protected override IOwnerMenuSupport GetIOwnerMenuSupport()
	{
		Bar bar = ((ControlDesigner)this).Control as Bar;
		if (bar.Owner is IOwnerMenuSupport)
		{
			return bar.Owner as IOwnerMenuSupport;
		}
		return base.GetIOwnerMenuSupport();
	}

	private int GetTabAt(int x, int y)
	{
		if (!(((ControlDesigner)this).Control is Bar bar))
		{
			return -1;
		}
		if (IsDockableWindow && bar.DockTabControl != null)
		{
			Point pt = ((Control)bar.DockTabControl).PointToClient(new Point(x, y));
			if (bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(pt))
			{
				return -1;
			}
			TabItem tabItem = bar.DockTabControl.HitTest(pt.X, pt.Y);
			if (tabItem != null)
			{
				return bar.Items.IndexOf(tabItem.AttachedItem);
			}
		}
		return -1;
	}

	private bool IsInTabSystemBox(int x, int y)
	{
		if (!(((ControlDesigner)this).Control is Bar bar))
		{
			return false;
		}
		if (IsDockableWindow && bar.DockTabControl != null)
		{
			Point pt = ((Control)bar.DockTabControl).PointToClient(new Point(x, y));
			if (bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(pt))
			{
				return true;
			}
		}
		return false;
	}

	private void MouseDownTabSystemBox(int x, int y)
	{
		if (!(((ControlDesigner)this).Control is Bar bar) || !IsDockableWindow || bar.DockTabControl == null)
		{
			return;
		}
		Point pt = ((Control)bar.DockTabControl).PointToClient(new Point(x, y));
		if (bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(pt))
		{
			if (bar.DockTabControl._TabSystemBox.ForwardEnabled && bar.DockTabControl._TabSystemBox.ForwardRect.Contains(pt))
			{
				bar.DockTabControl.ScrollForward();
			}
			else if (bar.DockTabControl._TabSystemBox.BackEnabled && bar.DockTabControl._TabSystemBox.BackRect.Contains(pt))
			{
				bar.DockTabControl.ScrollBackwards();
			}
		}
	}

	protected override void ThisComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (!m_InternalRemoving)
		{
			m_InternalRemoving = true;
			try
			{
				if (IsDockableWindow && ((ControlDesigner)this).Control is Bar bar && ((Control)bar).Parent is DockSite && ((DockSite)(object)((Control)bar).Parent).DocumentDockContainer != null)
				{
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					componentChangeService?.OnComponentChanging((DockSite)(object)((Control)bar).Parent, TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer", ignoreCase: true));
					((DockSite)(object)((Control)bar).Parent).GetDocumentUIManager().UnDock(bar, removeFromContainer: false);
					componentChangeService?.OnComponentChanged((DockSite)(object)((Control)bar).Parent, TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer", ignoreCase: true), null, null);
				}
			}
			finally
			{
				m_InternalRemoving = false;
			}
		}
		base.ThisComponentRemoving(sender, e);
	}
}
