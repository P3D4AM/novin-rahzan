using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
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
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCrtShiftNormal : BaseForm
{
	private Font _font;

	private readonly ShiftDet _clsShiftDet;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private CancellationTokenSource translateTokenSource;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel7;

	internal DateBoxFa TxtFinishM;

	internal DateBoxFa TXTStartDate;

	private LabelX labelX1;

	private TextBoxX TxtDesc;

	private GroupPanel groupPanelTaradod;

	private Panel PanelNormal;

	private LabelX LblNextDay;

	private LabelX LblValEndTime;

	private LabelX LblValStartTime;

	private LabelX LblEndTime;

	private LabelX LblStartTime;

	private LabelX labelX39;

	internal ButtonX BtnSelectShiftCoding;

	private ComboBoxEx CmbShiftCoding;

	private CheckBoxX ChbSat;

	private CheckBoxX ChbMon;

	private CheckBoxX ChbSun;

	private CheckBoxX ChbThr;

	private CheckBoxX ChbTue;

	private CheckBoxX ChbWed;

	private LabelX labelX3;

	private LabelX labelX2;

	private ToolTip toolTip1;

	private LabelX LblDescPeakOverTime;

	private LabelX LblTajilStartTime;

	private LabelX LblPeakOvertime;

	private LabelX LblTajilEndTime;

	private LabelX lblTakhirKh;

	private LabelX LblTakhirStartTime;

	private LabelX LblTakhirEndTime;

	private LabelX LblEndTaTime;

	private LabelX labelX8;

	private LabelX labelX16;

	private LabelX labelX10;

	private LabelX labelX4;

	private LabelX lblTajilKh;

	private LabelX labelX11;

	private LabelX LblStartTaTime;

	private LabelX LblEndAzTime;

	private LabelX LblStartAzTime;

	private LabelX labelX20;

	private LabelX labelX13;

	private CheckBoxX ChbSTTatili;

	private RadioButton RbOptionalNotKarkerd;

	private RadioButton RbOptionalKarkerd;

	private LabelX labelX5;

	private RadioButton RbMustWork;

	internal ButtonX BtnHelp;

	private LabelX LblJarimeV;

	private LabelX labelX7;

	public bool WorkOnHaliday => ChbSTTatili.Checked;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				translateTokenSource?.Cancel();
				translateTokenSource?.Dispose();
				translateTokenSource = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task TranslateFormAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[شیفت عادی]", out var value))
			{
				trDictionary.Add("[شیفت عادی]", Utilities.GetTranslate("[شیفت عادی]"));
			}
			if (!trDictionary.TryGetValue("[نوع تردد]", out value))
			{
				trDictionary.Add("[نوع تردد]", Utilities.GetTranslate("[نوع تردد]"));
			}
			if (!trDictionary.TryGetValue("[( در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها ]", out value))
			{
				trDictionary.Add("[( در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها ]", Utilities.GetTranslate("[( در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها ]"));
			}
			if (!trDictionary.TryGetValue("[تاریخ شروع]", out value))
			{
				trDictionary.Add("[تاریخ شروع]", Utilities.GetTranslate("[تاریخ شروع]"));
			}
			if (!trDictionary.TryGetValue("[تاریخ پایان]", out value))
			{
				trDictionary.Add("[تاریخ پایان]", Utilities.GetTranslate("[تاریخ پایان]"));
			}
			if (!trDictionary.TryGetValue("[ساعت پایان]", out value))
			{
				trDictionary.Add("[ساعت پایان]", Utilities.GetTranslate("[ساعت پایان]"));
			}
			if (!trDictionary.TryGetValue("[ساعت شروع]", out value))
			{
				trDictionary.Add("[ساعت شروع]", Utilities.GetTranslate("[ساعت شروع]"));
			}
			if (!trDictionary.TryGetValue("[شرح]", out value))
			{
				trDictionary.Add("[شرح]", Utilities.GetTranslate("[شرح]"));
			}
			if (!trDictionary.TryGetValue("[تاخیر در خروج ]", out value))
			{
				trDictionary.Add("[تاخیر در خروج ]", Utilities.GetTranslate("[تاخیر در خروج ]"));
			}
			if (!trDictionary.TryGetValue("[تاخیر در ورود]", out value))
			{
				trDictionary.Add("[تاخیر در ورود]", Utilities.GetTranslate("[تاخیر در ورود]"));
			}
			if (!trDictionary.TryGetValue("[تعجیل در ورود ]", out value))
			{
				trDictionary.Add("[تعجیل در ورود ]", Utilities.GetTranslate("[تعجیل در ورود ]"));
			}
			if (!trDictionary.TryGetValue("[تعجیل درخروج]", out value))
			{
				trDictionary.Add("[تعجیل درخروج]", Utilities.GetTranslate("[تعجیل درخروج]"));
			}
			if (!trDictionary.TryGetValue("[بازه ورود از]", out value))
			{
				trDictionary.Add("[بازه ورود از]", Utilities.GetTranslate("[بازه ورود از]"));
			}
			if (!trDictionary.TryGetValue("[بازه خروج از]", out value))
			{
				trDictionary.Add("[بازه خروج از]", Utilities.GetTranslate("[بازه خروج از]"));
			}
			if (!trDictionary.TryGetValue("[تا]", out value))
			{
				trDictionary.Add("[تا]", Utilities.GetTranslate("[تا]"));
			}
			if (!trDictionary.TryGetValue("[[روزهای جمعه]]", out value))
			{
				trDictionary.Add("[[روزهای جمعه]]", Utilities.GetTranslate("[[روزهای جمعه]]"));
			}
			if (!trDictionary.TryGetValue("[انتقال از روز قبل دارد]", out value))
			{
				trDictionary.Add("[انتقال از روز قبل دارد]", Utilities.GetTranslate("[انتقال از روز قبل دارد]"));
			}
			if (!trDictionary.TryGetValue("[انتقال از روز قبل دارد و حداکثر زمان خروج]", out value))
			{
				trDictionary.Add("[انتقال از روز قبل دارد و حداکثر زمان خروج]", Utilities.GetTranslate("[انتقال از روز قبل دارد و حداکثر زمان خروج]"));
			}
			if (!trDictionary.TryGetValue("[میباشد \r\n و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود]", out value))
			{
				trDictionary.Add("[میباشد \r\n و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود]", Utilities.GetTranslate("[میباشد \r\n و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود]"));
			}
			if (!trDictionary.TryGetValue("[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]", out value))
			{
				trDictionary.Add("[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]", Utilities.GetTranslate("[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]"));
			}
			if (!trDictionary.TryGetValue("[سقف اضافه کاری]", out value))
			{
				trDictionary.Add("[سقف اضافه کاری]", Utilities.GetTranslate("[سقف اضافه کاری]"));
			}
			if (!trDictionary.TryGetValue("[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]", out value))
			{
				trDictionary.Add("[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]", Utilities.GetTranslate("[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]"));
			}
			if (!trDictionary.TryGetValue("[در صورت حضور کارکرد محاسبه میشود]", out value))
			{
				trDictionary.Add("[در صورت حضور کارکرد محاسبه میشود]", Utilities.GetTranslate("[در صورت حضور کارکرد محاسبه میشود]"));
			}
			if (!trDictionary.TryGetValue("[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد]", out value))
			{
				trDictionary.Add("[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد]", Utilities.GetTranslate("[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد]"));
			}
			if (!trDictionary.TryGetValue("[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]", out value))
			{
				trDictionary.Add("[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]", Utilities.GetTranslate("[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]"));
			}
			if (!trDictionary.TryGetValue("[در صورت حضور، جمعه کاری محاسبه میشود]", out value))
			{
				trDictionary.Add("[در صورت حضور، جمعه کاری محاسبه میشود]", Utilities.GetTranslate("[در صورت حضور، جمعه کاری محاسبه میشود]"));
			}
			if (!trDictionary.TryGetValue("[مشخصات تردد]", out value))
			{
				trDictionary.Add("[مشخصات تردد]", Utilities.GetTranslate("[مشخصات تردد]"));
			}
			if (!trDictionary.TryGetValue("[جریمه ورود :]", out value))
			{
				trDictionary.Add("[جریمه ورود :]", Utilities.GetTranslate("[جریمه ورود :]"));
			}
			if (!trDictionary.TryGetValue("[شخص حداکثر میتواند تا این ساعت اضافه کاری کند]", out value))
			{
				trDictionary.Add("[شخص حداکثر میتواند تا این ساعت اضافه کاری کند]", Utilities.GetTranslate("[شخص حداکثر میتواند تا این ساعت اضافه کاری کند]"));
			}
			if (!trDictionary.TryGetValue("[شنبه]", out value))
			{
				trDictionary.Add("[شنبه]", Utilities.GetTranslate("[شنبه]"));
			}
			if (!trDictionary.TryGetValue("[یکشنبه]", out value))
			{
				trDictionary.Add("[یکشنبه]", Utilities.GetTranslate("[یکشنبه]"));
			}
			if (!trDictionary.TryGetValue("[دوشنبه]", out value))
			{
				trDictionary.Add("[دوشنبه]", Utilities.GetTranslate("[دوشنبه]"));
			}
			if (!trDictionary.TryGetValue("[سه شنبه]", out value))
			{
				trDictionary.Add("[سه شنبه]", Utilities.GetTranslate("[سه شنبه]"));
			}
			if (!trDictionary.TryGetValue("[چهارشنبه]", out value))
			{
				trDictionary.Add("[چهارشنبه]", Utilities.GetTranslate("[چهارشنبه]"));
			}
			if (!trDictionary.TryGetValue("[پنجشنبه]", out value))
			{
				trDictionary.Add("[پنجشنبه]", Utilities.GetTranslate("[پنجشنبه]"));
			}
			try
			{
				if (trDictionary.TryGetValue(((Control)TxtDesc).Text, out var value2))
				{
					((Control)TxtDesc).Text = value2;
				}
				if (trDictionary.TryGetValue("[[روزهای جمعه]]", out var value3))
				{
					((Control)labelX5).Text = value3;
				}
				if (trDictionary.TryGetValue("[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]", out var value4))
				{
					((Control)RbMustWork).Text = value4;
				}
				if (trDictionary.TryGetValue("[در صورت حضور کارکرد محاسبه میشود]", out var value5))
				{
					toolTip1.SetToolTip((Control)(object)RbMustWork, value5);
				}
				if (trDictionary.TryGetValue("[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد]", out var value6))
				{
					((Control)RbOptionalNotKarkerd).Text = value6;
				}
				if (trDictionary.TryGetValue("[در صورت حضور، جمعه کاری محاسبه میشود]", out var value7))
				{
					toolTip1.SetToolTip((Control)(object)RbOptionalNotKarkerd, value7);
					toolTip1.SetToolTip((Control)(object)RbOptionalKarkerd, value7);
				}
				if (trDictionary.TryGetValue("[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]", out var value8))
				{
					((Control)RbOptionalKarkerd).Text = value8;
				}
				if (trDictionary.TryGetValue("[نوع تردد]", out var value9))
				{
					((Control)labelX39).Text = value9;
				}
				if (trDictionary.TryGetValue("[( در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها ]", out var value10))
				{
					((Control)ChbSTTatili).Text = value10;
				}
				if (trDictionary.TryGetValue("[تاریخ شروع]", out var value11))
				{
					((Control)labelX2).Text = value11;
				}
				if (trDictionary.TryGetValue("[تاریخ پایان]", out var value12))
				{
					((Control)labelX3).Text = value12;
				}
				if (trDictionary.TryGetValue("[شرح]", out var value13))
				{
					((Control)labelX1).Text = value13;
					((Control)BtnHelp).Text = value13;
				}
				if (trDictionary.TryGetValue("[شنبه]", out var value14))
				{
					((Control)ChbSat).Text = value14;
				}
				if (trDictionary.TryGetValue("[یکشنبه]", out var value15))
				{
					((Control)ChbSun).Text = value15;
				}
				if (trDictionary.TryGetValue("[دوشنبه]", out var value16))
				{
					((Control)ChbMon).Text = value16;
				}
				if (trDictionary.TryGetValue("[سه شنبه]", out var value17))
				{
					((Control)ChbTue).Text = value17;
				}
				if (trDictionary.TryGetValue("[پنجشنبه]", out var value18))
				{
					((Control)ChbThr).Text = value18;
				}
				if (trDictionary.TryGetValue("[چهارشنبه]", out var value19))
				{
					((Control)ChbWed).Text = value19;
				}
				if (trDictionary.TryGetValue("[مشخصات تردد]", out var value20))
				{
					((Control)groupPanelTaradod).Text = value20;
				}
				if (trDictionary.TryGetValue("[جریمه ورود :]", out var value21))
				{
					((Control)labelX7).Text = value21;
				}
				if (trDictionary.TryGetValue("[شخص حداکثر میتواند تا این ساعت اضافه کاری کند]", out var value22))
				{
					((Control)LblDescPeakOverTime).Text = value22;
				}
				if (trDictionary.TryGetValue("[تاخیر در خروج ]", out var value23))
				{
					((Control)lblTakhirKh).Text = value23;
				}
				if (trDictionary.TryGetValue("[تعجیل در ورود ]", out var value24))
				{
					((Control)labelX10).Text = value24;
				}
				if (trDictionary.TryGetValue("[تا]", out var value25))
				{
					((Control)labelX8).Text = value25;
					((Control)labelX16).Text = value25;
				}
				if (trDictionary.TryGetValue("[سقف اضافه کاری]", out var value26))
				{
					((Control)labelX4).Text = value26;
				}
				if (trDictionary.TryGetValue("[تعجیل درخروج]", out var value27))
				{
					((Control)lblTajilKh).Text = value27;
				}
				if (trDictionary.TryGetValue("[تاخیر در ورود]", out var value28))
				{
					((Control)labelX11).Text = value28;
				}
				if (trDictionary.TryGetValue("[بازه ورود از]", out var value29))
				{
					((Control)labelX20).Text = value29;
				}
				if (trDictionary.TryGetValue("[بازه خروج از]", out var value30))
				{
					((Control)labelX13).Text = value30;
				}
				if (trDictionary.TryGetValue("[انتقال از روز قبل دارد]", out var value31))
				{
					((Control)LblNextDay).Text = value31;
				}
				if (trDictionary.TryGetValue("[ساعت پایان]", out var value32))
				{
					((Control)LblEndTime).Text = value32;
				}
				if (trDictionary.TryGetValue("[ساعت شروع]", out var value33))
				{
					((Control)LblStartTime).Text = value33;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TranslateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 173);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "TranslateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 177);
		}
	}

	public FrmCrtShiftNormal(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			((Control)TxtDesc).Text = "شیفت عادی";
			_clsShiftDet = new ShiftDet(contextGuid);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 193);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await TranslateFormAsync();
			await CmbShiftCoding_LoadDatdAsync(token);
			FillPanelTaradod();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FillPanelTaradod()
	{
		try
		{
			((Control)TxtDesc).Text = _clsShiftDet.Description;
			ChbSat.Checked = _clsShiftDet.Sat;
			ChbSun.Checked = _clsShiftDet.Sun;
			ChbMon.Checked = _clsShiftDet.Mon;
			ChbTue.Checked = _clsShiftDet.Tue;
			ChbWed.Checked = _clsShiftDet.Wed;
			ChbThr.Checked = _clsShiftDet.Tur;
			((Control)TXTStartDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(_clsShiftDet.DateStartM);
			((Control)TxtFinishM).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(_clsShiftDet.DateFinishM);
			((Control)TXTStartDate).Focus();
			FillGroupPanelTaradod(_clsShiftDet.ShiftCodeing);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillPanelTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 222);
		}
	}

	private async Task FillPanelTaradodAsync(short shiftCoding_Id)
	{
		try
		{
			if (_clsShiftDet.ShiftCodingId != shiftCoding_Id)
			{
				await _clsShiftDet.SetShiftCodingIdAsync(shiftCoding_Id);
				ShiftCodeing shiftCodeing = new ShiftCodeing(_clsShiftDet.ShiftCodeing);
				((Control)groupPanelTaradod).Text = "شیفت عادی";
				FillGroupPanelTaradod(shiftCodeing);
				((ListControl)CmbShiftCoding).SelectedValue = shiftCodeing.Id;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillPanelTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 237);
		}
	}

	private void FillGroupPanelTaradod(ShiftCodeing pShiftCodeing)
	{
		try
		{
			if (pShiftCodeing == null)
			{
				return;
			}
			((Control)LblPeakOvertime).Text = Time.IntToTime((int)pShiftCodeing.PeakOvertime);
			int num = pShiftCodeing.KhorojEnd.Hour * 60 + pShiftCodeing.KhorojEnd.Minute;
			((Control)LblDescPeakOverTime).Visible = pShiftCodeing.PeakOvertime > num;
			((ListControl)CmbShiftCoding).SelectedValue = _clsShiftDet.ShiftCodingId;
			((Control)LblValStartTime).Text = $"{pShiftCodeing.M1:00} : {pShiftCodeing.H1:00}";
			((Control)LblValEndTime).Text = $"{pShiftCodeing.M2:00} : {pShiftCodeing.H2:00}";
			((Control)LblTajilStartTime).Text = pShiftCodeing.TajilV.ToString();
			((Control)LblTakhirStartTime).Text = pShiftCodeing.TakhirV.ToString();
			((Control)LblJarimeV).Text = (pShiftCodeing.ChkJarime ? pShiftCodeing.JarimeV.ToString() : "---");
			((Control)LblTajilEndTime).Text = pShiftCodeing.TajilKh.ToString();
			((Control)LblTakhirEndTime).Text = pShiftCodeing.TakhirKh.ToString();
			((Control)LblStartAzTime).Text = pShiftCodeing.VorodStart.ToString("HH:mm");
			((Control)LblStartTaTime).Text = pShiftCodeing.VorodEnd.ToString("HH:mm");
			((Control)LblEndAzTime).Text = pShiftCodeing.KhorojStart.ToString("HH:mm");
			((Control)LblEndTaTime).Text = pShiftCodeing.KhorojEnd.ToString("HH:mm");
			((Control)LblNextDay).Visible = false;
			if (pShiftCodeing.NextDay)
			{
				((Control)LblNextDay).Visible = true;
				((Control)LblNextDay).Text = "انتقال از روز قبل دارد و حداکثر زمان خروج " + Time.IntToTime(pShiftCodeing.MaxTimeCanExit) + " میباشد \r\n و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود.";
				if (trDictionary.TryGetValue("[انتقال از روز قبل دارد و حداکثر زمان خروج]", out var value) && trDictionary.TryGetValue("[میباشد \r\n و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود]", out var value2))
				{
					((Control)LblNextDay).Text = value + " " + Time.IntToTime(pShiftCodeing.MaxTimeCanExit) + " " + value2;
				}
			}
			else
			{
				((Control)LblNextDay).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillGroupPanelTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 275);
		}
	}

	public ShiftDet ReadFormData()
	{
		try
		{
			_clsShiftDet.Sat = ChbSat.Checked;
			_clsShiftDet.Sun = ChbSun.Checked;
			_clsShiftDet.Mon = ChbMon.Checked;
			_clsShiftDet.Tue = ChbTue.Checked;
			_clsShiftDet.Wed = ChbWed.Checked;
			_clsShiftDet.Tur = ChbThr.Checked;
			if (RbOptionalKarkerd.Checked || RbMustWork.Checked)
			{
				_clsShiftDet.Fri = true;
			}
			if (RbOptionalKarkerd.Checked)
			{
				_clsShiftDet.FriType = ShiftDet.FriDayType.EkhtiyariAndKarkard;
			}
			else if (RbOptionalNotKarkerd.Checked)
			{
				_clsShiftDet.FriType = ShiftDet.FriDayType.EkhtiyariAndAdamKarkard;
			}
			else if (RbMustWork.Checked)
			{
				_clsShiftDet.FriType = ShiftDet.FriDayType.Elzami;
			}
			else
			{
				_clsShiftDet.FriType = ShiftDet.FriDayType.EkhtiyariAndKarkard;
			}
			_clsShiftDet.Description = Text.FixString(((Control)TxtDesc).Text);
			_clsShiftDet.DateStartM = TXTStartDate.MiladiDate.Date;
			_clsShiftDet.DateFinishM = TxtFinishM.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReadFormData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 303);
		}
		return _clsShiftDet;
	}

	private void SetTooltip()
	{
		try
		{
			string text = "ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.";
			if (trDictionary.TryGetValue("[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]", out var value))
			{
				text = value;
			}
			toolTip1.SetToolTip((Control)(object)LblNextDay, text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 317);
		}
	}

	private async Task CmbShiftCoding_LoadDatdAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			((ListControl)CmbShiftCoding).DisplayMember = "NameShiftCodeing";
			((ListControl)CmbShiftCoding).ValueMember = "Id";
			ComboBoxEx cmbShiftCoding = CmbShiftCoding;
			((ComboBox)cmbShiftCoding).DataSource = await ShiftCodeing.GetAllAsync(((CacheBaseForm)this).ContextGuid, (TypeShiftEnumType)0, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbShiftCoding_LoadDatdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 328);
		}
	}

	private void FRMCrtShiftNormal_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTStartDate).Focus();
			SetTooltip();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMCrtShiftNormal_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 338);
		}
	}

	private void FRMCrtShiftNormal_KeyDown(object sender, KeyEventArgs e)
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
					if ((int)keyCode == 123)
					{
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMCrtShiftNormal_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 359);
		}
	}

	private async void BtnSelectShiftCoding_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmShiftCodeingList frm = new FrmShiftCodeingList(((CacheBaseForm)this).ContextGuid, _font, isSelect: true, (TypeShiftEnumType)0);
			if ((int)((Form)frm).ShowDialog() == 1)
			{
				await CmbShiftCoding_LoadDatdAsync();
				await FillPanelTaradodAsync(frm.SelectedRow.Id);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectShiftCoding_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 370);
		}
	}

	private async void CmbShiftCoding_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)CmbShiftCoding).SelectedValue != null)
			{
				await FillPanelTaradodAsync(Convert.ToInt16(((ListControl)CmbShiftCoding).SelectedValue.ToString()));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbShiftCoding_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 379);
		}
	}

	private void BtnHelp_Click(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[1- حضور اختیاری در جمعه ها، حتی در صورت عدم حضور جزء کارکرد میباشد و در صورت حضور جمعه کاری نیز لحاظ میشود]");
			stringBuilder.AppendLine("[2- حضور اختیاری در جمعه ها، فقط در صورت حضور جزء کارکرد میباشد و در صورت حضور جمعه کاری لحاظ میشود]");
			stringBuilder.AppendLine("[3- حضور در روز جمعه الزامی است و مشابه روزهای عادی محاسبه می شود و در صورت عدم حضور غیبت محسوب میشود]");
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, stringBuilder.ToString(), "[توضیحات روز جمعه]")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHelp_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftNormal.cs", 396);
		}
	}

	private void FrmCrtShiftNormal_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb3: Expected O, but got Unknown
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Expected O, but got Unknown
		//IL_112d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1137: Expected O, but got Unknown
		//IL_1163: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Expected O, but got Unknown
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_2872: Expected O, but got Unknown
		//IL_288c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2896: Expected O, but got Unknown
		components = new Container();
		PanelEx1 = new PanelEx();
		groupPanel7 = new GroupPanel();
		labelX5 = new LabelX();
		RbMustWork = new RadioButton();
		RbOptionalNotKarkerd = new RadioButton();
		RbOptionalKarkerd = new RadioButton();
		labelX39 = new LabelX();
		ChbSTTatili = new CheckBoxX();
		labelX2 = new LabelX();
		ChbSat = new CheckBoxX();
		TXTStartDate = new DateBoxFa();
		ChbMon = new CheckBoxX();
		labelX1 = new LabelX();
		ChbSun = new CheckBoxX();
		labelX3 = new LabelX();
		ChbThr = new CheckBoxX();
		TxtFinishM = new DateBoxFa();
		ChbTue = new CheckBoxX();
		ChbWed = new CheckBoxX();
		CmbShiftCoding = new ComboBoxEx();
		BtnHelp = new ButtonX();
		BtnSelectShiftCoding = new ButtonX();
		TxtDesc = new TextBoxX();
		groupPanelTaradod = new GroupPanel();
		PanelNormal = new Panel();
		LblJarimeV = new LabelX();
		labelX7 = new LabelX();
		LblDescPeakOverTime = new LabelX();
		LblTajilStartTime = new LabelX();
		LblPeakOvertime = new LabelX();
		LblTajilEndTime = new LabelX();
		lblTakhirKh = new LabelX();
		LblTakhirStartTime = new LabelX();
		LblTakhirEndTime = new LabelX();
		LblEndTaTime = new LabelX();
		labelX8 = new LabelX();
		labelX16 = new LabelX();
		labelX10 = new LabelX();
		labelX4 = new LabelX();
		lblTajilKh = new LabelX();
		labelX11 = new LabelX();
		LblStartTaTime = new LabelX();
		LblEndAzTime = new LabelX();
		LblStartAzTime = new LabelX();
		labelX20 = new LabelX();
		labelX13 = new LabelX();
		LblNextDay = new LabelX();
		LblValEndTime = new LabelX();
		LblValStartTime = new LabelX();
		LblEndTime = new LabelX();
		LblStartTime = new LabelX();
		toolTip1 = new ToolTip(components);
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)groupPanelTaradod).SuspendLayout();
		((Control)PanelNormal).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel7);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(499, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel7).Controls.Add((Control)(object)RbMustWork);
		((Control)groupPanel7).Controls.Add((Control)(object)RbOptionalNotKarkerd);
		((Control)groupPanel7).Controls.Add((Control)(object)RbOptionalKarkerd);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX39);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbSTTatili);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbSat);
		((Control)groupPanel7).Controls.Add((Control)(object)TXTStartDate);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbMon);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbSun);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbThr);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtFinishM);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbTue);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbWed);
		((Control)groupPanel7).Controls.Add((Control)(object)CmbShiftCoding);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnHelp);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnSelectShiftCoding);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelTaradod);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(475, 476);
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
		((Control)groupPanel7).TabIndex = 0;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).ForeColor = Color.DarkRed;
		((Control)labelX5).Location = new Point(394, 337);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(58, 24);
		((Control)labelX5).TabIndex = 14;
		((Control)labelX5).Text = "روزهای جمعه :";
		((Control)RbMustWork).ForeColor = SystemColors.HotTrack;
		((Control)RbMustWork).Location = new Point(75, 368);
		((Control)RbMustWork).Name = "RbMustWork";
		((Control)RbMustWork).Size = new Size(314, 24);
		((Control)RbMustWork).TabIndex = 17;
		((Control)RbMustWork).Text = "حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود";
		toolTip1.SetToolTip((Control)(object)RbMustWork, "در صورت حضور کارکرد محاسبه میشود");
		((ButtonBase)RbMustWork).UseVisualStyleBackColor = true;
		((Control)RbOptionalNotKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalNotKarkerd).Location = new Point(75, 337);
		((Control)RbOptionalNotKarkerd).Name = "RbOptionalNotKarkerd";
		((Control)RbOptionalNotKarkerd).Size = new Size(314, 24);
		((Control)RbOptionalNotKarkerd).TabIndex = 16;
		((Control)RbOptionalNotKarkerd).Text = "حضور اختیاری در جمعه ها و عدم محاسبه کارکرد";
		toolTip1.SetToolTip((Control)(object)RbOptionalNotKarkerd, "در صورت حضور فقط جمعه کاری محاسبه میشود");
		((ButtonBase)RbOptionalNotKarkerd).UseVisualStyleBackColor = true;
		RbOptionalKarkerd.Checked = true;
		((Control)RbOptionalKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalKarkerd).Location = new Point(75, 308);
		((Control)RbOptionalKarkerd).Name = "RbOptionalKarkerd";
		((Control)RbOptionalKarkerd).Size = new Size(314, 24);
		((Control)RbOptionalKarkerd).TabIndex = 15;
		RbOptionalKarkerd.TabStop = true;
		((Control)RbOptionalKarkerd).Text = "حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود";
		toolTip1.SetToolTip((Control)(object)RbOptionalKarkerd, "در صورت حضور جمعه کاری محاسبه میشود");
		((ButtonBase)RbOptionalKarkerd).UseVisualStyleBackColor = true;
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(398, 42);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(60, 31);
		((Control)labelX39).TabIndex = 4;
		((Control)labelX39).Text = "نوع تردد :";
		labelX39.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbSTTatili).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSTTatili).Location = new Point(148, 398);
		((Control)ChbSTTatili).Name = "ChbSTTatili";
		((Control)ChbSTTatili).Size = new Size(307, 33);
		((BaseItemControl)ChbSTTatili).Style = (eDotNetBarStyle)9;
		((Control)ChbSTTatili).TabIndex = 19;
		((Control)ChbSTTatili).Text = "(در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(398, 5);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(60, 31);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "تاریخ شروع :";
		labelX2.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbSat).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSat.Checked = true;
		ChbSat.CheckState = (CheckState)1;
		ChbSat.CheckValue = "Y";
		((Control)ChbSat).Location = new Point(416, 269);
		((Control)ChbSat).Name = "ChbSat";
		((Control)ChbSat).Size = new Size(37, 33);
		((BaseItemControl)ChbSat).Style = (eDotNetBarStyle)9;
		((Control)ChbSat).TabIndex = 8;
		((Control)ChbSat).Text = "شنبه";
		TXTStartDate.AllowNull = false;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(287, 5);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2024, 8, 23, 10, 48, 12, 248);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(105, 20);
		((Control)TXTStartDate).TabIndex = 1;
		((Control)TXTStartDate).Text = "22/08/2024";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((BaseItemControl)ChbMon).BackgroundStyle.CornerType = (eCornerType)1;
		ChbMon.Checked = true;
		ChbMon.CheckState = (CheckState)1;
		ChbMon.CheckValue = "Y";
		((Control)ChbMon).Location = new Point(263, 269);
		((Control)ChbMon).Name = "ChbMon";
		((Control)ChbMon).Size = new Size(45, 33);
		((BaseItemControl)ChbMon).Style = (eDotNetBarStyle)9;
		((Control)ChbMon).TabIndex = 10;
		((Control)ChbMon).Text = "دوشنبه";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(427, 434);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(28, 31);
		((Control)labelX1).TabIndex = 20;
		((Control)labelX1).Text = "شرح :";
		labelX1.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbSun).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSun.Checked = true;
		ChbSun.CheckState = (CheckState)1;
		ChbSun.CheckValue = "Y";
		((Control)ChbSun).Location = new Point(339, 269);
		((Control)ChbSun).Name = "ChbSun";
		((Control)ChbSun).Size = new Size(46, 33);
		((BaseItemControl)ChbSun).Style = (eDotNetBarStyle)9;
		((Control)ChbSun).TabIndex = 9;
		((Control)ChbSun).Text = "یکشنبه";
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(122, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(71, 31);
		((Control)labelX3).TabIndex = 2;
		((Control)labelX3).Text = "تاریخ پایان :";
		labelX3.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbThr).BackgroundStyle.CornerType = (eCornerType)1;
		ChbThr.Checked = true;
		ChbThr.CheckState = (CheckState)1;
		ChbThr.CheckValue = "Y";
		((Control)ChbThr).Location = new Point(18, 269);
		((Control)ChbThr).Name = "ChbThr";
		((Control)ChbThr).Size = new Size(49, 33);
		((BaseItemControl)ChbThr).Style = (eDotNetBarStyle)9;
		((Control)ChbThr).TabIndex = 13;
		((Control)ChbThr).Text = "پنجشنبه";
		TxtFinishM.AllowNull = false;
		TxtFinishM.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtFinishM).Border.CornerType = (eCornerType)1;
		TxtFinishM.EnterToTab = true;
		TxtFinishM.EscToClose = true;
		TxtFinishM.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtFinishM.FocusBackColor = Color.Yellow;
		TxtFinishM.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtFinishM.FocusForeColor = Color.Blue;
		TxtFinishM.FocusTextSelect = true;
		((Control)TxtFinishM).Font = new Font("Times New Roman", 12f);
		((Control)TxtFinishM).Location = new Point(18, 3);
		TxtFinishM.MessageEmptyShowDialog = false;
		TxtFinishM.MessegeEmpty = "";
		TxtFinishM.MessegeEmptyInFormRight = true;
		TxtFinishM.MessegeEmptyShowInForm = false;
		TxtFinishM.MiladiDate = new DateTime(2024, 8, 23, 10, 48, 12, 265);
		((Control)TxtFinishM).Name = "TxtFinishM";
		((Control)TxtFinishM).RightToLeft = (RightToLeft)0;
		TxtFinishM.ShowNameOfDayOnToolTip = false;
		TxtFinishM.ShowToolTipText = false;
		((Control)TxtFinishM).Size = new Size(99, 20);
		((Control)TxtFinishM).TabIndex = 3;
		((Control)TxtFinishM).Text = "22/08/2024";
		((TextBox)TxtFinishM).TextAlign = (HorizontalAlignment)2;
		TxtFinishM.TootTipDelay = 500;
		((BaseItemControl)ChbTue).BackgroundStyle.CornerType = (eCornerType)1;
		ChbTue.Checked = true;
		ChbTue.CheckState = (CheckState)1;
		ChbTue.CheckValue = "Y";
		((Control)ChbTue).Location = new Point(182, 269);
		((Control)ChbTue).Name = "ChbTue";
		((Control)ChbTue).Size = new Size(51, 33);
		((BaseItemControl)ChbTue).Style = (eDotNetBarStyle)9;
		((Control)ChbTue).TabIndex = 11;
		((Control)ChbTue).Text = "سه شنبه";
		((BaseItemControl)ChbWed).BackgroundStyle.CornerType = (eCornerType)1;
		ChbWed.Checked = true;
		ChbWed.CheckState = (CheckState)1;
		ChbWed.CheckValue = "Y";
		((Control)ChbWed).Location = new Point(98, 269);
		((Control)ChbWed).Name = "ChbWed";
		((Control)ChbWed).Size = new Size(53, 33);
		((BaseItemControl)ChbWed).Style = (eDotNetBarStyle)9;
		((Control)ChbWed).TabIndex = 12;
		((Control)ChbWed).Text = "چهارشنبه";
		((Control)CmbShiftCoding).Anchor = (AnchorStyles)13;
		((ComboBox)CmbShiftCoding).DrawMode = (DrawMode)1;
		((ComboBox)CmbShiftCoding).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbShiftCoding).FormattingEnabled = true;
		((ComboBox)CmbShiftCoding).ItemHeight = 26;
		((Control)CmbShiftCoding).Location = new Point(57, 42);
		((Control)CmbShiftCoding).Name = "CmbShiftCoding";
		((Control)CmbShiftCoding).Size = new Size(334, 32);
		CmbShiftCoding.Style = (eDotNetBarStyle)9;
		((Control)CmbShiftCoding).TabIndex = 5;
		((ComboBox)CmbShiftCoding).SelectedIndexChanged += CmbShiftCoding_SelectedIndexChanged;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		BtnHelp.ColorTable = (eButtonColor)6;
		BtnHelp.Image = (Image)(object)Resources.helpinfo32;
		BtnHelp.ImagePosition = (eImagePosition)2;
		((Control)BtnHelp).Location = new Point(16, 308);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(54, 84);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 18;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "توضیح";
		((Control)BtnHelp).Click += BtnHelp_Click;
		((Control)BtnSelectShiftCoding).AccessibleRole = (AccessibleRole)43;
		BtnSelectShiftCoding.ColorTable = (eButtonColor)6;
		((Control)BtnSelectShiftCoding).Location = new Point(18, 42);
		((Control)BtnSelectShiftCoding).Name = "BtnSelectShiftCoding";
		((Control)BtnSelectShiftCoding).Size = new Size(34, 31);
		((PopupItemControl)BtnSelectShiftCoding).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectShiftCoding).TabIndex = 6;
		((Control)BtnSelectShiftCoding).TabStop = false;
		((Control)BtnSelectShiftCoding).Text = "...";
		((Control)BtnSelectShiftCoding).Click += BtnSelectShiftCoding_Click;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(18, 434);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(404, 31);
		((Control)TxtDesc).TabIndex = 21;
		((PanelControl)groupPanelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelTaradod).Controls.Add((Control)(object)PanelNormal);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblValEndTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblValStartTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblEndTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblStartTime);
		groupPanelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelTaradod).Location = new Point(18, 79);
		((Control)groupPanelTaradod).Name = "groupPanelTaradod";
		((Control)groupPanelTaradod).Size = new Size(437, 184);
		((PanelControl)groupPanelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelTaradod).TabIndex = 7;
		((Control)groupPanelTaradod).Text = "مشخصات تردد";
		((Control)PanelNormal).Controls.Add((Control)(object)LblJarimeV);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX7);
		((Control)PanelNormal).Controls.Add((Control)(object)LblDescPeakOverTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTajilStartTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblPeakOvertime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTajilEndTime);
		((Control)PanelNormal).Controls.Add((Control)(object)lblTakhirKh);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTakhirStartTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTakhirEndTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblEndTaTime);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX8);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX16);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX10);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX4);
		((Control)PanelNormal).Controls.Add((Control)(object)lblTajilKh);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX11);
		((Control)PanelNormal).Controls.Add((Control)(object)LblStartTaTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblEndAzTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblStartAzTime);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX20);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX13);
		((Control)PanelNormal).Controls.Add((Control)(object)LblNextDay);
		((Control)PanelNormal).Location = new Point(-1, 25);
		((Control)PanelNormal).Name = "PanelNormal";
		((Control)PanelNormal).Size = new Size(434, 128);
		((Control)PanelNormal).TabIndex = 4;
		((Control)LblJarimeV).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblJarimeV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblJarimeV).ForeColor = Color.DarkRed;
		((Control)LblJarimeV).Location = new Point(308, 49);
		((Control)LblJarimeV).Name = "LblJarimeV";
		((Control)LblJarimeV).Size = new Size(39, 24);
		((Control)LblJarimeV).TabIndex = 21;
		((Control)LblJarimeV).Text = "-----";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(351, 52);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(68, 24);
		((Control)labelX7).TabIndex = 20;
		((Control)labelX7).Text = "جریمه ورود :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)LblDescPeakOverTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblDescPeakOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDescPeakOverTime).ForeColor = Color.DarkRed;
		((Control)LblDescPeakOverTime).Location = new Point(72, 76);
		((Control)LblDescPeakOverTime).Name = "LblDescPeakOverTime";
		((Control)LblDescPeakOverTime).Size = new Size(228, 24);
		((Control)LblDescPeakOverTime).TabIndex = 18;
		((Control)LblDescPeakOverTime).Text = "شخص حداکثر میتواند تا این ساعت اضافه کاری کند";
		((Control)LblTajilStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTajilStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTajilStartTime).ForeColor = Color.DarkRed;
		((Control)LblTajilStartTime).Location = new Point(310, 5);
		((Control)LblTajilStartTime).Name = "LblTajilStartTime";
		((Control)LblTajilStartTime).Size = new Size(39, 24);
		((Control)LblTajilStartTime).TabIndex = 1;
		((Control)LblTajilStartTime).Text = "-----";
		((Control)LblPeakOvertime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblPeakOvertime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPeakOvertime).ForeColor = Color.DarkRed;
		((Control)LblPeakOvertime).Location = new Point(305, 76);
		((Control)LblPeakOvertime).Name = "LblPeakOvertime";
		((Control)LblPeakOvertime).Size = new Size(39, 24);
		((Control)LblPeakOvertime).TabIndex = 17;
		((Control)LblPeakOvertime).Text = "-----";
		((Control)LblTajilEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTajilEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTajilEndTime).ForeColor = Color.DarkRed;
		((Control)LblTajilEndTime).Location = new Point(308, 27);
		((Control)LblTajilEndTime).Name = "LblTajilEndTime";
		((Control)LblTajilEndTime).Size = new Size(39, 24);
		((Control)LblTajilEndTime).TabIndex = 9;
		((Control)LblTajilEndTime).Text = "-----";
		((Control)lblTakhirKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirKh).Location = new Point(242, 27);
		((Control)lblTakhirKh).Name = "lblTakhirKh";
		((Control)lblTakhirKh).Size = new Size(61, 24);
		((Control)lblTakhirKh).TabIndex = 10;
		((Control)lblTakhirKh).Text = "تاخیر در خروج: ";
		lblTakhirKh.TextAlignment = (StringAlignment)2;
		((Control)LblTakhirStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTakhirStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTakhirStartTime).ForeColor = Color.DarkRed;
		((Control)LblTakhirStartTime).Location = new Point(201, 5);
		((Control)LblTakhirStartTime).Name = "LblTakhirStartTime";
		((Control)LblTakhirStartTime).Size = new Size(36, 24);
		((Control)LblTakhirStartTime).TabIndex = 3;
		((Control)LblTakhirStartTime).Text = "-----";
		((Control)LblTakhirEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTakhirEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTakhirEndTime).ForeColor = Color.DarkRed;
		((Control)LblTakhirEndTime).Location = new Point(201, 27);
		((Control)LblTakhirEndTime).Name = "LblTakhirEndTime";
		((Control)LblTakhirEndTime).Size = new Size(36, 24);
		((Control)LblTakhirEndTime).TabIndex = 11;
		((Control)LblTakhirEndTime).Text = "-----";
		((Control)LblEndTaTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblEndTaTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndTaTime).ForeColor = Color.DarkRed;
		((Control)LblEndTaTime).Location = new Point(17, 27);
		((Control)LblEndTaTime).Name = "LblEndTaTime";
		((Control)LblEndTaTime).Size = new Size(36, 24);
		((Control)LblEndTaTime).TabIndex = 15;
		((Control)LblEndTaTime).Text = "-----";
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).ForeColor = SystemColors.ControlText;
		((Control)labelX8).Location = new Point(62, 2);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(15, 24);
		((Control)labelX8).TabIndex = 6;
		((Control)labelX8).Text = "تا :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).ForeColor = SystemColors.ControlText;
		((Control)labelX16).Location = new Point(62, 27);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(15, 24);
		((Control)labelX16).TabIndex = 14;
		((Control)labelX16).Text = "تا :";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(351, 5);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(60, 24);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "تعجیل در ورود:";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(349, 76);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(74, 24);
		((Control)labelX4).TabIndex = 16;
		((Control)labelX4).Text = "سقف اضافه کاری :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)lblTajilKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilKh).Location = new Point(351, 30);
		((Control)lblTajilKh).Name = "lblTajilKh";
		((Control)lblTajilKh).Size = new Size(68, 24);
		((Control)lblTajilKh).TabIndex = 8;
		((Control)lblTajilKh).Text = "تعجیل درخروج:";
		lblTajilKh.TextAlignment = (StringAlignment)2;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(242, 5);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(61, 24);
		((Control)labelX11).TabIndex = 2;
		((Control)labelX11).Text = "تاخیر در ورود:";
		labelX11.TextAlignment = (StringAlignment)2;
		((Control)LblStartTaTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartTaTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartTaTime).ForeColor = Color.DarkRed;
		((Control)LblStartTaTime).Location = new Point(17, 5);
		((Control)LblStartTaTime).Name = "LblStartTaTime";
		((Control)LblStartTaTime).Size = new Size(36, 24);
		((Control)LblStartTaTime).TabIndex = 7;
		((Control)LblStartTaTime).Text = "------";
		((Control)LblEndAzTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblEndAzTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndAzTime).ForeColor = Color.DarkRed;
		((Control)LblEndAzTime).Location = new Point(87, 27);
		((Control)LblEndAzTime).Name = "LblEndAzTime";
		((Control)LblEndAzTime).Size = new Size(36, 24);
		((Control)LblEndAzTime).TabIndex = 13;
		((Control)LblEndAzTime).Text = "------";
		((Control)LblStartAzTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartAzTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartAzTime).ForeColor = Color.DarkRed;
		((Control)LblStartAzTime).Location = new Point(87, 5);
		((Control)LblStartAzTime).Name = "LblStartAzTime";
		((Control)LblStartAzTime).Size = new Size(36, 24);
		((Control)LblStartAzTime).TabIndex = 5;
		((Control)LblStartAzTime).Text = "-----";
		((Control)labelX20).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).ForeColor = SystemColors.ControlText;
		((Control)labelX20).Location = new Point(128, 5);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(57, 24);
		((Control)labelX20).TabIndex = 4;
		((Control)labelX20).Text = "بازه ورود از:";
		labelX20.TextAlignment = (StringAlignment)2;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = SystemColors.ControlText;
		((Control)labelX13).Location = new Point(128, 27);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(57, 24);
		((Control)labelX13).TabIndex = 12;
		((Control)labelX13).Text = "بازه خروج از:";
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)LblNextDay).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNextDay).ForeColor = Color.DarkRed;
		((Control)LblNextDay).Location = new Point(7, 97);
		((Control)LblNextDay).Name = "LblNextDay";
		((Control)LblNextDay).Size = new Size(410, 26);
		((Control)LblNextDay).TabIndex = 19;
		((Control)LblNextDay).Text = "انتقال از روز قبل دارد";
		LblNextDay.WordWrap = true;
		((BaseItemControl)LblValEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValEndTime).ForeColor = Color.DarkRed;
		((Control)LblValEndTime).Location = new Point(35, -7);
		((Control)LblValEndTime).Name = "LblValEndTime";
		((Control)LblValEndTime).Size = new Size(39, 26);
		((Control)LblValEndTime).TabIndex = 3;
		((Control)LblValEndTime).Text = "-----";
		((Control)LblValStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblValStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValStartTime).ForeColor = Color.DarkRed;
		((Control)LblValStartTime).Location = new Point(265, -6);
		((Control)LblValStartTime).Name = "LblValStartTime";
		((Control)LblValStartTime).Size = new Size(63, 26);
		((Control)LblValStartTime).TabIndex = 1;
		((Control)LblValStartTime).Text = "-----";
		((BaseItemControl)LblEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndTime).Location = new Point(80, -6);
		((Control)LblEndTime).Name = "LblEndTime";
		((Control)LblEndTime).Size = new Size(79, 26);
		((Control)LblEndTime).TabIndex = 2;
		((Control)LblEndTime).Text = "ساعت پایان :";
		LblEndTime.TextAlignment = (StringAlignment)2;
		((Control)LblStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartTime).Location = new Point(334, -6);
		((Control)LblStartTime).Name = "LblStartTime";
		((Control)LblStartTime).Size = new Size(68, 26);
		((Control)LblStartTime).TabIndex = 0;
		((Control)LblStartTime).Text = "ساعت شروع :";
		LblStartTime.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(499, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCrtShiftNormal";
		((Control)this).Text = "FRMCrtShift1";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmCrtShiftNormal_FormClosed);
		((Form)this).Load += FRMCrtShiftNormal_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMCrtShiftNormal_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanelTaradod).ResumeLayout(false);
		((Control)PanelNormal).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
