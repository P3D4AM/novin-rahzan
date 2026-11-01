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
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceUI.Salary.Forms.Codeing;

public class FrmTaxTableList : FrmExtList
{
	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private string Frm_Text = "[جدول میزان مالیات بر درآمد حقوق ماهانه]";

	private string TrCountRecord = "[تعداد رکوردها]";

	private string TrMaliatTaklifiHoghogh = "[مالیات تکلیفی حقوق از پلکان قبلی]";

	private Font _font;

	private List<TaxTable> _listData;

	private SortableBindingList<TaxTable> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource taxTableBindingSource;

	private LabelX labelX3;

	private LabelX labelX2;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn yearDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGDescription;

	private DataGridViewTextBoxColumn StartDateCalture;

	private DataGridViewTextBoxColumn EndDateCalture;

	private DataGridViewTextBoxColumn fromPriceDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn toPriceDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn taxPercentageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn initialValueDataGridViewTextBoxColumn;

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

	private short Id
	{
		get
		{
			if (((DataGridView)DGrid).Rows.Count != 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				return short.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			}
			return 0;
		}
	}

	private TaxTable SelectedTaxTable => _listData?.FirstOrDefault((TaxTable q) => q.Id == Id) ?? new TaxTable(((CacheBaseForm)this).ContextGuid);

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

	public FrmTaxTableList(string contextGuid, Font font)
		: base(contextGuid)
	{
		InitializeComponent();
		_font = font;
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			Frm_Text = "[جدول میزان مالیات بر درآمد حقوق ماهانه]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 73);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersMain)
			{
				((Form)this).Close();
				return;
			}
			if ((int)((Form)this).WindowState == 2)
			{
				((DataGridViewColumn)StartDateCalture).MinimumWidth = 150;
				((DataGridViewColumn)EndDateCalture).MinimumWidth = 150;
				((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).MinimumWidth = 150;
				((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).MinimumWidth = 150;
				((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).MinimumWidth = 150;
				((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).MinimumWidth = 150;
			}
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((Control)DGrid).Focus();
			((Control)base.BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.TaxTable.TaxtableInsert;
			((Control)base.BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.TaxTable.TaxtableDelete;
			((Control)base.BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.TaxTable.TaxtableUpdate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 125);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 139);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await TaxTable.GetAllAsync(((CacheBaseForm)this).ContextGuid, 0, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmTaxTableList frmTaxTableList = this;
				List<TaxTable> listData = _listData;
				frmTaxTableList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<TaxTable>((IEnumerable<TaxTable>)listData, token) : null);
			}
			else
			{
				IEnumerable<TaxTable> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((TaxTable p) => p.Description.Contains(word) || p.Year.ToString().Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<TaxTable>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<TaxTable>)(object)_listForDisplay).Count;
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
						taxTableBindingSource.DataSource = _listForDisplay;
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

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			FrmTaxTable frm = new FrmTaxTable(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 205);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			FrmTaxTable frm = new FrmTaxTable(((CacheBaseForm)this).ContextGuid, Id);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 223);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)base.BtnDelete).Visible && ((Control)base.BtnDelete).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف کاربر انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					await SelectedTaxTable.DeleteAsync();
					_listData = null;
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 243);
		}
	}

	private void DGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		try
		{
			if (e.ColumnIndex == ((DataGridViewBand)initialValueDataGridViewTextBoxColumn).Index && ((ConvertEventArgs)e).Value != null)
			{
				((DataGridView)DGrid).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = TrMaliatTaklifiHoghogh;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellFormatting", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 260);
		}
	}

	private void FrmTaxTableList_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Utilities.GetTranslate(Frm_Text);
			TrCountRecord = Utilities.GetTranslate(TrCountRecord);
			TrMaliatTaklifiHoghogh = Utilities.GetTranslate(TrMaliatTaklifiHoghogh);
			((FrmExtList)this).Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmTaxTableList_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTableList.cs", 277);
		}
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Expected O, but got Unknown
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		yearDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGDescription = new DataGridViewTextBoxColumn();
		StartDateCalture = new DataGridViewTextBoxColumn();
		EndDateCalture = new DataGridViewTextBoxColumn();
		fromPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		toPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		taxPercentageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		initialValueDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		taxTableBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		labelX2 = new LabelX();
		labelX3 = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)taxTableBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Controls.Add((Control)(object)labelX3);
		((Control)base.PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
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
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)labelX2, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)labelX3, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
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
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[9]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)yearDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGDescription,
			(DataGridViewColumn)StartDateCalture,
			(DataGridViewColumn)EndDateCalture,
			(DataGridViewColumn)fromPriceDataGridViewTextBoxColumn,
			(DataGridViewColumn)toPriceDataGridViewTextBoxColumn,
			(DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn,
			(DataGridViewColumn)initialValueDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = taxTableBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(146, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(625, 449);
		((Control)DGrid).TabIndex = 19;
		((DataGridView)DGrid).CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).DataPropertyName = "Year";
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).HeaderText = "[سال]";
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).Name = "yearDataGridViewTextBoxColumn";
		((DataGridViewBand)yearDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)yearDataGridViewTextBoxColumn).Width = 68;
		((DataGridViewColumn)DGDescription).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDescription).DataPropertyName = "Description";
		((DataGridViewColumn)DGDescription).HeaderText = "[شرح حساب]";
		((DataGridViewColumn)DGDescription).MinimumWidth = 100;
		((DataGridViewColumn)DGDescription).Name = "DGDescription";
		((DataGridViewBand)DGDescription).ReadOnly = true;
		((DataGridViewColumn)StartDateCalture).DataPropertyName = "StartDateCalture";
		((DataGridViewColumn)StartDateCalture).HeaderText = "[از تاریخ]";
		((DataGridViewColumn)StartDateCalture).Name = "StartDateCalture";
		((DataGridViewBand)StartDateCalture).ReadOnly = true;
		((DataGridViewColumn)EndDateCalture).DataPropertyName = "EndDateCalture";
		((DataGridViewColumn)EndDateCalture).HeaderText = "[ToDate]";
		((DataGridViewColumn)EndDateCalture).Name = "EndDateCalture";
		((DataGridViewBand)EndDateCalture).ReadOnly = true;
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).DataPropertyName = "FromPrice";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)fromPriceDataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).HeaderText = "[از مبلغ]";
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).MinimumWidth = 70;
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).Name = "fromPriceDataGridViewTextBoxColumn";
		((DataGridViewBand)fromPriceDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)fromPriceDataGridViewTextBoxColumn).Width = 99;
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).DataPropertyName = "ToPrice";
		val4.Format = "N0";
		val4.NullValue = null;
		((DataGridViewBand)toPriceDataGridViewTextBoxColumn).DefaultCellStyle = val4;
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).HeaderText = "[ToPrice]";
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).MinimumWidth = 70;
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).Name = "toPriceDataGridViewTextBoxColumn";
		((DataGridViewBand)toPriceDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)toPriceDataGridViewTextBoxColumn).Width = 84;
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).DataPropertyName = "TaxPercentage";
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).HeaderText = "[درصد مالیات]";
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).Name = "taxPercentageDataGridViewTextBoxColumn";
		((DataGridViewBand)taxPercentageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)taxPercentageDataGridViewTextBoxColumn).Width = 117;
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).DataPropertyName = "InitialValue";
		val5.Format = "N0";
		val5.NullValue = null;
		((DataGridViewBand)initialValueDataGridViewTextBoxColumn).DefaultCellStyle = val5;
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).HeaderText = "[مقدار اولیه]";
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).MinimumWidth = 70;
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).Name = "initialValueDataGridViewTextBoxColumn";
		((DataGridViewBand)initialValueDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).ToolTipText = "[مالیات تکلیفی حقوق از پلکان قبلی]";
		((DataGridViewColumn)initialValueDataGridViewTextBoxColumn).Width = 134;
		taxTableBindingSource.DataSource = typeof(TaxTable);
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(13, 502);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 24);
		((Control)LblRowCount).TabIndex = 22;
		((Control)LblRowCount).Text = "labelX3";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).ForeColor = Color.DarkRed;
		((Control)labelX2).Location = new Point(12, 233);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(127, 78);
		((Control)labelX2).TabIndex = 22;
		((Control)labelX2).Text = "[تـوجه : جهت محـاسبه صحـیح مـالـیات حقـوق پرسنل لطفا! بازه های مالیاتی را به ترتیب و با دقت بالا وارد کنید]";
		labelX2.TextLineAlignment = (StringAlignment)0;
		labelX2.WordWrap = true;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX3).ForeColor = Color.DarkRed;
		((Control)labelX3).Location = new Point(12, 317);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(127, 78);
		((Control)labelX3).TabIndex = 22;
		((Control)labelX3).Text = "[همچنین بازه معاف از مـالـیـات کـه از صـفـر شروع میشود را وارد نمائید]";
		labelX3.TextLineAlignment = (StringAlignment)0;
		labelX3.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "FrmTaxTableList";
		((Control)this).Text = "[جدول میزان مالیات بر درآمد حقوق ماهانه]*";
		((Form)this).Load += FrmTaxTableList_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)taxTableBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
