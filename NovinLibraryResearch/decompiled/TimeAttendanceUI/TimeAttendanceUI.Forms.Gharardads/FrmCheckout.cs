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
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Validators;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmCheckout : FormExtention
{
	private TimeAttendanceDAL.Dal.Salary.Checkout clsCheckout;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private GharardadNew _gharardad;

	private IContainer components;

	private PanelEx panelEx1;

	private TextBoxX txtGharardadNo;

	private LabelX labelX2;

	private LabelX labelX4;

	private LabelX labelX3;

	private TextBoxFa txtFullName;

	private LabelX lblPrice;

	private TextBoxX txtDescription;

	private LabelX labelX5;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private CheckBoxX ChBxIsMoveMission;

	private LabelX labelX10;

	private LabelX labelX9;

	private TextBoxFa txtRestMission;

	private TextBoxFa txtOtherBenefitMab;

	private TextBoxFa txtSeverancePay;

	private TextBoxFa txtReward;

	private LabelX labelX1;

	private GroupPanel groupPanel1;

	private LabelX labelX7;

	private TextBoxFa txtOtherDeficitDesc;

	private LabelX labelX8;

	private TextBoxFa txtOtherDeficitMab;

	private GroupPanel groupPanel3;

	private TextBoxFa txtOtherBenefitDesc;

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

	public FrmCheckout(string contextGuid, GharardadNew gharardad)
		: base(contextGuid, true, true)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_gharardad = gharardad;
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCheckout.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			clsCheckout = await TimeAttendanceDAL.Dal.Salary.Checkout.GetAsync(((CacheBaseForm)this).ContextGuid, _gharardad, token);
			base.objsCanEmpty.Add((Control)(object)txtOtherBenefitDesc);
			base.objsCanEmpty.Add((Control)(object)txtOtherBenefitMab);
			base.objsCanEmpty.Add((Control)(object)txtOtherDeficitDesc);
			base.objsCanEmpty.Add((Control)(object)txtOtherDeficitMab);
			base.objsCanEmpty.Add((Control)(object)txtRestMission);
			await FillFormAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public async Task FillFormAsync(CancellationToken token)
	{
		GharardadNew gharardadNew = await clsCheckout.GetGharardadAsync(token);
		string _txtGharardadNo = gharardadNew.No;
		string _txtFullName = gharardadNew.FullName;
		string _txtRestMission = clsCheckout.RestMission.ToString();
		string _txtReward = clsCheckout.RewardMab.ToString();
		string _txtSeverancePay = clsCheckout.SeverancepayMab.ToString();
		string _txtOtherBenefitDesc = clsCheckout.OtherBenefitDesc;
		string _txtOtherBenefitMab = clsCheckout.OtherBenefitMab.ToString();
		string _txtOtherDeficitDesc = clsCheckout.OtherDeficitDesc;
		string _txtOtherDeficitMab = clsCheckout.OtherDeficitMab.ToString();
		string _txtDescription = clsCheckout.Desccription;
		while (!((Control)this).IsHandleCreated)
		{
			await Task.Delay(100);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
		}
		((Control)this).BeginInvoke((Delegate)(Action)delegate
		{
			try
			{
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					((Control)txtGharardadNo).Text = _txtGharardadNo;
					((Control)txtFullName).Text = _txtFullName;
					((Control)txtRestMission).Text = _txtRestMission;
					((Control)txtReward).Text = _txtReward;
					((Control)txtSeverancePay).Text = _txtSeverancePay;
					((Control)txtOtherBenefitDesc).Text = _txtOtherBenefitDesc;
					((Control)txtOtherBenefitMab).Text = _txtOtherBenefitMab;
					((Control)txtOtherDeficitDesc).Text = _txtOtherDeficitDesc;
					((Control)txtOtherDeficitMab).Text = _txtOtherDeficitMab;
					((Control)txtDescription).Text = _txtDescription;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
			}
		});
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Visible || !((Control)BtnAccept).Enabled)
		{
			return;
		}
		try
		{
			if (await GetDataAsync())
			{
				DialogResult val = CheckValidation.Validtor(((CacheBaseForm)this).ContextGuid, clsCheckout.CheckValidation(), isShowMessage: true);
				if ((int)val == 6 || (int)val == 5)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await clsCheckout.SaveAsync(token);
					((FormExtention)this).ResetControlChanged();
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else if ((int)val == 2)
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCheckout.cs", 139);
		}
	}

	private async Task<bool> GetDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			(await clsCheckout.GetGharardadAsync(token)).No = ((Control)txtGharardadNo).Text;
			clsCheckout.RestMission = Utilities.ParseToInt(((Control)txtRestMission).Text, 0);
			clsCheckout.RewardMab = Utilities.ParseToDecimal(((Control)txtReward).Text, 0m, 0);
			clsCheckout.SeverancepayMab = Utilities.ParseToDecimal(((Control)txtSeverancePay).Text, 0m, 0);
			clsCheckout.OtherBenefitDesc = ((Control)txtOtherBenefitDesc).Text;
			clsCheckout.OtherBenefitMab = Utilities.ParseToDecimal(((Control)txtOtherBenefitMab).Text, 0m, 0);
			clsCheckout.OtherDeficitDesc = ((Control)txtOtherDeficitDesc).Text;
			clsCheckout.OtherDeficitMab = Utilities.ParseToDecimal(((Control)txtOtherDeficitMab).Text, 0m, 0);
			clsCheckout.Desccription = ((Control)txtDescription).Text;
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return await Task.FromResult(result: false);
	}

	private void FrmCheckout_Load(object sender, EventArgs e)
	{
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
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
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Expected O, but got Unknown
		//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bd: Expected O, but got Unknown
		//IL_144e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1589: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d17: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		labelX1 = new LabelX();
		groupPanel1 = new GroupPanel();
		labelX7 = new LabelX();
		txtOtherDeficitDesc = new TextBoxFa();
		labelX8 = new LabelX();
		txtOtherDeficitMab = new TextBoxFa();
		groupPanel3 = new GroupPanel();
		labelX9 = new LabelX();
		txtOtherBenefitDesc = new TextBoxFa();
		labelX10 = new LabelX();
		txtOtherBenefitMab = new TextBoxFa();
		ChBxIsMoveMission = new CheckBoxX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		txtDescription = new TextBoxX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		txtSeverancePay = new TextBoxFa();
		txtReward = new TextBoxFa();
		txtRestMission = new TextBoxFa();
		txtFullName = new TextBoxFa();
		lblPrice = new LabelX();
		txtGharardadNo = new TextBoxX();
		labelX2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)groupPanel3).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel3);
		((Control)panelEx1).Controls.Add((Control)(object)ChBxIsMoveMission);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)txtDescription);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)txtSeverancePay);
		((Control)panelEx1).Controls.Add((Control)(object)txtReward);
		((Control)panelEx1).Controls.Add((Control)(object)txtRestMission);
		((Control)panelEx1).Controls.Add((Control)(object)txtFullName);
		((Control)panelEx1).Controls.Add((Control)(object)lblPrice);
		((Control)panelEx1).Controls.Add((Control)(object)txtGharardadNo);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(437, 491);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(320, 370);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(82, 21);
		((Control)labelX1).TabIndex = 13;
		((Control)labelX1).Text = "[شرح]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel1).Controls.Add((Control)(object)txtOtherDeficitDesc);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel1).Controls.Add((Control)(object)txtOtherDeficitMab);
		((Control)groupPanel1).Location = new Point(14, 273);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(388, 91);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.Class = "";
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.Class = "";
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.Class = "";
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 12;
		((Control)groupPanel1).Text = "سایر کسورات";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(303, 13);
		((Control)labelX7).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(76, 21);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[شرح] :";
		labelX7.TextAlignment = (StringAlignment)2;
		txtOtherDeficitDesc.AllowEmpty = true;
		((Control)txtOtherDeficitDesc).Anchor = (AnchorStyles)9;
		txtOtherDeficitDesc.BackColorTextBoxWhenFocus = Color.Yellow;
		txtOtherDeficitDesc.BackColorTextBoxWhenLostFocus = Color.White;
		txtOtherDeficitDesc.BackcolorTooltip = Color.Red;
		txtOtherDeficitDesc.CurrencyFormat = "";
		txtOtherDeficitDesc.ForeColorTooltip = Color.Black;
		txtOtherDeficitDesc.GetColoresFromSettingDLL = true;
		txtOtherDeficitDesc.HandleKeydown = true;
		((Control)txtOtherDeficitDesc).Location = new Point(3, 13);
		((TextBoxBase)txtOtherDeficitDesc).MaxLength = 19;
		((Control)txtOtherDeficitDesc).Name = "txtOtherDeficitDesc";
		txtOtherDeficitDesc.SendShiftTabAfterDown = true;
		txtOtherDeficitDesc.SendShiftTabAfterEnter = true;
		txtOtherDeficitDesc.SendTabAfterEnter = true;
		txtOtherDeficitDesc.SendTabAfterUP = true;
		txtOtherDeficitDesc.ShowHorofi = (ENShowHorofi)3;
		txtOtherDeficitDesc.ShowToolTipText = true;
		((Control)txtOtherDeficitDesc).Size = new Size(294, 21);
		((Control)txtOtherDeficitDesc).TabIndex = 1;
		((Control)txtOtherDeficitDesc).Text = "0";
		txtOtherDeficitDesc.ToolTipPostFix = "";
		txtOtherDeficitDesc.TootTipDelay = 1000;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(303, 45);
		((Control)labelX8).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(36, 21);
		((Control)labelX8).TabIndex = 2;
		((Control)labelX8).Text = "[مبلغ] :";
		labelX8.TextAlignment = (StringAlignment)2;
		txtOtherDeficitMab.AllowEmpty = true;
		((Control)txtOtherDeficitMab).Anchor = (AnchorStyles)9;
		txtOtherDeficitMab.BackColorTextBoxWhenFocus = Color.Yellow;
		txtOtherDeficitMab.BackColorTextBoxWhenLostFocus = Color.White;
		txtOtherDeficitMab.BackcolorTooltip = Color.Red;
		txtOtherDeficitMab.CurrencyFormat = "";
		txtOtherDeficitMab.ForeColorTooltip = Color.Black;
		txtOtherDeficitMab.GetColoresFromSettingDLL = true;
		txtOtherDeficitMab.HandleKeydown = true;
		((Control)txtOtherDeficitMab).Location = new Point(147, 45);
		((TextBoxBase)txtOtherDeficitMab).MaxLength = 19;
		((Control)txtOtherDeficitMab).Name = "txtOtherDeficitMab";
		txtOtherDeficitMab.SendShiftTabAfterDown = true;
		txtOtherDeficitMab.SendShiftTabAfterEnter = true;
		txtOtherDeficitMab.SendTabAfterEnter = true;
		txtOtherDeficitMab.SendTabAfterUP = true;
		txtOtherDeficitMab.ShowHorofi = (ENShowHorofi)3;
		txtOtherDeficitMab.ShowToolTipText = true;
		((Control)txtOtherDeficitMab).Size = new Size(150, 21);
		((Control)txtOtherDeficitMab).TabIndex = 3;
		((Control)txtOtherDeficitMab).Text = "0";
		txtOtherDeficitMab.ToolTipPostFix = "";
		txtOtherDeficitMab.TootTipDelay = 1000;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel3).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel3).Controls.Add((Control)(object)txtOtherBenefitDesc);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX10);
		((Control)groupPanel3).Controls.Add((Control)(object)txtOtherBenefitMab);
		((Control)groupPanel3).Location = new Point(14, 176);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(388, 91);
		((PanelControl)groupPanel3).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel3).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel3).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel3).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel3).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel3).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel3).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel3).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel3).Style.Class = "";
		((PanelControl)groupPanel3).Style.CornerDiameter = 4;
		((PanelControl)groupPanel3).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.Class = "";
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.Class = "";
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 11;
		((Control)groupPanel3).Text = "سایر مزایا";
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.Class = "";
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(303, 13);
		((Control)labelX9).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(76, 21);
		((Control)labelX9).TabIndex = 0;
		((Control)labelX9).Text = "[شرح]:";
		labelX9.TextAlignment = (StringAlignment)2;
		txtOtherBenefitDesc.AllowEmpty = true;
		((Control)txtOtherBenefitDesc).Anchor = (AnchorStyles)9;
		txtOtherBenefitDesc.BackColorTextBoxWhenFocus = Color.Yellow;
		txtOtherBenefitDesc.BackColorTextBoxWhenLostFocus = Color.White;
		txtOtherBenefitDesc.BackcolorTooltip = Color.Red;
		txtOtherBenefitDesc.CurrencyFormat = "";
		txtOtherBenefitDesc.ForeColorTooltip = Color.Black;
		txtOtherBenefitDesc.GetColoresFromSettingDLL = true;
		txtOtherBenefitDesc.HandleKeydown = true;
		((Control)txtOtherBenefitDesc).Location = new Point(3, 13);
		((TextBoxBase)txtOtherBenefitDesc).MaxLength = 19;
		((Control)txtOtherBenefitDesc).Name = "txtOtherBenefitDesc";
		txtOtherBenefitDesc.SendShiftTabAfterDown = true;
		txtOtherBenefitDesc.SendShiftTabAfterEnter = true;
		txtOtherBenefitDesc.SendTabAfterEnter = true;
		txtOtherBenefitDesc.SendTabAfterUP = true;
		txtOtherBenefitDesc.ShowHorofi = (ENShowHorofi)3;
		txtOtherBenefitDesc.ShowToolTipText = true;
		((Control)txtOtherBenefitDesc).Size = new Size(294, 21);
		((Control)txtOtherBenefitDesc).TabIndex = 1;
		((Control)txtOtherBenefitDesc).Text = "0";
		txtOtherBenefitDesc.ToolTipPostFix = "";
		txtOtherBenefitDesc.TootTipDelay = 1000;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.Class = "";
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(303, 45);
		((Control)labelX10).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(36, 21);
		((Control)labelX10).TabIndex = 2;
		((Control)labelX10).Text = "[مبلغ] :";
		labelX10.TextAlignment = (StringAlignment)2;
		txtOtherBenefitMab.AllowEmpty = true;
		((Control)txtOtherBenefitMab).Anchor = (AnchorStyles)9;
		txtOtherBenefitMab.BackColorTextBoxWhenFocus = Color.Yellow;
		txtOtherBenefitMab.BackColorTextBoxWhenLostFocus = Color.White;
		txtOtherBenefitMab.BackcolorTooltip = Color.Red;
		txtOtherBenefitMab.CurrencyFormat = "";
		txtOtherBenefitMab.ForeColorTooltip = Color.Black;
		txtOtherBenefitMab.GetColoresFromSettingDLL = true;
		txtOtherBenefitMab.HandleKeydown = true;
		((Control)txtOtherBenefitMab).Location = new Point(147, 45);
		((TextBoxBase)txtOtherBenefitMab).MaxLength = 19;
		((Control)txtOtherBenefitMab).Name = "txtOtherBenefitMab";
		txtOtherBenefitMab.SendShiftTabAfterDown = true;
		txtOtherBenefitMab.SendShiftTabAfterEnter = true;
		txtOtherBenefitMab.SendTabAfterEnter = true;
		txtOtherBenefitMab.SendTabAfterUP = true;
		txtOtherBenefitMab.ShowHorofi = (ENShowHorofi)3;
		txtOtherBenefitMab.ShowToolTipText = true;
		((Control)txtOtherBenefitMab).Size = new Size(150, 21);
		((Control)txtOtherBenefitMab).TabIndex = 3;
		((Control)txtOtherBenefitMab).Text = "0";
		txtOtherBenefitMab.ToolTipPostFix = "";
		txtOtherBenefitMab.TootTipDelay = 1000;
		((BaseItemControl)ChBxIsMoveMission).BackgroundStyle.Class = "";
		((BaseItemControl)ChBxIsMoveMission).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxIsMoveMission).Location = new Point(92, 66);
		((Control)ChBxIsMoveMission).Name = "ChBxIsMoveMission";
		((Control)ChBxIsMoveMission).Size = new Size(222, 23);
		((BaseItemControl)ChBxIsMoveMission).Style = (eDotNetBarStyle)9;
		((Control)ChBxIsMoveMission).TabIndex = 4;
		((Control)ChBxIsMoveMission).Text = "[انتقال مانده مرخصی به قرارداد بعد]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 454);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 16;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(137, 454);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 15;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		txtDescription.Border.Class = "TextBoxBorder";
		txtDescription.Border.CornerType = (eCornerType)1;
		((Control)txtDescription).Location = new Point(14, 370);
		((TextBoxBase)txtDescription).Multiline = true;
		((Control)txtDescription).Name = "txtDescription";
		((Control)txtDescription).Size = new Size(300, 75);
		((Control)txtDescription).TabIndex = 14;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(320, 149);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(80, 21);
		((Control)labelX5).TabIndex = 9;
		((Control)labelX5).Text = "[Sanvat] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(320, 95);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(97, 19);
		((Control)labelX4).TabIndex = 5;
		((Control)labelX4).Text = "[مانده مرخصی]:";
		labelX4.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(320, 12);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(97, 19);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[شماره قرارداد] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Visible = false;
		txtSeverancePay.AllowEmpty = true;
		txtSeverancePay.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSeverancePay.BackColorTextBoxWhenLostFocus = Color.White;
		txtSeverancePay.BackcolorTooltip = Color.Red;
		txtSeverancePay.CurrencyFormat = "";
		txtSeverancePay.ForeColorTooltip = Color.Black;
		txtSeverancePay.GetColoresFromSettingDLL = true;
		txtSeverancePay.HandleKeydown = true;
		((Control)txtSeverancePay).Location = new Point(164, 149);
		((TextBoxBase)txtSeverancePay).MaxLength = 19;
		((Control)txtSeverancePay).Name = "txtSeverancePay";
		txtSeverancePay.SendShiftTabAfterDown = true;
		txtSeverancePay.SendShiftTabAfterEnter = true;
		txtSeverancePay.SendTabAfterEnter = true;
		txtSeverancePay.SendTabAfterUP = true;
		txtSeverancePay.ShowHorofi = (ENShowHorofi)3;
		txtSeverancePay.ShowToolTipText = true;
		((Control)txtSeverancePay).Size = new Size(150, 21);
		((Control)txtSeverancePay).TabIndex = 10;
		((Control)txtSeverancePay).Text = "0";
		txtSeverancePay.ToolTipPostFix = "";
		txtSeverancePay.TootTipDelay = 1000;
		txtReward.AllowEmpty = true;
		txtReward.BackColorTextBoxWhenFocus = Color.Yellow;
		txtReward.BackColorTextBoxWhenLostFocus = Color.White;
		txtReward.BackcolorTooltip = Color.Red;
		txtReward.CurrencyFormat = "";
		txtReward.ForeColorTooltip = Color.Black;
		txtReward.GetColoresFromSettingDLL = true;
		txtReward.HandleKeydown = true;
		((Control)txtReward).Location = new Point(164, 122);
		((TextBoxBase)txtReward).MaxLength = 19;
		((Control)txtReward).Name = "txtReward";
		txtReward.SendShiftTabAfterDown = true;
		txtReward.SendShiftTabAfterEnter = true;
		txtReward.SendTabAfterEnter = true;
		txtReward.SendTabAfterUP = true;
		txtReward.ShowHorofi = (ENShowHorofi)3;
		txtReward.ShowToolTipText = true;
		((Control)txtReward).Size = new Size(150, 21);
		((Control)txtReward).TabIndex = 8;
		((Control)txtReward).Text = "0";
		txtReward.ToolTipPostFix = "";
		txtReward.TootTipDelay = 1000;
		txtRestMission.AllowEmpty = true;
		txtRestMission.BackColorTextBoxWhenFocus = Color.Yellow;
		txtRestMission.BackColorTextBoxWhenLostFocus = Color.White;
		txtRestMission.BackcolorTooltip = Color.Red;
		txtRestMission.CurrencyFormat = "";
		txtRestMission.ForeColorTooltip = Color.Black;
		txtRestMission.GetColoresFromSettingDLL = true;
		txtRestMission.HandleKeydown = true;
		((Control)txtRestMission).Location = new Point(164, 95);
		((TextBoxBase)txtRestMission).MaxLength = 19;
		((Control)txtRestMission).Name = "txtRestMission";
		txtRestMission.SendShiftTabAfterDown = true;
		txtRestMission.SendShiftTabAfterEnter = true;
		txtRestMission.SendTabAfterEnter = true;
		txtRestMission.SendTabAfterUP = true;
		txtRestMission.ShowHorofi = (ENShowHorofi)3;
		txtRestMission.ShowToolTipText = true;
		((Control)txtRestMission).Size = new Size(150, 21);
		((Control)txtRestMission).TabIndex = 6;
		((Control)txtRestMission).Text = "0";
		txtRestMission.ToolTipPostFix = "";
		txtRestMission.TootTipDelay = 1000;
		txtFullName.AllowEmpty = true;
		txtFullName.BackColorTextBoxWhenFocus = Color.Yellow;
		txtFullName.BackColorTextBoxWhenLostFocus = Color.White;
		txtFullName.BackcolorTooltip = Color.Red;
		txtFullName.CurrencyFormat = "";
		txtFullName.ForeColorTooltip = Color.Black;
		txtFullName.GetColoresFromSettingDLL = true;
		txtFullName.HandleKeydown = true;
		((Control)txtFullName).Location = new Point(14, 39);
		((TextBoxBase)txtFullName).MaxLength = 19;
		((Control)txtFullName).Name = "txtFullName";
		txtFullName.SendShiftTabAfterDown = true;
		txtFullName.SendShiftTabAfterEnter = true;
		txtFullName.SendTabAfterEnter = true;
		txtFullName.SendTabAfterUP = true;
		txtFullName.ShowHorofi = (ENShowHorofi)3;
		txtFullName.ShowToolTipText = true;
		((Control)txtFullName).Size = new Size(300, 21);
		((Control)txtFullName).TabIndex = 3;
		((Control)txtFullName).Text = "0";
		txtFullName.ToolTipPostFix = "";
		txtFullName.TootTipDelay = 1000;
		((BaseItemControl)lblPrice).BackgroundStyle.Class = "";
		((BaseItemControl)lblPrice).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblPrice).Location = new Point(320, 124);
		((Control)lblPrice).Margin = new Padding(3, 2, 3, 2);
		((Control)lblPrice).Name = "lblPrice";
		((Control)lblPrice).Size = new Size(80, 19);
		((Control)lblPrice).TabIndex = 7;
		((Control)lblPrice).Text = "[عیدی] :";
		lblPrice.TextAlignment = (StringAlignment)2;
		txtGharardadNo.Border.Class = "TextBoxBorder";
		txtGharardadNo.Border.CornerType = (eCornerType)1;
		((Control)txtGharardadNo).Enabled = false;
		((Control)txtGharardadNo).Location = new Point(164, 12);
		((Control)txtGharardadNo).Name = "txtGharardadNo";
		((Control)txtGharardadNo).Size = new Size(150, 21);
		((Control)txtGharardadNo).TabIndex = 1;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(320, 39);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(130, 19);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "[نام و نام خانوادگی] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(437, 513);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(420, 342);
		((Control)this).Name = "FrmCheckout";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "افزودن مزایا و کسورات*";
		((Form)this).Load += FrmCheckout_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)groupPanel1).ResumeLayout(false);
		((Control)groupPanel1).PerformLayout();
		((Control)groupPanel3).ResumeLayout(false);
		((Control)groupPanel3).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
