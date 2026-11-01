using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRClub;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubPersonInfo : BaseForm, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private ButtonX BtnSelectPerson;

	private GroupPanel groupPanel7;

	private PictureBox PicPerson;

	private TextBoxX TxtMob;

	private LabelX labelX3;

	private TextBoxX TxtFamily;

	private TextBoxX TxtName;

	private LabelX labelX1;

	private LabelX labelX2;

	private ButtonX BtnAddNewPerson;

	public FrmClubPersonInfo(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_club = club;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 28);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((BaseForm)this).OnLoad(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 40);
		}
	}

	public void FillFrmFields()
	{
		try
		{
			if (_club.ClsPerson != null)
			{
				((Control)TxtName).Text = ((PersonBase)_club.ClsPerson).Name;
				((Control)TxtFamily).Text = ((PersonBase)_club.ClsPerson).Family;
				((Control)TxtMob).Text = ((PersonBase)_club.ClsPerson).Mob1;
				if (File.Exists(((PersonBase)_club.ClsPerson).PicPath))
				{
					PicPerson.SizeMode = (PictureBoxSizeMode)1;
					PicPerson.Image = Image.FromFile(((PersonBase)_club.ClsPerson).PicPath);
				}
				else
				{
					PicPerson.Image = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFrmFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 64);
		}
	}

	public IClubForms NextPage()
	{
		try
		{
			return _nextPage ?? (_nextPage = new FrmClubContractType(((CacheBaseForm)this).ContextGuid, _club));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 73);
		}
		return null;
	}

	private async void BtnSelectPerson_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSelectPersonel frm = new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, isSelect: true, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_club.ClsPerson = frm.SelectedPerson;
			((Form)frm).Close();
			FillFrmFields();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectPerson_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 87);
		}
	}

	private async void BtnAddNewPerson_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceDAL.Dal.FLRPerson.Person person = await TimeAttendanceDAL.Dal.FLRPerson.Person.FactoryAsync(((CacheBaseForm)this).ContextGuid);
			FrmPersonel frm = new FrmPersonel(((CacheBaseForm)this).ContextGuid, person, _font, isReadOnly: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_club.ClsPerson = frm.ClsPerson;
			((Form)frm).Close();
			FillFrmFields();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAddNewPerson_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubPersonInfo.cs", 101);
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
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		BtnAddNewPerson = new ButtonX();
		groupPanel7 = new GroupPanel();
		PicPerson = new PictureBox();
		TxtMob = new TextBoxX();
		labelX3 = new LabelX();
		TxtFamily = new TextBoxX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		labelX2 = new LabelX();
		BtnSelectPerson = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)PicPerson).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)BtnAddNewPerson);
		((Control)groupPanel4).Controls.Add((Control)(object)groupPanel7);
		((Control)groupPanel4).Controls.Add((Control)(object)BtnSelectPerson);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 500);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "فرم اطلاعات شخصی عضو باشگاه";
		((Control)BtnAddNewPerson).AccessibleRole = (AccessibleRole)43;
		BtnAddNewPerson.ColorTable = (eButtonColor)6;
		((Control)BtnAddNewPerson).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAddNewPerson.ImagePosition = (eImagePosition)1;
		((Control)BtnAddNewPerson).Location = new Point(422, 23);
		((Control)BtnAddNewPerson).Name = "BtnAddNewPerson";
		((Control)BtnAddNewPerson).RightToLeft = (RightToLeft)1;
		BtnAddNewPerson.Shortcuts.Add((eShortcut)116);
		((Control)BtnAddNewPerson).Size = new Size(146, 32);
		((PopupItemControl)BtnAddNewPerson).Style = (eDotNetBarStyle)9;
		((Control)BtnAddNewPerson).TabIndex = 16;
		((Control)BtnAddNewPerson).Text = "ثبت نام عضو جدید";
		((Control)BtnAddNewPerson).Click += BtnAddNewPerson_Click;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)PicPerson);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtMob);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtFamily);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtName);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(9, 61);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(559, 398);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 15;
		PicPerson.BorderStyle = (BorderStyle)1;
		PicPerson.Image = (Image)(object)Resources.people;
		((Control)PicPerson).Location = new Point(14, 24);
		((Control)PicPerson).Name = "PicPerson";
		((Control)PicPerson).Size = new Size(182, 198);
		PicPerson.SizeMode = (PictureBoxSizeMode)1;
		PicPerson.TabIndex = 28;
		PicPerson.TabStop = false;
		TxtMob.Border.Class = "TextBoxBorder";
		TxtMob.Border.CornerType = (eCornerType)1;
		((Control)TxtMob).Location = new Point(256, 115);
		((TextBoxBase)TxtMob).MaxLength = 11;
		((Control)TxtMob).Name = "TxtMob";
		((TextBoxBase)TxtMob).ReadOnly = true;
		((Control)TxtMob).Size = new Size(154, 21);
		((Control)TxtMob).TabIndex = 15;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(417, 115);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(98, 21);
		((Control)labelX3).TabIndex = 14;
		((Control)labelX3).Text = "[موبایل]:";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtFamily.Border.Class = "TextBoxBorder";
		TxtFamily.Border.CornerType = (eCornerType)1;
		((Control)TxtFamily).Location = new Point(256, 69);
		((TextBoxBase)TxtFamily).MaxLength = 50;
		((Control)TxtFamily).Name = "TxtFamily";
		((TextBoxBase)TxtFamily).ReadOnly = true;
		((Control)TxtFamily).Size = new Size(154, 21);
		((Control)TxtFamily).TabIndex = 13;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(256, 23);
		((TextBoxBase)TxtName).MaxLength = 50;
		((Control)TxtName).Name = "TxtName";
		((TextBoxBase)TxtName).ReadOnly = true;
		((Control)TxtName).Size = new Size(154, 21);
		((Control)TxtName).TabIndex = 11;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(417, 23);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(98, 21);
		((Control)labelX1).TabIndex = 10;
		((Control)labelX1).Text = "[نام] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(417, 69);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(125, 21);
		((Control)labelX2).TabIndex = 12;
		((Control)labelX2).Text = "[نام و نام خانوادگی] *  :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)BtnSelectPerson).AccessibleRole = (AccessibleRole)43;
		BtnSelectPerson.ColorTable = (eButtonColor)6;
		((Control)BtnSelectPerson).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnSelectPerson.ImagePosition = (eImagePosition)1;
		((Control)BtnSelectPerson).Location = new Point(9, 23);
		((Control)BtnSelectPerson).Name = "BtnSelectPerson";
		((Control)BtnSelectPerson).RightToLeft = (RightToLeft)1;
		BtnSelectPerson.Shortcuts.Add((eShortcut)116);
		((Control)BtnSelectPerson).Size = new Size(146, 32);
		((PopupItemControl)BtnSelectPerson).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectPerson).TabIndex = 13;
		((Control)BtnSelectPerson).Text = "انتخاب عضو باشگاه";
		((Control)BtnSelectPerson).Click += BtnSelectPerson_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubPersonInfo";
		((Control)this).Text = "FrmCheckOutWellcome";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((ISupportInitialize)PicPerson).EndInit();
		((Control)this).ResumeLayout(false);
	}

	bool IClubForms.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IClubForms.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IClubForms.Show()
	{
		((Control)this).Show();
	}
}
