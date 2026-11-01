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
using TimeAttendanceDAL.Dal.Device;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmTypeTaradodList : FrmExtList
{
	private string Frm_Text = "[لیست عملکرد کاربران]";

	private string trCountRecords = "[تعداد رکوردها]";

	private List<LogonTypeTaradod> _listData;

	private SortableBindingList<LogonTypeTaradod> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource logOnTypeBindingSource;

	private DataGridViewTextBoxColumn typeTaradodNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn convertedTypeTaradodNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn enDeviceModelDG;

	private DataGridViewTextBoxColumn deviceModelNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn traficIdDG;

	private DataGridViewTextBoxColumn typeTaradodDG;

	private DataGridViewTextBoxColumn convertedTypeTaradodDG;

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

	public LogonTypeTaradod SelectedRow { get; private set; }

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

	public FrmTypeTaradodList(string contextGuid)
		: base(contextGuid)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs");
		Frm_Text = "[لیست انواع ترددها]";
		((Control)base.TxtSearch).Focus();
	}

	public FrmTypeTaradodList(string contextGuid, bool pIsSelect)
		: base(contextGuid)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs");
		Frm_Text = "[لیست انواع ترددها]";
		((Control)base.BtnSelect).Visible = pIsSelect;
		((Control)base.TxtSearch).Focus();
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		base.objDGrid = (Control)(object)DGrid;
		((FrmExtList)this).OnLoad(e);
		((Control)base.BtnAdd).Enabled = false;
		((Control)base.BtnDelete).Enabled = false;
		((FrmExtList)this).Edit_Click += new CRMEventHandler(EditEvent);
		((Control)DGrid).Focus();
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = await LogonTypeTaradod.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs", 139);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await LogonTypeTaradod.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmTypeTaradodList frmTypeTaradodList = this;
				List<LogonTypeTaradod> listData = _listData;
				frmTypeTaradodList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<LogonTypeTaradod>((IEnumerable<LogonTypeTaradod>)listData, token) : null);
			}
			else
			{
				IEnumerable<LogonTypeTaradod> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((LogonTypeTaradod p) => p.DeviceModelName.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<LogonTypeTaradod>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<LogonTypeTaradod>)(object)_listForDisplay).Count;
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
						logOnTypeBindingSource.DataSource = _listForDisplay;
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

	private async void EditEvent(object sender, EventArgs e)
	{
		if (!((Control)base.BtnEdit).Visible || !((Control)base.BtnEdit).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			FrmTypeTaradod frm = new FrmTypeTaradod(((CacheBaseForm)this).ContextGuid, SelectedRow.TraficId, SelectedRow.enDeviceModel);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EditEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs", 206);
		}
	}

	private void FrmTypeTaradodList_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			trCountRecords = Utilities.GetTranslate(trCountRecords);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmTypeTaradodList_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradodList.cs", 221);
		}
	}

	private void logOnTypeBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (LogonTypeTaradod)logOnTypeBindingSource.Current;
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		enDeviceModelDG = new DataGridViewTextBoxColumn();
		deviceModelNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		traficIdDG = new DataGridViewTextBoxColumn();
		typeTaradodDG = new DataGridViewTextBoxColumn();
		convertedTypeTaradodDG = new DataGridViewTextBoxColumn();
		logOnTypeBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)logOnTypeBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 444);
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
		((Control)base.TxtSearch).Margin = new Padding(5, 3, 5, 3);
		((Control)base.TxtSearch).Size = new Size(664, 21);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)enDeviceModelDG,
			(DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)traficIdDG,
			(DataGridViewColumn)typeTaradodDG,
			(DataGridViewColumn)convertedTypeTaradodDG
		});
		((DataGridView)DGrid).DataSource = logOnTypeBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(624, 342);
		((Control)DGrid).TabIndex = 19;
		((DataGridViewColumn)enDeviceModelDG).DataPropertyName = "enDeviceModel";
		((DataGridViewColumn)enDeviceModelDG).HeaderText = "enDeviceModel";
		((DataGridViewColumn)enDeviceModelDG).Name = "enDeviceModelDG";
		((DataGridViewBand)enDeviceModelDG).ReadOnly = true;
		((DataGridViewBand)enDeviceModelDG).Visible = false;
		((DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn).DataPropertyName = "DeviceModelName";
		((DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn).HeaderText = "[مدل دستگاه]";
		((DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)deviceModelNameDataGridViewTextBoxColumn).Name = "deviceModelNameDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceModelNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)traficIdDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)traficIdDG).DataPropertyName = "TraficId";
		((DataGridViewColumn)traficIdDG).HeaderText = "[کد تردد]";
		((DataGridViewColumn)traficIdDG).MinimumWidth = 50;
		((DataGridViewColumn)traficIdDG).Name = "traficIdDG";
		((DataGridViewBand)traficIdDG).ReadOnly = true;
		((DataGridViewColumn)traficIdDG).Width = 117;
		((DataGridViewColumn)typeTaradodDG).DataPropertyName = "TypeTaradodName";
		((DataGridViewColumn)typeTaradodDG).HeaderText = "[نوع تردد دستگاه]";
		((DataGridViewColumn)typeTaradodDG).Name = "typeTaradodDG";
		((DataGridViewBand)typeTaradodDG).ReadOnly = true;
		((DataGridViewColumn)convertedTypeTaradodDG).DataPropertyName = "ConvertedTypeTaradodName";
		((DataGridViewColumn)convertedTypeTaradodDG).HeaderText = "[نوع تردد تغییر یافته]";
		((DataGridViewColumn)convertedTypeTaradodDG).Name = "convertedTypeTaradodDG";
		((DataGridViewBand)convertedTypeTaradodDG).ReadOnly = true;
		logOnTypeBindingSource.DataSource = typeof(LogonTypeTaradod);
		logOnTypeBindingSource.CurrentChanged += logOnTypeBindingSource_CurrentChanged;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 396);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 21;
		((Control)LblRowCount).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 444);
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(800, 483);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(800, 483);
		((Control)this).Name = "FrmTypeTaradodList";
		((Control)this).Text = "FrmTypeTaradodList";
		((Form)this).Load += FrmTypeTaradodList_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)logOnTypeBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
