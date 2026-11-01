using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms.Codeing;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmAgreedDeduction : FormExtention
{
	private AgreedDeduction _clsAgreedDeduction;

	private Person _clsPerson;

	private string Frm_Text = "[افزودن کسورات توافقی]";

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX2;

	private LabelX labelX4;

	private TextBoxFa TxtAmountPayable;

	private LabelX labelX1;

	private LabelX labelX5;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX LblFullName;

	private ButtonX BtnMorePerson;

	private TextBoxX TxtCodePerson;

	private LabelX labelX7;

	private TextBoxFa TxtMonthlyInstallment;

	internal DateBoxFa TxtDateCreatedSh;

	private LabelX labelX3;

	private TextBoxX TxtDescription;

	private ButtonX BtnMoreHesab;

	private ComboBoxEx TxtHesabTitle;

	private LabelX labelX6;

	private TextBoxFa TxtInstallmentNo;

	private DateBoxFa dateBoxFa1;

	public FrmAgreedDeduction(string contextGuid, Font font, AgreedDeduction item)
		: base(contextGuid, true, true)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			_clsAgreedDeduction = item;
			if (item.Id <= 0)
			{
				Frm_Text = "[افزودن کسورات توافقی]";
			}
			else
			{
				Frm_Text = "[ویرایش کسورات توافقی ]";
			}
			new Waiter("در حال بارگزاری ...", (Form)(object)this, FillFormAsync(new CancellationTokenSource(30000).Token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task FillFormAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await LoadDataHesabAsync(0, token);
			if (_clsAgreedDeduction == null)
			{
				return;
			}
			Person person = await _clsAgreedDeduction.GetClsPersonAsync(token);
			string codeTafsil = ((person != null) ? ((PersonBase)person).CodeTafsil.ToString() : null);
			string fullName = await _clsAgreedDeduction.GetFullNameAsync(token);
			string hesabTitle = await _clsAgreedDeduction.GetHesabTitleAsync(token);
			do
			{
				await Task.Delay(100);
				if (((FormExtention)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((FormExtention)this).IsDisposed)
					{
						((Control)TxtCodePerson).Text = codeTafsil;
						((Control)LblFullName).Text = fullName;
						((Control)TxtHesabTitle).Text = hesabTitle;
						((Control)TxtAmountPayable).Text = _clsAgreedDeduction.AmountPayable.ToString(CultureInfo.InvariantCulture);
						((Control)TxtMonthlyInstallment).Text = _clsAgreedDeduction.MonthlyInstallment.ToString();
						((Control)TxtDateCreatedSh).Text = Calendar.MiladiToCalture(_clsAgreedDeduction.DateCreatedM);
						((Control)TxtDescription).Text = _clsAgreedDeduction.Description;
						if (_clsAgreedDeduction.MonthlyInstallment > 0m)
						{
							((Control)TxtInstallmentNo).Text = Math.Ceiling(_clsAgreedDeduction.AmountPayable / _clsAgreedDeduction.MonthlyInstallment).ToString("0");
						}
					}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataHesabAsync(int id = 0, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<FishHesab> list = (await FishHesab.SelectAllAsync(((CacheBaseForm)this).ContextGuid, getAllField: true, token)).Where((FishHesab q) => q.Type == 2).ToList();
			((ComboBox)TxtHesabTitle).DataSource = list;
			((ListControl)TxtHesabTitle).ValueMember = "Id";
			((ListControl)TxtHesabTitle).DisplayMember = "Name";
			int selectedIndex = list.FindIndex((FishHesab q) => q.Id == id);
			((ListControl)TxtHesabTitle).SelectedIndex = selectedIndex;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 102);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
		base.objsCanEmpty.Add((Control)(object)TxtDescription);
		ReleaseEvents(sw: true);
	}

	private async void TxtcodePerson_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(((Control)TxtCodePerson).Text))
			{
				((Control)TxtCodePerson).Text = "0";
			}
			_clsPerson = await Person.GetByPersonCodeTafsilAsync(((CacheBaseForm)this).ContextGuid, long.Parse(((Control)TxtCodePerson).Text));
			((Control)LblFullName).Text = ((PersonBase)_clsPerson).FullName;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtcodePerson_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 123);
		}
	}

	private async void BtnMorePerson_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSelectPersonel frm = new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, isSelect: true, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_clsPerson = frm.SelectedPerson;
			TextBoxX txtCodePerson = TxtCodePerson;
			Person clsPerson = _clsPerson;
			((Control)txtCodePerson).Text = ((clsPerson != null) ? ((PersonBase)clsPerson).CodeTafsil.ToString() : null);
			LabelX lblFullName = LblFullName;
			Person clsPerson2 = _clsPerson;
			((Control)lblFullName).Text = ((clsPerson2 != null) ? ((PersonBase)clsPerson2).FullName : null);
			((Component)(object)frm).Dispose();
			((Form)frm).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorePerson_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 138);
		}
	}

	private async void BtnMoreHesab_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2);
			if (((ListControl)TxtHesabTitle).SelectedValue != null)
			{
				short.Parse(((ListControl)TxtHesabTitle).SelectedValue.ToString());
			}
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			await LoadDataHesabAsync(frm.SelectedHesab.Id);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreHesab_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 151);
		}
	}

	private void ReleaseEvents(bool sw)
	{
		if (sw)
		{
			((Control)TxtMonthlyInstallment).TextChanged += TxtMonthlyInstallment_TextChanged;
			((Control)TxtInstallmentNo).TextChanged += TxtInstallmentNo_TextChanged;
		}
		else
		{
			((Control)TxtMonthlyInstallment).TextChanged -= TxtMonthlyInstallment_TextChanged;
			((Control)TxtInstallmentNo).TextChanged -= TxtInstallmentNo_TextChanged;
		}
	}

	private void TxtMonthlyInstallment_TextChanged(object sender, EventArgs e)
	{
		ReleaseEvents(sw: false);
		decimal num = Utilities.ParseToDecimal(((Control)TxtAmountPayable).Text, 0m, 0);
		decimal num2 = Utilities.ParseToDecimal(((Control)TxtMonthlyInstallment).Text, 0m, 0);
		if (num2 > 0m)
		{
			decimal d = num / num2;
			((Control)TxtInstallmentNo).Text = Math.Ceiling(d).ToString("0");
		}
		ReleaseEvents(sw: true);
	}

	private void TxtInstallmentNo_TextChanged(object sender, EventArgs e)
	{
		ReleaseEvents(sw: false);
		decimal num = Utilities.ParseToDecimal(((Control)TxtAmountPayable).Text, 0m, 0);
		decimal num2 = Utilities.ParseToDecimal(((Control)TxtInstallmentNo).Text, 0m, 0);
		if (num2 > 0m)
		{
			decimal d = num / num2;
			((Control)TxtMonthlyInstallment).Text = Math.Ceiling(d).ToString("0");
		}
		ReleaseEvents(sw: true);
	}

	private void GetData()
	{
		try
		{
			if (_clsAgreedDeduction == null)
			{
				_clsAgreedDeduction = new AgreedDeduction(((CacheBaseForm)this).ContextGuid);
			}
			if (_clsPerson != null)
			{
				_clsAgreedDeduction.PersonId = ((PersonBase)_clsPerson).Id;
			}
			if (((ListControl)TxtHesabTitle).SelectedValue != null)
			{
				_clsAgreedDeduction.HesabId = Utilities.ParseToInt(((ListControl)TxtHesabTitle).SelectedValue.ToString(), 0);
			}
			_clsAgreedDeduction.AmountPayable = Utilities.ParseToDecimal(((Control)TxtAmountPayable).Text, 0m, 0);
			_clsAgreedDeduction.MonthlyInstallment = Utilities.ParseToDecimal(((Control)TxtMonthlyInstallment).Text, 0m, 0);
			_clsAgreedDeduction.DateCreatedM = TxtDateCreatedSh.MiladiDate;
			_clsAgreedDeduction.Description = ((Control)TxtDescription).Text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			GetData();
			DialogResult val = await AgreedDeductionValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _clsAgreedDeduction, isShowMessage: true);
			if ((int)val == 6 || (int)val == 5)
			{
				((Form)this).DialogResult = (DialogResult)((await _clsAgreedDeduction.SaveAsync()) ? 1 : 2);
				((FormExtention)this).ResetControlChanged();
				((Form)this).Close();
			}
			else if ((int)val == 2)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 232);
		}
	}

	private void FrmAgreedDeduction_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmAgreedDeduction_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeduction.cs", 246);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
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
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Expected O, but got Unknown
		//IL_0c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Expected O, but got Unknown
		//IL_0d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d29: Expected O, but got Unknown
		//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1202: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_1374: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_14c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ce: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		labelX6 = new LabelX();
		TxtInstallmentNo = new TextBoxFa();
		TxtHesabTitle = new ComboBoxEx();
		BtnMoreHesab = new ButtonX();
		labelX3 = new LabelX();
		TxtDescription = new TextBoxX();
		TxtDateCreatedSh = new DateBoxFa();
		LblFullName = new LabelX();
		BtnMorePerson = new ButtonX();
		TxtCodePerson = new TextBoxX();
		labelX7 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		TxtMonthlyInstallment = new TextBoxFa();
		TxtAmountPayable = new TextBoxFa();
		labelX1 = new LabelX();
		labelX2 = new LabelX();
		dateBoxFa1 = new DateBoxFa();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)TxtInstallmentNo);
		((Control)panelEx1).Controls.Add((Control)(object)TxtHesabTitle);
		((Control)panelEx1).Controls.Add((Control)(object)BtnMoreHesab);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDescription);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDateCreatedSh);
		((Control)panelEx1).Controls.Add((Control)(object)LblFullName);
		((Control)panelEx1).Controls.Add((Control)(object)BtnMorePerson);
		((Control)panelEx1).Controls.Add((Control)(object)TxtCodePerson);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)TxtMonthlyInstallment);
		((Control)panelEx1).Controls.Add((Control)(object)TxtAmountPayable);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(433, 314);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(113, 95);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(86, 21);
		((Control)labelX6).TabIndex = 11;
		((Control)labelX6).Text = "[تعداد اقساط] : ";
		labelX6.TextAlignment = (StringAlignment)2;
		TxtInstallmentNo.AllowEmpty = true;
		((Control)TxtInstallmentNo).Anchor = (AnchorStyles)13;
		TxtInstallmentNo.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtInstallmentNo.BackColorTextBoxWhenLostFocus = Color.White;
		TxtInstallmentNo.BackcolorTooltip = Color.Red;
		TxtInstallmentNo.CurrencyFormat = "";
		TxtInstallmentNo.ForeColorTooltip = Color.Black;
		TxtInstallmentNo.GetColoresFromSettingDLL = true;
		TxtInstallmentNo.HandleKeydown = true;
		((Control)TxtInstallmentNo).Location = new Point(12, 95);
		((TextBoxBase)TxtInstallmentNo).MaxLength = 19;
		((Control)TxtInstallmentNo).Name = "TxtInstallmentNo";
		TxtInstallmentNo.SendShiftTabAfterDown = true;
		TxtInstallmentNo.SendShiftTabAfterEnter = true;
		TxtInstallmentNo.SendTabAfterEnter = true;
		TxtInstallmentNo.SendTabAfterUP = true;
		TxtInstallmentNo.ShowHorofi = (ENShowHorofi)3;
		TxtInstallmentNo.ShowToolTipText = true;
		((Control)TxtInstallmentNo).Size = new Size(95, 21);
		((Control)TxtInstallmentNo).TabIndex = 12;
		((Control)TxtInstallmentNo).Text = "0";
		TxtInstallmentNo.ToolTipPostFix = "";
		TxtInstallmentNo.TootTipDelay = 1000;
		TxtInstallmentNo.Value = new decimal(new int[4]);
		((Control)TxtHesabTitle).Anchor = (AnchorStyles)13;
		((ListControl)TxtHesabTitle).DisplayMember = "Text";
		((ComboBox)TxtHesabTitle).DrawMode = (DrawMode)1;
		((ListControl)TxtHesabTitle).FormattingEnabled = true;
		((ComboBox)TxtHesabTitle).ItemHeight = 16;
		((Control)TxtHesabTitle).Location = new Point(49, 43);
		((Control)TxtHesabTitle).Name = "TxtHesabTitle";
		((Control)TxtHesabTitle).Size = new Size(261, 22);
		TxtHesabTitle.Style = (eDotNetBarStyle)9;
		((Control)TxtHesabTitle).TabIndex = 5;
		((Control)BtnMoreHesab).AccessibleRole = (AccessibleRole)43;
		BtnMoreHesab.ColorTable = (eButtonColor)6;
		((Control)BtnMoreHesab).Location = new Point(13, 43);
		((Control)BtnMoreHesab).Name = "BtnMoreHesab";
		((Control)BtnMoreHesab).Size = new Size(30, 21);
		((PopupItemControl)BtnMoreHesab).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreHesab).TabIndex = 6;
		((Control)BtnMoreHesab).TabStop = false;
		((Control)BtnMoreHesab).Text = "...";
		((Control)BtnMoreHesab).Click += BtnMoreHesab_Click;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(316, 154);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(86, 19);
		((Control)labelX3).TabIndex = 15;
		((Control)labelX3).Text = "[شرح] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)TxtDescription).Anchor = (AnchorStyles)15;
		TxtDescription.Border.Class = "TextBoxBorder";
		TxtDescription.Border.CornerType = (eCornerType)1;
		((Control)TxtDescription).Location = new Point(13, 154);
		((TextBoxBase)TxtDescription).Multiline = true;
		((Control)TxtDescription).Name = "TxtDescription";
		((Control)TxtDescription).Size = new Size(297, 90);
		((Control)TxtDescription).TabIndex = 16;
		TxtDateCreatedSh.AllowNull = false;
		((Control)TxtDateCreatedSh).Anchor = (AnchorStyles)13;
		TxtDateCreatedSh.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtDateCreatedSh).Border.CornerType = (eCornerType)1;
		TxtDateCreatedSh.EnterToTab = true;
		TxtDateCreatedSh.EscToClose = true;
		TxtDateCreatedSh.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtDateCreatedSh.FocusBackColor = Color.Yellow;
		TxtDateCreatedSh.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtDateCreatedSh.FocusForeColor = Color.Blue;
		TxtDateCreatedSh.FocusTextSelect = true;
		((Control)TxtDateCreatedSh).Font = new Font("Times New Roman", 12f);
		((Control)TxtDateCreatedSh).Location = new Point(215, 122);
		TxtDateCreatedSh.MessageEmptyShowDialog = false;
		TxtDateCreatedSh.MessegeEmpty = "";
		TxtDateCreatedSh.MessegeEmptyInFormRight = true;
		TxtDateCreatedSh.MessegeEmptyShowInForm = false;
		TxtDateCreatedSh.MiladiDate = new DateTime(2021, 12, 19, 5, 13, 50, 723);
		((Control)TxtDateCreatedSh).Name = "TxtDateCreatedSh";
		((Control)TxtDateCreatedSh).RightToLeft = (RightToLeft)0;
		((Control)TxtDateCreatedSh).Size = new Size(95, 20);
		((Control)TxtDateCreatedSh).TabIndex = 14;
		((Control)TxtDateCreatedSh).Text = "18/12/2021";
		((TextBox)TxtDateCreatedSh).TextAlign = (HorizontalAlignment)2;
		TxtDateCreatedSh.TootTipDelay = 500;
		((Control)LblFullName).Anchor = (AnchorStyles)13;
		((Control)LblFullName).BackColor = Color.Transparent;
		((BaseItemControl)LblFullName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblFullName).ForeColor = Color.DarkRed;
		((Control)LblFullName).Location = new Point(12, 12);
		((Control)LblFullName).Name = "LblFullName";
		((Control)LblFullName).RightToLeft = (RightToLeft)1;
		((Control)LblFullName).Size = new Size(197, 21);
		((Control)LblFullName).TabIndex = 3;
		((Control)LblFullName).Text = "----------";
		((Control)BtnMorePerson).AccessibleRole = (AccessibleRole)43;
		((Control)BtnMorePerson).Anchor = (AnchorStyles)9;
		BtnMorePerson.ColorTable = (eButtonColor)6;
		((Control)BtnMorePerson).Location = new Point(215, 12);
		((Control)BtnMorePerson).Name = "BtnMorePerson";
		((Control)BtnMorePerson).Size = new Size(30, 21);
		((PopupItemControl)BtnMorePerson).Style = (eDotNetBarStyle)9;
		((Control)BtnMorePerson).TabIndex = 2;
		((Control)BtnMorePerson).TabStop = false;
		((Control)BtnMorePerson).Text = "...";
		((Control)BtnMorePerson).Click += BtnMorePerson_Click;
		((Control)TxtCodePerson).Anchor = (AnchorStyles)9;
		TxtCodePerson.Border.Class = "TextBoxBorder";
		TxtCodePerson.Border.CornerType = (eCornerType)1;
		((Control)TxtCodePerson).Location = new Point(251, 12);
		((Control)TxtCodePerson).Name = "TxtCodePerson";
		((Control)TxtCodePerson).Size = new Size(59, 21);
		((Control)TxtCodePerson).TabIndex = 1;
		((Control)TxtCodePerson).TextChanged += TxtcodePerson_TextChanged;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(316, 12);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(86, 21);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[کد پرسنلی] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 250);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 18;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(135, 250);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 17;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(316, 122);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(86, 26);
		((Control)labelX5).TabIndex = 13;
		((Control)labelX5).Text = "[تاریخ شروع] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(316, 95);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(86, 21);
		((Control)labelX4).TabIndex = 9;
		((Control)labelX4).Text = "[قسط ماهیانه] :";
		labelX4.TextAlignment = (StringAlignment)2;
		TxtMonthlyInstallment.AllowEmpty = true;
		((Control)TxtMonthlyInstallment).Anchor = (AnchorStyles)13;
		TxtMonthlyInstallment.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtMonthlyInstallment.BackColorTextBoxWhenLostFocus = Color.White;
		TxtMonthlyInstallment.BackcolorTooltip = Color.Red;
		TxtMonthlyInstallment.CurrencyFormat = "";
		TxtMonthlyInstallment.ForeColorTooltip = Color.Black;
		TxtMonthlyInstallment.GetColoresFromSettingDLL = true;
		TxtMonthlyInstallment.HandleKeydown = true;
		((Control)TxtMonthlyInstallment).Location = new Point(215, 95);
		((TextBoxBase)TxtMonthlyInstallment).MaxLength = 19;
		((Control)TxtMonthlyInstallment).Name = "TxtMonthlyInstallment";
		TxtMonthlyInstallment.SendShiftTabAfterDown = true;
		TxtMonthlyInstallment.SendShiftTabAfterEnter = true;
		TxtMonthlyInstallment.SendTabAfterEnter = true;
		TxtMonthlyInstallment.SendTabAfterUP = true;
		TxtMonthlyInstallment.ShowHorofi = (ENShowHorofi)3;
		TxtMonthlyInstallment.ShowToolTipText = true;
		((Control)TxtMonthlyInstallment).Size = new Size(95, 21);
		((Control)TxtMonthlyInstallment).TabIndex = 10;
		((Control)TxtMonthlyInstallment).Text = "0";
		TxtMonthlyInstallment.ToolTipPostFix = "";
		TxtMonthlyInstallment.TootTipDelay = 1000;
		TxtMonthlyInstallment.Value = new decimal(new int[4]);
		TxtAmountPayable.AllowEmpty = true;
		((Control)TxtAmountPayable).Anchor = (AnchorStyles)13;
		TxtAmountPayable.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtAmountPayable.BackColorTextBoxWhenLostFocus = Color.White;
		TxtAmountPayable.BackcolorTooltip = Color.Red;
		TxtAmountPayable.CurrencyFormat = "";
		TxtAmountPayable.ForeColorTooltip = Color.Black;
		TxtAmountPayable.GetColoresFromSettingDLL = true;
		TxtAmountPayable.HandleKeydown = true;
		((Control)TxtAmountPayable).Location = new Point(215, 68);
		((TextBoxBase)TxtAmountPayable).MaxLength = 19;
		((Control)TxtAmountPayable).Name = "TxtAmountPayable";
		TxtAmountPayable.SendShiftTabAfterDown = true;
		TxtAmountPayable.SendShiftTabAfterEnter = true;
		TxtAmountPayable.SendTabAfterEnter = true;
		TxtAmountPayable.SendTabAfterUP = true;
		TxtAmountPayable.ShowHorofi = (ENShowHorofi)3;
		TxtAmountPayable.ShowToolTipText = true;
		((Control)TxtAmountPayable).Size = new Size(95, 21);
		((Control)TxtAmountPayable).TabIndex = 8;
		((Control)TxtAmountPayable).Text = "0";
		TxtAmountPayable.ToolTipPostFix = "";
		TxtAmountPayable.TootTipDelay = 1000;
		TxtAmountPayable.Value = new decimal(new int[4]);
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(316, 68);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(106, 21);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "[مبلغ قابل پرداخت] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(316, 43);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(86, 21);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[نام حساب] :";
		labelX2.TextAlignment = (StringAlignment)2;
		dateBoxFa1.AllowNull = false;
		dateBoxFa1.BackcolorTooltip = Color.Red;
		((TextBoxX)dateBoxFa1).Border.Class = "TextBoxBorder";
		((TextBoxX)dateBoxFa1).Border.CornerType = (eCornerType)1;
		dateBoxFa1.EnterToTab = true;
		dateBoxFa1.EscToClose = true;
		dateBoxFa1.FarsyDateStyle = (ENFarsyDateStyle)1;
		dateBoxFa1.FocusBackColor = Color.Yellow;
		dateBoxFa1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dateBoxFa1.FocusForeColor = Color.Blue;
		dateBoxFa1.FocusTextSelect = true;
		((Control)dateBoxFa1).Location = new Point(316, 193);
		dateBoxFa1.MessageEmptyShowDialog = false;
		dateBoxFa1.MessegeEmpty = "";
		dateBoxFa1.MessegeEmptyInFormRight = true;
		dateBoxFa1.MessegeEmptyShowInForm = false;
		dateBoxFa1.MiladiDate = new DateTime(2021, 12, 19, 5, 13, 50, 792);
		((Control)dateBoxFa1).Name = "dateBoxFa1";
		((Control)dateBoxFa1).RightToLeft = (RightToLeft)0;
		((Control)dateBoxFa1).Size = new Size(100, 26);
		((Control)dateBoxFa1).TabIndex = 19;
		((Control)dateBoxFa1).Text = "18/12/2021";
		((TextBox)dateBoxFa1).TextAlign = (HorizontalAlignment)2;
		dateBoxFa1.TootTipDelay = 2000;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(433, 314);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(449, 353);
		((Control)this).Name = "FrmAgreedDeduction";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]";
		((Form)this).Load += FrmAgreedDeduction_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
