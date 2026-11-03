using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmScreenSaver : Form
{
	private AxSBXPC bpc;

	private const int SLEEP_MSG_LEN = 256;

	private const int COMPANY_NAME_LEN = 128;

	private const int CUSTOMER_NAME_LEN = 128;

	private IContainer components = null;

	public Label lblMessage;

	public Label lblEnrollData;

	public Label label1;

	public Label label2;

	public TextBox txtCompanyName;

	public TextBox txtCustomerName;

	public Button cmdGetCustomerInfo;

	public Button cmdSetCustomerInfo;

	private TextBox txtSleepMessage;

	public TextBox txtGlyphWidth;

	public Label label3;

	public TextBox txtGlyphHeight;

	public Label label4;

	public TextBox txtFontHeight;

	public Label label5;

	public TextBox txtFontWidth;

	public Label label6;

	public TextBox txtFontWeight;

	public Label label7;

	public Button cmdGetGlyphSize;

	private CheckBox chkItalic;

	private CheckBox chkUnderline;

	private CheckBox chkStrikeOut;

	public Button cmdSetSleepMsg;

	public Button cmdGetSleepMsg;

	public Button cmdExit;

	private CheckBox chkDebugOut;

	public TextBox txtDebugOutFile;

	public Button cmdDebugOutFileBrowse;

	private SaveFileDialog OpenSaveDlg;

	public frmScreenSaver(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void cmdGetCustomerInfo_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		string pData = "";
		string pData2 = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetCustomerInfo");
		if (!bpc.GeneralOperationXML(ref strXML))
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else
		{
			((Control)lblMessage).Text = "GetCustomerInfo OK";
			bpc.XML_ParseBinaryUnicode(ref strXML, "CompanyName", ref pData, 128);
			bpc.XML_ParseBinaryUnicode(ref strXML, "CustomerName", ref pData2, 128);
			((Control)txtCompanyName).Text = pData;
			((Control)txtCustomerName).Text = pData2;
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void frmScreenSaver_Load(object sender, EventArgs e)
	{
	}

	private void cmdSetCustomerInfo_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		string text = "";
		string text2 = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		text = ((Control)txtCompanyName).Text;
		text2 = ((Control)txtCustomerName).Text;
		util.MakeXMLRequestHeader(bpc, ref strXML, "SetCustomerInfo");
		bpc.XML_AddBinaryUnicode(ref strXML, "CompanyName", ref text);
		bpc.XML_AddBinaryUnicode(ref strXML, "CustomerName", ref text2);
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "SetCustomerInfo OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void frmScreenSaver_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void cmdGetGlyphSize_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMsgGlyphSize");
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "GetSleepMsgGlyphSize OK";
			int num = bpc.XML_ParseInt(ref strXML, "Width");
			int num2 = bpc.XML_ParseInt(ref strXML, "Height");
			((Control)txtGlyphWidth).Text = Convert.ToString(num);
			((Control)txtGlyphHeight).Text = Convert.ToString(num2);
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetSleepMsg_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMessage");
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "GetSleepMessage OK";
			string pData = "";
			bpc.XML_ParseBinaryUnicode(ref strXML, "SleepMessage", ref pData, 256);
			((Control)txtSleepMessage).Text = pData;
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetSleepMsg_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		string lpszXML = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMsgGlyphSize");
		if (!bpc.GeneralOperationXML(ref strXML))
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else
		{
			int width = bpc.XML_ParseInt(ref strXML, "Width");
			int height = bpc.XML_ParseInt(ref strXML, "Height");
			int nValue = Convert.ToInt32((((Control)txtFontHeight).Text == "") ? "0" : ((Control)txtFontHeight).Text);
			int nValue2 = Convert.ToInt32((((Control)txtFontWidth).Text == "") ? "0" : ((Control)txtFontWidth).Text);
			int nValue3 = Convert.ToInt32((((Control)txtFontWeight).Text == "") ? "0" : ((Control)txtFontWeight).Text);
			string text = ((Control)txtSleepMessage).Text;
			bpc.XML_AddString(ref lpszXML, "FaceName", "Arial");
			bpc.XML_AddInt(ref lpszXML, "Height", nValue);
			bpc.XML_AddInt(ref lpszXML, "Width", nValue2);
			bpc.XML_AddInt(ref lpszXML, "Weight", nValue3);
			bpc.XML_AddBoolean(ref lpszXML, "Italic", chkItalic.Checked);
			bpc.XML_AddBoolean(ref lpszXML, "Underline", chkUnderline.Checked);
			bpc.XML_AddBoolean(ref lpszXML, "StrikeOut", chkStrikeOut.Checked);
			if (chkDebugOut.Checked && ((Control)txtDebugOutFile).Text != "")
			{
				bpc.XML_AddString(ref lpszXML, "DebugOut", ((Control)txtDebugOutFile).Text);
			}
			strXML = "";
			util.MakeXMLRequestHeader(bpc, ref strXML, "SetSleepMessage");
			bpc.XML_AddBinaryGlyph(ref strXML, text, width, height, lpszXML);
			if (bpc.GeneralOperationXML(ref strXML))
			{
				((Control)lblMessage).Text = "SetSleepMessage OK";
			}
			else
			{
				bpc.GetLastError(ref dwErrorCode);
				((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			}
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdDebugOutFileBrowse_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((CommonDialog)OpenSaveDlg).ShowDialog();
		((Control)txtDebugOutFile).Text = ((FileDialog)OpenSaveDlg).FileName;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Expected O, but got Unknown
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Expected O, but got Unknown
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Expected O, but got Unknown
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Expected O, but got Unknown
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Expected O, but got Unknown
		//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Expected O, but got Unknown
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Expected O, but got Unknown
		//IL_0f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Expected O, but got Unknown
		//IL_0fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fed: Expected O, but got Unknown
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_1179: Unknown result type (might be due to invalid IL or missing references)
		//IL_1183: Expected O, but got Unknown
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1225: Expected O, but got Unknown
		//IL_12bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c7: Expected O, but got Unknown
		//IL_136a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1374: Expected O, but got Unknown
		//IL_1450: Unknown result type (might be due to invalid IL or missing references)
		//IL_145a: Expected O, but got Unknown
		//IL_1533: Unknown result type (might be due to invalid IL or missing references)
		//IL_153d: Expected O, but got Unknown
		//IL_160b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1615: Expected O, but got Unknown
		//IL_16c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cc: Expected O, but got Unknown
		//IL_1790: Unknown result type (might be due to invalid IL or missing references)
		//IL_179a: Expected O, but got Unknown
		//IL_1aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Expected O, but got Unknown
		lblMessage = new Label();
		lblEnrollData = new Label();
		label1 = new Label();
		label2 = new Label();
		txtCompanyName = new TextBox();
		txtCustomerName = new TextBox();
		cmdGetCustomerInfo = new Button();
		cmdSetCustomerInfo = new Button();
		txtSleepMessage = new TextBox();
		txtGlyphWidth = new TextBox();
		label3 = new Label();
		txtGlyphHeight = new TextBox();
		label4 = new Label();
		txtFontHeight = new TextBox();
		label5 = new Label();
		txtFontWidth = new TextBox();
		label6 = new Label();
		txtFontWeight = new TextBox();
		label7 = new Label();
		cmdGetGlyphSize = new Button();
		chkItalic = new CheckBox();
		chkUnderline = new CheckBox();
		chkStrikeOut = new CheckBox();
		cmdSetSleepMsg = new Button();
		cmdGetSleepMsg = new Button();
		cmdExit = new Button();
		chkDebugOut = new CheckBox();
		txtDebugOutFile = new TextBox();
		cmdDebugOutFileBrowse = new Button();
		OpenSaveDlg = new SaveFileDialog();
		((Control)this).SuspendLayout();
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(574, 28);
		((Control)lblMessage).TabIndex = 28;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(48, 67);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(112, 19);
		((Control)lblEnrollData).TabIndex = 36;
		((Control)lblEnrollData).Text = "Company Name:";
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(26, 209);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)0;
		((Control)label1).Size = new Size(104, 19);
		((Control)label1).TabIndex = 37;
		((Control)label1).Text = "Sleep Message:";
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = SystemColors.Control;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(48, 109);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(112, 19);
		((Control)label2).TabIndex = 38;
		((Control)label2).Text = "Customer Name:";
		txtCompanyName.AcceptsReturn = true;
		((Control)txtCompanyName).BackColor = SystemColors.Window;
		((Control)txtCompanyName).Cursor = Cursors.IBeam;
		((Control)txtCompanyName).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtCompanyName).ForeColor = SystemColors.WindowText;
		((Control)txtCompanyName).Location = new Point(197, 66);
		((TextBoxBase)txtCompanyName).MaxLength = 64;
		((Control)txtCompanyName).Name = "txtCompanyName";
		((Control)txtCompanyName).RightToLeft = (RightToLeft)0;
		((Control)txtCompanyName).Size = new Size(373, 26);
		((Control)txtCompanyName).TabIndex = 39;
		txtCustomerName.AcceptsReturn = true;
		((Control)txtCustomerName).BackColor = SystemColors.Window;
		((Control)txtCustomerName).Cursor = Cursors.IBeam;
		((Control)txtCustomerName).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtCustomerName).ForeColor = SystemColors.WindowText;
		((Control)txtCustomerName).Location = new Point(197, 105);
		((TextBoxBase)txtCustomerName).MaxLength = 64;
		((Control)txtCustomerName).Name = "txtCustomerName";
		((Control)txtCustomerName).RightToLeft = (RightToLeft)0;
		((Control)txtCustomerName).Size = new Size(373, 26);
		((Control)txtCustomerName).TabIndex = 40;
		((Control)cmdGetCustomerInfo).BackColor = SystemColors.Control;
		((Control)cmdGetCustomerInfo).Cursor = Cursors.Default;
		((Control)cmdGetCustomerInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetCustomerInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetCustomerInfo).Location = new Point(92, 157);
		((Control)cmdGetCustomerInfo).Name = "cmdGetCustomerInfo";
		((Control)cmdGetCustomerInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetCustomerInfo).Size = new Size(200, 30);
		((Control)cmdGetCustomerInfo).TabIndex = 55;
		((Control)cmdGetCustomerInfo).Text = "Get Customer Info";
		((ButtonBase)cmdGetCustomerInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetCustomerInfo).Click += cmdGetCustomerInfo_Click;
		((Control)cmdSetCustomerInfo).BackColor = SystemColors.Control;
		((Control)cmdSetCustomerInfo).Cursor = Cursors.Default;
		((Control)cmdSetCustomerInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetCustomerInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetCustomerInfo).Location = new Point(336, 157);
		((Control)cmdSetCustomerInfo).Name = "cmdSetCustomerInfo";
		((Control)cmdSetCustomerInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetCustomerInfo).Size = new Size(200, 30);
		((Control)cmdSetCustomerInfo).TabIndex = 56;
		((Control)cmdSetCustomerInfo).Text = "Set Customer Info";
		((ButtonBase)cmdSetCustomerInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetCustomerInfo).Click += cmdSetCustomerInfo_Click;
		((Control)txtSleepMessage).Location = new Point(140, 209);
		((TextBoxBase)txtSleepMessage).MaxLength = 128;
		((TextBoxBase)txtSleepMessage).Multiline = true;
		((Control)txtSleepMessage).Name = "txtSleepMessage";
		((Control)txtSleepMessage).Size = new Size(429, 83);
		((Control)txtSleepMessage).TabIndex = 57;
		txtGlyphWidth.AcceptsReturn = true;
		((Control)txtGlyphWidth).BackColor = SystemColors.Window;
		((Control)txtGlyphWidth).Cursor = Cursors.IBeam;
		((Control)txtGlyphWidth).Enabled = false;
		((Control)txtGlyphWidth).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGlyphWidth).ForeColor = SystemColors.WindowText;
		((Control)txtGlyphWidth).Location = new Point(144, 317);
		((TextBoxBase)txtGlyphWidth).MaxLength = 8;
		((Control)txtGlyphWidth).Name = "txtGlyphWidth";
		((Control)txtGlyphWidth).RightToLeft = (RightToLeft)0;
		((Control)txtGlyphWidth).Size = new Size(133, 26);
		((Control)txtGlyphWidth).TabIndex = 59;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = SystemColors.Control;
		((Control)label3).Cursor = Cursors.Default;
		((Control)label3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = SystemColors.ControlText;
		((Control)label3).Location = new Point(26, 320);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)0;
		((Control)label3).Size = new Size(89, 19);
		((Control)label3).TabIndex = 58;
		((Control)label3).Text = "Glyph Width:";
		txtGlyphHeight.AcceptsReturn = true;
		((Control)txtGlyphHeight).BackColor = SystemColors.Window;
		((Control)txtGlyphHeight).Cursor = Cursors.IBeam;
		((Control)txtGlyphHeight).Enabled = false;
		((Control)txtGlyphHeight).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGlyphHeight).ForeColor = SystemColors.WindowText;
		((Control)txtGlyphHeight).Location = new Point(144, 348);
		((TextBoxBase)txtGlyphHeight).MaxLength = 8;
		((Control)txtGlyphHeight).Name = "txtGlyphHeight";
		((Control)txtGlyphHeight).RightToLeft = (RightToLeft)0;
		((Control)txtGlyphHeight).Size = new Size(133, 26);
		((Control)txtGlyphHeight).TabIndex = 61;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = SystemColors.Control;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(26, 351);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)0;
		((Control)label4).Size = new Size(91, 19);
		((Control)label4).TabIndex = 60;
		((Control)label4).Text = "Glyph Height:";
		txtFontHeight.AcceptsReturn = true;
		((Control)txtFontHeight).BackColor = SystemColors.Window;
		((Control)txtFontHeight).Cursor = Cursors.IBeam;
		((Control)txtFontHeight).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtFontHeight).ForeColor = SystemColors.WindowText;
		((Control)txtFontHeight).Location = new Point(144, 381);
		((TextBoxBase)txtFontHeight).MaxLength = 8;
		((Control)txtFontHeight).Name = "txtFontHeight";
		((Control)txtFontHeight).RightToLeft = (RightToLeft)0;
		((Control)txtFontHeight).Size = new Size(133, 26);
		((Control)txtFontHeight).TabIndex = 63;
		((Control)txtFontHeight).Text = "20";
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = SystemColors.Control;
		((Control)label5).Cursor = Cursors.Default;
		((Control)label5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = SystemColors.ControlText;
		((Control)label5).Location = new Point(26, 384);
		((Control)label5).Name = "label5";
		((Control)label5).RightToLeft = (RightToLeft)0;
		((Control)label5).Size = new Size(83, 19);
		((Control)label5).TabIndex = 62;
		((Control)label5).Text = "Font Height:";
		txtFontWidth.AcceptsReturn = true;
		((Control)txtFontWidth).BackColor = SystemColors.Window;
		((Control)txtFontWidth).Cursor = Cursors.IBeam;
		((Control)txtFontWidth).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtFontWidth).ForeColor = SystemColors.WindowText;
		((Control)txtFontWidth).Location = new Point(144, 413);
		((TextBoxBase)txtFontWidth).MaxLength = 8;
		((Control)txtFontWidth).Name = "txtFontWidth";
		((Control)txtFontWidth).RightToLeft = (RightToLeft)0;
		((Control)txtFontWidth).Size = new Size(133, 26);
		((Control)txtFontWidth).TabIndex = 65;
		((Control)txtFontWidth).Text = "11";
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = SystemColors.Control;
		((Control)label6).Cursor = Cursors.Default;
		((Control)label6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label6).ForeColor = SystemColors.ControlText;
		((Control)label6).Location = new Point(26, 416);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)0;
		((Control)label6).Size = new Size(81, 19);
		((Control)label6).TabIndex = 64;
		((Control)label6).Text = "Font Width:";
		txtFontWeight.AcceptsReturn = true;
		((Control)txtFontWeight).BackColor = SystemColors.Window;
		((Control)txtFontWeight).Cursor = Cursors.IBeam;
		((Control)txtFontWeight).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtFontWeight).ForeColor = SystemColors.WindowText;
		((Control)txtFontWeight).Location = new Point(144, 446);
		((TextBoxBase)txtFontWeight).MaxLength = 8;
		((Control)txtFontWeight).Name = "txtFontWeight";
		((Control)txtFontWeight).RightToLeft = (RightToLeft)0;
		((Control)txtFontWeight).Size = new Size(133, 26);
		((Control)txtFontWeight).TabIndex = 67;
		((Control)txtFontWeight).Text = "700";
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = SystemColors.Control;
		((Control)label7).Cursor = Cursors.Default;
		((Control)label7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label7).ForeColor = SystemColors.ControlText;
		((Control)label7).Location = new Point(26, 449);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)0;
		((Control)label7).Size = new Size(87, 19);
		((Control)label7).TabIndex = 66;
		((Control)label7).Text = "Font Weight:";
		((Control)cmdGetGlyphSize).BackColor = SystemColors.Control;
		((Control)cmdGetGlyphSize).Cursor = Cursors.Default;
		((Control)cmdGetGlyphSize).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetGlyphSize).ForeColor = SystemColors.ControlText;
		((Control)cmdGetGlyphSize).Location = new Point(351, 329);
		((Control)cmdGetGlyphSize).Name = "cmdGetGlyphSize";
		((Control)cmdGetGlyphSize).RightToLeft = (RightToLeft)0;
		((Control)cmdGetGlyphSize).Size = new Size(200, 30);
		((Control)cmdGetGlyphSize).TabIndex = 68;
		((Control)cmdGetGlyphSize).Text = "Get Glyph Size";
		((ButtonBase)cmdGetGlyphSize).UseVisualStyleBackColor = false;
		((Control)cmdGetGlyphSize).Click += cmdGetGlyphSize_Click;
		((Control)chkItalic).AutoSize = true;
		((ButtonBase)chkItalic).FlatStyle = (FlatStyle)3;
		((Control)chkItalic).Font = new Font("Times New Roman", 12f);
		((Control)chkItalic).Location = new Point(380, 376);
		((Control)chkItalic).Name = "chkItalic";
		((Control)chkItalic).Size = new Size(63, 24);
		((Control)chkItalic).TabIndex = 69;
		((Control)chkItalic).Text = "Italic";
		((ButtonBase)chkItalic).UseVisualStyleBackColor = true;
		((Control)chkUnderline).AutoSize = true;
		((ButtonBase)chkUnderline).FlatStyle = (FlatStyle)3;
		((Control)chkUnderline).Font = new Font("Times New Roman", 12f);
		((Control)chkUnderline).Location = new Point(380, 408);
		((Control)chkUnderline).Name = "chkUnderline";
		((Control)chkUnderline).Size = new Size(92, 24);
		((Control)chkUnderline).TabIndex = 70;
		((Control)chkUnderline).Text = "Underline";
		((ButtonBase)chkUnderline).UseVisualStyleBackColor = true;
		((Control)chkStrikeOut).AutoSize = true;
		((ButtonBase)chkStrikeOut).FlatStyle = (FlatStyle)3;
		((Control)chkStrikeOut).Font = new Font("Times New Roman", 12f);
		((Control)chkStrikeOut).Location = new Point(380, 441);
		((Control)chkStrikeOut).Name = "chkStrikeOut";
		((Control)chkStrikeOut).Size = new Size(93, 24);
		((Control)chkStrikeOut).TabIndex = 71;
		((Control)chkStrikeOut).Text = "StrikeOut";
		((ButtonBase)chkStrikeOut).UseVisualStyleBackColor = true;
		((Control)cmdSetSleepMsg).BackColor = SystemColors.Control;
		((Control)cmdSetSleepMsg).Cursor = Cursors.Default;
		((Control)cmdSetSleepMsg).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetSleepMsg).ForeColor = SystemColors.ControlText;
		((Control)cmdSetSleepMsg).Location = new Point(243, 547);
		((Control)cmdSetSleepMsg).Name = "cmdSetSleepMsg";
		((Control)cmdSetSleepMsg).RightToLeft = (RightToLeft)0;
		((Control)cmdSetSleepMsg).Size = new Size(200, 30);
		((Control)cmdSetSleepMsg).TabIndex = 73;
		((Control)cmdSetSleepMsg).Text = "Set Sleep Message";
		((ButtonBase)cmdSetSleepMsg).UseVisualStyleBackColor = false;
		((Control)cmdSetSleepMsg).Click += cmdSetSleepMsg_Click;
		((Control)cmdGetSleepMsg).BackColor = SystemColors.Control;
		((Control)cmdGetSleepMsg).Cursor = Cursors.Default;
		((Control)cmdGetSleepMsg).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetSleepMsg).ForeColor = SystemColors.ControlText;
		((Control)cmdGetSleepMsg).Location = new Point(37, 547);
		((Control)cmdGetSleepMsg).Name = "cmdGetSleepMsg";
		((Control)cmdGetSleepMsg).RightToLeft = (RightToLeft)0;
		((Control)cmdGetSleepMsg).Size = new Size(200, 30);
		((Control)cmdGetSleepMsg).TabIndex = 72;
		((Control)cmdGetSleepMsg).Text = "Get Sleep Message";
		((ButtonBase)cmdGetSleepMsg).UseVisualStyleBackColor = false;
		((Control)cmdGetSleepMsg).Click += cmdGetSleepMsg_Click;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(458, 547);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(113, 30);
		((Control)cmdExit).TabIndex = 74;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)chkDebugOut).AutoSize = true;
		((ButtonBase)chkDebugOut).FlatStyle = (FlatStyle)3;
		((Control)chkDebugOut).Font = new Font("Times New Roman", 12f);
		((Control)chkDebugOut).Location = new Point(52, 497);
		((Control)chkDebugOut).Name = "chkDebugOut";
		((Control)chkDebugOut).Size = new Size(97, 24);
		((Control)chkDebugOut).TabIndex = 75;
		((Control)chkDebugOut).Text = "DebugOut";
		((ButtonBase)chkDebugOut).UseVisualStyleBackColor = true;
		txtDebugOutFile.AcceptsReturn = true;
		((Control)txtDebugOutFile).BackColor = SystemColors.Window;
		((Control)txtDebugOutFile).Cursor = Cursors.IBeam;
		((Control)txtDebugOutFile).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtDebugOutFile).ForeColor = SystemColors.WindowText;
		((Control)txtDebugOutFile).Location = new Point(159, 497);
		((TextBoxBase)txtDebugOutFile).MaxLength = 8;
		((Control)txtDebugOutFile).Name = "txtDebugOutFile";
		((Control)txtDebugOutFile).RightToLeft = (RightToLeft)0;
		((Control)txtDebugOutFile).Size = new Size(355, 26);
		((Control)txtDebugOutFile).TabIndex = 76;
		((Control)txtDebugOutFile).Text = "C:\\temp.bmp";
		((Control)cmdDebugOutFileBrowse).BackColor = SystemColors.Control;
		((Control)cmdDebugOutFileBrowse).Cursor = Cursors.Default;
		((Control)cmdDebugOutFileBrowse).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDebugOutFileBrowse).ForeColor = SystemColors.ControlText;
		((Control)cmdDebugOutFileBrowse).Location = new Point(514, 496);
		((Control)cmdDebugOutFileBrowse).Name = "cmdDebugOutFileBrowse";
		((Control)cmdDebugOutFileBrowse).RightToLeft = (RightToLeft)0;
		((Control)cmdDebugOutFileBrowse).Size = new Size(30, 27);
		((Control)cmdDebugOutFileBrowse).TabIndex = 77;
		((Control)cmdDebugOutFileBrowse).Text = "...";
		((ButtonBase)cmdDebugOutFileBrowse).UseVisualStyleBackColor = false;
		((Control)cmdDebugOutFileBrowse).Click += cmdDebugOutFileBrowse_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(600, 589);
		((Control)this).Controls.Add((Control)(object)cmdDebugOutFileBrowse);
		((Control)this).Controls.Add((Control)(object)txtDebugOutFile);
		((Control)this).Controls.Add((Control)(object)chkDebugOut);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdSetSleepMsg);
		((Control)this).Controls.Add((Control)(object)cmdGetSleepMsg);
		((Control)this).Controls.Add((Control)(object)chkStrikeOut);
		((Control)this).Controls.Add((Control)(object)chkUnderline);
		((Control)this).Controls.Add((Control)(object)chkItalic);
		((Control)this).Controls.Add((Control)(object)cmdGetGlyphSize);
		((Control)this).Controls.Add((Control)(object)txtFontWeight);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)txtFontWidth);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)txtFontHeight);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)txtGlyphHeight);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)txtGlyphWidth);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)txtSleepMessage);
		((Control)this).Controls.Add((Control)(object)cmdSetCustomerInfo);
		((Control)this).Controls.Add((Control)(object)cmdGetCustomerInfo);
		((Control)this).Controls.Add((Control)(object)txtCustomerName);
		((Control)this).Controls.Add((Control)(object)txtCompanyName);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmScreenSaver";
		((Control)this).Text = "frmScreenSaver";
		((Form)this).Load += frmScreenSaver_Load;
		((Form)this).FormClosing += new FormClosingEventHandler(frmScreenSaver_FormClosing);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
