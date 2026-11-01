using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceUI.Forms.User;

public class FrmLogUserList : FrmExtList
{
	private readonly string _lstUsersId = "";

	private readonly DateTime _dateM1 = DateTime.Now;

	private readonly DateTime _dateM2 = DateTime.Now;

	private string Desc = "";

	private string Frm_Text = "[لیست عملکرد کاربران]";

	private Font _font;

	private List<LogUsers> _listData;

	private SortableBindingList<LogUsers> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private LabelX LblRowCount;

	private DataGridViewX DGrid;

	private BindingSource logUsersBindingSource;

	private DataGridViewTextBoxColumn IdDg;

	private DataGridViewTextBoxColumn UserName;

	private DataGridViewTextBoxColumn Date;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FrmExtList)this).IsDisposed;
			}
			return true;
		}
	}

	public LogUsers SelectedRow { get; private set; }

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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FrmExtList)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmLogUserList(string contextGuid, Font font)
		: base(contextGuid)
	{
		InitializeComponent();
		_font = font;
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
			Frm_Text = "[لیست عملکرد کاربران]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 75);
		}
	}

	public FrmLogUserList(string contextGuid, bool btnSelect, Font font)
		: base(contextGuid)
	{
		InitializeComponent();
		_font = font;
		try
		{
			((Control)base.BtnSelect).Visible = btnSelect;
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
			Frm_Text = "[لیست عملکرد کاربران]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 92);
		}
	}

	public FrmLogUserList(string contextGuid, Font font, List<int> pLstUsersID, DateTime pDateM1, DateTime pDateM2, string pDesc)
		: base(contextGuid)
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_font = font;
		try
		{
			((Control)base.BtnSelect).Visible = true;
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
			Desc = pDesc;
			_dateM1 = pDateM1;
			_dateM2 = pDateM2;
			string[] array = Array.ConvertAll(pLstUsersID.Select((int q) => q).ToArray(), (int s) => s.ToString());
			array.Append("0");
			_lstUsersId = string.Join(",", array);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs");
			Frm_Text = "[لیست عملکرد کاربران]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 118);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersReportLogs)
			{
				((Form)this).Close();
				return;
			}
			((FrmExtList)this).OnLoad(e);
			((Control)base.BtnSelect).Click -= base.BtnSelect_Click;
			((Control)base.BtnSelect).Click += BtnSelectNew_Click;
			((Control)DGrid).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 143);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await LogUsers.SelectAllAsync(((CacheBaseForm)this).ContextGuid, _lstUsersId, _dateM1, _dateM2, Desc, token);
			if (!token.IsCancellationRequested)
			{
				Frm_Text = Utilities.GetTranslate(Frm_Text);
				((Control)this).Text = Frm_Text;
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token2 = _searchToken.Token;
				await SearchAsync(token2);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	protected override void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 174);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await LogUsers.SelectAllAsync(((CacheBaseForm)this).ContextGuid, _lstUsersId, _dateM1, _dateM2, Desc, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmLogUserList frmLogUserList = this;
				List<LogUsers> listData = _listData;
				frmLogUserList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<LogUsers>((IEnumerable<LogUsers>)listData, token) : null);
			}
			else
			{
				IEnumerable<LogUsers> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((LogUsers p) => p.UserName.Contains(word) || p.Description.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<LogUsers>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<LogUsers>)(object)_listForDisplay).Count;
			countRecordTr = Utilities.GetTranslate("[تعداد رکوردها]");
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						logUsersBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = countRecordTr + " : " + count.ToString("N0");
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async void BtnSelectNew_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (SelectedRow != null)
			{
				LogUsers logUsers = await LogUsers.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.Id);
				await ((CacheBaseForm)new FrmLogUser(((CacheBaseForm)this).ContextGuid, logUsers, _font)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectNew_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUserList.cs", 234);
		}
	}

	private void logUsersBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (LogUsers)logUsersBindingSource.Current;
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
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		LblRowCount = new LabelX();
		DGrid = new DataGridViewX();
		IdDg = new DataGridViewTextBoxColumn();
		UserName = new DataGridViewTextBoxColumn();
		Date = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		logUsersBindingSource = new BindingSource(components);
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)logUsersBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(1053, 533);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.GroupPanel1).Location = new Point(12, 11);
		((Control)base.GroupPanel1).Size = new Size(1566, 60);
		((PanelControl)base.GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)base.GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)base.GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)base.GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)base.GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)base.GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)base.GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)base.GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)base.GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)base.GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)base.GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)base.GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)base.LabelX1).Location = new Point(1484, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(1472, 21);
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(12, 474);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 30;
		((Control)LblRowCount).Text = "labelX2";
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)IdDg,
			(DataGridViewColumn)UserName,
			(DataGridViewColumn)Date,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = logUsersBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(893, 420);
		((Control)DGrid).TabIndex = 29;
		((DataGridViewColumn)IdDg).DataPropertyName = "Id";
		((DataGridViewColumn)IdDg).HeaderText = "[شناسه]";
		((DataGridViewColumn)IdDg).Name = "IdDg";
		((DataGridViewBand)IdDg).ReadOnly = true;
		((DataGridViewColumn)UserName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)UserName).DataPropertyName = "UserName";
		((DataGridViewColumn)UserName).HeaderText = "[نام کاربری]";
		((DataGridViewColumn)UserName).MinimumWidth = 120;
		((DataGridViewColumn)UserName).Name = "UserName";
		((DataGridViewBand)UserName).ReadOnly = true;
		((DataGridViewColumn)UserName).Width = 120;
		((DataGridViewColumn)Date).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Date).DataPropertyName = "DateCalture";
		((DataGridViewColumn)Date).HeaderText = "[تاریخ]";
		((DataGridViewColumn)Date).MinimumWidth = 100;
		((DataGridViewColumn)Date).Name = "Date";
		((DataGridViewBand)Date).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "TheTime";
		val2.Format = "T";
		val2.NullValue = null;
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).DefaultCellStyle = val2;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "[زمان]";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Width = 70;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "[عنوان]";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).MinimumWidth = 200;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		logUsersBindingSource.DataSource = typeof(LogUsers);
		logUsersBindingSource.CurrentChanged += logUsersBindingSource_CurrentChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1053, 533);
		((Control)this).Name = "FrmLogUserList";
		((Control)this).Text = "FrmLogUserList";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)logUsersBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
