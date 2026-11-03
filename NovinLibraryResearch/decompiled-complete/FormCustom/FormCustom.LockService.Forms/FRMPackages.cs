using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.LkService;
using PacketParser.Entities;

namespace FormCustom.LockService.Forms;

public class FRMPackages : AccountingBaseForm
{
	private bool _isDisposed;

	private Container components;

	internal Label LBLTolid;

	internal Button BTNTolid;

	internal Button BTNApplication;

	internal Label LBLSoftware;

	internal Button BTNCancel;

	internal Button BTNBarcode;

	internal Label Label1;

	internal Button BTNTarazo;

	internal Label Label2;

	internal Button BTNCallerId;

	internal Label Label3;

	internal PanelEx panelEx1;

	internal Button btnVoiceCall;

	internal Label label12;

	internal Button btnMultCurrency;

	internal Label label11;

	internal Button btnProductDepotPishrafteh;

	internal Label label10;

	internal Button btnProductSerial;

	internal Label label9;

	internal Button btnCustomerClub;

	internal Label label8;

	internal Button btbChequeKhazane;

	internal Label label7;

	internal Button btnAghsat;

	internal Label label6;

	internal Button btnTax;

	internal Label label5;

	internal Label label13;

	internal Button btnService;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
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
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMPackages.cs", 28);
		}
	}

	public FRMPackages(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			Task.Run(() => LkActivatorBaseBusiness.CheckLKWithWebServiceAsync(base.ContextGuid, Package.Accounting));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMPackages.cs", 38);
		}
	}

	public void FRMPackages_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			BTNCancel.PerformClick();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMPackages_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMPackages.cs", 47);
		}
	}

	public void Button1_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMPackages.cs", 57);
		}
	}

	public async void BTNTolid_Click(object sender, EventArgs e)
	{
	}

	public async void BTNApplication_Click(object sender, EventArgs e)
	{
	}

	public async void BTNBarcode_Click(object sender, EventArgs e)
	{
	}

	public async void BTNTarazo_Click(object sender, EventArgs e)
	{
	}

	public async void BTNCallerId_Click(object sender, EventArgs e)
	{
	}

	private void FRMPackages_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private async void btnTax_Click(object sender, EventArgs e)
	{
	}

	private async void btnAghsat_Click(object sender, EventArgs e)
	{
	}

	private async void btbChequeKhazane_Click(object sender, EventArgs e)
	{
	}

	private async void btnCustomerClub_Click(object sender, EventArgs e)
	{
	}

	private async void btnProductSerial_Click(object sender, EventArgs e)
	{
	}

	private async void btnProductDepotPishrafteh_Click(object sender, EventArgs e)
	{
	}

	private async void btnMultCurrency_Click(object sender, EventArgs e)
	{
	}

	private async void btnVoiceCall_Click(object sender, EventArgs e)
	{
	}

	private async void btnService_Click(object sender, EventArgs e)
	{
	}

	[DebuggerStepThrough]
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
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Expected O, but got Unknown
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Expected O, but got Unknown
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Expected O, but got Unknown
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Expected O, but got Unknown
		//IL_0eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec0: Expected O, but got Unknown
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f79: Expected O, but got Unknown
		//IL_0ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1008: Expected O, but got Unknown
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Expected O, but got Unknown
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_1150: Expected O, but got Unknown
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Expected O, but got Unknown
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1298: Expected O, but got Unknown
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		//IL_1351: Expected O, but got Unknown
		//IL_13d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e0: Expected O, but got Unknown
		//IL_148f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1499: Expected O, but got Unknown
		//IL_151e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Expected O, but got Unknown
		//IL_15d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e1: Expected O, but got Unknown
		//IL_1666: Unknown result type (might be due to invalid IL or missing references)
		//IL_1670: Expected O, but got Unknown
		//IL_171f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1729: Expected O, but got Unknown
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_182b: Expected O, but got Unknown
		//IL_1833: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Expected O, but got Unknown
		LBLTolid = new Label();
		BTNTolid = new Button();
		BTNApplication = new Button();
		LBLSoftware = new Label();
		BTNCancel = new Button();
		BTNCallerId = new Button();
		Label3 = new Label();
		BTNTarazo = new Button();
		Label2 = new Label();
		BTNBarcode = new Button();
		Label1 = new Label();
		panelEx1 = new PanelEx();
		label13 = new Label();
		btnService = new Button();
		btnVoiceCall = new Button();
		label12 = new Label();
		btnMultCurrency = new Button();
		label11 = new Label();
		btnProductDepotPishrafteh = new Button();
		label10 = new Label();
		btnProductSerial = new Button();
		label9 = new Label();
		btnCustomerClub = new Button();
		label8 = new Label();
		btbChequeKhazane = new Button();
		label7 = new Label();
		btnAghsat = new Button();
		label6 = new Label();
		btnTax = new Button();
		label5 = new Label();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)LBLTolid).Anchor = (AnchorStyles)9;
		((Control)LBLTolid).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLTolid).ForeColor = Color.Blue;
		((Control)LBLTolid).Location = new Point(147, 63);
		((Control)LBLTolid).Name = "LBLTolid";
		((Control)LBLTolid).Size = new Size(303, 31);
		((Control)LBLTolid).TabIndex = 0;
		((Control)LBLTolid).Text = "[سیستــــــــــــــــم تولیــــــــــد]";
		((Control)BTNTolid).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNTolid).ForeColor = Color.Red;
		((Control)BTNTolid).Location = new Point(19, 63);
		((Control)BTNTolid).Name = "BTNTolid";
		((Control)BTNTolid).Size = new Size(122, 31);
		((Control)BTNTolid).TabIndex = 1;
		((Control)BTNTolid).Text = "Activate";
		((ButtonBase)BTNTolid).UseVisualStyleBackColor = true;
		((Control)BTNTolid).Click += BTNTolid_Click;
		((Control)BTNApplication).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNApplication).ForeColor = Color.Red;
		((Control)BTNApplication).Location = new Point(19, 22);
		((Control)BTNApplication).Name = "BTNApplication";
		((Control)BTNApplication).Size = new Size(122, 31);
		((Control)BTNApplication).TabIndex = 3;
		((Control)BTNApplication).Text = "Activate";
		((ButtonBase)BTNApplication).UseVisualStyleBackColor = true;
		((Control)BTNApplication).Click += BTNApplication_Click;
		((Control)LBLSoftware).Anchor = (AnchorStyles)9;
		((Control)LBLSoftware).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLSoftware).ForeColor = Color.Blue;
		((Control)LBLSoftware).Location = new Point(147, 22);
		((Control)LBLSoftware).Name = "LBLSoftware";
		((Control)LBLSoftware).Size = new Size(303, 31);
		((Control)LBLSoftware).TabIndex = 2;
		((Control)LBLSoftware).Text = "[سیستـــــــم حســـــــــابـــــــداری]";
		((Control)BTNCancel).Dock = (DockStyle)2;
		((ButtonBase)BTNCancel).FlatStyle = (FlatStyle)1;
		((Control)BTNCancel).Location = new Point(0, 602);
		((Control)BTNCancel).Name = "BTNCancel";
		((Control)BTNCancel).Size = new Size(467, 38);
		((Control)BTNCancel).TabIndex = 4;
		((Control)BTNCancel).Text = "[خروج]";
		((ButtonBase)BTNCancel).UseVisualStyleBackColor = true;
		((Control)BTNCancel).Click += Button1_Click;
		((Control)BTNCallerId).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNCallerId).ForeColor = Color.Red;
		((Control)BTNCallerId).Location = new Point(19, 189);
		((Control)BTNCallerId).Name = "BTNCallerId";
		((Control)BTNCallerId).Size = new Size(122, 31);
		((Control)BTNCallerId).TabIndex = 7;
		((Control)BTNCallerId).Text = "Activate";
		((ButtonBase)BTNCallerId).UseVisualStyleBackColor = true;
		((Control)BTNCallerId).Click += BTNCallerId_Click;
		((Control)Label3).Anchor = (AnchorStyles)9;
		((Control)Label3).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).ForeColor = Color.Blue;
		((Control)Label3).Location = new Point(147, 189);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(303, 31);
		((Control)Label3).TabIndex = 6;
		((Control)Label3).Text = "[ارتبــــاط بـا مــــــودم کـــــالــرآی دی]";
		((Control)BTNTarazo).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNTarazo).ForeColor = Color.Red;
		((Control)BTNTarazo).Location = new Point(19, 149);
		((Control)BTNTarazo).Name = "BTNTarazo";
		((Control)BTNTarazo).Size = new Size(122, 31);
		((Control)BTNTarazo).TabIndex = 5;
		((Control)BTNTarazo).Text = "Activate";
		((ButtonBase)BTNTarazo).UseVisualStyleBackColor = true;
		((Control)BTNTarazo).Click += BTNTarazo_Click;
		((Control)Label2).Anchor = (AnchorStyles)9;
		((Control)Label2).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label2).ForeColor = Color.Blue;
		((Control)Label2).Location = new Point(147, 149);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(303, 31);
		((Control)Label2).TabIndex = 4;
		((Control)Label2).Text = "[ارتبـــــــاط بـــا تـــــرازوی دیجیتــــال]";
		((Control)BTNBarcode).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNBarcode).ForeColor = Color.Red;
		((Control)BTNBarcode).Location = new Point(19, 106);
		((Control)BTNBarcode).Name = "BTNBarcode";
		((Control)BTNBarcode).Size = new Size(122, 31);
		((Control)BTNBarcode).TabIndex = 3;
		((Control)BTNBarcode).Text = "Activate";
		((ButtonBase)BTNBarcode).UseVisualStyleBackColor = true;
		((Control)BTNBarcode).Click += BTNBarcode_Click;
		((Control)Label1).Anchor = (AnchorStyles)9;
		((Control)Label1).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = Color.Blue;
		((Control)Label1).Location = new Point(147, 106);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(303, 31);
		((Control)Label1).TabIndex = 2;
		((Control)Label1).Text = "[سیستــــــــــــم بـــــــارکـــــــد]";
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)panelEx1).Controls.Add((Control)(object)label13);
		((Control)panelEx1).Controls.Add((Control)(object)btnService);
		((Control)panelEx1).Controls.Add((Control)(object)btnVoiceCall);
		((Control)panelEx1).Controls.Add((Control)(object)label12);
		((Control)panelEx1).Controls.Add((Control)(object)btnMultCurrency);
		((Control)panelEx1).Controls.Add((Control)(object)label11);
		((Control)panelEx1).Controls.Add((Control)(object)btnProductDepotPishrafteh);
		((Control)panelEx1).Controls.Add((Control)(object)label10);
		((Control)panelEx1).Controls.Add((Control)(object)btnProductSerial);
		((Control)panelEx1).Controls.Add((Control)(object)label9);
		((Control)panelEx1).Controls.Add((Control)(object)btnCustomerClub);
		((Control)panelEx1).Controls.Add((Control)(object)label8);
		((Control)panelEx1).Controls.Add((Control)(object)btbChequeKhazane);
		((Control)panelEx1).Controls.Add((Control)(object)label7);
		((Control)panelEx1).Controls.Add((Control)(object)btnAghsat);
		((Control)panelEx1).Controls.Add((Control)(object)label6);
		((Control)panelEx1).Controls.Add((Control)(object)btnTax);
		((Control)panelEx1).Controls.Add((Control)(object)label5);
		((Control)panelEx1).Controls.Add((Control)(object)BTNCancel);
		((Control)panelEx1).Controls.Add((Control)(object)LBLSoftware);
		((Control)panelEx1).Controls.Add((Control)(object)BTNCallerId);
		((Control)panelEx1).Controls.Add((Control)(object)LBLTolid);
		((Control)panelEx1).Controls.Add((Control)(object)Label3);
		((Control)panelEx1).Controls.Add((Control)(object)Label1);
		((Control)panelEx1).Controls.Add((Control)(object)BTNTarazo);
		((Control)panelEx1).Controls.Add((Control)(object)BTNApplication);
		((Control)panelEx1).Controls.Add((Control)(object)Label2);
		((Control)panelEx1).Controls.Add((Control)(object)BTNBarcode);
		((Control)panelEx1).Controls.Add((Control)(object)BTNTolid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)1;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(467, 640);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		panelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		panelEx1.Style.Border = eBorderType.SingleLine;
		panelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		panelEx1.Style.BorderWidth = 5;
		panelEx1.Style.CornerDiameter = 10;
		panelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 43;
		((Control)label13).Anchor = (AnchorStyles)9;
		((Control)label13).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label13).ForeColor = Color.Blue;
		((Control)label13).Location = new Point(148, 524);
		((Control)label13).Name = "label13";
		((Control)label13).Size = new Size(303, 31);
		((Control)label13).TabIndex = 27;
		((Control)label13).Text = "[سیستـــــــم سرویس و خدمات]";
		((Control)btnService).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnService).ForeColor = Color.Red;
		((Control)btnService).Location = new Point(19, 524);
		((Control)btnService).Name = "btnService";
		((Control)btnService).Size = new Size(122, 31);
		((Control)btnService).TabIndex = 26;
		((Control)btnService).Text = "Activate";
		((ButtonBase)btnService).UseVisualStyleBackColor = true;
		((Control)btnService).Click += btnService_Click;
		((Control)btnVoiceCall).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnVoiceCall).ForeColor = Color.Red;
		((Control)btnVoiceCall).Location = new Point(19, 488);
		((Control)btnVoiceCall).Name = "btnVoiceCall";
		((Control)btnVoiceCall).Size = new Size(122, 31);
		((Control)btnVoiceCall).TabIndex = 25;
		((Control)btnVoiceCall).Text = "Activate";
		((ButtonBase)btnVoiceCall).UseVisualStyleBackColor = true;
		((Control)btnVoiceCall).Click += btnVoiceCall_Click;
		((Control)label12).Anchor = (AnchorStyles)9;
		((Control)label12).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label12).ForeColor = Color.Blue;
		((Control)label12).Location = new Point(152, 488);
		((Control)label12).Name = "label12";
		((Control)label12).Size = new Size(303, 31);
		((Control)label12).TabIndex = 24;
		((Control)label12).Text = "[سیستـــــــم فراخوان صوتی]";
		((Control)btnMultCurrency).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnMultCurrency).ForeColor = Color.Red;
		((Control)btnMultCurrency).Location = new Point(19, 451);
		((Control)btnMultCurrency).Name = "btnMultCurrency";
		((Control)btnMultCurrency).Size = new Size(122, 31);
		((Control)btnMultCurrency).TabIndex = 23;
		((Control)btnMultCurrency).Text = "Activate";
		((ButtonBase)btnMultCurrency).UseVisualStyleBackColor = true;
		((Control)btnMultCurrency).Click += btnMultCurrency_Click;
		((Control)label11).Anchor = (AnchorStyles)9;
		((Control)label11).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label11).ForeColor = Color.Blue;
		((Control)label11).Location = new Point(147, 455);
		((Control)label11).Name = "label11";
		((Control)label11).Size = new Size(303, 31);
		((Control)label11).TabIndex = 22;
		((Control)label11).Text = "[سیستـــــــم چند ارزی]";
		((Control)btnProductDepotPishrafteh).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnProductDepotPishrafteh).ForeColor = Color.Red;
		((Control)btnProductDepotPishrafteh).Location = new Point(19, 414);
		((Control)btnProductDepotPishrafteh).Name = "btnProductDepotPishrafteh";
		((Control)btnProductDepotPishrafteh).Size = new Size(122, 31);
		((Control)btnProductDepotPishrafteh).TabIndex = 21;
		((Control)btnProductDepotPishrafteh).Text = "Activate";
		((ButtonBase)btnProductDepotPishrafteh).UseVisualStyleBackColor = true;
		((Control)btnProductDepotPishrafteh).Click += btnProductDepotPishrafteh_Click;
		((Control)label10).Anchor = (AnchorStyles)9;
		((Control)label10).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label10).ForeColor = Color.Blue;
		((Control)label10).Location = new Point(147, 418);
		((Control)label10).Name = "label10";
		((Control)label10).Size = new Size(303, 31);
		((Control)label10).TabIndex = 20;
		((Control)label10).Text = "[سیستـــــــم کالا و انبار پیشرفته]";
		((Control)btnProductSerial).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnProductSerial).ForeColor = Color.Red;
		((Control)btnProductSerial).Location = new Point(19, 377);
		((Control)btnProductSerial).Name = "btnProductSerial";
		((Control)btnProductSerial).Size = new Size(122, 31);
		((Control)btnProductSerial).TabIndex = 19;
		((Control)btnProductSerial).Text = "Activate";
		((ButtonBase)btnProductSerial).UseVisualStyleBackColor = true;
		((Control)btnProductSerial).Click += btnProductSerial_Click;
		((Control)label9).Anchor = (AnchorStyles)9;
		((Control)label9).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label9).ForeColor = Color.Blue;
		((Control)label9).Location = new Point(147, 381);
		((Control)label9).Name = "label9";
		((Control)label9).Size = new Size(303, 31);
		((Control)label9).TabIndex = 18;
		((Control)label9).Text = "[سیستـــــــم کالای سریال دار]";
		((Control)btnCustomerClub).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnCustomerClub).ForeColor = Color.Red;
		((Control)btnCustomerClub).Location = new Point(19, 340);
		((Control)btnCustomerClub).Name = "btnCustomerClub";
		((Control)btnCustomerClub).Size = new Size(122, 31);
		((Control)btnCustomerClub).TabIndex = 17;
		((Control)btnCustomerClub).Text = "Activate";
		((ButtonBase)btnCustomerClub).UseVisualStyleBackColor = true;
		((Control)btnCustomerClub).Click += btnCustomerClub_Click;
		((Control)label8).Anchor = (AnchorStyles)9;
		((Control)label8).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label8).ForeColor = Color.Blue;
		((Control)label8).Location = new Point(147, 344);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(303, 31);
		((Control)label8).TabIndex = 16;
		((Control)label8).Text = "[سیستـــــــم باشگاه مشتریان]";
		((Control)btbChequeKhazane).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btbChequeKhazane).ForeColor = Color.Red;
		((Control)btbChequeKhazane).Location = new Point(19, 303);
		((Control)btbChequeKhazane).Name = "btbChequeKhazane";
		((Control)btbChequeKhazane).Size = new Size(122, 31);
		((Control)btbChequeKhazane).TabIndex = 15;
		((Control)btbChequeKhazane).Text = "Activate";
		((ButtonBase)btbChequeKhazane).UseVisualStyleBackColor = true;
		((Control)btbChequeKhazane).Click += btbChequeKhazane_Click;
		((Control)label7).Anchor = (AnchorStyles)9;
		((Control)label7).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label7).ForeColor = Color.Blue;
		((Control)label7).Location = new Point(147, 307);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(303, 31);
		((Control)label7).TabIndex = 14;
		((Control)label7).Text = "[سیستـــــــم چک و خزانه]";
		((Control)btnAghsat).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnAghsat).ForeColor = Color.Red;
		((Control)btnAghsat).Location = new Point(19, 266);
		((Control)btnAghsat).Name = "btnAghsat";
		((Control)btnAghsat).Size = new Size(122, 31);
		((Control)btnAghsat).TabIndex = 13;
		((Control)btnAghsat).Text = "Activate";
		((ButtonBase)btnAghsat).UseVisualStyleBackColor = true;
		((Control)btnAghsat).Click += btnAghsat_Click;
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label6).ForeColor = Color.Blue;
		((Control)label6).Location = new Point(147, 270);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(303, 31);
		((Control)label6).TabIndex = 12;
		((Control)label6).Text = "[سیستم وام و قسط]";
		((Control)btnTax).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)btnTax).ForeColor = Color.Red;
		((Control)btnTax).Location = new Point(19, 229);
		((Control)btnTax).Name = "btnTax";
		((Control)btnTax).Size = new Size(122, 31);
		((Control)btnTax).TabIndex = 11;
		((Control)btnTax).Text = "Activate";
		((ButtonBase)btnTax).UseVisualStyleBackColor = true;
		((Control)btnTax).Click += btnTax_Click;
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label5).ForeColor = Color.Blue;
		((Control)label5).Location = new Point(147, 233);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(303, 31);
		((Control)label5).TabIndex = 10;
		((Control)label5).Text = "[سیستم سامانه مودیان]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(467, 639);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMPackages";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فعال سازی بسته ها]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMPackages_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMPackages_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
