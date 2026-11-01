using System;
using System.Collections.Generic;
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
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Sms;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Sms;

public class FrmSmsCalendarDet : BaseForm
{
	private readonly SmsCalendar _cls;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	internal PanelEx PanelEx2;

	private TextBox TxtDesc;

	internal DateBoxFa FarsiDateDate;

	internal LabelX LabelX2;

	internal LabelX LabelX4;

	internal ComboBoxEx CmbGroup;

	internal LabelX labelX1;

	private BindingSource bindingSourcePersonGroup;

	public FrmSmsCalendarDet(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			((Control)FarsiDateDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now);
			_cls = new SmsCalendar(((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 29);
		}
	}

	public FrmSmsCalendarDet(string contextGuid, SmsCalendar smsCalendar)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			((Control)this).Text = "ویرایش ";
			_cls = smsCalendar;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 40);
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)FarsiDateDate).Text = Calendar.MiladiToCalture(_cls.DateM);
			((Control)TxtDesc).Text = _cls.Description;
			if (((ListControl)CmbGroup).SelectedIndex != -1)
			{
				((ListControl)CmbGroup).SelectedValue = 2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 61);
		}
	}

	private void GetData()
	{
		try
		{
			_cls.DateM = FarsiDateDate.MiladiDate;
			_cls.Description = Text.FixString(((Control)TxtDesc).Text.Trim());
			_cls.GroupId = int.Parse(((ListControl)CmbGroup).SelectedValue.ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 75);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Visible || !((Control)BtnAccept).Enabled)
		{
			return;
		}
		try
		{
			GetData();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await _cls.SaveAsync(token);
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 94);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 107);
		}
	}

	private void FRMHoliday_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 116)
					{
						e.Handled = true;
						((PopupItemControl)BtnAccept).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHoliday_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 134);
		}
	}

	private void FrmSmsCalendarDet_Load(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("در حال بارگزاری ...", (Form)(object)this, LoadDataGroupAsync(token), false, (EnWaiterType)0, (Control)null, "FrmSmsCalendarDet_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataGroupAsync(CancellationToken token)
	{
		try
		{
			List<PersonGroup> dataSource = await PersonGroup.SelectAllAsync(((CacheBaseForm)this).ContextGuid, containsAllGroups: true, token);
			bindingSourcePersonGroup.DataSource = dataSource;
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Sms\\FrmSmsCalendarDet.cs", 160);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Expected O, but got Unknown
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		PanelEx2 = new PanelEx();
		labelX1 = new LabelX();
		CmbGroup = new ComboBoxEx();
		bindingSourcePersonGroup = new BindingSource(components);
		TxtDesc = new TextBox();
		FarsiDateDate = new DateBoxFa();
		LabelX2 = new LabelX();
		LabelX4 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)bindingSourcePersonGroup).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)PanelEx2);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(351, 261);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)labelX1);
		((Control)PanelEx2).Controls.Add((Control)(object)CmbGroup);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)FarsiDateDate);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX4);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(7, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(337, 191);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.BorderWidth = 3;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(132, 67);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(188, 23);
		((Control)labelX1).TabIndex = 8;
		((Control)labelX1).Text = "عدم ارسال پیام برای تاریخ:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)CmbGroup).Anchor = (AnchorStyles)13;
		((ComboBox)CmbGroup).DataSource = bindingSourcePersonGroup;
		((ListControl)CmbGroup).DisplayMember = "Name";
		((ComboBox)CmbGroup).DrawMode = (DrawMode)1;
		((ComboBox)CmbGroup).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbGroup).FormattingEnabled = true;
		((ComboBox)CmbGroup).IntegralHeight = false;
		((ComboBox)CmbGroup).ItemHeight = 15;
		((Control)CmbGroup).Location = new Point(13, 36);
		((Control)CmbGroup).Name = "CmbGroup";
		((Control)CmbGroup).Size = new Size(313, 21);
		CmbGroup.Style = (eDotNetBarStyle)9;
		((Control)CmbGroup).TabIndex = 4;
		((ListControl)CmbGroup).ValueMember = "Id";
		bindingSourcePersonGroup.DataSource = typeof(PersonGroup);
		((Control)TxtDesc).Anchor = (AnchorStyles)13;
		((Control)TxtDesc).Location = new Point(13, 100);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(268, 77);
		((Control)TxtDesc).TabIndex = 7;
		FarsiDateDate.AllowNull = false;
		((Control)FarsiDateDate).Anchor = (AnchorStyles)13;
		FarsiDateDate.BackcolorTooltip = Color.Red;
		((TextBoxX)FarsiDateDate).Border.CornerType = (eCornerType)1;
		FarsiDateDate.EnterToTab = true;
		FarsiDateDate.EscToClose = true;
		FarsiDateDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		FarsiDateDate.FocusBackColor = Color.Yellow;
		FarsiDateDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		FarsiDateDate.FocusForeColor = Color.Blue;
		FarsiDateDate.FocusTextSelect = true;
		((Control)FarsiDateDate).Font = new Font("Arial", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)FarsiDateDate).Location = new Point(13, 67);
		FarsiDateDate.MessageEmptyShowDialog = false;
		FarsiDateDate.MessegeEmpty = "";
		FarsiDateDate.MessegeEmptyInFormRight = true;
		FarsiDateDate.MessegeEmptyShowInForm = false;
		FarsiDateDate.MiladiDate = new DateTime(2024, 10, 14, 4, 48, 43, 537);
		((Control)FarsiDateDate).Name = "FarsiDateDate";
		((Control)FarsiDateDate).RightToLeft = (RightToLeft)0;
		FarsiDateDate.ShowNameOfDayOnToolTip = false;
		FarsiDateDate.ShowToolTipText = false;
		((Control)FarsiDateDate).Size = new Size(113, 23);
		((Control)FarsiDateDate).TabIndex = 1;
		((Control)FarsiDateDate).Text = "13/10/2024";
		((TextBox)FarsiDateDate).TextAlign = (HorizontalAlignment)2;
		FarsiDateDate.TootTipDelay = 500;
		((Control)LabelX2).Anchor = (AnchorStyles)13;
		((BaseItemControl)LabelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX2).Location = new Point(49, 11);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).RightToLeft = (RightToLeft)1;
		((Control)LabelX2).Size = new Size(247, 23);
		((Control)LabelX2).TabIndex = 0;
		((Control)LabelX2).Text = "گروهی که نباید پیام ارسال شود را انتخاب کنید";
		LabelX2.TextAlignment = (StringAlignment)2;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX4).Location = new Point(287, 100);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).RightToLeft = (RightToLeft)1;
		((Control)LabelX4).Size = new Size(39, 23);
		((Control)LabelX4).TabIndex = 6;
		((Control)LabelX4).Text = "علت:";
		LabelX4.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(7, 209);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(104, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(117, 209);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(227, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(351, 261);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(367, 300);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(367, 300);
		((Control)this).Name = "FrmSmsCalendarDet";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FrmSmsCalendarDet_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)PanelEx2).PerformLayout();
		((ISupportInitialize)bindingSourcePersonGroup).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
