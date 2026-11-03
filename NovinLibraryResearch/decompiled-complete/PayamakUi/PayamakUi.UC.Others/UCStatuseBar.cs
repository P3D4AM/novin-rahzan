using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;
using PayamakSettings;
using Serilog;

namespace PayamakUi.UC.Others;

public class UCStatuseBar : UserControl, IHasContext
{
	private DateTime _D;

	private IContainer components;

	internal ToolStripStatusLabel tssTime;

	internal StatusStrip StatusStrip1;

	internal ToolStripStatusLabel TssDate;

	public Timer TimerTime;

	internal ToolStripStatusLabel ToolStripStatusLabel2;

	internal ToolStripStatusLabel ToolStripStatusLabel1;

	internal ToolStripStatusLabel ToolStripStatusLabel3;

	internal ToolStripStatusLabel ToolStripStatusClock1;

	internal UCSignal UcSignal1;

	internal ToolStripStatusLabel TssLblDB;

	internal ToolStripStatusLabel toolStripStatusLabel4;

	public string ContextGuid { get; set; }

	public DateTime DateM
	{
		get
		{
			try
			{
				_ = DateTime.Now;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateM", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCStatuseBar.cs", 71);
			}
			return _D;
		}
		set
		{
			try
			{
				_D = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateM", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCStatuseBar.cs", 85);
			}
		}
	}

	public UCStatuseBar()
	{
		try
		{
			InitializeComponent();
			DateM = Db.DateM;
			StartClockAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCStatuseBar.cs", 27);
		}
	}

	public async Task InitAsync(string ContextGuid, CancellationToken inputToken)
	{
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || inputToken.IsCancellationRequested)
				{
					return;
				}
			}
			if (((Control)this).IsDisposed || inputToken.IsCancellationRequested)
			{
				return;
			}
			AccGlobalSettings PayamakSetting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, inputToken);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((ToolStripItem)TssLblDB).Text = PayamakSetting.Payamak.ServerConnectionString;
					Match match = Regex.Match(((ToolStripItem)TssLblDB).Text, "Initial\\s+Catalog\\s*=\\s*([^;]+)", RegexOptions.IgnoreCase);
					if (match.Success)
					{
						string value = match.Groups[1].Value;
						((ToolStripItem)TssLblDB).Text = value;
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCStatuseBar.cs", 58);
		}
	}

	private async void StartClockAsync()
	{
		try
		{
			while (true)
			{
				DateTime now = DateTime.Now;
				((ToolStripItem)tssTime).Text = $"{now.Hour:D2}:{now.Minute:D2}:{now.Second:D2}";
				await Task.Delay(1000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartClockAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCStatuseBar.cs", 103);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		components = new Container();
		tssTime = new ToolStripStatusLabel();
		StatusStrip1 = new StatusStrip();
		ToolStripStatusLabel2 = new ToolStripStatusLabel();
		ToolStripStatusClock1 = new ToolStripStatusLabel();
		ToolStripStatusLabel1 = new ToolStripStatusLabel();
		TssDate = new ToolStripStatusLabel();
		toolStripStatusLabel4 = new ToolStripStatusLabel();
		TssLblDB = new ToolStripStatusLabel();
		TimerTime = new Timer(components);
		ToolStripStatusLabel3 = new ToolStripStatusLabel();
		UcSignal1 = new UCSignal();
		((Control)StatusStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		tssTime.BorderSides = (ToolStripStatusLabelBorderSides)4;
		((ToolStripItem)tssTime).ForeColor = Color.Red;
		((ToolStripItem)tssTime).Name = "tssTime";
		((ToolStripItem)tssTime).Size = new Size(33, 23);
		((ToolStripItem)tssTime).Text = "زمان";
		((ToolStripItem)tssTime).TextAlign = (ContentAlignment)64;
		((ToolStrip)StatusStrip1).BackColor = Color.LightSteelBlue;
		((Control)StatusStrip1).Dock = (DockStyle)5;
		((ToolStrip)StatusStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[7]
		{
			(ToolStripItem)ToolStripStatusLabel2,
			(ToolStripItem)ToolStripStatusClock1,
			(ToolStripItem)tssTime,
			(ToolStripItem)ToolStripStatusLabel1,
			(ToolStripItem)TssDate,
			(ToolStripItem)toolStripStatusLabel4,
			(ToolStripItem)TssLblDB
		});
		((Control)StatusStrip1).Location = new Point(0, 0);
		((Control)StatusStrip1).Name = "StatusStrip1";
		((Control)StatusStrip1).Size = new Size(533, 28);
		StatusStrip1.SizingGrip = false;
		((Control)StatusStrip1).TabIndex = 0;
		((Control)StatusStrip1).Text = "StatusStrip1";
		((ToolStripItem)ToolStripStatusLabel2).Name = "ToolStripStatusLabel2";
		((ToolStripItem)ToolStripStatusLabel2).Size = new Size(44, 23);
		((ToolStripItem)ToolStripStatusLabel2).Text = "ساعت :";
		((ToolStripItem)ToolStripStatusLabel2).TextAlign = (ContentAlignment)64;
		((ToolStripItem)ToolStripStatusClock1).Name = "ToolStripStatusClock1";
		((ToolStripItem)ToolStripStatusClock1).Size = new Size(44, 23);
		((ToolStripItem)ToolStripStatusClock1).Text = "ساعت :";
		((ToolStripItem)ToolStripStatusLabel1).Name = "ToolStripStatusLabel1";
		((ToolStripItem)ToolStripStatusLabel1).Size = new Size(37, 23);
		((ToolStripItem)ToolStripStatusLabel1).Text = "تاریخ :";
		((ToolStripItem)ToolStripStatusLabel1).TextAlign = (ContentAlignment)64;
		TssDate.BorderSides = (ToolStripStatusLabelBorderSides)4;
		((ToolStripItem)TssDate).ForeColor = Color.Red;
		((ToolStripItem)TssDate).Name = "TssDate";
		((ToolStripItem)TssDate).Size = new Size(35, 23);
		((ToolStripItem)TssDate).Text = "تاریخ";
		((ToolStripItem)TssDate).TextAlign = (ContentAlignment)64;
		((ToolStripItem)toolStripStatusLabel4).Name = "toolStripStatusLabel4";
		((ToolStripItem)toolStripStatusLabel4).Size = new Size(99, 23);
		((ToolStripItem)toolStripStatusLabel4).Text = "نام بانک اطلاعاتی :";
		TssLblDB.BorderSides = (ToolStripStatusLabelBorderSides)4;
		((ToolStripItem)TssLblDB).ForeColor = Color.Red;
		((ToolStripItem)TssLblDB).Name = "TssLblDB";
		((ToolStripItem)TssLblDB).Size = new Size(123, 23);
		((ToolStripItem)TssLblDB).Text = "ToolStripStatusLabel4";
		TimerTime.Enabled = true;
		TimerTime.Interval = 1000;
		((ToolStripItem)ToolStripStatusLabel3).Name = "ToolStripStatusLabel3";
		((ToolStripItem)ToolStripStatusLabel3).Size = new Size(0, 25);
		((Control)UcSignal1).BackColor = Color.Transparent;
		UcSignal1.ContextGuid = null;
		((Control)UcSignal1).Dock = (DockStyle)3;
		((Control)UcSignal1).Location = new Point(0, 0);
		((Control)UcSignal1).Name = "UcSignal1";
		UcSignal1.Signal = 40;
		((Control)UcSignal1).Size = new Size(42, 28);
		((Control)UcSignal1).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).Controls.Add((Control)(object)UcSignal1);
		((Control)this).Controls.Add((Control)(object)StatusStrip1);
		((Control)this).Name = "UCStatuseBar";
		((Control)this).Size = new Size(533, 28);
		((Control)StatusStrip1).ResumeLayout(false);
		((Control)StatusStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
