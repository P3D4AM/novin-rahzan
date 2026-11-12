using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiNotFoundPackageForm : Form
{
	private string name;

	private IContainer components;

	private Panel panel1;

	private PictureBox imageMessage;

	private Panel panel2;

	private Label textBlockTitle;

	private Panel panel3;

	private StiExceptionPreviewForm.ExceptionButton buttonContinue;

	private StiExceptionPreviewForm.ExceptionButton buttonReadMore;

	private Label textBlockMessage;

	public StiNotFoundPackageForm(string name)
	{
		InitializeComponent();
		((Form)this).ShowIcon = false;
		this.name = name;
		imageMessage.Image = StiReportDesignImages.Dictionary.BigNuGet();
		Localize();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Notices", "Warning");
		((Control)textBlockTitle).Text = StiLocalization.Get("Notices", "Warning");
		((Control)textBlockMessage).Text = (StiLocalization.IsRussian ? ("Для корректной работы подключите пакет \"" + name + "\".") : ("Connect the \"" + name + "\" package for the correct work."));
		((Control)buttonContinue).Text = Loc.Get("DesignerFx", "Continue");
		((Control)buttonReadMore).Text = StiLocalizationExt.Get("Publish", "ReadMore");
	}

	public static string GetCurrentLocalization()
	{
		if (StiLocalization.IsRussian)
		{
			return "ru";
		}
		if (StiLocalization.IsDe)
		{
			return "de";
		}
		return "en";
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

	private void ButtonContinue_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void ButtonTellMeMore_Click(object sender, EventArgs e)
	{
		string currentLocalization = GetCurrentLocalization();
		StiProcess.Start("https://www.stimulsoft.com/" + currentLocalization + "/downloads/additional-packages");
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiNotFoundPackageForm));
		panel1 = new Panel();
		imageMessage = new PictureBox();
		panel2 = new Panel();
		textBlockMessage = new Label();
		textBlockTitle = new Label();
		panel3 = new Panel();
		buttonContinue = new StiExceptionPreviewForm.ExceptionButton();
		buttonReadMore = new StiExceptionPreviewForm.ExceptionButton();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)imageMessage).BeginInit();
		((Control)panel2).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)imageMessage);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(600, 217);
		((Control)panel1).TabIndex = 0;
		imageMessage.Image = (Image)componentResourceManager.GetObject("imageMessage.Image");
		((Control)imageMessage).Location = new Point(229, 51);
		((Control)imageMessage).Name = "imageMessage";
		((Control)imageMessage).Size = new Size(120, 120);
		imageMessage.SizeMode = (PictureBoxSizeMode)4;
		imageMessage.TabIndex = 0;
		imageMessage.TabStop = false;
		((Control)panel2).Controls.Add((Control)(object)textBlockMessage);
		((Control)panel2).Controls.Add((Control)(object)textBlockTitle);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 217);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(600, 73);
		((Control)panel2).TabIndex = 1;
		((Control)textBlockMessage).Dock = (DockStyle)5;
		((Control)textBlockMessage).Font = new Font("Arial", 13f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBlockMessage).ForeColor = Color.DimGray;
		((Control)textBlockMessage).Location = new Point(0, 27);
		((Control)textBlockMessage).Name = "textBlockMessage";
		((Control)textBlockMessage).Padding = new Padding(6, 0, 6, 0);
		((Control)textBlockMessage).Size = new Size(600, 46);
		((Control)textBlockMessage).TabIndex = 2;
		((Control)textBlockMessage).Text = "message";
		textBlockMessage.TextAlign = (ContentAlignment)2;
		((Control)textBlockTitle).Dock = (DockStyle)1;
		((Control)textBlockTitle).Font = new Font("Arial", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBlockTitle).ForeColor = Color.FromArgb(3, 109, 185);
		((Control)textBlockTitle).Location = new Point(0, 0);
		((Control)textBlockTitle).Name = "textBlockTitle";
		((Control)textBlockTitle).Padding = new Padding(6, 0, 6, 0);
		((Control)textBlockTitle).Size = new Size(600, 27);
		((Control)textBlockTitle).TabIndex = 0;
		((Control)textBlockTitle).Text = "Warning";
		textBlockTitle.TextAlign = (ContentAlignment)2;
		((Control)panel3).Controls.Add((Control)(object)buttonContinue);
		((Control)panel3).Controls.Add((Control)(object)buttonReadMore);
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(0, 290);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(600, 65);
		((Control)panel3).TabIndex = 2;
		((Control)buttonContinue).Font = new Font("Microsoft Sans Serif", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)buttonContinue).ForeColor = Color.White;
		buttonContinue.IsContinueButton = true;
		((Control)buttonContinue).Location = new Point(306, 17);
		((Control)buttonContinue).Name = "buttonContinue";
		((Control)buttonContinue).Size = new Size(140, 30);
		((Control)buttonContinue).TabIndex = 3;
		((Control)buttonContinue).Text = "Continue";
		((ButtonBase)buttonContinue).UseVisualStyleBackColor = true;
		((Control)buttonContinue).Click += ButtonContinue_Click;
		((Control)buttonReadMore).Font = new Font("Microsoft Sans Serif", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		buttonReadMore.IsContinueButton = false;
		((Control)buttonReadMore).Location = new Point(154, 17);
		((Control)buttonReadMore).Name = "buttonTellMeMore";
		((Control)buttonReadMore).Size = new Size(140, 30);
		((Control)buttonReadMore).TabIndex = 2;
		((Control)buttonReadMore).Text = "Tell Me More";
		((ButtonBase)buttonReadMore).UseVisualStyleBackColor = true;
		((Control)buttonReadMore).Click += ButtonTellMeMore_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(600, 382);
		((Control)this).Controls.Add((Control)(object)panel3);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiNotFoundPackageForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Warning";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)imageMessage).EndInit();
		((Control)panel2).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
