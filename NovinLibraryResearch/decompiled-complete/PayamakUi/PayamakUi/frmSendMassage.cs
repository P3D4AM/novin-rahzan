using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi;

public class frmSendMassage : CacheBaseForm
{
	private List<BaseSmsNetAccount> senderList = new List<BaseSmsNetAccount>();

	private List<SmsOutBox> sendCount = new List<SmsOutBox>();

	private List<SmsOutBox> notSendCount = new List<SmsOutBox>();

	private PayamakClientHubInstances _instance;

	private IContainer components;

	private Label label1;

	private DataGridView dgvSender;

	private BindingSource SenderBindingSource;

	private DataGridViewCheckBoxColumn isCheckedDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;

	private Label label2;

	private TextBox txtResiver;

	private Label lblTextMessage;

	private TextBox txtMessage;

	private Label label4;

	private Label label5;

	private DateBoxFa txtSendDate;

	private DateBoxFa txtExpDate;

	private Button btnSend;

	private Button btnCancel;

	private NumericUpDown numSendMin;

	private NumericUpDown numSendHour;

	private DataGridViewCheckBoxColumn dgIsChecked;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn gsmCrecitDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn gsmUpdateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn gsmMaxSentDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn accountDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn domainDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullDomainDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn stDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;

	public SMSNet_Account CurrentItem { get; private set; }

	public frmSendMassage(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 34);
		}
	}

	private async void frmSendMassage_Load(object sender, EventArgs e)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>> returnedSaveFuncInfoWithValue = await _instance.SmsNetAccount.GetAllAsync(default(CancellationToken));
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				SenderBindingSource.DataSource = returnedSaveFuncInfoWithValue.value;
				senderList = returnedSaveFuncInfoWithValue.value;
			}
			numSendMin.Value = DateTime.Now.Minute;
			numSendHour.Value = DateTime.Now.Hour;
			txtSendDate.MiladiDate = DateTime.Now.Date;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmSendMassage_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 51);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void SenderBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			if (SenderBindingSource.Current is BaseSmsNetAccount baseSmsNetAccount)
			{
				CurrentItem = new SMSNet_Account
				{
					Id = baseSmsNetAccount.Id,
					Type = baseSmsNetAccount.Type,
					GsmCrecit = baseSmsNetAccount.GsmCrecit,
					GsmUpdate = baseSmsNetAccount.GsmUpdate,
					GsmMaxSent = baseSmsNetAccount.GsmMaxSent,
					Username = baseSmsNetAccount.Username,
					Password = baseSmsNetAccount.Password,
					FullDomain = baseSmsNetAccount.FullDomain,
					St = baseSmsNetAccount.St,
					Title = baseSmsNetAccount.Title,
					Domain = baseSmsNetAccount.Username,
					SenderNumber = baseSmsNetAccount.SenderNumber,
					Description = baseSmsNetAccount.Description
				};
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SenderBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 77);
		}
	}

	private void dgvSender_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (dgvSender.CurrentRow != null)
			{
				((DataGridViewBand)dgvSender.CurrentRow).Selected = true;
				CurrentItem.IsChecked = !CurrentItem.IsChecked;
				((DataGridViewBand)dgvSender.CurrentRow).DefaultCellStyle.BackColor = (((BaseSmsNetAccount)SenderBindingSource.Current).IsChecked ? Color.LightBlue : Color.White);
				((Control)dgvSender).Refresh();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "dgvSender_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 92);
		}
	}

	private async void btnSend_Click(object sender, EventArgs e)
	{
		Waiter _waiter = null;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		int invalidNum = 0;
		try
		{
			try
			{
				if (!(senderList?.Any((BaseSmsNetAccount q) => q.IsChecked) ?? false))
				{
					res.AddError("حداقل یک فرستنده انتخاب کنید", "", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 101);
				}
				if (string.IsNullOrEmpty(((Control)txtResiver).Text))
				{
					res.AddError("شماره موبایل گیرنده را وارد کنید", "Error", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 102);
				}
				if (string.IsNullOrEmpty(((Control)txtMessage).Text))
				{
					res.AddError("متن پیامک را وارد کنید", "Error", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 103);
				}
				if (res.HasError)
				{
					goto end_IL_0049;
				}
				_waiter = new Waiter("درحال ارسال پیامک ...", (Form)(object)this, null, freezeForm: true, EnWaiterType.Circle, null, "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs");
				List<WebTafsilNumbers> reciverList = new List<WebTafsilNumbers>();
				List<string> listNumbers = (from q in ((Control)txtResiver).Text.SplitString("\r\n")
					where !string.IsNullOrEmpty(q)
					select q).ToList();
				foreach (string item in listNumbers)
				{
					reciverList.Add(new WebTafsilNumbers(item));
				}
				reciverList = reciverList.Where((WebTafsilNumbers q) => q.CalcedNumber > 0 && q.Mobile).ToList();
				senderList = senderList?.Where((BaseSmsNetAccount q) => q.IsChecked)?.ToList();
				foreach (SMSNet_Account senderItem in senderList)
				{
					foreach (WebTafsilNumbers item2 in reciverList)
					{
						SmsOutBox smsout = new SmsOutBox("", "", 0)
						{
							SenderNumber = senderItem.SenderNumber,
							Number = item2.CalcedNumber.ToString(),
							Message = ((Control)txtMessage).Text,
							DateM = txtExpDate.MiladiDate,
							ST = true,
							AccountId = senderItem.Id
						};
						if (!(await _instance.SmsOutBox.SaveAsync(smsout)).HasError)
						{
							sendCount.Add(smsout);
						}
						else
						{
							notSendCount.Add(smsout);
						}
					}
				}
				invalidNum = listNumbers.Count - reciverList.Count;
				goto end_IL_0040;
				end_IL_0049:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 136);
				res.AddReturnedValue(ex, "", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 137);
				goto end_IL_0040;
			}
			end_IL_0040:;
		}
		finally
		{
			_waiter?.Dispose();
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs");
			}
			else
			{
				string text = "";
				if (invalidNum != 0)
				{
					text += $"تعداد {invalidNum} شماره گیرنده نامعتبر یافت شد. \r\n";
				}
				if (sendCount.Count != 0)
				{
					text += $"تعداد {sendCount.Count} ارسال با موفقیت انجام شد. \r\n";
				}
				if (notSendCount.Count != 0)
				{
					text += $"تعداد {notSendCount.Count} ارسال با شکست مواجه شد.";
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.ShowMessage(text, "پیغام سیستم", "btnSend_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs");
				}
			}
		}
	}

	private void txtMessage_TextChanged_1(object sender, EventArgs e)
	{
		try
		{
			((Control)lblTextMessage).Text = $"متن پیام ({((Control)txtMessage).Text.Length})";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMessage_TextChanged_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 159);
		}
	}

	private void frmSendMassage_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnSend.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmSendMassage_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\frmSendMassage.cs", 172);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Expected O, but got Unknown
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Expected O, but got Unknown
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b66: Expected O, but got Unknown
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Expected O, but got Unknown
		//IL_0c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Expected O, but got Unknown
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Expected O, but got Unknown
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa1: Expected O, but got Unknown
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1134: Expected O, but got Unknown
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Expected O, but got Unknown
		//IL_1199: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Expected O, but got Unknown
		//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1426: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		label1 = new Label();
		dgvSender = new DataGridView();
		dgIsChecked = new DataGridViewCheckBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		gsmCrecitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		gsmUpdateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		gsmMaxSentDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		accountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		usernameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		passwordDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		domainDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fullDomainDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		stDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		titleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		SenderBindingSource = new BindingSource(components);
		label2 = new Label();
		txtResiver = new TextBox();
		lblTextMessage = new Label();
		txtMessage = new TextBox();
		label4 = new Label();
		label5 = new Label();
		btnSend = new Button();
		btnCancel = new Button();
		numSendMin = new NumericUpDown();
		numSendHour = new NumericUpDown();
		txtExpDate = new DateBoxFa();
		txtSendDate = new DateBoxFa();
		((ISupportInitialize)dgvSender).BeginInit();
		((ISupportInitialize)SenderBindingSource).BeginInit();
		((ISupportInitialize)numSendMin).BeginInit();
		((ISupportInitialize)numSendHour).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(628, 35);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(103, 22);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "شماره فرستنده";
		dgvSender.AllowUserToAddRows = false;
		dgvSender.AllowUserToDeleteRows = false;
		dgvSender.AllowUserToResizeColumns = false;
		dgvSender.AllowUserToResizeRows = false;
		val.BackColor = Color.White;
		dgvSender.AlternatingRowsDefaultCellStyle = val;
		((Control)dgvSender).Anchor = (AnchorStyles)11;
		dgvSender.AutoGenerateColumns = false;
		dgvSender.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)16;
		dgvSender.BackgroundColor = SystemColors.Window;
		dgvSender.BorderStyle = (BorderStyle)2;
		dgvSender.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dgvSender.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[15]
		{
			(DataGridViewColumn)dgIsChecked,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)typeDataGridViewTextBoxColumn,
			(DataGridViewColumn)gsmCrecitDataGridViewTextBoxColumn,
			(DataGridViewColumn)gsmUpdateDataGridViewTextBoxColumn,
			(DataGridViewColumn)gsmMaxSentDataGridViewTextBoxColumn,
			(DataGridViewColumn)accountDataGridViewTextBoxColumn,
			(DataGridViewColumn)idDataGridViewTextBoxColumn,
			(DataGridViewColumn)usernameDataGridViewTextBoxColumn,
			(DataGridViewColumn)passwordDataGridViewTextBoxColumn,
			(DataGridViewColumn)domainDataGridViewTextBoxColumn,
			(DataGridViewColumn)fullDomainDataGridViewTextBoxColumn,
			(DataGridViewColumn)stDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)titleDataGridViewTextBoxColumn
		});
		dgvSender.DataSource = SenderBindingSource;
		dgvSender.GridColor = SystemColors.ActiveBorder;
		((Control)dgvSender).Location = new Point(392, 61);
		((Control)dgvSender).Margin = new Padding(4);
		((Control)dgvSender).Name = "dgvSender";
		dgvSender.ReadOnly = true;
		val2.Font = new Font("PeydaWeb ExtBd", 12f, (FontStyle)1);
		dgvSender.RowsDefaultCellStyle = val2;
		((Control)dgvSender).Size = new Size(335, 356);
		((Control)dgvSender).TabIndex = 2;
		dgvSender.CellContentClick += new DataGridViewCellEventHandler(dgvSender_CellContentClick);
		((DataGridViewColumn)dgIsChecked).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgIsChecked).DataPropertyName = "IsChecked";
		((DataGridViewColumn)dgIsChecked).FillWeight = 30.45685f;
		((DataGridViewColumn)dgIsChecked).HeaderText = "";
		((DataGridViewColumn)dgIsChecked).Name = "dgIsChecked";
		((DataGridViewBand)dgIsChecked).ReadOnly = true;
		((DataGridViewColumn)dgIsChecked).Width = 5;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).FillWeight = 169.5432f;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "فرستنده";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)senderNumberDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).DataPropertyName = "Type";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).HeaderText = "Type";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).Name = "typeDataGridViewTextBoxColumn";
		((DataGridViewBand)typeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)typeDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)gsmCrecitDataGridViewTextBoxColumn).DataPropertyName = "GsmCrecit";
		((DataGridViewColumn)gsmCrecitDataGridViewTextBoxColumn).HeaderText = "GsmCrecit";
		((DataGridViewColumn)gsmCrecitDataGridViewTextBoxColumn).Name = "gsmCrecitDataGridViewTextBoxColumn";
		((DataGridViewBand)gsmCrecitDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)gsmCrecitDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)gsmUpdateDataGridViewTextBoxColumn).DataPropertyName = "GsmUpdate";
		((DataGridViewColumn)gsmUpdateDataGridViewTextBoxColumn).HeaderText = "GsmUpdate";
		((DataGridViewColumn)gsmUpdateDataGridViewTextBoxColumn).Name = "gsmUpdateDataGridViewTextBoxColumn";
		((DataGridViewBand)gsmUpdateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)gsmUpdateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)gsmMaxSentDataGridViewTextBoxColumn).DataPropertyName = "GsmMaxSent";
		((DataGridViewColumn)gsmMaxSentDataGridViewTextBoxColumn).HeaderText = "GsmMaxSent";
		((DataGridViewColumn)gsmMaxSentDataGridViewTextBoxColumn).Name = "gsmMaxSentDataGridViewTextBoxColumn";
		((DataGridViewBand)gsmMaxSentDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)gsmMaxSentDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)accountDataGridViewTextBoxColumn).DataPropertyName = "Account";
		((DataGridViewColumn)accountDataGridViewTextBoxColumn).HeaderText = "Account";
		((DataGridViewColumn)accountDataGridViewTextBoxColumn).Name = "accountDataGridViewTextBoxColumn";
		((DataGridViewBand)accountDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)accountDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)idDataGridViewTextBoxColumn).DataPropertyName = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).HeaderText = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).Name = "idDataGridViewTextBoxColumn";
		((DataGridViewBand)idDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)idDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).DataPropertyName = "Username";
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).HeaderText = "Username";
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).Name = "usernameDataGridViewTextBoxColumn";
		((DataGridViewBand)usernameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)usernameDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).DataPropertyName = "Password";
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).HeaderText = "Password";
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).Name = "passwordDataGridViewTextBoxColumn";
		((DataGridViewBand)passwordDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)passwordDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).DataPropertyName = "Domain";
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).HeaderText = "Domain";
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).Name = "domainDataGridViewTextBoxColumn";
		((DataGridViewBand)domainDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)domainDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).DataPropertyName = "FullDomain";
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).HeaderText = "FullDomain";
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).Name = "fullDomainDataGridViewTextBoxColumn";
		((DataGridViewBand)fullDomainDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)fullDomainDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)stDataGridViewTextBoxColumn).DataPropertyName = "St";
		((DataGridViewColumn)stDataGridViewTextBoxColumn).HeaderText = "St";
		((DataGridViewColumn)stDataGridViewTextBoxColumn).Name = "stDataGridViewTextBoxColumn";
		((DataGridViewBand)stDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)stDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).DataPropertyName = "Title";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).HeaderText = "Title";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).Name = "titleDataGridViewTextBoxColumn";
		((DataGridViewBand)titleDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)titleDataGridViewTextBoxColumn).Visible = false;
		SenderBindingSource.DataSource = typeof(SMSNet_Account);
		SenderBindingSource.CurrentChanged += SenderBindingSource_CurrentChanged;
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label2).Location = new Point(286, 35);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(97, 22);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "شماره گیرنده";
		((Control)txtResiver).Anchor = (AnchorStyles)15;
		((Control)txtResiver).Font = new Font("PeydaWeb SemBd", 9.749999f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtResiver).Location = new Point(13, 61);
		((Control)txtResiver).Margin = new Padding(4);
		((TextBoxBase)txtResiver).Multiline = true;
		((Control)txtResiver).Name = "txtResiver";
		txtResiver.ScrollBars = (ScrollBars)2;
		((Control)txtResiver).Size = new Size(366, 355);
		((Control)txtResiver).TabIndex = 5;
		((Control)lblTextMessage).Anchor = (AnchorStyles)14;
		((Control)lblTextMessage).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)lblTextMessage).Location = new Point(360, 427);
		((Control)lblTextMessage).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTextMessage).Name = "lblTextMessage";
		((Control)lblTextMessage).Size = new Size(371, 22);
		((Control)lblTextMessage).TabIndex = 6;
		((Control)lblTextMessage).Text = "متن پیام";
		((Control)txtMessage).Anchor = (AnchorStyles)14;
		((Control)txtMessage).Location = new Point(347, 464);
		((Control)txtMessage).Margin = new Padding(4);
		((TextBoxBase)txtMessage).Multiline = true;
		((Control)txtMessage).Name = "txtMessage";
		((Control)txtMessage).Size = new Size(380, 350);
		((Control)txtMessage).TabIndex = 7;
		((Control)txtMessage).TextChanged += txtMessage_TextChanged_1;
		((Control)label4).Anchor = (AnchorStyles)6;
		((Control)label4).AutoSize = true;
		((Control)label4).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)label4).Location = new Point(202, 511);
		((Control)label4).Margin = new Padding(4, 0, 4, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(79, 22);
		((Control)label4).TabIndex = 8;
		((Control)label4).Text = "تاریخ ارسال";
		((Control)label5).Anchor = (AnchorStyles)6;
		((Control)label5).AutoSize = true;
		((Control)label5).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)label5).Location = new Point(202, 655);
		((Control)label5).Margin = new Padding(4, 0, 4, 0);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(79, 22);
		((Control)label5).TabIndex = 9;
		((Control)label5).Text = "تاریخ انقضا";
		((Control)btnSend).Anchor = (AnchorStyles)6;
		((Control)btnSend).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)btnSend).Location = new Point(189, 786);
		((Control)btnSend).Margin = new Padding(4);
		((Control)btnSend).Name = "btnSend";
		((Control)btnSend).Size = new Size(88, 28);
		((Control)btnSend).TabIndex = 14;
		((Control)btnSend).Text = "تایید";
		((ButtonBase)btnSend).UseVisualStyleBackColor = true;
		((Control)btnSend).Click += btnSend_Click;
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)btnCancel).Location = new Point(63, 786);
		((Control)btnCancel).Margin = new Padding(4);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Size = new Size(88, 28);
		((Control)btnCancel).TabIndex = 15;
		((Control)btnCancel).Text = "انصراف";
		((ButtonBase)btnCancel).UseVisualStyleBackColor = true;
		((Control)btnCancel).Click += button3_Click;
		((Control)numSendMin).Anchor = (AnchorStyles)6;
		((Control)numSendMin).Location = new Point(123, 610);
		((Control)numSendMin).Margin = new Padding(4);
		numSendMin.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)numSendMin).Name = "numSendMin";
		((Control)numSendMin).Size = new Size(58, 22);
		((Control)numSendMin).TabIndex = 16;
		((Control)numSendHour).Anchor = (AnchorStyles)6;
		((Control)numSendHour).Location = new Point(189, 610);
		((Control)numSendHour).Margin = new Padding(4);
		numSendHour.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)numSendHour).Name = "numSendHour";
		((Control)numSendHour).Size = new Size(58, 22);
		((Control)numSendHour).TabIndex = 17;
		txtExpDate.AllowNull = false;
		((Control)txtExpDate).Anchor = (AnchorStyles)6;
		txtExpDate.BackcolorTooltip = Color.Empty;
		txtExpDate.Border.CornerType = eCornerType.Square;
		txtExpDate.EnterToTab = true;
		txtExpDate.EscToClose = true;
		txtExpDate.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.none;
		txtExpDate.FocusBackColor = Color.Yellow;
		txtExpDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtExpDate.FocusForeColor = Color.Blue;
		txtExpDate.FocusTextSelect = true;
		((Control)txtExpDate).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)txtExpDate).Location = new Point(63, 691);
		((Control)txtExpDate).Margin = new Padding(4);
		txtExpDate.MessageEmptyShowDialog = false;
		txtExpDate.MessegeEmpty = "";
		txtExpDate.MessegeEmptyInFormRight = true;
		txtExpDate.MessegeEmptyShowInForm = false;
		txtExpDate.MiladiDate = new DateTime(2023, 5, 29, 19, 44, 19, 147);
		((TextBoxBase)txtExpDate).Multiline = true;
		((Control)txtExpDate).Name = "txtExpDate";
		((Control)txtExpDate).RightToLeft = (RightToLeft)0;
		txtExpDate.ShowNameOfDayOnToolTip = false;
		txtExpDate.ShowToolTipText = false;
		((Control)txtExpDate).Size = new Size(214, 39);
		((Control)txtExpDate).TabIndex = 11;
		((Control)txtExpDate).Text = "5/29/2023 ";
		((TextBox)txtExpDate).TextAlign = (HorizontalAlignment)2;
		txtExpDate.TootTipDelay = 0;
		txtSendDate.AllowNull = false;
		((Control)txtSendDate).Anchor = (AnchorStyles)6;
		txtSendDate.BackcolorTooltip = Color.Empty;
		txtSendDate.Border.CornerType = eCornerType.Square;
		txtSendDate.EnterToTab = true;
		txtSendDate.EscToClose = true;
		txtSendDate.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.none;
		txtSendDate.FocusBackColor = Color.White;
		txtSendDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtSendDate.FocusForeColor = Color.White;
		txtSendDate.FocusHighlightColor = Color.White;
		txtSendDate.FocusTextSelect = true;
		((Control)txtSendDate).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)txtSendDate).Location = new Point(63, 549);
		((Control)txtSendDate).Margin = new Padding(4);
		txtSendDate.MessageEmptyShowDialog = false;
		txtSendDate.MessegeEmpty = "";
		txtSendDate.MessegeEmptyInFormRight = true;
		txtSendDate.MessegeEmptyShowInForm = false;
		txtSendDate.MiladiDate = new DateTime(2023, 5, 29, 19, 44, 19, 149);
		((TextBoxBase)txtSendDate).Multiline = true;
		((Control)txtSendDate).Name = "txtSendDate";
		((Control)txtSendDate).RightToLeft = (RightToLeft)0;
		txtSendDate.ShowNameOfDayOnToolTip = false;
		txtSendDate.ShowToolTipText = false;
		((Control)txtSendDate).Size = new Size(214, 39);
		((Control)txtSendDate).TabIndex = 10;
		((Control)txtSendDate).Text = "5/29/2023 ";
		((TextBox)txtSendDate).TextAlign = (HorizontalAlignment)2;
		txtSendDate.TootTipDelay = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(750, 841);
		((Control)this).Controls.Add((Control)(object)numSendHour);
		((Control)this).Controls.Add((Control)(object)numSendMin);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnSend);
		((Control)this).Controls.Add((Control)(object)txtExpDate);
		((Control)this).Controls.Add((Control)(object)txtSendDate);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)txtMessage);
		((Control)this).Controls.Add((Control)(object)lblTextMessage);
		((Control)this).Controls.Add((Control)(object)txtResiver);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)dgvSender);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("Book Antiqua", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4);
		((Control)this).Name = "frmSendMassage";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Text = "پنل ارسال پیامک";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += frmSendMassage_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmSendMassage_KeyDown);
		((ISupportInitialize)dgvSender).EndInit();
		((ISupportInitialize)SenderBindingSource).EndInit();
		((ISupportInitialize)numSendMin).EndInit();
		((ISupportInitialize)numSendHour).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
