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
using FormCustom;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using PayamakSettings;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;
using PayamakUi.Mapper;
using PayamakUi.Properties;

namespace PayamakUi.UC.UCSMS;

public class UCsentList : UserControl, IHasContext
{
	public delegate void Exit_UserControlEventHandler();

	private List<SmsSent> LstsmsSend = new List<SmsSent>();

	private string ST = "";

	private string DateSH1 = "";

	private string DateSH2 = "";

	private string TextM = "";

	private string NUMFE = "";

	private string NumGI = "";

	private CancellationTokenSource TokenSource = new CancellationTokenSource();

	private CancellationTokenSource TokenDelete = new CancellationTokenSource();

	private PayamakClientHubInstances _service;

	private IContainer components;

	internal ButtonX BTNAdd;

	internal ButtonX BTNBlackList;

	internal ButtonX BTNForward;

	internal ButtonX BTNDelete;

	internal DataGridViewX DGrid;

	internal BindingSource SMSSentBindingSource;

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

	internal Label LblUnKnown;

	internal Label LblNotDeliverd;

	internal Label LblDelivered;

	internal PanelEx PanelEx1;

	internal Panel Panel1;

	internal ButtonX BTNPrint;

	internal PictureBox PictureBox1;

	internal PictureBox PictureBox6;

	internal PictureBox PictureBox5;

	internal PictureBox PictureBox4;

	internal PictureBox PictureBox3;

	internal PictureBox PictureBox2;

	internal DataGridViewImageColumn DataGridViewImageColumn1;

	internal Label LabelX10;

	internal Label LabelX9;

	internal Label LabelX8;

	internal Label LabelX7;

	internal Label LabelX6;

	internal Label LabelX5;

	internal Label LblBlackList;

	internal Label LblNaresideM;

	internal Label LblResideM;

	internal ExpandablePanel EPSearch;

	internal CheckBox CHBToDate;

	internal DateBoxFa Date1;

	internal DateBoxFa Date2;

	internal CheckBox ChbFromdate;

	internal TextBoxX TxtNumG;

	internal TextBoxX TxtNumF;

	internal Label LabelX12;

	internal Label LabelX11;

	internal ComboBoxEx CMBST;

	internal ComboItem CmbResideG;

	internal ComboItem CmbResidem;

	internal ComboItem CmbNM;

	internal ComboItem CmbUnknown;

	internal ComboItem CmbNG;

	internal ComboItem CmbBlack;

	internal Label LX1_Sent;

	internal TextBoxX TXTSearch;

	internal CheckBox chbST;

	internal PictureBox pictureBox7;

	internal ButtonX BTNSelectAll;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DGDeliverySTShort;

	private DataGridViewTextBoxColumn DGSendDateM;

	private DataGridViewTextBoxColumn DGSendDateSh;

	private DataGridViewTextBoxColumn DGSendTime;

	private DataGridViewTextBoxColumn DGDeliverySt;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn DGNumber;

	private DataGridViewCheckBoxColumn DGDeliveryRequested;

	private DataGridViewTextBoxColumn refrencedIdsDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn refrenseAccountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn refrencedIDDataGridViewTextBoxColumn;

	private DataGridViewImageColumn imgDataGridViewImageColumn;

	private DataGridViewCheckBoxColumn isCheckedDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGMessage;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fixedMessageDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn sTDataGridViewCheckBoxColumn;

	public string ContextGuid { get; set; }

	public event Exit_UserControlEventHandler Exit_UserControl;

	public UCsentList()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 46);
		}
	}

	public async Task LoadDataAsync()
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			if (!TokenSource.IsCancellationRequested)
			{
				_service = PayamakClientHubInstances.GetInstance(ContextGuid);
				Font font = await Font.GetFontProgramAsync(TokenSource.Token);
				ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await _service.SmsSent.GetAllAsync();
				LstsmsSend = await SmsSentMapper.Instance.MapListAsync(ContextGuid, returnedSaveFuncInfoWithValue.value);
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)DGrid).Focus();
					((Control)DGrid).Font = font;
					Date1.MiladiDate = DateTime.Now;
					Date2.MiladiDate = DateTime.Now;
					EPSearch.Expanded = true;
					((ListControl)CMBST).SelectedIndex = 0;
					DateSH1 = Date1.MiladiDate.MiladiToCalture();
					DateSH2 = Date2.MiladiDate.MiladiToCalture();
					ChbFromdate.Checked = true;
					CHBToDate.Checked = true;
				});
				if (!TokenSource.IsCancellationRequested)
				{
					Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 79);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentCell.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				((DataGridView)DGrid).CurrentCell = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index];
			}
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
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value?.ToString()?.FixString()?.Trim() ?? "";
				((Control)TextBoxX1).Text = text;
			}
			else
			{
				((Control)TextBoxX1).Text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 121);
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
			if ((int)e.KeyCode == 32)
			{
				if (((DataGridView)DGrid).RowCount == 0)
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
			if (((DataGridView)DGrid).RowCount != 0)
			{
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()?.FixString()?.Trim();
				((Control)TextBoxX1).Text = text;
			}
			else
			{
				((Control)TextBoxX1).Text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 166);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 180);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 194);
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
			WebErrorLog.Instance.StartLog(ex, "", "CMASelectReverce_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 217);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
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
					if (((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value != null && bool.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value.ToString()))
					{
						list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
					}
				}
				TokenDelete?.Cancel();
				TokenDelete = new CancellationTokenSource();
				CancellationToken token = TokenDelete.Token;
				await _service.SmsSent.DeleteListAsync(list, token);
			}
			Task.Run(() => LoadDataAsync());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 250);
		}
	}

	private void BTNForward_Click(object sender, EventArgs e)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!((Control)BTNForward).Enabled || !((Control)BTNForward).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount != 0)
			{
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()?.FixString()?.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					((Form)new FRMSend(ContextGuid)
					{
						Message = text
					}).ShowDialog((IWin32Window)(object)this);
				}
				RefreshLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNForward_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 275);
		}
	}

	private void BTNAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BTNAdd).Visible && ((Control)BTNAdd).Enabled)
			{
				((Control)DGrid).Focus();
				RefreshLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 290);
		}
	}

	private void BTNBlackList_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNBlackList_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 303);
		}
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount != 0 && e.RowIndex >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				if (((DataGridView)DGrid).RowCount != 0)
				{
					string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value?.ToString()?.FixString()?.Trim() ?? "";
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
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 329);
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
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 342);
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
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 354);
		}
	}

	private void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string textM = ((Control)TXTSearch).Text?.FixString()?.Trim();
			TextM = textM;
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 371);
		}
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		try
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				val.Close();
				((Control)this).Hide();
				((Control)this).Visible = false;
				this.Exit_UserControl?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 391);
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
			if (((DataGridView)DGrid).RowCount > 0)
			{
				int i = 0;
				for (int num7 = ((DataGridView)DGrid).RowCount - 1; i <= num7; i++)
				{
					switch (((DataGridView)DGrid)[((DataGridViewBand)DGDeliverySTShort).Index, i].Value.ToString())
					{
					case "42":
						num++;
						break;
					case "41":
						num3++;
						break;
					case "40":
						num2++;
						break;
					case "43":
						num4++;
						break;
					case "44":
						num6++;
						break;
					case "45":
						num5++;
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RefreshLables", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 446);
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
			string text = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()?.FixString()?.Trim();
			((Control)TextBoxX1).Text = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 461);
		}
	}

	private void BTNPrint_Click(object sender, EventArgs e)
	{
	}

	private void EPSearch_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
	{
		try
		{
			if (EPSearch.Expanded)
			{
				((Control)EPSearch).Height = 139;
				((Control)DGrid).Top = ((Control)EPSearch).Bottom + 5;
				((Control)DGrid).Height = ((Control)DGrid).Height - 114;
				((Control)EPSearch).Width = ((Control)DGrid).Width;
				((Control)EPSearch).Left = ((Control)DGrid).Left;
				((Control)TextBoxX1).Top = ((Control)DGrid).Bottom + 5;
				((Control)GroupPanel2).Height = ((Control)TextBoxX1).Height;
			}
			else
			{
				((Control)EPSearch).Height = 26;
				((Control)DGrid).Top = ((Control)EPSearch).Bottom + 5;
				((Control)DGrid).Height = ((Control)DGrid).Height + 114;
				((Control)EPSearch).Width = ((Control)DGrid).Width;
				((Control)EPSearch).Left = ((Control)DGrid).Left;
				((Control)TextBoxX1).Top = ((Control)DGrid).Bottom + 5;
				((Control)GroupPanel2).Height = ((Control)TextBoxX1).Height;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EPSearch_ExpandedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 508);
		}
	}

	private void Search(string Date1, string Date2, string ST, string text, string NumF, string NumG, CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			IEnumerable<SmsSent> TempLstSend = new List<SmsSent>();
			TempLstSend = LstsmsSend;
			if (!string.IsNullOrEmpty(ST))
			{
				TempLstSend = TempLstSend.Where((SmsSent p) => p.DeliverySTShort == short.Parse(ST));
			}
			if (!string.IsNullOrEmpty(text))
			{
				TempLstSend = TempLstSend.Where((SmsSent p) => p.Message.Contains(text));
			}
			if (!string.IsNullOrEmpty(NumF))
			{
				TempLstSend = TempLstSend.Where((SmsSent p) => p.Number.Contains(NumF));
			}
			if (!string.IsNullOrEmpty(NumG))
			{
				TempLstSend = TempLstSend.Where((SmsSent p) => p.SenderNumber.Contains(NumG));
			}
			if (!string.IsNullOrEmpty(Date1))
			{
				if (!string.IsNullOrEmpty(Date2))
				{
					TempLstSend = TempLstSend?.Where((SmsSent p) => p.SendDateM > Date1.ShamsiToMiladi());
				}
				else
				{
					TempLstSend = TempLstSend?.Where((SmsSent p) => p.SendDateM > Date1.ShamsiToMiladi() && p.SendDateM < Date2.ShamsiToMiladi());
				}
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				SMSSentBindingSource.DataSource = TempLstSend?.ToList() ?? new List<SmsSent>();
				if (((DataGridView)DGrid).RowCount != 0)
				{
					string text2 = ((DataGridView)DGrid)[((DataGridViewBand)DGMessage).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value?.ToString()?.FixString()?.Trim() ?? "";
					((Control)TextBoxX1).Text = text2;
				}
				RefreshLables();
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 551);
		}
	}

	private void CMBST_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			if (((Control)CMBST).Enabled)
			{
				STCMB();
			}
			else
			{
				ST = "";
			}
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
			RefreshLables();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMBST_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 575);
		}
	}

	private void chbST_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			if (chbST.Checked)
			{
				((Control)CMBST).Enabled = true;
				STCMB();
				Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
				RefreshLables();
			}
			else
			{
				((Control)CMBST).Enabled = false;
				ST = "";
				Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
				RefreshLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "chbST_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 602);
		}
	}

	private void ChbFromdate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (ChbFromdate.Checked)
			{
				((Control)Date1).Enabled = true;
				Filter("");
			}
			else
			{
				((Control)Date1).Enabled = false;
				DateSH1 = "";
				Filter("");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChbFromdate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 624);
		}
	}

	private void CHBToDate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CHBToDate.Checked)
			{
				((Control)Date2).Enabled = true;
				Filter("");
			}
			else
			{
				((Control)Date2).Enabled = false;
				DateSH2 = "";
				Filter("");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CHBToDate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 646);
		}
	}

	private void TxtNumF_GotFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtNumF).BackColor = Color.Yellow;
			((TextBoxBase)TxtNumF).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumF_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 659);
		}
	}

	private void TxtNumF_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			string nUMFE = ((Control)TxtNumF).Text?.FixString()?.Trim();
			NUMFE = nUMFE;
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumF_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 675);
		}
	}

	private void TxtNumG_GotFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtNumG).BackColor = Color.Yellow;
			((TextBoxBase)TxtNumG).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumG_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 688);
		}
	}

	private void TxtNumG_LostFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtNumG).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumG_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 700);
		}
	}

	private void TxtNumG_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			string numGI = ((Control)TxtNumG).Text?.FixString()?.Trim();
			NumGI = numGI;
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumG_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 716);
		}
	}

	private void Filter(string filter)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			if (ChbFromdate.Checked & !CHBToDate.Checked)
			{
				DateSH1 = ((Control)Date1).Text;
			}
			if (CHBToDate.Checked & !ChbFromdate.Checked)
			{
				DateSH2 = ((Control)Date2).Text;
			}
			if (ChbFromdate.Checked & CHBToDate.Checked)
			{
				DateSH1 = ((Control)Date1).Text;
				DateSH2 = ((Control)Date2).Text;
			}
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Filter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 747);
		}
	}

	private void TxtNumF_LostFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtNumF).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumF_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 759);
		}
	}

	private void Date1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Filter("");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Date1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 771);
		}
	}

	private void Date2_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Filter("");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Date2_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 781);
		}
	}

	private void STCMB()
	{
		try
		{
			if (((ListControl)CMBST).SelectedIndex == 0)
			{
				ST = "41";
				return;
			}
			if (((ListControl)CMBST).SelectedIndex == 4)
			{
				ST = "40";
				return;
			}
			if (((ListControl)CMBST).SelectedIndex == 3)
			{
				ST = "42";
				return;
			}
			if (((ListControl)CMBST).SelectedIndex == 1)
			{
				ST = "43";
				return;
			}
			if (((ListControl)CMBST).SelectedIndex == 2)
			{
				ST = "45";
				return;
			}
			if (((ListControl)CMBST).SelectedIndex == 5)
			{
				ST = "44";
				return;
			}
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "STCMB", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 803);
		}
	}

	private void TextBoxX1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TokenSource?.Cancel();
			TokenSource = new CancellationTokenSource();
			Search(DateSH1, DateSH2, ST, TextM, NUMFE, NumGI, TokenSource.Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBoxX1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 814);
		}
	}

	private void BTNSelectAll_Click_1(object sender, EventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "BTNSelectAll_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCsentList.cs", 846);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
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
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Expected O, but got Unknown
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Expected O, but got Unknown
		//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb1: Expected O, but got Unknown
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Expected O, but got Unknown
		//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df6: Expected O, but got Unknown
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7e: Expected O, but got Unknown
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Expected O, but got Unknown
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8d: Expected O, but got Unknown
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1015: Expected O, but got Unknown
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_121f: Expected O, but got Unknown
		//IL_14c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d3: Expected O, but got Unknown
		//IL_15b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ba: Expected O, but got Unknown
		//IL_15c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d1: Expected O, but got Unknown
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e8: Expected O, but got Unknown
		//IL_15f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ff: Expected O, but got Unknown
		//IL_24a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ab: Expected O, but got Unknown
		//IL_24d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e1: Expected O, but got Unknown
		//IL_2674: Unknown result type (might be due to invalid IL or missing references)
		//IL_267e: Expected O, but got Unknown
		//IL_26aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b4: Expected O, but got Unknown
		//IL_2e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e52: Expected O, but got Unknown
		//IL_2f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1c: Expected O, but got Unknown
		//IL_308b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3095: Expected O, but got Unknown
		//IL_3146: Unknown result type (might be due to invalid IL or missing references)
		//IL_3150: Expected O, but got Unknown
		//IL_321f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3229: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UCsentList));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		GroupPanel2 = new GroupPanel();
		pictureBox7 = new PictureBox();
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
		TextBoxX1 = new TextBoxX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGDeliverySTShort = new DataGridViewTextBoxColumn();
		DGSendDateM = new DataGridViewTextBoxColumn();
		DGSendDateSh = new DataGridViewTextBoxColumn();
		DGSendTime = new DataGridViewTextBoxColumn();
		DGDeliverySt = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		DGNumber = new DataGridViewTextBoxColumn();
		DGDeliveryRequested = new DataGridViewCheckBoxColumn();
		refrencedIdsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		refrenseAccountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		refrencedIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		imgDataGridViewImageColumn = new DataGridViewImageColumn();
		isCheckedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		DGMessage = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		customerIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fixedMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sTDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		ContextMenue = new ContextMenuStrip(components);
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		CMASelectAll = new ToolStripMenuItem();
		CMASelectNone = new ToolStripMenuItem();
		CMASelectReverce = new ToolStripMenuItem();
		SMSSentBindingSource = new BindingSource(components);
		PanelEx1 = new PanelEx();
		BTNSelectAll = new ButtonX();
		EPSearch = new ExpandablePanel();
		chbST = new CheckBox();
		CHBToDate = new CheckBox();
		Date1 = new DateBoxFa();
		Date2 = new DateBoxFa();
		ChbFromdate = new CheckBox();
		TxtNumG = new TextBoxX();
		TxtNumF = new TextBoxX();
		LabelX12 = new Label();
		LabelX11 = new Label();
		CMBST = new ComboBoxEx();
		CmbResideG = new ComboItem();
		CmbResidem = new ComboItem();
		CmbNM = new ComboItem();
		CmbUnknown = new ComboItem();
		CmbNG = new ComboItem();
		CmbBlack = new ComboItem();
		LX1_Sent = new Label();
		TXTSearch = new TextBoxX();
		BTNPrint = new ButtonX();
		BTNExit = new ButtonX();
		BTNDelete = new ButtonX();
		BTNAdd = new ButtonX();
		BTNForward = new ButtonX();
		BTNBlackList = new ButtonX();
		Panel1 = new Panel();
		DataGridViewImageColumn1 = new DataGridViewImageColumn();
		((Control)GroupPanel2).SuspendLayout();
		((ISupportInitialize)pictureBox7).BeginInit();
		((ISupportInitialize)PictureBox6).BeginInit();
		((ISupportInitialize)PictureBox5).BeginInit();
		((ISupportInitialize)PictureBox4).BeginInit();
		((ISupportInitialize)PictureBox3).BeginInit();
		((ISupportInitialize)PictureBox2).BeginInit();
		((ISupportInitialize)PictureBox1).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)ContextMenue).SuspendLayout();
		((ISupportInitialize)SMSSentBindingSource).BeginInit();
		((Control)PanelEx1).SuspendLayout();
		((Control)EPSearch).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)GroupPanel2).Anchor = (AnchorStyles)10;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)pictureBox7);
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
		((Control)GroupPanel2).Location = new Point(303, 405);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(456, 98);
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
		((Control)GroupPanel2).TabIndex = 1;
		((Control)pictureBox7).Anchor = (AnchorStyles)9;
		pictureBox7.Image = (Image)(object)Resources.letter4;
		((Control)pictureBox7).Location = new Point(417, -3);
		((Control)pictureBox7).Name = "pictureBox7";
		((Control)pictureBox7).Size = new Size(24, 24);
		pictureBox7.SizeMode = (PictureBoxSizeMode)4;
		pictureBox7.TabIndex = 29;
		pictureBox7.TabStop = false;
		((Control)LabelX10).Anchor = (AnchorStyles)9;
		((Control)LabelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX10).Location = new Point(83, 45);
		((Control)LabelX10).Name = "LabelX10";
		((Control)LabelX10).RightToLeft = (RightToLeft)1;
		((Control)LabelX10).Size = new Size(97, 24);
		((Control)LabelX10).TabIndex = 12;
		((Control)LabelX10).Text = "[لیست سیاه] :";
		LabelX10.TextAlign = (ContentAlignment)16;
		((Control)LabelX9).Anchor = (AnchorStyles)9;
		((Control)LabelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX9).Location = new Point(43, 19);
		((Control)LabelX9).Name = "LabelX9";
		((Control)LabelX9).RightToLeft = (RightToLeft)1;
		((Control)LabelX9).Size = new Size(137, 24);
		((Control)LabelX9).TabIndex = 10;
		((Control)LabelX9).Text = "[نرسیده به مخابرات] :";
		LabelX9.TextAlign = (ContentAlignment)16;
		((Control)LabelX8).Anchor = (AnchorStyles)9;
		((Control)LabelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX8).Location = new Point(54, -3);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).RightToLeft = (RightToLeft)1;
		((Control)LabelX8).Size = new Size(126, 24);
		((Control)LabelX8).TabIndex = 8;
		((Control)LabelX8).Text = "[رسیده به مخابرات] :";
		LabelX8.TextAlign = (ContentAlignment)16;
		((Control)LabelX7).Anchor = (AnchorStyles)9;
		((Control)LabelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX7).Location = new Point(338, 66);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).RightToLeft = (RightToLeft)1;
		((Control)LabelX7).Size = new Size(79, 24);
		((Control)LabelX7).TabIndex = 6;
		((Control)LabelX7).Text = "[نامشخص] :";
		LabelX7.TextAlign = (ContentAlignment)16;
		((Control)LabelX6).Anchor = (AnchorStyles)9;
		((Control)LabelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX6).Location = new Point(289, 42);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).RightToLeft = (RightToLeft)1;
		((Control)LabelX6).Size = new Size(128, 24);
		((Control)LabelX6).TabIndex = 4;
		((Control)LabelX6).Text = "[نرسیده به گوشی] :";
		LabelX6.TextAlign = (ContentAlignment)16;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((Control)LabelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX5).Location = new Point(289, 18);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).RightToLeft = (RightToLeft)1;
		((Control)LabelX5).Size = new Size(128, 24);
		((Control)LabelX5).TabIndex = 2;
		((Control)LabelX5).Text = "[رسیده به گوشی] :";
		LabelX5.TextAlign = (ContentAlignment)16;
		((Control)LblBlackList).Anchor = (AnchorStyles)9;
		((Control)LblBlackList).Location = new Point(4, 51);
		((Control)LblBlackList).Name = "LblBlackList";
		((Control)LblBlackList).Size = new Size(33, 24);
		((Control)LblBlackList).TabIndex = 13;
		((Control)LblBlackList).Text = "0";
		((Control)LblNaresideM).Anchor = (AnchorStyles)9;
		((Control)LblNaresideM).Location = new Point(3, 25);
		((Control)LblNaresideM).Name = "LblNaresideM";
		((Control)LblNaresideM).Size = new Size(34, 24);
		((Control)LblNaresideM).TabIndex = 11;
		((Control)LblNaresideM).Text = "0";
		((Control)LblResideM).Anchor = (AnchorStyles)9;
		((Control)LblResideM).Location = new Point(3, 3);
		((Control)LblResideM).Name = "LblResideM";
		((Control)LblResideM).Size = new Size(34, 24);
		((Control)LblResideM).TabIndex = 9;
		((Control)LblResideM).Text = "0";
		((Control)PictureBox6).Anchor = (AnchorStyles)9;
		PictureBox6.Image = (Image)componentResourceManager.GetObject("PictureBox6.Image");
		((Control)PictureBox6).Location = new Point(181, 46);
		((Control)PictureBox6).Name = "PictureBox6";
		((Control)PictureBox6).Size = new Size(24, 24);
		PictureBox6.SizeMode = (PictureBoxSizeMode)4;
		PictureBox6.TabIndex = 28;
		PictureBox6.TabStop = false;
		((Control)PictureBox5).Anchor = (AnchorStyles)9;
		PictureBox5.Image = (Image)componentResourceManager.GetObject("PictureBox5.Image");
		((Control)PictureBox5).Location = new Point(181, 24);
		((Control)PictureBox5).Name = "PictureBox5";
		((Control)PictureBox5).Size = new Size(24, 24);
		PictureBox5.SizeMode = (PictureBoxSizeMode)4;
		PictureBox5.TabIndex = 27;
		PictureBox5.TabStop = false;
		((Control)PictureBox4).Anchor = (AnchorStyles)9;
		PictureBox4.Image = (Image)componentResourceManager.GetObject("PictureBox4.Image");
		((Control)PictureBox4).Location = new Point(181, -1);
		((Control)PictureBox4).Name = "PictureBox4";
		((Control)PictureBox4).Size = new Size(24, 24);
		PictureBox4.SizeMode = (PictureBoxSizeMode)4;
		PictureBox4.TabIndex = 26;
		PictureBox4.TabStop = false;
		((Control)PictureBox3).Anchor = (AnchorStyles)9;
		PictureBox3.Image = (Image)componentResourceManager.GetObject("PictureBox3.Image");
		((Control)PictureBox3).Location = new Point(417, 68);
		((Control)PictureBox3).Name = "PictureBox3";
		((Control)PictureBox3).Size = new Size(24, 24);
		PictureBox3.SizeMode = (PictureBoxSizeMode)4;
		PictureBox3.TabIndex = 25;
		PictureBox3.TabStop = false;
		((Control)PictureBox2).Anchor = (AnchorStyles)9;
		PictureBox2.Image = (Image)componentResourceManager.GetObject("PictureBox2.Image");
		((Control)PictureBox2).Location = new Point(417, 45);
		((Control)PictureBox2).Name = "PictureBox2";
		((Control)PictureBox2).Size = new Size(24, 24);
		PictureBox2.SizeMode = (PictureBoxSizeMode)4;
		PictureBox2.TabIndex = 24;
		PictureBox2.TabStop = false;
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.Image = (Image)componentResourceManager.GetObject("PictureBox1.Image");
		((Control)PictureBox1).Location = new Point(417, 21);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(24, 24);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 23;
		PictureBox1.TabStop = false;
		((Control)LblUnKnown).Anchor = (AnchorStyles)9;
		((Control)LblUnKnown).Location = new Point(197, 72);
		((Control)LblUnKnown).Name = "LblUnKnown";
		((Control)LblUnKnown).Size = new Size(95, 22);
		((Control)LblUnKnown).TabIndex = 7;
		((Control)LblUnKnown).Text = "0";
		((Control)LblNotDeliverd).Anchor = (AnchorStyles)9;
		((Control)LblNotDeliverd).Location = new Point(197, 48);
		((Control)LblNotDeliverd).Name = "LblNotDeliverd";
		((Control)LblNotDeliverd).Size = new Size(95, 24);
		((Control)LblNotDeliverd).TabIndex = 5;
		((Control)LblNotDeliverd).Text = "0";
		((Control)LblDelivered).Anchor = (AnchorStyles)9;
		((Control)LblDelivered).Location = new Point(197, 24);
		((Control)LblDelivered).Name = "LblDelivered";
		((Control)LblDelivered).Size = new Size(95, 24);
		((Control)LblDelivered).TabIndex = 3;
		((Control)LblDelivered).Text = "0";
		((Control)LBLTotal).Location = new Point(203, 2);
		((Control)LBLTotal).Name = "LBLTotal";
		((Control)LBLTotal).Size = new Size(89, 19);
		((Control)LBLTotal).TabIndex = 1;
		((Control)LBLTotal).Text = "0";
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LabelX3).Location = new Point(324, 2);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).RightToLeft = (RightToLeft)1;
		((Control)LabelX3).Size = new Size(93, 19);
		((Control)LabelX3).TabIndex = 0;
		((Control)LabelX3).Text = "[کل پیام ها ]:";
		((Control)TextBoxX1).BackColor = Color.LightSteelBlue;
		TextBoxX1.Border.Class = "TextBoxBorder";
		TextBoxX1.Border.CornerType = eCornerType.Square;
		((Control)TextBoxX1).Dock = (DockStyle)5;
		((Control)TextBoxX1).Location = new Point(0, 0);
		((TextBoxBase)TextBoxX1).Multiline = true;
		((Control)TextBoxX1).Name = "TextBoxX1";
		((TextBoxBase)TextBoxX1).ReadOnly = true;
		((TextBox)TextBoxX1).ScrollBars = (ScrollBars)3;
		((Control)TextBoxX1).Size = new Size(287, 96);
		((Control)TextBoxX1).TabIndex = 0;
		TextBoxX1.WatermarkColor = Color.Empty;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.MintCream;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[22]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGDeliverySTShort,
			(DataGridViewColumn)DGSendDateM,
			(DataGridViewColumn)DGSendDateSh,
			(DataGridViewColumn)DGSendTime,
			(DataGridViewColumn)DGDeliverySt,
			(DataGridViewColumn)dataGridViewTextBoxColumn3,
			(DataGridViewColumn)DGNumber,
			(DataGridViewColumn)DGDeliveryRequested,
			(DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)imgDataGridViewImageColumn,
			(DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGMessage,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)customerIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)sTDataGridViewCheckBoxColumn
		});
		((Control)DGrid).ContextMenuStrip = ContextMenue;
		((DataGridView)DGrid).DataSource = SMSSentBindingSource;
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
		((Control)DGrid).Location = new Point(9, 163);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(750, 236);
		((Control)DGrid).TabIndex = 1;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((DataGridView)DGrid).CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)DGDeliverySTShort).DataPropertyName = "DeliverySTShort";
		((DataGridViewColumn)DGDeliverySTShort).HeaderText = "DeliverySTShort";
		((DataGridViewColumn)DGDeliverySTShort).Name = "DGDeliverySTShort";
		((DataGridViewBand)DGDeliverySTShort).ReadOnly = true;
		((DataGridViewBand)DGDeliverySTShort).Visible = false;
		((DataGridViewColumn)DGSendDateM).DataPropertyName = "SendDateM";
		((DataGridViewColumn)DGSendDateM).HeaderText = "SendDateM";
		((DataGridViewColumn)DGSendDateM).Name = "DGSendDateM";
		((DataGridViewBand)DGSendDateM).Visible = false;
		((DataGridViewColumn)DGSendDateSh).DataPropertyName = "SendDateSh";
		((DataGridViewColumn)DGSendDateSh).HeaderText = "[تاریخ ارسال]";
		((DataGridViewColumn)DGSendDateSh).Name = "DGSendDateSh";
		((DataGridViewBand)DGSendDateSh).ReadOnly = true;
		((DataGridViewColumn)DGSendTime).DataPropertyName = "SendTime";
		((DataGridViewColumn)DGSendTime).HeaderText = "[زمان ارسال]";
		((DataGridViewColumn)DGSendTime).Name = "DGSendTime";
		((DataGridViewBand)DGSendTime).ReadOnly = true;
		((DataGridViewColumn)DGDeliverySt).DataPropertyName = "DeliveryStatusForDisplay";
		((DataGridViewColumn)DGDeliverySt).HeaderText = "[وضعیت ارسال]";
		((DataGridViewColumn)DGDeliverySt).Name = "DGDeliverySt";
		((DataGridViewBand)DGDeliverySt).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "[شماره فرستنده]";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).ReadOnly = true;
		((DataGridViewColumn)DGNumber).DataPropertyName = "Number";
		((DataGridViewColumn)DGNumber).HeaderText = "[شماره گیرنده]";
		((DataGridViewColumn)DGNumber).Name = "DGNumber";
		((DataGridViewColumn)DGDeliveryRequested).AutoSizeMode = (DataGridViewAutoSizeColumnMode)2;
		((DataGridViewColumn)DGDeliveryRequested).DataPropertyName = "DeliveryRequested";
		((DataGridViewColumn)DGDeliveryRequested).HeaderText = "[تحویل شده]";
		((DataGridViewColumn)DGDeliveryRequested).Name = "DGDeliveryRequested";
		((DataGridViewBand)DGDeliveryRequested).Visible = false;
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).DataPropertyName = "RefrencedIds";
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).HeaderText = "RefrencedIds";
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).Name = "refrencedIdsDataGridViewTextBoxColumn";
		((DataGridViewBand)refrencedIdsDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)senderNumberDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).DataPropertyName = "RefrenseAccountId";
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).HeaderText = "RefrenseAccountId";
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).Name = "refrenseAccountIdDataGridViewTextBoxColumn";
		((DataGridViewBand)refrenseAccountIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).DataPropertyName = "RefrencedID";
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).HeaderText = "RefrencedID";
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).Name = "refrencedIDDataGridViewTextBoxColumn";
		((DataGridViewBand)refrencedIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)refrencedIDDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)imgDataGridViewImageColumn).DataPropertyName = "Img";
		((DataGridViewColumn)imgDataGridViewImageColumn).HeaderText = "Img";
		((DataGridViewColumn)imgDataGridViewImageColumn).Name = "imgDataGridViewImageColumn";
		((DataGridViewBand)imgDataGridViewImageColumn).Visible = false;
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).DataPropertyName = "IsChecked";
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).HeaderText = "IsChecked";
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).Name = "isCheckedDataGridViewCheckBoxColumn";
		((DataGridViewBand)isCheckedDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGMessage).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGMessage).DataPropertyName = "Message";
		((DataGridViewColumn)DGMessage).HeaderText = "[متن پیام]";
		((DataGridViewColumn)DGMessage).Name = "DGMessage";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "DateSH";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn2).Visible = false;
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).DataPropertyName = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).HeaderText = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).Name = "customerIdDataGridViewTextBoxColumn";
		((DataGridViewBand)customerIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).HeaderText = "FixedMessage";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).Name = "fixedMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)fixedMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)fixedMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).DataPropertyName = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).HeaderText = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).Name = "sTDataGridViewCheckBoxColumn";
		((DataGridViewBand)sTDataGridViewCheckBoxColumn).Visible = false;
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
		((ToolStripItem)CMASelectAll).Name = "CMASelectAll";
		((ToolStripItem)CMASelectAll).Size = new Size(184, 22);
		((ToolStripItem)CMASelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)CMASelectAll).Click += CMASelectAll_Click;
		((ToolStripItem)CMASelectNone).Name = "CMASelectNone";
		((ToolStripItem)CMASelectNone).Size = new Size(184, 22);
		((ToolStripItem)CMASelectNone).Text = "[انتخاب هیچکدام]";
		((ToolStripItem)CMASelectNone).Click += CMASelectNone_Click;
		((ToolStripItem)CMASelectReverce).Name = "CMASelectReverce";
		((ToolStripItem)CMASelectReverce).Size = new Size(184, 22);
		((ToolStripItem)CMASelectReverce).Text = "[معکوس کردن انتخاب]";
		((ToolStripItem)CMASelectReverce).Click += CMASelectReverce_Click;
		SMSSentBindingSource.DataSource = typeof(SmsSent);
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)BTNSelectAll);
		((Control)PanelEx1).Controls.Add((Control)(object)EPSearch);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel2);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNForward);
		((Control)PanelEx1).Controls.Add((Control)(object)BTNBlackList);
		((Control)PanelEx1).Controls.Add((Control)(object)Panel1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(768, 540);
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
		((Control)BTNSelectAll).Location = new Point(457, 506);
		((Control)BTNSelectAll).Name = "BTNSelectAll";
		((Control)BTNSelectAll).Size = new Size(110, 28);
		BTNSelectAll.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNSelectAll).TabIndex = 37;
		((Control)BTNSelectAll).Text = "[انتخاب همه]";
		((Control)BTNSelectAll).Click += BTNSelectAll_Click_1;
		((Control)EPSearch).Anchor = (AnchorStyles)13;
		EPSearch.CanvasColor = SystemColors.Control;
		EPSearch.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)EPSearch).Controls.Add((Control)(object)chbST);
		((Control)EPSearch).Controls.Add((Control)(object)CHBToDate);
		((Control)EPSearch).Controls.Add((Control)(object)Date1);
		((Control)EPSearch).Controls.Add((Control)(object)Date2);
		((Control)EPSearch).Controls.Add((Control)(object)ChbFromdate);
		((Control)EPSearch).Controls.Add((Control)(object)TxtNumG);
		((Control)EPSearch).Controls.Add((Control)(object)TxtNumF);
		((Control)EPSearch).Controls.Add((Control)(object)LabelX12);
		((Control)EPSearch).Controls.Add((Control)(object)LabelX11);
		((Control)EPSearch).Controls.Add((Control)(object)CMBST);
		((Control)EPSearch).Controls.Add((Control)(object)LX1_Sent);
		((Control)EPSearch).Controls.Add((Control)(object)TXTSearch);
		EPSearch.DisabledBackColor = Color.Empty;
		EPSearch.ExpandButtonVisible = false;
		EPSearch.ExpandOnTitleClick = true;
		((Control)EPSearch).Location = new Point(10, 3);
		((Control)EPSearch).Name = "EPSearch";
		((Control)EPSearch).Size = new Size(750, 154);
		EPSearch.Style.Alignment = (StringAlignment)1;
		EPSearch.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		EPSearch.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		EPSearch.Style.Border = eBorderType.SingleLine;
		EPSearch.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		EPSearch.Style.BorderWidth = 4;
		EPSearch.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		EPSearch.Style.GradientAngle = 90;
		EPSearch.StyleMouseOver.BackColor1.Color = Color.White;
		EPSearch.StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		((Control)EPSearch).TabIndex = 0;
		EPSearch.TitleStyle.Alignment = (StringAlignment)1;
		EPSearch.TitleStyle.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		EPSearch.TitleStyle.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		EPSearch.TitleStyle.BackgroundImagePosition = eBackgroundImagePosition.CenterRight;
		EPSearch.TitleStyle.Border = eBorderType.RaisedInner;
		EPSearch.TitleStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		EPSearch.TitleStyle.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		EPSearch.TitleStyle.GradientAngle = 90;
		EPSearch.TitleText = "[جســتجـو در ارســال شــده ها]";
		((Control)chbST).Anchor = (AnchorStyles)9;
		((Control)chbST).AutoSize = true;
		((Control)chbST).ForeColor = Color.DarkBlue;
		((Control)chbST).Location = new Point(383, 45);
		((Control)chbST).Name = "chbST";
		((Control)chbST).Size = new Size(71, 17);
		((Control)chbST).TabIndex = 6;
		((Control)chbST).Text = "[وضعیت] :";
		((ButtonBase)chbST).UseVisualStyleBackColor = true;
		chbST.CheckedChanged += chbST_CheckedChanged;
		((Control)CHBToDate).Anchor = (AnchorStyles)9;
		((Control)CHBToDate).AutoSize = true;
		((Control)CHBToDate).ForeColor = Color.DarkBlue;
		((Control)CHBToDate).Location = new Point(149, 75);
		((Control)CHBToDate).Name = "CHBToDate";
		((Control)CHBToDate).Size = new Size(74, 17);
		((Control)CHBToDate).TabIndex = 10;
		((Control)CHBToDate).Text = "[تا تاریخ] :";
		((ButtonBase)CHBToDate).UseVisualStyleBackColor = true;
		CHBToDate.CheckedChanged += CHBToDate_CheckedChanged;
		Date1.AllowNull = false;
		((Control)Date1).Anchor = (AnchorStyles)9;
		Date1.BackcolorTooltip = Color.Red;
		Date1.Border.CornerType = eCornerType.Square;
		((Control)Date1).Enabled = false;
		Date1.EnterToTab = true;
		Date1.EscToClose = true;
		Date1.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		Date1.FocusBackColor = Color.Yellow;
		Date1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		Date1.FocusForeColor = Color.Blue;
		Date1.FocusTextSelect = true;
		((Control)Date1).Font = new Font("Times New Roman", 12f);
		((Control)Date1).Location = new Point(241, 75);
		Date1.MessageEmptyShowDialog = false;
		Date1.MessegeEmpty = "";
		Date1.MessegeEmptyInFormRight = true;
		Date1.MessegeEmptyShowInForm = false;
		Date1.MiladiDate = new DateTime(2025, 7, 21, 3, 25, 11, 240);
		((Control)Date1).Name = "Date1";
		((Control)Date1).RightToLeft = (RightToLeft)0;
		Date1.ShowNameOfDayOnToolTip = false;
		Date1.ShowToolTipText = false;
		((Control)Date1).Size = new Size(132, 20);
		((Control)Date1).TabIndex = 9;
		((Control)Date1).Text = "7/20/2025 ";
		((TextBox)Date1).TextAlign = (HorizontalAlignment)2;
		Date1.TootTipDelay = 500;
		((Control)Date1).TextChanged += Date1_TextChanged;
		Date2.AllowNull = false;
		((Control)Date2).Anchor = (AnchorStyles)9;
		Date2.BackcolorTooltip = Color.Red;
		Date2.Border.CornerType = eCornerType.Square;
		((Control)Date2).Enabled = false;
		Date2.EnterToTab = true;
		Date2.EscToClose = true;
		Date2.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		Date2.FocusBackColor = Color.Yellow;
		Date2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		Date2.FocusForeColor = Color.Blue;
		Date2.FocusTextSelect = true;
		((Control)Date2).Font = new Font("Times New Roman", 12f);
		((Control)Date2).Location = new Point(17, 74);
		Date2.MessageEmptyShowDialog = false;
		Date2.MessegeEmpty = "";
		Date2.MessegeEmptyInFormRight = true;
		Date2.MessegeEmptyShowInForm = false;
		Date2.MiladiDate = new DateTime(2025, 7, 21, 3, 25, 11, 240);
		((Control)Date2).Name = "Date2";
		((Control)Date2).RightToLeft = (RightToLeft)0;
		Date2.ShowNameOfDayOnToolTip = false;
		Date2.ShowToolTipText = false;
		((Control)Date2).Size = new Size(132, 20);
		((Control)Date2).TabIndex = 11;
		((Control)Date2).Text = "7/20/2025 ";
		((TextBox)Date2).TextAlign = (HorizontalAlignment)2;
		Date2.TootTipDelay = 500;
		((Control)Date2).TextChanged += Date2_TextChanged;
		((Control)ChbFromdate).Anchor = (AnchorStyles)9;
		((Control)ChbFromdate).AutoSize = true;
		((Control)ChbFromdate).ForeColor = Color.DarkBlue;
		((Control)ChbFromdate).Location = new Point(383, 77);
		((Control)ChbFromdate).Name = "ChbFromdate";
		((Control)ChbFromdate).Size = new Size(74, 17);
		((Control)ChbFromdate).TabIndex = 8;
		((Control)ChbFromdate).Text = "[از تاریخ] :";
		((ButtonBase)ChbFromdate).UseVisualStyleBackColor = true;
		ChbFromdate.CheckedChanged += ChbFromdate_CheckedChanged;
		((Control)TxtNumG).Anchor = (AnchorStyles)9;
		TxtNumG.Border.Class = "TextBoxBorder";
		TxtNumG.Border.CornerType = eCornerType.Square;
		((Control)TxtNumG).Location = new Point(484, 74);
		((Control)TxtNumG).Name = "TxtNumG";
		((Control)TxtNumG).Size = new Size(159, 20);
		((Control)TxtNumG).TabIndex = 3;
		((Control)TxtNumG).TextChanged += TxtNumG_TextChanged;
		((Control)TxtNumG).Enter += TxtNumG_GotFocus;
		((Control)TxtNumG).Leave += TxtNumG_LostFocus;
		((Control)TxtNumF).Anchor = (AnchorStyles)9;
		TxtNumF.Border.Class = "TextBoxBorder";
		TxtNumF.Border.CornerType = eCornerType.Square;
		((Control)TxtNumF).Location = new Point(484, 47);
		((Control)TxtNumF).Name = "TxtNumF";
		((Control)TxtNumF).Size = new Size(159, 20);
		((Control)TxtNumF).TabIndex = 1;
		((Control)TxtNumF).TextChanged += TxtNumF_TextChanged;
		((Control)TxtNumF).Enter += TxtNumF_GotFocus;
		((Control)TxtNumF).Leave += TxtNumF_GotFocus;
		((Control)LabelX12).Anchor = (AnchorStyles)9;
		((Control)LabelX12).Location = new Point(648, 74);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).Size = new Size(92, 18);
		((Control)LabelX12).TabIndex = 2;
		((Control)LabelX12).Text = "[شماره گیرنده] :";
		LabelX12.TextAlign = (ContentAlignment)16;
		((Control)LabelX11).Anchor = (AnchorStyles)9;
		((Control)LabelX11).Location = new Point(648, 47);
		((Control)LabelX11).Name = "LabelX11";
		((Control)LabelX11).Size = new Size(92, 19);
		((Control)LabelX11).TabIndex = 0;
		((Control)LabelX11).Text = "[شماره فرستنده] :";
		LabelX11.TextAlign = (ContentAlignment)16;
		((Control)CMBST).Anchor = (AnchorStyles)9;
		((ListControl)CMBST).DisplayMember = "DateSH";
		((ComboBox)CMBST).DrawMode = (DrawMode)1;
		((ComboBox)CMBST).DropDownStyle = (ComboBoxStyle)2;
		((Control)CMBST).Enabled = false;
		((ListControl)CMBST).FormattingEnabled = true;
		((ComboBox)CMBST).ItemHeight = 15;
		CMBST.Items.AddRange(new object[6] { CmbResideG, CmbResidem, CmbNM, CmbUnknown, CmbNG, CmbBlack });
		((Control)CMBST).Location = new Point(241, 45);
		((Control)CMBST).Name = "CMBST";
		((Control)CMBST).Size = new Size(132, 21);
		((Control)CMBST).TabIndex = 7;
		((ListControl)CMBST).ValueMember = "DateSH";
		((ComboBox)CMBST).SelectedIndexChanged += CMBST_SelectedIndexChanged;
		CmbResideG.FontSize = 12f;
		CmbResideG.FontStyle = (FontStyle)1;
		CmbResideG.Text = "[رسیده به گوشی]";
		CmbResidem.FontSize = 12f;
		CmbResidem.FontStyle = (FontStyle)1;
		CmbResidem.Text = "[رسیده به مخابرات]";
		CmbNM.FontSize = 12f;
		CmbNM.FontStyle = (FontStyle)1;
		CmbNM.Text = "[نرسیده به مخابرات]";
		CmbUnknown.FontSize = 12f;
		CmbUnknown.FontStyle = (FontStyle)1;
		CmbUnknown.Text = "[نامشخص]";
		CmbNG.FontSize = 12f;
		CmbNG.FontStyle = (FontStyle)1;
		CmbNG.Text = "[نرسیده به گوشی]";
		CmbBlack.FontSize = 12f;
		CmbBlack.FontStyle = (FontStyle)1;
		CmbBlack.Text = "[لیست سیاه]";
		((Control)LX1_Sent).Anchor = (AnchorStyles)9;
		((Control)LX1_Sent).Location = new Point(648, 106);
		((Control)LX1_Sent).Name = "LX1_Sent";
		((Control)LX1_Sent).Size = new Size(92, 22);
		((Control)LX1_Sent).TabIndex = 4;
		((Control)LX1_Sent).Text = "[متن] :";
		LX1_Sent.TextAlign = (ContentAlignment)16;
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		TXTSearch.Border.Class = "TextBoxBorder";
		TXTSearch.Border.CornerType = eCornerType.Square;
		((Control)TXTSearch).Location = new Point(17, 106);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(627, 20);
		((Control)TXTSearch).TabIndex = 5;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((Control)TXTSearch).Enter += TXTSearch_GotFocus;
		((Control)TXTSearch).Leave += TXTSearch_LostFocus;
		((Control)BTNPrint).AccessibleRole = (AccessibleRole)43;
		((Control)BTNPrint).Anchor = (AnchorStyles)10;
		BTNPrint.ColorTable = eButtonColor.Office2007WithBackground;
		BTNPrint.Image = (Image)componentResourceManager.GetObject("BTNPrint.Image");
		BTNPrint.ImagePosition = eImagePosition.Right;
		((Control)BTNPrint).Location = new Point(573, 506);
		((Control)BTNPrint).Name = "BTNPrint";
		((Control)BTNPrint).Size = new Size(110, 28);
		BTNPrint.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNPrint).TabIndex = 4;
		((Control)BTNPrint).Text = "[چـــاپ]";
		((Control)BTNPrint).Click += BTNPrint_Click;
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTNExit.Image = (Image)componentResourceManager.GetObject("BTNExit.Image");
		((Control)BTNExit).Location = new Point(9, 506);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(30, 28);
		((Control)BTNExit).TabIndex = 7;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "[خروج]";
		((Control)BTNExit).Click += BTNExit_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BTNDelete).Anchor = (AnchorStyles)10;
		BTNDelete.ColorTable = eButtonColor.Office2007WithBackground;
		BTNDelete.Image = (Image)(object)Resources.delete1;
		((Control)BTNDelete).Location = new Point(693, 506);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).Size = new Size(30, 28);
		((Control)BTNDelete).TabIndex = 3;
		((Control)BTNDelete).TabStop = false;
		BTNDelete.Tooltip = "[حذف پیام ارسالی]";
		((Control)BTNDelete).Click += BtnDelete_Click;
		((Control)BTNAdd).AccessibleRole = (AccessibleRole)43;
		((Control)BTNAdd).Anchor = (AnchorStyles)6;
		BTNAdd.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNAdd).Enabled = false;
		BTNAdd.Image = (Image)componentResourceManager.GetObject("BTNAdd.Image");
		((Control)BTNAdd).Location = new Point(81, 506);
		((Control)BTNAdd).Name = "BTNAdd";
		((Control)BTNAdd).Size = new Size(30, 28);
		((Control)BTNAdd).TabIndex = 5;
		((Control)BTNAdd).TabStop = false;
		BTNAdd.Tooltip = "[اضافه کردن گیرنده به لیست مخاطبین]";
		((Control)BTNAdd).Click += BTNAdd_Click;
		((Control)BTNForward).AccessibleRole = (AccessibleRole)43;
		((Control)BTNForward).Anchor = (AnchorStyles)10;
		BTNForward.ColorTable = eButtonColor.Office2007WithBackground;
		BTNForward.Image = (Image)componentResourceManager.GetObject("BTNForward.Image");
		((Control)BTNForward).Location = new Point(729, 506);
		((Control)BTNForward).Name = "BTNForward";
		((Control)BTNForward).Size = new Size(30, 28);
		((Control)BTNForward).TabIndex = 2;
		((Control)BTNForward).TabStop = false;
		BTNForward.Tooltip = "[ارسال پیام به دیگری]";
		((Control)BTNForward).Click += BTNForward_Click;
		((Control)BTNBlackList).AccessibleRole = (AccessibleRole)43;
		((Control)BTNBlackList).Anchor = (AnchorStyles)6;
		((Control)BTNBlackList).BackColor = Color.Gainsboro;
		BTNBlackList.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNBlackList).Enabled = false;
		BTNBlackList.Image = (Image)componentResourceManager.GetObject("BTNBlackList.Image");
		((Control)BTNBlackList).Location = new Point(45, 506);
		((Control)BTNBlackList).Name = "BTNBlackList";
		((Control)BTNBlackList).Size = new Size(30, 28);
		((Control)BTNBlackList).TabIndex = 6;
		((Control)BTNBlackList).TabStop = false;
		BTNBlackList.Tooltip = "[حذف گیرنده از لیست مخاطبین]";
		((Control)BTNBlackList).Click += BTNBlackList_Click;
		((Control)Panel1).Anchor = (AnchorStyles)14;
		Panel1.BorderStyle = (BorderStyle)1;
		((Control)Panel1).Controls.Add((Control)(object)TextBoxX1);
		((Control)Panel1).Location = new Point(9, 405);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(289, 98);
		((Control)Panel1).TabIndex = 32;
		((DataGridViewColumn)DataGridViewImageColumn1).DataPropertyName = "IMG";
		((DataGridViewColumn)DataGridViewImageColumn1).FillWeight = 20f;
		((DataGridViewColumn)DataGridViewImageColumn1).HeaderText = "";
		DataGridViewImageColumn1.ImageLayout = (DataGridViewImageCellLayout)3;
		((DataGridViewColumn)DataGridViewImageColumn1).Name = "DataGridViewImageColumn1";
		((DataGridViewBand)DataGridViewImageColumn1).ReadOnly = true;
		((DataGridViewBand)DataGridViewImageColumn1).Resizable = (DataGridViewTriState)2;
		((DataGridViewColumn)DataGridViewImageColumn1).Width = 20;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "UCsentList";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(768, 540);
		((Control)GroupPanel2).ResumeLayout(false);
		((ISupportInitialize)pictureBox7).EndInit();
		((ISupportInitialize)PictureBox6).EndInit();
		((ISupportInitialize)PictureBox5).EndInit();
		((ISupportInitialize)PictureBox4).EndInit();
		((ISupportInitialize)PictureBox3).EndInit();
		((ISupportInitialize)PictureBox2).EndInit();
		((ISupportInitialize)PictureBox1).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)ContextMenue).ResumeLayout(false);
		((ISupportInitialize)SMSSentBindingSource).EndInit();
		((Control)PanelEx1).ResumeLayout(false);
		((Control)EPSearch).ResumeLayout(false);
		((Control)EPSearch).PerformLayout();
		((Control)Panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
