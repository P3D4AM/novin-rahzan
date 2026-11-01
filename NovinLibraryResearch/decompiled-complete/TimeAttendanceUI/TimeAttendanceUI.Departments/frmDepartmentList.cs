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
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Departments;

public class frmDepartmentList : CacheBaseForm
{
	private bool _isDisposed;

	private IEnumerable<AttDepartmentBusiness> _listData;

	private IEnumerable<AttDepartmentBusiness> _filteredList;

	private CancellationTokenSource _loadToken;

	private CancellationTokenSource _searchToken;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal DataGridViewX DGrid;

	private LabelX LblRecordCount;

	private LabelX labelX2;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	private BindingSource DepartmentBindingSource;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn statusDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn adminGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn createdDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn serverStatusDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryShortMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hostRefrencedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn contextGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn databaseManagerDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn changeStatusDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn isDisposedDataGridViewCheckBoxColumn;

	internal ButtonX btnSelect;

	public Guid DepartmentGuid { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!((CacheBaseForm)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public AttDepartmentBusiness Current { get; private set; }

	public string SearchString { get; private set; }

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			_listData = await AttDepartmentBusiness.GetAllAsync(((CacheBaseForm)this).ContextGuid, (bool?)true, token, "");
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken seToken = CancellationTokenSource.CreateLinkedTokenSource(token, _searchToken.Token).Token;
			Task.Run(() => SearchAsync(seToken));
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
			_filteredList = _listData;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<string> list = Text.SplitString(SearchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:");
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					if (!string.IsNullOrEmpty(item))
					{
						_filteredList = _filteredList?.Where((AttDepartmentBusiness x) => Text.StringComparisons(x.Name, item, StringComparison.OrdinalIgnoreCase));
					}
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				try
				{
					BindingSource departmentBindingSource = DepartmentBindingSource;
					IEnumerable<AttDepartmentBusiness> filteredList = _filteredList;
					object dataSource;
					if (filteredList == null)
					{
						dataSource = null;
					}
					else
					{
						IOrderedEnumerable<AttDepartmentBusiness> orderedEnumerable = filteredList.OrderBy((AttDepartmentBusiness q) => q.CreatedDate);
						dataSource = ((orderedEnumerable != null) ? Utilities.ToSortableBindingList<AttDepartmentBusiness>((IEnumerable<AttDepartmentBusiness>)orderedEnumerable, token) : null);
					}
					departmentBindingSource.DataSource = dataSource;
					((Control)LblRecordCount).Text = (_filteredList?.Count() ?? 0).ToString();
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				_loadToken?.Cancel();
				_loadToken?.Dispose();
				_loadToken = null;
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public frmDepartmentList(string contextGuid, bool selectMode)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			if (selectMode)
			{
				((Control)btnSelect).Visible = true;
			}
			else
			{
				((Control)btnSelect).Visible = false;
			}
			Task.Run(() => LoadDataAsync());
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
			if ((int)e.KeyCode == 40 || (int)e.KeyCode == 13)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
				((TextBoxBase)TxtSearch).SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DepartmentBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			Current = (AttDepartmentBusiness)DepartmentBindingSource.Current;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void frmDepartmentList_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				if (!string.IsNullOrEmpty(((Control)TxtSearch).Text))
				{
					((Control)TxtSearch).Text = "";
					return;
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if ((int)e.KeyCode == 45)
			{
				((PopupItemControl)BtnAdd).PerformClick();
			}
			else if ((int)e.KeyCode == 118)
			{
				((PopupItemControl)BtnEdit).PerformClick();
			}
			else if ((int)e.KeyCode == 46)
			{
				((PopupItemControl)BtnDelete).PerformClick();
			}
			else if ((int)e.KeyCode == 13)
			{
				e.Handled = true;
				((PopupItemControl)BtnEdit).PerformClick();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DGrid_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)(await ((CacheBaseForm)new frmDepartment(((CacheBaseForm)this).ContextGuid, new AttDepartmentBusiness(((CacheBaseForm)this).ContextGuid))).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadToken?.Cancel();
				_loadToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadToken.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (Current != null && (int)(await ((CacheBaseForm)new frmDepartment(((CacheBaseForm)this).ContextGuid, Current)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadToken?.Cancel();
				_loadToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadToken.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		JaegerMonitor monitor = new JaegerMonitor("BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartmentList.cs", 60000);
		try
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				try
				{
					((Control)DGrid).Focus();
					if (Current == null || (int)MessageBox.Show("آیا برای حذف کردن مطمئن هستید؟", "هشدار", (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) != 6)
					{
						goto end_IL_0076;
					}
					Current.Status = !Current.Status;
					ReturnedSaveFuncInfo val = res;
					val.AddReturnedValue(await Current.SaveAsync(((CacheBaseForm)this).system, false, false, default(CancellationToken), (short)10, ""), (ReturnedState)0, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartmentList.cs", 205);
					goto end_IL_006d;
					end_IL_0076:;
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
					res.AddReturnedValue(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartmentList.cs", 210);
					goto end_IL_006d;
				}
				return;
				end_IL_006d:;
			}
			finally
			{
				if (res.HasError)
				{
					await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<frmDepartmentList>(this, res, "خطا در حذف", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartmentList.cs");
				}
				else
				{
					_loadToken?.Cancel();
					_loadToken = new CancellationTokenSource(60000);
					CancellationToken token = _loadToken.Token;
					Task.Run(() => LoadDataAsync(token));
				}
			}
		}
		finally
		{
			((IDisposable)monitor)?.Dispose();
		}
	}

	private void btnSelect_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		new ReturnedSaveFuncInfo();
		try
		{
			if (_listData.Count() != 0)
			{
				if (Current == null)
				{
					((Form)this).Close();
				}
				DepartmentGuid = Current.Guid;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void frmDepartmentList_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)TxtSearch).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
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
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d27: Expected O, but got Unknown
		//IL_0dfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e04: Expected O, but got Unknown
		//IL_1133: Unknown result type (might be due to invalid IL or missing references)
		//IL_113d: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_119b: Expected O, but got Unknown
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Expected O, but got Unknown
		//IL_11ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f9: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		btnSelect = new ButtonX();
		DGrid = new DataGridViewX();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		modifiedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		adminGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		createdDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		serverStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryShortMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hostRefrencedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		databaseManagerDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		changeStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		isDisposedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		DepartmentBindingSource = new BindingSource(components);
		LblRecordCount = new LabelX();
		labelX2 = new LabelX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		GroupPanel1 = new GroupPanel();
		TxtSearch = new TextBoxX();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)DepartmentBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)btnSelect);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)LblRecordCount);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(784, 561);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 6;
		((Control)btnSelect).AccessibleRole = (AccessibleRole)43;
		btnSelect.ColorTable = (eButtonColor)6;
		((Control)btnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnSelect.ImagePosition = (eImagePosition)1;
		((Control)btnSelect).Location = new Point(5, 72);
		((Control)btnSelect).Name = "btnSelect";
		((Control)btnSelect).Size = new Size(131, 127);
		((PopupItemControl)btnSelect).Style = (eDotNetBarStyle)9;
		((Control)btnSelect).TabIndex = 17;
		((Control)btnSelect).Text = "[انتخاب (Enter)]";
		((Control)btnSelect).Click += btnSelect_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[16]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDataGridViewCheckBoxColumn,
			(DataGridViewColumn)modifiedDataGridViewTextBoxColumn,
			(DataGridViewColumn)adminGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)createdDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)serverStatusDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn,
			(DataGridViewColumn)contextGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn,
			(DataGridViewColumn)changeStatusDataGridViewTextBoxColumn,
			(DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn
		});
		((DataGridView)DGrid).DataSource = DepartmentBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(142, 72);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(637, 459);
		((Control)DGrid).TabIndex = 8;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((Control)DGrid).MouseDoubleClick += new MouseEventHandler(DGrid_MouseDoubleClick);
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "عنوان";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).DataPropertyName = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).HeaderText = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).Name = "statusDataGridViewCheckBoxColumn";
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).DataPropertyName = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).HeaderText = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).Name = "modifiedDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)adminGuidDataGridViewTextBoxColumn).DataPropertyName = "AdminGuid";
		((DataGridViewColumn)adminGuidDataGridViewTextBoxColumn).HeaderText = "AdminGuid";
		((DataGridViewColumn)adminGuidDataGridViewTextBoxColumn).Name = "adminGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)adminGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)adminGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)createdDateDataGridViewTextBoxColumn).DataPropertyName = "CreatedDate";
		((DataGridViewColumn)createdDateDataGridViewTextBoxColumn).HeaderText = "CreatedDate";
		((DataGridViewColumn)createdDateDataGridViewTextBoxColumn).Name = "createdDateDataGridViewTextBoxColumn";
		((DataGridViewBand)createdDateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)createdDateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).HeaderText = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).Name = "serverStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)serverStatusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)serverStatusDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).DataPropertyName = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).HeaderText = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).Name = "deliveryDateDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryDateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryDateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).DataPropertyName = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).HeaderText = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).Name = "deliveryIdDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).Name = "deliveryMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).Name = "deliveryShortMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryShortMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryShortMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).DataPropertyName = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).HeaderText = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).Name = "hostRefrencedDataGridViewTextBoxColumn";
		((DataGridViewBand)hostRefrencedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)hostRefrencedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).DataPropertyName = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).HeaderText = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).Name = "contextGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).DataPropertyName = "DatabaseManager";
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).HeaderText = "DatabaseManager";
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).Name = "databaseManagerDataGridViewTextBoxColumn";
		((DataGridViewBand)databaseManagerDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)databaseManagerDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).DataPropertyName = "ChangeStatus";
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).HeaderText = "ChangeStatus";
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).Name = "changeStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)changeStatusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)changeStatusDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).DataPropertyName = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).HeaderText = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).Name = "isDisposedDataGridViewCheckBoxColumn";
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).Visible = false;
		DepartmentBindingSource.DataSource = typeof(AttDepartmentBusiness);
		DepartmentBindingSource.CurrentChanged += DepartmentBindingSource_CurrentChanged;
		((Control)LblRecordCount).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRecordCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRecordCount).ForeColor = Color.DarkRed;
		((Control)LblRecordCount).Location = new Point(318, 535);
		((Control)LblRecordCount).Name = "LblRecordCount";
		((Control)LblRecordCount).Size = new Size(360, 23);
		((Control)LblRecordCount).TabIndex = 16;
		((Control)LblRecordCount).Text = "0";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(684, 535);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(95, 23);
		((Control)labelX2).TabIndex = 15;
		((Control)labelX2).Text = "تعداد دپارتمان ها:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(5, 150);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(131, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 11;
		((Control)BtnDelete).Text = "حذف (Delete)";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(5, 111);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(131, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 10;
		((Control)BtnEdit).Text = "ویرایش (F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(5, 72);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(131, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 9;
		((Control)BtnAdd).Text = "افزودن (Ins)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(776, 60);
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
		((Control)GroupPanel1).Text = "جستجو";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 1);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(763, 27);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "frmDepartmentList";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "لیست دپارتمان ها";
		((Form)this).FormClosed += new FormClosedEventHandler(frmDepartmentList_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmDepartmentList_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)DepartmentBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
