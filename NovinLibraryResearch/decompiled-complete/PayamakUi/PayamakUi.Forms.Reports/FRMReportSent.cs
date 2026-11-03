using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;
using PayamakUi.Properties;

namespace PayamakUi.Forms.Reports;

public class FRMReportSent : AccountingBaseForm
{
	private PayamakClientHubInstances _instance;

	private PayamakFilterModel _payamakFilterModel;

	private DataView DVG = new DataView();

	private IContainer components;

	internal PanelEx PanelEx1;

	internal GroupPanel GroupPanel2;

	internal Label LabelX10;

	internal Label LabelX9;

	internal Label LabelX8;

	internal Label LabelX7;

	internal Label LabelX6;

	internal Label LabelX5;

	internal Label LblBlackList;

	internal Label LblNaresideM;

	internal Label LblResideM;

	internal PictureBox PictureBox6;

	internal PictureBox PictureBox5;

	internal PictureBox PictureBox4;

	internal PictureBox PictureBox3;

	internal PictureBox PictureBox2;

	internal PictureBox PictureBox1;

	internal Label LblUnKnown;

	internal Label LblNotDeliverd;

	internal Label LblDelivered;

	internal Label LBLTotal;

	internal Label LabelX3;

	internal DataGridViewX DGrid;

	internal ButtonX BTNExit;

	internal ButtonX BTNDelete;

	internal ButtonX BTNForward;

	internal Panel Panel1;

	internal TextBoxX TextBoxX1;

	internal ContextMenuStrip ContextMenue;

	internal ToolStripMenuItem انتخابToolStripMenuItem;

	internal ToolStripMenuItem CMASelectAll;

	internal ToolStripMenuItem CMASelectNone;

	internal ToolStripMenuItem CMASelectReverce;

	internal BindingSource ViewSMSSentBindingSource;

	internal ToolStripMenuItem ارسالپیامبهدیگریToolStripMenuItem;

	internal ToolStripMenuItem حذفپیامانتخابیToolStripMenuItem;

	internal ToolStripMenuItem ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem;

	internal ToolStripMenuItem ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem;

	internal PictureBox PictureBox8;

	internal PictureBox PictureBox7;

	internal Label LabelX4;

	internal Label LblCancel;

	internal Label LabelX1;

	internal Label Lblwaiting;

	internal ButtonX BTNReply;

	internal PictureBox pictureBox9;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewImageColumn Img;

	private DataGridViewTextBoxColumn DeliverySTShort;

	private DataGridViewTextBoxColumn SendDateShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dgTime;

	private DataGridViewTextBoxColumn SenderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGNumber;

	private DataGridViewTextBoxColumn DeliveryST;

	private DataGridViewTextBoxColumn DGMessage;

	private DataGridViewTextBoxColumn SendDateMDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn DeliveryRequestedDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn RefrencedIdsDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn RefrenseAccountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn RefrencedIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DateSHDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn CustomerIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn CustomerNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn FixedMessageDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn STDataGridViewCheckBoxColumn;

	internal ButtonX BTNSelectAll;

	public FRMReportSent(string contextGuid, PayamakFilterModel payamakFilterModel, Font font)
		: base(contextGuid)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		((Control)this).KeyDown += new KeyEventHandler(FRMReportSent_KeyDown);
		try
		{
			InitializeComponent();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			_payamakFilterModel = payamakFilterModel;
			((Control)this).Text = "گزارش پیامک های ارسال شده";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 34);
		}
	}

	public FRMReportSent(string contextGuid)
		: base(contextGuid)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		((Control)this).KeyDown += new KeyEventHandler(FRMReportSent_KeyDown);
		try
		{
			InitializeComponent();
			((Control)this).Text = "گزارش پیامک های ارسال شده";
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 46);
		}
	}

	public async Task LoadDataAsync()
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await _instance.SmsSent.GetAllAsync(_payamakFilterModel.MobG, _payamakFilterModel.Status, _payamakFilterModel.Text, _payamakFilterModel.MobF, _payamakFilterModel.DateM1, _payamakFilterModel.DateM2);
			((DataGridView)DGrid).DataSource = returnedSaveFuncInfoWithValue.value;
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 59);
		}
	}

	private string Where_Add(string Where, string New_Condition)
	{
		try
		{
			if (string.IsNullOrEmpty(Where.Trim()))
			{
				return " ( " + New_Condition + " ) ";
			}
			return Where + " and ( " + New_Condition + " ) ";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Where_Add", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 140);
			return "";
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			if (e.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				int num = int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
				int i = 0;
				for (int num2 = ((DataGridView)DGrid).RowCount - 1; i <= num2; i++)
				{
					if (num == int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()))
					{
						if (!bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value?.ToString() ?? "False"))
						{
							((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = true;
						}
						else
						{
							((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = false;
						}
					}
				}
			}
			if (((DataGridView)DGrid).RowCount != 0)
			{
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString().FixString().Trim();
				((Control)TextBoxX1).Text = text;
			}
			else
			{
				((Control)TextBoxX1).Text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 170);
		}
	}

	private void CMASelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 181);
		}
	}

	private void CMASelectNone_Click(object sender, EventArgs e)
	{
		try
		{
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 191);
		}
	}

	private void CMASelectReverce_Click(object sender, EventArgs e)
	{
		try
		{
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				if (!bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()))
				{
					((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = true;
				}
				else
				{
					((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectReverce_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 204);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BTNDelete).Enabled || !((Control)BTNDelete).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			List<int> list = new List<int>();
			if ((int)MessageBox.Show("مایل به حذف پیام ارسالی انتخاب شده هستید ؟".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) == 6)
			{
				int i = 0;
				for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
				{
					if (Convert.ToBoolean(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value) && Convert.ToBoolean(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value))
					{
						list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
					}
				}
				_instance.SmsSent.DeleteListAsync(list, default(CancellationToken));
			}
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 226);
		}
	}

	private void BTNForward_Click(object sender, EventArgs e)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!((Control)BTNForward).Enabled || !((Control)BTNForward).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount != 0)
			{
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString().FixString().Trim();
				if (!string.IsNullOrEmpty(text))
				{
					((Form)new FRMSend(base.ContextGuid)
					{
						Message = text
					}).ShowDialog((IWin32Window)(object)this);
				}
				RefreshLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNForward_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 247);
		}
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e != null && e.ColumnIndex >= 0 && e.RowIndex >= 0 && ((DataGridView)DGrid).RowCount != 0 && ((DataGridView)DGrid).ColumnCount >= 0 && ((DataGridView)DGrid).CurrentCell != null)
			{
				if (((DataGridView)DGrid).RowCount != 0)
				{
					string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString().FixString().Trim();
					((Control)TextBoxX1).Text = text;
				}
				else
				{
					((Control)TextBoxX1).Text = "";
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 270);
		}
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Hide();
			((Control)this).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 280);
		}
	}

	private void RefreshLables()
	{
		try
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			if (((DataGridView)DGrid).RowCount > 0)
			{
				int i = 0;
				for (int num9 = ((DataGridView)DGrid).RowCount - 1; i <= num9; i++)
				{
					switch (short.Parse(((DataGridView)DGrid)[((DataGridViewBand)DeliverySTShort).Index, i].Value.ToString()))
					{
					case 42:
						num++;
						break;
					case 41:
						num3++;
						break;
					case 40:
						num2++;
						break;
					case 43:
						num4++;
						break;
					case 44:
						num6++;
						break;
					case 45:
						num5++;
						break;
					case 47:
						num8++;
						break;
					case 49:
						num7++;
						break;
					}
				}
			}
			((Control)LBLTotal).Text = ((DataGridView)DGrid).RowCount.ToString();
			((Control)LblDelivered).Text = num3.ToString();
			((Control)LblNotDeliverd).Text = num2.ToString();
			((Control)LblUnKnown).Text = num.ToString();
			((Control)LblBlackList).Text = num6.ToString();
			((Control)LblResideM).Text = num4.ToString();
			((Control)LblNaresideM).Text = num5.ToString();
			((Control)Lblwaiting).Text = num8.ToString();
			((Control)LblCancel).Text = num7.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RefreshLables", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 326);
		}
	}

	private void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString().FixString().Trim();
			((Control)TextBoxX1).Text = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 336);
		}
	}

	private void FRMReportSent_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if ((int)keyCode == 46)
			{
				BTNDelete.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMReportSent_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 362);
		}
	}

	private void حذفپیامانتخابیToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			BTNDelete.PerformClick();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "حذفپیامانتخابیToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 371);
		}
	}

	private void ارسالپیامبهدیگریToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			BTNForward.PerformClick();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ارسالپیامبهدیگریToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 380);
		}
	}

	private async void ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			int BlackList = 0;
			List<int> delList = new List<int>();
			int i = 0;
			for (int loopTo = ((DataGridView)DGrid).RowCount - 1; i <= loopTo; i++)
			{
				if (((DataGridView)DGrid)[((DataGridViewBand)DeliveryST).Index, i].Value.ToString() == "44")
				{
					BlackList++;
					SmsOutBox item = new SmsOutBox(((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, i].Value.ToString(), ((DataGridView)DGrid)[((DataGridViewBand)DGNumber).Index, i].Value.ToString(), 0);
					await _instance.SmsOutBox.SaveAsync(item);
				}
			}
			if (BlackList == 0)
			{
				MessageBox.Show("در این لیست  پیام لیست سیاهی جهت ارسال وجود ندارد ", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				return;
			}
			if ((int)MessageBox.Show(" تعداد  " + BlackList + " عدد پیامک لیست سیاه جهت ارسال از طریق سیم کارت در صف ارسال قرار گرفت.آیا مایل به حذف  شماره های لیست سیاه از لیست هستید ", "هشدار", (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) == 6)
			{
				int j = 0;
				for (int num = ((DataGridView)DGrid).RowCount - 1; j <= num; j++)
				{
					if (((DataGridView)DGrid)[((DataGridViewBand)DeliveryST).Index, j].Value.ToString() == "44")
					{
						delList.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, j].Value.ToString()));
					}
				}
				_instance.SmsSent.DeleteListAsync(delList, default(CancellationToken));
			}
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 421);
		}
	}

	private async void ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			int Count = 0;
			List<int> delList = new List<int>();
			int i = 0;
			for (int loopTo = ((DataGridView)DGrid).RowCount - 1; i <= loopTo; i++)
			{
				if (!bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()) && bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()))
				{
					Count++;
					SmsOutBox item = new SmsOutBox(((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, i].Value.ToString(), ((DataGridView)DGrid)[((DataGridViewBand)DGNumber).Index, i].Value.ToString(), 0);
					await _instance.SmsOutBox.SaveAsync(item);
				}
			}
			if (Count == 0)
			{
				MessageBox.Show("در این لیست  پیام انتخاب شده ای جهت ارسال وجود ندارد ".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				return;
			}
			if ((int)MessageBox.Show(" تعداد ".GetTranslate() + " " + Count + " " + "عدد پیامک انتخابی جهت ارسال از طریق سیم کارت در صف ارسال قرار گرفت.آیا مایل به حذف این شماره ها از لیست هستید ".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) == 6)
			{
				int j = 0;
				for (int num = ((DataGridView)DGrid).RowCount - 1; j <= num; j++)
				{
					if (!bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, j].Value.ToString()) && bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, j].Value.ToString()))
					{
						delList.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, j].Value.ToString()));
					}
				}
				_instance.SmsSent.DeleteListAsync(delList, default(CancellationToken));
			}
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 469);
		}
	}

	private async void FRMReportSent_Load(object sender, EventArgs e)
	{
		await LoadDataAsync();
	}

	private void BTNReply_Click(object sender, EventArgs e)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount <= 0)
			{
				return;
			}
			string text = "";
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				if (((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value != null && bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()))
				{
					if (!string.IsNullOrEmpty(text.Trim()))
					{
						text += ";";
					}
					text += ((DataGridView)DGrid)[((DataGridViewBand)DGNumber).Index, i].Value.ToString();
				}
			}
			if ((int)((Form)new FRMSend(base.ContextGuid)
			{
				Mobiles = text
			}).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Task.Run(() => LoadDataAsync());
			}
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNReply_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 506);
		}
	}

	private void BTNSelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			bool flag = true;
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				if (((DataGridViewBand)((DataGridView)DGrid).Rows[i]).Visible)
				{
					object value = ((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value;
					if (value == null || !(bool)value)
					{
						flag = false;
					}
				}
			}
			bool flag2 = !flag;
			int j = 0;
			for (int num2 = ((DataGridView)DGrid).RowCount - 1; j <= num2; j++)
			{
				if (((DataGridViewBand)((DataGridView)DGrid).Rows[j]).Visible)
				{
					((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, j].Value = flag2;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNSelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportSent.cs", 538);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Expected O, but got Unknown
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d47: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1081: Unknown result type (might be due to invalid IL or missing references)
		//IL_108b: Expected O, but got Unknown
		//IL_10af: Unknown result type (might be due to invalid IL or missing references)
		//IL_1272: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ed: Expected O, but got Unknown
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1399: Unknown result type (might be due to invalid IL or missing references)
		//IL_140e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Expected O, but got Unknown
		//IL_1439: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1560: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Expected O, but got Unknown
		//IL_168a: Unknown result type (might be due to invalid IL or missing references)
		//IL_170c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1716: Expected O, but got Unknown
		//IL_1734: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c3: Expected O, but got Unknown
		//IL_17e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1864: Unknown result type (might be due to invalid IL or missing references)
		//IL_186e: Expected O, but got Unknown
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1910: Unknown result type (might be due to invalid IL or missing references)
		//IL_191a: Expected O, but got Unknown
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Expected O, but got Unknown
		//IL_19e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2068: Unknown result type (might be due to invalid IL or missing references)
		//IL_20de: Unknown result type (might be due to invalid IL or missing references)
		//IL_2152: Unknown result type (might be due to invalid IL or missing references)
		//IL_215c: Expected O, but got Unknown
		//IL_217c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2365: Unknown result type (might be due to invalid IL or missing references)
		//IL_236f: Expected O, but got Unknown
		//IL_23eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_245c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2466: Expected O, but got Unknown
		//IL_2473: Unknown result type (might be due to invalid IL or missing references)
		//IL_247d: Expected O, but got Unknown
		//IL_248a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2494: Expected O, but got Unknown
		//IL_24a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ab: Expected O, but got Unknown
		//IL_2a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aae: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMReportSent));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		ContextMenue = new ContextMenuStrip(components);
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		CMASelectAll = new ToolStripMenuItem();
		CMASelectNone = new ToolStripMenuItem();
		CMASelectReverce = new ToolStripMenuItem();
		حذفپیامانتخابیToolStripMenuItem = new ToolStripMenuItem();
		ارسالپیامبهدیگریToolStripMenuItem = new ToolStripMenuItem();
		ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem = new ToolStripMenuItem();
		ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem = new ToolStripMenuItem();
		PanelEx1 = new PanelEx();
		BTNSelectAll = new ButtonX();
		BTNReply = new ButtonX();
		Panel1 = new Panel();
		TextBoxX1 = new TextBoxX();
		BTNDelete = new ButtonX();
		BTNForward = new ButtonX();
		BTNExit = new ButtonX();
		GroupPanel2 = new GroupPanel();
		pictureBox9 = new PictureBox();
		LabelX4 = new Label();
		LblCancel = new Label();
		LabelX1 = new Label();
		Lblwaiting = new Label();
		PictureBox8 = new PictureBox();
		PictureBox7 = new PictureBox();
		LabelX10 = new Label();
		LabelX9 = new Label();
		LabelX8 = new Label();
		LabelX7 = new Label();
		LabelX6 = new Label();
		LabelX5 = new Label();
		LblBlackList = new Label();
		LblNaresideM = new Label();
		LblResideM = new Label();
		PictureBox6 = new PictureBox();
		PictureBox5 = new PictureBox();
		PictureBox4 = new PictureBox();
		PictureBox3 = new PictureBox();
		PictureBox2 = new PictureBox();
		PictureBox1 = new PictureBox();
		LblUnKnown = new Label();
		LblNotDeliverd = new Label();
		LblDelivered = new Label();
		LBLTotal = new Label();
		LabelX3 = new Label();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		Img = new DataGridViewImageColumn();
		DeliverySTShort = new DataGridViewTextBoxColumn();
		SendDateShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dgTime = new DataGridViewTextBoxColumn();
		SenderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGNumber = new DataGridViewTextBoxColumn();
		DeliveryST = new DataGridViewTextBoxColumn();
		DGMessage = new DataGridViewTextBoxColumn();
		SendDateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DeliveryRequestedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		RefrencedIdsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		RefrenseAccountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		RefrencedIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		DateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DateSHDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		CustomerIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		CustomerNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		FixedMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		STDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		ViewSMSSentBindingSource = new BindingSource(components);
		((Control)ContextMenue).SuspendLayout();
		((Control)PanelEx1).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((Control)GroupPanel2).SuspendLayout();
		((ISupportInitialize)pictureBox9).BeginInit();
		((ISupportInitialize)PictureBox8).BeginInit();
		((ISupportInitialize)PictureBox7).BeginInit();
		((ISupportInitialize)PictureBox6).BeginInit();
		((ISupportInitialize)PictureBox5).BeginInit();
		((ISupportInitialize)PictureBox4).BeginInit();
		((ISupportInitialize)PictureBox3).BeginInit();
		((ISupportInitialize)PictureBox2).BeginInit();
		((ISupportInitialize)PictureBox1).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)ViewSMSSentBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((ToolStrip)ContextMenue).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)انتخابToolStripMenuItem,
			(ToolStripItem)حذفپیامانتخابیToolStripMenuItem,
			(ToolStripItem)ارسالپیامبهدیگریToolStripMenuItem,
			(ToolStripItem)ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem,
			(ToolStripItem)ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem
		});
		((Control)ContextMenue).Name = "ContextMenue";
		((Control)ContextMenue).RightToLeft = (RightToLeft)1;
		((Control)ContextMenue).Size = new Size(295, 114);
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)CMASelectAll,
			(ToolStripItem)CMASelectNone,
			(ToolStripItem)CMASelectReverce
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(294, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)انتخابToolStripMenuItem).Click += CMASelectAll_Click;
		((ToolStripItem)CMASelectAll).Name = "CMASelectAll";
		((ToolStripItem)CMASelectAll).Size = new Size(184, 22);
		((ToolStripItem)CMASelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)CMASelectNone).Name = "CMASelectNone";
		((ToolStripItem)CMASelectNone).Size = new Size(184, 22);
		((ToolStripItem)CMASelectNone).Text = "[انتخاب هیچکدام]";
		((ToolStripItem)CMASelectReverce).Name = "CMASelectReverce";
		((ToolStripItem)CMASelectReverce).Size = new Size(184, 22);
		((ToolStripItem)CMASelectReverce).Text = "[معکوس کردن انتخاب]";
		((ToolStripItem)حذفپیامانتخابیToolStripMenuItem).Name = "حذفپیامانتخابیToolStripMenuItem";
		((ToolStripItem)حذفپیامانتخابیToolStripMenuItem).Size = new Size(294, 22);
		((ToolStripItem)حذفپیامانتخابیToolStripMenuItem).Text = "[حذف پیامک انتخابی]";
		((ToolStripItem)حذفپیامانتخابیToolStripMenuItem).Click += حذفپیامانتخابیToolStripMenuItem_Click;
		((ToolStripItem)ارسالپیامبهدیگریToolStripMenuItem).Name = "ارسالپیامبهدیگریToolStripMenuItem";
		((ToolStripItem)ارسالپیامبهدیگریToolStripMenuItem).Size = new Size(294, 22);
		((ToolStripItem)ارسالپیامبهدیگریToolStripMenuItem).Text = "ارسال پیامک  انتخابی به دیگری";
		((ToolStripItem)ارسالپیامبهدیگریToolStripMenuItem).Click += ارسالپیامبهدیگریToolStripMenuItem_Click;
		((ToolStripItem)ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem).Name = "ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem";
		((ToolStripItem)ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem).Size = new Size(294, 22);
		((ToolStripItem)ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem).Text = "[ارسال پیامک انتخابی از طریق سیم کارت]";
		((ToolStripItem)ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem).Click += ارسالپیامانتخابیازطریقسیمکارتToolStripMenuItem_Click;
		((ToolStripItem)ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem).Name = "ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem";
		((ToolStripItem)ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem).Size = new Size(294, 22);
		((ToolStripItem)ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem).Text = "[ارسال پیامک لیست سیاه از طریق سیم کارت]";
		((ToolStripItem)ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem).Click += ارسالپیاملیستسیاهازطریقسیمکارتToolStripMenuItem_Click;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)BTNSelectAll);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNReply);
		((Control)PanelEx1).Controls.Add((Control)(object)Panel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNForward);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel2);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(796, 396);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BTNSelectAll).AccessibleRole = (AccessibleRole)43;
		((Control)BTNSelectAll).Anchor = (AnchorStyles)10;
		BTNSelectAll.ColorTable = eButtonColor.Office2007WithBackground;
		BTNSelectAll.Image = (Image)(object)Resources.accept_page1;
		BTNSelectAll.ImagePosition = eImagePosition.Right;
		((Control)BTNSelectAll).Location = new Point(563, 356);
		((Control)BTNSelectAll).Name = "BTNSelectAll";
		((Control)BTNSelectAll).Size = new Size(110, 28);
		BTNSelectAll.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNSelectAll).TabIndex = 38;
		((Control)BTNSelectAll).Text = "[انتخاب همه]";
		((Control)BTNSelectAll).Click += BTNSelectAll_Click;
		((Control)BTNReply).AccessibleRole = (AccessibleRole)43;
		((Control)BTNReply).Anchor = (AnchorStyles)10;
		BTNReply.ColorTable = eButtonColor.Office2007WithBackground;
		BTNReply.Image = (Image)componentResourceManager.GetObject("BTNReply.Image");
		((Control)BTNReply).Location = new Point(719, 356);
		((Control)BTNReply).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNReply).Name = "BTNReply";
		((Control)BTNReply).Size = new Size(30, 28);
		((Control)BTNReply).TabIndex = 34;
		((Control)BTNReply).TabStop = false;
		BTNReply.Tooltip = "[پاسخ دادن به پیام]";
		((Control)BTNReply).Click += BTNReply_Click;
		((Control)Panel1).Anchor = (AnchorStyles)14;
		Panel1.BorderStyle = (BorderStyle)1;
		((Control)Panel1).Controls.Add((Control)(object)TextBoxX1);
		((Control)Panel1).Location = new Point(8, 225);
		((Control)Panel1).Margin = new Padding(4, 3, 4, 3);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(390, 122);
		((Control)Panel1).TabIndex = 33;
		((Control)TextBoxX1).BackColor = Color.LightSteelBlue;
		TextBoxX1.Border.Class = "TextBoxBorder";
		TextBoxX1.Border.CornerType = eCornerType.Square;
		((Control)TextBoxX1).Dock = (DockStyle)5;
		((Control)TextBoxX1).Location = new Point(0, 0);
		((Control)TextBoxX1).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TextBoxX1).Multiline = true;
		((Control)TextBoxX1).Name = "TextBoxX1";
		((TextBoxBase)TextBoxX1).ReadOnly = true;
		((TextBox)TextBoxX1).ScrollBars = (ScrollBars)3;
		((Control)TextBoxX1).Size = new Size(388, 120);
		((Control)TextBoxX1).TabIndex = 0;
		TextBoxX1.WatermarkColor = Color.Empty;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BTNDelete).Anchor = (AnchorStyles)10;
		BTNDelete.ColorTable = eButtonColor.Office2007WithBackground;
		BTNDelete.Image = (Image)componentResourceManager.GetObject("BTNDelete.Image");
		((Control)BTNDelete).Location = new Point(681, 356);
		((Control)BTNDelete).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).Size = new Size(30, 28);
		((Control)BTNDelete).TabIndex = 12;
		((Control)BTNDelete).TabStop = false;
		BTNDelete.Tooltip = "[حذف پیام ارسالی]";
		((Control)BTNDelete).Click += BTNDelete_Click;
		((Control)BTNForward).AccessibleRole = (AccessibleRole)43;
		((Control)BTNForward).Anchor = (AnchorStyles)10;
		BTNForward.ColorTable = eButtonColor.Office2007WithBackground;
		BTNForward.Image = (Image)componentResourceManager.GetObject("BTNForward.Image");
		((Control)BTNForward).Location = new Point(756, 356);
		((Control)BTNForward).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNForward).Name = "BTNForward";
		((Control)BTNForward).Size = new Size(30, 28);
		((Control)BTNForward).TabIndex = 11;
		((Control)BTNForward).TabStop = false;
		BTNForward.Tooltip = "[ارسال پیام به دیگری]";
		((Control)BTNForward).Click += BTNForward_Click;
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTNExit.Image = (Image)componentResourceManager.GetObject("BTNExit.Image");
		((Control)BTNExit).Location = new Point(8, 356);
		((Control)BTNExit).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(30, 28);
		((Control)BTNExit).TabIndex = 10;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "[خروج]";
		((Control)BTNExit).Click += BTNExit_Click;
		((Control)GroupPanel2).Anchor = (AnchorStyles)10;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)pictureBox9);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX4);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblCancel);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel2).Controls.Add((Control)(object)Lblwaiting);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox8);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox7);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX10);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX9);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX8);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX7);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX6);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX5);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblBlackList);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblNaresideM);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblResideM);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox6);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox5);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox4);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox3);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox2);
		((Control)GroupPanel2).Controls.Add((Control)(object)PictureBox1);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblUnKnown);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblNotDeliverd);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblDelivered);
		((Control)GroupPanel2).Controls.Add((Control)(object)LBLTotal);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX3);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)GroupPanel2).Location = new Point(404, 225);
		((Control)GroupPanel2).Margin = new Padding(4, 3, 4, 3);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(385, 122);
		GroupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel2.Style.BackColorGradientAngle = 90;
		GroupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderBottomWidth = 1;
		GroupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderLeftWidth = 1;
		GroupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderRightWidth = 1;
		GroupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderTopWidth = 1;
		GroupPanel2.Style.CornerDiameter = 4;
		GroupPanel2.Style.CornerType = eCornerType.Rounded;
		GroupPanel2.Style.TextAlignment = eStyleTextAlignment.Center;
		GroupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel2).TabIndex = 3;
		((Control)pictureBox9).Anchor = (AnchorStyles)9;
		pictureBox9.Image = (Image)(object)Resources.report_Sent;
		((Control)pictureBox9).Location = new Point(345, -2);
		((Control)pictureBox9).Margin = new Padding(4, 3, 4, 3);
		((Control)pictureBox9).Name = "pictureBox9";
		((Control)pictureBox9).Size = new Size(24, 24);
		pictureBox9.SizeMode = (PictureBoxSizeMode)4;
		pictureBox9.TabIndex = 35;
		pictureBox9.TabStop = false;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((Control)LabelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX4).Location = new Point(40, 73);
		((Control)LabelX4).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).RightToLeft = (RightToLeft)1;
		((Control)LabelX4).Size = new Size(122, 24);
		((Control)LabelX4).TabIndex = 33;
		((Control)LabelX4).Text = "[کنسل شده] :";
		LabelX4.TextAlign = (ContentAlignment)16;
		((Control)LblCancel).Anchor = (AnchorStyles)9;
		((Control)LblCancel).Location = new Point(7, 79);
		((Control)LblCancel).Margin = new Padding(4, 0, 4, 0);
		((Control)LblCancel).Name = "LblCancel";
		((Control)LblCancel).RightToLeft = (RightToLeft)0;
		((Control)LblCancel).Size = new Size(34, 14);
		((Control)LblCancel).TabIndex = 34;
		((Control)LblCancel).Text = "0";
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX1).Location = new Point(227, 88);
		((Control)LabelX1).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).RightToLeft = (RightToLeft)1;
		((Control)LabelX1).Size = new Size(118, 24);
		((Control)LabelX1).TabIndex = 31;
		((Control)LabelX1).Text = "[در انتظار تحویل]  :";
		LabelX1.TextAlign = (ContentAlignment)16;
		((Control)Lblwaiting).Anchor = (AnchorStyles)9;
		((Control)Lblwaiting).Location = new Point(197, 95);
		((Control)Lblwaiting).Margin = new Padding(4, 0, 4, 0);
		((Control)Lblwaiting).Name = "Lblwaiting";
		((Control)Lblwaiting).RightToLeft = (RightToLeft)0;
		((Control)Lblwaiting).Size = new Size(33, 18);
		((Control)Lblwaiting).TabIndex = 32;
		((Control)Lblwaiting).Text = "0";
		((Control)PictureBox8).Anchor = (AnchorStyles)9;
		PictureBox8.Image = (Image)(object)Resources.list_go1;
		((Control)PictureBox8).Location = new Point(346, 93);
		((Control)PictureBox8).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox8).Name = "PictureBox8";
		((Control)PictureBox8).Size = new Size(26, 20);
		PictureBox8.SizeMode = (PictureBoxSizeMode)1;
		PictureBox8.TabIndex = 30;
		PictureBox8.TabStop = false;
		((Control)PictureBox7).Anchor = (AnchorStyles)9;
		PictureBox7.Image = (Image)(object)Resources.cancel1;
		((Control)PictureBox7).Location = new Point(164, 73);
		((Control)PictureBox7).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox7).Name = "PictureBox7";
		((Control)PictureBox7).Size = new Size(24, 24);
		PictureBox7.SizeMode = (PictureBoxSizeMode)4;
		PictureBox7.TabIndex = 29;
		PictureBox7.TabStop = false;
		((Control)LabelX10).Anchor = (AnchorStyles)9;
		((Control)LabelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX10).Location = new Point(54, 49);
		((Control)LabelX10).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX10).Name = "LabelX10";
		((Control)LabelX10).RightToLeft = (RightToLeft)1;
		((Control)LabelX10).Size = new Size(108, 24);
		((Control)LabelX10).TabIndex = 12;
		((Control)LabelX10).Text = "[لیست سیاه] :";
		LabelX10.TextAlign = (ContentAlignment)16;
		((Control)LabelX9).Anchor = (AnchorStyles)9;
		((Control)LabelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX9).Location = new Point(30, 25);
		((Control)LabelX9).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX9).Name = "LabelX9";
		((Control)LabelX9).RightToLeft = (RightToLeft)1;
		((Control)LabelX9).Size = new Size(132, 24);
		((Control)LabelX9).TabIndex = 10;
		((Control)LabelX9).Text = "[نرسیده به مخابرات] :";
		LabelX9.TextAlign = (ContentAlignment)16;
		((Control)LabelX8).Anchor = (AnchorStyles)9;
		((Control)LabelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX8).Location = new Point(30, 1);
		((Control)LabelX8).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).RightToLeft = (RightToLeft)1;
		((Control)LabelX8).Size = new Size(132, 24);
		((Control)LabelX8).TabIndex = 8;
		((Control)LabelX8).Text = "[رسیده به مخابرات] :";
		LabelX8.TextAlign = (ContentAlignment)16;
		((Control)LabelX7).Anchor = (AnchorStyles)9;
		((Control)LabelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX7).Location = new Point(267, 68);
		((Control)LabelX7).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).RightToLeft = (RightToLeft)1;
		((Control)LabelX7).Size = new Size(77, 24);
		((Control)LabelX7).TabIndex = 6;
		((Control)LabelX7).Text = "[نامشخص] :";
		LabelX7.TextAlign = (ContentAlignment)16;
		((Control)LabelX6).Anchor = (AnchorStyles)9;
		((Control)LabelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX6).Location = new Point(223, 46);
		((Control)LabelX6).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).RightToLeft = (RightToLeft)1;
		((Control)LabelX6).Size = new Size(121, 24);
		((Control)LabelX6).TabIndex = 4;
		((Control)LabelX6).Text = "[نرسیده به گوشی] :";
		LabelX6.TextAlign = (ContentAlignment)16;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((Control)LabelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX5).Location = new Point(227, 21);
		((Control)LabelX5).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).RightToLeft = (RightToLeft)1;
		((Control)LabelX5).Size = new Size(117, 24);
		((Control)LabelX5).TabIndex = 2;
		((Control)LabelX5).Text = "[رسیده به گوشی] :";
		LabelX5.TextAlign = (ContentAlignment)16;
		((Control)LblBlackList).Anchor = (AnchorStyles)9;
		((Control)LblBlackList).Location = new Point(7, 56);
		((Control)LblBlackList).Margin = new Padding(4, 0, 4, 0);
		((Control)LblBlackList).Name = "LblBlackList";
		((Control)LblBlackList).RightToLeft = (RightToLeft)0;
		((Control)LblBlackList).Size = new Size(38, 17);
		((Control)LblBlackList).TabIndex = 13;
		((Control)LblBlackList).Text = "0";
		((Control)LblNaresideM).Anchor = (AnchorStyles)9;
		((Control)LblNaresideM).Location = new Point(7, 32);
		((Control)LblNaresideM).Margin = new Padding(4, 0, 4, 0);
		((Control)LblNaresideM).Name = "LblNaresideM";
		((Control)LblNaresideM).RightToLeft = (RightToLeft)0;
		((Control)LblNaresideM).Size = new Size(15, 14);
		((Control)LblNaresideM).TabIndex = 11;
		((Control)LblNaresideM).Text = "0";
		((Control)LblResideM).Anchor = (AnchorStyles)9;
		((Control)LblResideM).Location = new Point(7, 9);
		((Control)LblResideM).Margin = new Padding(4, 0, 4, 0);
		((Control)LblResideM).Name = "LblResideM";
		((Control)LblResideM).RightToLeft = (RightToLeft)0;
		((Control)LblResideM).Size = new Size(27, 15);
		((Control)LblResideM).TabIndex = 9;
		((Control)LblResideM).Text = "0";
		((Control)PictureBox6).Anchor = (AnchorStyles)9;
		PictureBox6.Image = (Image)(object)Resources.SMSD_BlackList;
		((Control)PictureBox6).Location = new Point(164, 51);
		((Control)PictureBox6).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox6).Name = "PictureBox6";
		((Control)PictureBox6).Size = new Size(24, 24);
		PictureBox6.SizeMode = (PictureBoxSizeMode)4;
		PictureBox6.TabIndex = 28;
		PictureBox6.TabStop = false;
		((Control)PictureBox5).Anchor = (AnchorStyles)9;
		PictureBox5.Image = (Image)(object)Resources.SMSD_Nareside;
		((Control)PictureBox5).Location = new Point(164, 27);
		((Control)PictureBox5).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox5).Name = "PictureBox5";
		((Control)PictureBox5).Size = new Size(24, 24);
		PictureBox5.SizeMode = (PictureBoxSizeMode)4;
		PictureBox5.TabIndex = 27;
		PictureBox5.TabStop = false;
		((Control)PictureBox4).Anchor = (AnchorStyles)9;
		PictureBox4.Image = (Image)(object)Resources.SMSD_Mokhaberat_Reside;
		((Control)PictureBox4).Location = new Point(164, 3);
		((Control)PictureBox4).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox4).Name = "PictureBox4";
		((Control)PictureBox4).Size = new Size(24, 24);
		PictureBox4.SizeMode = (PictureBoxSizeMode)4;
		PictureBox4.TabIndex = 26;
		PictureBox4.TabStop = false;
		((Control)PictureBox3).Anchor = (AnchorStyles)9;
		PictureBox3.Image = (Image)(object)Resources.gnome_status;
		((Control)PictureBox3).Location = new Point(346, 69);
		((Control)PictureBox3).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox3).Name = "PictureBox3";
		((Control)PictureBox3).Size = new Size(25, 24);
		PictureBox3.SizeMode = (PictureBoxSizeMode)4;
		PictureBox3.TabIndex = 25;
		PictureBox3.TabStop = false;
		((Control)PictureBox2).Anchor = (AnchorStyles)9;
		PictureBox2.Image = (Image)(object)Resources._23;
		((Control)PictureBox2).Location = new Point(347, 45);
		((Control)PictureBox2).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox2).Name = "PictureBox2";
		((Control)PictureBox2).Size = new Size(24, 24);
		PictureBox2.SizeMode = (PictureBoxSizeMode)4;
		PictureBox2.TabIndex = 24;
		PictureBox2.TabStop = false;
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.Image = (Image)(object)Resources._1;
		((Control)PictureBox1).Location = new Point(347, 21);
		((Control)PictureBox1).Margin = new Padding(4, 3, 4, 3);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(24, 24);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 23;
		PictureBox1.TabStop = false;
		((Control)LblUnKnown).Anchor = (AnchorStyles)9;
		((Control)LblUnKnown).Location = new Point(197, 75);
		((Control)LblUnKnown).Margin = new Padding(4, 0, 4, 0);
		((Control)LblUnKnown).Name = "LblUnKnown";
		((Control)LblUnKnown).RightToLeft = (RightToLeft)0;
		((Control)LblUnKnown).Size = new Size(33, 15);
		((Control)LblUnKnown).TabIndex = 7;
		((Control)LblUnKnown).Text = "0";
		((Control)LblNotDeliverd).Anchor = (AnchorStyles)9;
		((Control)LblNotDeliverd).Location = new Point(197, 51);
		((Control)LblNotDeliverd).Margin = new Padding(4, 0, 4, 0);
		((Control)LblNotDeliverd).Name = "LblNotDeliverd";
		((Control)LblNotDeliverd).RightToLeft = (RightToLeft)0;
		((Control)LblNotDeliverd).Size = new Size(18, 24);
		((Control)LblNotDeliverd).TabIndex = 5;
		((Control)LblNotDeliverd).Text = "0";
		((Control)LblDelivered).Anchor = (AnchorStyles)9;
		((Control)LblDelivered).Location = new Point(197, 27);
		((Control)LblDelivered).Margin = new Padding(4, 0, 4, 0);
		((Control)LblDelivered).Name = "LblDelivered";
		((Control)LblDelivered).RightToLeft = (RightToLeft)0;
		((Control)LblDelivered).Size = new Size(18, 24);
		((Control)LblDelivered).TabIndex = 3;
		((Control)LblDelivered).Text = "0";
		((Control)LBLTotal).Location = new Point(197, 6);
		((Control)LBLTotal).Margin = new Padding(4, 0, 4, 0);
		((Control)LBLTotal).Name = "LBLTotal";
		((Control)LBLTotal).RightToLeft = (RightToLeft)0;
		((Control)LBLTotal).Size = new Size(33, 19);
		((Control)LBLTotal).TabIndex = 1;
		((Control)LBLTotal).Text = "0";
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX3).Location = new Point(238, 6);
		((Control)LabelX3).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).RightToLeft = (RightToLeft)1;
		((Control)LabelX3).Size = new Size(106, 19);
		((Control)LabelX3).TabIndex = 0;
		((Control)LabelX3).Text = "[کل پیام ها] :";
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.MintCream;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[21]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)Img,
			(DataGridViewColumn)DeliverySTShort,
			(DataGridViewColumn)SendDateShDataGridViewTextBoxColumn,
			(DataGridViewColumn)dgTime,
			(DataGridViewColumn)SenderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGNumber,
			(DataGridViewColumn)DeliveryST,
			(DataGridViewColumn)DGMessage,
			(DataGridViewColumn)SendDateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)DeliveryRequestedDataGridViewCheckBoxColumn,
			(DataGridViewColumn)RefrencedIdsDataGridViewTextBoxColumn,
			(DataGridViewColumn)RefrenseAccountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)RefrencedIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)DateSHDataGridViewTextBoxColumn,
			(DataGridViewColumn)CustomerIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)CustomerNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)FixedMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)STDataGridViewCheckBoxColumn
		});
		((Control)DGrid).ContextMenuStrip = ContextMenue;
		((DataGridView)DGrid).DataSource = ViewSMSSentBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).EditMode = (DataGridViewEditMode)4;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(8, 12);
		((Control)DGrid).Margin = new Padding(4, 3, 4, 3);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(780, 207);
		((Control)DGrid).TabIndex = 2;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((DataGridView)DGrid).CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		((Control)DGrid).KeyDown += new KeyEventHandler(FRMReportSent_KeyDown);
		((DataGridViewColumn)DGCheck).DataPropertyName = "IsChecked";
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewColumn)DGCheck).Width = 30;
		((DataGridViewColumn)Img).DataPropertyName = "Img";
		((DataGridViewColumn)Img).HeaderText = "";
		((DataGridViewColumn)Img).Name = "Img";
		((DataGridViewBand)Img).Visible = false;
		((DataGridViewColumn)Img).Width = 30;
		((DataGridViewColumn)DeliverySTShort).DataPropertyName = "DeliverySTShort";
		((DataGridViewColumn)DeliverySTShort).HeaderText = "DeliverySTShort";
		((DataGridViewColumn)DeliverySTShort).Name = "DeliverySTShort";
		((DataGridViewBand)DeliverySTShort).ReadOnly = true;
		((DataGridViewBand)DeliverySTShort).Visible = false;
		((DataGridViewColumn)SendDateShDataGridViewTextBoxColumn).DataPropertyName = "SendDateSh";
		((DataGridViewColumn)SendDateShDataGridViewTextBoxColumn).HeaderText = "[تاریخ ارسال]";
		((DataGridViewColumn)SendDateShDataGridViewTextBoxColumn).Name = "SendDateShDataGridViewTextBoxColumn";
		((DataGridViewBand)SendDateShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dgTime).DataPropertyName = "SendTime";
		((DataGridViewColumn)dgTime).HeaderText = "[زمان ارسال]";
		((DataGridViewColumn)dgTime).Name = "dgTime";
		((DataGridViewBand)dgTime).ReadOnly = true;
		((DataGridViewColumn)SenderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)SenderNumberDataGridViewTextBoxColumn).HeaderText = "شماره فرستنده";
		((DataGridViewColumn)SenderNumberDataGridViewTextBoxColumn).Name = "SenderNumberDataGridViewTextBoxColumn";
		((DataGridViewColumn)SenderNumberDataGridViewTextBoxColumn).Width = 150;
		((DataGridViewColumn)DGNumber).DataPropertyName = "Number";
		((DataGridViewColumn)DGNumber).HeaderText = "[شماره گیرنده]";
		((DataGridViewColumn)DGNumber).Name = "DGNumber";
		((DataGridViewColumn)DGNumber).Width = 150;
		((DataGridViewColumn)DeliveryST).DataPropertyName = "DeliveryStatusForDisplay";
		((DataGridViewColumn)DeliveryST).HeaderText = "[وضعیت ارسال]";
		((DataGridViewColumn)DeliveryST).Name = "DeliveryST";
		((DataGridViewBand)DeliveryST).ReadOnly = true;
		((DataGridViewColumn)DGMessage).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGMessage).DataPropertyName = "Message";
		((DataGridViewColumn)DGMessage).HeaderText = "[متن پیام]";
		((DataGridViewColumn)DGMessage).Name = "DGMessage";
		((DataGridViewColumn)SendDateMDataGridViewTextBoxColumn).DataPropertyName = "SendDateM";
		((DataGridViewColumn)SendDateMDataGridViewTextBoxColumn).HeaderText = "SendDateM";
		((DataGridViewColumn)SendDateMDataGridViewTextBoxColumn).Name = "SendDateMDataGridViewTextBoxColumn";
		((DataGridViewBand)SendDateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DeliveryRequestedDataGridViewCheckBoxColumn).DataPropertyName = "DeliveryRequested";
		((DataGridViewColumn)DeliveryRequestedDataGridViewCheckBoxColumn).HeaderText = "وضعیت تحویل";
		((DataGridViewColumn)DeliveryRequestedDataGridViewCheckBoxColumn).Name = "DeliveryRequestedDataGridViewCheckBoxColumn";
		((DataGridViewBand)DeliveryRequestedDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)RefrencedIdsDataGridViewTextBoxColumn).DataPropertyName = "RefrencedIds";
		((DataGridViewColumn)RefrencedIdsDataGridViewTextBoxColumn).HeaderText = "RefrencedIds";
		((DataGridViewColumn)RefrencedIdsDataGridViewTextBoxColumn).Name = "RefrencedIdsDataGridViewTextBoxColumn";
		((DataGridViewBand)RefrencedIdsDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)RefrenseAccountIdDataGridViewTextBoxColumn).DataPropertyName = "RefrenseAccountId";
		((DataGridViewColumn)RefrenseAccountIdDataGridViewTextBoxColumn).HeaderText = "RefrenseAccountId";
		((DataGridViewColumn)RefrenseAccountIdDataGridViewTextBoxColumn).Name = "RefrenseAccountIdDataGridViewTextBoxColumn";
		((DataGridViewBand)RefrenseAccountIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)RefrencedIDDataGridViewTextBoxColumn).DataPropertyName = "RefrencedID";
		((DataGridViewColumn)RefrencedIDDataGridViewTextBoxColumn).HeaderText = "RefrencedID";
		((DataGridViewColumn)RefrencedIDDataGridViewTextBoxColumn).Name = "RefrencedIDDataGridViewTextBoxColumn";
		((DataGridViewBand)RefrencedIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)RefrencedIDDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		((DataGridViewColumn)DateMDataGridViewTextBoxColumn).HeaderText = "DateM";
		((DataGridViewColumn)DateMDataGridViewTextBoxColumn).Name = "DateMDataGridViewTextBoxColumn";
		((DataGridViewBand)DateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DateSHDataGridViewTextBoxColumn).DataPropertyName = "DateSH";
		((DataGridViewColumn)DateSHDataGridViewTextBoxColumn).HeaderText = "DateSH";
		((DataGridViewColumn)DateSHDataGridViewTextBoxColumn).Name = "DateSHDataGridViewTextBoxColumn";
		((DataGridViewBand)DateSHDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)DateSHDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)CustomerIdDataGridViewTextBoxColumn).DataPropertyName = "CustomerId";
		((DataGridViewColumn)CustomerIdDataGridViewTextBoxColumn).HeaderText = "CustomerId";
		((DataGridViewColumn)CustomerIdDataGridViewTextBoxColumn).Name = "CustomerIdDataGridViewTextBoxColumn";
		((DataGridViewBand)CustomerIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)CustomerIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)CustomerNameDataGridViewTextBoxColumn).DataPropertyName = "CustomerName";
		((DataGridViewColumn)CustomerNameDataGridViewTextBoxColumn).HeaderText = "CustomerName";
		((DataGridViewColumn)CustomerNameDataGridViewTextBoxColumn).Name = "CustomerNameDataGridViewTextBoxColumn";
		((DataGridViewBand)CustomerNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)CustomerNameDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)FixedMessageDataGridViewTextBoxColumn).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)FixedMessageDataGridViewTextBoxColumn).HeaderText = "FixedMessage";
		((DataGridViewColumn)FixedMessageDataGridViewTextBoxColumn).Name = "FixedMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)FixedMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)FixedMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)STDataGridViewCheckBoxColumn).DataPropertyName = "ST";
		((DataGridViewColumn)STDataGridViewCheckBoxColumn).HeaderText = "ST";
		((DataGridViewColumn)STDataGridViewCheckBoxColumn).Name = "STDataGridViewCheckBoxColumn";
		((DataGridViewBand)STDataGridViewCheckBoxColumn).Visible = false;
		ViewSMSSentBindingSource.DataSource = typeof(SmsSent);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(796, 396);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Control)this).MinimumSize = new Size(812, 407);
		((Control)this).Name = "FRMReportSent";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FRMReportSent_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMReportSent_KeyDown);
		((Control)ContextMenue).ResumeLayout(false);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((Control)GroupPanel2).ResumeLayout(false);
		((ISupportInitialize)pictureBox9).EndInit();
		((ISupportInitialize)PictureBox8).EndInit();
		((ISupportInitialize)PictureBox7).EndInit();
		((ISupportInitialize)PictureBox6).EndInit();
		((ISupportInitialize)PictureBox5).EndInit();
		((ISupportInitialize)PictureBox4).EndInit();
		((ISupportInitialize)PictureBox3).EndInit();
		((ISupportInitialize)PictureBox2).EndInit();
		((ISupportInitialize)PictureBox1).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)ViewSMSSentBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
