using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design.DataTransformation.Controls;

[ToolboxItem(false)]
public class StiDataTransformReplaceZoneControl : StiDataTransformZoneControl
{
	private IContainer components;

	private Label labelFrom;

	private Label labelTo;

	private Panel panel1;

	public CheckBox checkBoxMatchWholeWord;

	public CheckBox checkBoxMatchCase;

	public ComboBox comboBoxFrom;

	public ComboBox comboBoxTo;

	public StiDataTransformReplaceZoneControl()
	{
		InitializeComponent();
		Localize();
		((Control)panel1).Height = ((Control)comboBoxFrom).Height;
	}

	private void Localize()
	{
		((Control)labelFrom).Text = Loc.Get("Editor", "FindWhat");
		((Control)labelTo).Text = Loc.Get("Editor", "ReplaceWith");
		((Control)checkBoxMatchCase).Text = Loc.Get("Editor", "MatchCase");
		((Control)checkBoxMatchWholeWord).Text = Loc.Get("Editor", "MatchWholeWord");
	}

	private void Label_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		labelFrom = new Label();
		labelTo = new Label();
		panel1 = new Panel();
		checkBoxMatchWholeWord = new CheckBox();
		checkBoxMatchCase = new CheckBox();
		comboBoxFrom = new ComboBox();
		comboBoxTo = new ComboBox();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelFrom).BackColor = Color.Transparent;
		((Control)labelFrom).Location = new Point(7, 8);
		((Control)labelFrom).Name = "labelFrom";
		((Control)labelFrom).Size = new Size(130, 16);
		((Control)labelFrom).TabIndex = 33;
		((Control)labelFrom).Text = "From";
		labelFrom.TextAlign = (ContentAlignment)16;
		((Control)labelFrom).MouseDown += new MouseEventHandler(Label_MouseDown);
		((Control)labelTo).BackColor = Color.Transparent;
		((Control)labelTo).Location = new Point(191, 8);
		((Control)labelTo).Name = "labelTo";
		((Control)labelTo).Size = new Size(130, 16);
		((Control)labelTo).TabIndex = 34;
		((Control)labelTo).Text = "To";
		labelTo.TextAlign = (ContentAlignment)16;
		((Control)labelTo).MouseDown += new MouseEventHandler(Label_MouseDown);
		((Control)panel1).BackColor = Color.Transparent;
		((Control)panel1).Controls.Add((Control)(object)checkBoxMatchWholeWord);
		((Control)panel1).Controls.Add((Control)(object)checkBoxMatchCase);
		((Control)panel1).Location = new Point(375, 28);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(275, 21);
		((Control)panel1).TabIndex = 37;
		((Control)checkBoxMatchWholeWord).AutoSize = true;
		((Control)checkBoxMatchWholeWord).BackColor = Color.Transparent;
		((Control)checkBoxMatchWholeWord).Dock = (DockStyle)3;
		((Control)checkBoxMatchWholeWord).Location = new Point(83, 0);
		((Control)checkBoxMatchWholeWord).Name = "checkBoxMatchWholeWord";
		((Control)checkBoxMatchWholeWord).Size = new Size(119, 21);
		((Control)checkBoxMatchWholeWord).TabIndex = 38;
		((Control)checkBoxMatchWholeWord).Text = "Match Whole Word";
		((ButtonBase)checkBoxMatchWholeWord).UseVisualStyleBackColor = false;
		((Control)checkBoxMatchCase).AutoSize = true;
		((Control)checkBoxMatchCase).BackColor = Color.Transparent;
		((Control)checkBoxMatchCase).Dock = (DockStyle)3;
		((Control)checkBoxMatchCase).Location = new Point(0, 0);
		((Control)checkBoxMatchCase).Name = "checkBoxMatchCase";
		((Control)checkBoxMatchCase).Size = new Size(83, 21);
		((Control)checkBoxMatchCase).TabIndex = 37;
		((Control)checkBoxMatchCase).Text = "Match Case";
		((ButtonBase)checkBoxMatchCase).UseVisualStyleBackColor = false;
		comboBoxFrom.AutoCompleteMode = (AutoCompleteMode)1;
		comboBoxFrom.AutoCompleteSource = (AutoCompleteSource)256;
		((Control)comboBoxFrom).Location = new Point(9, 27);
		comboBoxFrom.MaxDropDownItems = 15;
		((Control)comboBoxFrom).Name = "comboBoxFrom";
		((Control)comboBoxFrom).Size = new Size(178, 21);
		((Control)comboBoxFrom).TabIndex = 38;
		comboBoxTo.AutoCompleteMode = (AutoCompleteMode)1;
		comboBoxTo.AutoCompleteSource = (AutoCompleteSource)256;
		((Control)comboBoxTo).Location = new Point(193, 27);
		comboBoxTo.MaxDropDownItems = 15;
		((Control)comboBoxTo).Name = "comboBoxTo";
		((Control)comboBoxTo).Size = new Size(178, 21);
		((Control)comboBoxTo).TabIndex = 39;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)comboBoxTo);
		((Control)this).Controls.Add((Control)(object)comboBoxFrom);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)labelTo);
		((Control)this).Controls.Add((Control)(object)labelFrom);
		((Control)this).Name = "StiDataTransformReplaceZoneControl";
		((Control)this).Size = new Size(650, 56);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
