using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser.Entities;
using PacketParser.LkService;

namespace FormCustom.LockService.Forms;

public class frmActivationPackages : AccountingBaseForm
{
	private bool _isDisposed;

	private Container components;

	internal Button Btn_Ok;

	internal Label Label1;

	internal Label Label2;

	internal Button Btn_ecp;

	internal Panel Panel1;

	internal Label Label3;

	internal PictureBox Pbox_tolid;

	internal PictureBox Pbox_barcode;

	internal Label Label4;

	internal PictureBox Pbox_Special;

	internal Label Label5;

	private TextBox Txt_Tolid;

	private TextBox TxT_Barcode;

	private TextBox TXT_Special;

	internal PictureBox pboxProductSerial;

	private TextBox txtProductSerial;

	internal Label label13;

	internal PictureBox pboxVoiceCall;

	private TextBox txtVoiceCall;

	internal Label label12;

	internal PictureBox pboxService;

	private TextBox txtService;

	internal Label label11;

	internal PictureBox pboxProductDepot_Pishrafteh;

	private TextBox txtProductDepot_Pishrafteh;

	internal Label label10;

	internal PictureBox pboxCustomerClub;

	private TextBox txtCustomerClub;

	internal Label label9;

	internal PictureBox pboxCheque_Khazane;

	private TextBox txtCheque_Khazane;

	internal Label label8;

	internal PictureBox pboxAghsat;

	private TextBox txtAghsat;

	internal Label label7;

	internal PictureBox pboxTax;

	private TextBox txtTax;

	internal Label label6;

	internal PictureBox pboxMultiCurrency;

	private TextBox txtMultiCurrency;

	internal Label label14;

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

	public List<PackageContainer> Packages { get; private set; }

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 26);
		}
	}

	public frmActivationPackages(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	public void Btn_ecp_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void FRM_Activation_Barcod_Tolid_Special_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void Btn_Ok_Click(object sender, EventArgs e)
	{
		try
		{
			Packages = new List<PackageContainer>();
			if (!string.IsNullOrEmpty(((Control)TxT_Barcode).Text) && ((Control)TxT_Barcode).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Barcode, isActive: true, ((Control)TxT_Barcode).Text));
			}
			if (!string.IsNullOrEmpty(((Control)Txt_Tolid).Text) && ((Control)Txt_Tolid).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Tolid, isActive: true, ((Control)Txt_Tolid).Text));
			}
			if (!string.IsNullOrEmpty(((Control)TXT_Special).Text) && ((Control)TXT_Special).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.AdvancedFinance, isActive: true, ((Control)TXT_Special).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtTax).Text) && ((Control)txtTax).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Tax, isActive: true, ((Control)txtTax).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtAghsat).Text) && ((Control)txtAghsat).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Aghsat, isActive: true, ((Control)txtAghsat).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtCheque_Khazane).Text) && ((Control)txtCheque_Khazane).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Cheque, isActive: true, ((Control)txtCheque_Khazane).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtCustomerClub).Text) && ((Control)txtCustomerClub).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.CustomerClub, isActive: true, ((Control)txtCustomerClub).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtProductDepot_Pishrafteh).Text) && ((Control)txtProductDepot_Pishrafteh).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Depot, isActive: true, ((Control)txtProductDepot_Pishrafteh).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtService).Text) && ((Control)txtService).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.Service, isActive: true, ((Control)txtService).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtVoiceCall).Text) && ((Control)txtVoiceCall).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.VoiceCall, isActive: true, ((Control)txtVoiceCall).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtProductSerial).Text) && ((Control)txtProductSerial).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.ProductSerial, isActive: true, ((Control)txtProductSerial).Text));
			}
			if (!string.IsNullOrEmpty(((Control)txtMultiCurrency).Text) && ((Control)txtMultiCurrency).Text.Length == 14)
			{
				Packages.Add(new PackageContainer(EnLK_SpecialFacilities.MultiCurrency, isActive: true, ((Control)txtMultiCurrency).Text));
			}
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_Ok_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 66);
		}
	}

	public void TxT_Barcode_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_barcode).Visible = true;
			if (((Control)TxT_Barcode).Text.Length != 14)
			{
				Pbox_barcode.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_barcode.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxT_Barcode_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 78);
		}
	}

	public void Txt_Tolid_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_tolid).Visible = true;
			if (((Control)Txt_Tolid).Text.Length != 14)
			{
				Pbox_tolid.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_tolid.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Txt_Tolid_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 90);
		}
	}

	public void TXT_Special_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_Special).Visible = true;
			if (((Control)TXT_Special).Text.Length != 14)
			{
				Pbox_Special.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_Special.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXT_Special_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 102);
		}
	}

	private void txtTax_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxTax).Visible = true;
			if (((Control)txtTax).Text.Length != 14)
			{
				pboxTax.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxTax.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtTax_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 114);
		}
	}

	private void txtAghsat_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxAghsat).Visible = true;
			if (((Control)txtAghsat).Text.Length != 14)
			{
				pboxAghsat.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxAghsat.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtAghsat_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 126);
		}
	}

	private void txtCheque_Khazane_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxCheque_Khazane).Visible = true;
			if (((Control)txtCheque_Khazane).Text.Length != 14)
			{
				pboxCheque_Khazane.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxCheque_Khazane.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtCheque_Khazane_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 138);
		}
	}

	private void txtCustomerClub_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxCustomerClub).Visible = true;
			if (((Control)txtCustomerClub).Text.Length != 14)
			{
				pboxCustomerClub.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxCustomerClub.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtCustomerClub_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 150);
		}
	}

	private void txtProductDepot_Pishrafteh_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxProductDepot_Pishrafteh).Visible = true;
			if (((Control)txtProductDepot_Pishrafteh).Text.Length != 14)
			{
				pboxProductDepot_Pishrafteh.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxProductDepot_Pishrafteh.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtProductDepot_Pishrafteh_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 162);
		}
	}

	private void txtService_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxService).Visible = true;
			if (((Control)txtService).Text.Length != 14)
			{
				pboxService.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxService.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtService_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 174);
		}
	}

	private void txtVoiceCall_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxVoiceCall).Visible = true;
			if (((Control)txtVoiceCall).Text.Length != 14)
			{
				pboxVoiceCall.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxVoiceCall.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtVoiceCall_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 186);
		}
	}

	private void txtProductSerial_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxProductSerial).Visible = true;
			if (((Control)txtProductSerial).Text.Length != 14)
			{
				pboxProductSerial.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxProductSerial.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtProductSerial_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 198);
		}
	}

	private void txtMultiCurrency_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)pboxMultiCurrency).Visible = true;
			if (((Control)txtMultiCurrency).Text.Length != 14)
			{
				pboxMultiCurrency.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				pboxMultiCurrency.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMultiCurrency_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationPackages.cs", 210);
		}
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
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Expected O, but got Unknown
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Expected O, but got Unknown
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Expected O, but got Unknown
		//IL_0c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Expected O, but got Unknown
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf4: Expected O, but got Unknown
		//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8f: Expected O, but got Unknown
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Expected O, but got Unknown
		//IL_1077: Unknown result type (might be due to invalid IL or missing references)
		//IL_1081: Expected O, but got Unknown
		//IL_1155: Unknown result type (might be due to invalid IL or missing references)
		//IL_115f: Expected O, but got Unknown
		//IL_11f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fa: Expected O, but got Unknown
		//IL_12ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d8: Expected O, but got Unknown
		//IL_1369: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Expected O, but got Unknown
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Expected O, but got Unknown
		//IL_14e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ec: Expected O, but got Unknown
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c7: Expected O, but got Unknown
		//IL_1654: Unknown result type (might be due to invalid IL or missing references)
		//IL_165e: Expected O, but got Unknown
		//IL_172b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1735: Expected O, but got Unknown
		//IL_17c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cc: Expected O, but got Unknown
		//IL_1900: Unknown result type (might be due to invalid IL or missing references)
		//IL_190a: Expected O, but got Unknown
		//IL_19a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ab: Expected O, but got Unknown
		//IL_1af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afe: Expected O, but got Unknown
		Btn_Ok = new Button();
		Label1 = new Label();
		Label2 = new Label();
		Txt_Tolid = new TextBox();
		TxT_Barcode = new TextBox();
		Btn_ecp = new Button();
		Panel1 = new Panel();
		pboxMultiCurrency = new PictureBox();
		txtMultiCurrency = new TextBox();
		label14 = new Label();
		pboxProductSerial = new PictureBox();
		txtProductSerial = new TextBox();
		label13 = new Label();
		pboxVoiceCall = new PictureBox();
		txtVoiceCall = new TextBox();
		label12 = new Label();
		pboxService = new PictureBox();
		txtService = new TextBox();
		label11 = new Label();
		pboxProductDepot_Pishrafteh = new PictureBox();
		txtProductDepot_Pishrafteh = new TextBox();
		label10 = new Label();
		pboxCustomerClub = new PictureBox();
		txtCustomerClub = new TextBox();
		label9 = new Label();
		pboxCheque_Khazane = new PictureBox();
		txtCheque_Khazane = new TextBox();
		label8 = new Label();
		pboxAghsat = new PictureBox();
		txtAghsat = new TextBox();
		label7 = new Label();
		pboxTax = new PictureBox();
		txtTax = new TextBox();
		label6 = new Label();
		Pbox_Special = new PictureBox();
		TXT_Special = new TextBox();
		Label5 = new Label();
		Pbox_tolid = new PictureBox();
		Pbox_barcode = new PictureBox();
		Label3 = new Label();
		Label4 = new Label();
		((Control)Panel1).SuspendLayout();
		((ISupportInitialize)pboxMultiCurrency).BeginInit();
		((ISupportInitialize)pboxProductSerial).BeginInit();
		((ISupportInitialize)pboxVoiceCall).BeginInit();
		((ISupportInitialize)pboxService).BeginInit();
		((ISupportInitialize)pboxProductDepot_Pishrafteh).BeginInit();
		((ISupportInitialize)pboxCustomerClub).BeginInit();
		((ISupportInitialize)pboxCheque_Khazane).BeginInit();
		((ISupportInitialize)pboxAghsat).BeginInit();
		((ISupportInitialize)pboxTax).BeginInit();
		((ISupportInitialize)Pbox_Special).BeginInit();
		((ISupportInitialize)Pbox_tolid).BeginInit();
		((ISupportInitialize)Pbox_barcode).BeginInit();
		((Control)this).SuspendLayout();
		((Control)Btn_Ok).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)Btn_Ok).Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((ButtonBase)Btn_Ok).ImageAlign = (ContentAlignment)64;
		((Control)Btn_Ok).Location = new Point(103, 528);
		((Control)Btn_Ok).Name = "Btn_Ok";
		((Control)Btn_Ok).Size = new Size(92, 40);
		((Control)Btn_Ok).TabIndex = 3;
		((Control)Btn_Ok).Text = "[تائید]";
		((ButtonBase)Btn_Ok).TextAlign = (ContentAlignment)16;
		((ButtonBase)Btn_Ok).UseVisualStyleBackColor = true;
		((Control)Btn_Ok).Click += Btn_Ok_Click;
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).Location = new Point(254, 55);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)1;
		((Control)Label1).Size = new Size(178, 13);
		((Control)Label1).TabIndex = 1;
		((Control)Label1).Text = "[کد فعال  سازی زیر بسته ی بارکد]:";
		((Control)Label2).AutoSize = true;
		((Control)Label2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label2).Location = new Point(254, 20);
		((Control)Label2).Name = "Label2";
		((Control)Label2).RightToLeft = (RightToLeft)1;
		((Control)Label2).Size = new Size(183, 13);
		((Control)Label2).TabIndex = 2;
		((Control)Label2).Text = "[کد فعال  سازی زیر بسته ی تولید] : ";
		((Control)Txt_Tolid).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Txt_Tolid).Location = new Point(56, 13);
		((Control)Txt_Tolid).Name = "Txt_Tolid";
		((Control)Txt_Tolid).RightToLeft = (RightToLeft)0;
		((Control)Txt_Tolid).Size = new Size(191, 26);
		((Control)Txt_Tolid).TabIndex = 2;
		((Control)Txt_Tolid).TextChanged += Txt_Tolid_TextChanged;
		((Control)TxT_Barcode).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)TxT_Barcode).Location = new Point(56, 48);
		((Control)TxT_Barcode).Name = "TxT_Barcode";
		((Control)TxT_Barcode).RightToLeft = (RightToLeft)0;
		((Control)TxT_Barcode).Size = new Size(191, 26);
		((Control)TxT_Barcode).TabIndex = 1;
		((Control)TxT_Barcode).TextChanged += TxT_Barcode_TextChanged;
		Btn_ecp.DialogResult = (DialogResult)2;
		((Control)Btn_ecp).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)Btn_ecp).Image = (Image)(object)FormCustom.Properties.Resources.stop;
		((ButtonBase)Btn_ecp).ImageAlign = (ContentAlignment)64;
		((Control)Btn_ecp).Location = new Point(8, 528);
		((Control)Btn_ecp).Name = "Btn_ecp";
		((Control)Btn_ecp).Size = new Size(92, 40);
		((Control)Btn_ecp).TabIndex = 4;
		((Control)Btn_ecp).Text = "[انصراف]";
		((ButtonBase)Btn_ecp).TextAlign = (ContentAlignment)16;
		((ButtonBase)Btn_ecp).UseVisualStyleBackColor = true;
		((Control)Btn_ecp).Click += Btn_ecp_Click;
		Panel1.BorderStyle = (BorderStyle)2;
		((Control)Panel1).Controls.Add((Control)(object)pboxMultiCurrency);
		((Control)Panel1).Controls.Add((Control)(object)txtMultiCurrency);
		((Control)Panel1).Controls.Add((Control)(object)label14);
		((Control)Panel1).Controls.Add((Control)(object)pboxProductSerial);
		((Control)Panel1).Controls.Add((Control)(object)txtProductSerial);
		((Control)Panel1).Controls.Add((Control)(object)label13);
		((Control)Panel1).Controls.Add((Control)(object)pboxVoiceCall);
		((Control)Panel1).Controls.Add((Control)(object)txtVoiceCall);
		((Control)Panel1).Controls.Add((Control)(object)label12);
		((Control)Panel1).Controls.Add((Control)(object)pboxService);
		((Control)Panel1).Controls.Add((Control)(object)txtService);
		((Control)Panel1).Controls.Add((Control)(object)label11);
		((Control)Panel1).Controls.Add((Control)(object)pboxProductDepot_Pishrafteh);
		((Control)Panel1).Controls.Add((Control)(object)txtProductDepot_Pishrafteh);
		((Control)Panel1).Controls.Add((Control)(object)label10);
		((Control)Panel1).Controls.Add((Control)(object)pboxCustomerClub);
		((Control)Panel1).Controls.Add((Control)(object)txtCustomerClub);
		((Control)Panel1).Controls.Add((Control)(object)label9);
		((Control)Panel1).Controls.Add((Control)(object)pboxCheque_Khazane);
		((Control)Panel1).Controls.Add((Control)(object)txtCheque_Khazane);
		((Control)Panel1).Controls.Add((Control)(object)label8);
		((Control)Panel1).Controls.Add((Control)(object)pboxAghsat);
		((Control)Panel1).Controls.Add((Control)(object)txtAghsat);
		((Control)Panel1).Controls.Add((Control)(object)label7);
		((Control)Panel1).Controls.Add((Control)(object)pboxTax);
		((Control)Panel1).Controls.Add((Control)(object)txtTax);
		((Control)Panel1).Controls.Add((Control)(object)label6);
		((Control)Panel1).Controls.Add((Control)(object)Pbox_Special);
		((Control)Panel1).Controls.Add((Control)(object)TXT_Special);
		((Control)Panel1).Controls.Add((Control)(object)Label5);
		((Control)Panel1).Controls.Add((Control)(object)Pbox_tolid);
		((Control)Panel1).Controls.Add((Control)(object)Pbox_barcode);
		((Control)Panel1).Controls.Add((Control)(object)TxT_Barcode);
		((Control)Panel1).Controls.Add((Control)(object)Txt_Tolid);
		((Control)Panel1).Controls.Add((Control)(object)Label2);
		((Control)Panel1).Controls.Add((Control)(object)Label1);
		((Control)Panel1).Location = new Point(8, 62);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(556, 450);
		((Control)Panel1).TabIndex = 6;
		((Control)pboxMultiCurrency).Location = new Point(8, 325);
		((Control)pboxMultiCurrency).Name = "pboxMultiCurrency";
		((Control)pboxMultiCurrency).Size = new Size(43, 32);
		pboxMultiCurrency.TabIndex = 34;
		pboxMultiCurrency.TabStop = false;
		((Control)txtMultiCurrency).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtMultiCurrency).Location = new Point(56, 328);
		((Control)txtMultiCurrency).Name = "txtMultiCurrency";
		((Control)txtMultiCurrency).RightToLeft = (RightToLeft)0;
		((Control)txtMultiCurrency).Size = new Size(191, 26);
		((Control)txtMultiCurrency).TabIndex = 32;
		((Control)txtMultiCurrency).TextChanged += txtMultiCurrency_TextChanged;
		((Control)label14).AutoSize = true;
		((Control)label14).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label14).Location = new Point(254, 335);
		((Control)label14).Name = "label14";
		((Control)label14).RightToLeft = (RightToLeft)1;
		((Control)label14).Size = new Size(199, 13);
		((Control)label14).TabIndex = 33;
		((Control)label14).Text = "[کد فعال  سازی زیر بسته ی چند ارزی]:";
		((Control)pboxProductSerial).Location = new Point(8, 255);
		((Control)pboxProductSerial).Name = "pboxProductSerial";
		((Control)pboxProductSerial).Size = new Size(43, 32);
		pboxProductSerial.TabIndex = 31;
		pboxProductSerial.TabStop = false;
		((Control)txtProductSerial).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtProductSerial).Location = new Point(56, 258);
		((Control)txtProductSerial).Name = "txtProductSerial";
		((Control)txtProductSerial).RightToLeft = (RightToLeft)0;
		((Control)txtProductSerial).Size = new Size(191, 26);
		((Control)txtProductSerial).TabIndex = 29;
		((Control)txtProductSerial).TextChanged += txtProductSerial_TextChanged;
		((Control)label13).AutoSize = true;
		((Control)label13).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label13).Location = new Point(254, 265);
		((Control)label13).Name = "label13";
		((Control)label13).RightToLeft = (RightToLeft)1;
		((Control)label13).Size = new Size(240, 13);
		((Control)label13).TabIndex = 30;
		((Control)label13).Text = "[کد فعال  سازی زیر بسته ی کالای سریال دار]: ";
		((Control)pboxVoiceCall).Location = new Point(8, 360);
		((Control)pboxVoiceCall).Name = "pboxVoiceCall";
		((Control)pboxVoiceCall).Size = new Size(43, 32);
		pboxVoiceCall.TabIndex = 28;
		pboxVoiceCall.TabStop = false;
		((Control)txtVoiceCall).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtVoiceCall).Location = new Point(56, 363);
		((Control)txtVoiceCall).Name = "txtVoiceCall";
		((Control)txtVoiceCall).RightToLeft = (RightToLeft)0;
		((Control)txtVoiceCall).Size = new Size(191, 26);
		((Control)txtVoiceCall).TabIndex = 26;
		((Control)txtVoiceCall).TextChanged += txtVoiceCall_TextChanged;
		((Control)label12).AutoSize = true;
		((Control)label12).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label12).Location = new Point(254, 370);
		((Control)label12).Name = "label12";
		((Control)label12).RightToLeft = (RightToLeft)1;
		((Control)label12).Size = new Size(280, 13);
		((Control)label12).TabIndex = 27;
		((Control)label12).Text = "[کد فعال  سازی زیر بسته ی فراخوان فراخوان صوتی]: ";
		((Control)pboxService).Location = new Point(8, 395);
		((Control)pboxService).Name = "pboxService";
		((Control)pboxService).Size = new Size(43, 32);
		pboxService.TabIndex = 25;
		pboxService.TabStop = false;
		((Control)txtService).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtService).Location = new Point(56, 398);
		((Control)txtService).Name = "txtService";
		((Control)txtService).RightToLeft = (RightToLeft)0;
		((Control)txtService).Size = new Size(191, 26);
		((Control)txtService).TabIndex = 23;
		((Control)txtService).TextChanged += txtService_TextChanged;
		((Control)label11).AutoSize = true;
		((Control)label11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label11).Location = new Point(254, 405);
		((Control)label11).Name = "label11";
		((Control)label11).RightToLeft = (RightToLeft)1;
		((Control)label11).Size = new Size(247, 13);
		((Control)label11).TabIndex = 24;
		((Control)label11).Text = "[کد فعال  سازی زیر بسته ی سرویس و خدمات]: ";
		((Control)pboxProductDepot_Pishrafteh).Location = new Point(8, 290);
		((Control)pboxProductDepot_Pishrafteh).Name = "pboxProductDepot_Pishrafteh";
		((Control)pboxProductDepot_Pishrafteh).Size = new Size(43, 32);
		pboxProductDepot_Pishrafteh.TabIndex = 22;
		pboxProductDepot_Pishrafteh.TabStop = false;
		((Control)txtProductDepot_Pishrafteh).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtProductDepot_Pishrafteh).Location = new Point(56, 293);
		((Control)txtProductDepot_Pishrafteh).Name = "txtProductDepot_Pishrafteh";
		((Control)txtProductDepot_Pishrafteh).RightToLeft = (RightToLeft)0;
		((Control)txtProductDepot_Pishrafteh).Size = new Size(191, 26);
		((Control)txtProductDepot_Pishrafteh).TabIndex = 20;
		((Control)txtProductDepot_Pishrafteh).TextChanged += txtProductDepot_Pishrafteh_TextChanged;
		((Control)label10).AutoSize = true;
		((Control)label10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label10).Location = new Point(254, 300);
		((Control)label10).Name = "label10";
		((Control)label10).RightToLeft = (RightToLeft)1;
		((Control)label10).Size = new Size(254, 13);
		((Control)label10).TabIndex = 21;
		((Control)label10).Text = "[کد فعال  سازی زیر بسته ی کالا و انبار پیشرفته]: ";
		((Control)pboxCustomerClub).Location = new Point(8, 220);
		((Control)pboxCustomerClub).Name = "pboxCustomerClub";
		((Control)pboxCustomerClub).Size = new Size(43, 32);
		pboxCustomerClub.TabIndex = 19;
		pboxCustomerClub.TabStop = false;
		((Control)txtCustomerClub).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtCustomerClub).Location = new Point(56, 223);
		((Control)txtCustomerClub).Name = "txtCustomerClub";
		((Control)txtCustomerClub).RightToLeft = (RightToLeft)0;
		((Control)txtCustomerClub).Size = new Size(191, 26);
		((Control)txtCustomerClub).TabIndex = 17;
		((Control)txtCustomerClub).TextChanged += txtCustomerClub_TextChanged;
		((Control)label9).AutoSize = true;
		((Control)label9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label9).Location = new Point(254, 230);
		((Control)label9).Name = "label9";
		((Control)label9).RightToLeft = (RightToLeft)1;
		((Control)label9).Size = new Size(243, 13);
		((Control)label9).TabIndex = 18;
		((Control)label9).Text = "[کد فعال  سازی زیر بسته ی باشگاه مشتریان]: ";
		((Control)pboxCheque_Khazane).Location = new Point(8, 185);
		((Control)pboxCheque_Khazane).Name = "pboxCheque_Khazane";
		((Control)pboxCheque_Khazane).Size = new Size(43, 32);
		pboxCheque_Khazane.TabIndex = 16;
		pboxCheque_Khazane.TabStop = false;
		((Control)txtCheque_Khazane).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtCheque_Khazane).Location = new Point(56, 188);
		((Control)txtCheque_Khazane).Name = "txtCheque_Khazane";
		((Control)txtCheque_Khazane).RightToLeft = (RightToLeft)0;
		((Control)txtCheque_Khazane).Size = new Size(191, 26);
		((Control)txtCheque_Khazane).TabIndex = 14;
		((Control)txtCheque_Khazane).TextChanged += txtCheque_Khazane_TextChanged;
		((Control)label8).AutoSize = true;
		((Control)label8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label8).Location = new Point(254, 195);
		((Control)label8).Name = "label8";
		((Control)label8).RightToLeft = (RightToLeft)1;
		((Control)label8).Size = new Size(217, 13);
		((Control)label8).TabIndex = 15;
		((Control)label8).Text = "[کد فعال  سازی زیر بسته ی چک و خزانه]: ";
		((Control)pboxAghsat).Location = new Point(8, 150);
		((Control)pboxAghsat).Name = "pboxAghsat";
		((Control)pboxAghsat).Size = new Size(43, 32);
		pboxAghsat.TabIndex = 13;
		pboxAghsat.TabStop = false;
		((Control)txtAghsat).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtAghsat).Location = new Point(56, 153);
		((Control)txtAghsat).Name = "txtAghsat";
		((Control)txtAghsat).RightToLeft = (RightToLeft)0;
		((Control)txtAghsat).Size = new Size(191, 26);
		((Control)txtAghsat).TabIndex = 11;
		((Control)txtAghsat).TextChanged += txtAghsat_TextChanged;
		((Control)label7).AutoSize = true;
		((Control)label7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label7).Location = new Point(254, 160);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)1;
		((Control)label7).Size = new Size(216, 13);
		((Control)label7).TabIndex = 12;
		((Control)label7).Text = "[کد فعال  سازی زیر بسته ی وام و قسط] : ";
		((Control)pboxTax).Location = new Point(8, 115);
		((Control)pboxTax).Name = "pboxTax";
		((Control)pboxTax).Size = new Size(43, 32);
		pboxTax.TabIndex = 10;
		pboxTax.TabStop = false;
		((Control)txtTax).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtTax).Location = new Point(56, 118);
		((Control)txtTax).Name = "txtTax";
		((Control)txtTax).RightToLeft = (RightToLeft)0;
		((Control)txtTax).Size = new Size(191, 26);
		((Control)txtTax).TabIndex = 8;
		((Control)txtTax).TextChanged += txtTax_TextChanged;
		((Control)label6).AutoSize = true;
		((Control)label6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label6).Location = new Point(254, 125);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)1;
		((Control)label6).Size = new Size(236, 13);
		((Control)label6).TabIndex = 9;
		((Control)label6).Text = "کد فعال  سازی زیر بسته ی سامانه مودیان : ";
		((Control)Pbox_Special).Location = new Point(8, 80);
		((Control)Pbox_Special).Name = "Pbox_Special";
		((Control)Pbox_Special).Size = new Size(43, 32);
		Pbox_Special.TabIndex = 7;
		Pbox_Special.TabStop = false;
		((Control)TXT_Special).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)TXT_Special).Location = new Point(56, 83);
		((Control)TXT_Special).Name = "TXT_Special";
		((Control)TXT_Special).RightToLeft = (RightToLeft)0;
		((Control)TXT_Special).Size = new Size(191, 26);
		((Control)TXT_Special).TabIndex = 5;
		((Control)TXT_Special).TextChanged += TXT_Special_TextChanged;
		((Control)Label5).AutoSize = true;
		((Control)Label5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label5).Location = new Point(254, 90);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)1;
		((Control)Label5).Size = new Size(224, 13);
		((Control)Label5).TabIndex = 6;
		((Control)Label5).Text = "[کد فعال  سازی زیر بسته ی امکانات ویژه] : ";
		((Control)Pbox_tolid).Location = new Point(8, 10);
		((Control)Pbox_tolid).Name = "Pbox_tolid";
		((Control)Pbox_tolid).Size = new Size(43, 32);
		Pbox_tolid.TabIndex = 4;
		Pbox_tolid.TabStop = false;
		((Control)Pbox_barcode).Location = new Point(8, 45);
		((Control)Pbox_barcode).Name = "Pbox_barcode";
		((Control)Pbox_barcode).Size = new Size(43, 32);
		Pbox_barcode.TabIndex = 3;
		Pbox_barcode.TabStop = false;
		((Control)Label3).Anchor = (AnchorStyles)13;
		((Control)Label3).AutoSize = true;
		((Control)Label3).Font = new Font("B Titr", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).ForeColor = Color.Red;
		((Control)Label3).Location = new Point(80, 9);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(403, 26);
		((Control)Label3).TabIndex = 7;
		((Control)Label3).Text = "[در صورتی که کد فعال سازی زیر بسته های تولید، بارکد و امکانات ویژه را] ";
		((Control)Label4).Anchor = (AnchorStyles)13;
		((Control)Label4).AutoSize = true;
		((Control)Label4).Font = new Font("B Titr", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label4).ForeColor = Color.Red;
		((Control)Label4).Location = new Point(131, 33);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(318, 26);
		((Control)Label4).TabIndex = 8;
		((Control)Label4).Text = "[از طریق پیامک در یافت کرده اید در قسمت ذیل وارد کنید]";
		((Form)this).AcceptButton = (IButtonControl)(object)Btn_Ok;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)Btn_ecp;
		((Form)this).ClientSize = new Size(568, 583);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)Panel1);
		((Control)this).Controls.Add((Control)(object)Btn_ecp);
		((Control)this).Controls.Add((Control)(object)Btn_Ok);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmActivationPackages";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فعال سازی زیر بسته های تولید و بارکد امکانات ویژه]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRM_Activation_Barcod_Tolid_Special_FormClosed);
		((Control)Panel1).ResumeLayout(false);
		((Control)Panel1).PerformLayout();
		((ISupportInitialize)pboxMultiCurrency).EndInit();
		((ISupportInitialize)pboxProductSerial).EndInit();
		((ISupportInitialize)pboxVoiceCall).EndInit();
		((ISupportInitialize)pboxService).EndInit();
		((ISupportInitialize)pboxProductDepot_Pishrafteh).EndInit();
		((ISupportInitialize)pboxCustomerClub).EndInit();
		((ISupportInitialize)pboxCheque_Khazane).EndInit();
		((ISupportInitialize)pboxAghsat).EndInit();
		((ISupportInitialize)pboxTax).EndInit();
		((ISupportInitialize)Pbox_Special).EndInit();
		((ISupportInitialize)Pbox_tolid).EndInit();
		((ISupportInitialize)Pbox_barcode).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
