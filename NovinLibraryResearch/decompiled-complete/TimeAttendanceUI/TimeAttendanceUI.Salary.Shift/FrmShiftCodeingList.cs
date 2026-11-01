using System;
using System.Collections;
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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftCodeingList : FrmExtList
{
	private TypeShiftEnumType _typeShift;

	private List<ShiftCodeing> DefaultShiftCoding;

	private List<ShiftCodeing> _listData;

	private List<ShiftCodeing> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private Font _font;

	private IContainer components;

	internal DataGridViewX DGrid;

	private BindingSource shiftCodeingBindingSource;

	private LabelX LblRowCount;

	private LabelX lblDescForShiftProg;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn h1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn m1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn h2DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn m2DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGShiftColor;

	private DataGridViewCheckBoxColumn nextDayDataGridViewCheckBoxColumn;

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

	public List<ShiftCodeing> ListShiftCodeingsSelected
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			List<ShiftCodeing> list = new List<ShiftCodeing>();
			try
			{
				if (((DataGridView)DGrid).RowCount <= 0)
				{
					return list;
				}
				for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
				{
					if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
					{
						list.Add(((List<ShiftCodeing>)shiftCodeingBindingSource.DataSource)[i]);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListShiftCodeingsSelected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 140);
			}
			return list;
		}
	}

	public ShiftCodeing SelectedRow { get; private set; }

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

	public FrmShiftCodeingList(string contextGuid, Font font)
		: base(contextGuid)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)base.LabelX1).Location = new Point(681, 2);
			((Control)base.TxtSearch).Size = new Size(672, 21);
			((Control)this).Text = "لیست انواع ترددها";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 74);
		}
	}

	public FrmShiftCodeingList(string contextGuid, Font font, bool isSelect, TypeShiftEnumType ptype, List<ShiftCodeing> pDefaultShiftCoding = null)
		: base(contextGuid)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		try
		{
			_font = font;
			InitializeComponent();
			DefaultShiftCoding = pDefaultShiftCoding;
			if (DefaultShiftCoding != null)
			{
				((DataGridViewBand)DGCheck).Visible = true;
				((Control)lblDescForShiftProg).Visible = true;
			}
			_typeShift = ptype;
			((Control)DGrid).Focus();
			((Control)base.BtnSelect).Visible = isSelect;
			((Control)DGrid).Font = font;
			((Control)base.LabelX1).Location = new Point(681, 2);
			((Control)base.TxtSearch).Size = new Size(672, 21);
			((Control)this).Text = "لیست انواع ترددها" + text;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 108);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await ShiftCodeing.GetAllAsync(((CacheBaseForm)this).ContextGuid, (TypeShiftEnumType)0, token);
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

	protected override void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 173);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await ShiftCodeing.GetAllAsync(((CacheBaseForm)this).ContextGuid, (TypeShiftEnumType)0, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				_listForDisplay = _listData;
			}
			else
			{
				IEnumerable<ShiftCodeing> source = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					source = source.Where((ShiftCodeing p) => p.Description.Contains(word));
				}
				_listForDisplay = source.ToList();
			}
			string countRecordTr = "تعداد ردیف";
			int count = _listForDisplay.Count;
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
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Expected O, but got Unknown
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						shiftCodeingBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = countRecordTr + " : " + count.ToString("N0");
						List<ShiftCodeing> defaultShiftCoding = DefaultShiftCoding;
						if (defaultShiftCoding != null && defaultShiftCoding.Any())
						{
							List<short> list = DefaultShiftCoding.Select((ShiftCodeing p) => p.Id).ToList();
							{
								foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
								{
									DataGridViewRow val = item;
									if (list.Contains(short.Parse(val.Cells[((DataGridViewBand)DGID).Index].Value.ToString())))
									{
										val.Cells[((DataGridViewBand)DGCheck).Index].Value = true;
									}
								}
								return;
							}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 274);
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
				if (((Control)DGrid).Focused && SelectedRow.Id >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 295);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && SelectedRow.Id >= 0)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 307);
		}
	}

	private void DGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		try
		{
			if (e.ColumnIndex == ((DataGridViewBand)DGShiftColor).Index && e.RowIndex >= 0 && ((DataGridView)DGrid)[((DataGridViewBand)DGShiftColor).Index, e.RowIndex].Value.ToString() != "")
			{
				((DataGridView)DGrid).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = ColorTranslator.FromHtml(((DataGridView)DGrid)[((DataGridViewBand)DGShiftColor).Index, e.RowIndex].Value.ToString());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellPainting", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 321);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected I4, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		try
		{
			if (!((Control)base.BtnAdd).Visible || !((Control)base.BtnAdd).Enabled)
			{
				return;
			}
			((Control)DGrid).Focus();
			TypeShiftEnumType typeShift = _typeShift;
			switch ((int)typeShift)
			{
			default:
				if ((int)typeShift != 100)
				{
					break;
				}
				goto case 3;
			case 0:
				if ((int)((Form)new FrmShiftCodeing(((CacheBaseForm)this).ContextGuid)).ShowDialog() == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
				break;
			case 1:
				if ((int)((Form)new FrmShiftCodeingNormal(((CacheBaseForm)this).ContextGuid)).ShowDialog() == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
				break;
			case 2:
				if ((int)((Form)new FrmShiftCodeingShenavar(((CacheBaseForm)this).ContextGuid, new ShiftCodeing())).ShowDialog() == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
				break;
			case 3:
			case 4:
			case 5:
			case 11:
				if ((int)((Form)new FrmShiftCodeing(((CacheBaseForm)this).ContextGuid, _typeShift)).ShowDialog() == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
				break;
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 374);
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
			if (SelectedRow.Id <= 0)
			{
				return;
			}
			_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			TypeShiftEnumType typeShift = _typeShift;
			if ((int)typeShift != 1)
			{
				if ((int)typeShift == 2)
				{
					string contextGuid = ((CacheBaseForm)this).ContextGuid;
					if ((int)((Form)new FrmShiftCodeingShenavar(contextGuid, await ShiftCodeing.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.Id, token))).ShowDialog() == 1)
					{
						_listData = null;
						((FrmExtList)this).Search();
					}
				}
				else
				{
					ShiftCodeing shiftCoding = await ShiftCodeing.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.Id);
					if ((int)((Form)new FrmShiftCodeing(((CacheBaseForm)this).ContextGuid, shiftCoding)).ShowDialog() == 1)
					{
						_listData = null;
						((FrmExtList)this).Search();
					}
				}
			}
			else if ((int)((Form)new FrmShiftCodeingNormal(((CacheBaseForm)this).ContextGuid, SelectedRow.Id)).ShowDialog() == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 426);
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
			if (SelectedRow != null && (int)FMessageBox.Show(Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				ShiftCodeing obj = await ShiftCodeing.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedRow.Id, token);
				obj.St = false;
				await obj.SaveAsync(token);
				_listData = null;
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingList.cs", 450);
		}
	}

	private void shiftCodeingBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedRow = (ShiftCodeing)shiftCodeingBindingSource.Current;
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGCheck = new DataGridViewCheckBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		h1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		m1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		h2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		m2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGShiftColor = new DataGridViewTextBoxColumn();
		nextDayDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		shiftCodeingBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		lblDescForShiftProg = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)shiftCodeingBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)lblDescForShiftProg);
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 562);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		base.PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)lblDescForShiftProg, 0);
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
		((Control)base.BtnDelete).Location = new Point(13, 192);
		((Control)base.BtnAdd).Location = new Point(13, 116);
		((Control)base.BtnEdit).Location = new Point(13, 154);
		base.BtnSelect.ImagePosition = (eImagePosition)1;
		((Control)base.BtnSelect).Location = new Point(13, 78);
		((Control)base.BtnSelect).Size = new Size(126, 32);
		((Control)base.BtnSelect).Text = "[انتخاب] (Enter)";
		((Control)base.BtnOther).Location = new Point(13, 230);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[9]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)h1DataGridViewTextBoxColumn,
			(DataGridViewColumn)m1DataGridViewTextBoxColumn,
			(DataGridViewColumn)h2DataGridViewTextBoxColumn,
			(DataGridViewColumn)m2DataGridViewTextBoxColumn,
			(DataGridViewColumn)DGShiftColor,
			(DataGridViewColumn)nextDayDataGridViewCheckBoxColumn
		});
		((DataGridView)DGrid).DataSource = shiftCodeingBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 78);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(622, 459);
		((Control)DGrid).TabIndex = 0;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellPainting += new DataGridViewCellPaintingEventHandler(DGrid_CellPainting);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCheck).HeaderText = "  ";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Visible = false;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)h1DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)h1DataGridViewTextBoxColumn).DataPropertyName = "StartShiftTime";
		val3.NullValue = null;
		((DataGridViewBand)h1DataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)h1DataGridViewTextBoxColumn).HeaderText = "[ساعت ورود]";
		((DataGridViewColumn)h1DataGridViewTextBoxColumn).Name = "h1DataGridViewTextBoxColumn";
		((DataGridViewBand)h1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)h1DataGridViewTextBoxColumn).Width = 105;
		((DataGridViewColumn)m1DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)m1DataGridViewTextBoxColumn).DataPropertyName = "M1";
		((DataGridViewColumn)m1DataGridViewTextBoxColumn).HeaderText = "[دقیقه]";
		((DataGridViewColumn)m1DataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)m1DataGridViewTextBoxColumn).Name = "m1DataGridViewTextBoxColumn";
		((DataGridViewBand)m1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)m1DataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)h2DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)h2DataGridViewTextBoxColumn).DataPropertyName = "EndShiftTime";
		((DataGridViewColumn)h2DataGridViewTextBoxColumn).HeaderText = "[ساعت خروج]";
		((DataGridViewColumn)h2DataGridViewTextBoxColumn).Name = "h2DataGridViewTextBoxColumn";
		((DataGridViewBand)h2DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)h2DataGridViewTextBoxColumn).Width = 116;
		((DataGridViewColumn)m2DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)m2DataGridViewTextBoxColumn).DataPropertyName = "M2";
		((DataGridViewColumn)m2DataGridViewTextBoxColumn).HeaderText = "[دقیقه]";
		((DataGridViewColumn)m2DataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)m2DataGridViewTextBoxColumn).Name = "m2DataGridViewTextBoxColumn";
		((DataGridViewBand)m2DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)m2DataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DGShiftColor).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGShiftColor).DataPropertyName = "ShiftColor";
		((DataGridViewColumn)DGShiftColor).HeaderText = "[رنگ شیفت]";
		((DataGridViewColumn)DGShiftColor).MinimumWidth = 50;
		((DataGridViewColumn)DGShiftColor).Name = "DGShiftColor";
		((DataGridViewBand)DGShiftColor).ReadOnly = true;
		((DataGridViewColumn)DGShiftColor).Width = 97;
		((DataGridViewColumn)nextDayDataGridViewCheckBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)nextDayDataGridViewCheckBoxColumn).DataPropertyName = "NextDay";
		((DataGridViewColumn)nextDayDataGridViewCheckBoxColumn).HeaderText = "[انتقال از روز قبل]";
		((DataGridViewColumn)nextDayDataGridViewCheckBoxColumn).Name = "nextDayDataGridViewCheckBoxColumn";
		((DataGridViewBand)nextDayDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nextDayDataGridViewCheckBoxColumn).Width = 146;
		shiftCodeingBindingSource.DataSource = typeof(ShiftCodeing);
		shiftCodeingBindingSource.CurrentChanged += shiftCodeingBindingSource_CurrentChanged;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(13, 513);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 24);
		((Control)LblRowCount).TabIndex = 21;
		((Control)LblRowCount).Text = "labelX3";
		((BaseItemControl)lblDescForShiftProg).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblDescForShiftProg).ForeColor = Color.DarkRed;
		((Control)lblDescForShiftProg).Location = new Point(13, 268);
		((Control)lblDescForShiftProg).Name = "lblDescForShiftProg";
		((Control)lblDescForShiftProg).Size = new Size(126, 103);
		((Control)lblDescForShiftProg).TabIndex = 21;
		((Control)lblDescForShiftProg).Text = "[در حـالت انتـخاب شیـفت های بـرنـامـه ریـزی شما میتـوانید چندین شیفت را انتـخـاب نمــائید برای این کار حتما\u064b باید شیفت های مورد نظر را تیک بزنید ]";
		lblDescForShiftProg.TextLineAlignment = (StringAlignment)0;
		((Control)lblDescForShiftProg).Visible = false;
		lblDescForShiftProg.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 562);
		((Control)this).Name = "FrmShiftCodeingList";
		((Control)this).Text = "[فرم لیست شیفتها]";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)shiftCodeingBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
