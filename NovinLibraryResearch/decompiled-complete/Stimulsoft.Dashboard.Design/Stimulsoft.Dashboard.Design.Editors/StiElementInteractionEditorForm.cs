using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiElementInteractionEditorForm : StiElementEditorForm
{
	private StiHtmlToolTip htmlToolTip;

	private JObject storedState;

	private IContainer components;

	private ButtonItem menuExpression;

	private ToolTip toolTip;

	private Label labelToolTip;

	private StiComboBox comboBoxOnHoverMode;

	private Label labelOnHoverMode;

	private StiPanelBox panelBoxToolTip;

	private StiOperationButton buttonEditToolTip;

	private StiOffice2013GroupBox groupBoxOnHover;

	private StiOffice2013GroupBox groupBoxOnClick;

	private StiPanelBox panelBoxHyperlink;

	private HintTextBox textBoxHyperlink;

	private StiOperationButton buttonEditHyperlink;

	private StiComboBox comboBoxOnClickMode;

	private Label labelHyperlink;

	private Label labelOnClickMode;

	private Panel panelBlockToolTip;

	private Panel panelBlockHyperlink;

	private StiComboBox comboBoxHyperlinkDestination;

	private Label labelHyperlinkDestination;

	private Label labelHyperlinkSample;

	private StiSampleLabelUX labelExpressionSample2;

	private StiOperationButton buttonRemoveToolTip;

	private Label labelToolTipStatus;

	private Panel panelHyperlinkDestination;

	private Button buttonOk;

	private Button buttonCancel;

	private StiGroupLine groupLineButtons;

	private StiOffice2013GroupBox groupBoxOnDataManipulation;

	private Panel panelUserFiltering;

	private CheckBox checkBoxAllowUserFiltering;

	private Panel panelUserSorting;

	private CheckBox checkBoxAllowUserSorting;

	private Panel panelBlockDashboard;

	private StiComboBox comboBoxDrillDownPage;

	private Label labelShowDashboardPage;

	private Panel panelParameterUi;

	private Button buttonNewParameter;

	private StiParameterListBox listBoxParameters;

	private Panel panelParameterOption;

	private StiDropDownBox comboBoxParameterName;

	private Label labelParameterName;

	private Label labelParameterValue;

	private StiPanelBox panelBoxParameterValue;

	private HintTextBox textBoxParameterExpression;

	private StiOperationButton buttonParameterValue;

	private Label labelShowDashboardParameters;

	private Panel panelParameterDummy;

	private StiOffice2013GroupBox groupBoxFieldInteractions;

	private Label labelRunFieldsEditorInfo;

	private Button buttonRunFieldsEditor;

	private Panel panelOnDataManipulation;

	private Panel panelUserDrillDown;

	private CheckBox checkBoxAllowUserDrillDown;

	private StiOffice2013GroupBox groupBoxLayout;

	private CheckBox checkBoxShowSaveButton;

	private CheckBox checkBoxShowFullScreenButton;

	private StiOperationButton buttonParameterPredefined;

	private ContextMenuBar contextMenuBar;

	private StiOperationButton buttonHyperlinkPredefined;

	private Panel panelTableInteraction;

	private CheckBox checkBoxFullRowSelect;

	private CheckBox checkBoxDrillDownFiltered;

	private CheckBox checkBoxShowViewDataButton;

	private Panel panelUserColumnSelection;

	private CheckBox checkBoxAllowUserColumnSelection;

	private string LocOnHoverNone => Loc.GetEnum("StiInteractionOnHoverNone");

	private string LocOnHoverShowToolTip => Loc.GetEnum("StiInteractionOnHoverShowToolTip");

	private string LocOnHoverShowHyperlink => Loc.GetEnum("StiInteractionOnHoverShowHyperlink");

	private string LocOnClickNone => Loc.GetEnum("StiInteractionOnHoverNone");

	private string LocOnClickShowDashboard => Loc.GetEnum("StiInteractionOnClickShowDashboard");

	private string LocOnClickOpenHyperlink => Loc.GetEnum("StiInteractionOnClickOpenHyperlink");

	private string LocOnClickApplyFilter => Loc.GetEnum("StiInteractionOnClickApplyFilter");

	private string LocOnClickDrillDown => Loc.GetEnum("StiInteractionOnClickDrillDown");

	private string LocDestinationNewTab => Loc.GetEnum("StiInteractionOpenHyperlinkDestinationNewTab");

	private string LocDestinationCurrentTab => Loc.GetEnum("StiInteractionOpenHyperlinkDestinationCurrentTab");

	internal StiDashboardInteraction Interaction { get; set; }

	internal IStiAllowUserFilteringDashboardInteraction UserFilteringInteraction => Interaction as IStiAllowUserFilteringDashboardInteraction;

	internal IStiAllowUserSortingDashboardInteraction UserSortingInteraction => Interaction as IStiAllowUserSortingDashboardInteraction;

	internal IStiAllowUserColumnSelectionDashboardInteraction UserColumnSelectionInteraction => Interaction as IStiAllowUserColumnSelectionDashboardInteraction;

	internal IStiAllowUserDrillDownDashboardInteraction UserDrillDownInteraction => Interaction as IStiAllowUserDrillDownDashboardInteraction;

	internal IStiInteractionLayout LayoutInteraction => Interaction as IStiInteractionLayout;

	internal StiTableDashboardInteraction TableInteraction => Interaction as StiTableDashboardInteraction;

	public override bool AllowResize => false;

	private StiPage[] DrillDownPages => base.Element?.Report?.Pages?.ToList()?.Where((StiPage p) => p != base.Element.Page)?.ToArray();

	private StiVariable[] Variables => base.Element?.Report?.Dictionary?.Variables?.ToList()?.ToArray();

	internal List<StiDashboardDrillDownParameter> DrillDownParameters => Interaction?.DrillDownParameters;

	internal StiDashboardDrillDownParameter DrillDownParameter => listBoxParameters.SelectedRangeItem?.Parameter;

	public StiElementInteractionEditorForm()
		: this(null, null)
	{
	}

	public StiElementInteractionEditorForm(StiElement element, StiDashboardInteraction interaction)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		Interaction = interaction;
		listBoxParameters.Designer = Designer;
		listBoxParameters.Editor = this;
		listBoxParameters.AllowImage = true;
		LocalizeInternal();
		storedState = interaction?.SaveToJsonObject(StiJsonSaveMode.Report);
		ReadPropertiesFromElement();
		RefreshUiPanel();
		DoScaling();
		RefreshUiParameterAndHyperlinkButtons();
	}

	public override void Localize()
	{
		LocalizeInternal();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.GetMain("Interaction");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)groupBoxOnHover).Text = Loc.GetMain("OnHover");
		((Control)groupBoxOnClick).Text = Loc.GetMain("OnClick");
		((Control)groupBoxOnDataManipulation).Text = Loc.GetMain("OnDataManipulation");
		((Control)groupBoxFieldInteractions).Text = Loc.Get("Dashboard", "FieldInteractions");
		((Control)buttonRunFieldsEditor).Text = Loc.Get("Dashboard", "RunFieldsEditor");
		((Control)checkBoxAllowUserFiltering).Text = Loc.Get("Dashboard", "AllowUserFiltering");
		((Control)checkBoxAllowUserColumnSelection).Text = Loc.Get("Dashboard", "AllowUserColumnSelection");
		((Control)checkBoxAllowUserSorting).Text = Loc.Get("Dashboard", "AllowUserSorting");
		((Control)checkBoxAllowUserDrillDown).Text = Loc.Get("Dashboard", "AllowUserDrillDown");
		((Control)checkBoxDrillDownFiltered).Text = Loc.Get("Dashboard", "DrillDownFiltered");
		((Control)checkBoxFullRowSelect).Text = Loc.Get("Dashboard", "FullRowSelect");
		((Control)labelOnHoverMode).Text = Loc.GetMain("Mode");
		((Control)labelOnClickMode).Text = Loc.GetMain("Mode");
		((Control)labelHyperlink).Text = Loc.GetMain("Hyperlink");
		((Control)labelToolTip).Text = Loc.GetMain("ToolTip");
		((Control)labelShowDashboardParameters).Text = Loc.GetMain("Parameters");
		((Control)labelHyperlinkDestination).Text = Loc.GetMain("Destination");
		((Control)labelHyperlinkSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelRunFieldsEditorInfo).Text = Loc.Get("Dashboard", "RunFieldsEditorInfo");
		((Control)labelParameterName).Text = Loc.GetMain("Name");
		((Control)labelParameterValue).Text = Loc.GetMain("Value");
		((Control)labelShowDashboardPage).Text = Loc.Get("Components", "StiPage");
		((Control)buttonNewParameter).Text = Loc.GetCleaned("MainMenu", "menuEditDataParameterNew");
		if (base.Element is StiRegionMapElement)
		{
			((Control)labelExpressionSample2).Text = "http://example.com/{ident}";
		}
		else if (base.Element is StiChartElement)
		{
			((Control)labelExpressionSample2).Text = "http://example.com/{argument}/{value}";
		}
		else if (base.Element is StiIndicatorElement)
		{
			((Control)labelExpressionSample2).Text = "http://example.com/{value}/{series}/{target}";
		}
		else if (base.Element is StiTableElement)
		{
			((Control)labelExpressionSample2).Text = "http://example.com/{value}";
		}
		else if (base.Element is StiTextElement)
		{
			((Control)labelExpressionSample2).Text = "http://example.com/{myvariable}.com";
		}
		((ComboBox)comboBoxOnHoverMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxOnHoverMode).SelectedIndex;
		((ComboBox)comboBoxOnHoverMode).Items.Clear();
		((ComboBox)comboBoxOnHoverMode).Items.Add((object)LocOnHoverNone);
		if ((Interaction.AvailableOnHover & StiAvailableInteractionOnHover.ShowToolTip) > StiAvailableInteractionOnHover.None)
		{
			((ComboBox)comboBoxOnHoverMode).Items.Add((object)LocOnHoverShowToolTip);
		}
		if ((Interaction.AvailableOnHover & StiAvailableInteractionOnHover.ShowHyperlink) > StiAvailableInteractionOnHover.None)
		{
			((ComboBox)comboBoxOnHoverMode).Items.Add((object)LocOnHoverShowHyperlink);
		}
		((ListControl)comboBoxOnHoverMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxOnHoverMode).EndUpdate();
		((ComboBox)comboBoxOnClickMode).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxOnClickMode).SelectedIndex;
		((ComboBox)comboBoxOnClickMode).Items.Clear();
		((ComboBox)comboBoxOnClickMode).Items.Add((object)LocOnClickNone);
		if ((Interaction.AvailableOnClick & StiAvailableInteractionOnClick.ShowDashboard) > StiAvailableInteractionOnClick.None)
		{
			((ComboBox)comboBoxOnClickMode).Items.Add((object)LocOnClickShowDashboard);
		}
		if ((Interaction.AvailableOnClick & StiAvailableInteractionOnClick.OpenHyperlink) > StiAvailableInteractionOnClick.None)
		{
			((ComboBox)comboBoxOnClickMode).Items.Add((object)LocOnClickOpenHyperlink);
		}
		if ((Interaction.AvailableOnClick & StiAvailableInteractionOnClick.ApplyFilter) > StiAvailableInteractionOnClick.None)
		{
			((ComboBox)comboBoxOnClickMode).Items.Add((object)LocOnClickApplyFilter);
		}
		if ((Interaction.AvailableOnClick & StiAvailableInteractionOnClick.DrillDown) > StiAvailableInteractionOnClick.None)
		{
			((ComboBox)comboBoxOnClickMode).Items.Add((object)LocOnClickDrillDown);
		}
		((ListControl)comboBoxOnClickMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxOnClickMode).EndUpdate();
		((ComboBox)comboBoxHyperlinkDestination).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxHyperlinkDestination).SelectedIndex;
		((ComboBox)comboBoxHyperlinkDestination).Items.Clear();
		((ComboBox)comboBoxHyperlinkDestination).Items.Add((object)LocDestinationNewTab);
		((ComboBox)comboBoxHyperlinkDestination).Items.Add((object)LocDestinationCurrentTab);
		((ListControl)comboBoxHyperlinkDestination).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxHyperlinkDestination).EndUpdate();
		((ComboBox)comboBoxDrillDownPage).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxDrillDownPage).SelectedIndex;
		((ComboBox)comboBoxDrillDownPage).Items.Clear();
		((ComboBox)comboBoxDrillDownPage).Items.Add((object)"");
		if (DrillDownPages != null)
		{
			ObjectCollection items = ((ComboBox)comboBoxDrillDownPage).Items;
			object[] drillDownPages = DrillDownPages;
			items.AddRange(drillDownPages);
		}
		((ListControl)comboBoxDrillDownPage).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxDrillDownPage).EndUpdate();
		comboBoxParameterName.Items.Clear();
		comboBoxParameterName.Items.Add((object)"");
		comboBoxParameterName.Items.Add((object)"Title");
		if (Variables != null)
		{
			ObjectCollection items2 = comboBoxParameterName.Items;
			object[] drillDownPages = Variables.Select((StiVariable v) => v.Name).ToArray();
			items2.AddRange(drillDownPages);
		}
		((Control)checkBoxShowFullScreenButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("FormViewer", "FullScreen"));
		((Control)checkBoxShowSaveButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("Buttons", "Save"));
		((Control)checkBoxShowViewDataButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("FormTitles", "ViewDataForm"));
		((Control)groupBoxLayout).Text = Loc.GetMain("Layout");
		base.LockRefresh = false;
	}

	private void RefreshUiParameterAndHyperlinkButtons()
	{
		if (base.Element is IStiTextElement || base.Element is IStiImageElement)
		{
			((Control)buttonParameterPredefined).Visible = false;
			((Control)buttonHyperlinkPredefined).Visible = false;
			((Control)buttonParameterValue).Location = ((Control)buttonParameterPredefined).Location;
			((Control)buttonEditHyperlink).Location = ((Control)buttonHyperlinkPredefined).Location;
		}
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEditHyperlink).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonEditToolTip).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonParameterValue).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonRemoveToolTip).Image = StiReportDesignImages.Actions.Remove();
		StiComboBox stiComboBox = comboBoxOnClickMode;
		StiDropDownBox stiDropDownBox = comboBoxParameterName;
		StiComboBox stiComboBox2 = comboBoxOnHoverMode;
		int num = (((Control)comboBoxHyperlinkDestination).Width = ((Control)panelBoxToolTip).Width);
		int num3 = (((Control)stiComboBox2).Width = num);
		int width2 = (((Control)stiDropDownBox).Width = num3);
		((Control)stiComboBox).Width = width2;
		((Control)comboBoxDrillDownPage).Width = ((Control)panelBlockDashboard).Width - ((Control)comboBoxDrillDownPage).Left;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxHyperlink);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxParameterValue);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxToolTip);
	}

	public override void ReadPropertiesFromElement()
	{
		base.ReadPropertiesFromElement();
		ReadParametersPropertiesFromElement();
	}

	private void ReadParametersPropertiesFromElement()
	{
		((ListBox)listBoxParameters).BeginUpdate();
		((ListBox)listBoxParameters).Items.Clear();
		listBoxParameters.AddItems(DrillDownParameters);
		listBoxParameters.SelectFirstItem();
		((ListBox)listBoxParameters).EndUpdate();
	}

	private void WriteParametersPropertiesToElement()
	{
		Interaction.DrillDownParameters = (from t in listBoxParameters.ToItems()
			select t.Parameter).ToList();
	}

	public override void WritePropertiesToElement()
	{
		RefreshElementView();
		WriteParametersPropertiesToElement();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		listBoxParameters.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxParameters.ElementsSelectedChanged += delegate
		{
			base.Element.Select();
			RefreshUiPanel();
		};
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			base.LockRefresh = true;
			switch (Interaction.OnHover)
			{
			case StiInteractionOnHover.None:
				((ComboBox)comboBoxOnHoverMode).SelectedItem = LocOnHoverNone;
				break;
			case StiInteractionOnHover.ShowToolTip:
				((ComboBox)comboBoxOnHoverMode).SelectedItem = LocOnHoverShowToolTip;
				break;
			case StiInteractionOnHover.ShowHyperlink:
				((ComboBox)comboBoxOnHoverMode).SelectedItem = LocOnHoverShowHyperlink;
				break;
			}
			switch (Interaction.OnClick)
			{
			case StiInteractionOnClick.None:
				((ComboBox)comboBoxOnClickMode).SelectedItem = LocOnClickNone;
				break;
			case StiInteractionOnClick.ShowDashboard:
				((ComboBox)comboBoxOnClickMode).SelectedItem = LocOnClickShowDashboard;
				break;
			case StiInteractionOnClick.OpenHyperlink:
				((ComboBox)comboBoxOnClickMode).SelectedItem = LocOnClickOpenHyperlink;
				break;
			case StiInteractionOnClick.ApplyFilter:
				((ComboBox)comboBoxOnClickMode).SelectedItem = LocOnClickApplyFilter;
				break;
			case StiInteractionOnClick.DrillDown:
				((ComboBox)comboBoxOnClickMode).SelectedItem = LocOnClickDrillDown;
				break;
			}
			switch (Interaction.HyperlinkDestination)
			{
			case StiInteractionOpenHyperlinkDestination.NewTab:
				((ComboBox)comboBoxHyperlinkDestination).SelectedItem = LocDestinationNewTab;
				break;
			case StiInteractionOpenHyperlinkDestination.CurrectTab:
				((ComboBox)comboBoxHyperlinkDestination).SelectedItem = LocDestinationCurrentTab;
				break;
			}
			StiPage stiPage = DrillDownPages?.FirstOrDefault((StiPage p) => p.Guid == Interaction.DrillDownPageKey);
			if (stiPage == null)
			{
				((ListControl)comboBoxDrillDownPage).SelectedIndex = 0;
			}
			else
			{
				((ComboBox)comboBoxDrillDownPage).SelectedItem = stiPage;
			}
			((Control)labelToolTipStatus).Text = (string.IsNullOrWhiteSpace(Interaction.ToolTip) ? ("(" + Loc.Get("FormStyleDesigner", "NotSpecified") + ")") : ("(" + Loc.Get("FormFormatEditor", "Custom") + ")"));
			((Control)textBoxHyperlink).Text = Interaction.Hyperlink;
			((Control)panelBlockToolTip).Visible = Interaction.OnHover == StiInteractionOnHover.ShowToolTip;
			((Control)panelBlockHyperlink).Visible = Interaction.OnClick == StiInteractionOnClick.OpenHyperlink;
			((Control)panelBlockDashboard).Visible = Interaction.OnClick == StiInteractionOnClick.ShowDashboard;
			((Control)panelParameterOption).Visible = Interaction.DrillDownParameters.Count > 0;
			if (((Control)panelParameterOption).Visible)
			{
				((Control)comboBoxParameterName).Width = ((Control)panelBoxToolTip).Width;
			}
			((Control)groupBoxOnClick).Visible = Interaction.AvailableOnClick != StiAvailableInteractionOnClick.None;
			((Control)groupBoxOnHover).Visible = Interaction.AvailableOnHover != StiAvailableInteractionOnHover.None;
			((Control)groupBoxOnDataManipulation).Visible = Interaction.AvailableOnDataManipulation != StiAvailableInteractionOnDataManipulation.None;
			((Control)groupBoxFieldInteractions).Visible = Interaction is StiTableDashboardInteraction;
			((Control)groupBoxLayout).Visible = Interaction is IStiInteractionLayout;
			((Control)panelUserSorting).Visible = (Interaction.AvailableOnDataManipulation & StiAvailableInteractionOnDataManipulation.AllowSorting) > StiAvailableInteractionOnDataManipulation.None;
			((Control)panelUserColumnSelection).Visible = (Interaction.AvailableOnDataManipulation & StiAvailableInteractionOnDataManipulation.AllowColumnSelection) > StiAvailableInteractionOnDataManipulation.None;
			((Control)panelUserFiltering).Visible = (Interaction.AvailableOnDataManipulation & StiAvailableInteractionOnDataManipulation.AllowFiltering) > StiAvailableInteractionOnDataManipulation.None;
			((Control)panelUserDrillDown).Visible = (Interaction.AvailableOnDataManipulation & StiAvailableInteractionOnDataManipulation.AllowDrillDown) > StiAvailableInteractionOnDataManipulation.None;
			((Control)panelTableInteraction).Visible = TableInteraction != null && Interaction.OnClick == StiInteractionOnClick.ApplyFilter;
			checkBoxAllowUserColumnSelection.Checked = UserColumnSelectionInteraction != null && UserColumnSelectionInteraction.AllowUserColumnSelection;
			checkBoxAllowUserFiltering.Checked = UserFilteringInteraction != null && UserFilteringInteraction.AllowUserFiltering;
			checkBoxAllowUserSorting.Checked = UserSortingInteraction != null && UserSortingInteraction.AllowUserSorting;
			checkBoxAllowUserDrillDown.Checked = UserDrillDownInteraction != null && UserDrillDownInteraction.AllowUserDrillDown;
			checkBoxShowFullScreenButton.Checked = LayoutInteraction != null && LayoutInteraction.ShowFullScreenButton;
			checkBoxShowSaveButton.Checked = LayoutInteraction != null && LayoutInteraction.ShowSaveButton;
			checkBoxShowViewDataButton.Checked = LayoutInteraction != null && LayoutInteraction.ShowViewDataButton;
			checkBoxDrillDownFiltered.Checked = TableInteraction != null && TableInteraction.DrillDownFiltered;
			checkBoxFullRowSelect.Checked = TableInteraction != null && TableInteraction.FullRowSelect;
			if (htmlToolTip == null)
			{
				htmlToolTip = new StiHtmlToolTip();
			}
			htmlToolTip.HtmlText = Interaction.ToolTip;
			if (((ToolTip)htmlToolTip).GetToolTip((Control)(object)labelToolTipStatus) != Interaction.ToolTip)
			{
				((ToolTip)htmlToolTip).SetToolTip((Control)(object)labelToolTipStatus, Interaction.ToolTip);
			}
			((Control)buttonRemoveToolTip).Enabled = !string.IsNullOrEmpty(Interaction.ToolTip);
			SetListHeightFromCount(listBoxParameters);
			CalculateWindowHeight();
			base.LockRefresh = false;
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected virtual void SetListHeightFromCount(StiParameterListBox list)
	{
		int num = Math.Min(10, ((ListBox)list).Items.Count);
		((Control)list).Height = Math.Min(num + 1, 7) * ((ListBox)list).ItemHeight;
	}

	protected override void CalculateWindowHeight()
	{
		int num = (((Control)groupLineButtons).Visible ? ((Control)groupLineButtons).Height : 0);
		if (((Control)panelBlockToolTip).Visible)
		{
			((Control)groupBoxOnHover).Height = ((Control)comboBoxOnHoverMode).Bottom + ((Control)panelBlockToolTip).Height;
		}
		else
		{
			((Control)groupBoxOnHover).Height = ((Control)comboBoxOnHoverMode).Bottom + StiScale.YYI(12.0);
		}
		if (((Control)panelBlockDashboard).Visible)
		{
			((Control)panelBlockDashboard).Height = ((Control)panelParameterUi).Height + ((Control)listBoxParameters).Height + (((Control)panelParameterOption).Visible ? ((Control)panelParameterOption).Height : 0) + ((Control)comboBoxDrillDownPage).Bottom + ((Control)panelParameterDummy).Height + StiScale.YYI(12.0);
		}
		if (((Control)panelBlockHyperlink).Visible)
		{
			((Control)groupBoxOnClick).Height = ((Control)comboBoxOnClickMode).Bottom + ((Control)panelBlockHyperlink).Height;
		}
		else if (((Control)panelBlockDashboard).Visible)
		{
			((Control)groupBoxOnClick).Height = ((Control)comboBoxOnClickMode).Bottom + ((Control)panelBlockDashboard).Height;
		}
		else
		{
			((Control)groupBoxOnClick).Height = ((Control)comboBoxOnClickMode).Bottom + StiScale.YYI(12.0);
		}
		((Control)groupBoxOnDataManipulation).Height = StiScale.YYI(groupBoxOnDataManipulation.HeaderHeight) + (((Control)panelUserFiltering).Visible ? ((Control)panelUserFiltering).Height : 0) + (((Control)panelUserSorting).Visible ? ((Control)panelUserSorting).Height : 0) + (((Control)panelUserColumnSelection).Visible ? ((Control)panelUserColumnSelection).Height : 0) + (((Control)panelUserDrillDown).Visible ? ((Control)panelUserDrillDown).Height : 0) + (((Control)panelTableInteraction).Visible ? ((Control)panelTableInteraction).Height : 0) + ((Control)panelOnDataManipulation).Height;
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + (((Control)groupBoxOnHover).Visible ? ((Control)groupBoxOnHover).Height : 0) + (((Control)groupBoxOnClick).Visible ? ((Control)groupBoxOnClick).Height : 0) + (((Control)groupBoxLayout).Visible ? ((Control)groupBoxLayout).Height : 0) + (((Control)groupBoxOnDataManipulation).Visible ? ((Control)groupBoxOnDataManipulation).Height : 0) + (((Control)groupBoxFieldInteractions).Visible ? ((Control)groupBoxFieldInteractions).Height : 0) + num);
		Rectangle workingArea = Screen.FromControl((Control)(object)this).WorkingArea;
		if (((Control)this).Bottom > workingArea.Bottom)
		{
			((Control)this).Top = workingArea.Bottom - ((Control)this).Height;
		}
	}

	internal void RefreshParameter()
	{
		if (DrillDownParameter != null)
		{
			base.LockRefresh = true;
			((Control)comboBoxParameterName).Text = DrillDownParameter.Name;
			((Control)textBoxParameterExpression).Text = DrillDownParameter.Expression;
			base.LockRefresh = false;
		}
	}

	private string GetParameterName()
	{
		string name = Variables.FirstOrDefault()?.Name;
		if (!string.IsNullOrWhiteSpace(name))
		{
			return name;
		}
		name = Loc.GetMain("Name");
		int num = 2;
		while (Interaction.DrillDownParameters.Any((StiDashboardDrillDownParameter p) => p.Name == name))
		{
			name = Loc.GetMain("Name") + num;
			num++;
		}
		return name;
	}

	private List<string> GetIdents()
	{
		if (base.Element is StiRegionMapElement)
		{
			return new List<string> { "Ident", "Value" };
		}
		if (base.Element is StiTextElement)
		{
			return new List<string> { "Value" };
		}
		if (base.Element is StiChartElement)
		{
			if (((StiChartElement)base.Element).IsRange)
			{
				return new List<string> { "Argument", "Value", "EndValue", "Series" };
			}
			return new List<string> { "Argument", "Value", "Series" };
		}
		if (base.Element is StiIndicatorElement)
		{
			return new List<string> { "Value", "Series", "Target" };
		}
		if (base.Element is StiTableElement)
		{
			List<string> list = new List<string> { "Value" };
			StiTableElement stiTableElement = base.Element as StiTableElement;
			{
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					string label = StiLabelHelper.GetLabel(column);
					string item = "Row." + StiCodeDomSerializator.ReplaceSymbols(label);
					list.Add(item);
				}
				return list;
			}
		}
		return new List<string>();
	}

	private void ButtonParameterPredefined_Click(object sender, EventArgs e)
	{
		new StiIdentsMenu(menuExpression, GetIdents(), (TextBox)(object)textBoxParameterExpression, this).CreateItems();
		Point point = new Point(((Control)panelBoxParameterValue).Width, ((Control)panelBoxParameterValue).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxParameterValue).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ButtonHyperlinkPredefined_Click(object sender, EventArgs e)
	{
		new StiIdentsMenu(menuExpression, GetIdents(), (TextBox)(object)textBoxHyperlink, this).CreateItems();
		Point point = new Point(((Control)panelBoxHyperlink).Width, ((Control)panelBoxHyperlink).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxHyperlink).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ComboBoxDrillDownPage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (((ListControl)comboBoxDrillDownPage).SelectedIndex == -1)
			{
				Interaction.DrillDownPageKey = "";
			}
			else
			{
				Interaction.DrillDownPageKey = (((ComboBox)comboBoxDrillDownPage).SelectedItem as StiPage)?.Guid;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxOnHoverMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			string text = ((ComboBox)comboBoxOnHoverMode).SelectedItem as string;
			if (text == LocOnHoverNone)
			{
				Interaction.OnHover = StiInteractionOnHover.None;
			}
			else if (text == LocOnHoverShowToolTip)
			{
				Interaction.OnHover = StiInteractionOnHover.ShowToolTip;
			}
			else if (text == LocOnHoverShowHyperlink)
			{
				Interaction.OnHover = StiInteractionOnHover.ShowHyperlink;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void TextBoxHyperlink_TextChanged(object sender, EventArgs e)
	{
		Interaction.Hyperlink = ((Control)textBoxHyperlink).Text;
	}

	private void ComboBoxOnClickMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			string text = ((ComboBox)comboBoxOnClickMode).SelectedItem as string;
			if (text == LocOnClickNone)
			{
				Interaction.OnClick = StiInteractionOnClick.None;
			}
			else if (text == LocOnClickApplyFilter)
			{
				Interaction.OnClick = StiInteractionOnClick.ApplyFilter;
			}
			else if (text == LocOnClickOpenHyperlink)
			{
				Interaction.OnClick = StiInteractionOnClick.OpenHyperlink;
			}
			else if (text == LocOnClickShowDashboard)
			{
				Interaction.OnClick = StiInteractionOnClick.ShowDashboard;
			}
			else if (text == LocOnClickDrillDown)
			{
				Interaction.OnClick = StiInteractionOnClick.DrillDown;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxHyperlinkDestination_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			string text = ((ComboBox)comboBoxHyperlinkDestination).SelectedItem as string;
			if (text == LocDestinationCurrentTab)
			{
				Interaction.HyperlinkDestination = StiInteractionOpenHyperlinkDestination.CurrectTab;
			}
			else if (text == LocDestinationNewTab)
			{
				Interaction.HyperlinkDestination = StiInteractionOpenHyperlinkDestination.NewTab;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ButtonEditToolTip_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		StiToolTipEditorForm stiToolTipEditorForm = new StiToolTipEditorForm(Interaction.ToolTip, GetIdents());
		try
		{
			if ((int)((Form)stiToolTipEditorForm).ShowDialog() == 1)
			{
				Interaction.ToolTip = stiToolTipEditorForm.ToolTip;
				RefreshUiPanel();
			}
		}
		finally
		{
			((IDisposable)stiToolTipEditorForm)?.Dispose();
		}
	}

	private void ButtonNewParameter_Click(object sender, EventArgs e)
	{
		string parameterName = GetParameterName();
		Interaction.DrillDownParameters.Add(new StiDashboardDrillDownParameter
		{
			Name = parameterName
		});
		ReadParametersPropertiesFromElement();
		RefreshElementView();
		RefreshUiPanel();
		listBoxParameters.SelectLastItem();
	}

	private void ButtonEditHyperlink_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		StiExpressionEditorForm stiExpressionEditorForm = new StiExpressionEditorForm(((Control)textBoxHyperlink).Text, base.Element?.Report);
		try
		{
			stiExpressionEditorForm.Idents = GetIdents();
			if ((int)((Form)stiExpressionEditorForm).ShowDialog() == 1)
			{
				((Control)textBoxHyperlink).Text = stiExpressionEditorForm.CodeEditorText;
				((Control)textBoxHyperlink).Focus();
				((TextBoxBase)textBoxHyperlink).Select(0, 0);
			}
		}
		finally
		{
			((IDisposable)stiExpressionEditorForm)?.Dispose();
		}
	}

	private void ButtonRemoveToolTip_Click(object sender, EventArgs e)
	{
		Interaction.ToolTip = "";
		RefreshUiPanel();
	}

	private void CheckBoxAllowUserFiltering_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && UserFilteringInteraction != null)
		{
			UserFilteringInteraction.AllowUserFiltering = checkBoxAllowUserFiltering.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxAllowUserColumnSelection_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && UserSortingInteraction != null)
		{
			UserColumnSelectionInteraction.AllowUserColumnSelection = checkBoxAllowUserColumnSelection.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxAllowUserSorting_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && UserSortingInteraction != null)
		{
			UserSortingInteraction.AllowUserSorting = checkBoxAllowUserSorting.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxAllowUserDrillDown_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && UserDrillDownInteraction != null)
		{
			UserDrillDownInteraction.AllowUserDrillDown = checkBoxAllowUserDrillDown.Checked;
			RefreshUiPanel();
		}
	}

	private void ListBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			RefreshParameter();
		}
	}

	private void ListBoxParameters_MouseDown(object sender, MouseEventArgs e)
	{
		RefreshParameter();
	}

	private void ListBoxParameters_Enter(object sender, EventArgs e)
	{
		RefreshParameter();
	}

	private void ComboBoxParameterName_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			DrillDownParameter.Name = (string.IsNullOrWhiteSpace(((Control)comboBoxParameterName).Text) ? Loc.GetMain("Name") : ((Control)comboBoxParameterName).Text);
			RefreshElementView();
			listBoxParameters.InvalidateSelectedItem();
		}
	}

	private void TextBoxParameterValue_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			DrillDownParameter.Expression = ((Control)textBoxParameterExpression).Text;
			RefreshElementView();
			listBoxParameters.InvalidateSelectedItem();
		}
	}

	private void ButtonParameterValue_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		StiExpressionEditorForm stiExpressionEditorForm = new StiExpressionEditorForm(((Control)textBoxParameterExpression).Text, base.Element?.Report);
		try
		{
			stiExpressionEditorForm.Idents = GetIdents();
			if ((int)((Form)stiExpressionEditorForm).ShowDialog() == 1)
			{
				((Control)textBoxParameterExpression).Text = stiExpressionEditorForm.CodeEditorText;
				((Control)textBoxParameterExpression).Focus();
				((TextBoxBase)textBoxParameterExpression).Select(0, 0);
			}
		}
		finally
		{
			((IDisposable)stiExpressionEditorForm)?.Dispose();
		}
	}

	private void ButtonRunFieldsEditor_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
		StiInplaceEditor.OnFieldsEditorClick(StiElementActionHelper.CurrentComponent);
		StiElementActionHelper.Hide();
	}

	private void CheckBoxShowFullScreenButton_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && LayoutInteraction != null)
		{
			LayoutInteraction.ShowFullScreenButton = checkBoxShowFullScreenButton.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxShowSaveButton_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && LayoutInteraction != null)
		{
			LayoutInteraction.ShowSaveButton = checkBoxShowSaveButton.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxShowViewDataButton_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && LayoutInteraction != null)
		{
			LayoutInteraction.ShowViewDataButton = checkBoxShowViewDataButton.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxDrillDownSelected_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableInteraction != null)
		{
			TableInteraction.DrillDownFiltered = checkBoxDrillDownFiltered.Checked;
			RefreshUiPanel();
		}
	}

	private void CheckBoxFullRowSelect_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableInteraction != null)
		{
			TableInteraction.FullRowSelect = checkBoxFullRowSelect.Checked;
			RefreshUiPanel();
		}
	}

	private void LabelExpressionSample2_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)textBoxHyperlink).Text = ((Control)labelExpressionSample2).Text;
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)comboBoxOnHoverMode).Focus();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		StiDesignerControl designer = Designer;
		if (designer != null)
		{
			Form obj = ((Control)designer).FindForm();
			if (obj != null)
			{
				obj.Activate();
			}
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		if (storedState != null)
		{
			Interaction.LoadFromJsonObject(storedState);
		}
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_interaction.htm");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
		if (htmlToolTip != null)
		{
			((Component)(object)htmlToolTip).Dispose();
		}
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Expected O, but got Unknown
		//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_122f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1359: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Expected O, but got Unknown
		//IL_1380: Unknown result type (might be due to invalid IL or missing references)
		//IL_1448: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1576: Unknown result type (might be due to invalid IL or missing references)
		//IL_1609: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_171c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1726: Expected O, but got Unknown
		//IL_1757: Unknown result type (might be due to invalid IL or missing references)
		//IL_17df: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e9: Expected O, but got Unknown
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1882: Unknown result type (might be due to invalid IL or missing references)
		//IL_188c: Expected O, but got Unknown
		//IL_1909: Unknown result type (might be due to invalid IL or missing references)
		//IL_1970: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa4: Expected O, but got Unknown
		//IL_1ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_201c: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_216a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2258: Unknown result type (might be due to invalid IL or missing references)
		//IL_2360: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2469: Unknown result type (might be due to invalid IL or missing references)
		//IL_2515: Unknown result type (might be due to invalid IL or missing references)
		//IL_2586: Unknown result type (might be due to invalid IL or missing references)
		//IL_2635: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2755: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_285f: Unknown result type (might be due to invalid IL or missing references)
		//IL_290f: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5c: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiElementInteractionEditorForm));
		toolTip = new ToolTip(components);
		labelToolTip = new Label();
		menuExpression = new ButtonItem();
		contextMenuBar = new ContextMenuBar();
		comboBoxOnHoverMode = new StiComboBox();
		labelOnHoverMode = new Label();
		panelBoxToolTip = new StiPanelBox();
		labelToolTipStatus = new Label();
		buttonRemoveToolTip = new StiOperationButton();
		buttonEditToolTip = new StiOperationButton();
		groupBoxOnHover = new StiOffice2013GroupBox();
		panelBlockToolTip = new Panel();
		groupBoxOnClick = new StiOffice2013GroupBox();
		panelBlockDashboard = new Panel();
		panelParameterUi = new Panel();
		buttonNewParameter = new Button();
		labelShowDashboardParameters = new Label();
		listBoxParameters = new StiParameterListBox();
		comboBoxDrillDownPage = new StiComboBox();
		labelShowDashboardPage = new Label();
		panelParameterOption = new Panel();
		panelBoxParameterValue = new StiPanelBox();
		buttonParameterPredefined = new StiOperationButton();
		textBoxParameterExpression = new HintTextBox();
		buttonParameterValue = new StiOperationButton();
		comboBoxParameterName = new StiDropDownBox();
		labelParameterName = new Label();
		labelParameterValue = new Label();
		panelParameterDummy = new Panel();
		panelBlockHyperlink = new Panel();
		labelHyperlinkSample = new Label();
		labelExpressionSample2 = new StiSampleLabelUX();
		panelBoxHyperlink = new StiPanelBox();
		buttonHyperlinkPredefined = new StiOperationButton();
		textBoxHyperlink = new HintTextBox();
		buttonEditHyperlink = new StiOperationButton();
		labelHyperlink = new Label();
		comboBoxOnClickMode = new StiComboBox();
		labelOnClickMode = new Label();
		panelHyperlinkDestination = new Panel();
		comboBoxHyperlinkDestination = new StiComboBox();
		labelHyperlinkDestination = new Label();
		panelTableInteraction = new Panel();
		checkBoxFullRowSelect = new CheckBox();
		checkBoxDrillDownFiltered = new CheckBox();
		buttonOk = new Button();
		buttonCancel = new Button();
		groupLineButtons = new StiGroupLine();
		groupBoxOnDataManipulation = new StiOffice2013GroupBox();
		panelUserColumnSelection = new Panel();
		checkBoxAllowUserColumnSelection = new CheckBox();
		panelUserSorting = new Panel();
		checkBoxAllowUserSorting = new CheckBox();
		panelUserFiltering = new Panel();
		checkBoxAllowUserFiltering = new CheckBox();
		panelUserDrillDown = new Panel();
		checkBoxAllowUserDrillDown = new CheckBox();
		panelOnDataManipulation = new Panel();
		groupBoxFieldInteractions = new StiOffice2013GroupBox();
		labelRunFieldsEditorInfo = new Label();
		buttonRunFieldsEditor = new Button();
		groupBoxLayout = new StiOffice2013GroupBox();
		checkBoxShowViewDataButton = new CheckBox();
		checkBoxShowSaveButton = new CheckBox();
		checkBoxShowFullScreenButton = new CheckBox();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelBoxToolTip).SuspendLayout();
		((Control)groupBoxOnHover).SuspendLayout();
		((Control)panelBlockToolTip).SuspendLayout();
		((Control)groupBoxOnClick).SuspendLayout();
		((Control)panelBlockDashboard).SuspendLayout();
		((Control)panelParameterUi).SuspendLayout();
		((Control)panelParameterOption).SuspendLayout();
		((Control)panelBoxParameterValue).SuspendLayout();
		((Control)panelBlockHyperlink).SuspendLayout();
		((Control)panelBoxHyperlink).SuspendLayout();
		((Control)panelHyperlinkDestination).SuspendLayout();
		((Control)panelTableInteraction).SuspendLayout();
		((Control)groupLineButtons).SuspendLayout();
		((Control)groupBoxOnDataManipulation).SuspendLayout();
		((Control)panelUserColumnSelection).SuspendLayout();
		((Control)panelUserSorting).SuspendLayout();
		((Control)panelUserFiltering).SuspendLayout();
		((Control)panelUserDrillDown).SuspendLayout();
		((Control)groupBoxFieldInteractions).SuspendLayout();
		((Control)groupBoxLayout).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelToolTip).ForeColor = Color.DimGray;
		((Control)labelToolTip).Location = new Point(0, 25);
		((Control)labelToolTip).Margin = new Padding(6, 0, 6, 0);
		((Control)labelToolTip).Name = "labelToolTip";
		((Control)labelToolTip).Size = new Size(174, 48);
		((Control)labelToolTip).TabIndex = 4;
		((Control)labelToolTip).Text = "ToolTip";
		labelToolTip.TextAlign = (ContentAlignment)16;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockSide = eDockSide.Top;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(198, 207);
		((Control)contextMenuBar).Margin = new Padding(1);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(100, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 12;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		((Control)comboBoxOnHoverMode).Anchor = (AnchorStyles)13;
		comboBoxOnHoverMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOnHoverMode).FormattingEnabled = true;
		((Control)comboBoxOnHoverMode).Location = new Point(182, 75);
		((Control)comboBoxOnHoverMode).Margin = new Padding(4);
		((Control)comboBoxOnHoverMode).Name = "comboBoxOnHoverMode";
		((Control)comboBoxOnHoverMode).Size = new Size(626, 42);
		((Control)comboBoxOnHoverMode).TabIndex = 0;
		((ComboBox)comboBoxOnHoverMode).SelectedIndexChanged += ComboBoxOnHoverMode_SelectedIndexChanged;
		((Control)labelOnHoverMode).ForeColor = Color.DimGray;
		((Control)labelOnHoverMode).Location = new Point(0, 79);
		((Control)labelOnHoverMode).Margin = new Padding(6, 0, 6, 0);
		((Control)labelOnHoverMode).Name = "labelOnHoverMode";
		((Control)labelOnHoverMode).Size = new Size(172, 33);
		((Control)labelOnHoverMode).TabIndex = 10;
		((Control)labelOnHoverMode).Text = "Mode";
		labelOnHoverMode.TextAlign = (ContentAlignment)16;
		((Control)panelBoxToolTip).Anchor = (AnchorStyles)13;
		((Control)panelBoxToolTip).BackColor = Color.White;
		((Control)panelBoxToolTip).Controls.Add((Control)(object)labelToolTipStatus);
		((Control)panelBoxToolTip).Controls.Add((Control)(object)buttonRemoveToolTip);
		((Control)panelBoxToolTip).Controls.Add((Control)(object)buttonEditToolTip);
		((Control)panelBoxToolTip).Location = new Point(182, 23);
		((Control)panelBoxToolTip).Margin = new Padding(4);
		((Control)panelBoxToolTip).Name = "panelBoxToolTip";
		((Control)panelBoxToolTip).Size = new Size(630, 48);
		((Control)panelBoxToolTip).TabIndex = 0;
		((Control)labelToolTipStatus).Anchor = (AnchorStyles)15;
		((Control)labelToolTipStatus).Location = new Point(2, 2);
		((Control)labelToolTipStatus).Margin = new Padding(6, 0, 6, 0);
		((Control)labelToolTipStatus).Name = "labelToolTipStatus";
		((Control)labelToolTipStatus).Size = new Size(532, 44);
		((Control)labelToolTipStatus).TabIndex = 0;
		labelToolTipStatus.TextAlign = (ContentAlignment)16;
		((Control)buttonRemoveToolTip).Anchor = (AnchorStyles)9;
		((Control)buttonRemoveToolTip).Location = new Point(580, 2);
		((Control)buttonRemoveToolTip).Margin = new Padding(4);
		((Control)buttonRemoveToolTip).Name = "buttonRemoveToolTip";
		((Control)buttonRemoveToolTip).Size = new Size(48, 44);
		((Control)buttonRemoveToolTip).TabIndex = 2;
		((Control)buttonRemoveToolTip).TabStop = false;
		((ButtonBase)buttonRemoveToolTip).UseVisualStyleBackColor = true;
		((Control)buttonRemoveToolTip).Click += ButtonRemoveToolTip_Click;
		((Control)buttonEditToolTip).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditToolTip).Image = (Image)componentResourceManager.GetObject("buttonEditToolTip.Image");
		((Control)buttonEditToolTip).Location = new Point(536, 2);
		((Control)buttonEditToolTip).Margin = new Padding(4);
		((Control)buttonEditToolTip).Name = "buttonEditToolTip";
		((Control)buttonEditToolTip).Size = new Size(46, 44);
		((Control)buttonEditToolTip).TabIndex = 1;
		((Control)buttonEditToolTip).TabStop = false;
		((ButtonBase)buttonEditToolTip).UseVisualStyleBackColor = true;
		((Control)buttonEditToolTip).Click += ButtonEditToolTip_Click;
		((Control)groupBoxOnHover).BackColor = Color.White;
		((Control)groupBoxOnHover).Controls.Add((Control)(object)panelBlockToolTip);
		((Control)groupBoxOnHover).Controls.Add((Control)(object)comboBoxOnHoverMode);
		((Control)groupBoxOnHover).Controls.Add((Control)(object)labelOnHoverMode);
		((Control)groupBoxOnHover).Dock = (DockStyle)1;
		groupBoxOnHover.HeaderHeight = 24;
		((Control)groupBoxOnHover).Location = new Point(24, 23);
		((Control)groupBoxOnHover).Margin = new Padding(6);
		((Control)groupBoxOnHover).Name = "groupBoxOnHover";
		((Control)groupBoxOnHover).Size = new Size(812, 221);
		((Control)groupBoxOnHover).TabIndex = 0;
		((Control)groupBoxOnHover).TabStop = false;
		((Control)groupBoxOnHover).Text = "On Hover";
		((Control)panelBlockToolTip).Controls.Add((Control)(object)panelBoxToolTip);
		((Control)panelBlockToolTip).Controls.Add((Control)(object)labelToolTip);
		((Control)panelBlockToolTip).Dock = (DockStyle)2;
		((Control)panelBlockToolTip).Location = new Point(0, 125);
		((Control)panelBlockToolTip).Margin = new Padding(4);
		((Control)panelBlockToolTip).Name = "panelBlockToolTip";
		((Control)panelBlockToolTip).Size = new Size(812, 96);
		((Control)panelBlockToolTip).TabIndex = 1;
		((Control)groupBoxOnClick).BackColor = Color.White;
		((Control)groupBoxOnClick).Controls.Add((Control)(object)panelBlockDashboard);
		((Control)groupBoxOnClick).Controls.Add((Control)(object)panelBlockHyperlink);
		((Control)groupBoxOnClick).Controls.Add((Control)(object)comboBoxOnClickMode);
		((Control)groupBoxOnClick).Controls.Add((Control)(object)labelOnClickMode);
		((Control)groupBoxOnClick).Controls.Add((Control)(object)panelHyperlinkDestination);
		((Control)groupBoxOnClick).Dock = (DockStyle)1;
		groupBoxOnClick.HeaderHeight = 24;
		((Control)groupBoxOnClick).Location = new Point(24, 244);
		((Control)groupBoxOnClick).Margin = new Padding(6);
		((Control)groupBoxOnClick).Name = "groupBoxOnClick";
		((Control)groupBoxOnClick).Size = new Size(812, 775);
		((Control)groupBoxOnClick).TabIndex = 1;
		((Control)groupBoxOnClick).TabStop = false;
		((Control)groupBoxOnClick).Text = "On Click";
		((Control)panelBlockDashboard).Controls.Add((Control)(object)panelParameterUi);
		((Control)panelBlockDashboard).Controls.Add((Control)(object)listBoxParameters);
		((Control)panelBlockDashboard).Controls.Add((Control)(object)comboBoxDrillDownPage);
		((Control)panelBlockDashboard).Controls.Add((Control)(object)labelShowDashboardPage);
		((Control)panelBlockDashboard).Controls.Add((Control)(object)panelParameterOption);
		((Control)panelBlockDashboard).Controls.Add((Control)(object)panelParameterDummy);
		((Control)panelBlockDashboard).Dock = (DockStyle)2;
		((Control)panelBlockDashboard).Location = new Point(0, 115);
		((Control)panelBlockDashboard).Margin = new Padding(4);
		((Control)panelBlockDashboard).Name = "panelBlockDashboard";
		((Control)panelBlockDashboard).Size = new Size(812, 458);
		((Control)panelBlockDashboard).TabIndex = 11;
		((Control)panelBlockDashboard).Visible = false;
		((Control)panelParameterUi).BackColor = Color.White;
		((Control)panelParameterUi).Controls.Add((Control)(object)buttonNewParameter);
		((Control)panelParameterUi).Controls.Add((Control)(object)labelShowDashboardParameters);
		((Control)panelParameterUi).Dock = (DockStyle)2;
		((Control)panelParameterUi).Location = new Point(0, 81);
		((Control)panelParameterUi).Margin = new Padding(4);
		((Control)panelParameterUi).Name = "panelParameterUi";
		((Control)panelParameterUi).Size = new Size(812, 81);
		((Control)panelParameterUi).TabIndex = 11;
		((Control)buttonNewParameter).Anchor = (AnchorStyles)13;
		((ButtonBase)buttonNewParameter).FlatAppearance.BorderSize = 0;
		((Control)buttonNewParameter).Location = new Point(402, 17);
		((Control)buttonNewParameter).Margin = new Padding(4);
		((Control)buttonNewParameter).Name = "buttonNewParameter";
		((Control)buttonNewParameter).Size = new Size(410, 46);
		((Control)buttonNewParameter).TabIndex = 1;
		((Control)buttonNewParameter).Text = "New Parameter";
		((ButtonBase)buttonNewParameter).UseVisualStyleBackColor = true;
		((Control)buttonNewParameter).Click += ButtonNewParameter_Click;
		labelShowDashboardParameters.AutoEllipsis = true;
		((Control)labelShowDashboardParameters).ForeColor = Color.DimGray;
		((Control)labelShowDashboardParameters).Location = new Point(0, 17);
		((Control)labelShowDashboardParameters).Margin = new Padding(6, 0, 6, 0);
		((Control)labelShowDashboardParameters).Name = "labelShowDashboardParameters";
		((Control)labelShowDashboardParameters).Size = new Size(388, 46);
		((Control)labelShowDashboardParameters).TabIndex = 10;
		((Control)labelShowDashboardParameters).Text = "Parameters";
		labelShowDashboardParameters.TextAlign = (ContentAlignment)16;
		((Control)listBoxParameters).AllowDrop = true;
		((Control)listBoxParameters).Dock = (DockStyle)2;
		((Control)listBoxParameters).Location = new Point(0, 162);
		((Control)listBoxParameters).Margin = new Padding(4);
		((Control)listBoxParameters).Name = "listBoxParameters";
		((Control)listBoxParameters).Size = new Size(812, 138);
		((Control)listBoxParameters).TabIndex = 12;
		((ListBox)listBoxParameters).SelectedIndexChanged += ListBoxParameters_SelectedIndexChanged;
		((Control)listBoxParameters).Enter += ListBoxParameters_Enter;
		((Control)listBoxParameters).MouseDown += new MouseEventHandler(ListBoxParameters_MouseDown);
		((Control)comboBoxDrillDownPage).Anchor = (AnchorStyles)13;
		comboBoxDrillDownPage.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDrillDownPage).FormattingEnabled = true;
		((Control)comboBoxDrillDownPage).Location = new Point(182, 23);
		((Control)comboBoxDrillDownPage).Margin = new Padding(4);
		((Control)comboBoxDrillDownPage).Name = "comboBoxDrillDownPage";
		((Control)comboBoxDrillDownPage).Size = new Size(626, 42);
		((Control)comboBoxDrillDownPage).TabIndex = 0;
		((ComboBox)comboBoxDrillDownPage).SelectedIndexChanged += ComboBoxDrillDownPage_SelectedIndexChanged;
		labelShowDashboardPage.AutoEllipsis = true;
		((Control)labelShowDashboardPage).ForeColor = Color.DimGray;
		((Control)labelShowDashboardPage).Location = new Point(0, 27);
		((Control)labelShowDashboardPage).Margin = new Padding(6, 0, 6, 0);
		((Control)labelShowDashboardPage).Name = "labelShowDashboardPage";
		((Control)labelShowDashboardPage).Size = new Size(180, 33);
		((Control)labelShowDashboardPage).TabIndex = 10;
		((Control)labelShowDashboardPage).Text = "Page";
		labelShowDashboardPage.TextAlign = (ContentAlignment)16;
		((Control)panelParameterOption).BackColor = Color.White;
		((Control)panelParameterOption).Controls.Add((Control)(object)panelBoxParameterValue);
		((Control)panelParameterOption).Controls.Add((Control)(object)comboBoxParameterName);
		((Control)panelParameterOption).Controls.Add((Control)(object)labelParameterName);
		((Control)panelParameterOption).Controls.Add((Control)(object)labelParameterValue);
		((Control)panelParameterOption).Dock = (DockStyle)2;
		((Control)panelParameterOption).Location = new Point(0, 300);
		((Control)panelParameterOption).Margin = new Padding(6);
		((Control)panelParameterOption).Name = "panelParameterOption";
		((Control)panelParameterOption).Size = new Size(812, 135);
		((Control)panelParameterOption).TabIndex = 13;
		((Control)panelParameterOption).Visible = false;
		((Control)panelBoxParameterValue).Anchor = (AnchorStyles)13;
		((Control)panelBoxParameterValue).BackColor = Color.White;
		((Control)panelBoxParameterValue).Controls.Add((Control)(object)buttonParameterPredefined);
		((Control)panelBoxParameterValue).Controls.Add((Control)(object)textBoxParameterExpression);
		((Control)panelBoxParameterValue).Controls.Add((Control)(object)buttonParameterValue);
		((Control)panelBoxParameterValue).Location = new Point(182, 81);
		((Control)panelBoxParameterValue).Margin = new Padding(4);
		((Control)panelBoxParameterValue).Name = "panelBoxParameterValue";
		((Control)panelBoxParameterValue).Size = new Size(630, 48);
		((Control)panelBoxParameterValue).TabIndex = 13;
		((Control)buttonParameterPredefined).Anchor = (AnchorStyles)9;
		((Control)buttonParameterPredefined).Location = new Point(582, 2);
		((Control)buttonParameterPredefined).Margin = new Padding(4);
		((Control)buttonParameterPredefined).Name = "buttonParameterPredefined";
		((Control)buttonParameterPredefined).Size = new Size(46, 44);
		((Control)buttonParameterPredefined).TabIndex = 2;
		((Control)buttonParameterPredefined).TabStop = false;
		((ButtonBase)buttonParameterPredefined).UseVisualStyleBackColor = true;
		((Control)buttonParameterPredefined).Click += ButtonParameterPredefined_Click;
		((Control)textBoxParameterExpression).AllowDrop = true;
		((Control)textBoxParameterExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxParameterExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxParameterExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxParameterExpression).Location = new Point(8, 12);
		((Control)textBoxParameterExpression).Margin = new Padding(4);
		((Control)textBoxParameterExpression).Name = "textBoxParameterExpression";
		((Control)textBoxParameterExpression).Size = new Size(524, 24);
		((Control)textBoxParameterExpression).TabIndex = 0;
		((Control)textBoxParameterExpression).TextChanged += TextBoxParameterValue_TextChanged;
		((Control)buttonParameterValue).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonParameterValue).Image = (Image)componentResourceManager.GetObject("buttonParameterValue.Image");
		((Control)buttonParameterValue).Location = new Point(538, 2);
		((Control)buttonParameterValue).Margin = new Padding(4);
		((Control)buttonParameterValue).Name = "buttonParameterValue";
		((Control)buttonParameterValue).Size = new Size(46, 44);
		((Control)buttonParameterValue).TabIndex = 1;
		((Control)buttonParameterValue).TabStop = false;
		((ButtonBase)buttonParameterValue).UseVisualStyleBackColor = true;
		((Control)buttonParameterValue).Click += ButtonParameterValue_Click;
		((Control)comboBoxParameterName).Anchor = (AnchorStyles)13;
		((Control)comboBoxParameterName).BackColor = Color.White;
		comboBoxParameterName.DrawMode = (DrawMode)2;
		comboBoxParameterName.FormattingEnabled = true;
		comboBoxParameterName.IsEditable = true;
		((Control)comboBoxParameterName).Location = new Point(182, 23);
		((Control)comboBoxParameterName).Margin = new Padding(4);
		((Control)comboBoxParameterName).Name = "comboBoxParameterName";
		comboBoxParameterName.SelectedIndex = -1;
		comboBoxParameterName.SelectedItem = null;
		comboBoxParameterName.SelectedValue = null;
		((Control)comboBoxParameterName).Size = new Size(630, 42);
		((Control)comboBoxParameterName).TabIndex = 11;
		((Control)comboBoxParameterName).TextChanged += ComboBoxParameterName_TextChanged;
		((Control)labelParameterName).ForeColor = Color.DimGray;
		((Control)labelParameterName).Location = new Point(0, 23);
		((Control)labelParameterName).Margin = new Padding(6, 0, 6, 0);
		((Control)labelParameterName).Name = "labelParameterName";
		((Control)labelParameterName).Size = new Size(172, 33);
		((Control)labelParameterName).TabIndex = 12;
		((Control)labelParameterName).Text = "Name";
		labelParameterName.TextAlign = (ContentAlignment)16;
		((Control)labelParameterValue).ForeColor = Color.DimGray;
		((Control)labelParameterValue).Location = new Point(0, 81);
		((Control)labelParameterValue).Margin = new Padding(6, 0, 6, 0);
		((Control)labelParameterValue).Name = "labelParameterValue";
		((Control)labelParameterValue).Size = new Size(174, 48);
		((Control)labelParameterValue).TabIndex = 6;
		((Control)labelParameterValue).Text = "Value";
		labelParameterValue.TextAlign = (ContentAlignment)16;
		((Control)panelParameterDummy).BackColor = Color.White;
		((Control)panelParameterDummy).Dock = (DockStyle)2;
		((Control)panelParameterDummy).Location = new Point(0, 435);
		((Control)panelParameterDummy).Margin = new Padding(4);
		((Control)panelParameterDummy).Name = "panelParameterDummy";
		((Control)panelParameterDummy).Size = new Size(812, 23);
		((Control)panelParameterDummy).TabIndex = 14;
		((Control)panelBlockHyperlink).Controls.Add((Control)(object)labelHyperlinkSample);
		((Control)panelBlockHyperlink).Controls.Add((Control)(object)labelExpressionSample2);
		((Control)panelBlockHyperlink).Controls.Add((Control)(object)panelBoxHyperlink);
		((Control)panelBlockHyperlink).Controls.Add((Control)(object)labelHyperlink);
		((Control)panelBlockHyperlink).Dock = (DockStyle)2;
		((Control)panelBlockHyperlink).Location = new Point(0, 573);
		((Control)panelBlockHyperlink).Margin = new Padding(4);
		((Control)panelBlockHyperlink).Name = "panelBlockHyperlink";
		((Control)panelBlockHyperlink).Size = new Size(812, 121);
		((Control)panelBlockHyperlink).TabIndex = 1;
		((Control)labelHyperlinkSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelHyperlinkSample).ForeColor = Color.DimGray;
		((Control)labelHyperlinkSample).Location = new Point(178, 79);
		((Control)labelHyperlinkSample).Margin = new Padding(6, 0, 6, 0);
		((Control)labelHyperlinkSample).Name = "labelHyperlinkSample";
		((Control)labelHyperlinkSample).Size = new Size(92, 25);
		((Control)labelHyperlinkSample).TabIndex = 17;
		((Control)labelHyperlinkSample).Text = "Sample:";
		labelHyperlinkSample.TextAlign = (ContentAlignment)16;
		((Control)labelExpressionSample2).Anchor = (AnchorStyles)13;
		((Label)labelExpressionSample2).AutoEllipsis = true;
		((Control)labelExpressionSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSample2).ForeColor = Color.DimGray;
		((Control)labelExpressionSample2).Location = new Point(280, 79);
		((Control)labelExpressionSample2).Margin = new Padding(6, 0, 6, 0);
		((Control)labelExpressionSample2).Name = "labelExpressionSample2";
		((Control)labelExpressionSample2).Size = new Size(526, 25);
		((Control)labelExpressionSample2).TabIndex = 16;
		((Control)labelExpressionSample2).Text = "http://example.com/{argument}/{value}";
		((Label)labelExpressionSample2).TextAlign = (ContentAlignment)16;
		((Control)labelExpressionSample2).MouseDown += new MouseEventHandler(LabelExpressionSample2_MouseDown);
		((Control)panelBoxHyperlink).Anchor = (AnchorStyles)13;
		((Control)panelBoxHyperlink).BackColor = Color.White;
		((Control)panelBoxHyperlink).Controls.Add((Control)(object)buttonHyperlinkPredefined);
		((Control)panelBoxHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)panelBoxHyperlink).Controls.Add((Control)(object)buttonEditHyperlink);
		((Control)panelBoxHyperlink).Location = new Point(182, 23);
		((Control)panelBoxHyperlink).Margin = new Padding(4);
		((Control)panelBoxHyperlink).Name = "panelBoxHyperlink";
		((Control)panelBoxHyperlink).Size = new Size(630, 48);
		((Control)panelBoxHyperlink).TabIndex = 0;
		((Control)buttonHyperlinkPredefined).Anchor = (AnchorStyles)9;
		((Control)buttonHyperlinkPredefined).Location = new Point(582, 2);
		((Control)buttonHyperlinkPredefined).Margin = new Padding(4);
		((Control)buttonHyperlinkPredefined).Name = "buttonHyperlinkPredefined";
		((Control)buttonHyperlinkPredefined).Size = new Size(46, 44);
		((Control)buttonHyperlinkPredefined).TabIndex = 3;
		((Control)buttonHyperlinkPredefined).TabStop = false;
		((ButtonBase)buttonHyperlinkPredefined).UseVisualStyleBackColor = true;
		((Control)buttonHyperlinkPredefined).Click += ButtonHyperlinkPredefined_Click;
		((Control)textBoxHyperlink).AllowDrop = true;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((Control)textBoxHyperlink).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxHyperlink).BorderStyle = (BorderStyle)0;
		((Control)textBoxHyperlink).Location = new Point(8, 12);
		((Control)textBoxHyperlink).Margin = new Padding(4);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(524, 24);
		((Control)textBoxHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).TextChanged += TextBoxHyperlink_TextChanged;
		((Control)buttonEditHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditHyperlink).Image = (Image)componentResourceManager.GetObject("buttonEditHyperlink.Image");
		((Control)buttonEditHyperlink).Location = new Point(538, 2);
		((Control)buttonEditHyperlink).Margin = new Padding(4);
		((Control)buttonEditHyperlink).Name = "buttonEditHyperlink";
		((Control)buttonEditHyperlink).Size = new Size(46, 44);
		((Control)buttonEditHyperlink).TabIndex = 1;
		((Control)buttonEditHyperlink).TabStop = false;
		((ButtonBase)buttonEditHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonEditHyperlink).Click += ButtonEditHyperlink_Click;
		((Control)labelHyperlink).ForeColor = Color.DimGray;
		((Control)labelHyperlink).Location = new Point(0, 29);
		((Control)labelHyperlink).Margin = new Padding(6, 0, 6, 0);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(182, 42);
		((Control)labelHyperlink).TabIndex = 4;
		((Control)labelHyperlink).Text = "Hyperlink";
		labelHyperlink.TextAlign = (ContentAlignment)16;
		((Control)comboBoxOnClickMode).Anchor = (AnchorStyles)13;
		comboBoxOnClickMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOnClickMode).FormattingEnabled = true;
		((Control)comboBoxOnClickMode).Location = new Point(182, 75);
		((Control)comboBoxOnClickMode).Margin = new Padding(4);
		((Control)comboBoxOnClickMode).Name = "comboBoxOnClickMode";
		((Control)comboBoxOnClickMode).Size = new Size(626, 42);
		((Control)comboBoxOnClickMode).TabIndex = 0;
		((ComboBox)comboBoxOnClickMode).SelectedIndexChanged += ComboBoxOnClickMode_SelectedIndexChanged;
		((Control)labelOnClickMode).ForeColor = Color.DimGray;
		((Control)labelOnClickMode).Location = new Point(0, 79);
		((Control)labelOnClickMode).Margin = new Padding(4, 0, 4, 0);
		((Control)labelOnClickMode).Name = "labelOnClickMode";
		((Control)labelOnClickMode).Size = new Size(172, 33);
		((Control)labelOnClickMode).TabIndex = 10;
		((Control)labelOnClickMode).Text = "Mode";
		labelOnClickMode.TextAlign = (ContentAlignment)16;
		((Control)panelHyperlinkDestination).Controls.Add((Control)(object)comboBoxHyperlinkDestination);
		((Control)panelHyperlinkDestination).Controls.Add((Control)(object)labelHyperlinkDestination);
		((Control)panelHyperlinkDestination).Dock = (DockStyle)2;
		((Control)panelHyperlinkDestination).Location = new Point(0, 694);
		((Control)panelHyperlinkDestination).Margin = new Padding(4);
		((Control)panelHyperlinkDestination).Name = "panelHyperlinkDestination";
		((Control)panelHyperlinkDestination).Size = new Size(812, 81);
		((Control)panelHyperlinkDestination).TabIndex = 2;
		((Control)panelHyperlinkDestination).Visible = false;
		((Control)comboBoxHyperlinkDestination).Anchor = (AnchorStyles)13;
		comboBoxHyperlinkDestination.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxHyperlinkDestination).FormattingEnabled = true;
		((Control)comboBoxHyperlinkDestination).Location = new Point(182, 23);
		((Control)comboBoxHyperlinkDestination).Margin = new Padding(4);
		((Control)comboBoxHyperlinkDestination).Name = "comboBoxHyperlinkDestination";
		((Control)comboBoxHyperlinkDestination).Size = new Size(626, 42);
		((Control)comboBoxHyperlinkDestination).TabIndex = 0;
		((ComboBox)comboBoxHyperlinkDestination).SelectedIndexChanged += ComboBoxHyperlinkDestination_SelectedIndexChanged;
		((Control)labelHyperlinkDestination).ForeColor = Color.DimGray;
		((Control)labelHyperlinkDestination).Location = new Point(-2, 23);
		((Control)labelHyperlinkDestination).Margin = new Padding(6, 0, 6, 0);
		((Control)labelHyperlinkDestination).Name = "labelHyperlinkDestination";
		((Control)labelHyperlinkDestination).Size = new Size(172, 33);
		((Control)labelHyperlinkDestination).TabIndex = 10;
		((Control)labelHyperlinkDestination).Text = "Destination";
		labelHyperlinkDestination.TextAlign = (ContentAlignment)16;
		((Control)panelTableInteraction).Controls.Add((Control)(object)checkBoxFullRowSelect);
		((Control)panelTableInteraction).Controls.Add((Control)(object)checkBoxDrillDownFiltered);
		((Control)panelTableInteraction).Dock = (DockStyle)2;
		((Control)panelTableInteraction).Location = new Point(0, 291);
		((Control)panelTableInteraction).Margin = new Padding(4);
		((Control)panelTableInteraction).Name = "panelTableInteraction";
		((Control)panelTableInteraction).Size = new Size(812, 117);
		((Control)panelTableInteraction).TabIndex = 4;
		((Control)panelTableInteraction).Visible = false;
		((Control)checkBoxFullRowSelect).Anchor = (AnchorStyles)13;
		((Control)checkBoxFullRowSelect).AutoSize = true;
		((Control)checkBoxFullRowSelect).Location = new Point(24, 79);
		((Control)checkBoxFullRowSelect).Margin = new Padding(6);
		((Control)checkBoxFullRowSelect).Name = "checkBoxFullRowSelect";
		((Control)checkBoxFullRowSelect).Size = new Size(193, 29);
		((Control)checkBoxFullRowSelect).TabIndex = 2;
		((Control)checkBoxFullRowSelect).Text = "Full Row Select";
		((ButtonBase)checkBoxFullRowSelect).UseVisualStyleBackColor = true;
		checkBoxFullRowSelect.CheckedChanged += CheckBoxFullRowSelect_CheckedChanged;
		((Control)checkBoxDrillDownFiltered).Anchor = (AnchorStyles)13;
		((Control)checkBoxDrillDownFiltered).AutoSize = true;
		checkBoxDrillDownFiltered.Checked = true;
		checkBoxDrillDownFiltered.CheckState = (CheckState)1;
		((Control)checkBoxDrillDownFiltered).Location = new Point(24, 23);
		((Control)checkBoxDrillDownFiltered).Margin = new Padding(6);
		((Control)checkBoxDrillDownFiltered).Name = "checkBoxDrillDownFiltered";
		((Control)checkBoxDrillDownFiltered).Size = new Size(232, 29);
		((Control)checkBoxDrillDownFiltered).TabIndex = 1;
		((Control)checkBoxDrillDownFiltered).Text = "Drill-Down Selected";
		((ButtonBase)checkBoxDrillDownFiltered).UseVisualStyleBackColor = true;
		checkBoxDrillDownFiltered.CheckedChanged += CheckBoxDrillDownSelected_CheckedChanged;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(498, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(662, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(24, 1877);
		((Control)groupLineButtons).Margin = new Padding(6);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(812, 69);
		((Control)groupLineButtons).TabIndex = 6;
		((Control)groupBoxOnDataManipulation).BackColor = Color.White;
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelUserColumnSelection);
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelUserSorting);
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelUserFiltering);
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelUserDrillDown);
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelTableInteraction);
		((Control)groupBoxOnDataManipulation).Controls.Add((Control)(object)panelOnDataManipulation);
		((Control)groupBoxOnDataManipulation).Dock = (DockStyle)1;
		groupBoxOnDataManipulation.HeaderHeight = 24;
		((Control)groupBoxOnDataManipulation).Location = new Point(24, 1019);
		((Control)groupBoxOnDataManipulation).Margin = new Padding(6);
		((Control)groupBoxOnDataManipulation).Name = "groupBoxOnDataManipulation";
		((Control)groupBoxOnDataManipulation).Size = new Size(812, 431);
		((Control)groupBoxOnDataManipulation).TabIndex = 2;
		((Control)groupBoxOnDataManipulation).TabStop = false;
		((Control)groupBoxOnDataManipulation).Text = "On Data Manipulation";
		((Control)panelUserColumnSelection).Controls.Add((Control)(object)checkBoxAllowUserColumnSelection);
		((Control)panelUserColumnSelection).Dock = (DockStyle)2;
		((Control)panelUserColumnSelection).Location = new Point(0, 51);
		((Control)panelUserColumnSelection).Margin = new Padding(4);
		((Control)panelUserColumnSelection).Name = "panelUserColumnSelection";
		((Control)panelUserColumnSelection).Size = new Size(812, 60);
		((Control)panelUserColumnSelection).TabIndex = 0;
		((Control)checkBoxAllowUserColumnSelection).Anchor = (AnchorStyles)13;
		((Control)checkBoxAllowUserColumnSelection).AutoSize = true;
		((Control)checkBoxAllowUserColumnSelection).Location = new Point(24, 23);
		((Control)checkBoxAllowUserColumnSelection).Margin = new Padding(6);
		((Control)checkBoxAllowUserColumnSelection).Name = "checkBoxAllowUserColumnSelection";
		((Control)checkBoxAllowUserColumnSelection).Size = new Size(220, 29);
		((Control)checkBoxAllowUserColumnSelection).TabIndex = 0;
		((Control)checkBoxAllowUserColumnSelection).Text = "Allow User Sorting";
		((ButtonBase)checkBoxAllowUserColumnSelection).UseVisualStyleBackColor = true;
		checkBoxAllowUserColumnSelection.CheckedChanged += CheckBoxAllowUserColumnSelection_CheckedChanged;
		((Control)panelUserSorting).Controls.Add((Control)(object)checkBoxAllowUserSorting);
		((Control)panelUserSorting).Dock = (DockStyle)2;
		((Control)panelUserSorting).Location = new Point(0, 111);
		((Control)panelUserSorting).Margin = new Padding(4);
		((Control)panelUserSorting).Name = "panelUserSorting";
		((Control)panelUserSorting).Size = new Size(812, 60);
		((Control)panelUserSorting).TabIndex = 1;
		((Control)checkBoxAllowUserSorting).Anchor = (AnchorStyles)13;
		((Control)checkBoxAllowUserSorting).AutoSize = true;
		((Control)checkBoxAllowUserSorting).Location = new Point(24, 23);
		((Control)checkBoxAllowUserSorting).Margin = new Padding(6);
		((Control)checkBoxAllowUserSorting).Name = "checkBoxAllowUserSorting";
		((Control)checkBoxAllowUserSorting).Size = new Size(220, 29);
		((Control)checkBoxAllowUserSorting).TabIndex = 0;
		((Control)checkBoxAllowUserSorting).Text = "Allow User Sorting";
		((ButtonBase)checkBoxAllowUserSorting).UseVisualStyleBackColor = true;
		checkBoxAllowUserSorting.CheckedChanged += CheckBoxAllowUserSorting_CheckedChanged;
		((Control)panelUserFiltering).Controls.Add((Control)(object)checkBoxAllowUserFiltering);
		((Control)panelUserFiltering).Dock = (DockStyle)2;
		((Control)panelUserFiltering).Location = new Point(0, 171);
		((Control)panelUserFiltering).Margin = new Padding(4);
		((Control)panelUserFiltering).Name = "panelUserFiltering";
		((Control)panelUserFiltering).Size = new Size(812, 60);
		((Control)panelUserFiltering).TabIndex = 2;
		((Control)checkBoxAllowUserFiltering).Anchor = (AnchorStyles)13;
		((Control)checkBoxAllowUserFiltering).AutoSize = true;
		((Control)checkBoxAllowUserFiltering).Location = new Point(24, 23);
		((Control)checkBoxAllowUserFiltering).Margin = new Padding(6);
		((Control)checkBoxAllowUserFiltering).Name = "checkBoxAllowUserFiltering";
		((Control)checkBoxAllowUserFiltering).Size = new Size(229, 29);
		((Control)checkBoxAllowUserFiltering).TabIndex = 0;
		((Control)checkBoxAllowUserFiltering).Text = "Allow User Filtering";
		((ButtonBase)checkBoxAllowUserFiltering).UseVisualStyleBackColor = true;
		checkBoxAllowUserFiltering.CheckedChanged += CheckBoxAllowUserFiltering_CheckedChanged;
		((Control)panelUserDrillDown).Controls.Add((Control)(object)checkBoxAllowUserDrillDown);
		((Control)panelUserDrillDown).Dock = (DockStyle)2;
		((Control)panelUserDrillDown).Location = new Point(0, 231);
		((Control)panelUserDrillDown).Margin = new Padding(4);
		((Control)panelUserDrillDown).Name = "panelUserDrillDown";
		((Control)panelUserDrillDown).Size = new Size(812, 60);
		((Control)panelUserDrillDown).TabIndex = 3;
		((Control)checkBoxAllowUserDrillDown).Anchor = (AnchorStyles)13;
		((Control)checkBoxAllowUserDrillDown).AutoSize = true;
		((Control)checkBoxAllowUserDrillDown).Location = new Point(24, 23);
		((Control)checkBoxAllowUserDrillDown).Margin = new Padding(6);
		((Control)checkBoxAllowUserDrillDown).Name = "checkBoxAllowUserDrillDown";
		((Control)checkBoxAllowUserDrillDown).Size = new Size(249, 29);
		((Control)checkBoxAllowUserDrillDown).TabIndex = 0;
		((Control)checkBoxAllowUserDrillDown).Text = "Allow User Drill Down";
		((ButtonBase)checkBoxAllowUserDrillDown).UseVisualStyleBackColor = true;
		checkBoxAllowUserDrillDown.CheckedChanged += CheckBoxAllowUserDrillDown_CheckedChanged;
		((Control)panelOnDataManipulation).Dock = (DockStyle)2;
		((Control)panelOnDataManipulation).Location = new Point(0, 408);
		((Control)panelOnDataManipulation).Margin = new Padding(4);
		((Control)panelOnDataManipulation).Name = "panelOnDataManipulation";
		((Control)panelOnDataManipulation).Size = new Size(812, 23);
		((Control)panelOnDataManipulation).TabIndex = 2;
		((Control)groupBoxFieldInteractions).BackColor = Color.White;
		((Control)groupBoxFieldInteractions).Controls.Add((Control)(object)labelRunFieldsEditorInfo);
		((Control)groupBoxFieldInteractions).Controls.Add((Control)(object)buttonRunFieldsEditor);
		((Control)groupBoxFieldInteractions).Dock = (DockStyle)1;
		groupBoxFieldInteractions.HeaderHeight = 24;
		((Control)groupBoxFieldInteractions).Location = new Point(24, 1698);
		((Control)groupBoxFieldInteractions).Margin = new Padding(6);
		((Control)groupBoxFieldInteractions).Name = "groupBoxFieldInteractions";
		((Control)groupBoxFieldInteractions).Size = new Size(812, 202);
		((Control)groupBoxFieldInteractions).TabIndex = 3;
		((Control)groupBoxFieldInteractions).TabStop = false;
		((Control)groupBoxFieldInteractions).Text = "Field Interactions";
		((Control)labelRunFieldsEditorInfo).Anchor = (AnchorStyles)15;
		((Control)labelRunFieldsEditorInfo).ForeColor = Color.DimGray;
		((Control)labelRunFieldsEditorInfo).Location = new Point(24, 56);
		((Control)labelRunFieldsEditorInfo).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRunFieldsEditorInfo).Name = "labelRunFieldsEditorInfo";
		((Control)labelRunFieldsEditorInfo).Size = new Size(764, 75);
		((Control)labelRunFieldsEditorInfo).TabIndex = 0;
		((Control)labelRunFieldsEditorInfo).Text = "Run the Fields Editor to change interaction of each table column";
		labelRunFieldsEditorInfo.TextAlign = (ContentAlignment)16;
		((Control)buttonRunFieldsEditor).Anchor = (AnchorStyles)13;
		((ButtonBase)buttonRunFieldsEditor).FlatAppearance.BorderSize = 0;
		((Control)buttonRunFieldsEditor).Location = new Point(346, 135);
		((Control)buttonRunFieldsEditor).Margin = new Padding(4);
		((Control)buttonRunFieldsEditor).Name = "buttonRunFieldsEditor";
		((Control)buttonRunFieldsEditor).Size = new Size(466, 46);
		((Control)buttonRunFieldsEditor).TabIndex = 1;
		((Control)buttonRunFieldsEditor).Text = "Run Fields Editor";
		((ButtonBase)buttonRunFieldsEditor).UseVisualStyleBackColor = true;
		((Control)buttonRunFieldsEditor).Click += ButtonRunFieldsEditor_Click;
		((Control)groupBoxLayout).BackColor = Color.White;
		((Control)groupBoxLayout).Controls.Add((Control)(object)checkBoxShowViewDataButton);
		((Control)groupBoxLayout).Controls.Add((Control)(object)checkBoxShowSaveButton);
		((Control)groupBoxLayout).Controls.Add((Control)(object)checkBoxShowFullScreenButton);
		((Control)groupBoxLayout).Dock = (DockStyle)1;
		groupBoxLayout.HeaderHeight = 24;
		((Control)groupBoxLayout).Location = new Point(24, 1450);
		((Control)groupBoxLayout).Margin = new Padding(6);
		((Control)groupBoxLayout).Name = "groupBoxLayout";
		((Control)groupBoxLayout).Size = new Size(812, 248);
		((Control)groupBoxLayout).TabIndex = 5;
		((Control)groupBoxLayout).TabStop = false;
		((Control)groupBoxLayout).Text = "Layout";
		((Control)checkBoxShowViewDataButton).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowViewDataButton).AutoSize = true;
		((Control)checkBoxShowViewDataButton).Location = new Point(24, 188);
		((Control)checkBoxShowViewDataButton).Margin = new Padding(6);
		((Control)checkBoxShowViewDataButton).Name = "checkBoxShowViewDataButton";
		((Control)checkBoxShowViewDataButton).Size = new Size(276, 29);
		((Control)checkBoxShowViewDataButton).TabIndex = 2;
		((Control)checkBoxShowViewDataButton).Text = "Show 'View Data' Button";
		((ButtonBase)checkBoxShowViewDataButton).UseVisualStyleBackColor = true;
		checkBoxShowViewDataButton.CheckedChanged += CheckBoxShowViewDataButton_CheckedChanged;
		((Control)checkBoxShowSaveButton).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowSaveButton).AutoSize = true;
		((Control)checkBoxShowSaveButton).Location = new Point(24, 133);
		((Control)checkBoxShowSaveButton).Margin = new Padding(6);
		((Control)checkBoxShowSaveButton).Name = "checkBoxShowSaveButton";
		((Control)checkBoxShowSaveButton).Size = new Size(228, 29);
		((Control)checkBoxShowSaveButton).TabIndex = 1;
		((Control)checkBoxShowSaveButton).Text = "Show 'Save' Button";
		((ButtonBase)checkBoxShowSaveButton).UseVisualStyleBackColor = true;
		checkBoxShowSaveButton.CheckedChanged += CheckBoxShowSaveButton_CheckedChanged;
		((Control)checkBoxShowFullScreenButton).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowFullScreenButton).AutoSize = true;
		((Control)checkBoxShowFullScreenButton).Location = new Point(24, 77);
		((Control)checkBoxShowFullScreenButton).Margin = new Padding(6);
		((Control)checkBoxShowFullScreenButton).Name = "checkBoxShowFullScreenButton";
		((Control)checkBoxShowFullScreenButton).Size = new Size(288, 29);
		((Control)checkBoxShowFullScreenButton).TabIndex = 0;
		((Control)checkBoxShowFullScreenButton).Text = "Show 'Full Screen' Button";
		((ButtonBase)checkBoxShowFullScreenButton).UseVisualStyleBackColor = true;
		checkBoxShowFullScreenButton.CheckedChanged += CheckBoxShowFullScreenButton_CheckedChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(894, 1062);
		((Control)this).Controls.Add((Control)(object)groupBoxFieldInteractions);
		((Control)this).Controls.Add((Control)(object)groupBoxLayout);
		((Control)this).Controls.Add((Control)(object)groupBoxOnDataManipulation);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Control)this).Controls.Add((Control)(object)groupBoxOnClick);
		((Control)this).Controls.Add((Control)(object)groupBoxOnHover);
		((Form)this).Margin = new Padding(6);
		((Control)this).Name = "StiElementInteractionEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Interaction";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelBoxToolTip).ResumeLayout(false);
		((Control)groupBoxOnHover).ResumeLayout(false);
		((Control)panelBlockToolTip).ResumeLayout(false);
		((Control)groupBoxOnClick).ResumeLayout(false);
		((Control)panelBlockDashboard).ResumeLayout(false);
		((Control)panelParameterUi).ResumeLayout(false);
		((Control)panelParameterOption).ResumeLayout(false);
		((Control)panelBoxParameterValue).ResumeLayout(false);
		((Control)panelBoxParameterValue).PerformLayout();
		((Control)panelBlockHyperlink).ResumeLayout(false);
		((Control)panelBoxHyperlink).ResumeLayout(false);
		((Control)panelBoxHyperlink).PerformLayout();
		((Control)panelHyperlinkDestination).ResumeLayout(false);
		((Control)panelTableInteraction).ResumeLayout(false);
		((Control)panelTableInteraction).PerformLayout();
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)groupBoxOnDataManipulation).ResumeLayout(false);
		((Control)panelUserColumnSelection).ResumeLayout(false);
		((Control)panelUserColumnSelection).PerformLayout();
		((Control)panelUserSorting).ResumeLayout(false);
		((Control)panelUserSorting).PerformLayout();
		((Control)panelUserFiltering).ResumeLayout(false);
		((Control)panelUserFiltering).PerformLayout();
		((Control)panelUserDrillDown).ResumeLayout(false);
		((Control)panelUserDrillDown).PerformLayout();
		((Control)groupBoxFieldInteractions).ResumeLayout(false);
		((Control)groupBoxLayout).ResumeLayout(false);
		((Control)groupBoxLayout).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
