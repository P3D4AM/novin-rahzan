using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Dashboard.Actions;

public class StiElementActionForm : Form
{
	private const int WM_NCHITTEST = 132;

	private const int HTTRANSPARENT = -1;

	private IContainer components;

	private ToolTip toolTip;

	private StiElementActionButton buttonChangeType;

	private StiElementActionButton buttonFilter;

	private StiElementActionButton buttonFields;

	private ContextMenuBar contextMenuBarChangeType;

	private ButtonItem buttonItemChangeType;

	private ButtonItem buttonItemTable;

	private ButtonItem buttonItemCards;

	private ButtonItem buttonItemChart;

	private ButtonItem buttonItemGauge;

	private ItemContainer itemContainerLine1;

	private ItemContainer itemContainerLine2;

	private ButtonItem buttonItemIndicator;

	private ButtonItem buttonItemProgress;

	private ButtonItem buttonItemRegionMap;

	private ButtonItem buttonItemOnlineMap;

	private ButtonItem buttonItemPivotTable;

	private ItemContainer itemContainerLine3;

	private ButtonItem buttonItemComboBox;

	private ButtonItem buttonItemDatePicker;

	private ButtonItem buttonItemListBox;

	private ButtonItem buttonItemTreeViewBox;

	private ButtonItem buttonItemTreeView;

	private StiElementActionButton buttonSort;

	private StiElementActionButton buttonTopN;

	private StiComponent OriginalElement { get; set; }

	protected override bool ShowWithoutActivation => true;

	public StiElementActionForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		RecalculateButtons();
		DoScaling();
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).ShowInTaskbar = false;
		((Form)this).ControlBox = false;
		((Control)this).Width = ((Control)buttonFields).Width;
		((Control)this).Height = ((Control)buttonChangeType).Bottom;
		((Control)this).BackColor = Color.Gold;
		((Form)this).TransparencyKey = Color.Gold;
		Localize();
		buttonItemCards.Visible = false;
	}

	public void Localize()
	{
		toolTip.ShowAlways = true;
		toolTip.SetToolTip((Control)(object)buttonFields, Loc.Get("FormViewer", "Editor"));
		toolTip.SetToolTip((Control)(object)buttonFilter, Loc.GetMain("Filter"));
		toolTip.SetToolTip((Control)(object)buttonSort, Loc.GetMain("Sort"));
		toolTip.SetToolTip((Control)(object)buttonTopN, Loc.GetMain("TopN"));
		toolTip.SetToolTip((Control)(object)buttonChangeType, Loc.Get("ChartRibbon", "ChangeType"));
		buttonItemTable.Tooltip = Loc.Get("Components", "StiTable");
		buttonItemCards.Tooltip = Loc.Get("Components", "StiCards");
		buttonItemChart.Tooltip = Loc.Get("Components", "StiChart");
		buttonItemGauge.Tooltip = Loc.Get("Components", "StiGauge");
		buttonItemPivotTable.Tooltip = Loc.Get("Components", "StiPivotTable");
		buttonItemIndicator.Tooltip = Loc.Get("Components", "StiIndicator");
		buttonItemProgress.Tooltip = Loc.Get("Components", "StiProgress");
		buttonItemRegionMap.Tooltip = Loc.Get("Components", "StiRegionMap");
		buttonItemOnlineMap.Tooltip = Loc.Get("Components", "StiOnlineMap");
		buttonItemComboBox.Tooltip = Loc.Get("Components", "StiComboBox");
		buttonItemDatePicker.Tooltip = Loc.Get("Components", "StiDatePicker");
		buttonItemListBox.Tooltip = Loc.Get("Components", "StiListBox");
		buttonItemTreeViewBox.Tooltip = Loc.Get("Components", "StiTreeViewBox");
		buttonItemTreeView.Tooltip = Loc.Get("Components", "StiTreeView");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonChangeType).Image = StiReportDesignImages.Actions.ChangeType(StiImageSize.OneHalf);
		((ButtonBase)buttonFields).Image = StiReportDesignImages.Actions.Edit(StiImageSize.OneHalf);
		((ButtonBase)buttonFilter).Image = StiReportDesignImages.Actions.Filter(StiImageSize.OneHalf);
		((ButtonBase)buttonSort).Image = StiReportDesignImages.Actions.Sort(StiImageSize.OneHalf);
		((ButtonBase)buttonTopN).Image = StiReportDesignImages.Actions.TopN(StiImageSize.OneHalf);
		buttonItemChart.Image = StiReportDesignImages.Elements.Chart(StiImageSize.OneHalf);
		buttonItemCards.Image = StiReportDesignImages.Elements.Cards(StiImageSize.OneHalf);
		buttonItemComboBox.Image = StiReportDesignImages.Elements.ComboBox(StiImageSize.OneHalf);
		buttonItemDatePicker.Image = StiReportDesignImages.Elements.DatePicker(StiImageSize.OneHalf);
		buttonItemGauge.Image = StiReportDesignImages.Elements.Gauge(StiImageSize.OneHalf);
		buttonItemIndicator.Image = StiReportDesignImages.Elements.Indicator(StiImageSize.OneHalf);
		buttonItemListBox.Image = StiReportDesignImages.Elements.ListBox(StiImageSize.OneHalf);
		buttonItemOnlineMap.Image = StiReportDesignImages.Elements.OnlineMap(StiImageSize.OneHalf);
		buttonItemPivotTable.Image = StiReportDesignImages.Elements.PivotTable(StiImageSize.OneHalf);
		buttonItemProgress.Image = StiReportDesignImages.Elements.Progress(StiImageSize.OneHalf);
		buttonItemRegionMap.Image = StiReportDesignImages.Elements.RegionMap(StiImageSize.OneHalf);
		buttonItemTable.Image = StiReportDesignImages.Elements.Table(StiImageSize.OneHalf);
		buttonItemTreeView.Image = StiReportDesignImages.Elements.TreeView(StiImageSize.OneHalf);
		buttonItemTreeViewBox.Image = StiReportDesignImages.Elements.TreeViewBox(StiImageSize.OneHalf);
	}

	internal Size RecalculateButtons()
	{
		int num = StiScale.XXI(StiInplaceEditor.ButtonSize);
		int num2 = StiScale.YYI(StiInplaceEditor.ButtonMargin);
		int num3 = 0;
		StiComponent currentComponent = StiElementActionHelper.CurrentComponent;
		((Control)buttonFields).Size = new Size(num, num);
		((Control)buttonFilter).Size = new Size(num, num);
		((Control)buttonSort).Size = new Size(num, num);
		((Control)buttonTopN).Size = new Size(num, num);
		((Control)buttonChangeType).Size = new Size(num, num);
		bool flag = StiInplaceEditor.IsFieldsEditorAllowed(currentComponent);
		((Control)buttonFields).Visible = flag;
		if (flag)
		{
			num3 += ((Control)buttonFields).Height + num2;
		}
		bool flag2 = StiInplaceEditor.IsFilterEditorAllowed(currentComponent);
		((Control)buttonFilter).Visible = flag2;
		((Control)buttonFilter).Location = new Point(0, num3);
		if (flag2)
		{
			num3 += ((Control)buttonFilter).Height + num2;
		}
		bool flag3 = false;
		((Control)buttonSort).Visible = flag3;
		((Control)buttonSort).Location = new Point(0, num3);
		if (flag3)
		{
			num3 += ((Control)buttonSort).Height + num2;
		}
		bool flag4 = StiInplaceEditor.IsTopNEditorAllowed(currentComponent);
		((Control)buttonTopN).Visible = flag4;
		((Control)buttonTopN).Location = new Point(0, num3);
		if (flag4)
		{
			num3 += ((Control)buttonTopN).Height + num2;
		}
		bool flag5 = StiInplaceEditor.IsTypeChangingAllowed(currentComponent);
		((Control)buttonChangeType).Visible = flag5;
		((Control)buttonChangeType).Location = new Point(0, num3);
		if (flag5)
		{
			num3 += ((Control)buttonChangeType).Height + num2;
		}
		return new Size(num, num3);
	}

	private void ResetComponentButtonItems(ItemContainer container, object sender)
	{
		(from ButtonItem i in container.SubItems
			where i != sender
			select i).ToList().ForEach(delegate(ButtonItem i)
		{
			i.Checked = false;
		});
	}

	private StiComponent CreateNewComponent(StiComponent comp)
	{
		if (buttonItemTable.Checked)
		{
			return StiDashboardCreator.CreateTableElement(comp.Report) as StiComponent;
		}
		if (buttonItemCards.Checked)
		{
			return StiDashboardCreator.CreateCardsElement(comp.Report) as StiComponent;
		}
		if (buttonItemChart.Checked)
		{
			return StiDashboardCreator.CreateChartElement(comp.Report) as StiComponent;
		}
		if (buttonItemGauge.Checked)
		{
			return StiDashboardCreator.CreateGaugeElement(comp.Report) as StiComponent;
		}
		if (buttonItemPivotTable.Checked)
		{
			return StiDashboardCreator.CreatePivotTableElement(comp.Report) as StiComponent;
		}
		if (buttonItemIndicator.Checked)
		{
			return StiDashboardCreator.CreateIndicatorElement(comp.Report) as StiComponent;
		}
		if (buttonItemProgress.Checked)
		{
			return StiDashboardCreator.CreateProgressElement(comp.Report) as StiComponent;
		}
		if (buttonItemRegionMap.Checked)
		{
			return StiDashboardCreator.CreateRegionMapElement(comp.Report) as StiComponent;
		}
		if (buttonItemOnlineMap.Checked)
		{
			return StiDashboardCreator.CreateOnlineMapElement(comp.Report) as StiComponent;
		}
		if (buttonItemComboBox.Checked)
		{
			return StiDashboardCreator.CreateComboBoxElement(comp.Report) as StiComponent;
		}
		if (buttonItemDatePicker.Checked)
		{
			return StiDashboardCreator.CreateDatePickerElement(comp.Report) as StiComponent;
		}
		if (buttonItemListBox.Checked)
		{
			return StiDashboardCreator.CreateListBoxElement(comp.Report) as StiComponent;
		}
		if (buttonItemTreeViewBox.Checked)
		{
			return StiDashboardCreator.CreateTreeViewBoxElement(comp.Report) as StiComponent;
		}
		if (buttonItemTreeView.Checked)
		{
			return StiDashboardCreator.CreateTreeViewElement(comp.Report) as StiComponent;
		}
		return null;
	}

	protected override void WndProc(ref Message message)
	{
		((Form)this).WndProc(ref message);
		if (((Message)(ref message)).Msg == 132)
		{
			Point pt = ((Control)this).PointToClient(Cursor.Position);
			if (!((Control)buttonFields).ClientRectangle.Contains(pt) || ((Control)buttonFilter).ClientRectangle.Contains(pt) || ((Control)buttonChangeType).ClientRectangle.Contains(pt))
			{
				((Message)(ref message)).Result = (IntPtr)(-1);
			}
		}
	}

	private void ButtonFields_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnFieldsEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void ButtonInteraction_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnInteractionEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void ButtonChangeType_Click(object sender, EventArgs e)
	{
		Point point = new Point(((Control)buttonChangeType).Width + StiScale.I2, 0);
		point = ((Control)buttonChangeType).PointToScreen(point);
		buttonItemChangeType.Popup(point);
	}

	private void ButtonItemChangeType_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiComponent currentComponent = StiElementActionHelper.CurrentComponent;
		OriginalElement = StiElementActionHelper.CurrentComponent;
		buttonItemTable.Checked = currentComponent is IStiTableElement;
		buttonItemCards.Checked = currentComponent is IStiCardsElement;
		buttonItemChart.Checked = currentComponent is IStiChartElement;
		buttonItemGauge.Checked = currentComponent is IStiGaugeElement;
		buttonItemPivotTable.Checked = currentComponent is IStiPivotTableElement;
		buttonItemIndicator.Checked = currentComponent is IStiIndicatorElement;
		buttonItemProgress.Checked = currentComponent is IStiProgressElement;
		buttonItemRegionMap.Checked = currentComponent is IStiRegionMapElement;
		buttonItemOnlineMap.Checked = currentComponent is IStiOnlineMapElement;
		buttonItemComboBox.Checked = currentComponent is IStiComboBoxElement;
		buttonItemDatePicker.Checked = currentComponent is IStiDatePickerElement;
		buttonItemListBox.Checked = currentComponent is IStiListBoxElement;
		buttonItemTreeViewBox.Checked = currentComponent is IStiTreeViewBoxElement;
		buttonItemTreeView.Checked = currentComponent is IStiTreeViewElement;
	}

	private void ButtonItemElement_Click(object sender, EventArgs e)
	{
		StiElementActionHelper.CurrentComponent.Report.Designer.UndoRedoSave(Loc.Get("ChartRibbon", "ChangeType"));
		ResetComponentButtonItems(itemContainerLine1, sender);
		ResetComponentButtonItems(itemContainerLine2, sender);
		ResetComponentButtonItems(itemContainerLine3, sender);
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.Checked = true;
		StiComponent currentComponent = StiElementActionHelper.CurrentComponent;
		StiComponent stiComponent = CreateNewComponent(currentComponent);
		if (stiComponent == null)
		{
			return;
		}
		bool flag = stiComponent.GetType() == OriginalElement.GetType();
		if (flag)
		{
			stiComponent = OriginalElement;
		}
		StiContainer parent = currentComponent.Parent;
		StiPage page = currentComponent.Page;
		int index = parent.Components.IndexOf(currentComponent);
		parent.Components.Remove(currentComponent);
		parent.Components.Insert(index, stiComponent);
		stiComponent.Parent = parent;
		stiComponent.Page = page;
		if (!flag)
		{
			try
			{
				JObject jObject = OriginalElement.SaveToJsonObject(StiJsonSaveMode.Report);
				stiComponent.LoadFromJsonObject(jObject);
			}
			catch
			{
			}
			try
			{
				(stiComponent as IStiConvertibleElement)?.ConvertFrom(OriginalElement as IStiElement);
			}
			catch
			{
			}
			if (stiComponent is IStiDashboardElementStyle { Style: StiElementStyleIdent.Custom } stiDashboardElementStyle)
			{
				stiDashboardElementStyle.Style = StiElementStyleIdent.Auto;
			}
		}
		StiElementActionHelper.CurrentComponent = stiComponent;
		StiDesignerControl obj3 = currentComponent.Report.Designer.DesignerControl as StiDesignerControl;
		if (obj3 != null)
		{
			StiPageView pageView = obj3.PageView;
			if (pageView != null)
			{
				StiPageViewControl currentPageView = pageView.GetCurrentPageView();
				if (currentPageView != null)
				{
					StiViewControl view = currentPageView.View;
					if (view != null)
					{
						((Control)view).Invalidate();
					}
				}
			}
		}
		currentComponent.Report.Designer.Refresh();
	}

	private void ButtonItemFilter_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnFilterEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void ButtonItemSort_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnSortEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void ButtonItemDataTopN_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnDataTopNEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void ButtonItemDataTransformation_Click(object sender, EventArgs e)
	{
		StiInplaceEditor.OnDataTransformationEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Expected O, but got Unknown
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Expected O, but got Unknown
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Expected O, but got Unknown
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Expected O, but got Unknown
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Expected O, but got Unknown
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_0db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e47: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Expected O, but got Unknown
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1021: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiElementActionForm));
		toolTip = new ToolTip(components);
		contextMenuBarChangeType = new ContextMenuBar();
		buttonItemChangeType = new ButtonItem();
		itemContainerLine1 = new ItemContainer();
		buttonItemTable = new ButtonItem();
		buttonItemCards = new ButtonItem();
		buttonItemChart = new ButtonItem();
		buttonItemGauge = new ButtonItem();
		buttonItemPivotTable = new ButtonItem();
		itemContainerLine2 = new ItemContainer();
		buttonItemIndicator = new ButtonItem();
		buttonItemProgress = new ButtonItem();
		buttonItemRegionMap = new ButtonItem();
		buttonItemOnlineMap = new ButtonItem();
		itemContainerLine3 = new ItemContainer();
		buttonItemComboBox = new ButtonItem();
		buttonItemDatePicker = new ButtonItem();
		buttonItemListBox = new ButtonItem();
		buttonItemTreeViewBox = new ButtonItem();
		buttonItemTreeView = new ButtonItem();
		buttonChangeType = new StiElementActionButton();
		buttonTopN = new StiElementActionButton();
		buttonSort = new StiElementActionButton();
		buttonFilter = new StiElementActionButton();
		buttonFields = new StiElementActionButton();
		((ISupportInitialize)contextMenuBarChangeType).BeginInit();
		((Control)this).SuspendLayout();
		contextMenuBarChangeType.AntiAlias = true;
		contextMenuBarChangeType.DockTabStripHeight = 50;
		((Control)contextMenuBarChangeType).Font = new Font("Segoe UI", 9f);
		contextMenuBarChangeType.Items.AddRange(new BaseItem[1] { buttonItemChangeType });
		contextMenuBarChangeType.Location = new Point(192, 541);
		((Control)contextMenuBarChangeType).Margin = new Padding(5, 5, 5, 5);
		contextMenuBarChangeType.Name = "contextMenuBarChangeType";
		contextMenuBarChangeType.Size = new Size(208, 56);
		contextMenuBarChangeType.Stretch = true;
		contextMenuBarChangeType.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBarChangeType.TabIndex = 6;
		contextMenuBarChangeType.TabStop = false;
		((Control)contextMenuBarChangeType).Text = "contextMenuBarChangeType";
		contextMenuBarChangeType.WrapItemsDock = true;
		buttonItemChangeType.AutoExpandOnClick = true;
		buttonItemChangeType.HorizontalPadding = 0;
		buttonItemChangeType.ImagePaddingHorizontal = 16;
		buttonItemChangeType.ImagePaddingVertical = 12;
		buttonItemChangeType.Name = "buttonItemChangeType";
		buttonItemChangeType.SubItems.AddRange(new BaseItem[3] { itemContainerLine1, itemContainerLine2, itemContainerLine3 });
		buttonItemChangeType.Text = "Change Type";
		buttonItemChangeType.PopupOpen += ButtonItemChangeType_PopupOpen;
		itemContainerLine1.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerLine1.Name = "itemContainerLine1";
		itemContainerLine1.SubItems.AddRange(new BaseItem[5] { buttonItemTable, buttonItemCards, buttonItemPivotTable, buttonItemChart, buttonItemGauge });
		itemContainerLine1.TitleStyle.CornerType = eCornerType.Square;
		buttonItemTable.AutoCheckOnClick = true;
		buttonItemTable.AutoCollapseOnClick = false;
		buttonItemTable.HorizontalPadding = 0;
		buttonItemTable.Image = (Image)componentResourceManager.GetObject("buttonItemTable.Image");
		buttonItemTable.ImagePaddingHorizontal = 16;
		buttonItemTable.ImagePaddingVertical = 12;
		buttonItemTable.Name = "buttonItemTable";
		buttonItemTable.Text = "Table";
		buttonItemTable.Click += ButtonItemElement_Click;
		buttonItemCards.AutoCheckOnClick = true;
		buttonItemCards.AutoCollapseOnClick = false;
		buttonItemCards.HorizontalPadding = 0;
		buttonItemCards.Image = (Image)componentResourceManager.GetObject("buttonItemTable.Image");
		buttonItemCards.ImagePaddingHorizontal = 16;
		buttonItemCards.ImagePaddingVertical = 12;
		buttonItemCards.Name = "buttonItemCards";
		buttonItemCards.Text = "Cards";
		buttonItemCards.Click += ButtonItemElement_Click;
		buttonItemChart.AutoCheckOnClick = true;
		buttonItemChart.AutoCollapseOnClick = false;
		buttonItemChart.HorizontalPadding = 0;
		buttonItemChart.Image = (Image)componentResourceManager.GetObject("buttonItemChart.Image");
		buttonItemChart.ImagePaddingHorizontal = 16;
		buttonItemChart.ImagePaddingVertical = 12;
		buttonItemChart.Name = "buttonItemChart";
		buttonItemChart.Text = "Chart";
		buttonItemChart.Click += ButtonItemElement_Click;
		buttonItemGauge.AutoCheckOnClick = true;
		buttonItemGauge.AutoCollapseOnClick = false;
		buttonItemGauge.HorizontalPadding = 0;
		buttonItemGauge.Image = (Image)componentResourceManager.GetObject("buttonItemGauge.Image");
		buttonItemGauge.ImagePaddingHorizontal = 16;
		buttonItemGauge.ImagePaddingVertical = 12;
		buttonItemGauge.Name = "buttonItemGauge";
		buttonItemGauge.Text = "Gauge";
		buttonItemGauge.Click += ButtonItemElement_Click;
		buttonItemPivotTable.AutoCheckOnClick = true;
		buttonItemPivotTable.AutoCollapseOnClick = false;
		buttonItemPivotTable.HorizontalPadding = 0;
		buttonItemPivotTable.Image = (Image)componentResourceManager.GetObject("buttonItemPivotTable.Image");
		buttonItemPivotTable.ImagePaddingHorizontal = 16;
		buttonItemPivotTable.ImagePaddingVertical = 12;
		buttonItemPivotTable.Name = "buttonItemPivotTable";
		buttonItemPivotTable.Text = "buttonItem1";
		buttonItemPivotTable.Click += ButtonItemElement_Click;
		itemContainerLine2.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerLine2.Name = "itemContainerLine2";
		itemContainerLine2.SubItems.AddRange(new BaseItem[5] { buttonItemIndicator, buttonItemProgress, buttonItemRegionMap, buttonItemOnlineMap, buttonItemComboBox });
		itemContainerLine2.TitleStyle.CornerType = eCornerType.Square;
		buttonItemIndicator.AutoCheckOnClick = true;
		buttonItemIndicator.AutoCollapseOnClick = false;
		buttonItemIndicator.HorizontalPadding = 0;
		buttonItemIndicator.Image = (Image)componentResourceManager.GetObject("buttonItemIndicator.Image");
		buttonItemIndicator.ImagePaddingHorizontal = 16;
		buttonItemIndicator.ImagePaddingVertical = 12;
		buttonItemIndicator.Name = "buttonItemIndicator";
		buttonItemIndicator.Text = "buttonItem2";
		buttonItemIndicator.Click += ButtonItemElement_Click;
		buttonItemProgress.AutoCheckOnClick = true;
		buttonItemProgress.AutoCollapseOnClick = false;
		buttonItemProgress.HorizontalPadding = 0;
		buttonItemProgress.Image = (Image)componentResourceManager.GetObject("buttonItemProgress.Image");
		buttonItemProgress.ImagePaddingHorizontal = 16;
		buttonItemProgress.ImagePaddingVertical = 12;
		buttonItemProgress.Name = "buttonItemProgress";
		buttonItemProgress.Text = "buttonItem3";
		buttonItemProgress.Click += ButtonItemElement_Click;
		buttonItemRegionMap.AutoCheckOnClick = true;
		buttonItemRegionMap.AutoCollapseOnClick = false;
		buttonItemRegionMap.HorizontalPadding = 0;
		buttonItemRegionMap.Image = (Image)componentResourceManager.GetObject("buttonItemRegionMap.Image");
		buttonItemRegionMap.ImagePaddingHorizontal = 16;
		buttonItemRegionMap.ImagePaddingVertical = 12;
		buttonItemRegionMap.Name = "buttonItemRegionMap";
		buttonItemRegionMap.Text = "buttonItem4";
		buttonItemRegionMap.Click += ButtonItemElement_Click;
		buttonItemOnlineMap.AutoCheckOnClick = true;
		buttonItemOnlineMap.AutoCollapseOnClick = false;
		buttonItemOnlineMap.HorizontalPadding = 0;
		buttonItemOnlineMap.Image = (Image)componentResourceManager.GetObject("buttonItemOnlineMap.Image");
		buttonItemOnlineMap.ImagePaddingHorizontal = 16;
		buttonItemOnlineMap.ImagePaddingVertical = 12;
		buttonItemOnlineMap.Name = "buttonItemOnlineMap";
		buttonItemOnlineMap.Text = "buttonItem5";
		buttonItemOnlineMap.Click += ButtonItemElement_Click;
		itemContainerLine3.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerLine3.Name = "itemContainerLine3";
		itemContainerLine3.SubItems.AddRange(new BaseItem[4] { buttonItemDatePicker, buttonItemListBox, buttonItemTreeViewBox, buttonItemTreeView });
		itemContainerLine3.TitleStyle.CornerType = eCornerType.Square;
		buttonItemComboBox.AutoCheckOnClick = true;
		buttonItemComboBox.AutoCollapseOnClick = false;
		buttonItemComboBox.HorizontalPadding = 0;
		buttonItemComboBox.Image = (Image)componentResourceManager.GetObject("buttonItemComboBox.Image");
		buttonItemComboBox.ImagePaddingHorizontal = 16;
		buttonItemComboBox.ImagePaddingVertical = 12;
		buttonItemComboBox.Name = "buttonItemComboBox";
		buttonItemComboBox.Text = "buttonItem6";
		buttonItemComboBox.Click += ButtonItemElement_Click;
		buttonItemDatePicker.AutoCheckOnClick = true;
		buttonItemDatePicker.AutoCollapseOnClick = false;
		buttonItemDatePicker.HorizontalPadding = 0;
		buttonItemDatePicker.Image = (Image)componentResourceManager.GetObject("buttonItemDatePicker.Image");
		buttonItemDatePicker.ImagePaddingHorizontal = 16;
		buttonItemDatePicker.ImagePaddingVertical = 12;
		buttonItemDatePicker.Name = "buttonItemDatePicker";
		buttonItemDatePicker.Text = "buttonItem7";
		buttonItemDatePicker.Click += ButtonItemElement_Click;
		buttonItemListBox.AutoCheckOnClick = true;
		buttonItemListBox.AutoCollapseOnClick = false;
		buttonItemListBox.HorizontalPadding = 0;
		buttonItemListBox.Image = (Image)componentResourceManager.GetObject("buttonItemListBox.Image");
		buttonItemListBox.ImagePaddingHorizontal = 16;
		buttonItemListBox.ImagePaddingVertical = 12;
		buttonItemListBox.Name = "buttonItemListBox";
		buttonItemListBox.Text = "buttonItem8";
		buttonItemListBox.Click += ButtonItemElement_Click;
		buttonItemTreeViewBox.AutoCheckOnClick = true;
		buttonItemTreeViewBox.AutoCollapseOnClick = false;
		buttonItemTreeViewBox.HorizontalPadding = 0;
		buttonItemTreeViewBox.Image = (Image)componentResourceManager.GetObject("buttonItemTreeViewBox.Image");
		buttonItemTreeViewBox.ImagePaddingHorizontal = 16;
		buttonItemTreeViewBox.ImagePaddingVertical = 12;
		buttonItemTreeViewBox.Name = "buttonItemTreeViewBox";
		buttonItemTreeViewBox.Text = "buttonItem9";
		buttonItemTreeViewBox.Click += ButtonItemElement_Click;
		buttonItemTreeView.AutoCheckOnClick = true;
		buttonItemTreeView.AutoCollapseOnClick = false;
		buttonItemTreeView.HorizontalPadding = 0;
		buttonItemTreeView.Image = (Image)componentResourceManager.GetObject("buttonItemTreeView.Image");
		buttonItemTreeView.ImagePaddingHorizontal = 16;
		buttonItemTreeView.ImagePaddingVertical = 12;
		buttonItemTreeView.Name = "buttonItemTreeView";
		buttonItemTreeView.Text = "buttonItem10";
		buttonItemTreeView.Click += ButtonItemElement_Click;
		((ButtonBase)buttonChangeType).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonChangeType).Image = (Image)componentResourceManager.GetObject("buttonChangeType.Image");
		((Control)buttonChangeType).Location = new Point(0, 315);
		((Control)buttonChangeType).Margin = new Padding(5, 5, 5, 5);
		((Control)buttonChangeType).Name = "buttonChangeType";
		((Control)buttonChangeType).Size = new Size(75, 67);
		((Control)buttonChangeType).TabIndex = 3;
		((Control)buttonChangeType).TabStop = false;
		((Control)buttonChangeType).Click += ButtonChangeType_Click;
		((ButtonBase)buttonTopN).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonTopN).Image = (Image)componentResourceManager.GetObject("buttonTopN.Image");
		((Control)buttonTopN).Location = new Point(0, 236);
		((Control)buttonTopN).Margin = new Padding(5, 5, 5, 5);
		((Control)buttonTopN).Name = "buttonTopN";
		((Control)buttonTopN).Size = new Size(75, 67);
		((Control)buttonTopN).TabIndex = 1;
		((Control)buttonTopN).TabStop = false;
		((ButtonBase)buttonTopN).UseVisualStyleBackColor = true;
		((Control)buttonTopN).Click += ButtonItemDataTopN_Click;
		((ButtonBase)buttonSort).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSort).Image = (Image)componentResourceManager.GetObject("buttonSort.Image");
		((Control)buttonSort).Location = new Point(0, 157);
		((Control)buttonSort).Margin = new Padding(5, 5, 5, 5);
		((Control)buttonSort).Name = "buttonSort";
		((Control)buttonSort).Size = new Size(75, 67);
		((Control)buttonSort).TabIndex = 1;
		((Control)buttonSort).TabStop = false;
		((ButtonBase)buttonSort).UseVisualStyleBackColor = true;
		((Control)buttonSort).Click += ButtonItemSort_Click;
		((ButtonBase)buttonFilter).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonFilter).Image = (Image)componentResourceManager.GetObject("buttonFilter.Image");
		((Control)buttonFilter).Location = new Point(0, 79);
		((Control)buttonFilter).Margin = new Padding(5, 5, 5, 5);
		((Control)buttonFilter).Name = "buttonFilter";
		((Control)buttonFilter).Size = new Size(75, 67);
		((Control)buttonFilter).TabIndex = 1;
		((Control)buttonFilter).TabStop = false;
		((ButtonBase)buttonFilter).UseVisualStyleBackColor = true;
		((Control)buttonFilter).Click += ButtonItemFilter_Click;
		((ButtonBase)buttonFields).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonFields).Image = (Image)componentResourceManager.GetObject("buttonFields.Image");
		((Control)buttonFields).Location = new Point(0, 0);
		((Control)buttonFields).Margin = new Padding(5, 5, 5, 5);
		((Control)buttonFields).Name = "buttonFields";
		((Control)buttonFields).Size = new Size(75, 67);
		((Control)buttonFields).TabIndex = 0;
		((Control)buttonFields).TabStop = false;
		((ButtonBase)buttonFields).UseVisualStyleBackColor = true;
		((Control)buttonFields).Click += ButtonFields_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(16f, 31f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(429, 641);
		((Control)this).Controls.Add((Control)(object)contextMenuBarChangeType);
		((Control)this).Controls.Add((Control)(object)buttonChangeType);
		((Control)this).Controls.Add((Control)(object)buttonTopN);
		((Control)this).Controls.Add((Control)(object)buttonSort);
		((Control)this).Controls.Add((Control)(object)buttonFilter);
		((Control)this).Controls.Add((Control)(object)buttonFields);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(5, 5, 5, 5);
		((Control)this).Name = "StiElementActionForm";
		((ISupportInitialize)contextMenuBarChangeType).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
