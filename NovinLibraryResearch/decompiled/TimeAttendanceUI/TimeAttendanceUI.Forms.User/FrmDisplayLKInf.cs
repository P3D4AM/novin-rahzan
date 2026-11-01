using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using LK;
using NPResourceManagement;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.Forms.User;

public class FrmDisplayLKInf : CacheBaseForm
{
	private IContainer components;

	private LabelX lblLkKey;

	private LabelX lblLkSoftware;

	private LabelX labelX1;

	private LabelX labelX2;

	private LabelX labelX3;

	public string LkStr => ((Control)lblLkKey).Text;

	public string SoftwareStr => ((Control)lblLkSoftware).Text;

	public FrmDisplayLKInf(string contextGuid)
		: base(contextGuid)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		((Control)lblLkKey).Text = LK.GetSerialNumber(MainFormOwner.owner, (Package)2).ToString();
		ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor = SoftwareSerialManager.GetSoftwareSerial_Hozor();
		if (((ReturnedSaveFuncInfo)softwareSerial_Hozor).HasError)
		{
			((ReturnedSaveFuncInfo)(object)softwareSerial_Hozor).Show(contextGuid);
		}
		((Control)lblLkSoftware).Text = softwareSerial_Hozor.value;
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		lblLkKey = new LabelX();
		lblLkSoftware = new LabelX();
		labelX1 = new LabelX();
		labelX2 = new LabelX();
		labelX3 = new LabelX();
		((Control)this).SuspendLayout();
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblLkKey).Font = new Font("Tahoma", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblLkKey).ForeColor = SystemColors.HotTrack;
		((Control)lblLkKey).Location = new Point(12, 12);
		((Control)lblLkKey).Name = "lblLkKey";
		((Control)lblLkKey).Size = new Size(308, 66);
		((Control)lblLkKey).TabIndex = 0;
		((Control)lblLkKey).Text = "سریال قفل نرم افزار";
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblLkSoftware).Font = new Font("Tahoma", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblLkSoftware).ForeColor = Color.Red;
		((Control)lblLkSoftware).Location = new Point(12, 84);
		((Control)lblLkSoftware).Name = "lblLkSoftware";
		((Control)lblLkSoftware).Size = new Size(308, 63);
		((Control)lblLkSoftware).TabIndex = 0;
		((Control)lblLkSoftware).Text = "سریال قفل نرم افزار";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = SystemColors.HotTrack;
		((Control)labelX1).Location = new Point(732, 12);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(308, 66);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = TextResourceManagement.TextResource.CompanyName;
		labelX1.TextAlignment = (StringAlignment)1;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).ForeColor = SystemColors.HotTrack;
		((Control)labelX2).Location = new Point(562, 84);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(478, 66);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "شماره تماس : 9 - 05137597590";
		labelX2.TextAlignment = (StringAlignment)1;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = SystemColors.HotTrack;
		((Control)labelX3).Location = new Point(562, 156);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(478, 66);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "نرم افزار حضور و غیاب - حقوق و دستمزد";
		labelX3.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1052, 516);
		((Control)this).Controls.Add((Control)(object)lblLkSoftware);
		((Control)this).Controls.Add((Control)(object)labelX3);
		((Control)this).Controls.Add((Control)(object)labelX2);
		((Control)this).Controls.Add((Control)(object)labelX1);
		((Control)this).Controls.Add((Control)(object)lblLkKey);
		((Control)this).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "FrmDisplayLKInf";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FrmDisplayLKInf";
		((Form)this).TransparencyKey = SystemColors.Control;
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).ResumeLayout(false);
	}
}
