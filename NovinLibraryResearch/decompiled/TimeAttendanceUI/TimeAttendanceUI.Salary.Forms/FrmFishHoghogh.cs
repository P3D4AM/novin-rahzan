using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Reports;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms.Codeing;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmFishHoghogh : FormExtention
{
	private Fish _clsFish;

	private string Frm_Text = "[صدور فیش حقوقی]";

	private string BtnAccept_Text = "[ثبت](F5)";

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _changeToken;

	private CancellationTokenSource _fillGridDetToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private SplitContainer splitContainer1;

	private GroupPanel groupPanel1;

	internal DataGridViewX DGMazaya1;

	private ButtonX BTNADDMazaya;

	private ButtonX BTNDeleteMazaya;

	private ButtonX BtnEditMazaya;

	private GroupPanel groupPanel2;

	private ButtonX BtnEditKosorat;

	private ButtonX BtnAddKosorat;

	private ButtonX BtnDelKosor;

	internal DataGridViewX DGKosorat1;

	private ButtonX BtnAccept;

	private LabelX labelX38;

	private TextBoxX TXTDesc;

	private LabelX labelX37;

	private TextBoxFa TXTKolMande;

	private LabelX labelX36;

	private TextBoxFa TXTKolKosorat;

	private LabelX labelX35;

	private TextBoxFa TXTKolMazaya;

	private LabelX labelX34;

	private TextBoxFa TXTKolKarkerd;

	private GroupPanel GPHoghogh;

	private LabelX LblName;

	private LabelX LblCodeMeli;

	private LabelX LblFName;

	private LinkLabel LblCodeTafsil;

	private LabelX LblTadate;

	private LabelX LblfromDate;

	private NumericUpDown TXTFishNO;

	private LabelX labelX15;

	private LabelX labelX43;

	private LabelX labelX42;

	private LabelX labelX41;

	private LabelX labelX40;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private BindingSource fishMDetBindingSource;

	private BindingSource fishKDetBindingSource;

	private LinkLabel LblMaliat;

	private TextBoxFa TxtIncludedTax;

	private LinkLabel LblBime;

	private TextBoxFa TxtIncludedInsurance;

	private ButtonX btnNotifyMe;

	private Timer timerReadNotify;

	private ButtonX btnModifiedTaradod;

	private LabelX labelX44;

	private NumericUpDown NU_workDayBime;

	private NumericUpDown NU_workDayMaliat;

	private LabelX labelX53;

	private ContextMenuStrip contextMenuStrip1;

	private DateBoxFa TXTDate;

	private DataGridViewTextBoxColumn DGMName;

	private DataGridViewTextBoxColumn DGMCredit;

	private DataGridViewTextBoxColumn DgDescription;

	private DataGridViewTextBoxColumn DGKName;

	private DataGridViewTextBoxColumn DGKDebit;

	private DataGridViewTextBoxColumn DgDescriptionK;

	private GroupPanel GPKarkerd;

	private RadioButton RbtnEndMonth;

	private RadioButton RbtnEndGharardad;

	private NumericUpDown NU_Jarime;

	private LabelX labelX75;

	private LabelX labelX55;

	private LabelX labelX56;

	private LabelX labelX77;

	private TextBoxFa TXTJarimeSum;

	private TextBoxFa TXTJarimePrice;

	private LabelX labelX21;

	private LabelX labelX28;

	private LabelX labelX31;

	private LabelX labelX49;

	private MaskedTextBox TxtVacationRemind;

	private LabelX Lbl11;

	private LabelX Lbl10;

	private NumericUpDown NumDailyVacation;

	private LabelX labelX78;

	private LabelX Lbl1;

	private LabelX Lbl2;

	private NumericUpDown NUMVacation;

	private NumericUpDown NUHVacation;

	private LabelX LblCountMorHourly;

	private LabelX Lbl4;

	private LabelX Lbl3;

	private LabelX labelX57;

	private TextBoxFa TXTVacationSum;

	private LabelX labelX59;

	private LabelX Lbl5;

	private MaskedTextBox NU_Mission;

	private NumericUpDown NUH_ClocklyVacation;

	private LabelX labelX1;

	private TextBoxFa TXTVacationPrice;

	private TextBoxFa TXTVacationSumUsed;

	private TextBoxFa TXTMissionSum;

	private LabelX labelX24;

	private LabelX labelX76;

	private TextBoxFa TXTMissionPrice;

	private LabelX labelX48;

	private LabelX labelX25;

	private TextBoxFa TXTKarkerdSum;

	private NumericUpDown NUM_ClocklyVacation;

	private TextBoxFa TXTEzafeSum3;

	private TextBoxFa TXTEzafeSum2;

	private TextBoxFa TXTEzafeSum1;

	private LabelX LblCountMorNotHoghogh;

	private LabelX LblCountMorWithHoghogh;

	private TextBoxFa TXTShabSum;

	private LabelX labelX74;

	private LabelX labelX70;

	private LabelX labelX23;

	private LabelX labelX22;

	private LabelX labelX18;

	private LabelX LblNobatKari1;

	private LabelX labelX16;

	private LabelX LblCountKarkard;

	private LabelX labelX39;

	private LabelX LblCountKol;

	private LabelX labelX14;

	private LabelX labelX32;

	private LabelX labelX54;

	private LabelX LblCountGheybat;

	private RadioButton RbtnSaati;

	private TextBoxFa TXTNobatSum1;

	private LabelX labelX50;

	private RadioButton RbtnDaily;

	private TextBoxFa TXTJomeSum;

	private TextBoxFa TXTTatilSum;

	private RadioButton RbtnMonthly;

	private LabelX labelX66;

	private TextBoxFa TXTKasrSum3;

	private TextBoxFa TXTKasrSum2;

	private TextBoxFa TXTKasrSum1;

	private LabelX labelX62;

	private LabelX labelX5;

	private LabelX labelX6;

	private TextBoxFa TXTGheybatSum;

	private LabelX labelX73;

	private LabelX labelX8;

	private LabelX labelX69;

	private LabelX labelX10;

	private NumericUpDown NUHKasr3;

	private LabelX labelX29;

	private NumericUpDown NUHKasr2;

	private LabelX labelX30;

	private LabelX labelX72;

	private NumericUpDown NUHKasr1;

	private LabelX labelX68;

	private NumericUpDown NUMKasr3;

	private LabelX labelX12;

	private NumericUpDown NUMKasr2;

	private LabelX labelX51;

	private NumericUpDown NUMKasr1;

	private NumericUpDown NUHJome;

	private NumericUpDown NUHTatil;

	private NumericUpDown NUHNobat1;

	private NumericUpDown NU_Gheybat;

	private NumericUpDown NUHEzafe3;

	private NumericUpDown NUHShab;

	private NumericUpDown NUHEzafe2;

	private LabelX DotJome;

	private NumericUpDown NUHEzafe1;

	private LabelX LblKarkerd;

	private LabelX labelX65;

	private NumericUpDown NUHKarkerd;

	private LabelX labelX61;

	private LabelX labelX47;

	private LabelX labelX19;

	private NumericUpDown NUMEzafe3;

	private NumericUpDown NUMKarkerd;

	private NumericUpDown NUMEzafe2;

	private LabelX labelX46;

	private NumericUpDown NUMEzafe1;

	private NumericUpDown NUMShab;

	private LabelX labelX64;

	private NumericUpDown NUMJome;

	private NumericUpDown NUMTatil;

	private LabelX labelX60;

	private NumericUpDown NUMNobat1;

	private LabelX labelX45;

	private LabelX LblDot;

	private LabelX Counttaradodnaghes;

	private LabelX labelX52;

	private TextBoxFa TXTEzafePrice3;

	private TextBoxFa TXTJomePrice;

	private TextBoxFa TXTTatilPrice;

	private TextBoxFa TXTEzafePrice2;

	private TextBoxFa TXTKarkerdPrice;

	private TextBoxFa TXTEzafePrice1;

	private TextBoxFa TXTKasrPrice3;

	private TextBoxFa TXTShabPrice;

	private TextBoxFa TXTKasrPrice2;

	private TextBoxFa TXTNobatPrice1;

	private TextBoxFa TXTKasrPrice1;

	private LabelX labelX63;

	private TextBoxFa TXTGheybatPrice;

	private LabelX labelX58;

	private LabelX lblMablagh;

	private LabelX labelX7;

	private LabelX labelX9;

	private LabelX labelX71;

	private LabelX labelX11;

	private LabelX labelX67;

	private LabelX labelX13;

	private LabelX labelX26;

	private LabelX labelX27;

	private CheckBox ChkLockFish;

	private Button BtnAbort;

	private GroupBox groupBox1;

	private GroupBox GbxSalaryType;

	private LabelX labelX80;

	private LabelX labelX79;

	private LabelX LblNobatKari3;

	private TextBoxFa TXTNobatSum3;

	private LabelX labelX86;

	private NumericUpDown NUHNobat3;

	private LabelX labelX87;

	private NumericUpDown NUMNobat3;

	private TextBoxFa TXTNobatPrice3;

	private LabelX labelX88;

	private LabelX LblNobatKari2;

	private TextBoxFa TXTNobatSum2;

	private LabelX labelX82;

	private NumericUpDown NUHNobat2;

	private LabelX labelX83;

	private NumericUpDown NUMNobat2;

	private TextBoxFa TXTNobatPrice2;

	private LabelX labelX84;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
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
				_fillGridDetToken?.Cancel();
				_fillGridDetToken?.Dispose();
				_fillGridDetToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmFishHoghogh(string contextGuid, Fish pFish, Font font, bool isReadOnly)
		: base(contextGuid, true, true)
	{
		if (pFish == null)
		{
			throw new NullReferenceException("fish is null!!");
		}
		try
		{
			_font = font;
			((Control)this).Font = font;
			_clsFish = pFish;
			InitializeComponent();
			if (isReadOnly)
			{
				Frm_Text = "[مشاهده لاگ عملکرد کاربران ( قفط خواندنی ) اطلاعات فیش حقوقی]";
				((Control)BtnAccept).Enabled = false;
				((Control)BtnAccept).Text = "غیر قابل ویرایش";
				BtnAccept_Text = "[غیر قابل ویرایش]";
				((Control)ChkLockFish).Visible = false;
			}
			else if (pFish.Id > 0)
			{
				((Control)this).Text = "ویرایش فیش حقوقی";
				Frm_Text = "[ویرایش فیش حقوقی]";
			}
			else
			{
				((Control)this).Text = "صدور فیش حقوقی ";
				Frm_Text = "[صدور فیش حقوقی]";
				((Control)btnModifiedTaradod).Visible = true;
				((Control)btnNotifyMe).Width = 134;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 135);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			await FillObjectFromFishClsAsync(token);
			((FormExtention)this).ResetControlChanged();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		try
		{
			if (UserAttendance.SelectedUser != null && !UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.CanInsert(UserAttendance.SelectedUser?.AccessLevelMerg))
			{
				Utility.MessageBoxShowError("[با توجه به سطح دسترسی نرم افزار، شما مجوز صدور فیش حقوقی بر اساس اطلاعات حقوقی پرسنل ندارید]", ((CacheBaseForm)this).ContextGuid);
				((Form)this).Close();
				return;
			}
			if (_clsFish != null)
			{
				GharardadNew clsGharardad = _clsFish.ClsGharardad;
				if (clsGharardad != null && clsGharardad.ID <= 0)
				{
					Utility.MessageBoxShowError("[در بازه تاریخ مشخص شده برای این شخص قراردادی ثبت نشده است]", ((CacheBaseForm)this).ContextGuid);
					((Form)this).Close();
					return;
				}
			}
			((FormExtention)this).OnLoad(e);
			((Control)DGKosorat1).Font = _font;
			timerReadNotify.Tick += TimerReadNotify_Tick;
			base.objsCanEmpty.Add((Control)(object)NUHEzafe1);
			base.objsCanEmpty.Add((Control)(object)NUMEzafe1);
			base.objsCanEmpty.Add((Control)(object)TXTEzafePrice1);
			base.objsCanEmpty.Add((Control)(object)TXTEzafeSum1);
			base.objsCanEmpty.Add((Control)(object)NUHEzafe2);
			base.objsCanEmpty.Add((Control)(object)NUMEzafe2);
			base.objsCanEmpty.Add((Control)(object)TXTEzafePrice2);
			base.objsCanEmpty.Add((Control)(object)TXTEzafeSum2);
			base.objsCanEmpty.Add((Control)(object)NUHEzafe3);
			base.objsCanEmpty.Add((Control)(object)NUMEzafe3);
			base.objsCanEmpty.Add((Control)(object)TXTEzafePrice3);
			base.objsCanEmpty.Add((Control)(object)TXTEzafeSum3);
			base.objsCanEmpty.Add((Control)(object)NUHShab);
			base.objsCanEmpty.Add((Control)(object)NUMShab);
			base.objsCanEmpty.Add((Control)(object)TXTShabPrice);
			base.objsCanEmpty.Add((Control)(object)TXTShabSum);
			base.objsCanEmpty.Add((Control)(object)NUHNobat1);
			base.objsCanEmpty.Add((Control)(object)NUMNobat1);
			base.objsCanEmpty.Add((Control)(object)TXTNobatPrice1);
			base.objsCanEmpty.Add((Control)(object)TXTNobatSum1);
			base.objsCanEmpty.Add((Control)(object)NUHJome);
			base.objsCanEmpty.Add((Control)(object)NUMJome);
			base.objsCanEmpty.Add((Control)(object)NUHTatil);
			base.objsCanEmpty.Add((Control)(object)NUMTatil);
			base.objsCanEmpty.Add((Control)(object)TXTJomePrice);
			base.objsCanEmpty.Add((Control)(object)TXTJomeSum);
			base.objsCanEmpty.Add((Control)(object)TXTTatilPrice);
			base.objsCanEmpty.Add((Control)(object)TXTJarimePrice);
			base.objsCanEmpty.Add((Control)(object)TXTTatilSum);
			base.objsCanEmpty.Add((Control)(object)NUHVacation);
			base.objsCanEmpty.Add((Control)(object)NUMVacation);
			base.objsCanEmpty.Add((Control)(object)TXTVacationPrice);
			base.objsCanEmpty.Add((Control)(object)TXTVacationSum);
			base.objsCanEmpty.Add((Control)(object)NUH_ClocklyVacation);
			base.objsCanEmpty.Add((Control)(object)NUM_ClocklyVacation);
			base.objsCanEmpty.Add((Control)(object)NumDailyVacation);
			base.objsCanEmpty.Add((Control)(object)TXTVacationSumUsed);
			base.objsCanEmpty.Add((Control)(object)NU_Gheybat);
			base.objsCanEmpty.Add((Control)(object)TXTGheybatPrice);
			base.objsCanEmpty.Add((Control)(object)TXTGheybatSum);
			base.objsCanEmpty.Add((Control)(object)NUHKasr1);
			base.objsCanEmpty.Add((Control)(object)NUMKasr1);
			base.objsCanEmpty.Add((Control)(object)TXTKasrPrice1);
			base.objsCanEmpty.Add((Control)(object)TXTKasrSum1);
			base.objsCanEmpty.Add((Control)(object)NUHKasr2);
			base.objsCanEmpty.Add((Control)(object)NUMKasr2);
			base.objsCanEmpty.Add((Control)(object)TXTKasrPrice2);
			base.objsCanEmpty.Add((Control)(object)TXTKasrSum2);
			base.objsCanEmpty.Add((Control)(object)NUHKasr3);
			base.objsCanEmpty.Add((Control)(object)NUMKasr3);
			base.objsCanEmpty.Add((Control)(object)TXTKasrPrice3);
			base.objsCanEmpty.Add((Control)(object)TXTKasrSum3);
			base.objsCanEmpty.Add((Control)(object)NU_Mission);
			base.objsCanEmpty.Add((Control)(object)TXTMissionPrice);
			base.objsCanEmpty.Add((Control)(object)TXTMissionSum);
			base.objsCanEmpty.Add((Control)(object)TXTKolKarkerd);
			base.objsCanEmpty.Add((Control)(object)TXTKolMazaya);
			base.objsCanEmpty.Add((Control)(object)TXTKolKosorat);
			base.objsCanEmpty.Add((Control)(object)TXTKolMande);
			base.objsCanEmpty.Add((Control)(object)TXTDesc);
			((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 238);
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		try
		{
			if (!((Control)BtnAccept).Enabled)
			{
				((FormExtention)this).ResetControlChanged();
				((BaseForm)this).IsFormContentsChanged = false;
			}
			else
			{
				((FormExtention)this).OnFormClosing(e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnFormClosing", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 255);
		}
	}

	private void Rb_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnMonthly.Checked)
			{
				((Control)RbtnMonthly).Enabled = true;
				((Control)NUMKarkerd).Visible = false;
				((Control)NUHKarkerd).Visible = false;
				((Control)LblDot).Visible = false;
				((Control)LblKarkerd).Visible = false;
				NUHKarkerd.Value = _clsFish.CountKarKerd;
				NUMKarkerd.Value = 0m;
				((Control)TXTKarkerdSum).Text = _clsFish.SumKarkerd.ToString("N");
			}
			else if (RbtnDaily.Checked)
			{
				((Control)RbtnDaily).Enabled = true;
				((Control)NUHKarkerd).Visible = true;
				((Control)LblKarkerd).Visible = true;
				((Control)NUMKarkerd).Visible = false;
				((Control)LblDot).Visible = false;
				((Control)NUHKarkerd).Width = 100;
				NUHKarkerd.Maximum = 31m;
				if (_clsFish.CountKarKerd > 31)
				{
					Utility.MessageBoxShowError("[تعداد روزهای کارکرد بیشتر از 31 روز شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
				}
				else
				{
					NUHKarkerd.Value = ((_clsFish.CountKarKerd <= 31) ? _clsFish.CountKarKerd : 31);
				}
				NUMKarkerd.Value = 0m;
				((Control)TXTKarkerdSum).Text = _clsFish.SumKarkerd.ToString("N");
			}
			else
			{
				if (!RbtnSaati.Checked)
				{
					return;
				}
				((Control)RbtnSaati).Enabled = true;
				((Control)LblKarkerd).Visible = true;
				((Control)NUHKarkerd).Visible = true;
				((Control)NUMKarkerd).Visible = true;
				((Control)LblDot).Visible = true;
				((Control)NUHKarkerd).Width = 43;
				if (_clsFish != null)
				{
					if (_clsFish.ClsGharardad?.ShiftId == 0)
					{
						NUHKarkerd.Value = _clsFish.KarkerdMin / 60;
						NUMKarkerd.Value = _clsFish.KarkerdMin % 60;
						((Control)LblKarkerd).Text = Utilities.GetTranslate("[کارکرد کل]:");
					}
					else
					{
						NUHKarkerd.Value = _clsFish.MoazafiMin / 60;
						NUMKarkerd.Value = _clsFish.MoazafiMin % 60;
						((Control)LblKarkerd).Text = Utilities.GetTranslate("[کارکرد موظفی]:");
					}
					((Control)TXTKarkerdSum).Text = _clsFish.SumKarkerd.ToString("N");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Rb_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 321);
		}
	}

	private async void Rb_vacation_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			bool flag2;
			bool flag4;
			bool flag6;
			bool visible;
			if (RbtnEndGharardad.Checked)
			{
				((Control)RbtnEndGharardad).Enabled = true;
				NumericUpDown nUHVacation = NUHVacation;
				NumericUpDown nUMVacation = NUMVacation;
				TextBoxFa tXTVacationPrice = TXTVacationPrice;
				TextBoxFa tXTVacationSum = TXTVacationSum;
				flag2 = (((Control)TXTVacationSumUsed).Visible = false);
				flag4 = (((Control)tXTVacationSum).Visible = flag2);
				flag6 = (((Control)tXTVacationPrice).Visible = flag4);
				visible = (((Control)nUMVacation).Visible = flag6);
				((Control)nUHVacation).Visible = visible;
				LabelX lbl = Lbl1;
				LabelX lbl2 = Lbl2;
				LabelX lbl3 = Lbl3;
				LabelX lbl4 = Lbl4;
				flag2 = (((Control)Lbl5).Visible = false);
				flag4 = (((Control)lbl4).Visible = flag2);
				flag6 = (((Control)lbl3).Visible = flag4);
				visible = (((Control)lbl2).Visible = flag6);
				((Control)lbl).Visible = visible;
				MaskedTextBox txtVacationRemind = TxtVacationRemind;
				LabelX lbl5 = Lbl10;
				flag6 = (((Control)Lbl11).Visible = true);
				visible = (((Control)lbl5).Visible = flag6);
				((Control)txtVacationRemind).Visible = visible;
				MaskedTextBox txtVacationRemind2 = TxtVacationRemind;
				((Control)txtVacationRemind2).Text = (await _clsFish.GetVacationRemindAaync(token)).ToString("0.00");
				((Control)TXTVacationPrice).Text = "0";
				((Control)TXTVacationSum).Text = "0";
				((Control)TXTVacationSumUsed).Text = "0";
				return;
			}
			((Control)RbtnEndMonth).Enabled = true;
			NumericUpDown nUHVacation2 = NUHVacation;
			NumericUpDown nUMVacation2 = NUMVacation;
			TextBoxFa tXTVacationPrice2 = TXTVacationPrice;
			TextBoxFa tXTVacationSum2 = TXTVacationSum;
			flag2 = (((Control)TXTVacationSumUsed).Visible = true);
			flag4 = (((Control)tXTVacationSum2).Visible = flag2);
			flag6 = (((Control)tXTVacationPrice2).Visible = flag4);
			visible = (((Control)nUMVacation2).Visible = flag6);
			((Control)nUHVacation2).Visible = visible;
			LabelX lbl6 = Lbl1;
			LabelX lbl7 = Lbl2;
			LabelX lbl8 = Lbl3;
			LabelX lbl9 = Lbl4;
			flag2 = (((Control)Lbl5).Visible = true);
			flag4 = (((Control)lbl9).Visible = flag2);
			flag6 = (((Control)lbl8).Visible = flag4);
			visible = (((Control)lbl7).Visible = flag6);
			((Control)lbl6).Visible = visible;
			MaskedTextBox txtVacationRemind3 = TxtVacationRemind;
			LabelX lbl10 = Lbl10;
			flag6 = (((Control)Lbl11).Visible = false);
			visible = (((Control)lbl10).Visible = flag6);
			((Control)txtVacationRemind3).Visible = visible;
			if (_clsFish != null)
			{
				NUHVacation.Value = _clsFish.VacationMin / 60;
				NUMVacation.Value = _clsFish.VacationMin % 60;
				((Control)TXTVacationPrice).Text = _clsFish?.VacationPrice.ToString("N");
				((Control)TXTVacationSum).Text = _clsFish?.SumVacation.ToString("N");
				((Control)TXTVacationSumUsed).Text = _clsFish?.GetSumVacationUsed().ToString("N");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Rb_vacation_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 359);
		}
	}

	private async void Karkerd_Changed(object sender, EventArgs e)
	{
		try
		{
			switch (_clsFish.St)
			{
			case 2:
				_clsFish.CountKarKerd = (short)NUHKarkerd.Value;
				((Control)LblCountKarkard).Text = _clsFish.CountKarKerd.ToString();
				break;
			case 3:
				_clsFish.KarkerdMin = (int)(NUHKarkerd.Value * 60m + NUMKarkerd.Value);
				break;
			}
			_clsFish.MoazafiMin = (int)(NUHKarkerd.Value * 60m + NUMKarkerd.Value);
			_clsFish.KarkerdPrice = TXTKarkerdPrice.Value;
			((Control)TXTKarkerdSum).Text = _clsFish.SumKarkerd.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Karkerd_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 382);
		}
	}

	private async void EzafeKar_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.EzafeMin1 = (int)(NUHEzafe1.Value * 60m + NUMEzafe1.Value);
			_clsFish.EzafePrice1 = TXTEzafePrice1.Value;
			((Control)TXTEzafeSum1).Text = _clsFish.SumEzafeKar1.ToString("N");
			_clsFish.EzafeMin2 = (int)(NUHEzafe2.Value * 60m + NUMEzafe2.Value);
			_clsFish.EzafePrice2 = TXTEzafePrice2.Value;
			((Control)TXTEzafeSum2).Text = _clsFish.SumEzafeKar2.ToString("N");
			_clsFish.EzafeMin3 = (int)(NUHEzafe3.Value * 60m + NUMEzafe3.Value);
			_clsFish.EzafePrice3 = TXTEzafePrice3.Value;
			((Control)TXTEzafeSum3).Text = _clsFish.SumEzafeKar3.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EzafeKar_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 403);
		}
	}

	private async void ShabKari_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.ShabMin = (int)(NUHShab.Value * 60m + NUMShab.Value);
			_clsFish.ShabPrice = TXTShabPrice.Value;
			((Control)TXTShabSum).Text = _clsFish.SumShabKari.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ShabKari_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 415);
		}
	}

	private async void NobatKari1_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.NobatMin1 = (int)(NUHNobat1.Value * 60m + NUMNobat1.Value);
			_clsFish.NobatPrice1 = TXTNobatPrice1.Value;
			((Control)TXTNobatSum1).Text = _clsFish.SumNobatKari1.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NobatKari1_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 427);
		}
	}

	private async void NobatKari2_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.NobatMin2 = (int)(NUHNobat2.Value * 60m + NUMNobat2.Value);
			_clsFish.NobatPrice2 = TXTNobatPrice2.Value;
			((Control)TXTNobatSum2).Text = _clsFish.SumNobatKari2.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NobatKari2_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 439);
		}
	}

	private async void NobatKari3_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.NobatMin3 = (int)(NUHNobat3.Value * 60m + NUMNobat3.Value);
			_clsFish.NobatPrice3 = TXTNobatPrice3.Value;
			((Control)TXTNobatSum3).Text = _clsFish.SumNobatKari3.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NobatKari3_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 451);
		}
	}

	private async void JomeKari_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.JomeMin = (int)(NUHJome.Value * 60m + NUMJome.Value);
			_clsFish.JomePrice = TXTJomePrice.Value;
			((Control)TXTJomeSum).Text = _clsFish.SumJomeKari.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "JomeKari_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 463);
		}
	}

	private async void TatilKari_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.TatilMin = (int)(NUHTatil.Value * 60m + NUMTatil.Value);
			_clsFish.TatilPrice = TXTTatilPrice.Value;
			((Control)TXTTatilSum).Text = _clsFish.SumTatilKari.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TatilKari_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 475);
		}
	}

	private async void Jarime_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.JarimeRoz = int.Parse(((Control)NU_Jarime).Text);
			_clsFish.JarimePrice = TXTJarimePrice.Value;
			((Control)TXTJarimeSum).Text = _clsFish.SumJarime.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Jarime_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 487);
		}
	}

	private async void Vacation_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.VacationMin = (int)(NUHVacation.Value * 60m + NUMVacation.Value);
			_clsFish.VacationPrice = TXTVacationPrice.Value;
			((Control)TXTVacationSum).Text = _clsFish.SumVacation.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Vacation_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 499);
		}
	}

	private async void VacationUsed_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.VacationMinUsed = (int)(NUH_ClocklyVacation.Value * 60m + NUM_ClocklyVacation.Value);
			_clsFish.CountMorakhasi = (short)NumDailyVacation.Value;
			((Control)TXTVacationSumUsed).Text = _clsFish.GetSumVacationUsed().ToString("N");
			((Control)LblCountMorHourly).Text = (_clsFish.VacationMinUsed / 60).ToString().PadLeft(2, '0') + ":" + (_clsFish.VacationMinUsed % 60).ToString().PadLeft(2, '0');
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "VacationUsed_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 513);
		}
	}

	private async void KasrKar_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.KasrMin1 = (int)(NUHKasr1.Value * 60m + NUMKasr1.Value);
			_clsFish.KasrPrice1 = TXTKasrPrice1.Value;
			((Control)TXTKasrSum1).Text = _clsFish.SumKasrKar1.ToString("N");
			_clsFish.KasrMin2 = (int)(NUHKasr2.Value * 60m + NUMKasr2.Value);
			_clsFish.KasrPrice2 = TXTKasrPrice2.Value;
			((Control)TXTKasrSum2).Text = _clsFish.SumKasrKar2.ToString("N");
			_clsFish.KasrMin3 = (int)(NUHKasr3.Value * 60m + NUMKasr3.Value);
			_clsFish.KasrPrice3 = TXTKasrPrice3.Value;
			((Control)TXTKasrSum3).Text = _clsFish.SumKasrKar3.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "KasrKar_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 531);
		}
	}

	private async void Mission_Changed(object sender, EventArgs e)
	{
		try
		{
			double.TryParse(((Control)NU_Mission).Text.Trim(), out var result);
			_clsFish.MissionRoz = result;
			_clsFish.MissionPrice = TXTMissionPrice.Value;
			((Control)TXTMissionSum).Text = _clsFish.SumMission.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Mission_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 544);
		}
	}

	private async void Gheybat_Changed(object sender, EventArgs e)
	{
		try
		{
			_clsFish.GheybatRoze = (int)NU_Gheybat.Value;
			_clsFish.GheybatPrice = TXTGheybatPrice.Value;
			((Control)TXTGheybatSum).Text = _clsFish.SumGheybat.ToString("N");
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Gheybat_Changed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 555);
		}
	}

	private async void BtnEditMazaya_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGMazaya1).Focus();
			if (((DataGridView)DGMazaya1).RowCount > 0 && ((DataGridView)DGMazaya1).CurrentRow != null)
			{
				FishDetJson xx = ((List<FishDetJson>)fishMDetBindingSource.DataSource)[((DataGridViewBand)((DataGridView)DGMazaya1).CurrentRow).Index];
				FrmAccountEdit frm = new FrmAccountEdit(((CacheBaseForm)this).ContextGuid, xx);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					int index = _clsFish.ListFishDet.IndexOf(xx);
					FishDetJson fishDetJson = _clsFish.ListFishDet[index];
					decimal tmpValue = (_clsFish.ListFishDet[index].Credit = frm.AccountChange);
					fishDetJson.TmpValue = tmpValue;
					_clsFish.ListFishDet[index].Description = frm.DescChange;
					await FillGridsAsync();
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEditMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 577);
		}
	}

	private async void BtnEditKosorat_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGKosorat1).Focus();
			if (((DataGridView)DGKosorat1).RowCount > 0 && ((DataGridView)DGKosorat1).CurrentRow != null)
			{
				FishDetJson xx = ((List<FishDetJson>)fishKDetBindingSource.DataSource)[((DataGridViewBand)((DataGridView)DGKosorat1).CurrentRow).Index];
				FrmAccountEdit frm = new FrmAccountEdit(((CacheBaseForm)this).ContextGuid, xx);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					int index = _clsFish.ListFishDet.IndexOf(xx);
					FishDetJson fishDetJson = _clsFish.ListFishDet[index];
					decimal tmpValue = (_clsFish.ListFishDet[index].Debit = frm.AccountChange);
					fishDetJson.TmpValue = tmpValue;
					_clsFish.ListFishDet[index].Description = frm.DescChange;
					_clsFish.ListFishDet[index].DisableCompute = true;
					await FillGridsAsync();
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEditKosorat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 598);
		}
	}

	private async void BtnAddMazaya_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)DGMazaya1).Focus();
			FrmHesabList frmHesabList = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 1);
			((Form)frmHesabList).WindowState = (FormWindowState)0;
			FrmHesabList frm = frmHesabList;
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
			{
				return;
			}
			string selectedItems = frm.SelectedItems;
			if (selectedItems == "")
			{
				return;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			string[] array = selectedItems.Split(new char[1] { char.Parse(";") });
			foreach (string s in array)
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				FishHesab pFishHesab = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, int.Parse(s), token);
				FishDetJson item = new FishDetJson(((CacheBaseForm)this).ContextGuid, pFishHesab);
				_clsFish.ListFishDet.Add(item);
			}
			if (!token.IsCancellationRequested && !IsDisposed)
			{
				await FillGridsAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAddMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 623);
		}
	}

	private async void BtnAddKosorat_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)DGKosorat1).Focus();
			FrmHesabList frmHesabList = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2);
			((Form)frmHesabList).WindowState = (FormWindowState)0;
			FrmHesabList frm = frmHesabList;
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
			{
				return;
			}
			string selectedItems = frm.SelectedItems;
			if (!(selectedItems == ""))
			{
				string[] array = selectedItems.Split(new char[1] { char.Parse(";") });
				foreach (string s in array)
				{
					FishHesab pFishHesab = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, int.Parse(s));
					FishDetJson item = new FishDetJson(((CacheBaseForm)this).ContextGuid, pFishHesab);
					_clsFish.ListFishDet.Add(item);
				}
				await FillGridsAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAddKosorat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 644);
		}
	}

	private async void BtnDeleteMazaya_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGMazaya1).Rows.Count > 0)
			{
				((Control)DGMazaya1).Focus();
				FishDetJson item = ((List<FishDetJson>)fishMDetBindingSource.DataSource)[((DataGridViewBand)((DataGridView)DGMazaya1).CurrentRow).Index];
				_clsFish.ListFishDet.Remove(item);
				await FillGridsAsync();
			}
			else
			{
				Utility.MessageBoxShowError("[مزایایی جهت حذف کردن وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
			}
			TXTKolMazaya.Value = _clsFish.SumMazaya;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDeleteMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 664);
		}
	}

	private async void BtnDelKosor_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGKosorat1).Rows.Count > 0)
			{
				((Control)DGKosorat1).Focus();
				if (((DataGridView)DGKosorat1).CurrentRow != null)
				{
					FishDetJson item = ((List<FishDetJson>)fishKDetBindingSource.DataSource)[((DataGridViewBand)((DataGridView)DGKosorat1).CurrentRow).Index];
					_clsFish.ListFishDet.Remove(item);
				}
				await FillGridsAsync();
			}
			else
			{
				Utility.MessageBoxShowError("[کسوراتی جهت حذف کردن وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
			}
			TXTKolKosorat.Value = _clsFish.SumKosorat;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelKosor_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 688);
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
			await IsValidAndSaveAsync(isSave: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 702);
		}
	}

	private async void BtnNotifyMe_Click(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await PayRollCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _clsFish, isShowMessage: true, isShowButtons: false, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnNotifyMe_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 714);
		}
	}

	private async void BtnModifiedTaradod_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			FrmRptTaradodFull frm1 = new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, _clsFish.ClsTaradod);
			await ((CacheBaseForm)frm1).ShowAsync((IWin32Window)(object)this);
			if (!token.IsCancellationRequested)
			{
				_clsFish = await Fish.FactoryAsync(((CacheBaseForm)this).ContextGuid, frm1.ClsTaradodFull, token);
				((Component)(object)frm1).Dispose();
				ReleaseEvent(release: true);
				await FillObjectFromFishClsAsync();
				ReleaseEvent(release: false);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnModifiedTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 733);
		}
	}

	private async Task FillGridsAsync()
	{
		try
		{
			_fillGridDetToken?.Cancel();
			_fillGridDetToken = new CancellationTokenSource();
			CancellationToken token = _fillGridDetToken.Token;
			await Task.Delay(500);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			await _clsFish.CalculateFormulaInFishAsync(token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			decimal includedInsurance = await _clsFish.GetIncludedInsuranceAsync(token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			decimal includedTax = await _clsFish.GetIncludedTaxAsync(token);
			List<FishDetJson> lstFishDetMazaya = _clsFish.ListFishDet.Where((FishDetJson e1) => e1.TypeAccount.Equals(1)).ToList();
			List<FishDetJson> lstFishDetKosorat = _clsFish.ListFishDet.Where((FishDetJson e1) => e1.TypeAccount.Equals(2)).ToList();
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						fishMDetBindingSource.DataSource = lstFishDetMazaya;
						fishKDetBindingSource.DataSource = lstFishDetKosorat;
						((Control)TXTKolMazaya).Text = _clsFish.SumMazaya.ToString("N");
						((Control)TXTKolKosorat).Text = _clsFish.SumKosorat.ToString("N");
						((Control)TxtIncludedInsurance).Text = includedInsurance.ToString("N");
						((Control)TxtIncludedTax).Text = includedTax.ToString("N");
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				await RecalcMandehAsync(token);
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await IsValidAndSaveAsync(isSave: false, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillGridsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 783);
		}
	}

	private void ReleaseEvent(bool release)
	{
		try
		{
			if (release)
			{
				ChkLockFish.CheckedChanged -= ChkLockFish_CheckedChanged;
				NUHKarkerd.ValueChanged -= Karkerd_Changed;
				NUMKarkerd.ValueChanged -= Karkerd_Changed;
				((Control)TXTKarkerdPrice).TextChanged -= Karkerd_Changed;
				NUHEzafe1.ValueChanged -= EzafeKar_Changed;
				NUMEzafe1.ValueChanged -= EzafeKar_Changed;
				((Control)TXTEzafePrice1).TextChanged -= EzafeKar_Changed;
				NUHEzafe2.ValueChanged -= EzafeKar_Changed;
				NUMEzafe2.ValueChanged -= EzafeKar_Changed;
				((Control)TXTEzafePrice2).TextChanged -= EzafeKar_Changed;
				NUHEzafe3.ValueChanged -= EzafeKar_Changed;
				NUMEzafe3.ValueChanged -= EzafeKar_Changed;
				((Control)TXTEzafePrice3).TextChanged -= EzafeKar_Changed;
				NUHShab.ValueChanged -= ShabKari_Changed;
				NUMShab.ValueChanged -= ShabKari_Changed;
				((Control)TXTShabPrice).TextChanged -= ShabKari_Changed;
				NUHNobat1.ValueChanged -= NobatKari1_Changed;
				NUMNobat1.ValueChanged -= NobatKari1_Changed;
				((Control)TXTNobatPrice1).TextChanged -= NobatKari1_Changed;
				NUHNobat2.ValueChanged -= NobatKari2_Changed;
				NUMNobat2.ValueChanged -= NobatKari2_Changed;
				((Control)TXTNobatPrice2).TextChanged -= NobatKari2_Changed;
				NUHNobat3.ValueChanged -= NobatKari3_Changed;
				NUMNobat3.ValueChanged -= NobatKari3_Changed;
				((Control)TXTNobatPrice3).TextChanged -= NobatKari3_Changed;
				NUHJome.ValueChanged -= JomeKari_Changed;
				NUMJome.ValueChanged -= JomeKari_Changed;
				((Control)TXTJomePrice).TextChanged -= JomeKari_Changed;
				NUHTatil.ValueChanged -= TatilKari_Changed;
				NUMTatil.ValueChanged -= TatilKari_Changed;
				((Control)TXTTatilPrice).TextChanged -= TatilKari_Changed;
				NU_Jarime.TextChanged -= Jarime_Changed;
				((Control)TXTJarimePrice).TextChanged -= Jarime_Changed;
				((Control)NU_Mission).TextChanged -= Mission_Changed;
				((Control)TXTMissionPrice).TextChanged -= Mission_Changed;
				NUHVacation.ValueChanged -= Vacation_Changed;
				NUMVacation.ValueChanged -= Vacation_Changed;
				((Control)TXTVacationPrice).TextChanged -= Vacation_Changed;
				NUH_ClocklyVacation.ValueChanged -= VacationUsed_Changed;
				NUM_ClocklyVacation.ValueChanged -= VacationUsed_Changed;
				NumDailyVacation.ValueChanged -= VacationUsed_Changed;
				NU_Gheybat.ValueChanged -= Gheybat_Changed;
				((Control)TXTGheybatPrice).TextChanged -= Gheybat_Changed;
				NUHKasr1.ValueChanged -= KasrKar_Changed;
				NUMKasr1.ValueChanged -= KasrKar_Changed;
				((Control)TXTKasrPrice1).TextChanged -= KasrKar_Changed;
				NUHKasr2.ValueChanged -= KasrKar_Changed;
				NUMKasr2.ValueChanged -= KasrKar_Changed;
				((Control)TXTKasrPrice2).TextChanged -= KasrKar_Changed;
				NUHKasr3.ValueChanged -= KasrKar_Changed;
				NUMKasr3.ValueChanged -= KasrKar_Changed;
				((Control)TXTKasrPrice3).TextChanged -= KasrKar_Changed;
				((Control)TxtVacationRemind).TextChanged -= TxtVacationRemind_TextChanged;
				NU_workDayBime.ValueChanged -= NU_workDayBime_ValueChanged;
				NU_workDayMaliat.ValueChanged -= NU_workDayMaliat_ValueChanged;
			}
			else
			{
				ChkLockFish.CheckedChanged += ChkLockFish_CheckedChanged;
				NUHKarkerd.ValueChanged += Karkerd_Changed;
				NUMKarkerd.ValueChanged += Karkerd_Changed;
				((Control)TXTKarkerdPrice).TextChanged += Karkerd_Changed;
				NUHEzafe1.ValueChanged += EzafeKar_Changed;
				NUMEzafe1.ValueChanged += EzafeKar_Changed;
				((Control)TXTEzafePrice1).TextChanged += EzafeKar_Changed;
				NUHEzafe2.ValueChanged += EzafeKar_Changed;
				NUMEzafe2.ValueChanged += EzafeKar_Changed;
				((Control)TXTEzafePrice2).TextChanged += EzafeKar_Changed;
				NUHEzafe3.ValueChanged += EzafeKar_Changed;
				NUMEzafe3.ValueChanged += EzafeKar_Changed;
				((Control)TXTEzafePrice3).TextChanged += EzafeKar_Changed;
				NUHShab.ValueChanged += ShabKari_Changed;
				NUMShab.ValueChanged += ShabKari_Changed;
				((Control)TXTShabPrice).TextChanged += ShabKari_Changed;
				NUHNobat1.ValueChanged += NobatKari1_Changed;
				NUMNobat1.ValueChanged += NobatKari1_Changed;
				((Control)TXTNobatPrice1).TextChanged += NobatKari1_Changed;
				NUHNobat2.ValueChanged += NobatKari2_Changed;
				NUMNobat2.ValueChanged += NobatKari2_Changed;
				((Control)TXTNobatPrice2).TextChanged += NobatKari2_Changed;
				NUHNobat3.ValueChanged += NobatKari3_Changed;
				NUMNobat3.ValueChanged += NobatKari3_Changed;
				((Control)TXTNobatPrice3).TextChanged += NobatKari3_Changed;
				NUHJome.ValueChanged += JomeKari_Changed;
				NUMJome.ValueChanged += JomeKari_Changed;
				((Control)TXTJomePrice).TextChanged += JomeKari_Changed;
				NUHTatil.ValueChanged += TatilKari_Changed;
				NUMTatil.ValueChanged += TatilKari_Changed;
				((Control)TXTTatilPrice).TextChanged += TatilKari_Changed;
				NU_Jarime.TextChanged += Jarime_Changed;
				((Control)TXTJarimePrice).TextChanged += Jarime_Changed;
				((Control)NU_Mission).TextChanged += Mission_Changed;
				((Control)TXTMissionPrice).TextChanged += Mission_Changed;
				NUHVacation.ValueChanged += Vacation_Changed;
				NUMVacation.ValueChanged += Vacation_Changed;
				((Control)TXTVacationPrice).TextChanged += Vacation_Changed;
				NUH_ClocklyVacation.ValueChanged += VacationUsed_Changed;
				NUM_ClocklyVacation.ValueChanged += VacationUsed_Changed;
				NumDailyVacation.ValueChanged += VacationUsed_Changed;
				NU_Gheybat.ValueChanged += Gheybat_Changed;
				((Control)TXTGheybatPrice).TextChanged += Gheybat_Changed;
				NUHKasr1.ValueChanged += KasrKar_Changed;
				NUMKasr1.ValueChanged += KasrKar_Changed;
				((Control)TXTKasrPrice1).TextChanged += KasrKar_Changed;
				NUHKasr2.ValueChanged += KasrKar_Changed;
				NUMKasr2.ValueChanged += KasrKar_Changed;
				((Control)TXTKasrPrice2).TextChanged += KasrKar_Changed;
				NUHKasr3.ValueChanged += KasrKar_Changed;
				NUMKasr3.ValueChanged += KasrKar_Changed;
				((Control)TXTKasrPrice3).TextChanged += KasrKar_Changed;
				((Control)TxtVacationRemind).TextChanged += TxtVacationRemind_TextChanged;
				NU_workDayBime.ValueChanged += NU_workDayBime_ValueChanged;
				NU_workDayMaliat.ValueChanged += NU_workDayMaliat_ValueChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReleaseEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 909);
		}
	}

	private async Task IsValidAndSaveAsync(bool isSave, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			DialogResult val = await PayRollCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _clsFish, isSave, isShowButtons: true, token);
			((Control)btnNotifyMe).Visible = false;
			timerReadNotify.Stop();
			DialogResult val2 = val;
			List<object> listFishDetForReport;
			string extraVar;
			FrmPrint frm;
			switch (val2 - 1)
			{
			case 3:
			case 6:
				((Control)btnNotifyMe).Visible = true;
				timerReadNotify.Start();
				break;
			case 1:
			case 2:
				((Control)btnNotifyMe).Visible = true;
				timerReadNotify.Start();
				((Form)this).DialogResult = val;
				((Form)this).Close();
				break;
			case 4:
				if (!isSave)
				{
					break;
				}
				goto IL_0147;
			case 0:
				if (!isSave)
				{
					break;
				}
				goto IL_0147;
			case 5:
				{
					if (!isSave)
					{
						break;
					}
					goto IL_0147;
				}
				IL_0147:
				((Control)btnNotifyMe).Visible = false;
				if (_clsFish.EditST == 1)
				{
					((FormExtention)this).ResetControlChanged();
					FMessageBox.Show("با توجه به اینکه فیش قفل می باشد اجازه ذخیره تغییرات ندارید");
					((Form)this).Close();
					break;
				}
				await _clsFish.SaveAsync(token);
				((FormExtention)this).ResetControlChanged();
				((BaseForm)this).IsFormContentsChanged = false;
				listFishDetForReport = _clsFish.ListFishDetForReport;
				extraVar = "0";
				if (_clsFish.ClsGharardad != null)
				{
					extraVar = _clsFish.ClsGharardad.No;
				}
				frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)6, listFishDetForReport, null, _clsFish.Id, extraVar);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsValidAndSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 957);
		}
	}

	private async Task RecalcMandehAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			decimal kolKarkard = _clsFish.GetSumKolKarkerd();
			decimal sumKhales = _clsFish.SumKhales;
			decimal includedInsurance = await _clsFish.GetIncludedInsuranceAsync(token);
			decimal num = await _clsFish.GetIncludedTaxAsync(token);
			((Control)TXTKolKarkerd).Text = kolKarkard.ToString("N");
			((Control)TXTKolKarkerd).BackColor = ((kolKarkard < 0m) ? Color.LightPink : Color.LightGreen);
			((Control)TXTKolKarkerd).ForeColor = ((kolKarkard < 0m) ? Color.Maroon : Color.Black);
			((Control)TXTKolMande).Text = sumKhales.ToString("N");
			((Control)TXTKolMande).BackColor = ((sumKhales < 0m) ? Color.LightPink : Color.LightGreen);
			((Control)TXTKolMande).ForeColor = ((sumKhales < 0m) ? Color.Maroon : Color.Black);
			((Control)TxtIncludedInsurance).Text = includedInsurance.ToString("N");
			((Control)TxtIncludedTax).Text = num.ToString("N");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RecalcMandehAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 980);
		}
	}

	private async Task FillObjectFromFishClsAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			Fish clsFish = _clsFish;
			if (clsFish.ClsTaradod == null)
			{
				Fish fish = clsFish;
				fish.ClsTaradod = await new TaradodFull(((CacheBaseForm)this).ContextGuid).InitializeAsync(_clsFish.DateStartM, _clsFish.DateEndM, _clsFish.ClsPerson, null, checkValidationVorodAndKHoroj: false, token);
			}
			Person person = _clsFish.ClsPerson;
			short sumKarkard = _clsFish.CountKarKerd;
			short countVacationWithHoghogh = _clsFish.CountMorakhasiWithSalary;
			short countGheybat = _clsFish.CountGheybat;
			short countNaghes = _clsFish.CountNaghes;
			short countKarKerdBime = _clsFish.CountWorkdayBime;
			short countKarKerdMaliat = _clsFish.CountWorkdayMaliyat;
			short countMorakhasi = _clsFish.CountMorakhasi;
			decimal includedInsurance = await _clsFish.GetIncludedInsuranceAsync(token);
			decimal includedTax = await _clsFish.GetIncludedTaxAsync(token);
			decimal sumKolKarkerd = _clsFish.GetSumKolKarkerd();
			decimal sumKhales = _clsFish.SumKhales;
			int fishNo = _clsFish.FishNo;
			if (fishNo <= 0)
			{
				fishNo = await Fish.NextCodeAsync(((CacheBaseForm)this).ContextGuid);
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						ReleaseEvent(release: true);
						if (_clsFish.EditST == 1)
						{
							((Control)BtnAccept).Enabled = false;
						}
						_clsFish.FishNo = fishNo;
						((Control)TXTFishNO).Text = _clsFish.FishNo.ToString();
						((Control)this).Text = Utilities.GetTranslate(Frm_Text);
						((Control)BtnAccept).Text = Utilities.GetTranslate(BtnAccept_Text);
						ChkLockFish.Checked = _clsFish.EditST == 1;
						((Control)LblCodeTafsil).Text = ((PersonBase)person).CodeTafsil.ToString();
						((Control)LblFName).Text = ((PersonBase)person).FatherName;
						((Control)LblName).Text = ((PersonBase)person).FullName;
						((Control)LblCodeMeli).Text = ((PersonBase)person).CodeMeli;
						((Control)TXTDate).Text = Calendar.MiladiToCalture(_clsFish.DateM);
						((Control)LblCountKarkard).Text = sumKarkard.ToString();
						((Control)LblCountKol).Text = _clsFish.CountDaysOfMonth.ToString();
						((Control)LblCountMorWithHoghogh).Text = countVacationWithHoghogh.ToString();
						((Control)LblCountMorHourly).Text = ((_clsFish.VacationMinUsed > 0) ? ((_clsFish.VacationMinUsed / 60).ToString().PadLeft(2, '0') + ":" + (_clsFish.VacationMinUsed % 60).ToString().PadLeft(2, '0')) : "00:00");
						((Control)LblCountMorNotHoghogh).Text = _clsFish.CountMorakhasiNotSalary.ToString();
						((Control)LblCountGheybat).Text = countGheybat.ToString();
						((Control)Counttaradodnaghes).Text = countNaghes.ToString();
						((Control)TXTKarkerdPrice).Text = _clsFish.KarkerdPrice.ToString("N");
						((Control)NU_workDayBime).Text = countKarKerdBime.ToString();
						((Control)NU_workDayMaliat).Text = countKarKerdMaliat.ToString();
						switch (_clsFish.St)
						{
						case 1:
							RbtnMonthly.Checked = true;
							break;
						case 2:
							RbtnDaily.Checked = true;
							break;
						case 3:
							RbtnSaati.Checked = true;
							break;
						}
						if (_clsFish.EzafeMin1 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت اضافه کاری اول وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHEzafe1.Value = _clsFish.EzafeMin1 / 60;
							NUMEzafe1.Value = _clsFish.EzafeMin1 % 60;
						}
						((Control)TXTEzafePrice1).Text = _clsFish.EzafePrice1.ToString("N");
						((Control)TXTEzafeSum1).Text = _clsFish.SumEzafeKar1.ToString("N");
						if (_clsFish.EzafeMin2 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت اضافه کاری وسط وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHEzafe2.Value = _clsFish.EzafeMin2 / 60;
							NUMEzafe2.Value = _clsFish.EzafeMin2 % 60;
						}
						((Control)TXTEzafePrice2).Text = _clsFish.EzafePrice2.ToString("N");
						((Control)TXTEzafeSum2).Text = _clsFish.SumEzafeKar2.ToString("N");
						if (_clsFish.EzafeMin3 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت اضافه کاری آخر وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHEzafe3.Value = _clsFish.EzafeMin3 / 60;
							NUMEzafe3.Value = _clsFish.EzafeMin3 % 60;
						}
						((Control)TXTEzafePrice3).Text = _clsFish.EzafePrice3.ToString("N");
						((Control)TXTEzafeSum3).Text = _clsFish.SumEzafeKar3.ToString("N");
						if (_clsFish.ShabMin < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت شب کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHShab.Value = _clsFish.ShabMin / 60;
							NUMShab.Value = _clsFish.ShabMin % 60;
						}
						((Control)TXTShabPrice).Text = _clsFish.ShabPrice.ToString("N");
						((Control)TXTShabSum).Text = _clsFish.SumShabKari.ToString("N");
						if (_clsFish.NobatMin1 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت نوبت کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHNobat1.Value = _clsFish.NobatMin1 / 60;
							NUMNobat1.Value = _clsFish.NobatMin1 % 60;
						}
						((Control)TXTNobatPrice1).Text = _clsFish.NobatPrice1.ToString("N");
						((Control)TXTNobatSum1).Text = _clsFish.SumNobatKari1.ToString("N");
						if (_clsFish.NobatMin2 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت نوبت کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHNobat2.Value = _clsFish.NobatMin2 / 60;
							NUMNobat2.Value = _clsFish.NobatMin2 % 60;
						}
						((Control)TXTNobatPrice2).Text = _clsFish.NobatPrice2.ToString("N");
						((Control)TXTNobatSum2).Text = _clsFish.SumNobatKari2.ToString("N");
						if (_clsFish.NobatMin3 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت نوبت کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHNobat3.Value = _clsFish.NobatMin3 / 60;
							NUMNobat3.Value = _clsFish.NobatMin3 % 60;
						}
						((Control)TXTNobatPrice3).Text = _clsFish.NobatPrice3.ToString("N");
						((Control)TXTNobatSum3).Text = _clsFish.SumNobatKari3.ToString("N");
						if (_clsFish.JomeMin < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت جمعه کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHJome.Value = _clsFish.JomeMin / 60;
							NUMJome.Value = _clsFish.JomeMin % 60;
						}
						((Control)TXTJomePrice).Text = _clsFish.JomePrice.ToString("N");
						((Control)TXTJomeSum).Text = _clsFish.SumJomeKari.ToString("N");
						if (_clsFish.TatilMin < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت تعطیل کاری منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHTatil.Value = _clsFish.TatilMin / 60;
							NUMTatil.Value = _clsFish.TatilMin % 60;
						}
						((Control)TXTTatilPrice).Text = _clsFish.TatilPrice.ToString("N");
						((Control)TXTJarimePrice).Text = _clsFish.JarimePrice.ToString("N");
						((Control)TXTTatilSum).Text = _clsFish.SumTatilKari.ToString("N");
						if (_clsFish.VacationMinUsed < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت مرخصی استفاده شده منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUH_ClocklyVacation.Value = _clsFish.VacationMinUsed / 60;
							NUM_ClocklyVacation.Value = _clsFish.VacationMinUsed % 60;
						}
						if (countMorakhasi < 0)
						{
							Utility.MessageBoxShowError("[مجموع روزهای مرخصی استفاده شده منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NumDailyVacation.Value = countMorakhasi;
						}
						switch (_clsFish.VacationST)
						{
						case 0:
							RbtnEndGharardad.Checked = true;
							break;
						case 1:
							RbtnEndMonth.Checked = true;
							break;
						}
						if (_clsFish.GheybatRoze < 0)
						{
							Utility.MessageBoxShowError("[[مجموع روزهای غیبت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NU_Gheybat.Value = _clsFish.GheybatRoze;
						}
						((Control)TXTGheybatPrice).Text = _clsFish.GheybatPrice.ToString("N");
						((Control)TXTGheybatSum).Text = _clsFish.SumGheybat.ToString("N");
						if (_clsFish.KasrMin1 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت کسرکار اول وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHKasr1.Value = _clsFish.KasrMin1 / 60;
							NUMKasr1.Value = _clsFish.KasrMin1 % 60;
						}
						((Control)TXTKasrPrice1).Text = _clsFish.KasrPrice1.ToString("N");
						((Control)TXTKasrSum1).Text = _clsFish.SumKasrKar1.ToString("N");
						if (_clsFish.KasrMin2 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت کسرکار وسط وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHKasr2.Value = _clsFish.KasrMin2 / 60;
							NUMKasr2.Value = _clsFish.KasrMin2 % 60;
						}
						((Control)TXTKasrPrice2).Text = _clsFish.KasrPrice2.ToString("N");
						((Control)TXTKasrSum2).Text = _clsFish.SumKasrKar2.ToString("N");
						if (_clsFish.KasrMin3 < 0)
						{
							Utility.MessageBoxShowError("[مجموع ساعت کسرکار آخر وقت منفی شده است. در فرم گزارش کارکرد، علت بررسی شود]", ((CacheBaseForm)this).ContextGuid);
						}
						else
						{
							NUHKasr3.Value = _clsFish.KasrMin3 / 60;
							NUMKasr3.Value = _clsFish.KasrMin3 % 60;
						}
						((Control)TXTKasrPrice3).Text = _clsFish.KasrPrice3.ToString("N");
						((Control)TXTKasrSum3).Text = _clsFish.SumKasrKar3.ToString("N");
						((Control)NU_Mission).Text = _clsFish.MissionRoz.ToString();
						((Control)TXTMissionPrice).Text = _clsFish.MissionPrice.ToString("N");
						((Control)TXTMissionSum).Text = _clsFish.SumMission.ToString("N");
						((Control)NU_Jarime).Text = _clsFish.JarimeRoz.ToString();
						((Control)TXTJarimePrice).Text = _clsFish.JarimePrice.ToString("N");
						((Control)TXTJarimeSum).Text = _clsFish.SumJarime.ToString("N");
						((Control)TXTDesc).Text = _clsFish.Description;
						((Control)LblfromDate).Text = Calendar.MiladiToCalture(_clsFish.DateStartM);
						((Control)LblTadate).Text = Calendar.MiladiToCalture(_clsFish.DateEndM);
						((Control)TXTKolMazaya).Text = _clsFish.SumMazaya.ToString("N");
						((Control)TXTKolKosorat).Text = _clsFish.SumKosorat.ToString("N");
						((Control)TxtIncludedInsurance).Text = includedInsurance.ToString("N");
						((Control)TxtIncludedTax).Text = includedTax.ToString("N");
						((Control)TXTKolKarkerd).Text = sumKolKarkerd.ToString("N");
						((Control)TXTKolKarkerd).BackColor = ((sumKolKarkerd < 0m) ? Color.LightPink : Color.LightGreen);
						((Control)TXTKolMande).Text = sumKhales.ToString("N");
						((Control)TXTKolMande).BackColor = ((sumKhales < 0m) ? Color.LightPink : Color.LightGreen);
						((FormExtention)this).ResetControlChanged();
						ReleaseEvent(release: false);
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				await FillGridsAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "FillObjectFromFishCls", "FillObjectFromFishClsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1249);
		}
	}

	private void TimerReadNotify_Tick(object sender, EventArgs e)
	{
		try
		{
			((Control)btnNotifyMe).BackColor = ((((Control)btnNotifyMe).BackColor == Color.Fuchsia) ? Color.DarkMagenta : Color.Fuchsia);
			((Control)btnNotifyMe).Refresh();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TimerReadNotify_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1261);
		}
	}

	private void TXT_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBoxFa tXTEzafeSum = TXTEzafeSum1;
			TextBoxFa tXTEzafeSum2 = TXTEzafeSum2;
			TextBoxFa tXTEzafeSum3 = TXTEzafeSum3;
			TextBoxFa tXTGheybatSum = TXTGheybatSum;
			TextBoxFa tXTJomeSum = TXTJomeSum;
			TextBoxFa tXTKarkerdSum = TXTKarkerdSum;
			TextBoxFa tXTKasrSum = TXTKasrSum1;
			TextBoxFa tXTKasrSum2 = TXTKasrSum2;
			TextBoxFa tXTKasrSum3 = TXTKasrSum3;
			TextBoxFa tXTMissionSum = TXTMissionSum;
			TextBoxFa tXTNobatSum = TXTNobatSum1;
			TextBoxFa tXTShabSum = TXTShabSum;
			TextBoxFa tXTVacationSum = TXTVacationSum;
			TextBoxFa tXTTatilSum = TXTTatilSum;
			Color color = (((Control)TXTJarimeSum).BackColor = Color.LightGray);
			Color color3 = (((Control)tXTTatilSum).BackColor = color);
			Color color5 = (((Control)tXTVacationSum).BackColor = color3);
			Color color7 = (((Control)tXTShabSum).BackColor = color5);
			Color color9 = (((Control)tXTNobatSum).BackColor = color7);
			Color color11 = (((Control)tXTMissionSum).BackColor = color9);
			Color color13 = (((Control)tXTKasrSum3).BackColor = color11);
			Color color15 = (((Control)tXTKasrSum2).BackColor = color13);
			Color color17 = (((Control)tXTKasrSum).BackColor = color15);
			Color color19 = (((Control)tXTKarkerdSum).BackColor = color17);
			Color color21 = (((Control)tXTJomeSum).BackColor = color19);
			Color color23 = (((Control)tXTGheybatSum).BackColor = color21);
			Color color25 = (((Control)tXTEzafeSum3).BackColor = color23);
			Color backColor = (((Control)tXTEzafeSum2).BackColor = color25);
			((Control)tXTEzafeSum).BackColor = backColor;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXT_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1279);
		}
	}

	private async void NU_workDayBime_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			_clsFish.CountWorkdayBime = short.Parse(NU_workDayBime.Value.ToString());
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NU_workDayBime_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1292);
		}
	}

	private async void NU_workDayMaliat_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			_clsFish.CountWorkdayMaliyat = short.Parse(NU_workDayMaliat.Value.ToString());
			await FillGridsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NU_workDayMaliat_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1305);
		}
	}

	private void TXTDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateBoxFa tXTDate = TXTDate;
			if (((tXTDate != null) ? ((Control)tXTDate).Text : null) != null && _clsFish != null)
			{
				_clsFish.DateM = Utilities.CalendarToMiladi(((Control)TXTDate).Text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1319);
		}
	}

	private void TXTDesc_TextChanged(object sender, EventArgs e)
	{
		_clsFish.Description = ((Control)TXTDesc).Text;
	}

	private void TxtVacationRemind_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtVacationRemind).Text = ((Control)TxtVacationRemind).Text.Trim();
			double result;
			if (string.IsNullOrEmpty(((Control)TxtVacationRemind).Text))
			{
				_clsFish.SetVacationRemind(0.0);
			}
			else if (double.TryParse(((Control)TxtVacationRemind).Text, out result))
			{
				_clsFish.SetVacationRemind(result);
			}
			else
			{
				_clsFish.SetVacationRemind(0.0);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtVacationRemind_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1345);
		}
	}

	private void DGMazaya1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGMazaya1).Rows)
			{
				DataGridViewRow val = item;
				val.Cells[0].ToolTipText = val.Cells[((DataGridViewBand)DgDescription).Index].Value.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGMazaya1_DataBindingComplete", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1359);
		}
	}

	private void DGKosorat1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGKosorat1).Rows)
			{
				DataGridViewRow val = item;
				val.Cells[0].ToolTipText = val.Cells[((DataGridViewBand)DgDescriptionK).Index].Value.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGKosorat1_DataBindingComplete", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1372);
		}
	}

	private void ChkLockFish_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			_clsFish.EditST = (ChkLockFish.Checked ? ((short)1) : ((short)0));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkLockFish_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1384);
		}
	}

	private void FrmFishHoghogh_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (!((Control)BtnAccept).Enabled)
			{
				((FormExtention)this).ResetControlChanged();
			}
			((Form)this).Close();
			((Form)this).DialogResult = (DialogResult)3;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmFishHoghogh_FormClosing", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1402);
		}
	}

	private void BtnAbort_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)3;
		if (!((Control)BtnAccept).Enabled)
		{
			((FormExtention)this).ResetControlChanged();
		}
		((Form)this).Close();
	}

	private async void LblCodeTafsil_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_ = 2;
		try
		{
			if (_clsFish?.ClsGharardad != null)
			{
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				CancellationToken token = _changeToken.Token;
				TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
				GharardadNew gharardadTemp = new GharardadNew(((CacheBaseForm)this).ContextGuid, setting);
				if (!token.IsCancellationRequested)
				{
					Utilities.CopyPropertiesTo<GharardadNew, GharardadNew>(await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, _clsFish.ClsGharardad.ID, token), gharardadTemp);
					FrmGharardad frmGharardad = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, gharardadTemp);
					await ((CacheBaseForm)frmGharardad).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frmGharardad).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LblCodeTafsil_LinkClicked", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1432);
		}
	}

	private async void LblName_Click(object sender, EventArgs e)
	{
		try
		{
			FrmPersonel frmPerson = new FrmPersonel(((CacheBaseForm)this).ContextGuid, _clsFish.ClsPerson, _font, isReadOnly: false);
			await ((CacheBaseForm)frmPerson).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frmPerson).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LblName_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1443);
		}
	}

	private void DGMazaya1_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		BtnEditMazaya_Click(sender, (EventArgs)(object)e);
	}

	private void DGKosorat1_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		BtnEditKosorat_Click(sender, (EventArgs)(object)e);
	}

	private async void labelX33_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			FrmShowInsuranceTaxItems frm = new FrmShowInsuranceTaxItems(contextGuid, await _clsFish.GetIncludedTaxItemsAsync(token), _clsFish.FactorByTax, "محاسبه جمع مشمول مالیات");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "labelX33_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1470);
		}
	}

	private async void labelX20_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			FrmShowInsuranceTaxItems frm = new FrmShowInsuranceTaxItems(contextGuid, await _clsFish.GetIncludedInsuranceItemsAsync(token), _clsFish.FactorByInsurance, "محاسبه جمع مشمول بیمه");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "labelX20_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs", 1489);
		}
	}

	private void FrmFishHoghogh_Load(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "FrmFishHoghogh_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmFishHoghogh.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FrmFishHoghogh_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 116)
			{
				e.Handled = true;
				((PopupItemControl)BtnAccept).PerformClick();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Expected O, but got Unknown
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Expected O, but got Unknown
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected O, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Expected O, but got Unknown
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Expected O, but got Unknown
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Expected O, but got Unknown
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Expected O, but got Unknown
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Expected O, but got Unknown
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Expected O, but got Unknown
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Expected O, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Expected O, but got Unknown
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Expected O, but got Unknown
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Expected O, but got Unknown
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Expected O, but got Unknown
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Expected O, but got Unknown
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Expected O, but got Unknown
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Expected O, but got Unknown
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Expected O, but got Unknown
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Expected O, but got Unknown
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Expected O, but got Unknown
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Expected O, but got Unknown
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Expected O, but got Unknown
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Expected O, but got Unknown
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Expected O, but got Unknown
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Expected O, but got Unknown
		//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Expected O, but got Unknown
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Expected O, but got Unknown
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Expected O, but got Unknown
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Expected O, but got Unknown
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Expected O, but got Unknown
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Expected O, but got Unknown
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_094e: Expected O, but got Unknown
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Expected O, but got Unknown
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Expected O, but got Unknown
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Expected O, but got Unknown
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Expected O, but got Unknown
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Expected O, but got Unknown
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Expected O, but got Unknown
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c7: Expected O, but got Unknown
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Expected O, but got Unknown
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Expected O, but got Unknown
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Expected O, but got Unknown
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Expected O, but got Unknown
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Expected O, but got Unknown
		//IL_1f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f57: Expected O, but got Unknown
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_215e: Expected O, but got Unknown
		//IL_21d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2545: Unknown result type (might be due to invalid IL or missing references)
		//IL_254f: Expected O, but got Unknown
		//IL_25de: Unknown result type (might be due to invalid IL or missing references)
		//IL_2664: Unknown result type (might be due to invalid IL or missing references)
		//IL_266e: Expected O, but got Unknown
		//IL_2692: Unknown result type (might be due to invalid IL or missing references)
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_2872: Expected O, but got Unknown
		//IL_28e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c60: Expected O, but got Unknown
		//IL_2cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db0: Expected O, but got Unknown
		//IL_2e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_3099: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a3: Expected O, but got Unknown
		//IL_3147: Unknown result type (might be due to invalid IL or missing references)
		//IL_3460: Unknown result type (might be due to invalid IL or missing references)
		//IL_34da: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e4: Expected O, but got Unknown
		//IL_3508: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_363a: Unknown result type (might be due to invalid IL or missing references)
		//IL_380d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3817: Expected O, but got Unknown
		//IL_39e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ed: Expected O, but got Unknown
		//IL_3a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a73: Expected O, but got Unknown
		//IL_3a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df3: Expected O, but got Unknown
		//IL_3e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f40: Expected O, but got Unknown
		//IL_3f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4002: Expected O, but got Unknown
		//IL_4026: Unknown result type (might be due to invalid IL or missing references)
		//IL_40cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_426c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4276: Expected O, but got Unknown
		//IL_42a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4333: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4460: Unknown result type (might be due to invalid IL or missing references)
		//IL_4633: Unknown result type (might be due to invalid IL or missing references)
		//IL_463d: Expected O, but got Unknown
		//IL_46e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4778: Unknown result type (might be due to invalid IL or missing references)
		//IL_491a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4924: Expected O, but got Unknown
		//IL_4948: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b1f: Expected O, but got Unknown
		//IL_4ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf2: Expected O, but got Unknown
		//IL_4ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4edc: Expected O, but got Unknown
		//IL_4f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5017: Unknown result type (might be due to invalid IL or missing references)
		//IL_51c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_51d3: Expected O, but got Unknown
		//IL_5262: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_54b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_54c3: Expected O, but got Unknown
		//IL_572d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5737: Expected O, but got Unknown
		//IL_5917: Unknown result type (might be due to invalid IL or missing references)
		//IL_5921: Expected O, but got Unknown
		//IL_5b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b0a: Expected O, but got Unknown
		//IL_5b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ba9: Expected O, but got Unknown
		//IL_5bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c5b: Expected O, but got Unknown
		//IL_5c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e5c: Expected O, but got Unknown
		//IL_5eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef9: Expected O, but got Unknown
		//IL_5f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5faf: Expected O, but got Unknown
		//IL_5fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_605b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6065: Expected O, but got Unknown
		//IL_6089: Unknown result type (might be due to invalid IL or missing references)
		//IL_6111: Unknown result type (might be due to invalid IL or missing references)
		//IL_611b: Expected O, but got Unknown
		//IL_613f: Unknown result type (might be due to invalid IL or missing references)
		//IL_61c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d2: Expected O, but got Unknown
		//IL_61f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_627e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6288: Expected O, but got Unknown
		//IL_62ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_6334: Unknown result type (might be due to invalid IL or missing references)
		//IL_633e: Expected O, but got Unknown
		//IL_6362: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_63f6: Expected O, but got Unknown
		//IL_6423: Unknown result type (might be due to invalid IL or missing references)
		//IL_64a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6533: Unknown result type (might be due to invalid IL or missing references)
		//IL_653d: Expected O, but got Unknown
		//IL_656e: Unknown result type (might be due to invalid IL or missing references)
		//IL_65f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_668c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6716: Unknown result type (might be due to invalid IL or missing references)
		//IL_6720: Expected O, but got Unknown
		//IL_6741: Unknown result type (might be due to invalid IL or missing references)
		//IL_67cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_67d5: Expected O, but got Unknown
		//IL_6802: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_69d2: Expected O, but got Unknown
		//IL_6a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c51: Expected O, but got Unknown
		//IL_6e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e3b: Expected O, but got Unknown
		//IL_6eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7082: Unknown result type (might be due to invalid IL or missing references)
		//IL_708c: Expected O, but got Unknown
		//IL_726c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7276: Expected O, but got Unknown
		//IL_7456: Unknown result type (might be due to invalid IL or missing references)
		//IL_7460: Expected O, but got Unknown
		//IL_7503: Unknown result type (might be due to invalid IL or missing references)
		//IL_7598: Unknown result type (might be due to invalid IL or missing references)
		//IL_762d: Unknown result type (might be due to invalid IL or missing references)
		//IL_77ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_7809: Expected O, but got Unknown
		//IL_78ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_7941: Unknown result type (might be due to invalid IL or missing references)
		//IL_79d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_80af: Unknown result type (might be due to invalid IL or missing references)
		//IL_85ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_86bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_86c9: Expected O, but got Unknown
		//IL_86ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_8781: Unknown result type (might be due to invalid IL or missing references)
		//IL_878b: Expected O, but got Unknown
		//IL_87af: Unknown result type (might be due to invalid IL or missing references)
		//IL_88cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_88d9: Expected O, but got Unknown
		//IL_88fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_8998: Unknown result type (might be due to invalid IL or missing references)
		//IL_8a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8a28: Expected O, but got Unknown
		//IL_8a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_8ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_9117: Unknown result type (might be due to invalid IL or missing references)
		//IL_91b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_9244: Unknown result type (might be due to invalid IL or missing references)
		//IL_924e: Expected O, but got Unknown
		//IL_927e: Unknown result type (might be due to invalid IL or missing references)
		//IL_9307: Unknown result type (might be due to invalid IL or missing references)
		//IL_94c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_94d2: Expected O, but got Unknown
		//IL_9693: Unknown result type (might be due to invalid IL or missing references)
		//IL_969d: Expected O, but got Unknown
		//IL_9852: Unknown result type (might be due to invalid IL or missing references)
		//IL_985c: Expected O, but got Unknown
		//IL_9a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_9a1b: Expected O, but got Unknown
		//IL_9bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_9bd9: Expected O, but got Unknown
		//IL_9d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d97: Expected O, but got Unknown
		//IL_9f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9f56: Expected O, but got Unknown
		//IL_a10b: Unknown result type (might be due to invalid IL or missing references)
		//IL_a115: Expected O, but got Unknown
		//IL_a2ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_a2d4: Expected O, but got Unknown
		//IL_a489: Unknown result type (might be due to invalid IL or missing references)
		//IL_a493: Expected O, but got Unknown
		//IL_a648: Unknown result type (might be due to invalid IL or missing references)
		//IL_a652: Expected O, but got Unknown
		//IL_a6e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_a89f: Unknown result type (might be due to invalid IL or missing references)
		//IL_a8a9: Expected O, but got Unknown
		//IL_a938: Unknown result type (might be due to invalid IL or missing references)
		//IL_a9d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_aa68: Unknown result type (might be due to invalid IL or missing references)
		//IL_ab00: Unknown result type (might be due to invalid IL or missing references)
		//IL_ab98: Unknown result type (might be due to invalid IL or missing references)
		//IL_ac30: Unknown result type (might be due to invalid IL or missing references)
		//IL_acc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_ad60: Unknown result type (might be due to invalid IL or missing references)
		//IL_adf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_ae90: Unknown result type (might be due to invalid IL or missing references)
		//IL_afb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_b2f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_b302: Expected O, but got Unknown
		//IL_b3db: Unknown result type (might be due to invalid IL or missing references)
		//IL_b3e5: Expected O, but got Unknown
		//IL_b3f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_b3fc: Expected O, but got Unknown
		//IL_b54e: Unknown result type (might be due to invalid IL or missing references)
		//IL_b558: Expected O, but got Unknown
		//IL_b63d: Unknown result type (might be due to invalid IL or missing references)
		//IL_b647: Expected O, but got Unknown
		//IL_b712: Unknown result type (might be due to invalid IL or missing references)
		//IL_b71c: Expected O, but got Unknown
		//IL_ba32: Unknown result type (might be due to invalid IL or missing references)
		//IL_ba3c: Expected O, but got Unknown
		//IL_bb21: Unknown result type (might be due to invalid IL or missing references)
		//IL_bb2b: Expected O, but got Unknown
		//IL_bc9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_bca9: Expected O, but got Unknown
		//IL_bd87: Unknown result type (might be due to invalid IL or missing references)
		//IL_bd91: Expected O, but got Unknown
		//IL_bd9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_bda8: Expected O, but got Unknown
		//IL_c0e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_c174: Unknown result type (might be due to invalid IL or missing references)
		//IL_c245: Unknown result type (might be due to invalid IL or missing references)
		//IL_c24f: Expected O, but got Unknown
		//IL_c363: Unknown result type (might be due to invalid IL or missing references)
		//IL_c36d: Expected O, but got Unknown
		//IL_c3ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_c5e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_c5f2: Expected O, but got Unknown
		//IL_c669: Unknown result type (might be due to invalid IL or missing references)
		//IL_c700: Unknown result type (might be due to invalid IL or missing references)
		//IL_c7a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_c998: Unknown result type (might be due to invalid IL or missing references)
		//IL_c9a2: Expected O, but got Unknown
		//IL_cb57: Unknown result type (might be due to invalid IL or missing references)
		//IL_cb61: Expected O, but got Unknown
		//IL_cbd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_cdaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_cdb4: Expected O, but got Unknown
		//IL_ce2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_d094: Unknown result type (might be due to invalid IL or missing references)
		//IL_d09e: Expected O, but got Unknown
		//IL_d561: Unknown result type (might be due to invalid IL or missing references)
		//IL_d56b: Expected O, but got Unknown
		//IL_d597: Unknown result type (might be due to invalid IL or missing references)
		//IL_d5a1: Expected O, but got Unknown
		//IL_d6e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_d78a: Unknown result type (might be due to invalid IL or missing references)
		//IL_d81a: Unknown result type (might be due to invalid IL or missing references)
		//IL_d824: Expected O, but got Unknown
		//IL_d902: Unknown result type (might be due to invalid IL or missing references)
		//IL_d99a: Unknown result type (might be due to invalid IL or missing references)
		//IL_dae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_db7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_dc0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_dca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_dd37: Unknown result type (might be due to invalid IL or missing references)
		//IL_ddcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_de60: Unknown result type (might be due to invalid IL or missing references)
		//IL_df05: Unknown result type (might be due to invalid IL or missing references)
		//IL_df9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_e09e: Unknown result type (might be due to invalid IL or missing references)
		//IL_e0a8: Expected O, but got Unknown
		//IL_e0e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_e0ea: Expected O, but got Unknown
		//IL_e1d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_e1df: Expected O, but got Unknown
		//IL_e39c: Unknown result type (might be due to invalid IL or missing references)
		//IL_e3a6: Expected O, but got Unknown
		//IL_e3e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_e3f1: Expected O, but got Unknown
		//IL_e40b: Unknown result type (might be due to invalid IL or missing references)
		//IL_e415: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		GPKarkerd = new GroupPanel();
		LblNobatKari3 = new LabelX();
		TXTNobatSum3 = new TextBoxFa();
		labelX86 = new LabelX();
		NUHNobat3 = new NumericUpDown();
		labelX87 = new LabelX();
		NUMNobat3 = new NumericUpDown();
		TXTNobatPrice3 = new TextBoxFa();
		labelX88 = new LabelX();
		LblNobatKari2 = new LabelX();
		TXTNobatSum2 = new TextBoxFa();
		labelX82 = new LabelX();
		NUHNobat2 = new NumericUpDown();
		labelX83 = new LabelX();
		NUMNobat2 = new NumericUpDown();
		TXTNobatPrice2 = new TextBoxFa();
		labelX84 = new LabelX();
		groupBox1 = new GroupBox();
		labelX80 = new LabelX();
		RbtnEndGharardad = new RadioButton();
		RbtnEndMonth = new RadioButton();
		GbxSalaryType = new GroupBox();
		labelX79 = new LabelX();
		RbtnMonthly = new RadioButton();
		RbtnDaily = new RadioButton();
		RbtnSaati = new RadioButton();
		NU_Jarime = new NumericUpDown();
		labelX75 = new LabelX();
		labelX55 = new LabelX();
		labelX56 = new LabelX();
		labelX77 = new LabelX();
		TXTJarimeSum = new TextBoxFa();
		TXTJarimePrice = new TextBoxFa();
		labelX21 = new LabelX();
		labelX28 = new LabelX();
		labelX31 = new LabelX();
		labelX49 = new LabelX();
		TxtVacationRemind = new MaskedTextBox();
		Lbl11 = new LabelX();
		Lbl10 = new LabelX();
		NumDailyVacation = new NumericUpDown();
		labelX78 = new LabelX();
		Lbl1 = new LabelX();
		Lbl2 = new LabelX();
		NUMVacation = new NumericUpDown();
		NUHVacation = new NumericUpDown();
		LblCountMorHourly = new LabelX();
		Lbl4 = new LabelX();
		Lbl3 = new LabelX();
		labelX57 = new LabelX();
		TXTVacationSum = new TextBoxFa();
		labelX59 = new LabelX();
		Lbl5 = new LabelX();
		NU_Mission = new MaskedTextBox();
		NUH_ClocklyVacation = new NumericUpDown();
		labelX1 = new LabelX();
		TXTVacationPrice = new TextBoxFa();
		TXTVacationSumUsed = new TextBoxFa();
		TXTMissionSum = new TextBoxFa();
		labelX24 = new LabelX();
		labelX76 = new LabelX();
		TXTMissionPrice = new TextBoxFa();
		labelX48 = new LabelX();
		labelX25 = new LabelX();
		TXTKarkerdSum = new TextBoxFa();
		NUM_ClocklyVacation = new NumericUpDown();
		TXTEzafeSum3 = new TextBoxFa();
		TXTEzafeSum2 = new TextBoxFa();
		TXTEzafeSum1 = new TextBoxFa();
		LblCountMorNotHoghogh = new LabelX();
		LblCountMorWithHoghogh = new LabelX();
		TXTShabSum = new TextBoxFa();
		labelX74 = new LabelX();
		labelX70 = new LabelX();
		labelX23 = new LabelX();
		labelX22 = new LabelX();
		labelX18 = new LabelX();
		LblNobatKari1 = new LabelX();
		labelX16 = new LabelX();
		LblCountKarkard = new LabelX();
		labelX39 = new LabelX();
		LblCountKol = new LabelX();
		labelX14 = new LabelX();
		labelX32 = new LabelX();
		labelX54 = new LabelX();
		LblCountGheybat = new LabelX();
		TXTNobatSum1 = new TextBoxFa();
		labelX50 = new LabelX();
		TXTJomeSum = new TextBoxFa();
		TXTTatilSum = new TextBoxFa();
		labelX66 = new LabelX();
		TXTKasrSum3 = new TextBoxFa();
		TXTKasrSum2 = new TextBoxFa();
		TXTKasrSum1 = new TextBoxFa();
		labelX62 = new LabelX();
		labelX5 = new LabelX();
		labelX6 = new LabelX();
		TXTGheybatSum = new TextBoxFa();
		labelX73 = new LabelX();
		labelX8 = new LabelX();
		labelX69 = new LabelX();
		labelX10 = new LabelX();
		NUHKasr3 = new NumericUpDown();
		labelX29 = new LabelX();
		NUHKasr2 = new NumericUpDown();
		labelX30 = new LabelX();
		labelX72 = new LabelX();
		NUHKasr1 = new NumericUpDown();
		labelX68 = new LabelX();
		NUMKasr3 = new NumericUpDown();
		labelX12 = new LabelX();
		NUMKasr2 = new NumericUpDown();
		labelX51 = new LabelX();
		NUMKasr1 = new NumericUpDown();
		NUHJome = new NumericUpDown();
		NUHTatil = new NumericUpDown();
		NUHNobat1 = new NumericUpDown();
		NU_Gheybat = new NumericUpDown();
		NUHEzafe3 = new NumericUpDown();
		NUHShab = new NumericUpDown();
		NUHEzafe2 = new NumericUpDown();
		DotJome = new LabelX();
		NUHEzafe1 = new NumericUpDown();
		LblKarkerd = new LabelX();
		labelX65 = new LabelX();
		NUHKarkerd = new NumericUpDown();
		labelX61 = new LabelX();
		labelX47 = new LabelX();
		labelX19 = new LabelX();
		NUMEzafe3 = new NumericUpDown();
		NUMKarkerd = new NumericUpDown();
		NUMEzafe2 = new NumericUpDown();
		labelX46 = new LabelX();
		NUMEzafe1 = new NumericUpDown();
		NUMShab = new NumericUpDown();
		labelX64 = new LabelX();
		NUMJome = new NumericUpDown();
		NUMTatil = new NumericUpDown();
		labelX60 = new LabelX();
		NUMNobat1 = new NumericUpDown();
		labelX45 = new LabelX();
		LblDot = new LabelX();
		Counttaradodnaghes = new LabelX();
		labelX52 = new LabelX();
		TXTEzafePrice3 = new TextBoxFa();
		TXTJomePrice = new TextBoxFa();
		TXTTatilPrice = new TextBoxFa();
		TXTEzafePrice2 = new TextBoxFa();
		TXTKarkerdPrice = new TextBoxFa();
		TXTEzafePrice1 = new TextBoxFa();
		TXTKasrPrice3 = new TextBoxFa();
		TXTShabPrice = new TextBoxFa();
		TXTKasrPrice2 = new TextBoxFa();
		TXTNobatPrice1 = new TextBoxFa();
		TXTKasrPrice1 = new TextBoxFa();
		labelX63 = new LabelX();
		TXTGheybatPrice = new TextBoxFa();
		labelX58 = new LabelX();
		lblMablagh = new LabelX();
		labelX7 = new LabelX();
		labelX9 = new LabelX();
		labelX71 = new LabelX();
		labelX11 = new LabelX();
		labelX67 = new LabelX();
		labelX13 = new LabelX();
		labelX26 = new LabelX();
		labelX27 = new LabelX();
		NU_workDayMaliat = new NumericUpDown();
		labelX53 = new LabelX();
		NU_workDayBime = new NumericUpDown();
		splitContainer1 = new SplitContainer();
		DGMazaya1 = new DataGridViewX();
		DGMName = new DataGridViewTextBoxColumn();
		DGMCredit = new DataGridViewTextBoxColumn();
		DgDescription = new DataGridViewTextBoxColumn();
		fishMDetBindingSource = new BindingSource(components);
		BTNADDMazaya = new ButtonX();
		BTNDeleteMazaya = new ButtonX();
		BtnEditMazaya = new ButtonX();
		groupPanel1 = new GroupPanel();
		BtnEditKosorat = new ButtonX();
		BtnAddKosorat = new ButtonX();
		BtnDelKosor = new ButtonX();
		DGKosorat1 = new DataGridViewX();
		DGKName = new DataGridViewTextBoxColumn();
		DGKDebit = new DataGridViewTextBoxColumn();
		DgDescriptionK = new DataGridViewTextBoxColumn();
		fishKDetBindingSource = new BindingSource(components);
		groupPanel2 = new GroupPanel();
		labelX38 = new LabelX();
		labelX37 = new LabelX();
		TXTKolMande = new TextBoxFa();
		LblMaliat = new LinkLabel();
		TxtIncludedTax = new TextBoxFa();
		labelX36 = new LabelX();
		labelX44 = new LabelX();
		LblBime = new LinkLabel();
		TXTKolKosorat = new TextBoxFa();
		TxtIncludedInsurance = new TextBoxFa();
		labelX35 = new LabelX();
		TXTKolMazaya = new TextBoxFa();
		labelX34 = new LabelX();
		BtnAbort = new Button();
		TXTKolKarkerd = new TextBoxFa();
		GPHoghogh = new GroupPanel();
		ChkLockFish = new CheckBox();
		TXTDate = new DateBoxFa();
		labelX3 = new LabelX();
		LblName = new LabelX();
		BtnAccept = new ButtonX();
		LblCodeMeli = new LabelX();
		LblTadate = new LabelX();
		TXTFishNO = new NumericUpDown();
		labelX15 = new LabelX();
		labelX43 = new LabelX();
		labelX42 = new LabelX();
		labelX41 = new LabelX();
		labelX40 = new LabelX();
		labelX4 = new LabelX();
		labelX2 = new LabelX();
		LblfromDate = new LabelX();
		LblFName = new LabelX();
		LblCodeTafsil = new LinkLabel();
		btnModifiedTaradod = new ButtonX();
		btnNotifyMe = new ButtonX();
		TXTDesc = new TextBoxX();
		timerReadNotify = new Timer(components);
		contextMenuStrip1 = new ContextMenuStrip(components);
		((Control)panelEx1).SuspendLayout();
		((Control)GPKarkerd).SuspendLayout();
		((ISupportInitialize)NUHNobat3).BeginInit();
		((ISupportInitialize)NUMNobat3).BeginInit();
		((ISupportInitialize)NUHNobat2).BeginInit();
		((ISupportInitialize)NUMNobat2).BeginInit();
		((Control)groupBox1).SuspendLayout();
		((Control)GbxSalaryType).SuspendLayout();
		((ISupportInitialize)NU_Jarime).BeginInit();
		((ISupportInitialize)NumDailyVacation).BeginInit();
		((ISupportInitialize)NUMVacation).BeginInit();
		((ISupportInitialize)NUHVacation).BeginInit();
		((ISupportInitialize)NUH_ClocklyVacation).BeginInit();
		((ISupportInitialize)NUM_ClocklyVacation).BeginInit();
		((ISupportInitialize)NUHKasr3).BeginInit();
		((ISupportInitialize)NUHKasr2).BeginInit();
		((ISupportInitialize)NUHKasr1).BeginInit();
		((ISupportInitialize)NUMKasr3).BeginInit();
		((ISupportInitialize)NUMKasr2).BeginInit();
		((ISupportInitialize)NUMKasr1).BeginInit();
		((ISupportInitialize)NUHJome).BeginInit();
		((ISupportInitialize)NUHTatil).BeginInit();
		((ISupportInitialize)NUHNobat1).BeginInit();
		((ISupportInitialize)NU_Gheybat).BeginInit();
		((ISupportInitialize)NUHEzafe3).BeginInit();
		((ISupportInitialize)NUHShab).BeginInit();
		((ISupportInitialize)NUHEzafe2).BeginInit();
		((ISupportInitialize)NUHEzafe1).BeginInit();
		((ISupportInitialize)NUHKarkerd).BeginInit();
		((ISupportInitialize)NUMEzafe3).BeginInit();
		((ISupportInitialize)NUMKarkerd).BeginInit();
		((ISupportInitialize)NUMEzafe2).BeginInit();
		((ISupportInitialize)NUMEzafe1).BeginInit();
		((ISupportInitialize)NUMShab).BeginInit();
		((ISupportInitialize)NUMJome).BeginInit();
		((ISupportInitialize)NUMTatil).BeginInit();
		((ISupportInitialize)NUMNobat1).BeginInit();
		((ISupportInitialize)NU_workDayMaliat).BeginInit();
		((ISupportInitialize)NU_workDayBime).BeginInit();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((ISupportInitialize)DGMazaya1).BeginInit();
		((ISupportInitialize)fishMDetBindingSource).BeginInit();
		((ISupportInitialize)DGKosorat1).BeginInit();
		((ISupportInitialize)fishKDetBindingSource).BeginInit();
		((Control)GPHoghogh).SuspendLayout();
		((ISupportInitialize)TXTFishNO).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)GPKarkerd);
		((Control)panelEx1).Controls.Add((Control)(object)NU_workDayMaliat);
		((Control)panelEx1).Controls.Add((Control)(object)labelX53);
		((Control)panelEx1).Controls.Add((Control)(object)NU_workDayBime);
		((Control)panelEx1).Controls.Add((Control)(object)splitContainer1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX38);
		((Control)panelEx1).Controls.Add((Control)(object)labelX37);
		((Control)panelEx1).Controls.Add((Control)(object)TXTKolMande);
		((Control)panelEx1).Controls.Add((Control)(object)LblMaliat);
		((Control)panelEx1).Controls.Add((Control)(object)TxtIncludedTax);
		((Control)panelEx1).Controls.Add((Control)(object)labelX36);
		((Control)panelEx1).Controls.Add((Control)(object)labelX44);
		((Control)panelEx1).Controls.Add((Control)(object)LblBime);
		((Control)panelEx1).Controls.Add((Control)(object)TXTKolKosorat);
		((Control)panelEx1).Controls.Add((Control)(object)TxtIncludedInsurance);
		((Control)panelEx1).Controls.Add((Control)(object)labelX35);
		((Control)panelEx1).Controls.Add((Control)(object)TXTKolMazaya);
		((Control)panelEx1).Controls.Add((Control)(object)labelX34);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAbort);
		((Control)panelEx1).Controls.Add((Control)(object)TXTKolKarkerd);
		((Control)panelEx1).Controls.Add((Control)(object)GPHoghogh);
		((Control)panelEx1).Controls.Add((Control)(object)btnModifiedTaradod);
		((Control)panelEx1).Controls.Add((Control)(object)btnNotifyMe);
		((Control)panelEx1).Controls.Add((Control)(object)TXTDesc);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1394, 712);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)GPKarkerd).Anchor = (AnchorStyles)11;
		((ScrollableControl)GPKarkerd).AutoScroll = true;
		((PanelControl)GPKarkerd).CanvasColor = SystemColors.Control;
		((PanelControl)GPKarkerd).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GPKarkerd).Controls.Add((Control)(object)LblNobatKari3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatSum3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX86);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHNobat3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX87);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMNobat3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatPrice3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX88);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblNobatKari2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatSum2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX82);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHNobat2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX83);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMNobat2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatPrice2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX84);
		((Control)GPKarkerd).Controls.Add((Control)(object)groupBox1);
		((Control)GPKarkerd).Controls.Add((Control)(object)GbxSalaryType);
		((Control)GPKarkerd).Controls.Add((Control)(object)NU_Jarime);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX75);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX55);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX56);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX77);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTJarimeSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTJarimePrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX21);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX28);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX31);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX49);
		((Control)GPKarkerd).Controls.Add((Control)(object)TxtVacationRemind);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl11);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl10);
		((Control)GPKarkerd).Controls.Add((Control)(object)NumDailyVacation);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX78);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl1);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl2);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMVacation);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHVacation);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountMorHourly);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl4);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX57);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTVacationSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX59);
		((Control)GPKarkerd).Controls.Add((Control)(object)Lbl5);
		((Control)GPKarkerd).Controls.Add((Control)(object)NU_Mission);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUH_ClocklyVacation);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX1);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTVacationPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTVacationSumUsed);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTMissionSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX24);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX76);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTMissionPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX48);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX25);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKarkerdSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUM_ClocklyVacation);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafeSum3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafeSum2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafeSum1);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountMorNotHoghogh);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountMorWithHoghogh);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTShabSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX74);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX70);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX23);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX22);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX18);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblNobatKari1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX16);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountKarkard);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX39);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountKol);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX14);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX32);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX54);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblCountGheybat);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatSum1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX50);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTJomeSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTTatilSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX66);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrSum3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrSum2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrSum1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX62);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX5);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX6);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTGheybatSum);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX73);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX8);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX69);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX10);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHKasr3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX29);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHKasr2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX30);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX72);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHKasr1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX68);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMKasr3);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX12);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMKasr2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX51);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMKasr1);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHJome);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHTatil);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHNobat1);
		((Control)GPKarkerd).Controls.Add((Control)(object)NU_Gheybat);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHEzafe3);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHShab);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHEzafe2);
		((Control)GPKarkerd).Controls.Add((Control)(object)DotJome);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHEzafe1);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblKarkerd);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX65);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUHKarkerd);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX61);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX47);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX19);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMEzafe3);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMKarkerd);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMEzafe2);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX46);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMEzafe1);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMShab);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX64);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMJome);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMTatil);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX60);
		((Control)GPKarkerd).Controls.Add((Control)(object)NUMNobat1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX45);
		((Control)GPKarkerd).Controls.Add((Control)(object)LblDot);
		((Control)GPKarkerd).Controls.Add((Control)(object)Counttaradodnaghes);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX52);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafePrice3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTJomePrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTTatilPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafePrice2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKarkerdPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTEzafePrice1);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrPrice3);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTShabPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrPrice2);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTNobatPrice1);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTKasrPrice1);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX63);
		((Control)GPKarkerd).Controls.Add((Control)(object)TXTGheybatPrice);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX58);
		((Control)GPKarkerd).Controls.Add((Control)(object)lblMablagh);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX7);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX9);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX71);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX11);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX67);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX13);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX26);
		((Control)GPKarkerd).Controls.Add((Control)(object)labelX27);
		GPKarkerd.DisabledBackColor = Color.Empty;
		((Control)GPKarkerd).Location = new Point(772, 93);
		((Control)GPKarkerd).Name = "GPKarkerd";
		((Control)GPKarkerd).Size = new Size(610, 611);
		((PanelControl)GPKarkerd).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GPKarkerd).Style.BackColorGradientAngle = 90;
		((PanelControl)GPKarkerd).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GPKarkerd).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GPKarkerd).Style.BorderBottomWidth = 1;
		((PanelControl)GPKarkerd).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GPKarkerd).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GPKarkerd).Style.BorderLeftWidth = 1;
		((PanelControl)GPKarkerd).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GPKarkerd).Style.BorderRightWidth = 1;
		((PanelControl)GPKarkerd).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GPKarkerd).Style.BorderTopWidth = 1;
		((PanelControl)GPKarkerd).Style.CornerDiameter = 4;
		((PanelControl)GPKarkerd).Style.CornerType = (eCornerType)2;
		((PanelControl)GPKarkerd).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)GPKarkerd).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GPKarkerd).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GPKarkerd).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GPKarkerd).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GPKarkerd).TabIndex = 1;
		((Control)GPKarkerd).Text = "[کارکرد]";
		((Control)LblNobatKari3).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNobatKari3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNobatKari3).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)LblNobatKari3).Location = new Point(398, 504);
		((Control)LblNobatKari3).Margin = new Padding(3, 2, 3, 2);
		((Control)LblNobatKari3).Name = "LblNobatKari3";
		((Control)LblNobatKari3).Size = new Size(150, 21);
		((Control)LblNobatKari3).TabIndex = 122;
		((Control)LblNobatKari3).Text = "نوبت شب:";
		LblNobatKari3.TextAlignment = (StringAlignment)2;
		TXTNobatSum3.AllowEmpty = true;
		((Control)TXTNobatSum3).Anchor = (AnchorStyles)9;
		((Control)TXTNobatSum3).BackColor = Color.LightGray;
		TXTNobatSum3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatSum3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatSum3.BackcolorTooltip = Color.Red;
		TXTNobatSum3.CurrencyFormat = "";
		((Control)TXTNobatSum3).ForeColor = Color.DarkGreen;
		TXTNobatSum3.ForeColorTooltip = Color.Black;
		TXTNobatSum3.GetColoresFromSettingDLL = true;
		TXTNobatSum3.HandleKeydown = true;
		((Control)TXTNobatSum3).Location = new Point(8, 504);
		((TextBoxBase)TXTNobatSum3).MaxLength = 19;
		((Control)TXTNobatSum3).Name = "TXTNobatSum3";
		((TextBoxBase)TXTNobatSum3).ReadOnly = true;
		TXTNobatSum3.SendShiftTabAfterDown = true;
		TXTNobatSum3.SendShiftTabAfterEnter = true;
		TXTNobatSum3.SendTabAfterEnter = true;
		TXTNobatSum3.SendTabAfterUP = true;
		TXTNobatSum3.ShowHorofi = (ENShowHorofi)3;
		TXTNobatSum3.ShowToolTipText = true;
		((Control)TXTNobatSum3).Size = new Size(115, 21);
		((Control)TXTNobatSum3).TabIndex = 128;
		((Control)TXTNobatSum3).TabStop = false;
		((Control)TXTNobatSum3).Text = "0";
		TXTNobatSum3.ToolTipFont = new Font("B Traffic", 30f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatSum3.ToolTipPostFix = "";
		TXTNobatSum3.TootTipDelay = 1000;
		TXTNobatSum3.Value = new decimal(new int[4]);
		((Control)labelX86).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX86).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX86).Location = new Point(124, 504);
		((Control)labelX86).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX86).Name = "labelX86";
		((Control)labelX86).Size = new Size(33, 21);
		((Control)labelX86).TabIndex = 123;
		((Control)labelX86).Text = "[جمع] :";
		labelX86.TextAlignment = (StringAlignment)2;
		((Control)NUHNobat3).Anchor = (AnchorStyles)9;
		((Control)NUHNobat3).Location = new Point(296, 504);
		NUHNobat3.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHNobat3).Name = "NUHNobat3";
		((Control)NUHNobat3).Size = new Size(43, 21);
		((Control)NUHNobat3).TabIndex = 126;
		NUHNobat3.ValueChanged += NobatKari3_Changed;
		((Control)labelX87).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX87).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX87).Location = new Point(338, 504);
		((Control)labelX87).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX87).Name = "labelX87";
		((Control)labelX87).Size = new Size(13, 21);
		((Control)labelX87).TabIndex = 129;
		((Control)labelX87).Text = ":";
		labelX87.TextAlignment = (StringAlignment)1;
		((Control)NUMNobat3).Anchor = (AnchorStyles)9;
		((Control)NUMNobat3).Location = new Point(349, 504);
		NUMNobat3.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMNobat3).Name = "NUMNobat3";
		((Control)NUMNobat3).Size = new Size(42, 21);
		((Control)NUMNobat3).TabIndex = 125;
		NUMNobat3.ValueChanged += NobatKari3_Changed;
		TXTNobatPrice3.AllowEmpty = true;
		((Control)TXTNobatPrice3).Anchor = (AnchorStyles)9;
		((Control)TXTNobatPrice3).BackColor = Color.White;
		TXTNobatPrice3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatPrice3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatPrice3.BackcolorTooltip = Color.Red;
		TXTNobatPrice3.CurrencyFormat = "";
		((Control)TXTNobatPrice3).ForeColor = Color.ForestGreen;
		TXTNobatPrice3.ForeColorTooltip = Color.Black;
		TXTNobatPrice3.GetColoresFromSettingDLL = true;
		TXTNobatPrice3.HandleKeydown = true;
		((Control)TXTNobatPrice3).Location = new Point(163, 504);
		((TextBoxBase)TXTNobatPrice3).MaxLength = 19;
		((Control)TXTNobatPrice3).Name = "TXTNobatPrice3";
		TXTNobatPrice3.SendShiftTabAfterDown = true;
		TXTNobatPrice3.SendShiftTabAfterEnter = true;
		TXTNobatPrice3.SendTabAfterEnter = true;
		TXTNobatPrice3.SendTabAfterUP = true;
		TXTNobatPrice3.ShowHorofi = (ENShowHorofi)3;
		TXTNobatPrice3.ShowToolTipText = true;
		((Control)TXTNobatPrice3).Size = new Size(91, 21);
		((Control)TXTNobatPrice3).TabIndex = 127;
		((Control)TXTNobatPrice3).Text = "0";
		TXTNobatPrice3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatPrice3.ToolTipPostFix = "";
		TXTNobatPrice3.TootTipDelay = 1000;
		TXTNobatPrice3.Value = new decimal(new int[4]);
		((Control)TXTNobatPrice3).TextChanged += NobatKari3_Changed;
		((Control)labelX88).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX88).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX88).Location = new Point(256, 504);
		((Control)labelX88).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX88).Name = "labelX88";
		((Control)labelX88).Size = new Size(44, 21);
		((Control)labelX88).TabIndex = 124;
		((Control)labelX88).Text = "[مبلغ] :";
		labelX88.TextAlignment = (StringAlignment)2;
		((Control)LblNobatKari2).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNobatKari2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNobatKari2).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)LblNobatKari2).Location = new Point(398, 478);
		((Control)LblNobatKari2).Margin = new Padding(3, 2, 3, 2);
		((Control)LblNobatKari2).Name = "LblNobatKari2";
		((Control)LblNobatKari2).Size = new Size(150, 21);
		((Control)LblNobatKari2).TabIndex = 114;
		((Control)LblNobatKari2).Text = "نوبت عصر:";
		LblNobatKari2.TextAlignment = (StringAlignment)2;
		TXTNobatSum2.AllowEmpty = true;
		((Control)TXTNobatSum2).Anchor = (AnchorStyles)9;
		((Control)TXTNobatSum2).BackColor = Color.LightGray;
		TXTNobatSum2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatSum2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatSum2.BackcolorTooltip = Color.Red;
		TXTNobatSum2.CurrencyFormat = "";
		((Control)TXTNobatSum2).ForeColor = Color.DarkGreen;
		TXTNobatSum2.ForeColorTooltip = Color.Black;
		TXTNobatSum2.GetColoresFromSettingDLL = true;
		TXTNobatSum2.HandleKeydown = true;
		((Control)TXTNobatSum2).Location = new Point(8, 478);
		((TextBoxBase)TXTNobatSum2).MaxLength = 19;
		((Control)TXTNobatSum2).Name = "TXTNobatSum2";
		((TextBoxBase)TXTNobatSum2).ReadOnly = true;
		TXTNobatSum2.SendShiftTabAfterDown = true;
		TXTNobatSum2.SendShiftTabAfterEnter = true;
		TXTNobatSum2.SendTabAfterEnter = true;
		TXTNobatSum2.SendTabAfterUP = true;
		TXTNobatSum2.ShowHorofi = (ENShowHorofi)3;
		TXTNobatSum2.ShowToolTipText = true;
		((Control)TXTNobatSum2).Size = new Size(115, 21);
		((Control)TXTNobatSum2).TabIndex = 120;
		((Control)TXTNobatSum2).TabStop = false;
		((Control)TXTNobatSum2).Text = "0";
		TXTNobatSum2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatSum2.ToolTipPostFix = "";
		TXTNobatSum2.TootTipDelay = 1000;
		TXTNobatSum2.Value = new decimal(new int[4]);
		((Control)labelX82).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX82).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX82).Location = new Point(124, 478);
		((Control)labelX82).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX82).Name = "labelX82";
		((Control)labelX82).Size = new Size(33, 21);
		((Control)labelX82).TabIndex = 115;
		((Control)labelX82).Text = "[جمع] :";
		labelX82.TextAlignment = (StringAlignment)2;
		((Control)NUHNobat2).Anchor = (AnchorStyles)9;
		((Control)NUHNobat2).Location = new Point(296, 478);
		NUHNobat2.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHNobat2).Name = "NUHNobat2";
		((Control)NUHNobat2).Size = new Size(43, 21);
		((Control)NUHNobat2).TabIndex = 118;
		NUHNobat2.ValueChanged += NobatKari2_Changed;
		((Control)labelX83).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX83).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX83).Location = new Point(338, 478);
		((Control)labelX83).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX83).Name = "labelX83";
		((Control)labelX83).Size = new Size(13, 21);
		((Control)labelX83).TabIndex = 121;
		((Control)labelX83).Text = ":";
		labelX83.TextAlignment = (StringAlignment)1;
		((Control)NUMNobat2).Anchor = (AnchorStyles)9;
		((Control)NUMNobat2).Location = new Point(349, 478);
		NUMNobat2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMNobat2).Name = "NUMNobat2";
		((Control)NUMNobat2).Size = new Size(42, 21);
		((Control)NUMNobat2).TabIndex = 117;
		NUMNobat2.ValueChanged += NobatKari2_Changed;
		TXTNobatPrice2.AllowEmpty = true;
		((Control)TXTNobatPrice2).Anchor = (AnchorStyles)9;
		((Control)TXTNobatPrice2).BackColor = Color.White;
		TXTNobatPrice2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatPrice2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatPrice2.BackcolorTooltip = Color.Red;
		TXTNobatPrice2.CurrencyFormat = "";
		((Control)TXTNobatPrice2).ForeColor = Color.ForestGreen;
		TXTNobatPrice2.ForeColorTooltip = Color.Black;
		TXTNobatPrice2.GetColoresFromSettingDLL = true;
		TXTNobatPrice2.HandleKeydown = true;
		((Control)TXTNobatPrice2).Location = new Point(163, 478);
		((TextBoxBase)TXTNobatPrice2).MaxLength = 19;
		((Control)TXTNobatPrice2).Name = "TXTNobatPrice2";
		TXTNobatPrice2.SendShiftTabAfterDown = true;
		TXTNobatPrice2.SendShiftTabAfterEnter = true;
		TXTNobatPrice2.SendTabAfterEnter = true;
		TXTNobatPrice2.SendTabAfterUP = true;
		TXTNobatPrice2.ShowHorofi = (ENShowHorofi)3;
		TXTNobatPrice2.ShowToolTipText = true;
		((Control)TXTNobatPrice2).Size = new Size(91, 21);
		((Control)TXTNobatPrice2).TabIndex = 119;
		((Control)TXTNobatPrice2).Text = "0";
		TXTNobatPrice2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatPrice2.ToolTipPostFix = "";
		TXTNobatPrice2.TootTipDelay = 1000;
		TXTNobatPrice2.Value = new decimal(new int[4]);
		((Control)TXTNobatPrice2).TextChanged += NobatKari2_Changed;
		((Control)labelX84).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX84).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX84).Location = new Point(256, 478);
		((Control)labelX84).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX84).Name = "labelX84";
		((Control)labelX84).Size = new Size(44, 21);
		((Control)labelX84).TabIndex = 116;
		((Control)labelX84).Text = "[مبلغ] :";
		labelX84.TextAlignment = (StringAlignment)2;
		((Control)groupBox1).Anchor = (AnchorStyles)9;
		((Control)groupBox1).Controls.Add((Control)(object)labelX80);
		((Control)groupBox1).Controls.Add((Control)(object)RbtnEndGharardad);
		((Control)groupBox1).Controls.Add((Control)(object)RbtnEndMonth);
		((Control)groupBox1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)groupBox1).ForeColor = Color.Navy;
		((Control)groupBox1).Location = new Point(8, 179);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(545, 30);
		((Control)groupBox1).TabIndex = 113;
		groupBox1.TabStop = false;
		((Control)labelX80).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX80).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX80).Location = new Point(407, 7);
		((Control)labelX80).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX80).Name = "labelX80";
		((Control)labelX80).Size = new Size(133, 19);
		((Control)labelX80).TabIndex = 115;
		((Control)labelX80).Text = "[نحوه تسویه مانده مرخصی]:";
		labelX80.TextAlignment = (StringAlignment)2;
		((Control)RbtnEndGharardad).Anchor = (AnchorStyles)9;
		((Control)RbtnEndGharardad).AutoSize = true;
		((Control)RbtnEndGharardad).Enabled = false;
		((Control)RbtnEndGharardad).ForeColor = Color.DarkBlue;
		((Control)RbtnEndGharardad).Location = new Point(247, 10);
		((Control)RbtnEndGharardad).Name = "RbtnEndGharardad";
		((Control)RbtnEndGharardad).Size = new Size(104, 16);
		((Control)RbtnEndGharardad).TabIndex = 110;
		((Control)RbtnEndGharardad).Text = "[پایان قرارداد]";
		((ButtonBase)RbtnEndGharardad).UseVisualStyleBackColor = true;
		RbtnEndGharardad.CheckedChanged += Rb_vacation_CheckedChanged;
		((Control)RbtnEndMonth).Anchor = (AnchorStyles)9;
		((Control)RbtnEndMonth).AutoSize = true;
		((Control)RbtnEndMonth).Enabled = false;
		((Control)RbtnEndMonth).ForeColor = Color.DarkBlue;
		((Control)RbtnEndMonth).Location = new Point(99, 10);
		((Control)RbtnEndMonth).Name = "RbtnEndMonth";
		((Control)RbtnEndMonth).Size = new Size(94, 16);
		((Control)RbtnEndMonth).TabIndex = 109;
		((Control)RbtnEndMonth).Text = "[پایان هر ماه]";
		((ButtonBase)RbtnEndMonth).UseVisualStyleBackColor = true;
		RbtnEndMonth.CheckedChanged += Rb_vacation_CheckedChanged;
		((Control)GbxSalaryType).Anchor = (AnchorStyles)9;
		((Control)GbxSalaryType).BackgroundImageLayout = (ImageLayout)0;
		((Control)GbxSalaryType).Controls.Add((Control)(object)labelX79);
		((Control)GbxSalaryType).Controls.Add((Control)(object)RbtnMonthly);
		((Control)GbxSalaryType).Controls.Add((Control)(object)RbtnDaily);
		((Control)GbxSalaryType).Controls.Add((Control)(object)RbtnSaati);
		GbxSalaryType.FlatStyle = (FlatStyle)0;
		((Control)GbxSalaryType).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)GbxSalaryType).ForeColor = Color.Navy;
		((Control)GbxSalaryType).Location = new Point(8, 147);
		((Control)GbxSalaryType).Name = "GbxSalaryType";
		((Control)GbxSalaryType).Size = new Size(545, 30);
		((Control)GbxSalaryType).TabIndex = 112;
		GbxSalaryType.TabStop = false;
		((Control)labelX79).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX79).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX79).Location = new Point(410, 7);
		((Control)labelX79).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX79).Name = "labelX79";
		((Control)labelX79).Size = new Size(133, 19);
		((Control)labelX79).TabIndex = 114;
		((Control)labelX79).Text = "[نحوه محاسبه حقوق]:";
		labelX79.TextAlignment = (StringAlignment)2;
		((Control)RbtnMonthly).Anchor = (AnchorStyles)9;
		((Control)RbtnMonthly).AutoSize = true;
		((Control)RbtnMonthly).Enabled = false;
		((Control)RbtnMonthly).ForeColor = Color.DarkBlue;
		((Control)RbtnMonthly).Location = new Point(297, 9);
		((Control)RbtnMonthly).Name = "RbtnMonthly";
		((Control)RbtnMonthly).Size = new Size(105, 16);
		((Control)RbtnMonthly).TabIndex = 0;
		((Control)RbtnMonthly).Text = "[حقوق ثابت]";
		((ButtonBase)RbtnMonthly).UseVisualStyleBackColor = true;
		RbtnMonthly.CheckedChanged += Rb_CheckedChanged;
		((Control)RbtnDaily).Anchor = (AnchorStyles)9;
		((Control)RbtnDaily).AutoSize = true;
		((Control)RbtnDaily).Enabled = false;
		((Control)RbtnDaily).ForeColor = Color.DarkBlue;
		((Control)RbtnDaily).Location = new Point(144, 9);
		((Control)RbtnDaily).Name = "RbtnDaily";
		((Control)RbtnDaily).Size = new Size(125, 16);
		((Control)RbtnDaily).TabIndex = 0;
		((Control)RbtnDaily).Text = "[حقوق روزانه]";
		((ButtonBase)RbtnDaily).UseVisualStyleBackColor = true;
		RbtnDaily.CheckedChanged += Rb_CheckedChanged;
		((Control)RbtnSaati).Anchor = (AnchorStyles)9;
		((Control)RbtnSaati).AutoSize = true;
		((Control)RbtnSaati).Enabled = false;
		((Control)RbtnSaati).ForeColor = Color.DarkBlue;
		((Control)RbtnSaati).Location = new Point(11, 9);
		((Control)RbtnSaati).Name = "RbtnSaati";
		((Control)RbtnSaati).Size = new Size(102, 16);
		((Control)RbtnSaati).TabIndex = 0;
		((Control)RbtnSaati).Text = "[حقوق ساعتی]";
		((ButtonBase)RbtnSaati).UseVisualStyleBackColor = true;
		RbtnSaati.CheckedChanged += Rb_CheckedChanged;
		((Control)NU_Jarime).Anchor = (AnchorStyles)9;
		((Control)NU_Jarime).Location = new Point(349, 578);
		NU_Jarime.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		((Control)NU_Jarime).Name = "NU_Jarime";
		((Control)NU_Jarime).Size = new Size(42, 21);
		((Control)NU_Jarime).TabIndex = 108;
		((Control)labelX75).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX75).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX75).Location = new Point(302, 578);
		((Control)labelX75).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX75).Name = "labelX75";
		((Control)labelX75).Size = new Size(42, 21);
		((Control)labelX75).TabIndex = 106;
		((Control)labelX75).Text = "[روز]";
		((Control)labelX55).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX55).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX55).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX55).Location = new Point(398, 579);
		((Control)labelX55).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX55).Name = "labelX55";
		((Control)labelX55).Size = new Size(150, 21);
		((Control)labelX55).TabIndex = 99;
		((Control)labelX55).Text = "[جریمه تاخیر]:";
		labelX55.TextAlignment = (StringAlignment)2;
		((Control)labelX56).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX56).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX56).Location = new Point(123, 579);
		((Control)labelX56).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX56).Name = "labelX56";
		((Control)labelX56).Size = new Size(33, 21);
		((Control)labelX56).TabIndex = 100;
		((Control)labelX56).Text = "[جمع] :";
		labelX56.TextAlignment = (StringAlignment)2;
		((Control)labelX77).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX77).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX77).Location = new Point(255, 579);
		((Control)labelX77).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX77).Name = "labelX77";
		((Control)labelX77).Size = new Size(31, 21);
		((Control)labelX77).TabIndex = 101;
		((Control)labelX77).Text = "[مبلغ] :";
		labelX77.TextAlignment = (StringAlignment)2;
		TXTJarimeSum.AllowEmpty = true;
		((Control)TXTJarimeSum).Anchor = (AnchorStyles)9;
		((Control)TXTJarimeSum).BackColor = Color.LightGray;
		TXTJarimeSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTJarimeSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTJarimeSum.BackcolorTooltip = Color.Red;
		TXTJarimeSum.CurrencyFormat = "";
		((Control)TXTJarimeSum).ForeColor = Color.Red;
		TXTJarimeSum.ForeColorTooltip = Color.Black;
		TXTJarimeSum.GetColoresFromSettingDLL = true;
		TXTJarimeSum.HandleKeydown = true;
		((Control)TXTJarimeSum).Location = new Point(7, 579);
		((TextBoxBase)TXTJarimeSum).MaxLength = 19;
		((Control)TXTJarimeSum).Name = "TXTJarimeSum";
		((TextBoxBase)TXTJarimeSum).ReadOnly = true;
		TXTJarimeSum.SendShiftTabAfterDown = true;
		TXTJarimeSum.SendShiftTabAfterEnter = true;
		TXTJarimeSum.SendTabAfterEnter = true;
		TXTJarimeSum.SendTabAfterUP = true;
		TXTJarimeSum.ShowHorofi = (ENShowHorofi)3;
		TXTJarimeSum.ShowToolTipText = true;
		((Control)TXTJarimeSum).Size = new Size(115, 21);
		((Control)TXTJarimeSum).TabIndex = 105;
		((Control)TXTJarimeSum).TabStop = false;
		((Control)TXTJarimeSum).Text = "0";
		TXTJarimeSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTJarimeSum.ToolTipPostFix = "";
		TXTJarimeSum.TootTipDelay = 1000;
		TXTJarimeSum.Value = new decimal(new int[4]);
		((Control)TXTJarimeSum).Enter += TXT_Leave;
		((Control)TXTJarimeSum).Leave += TXT_Leave;
		TXTJarimePrice.AllowEmpty = true;
		((Control)TXTJarimePrice).Anchor = (AnchorStyles)9;
		((Control)TXTJarimePrice).BackColor = Color.White;
		TXTJarimePrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTJarimePrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTJarimePrice.BackcolorTooltip = Color.Red;
		TXTJarimePrice.CurrencyFormat = "";
		((Control)TXTJarimePrice).ForeColor = Color.Red;
		TXTJarimePrice.ForeColorTooltip = Color.Black;
		TXTJarimePrice.GetColoresFromSettingDLL = true;
		TXTJarimePrice.HandleKeydown = true;
		((Control)TXTJarimePrice).Location = new Point(162, 579);
		((TextBoxBase)TXTJarimePrice).MaxLength = 19;
		((Control)TXTJarimePrice).Name = "TXTJarimePrice";
		TXTJarimePrice.SendShiftTabAfterDown = true;
		TXTJarimePrice.SendShiftTabAfterEnter = true;
		TXTJarimePrice.SendTabAfterEnter = true;
		TXTJarimePrice.SendTabAfterUP = true;
		TXTJarimePrice.ShowHorofi = (ENShowHorofi)3;
		TXTJarimePrice.ShowToolTipText = true;
		((Control)TXTJarimePrice).Size = new Size(91, 21);
		((Control)TXTJarimePrice).TabIndex = 104;
		((Control)TXTJarimePrice).Text = "0";
		TXTJarimePrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTJarimePrice.ToolTipPostFix = "";
		TXTJarimePrice.TootTipDelay = 1000;
		TXTJarimePrice.Value = new decimal(new int[4]);
		((Control)TXTJarimePrice).TextChanged += Jarime_Changed;
		((Control)labelX21).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX21).Location = new Point(398, 553);
		((Control)labelX21).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(150, 21);
		((Control)labelX21).TabIndex = 91;
		((Control)labelX21).Text = "[تعطیل کاری] :";
		labelX21.TextAlignment = (StringAlignment)2;
		((Control)labelX28).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX28).Location = new Point(124, 553);
		((Control)labelX28).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX28).Name = "labelX28";
		((Control)labelX28).Size = new Size(33, 21);
		((Control)labelX28).TabIndex = 92;
		((Control)labelX28).Text = "[جمع] :";
		labelX28.TextAlignment = (StringAlignment)2;
		((Control)labelX31).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(338, 553);
		((Control)labelX31).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(13, 21);
		((Control)labelX31).TabIndex = 98;
		((Control)labelX31).Text = ":";
		labelX31.TextAlignment = (StringAlignment)1;
		((Control)labelX49).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX49).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX49).Location = new Point(256, 553);
		((Control)labelX49).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX49).Name = "labelX49";
		((Control)labelX49).Size = new Size(31, 21);
		((Control)labelX49).TabIndex = 93;
		((Control)labelX49).Text = "[مبلغ] :";
		labelX49.TextAlignment = (StringAlignment)2;
		((Control)TxtVacationRemind).Anchor = (AnchorStyles)9;
		((Control)TxtVacationRemind).Location = new Point(7, 628);
		((Control)TxtVacationRemind).Name = "TxtVacationRemind";
		TxtVacationRemind.ReadOnly = true;
		((Control)TxtVacationRemind).Size = new Size(57, 21);
		((Control)TxtVacationRemind).TabIndex = 90;
		((Control)TxtVacationRemind).TextChanged += TxtVacationRemind_TextChanged;
		((Control)Lbl11).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl11).Location = new Point(69, 627);
		((Control)Lbl11).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl11).Name = "Lbl11";
		((Control)Lbl11).Size = new Size(42, 21);
		((Control)Lbl11).TabIndex = 89;
		((Control)Lbl11).Text = "[روز]";
		((Control)Lbl10).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl10).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)Lbl10).Location = new Point(122, 627);
		((Control)Lbl10).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl10).Name = "Lbl10";
		((Control)Lbl10).Size = new Size(173, 24);
		((Control)Lbl10).TabIndex = 88;
		((Control)Lbl10).Text = "[مانده مرخصی تا پایان قرارداد]";
		Lbl10.TextAlignment = (StringAlignment)1;
		Lbl10.WordWrap = true;
		((Control)NumDailyVacation).Anchor = (AnchorStyles)9;
		((Control)NumDailyVacation).Location = new Point(350, 631);
		NumDailyVacation.Maximum = new decimal(new int[4] { 200, 0, 0, 0 });
		((Control)NumDailyVacation).Name = "NumDailyVacation";
		((Control)NumDailyVacation).Size = new Size(41, 21);
		((Control)NumDailyVacation).TabIndex = 48;
		NumDailyVacation.ValueChanged += VacationUsed_Changed;
		((Control)labelX78).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX78).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX78).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX78).Location = new Point(398, 631);
		((Control)labelX78).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX78).Name = "labelX78";
		((Control)labelX78).Size = new Size(150, 45);
		((Control)labelX78).TabIndex = 0;
		((Control)labelX78).Text = "[مرخصی استفاده شده]";
		labelX78.TextAlignment = (StringAlignment)2;
		labelX78.WordWrap = true;
		((Control)Lbl1).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)Lbl1).Location = new Point(398, 673);
		((Control)Lbl1).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl1).Name = "Lbl1";
		((Control)Lbl1).Size = new Size(150, 32);
		((Control)Lbl1).TabIndex = 0;
		((Control)Lbl1).Text = "[حق مرخصی در ماه]";
		Lbl1.TextAlignment = (StringAlignment)2;
		Lbl1.WordWrap = true;
		((Control)Lbl2).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl2).Location = new Point(337, 679);
		((Control)Lbl2).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl2).Name = "Lbl2";
		((Control)Lbl2).Size = new Size(13, 21);
		((Control)Lbl2).TabIndex = 77;
		((Control)Lbl2).Text = ":";
		Lbl2.TextAlignment = (StringAlignment)1;
		((Control)NUMVacation).Anchor = (AnchorStyles)9;
		((Control)NUMVacation).Location = new Point(349, 679);
		NUMVacation.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMVacation).Name = "NUMVacation";
		((Control)NUMVacation).Size = new Size(42, 21);
		((Control)NUMVacation).TabIndex = 46;
		NUMVacation.ValueChanged += Vacation_Changed;
		((Control)NUHVacation).Anchor = (AnchorStyles)9;
		((Control)NUHVacation).Location = new Point(294, 679);
		NUHVacation.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHVacation).Name = "NUHVacation";
		((Control)NUHVacation).Size = new Size(43, 21);
		((Control)NUHVacation).TabIndex = 47;
		NUHVacation.ValueChanged += Vacation_Changed;
		((Control)LblCountMorHourly).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountMorHourly).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountMorHourly).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountMorHourly).ForeColor = Color.Red;
		((Control)LblCountMorHourly).Location = new Point(298, 107);
		((Control)LblCountMorHourly).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountMorHourly).Name = "LblCountMorHourly";
		((Control)LblCountMorHourly).Size = new Size(40, 19);
		((Control)LblCountMorHourly).TabIndex = 0;
		((Control)LblCountMorHourly).Text = "0";
		((Control)Lbl4).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl4).Location = new Point(254, 652);
		((Control)Lbl4).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl4).Name = "Lbl4";
		((Control)Lbl4).Size = new Size(33, 21);
		((Control)Lbl4).TabIndex = 0;
		((Control)Lbl4).Text = "[مبلغ] :";
		Lbl4.TextAlignment = (StringAlignment)2;
		((Control)Lbl3).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl3).Location = new Point(123, 679);
		((Control)Lbl3).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl3).Name = "Lbl3";
		((Control)Lbl3).Size = new Size(33, 21);
		((Control)Lbl3).TabIndex = 0;
		((Control)Lbl3).Text = "[جمع] :";
		Lbl3.TextAlignment = (StringAlignment)2;
		((Control)labelX57).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX57).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX57).Location = new Point(337, 654);
		((Control)labelX57).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX57).Name = "labelX57";
		((Control)labelX57).Size = new Size(13, 21);
		((Control)labelX57).TabIndex = 87;
		((Control)labelX57).Text = ":";
		labelX57.TextAlignment = (StringAlignment)1;
		TXTVacationSum.AllowEmpty = true;
		((Control)TXTVacationSum).Anchor = (AnchorStyles)9;
		((Control)TXTVacationSum).BackColor = Color.LightGray;
		TXTVacationSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTVacationSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTVacationSum.BackcolorTooltip = Color.Red;
		TXTVacationSum.CurrencyFormat = "";
		((Control)TXTVacationSum).ForeColor = Color.DarkGreen;
		TXTVacationSum.ForeColorTooltip = Color.Black;
		TXTVacationSum.GetColoresFromSettingDLL = true;
		TXTVacationSum.HandleKeydown = true;
		((Control)TXTVacationSum).Location = new Point(7, 679);
		((TextBoxBase)TXTVacationSum).MaxLength = 19;
		((Control)TXTVacationSum).Name = "TXTVacationSum";
		((TextBoxBase)TXTVacationSum).ReadOnly = true;
		TXTVacationSum.SendShiftTabAfterDown = true;
		TXTVacationSum.SendShiftTabAfterEnter = true;
		TXTVacationSum.SendTabAfterEnter = true;
		TXTVacationSum.SendTabAfterUP = true;
		TXTVacationSum.ShowHorofi = (ENShowHorofi)3;
		TXTVacationSum.ShowToolTipText = true;
		((Control)TXTVacationSum).Size = new Size(115, 21);
		((Control)TXTVacationSum).TabIndex = 47;
		((Control)TXTVacationSum).TabStop = false;
		((Control)TXTVacationSum).Text = "0";
		TXTVacationSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTVacationSum.ToolTipPostFix = "";
		TXTVacationSum.TootTipDelay = 1000;
		TXTVacationSum.Value = new decimal(new int[4]);
		((Control)TXTVacationSum).Enter += TXT_Leave;
		((Control)TXTVacationSum).Leave += TXT_Leave;
		((Control)labelX59).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX59).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX59).Location = new Point(344, 107);
		((Control)labelX59).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX59).Name = "labelX59";
		((Control)labelX59).Size = new Size(204, 19);
		((Control)labelX59).TabIndex = 0;
		((Control)labelX59).Text = "[مرخصی ساعتی]:";
		labelX59.TextAlignment = (StringAlignment)2;
		((Control)Lbl5).Anchor = (AnchorStyles)9;
		((BaseItemControl)Lbl5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl5).Location = new Point(122, 653);
		((Control)Lbl5).Margin = new Padding(3, 2, 3, 2);
		((Control)Lbl5).Name = "Lbl5";
		((Control)Lbl5).Size = new Size(33, 21);
		((Control)Lbl5).TabIndex = 0;
		((Control)Lbl5).Text = "[جمع] :";
		Lbl5.TextAlignment = (StringAlignment)2;
		((Control)NU_Mission).Anchor = (AnchorStyles)9;
		((Control)NU_Mission).Location = new Point(350, 606);
		((Control)NU_Mission).Name = "NU_Mission";
		((Control)NU_Mission).RightToLeft = (RightToLeft)1;
		((Control)NU_Mission).Size = new Size(41, 21);
		((Control)NU_Mission).TabIndex = 43;
		NU_Mission.ValidatingType = typeof(int);
		((Control)NU_Mission).TextChanged += Mission_Changed;
		((Control)NUH_ClocklyVacation).Anchor = (AnchorStyles)9;
		((Control)NUH_ClocklyVacation).Location = new Point(294, 655);
		NUH_ClocklyVacation.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUH_ClocklyVacation).Name = "NUH_ClocklyVacation";
		((Control)NUH_ClocklyVacation).Size = new Size(44, 21);
		((Control)NUH_ClocklyVacation).TabIndex = 50;
		NUH_ClocklyVacation.ValueChanged += VacationUsed_Changed;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX1).Location = new Point(398, 606);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(150, 21);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[ماموریت] :";
		labelX1.TextAlignment = (StringAlignment)2;
		TXTVacationPrice.AllowEmpty = true;
		((Control)TXTVacationPrice).Anchor = (AnchorStyles)9;
		((Control)TXTVacationPrice).BackColor = Color.White;
		TXTVacationPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTVacationPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTVacationPrice.BackcolorTooltip = Color.Red;
		TXTVacationPrice.CurrencyFormat = "";
		((Control)TXTVacationPrice).ForeColor = Color.ForestGreen;
		TXTVacationPrice.ForeColorTooltip = Color.Black;
		TXTVacationPrice.GetColoresFromSettingDLL = true;
		TXTVacationPrice.HandleKeydown = true;
		((Control)TXTVacationPrice).Location = new Point(163, 653);
		((TextBoxBase)TXTVacationPrice).MaxLength = 19;
		((TextBoxBase)TXTVacationPrice).Multiline = true;
		((Control)TXTVacationPrice).Name = "TXTVacationPrice";
		TXTVacationPrice.SendShiftTabAfterDown = true;
		TXTVacationPrice.SendShiftTabAfterEnter = true;
		TXTVacationPrice.SendTabAfterEnter = true;
		TXTVacationPrice.SendTabAfterUP = true;
		TXTVacationPrice.ShowHorofi = (ENShowHorofi)3;
		TXTVacationPrice.ShowToolTipText = true;
		((Control)TXTVacationPrice).Size = new Size(89, 21);
		((Control)TXTVacationPrice).TabIndex = 51;
		((Control)TXTVacationPrice).Text = "0";
		TXTVacationPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTVacationPrice.ToolTipPostFix = "";
		TXTVacationPrice.TootTipDelay = 1000;
		TXTVacationPrice.Value = new decimal(new int[4]);
		((Control)TXTVacationPrice).TextChanged += Vacation_Changed;
		TXTVacationSumUsed.AllowEmpty = true;
		((Control)TXTVacationSumUsed).Anchor = (AnchorStyles)9;
		((Control)TXTVacationSumUsed).BackColor = Color.LightGray;
		TXTVacationSumUsed.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTVacationSumUsed.BackColorTextBoxWhenLostFocus = Color.White;
		TXTVacationSumUsed.BackcolorTooltip = Color.Red;
		TXTVacationSumUsed.CurrencyFormat = "";
		((Control)TXTVacationSumUsed).ForeColor = Color.Red;
		TXTVacationSumUsed.ForeColorTooltip = Color.Red;
		TXTVacationSumUsed.GetColoresFromSettingDLL = true;
		TXTVacationSumUsed.HandleKeydown = true;
		((Control)TXTVacationSumUsed).Location = new Point(6, 653);
		((TextBoxBase)TXTVacationSumUsed).MaxLength = 19;
		((Control)TXTVacationSumUsed).Name = "TXTVacationSumUsed";
		((TextBoxBase)TXTVacationSumUsed).ReadOnly = true;
		TXTVacationSumUsed.SendShiftTabAfterDown = true;
		TXTVacationSumUsed.SendShiftTabAfterEnter = true;
		TXTVacationSumUsed.SendTabAfterEnter = true;
		TXTVacationSumUsed.SendTabAfterUP = true;
		TXTVacationSumUsed.ShowHorofi = (ENShowHorofi)3;
		TXTVacationSumUsed.ShowToolTipText = true;
		((Control)TXTVacationSumUsed).Size = new Size(115, 21);
		((Control)TXTVacationSumUsed).TabIndex = 52;
		((Control)TXTVacationSumUsed).TabStop = false;
		((Control)TXTVacationSumUsed).Text = "0";
		TXTVacationSumUsed.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTVacationSumUsed.ToolTipPostFix = "";
		TXTVacationSumUsed.TootTipDelay = 1000;
		TXTVacationSumUsed.Value = new decimal(new int[4]);
		((Control)TXTVacationSumUsed).Enter += TXT_Leave;
		((Control)TXTVacationSumUsed).Leave += TXT_Leave;
		TXTMissionSum.AllowEmpty = true;
		((Control)TXTMissionSum).Anchor = (AnchorStyles)9;
		((Control)TXTMissionSum).BackColor = Color.LightGray;
		TXTMissionSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTMissionSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTMissionSum.BackcolorTooltip = Color.Red;
		TXTMissionSum.CurrencyFormat = "";
		((Control)TXTMissionSum).ForeColor = Color.DarkGreen;
		TXTMissionSum.ForeColorTooltip = Color.Black;
		TXTMissionSum.GetColoresFromSettingDLL = true;
		TXTMissionSum.HandleKeydown = true;
		((Control)TXTMissionSum).Location = new Point(7, 606);
		((TextBoxBase)TXTMissionSum).MaxLength = 19;
		((Control)TXTMissionSum).Name = "TXTMissionSum";
		((TextBoxBase)TXTMissionSum).ReadOnly = true;
		TXTMissionSum.SendShiftTabAfterDown = true;
		TXTMissionSum.SendShiftTabAfterEnter = true;
		TXTMissionSum.SendTabAfterEnter = true;
		TXTMissionSum.SendTabAfterUP = true;
		TXTMissionSum.ShowHorofi = (ENShowHorofi)3;
		TXTMissionSum.ShowToolTipText = true;
		((Control)TXTMissionSum).Size = new Size(115, 21);
		((Control)TXTMissionSum).TabIndex = 45;
		((Control)TXTMissionSum).TabStop = false;
		((Control)TXTMissionSum).Text = "0";
		TXTMissionSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTMissionSum.ToolTipPostFix = "";
		TXTMissionSum.TootTipDelay = 1000;
		TXTMissionSum.Value = new decimal(new int[4]);
		((Control)TXTMissionSum).Enter += TXT_Leave;
		((Control)TXTMissionSum).Leave += TXT_Leave;
		((Control)labelX24).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Location = new Point(123, 606);
		((Control)labelX24).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).Size = new Size(33, 21);
		((Control)labelX24).TabIndex = 0;
		((Control)labelX24).Text = "[جمع] :";
		labelX24.TextAlignment = (StringAlignment)2;
		((Control)labelX76).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX76).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX76).Location = new Point(301, 631);
		((Control)labelX76).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX76).Name = "labelX76";
		((Control)labelX76).Size = new Size(45, 21);
		((Control)labelX76).TabIndex = 0;
		((Control)labelX76).Text = "[روز]";
		TXTMissionPrice.AllowEmpty = true;
		((Control)TXTMissionPrice).Anchor = (AnchorStyles)9;
		((Control)TXTMissionPrice).BackColor = Color.White;
		TXTMissionPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTMissionPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTMissionPrice.BackcolorTooltip = Color.Red;
		TXTMissionPrice.CurrencyFormat = "";
		((Control)TXTMissionPrice).ForeColor = Color.ForestGreen;
		TXTMissionPrice.ForeColorTooltip = Color.Black;
		TXTMissionPrice.GetColoresFromSettingDLL = true;
		TXTMissionPrice.HandleKeydown = true;
		((Control)TXTMissionPrice).Location = new Point(162, 606);
		((TextBoxBase)TXTMissionPrice).MaxLength = 19;
		((Control)TXTMissionPrice).Name = "TXTMissionPrice";
		TXTMissionPrice.SendShiftTabAfterDown = true;
		TXTMissionPrice.SendShiftTabAfterEnter = true;
		TXTMissionPrice.SendTabAfterEnter = true;
		TXTMissionPrice.SendTabAfterUP = true;
		TXTMissionPrice.ShowHorofi = (ENShowHorofi)3;
		TXTMissionPrice.ShowToolTipText = true;
		((Control)TXTMissionPrice).Size = new Size(91, 21);
		((Control)TXTMissionPrice).TabIndex = 44;
		((Control)TXTMissionPrice).Text = "0";
		TXTMissionPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTMissionPrice.ToolTipPostFix = "";
		TXTMissionPrice.TootTipDelay = 1000;
		TXTMissionPrice.Value = new decimal(new int[4]);
		((Control)TXTMissionPrice).TextChanged += Mission_Changed;
		((Control)labelX48).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX48).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX48).Location = new Point(301, 605);
		((Control)labelX48).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX48).Name = "labelX48";
		((Control)labelX48).Size = new Size(42, 21);
		((Control)labelX48).TabIndex = 0;
		((Control)labelX48).Text = "[روز]";
		((Control)labelX25).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Location = new Point(255, 606);
		((Control)labelX25).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).Size = new Size(44, 21);
		((Control)labelX25).TabIndex = 0;
		((Control)labelX25).Text = "[مبلغ] :";
		labelX25.TextAlignment = (StringAlignment)2;
		TXTKarkerdSum.AllowEmpty = true;
		((Control)TXTKarkerdSum).Anchor = (AnchorStyles)9;
		((Control)TXTKarkerdSum).BackColor = Color.LightGray;
		TXTKarkerdSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKarkerdSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKarkerdSum.BackcolorTooltip = Color.Red;
		TXTKarkerdSum.CurrencyFormat = "";
		((Control)TXTKarkerdSum).ForeColor = Color.DarkGreen;
		TXTKarkerdSum.ForeColorTooltip = Color.Black;
		TXTKarkerdSum.GetColoresFromSettingDLL = true;
		TXTKarkerdSum.HandleKeydown = true;
		((Control)TXTKarkerdSum).Location = new Point(8, 216);
		((TextBoxBase)TXTKarkerdSum).MaxLength = 19;
		((Control)TXTKarkerdSum).Name = "TXTKarkerdSum";
		((TextBoxBase)TXTKarkerdSum).ReadOnly = true;
		TXTKarkerdSum.SendShiftTabAfterDown = true;
		TXTKarkerdSum.SendShiftTabAfterEnter = true;
		TXTKarkerdSum.SendTabAfterEnter = true;
		TXTKarkerdSum.SendTabAfterUP = true;
		TXTKarkerdSum.ShowHorofi = (ENShowHorofi)3;
		TXTKarkerdSum.ShowToolTipText = true;
		((Control)TXTKarkerdSum).Size = new Size(115, 21);
		((Control)TXTKarkerdSum).TabIndex = 3;
		((Control)TXTKarkerdSum).TabStop = false;
		((Control)TXTKarkerdSum).Text = "0";
		TXTKarkerdSum.ToolTipFont = new Font("B Traffic", 5f, (FontStyle)1);
		TXTKarkerdSum.ToolTipPostFix = "";
		TXTKarkerdSum.TootTipDelay = 1000;
		TXTKarkerdSum.Value = new decimal(new int[4]);
		((Control)TXTKarkerdSum).Enter += TXT_Leave;
		((Control)TXTKarkerdSum).Leave += TXT_Leave;
		((Control)NUM_ClocklyVacation).Anchor = (AnchorStyles)9;
		((Control)NUM_ClocklyVacation).Location = new Point(349, 655);
		NUM_ClocklyVacation.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUM_ClocklyVacation).Name = "NUM_ClocklyVacation";
		((Control)NUM_ClocklyVacation).Size = new Size(42, 21);
		((Control)NUM_ClocklyVacation).TabIndex = 49;
		NUM_ClocklyVacation.ValueChanged += VacationUsed_Changed;
		TXTEzafeSum3.AllowEmpty = true;
		((Control)TXTEzafeSum3).Anchor = (AnchorStyles)9;
		((Control)TXTEzafeSum3).BackColor = Color.LightGray;
		TXTEzafeSum3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafeSum3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafeSum3.BackcolorTooltip = Color.Red;
		TXTEzafeSum3.CurrencyFormat = "";
		((Control)TXTEzafeSum3).ForeColor = Color.DarkGreen;
		TXTEzafeSum3.ForeColorTooltip = Color.Black;
		TXTEzafeSum3.GetColoresFromSettingDLL = true;
		TXTEzafeSum3.HandleKeydown = true;
		((Control)TXTEzafeSum3).Location = new Point(8, 295);
		((TextBoxBase)TXTEzafeSum3).MaxLength = 19;
		((Control)TXTEzafeSum3).Name = "TXTEzafeSum3";
		((TextBoxBase)TXTEzafeSum3).ReadOnly = true;
		TXTEzafeSum3.SendShiftTabAfterDown = true;
		TXTEzafeSum3.SendShiftTabAfterEnter = true;
		TXTEzafeSum3.SendTabAfterEnter = true;
		TXTEzafeSum3.SendTabAfterUP = true;
		TXTEzafeSum3.ShowHorofi = (ENShowHorofi)3;
		TXTEzafeSum3.ShowToolTipText = true;
		((Control)TXTEzafeSum3).Size = new Size(115, 21);
		((Control)TXTEzafeSum3).TabIndex = 15;
		((Control)TXTEzafeSum3).TabStop = false;
		((Control)TXTEzafeSum3).Text = "0";
		TXTEzafeSum3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafeSum3.ToolTipPostFix = "";
		TXTEzafeSum3.TootTipDelay = 1000;
		TXTEzafeSum3.Value = new decimal(new int[4]);
		((Control)TXTEzafeSum3).Enter += TXT_Leave;
		((Control)TXTEzafeSum3).Leave += TXT_Leave;
		TXTEzafeSum2.AllowEmpty = true;
		((Control)TXTEzafeSum2).Anchor = (AnchorStyles)9;
		((Control)TXTEzafeSum2).BackColor = Color.LightGray;
		TXTEzafeSum2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafeSum2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafeSum2.BackcolorTooltip = Color.Red;
		TXTEzafeSum2.CurrencyFormat = "";
		((Control)TXTEzafeSum2).ForeColor = Color.DarkGreen;
		TXTEzafeSum2.ForeColorTooltip = Color.Black;
		TXTEzafeSum2.GetColoresFromSettingDLL = true;
		TXTEzafeSum2.HandleKeydown = true;
		((Control)TXTEzafeSum2).Location = new Point(8, 269);
		((TextBoxBase)TXTEzafeSum2).MaxLength = 19;
		((Control)TXTEzafeSum2).Name = "TXTEzafeSum2";
		((TextBoxBase)TXTEzafeSum2).ReadOnly = true;
		TXTEzafeSum2.SendShiftTabAfterDown = true;
		TXTEzafeSum2.SendShiftTabAfterEnter = true;
		TXTEzafeSum2.SendTabAfterEnter = true;
		TXTEzafeSum2.SendTabAfterUP = true;
		TXTEzafeSum2.ShowHorofi = (ENShowHorofi)3;
		TXTEzafeSum2.ShowToolTipText = true;
		((Control)TXTEzafeSum2).Size = new Size(115, 21);
		((Control)TXTEzafeSum2).TabIndex = 11;
		((Control)TXTEzafeSum2).TabStop = false;
		((Control)TXTEzafeSum2).Text = "0";
		TXTEzafeSum2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafeSum2.ToolTipPostFix = "";
		TXTEzafeSum2.TootTipDelay = 1000;
		TXTEzafeSum2.Value = new decimal(new int[4]);
		((Control)TXTEzafeSum2).Enter += TXT_Leave;
		((Control)TXTEzafeSum2).Leave += TXT_Leave;
		TXTEzafeSum1.AllowEmpty = true;
		((Control)TXTEzafeSum1).Anchor = (AnchorStyles)9;
		((Control)TXTEzafeSum1).BackColor = Color.LightGray;
		TXTEzafeSum1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafeSum1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafeSum1.BackcolorTooltip = Color.Red;
		TXTEzafeSum1.CurrencyFormat = "";
		((Control)TXTEzafeSum1).ForeColor = Color.DarkGreen;
		TXTEzafeSum1.ForeColorTooltip = Color.Black;
		TXTEzafeSum1.GetColoresFromSettingDLL = true;
		TXTEzafeSum1.HandleKeydown = true;
		((Control)TXTEzafeSum1).Location = new Point(8, 243);
		((TextBoxBase)TXTEzafeSum1).MaxLength = 19;
		((Control)TXTEzafeSum1).Name = "TXTEzafeSum1";
		((TextBoxBase)TXTEzafeSum1).ReadOnly = true;
		TXTEzafeSum1.SendShiftTabAfterDown = true;
		TXTEzafeSum1.SendShiftTabAfterEnter = true;
		TXTEzafeSum1.SendTabAfterEnter = true;
		TXTEzafeSum1.SendTabAfterUP = true;
		TXTEzafeSum1.ShowHorofi = (ENShowHorofi)3;
		TXTEzafeSum1.ShowToolTipText = true;
		((Control)TXTEzafeSum1).Size = new Size(115, 21);
		((Control)TXTEzafeSum1).TabIndex = 7;
		((Control)TXTEzafeSum1).TabStop = false;
		((Control)TXTEzafeSum1).Text = "0";
		TXTEzafeSum1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafeSum1.ToolTipPostFix = "";
		TXTEzafeSum1.TootTipDelay = 1000;
		TXTEzafeSum1.Value = new decimal(new int[4]);
		((Control)TXTEzafeSum1).Enter += TXT_Leave;
		((Control)TXTEzafeSum1).Leave += TXT_Leave;
		((Control)LblCountMorNotHoghogh).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountMorNotHoghogh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountMorNotHoghogh).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountMorNotHoghogh).ForeColor = Color.Red;
		((Control)LblCountMorNotHoghogh).Location = new Point(7, 83);
		((Control)LblCountMorNotHoghogh).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountMorNotHoghogh).Name = "LblCountMorNotHoghogh";
		((Control)LblCountMorNotHoghogh).Size = new Size(40, 19);
		((Control)LblCountMorNotHoghogh).TabIndex = 0;
		((Control)LblCountMorNotHoghogh).Text = "0";
		((Control)LblCountMorWithHoghogh).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountMorWithHoghogh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountMorWithHoghogh).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountMorWithHoghogh).ForeColor = Color.Red;
		((Control)LblCountMorWithHoghogh).Location = new Point(298, 83);
		((Control)LblCountMorWithHoghogh).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountMorWithHoghogh).Name = "LblCountMorWithHoghogh";
		((Control)LblCountMorWithHoghogh).Size = new Size(40, 19);
		((Control)LblCountMorWithHoghogh).TabIndex = 0;
		((Control)LblCountMorWithHoghogh).Text = "0";
		TXTShabSum.AllowEmpty = true;
		((Control)TXTShabSum).Anchor = (AnchorStyles)9;
		((Control)TXTShabSum).BackColor = Color.LightGray;
		TXTShabSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTShabSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTShabSum.BackcolorTooltip = Color.Red;
		TXTShabSum.CurrencyFormat = "";
		((Control)TXTShabSum).ForeColor = Color.DarkGreen;
		TXTShabSum.ForeColorTooltip = Color.Black;
		TXTShabSum.GetColoresFromSettingDLL = true;
		TXTShabSum.HandleKeydown = true;
		((Control)TXTShabSum).Location = new Point(8, 427);
		((TextBoxBase)TXTShabSum).MaxLength = 19;
		((Control)TXTShabSum).Name = "TXTShabSum";
		((TextBoxBase)TXTShabSum).ReadOnly = true;
		TXTShabSum.SendShiftTabAfterDown = true;
		TXTShabSum.SendShiftTabAfterEnter = true;
		TXTShabSum.SendTabAfterEnter = true;
		TXTShabSum.SendTabAfterUP = true;
		TXTShabSum.ShowHorofi = (ENShowHorofi)3;
		TXTShabSum.ShowToolTipText = true;
		((Control)TXTShabSum).Size = new Size(115, 21);
		((Control)TXTShabSum).TabIndex = 34;
		((Control)TXTShabSum).TabStop = false;
		((Control)TXTShabSum).Text = "0";
		TXTShabSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTShabSum.ToolTipPostFix = "";
		TXTShabSum.TootTipDelay = 1000;
		TXTShabSum.Value = new decimal(new int[4]);
		((Control)TXTShabSum).Enter += TXT_Leave;
		((Control)TXTShabSum).Leave += TXT_Leave;
		((Control)labelX74).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX74).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX74).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX74).Location = new Point(398, 374);
		((Control)labelX74).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX74).Name = "labelX74";
		((Control)labelX74).Size = new Size(150, 21);
		((Control)labelX74).TabIndex = 0;
		((Control)labelX74).Text = "[کسر کار آخر]";
		labelX74.TextAlignment = (StringAlignment)2;
		((Control)labelX70).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX70).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX70).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX70).Location = new Point(398, 347);
		((Control)labelX70).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX70).Name = "labelX70";
		((Control)labelX70).Size = new Size(150, 21);
		((Control)labelX70).TabIndex = 0;
		((Control)labelX70).Text = "[کسر کار وسط]";
		labelX70.TextAlignment = (StringAlignment)2;
		((Control)labelX23).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX23).Location = new Point(398, 321);
		((Control)labelX23).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(150, 21);
		((Control)labelX23).TabIndex = 0;
		((Control)labelX23).Text = "[کسر کار اول]";
		labelX23.TextAlignment = (StringAlignment)2;
		((Control)labelX22).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX22).Location = new Point(398, 401);
		((Control)labelX22).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).Size = new Size(150, 21);
		((Control)labelX22).TabIndex = 61;
		((Control)labelX22).Text = "[غیبت] :";
		labelX22.TextAlignment = (StringAlignment)2;
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX18).Location = new Point(398, 529);
		((Control)labelX18).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(150, 21);
		((Control)labelX18).TabIndex = 0;
		((Control)labelX18).Text = "[جمعه کاری] :";
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)LblNobatKari1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNobatKari1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNobatKari1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)LblNobatKari1).Location = new Point(398, 452);
		((Control)LblNobatKari1).Margin = new Padding(3, 2, 3, 2);
		((Control)LblNobatKari1).Name = "LblNobatKari1";
		((Control)LblNobatKari1).Size = new Size(150, 21);
		((Control)LblNobatKari1).TabIndex = 0;
		((Control)LblNobatKari1).Text = "نوبت صبح:";
		LblNobatKari1.TextAlignment = (StringAlignment)2;
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX16).Location = new Point(398, 427);
		((Control)labelX16).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(150, 21);
		((Control)labelX16).TabIndex = 0;
		((Control)labelX16).Text = "[شب کاری] :";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)LblCountKarkard).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountKarkard).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountKarkard).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountKarkard).ForeColor = Color.Red;
		((Control)LblCountKarkard).Location = new Point(7, 60);
		((Control)LblCountKarkard).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountKarkard).Name = "LblCountKarkard";
		((Control)LblCountKarkard).Size = new Size(40, 19);
		((Control)LblCountKarkard).TabIndex = 0;
		((Control)LblCountKarkard).Text = "0";
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(53, 60);
		((Control)labelX39).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(233, 19);
		((Control)labelX39).TabIndex = 0;
		((Control)labelX39).Text = "[تعداد روزهای کارکرد] :";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)LblCountKol).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountKol).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountKol).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountKol).ForeColor = Color.Red;
		((Control)LblCountKol).Location = new Point(298, 60);
		((Control)LblCountKol).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountKol).Name = "LblCountKol";
		((Control)LblCountKol).Size = new Size(40, 19);
		((Control)LblCountKol).TabIndex = 0;
		((Control)LblCountKol).Text = "0";
		((Control)labelX14).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Location = new Point(344, 60);
		((Control)labelX14).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(204, 19);
		((Control)labelX14).TabIndex = 0;
		((Control)labelX14).Text = "[کل روزها] :";
		labelX14.TextAlignment = (StringAlignment)2;
		((Control)labelX32).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX32).Location = new Point(53, 83);
		((Control)labelX32).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX32).Name = "labelX32";
		((Control)labelX32).Size = new Size(233, 19);
		((Control)labelX32).TabIndex = 0;
		((Control)labelX32).Text = "[مرخصی/ماموریت بدون حقوق]:";
		labelX32.TextAlignment = (StringAlignment)2;
		((Control)labelX54).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX54).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX54).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX54).Location = new Point(344, 83);
		((Control)labelX54).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX54).Name = "labelX54";
		((Control)labelX54).Size = new Size(204, 19);
		((Control)labelX54).TabIndex = 0;
		((Control)labelX54).Text = "[مرخصی/ماموریت با حقوق]:";
		labelX54.TextAlignment = (StringAlignment)2;
		((Control)LblCountGheybat).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCountGheybat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountGheybat).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblCountGheybat).ForeColor = Color.Red;
		((Control)LblCountGheybat).Location = new Point(7, 106);
		((Control)LblCountGheybat).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCountGheybat).Name = "LblCountGheybat";
		((Control)LblCountGheybat).Size = new Size(40, 19);
		((Control)LblCountGheybat).TabIndex = 0;
		((Control)LblCountGheybat).Text = "0";
		TXTNobatSum1.AllowEmpty = true;
		((Control)TXTNobatSum1).Anchor = (AnchorStyles)9;
		((Control)TXTNobatSum1).BackColor = Color.LightGray;
		TXTNobatSum1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatSum1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatSum1.BackcolorTooltip = Color.Red;
		TXTNobatSum1.CurrencyFormat = "";
		((Control)TXTNobatSum1).ForeColor = Color.DarkGreen;
		TXTNobatSum1.ForeColorTooltip = Color.Black;
		TXTNobatSum1.GetColoresFromSettingDLL = true;
		TXTNobatSum1.HandleKeydown = true;
		((Control)TXTNobatSum1).Location = new Point(8, 452);
		((TextBoxBase)TXTNobatSum1).MaxLength = 19;
		((Control)TXTNobatSum1).Name = "TXTNobatSum1";
		((TextBoxBase)TXTNobatSum1).ReadOnly = true;
		TXTNobatSum1.SendShiftTabAfterDown = true;
		TXTNobatSum1.SendShiftTabAfterEnter = true;
		TXTNobatSum1.SendTabAfterEnter = true;
		TXTNobatSum1.SendTabAfterUP = true;
		TXTNobatSum1.ShowHorofi = (ENShowHorofi)3;
		TXTNobatSum1.ShowToolTipText = true;
		((Control)TXTNobatSum1).Size = new Size(115, 21);
		((Control)TXTNobatSum1).TabIndex = 38;
		((Control)TXTNobatSum1).TabStop = false;
		((Control)TXTNobatSum1).Text = "0";
		TXTNobatSum1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatSum1.ToolTipPostFix = "";
		TXTNobatSum1.TootTipDelay = 1000;
		TXTNobatSum1.Value = new decimal(new int[4]);
		((Control)TXTNobatSum1).Enter += TXT_Leave;
		((Control)TXTNobatSum1).Leave += TXT_Leave;
		((Control)labelX50).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX50).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX50).Location = new Point(53, 106);
		((Control)labelX50).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX50).Name = "labelX50";
		((Control)labelX50).Size = new Size(233, 19);
		((Control)labelX50).TabIndex = 0;
		((Control)labelX50).Text = "[تعداد روزهای غیبت] :";
		labelX50.TextAlignment = (StringAlignment)2;
		TXTJomeSum.AllowEmpty = true;
		((Control)TXTJomeSum).Anchor = (AnchorStyles)9;
		((Control)TXTJomeSum).BackColor = Color.LightGray;
		TXTJomeSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTJomeSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTJomeSum.BackcolorTooltip = Color.Red;
		TXTJomeSum.CurrencyFormat = "";
		((Control)TXTJomeSum).ForeColor = Color.DarkGreen;
		TXTJomeSum.ForeColorTooltip = Color.Black;
		TXTJomeSum.GetColoresFromSettingDLL = true;
		TXTJomeSum.HandleKeydown = true;
		((Control)TXTJomeSum).Location = new Point(8, 529);
		((TextBoxBase)TXTJomeSum).MaxLength = 19;
		((Control)TXTJomeSum).Name = "TXTJomeSum";
		((TextBoxBase)TXTJomeSum).ReadOnly = true;
		TXTJomeSum.SendShiftTabAfterDown = true;
		TXTJomeSum.SendShiftTabAfterEnter = true;
		TXTJomeSum.SendTabAfterEnter = true;
		TXTJomeSum.SendTabAfterUP = true;
		TXTJomeSum.ShowHorofi = (ENShowHorofi)3;
		TXTJomeSum.ShowToolTipText = true;
		((Control)TXTJomeSum).Size = new Size(115, 21);
		((Control)TXTJomeSum).TabIndex = 42;
		((Control)TXTJomeSum).TabStop = false;
		((Control)TXTJomeSum).Text = "0";
		TXTJomeSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTJomeSum.ToolTipPostFix = "";
		TXTJomeSum.TootTipDelay = 1000;
		TXTJomeSum.Value = new decimal(new int[4]);
		((Control)TXTJomeSum).Enter += TXT_Leave;
		((Control)TXTJomeSum).Leave += TXT_Leave;
		TXTTatilSum.AllowEmpty = true;
		((Control)TXTTatilSum).Anchor = (AnchorStyles)9;
		((Control)TXTTatilSum).BackColor = Color.LightGray;
		TXTTatilSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTTatilSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTTatilSum.BackcolorTooltip = Color.Red;
		TXTTatilSum.CurrencyFormat = "";
		((Control)TXTTatilSum).ForeColor = Color.DarkGreen;
		TXTTatilSum.ForeColorTooltip = Color.Black;
		TXTTatilSum.GetColoresFromSettingDLL = true;
		TXTTatilSum.HandleKeydown = true;
		((Control)TXTTatilSum).Location = new Point(8, 553);
		((TextBoxBase)TXTTatilSum).MaxLength = 19;
		((Control)TXTTatilSum).Name = "TXTTatilSum";
		((TextBoxBase)TXTTatilSum).ReadOnly = true;
		TXTTatilSum.SendShiftTabAfterDown = true;
		TXTTatilSum.SendShiftTabAfterEnter = true;
		TXTTatilSum.SendTabAfterEnter = true;
		TXTTatilSum.SendTabAfterUP = true;
		TXTTatilSum.ShowHorofi = (ENShowHorofi)3;
		TXTTatilSum.ShowToolTipText = true;
		((Control)TXTTatilSum).Size = new Size(115, 21);
		((Control)TXTTatilSum).TabIndex = 97;
		((Control)TXTTatilSum).TabStop = false;
		((Control)TXTTatilSum).Text = "0";
		TXTTatilSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTTatilSum.ToolTipPostFix = "";
		TXTTatilSum.TootTipDelay = 1000;
		TXTTatilSum.Value = new decimal(new int[4]);
		((Control)labelX66).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX66).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX66).Location = new Point(124, 295);
		((Control)labelX66).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX66).Name = "labelX66";
		((Control)labelX66).Size = new Size(33, 21);
		((Control)labelX66).TabIndex = 0;
		((Control)labelX66).Text = "[جمع] :";
		labelX66.TextAlignment = (StringAlignment)2;
		TXTKasrSum3.AllowEmpty = true;
		((Control)TXTKasrSum3).Anchor = (AnchorStyles)9;
		((Control)TXTKasrSum3).BackColor = Color.LightGray;
		TXTKasrSum3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrSum3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrSum3.BackcolorTooltip = Color.Red;
		TXTKasrSum3.CurrencyFormat = "";
		((Control)TXTKasrSum3).ForeColor = Color.Red;
		TXTKasrSum3.ForeColorTooltip = Color.Red;
		TXTKasrSum3.GetColoresFromSettingDLL = true;
		TXTKasrSum3.HandleKeydown = true;
		((Control)TXTKasrSum3).Location = new Point(8, 374);
		((TextBoxBase)TXTKasrSum3).MaxLength = 19;
		((Control)TXTKasrSum3).Name = "TXTKasrSum3";
		((TextBoxBase)TXTKasrSum3).ReadOnly = true;
		TXTKasrSum3.SendShiftTabAfterDown = true;
		TXTKasrSum3.SendShiftTabAfterEnter = true;
		TXTKasrSum3.SendTabAfterEnter = true;
		TXTKasrSum3.SendTabAfterUP = true;
		TXTKasrSum3.ShowHorofi = (ENShowHorofi)3;
		TXTKasrSum3.ShowToolTipText = true;
		((Control)TXTKasrSum3).Size = new Size(115, 21);
		((Control)TXTKasrSum3).TabIndex = 27;
		((Control)TXTKasrSum3).TabStop = false;
		((Control)TXTKasrSum3).Text = "0";
		TXTKasrSum3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrSum3.ToolTipPostFix = "";
		TXTKasrSum3.TootTipDelay = 1000;
		TXTKasrSum3.Value = new decimal(new int[4]);
		((Control)TXTKasrSum3).Enter += TXT_Leave;
		((Control)TXTKasrSum3).Leave += TXT_Leave;
		TXTKasrSum2.AllowEmpty = true;
		((Control)TXTKasrSum2).Anchor = (AnchorStyles)9;
		((Control)TXTKasrSum2).BackColor = Color.LightGray;
		TXTKasrSum2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrSum2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrSum2.BackcolorTooltip = Color.Red;
		TXTKasrSum2.CurrencyFormat = "";
		((Control)TXTKasrSum2).ForeColor = Color.Red;
		TXTKasrSum2.ForeColorTooltip = Color.Red;
		TXTKasrSum2.GetColoresFromSettingDLL = true;
		TXTKasrSum2.HandleKeydown = true;
		((Control)TXTKasrSum2).Location = new Point(8, 347);
		((TextBoxBase)TXTKasrSum2).MaxLength = 19;
		((Control)TXTKasrSum2).Name = "TXTKasrSum2";
		((TextBoxBase)TXTKasrSum2).ReadOnly = true;
		TXTKasrSum2.SendShiftTabAfterDown = true;
		TXTKasrSum2.SendShiftTabAfterEnter = true;
		TXTKasrSum2.SendTabAfterEnter = true;
		TXTKasrSum2.SendTabAfterUP = true;
		TXTKasrSum2.ShowHorofi = (ENShowHorofi)3;
		TXTKasrSum2.ShowToolTipText = true;
		((Control)TXTKasrSum2).Size = new Size(115, 21);
		((Control)TXTKasrSum2).TabIndex = 23;
		((Control)TXTKasrSum2).TabStop = false;
		((Control)TXTKasrSum2).Text = "0";
		TXTKasrSum2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrSum2.ToolTipPostFix = "";
		TXTKasrSum2.TootTipDelay = 1000;
		TXTKasrSum2.Value = new decimal(new int[4]);
		((Control)TXTKasrSum2).Enter += TXT_Leave;
		((Control)TXTKasrSum2).Leave += TXT_Leave;
		TXTKasrSum1.AllowEmpty = true;
		((Control)TXTKasrSum1).Anchor = (AnchorStyles)9;
		((Control)TXTKasrSum1).BackColor = Color.LightGray;
		TXTKasrSum1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrSum1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrSum1.BackcolorTooltip = Color.Red;
		TXTKasrSum1.CurrencyFormat = "";
		((Control)TXTKasrSum1).ForeColor = Color.Red;
		TXTKasrSum1.ForeColorTooltip = Color.Red;
		TXTKasrSum1.GetColoresFromSettingDLL = true;
		TXTKasrSum1.HandleKeydown = true;
		((Control)TXTKasrSum1).Location = new Point(8, 321);
		((TextBoxBase)TXTKasrSum1).MaxLength = 19;
		((Control)TXTKasrSum1).Name = "TXTKasrSum1";
		((TextBoxBase)TXTKasrSum1).ReadOnly = true;
		TXTKasrSum1.SendShiftTabAfterDown = true;
		TXTKasrSum1.SendShiftTabAfterEnter = true;
		TXTKasrSum1.SendTabAfterEnter = true;
		TXTKasrSum1.SendTabAfterUP = true;
		TXTKasrSum1.ShowHorofi = (ENShowHorofi)3;
		TXTKasrSum1.ShowToolTipText = true;
		((Control)TXTKasrSum1).Size = new Size(115, 21);
		((Control)TXTKasrSum1).TabIndex = 19;
		((Control)TXTKasrSum1).TabStop = false;
		((Control)TXTKasrSum1).Text = "0";
		TXTKasrSum1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrSum1.ToolTipPostFix = "";
		TXTKasrSum1.TootTipDelay = 1000;
		TXTKasrSum1.Value = new decimal(new int[4]);
		((Control)TXTKasrSum1).Enter += TXT_Leave;
		((Control)TXTKasrSum1).Leave += TXT_Leave;
		((Control)labelX62).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX62).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX62).Location = new Point(124, 269);
		((Control)labelX62).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX62).Name = "labelX62";
		((Control)labelX62).Size = new Size(33, 21);
		((Control)labelX62).TabIndex = 0;
		((Control)labelX62).Text = "[جمع] :";
		labelX62.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(124, 216);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(33, 21);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[جمع] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(124, 243);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(33, 21);
		((Control)labelX6).TabIndex = 0;
		((Control)labelX6).Text = "[جمع] :";
		labelX6.TextAlignment = (StringAlignment)2;
		TXTGheybatSum.AllowEmpty = true;
		((Control)TXTGheybatSum).Anchor = (AnchorStyles)9;
		((Control)TXTGheybatSum).BackColor = Color.LightGray;
		TXTGheybatSum.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTGheybatSum.BackColorTextBoxWhenLostFocus = Color.White;
		TXTGheybatSum.BackcolorTooltip = Color.Red;
		TXTGheybatSum.CurrencyFormat = "";
		((Control)TXTGheybatSum).ForeColor = Color.Red;
		TXTGheybatSum.ForeColorTooltip = Color.Black;
		TXTGheybatSum.GetColoresFromSettingDLL = true;
		TXTGheybatSum.HandleKeydown = true;
		((Control)TXTGheybatSum).Location = new Point(8, 401);
		((TextBoxBase)TXTGheybatSum).MaxLength = 19;
		((Control)TXTGheybatSum).Name = "TXTGheybatSum";
		((TextBoxBase)TXTGheybatSum).ReadOnly = true;
		TXTGheybatSum.SendShiftTabAfterDown = true;
		TXTGheybatSum.SendShiftTabAfterEnter = true;
		TXTGheybatSum.SendTabAfterEnter = true;
		TXTGheybatSum.SendTabAfterUP = true;
		TXTGheybatSum.ShowHorofi = (ENShowHorofi)3;
		TXTGheybatSum.ShowToolTipText = true;
		((Control)TXTGheybatSum).Size = new Size(115, 21);
		((Control)TXTGheybatSum).TabIndex = 30;
		((Control)TXTGheybatSum).TabStop = false;
		((Control)TXTGheybatSum).Text = "0";
		TXTGheybatSum.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTGheybatSum.ToolTipPostFix = "";
		TXTGheybatSum.TootTipDelay = 1000;
		TXTGheybatSum.Value = new decimal(new int[4]);
		((Control)TXTGheybatSum).Enter += TXT_Leave;
		((Control)TXTGheybatSum).Leave += TXT_Leave;
		((Control)labelX73).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX73).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX73).Location = new Point(124, 374);
		((Control)labelX73).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX73).Name = "labelX73";
		((Control)labelX73).Size = new Size(33, 21);
		((Control)labelX73).TabIndex = 0;
		((Control)labelX73).Text = "[جمع] :";
		labelX73.TextAlignment = (StringAlignment)2;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(124, 427);
		((Control)labelX8).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(33, 21);
		((Control)labelX8).TabIndex = 0;
		((Control)labelX8).Text = "[جمع] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX69).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX69).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX69).Location = new Point(124, 347);
		((Control)labelX69).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX69).Name = "labelX69";
		((Control)labelX69).Size = new Size(33, 21);
		((Control)labelX69).TabIndex = 0;
		((Control)labelX69).Text = "[جمع] :";
		labelX69.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(124, 452);
		((Control)labelX10).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(33, 21);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "[جمع] :";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)NUHKasr3).Anchor = (AnchorStyles)9;
		((Control)NUHKasr3).Location = new Point(296, 374);
		NUHKasr3.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHKasr3).Name = "NUHKasr3";
		((Control)NUHKasr3).Size = new Size(43, 21);
		((Control)NUHKasr3).TabIndex = 25;
		NUHKasr3.ValueChanged += KasrKar_Changed;
		((Control)labelX29).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Location = new Point(124, 321);
		((Control)labelX29).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).Size = new Size(33, 21);
		((Control)labelX29).TabIndex = 0;
		((Control)labelX29).Text = "[جمع] :";
		labelX29.TextAlignment = (StringAlignment)2;
		((Control)NUHKasr2).Anchor = (AnchorStyles)9;
		((Control)NUHKasr2).Location = new Point(296, 347);
		NUHKasr2.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHKasr2).Name = "NUHKasr2";
		((Control)NUHKasr2).Size = new Size(43, 21);
		((Control)NUHKasr2).TabIndex = 21;
		NUHKasr2.ValueChanged += KasrKar_Changed;
		((Control)labelX30).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX30).Location = new Point(124, 401);
		((Control)labelX30).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX30).Name = "labelX30";
		((Control)labelX30).Size = new Size(33, 21);
		((Control)labelX30).TabIndex = 0;
		((Control)labelX30).Text = "[جمع] :";
		labelX30.TextAlignment = (StringAlignment)2;
		((Control)labelX72).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX72).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX72).Location = new Point(338, 374);
		((Control)labelX72).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX72).Name = "labelX72";
		((Control)labelX72).Size = new Size(13, 21);
		((Control)labelX72).TabIndex = 69;
		((Control)labelX72).Text = ":";
		labelX72.TextAlignment = (StringAlignment)1;
		((Control)NUHKasr1).Anchor = (AnchorStyles)9;
		((Control)NUHKasr1).Location = new Point(296, 321);
		NUHKasr1.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHKasr1).Name = "NUHKasr1";
		((Control)NUHKasr1).Size = new Size(43, 21);
		((Control)NUHKasr1).TabIndex = 17;
		NUHKasr1.ValueChanged += KasrKar_Changed;
		((Control)labelX68).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX68).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX68).Location = new Point(338, 347);
		((Control)labelX68).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX68).Name = "labelX68";
		((Control)labelX68).Size = new Size(13, 21);
		((Control)labelX68).TabIndex = 69;
		((Control)labelX68).Text = ":";
		labelX68.TextAlignment = (StringAlignment)1;
		((Control)NUMKasr3).Anchor = (AnchorStyles)9;
		((Control)NUMKasr3).Location = new Point(350, 374);
		NUMKasr3.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMKasr3).Name = "NUMKasr3";
		((Control)NUMKasr3).Size = new Size(42, 21);
		((Control)NUMKasr3).TabIndex = 24;
		NUMKasr3.ValueChanged += KasrKar_Changed;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(124, 529);
		((Control)labelX12).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(33, 21);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[جمع] :";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)NUMKasr2).Anchor = (AnchorStyles)9;
		((Control)NUMKasr2).Location = new Point(350, 347);
		NUMKasr2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMKasr2).Name = "NUMKasr2";
		((Control)NUMKasr2).Size = new Size(42, 21);
		((Control)NUMKasr2).TabIndex = 20;
		NUMKasr2.ValueChanged += KasrKar_Changed;
		((Control)labelX51).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX51).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX51).Location = new Point(338, 321);
		((Control)labelX51).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX51).Name = "labelX51";
		((Control)labelX51).Size = new Size(13, 21);
		((Control)labelX51).TabIndex = 16;
		((Control)labelX51).Text = ":";
		labelX51.TextAlignment = (StringAlignment)1;
		((Control)NUMKasr1).Anchor = (AnchorStyles)9;
		((Control)NUMKasr1).Location = new Point(350, 321);
		NUMKasr1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMKasr1).Name = "NUMKasr1";
		((Control)NUMKasr1).Size = new Size(42, 21);
		((Control)NUMKasr1).TabIndex = 16;
		NUMKasr1.ValueChanged += KasrKar_Changed;
		((Control)NUHJome).Anchor = (AnchorStyles)9;
		((Control)NUHJome).Location = new Point(296, 529);
		NUHJome.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHJome).Name = "NUHJome";
		((Control)NUHJome).Size = new Size(43, 21);
		((Control)NUHJome).TabIndex = 40;
		NUHJome.ValueChanged += JomeKari_Changed;
		((Control)NUHTatil).Anchor = (AnchorStyles)9;
		((Control)NUHTatil).Location = new Point(296, 553);
		NUHTatil.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHTatil).Name = "NUHTatil";
		((Control)NUHTatil).Size = new Size(43, 21);
		((Control)NUHTatil).TabIndex = 95;
		NUHTatil.ValueChanged += TatilKari_Changed;
		((Control)NUHNobat1).Anchor = (AnchorStyles)9;
		((Control)NUHNobat1).Location = new Point(296, 452);
		NUHNobat1.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHNobat1).Name = "NUHNobat1";
		((Control)NUHNobat1).Size = new Size(43, 21);
		((Control)NUHNobat1).TabIndex = 36;
		NUHNobat1.ValueChanged += NobatKari1_Changed;
		((Control)NU_Gheybat).Anchor = (AnchorStyles)9;
		((Control)NU_Gheybat).Location = new Point(296, 401);
		NU_Gheybat.Maximum = new decimal(new int[4] { 200, 0, 0, 0 });
		((Control)NU_Gheybat).Name = "NU_Gheybat";
		((Control)NU_Gheybat).Size = new Size(96, 21);
		((Control)NU_Gheybat).TabIndex = 28;
		NU_Gheybat.ValueChanged += Gheybat_Changed;
		((Control)NUHEzafe3).Anchor = (AnchorStyles)9;
		((Control)NUHEzafe3).Location = new Point(296, 295);
		NUHEzafe3.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHEzafe3).Name = "NUHEzafe3";
		((Control)NUHEzafe3).Size = new Size(43, 21);
		((Control)NUHEzafe3).TabIndex = 13;
		NUHEzafe3.ValueChanged += EzafeKar_Changed;
		((Control)NUHShab).Anchor = (AnchorStyles)9;
		((Control)NUHShab).Location = new Point(296, 427);
		NUHShab.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHShab).Name = "NUHShab";
		((Control)NUHShab).Size = new Size(43, 21);
		((Control)NUHShab).TabIndex = 32;
		NUHShab.ValueChanged += ShabKari_Changed;
		((Control)NUHEzafe2).Anchor = (AnchorStyles)9;
		((Control)NUHEzafe2).Location = new Point(296, 269);
		NUHEzafe2.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHEzafe2).Name = "NUHEzafe2";
		((Control)NUHEzafe2).Size = new Size(43, 21);
		((Control)NUHEzafe2).TabIndex = 9;
		NUHEzafe2.ValueChanged += EzafeKar_Changed;
		((Control)DotJome).Anchor = (AnchorStyles)9;
		((BaseItemControl)DotJome).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)DotJome).Location = new Point(338, 529);
		((Control)DotJome).Margin = new Padding(3, 2, 3, 2);
		((Control)DotJome).Name = "DotJome";
		((Control)DotJome).Size = new Size(13, 21);
		((Control)DotJome).TabIndex = 49;
		((Control)DotJome).Text = ":";
		DotJome.TextAlignment = (StringAlignment)1;
		((Control)NUHEzafe1).Anchor = (AnchorStyles)9;
		((Control)NUHEzafe1).Location = new Point(296, 243);
		NUHEzafe1.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHEzafe1).Name = "NUHEzafe1";
		((Control)NUHEzafe1).Size = new Size(43, 21);
		((Control)NUHEzafe1).TabIndex = 5;
		NUHEzafe1.ValueChanged += EzafeKar_Changed;
		((Control)LblKarkerd).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblKarkerd).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKarkerd).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)LblKarkerd).Location = new Point(398, 216);
		((Control)LblKarkerd).Margin = new Padding(3, 2, 3, 2);
		((Control)LblKarkerd).Name = "LblKarkerd";
		((Control)LblKarkerd).Size = new Size(150, 21);
		((Control)LblKarkerd).TabIndex = 0;
		((Control)LblKarkerd).Text = "کارکرد:";
		LblKarkerd.TextAlignment = (StringAlignment)2;
		((Control)LblKarkerd).Visible = false;
		((Control)labelX65).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX65).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX65).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX65).Location = new Point(398, 295);
		((Control)labelX65).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX65).Name = "labelX65";
		((Control)labelX65).Size = new Size(150, 21);
		((Control)labelX65).TabIndex = 0;
		((Control)labelX65).Text = "[اضافه کار آخر]";
		labelX65.TextAlignment = (StringAlignment)2;
		((Control)NUHKarkerd).Anchor = (AnchorStyles)9;
		((Control)NUHKarkerd).Location = new Point(295, 216);
		NUHKarkerd.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		((Control)NUHKarkerd).Name = "NUHKarkerd";
		((Control)NUHKarkerd).Size = new Size(43, 21);
		((Control)NUHKarkerd).TabIndex = 1;
		((Control)NUHKarkerd).Visible = false;
		NUHKarkerd.ValueChanged += Karkerd_Changed;
		((Control)labelX61).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX61).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX61).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX61).Location = new Point(398, 269);
		((Control)labelX61).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX61).Name = "labelX61";
		((Control)labelX61).Size = new Size(150, 21);
		((Control)labelX61).TabIndex = 0;
		((Control)labelX61).Text = "[اضافه کار وسط]";
		labelX61.TextAlignment = (StringAlignment)2;
		((Control)labelX47).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX47).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX47).Location = new Point(338, 452);
		((Control)labelX47).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX47).Name = "labelX47";
		((Control)labelX47).Size = new Size(13, 21);
		((Control)labelX47).TabIndex = 41;
		((Control)labelX47).Text = ":";
		labelX47.TextAlignment = (StringAlignment)1;
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX19).Location = new Point(398, 235);
		((Control)labelX19).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(150, 30);
		((Control)labelX19).TabIndex = 0;
		((Control)labelX19).Text = "[اضافه کار اول]";
		labelX19.TextAlignment = (StringAlignment)2;
		labelX19.WordWrap = true;
		((Control)NUMEzafe3).Anchor = (AnchorStyles)9;
		((Control)NUMEzafe3).Location = new Point(350, 295);
		NUMEzafe3.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMEzafe3).Name = "NUMEzafe3";
		((Control)NUMEzafe3).Size = new Size(42, 21);
		((Control)NUMEzafe3).TabIndex = 12;
		NUMEzafe3.ValueChanged += EzafeKar_Changed;
		((Control)NUMKarkerd).Anchor = (AnchorStyles)9;
		((Control)NUMKarkerd).Location = new Point(350, 216);
		NUMKarkerd.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMKarkerd).Name = "NUMKarkerd";
		((Control)NUMKarkerd).Size = new Size(42, 21);
		((Control)NUMKarkerd).TabIndex = 0;
		((Control)NUMKarkerd).Visible = false;
		NUMKarkerd.ValueChanged += Karkerd_Changed;
		((Control)NUMEzafe2).Anchor = (AnchorStyles)9;
		((Control)NUMEzafe2).Location = new Point(350, 269);
		NUMEzafe2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMEzafe2).Name = "NUMEzafe2";
		((Control)NUMEzafe2).Size = new Size(42, 21);
		((Control)NUMEzafe2).TabIndex = 8;
		NUMEzafe2.ValueChanged += EzafeKar_Changed;
		((Control)labelX46).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX46).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX46).Location = new Point(338, 427);
		((Control)labelX46).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX46).Name = "labelX46";
		((Control)labelX46).Size = new Size(13, 21);
		((Control)labelX46).TabIndex = 33;
		((Control)labelX46).Text = ":";
		labelX46.TextAlignment = (StringAlignment)1;
		((Control)NUMEzafe1).Anchor = (AnchorStyles)9;
		((Control)NUMEzafe1).Location = new Point(350, 243);
		NUMEzafe1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMEzafe1).Name = "NUMEzafe1";
		((Control)NUMEzafe1).Size = new Size(42, 21);
		((Control)NUMEzafe1).TabIndex = 4;
		NUMEzafe1.ValueChanged += EzafeKar_Changed;
		((Control)NUMShab).Anchor = (AnchorStyles)9;
		((Control)NUMShab).Location = new Point(350, 426);
		NUMShab.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMShab).Name = "NUMShab";
		((Control)NUMShab).Size = new Size(42, 21);
		((Control)NUMShab).TabIndex = 31;
		NUMShab.ValueChanged += ShabKari_Changed;
		((Control)labelX64).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX64).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX64).Location = new Point(338, 295);
		((Control)labelX64).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX64).Name = "labelX64";
		((Control)labelX64).Size = new Size(13, 21);
		((Control)labelX64).TabIndex = 25;
		((Control)labelX64).Text = ":";
		labelX64.TextAlignment = (StringAlignment)1;
		((Control)NUMJome).Anchor = (AnchorStyles)9;
		((Control)NUMJome).Location = new Point(350, 529);
		NUMJome.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMJome).Name = "NUMJome";
		((Control)NUMJome).Size = new Size(42, 21);
		((Control)NUMJome).TabIndex = 39;
		NUMJome.ValueChanged += JomeKari_Changed;
		((Control)NUMTatil).Anchor = (AnchorStyles)9;
		((Control)NUMTatil).Location = new Point(350, 553);
		NUMTatil.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMTatil).Name = "NUMTatil";
		((Control)NUMTatil).Size = new Size(42, 21);
		((Control)NUMTatil).TabIndex = 94;
		NUMTatil.ValueChanged += TatilKari_Changed;
		((Control)labelX60).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX60).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX60).Location = new Point(338, 269);
		((Control)labelX60).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX60).Name = "labelX60";
		((Control)labelX60).Size = new Size(13, 21);
		((Control)labelX60).TabIndex = 25;
		((Control)labelX60).Text = ":";
		labelX60.TextAlignment = (StringAlignment)1;
		((Control)NUMNobat1).Anchor = (AnchorStyles)9;
		((Control)NUMNobat1).Location = new Point(349, 452);
		NUMNobat1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)NUMNobat1).Name = "NUMNobat1";
		((Control)NUMNobat1).Size = new Size(42, 21);
		((Control)NUMNobat1).TabIndex = 35;
		NUMNobat1.ValueChanged += NobatKari1_Changed;
		((Control)labelX45).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX45).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX45).Location = new Point(338, 243);
		((Control)labelX45).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX45).Name = "labelX45";
		((Control)labelX45).Size = new Size(13, 21);
		((Control)labelX45).TabIndex = 25;
		((Control)labelX45).Text = ":";
		labelX45.TextAlignment = (StringAlignment)1;
		((Control)LblDot).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblDot).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDot).Location = new Point(338, 216);
		((Control)LblDot).Margin = new Padding(3, 2, 3, 2);
		((Control)LblDot).Name = "LblDot";
		((Control)LblDot).Size = new Size(13, 21);
		((Control)LblDot).TabIndex = 17;
		((Control)LblDot).Text = ":";
		LblDot.TextAlignment = (StringAlignment)1;
		((Control)LblDot).Visible = false;
		((Control)Counttaradodnaghes).Anchor = (AnchorStyles)9;
		((BaseItemControl)Counttaradodnaghes).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Counttaradodnaghes).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Counttaradodnaghes).ForeColor = Color.Red;
		((Control)Counttaradodnaghes).Location = new Point(7, 129);
		((Control)Counttaradodnaghes).Margin = new Padding(3, 2, 3, 2);
		((Control)Counttaradodnaghes).Name = "Counttaradodnaghes";
		((Control)Counttaradodnaghes).Size = new Size(40, 19);
		((Control)Counttaradodnaghes).TabIndex = 0;
		((Control)Counttaradodnaghes).Text = "0";
		((Control)labelX52).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX52).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX52).Location = new Point(53, 129);
		((Control)labelX52).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX52).Name = "labelX52";
		((Control)labelX52).Size = new Size(233, 19);
		((Control)labelX52).TabIndex = 0;
		((Control)labelX52).Text = "[تعداد ترددهای ناقص] :";
		labelX52.TextAlignment = (StringAlignment)2;
		TXTEzafePrice3.AllowEmpty = true;
		((Control)TXTEzafePrice3).Anchor = (AnchorStyles)9;
		((Control)TXTEzafePrice3).BackColor = Color.White;
		TXTEzafePrice3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafePrice3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafePrice3.BackcolorTooltip = Color.Red;
		TXTEzafePrice3.CurrencyFormat = "";
		((Control)TXTEzafePrice3).ForeColor = Color.ForestGreen;
		TXTEzafePrice3.ForeColorTooltip = Color.Black;
		TXTEzafePrice3.GetColoresFromSettingDLL = true;
		TXTEzafePrice3.HandleKeydown = true;
		((Control)TXTEzafePrice3).Location = new Point(163, 295);
		((TextBoxBase)TXTEzafePrice3).MaxLength = 19;
		((Control)TXTEzafePrice3).Name = "TXTEzafePrice3";
		TXTEzafePrice3.SendShiftTabAfterDown = true;
		TXTEzafePrice3.SendShiftTabAfterEnter = true;
		TXTEzafePrice3.SendTabAfterEnter = true;
		TXTEzafePrice3.SendTabAfterUP = true;
		TXTEzafePrice3.ShowHorofi = (ENShowHorofi)3;
		TXTEzafePrice3.ShowToolTipText = true;
		((Control)TXTEzafePrice3).Size = new Size(91, 21);
		((Control)TXTEzafePrice3).TabIndex = 14;
		((Control)TXTEzafePrice3).Text = "0";
		TXTEzafePrice3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafePrice3.ToolTipPostFix = "";
		TXTEzafePrice3.TootTipDelay = 1000;
		TXTEzafePrice3.Value = new decimal(new int[4]);
		((TextBoxBase)TXTEzafePrice3).WordWrap = false;
		((Control)TXTEzafePrice3).TextChanged += EzafeKar_Changed;
		TXTJomePrice.AllowEmpty = true;
		((Control)TXTJomePrice).Anchor = (AnchorStyles)9;
		((Control)TXTJomePrice).BackColor = Color.White;
		TXTJomePrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTJomePrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTJomePrice.BackcolorTooltip = Color.Red;
		TXTJomePrice.CurrencyFormat = "";
		((Control)TXTJomePrice).ForeColor = Color.ForestGreen;
		TXTJomePrice.ForeColorTooltip = Color.Black;
		TXTJomePrice.GetColoresFromSettingDLL = true;
		TXTJomePrice.HandleKeydown = true;
		((Control)TXTJomePrice).Location = new Point(163, 529);
		((TextBoxBase)TXTJomePrice).MaxLength = 19;
		((Control)TXTJomePrice).Name = "TXTJomePrice";
		TXTJomePrice.SendShiftTabAfterDown = true;
		TXTJomePrice.SendShiftTabAfterEnter = true;
		TXTJomePrice.SendTabAfterEnter = true;
		TXTJomePrice.SendTabAfterUP = true;
		TXTJomePrice.ShowHorofi = (ENShowHorofi)3;
		TXTJomePrice.ShowToolTipText = true;
		((Control)TXTJomePrice).Size = new Size(91, 21);
		((Control)TXTJomePrice).TabIndex = 41;
		((Control)TXTJomePrice).Text = "0";
		TXTJomePrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTJomePrice.ToolTipPostFix = "";
		TXTJomePrice.TootTipDelay = 1000;
		TXTJomePrice.Value = new decimal(new int[4]);
		((Control)TXTJomePrice).TextChanged += JomeKari_Changed;
		TXTTatilPrice.AllowEmpty = true;
		((Control)TXTTatilPrice).Anchor = (AnchorStyles)9;
		((Control)TXTTatilPrice).BackColor = Color.White;
		TXTTatilPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTTatilPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTTatilPrice.BackcolorTooltip = Color.Red;
		TXTTatilPrice.CurrencyFormat = "";
		((Control)TXTTatilPrice).ForeColor = Color.ForestGreen;
		TXTTatilPrice.ForeColorTooltip = Color.Black;
		TXTTatilPrice.GetColoresFromSettingDLL = true;
		TXTTatilPrice.HandleKeydown = true;
		((Control)TXTTatilPrice).Location = new Point(163, 553);
		((TextBoxBase)TXTTatilPrice).MaxLength = 19;
		((Control)TXTTatilPrice).Name = "TXTTatilPrice";
		TXTTatilPrice.SendShiftTabAfterDown = true;
		TXTTatilPrice.SendShiftTabAfterEnter = true;
		TXTTatilPrice.SendTabAfterEnter = true;
		TXTTatilPrice.SendTabAfterUP = true;
		TXTTatilPrice.ShowHorofi = (ENShowHorofi)3;
		TXTTatilPrice.ShowToolTipText = true;
		((Control)TXTTatilPrice).Size = new Size(91, 21);
		((Control)TXTTatilPrice).TabIndex = 96;
		((Control)TXTTatilPrice).Text = "0";
		TXTTatilPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTTatilPrice.ToolTipPostFix = "";
		TXTTatilPrice.TootTipDelay = 1000;
		TXTTatilPrice.Value = new decimal(new int[4]);
		((Control)TXTTatilPrice).TextChanged += TatilKari_Changed;
		TXTEzafePrice2.AllowEmpty = true;
		((Control)TXTEzafePrice2).Anchor = (AnchorStyles)9;
		((Control)TXTEzafePrice2).BackColor = Color.White;
		TXTEzafePrice2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafePrice2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafePrice2.BackcolorTooltip = Color.Red;
		TXTEzafePrice2.CurrencyFormat = "";
		((Control)TXTEzafePrice2).ForeColor = Color.ForestGreen;
		TXTEzafePrice2.ForeColorTooltip = Color.Black;
		TXTEzafePrice2.GetColoresFromSettingDLL = true;
		TXTEzafePrice2.HandleKeydown = true;
		((Control)TXTEzafePrice2).Location = new Point(163, 269);
		((TextBoxBase)TXTEzafePrice2).MaxLength = 19;
		((Control)TXTEzafePrice2).Name = "TXTEzafePrice2";
		TXTEzafePrice2.SendShiftTabAfterDown = true;
		TXTEzafePrice2.SendShiftTabAfterEnter = true;
		TXTEzafePrice2.SendTabAfterEnter = true;
		TXTEzafePrice2.SendTabAfterUP = true;
		TXTEzafePrice2.ShowHorofi = (ENShowHorofi)3;
		TXTEzafePrice2.ShowToolTipText = true;
		((Control)TXTEzafePrice2).Size = new Size(91, 21);
		((Control)TXTEzafePrice2).TabIndex = 10;
		((Control)TXTEzafePrice2).Text = "0";
		TXTEzafePrice2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafePrice2.ToolTipPostFix = "";
		TXTEzafePrice2.TootTipDelay = 1000;
		TXTEzafePrice2.Value = new decimal(new int[4]);
		((Control)TXTEzafePrice2).TextChanged += EzafeKar_Changed;
		TXTKarkerdPrice.AllowEmpty = true;
		((Control)TXTKarkerdPrice).Anchor = (AnchorStyles)9;
		((Control)TXTKarkerdPrice).BackColor = Color.White;
		TXTKarkerdPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKarkerdPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKarkerdPrice.BackcolorTooltip = Color.Red;
		TXTKarkerdPrice.CurrencyFormat = "";
		((Control)TXTKarkerdPrice).ForeColor = Color.ForestGreen;
		TXTKarkerdPrice.ForeColorTooltip = Color.Black;
		TXTKarkerdPrice.GetColoresFromSettingDLL = true;
		TXTKarkerdPrice.HandleKeydown = true;
		((Control)TXTKarkerdPrice).Location = new Point(163, 216);
		((TextBoxBase)TXTKarkerdPrice).MaxLength = 19;
		((Control)TXTKarkerdPrice).Name = "TXTKarkerdPrice";
		TXTKarkerdPrice.SendShiftTabAfterDown = true;
		TXTKarkerdPrice.SendShiftTabAfterEnter = true;
		TXTKarkerdPrice.SendTabAfterEnter = true;
		TXTKarkerdPrice.SendTabAfterUP = true;
		TXTKarkerdPrice.ShowHorofi = (ENShowHorofi)3;
		TXTKarkerdPrice.ShowToolTipText = true;
		((Control)TXTKarkerdPrice).Size = new Size(91, 21);
		((Control)TXTKarkerdPrice).TabIndex = 2;
		((Control)TXTKarkerdPrice).Text = "0";
		TXTKarkerdPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKarkerdPrice.ToolTipPostFix = "";
		TXTKarkerdPrice.TootTipDelay = 1000;
		TXTKarkerdPrice.Value = new decimal(new int[4]);
		((Control)TXTKarkerdPrice).TextChanged += Karkerd_Changed;
		TXTEzafePrice1.AllowEmpty = true;
		((Control)TXTEzafePrice1).Anchor = (AnchorStyles)9;
		((Control)TXTEzafePrice1).BackColor = Color.White;
		TXTEzafePrice1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTEzafePrice1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTEzafePrice1.BackcolorTooltip = Color.Red;
		TXTEzafePrice1.CurrencyFormat = "";
		((Control)TXTEzafePrice1).ForeColor = Color.ForestGreen;
		TXTEzafePrice1.ForeColorTooltip = Color.Black;
		TXTEzafePrice1.GetColoresFromSettingDLL = true;
		TXTEzafePrice1.HandleKeydown = true;
		((Control)TXTEzafePrice1).Location = new Point(163, 243);
		((TextBoxBase)TXTEzafePrice1).MaxLength = 19;
		((Control)TXTEzafePrice1).Name = "TXTEzafePrice1";
		TXTEzafePrice1.SendShiftTabAfterDown = true;
		TXTEzafePrice1.SendShiftTabAfterEnter = true;
		TXTEzafePrice1.SendTabAfterEnter = true;
		TXTEzafePrice1.SendTabAfterUP = true;
		TXTEzafePrice1.ShowHorofi = (ENShowHorofi)3;
		TXTEzafePrice1.ShowToolTipText = true;
		((Control)TXTEzafePrice1).Size = new Size(91, 21);
		((Control)TXTEzafePrice1).TabIndex = 6;
		((Control)TXTEzafePrice1).Text = "0";
		TXTEzafePrice1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTEzafePrice1.ToolTipPostFix = "";
		TXTEzafePrice1.TootTipDelay = 1000;
		TXTEzafePrice1.Value = new decimal(new int[4]);
		((Control)TXTEzafePrice1).TextChanged += EzafeKar_Changed;
		TXTKasrPrice3.AllowEmpty = true;
		((Control)TXTKasrPrice3).Anchor = (AnchorStyles)9;
		((Control)TXTKasrPrice3).BackColor = Color.White;
		TXTKasrPrice3.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrPrice3.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrPrice3.BackcolorTooltip = Color.Red;
		TXTKasrPrice3.CurrencyFormat = "";
		((Control)TXTKasrPrice3).ForeColor = Color.Red;
		TXTKasrPrice3.ForeColorTooltip = Color.Black;
		TXTKasrPrice3.GetColoresFromSettingDLL = true;
		TXTKasrPrice3.HandleKeydown = true;
		((Control)TXTKasrPrice3).Location = new Point(163, 374);
		((TextBoxBase)TXTKasrPrice3).MaxLength = 19;
		((Control)TXTKasrPrice3).Name = "TXTKasrPrice3";
		TXTKasrPrice3.SendShiftTabAfterDown = true;
		TXTKasrPrice3.SendShiftTabAfterEnter = true;
		TXTKasrPrice3.SendTabAfterEnter = true;
		TXTKasrPrice3.SendTabAfterUP = true;
		TXTKasrPrice3.ShowHorofi = (ENShowHorofi)3;
		TXTKasrPrice3.ShowToolTipText = true;
		((Control)TXTKasrPrice3).Size = new Size(91, 21);
		((Control)TXTKasrPrice3).TabIndex = 26;
		((Control)TXTKasrPrice3).Text = "0";
		TXTKasrPrice3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrPrice3.ToolTipPostFix = "";
		TXTKasrPrice3.TootTipDelay = 1000;
		TXTKasrPrice3.Value = new decimal(new int[4]);
		((Control)TXTKasrPrice3).TextChanged += KasrKar_Changed;
		TXTShabPrice.AllowEmpty = true;
		((Control)TXTShabPrice).Anchor = (AnchorStyles)9;
		((Control)TXTShabPrice).BackColor = Color.White;
		TXTShabPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTShabPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTShabPrice.BackcolorTooltip = Color.Red;
		TXTShabPrice.CurrencyFormat = "";
		((Control)TXTShabPrice).ForeColor = Color.ForestGreen;
		TXTShabPrice.ForeColorTooltip = Color.Black;
		TXTShabPrice.GetColoresFromSettingDLL = true;
		TXTShabPrice.HandleKeydown = true;
		((Control)TXTShabPrice).Location = new Point(163, 427);
		((TextBoxBase)TXTShabPrice).MaxLength = 19;
		((Control)TXTShabPrice).Name = "TXTShabPrice";
		TXTShabPrice.SendShiftTabAfterDown = true;
		TXTShabPrice.SendShiftTabAfterEnter = true;
		TXTShabPrice.SendTabAfterEnter = true;
		TXTShabPrice.SendTabAfterUP = true;
		TXTShabPrice.ShowHorofi = (ENShowHorofi)3;
		TXTShabPrice.ShowToolTipText = true;
		((Control)TXTShabPrice).Size = new Size(91, 21);
		((Control)TXTShabPrice).TabIndex = 33;
		((Control)TXTShabPrice).Text = "0";
		TXTShabPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTShabPrice.ToolTipPostFix = "";
		TXTShabPrice.TootTipDelay = 1000;
		TXTShabPrice.Value = new decimal(new int[4]);
		((Control)TXTShabPrice).TextChanged += ShabKari_Changed;
		TXTKasrPrice2.AllowEmpty = true;
		((Control)TXTKasrPrice2).Anchor = (AnchorStyles)9;
		((Control)TXTKasrPrice2).BackColor = Color.White;
		TXTKasrPrice2.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrPrice2.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrPrice2.BackcolorTooltip = Color.Red;
		TXTKasrPrice2.CurrencyFormat = "";
		((Control)TXTKasrPrice2).ForeColor = Color.Red;
		TXTKasrPrice2.ForeColorTooltip = Color.Black;
		TXTKasrPrice2.GetColoresFromSettingDLL = true;
		TXTKasrPrice2.HandleKeydown = true;
		((Control)TXTKasrPrice2).Location = new Point(163, 347);
		((TextBoxBase)TXTKasrPrice2).MaxLength = 19;
		((Control)TXTKasrPrice2).Name = "TXTKasrPrice2";
		TXTKasrPrice2.SendShiftTabAfterDown = true;
		TXTKasrPrice2.SendShiftTabAfterEnter = true;
		TXTKasrPrice2.SendTabAfterEnter = true;
		TXTKasrPrice2.SendTabAfterUP = true;
		TXTKasrPrice2.ShowHorofi = (ENShowHorofi)3;
		TXTKasrPrice2.ShowToolTipText = true;
		((Control)TXTKasrPrice2).Size = new Size(91, 21);
		((Control)TXTKasrPrice2).TabIndex = 22;
		((Control)TXTKasrPrice2).Text = "0";
		TXTKasrPrice2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrPrice2.ToolTipPostFix = "";
		TXTKasrPrice2.TootTipDelay = 1000;
		TXTKasrPrice2.Value = new decimal(new int[4]);
		((Control)TXTKasrPrice2).TextChanged += KasrKar_Changed;
		TXTNobatPrice1.AllowEmpty = true;
		((Control)TXTNobatPrice1).Anchor = (AnchorStyles)9;
		((Control)TXTNobatPrice1).BackColor = Color.White;
		TXTNobatPrice1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTNobatPrice1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTNobatPrice1.BackcolorTooltip = Color.Red;
		TXTNobatPrice1.CurrencyFormat = "";
		((Control)TXTNobatPrice1).ForeColor = Color.ForestGreen;
		TXTNobatPrice1.ForeColorTooltip = Color.Black;
		TXTNobatPrice1.GetColoresFromSettingDLL = true;
		TXTNobatPrice1.HandleKeydown = true;
		((Control)TXTNobatPrice1).Location = new Point(163, 452);
		((TextBoxBase)TXTNobatPrice1).MaxLength = 19;
		((Control)TXTNobatPrice1).Name = "TXTNobatPrice1";
		TXTNobatPrice1.SendShiftTabAfterDown = true;
		TXTNobatPrice1.SendShiftTabAfterEnter = true;
		TXTNobatPrice1.SendTabAfterEnter = true;
		TXTNobatPrice1.SendTabAfterUP = true;
		TXTNobatPrice1.ShowHorofi = (ENShowHorofi)3;
		TXTNobatPrice1.ShowToolTipText = true;
		((Control)TXTNobatPrice1).Size = new Size(91, 21);
		((Control)TXTNobatPrice1).TabIndex = 37;
		((Control)TXTNobatPrice1).Text = "0";
		TXTNobatPrice1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTNobatPrice1.ToolTipPostFix = "";
		TXTNobatPrice1.TootTipDelay = 1000;
		TXTNobatPrice1.Value = new decimal(new int[4]);
		((Control)TXTNobatPrice1).TextChanged += NobatKari1_Changed;
		TXTKasrPrice1.AllowEmpty = true;
		((Control)TXTKasrPrice1).Anchor = (AnchorStyles)9;
		((Control)TXTKasrPrice1).BackColor = Color.White;
		TXTKasrPrice1.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKasrPrice1.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKasrPrice1.BackcolorTooltip = Color.Red;
		TXTKasrPrice1.CurrencyFormat = "";
		((Control)TXTKasrPrice1).ForeColor = Color.Red;
		TXTKasrPrice1.ForeColorTooltip = Color.Black;
		TXTKasrPrice1.GetColoresFromSettingDLL = true;
		TXTKasrPrice1.HandleKeydown = true;
		((Control)TXTKasrPrice1).Location = new Point(163, 321);
		((TextBoxBase)TXTKasrPrice1).MaxLength = 19;
		((Control)TXTKasrPrice1).Name = "TXTKasrPrice1";
		TXTKasrPrice1.SendShiftTabAfterDown = true;
		TXTKasrPrice1.SendShiftTabAfterEnter = true;
		TXTKasrPrice1.SendTabAfterEnter = true;
		TXTKasrPrice1.SendTabAfterUP = true;
		TXTKasrPrice1.ShowHorofi = (ENShowHorofi)3;
		TXTKasrPrice1.ShowToolTipText = true;
		((Control)TXTKasrPrice1).Size = new Size(91, 21);
		((Control)TXTKasrPrice1).TabIndex = 18;
		((Control)TXTKasrPrice1).Text = "0";
		TXTKasrPrice1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKasrPrice1.ToolTipPostFix = "";
		TXTKasrPrice1.TootTipDelay = 1000;
		TXTKasrPrice1.Value = new decimal(new int[4]);
		((Control)TXTKasrPrice1).TextChanged += KasrKar_Changed;
		((Control)labelX63).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX63).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX63).Location = new Point(256, 295);
		((Control)labelX63).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX63).Name = "labelX63";
		((Control)labelX63).Size = new Size(44, 21);
		((Control)labelX63).TabIndex = 0;
		((Control)labelX63).Text = "[مبلغ] :";
		labelX63.TextAlignment = (StringAlignment)2;
		TXTGheybatPrice.AllowEmpty = true;
		((Control)TXTGheybatPrice).Anchor = (AnchorStyles)9;
		((Control)TXTGheybatPrice).BackColor = Color.White;
		TXTGheybatPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTGheybatPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TXTGheybatPrice.BackcolorTooltip = Color.Red;
		TXTGheybatPrice.CurrencyFormat = "";
		((Control)TXTGheybatPrice).ForeColor = Color.Red;
		TXTGheybatPrice.ForeColorTooltip = Color.Black;
		TXTGheybatPrice.GetColoresFromSettingDLL = true;
		TXTGheybatPrice.HandleKeydown = true;
		((Control)TXTGheybatPrice).Location = new Point(163, 401);
		((TextBoxBase)TXTGheybatPrice).MaxLength = 19;
		((Control)TXTGheybatPrice).Name = "TXTGheybatPrice";
		TXTGheybatPrice.SendShiftTabAfterDown = true;
		TXTGheybatPrice.SendShiftTabAfterEnter = true;
		TXTGheybatPrice.SendTabAfterEnter = true;
		TXTGheybatPrice.SendTabAfterUP = true;
		TXTGheybatPrice.ShowHorofi = (ENShowHorofi)3;
		TXTGheybatPrice.ShowToolTipText = true;
		((Control)TXTGheybatPrice).Size = new Size(91, 21);
		((Control)TXTGheybatPrice).TabIndex = 29;
		((Control)TXTGheybatPrice).Text = "0";
		TXTGheybatPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTGheybatPrice.ToolTipPostFix = "";
		TXTGheybatPrice.TootTipDelay = 1000;
		TXTGheybatPrice.Value = new decimal(new int[4]);
		((Control)TXTGheybatPrice).TextChanged += Gheybat_Changed;
		((Control)labelX58).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX58).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX58).Location = new Point(256, 269);
		((Control)labelX58).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX58).Name = "labelX58";
		((Control)labelX58).Size = new Size(44, 21);
		((Control)labelX58).TabIndex = 0;
		((Control)labelX58).Text = "[مبلغ] :";
		labelX58.TextAlignment = (StringAlignment)2;
		((Control)lblMablagh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblMablagh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMablagh).Location = new Point(256, 216);
		((Control)lblMablagh).Margin = new Padding(3, 2, 3, 2);
		((Control)lblMablagh).Name = "lblMablagh";
		((Control)lblMablagh).Size = new Size(44, 21);
		((Control)lblMablagh).TabIndex = 0;
		((Control)lblMablagh).Text = "[حقوق] :";
		lblMablagh.TextAlignment = (StringAlignment)2;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(256, 243);
		((Control)labelX7).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(44, 21);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[مبلغ] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(256, 427);
		((Control)labelX9).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(44, 21);
		((Control)labelX9).TabIndex = 0;
		((Control)labelX9).Text = "[مبلغ] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)labelX71).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX71).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX71).Location = new Point(256, 374);
		((Control)labelX71).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX71).Name = "labelX71";
		((Control)labelX71).Size = new Size(44, 21);
		((Control)labelX71).TabIndex = 0;
		((Control)labelX71).Text = "[مبلغ] :";
		labelX71.TextAlignment = (StringAlignment)2;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(256, 452);
		((Control)labelX11).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(44, 21);
		((Control)labelX11).TabIndex = 0;
		((Control)labelX11).Text = "[مبلغ] :";
		labelX11.TextAlignment = (StringAlignment)2;
		((Control)labelX67).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX67).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX67).Location = new Point(256, 347);
		((Control)labelX67).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX67).Name = "labelX67";
		((Control)labelX67).Size = new Size(44, 21);
		((Control)labelX67).TabIndex = 0;
		((Control)labelX67).Text = "[مبلغ] :";
		labelX67.TextAlignment = (StringAlignment)2;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Location = new Point(256, 529);
		((Control)labelX13).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(44, 21);
		((Control)labelX13).TabIndex = 0;
		((Control)labelX13).Text = "[مبلغ] :";
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)labelX26).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Location = new Point(256, 321);
		((Control)labelX26).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).Size = new Size(44, 21);
		((Control)labelX26).TabIndex = 0;
		((Control)labelX26).Text = "[مبلغ] :";
		labelX26.TextAlignment = (StringAlignment)2;
		((Control)labelX27).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Location = new Point(256, 401);
		((Control)labelX27).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).Size = new Size(44, 21);
		((Control)labelX27).TabIndex = 0;
		((Control)labelX27).Text = "[مبلغ] :";
		labelX27.TextAlignment = (StringAlignment)2;
		((Control)NU_workDayMaliat).Anchor = (AnchorStyles)6;
		((Control)NU_workDayMaliat).Location = new Point(225, 655);
		NU_workDayMaliat.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		((Control)NU_workDayMaliat).Name = "NU_workDayMaliat";
		((Control)NU_workDayMaliat).Size = new Size(94, 21);
		((Control)NU_workDayMaliat).TabIndex = 55;
		NU_workDayMaliat.ValueChanged += NU_workDayMaliat_ValueChanged;
		((Control)labelX53).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX53).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX53).Location = new Point(325, 656);
		((Control)labelX53).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX53).Name = "labelX53";
		((Control)labelX53).Size = new Size(111, 21);
		((Control)labelX53).TabIndex = 0;
		((Control)labelX53).Text = "[کارکرد مالیات(روز)]:";
		labelX53.TextAlignment = (StringAlignment)2;
		((Control)NU_workDayBime).Anchor = (AnchorStyles)6;
		((Control)NU_workDayBime).Location = new Point(225, 601);
		NU_workDayBime.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		((Control)NU_workDayBime).Name = "NU_workDayBime";
		((Control)NU_workDayBime).Size = new Size(94, 21);
		((Control)NU_workDayBime).TabIndex = 52;
		NU_workDayBime.ValueChanged += NU_workDayBime_ValueChanged;
		((Control)splitContainer1).Anchor = (AnchorStyles)15;
		((Control)splitContainer1).Location = new Point(12, 93);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)DGMazaya1);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)BTNADDMazaya);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)BTNDeleteMazaya);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)BtnEditMazaya);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)groupPanel1);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)BtnEditKosorat);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)BtnAddKosorat);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)BtnDelKosor);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)DGKosorat1);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)groupPanel2);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(743, 500);
		splitContainer1.SplitterDistance = 359;
		((Control)splitContainer1).TabIndex = 2;
		((DataGridView)DGMazaya1).AllowUserToAddRows = false;
		((DataGridView)DGMazaya1).AllowUserToDeleteRows = false;
		((DataGridView)DGMazaya1).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGMazaya1).AlternatingRowsDefaultCellStyle = val;
		((Control)DGMazaya1).Anchor = (AnchorStyles)15;
		((DataGridView)DGMazaya1).AutoGenerateColumns = false;
		((DataGridView)DGMazaya1).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGMazaya1).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGMName,
			(DataGridViewColumn)DGMCredit,
			(DataGridViewColumn)DgDescription
		});
		((DataGridView)DGMazaya1).DataSource = fishMDetBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGMazaya1).DefaultCellStyle = val2;
		((DataGridView)DGMazaya1).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGMazaya1).Location = new Point(0, 62);
		((Control)DGMazaya1).Name = "DGMazaya1";
		((DataGridView)DGMazaya1).ReadOnly = true;
		((DataGridView)DGMazaya1).RowHeadersVisible = false;
		((DataGridView)DGMazaya1).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGMazaya1).Size = new Size(346, 438);
		((Control)DGMazaya1).TabIndex = 0;
		((Control)DGMazaya1).TabStop = false;
		((DataGridView)DGMazaya1).CellContentClick += new DataGridViewCellEventHandler(DGMazaya1_CellContentClick);
		((DataGridView)DGMazaya1).DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DGMazaya1_DataBindingComplete);
		((DataGridViewColumn)DGMName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGMName).DataPropertyName = "Name";
		((DataGridViewColumn)DGMName).HeaderText = "[نام]";
		((DataGridViewColumn)DGMName).Name = "DGMName";
		((DataGridViewBand)DGMName).ReadOnly = true;
		((DataGridViewColumn)DGMCredit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGMCredit).DataPropertyName = "TmpValue";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)DGMCredit).DefaultCellStyle = val3;
		((DataGridViewColumn)DGMCredit).HeaderText = "[مبلغ]";
		((DataGridViewColumn)DGMCredit).MinimumWidth = 100;
		((DataGridViewColumn)DGMCredit).Name = "DGMCredit";
		((DataGridViewBand)DGMCredit).ReadOnly = true;
		((DataGridViewColumn)DgDescription).DataPropertyName = "Description";
		((DataGridViewColumn)DgDescription).HeaderText = "[شرح]";
		((DataGridViewColumn)DgDescription).Name = "DgDescription";
		((DataGridViewBand)DgDescription).ReadOnly = true;
		((DataGridViewBand)DgDescription).Visible = false;
		fishMDetBindingSource.DataSource = typeof(FishDet);
		((Control)BTNADDMazaya).AccessibleRole = (AccessibleRole)43;
		((Control)BTNADDMazaya).Anchor = (AnchorStyles)13;
		BTNADDMazaya.ColorTable = (eButtonColor)6;
		((Control)BTNADDMazaya).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNADDMazaya.Image = (Image)(object)Resources.edit_add1;
		BTNADDMazaya.ImagePosition = (eImagePosition)1;
		BTNADDMazaya.ImageTextSpacing = 10;
		((Control)BTNADDMazaya).Location = new Point(52, 23);
		((Control)BTNADDMazaya).Name = "BTNADDMazaya";
		((Control)BTNADDMazaya).RightToLeft = (RightToLeft)1;
		((Control)BTNADDMazaya).Size = new Size(246, 33);
		((PopupItemControl)BTNADDMazaya).Style = (eDotNetBarStyle)9;
		((Control)BTNADDMazaya).TabIndex = 2;
		((Control)BTNADDMazaya).TabStop = false;
		((Control)BTNADDMazaya).Click += BtnAddMazaya_Click;
		((Control)BTNDeleteMazaya).AccessibleRole = (AccessibleRole)43;
		((Control)BTNDeleteMazaya).Anchor = (AnchorStyles)9;
		BTNDeleteMazaya.ColorTable = (eButtonColor)6;
		((Control)BTNDeleteMazaya).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNDeleteMazaya.Image = (Image)(object)Resources.edit_delete;
		BTNDeleteMazaya.ImagePosition = (eImagePosition)1;
		((Control)BTNDeleteMazaya).Location = new Point(299, 23);
		((Control)BTNDeleteMazaya).Name = "BTNDeleteMazaya";
		((Control)BTNDeleteMazaya).RightToLeft = (RightToLeft)1;
		((Control)BTNDeleteMazaya).Size = new Size(47, 33);
		((PopupItemControl)BTNDeleteMazaya).Style = (eDotNetBarStyle)9;
		((Control)BTNDeleteMazaya).TabIndex = 1;
		((Control)BTNDeleteMazaya).TabStop = false;
		((Control)BTNDeleteMazaya).Click += BtnDeleteMazaya_Click;
		((Control)BtnEditMazaya).AccessibleRole = (AccessibleRole)43;
		BtnEditMazaya.ColorTable = (eButtonColor)6;
		((Control)BtnEditMazaya).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnEditMazaya.Image = (Image)(object)Resources.edit2;
		((Control)BtnEditMazaya).Location = new Point(0, 23);
		((Control)BtnEditMazaya).Name = "BtnEditMazaya";
		((Control)BtnEditMazaya).Size = new Size(51, 33);
		((PopupItemControl)BtnEditMazaya).Style = (eDotNetBarStyle)9;
		((Control)BtnEditMazaya).TabIndex = 3;
		((Control)BtnEditMazaya).Click += BtnEditMazaya_Click;
		((Control)groupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(0, 3);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(346, 23);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 0;
		((Control)groupPanel1).Text = "[مزایا]";
		((Control)BtnEditKosorat).AccessibleRole = (AccessibleRole)43;
		BtnEditKosorat.ColorTable = (eButtonColor)6;
		BtnEditKosorat.Image = (Image)(object)Resources.edit2;
		((Control)BtnEditKosorat).Location = new Point(0, 23);
		((Control)BtnEditKosorat).Name = "BtnEditKosorat";
		((Control)BtnEditKosorat).Size = new Size(50, 33);
		((PopupItemControl)BtnEditKosorat).Style = (eDotNetBarStyle)9;
		((Control)BtnEditKosorat).TabIndex = 3;
		((Control)BtnEditKosorat).Click += BtnEditKosorat_Click;
		((Control)BtnAddKosorat).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAddKosorat).Anchor = (AnchorStyles)13;
		BtnAddKosorat.ColorTable = (eButtonColor)6;
		((Control)BtnAddKosorat).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAddKosorat.Image = (Image)(object)Resources.edit_add1;
		BtnAddKosorat.ImagePosition = (eImagePosition)1;
		BtnAddKosorat.ImageTextSpacing = 10;
		((Control)BtnAddKosorat).Location = new Point(51, 23);
		((Control)BtnAddKosorat).Name = "BtnAddKosorat";
		((Control)BtnAddKosorat).RightToLeft = (RightToLeft)1;
		((Control)BtnAddKosorat).Size = new Size(264, 33);
		((PopupItemControl)BtnAddKosorat).Style = (eDotNetBarStyle)9;
		((Control)BtnAddKosorat).TabIndex = 4;
		((Control)BtnAddKosorat).TabStop = false;
		((Control)BtnAddKosorat).Click += BtnAddKosorat_Click;
		((Control)BtnDelKosor).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelKosor).Anchor = (AnchorStyles)9;
		BtnDelKosor.ColorTable = (eButtonColor)6;
		((Control)BtnDelKosor).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnDelKosor.Image = (Image)(object)Resources.edit_delete;
		BtnDelKosor.ImagePosition = (eImagePosition)1;
		((Control)BtnDelKosor).Location = new Point(318, 23);
		((Control)BtnDelKosor).Name = "BtnDelKosor";
		((Control)BtnDelKosor).RightToLeft = (RightToLeft)1;
		((Control)BtnDelKosor).Size = new Size(47, 33);
		((PopupItemControl)BtnDelKosor).Style = (eDotNetBarStyle)9;
		((Control)BtnDelKosor).TabIndex = 1;
		((Control)BtnDelKosor).TabStop = false;
		((Control)BtnDelKosor).Click += BtnDelKosor_Click;
		((DataGridView)DGKosorat1).AllowUserToAddRows = false;
		((DataGridView)DGKosorat1).AllowUserToDeleteRows = false;
		((DataGridView)DGKosorat1).AllowUserToResizeRows = false;
		val4.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGKosorat1).AlternatingRowsDefaultCellStyle = val4;
		((Control)DGKosorat1).Anchor = (AnchorStyles)15;
		((DataGridView)DGKosorat1).AutoGenerateColumns = false;
		((DataGridView)DGKosorat1).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGKosorat1).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGKName,
			(DataGridViewColumn)DGKDebit,
			(DataGridViewColumn)DgDescriptionK
		});
		((DataGridView)DGKosorat1).DataSource = fishKDetBindingSource;
		val5.Alignment = (DataGridViewContentAlignment)16;
		val5.BackColor = SystemColors.Window;
		val5.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val5.ForeColor = SystemColors.ControlText;
		val5.SelectionBackColor = SystemColors.Highlight;
		val5.SelectionForeColor = SystemColors.ControlText;
		val5.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGKosorat1).DefaultCellStyle = val5;
		((DataGridView)DGKosorat1).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGKosorat1).Location = new Point(0, 62);
		((Control)DGKosorat1).Name = "DGKosorat1";
		((DataGridView)DGKosorat1).ReadOnly = true;
		((DataGridView)DGKosorat1).RowHeadersVisible = false;
		((DataGridView)DGKosorat1).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGKosorat1).Size = new Size(365, 438);
		((Control)DGKosorat1).TabIndex = 0;
		((Control)DGKosorat1).TabStop = false;
		((DataGridView)DGKosorat1).CellContentClick += new DataGridViewCellEventHandler(DGKosorat1_CellContentClick);
		((DataGridView)DGKosorat1).DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DGKosorat1_DataBindingComplete);
		((DataGridViewColumn)DGKName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGKName).DataPropertyName = "Name";
		((DataGridViewColumn)DGKName).HeaderText = "[نام]";
		((DataGridViewColumn)DGKName).MinimumWidth = 50;
		((DataGridViewColumn)DGKName).Name = "DGKName";
		((DataGridViewBand)DGKName).ReadOnly = true;
		((DataGridViewColumn)DGKDebit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGKDebit).DataPropertyName = "TmpValue";
		val6.Format = "N0";
		val6.NullValue = null;
		((DataGridViewBand)DGKDebit).DefaultCellStyle = val6;
		((DataGridViewColumn)DGKDebit).HeaderText = "[مبلغ]";
		((DataGridViewColumn)DGKDebit).MinimumWidth = 100;
		((DataGridViewColumn)DGKDebit).Name = "DGKDebit";
		((DataGridViewBand)DGKDebit).ReadOnly = true;
		((DataGridViewColumn)DgDescriptionK).DataPropertyName = "Description";
		((DataGridViewColumn)DgDescriptionK).HeaderText = "[شرح]";
		((DataGridViewColumn)DgDescriptionK).Name = "DgDescriptionK";
		((DataGridViewBand)DgDescriptionK).ReadOnly = true;
		((DataGridViewBand)DgDescriptionK).Visible = false;
		fishKDetBindingSource.DataSource = typeof(FishDet);
		((Control)groupPanel2).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(0, 3);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(365, 23);
		((PanelControl)groupPanel2).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel2).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel2).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel2).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel2).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel2).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel2).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel2).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel2).Style.CornerDiameter = 4;
		((PanelControl)groupPanel2).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel2).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel2).TabIndex = 0;
		((Control)groupPanel2).Text = "[کسورات]";
		((Control)labelX38).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX38).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX38).Location = new Point(661, 607);
		((Control)labelX38).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX38).Name = "labelX38";
		((Control)labelX38).Size = new Size(94, 21);
		((Control)labelX38).TabIndex = 0;
		((Control)labelX38).Text = "[شرح] (F6) :";
		((Control)labelX37).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX37).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX37).Location = new Point(141, 682);
		((Control)labelX37).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX37).Name = "labelX37";
		((Control)labelX37).Size = new Size(78, 21);
		((Control)labelX37).TabIndex = 0;
		((Control)labelX37).Text = "[مانده خالص] :";
		labelX37.TextAlignment = (StringAlignment)2;
		TXTKolMande.AllowEmpty = true;
		((Control)TXTKolMande).Anchor = (AnchorStyles)6;
		((Control)TXTKolMande).BackColor = Color.White;
		TXTKolMande.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolMande.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolMande.BackcolorTooltip = Color.Red;
		TXTKolMande.CurrencyFormat = "";
		((Control)TXTKolMande).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TXTKolMande).ForeColor = Color.Black;
		TXTKolMande.ForeColorTooltip = Color.Black;
		TXTKolMande.GetColoresFromSettingDLL = true;
		TXTKolMande.HandleKeydown = true;
		((Control)TXTKolMande).Location = new Point(12, 682);
		((TextBoxBase)TXTKolMande).MaxLength = 19;
		((Control)TXTKolMande).Name = "TXTKolMande";
		((TextBoxBase)TXTKolMande).ReadOnly = true;
		TXTKolMande.SendShiftTabAfterDown = true;
		TXTKolMande.SendShiftTabAfterEnter = true;
		TXTKolMande.SendTabAfterEnter = true;
		TXTKolMande.SendTabAfterUP = true;
		TXTKolMande.ShowHorofi = (ENShowHorofi)3;
		TXTKolMande.ShowToolTipText = true;
		((Control)TXTKolMande).Size = new Size(123, 22);
		((Control)TXTKolMande).TabIndex = 59;
		((Control)TXTKolMande).TabStop = false;
		((Control)TXTKolMande).Text = "0";
		TXTKolMande.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolMande.ToolTipPostFix = "";
		TXTKolMande.TootTipDelay = 1000;
		TXTKolMande.Value = new decimal(new int[4]);
		((Control)LblMaliat).Anchor = (AnchorStyles)6;
		LblMaliat.LinkBehavior = (LinkBehavior)3;
		LblMaliat.LinkColor = Color.Navy;
		((Control)LblMaliat).Location = new Point(326, 682);
		((Control)LblMaliat).Margin = new Padding(3, 2, 3, 2);
		((Control)LblMaliat).Name = "LblMaliat";
		((Control)LblMaliat).Size = new Size(126, 21);
		((Control)LblMaliat).TabIndex = 0;
		LblMaliat.TabStop = true;
		((Control)LblMaliat).Text = "مشمول مالیات؟";
		((Label)LblMaliat).TextAlign = (ContentAlignment)64;
		((Control)LblMaliat).Click += labelX33_Click;
		TxtIncludedTax.AllowEmpty = true;
		((Control)TxtIncludedTax).Anchor = (AnchorStyles)6;
		((Control)TxtIncludedTax).BackColor = Color.White;
		TxtIncludedTax.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtIncludedTax.BackColorTextBoxWhenLostFocus = Color.White;
		TxtIncludedTax.BackcolorTooltip = Color.Red;
		TxtIncludedTax.CurrencyFormat = "";
		((Control)TxtIncludedTax).ForeColor = Color.Navy;
		TxtIncludedTax.ForeColorTooltip = Color.Black;
		TxtIncludedTax.GetColoresFromSettingDLL = true;
		TxtIncludedTax.HandleKeydown = true;
		((Control)TxtIncludedTax).Location = new Point(225, 683);
		((TextBoxBase)TxtIncludedTax).MaxLength = 19;
		((Control)TxtIncludedTax).Name = "TxtIncludedTax";
		((TextBoxBase)TxtIncludedTax).ReadOnly = true;
		TxtIncludedTax.SendShiftTabAfterDown = true;
		TxtIncludedTax.SendShiftTabAfterEnter = true;
		TxtIncludedTax.SendTabAfterEnter = true;
		TxtIncludedTax.SendTabAfterUP = true;
		TxtIncludedTax.ShowHorofi = (ENShowHorofi)3;
		TxtIncludedTax.ShowToolTipText = true;
		((Control)TxtIncludedTax).Size = new Size(94, 21);
		((Control)TxtIncludedTax).TabIndex = 54;
		((Control)TxtIncludedTax).TabStop = false;
		((Control)TxtIncludedTax).Text = "0";
		TxtIncludedTax.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtIncludedTax.ToolTipPostFix = "";
		TxtIncludedTax.TootTipDelay = 1000;
		TxtIncludedTax.Value = new decimal(new int[4]);
		((Control)labelX36).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX36).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX36).Location = new Point(141, 653);
		((Control)labelX36).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX36).Name = "labelX36";
		((Control)labelX36).Size = new Size(78, 21);
		((Control)labelX36).TabIndex = 0;
		((Control)labelX36).Text = "[جمع کسورات] :";
		labelX36.TextAlignment = (StringAlignment)2;
		((Control)labelX44).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX44).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX44).Location = new Point(325, 604);
		((Control)labelX44).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX44).Name = "labelX44";
		((Control)labelX44).Size = new Size(111, 21);
		((Control)labelX44).TabIndex = 0;
		((Control)labelX44).Text = "[کارکرد بیمه(روز)]:";
		labelX44.TextAlignment = (StringAlignment)2;
		((Control)LblBime).Anchor = (AnchorStyles)6;
		LblBime.LinkBehavior = (LinkBehavior)3;
		LblBime.LinkColor = Color.Navy;
		((Control)LblBime).Location = new Point(325, 629);
		((Control)LblBime).Margin = new Padding(3, 2, 3, 2);
		((Control)LblBime).Name = "LblBime";
		((Control)LblBime).Size = new Size(111, 21);
		((Control)LblBime).TabIndex = 0;
		LblBime.TabStop = true;
		((Control)LblBime).Text = "مشمول بیمه؟";
		((Label)LblBime).TextAlign = (ContentAlignment)64;
		((Control)LblBime).Click += labelX20_Click;
		TXTKolKosorat.AllowEmpty = true;
		((Control)TXTKolKosorat).Anchor = (AnchorStyles)6;
		((Control)TXTKolKosorat).BackColor = Color.White;
		TXTKolKosorat.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolKosorat.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolKosorat.BackcolorTooltip = Color.Red;
		TXTKolKosorat.CurrencyFormat = "";
		((Control)TXTKolKosorat).ForeColor = Color.Red;
		TXTKolKosorat.ForeColorTooltip = Color.Black;
		TXTKolKosorat.GetColoresFromSettingDLL = false;
		TXTKolKosorat.HandleKeydown = true;
		((Control)TXTKolKosorat).Location = new Point(12, 653);
		((TextBoxBase)TXTKolKosorat).MaxLength = 19;
		((Control)TXTKolKosorat).Name = "TXTKolKosorat";
		((TextBoxBase)TXTKolKosorat).ReadOnly = true;
		TXTKolKosorat.SendShiftTabAfterDown = true;
		TXTKolKosorat.SendShiftTabAfterEnter = true;
		TXTKolKosorat.SendTabAfterEnter = true;
		TXTKolKosorat.SendTabAfterUP = true;
		TXTKolKosorat.ShowHorofi = (ENShowHorofi)3;
		TXTKolKosorat.ShowToolTipText = true;
		((Control)TXTKolKosorat).Size = new Size(123, 21);
		((Control)TXTKolKosorat).TabIndex = 58;
		((Control)TXTKolKosorat).TabStop = false;
		((Control)TXTKolKosorat).Text = "0";
		TXTKolKosorat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolKosorat.ToolTipPostFix = "";
		TXTKolKosorat.TootTipDelay = 1000;
		TXTKolKosorat.Value = new decimal(new int[4]);
		TxtIncludedInsurance.AllowEmpty = true;
		((Control)TxtIncludedInsurance).Anchor = (AnchorStyles)6;
		((Control)TxtIncludedInsurance).BackColor = Color.White;
		TxtIncludedInsurance.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtIncludedInsurance.BackColorTextBoxWhenLostFocus = Color.White;
		TxtIncludedInsurance.BackcolorTooltip = Color.Red;
		TxtIncludedInsurance.CurrencyFormat = "";
		((Control)TxtIncludedInsurance).ForeColor = Color.Navy;
		TxtIncludedInsurance.ForeColorTooltip = Color.Black;
		TxtIncludedInsurance.GetColoresFromSettingDLL = true;
		TxtIncludedInsurance.HandleKeydown = true;
		((Control)TxtIncludedInsurance).Location = new Point(225, 628);
		((TextBoxBase)TxtIncludedInsurance).MaxLength = 19;
		((Control)TxtIncludedInsurance).Name = "TxtIncludedInsurance";
		((TextBoxBase)TxtIncludedInsurance).ReadOnly = true;
		TxtIncludedInsurance.SendShiftTabAfterDown = true;
		TxtIncludedInsurance.SendShiftTabAfterEnter = true;
		TxtIncludedInsurance.SendTabAfterEnter = true;
		TxtIncludedInsurance.SendTabAfterUP = true;
		TxtIncludedInsurance.ShowHorofi = (ENShowHorofi)3;
		TxtIncludedInsurance.ShowToolTipText = true;
		((Control)TxtIncludedInsurance).Size = new Size(94, 21);
		((Control)TxtIncludedInsurance).TabIndex = 53;
		((Control)TxtIncludedInsurance).TabStop = false;
		((Control)TxtIncludedInsurance).Text = "0";
		TxtIncludedInsurance.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtIncludedInsurance.ToolTipPostFix = "";
		TxtIncludedInsurance.TootTipDelay = 1000;
		TxtIncludedInsurance.Value = new decimal(new int[4]);
		((Control)labelX35).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Location = new Point(141, 624);
		((Control)labelX35).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).Size = new Size(78, 21);
		((Control)labelX35).TabIndex = 0;
		((Control)labelX35).Text = "[جمع مزایا] :";
		labelX35.TextAlignment = (StringAlignment)2;
		TXTKolMazaya.AllowEmpty = true;
		((Control)TXTKolMazaya).Anchor = (AnchorStyles)6;
		((Control)TXTKolMazaya).BackColor = Color.White;
		TXTKolMazaya.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolMazaya.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolMazaya.BackcolorTooltip = Color.Red;
		TXTKolMazaya.CurrencyFormat = "";
		((Control)TXTKolMazaya).ForeColor = Color.DarkGreen;
		TXTKolMazaya.ForeColorTooltip = Color.Black;
		TXTKolMazaya.GetColoresFromSettingDLL = true;
		TXTKolMazaya.HandleKeydown = true;
		((Control)TXTKolMazaya).Location = new Point(12, 624);
		((TextBoxBase)TXTKolMazaya).MaxLength = 19;
		((Control)TXTKolMazaya).Name = "TXTKolMazaya";
		((TextBoxBase)TXTKolMazaya).ReadOnly = true;
		TXTKolMazaya.SendShiftTabAfterDown = true;
		TXTKolMazaya.SendShiftTabAfterEnter = true;
		TXTKolMazaya.SendTabAfterEnter = true;
		TXTKolMazaya.SendTabAfterUP = true;
		TXTKolMazaya.ShowHorofi = (ENShowHorofi)3;
		TXTKolMazaya.ShowToolTipText = true;
		((Control)TXTKolMazaya).Size = new Size(123, 21);
		((Control)TXTKolMazaya).TabIndex = 57;
		((Control)TXTKolMazaya).TabStop = false;
		((Control)TXTKolMazaya).Text = "0";
		TXTKolMazaya.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolMazaya.ToolTipPostFix = "";
		TXTKolMazaya.TootTipDelay = 1000;
		TXTKolMazaya.Value = new decimal(new int[4]);
		((Control)labelX34).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX34).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX34).Location = new Point(141, 595);
		((Control)labelX34).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX34).Name = "labelX34";
		((Control)labelX34).Size = new Size(78, 21);
		((Control)labelX34).TabIndex = 0;
		((Control)labelX34).Text = "[جمع کارکرد] :";
		labelX34.TextAlignment = (StringAlignment)2;
		((Control)BtnAbort).Anchor = (AnchorStyles)10;
		BtnAbort.DialogResult = (DialogResult)2;
		((Control)BtnAbort).Location = new Point(1384, 702);
		((Control)BtnAbort).Name = "BtnAbort";
		((Control)BtnAbort).Size = new Size(10, 10);
		((Control)BtnAbort).TabIndex = 65;
		((Control)BtnAbort).Text = "[لغو]";
		((ButtonBase)BtnAbort).UseVisualStyleBackColor = true;
		((Control)BtnAbort).Click += BtnAbort_Click;
		TXTKolKarkerd.AllowEmpty = true;
		((Control)TXTKolKarkerd).Anchor = (AnchorStyles)6;
		((Control)TXTKolKarkerd).BackColor = Color.White;
		TXTKolKarkerd.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolKarkerd.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolKarkerd.BackcolorTooltip = Color.Red;
		TXTKolKarkerd.CurrencyFormat = "";
		((Control)TXTKolKarkerd).ForeColor = Color.Black;
		TXTKolKarkerd.ForeColorTooltip = Color.Black;
		TXTKolKarkerd.GetColoresFromSettingDLL = true;
		TXTKolKarkerd.HandleKeydown = true;
		((Control)TXTKolKarkerd).Location = new Point(12, 595);
		((TextBoxBase)TXTKolKarkerd).MaxLength = 19;
		((Control)TXTKolKarkerd).Name = "TXTKolKarkerd";
		((TextBoxBase)TXTKolKarkerd).ReadOnly = true;
		TXTKolKarkerd.SendShiftTabAfterDown = true;
		TXTKolKarkerd.SendShiftTabAfterEnter = true;
		TXTKolKarkerd.SendTabAfterEnter = true;
		TXTKolKarkerd.SendTabAfterUP = true;
		TXTKolKarkerd.ShowHorofi = (ENShowHorofi)3;
		TXTKolKarkerd.ShowToolTipText = true;
		((Control)TXTKolKarkerd).Size = new Size(123, 21);
		((Control)TXTKolKarkerd).TabIndex = 56;
		((Control)TXTKolKarkerd).TabStop = false;
		((Control)TXTKolKarkerd).Text = "0";
		TXTKolKarkerd.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolKarkerd.ToolTipPostFix = "";
		TXTKolKarkerd.TootTipDelay = 1000;
		TXTKolKarkerd.Value = new decimal(new int[4]);
		((Control)GPHoghogh).Anchor = (AnchorStyles)13;
		((PanelControl)GPHoghogh).CanvasColor = SystemColors.Control;
		((PanelControl)GPHoghogh).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GPHoghogh).Controls.Add((Control)(object)ChkLockFish);
		((Control)GPHoghogh).Controls.Add((Control)(object)TXTDate);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX3);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblName);
		((Control)GPHoghogh).Controls.Add((Control)(object)BtnAccept);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblCodeMeli);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblTadate);
		((Control)GPHoghogh).Controls.Add((Control)(object)TXTFishNO);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX15);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX43);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX42);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX41);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX40);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX4);
		((Control)GPHoghogh).Controls.Add((Control)(object)labelX2);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblfromDate);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblFName);
		((Control)GPHoghogh).Controls.Add((Control)(object)LblCodeTafsil);
		GPHoghogh.DisabledBackColor = Color.Empty;
		((Control)GPHoghogh).Location = new Point(14, 3);
		((Control)GPHoghogh).Name = "GPHoghogh";
		((Control)GPHoghogh).Size = new Size(1368, 84);
		((PanelControl)GPHoghogh).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GPHoghogh).Style.BackColorGradientAngle = 90;
		((PanelControl)GPHoghogh).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GPHoghogh).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GPHoghogh).Style.BorderBottomWidth = 1;
		((PanelControl)GPHoghogh).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GPHoghogh).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GPHoghogh).Style.BorderLeftWidth = 1;
		((PanelControl)GPHoghogh).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GPHoghogh).Style.BorderRightWidth = 1;
		((PanelControl)GPHoghogh).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GPHoghogh).Style.BorderTopWidth = 1;
		((PanelControl)GPHoghogh).Style.CornerDiameter = 4;
		((PanelControl)GPHoghogh).Style.CornerType = (eCornerType)2;
		((PanelControl)GPHoghogh).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)GPHoghogh).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GPHoghogh).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GPHoghogh).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GPHoghogh).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GPHoghogh).TabIndex = 0;
		((Control)GPHoghogh).Text = "[مشخصات]";
		((Control)ChkLockFish).AutoSize = true;
		((Control)ChkLockFish).Location = new Point(132, 36);
		((Control)ChkLockFish).Name = "ChkLockFish";
		((Control)ChkLockFish).Size = new Size(144, 17);
		((Control)ChkLockFish).TabIndex = 64;
		((Control)ChkLockFish).Text = "[ثبت دائم شود]";
		((ButtonBase)ChkLockFish).UseVisualStyleBackColor = true;
		((Control)ChkLockFish).Visible = false;
		ChkLockFish.CheckedChanged += ChkLockFish_CheckedChanged;
		TXTDate.AllowNull = false;
		((Control)TXTDate).Anchor = (AnchorStyles)9;
		TXTDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTDate).Border.Class = "TextBoxBorder";
		((TextBoxX)TXTDate).Border.CornerType = (eCornerType)1;
		TXTDate.EnterToTab = true;
		TXTDate.EscToClose = true;
		TXTDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTDate.FocusBackColor = Color.Yellow;
		TXTDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDate.FocusForeColor = Color.Blue;
		TXTDate.FocusTextSelect = true;
		((Control)TXTDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTDate).Location = new Point(1002, 5);
		TXTDate.MessageEmptyShowDialog = false;
		TXTDate.MessegeEmpty = "";
		TXTDate.MessegeEmptyInFormRight = true;
		TXTDate.MessegeEmptyShowInForm = false;
		TXTDate.MiladiDate = new DateTime(2024, 2, 28, 18, 56, 1, 616);
		((Control)TXTDate).Name = "TXTDate";
		((Control)TXTDate).RightToLeft = (RightToLeft)0;
		TXTDate.ShowNameOfDayOnToolTip = false;
		TXTDate.ShowToolTipText = false;
		((Control)TXTDate).Size = new Size(100, 26);
		((Control)TXTDate).TabIndex = 62;
		((Control)TXTDate).Text = "28/02/2024";
		((TextBox)TXTDate).TextAlign = (HorizontalAlignment)2;
		TXTDate.TootTipDelay = 500;
		((Control)TXTDate).TextChanged += TXTDate_TextChanged;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(844, 33);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(43, 21);
		((Control)labelX3).TabIndex = 12;
		((Control)labelX3).Text = "[نام پدر] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)LblName).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblName).ForeColor = Color.DarkRed;
		((Control)LblName).Location = new Point(899, 33);
		((Control)LblName).Margin = new Padding(3, 2, 3, 2);
		((Control)LblName).Name = "LblName";
		((Control)LblName).Size = new Size(205, 21);
		((Control)LblName).TabIndex = 0;
		((Control)LblName).Text = "--------";
		((Control)LblName).Click += LblName_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(3, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(123, 51);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 63;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)LblCodeMeli).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblCodeMeli).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCodeMeli).ForeColor = Color.DarkRed;
		((Control)LblCodeMeli).Location = new Point(577, 33);
		((Control)LblCodeMeli).Margin = new Padding(3, 2, 3, 2);
		((Control)LblCodeMeli).Name = "LblCodeMeli";
		((Control)LblCodeMeli).Size = new Size(97, 21);
		((Control)LblCodeMeli).TabIndex = 0;
		((Control)LblCodeMeli).Text = "--------";
		((Control)LblTadate).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTadate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTadate).ForeColor = Color.DarkRed;
		((Control)LblTadate).Location = new Point(577, 3);
		((Control)LblTadate).Margin = new Padding(3, 2, 3, 2);
		((Control)LblTadate).Name = "LblTadate";
		((Control)LblTadate).Size = new Size(97, 26);
		((Control)LblTadate).TabIndex = 0;
		((Control)LblTadate).Text = "---------";
		((Control)TXTFishNO).Anchor = (AnchorStyles)9;
		((Control)TXTFishNO).ForeColor = Color.Red;
		((Control)TXTFishNO).Location = new Point(1206, 3);
		TXTFishNO.Maximum = new decimal(new int[4] { 100000, 0, 0, 0 });
		TXTFishNO.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TXTFishNO).Name = "TXTFishNO";
		((Control)TXTFishNO).Size = new Size(62, 21);
		((Control)TXTFishNO).TabIndex = 61;
		((UpDownBase)TXTFishNO).TextAlign = (HorizontalAlignment)2;
		TXTFishNO.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Location = new Point(1274, 33);
		((Control)labelX15).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(84, 21);
		((Control)labelX15).TabIndex = 8;
		((Control)labelX15).Text = "[کد تفصیل] * :";
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)labelX43).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX43).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX43).Location = new Point(678, 33);
		((Control)labelX43).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX43).Name = "labelX43";
		((Control)labelX43).Size = new Size(48, 21);
		((Control)labelX43).TabIndex = 14;
		((Control)labelX43).Text = "[کد ملی] :";
		labelX43.TextAlignment = (StringAlignment)2;
		((Control)labelX42).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX42).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX42).Location = new Point(678, 3);
		((Control)labelX42).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX42).Name = "labelX42";
		((Control)labelX42).Size = new Size(48, 26);
		((Control)labelX42).TabIndex = 6;
		((Control)labelX42).Text = "تا تاریخ :";
		labelX42.TextAlignment = (StringAlignment)2;
		((Control)labelX41).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX41).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX41).Location = new Point(844, 3);
		((Control)labelX41).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX41).Name = "labelX41";
		((Control)labelX41).Size = new Size(48, 26);
		((Control)labelX41).TabIndex = 4;
		((Control)labelX41).Text = "از تاریخ :";
		labelX41.TextAlignment = (StringAlignment)2;
		((Control)labelX40).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX40).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX40).Location = new Point(1107, 3);
		((Control)labelX40).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX40).Name = "labelX40";
		((Control)labelX40).Size = new Size(93, 26);
		((Control)labelX40).TabIndex = 2;
		((Control)labelX40).Text = "[تاریخ ثبت فیش] :";
		labelX40.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(1274, 3);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(84, 26);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "[شماره فیش]* :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(1107, 33);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(64, 21);
		((Control)labelX2).TabIndex = 10;
		((Control)labelX2).Text = "[نام شخص] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)LblfromDate).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblfromDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblfromDate).ForeColor = Color.DarkRed;
		((Control)LblfromDate).Location = new Point(740, 3);
		((Control)LblfromDate).Margin = new Padding(3, 2, 3, 2);
		((Control)LblfromDate).Name = "LblfromDate";
		((Control)LblfromDate).Size = new Size(97, 26);
		((Control)LblfromDate).TabIndex = 0;
		((Control)LblfromDate).Text = "--------";
		((Control)LblFName).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblFName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblFName).ForeColor = Color.DarkRed;
		((Control)LblFName).Location = new Point(740, 33);
		((Control)LblFName).Margin = new Padding(3, 2, 3, 2);
		((Control)LblFName).Name = "LblFName";
		((Control)LblFName).Size = new Size(97, 21);
		((Control)LblFName).TabIndex = 0;
		((Control)LblFName).Text = "--------";
		((Control)LblCodeTafsil).Anchor = (AnchorStyles)9;
		((Control)LblCodeTafsil).ForeColor = Color.DarkRed;
		LblCodeTafsil.LinkBehavior = (LinkBehavior)3;
		LblCodeTafsil.LinkColor = Color.DarkRed;
		((Control)LblCodeTafsil).Location = new Point(1182, 33);
		((Control)LblCodeTafsil).Name = "LblCodeTafsil";
		((Control)LblCodeTafsil).Size = new Size(86, 21);
		((Control)LblCodeTafsil).TabIndex = 0;
		LblCodeTafsil.TabStop = true;
		((Control)LblCodeTafsil).Text = "-------";
		((Label)LblCodeTafsil).TextAlign = (ContentAlignment)16;
		LblCodeTafsil.LinkClicked += new LinkLabelLinkClickedEventHandler(LblCodeTafsil_LinkClicked);
		((Control)btnModifiedTaradod).AccessibleRole = (AccessibleRole)43;
		((Control)btnModifiedTaradod).Anchor = (AnchorStyles)6;
		btnModifiedTaradod.ColorTable = (eButtonColor)6;
		((Control)btnModifiedTaradod).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnModifiedTaradod.ImagePosition = (eImagePosition)1;
		((Control)btnModifiedTaradod).Location = new Point(443, 596);
		((Control)btnModifiedTaradod).Name = "btnModifiedTaradod";
		((Control)btnModifiedTaradod).RightToLeft = (RightToLeft)1;
		((Control)btnModifiedTaradod).Size = new Size(72, 30);
		((PopupItemControl)btnModifiedTaradod).Style = (eDotNetBarStyle)9;
		((Control)btnModifiedTaradod).TabIndex = 60;
		((Control)btnModifiedTaradod).Text = "[اصلاح تردد]";
		((Control)btnModifiedTaradod).Visible = false;
		((Control)btnModifiedTaradod).Click += BtnModifiedTaradod_Click;
		((Control)btnNotifyMe).AccessibleRole = (AccessibleRole)43;
		((Control)btnNotifyMe).Anchor = (AnchorStyles)6;
		((Control)btnNotifyMe).BackColor = Color.DarkMagenta;
		btnNotifyMe.ColorTable = (eButtonColor)8;
		((Control)btnNotifyMe).Font = new Font("Tahoma", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnNotifyMe).Location = new Point(521, 599);
		((Control)btnNotifyMe).Name = "btnNotifyMe";
		((Control)btnNotifyMe).Size = new Size(134, 27);
		((PopupItemControl)btnNotifyMe).Style = (eDotNetBarStyle)9;
		((Control)btnNotifyMe).TabIndex = 10;
		((Control)btnNotifyMe).Text = "[پیام های سیستم]";
		((Control)btnNotifyMe).Visible = false;
		((Control)btnNotifyMe).Click += BtnNotifyMe_Click;
		((Control)TXTDesc).Anchor = (AnchorStyles)14;
		TXTDesc.Border.Class = "TextBoxBorder";
		TXTDesc.Border.CornerType = (eCornerType)1;
		((Control)TXTDesc).Location = new Point(458, 629);
		((TextBoxBase)TXTDesc).Multiline = true;
		((Control)TXTDesc).Name = "TXTDesc";
		((Control)TXTDesc).Size = new Size(297, 74);
		((Control)TXTDesc).TabIndex = 0;
		((Control)TXTDesc).TextChanged += TXTDesc_TextChanged;
		timerReadNotify.Interval = 750;
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).Size = new Size(61, 4);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnAbort;
		((Form)this).ClientSize = new Size(1394, 734);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "FrmFishHoghogh";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مشخصات حقوقی پرسنل]*";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosing += new FormClosingEventHandler(FrmFishHoghogh_FormClosing);
		((Form)this).Load += FrmFishHoghogh_Load;
		((Control)this).KeyDown += new KeyEventHandler(FrmFishHoghogh_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)GPKarkerd).ResumeLayout(false);
		((Control)GPKarkerd).PerformLayout();
		((ISupportInitialize)NUHNobat3).EndInit();
		((ISupportInitialize)NUMNobat3).EndInit();
		((ISupportInitialize)NUHNobat2).EndInit();
		((ISupportInitialize)NUMNobat2).EndInit();
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)GbxSalaryType).ResumeLayout(false);
		((Control)GbxSalaryType).PerformLayout();
		((ISupportInitialize)NU_Jarime).EndInit();
		((ISupportInitialize)NumDailyVacation).EndInit();
		((ISupportInitialize)NUMVacation).EndInit();
		((ISupportInitialize)NUHVacation).EndInit();
		((ISupportInitialize)NUH_ClocklyVacation).EndInit();
		((ISupportInitialize)NUM_ClocklyVacation).EndInit();
		((ISupportInitialize)NUHKasr3).EndInit();
		((ISupportInitialize)NUHKasr2).EndInit();
		((ISupportInitialize)NUHKasr1).EndInit();
		((ISupportInitialize)NUMKasr3).EndInit();
		((ISupportInitialize)NUMKasr2).EndInit();
		((ISupportInitialize)NUMKasr1).EndInit();
		((ISupportInitialize)NUHJome).EndInit();
		((ISupportInitialize)NUHTatil).EndInit();
		((ISupportInitialize)NUHNobat1).EndInit();
		((ISupportInitialize)NU_Gheybat).EndInit();
		((ISupportInitialize)NUHEzafe3).EndInit();
		((ISupportInitialize)NUHShab).EndInit();
		((ISupportInitialize)NUHEzafe2).EndInit();
		((ISupportInitialize)NUHEzafe1).EndInit();
		((ISupportInitialize)NUHKarkerd).EndInit();
		((ISupportInitialize)NUMEzafe3).EndInit();
		((ISupportInitialize)NUMKarkerd).EndInit();
		((ISupportInitialize)NUMEzafe2).EndInit();
		((ISupportInitialize)NUMEzafe1).EndInit();
		((ISupportInitialize)NUMShab).EndInit();
		((ISupportInitialize)NUMJome).EndInit();
		((ISupportInitialize)NUMTatil).EndInit();
		((ISupportInitialize)NUMNobat1).EndInit();
		((ISupportInitialize)NU_workDayMaliat).EndInit();
		((ISupportInitialize)NU_workDayBime).EndInit();
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((ISupportInitialize)DGMazaya1).EndInit();
		((ISupportInitialize)fishMDetBindingSource).EndInit();
		((ISupportInitialize)DGKosorat1).EndInit();
		((ISupportInitialize)fishKDetBindingSource).EndInit();
		((Control)GPHoghogh).ResumeLayout(false);
		((Control)GPHoghogh).PerformLayout();
		((ISupportInitialize)TXTFishNO).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
