using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class DefMorakhasiType : BaseForm
{
	private TypeMorakhasi cls;

	private bool _insert;

	private string Frm_Text = "[لیست عملکرد کاربران]";

	private string trCountRecords = "[تعداد رکوردها]";

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private PanelEx paneldays;

	private LabelX labelX8;

	private TextBoxX TxtMor;

	private CheckBoxX ChbKosor;

	private CheckBoxX ChbEzaf;

	private CheckBoxX ChbBime;

	private CheckBoxX ChbHoghogh;

	private CheckBoxX ChbMal;

	private Panel panel1;

	public DefMorakhasiType(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			cls = new TypeMorakhasi(contextGuid);
			Frm_Text = "[افزودن نوع مرخصی]";
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 32);
		}
	}

	public DefMorakhasiType(string contextGuid, TypeMorakhasi morakhasi)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			cls = morakhasi;
			Frm_Text = "[ویرایش نوع مرخصی]";
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 45);
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)TxtMor).Text = cls.Name;
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationManagement)
			{
				((Control)panel1).Enabled = false;
				return;
			}
			ChbHoghogh.Checked = cls.Hoghogh;
			ChbBime.Checked = cls.Bimeh;
			ChbEzaf.Checked = cls.Ezafat;
			ChbKosor.Checked = cls.Kosorat;
			ChbMal.Checked = cls.Maliyat;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 66);
		}
	}

	private async Task GetData()
	{
		try
		{
			if (cls.Id == 0)
			{
				_insert = true;
				int num = await TypeMorakhasi.MaxIDAsync(((CacheBaseForm)this).ContextGuid);
				cls.Id = short.Parse(num.ToString());
			}
			cls.Name = Text.FixString(((Control)TxtMor).Text);
			cls.Bimeh = ChbBime.Checked;
			cls.Hoghogh = ChbHoghogh.Checked;
			cls.Ezafat = ChbEzaf.Checked;
			cls.Kosorat = ChbKosor.Checked;
			cls.Maliyat = ChbMal.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 88);
		}
	}

	private bool checkName()
	{
		try
		{
			if (cls.Name != "")
			{
				return true;
			}
			Utility.MessageBoxShowError("[وارد کردن نام مرخصی الزامی است]", ((CacheBaseForm)this).ContextGuid);
			((Control)TxtMor).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "checkName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 102);
		}
		return false;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			await GetData();
			if (!checkName())
			{
				return;
			}
			if (_insert)
			{
				if (await cls.InsertAsync())
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else
				{
					((Control)BtnAccept).Enabled = true;
					((Control)TxtMor).Focus();
				}
			}
			else if (await cls.UpdateAsync())
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				((Control)BtnAccept).Enabled = true;
				((Control)TxtMor).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 142);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 154);
		}
	}

	private async void DefMorakhasiType_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode == 27)
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
			else if ((int)keyCode != 116)
			{
				if ((int)keyCode == 123)
				{
					e.Handled = true;
					FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BtnAccept).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DefMorakhasiType_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 186);
		}
	}

	private void TxtMor_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((TextBoxBase)(TextBox)sender).SelectAll();
			((Control)(TextBox)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtMor_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 198);
		}
	}

	private void TxtMor_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtMor_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 209);
		}
	}

	private void DefMorakhasiType_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			trCountRecords = Utilities.GetTranslate(trCountRecords);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DefMorakhasiType_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiType.cs", 223);
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091f: Expected O, but got Unknown
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		paneldays = new PanelEx();
		panel1 = new Panel();
		ChbMal = new CheckBoxX();
		ChbBime = new CheckBoxX();
		ChbEzaf = new CheckBoxX();
		ChbKosor = new CheckBoxX();
		ChbHoghogh = new CheckBoxX();
		TxtMor = new TextBoxX();
		labelX8 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)paneldays).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)paneldays);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(326, 174);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)paneldays).Anchor = (AnchorStyles)15;
		paneldays.CanvasColor = SystemColors.Control;
		paneldays.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)paneldays).Controls.Add((Control)(object)panel1);
		((Control)paneldays).Controls.Add((Control)(object)TxtMor);
		((Control)paneldays).Controls.Add((Control)(object)labelX8);
		paneldays.DisabledBackColor = Color.Empty;
		((Control)paneldays).Location = new Point(8, 12);
		((Control)paneldays).Name = "paneldays";
		((Control)paneldays).Size = new Size(311, 110);
		paneldays.Style.Alignment = (StringAlignment)1;
		paneldays.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		paneldays.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		paneldays.Style.Border = (eBorderType)1;
		paneldays.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		paneldays.Style.BorderWidth = 2;
		paneldays.Style.CornerType = (eCornerType)2;
		paneldays.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		paneldays.Style.GradientAngle = 90;
		((Control)paneldays).TabIndex = 0;
		((Control)panel1).Anchor = (AnchorStyles)15;
		((Control)panel1).Controls.Add((Control)(object)ChbMal);
		((Control)panel1).Controls.Add((Control)(object)ChbBime);
		((Control)panel1).Controls.Add((Control)(object)ChbEzaf);
		((Control)panel1).Controls.Add((Control)(object)ChbKosor);
		((Control)panel1).Controls.Add((Control)(object)ChbHoghogh);
		((Control)panel1).Location = new Point(2, 43);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(307, 65);
		((Control)panel1).TabIndex = 2;
		((Control)ChbMal).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbMal).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbMal).Location = new Point(3, 3);
		((Control)ChbMal).Name = "ChbMal";
		((Control)ChbMal).Size = new Size(61, 23);
		((BaseItemControl)ChbMal).Style = (eDotNetBarStyle)9;
		((Control)ChbMal).TabIndex = 2;
		((Control)ChbMal).Text = "[مالیات]";
		((Control)ChbBime).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbBime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbBime).Location = new Point(65, 3);
		((Control)ChbBime).Name = "ChbBime";
		((Control)ChbBime).Size = new Size(100, 23);
		((BaseItemControl)ChbBime).Style = (eDotNetBarStyle)9;
		((Control)ChbBime).TabIndex = 1;
		((Control)ChbBime).Text = "[بیمه]";
		((Control)ChbEzaf).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbEzaf).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbEzaf).Location = new Point(195, 32);
		((Control)ChbEzaf).Name = "ChbEzaf";
		((Control)ChbEzaf).Size = new Size(100, 23);
		((BaseItemControl)ChbEzaf).Style = (eDotNetBarStyle)9;
		((Control)ChbEzaf).TabIndex = 3;
		((Control)ChbEzaf).Text = "[اضافات]";
		((Control)ChbKosor).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbKosor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbKosor).Location = new Point(65, 35);
		((Control)ChbKosor).Name = "ChbKosor";
		((Control)ChbKosor).Size = new Size(100, 23);
		((BaseItemControl)ChbKosor).Style = (eDotNetBarStyle)9;
		((Control)ChbKosor).TabIndex = 4;
		((Control)ChbKosor).Text = "[کسورات]";
		((Control)ChbHoghogh).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbHoghogh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbHoghogh).Location = new Point(195, 3);
		((Control)ChbHoghogh).Name = "ChbHoghogh";
		((Control)ChbHoghogh).Size = new Size(100, 23);
		((BaseItemControl)ChbHoghogh).Style = (eDotNetBarStyle)9;
		((Control)ChbHoghogh).TabIndex = 0;
		((Control)ChbHoghogh).Text = "[حقوق]";
		((Control)TxtMor).Anchor = (AnchorStyles)13;
		TxtMor.Border.Class = "TextBoxBorder";
		TxtMor.Border.CornerType = (eCornerType)1;
		((Control)TxtMor).Location = new Point(14, 13);
		((Control)TxtMor).Name = "TxtMor";
		((Control)TxtMor).Size = new Size(187, 21);
		((Control)TxtMor).TabIndex = 1;
		((Control)TxtMor).Enter += TxtMor_Enter;
		((Control)TxtMor).Leave += TxtMor_Leave;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(207, 12);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(99, 23);
		((Control)labelX8).TabIndex = 0;
		((Control)labelX8).Text = "نام نوع مرخصی:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(137, 128);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(121, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(8, 128);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(123, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(326, 174);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(342, 200);
		((Control)this).Name = "DefMorakhasiType";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[انواع مرخصی]";
		((Form)this).Load += DefMorakhasiType_Load;
		((Control)this).KeyDown += new KeyEventHandler(DefMorakhasiType_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)paneldays).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
