using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using PayamakSettings;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;

namespace PayamakUi.UC.UCSMS;

public class UCOutBox : UserControl, IHasContext
{
	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private string _selectedDate;

	private PayamakClientHubInstances _instance;

	private IContainer components;

	internal GroupPanel GroupPanel1;

	internal Label LblSearch_Outbox;

	internal TextBoxX TXTSearch;

	internal ButtonX BTNAddPeople;

	internal ButtonX BTNBlackList;

	internal ButtonX BTNForward;

	internal ButtonX BTNDelete;

	internal DataGridViewX DGrid;

	internal BindingSource SMSOutBoxBindingSource;

	internal ContextMenuStrip ContextMenue;

	internal ToolStripMenuItem انتخابToolStripMenuItem;

	internal ToolStripMenuItem CMASelectAll;

	internal ToolStripMenuItem CMASelectNone;

	internal ToolStripMenuItem CMASelectReverce;

	internal ButtonX BTNExit;

	internal GroupPanel GroupPanel2;

	internal Label LBLTotal;

	internal Label LabelX3;

	internal TextBoxX TextBoxX1;

	internal Label LabelX1;

	internal ComboBoxEx CMBDates;

	internal PanelEx PanelEx1;

	internal Panel Panel1;

	internal ButtonX BTNPrint;

	private DataGridViewCheckBoxXColumn DGCheck;

	private DataGridViewTextBoxColumn schadulingMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn schadulingShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn schadulingTypeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deletedItemsIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn draftIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sentBoxIdDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn deliveryDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn refrenceAccountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn rstr4DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn rstr5DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGNumber;

	private DataGridViewTextBoxColumn refrencedIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn accountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGMessage;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn fixedMessageDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn sTDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	public string ContextGuid { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!((Control)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public UCOutBox()
	{
		InitializeComponent();
		Task.Run((Func<Task?>)LoadDataAsync);
	}

	private async Task LoadDataAsync()
	{
		try
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				return;
			}
			Font font = await Font.GetFontProgramAsync(default(CancellationToken));
			_instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			SortableBindingList<BaseSmsOutBox> listForBind = (await _instance.SmsOutBox.GetAllAsync(token, (_selectedDate ?? DateTime.Now.Date.ToString()).ShamsiToMiladi(), DateTime.Now, st: true))?.value?.ToSortableBindingList(token);
			do
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((Control)DGrid).Font = font;
						((Control)DGrid).Focus();
						SMSOutBoxBindingSource.DataSource = listForBind;
						((Control)LBLTotal).Text = ((DataGridView)DGrid).RowCount.ToString();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 61);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 64);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex < 0 || e.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
			{
				return;
			}
			int num = int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			int i = 0;
			for (int num2 = ((DataGridView)DGrid).RowCount - 1; i <= num2; i++)
			{
				if (num == int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()))
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 88);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount <= 0)
			{
				return;
			}
			if (((DataGridView)DGrid).CurrentCell.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				((DataGridView)DGrid).CurrentCell = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index];
			}
			if ((int)e.KeyCode != 32 || ((DataGridView)DGrid).RowCount == 0)
			{
				return;
			}
			int num = int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			int i = 0;
			for (int num2 = ((DataGridView)DGrid).RowCount - 1; i <= num2; i++)
			{
				if (num == int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()))
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 121);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 134);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 147);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectReverce_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 169);
		}
	}

	private async void BtnDelete_Inbox_Click(object sender, EventArgs e)
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
			LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDelete_Inbox_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 194);
		}
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount != 0)
			{
				_ = e.RowIndex;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 211);
		}
	}

	private void TXTSearch_GotFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTSearch).BackColor = Color.Yellow;
			((TextBoxBase)TXTSearch).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 224);
		}
	}

	private void TXTSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode == 13) | ((int)e.KeyCode == 40))
			{
				((Control)DGrid).Focus();
			}
			else if ((((Control)TXTSearch).Text != "") & ((int)e.KeyCode == 27))
			{
				((Control)TXTSearch).Text = "";
			}
			else if ((((Control)TXTSearch).Text == "") & ((int)e.KeyCode == 27))
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 247);
		}
	}

	private void TXTSearch_LostFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTSearch).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 259);
		}
	}

	private void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 271);
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 284);
		}
	}

	private void BTNForward_Click(object sender, EventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount > 0)
			{
				string message = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
				if ((int)((Form)new FRMSend(ContextGuid)
				{
					Message = message
				}).ShowDialog((IWin32Window)(object)this) == 1)
				{
					LoadDataAsync();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNForward_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 301);
		}
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Visible = false;
			((Control)this).Hide();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 313);
		}
	}

	private void BTNAddPeople_Click(object sender, EventArgs e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNAddPeople_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 325);
		}
	}

	private void BTNBlackList_Click(object sender, EventArgs e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNBlackList_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 337);
		}
	}

	public void Search_Press(char keyChar)
	{
		try
		{
			if (!((Control)TXTSearch).Focused)
			{
				((Control)TXTSearch).Focus();
				((Control)TXTSearch).Text = keyChar.ToString();
				((TextBoxBase)TXTSearch).SelectionStart = 999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search_Press", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 358);
		}
	}

	private void Search()
	{
		try
		{
			string text = ((Control)TXTSearch).Text?.FixString()?.Trim();
			if (string.IsNullOrEmpty(text.Trim()))
			{
				SMSOutBoxBindingSource.Filter = "";
			}
			else
			{
				SMSOutBoxBindingSource.Filter = "Message like '%" + text + "%' or Number like '%" + text + "%'or SenderNumber like '%" + text + "%'";
			}
			((Control)LBLTotal).Text = ((DataGridView)DGrid).RowCount.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 373);
		}
	}

	private void CMBDates_SelectedValueChanged(object sender, EventArgs e)
	{
		try
		{
			_selectedDate = ((ListControl)CMBDates).SelectedValue?.ToString();
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMBDates_SelectedValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 383);
		}
	}

	private void BTNPrint_Click(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCOutBox.cs", 420);
		}
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
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
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Expected O, but got Unknown
		//IL_12d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Expected O, but got Unknown
		//IL_1601: Unknown result type (might be due to invalid IL or missing references)
		//IL_160b: Expected O, but got Unknown
		//IL_16d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e2: Expected O, but got Unknown
		//IL_1794: Unknown result type (might be due to invalid IL or missing references)
		//IL_179e: Expected O, but got Unknown
		//IL_1853: Unknown result type (might be due to invalid IL or missing references)
		//IL_185d: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UCOutBox));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		GroupPanel2 = new GroupPanel();
		LBLTotal = new Label();
		LabelX3 = new Label();
		TextBoxX1 = new TextBoxX();
		SMSOutBoxBindingSource = new BindingSource(components);
		BTNExit = new ButtonX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxXColumn();
		schadulingMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		schadulingShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		priorityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deletedItemsIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		draftIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sentBoxIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		refrenceAccountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		rstr4DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		rstr5DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGNumber = new DataGridViewTextBoxColumn();
		refrencedIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		accountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		DGMessage = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		customerIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		fixedMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sTDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		ContextMenue = new ContextMenuStrip(components);
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		CMASelectAll = new ToolStripMenuItem();
		CMASelectNone = new ToolStripMenuItem();
		CMASelectReverce = new ToolStripMenuItem();
		GroupPanel1 = new GroupPanel();
		BTNPrint = new ButtonX();
		CMBDates = new ComboBoxEx();
		LabelX1 = new Label();
		TXTSearch = new TextBoxX();
		LblSearch_Outbox = new Label();
		BTNAddPeople = new ButtonX();
		BTNBlackList = new ButtonX();
		BTNForward = new ButtonX();
		BTNDelete = new ButtonX();
		PanelEx1 = new PanelEx();
		Panel1 = new Panel();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		schadulingTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((Control)GroupPanel2).SuspendLayout();
		((ISupportInitialize)SMSOutBoxBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)ContextMenue).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((Control)PanelEx1).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)GroupPanel2).Anchor = (AnchorStyles)10;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)LBLTotal);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX3);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)GroupPanel2).Location = new Point(240, 261);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(177, 48);
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
		((Control)LBLTotal).Name = "LBLTotal";
		((Control)LBLTotal).Size = new Size(74, 19);
		((Control)LBLTotal).TabIndex = 16;
		((Control)LBLTotal).Text = "0";
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX3).Location = new Point(72, 10);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).RightToLeft = (RightToLeft)1;
		((Control)LabelX3).Size = new Size(65, 19);
		((Control)LabelX3).TabIndex = 14;
		((Control)LabelX3).Text = "کل پیام ها :";
		((Control)TextBoxX1).BackColor = Color.LightSteelBlue;
		TextBoxX1.Border.Class = "TextBoxBorder";
		TextBoxX1.Border.CornerType = eCornerType.Square;
		((Control)TextBoxX1).DataBindings.Add(new Binding("Text", (object)SMSOutBoxBindingSource, "Message", true));
		((Control)TextBoxX1).Dock = (DockStyle)5;
		((Control)TextBoxX1).Location = new Point(0, 0);
		((TextBoxBase)TextBoxX1).Multiline = true;
		((Control)TextBoxX1).Name = "TextBoxX1";
		((TextBoxBase)TextBoxX1).ReadOnly = true;
		((TextBox)TextBoxX1).ScrollBars = (ScrollBars)3;
		((Control)TextBoxX1).Size = new Size(229, 46);
		((Control)TextBoxX1).TabIndex = 24;
		SMSOutBoxBindingSource.DataSource = typeof(SmsOutBox);
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTNExit.Image = (Image)componentResourceManager.GetObject("BTNExit.Image");
		((Control)BTNExit).Location = new Point(3, 314);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(30, 28);
		((Control)BTNExit).TabIndex = 23;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "خروج از صفحه";
		((Control)BTNExit).Click += BTNExit_Click;
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
			DGCheck,
			(DataGridViewColumn)schadulingMDataGridViewTextBoxColumn,
			(DataGridViewColumn)schadulingShDataGridViewTextBoxColumn,
			(DataGridViewColumn)priorityDataGridViewTextBoxColumn,
			(DataGridViewColumn)deletedItemsIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)draftIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)sentBoxIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryDataGridViewCheckBoxColumn,
			(DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)rstr4DataGridViewTextBoxColumn,
			(DataGridViewColumn)rstr5DataGridViewTextBoxColumn,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGNumber,
			(DataGridViewColumn)refrencedIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)accountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGMessage,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)customerIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn3,
			(DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)sTDataGridViewCheckBoxColumn,
			(DataGridViewColumn)schadulingTypeDataGridViewTextBoxColumn
		});
		((Control)DGrid).ContextMenuStrip = ContextMenue;
		((DataGridView)DGrid).DataSource = SMSOutBoxBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).EditMode = (DataGridViewEditMode)4;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 97);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(414, 158);
		((Control)DGrid).TabIndex = 22;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((DataGridView)DGrid).CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		DGCheck.Checked = true;
		DGCheck.CheckState = (CheckState)2;
		DGCheck.CheckValue = "N";
		((DataGridViewColumn)DGCheck).DataPropertyName = "SchadulingM";
		((DataGridViewColumn)DGCheck).HeaderText = "انتخاب";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).DataPropertyName = "SchadulingM";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).HeaderText = "SchadulingM";
		((DataGridViewColumn)schadulingMDataGridViewTextBoxColumn).Name = "schadulingMDataGridViewTextBoxColumn";
		((DataGridViewColumn)schadulingShDataGridViewTextBoxColumn).DataPropertyName = "SchadulingSh";
		((DataGridViewColumn)schadulingShDataGridViewTextBoxColumn).HeaderText = "SchadulingSh";
		((DataGridViewColumn)schadulingShDataGridViewTextBoxColumn).Name = "schadulingShDataGridViewTextBoxColumn";
		((DataGridViewBand)schadulingShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)priorityDataGridViewTextBoxColumn).DataPropertyName = "Priority";
		((DataGridViewColumn)priorityDataGridViewTextBoxColumn).HeaderText = "Priority";
		((DataGridViewColumn)priorityDataGridViewTextBoxColumn).Name = "priorityDataGridViewTextBoxColumn";
		((DataGridViewBand)priorityDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deletedItemsIdDataGridViewTextBoxColumn).DataPropertyName = "DeletedItemsId";
		((DataGridViewColumn)deletedItemsIdDataGridViewTextBoxColumn).HeaderText = "DeletedItemsId";
		((DataGridViewColumn)deletedItemsIdDataGridViewTextBoxColumn).Name = "deletedItemsIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)draftIdDataGridViewTextBoxColumn).DataPropertyName = "DraftId";
		((DataGridViewColumn)draftIdDataGridViewTextBoxColumn).HeaderText = "DraftId";
		((DataGridViewColumn)draftIdDataGridViewTextBoxColumn).Name = "draftIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)sentBoxIdDataGridViewTextBoxColumn).DataPropertyName = "SentBoxId";
		((DataGridViewColumn)sentBoxIdDataGridViewTextBoxColumn).HeaderText = "SentBoxId";
		((DataGridViewColumn)sentBoxIdDataGridViewTextBoxColumn).Name = "sentBoxIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).DataPropertyName = "Delivery";
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).HeaderText = "Delivery";
		((DataGridViewColumn)deliveryDataGridViewCheckBoxColumn).Name = "deliveryDataGridViewCheckBoxColumn";
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).DataPropertyName = "RefrenceAccountId";
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).HeaderText = "RefrenceAccountId";
		((DataGridViewColumn)refrenceAccountIdDataGridViewTextBoxColumn).Name = "refrenceAccountIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)rstr4DataGridViewTextBoxColumn).DataPropertyName = "Rstr4";
		((DataGridViewColumn)rstr4DataGridViewTextBoxColumn).HeaderText = "Rstr4";
		((DataGridViewColumn)rstr4DataGridViewTextBoxColumn).Name = "rstr4DataGridViewTextBoxColumn";
		((DataGridViewColumn)rstr5DataGridViewTextBoxColumn).DataPropertyName = "Rstr5";
		((DataGridViewColumn)rstr5DataGridViewTextBoxColumn).HeaderText = "Rstr5";
		((DataGridViewColumn)rstr5DataGridViewTextBoxColumn).Name = "rstr5DataGridViewTextBoxColumn";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewColumn)DGNumber).DataPropertyName = "Number";
		((DataGridViewColumn)DGNumber).HeaderText = "شماره";
		((DataGridViewColumn)DGNumber).Name = "DGNumber";
		((DataGridViewColumn)refrencedIdDataGridViewTextBoxColumn).DataPropertyName = "RefrencedId";
		((DataGridViewColumn)refrencedIdDataGridViewTextBoxColumn).HeaderText = "RefrencedId";
		((DataGridViewColumn)refrencedIdDataGridViewTextBoxColumn).Name = "refrencedIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)accountIdDataGridViewTextBoxColumn).DataPropertyName = "AccountId";
		((DataGridViewColumn)accountIdDataGridViewTextBoxColumn).HeaderText = "AccountId";
		((DataGridViewColumn)accountIdDataGridViewTextBoxColumn).Name = "accountIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewColumn)DGMessage).DataPropertyName = "Message";
		((DataGridViewColumn)DGMessage).HeaderText = "Message";
		((DataGridViewColumn)DGMessage).Name = "DGMessage";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).DataPropertyName = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).HeaderText = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).Name = "customerIdDataGridViewTextBoxColumn";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "CustomerName";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "CustomerName";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).ReadOnly = true;
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).HeaderText = "[متن پیام]";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).Name = "fixedMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)fixedMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).DataPropertyName = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).HeaderText = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).Name = "sTDataGridViewCheckBoxColumn";
		((ToolStrip)ContextMenue).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)انتخابToolStripMenuItem });
		((Control)ContextMenue).Name = "ContextMenue";
		((Control)ContextMenue).RightToLeft = (RightToLeft)1;
		((Control)ContextMenue).Size = new Size(107, 26);
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)CMASelectAll,
			(ToolStripItem)CMASelectNone,
			(ToolStripItem)CMASelectReverce
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(106, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)CMASelectAll).Name = "CMASelectAll";
		((ToolStripItem)CMASelectAll).Size = new Size(176, 22);
		((ToolStripItem)CMASelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)CMASelectAll).Click += CMASelectAll_Click;
		((ToolStripItem)CMASelectNone).Name = "CMASelectNone";
		((ToolStripItem)CMASelectNone).Size = new Size(176, 22);
		((ToolStripItem)CMASelectNone).Text = "[انتخاب هیچکدام]";
		((ToolStripItem)CMASelectNone).Click += CMASelectNone_Click;
		((ToolStripItem)CMASelectReverce).Name = "CMASelectReverce";
		((ToolStripItem)CMASelectReverce).Size = new Size(176, 22);
		((ToolStripItem)CMASelectReverce).Text = "[معکوس کردن انتخاب]";
		((ToolStripItem)CMASelectReverce).Click += CMASelectReverce_Click;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((Control)GroupPanel1).BackColor = Color.Transparent;
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)BTNPrint);
		((Control)GroupPanel1).Controls.Add((Control)(object)CMBDates);
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TXTSearch);
		((Control)GroupPanel1).Controls.Add((Control)(object)LblSearch_Outbox);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 3);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(414, 88);
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
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 21;
		((Control)GroupPanel1).Text = "پیام های در حال ارسال";
		((Control)BTNPrint).AccessibleRole = (AccessibleRole)43;
		BTNPrint.ColorTable = eButtonColor.Office2007WithBackground;
		BTNPrint.Image = (Image)componentResourceManager.GetObject("BTNPrint.Image");
		BTNPrint.ImagePosition = eImagePosition.Right;
		((Control)BTNPrint).Location = new Point(3, 3);
		((Control)BTNPrint).Name = "BTNPrint";
		((Control)BTNPrint).Size = new Size(84, 44);
		BTNPrint.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNPrint).TabIndex = 21;
		((Control)BTNPrint).Text = "           چـــاپ";
		((Control)BTNPrint).Click += BTNPrint_Click;
		((Control)CMBDates).Anchor = (AnchorStyles)9;
		((ListControl)CMBDates).DisplayMember = "DateSH";
		((ComboBox)CMBDates).DrawMode = (DrawMode)1;
		((ComboBox)CMBDates).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBDates).FormattingEnabled = true;
		((ComboBox)CMBDates).ItemHeight = 15;
		((Control)CMBDates).Location = new Point(207, 27);
		((Control)CMBDates).Name = "CMBDates";
		((Control)CMBDates).Size = new Size(148, 21);
		((Control)CMBDates).TabIndex = 10;
		((ListControl)CMBDates).ValueMember = "DateSH";
		((ListControl)CMBDates).SelectedValueChanged += CMBDates_SelectedValueChanged;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Location = new Point(361, 25);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(57, 22);
		((Control)LabelX1).TabIndex = 8;
		((Control)LabelX1).Text = "تاریخ :";
		LabelX1.TextAlign = (ContentAlignment)64;
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		TXTSearch.Border.Class = "TextBoxBorder";
		TXTSearch.Border.CornerType = eCornerType.Square;
		((Control)TXTSearch).Location = new Point(93, 3);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(262, 20);
		((Control)TXTSearch).TabIndex = 1;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((Control)TXTSearch).Enter += TXTSearch_GotFocus;
		((Control)TXTSearch).Leave += TXTSearch_LostFocus;
		((Control)LblSearch_Outbox).Anchor = (AnchorStyles)9;
		((Control)LblSearch_Outbox).Location = new Point(361, 1);
		((Control)LblSearch_Outbox).Name = "LblSearch_Outbox";
		((Control)LblSearch_Outbox).Size = new Size(57, 25);
		((Control)LblSearch_Outbox).TabIndex = 7;
		((Control)LblSearch_Outbox).Text = "جستجو :";
		LblSearch_Outbox.TextAlign = (ContentAlignment)64;
		((Control)BTNAddPeople).AccessibleRole = (AccessibleRole)43;
		((Control)BTNAddPeople).Anchor = (AnchorStyles)6;
		BTNAddPeople.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNAddPeople).Enabled = false;
		BTNAddPeople.Image = (Image)componentResourceManager.GetObject("BTNAddPeople.Image");
		((Control)BTNAddPeople).Location = new Point(75, 314);
		((Control)BTNAddPeople).Name = "BTNAddPeople";
		((Control)BTNAddPeople).Size = new Size(30, 28);
		((Control)BTNAddPeople).TabIndex = 17;
		((Control)BTNAddPeople).TabStop = false;
		BTNAddPeople.Tooltip = "اضافه کردن گیرنده به لیست مخاطبین";
		((Control)BTNAddPeople).Click += BTNAddPeople_Click;
		((Control)BTNBlackList).AccessibleRole = (AccessibleRole)43;
		((Control)BTNBlackList).Anchor = (AnchorStyles)6;
		((Control)BTNBlackList).BackColor = Color.Gainsboro;
		BTNBlackList.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNBlackList).Enabled = false;
		BTNBlackList.Image = (Image)componentResourceManager.GetObject("BTNBlackList.Image");
		((Control)BTNBlackList).Location = new Point(39, 314);
		((Control)BTNBlackList).Name = "BTNBlackList";
		((Control)BTNBlackList).Size = new Size(30, 28);
		((Control)BTNBlackList).TabIndex = 16;
		((Control)BTNBlackList).TabStop = false;
		BTNBlackList.Tooltip = "حذف گیرنده از لیست مخاطبین";
		((Control)BTNBlackList).Click += BTNBlackList_Click;
		((Control)BTNForward).AccessibleRole = (AccessibleRole)43;
		((Control)BTNForward).Anchor = (AnchorStyles)10;
		BTNForward.ColorTable = eButtonColor.Office2007WithBackground;
		BTNForward.Image = (Image)componentResourceManager.GetObject("BTNForward.Image");
		((Control)BTNForward).Location = new Point(387, 314);
		((Control)BTNForward).Name = "BTNForward";
		((Control)BTNForward).Size = new Size(30, 28);
		((Control)BTNForward).TabIndex = 18;
		((Control)BTNForward).TabStop = false;
		BTNForward.Tooltip = "ارسال پیام به دیگری";
		((Control)BTNForward).Click += BTNForward_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BTNDelete).Anchor = (AnchorStyles)10;
		BTNDelete.ColorTable = eButtonColor.Office2007WithBackground;
		BTNDelete.Image = (Image)componentResourceManager.GetObject("BTNDelete.Image");
		((Control)BTNDelete).Location = new Point(351, 315);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).Size = new Size(30, 28);
		((Control)BTNDelete).TabIndex = 14;
		((Control)BTNDelete).TabStop = false;
		BTNDelete.Tooltip = "حذف پیام ارسالی";
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)Panel1);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel2);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNAddPeople);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNForward);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNBlackList);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(420, 346);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 2;
		((Control)Panel1).Anchor = (AnchorStyles)14;
		Panel1.BorderStyle = (BorderStyle)1;
		((Control)Panel1).Controls.Add((Control)(object)TextBoxX1);
		((Control)Panel1).Location = new Point(3, 261);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(231, 48);
		((Control)Panel1).TabIndex = 26;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).DataPropertyName = "SchadulingType";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).HeaderText = "SchadulingType";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Name = "dataGridViewTextBoxColumn4";
		((DataGridViewColumn)schadulingTypeDataGridViewTextBoxColumn).DataPropertyName = "SchadulingType";
		((DataGridViewColumn)schadulingTypeDataGridViewTextBoxColumn).HeaderText = "SchadulingType";
		((DataGridViewColumn)schadulingTypeDataGridViewTextBoxColumn).Name = "schadulingTypeDataGridViewTextBoxColumn";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "UCOutBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(420, 346);
		((Control)GroupPanel2).ResumeLayout(false);
		((ISupportInitialize)SMSOutBoxBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)ContextMenue).ResumeLayout(false);
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
