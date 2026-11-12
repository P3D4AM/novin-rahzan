using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiSybaseAdsWarningForm : StiFormUX
{
	private IContainer components;

	private StiLabelUX labelMessage;

	private ErrorProvider errorProvider;

	private StiButtonUX btOk;

	private PictureBox pictureBoxMessage;

	private StiLinkLabelUX linkLabelOpenFolder;

	internal static bool NeedsToShow
	{
		get
		{
			try
			{
				return !Directory.Exists(AdvantageFolder) || Directory.GetFiles(AdvantageFolder).Length == 0;
			}
			catch
			{
				return true;
			}
		}
	}

	private static string AdvantageFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "DataAdapters", "Advantage.Data.Provider");

	public StiSybaseAdsWarningForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		((Form)this).ShowIcon = false;
		DoScaling();
		try
		{
			if (!Directory.Exists(AdvantageFolder))
			{
				Directory.CreateDirectory(AdvantageFolder);
			}
		}
		catch
		{
		}
	}

	private void DoScaling()
	{
		pictureBoxMessage.Image = StiReportDesignImages.Dictionary.SybaseAdsDatabase();
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	internal static void ShowWarning(Form owner)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (!NeedsToShow)
		{
			return;
		}
		StiSybaseAdsWarningForm stiSybaseAdsWarningForm = new StiSybaseAdsWarningForm();
		try
		{
			((Form)stiSybaseAdsWarningForm).Owner = owner;
			((Form)stiSybaseAdsWarningForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiSybaseAdsWarningForm)?.Dispose();
		}
	}

	private void LinkLabelOpenFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		StiProcess.Start(AdvantageFolder);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSybaseAdsWarningForm));
		labelMessage = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		btOk = new StiButtonUX();
		pictureBoxMessage = new PictureBox();
		linkLabelOpenFolder = new StiLinkLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((ISupportInitialize)pictureBoxMessage).BeginInit();
		((Control)this).SuspendLayout();
		((Control)labelMessage).Anchor = (AnchorStyles)13;
		((Control)labelMessage).Font = new Font("Microsoft Sans Serif", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)labelMessage).ForeColor = Color.DimGray;
		((Control)labelMessage).Location = new Point(123, 25);
		((Control)labelMessage).Name = "labelMessage";
		((Control)labelMessage).Size = new Size(333, 35);
		((Control)labelMessage).TabIndex = 1;
		((Control)labelMessage).Text = "Please copy 'Sybase ADS' adapter's assemblies to the data adapter's folder. Restart the designer after!";
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Button)btOk).DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(385, 108);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)pictureBoxMessage).Location = new Point(24, 24);
		((Control)pictureBoxMessage).Margin = new Padding(2, 2, 2, 2);
		((Control)pictureBoxMessage).Name = "pictureBoxMessage";
		((Control)pictureBoxMessage).Size = new Size(64, 64);
		pictureBoxMessage.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxMessage.TabIndex = 2;
		pictureBoxMessage.TabStop = false;
		((Control)linkLabelOpenFolder).Anchor = (AnchorStyles)13;
		((Control)linkLabelOpenFolder).AutoSize = true;
		((Control)linkLabelOpenFolder).Location = new Point(123, 71);
		((Control)linkLabelOpenFolder).Margin = new Padding(2, 0, 2, 0);
		((Control)linkLabelOpenFolder).Name = "linkLabelOpenFolder";
		((Control)linkLabelOpenFolder).Size = new Size(65, 13);
		((Control)linkLabelOpenFolder).TabIndex = 3;
		((LinkLabel)linkLabelOpenFolder).TabStop = true;
		((Control)linkLabelOpenFolder).Text = "Open Folder";
		((LinkLabel)linkLabelOpenFolder).LinkClicked += new LinkLabelLinkClickedEventHandler(LinkLabelOpenFolder_LinkClicked);
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(472, 145);
		((Control)this).Controls.Add((Control)(object)linkLabelOpenFolder);
		((Control)this).Controls.Add((Control)(object)pictureBoxMessage);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)labelMessage);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSybaseAdsWarningForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit category";
		((ISupportInitialize)errorProvider).EndInit();
		((ISupportInitialize)pictureBoxMessage).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
