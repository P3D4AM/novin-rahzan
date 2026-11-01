using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using Serilog;
using TimeAttendance.ValidatorAttribute.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.View.Gharardad;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmCopyGharardadsNew : FormExtention
{
	private List<GharardadNew> _listGharardadNew;

	private ClsCopyGharardad _copyGharardad;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private NumericUpDown TxtGharardadNo;

	internal DateBoxFa TXTDateRegister;

	private LabelX labelX12;

	private LabelX labelX3;

	internal DateBoxFa TxtEndDate;

	internal DateBoxFa TXTStartDate;

	internal LabelX labelX8;

	internal LabelX labelX10;

	private TabItem tabItem1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabItem tabItem2;

	private Label label1;

	private RadioButton rbIncreasePercentage;

	private CheckBoxX chBxBaseSalary;

	private TextBoxX txtPercentage;

	private CheckBoxX chbxAbsent;

	private CheckBoxX chbxLeaveTime;

	private CheckBoxX chbxVacation;

	private CheckBoxX chbxNightTime;

	private CheckBoxX chbxHolidayTime;

	private CheckBoxX chbxShiftTime;

	private CheckBoxX chBxOverTime;

	internal LabelX labelX2;

	internal LabelX labelX1;

	private PanelEx panelEx2;

	internal LabelX labelX14;

	internal LabelX labelX16;

	internal LabelX labelX13;

	internal LabelX labelX11;

	internal LabelX labelX9;

	internal LabelX labelX7;

	internal LabelX labelX15;

	internal LabelX labelX6;

	internal LabelX labelX5;

	internal LabelX labelX4;

	internal ButtonX BtnKsorat;

	internal ButtonX BtnMazaya;

	private Label label2;

	private TextBoxX txtBaseYear;

	private RadioButton rbMinSalary;

	private RadioButton rbNothing;

	private CheckBoxX chbxShift;

	private CheckBoxX checkBoxX1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmCopyGharardadsNew(string contextGuid, List<GharardadNew> plst)
		: base(contextGuid, true, true)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		_listGharardadNew = plst;
		InitializeComponent();
		TxtGharardadNo.Value = _copyGharardad.No;
		((Control)TXTStartDate).Text = Calendar.MiladiToCalture(_copyGharardad.DateStartM);
		((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_copyGharardad.DateFinishM);
		((Control)TXTDateRegister).Text = _copyGharardad.DateSh;
		rbIncreasePercentage.Checked = true;
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardadsNew.cs");
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_copyGharardad = await ClsCopyGharardad.GetAsync(((CacheBaseForm)this).ContextGuid, _listGharardadNew, token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			DialogResult val = await CopyGharardadCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _copyGharardad, token);
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					await _copyGharardad.CreateNewGharardadAsync(token);
					((Form)this).DialogResult = (DialogResult)1;
					((FormExtention)this).ResetControlChanged();
					((Form)this).Close();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardadsNew.cs", 84);
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
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Expected O, but got Unknown
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Expected O, but got Unknown
		//IL_0df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e02: Expected O, but got Unknown
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Expected O, but got Unknown
		//IL_0f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f83: Expected O, but got Unknown
		//IL_1037: Unknown result type (might be due to invalid IL or missing references)
		//IL_1041: Expected O, but got Unknown
		//IL_10f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ff: Expected O, but got Unknown
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bd: Expected O, but got Unknown
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Expected O, but got Unknown
		//IL_1484: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Expected O, but got Unknown
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c7: Expected O, but got Unknown
		//IL_1f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f78: Expected O, but got Unknown
		//IL_2178: Unknown result type (might be due to invalid IL or missing references)
		//IL_2182: Expected O, but got Unknown
		//IL_2250: Unknown result type (might be due to invalid IL or missing references)
		//IL_225a: Expected O, but got Unknown
		//IL_2335: Unknown result type (might be due to invalid IL or missing references)
		//IL_233f: Expected O, but got Unknown
		//IL_23fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2406: Expected O, but got Unknown
		//IL_24de: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e8: Expected O, but got Unknown
		//IL_25c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d2: Expected O, but got Unknown
		//IL_27b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27bd: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		chbxShift = new CheckBoxX();
		rbNothing = new RadioButton();
		panelEx2 = new PanelEx();
		labelX14 = new LabelX();
		labelX16 = new LabelX();
		labelX13 = new LabelX();
		labelX11 = new LabelX();
		labelX9 = new LabelX();
		labelX7 = new LabelX();
		labelX15 = new LabelX();
		labelX6 = new LabelX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		chBxBaseSalary = new CheckBoxX();
		BtnKsorat = new ButtonX();
		labelX1 = new LabelX();
		BtnMazaya = new ButtonX();
		labelX2 = new LabelX();
		chbxAbsent = new CheckBoxX();
		chBxOverTime = new CheckBoxX();
		chbxLeaveTime = new CheckBoxX();
		chbxShiftTime = new CheckBoxX();
		chbxVacation = new CheckBoxX();
		chbxHolidayTime = new CheckBoxX();
		chbxNightTime = new CheckBoxX();
		txtBaseYear = new TextBoxX();
		txtPercentage = new TextBoxX();
		label2 = new Label();
		label1 = new Label();
		rbMinSalary = new RadioButton();
		rbIncreasePercentage = new RadioButton();
		TxtGharardadNo = new NumericUpDown();
		TXTDateRegister = new DateBoxFa();
		labelX12 = new LabelX();
		labelX3 = new LabelX();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		labelX8 = new LabelX();
		labelX10 = new LabelX();
		tabItem1 = new TabItem(components);
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		tabItem2 = new TabItem(components);
		checkBoxX1 = new CheckBoxX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel1).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((ISupportInitialize)TxtGharardadNo).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
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
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Location = new Point(12, 12);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(760, 484);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		((Control)tabControl1).TabStop = false;
		((Control)tabControl1).Text = "tabControl1";
		((Control)tabControlPanel1).Controls.Add((Control)(object)checkBoxX1);
		((Control)tabControlPanel1).Controls.Add((Control)(object)chbxShift);
		((Control)tabControlPanel1).Controls.Add((Control)(object)rbNothing);
		((Control)tabControlPanel1).Controls.Add((Control)(object)panelEx2);
		((Control)tabControlPanel1).Controls.Add((Control)(object)txtBaseYear);
		((Control)tabControlPanel1).Controls.Add((Control)(object)txtPercentage);
		((Control)tabControlPanel1).Controls.Add((Control)(object)label2);
		((Control)tabControlPanel1).Controls.Add((Control)(object)label1);
		((Control)tabControlPanel1).Controls.Add((Control)(object)rbMinSalary);
		((Control)tabControlPanel1).Controls.Add((Control)(object)rbIncreasePercentage);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtGharardadNo);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTDateRegister);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX12);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX3);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtEndDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTStartDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX8);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX10);
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(760, 458);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		((Control)chbxShift).AutoSize = true;
		((Control)chbxShift).BackColor = Color.Transparent;
		((BaseItemControl)chbxShift).BackgroundStyle.Class = "";
		((BaseItemControl)chbxShift).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbxShift).Location = new Point(434, 275);
		((Control)chbxShift).Name = "chbxShift";
		((Control)chbxShift).Size = new Size(318, 16);
		((BaseItemControl)chbxShift).Style = (eDotNetBarStyle)9;
		((Control)chbxShift).TabIndex = 17;
		((Control)chbxShift).Text = "مزایا و کسورات براسای قیمت های جدید بروزرسانی شود";
		((Control)rbNothing).BackColor = Color.Transparent;
		((Control)rbNothing).Location = new Point(286, 219);
		((Control)rbNothing).Name = "rbNothing";
		((Control)rbNothing).Size = new Size(466, 21);
		((Control)rbNothing).TabIndex = 16;
		((Control)rbNothing).Text = "هیچ کدام ، تنها قرارداد ها را به سال آینده منتقل کنید و کاربر جاری آنها را اصلاح میکند.";
		((ButtonBase)rbNothing).UseVisualStyleBackColor = false;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)labelX14);
		((Control)panelEx2).Controls.Add((Control)(object)labelX16);
		((Control)panelEx2).Controls.Add((Control)(object)labelX13);
		((Control)panelEx2).Controls.Add((Control)(object)labelX11);
		((Control)panelEx2).Controls.Add((Control)(object)labelX9);
		((Control)panelEx2).Controls.Add((Control)(object)labelX7);
		((Control)panelEx2).Controls.Add((Control)(object)labelX15);
		((Control)panelEx2).Controls.Add((Control)(object)labelX6);
		((Control)panelEx2).Controls.Add((Control)(object)labelX5);
		((Control)panelEx2).Controls.Add((Control)(object)labelX4);
		((Control)panelEx2).Controls.Add((Control)(object)chBxBaseSalary);
		((Control)panelEx2).Controls.Add((Control)(object)BtnKsorat);
		((Control)panelEx2).Controls.Add((Control)(object)labelX1);
		((Control)panelEx2).Controls.Add((Control)(object)BtnMazaya);
		((Control)panelEx2).Controls.Add((Control)(object)labelX2);
		((Control)panelEx2).Controls.Add((Control)(object)chbxAbsent);
		((Control)panelEx2).Controls.Add((Control)(object)chBxOverTime);
		((Control)panelEx2).Controls.Add((Control)(object)chbxLeaveTime);
		((Control)panelEx2).Controls.Add((Control)(object)chbxShiftTime);
		((Control)panelEx2).Controls.Add((Control)(object)chbxVacation);
		((Control)panelEx2).Controls.Add((Control)(object)chbxHolidayTime);
		((Control)panelEx2).Controls.Add((Control)(object)chbxNightTime);
		((Control)panelEx2).Location = new Point(14, 4);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(200, 237);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.Color = Color.FromArgb(255, 128, 0);
		panelEx2.Style.BackColor2.Color = Color.FromArgb(255, 224, 192);
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 12;
		((Control)labelX14).BackColor = Color.Transparent;
		((BaseItemControl)labelX14).BackgroundStyle.Class = "";
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX14).ForeColor = Color.Red;
		((Control)labelX14).Location = new Point(158, 213);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(35, 16);
		((Control)labelX14).TabIndex = 19;
		((Control)labelX14).Text = "-------";
		((Control)labelX16).BackColor = Color.Transparent;
		((BaseItemControl)labelX16).BackgroundStyle.Class = "";
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX16).ForeColor = Color.Red;
		((Control)labelX16).Location = new Point(146, 191);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(47, 16);
		((Control)labelX16).TabIndex = 16;
		((Control)labelX16).Text = "---------";
		((Control)labelX13).BackColor = Color.Transparent;
		((BaseItemControl)labelX13).BackgroundStyle.Class = "";
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX13).ForeColor = Color.Red;
		((Control)labelX13).Location = new Point(158, 142);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(35, 16);
		((Control)labelX13).TabIndex = 12;
		((Control)labelX13).Text = "-------";
		((Control)labelX11).BackColor = Color.Transparent;
		((BaseItemControl)labelX11).BackgroundStyle.Class = "";
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX11).ForeColor = Color.Red;
		((Control)labelX11).Location = new Point(158, 164);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(35, 16);
		((Control)labelX11).TabIndex = 14;
		((Control)labelX11).Text = "-------";
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.Class = "";
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX9).ForeColor = Color.Red;
		((Control)labelX9).Location = new Point(158, 120);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(35, 16);
		((Control)labelX9).TabIndex = 10;
		((Control)labelX9).Text = "-------";
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).ForeColor = Color.Red;
		((Control)labelX7).Location = new Point(158, 98);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(35, 16);
		((Control)labelX7).TabIndex = 8;
		((Control)labelX7).Text = "-------";
		((Control)labelX15).BackColor = Color.Transparent;
		((BaseItemControl)labelX15).BackgroundStyle.Class = "";
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX15).ForeColor = Color.Red;
		((Control)labelX15).Location = new Point(158, 54);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(35, 16);
		((Control)labelX15).TabIndex = 4;
		((Control)labelX15).Text = "-------";
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX6).ForeColor = Color.Red;
		((Control)labelX6).Location = new Point(158, 76);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(35, 16);
		((Control)labelX6).TabIndex = 6;
		((Control)labelX6).Text = "-------";
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).ForeColor = Color.Red;
		((Control)labelX5).Location = new Point(158, 32);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(35, 16);
		((Control)labelX5).TabIndex = 2;
		((Control)labelX5).Text = "-------";
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).ForeColor = Color.Red;
		((Control)labelX4).Location = new Point(158, 10);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(35, 16);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "-------";
		((Control)chBxBaseSalary).AutoSize = true;
		((Control)chBxBaseSalary).BackColor = Color.Transparent;
		((BaseItemControl)chBxBaseSalary).BackgroundStyle.Class = "";
		((BaseItemControl)chBxBaseSalary).BackgroundStyle.CornerType = (eCornerType)1;
		chBxBaseSalary.Checked = true;
		chBxBaseSalary.CheckState = (CheckState)1;
		chBxBaseSalary.CheckValue = "Y";
		((Control)chBxBaseSalary).Location = new Point(68, 10);
		((Control)chBxBaseSalary).Name = "chBxBaseSalary";
		((Control)chBxBaseSalary).Size = new Size(76, 16);
		((BaseItemControl)chBxBaseSalary).Style = (eDotNetBarStyle)9;
		((Control)chBxBaseSalary).TabIndex = 1;
		((Control)chBxBaseSalary).Text = "[حقوق پایه]";
		((Control)BtnKsorat).AccessibleRole = (AccessibleRole)43;
		BtnKsorat.ColorTable = (eButtonColor)6;
		((Control)BtnKsorat).Location = new Point(66, 213);
		((Control)BtnKsorat).Name = "BtnKsorat";
		((Control)BtnKsorat).Size = new Size(23, 16);
		((PopupItemControl)BtnKsorat).Style = (eDotNetBarStyle)9;
		((Control)BtnKsorat).TabIndex = 21;
		((Control)BtnKsorat).TabStop = false;
		((Control)BtnKsorat).Text = "...";
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(110, 191);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(30, 16);
		((Control)labelX1).TabIndex = 17;
		((Control)labelX1).Text = "[مزایا]";
		((Control)BtnMazaya).AccessibleRole = (AccessibleRole)43;
		BtnMazaya.ColorTable = (eButtonColor)6;
		((Control)BtnMazaya).Location = new Point(85, 191);
		((Control)BtnMazaya).Name = "BtnMazaya";
		((Control)BtnMazaya).Size = new Size(23, 16);
		((PopupItemControl)BtnMazaya).Style = (eDotNetBarStyle)9;
		((Control)BtnMazaya).TabIndex = 18;
		((Control)BtnMazaya).TabStop = false;
		((Control)BtnMazaya).Text = "...";
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(95, 213);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(49, 16);
		((Control)labelX2).TabIndex = 20;
		((Control)labelX2).Text = "[کسورات]";
		((Control)chbxAbsent).AutoSize = true;
		((Control)chbxAbsent).BackColor = Color.Transparent;
		((BaseItemControl)chbxAbsent).BackgroundStyle.Class = "";
		((BaseItemControl)chbxAbsent).BackgroundStyle.CornerType = (eCornerType)1;
		chbxAbsent.Checked = true;
		chbxAbsent.CheckState = (CheckState)1;
		chbxAbsent.CheckValue = "Y";
		((Control)chbxAbsent).Location = new Point(94, 164);
		((Control)chbxAbsent).Name = "chbxAbsent";
		((Control)chbxAbsent).Size = new Size(50, 16);
		((BaseItemControl)chbxAbsent).Style = (eDotNetBarStyle)9;
		((Control)chbxAbsent).TabIndex = 15;
		((Control)chbxAbsent).Text = "[غیبت]";
		((Control)chBxOverTime).AutoSize = true;
		((Control)chBxOverTime).BackColor = Color.Transparent;
		((BaseItemControl)chBxOverTime).BackgroundStyle.Class = "";
		((BaseItemControl)chBxOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		chBxOverTime.Checked = true;
		chBxOverTime.CheckState = (CheckState)1;
		chBxOverTime.CheckValue = "Y";
		((Control)chBxOverTime).Location = new Point(62, 32);
		((Control)chBxOverTime).Name = "chBxOverTime";
		((Control)chBxOverTime).Size = new Size(82, 16);
		((BaseItemControl)chBxOverTime).Style = (eDotNetBarStyle)9;
		((Control)chBxOverTime).TabIndex = 3;
		((Control)chBxOverTime).Text = "[اضافه کار]";
		((Control)chbxLeaveTime).AutoSize = true;
		((Control)chbxLeaveTime).BackColor = Color.Transparent;
		((BaseItemControl)chbxLeaveTime).BackgroundStyle.Class = "";
		((BaseItemControl)chbxLeaveTime).BackgroundStyle.CornerType = (eCornerType)1;
		chbxLeaveTime.Checked = true;
		chbxLeaveTime.CheckState = (CheckState)1;
		chbxLeaveTime.CheckValue = "Y";
		((Control)chbxLeaveTime).Location = new Point(79, 142);
		((Control)chbxLeaveTime).Name = "chbxLeaveTime";
		((Control)chbxLeaveTime).Size = new Size(65, 16);
		((BaseItemControl)chbxLeaveTime).Style = (eDotNetBarStyle)9;
		((Control)chbxLeaveTime).TabIndex = 13;
		((Control)chbxLeaveTime).Text = "[کسرکار]";
		((Control)chbxShiftTime).AutoSize = true;
		((Control)chbxShiftTime).BackColor = Color.Transparent;
		((BaseItemControl)chbxShiftTime).BackgroundStyle.Class = "";
		((BaseItemControl)chbxShiftTime).BackgroundStyle.CornerType = (eCornerType)1;
		chbxShiftTime.Checked = true;
		chbxShiftTime.CheckState = (CheckState)1;
		chbxShiftTime.CheckValue = "Y";
		((Control)chbxShiftTime).Location = new Point(68, 54);
		((Control)chbxShiftTime).Name = "chbxShiftTime";
		((Control)chbxShiftTime).Size = new Size(76, 16);
		((BaseItemControl)chbxShiftTime).Style = (eDotNetBarStyle)9;
		((Control)chbxShiftTime).TabIndex = 5;
		((Control)chbxShiftTime).Text = "[NobatKari]";
		((Control)chbxVacation).AutoSize = true;
		((Control)chbxVacation).BackColor = Color.Transparent;
		((BaseItemControl)chbxVacation).BackgroundStyle.Class = "";
		((BaseItemControl)chbxVacation).BackgroundStyle.CornerType = (eCornerType)1;
		chbxVacation.Checked = true;
		chbxVacation.CheckState = (CheckState)1;
		chbxVacation.CheckValue = "Y";
		((Control)chbxVacation).Location = new Point(78, 120);
		((Control)chbxVacation).Name = "chbxVacation";
		((Control)chbxVacation).Size = new Size(66, 16);
		((BaseItemControl)chbxVacation).Style = (eDotNetBarStyle)9;
		((Control)chbxVacation).TabIndex = 11;
		((Control)chbxVacation).Text = "[مرخصی]";
		((Control)chbxHolidayTime).AutoSize = true;
		((Control)chbxHolidayTime).BackColor = Color.Transparent;
		((BaseItemControl)chbxHolidayTime).BackgroundStyle.Class = "";
		((BaseItemControl)chbxHolidayTime).BackgroundStyle.CornerType = (eCornerType)1;
		chbxHolidayTime.Checked = true;
		chbxHolidayTime.CheckState = (CheckState)1;
		chbxHolidayTime.CheckValue = "Y";
		((Control)chbxHolidayTime).Location = new Point(63, 76);
		((Control)chbxHolidayTime).Name = "chbxHolidayTime";
		((Control)chbxHolidayTime).Size = new Size(81, 16);
		((BaseItemControl)chbxHolidayTime).Style = (eDotNetBarStyle)9;
		((Control)chbxHolidayTime).TabIndex = 7;
		((Control)chbxHolidayTime).Text = "[جمعه کاری]";
		((Control)chbxNightTime).AutoSize = true;
		((Control)chbxNightTime).BackColor = Color.Transparent;
		((BaseItemControl)chbxNightTime).BackgroundStyle.Class = "";
		((BaseItemControl)chbxNightTime).BackgroundStyle.CornerType = (eCornerType)1;
		chbxNightTime.Checked = true;
		chbxNightTime.CheckState = (CheckState)1;
		chbxNightTime.CheckValue = "Y";
		((Control)chbxNightTime).Location = new Point(69, 98);
		((Control)chbxNightTime).Name = "chbxNightTime";
		((Control)chbxNightTime).Size = new Size(75, 16);
		((BaseItemControl)chbxNightTime).Style = (eDotNetBarStyle)9;
		((Control)chbxNightTime).TabIndex = 9;
		((Control)chbxNightTime).Text = "[شب کاری]";
		txtBaseYear.Border.Class = "TextBoxBorder";
		txtBaseYear.Border.CornerType = (eCornerType)1;
		((Control)txtBaseYear).Enabled = false;
		((Control)txtBaseYear).Location = new Point(276, 169);
		((Control)txtBaseYear).Name = "txtBaseYear";
		((Control)txtBaseYear).Size = new Size(60, 21);
		((Control)txtBaseYear).TabIndex = 14;
		txtPercentage.Border.Class = "TextBoxBorder";
		txtPercentage.Border.CornerType = (eCornerType)1;
		((Control)txtPercentage).Location = new Point(621, 119);
		((Control)txtPercentage).Name = "txtPercentage";
		((Control)txtPercentage).Size = new Size(60, 21);
		((Control)txtPercentage).TabIndex = 9;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).ForeColor = Color.Red;
		((Control)label2).Location = new Point(220, 119);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(60, 21);
		((Control)label2).TabIndex = 11;
		((Control)label2).Text = "-------->";
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Location = new Point(286, 119);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(329, 21);
		((Control)label1).TabIndex = 10;
		((Control)label1).Text = "[درصدی به پارامترهای حقوق که شامل موارد روبرو خواهد شد] :";
		((Control)rbMinSalary).BackColor = Color.Transparent;
		((Control)rbMinSalary).Location = new Point(342, 169);
		((Control)rbMinSalary).Name = "rbMinSalary";
		((Control)rbMinSalary).Size = new Size(410, 21);
		((Control)rbMinSalary).TabIndex = 13;
		((Control)rbMinSalary).Text = "[اعمال حداقل حقوق روزانه برای کلیه پرسنل (یا انتخاب شده)  بر مبنای سال]";
		((ButtonBase)rbMinSalary).UseVisualStyleBackColor = false;
		((Control)rbIncreasePercentage).BackColor = Color.Transparent;
		rbIncreasePercentage.Checked = true;
		((Control)rbIncreasePercentage).Location = new Point(687, 119);
		((Control)rbIncreasePercentage).Name = "rbIncreasePercentage";
		((Control)rbIncreasePercentage).Size = new Size(65, 21);
		((Control)rbIncreasePercentage).TabIndex = 8;
		rbIncreasePercentage.TabStop = true;
		((Control)rbIncreasePercentage).Text = "[افزایش]";
		((ButtonBase)rbIncreasePercentage).UseVisualStyleBackColor = false;
		((Control)TxtGharardadNo).Anchor = (AnchorStyles)13;
		((Control)TxtGharardadNo).Location = new Point(522, 9);
		TxtGharardadNo.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		TxtGharardadNo.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TxtGharardadNo).Name = "TxtGharardadNo";
		((Control)TxtGharardadNo).Size = new Size(99, 21);
		((Control)TxtGharardadNo).TabIndex = 1;
		((UpDownBase)TxtGharardadNo).TextAlign = (HorizontalAlignment)2;
		TxtGharardadNo.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		TXTDateRegister.BackcolorTooltip = Color.Red;
		TXTDateRegister.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TXTDateRegister).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTDateRegister).Location = new Point(287, 9);
		TXTDateRegister.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TXTDateRegister).Name = "TXTDateRegister";
		((Control)TXTDateRegister).Size = new Size(107, 26);
		((Control)TXTDateRegister).TabIndex = 3;
		((Control)TXTDateRegister).Text = "1391/12/12";
		((TextBox)TXTDateRegister).TextAlign = (HorizontalAlignment)2;
		TXTDateRegister.TootTipDelay = 500;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.Class = "";
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(627, 9);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(125, 21);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[شماره شروع قرار داد]";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(400, 9);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(112, 21);
		((Control)labelX3).TabIndex = 2;
		((Control)labelX3).Text = "[,تاریخ ثبت قرارداد ها] :";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtEndDate.BackcolorTooltip = Color.Red;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TxtEndDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtEndDate).Location = new Point(287, 64);
		TxtEndDate.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).Size = new Size(107, 26);
		((Control)TxtEndDate).TabIndex = 7;
		((Control)TxtEndDate).Text = "1391/12/12";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TXTStartDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate).Location = new Point(522, 64);
		TXTStartDate.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).Size = new Size(99, 26);
		((Control)TXTStartDate).TabIndex = 5;
		((Control)TXTStartDate).Text = "1391/12/12";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(400, 64);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(116, 26);
		((Control)labelX8).TabIndex = 6;
		((Control)labelX8).Text = "[تاریخ پایان قرارداد ها] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.Class = "";
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(627, 64);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(125, 26);
		((Control)labelX10).TabIndex = 4;
		((Control)labelX10).Text = "[تاریخ شروع قرارداد ها] :";
		labelX10.TextAlignment = (StringAlignment)2;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 502);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 502);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[مزایا و کسورات]";
		((Control)checkBoxX1).AutoSize = true;
		((Control)checkBoxX1).BackColor = Color.Transparent;
		((BaseItemControl)checkBoxX1).BackgroundStyle.Class = "";
		((BaseItemControl)checkBoxX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)checkBoxX1).Location = new Point(405, 297);
		((Control)checkBoxX1).Name = "checkBoxX1";
		((Control)checkBoxX1).Size = new Size(347, 16);
		((BaseItemControl)checkBoxX1).Style = (eDotNetBarStyle)9;
		((Control)checkBoxX1).TabIndex = 17;
		((Control)checkBoxX1).Text = "[شیفت ها هم بر مبنای تاریخ قراردادها به سال جدید منتقل شود]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmCopyGharardadsNew";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[انتقال قراردادها به سال جدید]";
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel1).ResumeLayout(false);
		((Control)tabControlPanel1).PerformLayout();
		((Control)panelEx2).ResumeLayout(false);
		((Control)panelEx2).PerformLayout();
		((ISupportInitialize)TxtGharardadNo).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
