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
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Forms.Reports;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckOutContractSelect : FormExtention, ICheckout
{
	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX7;

	internal LabelX labelX6;

	internal LabelX labelX5;

	internal LabelX labelX3;

	private TabItem tabItem2;

	private BindingSource personBindingSource;

	private ButtonX BtnSelectContracts;

	internal DataGridViewX DGrid;

	private DataGridViewButtonColumn DGBtnView;

	private DataGridViewTextBoxColumn DGPersonId;

	private DataGridViewTextBoxColumn DGContractId;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewTextBoxColumn DGDateStartSh;

	private DataGridViewTextBoxColumn DGDateFinishSh;

	private DataGridViewTextBoxColumn DGSumReward;

	private DataGridViewTextBoxColumn DGSumSanavat;

	private DataGridViewTextBoxColumn DGSumVacationPrice;

	private DataGridViewTextBoxColumn DGSumAll;

	public FrmCheckOutContractSelect(string contextGuid, Font font, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_checkout = checkout;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
	}

	private async void BtnSelectContracts_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)27, _checkout);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			await LoadGridAsync();
			await LoadFrmCheckoutDetailsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectContracts_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckOutContractSelect.cs", 54);
		}
	}

	public void FillCheckout()
	{
		throw new NotImplementedException();
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		return _nextPage ?? (_nextPage = new FrmIncreaseSalaryParam(((CacheBaseForm)this).ContextGuid, _checkout, _font));
	}

	private async void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGBtnView).Index)
		{
			await LoadFrmCheckoutDetailsAsync(e.RowIndex);
		}
	}

	private async void DGrid_VisibleChanged(object sender, EventArgs e)
	{
		await LoadGridAsync();
	}

	private async Task LoadGridAsync(CancellationToken token = default(CancellationToken))
	{
	}

	private async Task LoadFrmCheckoutDetailsAsync(int fromRowIndex = 0)
	{
		_ = 1;
		try
		{
			for (int i = fromRowIndex; i < ((DataGridView)DGrid).RowCount && (int)(await ((CacheBaseForm)new FrmCheckoutDetails(((CacheBaseForm)this).ContextGuid, _font, int.Parse(((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGContractId).Index].Value.ToString()), _checkout)).ShowAsync((IWin32Window)(object)this)) != 7; i++)
			{
			}
			await LoadGridAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFrmCheckoutDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckOutContractSelect.cs", 130);
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
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
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
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Expected O, but got Unknown
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Expected O, but got Unknown
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		BtnSelectContracts = new ButtonX();
		labelX7 = new LabelX();
		DGrid = new DataGridViewX();
		personBindingSource = new BindingSource(components);
		labelX6 = new LabelX();
		labelX5 = new LabelX();
		labelX3 = new LabelX();
		tabItem2 = new TabItem(components);
		DGBtnView = new DataGridViewButtonColumn();
		DGPersonId = new DataGridViewTextBoxColumn();
		DGContractId = new DataGridViewTextBoxColumn();
		DGFullName = new DataGridViewTextBoxColumn();
		DGDateStartSh = new DataGridViewTextBoxColumn();
		DGDateFinishSh = new DataGridViewTextBoxColumn();
		DGSumReward = new DataGridViewTextBoxColumn();
		DGSumSanavat = new DataGridViewTextBoxColumn();
		DGSumVacationPrice = new DataGridViewTextBoxColumn();
		DGSumAll = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 499);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)BtnSelectContracts);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel4).Controls.Add((Control)(object)DGrid);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Padding = new Padding(10);
		((Control)groupPanel4).Size = new Size(582, 499);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.Class = "";
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.Class = "";
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.Class = "";
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "انتخاب قراردادها";
		((Control)BtnSelectContracts).AccessibleRole = (AccessibleRole)43;
		BtnSelectContracts.ColorTable = (eButtonColor)6;
		((Control)BtnSelectContracts).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelectContracts).ForeColor = Color.Black;
		BtnSelectContracts.ImagePosition = (eImagePosition)1;
		((Control)BtnSelectContracts).Location = new Point(13, 77);
		((Control)BtnSelectContracts).Name = "BtnSelectContracts";
		((Control)BtnSelectContracts).RightToLeft = (RightToLeft)1;
		BtnSelectContracts.Shortcuts.Add((eShortcut)116);
		((Control)BtnSelectContracts).Size = new Size(146, 40);
		((PopupItemControl)BtnSelectContracts).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectContracts).TabIndex = 12;
		((Control)BtnSelectContracts).Text = "انتخـاب قـراردادهـا";
		((Control)BtnSelectContracts).Click += BtnSelectContracts_Click;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).ForeColor = Color.Maroon;
		((Control)labelX7).Location = new Point(19, 89);
		((Control)labelX7).Name = "labelX7";
		labelX7.PaddingBottom = 5;
		labelX7.PaddingLeft = 5;
		labelX7.PaddingRight = 5;
		labelX7.PaddingTop = 5;
		((Control)labelX7).Size = new Size(545, 28);
		((Control)labelX7).TabIndex = 10;
		((Control)labelX7).Text = "مرخصی های روزانه و ساعتی ، ماموریت و ... به درستی ثبت شده باشد .";
		labelX7.WordWrap = true;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)9;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		val2.Alignment = (DataGridViewContentAlignment)32;
		val2.BackColor = SystemColors.Control;
		val2.Font = new Font("B Homa", 9.75f);
		val2.ForeColor = SystemColors.WindowText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DGrid).ColumnHeadersDefaultCellStyle = val2;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[10]
		{
			(DataGridViewColumn)DGBtnView,
			(DataGridViewColumn)DGPersonId,
			(DataGridViewColumn)DGContractId,
			(DataGridViewColumn)DGFullName,
			(DataGridViewColumn)DGDateStartSh,
			(DataGridViewColumn)DGDateFinishSh,
			(DataGridViewColumn)DGSumReward,
			(DataGridViewColumn)DGSumSanavat,
			(DataGridViewColumn)DGSumVacationPrice,
			(DataGridViewColumn)DGSumAll
		});
		((DataGridView)DGrid).DataSource = personBindingSource;
		val3.Alignment = (DataGridViewContentAlignment)32;
		val3.BackColor = SystemColors.Window;
		val3.Font = new Font("B Homa", 9.75f);
		val3.ForeColor = SystemColors.ControlText;
		val3.NullValue = "False";
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.ControlText;
		val3.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val3;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(12, 129);
		((Control)DGrid).Margin = new Padding(2);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(556, 339);
		((Control)DGrid).TabIndex = 2;
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((Control)DGrid).VisibleChanged += DGrid_VisibleChanged;
		personBindingSource.DataSource = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person);
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX6).ForeColor = Color.Maroon;
		((Control)labelX6).Location = new Point(19, 63);
		((Control)labelX6).Name = "labelX6";
		labelX6.PaddingBottom = 5;
		labelX6.PaddingLeft = 5;
		labelX6.PaddingRight = 5;
		labelX6.PaddingTop = 5;
		((Control)labelX6).Size = new Size(545, 26);
		((Control)labelX6).TabIndex = 10;
		((Control)labelX6).Text = "در تردد اشخاص ورود و خروج ناقص،وجود نداشته نباشد.";
		labelX6.WordWrap = true;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).ForeColor = Color.Maroon;
		((Control)labelX5).Location = new Point(19, 35);
		((Control)labelX5).Name = "labelX5";
		labelX5.PaddingBottom = 5;
		labelX5.PaddingLeft = 5;
		labelX5.PaddingRight = 5;
		labelX5.PaddingTop = 5;
		((Control)labelX5).Size = new Size(545, 30);
		((Control)labelX5).TabIndex = 10;
		((Control)labelX5).Text = "نکته مهم - به این موارد دقت داشته باشید :";
		labelX5.WordWrap = true;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(13, 7);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(557, 34);
		((Control)labelX3).TabIndex = 10;
		((Control)labelX3).Text = "در این مرحله باید قراردادهایی که قصد دارید تسویه شوند و یا به قرارداد جدید منتقل شوند انتخاب نمایید .";
		labelX3.TextAlignment = (StringAlignment)1;
		labelX3.WordWrap = true;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "";
		((DataGridViewColumn)DGBtnView).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGBtnView).DataPropertyName = "btnField";
		((DataGridViewColumn)DGBtnView).HeaderText = "مشاهده جزئیات";
		((DataGridViewColumn)DGBtnView).Name = "DGBtnView";
		((DataGridViewBand)DGBtnView).ReadOnly = true;
		DGBtnView.Text = "جزئیات";
		((DataGridViewColumn)DGBtnView).ToolTipText = "مشاهده جزئیات تسویه حساب";
		((DataGridViewColumn)DGBtnView).Width = 88;
		((DataGridViewColumn)DGPersonId).DataPropertyName = "Id";
		((DataGridViewColumn)DGPersonId).HeaderText = "personId";
		((DataGridViewColumn)DGPersonId).Name = "DGPersonId";
		((DataGridViewBand)DGPersonId).ReadOnly = true;
		((DataGridViewBand)DGPersonId).Visible = false;
		((DataGridViewColumn)DGContractId).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGContractId).DataPropertyName = "ContractId";
		((DataGridViewColumn)DGContractId).HeaderText = "شماره قرارداد";
		((DataGridViewColumn)DGContractId).Name = "DGContractId";
		((DataGridViewBand)DGContractId).ReadOnly = true;
		((DataGridViewColumn)DGContractId).Width = 94;
		((DataGridViewColumn)DGFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DGFullName).HeaderText = "نام و نام خانوادگی";
		((DataGridViewColumn)DGFullName).Name = "DGFullName";
		((DataGridViewBand)DGFullName).ReadOnly = true;
		((DataGridViewColumn)DGFullName).Width = 117;
		((DataGridViewColumn)DGDateStartSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateStartSh).DataPropertyName = "DateStartSh";
		((DataGridViewColumn)DGDateStartSh).HeaderText = "تاریخ شروع قرارداد";
		((DataGridViewColumn)DGDateStartSh).Name = "DGDateStartSh";
		((DataGridViewBand)DGDateStartSh).ReadOnly = true;
		((DataGridViewColumn)DGDateStartSh).Width = 115;
		((DataGridViewColumn)DGDateFinishSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateFinishSh).DataPropertyName = "DateFinishSh";
		((DataGridViewColumn)DGDateFinishSh).HeaderText = "تاریخ اتمام قرارداد";
		((DataGridViewColumn)DGDateFinishSh).Name = "DGDateFinishSh";
		((DataGridViewBand)DGDateFinishSh).ReadOnly = true;
		((DataGridViewColumn)DGDateFinishSh).Width = 117;
		((DataGridViewColumn)DGSumReward).DataPropertyName = "SumRewardPrice";
		((DataGridViewColumn)DGSumReward).HeaderText = "مبلغ پاداش";
		((DataGridViewColumn)DGSumReward).Name = "DGSumReward";
		((DataGridViewBand)DGSumReward).ReadOnly = true;
		((DataGridViewColumn)DGSumSanavat).DataPropertyName = "SumSanavatPrice";
		((DataGridViewColumn)DGSumSanavat).HeaderText = "مبلغ سنوات";
		((DataGridViewColumn)DGSumSanavat).Name = "DGSumSanavat";
		((DataGridViewBand)DGSumSanavat).ReadOnly = true;
		((DataGridViewColumn)DGSumVacationPrice).DataPropertyName = "SumVacationPrice";
		((DataGridViewColumn)DGSumVacationPrice).HeaderText = "مبلغ مانده مرخصی";
		((DataGridViewColumn)DGSumVacationPrice).Name = "DGSumVacationPrice";
		((DataGridViewBand)DGSumVacationPrice).ReadOnly = true;
		((DataGridViewBand)DGSumVacationPrice).Resizable = (DataGridViewTriState)1;
		DGSumVacationPrice.SortMode = (DataGridViewColumnSortMode)0;
		((DataGridViewColumn)DGSumAll).DataPropertyName = "SumAll";
		((DataGridViewColumn)DGSumAll).HeaderText = "مبلغ تسویه حساب";
		((DataGridViewColumn)DGSumAll).Name = "DGSumAll";
		((DataGridViewBand)DGSumAll).ReadOnly = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 521);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckOutContractSelect";
		((Control)this).Text = "FrmCheckOutContractSelect";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	bool ICheckout.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void ICheckout.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void ICheckout.Show(IWin32Window owner)
	{
		((Form)this).Show(owner);
	}
}
