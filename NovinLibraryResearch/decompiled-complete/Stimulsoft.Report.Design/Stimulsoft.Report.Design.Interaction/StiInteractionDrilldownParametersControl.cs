using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Interaction;

[ToolboxItem(false)]
public class StiInteractionDrilldownParametersControl : UserControl
{
	private StiInteraction interaction;

	private StiComponent comp;

	private bool locked;

	private IContainer components;

	private Panel panelLeft;

	private Panel panel1;

	private StiTextBox textBoxName;

	private Label labelName;

	internal StiDrillDownParametersListBox listBoxParameters;

	private Panel panelTop;

	private Label labelValue;

	private StiButtonEdit textBoxValue;

	private Button buttonAddParameter;

	internal string NoSelection => "[" + Loc.Get("Report", "NotAssigned") + "]";

	internal bool IsNotEmpty
	{
		get
		{
			if (string.IsNullOrWhiteSpace(((Control)textBoxName).Text) && string.IsNullOrWhiteSpace(((Control)textBoxValue).Text) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter1.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter2.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter3.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter4.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter5.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter6.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter7.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter8.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter9.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter10.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter1.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter2.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter3.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter4.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter5.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter6.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter7.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter8.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter9.Expression.Value))
			{
				return !string.IsNullOrWhiteSpace(interaction.DrillDownParameter10.Expression.Value);
			}
			return true;
		}
	}

	public StiInteractionDrilldownParametersControl(StiComponent comp, StiInteraction interaction)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.comp = comp;
			this.interaction = interaction;
			((Control)textBoxValue).Text = this.interaction.Bookmark.Value;
			Localize();
			DoScaling();
			listBoxParameters.Editor = this;
			((ListBox)listBoxParameters).Items.Clear();
			if (!interaction.DrillDownParameter1.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter1.Clone());
			}
			if (!interaction.DrillDownParameter2.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter2.Clone());
			}
			if (!interaction.DrillDownParameter3.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter3.Clone());
			}
			if (!interaction.DrillDownParameter4.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter4.Clone());
			}
			if (!interaction.DrillDownParameter5.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter5.Clone());
			}
			if (!interaction.DrillDownParameter6.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter6.Clone());
			}
			if (!interaction.DrillDownParameter7.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter7.Clone());
			}
			if (!interaction.DrillDownParameter8.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter8.Clone());
			}
			if (!interaction.DrillDownParameter9.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter9.Clone());
			}
			if (!interaction.DrillDownParameter10.IsDefault)
			{
				((ListBox)listBoxParameters).Items.Add(interaction.DrillDownParameter10.Clone());
			}
			if (((ListBox)listBoxParameters).Items.Count > 0)
			{
				((ListControl)listBoxParameters).SelectedIndex = 0;
			}
			RefreshUI();
		}
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelValue).Text = Loc.GetCleaned("Report", "LabelValue");
		Size size = TextRenderer.MeasureText(((Control)labelName).Text, ((Control)labelName).Font);
		Size size2 = TextRenderer.MeasureText(((Control)labelValue).Text, ((Control)labelValue).Font);
		int width = ((size.Width > size2.Width) ? size.Width : size2.Width) + StiScale.I10;
		((Control)labelName).Width = width;
		((Control)labelValue).Width = width;
		((Control)textBoxName).Left = ((Control)labelName).Width + StiScale.I12;
		((Control)textBoxValue).Left = ((Control)labelValue).Width + StiScale.I12;
		((Control)textBoxName).Width = ((Control)panelTop).Width - StiScale.I12 - ((Control)textBoxName).Left;
		((Control)textBoxValue).Width = ((Control)panelTop).Width - StiScale.I12 - ((Control)textBoxValue).Left;
		((Control)buttonAddParameter).Text = StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew");
	}

	private void DoScaling()
	{
		textBoxValue.Image = StiReportDesignImages.Actions.EditSmall();
	}

	public void Save()
	{
		interaction.ResetParametersToDefaultState();
		int num = 1;
		foreach (StiDrillDownParameter item in ((ListBox)listBoxParameters).Items)
		{
			if (!item.IsDefault)
			{
				switch (num)
				{
				case 1:
					interaction.DrillDownParameter1 = item;
					break;
				case 2:
					interaction.DrillDownParameter2 = item;
					break;
				case 3:
					interaction.DrillDownParameter3 = item;
					break;
				case 4:
					interaction.DrillDownParameter4 = item;
					break;
				case 5:
					interaction.DrillDownParameter5 = item;
					break;
				case 6:
					interaction.DrillDownParameter6 = item;
					break;
				case 7:
					interaction.DrillDownParameter7 = item;
					break;
				case 8:
					interaction.DrillDownParameter8 = item;
					break;
				case 9:
					interaction.DrillDownParameter9 = item;
					break;
				case 10:
					interaction.DrillDownParameter10 = item;
					break;
				}
				num++;
			}
		}
	}

	private void RunExpressionEditor(StiButtonEdit textBox)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		string text = ((Control)textBox).Text;
		DialogResult val = StiTextEditor.EditText(comp.Report, ref text, StiCodeEditorMode.EditorInteraction, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)textBox).Text = text;
		}
		((Control)textBox).Focus();
		textBox.Select(0, 0);
	}

	internal void RefreshUI()
	{
		((Control)buttonAddParameter).Enabled = ((ListBox)listBoxParameters).Items.Count < 10;
		((Control)panelTop).Enabled = ((ListBox)listBoxParameters).Items.Count > 0;
	}

	private void ListBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiDrillDownParameter selectedParameter = listBoxParameters.SelectedParameter;
		if (selectedParameter != null)
		{
			locked = true;
			((Control)textBoxName).Text = selectedParameter.Name;
			((Control)textBoxValue).Text = selectedParameter.Expression.Value;
			locked = false;
		}
	}

	private void TextBoxValue_ButtonClick(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxValue);
	}

	private void ButtonAddParameter_Click(object sender, EventArgs e)
	{
		int num = 1;
		string name = "Name";
		while (true)
		{
			name = ((num == 1) ? "Name" : $"Name{num}");
			if (!((IEnumerable)((ListBox)listBoxParameters).Items).Cast<StiDrillDownParameter>().Any((StiDrillDownParameter p) => p.Name == name))
			{
				break;
			}
			num++;
		}
		((ListBox)listBoxParameters).Items.Add((object)new StiDrillDownParameter(name));
		((ListControl)listBoxParameters).SelectedIndex = ((ListBox)listBoxParameters).Items.Count - 1;
		RefreshUI();
		((Control)textBoxName).Focus();
	}

	private void TextBox_TextChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			StiDrillDownParameter selectedParameter = listBoxParameters.SelectedParameter;
			if (selectedParameter != null)
			{
				selectedParameter.Name = ((Control)textBoxName).Text;
				selectedParameter.Expression.Value = ((Control)textBoxValue).Text;
			}
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		panelLeft = new Panel();
		buttonAddParameter = new Button();
		panel1 = new Panel();
		panelTop = new Panel();
		labelValue = new Label();
		textBoxValue = new StiButtonEdit();
		labelName = new Label();
		textBoxName = new StiTextBox();
		listBoxParameters = new StiDrillDownParametersListBox();
		((Control)panelLeft).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panelTop).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelLeft).Controls.Add((Control)(object)buttonAddParameter);
		((Control)panelLeft).Controls.Add((Control)(object)listBoxParameters);
		((Control)panelLeft).Dock = (DockStyle)3;
		((Control)panelLeft).Location = new Point(0, 0);
		((Control)panelLeft).Name = "panelLeft";
		((Control)panelLeft).Size = new Size(173, 400);
		((Control)panelLeft).TabIndex = 0;
		((Control)buttonAddParameter).Location = new Point(12, 12);
		((Control)buttonAddParameter).Name = "buttonAddParameter";
		((Control)buttonAddParameter).Size = new Size(113, 24);
		((Control)buttonAddParameter).TabIndex = 1;
		((Control)buttonAddParameter).Text = "Add Parameter";
		((ButtonBase)buttonAddParameter).UseVisualStyleBackColor = true;
		((Control)buttonAddParameter).Click += ButtonAddParameter_Click;
		((Control)panel1).Controls.Add((Control)(object)panelTop);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(173, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(427, 400);
		((Control)panel1).TabIndex = 1;
		((Control)panelTop).Controls.Add((Control)(object)labelValue);
		((Control)panelTop).Controls.Add((Control)(object)textBoxValue);
		((Control)panelTop).Controls.Add((Control)(object)labelName);
		((Control)panelTop).Controls.Add((Control)(object)textBoxName);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(427, 118);
		((Control)panelTop).TabIndex = 7;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(0, 81);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(113, 21);
		((Control)labelValue).TabIndex = 5;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		((Control)textBoxValue).AllowDrop = true;
		((Control)textBoxValue).Anchor = (AnchorStyles)13;
		((Control)textBoxValue).BackColor = SystemColors.Window;
		((Control)textBoxValue).Location = new Point(118, 81);
		((Control)textBoxValue).Margin = new Padding(2);
		((Control)textBoxValue).Name = "textBoxValue";
		((Control)textBoxValue).Size = new Size(297, 21);
		((Control)textBoxValue).TabIndex = 4;
		textBoxValue.ButtonClick += TextBoxValue_ButtonClick;
		((Control)textBoxValue).TextChanged += TextBox_TextChanged;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(0, 48);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(113, 21);
		((Control)labelName).TabIndex = 0;
		((Control)labelName).Text = "Name";
		labelName.TextAlign = (ContentAlignment)16;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).BackColor = Color.White;
		((Control)textBoxName).Location = new Point(118, 48);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(297, 21);
		((Control)textBoxName).TabIndex = 1;
		((Control)textBoxName).TextChanged += TextBox_TextChanged;
		((Control)listBoxParameters).AllowDrop = true;
		((Control)listBoxParameters).Anchor = (AnchorStyles)15;
		((ListControl)listBoxParameters).FormattingEnabled = true;
		((Control)listBoxParameters).Location = new Point(12, 48);
		((Control)listBoxParameters).Name = "listBoxParameters";
		((Control)listBoxParameters).Size = new Size(148, 337);
		((Control)listBoxParameters).TabIndex = 0;
		((ListBox)listBoxParameters).SelectedIndexChanged += ListBoxParameters_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)panelLeft);
		((Control)this).Name = "StiInteractionDrilldownParametersControl";
		((Control)this).Size = new Size(600, 400);
		((Control)panelLeft).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)panelTop).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
