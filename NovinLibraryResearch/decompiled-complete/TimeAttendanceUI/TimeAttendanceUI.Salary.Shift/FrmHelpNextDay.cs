using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmHelpNextDay : CacheBaseForm
{
	private string _title = "";

	private string _desc = "";

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnExit;

	private LabelX labelX1;

	public FrmHelpNextDay(string contextGuid, string desc, string title)
		: base(contextGuid)
	{
		_title = title;
		_desc = desc;
		InitializeComponent();
		if (string.IsNullOrEmpty(_desc))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[در شیفت شناور]:");
			stringBuilder.AppendLine("[حداکثر مدت حضور در شیفت برابر است با مدت زمان موظفی حضور در شیفت + مدت زمان مازاد مجاز]");
			stringBuilder.AppendLine("[لذا اگر انتقال به روز بعد فعال باشد فقط در صورتیکه اولین تردد روز بعد، قبل از ]");
			stringBuilder.AppendLine("[مجموع زمان تردد ورود بعلاوه حداکثر مدت حضور در شیفت باشد ]");
			stringBuilder.AppendLine("[بعنوان تردد خروج در نظر گرفته شده و عملیات انتقال به روز بعد انجام خواهد شد]");
			stringBuilder.AppendLine("[در شیفت عادی:]");
			stringBuilder.AppendLine("[حداکثر زمان خروج ساعتی است که اگر شخص بعد از این ساعت تردد ثبت نماید بعنوان تردد ورود شیفت بعدی لحاظ خواهد شد]");
			stringBuilder.AppendLine("[لذا اگر انتقال به روز بعد فعال باشد فقط در صورتیکه اولین تردد روز بعد، قبل از حداکثر زمان خروج باشد ]");
			stringBuilder.AppendLine("[بعنوان تردد خروج در نظر گرفته شده و عملیات انتقال به روز بعد انجام خواهد شد]");
			_desc = stringBuilder.ToString();
		}
	}

	private void FrmHelpNextDay_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Text = Utilities.GetTranslate(_title);
			((Control)labelX1).Text = Utilities.GetTranslate(_desc);
			((Control)BtnExit).Text = Utilities.GetTranslate("[بسیار خوب]");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmHelpNextDay_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmHelpNextDay.cs", 58);
		}
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
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		panelEx1 = new PanelEx();
		BtnExit = new ButtonX();
		labelX1 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.ActiveCaptionText;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnExit);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(6);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(753, 329);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.FromArgb(255, 224, 192);
		panelEx1.Style.BackColor2.Color = Color.White;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)14;
		BtnExit.ColorTable = (eButtonColor)3;
		BtnExit.DialogResult = (DialogResult)2;
		((Control)BtnExit).Location = new Point(12, 279);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(729, 41);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 1;
		((Control)BtnExit).Text = "بسیار خوب";
		((Control)labelX1).Anchor = (AnchorStyles)13;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("B Homa", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Black;
		((Control)labelX1).Location = new Point(15, 15);
		((Control)labelX1).Margin = new Padding(6);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(723, 255);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "در حالت عادی";
		labelX1.TextLineAlignment = (StringAlignment)0;
		labelX1.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(13f, 29f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ActiveCaption;
		((Form)this).CancelButton = (IButtonControl)(object)BtnExit;
		((Form)this).ClientSize = new Size(753, 329);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("B Homa", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(769, 400);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(769, 300);
		((Control)this).Name = "FrmHelpNextDay";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "توضیحات انتقال از روز قبل";
		((Form)this).Load += FrmHelpNextDay_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
