using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Map.Design;

[ToolboxItem(false)]
public class StiMapDataColumnsControl : UserControl
{
	private StiMapDesignerForm designerForm;

	private StiMap map;

	private IContainer components;

	private Label labelColumnKey;

	public StiMeterListBox listBoxKey;

	private Label labelColumnName;

	public StiMeterListBox listBoxName;

	private Label labelColumnValue;

	public StiMeterListBox listBoxValue;

	private Label labelColumnGroup;

	public StiMeterListBox listBoxGroup;

	private Label labelColumnColor;

	public StiMeterListBox listBoxColor;

	public StiMapDataColumnsControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)labelColumnKey).Text = StiLocalization.Get("PropertyMain", "Key").Replace(":", "");
		((Control)labelColumnName).Text = StiLocalization.Get("Report", "LabelName").Replace(":", "");
		((Control)labelColumnValue).Text = StiLocalization.Get("Report", "LabelValue").Replace(":", "");
		((Control)labelColumnGroup).Text = StiLocalization.Get("FormTitles", "GroupConditionForm").Replace(":", "");
		((Control)labelColumnColor).Text = StiLocalization.Get("Report", "LabelColor").Replace(":", "");
	}

	private void DoScaling()
	{
	}

	public void Init(StiMapDesignerForm designerForm, StiMap map)
	{
		this.designerForm = designerForm;
		this.map = map;
		UpdateColumnsState();
		listBoxKey.Designer = designerForm.DesignerControl;
		listBoxName.Designer = designerForm.DesignerControl;
		listBoxValue.Designer = designerForm.DesignerControl;
		listBoxGroup.Designer = designerForm.DesignerControl;
		listBoxColor.Designer = designerForm.DesignerControl;
		if (!string.IsNullOrEmpty(this.map.KeyDataColumn))
		{
			listBoxKey.AddItem(new StiMeterItem(this.map.KeyDataColumn));
		}
		if (!string.IsNullOrEmpty(this.map.NameDataColumn))
		{
			listBoxName.AddItem(new StiMeterItem(this.map.NameDataColumn));
		}
		if (!string.IsNullOrEmpty(this.map.ValueDataColumn))
		{
			listBoxValue.AddItem(new StiMeterItem(this.map.ValueDataColumn));
		}
		if (!string.IsNullOrEmpty(this.map.GroupDataColumn))
		{
			listBoxGroup.AddItem(new StiMeterItem(this.map.GroupDataColumn));
		}
		if (!string.IsNullOrEmpty(this.map.ColorDataColumn))
		{
			listBoxColor.AddItem(new StiMeterItem(this.map.ColorDataColumn));
		}
		listBoxKey.ElementsChanged += ListBoxKey_ElementsChanged;
		listBoxName.ElementsChanged += ListBoxName_ElementsChanged;
		listBoxValue.ElementsChanged += ListBoxValue_ElementsChanged;
		listBoxGroup.ElementsChanged += ListBoxGroup_ElementsChanged;
		listBoxColor.ElementsChanged += ListBoxColor_ElementsChanged;
	}

	internal void UpdateColumnsState()
	{
		Label obj = labelColumnColor;
		bool visible = (((Control)listBoxColor).Visible = map.MapType == StiMapType.Individual);
		((Control)obj).Visible = visible;
		Label obj2 = labelColumnGroup;
		visible = (((Control)listBoxGroup).Visible = map.MapType == StiMapType.Group || map.MapType == StiMapType.HeatmapWithGroup);
		((Control)obj2).Visible = visible;
	}

	private void ListBoxKey_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxKey).Items.Count == 0)
		{
			map.KeyDataColumn = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxKey).Items[0] as StiMeterItem;
			map.KeyDataColumn = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ListBoxName_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxName).Items.Count == 0)
		{
			map.NameDataColumn = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxName).Items[0] as StiMeterItem;
			map.NameDataColumn = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ListBoxValue_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxValue).Items.Count == 0)
		{
			map.ValueDataColumn = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxValue).Items[0] as StiMeterItem;
			map.ValueDataColumn = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ListBoxGroup_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxGroup).Items.Count == 0)
		{
			map.GroupDataColumn = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxGroup).Items[0] as StiMeterItem;
			map.GroupDataColumn = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ListBoxColor_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxColor).Items.Count == 0)
		{
			map.ColorDataColumn = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxColor).Items[0] as StiMeterItem;
			map.ColorDataColumn = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		labelColumnKey = new Label();
		labelColumnName = new Label();
		labelColumnValue = new Label();
		labelColumnGroup = new Label();
		labelColumnColor = new Label();
		listBoxColor = new StiMeterListBox();
		listBoxGroup = new StiMeterListBox();
		listBoxValue = new StiMeterListBox();
		listBoxName = new StiMeterListBox();
		listBoxKey = new StiMeterListBox();
		((Control)this).SuspendLayout();
		((Control)labelColumnKey).BackColor = Color.White;
		((Control)labelColumnKey).Dock = (DockStyle)1;
		((Control)labelColumnKey).ForeColor = Color.Black;
		((Control)labelColumnKey).Location = new Point(15, 0);
		((Control)labelColumnKey).Margin = new Padding(4, 0, 4, 0);
		((Control)labelColumnKey).Name = "labelColumnKey";
		((Control)labelColumnKey).Size = new Size(270, 23);
		((Control)labelColumnKey).TabIndex = 0;
		((Control)labelColumnKey).Text = "Key";
		labelColumnKey.TextAlign = (ContentAlignment)16;
		((Control)labelColumnName).BackColor = Color.White;
		((Control)labelColumnName).Dock = (DockStyle)1;
		((Control)labelColumnName).ForeColor = Color.Black;
		((Control)labelColumnName).Location = new Point(15, 63);
		((Control)labelColumnName).Margin = new Padding(4, 0, 4, 0);
		((Control)labelColumnName).Name = "labelColumnName";
		((Control)labelColumnName).Size = new Size(270, 23);
		((Control)labelColumnName).TabIndex = 2;
		((Control)labelColumnName).Text = "Name";
		labelColumnName.TextAlign = (ContentAlignment)16;
		((Control)labelColumnValue).BackColor = Color.White;
		((Control)labelColumnValue).Dock = (DockStyle)1;
		((Control)labelColumnValue).ForeColor = Color.Black;
		((Control)labelColumnValue).Location = new Point(15, 126);
		((Control)labelColumnValue).Margin = new Padding(4, 0, 4, 0);
		((Control)labelColumnValue).Name = "labelColumnValue";
		((Control)labelColumnValue).Size = new Size(270, 23);
		((Control)labelColumnValue).TabIndex = 4;
		((Control)labelColumnValue).Text = "Value";
		labelColumnValue.TextAlign = (ContentAlignment)16;
		((Control)labelColumnGroup).BackColor = Color.White;
		((Control)labelColumnGroup).Dock = (DockStyle)1;
		((Control)labelColumnGroup).ForeColor = Color.Black;
		((Control)labelColumnGroup).Location = new Point(15, 189);
		((Control)labelColumnGroup).Margin = new Padding(4, 0, 4, 0);
		((Control)labelColumnGroup).Name = "labelColumnGroup";
		((Control)labelColumnGroup).Size = new Size(270, 23);
		((Control)labelColumnGroup).TabIndex = 6;
		((Control)labelColumnGroup).Text = "Group";
		labelColumnGroup.TextAlign = (ContentAlignment)16;
		((Control)labelColumnColor).BackColor = Color.White;
		((Control)labelColumnColor).Dock = (DockStyle)1;
		((Control)labelColumnColor).ForeColor = Color.Black;
		((Control)labelColumnColor).Location = new Point(15, 252);
		((Control)labelColumnColor).Margin = new Padding(4, 0, 4, 0);
		((Control)labelColumnColor).Name = "labelColumnColor";
		((Control)labelColumnColor).Size = new Size(270, 23);
		((Control)labelColumnColor).TabIndex = 8;
		((Control)labelColumnColor).Text = "Color";
		labelColumnColor.TextAlign = (ContentAlignment)16;
		((Control)listBoxColor).AllowDrop = true;
		((Control)listBoxColor).Dock = (DockStyle)1;
		((Control)listBoxColor).Location = new Point(15, 275);
		((Control)listBoxColor).Margin = new Padding(4);
		((Control)listBoxColor).Name = "listBoxColor";
		((Control)listBoxColor).Size = new Size(270, 40);
		((Control)listBoxColor).TabIndex = 3;
		((Control)listBoxGroup).AllowDrop = true;
		((Control)listBoxGroup).Dock = (DockStyle)1;
		((Control)listBoxGroup).Location = new Point(15, 212);
		((Control)listBoxGroup).Margin = new Padding(4);
		((Control)listBoxGroup).Name = "listBoxGroup";
		((Control)listBoxGroup).Size = new Size(270, 40);
		((Control)listBoxGroup).TabIndex = 7;
		((Control)listBoxValue).AllowDrop = true;
		((Control)listBoxValue).Dock = (DockStyle)1;
		((Control)listBoxValue).Location = new Point(15, 149);
		((Control)listBoxValue).Margin = new Padding(4);
		((Control)listBoxValue).Name = "listBoxValue";
		((Control)listBoxValue).Size = new Size(270, 40);
		((Control)listBoxValue).TabIndex = 5;
		((Control)listBoxName).AllowDrop = true;
		((Control)listBoxName).Dock = (DockStyle)1;
		((Control)listBoxName).Location = new Point(15, 86);
		((Control)listBoxName).Margin = new Padding(4);
		((Control)listBoxName).Name = "listBoxName";
		((Control)listBoxName).Size = new Size(270, 40);
		((Control)listBoxName).TabIndex = 3;
		((Control)listBoxKey).AllowDrop = true;
		((Control)listBoxKey).Dock = (DockStyle)1;
		((Control)listBoxKey).Location = new Point(15, 23);
		((Control)listBoxKey).Margin = new Padding(4);
		((Control)listBoxKey).Name = "listBoxKey";
		((Control)listBoxKey).Size = new Size(270, 40);
		((Control)listBoxKey).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)listBoxColor);
		((Control)this).Controls.Add((Control)(object)labelColumnColor);
		((Control)this).Controls.Add((Control)(object)listBoxGroup);
		((Control)this).Controls.Add((Control)(object)labelColumnGroup);
		((Control)this).Controls.Add((Control)(object)listBoxValue);
		((Control)this).Controls.Add((Control)(object)labelColumnValue);
		((Control)this).Controls.Add((Control)(object)listBoxName);
		((Control)this).Controls.Add((Control)(object)labelColumnName);
		((Control)this).Controls.Add((Control)(object)listBoxKey);
		((Control)this).Controls.Add((Control)(object)labelColumnKey);
		((Control)this).Name = "StiMapDataColumnsControl";
		((Control)this).Padding = new Padding(15, 0, 15, 0);
		((Control)this).Size = new Size(300, 339);
		((Control)this).ResumeLayout(false);
	}
}
