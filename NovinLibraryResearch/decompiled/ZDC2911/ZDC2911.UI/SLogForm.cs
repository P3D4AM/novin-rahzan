using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.Business;
using ZDC2911.Entity;
using ZDC2911.IConvert;

namespace ZDC2911.UI;

public class SLogForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private IContainer components = null;

	private ListView lvw_SLogList;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader3;

	private ColumnHeader columnHeader4;

	private ColumnHeader columnHeader5;

	private ColumnHeader columnHeader6;

	private Label lbl_EndDate;

	private Label lbl_BeginDate;

	private Button btn_Clear;

	private Button btn_DownloadHistory;

	private Button btn_DownloadNew;

	private CheckBox chk_NewFlag;

	private DateTimePicker dtp_End;

	private DateTimePicker dtp_Begin;

	public SLogForm(DeviceCommEty deviceEty)
	{
		InitializeComponent();
		device = deviceEty.Device;
		deviceConnection = deviceEty.DeviceConnection;
	}

	private List<DateTime> GetDateTimeList()
	{
		List<DateTime> list = new List<DateTime>();
		list.Add(dtp_Begin.Value);
		list.Add(dtp_End.Value);
		return list;
	}

	private void AddRecordToListView(List<Record> recordList)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		lvw_SLogList.Items.Clear();
		int num = 1;
		foreach (Record record in recordList)
		{
			string text = ConvertObject.SLogType(record.Action);
			ListViewItem val = new ListViewItem(new string[6]
			{
				num.ToString(),
				record.MDIN.ToString(),
				record.DIN.ToString(),
				text,
				record.Verify.ToString(),
				record.Clock.ToString("yyyy-MM-dd HH:mm:ss")
			});
			lvw_SLogList.Items.Add(val);
			num++;
		}
	}

	private void SLogForm_Load(object sender, EventArgs e)
	{
		dtp_Begin.MinDate = InitData.MinDateTime;
		dtp_Begin.MaxDate = InitData.MaxDateTime;
		dtp_Begin.Value = InitData.MinDateTime;
		dtp_End.MinDate = InitData.MinDateTime;
		dtp_End.MaxDate = InitData.MaxDateTime;
		dtp_End.Value = InitData.MaxDateTime;
	}

	private void btn_DownloadNew_Click(object sender, EventArgs e)
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			List<DateTime> dateTimeList = GetDateTimeList();
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj = true;
			obj2 = dateTimeList;
			if (!deviceConnection.GetProperty((DeviceProperty)18, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get New Slog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			if ((int)obj2 == 0)
			{
				lvw_SLogList.Items.Clear();
				return;
			}
			List<bool> list = new List<bool>();
			list.Add(item: true);
			list.Add(chk_NewFlag.Checked);
			obj = list;
			obj2 = dateTimeList;
			if (deviceConnection.GetProperty((DeviceProperty)12, obj, ref device, ref obj2))
			{
				List<Record> recordList = (List<Record>)obj2;
				AddRecordToListView(recordList);
			}
			else
			{
				MessageBox.Show("Get New Slog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_DownloadHistory_Click(object sender, EventArgs e)
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			List<DateTime> dateTimeList = GetDateTimeList();
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj = false;
			obj2 = dateTimeList;
			if (!deviceConnection.GetProperty((DeviceProperty)18, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get All Slog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			if ((int)obj2 == 0)
			{
				lvw_SLogList.Items.Clear();
				return;
			}
			List<bool> list = new List<bool>();
			list.Add(item: false);
			list.Add(chk_NewFlag.Checked);
			obj = list;
			obj2 = dateTimeList;
			if (deviceConnection.GetProperty((DeviceProperty)12, obj, ref device, ref obj2))
			{
				List<Record> recordList = (List<Record>)obj2;
				AddRecordToListView(recordList);
			}
			else
			{
				MessageBox.Show("Get All Slog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_Clear_Click(object sender, EventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			if (deviceConnection.SetProperty((DeviceProperty)12, obj, device, obj2))
			{
				lvw_SLogList.Items.Clear();
				MessageBox.Show("Clear All Slog Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear All Slog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
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
		lvw_SLogList = new ListView();
		columnHeader1 = new ColumnHeader();
		columnHeader2 = new ColumnHeader();
		columnHeader3 = new ColumnHeader();
		columnHeader4 = new ColumnHeader();
		columnHeader5 = new ColumnHeader();
		columnHeader6 = new ColumnHeader();
		lbl_EndDate = new Label();
		lbl_BeginDate = new Label();
		btn_Clear = new Button();
		btn_DownloadHistory = new Button();
		btn_DownloadNew = new Button();
		chk_NewFlag = new CheckBox();
		dtp_End = new DateTimePicker();
		dtp_Begin = new DateTimePicker();
		((Control)this).SuspendLayout();
		lvw_SLogList.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[6] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
		lvw_SLogList.FullRowSelect = true;
		lvw_SLogList.GridLines = true;
		lvw_SLogList.HideSelection = false;
		((Control)lvw_SLogList).Location = new Point(12, 12);
		lvw_SLogList.MultiSelect = false;
		((Control)lvw_SLogList).Name = "lvw_SLogList";
		((Control)lvw_SLogList).Size = new Size(637, 354);
		((Control)lvw_SLogList).TabIndex = 0;
		lvw_SLogList.UseCompatibleStateImageBehavior = false;
		lvw_SLogList.View = (View)1;
		columnHeader1.Text = "SN";
		columnHeader2.Text = "Administrator ID";
		columnHeader2.TextAlign = (HorizontalAlignment)2;
		columnHeader2.Width = 120;
		columnHeader3.Text = "UserID";
		columnHeader3.TextAlign = (HorizontalAlignment)2;
		columnHeader3.Width = 120;
		columnHeader4.Text = "Operation";
		columnHeader4.TextAlign = (HorizontalAlignment)2;
		columnHeader4.Width = 120;
		columnHeader5.Text = "Enroll";
		columnHeader5.TextAlign = (HorizontalAlignment)2;
		columnHeader6.Text = "Datetime";
		columnHeader6.TextAlign = (HorizontalAlignment)2;
		columnHeader6.Width = 150;
		((Control)lbl_EndDate).AutoSize = true;
		((Control)lbl_EndDate).Location = new Point(237, 378);
		((Control)lbl_EndDate).Name = "lbl_EndDate";
		((Control)lbl_EndDate).Size = new Size(23, 12);
		((Control)lbl_EndDate).TabIndex = 7;
		((Control)lbl_EndDate).Text = "To:";
		lbl_EndDate.TextAlign = (ContentAlignment)32;
		((Control)lbl_BeginDate).AutoSize = true;
		((Control)lbl_BeginDate).Location = new Point(12, 378);
		((Control)lbl_BeginDate).Name = "lbl_BeginDate";
		((Control)lbl_BeginDate).Size = new Size(35, 12);
		((Control)lbl_BeginDate).TabIndex = 1;
		((Control)lbl_BeginDate).Text = "From:";
		lbl_BeginDate.TextAlign = (ContentAlignment)32;
		((Control)btn_Clear).Location = new Point(550, 401);
		((Control)btn_Clear).Name = "btn_Clear";
		((Control)btn_Clear).Size = new Size(99, 23);
		((Control)btn_Clear).TabIndex = 15;
		((Control)btn_Clear).Text = "Clear All Slog";
		((ButtonBase)btn_Clear).UseVisualStyleBackColor = true;
		((Control)btn_Clear).Click += btn_Clear_Click;
		((Control)btn_DownloadHistory).Location = new Point(441, 402);
		((Control)btn_DownloadHistory).Name = "btn_DownloadHistory";
		((Control)btn_DownloadHistory).Size = new Size(103, 23);
		((Control)btn_DownloadHistory).TabIndex = 14;
		((Control)btn_DownloadHistory).Text = "Get All Slog";
		((ButtonBase)btn_DownloadHistory).UseVisualStyleBackColor = true;
		((Control)btn_DownloadHistory).Click += btn_DownloadHistory_Click;
		((Control)btn_DownloadNew).Location = new Point(332, 402);
		((Control)btn_DownloadNew).Name = "btn_DownloadNew";
		((Control)btn_DownloadNew).Size = new Size(103, 23);
		((Control)btn_DownloadNew).TabIndex = 13;
		((Control)btn_DownloadNew).Text = "Get New Slog";
		((ButtonBase)btn_DownloadNew).UseVisualStyleBackColor = true;
		((Control)btn_DownloadNew).Click += btn_DownloadNew_Click;
		((Control)chk_NewFlag).AutoSize = true;
		((Control)chk_NewFlag).Location = new Point(456, 377);
		((Control)chk_NewFlag).Name = "chk_NewFlag";
		((Control)chk_NewFlag).Size = new Size(162, 16);
		((Control)chk_NewFlag).TabIndex = 20;
		((Control)chk_NewFlag).Text = "Clear New Slog Position";
		((ButtonBase)chk_NewFlag).UseVisualStyleBackColor = true;
		dtp_End.CustomFormat = "yyyy-MM-dd";
		dtp_End.Format = (DateTimePickerFormat)8;
		((Control)dtp_End).Location = new Point(266, 372);
		((Control)dtp_End).Name = "dtp_End";
		((Control)dtp_End).Size = new Size(120, 21);
		((Control)dtp_End).TabIndex = 19;
		dtp_Begin.CustomFormat = "yyyy-MM-dd";
		dtp_Begin.Format = (DateTimePickerFormat)8;
		((Control)dtp_Begin).Location = new Point(53, 372);
		((Control)dtp_Begin).Name = "dtp_Begin";
		((Control)dtp_Begin).Size = new Size(120, 21);
		((Control)dtp_Begin).TabIndex = 18;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(661, 440);
		((Control)this).Controls.Add((Control)(object)chk_NewFlag);
		((Control)this).Controls.Add((Control)(object)dtp_End);
		((Control)this).Controls.Add((Control)(object)dtp_Begin);
		((Control)this).Controls.Add((Control)(object)btn_DownloadNew);
		((Control)this).Controls.Add((Control)(object)btn_DownloadHistory);
		((Control)this).Controls.Add((Control)(object)btn_Clear);
		((Control)this).Controls.Add((Control)(object)lbl_EndDate);
		((Control)this).Controls.Add((Control)(object)lbl_BeginDate);
		((Control)this).Controls.Add((Control)(object)lvw_SLogList);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "SLogForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Slog Manage";
		((Form)this).Load += SLogForm_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
