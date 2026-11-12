using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiCategoryEditForm : StiFormUX
{
	private bool isVariableMode;

	private string categoryName;

	private bool isEditMode;

	private StiDesignerControl designerControl;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiTextBox tbCategoryName;

	private StiLabelUX lbCategoryName;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btSaveCopy;

	public bool CopyModeActivated { get; private set; }

	public string NewCategoryName => ((Control)tbCategoryName).Text;

	public StiCategoryEditForm(StiDesignerControl designerControl, string categoryName, bool isEditMode, bool isVariableMode = false)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.designerControl = designerControl;
			this.categoryName = categoryName;
			this.isEditMode = isEditMode;
			this.isVariableMode = isVariableMode;
			((Control)tbCategoryName).Text = categoryName;
			Localize();
			DoScaling();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			((Control)btSaveCopy).Visible = isEditMode;
		}
	}

	private void Localize()
	{
		((Control)lbCategoryName).Text = Loc.GetCleaned("Report", "LabelCategory");
		((Control)btSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)btSaveCopy);
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		if (((Control)tbCategoryName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)tbCategoryName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Category")));
			((Control)tbCategoryName).Focus();
			return;
		}
		if (isVariableMode)
		{
			if (isEditMode && categoryName == ((Control)tbCategoryName).Text)
			{
				((Form)this).DialogResult = (DialogResult)1;
				return;
			}
			if (designerControl.Report.Dictionary.Variables.ContainsCategory(((Control)tbCategoryName).Text))
			{
				errorProvider.SetError((Control)(object)tbCategoryName, StiLocalization.Get("A_WebViewer", "CategoryAlreadyExists"));
				((Control)tbCategoryName).Focus();
				return;
			}
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void ButtonSaveCopy_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		CopyModeActivated = true;
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
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCategoryEditForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tbCategoryName = new StiTextBox();
		lbCategoryName = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		glButtons = new StiButtonPanelUX();
		btSaveCopy = new StiButtonUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(186, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += ButtonOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(267, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += ButtonCancel_Click;
		((Control)tbCategoryName).Anchor = (AnchorStyles)13;
		((Control)tbCategoryName).Location = new Point(126, 12);
		((Control)tbCategoryName).Name = "tbCategoryName";
		((Control)tbCategoryName).Size = new Size(216, 20);
		((Control)tbCategoryName).TabIndex = 0;
		((Control)lbCategoryName).ForeColor = Color.DimGray;
		((Control)lbCategoryName).Location = new Point(12, 12);
		((Control)lbCategoryName).Name = "lbCategoryName";
		((Control)lbCategoryName).Size = new Size(108, 22);
		((Control)lbCategoryName).TabIndex = 1;
		((Control)lbCategoryName).Text = "Name";
		((Label)lbCategoryName).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btSaveCopy);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 47);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(354, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)btSaveCopy).Anchor = (AnchorStyles)6;
		((Control)btSaveCopy).Location = new Point(12, 12);
		((Control)btSaveCopy).Name = "btSaveCopy";
		((Control)btSaveCopy).Size = new Size(93, 24);
		((Control)btSaveCopy).TabIndex = 0;
		((Control)btSaveCopy).Text = "Save a Copy";
		((ButtonBase)btSaveCopy).UseVisualStyleBackColor = true;
		((Control)btSaveCopy).Visible = false;
		((Control)btSaveCopy).Click += ButtonSaveCopy_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(354, 93);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)lbCategoryName);
		((Control)this).Controls.Add((Control)(object)tbCategoryName);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCategoryEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit category";
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
