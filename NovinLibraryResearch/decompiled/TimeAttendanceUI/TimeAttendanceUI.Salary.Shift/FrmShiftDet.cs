using System;
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
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftDet : BaseForm
{
	public ShiftDet ClsShiftDet;

	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private GroupPanel groupPanel6;

	internal DateBoxFa TxtFinishM;

	private GroupPanel groupPanel8;

	internal DateBoxFa TXTStartDate;

	private LabelX labelX1;

	private TextBoxX TxtDesc;

	private GroupPanel groupPanelTaradod;

	private Panel PanelNormal;

	private LabelX LblTajilStartTime;

	private LabelX LblNextDay;

	private LabelX LblTajilEndTime;

	private LabelX lblTakhirKh;

	private LabelX LblTakhirStartTime;

	private LabelX LblTakhirEndTime;

	private LabelX LblEndTaTime;

	private LabelX labelX8;

	private LabelX labelX16;

	private LabelX labelX10;

	private LabelX lblTajilKh;

	private LabelX labelX11;

	private LabelX LblStartTaTime;

	private LabelX LblEndAzTime;

	private LabelX LblStartAzTime;

	private LabelX labelX20;

	private LabelX labelX13;

	private LabelX LblValEndTime;

	private LabelX LblValStartTime;

	private LabelX LblEndTime;

	private LabelX LblStartTime;

	private GroupPanel groupPanelSelTaradod;

	private LabelX labelX39;

	internal ButtonX BtnSelectShiftCoding;

	private ComboBoxEx CmbShiftCodingType;

	private GroupPanel groupPanel1;

	private LabelX labelX2;

	private GroupPanel groupPanelSurcease;

	private ToolTip toolTip1;

	private LabelX LblDescPeakOverTime;

	private LabelX LblPeakOvertime;

	private LabelX labelX3;

	private CheckBoxX ChbSat;

	private CheckBoxX ChbMon;

	private CheckBoxX ChbSun;

	private CheckBoxX ChbFri;

	private CheckBoxX ChbThr;

	private CheckBoxX ChbTue;

	private CheckBoxX ChbWed;

	private LabelX lblDescFri;

	private LabelX LblJarimeV;

	private LabelX labelX5;

	private RadioButton RbtFr3;

	private RadioButton RbtFr2;

	private RadioButton RbtFr1;

	private LabelX labelX4;

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
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmShiftDet(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_font = font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token, null), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 51);
		}
	}

	public FrmShiftDet(string contextGuid, Font font, ShiftDet pcls)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_font = font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token, pcls), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 64);
		}
	}

	private async Task LoadDataAsync(CancellationToken token, ShiftDet shiftDet)
	{
		_ = 2;
		try
		{
			await CmbShiftCodingType_LoadAsync(token);
			if (shiftDet != null)
			{
				ClsShiftDet = shiftDet;
				await FillFormAsync(ClsShiftDet.ShiftCodingId);
			}
			else
			{
				ClsShiftDet = new ShiftDet(((CacheBaseForm)this).ContextGuid);
				await FillFormAsync(0);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task FillFormAsync(short shiftCodingId)
	{
		_ = 2;
		try
		{
			await FillPanelTaradodDetailsAsync(shiftCodingId);
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed)
				{
					return;
				}
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)TxtDesc).Text = ClsShiftDet.Description;
					ChbSat.Checked = ClsShiftDet.Sat;
					ChbSun.Checked = ClsShiftDet.Sun;
					ChbMon.Checked = ClsShiftDet.Mon;
					ChbTue.Checked = ClsShiftDet.Tue;
					ChbWed.Checked = ClsShiftDet.Wed;
					ChbThr.Checked = ClsShiftDet.Tur;
					ChbFri.Checked = ClsShiftDet.Fri;
					switch (ClsShiftDet.FriType)
					{
					case ShiftDet.FriDayType.EkhtiyariAndKarkard:
						RbtFr1.Checked = true;
						break;
					case ShiftDet.FriDayType.EkhtiyariAndAdamKarkard:
						RbtFr2.Checked = true;
						break;
					case ShiftDet.FriDayType.Elzami:
						RbtFr3.Checked = true;
						break;
					default:
						RbtFr1.Checked = true;
						break;
					}
					((Control)TXTStartDate).Text = Calendar.MiladiToCalture(ClsShiftDet.DateStartM);
					((Control)TxtFinishM).Text = Calendar.MiladiToCalture(ClsShiftDet.DateFinishM);
					((Control)TXTStartDate).Focus();
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 126);
				}
			});
			await SetDescFriLabelAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 131);
		}
	}

	private async Task FillPanelTaradodDetailsAsync(short shiftCoding_ID = 0, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (shiftCoding_ID == 0)
			{
				ComboBoxEx cmbShiftCodingType = CmbShiftCodingType;
				short num = short.Parse(((cmbShiftCodingType == null) ? null : ((ListControl)cmbShiftCodingType).SelectedValue?.ToString()) ?? "0");
				if (num > 0)
				{
					await ClsShiftDet.SetShiftCodingIdAsync(num, token);
				}
			}
			else if (ClsShiftDet.ShiftCodingId != shiftCoding_ID)
			{
				await ClsShiftDet.SetShiftCodingIdAsync(shiftCoding_ID);
			}
			string FrmShiftDetStr1 = Utilities.GetTranslate(" [انتقال از روز قبل دارد و حداکثر مدت حضور در این شیفت شناور ]");
			string FrmShiftDetStr2 = Utilities.GetTranslate("[ ساعت میباشد و تردد بعد از این مدت بعنوان ورود شیفت بعد در نظر گرفته میشود].");
			string FrmShiftDetStr3 = Utilities.GetTranslate("[انتقال از روز قبل دارد و حداکثر زمان خروج ساعت  ]");
			string FrmShiftDetStr4 = Utilities.GetTranslate("[میباشد و تردد بعد از این ساعت بعنوان ورود شیفت بعد در نظر گرفته میشود.]");
			string ShiftAddi = "شیفت عادی";
			string StartClock = "ساعت شروع:";
			string ShiftShenavar = "شیفت شناور";
			string ShenavarStr13 = "مدت حضور:";
			string ShenavarStr12 = Utilities.GetTranslate("[شخص حداکثر میتواند این مقدار ساعت اضافه کاری کند]");
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_0193: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Invalid comparison between Unknown and I4
				//IL_024d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0252: Unknown result type (might be due to invalid IL or missing references)
				//IL_0253: Unknown result type (might be due to invalid IL or missing references)
				//IL_0255: Invalid comparison between Unknown and I4
				//IL_0267: Unknown result type (might be due to invalid IL or missing references)
				//IL_026a: Invalid comparison between Unknown and I4
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_0259: Invalid comparison between Unknown and I4
				//IL_026c: Unknown result type (might be due to invalid IL or missing references)
				//IL_026f: Invalid comparison between Unknown and I4
				//IL_025b: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Invalid comparison between Unknown and I4
				try
				{
					((Control)groupPanelSurcease).Visible = false;
					((Control)groupPanelTaradod).Visible = true;
					if (ClsShiftDet.ShiftCodeing != null)
					{
						((ListControl)CmbShiftCodingType).SelectedValue = ClsShiftDet.ShiftCodingId;
						((Control)LblPeakOvertime).Text = Time.IntToTime((int)(ClsShiftDet.ShiftCodeing?.PeakOvertime ?? 0));
						((Control)LblDescPeakOverTime).Visible = ClsShiftDet.ShiftCodeing.PeakOvertime > ClsShiftDet.ShiftCodeing.KhorojEnd.Hour * 60 + ClsShiftDet.ShiftCodeing.KhorojEnd.Minute;
						if (string.IsNullOrEmpty(ClsShiftDet.Description))
						{
							ShiftDet clsShiftDet = ClsShiftDet;
							string description = (((Control)TxtDesc).Text = ClsShiftDet.ShiftCodeing?.Description ?? "");
							clsShiftDet.Description = description;
						}
						if (ClsShiftDet.ShiftCodeing.NextDay)
						{
							((Control)LblNextDay).Visible = true;
							ShiftCodeing shiftCodeing = ClsShiftDet.ShiftCodeing;
							if (shiftCodeing != null && (int)shiftCodeing.TypeShift == 2)
							{
								((Control)LblNextDay).Text = FrmShiftDetStr1 + Time.IntToTime(ClsShiftDet.ShiftCodeing?.MaxTimeCanExit ?? 0) + FrmShiftDetStr2;
							}
							else
							{
								((Control)LblNextDay).Text = FrmShiftDetStr3 + Time.IntToTime(ClsShiftDet.ShiftCodeing?.MaxTimeCanExit ?? 0) + FrmShiftDetStr4;
							}
						}
						else
						{
							((Control)LblNextDay).Visible = false;
						}
						ShiftCodeing shiftCodeing2 = ClsShiftDet.ShiftCodeing;
						TypeShiftEnumType val = (TypeShiftEnumType)((shiftCodeing2 != null) ? ((int)shiftCodeing2.TypeShift) : 0);
						if ((int)val <= 2)
						{
							if ((int)val != 1)
							{
								if ((int)val == 2)
								{
									((Control)PanelNormal).Visible = false;
									((Control)groupPanelTaradod).Text = ShiftShenavar;
									((Control)LblStartTime).Text = ShenavarStr13;
									((Control)LblValStartTime).Text = ClsShiftDet.ShiftCodeing?.Khoroj.ToString("HH:mm") ?? "00:00";
									((Control)LblEndTime).Visible = false;
									((Control)LblValEndTime).Visible = false;
									((Control)LblDescPeakOverTime).Text = ShenavarStr12;
								}
								return;
							}
						}
						else
						{
							if ((int)val == 11)
							{
								((Control)groupPanelSurcease).Visible = true;
								((Control)groupPanelTaradod).Visible = false;
								return;
							}
							if ((int)val != 100)
							{
								return;
							}
						}
						((Control)PanelNormal).Visible = true;
						((Control)LblEndTime).Visible = true;
						((Control)LblValEndTime).Visible = true;
						((Control)groupPanelTaradod).Text = ShiftAddi;
						((Control)LblStartTime).Text = StartClock;
						((Control)LblValStartTime).Text = ClsShiftDet.ShiftCodeing?.Vorod.ToString("HH:mm") ?? "00:00";
						((Control)LblValEndTime).Text = ClsShiftDet.ShiftCodeing?.Khoroj.ToString("HH:mm") ?? "00:00";
						((Control)LblTajilStartTime).Text = ClsShiftDet.ShiftCodeing?.TajilV.ToString() ?? "";
						((Control)LblTakhirStartTime).Text = ClsShiftDet.ShiftCodeing?.TakhirV.ToString() ?? "";
						((Control)LblJarimeV).Text = ClsShiftDet.ShiftCodeing?.JarimeV.ToString() ?? "";
						((Control)LblStartAzTime).Text = ClsShiftDet.ShiftCodeing?.VorodStart.ToString("HH:mm") ?? "00:00";
						((Control)LblStartTaTime).Text = ClsShiftDet.ShiftCodeing?.VorodEnd.ToString("HH:mm") ?? "00:00";
						((Control)LblTajilEndTime).Text = ClsShiftDet.ShiftCodeing?.TajilKh.ToString() ?? "";
						((Control)LblTakhirEndTime).Text = ClsShiftDet.ShiftCodeing?.TakhirKh.ToString() ?? "";
						((Control)LblEndAzTime).Text = ClsShiftDet.ShiftCodeing?.KhorojStart.ToString("HH:mm") ?? "00:00";
						((Control)LblEndTaTime).Text = ClsShiftDet.ShiftCodeing?.KhorojEnd.ToString("HH:mm") ?? "00:00";
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillPanelTaradodDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 234);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillPanelTaradodDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 237);
		}
	}

	private async Task SetTooltipAsync()
	{
		try
		{
			string text = "ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.";
			toolTip1.SetToolTip((Control)(object)LblNextDay, text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltipAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 247);
		}
	}

	private async Task SetDescFriLabelAsync()
	{
		try
		{
			string FrmShiftDetStr7 = "حضور اختیاری در جمعه ها و عدم محاسبه کارکرد(در صورت حضور جمعه کاری محاسبه میشود)";
			string FrmShiftDetStr8 = "حضور الزامی در جمعه ها (در صورت عدم حضور غیبت  محاسبه میشود)";
			string FrmShiftDetStr9 = "حضور اختیاری در جمعه ها و محاسبه کارکرد(در صورت حضور جمعه کاری محسوب میشود)";
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed)
				{
					return;
				}
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (RbtFr2.Checked)
					{
						((Control)lblDescFri).Text = FrmShiftDetStr7;
						ChbFri.Checked = false;
					}
					else if (RbtFr3.Checked)
					{
						((Control)lblDescFri).Text = FrmShiftDetStr8;
						ChbFri.Checked = true;
					}
					else
					{
						((Control)lblDescFri).Text = FrmShiftDetStr9;
						ChbFri.Checked = true;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SetDescFriLabelAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 285);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetDescFriLabelAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 288);
		}
	}

	private void GetData()
	{
		try
		{
			ClsShiftDet.Sat = ChbSat.Checked;
			ClsShiftDet.Sun = ChbSun.Checked;
			ClsShiftDet.Mon = ChbMon.Checked;
			ClsShiftDet.Tue = ChbTue.Checked;
			ClsShiftDet.Wed = ChbWed.Checked;
			ClsShiftDet.Tur = ChbThr.Checked;
			ClsShiftDet.Fri = ChbFri.Checked;
			if (RbtFr2.Checked)
			{
				ClsShiftDet.FriType = ShiftDet.FriDayType.EkhtiyariAndAdamKarkard;
			}
			else if (RbtFr3.Checked)
			{
				ClsShiftDet.FriType = ShiftDet.FriDayType.Elzami;
			}
			else
			{
				ClsShiftDet.FriType = ShiftDet.FriDayType.EkhtiyariAndKarkard;
			}
			ClsShiftDet.Description = Text.FixString(((Control)TxtDesc).Text);
			ClsShiftDet.DateStartM = TXTStartDate.MiladiDate.Date;
			ClsShiftDet.DateFinishM = TxtFinishM.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 308);
		}
	}

	private async Task CmbShiftCodingType_LoadAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cmbShiftCodingType = CmbShiftCodingType;
			((ComboBox)cmbShiftCodingType).DataSource = await ShiftCodeing.GetAllAsync(((CacheBaseForm)this).ContextGuid, (TypeShiftEnumType)0, token);
			((ListControl)CmbShiftCodingType).DisplayMember = "NameShiftCodeing";
			((ListControl)CmbShiftCodingType).ValueMember = "Id";
			((ComboBox)CmbShiftCodingType).SelectedIndexChanged += CmbShiftCodingType_SelectedIndexChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbShiftCodingType_LoadAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 319);
		}
	}

	private async void FRMShiftDet_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTStartDate).Focus();
			await SetTooltipAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMShiftDet_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 328);
		}
	}

	private void FRMShiftDet_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMShiftDet_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 353);
		}
	}

	private void TxtDesc_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TxtDesc).SelectAll();
			((Control)TxtDesc).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 362);
		}
	}

	private void TxtDesc_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtDesc).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 370);
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
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				((ComboBox)CmbShiftCodingType).SelectedIndexChanged -= CmbShiftCodingType_SelectedIndexChanged;
				await CmbShiftCodingType_LoadAsync(token);
				await FillPanelTaradodDetailsAsync(frm.SelectedRow.Id);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectShiftCoding_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 386);
		}
	}

	private async void CmbShiftCodingType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)CmbShiftCodingType).SelectedValue != null)
			{
				await FillPanelTaradodDetailsAsync(Convert.ToInt16(((ListControl)CmbShiftCodingType).SelectedValue.ToString()));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbShiftCodingType_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 395);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			GetData();
			DialogResult val = ShiftDetCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, ClsShiftDet, isShowMessage: true);
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 418);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 427);
		}
	}

	private async void RbtFri_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			await SetDescFriLabelAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbtFri_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftDet.cs", 435);
		}
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
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Expected O, but got Unknown
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Expected O, but got Unknown
		//IL_0f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1169: Unknown result type (might be due to invalid IL or missing references)
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_144d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1457: Expected O, but got Unknown
		//IL_1477: Unknown result type (might be due to invalid IL or missing references)
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Expected O, but got Unknown
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1947: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_2008: Unknown result type (might be due to invalid IL or missing references)
		//IL_217f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2366: Unknown result type (might be due to invalid IL or missing references)
		//IL_2402: Unknown result type (might be due to invalid IL or missing references)
		//IL_2604: Unknown result type (might be due to invalid IL or missing references)
		//IL_2690: Unknown result type (might be due to invalid IL or missing references)
		//IL_271a: Unknown result type (might be due to invalid IL or missing references)
		//IL_27bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2858: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2987: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3012: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_315c: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_327e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3322: Unknown result type (might be due to invalid IL or missing references)
		//IL_33aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_343e: Unknown result type (might be due to invalid IL or missing references)
		//IL_34f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cb: Expected O, but got Unknown
		//IL_36f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ab: Expected O, but got Unknown
		//IL_37be: Unknown result type (might be due to invalid IL or missing references)
		//IL_3829: Unknown result type (might be due to invalid IL or missing references)
		//IL_3833: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		groupPanel6 = new GroupPanel();
		TxtFinishM = new DateBoxFa();
		groupPanel8 = new GroupPanel();
		TXTStartDate = new DateBoxFa();
		labelX1 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanelSelTaradod = new GroupPanel();
		labelX39 = new LabelX();
		BtnSelectShiftCoding = new ButtonX();
		CmbShiftCodingType = new ComboBoxEx();
		groupPanel1 = new GroupPanel();
		RbtFr3 = new RadioButton();
		RbtFr2 = new RadioButton();
		RbtFr1 = new RadioButton();
		labelX4 = new LabelX();
		lblDescFri = new LabelX();
		ChbSat = new CheckBoxX();
		ChbMon = new CheckBoxX();
		ChbSun = new CheckBoxX();
		ChbFri = new CheckBoxX();
		ChbThr = new CheckBoxX();
		ChbTue = new CheckBoxX();
		ChbWed = new CheckBoxX();
		groupPanelTaradod = new GroupPanel();
		LblDescPeakOverTime = new LabelX();
		LblNextDay = new LabelX();
		PanelNormal = new Panel();
		LblJarimeV = new LabelX();
		labelX5 = new LabelX();
		LblTajilStartTime = new LabelX();
		LblTajilEndTime = new LabelX();
		lblTakhirKh = new LabelX();
		LblTakhirStartTime = new LabelX();
		LblTakhirEndTime = new LabelX();
		LblEndTaTime = new LabelX();
		labelX8 = new LabelX();
		labelX16 = new LabelX();
		labelX10 = new LabelX();
		lblTajilKh = new LabelX();
		labelX11 = new LabelX();
		LblStartTaTime = new LabelX();
		LblEndAzTime = new LabelX();
		LblStartAzTime = new LabelX();
		labelX20 = new LabelX();
		labelX13 = new LabelX();
		LblValEndTime = new LabelX();
		LblPeakOvertime = new LabelX();
		labelX3 = new LabelX();
		LblValStartTime = new LabelX();
		LblEndTime = new LabelX();
		LblStartTime = new LabelX();
		groupPanelSurcease = new GroupPanel();
		labelX2 = new LabelX();
		toolTip1 = new ToolTip(components);
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)groupPanel6).SuspendLayout();
		((Control)groupPanel8).SuspendLayout();
		((Control)groupPanelSelTaradod).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)groupPanelTaradod).SuspendLayout();
		((Control)PanelNormal).SuspendLayout();
		((Control)groupPanelSurcease).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(720, 565);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 519);
		((Control)BtnCancel).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
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
		((Control)BtnAccept).Location = new Point(138, 519);
		((Control)BtnAccept).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel6);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel8);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelSelTaradod);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel1);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelTaradod);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelSurcease);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(14, 12);
		((Control)groupPanel7).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(692, 493);
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
		((PanelControl)groupPanel6).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel6).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel6).Controls.Add((Control)(object)TxtFinishM);
		groupPanel6.DisabledBackColor = Color.Empty;
		((Control)groupPanel6).Location = new Point(23, 9);
		((Control)groupPanel6).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanel6).Name = "groupPanel6";
		((Control)groupPanel6).Size = new Size(247, 58);
		((PanelControl)groupPanel6).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel6).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel6).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel6).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel6).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel6).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel6).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel6).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel6).Style.CornerDiameter = 4;
		((PanelControl)groupPanel6).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel6).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel6).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel6).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel6).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel6).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel6).TabIndex = 1;
		((Control)groupPanel6).Text = "تاریخ پایان";
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
		((Control)TxtFinishM).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtFinishM).Location = new Point(24, 5);
		((Control)TxtFinishM).Margin = new Padding(4, 3, 4, 3);
		TxtFinishM.MessageEmptyShowDialog = false;
		TxtFinishM.MessegeEmpty = "";
		TxtFinishM.MessegeEmptyInFormRight = true;
		TxtFinishM.MessegeEmptyShowInForm = false;
		TxtFinishM.MiladiDate = new DateTime(2024, 7, 31, 17, 24, 20, 527);
		((Control)TxtFinishM).Name = "TxtFinishM";
		((Control)TxtFinishM).RightToLeft = (RightToLeft)0;
		TxtFinishM.ShowNameOfDayOnToolTip = false;
		TxtFinishM.ShowToolTipText = false;
		((Control)TxtFinishM).Size = new Size(201, 20);
		((Control)TxtFinishM).TabIndex = 0;
		((Control)TxtFinishM).Text = "30/07/2024";
		((TextBox)TxtFinishM).TextAlign = (HorizontalAlignment)2;
		TxtFinishM.TootTipDelay = 500;
		((PanelControl)groupPanel8).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel8).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel8).Controls.Add((Control)(object)TXTStartDate);
		groupPanel8.DisabledBackColor = Color.Empty;
		((Control)groupPanel8).Location = new Point(410, 9);
		((Control)groupPanel8).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanel8).Name = "groupPanel8";
		((Control)groupPanel8).Size = new Size(247, 59);
		((PanelControl)groupPanel8).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel8).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel8).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel8).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel8).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel8).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel8).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel8).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel8).Style.CornerDiameter = 4;
		((PanelControl)groupPanel8).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel8).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel8).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel8).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel8).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel8).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel8).TabIndex = 0;
		((Control)groupPanel8).Text = "تاریخ شروع";
		TXTStartDate.AllowNull = false;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate).Location = new Point(27, 5);
		((Control)TXTStartDate).Margin = new Padding(4, 3, 4, 3);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2024, 7, 31, 17, 24, 20, 532);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(191, 20);
		((Control)TXTStartDate).TabIndex = 0;
		((Control)TXTStartDate).Text = "30/07/2024";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(607, 421);
		((Control)labelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(50, 23);
		((Control)labelX1).TabIndex = 5;
		((Control)labelX1).Text = "[شرح] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(23, 424);
		((Control)TxtDesc).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(576, 54);
		((Control)TxtDesc).TabIndex = 6;
		((Control)TxtDesc).Enter += TxtDesc_Enter;
		((Control)TxtDesc).Leave += TxtDesc_Leave;
		((Control)groupPanelSelTaradod).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanelSelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelSelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX39);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)BtnSelectShiftCoding);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)CmbShiftCodingType);
		groupPanelSelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelSelTaradod).Location = new Point(23, 74);
		((Control)groupPanelSelTaradod).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanelSelTaradod).Name = "groupPanelSelTaradod";
		((Control)groupPanelSelTaradod).Size = new Size(634, 58);
		((PanelControl)groupPanelSelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelSelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelSelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelSelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelSelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelSelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelSelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelSelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelSelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelSelTaradod).TabIndex = 2;
		((Control)groupPanelSelTaradod).Text = "[انتخاب نوع تردد]";
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)labelX39).Location = new Point(530, 3);
		((Control)labelX39).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(86, 23);
		((Control)labelX39).TabIndex = 0;
		((Control)labelX39).Text = "[نوع تردد]:";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)BtnSelectShiftCoding).AccessibleRole = (AccessibleRole)43;
		BtnSelectShiftCoding.ColorTable = (eButtonColor)6;
		((Control)BtnSelectShiftCoding).Location = new Point(4, 5);
		((Control)BtnSelectShiftCoding).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnSelectShiftCoding).Name = "BtnSelectShiftCoding";
		((Control)BtnSelectShiftCoding).Size = new Size(40, 23);
		((PopupItemControl)BtnSelectShiftCoding).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectShiftCoding).TabIndex = 2;
		((Control)BtnSelectShiftCoding).TabStop = false;
		((Control)BtnSelectShiftCoding).Text = "...";
		((Control)BtnSelectShiftCoding).Click += BtnSelectShiftCoding_Click;
		((Control)CmbShiftCodingType).Anchor = (AnchorStyles)13;
		((ComboBox)CmbShiftCodingType).DrawMode = (DrawMode)1;
		((ComboBox)CmbShiftCodingType).DropDownStyle = (ComboBoxStyle)2;
		((Control)CmbShiftCodingType).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ListControl)CmbShiftCodingType).FormattingEnabled = true;
		((ComboBox)CmbShiftCodingType).ItemHeight = 18;
		((Control)CmbShiftCodingType).Location = new Point(50, 5);
		((Control)CmbShiftCodingType).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbShiftCodingType).Name = "CmbShiftCodingType";
		((Control)CmbShiftCodingType).Size = new Size(472, 24);
		CmbShiftCodingType.Style = (eDotNetBarStyle)9;
		((Control)CmbShiftCodingType).TabIndex = 1;
		((Control)groupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)RbtFr3);
		((Control)groupPanel1).Controls.Add((Control)(object)RbtFr2);
		((Control)groupPanel1).Controls.Add((Control)(object)RbtFr1);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel1).Controls.Add((Control)(object)lblDescFri);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbSat);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbMon);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbSun);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbThr);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbTue);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbWed);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(23, 329);
		((Control)groupPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(634, 88);
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
		((Control)groupPanel1).TabIndex = 4;
		((Control)groupPanel1).Text = "[روز های هفته ]";
		((Control)RbtFr3).AutoSize = true;
		((Control)RbtFr3).Location = new Point(4, 8);
		((Control)RbtFr3).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtFr3).Name = "RbtFr3";
		((Control)RbtFr3).Size = new Size(14, 13);
		((Control)RbtFr3).TabIndex = 19;
		((ButtonBase)RbtFr3).UseVisualStyleBackColor = true;
		RbtFr3.CheckedChanged += RbtFri_CheckedChanged;
		((Control)RbtFr2).AutoSize = true;
		((Control)RbtFr2).Location = new Point(27, 8);
		((Control)RbtFr2).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtFr2).Name = "RbtFr2";
		((Control)RbtFr2).Size = new Size(14, 13);
		((Control)RbtFr2).TabIndex = 18;
		((ButtonBase)RbtFr2).UseVisualStyleBackColor = true;
		RbtFr2.CheckedChanged += RbtFri_CheckedChanged;
		((Control)RbtFr1).AutoSize = true;
		RbtFr1.Checked = true;
		((Control)RbtFr1).Location = new Point(50, 8);
		((Control)RbtFr1).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtFr1).Name = "RbtFr1";
		((Control)RbtFr1).Size = new Size(14, 13);
		((Control)RbtFr1).TabIndex = 17;
		RbtFr1.TabStop = true;
		((ButtonBase)RbtFr1).UseVisualStyleBackColor = true;
		RbtFr1.CheckedChanged += RbtFri_CheckedChanged;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(1, -10);
		((Control)labelX4).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(68, 23);
		((Control)labelX4).TabIndex = 16;
		((Control)labelX4).Text = "[جمعه]";
		labelX4.TextAlignment = (StringAlignment)1;
		((Control)lblDescFri).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblDescFri).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblDescFri).ForeColor = Color.DarkRed;
		((Control)lblDescFri).Location = new Point(10, 32);
		((Control)lblDescFri).Margin = new Padding(4, 3, 4, 3);
		((Control)lblDescFri).Name = "lblDescFri";
		((Control)lblDescFri).Size = new Size(612, 21);
		((Control)lblDescFri).TabIndex = 8;
		((Control)lblDescFri).Text = "[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد(در صورت حضور جمعه کاری محاسبه میشود)]";
		((BaseItemControl)ChbSat).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSat.Checked = true;
		ChbSat.CheckState = (CheckState)1;
		ChbSat.CheckValue = "Y";
		((Control)ChbSat).Location = new Point(556, 3);
		((Control)ChbSat).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbSat).Name = "ChbSat";
		((Control)ChbSat).Size = new Size(55, 23);
		((BaseItemControl)ChbSat).Style = (eDotNetBarStyle)9;
		((Control)ChbSat).TabIndex = 0;
		((Control)ChbSat).Text = "[شنبه]";
		((BaseItemControl)ChbMon).BackgroundStyle.CornerType = (eCornerType)1;
		ChbMon.Checked = true;
		ChbMon.CheckState = (CheckState)1;
		ChbMon.CheckValue = "Y";
		((Control)ChbMon).Location = new Point(383, 3);
		((Control)ChbMon).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbMon).Name = "ChbMon";
		((Control)ChbMon).Size = new Size(69, 23);
		((BaseItemControl)ChbMon).Style = (eDotNetBarStyle)9;
		((Control)ChbMon).TabIndex = 2;
		((Control)ChbMon).Text = "[دوشنبه]";
		((BaseItemControl)ChbSun).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSun.Checked = true;
		ChbSun.CheckState = (CheckState)1;
		ChbSun.CheckValue = "Y";
		((Control)ChbSun).Location = new Point(470, 3);
		((Control)ChbSun).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbSun).Name = "ChbSun";
		((Control)ChbSun).Size = new Size(68, 23);
		((BaseItemControl)ChbSun).Style = (eDotNetBarStyle)9;
		((Control)ChbSun).TabIndex = 1;
		((Control)ChbSun).Text = "[یکشنبه]";
		((BaseItemControl)ChbFri).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbFri).Location = new Point(450, 47);
		((Control)ChbFri).Name = "ChbFri";
		((Control)ChbFri).Size = new Size(56, 23);
		((BaseItemControl)ChbFri).Style = (eDotNetBarStyle)9;
		((Control)ChbFri).TabIndex = 6;
		((Control)ChbFri).Text = "[جمعه]";
		((Control)ChbFri).Visible = false;
		((BaseItemControl)ChbThr).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbThr).Location = new Point(88, 3);
		((Control)ChbThr).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbThr).Name = "ChbThr";
		((Control)ChbThr).Size = new Size(75, 23);
		((BaseItemControl)ChbThr).Style = (eDotNetBarStyle)9;
		((Control)ChbThr).TabIndex = 5;
		((Control)ChbThr).Text = "[پنجشنبه]";
		((BaseItemControl)ChbTue).BackgroundStyle.CornerType = (eCornerType)1;
		ChbTue.Checked = true;
		ChbTue.CheckState = (CheckState)1;
		ChbTue.CheckValue = "Y";
		((Control)ChbTue).Location = new Point(282, 3);
		((Control)ChbTue).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbTue).Name = "ChbTue";
		((Control)ChbTue).Size = new Size(82, 23);
		((BaseItemControl)ChbTue).Style = (eDotNetBarStyle)9;
		((Control)ChbTue).TabIndex = 3;
		((Control)ChbTue).Text = "[سه شنبه]";
		((BaseItemControl)ChbWed).BackgroundStyle.CornerType = (eCornerType)1;
		ChbWed.Checked = true;
		ChbWed.CheckState = (CheckState)1;
		ChbWed.CheckValue = "Y";
		((Control)ChbWed).Location = new Point(181, 3);
		((Control)ChbWed).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbWed).Name = "ChbWed";
		((Control)ChbWed).Size = new Size(83, 23);
		((BaseItemControl)ChbWed).Style = (eDotNetBarStyle)9;
		((Control)ChbWed).TabIndex = 4;
		((Control)ChbWed).Text = "[چهارشنبه]";
		((Control)groupPanelTaradod).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblDescPeakOverTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblNextDay);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)PanelNormal);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblValEndTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblPeakOvertime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)labelX3);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblValStartTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblEndTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblStartTime);
		groupPanelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelTaradod).Location = new Point(23, 138);
		((Control)groupPanelTaradod).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanelTaradod).Name = "groupPanelTaradod";
		((Control)groupPanelTaradod).Size = new Size(634, 185);
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
		((Control)groupPanelTaradod).TabIndex = 3;
		((Control)groupPanelTaradod).Text = "[مشخصات تردد]";
		((Control)LblDescPeakOverTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblDescPeakOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDescPeakOverTime).ForeColor = Color.DarkRed;
		((Control)LblDescPeakOverTime).Location = new Point(136, 111);
		((Control)LblDescPeakOverTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblDescPeakOverTime).Name = "LblDescPeakOverTime";
		((Control)LblDescPeakOverTime).Size = new Size(310, 21);
		((Control)LblDescPeakOverTime).TabIndex = 7;
		((Control)LblDescPeakOverTime).Text = "[شخص حداکثر میتواند تا این ساعت اضافه کاری کند]";
		((Control)LblNextDay).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNextDay).ForeColor = Color.DarkRed;
		((Control)LblNextDay).Location = new Point(10, 133);
		((Control)LblNextDay).Margin = new Padding(4, 3, 4, 3);
		((Control)LblNextDay).Name = "LblNextDay";
		((Control)LblNextDay).Size = new Size(606, 27);
		((Control)LblNextDay).TabIndex = 8;
		((Control)LblNextDay).Text = "[انتقال از روز قبل دارد]";
		LblNextDay.WordWrap = true;
		((Control)PanelNormal).Controls.Add((Control)(object)LblJarimeV);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX5);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTajilStartTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTajilEndTime);
		((Control)PanelNormal).Controls.Add((Control)(object)lblTakhirKh);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTakhirStartTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblTakhirEndTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblEndTaTime);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX8);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX16);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX10);
		((Control)PanelNormal).Controls.Add((Control)(object)lblTajilKh);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX11);
		((Control)PanelNormal).Controls.Add((Control)(object)LblStartTaTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblEndAzTime);
		((Control)PanelNormal).Controls.Add((Control)(object)LblStartAzTime);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX20);
		((Control)PanelNormal).Controls.Add((Control)(object)labelX13);
		((Control)PanelNormal).Location = new Point(4, 33);
		((Control)PanelNormal).Margin = new Padding(4, 3, 4, 3);
		((Control)PanelNormal).Name = "PanelNormal";
		((Control)PanelNormal).Size = new Size(612, 79);
		((Control)PanelNormal).TabIndex = 4;
		((Control)LblJarimeV).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblJarimeV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblJarimeV).ForeColor = Color.DarkRed;
		((Control)LblJarimeV).Location = new Point(430, 54);
		((Control)LblJarimeV).Margin = new Padding(4, 3, 4, 3);
		((Control)LblJarimeV).Name = "LblJarimeV";
		((Control)LblJarimeV).Size = new Size(63, 21);
		((Control)LblJarimeV).TabIndex = 17;
		((Control)LblJarimeV).Text = "-----";
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(507, 54);
		((Control)labelX5).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(92, 21);
		((Control)labelX5).TabIndex = 16;
		((Control)labelX5).Text = "[تاخیر جریمه:]";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)LblTajilStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTajilStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTajilStartTime).ForeColor = Color.DarkRed;
		((Control)LblTajilStartTime).Location = new Point(430, 3);
		((Control)LblTajilStartTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblTajilStartTime).Name = "LblTajilStartTime";
		((Control)LblTajilStartTime).Size = new Size(63, 23);
		((Control)LblTajilStartTime).TabIndex = 1;
		((Control)LblTajilStartTime).Text = "-----";
		((Control)LblTajilEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTajilEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTajilEndTime).ForeColor = Color.DarkRed;
		((Control)LblTajilEndTime).Location = new Point(430, 30);
		((Control)LblTajilEndTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblTajilEndTime).Name = "LblTajilEndTime";
		((Control)LblTajilEndTime).Size = new Size(63, 21);
		((Control)LblTajilEndTime).TabIndex = 9;
		((Control)LblTajilEndTime).Text = "-----";
		((Control)lblTakhirKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirKh).Location = new Point(332, 30);
		((Control)lblTakhirKh).Margin = new Padding(4, 3, 4, 3);
		((Control)lblTakhirKh).Name = "lblTakhirKh";
		((Control)lblTakhirKh).Size = new Size(89, 21);
		((Control)lblTakhirKh).TabIndex = 10;
		((Control)lblTakhirKh).Text = "[تاخیر در خروج ]";
		lblTakhirKh.TextAlignment = (StringAlignment)2;
		((Control)LblTakhirStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTakhirStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTakhirStartTime).ForeColor = Color.DarkRed;
		((Control)LblTakhirStartTime).Location = new Point(262, 3);
		((Control)LblTakhirStartTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblTakhirStartTime).Name = "LblTakhirStartTime";
		((Control)LblTakhirStartTime).Size = new Size(63, 23);
		((Control)LblTakhirStartTime).TabIndex = 3;
		((Control)LblTakhirStartTime).Text = "-----";
		((Control)LblTakhirEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTakhirEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTakhirEndTime).ForeColor = Color.DarkRed;
		((Control)LblTakhirEndTime).Location = new Point(262, 30);
		((Control)LblTakhirEndTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblTakhirEndTime).Name = "LblTakhirEndTime";
		((Control)LblTakhirEndTime).Size = new Size(63, 21);
		((Control)LblTakhirEndTime).TabIndex = 11;
		((Control)LblTakhirEndTime).Text = "-----";
		((Control)LblEndTaTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblEndTaTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndTaTime).ForeColor = Color.DarkRed;
		((Control)LblEndTaTime).Location = new Point(10, 30);
		((Control)LblEndTaTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblEndTaTime).Name = "LblEndTaTime";
		((Control)LblEndTaTime).Size = new Size(63, 21);
		((Control)LblEndTaTime).TabIndex = 15;
		((Control)LblEndTaTime).Text = "-------";
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).ForeColor = SystemColors.ControlText;
		((Control)labelX8).Location = new Point(80, 3);
		((Control)labelX8).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(21, 23);
		((Control)labelX8).TabIndex = 6;
		((Control)labelX8).Text = "[تا]:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).ForeColor = SystemColors.ControlText;
		((Control)labelX16).Location = new Point(80, 30);
		((Control)labelX16).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(21, 21);
		((Control)labelX16).TabIndex = 14;
		((Control)labelX16).Text = "تا:";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(507, 3);
		((Control)labelX10).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(92, 23);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "[تعجیل در ورود ]:";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)lblTajilKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilKh).Location = new Point(507, 30);
		((Control)lblTajilKh).Margin = new Padding(4, 3, 4, 3);
		((Control)lblTajilKh).Name = "lblTajilKh";
		((Control)lblTajilKh).Size = new Size(92, 21);
		((Control)lblTajilKh).TabIndex = 8;
		((Control)lblTajilKh).Text = "[تعجیل درخروج]:";
		lblTajilKh.TextAlignment = (StringAlignment)2;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(332, 3);
		((Control)labelX11).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(83, 23);
		((Control)labelX11).TabIndex = 2;
		((Control)labelX11).Text = "[تاخیر در ورود]:";
		labelX11.TextAlignment = (StringAlignment)2;
		((Control)LblStartTaTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartTaTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartTaTime).ForeColor = Color.DarkRed;
		((Control)LblStartTaTime).Location = new Point(10, 3);
		((Control)LblStartTaTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblStartTaTime).Name = "LblStartTaTime";
		((Control)LblStartTaTime).Size = new Size(63, 23);
		((Control)LblStartTaTime).TabIndex = 7;
		((Control)LblStartTaTime).Text = "------";
		((Control)LblEndAzTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblEndAzTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndAzTime).ForeColor = Color.DarkRed;
		((Control)LblEndAzTime).Location = new Point(108, 30);
		((Control)LblEndAzTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblEndAzTime).Name = "LblEndAzTime";
		((Control)LblEndAzTime).Size = new Size(63, 21);
		((Control)LblEndAzTime).TabIndex = 13;
		((Control)LblEndAzTime).Text = "------";
		((Control)LblStartAzTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartAzTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartAzTime).ForeColor = Color.DarkRed;
		((Control)LblStartAzTime).Location = new Point(108, 3);
		((Control)LblStartAzTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblStartAzTime).Name = "LblStartAzTime";
		((Control)LblStartAzTime).Size = new Size(63, 23);
		((Control)LblStartAzTime).TabIndex = 5;
		((Control)LblStartAzTime).Text = "-----";
		((Control)labelX20).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).ForeColor = SystemColors.ControlText;
		((Control)labelX20).Location = new Point(178, 3);
		((Control)labelX20).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(77, 23);
		((Control)labelX20).TabIndex = 4;
		((Control)labelX20).Text = "[بازه ورود از]:";
		labelX20.TextAlignment = (StringAlignment)2;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = SystemColors.ControlText;
		((Control)labelX13).Location = new Point(178, 30);
		((Control)labelX13).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(77, 21);
		((Control)labelX13).TabIndex = 12;
		((Control)labelX13).Text = "[بازه خروج از]:";
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)LblValEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblValEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValEndTime).ForeColor = Color.DarkRed;
		((Control)LblValEndTime).Location = new Point(234, 4);
		((Control)LblValEndTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblValEndTime).Name = "LblValEndTime";
		((Control)LblValEndTime).Size = new Size(46, 23);
		((Control)LblValEndTime).TabIndex = 3;
		((Control)LblValEndTime).Text = "-----";
		((Control)LblPeakOvertime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblPeakOvertime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPeakOvertime).ForeColor = Color.DarkRed;
		((Control)LblPeakOvertime).Location = new Point(443, 111);
		((Control)LblPeakOvertime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPeakOvertime).Name = "LblPeakOvertime";
		((Control)LblPeakOvertime).Size = new Size(52, 21);
		((Control)LblPeakOvertime).TabIndex = 6;
		((Control)LblPeakOvertime).Text = "-----";
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(503, 111);
		((Control)labelX3).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(118, 21);
		((Control)labelX3).TabIndex = 5;
		((Control)labelX3).Text = "[سقف اضافه کاری]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)LblValStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblValStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValStartTime).ForeColor = Color.DarkRed;
		((Control)LblValStartTime).Location = new Point(468, 2);
		((Control)LblValStartTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblValStartTime).Name = "LblValStartTime";
		((Control)LblValStartTime).Size = new Size(46, 23);
		((Control)LblValStartTime).TabIndex = 1;
		((Control)LblValStartTime).Text = "-----";
		((Control)LblEndTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblEndTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndTime).Location = new Point(287, 4);
		((Control)LblEndTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblEndTime).Name = "LblEndTime";
		((Control)LblEndTime).Size = new Size(92, 21);
		((Control)LblEndTime).TabIndex = 2;
		((Control)LblEndTime).Text = "[ساعت پایان]:";
		LblEndTime.TextAlignment = (StringAlignment)2;
		((Control)LblStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartTime).Location = new Point(516, 4);
		((Control)LblStartTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblStartTime).Name = "LblStartTime";
		((Control)LblStartTime).Size = new Size(92, 21);
		((Control)LblStartTime).TabIndex = 0;
		((Control)LblStartTime).Text = "ساعت شروع:";
		LblStartTime.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanelSurcease).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelSurcease).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelSurcease).Controls.Add((Control)(object)labelX2);
		groupPanelSurcease.DisabledBackColor = Color.Empty;
		((Control)groupPanelSurcease).Location = new Point(23, 138);
		((Control)groupPanelSurcease).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanelSurcease).Name = "groupPanelSurcease";
		((Control)groupPanelSurcease).Size = new Size(507, 167);
		((PanelControl)groupPanelSurcease).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelSurcease).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelSurcease).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelSurcease).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelSurcease).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelSurcease).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelSurcease).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelSurcease).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelSurcease).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelSurcease).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelSurcease).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelSurcease).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelSurcease).Style.CornerDiameter = 4;
		((PanelControl)groupPanelSurcease).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelSurcease).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelSurcease).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelSurcease).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelSurcease).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelSurcease).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelSurcease).TabIndex = 15;
		((Control)groupPanelSurcease).Text = "[مشخصات تردد]";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).ForeColor = Color.DarkRed;
		((Control)labelX2).Location = new Point(10, 29);
		((Control)labelX2).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(468, 55);
		((Control)labelX2).TabIndex = 14;
		((Control)labelX2).Text = "[شیفت استراحت]";
		labelX2.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(720, 565);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(603, 544);
		((Control)this).Name = "FrmShiftDet";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[جزئیات شیفت]";
		((Form)this).Load += FRMShiftDet_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMShiftDet_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanel6).ResumeLayout(false);
		((Control)groupPanel8).ResumeLayout(false);
		((Control)groupPanelSelTaradod).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)groupPanel1).PerformLayout();
		((Control)groupPanelTaradod).ResumeLayout(false);
		((Control)PanelNormal).ResumeLayout(false);
		((Control)groupPanelSurcease).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
