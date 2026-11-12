using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Dashboard.Design.Editors.RegionMap;

[ToolboxItem(false)]
public class StiRegionMapDataColumnsControl : UserControl
{
	private StiRegionMapElementEditorForm editorForm;

	private StiRegionMapElement mapElement;

	private bool lockRefresh;

	private IContainer components;

	public StiMeterListBox listBoxColor;

	private StiChartGroupLabel labelColumnColor;

	public StiMeterListBox listBoxGroup;

	private StiChartGroupLabel labelColumnGroup;

	public StiMeterListBox listBoxValue;

	private StiChartGroupLabel labelColumnValue;

	public StiMeterListBox listBoxName;

	private StiChartGroupLabel labelColumnName;

	public StiMeterListBox listBoxKey;

	private StiChartGroupLabel labelColumnKey;

	public StiRegionMapDataColumnsControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			AttachEvents();
			Localize();
		}
	}

	public void Localize()
	{
		((Control)labelColumnKey).Text = StiLocalization.Get("PropertyMain", "Key");
		((Control)labelColumnName).Text = StiLocalization.Get("Report", "LabelName").Replace(":", string.Empty);
		((Control)labelColumnValue).Text = StiLocalization.Get("Report", "LabelValue").Replace(":", string.Empty);
		((Control)labelColumnGroup).Text = StiLocalization.Get("FormTitles", "GroupConditionForm");
		((Control)labelColumnColor).Text = StiLocalization.Get("Report", "LabelColor").Replace(":", string.Empty);
	}

	internal void CalculateControlHeight()
	{
		if (((Control)this).Visible)
		{
			((Control)this).Height = (from Control c in (IEnumerable)((Control)this).Controls
				where c.Visible
				select c.Bottom).Max() + 4;
		}
	}

	protected void AttachEvents()
	{
		AttachEventsToKeyDataColumn();
		AttachEventsToNameDataColumn();
		AttachEventsToValueDataColumn();
		AttachEventsToGroupDataColumn();
		AttachEventsToColorDataColumn();
	}

	private void AttachEventsToKeyDataColumn()
	{
		listBoxKey.ElementsConverting += ListBox_ElementsConverting;
		listBoxKey.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			StiElementChangedProcessor.ProcessElementChanging(mapElement, changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			editorForm.CalculateWindowSize();
		};
		listBoxKey.ElementsDragEnter += delegate
		{
			editorForm.ResetComponentHighlights();
		};
		listBoxKey.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(editorForm.Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(editorForm.Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxKey).Items.Clear();
				listBoxKey.AddItem(StiMeterHelper.RegionMap.GetKey(draggedDataColumnOrVariable));
				editorForm.Designer.PageView.ResetDragging();
				mapElement.Select();
				WritePropertiesToElement();
				listBoxKey.SelectedMeterItem = listBoxKey.LastItem();
				((Control)listBoxKey).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(editorForm.Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(editorForm.Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxKey).Items.Clear();
				listBoxKey.AddItem(StiMeterHelper.RegionMap.GetKey(stiTableColumn));
				editorForm.Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxKey.SelectedMeterItem = listBoxKey.LastItem();
				((Control)listBoxKey).Focus();
			}
			editorForm.CalculateWindowSize();
			editorForm.ResetComponentHighlights();
		};
		listBoxKey.ElementsSelectedChanged += delegate
		{
			mapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToNameDataColumn()
	{
		listBoxName.ElementsConverting += ListBox_ElementsConverting;
		listBoxName.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			StiElementChangedProcessor.ProcessElementChanging(mapElement, changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			editorForm.CalculateWindowSize();
		};
		listBoxName.ElementsDragEnter += delegate
		{
			editorForm.ResetComponentHighlights();
		};
		listBoxName.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(editorForm.Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(editorForm.Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxName).Items.Clear();
				listBoxName.AddItem(StiMeterHelper.RegionMap.GetName(draggedDataColumnOrVariable));
				editorForm.Designer.PageView.ResetDragging();
				mapElement.Select();
				WritePropertiesToElement();
				listBoxName.SelectedMeterItem = listBoxName.LastItem();
				((Control)listBoxName).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(editorForm.Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(editorForm.Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxName).Items.Clear();
				listBoxName.AddItem(StiMeterHelper.RegionMap.GetName(stiTableColumn));
				editorForm.Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxName.SelectedMeterItem = listBoxName.LastItem();
				((Control)listBoxName).Focus();
			}
			editorForm.ResetComponentHighlights();
			editorForm.CalculateWindowSize();
		};
		listBoxName.ElementsSelectedChanged += delegate
		{
			mapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToValueDataColumn()
	{
		listBoxValue.ElementsConverting += ListBox_ElementsConverting;
		listBoxValue.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			StiElementChangedProcessor.ProcessElementChanging(mapElement, changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			editorForm.CalculateWindowSize();
		};
		listBoxValue.ElementsDragEnter += delegate
		{
			editorForm.ResetComponentHighlights();
		};
		listBoxValue.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(editorForm.Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(editorForm.Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxValue).Items.Clear();
				listBoxValue.AddItem(StiMeterHelper.RegionMap.GetValue(draggedDataColumnOrVariable));
				editorForm.Designer.PageView.ResetDragging();
				mapElement.Select();
				WritePropertiesToElement();
				listBoxValue.SelectedMeterItem = listBoxValue.LastItem();
				((Control)listBoxValue).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(editorForm.Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(editorForm.Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxValue).Items.Clear();
				listBoxValue.AddItem(StiMeterHelper.RegionMap.GetValue(stiTableColumn, stiTableElement.Dashboard));
				editorForm.Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxValue.SelectedMeterItem = listBoxValue.LastItem();
				((Control)listBoxValue).Focus();
			}
			editorForm.ResetComponentHighlights();
			editorForm.CalculateWindowSize();
		};
		listBoxValue.ElementsSelectedChanged += delegate
		{
			mapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToGroupDataColumn()
	{
		listBoxGroup.ElementsConverting += ListBox_ElementsConverting;
		listBoxGroup.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			StiElementChangedProcessor.ProcessElementChanging(mapElement, changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			editorForm.CalculateWindowSize();
		};
		listBoxGroup.ElementsDragEnter += delegate
		{
			editorForm.ResetComponentHighlights();
		};
		listBoxGroup.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(editorForm.Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(editorForm.Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxGroup).Items.Clear();
				listBoxGroup.AddItem(StiMeterHelper.RegionMap.GetGroup(draggedDataColumnOrVariable));
				editorForm.Designer.PageView.ResetDragging();
				mapElement.Select();
				WritePropertiesToElement();
				listBoxGroup.SelectedMeterItem = listBoxGroup.LastItem();
				((Control)listBoxGroup).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(editorForm.Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(editorForm.Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxGroup).Items.Clear();
				listBoxGroup.AddItem(StiMeterHelper.RegionMap.GetGroup(stiTableColumn));
				editorForm.Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxGroup.SelectedMeterItem = listBoxGroup.LastItem();
				((Control)listBoxGroup).Focus();
			}
			editorForm.ResetComponentHighlights();
			editorForm.CalculateWindowSize();
		};
		listBoxGroup.ElementsSelectedChanged += delegate
		{
			mapElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToColorDataColumn()
	{
		listBoxColor.ElementsConverting += ListBox_ElementsConverting;
		listBoxColor.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			StiElementChangedProcessor.ProcessElementChanging(mapElement, changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			editorForm.CalculateWindowSize();
		};
		listBoxColor.ElementsDragEnter += delegate
		{
			editorForm.ResetComponentHighlights();
		};
		listBoxColor.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(editorForm.Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(editorForm.Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxColor).Items.Clear();
				listBoxColor.AddItem(StiMeterHelper.RegionMap.GetColor(draggedDataColumnOrVariable));
				editorForm.Designer.PageView.ResetDragging();
				mapElement.Select();
				WritePropertiesToElement();
				listBoxColor.SelectedMeterItem = listBoxColor.LastItem();
				((Control)listBoxColor).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(editorForm.Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(editorForm.Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxColor).Items.Clear();
				listBoxColor.AddItem(StiMeterHelper.RegionMap.GetColor(stiTableColumn));
				editorForm.Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxColor.SelectedMeterItem = listBoxColor.LastItem();
				((Control)listBoxColor).Focus();
			}
			editorForm.ResetComponentHighlights();
			editorForm.CalculateWindowSize();
		};
		listBoxColor.ElementsSelectedChanged += delegate
		{
			mapElement.Select();
			RefreshUiPanel();
		};
	}

	public void WritePropertiesToElement()
	{
		WriteKeyDataColumnPropertiesToElement();
		WriteNameDataColumnPropertiesToElement();
		WriteValueDataColumnPropertiesToElement();
		WriteGroupDataColumnPropertiesToElement();
		WriteColorDataColumnPropertiesToElement();
		editorForm.RefreshElementView();
	}

	private void WriteKeyDataColumnPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxKey.FirstItem();
		mapElement.KeyMeter = stiMeterItem?.Meter as StiKeyMapMeter;
	}

	private void WriteNameDataColumnPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxName.FirstItem();
		mapElement.NameMeter = stiMeterItem?.Meter as StiNameMapMeter;
	}

	private void WriteValueDataColumnPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxValue.FirstItem();
		mapElement.ValueMeter = stiMeterItem?.Meter as StiValueMapMeter;
	}

	private void WriteGroupDataColumnPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxGroup.FirstItem();
		mapElement.GroupMeter = stiMeterItem?.Meter as StiGroupMapMeter;
	}

	private void WriteColorDataColumnPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxColor.FirstItem();
		mapElement.ColorMeter = stiMeterItem?.Meter as StiColorMapMeter;
	}

	public void ReadPropertiesFromElement()
	{
		ReadKeyDataColumnPropertiesFromElement();
		ReadNameDataColumnPropertiesFromElement();
		ReadValueDataColumnPropertiesFromElement();
		ReadGroupDataColumnPropertiesFromElement();
		ReadColorDataColumnPropertiesFromElement();
	}

	private void ReadKeyDataColumnPropertiesFromElement()
	{
		((ListBox)listBoxKey).BeginUpdate();
		((ListBox)listBoxKey).Items.Clear();
		listBoxKey.AddItem(editorForm.RegionControl.MapKeyDataColumn);
		listBoxKey.SelectFirstItem();
		((ListBox)listBoxKey).EndUpdate();
	}

	private void ReadNameDataColumnPropertiesFromElement()
	{
		((ListBox)listBoxName).BeginUpdate();
		((ListBox)listBoxName).Items.Clear();
		listBoxName.AddItem(editorForm.RegionControl.MapNameDataColumn);
		listBoxName.SelectFirstItem();
		((ListBox)listBoxName).EndUpdate();
	}

	private void ReadValueDataColumnPropertiesFromElement()
	{
		((ListBox)listBoxValue).BeginUpdate();
		((ListBox)listBoxValue).Items.Clear();
		listBoxValue.AddItem(editorForm.RegionControl.MapValueDataColumn);
		listBoxValue.SelectFirstItem();
		((ListBox)listBoxValue).EndUpdate();
	}

	private void ReadGroupDataColumnPropertiesFromElement()
	{
		((ListBox)listBoxGroup).BeginUpdate();
		((ListBox)listBoxGroup).Items.Clear();
		listBoxGroup.AddItem(editorForm.RegionControl.MapGroupDataColumn);
		listBoxGroup.SelectFirstItem();
		((ListBox)listBoxGroup).EndUpdate();
	}

	private void ReadColorDataColumnPropertiesFromElement()
	{
		((ListBox)listBoxColor).BeginUpdate();
		((ListBox)listBoxColor).Items.Clear();
		listBoxColor.AddItem(editorForm.RegionControl.MapColorDataColumn);
		listBoxColor.SelectFirstItem();
		((ListBox)listBoxColor).EndUpdate();
	}

	public void Init(StiRegionMapElementEditorForm designerForm, StiRegionMapElement map)
	{
		editorForm = designerForm;
		mapElement = map;
		UpdateColumnsState();
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = designerForm.Designer;
			item.Editor = designerForm;
			item.AllowMultipleElements = false;
			item.AllowImage = false;
		}
		ReadPropertiesFromElement();
	}

	internal void UpdateColumnsState()
	{
		if (mapElement != null)
		{
			StiChartGroupLabel stiChartGroupLabel = labelColumnColor;
			bool visible = (((Control)listBoxColor).Visible = mapElement.MapType == StiMapType.Individual);
			((Control)stiChartGroupLabel).Visible = visible;
			StiChartGroupLabel stiChartGroupLabel2 = labelColumnGroup;
			visible = (((Control)listBoxGroup).Visible = mapElement.MapType == StiMapType.Group || mapElement.MapType == StiMapType.HeatmapWithGroup);
			((Control)stiChartGroupLabel2).Visible = visible;
		}
	}

	public void RefreshUiPanel()
	{
		if (!lockRefresh)
		{
			lockRefresh = true;
			((Control)this).SuspendLayout();
			editorForm.RegionControl.RefreshUiPanel();
			SetListHeightFromCount(listBoxKey);
			SetListHeightFromCount(listBoxName);
			SetListHeightFromCount(listBoxValue);
			SetListHeightFromCount(listBoxColor);
			SetListHeightFromCount(listBoxGroup);
			((Control)this).ResumeLayout();
			lockRefresh = false;
		}
	}

	private void SetListHeightFromCount(StiMeterListBox list)
	{
		((Control)list).Height = Math.Max(((ListBox)list).Items.Count, 1) * ((ListBox)list).ItemHeight + (int)StiScale.Y;
	}

	internal void RefreshExpressionHint()
	{
		StiRegionMapDataControl regionControl = editorForm.RegionControl;
		regionControl.textBoxExpression.Hint = ((regionControl.SelectedListBox != null) ? Loc.GetMain("Field") : "");
	}

	internal void RefreshExpression()
	{
		StiRegionMapDataControl regionControl = editorForm.RegionControl;
		regionControl.LockRefresh = true;
		if (regionControl.SelectedListBox == listBoxKey && regionControl.MapKeyDataColumn != null)
		{
			((Control)regionControl.textBoxExpression).Text = regionControl.MapKeyDataColumn.Expression;
		}
		else if (regionControl.SelectedListBox == listBoxName && regionControl.MapNameDataColumn != null)
		{
			((Control)regionControl.textBoxExpression).Text = regionControl.MapNameDataColumn.Expression;
		}
		else if (regionControl.SelectedListBox == listBoxValue && regionControl.MapValueDataColumn != null)
		{
			((Control)regionControl.textBoxExpression).Text = regionControl.MapValueDataColumn.Expression;
		}
		else if (regionControl.SelectedListBox == listBoxGroup && regionControl.MapGroupDataColumn != null)
		{
			((Control)regionControl.textBoxExpression).Text = regionControl.MapGroupDataColumn.Expression;
		}
		else if (regionControl.SelectedListBox == listBoxColor && regionControl.MapColorDataColumn != null)
		{
			((Control)regionControl.textBoxExpression).Text = regionControl.MapColorDataColumn.Expression;
		}
		else
		{
			((Control)regionControl.textBoxExpression).Text = string.Empty;
		}
		regionControl.LockRefresh = false;
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		StiMeterItem stiMeterItem = sender as StiMeterItem;
		if (stiMeterItem.List == listBoxKey)
		{
			stiMeterItem.Meter = StiMeterHelper.RegionMap.GetKey(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxName)
		{
			stiMeterItem.Meter = StiMeterHelper.RegionMap.GetName(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxValue)
		{
			stiMeterItem.Meter = StiMeterHelper.RegionMap.GetValue(stiMeterItem.Meter, mapElement.Dashboard);
		}
		if (stiMeterItem.List == listBoxGroup)
		{
			stiMeterItem.Meter = StiMeterHelper.RegionMap.GetGroup(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxColor)
		{
			stiMeterItem.Meter = StiMeterHelper.RegionMap.GetColor(stiMeterItem.Meter);
		}
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		editorForm.RegionControl.SelectedListBox = sender as StiMeterListBox;
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			if (item != sender)
			{
				item.SelectedMeterItem = null;
			}
		}
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		editorForm.RegionControl.SelectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		editorForm.RegionControl.SelectedListBox = sender as StiMeterListBox;
		RefreshExpression();
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
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		listBoxColor = new StiMeterListBox();
		labelColumnColor = new StiChartGroupLabel();
		listBoxGroup = new StiMeterListBox();
		labelColumnGroup = new StiChartGroupLabel();
		listBoxValue = new StiMeterListBox();
		labelColumnValue = new StiChartGroupLabel();
		listBoxName = new StiMeterListBox();
		labelColumnName = new StiChartGroupLabel();
		listBoxKey = new StiMeterListBox();
		labelColumnKey = new StiChartGroupLabel();
		((Control)this).SuspendLayout();
		((Control)listBoxColor).AllowDrop = true;
		((Control)listBoxColor).Dock = (DockStyle)1;
		((Control)listBoxColor).Location = new Point(0, 224);
		((Control)listBoxColor).Margin = new Padding(2);
		((Control)listBoxColor).Name = "listBoxColor";
		((Control)listBoxColor).Size = new Size(500, 32);
		((Control)listBoxColor).TabIndex = 9;
		((ListBox)listBoxColor).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxColor).Enter += ListBox_Enter;
		((Control)listBoxColor).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumnColor).Dock = (DockStyle)1;
		((Control)labelColumnColor).Location = new Point(0, 192);
		((Control)labelColumnColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumnColor).Name = "labelColumnColor";
		((Control)labelColumnColor).Size = new Size(500, 32);
		((Control)labelColumnColor).TabIndex = 8;
		((Control)labelColumnColor).Text = "Color";
		((Control)listBoxGroup).AllowDrop = true;
		((Control)listBoxGroup).Dock = (DockStyle)1;
		((Control)listBoxGroup).Location = new Point(0, 494);
		((Control)listBoxGroup).Margin = new Padding(2);
		((Control)listBoxGroup).Name = "listBoxGroup";
		((Control)listBoxGroup).Size = new Size(500, 32);
		((Control)listBoxGroup).TabIndex = 7;
		((ListBox)listBoxGroup).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxGroup).Enter += ListBox_Enter;
		((Control)listBoxGroup).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumnGroup).Dock = (DockStyle)1;
		((Control)labelColumnGroup).Location = new Point(0, 462);
		((Control)labelColumnGroup).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumnGroup).Name = "labelColumnGroup";
		((Control)labelColumnGroup).Size = new Size(500, 32);
		((Control)labelColumnGroup).TabIndex = 6;
		((Control)labelColumnGroup).Text = "Group";
		((Control)listBoxValue).AllowDrop = true;
		((Control)listBoxValue).Dock = (DockStyle)1;
		((Control)listBoxValue).Location = new Point(0, 160);
		((Control)listBoxValue).Margin = new Padding(2);
		((Control)listBoxValue).Name = "listBoxValue";
		((Control)listBoxValue).Size = new Size(500, 32);
		((Control)listBoxValue).TabIndex = 5;
		((ListBox)listBoxValue).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValue).Enter += ListBox_Enter;
		((Control)listBoxValue).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumnValue).Dock = (DockStyle)1;
		((Control)labelColumnValue).Location = new Point(0, 128);
		((Control)labelColumnValue).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumnValue).Name = "labelColumnValue";
		((Control)labelColumnValue).Size = new Size(500, 32);
		((Control)labelColumnValue).TabIndex = 4;
		((Control)labelColumnValue).Text = "Value";
		((Control)listBoxName).AllowDrop = true;
		((Control)listBoxName).Dock = (DockStyle)1;
		((Control)listBoxName).Location = new Point(0, 96);
		((Control)listBoxName).Margin = new Padding(2);
		((Control)listBoxName).Name = "listBoxName";
		((Control)listBoxName).Size = new Size(500, 32);
		((Control)listBoxName).TabIndex = 3;
		((ListBox)listBoxName).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxName).Enter += ListBox_Enter;
		((Control)listBoxName).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumnName).Dock = (DockStyle)1;
		((Control)labelColumnName).Location = new Point(0, 64);
		((Control)labelColumnName).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumnName).Name = "labelColumnName";
		((Control)labelColumnName).Size = new Size(500, 32);
		((Control)labelColumnName).TabIndex = 2;
		((Control)labelColumnName).Text = "Name";
		((Control)listBoxKey).AllowDrop = true;
		((Control)listBoxKey).BackColor = Color.White;
		((Control)listBoxKey).Dock = (DockStyle)1;
		((Control)listBoxKey).Location = new Point(0, 32);
		((Control)listBoxKey).Margin = new Padding(2);
		((Control)listBoxKey).Name = "listBoxKey";
		((Control)listBoxKey).Size = new Size(500, 32);
		((Control)listBoxKey).TabIndex = 1;
		((ListBox)listBoxKey).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxKey).Enter += ListBox_Enter;
		((Control)listBoxKey).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumnKey).Dock = (DockStyle)1;
		((Control)labelColumnKey).Location = new Point(0, 0);
		((Control)labelColumnKey).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumnKey).Name = "labelColumnKey";
		((Control)labelColumnKey).Size = new Size(500, 32);
		((Control)labelColumnKey).TabIndex = 0;
		((Control)labelColumnKey).Text = "Key";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)listBoxGroup);
		((Control)this).Controls.Add((Control)(object)labelColumnGroup);
		((Control)this).Controls.Add((Control)(object)listBoxColor);
		((Control)this).Controls.Add((Control)(object)labelColumnColor);
		((Control)this).Controls.Add((Control)(object)listBoxValue);
		((Control)this).Controls.Add((Control)(object)labelColumnValue);
		((Control)this).Controls.Add((Control)(object)listBoxName);
		((Control)this).Controls.Add((Control)(object)labelColumnName);
		((Control)this).Controls.Add((Control)(object)listBoxKey);
		((Control)this).Controls.Add((Control)(object)labelColumnKey);
		((Control)this).Margin = new Padding(2);
		((Control)this).Name = "StiRegionMapDataColumnsControl";
		((Control)this).Size = new Size(500, 526);
		((Control)this).ResumeLayout(false);
	}
}
