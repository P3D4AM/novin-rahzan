using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDevices.FLREbio;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Other;

public class FrmWaitGetDeviceInfo : BaseForm
{
	private readonly DeviceInfo _deviceInfo;

	public CancellationTokenSource tokenSource = new CancellationTokenSource();

	private IContainer components;

	internal PanelEx PanelEx1;

	private Label label2;

	private Timer timer1;

	private PictureBox pictureBox3;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private ButtonX BtnCancel;

	public FrmWaitGetDeviceInfo(string contextGuid, DeviceInfo deviceInfo)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_deviceInfo = deviceInfo;
		InitializeComponent();
	}

	private async Task ReceiveDate()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"in {DateTime.Now} method ReceiveData");
		try
		{
			if (_deviceInfo != null)
			{
				tokenSource?.Cancel();
				tokenSource = new CancellationTokenSource();
				CancellationToken token = tokenSource.Token;
				FrmEbio ifDeviceIsEbio = null;
				if ((int)_deviceInfo.TypeDeviceManufacture == 2)
				{
					ifDeviceIsEbio = (((int)_deviceInfo.TransferMode == 1) ? new FrmEbio(((CacheBaseForm)this).ContextGuid, _deviceInfo.DeviceNo, int.Parse(_deviceInfo.CommunicationPass), _deviceInfo.Ip, _deviceInfo.TcpPort) : new FrmEbio(((CacheBaseForm)this).ContextGuid, _deviceInfo.DeviceNo));
				}
				ReturnedSaveFuncInfo res = await _deviceInfo.ReceiveDataAsync(stringBuilder, ifDeviceIsEbio, token);
				if (res.HasError || res.HasWarning)
				{
					await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, res, "[دریافت داده]", (MessageBoxButtons)0, false, (LanguageTypes)0, "ReceiveDate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMWaitGetDeviceInfo.cs")).ShowAsync((IWin32Window)(object)this);
				}
				if (res.HasInformation)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, res.InformationMessage, "", 3);
				}
			}
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReceiveDate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMWaitGetDeviceInfo.cs", 57);
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		timer1.Stop();
		ReceiveDate();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		tokenSource?.Cancel();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmWaitGetDeviceInfo));
		PanelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		pictureBox3 = new PictureBox();
		pictureBox2 = new PictureBox();
		pictureBox1 = new PictureBox();
		label2 = new Label();
		timer1 = new Timer(components);
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox3).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox3);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox2);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)PanelEx1).Controls.Add((Control)(object)label2);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(251, 268);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.CornerType = (eCornerType)2;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 5;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)14;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 220);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(227, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 7;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "لغو عملیات دریافت ترددها";
		((Control)BtnCancel).Click += BtnCancel_Click;
		pictureBox3.Image = (Image)(object)Resources.backup;
		((Control)pictureBox3).Location = new Point(182, 148);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(59, 55);
		pictureBox3.SizeMode = (PictureBoxSizeMode)1;
		pictureBox3.TabIndex = 6;
		pictureBox3.TabStop = false;
		pictureBox2.Image = (Image)(object)Resources._6_2_arrow_png_file;
		((Control)pictureBox2).Location = new Point(80, 163);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(96, 40);
		pictureBox2.SizeMode = (PictureBoxSizeMode)1;
		pictureBox2.TabIndex = 5;
		pictureBox2.TabStop = false;
		pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
		((Control)pictureBox1).Location = new Point(12, 148);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(62, 55);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 4;
		pictureBox1.TabStop = false;
		((Control)label2).Anchor = (AnchorStyles)13;
		((Control)label2).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)label2).ForeColor = Color.Crimson;
		((Control)label2).Location = new Point(33, 9);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(185, 34);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "در حال دریافت اطلاعات دستگاه می باشد";
		label2.TextAlign = (ContentAlignment)32;
		timer1.Enabled = true;
		timer1.Interval = 500;
		timer1.Tick += timer1_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(251, 268);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FrmWaitGetDeviceInfo";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMWait";
		((Form)this).TransparencyKey = Color.Transparent;
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox3).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
