using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiPromptForm : StiFormUX
{
	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelPassword;

	private StiLabelUX labelUserName;

	private StiTextBox textBoxPassword;

	private StiTextBox textBoxUserName;

	public StiCheckBoxUX checkBoxCacheData;

	public string UserName
	{
		get
		{
			return ((Control)textBoxUserName).Text;
		}
		set
		{
			((Control)textBoxUserName).Text = value;
		}
	}

	public string Password
	{
		get
		{
			return ((Control)textBoxPassword).Text;
		}
		set
		{
			((Control)textBoxPassword).Text = value;
		}
	}

	public bool CacheData
	{
		get
		{
			return ((CheckBox)checkBoxCacheData).Checked;
		}
		set
		{
			((CheckBox)checkBoxCacheData).Checked = value;
		}
	}

	public StiPromptForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "PromptForm");
		((Control)labelUserName).Text = Loc.GetCleaned("Report", "LabelUserName");
		((Control)labelPassword).Text = Loc.GetCleaned("Report", "LabelPassword");
		((Control)textBoxUserName).Text = Loc.GetCleaned("Report", "LabelUserName");
		((Control)textBoxPassword).Text = Loc.GetCleaned("Report", "LabelPassword");
		((Control)checkBoxCacheData).Text = StiLocalization.Get("PromptForm", "CacheLoginData");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
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
		glButtons = new StiButtonPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		labelPassword = new StiLabelUX();
		labelUserName = new StiLabelUX();
		textBoxPassword = new StiTextBox();
		textBoxUserName = new StiTextBox();
		checkBoxCacheData = new StiCheckBoxUX();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 77);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(368, 46);
		((Control)glButtons).TabIndex = 2;
		((Control)checkBoxCacheData).Anchor = (AnchorStyles)13;
		((Control)checkBoxCacheData).Location = new Point(144, 78);
		((Control)checkBoxCacheData).Name = "checkBoxCacheData";
		((Control)checkBoxCacheData).Size = new Size(242, 24);
		((Control)checkBoxCacheData).TabIndex = 7;
		((CheckBox)checkBoxCacheData).Checked = true;
		((Control)checkBoxCacheData).Text = "Cache login data for current session";
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Button)btOk).DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(200, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(281, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)labelPassword).Enabled = false;
		((Control)labelPassword).ForeColor = Color.DimGray;
		((Control)labelPassword).Location = new Point(12, 45);
		((Control)labelPassword).Name = "labelPassword";
		((Control)labelPassword).Size = new Size(126, 20);
		((Control)labelPassword).TabIndex = 4;
		((Control)labelPassword).Text = "Password";
		((Label)labelPassword).TextAlign = (ContentAlignment)16;
		((Control)labelUserName).Enabled = false;
		((Control)labelUserName).ForeColor = Color.DimGray;
		((Control)labelUserName).Location = new Point(12, 12);
		((Control)labelUserName).Name = "labelUserName";
		((Control)labelUserName).Size = new Size(126, 20);
		((Control)labelUserName).TabIndex = 3;
		((Control)labelUserName).Text = "User Name";
		((Label)labelUserName).TextAlign = (ContentAlignment)16;
		((Control)textBoxPassword).Anchor = (AnchorStyles)13;
		((Control)textBoxPassword).BackColor = SystemColors.Window;
		((Control)textBoxPassword).Location = new Point(144, 45);
		((Control)textBoxPassword).Name = "textBoxPassword";
		textBoxPassword.PasswordChar = '*';
		((Control)textBoxPassword).Size = new Size(212, 21);
		((Control)textBoxPassword).TabIndex = 6;
		((Control)textBoxUserName).Anchor = (AnchorStyles)13;
		((Control)textBoxUserName).BackColor = SystemColors.Window;
		((Control)textBoxUserName).Location = new Point(144, 12);
		((Control)textBoxUserName).Name = "textBoxUserName";
		((Control)textBoxUserName).Size = new Size(212, 21);
		((Control)textBoxUserName).TabIndex = 5;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(410, 160);
		((Control)this).Controls.Add((Control)(object)labelPassword);
		((Control)this).Controls.Add((Control)(object)labelUserName);
		((Control)this).Controls.Add((Control)(object)textBoxPassword);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)textBoxUserName);
		((Control)this).Controls.Add((Control)(object)checkBoxCacheData);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPromptForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Enter information to logon  on to the database";
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
