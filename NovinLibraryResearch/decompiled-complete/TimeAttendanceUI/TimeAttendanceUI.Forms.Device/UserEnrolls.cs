using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxSBXPCLib;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using Microsoft.EntityFrameworkCore.Internal;
using PacketParser.BusinessLayer;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Enroll;
using TimeAttendanceDevices;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Device;

public class UserEnrolls : CacheBaseForm
{
	public List<DeviceEnroll> DeviceEnrollList = new List<DeviceEnroll>();

	private AxSBXPC bpc;

	private ClsSbxpc sbxpc;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal DataGridViewX DGridPersonelOnDevice;

	private LabelX CountRows;

	private LabelX labelX2;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	internal ButtonX BtnGetFromDevice;

	internal DataGridViewX DGridPersonelOnDatabase;

	internal ButtonX BtnRight;

	internal ButtonX BtnLeft;

	internal GroupPanel groupPanel2;

	internal TextBoxX textBoxX1;

	internal ButtonX BtnDelete;

	private LabelX labelX3;

	private LabelX labelX1;

	private PictureBox PBoxDatabase;

	private PictureBox PBoxDevice;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private BindingSource bindingSourceDeviceData;

	private BindingSource bindingSourceDataSql;

	private DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn eMachineNumberDataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn enrollNumberDataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn priviligeDataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn password1DataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn eMachineNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn enrollNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn VerifyName;

	private DataGridViewTextBoxColumn priviligeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn password1DataGridViewTextBoxColumn;

	public DeviceInfo _clsDeviceInfo { get; set; }

	public UserEnrolls(string contextGuid, DeviceInfo deviceInfo, AxSBXPC axSBXPC)
		: base(contextGuid)
	{
		_clsDeviceInfo = deviceInfo;
		bpc = axSBXPC;
		InitializeComponent();
	}

	private void UserEnrolls_Load(object sender, EventArgs e)
	{
		sbxpc = new ClsSbxpc(bpc);
		GetEnrollsFromDevice();
	}

	private async Task<bool> GetEnrollsFromDevice(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!((Control)BtnGetFromDevice).Enabled)
			{
				return false;
			}
			((Control)BtnGetFromDevice).Enabled = false;
			if (_clsDeviceInfo != null && (int)_clsDeviceInfo.TypeDeviceManufacture == 2)
			{
				string ipAddress = _clsDeviceInfo.Ip;
				int deviceNo = _clsDeviceInfo.DeviceNo;
				int tcpPort = _clsDeviceInfo.TcpPort;
				int password = int.Parse(_clsDeviceInfo.CommunicationPass);
				if (sbxpc.ConnectTcpIp(deviceNo, ref ipAddress, tcpPort, password) ? true : false)
				{
					ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>> rews = sbxpc.GetAllEnrollData(_clsDeviceInfo.DeviceNo);
					List<DeviceEnroll> value = rews.value;
					if (value != null && EnumerableExtensions.Any((IEnumerable)value))
					{
						foreach (DeviceEnroll item in rews.value)
						{
							item.CompanyName = _clsDeviceInfo.DeviceName;
							item.VerifyName = GetVerifyName(item.FingerNumber);
						}
					}
					do
					{
						await Task.Delay(100);
						if (((CacheBaseForm)this).IsDisposed || token.IsCancellationRequested)
						{
							return false;
						}
					}
					while (!((Control)this).IsHandleCreated);
					((Control)this).BeginInvoke((Delegate)(Action)delegate
					{
						try
						{
							bindingSourceDeviceData.DataSource = rews?.value;
							((Control)CountRows).Text = bindingSourceDeviceData.Count.ToString();
						}
						catch (TaskCanceledException)
						{
						}
						catch (Exception ex3)
						{
							Log.Error(ex3, "");
						}
					});
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			((Control)BtnGetFromDevice).Enabled = true;
		}
		return false;
	}

	private string GetVerifyName(int fingerNumber)
	{
		try
		{
			return fingerNumber switch
			{
				0 => "انگشت 1", 
				1 => "انگشت 2", 
				2 => "انگشت 3", 
				3 => "انگشت 4", 
				4 => "انگشت 5", 
				5 => "انگشت 6", 
				6 => "انگشت 7", 
				7 => "انگشت 8", 
				8 => "انگشت 9", 
				9 => "انگشت 10", 
				11 => "کارت", 
				14 => "تایم زون کاربر", 
				15 => "رمز", 
				16 => "دپارتمان یوزر", 
				17 => "صورت", 
				_ => fingerNumber.ToString(), 
			};
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return "نامشخص";
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((CacheBaseForm)this).Dispose(disposing);
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
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Expected O, but got Unknown
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		//IL_0ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Expected O, but got Unknown
		//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e93: Expected O, but got Unknown
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_106a: Unknown result type (might be due to invalid IL or missing references)
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_132f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b3: Expected O, but got Unknown
		//IL_1407: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c0: Expected O, but got Unknown
		//IL_1514: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a43: Expected O, but got Unknown
		//IL_1a48: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		pictureBox2 = new PictureBox();
		pictureBox1 = new PictureBox();
		PBoxDatabase = new PictureBox();
		PBoxDevice = new PictureBox();
		labelX3 = new LabelX();
		labelX1 = new LabelX();
		groupPanel2 = new GroupPanel();
		textBoxX1 = new TextBoxX();
		BtnLeft = new ButtonX();
		BtnRight = new ButtonX();
		DGridPersonelOnDatabase = new DataGridViewX();
		bindingSourceDataSql = new BindingSource(components);
		DGridPersonelOnDevice = new DataGridViewX();
		bindingSourceDeviceData = new BindingSource(components);
		CountRows = new LabelX();
		labelX2 = new LabelX();
		GroupPanel1 = new GroupPanel();
		TxtSearch = new TextBoxX();
		BtnDelete = new ButtonX();
		BtnGetFromDevice = new ButtonX();
		companyNameDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		eMachineNumberDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		enrollNumberDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		priviligeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		password1DataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		companyNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		eMachineNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		enrollNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		VerifyName = new DataGridViewTextBoxColumn();
		priviligeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		password1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)PBoxDatabase).BeginInit();
		((ISupportInitialize)PBoxDevice).BeginInit();
		((Control)groupPanel2).SuspendLayout();
		((ISupportInitialize)DGridPersonelOnDatabase).BeginInit();
		((ISupportInitialize)bindingSourceDataSql).BeginInit();
		((ISupportInitialize)DGridPersonelOnDevice).BeginInit();
		((ISupportInitialize)bindingSourceDeviceData).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox2);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)PanelEx1).Controls.Add((Control)(object)PBoxDatabase);
		((Control)PanelEx1).Controls.Add((Control)(object)PBoxDevice);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX3);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX1);
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel2);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnLeft);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnRight);
		((Control)PanelEx1).Controls.Add((Control)(object)DGridPersonelOnDatabase);
		((Control)PanelEx1).Controls.Add((Control)(object)DGridPersonelOnDevice);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnGetFromDevice);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(1298, 745);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 4;
		pictureBox2.Image = (Image)(object)Resources.backup;
		((Control)pictureBox2).Location = new Point(12, 6);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(65, 66);
		pictureBox2.SizeMode = (PictureBoxSizeMode)1;
		pictureBox2.TabIndex = 17;
		pictureBox2.TabStop = false;
		pictureBox1.Image = (Image)(object)Resources.hozor;
		((Control)pictureBox1).Location = new Point(1227, 5);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(65, 60);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 16;
		pictureBox1.TabStop = false;
		PBoxDatabase.Image = (Image)(object)Resources.backup;
		((Control)PBoxDatabase).Location = new Point(732, 359);
		((Control)PBoxDatabase).Name = "PBoxDatabase";
		((Control)PBoxDatabase).Size = new Size(47, 47);
		PBoxDatabase.SizeMode = (PictureBoxSizeMode)1;
		PBoxDatabase.TabIndex = 15;
		PBoxDatabase.TabStop = false;
		PBoxDevice.Image = (Image)(object)Resources.hozor;
		((Control)PBoxDevice).Location = new Point(732, 171);
		((Control)PBoxDevice).Name = "PBoxDevice";
		((Control)PBoxDevice).Size = new Size(47, 44);
		PBoxDevice.SizeMode = (PictureBoxSizeMode)1;
		PBoxDevice.TabIndex = 14;
		PBoxDevice.TabStop = false;
		((Control)labelX3).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).ForeColor = Color.DarkRed;
		((Control)labelX3).Location = new Point(528, 696);
		((Control)labelX3).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(52, 25);
		((Control)labelX3).TabIndex = 13;
		((Control)labelX3).Text = "labelX3";
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(585, 696);
		((Control)labelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(139, 25);
		((Control)labelX1).TabIndex = 12;
		((Control)labelX1).Text = "تعداد پرسنل در پایگاه داده:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)groupPanel2).Anchor = (AnchorStyles)9;
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel2).Controls.Add((Control)(object)textBoxX1);
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(787, 3);
		((Control)groupPanel2).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(433, 65);
		((PanelControl)groupPanel2).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel2).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel2).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel2).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel2).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel2).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel2).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel2).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel2).Style.CornerDiameter = 4;
		((PanelControl)groupPanel2).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel2).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)groupPanel2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel2).TabIndex = 11;
		((Control)groupPanel2).Text = "جستجوی پرسنل روی دستگاه";
		groupPanel2.TitleImage = (Image)(object)Resources.search2;
		((Control)textBoxX1).Anchor = (AnchorStyles)13;
		textBoxX1.Border.Class = "TextBoxBorder";
		textBoxX1.Border.CornerType = (eCornerType)1;
		((Control)textBoxX1).Location = new Point(4, 5);
		((Control)textBoxX1).Margin = new Padding(4, 3, 4, 3);
		((Control)textBoxX1).Name = "textBoxX1";
		((Control)textBoxX1).Size = new Size(419, 22);
		((Control)textBoxX1).TabIndex = 1;
		((Control)BtnLeft).AccessibleRole = (AccessibleRole)43;
		((Control)BtnLeft).Anchor = (AnchorStyles)9;
		BtnLeft.ColorTable = (eButtonColor)6;
		((Control)BtnLeft).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnLeft.Image = (Image)(object)Resources.left3;
		BtnLeft.ImagePosition = (eImagePosition)1;
		((Control)BtnLeft).Location = new Point(732, 412);
		((Control)BtnLeft).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnLeft).Name = "BtnLeft";
		((Control)BtnLeft).Size = new Size(47, 45);
		((PopupItemControl)BtnLeft).Style = (eDotNetBarStyle)9;
		((Control)BtnLeft).TabIndex = 10;
		((Control)BtnRight).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRight).Anchor = (AnchorStyles)9;
		((Control)BtnRight).BackgroundImageLayout = (ImageLayout)2;
		BtnRight.ColorTable = (eButtonColor)6;
		((Control)BtnRight).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnRight.Image = (Image)(object)Resources.right3;
		BtnRight.ImagePosition = (eImagePosition)1;
		((Control)BtnRight).Location = new Point(732, 221);
		((Control)BtnRight).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnRight).Name = "BtnRight";
		((Control)BtnRight).Size = new Size(47, 43);
		((PopupItemControl)BtnRight).Style = (eDotNetBarStyle)9;
		((Control)BtnRight).TabIndex = 9;
		((DataGridView)DGridPersonelOnDatabase).AllowUserToAddRows = false;
		((DataGridView)DGridPersonelOnDatabase).AllowUserToDeleteRows = false;
		((DataGridView)DGridPersonelOnDatabase).AllowUserToOrderColumns = true;
		((DataGridView)DGridPersonelOnDatabase).AllowUserToResizeRows = false;
		((Control)DGridPersonelOnDatabase).Anchor = (AnchorStyles)15;
		((DataGridView)DGridPersonelOnDatabase).AutoGenerateColumns = false;
		((DataGridView)DGridPersonelOnDatabase).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridPersonelOnDatabase).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)companyNameDataGridViewTextBoxColumn1,
			(DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1,
			(DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)priviligeDataGridViewTextBoxColumn1,
			(DataGridViewColumn)password1DataGridViewTextBoxColumn1
		});
		((DataGridView)DGridPersonelOnDatabase).DataSource = bindingSourceDataSql;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 9f);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridPersonelOnDatabase).DefaultCellStyle = val;
		((DataGridView)DGridPersonelOnDatabase).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridPersonelOnDatabase).Location = new Point(12, 78);
		((Control)DGridPersonelOnDatabase).Margin = new Padding(4, 3, 4, 3);
		((Control)DGridPersonelOnDatabase).Name = "DGridPersonelOnDatabase";
		((DataGridView)DGridPersonelOnDatabase).ReadOnly = true;
		((Control)DGridPersonelOnDatabase).RightToLeft = (RightToLeft)1;
		((DataGridView)DGridPersonelOnDatabase).RowHeadersVisible = false;
		((DataGridView)DGridPersonelOnDatabase).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridPersonelOnDatabase).Size = new Size(712, 612);
		((Control)DGridPersonelOnDatabase).TabIndex = 8;
		bindingSourceDataSql.DataSource = typeof(DeviceEnroll);
		((DataGridView)DGridPersonelOnDevice).AllowUserToAddRows = false;
		((DataGridView)DGridPersonelOnDevice).AllowUserToDeleteRows = false;
		((DataGridView)DGridPersonelOnDevice).AllowUserToOrderColumns = true;
		((DataGridView)DGridPersonelOnDevice).AllowUserToResizeRows = false;
		((Control)DGridPersonelOnDevice).Anchor = (AnchorStyles)11;
		((DataGridView)DGridPersonelOnDevice).AutoGenerateColumns = false;
		((DataGridView)DGridPersonelOnDevice).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridPersonelOnDevice).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)companyNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)VerifyName,
			(DataGridViewColumn)priviligeDataGridViewTextBoxColumn,
			(DataGridViewColumn)password1DataGridViewTextBoxColumn
		});
		((DataGridView)DGridPersonelOnDevice).DataSource = bindingSourceDeviceData;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 9f);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridPersonelOnDevice).DefaultCellStyle = val2;
		((DataGridView)DGridPersonelOnDevice).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridPersonelOnDevice).Location = new Point(787, 78);
		((Control)DGridPersonelOnDevice).Margin = new Padding(4, 3, 4, 3);
		((Control)DGridPersonelOnDevice).Name = "DGridPersonelOnDevice";
		((DataGridView)DGridPersonelOnDevice).ReadOnly = true;
		((Control)DGridPersonelOnDevice).RightToLeft = (RightToLeft)1;
		((DataGridView)DGridPersonelOnDevice).RowHeadersVisible = false;
		((DataGridView)DGridPersonelOnDevice).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridPersonelOnDevice).Size = new Size(505, 560);
		((Control)DGridPersonelOnDevice).TabIndex = 7;
		bindingSourceDeviceData.DataSource = typeof(DeviceEnroll);
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(1099, 644);
		((Control)CountRows).Margin = new Padding(4, 3, 4, 3);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(52, 25);
		((Control)CountRows).TabIndex = 6;
		((Control)CountRows).Text = "----";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(1159, 644);
		((Control)labelX2).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(133, 25);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "تعداد پرسنل روی دستگاه:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(84, 6);
		((Control)GroupPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(640, 65);
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
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "جستجوی پرسنل در پایگاه داده";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(4, 5);
		((Control)TxtSearch).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(626, 22);
		((Control)TxtSearch).TabIndex = 1;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelete).Anchor = (AnchorStyles)10;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImageFixedSize = new Size(25, 25);
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(787, 675);
		((Control)BtnDelete).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).RightToLeft = (RightToLeft)1;
		((Control)BtnDelete).Size = new Size(238, 36);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 4;
		((Control)BtnDelete).Text = "   حذف تمام اطلاعات پرسنل از دستگاه";
		BtnDelete.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnGetFromDevice).AccessibleRole = (AccessibleRole)43;
		((Control)BtnGetFromDevice).Anchor = (AnchorStyles)10;
		BtnGetFromDevice.ColorTable = (eButtonColor)6;
		((Control)BtnGetFromDevice).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnGetFromDevice.Image = (Image)(object)Resources.utaradod;
		BtnGetFromDevice.ImageFixedSize = new Size(25, 25);
		BtnGetFromDevice.ImagePosition = (eImagePosition)1;
		((Control)BtnGetFromDevice).Location = new Point(1054, 675);
		((Control)BtnGetFromDevice).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnGetFromDevice).Name = "BtnGetFromDevice";
		((Control)BtnGetFromDevice).RightToLeft = (RightToLeft)1;
		((Control)BtnGetFromDevice).Size = new Size(238, 36);
		((PopupItemControl)BtnGetFromDevice).Style = (eDotNetBarStyle)9;
		((Control)BtnGetFromDevice).TabIndex = 2;
		((Control)BtnGetFromDevice).Text = "   بازخوانی اطلاعات پرسنل از دستگاه";
		BtnGetFromDevice.TextAlignment = (eButtonTextAlignment)0;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn1).DataPropertyName = "CompanyName";
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn1).HeaderText = "CoName";
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn1).MinimumWidth = 50;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn1).Name = "companyNameDataGridViewTextBoxColumn1";
		((DataGridViewBand)companyNameDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).DataPropertyName = "EMachineNumber";
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).HeaderText = "DevNo";
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).MinimumWidth = 30;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).Name = "eMachineNumberDataGridViewTextBoxColumn1";
		((DataGridViewBand)eMachineNumberDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn1).Width = 68;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).DataPropertyName = "EnrollNumber";
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).HeaderText = "PersonNo";
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).MinimumWidth = 30;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).Name = "enrollNumberDataGridViewTextBoxColumn1";
		((DataGridViewBand)enrollNumberDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn1).Width = 84;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "VerifyName";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "VerifyName";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).MinimumWidth = 100;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).DataPropertyName = "Privilige";
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).HeaderText = "Privilige";
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).MinimumWidth = 30;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).Name = "priviligeDataGridViewTextBoxColumn1";
		((DataGridViewBand)priviligeDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn1).Width = 71;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).DataPropertyName = "Password1";
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).HeaderText = "Pass";
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).MinimumWidth = 30;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).Name = "password1DataGridViewTextBoxColumn1";
		((DataGridViewBand)password1DataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn1).Width = 55;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn).DataPropertyName = "CompanyName";
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn).HeaderText = "CoName";
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)companyNameDataGridViewTextBoxColumn).Name = "companyNameDataGridViewTextBoxColumn";
		((DataGridViewBand)companyNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).DataPropertyName = "EMachineNumber";
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).HeaderText = "DevNum";
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).Name = "eMachineNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)eMachineNumberDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)eMachineNumberDataGridViewTextBoxColumn).Width = 78;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).DataPropertyName = "EnrollNumber";
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).HeaderText = "PersonNo";
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).Name = "enrollNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)enrollNumberDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)enrollNumberDataGridViewTextBoxColumn).Width = 84;
		((DataGridViewColumn)VerifyName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)VerifyName).DataPropertyName = "VerifyName";
		((DataGridViewColumn)VerifyName).HeaderText = "VerifyName";
		((DataGridViewColumn)VerifyName).MinimumWidth = 100;
		((DataGridViewColumn)VerifyName).Name = "VerifyName";
		((DataGridViewBand)VerifyName).ReadOnly = true;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).DataPropertyName = "Privilige";
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).HeaderText = "Privilige";
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).Name = "priviligeDataGridViewTextBoxColumn";
		((DataGridViewBand)priviligeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)priviligeDataGridViewTextBoxColumn).Width = 71;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).DataPropertyName = "Password1";
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).HeaderText = "Pass";
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).MinimumWidth = 30;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).Name = "password1DataGridViewTextBoxColumn";
		((DataGridViewBand)password1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)password1DataGridViewTextBoxColumn).Width = 55;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 14f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1298, 745);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 9f);
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Control)this).Name = "UserEnrolls";
		((Control)this).Text = "مدیریت اطلاعات شناسایی پرسنل";
		((Form)this).Load += UserEnrolls_Load;
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)PBoxDatabase).EndInit();
		((ISupportInitialize)PBoxDevice).EndInit();
		((Control)groupPanel2).ResumeLayout(false);
		((ISupportInitialize)DGridPersonelOnDatabase).EndInit();
		((ISupportInitialize)bindingSourceDataSql).EndInit();
		((ISupportInitialize)DGridPersonelOnDevice).EndInit();
		((ISupportInitialize)bindingSourceDeviceData).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
