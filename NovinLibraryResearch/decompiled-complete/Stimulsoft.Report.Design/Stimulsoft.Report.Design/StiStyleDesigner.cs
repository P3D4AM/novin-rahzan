using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Styles;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Styles.Helpers;

namespace Stimulsoft.Report.Design;

public class StiStyleDesigner : Form
{
	private bool lockExpand;

	private List<Type> supportedStyles;

	private List<Type> showStyles = new List<Type>();

	private List<ButtonItem> customShowItems = new List<ButtonItem>();

	private StiNoStyleControl noStyleControl;

	private StiStylesDragDropAction dragDropAction;

	private bool LockUpdateState;

	private StiDesignerControl designer;

	private bool changed;

	private List<StiStyleDesignerChangeName> hashChangeName = new List<StiStyleDesignerChangeName>();

	private List<string> hashUsedStylesInReport = new List<string>();

	private List<string> hashExpandedCollections = new List<string>();

	private StiDictionarySortOrder sortType = StiDictionarySortOrder.None;

	private object selectedNodeTag;

	private string findText;

	private Type chartStyleType = typeof(StiChartStyle);

	private Type indicatorStyleType = typeof(StiIndicatorStyle);

	private Type progressStyleType = typeof(StiProgressStyle);

	private Type crossTabStyleType = typeof(StiCrossTabStyle);

	private Type dialogStyleType = typeof(StiDialogStyle);

	private Type gaugeStyleType = typeof(StiGaugeStyle);

	private Type mapStyleType = typeof(StiMapStyle);

	private Type componentStyleType = typeof(StiStyle);

	private Type tableStyleType = typeof(StiTableStyle);

	private Type cardsStyleType = typeof(StiCardsStyle);

	private bool showGetStylesButton;

	private IContainer components;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine glButtons;

	private StiReportPropertyGrid Properties;

	private ButtonItem tbStyleAdd;

	private ButtonItem tbActions;

	private ButtonItem tbStyleOpen;

	private ButtonItem tbStyleSave;

	private ButtonItem tbStyleGet;

	private ButtonItem tbStylesCreate;

	private ImageList ilStyles;

	private ToolTip toolTip1;

	private ButtonItem tbStyleRemove;

	private StiStylesTreeView treeViewStyles;

	private ButtonItem biComponentStyle;

	private ButtonItem biMapStyle;

	private ButtonItem biTableStyle;

	private ButtonItem biCardsStyle;

	private ButtonItem biChartStyle;

	private ButtonItem biIndicatorStyle;

	private ButtonItem biProgressStyle;

	private ButtonItem biGaugeStyle;

	private ButtonItem biCrossTabStyle;

	private ButtonItem biDialogStyle;

	private Panel panelRight;

	private Bar bar1;

	private ButtonItem btSettings;

	private ButtonItem miShowComponentStyle;

	private ButtonItem miShowChartStyle;

	private ButtonItem miShowIndicatorStyle;

	private ButtonItem miShowProgressStyle;

	private ButtonItem miShowGaugeStyle;

	private ButtonItem miShowMapStyle;

	private ButtonItem miShowCrossTabStyle;

	private ButtonItem miShowTableStyle;

	private ButtonItem miShowCardsStyle;

	private ButtonItem miShowDialogStyle;

	private ButtonItem miSortAsc;

	private ButtonItem miSortDesc;

	private ButtonItem miSortNone;

	private TextBoxItem textBoxFindComponent;

	private LabelItem miHeaderFilter;

	private LabelItem miHeaderSort;

	private StiGroupLine stiGroupLine1;

	public DotNetBarManager DotNetBarManager;

	public DockSite DockSiteBottom;

	public DockSite DockSiteLeft;

	public DockSite DockSiteRight;

	public DockSite DockSiteTop;

	private ButtonItem contextMenuStyles;

	private ButtonItem buttonItemAddStyle;

	private ButtonItem buttonItemDuplicate;

	private ButtonItem buttonItemCut;

	private ButtonItem buttonItemCopy;

	private ButtonItem buttonItemAddComponentStyle;

	private ButtonItem buttonItemAddChartStyle;

	private ButtonItem buttonItemAddCardsStyle;

	private ButtonItem buttonItemAddGaugeStyle;

	private ButtonItem buttonItemAddMapStyle;

	private ButtonItem buttonItemAddCrossTableStyle;

	private ButtonItem buttonItemAddTableStyle;

	private ButtonItem buttonItemAddIndicatorStyle;

	private ButtonItem buttonItemAddProgressStyle;

	private ButtonItem buttonItemAddDialogStyle;

	private ButtonItem buttonItemPaste;

	private ButtonItem buttonItemDelete;

	private ButtonItem buttonItemCreateStyleCollection;

	private DockSite dockSite4;

	private DockSite dockSite1;

	private DockSite dockSite2;

	private DockSite dockSite3;

	private ContextMenuBar contextMenuBar;

	public StiReport Report { get; set; }

	internal List<StiBaseStyle> HashAllStyles { get; } = new List<StiBaseStyle>();


	internal Dictionary<string, List<StiBaseStyle>> HashStyles { get; } = new Dictionary<string, List<StiBaseStyle>>();


	internal StiStylesTreeView TreeViewStyles => treeViewStyles;

	public bool ShowGetStylesButton
	{
		get
		{
			return showGetStylesButton;
		}
		set
		{
			showGetStylesButton = value;
			tbStyleGet.Visible = value;
		}
	}

	public bool AllowMoveStyles
	{
		get
		{
			if (sortType == StiDictionarySortOrder.None && showStyles.Count == supportedStyles.Count)
			{
				return findText == null;
			}
			return false;
		}
	}

	public StiStyleDesigner(StiDesignerControl designer, StiReport report)
	{
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		DoScaling();
		((Form)this).ShowIcon = false;
		Localize();
		this.designer = designer;
		Report = report;
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		((Control)Properties).Width = StiSettings.GetInt("StiStyleDesigner", "Spl", ((Control)panelRight).Width);
		sortType = (StiDictionarySortOrder)StiSettings.GetInt("StiStyleDesigner", "Sort", 2);
		supportedStyles = new List<Type>
		{
			typeof(StiIndicatorStyle),
			typeof(StiProgressStyle),
			typeof(StiCrossTabStyle),
			typeof(StiCardsStyle),
			typeof(StiTableStyle),
			typeof(StiChartStyle),
			typeof(StiGaugeStyle),
			typeof(StiMapStyle),
			typeof(StiStyle),
			typeof(StiDialogStyle)
		};
		miShowComponentStyle.Tag = typeof(StiStyle);
		miShowCrossTabStyle.Tag = typeof(StiCrossTabStyle);
		miShowTableStyle.Tag = typeof(StiTableStyle);
		miShowCardsStyle.Tag = typeof(StiCardsStyle);
		miShowChartStyle.Tag = typeof(StiChartStyle);
		miShowIndicatorStyle.Tag = typeof(StiIndicatorStyle);
		miShowProgressStyle.Tag = typeof(StiProgressStyle);
		miShowGaugeStyle.Tag = typeof(StiGaugeStyle);
		miShowMapStyle.Tag = typeof(StiMapStyle);
		miShowDialogStyle.Tag = typeof(StiDialogStyle);
		foreach (Type supportedStyle in supportedStyles)
		{
			if (StiSettings.GetBool("StiStyleDesigner", "Show" + supportedStyle.FullName, nullValue: true))
			{
				showStyles.Add(supportedStyle);
			}
		}
		miShowComponentStyle.Checked = showStyles.Contains((Type)miShowComponentStyle.Tag);
		miShowCrossTabStyle.Checked = showStyles.Contains((Type)miShowCrossTabStyle.Tag);
		miShowTableStyle.Checked = showStyles.Contains((Type)miShowTableStyle.Tag);
		miShowCardsStyle.Checked = showStyles.Contains((Type)miShowCardsStyle.Tag);
		miShowChartStyle.Checked = showStyles.Contains((Type)miShowChartStyle.Tag);
		miShowIndicatorStyle.Checked = showStyles.Contains((Type)miShowIndicatorStyle.Tag);
		miShowProgressStyle.Checked = showStyles.Contains((Type)miShowProgressStyle.Tag);
		miShowGaugeStyle.Checked = showStyles.Contains((Type)miShowGaugeStyle.Tag);
		miShowMapStyle.Checked = showStyles.Contains((Type)miShowMapStyle.Tag);
		miShowDialogStyle.Checked = showStyles.Contains((Type)miShowDialogStyle.Tag);
		switch (sortType)
		{
		case StiDictionarySortOrder.Asc:
			miSortAsc.Checked = true;
			break;
		case StiDictionarySortOrder.Desc:
			miSortDesc.Checked = true;
			break;
		case StiDictionarySortOrder.None:
			miSortNone.Checked = true;
			break;
		}
		FillUsedStylesInReport();
		PrepareStyles();
		Build();
		if (((TreeView)treeViewStyles).Nodes.Count > 0)
		{
			((TreeView)treeViewStyles).SelectedNode = ((TreeView)treeViewStyles).Nodes[0];
		}
		Properties.SetStandardPropertyTabs();
		UpdateState();
		tbStyleGet.Enabled = false;
		StiComponentsCollection stiComponentsCollection = report.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (!(item is StiPage) && item.IsSelected)
			{
				tbStyleGet.Enabled = true;
				break;
			}
		}
		miShowComponentStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowChartStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowCardsStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowIndicatorStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowProgressStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowGaugeStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowMapStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowCrossTabStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowTableStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miShowDialogStyle.CheckedChanged += MenuitemShowStyle_CheckedChanged;
		miSortAsc.Click += MenuItemSortAsc_Click;
		miSortDesc.Click += MenuItemSortDesc_Click;
		miSortNone.Click += MenuItemSortNone_Click;
		textBoxFindComponent.KeyDown += new KeyEventHandler(TextBoxFindComponent_KeyDown);
		textBoxFindComponent.TextChanged += TextBoxFindComponent_TextChanged;
		textBoxFindComponent.GotFocus += TextBoxFindComponent_GotFocus;
		textBoxFindComponent.LostFocus += TextBoxFindComponent_LostFocus;
		dragDropAction = new StiStylesDragDropAction(this);
		StiStyleDesignerOpeningEventArgs stiStyleDesignerOpeningEventArgs = new StiStyleDesignerOpeningEventArgs();
		StiOptions.Designer.StyleDesigner.InvokeOpening(this, stiStyleDesignerOpeningEventArgs);
		if (stiStyleDesignerOpeningEventArgs.CustomAddButtons != null && stiStyleDesignerOpeningEventArgs.CustomAddButtons.Count > 0)
		{
			int num = btSettings.SubItems.IndexOf(miShowDialogStyle) + 1;
			foreach (StiCustomStyleButtonInfo customAddButton in stiStyleDesignerOpeningEventArgs.CustomAddButtons)
			{
				supportedStyles.Add(customAddButton.StyleType);
				ButtonItem buttonItem = new ButtonItem
				{
					Image = customAddButton.Icon32,
					Text = customAddButton.Text,
					Tag = customAddButton.StyleType,
					AutoCollapseOnClick = false,
					AutoCheckOnClick = true
				};
				if (StiSettings.GetBool("StiStyleDesigner", "Show" + customAddButton.StyleType.FullName, nullValue: true))
				{
					buttonItem.Checked = true;
					showStyles.Add(customAddButton.StyleType);
				}
				customShowItems.Add(buttonItem);
				buttonItem.Click += MenuitemShowStyle_CheckedChanged;
				btSettings.SubItems.Insert(num, buttonItem);
				num++;
				ButtonItem buttonItem2 = new ButtonItem
				{
					Image = customAddButton.Icon32,
					Text = customAddButton.Text,
					Tag = customAddButton.StyleType
				};
				buttonItem2.Click += ButtonAddCustomStyle_Click;
				tbStyleAdd.SubItems.Add(buttonItem2);
				ButtonItem buttonItem3 = new ButtonItem
				{
					Image = customAddButton.Icon16,
					Text = customAddButton.Text,
					Tag = customAddButton.StyleType
				};
				buttonItem3.Click += MenuItemAddCustomStyle_Click;
				buttonItemAddStyle.SubItems.Add(buttonItem3);
			}
		}
		contextMenuBar.SetContextMenuEx((Control)(object)treeViewStyles, contextMenuStyles);
		contextMenuBar.SetContextMenuEx((Control)(object)noStyleControl, contextMenuStyles);
		contextMenuBar.SetContextMenuEx((Control)(object)this, contextMenuStyles);
		miShowCardsStyle.Visible = false;
		biCardsStyle.Visible = false;
		buttonItemAddCardsStyle.Visible = false;
	}

	private void Collect()
	{
		if (supportedStyles != null)
		{
			supportedStyles.Clear();
			supportedStyles = null;
		}
		if (showStyles != null)
		{
			showStyles.Clear();
			showStyles = null;
		}
		if (customShowItems != null)
		{
			customShowItems.Clear();
			customShowItems = null;
		}
		if (dragDropAction != null)
		{
			dragDropAction.Dispose();
			dragDropAction = null;
		}
		if (hashChangeName != null)
		{
			hashChangeName.Clear();
			hashChangeName = null;
		}
		if (hashUsedStylesInReport != null)
		{
			hashUsedStylesInReport.Clear();
			hashUsedStylesInReport = null;
		}
		if (hashExpandedCollections != null)
		{
			hashExpandedCollections.Clear();
			hashExpandedCollections = null;
		}
		noStyleControl = null;
		designer = null;
		Report = null;
		chartStyleType = null;
		indicatorStyleType = null;
		progressStyleType = null;
		crossTabStyleType = null;
		dialogStyleType = null;
		gaugeStyleType = null;
		mapStyleType = null;
		componentStyleType = null;
		tableStyleType = null;
		cardsStyleType = null;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormStyleDesigner", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		textBoxFindComponent.WatermarkText = StiLocalization.Get("Editor", "Search");
		miHeaderSort.Text = StiLocalization.Get("PropertyMain", "Sort");
		miSortAsc.Text = StiLocalization.Get("FormBand", "Ascending");
		miSortDesc.Text = StiLocalization.Get("FormBand", "Descending");
		miSortNone.Text = StiLocalization.Get("FormBand", "NoSort");
		miHeaderFilter.Text = StiLocalization.Get("PropertyMain", "Filter");
		miShowComponentStyle.Text = StiLocalization.Get("Components", "StiComponent");
		miShowCrossTabStyle.Text = StiLocalization.Get("Components", "StiCrossTab");
		miShowTableStyle.Text = StiLocalization.Get("Components", "StiTable");
		miShowCardsStyle.Text = StiLocalization.Get("Components", "StiCards");
		miShowChartStyle.Text = StiLocalization.Get("Components", "StiChart");
		miShowIndicatorStyle.Text = StiLocalization.Get("Components", "StiIndicator");
		miShowProgressStyle.Text = StiLocalization.Get("Components", "StiProgress");
		miShowGaugeStyle.Text = StiLocalization.Get("Components", "StiGauge");
		miShowMapStyle.Text = StiLocalization.Get("Components", "StiMap");
		miShowDialogStyle.Text = StiLocalization.Get("PropertyCategory", "ControlCategory");
		buttonItemCreateStyleCollection.Text = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "CreateStyleCollection"));
		tbActions.Text = StiLocalization.Get("FormDictionaryDesigner", "Actions");
		tbStyleSave.Text = StiLocalization.Get("MainMenu", "menuFileSaveAs");
		tbStyleOpen.Text = StiLocalization.Get("Buttons", "Open") + "...";
		tbStylesCreate.Text = StiLocalization.Get("FormStyleDesigner", "CreateStyleCollection") + "...";
		tbStyleGet.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "GetStyle"));
		tbStyleAdd.Text = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Add"));
		tbStyleAdd.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Add"));
		tbStyleRemove.Tooltip = StiLocalization.Get("Buttons", "Remove");
		buttonItemDuplicate.Text = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Duplicate"));
		buttonItemCopy.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCopy"));
		buttonItemCut.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCut"));
		buttonItemPaste.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditPaste"));
		biComponentStyle.Text = StiLocalization.Get("Components", "StiComponent");
		biCrossTabStyle.Text = StiLocalization.Get("Components", "StiCrossTab");
		biCardsStyle.Text = StiLocalization.Get("Components", "StiCards");
		biChartStyle.Text = StiLocalization.Get("Components", "StiChart");
		biGaugeStyle.Text = StiLocalization.Get("Components", "StiGauge");
		biIndicatorStyle.Text = StiLocalization.Get("Components", "StiIndicator");
		biProgressStyle.Text = StiLocalization.Get("Components", "StiProgress");
		biMapStyle.Text = StiLocalization.Get("Components", "StiMap");
		biTableStyle.Text = StiLocalization.Get("Components", "StiTable");
		biDialogStyle.Text = StiLocalization.Get("PropertyCategory", "ControlCategory");
		buttonItemAddStyle.Text = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Add"));
		buttonItemDelete.Text = StiLocalization.Get("Buttons", "Remove");
		buttonItemAddComponentStyle.Text = StiLocalization.Get("Components", "StiComponent");
		buttonItemAddCrossTableStyle.Text = StiLocalization.Get("Components", "StiCrossTab");
		buttonItemAddChartStyle.Text = StiLocalization.Get("Components", "StiChart");
		buttonItemAddCardsStyle.Text = StiLocalization.Get("Components", "StiCards");
		buttonItemAddIndicatorStyle.Text = StiLocalization.Get("Components", "StiIndicator");
		buttonItemAddProgressStyle.Text = StiLocalization.Get("Components", "StiProgress");
		buttonItemAddGaugeStyle.Text = StiLocalization.Get("Components", "StiGauge");
		buttonItemAddMapStyle.Text = StiLocalization.Get("Components", "StiMap");
		buttonItemAddTableStyle.Text = StiLocalization.Get("Components", "StiTable");
		buttonItemAddDialogStyle.Text = StiLocalization.Get("PropertyCategory", "ControlCategory");
	}

	private bool ContainsStyles(Type type)
	{
		return HashAllStyles.FirstOrDefault((StiBaseStyle x) => x.GetType() == type) != null;
	}

	private bool ProcessFind()
	{
		if (textBoxFindComponent.Text.Length == 0)
		{
			if (findText != null)
			{
				findText = null;
				return true;
			}
		}
		else if (findText != textBoxFindComponent.Text)
		{
			findText = textBoxFindComponent.Text;
			return true;
		}
		return false;
	}

	private bool ProcessCleanFind()
	{
		textBoxFindComponent.Text = string.Empty;
		if (findText != null)
		{
			findText = null;
			return true;
		}
		return false;
	}

	private void ProcessMoveUp()
	{
		if (!AllowMoveStyles)
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode == null || !(selectedNode.Tag is StiBaseStyle))
		{
			return;
		}
		StiBaseStyle item = (StiBaseStyle)selectedNode.Tag;
		if (selectedNode.Parent == null)
		{
			int num = HashStyles.Count - 1;
			int num2 = ((TreeView)treeViewStyles).Nodes.IndexOf(selectedNode) - num;
			if (num2 != 0)
			{
				((TreeView)treeViewStyles).Nodes.Remove(selectedNode);
				((TreeView)treeViewStyles).Nodes.Insert(num2 - 1 + num, selectedNode);
				List<StiBaseStyle> list = HashStyles[string.Empty];
				list.RemoveAt(num2);
				list.Insert(num2 - 1, item);
				((TreeView)treeViewStyles).SelectedNode = selectedNode;
			}
			return;
		}
		TreeNode parent = selectedNode.Parent;
		string key = (string)parent.Tag;
		int num3 = parent.Nodes.IndexOf(selectedNode);
		if (num3 != 0)
		{
			parent.Nodes.Remove(selectedNode);
			parent.Nodes.Insert(num3 - 1, selectedNode);
			List<StiBaseStyle> list2 = HashStyles[key];
			list2.RemoveAt(num3);
			list2.Insert(num3 - 1, item);
			((TreeView)treeViewStyles).SelectedNode = selectedNode;
		}
	}

	private void ProcessMoveDown()
	{
		if (!AllowMoveStyles)
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode == null || !(selectedNode.Tag is StiBaseStyle))
		{
			return;
		}
		StiBaseStyle item = (StiBaseStyle)selectedNode.Tag;
		if (selectedNode.Parent == null)
		{
			int num = HashStyles.Count - 1;
			int num2 = ((TreeView)treeViewStyles).Nodes.IndexOf(selectedNode) - num;
			List<StiBaseStyle> list = HashStyles[string.Empty];
			if (num2 != list.Count - 1)
			{
				((TreeView)treeViewStyles).Nodes.Remove(selectedNode);
				((TreeView)treeViewStyles).Nodes.Insert(num2 + 1 + num, selectedNode);
				list.RemoveAt(num2);
				list.Insert(num2 + 1, item);
				((TreeView)treeViewStyles).SelectedNode = selectedNode;
			}
			return;
		}
		TreeNode parent = selectedNode.Parent;
		string key = (string)parent.Tag;
		int num3 = parent.Nodes.IndexOf(selectedNode);
		List<StiBaseStyle> list2 = HashStyles[key];
		if (num3 != list2.Count - 1)
		{
			parent.Nodes.Remove(selectedNode);
			parent.Nodes.Insert(num3 + 1, selectedNode);
			list2.RemoveAt(num3);
			list2.Insert(num3 + 1, item);
			((TreeView)treeViewStyles).SelectedNode = selectedNode;
		}
	}

	private void ProcessStyleDuplicate()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode != null && selectedNode.Tag is StiBaseStyle)
		{
			StiBaseStyle stiBaseStyle = ((StiBaseStyle)selectedNode.Tag).Clone() as StiBaseStyle;
			AddStyle(stiBaseStyle, null, stiBaseStyle.Name + "Copy");
			changed = true;
			UpdateNoStyleState();
		}
	}

	private void ProcessNewCrossTabStyle()
	{
		AddStyle(new StiCrossTabStyle(), typeof(StiCrossTabStyle));
	}

	private void ProcessNewTableStyle()
	{
		AddStyle(new StiTableStyle(), typeof(StiTableStyle));
	}

	private void ProcessNewCardsStyle()
	{
		AddStyle(new StiCardsStyle(), typeof(StiCardsStyle));
	}

	private void ProcessNewChartStyle()
	{
		AddStyle(new StiChartStyle(), typeof(StiChartStyle));
	}

	private void ProcessNewIndicatorStyle()
	{
		AddStyle(new StiIndicatorStyle(), typeof(StiIndicatorStyle));
	}

	private void ProcessNewProgressStyle()
	{
		AddStyle(new StiProgressStyle(), typeof(StiProgressStyle));
	}

	private void ProcessNewGaugeStyle()
	{
		AddStyle(new StiGaugeStyle(), typeof(StiGaugeStyle));
	}

	private void ProcessNewMapStyle()
	{
		AddStyle(new StiMapStyle(), typeof(StiMapStyle));
	}

	internal void ProcessNewComponentStyle()
	{
		AddStyle(new StiStyle(), null);
	}

	private void ProcessNewDialogStyle()
	{
		AddStyle(new StiDialogStyle(), null);
	}

	private void ProcessNewCustomStyle(Type type)
	{
		if (StiActivator.CreateObject(type) is StiBaseStyle style)
		{
			AddStyle(style, null);
		}
	}

	private void ProcessRemove()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode != null)
		{
			if (selectedNode.Tag is StiBaseStyle)
			{
				RemoveStyle();
			}
			else
			{
				RemoveStyleCollection();
			}
		}
	}

	private void ProcessStyleCut()
	{
		ProcessStyleCopy();
		RemoveStyle();
	}

	private void ProcessStyleCopy()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode != null && selectedNode.Tag is StiBaseStyle)
		{
			changed = true;
			StiBaseStyle stiBaseStyle = (StiBaseStyle)selectedNode.Tag;
			string obj = stiBaseStyle.SaveToJsonObject(StiJsonSaveMode.Report).ToString();
			StiClipboard.SetData("StiBaseStyle", obj);
		}
	}

	private void ProcessStylePaste()
	{
		try
		{
			string data = StiClipboard.GetData("StiBaseStyle");
			if (!string.IsNullOrEmpty(data))
			{
				JObject styleJObject = (JObject)JsonConvert.DeserializeObject(data);
				StiBaseStyle stiBaseStyle = StiStylesCollection.LoadStyleFromJObject(styleJObject);
				changed = true;
				AddStyle(stiBaseStyle, null, stiBaseStyle.Name + "Copy");
				UpdateNoStyleState();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ProcessStyleSave()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "StylesFiles");
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Save"));
				((FileDialog)val).FileName = StiLocalization.Get("PropertyMain", "Styles");
				((FileDialog)val).DefaultExt = "sts";
				if ((int)((CommonDialog)val).ShowDialog() != 1 || string.IsNullOrEmpty(((FileDialog)val).FileName))
				{
					return;
				}
				StiStylesCollection stiStylesCollection = new StiStylesCollection();
				foreach (StiBaseStyle hashAllStyle in HashAllStyles)
				{
					stiStylesCollection.Add(hashAllStyle);
				}
				StiStylesSheet stiStylesSheet = new StiStylesSheet(stiStylesCollection);
				stiStylesSheet.Save(((FileDialog)val).FileName);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ProcessStyleOpen()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "StylesFiles");
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormStyleDesigner", "Open"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() != 1 || string.IsNullOrEmpty(((FileDialog)val).FileName))
				{
					return;
				}
				changed = true;
				StiStylesSheet stiStylesSheet = new StiStylesSheet(new StiStylesCollection());
				if (StiJsonChecker.IsValidJsonFile(((FileDialog)val).FileName))
				{
					stiStylesSheet.LoadFromJsonFile(((FileDialog)val).FileName);
				}
				else
				{
					stiStylesSheet.Load(((FileDialog)val).FileName);
				}
				HashAllStyles.Clear();
				HashStyles.Clear();
				hashExpandedCollections.Clear();
				HashStyles.Add(string.Empty, new List<StiBaseStyle>());
				foreach (StiBaseStyle style in stiStylesSheet.Styles)
				{
					HashAllStyles.Add(style);
					if (string.IsNullOrEmpty(style.CollectionName))
					{
						HashStyles[string.Empty].Add(style);
						continue;
					}
					if (!HashStyles.ContainsKey(style.CollectionName))
					{
						HashStyles.Add(style.CollectionName, new List<StiBaseStyle>());
					}
					HashStyles[style.CollectionName].Add(style);
				}
				Build();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ProcessStyleGet()
	{
		CheckUndoRedo();
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		StiComponentsCollection selectedComponentsWithChilds = Report.GetCurrentPage().GetSelectedComponentsWithChilds();
		foreach (StiComponent item in selectedComponentsWithChilds)
		{
			if (item is StiPage)
			{
				continue;
			}
			StiBaseStyle stiBaseStyle = null;
			if (item is IStiChartElement)
			{
				IStiChartElement element = (IStiChartElement)item;
				IStiChartStyle stiChartStyle = null;
				if (element.Style == StiElementStyleIdent.Custom && !string.IsNullOrEmpty(element.CustomStyleName))
				{
					if (element.Report.Styles.ToList().FirstOrDefault((StiBaseStyle x) => x.Name == element.CustomStyleName) is StiChartStyle)
					{
						stiChartStyle = new StiCustomStyle(element.CustomStyleName);
					}
				}
				else
				{
					StiElementStyleIdent style = ((element.Style == StiElementStyleIdent.Auto) ? ((IStiDashboard)element.Page).Style : element.Style);
					stiChartStyle = StiDashboardStyleHelper.GetChartStyle(style);
				}
				if (stiChartStyle == null)
				{
					stiChartStyle = new StiStyle29();
				}
				stiBaseStyle = StiDashboardStyleHelper.GetCopyChartStyle(stiChartStyle, element);
			}
			else if (item is IStiTableElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.GetCopyTableStyle((IStiTableElement)item, StiDashboardStyleHelper.GetTableStyle((IStiTableElement)item));
			}
			else if (item is IStiCardsElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportCardsStyle((IStiCardsElement)item);
			}
			else if (item is IStiGaugeElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportGaugeStyle((IStiGaugeElement)item);
			}
			else if (item is IStiPivotTableElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportPivotTableStyle((IStiPivotTableElement)item);
			}
			else if (item is IStiIndicatorElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportIndicatorStyle((IStiIndicatorElement)item);
			}
			else if (item is IStiProgressElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportProgressStyle((IStiProgressElement)item);
			}
			else if (item is IStiRegionMapElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportRegionMapStyle((IStiRegionMapElement)item);
			}
			else if (item is IStiControlElement)
			{
				stiBaseStyle = StiDashboardStyleHelper.ConvertToReportControlStyle((IStiControlElement)item);
			}
			else if (item is StiChart)
			{
				stiBaseStyle = new StiChartStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
			}
			else if (item is StiCrossTab)
			{
				stiBaseStyle = new StiCrossTabStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
			}
			else if (item is StiGauge)
			{
				stiBaseStyle = new StiGaugeStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
			}
			else if (item is StiMap)
			{
				stiBaseStyle = new StiMapStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
			}
			else if (item is StiTable)
			{
				stiBaseStyle = new StiTableStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
			}
			else
			{
				stiBaseStyle = new StiStyle();
				stiBaseStyle.GetStyleFromComponent(item, StiStyleElements.All);
				((StiStyle)stiBaseStyle).AllowUseHorAlignment = false;
				((StiStyle)stiBaseStyle).AllowUseVertAlignment = false;
			}
			stiBaseStyle.Name = GenerateNewStyleName(item.Name + "Style");
			stiBaseStyle.Description = "Style based on formating of '" + item.Name + "' component";
			bool flag = false;
			foreach (StiBaseStyle item2 in list)
			{
				if (item2.Equals(stiBaseStyle, allowEqualName: false, allowEqualDescription: false))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(stiBaseStyle);
			}
		}
		CheckUndoRedo();
		LockUpdateState = true;
		foreach (StiBaseStyle item3 in list)
		{
			AddStyle(item3, null);
		}
		LockUpdateState = false;
		UpdateState();
	}

	private void ProcessStylesCreate()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		StiStylesCreateForm stiStylesCreateForm = new StiStylesCreateForm(this, Report);
		try
		{
			if ((int)((Form)stiStylesCreateForm).ShowDialog((IWin32Window)(object)this) != 1)
			{
				return;
			}
			changed = true;
			if (stiStylesCreateForm.Styles == null || stiStylesCreateForm.Styles.Count <= 0)
			{
				return;
			}
			if (stiStylesCreateForm.checkBoxRemoveExistingStyles.Checked)
			{
				HashAllStyles.Clear();
				HashStyles.Clear();
				hashExpandedCollections.Clear();
				HashStyles.Add(string.Empty, new List<StiBaseStyle>());
				((TreeView)treeViewStyles).Nodes.Clear();
			}
			string collectionName = stiStylesCreateForm.Styles[0].CollectionName;
			if (!HashStyles.ContainsKey(collectionName))
			{
				HashStyles.Add(collectionName, new List<StiBaseStyle>());
			}
			if (!hashExpandedCollections.Contains(collectionName))
			{
				hashExpandedCollections.Add(collectionName);
			}
			foreach (StiBaseStyle style in stiStylesCreateForm.Styles)
			{
				HashAllStyles.Add(style);
				if (string.IsNullOrEmpty(style.CollectionName))
				{
					HashStyles[string.Empty].Add(style);
				}
				else
				{
					HashStyles[style.CollectionName].Add(style);
				}
			}
			Build();
			TreeNode selectedNode = FindGroupNode(collectionName);
			((TreeView)treeViewStyles).SelectedNode = selectedNode;
		}
		finally
		{
			((IDisposable)stiStylesCreateForm)?.Dispose();
		}
	}

	private void UpdateNoStyleState()
	{
		if (noStyleControl == null)
		{
			StiNoStyleControl stiNoStyleControl = new StiNoStyleControl(this);
			((Control)stiNoStyleControl).Anchor = (AnchorStyles)0;
			((Control)stiNoStyleControl).Visible = false;
			noStyleControl = stiNoStyleControl;
			((Control)noStyleControl).Location = new Point((((Control)this).Width - ((Control)noStyleControl).Width) / 2, (((Control)this).Height - ((Control)noStyleControl).Height) / 2);
			((Control)this).Controls.Add((Control)(object)noStyleControl);
			((Control)this).SizeChanged += delegate
			{
				((Control)noStyleControl).Location = new Point((((Control)this).Width - ((Control)noStyleControl).Width) / 2, (((Control)this).Height - ((Control)noStyleControl).Height) / 2);
			};
		}
		if (((TreeView)treeViewStyles).Nodes.Count == 0)
		{
			((Control)noStyleControl).Visible = true;
			((Control)treeViewStyles).Visible = false;
			((Control)panelRight).Visible = false;
		}
		else
		{
			((Control)noStyleControl).Visible = false;
			((Control)treeViewStyles).Visible = true;
			((Control)panelRight).Visible = true;
		}
	}

	private void CreateNewGroupNode(string key)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		List<string> sortCategories = GetSortCategories();
		int num = sortCategories.IndexOf(key);
		TreeNode val = new TreeNode(key)
		{
			Tag = key
		};
		((TreeView)treeViewStyles).Nodes.Insert(num, val);
		val.Expand();
	}

	private void DoScaling()
	{
		btSettings.Image = StiReportDesignImages.Actions.Settings();
		miSortAsc.Image = StiReportDesignImages.Actions.SortAZ();
		miSortDesc.Image = StiReportDesignImages.Actions.SortZA();
		biCardsStyle.Image = StiReportDesignImages.Elements.Cards();
		miShowComponentStyle.Image = StiReportImages.Styles.Style();
		miShowChartStyle.Image = StiReportDesignImages.Elements.Chart();
		miShowCrossTabStyle.Image = StiReportDesignImages.Elements.PivotTable();
		miShowDialogStyle.Image = StiReportDesignImages.Elements.ComboBox();
		miShowGaugeStyle.Image = StiReportDesignImages.Elements.Gauge();
		miShowIndicatorStyle.Image = StiReportDesignImages.Elements.Indicator();
		miShowCardsStyle.Image = StiReportDesignImages.Elements.Cards();
		miShowMapStyle.Image = StiReportDesignImages.Elements.Map();
		miShowProgressStyle.Image = StiReportDesignImages.Elements.Progress();
		miShowTableStyle.Image = StiReportDesignImages.Elements.Table();
		textBoxFindComponent.TextBoxWidth = StiScale.XXI(150.0);
		textBoxFindComponent.TextBoxHeight = StiScale.YYI(16.0);
		((TreeView)treeViewStyles).ItemHeight = StiScale.I(45);
		buttonItemAddStyle.Image = StiReportDesignImages.Actions.NewStyle();
		buttonItemCreateStyleCollection.Image = StiReportDesignImages.Actions.NewStyleCollection();
		buttonItemCut.Image = StiReportDesignImages.Editor.ClipboardCut();
		buttonItemCopy.Image = StiReportDesignImages.Editor.ClipboardCopy();
		buttonItemPaste.Image = StiReportDesignImages.Editor.ClipboardPaste();
		buttonItemDuplicate.Image = StiReportDesignImages.Actions.Duplicate();
		buttonItemDelete.Image = StiReportDesignImages.Actions.Remove();
		buttonItemAddComponentStyle.Image = StiReportDesignImages.Elements.Text();
		buttonItemAddChartStyle.Image = StiReportDesignImages.Elements.Chart();
		buttonItemAddCardsStyle.Image = StiReportDesignImages.Elements.Cards();
		buttonItemAddCrossTableStyle.Image = StiReportDesignImages.Elements.PivotTable();
		buttonItemAddDialogStyle.Image = StiReportDesignImages.Elements.ComboBox();
		buttonItemAddGaugeStyle.Image = StiReportDesignImages.Elements.Gauge();
		buttonItemAddIndicatorStyle.Image = StiReportDesignImages.Elements.Indicator();
		buttonItemAddMapStyle.Image = StiReportDesignImages.Elements.Map();
		buttonItemAddProgressStyle.Image = StiReportDesignImages.Elements.Progress();
		buttonItemAddTableStyle.Image = StiReportDesignImages.Elements.Table();
		tbStyleGet.Image = StiReportDesignImages.Actions.GetStyle();
		tbStyleOpen.Image = StiReportDesignImages.Actions.OpenFile();
		tbStyleSave.Image = StiReportDesignImages.Actions.SaveFile();
		tbStylesCreate.Image = StiReportDesignImages.Actions.NewStyleCollection();
		tbStyleAdd.Image = StiReportDesignImages.Actions.NewStyle();
		tbStyleRemove.Image = StiReportDesignImages.Actions.Remove();
	}

	public static void SetBorder(StiBorder border, StiBorderSides side, bool value)
	{
		if (value)
		{
			border.Side |= side;
		}
		else
		{
			border.Side ^= side;
		}
	}

	private void UpdateState()
	{
		if (((TreeView)treeViewStyles).SelectedNode == null)
		{
			((PropertyGrid)Properties).SelectedObject = null;
			buttonItemDelete.Enabled = false;
			tbStyleRemove.Enabled = false;
			buttonItemCut.Enabled = false;
			buttonItemCopy.Enabled = false;
			buttonItemPaste.Enabled = false;
			buttonItemDuplicate.Enabled = false;
		}
		else if (((TreeView)treeViewStyles).SelectedNode.Tag is StiBaseStyle)
		{
			((PropertyGrid)Properties).SelectedObject = ((TreeView)treeViewStyles).SelectedNode.Tag;
			tbStyleRemove.Enabled = true;
			buttonItemDelete.Enabled = true;
			buttonItemCut.Enabled = true;
			buttonItemCopy.Enabled = true;
			buttonItemPaste.Enabled = true;
			buttonItemDuplicate.Enabled = true;
			ReadStyleSettings();
		}
		else
		{
			((PropertyGrid)Properties).SelectedObject = null;
			buttonItemDelete.Enabled = true;
			tbStyleRemove.Enabled = true;
			buttonItemCut.Enabled = false;
			buttonItemCopy.Enabled = false;
			buttonItemPaste.Enabled = false;
			buttonItemDuplicate.Enabled = false;
		}
		UpdateNoStyleState();
	}

	private void ReadStyleSettings()
	{
		if (((TreeView)treeViewStyles).SelectedNode == null)
		{
			((PropertyGrid)Properties).SelectedObject = null;
		}
		else if (!(((TreeView)treeViewStyles).SelectedNode.Tag is StiStyle))
		{
			((PropertyGrid)Properties).SelectedObject = ((TreeView)treeViewStyles).SelectedNode.Tag;
		}
		else
		{
			((PropertyGrid)Properties).SelectedObject = ((TreeView)treeViewStyles).SelectedNode.Tag;
		}
	}

	private void SelectNext(TreeNode node)
	{
		if (node.Parent == null)
		{
			if (((TreeView)treeViewStyles).Nodes.Count > 1)
			{
				int num = ((TreeView)treeViewStyles).Nodes.IndexOf(node);
				if (num + 1 <= ((TreeView)treeViewStyles).Nodes.Count - 1)
				{
					num++;
				}
				else if (num > 0)
				{
					num--;
				}
				((TreeView)treeViewStyles).SelectedNode = ((TreeView)treeViewStyles).Nodes[num];
			}
			return;
		}
		TreeNode parent = node.Parent;
		if (parent.Nodes.Count > 1)
		{
			int num2 = parent.Nodes.IndexOf(node);
			if (num2 + 1 <= parent.Nodes.Count - 1)
			{
				num2++;
			}
			else if (num2 > 0)
			{
				num2--;
			}
			((TreeView)treeViewStyles).SelectedNode = parent.Nodes[num2];
		}
		else
		{
			((TreeView)treeViewStyles).SelectedNode = parent;
		}
	}

	private void RemoveStyle()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode != null && selectedNode.Tag is StiBaseStyle)
		{
			LockUpdateState = true;
			StiBaseStyle stiBaseStyle = (StiBaseStyle)selectedNode.Tag;
			HashAllStyles.Remove(stiBaseStyle);
			if (string.IsNullOrEmpty(stiBaseStyle.CollectionName))
			{
				HashStyles[string.Empty].Remove(stiBaseStyle);
			}
			else
			{
				HashStyles[stiBaseStyle.CollectionName].Remove(stiBaseStyle);
			}
			TreeNode parent = selectedNode.Parent;
			SelectNext(selectedNode);
			if (selectedNode.Parent == null)
			{
				((TreeView)treeViewStyles).Nodes.Remove(selectedNode);
			}
			else
			{
				selectedNode.Parent.Nodes.Remove(selectedNode);
			}
			if (!string.IsNullOrEmpty(stiBaseStyle.CollectionName) && HashStyles[stiBaseStyle.CollectionName].Count == 0)
			{
				SelectNext(parent);
				((TreeView)treeViewStyles).Nodes.Remove(parent);
				HashStyles.Remove(stiBaseStyle.CollectionName);
			}
			LockUpdateState = false;
			changed = true;
			UpdateState();
		}
	}

	private void RemoveStyleCollection()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode == null || !(selectedNode.Tag is string))
		{
			return;
		}
		LockUpdateState = true;
		string text = (string)selectedNode.Tag;
		SelectNext(selectedNode);
		((TreeView)treeViewStyles).Nodes.Remove(selectedNode);
		int num = 0;
		while (num < HashAllStyles.Count)
		{
			if (HashAllStyles[num].CollectionName == text)
			{
				HashAllStyles.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		HashStyles.Remove(text);
		LockUpdateState = false;
		changed = true;
		UpdateState();
	}

	private void CheckUndoRedo()
	{
		if (!changed)
		{
			changed = true;
			if (designer != null)
			{
				designer.UndoRedoSave(StiLocalization.Get("FormStyleDesigner", "title"));
			}
		}
	}

	private TreeNode FindGroupNode(string key)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		string text = key.ToLowerInvariant();
		foreach (TreeNode node in ((TreeView)treeViewStyles).Nodes)
		{
			TreeNode val = node;
			if (val.Tag is string && ((string)val.Tag).ToLowerInvariant() == text)
			{
				return val;
			}
		}
		return null;
	}

	private string GenerateNewStyleName(string baseName)
	{
		int num = 1;
		string name;
		while (true)
		{
			name = baseName + num;
			StiBaseStyle stiBaseStyle = HashAllStyles.FirstOrDefault((StiBaseStyle x) => x.Name == name);
			if (stiBaseStyle == null)
			{
				break;
			}
			num++;
		}
		return name;
	}

	private string GetCurrentSelectedGroup()
	{
		TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
		if (selectedNode != null)
		{
			if (selectedNode.Tag is string)
			{
				return (string)selectedNode.Tag;
			}
			string collectionName = ((StiBaseStyle)selectedNode.Tag).CollectionName;
			if (!string.IsNullOrEmpty(collectionName))
			{
				return collectionName;
			}
		}
		return string.Empty;
	}

	private void AddStyle(StiBaseStyle style, Type styleType, string baseName = null)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		if (baseName == null)
		{
			baseName = StiLocalization.Get("FormStyleDesigner", "Style");
		}
		if (styleType != null)
		{
			StiStyleBasedHelper.StyleInfo info = StiStyleBasedHelper.GetInfo(styleType);
			if (info != null)
			{
				StiCreateStyleBasedForm stiCreateStyleBasedForm = new StiCreateStyleBasedForm(info);
				((Form)stiCreateStyleBasedForm).Owner = (Form)(object)this;
				StiCreateStyleBasedForm stiCreateStyleBasedForm2 = stiCreateStyleBasedForm;
				if ((int)((Form)stiCreateStyleBasedForm2).ShowDialog() != 1)
				{
					return;
				}
				style = stiCreateStyleBasedForm2.ResultStyle;
			}
		}
		style.PropName = GenerateNewStyleName(baseName);
		string currentSelectedGroup = GetCurrentSelectedGroup();
		HashAllStyles.Add(style);
		Type type = style.GetType();
		if (type == chartStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowChartStyle.Checked = true;
			}
		}
		else if (type == indicatorStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowIndicatorStyle.Checked = true;
			}
		}
		else if (type == progressStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowProgressStyle.Checked = true;
			}
		}
		else if (type == crossTabStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowCrossTabStyle.Checked = true;
			}
		}
		else if (type == dialogStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowDialogStyle.Checked = true;
			}
		}
		else if (type == gaugeStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowGaugeStyle.Checked = true;
			}
		}
		else if (type == mapStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowMapStyle.Checked = true;
			}
		}
		else if (type == componentStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowComponentStyle.Checked = true;
			}
		}
		else if (type == tableStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowTableStyle.Checked = true;
			}
		}
		else if (type == cardsStyleType)
		{
			if (!showStyles.Contains(type))
			{
				miShowCardsStyle.Checked = true;
			}
		}
		else
		{
			foreach (ButtonItem customShowItem in customShowItems)
			{
				if (type == (Type)customShowItem.Tag)
				{
					customShowItem.Checked = true;
					break;
				}
			}
		}
		if (AllowMoveStyles)
		{
			style.CollectionName = currentSelectedGroup;
			TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
			if (selectedNode == null || selectedNode.Tag is string)
			{
				HashStyles[currentSelectedGroup].Add(style);
			}
			else
			{
				StiBaseStyle stiBaseStyle = (StiBaseStyle)selectedNode.Tag;
				if (string.IsNullOrEmpty(stiBaseStyle.CollectionName))
				{
					HashStyles[string.Empty].Add(style);
				}
				else
				{
					HashStyles[currentSelectedGroup].Add(style);
				}
			}
		}
		else
		{
			HashStyles[currentSelectedGroup].Add(style);
		}
		List<StiBaseStyle> list = FilterStyles(HashStyles[currentSelectedGroup]);
		int num = list.IndexOf(style);
		if (num != -1)
		{
			if (currentSelectedGroup == string.Empty)
			{
				num += HashStyles.Count - 1;
			}
			TreeNode val = new TreeNode(style.Name)
			{
				Tag = style
			};
			if (currentSelectedGroup == string.Empty)
			{
				((TreeView)treeViewStyles).Nodes.Insert(num, val);
			}
			else
			{
				TreeNode val2 = FindGroupNode(currentSelectedGroup);
				val2.Nodes.Insert(num, val);
			}
			((TreeView)treeViewStyles).SelectedNode = val;
			changed = true;
			UpdateNoStyleState();
			UpdateState();
		}
	}

	private void FillUsedStylesInReport()
	{
		StiComponentsCollection stiComponentsCollection = Report.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			foreach (object condition in item.Conditions)
			{
				if (condition is StiCondition stiCondition && !string.IsNullOrEmpty(stiCondition.Style) && (!stiCondition.Style.StartsWith("##") || !stiCondition.Style.EndsWith("##")) && !hashUsedStylesInReport.Contains(stiCondition.Style))
				{
					hashUsedStylesInReport.Add(stiCondition.Style);
				}
			}
			if (item is StiCrossTab)
			{
				StiCrossTab stiCrossTab = (StiCrossTab)item;
				if (!string.IsNullOrEmpty(stiCrossTab.CrossTabStyle) && !hashUsedStylesInReport.Contains(stiCrossTab.CrossTabStyle))
				{
					hashUsedStylesInReport.Add(stiCrossTab.CrossTabStyle);
				}
			}
			if (item is StiGauge)
			{
				StiGauge stiGauge = (StiGauge)item;
				if (!string.IsNullOrEmpty(stiGauge.CustomStyleName) && !hashUsedStylesInReport.Contains(stiGauge.CustomStyleName))
				{
					hashUsedStylesInReport.Add(stiGauge.CustomStyleName);
				}
			}
			if (item is StiChart)
			{
				StiChart stiChart = (StiChart)item;
				if (!string.IsNullOrEmpty(stiChart.CustomStyleName) && !hashUsedStylesInReport.Contains(stiChart.CustomStyleName))
				{
					hashUsedStylesInReport.Add(stiChart.CustomStyleName);
				}
			}
			if (!string.IsNullOrEmpty(item.ComponentStyle) && !hashUsedStylesInReport.Contains(item.ComponentStyle))
			{
				hashUsedStylesInReport.Add(item.ComponentStyle);
			}
			if (item is IStiDashboardElementStyle)
			{
				IStiDashboardElementStyle stiDashboardElementStyle = (IStiDashboardElementStyle)item;
				if (stiDashboardElementStyle.Style == StiElementStyleIdent.Custom && !string.IsNullOrEmpty(stiDashboardElementStyle.CustomStyleName) && !hashUsedStylesInReport.Contains(stiDashboardElementStyle.CustomStyleName))
				{
					hashUsedStylesInReport.Add(stiDashboardElementStyle.CustomStyleName);
				}
			}
			if (item is IStiOddEvenStyles stiOddEvenStyles)
			{
				if (!string.IsNullOrEmpty(stiOddEvenStyles.OddStyle) && !hashUsedStylesInReport.Contains(stiOddEvenStyles.OddStyle))
				{
					hashUsedStylesInReport.Add(stiOddEvenStyles.OddStyle);
				}
				if (!string.IsNullOrEmpty(stiOddEvenStyles.EvenStyle) && !hashUsedStylesInReport.Contains(stiOddEvenStyles.EvenStyle))
				{
					hashUsedStylesInReport.Add(stiOddEvenStyles.EvenStyle);
				}
			}
		}
	}

	private void PrepareStyles()
	{
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		HashStyles.Add(string.Empty, list);
		foreach (StiBaseStyle style in Report.Styles)
		{
			StiBaseStyle stiBaseStyle2 = (StiBaseStyle)style.Clone();
			HashAllStyles.Add(stiBaseStyle2);
			if (string.IsNullOrEmpty(stiBaseStyle2.CollectionName))
			{
				list.Add(stiBaseStyle2);
				continue;
			}
			List<StiBaseStyle> value = null;
			HashStyles.TryGetValue(stiBaseStyle2.CollectionName, out value);
			if (value == null)
			{
				value = new List<StiBaseStyle>();
				HashStyles.Add(stiBaseStyle2.CollectionName, value);
			}
			value.Add(stiBaseStyle2);
		}
	}

	private bool AllowShowingStyle(Type type)
	{
		return showStyles.Contains(type);
	}

	private List<StiBaseStyle> FilterStyles(List<StiBaseStyle> list)
	{
		IEnumerable<StiBaseStyle> source = list.Where((StiBaseStyle x) => AllowShowingStyle(x.GetType()));
		if (findText != null)
		{
			string lowerFindText = findText.ToLowerInvariant();
			source = source.Where((StiBaseStyle x) => x.Name.ToLowerInvariant().Contains(lowerFindText));
		}
		return sortType switch
		{
			StiDictionarySortOrder.Asc => source.OrderBy((StiBaseStyle x) => x.Name).ToList(), 
			StiDictionarySortOrder.Desc => source.OrderByDescending((StiBaseStyle x) => x.Name).ToList(), 
			_ => source.ToList(), 
		};
	}

	private List<string> GetSortCategories()
	{
		List<string> list = HashStyles.Keys.ToList();
		list.Remove(string.Empty);
		return sortType switch
		{
			StiDictionarySortOrder.Asc => list.OrderBy((string x) => x).ToList(), 
			StiDictionarySortOrder.Desc => list.OrderByDescending((string x) => x).ToList(), 
			_ => list, 
		};
	}

	private void Build()
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		LockUpdateState = true;
		if (((TreeView)treeViewStyles).SelectedNode != null)
		{
			selectedNodeTag = ((TreeView)treeViewStyles).SelectedNode.Tag;
		}
		((TreeView)treeViewStyles).Nodes.Clear();
		List<string> sortCategories = GetSortCategories();
		if (sortCategories.Count > 0)
		{
			foreach (string item in sortCategories)
			{
				List<StiBaseStyle> list = FilterStyles(HashStyles[item]);
				if (list.Count <= 0)
				{
					continue;
				}
				TreeNode val = new TreeNode(item)
				{
					Tag = item
				};
				((TreeView)treeViewStyles).Nodes.Add(val);
				if (selectedNodeTag != null && selectedNodeTag.Equals(item))
				{
					selectedNodeTag = null;
					((TreeView)treeViewStyles).SelectedNode = val;
				}
				foreach (StiBaseStyle item2 in list)
				{
					TreeNode val2 = new TreeNode(item2.Name)
					{
						Tag = item2
					};
					val.Nodes.Add(val2);
					if (selectedNodeTag != null && selectedNodeTag.Equals(item2))
					{
						selectedNodeTag = null;
						((TreeView)treeViewStyles).SelectedNode = val2;
					}
				}
				if (hashExpandedCollections.Contains(item) || !string.IsNullOrEmpty(findText))
				{
					lockExpand = true;
					val.Expand();
					lockExpand = false;
				}
			}
		}
		List<StiBaseStyle> list2 = FilterStyles(HashStyles[string.Empty]);
		if (list2.Count > 0)
		{
			foreach (StiBaseStyle item3 in list2)
			{
				TreeNode val3 = new TreeNode(item3.Name)
				{
					Tag = item3
				};
				((TreeView)treeViewStyles).Nodes.Add(val3);
				if (selectedNodeTag != null && selectedNodeTag.Equals(item3))
				{
					selectedNodeTag = null;
					((TreeView)treeViewStyles).SelectedNode = val3;
				}
			}
		}
		LockUpdateState = false;
		selectedNodeTag = null;
		UpdateState();
	}

	internal void ProcessDropStyleTo(TreeNode dragNode, TreeNode dropTo, bool isUp)
	{
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		StiBaseStyle stiBaseStyle = (StiBaseStyle)dragNode.Tag;
		if (string.IsNullOrEmpty(stiBaseStyle.CollectionName))
		{
			HashStyles[string.Empty].Remove(stiBaseStyle);
		}
		else
		{
			List<StiBaseStyle> list = HashStyles[stiBaseStyle.CollectionName];
			list.Remove(stiBaseStyle);
			if (list.Count == 0 && (!(dropTo.Tag is string) || !((string)dropTo.Tag == stiBaseStyle.CollectionName)))
			{
				TreeNode val = FindGroupNode(stiBaseStyle.CollectionName);
				((TreeView)treeViewStyles).Nodes.Remove(val);
				HashStyles.Remove(stiBaseStyle.CollectionName);
			}
		}
		stiBaseStyle.CollectionName = string.Empty;
		if (dropTo == null || (dropTo.Tag is StiBaseStyle && string.IsNullOrEmpty(((StiBaseStyle)dropTo.Tag).CollectionName)))
		{
			List<StiBaseStyle> list2 = HashStyles[string.Empty];
			if (dropTo != null)
			{
				int num = list2.IndexOf((StiBaseStyle)dropTo.Tag);
				if (!isUp)
				{
					num++;
				}
				list2.Insert(num, stiBaseStyle);
			}
			else
			{
				list2.Add(stiBaseStyle);
			}
			int num2 = list2.IndexOf(stiBaseStyle) + (HashStyles.Count - 1);
			TreeNode val2 = new TreeNode(stiBaseStyle.Name)
			{
				Tag = stiBaseStyle
			};
			((TreeView)treeViewStyles).Nodes.Insert(num2, val2);
			((TreeView)treeViewStyles).SelectedNode = val2;
		}
		else if (dropTo.Tag is string)
		{
			string text = (string)dropTo.Tag;
			List<StiBaseStyle> list3 = HashStyles[text];
			list3.Add(stiBaseStyle);
			int num3 = list3.IndexOf(stiBaseStyle);
			stiBaseStyle.CollectionName = text;
			TreeNode val3 = FindGroupNode(text);
			TreeNode val4 = new TreeNode(stiBaseStyle.Name)
			{
				Tag = stiBaseStyle
			};
			val3.Nodes.Insert(num3, val4);
			((TreeView)treeViewStyles).SelectedNode = val4;
		}
		else
		{
			StiBaseStyle stiBaseStyle2 = (StiBaseStyle)dropTo.Tag;
			string collectionName = stiBaseStyle2.CollectionName;
			List<StiBaseStyle> list4 = HashStyles[collectionName];
			int num4 = list4.IndexOf(stiBaseStyle2);
			if (!isUp)
			{
				num4++;
			}
			stiBaseStyle.CollectionName = collectionName;
			list4.Insert(num4, stiBaseStyle);
			TreeNode val5 = new TreeNode(stiBaseStyle.Name)
			{
				Tag = stiBaseStyle
			};
			TreeNode val6 = FindGroupNode(collectionName);
			val6.Nodes.Insert(num4, val5);
			((TreeView)treeViewStyles).SelectedNode = val5;
		}
		UpdateState();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (designer.Report != null)
		{
			designer.Report.IsModified = true;
		}
		StiStylesCollection styles = Report.Styles;
		styles.Clear();
		designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Styles"));
		foreach (KeyValuePair<string, List<StiBaseStyle>> hashStyle in HashStyles)
		{
			if (hashStyle.Key == string.Empty)
			{
				continue;
			}
			foreach (StiBaseStyle item in hashStyle.Value)
			{
				styles.Add(item);
			}
		}
		foreach (StiBaseStyle item2 in HashStyles[string.Empty])
		{
			styles.Add(item2);
		}
		if (hashChangeName.Count > 0)
		{
			StiComponentsCollection stiComponentsCollection = Report.GetComponents();
			foreach (StiStyleDesignerChangeName item3 in hashChangeName)
			{
				foreach (StiComponent item4 in stiComponentsCollection)
				{
					StiStylesHelper.ChangeComponentStyleName(item4, item3.OldName, item3.NewName);
				}
			}
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_appearance_style_designer.htm");
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiStyleDesigner", "Spl", ((Control)panelRight).Width);
		StiSettings.Set("StiStyleDesigner", "Sort", (int)sortType);
		StiSettings.Save();
		Collect();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13 && textBoxFindComponent.Focused)
		{
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			if (textBoxFindComponent.Text.Length > 0)
			{
				if (ProcessCleanFind())
				{
					Build();
				}
			}
			else
			{
				((Form)this).Close();
			}
		}
		else
		{
			if (!((Control)treeViewStyles).Focused)
			{
				return;
			}
			if ((Control.ModifierKeys & 0x10000) > 0)
			{
				Keys keyCode = e.KeyCode;
				if ((int)keyCode != 38)
				{
					if ((int)keyCode != 40)
					{
						if ((int)keyCode == 45)
						{
							e.Handled = true;
							ProcessStylePaste();
						}
					}
					else
					{
						e.Handled = true;
						ProcessMoveDown();
					}
				}
				else
				{
					e.Handled = true;
					ProcessMoveUp();
				}
			}
			else
			{
				if ((Control.ModifierKeys & 0x20000) <= 0)
				{
					return;
				}
				Keys keyCode2 = e.KeyCode;
				if ((int)keyCode2 <= 67)
				{
					if ((int)keyCode2 != 45)
					{
						if ((int)keyCode2 == 67)
						{
							e.Handled = true;
							ProcessStyleCopy();
						}
					}
					else
					{
						e.Handled = true;
						ProcessStyleCopy();
					}
				}
				else if ((int)keyCode2 != 86)
				{
					if ((int)keyCode2 == 88)
					{
						e.Handled = true;
						ProcessStyleCut();
					}
				}
				else
				{
					e.Handled = true;
					ProcessStylePaste();
				}
			}
		}
	}

	private void ButtonAddCustomStyle_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = (ButtonItem)sender;
		ProcessNewCustomStyle((Type)buttonItem.Tag);
	}

	private void MenuItemAddCustomStyle_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = (ButtonItem)sender;
		ProcessNewCustomStyle((Type)buttonItem.Tag);
	}

	private void ButtonAddComponentStyle_Click(object sender, EventArgs e)
	{
		ProcessNewComponentStyle();
	}

	private void ButtonAddMapStyle_Click(object sender, EventArgs e)
	{
		ProcessNewMapStyle();
	}

	private void ButtonAddTableStyle_Click(object sender, EventArgs e)
	{
		ProcessNewTableStyle();
	}

	private void ButtonAddCardsStyle_Click(object sender, EventArgs e)
	{
		ProcessNewCardsStyle();
	}

	private void ButtonAddChartStyle_Click(object sender, EventArgs e)
	{
		ProcessNewChartStyle();
	}

	private void ButtonAddIndicatorStyle_Click(object sender, EventArgs e)
	{
		ProcessNewIndicatorStyle();
	}

	private void ButtonAddProgressStyle_Click(object sender, EventArgs e)
	{
		ProcessNewProgressStyle();
	}

	private void ButtonAddGaugeStyle_Click(object sender, EventArgs e)
	{
		ProcessNewGaugeStyle();
	}

	private void ButtonAddCrossTabStyle_Click(object sender, EventArgs e)
	{
		ProcessNewCrossTabStyle();
	}

	private void ButtonAddDialogStyle_Click(object sender, EventArgs e)
	{
		ProcessNewDialogStyle();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		ProcessRemove();
	}

	private void ButtonStyleDuplicate_Click(object sender, EventArgs e)
	{
		ProcessStyleDuplicate();
	}

	private void ButtonStyleCut_Click(object sender, EventArgs e)
	{
		ProcessStyleCut();
	}

	private void ButtonStyleCopy_Click(object sender, EventArgs e)
	{
		ProcessStyleCopy();
	}

	private void ButtonStylePaste_Click(object sender, EventArgs e)
	{
		ProcessStylePaste();
	}

	private void ButtonStyleSave_Click(object sender, EventArgs e)
	{
		ProcessStyleSave();
	}

	private void ButtonStyleOpen_Click(object sender, EventArgs e)
	{
		ProcessStyleOpen();
	}

	private void ButtonStyleGet_Click(object sender, EventArgs e)
	{
		ProcessStyleGet();
	}

	private void ButtonStylesCreate_Click(object sender, EventArgs e)
	{
		ProcessStylesCreate();
	}

	private void Properties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		CheckUndoRedo();
		((Control)treeViewStyles).Invalidate();
		switch (e.ChangedItem.PropertyDescriptor.Name)
		{
		case "Border":
			ReadStyleSettings();
			UpdateState();
			break;
		case "Name":
		{
			bool flag = true;
			StiBaseStyle style = (StiBaseStyle)((TreeView)treeViewStyles).SelectedNode.Tag;
			string newName = e.ChangedItem.Value as string;
			if (string.IsNullOrEmpty(newName) || HashAllStyles.FirstOrDefault((StiBaseStyle x) => x != style && x.Name == newName) != null)
			{
				flag = false;
			}
			if (flag)
			{
				string oldName = (string)e.OldValue;
				StiStyleDesignerChangeName stiStyleDesignerChangeName = hashChangeName.FirstOrDefault((StiStyleDesignerChangeName x) => x.NewName == oldName);
				if (stiStyleDesignerChangeName != null)
				{
					stiStyleDesignerChangeName.NewName = newName;
				}
				else
				{
					hashChangeName.Add(new StiStyleDesignerChangeName(oldName, newName));
				}
			}
			else
			{
				style.Name = (string)e.OldValue;
			}
			break;
		}
		case "CollectionName":
		{
			if (((TreeView)treeViewStyles).SelectedNode == null)
			{
				break;
			}
			TreeNode selectedNode = ((TreeView)treeViewStyles).SelectedNode;
			StiBaseStyle stiBaseStyle = (StiBaseStyle)selectedNode.Tag;
			TreeNode parent = ((TreeView)treeViewStyles).SelectedNode.Parent;
			if (parent != null)
			{
				parent.Nodes.Remove(selectedNode);
			}
			else
			{
				((TreeView)treeViewStyles).Nodes.Remove(selectedNode);
			}
			foreach (KeyValuePair<string, List<StiBaseStyle>> hashStyle in HashStyles)
			{
				if (hashStyle.Value.Contains(stiBaseStyle))
				{
					hashStyle.Value.Remove(stiBaseStyle);
					break;
				}
			}
			if (string.IsNullOrEmpty(stiBaseStyle.CollectionName))
			{
				List<StiBaseStyle> list = HashStyles[string.Empty];
				list.Add(stiBaseStyle);
				List<StiBaseStyle> list2 = FilterStyles(list);
				int num = list2.IndexOf(stiBaseStyle) + (HashStyles.Count - 1);
				TreeNode val = new TreeNode(stiBaseStyle.Name)
				{
					Tag = stiBaseStyle
				};
				((TreeView)treeViewStyles).Nodes.Insert(num, val);
				((TreeView)treeViewStyles).SelectedNode = val;
				break;
			}
			List<StiBaseStyle> list3 = null;
			foreach (KeyValuePair<string, List<StiBaseStyle>> hashStyle2 in HashStyles)
			{
				if (hashStyle2.Key.ToLowerInvariant() == stiBaseStyle.CollectionName.ToLowerInvariant())
				{
					list3 = hashStyle2.Value;
					break;
				}
			}
			if (list3 == null)
			{
				list3 = new List<StiBaseStyle>();
				list3.Add(stiBaseStyle);
				HashStyles.Add(stiBaseStyle.CollectionName, list3);
				CreateNewGroupNode(stiBaseStyle.CollectionName);
			}
			else
			{
				list3.Add(stiBaseStyle);
			}
			List<StiBaseStyle> list4 = FilterStyles(list3);
			int num2 = list4.IndexOf(stiBaseStyle);
			TreeNode val2 = new TreeNode(stiBaseStyle.Name)
			{
				Tag = stiBaseStyle
			};
			TreeNode val3 = FindGroupNode(stiBaseStyle.CollectionName);
			val3.Nodes.Insert(num2, val2);
			((TreeView)treeViewStyles).SelectedNode = val2;
			break;
		}
		}
	}

	private void Properties_Enter(object sender, EventArgs e)
	{
		tbStyleRemove.Shortcuts.Clear();
	}

	private void Properties_Leave(object sender, EventArgs e)
	{
		tbStyleRemove.Shortcuts.Add(eShortcut.Del);
	}

	private void TreeViewStyles_SizeChanged(object sender, EventArgs e)
	{
		((Control)treeViewStyles).Invalidate();
	}

	private void TreeViewStyles_AfterExpand(object sender, TreeViewEventArgs e)
	{
		if (lockExpand || !(e.Node.Tag is string item))
		{
			return;
		}
		if (e.Node.IsExpanded)
		{
			if (!hashExpandedCollections.Contains(item))
			{
				hashExpandedCollections.Add(item);
			}
		}
		else if (hashExpandedCollections.Contains(item))
		{
			hashExpandedCollections.Remove(item);
		}
	}

	private void TreeViewStyles_DrawNode(object sender, DrawTreeNodeEventArgs e)
	{
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Invalid comparison between Unknown and I4
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Invalid comparison between Unknown and I4
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		e.DrawDefault = false;
		TreeNode node = e.Node;
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		if (node.Nodes.Count > 0)
		{
			Rectangle rectangle = new Rectangle(bounds.X + StiScale.I8, bounds.Y + (bounds.Height - StiScale.I9) / 2, StiScale.I9, StiScale.I9);
			if (e.Node.IsExpanded)
			{
				graphics.DrawImage(treeViewStyles.CollapsedIcon, rectangle);
			}
			else
			{
				graphics.DrawImage(treeViewStyles.ExpandedIcon, rectangle);
			}
			bounds.X += StiScale.I(26);
			bounds.Width -= StiScale.I(26);
		}
		bounds.X += StiScale.I2;
		bounds.Width -= StiScale.I2;
		if (node.Parent != null)
		{
			bounds.X += StiScale.I20;
			bounds.Width -= StiScale.I20;
		}
		if (e.Node.Tag is string)
		{
			Rectangle rectangle2 = new Rectangle(bounds.X + StiScale.I4, bounds.Y + StiScale.I5, StiScale.I(32), StiScale.I(32));
			if (e.Node.IsExpanded)
			{
				graphics.DrawImage(treeViewStyles.FolderIconOpen, rectangle2);
			}
			else
			{
				graphics.DrawImage(treeViewStyles.FolderIcon, rectangle2);
			}
			string text = (string)e.Node.Tag;
			StringFormat val = new StringFormat();
			try
			{
				val.LineAlignment = (StringAlignment)1;
				Brush val2 = (((e.State & 1) > 0) ? Brushes.White : Brushes.Black);
				graphics.DrawString(text, ((Control)treeViewStyles).Font, val2, new RectangleF(bounds.X + StiScale.I(38), bounds.Y, bounds.Width, bounds.Height), val);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		StiBaseStyle stiBaseStyle = (StiBaseStyle)e.Node.Tag;
		StiBaseStyle.DrawStyle(graphics, bounds, stiBaseStyle, (DrawItemState)(((e.State & 1) > 0) ? 1 : 32), fillBackground: false, paintImage: true);
		if (hashUsedStylesInReport != null && hashUsedStylesInReport.Contains(stiBaseStyle.Name))
		{
			Rectangle rectangle3 = new Rectangle(bounds.Right - StiScale.I(24), bounds.Y + (bounds.Height - StiScale.I12) / 2, StiScale.I12, StiScale.I12);
			graphics.FillEllipse(Brushes.White, rectangle3);
			rectangle3.X += StiScale.I2;
			rectangle3.Y += StiScale.I2;
			rectangle3.Width -= StiScale.I4;
			rectangle3.Height -= StiScale.I4;
			SolidBrush val3 = new SolidBrush(Color.FromArgb(255, 34, 118, 213));
			try
			{
				graphics.FillEllipse((Brush)(object)val3, rectangle3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
	}

	private void TreeViewStyles_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (!LockUpdateState && !dragDropAction.IsDragDropActive)
		{
			UpdateState();
		}
	}

	private void MenuItemSortAsc_Click(object sender, EventArgs e)
	{
		sortType = StiDictionarySortOrder.Asc;
		miSortAsc.Checked = true;
		miSortDesc.Checked = false;
		miSortNone.Checked = false;
		Build();
	}

	private void MenuItemSortDesc_Click(object sender, EventArgs e)
	{
		sortType = StiDictionarySortOrder.Desc;
		miSortAsc.Checked = false;
		miSortDesc.Checked = true;
		miSortNone.Checked = false;
		Build();
	}

	private void MenuItemSortNone_Click(object sender, EventArgs e)
	{
		sortType = StiDictionarySortOrder.None;
		miSortAsc.Checked = false;
		miSortDesc.Checked = false;
		miSortNone.Checked = true;
		Build();
	}

	private void MenuitemShowStyle_CheckedChanged(object sender, EventArgs e)
	{
		ButtonItem buttonItem = (ButtonItem)sender;
		Type type = (Type)buttonItem.Tag;
		if (buttonItem.Checked)
		{
			if (!showStyles.Contains(type))
			{
				showStyles.Add(type);
			}
		}
		else if (showStyles.Contains(type))
		{
			showStyles.Remove(type);
		}
		if (ContainsStyles(type))
		{
			Build();
		}
	}

	private void TextBoxFindComponent_LostFocus(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
	}

	private void TextBoxFindComponent_GotFocus(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = null;
	}

	private void TextBoxFindComponent_TextChanged(object sender, EventArgs e)
	{
		if (ProcessFind())
		{
			Build();
		}
	}

	private void TextBoxFindComponent_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			if (ProcessCleanFind())
			{
				Build();
			}
		}
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Expected O, but got Unknown
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Expected O, but got Unknown
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Expected O, but got Unknown
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Expected O, but got Unknown
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Expected O, but got Unknown
		//IL_0e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5f: Expected O, but got Unknown
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Expected O, but got Unknown
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Expected O, but got Unknown
		//IL_0f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f58: Expected O, but got Unknown
		//IL_0fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Expected O, but got Unknown
		//IL_0ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffe: Expected O, but got Unknown
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Expected O, but got Unknown
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a4: Expected O, but got Unknown
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Expected O, but got Unknown
		//IL_1140: Unknown result type (might be due to invalid IL or missing references)
		//IL_114a: Expected O, but got Unknown
		//IL_1d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0d: Expected O, but got Unknown
		//IL_1e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5a: Expected O, but got Unknown
		//IL_1e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e71: Expected O, but got Unknown
		//IL_1f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4c: Expected O, but got Unknown
		//IL_20bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c6: Expected O, but got Unknown
		//IL_212b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2135: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiStyleDesigner));
		ilStyles = new ImageList(components);
		toolTip1 = new ToolTip(components);
		glButtons = new StiGroupLine();
		btCancel = new Button();
		btOk = new Button();
		panelRight = new Panel();
		tbStyleOpen = new ButtonItem();
		tbStyleSave = new ButtonItem();
		tbStyleAdd = new ButtonItem();
		biComponentStyle = new ButtonItem();
		biChartStyle = new ButtonItem();
		biGaugeStyle = new ButtonItem();
		biMapStyle = new ButtonItem();
		biCrossTabStyle = new ButtonItem();
		biTableStyle = new ButtonItem();
		biCardsStyle = new ButtonItem();
		biIndicatorStyle = new ButtonItem();
		biProgressStyle = new ButtonItem();
		biDialogStyle = new ButtonItem();
		tbActions = new ButtonItem();
		tbStylesCreate = new ButtonItem();
		tbStyleRemove = new ButtonItem();
		tbStyleGet = new ButtonItem();
		bar1 = new Bar();
		btSettings = new ButtonItem();
		miHeaderFilter = new LabelItem();
		miShowComponentStyle = new ButtonItem();
		miShowChartStyle = new ButtonItem();
		miShowGaugeStyle = new ButtonItem();
		miShowMapStyle = new ButtonItem();
		miShowCrossTabStyle = new ButtonItem();
		miShowTableStyle = new ButtonItem();
		miShowCardsStyle = new ButtonItem();
		miShowIndicatorStyle = new ButtonItem();
		miShowProgressStyle = new ButtonItem();
		miShowDialogStyle = new ButtonItem();
		miHeaderSort = new LabelItem();
		miSortAsc = new ButtonItem();
		miSortDesc = new ButtonItem();
		miSortNone = new ButtonItem();
		textBoxFindComponent = new TextBoxItem();
		stiGroupLine1 = new StiGroupLine();
		DotNetBarManager = new DotNetBarManager(components);
		DockSiteBottom = new DockSite();
		DockSiteLeft = new DockSite();
		DockSiteRight = new DockSite();
		dockSite4 = new DockSite();
		dockSite1 = new DockSite();
		dockSite2 = new DockSite();
		dockSite3 = new DockSite();
		DockSiteTop = new DockSite();
		contextMenuStyles = new ButtonItem();
		buttonItemAddStyle = new ButtonItem();
		buttonItemAddComponentStyle = new ButtonItem();
		buttonItemAddChartStyle = new ButtonItem();
		buttonItemAddGaugeStyle = new ButtonItem();
		buttonItemAddMapStyle = new ButtonItem();
		buttonItemAddCrossTableStyle = new ButtonItem();
		buttonItemAddTableStyle = new ButtonItem();
		buttonItemAddCardsStyle = new ButtonItem();
		buttonItemAddIndicatorStyle = new ButtonItem();
		buttonItemAddProgressStyle = new ButtonItem();
		buttonItemAddDialogStyle = new ButtonItem();
		buttonItemDuplicate = new ButtonItem();
		buttonItemCut = new ButtonItem();
		buttonItemCopy = new ButtonItem();
		buttonItemPaste = new ButtonItem();
		buttonItemDelete = new ButtonItem();
		buttonItemCreateStyleCollection = new ButtonItem();
		contextMenuBar = new ContextMenuBar();
		treeViewStyles = new StiStylesTreeView();
		Properties = new StiReportPropertyGrid();
		((Control)glButtons).SuspendLayout();
		((Control)panelRight).SuspendLayout();
		((ISupportInitialize)bar1).BeginInit();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)this).SuspendLayout();
		ilStyles.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilStyles.ImageStream");
		ilStyles.TransparentColor = Color.Magenta;
		ilStyles.Images.SetKeyName(0, "");
		ilStyles.Images.SetKeyName(1, "");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 614);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(784, 48);
		((Control)glButtons).TabIndex = 1;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Control)btCancel).BackColor = Color.White;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(695, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).BackColor = Color.White;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(614, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)panelRight).Anchor = (AnchorStyles)11;
		((Control)panelRight).Controls.Add((Control)(object)Properties);
		((Control)panelRight).Location = new Point(460, 46);
		((Control)panelRight).Name = "panelRight";
		((Control)panelRight).Size = new Size(309, 554);
		((Control)panelRight).TabIndex = 18;
		tbStyleOpen.BeginGroup = true;
		tbStyleOpen.ButtonStyle = eButtonStyle.ImageAndText;
		tbStyleOpen.Name = "tbStyleOpen";
		tbStyleOpen.Click += ButtonStyleOpen_Click;
		tbStyleSave.Name = "tbStyleSave";
		tbStyleSave.Click += ButtonStyleSave_Click;
		tbStyleAdd.AutoExpandOnClick = true;
		tbStyleAdd.ButtonStyle = eButtonStyle.ImageAndText;
		tbStyleAdd.Name = "tbStyleAdd";
		tbStyleAdd.SubItems.AddRange(new BaseItem[10] { biComponentStyle, biChartStyle, biGaugeStyle, biMapStyle, biCrossTabStyle, biTableStyle, biCardsStyle, biIndicatorStyle, biProgressStyle, biDialogStyle });
		biComponentStyle.Image = (Image)componentResourceManager.GetObject("biComponentStyle.Image");
		biComponentStyle.Name = "biComponentStyle";
		biComponentStyle.Text = "Component";
		biComponentStyle.Click += ButtonAddComponentStyle_Click;
		biChartStyle.Image = (Image)componentResourceManager.GetObject("biChartStyle.Image");
		biChartStyle.Name = "biChartStyle";
		biChartStyle.Text = "Chart";
		biChartStyle.Click += ButtonAddChartStyle_Click;
		biGaugeStyle.Image = (Image)componentResourceManager.GetObject("biGaugeStyle.Image");
		biGaugeStyle.Name = "biGaugeStyle";
		biGaugeStyle.Text = "Gauge";
		biGaugeStyle.Click += ButtonAddGaugeStyle_Click;
		biMapStyle.Image = (Image)componentResourceManager.GetObject("biMapStyle.Image");
		biMapStyle.Name = "biMapStyle";
		biMapStyle.Text = "Map";
		biMapStyle.Click += ButtonAddMapStyle_Click;
		biCrossTabStyle.Image = (Image)componentResourceManager.GetObject("biCrossTabStyle.Image");
		biCrossTabStyle.Name = "biCrossTabStyle";
		biCrossTabStyle.Text = "Cross-Tab";
		biCrossTabStyle.Click += ButtonAddCrossTabStyle_Click;
		biTableStyle.Image = (Image)componentResourceManager.GetObject("biTableStyle.Image");
		biTableStyle.Name = "biTableStyle";
		biTableStyle.Text = "Table";
		biTableStyle.Click += ButtonAddTableStyle_Click;
		biCardsStyle.Image = (Image)componentResourceManager.GetObject("biTableStyle.Image");
		biCardsStyle.Name = "biCardsStyle";
		biCardsStyle.Text = "Cards";
		biCardsStyle.Click += ButtonAddCardsStyle_Click;
		biIndicatorStyle.Image = (Image)componentResourceManager.GetObject("biIndicatorStyle.Image");
		biIndicatorStyle.Name = "biIndicatorStyle";
		biIndicatorStyle.Text = "Indicator";
		biIndicatorStyle.Click += ButtonAddIndicatorStyle_Click;
		biProgressStyle.Image = (Image)componentResourceManager.GetObject("biProgressStyle.Image");
		biProgressStyle.Name = "biProgressStyle";
		biProgressStyle.Text = "Progress";
		biProgressStyle.Click += ButtonAddProgressStyle_Click;
		biDialogStyle.Image = (Image)componentResourceManager.GetObject("biDialogStyle.Image");
		biDialogStyle.Name = "biDialogStyle";
		biDialogStyle.Text = "Dialog";
		biDialogStyle.Click += ButtonAddDialogStyle_Click;
		tbActions.AutoExpandOnClick = true;
		tbActions.Name = "tbActions";
		tbActions.SubItems.AddRange(new BaseItem[3] { tbStyleOpen, tbStyleSave, tbStylesCreate });
		tbActions.Text = "Actions";
		tbStylesCreate.BeginGroup = true;
		tbStylesCreate.ButtonStyle = eButtonStyle.ImageAndText;
		tbStylesCreate.Image = (Image)componentResourceManager.GetObject("tbStylesCreate.Image");
		tbStylesCreate.Name = "tbStylesCreate";
		tbStylesCreate.Click += ButtonStylesCreate_Click;
		tbStyleRemove.Name = "tbStyleRemove";
		tbStyleRemove.Shortcuts.Add(eShortcut.Del);
		tbStyleRemove.Click += ButtonRemove_Click;
		tbStyleGet.AutoExpandOnClick = true;
		tbStyleGet.BeginGroup = true;
		tbStyleGet.Name = "tbStyleGet";
		tbStyleGet.Click += ButtonStyleGet_Click;
		((Control)bar1).AccessibleDescription = "bar1 (bar1)";
		((Control)bar1).AccessibleName = "bar1";
		((Control)bar1).AccessibleRole = (AccessibleRole)22;
		bar1.BarType = eBarType.MenuBar;
		((Control)bar1).Dock = (DockStyle)1;
		((Control)bar1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		bar1.Items.AddRange(new BaseItem[6] { tbActions, tbStyleAdd, tbStyleRemove, tbStyleGet, btSettings, textBoxFindComponent });
		bar1.Location = new Point(0, 0);
		((Control)bar1).MinimumSize = new Size(0, 30);
		bar1.Name = "bar1";
		bar1.PaddingLeft = 5;
		bar1.PaddingTop = 5;
		bar1.RoundCorners = false;
		bar1.Size = new Size(784, 30);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.StyleManagerControlled;
		bar1.TabIndex = 19;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		btSettings.AutoExpandOnClick = true;
		btSettings.BeginGroup = true;
		btSettings.Name = "btSettings";
		btSettings.SubItems.AddRange(new BaseItem[15]
		{
			miHeaderFilter, miShowComponentStyle, miShowChartStyle, miShowGaugeStyle, miShowMapStyle, miShowCrossTabStyle, miShowTableStyle, miShowCardsStyle, miShowIndicatorStyle, miShowProgressStyle,
			miShowDialogStyle, miHeaderSort, miSortAsc, miSortDesc, miSortNone
		});
		miHeaderFilter.BackColor = Color.FromArgb(238, 238, 238);
		miHeaderFilter.ForeColor = Color.FromArgb(136, 136, 136);
		miHeaderFilter.Height = 24;
		miHeaderFilter.Name = "miHeaderFilter";
		miHeaderFilter.Text = "Filter";
		miShowComponentStyle.AutoCheckOnClick = true;
		miShowComponentStyle.AutoCollapseOnClick = false;
		miShowComponentStyle.Image = (Image)componentResourceManager.GetObject("miShowComponentStyle.Image");
		miShowComponentStyle.Name = "miShowComponentStyle";
		miShowComponentStyle.Text = "Style";
		miShowChartStyle.AutoCheckOnClick = true;
		miShowChartStyle.AutoCollapseOnClick = false;
		miShowChartStyle.Image = (Image)componentResourceManager.GetObject("miShowChartStyle.Image");
		miShowChartStyle.Name = "miShowChartStyle";
		miShowChartStyle.Text = "Chart";
		miShowGaugeStyle.AutoCheckOnClick = true;
		miShowGaugeStyle.AutoCollapseOnClick = false;
		miShowGaugeStyle.Image = (Image)componentResourceManager.GetObject("miShowGaugeStyle.Image");
		miShowGaugeStyle.Name = "miShowGaugeStyle";
		miShowGaugeStyle.Text = "Gauge";
		miShowMapStyle.AutoCheckOnClick = true;
		miShowMapStyle.AutoCollapseOnClick = false;
		miShowMapStyle.Image = (Image)componentResourceManager.GetObject("miShowMapStyle.Image");
		miShowMapStyle.Name = "miShowMapStyle";
		miShowMapStyle.Text = "Map";
		miShowCrossTabStyle.AutoCheckOnClick = true;
		miShowCrossTabStyle.AutoCollapseOnClick = false;
		miShowCrossTabStyle.Image = (Image)componentResourceManager.GetObject("miShowCrossTabStyle.Image");
		miShowCrossTabStyle.Name = "miShowCrossTabStyle";
		miShowCrossTabStyle.Text = "CrossTab";
		miShowTableStyle.AutoCheckOnClick = true;
		miShowTableStyle.AutoCollapseOnClick = false;
		miShowTableStyle.Image = (Image)componentResourceManager.GetObject("miShowTableStyle.Image");
		miShowTableStyle.Name = "miShowTableStyle";
		miShowTableStyle.Text = "Table";
		miShowCardsStyle.AutoCheckOnClick = true;
		miShowCardsStyle.AutoCollapseOnClick = false;
		miShowCardsStyle.Image = (Image)componentResourceManager.GetObject("miShowTableStyle.Image");
		miShowCardsStyle.Name = "miShowCardsStyle";
		miShowCardsStyle.Text = "Cards";
		miShowIndicatorStyle.AutoCheckOnClick = true;
		miShowIndicatorStyle.AutoCollapseOnClick = false;
		miShowIndicatorStyle.Image = (Image)componentResourceManager.GetObject("miShowIndicatorStyle.Image");
		miShowIndicatorStyle.Name = "miShowIndicatorStyle";
		miShowIndicatorStyle.Text = "Indicator";
		miShowProgressStyle.AutoCheckOnClick = true;
		miShowProgressStyle.AutoCollapseOnClick = false;
		miShowProgressStyle.Image = (Image)componentResourceManager.GetObject("miShowProgressStyle.Image");
		miShowProgressStyle.Name = "miShowProgressStyle";
		miShowProgressStyle.Text = "Progress";
		miShowDialogStyle.AutoCheckOnClick = true;
		miShowDialogStyle.AutoCollapseOnClick = false;
		miShowDialogStyle.Image = (Image)componentResourceManager.GetObject("miShowDialogStyle.Image");
		miShowDialogStyle.Name = "miShowDialogStyle";
		miShowDialogStyle.Text = "Dialog";
		miHeaderSort.BackColor = Color.FromArgb(238, 238, 238);
		miHeaderSort.ForeColor = Color.FromArgb(136, 136, 136);
		miHeaderSort.Height = 24;
		miHeaderSort.Name = "miHeaderSort";
		miHeaderSort.Text = "Sort";
		miSortAsc.AutoCheckOnClick = true;
		miSortAsc.AutoCollapseOnClick = false;
		miSortAsc.Name = "miSortAsc";
		miSortAsc.Text = "Ascending";
		miSortDesc.AutoCheckOnClick = true;
		miSortDesc.AutoCollapseOnClick = false;
		miSortDesc.Name = "miSortDesc";
		miSortDesc.Text = "Descending";
		miSortNone.AutoCheckOnClick = true;
		miSortNone.AutoCollapseOnClick = false;
		miSortNone.Name = "miSortNone";
		miSortNone.Text = "NoSort";
		textBoxFindComponent.BeginGroup = true;
		textBoxFindComponent.ControlText = "";
		textBoxFindComponent.Name = "textBoxFindComponent";
		textBoxFindComponent.TextBoxWidth = 150;
		textBoxFindComponent.WatermarkColor = SystemColors.GrayText;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Dock = (DockStyle)1;
		stiGroupLine1.LinePosition = StiLinePosition.Bottom;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 30);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(784, 4);
		((Control)stiGroupLine1).TabIndex = 20;
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.F1);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Del);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Ins);
		DotNetBarManager.BottomDockSite = DockSiteBottom;
		DotNetBarManager.EnableFullSizeDock = false;
		DotNetBarManager.LeftDockSite = DockSiteLeft;
		DotNetBarManager.ParentForm = null;
		DotNetBarManager.RightDockSite = DockSiteRight;
		DotNetBarManager.Style = eDotNetBarStyle.StyleManagerControlled;
		DotNetBarManager.ToolbarBottomDockSite = dockSite4;
		DotNetBarManager.ToolbarLeftDockSite = dockSite1;
		DotNetBarManager.ToolbarRightDockSite = dockSite2;
		DotNetBarManager.ToolbarTopDockSite = dockSite3;
		DotNetBarManager.TopDockSite = DockSiteTop;
		((Control)DockSiteBottom).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteBottom).Dock = (DockStyle)2;
		DockSiteBottom.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteBottom).Location = new Point(0, 662);
		((Control)DockSiteBottom).Name = "DockSiteBottom";
		((Control)DockSiteBottom).Size = new Size(784, 0);
		((Control)DockSiteBottom).TabIndex = 24;
		((Control)DockSiteBottom).TabStop = false;
		((Control)DockSiteLeft).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteLeft).Dock = (DockStyle)3;
		DockSiteLeft.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteLeft).Location = new Point(0, 0);
		((Control)DockSiteLeft).Name = "DockSiteLeft";
		((Control)DockSiteLeft).Size = new Size(0, 662);
		((Control)DockSiteLeft).TabIndex = 22;
		((Control)DockSiteLeft).TabStop = false;
		((Control)DockSiteRight).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteRight).Dock = (DockStyle)4;
		DockSiteRight.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteRight).Location = new Point(784, 0);
		((Control)DockSiteRight).Name = "DockSiteRight";
		((Control)DockSiteRight).Size = new Size(0, 662);
		((Control)DockSiteRight).TabIndex = 23;
		((Control)DockSiteRight).TabStop = false;
		((Control)dockSite4).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite4).Dock = (DockStyle)2;
		((Control)dockSite4).Location = new Point(0, 662);
		((Control)dockSite4).Name = "dockSite4";
		((Control)dockSite4).Size = new Size(784, 0);
		((Control)dockSite4).TabIndex = 29;
		((Control)dockSite4).TabStop = false;
		((Control)dockSite1).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite1).Dock = (DockStyle)3;
		((Control)dockSite1).Location = new Point(0, 0);
		((Control)dockSite1).Name = "dockSite1";
		((Control)dockSite1).Size = new Size(0, 662);
		((Control)dockSite1).TabIndex = 26;
		((Control)dockSite1).TabStop = false;
		((Control)dockSite2).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite2).Dock = (DockStyle)4;
		((Control)dockSite2).Location = new Point(784, 0);
		((Control)dockSite2).Name = "dockSite2";
		((Control)dockSite2).Size = new Size(0, 662);
		((Control)dockSite2).TabIndex = 27;
		((Control)dockSite2).TabStop = false;
		((Control)dockSite3).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite3).Dock = (DockStyle)1;
		((Control)dockSite3).Location = new Point(0, 0);
		((Control)dockSite3).Name = "dockSite3";
		((Control)dockSite3).Size = new Size(784, 0);
		((Control)dockSite3).TabIndex = 28;
		((Control)dockSite3).TabStop = false;
		((Control)DockSiteTop).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteTop).Dock = (DockStyle)1;
		DockSiteTop.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteTop).Location = new Point(0, 0);
		((Control)DockSiteTop).Name = "DockSiteTop";
		((Control)DockSiteTop).Size = new Size(784, 0);
		((Control)DockSiteTop).TabIndex = 25;
		((Control)DockSiteTop).TabStop = false;
		contextMenuStyles.Name = "contextMenuStyles";
		contextMenuStyles.SubItems.AddRange(new BaseItem[7] { buttonItemAddStyle, buttonItemDuplicate, buttonItemCut, buttonItemCopy, buttonItemPaste, buttonItemDelete, buttonItemCreateStyleCollection });
		contextMenuStyles.Text = "contextMenuStyles";
		buttonItemAddStyle.Name = "buttonItemAddStyle";
		buttonItemAddStyle.SubItems.AddRange(new BaseItem[10] { buttonItemAddComponentStyle, buttonItemAddChartStyle, buttonItemAddGaugeStyle, buttonItemAddMapStyle, buttonItemAddCrossTableStyle, buttonItemAddTableStyle, buttonItemAddCardsStyle, buttonItemAddIndicatorStyle, buttonItemAddProgressStyle, buttonItemAddDialogStyle });
		buttonItemAddStyle.Text = "Add Style";
		buttonItemAddComponentStyle.Name = "buttonItemAddComponentStyle";
		buttonItemAddComponentStyle.Text = "Component";
		buttonItemAddComponentStyle.Click += ButtonAddComponentStyle_Click;
		buttonItemAddChartStyle.Name = "buttonItemAddChartStyle";
		buttonItemAddChartStyle.Text = "Chart";
		buttonItemAddChartStyle.Click += ButtonAddChartStyle_Click;
		buttonItemAddGaugeStyle.Name = "buttonItemAddGaugeStyle";
		buttonItemAddGaugeStyle.Text = "Gauge";
		buttonItemAddGaugeStyle.Click += ButtonAddGaugeStyle_Click;
		buttonItemAddMapStyle.Name = "buttonItemAddMapStyle";
		buttonItemAddMapStyle.Text = "Map";
		buttonItemAddMapStyle.Click += ButtonAddMapStyle_Click;
		buttonItemAddCrossTableStyle.Name = "buttonItemAddCrossTableStyle";
		buttonItemAddCrossTableStyle.Text = "Cross-Table";
		buttonItemAddCrossTableStyle.Click += ButtonAddCrossTabStyle_Click;
		buttonItemAddTableStyle.Name = "buttonItemAddTableStyle";
		buttonItemAddTableStyle.Text = "Table";
		buttonItemAddTableStyle.Click += ButtonAddTableStyle_Click;
		buttonItemAddCardsStyle.Name = "buttonItemAddCardsStyle";
		buttonItemAddCardsStyle.Text = "Cards";
		buttonItemAddCardsStyle.Click += ButtonAddCardsStyle_Click;
		buttonItemAddIndicatorStyle.Name = "buttonItemAddIndicatorStyle";
		buttonItemAddIndicatorStyle.Text = "Indicator";
		buttonItemAddIndicatorStyle.Click += ButtonAddIndicatorStyle_Click;
		buttonItemAddProgressStyle.Name = "buttonItemAddProgressStyle";
		buttonItemAddProgressStyle.Text = "Progress";
		buttonItemAddProgressStyle.Click += ButtonAddProgressStyle_Click;
		buttonItemAddDialogStyle.Name = "buttonItemAddDialogStyle";
		buttonItemAddDialogStyle.Text = "Dialog";
		buttonItemAddDialogStyle.Click += ButtonAddDialogStyle_Click;
		buttonItemDuplicate.BeginGroup = true;
		buttonItemDuplicate.Name = "buttonItemDuplicate";
		buttonItemDuplicate.Text = "Duplicate";
		buttonItemDuplicate.Click += ButtonStyleDuplicate_Click;
		buttonItemCut.BeginGroup = true;
		buttonItemCut.Name = "buttonItemCut";
		buttonItemCut.Text = "Cut";
		buttonItemCut.Click += ButtonStyleCut_Click;
		buttonItemCopy.Name = "buttonItemCopy";
		buttonItemCopy.Text = "Copy";
		buttonItemCopy.Click += ButtonStyleCopy_Click;
		buttonItemPaste.Name = "buttonItemPaste";
		buttonItemPaste.Text = "Paste";
		buttonItemPaste.Click += ButtonStylePaste_Click;
		buttonItemDelete.Name = "buttonItemDelete";
		buttonItemDelete.Text = "Delete";
		buttonItemDelete.Click += ButtonRemove_Click;
		buttonItemCreateStyleCollection.BeginGroup = true;
		buttonItemCreateStyleCollection.Name = "buttonItemCreateStyleCollection";
		buttonItemCreateStyleCollection.Text = "Create Style Collection";
		buttonItemCreateStyleCollection.Click += ButtonStylesCreate_Click;
		contextMenuBar.AntiAlias = true;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { contextMenuStyles });
		contextMenuBar.Location = new Point(134, 348);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(204, 25);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 30;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar1";
		((Control)treeViewStyles).Anchor = (AnchorStyles)15;
		((TreeView)treeViewStyles).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewStyles).DrawMode = (TreeViewDrawMode)2;
		((TreeView)treeViewStyles).HideSelection = false;
		((TreeView)treeViewStyles).ItemHeight = 45;
		((Control)treeViewStyles).Location = new Point(12, 46);
		((Control)treeViewStyles).Name = "treeViewStyles";
		((TreeView)treeViewStyles).ShowLines = false;
		((Control)treeViewStyles).Size = new Size(434, 554);
		((Control)treeViewStyles).TabIndex = 17;
		((TreeView)treeViewStyles).DrawNode += new DrawTreeNodeEventHandler(TreeViewStyles_DrawNode);
		((TreeView)treeViewStyles).AfterSelect += new TreeViewEventHandler(TreeViewStyles_AfterSelect);
		((Control)treeViewStyles).SizeChanged += TreeViewStyles_SizeChanged;
		((PropertyGrid)Properties).CategoryForeColor = SystemColors.InactiveCaptionText;
		((Control)Properties).Dock = (DockStyle)5;
		((PropertyGrid)Properties).HelpBackColor = Color.White;
		((PropertyGrid)Properties).HelpForeColor = SystemColors.InfoText;
		((PropertyGrid)Properties).LineColor = SystemColors.ScrollBar;
		((Control)Properties).Location = new Point(0, 0);
		((Control)Properties).Name = "Properties";
		Properties.ScrollOffset = 0;
		Properties.ShowEventsTag = false;
		((Control)Properties).Size = new Size(309, 554);
		((Control)Properties).TabIndex = 13;
		((PropertyGrid)Properties).PropertyValueChanged += new PropertyValueChangedEventHandler(Properties_PropertyValueChanged);
		((Control)Properties).Enter += Properties_Enter;
		((Control)Properties).Leave += Properties_Leave;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(784, 662);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)treeViewStyles);
		((Control)this).Controls.Add((Control)(object)panelRight);
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)DockSiteTop);
		((Control)this).Controls.Add((Control)(object)DockSiteBottom);
		((Control)this).Controls.Add((Control)(object)DockSiteLeft);
		((Control)this).Controls.Add((Control)(object)DockSiteRight);
		((Control)this).Controls.Add((Control)(object)dockSite1);
		((Control)this).Controls.Add((Control)(object)dockSite2);
		((Control)this).Controls.Add((Control)(object)dockSite3);
		((Control)this).Controls.Add((Control)(object)dockSite4);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiStyleDesigner";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiStyleDesigner";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)panelRight).ResumeLayout(false);
		((ISupportInitialize)bar1).EndInit();
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
