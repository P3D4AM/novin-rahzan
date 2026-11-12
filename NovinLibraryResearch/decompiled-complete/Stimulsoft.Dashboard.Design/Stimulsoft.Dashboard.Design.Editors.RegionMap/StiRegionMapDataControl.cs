using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Dashboard.Design.Editors.RegionMap;

[ToolboxItem(false)]
public class StiRegionMapDataControl : UserControl
{
	private class StiMapLanguage
	{
		public string Language { get; set; }

		public string Text { get; set; }

		public StiMapLanguage(string lang, string text)
		{
			Language = lang;
			Text = text;
		}

		public override string ToString()
		{
			return Text;
		}
	}

	private StiRegionMapElementEditorForm editorForm;

	private string currentMapIdent;

	private bool lockLanguage;

	private StiRegionMapElement mapElement;

	private DataTable currentDataTable;

	private List<StiMapData> mapData = new List<StiMapData>();

	private List<StiMapToolboxInfo> mapsInfos = new List<StiMapToolboxInfo>();

	private IContainer components;

	private Panel panelTop;

	private Panel panelDataGrid;

	private Label labelDataFrom;

	internal StiComboBox comboBoxDataFrom;

	private Panel panelSettings;

	private Label labelMapType;

	private StiComboBox comboBoxMapType;

	private CheckBox checkBoxShowValue;

	private StiDataGridView dataGrid;

	private StiChartButton buttonMapIdent;

	private StiPanelBox panelBoxExpression;

	internal HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private Label labelExpression;

	private StiRegionMapDataColumnsControl columnsControl;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiComboBox comboBoxDisplayNameType;

	private StiComboBox comboBoxLanguage;

	private Label labelDisplayNameType;

	private Label labelLanguage;

	private CheckBox checkBoxColorEach;

	private CheckBox checkBoxShowBubble;

	private Panel panelExpression;

	private Panel panelMapType;

	private Panel panelDisplayNameType;

	private Panel panelLanguage;

	private Panel panelShowValue;

	private Panel panelColorEach;

	private Panel panelShowBubble;

	private StiOperationButton buttonExpressionEdit;

	internal StiRegionMapDataColumnsControl ColumnsControl => columnsControl;

	internal bool LockRefresh { get; set; }

	internal StiMeterListBox SelectedListBox { get; set; }

	private bool AllowGroup
	{
		get
		{
			if (mapElement.MapType != StiMapType.Group)
			{
				return mapElement.MapType == StiMapType.HeatmapWithGroup;
			}
			return true;
		}
	}

	private bool AllowColor
	{
		get
		{
			if (mapElement.MapType == StiMapType.Individual)
			{
				return mapElement.ColorEach;
			}
			return false;
		}
	}

	internal StiKeyMapMeter MapKeyDataColumn => mapElement?.KeyMeter;

	internal StiNameMapMeter MapNameDataColumn => mapElement?.NameMeter;

	internal StiValueMapMeter MapValueDataColumn => mapElement?.ValueMeter;

	internal StiGroupMapMeter MapGroupDataColumn => mapElement?.GroupMeter;

	internal StiColorMapMeter MapColorDataColumn => mapElement?.ColorMeter;

	public StiRegionMapDataControl(StiRegionMapElementEditorForm editorForm, StiRegionMapElement map)
	{
		this.editorForm = editorForm;
		mapElement = map;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			InitMaps();
			InitializeControls();
			DoScaling();
		}
	}

	public void Localize()
	{
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		((Control)buttonMapIdent).Text = GetMapID(mapElement.MapIdent);
		((Control)labelDataFrom).Text = string.Format(StiLocalization.Get("Adapters", "AdapterConnection"), string.Empty);
		int selectedIndex = ((ListControl)comboBoxDataFrom).SelectedIndex;
		((ComboBox)comboBoxDataFrom).BeginUpdate();
		((ComboBox)comboBoxDataFrom).Items.Clear();
		((ComboBox)comboBoxDataFrom).Items.Add((object)StiLocalization.Get("Chart", "DataColumns"));
		((ComboBox)comboBoxDataFrom).Items.Add((object)StiLocalization.Get("PropertyEnum", "FormStartPositionManual"));
		((ListControl)comboBoxDataFrom).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxDataFrom).EndUpdate();
		selectedIndex = ((ListControl)comboBoxDisplayNameType).SelectedIndex;
		((ComboBox)comboBoxDisplayNameType).BeginUpdate();
		((ComboBox)comboBoxDisplayNameType).Items.Clear();
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeNone"));
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeFull"));
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeShort"));
		((ListControl)comboBoxDisplayNameType).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxDisplayNameType).EndUpdate();
		((Control)checkBoxShowBubble).Text = StiLocalization.Get("PropertyMain", "ShowBubble");
		((Control)checkBoxShowValue).Text = StiLocalization.Get("PropertyMain", "ShowValue");
		((Control)checkBoxColorEach).Text = StiLocalization.Get("PropertyMain", "ColorEach");
		((Control)labelMapType).Text = StiLocalization.Get("PropertyMain", "MapType");
		((Control)labelDisplayNameType).Text = StiLocalization.Get("PropertyMain", "DisplayNameType");
		((Control)labelLanguage).Text = StiLocalization.Get("PropertyMain", "Language");
		selectedIndex = ((ListControl)comboBoxMapType).SelectedIndex;
		((ComboBox)comboBoxMapType).BeginUpdate();
		((ComboBox)comboBoxMapType).Items.Clear();
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeIndividual"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeGroup"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeHeatmap"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeHeatmapWithGroup"));
		((ListControl)comboBoxMapType).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxMapType).EndUpdate();
		if (((BaseCollection)((DataGridView)dataGrid).Columns).Count > 0)
		{
			foreach (DataGridViewColumn item in (BaseCollection)((DataGridView)dataGrid).Columns)
			{
				DataGridViewColumn val = item;
				switch (val.Name)
				{
				case "Key":
					val.HeaderText = StiLocalization.Get("PropertyMain", "Key");
					break;
				case "Gss":
					val.HeaderText = "GSS";
					break;
				case "Name":
					val.HeaderText = StiLocalization.Get("PropertyMain", "Name");
					break;
				case "Value":
					val.HeaderText = StiLocalization.Get("PropertyMain", "Value");
					break;
				case "Group":
					val.HeaderText = StiLocalization.Get("PropertyEnum", "StiMapTypeGroup");
					break;
				case "Color":
					val.HeaderText = StiLocalization.Get("PropertyMain", "Color");
					break;
				}
			}
		}
		if (columnsControl != null)
		{
			columnsControl.Localize();
		}
	}

	private void LocalizeMapIdent()
	{
		((Control)buttonMapIdent).Text = GetMapID(mapElement.MapIdent);
	}

	private void DoScaling()
	{
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		((DataGridView)dataGrid).RowTemplate.Height = StiScale.I(24);
		((DataGridView)dataGrid).ColumnHeadersHeight = StiScale.I(26);
		((Control)comboBoxDataFrom).Width = ((Control)panelTop).Width - ((Control)comboBoxDataFrom).Left;
		((Control)comboBoxMapType).Width = ((Control)panelMapType).Width - ((Control)comboBoxMapType).Left;
		((Control)comboBoxDisplayNameType).Width = ((Control)panelDisplayNameType).Width - ((Control)comboBoxDisplayNameType).Left;
	}

	private void InitMaps()
	{
		List<StiMapToolboxInfo> mapToolboxItems = StiToolboxHelper.GetMapToolboxItems();
		foreach (StiMapToolboxInfo item in mapToolboxItems)
		{
			foreach (StiMapToolboxInfo info in item.Infos)
			{
				if (mapsInfos.FirstOrDefault((StiMapToolboxInfo x) => x.MapID == info.MapID) == null)
				{
					mapsInfos.Add(info);
				}
			}
		}
	}

	private void InitializeControls()
	{
		((ComboBox)comboBoxMapType).SelectedIndexChanged += ComboBoxMapType_SelectedIndexChanged;
		ReadMapType();
		ReadShowName();
		((ComboBox)comboBoxDisplayNameType).SelectedIndexChanged += ComboBoxDisplayNameType_SelectedIndexChanged;
		ReadOptions();
		ReadIcon();
		((ComboBox)comboBoxDataFrom).SelectedIndexChanged += ComboBoxDataFrom_SelectedIndexChanged;
		ReadDataFrom();
		((Control)buttonMapIdent).Click += ButtonMapIdent_Click;
		((ComboBox)comboBoxLanguage).SelectedIndexChanged += ComboBoxLanguage_SelectedIndexChanged;
	}

	internal void DetachEvents()
	{
		((ComboBox)comboBoxMapType).SelectedIndexChanged -= ComboBoxMapType_SelectedIndexChanged;
		((ComboBox)comboBoxDisplayNameType).SelectedIndexChanged -= ComboBoxDisplayNameType_SelectedIndexChanged;
		((ComboBox)comboBoxDataFrom).SelectedIndexChanged -= ComboBoxDataFrom_SelectedIndexChanged;
		((Control)buttonMapIdent).Click -= ButtonMapIdent_Click;
	}

	private void ReadDataFrom()
	{
		((ListControl)comboBoxDataFrom).SelectedIndex = ((mapElement.DataFrom != StiMapSource.DataColumns) ? 1 : 0);
	}

	private void ReadIcon()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		buttonMapIdent.ImageFixedSize = ((StiScale.Id == Stimulsoft.Base.SystemScaleID.x1) ? new Size(32, 32) : new Size(64, 64));
		if (StiCustomMapFinder.IsCustom(mapElement.MapIdent))
		{
			string icon = StiCustomMapFinder.GetIcon(mapElement.Report, mapElement.MapIdent);
			if (icon != null)
			{
				byte[] buffer = Convert.FromBase64String(icon);
				Image image = buttonMapIdent.Image;
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(buffer)));
				if (image != null)
				{
					image.Dispose();
				}
			}
			else
			{
				byte[] defaultIcon = StiCustomMapFinder.GetDefaultIcon();
				Image image2 = buttonMapIdent.Image;
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(defaultIcon)));
				if (image2 != null)
				{
					image2.Dispose();
				}
			}
		}
		else
		{
			Image image3 = buttonMapIdent.Image;
			buttonMapIdent.Image = StiImageResizer.GetImage((Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Map." + mapElement.MapIdent + StiScale.StepName + ".png"));
			if (image3 != null)
			{
				image3.Dispose();
			}
		}
	}

	private void ReadOptions()
	{
		checkBoxShowBubble.Checked = mapElement.ShowBubble;
		checkBoxShowValue.Checked = mapElement.ShowValue;
		checkBoxColorEach.Checked = mapElement.ColorEach;
	}

	private void ReadShowName()
	{
		switch (mapElement.ShowName)
		{
		case StiDisplayNameType.None:
			((ListControl)comboBoxDisplayNameType).SelectedIndex = 0;
			break;
		case StiDisplayNameType.Full:
			((ListControl)comboBoxDisplayNameType).SelectedIndex = 1;
			break;
		case StiDisplayNameType.Short:
			((ListControl)comboBoxDisplayNameType).SelectedIndex = 2;
			break;
		}
	}

	private void ReadMapType()
	{
		switch (mapElement.MapType)
		{
		case StiMapType.Individual:
			((ListControl)comboBoxMapType).SelectedIndex = 0;
			break;
		case StiMapType.Group:
			((ListControl)comboBoxMapType).SelectedIndex = 1;
			break;
		case StiMapType.Heatmap:
			((ListControl)comboBoxMapType).SelectedIndex = 2;
			break;
		case StiMapType.HeatmapWithGroup:
			((ListControl)comboBoxMapType).SelectedIndex = 3;
			break;
		}
	}

	private string GetMapID(string id)
	{
		StiMapID result = StiMapID.USA;
		if (Enum.TryParse<StiMapID>(id, out result))
		{
			if (!StiLocalization.IsRussian)
			{
				return StiMapLoc.GetEn(result);
			}
			return StiMapLoc.GetRu(result);
		}
		if (id == StiMapID.USAAndCanada.ToString())
		{
			return "USA+Canada";
		}
		if (id == StiMapID.NorthAmerica.ToString())
		{
			return "North America";
		}
		if (id == StiMapID.SouthAmerica.ToString())
		{
			return "South America";
		}
		if (id == StiMapID.BosniaAndHerzegovina.ToString())
		{
			return "Bosnia And Herzegovina";
		}
		if (id == StiMapID.CzechRepublic.ToString())
		{
			return "Czech Republic";
		}
		if (id == StiMapID.FalklandIslands.ToString())
		{
			return "Falkland Islands";
		}
		if (id == StiMapID.NewZealand.ToString())
		{
			return "New Zealand";
		}
		if (id == StiMapID.SanMarino.ToString())
		{
			return "San Marino";
		}
		if (id == StiMapID.SaudiArabia.ToString())
		{
			return "Saudi Arabia";
		}
		if (id == StiMapID.SouthAfrica.ToString())
		{
			return "South Africa";
		}
		if (id == StiMapID.SouthKorea.ToString())
		{
			return "South Korea";
		}
		return id;
	}

	public void CalculateControlHeight()
	{
		if (!((Control)this).Visible)
		{
			return;
		}
		columnsControl.CalculateControlHeight();
		if (mapElement.DataFrom == StiMapSource.DataColumns)
		{
			((Control)columnsControl).Visible = true;
			((Control)panelDataGrid).Visible = false;
			((Control)panelColorEach).Visible = mapElement.MapType == StiMapType.Individual;
		}
		else
		{
			((Control)columnsControl).Visible = false;
			((Control)panelDataGrid).Visible = true;
			((Control)panelColorEach).Visible = mapElement.MapType == StiMapType.Individual;
		}
		StiMapToolboxInfo stiMapToolboxInfo = mapsInfos.FirstOrDefault((StiMapToolboxInfo x) => x.MapID == mapElement.MapIdent);
		if (stiMapToolboxInfo != null && stiMapToolboxInfo.Language != null)
		{
			((Control)panelLanguage).Visible = true;
			lockLanguage = true;
			int selectedIndex = 0;
			((ComboBox)comboBoxLanguage).Items.Clear();
			int num = 0;
			string[] language = stiMapToolboxInfo.Language;
			foreach (string text in language)
			{
				StiMapLanguage stiMapLanguage = new StiMapLanguage(text, stiMapToolboxInfo.GetLangOriginalName(text));
				((ComboBox)comboBoxLanguage).Items.Add((object)stiMapLanguage);
				if (text == mapElement.Language)
				{
					selectedIndex = num;
				}
				num++;
			}
			((ListControl)comboBoxLanguage).SelectedIndex = selectedIndex;
			lockLanguage = false;
		}
		else
		{
			((Control)panelLanguage).Visible = false;
		}
		if (((Control)panelSettings).Visible)
		{
			((Control)panelSettings).Height = (from Control c in (IEnumerable)((Control)panelSettings).Controls
				where c.Visible
				select c.Bottom).Max();
		}
		((Control)this).Height = (from Control c in (IEnumerable)((Control)this).Controls
			where c.Visible
			select c.Height).Sum();
	}

	private void RefreshExpressionHint()
	{
		if (columnsControl != null)
		{
			textBoxExpression.Hint = ((SelectedListBox != null) ? Loc.GetMain("Field") : string.Empty);
		}
	}

	internal void Init()
	{
		columnsControl.Init(editorForm, mapElement);
	}

	private static Image GetMapImage(string ident)
	{
		string text = "Stimulsoft.Report.Design.Resources.Office2013.Map." + ident;
		if (StiImageUtils.ExistsImage("Stimulsoft.Report.Design", text + StiScale.StepName + ".png"))
		{
			return (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", text + StiScale.StepName + ".png", makeTransparent: false);
		}
		return StiImageResizer.GetImage((Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", text + ".png", makeTransparent: false));
	}

	internal void RefreshMapTextIcon()
	{
		RefreshMapImage();
		LocalizeMapIdent();
	}

	private void RefreshMapImage()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		if (currentMapIdent == mapElement.MapIdent)
		{
			return;
		}
		if (StiCustomMapFinder.IsCustom(mapElement.MapIdent))
		{
			string icon = StiCustomMapFinder.GetIcon(mapElement.Report, mapElement.MapIdent);
			if (icon != null)
			{
				byte[] buffer = Convert.FromBase64String(icon);
				Image image = buttonMapIdent.Image;
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(buffer)));
				if (image != null)
				{
					image.Dispose();
				}
			}
			else
			{
				byte[] defaultIcon = StiCustomMapFinder.GetDefaultIcon();
				Image image2 = buttonMapIdent.Image;
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(defaultIcon)));
				if (image2 != null)
				{
					image2.Dispose();
				}
			}
		}
		else
		{
			Image image3 = buttonMapIdent.Image;
			buttonMapIdent.Image = GetMapImage(mapElement.MapIdent);
			if (image3 != null)
			{
				image3.Dispose();
			}
		}
		currentMapIdent = mapElement.MapIdent;
	}

	public void RefreshUiPanel()
	{
		if (!LockRefresh)
		{
			LockRefresh = true;
			((Control)panelExpression).Visible = SelectedListBox != null && ((ListControl)SelectedListBox).SelectedIndex != -1;
			((Control)this).SuspendLayout();
			RefreshMapTextIcon();
			((Control)this).ResumeLayout();
			LockRefresh = false;
		}
	}

	private string SplitToWord(string text)
	{
		string text2 = text.ToUpperInvariant();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			if (i > 0 && text[i] == text2[i])
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(text2[i]);
			}
			else
			{
				stringBuilder.Append(text[i]);
			}
		}
		return stringBuilder.ToString();
	}

	internal void SetDefaultValues(List<StiMapData> data)
	{
		int num = 1;
		foreach (StiMapData item in data.OrderBy((StiMapData x) => x.Key))
		{
			item.Group = num.ToString();
			num++;
			item.Name = SplitToWord(item.Key);
		}
	}

	internal void ResetData()
	{
		foreach (DataRow row in currentDataTable.Rows)
		{
			row[1] = null;
			row[2] = 0.0;
			row[3] = null;
			row[4] = null;
		}
		foreach (StiMapData mapDatum in mapData)
		{
			mapDatum.Name = null;
			mapDatum.Value = null;
			mapDatum.Group = null;
			mapDatum.Color = null;
		}
		mapElement.MapData = StiJsonHelper.SaveToJsonString(mapData);
	}

	private void UpdateData()
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		((DataGridView)dataGrid).DataSource = null;
		mapData = (from x in mapElement.GetMapData()
			orderby x.Key
			select x).ToList();
		bool allowGroup = AllowGroup;
		bool allowColor = AllowColor;
		currentDataTable = new DataTable("Data");
		currentDataTable.Columns.Add("Key", typeof(string));
		currentDataTable.Columns.Add("Name", typeof(string));
		currentDataTable.Columns.Add("Value", typeof(string));
		if (allowGroup)
		{
			currentDataTable.Columns.Add("Group", typeof(string));
		}
		if (allowColor)
		{
			currentDataTable.Columns.Add("Color", typeof(string));
		}
		foreach (StiMapData mapDatum in mapData)
		{
			DataRow dataRow = currentDataTable.NewRow();
			dataRow["Key"] = mapDatum.Key;
			dataRow["Name"] = mapDatum.Name;
			dataRow["Value"] = mapDatum.Value;
			if (allowGroup)
			{
				dataRow["Group"] = mapDatum.Group;
			}
			if (allowColor)
			{
				dataRow["Color"] = mapDatum.Color;
			}
			currentDataTable.Rows.Add(dataRow);
		}
		((DataGridView)dataGrid).DataSource = currentDataTable;
		foreach (DataGridViewColumn item in (BaseCollection)((DataGridView)dataGrid).Columns)
		{
			DataGridViewColumn val = item;
			((DataGridViewBand)val).Resizable = (DataGridViewTriState)2;
			val.SortMode = (DataGridViewColumnSortMode)0;
			((DataGridViewCell)val.HeaderCell).Style.Alignment = (DataGridViewContentAlignment)32;
			switch (val.Name)
			{
			case "Key":
				((DataGridViewBand)val).Resizable = (DataGridViewTriState)1;
				val.HeaderText = StiLocalization.Get("PropertyMain", "Key");
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
				val.Width = StiScale.I(100);
				break;
			case "Name":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Name");
				val.Width = StiScale.I(130);
				break;
			case "Value":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Value");
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)64;
				val.Width = StiScale.I(80);
				break;
			case "Group":
				val.HeaderText = StiLocalization.Get("PropertyEnum", "StiMapTypeGroup");
				val.Width = StiScale.I(80);
				break;
			case "Color":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Color");
				val.Width = StiScale.I(80);
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
				break;
			}
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		dataGrid.InitGridProperties();
		((DataGridView)dataGrid).EditMode = (DataGridViewEditMode)2;
		dataGrid.ColumnHeadersDefaultCellStyle.Font = ((Control)dataGrid).Font;
		dataGrid.DefaultCellStyle.Font = ((Control)dataGrid).Font;
		((Control)comboBoxDataFrom).Focus();
		UpdateData();
		RefreshUiPanel();
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(mapElement, ref textBoxExpression))
		{
			editorForm.RefreshElementView();
		}
	}

	private void ButtonMapIdent_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		StiMapCategoriesForm stiMapCategoriesForm = new StiMapCategoriesForm(mapElement.Report);
		try
		{
			((Form)stiMapCategoriesForm).Owner = (Form)(object)editorForm;
			if ((int)((Form)stiMapCategoriesForm).ShowDialog() == 1 && !string.IsNullOrEmpty(stiMapCategoriesForm.MapID))
			{
				mapElement.MapIdent = stiMapCategoriesForm.MapID;
				mapElement.MapData = null;
				UpdateData();
				columnsControl.UpdateColumnsState();
				RefreshUiPanel();
				editorForm.RefreshElementView();
				editorForm.CalculateWindowSize();
			}
		}
		finally
		{
			((IDisposable)stiMapCategoriesForm)?.Dispose();
		}
	}

	private void PanelDataGrid_Paint(object sender, PaintEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Pen val = new Pen(Color.FromArgb(255, 170, 170, 170), 1f);
		try
		{
			graphics.DrawRectangle(val, 0, 0, ((Control)panelDataGrid).Width - 1, ((Control)panelDataGrid).Height - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ComboBoxDisplayNameType_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (((ListControl)comboBoxDisplayNameType).SelectedIndex)
		{
		case 0:
			mapElement.ShowName = StiDisplayNameType.None;
			break;
		case 1:
			mapElement.ShowName = StiDisplayNameType.Full;
			break;
		case 2:
			mapElement.ShowName = StiDisplayNameType.Short;
			break;
		}
		editorForm.RefreshElementView();
	}

	private void CheckBoxShowValue_CheckedChanged(object sender, EventArgs e)
	{
		mapElement.ShowValue = checkBoxShowValue.Checked;
		editorForm.RefreshElementView();
	}

	private void CheckBoxColorEach_CheckedChanged(object sender, EventArgs e)
	{
		mapElement.ColorEach = checkBoxColorEach.Checked;
		UpdateData();
		editorForm.RefreshElementView();
	}

	private void CheckBoxShowBubble_CheckedChanged(object sender, EventArgs e)
	{
		mapElement.ShowBubble = checkBoxShowBubble.Checked;
		UpdateData();
		editorForm.RefreshElementView();
	}

	private void ComboBoxDataFrom_SelectedIndexChanged(object sender, EventArgs e)
	{
		mapElement.DataFrom = ((((ListControl)comboBoxDataFrom).SelectedIndex == 0) ? StiMapSource.DataColumns : StiMapSource.Manual);
		editorForm.CalculateWindowSize();
		editorForm.RefreshElementView();
		editorForm.RefreshPropertyGrid();
	}

	private void ComboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (((ListControl)comboBoxMapType).SelectedIndex)
		{
		case 0:
			mapElement.MapType = StiMapType.Individual;
			break;
		case 1:
			mapElement.MapType = StiMapType.Group;
			break;
		case 2:
			mapElement.MapType = StiMapType.Heatmap;
			break;
		case 3:
			mapElement.MapType = StiMapType.HeatmapWithGroup;
			break;
		}
		columnsControl.UpdateColumnsState();
		editorForm.CalculateWindowSize();
		UpdateData();
		editorForm.RefreshElementView();
	}

	private void ComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!lockLanguage)
		{
			StiMapLanguage stiMapLanguage = ((ComboBox)comboBoxLanguage).SelectedItem as StiMapLanguage;
			mapElement.Language = ((stiMapLanguage != null && stiMapLanguage.Language != "EN") ? stiMapLanguage.Language : null);
			UpdateData();
			RefreshUiPanel();
			editorForm.RefreshElementView();
		}
	}

	private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex != -1 && mapData != null && mapData.Count != 0)
		{
			StiMapData stiMapData = mapData[e.RowIndex];
			DataRow dataRow = currentDataTable.Rows[e.RowIndex];
			DataTable dataTable = (DataTable)((DataGridView)dataGrid).DataSource;
			switch (dataTable.Columns[e.ColumnIndex].ColumnName)
			{
			case "Name":
				stiMapData.Name = dataRow["Name"] as string;
				break;
			case "Value":
				stiMapData.Value = dataRow["Value"] as string;
				break;
			case "Group":
				stiMapData.Group = dataRow["Group"] as string;
				break;
			case "Color":
				stiMapData.Color = dataRow["Color"] as string;
				break;
			}
			mapElement.MapData = StiJsonHelper.SaveToJsonString(mapData);
			editorForm.RefreshElementView();
		}
	}

	private void DataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		StiTablePartDrawer stiTablePartDrawer = new StiTablePartDrawer(e, (DataGridView)(object)dataGrid);
		stiTablePartDrawer.DrawCell();
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			editorForm.RefreshElementView();
		}
	}

	private void TextBoxExpression_Leave(object sender, EventArgs e)
	{
		editorForm.RefreshElementView();
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		if (!LockRefresh && columnsControl != null)
		{
			if (SelectedListBox == columnsControl.listBoxKey && MapKeyDataColumn != null)
			{
				MapKeyDataColumn.Expression = ((Control)textBoxExpression).Text;
				columnsControl.listBoxKey.SetSelectedText(StiLabelHelper.GetLabel(MapKeyDataColumn));
			}
			if (SelectedListBox == columnsControl.listBoxName && MapNameDataColumn != null)
			{
				MapNameDataColumn.Expression = ((Control)textBoxExpression).Text;
				columnsControl.listBoxName.SetSelectedText(StiLabelHelper.GetLabel(MapNameDataColumn));
			}
			if (SelectedListBox == columnsControl.listBoxValue && MapValueDataColumn != null)
			{
				MapValueDataColumn.Expression = ((Control)textBoxExpression).Text;
				columnsControl.listBoxValue.SetSelectedText(StiLabelHelper.GetLabel(MapValueDataColumn));
			}
			if (SelectedListBox == columnsControl.listBoxGroup && MapGroupDataColumn != null)
			{
				MapGroupDataColumn.Expression = ((Control)textBoxExpression).Text;
				columnsControl.listBoxGroup.SetSelectedText(StiLabelHelper.GetLabel(MapGroupDataColumn));
			}
			if (SelectedListBox == columnsControl.listBoxColor && MapColorDataColumn != null)
			{
				MapColorDataColumn.Expression = ((Control)textBoxExpression).Text;
				columnsControl.listBoxColor.SetSelectedText(StiLabelHelper.GetLabel(MapColorDataColumn));
			}
			RefreshExpressionHint();
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiRegionMapColumnsExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, editorForm, SelectedListBox).CreateItems();
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
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Expected O, but got Unknown
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee1: Expected O, but got Unknown
		//IL_0f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_102e: Expected O, but got Unknown
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fe: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRegionMapDataControl));
		labelDataFrom = new Label();
		comboBoxDataFrom = new StiComboBox();
		labelMapType = new Label();
		comboBoxMapType = new StiComboBox();
		checkBoxShowValue = new CheckBox();
		dataGrid = new StiDataGridView();
		panelTop = new Panel();
		panelDataGrid = new Panel();
		panelSettings = new Panel();
		buttonMapIdent = new StiChartButton();
		checkBoxColorEach = new CheckBox();
		checkBoxShowBubble = new CheckBox();
		comboBoxDisplayNameType = new StiComboBox();
		comboBoxLanguage = new StiComboBox();
		labelDisplayNameType = new Label();
		labelLanguage = new Label();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new Label();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		columnsControl = new StiRegionMapDataColumnsControl();
		panelExpression = new Panel();
		panelMapType = new Panel();
		panelDisplayNameType = new Panel();
		panelLanguage = new Panel();
		panelShowValue = new Panel();
		panelColorEach = new Panel();
		panelShowBubble = new Panel();
		((ISupportInitialize)dataGrid).BeginInit();
		((Control)panelTop).SuspendLayout();
		((Control)panelDataGrid).SuspendLayout();
		((Control)panelSettings).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelExpression).SuspendLayout();
		((Control)panelMapType).SuspendLayout();
		((Control)panelDisplayNameType).SuspendLayout();
		((Control)panelLanguage).SuspendLayout();
		((Control)panelShowValue).SuspendLayout();
		((Control)panelColorEach).SuspendLayout();
		((Control)panelShowBubble).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelDataFrom).ForeColor = Color.DimGray;
		((Control)labelDataFrom).Location = new Point(4, 6);
		((Control)labelDataFrom).Margin = new Padding(2, 0, 2, 0);
		((Control)labelDataFrom).Name = "labelDataFrom";
		((Control)labelDataFrom).Size = new Size(195, 21);
		((Control)labelDataFrom).TabIndex = 0;
		((Control)labelDataFrom).Text = "Data From";
		labelDataFrom.TextAlign = (ContentAlignment)16;
		((Control)comboBoxDataFrom).Anchor = (AnchorStyles)13;
		comboBoxDataFrom.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDataFrom).FormattingEnabled = true;
		((Control)comboBoxDataFrom).Location = new Point(203, 6);
		((Control)comboBoxDataFrom).Margin = new Padding(2);
		((Control)comboBoxDataFrom).Name = "comboBoxDataFrom";
		((Control)comboBoxDataFrom).Size = new Size(271, 21);
		((Control)comboBoxDataFrom).TabIndex = 1;
		((Control)labelMapType).ForeColor = Color.DimGray;
		((Control)labelMapType).Location = new Point(0, 12);
		((Control)labelMapType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMapType).Name = "labelMapType";
		((Control)labelMapType).Size = new Size(199, 19);
		((Control)labelMapType).TabIndex = 4;
		((Control)labelMapType).Text = "Map Type";
		labelMapType.TextAlign = (ContentAlignment)16;
		((Control)comboBoxMapType).Anchor = (AnchorStyles)13;
		comboBoxMapType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMapType).FormattingEnabled = true;
		((Control)comboBoxMapType).Location = new Point(203, 12);
		((Control)comboBoxMapType).Margin = new Padding(2);
		((Control)comboBoxMapType).Name = "comboBoxMapType";
		((Control)comboBoxMapType).Size = new Size(271, 21);
		((Control)comboBoxMapType).TabIndex = 2;
		((Control)checkBoxShowValue).AutoSize = true;
		((Control)checkBoxShowValue).Location = new Point(203, 12);
		((Control)checkBoxShowValue).Margin = new Padding(2);
		((Control)checkBoxShowValue).Name = "checkBoxShowValue";
		((Control)checkBoxShowValue).Size = new Size(83, 17);
		((Control)checkBoxShowValue).TabIndex = 4;
		((Control)checkBoxShowValue).Text = "Show Value";
		((ButtonBase)checkBoxShowValue).UseVisualStyleBackColor = true;
		checkBoxShowValue.CheckedChanged += CheckBoxShowValue_CheckedChanged;
		((DataGridView)dataGrid).AllowUserToAddRows = false;
		((DataGridView)dataGrid).AllowUserToDeleteRows = false;
		((DataGridView)dataGrid).AllowUserToResizeRows = false;
		dataGrid.ColumnHeadersDefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		dataGrid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
		dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
		dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		dataGrid.ColumnHeadersDefaultCellStyle.WrapMode = (DataGridViewTriState)1;
		((DataGridView)dataGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dataGrid.DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		dataGrid.DefaultCellStyle.BackColor = SystemColors.Window;
		dataGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		dataGrid.DefaultCellStyle.ForeColor = SystemColors.ControlText;
		dataGrid.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		dataGrid.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		dataGrid.DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((Control)dataGrid).Dock = (DockStyle)5;
		((Control)dataGrid).Location = new Point(2, 2);
		((Control)dataGrid).Margin = new Padding(2);
		((Control)dataGrid).Name = "dataGrid";
		((DataGridView)dataGrid).RowHeadersWidth = 82;
		((DataGridView)dataGrid).ShowEditingIcon = false;
		((Control)dataGrid).Size = new Size(470, 256);
		((Control)dataGrid).TabIndex = 0;
		((Control)dataGrid).TabStop = false;
		((DataGridView)dataGrid).CellPainting += new DataGridViewCellPaintingEventHandler(DataGrid_CellPainting);
		((DataGridView)dataGrid).CellValueChanged += new DataGridViewCellEventHandler(DataGrid_CellValueChanged);
		((Control)panelTop).BackColor = Color.White;
		((Control)panelTop).Controls.Add((Control)(object)labelDataFrom);
		((Control)panelTop).Controls.Add((Control)(object)comboBoxDataFrom);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Margin = new Padding(2);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(474, 38);
		((Control)panelTop).TabIndex = 0;
		((Control)panelDataGrid).BackColor = Color.White;
		((Control)panelDataGrid).Controls.Add((Control)(object)dataGrid);
		((Control)panelDataGrid).Dock = (DockStyle)1;
		((Control)panelDataGrid).Location = new Point(0, 298);
		((Control)panelDataGrid).Margin = new Padding(2);
		((Control)panelDataGrid).Name = "panelDataGrid";
		((Control)panelDataGrid).Padding = new Padding(2);
		((Control)panelDataGrid).Size = new Size(474, 260);
		((Control)panelDataGrid).TabIndex = 1;
		((Control)panelDataGrid).Paint += new PaintEventHandler(PanelDataGrid_Paint);
		((Control)panelSettings).BackColor = Color.White;
		((Control)panelSettings).Controls.Add((Control)(object)buttonMapIdent);
		((Control)panelSettings).Dock = (DockStyle)1;
		((Control)panelSettings).Location = new Point(0, 558);
		((Control)panelSettings).Margin = new Padding(2);
		((Control)panelSettings).Name = "panelSettings";
		((Control)panelSettings).Size = new Size(474, 56);
		((Control)panelSettings).TabIndex = 2;
		((Control)buttonMapIdent).AccessibleRole = (AccessibleRole)43;
		((Control)buttonMapIdent).Anchor = (AnchorStyles)13;
		buttonMapIdent.ColorTable = eButtonColor.OrangeWithBackground;
		buttonMapIdent.Is20Button = false;
		((Control)buttonMapIdent).Location = new Point(0, 12);
		buttonMapIdent.LockClosePopup = false;
		((Control)buttonMapIdent).Margin = new Padding(4);
		((Control)buttonMapIdent).Name = "buttonMapIdent";
		buttonMapIdent.PopupSide = ePopupSide.Right;
		((Control)buttonMapIdent).Size = new Size(474, 42);
		buttonMapIdent.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonMapIdent).TabIndex = 0;
		((Control)buttonMapIdent).TabStop = false;
		((Control)buttonMapIdent).Text = "Map";
		buttonMapIdent.TextAlignment = eButtonTextAlignment.Left;
		((Control)checkBoxColorEach).AutoSize = true;
		((Control)checkBoxColorEach).Location = new Point(203, 12);
		((Control)checkBoxColorEach).Margin = new Padding(2);
		((Control)checkBoxColorEach).Name = "checkBoxColorEach";
		((Control)checkBoxColorEach).Size = new Size(78, 17);
		((Control)checkBoxColorEach).TabIndex = 10;
		((Control)checkBoxColorEach).Text = "Color Each";
		((ButtonBase)checkBoxColorEach).UseVisualStyleBackColor = true;
		checkBoxColorEach.CheckedChanged += CheckBoxColorEach_CheckedChanged;
		((Control)checkBoxShowBubble).AutoSize = true;
		((Control)checkBoxShowBubble).Location = new Point(203, 12);
		((Control)checkBoxShowBubble).Margin = new Padding(2);
		((Control)checkBoxShowBubble).Name = "checkBoxShowBubble";
		((Control)checkBoxShowBubble).Size = new Size(89, 17);
		((Control)checkBoxShowBubble).TabIndex = 10;
		((Control)checkBoxShowBubble).Text = "Show Bubble";
		((ButtonBase)checkBoxShowBubble).UseVisualStyleBackColor = true;
		checkBoxShowBubble.CheckedChanged += CheckBoxShowBubble_CheckedChanged;
		((Control)comboBoxDisplayNameType).Anchor = (AnchorStyles)13;
		comboBoxDisplayNameType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDisplayNameType).FormattingEnabled = true;
		((Control)comboBoxDisplayNameType).Location = new Point(203, 12);
		((Control)comboBoxDisplayNameType).Margin = new Padding(2);
		((Control)comboBoxDisplayNameType).Name = "comboBoxDisplayNameType";
		((Control)comboBoxDisplayNameType).Size = new Size(271, 21);
		((Control)comboBoxDisplayNameType).TabIndex = 3;
		((Control)comboBoxLanguage).Anchor = (AnchorStyles)13;
		comboBoxLanguage.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxLanguage).FormattingEnabled = true;
		((Control)comboBoxLanguage).Location = new Point(203, 12);
		((Control)comboBoxLanguage).Margin = new Padding(2);
		((Control)comboBoxLanguage).Name = "comboBoxLanguage";
		((Control)comboBoxLanguage).Size = new Size(271, 21);
		((Control)comboBoxLanguage).TabIndex = 3;
		((Control)labelDisplayNameType).ForeColor = Color.DimGray;
		((Control)labelDisplayNameType).Location = new Point(0, 12);
		((Control)labelDisplayNameType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelDisplayNameType).Name = "labelDisplayNameType";
		((Control)labelDisplayNameType).Size = new Size(199, 21);
		((Control)labelDisplayNameType).TabIndex = 9;
		((Control)labelDisplayNameType).Text = "Display Name Type";
		labelDisplayNameType.TextAlign = (ContentAlignment)16;
		((Control)labelLanguage).ForeColor = Color.DimGray;
		((Control)labelLanguage).Location = new Point(0, 12);
		((Control)labelLanguage).Margin = new Padding(2, 0, 2, 0);
		((Control)labelLanguage).Name = "labelLanguage";
		((Control)labelLanguage).Size = new Size(199, 21);
		((Control)labelLanguage).TabIndex = 9;
		((Control)labelLanguage).Text = "Language";
		labelLanguage.TextAlign = (ContentAlignment)16;
		((Control)panelBoxExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(203, 12);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(271, 25);
		((Control)panelBoxExpression).TabIndex = 1;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(225, 1);
		((Control)buttonExpressionEdit).Margin = new Padding(2);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(23, 23);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(218, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).Enter += TextBoxExpression_Leave;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(247, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(0, 12);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(199, 24);
		((Control)labelExpression).TabIndex = 8;
		((Control)labelExpression).Text = "Expression";
		labelExpression.TextAlign = (ContentAlignment)16;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(203, 190);
		((Control)contextMenuBar).Margin = new Padding(2);
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
		menuExpression.LockClosePopup = false;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)columnsControl).BackColor = Color.White;
		((Control)columnsControl).Dock = (DockStyle)1;
		((Control)columnsControl).Location = new Point(0, 38);
		((Control)columnsControl).Margin = new Padding(1);
		((Control)columnsControl).Name = "columnsControl";
		((Control)columnsControl).Size = new Size(474, 260);
		((Control)columnsControl).TabIndex = 1;
		((Control)columnsControl).Visible = false;
		((Control)panelExpression).Controls.Add((Control)(object)labelExpression);
		((Control)panelExpression).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelExpression).Dock = (DockStyle)1;
		((Control)panelExpression).Location = new Point(0, 614);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Size = new Size(474, 38);
		((Control)panelExpression).TabIndex = 13;
		((Control)panelMapType).Controls.Add((Control)(object)comboBoxMapType);
		((Control)panelMapType).Controls.Add((Control)(object)labelMapType);
		((Control)panelMapType).Dock = (DockStyle)1;
		((Control)panelMapType).Location = new Point(0, 652);
		((Control)panelMapType).Name = "panelMapType";
		((Control)panelMapType).Size = new Size(474, 38);
		((Control)panelMapType).TabIndex = 14;
		((Control)panelDisplayNameType).Controls.Add((Control)(object)comboBoxDisplayNameType);
		((Control)panelDisplayNameType).Controls.Add((Control)(object)labelDisplayNameType);
		((Control)panelDisplayNameType).Dock = (DockStyle)1;
		((Control)panelDisplayNameType).Location = new Point(0, 690);
		((Control)panelDisplayNameType).Name = "panelLanguage";
		((Control)panelDisplayNameType).Size = new Size(474, 38);
		((Control)panelDisplayNameType).TabIndex = 15;
		((Control)panelLanguage).Controls.Add((Control)(object)comboBoxLanguage);
		((Control)panelLanguage).Controls.Add((Control)(object)labelLanguage);
		((Control)panelLanguage).Dock = (DockStyle)1;
		((Control)panelLanguage).Location = new Point(0, 690);
		((Control)panelLanguage).Name = "panelDisplayNameType";
		((Control)panelLanguage).Size = new Size(474, 38);
		((Control)panelLanguage).TabIndex = 16;
		((Control)panelShowValue).Controls.Add((Control)(object)checkBoxShowValue);
		((Control)panelShowValue).Dock = (DockStyle)1;
		((Control)panelShowValue).Location = new Point(0, 728);
		((Control)panelShowValue).Name = "panelShowValue";
		((Control)panelShowValue).Size = new Size(474, 32);
		((Control)panelShowValue).TabIndex = 17;
		((Control)panelColorEach).Controls.Add((Control)(object)checkBoxColorEach);
		((Control)panelColorEach).Dock = (DockStyle)1;
		((Control)panelColorEach).Location = new Point(0, 760);
		((Control)panelColorEach).Name = "panelColorEach";
		((Control)panelColorEach).Size = new Size(474, 32);
		((Control)panelColorEach).TabIndex = 18;
		((Control)panelShowBubble).Controls.Add((Control)(object)checkBoxShowBubble);
		((Control)panelShowBubble).Dock = (DockStyle)1;
		((Control)panelShowBubble).Location = new Point(0, 792);
		((Control)panelShowBubble).Name = "panelShowBubble";
		((Control)panelShowBubble).Size = new Size(474, 32);
		((Control)panelShowBubble).TabIndex = 19;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelShowBubble);
		((Control)this).Controls.Add((Control)(object)panelColorEach);
		((Control)this).Controls.Add((Control)(object)panelShowValue);
		((Control)this).Controls.Add((Control)(object)panelLanguage);
		((Control)this).Controls.Add((Control)(object)panelDisplayNameType);
		((Control)this).Controls.Add((Control)(object)panelMapType);
		((Control)this).Controls.Add((Control)(object)panelExpression);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)panelSettings);
		((Control)this).Controls.Add((Control)(object)panelDataGrid);
		((Control)this).Controls.Add((Control)(object)columnsControl);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Margin = new Padding(2);
		((Control)this).Name = "StiRegionMapDataControl";
		((Control)this).Size = new Size(474, 837);
		((UserControl)this).Load += This_Load;
		((ISupportInitialize)dataGrid).EndInit();
		((Control)panelTop).ResumeLayout(false);
		((Control)panelDataGrid).ResumeLayout(false);
		((Control)panelSettings).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelMapType).ResumeLayout(false);
		((Control)panelDisplayNameType).ResumeLayout(false);
		((Control)panelLanguage).ResumeLayout(false);
		((Control)panelShowValue).ResumeLayout(false);
		((Control)panelShowValue).PerformLayout();
		((Control)panelColorEach).ResumeLayout(false);
		((Control)panelColorEach).PerformLayout();
		((Control)panelShowBubble).ResumeLayout(false);
		((Control)panelShowBubble).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
