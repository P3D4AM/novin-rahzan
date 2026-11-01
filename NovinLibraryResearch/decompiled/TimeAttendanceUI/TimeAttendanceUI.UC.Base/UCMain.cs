using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NPResourceManagement;
using PacketParser.Entities;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.UC.Base;

public class UCMain : UserControl
{
	private IContainer components;

	private Timer timer1;

	private PictureBox pictureBox1;

	public UCMain()
	{
		InitializeComponent();
	}

	private Image NextPic()
	{
		Bitmap hozor_Back = ImageResourceManagement.Hozor_Back4;
		try
		{
			string text = Path.Combine(Application.StartupPath, "DesctopPictures");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				AttLogger.LoggerUi.Information("create Directory from " + text);
				return (Image)(object)hozor_Back;
			}
			string[] files = Directory.GetFiles(text, "*.jpg");
			if (files.Length == 0)
			{
				return (Image)(object)hozor_Back;
			}
			int num = new Random().Next(files.Length);
			if (num > files.Length - 1)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "index=" + num + "length=" + files.Length, "", 3);
			}
			if (num >= 0 && files.Length > num)
			{
				return Image.FromFile(files[num]);
			}
			return (Image)(object)hozor_Back;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMain.cs", 45);
			return (Image)(object)hozor_Back;
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			timer1.Interval = 10000;
			pictureBox1.Image = NextPic();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMain.cs", 63);
		}
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		components = new Container();
		timer1 = new Timer(components);
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		timer1.Enabled = true;
		timer1.Interval = 1;
		timer1.Tick += timer1_Tick;
		((Control)pictureBox1).BackColor = SystemColors.Desktop;
		((Control)pictureBox1).Dock = (DockStyle)5;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(896, 258);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Name = "UCMain";
		((Control)this).Size = new Size(896, 258);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
