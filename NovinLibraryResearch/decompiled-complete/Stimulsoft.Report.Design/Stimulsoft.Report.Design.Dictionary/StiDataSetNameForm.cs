using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataSetNameForm : StiFormUX
{
	private IContainer components;

	private StiLabelUX lbDataSetName;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiTextBox tbDataSetName;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX glButtons;

	public StiDataSetNameForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
			DoScaling();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
		}
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "DataSetName");
		((Control)lbDataSetName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		if (((Control)tbDataSetName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)tbDataSetName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			((Control)tbDataSetName).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataSetNameForm));
		lbDataSetName = new StiLabelUX();
		tbDataSetName = new StiTextBox();
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		errorProvider = new ErrorProvider(components);
		glButtons = new StiButtonPanelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbDataSetName).ForeColor = Color.DimGray;
		((Control)lbDataSetName).Location = new Point(12, 12);
		((Control)lbDataSetName).Name = "lbDataSetName";
		((Control)lbDataSetName).Size = new Size(134, 20);
		((Control)lbDataSetName).TabIndex = 3;
		((Control)lbDataSetName).Text = "Name";
		((Label)lbDataSetName).TextAlign = (ContentAlignment)16;
		((Control)tbDataSetName).Anchor = (AnchorStyles)13;
		((Control)tbDataSetName).Location = new Point(152, 12);
		((Control)tbDataSetName).Name = "tbDataSetName";
		((Control)tbDataSetName).Size = new Size(238, 20);
		((Control)tbDataSetName).TabIndex = 0;
		((Control)tbDataSetName).Text = "Schema";
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(315, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(234, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 47);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(402, 46);
		((Control)glButtons).TabIndex = 4;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(402, 93);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)lbDataSetName);
		((Control)this).Controls.Add((Control)(object)tbDataSetName);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataSetNameForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDataSetNameForm";
		((Form)this).Closed += ThisClosed;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
