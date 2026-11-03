using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

[ToolboxItem(false)]
public class StiDocumentSecurityUserControl : UserControl
{
	private IContainer components;

	internal StiComboBox comboBoxEncryptionKeyLength;

	internal StiLabelInfo labelEncryptionKeyLength;

	internal StiLabelInfo labelUserPassword;

	internal StiTextBox textBoxUserPassword;

	internal StiCheckBoxInfo checkBoxAddOrModifyTextAnnotations;

	internal StiCheckBoxInfo checkBoxPrintDocument;

	internal StiCheckBoxInfo checkBoxModifyContents;

	internal StiCheckBoxInfo checkBoxCopyTextAndGraphics;

	internal StiTextBox textBoxOwnerPassword;

	internal StiLabelInfo labelOwnerPassword;

	public StiDocumentSecurityUserControl()
	{
		InitializeComponent();
		LocalizeSuperToolTip();
		if (!string.IsNullOrWhiteSpace(StiOptions.Export.Pdf.Security.DefaultUserPassword))
		{
			((Control)textBoxUserPassword).Text = StiOptions.Export.Pdf.Security.DefaultUserPassword;
		}
		if (!string.IsNullOrWhiteSpace(StiOptions.Export.Pdf.Security.DefaultOwnerPassword))
		{
			((Control)textBoxOwnerPassword).Text = StiOptions.Export.Pdf.Security.DefaultOwnerPassword;
		}
		((Control)comboBoxEncryptionKeyLength).Width = ((Control)textBoxOwnerPassword).Width;
	}

	private void LocalizeSuperToolTip()
	{
		labelUserPassword.SetToolTip(StiLocalization.Get("HelpViewer", "UserPassword"));
		labelOwnerPassword.SetToolTip(StiLocalization.Get("HelpViewer", "OwnerPassword"));
		labelEncryptionKeyLength.SetToolTip(StiLocalization.Get("HelpViewer", "EncryptionKeyLength"));
		checkBoxPrintDocument.SetToolTip(StiLocalization.Get("HelpViewer", "AllowPrintDocument"));
		checkBoxModifyContents.SetToolTip(StiLocalization.Get("HelpViewer", "AllowModifyContents"));
		checkBoxCopyTextAndGraphics.SetToolTip(StiLocalization.Get("HelpViewer", "AllowCopyTextAndGraphics"));
		checkBoxAddOrModifyTextAnnotations.SetToolTip(StiLocalization.Get("HelpViewer", "AllowAddOrModifyTextAnnotations"));
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
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		comboBoxEncryptionKeyLength = new StiComboBox();
		textBoxUserPassword = new StiTextBox();
		textBoxOwnerPassword = new StiTextBox();
		labelEncryptionKeyLength = new StiLabelInfo();
		labelUserPassword = new StiLabelInfo();
		checkBoxAddOrModifyTextAnnotations = new StiCheckBoxInfo();
		checkBoxPrintDocument = new StiCheckBoxInfo();
		checkBoxModifyContents = new StiCheckBoxInfo();
		checkBoxCopyTextAndGraphics = new StiCheckBoxInfo();
		labelOwnerPassword = new StiLabelInfo();
		((Control)this).SuspendLayout();
		((Control)comboBoxEncryptionKeyLength).Anchor = (AnchorStyles)13;
		comboBoxEncryptionKeyLength.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxEncryptionKeyLength).Items.AddRange(new object[5] { "40 bit RC4 (Acrobat 3)", "128 bit RC4 (Acrobat 5)", "128 bit AES (Acrobat 7)", "256 bit AES (Acrobat 9)", "256 bit AES (Acrobat X)" });
		((Control)comboBoxEncryptionKeyLength).Location = new Point(197, 76);
		((Control)comboBoxEncryptionKeyLength).Name = "comboBoxEncryptionKeyLength";
		((Control)comboBoxEncryptionKeyLength).Size = new Size(146, 21);
		((Control)comboBoxEncryptionKeyLength).TabIndex = 2;
		((Control)textBoxUserPassword).Anchor = (AnchorStyles)13;
		((Control)textBoxUserPassword).BackColor = SystemColors.Window;
		((Control)textBoxUserPassword).ForeColor = SystemColors.ControlText;
		((Control)textBoxUserPassword).Location = new Point(197, 12);
		((StiEditBase)textBoxUserPassword).MaxLength = 32;
		((Control)textBoxUserPassword).Name = "textBoxUserPassword";
		textBoxUserPassword.PasswordChar = '*';
		((Control)textBoxUserPassword).Size = new Size(146, 21);
		((Control)textBoxUserPassword).TabIndex = 0;
		((Control)textBoxOwnerPassword).Anchor = (AnchorStyles)13;
		((Control)textBoxOwnerPassword).BackColor = SystemColors.Window;
		((Control)textBoxOwnerPassword).ForeColor = SystemColors.ControlText;
		((Control)textBoxOwnerPassword).Location = new Point(197, 44);
		((StiEditBase)textBoxOwnerPassword).MaxLength = 32;
		((Control)textBoxOwnerPassword).Name = "textBoxOwnerPassword";
		textBoxOwnerPassword.PasswordChar = '*';
		((Control)textBoxOwnerPassword).Size = new Size(146, 21);
		((Control)textBoxOwnerPassword).TabIndex = 1;
		((Control)labelEncryptionKeyLength).ForeColor = Color.DimGray;
		((Control)labelEncryptionKeyLength).Location = new Point(12, 76);
		((Control)labelEncryptionKeyLength).Name = "labelEncryptionKeyLength";
		((Control)labelEncryptionKeyLength).Padding = new Padding(0, 0, 12, 0);
		((Control)labelEncryptionKeyLength).Size = new Size(179, 21);
		((Control)labelEncryptionKeyLength).TabIndex = 14;
		((Control)labelEncryptionKeyLength).Text = "Encryption key length";
		((Label)labelEncryptionKeyLength).TextAlign = (ContentAlignment)16;
		((Control)labelUserPassword).ForeColor = Color.DimGray;
		((Control)labelUserPassword).Location = new Point(12, 12);
		((Control)labelUserPassword).Name = "labelUserPassword";
		((Control)labelUserPassword).Padding = new Padding(0, 0, 12, 0);
		((Control)labelUserPassword).Size = new Size(176, 20);
		((Control)labelUserPassword).TabIndex = 7;
		((Control)labelUserPassword).Text = "User Password";
		((Label)labelUserPassword).TextAlign = (ContentAlignment)16;
		((Control)checkBoxAddOrModifyTextAnnotations).Anchor = (AnchorStyles)13;
		((Control)checkBoxAddOrModifyTextAnnotations).AutoSize = true;
		((CheckBox)checkBoxAddOrModifyTextAnnotations).Checked = true;
		((CheckBox)checkBoxAddOrModifyTextAnnotations).CheckState = (CheckState)1;
		((Control)checkBoxAddOrModifyTextAnnotations).ForeColor = SystemColors.ControlText;
		((Control)checkBoxAddOrModifyTextAnnotations).Location = new Point(15, 201);
		((Control)checkBoxAddOrModifyTextAnnotations).Name = "checkBoxAddOrModifyTextAnnotations";
		((Control)checkBoxAddOrModifyTextAnnotations).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxAddOrModifyTextAnnotations).Size = new Size(186, 17);
		((Control)checkBoxAddOrModifyTextAnnotations).TabIndex = 6;
		((Control)checkBoxAddOrModifyTextAnnotations).Text = "Add or Modify Text Annotations";
		((Control)checkBoxPrintDocument).Anchor = (AnchorStyles)13;
		((Control)checkBoxPrintDocument).AutoSize = true;
		((CheckBox)checkBoxPrintDocument).Checked = true;
		((CheckBox)checkBoxPrintDocument).CheckState = (CheckState)1;
		((Control)checkBoxPrintDocument).ForeColor = SystemColors.ControlText;
		((Control)checkBoxPrintDocument).Location = new Point(15, 113);
		((Control)checkBoxPrintDocument).Name = "checkBoxPrintDocument";
		((Control)checkBoxPrintDocument).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxPrintDocument).Size = new Size(139, 17);
		((Control)checkBoxPrintDocument).TabIndex = 3;
		((Control)checkBoxPrintDocument).Text = "Allow Print Document";
		((Control)checkBoxModifyContents).Anchor = (AnchorStyles)13;
		((Control)checkBoxModifyContents).AutoSize = true;
		((CheckBox)checkBoxModifyContents).Checked = true;
		((CheckBox)checkBoxModifyContents).CheckState = (CheckState)1;
		((Control)checkBoxModifyContents).ForeColor = SystemColors.ControlText;
		((Control)checkBoxModifyContents).Location = new Point(15, 143);
		((Control)checkBoxModifyContents).Name = "checkBoxModifyContents";
		((Control)checkBoxModifyContents).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxModifyContents).Size = new Size(142, 17);
		((Control)checkBoxModifyContents).TabIndex = 4;
		((Control)checkBoxModifyContents).Text = "Allow Modify Contents";
		((Control)checkBoxCopyTextAndGraphics).Anchor = (AnchorStyles)13;
		((Control)checkBoxCopyTextAndGraphics).AutoSize = true;
		((CheckBox)checkBoxCopyTextAndGraphics).Checked = true;
		((CheckBox)checkBoxCopyTextAndGraphics).CheckState = (CheckState)1;
		((Control)checkBoxCopyTextAndGraphics).ForeColor = SystemColors.ControlText;
		((Control)checkBoxCopyTextAndGraphics).Location = new Point(15, 173);
		((Control)checkBoxCopyTextAndGraphics).Name = "checkBoxCopyTextAndGraphics";
		((Control)checkBoxCopyTextAndGraphics).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCopyTextAndGraphics).Size = new Size(180, 17);
		((Control)checkBoxCopyTextAndGraphics).TabIndex = 5;
		((Control)checkBoxCopyTextAndGraphics).Text = "Allow Copy Text and Graphics";
		((Control)labelOwnerPassword).ForeColor = Color.DimGray;
		((Control)labelOwnerPassword).Location = new Point(12, 44);
		((Control)labelOwnerPassword).Name = "labelOwnerPassword";
		((Control)labelOwnerPassword).Padding = new Padding(0, 0, 12, 0);
		((Control)labelOwnerPassword).Size = new Size(176, 20);
		((Control)labelOwnerPassword).TabIndex = 11;
		((Control)labelOwnerPassword).Text = "Owner Password";
		((Label)labelOwnerPassword).TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)comboBoxEncryptionKeyLength);
		((Control)this).Controls.Add((Control)(object)labelEncryptionKeyLength);
		((Control)this).Controls.Add((Control)(object)labelUserPassword);
		((Control)this).Controls.Add((Control)(object)textBoxUserPassword);
		((Control)this).Controls.Add((Control)(object)checkBoxAddOrModifyTextAnnotations);
		((Control)this).Controls.Add((Control)(object)checkBoxPrintDocument);
		((Control)this).Controls.Add((Control)(object)checkBoxModifyContents);
		((Control)this).Controls.Add((Control)(object)checkBoxCopyTextAndGraphics);
		((Control)this).Controls.Add((Control)(object)textBoxOwnerPassword);
		((Control)this).Controls.Add((Control)(object)labelOwnerPassword);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "StiDocumentSecurityUserControl";
		((Control)this).Size = new Size(355, 229);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
