using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsTotals : UserControl
{
	private DataTable table;

	private DataSet dataSet = new DataSet();

	private TreeView selectedColumns;

	private IContainer components;

	private DataGridView dgTotals;

	public TreeView SelectedColumns
	{
		get
		{
			return selectedColumns;
		}
		set
		{
			selectedColumns = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	public StiDataSource DataSource { get; set; }

	public StiBusinessObject BusinessObject { get; set; }

	public List<string> Totals { get; private set; }

	public StiColumnsTotals()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		InitializeComponent();
		table = new DataTable("Totals");
		table.Columns.Add("ColumnName");
		table.Columns.Add("Function");
		dataSet.Tables.Add(table);
		dgTotals.DataSource = dataSet;
		dgTotals.DataMember = "Totals";
		DoScaling();
		DataGridViewTextBoxColumn val = new DataGridViewTextBoxColumn
		{
			Name = "ColumnName",
			HeaderText = StiLocalization.Get("PropertyMain", "Column"),
			DataPropertyName = "ColumnName",
			ReadOnly = true
		};
		StiAggregateFunctionsDataGridViewComboBoxColumn stiAggregateFunctionsDataGridViewComboBoxColumn = new StiAggregateFunctionsDataGridViewComboBoxColumn();
		((DataGridViewColumn)stiAggregateFunctionsDataGridViewComboBoxColumn).Width = StiScale.XXI(120.0);
		((DataGridViewComboBoxColumn)stiAggregateFunctionsDataGridViewComboBoxColumn).DropDownWidth = StiScale.XXI(120.0);
		((DataGridViewColumn)stiAggregateFunctionsDataGridViewComboBoxColumn).Name = "Function";
		((DataGridViewColumn)stiAggregateFunctionsDataGridViewComboBoxColumn).HeaderText = StiLocalization.Get("PropertyMain", "Function");
		((DataGridViewColumn)stiAggregateFunctionsDataGridViewComboBoxColumn).DataPropertyName = "Function";
		((DataGridViewBand)stiAggregateFunctionsDataGridViewComboBoxColumn).ReadOnly = false;
		StiAggregateFunctionsDataGridViewComboBoxColumn stiAggregateFunctionsDataGridViewComboBoxColumn2 = stiAggregateFunctionsDataGridViewComboBoxColumn;
		dgTotals.Columns.Add((DataGridViewColumn)(object)val);
		dgTotals.Columns.Add((DataGridViewColumn)(object)stiAggregateFunctionsDataGridViewComboBoxColumn2);
		dgTotals.CellFormatting += new DataGridViewCellFormattingEventHandler(DgTotals_CellFormatting);
		dgTotals.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(DgTotals_EditingControlShowing);
	}

	private void DoScaling()
	{
		dgTotals.ColumnHeadersHeight = StiScale.YYI(20.0);
		dgTotals.RowTemplate.Height = StiScale.YYI(20.0);
	}

	private void Build()
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		TreeNodeCollection nodes = SelectedColumns.Nodes;
		if (nodes.Count > 0 && nodes[0].Nodes.Count > 0)
		{
			foreach (TreeNode item in nodes)
			{
				TreeNode val = item;
				if ((DataSource != null && val.Text == DataSource.Name) || (BusinessObject != null && val.Text == BusinessObject.Name))
				{
					nodes = val.Nodes;
					break;
				}
			}
		}
		foreach (TreeNode item2 in nodes)
		{
			TreeNode val2 = item2;
			bool flag = false;
			foreach (DataRow row in table.Rows)
			{
				string text = row["ColumnName"] as string;
				if (text == val2.Text)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				DataRow dataRow2 = table.NewRow();
				dataRow2["ColumnName"] = val2.Text;
				dataRow2["Function"] = StiLocalization.Get("Wizards", "NoFunction");
				table.Rows.Add(dataRow2);
			}
		}
		int num = 0;
		while (num < table.Rows.Count)
		{
			DataRow dataRow3 = table.Rows[num];
			bool flag2 = false;
			foreach (TreeNode item3 in nodes)
			{
				TreeNode val3 = item3;
				string text2 = dataRow3["ColumnName"] as string;
				if (text2 == val3.Text)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				table.Rows.Remove(dataRow3);
			}
			else
			{
				num++;
			}
		}
		CreateTotals();
		AutoWidth();
	}

	private void CreateTotals()
	{
		Totals = new List<string>();
		foreach (DataRow row in table.Rows)
		{
			Totals.Add(row.ItemArray[1] as string);
		}
	}

	private void AutoWidth()
	{
		dgTotals.AutoResizeColumn(0);
	}

	private void DgTotals_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if (e.ColumnIndex == 1)
		{
			DataGridViewComboBoxCell val = (DataGridViewComboBoxCell)dgTotals[e.ColumnIndex, e.RowIndex];
			((DataGridViewCell)val).ReadOnly = false;
		}
	}

	private void DgTotals_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		Control control = e.Control;
		ComboBox val = (ComboBox)(object)((control is ComboBox) ? control : null);
		if (val != null)
		{
			val.SelectedIndexChanged -= Combo_SelectedIndexChanged;
			val.SelectedIndexChanged += Combo_SelectedIndexChanged;
		}
	}

	private void Combo_SelectedIndexChanged(object sender, EventArgs e)
	{
		CreateTotals();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			dataSet.Dispose();
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		dgTotals = new DataGridView();
		((ISupportInitialize)dgTotals).BeginInit();
		((Control)this).SuspendLayout();
		dgTotals.BackgroundColor = SystemColors.Window;
		dgTotals.BorderStyle = (BorderStyle)0;
		dgTotals.DataMember = "";
		dgTotals.AllowUserToResizeRows = false;
		dgTotals.AllowUserToResizeColumns = false;
		dgTotals.AllowUserToOrderColumns = false;
		dgTotals.AllowUserToDeleteRows = false;
		dgTotals.AllowUserToAddRows = false;
		dgTotals.AutoGenerateColumns = false;
		dgTotals.ColumnHeadersHeight = 16;
		dgTotals.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)1;
		((Control)dgTotals).Dock = (DockStyle)5;
		((Control)dgTotals).Location = new Point(0, 0);
		((Control)dgTotals).Name = "dgTotals";
		dgTotals.RowHeadersVisible = false;
		((Control)dgTotals).Size = new Size(344, 304);
		((Control)dgTotals).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)dgTotals);
		((Control)this).Name = "StiColumnsTotals";
		((Control)this).Size = new Size(344, 304);
		((ISupportInitialize)dgTotals).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
