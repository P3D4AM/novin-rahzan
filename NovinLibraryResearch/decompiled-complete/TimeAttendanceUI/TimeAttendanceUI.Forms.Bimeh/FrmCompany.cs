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
using FormCustom.Waiting;
using PacketParser;
using Serilog;
using TimeAttendance.ValidatorAttribute.Company;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Bimeh;

public class FrmCompany : FormExtention
{
	private Company _clsCompany;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private string _frmText = "";

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private LabelX labelX1;

	private LabelX labelX5;

	private Label label1;

	private TextBoxX TxtNerkh;

	private TextBoxX TxtAddressCompany;

	private TextBoxX TxtNameEmployer;

	private TextBoxX TxtNameCo;

	private LabelX labelX6;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private TextBoxX TxtNOPeyman;

	private TextBoxX txtWorkShopNo;

	private TextBoxX TxtInsuranceBranch;

	private TextBoxX TxtWorkShopName;

	private LabelX labelX8;

	private LabelX labelX7;

	private TextBoxX txtWorkShopAddress;

	private LabelX labelX9;

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
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmCompany(string contextGuid)
		: base(contextGuid, true, true)
	{
		_frmText = "[افزودن شرکت جدید]";
		InitializeComponent();
		_clsCompany = new Company();
		((Control)txtWorkShopNo).Focus();
		((Control)TxtNerkh).Text = "23";
	}

	public FrmCompany(string contextGuid, short id)
		: base(contextGuid, true, true)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		_frmText = "[ویرایش شرکت]";
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(id, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmCompany.cs");
		((Control)txtWorkShopNo).Focus();
	}

	public FrmCompany(string contextGuid, Company pCompany)
		: base(contextGuid, true, true)
	{
		_frmText = "[مشاهده لاگ عملکرد کاربران ( قفط خواندنی ) اطلاعات شرکت]";
		InitializeComponent();
		_clsCompany = pCompany;
		((Control)BtnAccept).Enabled = false;
		((Control)txtWorkShopNo).Focus();
	}

	private async Task LoadDataAsync(short companyId, CancellationToken token)
	{
		try
		{
			((Control)this).Text = Utilities.GetTranslate(_frmText);
			if (_clsCompany == null)
			{
				if (companyId > 0)
				{
					_clsCompany = await Company.GetAsync(((CacheBaseForm)this).ContextGuid, companyId, token);
				}
				else
				{
					_clsCompany = new Company();
				}
			}
			FillForm();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void FillForm()
	{
		((Control)TxtNameCo).Text = _clsCompany.Name;
		((Control)TxtNameEmployer).Text = _clsCompany.NameKarfarma;
		((Control)TxtAddressCompany).Text = _clsCompany.Address;
		((Control)TxtNerkh).Text = _clsCompany.NerkhBimeh.ToString();
		((Control)txtWorkShopNo).Text = _clsCompany.Code;
		((Control)TxtNOPeyman).Text = _clsCompany.NoPeyman;
		((Control)TxtWorkShopName).Text = _clsCompany.NameWorkShop;
		((Control)TxtInsuranceBranch).Text = _clsCompany.InsuranceBranch;
		((Control)txtWorkShopAddress).Text = _clsCompany.AddressWorkShop;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
	}

	private void GetData()
	{
		_clsCompany.Name = ((Control)TxtNameCo).Text;
		_clsCompany.NameKarfarma = ((Control)TxtNameEmployer).Text;
		_clsCompany.Address = ((Control)TxtAddressCompany).Text;
		_clsCompany.NerkhBimeh = Utilities.ParseToInt(((Control)TxtNerkh).Text, 0);
		_clsCompany.Code = ((Control)txtWorkShopNo).Text;
		_clsCompany.NoPeyman = ((Control)TxtNOPeyman).Text;
		_clsCompany.InsuranceBranch = ((Control)TxtInsuranceBranch).Text;
		_clsCompany.NameWorkShop = ((Control)TxtWorkShopName).Text;
		_clsCompany.AddressWorkShop = ((Control)txtWorkShopAddress).Text;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			GetData();
			DialogResult val = CompanyValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _clsCompany, isShowMessage: true);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					((Form)this).DialogResult = (DialogResult)((await _clsCompany.SaveAsync(token)) ? 1 : 2);
					((FormExtention)this).ResetControlChanged();
					Company.Reset();
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
			Log.Error(ex, "");
		}
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
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1159: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		TxtInsuranceBranch = new TextBoxX();
		TxtWorkShopName = new TextBoxX();
		txtWorkShopNo = new TextBoxX();
		TxtNOPeyman = new TextBoxX();
		label1 = new Label();
		TxtNerkh = new TextBoxX();
		TxtAddressCompany = new TextBoxX();
		TxtNameEmployer = new TextBoxX();
		txtWorkShopAddress = new TextBoxX();
		TxtNameCo = new TextBoxX();
		labelX6 = new LabelX();
		labelX8 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX9 = new LabelX();
		labelX7 = new LabelX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)7;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(601, 337);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(15, 281);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(121, 281);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)TxtInsuranceBranch);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtWorkShopName);
		((Control)groupPanel7).Controls.Add((Control)(object)txtWorkShopNo);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtNOPeyman);
		((Control)groupPanel7).Controls.Add((Control)(object)label1);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtNerkh);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtAddressCompany);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtNameEmployer);
		((Control)groupPanel7).Controls.Add((Control)(object)txtWorkShopAddress);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtNameCo);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(15, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(574, 264);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		TxtInsuranceBranch.Border.Class = "TextBoxBorder";
		TxtInsuranceBranch.Border.CornerType = (eCornerType)1;
		((Control)TxtInsuranceBranch).Location = new Point(235, 172);
		((TextBoxBase)TxtInsuranceBranch).MaxLength = 100;
		((Control)TxtInsuranceBranch).Name = "TxtInsuranceBranch";
		((Control)TxtInsuranceBranch).Size = new Size(148, 21);
		((Control)TxtInsuranceBranch).TabIndex = 14;
		TxtWorkShopName.Border.Class = "TextBoxBorder";
		TxtWorkShopName.Border.CornerType = (eCornerType)1;
		((Control)TxtWorkShopName).Location = new Point(3, 172);
		((TextBoxBase)TxtWorkShopName).MaxLength = 100;
		((Control)TxtWorkShopName).Name = "TxtWorkShopName";
		((Control)TxtWorkShopName).Size = new Size(151, 21);
		((Control)TxtWorkShopName).TabIndex = 16;
		txtWorkShopNo.Border.Class = "TextBoxBorder";
		txtWorkShopNo.Border.CornerType = (eCornerType)1;
		((Control)txtWorkShopNo).Location = new Point(235, 145);
		((TextBoxBase)txtWorkShopNo).MaxLength = 10;
		((Control)txtWorkShopNo).Name = "txtWorkShopNo";
		((Control)txtWorkShopNo).Size = new Size(148, 21);
		((Control)txtWorkShopNo).TabIndex = 10;
		TxtNOPeyman.Border.Class = "TextBoxBorder";
		TxtNOPeyman.Border.CornerType = (eCornerType)1;
		((Control)TxtNOPeyman).Location = new Point(3, 145);
		((TextBoxBase)TxtNOPeyman).MaxLength = 3;
		((Control)TxtNOPeyman).Name = "TxtNOPeyman";
		((Control)TxtNOPeyman).Size = new Size(151, 21);
		((Control)TxtNOPeyman).TabIndex = 12;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(297, 118);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(20, 13);
		((Control)label1).TabIndex = 8;
		((Control)label1).Text = "%";
		TxtNerkh.Border.Class = "TextBoxBorder";
		TxtNerkh.Border.CornerType = (eCornerType)1;
		((Control)TxtNerkh).Location = new Point(323, 118);
		((TextBoxBase)TxtNerkh).MaxLength = 5;
		((Control)TxtNerkh).Name = "TxtNerkh";
		((Control)TxtNerkh).Size = new Size(60, 21);
		((Control)TxtNerkh).TabIndex = 7;
		TxtAddressCompany.Border.Class = "TextBoxBorder";
		TxtAddressCompany.Border.CornerType = (eCornerType)1;
		((Control)TxtAddressCompany).Location = new Point(3, 57);
		((TextBoxBase)TxtAddressCompany).MaxLength = 100;
		((TextBoxBase)TxtAddressCompany).Multiline = true;
		((Control)TxtAddressCompany).Name = "TxtAddressCompany";
		((Control)TxtAddressCompany).Size = new Size(380, 55);
		((Control)TxtAddressCompany).TabIndex = 5;
		TxtNameEmployer.Border.Class = "TextBoxBorder";
		TxtNameEmployer.Border.CornerType = (eCornerType)1;
		((Control)TxtNameEmployer).Location = new Point(3, 30);
		((TextBoxBase)TxtNameEmployer).MaxLength = 100;
		((Control)TxtNameEmployer).Name = "TxtNameEmployer";
		((Control)TxtNameEmployer).Size = new Size(380, 21);
		((Control)TxtNameEmployer).TabIndex = 3;
		txtWorkShopAddress.Border.Class = "TextBoxBorder";
		txtWorkShopAddress.Border.CornerType = (eCornerType)1;
		((Control)txtWorkShopAddress).Location = new Point(3, 199);
		((TextBoxBase)txtWorkShopAddress).MaxLength = 100;
		((TextBoxBase)txtWorkShopAddress).Multiline = true;
		((Control)txtWorkShopAddress).Name = "txtWorkShopAddress";
		((Control)txtWorkShopAddress).Size = new Size(380, 54);
		((Control)txtWorkShopAddress).TabIndex = 18;
		TxtNameCo.Border.Class = "TextBoxBorder";
		TxtNameCo.Border.CornerType = (eCornerType)1;
		((Control)TxtNameCo).Location = new Point(3, 3);
		((TextBoxBase)TxtNameCo).MaxLength = 100;
		((Control)TxtNameCo).Name = "TxtNameCo";
		((Control)TxtNameCo).Size = new Size(380, 21);
		((Control)TxtNameCo).TabIndex = 1;
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(389, 118);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(85, 21);
		((Control)labelX6).TabIndex = 6;
		((Control)labelX6).Text = "[نرخ حق بیمه] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(160, 172);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(69, 21);
		((Control)labelX8).TabIndex = 15;
		((Control)labelX8).Text = "[نام کارگاه] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(389, 59);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(85, 23);
		((Control)labelX4).TabIndex = 4;
		((Control)labelX4).Text = "[آدرس]";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(160, 145);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(69, 21);
		((Control)labelX3).TabIndex = 11;
		((Control)labelX3).Text = "[ردیف پیمان] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(389, 32);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(85, 21);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "[نام کارفرما] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(391, 199);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(85, 27);
		((Control)labelX9).TabIndex = 17;
		((Control)labelX9).Text = "[آدرس کارگاه] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(391, 172);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(138, 21);
		((Control)labelX7).TabIndex = 13;
		((Control)labelX7).Text = "[نام شعبه تامین اجتماعی] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(389, 1);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(112, 21);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[نام شرکت] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(391, 145);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(85, 21);
		((Control)labelX5).TabIndex = 9;
		((Control)labelX5).Text = "[کد کارگاه] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(601, 337);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(500, 309);
		((Control)this).Name = "FrmCompany";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "افزودن شرکت";
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanel7).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
