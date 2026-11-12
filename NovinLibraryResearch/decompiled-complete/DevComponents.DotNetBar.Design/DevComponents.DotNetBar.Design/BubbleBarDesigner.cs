using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class BubbleBarDesigner : ControlDesigner
{
	private bool m_IgnoreMouseUp;

	private Point m_MouseDownPosition = Point.Empty;

	private bool m_Capture;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[2]
			{
				new DesignerVerb("Create Tab", CreateTab),
				new DesignerVerb("Create Button", CreateButton)
			};
			return new DesignerVerbCollection(value);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			if (((ControlDesigner)this).Control is BubbleBar bubbleBar)
			{
				foreach (BubbleBarTab tab in bubbleBar.Tabs)
				{
					arrayList.Add(tab);
					GetTabComponents(tab, arrayList);
				}
			}
			return arrayList;
		}
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += OnSelectionChanged;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoving;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		((ControlDesigner)this).Dispose(disposing);
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component is BubbleBar bubbleBar)
		{
			bubbleBar.ButtonBackAreaStyle.SetColorScheme(new ColorScheme(eDotNetBarStyle.Office2003));
			bubbleBar.ButtonBackAreaStyle.BackColor = Color.FromArgb(66, Color.DimGray);
			bubbleBar.ButtonBackAreaStyle.BorderColor = Color.FromArgb(180, Color.WhiteSmoke);
			bubbleBar.ButtonBackAreaStyle.BorderTop = eStyleBorderType.Solid;
			bubbleBar.ButtonBackAreaStyle.BorderTopWidth = 1;
			bubbleBar.ButtonBackAreaStyle.BorderBottom = eStyleBorderType.Solid;
			bubbleBar.ButtonBackAreaStyle.BorderBottomWidth = 1;
			bubbleBar.ButtonBackAreaStyle.BorderLeft = eStyleBorderType.Solid;
			bubbleBar.ButtonBackAreaStyle.BorderLeftWidth = 1;
			bubbleBar.ButtonBackAreaStyle.BorderRight = eStyleBorderType.Solid;
			bubbleBar.ButtonBackAreaStyle.BorderRightWidth = 1;
			bubbleBar.ButtonBackAreaStyle.PaddingBottom = 3;
			bubbleBar.ButtonBackAreaStyle.PaddingTop = 3;
			bubbleBar.ButtonBackAreaStyle.PaddingLeft = 3;
			bubbleBar.ButtonBackAreaStyle.PaddingRight = 3;
			bubbleBar.SelectedTabColors.BorderColor = Color.Black;
			bubbleBar.MouseOverTabColors.BorderColor = SystemColors.Highlight;
		}
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService == null)
		{
			return;
		}
		bool flag = ResetFocus();
		if (selectionService.PrimarySelection is BubbleBarTab)
		{
			if (((BubbleBarTab)selectionService.PrimarySelection).Parent == ((ControlDesigner)this).Control)
			{
				((BubbleBarTab)selectionService.PrimarySelection).Focus = true;
				flag = true;
			}
		}
		else if (selectionService.PrimarySelection is BubbleButton && ((BubbleButton)selectionService.PrimarySelection).Parent.Parent == ((ControlDesigner)this).Control)
		{
			((BubbleButton)selectionService.PrimarySelection).Focus = true;
			flag = true;
		}
		if (flag)
		{
			((ControlDesigner)this).Control.Refresh();
		}
	}

	private bool ResetFocus()
	{
		bool result = false;
		BubbleBar bubbleBar = ((ControlDesigner)this).Control as BubbleBar;
		foreach (BubbleBarTab tab in bubbleBar.Tabs)
		{
			if (tab.Focus)
			{
				tab.Focus = false;
				result = true;
			}
			foreach (BubbleButton button in tab.Buttons)
			{
				if (button.Focus)
				{
					button.Focus = false;
					result = true;
				}
			}
		}
		return result;
	}

	private void OnComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component == ((ComponentDesigner)this).Component)
		{
			ThisComponentRemoving(sender, e);
		}
		else if (e.Component is BubbleBarTab)
		{
			BubbleBarTab bubbleBarTab = e.Component as BubbleBarTab;
			if (bubbleBarTab.Parent == ((ControlDesigner)this).Control)
			{
				IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
				BubbleButton[] array = new BubbleButton[bubbleBarTab.Buttons.Count];
				bubbleBarTab.Buttons.CopyTo(array, 0);
				BubbleButton[] array2 = array;
				foreach (BubbleButton component in array2)
				{
					designerHost.DestroyComponent(component);
				}
				BubbleBar bubbleBar = ((ComponentDesigner)this).Component as BubbleBar;
				if (bubbleBar.Tabs.Contains(bubbleBarTab))
				{
					bubbleBar.Tabs.Remove(bubbleBarTab);
				}
				designerHost.DestroyComponent(bubbleBarTab);
				bubbleBar.RecalcLayout();
				((Control)bubbleBar).Refresh();
			}
		}
		else
		{
			if (!(e.Component is BubbleButton))
			{
				return;
			}
			BubbleButton bubbleButton = e.Component as BubbleButton;
			if (bubbleButton.GetBubbleBar() == ((ControlDesigner)this).Control)
			{
				if (bubbleButton.Parent.Buttons.Contains(bubbleButton))
				{
					bubbleButton.Parent.Buttons.Remove(bubbleButton);
				}
				IDesignerHost designerHost2 = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
				designerHost2.DestroyComponent(bubbleButton);
				BubbleBar bubbleBar2 = ((ComponentDesigner)this).Component as BubbleBar;
				bubbleBar2.RecalcLayout();
				((Control)bubbleBar2).Refresh();
			}
		}
	}

	protected virtual void ThisComponentRemoving(object sender, ComponentEventArgs e)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		BubbleBar bubbleBar = ((ComponentDesigner)this).Component as BubbleBar;
		if (designerHost == null)
		{
			return;
		}
		foreach (BubbleBarTab tab in bubbleBar.Tabs)
		{
			foreach (BubbleButton button in tab.Buttons)
			{
				designerHost.DestroyComponent(button);
			}
			designerHost.DestroyComponent(tab);
		}
	}

	private void CreateButton(object sender, EventArgs e)
	{
		if (!(((ControlDesigner)this).Control is BubbleBar bubbleBar))
		{
			return;
		}
		if (bubbleBar.SelectedTab == null)
		{
			BubbleBarTab selectedTab = CreateTab();
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true));
			bubbleBar.SelectedTab = selectedTab;
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true), null, null);
		}
		if (bubbleBar.SelectedTab == null)
		{
			return;
		}
		BubbleButton bubbleButton = CreateButton(bubbleBar.SelectedTab);
		if (bubbleButton != null)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(bubbleButton);
				selectionService.SetSelectedComponents(arrayList);
			}
		}
	}

	private void CreateTab(object sender, EventArgs e)
	{
		if (!(((ControlDesigner)this).Control is BubbleBar bubbleBar))
		{
			return;
		}
		BubbleBarTab bubbleBarTab = CreateTab();
		if (bubbleBarTab != null)
		{
			if (bubbleBar.SelectedTab != bubbleBarTab)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true));
				bubbleBar.SelectedTab = bubbleBarTab;
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true), null, null);
			}
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(bubbleBarTab);
				selectionService.SetSelectedComponents(arrayList);
			}
		}
	}

	private BubbleBarTab CreateTab()
	{
		if (!(((ControlDesigner)this).Control is BubbleBar bubbleBar))
		{
			return null;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("Tabs", ignoreCase: true));
		if (!(designerHost.CreateComponent(typeof(BubbleBarTab)) is BubbleBarTab bubbleBarTab))
		{
			return null;
		}
		bubbleBarTab.Text = bubbleBarTab.Name;
		eTabItemColor predefinedColor = eTabItemColor.Blue;
		if (bubbleBar.Tabs.Count > 0)
		{
			int num = bubbleBar.Tabs.Count + 1;
			Type typeFromHandle = typeof(eTabItemColor);
			FieldInfo[] fields = typeFromHandle.GetFields(BindingFlags.Static | BindingFlags.Public);
			int num2 = fields.Length;
			while (num > num2)
			{
				num -= num2;
			}
			if (num == 0)
			{
				num++;
			}
			predefinedColor = (eTabItemColor)Enum.Parse(typeof(eTabItemColor), fields[num].Name);
		}
		bubbleBarTab.PredefinedColor = predefinedColor;
		bubbleBar.Tabs.Add(bubbleBarTab);
		componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("Tabs", ignoreCase: true), null, null);
		return bubbleBarTab;
	}

	private BubbleButton CreateButton(BubbleBarTab tab)
	{
		if (!(((ControlDesigner)this).Control is BubbleBar))
		{
			return null;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(tab).Find("Buttons", ignoreCase: true));
		if (!(designerHost.CreateComponent(typeof(BubbleButton)) is BubbleButton bubbleButton))
		{
			return null;
		}
		bubbleButton.Image = (Image)(object)Helpers.LoadBitmap("SystemImages.Note24.png");
		bubbleButton.ImageLarge = (Image)(object)Helpers.LoadBitmap("SystemImages.Note64.png");
		tab.Buttons.Add(bubbleButton);
		componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(tab).Find("Buttons", ignoreCase: true), null, null);
		return bubbleButton;
	}

	private void GetTabComponents(BubbleBarTab tab, ArrayList c)
	{
		foreach (BubbleButton button in tab.Buttons)
		{
			c.Add(button);
		}
	}

	protected override void OnSetCursor()
	{
		BubbleBar bubbleBar = ((ControlDesigner)this).Control as BubbleBar;
		Point p = ((Control)bubbleBar).PointToClient(Control.MousePosition);
		BubbleButton buttonAt = bubbleBar.GetButtonAt(p);
		if (buttonAt != null)
		{
			Cursor.Current = Cursors.Default;
			return;
		}
		BubbleBarTab tabAt = bubbleBar.GetTabAt(p);
		if (tabAt != null)
		{
			Cursor.Current = Cursors.Default;
		}
		else
		{
			((ControlDesigner)this).OnSetCursor();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		switch (((Message)(ref m)).Msg)
		{
		case 513:
		case 516:
			if (OnMouseDown(ref m))
			{
				return;
			}
			break;
		case 514:
		case 517:
			if (OnMouseUp(ref m))
			{
				return;
			}
			break;
		case 512:
			if (OnMouseMove(ref m))
			{
				return;
			}
			break;
		case 515:
			if (OnMouseDoubleClick(m))
			{
				return;
			}
			break;
		}
		((ControlDesigner)this).WndProc(ref m);
	}

	private bool OnMouseDoubleClick(Message m)
	{
		bool result = false;
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && selectionService.PrimarySelection is BubbleButton && ((BubbleButton)selectionService.PrimarySelection).GetBubbleBar() == ((ControlDesigner)this).Control)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IDesigner designer = designerHost.GetDesigner(selectionService.PrimarySelection as IComponent);
				if (designer != null)
				{
					designer.DoDefaultAction();
					result = true;
				}
			}
		}
		return result;
	}

	protected virtual bool OnMouseDown(ref Message m)
	{
		if (!(((ControlDesigner)this).Control is BubbleBar bubbleBar))
		{
			return false;
		}
		Point p = (m_MouseDownPosition = ((Control)bubbleBar).PointToClient(Control.MousePosition));
		BubbleButton buttonAt = bubbleBar.GetButtonAt(p);
		if (buttonAt != null)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(buttonAt);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
				m_IgnoreMouseUp = true;
				if (((Message)(ref m)).Msg == 516)
				{
					((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
				}
				return true;
			}
		}
		BubbleBarTab tabAt = bubbleBar.GetTabAt(p);
		if (tabAt != null)
		{
			ISelectionService selectionService2 = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService2 != null && selectionService2.PrimarySelection != tabAt)
			{
				ArrayList arrayList2 = new ArrayList(1);
				arrayList2.Add(tabAt);
				selectionService2.SetSelectedComponents(arrayList2, SelectionTypes.Click);
				if (bubbleBar.SelectedTab != tabAt)
				{
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true));
					bubbleBar.SelectedTab = tabAt;
					componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(bubbleBar).Find("SelectedTab", ignoreCase: true), null, null);
				}
				m_IgnoreMouseUp = true;
				if (((Message)(ref m)).Msg != 516)
				{
					return true;
				}
			}
			if (((Message)(ref m)).Msg == 516)
			{
				((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
				return true;
			}
		}
		return false;
	}

	protected virtual bool OnMouseMove(ref Message m)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		BubbleBar bubbleBar = ((ControlDesigner)this).Control as BubbleBar;
		Point point = ((Control)bubbleBar).PointToClient(Control.MousePosition);
		if ((int)Control.MouseButtons == 1048576)
		{
			if (bubbleBar.DragInProgress)
			{
				bubbleBar.DragMouseMove(point);
				return true;
			}
			if ((!m_MouseDownPosition.IsEmpty && Math.Abs(m_MouseDownPosition.X - point.X) >= 2) || Math.Abs(m_MouseDownPosition.Y - point.Y) >= 2)
			{
				BubbleBarTab tabAt = bubbleBar.GetTabAt(point);
				if (tabAt != null)
				{
					bubbleBar.StartDrag(tabAt);
					Control val = Control.FromHandle(((Message)(ref m)).HWnd);
					if (val != null)
					{
						m_Capture = true;
						val.Capture = true;
					}
					return true;
				}
				BubbleButton buttonAt = bubbleBar.GetButtonAt(point);
				if (buttonAt != null)
				{
					bubbleBar.StartDrag(buttonAt);
					Control val2 = Control.FromHandle(((Message)(ref m)).HWnd);
					if (val2 != null)
					{
						m_Capture = true;
						val2.Capture = true;
					}
					return true;
				}
				m_MouseDownPosition = Point.Empty;
			}
		}
		BubbleBarTab tabAt2 = bubbleBar.GetTabAt(point);
		if (tabAt2 != null)
		{
			bubbleBar.SetMouseOverTab(tabAt2);
		}
		else
		{
			bubbleBar.SetMouseOverTab(null);
		}
		return false;
	}

	private bool OnMouseUp(ref Message m)
	{
		if (m_Capture)
		{
			Control val = Control.FromHandle(((Message)(ref m)).HWnd);
			if (val != null)
			{
				val.Capture = false;
			}
			m_Capture = false;
		}
		if (((ControlDesigner)this).Control is BubbleBar { DragInProgress: not false } bubbleBar)
		{
			Point mousePosition = ((Control)bubbleBar).PointToClient(Control.MousePosition);
			bubbleBar.DragMouseUp(mousePosition);
			BubbleBarTab selectedTab = bubbleBar.SelectedTab;
			if (selectedTab != null)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(selectedTab).Find("Buttons", ignoreCase: true));
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(selectedTab).Find("Buttons", ignoreCase: true), null, null);
			}
		}
		if (m_IgnoreMouseUp)
		{
			m_IgnoreMouseUp = false;
			return true;
		}
		return false;
	}
}
