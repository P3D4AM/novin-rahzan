using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Smackbio;

namespace SBXPCSampleCSharp;

public class FrmMonitoringEbio : Form
{
	public delegate void delegateAddEvent(string msg);

	private LogServer m_LogServer;

	private bool m_Running;

	private ManualResetEvent m_StopEvent;

	private IContainer components = null;

	private Label lblEventCount;

	private Label label1;

	private TextBox txtPortNo;

	private Label lblPortNo;

	public Button cmdStopMonitoring;

	public Button cmdStartMonitoring;

	public Button cmdClearLogs;

	private ListBox lstEvents;

	public FrmMonitoringEbio()
	{
		InitializeComponent();
		m_Running = false;
	}

	private void FrmMonitoringEbio_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (m_Running && ((Control)cmdStopMonitoring).Enabled)
		{
			cmdStopMonitoring.PerformClick();
		}
	}

	private void cmdStartMonitoring_Click(object sender, EventArgs e)
	{
		((Control)cmdStartMonitoring).Enabled = false;
		((Control)cmdStopMonitoring).Enabled = true;
		((Control)txtPortNo).Enabled = false;
		m_Running = true;
		m_StopEvent = new ManualResetEvent(initialState: false);
		ThreadPool.QueueUserWorkItem(MonitoringThread);
	}

	private void cmdStopMonitoring_Click(object sender, EventArgs e)
	{
		m_Running = false;
		m_StopEvent.WaitOne();
		((Control)cmdStartMonitoring).Enabled = true;
		((Control)cmdStopMonitoring).Enabled = false;
		((Control)txtPortNo).Enabled = true;
	}

	private void cmdClearLogs_Click(object sender, EventArgs e)
	{
		lstEvents.Items.Clear();
		((Control)lblEventCount).Text = lstEvents.Items.Count.ToString();
	}

	public void OnAddEvent(string msg)
	{
		lstEvents.Items.Add((object)msg);
		((Control)lblEventCount).Text = lstEvents.Items.Count.ToString();
	}

	public bool OnTimeLog(string terminalType, int terminalID, string serialNumber, int transactionID, DateTime logTime, long userID, int doorID, string attendanceStatus, string verifyMode, int jobCode, string antipass, byte[] photo)
	{
		try
		{
			string text = "[" + terminalType + ":";
			text += terminalID;
			text = text + " SN=" + serialNumber + "] ";
			text += "TimeLog";
			text = text + "(" + transactionID + ") ";
			text = text + logTime.ToString() + ", ";
			text = text + "UserID=" + $"{userID}, ";
			text = text + "Door=" + doorID + ", ";
			text = text + "AttendStat=" + attendanceStatus + ", ";
			text = text + "VMode=" + verifyMode + ", ";
			text = text + "JobCode=" + jobCode + ", ";
			text = text + "Antipass=" + antipass + ", ";
			if (photo == null)
			{
				text += "Photo=No";
			}
			else
			{
				text += "Photo=Yes ";
				text = text + "(" + Convert.ToString(photo.Length) + "bytes)";
			}
			((Control)this).BeginInvoke((Delegate)new delegateAddEvent(OnAddEvent), new object[1] { text });
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool OnAdminLog(string terminalType, int terminalID, string serialNumber, int transactionID, DateTime logTime, long adminID, long userID, string action, int result)
	{
		try
		{
			string text = "[" + terminalType + ":";
			text += terminalID;
			text = text + " SN=" + serialNumber + "] ";
			text += "AdminLog";
			text = text + "(" + transactionID + ") ";
			text = text + logTime.ToString() + ", ";
			text = text + "AdminID=" + $"{adminID}, ";
			text = text + "UserID=" + $"{userID}, ";
			text = text + "Action=" + action + ", ";
			text = text + "Status=" + $"{result:D}";
			((Control)this).BeginInvoke((Delegate)new delegateAddEvent(OnAddEvent), new object[1] { text });
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool OnAlarmLog(string terminalType, int terminalID, string serialNumber, int transactionID, DateTime logTime, long userID, int doorID, string alarmType)
	{
		try
		{
			string text = "[" + terminalType + ":";
			text += terminalID;
			text = text + " SN=" + serialNumber + "] ";
			text += "AlarmLog";
			text = text + "(" + transactionID + ") ";
			text = text + logTime.ToString() + ", ";
			text = text + "UserID=" + $"{userID}, ";
			text = text + "Door=" + doorID + ", ";
			text = text + "Type=" + alarmType;
			((Control)this).BeginInvoke((Delegate)new delegateAddEvent(OnAddEvent), new object[1] { text });
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void OnPing(string terminalType, int terminalID, string serialNumber, int transactionID)
	{
		string text = "[" + terminalType + ":";
		text += terminalID;
		text = text + " SN=" + serialNumber + "] ";
		text += "KeepAlive";
		text = text + "(" + transactionID + ") ";
		((Control)this).BeginInvoke((Delegate)new delegateAddEvent(OnAddEvent), new object[1] { text });
	}

	private void MonitoringThread(object state)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		ushort num = Convert.ToUInt16(((Control)txtPortNo).Text);
		m_LogServer = new LogServer(num, new TimeLogCallback(OnTimeLog), new AdminLogCallback(OnAdminLog), new AlarmLogCallback(OnAlarmLog), new PingCallback(OnPing));
		while (m_Running)
		{
			Thread.Sleep(100);
		}
		m_LogServer.Dispose();
		m_StopEvent.Set();
	}

	private void FrmMonitoringEbio_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Form)this).Close();
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
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
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		lblEventCount = new Label();
		label1 = new Label();
		txtPortNo = new TextBox();
		lblPortNo = new Label();
		cmdStopMonitoring = new Button();
		cmdStartMonitoring = new Button();
		cmdClearLogs = new Button();
		lstEvents = new ListBox();
		((Control)this).SuspendLayout();
		((Control)lblEventCount).AutoSize = true;
		((Control)lblEventCount).Font = new Font("Tahoma", 12f);
		((Control)lblEventCount).Location = new Point(730, 578);
		((Control)lblEventCount).Name = "lblEventCount";
		((Control)lblEventCount).Size = new Size(18, 19);
		((Control)lblEventCount).TabIndex = 96;
		((Control)lblEventCount).Text = "0";
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Tahoma", 12f);
		((Control)label1).Location = new Point(631, 578);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(101, 19);
		((Control)label1).TabIndex = 97;
		((Control)label1).Text = "Event Count:";
		((Control)txtPortNo).Font = new Font("Tahoma", 12f);
		((Control)txtPortNo).Location = new Point(118, 575);
		((Control)txtPortNo).Name = "txtPortNo";
		((Control)txtPortNo).Size = new Size(68, 27);
		((Control)txtPortNo).TabIndex = 95;
		((Control)txtPortNo).Text = "5005";
		txtPortNo.TextAlign = (HorizontalAlignment)1;
		((Control)lblPortNo).AutoSize = true;
		((Control)lblPortNo).Font = new Font("Tahoma", 12f);
		((Control)lblPortNo).Location = new Point(5, 578);
		((Control)lblPortNo).Name = "lblPortNo";
		((Control)lblPortNo).Size = new Size(106, 19);
		((Control)lblPortNo).TabIndex = 94;
		((Control)lblPortNo).Text = "Port Number:";
		((Control)cmdStopMonitoring).BackColor = SystemColors.Control;
		((Control)cmdStopMonitoring).Cursor = Cursors.Default;
		((Control)cmdStopMonitoring).Enabled = false;
		((Control)cmdStopMonitoring).Font = new Font("Tahoma", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdStopMonitoring).ForeColor = SystemColors.ControlText;
		((Control)cmdStopMonitoring).Location = new Point(423, 570);
		((Control)cmdStopMonitoring).Name = "cmdStopMonitoring";
		((Control)cmdStopMonitoring).RightToLeft = (RightToLeft)0;
		((Control)cmdStopMonitoring).Size = new Size(190, 36);
		((Control)cmdStopMonitoring).TabIndex = 93;
		((Control)cmdStopMonitoring).Text = "Stop Monitoring";
		((ButtonBase)cmdStopMonitoring).UseVisualStyleBackColor = false;
		((Control)cmdStopMonitoring).Click += cmdStopMonitoring_Click;
		((Control)cmdStartMonitoring).BackColor = SystemColors.Control;
		((Control)cmdStartMonitoring).Cursor = Cursors.Default;
		((Control)cmdStartMonitoring).Font = new Font("Tahoma", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdStartMonitoring).ForeColor = SystemColors.ControlText;
		((Control)cmdStartMonitoring).Location = new Point(227, 570);
		((Control)cmdStartMonitoring).Name = "cmdStartMonitoring";
		((Control)cmdStartMonitoring).RightToLeft = (RightToLeft)0;
		((Control)cmdStartMonitoring).Size = new Size(190, 36);
		((Control)cmdStartMonitoring).TabIndex = 92;
		((Control)cmdStartMonitoring).Text = "Start Monitoring";
		((ButtonBase)cmdStartMonitoring).UseVisualStyleBackColor = false;
		((Control)cmdStartMonitoring).Click += cmdStartMonitoring_Click;
		((Control)cmdClearLogs).BackColor = SystemColors.Control;
		((Control)cmdClearLogs).Cursor = Cursors.Default;
		((Control)cmdClearLogs).Font = new Font("Tahoma", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdClearLogs).ForeColor = SystemColors.ControlText;
		((Control)cmdClearLogs).Location = new Point(824, 570);
		((Control)cmdClearLogs).Name = "cmdClearLogs";
		((Control)cmdClearLogs).RightToLeft = (RightToLeft)0;
		((Control)cmdClearLogs).Size = new Size(190, 36);
		((Control)cmdClearLogs).TabIndex = 91;
		((Control)cmdClearLogs).Text = "Clear Logs";
		((ButtonBase)cmdClearLogs).UseVisualStyleBackColor = false;
		((Control)cmdClearLogs).Click += cmdClearLogs_Click;
		((Control)lstEvents).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ListControl)lstEvents).FormattingEnabled = true;
		lstEvents.HorizontalExtent = 2000;
		lstEvents.HorizontalScrollbar = true;
		lstEvents.ItemHeight = 16;
		((Control)lstEvents).Location = new Point(2, 2);
		((Control)lstEvents).Name = "lstEvents";
		((Control)lstEvents).Size = new Size(1011, 548);
		((Control)lstEvents).TabIndex = 90;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1016, 616);
		((Control)this).Controls.Add((Control)(object)lblEventCount);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)txtPortNo);
		((Control)this).Controls.Add((Control)(object)lblPortNo);
		((Control)this).Controls.Add((Control)(object)cmdStopMonitoring);
		((Control)this).Controls.Add((Control)(object)cmdStartMonitoring);
		((Control)this).Controls.Add((Control)(object)cmdClearLogs);
		((Control)this).Controls.Add((Control)(object)lstEvents);
		((Control)this).Name = "FrmMonitoringEbio";
		((Control)this).Text = "FrmMonitoringEbio";
		((Form)this).FormClosing += new FormClosingEventHandler(FrmMonitoringEbio_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(FrmMonitoringEbio_FormClosed);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
