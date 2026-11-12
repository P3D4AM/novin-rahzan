using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataParameterEditForm : StiFormUX
{
	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX lbName;

	private StiLabelUX lbType;

	private ErrorProvider errorProvider;

	private StiExpressionButton tbInsert;

	private StiLabelUX lbValue;

	public StiTextBoxUX tbValue;

	private ImageList ilButton;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btSaveCopy;

	public StiTextBox tbName;

	public StiComboBox cbType;

	public bool CopyModeActivated { get; private set; }

	public StiDataParameterEditForm(StiReport report, bool isEditMode)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			DoScaling();
			tbInsert.Report = report;
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			((Control)btSaveCopy).Visible = isEditMode;
		}
	}

	private void Localize()
	{
		((Control)btSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbType).Text = Loc.GetCleaned("Report", "LabelType");
		((Control)lbValue).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)btSaveCopy);
	}

	private void DoScaling()
	{
		((ButtonBase)tbInsert).Image = StiReportDesignImages.Editor.Function();
		((Control)tbValue).Width = ((Control)tbName).Width;
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)cbType).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)cbType, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Type")));
			flag = false;
			((Control)cbType).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)cbType, "");
		}
		if (((Control)tbName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbName, "");
		}
		if (flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_SaveCopyClick(object sender, EventArgs e)
	{
		CopyModeActivated = true;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_datasources_queries_parameters.htm");
	}

	private void ButtonInsert_Close(object sender, EventArgs e)
	{
		((TextBoxBase)tbValue).SelectedText = tbInsert.ExpressionString;
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataParameterEditForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tbName = new StiTextBox();
		lbName = new StiLabelUX();
		lbType = new StiLabelUX();
		cbType = new StiComboBox();
		errorProvider = new ErrorProvider(components);
		ilButton = new ImageList(components);
		lbValue = new StiLabelUX();
		tbValue = new StiTextBoxUX();
		glButtons = new StiButtonPanelUX();
		btSaveCopy = new StiButtonUX();
		tbInsert = new StiExpressionButton();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(302, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(383, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(128, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(330, 20);
		((Control)tbName).TabIndex = 0;
		((Control)lbName).ForeColor = Color.DimGray;
		((Control)lbName).Location = new Point(12, 12);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(110, 20);
		((Control)lbName).TabIndex = 6;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)lbType).ForeColor = Color.DimGray;
		((Control)lbType).Location = new Point(12, 44);
		((Control)lbType).Name = "lbType";
		((Control)lbType).Size = new Size(110, 21);
		((Control)lbType).TabIndex = 8;
		((Control)lbType).Text = "Type";
		((Label)lbType).TextAlign = (ContentAlignment)16;
		((Control)cbType).Anchor = (AnchorStyles)13;
		((Control)cbType).BackColor = SystemColors.Window;
		cbType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbType).Location = new Point(128, 44);
		((ComboBox)cbType).MaxDropDownItems = 15;
		((Control)cbType).Name = "cbType";
		((Control)cbType).Size = new Size(216, 21);
		((Control)cbType).TabIndex = 2;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		ilButton.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButton.ImageStream");
		ilButton.TransparentColor = Color.Magenta;
		ilButton.Images.SetKeyName(0, "");
		((Control)lbValue).ForeColor = Color.DimGray;
		((Control)lbValue).Location = new Point(12, 102);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(110, 20);
		((Control)lbValue).TabIndex = 15;
		((Control)lbValue).Text = "Expression";
		((Label)lbValue).TextAlign = (ContentAlignment)16;
		((TextBox)tbValue).AcceptsReturn = true;
		((TextBoxBase)tbValue).AcceptsTab = true;
		((Control)tbValue).Anchor = (AnchorStyles)15;
		((Control)tbValue).Location = new Point(128, 103);
		((TextBoxBase)tbValue).Multiline = true;
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(330, 110);
		((Control)tbValue).TabIndex = 5;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btSaveCopy);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 227);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(470, 46);
		((Control)glButtons).TabIndex = 6;
		((Control)btSaveCopy).Anchor = (AnchorStyles)6;
		((Control)btSaveCopy).Location = new Point(12, 12);
		((Control)btSaveCopy).Name = "btSaveCopy";
		((Control)btSaveCopy).Size = new Size(93, 24);
		((Control)btSaveCopy).TabIndex = 0;
		((Control)btSaveCopy).Text = "Save a Copy";
		((ButtonBase)btSaveCopy).UseVisualStyleBackColor = true;
		((Control)btSaveCopy).Visible = false;
		((Control)btSaveCopy).Click += This_SaveCopyClick;
		tbInsert.Draw3DButton = false;
		tbInsert.DropDownMenu = null;
		tbInsert.HotDragDrop = false;
		((ButtonBase)tbInsert).ImageIndex = 0;
		((ButtonBase)tbInsert).ImageList = ilButton;
		((Control)tbInsert).Location = new Point(128, 77);
		((Control)tbInsert).Name = "tbInsert";
		((Control)tbInsert).Size = new Size(32, 22);
		tbInsert.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbInsert).TabIndex = 4;
		((ButtonBase)tbInsert).UseVisualStyleBackColor = false;
		tbInsert.Close += ButtonInsert_Close;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(470, 273);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)tbInsert);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Controls.Add((Control)(object)tbValue);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)cbType);
		((Control)this).Controls.Add((Control)(object)lbType);
		((Control)this).Controls.Add((Control)(object)lbName);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataParameterEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit parameter";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
