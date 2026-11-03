using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom.Cache;
using PacketParser.Entities;
using PayamakSettings;
using PayamakUi.Classes;
using PayamakUi.Forms.FRMOthers;
using Tele;
using mCore;

namespace PayamakUi;

public class FrmSetting : CacheBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _tokenSaveDb;

	private IContainer components;

	internal TabControl TabControl1;

	internal TabControlPanel TabControlPanel1;

	internal TabItem TabItem1;

	internal TabControlPanel TabControlPanel2;

	internal TabItem TabItem2;

	internal GroupPanel GroupPanel1;

	internal ButtonX BtnSave_Sound;

	internal ButtonX BtnExit_Sound;

	internal GroupBox Gb_Sound;

	internal GroupPanel Gp_setting;

	internal ButtonX BTN_Save;

	internal ButtonX BTN_Exit;

	internal GroupBox Gb_Setting;

	internal ComboBoxEx CMB_PortSpeed;

	internal ComboItem ComboItem1;

	internal ComboItem ComboItem11;

	internal ComboItem ComboItem3;

	internal ComboItem ComboItem7;

	internal ComboItem ComboItem8;

	internal ComboItem ComboItem12;

	internal ComboItem ComboItem4;

	internal ComboItem ComboItem2;

	internal ComboItem ComboItem9;

	internal ComboItem ComboItem13;

	internal ComboItem ComboItem5;

	internal ComboItem ComboItem10;

	internal ComboItem ComboItem14;

	internal ComboItem ComboItem6;

	internal ComboBoxEx CMB_PortNumber;

	internal NumericUpDown NumericUpDown1;

	internal Label LabelX2;

	internal Label LabelX1;

	internal CheckBoxX Cb_AutoAnswer;

	internal CheckBoxX Cb_AutoConnect;

	internal GroupPanel Gp_Delivery;

	internal GroupPanel Gp_Call;

	internal GroupPanel Gp_Recieve;

	internal GroupPanel Gp_Send;

	internal ButtonX Btnplay_Delivery;

	internal ButtonX BtnPlay_Call;

	internal ButtonX BtnPlay_Send;

	internal ButtonX Btn_call;

	internal ButtonX Btn_Recieve;

	internal ButtonX Btn_Send;

	internal ButtonX BtnPlay_Recieve;

	internal ButtonX Btn_Delivery;

	internal TextBoxX Txt_Delivery;

	internal TextBoxX Txt_Call;

	internal TextBoxX Txt_Recieve;

	internal TextBoxX Txt_Send;

	internal CheckBoxX Chb_Delivery;

	internal CheckBoxX Chb_Call;

	internal CheckBoxX Chb_Recieve;

	internal RadioButton RBtnSms;

	internal CheckBoxX Chb_Send;

	internal TabControlPanel TabControlPanel3;

	internal TabItem TabItem3;

	internal GroupPanel GroupPanel7;

	internal ButtonX BTNAutoDetect;

	internal TabControlPanel TabControlPanel4;

	internal TabItem TabItem4;

	internal GroupPanel GroupPanel4;

	internal GroupPanel PNLDialup;

	internal CheckBoxX CheckBoxcall;

	internal ButtonX BtnAccept;

	internal ButtonX BtnExit;

	internal CheckBoxX CheckBoxSMS;

	internal Label LblTemplate;

	internal Label LblMsg;

	internal TextBoxX TxtTem;

	internal TextBoxX TxtMsg;

	internal Label LblTemp;

	internal CheckBoxX CHBGSM;

	internal CheckBoxX CHBDeliver;

	internal TabControlPanel TabControlPanel6;

	internal TabItem TabItem6;

	internal PanelEx PanelEx3;

	internal GroupBox GroupBox1;

	internal Label LBLFontItalic;

	internal Label Label8;

	internal Label LBLFontBold;

	internal Label Label6;

	internal Label LBLFontSize;

	internal Label Label4;

	internal Label LBLFontName;

	internal Label Label2;

	internal ButtonX BTNSelectFont;

	internal Label LBLPreviwe;

	internal GroupPanel GroupPanel9;

	internal FontDialog FontDialog1;

	internal ButtonX BtnSave;

	internal ButtonX BtnExitFont;

	internal TabControlPanel TabControlPanel7;

	internal TabItem TabItem7;

	internal GroupPanel GroupPanel10;

	internal ButtonX BtnSaveDial;

	internal ButtonX BtnExitDial;

	internal GroupBox GroupBoxDialUP;

	internal ButtonX BtnAutoPort;

	internal ComboBoxEx CMBSpeedPortDial;

	internal ComboItem ComboItem29;

	internal ComboItem ComboItem30;

	internal ComboItem ComboItem31;

	internal ComboItem ComboItem32;

	internal ComboItem ComboItem33;

	internal ComboItem ComboItem34;

	internal ComboItem ComboItem35;

	internal ComboItem ComboItem36;

	internal ComboItem ComboItem37;

	internal ComboItem ComboItem38;

	internal ComboItem ComboItem39;

	internal ComboItem ComboItem40;

	internal ComboItem ComboItem41;

	internal ComboItem ComboItem42;

	internal ComboBoxEx CMBPortNumberDial;

	internal Label LabelX8;

	internal Label LabelX9;

	internal CheckBoxX ChbAnswerDial;

	internal CheckBoxX ChbContactDial;

	internal Label LabelX4;

	internal RadioButton RBtnCall;

	internal Panel Panel1;

	internal ButtonX BTN_File;

	internal Label LBL_AutoAnswer;

	internal Label LabelX6;

	internal Label LabelX5;

	internal CheckBoxX ChbHolidays;

	internal Label LabelX11;

	internal Label LabelX10;

	internal NumericUpDown Hour2;

	internal OpenFileDialog OFD;

	internal NumericUpDown Min2;

	internal NumericUpDown Hour1;

	internal NumericUpDown Min1;

	internal Label LabelX7;

	internal GroupBox GroupBox2;

	internal Label LabelX12;

	internal NumericUpDown CountSent;

	internal Label LabelX15;

	internal Label LabelX14;

	internal GroupBox GroupBox4;

	internal Label LabelX13;

	internal Label LabelX16;

	internal GroupBox GroupBox3;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FrmSetting(string contextGuid)
		: base(contextGuid)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((Control)this).KeyDown += new KeyEventHandler(FRMSetting_KeyDown);
		InitializeComponent();
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 33;
		try
		{
			CHBGSM.Checked = false;
			ChbHolidays.Checked = true;
			string[] portNames = SerialPort.GetPortNames();
			foreach (string text in portNames)
			{
				if (text.Contains("COM"))
				{
					CMB_PortNumber.Items.Add((object)text.ToString().Replace("COM", "").Replace("c", "")
						.Replace("i", "")
						.Replace("o", ""));
					CMBPortNumberDial.Items.Add((object)text.ToString().Replace("COM", "").Replace("c", "")
						.Replace("i", "")
						.Replace("o", ""));
				}
			}
			CheckBoxX chbHolidays = ChbHolidays;
			chbHolidays.Checked = await CLSSMS.GetHolidaySmsEnableAsync();
			NumericUpDown countSent = CountSent;
			countSent.Value = await CLSSMS.GetCountSentAsync();
			countSent = Hour1;
			countSent.Value = (await CLSSMS.GetSmsFromTimeAsync()).Hour;
			countSent = Hour2;
			countSent.Value = (await CLSSMS.GetSmsToTimeAsync()).Hour;
			countSent = Min1;
			countSent.Value = (await CLSSMS.GetSmsFromTimeAsync()).Minute;
			countSent = Min2;
			countSent.Value = (await CLSSMS.GetSmsToTimeAsync()).Minute;
			ComboBoxEx cMB_PortNumber = CMB_PortNumber;
			((Control)cMB_PortNumber).Text = (await Db.GetNumberPortAsync()).ToString();
			cMB_PortNumber = CMB_PortSpeed;
			((Control)cMB_PortNumber).Text = (await Db.GetBaudRateAsync()).ToString();
			chbHolidays = Cb_AutoAnswer;
			chbHolidays.Checked = await Db.GetIsAutoAnswerAsync();
			chbHolidays = Cb_AutoConnect;
			chbHolidays.Checked = await Db.GetAutoConnectAsync();
			CHBGSM.Checked = CLSSMS.GsmEnable;
			chbHolidays = CHBDeliver;
			chbHolidays.Checked = await CLSSMS.GetDeliveryEnableAsync();
			Label lBL_AutoAnswer = LBL_AutoAnswer;
			((Control)lBL_AutoAnswer).Text = await Db.GetAutoAnswerFileAsync();
			Panel panel = Panel1;
			((Control)panel).Enabled = await Db.GetIsAutoAnswerAsync();
			cMB_PortNumber = CMBPortNumberDial;
			((Control)cMB_PortNumber).Text = (await CLSDialUp.GetNumberPortAsync()).ToString();
			cMB_PortNumber = CMBSpeedPortDial;
			((Control)cMB_PortNumber).Text = (await CLSDialUp.GetSpeedPortAsync()).ToString();
			chbHolidays = ChbAnswerDial;
			chbHolidays.Checked = await CLSDialUp.GetIsAutoAnswerAsync();
			chbHolidays = ChbContactDial;
			chbHolidays.Checked = await CLSDialUp.GetAutoConnectAsync();
			countSent = NumericUpDown1;
			countSent.Value = await CLSDialUp.GetNumberAnswerAutoAsync();
			GroupBox groupBoxDialUP = GroupBoxDialUP;
			((Control)groupBoxDialUP).Enabled = await CLSDialUp.GetAutoConnectAsync();
			TextBoxX txt_Call = Txt_Call;
			((Control)txt_Call).Text = await Sound.GetPathCallAsync();
			txt_Call = Txt_Delivery;
			((Control)txt_Call).Text = await Sound.GetPathDeliveryAsync();
			txt_Call = Txt_Recieve;
			((Control)txt_Call).Text = await Sound.GetPathRecieveAsync();
			txt_Call = Txt_Send;
			((Control)txt_Call).Text = await Sound.GetPathSendAsync();
			chbHolidays = Chb_Call;
			chbHolidays.Checked = await Sound.GetIsPlayCallAsync();
			chbHolidays = Chb_Delivery;
			chbHolidays.Checked = await Sound.GetIsPlaydeliveryAsync();
			chbHolidays = Chb_Recieve;
			chbHolidays.Checked = await Sound.GetIsPlayRecieveAsync();
			chbHolidays = Chb_Send;
			chbHolidays.Checked = await Sound.GetIsPlaySendAsync();
			chbHolidays = CheckBoxcall;
			chbHolidays.Checked = await Answer.GetIsCallAsync();
			chbHolidays = CheckBoxSMS;
			chbHolidays.Checked = await Answer.GetIsMsgAsync();
			RadioButton rBtnCall = RBtnCall;
			rBtnCall.Checked = await Answer.GetIsByCallAsync();
			rBtnCall = RBtnSms;
			rBtnCall.Checked = await Answer.GetIsByMsgAsync();
			txt_Call = TxtMsg;
			((Control)txt_Call).Text = await Answer.GetTemplateCallAsync();
			txt_Call = TxtTem;
			((Control)txt_Call).Text = await Answer.GetTemplateMsgAsync();
			Font val = await Font.GetFontProgramAsync(default(CancellationToken));
			((Control)LBLFontBold).Text = (val.Bold ? "بلی" : "خیر");
			((Control)LBLFontItalic).Text = (val.Italic ? "بلی" : "خیر");
			((Control)LBLFontName).Text = val.Name;
			((Control)LBLFontSize).Text = val.Size.ToString();
			((Control)LBLPreviwe).Font = val;
			FontDialog1.Font = val;
			((Control)LBLFontBold).Font = val;
			((Control)LBLFontItalic).Font = val;
			((Control)LBLFontName).Font = val;
			((Control)LBLFontSize).Font = val;
			((Control)GroupBoxDialUP).Enabled = ChbContactDial.Checked;
			((Control)Gb_Setting).Enabled = CHBGSM.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 94);
		}
	}

	private void CBAutoAnswer_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Panel1).Enabled = Cb_AutoAnswer.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CBAutoAnswer_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 105);
		}
	}

	private void BTNFile_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)LBL_AutoAnswer).Text = ((FileDialog)val).FileName;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNFile_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 121);
		}
	}

	private void FRMSetting_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if ((int)keyCode == 116)
			{
				BTN_Save.PerformClick();
			}
			else if ((int)keyCode == 13)
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSetting_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 153);
		}
	}

	private void BtnExit_Sound_Click(object sender, EventArgs e)
	{
	}

	private void BTN_Exit_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTN_Exit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 171);
		}
	}

	private async void BTN_Save_Click_1(object sender, EventArgs e)
	{
		_ = 9;
		try
		{
			if (!((Control)BTN_Save).Enabled || !((Control)BTN_Save).Visible)
			{
				return;
			}
			await CLSSMS.SetHolidaySmsEnableAsync(ChbHolidays.Checked);
			DateTime smsFromTimeAsync = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + Hour1.Value + ":" + Min1.Value);
			DateTime Date2 = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + Hour2.Value + ":" + Min2.Value);
			await CLSSMS.SetSmsFromTimeAsync(smsFromTimeAsync);
			await CLSSMS.SetSmsToTimeAsync(Date2);
			await CLSSMS.SetCountSentAsync(int.Parse(CountSent.Value.ToString()));
			if (CHBGSM.Checked)
			{
				if (((ListControl)CMB_PortNumber).SelectedIndex == -1)
				{
					MessageBox.Show("شماره پورت انتخاب نشده است", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return;
				}
				if (((ListControl)CMB_PortSpeed).SelectedIndex == -1)
				{
					MessageBox.Show("سرعت انتخاب نشده است", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return;
				}
				SMS val = new SMS
				{
					Port = "COM" + ((ComboBox)CMB_PortNumber).SelectedItem.ToString(),
					Parity = (Parity)2,
					DisableCheckPIN = true,
					DataBits = (DataBits)8,
					FlowControl = (FlowControl)0,
					BaudRate = (BaudRate)((ComboBox)CMB_PortSpeed).SelectedItem
				};
				val.Connect();
				MessageBox.Show("مودم با مدل " + val.Model + " بر روی پورت شماره " + val.Port + "شناسایی شد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				val.Disconnect();
				await Db.SetNumberPortAsync(short.Parse(((Control)CMB_PortNumber).Text.ToString().Replace("COM", "").Trim()));
				await Db.SetBaudRateAsync(long.Parse(((Control)CMB_PortSpeed).Text.ToString()));
				await Db.SetAutoAnswerFileAsync(((Control)LBL_AutoAnswer).Text);
				await Db.SetAutoConnectAsync(Cb_AutoConnect.Checked);
				await Db.SetIsAutoAnswerAsync(Cb_AutoAnswer.Checked);
				CLSSMS.GsmEnable = CHBGSM.Checked;
				await CLSSMS.SetDeliveryEnableAsync(CHBDeliver.Checked);
				Process.StartGSMEngine();
				MessageBox.Show("تنظیمات با موفقیت ثبت شد.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				CLSSMS.GsmEnable = CHBGSM.Checked;
				MessageBox.Show("تنظیمات با موفقیت ثبت شد.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTN_Save_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 238);
		}
	}

	private void BTNExit_DB_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_DB_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 251);
		}
	}

	private async Task<bool> TestConnectionAsync(string ConnectionString, CancellationToken token)
	{
		try
		{
			SqlConnection cn = new SqlConnection(ConnectionString);
			try
			{
				if (string.IsNullOrEmpty(ConnectionString.Trim()))
				{
					return true;
				}
				await ((DbConnection)(object)cn).OpenAsync(token);
				((DbConnection)(object)cn).Close();
				return true;
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TestConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 273);
			return false;
		}
	}

	private void Btn_Send_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)Txt_Send).Text = LoadFromFile();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Send_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 284);
		}
	}

	private string LoadFromFile()
	{
		return "";
	}

	private void BtnPlay_Send_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!string.IsNullOrEmpty(((Control)Txt_Send).Text))
			{
				new SoundPlayer(((Control)Txt_Send).Text).Play();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnPlay_Send_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 346);
		}
	}

	private void BtnPlay_Recieve_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!string.IsNullOrEmpty(((Control)Txt_Recieve).Text))
			{
				new SoundPlayer(((Control)Txt_Recieve).Text).Play();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnPlay_Recieve_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 361);
		}
	}

	private void BtnPlay_Call_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!string.IsNullOrEmpty(((Control)Txt_Call).Text))
			{
				new SoundPlayer(((Control)Txt_Call).Text).Play();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnPlay_Call_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 376);
		}
	}

	private void Btnplay_Delivery_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!string.IsNullOrEmpty(((Control)Txt_Delivery).Text))
			{
				new SoundPlayer(((Control)Txt_Delivery).Text).Play();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btnplay_Delivery_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 391);
		}
	}

	private void Btn_Recieve_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)Txt_Recieve).Text = LoadFromFile();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Recieve_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 400);
		}
	}

	private void Btn_call_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)Txt_Call).Text = LoadFromFile();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_call_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 409);
		}
	}

	private void Btn_Delivery_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)Txt_Delivery).Text = LoadFromFile();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Delivery_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 418);
		}
	}

	private void BTNAutoDetect_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CMB_PortNumber.Items.Count <= 0)
			{
				return;
			}
			FRMProgress fRMProgress = new FRMProgress(base.ContextGuid, CMB_PortNumber.Items.Count);
			((Form)fRMProgress).Show((IWin32Window)(object)this);
			int i = 0;
			for (int num = CMB_PortNumber.Items.Count - 1; i <= num; i++)
			{
				SMS val = new SMS();
				fRMProgress.PortNumber = "Com" + CMB_PortNumber.Items[i].ToString();
				try
				{
					val.Port = "Com" + CMB_PortNumber.Items[i].ToString();
					SerialPort val2 = new SerialPort(val.Port.Replace("com", ""), 115200);
					val2.Open();
					val2.Close();
				}
				catch (Exception)
				{
					continue;
				}
				val.Parity = (Parity)2;
				val.DisableCheckPIN = true;
				val.DataBits = (DataBits)8;
				val.FlowControl = (FlowControl)0;
				val.BaudRate = (BaudRate)115200;
				val.Timeout = 500L;
				try
				{
					if (val.Connect())
					{
						MessageBox.Show("مودم با مدل " + val.Model + " بر روی پورت شماره " + val.Port + "شناسایی شد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
						val.Disconnect();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "BTNAutoDetect_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 459);
				}
			}
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "BTNAutoDetect_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 463);
		}
	}

	private void BtnSave_Sound_Click(object sender, EventArgs e)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((Control)BtnSave_Sound).Enabled && ((Control)BtnSave_Sound).Visible)
			{
				Sound.SetPathSendAsync(((Control)Txt_Send).Text);
				Sound.SetPathRecieveAsync(((Control)Txt_Recieve).Text);
				Sound.SetPathCallAsync(((Control)Txt_Call).Text);
				Sound.SetPathDeliveryAsync(((Control)Txt_Delivery).Text);
				Sound.SetIsPlaySendAsync(Chb_Send.Checked);
				Sound.SetIsPlayRecieve(Chb_Recieve.Checked);
				Sound.SetIsPlayCallAsync(Chb_Call.Checked);
				Sound.SetIsPlaydeliveryAsync(Chb_Delivery.Checked);
				MessageBox.Show("تنظیمات با موفقیت ثبت شد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSave_Sound_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 483);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 493);
		}
	}

	private void RBtnSms_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RBtnSms.Checked)
			{
				((Control)LblMsg).Enabled = true;
				((Control)TxtMsg).Enabled = true;
			}
			else
			{
				((Control)LblMsg).Enabled = false;
				((Control)TxtMsg).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RBtnSms_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 511);
		}
	}

	private void CheckBoxSMS_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CheckBoxSMS.Checked)
			{
				((Control)LblTemp).Enabled = true;
				((Control)LblTemplate).Enabled = true;
				((Control)TxtTem).Enabled = true;
			}
			else
			{
				((Control)LblTemp).Enabled = false;
				((Control)LblTemplate).Enabled = false;
				((Control)TxtTem).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckBoxSMS_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 531);
		}
	}

	private void CheckBoxcall_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CheckBoxcall.Checked)
			{
				((Control)LblMsg).Enabled = true;
				((Control)RBtnCall).Enabled = true;
				((Control)TxtMsg).Enabled = true;
				((Control)RBtnSms).Enabled = true;
			}
			else
			{
				((Control)LblMsg).Enabled = false;
				((Control)RBtnCall).Enabled = false;
				((Control)TxtMsg).Enabled = false;
				((Control)RBtnSms).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckBoxcall_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 553);
		}
	}

	private void CHBGSM_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Gb_Setting).Enabled = CHBGSM.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CHBGSM_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 562);
		}
	}

	private void BtnExitFont_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExitFont_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 572);
		}
	}

	private async void BTNSelectFont_Click(object sender, EventArgs e)
	{
		try
		{
			Font val = await Font.GetFontProgramAsync(default(CancellationToken));
			if ((int)((CommonDialog)FontDialog1).ShowDialog() == 1)
			{
				Font._FontProgram = FontDialog1.Font;
				((Control)LBLPreviwe).Font = val;
				((Control)LBLFontBold).Font = val;
				((Control)LBLFontItalic).Font = val;
				((Control)LBLFontName).Font = val;
				((Control)LBLFontSize).Font = val;
				((Control)LBLFontBold).Text = (val.Bold ? "بلی" : "خیر");
				((Control)LBLFontItalic).Text = (val.Italic ? "بلی" : "خیر");
				((Control)LBLFontName).Text = val.Name;
				((Control)LBLFontSize).Text = val.Size.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNSelectFont_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 594);
		}
	}

	private async void BtnSave_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSave).Enabled && ((Control)BtnSave).Visible)
			{
				await Font.SetFontProgramAsync(((Control)LBLPreviwe).Font, default(CancellationToken));
				MessageBox.Show("تنظیمات با موفقیت ثبت شد.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSave_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 611);
		}
	}

	private void BtnExitFont_Click_1(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExitFont_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 621);
		}
	}

	private void BtnExitDial_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExitDial_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 631);
		}
	}

	private void BtnAutoPort_Click(object sender, EventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CMBPortNumberDial.Items.Count <= 0)
			{
				return;
			}
			FRMProgress fRMProgress = new FRMProgress(base.ContextGuid, CMBPortNumberDial.Items.Count);
			((Form)fRMProgress).ShowDialog((IWin32Window)(object)this);
			int i = 0;
			for (int num = CMBPortNumberDial.Items.Count - 1; i <= num; i++)
			{
				string text = "Com" + CMBPortNumberDial.Items[i].ToString();
				int num2 = int.Parse(((ComboBox)CMBSpeedPortDial).SelectedItem.ToString());
				fRMProgress.PortNumber = text;
				try
				{
					NovinPhone val = new NovinPhone(text, (long)num2, (short)8, (StopBits)1, (Parity)0, (Handshake)0, (short)3);
					if (val.Connect())
					{
						MessageBox.Show("مودم با مدل " + val.Model + " بر روی پورت شماره " + val.Port + "شناسایی شد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					}
					val.Disconnect();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BtnAutoPort_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 663);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "BtnAutoPort_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 669);
		}
	}

	private void BtnSaveDial_Click(object sender, EventArgs e)
	{
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!((Control)BtnSaveDial).Enabled || !((Control)BtnSaveDial).Visible)
			{
				return;
			}
			if (ChbContactDial.Checked)
			{
				if (((ListControl)CMBPortNumberDial).SelectedIndex == -1)
				{
					MessageBox.Show("شماره پورت انتخاب نشده است", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return;
				}
				if (((ListControl)CMBSpeedPortDial).SelectedIndex == -1)
				{
					MessageBox.Show("سرعت انتخاب نشده است", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return;
				}
				string text = "com" + ((ComboBox)CMBPortNumberDial).SelectedItem.ToString();
				int num = int.Parse(((ComboBox)CMBSpeedPortDial).SelectedItem.ToString());
				NovinPhone val = new NovinPhone(text, (long)num, (short)8, (StopBits)1, (Parity)0, (Handshake)0, (short)3);
				if (val.Connect())
				{
					MessageBox.Show("مودم شناسایی شد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
				else
				{
					MessageBox.Show("خطا در برقراری ارتباط", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
				val.Disconnect();
				CLSDialUp.SetNumberPort(short.Parse(((Control)CMBPortNumberDial).Text.ToString().Replace("COM", "").Trim()));
				CLSDialUp.SetSpeedPort(long.Parse(((Control)CMBSpeedPortDial).Text.ToString()));
				CLSDialUp.SetAutoConnect(ChbContactDial.Checked);
				CLSDialUp.SetIsAutoAnswer(ChbAnswerDial.Checked);
				CLSDialUp.SetNumberAnswerAuto(int.Parse(NumericUpDown1.Value.ToString()));
				MessageBox.Show("تنظیمات با موفقیت ثبت شد.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				CLSDialUp.SetAutoConnect(ChbContactDial.Checked);
				MessageBox.Show("تنظیمات با موفقیت ثبت شد.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSaveDial_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 725);
		}
	}

	private void ChbContactDial_CheckedChanged(object sender, EventArgs e)
	{
		((Control)GroupBoxDialUP).Enabled = ChbContactDial.Checked;
	}

	private async void FrmSetting_Load(object sender, EventArgs e)
	{
		await LoadDataAsync();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmSetting.cs", 748);
		}
	}

	private void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Expected O, but got Unknown
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2281: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3232: Unknown result type (might be due to invalid IL or missing references)
		//IL_48cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_553e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6208: Unknown result type (might be due to invalid IL or missing references)
		//IL_6212: Expected O, but got Unknown
		components = new Container();
		TabControl1 = new TabControl();
		TabControlPanel1 = new TabControlPanel();
		Gp_setting = new GroupPanel();
		LabelX13 = new Label();
		BTN_Save = new ButtonX();
		BTN_Exit = new ButtonX();
		CHBGSM = new CheckBoxX();
		Gb_Setting = new GroupBox();
		GroupBox2 = new GroupBox();
		LabelX11 = new Label();
		Min2 = new NumericUpDown();
		LabelX10 = new Label();
		LabelX6 = new Label();
		Hour2 = new NumericUpDown();
		LabelX7 = new Label();
		Min1 = new NumericUpDown();
		Hour1 = new NumericUpDown();
		LabelX5 = new Label();
		ChbHolidays = new CheckBoxX();
		BTNAutoDetect = new ButtonX();
		Panel1 = new Panel();
		LabelX16 = new Label();
		BTN_File = new ButtonX();
		LBL_AutoAnswer = new Label();
		CMB_PortSpeed = new ComboBoxEx();
		ComboItem1 = new ComboItem();
		ComboItem11 = new ComboItem();
		ComboItem3 = new ComboItem();
		ComboItem7 = new ComboItem();
		ComboItem8 = new ComboItem();
		ComboItem12 = new ComboItem();
		ComboItem4 = new ComboItem();
		ComboItem2 = new ComboItem();
		ComboItem9 = new ComboItem();
		ComboItem13 = new ComboItem();
		ComboItem5 = new ComboItem();
		ComboItem10 = new ComboItem();
		ComboItem14 = new ComboItem();
		ComboItem6 = new ComboItem();
		CMB_PortNumber = new ComboBoxEx();
		LabelX2 = new Label();
		LabelX1 = new Label();
		Cb_AutoAnswer = new CheckBoxX();
		Cb_AutoConnect = new CheckBoxX();
		CHBDeliver = new CheckBoxX();
		GroupBox3 = new GroupBox();
		GroupBox4 = new GroupBox();
		LabelX15 = new Label();
		LabelX12 = new Label();
		CountSent = new NumericUpDown();
		LabelX14 = new Label();
		TabItem1 = new TabItem(components);
		TabControlPanel7 = new TabControlPanel();
		GroupPanel10 = new GroupPanel();
		BtnSaveDial = new ButtonX();
		BtnExitDial = new ButtonX();
		GroupBoxDialUP = new GroupBox();
		LabelX4 = new Label();
		NumericUpDown1 = new NumericUpDown();
		BtnAutoPort = new ButtonX();
		CMBSpeedPortDial = new ComboBoxEx();
		ComboItem29 = new ComboItem();
		ComboItem30 = new ComboItem();
		ComboItem31 = new ComboItem();
		ComboItem32 = new ComboItem();
		ComboItem33 = new ComboItem();
		ComboItem34 = new ComboItem();
		ComboItem35 = new ComboItem();
		ComboItem36 = new ComboItem();
		ComboItem37 = new ComboItem();
		ComboItem38 = new ComboItem();
		ComboItem39 = new ComboItem();
		ComboItem40 = new ComboItem();
		ComboItem41 = new ComboItem();
		ComboItem42 = new ComboItem();
		CMBPortNumberDial = new ComboBoxEx();
		LabelX8 = new Label();
		LabelX9 = new Label();
		ChbAnswerDial = new CheckBoxX();
		ChbContactDial = new CheckBoxX();
		TabItem7 = new TabItem(components);
		TabControlPanel3 = new TabControlPanel();
		GroupPanel7 = new GroupPanel();
		TabItem3 = new TabItem(components);
		TabControlPanel2 = new TabControlPanel();
		GroupPanel1 = new GroupPanel();
		BtnSave_Sound = new ButtonX();
		BtnExit_Sound = new ButtonX();
		Gb_Sound = new GroupBox();
		Chb_Delivery = new CheckBoxX();
		Chb_Call = new CheckBoxX();
		Chb_Recieve = new CheckBoxX();
		Chb_Send = new CheckBoxX();
		Gp_Delivery = new GroupPanel();
		Txt_Delivery = new TextBoxX();
		Btnplay_Delivery = new ButtonX();
		Btn_Delivery = new ButtonX();
		Gp_Call = new GroupPanel();
		Txt_Call = new TextBoxX();
		Btn_call = new ButtonX();
		BtnPlay_Call = new ButtonX();
		Gp_Recieve = new GroupPanel();
		Txt_Recieve = new TextBoxX();
		Btn_Recieve = new ButtonX();
		BtnPlay_Recieve = new ButtonX();
		Gp_Send = new GroupPanel();
		Txt_Send = new TextBoxX();
		Btn_Send = new ButtonX();
		BtnPlay_Send = new ButtonX();
		TabItem2 = new TabItem(components);
		TabControlPanel4 = new TabControlPanel();
		GroupPanel4 = new GroupPanel();
		PNLDialup = new GroupPanel();
		LblTemp = new Label();
		LblTemplate = new Label();
		LblMsg = new Label();
		TxtTem = new TextBoxX();
		TxtMsg = new TextBoxX();
		RBtnCall = new RadioButton();
		RBtnSms = new RadioButton();
		CheckBoxSMS = new CheckBoxX();
		CheckBoxcall = new CheckBoxX();
		BtnAccept = new ButtonX();
		BtnExit = new ButtonX();
		TabItem4 = new TabItem(components);
		TabControlPanel6 = new TabControlPanel();
		PanelEx3 = new PanelEx();
		BtnSave = new ButtonX();
		BtnExitFont = new ButtonX();
		GroupBox1 = new GroupBox();
		LBLFontItalic = new Label();
		Label8 = new Label();
		LBLFontBold = new Label();
		Label6 = new Label();
		LBLFontSize = new Label();
		Label4 = new Label();
		LBLFontName = new Label();
		Label2 = new Label();
		BTNSelectFont = new ButtonX();
		LBLPreviwe = new Label();
		GroupPanel9 = new GroupPanel();
		TabItem6 = new TabItem(components);
		OFD = new OpenFileDialog();
		FontDialog1 = new FontDialog();
		((ISupportInitialize)TabControl1).BeginInit();
		((Control)TabControl1).SuspendLayout();
		((Control)TabControlPanel1).SuspendLayout();
		((Control)Gp_setting).SuspendLayout();
		((Control)Gb_Setting).SuspendLayout();
		((Control)GroupBox2).SuspendLayout();
		((ISupportInitialize)Min2).BeginInit();
		((ISupportInitialize)Hour2).BeginInit();
		((ISupportInitialize)Min1).BeginInit();
		((ISupportInitialize)Hour1).BeginInit();
		((Control)Panel1).SuspendLayout();
		((Control)GroupBox4).SuspendLayout();
		((ISupportInitialize)CountSent).BeginInit();
		((Control)TabControlPanel7).SuspendLayout();
		((Control)GroupPanel10).SuspendLayout();
		((Control)GroupBoxDialUP).SuspendLayout();
		((ISupportInitialize)NumericUpDown1).BeginInit();
		((Control)TabControlPanel3).SuspendLayout();
		((Control)TabControlPanel2).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((Control)Gb_Sound).SuspendLayout();
		((Control)Gp_Delivery).SuspendLayout();
		((Control)Gp_Call).SuspendLayout();
		((Control)Gp_Recieve).SuspendLayout();
		((Control)Gp_Send).SuspendLayout();
		((Control)TabControlPanel4).SuspendLayout();
		((Control)GroupPanel4).SuspendLayout();
		((Control)PNLDialup).SuspendLayout();
		((Control)TabControlPanel6).SuspendLayout();
		((Control)PanelEx3).SuspendLayout();
		((Control)GroupBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TabControl1).Anchor = (AnchorStyles)15;
		((Control)TabControl1).BackColor = Color.LightGray;
		TabControl1.CanReorderTabs = true;
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel3);
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel2);
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel1);
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel7);
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel4);
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel6);
		((Control)TabControl1).Location = new Point(0, 0);
		((Control)TabControl1).Name = "TabControl1";
		TabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		TabControl1.SelectedTabIndex = 0;
		((Control)TabControl1).Size = new Size(696, 524);
		((Control)TabControl1).TabIndex = 0;
		TabControl1.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		TabControl1.Tabs.Add(TabItem1);
		TabControl1.Tabs.Add(TabItem7);
		TabControl1.Tabs.Add(TabItem2);
		TabControl1.Tabs.Add(TabItem3);
		TabControl1.Tabs.Add(TabItem4);
		TabControl1.Tabs.Add(TabItem6);
		((Control)TabControl1).Text = "TabControl1";
		((Control)TabControlPanel1).Controls.Add((Control)(object)Gp_setting);
		TabControlPanel1.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel1).Dock = (DockStyle)5;
		((Control)TabControlPanel1).Location = new Point(0, 26);
		((Control)TabControlPanel1).Name = "TabControlPanel1";
		((Control)TabControlPanel1).Padding = new Padding(1);
		((Control)TabControlPanel1).Size = new Size(696, 498);
		TabControlPanel1.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel1.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel1.Style.Border = eBorderType.SingleLine;
		TabControlPanel1.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel1.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel1.Style.GradientAngle = 90;
		((Control)TabControlPanel1).TabIndex = 1;
		TabControlPanel1.TabItem = TabItem1;
		((Control)Gp_setting).Anchor = (AnchorStyles)13;
		((Control)Gp_setting).BackColor = Color.Transparent;
		Gp_setting.CanvasColor = SystemColors.Control;
		Gp_setting.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gp_setting).Controls.Add((Control)(object)LabelX13);
		((Control)Gp_setting).Controls.Add((Control)(object)BTN_Save);
		((Control)Gp_setting).Controls.Add((Control)(object)BTN_Exit);
		((Control)Gp_setting).Controls.Add((Control)(object)CHBGSM);
		((Control)Gp_setting).Controls.Add((Control)(object)Gb_Setting);
		((Control)Gp_setting).Controls.Add((Control)(object)GroupBox4);
		Gp_setting.DisabledBackColor = Color.Empty;
		((Control)Gp_setting).Location = new Point(3, 4);
		((Control)Gp_setting).Name = "Gp_setting";
		((Control)Gp_setting).Size = new Size(689, 482);
		Gp_setting.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gp_setting.Style.BackColorGradientAngle = 90;
		Gp_setting.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gp_setting.Style.BorderBottom = eStyleBorderType.Solid;
		Gp_setting.Style.BorderBottomWidth = 1;
		Gp_setting.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gp_setting.Style.BorderLeft = eStyleBorderType.Solid;
		Gp_setting.Style.BorderLeftWidth = 1;
		Gp_setting.Style.BorderRight = eStyleBorderType.Solid;
		Gp_setting.Style.BorderRightWidth = 1;
		Gp_setting.Style.BorderTop = eStyleBorderType.Solid;
		Gp_setting.Style.BorderTopWidth = 1;
		Gp_setting.Style.CornerDiameter = 4;
		Gp_setting.Style.CornerType = eCornerType.Rounded;
		Gp_setting.Style.TextAlignment = eStyleTextAlignment.Far;
		Gp_setting.Style.TextColor = Color.Blue;
		Gp_setting.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gp_setting.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		Gp_setting.StyleMouseDown.CornerType = eCornerType.Square;
		Gp_setting.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gp_setting).TabIndex = 0;
		((Control)LabelX13).Anchor = (AnchorStyles)9;
		((Control)LabelX13).Location = new Point(458, 62);
		((Control)LabelX13).Name = "LabelX13";
		((Control)LabelX13).Size = new Size(71, 21);
		((Control)LabelX13).TabIndex = 18;
		((Control)LabelX13).Text = "(پیامک GSM)";
		LabelX13.TextAlign = (ContentAlignment)16;
		((Control)BTN_Save).AccessibleRole = (AccessibleRole)43;
		((Control)BTN_Save).Anchor = (AnchorStyles)6;
		BTN_Save.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTN_Save).Location = new Point(94, 406);
		((Control)BTN_Save).Name = "BTN_Save";
		((Control)BTN_Save).Size = new Size(77, 31);
		BTN_Save.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTN_Save).TabIndex = 1;
		((Control)BTN_Save).Text = "ذخیره";
		BTN_Save.Tooltip = "ذخیره تنظیمات";
		((Control)BTN_Save).Click += BTN_Save_Click_1;
		((Control)BTN_Exit).AccessibleRole = (AccessibleRole)43;
		((Control)BTN_Exit).Anchor = (AnchorStyles)6;
		BTN_Exit.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTN_Exit).Location = new Point(11, 406);
		((Control)BTN_Exit).Name = "BTN_Exit";
		((Control)BTN_Exit).Size = new Size(77, 31);
		BTN_Exit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTN_Exit).TabIndex = 2;
		((Control)BTN_Exit).TabStop = false;
		((Control)BTN_Exit).Text = "خروج";
		BTN_Exit.Tooltip = "خروج";
		((Control)BTN_Exit).Click += BTN_Exit_Click;
		((Control)CHBGSM).Anchor = (AnchorStyles)13;
		CHBGSM.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)CHBGSM).Location = new Point(532, 60);
		((Control)CHBGSM).Name = "CHBGSM";
		((Control)CHBGSM).Size = new Size(135, 23);
		CHBGSM.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CHBGSM).TabIndex = 3;
		((Control)CHBGSM).Text = "ارسال از طریق موبایل";
		CHBGSM.CheckedChanged += CHBGSM_CheckedChanged;
		((Control)Gb_Setting).Anchor = (AnchorStyles)13;
		((Control)Gb_Setting).BackColor = Color.Transparent;
		((Control)Gb_Setting).Controls.Add((Control)(object)GroupBox2);
		((Control)Gb_Setting).Controls.Add((Control)(object)LabelX5);
		((Control)Gb_Setting).Controls.Add((Control)(object)ChbHolidays);
		((Control)Gb_Setting).Controls.Add((Control)(object)BTNAutoDetect);
		((Control)Gb_Setting).Controls.Add((Control)(object)Panel1);
		((Control)Gb_Setting).Controls.Add((Control)(object)CMB_PortSpeed);
		((Control)Gb_Setting).Controls.Add((Control)(object)CMB_PortNumber);
		((Control)Gb_Setting).Controls.Add((Control)(object)LabelX2);
		((Control)Gb_Setting).Controls.Add((Control)(object)LabelX1);
		((Control)Gb_Setting).Controls.Add((Control)(object)Cb_AutoAnswer);
		((Control)Gb_Setting).Controls.Add((Control)(object)Cb_AutoConnect);
		((Control)Gb_Setting).Controls.Add((Control)(object)CHBDeliver);
		((Control)Gb_Setting).Controls.Add((Control)(object)GroupBox3);
		((Control)Gb_Setting).Location = new Point(10, 97);
		((Control)Gb_Setting).Name = "Gb_Setting";
		((Control)Gb_Setting).Size = new Size(657, 289);
		((Control)Gb_Setting).TabIndex = 0;
		Gb_Setting.TabStop = false;
		((Control)Gb_Setting).Text = "تنظیمات پیامک GSM";
		((Control)GroupBox2).Anchor = (AnchorStyles)13;
		((Control)GroupBox2).Controls.Add((Control)(object)LabelX11);
		((Control)GroupBox2).Controls.Add((Control)(object)Min2);
		((Control)GroupBox2).Controls.Add((Control)(object)LabelX10);
		((Control)GroupBox2).Controls.Add((Control)(object)LabelX6);
		((Control)GroupBox2).Controls.Add((Control)(object)Hour2);
		((Control)GroupBox2).Controls.Add((Control)(object)LabelX7);
		((Control)GroupBox2).Controls.Add((Control)(object)Min1);
		((Control)GroupBox2).Controls.Add((Control)(object)Hour1);
		((Control)GroupBox2).Location = new Point(6, 19);
		((Control)GroupBox2).Name = "GroupBox2";
		((Control)GroupBox2).Size = new Size(553, 45);
		((Control)GroupBox2).TabIndex = 14;
		GroupBox2.TabStop = false;
		((Control)LabelX11).Anchor = (AnchorStyles)9;
		((Control)LabelX11).Location = new Point(431, 16);
		((Control)LabelX11).Name = "LabelX11";
		((Control)LabelX11).Size = new Size(10, 21);
		((Control)LabelX11).TabIndex = 13;
		((Control)LabelX11).Text = ":";
		((Control)Min2).Anchor = (AnchorStyles)9;
		((Control)Min2).Location = new Point(229, 16);
		Min2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Min2).Name = "Min2";
		((Control)Min2).Size = new Size(40, 20);
		((Control)Min2).TabIndex = 10;
		Min2.Value = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)LabelX10).Anchor = (AnchorStyles)9;
		((Control)LabelX10).Location = new Point(211, 16);
		((Control)LabelX10).Name = "LabelX10";
		((Control)LabelX10).Size = new Size(10, 21);
		((Control)LabelX10).TabIndex = 12;
		((Control)LabelX10).Text = ":";
		((Control)LabelX6).Anchor = (AnchorStyles)9;
		((Control)LabelX6).Location = new Point(489, 16);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).Size = new Size(61, 21);
		((Control)LabelX6).TabIndex = 6;
		((Control)LabelX6).Text = "از ساعت :";
		LabelX6.TextAlign = (ContentAlignment)16;
		((Control)Hour2).Anchor = (AnchorStyles)9;
		((Control)Hour2).Location = new Point(167, 16);
		Hour2.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Hour2).Name = "Hour2";
		((Control)Hour2).Size = new Size(40, 20);
		((Control)Hour2).TabIndex = 11;
		Hour2.Value = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)LabelX7).Anchor = (AnchorStyles)9;
		((Control)LabelX7).Location = new Point(275, 16);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).Size = new Size(78, 21);
		((Control)LabelX7).TabIndex = 7;
		((Control)LabelX7).Text = "تا ساعت :";
		LabelX7.TextAlign = (ContentAlignment)16;
		((Control)Min1).Anchor = (AnchorStyles)9;
		((Control)Min1).Location = new Point(447, 16);
		Min1.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Min1).Name = "Min1";
		((Control)Min1).Size = new Size(40, 20);
		((Control)Min1).TabIndex = 8;
		((Control)Hour1).Anchor = (AnchorStyles)9;
		((Control)Hour1).Location = new Point(391, 16);
		Hour1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Hour1).Name = "Hour1";
		((Control)Hour1).Size = new Size(40, 20);
		((Control)Hour1).TabIndex = 9;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((Control)LabelX5).Location = new Point(565, 35);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).Size = new Size(78, 21);
		((Control)LabelX5).TabIndex = 5;
		((Control)LabelX5).Text = "زمان ارسال :";
		LabelX5.TextAlign = (ContentAlignment)16;
		((Control)ChbHolidays).Anchor = (AnchorStyles)13;
		ChbHolidays.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)ChbHolidays).Location = new Point(6, 71);
		((Control)ChbHolidays).Name = "ChbHolidays";
		((Control)ChbHolidays).Size = new Size(636, 23);
		ChbHolidays.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ChbHolidays).TabIndex = 4;
		((Control)ChbHolidays).Text = "ارسال در روزهای تعطیل";
		((Control)BTNAutoDetect).AccessibleRole = (AccessibleRole)43;
		((Control)BTNAutoDetect).Anchor = (AnchorStyles)6;
		((Control)BTNAutoDetect).Location = new Point(6, 224);
		((Control)BTNAutoDetect).Name = "BTNAutoDetect";
		((Control)BTNAutoDetect).Size = new Size(155, 47);
		BTNAutoDetect.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNAutoDetect).TabIndex = 7;
		((Control)BTNAutoDetect).Text = "تشخیص اتوماتیک پورت";
		BTNAutoDetect.Tooltip = "ذخیره تنظیمات";
		((Control)BTNAutoDetect).Click += BTNAutoDetect_Click;
		((Control)Panel1).Anchor = (AnchorStyles)13;
		((Control)Panel1).Controls.Add((Control)(object)LabelX16);
		((Control)Panel1).Controls.Add((Control)(object)BTN_File);
		((Control)Panel1).Controls.Add((Control)(object)LBL_AutoAnswer);
		((Control)Panel1).Location = new Point(10, 185);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(623, 29);
		((Control)Panel1).TabIndex = 2;
		((Control)LabelX16).Anchor = (AnchorStyles)9;
		((Control)LabelX16).Location = new Point(532, 4);
		((Control)LabelX16).Name = "LabelX16";
		((Control)LabelX16).Size = new Size(87, 21);
		((Control)LabelX16).TabIndex = 4;
		((Control)LabelX16).Text = "منشی تلفنی :";
		LabelX16.TextAlign = (ContentAlignment)16;
		((Control)BTN_File).AccessibleRole = (AccessibleRole)43;
		((Control)BTN_File).Location = new Point(3, 4);
		((Control)BTN_File).Name = "BTN_File";
		((Control)BTN_File).Size = new Size(20, 20);
		BTN_File.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTN_File).TabIndex = 1;
		((Control)BTN_File).Text = "...";
		((Control)BTN_File).Click += BTNFile_Click;
		((Control)LBL_AutoAnswer).Anchor = (AnchorStyles)13;
		((Control)LBL_AutoAnswer).Location = new Point(29, 4);
		((Control)LBL_AutoAnswer).Name = "LBL_AutoAnswer";
		((Control)LBL_AutoAnswer).Size = new Size(497, 20);
		((Control)LBL_AutoAnswer).TabIndex = 0;
		((Control)LBL_AutoAnswer).Text = "------";
		((Control)CMB_PortSpeed).Anchor = (AnchorStyles)13;
		((ListControl)CMB_PortSpeed).DisplayMember = "Text";
		((ComboBox)CMB_PortSpeed).DrawMode = (DrawMode)1;
		((ComboBox)CMB_PortSpeed).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMB_PortSpeed).FormattingEnabled = true;
		((ComboBox)CMB_PortSpeed).ItemHeight = 15;
		CMB_PortSpeed.Items.AddRange(new object[14]
		{
			ComboItem1, ComboItem11, ComboItem3, ComboItem7, ComboItem8, ComboItem12, ComboItem4, ComboItem2, ComboItem9, ComboItem13,
			ComboItem5, ComboItem10, ComboItem14, ComboItem6
		});
		((Control)CMB_PortSpeed).Location = new Point(166, 250);
		((Control)CMB_PortSpeed).Name = "CMB_PortSpeed";
		((Control)CMB_PortSpeed).Size = new Size(393, 21);
		((ComboBox)CMB_PortSpeed).Sorted = true;
		CMB_PortSpeed.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CMB_PortSpeed).TabIndex = 6;
		ComboItem1.Text = "110";
		ComboItem11.Text = "115200";
		ComboItem3.Text = "1200";
		ComboItem7.Text = "14400";
		ComboItem8.Text = "19200";
		ComboItem12.Text = "230400";
		ComboItem4.Text = "2400";
		ComboItem2.Text = "300";
		ComboItem9.Text = "38400";
		ComboItem13.Text = "460800";
		ComboItem5.Text = "4800";
		ComboItem10.Text = "57600";
		ComboItem14.Text = "921600";
		ComboItem6.Text = "9600";
		((Control)CMB_PortNumber).Anchor = (AnchorStyles)13;
		((ListControl)CMB_PortNumber).DisplayMember = "Text";
		((ComboBox)CMB_PortNumber).DrawMode = (DrawMode)1;
		((ComboBox)CMB_PortNumber).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMB_PortNumber).FormattingEnabled = true;
		((ComboBox)CMB_PortNumber).ItemHeight = 15;
		((Control)CMB_PortNumber).Location = new Point(165, 223);
		((Control)CMB_PortNumber).Name = "CMB_PortNumber";
		((Control)CMB_PortNumber).Size = new Size(393, 21);
		CMB_PortNumber.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CMB_PortNumber).TabIndex = 4;
		((Control)LabelX2).Anchor = (AnchorStyles)9;
		((Control)LabelX2).Location = new Point(565, 250);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(83, 21);
		((Control)LabelX2).TabIndex = 5;
		((Control)LabelX2).Text = "سرعت پورت :";
		LabelX2.TextAlign = (ContentAlignment)16;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Location = new Point(564, 223);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(78, 21);
		((Control)LabelX1).TabIndex = 3;
		((Control)LabelX1).Text = "شماره پورت :";
		LabelX1.TextAlign = (ContentAlignment)16;
		((Control)Cb_AutoAnswer).Anchor = (AnchorStyles)13;
		Cb_AutoAnswer.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Cb_AutoAnswer).Location = new Point(12, 160);
		((Control)Cb_AutoAnswer).Name = "Cb_AutoAnswer";
		((Control)Cb_AutoAnswer).Size = new Size(627, 23);
		Cb_AutoAnswer.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Cb_AutoAnswer).TabIndex = 1;
		((Control)Cb_AutoAnswer).Text = "پاسخگویی اتوماتیک";
		Cb_AutoAnswer.CheckValueChanged += CBAutoAnswer_CheckedChanged;
		((Control)Cb_AutoConnect).Anchor = (AnchorStyles)13;
		Cb_AutoConnect.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Cb_AutoConnect).Location = new Point(6, 127);
		((Control)Cb_AutoConnect).Name = "Cb_AutoConnect";
		((Control)Cb_AutoConnect).Size = new Size(636, 23);
		Cb_AutoConnect.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Cb_AutoConnect).TabIndex = 0;
		((Control)Cb_AutoConnect).Text = "برقراری ارتباط اتوماتیک";
		((Control)CHBDeliver).Anchor = (AnchorStyles)13;
		CHBDeliver.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)CHBDeliver).Location = new Point(6, 99);
		((Control)CHBDeliver).Name = "CHBDeliver";
		((Control)CHBDeliver).Size = new Size(636, 23);
		CHBDeliver.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CHBDeliver).TabIndex = 8;
		((Control)CHBDeliver).Text = "اعلام تحویل";
		((Control)GroupBox3).Location = new Point(6, 149);
		((Control)GroupBox3).Name = "GroupBox3";
		((Control)GroupBox3).Size = new Size(639, 69);
		((Control)GroupBox3).TabIndex = 14;
		GroupBox3.TabStop = false;
		((Control)GroupBox4).Anchor = (AnchorStyles)13;
		((Control)GroupBox4).Controls.Add((Control)(object)LabelX15);
		((Control)GroupBox4).Controls.Add((Control)(object)LabelX12);
		((Control)GroupBox4).Controls.Add((Control)(object)CountSent);
		((Control)GroupBox4).Controls.Add((Control)(object)LabelX14);
		((Control)GroupBox4).Location = new Point(11, 1);
		((Control)GroupBox4).Name = "GroupBox4";
		((Control)GroupBox4).Size = new Size(656, 47);
		((Control)GroupBox4).TabIndex = 17;
		GroupBox4.TabStop = false;
		((Control)GroupBox4).Text = "تنظیمات پیامک اینترنتی";
		((Control)LabelX15).Anchor = (AnchorStyles)9;
		((Control)LabelX15).Location = new Point(308, 16);
		((Control)LabelX15).Name = "LabelX15";
		((Control)LabelX15).Size = new Size(61, 21);
		((Control)LabelX15).TabIndex = 16;
		((Control)LabelX15).Text = "تایی";
		((Control)LabelX12).Anchor = (AnchorStyles)9;
		((Control)LabelX12).Location = new Point(493, 18);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).Size = new Size(150, 21);
		((Control)LabelX12).TabIndex = 15;
		((Control)LabelX12).Text = "نحوه ارسال پیامک اینترنتی :";
		LabelX12.TextAlign = (ContentAlignment)16;
		((Control)CountSent).Anchor = (AnchorStyles)9;
		((Control)CountSent).Location = new Point(375, 18);
		CountSent.Maximum = new decimal(new int[4] { 100000, 0, 0, 0 });
		CountSent.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)CountSent).Name = "CountSent";
		((Control)CountSent).Size = new Size(40, 20);
		((Control)CountSent).TabIndex = 10;
		CountSent.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)LabelX14).Anchor = (AnchorStyles)9;
		((Control)LabelX14).Location = new Point(426, 18);
		((Control)LabelX14).Name = "LabelX14";
		((Control)LabelX14).Size = new Size(61, 21);
		((Control)LabelX14).TabIndex = 15;
		((Control)LabelX14).Text = "گروه های";
		LabelX14.TextAlign = (ContentAlignment)16;
		TabItem1.AttachedControl = (Control)(object)TabControlPanel1;
		TabItem1.Name = "TabItem1";
		TabItem1.Text = "پیامک";
		((Control)TabControlPanel7).Controls.Add((Control)(object)GroupPanel10);
		TabControlPanel7.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel7).Dock = (DockStyle)5;
		((Control)TabControlPanel7).Location = new Point(0, 26);
		((Control)TabControlPanel7).Name = "TabControlPanel7";
		((Control)TabControlPanel7).Padding = new Padding(1);
		((Control)TabControlPanel7).Size = new Size(696, 498);
		TabControlPanel7.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel7.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel7.Style.Border = eBorderType.SingleLine;
		TabControlPanel7.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel7.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel7.Style.GradientAngle = 90;
		((Control)TabControlPanel7).TabIndex = 7;
		TabControlPanel7.TabItem = TabItem7;
		((Control)GroupPanel10).Anchor = (AnchorStyles)13;
		((Control)GroupPanel10).BackColor = Color.Transparent;
		GroupPanel10.CanvasColor = SystemColors.Control;
		GroupPanel10.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel10).Controls.Add((Control)(object)BtnSaveDial);
		((Control)GroupPanel10).Controls.Add((Control)(object)BtnExitDial);
		((Control)GroupPanel10).Controls.Add((Control)(object)GroupBoxDialUP);
		((Control)GroupPanel10).Controls.Add((Control)(object)ChbContactDial);
		GroupPanel10.DisabledBackColor = Color.Empty;
		((Control)GroupPanel10).Location = new Point(4, 2);
		((Control)GroupPanel10).Name = "GroupPanel10";
		((Control)GroupPanel10).Size = new Size(689, 484);
		GroupPanel10.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel10.Style.BackColorGradientAngle = 90;
		GroupPanel10.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel10.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel10.Style.BorderBottomWidth = 1;
		GroupPanel10.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel10.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel10.Style.BorderLeftWidth = 1;
		GroupPanel10.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel10.Style.BorderRightWidth = 1;
		GroupPanel10.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel10.Style.BorderTopWidth = 1;
		GroupPanel10.Style.CornerDiameter = 4;
		GroupPanel10.Style.CornerType = eCornerType.Rounded;
		GroupPanel10.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel10.Style.TextColor = Color.Blue;
		GroupPanel10.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel10.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		GroupPanel10.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel10.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel10).TabIndex = 7;
		((Control)BtnSaveDial).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSaveDial).Anchor = (AnchorStyles)9;
		BtnSaveDial.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSaveDial).Location = new Point(94, 406);
		((Control)BtnSaveDial).Name = "BtnSaveDial";
		((Control)BtnSaveDial).Size = new Size(77, 31);
		BtnSaveDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSaveDial).TabIndex = 1;
		((Control)BtnSaveDial).Text = "ذخیره";
		BtnSaveDial.Tooltip = "ذخیره تنظیمات";
		((Control)BtnExitDial).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExitDial).Anchor = (AnchorStyles)9;
		BtnExitDial.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnExitDial).Location = new Point(11, 406);
		((Control)BtnExitDial).Name = "BtnExitDial";
		((Control)BtnExitDial).Size = new Size(77, 31);
		BtnExitDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExitDial).TabIndex = 2;
		((Control)BtnExitDial).TabStop = false;
		((Control)BtnExitDial).Text = "خروج";
		BtnExitDial.Tooltip = "خروج";
		((Control)GroupBoxDialUP).Anchor = (AnchorStyles)13;
		((Control)GroupBoxDialUP).BackColor = Color.Transparent;
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)LabelX4);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)NumericUpDown1);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)BtnAutoPort);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)CMBSpeedPortDial);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)CMBPortNumberDial);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)LabelX8);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)LabelX9);
		((Control)GroupBoxDialUP).Controls.Add((Control)(object)ChbAnswerDial);
		((Control)GroupBoxDialUP).Location = new Point(13, 46);
		((Control)GroupBoxDialUP).Name = "GroupBoxDialUP";
		((Control)GroupBoxDialUP).Size = new Size(651, 344);
		((Control)GroupBoxDialUP).TabIndex = 0;
		GroupBoxDialUP.TabStop = false;
		((Control)GroupBoxDialUP).Text = "تنظیمات";
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((Control)LabelX4).Location = new Point(366, 22);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(72, 21);
		((Control)LabelX4).TabIndex = 9;
		((Control)LabelX4).Text = "زنگ";
		((Control)NumericUpDown1).Anchor = (AnchorStyles)9;
		((Control)NumericUpDown1).Location = new Point(441, 22);
		((Control)NumericUpDown1).Name = "NumericUpDown1";
		((Control)NumericUpDown1).Size = new Size(39, 20);
		((Control)NumericUpDown1).TabIndex = 8;
		((Control)BtnAutoPort).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAutoPort).Anchor = (AnchorStyles)14;
		((Control)BtnAutoPort).Location = new Point(14, 170);
		((Control)BtnAutoPort).Name = "BtnAutoPort";
		((Control)BtnAutoPort).Size = new Size(627, 168);
		BtnAutoPort.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAutoPort).TabIndex = 7;
		((Control)BtnAutoPort).Text = "تشخیص اتوماتیک پورت";
		BtnAutoPort.Tooltip = "ذخیره تنظیمات";
		((Control)CMBSpeedPortDial).Anchor = (AnchorStyles)13;
		((ListControl)CMBSpeedPortDial).DisplayMember = "Text";
		((ComboBox)CMBSpeedPortDial).DrawMode = (DrawMode)1;
		((ComboBox)CMBSpeedPortDial).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBSpeedPortDial).FormattingEnabled = true;
		((ComboBox)CMBSpeedPortDial).ItemHeight = 15;
		CMBSpeedPortDial.Items.AddRange(new object[14]
		{
			ComboItem29, ComboItem30, ComboItem31, ComboItem32, ComboItem33, ComboItem34, ComboItem35, ComboItem36, ComboItem37, ComboItem38,
			ComboItem39, ComboItem40, ComboItem41, ComboItem42
		});
		((Control)CMBSpeedPortDial).Location = new Point(13, 101);
		((Control)CMBSpeedPortDial).Name = "CMBSpeedPortDial";
		((Control)CMBSpeedPortDial).Size = new Size(544, 21);
		((ComboBox)CMBSpeedPortDial).Sorted = true;
		CMBSpeedPortDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CMBSpeedPortDial).TabIndex = 6;
		ComboItem29.Text = "110";
		ComboItem30.Text = "115200";
		ComboItem31.Text = "1200";
		ComboItem32.Text = "14400";
		ComboItem33.Text = "19200";
		ComboItem34.Text = "230400";
		ComboItem35.Text = "2400";
		ComboItem36.Text = "300";
		ComboItem37.Text = "38400";
		ComboItem38.Text = "460800";
		ComboItem39.Text = "4800";
		ComboItem40.Text = "57600";
		ComboItem41.Text = "921600";
		ComboItem42.Text = "9600";
		((Control)CMBPortNumberDial).Anchor = (AnchorStyles)13;
		((ListControl)CMBPortNumberDial).DisplayMember = "Text";
		((ComboBox)CMBPortNumberDial).DrawMode = (DrawMode)1;
		((ComboBox)CMBPortNumberDial).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBPortNumberDial).FormattingEnabled = true;
		((ComboBox)CMBPortNumberDial).ItemHeight = 15;
		((Control)CMBPortNumberDial).Location = new Point(13, 61);
		((Control)CMBPortNumberDial).Name = "CMBPortNumberDial";
		((Control)CMBPortNumberDial).Size = new Size(544, 21);
		CMBPortNumberDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CMBPortNumberDial).TabIndex = 4;
		((Control)LabelX8).Anchor = (AnchorStyles)9;
		((Control)LabelX8).Location = new Point(563, 100);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).Size = new Size(83, 21);
		((Control)LabelX8).TabIndex = 5;
		((Control)LabelX8).Text = "سرعت پورت :";
		LabelX8.TextAlign = (ContentAlignment)16;
		((Control)LabelX9).Anchor = (AnchorStyles)9;
		((Control)LabelX9).Location = new Point(563, 59);
		((Control)LabelX9).Name = "LabelX9";
		((Control)LabelX9).Size = new Size(78, 21);
		((Control)LabelX9).TabIndex = 3;
		((Control)LabelX9).Text = "شماره پورت :";
		LabelX9.TextAlign = (ContentAlignment)16;
		((Control)ChbAnswerDial).Anchor = (AnchorStyles)9;
		ChbAnswerDial.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)ChbAnswerDial).Location = new Point(476, 20);
		((Control)ChbAnswerDial).Name = "ChbAnswerDial";
		((Control)ChbAnswerDial).Size = new Size(165, 23);
		ChbAnswerDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ChbAnswerDial).TabIndex = 1;
		((Control)ChbAnswerDial).Text = "پاسخگویی اتوماتیک بعد از";
		((Control)ChbContactDial).Anchor = (AnchorStyles)13;
		ChbContactDial.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)ChbContactDial).Location = new Point(43, 11);
		((Control)ChbContactDial).Name = "ChbContactDial";
		((Control)ChbContactDial).Size = new Size(621, 23);
		ChbContactDial.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ChbContactDial).TabIndex = 0;
		((Control)ChbContactDial).Text = "برقراری ارتباط اتوماتیک";
		TabItem7.AttachedControl = (Control)(object)TabControlPanel7;
		TabItem7.Name = "TabItem7";
		TabItem7.Text = "مودم";
		TabItem7.Visible = false;
		((Control)TabControlPanel3).Controls.Add((Control)(object)GroupPanel7);
		TabControlPanel3.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel3).Dock = (DockStyle)5;
		((Control)TabControlPanel3).Location = new Point(0, 26);
		((Control)TabControlPanel3).Name = "TabControlPanel3";
		((Control)TabControlPanel3).Padding = new Padding(1);
		((Control)TabControlPanel3).Size = new Size(696, 498);
		TabControlPanel3.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel3.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel3.Style.Border = eBorderType.SingleLine;
		TabControlPanel3.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel3.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel3.Style.GradientAngle = 90;
		((Control)TabControlPanel3).TabIndex = 3;
		TabControlPanel3.TabItem = TabItem3;
		((Control)TabControlPanel3).Text = "بانک اطلاعاتی";
		((Control)GroupPanel7).Anchor = (AnchorStyles)15;
		((Control)GroupPanel7).BackColor = Color.Transparent;
		GroupPanel7.CanvasColor = SystemColors.Control;
		GroupPanel7.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		GroupPanel7.DisabledBackColor = Color.Empty;
		((Control)GroupPanel7).Location = new Point(3, 4);
		((Control)GroupPanel7).Name = "GroupPanel7";
		((Control)GroupPanel7).Size = new Size(689, 492);
		GroupPanel7.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel7.Style.BackColorGradientAngle = 90;
		GroupPanel7.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel7.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel7.Style.BorderBottomWidth = 1;
		GroupPanel7.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel7.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel7.Style.BorderLeftWidth = 1;
		GroupPanel7.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel7.Style.BorderRightWidth = 1;
		GroupPanel7.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel7.Style.BorderTopWidth = 1;
		GroupPanel7.Style.CornerDiameter = 4;
		GroupPanel7.Style.CornerType = eCornerType.Rounded;
		GroupPanel7.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel7.Style.TextColor = Color.Blue;
		GroupPanel7.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel7.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		GroupPanel7.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel7.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel7).TabIndex = 4;
		TabItem3.AttachedControl = (Control)(object)TabControlPanel3;
		TabItem3.Name = "TabItem3";
		TabItem3.Text = " بانک اطلاعاتی";
		((Control)TabControlPanel2).Controls.Add((Control)(object)GroupPanel1);
		TabControlPanel2.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel2).Dock = (DockStyle)5;
		((Control)TabControlPanel2).Location = new Point(0, 26);
		((Control)TabControlPanel2).Name = "TabControlPanel2";
		((Control)TabControlPanel2).Padding = new Padding(1);
		((Control)TabControlPanel2).Size = new Size(696, 498);
		TabControlPanel2.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel2.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel2.Style.Border = eBorderType.SingleLine;
		TabControlPanel2.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel2.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel2.Style.GradientAngle = 90;
		((Control)TabControlPanel2).TabIndex = 2;
		TabControlPanel2.TabItem = TabItem2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)15;
		((Control)GroupPanel1).BackColor = Color.Transparent;
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnSave_Sound);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnExit_Sound);
		((Control)GroupPanel1).Controls.Add((Control)(object)Gb_Sound);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 4);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(689, 492);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel1.Style.TextColor = Color.Blue;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)BtnSave_Sound).AccessibleRole = (AccessibleRole)43;
		BtnSave_Sound.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSave_Sound).Location = new Point(94, 406);
		((Control)BtnSave_Sound).Name = "BtnSave_Sound";
		((Control)BtnSave_Sound).Size = new Size(72, 31);
		BtnSave_Sound.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSave_Sound).TabIndex = 0;
		((Control)BtnSave_Sound).Text = "ذخیره";
		BtnSave_Sound.Tooltip = "ذخیره تنظیمات";
		((Control)BtnSave_Sound).Click += BtnSave_Sound_Click;
		((Control)BtnExit_Sound).AccessibleRole = (AccessibleRole)43;
		BtnExit_Sound.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnExit_Sound).Location = new Point(11, 406);
		((Control)BtnExit_Sound).Name = "BtnExit_Sound";
		((Control)BtnExit_Sound).Size = new Size(72, 31);
		BtnExit_Sound.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExit_Sound).TabIndex = 1;
		((Control)BtnExit_Sound).TabStop = false;
		((Control)BtnExit_Sound).Text = "خروج";
		BtnExit_Sound.Tooltip = "خروج";
		((Control)Gb_Sound).Anchor = (AnchorStyles)13;
		((Control)Gb_Sound).BackColor = Color.Transparent;
		((Control)Gb_Sound).Controls.Add((Control)(object)Chb_Delivery);
		((Control)Gb_Sound).Controls.Add((Control)(object)Chb_Call);
		((Control)Gb_Sound).Controls.Add((Control)(object)Chb_Recieve);
		((Control)Gb_Sound).Controls.Add((Control)(object)Chb_Send);
		((Control)Gb_Sound).Controls.Add((Control)(object)Gp_Delivery);
		((Control)Gb_Sound).Controls.Add((Control)(object)Gp_Call);
		((Control)Gb_Sound).Controls.Add((Control)(object)Gp_Recieve);
		((Control)Gb_Sound).Controls.Add((Control)(object)Gp_Send);
		((Control)Gb_Sound).Location = new Point(12, -2);
		((Control)Gb_Sound).Name = "Gb_Sound";
		((Control)Gb_Sound).Size = new Size(659, 388);
		((Control)Gb_Sound).TabIndex = 0;
		Gb_Sound.TabStop = false;
		((Control)Gb_Sound).Text = "تنظیمات";
		((Control)Chb_Delivery).Anchor = (AnchorStyles)9;
		Chb_Delivery.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Chb_Delivery).Location = new Point(540, 131);
		((Control)Chb_Delivery).Name = "Chb_Delivery";
		((Control)Chb_Delivery).Size = new Size(115, 34);
		Chb_Delivery.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Chb_Delivery).TabIndex = 6;
		((Control)Chb_Delivery).Text = "آهنگ اعلام تحویل";
		((Control)Chb_Call).Anchor = (AnchorStyles)9;
		Chb_Call.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Chb_Call).Location = new Point(555, 91);
		((Control)Chb_Call).Name = "Chb_Call";
		((Control)Chb_Call).Size = new Size(100, 34);
		Chb_Call.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Chb_Call).TabIndex = 4;
		((Control)Chb_Call).Text = "آهنگ تماس ";
		((Control)Chb_Recieve).Anchor = (AnchorStyles)9;
		Chb_Recieve.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Chb_Recieve).Location = new Point(540, 51);
		((Control)Chb_Recieve).Name = "Chb_Recieve";
		((Control)Chb_Recieve).Size = new Size(115, 34);
		Chb_Recieve.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Chb_Recieve).TabIndex = 2;
		((Control)Chb_Recieve).Text = "آهنگ دریافت پیام";
		((Control)Chb_Send).Anchor = (AnchorStyles)9;
		Chb_Send.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)Chb_Send).Location = new Point(540, 15);
		((Control)Chb_Send).Name = "Chb_Send";
		((Control)Chb_Send).Size = new Size(115, 34);
		Chb_Send.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Chb_Send).TabIndex = 0;
		((Control)Chb_Send).Text = "آهنگ ارسال پیام";
		((Control)Gp_Delivery).Anchor = (AnchorStyles)13;
		Gp_Delivery.CanvasColor = SystemColors.Control;
		Gp_Delivery.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gp_Delivery).Controls.Add((Control)(object)Txt_Delivery);
		((Control)Gp_Delivery).Controls.Add((Control)(object)Btnplay_Delivery);
		((Control)Gp_Delivery).Controls.Add((Control)(object)Btn_Delivery);
		Gp_Delivery.DisabledBackColor = Color.Empty;
		((Control)Gp_Delivery).Location = new Point(6, 133);
		((Control)Gp_Delivery).Name = "Gp_Delivery";
		((Control)Gp_Delivery).Size = new Size(528, 32);
		Gp_Delivery.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gp_Delivery.Style.BackColorGradientAngle = 90;
		Gp_Delivery.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gp_Delivery.Style.BorderBottom = eStyleBorderType.Solid;
		Gp_Delivery.Style.BorderBottomWidth = 1;
		Gp_Delivery.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gp_Delivery.Style.BorderLeft = eStyleBorderType.Solid;
		Gp_Delivery.Style.BorderLeftWidth = 1;
		Gp_Delivery.Style.BorderRight = eStyleBorderType.Solid;
		Gp_Delivery.Style.BorderRightWidth = 1;
		Gp_Delivery.Style.BorderTop = eStyleBorderType.Solid;
		Gp_Delivery.Style.BorderTopWidth = 1;
		Gp_Delivery.Style.CornerDiameter = 4;
		Gp_Delivery.Style.CornerType = eCornerType.Rounded;
		Gp_Delivery.Style.TextAlignment = eStyleTextAlignment.Center;
		Gp_Delivery.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gp_Delivery.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gp_Delivery.StyleMouseDown.CornerType = eCornerType.Square;
		Gp_Delivery.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gp_Delivery).TabIndex = 7;
		((Control)Txt_Delivery).Anchor = (AnchorStyles)13;
		Txt_Delivery.Border.Class = "TextBoxBorder";
		Txt_Delivery.Border.CornerType = eCornerType.Square;
		((Control)Txt_Delivery).Location = new Point(67, 3);
		((Control)Txt_Delivery).Name = "Txt_Delivery";
		((TextBoxBase)Txt_Delivery).ReadOnly = true;
		((Control)Txt_Delivery).Size = new Size(434, 20);
		((Control)Txt_Delivery).TabIndex = 0;
		((Control)Btnplay_Delivery).AccessibleRole = (AccessibleRole)43;
		Btnplay_Delivery.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Btnplay_Delivery).Location = new Point(5, 2);
		((Control)Btnplay_Delivery).Name = "Btnplay_Delivery";
		((Control)Btnplay_Delivery).Size = new Size(24, 24);
		Btnplay_Delivery.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Btnplay_Delivery).TabIndex = 1;
		((Control)Btnplay_Delivery).Click += Btnplay_Delivery_Click;
		((Control)Btn_Delivery).AccessibleRole = (AccessibleRole)43;
		Btn_Delivery.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Delivery).Location = new Point(36, 0);
		((Control)Btn_Delivery).Name = "Btn_Delivery";
		((Control)Btn_Delivery).Size = new Size(25, 25);
		Btn_Delivery.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Btn_Delivery).TabIndex = 0;
		((Control)Btn_Delivery).Text = "...";
		((Control)Btn_Delivery).Click += Btn_Delivery_Click;
		((Control)Gp_Call).Anchor = (AnchorStyles)13;
		Gp_Call.CanvasColor = SystemColors.Control;
		Gp_Call.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gp_Call).Controls.Add((Control)(object)Txt_Call);
		((Control)Gp_Call).Controls.Add((Control)(object)Btn_call);
		((Control)Gp_Call).Controls.Add((Control)(object)BtnPlay_Call);
		Gp_Call.DisabledBackColor = Color.Empty;
		((Control)Gp_Call).Location = new Point(6, 93);
		((Control)Gp_Call).Name = "Gp_Call";
		((Control)Gp_Call).Size = new Size(528, 32);
		Gp_Call.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gp_Call.Style.BackColorGradientAngle = 90;
		Gp_Call.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gp_Call.Style.BorderBottom = eStyleBorderType.Solid;
		Gp_Call.Style.BorderBottomWidth = 1;
		Gp_Call.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gp_Call.Style.BorderLeft = eStyleBorderType.Solid;
		Gp_Call.Style.BorderLeftWidth = 1;
		Gp_Call.Style.BorderRight = eStyleBorderType.Solid;
		Gp_Call.Style.BorderRightWidth = 1;
		Gp_Call.Style.BorderTop = eStyleBorderType.Solid;
		Gp_Call.Style.BorderTopWidth = 1;
		Gp_Call.Style.CornerDiameter = 4;
		Gp_Call.Style.CornerType = eCornerType.Rounded;
		Gp_Call.Style.TextAlignment = eStyleTextAlignment.Center;
		Gp_Call.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gp_Call.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gp_Call.StyleMouseDown.CornerType = eCornerType.Square;
		Gp_Call.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gp_Call).TabIndex = 5;
		((Control)Txt_Call).Anchor = (AnchorStyles)13;
		Txt_Call.Border.Class = "TextBoxBorder";
		Txt_Call.Border.CornerType = eCornerType.Square;
		((Control)Txt_Call).Location = new Point(67, 5);
		((Control)Txt_Call).Name = "Txt_Call";
		((TextBoxBase)Txt_Call).ReadOnly = true;
		((Control)Txt_Call).Size = new Size(434, 20);
		((Control)Txt_Call).TabIndex = 0;
		((Control)Btn_call).AccessibleRole = (AccessibleRole)43;
		Btn_call.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_call).Location = new Point(38, 1);
		((Control)Btn_call).Name = "Btn_call";
		((Control)Btn_call).Size = new Size(25, 25);
		Btn_call.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Btn_call).TabIndex = 1;
		((Control)Btn_call).Text = "...";
		((Control)Btn_call).Click += Btn_call_Click;
		((Control)BtnPlay_Call).AccessibleRole = (AccessibleRole)43;
		BtnPlay_Call.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnPlay_Call).Location = new Point(6, 1);
		((Control)BtnPlay_Call).Name = "BtnPlay_Call";
		((Control)BtnPlay_Call).Size = new Size(24, 24);
		BtnPlay_Call.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnPlay_Call).TabIndex = 2;
		((Control)BtnPlay_Call).Click += BtnPlay_Call_Click;
		((Control)Gp_Recieve).Anchor = (AnchorStyles)13;
		Gp_Recieve.CanvasColor = SystemColors.Control;
		Gp_Recieve.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gp_Recieve).Controls.Add((Control)(object)Txt_Recieve);
		((Control)Gp_Recieve).Controls.Add((Control)(object)Btn_Recieve);
		((Control)Gp_Recieve).Controls.Add((Control)(object)BtnPlay_Recieve);
		Gp_Recieve.DisabledBackColor = Color.Empty;
		((Control)Gp_Recieve).Location = new Point(6, 54);
		((Control)Gp_Recieve).Name = "Gp_Recieve";
		((Control)Gp_Recieve).Size = new Size(528, 32);
		Gp_Recieve.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gp_Recieve.Style.BackColorGradientAngle = 90;
		Gp_Recieve.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gp_Recieve.Style.BorderBottom = eStyleBorderType.Solid;
		Gp_Recieve.Style.BorderBottomWidth = 1;
		Gp_Recieve.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gp_Recieve.Style.BorderLeft = eStyleBorderType.Solid;
		Gp_Recieve.Style.BorderLeftWidth = 1;
		Gp_Recieve.Style.BorderRight = eStyleBorderType.Solid;
		Gp_Recieve.Style.BorderRightWidth = 1;
		Gp_Recieve.Style.BorderTop = eStyleBorderType.Solid;
		Gp_Recieve.Style.BorderTopWidth = 1;
		Gp_Recieve.Style.CornerDiameter = 4;
		Gp_Recieve.Style.CornerType = eCornerType.Rounded;
		Gp_Recieve.Style.TextAlignment = eStyleTextAlignment.Center;
		Gp_Recieve.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gp_Recieve.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gp_Recieve.StyleMouseDown.CornerType = eCornerType.Square;
		Gp_Recieve.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gp_Recieve).TabIndex = 3;
		((Control)Txt_Recieve).Anchor = (AnchorStyles)13;
		Txt_Recieve.Border.Class = "TextBoxBorder";
		Txt_Recieve.Border.CornerType = eCornerType.Square;
		((Control)Txt_Recieve).Location = new Point(67, 2);
		((Control)Txt_Recieve).Name = "Txt_Recieve";
		((TextBoxBase)Txt_Recieve).ReadOnly = true;
		((Control)Txt_Recieve).Size = new Size(434, 20);
		((Control)Txt_Recieve).TabIndex = 0;
		((Control)Btn_Recieve).AccessibleRole = (AccessibleRole)43;
		Btn_Recieve.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Recieve).Location = new Point(37, 1);
		((Control)Btn_Recieve).Name = "Btn_Recieve";
		((Control)Btn_Recieve).Size = new Size(25, 25);
		Btn_Recieve.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Btn_Recieve).TabIndex = 1;
		((Control)Btn_Recieve).Text = "...";
		((Control)Btn_Recieve).Click += Btn_Recieve_Click;
		((Control)BtnPlay_Recieve).AccessibleRole = (AccessibleRole)43;
		BtnPlay_Recieve.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnPlay_Recieve).Location = new Point(5, 1);
		((Control)BtnPlay_Recieve).Name = "BtnPlay_Recieve";
		((Control)BtnPlay_Recieve).Size = new Size(24, 24);
		BtnPlay_Recieve.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnPlay_Recieve).TabIndex = 2;
		((Control)BtnPlay_Recieve).Click += BtnPlay_Recieve_Click;
		((Control)Gp_Send).Anchor = (AnchorStyles)13;
		Gp_Send.CanvasColor = SystemColors.Control;
		Gp_Send.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gp_Send).Controls.Add((Control)(object)Txt_Send);
		((Control)Gp_Send).Controls.Add((Control)(object)Btn_Send);
		((Control)Gp_Send).Controls.Add((Control)(object)BtnPlay_Send);
		Gp_Send.DisabledBackColor = Color.Empty;
		((Control)Gp_Send).Location = new Point(6, 16);
		((Control)Gp_Send).Name = "Gp_Send";
		((Control)Gp_Send).Size = new Size(528, 32);
		Gp_Send.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gp_Send.Style.BackColorGradientAngle = 90;
		Gp_Send.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gp_Send.Style.BorderBottom = eStyleBorderType.Solid;
		Gp_Send.Style.BorderBottomWidth = 1;
		Gp_Send.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gp_Send.Style.BorderLeft = eStyleBorderType.Solid;
		Gp_Send.Style.BorderLeftWidth = 1;
		Gp_Send.Style.BorderRight = eStyleBorderType.Solid;
		Gp_Send.Style.BorderRightWidth = 1;
		Gp_Send.Style.BorderTop = eStyleBorderType.Solid;
		Gp_Send.Style.BorderTopWidth = 1;
		Gp_Send.Style.CornerDiameter = 4;
		Gp_Send.Style.CornerType = eCornerType.Rounded;
		Gp_Send.Style.TextAlignment = eStyleTextAlignment.Center;
		Gp_Send.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gp_Send.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gp_Send.StyleMouseDown.CornerType = eCornerType.Square;
		Gp_Send.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gp_Send).TabIndex = 1;
		((Control)Txt_Send).Anchor = (AnchorStyles)13;
		Txt_Send.Border.Class = "TextBoxBorder";
		Txt_Send.Border.CornerType = eCornerType.Square;
		((Control)Txt_Send).Location = new Point(67, 3);
		((Control)Txt_Send).Name = "Txt_Send";
		((TextBoxBase)Txt_Send).ReadOnly = true;
		((Control)Txt_Send).Size = new Size(434, 20);
		((Control)Txt_Send).TabIndex = 0;
		((Control)Btn_Send).AccessibleRole = (AccessibleRole)43;
		Btn_Send.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Send).Location = new Point(37, 0);
		((Control)Btn_Send).Name = "Btn_Send";
		((Control)Btn_Send).Size = new Size(25, 25);
		Btn_Send.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Btn_Send).TabIndex = 1;
		((Control)Btn_Send).Text = "...";
		((Control)Btn_Send).Click += Btn_Send_Click;
		((Control)BtnPlay_Send).AccessibleRole = (AccessibleRole)43;
		BtnPlay_Send.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnPlay_Send).Location = new Point(6, 1);
		((Control)BtnPlay_Send).Name = "BtnPlay_Send";
		((Control)BtnPlay_Send).Size = new Size(24, 24);
		BtnPlay_Send.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnPlay_Send).TabIndex = 2;
		((Control)BtnPlay_Send).Click += BtnPlay_Send_Click;
		TabItem2.AttachedControl = (Control)(object)TabControlPanel2;
		TabItem2.Name = "TabItem2";
		TabItem2.Text = " صدا";
		((Control)TabControlPanel4).Controls.Add((Control)(object)GroupPanel4);
		TabControlPanel4.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel4).Dock = (DockStyle)5;
		((Control)TabControlPanel4).Location = new Point(0, 26);
		((Control)TabControlPanel4).Name = "TabControlPanel4";
		((Control)TabControlPanel4).Padding = new Padding(1);
		((Control)TabControlPanel4).Size = new Size(696, 498);
		TabControlPanel4.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel4.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel4.Style.Border = eBorderType.SingleLine;
		TabControlPanel4.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel4.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel4.Style.GradientAngle = 90;
		((Control)TabControlPanel4).TabIndex = 4;
		TabControlPanel4.TabItem = TabItem4;
		((Control)GroupPanel4).Anchor = (AnchorStyles)15;
		((Control)GroupPanel4).BackColor = Color.Transparent;
		GroupPanel4.CanvasColor = SystemColors.Control;
		GroupPanel4.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel4).Controls.Add((Control)(object)PNLDialup);
		((Control)GroupPanel4).Controls.Add((Control)(object)BtnAccept);
		((Control)GroupPanel4).Controls.Add((Control)(object)BtnExit);
		GroupPanel4.DisabledBackColor = Color.Empty;
		((Control)GroupPanel4).Location = new Point(3, 4);
		((Control)GroupPanel4).Name = "GroupPanel4";
		((Control)GroupPanel4).Size = new Size(689, 492);
		GroupPanel4.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel4.Style.BackColorGradientAngle = 90;
		GroupPanel4.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel4.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderBottomWidth = 1;
		GroupPanel4.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel4.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderLeftWidth = 1;
		GroupPanel4.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderRightWidth = 1;
		GroupPanel4.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderTopWidth = 1;
		GroupPanel4.Style.CornerDiameter = 4;
		GroupPanel4.Style.CornerType = eCornerType.Rounded;
		GroupPanel4.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel4.Style.TextColor = Color.Blue;
		GroupPanel4.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel4.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		GroupPanel4.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel4.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel4).TabIndex = 5;
		((Control)PNLDialup).Anchor = (AnchorStyles)13;
		PNLDialup.CanvasColor = SystemColors.Control;
		PNLDialup.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)PNLDialup).Controls.Add((Control)(object)LblTemp);
		((Control)PNLDialup).Controls.Add((Control)(object)LblTemplate);
		((Control)PNLDialup).Controls.Add((Control)(object)LblMsg);
		((Control)PNLDialup).Controls.Add((Control)(object)TxtTem);
		((Control)PNLDialup).Controls.Add((Control)(object)TxtMsg);
		((Control)PNLDialup).Controls.Add((Control)(object)RBtnCall);
		((Control)PNLDialup).Controls.Add((Control)(object)RBtnSms);
		((Control)PNLDialup).Controls.Add((Control)(object)CheckBoxSMS);
		((Control)PNLDialup).Controls.Add((Control)(object)CheckBoxcall);
		PNLDialup.DisabledBackColor = Color.Empty;
		((Control)PNLDialup).Location = new Point(18, 0);
		((Control)PNLDialup).Name = "PNLDialup";
		((Control)PNLDialup).Size = new Size(647, 391);
		PNLDialup.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		PNLDialup.Style.BackColorGradientAngle = 90;
		PNLDialup.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		PNLDialup.Style.BorderBottom = eStyleBorderType.Solid;
		PNLDialup.Style.BorderBottomWidth = 1;
		PNLDialup.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		PNLDialup.Style.BorderLeft = eStyleBorderType.Solid;
		PNLDialup.Style.BorderLeftWidth = 1;
		PNLDialup.Style.BorderRight = eStyleBorderType.Solid;
		PNLDialup.Style.BorderRightWidth = 1;
		PNLDialup.Style.BorderTop = eStyleBorderType.Solid;
		PNLDialup.Style.BorderTopWidth = 1;
		PNLDialup.Style.CornerDiameter = 4;
		PNLDialup.Style.CornerType = eCornerType.Rounded;
		PNLDialup.Style.TextAlignment = eStyleTextAlignment.Center;
		PNLDialup.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		PNLDialup.Style.TextLineAlignment = eStyleTextAlignment.Near;
		PNLDialup.StyleMouseDown.CornerType = eCornerType.Square;
		PNLDialup.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)PNLDialup).TabIndex = 4;
		((Control)LblTemp).Anchor = (AnchorStyles)9;
		((Control)LblTemp).Enabled = false;
		((Control)LblTemp).ForeColor = SystemColors.Desktop;
		((Control)LblTemp).Location = new Point(475, 205);
		((Control)LblTemp).Name = "LblTemp";
		((Control)LblTemp).Size = new Size(114, 23);
		((Control)LblTemp).TabIndex = 8;
		((Control)LblTemp).Text = "از طریق ارسال پیامک";
		((Control)LblTemplate).Anchor = (AnchorStyles)9;
		((Control)LblTemplate).Enabled = false;
		((Control)LblTemplate).Location = new Point(496, 229);
		((Control)LblTemplate).Name = "LblTemplate";
		((Control)LblTemplate).Size = new Size(67, 23);
		((Control)LblTemplate).TabIndex = 7;
		((Control)LblTemplate).Text = "الگوی پیام :";
		((Control)LblMsg).Anchor = (AnchorStyles)9;
		((Control)LblMsg).Enabled = false;
		((Control)LblMsg).Location = new Point(496, 72);
		((Control)LblMsg).Name = "LblMsg";
		((Control)LblMsg).Size = new Size(67, 23);
		((Control)LblMsg).TabIndex = 6;
		((Control)LblMsg).Text = "الگوی پیام :";
		((Control)TxtTem).Anchor = (AnchorStyles)13;
		TxtTem.Border.Class = "TextBoxBorder";
		TxtTem.Border.CornerType = eCornerType.Square;
		((Control)TxtTem).Enabled = false;
		((Control)TxtTem).Location = new Point(4, 234);
		((TextBoxBase)TxtTem).Multiline = true;
		((Control)TxtTem).Name = "TxtTem";
		((TextBox)TxtTem).ScrollBars = (ScrollBars)2;
		((Control)TxtTem).Size = new Size(490, 107);
		((Control)TxtTem).TabIndex = 5;
		((Control)TxtMsg).Anchor = (AnchorStyles)13;
		TxtMsg.Border.Class = "TextBoxBorder";
		TxtMsg.Border.CornerType = eCornerType.Square;
		((Control)TxtMsg).Enabled = false;
		((Control)TxtMsg).Location = new Point(3, 75);
		((TextBoxBase)TxtMsg).Multiline = true;
		((Control)TxtMsg).Name = "TxtMsg";
		((TextBox)TxtMsg).ScrollBars = (ScrollBars)2;
		((Control)TxtMsg).Size = new Size(490, 90);
		((Control)TxtMsg).TabIndex = 4;
		((Control)RBtnCall).Anchor = (AnchorStyles)9;
		((Control)RBtnCall).AutoSize = true;
		((Control)RBtnCall).ForeColor = SystemColors.Desktop;
		((Control)RBtnCall).Location = new Point(463, 29);
		((Control)RBtnCall).Name = "RBtnCall";
		((Control)RBtnCall).Size = new Size(126, 17);
		((Control)RBtnCall).TabIndex = 2;
		RBtnCall.TabStop = true;
		((Control)RBtnCall).Text = "از طریق برقراری تماس";
		((ButtonBase)RBtnCall).UseVisualStyleBackColor = true;
		((Control)RBtnSms).Anchor = (AnchorStyles)9;
		((Control)RBtnSms).AutoSize = true;
		((Control)RBtnSms).ForeColor = SystemColors.Desktop;
		((Control)RBtnSms).Location = new Point(471, 46);
		((Control)RBtnSms).Name = "RBtnSms";
		((Control)RBtnSms).Size = new Size(118, 17);
		((Control)RBtnSms).TabIndex = 1;
		RBtnSms.TabStop = true;
		((Control)RBtnSms).Text = "از طریق ارسال پیامک";
		((ButtonBase)RBtnSms).UseVisualStyleBackColor = true;
		((Control)CheckBoxSMS).Anchor = (AnchorStyles)9;
		((Control)CheckBoxSMS).BackColor = Color.Transparent;
		CheckBoxSMS.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)CheckBoxSMS).Location = new Point(390, 176);
		((Control)CheckBoxSMS).Name = "CheckBoxSMS";
		((Control)CheckBoxSMS).Size = new Size(232, 23);
		CheckBoxSMS.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CheckBoxSMS).TabIndex = 1;
		((Control)CheckBoxSMS).Text = "انتقال پیامک ها ی دریافتی به سیم کارت";
		((Control)CheckBoxcall).Anchor = (AnchorStyles)9;
		((Control)CheckBoxcall).BackColor = Color.Transparent;
		CheckBoxcall.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)CheckBoxcall).Location = new Point(390, 2);
		((Control)CheckBoxcall).Name = "CheckBoxcall";
		((Control)CheckBoxcall).Size = new Size(232, 23);
		CheckBoxcall.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CheckBoxcall).TabIndex = 0;
		((Control)CheckBoxcall).Text = "انتقال تماس های دریافتی به سیم کارت";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnAccept).Location = new Point(94, 406);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).Size = new Size(84, 31);
		BtnAccept.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "ذخیره";
		BtnAccept.Tooltip = "ذخیره تنظیمات";
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		BtnExit.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnExit).Location = new Point(11, 406);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(84, 31);
		BtnExit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExit).TabIndex = 1;
		((Control)BtnExit).TabStop = false;
		((Control)BtnExit).Text = "خروج";
		BtnExit.Tooltip = "خروج";
		((Control)BtnExit).Click += BtnExitFont_Click_1;
		TabItem4.AttachedControl = (Control)(object)TabControlPanel4;
		TabItem4.Name = "TabItem4";
		TabItem4.Text = "انتقال به موبایل";
		((Control)TabControlPanel6).Controls.Add((Control)(object)PanelEx3);
		TabControlPanel6.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel6).Dock = (DockStyle)5;
		((Control)TabControlPanel6).Location = new Point(0, 26);
		((Control)TabControlPanel6).Name = "TabControlPanel6";
		((Control)TabControlPanel6).Padding = new Padding(1);
		((Control)TabControlPanel6).Size = new Size(696, 498);
		TabControlPanel6.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel6.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel6.Style.Border = eBorderType.SingleLine;
		TabControlPanel6.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel6.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel6.Style.GradientAngle = 90;
		((Control)TabControlPanel6).TabIndex = 6;
		TabControlPanel6.TabItem = TabItem6;
		((Control)PanelEx3).Anchor = (AnchorStyles)13;
		PanelEx3.CanvasColor = SystemColors.Control;
		PanelEx3.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx3).Controls.Add((Control)(object)BtnSave);
		((Control)PanelEx3).Controls.Add((Control)(object)BtnExitFont);
		((Control)PanelEx3).Controls.Add((Control)(object)GroupBox1);
		((Control)PanelEx3).Controls.Add((Control)(object)LBLPreviwe);
		((Control)PanelEx3).Controls.Add((Control)(object)GroupPanel9);
		PanelEx3.DisabledBackColor = Color.Empty;
		((Control)PanelEx3).Location = new Point(4, 4);
		((Control)PanelEx3).Name = "PanelEx3";
		((Control)PanelEx3).Size = new Size(688, 482);
		PanelEx3.Style.Alignment = (StringAlignment)1;
		PanelEx3.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx3.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx3.Style.Border = eBorderType.SingleLine;
		PanelEx3.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx3.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx3.Style.GradientAngle = 90;
		((Control)PanelEx3).TabIndex = 6;
		((Control)BtnSave).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSave).Anchor = (AnchorStyles)6;
		BtnSave.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSave).Location = new Point(94, 406);
		((Control)BtnSave).Name = "BtnSave";
		((Control)BtnSave).Size = new Size(84, 31);
		BtnSave.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSave).TabIndex = 6;
		((Control)BtnSave).Text = "ذخیره";
		BtnSave.Tooltip = "ذخیره تنظیمات";
		((Control)BtnExitFont).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExitFont).Anchor = (AnchorStyles)6;
		BtnExitFont.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnExitFont).Location = new Point(11, 406);
		((Control)BtnExitFont).Name = "BtnExitFont";
		((Control)BtnExitFont).Size = new Size(84, 31);
		BtnExitFont.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExitFont).TabIndex = 7;
		((Control)BtnExitFont).TabStop = false;
		((Control)BtnExitFont).Text = "خروج";
		BtnExitFont.Tooltip = "خروج";
		((Control)BtnExitFont).Click += BtnExitFont_Click;
		((Control)GroupBox1).Anchor = (AnchorStyles)13;
		((Control)GroupBox1).BackColor = Color.Transparent;
		((Control)GroupBox1).Controls.Add((Control)(object)LBLFontItalic);
		((Control)GroupBox1).Controls.Add((Control)(object)Label8);
		((Control)GroupBox1).Controls.Add((Control)(object)LBLFontBold);
		((Control)GroupBox1).Controls.Add((Control)(object)Label6);
		((Control)GroupBox1).Controls.Add((Control)(object)LBLFontSize);
		((Control)GroupBox1).Controls.Add((Control)(object)Label4);
		((Control)GroupBox1).Controls.Add((Control)(object)LBLFontName);
		((Control)GroupBox1).Controls.Add((Control)(object)Label2);
		((Control)GroupBox1).Controls.Add((Control)(object)BTNSelectFont);
		((Control)GroupBox1).Location = new Point(10, 37);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Size = new Size(668, 124);
		((Control)GroupBox1).TabIndex = 1;
		GroupBox1.TabStop = false;
		((Control)GroupBox1).Text = "مشخصات قلم انتخابی";
		((Control)LBLFontItalic).Anchor = (AnchorStyles)9;
		((Control)LBLFontItalic).ForeColor = Color.Blue;
		((Control)LBLFontItalic).Location = new Point(420, 93);
		((Control)LBLFontItalic).Name = "LBLFontItalic";
		((Control)LBLFontItalic).Size = new Size(126, 26);
		((Control)LBLFontItalic).TabIndex = 17;
		((Control)LBLFontItalic).Text = "بلی";
		LBLFontItalic.TextAlign = (ContentAlignment)16;
		((Control)Label8).Anchor = (AnchorStyles)9;
		((Control)Label8).Location = new Point(552, 93);
		((Control)Label8).Name = "Label8";
		((Control)Label8).Size = new Size(126, 26);
		((Control)Label8).TabIndex = 16;
		((Control)Label8).Text = "قلم کج (Italic) :";
		Label8.TextAlign = (ContentAlignment)64;
		((Control)LBLFontBold).Anchor = (AnchorStyles)9;
		((Control)LBLFontBold).ForeColor = Color.Blue;
		((Control)LBLFontBold).Location = new Point(420, 67);
		((Control)LBLFontBold).Name = "LBLFontBold";
		((Control)LBLFontBold).Size = new Size(126, 26);
		((Control)LBLFontBold).TabIndex = 15;
		((Control)LBLFontBold).Text = "بلی";
		LBLFontBold.TextAlign = (ContentAlignment)16;
		((Control)Label6).Anchor = (AnchorStyles)9;
		((Control)Label6).Location = new Point(552, 67);
		((Control)Label6).Name = "Label6";
		((Control)Label6).Size = new Size(126, 26);
		((Control)Label6).TabIndex = 14;
		((Control)Label6).Text = " قلم نوع تو پر(Bold) :";
		Label6.TextAlign = (ContentAlignment)64;
		((Control)LBLFontSize).Anchor = (AnchorStyles)9;
		((Control)LBLFontSize).ForeColor = Color.Blue;
		((Control)LBLFontSize).Location = new Point(420, 39);
		((Control)LBLFontSize).Name = "LBLFontSize";
		((Control)LBLFontSize).Size = new Size(126, 26);
		((Control)LBLFontSize).TabIndex = 13;
		((Control)LBLFontSize).Text = "FontSize";
		LBLFontSize.TextAlign = (ContentAlignment)16;
		((Control)Label4).Anchor = (AnchorStyles)9;
		((Control)Label4).Location = new Point(552, 39);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(126, 26);
		((Control)Label4).TabIndex = 12;
		((Control)Label4).Text = "سایز قلم انتخابی :";
		Label4.TextAlign = (ContentAlignment)64;
		((Control)LBLFontName).Anchor = (AnchorStyles)9;
		((Control)LBLFontName).ForeColor = Color.Blue;
		((Control)LBLFontName).Location = new Point(420, 13);
		((Control)LBLFontName).Name = "LBLFontName";
		((Control)LBLFontName).Size = new Size(126, 26);
		((Control)LBLFontName).TabIndex = 11;
		((Control)LBLFontName).Text = "FontName";
		LBLFontName.TextAlign = (ContentAlignment)16;
		((Control)Label2).Anchor = (AnchorStyles)9;
		((Control)Label2).Location = new Point(552, 13);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(126, 26);
		((Control)Label2).TabIndex = 10;
		((Control)Label2).Text = "نام قلم انتخاب شده :";
		Label2.TextAlign = (ContentAlignment)64;
		((Control)BTNSelectFont).AccessibleRole = (AccessibleRole)43;
		((Control)BTNSelectFont).Anchor = (AnchorStyles)6;
		BTNSelectFont.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNSelectFont).Location = new Point(6, 93);
		((Control)BTNSelectFont).Name = "BTNSelectFont";
		((Control)BTNSelectFont).Size = new Size(75, 23);
		BTNSelectFont.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNSelectFont).TabIndex = 2;
		((Control)BTNSelectFont).Text = "انتخاب قلم ";
		((Control)BTNSelectFont).Click += BTNSelectFont_Click;
		((Control)LBLPreviwe).Anchor = (AnchorStyles)15;
		((Control)LBLPreviwe).BackColor = Color.Transparent;
		((Control)LBLPreviwe).Location = new Point(10, 177);
		((Control)LBLPreviwe).Name = "LBLPreviwe";
		((Control)LBLPreviwe).Size = new Size(653, 201);
		((Control)LBLPreviwe).TabIndex = 2;
		((Control)GroupPanel9).Anchor = (AnchorStyles)13;
		((Control)GroupPanel9).BackColor = Color.Transparent;
		GroupPanel9.CanvasColor = SystemColors.Control;
		GroupPanel9.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		GroupPanel9.DisabledBackColor = Color.Empty;
		((Control)GroupPanel9).Location = new Point(7, 3);
		((Control)GroupPanel9).Name = "GroupPanel9";
		((Control)GroupPanel9).Size = new Size(674, 397);
		GroupPanel9.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel9.Style.BackColorGradientAngle = 90;
		GroupPanel9.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel9.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel9.Style.BorderBottomWidth = 1;
		GroupPanel9.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel9.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel9.Style.BorderLeftWidth = 1;
		GroupPanel9.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel9.Style.BorderRightWidth = 1;
		GroupPanel9.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel9.Style.BorderTopWidth = 1;
		GroupPanel9.Style.CornerDiameter = 4;
		GroupPanel9.Style.CornerType = eCornerType.Rounded;
		GroupPanel9.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel9.Style.TextColor = Color.Blue;
		GroupPanel9.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel9.Style.TextShadowColorSchemePart = eColorSchemePart.BarCaptionInactiveText;
		GroupPanel9.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel9.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel9).TabIndex = 5;
		TabItem6.AttachedControl = (Control)(object)TabControlPanel6;
		TabItem6.Name = "TabItem6";
		TabItem6.Text = "فونت";
		((FileDialog)OFD).FileName = "OpenFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.LightGray;
		((Form)this).ClientSize = new Size(694, 524);
		((Control)this).Controls.Add((Control)(object)TabControl1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmSetting";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "تنظیمات نرم افزار";
		((Form)this).TransparencyKey = Color.LightGray;
		((Form)this).Load += FrmSetting_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMSetting_KeyDown);
		((ISupportInitialize)TabControl1).EndInit();
		TabControl1.ResumeLayout(performLayout: false);
		((Control)TabControlPanel1).ResumeLayout(false);
		((Control)Gp_setting).ResumeLayout(false);
		((Control)Gb_Setting).ResumeLayout(false);
		((Control)GroupBox2).ResumeLayout(false);
		((ISupportInitialize)Min2).EndInit();
		((ISupportInitialize)Hour2).EndInit();
		((ISupportInitialize)Min1).EndInit();
		((ISupportInitialize)Hour1).EndInit();
		((Control)Panel1).ResumeLayout(false);
		((Control)GroupBox4).ResumeLayout(false);
		((ISupportInitialize)CountSent).EndInit();
		((Control)TabControlPanel7).ResumeLayout(false);
		((Control)GroupPanel10).ResumeLayout(false);
		((Control)GroupBoxDialUP).ResumeLayout(false);
		((ISupportInitialize)NumericUpDown1).EndInit();
		((Control)TabControlPanel3).ResumeLayout(false);
		((Control)TabControlPanel2).ResumeLayout(false);
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)Gb_Sound).ResumeLayout(false);
		((Control)Gp_Delivery).ResumeLayout(false);
		((Control)Gp_Call).ResumeLayout(false);
		((Control)Gp_Recieve).ResumeLayout(false);
		((Control)Gp_Send).ResumeLayout(false);
		((Control)TabControlPanel4).ResumeLayout(false);
		((Control)GroupPanel4).ResumeLayout(false);
		((Control)PNLDialup).ResumeLayout(false);
		((Control)PNLDialup).PerformLayout();
		((Control)TabControlPanel6).ResumeLayout(false);
		((Control)PanelEx3).ResumeLayout(false);
		((Control)GroupBox1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
