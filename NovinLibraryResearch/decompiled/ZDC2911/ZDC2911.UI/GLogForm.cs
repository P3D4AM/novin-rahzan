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

public class GLogForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private IContainer components = null;

	private ListView lvw_GLogList;

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

	private DateTimePicker dtp_Begin;

	private DateTimePicker dtp_End;

	private CheckBox chk_NewFlag;

	public GLogForm(DeviceCommEty deviceEty)
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
		lvw_GLogList.Items.Clear();
		int num = 1;
		foreach (Record record in recordList)
		{
			string text = ConvertObject.GLogType(record.Verify);
			string text2 = ConvertObject.IOMode(record.Action);
			ListViewItem val = new ListViewItem(new string[6]
			{
				num.ToString(),
				record.DN.ToString(),
				record.DIN.ToString(),
				text,
				text2,
				record.Clock.ToString("yyyy-MM-dd HH:mm:ss")
			});
			lvw_GLogList.Items.Add(val);
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
			if (!deviceConnection.GetProperty((DeviceProperty)17, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get New Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			if ((int)obj2 == 0)
			{
				lvw_GLogList.Items.Clear();
				return;
			}
			List<bool> list = new List<bool>();
			list.Add(item: true);
			list.Add(chk_NewFlag.Checked);
			obj = list;
			obj2 = dateTimeList;
			if (deviceConnection.GetProperty((DeviceProperty)13, obj, ref device, ref obj2))
			{
				List<Record> recordList = (List<Record>)obj2;
				AddRecordToListView(recordList);
			}
			else
			{
				MessageBox.Show("Get New Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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
			if (!deviceConnection.GetProperty((DeviceProperty)17, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get All Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			if ((int)obj2 == 0)
			{
				lvw_GLogList.Items.Clear();
				return;
			}
			List<bool> list = new List<bool>();
			list.Add(item: false);
			list.Add(chk_NewFlag.Checked);
			obj = list;
			obj2 = dateTimeList;
			if (deviceConnection.GetProperty((DeviceProperty)13, obj, ref device, ref obj2))
			{
				List<Record> recordList = (List<Record>)obj2;
				AddRecordToListView(recordList);
			}
			else
			{
				MessageBox.Show("Get All Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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
			if (deviceConnection.SetProperty((DeviceProperty)13, obj, device, obj2))
			{
				lvw_GLogList.Items.Clear();
				MessageBox.Show("Clear All Glog Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear All Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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
		lvw_GLogList = new ListView();
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
		dtp_Begin = new DateTimePicker();
		dtp_End = new DateTimePicker();
		chk_NewFlag = new CheckBox();
		((Control)this).SuspendLayout();
		lvw_GLogList.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[6] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
		lvw_GLogList.FullRowSelect = true;
		lvw_GLogList.GridLines = true;
		lvw_GLogList.HideSelection = false;
		((Control)lvw_GLogList).Location = new Point(12, 12);
		lvw_GLogList.MultiSelect = false;
		((Control)lvw_GLogList).Name = "lvw_GLogList";
		((Control)lvw_GLogList).Size = new Size(637, 354);
		((Control)lvw_GLogList).TabIndex = 0;
		lvw_GLogList.UseCompatibleStateImageBehavior = false;
		lvw_GLogList.View = (View)1;
		columnHeader1.Text = "SN";
		columnHeader2.Text = "Device No.";
		columnHeader2.TextAlign = (HorizontalAlignment)2;
		columnHeader2.Width = 120;
		columnHeader3.Text = "UserID";
		columnHeader3.TextAlign = (HorizontalAlignment)2;
		columnHeader3.Width = 120;
		columnHeader4.Text = "Verify Mode";
		columnHeader4.TextAlign = (HorizontalAlignment)2;
		columnHeader4.Width = 80;
		columnHeader5.Text = "Action";
		columnHeader5.TextAlign = (HorizontalAlignment)2;
		columnHeader5.Width = 80;
		columnHeader6.Text = "Datetime";
		columnHeader6.TextAlign = (HorizontalAlignment)2;
		columnHeader6.Width = 150;
		((Control)lbl_EndDate).AutoSize = true;
		((Control)lbl_EndDate).Location = new Point(239, 378);
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
		((Control)btn_Clear).Text = "Clear All Glog";
		((ButtonBase)btn_Clear).UseVisualStyleBackColor = true;
		((Control)btn_Clear).Click += btn_Clear_Click;
		((Control)btn_DownloadHistory).Location = new Point(441, 401);
		((Control)btn_DownloadHistory).Name = "btn_DownloadHistory";
		((Control)btn_DownloadHistory).Size = new Size(103, 23);
		((Control)btn_DownloadHistory).TabIndex = 14;
		((Control)btn_DownloadHistory).Text = "Get All Glog";
		((ButtonBase)btn_DownloadHistory).UseVisualStyleBackColor = true;
		((Control)btn_DownloadHistory).Click += btn_DownloadHistory_Click;
		((Control)btn_DownloadNew).Location = new Point(332, 401);
		((Control)btn_DownloadNew).Name = "btn_DownloadNew";
		((Control)btn_DownloadNew).Size = new Size(103, 23);
		((Control)btn_DownloadNew).TabIndex = 13;
		((Control)btn_DownloadNew).Text = "Get New Glog";
		((ButtonBase)btn_DownloadNew).UseVisualStyleBackColor = true;
		((Control)btn_DownloadNew).Click += btn_DownloadNew_Click;
		dtp_Begin.CustomFormat = "yyyy-MM-dd";
		dtp_Begin.Format = (DateTimePickerFormat)8;
		((Control)dtp_Begin).Location = new Point(53, 372);
		((Control)dtp_Begin).Name = "dtp_Begin";
		((Control)dtp_Begin).Size = new Size(120, 21);
		((Control)dtp_Begin).TabIndex = 16;
		dtp_End.CustomFormat = "yyyy-MM-dd";
		dtp_End.Format = (DateTimePickerFormat)8;
		((Control)dtp_End).Location = new Point(268, 372);
		((Control)dtp_End).Name = "dtp_End";
		((Control)dtp_End).Size = new Size(120, 21);
		((Control)dtp_End).TabIndex = 16;
		((Control)chk_NewFlag).AutoSize = true;
		((Control)chk_NewFlag).Location = new Point(456, 377);
		((Control)chk_NewFlag).Name = "chk_NewFlag";
		((Control)chk_NewFlag).Size = new Size(162, 16);
		((Control)chk_NewFlag).TabIndex = 17;
		((Control)chk_NewFlag).Text = "Clear New Glog Position";
		((ButtonBase)chk_NewFlag).UseVisualStyleBackColor = true;
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
		((Control)this).Controls.Add((Control)(object)lvw_GLogList);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "GLogForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Glog Manage";
		((Form)this).Load += SLogForm_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
