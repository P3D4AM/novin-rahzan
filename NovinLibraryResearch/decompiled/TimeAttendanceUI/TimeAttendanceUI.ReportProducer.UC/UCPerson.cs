using System;
using System.Collections;
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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCPerson : UserControl, IHasContext
{
	private Font _font;

	private List<PersonGroup> _lstPersonGroup;

	private List<Per_KhedmatPlace> _lstPerKhedmatPlace;

	private List<Person> _listData;

	private SortableBindingList<Person> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx PanelButtomHesabDes;

	internal DataGridViewX DGridGroup;

	internal DataGridViewX DGrid;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private TabItem tabItem1;

	private TabControlPanel tabControlPanel2;

	internal DataGridViewX dataGrid;

	private TabItem tabItem2;

	private LabelX labelX2;

	private TextBoxX TxtSearch;

	private BindingSource persongroupBindingSource;

	private BindingSource personBindingSource;

	private BindingSource perKhedmatPlaceBindingSource;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem SelectAllTSMI;

	private ToolStripMenuItem UnSelectAllTSMI;

	private ToolStripMenuItem ReverseSelectAllDetTSMI;

	private DataGridViewCheckBoxColumn DGCheckGroup;

	private DataGridViewTextBoxColumn DGIDGroup;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DGIDPerson;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewCheckBoxColumn DGCheckKh;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGIDPer_KhedmatPlace;

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

	public short IdGroup
	{
		get
		{
			if (((DataGridView)DGridGroup).RowCount <= 0 || ((DataGridView)DGridGroup).CurrentRow == null)
			{
				return 0;
			}
			short result = 0;
			try
			{
				object value = ((DataGridView)DGridGroup)[((DataGridViewBand)DGIDGroup).Index, ((DataGridViewBand)((DataGridView)DGridGroup).CurrentRow).Index].Value;
				if (value != null)
				{
					short.TryParse(value.ToString(), out result);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IdGroup", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 199);
			}
			return result;
		}
	}

	public int IdPerson
	{
		get
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			short result = 0;
			try
			{
				object value = ((DataGridView)DGrid)[((DataGridViewBand)DGIDPerson).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
				if (value != null)
				{
					short.TryParse(value.ToString(), out result);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return result;
		}
	}

	public short IdPerKhedmatPlace
	{
		get
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			short result = 0;
			try
			{
				if (((DataGridView)dataGrid).CurrentRow != null)
				{
					object value = ((DataGridView)dataGrid)[((DataGridViewBand)DGIDPer_KhedmatPlace).Index, ((DataGridViewBand)((DataGridView)dataGrid).CurrentRow).Index].Value;
					if (value != null)
					{
						short.TryParse(value.ToString(), out result);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return result;
		}
	}

	public List<int> LstSelected
	{
		get
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			List<int> list = new List<int>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if ((bool)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i].Value)
						{
							list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGIDPerson).Index, i].Value.ToString()));
						}
					}
				}
				if (list.Count == 0)
				{
					foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
					{
						DataGridViewRow val = item;
						list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGIDPerson).Index, ((DataGridViewBand)val).Index].Value.ToString()));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LstSelected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 262);
			}
			return list;
		}
	}

	public bool EnalbleControl
	{
		get
		{
			return ucPanel1.Expand;
		}
		set
		{
			try
			{
				ucPanel1.Expand = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 282);
			}
		}
	}

	public void SetContextGuid(string contextGuid)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new InvalidOperationException("contextGuid is null or rmpty!!");
			}
			ContextGuid = contextGuid;
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
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UCPerson()
	{
		try
		{
			_font = TimeAttendanceSettings.appFont;
			InitializeComponent();
			((Control)DGrid).Font = _font;
			((Control)DGrid).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 77);
		}
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			_lstPerKhedmatPlace = await Per_KhedmatPlace.SelectAllAsync(ContextGuid, "", token);
			_lstPersonGroup = await PersonGroup.SelectAllAsync(ContextGuid, containsAllGroups: true, token);
			_listData = await Person.GetAllAsync(ContextGuid, new List<int>(), personStatus: true, token);
			if (!IsDisposed && !token.IsCancellationRequested)
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

	protected void Search()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Control)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 109);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await Person.GetAllAsync(ContextGuid, new List<int>(), personStatus: true, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				UCPerson uCPerson = this;
				List<Person> listData = _listData;
				uCPerson._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<Person>((IEnumerable<Person>)listData, token) : null);
			}
			else
			{
				IEnumerable<Person> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((Person p) => ((PersonBase)p).FullName.Contains(word) || ((PersonBase)p).CodeTafsil.ToString().Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<Person>(enumerable, token);
			}
			_ = ((Collection<Person>)(object)_listForDisplay).Count;
			Utilities.GetTranslate("[تعداد رکوردها]");
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
						personBindingSource.DataSource = _listForDisplay;
						persongroupBindingSource.DataSource = Utilities.ToSortableBindingList<PersonGroup>((IEnumerable<PersonGroup>)_lstPersonGroup, token);
						((Control)PanelButtomHesabDes).Top = 0;
						((Control)ucPanel1.panelMain).Controls.Add((Control)(object)PanelButtomHesabDes);
						((Control)this).Size = ((Control)ucPanel1).Size;
						tabControl1.SelectedTab = tabItem1;
						ucPanel1.HeaderText2 = (EnalbleControl ? "کلیه پرسنل" : "کلیه پرسنل - غیر فعال");
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

	public void VisTab()
	{
		tabItem2.Visible = true;
	}

	public void CheckKoli()
	{
		try
		{
			if (_lstPersonGroup != null)
			{
				foreach (PersonGroup item in _lstPersonGroup.Where((PersonGroup item) => item != null))
				{
					item.Check = true;
				}
			}
			if (_listData == null)
			{
				return;
			}
			foreach (Person item2 in _listData.Where((Person item) => item != null))
			{
				item2.Check = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckKoli", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 182);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DGCheck).Index].Value = !Convert.ToBoolean(((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DGCheck).Index].Value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 301);
		}
	}

	private void DGridGroup_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGridGroup).RowCount == 0 || e.RowIndex < 0)
			{
				return;
			}
			if (IdGroup == -1)
			{
				_lstPersonGroup[0].Check = !_lstPersonGroup[0].Check;
				foreach (PersonGroup item in _lstPersonGroup)
				{
					item.Check = _lstPersonGroup[0].Check;
				}
			}
			else
			{
				_lstPersonGroup[e.RowIndex].Check = !_lstPersonGroup[e.RowIndex].Check;
				if (!_lstPersonGroup[e.RowIndex].Check)
				{
					_lstPersonGroup[0].Check = false;
				}
			}
			persongroupBindingSource.DataSource = _lstPersonGroup.Select((PersonGroup e1) => e1).ToList();
			int i;
			for (i = 1; i < _lstPersonGroup.Count; i++)
			{
				foreach (Person item2 in _listData.Where((Person person) => ((PersonBase)person).GroupId == _lstPersonGroup[i].Id))
				{
					item2.Check = _lstPersonGroup[i].Check;
				}
			}
			personBindingSource.DataSource = _listData.Select((Person e1) => e1).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 337);
		}
	}

	private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if ((DGrid != null && ((DataGridView)DGrid).RowCount == 0) || e.RowIndex < 0 || e.ColumnIndex != ((DataGridViewBand)DGCheck).Index || _lstPerKhedmatPlace == null)
			{
				return;
			}
			for (int i = 0; i <= _lstPerKhedmatPlace.Count - 1; i++)
			{
				if (_lstPerKhedmatPlace[i].ID == IdPerKhedmatPlace)
				{
					_lstPerKhedmatPlace[i].Check = !_lstPerKhedmatPlace[i].Check;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "dataGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 361);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		try
		{
			if (!(((ContainerControl)this).ActiveControl.Name != "DGrid") && Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = ((Control)TxtSearch).Text.Length;
				((Control)TxtSearch).Select();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 380);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Control)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 394);
		}
	}

	private void TxtPerson_Enter(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TxtPerson_Leave(object sender, EventArgs e)
	{
		((Control)TxtSearch).BackColor = Color.White;
	}

	private void SelectAllTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				if (((DataGridViewBand)DGCheck).Index >= 0 && ((BaseCollection)((DataGridView)DGrid).Rows[i].Cells).Count > ((DataGridViewBand)DGCheck).Index)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
					DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val != null)
					{
						((DataGridViewCell)val).Value = true;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectAllTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 423);
		}
	}

	private void UnSelectAllTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UnSelectAllTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 438);
		}
	}

	private void ReverseSelectAllDetTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null && (((DataGridViewCell)val).Value == null || !(bool)((DataGridViewCell)val).Value))
				{
					((DataGridViewCell)val).Value = true;
				}
				else if (val != null)
				{
					((DataGridViewCell)val).Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReverseSelectAllDetTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 456);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
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
			Keys keyData = e.KeyData;
			if ((int)keyData != 27)
			{
				if ((int)keyData == 38 || (int)keyData == 40)
				{
					e.Handled = true;
					((Control)DGrid).Select();
				}
			}
			else
			{
				e.Handled = true;
				((Control)TxtSearch).Text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 479);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyData == 32 && IdPerson >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
			{
				e.Handled = true;
				DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 493);
		}
	}

	private void DGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			DGrid_CellClick(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellDoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCPerson.cs", 505);
		}
	}

	private async void UCPerson_Load(object sender, EventArgs e)
	{
		await LoadDataAsync();
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
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
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Expected O, but got Unknown
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Expected O, but got Unknown
		//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1150: Unknown result type (might be due to invalid IL or missing references)
		//IL_115a: Expected O, but got Unknown
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1248: Unknown result type (might be due to invalid IL or missing references)
		//IL_1252: Expected O, but got Unknown
		//IL_13eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f5: Expected O, but got Unknown
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14df: Expected O, but got Unknown
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		PanelButtomHesabDes = new PanelEx();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		labelX2 = new LabelX();
		TxtSearch = new TextBoxX();
		DGridGroup = new DataGridViewX();
		DGCheckGroup = new DataGridViewCheckBoxColumn();
		DGIDGroup = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		persongroupBindingSource = new BindingSource(components);
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGIDPerson = new DataGridViewTextBoxColumn();
		DGFullName = new DataGridViewTextBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		SelectAllTSMI = new ToolStripMenuItem();
		UnSelectAllTSMI = new ToolStripMenuItem();
		ReverseSelectAllDetTSMI = new ToolStripMenuItem();
		personBindingSource = new BindingSource(components);
		tabItem1 = new TabItem(components);
		tabControlPanel2 = new TabControlPanel();
		dataGrid = new DataGridViewX();
		DGCheckKh = new DataGridViewCheckBoxColumn();
		nameDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		descDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGIDPer_KhedmatPlace = new DataGridViewTextBoxColumn();
		perKhedmatPlaceBindingSource = new BindingSource(components);
		tabItem2 = new TabItem(components);
		ucPanel1 = new UCPanel();
		((Control)PanelButtomHesabDes).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel1).SuspendLayout();
		((ISupportInitialize)DGridGroup).BeginInit();
		((ISupportInitialize)persongroupBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)tabControlPanel2).SuspendLayout();
		((ISupportInitialize)dataGrid).BeginInit();
		((ISupportInitialize)perKhedmatPlaceBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)PanelButtomHesabDes).Anchor = (AnchorStyles)15;
		PanelButtomHesabDes.CanvasColor = SystemColors.Control;
		PanelButtomHesabDes.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelButtomHesabDes).Controls.Add((Control)(object)tabControl1);
		PanelButtomHesabDes.DisabledBackColor = Color.Empty;
		((Control)PanelButtomHesabDes).Location = new Point(0, 30);
		((Control)PanelButtomHesabDes).Margin = new Padding(0);
		((Control)PanelButtomHesabDes).Name = "PanelButtomHesabDes";
		((Control)PanelButtomHesabDes).Size = new Size(600, 387);
		PanelButtomHesabDes.Style.Alignment = (StringAlignment)1;
		PanelButtomHesabDes.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelButtomHesabDes.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelButtomHesabDes.Style.Border = (eBorderType)1;
		PanelButtomHesabDes.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelButtomHesabDes.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelButtomHesabDes.Style.GradientAngle = 90;
		((Control)PanelButtomHesabDes).TabIndex = 4;
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel2);
		((Control)tabControl1).Location = new Point(0, 0);
		((Control)tabControl1).Name = "tabControl1";
		((Control)tabControl1).Padding = new Padding(3);
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 1;
		((Control)tabControl1).Size = new Size(600, 381);
		((Control)tabControl1).TabIndex = 6;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		tabControl1.Tabs.Add(tabItem2);
		((Control)tabControl1).Text = "tabControl1";
		((Control)tabControlPanel1).Anchor = (AnchorStyles)15;
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX2);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtSearch);
		((Control)tabControlPanel1).Controls.Add((Control)(object)DGridGroup);
		((Control)tabControlPanel1).Controls.Add((Control)(object)DGrid);
		((PanelEx)tabControlPanel1).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Margin = new Padding(0);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(3);
		((Control)tabControlPanel1).Size = new Size(600, 355);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 1;
		tabControlPanel1.TabItem = tabItem1;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(413, 6);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(182, 23);
		((Control)labelX2).TabIndex = 8;
		((Control)labelX2).Text = "[نــام یا کــد پرســنل] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(5, 6);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(402, 21);
		((Control)TxtSearch).TabIndex = 7;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).Enter += TxtPerson_Enter;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtPerson_Leave;
		((DataGridView)DGridGroup).AllowUserToAddRows = false;
		((DataGridView)DGridGroup).AllowUserToDeleteRows = false;
		((DataGridView)DGridGroup).AllowUserToResizeColumns = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridGroup).AlternatingRowsDefaultCellStyle = val;
		((Control)DGridGroup).Anchor = (AnchorStyles)11;
		((DataGridView)DGridGroup).AutoGenerateColumns = false;
		((DataGridView)DGridGroup).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridGroup).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGCheckGroup,
			(DataGridViewColumn)DGIDGroup,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn
		});
		((DataGridView)DGridGroup).DataSource = persongroupBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridGroup).DefaultCellStyle = val2;
		((DataGridView)DGridGroup).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridGroup).Location = new Point(413, 32);
		((Control)DGridGroup).Margin = new Padding(0);
		((DataGridView)DGridGroup).MultiSelect = false;
		((Control)DGridGroup).Name = "DGridGroup";
		((DataGridView)DGridGroup).ReadOnly = true;
		((DataGridView)DGridGroup).RowHeadersVisible = false;
		((DataGridView)DGridGroup).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridGroup).Size = new Size(185, 322);
		((Control)DGridGroup).TabIndex = 4;
		((DataGridView)DGridGroup).CellClick += new DataGridViewCellEventHandler(DGridGroup_CellClick);
		((DataGridViewColumn)DGCheckGroup).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheckGroup).DataPropertyName = "Check";
		((DataGridViewColumn)DGCheckGroup).HeaderText = "";
		((DataGridViewColumn)DGCheckGroup).MinimumWidth = 30;
		((DataGridViewColumn)DGCheckGroup).Name = "DGCheckGroup";
		((DataGridViewBand)DGCheckGroup).ReadOnly = true;
		((DataGridViewColumn)DGCheckGroup).Width = 30;
		((DataGridViewColumn)DGIDGroup).DataPropertyName = "Id";
		((DataGridViewColumn)DGIDGroup).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGIDGroup).Name = "DGIDGroup";
		((DataGridViewBand)DGIDGroup).ReadOnly = true;
		((DataGridViewBand)DGIDGroup).Visible = false;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[گروه]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		persongroupBindingSource.DataSource = typeof(PersonGroup);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val3.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val3;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGIDPerson,
			(DataGridViewColumn)DGFullName
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGrid).DataSource = personBindingSource;
		val4.Alignment = (DataGridViewContentAlignment)16;
		val4.BackColor = SystemColors.Window;
		val4.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val4.ForeColor = SystemColors.ControlText;
		val4.SelectionBackColor = SystemColors.Highlight;
		val4.SelectionForeColor = SystemColors.ControlText;
		val4.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val4;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(2, 32);
		((Control)DGrid).Margin = new Padding(0);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(405, 324);
		((Control)DGrid).TabIndex = 5;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellDoubleClick += new DataGridViewCellEventHandler(DGrid_CellDoubleClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).DataPropertyName = "Check";
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).MinimumWidth = 30;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewColumn)DGCheck).Width = 30;
		((DataGridViewColumn)DGIDPerson).DataPropertyName = "Id";
		((DataGridViewColumn)DGIDPerson).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGIDPerson).Name = "DGIDPerson";
		((DataGridViewBand)DGIDPerson).ReadOnly = true;
		((DataGridViewBand)DGIDPerson).Visible = false;
		((DataGridViewColumn)DGFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DGFullName).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)DGFullName).Name = "DGFullName";
		((DataGridViewBand)DGFullName).ReadOnly = true;
		((ToolStrip)contextMenuStrip1).ImageScalingSize = new Size(20, 20);
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)SelectAllTSMI,
			(ToolStripItem)UnSelectAllTSMI,
			(ToolStripItem)ReverseSelectAllDetTSMI
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(167, 70);
		((ToolStripItem)SelectAllTSMI).Name = "SelectAllTSMI";
		((ToolStripItem)SelectAllTSMI).Size = new Size(166, 22);
		((ToolStripItem)SelectAllTSMI).Text = "[انتخاب همه]";
		((ToolStripItem)SelectAllTSMI).Click += SelectAllTSMI_Click;
		((ToolStripItem)UnSelectAllTSMI).Name = "UnSelectAllTSMI";
		((ToolStripItem)UnSelectAllTSMI).Size = new Size(166, 22);
		((ToolStripItem)UnSelectAllTSMI).Text = "[پاک کردن همه]";
		((ToolStripItem)UnSelectAllTSMI).Click += UnSelectAllTSMI_Click;
		((ToolStripItem)ReverseSelectAllDetTSMI).Name = "ReverseSelectAllDetTSMI";
		((ToolStripItem)ReverseSelectAllDetTSMI).Size = new Size(166, 22);
		((ToolStripItem)ReverseSelectAllDetTSMI).Text = "[معکوس کردن انتخاب ها]";
		((ToolStripItem)ReverseSelectAllDetTSMI).Click += ReverseSelectAllDetTSMI_Click;
		personBindingSource.DataSource = typeof(Person);
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[انتخاب]";
		((Control)tabControlPanel2).Anchor = (AnchorStyles)15;
		((Control)tabControlPanel2).Controls.Add((Control)(object)dataGrid);
		((PanelEx)tabControlPanel2).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel2).Location = new Point(0, 26);
		((Control)tabControlPanel2).Margin = new Padding(0);
		((Control)tabControlPanel2).Name = "tabControlPanel2";
		((Control)tabControlPanel2).Padding = new Padding(3);
		((Control)tabControlPanel2).Size = new Size(600, 355);
		((PanelEx)tabControlPanel2).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel2).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel2).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel2).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel2).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel2).Style.GradientAngle = 90;
		((Control)tabControlPanel2).TabIndex = 2;
		tabControlPanel2.TabItem = tabItem2;
		((DataGridView)dataGrid).AllowUserToAddRows = false;
		((DataGridView)dataGrid).AllowUserToDeleteRows = false;
		((DataGridView)dataGrid).AllowUserToResizeRows = false;
		val5.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)dataGrid).AlternatingRowsDefaultCellStyle = val5;
		((Control)dataGrid).Anchor = (AnchorStyles)15;
		((DataGridView)dataGrid).AutoGenerateColumns = false;
		((DataGridView)dataGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)dataGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[4]
		{
			(DataGridViewColumn)DGCheckKh,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn1,
			(DataGridViewColumn)descDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGIDPer_KhedmatPlace
		});
		((DataGridView)dataGrid).DataSource = perKhedmatPlaceBindingSource;
		val6.Alignment = (DataGridViewContentAlignment)16;
		val6.BackColor = SystemColors.Window;
		val6.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val6.ForeColor = SystemColors.ControlText;
		val6.SelectionBackColor = SystemColors.Highlight;
		val6.SelectionForeColor = SystemColors.ControlText;
		val6.WrapMode = (DataGridViewTriState)2;
		((DataGridView)dataGrid).DefaultCellStyle = val6;
		((DataGridView)dataGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)dataGrid).Location = new Point(2, 5);
		((Control)dataGrid).Margin = new Padding(0);
		((DataGridView)dataGrid).MultiSelect = false;
		((Control)dataGrid).Name = "dataGrid";
		((DataGridView)dataGrid).ReadOnly = true;
		((DataGridView)dataGrid).RowHeadersVisible = false;
		((DataGridView)dataGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dataGrid).Size = new Size(596, 348);
		((Control)dataGrid).TabIndex = 6;
		((DataGridView)dataGrid).CellClick += new DataGridViewCellEventHandler(dataGrid_CellClick);
		((DataGridViewColumn)DGCheckKh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheckKh).DataPropertyName = "Check";
		((DataGridViewColumn)DGCheckKh).HeaderText = "";
		((DataGridViewColumn)DGCheckKh).MinimumWidth = 30;
		((DataGridViewColumn)DGCheckKh).Name = "DGCheckKh";
		((DataGridViewBand)DGCheckKh).ReadOnly = true;
		((DataGridViewColumn)DGCheckKh).Width = 30;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).HeaderText = "[نام]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).Name = "nameDataGridViewTextBoxColumn1";
		((DataGridViewBand)nameDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).DataPropertyName = "Desc";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).Name = "descDataGridViewTextBoxColumn";
		((DataGridViewBand)descDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)DGIDPer_KhedmatPlace).DataPropertyName = "ID";
		((DataGridViewColumn)DGIDPer_KhedmatPlace).HeaderText = "ID";
		((DataGridViewColumn)DGIDPer_KhedmatPlace).Name = "DGIDPer_KhedmatPlace";
		((DataGridViewBand)DGIDPer_KhedmatPlace).ReadOnly = true;
		perKhedmatPlaceBindingSource.DataSource = typeof(Per_KhedmatPlace);
		tabItem2.AttachedControl = (Control)(object)tabControlPanel2;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[مراکز خدمت]";
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Margin = new Padding(0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 420);
		((Control)ucPanel1).TabIndex = 0;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelButtomHesabDes);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(0);
		((Control)this).MaximumSize = new Size(600, 1000);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCPerson";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(600, 420);
		((UserControl)this).Load += UCPerson_Load;
		((Control)PanelButtomHesabDes).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel1).ResumeLayout(false);
		((ISupportInitialize)DGridGroup).EndInit();
		((ISupportInitialize)persongroupBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)tabControlPanel2).ResumeLayout(false);
		((ISupportInitialize)dataGrid).EndInit();
		((ISupportInitialize)perKhedmatPlaceBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
