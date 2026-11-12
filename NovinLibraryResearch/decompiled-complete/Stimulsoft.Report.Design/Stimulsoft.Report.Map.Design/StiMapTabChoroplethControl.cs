using System;
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
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Report.Map.Design;

[ToolboxItem(false)]
public class StiMapTabChoroplethControl : UserControl
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

	private StiMapDesignerForm designerForm;

	private StiMap map;

	private DataTable currentDataTable;

	private List<StiMapData> mapData = new List<StiMapData>();

	private List<StiMapToolboxInfo> mapsInfos = new List<StiMapToolboxInfo>();

	private bool lockLanguage;

	private IContainer components;

	private Panel bottomSeparator;

	private Panel panelDataGrid;

	private Panel panelTop;

	private Panel panelBody;

	private Label labelDataFrom;

	internal StiComboBox comboBoxDataFrom;

	private Panel panelBottom;

	private Label labelMapType;

	private StiComboBox comboBoxMapType;

	private Label labelLang;

	private StiComboBox comboBoxLang;

	private CheckBox checkBoxShowValue;

	private DataGridView dataGrid;

	private StiChartButton buttonMapIdent;

	private StiMapDataColumnsControl columnsControl;

	private StiComboBox comboBoxDisplayNameType;

	private Label labelDisplayNameType;

	private CheckBox checkBoxColorEach;

	private bool AllowGroup
	{
		get
		{
			if (map.MapType != StiMapType.Group)
			{
				return map.MapType == StiMapType.HeatmapWithGroup;
			}
			return true;
		}
	}

	private bool AllowColor
	{
		get
		{
			if (map.MapType == StiMapType.Individual)
			{
				return map.ColorEach;
			}
			return false;
		}
	}

	private StiMapType CurrentMapType
	{
		get
		{
			return ((ListControl)comboBoxMapType).SelectedIndex switch
			{
				0 => StiMapType.Individual, 
				1 => StiMapType.Group, 
				2 => StiMapType.Heatmap, 
				3 => StiMapType.HeatmapWithGroup, 
				_ => StiMapType.Individual, 
			};
		}
		set
		{
			switch (value)
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
	}

	private StiDisplayNameType CurrentDisplayNameType
	{
		get
		{
			return ((ListControl)comboBoxDisplayNameType).SelectedIndex switch
			{
				0 => StiDisplayNameType.None, 
				1 => StiDisplayNameType.Full, 
				2 => StiDisplayNameType.Short, 
				_ => StiDisplayNameType.None, 
			};
		}
		set
		{
			switch (value)
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
	}

	private StiMapSource CurrentDataFrom => ((ListControl)comboBoxDataFrom).SelectedIndex switch
	{
		0 => StiMapSource.DataColumns, 
		1 => StiMapSource.Manual, 
		_ => StiMapSource.DataColumns, 
	};

	public StiMapTabChoroplethControl(StiMapDesignerForm mapDesigner, StiMap map)
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		designerForm = mapDesigner;
		this.map = map;
		InitMaps();
		Localize();
		columnsControl.Init(designerForm, this.map);
		checkBoxShowValue.Checked = this.map.ShowValue;
		checkBoxColorEach.Checked = this.map.ColorEach;
		CurrentMapType = this.map.MapType;
		CurrentDisplayNameType = this.map.DisplayNameType;
		((ComboBox)comboBoxDisplayNameType).SelectedIndexChanged += ComboBoxDisplayNameType_SelectedIndexChanged;
		buttonMapIdent.ImageFixedSize = ((StiScale.Id == Stimulsoft.Base.SystemScaleID.x1) ? new Size(32, 32) : new Size(64, 64));
		if (StiCustomMapFinder.IsCustom(this.map.MapIdent))
		{
			string icon = StiCustomMapFinder.GetIcon(this.map.Report, this.map.MapIdent);
			if (icon != null)
			{
				byte[] buffer = Convert.FromBase64String(icon);
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(buffer)));
			}
			else
			{
				byte[] defaultIcon = StiCustomMapFinder.GetDefaultIcon();
				buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(defaultIcon)));
			}
		}
		else
		{
			buttonMapIdent.Image = GetMapImage(map.MapIdent);
		}
		((ComboBox)comboBoxDataFrom).SelectedIndexChanged += ComboBoxDataFrom_SelectedIndexChanged;
		((ListControl)comboBoxDataFrom).SelectedIndex = ((this.map.DataFrom != StiMapSource.DataColumns) ? 1 : 0);
		((ComboBox)comboBoxLang).SelectedIndexChanged += ComboBoxLanguage_SelectedIndexChanged;
		((UserControl)this).Load += This_ControlLoad;
		((Control)this).SizeChanged += This_SizeChanged;
		((Control)buttonMapIdent).Click += ButtonMapIdent_Click;
		DoScaling();
	}

	private void Localize()
	{
		((Control)buttonMapIdent).Text = GetMapID(map.MapIdent);
		((Control)labelDataFrom).Text = string.Format(StiLocalization.Get("Adapters", "AdapterConnection"), string.Empty).Trim();
		int selectedIndex = ((ListControl)comboBoxDataFrom).SelectedIndex;
		((ComboBox)comboBoxDataFrom).Items.Clear();
		((ComboBox)comboBoxDataFrom).Items.Add((object)StiLocalization.Get("Chart", "DataColumns"));
		((ComboBox)comboBoxDataFrom).Items.Add((object)StiLocalization.Get("PropertyEnum", "FormStartPositionManual"));
		((ListControl)comboBoxDataFrom).SelectedIndex = selectedIndex;
		selectedIndex = ((ListControl)comboBoxDisplayNameType).SelectedIndex;
		((ComboBox)comboBoxDisplayNameType).Items.Clear();
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeNone"));
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeFull"));
		((ComboBox)comboBoxDisplayNameType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDisplayNameTypeShort"));
		((ListControl)comboBoxDisplayNameType).SelectedIndex = selectedIndex;
		((Control)checkBoxShowValue).Text = StiLocalization.Get("PropertyMain", "ShowValue");
		((Control)checkBoxColorEach).Text = StiLocalization.Get("PropertyMain", "ColorEach");
		((Control)labelMapType).Text = StiLocalization.Get("PropertyMain", "MapType");
		((Control)labelDisplayNameType).Text = StiLocalization.Get("PropertyMain", "DisplayNameType");
		((Control)labelLang).Text = StiLocalization.Get("PropertyMain", "Language");
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeIndividual"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeGroup"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeHeatmap"));
		((ComboBox)comboBoxMapType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMapTypeHeatmapWithGroup"));
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

	private void DoScaling()
	{
		dataGrid.RowTemplate.Height = StiScale.I(24);
		dataGrid.ColumnHeadersHeight = StiScale.I(26);
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
		map.MapData = StiJsonHelper.SaveToJsonString(mapData);
	}

	private void UpdateData()
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		dataGrid.DataSource = null;
		mapData = (from x in map.GetMapData()
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
		dataGrid.DataSource = currentDataTable;
		foreach (DataGridViewColumn item in (BaseCollection)dataGrid.Columns)
		{
			DataGridViewColumn val = item;
			((DataGridViewBand)val).Resizable = (DataGridViewTriState)2;
			val.SortMode = (DataGridViewColumnSortMode)0;
			switch (val.Name)
			{
			case "Key":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Key");
				((DataGridViewCell)val.HeaderCell).Style.Alignment = (DataGridViewContentAlignment)32;
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
				val.Width = StiScale.I(80);
				break;
			case "Name":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Name");
				val.Width = StiScale.XXI(130.0);
				val.Width = StiScale.I(130);
				break;
			case "Value":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Value");
				((DataGridViewCell)val.HeaderCell).Style.Alignment = (DataGridViewContentAlignment)32;
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)64;
				val.Width = StiScale.I(80);
				break;
			case "Group":
				val.HeaderText = StiLocalization.Get("PropertyEnum", "StiMapTypeGroup");
				val.Width = StiScale.XXI(80.0);
				break;
			case "Color":
				val.HeaderText = StiLocalization.Get("PropertyMain", "Color");
				val.Width = StiScale.XXI(80.0);
				((DataGridViewCell)val.HeaderCell).Style.Alignment = (DataGridViewContentAlignment)32;
				((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
				break;
			default:
				((DataGridViewCell)val.HeaderCell).Style.Alignment = (DataGridViewContentAlignment)16;
				val.Width = StiScale.I(80);
				break;
			}
		}
	}

	private string GetMapID(string id)
	{
		if (Enum.TryParse<StiMapID>(id, out var result))
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

	private void This_SizeChanged(object sender, EventArgs e)
	{
		((Control)bottomSeparator).Invalidate();
		((Control)panelDataGrid).Invalidate();
	}

	private void This_ControlLoad(object sender, EventArgs e)
	{
		((UserControl)this).Load -= This_ControlLoad;
		UpdateData();
	}

	private void ComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!lockLanguage)
		{
			StiMapLanguage stiMapLanguage = ((ComboBox)comboBoxLang).SelectedItem as StiMapLanguage;
			map.Language = ((stiMapLanguage != null && stiMapLanguage.Language != "EN") ? stiMapLanguage.Language : null);
			UpdateData();
			((Control)designerForm.DesignerControl).Refresh();
		}
	}

	private void ComboBoxDataFrom_SelectedIndexChanged(object sender, EventArgs e)
	{
		map.DataFrom = CurrentDataFrom;
		StiMapToolboxInfo stiMapToolboxInfo = mapsInfos.FirstOrDefault((StiMapToolboxInfo x) => x.MapID == map.MapIdent);
		if (stiMapToolboxInfo != null && stiMapToolboxInfo.Language != null)
		{
			((Control)labelLang).Visible = true;
			((Control)comboBoxLang).Visible = true;
			lockLanguage = true;
			int selectedIndex = 0;
			((ComboBox)comboBoxLang).Items.Clear();
			int num = 0;
			string[] language = stiMapToolboxInfo.Language;
			foreach (string text in language)
			{
				StiMapLanguage stiMapLanguage = new StiMapLanguage(text, stiMapToolboxInfo.GetLangOriginalName(text));
				((ComboBox)comboBoxLang).Items.Add((object)stiMapLanguage);
				if (text == map.Language)
				{
					selectedIndex = num;
				}
				num++;
			}
			((ListControl)comboBoxLang).SelectedIndex = selectedIndex;
			lockLanguage = false;
		}
		else
		{
			((Control)labelLang).Visible = false;
			((Control)comboBoxLang).Visible = false;
		}
		switch (map.DataFrom)
		{
		case StiMapSource.DataColumns:
			((Control)panelDataGrid).Hide();
			((Control)columnsControl).Show();
			((Control)checkBoxColorEach).Visible = CurrentMapType == StiMapType.Individual;
			break;
		case StiMapSource.Manual:
			((Control)columnsControl).Hide();
			((Control)panelDataGrid).Show();
			((Control)panelBottom).Height = StiScale.YYI(175.0);
			((Control)checkBoxColorEach).Visible = CurrentMapType == StiMapType.Individual;
			break;
		}
		int num2 = ((Control)labelDisplayNameType).Location.Y + StiScale.YYI(32.0);
		int num3 = ((Control)comboBoxDisplayNameType).Location.Y + StiScale.YYI(32.0);
		if (((Control)comboBoxLang).Visible)
		{
			((Control)labelLang).Location = new Point(StiScale.XXI(9.0), num2);
			((Control)comboBoxLang).Location = new Point(StiScale.XXI(180.0), num3);
			num2 += StiScale.YYI(32.0);
			num3 += StiScale.YYI(32.0);
		}
		if (((Control)checkBoxShowValue).Visible)
		{
			((Control)checkBoxShowValue).Location = new Point(StiScale.XXI(180.0), num3);
			num3 += StiScale.YYI(28.0);
		}
		if (((Control)checkBoxColorEach).Visible)
		{
			((Control)checkBoxColorEach).Location = new Point(StiScale.XXI(180.0), num3);
			num3 += StiScale.YYI(28.0);
		}
		((Control)panelBottom).Height = num3;
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ComboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (CurrentMapType == StiMapType.Individual)
		{
			map.MapType = StiMapType.Individual;
		}
		else
		{
			map.MapType = CurrentMapType;
		}
		ComboBoxDataFrom_SelectedIndexChanged(null, null);
		columnsControl?.UpdateColumnsState();
		UpdateData();
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ButtonMapIdent_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		StiMapCategoriesForm stiMapCategoriesForm = new StiMapCategoriesForm(map.Report);
		try
		{
			((Form)stiMapCategoriesForm).Owner = (Form)(object)designerForm;
			if ((int)((Form)stiMapCategoriesForm).ShowDialog() != 1 || string.IsNullOrEmpty(stiMapCategoriesForm.MapID))
			{
				return;
			}
			map.MapIdent = stiMapCategoriesForm.MapID;
			map.MapData = null;
			((Control)buttonMapIdent).Text = GetMapID(map.MapIdent);
			if (StiCustomMapFinder.IsCustom(map.MapIdent))
			{
				string icon = StiCustomMapFinder.GetIcon(map.Report, map.MapIdent);
				if (icon != null)
				{
					byte[] buffer = Convert.FromBase64String(icon);
					buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(buffer)));
				}
				else
				{
					byte[] defaultIcon = StiCustomMapFinder.GetDefaultIcon();
					buttonMapIdent.Image = StiImageResizer.GetImage((Image)new Bitmap((Stream)new MemoryStream(defaultIcon)));
				}
			}
			else
			{
				buttonMapIdent.Image = GetMapImage(map.MapIdent);
			}
			ComboBoxDataFrom_SelectedIndexChanged(null, null);
			UpdateData();
			((Control)designerForm.DesignerControl).Refresh();
		}
		finally
		{
			((IDisposable)stiMapCategoriesForm)?.Dispose();
		}
	}

	private void ComboBoxDisplayNameType_SelectedIndexChanged(object sender, EventArgs e)
	{
		map.DisplayNameType = CurrentDisplayNameType;
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void CheckBoxShowValue_CheckedChanged(object sender, EventArgs e)
	{
		map.ShowValue = checkBoxShowValue.Checked;
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void CheckBoxColorEach_CheckedChanged(object sender, EventArgs e)
	{
		map.ColorEach = checkBoxColorEach.Checked;
		UpdateData();
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex != -1 && mapData != null && mapData.Count != 0)
		{
			StiMapData stiMapData = mapData[e.RowIndex];
			DataRow dataRow = currentDataTable.Rows[e.RowIndex];
			DataTable dataTable = (DataTable)dataGrid.DataSource;
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
			map.MapData = StiJsonHelper.SaveToJsonString(mapData);
			((Control)designerForm.DesignerControl).Refresh();
		}
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Expected O, but got Unknown
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		labelDataFrom = new Label();
		comboBoxDataFrom = new StiComboBox();
		labelMapType = new Label();
		labelLang = new Label();
		comboBoxMapType = new StiComboBox();
		checkBoxShowValue = new CheckBox();
		dataGrid = new DataGridView();
		panelDataGrid = new Panel();
		panelTop = new Panel();
		panelBody = new Panel();
		columnsControl = new StiMapDataColumnsControl();
		panelBottom = new Panel();
		checkBoxColorEach = new CheckBox();
		comboBoxDisplayNameType = new StiComboBox();
		comboBoxLang = new StiComboBox();
		labelDisplayNameType = new Label();
		buttonMapIdent = new StiChartButton();
		bottomSeparator = new Panel();
		((ISupportInitialize)dataGrid).BeginInit();
		((Control)panelDataGrid).SuspendLayout();
		((Control)panelTop).SuspendLayout();
		((Control)panelBody).SuspendLayout();
		((Control)panelBottom).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelDataFrom).ForeColor = Color.Black;
		((Control)labelDataFrom).Location = new Point(9, 13);
		((Control)labelDataFrom).Name = "labelDataFrom";
		((Control)labelDataFrom).Size = new Size(120, 13);
		((Control)labelDataFrom).TabIndex = 0;
		((Control)labelDataFrom).Text = "Data From:";
		comboBoxDataFrom.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDataFrom).FormattingEnabled = true;
		((Control)comboBoxDataFrom).Location = new Point(180, 9);
		((Control)comboBoxDataFrom).Name = "comboBoxDataFrom";
		((Control)comboBoxDataFrom).Size = new Size(200, 21);
		((Control)comboBoxDataFrom).TabIndex = 1;
		((Control)labelMapType).ForeColor = Color.Black;
		((Control)labelMapType).Location = new Point(9, 62);
		((Control)labelMapType).Name = "labelMapType";
		((Control)labelMapType).Size = new Size(120, 13);
		((Control)labelMapType).TabIndex = 4;
		((Control)labelMapType).Text = "Map Type:";
		((Control)labelLang).ForeColor = Color.Black;
		((Control)labelLang).Location = new Point(120, 127);
		((Control)labelLang).Name = "labelLang";
		((Control)labelLang).Size = new Size(120, 13);
		((Control)labelLang).TabIndex = 4;
		((Control)labelLang).Text = "Lang:";
		comboBoxMapType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMapType).FormattingEnabled = true;
		((Control)comboBoxMapType).Location = new Point(180, 58);
		((Control)comboBoxMapType).Name = "comboBoxMapType";
		((Control)comboBoxMapType).Size = new Size(200, 21);
		((Control)comboBoxMapType).TabIndex = 1;
		((ComboBox)comboBoxMapType).SelectedIndexChanged += ComboBoxMapType_SelectedIndexChanged;
		((Control)checkBoxShowValue).AutoSize = true;
		((Control)checkBoxShowValue).Location = new Point(180, 156);
		((Control)checkBoxShowValue).Name = "checkBoxShowValue";
		((Control)checkBoxShowValue).Size = new Size(83, 17);
		((Control)checkBoxShowValue).TabIndex = 4;
		((Control)checkBoxShowValue).Text = "Show Value";
		((ButtonBase)checkBoxShowValue).UseVisualStyleBackColor = true;
		checkBoxShowValue.CheckedChanged += CheckBoxShowValue_CheckedChanged;
		dataGrid.AllowUserToAddRows = false;
		dataGrid.AllowUserToDeleteRows = false;
		dataGrid.AllowUserToResizeRows = false;
		((Control)dataGrid).Anchor = (AnchorStyles)15;
		dataGrid.BackgroundColor = Color.White;
		dataGrid.BorderStyle = (BorderStyle)0;
		dataGrid.ColumnHeadersBorderStyle = (DataGridViewHeaderBorderStyle)1;
		val.Alignment = (DataGridViewContentAlignment)32;
		val.BackColor = SystemColors.Control;
		val.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		dataGrid.ColumnHeadersDefaultCellStyle = val;
		dataGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		dataGrid.DefaultCellStyle = val2;
		((Control)dataGrid).Location = new Point(2, 2);
		((Control)dataGrid).Name = "dataGrid";
		dataGrid.RowHeadersVisible = false;
		dataGrid.RowHeadersWidth = 82;
		dataGrid.ShowEditingIcon = false;
		((Control)dataGrid).Size = new Size(672, 275);
		((Control)dataGrid).TabIndex = 0;
		((Control)dataGrid).TabStop = false;
		dataGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGrid_CellValueChanged);
		((Control)panelDataGrid).Anchor = (AnchorStyles)15;
		((Control)panelDataGrid).BackColor = Color.White;
		((Control)panelDataGrid).Controls.Add((Control)(object)dataGrid);
		((Control)panelDataGrid).Location = new Point(12, 0);
		((Control)panelDataGrid).Name = "panelDataGrid";
		((Control)panelDataGrid).Size = new Size(676, 279);
		((Control)panelDataGrid).TabIndex = 1;
		((Control)panelTop).BackColor = Color.White;
		((Control)panelTop).Controls.Add((Control)(object)labelDataFrom);
		((Control)panelTop).Controls.Add((Control)(object)comboBoxDataFrom);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(700, 40);
		((Control)panelTop).TabIndex = 0;
		((Control)panelBody).BackColor = Color.White;
		((Control)panelBody).Controls.Add((Control)(object)panelDataGrid);
		((Control)panelBody).Controls.Add((Control)(object)columnsControl);
		((Control)panelBody).Dock = (DockStyle)5;
		((Control)panelBody).Location = new Point(0, 40);
		((Control)panelBody).Name = "panelBody";
		((Control)panelBody).Size = new Size(700, 285);
		((Control)panelBody).TabIndex = 0;
		((Control)columnsControl).BackColor = Color.White;
		((Control)columnsControl).Dock = (DockStyle)1;
		((Control)columnsControl).Location = new Point(0, 0);
		((Control)columnsControl).Margin = new Padding(6, 6, 6, 6);
		((Control)columnsControl).Name = "columnsControl";
		((Control)columnsControl).Padding = new Padding(9, 0, 9, 0);
		((Control)columnsControl).Size = new Size(700, 285);
		((Control)columnsControl).TabIndex = 0;
		((Control)columnsControl).Visible = false;
		((Control)panelBottom).BackColor = Color.White;
		((Control)panelBottom).Controls.Add((Control)(object)checkBoxColorEach);
		((Control)panelBottom).Controls.Add((Control)(object)comboBoxLang);
		((Control)panelBottom).Controls.Add((Control)(object)labelLang);
		((Control)panelBottom).Controls.Add((Control)(object)comboBoxDisplayNameType);
		((Control)panelBottom).Controls.Add((Control)(object)labelDisplayNameType);
		((Control)panelBottom).Controls.Add((Control)(object)buttonMapIdent);
		((Control)panelBottom).Controls.Add((Control)(object)checkBoxShowValue);
		((Control)panelBottom).Controls.Add((Control)(object)comboBoxMapType);
		((Control)panelBottom).Controls.Add((Control)(object)labelMapType);
		((Control)panelBottom).Controls.Add((Control)(object)bottomSeparator);
		((Control)panelBottom).Dock = (DockStyle)2;
		((Control)panelBottom).Location = new Point(0, 355);
		((Control)panelBottom).Name = "panelBottom";
		((Control)panelBottom).Size = new Size(700, 205);
		((Control)panelBottom).TabIndex = 2;
		((Control)checkBoxColorEach).AutoSize = true;
		((Control)checkBoxColorEach).Location = new Point(180, 181);
		((Control)checkBoxColorEach).Name = "checkBoxColorEach";
		((Control)checkBoxColorEach).Size = new Size(78, 17);
		((Control)checkBoxColorEach).TabIndex = 5;
		((Control)checkBoxColorEach).Text = "Color Each";
		((ButtonBase)checkBoxColorEach).UseVisualStyleBackColor = true;
		checkBoxColorEach.CheckedChanged += CheckBoxColorEach_CheckedChanged;
		comboBoxDisplayNameType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDisplayNameType).FormattingEnabled = true;
		((Control)comboBoxDisplayNameType).Location = new Point(180, 91);
		((Control)comboBoxDisplayNameType).Name = "comboBoxDisplayNameType";
		((Control)comboBoxDisplayNameType).Size = new Size(200, 21);
		((Control)comboBoxDisplayNameType).TabIndex = 2;
		comboBoxLang.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxLang).FormattingEnabled = true;
		((Control)comboBoxLang).Location = new Point(180, 124);
		((Control)comboBoxLang).Name = "comboBoxLang";
		((Control)comboBoxLang).Size = new Size(200, 21);
		((Control)comboBoxLang).TabIndex = 3;
		((Control)labelDisplayNameType).ForeColor = Color.Black;
		((Control)labelDisplayNameType).Location = new Point(9, 95);
		((Control)labelDisplayNameType).Name = "labelDisplayNameType";
		((Control)labelDisplayNameType).Size = new Size(120, 13);
		((Control)labelDisplayNameType).TabIndex = 9;
		((Control)labelDisplayNameType).Text = "Display Name Type:";
		((Control)buttonMapIdent).AccessibleRole = (AccessibleRole)43;
		((Control)buttonMapIdent).Anchor = (AnchorStyles)13;
		buttonMapIdent.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonMapIdent).Location = new Point(12, 8);
		buttonMapIdent.LockClosePopup = false;
		((Control)buttonMapIdent).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonMapIdent).Name = "buttonMapIdent";
		buttonMapIdent.PopupSide = ePopupSide.Right;
		((Control)buttonMapIdent).Size = new Size(676, 38);
		buttonMapIdent.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonMapIdent).TabIndex = 0;
		((Control)buttonMapIdent).TabStop = false;
		((Control)buttonMapIdent).Text = "Map";
		buttonMapIdent.TextAlignment = eButtonTextAlignment.Left;
		((Control)bottomSeparator).BackColor = Color.White;
		((Control)bottomSeparator).Dock = (DockStyle)1;
		((Control)bottomSeparator).Location = new Point(0, 0);
		((Control)bottomSeparator).Name = "bottomSeparator";
		((Control)bottomSeparator).Size = new Size(700, 1);
		((Control)bottomSeparator).TabIndex = 6;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelBody);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Controls.Add((Control)(object)panelBottom);
		((Control)this).Name = "StiMapTabChoroplethControl";
		((Control)this).Size = new Size(700, 560);
		((ISupportInitialize)dataGrid).EndInit();
		((Control)panelDataGrid).ResumeLayout(false);
		((Control)panelTop).ResumeLayout(false);
		((Control)panelBody).ResumeLayout(false);
		((Control)panelBottom).ResumeLayout(false);
		((Control)panelBottom).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
