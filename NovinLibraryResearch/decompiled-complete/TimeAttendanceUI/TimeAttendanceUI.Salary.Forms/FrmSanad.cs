using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendance.ValidatorAttribute.Sanad;
using TimeAttendanceDAL.Dal.FLRSanad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.View.Sanad;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmSanad : FormExtention
{
	private AttSanad _sanad;

	private static List<int> _fishIdList = new List<int>();

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnAccept;

	internal DataGridViewX DGrid;

	private TextBoxX TXTBes;

	private TextBoxX TXTBed;

	internal LabelX LabelX1;

	internal TextBoxX TxtDesc;

	private ButtonX BtnPrint;

	private NumericUpDown NUDSanadNO;

	internal LabelX labelX3;

	internal LabelX labelX2;

	private DateBoxFa TXTDate;

	private BindingSource aTTSanadDetBindingSource;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGCodeMoein;

	private DataGridViewTextBoxColumn DGCodeTafsil;

	private DataGridViewTextBoxColumn DGName;

	private DataGridViewTextBoxColumn DGDesc;

	private DataGridViewTextBoxColumn DGDebit;

	private DataGridViewTextBoxColumn DGCredit;

	public FrmSanad(string contextGuid)
		: base(contextGuid, true, true)
	{
	}

	public static async Task<FrmSanad> CreateAsync(string contextGuid, List<Fish> fishs, CancellationToken token = default(CancellationToken))
	{
		FrmSanad ret = new FrmSanad(contextGuid);
		try
		{
			_fishIdList = fishs.Select((Fish q) => q.Id).ToList();
			ret.InitializeComponent();
			FrmSanad frmSanad = ret;
			frmSanad._sanad = await AttSanad.CreateAsync(contextGuid, fishs, token);
			await ret.FillFormAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 42);
		}
		return ret;
	}

	public FrmSanad(string contextGuid, AttSanad sanad)
		: base(contextGuid, true, true)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_sanad = sanad;
			new Waiter("در حال پردازش", (Form)(object)this, FillFormAsync(), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 55);
		}
	}

	private async Task FillFormAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			List<ATT_SanadDet> dataSource = await _sanad.GetLstSanadDetsAsync(token);
			aTTSanadDetBindingSource.DataSource = dataSource;
			((Control)TxtDesc).Text = _sanad.Description;
			NUDSanadNO.Value = _sanad.No;
			TXTDate.MiladiDate = _sanad.DateM;
			await RefLblAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 71);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingMain)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 82);
		}
	}

	private async Task RefLblAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			decimal Bed = await _sanad.GetSumDebitAsync(token);
			decimal num = await _sanad.GetSumCreditAsync(token);
			((Control)TXTBed).Text = Bed.ToString("N0");
			((Control)TXTBes).Text = num.ToString("N0");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefLblAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 94);
		}
	}

	private void GetData()
	{
		try
		{
			_sanad.DateM = TXTDate.MiladiDate;
			_sanad.Description = ((Control)TxtDesc).Text;
			_sanad.No = Utilities.ParseToInt(NUDSanadNO.Value.ToString(CultureInfo.InvariantCulture), 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 105);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			((Control)DGrid).Focus();
			GetData();
			DialogResult val = await AttSanadValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _sanad, isShowMessage: true);
			if ((int)val != 2)
			{
				if (val - 5 > 1)
				{
					return;
				}
				((Form)this).DialogResult = (DialogResult)((await _sanad.SaveAsync()) ? 1 : 2);
				foreach (int fishId in _fishIdList)
				{
					Fish obj = await Fish.GetAsync(((CacheBaseForm)this).ContextGuid, fishId, loadClsTaradod: false);
					obj.IdSanad = _sanad.No;
					await obj.SaveAsync();
				}
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به چاپ] " + _sanad.Description + " [هستید ؟ ]"), Utilities.GetTranslate("[پیام سیستم]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					((PopupItemControl)BtnPrint).PerformClick();
				}
				((FormExtention)this).ResetControlChanged();
				((Form)this).Close();
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 141);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible)
			{
				return;
			}
			GetData();
			AttSanadViewReport attSanadViewReport = await AttSanadViewReport.ConvertToViewReportAsync(_sanad);
			List<object> lst = new List<object>
			{
				new
				{
					DateCalture = Calendar.MiladiToCalture(attSanadViewReport.DateM),
					Description = attSanadViewReport.Description,
					No = attSanadViewReport.No
				}
			};
			List<ATTSanadDetViewReport> lstSanadDets = attSanadViewReport.lstSanadDets;
			List<object> list = new List<object>();
			foreach (ATTSanadDetViewReport item in lstSanadDets)
			{
				list.Add(new { item.Debit, item.Credit, item.Description, item.CodeMoein, item.CodeTafsil, item.titleTafsil });
			}
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)5, lst, null, attSanadViewReport.ID, "", list);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanad.cs", 165);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Expected O, but got Unknown
		//IL_0d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d38: Expected O, but got Unknown
		//IL_0e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e43: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSanad));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		NUDSanadNO = new NumericUpDown();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		TXTDate = new DateBoxFa();
		BtnPrint = new ButtonX();
		LabelX1 = new LabelX();
		TxtDesc = new TextBoxX();
		TXTBes = new TextBoxX();
		TXTBed = new TextBoxX();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGCodeMoein = new DataGridViewTextBoxColumn();
		DGCodeTafsil = new DataGridViewTextBoxColumn();
		DGName = new DataGridViewTextBoxColumn();
		DGDesc = new DataGridViewTextBoxColumn();
		DGDebit = new DataGridViewTextBoxColumn();
		DGCredit = new DataGridViewTextBoxColumn();
		aTTSanadDetBindingSource = new BindingSource(components);
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)NUDSanadNO).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)aTTSanadDetBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)NUDSanadNO);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)TXTDate);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)LabelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)TXTBes);
		((Control)panelEx1).Controls.Add((Control)(object)TXTBed);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(784, 539);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)NUDSanadNO).Anchor = (AnchorStyles)9;
		((Control)NUDSanadNO).ForeColor = Color.DarkRed;
		((Control)NUDSanadNO).Location = new Point(653, 12);
		NUDSanadNO.Maximum = new decimal(new int[4] { -727379969, 232, 0, 0 });
		((Control)NUDSanadNO).Name = "NUDSanadNO";
		((Control)NUDSanadNO).Size = new Size(77, 21);
		((Control)NUDSanadNO).TabIndex = 1;
		((UpDownBase)NUDSanadNO).TextAlign = (HorizontalAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(735, 12);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(46, 21);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[شماره]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(241, 12);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(46, 21);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "[تاریخ]:";
		labelX2.TextAlignment = (StringAlignment)2;
		TXTDate.BackcolorTooltip = Color.Red;
		TXTDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TXTDate).Location = new Point(135, 12);
		TXTDate.MiladiDate = new DateTime(2017, 5, 8, 18, 43, 24, 0);
		((Control)TXTDate).Name = "TXTDate";
		((Control)TXTDate).Size = new Size(100, 21);
		((Control)TXTDate).TabIndex = 3;
		((Control)TXTDate).Text = "1396/02/18";
		TXTDate.TootTipDelay = 500;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(12, 56);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(117, 35);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 10;
		((Control)BtnPrint).Text = "[چاپ]";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(735, 56);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(46, 21);
		((Control)LabelX1).TabIndex = 4;
		((Control)LabelX1).Text = "[شرح]:";
		LabelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)13;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(135, 56);
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(595, 21);
		((Control)TxtDesc).TabIndex = 5;
		((Control)TXTBes).Anchor = (AnchorStyles)6;
		TXTBes.Border.Class = "TextBoxBorder";
		TXTBes.Border.CornerType = (eCornerType)1;
		((Control)TXTBes).Enabled = false;
		((Control)TXTBes).Location = new Point(12, 515);
		((Control)TXTBes).Name = "TXTBes";
		((Control)TXTBes).Size = new Size(130, 21);
		((Control)TXTBes).TabIndex = 8;
		((Control)TXTBed).Anchor = (AnchorStyles)6;
		TXTBed.Border.Class = "TextBoxBorder";
		TXTBed.Border.CornerType = (eCornerType)1;
		((Control)TXTBed).Enabled = false;
		((Control)TXTBed).Location = new Point(148, 515);
		((Control)TXTBed).Name = "TXTBed";
		((Control)TXTBed).Size = new Size(130, 21);
		((Control)TXTBed).TabIndex = 7;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCodeMoein,
			(DataGridViewColumn)DGCodeTafsil,
			(DataGridViewColumn)DGName,
			(DataGridViewColumn)DGDesc,
			(DataGridViewColumn)DGDebit,
			(DataGridViewColumn)DGCredit
		});
		((DataGridView)DGrid).DataSource = aTTSanadDetBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(12, 97);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(760, 412);
		((Control)DGrid).TabIndex = 6;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCodeMoein).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeMoein).DataPropertyName = "CodeMoein";
		((DataGridViewColumn)DGCodeMoein).HeaderText = "[کد معین]";
		((DataGridViewColumn)DGCodeMoein).Name = "DGCodeMoein";
		((DataGridViewBand)DGCodeMoein).ReadOnly = true;
		((DataGridViewColumn)DGCodeMoein).Width = 73;
		((DataGridViewColumn)DGCodeTafsil).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeTafsil).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)DGCodeTafsil).HeaderText = "[کد تفصیل]";
		((DataGridViewColumn)DGCodeTafsil).Name = "DGCodeTafsil";
		((DataGridViewBand)DGCodeTafsil).ReadOnly = true;
		((DataGridViewColumn)DGCodeTafsil).Width = 81;
		((DataGridViewColumn)DGName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGName).DataPropertyName = "TitleTafsil";
		((DataGridViewColumn)DGName).HeaderText = "[نام تفصیل]";
		((DataGridViewColumn)DGName).Name = "DGName";
		((DataGridViewBand)DGName).ReadOnly = true;
		((DataGridViewColumn)DGName).Width = 84;
		((DataGridViewColumn)DGDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDesc).DataPropertyName = "Description";
		((DataGridViewColumn)DGDesc).HeaderText = "[شرح]";
		((DataGridViewColumn)DGDesc).Name = "DGDesc";
		((DataGridViewBand)DGDesc).ReadOnly = true;
		((DataGridViewColumn)DGDebit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDebit).DataPropertyName = "Debit";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)DGDebit).DefaultCellStyle = val3;
		((DataGridViewColumn)DGDebit).HeaderText = "[بدهکار]";
		((DataGridViewColumn)DGDebit).MinimumWidth = 120;
		((DataGridViewColumn)DGDebit).Name = "DGDebit";
		((DataGridViewBand)DGDebit).ReadOnly = true;
		((DataGridViewColumn)DGDebit).Width = 120;
		((DataGridViewColumn)DGCredit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCredit).DataPropertyName = "Credit";
		val4.Format = "N0";
		val4.NullValue = null;
		((DataGridViewBand)DGCredit).DefaultCellStyle = val4;
		((DataGridViewColumn)DGCredit).HeaderText = "[بستانکار]";
		((DataGridViewColumn)DGCredit).MinimumWidth = 120;
		((DataGridViewColumn)DGCredit).Name = "DGCredit";
		((DataGridViewBand)DGCredit).ReadOnly = true;
		((DataGridViewColumn)DGCredit).Width = 120;
		aTTSanadDetBindingSource.DataSource = typeof(ATT_SanadDet);
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(12, 12);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(117, 35);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 9;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "FrmSanad";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[صدور سند حسابداری حقوق]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)NUDSanadNO).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)aTTSanadDetBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
