using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataColumnsEditorForm : StiInplaceToolForm
{
	private StiDataSource dataSource;

	private IContainer components;

	private StiButtonPanelUX stiGroupLine1;

	private StiButtonUX buttonCancel;

	private StiButtonUX buttonOk;

	private StiDataColumnListBox listBoxItems;

	private StiButtonUX buttonNewColumn;

	public override bool AllowResize => false;

	private StiReport Report => dataSource?.Dictionary?.Report;

	private StiDesignerControl Designer => Report?.Designer?.DesignerControl as StiDesignerControl;

	private StiPropertyGrid PropertyGrid => StiPropertiesPanelHelper.GetPropertyGrid(Designer);

	public StiDataColumnsEditorForm(StiDataSource dataSource)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.dataSource = dataSource;
			DoScaling();
			Build();
			Localize();
			((Form)this).ShowIcon = false;
			((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		}
	}

	private void Build()
	{
		if (dataSource.Columns == null)
		{
			return;
		}
		int num = 1;
		foreach (StiDataColumn column in dataSource.Columns)
		{
			((ListBox)listBoxItems).Items.Add((object)column);
			num++;
		}
		if (((ListBox)listBoxItems).Items.Count > 0)
		{
			((ListControl)listBoxItems).SelectedIndex = 0;
		}
	}

	private void DoScaling()
	{
		((ListBox)listBoxItems).ItemHeight = StiScale.I(24);
	}

	public override void Localize()
	{
		((Control)this).Text = Loc.GetMain("Columns");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonNewColumn).Text = StiLocalization.Get("FormDictionaryDesigner", "ColumnNew");
		((Control)buttonNewColumn).Width = TextRenderer.MeasureText(((Control)buttonNewColumn).Text, ((Control)buttonNewColumn).Font).Width + StiScaleUI.I8;
		((Control)listBoxItems).Invalidate();
	}

	private void ButtonAddColumn_Click(object sender, EventArgs e)
	{
		StiDataColumn column = new StiDataColumn();
		int num = 2;
		while (((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiDataColumn>().Any((StiDataColumn c) => c.Name == column.Name))
		{
			column.Name = "Column" + num++;
		}
		((ListBox)listBoxItems).Items.Add((object)column);
		((ListBox)listBoxItems).SelectedItem = column;
		((Control)listBoxItems).Focus();
	}

	private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxItems).SelectedItem != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, ((ListBox)listBoxItems).SelectedItem);
		}
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		((Control)listBoxItems).Invalidate();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		base.OnFormClosed(e);
		((PropertyGrid)PropertyGrid).SelectedObject = dataSource;
		((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Designer.UndoRedoSave(Loc.GetMain("Columns"), onlyCurrentPage: true);
		dataSource.Columns.Clear();
		dataSource.Columns.AddRange(((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiDataColumn>().ToArray());
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Expected O, but got Unknown
		stiGroupLine1 = new StiButtonPanelUX();
		buttonCancel = new StiButtonUX();
		buttonOk = new StiButtonUX();
		buttonNewColumn = new StiButtonUX();
		listBoxItems = new StiDataColumnListBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 424);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(306, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(219, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(138, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonNewColumn).Anchor = (AnchorStyles)10;
		((Control)buttonNewColumn).Location = new Point(12, 12);
		((Control)buttonNewColumn).Name = "buttonNewColumn";
		((Control)buttonNewColumn).Size = new Size(113, 24);
		((Control)buttonNewColumn).TabIndex = 0;
		((Control)buttonNewColumn).Text = "New Column";
		((ButtonBase)buttonNewColumn).UseVisualStyleBackColor = true;
		((Control)buttonNewColumn).Click += ButtonAddColumn_Click;
		((Control)listBoxItems).AllowDrop = true;
		((Control)listBoxItems).Location = new Point(12, 49);
		((Control)listBoxItems).Margin = new Padding(3, 3, 3, 0);
		((Control)listBoxItems).Name = "listBoxItems";
		((Control)listBoxItems).Size = new Size(282, 359);
		((Control)listBoxItems).TabIndex = 1;
		((ListBox)listBoxItems).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(306, 470);
		((Control)this).Controls.Add((Control)(object)buttonNewColumn);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(3, 3, 3, 3);
		((Control)this).MinimumSize = new Size(290, 375);
		((Control)this).Name = "StiDataColumnsEditorForm";
		((Control)this).Text = "Columns";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
