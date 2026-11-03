using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PayamakSettings;
using PayamakUi.Classes;
using PayamakUi.Classes.Entities;
using PayamakUi.Classes.GSM;
using PayamakUi.Forms.FRMOprate;
using PayamakUi.Properties;
using PayamakUi.UC.Templates;
using PayamakUi.Users;

namespace PayamakUi.UC.UCSMS;

public class UCSend : UserControl, IHasContext, INpDisposable, IDisposable
{
	public delegate void Exit_UserControlEventHandler();

	private PayamakClientHubInstances _instance;

	private List<DateTime> _Dates = new List<DateTime>();

	private SMSTemplate _ClsTemplate;

	private SmsOutBox _cls;

	public const string x = ".,-!?@~_\"\\/&';^|:(){}[]=€$£§%¥*+#¿¡¤¢«»®©°<> ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz01234567890";

	public List<string> _Number;

	private const string SplitCheractre = ";";

	private IContainer components;

	internal PanelEx PanelEx1;

	internal GroupPanel GroupPanel_Send;

	internal TabControl TabControl_Send;

	internal TabControlPanel TabControlPanel1;

	internal TabControlPanel TabControlPanel2;

	internal TabControl TabControl1;

	internal Label LabelX1;

	internal GroupPanel Gpl_Message;

	internal CheckBoxX CBScaduling;

	internal Label Lbl_Priroty;

	internal ComboBoxEx CmbPriority;

	internal ComboItem ComboItem9;

	internal ComboItem ComboItem8;

	internal ComboItem ComboItem7;

	internal ButtonX Btn_Right;

	internal ButtonX Btn_Left;

	internal GroupPanel Gpl_Tel;

	internal UCMobiles UcMobiles1;

	internal Panel PNLMobiles_List;

	internal ButtonX BtnNetSent;

	internal ButtonX Btn_Draft;

	internal ButtonX BTNSend;

	internal ButtonX BTNExit;

	internal UCSMS TXTMessage;

	internal UCTemplateList UcTemplateList1;

	internal Label Lblrightel;

	internal Label LabelX7;

	internal Label Lbl4Num;

	internal Label LabelX11;

	internal Label Lbl021;

	internal Label lblPanel;

	internal Label LabelX5;

	internal Label LabelX2;

	internal Label LBLNum_Tekrari;

	internal Label LabelX3;

	internal Label LBLNum_Incorrect;

	internal Label LabelX6;

	internal Label LBLNum_Correct;

	internal Label LabelX4;

	internal Label LBLNum_Total;

	internal Label LblNumberTel;

	internal Label LBLNum_HamraheAval;

	internal Label LabelX8;

	internal Label LBLNum_Irancell;

	internal Label LabelX10;

	private TextBox TxtMob;

	public string Message
	{
		get
		{
			try
			{
				return TXTMessage.Text.FixString().Trim();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Message", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 148);
				return "";
			}
		}
		set
		{
			try
			{
				TXTMessage.Text = value.FixString().Trim();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Message", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 161);
			}
		}
	}

	public string Mobiles
	{
		get
		{
			try
			{
				return ((Control)TxtMob).Text.Trim();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Mobiles", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 176);
				return "";
			}
		}
		set
		{
			try
			{
				((Control)TxtMob).Text = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Mobiles", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 189);
			}
		}
	}

	public int Number_Total
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_Total).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Total", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 344);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_Total).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Total", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 357);
			}
		}
	}

	public int Number_Currect
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_Correct).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Currect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 372);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_Correct).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Currect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 385);
			}
		}
	}

	public int Number_HamraheAval
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_HamraheAval).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_HamraheAval", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 400);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_HamraheAval).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_HamraheAval", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 413);
			}
		}
	}

	public int Number_InCorrect
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_Incorrect).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_InCorrect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 428);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_Incorrect).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_InCorrect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 441);
			}
		}
	}

	public int Number_IranCell
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_Irancell).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_IranCell", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 456);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_Irancell).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_IranCell", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 469);
			}
		}
	}

	public int Number_Panel
	{
		get
		{
			try
			{
				return int.Parse(((Control)lblPanel).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Panel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 484);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)lblPanel).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Panel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 497);
			}
		}
	}

	public int Number_Rightel
	{
		get
		{
			try
			{
				return int.Parse(((Control)Lblrightel).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Rightel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 512);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)Lblrightel).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Rightel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 525);
			}
		}
	}

	public int Number_021
	{
		get
		{
			try
			{
				return int.Parse(((Control)Lbl021).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_021", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 540);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)Lbl021).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_021", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 553);
			}
		}
	}

	public int Number_4Num
	{
		get
		{
			try
			{
				return int.Parse(((Control)Lbl4Num).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_4Num", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 568);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)Lbl4Num).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_4Num", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 581);
			}
		}
	}

	public int Number_Tekrari
	{
		get
		{
			try
			{
				return int.Parse(((Control)LBLNum_Tekrari).Text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Tekrari", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 596);
				return 0;
			}
		}
		set
		{
			try
			{
				((Control)LBLNum_Tekrari).Text = value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_Tekrari", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 609);
			}
		}
	}

	public List<object> List => UcMobiles1.List;

	public string ContextGuid { get; set; }

	public event Exit_UserControlEventHandler Exit_UserControl;

	public async Task<ReturnedSaveFuncInfo> LoadDataAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(UcTemplateList1.ContextGuid))
			{
				UcTemplateList1.ContextGuid = ContextGuid;
			}
			await UcTemplateList1.LoadDataAsync(token);
			if (_instance == null)
			{
				_instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 38);
		}
		return ret;
	}

	public UCSend()
	{
		InitializeComponent();
		UcTemplateList1.ContextGuid = ContextGuid;
		UcMobiles1.ContextGuid = ContextGuid;
		((Control)UcMobiles1).Visible = false;
		UcTemplateList1.UcParametr1.OnParameterSelected += UcParametr1_OnParameterSelected;
	}

	private async void Btn_Send2_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			((Control)BTNSend).Enabled = false;
			short num = (short)((((ListControl)CmbPriority).SelectedIndex >= 0) ? ((short)((ListControl)CmbPriority).SelectedIndex) : 0);
			string encoding = ((TXTMessage.Text.Trim() == "") ? "Unicode_16Bit" : ((!TXTMessage.IsLatin(TXTMessage.Text.Trim())) ? "Unicode_16Bit" : "ANSI_8Bit"));
			((Control)TxtMob).Text = ((Control)TxtMob).Text.RemoveNoNumbers(";");
			if (CBScaduling.Checked)
			{
				if (GetDates())
				{
					string message = TXTMessage.Text.Trim();
					List<string> number = _Number;
					short priority = num;
					await new GSM(message, number, priority, await CLSSMS.GetDeliveryEnableAsync(), encoding, _Dates).SaveAsync(ContextGuid);
				}
				else
				{
					string message = TXTMessage.Text.Trim();
					List<string> number = _Number;
					short priority = num;
					await new GSM(message, number, priority, await CLSSMS.GetDeliveryEnableAsync(), encoding).SaveAsync(ContextGuid);
				}
			}
			((Control)BTNSend).Enabled = true;
			BTNExit.PerformClick();
		}
		catch (Exception ex)
		{
			((Control)BTNSend).Enabled = true;
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Send2_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 106);
		}
	}

	private void UcParametr1_OnParameterSelected(object sender, string text)
	{
		try
		{
			TextBoxX txtContent = TXTMessage.TxtContent;
			((Control)txtContent).Text = ((Control)txtContent).Text + text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcParametr1_OnParameterSelected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 115);
		}
	}

	private void Btn_List_Click(object sender, EventArgs e)
	{
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		try
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				val.Close();
				((Control)this).Visible = false;
				((Control)this).Hide();
				this.Exit_UserControl?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 209);
		}
	}

	private void TXTMessage_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 113)
			{
				e.Handled = true;
				SendKeys.Send("[field2]");
			}
			else if ((int)keyCode == 114)
			{
				e.Handled = true;
				SendKeys.Send("[field3]");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMessage_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 236);
		}
	}

	public async Task ResetFormAsync()
	{
		_ = 1;
		try
		{
			((Control)PNLMobiles_List).Visible = true;
			((Control)PNLMobiles_List).Enabled = true;
			((Control)PNLMobiles_List).BringToFront();
			((Control)UcMobiles1).Visible = false;
			await UcMobiles1.SetListAsync(null);
			TXTMessage.Text = "";
			((Control)TxtMob).Text = "";
			((ListControl)CmbPriority).SelectedIndex = 1;
			if (string.IsNullOrEmpty(UcTemplateList1.ContextGuid))
			{
				UcTemplateList1.ContextGuid = ContextGuid;
			}
			if (_instance == null)
			{
				_instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			}
			await UcTemplateList1.LoadDataAsync(default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 266);
		}
	}

	private void TXTMob_GotFocus(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (sender is TextBox)
			{
				((TextBoxBase)(TextBox)sender).SelectAll();
			}
			if (sender is RichTextBox)
			{
				((TextBoxBase)(RichTextBox)sender).SelectAll();
			}
			((Control)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMob_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 282);
		}
	}

	private void TXTMob_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				_ = ((Control)TxtMob).Text.Trim() == "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMob_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 304);
		}
	}

	private void TXTMob_LostFocus(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMob_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 316);
		}
	}

	public void Send()
	{
		try
		{
			BTNSend.PerformClick();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 328);
		}
	}

	private async void Btn_Draft_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)Btn_Draft).Enabled = false;
			if ((int)((Form)new FRMTemplate(ContextGuid, new SMSTemplate
			{
				Content = TXTMessage.Text.FixString()
			})).ShowDialog((IWin32Window)(object)this) == 1)
			{
				CancellationToken token = new CancellationTokenSource(60000).Token;
				await UcTemplateList1.LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Draft_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 626);
		}
		finally
		{
			((Control)Btn_Draft).Enabled = true;
		}
	}

	private async void BtnNetSent_Click(object sender, EventArgs e)
	{
		bool flag = false;
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!((Control)BtnNetSent).Enabled || !((Control)BtnNetSent).Visible)
			{
				return;
			}
			line = 1;
			if (((Control)TXTMessage).Visible)
			{
				((Control)TXTMessage).Focus();
			}
			line = 2;
			((Control)BtnNetSent).Enabled = false;
			line = 3;
			FRMAccountMain frm = new FRMAccountMain(ContextGuid);
			line = 4;
			if ((int)((Form)frm).ShowDialog((IWin32Window)(object)this) != 1)
			{
				((Control)BtnNetSent).Enabled = true;
				return;
			}
			line = 6;
			((Control)TxtMob).Text = ((Control)TxtMob).Text.RemoveNoNumbers(";");
			line = 7;
			if (((Control)UcMobiles1).Visible)
			{
				line = 8;
				await new Net(frm.SelectedAcount.Username, frm.SelectedAcount.Password, frm.SelectedAcount.SenderNumber, frm.SelectedAcount.Domain).SendAsync(UcMobiles1.List, TXTMessage.Text, frm.SmsAccount.Id);
				BTNExit.PerformClick();
				return;
			}
			line = 9;
			bool Parametric = false;
			line = 10;
			string _msg = TXTMessage.Text;
			if (CBScaduling.Checked)
			{
				line = 11;
				if (!GetDates())
				{
					line = 25;
					((Control)BtnNetSent).Enabled = true;
					return;
				}
				line = 12;
				if (_msg.ToUpper().Contains("FIELD"))
				{
					line = 13;
					Parametric = true;
				}
				line = 14;
				foreach (string item4 in _Number)
				{
					line = 15;
					string text = _msg;
					line = 16;
					if (Parametric)
					{
						line = 17;
						text.Replace("[field", "[Field").Replace("[FIELD", "[Field");
						line = 18;
						throw new NotImplementedException();
					}
					line = 23;
					SmsOutBox item2 = new SmsOutBox(text, item4, frm.SelectedAcount.SenderNumber, 0, frm.SelectedAcount.Id);
					line = 24;
					await _instance.SmsOutBox.SaveAsync(item2);
					flag = true;
				}
			}
			else
			{
				line = 26;
				if (_msg.ToUpper().Contains("FIELD"))
				{
					line = 27;
					Parametric = true;
				}
				line = 280;
				if (_Number == null || !_Number.Any())
				{
					line = 281;
					ret.AddError(EnSystem.UserInterface, "شماره ای برای ارسال پیامک وجود ندارد!", "", "BtnNetSent_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 743);
					line = 282;
					return;
				}
				line = 283;
				foreach (string item in _Number)
				{
					line = 29;
					string message2 = _msg;
					line = 30;
					if (Parametric)
					{
						line = 31;
						message2 = message2.Replace("[field", "[Field").Replace("[FIELD", "[Field");
						line = 32;
						List<TafsilNumberBusiness> list = await TafsilNumberBusiness.GetInfoAsync(ContextGuid, item.ParseToLong(0L), true);
						if (list != null && list.Any())
						{
							TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, list.First().TafsilGuid, "", default(CancellationToken), "BtnNetSent_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs");
							if (tafsilBusiness != null)
							{
								message2 = message2.Replace("[Field2]", tafsilBusiness.Account.AccountFormated(shortFormat: false));
								message2 = message2.Replace("[Field3]", tafsilBusiness?.Name ?? "");
							}
						}
					}
					line = 37;
					SmsOutBox item3 = new SmsOutBox(message2, item, frm.SelectedAcount.SenderNumber, 0, frm.SelectedAcount.Id);
					line = 38;
					await _instance.SmsOutBox.SaveAsync(item3);
					flag = true;
				}
			}
			line = 39;
			((Control)BtnNetSent).Enabled = true;
			line = 40;
			if (flag)
			{
				MessageBox.Show("عملیات ارسال پیام به اتمام رسید");
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "BtnNetSent_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 801);
			WebErrorLog.Instance.StartLog(ex, $"Error in Line {line}", "BtnNetSent_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 802);
		}
		finally
		{
			((Control)BtnNetSent).Enabled = true;
			if (ret.HasError)
			{
				this.ShowReturnedSavedFuncInfoForUserControl(ret, "خطا در ارسال پیامک", "BtnNetSent_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs");
			}
			else
			{
				BTNExit.PerformClick();
			}
		}
	}

	private bool GetDates()
	{
		return false;
	}

	public async Task SetListAsync(List<object> value)
	{
		try
		{
			await UcMobiles1.SetListAsync(value);
			if (value == null)
			{
				((Control)UcMobiles1).Visible = false;
				((Control)PNLMobiles_List).BringToFront();
				((Control)PNLMobiles_List).Visible = true;
			}
			else
			{
				((Control)UcMobiles1).Visible = true;
				((Control)UcMobiles1).BringToFront();
				((Control)PNLMobiles_List).Visible = false;
				TXTMessage.Text = UcMobiles1.MSG_CheckAghsat;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 848);
		}
	}

	private void UcTemplateList1_OnSelct(string SelectedItem)
	{
		try
		{
			TXTMessage.Text = SelectedItem;
			((Control)TXTMessage).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcTemplateList1_OnSelct", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 860);
		}
	}

	private void TXTMob_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int number_Panel = 0;
			int number_ = 0;
			int number_4Num = 0;
			int num7 = 0;
			_Number = new List<string>();
			foreach (string item in ((Control)TxtMob).Text.SplitString())
			{
				if (item.Length < 4)
				{
					continue;
				}
				WebTafsilNumbers webTafsilNumbers = new WebTafsilNumbers(item);
				EnCellPhoneOperatorType enCellPhoneOperatorType = CLSSMS.CheckNumberOperator(webTafsilNumbers.CalcedNumber.ToString());
				num3++;
				if (webTafsilNumbers.IsValid && webTafsilNumbers.Mobile)
				{
					num++;
					switch (enCellPhoneOperatorType)
					{
					case EnCellPhoneOperatorType.IranCell:
						num4++;
						break;
					case EnCellPhoneOperatorType.HamraheAval:
						num5++;
						break;
					case EnCellPhoneOperatorType.Rightel:
						num7++;
						break;
					default:
						WebErrorLog.Instance.StartLog($"New OPerator Type({enCellPhoneOperatorType})", "", "TXTMob_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 901);
						break;
					case EnCellPhoneOperatorType.None:
						break;
					}
					if (_Number.Contains(webTafsilNumbers.CalcedNumber.ToString()))
					{
						num6++;
					}
					else
					{
						_Number.Add(webTafsilNumbers.CalcedNumber.ToString());
					}
				}
				else
				{
					num2++;
				}
			}
			Number_Total = num3;
			Number_Currect = num;
			Number_HamraheAval = num5;
			Number_InCorrect = num2;
			Number_IranCell = num4;
			Number_Tekrari = num6;
			Number_Panel = number_Panel;
			Number_021 = number_;
			Number_Rightel = num7;
			Number_4Num = number_4Num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMob_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSend.cs", 924);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_1d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5d: Expected O, but got Unknown
		//IL_1de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_202a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2034: Expected O, but got Unknown
		//IL_2160: Unknown result type (might be due to invalid IL or missing references)
		//IL_216a: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		Gpl_Message = new GroupPanel();
		CBScaduling = new CheckBoxX();
		Lbl_Priroty = new Label();
		CmbPriority = new ComboBoxEx();
		ComboItem9 = new ComboItem();
		ComboItem8 = new ComboItem();
		ComboItem7 = new ComboItem();
		Btn_Right = new ButtonX();
		Btn_Left = new ButtonX();
		Gpl_Tel = new GroupPanel();
		Lblrightel = new Label();
		LabelX7 = new Label();
		Lbl4Num = new Label();
		LabelX11 = new Label();
		Lbl021 = new Label();
		lblPanel = new Label();
		LabelX5 = new Label();
		LabelX2 = new Label();
		LBLNum_Tekrari = new Label();
		LblNumberTel = new Label();
		LabelX3 = new Label();
		LBLNum_Incorrect = new Label();
		LabelX6 = new Label();
		LBLNum_Correct = new Label();
		LabelX4 = new Label();
		LBLNum_Total = new Label();
		LBLNum_HamraheAval = new Label();
		LabelX8 = new Label();
		LBLNum_Irancell = new Label();
		LabelX10 = new Label();
		PNLMobiles_List = new Panel();
		TxtMob = new TextBox();
		BtnNetSent = new ButtonX();
		GroupPanel_Send = new GroupPanel();
		Btn_Draft = new ButtonX();
		BTNSend = new ButtonX();
		BTNExit = new ButtonX();
		LabelX1 = new Label();
		TabControl_Send = new TabControl();
		TabControlPanel1 = new TabControlPanel();
		TabControlPanel2 = new TabControlPanel();
		TabControl1 = new TabControl();
		UcTemplateList1 = new UCTemplateList();
		TXTMessage = new UCSMS();
		UcMobiles1 = new UCMobiles();
		((Control)PanelEx1).SuspendLayout();
		((Control)Gpl_Message).SuspendLayout();
		((Control)Gpl_Tel).SuspendLayout();
		((Control)PNLMobiles_List).SuspendLayout();
		((ISupportInitialize)TabControl_Send).BeginInit();
		((Control)TabControl_Send).SuspendLayout();
		((ISupportInitialize)TabControl1).BeginInit();
		((Control)TabControl1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)PanelEx1).Anchor = (AnchorStyles)15;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)UcTemplateList1);
		((Control)PanelEx1).Controls.Add((Control)(object)Gpl_Message);
		((Control)PanelEx1).Controls.Add((Control)(object)Gpl_Tel);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnNetSent);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel_Send);
		((Control)PanelEx1).Controls.Add((Control)(object)Btn_Draft);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNSend);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Location = new Point(-1, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(748, 496);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 3;
		((Control)Gpl_Message).Anchor = (AnchorStyles)15;
		Gpl_Message.CanvasColor = SystemColors.Control;
		Gpl_Message.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gpl_Message).Controls.Add((Control)(object)CBScaduling);
		((Control)Gpl_Message).Controls.Add((Control)(object)Lbl_Priroty);
		((Control)Gpl_Message).Controls.Add((Control)(object)CmbPriority);
		((Control)Gpl_Message).Controls.Add((Control)(object)Btn_Right);
		((Control)Gpl_Message).Controls.Add((Control)(object)Btn_Left);
		((Control)Gpl_Message).Controls.Add((Control)(object)TXTMessage);
		Gpl_Message.DisabledBackColor = Color.Empty;
		((Control)Gpl_Message).Location = new Point(5, 37);
		((Control)Gpl_Message).Name = "Gpl_Message";
		((Control)Gpl_Message).Size = new Size(310, 415);
		Gpl_Message.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gpl_Message.Style.BackColorGradientAngle = 90;
		Gpl_Message.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gpl_Message.Style.BorderBottom = eStyleBorderType.Solid;
		Gpl_Message.Style.BorderBottomWidth = 1;
		Gpl_Message.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gpl_Message.Style.BorderLeft = eStyleBorderType.Solid;
		Gpl_Message.Style.BorderLeftWidth = 1;
		Gpl_Message.Style.BorderRight = eStyleBorderType.Solid;
		Gpl_Message.Style.BorderRightWidth = 1;
		Gpl_Message.Style.BorderTop = eStyleBorderType.Solid;
		Gpl_Message.Style.BorderTopWidth = 1;
		Gpl_Message.Style.CornerDiameter = 4;
		Gpl_Message.Style.CornerType = eCornerType.Rounded;
		Gpl_Message.Style.TextAlignment = eStyleTextAlignment.Center;
		Gpl_Message.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gpl_Message.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gpl_Message.StyleMouseDown.CornerType = eCornerType.Square;
		Gpl_Message.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gpl_Message).TabIndex = 36;
		((Control)Gpl_Message).Text = "متن پیام";
		((Control)CBScaduling).Anchor = (AnchorStyles)9;
		CBScaduling.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)CBScaduling).Location = new Point(168, 35);
		((Control)CBScaduling).Name = "CBScaduling";
		((Control)CBScaduling).RightToLeft = (RightToLeft)1;
		((Control)CBScaduling).Size = new Size(133, 22);
		CBScaduling.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)CBScaduling).TabIndex = 31;
		((Control)CBScaduling).TabStop = false;
		((Control)CBScaduling).Text = "ارسال زمانبندی شده";
		((Control)Lbl_Priroty).Anchor = (AnchorStyles)9;
		((Control)Lbl_Priroty).Location = new Point(223, 6);
		((Control)Lbl_Priroty).Name = "Lbl_Priroty";
		((Control)Lbl_Priroty).Size = new Size(78, 20);
		((Control)Lbl_Priroty).TabIndex = 21;
		((Control)Lbl_Priroty).Text = "اولویت ارسال :";
		((Control)CmbPriority).Anchor = (AnchorStyles)9;
		((ListControl)CmbPriority).DisplayMember = "Text";
		((ComboBox)CmbPriority).DrawMode = (DrawMode)1;
		((ComboBox)CmbPriority).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbPriority).FormattingEnabled = true;
		((ComboBox)CmbPriority).ItemHeight = 15;
		CmbPriority.Items.AddRange(new object[3] { ComboItem9, ComboItem8, ComboItem7 });
		((Control)CmbPriority).Location = new Point(154, 5);
		((Control)CmbPriority).Name = "CmbPriority";
		((Control)CmbPriority).Size = new Size(63, 21);
		((Control)CmbPriority).TabIndex = 20;
		((Control)CmbPriority).TabStop = false;
		ComboItem9.FontStyle = (FontStyle)1;
		ComboItem9.Text = "فوری";
		ComboItem8.FontStyle = (FontStyle)1;
		ComboItem8.Text = "عادی";
		ComboItem7.FontStyle = (FontStyle)1;
		ComboItem7.Text = "کم";
		((Control)Btn_Right).AccessibleRole = (AccessibleRole)43;
		((Control)Btn_Right).Anchor = (AnchorStyles)6;
		Btn_Right.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Right).Enabled = false;
		((Control)Btn_Right).Location = new Point(40, -91);
		((Control)Btn_Right).Name = "Btn_Right";
		((Control)Btn_Right).Size = new Size(28, 20);
		((Control)Btn_Right).TabIndex = 10;
		Btn_Right.Tooltip = "راست چین";
		((Control)Btn_Right).Visible = false;
		((Control)Btn_Left).AccessibleRole = (AccessibleRole)43;
		((Control)Btn_Left).Anchor = (AnchorStyles)6;
		Btn_Left.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Left).Enabled = false;
		((Control)Btn_Left).Location = new Point(6, -91);
		((Control)Btn_Left).Name = "Btn_Left";
		((Control)Btn_Left).Size = new Size(28, 20);
		((Control)Btn_Left).TabIndex = 9;
		Btn_Left.Tooltip = "چپ چین";
		((Control)Btn_Left).Visible = false;
		((Control)Gpl_Tel).Anchor = (AnchorStyles)11;
		Gpl_Tel.CanvasColor = SystemColors.Control;
		Gpl_Tel.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)Gpl_Tel).Controls.Add((Control)(object)Lblrightel);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX7);
		((Control)Gpl_Tel).Controls.Add((Control)(object)Lbl4Num);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX11);
		((Control)Gpl_Tel).Controls.Add((Control)(object)Lbl021);
		((Control)Gpl_Tel).Controls.Add((Control)(object)lblPanel);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX5);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX2);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_Tekrari);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LblNumberTel);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX3);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_Incorrect);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX6);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_Correct);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX4);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_Total);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_HamraheAval);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX8);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LBLNum_Irancell);
		((Control)Gpl_Tel).Controls.Add((Control)(object)LabelX10);
		((Control)Gpl_Tel).Controls.Add((Control)(object)PNLMobiles_List);
		((Control)Gpl_Tel).Controls.Add((Control)(object)UcMobiles1);
		Gpl_Tel.DisabledBackColor = Color.Empty;
		((Control)Gpl_Tel).Location = new Point(321, 37);
		((Control)Gpl_Tel).Name = "Gpl_Tel";
		((Control)Gpl_Tel).Size = new Size(169, 415);
		Gpl_Tel.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Gpl_Tel.Style.BackColorGradientAngle = 90;
		Gpl_Tel.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Gpl_Tel.Style.BorderBottom = eStyleBorderType.Solid;
		Gpl_Tel.Style.BorderBottomWidth = 1;
		Gpl_Tel.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Gpl_Tel.Style.BorderLeft = eStyleBorderType.Solid;
		Gpl_Tel.Style.BorderLeftWidth = 1;
		Gpl_Tel.Style.BorderRight = eStyleBorderType.Solid;
		Gpl_Tel.Style.BorderRightWidth = 1;
		Gpl_Tel.Style.BorderTop = eStyleBorderType.Solid;
		Gpl_Tel.Style.BorderTopWidth = 1;
		Gpl_Tel.Style.CornerDiameter = 4;
		Gpl_Tel.Style.CornerType = eCornerType.Rounded;
		Gpl_Tel.Style.TextAlignment = eStyleTextAlignment.Center;
		Gpl_Tel.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Gpl_Tel.Style.TextLineAlignment = eStyleTextAlignment.Near;
		Gpl_Tel.StyleMouseDown.CornerType = eCornerType.Square;
		Gpl_Tel.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)Gpl_Tel).TabIndex = 35;
		((Control)Gpl_Tel).Text = "شماره تلفن";
		((Control)Lblrightel).Anchor = (AnchorStyles)10;
		((Control)Lblrightel).ForeColor = Color.DarkRed;
		((Control)Lblrightel).Location = new Point(96, 369);
		((Control)Lblrightel).Name = "Lblrightel";
		((Control)Lblrightel).RightToLeft = (RightToLeft)0;
		((Control)Lblrightel).Size = new Size(19, 17);
		((Control)Lblrightel).TabIndex = 44;
		((Control)Lblrightel).Text = "0";
		((Control)LabelX7).Anchor = (AnchorStyles)10;
		((Control)LabelX7).Location = new Point(111, 365);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).Size = new Size(39, 17);
		((Control)LabelX7).TabIndex = 43;
		((Control)LabelX7).Text = "رایتل :";
		LabelX7.TextAlign = (ContentAlignment)16;
		((Control)Lbl4Num).Anchor = (AnchorStyles)10;
		((Control)Lbl4Num).ForeColor = Color.DarkRed;
		((Control)Lbl4Num).Location = new Point(9, 368);
		((Control)Lbl4Num).Name = "Lbl4Num";
		((Control)Lbl4Num).RightToLeft = (RightToLeft)0;
		((Control)Lbl4Num).Size = new Size(18, 17);
		((Control)Lbl4Num).TabIndex = 42;
		((Control)Lbl4Num).Text = "0";
		((Control)LabelX11).Anchor = (AnchorStyles)10;
		((Control)LabelX11).Location = new Point(28, 354);
		((Control)LabelX11).Name = "LabelX11";
		((Control)LabelX11).Size = new Size(57, 28);
		((Control)LabelX11).TabIndex = 41;
		((Control)LabelX11).Text = "خطوط\r\n 4 رقمی :";
		LabelX11.TextAlign = (ContentAlignment)32;
		((Control)Lbl021).Anchor = (AnchorStyles)10;
		((Control)Lbl021).ForeColor = Color.DarkRed;
		((Control)Lbl021).Location = new Point(9, 340);
		((Control)Lbl021).Name = "Lbl021";
		((Control)Lbl021).RightToLeft = (RightToLeft)0;
		((Control)Lbl021).Size = new Size(18, 17);
		((Control)Lbl021).TabIndex = 40;
		((Control)Lbl021).Text = "0";
		((Control)lblPanel).Anchor = (AnchorStyles)10;
		((Control)lblPanel).ForeColor = Color.DarkRed;
		((Control)lblPanel).Location = new Point(9, 304);
		((Control)lblPanel).Name = "lblPanel";
		((Control)lblPanel).RightToLeft = (RightToLeft)0;
		((Control)lblPanel).Size = new Size(13, 17);
		((Control)lblPanel).TabIndex = 39;
		((Control)lblPanel).Text = "0";
		((Control)LabelX5).Anchor = (AnchorStyles)10;
		((Control)LabelX5).Location = new Point(28, 327);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).Size = new Size(63, 27);
		((Control)LabelX5).TabIndex = 38;
		((Control)LabelX5).Text = "خطوط\r\n ثابت 021 :";
		LabelX5.TextAlign = (ContentAlignment)32;
		((Control)LabelX2).Anchor = (AnchorStyles)10;
		((Control)LabelX2).Location = new Point(28, 287);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(58, 34);
		((Control)LabelX2).TabIndex = 37;
		((Control)LabelX2).Text = "سامانه \r\nپیامکی :";
		LabelX2.TextAlign = (ContentAlignment)32;
		((Control)LBLNum_Tekrari).Anchor = (AnchorStyles)10;
		((Control)LBLNum_Tekrari).ForeColor = Color.DarkRed;
		((Control)LBLNum_Tekrari).Location = new Point(96, 322);
		((Control)LBLNum_Tekrari).Name = "LBLNum_Tekrari";
		((Control)LBLNum_Tekrari).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_Tekrari).Size = new Size(18, 17);
		((Control)LBLNum_Tekrari).TabIndex = 34;
		((Control)LBLNum_Tekrari).Text = "0";
		((Control)LblNumberTel).Anchor = (AnchorStyles)10;
		((Control)LblNumberTel).Location = new Point(110, 265);
		((Control)LblNumberTel).Name = "LblNumberTel";
		((Control)LblNumberTel).Size = new Size(29, 17);
		((Control)LblNumberTel).TabIndex = 1;
		((Control)LblNumberTel).Text = "کل:";
		LblNumberTel.TextAlign = (ContentAlignment)16;
		((Control)LabelX3).Anchor = (AnchorStyles)10;
		((Control)LabelX3).Location = new Point(110, 320);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).Size = new Size(51, 17);
		((Control)LabelX3).TabIndex = 33;
		((Control)LabelX3).Text = "تکراری :";
		LabelX3.TextAlign = (ContentAlignment)16;
		((Control)LBLNum_Incorrect).Anchor = (AnchorStyles)10;
		((Control)LBLNum_Incorrect).ForeColor = Color.DarkRed;
		((Control)LBLNum_Incorrect).Location = new Point(96, 301);
		((Control)LBLNum_Incorrect).Name = "LBLNum_Incorrect";
		((Control)LBLNum_Incorrect).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_Incorrect).Size = new Size(16, 17);
		((Control)LBLNum_Incorrect).TabIndex = 28;
		((Control)LBLNum_Incorrect).Text = "0";
		((Control)LabelX6).Anchor = (AnchorStyles)10;
		((Control)LabelX6).Location = new Point(111, 302);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).Size = new Size(47, 17);
		((Control)LabelX6).TabIndex = 27;
		((Control)LabelX6).Text = "اشتباه :";
		LabelX6.TextAlign = (ContentAlignment)16;
		((Control)LBLNum_Correct).Anchor = (AnchorStyles)10;
		((Control)LBLNum_Correct).ForeColor = Color.DarkRed;
		((Control)LBLNum_Correct).Location = new Point(96, 282);
		((Control)LBLNum_Correct).Name = "LBLNum_Correct";
		((Control)LBLNum_Correct).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_Correct).Size = new Size(16, 17);
		((Control)LBLNum_Correct).TabIndex = 26;
		((Control)LBLNum_Correct).Text = "0";
		((Control)LabelX4).Anchor = (AnchorStyles)10;
		((Control)LabelX4).Location = new Point(105, 282);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(50, 17);
		((Control)LabelX4).TabIndex = 25;
		((Control)LabelX4).Text = "صحیح :";
		LabelX4.TextAlign = (ContentAlignment)16;
		((Control)LBLNum_Total).Anchor = (AnchorStyles)10;
		((Control)LBLNum_Total).ForeColor = Color.DarkRed;
		((Control)LBLNum_Total).Location = new Point(96, 265);
		((Control)LBLNum_Total).Name = "LBLNum_Total";
		((Control)LBLNum_Total).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_Total).Size = new Size(19, 17);
		((Control)LBLNum_Total).TabIndex = 2;
		((Control)LBLNum_Total).Text = "0";
		((Control)LBLNum_HamraheAval).Anchor = (AnchorStyles)10;
		((Control)LBLNum_HamraheAval).ForeColor = Color.DarkRed;
		((Control)LBLNum_HamraheAval).Location = new Point(9, 267);
		((Control)LBLNum_HamraheAval).Name = "LBLNum_HamraheAval";
		((Control)LBLNum_HamraheAval).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_HamraheAval).Size = new Size(13, 17);
		((Control)LBLNum_HamraheAval).TabIndex = 32;
		((Control)LBLNum_HamraheAval).Text = "0";
		((Control)LabelX8).Anchor = (AnchorStyles)10;
		((Control)LabelX8).Location = new Point(28, 265);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).Size = new Size(62, 17);
		((Control)LabelX8).TabIndex = 31;
		((Control)LabelX8).Text = "همراه اول :";
		LabelX8.TextAlign = (ContentAlignment)16;
		((Control)LBLNum_Irancell).Anchor = (AnchorStyles)10;
		((Control)LBLNum_Irancell).ForeColor = Color.DarkRed;
		((Control)LBLNum_Irancell).Location = new Point(96, 344);
		((Control)LBLNum_Irancell).Name = "LBLNum_Irancell";
		((Control)LBLNum_Irancell).RightToLeft = (RightToLeft)0;
		((Control)LBLNum_Irancell).Size = new Size(18, 17);
		((Control)LBLNum_Irancell).TabIndex = 30;
		((Control)LBLNum_Irancell).Text = "0";
		((Control)LabelX10).Anchor = (AnchorStyles)10;
		((Control)LabelX10).Location = new Point(111, 340);
		((Control)LabelX10).Name = "LabelX10";
		((Control)LabelX10).Size = new Size(52, 17);
		((Control)LabelX10).TabIndex = 29;
		((Control)LabelX10).Text = "ایرانسل :";
		LabelX10.TextAlign = (ContentAlignment)16;
		((Control)PNLMobiles_List).Anchor = (AnchorStyles)11;
		((Control)PNLMobiles_List).Controls.Add((Control)(object)TxtMob);
		((Control)PNLMobiles_List).Location = new Point(2, 3);
		((Control)PNLMobiles_List).Name = "PNLMobiles_List";
		((Control)PNLMobiles_List).Size = new Size(158, 257);
		((Control)PNLMobiles_List).TabIndex = 36;
		((Control)TxtMob).Anchor = (AnchorStyles)11;
		((Control)TxtMob).Location = new Point(0, 0);
		((TextBoxBase)TxtMob).Multiline = true;
		((Control)TxtMob).Name = "TxtMob";
		TxtMob.ScrollBars = (ScrollBars)2;
		((Control)TxtMob).Size = new Size(158, 257);
		((Control)TxtMob).TabIndex = 25;
		((Control)TxtMob).TextChanged += TXTMob_TextChanged;
		((Control)BtnNetSent).AccessibleRole = (AccessibleRole)43;
		((Control)BtnNetSent).Anchor = (AnchorStyles)6;
		BtnNetSent.ColorTable = eButtonColor.Office2007WithBackground;
		BtnNetSent.Image = (Image)(object)Resources.send6;
		((Control)BtnNetSent).Location = new Point(91, 458);
		((Control)BtnNetSent).Name = "BtnNetSent";
		((Control)BtnNetSent).Size = new Size(39, 30);
		BtnNetSent.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnNetSent).TabIndex = 34;
		BtnNetSent.Tooltip = "ارسال از طریق پنل اینترنتی";
		((Control)BtnNetSent).Click += BtnNetSent_Click;
		((Control)GroupPanel_Send).Anchor = (AnchorStyles)13;
		GroupPanel_Send.CanvasColor = SystemColors.Control;
		GroupPanel_Send.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		GroupPanel_Send.DisabledBackColor = Color.Empty;
		((Control)GroupPanel_Send).Location = new Point(0, 2);
		((Control)GroupPanel_Send).Name = "GroupPanel_Send";
		((Control)GroupPanel_Send).RightToLeft = (RightToLeft)1;
		((Control)GroupPanel_Send).Size = new Size(748, 40);
		GroupPanel_Send.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel_Send.Style.BackColorGradientAngle = 90;
		GroupPanel_Send.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel_Send.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel_Send.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel_Send.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel_Send.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel_Send.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel_Send.Style.CornerDiameter = 4;
		GroupPanel_Send.Style.CornerType = eCornerType.Rounded;
		GroupPanel_Send.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel_Send.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel_Send.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel_Send.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel_Send.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel_Send).TabIndex = 0;
		((Control)GroupPanel_Send).Text = "ارسال پیام";
		((Control)Btn_Draft).AccessibleRole = (AccessibleRole)43;
		((Control)Btn_Draft).Anchor = (AnchorStyles)6;
		Btn_Draft.ColorTable = eButtonColor.Office2007WithBackground;
		Btn_Draft.Image = (Image)(object)Resources.accept_page11;
		((Control)Btn_Draft).Location = new Point(136, 458);
		((Control)Btn_Draft).Name = "Btn_Draft";
		((Control)Btn_Draft).Size = new Size(39, 30);
		((Control)Btn_Draft).TabIndex = 26;
		((Control)Btn_Draft).TabStop = false;
		Btn_Draft.Tooltip = "ذخیره در پیش نویس ها";
		((Control)Btn_Draft).Click += Btn_Draft_Click;
		((Control)BTNSend).AccessibleRole = (AccessibleRole)43;
		((Control)BTNSend).Anchor = (AnchorStyles)6;
		BTNSend.ColorTable = eButtonColor.Office2007WithBackground;
		BTNSend.Image = (Image)(object)Resources.send51;
		((Control)BTNSend).Location = new Point(48, 458);
		((Control)BTNSend).Name = "BTNSend";
		((Control)BTNSend).Size = new Size(39, 30);
		BTNSend.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNSend).TabIndex = 1;
		BTNSend.Tooltip = "ارسال از طریق سیم کارت";
		((Control)BTNSend).Click += Btn_Send2_Click;
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTNExit.Image = (Image)(object)Resources._123;
		((Control)BTNExit).Location = new Point(5, 458);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(39, 30);
		((Control)BTNExit).TabIndex = 33;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "خروج";
		((Control)BTNExit).Click += BTNExit_Click;
		((Control)LabelX1).Anchor = (AnchorStyles)10;
		((Control)LabelX1).ForeColor = Color.DarkRed;
		((Control)LabelX1).Location = new Point(489, 467);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).RightToLeft = (RightToLeft)0;
		((Control)LabelX1).Size = new Size(256, 21);
		((Control)LabelX1).TabIndex = 24;
		((Control)LabelX1).Text = "F2  : مانده حساب      F3  : نام کامل طرف حساب";
		TabControl_Send.CanReorderTabs = true;
		((Control)TabControl_Send).Controls.Add((Control)(object)TabControlPanel1);
		((Control)TabControl_Send).Location = new Point(0, 0);
		((Control)TabControl_Send).Name = "TabControl_Send";
		TabControl_Send.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		TabControl_Send.SelectedTabIndex = -1;
		((Control)TabControl_Send).Size = new Size(256, 152);
		((Control)TabControl_Send).TabIndex = 0;
		TabControl_Send.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		TabControlPanel1.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel1).Dock = (DockStyle)5;
		((Control)TabControlPanel1).Location = new Point(0, 26);
		((Control)TabControlPanel1).Name = "TabControlPanel1";
		((Control)TabControlPanel1).Padding = new Padding(1);
		((Control)TabControlPanel1).Size = new Size(256, 126);
		TabControlPanel1.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel1.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel1.Style.Border = eBorderType.SingleLine;
		TabControlPanel1.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel1.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel1.Style.GradientAngle = 90;
		((Control)TabControlPanel1).TabIndex = 1;
		TabControlPanel2.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel2).Dock = (DockStyle)5;
		((Control)TabControlPanel2).Location = new Point(0, 26);
		((Control)TabControlPanel2).Name = "TabControlPanel2";
		((Control)TabControlPanel2).Padding = new Padding(1);
		((Control)TabControlPanel2).Size = new Size(256, 126);
		TabControlPanel2.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel2.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel2.Style.Border = eBorderType.SingleLine;
		TabControlPanel2.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel2.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel2.Style.GradientAngle = 90;
		((Control)TabControlPanel2).TabIndex = 1;
		TabControl1.CanReorderTabs = true;
		((Control)TabControl1).Controls.Add((Control)(object)TabControlPanel2);
		((Control)TabControl1).Location = new Point(0, 0);
		((Control)TabControl1).Name = "TabControl1";
		TabControl1.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		TabControl1.SelectedTabIndex = -1;
		((Control)TabControl1).Size = new Size(256, 152);
		((Control)TabControl1).TabIndex = 0;
		TabControl1.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		((Control)UcTemplateList1).Anchor = (AnchorStyles)11;
		UcTemplateList1.ContextGuid = null;
		((Control)UcTemplateList1).Location = new Point(496, 37);
		((Control)UcTemplateList1).Name = "UcTemplateList1";
		((Control)UcTemplateList1).Size = new Size(249, 415);
		((Control)UcTemplateList1).TabIndex = 37;
		UcTemplateList1.OnSelect += UcTemplateList1_OnSelct;
		((Control)TXTMessage).Anchor = (AnchorStyles)15;
		TXTMessage.ContextGuid = null;
		((Control)TXTMessage).Location = new Point(2, 63);
		((Control)TXTMessage).Name = "TXTMessage";
		((Control)TXTMessage).Size = new Size(296, 328);
		((Control)TXTMessage).TabIndex = 33;
		((Control)TXTMessage).KeyDown += new KeyEventHandler(TXTMessage_KeyDown);
		((Control)UcMobiles1).Anchor = (AnchorStyles)11;
		UcMobiles1.ContextGuid = null;
		((Control)UcMobiles1).Location = new Point(2, 3);
		UcMobiles1.MSG_CheckAghsat = "";
		((TextBoxBase)UcMobiles1).Multiline = true;
		((Control)UcMobiles1).Name = "UcMobiles1";
		((TextBoxBase)UcMobiles1).ReadOnly = true;
		((Control)UcMobiles1).Size = new Size(158, 176);
		((Control)UcMobiles1).TabIndex = 35;
		((Control)UcMobiles1).Text = "123";
		((Control)UcMobiles1).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "UCSend";
		((Control)this).Size = new Size(748, 496);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)Gpl_Message).ResumeLayout(false);
		((Control)Gpl_Tel).ResumeLayout(false);
		((Control)Gpl_Tel).PerformLayout();
		((Control)PNLMobiles_List).ResumeLayout(false);
		((Control)PNLMobiles_List).PerformLayout();
		((ISupportInitialize)TabControl_Send).EndInit();
		TabControl_Send.ResumeLayout(performLayout: false);
		((ISupportInitialize)TabControl1).EndInit();
		TabControl1.ResumeLayout(performLayout: false);
		((Control)this).ResumeLayout(false);
	}

	bool INpDisposable.get_IsDisposed()
	{
		return ((Control)this).IsDisposed;
	}
}
