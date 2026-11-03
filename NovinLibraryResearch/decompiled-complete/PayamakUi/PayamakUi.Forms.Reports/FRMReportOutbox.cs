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
using FormCustom;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;
using PayamakUi.Mapper;
using PayamakUi.Properties;

namespace PayamakUi.Forms.Reports;

public class FRMReportOutbox : AccountingBaseForm
{
	private List<SmsOutBox> LstOutBox = new List<SmsOutBox>();

	private CancellationTokenSource _loadDataToken;

	private PayamakClientHubInstances _instance;

	private PayamakFilterModel _payamakFilterModel;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal Panel Panel1;

	internal TextBoxX TextBoxX1;

	internal GroupPanel GroupPanel2;

	internal Label LBLTotal;

	internal Label LabelX3;

	internal ButtonX BTNDelete;

	internal ButtonX BTNExit;

	internal ButtonX BTNForward;

	internal DataGridViewX DGrid;

	internal ContextMenuStrip ContextMenue;

	internal ToolStripMenuItem انتخابToolStripMenuItem;

	internal ToolStripMenuItem CMASelectAll;

	internal ToolStripMenuItem CMASelectNone;

	internal ToolStripMenuItem CMASelectReverce;

	internal BindingSource ViewOutBoxBindingSource;

	internal ButtonX BTNReply;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewImageColumn Img;

	private DataGridViewTextBoxColumn DGNumber;

	private DataGridViewTextBoxColumn schadulingMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGSchadulingSh;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewCheckBoxColumn deliveryDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn refrenceAccountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

	private DataGridViewTextBoxColumn DGMessage;

	private DataGridViewCheckBoxColumn DGST;

	internal ButtonX BTNSelectAll;

	public FRMReportOutbox(string contextGuid, PayamakFilterModel payamakFilterModel, Font font)
		: base(contextGuid)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		try
		{
			InitializeComponent();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
			((Control)this).KeyDown += new KeyEventHandler(FRMReportOutbox_KeyDown);
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			_payamakFilterModel = payamakFilterModel;
			Task.Run(() => LoadData());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 40);
		}
	}

	public async Task LoadData(CancellationToken internalToken = default(CancellationToken))
	{
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { internalToken });
			CancellationToken token = _loadDataToken.Token;
			ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>> returnedSaveFuncInfoWithValue = await _instance.SmsOutBox.GetAllAsync(token, _payamakFilterModel.DateM1, _payamakFilterModel.DateM2, st: true);
			if (returnedSaveFuncInfoWithValue?.value != null)
			{
				LstOutBox = await SmsOutBoxMapper.Instance.MapListAsync(base.ContextGuid, returnedSaveFuncInfoWithValue.value);
			}
			Task.Run(() => Search(token));
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 55);
		}
	}

	private async Task Search(CancellationToken token)
	{
		IEnumerable<SmsOutBox> SmstempOutBox = LstOutBox;
		if (token.IsCancellationRequested)
		{
			return;
		}
		SmstempOutBox = SmstempOutBox.Where((SmsOutBox p) => p.FixedMessage.Contains(_payamakFilterModel.MobG) || p.Number.Contains(((Control)this).Text) || p.SenderNumber.Contains(_payamakFilterModel.MobF))?.ToSortableBindingList(token);
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						ViewOutBoxBindingSource.DataSource = SmstempOutBox;
						((Control)LBLTotal).Text = SmstempOutBox.Count().ToString("N0");
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 83);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 86);
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
			WebErrorLog.Instance.StartLog(ex, "", "Where_Add", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 192);
			return Where;
		}
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 204);
		}
	}

	private void BTNPrint_Click(object sender, EventArgs e)
	{
	}

	private void BTNForward_Click(object sender, EventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount <= 0)
			{
				return;
			}
			string message = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
			if ((int)((Form)new FRMSend(base.ContextGuid, message)
			{
				Message = message
			}).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Task.Run(() => LoadData());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNForward_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 238);
		}
	}

	private void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentCell.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				((DataGridView)DGrid).CurrentCell = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index];
			}
			DataGridViewCell obj = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index];
			string text = ((obj == null) ? null : obj.Value?.ToString()?.FixString()?.Trim());
			((Control)TextBoxX1).Text = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 252);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		try
		{
			List<int> list = new List<int>();
			((Control)DGrid).Focus();
			if ((int)MessageBox.Show("مایل به حذف پیام در حال ارسال انتخاب شده هستید ؟".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) == 7)
			{
				return;
			}
			int i = 0;
			for (int num = ((DataGridView)DGrid).RowCount - 1; i <= num; i++)
			{
				if (((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value != null && bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()))
				{
					list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
				}
			}
			await _instance.SmsOutBox.DeleteListAsync(list, default(CancellationToken));
			Task.Run(() => LoadData());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 277);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 289);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 302);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectReverce_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 324);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentCell == null)
			{
				((Control)TextBoxX1).Text = "";
				return;
			}
			if ((int)e.KeyCode == 32 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
			{
				if (((DataGridView)DGrid).RowCount == 0)
				{
					return;
				}
				int num = (int)((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
				int i = 0;
				for (int num2 = ((DataGridView)DGrid).RowCount - 1; i <= num2; i++)
				{
					if (num == (int)((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value)
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
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 372);
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
				int num = (int)((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
				int i = 0;
				for (int num2 = ((DataGridView)DGrid).RowCount - 1; i <= num2; i++)
				{
					if (num == (int)((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value)
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
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 413);
		}
	}

	private void FRMReportOutbox_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "FRMReportOutbox_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 441);
		}
	}

	private void FRMReportOutbox_Load(object sender, EventArgs e)
	{
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
				Task.Run(() => LoadData());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNReply_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 475);
		}
	}

	private void RefreshLables()
	{
		try
		{
			((Control)LBLTotal).Text = ((DataGridView)DGrid).RowCount.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RefreshLables", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 485);
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
						break;
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
			WebErrorLog.Instance.StartLog(ex, "", "BTNSelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMReportOutboxcs.cs", 515);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
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
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Expected O, but got Unknown
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Expected O, but got Unknown
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected O, but got Unknown
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10db: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Expected O, but got Unknown
		//IL_1109: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1322: Unknown result type (might be due to invalid IL or missing references)
		//IL_132c: Expected O, but got Unknown
		//IL_134a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1424: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		ContextMenue = new ContextMenuStrip(components);
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		CMASelectAll = new ToolStripMenuItem();
		CMASelectNone = new ToolStripMenuItem();
		CMASelectReverce = new ToolStripMenuItem();
		PanelEx1 = new PanelEx();
		BTNReply = new ButtonX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		Img = new DataGridViewImageColumn();
		DGNumber = new DataGridViewTextBoxColumn();
		schadulingMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGSchadulingSh = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
		deliveryDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		refrenceAccountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		messageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
		DGMessage = new DataGridViewTextBoxColumn();
		DGST = new DataGridViewCheckBoxColumn();
		ViewOutBoxBindingSource = new BindingSource(components);
		Panel1 = new Panel();
		TextBoxX1 = new TextBoxX();
		GroupPanel2 = new GroupPanel();
		LBLTotal = new Label();
		LabelX3 = new Label();
		BTNDelete = new ButtonX();
		BTNExit = new ButtonX();
		BTNForward = new ButtonX();
		BTNSelectAll = new ButtonX();
		((Control)ContextMenue).SuspendLayout();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)ViewOutBoxBindingSource).BeginInit();
		((Control)Panel1).SuspendLayout();
		((Control)GroupPanel2).SuspendLayout();
		((Control)this).SuspendLayout();
		((ToolStrip)ContextMenue).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)انتخابToolStripMenuItem });
		((Control)ContextMenue).Name = "ContextMenue";
		((Control)ContextMenue).RightToLeft = (RightToLeft)1;
		((Control)ContextMenue).Size = new Size(115, 26);
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)CMASelectAll,
			(ToolStripItem)CMASelectNone,
			(ToolStripItem)CMASelectReverce
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(114, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)انتخابToolStripMenuItem).Click += CMASelectAll_Click;
		((ToolStripItem)CMASelectAll).Name = "CMASelectAll";
		((ToolStripItem)CMASelectAll).Size = new Size(184, 22);
		((ToolStripItem)CMASelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)CMASelectNone).Name = "CMASelectNone";
		((ToolStripItem)CMASelectNone).Size = new Size(184, 22);
		((ToolStripItem)CMASelectNone).Text = "[انتخاب هیچکدام]";
		((ToolStripItem)CMASelectNone).Click += CMASelectNone_Click;
		((ToolStripItem)CMASelectReverce).Name = "CMASelectReverce";
		((ToolStripItem)CMASelectReverce).Size = new Size(184, 22);
		((ToolStripItem)CMASelectReverce).Text = "[معکوس کردن انتخاب]";
		((ToolStripItem)CMASelectReverce).Click += CMASelectReverce_Click;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)BTNSelectAll);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNReply);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)Panel1);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel2);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNForward);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(621, 343);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 3;
		((Control)BTNReply).AccessibleRole = (AccessibleRole)43;
		((Control)BTNReply).Anchor = (AnchorStyles)10;
		BTNReply.ColorTable = eButtonColor.Office2007WithBackground;
		BTNReply.Image = (Image)(object)Resources.Replyed;
		((Control)BTNReply).Location = new Point(540, 312);
		((Control)BTNReply).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNReply).Name = "BTNReply";
		((Control)BTNReply).Size = new Size(35, 28);
		((Control)BTNReply).TabIndex = 28;
		((Control)BTNReply).TabStop = false;
		BTNReply.Tooltip = "پاسخ دادن به پیام";
		((Control)BTNReply).Click += BTNReply_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.MintCream;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[24]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)Img,
			(DataGridViewColumn)DGNumber,
			(DataGridViewColumn)schadulingMDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGSchadulingSh,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)dataGridViewTextBoxColumn3,
			(DataGridViewColumn)dataGridViewTextBoxColumn4,
			(DataGridViewColumn)dataGridViewTextBoxColumn5,
			(DataGridViewColumn)deliveryDataGridViewCheckBoxColumn,
			(DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn6,
			(DataGridViewColumn)dataGridViewTextBoxColumn7,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn8,
			(DataGridViewColumn)dataGridViewTextBoxColumn9,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)messageDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn11,
			(DataGridViewColumn)dataGridViewTextBoxColumn12,
			(DataGridViewColumn)dataGridViewTextBoxColumn13,
			(DataGridViewColumn)DGMessage,
			(DataGridViewColumn)DGST
		});
		((Control)DGrid).ContextMenuStrip = ContextMenue;
		((DataGridView)DGrid).DataSource = ViewOutBoxBindingSource;
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
		((Control)DGrid).Location = new Point(4, 12);
		((Control)DGrid).Margin = new Padding(4, 3, 4, 3);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(612, 240);
		((Control)DGrid).TabIndex = 27;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)Img).DataPropertyName = "Img";
		((DataGridViewColumn)Img).HeaderText = "";
		((DataGridViewColumn)Img).Name = "Img";
		((DataGridViewBand)Img).Visible = false;
		((DataGridViewColumn)DGNumber).DataPropertyName = "Number";
		((DataGridViewColumn)DGNumber).HeaderText = "شماره گیرنده";
		((DataGridViewColumn)DGNumber).Name = "DGNumber";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).DataPropertyName = "SchadulingM";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).HeaderText = "SchadulingM";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).Name = "schadulingMDataGridViewTextBoxColumn";
		((DataGridViewBand)schadulingMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DGSchadulingSh).DataPropertyName = "SchadulingSh";
		((DataGridViewColumn)DGSchadulingSh).HeaderText = "تاریخ ارسال";
		((DataGridViewColumn)DGSchadulingSh).Name = "DGSchadulingSh";
		((DataGridViewBand)DGSchadulingSh).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "SchadulingType";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "SchadulingType";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Priority";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "Priority";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "DeletedItemsId";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "DeletedItemsId";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).DataPropertyName = "DraftId";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).HeaderText = "DraftId";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Name = "dataGridViewTextBoxColumn4";
		((DataGridViewBand)dataGridViewTextBoxColumn4).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn5).DataPropertyName = "SentBoxId";
		((DataGridViewColumn)dataGridViewTextBoxColumn5).HeaderText = "SentBoxId";
		((DataGridViewColumn)dataGridViewTextBoxColumn5).Name = "dataGridViewTextBoxColumn5";
		((DataGridViewBand)dataGridViewTextBoxColumn5).Visible = false;
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).DataPropertyName = "Delivery";
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).HeaderText = "تحویل شده";
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).Name = "deliveryDataGridViewCheckBoxColumn";
		((DataGridViewBand)deliveryDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).DataPropertyName = "RefrenceAccountId";
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).HeaderText = "RefrenceAccountId";
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).Name = "refrenceAccountIdDataGridViewTextBoxColumn";
		((DataGridViewBand)refrenceAccountIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn6).DataPropertyName = "Rstr4";
		((DataGridViewColumn)dataGridViewTextBoxColumn6).HeaderText = "Rstr4";
		((DataGridViewColumn)dataGridViewTextBoxColumn6).Name = "dataGridViewTextBoxColumn6";
		((DataGridViewBand)dataGridViewTextBoxColumn6).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn7).DataPropertyName = "Rstr5";
		((DataGridViewColumn)dataGridViewTextBoxColumn7).HeaderText = "Rstr5";
		((DataGridViewColumn)dataGridViewTextBoxColumn7).Name = "dataGridViewTextBoxColumn7";
		((DataGridViewBand)dataGridViewTextBoxColumn7).Visible = false;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)senderNumberDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn8).DataPropertyName = "RefrencedId";
		((DataGridViewColumn)dataGridViewTextBoxColumn8).HeaderText = "RefrencedId";
		((DataGridViewColumn)dataGridViewTextBoxColumn8).Name = "dataGridViewTextBoxColumn8";
		((DataGridViewBand)dataGridViewTextBoxColumn8).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn9).DataPropertyName = "AccountId";
		((DataGridViewColumn)dataGridViewTextBoxColumn9).HeaderText = "AccountId";
		((DataGridViewColumn)dataGridViewTextBoxColumn9).Name = "dataGridViewTextBoxColumn9";
		((DataGridViewBand)dataGridViewTextBoxColumn9).Visible = false;
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).DataPropertyName = "Message";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).HeaderText = "Message";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).Name = "messageDataGridViewTextBoxColumn";
		((DataGridViewBand)messageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn11).DataPropertyName = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn11).HeaderText = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn11).Name = "dataGridViewTextBoxColumn11";
		((DataGridViewBand)dataGridViewTextBoxColumn11).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn11).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn12).DataPropertyName = "CustomerId";
		((DataGridViewColumn)dataGridViewTextBoxColumn12).HeaderText = "CustomerId";
		((DataGridViewColumn)dataGridViewTextBoxColumn12).Name = "dataGridViewTextBoxColumn12";
		((DataGridViewBand)dataGridViewTextBoxColumn12).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn13).DataPropertyName = "CustomerName";
		((DataGridViewColumn)dataGridViewTextBoxColumn13).HeaderText = "شخص";
		((DataGridViewColumn)dataGridViewTextBoxColumn13).Name = "dataGridViewTextBoxColumn13";
		((DataGridViewBand)dataGridViewTextBoxColumn13).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn13).Visible = false;
		((DataGridViewColumn)DGMessage).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGMessage).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)DGMessage).HeaderText = "متن پیام";
		((DataGridViewColumn)DGMessage).Name = "DGMessage";
		((DataGridViewBand)DGMessage).ReadOnly = true;
		((DataGridViewColumn)DGST).DataPropertyName = "ST";
		((DataGridViewColumn)DGST).HeaderText = "ST";
		((DataGridViewColumn)DGST).Name = "DGST";
		((DataGridViewBand)DGST).Visible = false;
		ViewOutBoxBindingSource.DataSource = typeof(SmsOutBox);
		((Control)Panel1).Anchor = (AnchorStyles)14;
		Panel1.BorderStyle = (BorderStyle)1;
		((Control)Panel1).Controls.Add((Control)(object)TextBoxX1);
		((Control)Panel1).Location = new Point(4, 258);
		((Control)Panel1).Margin = new Padding(4, 3, 4, 3);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(400, 48);
		((Control)Panel1).TabIndex = 26;
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
		((Control)TextBoxX1).Size = new Size(398, 46);
		((Control)TextBoxX1).TabIndex = 24;
		((Control)GroupPanel2).Anchor = (AnchorStyles)10;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)LBLTotal);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX3);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)GroupPanel2).Location = new Point(411, 258);
		((Control)GroupPanel2).Margin = new Padding(4, 3, 4, 3);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(206, 48);
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
		((Control)GroupPanel2).TabIndex = 25;
		((Control)LBLTotal).Location = new Point(1, 10);
		((Control)LBLTotal).Margin = new Padding(4, 0, 4, 0);
		((Control)LBLTotal).Name = "LBLTotal";
		((Control)LBLTotal).Size = new Size(86, 19);
		((Control)LBLTotal).TabIndex = 16;
		((Control)LBLTotal).Text = "0";
		LBLTotal.TextAlign = (ContentAlignment)16;
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX3).Location = new Point(94, 10);
		((Control)LabelX3).Margin = new Padding(4, 0, 4, 0);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).RightToLeft = (RightToLeft)1;
		((Control)LabelX3).Size = new Size(112, 19);
		((Control)LabelX3).TabIndex = 14;
		((Control)LabelX3).Text = "کل پیام ها :";
		LabelX3.TextAlign = (ContentAlignment)64;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BTNDelete).Anchor = (AnchorStyles)10;
		((Control)BTNDelete).BackgroundImageLayout = (ImageLayout)2;
		BTNDelete.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNDelete).Cursor = Cursors.Default;
		BTNDelete.Image = (Image)(object)Resources.delete1;
		((Control)BTNDelete).Location = new Point(495, 312);
		((Control)BTNDelete).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).Size = new Size(35, 28);
		BTNDelete.Style = eDotNetBarStyle.Metro;
		((Control)BTNDelete).TabIndex = 14;
		((Control)BTNDelete).TabStop = false;
		BTNDelete.Tooltip = "حذف پیام ارسالی";
		((Control)BTNDelete).Click += BTNDelete_Click;
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTNExit.Image = (Image)(object)Resources.gnome_application_exit;
		((Control)BTNExit).Location = new Point(4, 311);
		((Control)BTNExit).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(35, 28);
		((Control)BTNExit).TabIndex = 23;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "خروج از صفحه";
		((Control)BTNExit).Click += BTNExit_Click;
		((Control)BTNForward).AccessibleRole = (AccessibleRole)43;
		((Control)BTNForward).Anchor = (AnchorStyles)10;
		BTNForward.ColorTable = eButtonColor.Office2007WithBackground;
		BTNForward.Image = (Image)(object)Resources.mail_forward2;
		((Control)BTNForward).Location = new Point(582, 312);
		((Control)BTNForward).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNForward).Name = "BTNForward";
		((Control)BTNForward).Size = new Size(35, 28);
		((Control)BTNForward).TabIndex = 18;
		((Control)BTNForward).TabStop = false;
		BTNForward.Tooltip = "ارسال پیام به دیگری";
		((Control)BTNForward).Click += BTNForward_Click;
		((Control)BTNSelectAll).AccessibleRole = (AccessibleRole)43;
		((Control)BTNSelectAll).Anchor = (AnchorStyles)10;
		BTNSelectAll.ColorTable = eButtonColor.Office2007WithBackground;
		BTNSelectAll.Image = (Image)(object)Resources.accept_page1;
		BTNSelectAll.ImagePosition = eImagePosition.Right;
		((Control)BTNSelectAll).Location = new Point(378, 311);
		((Control)BTNSelectAll).Name = "BTNSelectAll";
		((Control)BTNSelectAll).Size = new Size(110, 28);
		BTNSelectAll.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNSelectAll).TabIndex = 38;
		((Control)BTNSelectAll).Text = "[انتخاب همه]";
		((Control)BTNSelectAll).Click += BTNSelectAll_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(621, 343);
		((Control)this).ContextMenuStrip = ContextMenue;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Control)this).Name = "FRMReportOutbox";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "گزارش صندوق در حال ارسال";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FRMReportOutbox_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMReportOutbox_KeyDown);
		((Control)ContextMenue).ResumeLayout(false);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)ViewOutBoxBindingSource).EndInit();
		((Control)Panel1).ResumeLayout(false);
		((Control)GroupPanel2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
