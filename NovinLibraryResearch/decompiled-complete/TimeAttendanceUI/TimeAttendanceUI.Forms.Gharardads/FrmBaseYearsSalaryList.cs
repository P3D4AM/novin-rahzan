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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Database;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmBaseYearsSalaryList : FrmExtList
{
	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private List<BaseYearsSalary> _listData;

	private SortableBindingList<BaseYearsSalary> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource baseYearsSalaryBindingSource;

	private DataGridViewTextBoxColumn nYearDG;

	private DataGridViewTextBoxColumn groupIdDGrid;

	private DataGridViewTextBoxColumn NameDG;

	private DataGridViewTextBoxColumn PriceDG;

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

	private Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[لیست پایه سنواتی]", out var value))
			{
				trDictionary.Add("[لیست پایه سنواتی]", Utilities.GetTranslate("[لیست پایه سنواتی]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب گروه پایه سنواتی]", out value))
			{
				trDictionary.Add("[انتخاب گروه پایه سنواتی]", Utilities.GetTranslate("[انتخاب گروه پایه سنواتی]"));
			}
			if (!trDictionary.TryGetValue("[تعداد رکوردها]", out value))
			{
				trDictionary.Add("[تعداد رکوردها]", Utilities.GetTranslate("[تعداد رکوردها]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 46);
		}
		return Task.CompletedTask;
	}

	public FrmBaseYearsSalaryList(string contextGuid, Font font)
		: base(contextGuid)
	{
		Task.Run((Func<Task?>)SetTrDictionaryAsync);
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)this).Text = "لیست پایه سنواتی";
			if (trDictionary.TryGetValue("[لیست پایه سنواتی]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 66);
		}
	}

	public FrmBaseYearsSalaryList(string contextGuid, bool pIsSelect, Font font)
		: base(contextGuid)
	{
		Task.Run((Func<Task?>)SetTrDictionaryAsync);
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)base.BtnSelect).Visible = pIsSelect;
			((Control)this).Text = "انتخاب گروه پایه سنواتی";
			if (trDictionary.TryGetValue("[انتخاب گروه پایه سنواتی]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 84);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await BaseYearsSalary.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
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

	protected override void OnLoad(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((FrmExtList)this).Other_Click += new CRMEventHandler(BtnOther_Click);
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 122);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 140);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await BaseYearsSalary.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmBaseYearsSalaryList frmBaseYearsSalaryList = this;
				List<BaseYearsSalary> listData = _listData;
				frmBaseYearsSalaryList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<BaseYearsSalary>((IEnumerable<BaseYearsSalary>)(from p in listData
					orderby p.GroupId, p.nYear
					select p), token) : null);
			}
			else
			{
				IEnumerable<BaseYearsSalary> source = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					source = source.Where((BaseYearsSalary p) => p.Name.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<BaseYearsSalary>((IEnumerable<BaseYearsSalary>)(from p in source
					orderby p.GroupId, p.nYear
					select p), token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<BaseYearsSalary>)(object)_listForDisplay).Count;
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
						baseYearsSalaryBindingSource.DataSource = _listForDisplay;
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

	private async Task<BaseYearsSalary> GetCurrentClsAsync(CancellationToken token)
	{
		try
		{
			if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return null;
			}
			return await BaseYearsSalary.GetAsync(((CacheBaseForm)this).ContextGuid, Convert.ToInt16(((DataGridView)DGrid)[((DataGridViewBand)nYearDG).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()), Convert.ToInt16(((DataGridView)DGrid)[((DataGridViewBand)groupIdDGrid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetCurrentClsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 203);
		}
		return await Task.FromResult<BaseYearsSalary>(null);
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)base.BtnAdd).Visible && ((Control)base.BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)(await ((CacheBaseForm)new FrmBaseYearsSalary(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 224);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			BaseYearsSalary baseYearsSalary = await GetCurrentClsAsync(token);
			if (baseYearsSalary != null && (int)FMessageBox.Show(Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7 && await baseYearsSalary.DeleteAsync(token))
			{
				new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 245);
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
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			BaseYearsSalary baseYearsSalary = await GetCurrentClsAsync(token);
			if (baseYearsSalary != null && (int)(await ((CacheBaseForm)new FrmBaseYearsSalary(((CacheBaseForm)this).ContextGuid, baseYearsSalary)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 266);
		}
	}

	private async void BtnOther_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if ((int)FMessageBox.Show("تمام داده های این جدول جایگزین خواهند شد. آیا ادامه می دهید؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				ReturnedSaveFuncInfo val = await BaseYearSalary.InsertAsync(((CacheBaseForm)this).ContextGuid, token);
				if (val.HasError)
				{
					val.Show(((CacheBaseForm)this).ContextGuid);
				}
				else
				{
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnOther_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnOther_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalaryList.cs", 291);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FrmExtList)this).Dispose(disposing);
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
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		nYearDG = new DataGridViewTextBoxColumn();
		groupIdDGrid = new DataGridViewTextBoxColumn();
		NameDG = new DataGridViewTextBoxColumn();
		PriceDG = new DataGridViewTextBoxColumn();
		baseYearsSalaryBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)baseYearsSalaryBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(800, 450);
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
		((Control)base.GroupPanel1).Size = new Size(790, 60);
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
		((Control)base.LabelX1).Location = new Point(708, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(679, 21);
		((Control)base.BtnSelect).Text = "[انتخاب]\r\n    \r\n    (Enter)";
		((Control)base.BtnOther).Text = "بروزرسانی داده ها";
		((Control)base.BtnOther).Visible = true;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[4]
		{
			(DataGridViewColumn)nYearDG,
			(DataGridViewColumn)groupIdDGrid,
			(DataGridViewColumn)NameDG,
			(DataGridViewColumn)PriceDG
		});
		((DataGridView)DGrid).DataSource = baseYearsSalaryBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(640, 345);
		((Control)DGrid).TabIndex = 27;
		((DataGridViewColumn)nYearDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)nYearDG).DataPropertyName = "nYear";
		((DataGridViewColumn)nYearDG).HeaderText = "مدت سنوات (سال)";
		((DataGridViewColumn)nYearDG).MinimumWidth = 70;
		((DataGridViewColumn)nYearDG).Name = "nYearDG";
		((DataGridViewBand)nYearDG).ReadOnly = true;
		((DataGridViewColumn)nYearDG).Width = 120;
		((DataGridViewColumn)groupIdDGrid).DataPropertyName = "GroupId";
		((DataGridViewColumn)groupIdDGrid).HeaderText = "[شناسه گروه]";
		((DataGridViewColumn)groupIdDGrid).Name = "groupIdDGrid";
		((DataGridViewBand)groupIdDGrid).ReadOnly = true;
		((DataGridViewBand)groupIdDGrid).Visible = false;
		((DataGridViewColumn)NameDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)NameDG).DataPropertyName = "Name";
		((DataGridViewColumn)NameDG).HeaderText = "[نام گروه]";
		((DataGridViewColumn)NameDG).MinimumWidth = 100;
		((DataGridViewColumn)NameDG).Name = "NameDG";
		((DataGridViewBand)NameDG).ReadOnly = true;
		((DataGridViewColumn)PriceDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)PriceDG).DataPropertyName = "Price";
		val2.Format = "N0";
		val2.NullValue = null;
		((DataGridViewBand)PriceDG).DefaultCellStyle = val2;
		((DataGridViewColumn)PriceDG).HeaderText = "[مبلغ]";
		((DataGridViewColumn)PriceDG).MinimumWidth = 150;
		((DataGridViewColumn)PriceDG).Name = "PriceDG";
		((DataGridViewBand)PriceDG).ReadOnly = true;
		((DataGridViewColumn)PriceDG).Width = 150;
		baseYearsSalaryBindingSource.DataSource = typeof(BaseYearsSalary);
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 399);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 28;
		((Control)LblRowCount).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).Name = "FrmBaseYearsSalaryList";
		((Control)this).Text = "[پایه سنواتی]*";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)baseYearsSalaryBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
