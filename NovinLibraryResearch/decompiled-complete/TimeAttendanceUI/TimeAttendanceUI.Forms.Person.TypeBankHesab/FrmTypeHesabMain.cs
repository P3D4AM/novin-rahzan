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
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceUI.Forms.Person.TypeBankHesab;

public class FrmTypeHesabMain : FrmExtList
{
	private List<BankHesabType> _listData;

	private SortableBindingList<BankHesabType> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private BindingSource bankHesabTypeBindingSource;

	internal DataGridViewX DGrid;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;

	private LabelX LblRowCount;

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

	public BankHesabType SelectedRow { get; private set; }

	public Font FontFrm { get; private set; }

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

	public FrmTypeHesabMain(string contextGuid, Font font)
		: base(contextGuid)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			FontFrm = font;
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)this).Text = "لیست انواع حساب های بانکی";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 75);
		}
	}

	public FrmTypeHesabMain(string contextGuid, bool pIsSelect, Font font)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			FontFrm = font;
			((Control)base.BtnSelect).Visible = pIsSelect;
			((Control)this).Text = "لیست انواع حساب های بانکی";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 90);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await BankHesabType.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
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
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(AddEvent);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(EditEvent);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(DeleteEvent);
			((Control)DGrid).Font = FontFrm;
			((Control)DGrid).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 137);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 153);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await BankHesabType.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmTypeHesabMain frmTypeHesabMain = this;
				List<BankHesabType> listData = _listData;
				frmTypeHesabMain._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<BankHesabType>((IEnumerable<BankHesabType>)listData, token) : null);
			}
			else
			{
				IEnumerable<BankHesabType> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((BankHesabType p) => p.Name.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<BankHesabType>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<BankHesabType>)(object)_listForDisplay).Count;
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
						bankHesabTypeBindingSource.DataSource = _listForDisplay;
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

	private async void AddEvent(object sender, EventArgs e)
	{
		try
		{
			if (((Control)base.BtnAdd).Visible && ((Control)base.BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)(await ((CacheBaseForm)new FRMTypeHesab(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 216);
		}
	}

	private async void DeleteEvent(object sender, EventArgs e)
	{
		if (!((Control)base.BtnDelete).Visible || !((Control)base.BtnDelete).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (SelectedRow != null && (int)FMessageBox.Show("براي حذف کردن مطمئن هستيد؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				SelectedRow.ST = false;
				await SelectedRow.SaveAsync(token);
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 237);
		}
	}

	private async void EditEvent(object sender, EventArgs e)
	{
		if (!((Control)base.BtnEdit).Visible || !((Control)base.BtnEdit).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (SelectedRow != null && (int)(await ((CacheBaseForm)new FRMTypeHesab(((CacheBaseForm)this).ContextGuid, SelectedRow)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EditEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeBankHesab\\FRMTypeHesabMain.cs", 254);
		}
	}

	private void bankHesabTypeBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (BankHesabType)bankHesabTypeBindingSource.Current;
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
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		bankHesabTypeBindingSource = new BindingSource(components);
		DGrid = new DataGridViewX();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)bankHesabTypeBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(534, 421);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.GroupPanel1).Size = new Size(258, 60);
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
		((Control)base.LabelX1).Location = new Point(182, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(170, 21);
		bankHesabTypeBindingSource.DataSource = typeof(BankHesabType);
		bankHesabTypeBindingSource.CurrentChanged += bankHesabTypeBindingSource_CurrentChanged;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)descDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = bankHesabTypeBindingSource;
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
		((Control)DGrid).Size = new Size(375, 309);
		((Control)DGrid).TabIndex = 0;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[عنوان]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 200;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Width = 200;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).DataPropertyName = "Desc";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).Name = "descDataGridViewTextBoxColumn";
		((DataGridViewBand)descDataGridViewTextBoxColumn).ReadOnly = true;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 364);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 23;
		((Control)LblRowCount).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(534, 421);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(550, 460);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(550, 460);
		((Control)this).Name = "FrmTypeHesabMain";
		((Control)this).Text = "[لیست انواع حساب بانکی]";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)bankHesabTypeBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
