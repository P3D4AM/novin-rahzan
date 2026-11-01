using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using NPResourceManagement;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCreateShift : BaseForm
{
	private Font _font;

	private Form[] _lstfrm;

	private int _top = -1;

	private readonly int _count;

	private string Frm_Text = "[تعریف شیفت]";

	private IContainer components;

	public PanelEx PanelEx1;

	private Panel pnlLeft;

	private Panel pnlBottom;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel pnlContent;

	private ButtonX btnBack;

	private PictureBox pictureBox1;

	private ButtonX btnCancel;

	private ButtonX btnNext;

	private ButtonX BtnAccept;

	public Shifts ClsShift { get; }

	public FrmCreateShift(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_font = font;
		_lstfrm = (Form[])(object)new Form[5]
		{
			(Form)new FrmCrtShift1(((CacheBaseForm)this).ContextGuid),
			(Form)new FrmCrtShiftNormal(((CacheBaseForm)this).ContextGuid, _font),
			(Form)new FrmCrtShiftCircle1(((CacheBaseForm)this).ContextGuid),
			(Form)new FrmCrtShiftCircle2(((CacheBaseForm)this).ContextGuid, _font),
			(Form)new FrmCrtShiftShenavar(((CacheBaseForm)this).ContextGuid, _font)
		};
		try
		{
			ClsShift = new Shifts(((CacheBaseForm)this).ContextGuid)
			{
				PackageType = (PackageEnumType)1,
				Periodic = 0
			};
			InitializeComponent();
			_count = _lstfrm.Count();
			Frm_Text = "[تعریف شیفت]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 47);
		}
	}

	private void LoadNewForm()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (pnlContent == null)
			{
				return;
			}
			if (_lstfrm.Count() <= 0 || _top > _lstfrm.Count() - 1)
			{
				MessageBox.Show("بارگزاری فرم با خطا مواجه شد . مجدد تلاش نمائید");
				return;
			}
			_lstfrm[_top].TopLevel = false;
			((ScrollableControl)_lstfrm[_top]).AutoScroll = true;
			((Control)pnlContent).Controls.Clear();
			((Control)pnlContent).Controls.Add((Control)(object)_lstfrm[_top]);
			if (_top == 3)
			{
				if (_lstfrm.Count() < 4)
				{
					MessageBox.Show("بارگزاری صفحه بعد با خطا مواجه شد ");
					return;
				}
				FrmCrtShiftCircle1 frmCrtShiftCircle = (FrmCrtShiftCircle1)(object)_lstfrm[2];
				((FrmCrtShiftCircle2)(object)_lstfrm[3]).Init(frmCrtShiftCircle.DayofCircle, frmCrtShiftCircle.DStartDate);
			}
			((Control)_lstfrm[_top]).Show();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadNewForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 77);
		}
	}

	private void Back()
	{
		try
		{
			_top--;
			if (_top == 1 || _top == 3)
			{
				_top = 0;
			}
			if (_top > -1)
			{
				((Control)btnBack).Enabled = true;
				((Control)btnNext).Enabled = true;
				((Control)BtnAccept).Enabled = false;
				LoadNewForm();
				if (_top - 1 <= -1)
				{
					((Control)btnBack).Enabled = false;
				}
				if (_top >= _count)
				{
					((Control)btnNext).Enabled = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Back", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 98);
		}
	}

	private void Next(int ptop = -2)
	{
		try
		{
			if (ptop >= 0)
			{
				_top = ptop;
			}
			else
			{
				_top++;
			}
			if (_top < _count)
			{
				((Control)btnBack).Enabled = true;
				((Control)btnNext).Enabled = true;
				((Control)BtnAccept).Enabled = false;
				LoadNewForm();
				if (_top + 1 == _count || _top == 1 || _top == 3)
				{
					((Control)btnNext).Enabled = false;
					((Control)BtnAccept).Enabled = true;
				}
				if (_top <= 0)
				{
					((Control)btnBack).Enabled = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Next", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 120);
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		try
		{
			switch (_top)
			{
			case 0:
			{
				TypeShiftEnumType typeShift = ((FrmCrtShift1)(object)_lstfrm[_top]).TypeShift;
				if ((int)typeShift != 1)
				{
					if ((int)typeShift == 2)
					{
						Next(4);
					}
					else
					{
						Next(2);
					}
				}
				else
				{
					Next();
				}
				break;
			}
			case 2:
				Next();
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnNext_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 151);
		}
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		Back();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void FrmCreateShift_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			Next();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmCreateShift_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 164);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			await GetDataAsync();
			switch (await ShiftsCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, ClsShift, isShowMessage: true) - 2)
			{
			case 2:
				if (ClsShift.LstShiftDet.Count > 10 && (ClsShift.Name == "" || ClsShift.Description == ""))
				{
					Utility.Show(new ReturnedSaveFuncInfo((ReturnedState)2, "[آیتمهای جزئیات شیفت فاقد عنوان و توضیحات هستند لذا امکان ادامه کار وجود ندارد.\r\n لطفا مجددا شیفت را ایجاد نمائید]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 177), ((CacheBaseForm)this).ContextGuid);
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
				break;
			case 3:
			case 4:
				((Form)this).DialogResult = (DialogResult)((await ClsShift.SaveAsync()) ? 1 : 2);
				((Form)this).Close();
				break;
			case 0:
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
				break;
			case 1:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 192);
		}
	}

	private async Task GetDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			ClsShift.Name = ((FrmCrtShift1)(object)_lstfrm[0]).StrShiftName;
			ClsShift.Code = ((FrmCrtShift1)(object)_lstfrm[0]).StrShiftCode;
			TypeShiftEnumType typeShift = ((FrmCrtShift1)(object)_lstfrm[0]).TypeShift;
			if ((int)typeShift == 1)
			{
				ClsShift.StTatili = ((FrmCrtShiftNormal)(object)_lstfrm[1]).WorkOnHaliday;
			}
			else
			{
				ClsShift.StTatili = (((int)typeShift == 100) ? ((FrmCrtShiftCircle1)(object)_lstfrm[2]).WorkOnHaliday : ((FrmCrtShiftShenavar)(object)_lstfrm[4]).WorkOnHaliday);
			}
			ClsShift.Description = ((FrmCrtShift1)(object)_lstfrm[0]).StrShiftDesc;
			if ((int)typeShift != 1)
			{
				if ((int)typeShift != 2)
				{
					await GetDataShiftCircleAsync(token);
					return;
				}
				ShiftDet data = ((FrmCrtShiftShenavar)(object)_lstfrm[4]).GetData();
				ClsShift.LstShiftDet.Add(data);
			}
			else
			{
				ShiftDet data = ((FrmCrtShiftNormal)(object)_lstfrm[1]).ReadFormData();
				ClsShift.LstShiftDet.Add(data);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 227);
		}
	}

	private async Task<bool> GetDataShiftCircleAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			ShiftDet xcls = new ShiftDet(((CacheBaseForm)this).ContextGuid);
			FrmCrtShiftCircle1 frm1 = (FrmCrtShiftCircle1)(object)_lstfrm[2];
			FrmCrtShiftCircle2 frm2 = (FrmCrtShiftCircle2)(object)_lstfrm[3];
			xcls.Description = Text.FixString(frm1.Desc);
			DateTime xAzDate = frm1.DStartDate.Date;
			int xxxx = frm1.DFinishDate.Date.AddDays(1.0).AddSeconds(-1.0).Subtract(xAzDate)
				.Days;
			foreach (short item in frm2.GetLstShiftId())
			{
				await xcls.SetShiftCodingIdAsync(item);
				DateTime dateStartM = (xcls.DateFinishM = xAzDate);
				xcls.DateStartM = dateStartM;
				int i = 0;
				while (i < xxxx)
				{
					bool flag2 = (xcls.Fri = false);
					bool flag4 = (xcls.Tur = flag2);
					bool flag6 = (xcls.Wed = flag4);
					bool flag8 = (xcls.Tue = flag6);
					bool flag10 = (xcls.Mon = flag8);
					bool sat = (xcls.Sun = flag10);
					xcls.Sat = sat;
					bool flag12 = false;
					int num = 0;
					while (num < 7 && i < xxxx)
					{
						if (xcls.ShiftCodingId == frm2.GetShiftCodingId(i % frm1.DayofCircle))
						{
							switch (xcls.DateFinishM.AddDays(num).DayOfWeek)
							{
							case DayOfWeek.Sunday:
								sat = (xcls.Sun = true);
								flag12 = sat;
								break;
							case DayOfWeek.Monday:
								sat = (xcls.Mon = true);
								flag12 = sat;
								break;
							case DayOfWeek.Tuesday:
								sat = (xcls.Tue = true);
								flag12 = sat;
								break;
							case DayOfWeek.Wednesday:
								sat = (xcls.Wed = true);
								flag12 = sat;
								break;
							case DayOfWeek.Thursday:
								sat = (xcls.Tur = true);
								flag12 = sat;
								break;
							case DayOfWeek.Friday:
								sat = (xcls.Fri = true);
								flag12 = sat;
								xcls.FriType = frm1.FriDayType;
								break;
							case DayOfWeek.Saturday:
								sat = (xcls.Sat = true);
								flag12 = sat;
								break;
							}
						}
						else if (frm2.GetShiftCodingId(i % frm1.DayofCircle) != 0 && xcls.ShiftCodingId == frm2.GetShiftCodingId(i % frm1.DayofCircle))
						{
							break;
						}
						num++;
						i++;
					}
					xcls.DateFinishM = xcls.DateFinishM.AddDays(num).AddSeconds(-1.0);
					if (flag12 && xcls.ShiftCodingId > 0)
					{
						List<ShiftDet> lstShiftDet = ClsShift.LstShiftDet;
						lstShiftDet.Add(await ShiftDet.Clone_ShiftDetAysnc(((CacheBaseForm)this).ContextGuid, xcls, token));
					}
					dateStartM = (xcls.DateFinishM = xcls.DateFinishM.AddSeconds(1.0));
					xcls.DateStartM = dateStartM;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDataShiftCircleAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 278);
		}
		return ClsShift.LstShiftDet.Count > 0;
	}

	private void FrmCreateShift_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((PopupItemControl)btnCancel).PerformClick();
			}
			if ((int)e.KeyCode == 116)
			{
				if (((Control)btnNext).Enabled)
				{
					((PopupItemControl)btnNext).PerformClick();
				}
				else if (((Control)BtnAccept).Enabled)
				{
					((PopupItemControl)BtnAccept).PerformClick();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmCreateShift_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCreateShift.cs", 296);
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
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Expected O, but got Unknown
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		pnlContent = new Panel();
		pnlLeft = new Panel();
		pictureBox1 = new PictureBox();
		pnlBottom = new Panel();
		tableLayoutPanel1 = new TableLayoutPanel();
		btnCancel = new ButtonX();
		btnBack = new ButtonX();
		btnNext = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((Control)pnlLeft).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)pnlBottom).SuspendLayout();
		((Control)tableLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pnlContent);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlLeft);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlBottom);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(2);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(915, 562);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 1;
		((Control)pnlContent).Anchor = (AnchorStyles)15;
		((Control)pnlContent).Location = new Point(0, 0);
		((Control)pnlContent).Margin = new Padding(2);
		((Control)pnlContent).Name = "pnlContent";
		((Control)pnlContent).Padding = new Padding(8, 6, 8, 6);
		((Control)pnlContent).Size = new Size(582, 500);
		((Control)pnlContent).TabIndex = 10;
		((Control)pnlLeft).BackColor = Color.FromArgb(255, 153, 0);
		((Control)pnlLeft).Controls.Add((Control)(object)pictureBox1);
		((Control)pnlLeft).Dock = (DockStyle)4;
		((Control)pnlLeft).Location = new Point(587, 0);
		((Control)pnlLeft).Margin = new Padding(2);
		((Control)pnlLeft).Name = "pnlLeft";
		((Control)pnlLeft).Size = new Size(328, 504);
		((Control)pnlLeft).TabIndex = 9;
		((Control)pictureBox1).BackColor = Color.FromArgb(0, 0, 0, 0);
		((Control)pictureBox1).Dock = (DockStyle)1;
		pictureBox1.Image = (Image)(object)ImageResourceManagement.Devices;
		pictureBox1.InitialImage = (Image)(object)Resources.blue_package;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Margin = new Padding(2);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(328, 504);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)pnlBottom).BackColor = SystemColors.Control;
		((Control)pnlBottom).Controls.Add((Control)(object)tableLayoutPanel1);
		((Control)pnlBottom).Dock = (DockStyle)2;
		((Control)pnlBottom).Location = new Point(0, 504);
		((Control)pnlBottom).Margin = new Padding(2);
		((Control)pnlBottom).Name = "pnlBottom";
		((Control)pnlBottom).Padding = new Padding(8, 6, 8, 6);
		((Control)pnlBottom).Size = new Size(915, 58);
		((Control)pnlBottom).TabIndex = 8;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.Controls.Add((Control)(object)btnCancel, 3, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnBack, 2, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnNext, 1, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)BtnAccept, 0, 0);
		((Control)tableLayoutPanel1).Dock = (DockStyle)3;
		((Control)tableLayoutPanel1).Location = new Point(8, 6);
		((Control)tableLayoutPanel1).Margin = new Padding(2);
		((Control)tableLayoutPanel1).Name = "tableLayoutPanel1";
		((Control)tableLayoutPanel1).Padding = new Padding(2);
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 100f));
		((Control)tableLayoutPanel1).Size = new Size(513, 46);
		((Control)tableLayoutPanel1).TabIndex = 0;
		((Control)btnCancel).AccessibleRole = (AccessibleRole)43;
		btnCancel.ColorTable = (eButtonColor)6;
		((Control)btnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnCancel).ForeColor = Color.Black;
		btnCancel.Image = (Image)(object)Resources.stop;
		btnCancel.ImagePosition = (eImagePosition)1;
		((Control)btnCancel).Location = new Point(11, 4);
		((Control)btnCancel).Margin = new Padding(2);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(122, 38);
		((PopupItemControl)btnCancel).Style = (eDotNetBarStyle)9;
		((Control)btnCancel).TabIndex = 4;
		((Control)btnCancel).TabStop = false;
		((Control)btnCancel).Text = "[لغو]   (Esc)";
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)btnBack).AccessibleRole = (AccessibleRole)43;
		btnBack.ColorTable = (eButtonColor)6;
		((Control)btnBack).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnBack).ForeColor = Color.Black;
		btnBack.Image = (Image)(object)Resources.next2;
		btnBack.ImagePosition = (eImagePosition)1;
		((Control)btnBack).Location = new Point(137, 4);
		((Control)btnBack).Margin = new Padding(2);
		((Control)btnBack).Name = "btnBack";
		((Control)btnBack).RightToLeft = (RightToLeft)1;
		((Control)btnBack).Size = new Size(121, 38);
		((PopupItemControl)btnBack).Style = (eDotNetBarStyle)9;
		((Control)btnBack).TabIndex = 4;
		((Control)btnBack).TabStop = false;
		((Control)btnBack).Text = "[قبلی]";
		((Control)btnBack).Click += btnBack_Click;
		((Control)btnNext).AccessibleRole = (AccessibleRole)43;
		btnNext.ColorTable = (eButtonColor)6;
		((Control)btnNext).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnNext).ForeColor = Color.Black;
		btnNext.Image = (Image)(object)Resources.back2;
		btnNext.ImagePosition = (eImagePosition)1;
		((Control)btnNext).Location = new Point(262, 4);
		((Control)btnNext).Margin = new Padding(2);
		((Control)btnNext).Name = "btnNext";
		((Control)btnNext).RightToLeft = (RightToLeft)1;
		((Control)btnNext).Size = new Size(122, 38);
		((PopupItemControl)btnNext).Style = (eDotNetBarStyle)9;
		((Control)btnNext).TabIndex = 4;
		((Control)btnNext).TabStop = false;
		((Control)btnNext).Text = "[بعدی]";
		((Control)btnNext).Click += btnNext_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(388, 4);
		((Control)BtnAccept).Margin = new Padding(2);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(121, 38);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 3;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(915, 562);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmCreateShift";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FrmCreateShift";
		((Form)this).Load += FrmCreateShift_Load;
		((Control)this).KeyDown += new KeyEventHandler(FrmCreateShift_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)pnlLeft).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)pnlBottom).ResumeLayout(false);
		((Control)tableLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
