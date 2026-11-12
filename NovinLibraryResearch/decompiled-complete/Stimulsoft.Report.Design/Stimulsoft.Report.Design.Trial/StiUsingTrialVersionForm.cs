using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design.Trial;

internal class StiUsingTrialVersionForm : Form
{
	private int days;

	private IContainer components;

	private PictureBox imageWarning;

	private Panel panel1;

	private Label textBlockTitle;

	private Panel panel2;

	private Button buttonOK;

	public StiUsingTrialVersionForm(int days)
	{
		InitializeComponent();
		this.days = days;
		((Form)this).Icon = StiAppIcons.GetIcon();
		Localize();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormDesigner", "title");
		((Control)buttonOK).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", "");
		((Control)textBlockTitle).Text = ((days > 120) ? StiLocalization.Get("Notices", "YourTrialHasExpired") : StiLocalization.Get("Notices", "YouUsingTrialVersion"));
	}

	private void ButtonContinue_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void Form_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void Form_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (days > 120)
		{
			Application.Exit();
		}
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiUsingTrialVersionForm));
		imageWarning = new PictureBox();
		panel1 = new Panel();
		textBlockTitle = new Label();
		panel2 = new Panel();
		buttonOK = new Button();
		((ISupportInitialize)imageWarning).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		imageWarning.Image = (Image)componentResourceManager.GetObject("imageWarning.Image");
		((Control)imageWarning).Location = new Point(287, 44);
		((Control)imageWarning).Name = "imageWarning";
		((Control)imageWarning).Size = new Size(112, 112);
		imageWarning.SizeMode = (PictureBoxSizeMode)1;
		imageWarning.TabIndex = 0;
		imageWarning.TabStop = false;
		((Control)panel1).Controls.Add((Control)(object)imageWarning);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(687, 200);
		((Control)panel1).TabIndex = 1;
		((Control)textBlockTitle).Dock = (DockStyle)1;
		((Control)textBlockTitle).Font = new Font("Arial", 16.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBlockTitle).ForeColor = Color.FromArgb(3, 109, 185);
		((Control)textBlockTitle).Location = new Point(0, 0);
		((Control)textBlockTitle).Name = "textBlockTitle";
		((Control)textBlockTitle).Size = new Size(687, 45);
		((Control)textBlockTitle).TabIndex = 2;
		((Control)textBlockTitle).Text = "You are using a trial version!";
		textBlockTitle.TextAlign = (ContentAlignment)32;
		((Control)panel2).Controls.Add((Control)(object)textBlockTitle);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 200);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(687, 50);
		((Control)panel2).TabIndex = 4;
		((Control)buttonOK).BackColor = Color.White;
		((Control)buttonOK).Font = new Font("Arial", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)buttonOK).Location = new Point(278, 270);
		((Control)buttonOK).Name = "buttonContinue";
		((Control)buttonOK).Size = new Size(130, 30);
		((Control)buttonOK).TabIndex = 5;
		((Control)buttonOK).Text = "OK";
		((ButtonBase)buttonOK).UseVisualStyleBackColor = false;
		((Control)buttonOK).Click += ButtonContinue_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(687, 414);
		((Control)this).Controls.Add((Control)(object)buttonOK);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiUsingTrialVersionForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Trial Version";
		((Form)this).FormClosed += new FormClosedEventHandler(Form_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(Form_KeyDown);
		((ISupportInitialize)imageWarning).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
