using System;
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
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftSplit : BaseForm
{
	public ShiftDet ClsShiftDet1;

	public ShiftDet ClsShiftDet2;

	public ShiftDet ClsShiftDet3;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private ShiftDet _clsShiftDet;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private GroupPanel groupPanel6;

	internal DateBoxFa TXTEndDate1;

	private GroupPanel groupPanel8;

	internal DateBoxFa TXTStartDate1;

	private ToolTip toolTip1;

	private GroupPanel groupPanel3;

	internal DateBoxFa TXTEndDate3;

	private GroupPanel groupPanel4;

	internal DateBoxFa TXTStartDate3;

	private GroupPanel groupPanel1;

	internal DateBoxFa TXTEndDate2;

	private GroupPanel groupPanel2;

	internal DateBoxFa TXTStartDate2;

	private LabelX labelX5;

	private LabelX labelX2;

	private LabelX labelX1;

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

	public FrmShiftSplit(string contextGuid, ShiftDet pcls)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_clsShiftDet = pcls;
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			string text = Utilities.GetTranslate("[فرم تکه کردن بازه زمانی یک شیفت به سه بازه زمانی - مانند جدا کردن بازه ایام ماه رمضان بعنوان بازه میانی شیفت ]");
			ClsShiftDet1 = await ShiftDet.Clone_ShiftDetAysnc(((CacheBaseForm)this).ContextGuid, _clsShiftDet);
			ClsShiftDet2 = await ShiftDet.Clone_ShiftDetAysnc(((CacheBaseForm)this).ContextGuid, _clsShiftDet);
			ClsShiftDet3 = await ShiftDet.Clone_ShiftDetAysnc(((CacheBaseForm)this).ContextGuid, _clsShiftDet);
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
						((Control)this).Text = text;
						((Control)TXTStartDate1).Text = Calendar.MiladiToCalture(ClsShiftDet1.DateStartM);
						((Control)TXTStartDate2).Text = Calendar.MiladiToCalture(DateTime.Now.Date);
						((Control)TXTStartDate3).Text = Calendar.MiladiToCalture(DateTime.Now.Date.AddDays(1.0));
						((Control)TXTEndDate1).Text = Calendar.MiladiToCalture(DateTime.Now.Date.AddSeconds(-1.0));
						((Control)TXTEndDate2).Text = Calendar.MiladiToCalture(DateTime.Now.Date.AddDays(1.0).AddSeconds(-1.0));
						((Control)TXTEndDate3).Text = Calendar.MiladiToCalture(ClsShiftDet1.DateFinishM);
						((Control)TXTStartDate2).Focus();
					}
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

	private void FillClsOfForm()
	{
		try
		{
			ClsShiftDet1.DateFinishM = TXTEndDate1.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
			ClsShiftDet2.DateStartM = TXTStartDate2.MiladiDate.Date;
			ClsShiftDet2.DateFinishM = TXTEndDate2.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
			ClsShiftDet3.DateStartM = TXTStartDate3.MiladiDate.Date;
			ClsShiftDet3.DateFinishM = TXTEndDate3.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillClsOfForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 91);
		}
	}

	private ReturnedSaveFuncInfo ValidateForm()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (Utilities.CalendarToMiladi(((Control)TXTEndDate1).Text) < Utilities.CalendarToMiladi(((Control)TXTStartDate1).Text))
			{
				val.AddError((EnSystem)2, "[تاریخ پایان قسمت اول شیفت بایستی بزرگتر از تاریخ شروع آن باشد]", "", "ValidateForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 101, (StringBuilder)null);
			}
			if (Utilities.CalendarToMiladi(((Control)TXTEndDate2).Text) < Utilities.CalendarToMiladi(((Control)TXTStartDate2).Text))
			{
				val.AddError((EnSystem)2, "[تاریخ پایان قسمت میانی شیفت بایستی بزرگتر از تاریخ شروع آن باشد]", "", "ValidateForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 104, (StringBuilder)null);
			}
			if (Utilities.CalendarToMiladi(((Control)TXTEndDate3).Text) < Utilities.CalendarToMiladi(((Control)TXTStartDate3).Text))
			{
				val.AddError((EnSystem)2, "[تاریخ پایان قسمت انتهای شیفت بایستی بزرگتر از تاریخ شروع آن باشد]", "", "ValidateForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 107, (StringBuilder)null);
			}
			return val;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			val.AddError(ex.Message, "", "ValidateForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 110, (StringBuilder)null);
			return val;
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected I4, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		try
		{
			if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
			{
				return;
			}
			ReturnedSaveFuncInfo val = ValidateForm();
			if (val.HasError)
			{
				((Form)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val, "[خطا]", (MessageBoxButtons)5, false, (LanguageTypes)0, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs")).ShowDialog();
				return;
			}
			FillClsOfForm();
			DialogResult val2 = ShiftDetCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, ClsShiftDet1, isShowMessage: true);
			DialogResult val3 = ShiftDetCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, ClsShiftDet2, isShowMessage: true);
			DialogResult val4 = ShiftDetCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, ClsShiftDet3, isShowMessage: true);
			DialogResult val5 = val2;
			switch (val5 - 2)
			{
			case 4:
				if ((int)val3 != 6 || (int)val4 != 6)
				{
					break;
				}
				goto case 3;
			case 3:
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			case 0:
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
				break;
			case 1:
			case 2:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 141);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void TXTStartDate2_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTEndDate1).Text = Calendar.AddDay(((Control)TXTStartDate2).Text, -1);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTStartDate2_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 154);
		}
	}

	private void TXTEndDate2_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTStartDate3).Text = Calendar.AddDay(((Control)TXTEndDate2).Text, 1);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTEndDate2_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftSplit.cs", 162);
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
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Expected O, but got Unknown
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c13: Expected O, but got Unknown
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Expected O, but got Unknown
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Expected O, but got Unknown
		//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Expected O, but got Unknown
		//IL_1359: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Expected O, but got Unknown
		//IL_1396: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a0: Expected O, but got Unknown
		//IL_1712: Unknown result type (might be due to invalid IL or missing references)
		//IL_171c: Expected O, but got Unknown
		//IL_174f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1759: Expected O, but got Unknown
		//IL_1ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ade: Expected O, but got Unknown
		//IL_1b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1b: Expected O, but got Unknown
		//IL_1e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8b: Expected O, but got Unknown
		//IL_1ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec8: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		groupPanel3 = new GroupPanel();
		TXTEndDate3 = new DateBoxFa();
		groupPanel4 = new GroupPanel();
		TXTStartDate3 = new DateBoxFa();
		groupPanel1 = new GroupPanel();
		TXTEndDate2 = new DateBoxFa();
		groupPanel2 = new GroupPanel();
		TXTStartDate2 = new DateBoxFa();
		groupPanel6 = new GroupPanel();
		TXTEndDate1 = new DateBoxFa();
		groupPanel8 = new GroupPanel();
		TXTStartDate1 = new DateBoxFa();
		toolTip1 = new ToolTip(components);
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)groupPanel3).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)groupPanel2).SuspendLayout();
		((Control)groupPanel6).SuspendLayout();
		((Control)groupPanel8).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(617, 276);
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
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 230);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 230);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel3);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel4);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel1);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel2);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel6);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel8);
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(593, 212);
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
		((PanelControl)groupPanel7).Style.Class = "";
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.Class = "";
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.Class = "";
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(222, 161);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(138, 23);
		((Control)labelX2).TabIndex = 8;
		((Control)labelX2).Text = "[قسمت انتهای شیفت]";
		labelX2.TextAlignment = (StringAlignment)1;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(222, 97);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(138, 23);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "[قسمت میانی شیفت]";
		labelX1.TextAlignment = (StringAlignment)1;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(222, 27);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(138, 23);
		((Control)labelX5).TabIndex = 6;
		((Control)labelX5).Text = "[قسمت اول شیفت]";
		labelX5.TextAlignment = (StringAlignment)1;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel3).Controls.Add((Control)(object)TXTEndDate3);
		((Control)groupPanel3).Location = new Point(20, 136);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(196, 58);
		((PanelControl)groupPanel3).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel3).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel3).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel3).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel3).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel3).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel3).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel3).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel3).Style.Class = "";
		((PanelControl)groupPanel3).Style.CornerDiameter = 4;
		((PanelControl)groupPanel3).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.Class = "";
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.Class = "";
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 5;
		((Control)groupPanel3).Text = "[تاریخ پایان]";
		TXTEndDate3.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTEndDate3).Border.Class = "";
		((TextBoxX)TXTEndDate3).Border.CornerType = (eCornerType)1;
		((Control)TXTEndDate3).Enabled = false;
		TXTEndDate3.EnterToTab = true;
		TXTEndDate3.EscToClose = true;
		TXTEndDate3.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTEndDate3.FocusBackColor = Color.Yellow;
		TXTEndDate3.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTEndDate3.FocusForeColor = Color.Blue;
		TXTEndDate3.FocusTextSelect = true;
		((Control)TXTEndDate3).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTEndDate3).Location = new Point(9, 5);
		TXTEndDate3.MessageEmptyShowDialog = false;
		TXTEndDate3.MessegeEmpty = "";
		TXTEndDate3.MessegeEmptyInFormRight = true;
		TXTEndDate3.MessegeEmptyShowInForm = false;
		TXTEndDate3.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTEndDate3).Name = "TXTEndDate3";
		((Control)TXTEndDate3).RightToLeft = (RightToLeft)0;
		((Control)TXTEndDate3).Size = new Size(172, 26);
		((Control)TXTEndDate3).TabIndex = 0;
		((Control)TXTEndDate3).Text = "1391/12/12";
		((TextBox)TXTEndDate3).TextAlign = (HorizontalAlignment)2;
		TXTEndDate3.TootTipDelay = 500;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)TXTStartDate3);
		((Control)groupPanel4).Location = new Point(366, 136);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(197, 59);
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
		((PanelControl)groupPanel4).Style.Class = "";
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.Class = "";
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.Class = "";
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 4;
		((Control)groupPanel4).Text = "[تاریخ شروع]";
		TXTStartDate3.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate3).Border.Class = "";
		((TextBoxX)TXTStartDate3).Border.CornerType = (eCornerType)1;
		((Control)TXTStartDate3).Enabled = false;
		TXTStartDate3.EnterToTab = true;
		TXTStartDate3.EscToClose = true;
		TXTStartDate3.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate3.FocusBackColor = Color.Yellow;
		TXTStartDate3.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate3.FocusForeColor = Color.Blue;
		TXTStartDate3.FocusTextSelect = true;
		((Control)TXTStartDate3).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate3).Location = new Point(13, 5);
		TXTStartDate3.MessageEmptyShowDialog = false;
		TXTStartDate3.MessegeEmpty = "";
		TXTStartDate3.MessegeEmptyInFormRight = true;
		TXTStartDate3.MessegeEmptyShowInForm = false;
		TXTStartDate3.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTStartDate3).Name = "TXTStartDate3";
		((Control)TXTStartDate3).RightToLeft = (RightToLeft)0;
		((Control)TXTStartDate3).Size = new Size(164, 26);
		((Control)TXTStartDate3).TabIndex = 0;
		((Control)TXTStartDate3).Text = "1391/12/12";
		((TextBox)TXTStartDate3).TextAlign = (HorizontalAlignment)2;
		TXTStartDate3.TootTipDelay = 500;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)TXTEndDate2);
		((Control)groupPanel1).Location = new Point(20, 73);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(196, 58);
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
		((PanelControl)groupPanel1).Style.Class = "";
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.Class = "";
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.Class = "";
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 3;
		((Control)groupPanel1).Text = "[تاریخ پایان]";
		TXTEndDate2.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTEndDate2).Border.Class = "";
		((TextBoxX)TXTEndDate2).Border.CornerType = (eCornerType)1;
		TXTEndDate2.EnterToTab = true;
		TXTEndDate2.EscToClose = true;
		TXTEndDate2.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTEndDate2.FocusBackColor = Color.Yellow;
		TXTEndDate2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTEndDate2.FocusForeColor = Color.Blue;
		TXTEndDate2.FocusTextSelect = true;
		((Control)TXTEndDate2).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTEndDate2).Location = new Point(9, 5);
		TXTEndDate2.MessageEmptyShowDialog = false;
		TXTEndDate2.MessegeEmpty = "";
		TXTEndDate2.MessegeEmptyInFormRight = true;
		TXTEndDate2.MessegeEmptyShowInForm = false;
		TXTEndDate2.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTEndDate2).Name = "TXTEndDate2";
		((Control)TXTEndDate2).RightToLeft = (RightToLeft)0;
		((Control)TXTEndDate2).Size = new Size(172, 26);
		((Control)TXTEndDate2).TabIndex = 0;
		((Control)TXTEndDate2).Text = "1391/12/12";
		((TextBox)TXTEndDate2).TextAlign = (HorizontalAlignment)2;
		TXTEndDate2.TootTipDelay = 500;
		((Control)TXTEndDate2).TextChanged += TXTEndDate2_TextChanged;
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel2).Controls.Add((Control)(object)TXTStartDate2);
		((Control)groupPanel2).Location = new Point(366, 73);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(197, 59);
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
		((PanelControl)groupPanel2).Style.Class = "";
		((PanelControl)groupPanel2).Style.CornerDiameter = 4;
		((PanelControl)groupPanel2).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel2).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel2).StyleMouseDown.Class = "";
		((PanelControl)groupPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel2).StyleMouseOver.Class = "";
		((PanelControl)groupPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel2).TabIndex = 2;
		((Control)groupPanel2).Text = "[تاریخ شروع]";
		TXTStartDate2.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate2).Border.Class = "";
		((TextBoxX)TXTStartDate2).Border.CornerType = (eCornerType)1;
		TXTStartDate2.EnterToTab = true;
		TXTStartDate2.EscToClose = true;
		TXTStartDate2.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate2.FocusBackColor = Color.Yellow;
		TXTStartDate2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate2.FocusForeColor = Color.Blue;
		TXTStartDate2.FocusTextSelect = true;
		((Control)TXTStartDate2).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate2).Location = new Point(13, 5);
		TXTStartDate2.MessageEmptyShowDialog = false;
		TXTStartDate2.MessegeEmpty = "";
		TXTStartDate2.MessegeEmptyInFormRight = true;
		TXTStartDate2.MessegeEmptyShowInForm = false;
		TXTStartDate2.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTStartDate2).Name = "TXTStartDate2";
		((Control)TXTStartDate2).RightToLeft = (RightToLeft)0;
		((Control)TXTStartDate2).Size = new Size(164, 26);
		((Control)TXTStartDate2).TabIndex = 0;
		((Control)TXTStartDate2).Text = "1391/12/12";
		((TextBox)TXTStartDate2).TextAlign = (HorizontalAlignment)2;
		TXTStartDate2.TootTipDelay = 500;
		((Control)TXTStartDate2).TextChanged += TXTStartDate2_TextChanged;
		((PanelControl)groupPanel6).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel6).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel6).Controls.Add((Control)(object)TXTEndDate1);
		((Control)groupPanel6).Location = new Point(20, 9);
		((Control)groupPanel6).Name = "groupPanel6";
		((Control)groupPanel6).Size = new Size(196, 58);
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
		((PanelControl)groupPanel6).Style.Class = "";
		((PanelControl)groupPanel6).Style.CornerDiameter = 4;
		((PanelControl)groupPanel6).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel6).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel6).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel6).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel6).StyleMouseDown.Class = "";
		((PanelControl)groupPanel6).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel6).StyleMouseOver.Class = "";
		((PanelControl)groupPanel6).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel6).TabIndex = 1;
		((Control)groupPanel6).Text = "[تاریخ پایان]";
		TXTEndDate1.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTEndDate1).Border.Class = "";
		((TextBoxX)TXTEndDate1).Border.CornerType = (eCornerType)1;
		((Control)TXTEndDate1).Enabled = false;
		TXTEndDate1.EnterToTab = true;
		TXTEndDate1.EscToClose = true;
		TXTEndDate1.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTEndDate1.FocusBackColor = Color.Yellow;
		TXTEndDate1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTEndDate1.FocusForeColor = Color.Blue;
		TXTEndDate1.FocusTextSelect = true;
		((Control)TXTEndDate1).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTEndDate1).Location = new Point(8, 5);
		TXTEndDate1.MessageEmptyShowDialog = false;
		TXTEndDate1.MessegeEmpty = "";
		TXTEndDate1.MessegeEmptyInFormRight = true;
		TXTEndDate1.MessegeEmptyShowInForm = false;
		TXTEndDate1.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTEndDate1).Name = "TXTEndDate1";
		((Control)TXTEndDate1).RightToLeft = (RightToLeft)0;
		((Control)TXTEndDate1).Size = new Size(172, 26);
		((Control)TXTEndDate1).TabIndex = 0;
		((Control)TXTEndDate1).Text = "1391/12/12";
		((TextBox)TXTEndDate1).TextAlign = (HorizontalAlignment)2;
		TXTEndDate1.TootTipDelay = 500;
		((PanelControl)groupPanel8).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel8).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel8).Controls.Add((Control)(object)TXTStartDate1);
		((Control)groupPanel8).Location = new Point(366, 9);
		((Control)groupPanel8).Name = "groupPanel8";
		((Control)groupPanel8).Size = new Size(197, 59);
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
		((PanelControl)groupPanel8).Style.Class = "";
		((PanelControl)groupPanel8).Style.CornerDiameter = 4;
		((PanelControl)groupPanel8).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel8).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel8).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel8).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel8).StyleMouseDown.Class = "";
		((PanelControl)groupPanel8).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel8).StyleMouseOver.Class = "";
		((PanelControl)groupPanel8).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel8).TabIndex = 0;
		((Control)groupPanel8).Text = "[تاریخ شروع]";
		TXTStartDate1.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate1).Border.Class = "";
		((TextBoxX)TXTStartDate1).Border.CornerType = (eCornerType)1;
		((Control)TXTStartDate1).Enabled = false;
		TXTStartDate1.EnterToTab = true;
		TXTStartDate1.EscToClose = true;
		TXTStartDate1.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate1.FocusBackColor = Color.Yellow;
		TXTStartDate1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate1.FocusForeColor = Color.Blue;
		TXTStartDate1.FocusTextSelect = true;
		((Control)TXTStartDate1).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate1).Location = new Point(15, 4);
		TXTStartDate1.MessageEmptyShowDialog = false;
		TXTStartDate1.MessegeEmpty = "";
		TXTStartDate1.MessegeEmptyInFormRight = true;
		TXTStartDate1.MessegeEmptyShowInForm = false;
		TXTStartDate1.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTStartDate1).Name = "TXTStartDate1";
		((Control)TXTStartDate1).RightToLeft = (RightToLeft)0;
		((Control)TXTStartDate1).Size = new Size(164, 26);
		((Control)TXTStartDate1).TabIndex = 0;
		((Control)TXTStartDate1).Text = "1391/12/12";
		((TextBox)TXTStartDate1).TextAlign = (HorizontalAlignment)2;
		TXTStartDate1.TootTipDelay = 500;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(617, 276);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(633, 315);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(633, 315);
		((Control)this).Name = "FrmShiftSplit";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[جزئیات شیفت]";
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanel3).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)groupPanel2).ResumeLayout(false);
		((Control)groupPanel6).ResumeLayout(false);
		((Control)groupPanel8).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
