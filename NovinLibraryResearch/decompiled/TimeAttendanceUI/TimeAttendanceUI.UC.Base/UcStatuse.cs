using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;
using Serilog;

namespace TimeAttendanceUI.UC.Base;

public class UcStatuse : UserControl
{
	private IContainer components;

	private Timer TimerDate;

	private Timer TimerTime;

	internal StatusStrip StatusStrip1;

	internal ToolStripStatusLabel ToolStripStatusLabel2;

	internal ToolStripStatusLabel tssTime;

	internal ToolStripStatusLabel ToolStripStatusLabel1;

	internal ToolStripStatusLabel TssDate;

	internal ToolStripStatusLabel ToolStripStatusLabel4;

	internal ToolStripStatusLabel tssuser;

	internal ToolStripStatusLabel ToolStripStatusLabel5;

	internal ToolStripStatusLabel TSSDBName;

	internal ToolStripStatusLabel tssDesc;

	private ToolStripStatusLabel toolStripStatusLabel3;

	public UcStatuse()
	{
		InitializeComponent();
	}

	private void TimerDate_Tick(object sender, EventArgs e)
	{
		try
		{
			TimerDate.Interval = 60000;
			((ToolStripItem)TssDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now);
			if (!((ToolStripItem)TssDate).Visible)
			{
				((ToolStripItem)TssDate).Visible = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TimerTime_Tick(object sender, EventArgs e)
	{
		try
		{
			((ToolStripItem)tssTime).Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TimerTime_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCStatuse.cs", 42);
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
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Expected O, but got Unknown
		components = new Container();
		TimerDate = new Timer(components);
		TimerTime = new Timer(components);
		StatusStrip1 = new StatusStrip();
		ToolStripStatusLabel1 = new ToolStripStatusLabel();
		TssDate = new ToolStripStatusLabel();
		ToolStripStatusLabel2 = new ToolStripStatusLabel();
		tssTime = new ToolStripStatusLabel();
		ToolStripStatusLabel4 = new ToolStripStatusLabel();
		tssuser = new ToolStripStatusLabel();
		ToolStripStatusLabel5 = new ToolStripStatusLabel();
		TSSDBName = new ToolStripStatusLabel();
		tssDesc = new ToolStripStatusLabel();
		toolStripStatusLabel3 = new ToolStripStatusLabel();
		((Control)StatusStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		TimerDate.Enabled = true;
		TimerDate.Tick += TimerDate_Tick;
		TimerTime.Enabled = true;
		TimerTime.Interval = 1000;
		TimerTime.Tick += TimerTime_Tick;
		((ToolStrip)StatusStrip1).BackColor = Color.LightSteelBlue;
		((Control)StatusStrip1).Dock = (DockStyle)5;
		((ToolStrip)StatusStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[10]
		{
			(ToolStripItem)ToolStripStatusLabel1,
			(ToolStripItem)TssDate,
			(ToolStripItem)ToolStripStatusLabel2,
			(ToolStripItem)tssTime,
			(ToolStripItem)ToolStripStatusLabel4,
			(ToolStripItem)tssuser,
			(ToolStripItem)toolStripStatusLabel3,
			(ToolStripItem)tssDesc,
			(ToolStripItem)ToolStripStatusLabel5,
			(ToolStripItem)TSSDBName
		});
		((Control)StatusStrip1).Location = new Point(0, 0);
		((Control)StatusStrip1).Name = "StatusStrip1";
		((Control)StatusStrip1).Size = new Size(641, 32);
		((Control)StatusStrip1).TabIndex = 1;
		((Control)StatusStrip1).Text = "StatusStrip1";
		((ToolStripItem)ToolStripStatusLabel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolStripStatusLabel1).Name = "ToolStripStatusLabel1";
		((ToolStripItem)ToolStripStatusLabel1).Size = new Size(39, 27);
		((ToolStripItem)ToolStripStatusLabel1).Text = "[تاریخ] :";
		((ToolStripItem)ToolStripStatusLabel1).TextAlign = (ContentAlignment)64;
		TssDate.BorderSides = (ToolStripStatusLabelBorderSides)1;
		((ToolStripItem)TssDate).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)TssDate).ForeColor = Color.Red;
		((ToolStripItem)TssDate).Name = "TssDate";
		((ToolStripItem)TssDate).Size = new Size(37, 27);
		((ToolStripItem)TssDate).Text = "[تاریخ]";
		((ToolStripItem)TssDate).TextAlign = (ContentAlignment)64;
		((ToolStripItem)ToolStripStatusLabel2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolStripStatusLabel2).Name = "ToolStripStatusLabel2";
		((ToolStripItem)ToolStripStatusLabel2).Size = new Size(47, 27);
		((ToolStripItem)ToolStripStatusLabel2).Text = "[ساعت] :";
		((ToolStripItem)ToolStripStatusLabel2).TextAlign = (ContentAlignment)64;
		tssTime.BorderSides = (ToolStripStatusLabelBorderSides)1;
		((ToolStripItem)tssTime).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)tssTime).ForeColor = Color.Red;
		((ToolStripItem)tssTime).Name = "tssTime";
		((ToolStripItem)tssTime).Size = new Size(25, 27);
		((ToolStripItem)tssTime).Text = "16";
		((ToolStripItem)tssTime).TextAlign = (ContentAlignment)64;
		((ToolStripItem)ToolStripStatusLabel4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolStripStatusLabel4).Name = "ToolStripStatusLabel4";
		((ToolStripItem)ToolStripStatusLabel4).Size = new Size(35, 27);
		((ToolStripItem)ToolStripStatusLabel4).Text = "[کاربر]:";
		tssuser.BorderSides = (ToolStripStatusLabelBorderSides)1;
		((ToolStripItem)tssuser).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)tssuser).ForeColor = Color.Red;
		((ToolStripItem)tssuser).Name = "tssuser";
		((ToolStripItem)tssuser).Size = new Size(55, 27);
		((ToolStripItem)tssuser).Text = "tsslabel";
		((ToolStripItem)ToolStripStatusLabel5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolStripStatusLabel5).Name = "ToolStripStatusLabel5";
		((ToolStripItem)ToolStripStatusLabel5).Size = new Size(85, 27);
		((ToolStripItem)ToolStripStatusLabel5).Text = "[نام پایگاه داده] :";
		((ToolStripItem)TSSDBName).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)TSSDBName).ForeColor = Color.Red;
		((ToolStripItem)TSSDBName).Name = "TSSDBName";
		((ToolStripItem)TSSDBName).RightToLeft = (RightToLeft)1;
		((ToolStripItem)TSSDBName).Size = new Size(61, 27);
		((ToolStripItem)TSSDBName).Text = "Database";
		tssDesc.BorderSides = (ToolStripStatusLabelBorderSides)1;
		((ToolStripItem)tssDesc).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)tssDesc).ForeColor = Color.Blue;
		((ToolStripItem)tssDesc).Name = "tssDesc";
		((ToolStripItem)tssDesc).Size = new Size(4, 27);
		((ToolStripItem)toolStripStatusLabel3).Name = "toolStripStatusLabel3";
		((ToolStripItem)toolStripStatusLabel3).Size = new Size(25, 27);
		toolStripStatusLabel3.Spring = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)StatusStrip1);
		((Control)this).Name = "UcStatuse";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(641, 32);
		((Control)StatusStrip1).ResumeLayout(false);
		((Control)StatusStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
