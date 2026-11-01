using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Sms;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Sms;

public class FrmSmsCalendarList : BaseForm
{
	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private List<SmsCalendar> _lstSmsCalendar = new List<SmsCalendar>();

	private List<SmsCalendar> _dGridListsForDisplay = new List<SmsCalendar>();

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BTNEdit;

	private ButtonX BTNDelete;

	private ButtonX BTNADD;

	public DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	private BindingSource SmsCalendarBindingSource;

	private DataGridViewTextBoxColumn GroupId;

	private DataGridViewTextBoxColumn GroupName;

	private DataGridViewTextBoxColumn DateM;

	private DataGridViewTextBoxColumn Description;

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

	public SmsCalendar SelectedItem { get; private set; }

	public FrmSmsCalendarList(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("در حال دریافت لیست...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs");
			((Control)TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 39);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_lstSmsCalendar = await SmsCalendar.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
			SmsCalendarBindingSource.DataSource = _lstSmsCalendar;
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 51);
		}
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await SearchAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_lstSmsCalendar == null || _lstSmsCalendar.Count <= 0)
			{
				_lstSmsCalendar = await SmsCalendar.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			string text = Text.FixString(((Control)TxtSearch).Text).Trim();
			if (text == "")
			{
				_dGridListsForDisplay = _lstSmsCalendar;
				SmsCalendarBindingSource.DataSource = Utilities.ToSortableBindingList<SmsCalendar>((IEnumerable<SmsCalendar>)_dGridListsForDisplay, default(CancellationToken));
				return;
			}
			string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			_dGridListsForDisplay = _lstSmsCalendar.Select((SmsCalendar e1) => e1).ToList();
			string[] array2 = array;
			foreach (string item in array2)
			{
				_dGridListsForDisplay = _dGridListsForDisplay.Where((SmsCalendar e1) => Calendar.MiladiToCalture(e1.DateM).Contains(item) || e1.Description.Contains(item) || e1.GroupName.Contains(item)).ToList();
			}
			SmsCalendarBindingSource.DataSource = Utilities.ToSortableBindingList<SmsCalendar>((IEnumerable<SmsCalendar>)_dGridListsForDisplay, default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 94);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			_ = e.RowIndex;
			_ = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 103);
		}
	}

	private void FRMHesabList_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 32)
			{
				if ((int)keyCode == 13)
				{
					return;
				}
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 32)
					{
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else if ((int)keyCode <= 45)
			{
				if ((int)keyCode != 40)
				{
					if ((int)keyCode == 45)
					{
						e.Handled = true;
						((PopupItemControl)BTNADD).PerformClick();
					}
				}
				else if (((Control)TxtSearch).Focused)
				{
					((Control)DGrid).Focus();
				}
			}
			else if ((int)keyCode != 46)
			{
				if ((int)keyCode == 118)
				{
					e.Handled = true;
					((PopupItemControl)BTNEdit).PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BTNDelete).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHesabList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 147);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
	}

	private async void BTNADD_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			if (((Control)BTNADD).Visible && ((Control)BTNADD).Enabled && (int)(await ((CacheBaseForm)new FrmSmsCalendarDet(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNADD_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 179);
		}
	}

	private async void BTNEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BTNEdit).Visible && ((Control)BTNEdit).Enabled)
			{
				((Control)DGrid).Focus();
				if (SelectedItem != null && (int)(await ((CacheBaseForm)new FrmSmsCalendarDet(((CacheBaseForm)this).ContextGuid, SelectedItem)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 198);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BTNDelete).Visible && ((Control)BTNDelete).Enabled)
			{
				((Control)DGrid).Focus();
				if (SelectedItem != null && (int)FMessageBox.Show("براي حذف کردن مطمئن هستيد؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
				{
					await SelectedItem.DeleteAsync(default(CancellationToken));
					SelectedItem = null;
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarList.cs", 219);
		}
	}

	private void fishHesabBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedItem = (SmsCalendar)SmsCalendarBindingSource.Current;
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

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
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
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcb: Expected O, but got Unknown
		//IL_0c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSmsCalendarList));
		panelEx1 = new PanelEx();
		GroupPanel1 = new GroupPanel();
		TxtSearch = new TextBoxX();
		BTNEdit = new ButtonX();
		BTNDelete = new ButtonX();
		BTNADD = new ButtonX();
		DGrid = new DataGridViewX();
		SmsCalendarBindingSource = new BindingSource(components);
		GroupId = new DataGridViewTextBoxColumn();
		GroupName = new DataGridViewTextBoxColumn();
		DateM = new DataGridViewTextBoxColumn();
		Description = new DataGridViewTextBoxColumn();
		((Control)panelEx1).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)SmsCalendarBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)BTNEdit);
		((Control)panelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)panelEx1).Controls.Add((Control)(object)BTNADD);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(820, 562);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		panelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(15, 7);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(793, 60);
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
		((Control)TxtSearch).Size = new Size(781, 21);
		((Control)TxtSearch).TabIndex = 0;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BTNEdit).AccessibleRole = (AccessibleRole)43;
		BTNEdit.ColorTable = (eButtonColor)6;
		((Control)BTNEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNEdit.Image = (Image)(object)Resources.edit2;
		BTNEdit.ImagePosition = (eImagePosition)1;
		((Control)BTNEdit).Location = new Point(15, 112);
		((Control)BTNEdit).Name = "BTNEdit";
		((Control)BTNEdit).RightToLeft = (RightToLeft)1;
		((Control)BTNEdit).Size = new Size(128, 34);
		((PopupItemControl)BTNEdit).Style = (eDotNetBarStyle)9;
		((Control)BTNEdit).TabIndex = 4;
		((Control)BTNEdit).TabStop = false;
		((Control)BTNEdit).Text = "[ویرایش] (F7)";
		((Control)BTNEdit).Click += BTNEdit_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		BTNDelete.ColorTable = (eButtonColor)6;
		((Control)BTNDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNDelete.Image = (Image)(object)Resources.edit_delete;
		BTNDelete.ImagePosition = (eImagePosition)1;
		((Control)BTNDelete).Location = new Point(15, 152);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).RightToLeft = (RightToLeft)1;
		((Control)BTNDelete).Size = new Size(128, 34);
		((PopupItemControl)BTNDelete).Style = (eDotNetBarStyle)9;
		((Control)BTNDelete).TabIndex = 5;
		((Control)BTNDelete).TabStop = false;
		((Control)BTNDelete).Text = "[حذف] (Delete)";
		((Control)BTNDelete).Click += BTNDelete_Click;
		((Control)BTNADD).AccessibleRole = (AccessibleRole)43;
		BTNADD.ColorTable = (eButtonColor)6;
		((Control)BTNADD).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNADD.Image = (Image)(object)Resources.edit_add1;
		BTNADD.ImagePosition = (eImagePosition)1;
		((Control)BTNADD).Location = new Point(15, 74);
		((Control)BTNADD).Name = "BTNADD";
		((Control)BTNADD).RightToLeft = (RightToLeft)1;
		((Control)BTNADD).Size = new Size(128, 34);
		((PopupItemControl)BTNADD).Style = (eDotNetBarStyle)9;
		((Control)BTNADD).TabIndex = 7;
		((Control)BTNADD).TabStop = false;
		((Control)BTNADD).Text = "[اضافه] (Insert)";
		((Control)BTNADD).Click += BTNADD_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[4]
		{
			(DataGridViewColumn)GroupId,
			(DataGridViewColumn)GroupName,
			(DataGridViewColumn)DateM,
			(DataGridViewColumn)Description
		});
		((DataGridView)DGrid).DataSource = SmsCalendarBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(152, 73);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(656, 477);
		((Control)DGrid).TabIndex = 2;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		SmsCalendarBindingSource.DataSource = typeof(SmsCalendar);
		SmsCalendarBindingSource.CurrentChanged += fishHesabBindingSource_CurrentChanged;
		((DataGridViewColumn)GroupId).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)GroupId).DataPropertyName = "GroupId";
		((DataGridViewColumn)GroupId).HeaderText = "شناسه گروه";
		((DataGridViewColumn)GroupId).MinimumWidth = 30;
		((DataGridViewColumn)GroupId).Name = "GroupId";
		((DataGridViewBand)GroupId).ReadOnly = true;
		((DataGridViewColumn)GroupId).Width = 98;
		((DataGridViewColumn)GroupName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)GroupName).DataPropertyName = "GroupName";
		((DataGridViewColumn)GroupName).HeaderText = "نام گروه";
		((DataGridViewColumn)GroupName).MinimumWidth = 50;
		((DataGridViewColumn)GroupName).Name = "GroupName";
		((DataGridViewBand)GroupName).ReadOnly = true;
		((DataGridViewColumn)GroupName).Width = 74;
		((DataGridViewColumn)DateM).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateM).DataPropertyName = "DateCalture";
		((DataGridViewColumn)DateM).HeaderText = "تاریخ عدم ارسال پیام";
		((DataGridViewColumn)DateM).MinimumWidth = 130;
		((DataGridViewColumn)DateM).Name = "DateM";
		((DataGridViewBand)DateM).ReadOnly = true;
		((DataGridViewColumn)DateM).Width = 130;
		((DataGridViewColumn)Description).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)Description).DataPropertyName = "Description";
		((DataGridViewColumn)Description).HeaderText = "علت عدم ارسال پیام";
		((DataGridViewColumn)Description).MinimumWidth = 50;
		((DataGridViewColumn)Description).Name = "Description";
		((DataGridViewBand)Description).ReadOnly = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(820, 562);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).IsMdiContainer = true;
		((Control)this).MinimumSize = new Size(709, 370);
		((Control)this).Name = "FrmSmsCalendarList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "لغو ارسال پیام بصورت گروهی";
		((Control)this).KeyDown += new KeyEventHandler(FRMHesabList_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)SmsCalendarBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
