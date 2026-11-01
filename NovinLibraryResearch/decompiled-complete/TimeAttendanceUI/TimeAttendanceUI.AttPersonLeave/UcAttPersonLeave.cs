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
using EntityCache;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms.Reports;

namespace TimeAttendanceUI.AttPersonLeave;

public class UcAttPersonLeave : UserControl
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _hightlightToken;

	private IEnumerable<AttPersonLeaveBusiness> _list;

	private IEnumerable<AttPersonLeaveBusiness> _filteredList;

	private IContainer components;

	private DataGridView dgvBindingObj;

	private ContextMenuStrip rightClickMenuStrip;

	private ToolStripMenuItem EditMenuStrip;

	private ToolStripMenuItem ConfirmMenuStrip;

	private BindingSource BindingSourceAttPersonLeaveBusiness;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private ToolStripMenuItem MenuStripReject;

	private ToolStripMenuItem MenuStripRemove;

	private CheckBox chbPending;

	private CheckBox chbAcceped;

	private CheckBox chbFailed;

	private CheckBox chbRemoved;

	private CheckBox ChbDaily;

	private CheckBox ChbHourly;

	private TextBox TbxSerach;

	private Label label1;

	private DateBoxFa DateBoxTo;

	private DateBoxFa DateBoxFrom;

	private Label label2;

	private Label label3;

	private CheckBox ChkAll;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private GroupBox groupBox1;

	private GroupBox groupBox5;

	private RadioButton rbtnMission;

	private RadioButton rbtnOverTime;

	private RadioButton rbtnLeave;

	private GroupBox grpType;

	private GroupBox groupBox3;

	private GroupBox groupBox2;

	private FlowLayoutPanel flowLayoutPanel1;

	private ToolStripSeparator toolStripMenuItem1;

	private ToolStripMenuItem انتخابToolStripMenuItem;

	private ToolStripMenuItem mnuSelectAll;

	private ToolStripMenuItem mnuSelectNone;

	private ToolStripMenuItem mnuReverse;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn personGuidDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn dgIsChecked;

	private DataGridViewTextBoxColumn QuiddityForDisplay;

	private DataGridViewTextBoxColumn FullName;

	private DataGridViewTextBoxColumn requestDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateFromDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TimeFromSh;

	private DataGridViewTextBoxColumn dateToDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TimeToSh;

	private DataGridViewTextBoxColumn managerDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dgType;

	private DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dgKind;

	private DataGridViewTextBoxColumn reasonDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn managerMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn contextGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryShortMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hostRefrencedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn serverStatusDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	public AttPersonLeaveBusiness CurrentItem { get; private set; }

	public string ContextGuid { get; private set; }

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

	public event Action<List<AttPersonLeaveBusiness>> OnConfirm;

	public event Action<AttPersonLeaveBusiness> OnReject;

	public void Initialize(string contextGuid, CancellationToken token)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ContextGuid = contextGuid;
			DateBoxFrom.MiladiDate = DateTime.Now.AddDays(-2.0);
			DateBoxTo.MiladiDate = DateTime.Now;
			ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.PersonLeave.LeaveSaved += PersonLeave_LeaveSaved;
			new Waiter("بارگذاری داده", (Control)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\UcAttPersonLeave.cs");
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
			await Task.Delay(300);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			if (_list == null)
			{
				_list = await AttPersonLeaveBusiness.GetAllAsync(ContextGuid, default(CancellationToken), "");
			}
			if (_list == null || IsDisposed || token.IsCancellationRequested)
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
					List<EnPersonLeaveStatus> list = new List<EnPersonLeaveStatus>();
					if (chbAcceped.Checked)
					{
						list.Add((EnPersonLeaveStatus)2);
						list.Add((EnPersonLeaveStatus)5);
					}
					if (chbPending.Checked)
					{
						list.Add((EnPersonLeaveStatus)0);
						list.Add((EnPersonLeaveStatus)4);
					}
					if (chbFailed.Checked)
					{
						list.Add((EnPersonLeaveStatus)1);
					}
					if (chbRemoved.Checked)
					{
						list.Add((EnPersonLeaveStatus)3);
					}
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						_loadDataToken?.Cancel();
						_loadDataToken = new CancellationTokenSource(60000);
						CancellationTokenSource t = CancellationTokenSource.CreateLinkedTokenSource(token, _loadDataToken.Token);
						Task.Run(() => BindDataAsync(list, t.Token));
					}
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

	private async Task BindDataAsync(List<EnPersonLeaveStatus> statusList, CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_filteredList = _list;
			DateTime fromDate = DateBoxFrom.MiladiDate.Date;
			DateTime toDate = DateBoxTo.MiladiDate.Date;
			_filteredList = _filteredList?.Where((AttPersonLeaveBusiness p) => p.RequestDate.Date <= toDate && p.RequestDate.Date >= fromDate);
			if (statusList?.Any() ?? false)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness x) => statusList.Contains(x.LeaveStatus));
			}
			if (!ChbHourly.Checked && ChbDaily.Checked)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness p) => (int)p.Type > 0);
			}
			if (!ChbDaily.Checked && ChbHourly.Checked)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness p) => (int)p.Type != 1);
			}
			if (rbtnLeave.Checked)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness q) => (int)q.Quiddity == 0);
			}
			if (rbtnOverTime.Checked)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness q) => (int)q.Quiddity == 1);
			}
			if (rbtnMission.Checked)
			{
				_filteredList = _filteredList?.Where((AttPersonLeaveBusiness q) => (int)q.Quiddity == 2);
			}
			foreach (AttPersonLeaveBusiness item3 in _filteredList)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				Person person = await Person.GetBypersonGuidAsync(ContextGuid, item3.PersonGuid, token);
				if (person != null)
				{
					item3.FullName = ((PersonBase)person).FullName;
				}
				else
				{
					item3.FullName = "نامشخص";
				}
			}
			string text = Text.FixString(((Control)TbxSerach).Text).Trim();
			if (text != "")
			{
				string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string item in array)
				{
					_filteredList = _filteredList.Where((AttPersonLeaveBusiness p) => p.FullName.Contains(item)).ToList();
				}
			}
			foreach (AttPersonLeaveBusiness item3 in _filteredList)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				if ((int)item3.Type != 1)
				{
					continue;
				}
				Person person2 = await Person.GetBypersonGuidAsync(ContextGuid, item3.PersonGuid, token);
				if (person2 == null)
				{
					continue;
				}
				GharardadNew gharardadNew = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)person2).Id, item3.DateFrom, token);
				if (gharardadNew != null)
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					ShiftDetView shiftDetView = await Shifts.GetShiftAsync(await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, item3.DateFrom, gharardadNew.ID, gharardadNew.PersonId, token), item3.DateFrom, token);
					if (shiftDetView != null)
					{
						ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
						item3.DateFrom = item3.DateFrom.AddMinutes(clsShiftCoding.H1 * 60 + clsShiftCoding.M1);
						item3.DateTo = item3.DateTo.AddMinutes(clsShiftCoding.H2 * 60 + clsShiftCoding.M2);
					}
				}
			}
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
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						BindingSource bindingSourceAttPersonLeaveBusiness = BindingSourceAttPersonLeaveBusiness;
						IEnumerable<AttPersonLeaveBusiness> filteredList = _filteredList;
						object dataSource;
						if (filteredList == null)
						{
							dataSource = null;
						}
						else
						{
							IOrderedEnumerable<AttPersonLeaveBusiness> orderedEnumerable = filteredList.OrderByDescending((AttPersonLeaveBusiness q) => q.RequestDate);
							dataSource = ((orderedEnumerable != null) ? Utilities.ToSortableBindingList<AttPersonLeaveBusiness>((IEnumerable<AttPersonLeaveBusiness>)orderedEnumerable, token) : null);
						}
						bindingSourceAttPersonLeaveBusiness.DataSource = dataSource;
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

	private void PersonLeave_LeaveSaved(object sender, EventArgs e)
	{
		try
		{
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

	private void EnableEventChk(bool enable)
	{
		try
		{
			if (enable)
			{
				chbAcceped.CheckedChanged += chbAll_CheckedChanged;
				chbFailed.CheckedChanged += chbAll_CheckedChanged;
				chbPending.CheckedChanged += chbAll_CheckedChanged;
				chbRemoved.CheckedChanged += chbAll_CheckedChanged;
				ChbDaily.CheckedChanged += chbAll_CheckedChanged;
				ChbHourly.CheckedChanged += chbAll_CheckedChanged;
			}
			else
			{
				chbAcceped.CheckedChanged -= chbAll_CheckedChanged;
				chbFailed.CheckedChanged -= chbAll_CheckedChanged;
				chbPending.CheckedChanged -= chbAll_CheckedChanged;
				chbRemoved.CheckedChanged -= chbAll_CheckedChanged;
				ChbDaily.CheckedChanged -= chbAll_CheckedChanged;
				ChbHourly.CheckedChanged -= chbAll_CheckedChanged;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task HightlightAsync(CancellationToken token)
	{
		try
		{
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
					foreach (AttPersonLeaveBusiness item in _filteredList ?? new List<AttPersonLeaveBusiness>())
					{
						for (int i = 0; i < dgvBindingObj.RowCount; i++)
						{
							if (IsDisposed || token.IsCancellationRequested)
							{
								return;
							}
							if (dgvBindingObj[((DataGridViewBand)dgIsChecked).Index, i] != null)
							{
								dgvBindingObj[((DataGridViewBand)dgIsChecked).Index, i].Value = item.IsChecked;
								((DataGridViewBand)dgvBindingObj.Rows[i]).DefaultCellStyle.BackColor = (item.IsChecked ? Color.LightBlue : Color.White);
							}
						}
					}
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

	public UcAttPersonLeave()
	{
		try
		{
			InitializeComponent();
			FormExtentions.Initialize(dgvBindingObj, ((Control)this).Font, true);
			chbPending.Checked = true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BindingSourceAttPersonLeaveBusiness_CurrentChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			CurrentItem = (AttPersonLeaveBusiness)BindingSourceAttPersonLeaveBusiness.Current;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void dgvBindingObj_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		try
		{
			if (CurrentItem == null)
			{
				return;
			}
			if ((int)e.KeyCode == 13 || (int)e.KeyCode == 118)
			{
				e.Handled = true;
				((ToolStripItem)EditMenuStrip).PerformClick();
			}
			else
			{
				if ((int)e.KeyCode != 32 || CurrentItem == null)
				{
					return;
				}
				DataGridView obj = dgvBindingObj;
				if (((obj != null) ? obj.CurrentCell : null) == null)
				{
					return;
				}
				DataGridView obj2 = dgvBindingObj;
				if (((obj2 != null) ? obj2.CurrentRow : null) != null)
				{
					DataGridView obj3 = dgvBindingObj;
					if (((obj3 != null) ? new int?(obj3.CurrentCell.ColumnIndex) : null) == ((DataGridViewBand)dgIsChecked).Index)
					{
						CurrentItem.IsChecked = !CurrentItem.IsChecked;
						((DataGridViewBand)dgvBindingObj.CurrentRow).DefaultCellStyle.BackColor = (CurrentItem.IsChecked ? Color.LightBlue : Color.White);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void EditMenuStrip_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentItem == null)
			{
				return;
			}
			if ((int)CurrentItem.Quiddity == 0)
			{
				if ((int)(await ((CacheBaseForm)new frmAttPersonLeave(ContextGuid, CurrentItem)).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
			}
			else
			{
				Person person = await Person.GetBypersonGuidAsync(ContextGuid, CurrentItem.PersonGuid, default(CancellationToken));
				if (person == null)
				{
					return;
				}
				EN_TypeAtt type = EN_TypeAtt.EzaferKar;
				if ((int)CurrentItem.Quiddity == 2)
				{
					type = EN_TypeAtt.Mission;
				}
				string description = $"از ساعت {CurrentItem.DateFrom.Hour}:{CurrentItem.DateFrom.Minute} تا ساعت {CurrentItem.DateTo.Hour}:{CurrentItem.DateTo.Minute} \r\n {CurrentItem.Reason}";
				if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(ContextGuid, type, person, CurrentItem.DateFrom, CurrentItem.DateTo, description)).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				CurrentItem.LeaveStatus = (EnPersonLeaveStatus)2;
				await CurrentItem.SaveAsync((EnSystem)2, true, (Priority)0, default(CancellationToken), true, false, "");
			}
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

	private void ConfirmMenuStrip_Click_1(object sender, EventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<AttPersonLeaveBusiness> list = _filteredList?.Where((AttPersonLeaveBusiness q) => q.IsChecked)?.ToList();
			if ((list == null || !list.Any()) && CurrentItem != null && (int)CurrentItem.Quiddity == 0 && (int)CurrentItem.Type == 0)
			{
				return;
			}
			if ((list == null || !list.Any()) && CurrentItem != null)
			{
				if (list == null)
				{
					list = new List<AttPersonLeaveBusiness>();
				}
				list.Add(CurrentItem);
			}
			this.OnConfirm?.Invoke(list);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void MenuStripReject_Click(object sender, EventArgs e)
	{
		this.OnReject?.Invoke(CurrentItem);
	}

	private void rightClickMenuStrip_Opening(object sender, CancelEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CurrentItem != null && (int)CurrentItem.Type == 0 && (int)CurrentItem.Quiddity == 0)
			{
				((ToolStripItem)ConfirmMenuStrip).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TbxSerach_TextChanged(object sender, EventArgs e)
	{
		try
		{
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

	private void dateBoxFa1_TextChanged(object sender, EventArgs e)
	{
		try
		{
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

	private void ChkAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			EnableEventChk(enable: false);
			CheckBox obj = chbAcceped;
			CheckBox obj2 = chbFailed;
			CheckBox obj3 = chbPending;
			CheckBox obj4 = chbRemoved;
			CheckBox chbHourly = ChbHourly;
			bool flag = (ChbDaily.Checked = ChkAll.Checked);
			bool flag3 = (chbHourly.Checked = flag);
			bool flag5 = (obj4.Checked = flag3);
			bool flag7 = (obj3.Checked = flag5);
			bool checked2 = (obj2.Checked = flag7);
			obj.Checked = checked2;
			EnableEventChk(enable: true);
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

	private void rbtnLeave_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GroupBox obj = grpType;
			DataGridViewTextBoxColumn obj2 = dgKind;
			bool flag = (((DataGridViewBand)dgType).Visible = rbtnLeave.Checked);
			bool enabled = (((DataGridViewBand)obj2).Visible = flag);
			((Control)obj).Enabled = enabled;
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

	private void dgvBindingObj_MouseClick(object sender, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		try
		{
			if (CurrentItem == null || (int)e.Button == 2097152)
			{
				return;
			}
			DataGridView obj = dgvBindingObj;
			if (((obj != null) ? obj.CurrentCell : null) == null)
			{
				return;
			}
			DataGridView obj2 = dgvBindingObj;
			if (((obj2 != null) ? obj2.CurrentRow : null) != null)
			{
				DataGridView obj3 = dgvBindingObj;
				if (((obj3 != null) ? new int?(obj3.CurrentCell.ColumnIndex) : null) == ((DataGridViewBand)dgIsChecked).Index)
				{
					CurrentItem.IsChecked = !CurrentItem.IsChecked;
					((DataGridViewBand)dgvBindingObj.CurrentRow).DefaultCellStyle.BackColor = (CurrentItem.IsChecked ? Color.LightBlue : Color.White);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void mnuSelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			IEnumerable<AttPersonLeaveBusiness> filteredList = _filteredList;
			if (filteredList == null || !filteredList.Any())
			{
				return;
			}
			foreach (AttPersonLeaveBusiness filtered in _filteredList)
			{
				filtered.IsChecked = true;
			}
			BindingSourceAttPersonLeaveBusiness.ResetBindings(false);
			((Control)dgvBindingObj).ResetBindings();
			_hightlightToken?.Cancel();
			_hightlightToken = new CancellationTokenSource(60000);
			CancellationToken token = _hightlightToken.Token;
			Task.Run(() => HightlightAsync(token));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void mnuSelectNone_Click(object sender, EventArgs e)
	{
		try
		{
			IEnumerable<AttPersonLeaveBusiness> filteredList = _filteredList;
			if (filteredList == null || !filteredList.Any())
			{
				return;
			}
			foreach (AttPersonLeaveBusiness filtered in _filteredList)
			{
				filtered.IsChecked = false;
			}
			BindingSourceAttPersonLeaveBusiness.ResetBindings(false);
			((Control)dgvBindingObj).ResetBindings();
			_hightlightToken?.Cancel();
			_hightlightToken = new CancellationTokenSource(60000);
			CancellationToken token = _hightlightToken.Token;
			Task.Run(() => HightlightAsync(token));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void mnuReverse_Click(object sender, EventArgs e)
	{
		try
		{
			IEnumerable<AttPersonLeaveBusiness> filteredList = _filteredList;
			if (filteredList == null || !filteredList.Any())
			{
				return;
			}
			foreach (AttPersonLeaveBusiness filtered in _filteredList)
			{
				filtered.IsChecked = !filtered.IsChecked;
			}
			BindingSourceAttPersonLeaveBusiness.ResetBindings(false);
			((Control)dgvBindingObj).ResetBindings();
			_hightlightToken?.Cancel();
			_hightlightToken = new CancellationTokenSource(60000);
			CancellationToken token = _hightlightToken.Token;
			Task.Run(() => HightlightAsync(token));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void MenuStripRemove_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CurrentItem != null)
			{
				CurrentItem.LeaveStatus = (EnPersonLeaveStatus)3;
				ReturnedSaveFuncInfo val = res;
				val.AddReturnedValue(await CurrentItem.SaveAsync((EnSystem)2, true, (Priority)0, default(CancellationToken), true, false, ""), (ReturnedState)0, "MenuStripRemove_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\UcAttPersonLeave.cs", 447);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			res.AddReturnedValue(ex, "", "MenuStripRemove_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\UcAttPersonLeave.cs", 452);
		}
		finally
		{
			if (res.HasError)
			{
				res.Show(ContextGuid);
			}
			else
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken token = _searchToken.Token;
				Task.Run(() => SearchAsync(token));
			}
		}
	}

	private void chbAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ChkAll.CheckedChanged -= ChkAll_CheckedChanged;
			ChkAll.Checked = false;
			ChkAll.CheckedChanged += ChkAll_CheckedChanged;
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

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
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
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Expected O, but got Unknown
		//IL_1d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Expected O, but got Unknown
		//IL_1d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d70: Expected O, but got Unknown
		//IL_1f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f18: Expected O, but got Unknown
		//IL_1f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4e: Expected O, but got Unknown
		//IL_2118: Unknown result type (might be due to invalid IL or missing references)
		//IL_2122: Expected O, but got Unknown
		//IL_2127: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		dgvBindingObj = new DataGridView();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		personGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dgIsChecked = new DataGridViewCheckBoxColumn();
		QuiddityForDisplay = new DataGridViewTextBoxColumn();
		FullName = new DataGridViewTextBoxColumn();
		requestDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateFromDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		TimeFromSh = new DataGridViewTextBoxColumn();
		dateToDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		TimeToSh = new DataGridViewTextBoxColumn();
		managerDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dgType = new DataGridViewTextBoxColumn();
		statusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dgKind = new DataGridViewTextBoxColumn();
		reasonDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		managerMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryShortMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hostRefrencedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		rightClickMenuStrip = new ContextMenuStrip(components);
		EditMenuStrip = new ToolStripMenuItem();
		ConfirmMenuStrip = new ToolStripMenuItem();
		MenuStripReject = new ToolStripMenuItem();
		MenuStripRemove = new ToolStripMenuItem();
		toolStripMenuItem1 = new ToolStripSeparator();
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		mnuSelectAll = new ToolStripMenuItem();
		mnuSelectNone = new ToolStripMenuItem();
		mnuReverse = new ToolStripMenuItem();
		BindingSourceAttPersonLeaveBusiness = new BindingSource(components);
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		chbPending = new CheckBox();
		chbAcceped = new CheckBox();
		chbFailed = new CheckBox();
		chbRemoved = new CheckBox();
		ChbDaily = new CheckBox();
		ChbHourly = new CheckBox();
		TbxSerach = new TextBox();
		label1 = new Label();
		label2 = new Label();
		label3 = new Label();
		ChkAll = new CheckBox();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		groupBox1 = new GroupBox();
		flowLayoutPanel1 = new FlowLayoutPanel();
		groupBox5 = new GroupBox();
		rbtnMission = new RadioButton();
		rbtnOverTime = new RadioButton();
		rbtnLeave = new RadioButton();
		groupBox2 = new GroupBox();
		grpType = new GroupBox();
		groupBox3 = new GroupBox();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		DateBoxFrom = new DateBoxFa();
		DateBoxTo = new DateBoxFa();
		serverStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((ISupportInitialize)dgvBindingObj).BeginInit();
		((Control)rightClickMenuStrip).SuspendLayout();
		((ISupportInitialize)BindingSourceAttPersonLeaveBusiness).BeginInit();
		((Control)groupBox1).SuspendLayout();
		((Control)flowLayoutPanel1).SuspendLayout();
		((Control)groupBox5).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
		((Control)grpType).SuspendLayout();
		((Control)groupBox3).SuspendLayout();
		((Control)this).SuspendLayout();
		dgvBindingObj.AllowUserToAddRows = false;
		dgvBindingObj.AllowUserToDeleteRows = false;
		((Control)dgvBindingObj).Anchor = (AnchorStyles)15;
		dgvBindingObj.AutoGenerateColumns = false;
		dgvBindingObj.BackgroundColor = SystemColors.ActiveCaption;
		dgvBindingObj.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dgvBindingObj.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[23]
		{
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)personGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)dgIsChecked,
			(DataGridViewColumn)QuiddityForDisplay,
			(DataGridViewColumn)FullName,
			(DataGridViewColumn)requestDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateFromDataGridViewTextBoxColumn,
			(DataGridViewColumn)TimeFromSh,
			(DataGridViewColumn)dateToDataGridViewTextBoxColumn,
			(DataGridViewColumn)TimeToSh,
			(DataGridViewColumn)managerDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)dgType,
			(DataGridViewColumn)statusDataGridViewTextBoxColumn,
			(DataGridViewColumn)dgKind,
			(DataGridViewColumn)reasonDataGridViewTextBoxColumn,
			(DataGridViewColumn)managerMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)contextGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)serverStatusDataGridViewTextBoxColumn
		});
		((Control)dgvBindingObj).ContextMenuStrip = rightClickMenuStrip;
		dgvBindingObj.DataSource = BindingSourceAttPersonLeaveBusiness;
		((Control)dgvBindingObj).Location = new Point(0, 162);
		((Control)dgvBindingObj).Margin = new Padding(3, 4, 3, 4);
		dgvBindingObj.MultiSelect = false;
		((Control)dgvBindingObj).Name = "dgvBindingObj";
		dgvBindingObj.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dgvBindingObj).Size = new Size(1232, 255);
		((Control)dgvBindingObj).TabIndex = 0;
		((Control)dgvBindingObj).KeyDown += new KeyEventHandler(dgvBindingObj_KeyDown);
		((Control)dgvBindingObj).MouseClick += new MouseEventHandler(dgvBindingObj_MouseClick);
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)personGuidDataGridViewTextBoxColumn).DataPropertyName = "PersonGuid";
		((DataGridViewColumn)personGuidDataGridViewTextBoxColumn).HeaderText = "PersonGuid";
		((DataGridViewColumn)personGuidDataGridViewTextBoxColumn).Name = "personGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)personGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dgIsChecked).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgIsChecked).DataPropertyName = "IsChecked";
		((DataGridViewColumn)dgIsChecked).HeaderText = "";
		((DataGridViewColumn)dgIsChecked).Name = "dgIsChecked";
		((DataGridViewColumn)dgIsChecked).Width = 5;
		((DataGridViewColumn)QuiddityForDisplay).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)QuiddityForDisplay).DataPropertyName = "QuiddityForDisplay";
		((DataGridViewColumn)QuiddityForDisplay).HeaderText = "ماهیت";
		((DataGridViewColumn)QuiddityForDisplay).Name = "QuiddityForDisplay";
		((DataGridViewBand)QuiddityForDisplay).ReadOnly = true;
		((DataGridViewColumn)QuiddityForDisplay).Width = 60;
		((DataGridViewColumn)FullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)FullName).DataPropertyName = "FullName";
		((DataGridViewColumn)FullName).HeaderText = "نام و نام خانوادگی";
		((DataGridViewColumn)FullName).Name = "FullName";
		((DataGridViewBand)FullName).ReadOnly = true;
		((DataGridViewColumn)FullName).Width = 109;
		((DataGridViewColumn)requestDateDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)requestDateDataGridViewTextBoxColumn).DataPropertyName = "RequestDateSh";
		((DataGridViewColumn)requestDateDataGridViewTextBoxColumn).HeaderText = "تاریخ درخواست";
		((DataGridViewColumn)requestDateDataGridViewTextBoxColumn).Name = "requestDateDataGridViewTextBoxColumn";
		((DataGridViewBand)requestDateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)requestDateDataGridViewTextBoxColumn).Width = 95;
		((DataGridViewColumn)dateFromDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateFromDataGridViewTextBoxColumn).DataPropertyName = "DateFromSh";
		((DataGridViewColumn)dateFromDataGridViewTextBoxColumn).HeaderText = "از تاریخ";
		((DataGridViewColumn)dateFromDataGridViewTextBoxColumn).Name = "dateFromDataGridViewTextBoxColumn";
		((DataGridViewBand)dateFromDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateFromDataGridViewTextBoxColumn).Width = 63;
		((DataGridViewColumn)TimeFromSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TimeFromSh).DataPropertyName = "DateFrom";
		val.Format = "HH:mm";
		val.NullValue = "00:00";
		((DataGridViewBand)TimeFromSh).DefaultCellStyle = val;
		((DataGridViewColumn)TimeFromSh).HeaderText = "از ساعت";
		((DataGridViewColumn)TimeFromSh).Name = "TimeFromSh";
		((DataGridViewBand)TimeFromSh).ReadOnly = true;
		((DataGridViewColumn)TimeFromSh).Width = 66;
		((DataGridViewColumn)dateToDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateToDataGridViewTextBoxColumn).DataPropertyName = "DateToSh";
		((DataGridViewColumn)dateToDataGridViewTextBoxColumn).HeaderText = "تا تاریخ ";
		((DataGridViewColumn)dateToDataGridViewTextBoxColumn).Name = "dateToDataGridViewTextBoxColumn";
		((DataGridViewBand)dateToDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateToDataGridViewTextBoxColumn).Width = 66;
		((DataGridViewColumn)TimeToSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TimeToSh).DataPropertyName = "DateTo";
		val2.Format = "HH:mm";
		val2.NullValue = "00:00";
		((DataGridViewBand)TimeToSh).DefaultCellStyle = val2;
		((DataGridViewColumn)TimeToSh).HeaderText = "تا ساعت";
		((DataGridViewColumn)TimeToSh).Name = "TimeToSh";
		((DataGridViewBand)TimeToSh).ReadOnly = true;
		((DataGridViewColumn)TimeToSh).Width = 66;
		((DataGridViewColumn)managerDateDataGridViewTextBoxColumn).DataPropertyName = "ManagerDateSh";
		((DataGridViewColumn)managerDateDataGridViewTextBoxColumn).HeaderText = "ManagerDate";
		((DataGridViewColumn)managerDateDataGridViewTextBoxColumn).Name = "managerDateDataGridViewTextBoxColumn";
		((DataGridViewBand)managerDateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)managerDateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dgType).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgType).DataPropertyName = "TypeDisplay";
		((DataGridViewColumn)dgType).HeaderText = "دوره";
		((DataGridViewColumn)dgType).Name = "dgType";
		((DataGridViewBand)dgType).ReadOnly = true;
		((DataGridViewColumn)dgType).Width = 54;
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).DataPropertyName = "StatusDisplay";
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).HeaderText = "وضعیت";
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).Name = "statusDataGridViewTextBoxColumn";
		((DataGridViewBand)statusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).Width = 65;
		((DataGridViewColumn)dgKind).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgKind).DataPropertyName = "KindDisplay";
		((DataGridViewColumn)dgKind).HeaderText = "نوع";
		((DataGridViewColumn)dgKind).Name = "dgKind";
		((DataGridViewBand)dgKind).ReadOnly = true;
		((DataGridViewColumn)dgKind).Width = 49;
		((DataGridViewColumn)reasonDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)reasonDataGridViewTextBoxColumn).DataPropertyName = "Reason";
		((DataGridViewColumn)reasonDataGridViewTextBoxColumn).HeaderText = "دلیل";
		((DataGridViewColumn)reasonDataGridViewTextBoxColumn).Name = "reasonDataGridViewTextBoxColumn";
		((DataGridViewBand)reasonDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)managerMessageDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)managerMessageDataGridViewTextBoxColumn).DataPropertyName = "ManagerMessage";
		((DataGridViewColumn)managerMessageDataGridViewTextBoxColumn).HeaderText = "پیام مدیریت";
		((DataGridViewColumn)managerMessageDataGridViewTextBoxColumn).Name = "managerMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)managerMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).DataPropertyName = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).HeaderText = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).Name = "contextGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).Name = "deliveryMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).Name = "deliveryShortMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryShortMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).DataPropertyName = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).HeaderText = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).Name = "hostRefrencedDataGridViewTextBoxColumn";
		((DataGridViewBand)hostRefrencedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).DataPropertyName = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).HeaderText = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).Name = "deliveryDateDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryDateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).DataPropertyName = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).HeaderText = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).Name = "deliveryIdDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryIdDataGridViewTextBoxColumn).Visible = false;
		((Control)rightClickMenuStrip).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStrip)rightClickMenuStrip).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[6]
		{
			(ToolStripItem)EditMenuStrip,
			(ToolStripItem)ConfirmMenuStrip,
			(ToolStripItem)MenuStripReject,
			(ToolStripItem)MenuStripRemove,
			(ToolStripItem)toolStripMenuItem1,
			(ToolStripItem)انتخابToolStripMenuItem
		});
		((Control)rightClickMenuStrip).Name = "rightClickMenuStrip";
		((Control)rightClickMenuStrip).RightToLeft = (RightToLeft)1;
		((Control)rightClickMenuStrip).Size = new Size(194, 130);
		((ToolStripDropDown)rightClickMenuStrip).Opening += rightClickMenuStrip_Opening;
		((ToolStripItem)EditMenuStrip).Name = "EditMenuStrip";
		((ToolStripItem)EditMenuStrip).Size = new Size(193, 24);
		((ToolStripItem)EditMenuStrip).Text = "ویرایش / رد درخواست";
		((ToolStripItem)EditMenuStrip).Click += EditMenuStrip_Click;
		((ToolStripItem)ConfirmMenuStrip).Name = "ConfirmMenuStrip";
		((ToolStripItem)ConfirmMenuStrip).Size = new Size(193, 24);
		((ToolStripItem)ConfirmMenuStrip).Text = "تائید و ثبت درخواست";
		((ToolStripItem)ConfirmMenuStrip).Click += ConfirmMenuStrip_Click_1;
		((ToolStripItem)MenuStripReject).Name = "MenuStripReject";
		((ToolStripItem)MenuStripReject).Size = new Size(193, 24);
		((ToolStripItem)MenuStripReject).Text = "رد درخواست - آنی";
		((ToolStripItem)MenuStripReject).Click += MenuStripReject_Click;
		((ToolStripItem)MenuStripRemove).Name = "MenuStripRemove";
		((ToolStripItem)MenuStripRemove).Size = new Size(193, 24);
		((ToolStripItem)MenuStripRemove).Text = "حذف درخواست";
		((ToolStripItem)MenuStripRemove).Click += MenuStripRemove_Click;
		((ToolStripItem)toolStripMenuItem1).Name = "toolStripMenuItem1";
		((ToolStripItem)toolStripMenuItem1).Size = new Size(190, 6);
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)mnuSelectAll,
			(ToolStripItem)mnuSelectNone,
			(ToolStripItem)mnuReverse
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(193, 24);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "انتخاب";
		((ToolStripItem)mnuSelectAll).Name = "mnuSelectAll";
		((ToolStripItem)mnuSelectAll).Size = new Size(178, 24);
		((ToolStripItem)mnuSelectAll).Text = "انتخاب همه";
		((ToolStripItem)mnuSelectAll).Click += mnuSelectAll_Click;
		((ToolStripItem)mnuSelectNone).Name = "mnuSelectNone";
		((ToolStripItem)mnuSelectNone).Size = new Size(178, 24);
		((ToolStripItem)mnuSelectNone).Text = "انتخاب هیچکدام";
		((ToolStripItem)mnuSelectNone).Click += mnuSelectNone_Click;
		((ToolStripItem)mnuReverse).Name = "mnuReverse";
		((ToolStripItem)mnuReverse).Size = new Size(178, 24);
		((ToolStripItem)mnuReverse).Text = "معکوس کردن انتخاب";
		((ToolStripItem)mnuReverse).Click += mnuReverse_Click;
		BindingSourceAttPersonLeaveBusiness.DataSource = typeof(AttPersonLeaveBusiness);
		BindingSourceAttPersonLeaveBusiness.CurrentChanged += BindingSourceAttPersonLeaveBusiness_CurrentChanged;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).Visible = false;
		((Control)chbPending).Anchor = (AnchorStyles)9;
		((Control)chbPending).AutoSize = true;
		chbPending.Checked = true;
		chbPending.CheckState = (CheckState)1;
		((Control)chbPending).Cursor = Cursors.Hand;
		((Control)chbPending).Location = new Point(265, 25);
		((Control)chbPending).Name = "chbPending";
		((Control)chbPending).Size = new Size(87, 17);
		((Control)chbPending).TabIndex = 1;
		((Control)chbPending).Text = "درانتظار تایید";
		((ButtonBase)chbPending).UseVisualStyleBackColor = true;
		chbPending.CheckedChanged += chbAll_CheckedChanged;
		((Control)chbAcceped).Anchor = (AnchorStyles)9;
		((Control)chbAcceped).AutoSize = true;
		((Control)chbAcceped).Cursor = Cursors.Hand;
		((Control)chbAcceped).Location = new Point(181, 25);
		((Control)chbAcceped).Name = "chbAcceped";
		((Control)chbAcceped).Size = new Size(70, 17);
		((Control)chbAcceped).TabIndex = 1;
		((Control)chbAcceped).Text = "تایید شده";
		((ButtonBase)chbAcceped).UseVisualStyleBackColor = true;
		chbAcceped.CheckedChanged += chbAll_CheckedChanged;
		((Control)chbFailed).Anchor = (AnchorStyles)9;
		((Control)chbFailed).AutoSize = true;
		((Control)chbFailed).Cursor = Cursors.Hand;
		((Control)chbFailed).Location = new Point(104, 25);
		((Control)chbFailed).Name = "chbFailed";
		((Control)chbFailed).Size = new Size(59, 17);
		((Control)chbFailed).TabIndex = 1;
		((Control)chbFailed).Text = "رد شده";
		((ButtonBase)chbFailed).UseVisualStyleBackColor = true;
		chbFailed.CheckedChanged += chbAll_CheckedChanged;
		((Control)chbRemoved).Anchor = (AnchorStyles)9;
		((Control)chbRemoved).AutoSize = true;
		((Control)chbRemoved).Cursor = Cursors.Hand;
		((Control)chbRemoved).Location = new Point(7, 25);
		((Control)chbRemoved).Name = "chbRemoved";
		((Control)chbRemoved).Size = new Size(70, 17);
		((Control)chbRemoved).TabIndex = 1;
		((Control)chbRemoved).Text = "حذف شده";
		((ButtonBase)chbRemoved).UseVisualStyleBackColor = true;
		chbRemoved.CheckedChanged += chbAll_CheckedChanged;
		((Control)ChbDaily).Anchor = (AnchorStyles)9;
		((Control)ChbDaily).AutoSize = true;
		ChbDaily.Checked = true;
		ChbDaily.CheckState = (CheckState)1;
		((Control)ChbDaily).Cursor = Cursors.Hand;
		((Control)ChbDaily).Location = new Point(5, 25);
		((Control)ChbDaily).Name = "ChbDaily";
		((Control)ChbDaily).Size = new Size(54, 17);
		((Control)ChbDaily).TabIndex = 2;
		((Control)ChbDaily).Text = "روزانه";
		((ButtonBase)ChbDaily).UseVisualStyleBackColor = true;
		ChbDaily.CheckedChanged += chbAll_CheckedChanged;
		((Control)ChbHourly).Anchor = (AnchorStyles)9;
		((Control)ChbHourly).AutoSize = true;
		ChbHourly.Checked = true;
		ChbHourly.CheckState = (CheckState)1;
		((Control)ChbHourly).Cursor = Cursors.Hand;
		((Control)ChbHourly).Location = new Point(85, 25);
		((Control)ChbHourly).Name = "ChbHourly";
		((Control)ChbHourly).Size = new Size(60, 17);
		((Control)ChbHourly).TabIndex = 3;
		((Control)ChbHourly).Text = "ساعتی";
		((ButtonBase)ChbHourly).UseVisualStyleBackColor = true;
		ChbHourly.CheckedChanged += chbAll_CheckedChanged;
		((Control)TbxSerach).Anchor = (AnchorStyles)13;
		((Control)TbxSerach).Location = new Point(4, 135);
		((Control)TbxSerach).Name = "TbxSerach";
		((Control)TbxSerach).Size = new Size(1171, 20);
		((Control)TbxSerach).TabIndex = 4;
		((Control)TbxSerach).TextChanged += TbxSerach_TextChanged;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(1181, 139);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(43, 13);
		((Control)label1).TabIndex = 5;
		((Control)label1).Text = "جستجو:";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(252, 27);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(18, 13);
		((Control)label2).TabIndex = 8;
		((Control)label2).Text = "از:";
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).Location = new Point(106, 27);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(18, 13);
		((Control)label3).TabIndex = 8;
		((Control)label3).Text = "تا:";
		((Control)ChkAll).Anchor = (AnchorStyles)9;
		((Control)ChkAll).AutoSize = true;
		((Control)ChkAll).Cursor = Cursors.Hand;
		((Control)ChkAll).Location = new Point(372, 25);
		((Control)ChkAll).Name = "ChkAll";
		((Control)ChkAll).Size = new Size(43, 17);
		((Control)ChkAll).TabIndex = 9;
		((Control)ChkAll).Text = "همه";
		((ButtonBase)ChkAll).UseVisualStyleBackColor = true;
		ChkAll.CheckedChanged += ChkAll_CheckedChanged;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).Visible = false;
		((Control)groupBox1).Anchor = (AnchorStyles)13;
		((Control)groupBox1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)groupBox1).Location = new Point(1, 1);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(1227, 129);
		((Control)groupBox1).TabIndex = 10;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "فیلتر";
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)groupBox5);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)groupBox2);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)groupBox3);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)grpType);
		((Control)flowLayoutPanel1).Dock = (DockStyle)5;
		((Control)flowLayoutPanel1).Location = new Point(3, 16);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(1221, 110);
		((Control)flowLayoutPanel1).TabIndex = 0;
		((Control)groupBox5).Anchor = (AnchorStyles)9;
		((Control)groupBox5).Controls.Add((Control)(object)rbtnMission);
		((Control)groupBox5).Controls.Add((Control)(object)rbtnOverTime);
		((Control)groupBox5).Controls.Add((Control)(object)rbtnLeave);
		((Control)groupBox5).Location = new Point(978, 3);
		((Control)groupBox5).Name = "groupBox5";
		((Control)groupBox5).Size = new Size(240, 53);
		((Control)groupBox5).TabIndex = 0;
		groupBox5.TabStop = false;
		((Control)groupBox5).Text = "ماهیت";
		((Control)rbtnMission).Anchor = (AnchorStyles)9;
		((Control)rbtnMission).AutoSize = true;
		((Control)rbtnMission).Cursor = Cursors.Hand;
		((Control)rbtnMission).Location = new Point(6, 25);
		((Control)rbtnMission).Name = "rbtnMission";
		((Control)rbtnMission).Size = new Size(62, 17);
		((Control)rbtnMission).TabIndex = 2;
		((Control)rbtnMission).Text = "ماموریت";
		((ButtonBase)rbtnMission).UseVisualStyleBackColor = true;
		rbtnMission.CheckedChanged += rbtnLeave_CheckedChanged;
		((Control)rbtnOverTime).Anchor = (AnchorStyles)9;
		((Control)rbtnOverTime).AutoSize = true;
		((Control)rbtnOverTime).Cursor = Cursors.Hand;
		((Control)rbtnOverTime).Location = new Point(159, 25);
		((Control)rbtnOverTime).Name = "rbtnOverTime";
		((Control)rbtnOverTime).Size = new Size(71, 17);
		((Control)rbtnOverTime).TabIndex = 2;
		((Control)rbtnOverTime).Text = "اضافه کار";
		((ButtonBase)rbtnOverTime).UseVisualStyleBackColor = true;
		rbtnOverTime.CheckedChanged += rbtnLeave_CheckedChanged;
		((Control)rbtnLeave).Anchor = (AnchorStyles)9;
		((Control)rbtnLeave).AutoSize = true;
		rbtnLeave.Checked = true;
		((Control)rbtnLeave).Cursor = Cursors.Hand;
		((Control)rbtnLeave).Location = new Point(84, 25);
		((Control)rbtnLeave).Name = "rbtnLeave";
		((Control)rbtnLeave).Size = new Size(61, 17);
		((Control)rbtnLeave).TabIndex = 3;
		rbtnLeave.TabStop = true;
		((Control)rbtnLeave).Text = "مرخصی";
		((ButtonBase)rbtnLeave).UseVisualStyleBackColor = true;
		rbtnLeave.CheckedChanged += rbtnLeave_CheckedChanged;
		((Control)groupBox2).Anchor = (AnchorStyles)9;
		((Control)groupBox2).Controls.Add((Control)(object)chbPending);
		((Control)groupBox2).Controls.Add((Control)(object)chbAcceped);
		((Control)groupBox2).Controls.Add((Control)(object)ChkAll);
		((Control)groupBox2).Controls.Add((Control)(object)chbFailed);
		((Control)groupBox2).Controls.Add((Control)(object)chbRemoved);
		((Control)groupBox2).Location = new Point(547, 3);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(425, 53);
		((Control)groupBox2).TabIndex = 0;
		groupBox2.TabStop = false;
		((Control)groupBox2).Text = "وضعیت";
		((Control)grpType).Anchor = (AnchorStyles)9;
		((Control)grpType).Controls.Add((Control)(object)ChbDaily);
		((Control)grpType).Controls.Add((Control)(object)ChbHourly);
		((Control)grpType).Location = new Point(84, 3);
		((Control)grpType).Name = "grpType";
		((Control)grpType).Size = new Size(161, 53);
		((Control)grpType).TabIndex = 0;
		grpType.TabStop = false;
		((Control)grpType).Text = "نوع مرخصی";
		((Control)groupBox3).Anchor = (AnchorStyles)13;
		((Control)groupBox3).Controls.Add((Control)(object)DateBoxFrom);
		((Control)groupBox3).Controls.Add((Control)(object)DateBoxTo);
		((Control)groupBox3).Controls.Add((Control)(object)label3);
		((Control)groupBox3).Controls.Add((Control)(object)label2);
		((Control)groupBox3).Location = new Point(251, 3);
		((Control)groupBox3).Name = "groupBox3";
		((Control)groupBox3).Size = new Size(290, 53);
		((Control)groupBox3).TabIndex = 0;
		groupBox3.TabStop = false;
		((Control)groupBox3).Text = "بازه مرخصی";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "ServerStatus";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).Visible = false;
		DateBoxFrom.AllowNull = false;
		((Control)DateBoxFrom).Anchor = (AnchorStyles)9;
		DateBoxFrom.BackcolorTooltip = Color.Empty;
		((TextBoxX)DateBoxFrom).Border.Class = "TextBoxBorder";
		((TextBoxX)DateBoxFrom).Border.CornerType = (eCornerType)1;
		DateBoxFrom.EnterToTab = true;
		DateBoxFrom.EscToClose = true;
		DateBoxFrom.FarsyDateStyle = (ENFarsyDateStyle)0;
		DateBoxFrom.FocusBackColor = Color.Yellow;
		DateBoxFrom.FocusFont = new Font("Tahoma", 9f);
		DateBoxFrom.FocusForeColor = Color.Blue;
		DateBoxFrom.FocusTextSelect = true;
		((Control)DateBoxFrom).Font = new Font("Tahoma", 9f);
		((Control)DateBoxFrom).Location = new Point(153, 20);
		DateBoxFrom.MessageEmptyShowDialog = false;
		DateBoxFrom.MessegeEmpty = "";
		DateBoxFrom.MessegeEmptyInFormRight = true;
		DateBoxFrom.MessegeEmptyShowInForm = false;
		DateBoxFrom.MiladiDate = new DateTime(2025, 6, 29, 8, 0, 29, 951);
		((Control)DateBoxFrom).Name = "DateBoxFrom";
		((TextBoxX)DateBoxFrom).PreventEnterBeep = true;
		((Control)DateBoxFrom).RightToLeft = (RightToLeft)0;
		DateBoxFrom.ShowNameOfDayOnToolTip = false;
		DateBoxFrom.ShowToolTipText = false;
		((Control)DateBoxFrom).Size = new Size(95, 26);
		((Control)DateBoxFrom).TabIndex = 7;
		((Control)DateBoxFrom).Text = "28/06/2025";
		((TextBox)DateBoxFrom).TextAlign = (HorizontalAlignment)2;
		DateBoxFrom.TootTipDelay = 0;
		((Control)DateBoxFrom).TextChanged += dateBoxFa1_TextChanged;
		DateBoxTo.AllowNull = false;
		((Control)DateBoxTo).Anchor = (AnchorStyles)9;
		DateBoxTo.BackcolorTooltip = Color.Empty;
		((TextBoxX)DateBoxTo).Border.Class = "TextBoxBorder";
		((TextBoxX)DateBoxTo).Border.CornerType = (eCornerType)1;
		DateBoxTo.EnterToTab = true;
		DateBoxTo.EscToClose = true;
		DateBoxTo.FarsyDateStyle = (ENFarsyDateStyle)0;
		DateBoxTo.FocusBackColor = Color.Yellow;
		DateBoxTo.FocusFont = new Font("Tahoma", 9f);
		DateBoxTo.FocusForeColor = Color.Blue;
		DateBoxTo.FocusTextSelect = true;
		((Control)DateBoxTo).Font = new Font("Tahoma", 9f);
		((Control)DateBoxTo).Location = new Point(8, 20);
		DateBoxTo.MessageEmptyShowDialog = false;
		DateBoxTo.MessegeEmpty = "";
		DateBoxTo.MessegeEmptyInFormRight = true;
		DateBoxTo.MessegeEmptyShowInForm = false;
		DateBoxTo.MiladiDate = new DateTime(2025, 6, 28, 20, 9, 57, 953);
		((Control)DateBoxTo).Name = "DateBoxTo";
		((TextBoxX)DateBoxTo).PreventEnterBeep = true;
		((Control)DateBoxTo).RightToLeft = (RightToLeft)0;
		DateBoxTo.ShowNameOfDayOnToolTip = false;
		DateBoxTo.ShowToolTipText = false;
		((Control)DateBoxTo).Size = new Size(95, 26);
		((Control)DateBoxTo).TabIndex = 6;
		((Control)DateBoxTo).Text = "28/06/2025";
		((TextBox)DateBoxTo).TextAlign = (HorizontalAlignment)2;
		DateBoxTo.TootTipDelay = 0;
		((Control)DateBoxTo).TextChanged += dateBoxFa1_TextChanged;
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).HeaderText = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).Name = "serverStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)serverStatusDataGridViewTextBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(175, 210, 255);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)TbxSerach);
		((Control)this).Controls.Add((Control)(object)dgvBindingObj);
		((Control)this).Font = new Font("Microsoft Sans Serif", 8.25f);
		((Control)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "UcAttPersonLeave";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(1232, 418);
		((ISupportInitialize)dgvBindingObj).EndInit();
		((Control)rightClickMenuStrip).ResumeLayout(false);
		((ISupportInitialize)BindingSourceAttPersonLeaveBusiness).EndInit();
		((Control)groupBox1).ResumeLayout(false);
		((Control)flowLayoutPanel1).ResumeLayout(false);
		((Control)groupBox5).ResumeLayout(false);
		((Control)groupBox5).PerformLayout();
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)grpType).ResumeLayout(false);
		((Control)grpType).PerformLayout();
		((Control)groupBox3).ResumeLayout(false);
		((Control)groupBox3).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
