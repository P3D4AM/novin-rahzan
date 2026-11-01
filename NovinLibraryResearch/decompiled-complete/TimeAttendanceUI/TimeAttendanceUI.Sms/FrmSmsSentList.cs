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

public class FrmSmsSentList : BaseForm
{
	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private List<BaseSmsOutBox> _lstSmsOutBox = new List<BaseSmsOutBox>();

	private List<BaseSmsOutBox> _dGridListsForDisplay = new List<BaseSmsOutBox>();

	private IContainer components;

	private PanelEx panelEx1;

	public DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	private BindingSource SmsSentListBindingSource;

	private DateBoxFa TbxDateFrom;

	private Label label1;

	private Label label2;

	private DateBoxFa TbxDateTo;

	private Label LblCount;

	private Label label3;

	private DataGridViewTextBoxColumn Number;

	private DataGridViewTextBoxColumn FixedMessage;

	private DataGridViewTextBoxColumn DateSH;

	private DataGridViewTextBoxColumn DateM;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn SentBoxId;

	private DataGridViewTextBoxColumn TryCount;

	private ButtonX BtnRefresh;

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

	public FrmSmsSentList(string contextGuid, Font font)
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
			new Waiter("در حال دریافت لیست...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs");
			((Control)TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs", 41);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>> result = await PayamakClientHubInstances.GetInstance(((CacheBaseForm)this).ContextGuid).SmsOutBox.GetAllAsync(token, TbxDateFrom.MiladiDate.Date, TbxDateTo.MiladiDate.Date.AddDays(1.0), false);
			if (result == null)
			{
				return;
			}
			if (((ReturnedSaveFuncInfo)result).HasError)
			{
				await Task.Delay(10000);
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "ارتباط با سرویس پیامک لحظه ای قطع شد. در صورت تکرار این پیام، اجرا بودن سرویس پیامک را بررسی نمائید.", "هشدار", 5);
				Log.Error("سرویس پیامک در این لحظه پاسخگو نبود");
				Log.Error(((ReturnedSaveFuncInfo)result).ErrorMessage);
				await Task.Delay(10000, token);
				await LoadDataAsync(token);
			}
			else
			{
				_lstSmsOutBox = result.value.Where((BaseSmsOutBox p) => p.SentBoxId > 0).ToList();
				SmsSentListBindingSource.DataSource = Utilities.ToSortableBindingList<BaseSmsOutBox>((IEnumerable<BaseSmsOutBox>)_lstSmsOutBox.OrderByDescending((BaseSmsOutBox p) => ((BaseSms)p).DateM).ToList(), default(CancellationToken));
				((Control)LblCount).Text = _lstSmsOutBox.Count.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs", 68);
		}
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await Task.Delay(2000, token);
			if (!token.IsCancellationRequested)
			{
				new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs");
			}
		}
		catch (TaskCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
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
				_dGridListsForDisplay = _lstSmsOutBox.Select((BaseSmsOutBox e1) => e1).ToList();
				string[] array2 = array;
				foreach (string item in array2)
				{
					_dGridListsForDisplay = _dGridListsForDisplay.Where((BaseSmsOutBox e1) => Calendar.MiladiToCalture(((BaseSms)e1).DateM).Contains(item) || ((BaseSms)e1).Message.Contains(item) || ((BaseSms)e1).Number.Contains(item)).ToList();
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
						SmsSentListBindingSource.DataSource = Utilities.ToSortableBindingList<BaseSmsOutBox>((IEnumerable<BaseSmsOutBox>)_dGridListsForDisplay.OrderByDescending((BaseSmsOutBox p) => ((BaseSms)p).DateM).ToList(), default(CancellationToken));
						((Control)LblCount).Text = _dGridListsForDisplay.Count.ToString();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs", 129);
		}
	}

	private void FrmSmsSentList_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmSmsSentList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs", 153);
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
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TbxDates_TextChanged(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_lstSmsOutBox.Clear();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "TbxDates_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_lstSmsOutBox.Clear();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "BtnRefresh_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsSentList.cs");
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
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
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
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Expected O, but got Unknown
		//IL_0d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d61: Expected O, but got Unknown
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Expected O, but got Unknown
		//IL_1174: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Expected O, but got Unknown
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b4: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		DataGridViewCellStyle val7 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSmsSentList));
		panelEx1 = new PanelEx();
		BtnRefresh = new ButtonX();
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
		DateM = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		SentBoxId = new DataGridViewTextBoxColumn();
		TryCount = new DataGridViewTextBoxColumn();
		SmsSentListBindingSource = new BindingSource(components);
		((Control)panelEx1).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)SmsSentListBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnRefresh);
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
		((Control)panelEx1).Size = new Size(1182, 276);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		panelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnRefresh).AccessibleRole = (AccessibleRole)43;
		BtnRefresh.ColorTable = (eButtonColor)6;
		((Control)BtnRefresh).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnRefresh.Image = (Image)(object)Resources.backup;
		BtnRefresh.ImageFixedSize = new Size(30, 30);
		BtnRefresh.ImagePosition = (eImagePosition)2;
		BtnRefresh.ImageTextSpacing = 10;
		((Control)BtnRefresh).Location = new Point(151, 7);
		((Control)BtnRefresh).Margin = new Padding(0);
		((Control)BtnRefresh).Name = "BtnRefresh";
		((Control)BtnRefresh).RightToLeft = (RightToLeft)1;
		((Control)BtnRefresh).Size = new Size(59, 59);
		((PopupItemControl)BtnRefresh).Style = (eDotNetBarStyle)9;
		((Control)BtnRefresh).TabIndex = 25;
		((Control)BtnRefresh).TabStop = false;
		((Control)BtnRefresh).Click += BtnRefresh_Click;
		((Control)LblCount).Anchor = (AnchorStyles)10;
		((Control)LblCount).ForeColor = Color.Blue;
		((Control)LblCount).Location = new Point(858, 251);
		((Control)LblCount).Name = "LblCount";
		((Control)LblCount).RightToLeft = (RightToLeft)1;
		((Control)LblCount).Size = new Size(264, 13);
		((Control)LblCount).TabIndex = 21;
		((Control)LblCount).Text = "---";
		((Control)label3).Anchor = (AnchorStyles)10;
		((Control)label3).AutoSize = true;
		((Control)label3).Location = new Point(1128, 251);
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
		TbxDateTo.MiladiDate = new DateTime(2024, 10, 31, 16, 5, 8, 442);
		((Control)TbxDateTo).Name = "TbxDateTo";
		((Control)TbxDateTo).RightToLeft = (RightToLeft)0;
		TbxDateTo.ShowNameOfDayOnToolTip = false;
		TbxDateTo.ShowToolTipText = false;
		((Control)TbxDateTo).Size = new Size(107, 26);
		((Control)TbxDateTo).TabIndex = 18;
		((Control)TbxDateTo).Text = "31/10/2024";
		((TextBox)TbxDateTo).TextAlign = (HorizontalAlignment)2;
		TbxDateTo.TootTipDelay = 500;
		((Control)TbxDateTo).TextChanged += TbxDates_TextChanged;
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
		TbxDateFrom.MiladiDate = new DateTime(2024, 11, 1, 1, 6, 17, 442);
		((Control)TbxDateFrom).Name = "TbxDateFrom";
		((Control)TbxDateFrom).RightToLeft = (RightToLeft)0;
		TbxDateFrom.ShowNameOfDayOnToolTip = false;
		TbxDateFrom.ShowToolTipText = false;
		((Control)TbxDateFrom).Size = new Size(107, 26);
		((Control)TbxDateFrom).TabIndex = 16;
		((Control)TbxDateFrom).Text = "31/10/2024";
		((TextBox)TbxDateFrom).TextAlign = (HorizontalAlignment)2;
		TbxDateFrom.TootTipDelay = 500;
		((Control)TbxDateFrom).TextChanged += TbxDates_TextChanged;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(226, 7);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(944, 60);
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
		((Control)TxtSearch).Size = new Size(932, 21);
		((Control)TxtSearch).TabIndex = 0;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)Number,
			(DataGridViewColumn)FixedMessage,
			(DataGridViewColumn)DateSH,
			(DataGridViewColumn)DateM,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)SentBoxId,
			(DataGridViewColumn)TryCount
		});
		((DataGridView)DGrid).DataSource = SmsSentListBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(15, 73);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(1155, 163);
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
		((DataGridViewColumn)DateM).DataPropertyName = "DateM";
		val5.Format = "t";
		((DataGridViewBand)DateM).DefaultCellStyle = val5;
		((DataGridViewColumn)DateM).HeaderText = "ساعت درج";
		((DataGridViewColumn)DateM).Name = "DateM";
		((DataGridViewBand)DateM).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "NextTry";
		val6.Alignment = (DataGridViewContentAlignment)32;
		val6.Format = "t";
		val6.NullValue = null;
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).DefaultCellStyle = val6;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "ساعت ارسال";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Width = 102;
		((DataGridViewColumn)SentBoxId).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)SentBoxId).DataPropertyName = "SentBoxId";
		val7.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)SentBoxId).DefaultCellStyle = val7;
		((DataGridViewColumn)SentBoxId).HeaderText = "شناسه پیام";
		((DataGridViewColumn)SentBoxId).MinimumWidth = 50;
		((DataGridViewColumn)SentBoxId).Name = "SentBoxId";
		((DataGridViewBand)SentBoxId).ReadOnly = true;
		((DataGridViewColumn)SentBoxId).Width = 93;
		((DataGridViewColumn)TryCount).DataPropertyName = "TryCount";
		((DataGridViewColumn)TryCount).HeaderText = "تعداد تلاش";
		((DataGridViewColumn)TryCount).Name = "TryCount";
		((DataGridViewBand)TryCount).ReadOnly = true;
		SmsSentListBindingSource.DataSource = typeof(BaseSmsOutBox);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Form)this).ClientSize = new Size(1182, 276);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).IsMdiContainer = true;
		((Control)this).Name = "FrmSmsSentList";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "لیست پیامهای ارسال شده";
		((Control)this).KeyDown += new KeyEventHandler(FrmSmsSentList_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)SmsSentListBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
