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
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceUI.Forms.Bimeh;

public class FrmCompanyMain : FrmExtList
{
	private bool _isDisposed;

	private List<Company> _listData;

	private SortableBindingList<Company> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private IContainer components;

	private DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource companyBindingSource;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGCode;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameKarfarmaDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn addressDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nerkhBimehDataGridViewTextBoxColumn;

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

	public Company SelectedRow { get; private set; }

	public FrmCompanyMain(string contextGuid, Font font)
		: base(contextGuid)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "لیست شرکت ها";
			text = Utilities.GetTranslate("[لیست شرکت ها]");
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)this).Text = text;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmCompanyMain(string contgextGuid, Font font, bool pIsSelect)
		: base(contgextGuid)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "لیست شرکت ها";
			text = Utilities.GetTranslate("[لیست شرکت ها]");
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)base.BtnSelect).Visible = pIsSelect;
			((Control)this).Text = text;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((Control)DGrid).Focus();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Company.CompanyMain)
			{
				((Form)this).Close();
				return;
			}
			((Control)base.BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Company.CompanyInsert;
			((Control)base.BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Company.CompanyDelete;
			((Control)base.BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Company.CompanyUpdate;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs", 103);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)base.BtnAdd).Enabled || !((Control)base.BtnAdd).Visible)
		{
			return;
		}
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Company.CanInsert(UserAttendance.SelectedUser.AccessLevelMerg, companyBindingSource.Count + 1))
			{
				Utility.MessageBoxShowError("[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید شرکت ایجاد نمائید]", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			FrmCompany frm = new FrmCompany(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs", 152);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)base.BtnEdit).Enabled || !((Control)base.BtnEdit).Visible)
		{
			return;
		}
		try
		{
			if (((DataGridView)DGrid).RowCount == 0)
			{
				Utility.MessageBoxShowError("[هیچ سطری برای ویرایش وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
			}
			else if (SelectedRow != null)
			{
				FrmCompany frm = new FrmCompany(((CacheBaseForm)this).ContextGuid, SelectedRow.Id);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs", 178);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnDelete).Enabled || !((Control)base.BtnDelete).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (SelectedRow != null)
			{
				Company company = await Company.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.Id);
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف شرکت انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6 && await company.DeleteAsync())
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs");
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FRMCompanyMain.cs", 202);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await Company.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
			if (!token.IsCancellationRequested)
			{
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

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await Company.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmCompanyMain frmCompanyMain = this;
				List<Company> listData = _listData;
				frmCompanyMain._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<Company>((IEnumerable<Company>)listData, token) : null);
			}
			else
			{
				IEnumerable<Company> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					enumerable = enumerable.Where((Company p) => p.Name.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<Company>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<Company>)(object)_listForDisplay).Count;
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
						companyBindingSource.DataSource = _listForDisplay;
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

	private void companyBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (Company)companyBindingSource.Current;
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
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
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Expected O, but got Unknown
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGCode = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nameKarfarmaDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		addressDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nerkhBimehDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		companyBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)companyBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 562);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.Color = Color.SteelBlue;
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
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
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
		((Control)base.LabelX1).Location = new Point(676, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(664, 21);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCode,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn,
			(DataGridViewColumn)addressDataGridViewTextBoxColumn,
			(DataGridViewColumn)nerkhBimehDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = companyBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(148, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(624, 459);
		((Control)DGrid).TabIndex = 24;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "Id";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCode).DataPropertyName = "Code";
		((DataGridViewColumn)DGCode).HeaderText = "[شماره کارگاه]";
		((DataGridViewColumn)DGCode).MinimumWidth = 50;
		((DataGridViewColumn)DGCode).Name = "DGCode";
		((DataGridViewBand)DGCode).ReadOnly = true;
		((DataGridViewColumn)DGCode).Width = 96;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[نام شرکت]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 120;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).DataPropertyName = "NameKarfarma";
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).HeaderText = "[نام کارفرما]";
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).Name = "nameKarfarmaDataGridViewTextBoxColumn";
		((DataGridViewBand)nameKarfarmaDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nameKarfarmaDataGridViewTextBoxColumn).Width = 127;
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).DataPropertyName = "Address";
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).HeaderText = "[آدرس]";
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).Name = "addressDataGridViewTextBoxColumn";
		((DataGridViewBand)addressDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)addressDataGridViewTextBoxColumn).Width = 150;
		((DataGridViewColumn)nerkhBimehDataGridViewTextBoxColumn).DataPropertyName = "NerkhBimeh";
		((DataGridViewColumn)nerkhBimehDataGridViewTextBoxColumn).HeaderText = "NerkhBimeh";
		((DataGridViewColumn)nerkhBimehDataGridViewTextBoxColumn).Name = "nerkhBimehDataGridViewTextBoxColumn";
		((DataGridViewBand)nerkhBimehDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)nerkhBimehDataGridViewTextBoxColumn).Visible = false;
		companyBindingSource.DataSource = typeof(Company);
		companyBindingSource.CurrentChanged += companyBindingSource_CurrentChanged;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(14, 513);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 25;
		((Control)LblRowCount).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 562);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "FrmCompanyMain";
		((Control)this).Text = "[لیست شرکت ها]*";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)companyBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
