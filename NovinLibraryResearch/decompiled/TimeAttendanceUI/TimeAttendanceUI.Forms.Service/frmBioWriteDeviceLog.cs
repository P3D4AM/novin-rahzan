using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.UC;
using PacketParser;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.Forms.Service;

public class frmBioWriteDeviceLog : AccountingBaseForm
{
	private bool _isDisposed;

	private IEnumerable<BioWriteDeviceLogModel> _list;

	private IEnumerable<BioWriteDeviceLogModel> _filteredList;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _initToken;

	private IContainer components;

	private DataGridView DGrid;

	private BindingSource WriteDeviceLogBindingSource;

	private UcSearchBox txtSearch;

	private DataGridViewTextBoxColumn dgDeviceName;

	private DataGridViewTextBoxColumn dgPersonName;

	private DataGridViewTextBoxColumn dateShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn Time;

	private DataGridViewTextBoxColumn hardwareCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hardwareNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn resultForDisplayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn resultDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn userGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn statusDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn bioGuidDataGridViewTextBoxColumn;

	public BioWriteDeviceLogFilter Filter { get; private set; }

	public string SearchString { get; private set; }

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

	private async Task InitializeAsync(CancellationToken token)
	{
		try
		{
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
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
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					FormExtentions.Initialize(DGrid, setting.FontProgram, true);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 47);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 50);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_list = await BioWriteDeviceLogBusiness.GetAllAsync(((CacheBaseForm)this).ContextGuid, Filter, token, "");
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken seToken = CancellationTokenSource.CreateLinkedTokenSource(_searchToken.Token, token).Token;
				Task.Run(() => SearchAsync(seToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 64);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			_filteredList = _list;
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
						_filteredList = _filteredList?.Where((BioWriteDeviceLogModel x) => Text.StringComparisons(x.DeviceName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.PersonName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.HardwareName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.ResultForDisplay, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.Description, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.HardwareCode, item, StringComparison.OrdinalIgnoreCase));
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
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						BindingSource writeDeviceLogBindingSource = WriteDeviceLogBindingSource;
						IEnumerable<BioWriteDeviceLogModel> filteredList = _filteredList;
						object dataSource;
						if (filteredList == null)
						{
							dataSource = null;
						}
						else
						{
							IOrderedEnumerable<BioWriteDeviceLogModel> orderedEnumerable = filteredList.OrderByDescending((BioWriteDeviceLogModel q) => q.DateM);
							dataSource = ((orderedEnumerable != null) ? Utilities.ToSortableBindingList<BioWriteDeviceLogModel>((IEnumerable<BioWriteDeviceLogModel>)orderedEnumerable, token) : null);
						}
						writeDeviceLogBindingSource.DataSource = dataSource;
						((DataGridViewBand)dgDeviceName).Visible = (_filteredList?.GroupBy((BioWriteDeviceLogModel q) => q.DeviceGuid)?.Count()).GetValueOrDefault() > 1;
						((DataGridViewBand)dgPersonName).Visible = (_filteredList?.GroupBy((BioWriteDeviceLogModel q) => q.PersonName)?.Count()).GetValueOrDefault() > 1;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 103);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 106);
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
				_initToken?.Cancel();
				_initToken?.Dispose();
				_initToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 130);
		}
	}

	public frmBioWriteDeviceLog(string contextGuid, BioWriteDeviceLogFilter filter)
		: base(contextGuid)
	{
		frmBioWriteDeviceLog frmBioWriteDeviceLog2 = this;
		try
		{
			InitializeComponent();
			Filter = filter;
			_initToken = new CancellationTokenSource(60000);
			CancellationToken initToken = _initToken.Token;
			Task.Run(() => frmBioWriteDeviceLog2.InitializeAsync(initToken));
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmBioWriteDeviceLog2.LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 149);
		}
	}

	private void frmBioWriteDeviceLog_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmBioWriteDeviceLog_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				if (!string.IsNullOrEmpty(((Control)txtSearch).Text))
				{
					((Control)txtSearch).Text = "";
					return;
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBioWriteDeviceLog_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 170);
		}
	}

	private void txtSearch_OnKeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 179);
		}
	}

	private void txtSearch_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)txtSearch).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 191);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)txtSearch).Focus();
				((Control)txtSearch).Text = e.KeyChar.ToString();
				txtSearch.SelectionStart = 9999;
				txtSearch.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDeviceLog.cs", 203);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Expected O, but got Unknown
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Expected O, but got Unknown
		components = new Container();
		DGrid = new DataGridView();
		WriteDeviceLogBindingSource = new BindingSource(components);
		txtSearch = new UcSearchBox();
		dgDeviceName = new DataGridViewTextBoxColumn();
		dgPersonName = new DataGridViewTextBoxColumn();
		dateShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		Time = new DataGridViewTextBoxColumn();
		hardwareCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hardwareNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		resultForDisplayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		resultDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		userGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modifiedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		bioGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)WriteDeviceLogBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.BackgroundColor = SystemColors.ControlLightLight;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[16]
		{
			(DataGridViewColumn)dgDeviceName,
			(DataGridViewColumn)dgPersonName,
			(DataGridViewColumn)dateShDataGridViewTextBoxColumn,
			(DataGridViewColumn)Time,
			(DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)resultDataGridViewTextBoxColumn,
			(DataGridViewColumn)userGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)modifiedDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDataGridViewCheckBoxColumn,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)bioGuidDataGridViewTextBoxColumn
		});
		DGrid.DataSource = WriteDeviceLogBindingSource;
		((Control)DGrid).Location = new Point(0, 56);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersWidth = 62;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(784, 502);
		((Control)DGrid).TabIndex = 22;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		WriteDeviceLogBindingSource.DataSource = typeof(BioWriteDeviceLogModel);
		((Control)txtSearch).Anchor = (AnchorStyles)13;
		((Control)txtSearch).BackColor = Color.Transparent;
		((Control)txtSearch).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		txtSearch.IsSaerchBox = true;
		((Control)txtSearch).Location = new Point(67, 8);
		((Control)txtSearch).Margin = new Padding(4, 5, 4, 5);
		((Control)txtSearch).Name = "txtSearch";
		((Control)txtSearch).RightToLeft = (RightToLeft)1;
		txtSearch.SelectionLength = 0;
		txtSearch.SelectionStart = 0;
		((Control)txtSearch).Size = new Size(614, 40);
		((Control)txtSearch).TabIndex = 23;
		txtSearch.OnTextChanged += new EventHandler(txtSearch_OnTextChanged);
		txtSearch.OnKeyDown += new KeyEventHandler(txtSearch_OnKeyDown);
		((DataGridViewColumn)dgDeviceName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgDeviceName).DataPropertyName = "DeviceName";
		((DataGridViewColumn)dgDeviceName).HeaderText = "[دستگاه]";
		((DataGridViewColumn)dgDeviceName).Name = "dgDeviceName";
		((DataGridViewBand)dgDeviceName).ReadOnly = true;
		((DataGridViewColumn)dgDeviceName).Width = 79;
		((DataGridViewColumn)dgPersonName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgPersonName).DataPropertyName = "PersonName";
		((DataGridViewColumn)dgPersonName).HeaderText = "[شخص]";
		((DataGridViewColumn)dgPersonName).Name = "dgPersonName";
		((DataGridViewBand)dgPersonName).ReadOnly = true;
		((DataGridViewColumn)dgPersonName).Width = 74;
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).DataPropertyName = "DateSh";
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).HeaderText = "[تاریخ]";
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).Name = "dateShDataGridViewTextBoxColumn";
		((DataGridViewBand)dateShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).Width = 68;
		((DataGridViewColumn)Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Time).DataPropertyName = "Time";
		((DataGridViewColumn)Time).HeaderText = "[زمان]";
		((DataGridViewColumn)Time).Name = "Time";
		((DataGridViewBand)Time).ReadOnly = true;
		((DataGridViewColumn)Time).Width = 66;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).DataPropertyName = "HardwareCode";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).HeaderText = "[کد سخت افزاری]";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Name = "hardwareCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)hardwareCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Width = 123;
		((DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn).DataPropertyName = "HardwareName";
		((DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn).HeaderText = "[نام سخت افزاری]";
		((DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn).Name = "hardwareNameDataGridViewTextBoxColumn";
		((DataGridViewBand)hardwareNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hardwareNameDataGridViewTextBoxColumn).Width = 125;
		((DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn).DataPropertyName = "ResultForDisplay";
		((DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn).HeaderText = "[نتیجه]";
		((DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn).Name = "resultForDisplayDataGridViewTextBoxColumn";
		((DataGridViewBand)resultForDisplayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)resultForDisplayDataGridViewTextBoxColumn).Width = 72;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)resultDataGridViewTextBoxColumn).DataPropertyName = "Result";
		((DataGridViewColumn)resultDataGridViewTextBoxColumn).HeaderText = "Result";
		((DataGridViewColumn)resultDataGridViewTextBoxColumn).Name = "resultDataGridViewTextBoxColumn";
		((DataGridViewBand)resultDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)resultDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).DataPropertyName = "UserGuid";
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).HeaderText = "UserGuid";
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).Name = "userGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)userGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)userGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).DataPropertyName = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).HeaderText = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).Name = "modifiedDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).DataPropertyName = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).HeaderText = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).Name = "statusDataGridViewCheckBoxColumn";
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).DataPropertyName = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).HeaderText = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).Name = "deviceGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).DataPropertyName = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).HeaderText = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).Name = "bioGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)txtSearch);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmBioWriteDeviceLog";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[سابقه اطلاعات دستگاه]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(frmBioWriteDeviceLog_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmBioWriteDeviceLog_KeyDown);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)WriteDeviceLogBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
