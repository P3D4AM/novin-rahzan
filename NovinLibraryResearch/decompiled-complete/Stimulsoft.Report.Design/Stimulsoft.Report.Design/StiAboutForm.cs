using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design;

public class StiAboutForm : Form
{
	private IContainer components;

	private Label labelVersion;

	private Label labelCopyright;

	private LinkLabel linkLabelStimulsoft;

	private Label labelRights;

	private Label labelAbout;

	private Label labelInfo;

	private PictureBox pictureBoxLogo;

	public StiAboutForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		((Control)pictureBoxLogo).Left = (((Control)this).ClientRectangle.Width - ((Control)pictureBoxLogo).Width) / 2;
	}

	private void SetWarningText(string text)
	{
		((Control)labelInfo).Text = (string.IsNullOrEmpty(((Control)labelInfo).Text) ? text : (((Control)labelInfo).Text + ", " + text));
		((Control)labelInfo).ForeColor = Color.FromArgb(184, 64, 1);
	}

	private string GetFrameworkVersion()
	{
		string text = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
		if (!string.IsNullOrEmpty(text) && text.IndexOf("=v") > 0)
		{
			string text2 = text.Substring(text.IndexOf("=v") + 2);
			return ".NET" + ((text.StartsWith(".NETCoreApp") && text2.StartsWith("3")) ? " Core" : "") + " " + text2;
		}
		return ".NET";
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)labelAbout).Text = (StiStimulsoftApps.IsDesktopApp ? "Stimulsoft Desktop" : "Stimulsoft Designer");
		((Control)labelInfo).Text = StiAccountSettings.CurrentAccount?.User?.UserName;
		bool flag = StiLicenseKeyValidator.IsTrial(StiLicense.LicenseKey);
		if (flag)
		{
			SetWarningText(StiLocalizationExt.Get("Publish", "TrialVersion"));
		}
		if (StiAccountSettings.CurrentAccount != null && !flag)
		{
			using BackgroundWorker backgroundWorker = new BackgroundWorker();
			string textExpired = null;
			backgroundWorker.DoWork += delegate
			{
				textExpired = StiAccountSettings.CurrentAccount.AccountChecker.ProductsExpired();
				if (string.IsNullOrEmpty(textExpired))
				{
					textExpired = StiAccountSettings.CurrentAccount.AccountChecker.ProductsExpired(30);
				}
			};
			backgroundWorker.RunWorkerCompleted += delegate
			{
				if (!string.IsNullOrEmpty(textExpired))
				{
					SetWarningText(textExpired);
				}
			};
			backgroundWorker.RunWorkerAsync();
		}
		((Control)labelCopyright).Text = $"Copyright 2003-{StiVersion.Created.Year} Stimulsoft";
		Assembly assembly = Assembly.GetAssembly(typeof(StiReport));
		Version version = assembly.GetName().Version;
		string text = ((version.Revision > 0) ? $".{version.Revision}" : string.Empty);
		string frameworkVersion = GetFrameworkVersion();
		((Control)labelVersion).Text = string.Format("Version: {0}.{1}.{2}{3} from {4}, WinForms, {5}", version.Major, version.Minor, version.Build, text, "7 December 2021", frameworkVersion);
	}

	private void This_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27 || (int)e.KeyCode == 13)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void LinkLabelStimulsoft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		StiProcess.Start("https://www.stimulsoft.com");
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		((ScrollableControl)this).OnPaintBackground(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		Pen val = new Pen(Color.White);
		try
		{
			val.Alignment = (PenAlignment)1;
			val.Width = (float)(StiScale.Factor * 2.0);
			e.Graphics.DrawRectangle(val, clientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiAboutForm));
		labelVersion = new Label();
		labelCopyright = new Label();
		linkLabelStimulsoft = new LinkLabel();
		labelRights = new Label();
		labelAbout = new Label();
		labelInfo = new Label();
		pictureBoxLogo = new PictureBox();
		((ISupportInitialize)pictureBoxLogo).BeginInit();
		((Control)this).SuspendLayout();
		((Control)labelVersion).BackColor = Color.Transparent;
		labelVersion.FlatStyle = (FlatStyle)1;
		((Control)labelVersion).Location = new Point(30, 465);
		((Control)labelVersion).Margin = new Padding(6, 0, 6, 0);
		((Control)labelVersion).Name = "labelVersion";
		((Control)labelVersion).Size = new Size(990, 31);
		((Control)labelVersion).TabIndex = 2;
		((Control)labelVersion).Text = "Version";
		labelVersion.TextAlign = (ContentAlignment)32;
		((Control)labelVersion).Click += This_Click;
		((Control)labelCopyright).BackColor = Color.Transparent;
		labelCopyright.FlatStyle = (FlatStyle)1;
		((Control)labelCopyright).Location = new Point(24, 415);
		((Control)labelCopyright).Margin = new Padding(6, 0, 6, 0);
		((Control)labelCopyright).Name = "labelCopyright";
		((Control)labelCopyright).Size = new Size(996, 31);
		((Control)labelCopyright).TabIndex = 4;
		((Control)labelCopyright).Text = "Copyright (C) 2003-2021 Stimulsoft";
		labelCopyright.TextAlign = (ContentAlignment)32;
		((Control)labelCopyright).Click += This_Click;
		((Control)linkLabelStimulsoft).BackColor = Color.Transparent;
		((Control)linkLabelStimulsoft).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)linkLabelStimulsoft).Location = new Point(24, 631);
		((Control)linkLabelStimulsoft).Margin = new Padding(6, 0, 6, 0);
		((Control)linkLabelStimulsoft).Name = "linkLabelStimulsoft";
		((Control)linkLabelStimulsoft).Size = new Size(996, 31);
		((Control)linkLabelStimulsoft).TabIndex = 7;
		linkLabelStimulsoft.TabStop = true;
		((Control)linkLabelStimulsoft).Text = "www.stimulsoft.com";
		((Label)linkLabelStimulsoft).TextAlign = (ContentAlignment)2;
		linkLabelStimulsoft.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkLabelStimulsoft_LinkClicked);
		((Control)labelRights).BackColor = Color.Transparent;
		labelRights.FlatStyle = (FlatStyle)1;
		((Control)labelRights).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelRights).Location = new Point(24, 512);
		((Control)labelRights).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRights).Name = "labelRights";
		((Control)labelRights).Size = new Size(996, 31);
		((Control)labelRights).TabIndex = 4;
		((Control)labelRights).Text = "All rights reserved";
		labelRights.TextAlign = (ContentAlignment)32;
		((Control)labelRights).Click += This_Click;
		((Control)labelAbout).BackColor = Color.Transparent;
		labelAbout.FlatStyle = (FlatStyle)1;
		((Control)labelAbout).Font = new Font("Microsoft Sans Serif", 20f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelAbout).ForeColor = Color.FromArgb(0, 112, 184);
		((Control)labelAbout).Location = new Point(24, 50);
		((Control)labelAbout).Margin = new Padding(6, 0, 6, 0);
		((Control)labelAbout).Name = "labelAbout";
		((Control)labelAbout).Size = new Size(996, 73);
		((Control)labelAbout).TabIndex = 4;
		((Control)labelAbout).Text = "Stimulsoft Designer";
		labelAbout.TextAlign = (ContentAlignment)32;
		((Control)labelAbout).Click += This_Click;
		((Control)labelInfo).BackColor = Color.Transparent;
		labelInfo.FlatStyle = (FlatStyle)1;
		((Control)labelInfo).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelInfo).Location = new Point(24, 558);
		((Control)labelInfo).Margin = new Padding(6, 0, 6, 0);
		((Control)labelInfo).Name = "labelInfo";
		((Control)labelInfo).Size = new Size(996, 31);
		((Control)labelInfo).TabIndex = 8;
		((Control)labelInfo).Text = "Expired";
		labelInfo.TextAlign = (ContentAlignment)32;
		((Control)pictureBoxLogo).Enabled = false;
		pictureBoxLogo.Image = (Image)componentResourceManager.GetObject("pictureBoxLogo.Image");
		((Control)pictureBoxLogo).Location = new Point(430, 171);
		((Control)pictureBoxLogo).Margin = new Padding(6);
		((Control)pictureBoxLogo).Name = "pictureBoxLogo";
		((Control)pictureBoxLogo).Size = new Size(200, 192);
		pictureBoxLogo.SizeMode = (PictureBoxSizeMode)4;
		pictureBoxLogo.TabIndex = 9;
		pictureBoxLogo.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(1044, 679);
		((Control)this).Controls.Add((Control)(object)pictureBoxLogo);
		((Control)this).Controls.Add((Control)(object)labelInfo);
		((Control)this).Controls.Add((Control)(object)linkLabelStimulsoft);
		((Control)this).Controls.Add((Control)(object)labelAbout);
		((Control)this).Controls.Add((Control)(object)labelRights);
		((Control)this).Controls.Add((Control)(object)labelCopyright);
		((Control)this).Controls.Add((Control)(object)labelVersion);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiAboutForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Form)this).Load += This_Load;
		((Control)this).Click += This_Click;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)pictureBoxLogo).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
