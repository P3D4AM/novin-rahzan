using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Froms.LkManage;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceUI.Forms.Other;

public class FrmSoftwareProp : LkUiFactory
{
	private IContainer components;

	private PanelEx panelEx1;

	private PanelEx panelEx2;

	private LabelX LBLVersionNO;

	private LabelX LBLSerilLK;

	private LabelX labelX2;

	private LabelX labelX1;

	private LabelX LBLVersionName;

	private LabelX labelX3;

	private async Task loadDataAsync()
	{
		try
		{
			LkOperations lkOperations = LkOperations.GetLkOperations(((CacheBaseForm)this).ContextGuid);
			Func<string> serial = lkOperations.LkSerial;
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)LBLVersionNO).Text = VersionManagment.AssemblyVersion;
					((Control)LBLSerilLK).Text = serial.ToString();
					((Control)LBLVersionName).Text = "تابستان 1404";
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmSoftwareProp(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		((Control)LBLVersionNO).Text = VersionManagment.AssemblyVersion;
		((Control)LBLSerilLK).Text = LkOperations.GetLkOperations(((CacheBaseForm)this).ContextGuid).LkSerial();
		((Control)LBLVersionName).Text = "تابستان 1404";
	}

	private void FRMSoftwareProp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
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
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Expected O, but got Unknown
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		panelEx2 = new PanelEx();
		LBLVersionNO = new LabelX();
		LBLVersionName = new LabelX();
		LBLSerilLK = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)panelEx2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(368, 133);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)panelEx2).Anchor = (AnchorStyles)15;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)LBLVersionNO);
		((Control)panelEx2).Controls.Add((Control)(object)LBLVersionName);
		((Control)panelEx2).Controls.Add((Control)(object)LBLSerilLK);
		((Control)panelEx2).Controls.Add((Control)(object)labelX3);
		((Control)panelEx2).Controls.Add((Control)(object)labelX2);
		((Control)panelEx2).Controls.Add((Control)(object)labelX1);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Location = new Point(7, 12);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(349, 109);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.BorderWidth = 2;
		panelEx2.Style.CornerType = (eCornerType)2;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 0;
		((Control)LBLVersionNO).Anchor = (AnchorStyles)9;
		((BaseItemControl)LBLVersionNO).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LBLVersionNO).ForeColor = Color.Black;
		((Control)LBLVersionNO).Location = new Point(67, 14);
		((Control)LBLVersionNO).Name = "LBLVersionNO";
		((Control)LBLVersionNO).Size = new Size(156, 23);
		((Control)LBLVersionNO).TabIndex = 8;
		((Control)LBLVersionNO).Text = "1.1.1";
		((Control)LBLVersionName).Anchor = (AnchorStyles)9;
		((BaseItemControl)LBLVersionName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LBLVersionName).ForeColor = Color.Black;
		((Control)LBLVersionName).Location = new Point(67, 43);
		((Control)LBLVersionName).Name = "LBLVersionName";
		((Control)LBLVersionName).Size = new Size(156, 23);
		((Control)LBLVersionName).TabIndex = 5;
		((Control)LBLVersionName).Text = "----------";
		((Control)LBLSerilLK).Anchor = (AnchorStyles)9;
		((BaseItemControl)LBLSerilLK).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LBLSerilLK).ForeColor = Color.Black;
		((Control)LBLSerilLK).Location = new Point(67, 72);
		((Control)LBLSerilLK).Name = "LBLSerilLK";
		((Control)LBLSerilLK).Size = new Size(156, 23);
		((Control)LBLSerilLK).TabIndex = 5;
		((Control)LBLSerilLK).Text = "----------";
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.DarkRed;
		((Control)labelX3).Location = new Point(233, 43);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(107, 23);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "[نام نسخه] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).ForeColor = Color.DarkRed;
		((Control)labelX2).Location = new Point(233, 72);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(107, 23);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[سریال قفل] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.DarkRed;
		((Control)labelX1).Location = new Point(233, 14);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(107, 23);
		((Control)labelX1).TabIndex = 3;
		((Control)labelX1).Text = "[نسخه] (version) :";
		labelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(368, 133);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmSoftwareProp";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مشخصات نرم افزار]";
		((Control)this).KeyDown += new KeyEventHandler(FRMSoftwareProp_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
