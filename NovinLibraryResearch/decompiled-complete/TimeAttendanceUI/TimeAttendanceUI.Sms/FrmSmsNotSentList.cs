using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Sms;

public class FrmSmsNotSentList : BaseForm
{
	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _deleteToken;

	private List<BaseSmsOutBox> _lstSmsOutBox = new List<BaseSmsOutBox>();

	private List<BaseSmsOutBox> _dGridListsForDisplay = new List<BaseSmsOutBox>();

	private IContainer components;

	private PanelEx panelEx1;

	public DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	private BindingSource SmsNotSentListBindingSource;

	private DateBoxFa TbxDateFrom;

	private Label label1;

	private Label label2;

	private DateBoxFa TbxDateTo;

	private Label LblCount;

	private Label label3;

	private ButtonX BTNDelete;

	private Timer timer1;

	private ButtonX BtnAutoRefresh;

	private Label label4;

	private Panel PanelAutoUpdate;

	private Label label5;

	private NumericUpDown NumAutoUpdate;

	private Label LblAutoUpdate;

	private Label label6;

	private DataGridViewTextBoxColumn Number;

	private DataGridViewTextBoxColumn FixedMessage;

	private DataGridViewTextBoxColumn DateSH;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TryCount;

	private DataGridViewTextBoxColumn Column1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public BaseSmsOutBox SelectedItem { get; private set; }

	public bool IsTimerAutoUpdateEnabled { get; private set; }

	public FrmSmsNotSentList(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			TbxDateFrom.MiladiDate = DateTime.Now.Date;
			TbxDateTo.MiladiDate = DateTime.Now.Date.AddSeconds(86399.0);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs");
			((Control)TxtSearch).Focus();
			((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
			((Control)TbxDateFrom).TextChanged += TbxDates_TextChanged;
			((Control)TbxDateTo).TextChanged += TbxDates_TextChanged;
			NumAutoUpdate.ValueChanged += NumAutoUpdate_ValueChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs", 47);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		if (token.IsCancellationRequested || IsDisposed)
		{
			return;
		}
		SetTimerAutoUpdateTemprary(enable: false);
		try
		{
			ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>> result = await PayamakClientHubInstances.GetInstance(((CacheBaseForm)this).ContextGuid).SmsOutBox.GetAllAsync(token, TbxDateFrom.MiladiDate.Date, TbxDateTo.MiladiDate.Date.AddDays(1.0), true);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			if (result != null)
			{
				if (((ReturnedSaveFuncInfo)result).HasError)
				{
					await Task.Delay(10000, token);
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "ارتباط با سرویس پیامک لحظه ای قطع شد. در صورت تکرار این پیام، اجرا بودن سرویس پیامک را بررسی نمائید.", "هشدار", 5);
					Log.Error("سرویس پیامک در این لحظه پاسخگو نبود");
					Log.Error(((ReturnedSaveFuncInfo)result).ErrorMessage);
					await LoadDataAsync(token);
					return;
				}
				_lstSmsOutBox = result.value;
				do
				{
					await Task.Delay(100);
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
				}
				while (!((Control)this).IsHandleCreated);
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!token.IsCancellationRequested && !IsDisposed)
						{
							BindingSource smsNotSentListBindingSource = SmsNotSentListBindingSource;
							List<BaseSmsOutBox> lstSmsOutBox = _lstSmsOutBox;
							object dataSource;
							if (lstSmsOutBox == null)
							{
								dataSource = null;
							}
							else
							{
								IOrderedEnumerable<BaseSmsOutBox> orderedEnumerable = lstSmsOutBox.OrderByDescending((BaseSmsOutBox p) => ((BaseSms)p).DateM);
								if (orderedEnumerable == null)
								{
									dataSource = null;
								}
								else
								{
									List<BaseSmsOutBox> list = orderedEnumerable.ToList();
									dataSource = ((list != null) ? Utilities.ToSortableBindingList<BaseSmsOutBox>((IEnumerable<BaseSmsOutBox>)list, default(CancellationToken)) : null);
								}
							}
							smsNotSentListBindingSource.DataSource = dataSource;
							((Control)LblCount).Text = (_lstSmsOutBox?.Count ?? (-1)).ToString();
						}
					}
					catch (TaskCanceledException)
					{
					}
					catch (Exception ex4)
					{
						Log.Error(ex4, "");
					}
				});
			}
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
		}
		catch (TaskCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs", 94);
		}
		finally
		{
			SetTimerAutoUpdateTemprary(enable: true);
		}
	}

	private void SetTimerAutoUpdateTemprary(bool enable)
	{
		try
		{
			if (enable)
			{
				if (IsTimerAutoUpdateEnabled)
				{
					timer1.Enabled = true;
					timer1.Interval = (int)NumAutoUpdate.Value * 1000;
					timer1.Start();
				}
			}
			else
			{
				timer1.Stop();
				timer1.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		SetTimerAutoUpdateTemprary(enable: false);
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			SetTimerAutoUpdateTemprary(enable: false);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		if (token.IsCancellationRequested || IsDisposed)
		{
			return;
		}
		SetTimerAutoUpdateTemprary(enable: true);
		try
		{
			if (_lstSmsOutBox == null || _lstSmsOutBox.Count <= 0)
			{
				await LoadDataAsync(token);
			}
			string text = Text.FixString(((Control)TxtSearch).Text).Trim();
			if (text == "")
			{
				_dGridListsForDisplay = _lstSmsOutBox;
			}
			else
			{
				string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				_dGridListsForDisplay = _lstSmsOutBox.Select((BaseSmsOutBox e1) => e1)?.ToList();
				string[] array2 = array;
				foreach (string item in array2)
				{
					_dGridListsForDisplay = _dGridListsForDisplay?.Where((BaseSmsOutBox e1) => Calendar.MiladiToCalture(((BaseSms)e1).DateM).Contains(item) || ((BaseSms)e1).Message.Contains(item) || ((BaseSms)e1).Number.Contains(item))?.ToList();
				}
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						BindingSource smsNotSentListBindingSource = SmsNotSentListBindingSource;
						List<BaseSmsOutBox> dGridListsForDisplay = _dGridListsForDisplay;
						object dataSource;
						if (dGridListsForDisplay == null)
						{
							dataSource = null;
						}
						else
						{
							IOrderedEnumerable<BaseSmsOutBox> orderedEnumerable = dGridListsForDisplay.OrderByDescending((BaseSmsOutBox p) => ((BaseSms)p).DateM);
							if (orderedEnumerable == null)
							{
								dataSource = null;
							}
							else
							{
								List<BaseSmsOutBox> list = orderedEnumerable.ToList();
								dataSource = ((list != null) ? Utilities.ToSortableBindingList<BaseSmsOutBox>((IEnumerable<BaseSmsOutBox>)list, default(CancellationToken)) : null);
							}
						}
						smsNotSentListBindingSource.DataSource = dataSource;
						((Control)LblCount).Text = (_dGridListsForDisplay?.Count ?? (-1)).ToString();
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs", 182);
		}
		finally
		{
			SetTimerAutoUpdateTemprary(enable: true);
		}
	}

	private void FRMHesabList_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 13)
			{
				return;
			}
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 40 && ((Control)TxtSearch).Focused)
				{
					((Control)DGrid).Focus();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHesabList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs", 207);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_deleteToken?.Cancel();
				_deleteToken?.Dispose();
				_deleteToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void TbxDates_TextChanged(object sender, EventArgs e)
	{
		SetTimerAutoUpdateTemprary(enable: false);
		try
		{
			_lstSmsOutBox?.Clear();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await Task.Delay(500);
			if (!token.IsCancellationRequested)
			{
				new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "TbxDates_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			SetTimerAutoUpdateTemprary(enable: true);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		try
		{
			SetTimerAutoUpdateTemprary(enable: false);
			if (SelectedItem != null && (int)MessageBoxFarsi.Show("آیا برای حذف مطمئن هستید؟", "هشدار", (MessageBoxFarsiButtons)4, (MessageBoxFarsiIcon)2) == 6)
			{
				PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(((CacheBaseForm)this).ContextGuid);
				_deleteToken?.Cancel();
				_deleteToken = new CancellationTokenSource();
				CancellationToken token = _deleteToken.Token;
				ReturnedSaveFuncInfo val = await instance.SmsOutBox.DeleteListAsync(new List<int>(1) { ((BaseSms)SelectedItem).ID }, token);
				if (val != null && val.HasError)
				{
					MessageBoxFarsi.Show(val.ErrorMessage, "خطای حذف", (MessageBoxFarsiButtons)0, (MessageBoxFarsiIcon)4);
					return;
				}
				_lstSmsOutBox?.Clear();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token2 = _searchToken.Token;
				new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token2), false, (EnWaiterType)0, (Control)null, "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			SetTimerAutoUpdateTemprary(enable: true);
		}
	}

	private void SmsSentListBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		SelectedItem = (BaseSmsOutBox)SmsNotSentListBindingSource.Current;
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_lstSmsOutBox?.Clear();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsNotSentList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnAutoRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)PanelAutoUpdate).Visible)
			{
				((Control)PanelAutoUpdate).Visible = false;
				IsTimerAutoUpdateEnabled = false;
				SetTimerAutoUpdateTemprary(enable: false);
			}
			else
			{
				((Control)PanelAutoUpdate).Visible = true;
				IsTimerAutoUpdateEnabled = true;
				SetTimerAutoUpdateTemprary(enable: true);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void NumAutoUpdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (NumAutoUpdate.Value < 5m)
			{
				NumAutoUpdate.Value = 10m;
			}
			timer1.Interval = (int)NumAutoUpdate.Value * 1000;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
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
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0e: Expected O, but got Unknown
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Expected O, but got Unknown
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Expected O, but got Unknown
		//IL_15c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cb: Expected O, but got Unknown
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e9: Expected O, but got Unknown
		//IL_160e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSmsNotSentList));
		panelEx1 = new PanelEx();
		PanelAutoUpdate = new Panel();
		label6 = new Label();
		label5 = new Label();
		label4 = new Label();
		NumAutoUpdate = new NumericUpDown();
		LblAutoUpdate = new Label();
		BtnAutoRefresh = new ButtonX();
		BTNDelete = new ButtonX();
		LblCount = new Label();
		label3 = new Label();
		label2 = new Label();
		TbxDateTo = new DateBoxFa();
		label1 = new Label();
		TbxDateFrom = new DateBoxFa();
		GroupPanel1 = new GroupPanel();
		TxtSearch = new TextBoxX();
		DGrid = new DataGridViewX();
		Number = new DataGridViewTextBoxColumn();
		FixedMessage = new DataGridViewTextBoxColumn();
		DateSH = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		TryCount = new DataGridViewTextBoxColumn();
		Column1 = new DataGridViewTextBoxColumn();
		SmsNotSentListBindingSource = new BindingSource(components);
		timer1 = new Timer(components);
		((Control)panelEx1).SuspendLayout();
		((Control)PanelAutoUpdate).SuspendLayout();
		((ISupportInitialize)NumAutoUpdate).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)SmsNotSentListBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)PanelAutoUpdate);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAutoRefresh);
		((Control)panelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)panelEx1).Controls.Add((Control)(object)LblCount);
		((Control)panelEx1).Controls.Add((Control)(object)label3);
		((Control)panelEx1).Controls.Add((Control)(object)label2);
		((Control)panelEx1).Controls.Add((Control)(object)TbxDateTo);
		((Control)panelEx1).Controls.Add((Control)(object)label1);
		((Control)panelEx1).Controls.Add((Control)(object)TbxDateFrom);
		((Control)panelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1182, 270);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		panelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)PanelAutoUpdate).Controls.Add((Control)(object)label6);
		((Control)PanelAutoUpdate).Controls.Add((Control)(object)label5);
		((Control)PanelAutoUpdate).Controls.Add((Control)(object)label4);
		((Control)PanelAutoUpdate).Controls.Add((Control)(object)NumAutoUpdate);
		((Control)PanelAutoUpdate).Controls.Add((Control)(object)LblAutoUpdate);
		((Control)PanelAutoUpdate).Location = new Point(3, 171);
		((Control)PanelAutoUpdate).Name = "PanelAutoUpdate";
		((Control)PanelAutoUpdate).Size = new Size(141, 82);
		((Control)PanelAutoUpdate).TabIndex = 29;
		((Control)PanelAutoUpdate).Visible = false;
		((Control)label6).ForeColor = Color.MediumBlue;
		((Control)label6).Location = new Point(111, 56);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(27, 19);
		((Control)label6).TabIndex = 32;
		((Control)label6).Text = "هر";
		label6.TextAlign = (ContentAlignment)16;
		((Control)label5).ForeColor = Color.MediumBlue;
		((Control)label5).Location = new Point(6, 56);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(37, 19);
		((Control)label5).TabIndex = 31;
		((Control)label5).Text = "ثانیه";
		label5.TextAlign = (ContentAlignment)64;
		((Control)label4).ForeColor = Color.DarkRed;
		((Control)label4).Location = new Point(9, 11);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(123, 22);
		((Control)label4).TabIndex = 27;
		((Control)label4).Text = "بروزرسانی فعال است";
		label4.TextAlign = (ContentAlignment)32;
		((Control)NumAutoUpdate).Location = new Point(47, 55);
		NumAutoUpdate.Maximum = new decimal(new int[4] { 3600, 0, 0, 0 });
		NumAutoUpdate.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumAutoUpdate).Name = "NumAutoUpdate";
		((Control)NumAutoUpdate).Size = new Size(61, 21);
		((Control)NumAutoUpdate).TabIndex = 30;
		((UpDownBase)NumAutoUpdate).TextAlign = (HorizontalAlignment)2;
		NumAutoUpdate.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)LblAutoUpdate).ForeColor = Color.MediumBlue;
		((Control)LblAutoUpdate).Location = new Point(9, 33);
		((Control)LblAutoUpdate).Name = "LblAutoUpdate";
		((Control)LblAutoUpdate).Size = new Size(123, 19);
		((Control)LblAutoUpdate).TabIndex = 29;
		((Control)LblAutoUpdate).Text = "فاصله بین بروز رسانی";
		LblAutoUpdate.TextAlign = (ContentAlignment)32;
		((Control)BtnAutoRefresh).AccessibleRole = (AccessibleRole)43;
		BtnAutoRefresh.ColorTable = (eButtonColor)6;
		((Control)BtnAutoRefresh).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAutoRefresh.Image = (Image)(object)Resources.backup;
		BtnAutoRefresh.ImageFixedSize = new Size(30, 30);
		BtnAutoRefresh.ImagePosition = (eImagePosition)1;
		BtnAutoRefresh.ImageTextSpacing = 10;
		((Control)BtnAutoRefresh).Location = new Point(10, 119);
		((Control)BtnAutoRefresh).Margin = new Padding(0);
		((Control)BtnAutoRefresh).Name = "BtnAutoRefresh";
		((Control)BtnAutoRefresh).RightToLeft = (RightToLeft)1;
		((Control)BtnAutoRefresh).Size = new Size(128, 34);
		((PopupItemControl)BtnAutoRefresh).Style = (eDotNetBarStyle)9;
		((Control)BtnAutoRefresh).TabIndex = 24;
		((Control)BtnAutoRefresh).TabStop = false;
		((Control)BtnAutoRefresh).Text = "بروزرسانی خودکار";
		((Control)BtnAutoRefresh).Click += BtnAutoRefresh_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		BTNDelete.ColorTable = (eButtonColor)6;
		((Control)BTNDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNDelete.Image = (Image)(object)Resources.edit_delete;
		BTNDelete.ImagePosition = (eImagePosition)1;
		((Control)BTNDelete).Location = new Point(10, 79);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).RightToLeft = (RightToLeft)1;
		((Control)BTNDelete).Size = new Size(128, 34);
		((PopupItemControl)BTNDelete).Style = (eDotNetBarStyle)9;
		((Control)BTNDelete).TabIndex = 23;
		((Control)BTNDelete).TabStop = false;
		((Control)BTNDelete).Text = "[حذف] (Delete)";
		((Control)BTNDelete).Click += BTNDelete_Click;
		((Control)LblCount).Anchor = (AnchorStyles)10;
		((Control)LblCount).ForeColor = Color.Blue;
		((Control)LblCount).Location = new Point(858, 245);
		((Control)LblCount).Name = "LblCount";
		((Control)LblCount).RightToLeft = (RightToLeft)1;
		((Control)LblCount).Size = new Size(264, 13);
		((Control)LblCount).TabIndex = 21;
		((Control)LblCount).Text = "---";
		((Control)label3).Anchor = (AnchorStyles)10;
		((Control)label3).AutoSize = true;
		((Control)label3).Location = new Point(1128, 245);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)1;
		((Control)label3).Size = new Size(39, 13);
		((Control)label3).TabIndex = 20;
		((Control)label3).Text = "تعداد: ";
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(126, 46);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(21, 13);
		((Control)label2).TabIndex = 19;
		((Control)label2).Text = "تا: ";
		label2.TextAlign = (ContentAlignment)16;
		TbxDateTo.AllowNull = false;
		TbxDateTo.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxDateTo).Border.Class = "TextBoxBorder";
		((TextBoxX)TbxDateTo).Border.CornerType = (eCornerType)1;
		TbxDateTo.EnterToTab = true;
		TbxDateTo.EscToClose = true;
		TbxDateTo.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxDateTo.FocusBackColor = Color.Yellow;
		TbxDateTo.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxDateTo.FocusForeColor = Color.Blue;
		TbxDateTo.FocusTextSelect = true;
		((Control)TbxDateTo).Font = new Font("Times New Roman", 12f);
		((Control)TbxDateTo).Location = new Point(15, 40);
		TbxDateTo.MessageEmptyShowDialog = false;
		TbxDateTo.MessegeEmpty = "";
		TbxDateTo.MessegeEmptyInFormRight = true;
		TbxDateTo.MessegeEmptyShowInForm = false;
		TbxDateTo.MiladiDate = new DateTime(2024, 11, 1, 8, 19, 3, 325);
		((Control)TbxDateTo).Name = "TbxDateTo";
		((Control)TbxDateTo).RightToLeft = (RightToLeft)0;
		TbxDateTo.ShowNameOfDayOnToolTip = false;
		TbxDateTo.ShowToolTipText = false;
		((Control)TbxDateTo).Size = new Size(107, 26);
		((Control)TbxDateTo).TabIndex = 18;
		((Control)TbxDateTo).Text = "31/10/2024";
		((TextBox)TbxDateTo).TextAlign = (HorizontalAlignment)2;
		TbxDateTo.TootTipDelay = 500;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(126, 13);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(22, 13);
		((Control)label1).TabIndex = 17;
		((Control)label1).Text = "از: ";
		label1.TextAlign = (ContentAlignment)16;
		TbxDateFrom.AllowNull = false;
		TbxDateFrom.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxDateFrom).Border.Class = "TextBoxBorder";
		((TextBoxX)TbxDateFrom).Border.CornerType = (eCornerType)1;
		TbxDateFrom.EnterToTab = true;
		TbxDateFrom.EscToClose = true;
		TbxDateFrom.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxDateFrom.FocusBackColor = Color.Yellow;
		TbxDateFrom.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxDateFrom.FocusForeColor = Color.Blue;
		TbxDateFrom.FocusTextSelect = true;
		((Control)TbxDateFrom).Font = new Font("Times New Roman", 12f);
		((Control)TbxDateFrom).Location = new Point(15, 7);
		TbxDateFrom.MessageEmptyShowDialog = false;
		TbxDateFrom.MessegeEmpty = "";
		TbxDateFrom.MessegeEmptyInFormRight = true;
		TbxDateFrom.MessegeEmptyShowInForm = false;
		TbxDateFrom.MiladiDate = new DateTime(2024, 10, 31, 17, 20, 12, 341);
		((Control)TbxDateFrom).Name = "TbxDateFrom";
		((Control)TbxDateFrom).RightToLeft = (RightToLeft)0;
		TbxDateFrom.ShowNameOfDayOnToolTip = false;
		TbxDateFrom.ShowToolTipText = false;
		((Control)TbxDateFrom).Size = new Size(107, 26);
		((Control)TbxDateFrom).TabIndex = 16;
		((Control)TbxDateFrom).Text = "31/10/2024";
		((TextBox)TbxDateFrom).TextAlign = (HorizontalAlignment)2;
		TbxDateFrom.TootTipDelay = 500;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(182, 7);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(988, 60);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(976, 21);
		((Control)TxtSearch).TabIndex = 0;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)Number,
			(DataGridViewColumn)FixedMessage,
			(DataGridViewColumn)DateSH,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)TryCount,
			(DataGridViewColumn)Column1
		});
		((DataGridView)DGrid).DataSource = SmsNotSentListBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(146, 73);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(1024, 157);
		((Control)DGrid).TabIndex = 2;
		((DataGridViewColumn)Number).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Number).DataPropertyName = "Number";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)Number).DefaultCellStyle = val3;
		((DataGridViewColumn)Number).HeaderText = "گیرنده";
		((DataGridViewColumn)Number).MinimumWidth = 100;
		((DataGridViewColumn)Number).Name = "Number";
		((DataGridViewBand)Number).ReadOnly = true;
		((DataGridViewColumn)FixedMessage).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)FixedMessage).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)FixedMessage).HeaderText = "متن پیام";
		((DataGridViewColumn)FixedMessage).Name = "FixedMessage";
		((DataGridViewBand)FixedMessage).ReadOnly = true;
		((DataGridViewColumn)DateSH).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateSH).DataPropertyName = "DateSH";
		val4.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DateSH).DefaultCellStyle = val4;
		((DataGridViewColumn)DateSH).HeaderText = "تاریخ ارسال";
		((DataGridViewColumn)DateSH).MinimumWidth = 100;
		((DataGridViewColumn)DateSH).Name = "DateSH";
		((DataGridViewBand)DateSH).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		val5.Alignment = (DataGridViewContentAlignment)32;
		val5.Format = "t";
		val5.NullValue = null;
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).DefaultCellStyle = val5;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "ساعت";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Width = 66;
		((DataGridViewColumn)TryCount).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TryCount).DataPropertyName = "TryCount";
		((DataGridViewColumn)TryCount).HeaderText = "تعداد تلاش";
		((DataGridViewColumn)TryCount).Name = "TryCount";
		((DataGridViewBand)TryCount).ReadOnly = true;
		((DataGridViewColumn)TryCount).Width = 88;
		((DataGridViewColumn)Column1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Column1).DataPropertyName = "NextTry";
		val6.Format = "t";
		((DataGridViewBand)Column1).DefaultCellStyle = val6;
		((DataGridViewColumn)Column1).HeaderText = "تلاش بعدی";
		((DataGridViewColumn)Column1).Name = "Column1";
		((DataGridViewBand)Column1).ReadOnly = true;
		((DataGridViewColumn)Column1).Width = 89;
		SmsNotSentListBindingSource.DataSource = typeof(BaseSmsOutBox);
		SmsNotSentListBindingSource.CurrentChanged += SmsSentListBindingSource_CurrentChanged;
		timer1.Interval = 30000;
		timer1.Tick += timer1_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Form)this).ClientSize = new Size(1182, 270);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FrmSmsNotSentList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "لیست پیامهای در نوبت ارسال";
		((Control)this).KeyDown += new KeyEventHandler(FRMHesabList_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)PanelAutoUpdate).ResumeLayout(false);
		((ISupportInitialize)NumAutoUpdate).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)SmsNotSentListBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
