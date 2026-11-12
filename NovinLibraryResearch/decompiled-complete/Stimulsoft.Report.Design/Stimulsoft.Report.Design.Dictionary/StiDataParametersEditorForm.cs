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

public class StiDataParametersEditorForm : StiInplaceToolForm
{
	private StiDataSource dataSource;

	private IContainer components;

	private StiButtonPanelUX stiGroupLine1;

	private StiButtonUX buttonCancel;

	private StiButtonUX buttonOk;

	private StiDataParameterListBox listBoxItems;

	private StiButtonUX buttonNewParameter;

	public override bool AllowResize => false;

	private StiReport Report => dataSource?.Dictionary?.Report;

	private StiDesignerControl Designer => Report?.Designer?.DesignerControl as StiDesignerControl;

	private StiPropertyGrid PropertyGrid => StiPropertiesPanelHelper.GetPropertyGrid(Designer);

	public StiDataParametersEditorForm(StiDataSource dataSource)
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
		if (dataSource.Parameters == null)
		{
			return;
		}
		int num = 1;
		foreach (StiDataParameter parameter in dataSource.Parameters)
		{
			((ListBox)listBoxItems).Items.Add((object)parameter);
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
		((Control)this).Text = Loc.GetMain("Parameters");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonNewParameter).Text = StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew");
		((Control)buttonNewParameter).Width = TextRenderer.MeasureText(((Control)buttonNewParameter).Text, ((Control)buttonNewParameter).Font).Width + StiScaleUI.I8;
		((Control)listBoxItems).Invalidate();
	}

	private void ButtonAddParameter_Click(object sender, EventArgs e)
	{
		StiDataParameter parameter = new StiDataParameter();
		int num = 2;
		while (((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiDataParameter>().Any((StiDataParameter p) => p.Name == parameter.Name))
		{
			parameter.Name = "Parameter" + num++;
		}
		((ListBox)listBoxItems).Items.Add((object)parameter);
		((ListBox)listBoxItems).SelectedItem = parameter;
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
		Designer.UndoRedoSave(Loc.GetMain("Parameters"), onlyCurrentPage: true);
		dataSource.Parameters.Clear();
		dataSource.Parameters.AddRange(((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiDataParameter>().ToArray());
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
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Expected O, but got Unknown
		stiGroupLine1 = new StiButtonPanelUX();
		buttonCancel = new StiButtonUX();
		buttonOk = new StiButtonUX();
		buttonNewParameter = new StiButtonUX();
		listBoxItems = new StiDataParameterListBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 816);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(612, 88);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(438, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(276, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonNewParameter).Anchor = (AnchorStyles)10;
		((Control)buttonNewParameter).Location = new Point(24, 24);
		((Control)buttonNewParameter).Margin = new Padding(6);
		((Control)buttonNewParameter).Name = "buttonAddConstantLine";
		((Control)buttonNewParameter).Size = new Size(226, 46);
		((Control)buttonNewParameter).TabIndex = 0;
		((Control)buttonNewParameter).Text = "Add Constant Line";
		((ButtonBase)buttonNewParameter).UseVisualStyleBackColor = true;
		((Control)buttonNewParameter).Click += ButtonAddParameter_Click;
		((Control)listBoxItems).AllowDrop = true;
		((Control)listBoxItems).Location = new Point(24, 94);
		((Control)listBoxItems).Margin = new Padding(6, 6, 6, 0);
		((Control)listBoxItems).Name = "treeViewItems";
		((Control)listBoxItems).Size = new Size(564, 690);
		((Control)listBoxItems).TabIndex = 1;
		((ListBox)listBoxItems).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(612, 904);
		((Control)this).Controls.Add((Control)(object)buttonNewParameter);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(564, 686);
		((Control)this).Name = "StiDataColumnsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Constants Lines";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
