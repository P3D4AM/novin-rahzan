using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Forms.User;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UCUsers : UserControl, IHasContext
{
	private Font _font;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private List<UserAttendance> _dGridLists;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	private LabelX CountRows;

	private LabelX labelX2;

	private BindingSource userAttendanceBindingSource;

	internal ButtonX BtnAccessLevel;

	private DataGridViewTextBoxColumn UserGUID;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn DgridID;

	public string ContextGuid { get; set; }

	public UserAttendance SelectedRow { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UCUsers(string contextGuid, Font font)
	{
		try
		{
			_font = font;
			ContextGuid = contextGuid;
			InitializeComponent();
			((Control)DGrid).Font = _font;
			((Control)BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersInsert;
			((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersDelete;
			((Control)BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersUpdate;
			((Control)BtnAccessLevel).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersAccessLevel;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			userAttendanceBindingSource.DataSource = null;
			_dGridLists = await UserAttendance.GetAllAsync(ContextGuid, token);
			RefreshLables();
			userAttendanceBindingSource.DataSource = _dGridLists;
			((Control)TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void RefreshLables()
	{
		try
		{
			((Control)CountRows).Text = userAttendanceBindingSource.Count.ToString();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.CanInsert(UserAttendance.SelectedUser.AccessLevelMerg, userAttendanceBindingSource.Count + 1))
			{
				Utility.MessageBoxShowError("با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید کاربر ایجاد نمائید", ContextGuid);
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			FRMUsers frm = new FRMUsers(ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync(token);
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 115);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			FRMUsers frm = new FRMUsers(ContextGuid, SelectedRow);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync(token);
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 141);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!((Control)BtnDelete).Visible || !((Control)BtnDelete).Enabled)
			{
				return;
			}
			((Control)DGrid).Focus();
			switch (((DataGridView)DGrid).RowCount)
			{
			case 0:
				return;
			case 1:
				Utility.MessageBoxShowError("حداقل يك كاربر بايد در سيستم وجود داشته باشد", ContextGuid);
				return;
			}
			if (((DataGridView)DGrid).RowCount <= 1)
			{
				return;
			}
			if (SelectedRow.ID == UserAttendance.SelectedUser.ID)
			{
				Utility.MessageBoxShowError("يك كاربر نمي تواند خودش را حذف كند لطفا جهت حذف اين كاربر با شناسه كاربري ديگري وارد شويد ", ContextGuid);
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if ((int)FMessageBox.Show("مایل به حذف کاربر انتخاب شده هستید ؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				SelectedRow.ST = false;
				await SelectedRow.SaveAsync(token);
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 177);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string text = Text.FixString(((Control)TxtSearch).Text);
			if (string.IsNullOrEmpty(text))
			{
				userAttendanceBindingSource.Filter = "St=true";
				return;
			}
			userAttendanceBindingSource.Filter = " Name like '%" + text + "%' or Description like '%" + text + "%'";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 188);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Focus();
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = 9999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 200);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13 || (int)e.KeyCode == 40)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 210);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((PopupItemControl)BtnEdit).PerformClick();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 228);
		}
	}

	private async void BtnAccessLevel_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (SelectedRow.ID != UserAttendance.SelectedUser.ID)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				if ((int)(await ((CacheBaseForm)new FrmUseAccLevelNew(ContextGuid, SelectedRow)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync(token);
				}
			}
			else
			{
				Utility.MessageBoxShowError("شما مجاز به تغییر سطح دسترسی خودتان نیستید.", ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccessLevel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCUsers.cs", 251);
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Expected O, but got Unknown
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4f: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		CountRows = new LabelX();
		labelX2 = new LabelX();
		BtnAccessLevel = new ButtonX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		DGrid = new DataGridViewX();
		UserGUID = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		DgridID = new DataGridViewTextBoxColumn();
		userAttendanceBindingSource = new BindingSource(components);
		GroupPanel1 = new GroupPanel();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)userAttendanceBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAccessLevel);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(502, 317);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 3;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(157, 285);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(242, 23);
		((Control)CountRows).TabIndex = 20;
		((Control)CountRows).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(405, 285);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(95, 23);
		((Control)labelX2).TabIndex = 19;
		((Control)labelX2).Text = "[تعداد کل کاربران] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)BtnAccessLevel).AccessibleRole = (AccessibleRole)43;
		BtnAccessLevel.ColorTable = (eButtonColor)6;
		((Control)BtnAccessLevel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAccessLevel.Image = (Image)(object)Resources.Key01;
		BtnAccessLevel.ImagePosition = (eImagePosition)1;
		((Control)BtnAccessLevel).Location = new Point(5, 189);
		((Control)BtnAccessLevel).Name = "BtnAccessLevel";
		((Control)BtnAccessLevel).Size = new Size(128, 33);
		((PopupItemControl)BtnAccessLevel).Style = (eDotNetBarStyle)9;
		((Control)BtnAccessLevel).TabIndex = 11;
		((Control)BtnAccessLevel).Text = "[سطح دسترسی]";
		((Control)BtnAccessLevel).Click += BtnAccessLevel_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(5, 150);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(128, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 11;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(5, 111);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(128, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 10;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(5, 72);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(128, 47);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 9;
		((Control)BtnAdd).Text = "[اضافه](Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)UserGUID,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)dataGridViewTextBoxColumn4,
			(DataGridViewColumn)DgridID
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
		((Control)DGrid).Location = new Point(139, 72);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(357, 207);
		((Control)DGrid).TabIndex = 8;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((DataGridViewColumn)UserGUID).DataPropertyName = "UserGUID";
		((DataGridViewColumn)UserGUID).HeaderText = "UserGUID";
		((DataGridViewColumn)UserGUID).Name = "UserGUID";
		((DataGridViewBand)UserGUID).ReadOnly = true;
		((DataGridViewBand)UserGUID).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "Code";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "[کد]";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Width = 70;
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
		((DataGridViewColumn)DgridID).DataPropertyName = "ID";
		((DataGridViewColumn)DgridID).HeaderText = "ID";
		((DataGridViewColumn)DgridID).Name = "DgridID";
		((DataGridViewBand)DgridID).ReadOnly = true;
		((DataGridViewBand)DgridID).Visible = false;
		userAttendanceBindingSource.DataSource = typeof(UserAttendance);
		userAttendanceBindingSource.CurrentChanged += userAttendanceBindingSource_CurrentChanged;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(493, 60);
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
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(399, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(60, 23);
		((Control)LabelX1).TabIndex = 8;
		((Control)LabelX1).Text = "[جستجو]:";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(390, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).Name = "UCUsers";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(502, 317);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)userAttendanceBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
