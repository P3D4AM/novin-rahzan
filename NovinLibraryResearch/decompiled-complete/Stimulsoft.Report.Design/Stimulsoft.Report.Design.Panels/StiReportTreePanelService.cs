using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Design.Panels;

[StiServiceBitmap(typeof(StiReportTreePanelService), "Stimulsoft.Report.Design.Images.Panels.ReportTree.png")]
public class StiReportTreePanelService : StiPanelService
{
	private class FindHelper
	{
		public string TextFind { get; set; }

		public int Count { get; set; } = -1;


		public int CurrentIndex { get; set; }

		public void Clear()
		{
			TextFind = null;
			Count = -1;
		}
	}

	private FindHelper findHelper = new FindHelper();

	private int imageIndex = 3;

	private bool locked;

	private bool locked2;

	private TreeNode selectedNode;

	private StiTreeView treeViewReportTree;

	private StiButtonEdit buttonFindComponent;

	private Hashtable componentToImage;

	private ToolTip toolTip;

	private ButtonItem itemContext;

	private ButtonItem itemExpandAll;

	private ButtonItem itemCollapseAll;

	private StiOperationCheckBox buttonComponentState;

	private StiOperationCheckBox buttonEventState;

	public override string ServiceName => StiLocalization.Get("Panels", "ReportTree");

	public static Image EventImage => StiReportDesignImages.Actions.Events();

	public override eShortcut Shortcut => eShortcut.CtrlShiftL;

	public override DockStyle DockStyle => (DockStyle)4;

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		ReportTreeBuild();
	}

	private void TreeViewReportTree_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (e.Node.Tag == null || locked || locked2)
		{
			return;
		}
		locked = true;
		Designer.LockTabControlRefresh = true;
		if ((Control.ModifierKeys & 0x20000) == 0 && (Control.ModifierKeys & 0x10000) == 0)
		{
			Report.ResetSelection();
		}
		if (e.Node.Tag is IStiSelect stiSelect)
		{
			stiSelect.Select();
			StiPage stiPage = stiSelect as StiPage;
			if (stiPage == null && stiSelect is StiComponent)
			{
				stiPage = ((StiComponent)stiSelect).Page;
			}
			if (stiPage != null && Report.GetCurrentPage() != stiPage)
			{
				Designer.SelectTabPage(stiPage);
				((Control)treeViewReportTree).Focus();
				Report.CurrentPage = Report.Pages.IndexOf(stiPage);
			}
		}
		((Control)Designer).Refresh();
		Designer.InvokeSelectedObjectsChanged();
		Designer.LockTabControlRefresh = false;
		locked = false;
	}

	private void TreeViewReportTree_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		object item = e.Item;
		TreeNode val = (TreeNode)((item is TreeNode) ? item : null);
		if (val != null && val.Parent != null && val.Tag is StiComponent { Linked: false, Inherited: false })
		{
			((Control)treeViewReportTree).DoDragDrop(e.Item, (DragDropEffects)2);
		}
	}

	private void TreeViewReportTree_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void TreeViewReportTree_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode nodeAt = ((TreeView)treeViewReportTree).GetNodeAt(((Control)treeViewReportTree).PointToClient(Cursor.Position));
		if (val == null || nodeAt == null)
		{
			return;
		}
		StiComponent stiComponent = val.Tag as StiComponent;
		StiComponent stiComponent2 = nodeAt.Tag as StiComponent;
		if (stiComponent != stiComponent2)
		{
			if (stiComponent.CanContainIn(stiComponent2))
			{
				((TreeView)treeViewReportTree).SelectedNode = nodeAt;
				return;
			}
			locked2 = true;
			((TreeView)treeViewReportTree).SelectedNode = nodeAt;
			locked2 = false;
		}
	}

	private void TreeViewReportTree_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode nodeAt = ((TreeView)treeViewReportTree).GetNodeAt(((Control)treeViewReportTree).PointToClient(Cursor.Position));
		if (nodeAt == null || val == null)
		{
			return;
		}
		StiComponent stiComponent = val.Tag as StiComponent;
		StiComponent stiComponent2 = nodeAt.Tag as StiComponent;
		if (stiComponent is StiContainer stiContainer)
		{
			StiComponentsCollection components = stiContainer.GetComponents();
			if (components.IndexOf(stiComponent2) != -1)
			{
				return;
			}
		}
		if (stiComponent != stiComponent2 && stiComponent.CanContainIn(stiComponent2) && stiComponent.Parent != stiComponent2)
		{
			bool flag = stiComponent.Page != stiComponent2.Page && stiComponent is StiContainer;
			stiComponent.Parent.Components.Remove(stiComponent);
			((StiContainer)stiComponent2).Components.Add(stiComponent);
			if (flag)
			{
				foreach (StiComponent component in (stiComponent as StiContainer).GetComponents())
				{
					component.Page = stiComponent2.Page;
				}
			}
			if (stiComponent.Left < 0.0 || stiComponent.Left > stiComponent2.Width)
			{
				stiComponent.Left = 0.0;
			}
			if (stiComponent.Top < 0.0 || stiComponent.Top > stiComponent2.Height)
			{
				stiComponent.Top = 0.0;
			}
			Designer.Report.ResetSelection();
			stiComponent.Select();
			((Control)Designer).Refresh();
		}
		else
		{
			if (stiComponent == stiComponent2 || stiComponent2 == null || stiComponent2 is StiContainer || !stiComponent.CanContainIn(stiComponent2.Parent))
			{
				return;
			}
			stiComponent.Parent.Components.Remove(stiComponent);
			int num = stiComponent2.Parent.Components.IndexOf(stiComponent2);
			if (num == -1)
			{
				return;
			}
			stiComponent2.Parent.Components.Insert(num, stiComponent);
			if (stiComponent.Parent != stiComponent2.Parent)
			{
				if (stiComponent.Left < 0.0 || stiComponent.Left > stiComponent2.Width)
				{
					stiComponent.Left = 0.0;
				}
				if (stiComponent.Top < 0.0 || stiComponent.Top > stiComponent2.Height)
				{
					stiComponent.Top = 0.0;
				}
			}
			Designer.Report.ResetSelection();
			stiComponent.Select();
			((Control)Designer).Refresh();
		}
	}

	private void TreeViewReportTree_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode != 46)
		{
			return;
		}
		Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditDelete") + " " + StiLocalization.Get("Report", "Components"), onlyCurrentPage: true);
		if (((TreeView)treeViewReportTree).SelectedNode != null)
		{
			TreeNode nextVisibleNode = ((TreeView)treeViewReportTree).SelectedNode.NextVisibleNode;
			Designer.Report.GetCurrentPage().RemoveAllSelected();
			if (nextVisibleNode != null && nextVisibleNode.Tag is StiComponent)
			{
				((StiComponent)nextVisibleNode.Tag).Select();
			}
			((Control)Designer).Refresh();
		}
	}

	private void TreeViewReportTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		TreeNode node = e.Node;
		StiComponent stiComponent = node.Tag as StiComponent;
		if (node.Nodes.Count == 1 && node.Nodes[0].Text == "##TEMP##")
		{
			ReportTreeEventsBuild(stiComponent.GetEvents(), node);
			if (stiComponent is StiContainer)
			{
				ReportTreeComponentBuild(stiComponent as StiContainer, node);
			}
			node.Nodes.RemoveAt(0);
		}
	}

	private void TreeViewReportTree_DoubleClick(object sender, EventArgs e)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		TreeNode val = ((TreeView)treeViewReportTree).SelectedNode;
		if (!(val.Tag is StiEvent))
		{
			return;
		}
		StiEvent stiEvent = (StiEvent)val.Tag;
		if (stiEvent.Script.StartsWith("<xml xmlns=\"https://developers.google.com/blockly/xml\">"))
		{
			if ((int)StiEventEditor.EditBlocklyEvent(Designer.Report, stiEvent) != 1)
			{
				return;
			}
		}
		else if ((int)StiEventEditor.EditEventSwitch(Designer.Report, stiEvent) != 1)
		{
			return;
		}
		if (string.IsNullOrEmpty(stiEvent.Script))
		{
			TreeNode parent = val.Parent;
			if (parent != null)
			{
				((TreeView)treeViewReportTree).SelectedNode = parent;
				parent.Nodes.Remove(val);
			}
		}
	}

	private void ItemExpandAll_Click(object sender, EventArgs e)
	{
		((TreeView)treeViewReportTree).ExpandAll();
	}

	private void ItemCollapseAll_Click(object sender, EventArgs e)
	{
		((TreeView)treeViewReportTree).CollapseAll();
	}

	private void ButtonFindComponent_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyData == 13)
		{
			e.Handled = true;
			ProcessFindComponent();
		}
	}

	private void ButtonFindComponent_Click(object sender, EventArgs e)
	{
		ProcessFindComponent();
	}

	private void ButtonState_CheckedChanged(object sender, EventArgs e)
	{
		ReportTreeBuild();
	}

	public override void Localize()
	{
		base.Localize();
		if (itemExpandAll != null)
		{
			itemExpandAll.Text = StiLocalization.Get("Report", "ExpandAll");
		}
		if (itemCollapseAll != null)
		{
			itemCollapseAll.Text = StiLocalization.Get("Report", "CollapseAll");
		}
		if (buttonComponentState != null)
		{
			toolTip.SetToolTip((Control)(object)buttonComponentState, LocComponentState());
		}
		if (buttonEventState != null)
		{
			toolTip.SetToolTip((Control)(object)buttonEventState, LocEventState());
		}
	}

	private static string LocEventState()
	{
		return Funcs.ToProperCase(Loc.Get("MainMenu", "menuViewQuickInfoShowEvents"));
	}

	private static string LocComponentState()
	{
		return Funcs.ToProperCase(string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.GetReport("Components")).Replace("'", ""));
	}

	private void ProcessFindComponent()
	{
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)buttonFindComponent).Text.Length > 0 && Report != null && treeViewReportTree != null)
		{
			if (findHelper.TextFind != ((Control)buttonFindComponent).Text.ToLowerInvariant())
			{
				findHelper.Clear();
			}
			if (string.IsNullOrEmpty(findHelper.TextFind))
			{
				findHelper.TextFind = ((Control)buttonFindComponent).Text.ToLowerInvariant();
			}
			findHelper.CurrentIndex = 0;
			foreach (StiPage page in Report.Pages)
			{
				if (page.Name.ToLowerInvariant().Contains(findHelper.TextFind))
				{
					List<StiComponent> list = new List<StiComponent> { page };
					findHelper.CurrentIndex++;
					if (findHelper.CurrentIndex > findHelper.Count)
					{
						SelectComponent(list);
						return;
					}
					continue;
				}
				for (int i = 0; i < page.Components.Count; i++)
				{
					if (page.Components[i].Name.ToLowerInvariant().Contains(findHelper.TextFind))
					{
						List<StiComponent> list2 = new List<StiComponent>
						{
							page.Components[i],
							page
						};
						findHelper.CurrentIndex++;
						if (findHelper.CurrentIndex > findHelper.Count)
						{
							SelectComponent(list2);
							return;
						}
					}
					else if (page.Components[i] is StiContainer)
					{
						List<StiComponent> list3 = FindComponent(page.Components[i] as StiContainer);
						if (list3 != null)
						{
							list3.Add(page.Components[i]);
							list3.Add(page);
							SelectComponent(list3);
							return;
						}
					}
				}
			}
		}
		MessageBox.Show(string.Format(StiLocalization.Get("DesignerFx", "TextNotFound"), findHelper.TextFind), StiLocalization.Get("Editor", "titleFind"), (MessageBoxButtons)0);
		findHelper.Clear();
	}

	private List<StiComponent> FindComponent(StiContainer container)
	{
		for (int i = 0; i < container.Components.Count; i++)
		{
			if (container.Components[i].Name.ToLower().Contains(findHelper.TextFind))
			{
				findHelper.CurrentIndex++;
				if (findHelper.CurrentIndex > findHelper.Count)
				{
					return new List<StiComponent> { container.Components[i] };
				}
			}
			else if (container.Components[i] is StiContainer)
			{
				List<StiComponent> list = FindComponent(container.Components[i] as StiContainer);
				if (list != null)
				{
					list.Add(container.Components[i]);
					list.Add(container);
					return list;
				}
			}
		}
		return null;
	}

	private void SelectComponent(List<StiComponent> list)
	{
		TreeNode val = ((TreeView)treeViewReportTree).Nodes[0];
		val.Expand();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = list[num];
			for (int i = 0; i < val.Nodes.Count; i++)
			{
				if (val.Nodes[i].Tag == stiComponent)
				{
					if (num == 0)
					{
						((Control)treeViewReportTree).Focus();
						((TreeView)treeViewReportTree).SelectedNode = val.Nodes[i];
						findHelper.Count = ((findHelper.Count == -1) ? 1 : (findHelper.Count + 1));
						return;
					}
					val.Nodes[i].Expand();
					val = val.Nodes[i];
					break;
				}
			}
		}
	}

	public static StiReportTreePanelService GetService()
	{
		return GetService(null);
	}

	public static StiReportTreePanelService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Panels.FirstOrDefault((StiPanelService c) => c.GetType() == typeof(StiReportTreePanelService)) as StiReportTreePanelService;
		}
		return designer.Services.GetServices(typeof(StiPanelService)).OfType<StiReportTreePanelService>().FirstOrDefault();
	}

	private void ReportTreeReportEventsBuild(StiEventsCollection events, TreeNode nodeComp)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (!((CheckBox)buttonEventState).Checked)
		{
			return;
		}
		foreach (StiEvent @event in events)
		{
			if (@event.Script.Length != 0)
			{
				nodeComp.Nodes.Add(new TreeNode(@event.ToString())
				{
					Tag = @event
				});
			}
		}
	}

	private void ReportTreeEventsBuild(StiEventsCollection events, TreeNode nodeComp)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		if (!((CheckBox)buttonComponentState).Checked || !((CheckBox)buttonEventState).Checked)
		{
			return;
		}
		foreach (StiEvent @event in events)
		{
			if (@event.Script.Length != 0)
			{
				nodeComp.Nodes.Add(new TreeNode(@event.ToString())
				{
					Tag = @event
				});
			}
		}
	}

	private void ReportTreeOnlyEventsBuild(TreeNode nodePage, StiContainer cont)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		if (!((CheckBox)buttonEventState).Checked)
		{
			return;
		}
		foreach (StiComponent component in cont.Components)
		{
			StiEventsCollection events = component.GetEvents();
			foreach (StiEvent item in events)
			{
				if (!string.IsNullOrEmpty(item.Script))
				{
					nodePage.Nodes.Add(new TreeNode($"{component.Name}.{item}")
					{
						Tag = item
					});
				}
			}
			if (component is StiContainer cont2)
			{
				ReportTreeOnlyEventsBuild(nodePage, cont2);
			}
		}
	}

	private bool IsNeedEvents(StiEventsCollection events)
	{
		if (!((CheckBox)buttonEventState).Checked)
		{
			return false;
		}
		foreach (StiEvent @event in events)
		{
			if (@event.Script.Length != 0)
			{
				return true;
			}
		}
		return false;
	}

	private void ReportTreeComponentBuild(StiContainer cont, TreeNode parentNode)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		if (!((CheckBox)buttonComponentState).Checked)
		{
			return;
		}
		foreach (StiComponent component in cont.Components)
		{
			if (component is IStiHideFromReportTree)
			{
				continue;
			}
			string text = component.ToString(StiOptions.Dictionary.ShowOnlyAliasForComponents && Designer.UseAliases);
			TreeNode val = new TreeNode(text);
			if (componentToImage[component.GetType()] == null)
			{
				Image imageType = Designer.GetImageType(component.GetType(), typeof(StiComponent));
				if (imageType != null)
				{
					componentToImage[component.GetType()] = imageIndex;
					((TreeView)treeViewReportTree).ImageList.Images.Add(imageType);
					val.SelectedImageIndex = imageIndex;
					val.ImageIndex = imageIndex++;
				}
			}
			else
			{
				int num2 = (val.SelectedImageIndex = (int)componentToImage[component.GetType()]);
				val.ImageIndex = num2;
			}
			val.Tag = component;
			parentNode.Nodes.Add(val);
			if (Designer.SelectedObject == component)
			{
				selectedNode = val;
			}
			if (component is StiContainer && ((StiContainer)component).HasSelected)
			{
				ReportTreeEventsBuild(component.GetEvents(), val);
				ReportTreeComponentBuild(component as StiContainer, val);
			}
			else
			{
				StiEventsCollection events = component.GetEvents();
				if ((component is StiContainer && ((StiContainer)component).Components.Count > 0) || IsNeedEvents(events))
				{
					val.Nodes.Add(new TreeNode("##TEMP##"));
				}
			}
			if (Designer.SelectedObject == component)
			{
				((TreeView)treeViewReportTree).SelectedNode = parentNode;
				((TreeView)treeViewReportTree).SelectedNode.EnsureVisible();
			}
		}
	}

	private void ReportTreeBuild()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		if (Report == null)
		{
			((TreeView)treeViewReportTree).Nodes.Clear();
			((Control)treeViewReportTree).Enabled = false;
		}
		else
		{
			if (locked)
			{
				return;
			}
			locked = true;
			findHelper.Clear();
			((TreeView)treeViewReportTree).BeginUpdate();
			double step = StiScale.Step;
			ImageList val = new ImageList
			{
				ColorDepth = (ColorDepth)32,
				ImageSize = new Size((int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step))
			};
			val.Images.Add(EventImage);
			imageIndex = 1;
			StiNodesMemory stiNodesMemory = new StiNodesMemory();
			stiNodesMemory.SaveExpNodes((TreeView)(object)treeViewReportTree, forceNodesSave: true);
			((TreeView)treeViewReportTree).ImageList = val;
			((TreeView)treeViewReportTree).Nodes.Clear();
			if (Report == null)
			{
				((Control)treeViewReportTree).Enabled = false;
			}
			else
			{
				componentToImage = new Hashtable();
				TreeNode val2 = new TreeNode(Report.ToString());
				Image imageType = Designer.GetImageType(typeof(StiReport), typeof(StiReport));
				if (imageType != null)
				{
					val.Images.Add(imageType);
					val2.ImageIndex = imageIndex;
					val2.SelectedImageIndex = imageIndex++;
				}
				val2.Tag = Designer.Report;
				((TreeView)treeViewReportTree).Nodes.Add(val2);
				if (Designer.SelectedObject == Report)
				{
					selectedNode = val2;
				}
				ReportTreeReportEventsBuild(Designer.Report.GetEvents(), val2);
				foreach (StiPage page in Report.Pages)
				{
					Image imageType2 = Designer.GetImageType(page.GetType(), typeof(StiPage));
					if (imageType2 != null)
					{
						val.Images.Add(imageType2);
						imageIndex++;
					}
					string text = page.ToString(StiOptions.Dictionary.ShowOnlyAliasForPages && Designer.UseAliases);
					TreeNode val3 = new TreeNode(text);
					val2.Nodes.Add(val3);
					if (Designer.SelectedObject == page)
					{
						selectedNode = val3;
					}
					val3.Tag = page;
					if (imageType2 != null)
					{
						int num2 = (val3.SelectedImageIndex = imageIndex - 1);
						val3.ImageIndex = num2;
					}
					if (!((CheckBox)buttonComponentState).Checked && ((CheckBox)buttonEventState).Checked)
					{
						ReportTreeReportEventsBuild(page.GetEvents(), val3);
						ReportTreeOnlyEventsBuild(val3, page);
						continue;
					}
					if (page.HasSelected)
					{
						ReportTreeEventsBuild(page.GetEvents(), val3);
						ReportTreeComponentBuild(page, val3);
						continue;
					}
					StiEventsCollection events = page.GetEvents();
					if ((page.Components.Count > 0 && ((CheckBox)buttonComponentState).Checked) || IsNeedEvents(events))
					{
						val3.Nodes.Add(new TreeNode("##TEMP##"));
					}
				}
				((TreeView)treeViewReportTree).SelectedNode = selectedNode;
				selectedNode = null;
				StiTreeView stiTreeView = treeViewReportTree;
				if (stiTreeView != null)
				{
					TreeNode obj = ((TreeView)stiTreeView).SelectedNode;
					if (obj != null)
					{
						obj.EnsureVisible();
					}
				}
				((Control)treeViewReportTree).Enabled = true;
			}
			stiNodesMemory.LoadExpNodes((TreeView)(object)treeViewReportTree, restoreSelectedNode: false);
			((TreeView)treeViewReportTree).EndUpdate();
			((Control)treeViewReportTree).Enabled = true;
			locked = false;
		}
	}

	public override void ApplyStyle()
	{
		((Control)base.DockingContainer).BackColor = StiGuiHelper.GetControlColor();
	}

	private void CreateContexMenu()
	{
		itemContext = new ButtonItem();
		itemExpandAll = new ButtonItem(StiLocalization.Get("Report", "ExpandAll"));
		itemExpandAll.Click += ItemExpandAll_Click;
		itemCollapseAll = new ButtonItem(StiLocalization.Get("Report", "CollapseAll"));
		itemCollapseAll.Click += ItemCollapseAll_Click;
		itemExpandAll.Image = StiReportDesignImages.Actions.ExpandAll();
		itemCollapseAll.Image = StiReportDesignImages.Actions.CollapseAll();
		itemContext.SubItems.AddRange(new BaseItem[2] { itemExpandAll, itemCollapseAll });
		Designer.ContextMenuBar.Items.Add(itemContext);
		Designer.ContextMenuBar.SetContextMenuEx((Control)(object)treeViewReportTree, itemContext);
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		base.Init(designer);
		base.DockingItem.Image = StiReportDesignImages.Actions.ReportTree();
		toolTip = new ToolTip();
		StiTreeView stiTreeView = new StiTreeView();
		((TreeView)stiTreeView).BorderStyle = (BorderStyle)0;
		((Control)stiTreeView).Dock = (DockStyle)5;
		treeViewReportTree = stiTreeView;
		if (StiOptions.Designer.AllowUseDragDrop)
		{
			((Control)treeViewReportTree).AllowDrop = true;
		}
		((TreeView)treeViewReportTree).HideSelection = false;
		StiFlatButtonEdit obj = new StiFlatButtonEdit
		{
			Hint = StiLocalization.Get("FormViewer", "Find"),
			AllowDrawBorder = false
		};
		((Control)obj).Height = (int)Math.Ceiling(24.0 * StiScale.Y);
		((Control)obj).BackColor = Color.White;
		obj.Image = StiReportDesignImages.Actions.SearchSmall();
		obj.Flat = false;
		((Control)obj).ForeColor = SystemColors.ControlText;
		((Control)obj).Dock = (DockStyle)5;
		buttonFindComponent = obj;
		((Control)buttonFindComponent).KeyDown += new KeyEventHandler(ButtonFindComponent_KeyDown);
		buttonFindComponent.ButtonClick += ButtonFindComponent_Click;
		Panel val = new Panel();
		((Control)val).Dock = (DockStyle)1;
		((Control)val).Height = (int)Math.Ceiling(26.0 * StiScale.Y);
		((Control)val).Padding = new Padding(StiScale.I2);
		val.BorderStyle = (BorderStyle)0;
		((Control)val).Controls.Add((Control)(object)buttonFindComponent);
		((Control)val).Paint += new PaintEventHandler(PanelFind_Paint);
		buttonComponentState = GetCheckBox(StiReportDesignImages.Actions.Components());
		((CheckBox)buttonComponentState).CheckedChanged += ButtonState_CheckedChanged;
		toolTip.SetToolTip((Control)(object)buttonComponentState, LocComponentState());
		((Control)val).Controls.Add((Control)(object)buttonComponentState);
		buttonEventState = GetCheckBox(StiReportDesignImages.Actions.Events());
		((CheckBox)buttonEventState).CheckedChanged += ButtonState_CheckedChanged;
		toolTip.SetToolTip((Control)(object)buttonEventState, LocEventState());
		((Control)val).Controls.Add((Control)(object)buttonEventState);
		((TreeView)treeViewReportTree).AfterSelect += new TreeViewEventHandler(TreeViewReportTree_AfterSelect);
		((Control)treeViewReportTree).DragOver += new DragEventHandler(TreeViewReportTree_DragOver);
		((Control)treeViewReportTree).DragDrop += new DragEventHandler(TreeViewReportTree_DragDrop);
		((TreeView)treeViewReportTree).ItemDrag += new ItemDragEventHandler(TreeViewReportTree_ItemDrag);
		((Control)treeViewReportTree).DragEnter += new DragEventHandler(TreeViewReportTree_DragEnter);
		((Control)treeViewReportTree).KeyDown += new KeyEventHandler(TreeViewReportTree_KeyDown);
		((TreeView)treeViewReportTree).BeforeExpand += new TreeViewCancelEventHandler(TreeViewReportTree_BeforeExpand);
		((Control)treeViewReportTree).DoubleClick += TreeViewReportTree_DoubleClick;
		designer.RefreshDesigner += Designer_RefreshDesigner;
		((Control)base.DockingContainer).Controls.Add((Control)(object)treeViewReportTree);
		((Control)base.DockingContainer).Controls.Add((Control)(object)val);
		CreateContexMenu();
		ReportTreeBuild();
	}

	private StiOperationCheckBox GetCheckBox(Image image)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		StiOperationCheckBox stiOperationCheckBox = new StiOperationCheckBox();
		((CheckBox)stiOperationCheckBox).Checked = true;
		((ButtonBase)stiOperationCheckBox).Image = image;
		stiOperationCheckBox.BorderColor = Color.Transparent;
		stiOperationCheckBox.BorderColorChecked = StiColor.Get("#0078d7");
		stiOperationCheckBox.BorderColorMouseOver = Color.Transparent;
		stiOperationCheckBox.BorderColorPressed = Color.Transparent;
		((Control)stiOperationCheckBox).BackColor = StiColor.Get("#ffffff");
		stiOperationCheckBox.BackColorChecked = StiColor.Get("#ffffff");
		stiOperationCheckBox.BackColorMouseOver = StiColor.Get("#b3d7f3");
		stiOperationCheckBox.BackColorPressed = StiColor.Get("#80bceb");
		((Control)stiOperationCheckBox).Dock = (DockStyle)4;
		((Control)stiOperationCheckBox).Padding = new Padding(0);
		((Control)stiOperationCheckBox).Width = (int)Math.Ceiling(21.0 * StiScale.Y);
		((Control)stiOperationCheckBox).Height = (int)Math.Ceiling(24.0 * StiScale.Y);
		return stiOperationCheckBox;
	}

	private void PanelFind_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(195, 195, 195), 1f);
		try
		{
			val2.DashStyle = (DashStyle)0;
			e.Graphics.DrawLine(val2, 1, 1, 1, ((Control)val).Height - 2);
			e.Graphics.DrawLine(val2, 1, 1, ((Control)val).Width - 2, 1);
			e.Graphics.DrawLine(val2, ((Control)val).Width - 2, ((Control)val).Height - 2, 1, ((Control)val).Height - 2);
			e.Graphics.DrawLine(val2, ((Control)val).Width - 2, ((Control)val).Height - 2, ((Control)val).Width - 2, 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		((Component)(object)treeViewReportTree)?.Dispose();
		treeViewReportTree = null;
		((Component)(object)buttonFindComponent)?.Dispose();
		buttonFindComponent = null;
		itemContext?.Dispose();
		itemContext = null;
		itemExpandAll?.Dispose();
		itemExpandAll = null;
		itemCollapseAll?.Dispose();
		itemCollapseAll = null;
		((Component)(object)buttonComponentState)?.Dispose();
		buttonComponentState = null;
		((Component)(object)buttonEventState)?.Dispose();
		buttonEventState = null;
		((Component)(object)toolTip)?.Dispose();
		toolTip = null;
		selectedNode = null;
	}
}
