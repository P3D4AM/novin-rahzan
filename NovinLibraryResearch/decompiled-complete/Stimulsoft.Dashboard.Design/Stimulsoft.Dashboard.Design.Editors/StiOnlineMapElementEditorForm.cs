using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Design.Editors.OnlineMap;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiOnlineMapElementEditorForm : StiElementEditorForm
{
	private const string ColumnLocation = "Location";

	private const string ColumnLongitude = "Longitude";

	private const string ColumnLatitude = "Latitude";

	private const string ColumnValue = "Value";

	private const string ColumnColor = "Color";

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox selectedListBox;

	private bool isShowMore;

	private StiPopup iconsPopup;

	private StiPopup iconsGisPopup;

	private Panel panelIcons;

	private Panel panelIconsGis;

	private StiHintTextBox textBoxSearch;

	private StiHintTextBox textBoxSearchGis;

	private bool scale125fixed;

	private Timer timerRefreshGrid;

	private string LocLocationAuto = Loc.GetEnum("StiOnlineMapLocationTypeAuto");

	private string LocLocationAdminDivision1 = Loc.GetEnum("StiOnlineMapLocationTypeAdminDivision1");

	private string LocLocationAdminDivision2 = Loc.GetEnum("StiOnlineMapLocationTypeAdminDivision2");

	private string LocLocationCountryRegion = Loc.GetEnum("StiOnlineMapLocationTypeCountryRegion");

	private string LocLocationNeighborhood = Loc.GetEnum("StiOnlineMapLocationTypeNeighborhood");

	private string LocLocationPopulatedPlace = Loc.GetEnum("StiOnlineMapLocationTypePopulatedPlace");

	private string LocLocationPostcode1 = Loc.GetEnum("StiOnlineMapLocationTypePostcode1");

	private string LocLocationPostcode2 = Loc.GetEnum("StiOnlineMapLocationTypePostcode2");

	private string LocLocationPostcode3 = Loc.GetEnum("StiOnlineMapLocationTypePostcode3");

	private string LocLocationPostcode4 = Loc.GetEnum("StiOnlineMapLocationTypePostcode4");

	private string LocLocationColorTypeSingle = Loc.GetEnum("BorderStyleFixedSingle");

	private string LocLocationColorTypeColorEach = Loc.GetMain("ColorEach");

	private string LocLocationColorTypeGroup = Loc.GetMain("Group");

	private string LocLocationColorTypeValue = Loc.GetMain("Value");

	private string LocLocationViewModeValue = Loc.GetMain("Value");

	private string LocLocationViewModeIcon = Loc.GetMain("Icon");

	private string LocLocationViewModeChart = Loc.Get("PropertyCategory", "ChartCategory");

	private string LocLocationViewModeBubble = Loc.Get("Chart", "Bubble");

	private string LocLocationViewModeHeatmap = Loc.GetEnum("StiMapTypeHeatmap");

	private string LocGradientTypeBlackAquaWhite = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeBlackAquaWhite");

	private string LocGradientTypeBlueRed = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeBlueRed");

	private string LocGradientTypeColorSpectrum = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeColorSpectrum");

	private string LocGradientTypeCustom = Loc.Get("Wizards", "Custom");

	private string LocGradientTypeDeepSea = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeDeepSea");

	private string LocGradientTypeHeatedMetal = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeHeatedMetal");

	private string LocGradientTypeIncandescent = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeIncandescent");

	private string LocGradientTypeSteppedColors = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeSteppedColors");

	private string LocGradientTypeSunrise = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeSunrise");

	private string LocGradientTypeVisibleSpectrum = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeVisibleSpectrum");

	private IContainer components;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLatitude;

	private StiPanelUX panelUi;

	private ToolTip toolTip;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLongitude;

	private StiChartGroupLabel labelLongitude;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelExpression;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiPanelUX panelLine;

	private StiLabelUX labelOr;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLocation;

	private StiLabelPanel labelLocation;

	private StiPanelUX panelOr;

	private StiLabelPanel labelLatitude;

	private StiPanelUX panelMoreOptions;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLocationValue;

	private StiChartGroupLabel labelLocationValue;

	private StiComboBox comboBoxViewMode;

	private StiLabelUX labelViewMode;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLocationColorMeter;

	private StiComboBox comboBoxLocationColorType;

	private StiColorBoxXF colorBoxLocationColor;

	private StiComboBox comboBoxCulture;

	private StiComboBox comboBoxLocationType;

	private StiLabelUX labelLocationColor;

	private StiLabelUX labelCulture;

	private StiLabelUX labelLocationType;

	private StiChartGroupLabel labelLocationColorMeter;

	private StiButtonUX buttonShowMore;

	private StiPanelUX moreOptionsPanel;

	private StiOperationButton buttonExpressionEdit;

	private StiPanelUX panelType;

	private StiPanelUX panelViewMode;

	private StiPanelUX panelCulture;

	private StiPanelUX panelColor;

	private StiPanelUX panelLocationColor;

	private StiPanelUX panelIcon;

	private StiPanelUX panelGisIcon;

	private StiLabelUX labelIcon;

	private StiLabelUX labelGisIcon;

	private StiPanelUX panelBoxIcon;

	private StiPanelUX panelBoxGisIcon;

	private StiOperationButton buttonIcon;

	private StiOperationButton buttonGisIcon;

	private StiColorBoxXF colorBoxLocationIconColor;

	private StiColorBoxXF colorBoxGisIconColor;

	private StiPanelUX panelCustomLocationIcon;

	private StiPanelUX panelCustomIcon;

	private StiOperationButton buttonOpen;

	private StiOperationButton buttonRemove;

	private StiOperationButton buttonCustomIcon;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLocationArgument;

	private StiChartGroupLabel labelLocationArgument;

	private StiPanelUX panelHeatmapColor;

	private StiComboBox comboBoxHeatmapColor;

	private StiLabelUX labelHeatmapColor;

	private StiPanelUX panelOrGis;

	private StiLabelUX labelOr2;

	private StiPanelUX panel2;

	private StiChartGroupLabel labelGis;

	private StiChartGroupLabel labelGisColor;

	private StiChartGroupLabel labelGisLineSize;

	private StiChartGroupLabel labelGisDescription;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxGis;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxGisColor;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxGisLineSize;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxGisDescription;

	private StiPanelUX panelGisProvider;

	private StiComboBox comboBoxProvider;

	private StiLabelUX labelProvider;

	private StiOperationButton buttonCustomizeColor;

	private StiExcelGrid gridData;

	private StiLabelPanel labelGridData;

	private StiOperationButton buttonUseDataFields;

	private StiOperationButton buttonEnterLocationDataManually;

	private StiOperationButton buttonEnterCoordinateDataManually;

	internal StiOnlineMapElement MapElement => base.Element as StiOnlineMapElement;

	internal StiLatitudeMapMeter MapLatitude => MapElement?.Latitude;

	internal StiLongitudeMapMeter MapLongitude => MapElement?.Longitude;

	internal StiLocationMapMeter MapLocation => MapElement?.Location;

	internal StiGisMapMeter MapGis => MapElement?.Gis;

	internal StiGisMapColorMeter MapGisColor => MapElement?.GisColor;

	internal StiGisMapLineSizeMeter MapGisLineSize => MapElement?.LineSize;

	internal StiGisMapDescriptionMeter MapGisDescription => MapElement?.Description;

	internal StiLocationValueMapMeter MapLocationValue => MapElement?.LocationValue;

	internal StiLocationColorMapMeter MapLocationColorMeter => MapElement?.LocationColorMeter;

	internal StiLocationArgumentMapMeter MapLocationArgumentMeter => MapElement?.LocationArgument;

	private bool IsMapLocationSpecified
	{
		get
		{
			if (MapLocation != null)
			{
				return IsDataFieldsMode;
			}
			return false;
		}
	}

	internal bool IsLatitudePresent => ((ListBox)listBoxLatitude).Items.Count > 0;

	internal bool IsLongitudePresent => ((ListBox)listBoxLongitude).Items.Count > 0;

	internal bool IsLocationPresent
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ((ListBox)listBoxLocation).Items.Count > 0;
			}
			return true;
		}
	}

	internal bool IsGisPresent => ((ListBox)listBoxGis).Items.Count > 0;

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsLatitudePresent && !IsLongitudePresent)
			{
				return IsLocationPresent;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LatitudeListBox => listBoxLatitude;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LongitudeListBox => listBoxLongitude;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox GisListBox => listBoxGis;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox GisColorListBox => listBoxGisColor;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox GisLineSizeListBox => listBoxGisLineSize;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox GisDescriptionListBox => listBoxGisDescription;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LocationListBox => listBoxLocation;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LocationValueListBox => listBoxLocationValue;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LocationColorMeterListBox => listBoxLocationColorMeter;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LocationArgumentMeterListBox => listBoxLocationArgument;

	internal bool UseCustomIcon => MapElement.CustomIcon != null;

	private bool IsDataEnterMode => MapElement.DataMode == StiDataMode.ManuallyEnteringData;

	private bool IsDataFieldsMode => MapElement.DataMode == StiDataMode.UsingDataFields;

	private bool IsMapCoordinateMode => MapElement.MapMode == StiOnlineMapMode.Coordinate;

	private bool IsMapLocationMode => MapElement.MapMode == StiOnlineMapMode.Location;

	private string LocLocationColor
	{
		get
		{
			StiOnlineMapElement mapElement = MapElement;
			if (mapElement != null && mapElement.LocationColorType == StiOnlineMapLocationColorType.Group)
			{
				return Loc.GetMain("Color") + " " + Loc.GetMain("Group");
			}
			StiOnlineMapElement mapElement2 = MapElement;
			if (mapElement2 != null && mapElement2.LocationColorType == StiOnlineMapLocationColorType.Value)
			{
				return Loc.GetMain("Color") + " " + Loc.GetMain("Value");
			}
			return Loc.GetMain("Color");
		}
	}

	public StiOnlineMapElementEditorForm()
		: this(null)
	{
	}

	public StiOnlineMapElementEditorForm(StiElement element)
		: base(element)
	{
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		InitializeComponent();
		colorBoxLocationColor.SelectedColor = Color.LimeGreen;
		colorBoxLocationIconColor.SelectedColor = Color.Maroon;
		colorBoxGisIconColor.SelectedColor = Color.Lime;
		((ScrollableControl)this).AutoScroll = false;
		if (StiOptions.Appearance.ShowFormsHelp && StiLocalization.CultureName == "ru")
		{
			((Form)this).HelpButton = true;
		}
		((ComboBox)comboBoxProvider).DropDownHeight = StiScale.XXI(280.0);
		selectedListBox = listBoxLocation;
		List<Stimulsoft.Dashboard.Design.Controls.StiMeterListBox> list = (from Control c in (IEnumerable)((Control)this).Controls
			where c is Stimulsoft.Dashboard.Design.Controls.StiMeterListBox
			select c).Cast<Stimulsoft.Dashboard.Design.Controls.StiMeterListBox>().ToList();
		list.Add(listBoxLocationValue);
		list.Add(listBoxLocationColorMeter);
		list.Add(listBoxLocationArgument);
		foreach (Stimulsoft.Dashboard.Design.Controls.StiMeterListBox item in list)
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = false;
			item.AllowImage = false;
			((Control)item).Height = StiScale.I(48);
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		if (MapElement.LocationColor != Color.LimeGreen || MapElement.ValueViewMode != 0 || MapElement.LocationType != 0 || MapElement.Culture != StiOnlineMapCulture.en_US || MapElement.LocationColorType != 0 || MapElement.LocationColorMeter != null || MapElement.LocationArgument != null)
		{
			isShowMore = true;
		}
		LocalizeInternal();
		SetLabelHeights();
		ReadCultureFromElement();
		ReadPropertiesFromElement();
		InitializeMenus();
		AttachEvents();
		ActivateDictionaryPanel();
		RefreshUiPanel();
		DoScaling();
		((DataGridView)gridData).BorderStyle = (BorderStyle)1;
		((DataGridView)gridData).CellValueChanged += new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged += GridData_CellRowsChanged;
		((Control)gridData).Height = StiScale.I(300);
		((DataGridView)gridData).AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)16;
		((Form)this).Load += This_LoadAsync;
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxLatitude).Invalidate();
		((Control)listBoxLongitude).Invalidate();
		((Control)listBoxGis).Invalidate();
		((Control)listBoxGisColor).Invalidate();
		((Control)listBoxGisLineSize).Invalidate();
		((Control)listBoxGisDescription).Invalidate();
		((Control)listBoxLocation).Invalidate();
		((Control)listBoxLocationValue).Invalidate();
		((Control)listBoxLocationColorMeter).Invalidate();
		((Control)listBoxLocationArgument).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiOnlineMap");
		((Control)labelGridData).Text = Loc.GetMain("Data");
		((Control)labelProvider).Text = Loc.GetMain("Provider");
		((Control)labelLatitude).Text = Loc.GetMain("Latitude");
		((Control)labelLongitude).Text = Loc.GetMain("Longitude");
		((Control)labelExpression).Text = Loc.GetMain("Expression");
		((Control)labelLocation).Text = Loc.GetMain("Location");
		StiLabelUX stiLabelUX = labelOr;
		string text2 = (((Control)labelOr2).Text = Loc.GetEnum("StiFilterModeOr").ToLowerInvariant());
		((Control)stiLabelUX).Text = text2;
		((Control)labelExpression).Text = Loc.GetMain("Expression");
		((Control)labelLocationType).Text = Loc.Get("Export", "Type").Replace(":", "");
		((Control)labelCulture).Text = Loc.GetMain("Culture");
		((Control)labelLocationColor).Text = Loc.GetMain("Color");
		((Control)labelLocationValue).Text = Loc.GetMain("Value");
		((Control)labelViewMode).Text = Loc.GetMain("Mode");
		((Control)labelIcon).Text = Loc.GetMain("Icon");
		((Control)labelGisIcon).Text = Loc.GetMain("Icon");
		((Control)labelLocationArgument).Text = Loc.GetMain("Argument");
		((Control)labelHeatmapColor).Text = Loc.GetMain("Color");
		((Control)labelGisColor).Text = Loc.GetMain("Color");
		((Control)labelGisLineSize).Text = Loc.GetMain("Size");
		((Control)labelGisDescription).Text = Loc.GetMain("Description");
		LocLocationAuto = Loc.GetEnum("StiOnlineMapLocationTypeAuto");
		LocLocationAdminDivision1 = Loc.GetEnum("StiOnlineMapLocationTypeAdminDivision1");
		LocLocationAdminDivision2 = Loc.GetEnum("StiOnlineMapLocationTypeAdminDivision2");
		LocLocationCountryRegion = Loc.GetEnum("StiOnlineMapLocationTypeCountryRegion");
		LocLocationNeighborhood = Loc.GetEnum("StiOnlineMapLocationTypeNeighborhood");
		LocLocationPopulatedPlace = Loc.GetEnum("StiOnlineMapLocationTypePopulatedPlace");
		LocLocationPostcode1 = Loc.GetEnum("StiOnlineMapLocationTypePostcode1");
		LocLocationPostcode2 = Loc.GetEnum("StiOnlineMapLocationTypePostcode2");
		LocLocationPostcode3 = Loc.GetEnum("StiOnlineMapLocationTypePostcode3");
		LocLocationPostcode4 = Loc.GetEnum("StiOnlineMapLocationTypePostcode4");
		LocLocationColorTypeSingle = Loc.GetEnum("BorderStyleFixedSingle");
		LocLocationColorTypeColorEach = Loc.GetMain("ColorEach");
		LocLocationColorTypeGroup = Loc.GetMain("Group");
		LocLocationColorTypeValue = Loc.GetMain("Value");
		LocLocationViewModeHeatmap = Loc.GetEnum("StiMapTypeHeatmap");
		LocLocationViewModeBubble = Loc.Get("Chart", "Bubble");
		LocLocationViewModeValue = Loc.GetMain("Value");
		LocLocationViewModeIcon = Loc.GetMain("Icon");
		LocLocationViewModeChart = Loc.Get("PropertyCategory", "ChartCategory");
		LocGradientTypeBlackAquaWhite = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeBlackAquaWhite");
		LocGradientTypeBlueRed = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeBlueRed");
		LocGradientTypeColorSpectrum = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeColorSpectrum");
		LocGradientTypeCustom = Loc.Get("Wizards", "Custom");
		LocGradientTypeDeepSea = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeDeepSea");
		LocGradientTypeHeatedMetal = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeHeatedMetal");
		LocGradientTypeIncandescent = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeIncandescent");
		LocGradientTypeSteppedColors = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeSteppedColors");
		LocGradientTypeSunrise = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeSunrise");
		LocGradientTypeVisibleSpectrum = Loc.GetEnum("StiOnlineMapHeatmapColorGradientTypeVisibleSpectrum");
		((Control)labelLocationColorMeter).Text = LocLocationColor;
		StiExcelGridLocalizer.Localize(gridData);
		LocalizeGridDataColumns();
		toolTip.SetToolTip((Control)(object)buttonCustomizeColor, Loc.Get("Gui", "barsys_customize_tooltip"));
		toolTip.SetToolTip((Control)(object)buttonUseDataFields, Loc.Get("Report", "UseDataFields"));
		toolTip.SetToolTip((Control)(object)buttonEnterLocationDataManually, Loc.Get("Report", "EnterDataManually"));
		toolTip.SetToolTip((Control)(object)buttonEnterCoordinateDataManually, Loc.Get("Report", "EnterDataManually"));
		((ComboBox)comboBoxLocationType).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxLocationType).SelectedIndex;
		((ComboBox)comboBoxLocationType).Items.Clear();
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationAuto);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationAdminDivision1);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationAdminDivision2);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationCountryRegion);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationNeighborhood);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationPopulatedPlace);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationPostcode1);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationPostcode2);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationPostcode3);
		((ComboBox)comboBoxLocationType).Items.Add((object)LocLocationPostcode4);
		((ListControl)comboBoxLocationType).SelectedIndex = ((selectedIndex != -1) ? selectedIndex : 2);
		((ComboBox)comboBoxLocationType).EndUpdate();
		((ComboBox)comboBoxProvider).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxProvider).SelectedIndex;
		((ComboBox)comboBoxProvider).Items.Clear();
		Array values = Enum.GetValues(typeof(StiGeoMapProviderType));
		foreach (StiGeoMapProviderType item in values)
		{
			if (item != StiGeoMapProviderType.YandexMap && item != StiGeoMapProviderType.YandexSatelliteMap)
			{
				StiOnlineMapGisItem stiOnlineMapGisItem = new StiOnlineMapGisItem(item);
				((ComboBox)comboBoxProvider).Items.Add((object)stiOnlineMapGisItem);
			}
		}
		((ListControl)comboBoxProvider).SelectedIndex = ((selectedIndex != -1) ? selectedIndex : 2);
		((ComboBox)comboBoxProvider).EndUpdate();
		((ComboBox)comboBoxLocationColorType).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxLocationColorType).SelectedIndex;
		((ComboBox)comboBoxLocationColorType).Items.Clear();
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeSingle);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeColorEach);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeGroup);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeValue);
		((ListControl)comboBoxLocationColorType).SelectedIndex = ((selectedIndex != -1) ? selectedIndex : 2);
		((ComboBox)comboBoxLocationColorType).EndUpdate();
		LocalizeViewMode();
		((ComboBox)comboBoxLocationColorType).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxLocationColorType).SelectedIndex;
		((ComboBox)comboBoxLocationColorType).Items.Clear();
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeSingle);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeColorEach);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeGroup);
		((ComboBox)comboBoxLocationColorType).Items.Add((object)LocLocationColorTypeValue);
		((ListControl)comboBoxLocationColorType).SelectedIndex = ((selectedIndex != -1) ? selectedIndex : 2);
		((ComboBox)comboBoxLocationColorType).EndUpdate();
		((ComboBox)comboBoxHeatmapColor).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxHeatmapColor).SelectedIndex;
		((ComboBox)comboBoxHeatmapColor).Items.Clear();
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeBlackAquaWhite);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeBlueRed);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeColorSpectrum);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeCustom);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeDeepSea);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeHeatedMetal);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeIncandescent);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeSteppedColors);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeSunrise);
		((ComboBox)comboBoxHeatmapColor).Items.Add((object)LocGradientTypeVisibleSpectrum);
		((ListControl)comboBoxHeatmapColor).SelectedIndex = ((selectedIndex != -1) ? selectedIndex : 7);
		((ComboBox)comboBoxHeatmapColor).EndUpdate();
		((Control)buttonShowMore).Text = (isShowMore ? Loc.Get("Buttons", "LessOptions") : Loc.Get("Buttons", "MoreOptions"));
	}

	private void LocalizeGridDataColumns()
	{
		SetColumnHeaderText("Location", Loc.GetMain("Location"));
		SetColumnHeaderText("Latitude", Loc.GetMain("Latitude"));
		SetColumnHeaderText("Longitude", Loc.GetMain("Longitude"));
		SetColumnHeaderText("Value", Loc.GetMain("Value"));
		SetColumnHeaderText("Color", LocLocationColor);
	}

	private void LocalizeViewMode()
	{
		((ComboBox)comboBoxViewMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxViewMode).SelectedIndex;
		((ComboBox)comboBoxViewMode).Items.Clear();
		if (IsMapLocationSpecified || IsDataEnterMode)
		{
			((ComboBox)comboBoxViewMode).Items.Add((object)LocLocationViewModeBubble);
			((ComboBox)comboBoxViewMode).Items.Add((object)LocLocationViewModeValue);
		}
		((ComboBox)comboBoxViewMode).Items.Add((object)LocLocationViewModeIcon);
		if (IsMapLocationSpecified || IsDataEnterMode)
		{
			((ComboBox)comboBoxViewMode).Items.Add((object)LocLocationViewModeChart);
		}
		((ComboBox)comboBoxViewMode).Items.Add((object)LocLocationViewModeHeatmap);
		((ListControl)comboBoxViewMode).SelectedIndex = ((selectedIndex != -1 && selectedIndex < ((ComboBox)comboBoxViewMode).Items.Count) ? selectedIndex : 0);
		((ComboBox)comboBoxViewMode).EndUpdate();
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiOnlineMapExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEnterLocationDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonEnterCoordinateDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonUseDataFields).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonCustomizeColor).Image = StiReportDesignImages.Actions.Edit();
		int width;
		if (StiScale.Factor == 1.25 && !scale125fixed)
		{
			StiPanelBox stiPanelBox = panelBoxExpression;
			width = ((Control)stiPanelBox).Width;
			((Control)stiPanelBox).Width = width - 1;
			scale125fixed = true;
		}
		StiComboBox stiComboBox = comboBoxCulture;
		StiComboBox stiComboBox2 = comboBoxLocationColorType;
		StiComboBox stiComboBox3 = comboBoxLocationType;
		StiComboBox stiComboBox4 = comboBoxViewMode;
		int num2 = (((Control)comboBoxProvider).Width = ((Control)panelType).Width - ((Control)comboBoxCulture).Left);
		int num4 = (((Control)stiComboBox4).Width = num2);
		int num6 = (((Control)stiComboBox3).Width = num4);
		width = (((Control)stiComboBox2).Width = num6);
		((Control)stiComboBox).Width = width;
		((Control)colorBoxLocationIconColor).Width = ((Control)panelBoxExpression).Right - ((Control)colorBoxLocationIconColor).Left;
		((Control)colorBoxGisIconColor).Width = ((Control)panelBoxExpression).Right - ((Control)colorBoxGisIconColor).Left;
		((Control)buttonShowMore).Width = ((Control)panelBoxExpression).Right - ((Control)buttonShowMore).Left + 1;
		((Control)panelBoxIcon).Left = ((Control)comboBoxViewMode).Left;
		((Control)panelBoxGisIcon).Left = ((Control)comboBoxViewMode).Left;
		StiOperationButton stiOperationButton = buttonCustomizeColor;
		width = (((Control)buttonCustomizeColor).Height = ((Control)comboBoxHeatmapColor).Height + 2);
		((Control)stiOperationButton).Width = width;
		((Control)buttonCustomizeColor).Left = ((Control)buttonShowMore).Right - ((Control)buttonCustomizeColor).Width;
		((Control)buttonCustomizeColor).Top = ((Control)comboBoxHeatmapColor).Top - 1;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxIcon);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxGisIcon);
		if (StiScale.Factor < 2.0)
		{
			int num9 = (from string i in (IEnumerable)((ComboBox)comboBoxLocationType).Items
				select TextRenderer.MeasureText(i, ((Control)comboBoxLocationType).Font).Width).Max();
			if (num9 > ((Control)comboBoxLocationType).Width)
			{
				comboBoxLocationType.AutoDropDownWidth = false;
				((ComboBox)comboBoxLocationType).DropDownWidth = StiScale.I(10 + num9);
			}
			num9 = (from StiOnlineMapGisItem i in (IEnumerable)((ComboBox)comboBoxProvider).Items
				select TextRenderer.MeasureText(i.ToString(), ((Control)comboBoxLocationType).Font).Width).Max();
			if (num9 > ((Control)comboBoxProvider).Width)
			{
				comboBoxProvider.AutoDropDownWidth = false;
				((ComboBox)comboBoxProvider).DropDownWidth = StiScale.I(10 + num9);
			}
		}
	}

	private void RefreshImageAndStatus()
	{
		((Control)panelCustomIcon).Invalidate();
		RefreshElementView();
		CheckStatus();
	}

	private void CheckStatus()
	{
		((Control)buttonRemove).Enabled = MapElement.CustomIcon != null;
	}

	private void SetLabelHeights()
	{
		((Control)panelCustomLocationIcon).Height = StiScale.I(135);
		((Control)labelLocation).Height = StiScale.I(31);
		((Control)labelLocationValue).Height = StiScale.I(31);
		((Control)labelLocationArgument).Height = StiScale.I(31);
		((Control)labelLocationColorMeter).Height = StiScale.I(31);
		((Control)labelLatitude).Height = StiScale.I(31);
		((Control)labelLongitude).Height = StiScale.I(31);
		((Control)labelExpression).Height = StiScale.I(31);
		((Control)labelGis).Height = StiScale.I(31);
		((Control)labelGisColor).Height = StiScale.I(31);
		((Control)labelGisLineSize).Height = StiScale.I(31);
		((Control)labelGisDescription).Height = StiScale.I(31);
	}

	public override void ReadPropertiesFromElement()
	{
		ReadGisColorPropertiesFromElement();
		ReadGisLineSizePropertiesFromElement();
		ReadGisDescriptionPropertiesFromElement();
		ReadGisPropertiesFromElement();
		ReadLatitudePropertiesFromElement();
		ReadLongitudePropertiesFromElement();
		ReadLocationPropertiesFromElement();
		ReadIconColorPropertiesFromElement();
		colorBoxGisIconColor.SelectedColor = MapElement.GisSettings.IconColor;
		((ListControl)comboBoxProvider).SelectedIndex = (int)MapElement.Provider;
	}

	private void ReadCultureFromElement()
	{
		((ComboBox)comboBoxCulture).BeginUpdate();
		((ComboBox)comboBoxCulture).Items.Clear();
		StiOnlineMapCulture[] array = (StiOnlineMapCulture[])Enum.GetValues(typeof(StiOnlineMapCulture));
		for (int i = 0; i < array.Length; i++)
		{
			StiOnlineMapCulture stiOnlineMapCulture = array[i];
			((ComboBox)comboBoxCulture).Items.Add((object)stiOnlineMapCulture.ToString().Replace('_', '-'));
		}
		string text = ((StiLocalization.CultureName == "en") ? "en-US" : StiLocalization.CultureName);
		((ListControl)comboBoxCulture).SelectedIndex = Math.Max(0, ((ComboBox)comboBoxCulture).Items.IndexOf((object)text));
		((ComboBox)comboBoxCulture).EndUpdate();
	}

	private void ReadLatitudePropertiesFromElement()
	{
		if (!listBoxLatitude.ItemsEqualTo(MapLatitude))
		{
			((ListBox)listBoxLatitude).BeginUpdate();
			((ListBox)listBoxLatitude).Items.Clear();
			listBoxLatitude.AddItem(MapLatitude);
			listBoxLatitude.SelectFirstItem();
			((ListBox)listBoxLatitude).EndUpdate();
		}
	}

	private void ReadLongitudePropertiesFromElement()
	{
		if (!listBoxLongitude.ItemsEqualTo(MapLongitude))
		{
			((ListBox)listBoxLongitude).BeginUpdate();
			((ListBox)listBoxLongitude).Items.Clear();
			listBoxLongitude.AddItem(MapLongitude);
			listBoxLongitude.SelectFirstItem();
			((ListBox)listBoxLongitude).EndUpdate();
		}
	}

	private void ShowHeatmapGradientForm()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		StiHeatmapGradientForm stiHeatmapGradientForm = new StiHeatmapGradientForm(MapElement.Report, this, Designer, GetRanges(), MapElement.HeatmapRadius);
		try
		{
			((Form)stiHeatmapGradientForm).Owner = (Form)(object)this;
			if ((int)((Form)stiHeatmapGradientForm).ShowDialog() == 1)
			{
				MapElement.HeatmapRadius = stiHeatmapGradientForm.Raduis;
				MapElement.HeatmapGradientRanges = stiHeatmapGradientForm.Ranges.Cast<IStiHeatmapGradientRange>().ToList();
				MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.Custom;
				ReadLocationPropertiesFromElement();
				RefreshUiPanel();
				RefreshElementView();
			}
		}
		finally
		{
			((IDisposable)stiHeatmapGradientForm)?.Dispose();
		}
	}

	private List<StiHeatmapGradientRange> GetRanges()
	{
		List<StiHeatmapGradientRange> list = new List<StiHeatmapGradientRange>();
		switch (MapElement.HeatmapColorGradientType)
		{
		case StiOnlineMapHeatmapColorGradientType.BlackAquaWhite:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Black));
			list.Add(new StiHeatmapGradientRange(50.0, Color.Aqua));
			list.Add(new StiHeatmapGradientRange(100.0, Color.White));
			break;
		case StiOnlineMapHeatmapColorGradientType.BlueRed:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Blue));
			list.Add(new StiHeatmapGradientRange(100.0, Color.Red));
			break;
		case StiOnlineMapHeatmapColorGradientType.DeepSea:
			list.Add(new StiHeatmapGradientRange(0.0, Color.FromArgb(0, 0, 0)));
			list.Add(new StiHeatmapGradientRange(60.0, Color.FromArgb(24, 53, 103)));
			list.Add(new StiHeatmapGradientRange(75.0, Color.FromArgb(46, 100, 158)));
			list.Add(new StiHeatmapGradientRange(90.0, Color.FromArgb(23, 173, 203)));
			list.Add(new StiHeatmapGradientRange(100.0, Color.FromArgb(0, 250, 250)));
			break;
		case StiOnlineMapHeatmapColorGradientType.ColorSpectrum:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Navy));
			list.Add(new StiHeatmapGradientRange(25.0, Color.Blue));
			list.Add(new StiHeatmapGradientRange(50.0, Color.Green));
			list.Add(new StiHeatmapGradientRange(75.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(100.0, Color.Red));
			break;
		case StiOnlineMapHeatmapColorGradientType.Incandescent:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Black));
			list.Add(new StiHeatmapGradientRange(33.0, Color.DarkRed));
			list.Add(new StiHeatmapGradientRange(66.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(100.0, Color.White));
			break;
		case StiOnlineMapHeatmapColorGradientType.HeatedMetal:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Black));
			list.Add(new StiHeatmapGradientRange(40.0, Color.Purple));
			list.Add(new StiHeatmapGradientRange(60.0, Color.Red));
			list.Add(new StiHeatmapGradientRange(80.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(100.0, Color.White));
			break;
		case StiOnlineMapHeatmapColorGradientType.Sunrise:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Red));
			list.Add(new StiHeatmapGradientRange(66.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(100.0, Color.White));
			break;
		case StiOnlineMapHeatmapColorGradientType.SteppedColors:
			list.Add(new StiHeatmapGradientRange(0.0, Color.Navy));
			list.Add(new StiHeatmapGradientRange(25.0, Color.Navy));
			list.Add(new StiHeatmapGradientRange(26.0, Color.Green));
			list.Add(new StiHeatmapGradientRange(5.0, Color.Green));
			list.Add(new StiHeatmapGradientRange(51.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(75.0, Color.Yellow));
			list.Add(new StiHeatmapGradientRange(76.0, Color.Red));
			list.Add(new StiHeatmapGradientRange(100.0, Color.Red));
			break;
		case StiOnlineMapHeatmapColorGradientType.VisibleSpectrum:
			list.Add(new StiHeatmapGradientRange(0.0, Color.FromArgb(255, 0, 255)));
			list.Add(new StiHeatmapGradientRange(25.0, Color.FromArgb(0, 0, 255)));
			list.Add(new StiHeatmapGradientRange(50.0, Color.FromArgb(0, 255, 0)));
			list.Add(new StiHeatmapGradientRange(75.0, Color.FromArgb(255, 255, 0)));
			list.Add(new StiHeatmapGradientRange(100.0, Color.FromArgb(255, 0, 0)));
			break;
		case StiOnlineMapHeatmapColorGradientType.Custom:
			list.AddRange(MapElement.HeatmapGradientRanges.Select((IStiHeatmapGradientRange r) => r.Clone()).Cast<StiHeatmapGradientRange>().ToList());
			break;
		}
		return list;
	}

	private void ReadGisPropertiesFromElement()
	{
		if (!listBoxGis.ItemsEqualTo(MapGis))
		{
			((ListBox)listBoxGis).BeginUpdate();
			((ListBox)listBoxGis).Items.Clear();
			listBoxGis.AddItem(MapGis);
			listBoxGis.SelectFirstItem();
			((ListBox)listBoxGis).EndUpdate();
		}
	}

	private void ReadGisColorPropertiesFromElement()
	{
		if (!listBoxGisColor.ItemsEqualTo(MapGisColor))
		{
			((ListBox)listBoxGisColor).BeginUpdate();
			((ListBox)listBoxGisColor).Items.Clear();
			listBoxGisColor.AddItem(MapGisColor);
			listBoxGisColor.SelectFirstItem();
			((ListBox)listBoxGisColor).EndUpdate();
		}
	}

	private void ReadIconColorPropertiesFromElement()
	{
		colorBoxLocationColor.SelectedColor = MapElement.LocationColor;
		colorBoxLocationIconColor.SelectedColor = MapElement.IconColor;
	}

	private void ReadGisLineSizePropertiesFromElement()
	{
		if (!listBoxGisLineSize.ItemsEqualTo(MapGisLineSize))
		{
			((ListBox)listBoxGisLineSize).BeginUpdate();
			((ListBox)listBoxGisLineSize).Items.Clear();
			listBoxGisLineSize.AddItem(MapGisLineSize);
			listBoxGisLineSize.SelectFirstItem();
			((ListBox)listBoxGisLineSize).EndUpdate();
		}
	}

	private void ReadGisDescriptionPropertiesFromElement()
	{
		if (!listBoxGisDescription.ItemsEqualTo(MapGisDescription))
		{
			((ListBox)listBoxGisDescription).BeginUpdate();
			((ListBox)listBoxGisDescription).Items.Clear();
			listBoxGisDescription.AddItem(MapGisDescription);
			listBoxGisDescription.SelectFirstItem();
			((ListBox)listBoxGisDescription).EndUpdate();
		}
	}

	private void ReadLocationPropertiesFromElement()
	{
		if (listBoxLocation.ItemsEqualTo(MapLocation) && listBoxLocationValue.ItemsEqualTo(MapLocationValue) && listBoxLocationColorMeter.ItemsEqualTo(MapLocationColorMeter) && listBoxLocationArgument.ItemsEqualTo(MapLocationArgumentMeter))
		{
			return;
		}
		((ListBox)listBoxLocation).BeginUpdate();
		((ListBox)listBoxLocation).Items.Clear();
		listBoxLocation.AddItem(MapLocation);
		listBoxLocation.SelectFirstItem();
		((ListBox)listBoxLocation).EndUpdate();
		((ListBox)listBoxLocationValue).BeginUpdate();
		((ListBox)listBoxLocationValue).Items.Clear();
		listBoxLocationValue.AddItem(MapLocationValue);
		listBoxLocationValue.SelectFirstItem();
		((ListBox)listBoxLocationValue).EndUpdate();
		((ListBox)listBoxLocationColorMeter).BeginUpdate();
		((ListBox)listBoxLocationColorMeter).Items.Clear();
		listBoxLocationColorMeter.AddItem(MapLocationColorMeter);
		listBoxLocationColorMeter.SelectFirstItem();
		((ListBox)listBoxLocationColorMeter).EndUpdate();
		((ListBox)listBoxLocationArgument).BeginUpdate();
		((ListBox)listBoxLocationArgument).Items.Clear();
		listBoxLocationArgument.AddItem(MapLocationArgumentMeter);
		listBoxLocationArgument.SelectFirstItem();
		((ListBox)listBoxLocationArgument).EndUpdate();
		switch (MapElement?.LocationType)
		{
		case StiOnlineMapLocationType.Auto:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationAuto;
			break;
		case StiOnlineMapLocationType.AdminDivision1:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationAdminDivision1;
			break;
		case StiOnlineMapLocationType.AdminDivision2:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationAdminDivision2;
			break;
		case StiOnlineMapLocationType.CountryRegion:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationCountryRegion;
			break;
		case StiOnlineMapLocationType.Neighborhood:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationNeighborhood;
			break;
		case StiOnlineMapLocationType.PopulatedPlace:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationPopulatedPlace;
			break;
		case StiOnlineMapLocationType.Postcode1:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationPostcode1;
			break;
		case StiOnlineMapLocationType.Postcode2:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationPostcode2;
			break;
		case StiOnlineMapLocationType.Postcode3:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationPostcode3;
			break;
		case StiOnlineMapLocationType.Postcode4:
			((ComboBox)comboBoxLocationType).SelectedItem = LocLocationPostcode4;
			break;
		}
		StiOnlineMapElement mapElement = MapElement;
		if (mapElement != null)
		{
			_ = mapElement.Culture;
			if (true)
			{
				((ComboBox)comboBoxCulture).SelectedItem = MapElement.Culture.ToString().Replace("_", "-");
			}
		}
		switch (MapElement?.LocationColorType)
		{
		case StiOnlineMapLocationColorType.Single:
			((ComboBox)comboBoxLocationColorType).SelectedItem = LocLocationColorTypeSingle;
			break;
		case StiOnlineMapLocationColorType.ColorEach:
			((ComboBox)comboBoxLocationColorType).SelectedItem = LocLocationColorTypeColorEach;
			break;
		case StiOnlineMapLocationColorType.Group:
			((ComboBox)comboBoxLocationColorType).SelectedItem = LocLocationColorTypeGroup;
			break;
		case StiOnlineMapLocationColorType.Value:
			((ComboBox)comboBoxLocationColorType).SelectedItem = LocLocationColorTypeValue;
			break;
		}
		switch (MapElement?.HeatmapColorGradientType)
		{
		case StiOnlineMapHeatmapColorGradientType.BlackAquaWhite:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeBlackAquaWhite;
			break;
		case StiOnlineMapHeatmapColorGradientType.BlueRed:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeBlueRed;
			break;
		case StiOnlineMapHeatmapColorGradientType.ColorSpectrum:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeColorSpectrum;
			break;
		case StiOnlineMapHeatmapColorGradientType.Custom:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeCustom;
			break;
		case StiOnlineMapHeatmapColorGradientType.DeepSea:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeDeepSea;
			break;
		case StiOnlineMapHeatmapColorGradientType.HeatedMetal:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeHeatedMetal;
			break;
		case StiOnlineMapHeatmapColorGradientType.Incandescent:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeIncandescent;
			break;
		case StiOnlineMapHeatmapColorGradientType.SteppedColors:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeSteppedColors;
			break;
		case StiOnlineMapHeatmapColorGradientType.Sunrise:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeSunrise;
			break;
		case StiOnlineMapHeatmapColorGradientType.VisibleSpectrum:
			((ComboBox)comboBoxHeatmapColor).SelectedItem = LocGradientTypeVisibleSpectrum;
			break;
		}
		switch (MapElement?.ValueViewMode)
		{
		case StiOnlineMapValueViewMode.Bubble:
			((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeBubble;
			break;
		case StiOnlineMapValueViewMode.Value:
			((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeValue;
			break;
		case StiOnlineMapValueViewMode.Icon:
			((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeIcon;
			break;
		case StiOnlineMapValueViewMode.Chart:
			((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeChart;
			break;
		case StiOnlineMapValueViewMode.Heatmap:
			((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeHeatmap;
			break;
		}
		colorBoxLocationColor.SelectedColor = MapElement.LocationColor;
		colorBoxLocationIconColor.SelectedColor = MapElement.IconColor;
		UpdateLocationControls();
	}

	public override void WritePropertiesToElement()
	{
		WriteLatitudePropertiesToElement();
		WriteLongitudePropertiesToElement();
		WriteLocationPropertiesToElement();
		WriteGisPropertiesToElement();
		RefreshElementView();
	}

	private void WriteLatitudePropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxLatitude.FirstItem();
		MapElement.Latitude = stiMeterItem?.Meter as StiLatitudeMapMeter;
	}

	private void WriteLongitudePropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxLongitude.FirstItem();
		MapElement.Longitude = stiMeterItem?.Meter as StiLongitudeMapMeter;
	}

	private void WriteLocationPropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxLocation.FirstItem();
		MapElement.Location = stiMeterItem?.Meter as StiLocationMapMeter;
		stiMeterItem = listBoxLocationValue.FirstItem();
		MapElement.LocationValue = stiMeterItem?.Meter as StiLocationValueMapMeter;
		stiMeterItem = listBoxLocationColorMeter.FirstItem();
		MapElement.LocationColorMeter = stiMeterItem?.Meter as StiLocationColorMapMeter;
		stiMeterItem = listBoxLocationArgument.FirstItem();
		MapElement.LocationArgument = stiMeterItem?.Meter as StiLocationArgumentMapMeter;
		MapElement.Culture = (StiOnlineMapCulture)Enum.Parse(typeof(StiOnlineMapCulture), ((ComboBox)comboBoxCulture).SelectedItem.ToString().Replace("-", "_"));
		string text = ((ComboBox)comboBoxLocationType).SelectedItem as string;
		if (text == LocLocationAuto)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Auto;
		}
		if (text == LocLocationAdminDivision1)
		{
			MapElement.LocationType = StiOnlineMapLocationType.AdminDivision1;
		}
		else if (text == LocLocationAdminDivision2)
		{
			MapElement.LocationType = StiOnlineMapLocationType.AdminDivision2;
		}
		else if (text == LocLocationCountryRegion)
		{
			MapElement.LocationType = StiOnlineMapLocationType.CountryRegion;
		}
		else if (text == LocLocationNeighborhood)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Neighborhood;
		}
		else if (text == LocLocationPopulatedPlace)
		{
			MapElement.LocationType = StiOnlineMapLocationType.PopulatedPlace;
		}
		else if (text == LocLocationPostcode1)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Postcode1;
		}
		else if (text == LocLocationPostcode2)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Postcode2;
		}
		else if (text == LocLocationPostcode3)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Postcode3;
		}
		else if (text == LocLocationPostcode4)
		{
			MapElement.LocationType = StiOnlineMapLocationType.Postcode4;
		}
		string text2 = ((ComboBox)comboBoxLocationColorType).SelectedItem as string;
		if (text2 == LocLocationColorTypeSingle)
		{
			MapElement.LocationColorType = StiOnlineMapLocationColorType.Single;
		}
		else if (text2 == LocLocationColorTypeColorEach)
		{
			MapElement.LocationColorType = StiOnlineMapLocationColorType.ColorEach;
		}
		else if (text2 == LocLocationColorTypeGroup)
		{
			MapElement.LocationColorType = StiOnlineMapLocationColorType.Group;
		}
		else if (text2 == LocLocationColorTypeValue)
		{
			MapElement.LocationColorType = StiOnlineMapLocationColorType.Value;
		}
		string text3 = ((ComboBox)comboBoxHeatmapColor).SelectedItem as string;
		if (text3 == LocGradientTypeBlackAquaWhite)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.BlackAquaWhite;
		}
		else if (text3 == LocGradientTypeBlueRed)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.BlueRed;
		}
		else if (text3 == LocGradientTypeColorSpectrum.ToString())
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.ColorSpectrum;
		}
		else if (text3 == LocGradientTypeCustom)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.Custom;
		}
		else if (text3 == LocGradientTypeDeepSea)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.DeepSea;
		}
		else if (text3 == LocGradientTypeHeatedMetal)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.HeatedMetal;
		}
		else if (text3 == LocGradientTypeIncandescent)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.Incandescent;
		}
		else if (text3 == LocGradientTypeSteppedColors)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.SteppedColors;
		}
		else if (text3 == LocGradientTypeSunrise)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.Sunrise;
		}
		else if (text3 == LocGradientTypeVisibleSpectrum)
		{
			MapElement.HeatmapColorGradientType = StiOnlineMapHeatmapColorGradientType.VisibleSpectrum;
		}
		MapElement.LocationColor = colorBoxLocationColor.SelectedColor;
		MapElement.IconColor = colorBoxLocationIconColor.SelectedColor;
		string text4 = ((ComboBox)comboBoxViewMode).SelectedItem as string;
		if (text4 == LocLocationViewModeBubble)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Bubble;
		}
		else if (text4 == LocLocationViewModeValue)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Value;
		}
		else if (text4 == LocLocationViewModeIcon)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Icon;
		}
		else if (text4 == LocLocationViewModeChart)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Chart;
		}
		else if (text4 == LocLocationViewModeHeatmap)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Heatmap;
		}
	}

	private void WriteGisPropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxGis.FirstItem();
		MapElement.Gis = stiMeterItem?.Meter as StiGisMapMeter;
		stiMeterItem = listBoxGisColor.FirstItem();
		MapElement.GisColor = stiMeterItem?.Meter as StiGisMapColorMeter;
		stiMeterItem = listBoxGisLineSize.FirstItem();
		MapElement.LineSize = stiMeterItem?.Meter as StiGisMapLineSizeMeter;
		stiMeterItem = listBoxGisDescription.FirstItem();
		MapElement.Description = stiMeterItem?.Meter as StiGisMapDescriptionMeter;
		StiOnlineMapGisItem stiOnlineMapGisItem = ((ComboBox)comboBoxProvider).SelectedItem as StiOnlineMapGisItem;
		MapElement.Provider = stiOnlineMapGisItem.Id;
		MapElement.GisSettings.IconColor = colorBoxGisIconColor.SelectedColor;
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToLatitude();
		AttachEventsToLongitude();
		AttachEventsToGis();
		AttachEventsToGisColor();
		AttachEventsToGisLineSize();
		AttachEventsToGisDescription();
		AttachEventsToLocation();
		AttachEventsToValue();
		AttachEventsToArgument();
		AttachEventsToColorGroup();
	}

	private void AttachEventsToLatitude()
	{
		listBoxLatitude.ElementsConverting += ListBox_ElementsConverting;
		listBoxLatitude.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLatitude.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLatitude.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxLocationValue).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				listBoxLatitude.AddItem(StiMeterHelper.OnlineMap.GetLatitude(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLatitude.SelectedMeterItem = listBoxLatitude.LastItem();
				((Control)listBoxLatitude).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxLocationValue).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				listBoxLatitude.AddItem(StiMeterHelper.OnlineMap.GetLatitude(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLatitude.SelectedMeterItem = listBoxLatitude.LastItem();
				((Control)listBoxLatitude).Focus();
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxLatitude.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToLongitude()
	{
		listBoxLongitude.ElementsConverting += ListBox_ElementsConverting;
		listBoxLongitude.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLongitude.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLongitude.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxLocationValue).Items.Clear();
				((ListBox)listBoxLongitude).Items.Clear();
				listBoxLongitude.AddItem(StiMeterHelper.OnlineMap.GetLongitude(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLongitude.SelectedMeterItem = listBoxLongitude.LastItem();
				((Control)listBoxLongitude).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxLocationValue).Items.Clear();
				((ListBox)listBoxLongitude).Items.Clear();
				listBoxLongitude.AddItem(StiMeterHelper.OnlineMap.GetLongitude(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLongitude.SelectedMeterItem = listBoxLongitude.LastItem();
				((Control)listBoxLongitude).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxLongitude.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToLocation()
	{
		listBoxLocation.ElementsConverting += ListBox_ElementsConverting;
		listBoxLocation.ElementsChanged += delegate
		{
			UpdateLocationControls();
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLocation.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLocation.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				listBoxLocation.AddItem(StiMeterHelper.OnlineMap.GetLocation(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLocation.SelectedMeterItem = listBoxLocation.LastItem();
				((Control)listBoxLocation).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				listBoxLocation.AddItem(StiMeterHelper.OnlineMap.GetLocation(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLocation.SelectedMeterItem = listBoxLocation.LastItem();
				((Control)listBoxLocation).Focus();
			}
			UpdateLocationControls();
			ResetComponentHighlights();
		};
		listBoxLocation.ElementsSelectedChanged += delegate
		{
			UpdateLocationControls();
			MapElement.Select();
			RefreshUiPanel();
		};
		((ComboBox)comboBoxCulture).SelectedIndexChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		((ComboBox)comboBoxViewMode).SelectedIndexChanged += delegate
		{
			if (!base.LockRefresh)
			{
				UpdateLocationControls();
				WritePropertiesToElement();
				RefreshUiPanel();
			}
		};
		((ComboBox)comboBoxLocationType).SelectedIndexChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		((ComboBox)comboBoxHeatmapColor).SelectedIndexChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		((ComboBox)comboBoxLocationColorType).SelectedIndexChanged += delegate
		{
			string text = ((ComboBox)comboBoxLocationColorType).SelectedItem as string;
			if (text == LocLocationColorTypeSingle)
			{
				MapElement.LocationColorType = StiOnlineMapLocationColorType.Single;
			}
			else if (text == LocLocationColorTypeColorEach)
			{
				MapElement.LocationColorType = StiOnlineMapLocationColorType.ColorEach;
			}
			else if (text == LocLocationColorTypeGroup)
			{
				MapElement.LocationColorType = StiOnlineMapLocationColorType.Group;
			}
			else if (text == LocLocationColorTypeValue)
			{
				MapElement.LocationColorType = StiOnlineMapLocationColorType.Value;
			}
			UpdateLocationControls();
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		colorBoxLocationColor.SelectedColorChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		colorBoxLocationIconColor.SelectedColorChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
			BuildIconPanel();
		};
	}

	private void AttachEventsToGis()
	{
		listBoxGis.ElementsConverting += ListBox_ElementsConverting;
		listBoxGis.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxGis.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxGis.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				listBoxGis.AddItem(StiMeterHelper.OnlineMap.GetGis(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxGis.SelectedMeterItem = listBoxGis.LastItem();
				((Control)listBoxGis).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				listBoxGis.AddItem(StiMeterHelper.OnlineMap.GetGis(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxGis.SelectedMeterItem = listBoxGis.LastItem();
				((Control)listBoxGis).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxGis.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
		((ComboBox)comboBoxProvider).SelectedIndexChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		colorBoxGisIconColor.SelectedColorChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
			BuildGisIconPanel();
		};
	}

	private void AttachEventsToGisColor()
	{
		listBoxGisColor.ElementsConverting += ListBox_ElementsConverting;
		listBoxGisColor.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxGisColor.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxGisColor.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				listBoxGisColor.AddItem(StiMeterHelper.OnlineMap.GetGisColor(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxGisColor.SelectedMeterItem = listBoxGisColor.LastItem();
				((Control)listBoxGisColor).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisColor).Items.Clear();
				listBoxGisColor.AddItem(StiMeterHelper.OnlineMap.GetGisColor(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxGisColor.SelectedMeterItem = listBoxGisColor.LastItem();
				((Control)listBoxGisColor).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxGisColor.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToGisLineSize()
	{
		listBoxGisLineSize.ElementsConverting += ListBox_ElementsConverting;
		listBoxGisLineSize.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxGisLineSize.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxGisLineSize.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				listBoxGisLineSize.AddItem(StiMeterHelper.OnlineMap.GetGisLineSize(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxGisLineSize.SelectedMeterItem = listBoxGisLineSize.LastItem();
				((Control)listBoxGisLineSize).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				listBoxGisLineSize.AddItem(StiMeterHelper.OnlineMap.GetGisLineSize(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxGisLineSize.SelectedMeterItem = listBoxGisLineSize.LastItem();
				((Control)listBoxGisLineSize).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxGisLineSize.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToGisDescription()
	{
		listBoxGisDescription.ElementsConverting += ListBox_ElementsConverting;
		listBoxGisDescription.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxGisDescription.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxGisDescription.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				listBoxGisDescription.AddItem(StiMeterHelper.OnlineMap.GetGisDescription(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxGisDescription.SelectedMeterItem = listBoxGisDescription.LastItem();
				((Control)listBoxGisDescription).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLongitude).Items.Clear();
				((ListBox)listBoxLatitude).Items.Clear();
				((ListBox)listBoxLocation).Items.Clear();
				((ListBox)listBoxGis).Items.Clear();
				((ListBox)listBoxGisLineSize).Items.Clear();
				((ListBox)listBoxGisDescription).Items.Clear();
				listBoxGisDescription.AddItem(StiMeterHelper.OnlineMap.GetGisDescription(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxGisDescription.SelectedMeterItem = listBoxGisDescription.LastItem();
				((Control)listBoxGisDescription).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxGisDescription.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void UpdateLocationControls()
	{
		if (MapLocation == null && IsDataFieldsMode)
		{
			return;
		}
		if (((ComboBox)comboBoxViewMode).Items.Count == 2)
		{
			base.LockRefresh = true;
			LocalizeViewMode();
			if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon)
			{
				((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeIcon;
			}
			else if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
			{
				((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeHeatmap;
			}
			base.LockRefresh = false;
		}
		StiPanelUX stiPanelUX = panelViewMode;
		StiPanelUX stiPanelUX2 = panelCustomLocationIcon;
		StiPanelUX stiPanelUX3 = panelType;
		StiPanelUX stiPanelUX4 = panelCulture;
		StiPanelUX stiPanelUX5 = panelLocationColor;
		bool flag2 = (((Control)panelColor).Visible = true);
		bool flag4 = (((Control)stiPanelUX5).Visible = flag2);
		bool flag6 = (((Control)stiPanelUX4).Visible = flag4);
		bool flag8 = (((Control)stiPanelUX3).Visible = flag6);
		bool visible = (((Control)stiPanelUX2).Visible = flag8);
		((Control)stiPanelUX).Visible = visible;
		if (MapElement.LocationColorType == StiOnlineMapLocationColorType.Single)
		{
			((Control)panelLocationColor).Visible = true;
			((Control)labelLocationColorMeter).Visible = false;
			((Control)listBoxLocationColorMeter).Visible = false;
			SetColumnVisibility("Color", visible: false);
		}
		else if (MapElement.LocationColorType == StiOnlineMapLocationColorType.ColorEach)
		{
			((Control)panelLocationColor).Visible = false;
			((Control)labelLocationColorMeter).Visible = false;
			((Control)listBoxLocationColorMeter).Visible = false;
			SetColumnVisibility("Color", visible: false);
		}
		else if (MapElement.LocationColorType == StiOnlineMapLocationColorType.Group || MapElement.LocationColorType == StiOnlineMapLocationColorType.Value)
		{
			((Control)panelLocationColor).Visible = false;
			((Control)labelLocationColorMeter).Visible = IsDataFieldsMode;
			((Control)listBoxLocationColorMeter).Visible = IsDataFieldsMode;
			((Control)labelLocationColorMeter).Text = LocLocationColor;
			SetColumnVisibility("Color", IsMapLocationMode);
			SetColumnHeaderText("Color", LocLocationColor);
		}
		if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
		{
			StiPanelUX stiPanelUX6 = panelColor;
			StiPanelUX stiPanelUX7 = panelLocationColor;
			StiPanelUX stiPanelUX8 = panelCustomLocationIcon;
			flag6 = (((Control)panelIcon).Visible = false);
			flag8 = (((Control)stiPanelUX8).Visible = flag6);
			visible = (((Control)stiPanelUX7).Visible = flag8);
			((Control)stiPanelUX6).Visible = visible;
		}
		SetColumnVisibility("Location", IsMapLocationMode);
		SetColumnVisibility("Value", IsMapLocationMode);
		SetColumnVisibility("Latitude", IsMapCoordinateMode);
		SetColumnVisibility("Longitude", IsMapCoordinateMode);
		((Control)panelMoreOptions).Visible = isShowMore;
		StiChartGroupLabel stiChartGroupLabel = labelLocationArgument;
		visible = (((Control)listBoxLocationArgument).Visible = MapElement.ValueViewMode == StiOnlineMapValueViewMode.Chart);
		((Control)stiChartGroupLabel).Visible = visible;
		((Control)panelViewMode).Visible = IsMapLocationSpecified || IsDataEnterMode;
		((Control)panelIcon).Visible = (IsMapLocationSpecified || IsDataEnterMode) && MapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon && MapElement.CustomIcon == null;
		((Control)panelCustomLocationIcon).Visible = (IsMapLocationSpecified || IsDataEnterMode) && MapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon && MapElement.CustomIcon != null;
		((Control)panelHeatmapColor).Visible = MapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap;
		if (!isShowMore)
		{
			((Control)labelLocationColorMeter).Visible = false;
			((Control)listBoxLocationColorMeter).Visible = false;
			StiChartGroupLabel stiChartGroupLabel2 = labelLocationArgument;
			visible = (((Control)listBoxLocationArgument).Visible = false);
			((Control)stiChartGroupLabel2).Visible = visible;
		}
	}

	private void SetColumnVisibility(string columnName, bool visible)
	{
		DataGridViewColumn val = ((DataGridView)gridData).Columns[columnName];
		if (val != null)
		{
			((DataGridViewBand)val).Visible = visible;
		}
	}

	private void SetColumnHeaderText(string columnName, string headerText)
	{
		DataGridViewColumn val = ((DataGridView)gridData).Columns[columnName];
		if (val != null)
		{
			val.HeaderText = headerText;
		}
	}

	private void AttachEventsToValue()
	{
		listBoxLocationValue.ElementsConverting += ListBox_ElementsConverting;
		listBoxLocationValue.ElementsChanged += delegate
		{
			UpdateLocationControls();
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLocationValue.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLocationValue.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLocationValue).Items.Clear();
				listBoxLocationValue.AddItem(StiMeterHelper.OnlineMap.GetLocationValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLocationValue.SelectedMeterItem = listBoxLocationValue.LastItem();
				((Control)listBoxLocationValue).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLocationValue).Items.Clear();
				listBoxLocationValue.AddItem(StiMeterHelper.OnlineMap.GetLocationValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLocationValue.SelectedMeterItem = listBoxLocationValue.LastItem();
				((Control)listBoxLocationValue).Focus();
			}
			UpdateLocationControls();
			ResetComponentHighlights();
		};
		listBoxLocationValue.ElementsSelectedChanged += delegate
		{
			UpdateLocationControls();
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToArgument()
	{
		listBoxLocationArgument.ElementsConverting += ListBox_ElementsConverting;
		listBoxLocationArgument.ElementsChanged += delegate
		{
			UpdateLocationControls();
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLocationArgument.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLocationArgument.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLocationArgument).Items.Clear();
				listBoxLocationArgument.AddItem(StiMeterHelper.OnlineMap.GetLocationArgument(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLocationArgument.SelectedMeterItem = listBoxLocationArgument.LastItem();
				((Control)listBoxLocationArgument).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLocationArgument).Items.Clear();
				listBoxLocationArgument.AddItem(StiMeterHelper.OnlineMap.GetLocationArgument(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLocationArgument.SelectedMeterItem = listBoxLocationArgument.LastItem();
				((Control)listBoxLocationArgument).Focus();
			}
			UpdateLocationControls();
			ResetComponentHighlights();
		};
		listBoxLocationArgument.ElementsSelectedChanged += delegate
		{
			UpdateLocationControls();
			MapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToColorGroup()
	{
		listBoxLocationColorMeter.ElementsConverting += ListBox_ElementsConverting;
		listBoxLocationColorMeter.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxLocationColorMeter.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxLocationColorMeter.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLocationColorMeter).Items.Clear();
				listBoxLocationColorMeter.AddItem(StiMeterHelper.OnlineMap.GetLocationColorMeter(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				MapElement.Select();
				WritePropertiesToElement();
				listBoxLocationColorMeter.SelectedMeterItem = listBoxLocationColorMeter.LastItem();
				((Control)listBoxLocationColorMeter).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLocationColorMeter).Items.Clear();
				listBoxLocationColorMeter.AddItem(StiMeterHelper.OnlineMap.GetLocationColorMeter(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLocationColorMeter.SelectedMeterItem = listBoxLocationColorMeter.LastItem();
				((Control)listBoxLocationColorMeter).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxLocationColorMeter.ElementsSelectedChanged += delegate
		{
			MapElement.Select();
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
			((Control)panelMoreOptions).Visible = MapLatitude != null || MapLongitude != null || MapLocation != null || IsDataEnterMode;
			((Control)panelUi).Visible = MapLatitude != null || MapLongitude != null || IsMapLocationSpecified || MapGis != null;
			StiChartGroupLabel stiChartGroupLabel = labelLocationValue;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox = listBoxLocationValue;
			StiChartGroupLabel stiChartGroupLabel2 = labelLocationColorMeter;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox2 = listBoxLocationColorMeter;
			StiChartGroupLabel stiChartGroupLabel3 = labelLocationArgument;
			bool flag = (((Control)listBoxLocationArgument).Visible = IsMapLocationSpecified);
			bool flag3 = (((Control)stiChartGroupLabel3).Visible = flag);
			bool flag5 = (((Control)stiMeterListBox2).Visible = flag3);
			bool flag7 = (((Control)stiChartGroupLabel2).Visible = flag5);
			bool visible = (((Control)stiMeterListBox).Visible = flag7);
			((Control)stiChartGroupLabel).Visible = visible;
			((Control)moreOptionsPanel).Visible = ((Control)labelLocationValue).Visible || IsDataEnterMode;
			((Control)panelGisProvider).Visible = MapGis != null;
			((Control)panelOr).Visible = MapGis == null && !((Control)panelUi).Visible && IsDataFieldsMode;
			((Control)panelOrGis).Visible = MapGis == null && !((Control)panelUi).Visible && IsDataFieldsMode;
			StiChartGroupLabel stiChartGroupLabel4 = labelGisDescription;
			StiChartGroupLabel stiChartGroupLabel5 = labelGisColor;
			StiChartGroupLabel stiChartGroupLabel6 = labelGisLineSize;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox3 = listBoxGisColor;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox4 = listBoxGisLineSize;
			flag = (((Control)listBoxGisDescription).Visible = MapGis != null);
			flag3 = (((Control)stiMeterListBox4).Visible = flag);
			flag5 = (((Control)stiMeterListBox3).Visible = flag3);
			flag7 = (((Control)stiChartGroupLabel6).Visible = flag5);
			visible = (((Control)stiChartGroupLabel5).Visible = flag7);
			((Control)stiChartGroupLabel4).Visible = visible;
			((Control)panelGisIcon).Visible = MapGis != null;
			if (MapLatitude != null || MapLongitude != null)
			{
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox5 = listBoxLocation;
				StiLabelPanel stiLabelPanel = labelLocation;
				StiChartGroupLabel stiChartGroupLabel7 = labelGis;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox6 = listBoxGis;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox7 = listBoxGisColor;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox8 = listBoxGisLineSize;
				bool flag15 = (((Control)listBoxGisDescription).Visible = false);
				flag = (((Control)stiMeterListBox8).Visible = flag15);
				flag3 = (((Control)stiMeterListBox7).Visible = flag);
				flag5 = (((Control)stiMeterListBox6).Visible = flag3);
				flag7 = (((Control)stiChartGroupLabel7).Visible = flag5);
				visible = (((Control)stiLabelPanel).Visible = flag7);
				((Control)stiMeterListBox5).Visible = visible;
			}
			else if (IsMapLocationSpecified)
			{
				StiLabelPanel stiLabelPanel2 = labelLatitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox9 = listBoxLatitude;
				StiChartGroupLabel stiChartGroupLabel8 = labelLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox10 = listBoxLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox11 = listBoxGis;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox12 = listBoxGisColor;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox13 = listBoxGisLineSize;
				bool flag22 = (((Control)labelGis).Visible = false);
				bool flag15 = (((Control)stiMeterListBox13).Visible = flag22);
				flag = (((Control)stiMeterListBox12).Visible = flag15);
				flag3 = (((Control)stiMeterListBox11).Visible = flag);
				flag5 = (((Control)stiMeterListBox10).Visible = flag3);
				flag7 = (((Control)stiChartGroupLabel8).Visible = flag5);
				visible = (((Control)stiMeterListBox9).Visible = flag7);
				((Control)stiLabelPanel2).Visible = visible;
				StiLabelPanel stiLabelPanel3 = labelLocation;
				visible = (((Control)listBoxLocation).Visible = IsDataFieldsMode);
				((Control)stiLabelPanel3).Visible = visible;
			}
			else if (MapGis != null)
			{
				StiLabelPanel stiLabelPanel4 = labelLatitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox14 = listBoxLatitude;
				StiChartGroupLabel stiChartGroupLabel9 = labelLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox15 = listBoxLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox16 = listBoxLocation;
				flag = (((Control)labelLocation).Visible = false);
				flag3 = (((Control)stiMeterListBox16).Visible = flag);
				flag5 = (((Control)stiMeterListBox15).Visible = flag3);
				flag7 = (((Control)stiChartGroupLabel9).Visible = flag5);
				visible = (((Control)stiMeterListBox14).Visible = flag7);
				((Control)stiLabelPanel4).Visible = visible;
			}
			else
			{
				StiLabelPanel stiLabelPanel5 = labelLatitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox17 = listBoxLatitude;
				StiChartGroupLabel stiChartGroupLabel10 = labelLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox18 = listBoxLongitude;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox19 = listBoxLocation;
				Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox20 = listBoxGis;
				StiLabelPanel stiLabelPanel6 = labelLocation;
				bool flag22 = (((Control)labelGis).Visible = IsDataFieldsMode);
				bool flag15 = (((Control)stiLabelPanel6).Visible = flag22);
				flag = (((Control)stiMeterListBox20).Visible = flag15);
				flag3 = (((Control)stiMeterListBox19).Visible = flag);
				flag5 = (((Control)stiMeterListBox18).Visible = flag3);
				flag7 = (((Control)stiChartGroupLabel10).Visible = flag5);
				visible = (((Control)stiMeterListBox17).Visible = flag7);
				((Control)stiLabelPanel5).Visible = visible;
			}
			StiLabelPanel stiLabelPanel7 = labelGridData;
			StiExcelGrid stiExcelGrid = gridData;
			flag7 = (((Control)buttonUseDataFields).Visible = IsDataEnterMode);
			visible = (((Control)stiExcelGrid).Visible = flag7);
			((Control)stiLabelPanel7).Visible = visible;
			SetListHeightFromCount(listBoxLatitude, multiple: false);
			SetListHeightFromCount(listBoxLongitude, multiple: false);
			SetListHeightFromCount(listBoxGis, multiple: false);
			SetListHeightFromCount(listBoxGisColor, multiple: false);
			SetListHeightFromCount(listBoxGisLineSize, multiple: false);
			SetListHeightFromCount(listBoxGisDescription, multiple: false);
			SetListHeightFromCount(listBoxLocation, multiple: false);
			SetListHeightFromCount(listBoxLocationValue, multiple: false);
			SetListHeightFromCount(listBoxLocationColorMeter, multiple: false);
			SetListHeightFromCount(listBoxLocationArgument, multiple: false);
			UpdateLocationControls();
			if (MapLatitude != null || MapLongitude != null)
			{
				UpdateLatLonControls();
			}
			CalculateWindowHeight();
			CalculateWindowPosition();
			RefreshSampleIcon();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	private void UpdateLatLonControls()
	{
		StiPanelUX stiPanelUX = panelHeatmapColor;
		StiPanelUX stiPanelUX2 = panelIcon;
		StiPanelUX stiPanelUX3 = panelCustomLocationIcon;
		StiPanelUX stiPanelUX4 = panelCustomLocationIcon;
		StiPanelUX stiPanelUX5 = panelType;
		StiPanelUX stiPanelUX6 = panelCulture;
		StiPanelUX stiPanelUX7 = panelColor;
		bool flag2 = (((Control)panelLocationColor).Visible = false);
		bool flag4 = (((Control)stiPanelUX7).Visible = flag2);
		bool flag6 = (((Control)stiPanelUX6).Visible = flag4);
		bool flag8 = (((Control)stiPanelUX5).Visible = flag6);
		bool flag10 = (((Control)stiPanelUX4).Visible = flag8);
		bool flag12 = (((Control)stiPanelUX3).Visible = flag10);
		bool visible = (((Control)stiPanelUX2).Visible = flag12);
		((Control)stiPanelUX).Visible = visible;
		((Control)panelViewMode).Visible = true;
		if (MapElement.ValueViewMode != StiOnlineMapValueViewMode.Icon && MapElement.ValueViewMode != StiOnlineMapValueViewMode.Heatmap)
		{
			MapElement.ValueViewMode = StiOnlineMapValueViewMode.Icon;
		}
		if (((ComboBox)comboBoxViewMode).Items.Count != 2)
		{
			base.LockRefresh = true;
			LocalizeViewMode();
			if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon)
			{
				((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeIcon;
			}
			else if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
			{
				((ComboBox)comboBoxViewMode).SelectedItem = LocLocationViewModeHeatmap;
			}
			base.LockRefresh = false;
		}
		if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon)
		{
			((Control)panelIcon).Visible = !UseCustomIcon;
			((Control)panelCustomLocationIcon).Visible = UseCustomIcon;
		}
		else if (MapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
		{
			((Control)panelHeatmapColor).Visible = true;
		}
	}

	private void RefreshExpressionHint()
	{
		textBoxExpression.Hint = ((selectedListBox != null) ? Loc.GetMain("Field") : "");
	}

	internal void RefreshExpression()
	{
		bool lockRefresh = base.LockRefresh;
		base.LockRefresh = true;
		if (selectedListBox == listBoxLatitude && MapLatitude != null)
		{
			((Control)textBoxExpression).Text = MapLatitude.Expression;
		}
		else if (selectedListBox == listBoxLongitude && MapLongitude != null)
		{
			((Control)textBoxExpression).Text = MapLongitude.Expression;
		}
		else if (selectedListBox == listBoxLocation && MapLocation != null)
		{
			((Control)textBoxExpression).Text = MapLocation.Expression;
		}
		else if (selectedListBox == listBoxGis && MapGis != null)
		{
			((Control)textBoxExpression).Text = MapGis.Expression;
		}
		else if (selectedListBox == listBoxGisColor && MapGisColor != null)
		{
			((Control)textBoxExpression).Text = MapGisColor.Expression;
		}
		else if (selectedListBox == listBoxGisLineSize && MapGisLineSize != null)
		{
			((Control)textBoxExpression).Text = MapGisLineSize.Expression;
		}
		else if (selectedListBox == listBoxGisDescription && MapGisDescription != null)
		{
			((Control)textBoxExpression).Text = MapGisDescription.Expression;
		}
		else if (selectedListBox == listBoxLocationValue && MapLocationValue != null)
		{
			((Control)textBoxExpression).Text = MapLocationValue.Expression;
		}
		else if (selectedListBox == listBoxLocationColorMeter && MapLocationColorMeter != null)
		{
			((Control)textBoxExpression).Text = MapLocationColorMeter.Expression;
		}
		else if (selectedListBox == listBoxLocationArgument && MapLocationArgumentMeter != null)
		{
			((Control)textBoxExpression).Text = MapLocationArgumentMeter.Expression;
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = lockRefresh;
	}

	protected override void CalculateWindowHeight()
	{
		if (((Control)this).Visible)
		{
			ArrangeControls((Control)(object)panelMoreOptions, (Control[])(object)new Control[8]
			{
				(Control)panelLocationColor,
				(Control)panelColor,
				(Control)panelHeatmapColor,
				(Control)panelCulture,
				(Control)panelType,
				(Control)panelCustomLocationIcon,
				(Control)panelIcon,
				(Control)panelViewMode
			});
			((Control)panelMoreOptions).ClientSize = new Size(((Control)panelMoreOptions).ClientSize.Width, (from Control c in (IEnumerable)((Control)panelMoreOptions).Controls
				where c.Visible
				select c.Height).Sum());
			ArrangeControls((Control)(object)this, (Control[])(object)new Control[27]
			{
				(Control)panelGisIcon,
				(Control)panelGisProvider,
				(Control)moreOptionsPanel,
				(Control)panelMoreOptions,
				(Control)panelUi,
				(Control)listBoxGisLineSize,
				(Control)labelGisLineSize,
				(Control)listBoxGisColor,
				(Control)labelGisColor,
				(Control)listBoxGisDescription,
				(Control)labelGisDescription,
				(Control)listBoxGis,
				(Control)labelGis,
				(Control)panelOrGis,
				(Control)listBoxLongitude,
				(Control)labelLongitude,
				(Control)listBoxLatitude,
				(Control)labelLatitude,
				(Control)panelOr,
				(Control)listBoxLocationArgument,
				(Control)labelLocationArgument,
				(Control)listBoxLocationColorMeter,
				(Control)labelLocationColorMeter,
				(Control)listBoxLocationValue,
				(Control)labelLocationValue,
				(Control)listBoxLocation,
				(Control)labelLocation
			});
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + (from Control c in (IEnumerable)((Control)this).Controls
				where c.Visible
				select c.Height).Sum());
		}
	}

	private void ArrangeControls(Control control, Control[] children)
	{
		int num = 0;
		foreach (Control val in children)
		{
			if (val.Visible)
			{
				control.Controls.SetChildIndex(val, ++num);
			}
		}
	}

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel();
		return val;
	}

	private Panel GetGisIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = Color.White,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearchGis = stiHintTextBox;
		((Control)textBoxSearchGis).TextChanged += TextBoxSearchGis_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearchGis);
		panelIconsGis = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIconsGis);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildGisIconPanel();
		return val;
	}

	private void BuildIconPanel(string filterName = null)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(MapElement.Icon, MapElement.IconColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void BuildGisIconPanel(string filterName = null)
	{
		((Control)panelIconsGis).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(MapElement.GisSettings.Icon, MapElement.GisSettings.IconColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIconsGis).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontGisIconSelectionControl_ValueChanged;
		((Control)panelIconsGis).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void CheckColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = MapElement.IconColor;
			}
		}
	}

	private void CheckGisColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconsGisPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = MapElement.GisSettings.IconColor;
			}
		}
	}

	private void RefreshSampleIcon()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			Color iconColor = MapElement.IconColor;
			SolidBrush val3 = new SolidBrush(iconColor);
			try
			{
				val.FillRectangle(Brushes.White, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
				val.DrawString(StiFontIconsHelper.GetContent(MapElement.Icon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
	}

	private void RefreshSampleGisIcon()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxGisIcon).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			Color iconColor = MapElement.GisSettings.IconColor;
			SolidBrush val3 = new SolidBrush(iconColor);
			try
			{
				val.FillRectangle(Brushes.White, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxGisIcon).Width - StiScale.I4, ((Control)panelBoxGisIcon).Height - StiScale.I6));
				val.DrawString(StiFontIconsHelper.GetContent(MapElement.GisSettings.Icon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxGisIcon).Width, ((Control)panelBoxGisIcon).Height));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panelBoxGisIcon).Width - 1, ((Control)panelBoxGisIcon).Height - 1);
	}

	private void RefreshGridData()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (timerRefreshGrid == null)
		{
			timerRefreshGrid = new Timer();
			timerRefreshGrid.Interval = 100;
			timerRefreshGrid.Tick += delegate
			{
				timerRefreshGrid.Stop();
				SaveManuallyEnteredData();
				RefreshElementView();
				RefreshUiPanel();
			};
		}
		timerRefreshGrid.Stop();
		timerRefreshGrid.Start();
	}

	private Task BuildDataGridAsync()
	{
		return Task.Factory.StartNew(delegate
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			string[] columns = new string[5] { "Location", "Latitude", "Longitude", "Value", "Color" };
			List<string[]> rows = StiStringsTableHelper.LoadStringRowsFromPackedString(MapElement.ManuallyEnteredData, columns);
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				((DataGridView)gridData).AutoGenerateColumns = false;
				((DataGridView)gridData).DataSource = StiStringsTableHelper.LoadDataTableFromStringRows(rows, columns);
				gridData.AddNewEmptyRowsUpTo(10);
				DataGridViewColumn val = gridData.AddTextBoxColumn("Location", Loc.GetMain("Location"), 0, (DataGridViewContentAlignment)16);
				((DataGridViewBand)val).Visible = IsMapLocationMode;
				val = gridData.AddTextBoxColumn("Latitude", Loc.GetMain("Latitude"), 0, (DataGridViewContentAlignment)32);
				((DataGridViewBand)val).Visible = IsMapCoordinateMode;
				val = gridData.AddTextBoxColumn("Longitude", Loc.GetMain("Longitude"), 0, (DataGridViewContentAlignment)32);
				((DataGridViewBand)val).Visible = IsMapCoordinateMode;
				val = gridData.AddTextBoxColumn("Value", Loc.GetMain("Value"), 0, (DataGridViewContentAlignment)64);
				((DataGridViewBand)val).Visible = IsMapLocationMode;
				val = gridData.AddTextBoxColumn("Color", LocLocationColor, 0, (DataGridViewContentAlignment)16);
				((DataGridViewBand)val).Visible = IsMapLocationMode && (MapElement.LocationColorType == StiOnlineMapLocationColorType.Group || MapElement.LocationColorType == StiOnlineMapLocationColorType.Value);
				if (IsDataEnterMode)
				{
					((Control)gridData).Focus();
					UpdateLocationControls();
				}
			});
		});
	}

	private void SaveManuallyEnteredData()
	{
		MapElement.ManuallyEnteredData = StiStringsTableHelper.SaveDataTableToPackedString(gridData.DataTable);
	}

	private void GridData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex != -1)
		{
			RefreshGridData();
		}
	}

	private void GridData_CellRowsChanged(object sender, EventArgs e)
	{
		RefreshGridData();
	}

	private void ButtonEnterLocationDataManually_Click(object sender, EventArgs e)
	{
		MapElement.DataMode = StiDataMode.ManuallyEnteringData;
		MapElement.MapMode = StiOnlineMapMode.Location;
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonEnterCoordinateDataManually_Click(object sender, EventArgs e)
	{
		MapElement.DataMode = StiDataMode.ManuallyEnteringData;
		MapElement.MapMode = StiOnlineMapMode.Coordinate;
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonUseDataFields_Click(object sender, EventArgs e)
	{
		isShowMore = true;
		MapElement.DataMode = StiDataMode.UsingDataFields;
		RefreshElementView();
		RefreshUiPanel();
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void TextBoxSearchGis_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildGisIconPanel(filterName);
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIcon();
	}

	private void PanelBoxGisIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleGisIcon();
	}

	private void ButtonAddRange_Click(object sender, EventArgs e)
	{
		isShowMore = !isShowMore;
		((Control)buttonShowMore).Text = (isShowMore ? Loc.Get("Buttons", "LessOptions") : Loc.Get("Buttons", "MoreOptions"));
		UpdateLocationControls();
		RefreshUiPanel();
	}

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		CheckColorIcons();
		((ToolStripDropDown)iconsPopup).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void ButtonGisIcon_Click(object sender, EventArgs e)
	{
		CheckGisColorIcons();
		((ToolStripDropDown)iconsGisPopup).Show(new Point(((Control)panelBoxGisIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonGisIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonGisIcon).Height + StiScale.I2));
		((Control)textBoxSearchGis).Focus();
	}

	private void ButtonCustomIcon_Click(object sender, EventArgs e)
	{
		((Control)iconsPopup).Hide();
		ButtonOpen_Click(sender, e);
	}

	private void ButtonCustomizeColor_Click(object sender, EventArgs e)
	{
		ShowHeatmapGradientForm();
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		MapElement.Icon = ((StiFontIconSelectionControl)sender).SelectedIcon.GetValueOrDefault();
		RefreshElementView();
		RefreshSampleIcon();
		((Control)iconsPopup).Hide();
	}

	private void FontGisIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		MapElement.GisSettings.Icon = ((StiFontIconSelectionControl)sender).SelectedIcon.GetValueOrDefault();
		RefreshElementView();
		RefreshSampleGisIcon();
		((Control)iconsGisPopup).Hide();
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		if (!(e.Data.GetData(DataFormats.FileDrop) is string[] source))
		{
			return;
		}
		foreach (string item in source.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					MapElement.CustomIcon = StiImageHelper.TryBytesFromFile(item);
					RefreshImageAndStatus();
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		if (e.Data.GetData(DataFormats.FileDrop) is string[] source)
		{
			foreach (string item in source.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelCustomIcon, e.Graphics, MapElement.CustomIcon, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		MapElement.CustomIcon = null;
		RefreshUiPanel();
		RefreshImageAndStatus();
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					MapElement.CustomIcon = StiImageHelper.TryBytesFromFile(((FileDialog)val).FileName);
					RefreshUiPanel();
					RefreshImageAndStatus();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterItem;
		if (stiMeterItem.List == listBoxLatitude)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLatitude(stiMeterItem.Meter);
			((ListBox)listBoxLocation).Items.Clear();
		}
		if (stiMeterItem.List == listBoxLongitude)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLongitude(stiMeterItem.Meter);
			((ListBox)listBoxLocation).Items.Clear();
		}
		if (stiMeterItem.List == listBoxLocation)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLocation(stiMeterItem.Meter);
			((ListBox)listBoxLongitude).Items.Clear();
			((ListBox)listBoxLatitude).Items.Clear();
		}
		if (stiMeterItem.List == listBoxGis)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetGis(stiMeterItem.Meter);
			((ListBox)listBoxGisColor).Items.Clear();
		}
		if (stiMeterItem.List == listBoxGisColor)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetGisColor(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxGisLineSize)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetGisLineSize(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxGisDescription)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetGisDescription(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxLocationValue)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLocationValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxLocationColorMeter)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLocationColorMeter(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxLocationArgument)
		{
			stiMeterItem.Meter = StiMeterHelper.OnlineMap.GetLocationArgument(stiMeterItem.Meter);
		}
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxLatitude.SelectedMeterItem = listBoxLatitude.FirstItem();
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_maps_online_map.htm");
	}

	private async void This_LoadAsync(object sender, EventArgs e)
	{
		await BuildDataGridAsync();
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		iconsGisPopup = new StiPopup((Control)(object)GetGisIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
		((Control)panelBoxGisIcon).Paint += new PaintEventHandler(PanelBoxGisIcon_Paint);
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		base.OnFormClosed(e);
		if (MapElement.Location == null)
		{
			MapElement.LocationColor = Color.LimeGreen;
			MapElement.LocationColorType = StiOnlineMapLocationColorType.Single;
			MapElement.LocationType = StiOnlineMapLocationType.Auto;
			MapElement.Culture = StiOnlineMapCulture.en_US;
			MapElement.LocationColorMeter = null;
			MapElement.LocationValue = null;
			MapElement.LocationArgument = null;
			if (MapElement.Latitude == null && MapElement.Longitude == null)
			{
				MapElement.Icon = StiFontIcons.MapMarker;
				MapElement.IconColor = Color.Maroon;
				MapElement.CustomIcon = null;
				MapElement.ValueViewMode = StiOnlineMapValueViewMode.Bubble;
			}
		}
		else
		{
			if (MapElement.LocationColorType != 0)
			{
				MapElement.LocationColor = Color.LimeGreen;
			}
			if (MapElement.LocationColorType != StiOnlineMapLocationColorType.Group && MapElement.LocationColorType != StiOnlineMapLocationColorType.Value)
			{
				MapElement.LocationColorMeter = null;
			}
			if (MapElement.ValueViewMode != StiOnlineMapValueViewMode.Icon)
			{
				MapElement.Icon = StiFontIcons.MapMarker;
				MapElement.IconColor = Color.Maroon;
				MapElement.CustomIcon = null;
			}
			if (MapElement.ValueViewMode != StiOnlineMapValueViewMode.Chart)
			{
				MapElement.LocationArgument = null;
			}
		}
		SaveManuallyEnteredData();
		StiStringsTableHelper.RemoveFromCache(base.Element);
		((DataGridView)gridData).CellValueChanged -= new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged -= GridData_CellRowsChanged;
		((Form)this).Load -= This_LoadAsync;
		((Component)(object)timerRefreshGrid)?.Dispose();
		timerRefreshGrid = null;
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			RefreshElementView();
		}
	}

	private void TextBoxExpression_Leave(object sender, EventArgs e)
	{
		RefreshElementView();
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		if (selectedListBox == listBoxLatitude && MapLatitude != null)
		{
			MapLatitude.Expression = ((Control)textBoxExpression).Text;
			listBoxLatitude.SetSelectedText(StiLabelHelper.GetLabel(MapLatitude));
		}
		if (selectedListBox == listBoxLongitude && MapLongitude != null)
		{
			MapLongitude.Expression = ((Control)textBoxExpression).Text;
			listBoxLongitude.SetSelectedText(StiLabelHelper.GetLabel(MapLongitude));
		}
		if (selectedListBox == listBoxLocation && MapLocation != null)
		{
			MapLocation.Expression = ((Control)textBoxExpression).Text;
			listBoxLocation.SetSelectedText(StiLabelHelper.GetLabel(MapLocation));
		}
		if (selectedListBox == listBoxGis && MapGis != null)
		{
			MapGis.Expression = ((Control)textBoxExpression).Text;
			listBoxGis.SetSelectedText(StiLabelHelper.GetLabel(MapGis));
		}
		if (selectedListBox == listBoxGisColor && MapGisColor != null)
		{
			MapGisColor.Expression = ((Control)textBoxExpression).Text;
			listBoxGisColor.SetSelectedText(StiLabelHelper.GetLabel(MapGisColor));
		}
		if (selectedListBox == listBoxGisLineSize && MapGisLineSize != null)
		{
			MapGisLineSize.Expression = ((Control)textBoxExpression).Text;
			listBoxGisLineSize.SetSelectedText(StiLabelHelper.GetLabel(MapGisLineSize));
		}
		if (selectedListBox == listBoxGisDescription && MapGisDescription != null)
		{
			MapGisDescription.Expression = ((Control)textBoxExpression).Text;
			listBoxGisDescription.SetSelectedText(StiLabelHelper.GetLabel(MapGisDescription));
		}
		if (selectedListBox == listBoxLocationValue && MapLocationValue != null)
		{
			MapLocationValue.Expression = ((Control)textBoxExpression).Text;
			listBoxLocationValue.SetSelectedText(StiLabelHelper.GetLabel(MapLocationValue));
		}
		if (selectedListBox == listBoxLocationColorMeter && MapLocationColorMeter != null)
		{
			MapLocationColorMeter.Expression = ((Control)textBoxExpression).Text;
			listBoxLocationColorMeter.SetSelectedText(StiLabelHelper.GetLabel(MapLocationColorMeter));
		}
		if (selectedListBox == listBoxLocationArgument && MapLocationArgumentMeter != null)
		{
			MapLocationArgumentMeter.Expression = ((Control)textBoxExpression).Text;
			listBoxLocationArgument.SetSelectedText(StiLabelHelper.GetLabel(MapLocationArgumentMeter));
		}
		RefreshExpressionHint();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiOnlineMapExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Expected O, but got Unknown
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Expected O, but got Unknown
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fb: Expected O, but got Unknown
		//IL_1108: Unknown result type (might be due to invalid IL or missing references)
		//IL_1112: Expected O, but got Unknown
		//IL_111f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Expected O, but got Unknown
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1168: Expected O, but got Unknown
		//IL_1188: Unknown result type (might be due to invalid IL or missing references)
		//IL_120f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1219: Expected O, but got Unknown
		//IL_123a: Unknown result type (might be due to invalid IL or missing references)
		//IL_130e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1385: Unknown result type (might be due to invalid IL or missing references)
		//IL_1422: Unknown result type (might be due to invalid IL or missing references)
		//IL_1491: Unknown result type (might be due to invalid IL or missing references)
		//IL_149b: Expected O, but got Unknown
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_156b: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1708: Unknown result type (might be due to invalid IL or missing references)
		//IL_1762: Unknown result type (might be due to invalid IL or missing references)
		//IL_1829: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1927: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be9: Expected O, but got Unknown
		//IL_1c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d13: Expected O, but got Unknown
		//IL_1d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eff: Expected O, but got Unknown
		//IL_1f2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2040: Unknown result type (might be due to invalid IL or missing references)
		//IL_204a: Expected O, but got Unknown
		//IL_208d: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2119: Unknown result type (might be due to invalid IL or missing references)
		//IL_2123: Expected O, but got Unknown
		//IL_2143: Unknown result type (might be due to invalid IL or missing references)
		//IL_231b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2393: Unknown result type (might be due to invalid IL or missing references)
		//IL_239d: Expected O, but got Unknown
		//IL_23ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2464: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2509: Unknown result type (might be due to invalid IL or missing references)
		//IL_257f: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2601: Expected O, but got Unknown
		//IL_262e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2652: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_273d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2747: Expected O, but got Unknown
		//IL_2787: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2813: Unknown result type (might be due to invalid IL or missing references)
		//IL_281d: Expected O, but got Unknown
		//IL_283d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd5: Expected O, but got Unknown
		//IL_2d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d91: Expected O, but got Unknown
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4d: Expected O, but got Unknown
		//IL_2e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f09: Expected O, but got Unknown
		//IL_2fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_3028: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fd: Expected O, but got Unknown
		//IL_318c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3196: Expected O, but got Unknown
		//IL_3286: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3309: Expected O, but got Unknown
		//IL_3329: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e2: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiOnlineMapElementEditorForm));
		toolTip = new ToolTip(components);
		moreOptionsPanel = new StiPanelUX();
		buttonShowMore = new StiButtonUX();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		panelMoreOptions = new StiPanelUX();
		panelLocationColor = new StiPanelUX();
		colorBoxLocationColor = new StiColorBoxXF();
		panelColor = new StiPanelUX();
		comboBoxLocationColorType = new StiComboBox();
		labelLocationColor = new StiLabelUX();
		panelHeatmapColor = new StiPanelUX();
		buttonCustomizeColor = new StiOperationButton();
		comboBoxHeatmapColor = new StiComboBox();
		labelHeatmapColor = new StiLabelUX();
		panelCulture = new StiPanelUX();
		comboBoxCulture = new StiComboBox();
		labelCulture = new StiLabelUX();
		panelType = new StiPanelUX();
		comboBoxLocationType = new StiComboBox();
		labelLocationType = new StiLabelUX();
		panelCustomLocationIcon = new StiPanelUX();
		panelCustomIcon = new StiPanelUX();
		buttonOpen = new StiOperationButton();
		buttonRemove = new StiOperationButton();
		panelIcon = new StiPanelUX();
		colorBoxLocationIconColor = new StiColorBoxXF();
		panelBoxIcon = new StiPanelUX();
		buttonCustomIcon = new StiOperationButton();
		buttonIcon = new StiOperationButton();
		labelIcon = new StiLabelUX();
		panelViewMode = new StiPanelUX();
		comboBoxViewMode = new StiComboBox();
		labelViewMode = new StiLabelUX();
		panelGisIcon = new StiPanelUX();
		colorBoxGisIconColor = new StiColorBoxXF();
		panelBoxGisIcon = new StiPanelUX();
		buttonGisIcon = new StiOperationButton();
		labelGisIcon = new StiLabelUX();
		panelUi = new StiPanelUX();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new StiLabelUX();
		listBoxLongitude = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLongitude = new StiChartGroupLabel();
		listBoxLatitude = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLatitude = new StiLabelPanel();
		buttonEnterCoordinateDataManually = new StiOperationButton();
		panelOr = new StiPanelUX();
		labelOr = new StiLabelUX();
		panelLine = new StiPanelUX();
		listBoxLocationColorMeter = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLocationColorMeter = new StiChartGroupLabel();
		listBoxLocationArgument = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLocationArgument = new StiChartGroupLabel();
		listBoxLocationValue = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLocationValue = new StiChartGroupLabel();
		listBoxLocation = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLocation = new StiLabelPanel();
		buttonEnterLocationDataManually = new StiOperationButton();
		panelOrGis = new StiPanelUX();
		labelOr2 = new StiLabelUX();
		panel2 = new StiPanelUX();
		labelGis = new StiChartGroupLabel();
		labelGisColor = new StiChartGroupLabel();
		labelGisLineSize = new StiChartGroupLabel();
		labelGisDescription = new StiChartGroupLabel();
		listBoxGis = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		listBoxGisColor = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		listBoxGisLineSize = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		listBoxGisDescription = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		panelGisProvider = new StiPanelUX();
		comboBoxProvider = new StiComboBox();
		labelProvider = new StiLabelUX();
		gridData = new StiExcelGrid();
		labelGridData = new StiLabelPanel();
		buttonUseDataFields = new StiOperationButton();
		((Control)moreOptionsPanel).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelMoreOptions).SuspendLayout();
		((Control)panelLocationColor).SuspendLayout();
		((Control)panelColor).SuspendLayout();
		((Control)panelHeatmapColor).SuspendLayout();
		((Control)panelCulture).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)panelCustomLocationIcon).SuspendLayout();
		((Control)panelCustomIcon).SuspendLayout();
		((Control)panelIcon).SuspendLayout();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)panelViewMode).SuspendLayout();
		((Control)panelGisIcon).SuspendLayout();
		((Control)panelBoxGisIcon).SuspendLayout();
		((Control)panelUi).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)labelLatitude).SuspendLayout();
		((Control)panelOr).SuspendLayout();
		((Control)labelLocation).SuspendLayout();
		((Control)panelOrGis).SuspendLayout();
		((Control)panelGisProvider).SuspendLayout();
		((ISupportInitialize)gridData).BeginInit();
		((Control)labelGridData).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)moreOptionsPanel).Controls.Add((Control)(object)buttonShowMore);
		((Control)moreOptionsPanel).Dock = (DockStyle)1;
		((Control)moreOptionsPanel).Location = new Point(12, 777);
		((Control)moreOptionsPanel).Margin = new Padding(1, 1, 1, 1);
		((Control)moreOptionsPanel).Name = "moreOptionsPanel";
		((Control)moreOptionsPanel).Size = new Size(265, 28);
		((Control)moreOptionsPanel).TabIndex = 28;
		((Control)buttonShowMore).Anchor = (AnchorStyles)13;
		((ButtonBase)buttonShowMore).FlatAppearance.BorderSize = 0;
		((Control)buttonShowMore).Location = new Point(148, 5);
		((Control)buttonShowMore).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonShowMore).Name = "buttonShowMore";
		((Control)buttonShowMore).Size = new Size(118, 22);
		((Control)buttonShowMore).TabIndex = 30;
		((Control)buttonShowMore).Text = "More options";
		((ButtonBase)buttonShowMore).UseVisualStyleBackColor = true;
		((Control)buttonShowMore).Click += ButtonAddRange_Click;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(195, 304);
		((Control)contextMenuBar).Margin = new Padding(2, 2, 2, 2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(80, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 12;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelLocationColor);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelColor);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelHeatmapColor);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelCulture);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelType);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelCustomLocationIcon);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelIcon);
		((Control)panelMoreOptions).Controls.Add((Control)(object)panelViewMode);
		((Control)panelMoreOptions).Dock = (DockStyle)1;
		((Control)panelMoreOptions).Location = new Point(12, 473);
		((Control)panelMoreOptions).Margin = new Padding(2, 2, 2, 2);
		((Control)panelMoreOptions).Name = "panelMoreOptions";
		((Control)panelMoreOptions).Size = new Size(265, 304);
		((Control)panelMoreOptions).TabIndex = 14;
		((Control)panelMoreOptions).Visible = false;
		((Control)panelLocationColor).Controls.Add((Control)(object)colorBoxLocationColor);
		((Control)panelLocationColor).Dock = (DockStyle)1;
		((Control)panelLocationColor).Location = new Point(0, 266);
		((Control)panelLocationColor).Name = "panelLocationColor";
		((Control)panelLocationColor).Size = new Size(265, 34);
		((Control)panelLocationColor).TabIndex = 6;
		((Control)colorBoxLocationColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxLocationColor).Anchor = (AnchorStyles)13;
		((Control)colorBoxLocationColor).Location = new Point(89, 6);
		((Control)colorBoxLocationColor).Name = "colorBoxLocationColor";
		((Control)colorBoxLocationColor).Size = new Size(134, 22);
		((Control)colorBoxLocationColor).TabIndex = 26;
		colorBoxLocationColor.ShowColorName = false;
		((Control)panelColor).Controls.Add((Control)(object)comboBoxLocationColorType);
		((Control)panelColor).Controls.Add((Control)(object)labelLocationColor);
		((Control)panelColor).Dock = (DockStyle)1;
		((Control)panelColor).Location = new Point(0, 233);
		((Control)panelColor).Name = "panelColor";
		((Control)panelColor).Size = new Size(265, 33);
		((Control)panelColor).TabIndex = 5;
		((Control)comboBoxLocationColorType).Anchor = (AnchorStyles)13;
		comboBoxLocationColorType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxLocationColorType).FormattingEnabled = true;
		((Control)comboBoxLocationColorType).Location = new Point(89, 6);
		((Control)comboBoxLocationColorType).Margin = new Padding(2, 2, 2, 2);
		((Control)comboBoxLocationColorType).Name = "comboBoxLocationColorType";
		((Control)comboBoxLocationColorType).Size = new Size(176, 21);
		((Control)comboBoxLocationColorType).TabIndex = 25;
		((Control)labelLocationColor).Location = new Point(0, 6);
		((Control)labelLocationColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLocationColor).Name = "labelLocationColor";
		((Control)labelLocationColor).Size = new Size(83, 22);
		((Control)labelLocationColor).TabIndex = 19;
		((Control)labelLocationColor).Text = "Color";
		((Label)labelLocationColor).TextAlign = (ContentAlignment)16;
		((Control)panelHeatmapColor).Controls.Add((Control)(object)buttonCustomizeColor);
		((Control)panelHeatmapColor).Controls.Add((Control)(object)comboBoxHeatmapColor);
		((Control)panelHeatmapColor).Controls.Add((Control)(object)labelHeatmapColor);
		((Control)panelHeatmapColor).Dock = (DockStyle)1;
		((Control)panelHeatmapColor).Location = new Point(0, 200);
		((Control)panelHeatmapColor).Name = "panelHeatmapColor";
		((Control)panelHeatmapColor).Size = new Size(265, 33);
		((Control)panelHeatmapColor).TabIndex = 29;
		((Control)buttonCustomizeColor).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonCustomizeColor).Image = (Image)componentResourceManager.GetObject("buttonCustomizeColor.Image");
		((Control)buttonCustomizeColor).Location = new Point(243, 5);
		((Control)buttonCustomizeColor).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonCustomizeColor).Name = "buttonCustomizeColor";
		((Control)buttonCustomizeColor).Size = new Size(23, 23);
		((Control)buttonCustomizeColor).TabIndex = 32;
		((Control)buttonCustomizeColor).TabStop = false;
		((ButtonBase)buttonCustomizeColor).UseVisualStyleBackColor = true;
		((Control)buttonCustomizeColor).Click += ButtonCustomizeColor_Click;
		((Control)comboBoxHeatmapColor).Anchor = (AnchorStyles)13;
		comboBoxHeatmapColor.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxHeatmapColor).FormattingEnabled = true;
		((Control)comboBoxHeatmapColor).Location = new Point(89, 6);
		((Control)comboBoxHeatmapColor).Margin = new Padding(2, 2, 2, 2);
		((ComboBox)comboBoxHeatmapColor).MaxDropDownItems = 25;
		((Control)comboBoxHeatmapColor).Name = "comboBoxHeatmapColor";
		((Control)comboBoxHeatmapColor).Size = new Size(153, 21);
		((Control)comboBoxHeatmapColor).TabIndex = 25;
		((Control)labelHeatmapColor).Location = new Point(0, 6);
		((Control)labelHeatmapColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelHeatmapColor).Name = "labelHeatmapColor";
		((Control)labelHeatmapColor).Size = new Size(83, 22);
		((Control)labelHeatmapColor).TabIndex = 19;
		((Control)labelHeatmapColor).Text = "Color";
		((Label)labelHeatmapColor).TextAlign = (ContentAlignment)16;
		((Control)panelCulture).Controls.Add((Control)(object)comboBoxCulture);
		((Control)panelCulture).Controls.Add((Control)(object)labelCulture);
		((Control)panelCulture).Dock = (DockStyle)1;
		((Control)panelCulture).Location = new Point(0, 167);
		((Control)panelCulture).Name = "panelCulture";
		((Control)panelCulture).Size = new Size(265, 33);
		((Control)panelCulture).TabIndex = 4;
		((Control)comboBoxCulture).Anchor = (AnchorStyles)13;
		comboBoxCulture.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCulture).FormattingEnabled = true;
		((Control)comboBoxCulture).Location = new Point(89, 6);
		((Control)comboBoxCulture).Margin = new Padding(2, 2, 2, 2);
		((Control)comboBoxCulture).Name = "comboBoxCulture";
		((Control)comboBoxCulture).Size = new Size(176, 21);
		((ComboBox)comboBoxCulture).Sorted = true;
		((Control)comboBoxCulture).TabIndex = 23;
		((Control)labelCulture).Location = new Point(0, 6);
		((Control)labelCulture).Margin = new Padding(2, 0, 2, 0);
		((Control)labelCulture).Name = "labelCulture";
		((Control)labelCulture).Size = new Size(83, 22);
		((Control)labelCulture).TabIndex = 20;
		((Control)labelCulture).Text = "Culture";
		((Label)labelCulture).TextAlign = (ContentAlignment)16;
		((Control)panelType).Controls.Add((Control)(object)comboBoxLocationType);
		((Control)panelType).Controls.Add((Control)(object)labelLocationType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(0, 134);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(265, 33);
		((Control)panelType).TabIndex = 3;
		((Control)comboBoxLocationType).Anchor = (AnchorStyles)13;
		comboBoxLocationType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxLocationType).FormattingEnabled = true;
		((Control)comboBoxLocationType).Location = new Point(89, 6);
		((Control)comboBoxLocationType).Margin = new Padding(2, 2, 2, 2);
		((ComboBox)comboBoxLocationType).MaxDropDownItems = 25;
		((Control)comboBoxLocationType).Name = "comboBoxLocationType";
		((Control)comboBoxLocationType).Size = new Size(176, 21);
		((Control)comboBoxLocationType).TabIndex = 21;
		((Control)labelLocationType).Location = new Point(0, 6);
		((Control)labelLocationType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLocationType).Name = "labelLocationType";
		((Control)labelLocationType).Size = new Size(83, 22);
		((Control)labelLocationType).TabIndex = 18;
		((Control)labelLocationType).Text = "Type";
		((Label)labelLocationType).TextAlign = (ContentAlignment)16;
		((Control)panelCustomLocationIcon).Controls.Add((Control)(object)panelCustomIcon);
		((Control)panelCustomLocationIcon).Dock = (DockStyle)1;
		((Control)panelCustomLocationIcon).Location = new Point(0, 70);
		((Control)panelCustomLocationIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)panelCustomLocationIcon).Name = "panelCustomLocationIcon";
		((Control)panelCustomLocationIcon).Size = new Size(265, 64);
		((Control)panelCustomLocationIcon).TabIndex = 2;
		((Control)panelCustomLocationIcon).Visible = false;
		((Control)panelCustomIcon).AllowDrop = true;
		((Control)panelCustomIcon).Anchor = (AnchorStyles)15;
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonOpen);
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonRemove);
		((Control)panelCustomIcon).Location = new Point(0, 5);
		((Control)panelCustomIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)panelCustomIcon).Name = "panelCustomIcon";
		((Control)panelCustomIcon).Size = new Size(265, 58);
		((Control)panelCustomIcon).TabIndex = 19;
		((Control)panelCustomIcon).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelCustomIcon).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelCustomIcon).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelCustomIcon).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(235, 5);
		((Control)buttonOpen).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(24, 23);
		((Control)buttonOpen).TabIndex = 0;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(235, 30);
		((Control)buttonRemove).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 23);
		((Control)buttonRemove).TabIndex = 1;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)panelIcon).Controls.Add((Control)(object)colorBoxLocationIconColor);
		((Control)panelIcon).Controls.Add((Control)(object)panelBoxIcon);
		((Control)panelIcon).Controls.Add((Control)(object)labelIcon);
		((Control)panelIcon).Dock = (DockStyle)1;
		((Control)panelIcon).Location = new Point(0, 33);
		((Control)panelIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)panelIcon).Name = "panelIcon";
		((Control)panelIcon).Size = new Size(265, 37);
		((Control)panelIcon).TabIndex = 1;
		((Control)colorBoxLocationIconColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxLocationIconColor).Anchor = (AnchorStyles)9;
		((Control)colorBoxLocationIconColor).Location = new Point(175, 6);
		((Control)colorBoxLocationIconColor).Margin = new Padding(2, 2, 2, 2);
		((Control)colorBoxLocationIconColor).Name = "colorBoxLocationIconColor";
		((Control)colorBoxLocationIconColor).Size = new Size(89, 25);
		((Control)colorBoxLocationIconColor).TabIndex = 18;
		colorBoxLocationIconColor.ShowColorName = false;
		((Control)panelBoxIcon).Anchor = (AnchorStyles)9;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonCustomIcon);
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(103, 6);
		((Control)panelBoxIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Size = new Size(74, 25);
		((Control)panelBoxIcon).TabIndex = 17;
		((Control)panelBoxIcon).Click += ButtonIcon_Click;
		((Control)buttonCustomIcon).Anchor = (AnchorStyles)9;
		((Control)buttonCustomIcon).Font = new Font("Microsoft Sans Serif", 6.5f);
		((Control)buttonCustomIcon).Location = new Point(28, 1);
		((Control)buttonCustomIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonCustomIcon).Name = "buttonCustomIcon";
		((Control)buttonCustomIcon).Size = new Size(23, 23);
		((Control)buttonCustomIcon).TabIndex = 4;
		((Control)buttonCustomIcon).TabStop = false;
		((Control)buttonCustomIcon).Tag = ".";
		((Control)buttonCustomIcon).Text = "...";
		((ButtonBase)buttonCustomIcon).UseVisualStyleBackColor = true;
		((Control)buttonCustomIcon).Click += ButtonCustomIcon_Click;
		((Control)buttonIcon).Anchor = (AnchorStyles)9;
		((Control)buttonIcon).Location = new Point(50, 1);
		((Control)buttonIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(23, 23);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)labelIcon).Location = new Point(0, 9);
		((Control)labelIcon).Margin = new Padding(1, 0, 1, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(84, 20);
		((Control)labelIcon).TabIndex = 20;
		((Control)labelIcon).Text = "Icon";
		((Label)labelIcon).TextAlign = (ContentAlignment)16;
		((Control)panelViewMode).Controls.Add((Control)(object)comboBoxViewMode);
		((Control)panelViewMode).Controls.Add((Control)(object)labelViewMode);
		((Control)panelViewMode).Dock = (DockStyle)1;
		((Control)panelViewMode).Location = new Point(0, 0);
		((Control)panelViewMode).Name = "panelViewMode";
		((Control)panelViewMode).Size = new Size(265, 33);
		((Control)panelViewMode).TabIndex = 0;
		((Control)comboBoxViewMode).Anchor = (AnchorStyles)13;
		comboBoxViewMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxViewMode).FormattingEnabled = true;
		((Control)comboBoxViewMode).Location = new Point(89, 6);
		((Control)comboBoxViewMode).Margin = new Padding(2, 2, 2, 2);
		((Control)comboBoxViewMode).Name = "comboBoxViewMode";
		((Control)comboBoxViewMode).Size = new Size(176, 21);
		((Control)comboBoxViewMode).TabIndex = 15;
		((Control)labelViewMode).Location = new Point(0, 6);
		((Control)labelViewMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelViewMode).Name = "labelViewMode";
		((Control)labelViewMode).Size = new Size(83, 22);
		((Control)labelViewMode).TabIndex = 20;
		((Control)labelViewMode).Text = "View Mode";
		((Label)labelViewMode).TextAlign = (ContentAlignment)16;
		((Control)panelGisIcon).Controls.Add((Control)(object)colorBoxGisIconColor);
		((Control)panelGisIcon).Controls.Add((Control)(object)panelBoxGisIcon);
		((Control)panelGisIcon).Controls.Add((Control)(object)labelGisIcon);
		((Control)panelGisIcon).Dock = (DockStyle)1;
		((Control)panelGisIcon).Location = new Point(12, 833);
		((Control)panelGisIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)panelGisIcon).Name = "panelGisIcon";
		((Control)panelGisIcon).Size = new Size(265, 37);
		((Control)panelGisIcon).TabIndex = 1;
		((Control)colorBoxGisIconColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxGisIconColor).Anchor = (AnchorStyles)9;
		((Control)colorBoxGisIconColor).Location = new Point(175, 6);
		((Control)colorBoxGisIconColor).Margin = new Padding(2, 2, 2, 2);
		((Control)colorBoxGisIconColor).Name = "colorBoxGisIconColor";
		((Control)colorBoxGisIconColor).Size = new Size(89, 25);
		((Control)colorBoxGisIconColor).TabIndex = 18;
		colorBoxGisIconColor.ShowColorName = false;
		((Control)panelBoxGisIcon).Anchor = (AnchorStyles)9;
		((Control)panelBoxGisIcon).Controls.Add((Control)(object)buttonGisIcon);
		((Control)panelBoxGisIcon).Location = new Point(103, 6);
		((Control)panelBoxGisIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)panelBoxGisIcon).Name = "panelBoxGisIcon";
		((Control)panelBoxGisIcon).Size = new Size(74, 25);
		((Control)panelBoxGisIcon).TabIndex = 17;
		((Control)panelBoxGisIcon).Click += ButtonGisIcon_Click;
		((Control)buttonGisIcon).Anchor = (AnchorStyles)9;
		((Control)buttonGisIcon).Location = new Point(50, 1);
		((Control)buttonGisIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonGisIcon).Name = "buttonGisIcon";
		((Control)buttonGisIcon).Size = new Size(23, 23);
		((Control)buttonGisIcon).TabIndex = 1;
		((Control)buttonGisIcon).TabStop = false;
		((ButtonBase)buttonGisIcon).UseVisualStyleBackColor = true;
		((Control)buttonGisIcon).Click += ButtonGisIcon_Click;
		((Control)labelGisIcon).Location = new Point(0, 9);
		((Control)labelGisIcon).Margin = new Padding(1, 0, 1, 0);
		((Control)labelGisIcon).Name = "labelGisIcon";
		((Control)labelGisIcon).Size = new Size(84, 20);
		((Control)labelGisIcon).TabIndex = 20;
		((Control)labelGisIcon).Text = "Icon";
		((Label)labelGisIcon).TextAlign = (ContentAlignment)16;
		((Control)panelUi).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelUi).Controls.Add((Control)(object)labelExpression);
		((Control)panelUi).Dock = (DockStyle)1;
		((Control)panelUi).Location = new Point(12, 413);
		((Control)panelUi).Margin = new Padding(2, 2, 2, 2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(265, 60);
		((Control)panelUi).TabIndex = 13;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(0, 16);
		((Control)panelBoxExpression).Margin = new Padding(2, 2, 2, 2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(265, 25);
		((Control)panelBoxExpression).TabIndex = 13;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(219, 1);
		((Control)buttonExpressionEdit).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(23, 23);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2, 2, 2, 2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(211, 13);
		((Control)textBoxExpression).TabIndex = 12;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(241, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).Location = new Point(0, 0);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(265, 16);
		((Control)labelExpression).TabIndex = 12;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)listBoxLongitude).AllowDrop = true;
		((Control)listBoxLongitude).Dock = (DockStyle)1;
		((Control)listBoxLongitude).Location = new Point(12, 381);
		((Control)listBoxLongitude).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLongitude).Name = "listBoxLongitude";
		((Control)listBoxLongitude).Size = new Size(265, 32);
		((Control)listBoxLongitude).TabIndex = 11;
		((ListBox)listBoxLongitude).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLongitude).Enter += ListBox_Enter;
		((Control)listBoxLongitude).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLongitude).Dock = (DockStyle)1;
		((Control)labelLongitude).Location = new Point(12, 365);
		((Control)labelLongitude).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLongitude).Name = "labelLongitude";
		((Control)labelLongitude).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLongitude).Size = new Size(265, 16);
		((Control)labelLongitude).TabIndex = 10;
		((Control)labelLongitude).Text = "Longitude";
		((Control)listBoxLatitude).AllowDrop = true;
		((Control)listBoxLatitude).Dock = (DockStyle)1;
		((Control)listBoxLatitude).Location = new Point(12, 333);
		((Control)listBoxLatitude).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLatitude).Name = "listBoxLatitude";
		((Control)listBoxLatitude).Size = new Size(265, 32);
		((Control)listBoxLatitude).TabIndex = 9;
		((ListBox)listBoxLatitude).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLatitude).Enter += ListBox_Enter;
		((Control)listBoxLatitude).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLatitude).Controls.Add((Control)(object)buttonEnterCoordinateDataManually);
		((Control)labelLatitude).Dock = (DockStyle)1;
		((Control)labelLatitude).Location = new Point(12, 301);
		((Control)labelLatitude).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLatitude).Name = "labelLatitude";
		((Control)labelLatitude).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLatitude).Size = new Size(265, 32);
		((Control)labelLatitude).TabIndex = 8;
		((Control)labelLatitude).Text = "Latitude";
		labelLatitude.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterCoordinateDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterCoordinateDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterCoordinateDataManually.Image");
		((Control)buttonEnterCoordinateDataManually).Location = new Point(241, 4);
		((Control)buttonEnterCoordinateDataManually).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonEnterCoordinateDataManually).Name = "buttonEnterCoordinateDataManually";
		((Control)buttonEnterCoordinateDataManually).Size = new Size(24, 23);
		((Control)buttonEnterCoordinateDataManually).TabIndex = 20;
		((Control)buttonEnterCoordinateDataManually).TabStop = false;
		((ButtonBase)buttonEnterCoordinateDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterCoordinateDataManually).Click += ButtonEnterCoordinateDataManually_Click;
		((Control)panelOr).Controls.Add((Control)(object)labelOr);
		((Control)panelOr).Controls.Add((Control)(object)panelLine);
		((Control)panelOr).Dock = (DockStyle)1;
		((Control)panelOr).Location = new Point(12, 273);
		((Control)panelOr).Name = "panelOr";
		((Control)panelOr).Size = new Size(265, 28);
		((Control)panelOr).TabIndex = 6;
		((Control)labelOr).AutoSize = true;
		((Control)labelOr).Location = new Point(125, 15);
		((Control)labelOr).Name = "labelOr";
		((Control)labelOr).Size = new Size(16, 13);
		((Control)labelOr).TabIndex = 14;
		((Control)labelOr).Text = "or";
		((Control)panelLine).Anchor = (AnchorStyles)1;
		((Control)panelLine).Location = new Point(0, 22);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(265, 1);
		((Control)panelLine).TabIndex = 13;
		((Control)listBoxLocationColorMeter).AllowDrop = true;
		((Control)listBoxLocationColorMeter).Dock = (DockStyle)1;
		((Control)listBoxLocationColorMeter).Location = new Point(12, 241);
		((Control)listBoxLocationColorMeter).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLocationColorMeter).Name = "listBoxLocationColorMeter";
		((Control)listBoxLocationColorMeter).Size = new Size(265, 32);
		((Control)listBoxLocationColorMeter).TabIndex = 7;
		((ListBox)listBoxLocationColorMeter).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLocationColorMeter).Enter += ListBox_Enter;
		((Control)listBoxLocationColorMeter).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLocationColorMeter).Dock = (DockStyle)1;
		((Control)labelLocationColorMeter).Location = new Point(12, 225);
		((Control)labelLocationColorMeter).Margin = new Padding(1, 0, 1, 0);
		((Control)labelLocationColorMeter).Name = "labelLocationColorMeter";
		((Control)labelLocationColorMeter).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLocationColorMeter).Size = new Size(265, 16);
		((Control)labelLocationColorMeter).TabIndex = 6;
		((Control)labelLocationColorMeter).Text = "Color";
		((Control)listBoxLocationArgument).AllowDrop = true;
		((Control)listBoxLocationArgument).Dock = (DockStyle)1;
		((Control)listBoxLocationArgument).Location = new Point(12, 193);
		((Control)listBoxLocationArgument).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLocationArgument).Name = "listBoxLocationArgument";
		((Control)listBoxLocationArgument).Size = new Size(265, 32);
		((Control)listBoxLocationArgument).TabIndex = 5;
		((Control)listBoxLocationArgument).Enter += ListBox_Enter;
		((Control)labelLocationArgument).Dock = (DockStyle)1;
		((Control)labelLocationArgument).Location = new Point(12, 177);
		((Control)labelLocationArgument).Margin = new Padding(1, 0, 1, 0);
		((Control)labelLocationArgument).Name = "labelLocationArgument";
		((Control)labelLocationArgument).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLocationArgument).Size = new Size(265, 16);
		((Control)labelLocationArgument).TabIndex = 4;
		((Control)labelLocationArgument).Text = "Argument";
		((Control)listBoxLocationValue).AllowDrop = true;
		((Control)listBoxLocationValue).Dock = (DockStyle)1;
		((Control)listBoxLocationValue).Location = new Point(12, 145);
		((Control)listBoxLocationValue).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLocationValue).Name = "listBoxLocationValue";
		((Control)listBoxLocationValue).Size = new Size(265, 32);
		((Control)listBoxLocationValue).TabIndex = 3;
		((ListBox)listBoxLocationValue).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLocationValue).Enter += ListBox_Enter;
		((Control)listBoxLocationValue).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLocationValue).Dock = (DockStyle)1;
		((Control)labelLocationValue).Location = new Point(12, 129);
		((Control)labelLocationValue).Margin = new Padding(1, 0, 1, 0);
		((Control)labelLocationValue).Name = "labelLocationValue";
		((Control)labelLocationValue).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLocationValue).Size = new Size(265, 16);
		((Control)labelLocationValue).TabIndex = 2;
		((Control)labelLocationValue).Text = "Value";
		((Control)listBoxLocation).AllowDrop = true;
		((Control)listBoxLocation).Dock = (DockStyle)1;
		((Control)listBoxLocation).Location = new Point(12, 97);
		((Control)listBoxLocation).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxLocation).Name = "listBoxLocation";
		((Control)listBoxLocation).Size = new Size(265, 32);
		((Control)listBoxLocation).TabIndex = 1;
		((ListBox)listBoxLocation).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLocation).Enter += ListBox_Enter;
		((Control)listBoxLocation).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLocation).Controls.Add((Control)(object)buttonEnterLocationDataManually);
		((Control)labelLocation).Dock = (DockStyle)1;
		((Control)labelLocation).Location = new Point(12, 65);
		((Control)labelLocation).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLocation).Name = "labelLocation";
		((Control)labelLocation).Padding = new Padding(2, 2, 2, 2);
		((Control)labelLocation).Size = new Size(265, 32);
		((Control)labelLocation).TabIndex = 0;
		((Control)labelLocation).Text = "Location";
		labelLocation.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterLocationDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterLocationDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterLocationDataManually.Image");
		((Control)buttonEnterLocationDataManually).Location = new Point(241, 4);
		((Control)buttonEnterLocationDataManually).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonEnterLocationDataManually).Name = "buttonEnterLocationDataManually";
		((Control)buttonEnterLocationDataManually).Size = new Size(24, 23);
		((Control)buttonEnterLocationDataManually).TabIndex = 19;
		((Control)buttonEnterLocationDataManually).TabStop = false;
		((ButtonBase)buttonEnterLocationDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterLocationDataManually).Click += ButtonEnterLocationDataManually_Click;
		((Control)panelOrGis).Controls.Add((Control)(object)labelOr2);
		((Control)panelOrGis).Controls.Add((Control)(object)panel2);
		((Control)panelOrGis).Dock = (DockStyle)1;
		((Control)panelOrGis).Location = new Point(12, 805);
		((Control)panelOrGis).Name = "panelOrGis";
		((Control)panelOrGis).Size = new Size(265, 28);
		((Control)panelOrGis).TabIndex = 29;
		((Control)labelOr2).AutoSize = true;
		((Control)labelOr2).Location = new Point(125, 15);
		((Control)labelOr2).Name = "labelOr2";
		((Control)labelOr2).Size = new Size(16, 13);
		((Control)labelOr2).TabIndex = 14;
		((Control)labelOr2).Text = "or";
		((Control)panel2).Anchor = (AnchorStyles)1;
		((Control)panel2).Location = new Point(0, 22);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(265, 1);
		((Control)panel2).TabIndex = 13;
		((Control)labelGis).Dock = (DockStyle)1;
		((Control)labelGis).Location = new Point(12, 935);
		((Control)labelGis).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGis).Name = "labelGis";
		((Control)labelGis).Padding = new Padding(2, 2, 2, 2);
		((Control)labelGis).Size = new Size(265, 16);
		((Control)labelGis).TabIndex = 30;
		((Control)labelGis).Text = "GIS";
		((Control)labelGisColor).Dock = (DockStyle)1;
		((Control)labelGisColor).Location = new Point(12, 983);
		((Control)labelGisColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGisColor).Name = "labelGisColor";
		((Control)labelGisColor).Padding = new Padding(2, 2, 2, 2);
		((Control)labelGisColor).Size = new Size(265, 16);
		((Control)labelGisColor).TabIndex = 30;
		((Control)labelGisColor).Text = "Color";
		((Control)labelGisLineSize).Dock = (DockStyle)1;
		((Control)labelGisLineSize).Location = new Point(12, 1031);
		((Control)labelGisLineSize).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGisLineSize).Name = "labelGisLineSize";
		((Control)labelGisLineSize).Padding = new Padding(2, 2, 2, 2);
		((Control)labelGisLineSize).Size = new Size(265, 16);
		((Control)labelGisLineSize).TabIndex = 30;
		((Control)labelGisLineSize).Text = "Line Size";
		((Control)labelGisDescription).Dock = (DockStyle)1;
		((Control)labelGisDescription).Location = new Point(12, 1079);
		((Control)labelGisDescription).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGisDescription).Name = "labelGisDescription";
		((Control)labelGisDescription).Padding = new Padding(2, 2, 2, 2);
		((Control)labelGisDescription).Size = new Size(265, 16);
		((Control)labelGisDescription).TabIndex = 30;
		((Control)labelGisDescription).Text = "Description";
		((Control)listBoxGis).AllowDrop = true;
		((Control)listBoxGis).Dock = (DockStyle)1;
		((Control)listBoxGis).Location = new Point(12, 903);
		((Control)listBoxGis).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxGis).Name = "listBoxGis";
		((Control)listBoxGis).Size = new Size(265, 32);
		((Control)listBoxGis).TabIndex = 31;
		((ListBox)listBoxGis).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxGis).Enter += ListBox_Enter;
		((Control)listBoxGis).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)listBoxGisColor).AllowDrop = true;
		((Control)listBoxGisColor).Dock = (DockStyle)1;
		((Control)listBoxGisColor).Location = new Point(12, 951);
		((Control)listBoxGisColor).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxGisColor).Name = "listBoxGisColor";
		((Control)listBoxGisColor).Size = new Size(265, 32);
		((Control)listBoxGisColor).TabIndex = 31;
		((ListBox)listBoxGisColor).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxGisColor).Enter += ListBox_Enter;
		((Control)listBoxGisColor).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)listBoxGisLineSize).AllowDrop = true;
		((Control)listBoxGisLineSize).Dock = (DockStyle)1;
		((Control)listBoxGisLineSize).Location = new Point(12, 999);
		((Control)listBoxGisLineSize).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxGisLineSize).Name = "listBoxGisLineSize";
		((Control)listBoxGisLineSize).Size = new Size(265, 32);
		((Control)listBoxGisLineSize).TabIndex = 31;
		((ListBox)listBoxGisLineSize).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxGisLineSize).Enter += ListBox_Enter;
		((Control)listBoxGisLineSize).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)listBoxGisDescription).AllowDrop = true;
		((Control)listBoxGisDescription).Dock = (DockStyle)1;
		((Control)listBoxGisDescription).Location = new Point(12, 1047);
		((Control)listBoxGisDescription).Margin = new Padding(2, 2, 2, 2);
		((Control)listBoxGisDescription).Name = "listBoxGisDescription";
		((Control)listBoxGisDescription).Size = new Size(265, 32);
		((Control)listBoxGisDescription).TabIndex = 31;
		((ListBox)listBoxGisDescription).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxGisDescription).Enter += ListBox_Enter;
		((Control)listBoxGisDescription).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)panelGisProvider).Controls.Add((Control)(object)comboBoxProvider);
		((Control)panelGisProvider).Controls.Add((Control)(object)labelProvider);
		((Control)panelGisProvider).Dock = (DockStyle)1;
		((Control)panelGisProvider).Location = new Point(12, 870);
		((Control)panelGisProvider).Name = "panelGisProvider";
		((Control)panelGisProvider).Size = new Size(265, 33);
		((Control)panelGisProvider).TabIndex = 32;
		((Control)comboBoxProvider).Anchor = (AnchorStyles)13;
		comboBoxProvider.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxProvider).FormattingEnabled = true;
		((Control)comboBoxProvider).Location = new Point(89, 6);
		((Control)comboBoxProvider).Margin = new Padding(2, 2, 2, 2);
		((Control)comboBoxProvider).Name = "comboBoxProvider";
		((Control)comboBoxProvider).Size = new Size(176, 21);
		((Control)comboBoxProvider).TabIndex = 15;
		((Control)labelProvider).Location = new Point(0, 6);
		((Control)labelProvider).Margin = new Padding(2, 0, 2, 0);
		((Control)labelProvider).Name = "labelProvider";
		((Control)labelProvider).Size = new Size(83, 22);
		((Control)labelProvider).TabIndex = 20;
		((Control)labelProvider).Text = "Provider";
		((Label)labelProvider).TextAlign = (ContentAlignment)16;
		((DataGridView)gridData).AllowUserToAddRows = false;
		((DataGridView)gridData).AllowUserToDeleteRows = false;
		((DataGridView)gridData).BackgroundColor = Color.White;
		((DataGridView)gridData).BorderStyle = (BorderStyle)0;
		gridData.ColumnHeadersDefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		gridData.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
		gridData.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.1f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		gridData.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
		gridData.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		gridData.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		gridData.ColumnHeadersDefaultCellStyle.WrapMode = (DataGridViewTriState)1;
		gridData.DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		gridData.DefaultCellStyle.BackColor = Color.White;
		gridData.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.1f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		gridData.DefaultCellStyle.ForeColor = SystemColors.ControlText;
		gridData.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		gridData.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		gridData.DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((Control)gridData).Dock = (DockStyle)1;
		((Control)gridData).Location = new Point(12, 44);
		((Control)gridData).Name = "gridData";
		((DataGridView)gridData).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)gridData).Size = new Size(265, 21);
		((Control)gridData).TabIndex = 33;
		((Control)labelGridData).Controls.Add((Control)(object)buttonUseDataFields);
		((Control)labelGridData).Dock = (DockStyle)1;
		((Control)labelGridData).Location = new Point(12, 12);
		((Control)labelGridData).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGridData).Name = "labelGridData";
		((Control)labelGridData).Size = new Size(265, 32);
		((Control)labelGridData).TabIndex = 34;
		((Control)labelGridData).Text = "Data";
		labelGridData.TextAlign = (ContentAlignment)16;
		((Control)buttonUseDataFields).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonUseDataFields).Image = (Image)componentResourceManager.GetObject("buttonUseDataFields.Image");
		((Control)buttonUseDataFields).Location = new Point(242, 4);
		((Control)buttonUseDataFields).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonUseDataFields).Name = "buttonUseDataFields";
		((Control)buttonUseDataFields).Size = new Size(24, 23);
		((Control)buttonUseDataFields).TabIndex = 18;
		((Control)buttonUseDataFields).TabStop = false;
		((ButtonBase)buttonUseDataFields).UseVisualStyleBackColor = true;
		((Control)buttonUseDataFields).Click += ButtonUseDataFields_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(289, 552);
		((Control)this).Controls.Add((Control)(object)labelGisDescription);
		((Control)this).Controls.Add((Control)(object)listBoxGisDescription);
		((Control)this).Controls.Add((Control)(object)labelGisLineSize);
		((Control)this).Controls.Add((Control)(object)listBoxGisLineSize);
		((Control)this).Controls.Add((Control)(object)labelGisColor);
		((Control)this).Controls.Add((Control)(object)listBoxGisColor);
		((Control)this).Controls.Add((Control)(object)labelGis);
		((Control)this).Controls.Add((Control)(object)listBoxGis);
		((Control)this).Controls.Add((Control)(object)panelGisProvider);
		((Control)this).Controls.Add((Control)(object)panelGisIcon);
		((Control)this).Controls.Add((Control)(object)panelOrGis);
		((Control)this).Controls.Add((Control)(object)moreOptionsPanel);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)panelMoreOptions);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)listBoxLongitude);
		((Control)this).Controls.Add((Control)(object)labelLongitude);
		((Control)this).Controls.Add((Control)(object)listBoxLatitude);
		((Control)this).Controls.Add((Control)(object)labelLatitude);
		((Control)this).Controls.Add((Control)(object)panelOr);
		((Control)this).Controls.Add((Control)(object)listBoxLocationColorMeter);
		((Control)this).Controls.Add((Control)(object)labelLocationColorMeter);
		((Control)this).Controls.Add((Control)(object)listBoxLocationArgument);
		((Control)this).Controls.Add((Control)(object)labelLocationArgument);
		((Control)this).Controls.Add((Control)(object)listBoxLocationValue);
		((Control)this).Controls.Add((Control)(object)labelLocationValue);
		((Control)this).Controls.Add((Control)(object)listBoxLocation);
		((Control)this).Controls.Add((Control)(object)labelLocation);
		((Control)this).Controls.Add((Control)(object)gridData);
		((Control)this).Controls.Add((Control)(object)labelGridData);
		((Form)this).Margin = new Padding(0);
		((Control)this).Name = "StiOnlineMapElementEditorForm";
		((Control)this).Padding = new Padding(12, 12, 12, 12);
		((Control)this).Text = "Online Map";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)moreOptionsPanel).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelMoreOptions).ResumeLayout(false);
		((Control)panelLocationColor).ResumeLayout(false);
		((Control)panelColor).ResumeLayout(false);
		((Control)panelHeatmapColor).ResumeLayout(false);
		((Control)panelCulture).ResumeLayout(false);
		((Control)panelType).ResumeLayout(false);
		((Control)panelCustomLocationIcon).ResumeLayout(false);
		((Control)panelCustomIcon).ResumeLayout(false);
		((Control)panelIcon).ResumeLayout(false);
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)panelViewMode).ResumeLayout(false);
		((Control)panelGisIcon).ResumeLayout(false);
		((Control)panelBoxGisIcon).ResumeLayout(false);
		((Control)panelUi).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)labelLatitude).ResumeLayout(false);
		((Control)panelOr).ResumeLayout(false);
		((Control)panelOr).PerformLayout();
		((Control)labelLocation).ResumeLayout(false);
		((Control)panelOrGis).ResumeLayout(false);
		((Control)panelOrGis).PerformLayout();
		((Control)panelGisProvider).ResumeLayout(false);
		((ISupportInitialize)gridData).EndInit();
		((Control)labelGridData).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
