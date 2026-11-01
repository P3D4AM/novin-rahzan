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
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.User;

public class FrmUserMain : FrmExtList
{
	private string Frm_Text = "[لیست کاربران]";

	private string trCountRecords = "[تعداد رکوردها]";

	private string trAccessLevel = "[سطح دسترسی]";

	private Font _font;

	private List<UserAttendance> _listData;

	private SortableBindingList<UserAttendance> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private BindingSource userAttendanceBindingSource;

	internal DataGridViewX DGrid;

	private LabelX LblRowCount;

	private DataGridViewCheckBoxColumn DgCheck;

	private DataGridViewTextBoxColumn UserGUID;

	private DataGridViewTextBoxColumn DgCode;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn DGUserId;

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

	public List<UserAttendance> ListUserAttendancesSelected
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<UserAttendance> list = new List<UserAttendance>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DgCheck).Index, i]).Value))
						{
							list.Add(((List<UserAttendance>)userAttendanceBindingSource.DataSource)[i]);
						}
					}
				}
				if (list.Count == 0 && ((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null)
				{
					list.Add(((List<UserAttendance>)userAttendanceBindingSource.DataSource)[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index]);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListUserAttendancesSelected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 102);
			}
			return list;
		}
	}

	public UserAttendance SelectedRow { get; private set; }

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

	public FrmUserMain(string contextGuid, bool isSelect, Font font)
		: base(contextGuid)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Focus();
			((DataGridViewBand)DgCheck).Visible = true;
			if (isSelect)
			{
				((Control)base.BtnSelect).Visible = true;
			}
			((Control)DGrid).Font = _font;
			if (isSelect)
			{
				Frm_Text = "[انتخاب تعدادی کاربر]";
			}
			else
			{
				Frm_Text = "[لیست کاربران]";
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 80);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersMain)
			{
				((Form)this).Close();
				return;
			}
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((Control)base.BtnOther).Visible = true;
			((Control)DGrid).Focus();
			((Control)base.BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersInsert;
			((Control)base.BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersDelete;
			((Control)base.BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersUpdate;
			((Control)base.BtnOther).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersAccessLevel;
			base.BtnOther.Image = (Image)(object)Resources.Key01;
			base.BtnOther.ImagePosition = (eImagePosition)1;
			((Control)base.BtnOther).Name = "BtnAccessLevel";
			((Control)base.BtnOther).Text = trAccessLevel;
			((Control)base.BtnOther).Click += BtnAccessLevel_Click;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 141);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 154);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await UserAttendance.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmUserMain frmUserMain = this;
				List<UserAttendance> listData = _listData;
				frmUserMain._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<UserAttendance>((IEnumerable<UserAttendance>)listData, token) : null);
			}
			else
			{
				IEnumerable<UserAttendance> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((UserAttendance p) => p.Description.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<UserAttendance>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<UserAttendance>)(object)_listForDisplay).Count;
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
						userAttendanceBindingSource.DataSource = _listForDisplay;
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

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 32)
			{
				e.Handled = true;
				if (((Control)DGrid).Focused)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DgCheck).Index];
					DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val != null)
					{
						((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
					}
				}
			}
			else
			{
				((FrmExtList)this).OnKeyDown(e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 217);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.ColumnIndex == ((DataGridViewBand)DgCheck).Index)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 228);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.CanInsert(UserAttendance.SelectedUser.AccessLevelMerg, userAttendanceBindingSource.Count + 1))
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید کاربر ایجاد نمائید]", "", 3);
				return;
			}
			FRMUsers frm = new FRMUsers(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 251);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		try
		{
			if (SelectedRow.ID != UserAttendance.SelectedUser.ID)
			{
				Utility.MessageBoxShow("[شما اجازه ویرایش کاربران دیگر را ندارید]", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			FRMUsers frm = new FRMUsers(((CacheBaseForm)this).ContextGuid, SelectedRow);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 275);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnDelete).Visible || !((Control)base.BtnDelete).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			switch (((DataGridView)DGrid).RowCount)
			{
			case 0:
				return;
			case 1:
				Utility.MessageBoxShow("[حداقل یک کاربر باید در سیستم وجود داشته باشد]", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			if (((DataGridView)DGrid).RowCount <= 1)
			{
				return;
			}
			if (SelectedRow.ID == UserAttendance.SelectedUser.ID)
			{
				Utility.MessageBoxShow("[یک کاربر نمی تواند خودش را حذف کند. لطفا جهت حذف این کاربر با شناسه کاربری دیگری وارد شوید ]", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			UserAttendance userAttendance = await UserAttendance.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.ID, token);
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف کاربر انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				userAttendance.ST = false;
				await userAttendance.SaveAsync(token);
				_listData = null;
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 319);
		}
	}

	private async void BtnAccessLevel_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedRow.ID != UserAttendance.SelectedUser.ID)
			{
				if (SelectedRow != null && (int)(await ((CacheBaseForm)new FrmUseAccLevelNew(((CacheBaseForm)this).ContextGuid, SelectedRow)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
			}
			else
			{
				Utility.MessageBoxShow("[شما مجاز به تغییر سطح دسترسی خودتان نیستید]", ((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccessLevel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 342);
		}
	}

	private void FrmUserMain_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			trCountRecords = Utilities.GetTranslate(trCountRecords);
			trAccessLevel = Utilities.GetTranslate(trAccessLevel);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmUserMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUserMain.cs", 358);
		}
	}

	private void userAttendanceBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (UserAttendance)userAttendanceBindingSource.Current;
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
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DgCheck = new DataGridViewCheckBoxColumn();
		UserGUID = new DataGridViewTextBoxColumn();
		DgCode = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		DGUserId = new DataGridViewTextBoxColumn();
		userAttendanceBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)userAttendanceBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(775, 567);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.GroupPanel1).Size = new Size(713, 60);
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
		((Control)base.LabelX1).Location = new Point(631, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(619, 21);
		((Control)base.BtnAdd).Text = "[اضافه](Insert)";
		((Control)base.BtnSelect).Size = new Size(127, 146);
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
			(DataGridViewColumn)DgCheck,
			(DataGridViewColumn)UserGUID,
			(DataGridViewColumn)DgCode,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)dataGridViewTextBoxColumn4,
			(DataGridViewColumn)DGUserId
		});
		((DataGridView)DGrid).DataSource = userAttendanceBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(616, 465);
		((Control)DGrid).TabIndex = 19;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridViewColumn)DgCheck).HeaderText = "";
		((DataGridViewColumn)DgCheck).MinimumWidth = 20;
		((DataGridViewColumn)DgCheck).Name = "DgCheck";
		((DataGridViewBand)DgCheck).ReadOnly = true;
		((DataGridViewBand)DgCheck).Visible = false;
		((DataGridViewColumn)DgCheck).Width = 20;
		((DataGridViewColumn)UserGUID).DataPropertyName = "UserGUID";
		((DataGridViewColumn)UserGUID).HeaderText = "[UserGUID]";
		((DataGridViewColumn)UserGUID).Name = "UserGUID";
		((DataGridViewBand)UserGUID).ReadOnly = true;
		((DataGridViewBand)UserGUID).Visible = false;
		((DataGridViewColumn)DgCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgCode).DataPropertyName = "Code";
		((DataGridViewColumn)DgCode).HeaderText = "[کد]";
		((DataGridViewColumn)DgCode).Name = "DgCode";
		((DataGridViewBand)DgCode).ReadOnly = true;
		((DataGridViewColumn)DgCode).Width = 54;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Name";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "[نام کاربری]";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).HeaderText = "[شرح]";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Name = "dataGridViewTextBoxColumn4";
		((DataGridViewBand)dataGridViewTextBoxColumn4).ReadOnly = true;
		((DataGridViewColumn)DGUserId).DataPropertyName = "ID";
		((DataGridViewColumn)DGUserId).HeaderText = "[شناسه کاربر]";
		((DataGridViewColumn)DGUserId).Name = "DGUserId";
		((DataGridViewBand)DGUserId).ReadOnly = true;
		((DataGridViewBand)DGUserId).Visible = false;
		userAttendanceBindingSource.DataSource = typeof(UserAttendance);
		userAttendanceBindingSource.CurrentChanged += userAttendanceBindingSource_CurrentChanged;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(13, 518);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 24);
		((Control)LblRowCount).TabIndex = 22;
		((Control)LblRowCount).Text = "labelX3";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(775, 567);
		((Control)this).Name = "FrmUserMain";
		((Control)this).Text = "[کاربر]*";
		((Form)this).Load += FrmUserMain_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)userAttendanceBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
