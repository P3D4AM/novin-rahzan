using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public class StiPageRenameForm : StiFormUX
{
	private StiPage page;

	private IContainer components;

	private StiButtonUX buttonCancel;

	private StiButtonUX buttonOk;

	public StiTextBox textBoxName;

	private StiLabelUX labelName;

	private StiButtonPanelUX glButtons;

	public StiTextBox textBoxAlias;

	private StiLabelUX labelAlias;

	public StiPageRenameForm(StiPage page)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.page = page;
			((Control)textBoxName).Text = page.Name;
			((Control)textBoxAlias).Text = page.Alias;
			((Form)this).ShowIcon = false;
			Localize();
		}
	}

	private void Localize()
	{
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("Buttons", "Rename");
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		page.Report?.Designer?.DesignerControl?.UndoRedoSave(Loc.Get("Buttons", "Rename"));
		page.Name = ((Control)textBoxName).Text;
		page.Alias = ((Control)textBoxAlias).Text;
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		buttonCancel = new StiButtonUX();
		buttonOk = new StiButtonUX();
		textBoxName = new StiTextBox();
		labelName = new StiLabelUX();
		glButtons = new StiButtonPanelUX();
		textBoxAlias = new StiTextBox();
		labelAlias = new StiLabelUX();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(448, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Button)buttonOk).DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(286, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)textBoxName).Location = new Point(238, 24);
		((Control)textBoxName).Margin = new Padding(6);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(360, 31);
		((Control)textBoxName).TabIndex = 0;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(24, 24);
		((Control)labelName).Margin = new Padding(6, 0, 6, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(202, 31);
		((Control)labelName).TabIndex = 2;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 138);
		((Control)glButtons).Margin = new Padding(6);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(624, 92);
		((Control)glButtons).TabIndex = 4;
		((Control)textBoxAlias).Location = new Point(238, 80);
		((Control)textBoxAlias).Margin = new Padding(6);
		((Control)textBoxAlias).Name = "textBoxAlias";
		((Control)textBoxAlias).Size = new Size(360, 31);
		((Control)textBoxAlias).TabIndex = 1;
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(24, 80);
		((Control)labelAlias).Margin = new Padding(6, 0, 6, 0);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(202, 31);
		((Control)labelAlias).TabIndex = 3;
		((Control)labelAlias).Text = "Name";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(624, 230);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)textBoxAlias);
		((Control)this).Controls.Add((Control)(object)textBoxName);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPageRenameForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiPageRenameForm";
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
