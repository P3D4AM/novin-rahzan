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
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.View.Shift;

namespace TimeAttendanceUI.SportClub;

public class FrmPackageList : FrmExtList
{
	private bool _isDisposed;

	private Shifts _clsShifts;

	private int _currRowIndex;

	private List<ShiftViewModel> _listData;

	private SortableBindingList<ShiftViewModel> _listDataForDisplay;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private string _searchString = "";

	private IContainer components;

	internal DataGridViewX DGrid;

	private LabelX lblDesc;

	private LabelX LblRowCount;

	private DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn packageNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descOfDaysDataGridViewTextBoxColumn;

	private BindingSource shiftsBindingSource;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGDescription;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGAzTar;

	private DataGridViewTextBoxColumn DGTaTar;

	private DataGridViewTextBoxColumn StartTime;

	private DataGridViewTextBoxColumn FinishTime;

	private DataGridViewTextBoxColumn periodicDataGridViewTextBoxColumn;

	public sealed override string Text
	{
		get
		{
			return ((Form)this).Text;
		}
		set
		{
			((Form)this).Text = value;
		}
	}

	public ShiftViewModel SelectedShift { get; private set; }

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

	public FrmPackageList(string contextGuid, Font font)
		: base(contextGuid)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)this).Text = "لیست پکیج ها";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 47);
		}
	}

	public FrmPackageList(string contextGuid, Font font, bool btSelect)
		: base(contextGuid)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)base.BtnSelect).Visible = btSelect;
			((Control)this).Text = "انتخاب از لیست پکیج ها";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 65);
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
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 88);
		}
	}

	protected override void Search()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست...", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 100);
		}
	}

	private void SetLabels(int index)
	{
		try
		{
			((Control)lblDesc).Text = "توضیحات : ";
			if (index >= 0)
			{
				ShiftViewModel selectedShift = SelectedShift;
				((Control)lblDesc).Text = "توضیحات : " + selectedShift?.Description;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetLabels", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 112);
		}
	}

	private void DGrid_CurrentCellChanged(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentCell != null && _currRowIndex != ((DataGridView)DGrid).CurrentCell.RowIndex)
			{
				_currRowIndex = ((DataGridView)DGrid).CurrentCell.RowIndex;
				SetLabels(_currRowIndex);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CurrentCellChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 125);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)base.BtnAdd).Visible && ((Control)base.BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)(await ((CacheBaseForm)new FrmPackage(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 144);
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
			if (SelectedShift != null && (int)FMessageBox.Show("براي حذف کردن مطمئن هستيد؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_clsShifts = await Shifts.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedShift.ShiftId);
				_clsShifts.St = false;
				if (await _clsShifts.SaveAsync())
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 164);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnEdit).Visible || !((Control)base.BtnEdit).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (SelectedShift != null && (int)(await ((CacheBaseForm)new FrmPackage(((CacheBaseForm)this).ContextGuid, SelectedShift.ShiftId)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackageList.cs", 182);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				_listData = await ShiftViewModel.SelectAllAsync(((CacheBaseForm)this).ContextGuid, (PackageEnumType)50, token);
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					_searchToken?.Cancel();
					_searchToken = new CancellationTokenSource();
					CancellationToken token2 = _searchToken.Token;
					await SearchAsync(token2);
				}
			}
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
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmPackageList frmPackageList = this;
				List<ShiftViewModel> listData = _listData;
				frmPackageList._listDataForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<ShiftViewModel>((IEnumerable<ShiftViewModel>)listData, token) : null);
			}
			else
			{
				IEnumerable<ShiftViewModel> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					enumerable = enumerable.Where((ShiftViewModel p) => p.Name.Contains(word));
				}
				_listDataForDisplay = Utilities.ToSortableBindingList<ShiftViewModel>(enumerable, token);
			}
			int count = ((Collection<ShiftViewModel>)(object)_listDataForDisplay).Count;
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
						shiftsBindingSource.DataSource = _listDataForDisplay;
						((Control)LblRowCount).Text = "تعداد رکوردها : " + count.ToString("N0");
						if (((DataGridView)DGrid).CurrentRow != null)
						{
							SetLabels(((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index);
						}
						else
						{
							SetLabels(-1);
						}
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

	private void shiftsBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedShift = (ShiftViewModel)shiftsBindingSource.Current;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
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
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGDescription = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGAzTar = new DataGridViewTextBoxColumn();
		DGTaTar = new DataGridViewTextBoxColumn();
		StartTime = new DataGridViewTextBoxColumn();
		FinishTime = new DataGridViewTextBoxColumn();
		periodicDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		shiftsBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		lblDesc = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)shiftsBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)lblDesc);
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 561);
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
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)lblDesc, 0);
		((Control)base.GroupPanel1).Size = new Size(759, 60);
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
		((Control)base.LabelX1).Location = new Point(677, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(665, 21);
		((Control)base.TxtSearch).TextChanged += TxtSearch_TextChanged;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[8]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGDescription,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGAzTar,
			(DataGridViewColumn)DGTaTar,
			(DataGridViewColumn)StartTime,
			(DataGridViewColumn)FinishTime,
			(DataGridViewColumn)periodicDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = shiftsBindingSource;
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
		((Control)DGrid).Size = new Size(625, 430);
		((Control)DGrid).TabIndex = 19;
		((DataGridView)DGrid).CurrentCellChanged += DGrid_CurrentCellChanged;
		((DataGridViewColumn)DGID).DataPropertyName = "ShiftId";
		((DataGridViewColumn)DGID).HeaderText = "Id";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGDescription).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDescription).DataPropertyName = "Name";
		((DataGridViewColumn)DGDescription).HeaderText = "نام پکیج/شیفت";
		((DataGridViewColumn)DGDescription).MinimumWidth = 100;
		((DataGridViewColumn)DGDescription).Name = "DGDescription";
		((DataGridViewBand)DGDescription).ReadOnly = true;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "شرح";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)DGAzTar).DataPropertyName = "DateStartSh";
		((DataGridViewColumn)DGAzTar).HeaderText = "[از تاریخ]";
		((DataGridViewColumn)DGAzTar).MinimumWidth = 120;
		((DataGridViewColumn)DGAzTar).Name = "DGAzTar";
		((DataGridViewBand)DGAzTar).ReadOnly = true;
		((DataGridViewColumn)DGAzTar).Width = 120;
		((DataGridViewColumn)DGTaTar).DataPropertyName = "DateFinishSh";
		((DataGridViewColumn)DGTaTar).HeaderText = "[ToDate]";
		((DataGridViewColumn)DGTaTar).MinimumWidth = 120;
		((DataGridViewColumn)DGTaTar).Name = "DGTaTar";
		((DataGridViewBand)DGTaTar).ReadOnly = true;
		((DataGridViewColumn)DGTaTar).Width = 120;
		((DataGridViewColumn)StartTime).HeaderText = "[ساعت شروع]";
		((DataGridViewColumn)StartTime).MinimumWidth = 50;
		((DataGridViewColumn)StartTime).Name = "StartTime";
		((DataGridViewBand)StartTime).ReadOnly = true;
		((DataGridViewBand)StartTime).Visible = false;
		((DataGridViewColumn)StartTime).Width = 50;
		((DataGridViewColumn)FinishTime).HeaderText = "[ساعت پایان]";
		((DataGridViewColumn)FinishTime).MinimumWidth = 50;
		((DataGridViewColumn)FinishTime).Name = "FinishTime";
		((DataGridViewBand)FinishTime).ReadOnly = true;
		((DataGridViewBand)FinishTime).Visible = false;
		((DataGridViewColumn)FinishTime).Width = 50;
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).DataPropertyName = "Periodic";
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).HeaderText = "[جلسات]";
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).Name = "periodicDataGridViewTextBoxColumn";
		((DataGridViewBand)periodicDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).Width = 83;
		shiftsBindingSource.DataSource = typeof(Shifts);
		shiftsBindingSource.CurrentChanged += shiftsBindingSource_CurrentChanged;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 484);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 27;
		((Control)LblRowCount).Text = "labelX2";
		((Control)lblDesc).Anchor = (AnchorStyles)14;
		((BaseItemControl)lblDesc).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblDesc).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)lblDesc).Location = new Point(147, 513);
		((Control)lblDesc).Name = "lblDesc";
		((Control)lblDesc).Size = new Size(625, 23);
		((Control)lblDesc).TabIndex = 27;
		((Control)lblDesc).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "FrmPackageList";
		((Control)this).Text = "[فرم پکیج]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)shiftsBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
