using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Reports;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.ReportProducer;

public class FrmRPTGenMain : BaseForm
{
	private Font _font;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnExec;

	private LabelX CountRows;

	private LabelX labelX2;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	private BindingSource reportGeneratorHozorBindingSource;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn DGridID;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

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

	public int Id
	{
		get
		{
			if (((DataGridView)DGrid).CurrentRow != null)
			{
				if (((DataGridView)DGrid).RowCount != 0)
				{
					return int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGridID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
				}
				return 0;
			}
			return 0;
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
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmRPTGenMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		_font = font;
		((Control)DGrid).Font = _font;
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((BaseForm)this).OnLoad(e);
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Reports.ReportsGozareshSazPoya)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 64);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			BindingSource val = reportGeneratorHozorBindingSource;
			val.DataSource = await RPTGen.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			((Control)CountRows).Text = reportGeneratorHozorBindingSource.Count.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 76);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BtnAdd).Visible && ((Control)BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				RPTGen report = new RPTGen(((CacheBaseForm)this).ContextGuid);
				FrmRPTGen frmRPTGen = new FrmRPTGen(((CacheBaseForm)this).ContextGuid, report, _font);
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				if ((int)(await ((CacheBaseForm)frmRPTGen).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 98);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (Id > 0)
			{
				RPTGen report = await RPTGen.GetAsync(((CacheBaseForm)this).ContextGuid, Id);
				if ((int)(await ((CacheBaseForm)new FrmRPTGen(((CacheBaseForm)this).ContextGuid, report, _font)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
				}
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 132);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (((Control)BtnDelete).Enabled && ((Control)BtnDelete).Visible)
			{
				((Control)DGrid).Focus();
				if (Id > 0 && (int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف طراحی گزارش انتخاب شده هستید ؟]"), Utilities.GetTranslate("[خطا]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					RPTGen obj = await RPTGen.GetAsync(((CacheBaseForm)this).ContextGuid, Id);
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await obj.DeleteAsync(token);
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 158);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string text = Text.FixString(((Control)TxtSearch).Text).Trim();
			if (string.IsNullOrEmpty(text.Trim()))
			{
				reportGeneratorHozorBindingSource.Filter = "";
				return;
			}
			reportGeneratorHozorBindingSource.Filter = "[Title] like '%" + text + "%' or [RPT] like '%" + text + "%' or [SQLQuery] like '%" + text + "%' or [شرح] like '%" + text + "%'";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 174);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode == 13) | ((int)e.KeyCode == 40))
			{
				((Control)DGrid).Focus();
			}
			else if (!string.IsNullOrEmpty(((Control)TxtSearch).Text) & ((int)e.KeyCode == 27))
			{
				((Control)TxtSearch).Text = "";
			}
			else if (string.IsNullOrEmpty(((Control)TxtSearch).Text) & ((int)e.KeyCode == 27))
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 191);
		}
	}

	private void FRMRPTGenMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode == 27)
					{
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else
				{
					((PopupItemControl)BtnEdit).PerformClick();
				}
			}
			else if ((int)keyCode != 45)
			{
				if ((int)keyCode != 46)
				{
					if ((int)keyCode == 118)
					{
						((PopupItemControl)BtnEdit).PerformClick();
					}
				}
				else
				{
					((PopupItemControl)BtnDelete).PerformClick();
				}
			}
			else
			{
				((PopupItemControl)BtnAdd).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMRPTGenMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 221);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	private async void BtnExec_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			RPTGen rpt = await RPTGen.GetAsync(((CacheBaseForm)this).ContextGuid, Id);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, rpt, _font);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnExec_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 242);
		}
	}

	private void TxtSearch_Enter(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.Yellow;
			((TextBoxBase)TxtSearch).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 255);
		}
	}

	private async void FRMRPTGenMain_Load(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await LoadDataAsync(token);
			await RPTGen.DeserializeALLAsync(((CacheBaseForm)this).ContextGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMRPTGenMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 272);
		}
	}

	private void TxtSearch_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGenMain.cs", 284);
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
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Expected O, but got Unknown
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Expected O, but got Unknown
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Expected O, but got Unknown
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Expected O, but got Unknown
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed4: Expected O, but got Unknown
		//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2a: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnExec = new ButtonX();
		CountRows = new LabelX();
		labelX2 = new LabelX();
		DGrid = new DataGridViewX();
		reportGeneratorHozorBindingSource = new BindingSource(components);
		GroupPanel1 = new GroupPanel();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		DGridID = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)reportGeneratorHozorBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnExec);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(720, 382);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 3;
		((Control)BtnExec).AccessibleRole = (AccessibleRole)43;
		BtnExec.ColorTable = (eButtonColor)6;
		((Control)BtnExec).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnExec.ImagePosition = (eImagePosition)1;
		((Control)BtnExec).Location = new Point(6, 189);
		((Control)BtnExec).Name = "BtnExec";
		((Control)BtnExec).Size = new Size(145, 33);
		((PopupItemControl)BtnExec).Style = (eDotNetBarStyle)9;
		((Control)BtnExec).TabIndex = 7;
		((Control)BtnExec).TabStop = false;
		((Control)BtnExec).Text = "[اجرای کوئری]";
		((Control)BtnExec).Click += BtnExec_Click;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.Class = "";
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(157, 347);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(436, 23);
		((Control)CountRows).TabIndex = 6;
		((Control)CountRows).Text = "0";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(599, 347);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(114, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[تعداد کل طراحی ها] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn4,
			(DataGridViewColumn)dataGridViewTextBoxColumn3,
			(DataGridViewColumn)DGridID,
			(DataGridViewColumn)dataGridViewTextBoxColumn2
		});
		((DataGridView)DGrid).DataSource = reportGeneratorHozorBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(157, 72);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(556, 269);
		((Control)DGrid).TabIndex = 1;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		reportGeneratorHozorBindingSource.DataMember = "ReportGeneratorHozor";
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		((Control)GroupPanel1).Location = new Point(6, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(707, 60);
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
		((PanelControl)GroupPanel1).Style.Class = "";
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.Class = "";
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.Class = "";
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(634, 8);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(50, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 9);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(625, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtSearch_Leave;
		((Control)TxtSearch).Enter += TxtSearch_Enter;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(6, 150);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(145, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 4;
		((Control)BtnDelete).TabStop = false;
		((Control)BtnDelete).Text = "[حذف] (Delete)";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(6, 111);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(145, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 3;
		((Control)BtnEdit).TabStop = false;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(6, 72);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(145, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 2;
		((Control)BtnAdd).TabStop = false;
		((Control)BtnAdd).Text = "[اضافه](Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "Title";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "[عنوان]";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).MinimumWidth = 100;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).DataPropertyName = "RPT";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).HeaderText = "[گزارش]";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).MinimumWidth = 150;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Name = "dataGridViewTextBoxColumn4";
		((DataGridViewBand)dataGridViewTextBoxColumn4).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Width = 150;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "SQLQuery";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "[کوئری]";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).MinimumWidth = 150;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Width = 150;
		((DataGridViewColumn)DGridID).DataPropertyName = "Id";
		((DataGridViewColumn)DGridID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGridID).Name = "DGridID";
		((DataGridViewBand)DGridID).ReadOnly = true;
		((DataGridViewBand)DGridID).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "[شرح]";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(720, 382);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FRMRPTGenMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست طراحی گزارشات]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FRMRPTGenMain_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMRPTGenMain_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)reportGeneratorHozorBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
