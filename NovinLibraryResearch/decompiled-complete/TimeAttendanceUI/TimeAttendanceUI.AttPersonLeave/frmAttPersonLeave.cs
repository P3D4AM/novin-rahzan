using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.Attendance;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.AttPersonLeave;

public class frmAttPersonLeave : CacheBaseForm
{
	private CancellationTokenSource _loadToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private GroupBox groupBox2;

	private Label label4;

	private TextBox txtReason;

	private Label label5;

	private Label label9;

	private Label label6;

	private TextBox txtManagerMessage;

	private Label label7;

	private Panel panel4;

	private RadioButton rbtnRemoved;

	private RadioButton rbnAccepted;

	private RadioButton rbnPending;

	private RadioButton rbnFailed;

	private Panel panel1;

	private RadioButton rbnDaily;

	private RadioButton rbnHourly;

	private Panel panel2;

	private RadioButton RbnTashvighi;

	private RadioButton RbnMission;

	private RadioButton rbnBiHoghogh;

	private RadioButton rbnEstehghaghi;

	private RadioButton rbnEstelaji;

	private GroupBox groupBox1;

	private DateBoxFa dateModifiedDate;

	private TextBox txtToHour;

	private Label label1;

	private TextBox txtFromHour;

	private DateBoxFa dateRequestDate;

	private Label label2;

	private DateBoxFa dateFromDate;

	private DateBoxFa dateToDate;

	private Label label8;

	private Label label11;

	private ButtonX BtnAccept;

	public AttPersonLeaveBusiness Item { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((CacheBaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				_loadToken?.Cancel();
				_loadToken = null;
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
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
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					Person obj = await Person.GetBypersonGuidAsync(((CacheBaseForm)this).ContextGuid, Item.PersonGuid, token);
					string text = ((obj != null) ? ((PersonBase)obj).FullName : null);
					if (text == null)
					{
						MessageBox.Show("درخواست دهنده نامشخص است. لذا امکان بررسی وجود ندارد");
					}
					else
					{
						((Control)this).Text = "ویرایش درخواست " + text;
						DateBoxFa obj2 = dateRequestDate;
						AttPersonLeaveBusiness item = Item;
						((Control)obj2).Text = ((item != null) ? item.RequestDateSh : null);
						DateBoxFa obj3 = dateModifiedDate;
						AttPersonLeaveBusiness item2 = Item;
						string obj4 = ((item2 != null) ? Calendar.MiladiToShamsi(item2.Modified) : null);
						AttPersonLeaveBusiness item3 = Item;
						((Control)obj3).Text = obj4 + " " + ((item3 != null) ? item3.Modified.TimeOfDay.ToString().Substring(0, 5) : null);
						if ((int)Item.Type == 1)
						{
							((Control)txtToHour).Text = "00:00";
							((Control)txtFromHour).Text = "00:00";
						}
						else
						{
							TextBox obj5 = txtFromHour;
							AttPersonLeaveBusiness item4 = Item;
							((Control)obj5).Text = ((item4 != null) ? item4.DateFrom.TimeOfDay.ToString().Substring(0, 5) : null);
							TextBox obj6 = txtToHour;
							AttPersonLeaveBusiness item5 = Item;
							((Control)obj6).Text = ((item5 != null) ? item5.DateTo.TimeOfDay.ToString().Substring(0, 5) : null);
						}
						DateBoxFa obj7 = dateToDate;
						AttPersonLeaveBusiness item6 = Item;
						((Control)obj7).Text = ((item6 != null) ? item6.DateToSh : null);
						DateBoxFa obj8 = dateFromDate;
						AttPersonLeaveBusiness item7 = Item;
						((Control)obj8).Text = ((item7 != null) ? item7.DateFromSh : null);
						TextBox obj9 = txtReason;
						AttPersonLeaveBusiness item8 = Item;
						((Control)obj9).Text = ((item8 != null) ? item8.Reason : null);
						TextBox obj10 = txtManagerMessage;
						AttPersonLeaveBusiness item9 = Item;
						((Control)obj10).Text = ((item9 != null) ? item9.ManagerMessage : null);
						EnPersonLeaveType type = Item.Type;
						if ((int)type != 0)
						{
							if ((int)type == 1)
							{
								rbnDaily.Checked = true;
							}
							else
							{
								rbnHourly.Checked = true;
							}
						}
						else
						{
							rbnHourly.Checked = true;
						}
						EnPersonLeaveStatus leaveStatus = Item.LeaveStatus;
						switch ((int)leaveStatus)
						{
						case 0:
							rbnPending.Checked = true;
							break;
						case 1:
							rbnFailed.Checked = true;
							break;
						case 2:
							rbnAccepted.Checked = true;
							break;
						case 3:
							rbtnRemoved.Checked = true;
							break;
						default:
							rbnPending.Checked = true;
							break;
						}
						EnPersonLeaveKind kind = Item.Kind;
						switch (kind - 1)
						{
						case 1:
							rbnEstehghaghi.Checked = true;
							break;
						case 0:
							rbnEstelaji.Checked = true;
							break;
						case 3:
							rbnBiHoghogh.Checked = true;
							break;
						case 4:
							RbnMission.Checked = true;
							break;
						case 2:
							RbnTashvighi.Checked = true;
							break;
						default:
							rbnEstehghaghi.Checked = true;
							break;
						}
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

	public frmAttPersonLeave(string contextGuid, AttPersonLeaveBusiness item)
		: base(contextGuid)
	{
		if (item == null)
		{
			throw new NullReferenceException("item");
		}
		try
		{
			InitializeComponent();
			Item = item;
			_loadToken?.Cancel();
			_loadToken = new CancellationTokenSource(60000);
			Task.Run(() => LoadDataAsync(_loadToken.Token));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void frmAttPersonLeave_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					((PopupItemControl)BtnAccept).PerformClick();
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
			Log.Error(ex, "");
		}
	}

	private void frmAttPersonLeave_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmAttPersonLeave_Load(object sender, EventArgs e)
	{
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (rbnAccepted.Checked)
			{
				Item.LeaveStatus = (EnPersonLeaveStatus)2;
			}
			if (rbnFailed.Checked)
			{
				Item.LeaveStatus = (EnPersonLeaveStatus)1;
			}
			if (rbnPending.Checked)
			{
				Item.LeaveStatus = (EnPersonLeaveStatus)0;
			}
			if (rbtnRemoved.Checked)
			{
				Item.LeaveStatus = (EnPersonLeaveStatus)3;
			}
			Item.ManagerMessage = ((Control)txtManagerMessage).Text;
			Item.ManagerDate = DateTime.Now;
			Item.Modified = DateTime.Now;
			if (rbnEstehghaghi.Checked)
			{
				Item.Kind = (EnPersonLeaveKind)2;
			}
			if (rbnEstelaji.Checked)
			{
				Item.Kind = (EnPersonLeaveKind)1;
			}
			if (rbnBiHoghogh.Checked)
			{
				Item.Kind = (EnPersonLeaveKind)4;
			}
			if (RbnMission.Checked)
			{
				Item.Kind = (EnPersonLeaveKind)5;
			}
			if (RbnTashvighi.Checked)
			{
				Item.Kind = (EnPersonLeaveKind)3;
			}
			if (rbnHourly.Checked)
			{
				Item.Type = (EnPersonLeaveType)0;
			}
			if (rbnDaily.Checked)
			{
				Item.Type = (EnPersonLeaveType)1;
			}
			Item.DateFrom = Utilities.CalendarToMiladi(((Control)dateFromDate).Text);
			TimeSpan result;
			TimeSpan timeSpan = (TimeSpan.TryParse(((Control)txtFromHour).Text, out result) ? result : TimeSpan.Zero);
			TimeSpan result2;
			TimeSpan timeSpan2 = (TimeSpan.TryParse(((Control)txtToHour).Text, out result2) ? result2 : TimeSpan.Zero);
			Item.DateFrom = Utilities.CalendarToMiladi(((Control)dateFromDate).Text).Date.AddMinutes(timeSpan.TotalMinutes);
			Item.DateTo = Utilities.CalendarToMiladi(((Control)dateToDate).Text).Date.AddMinutes(timeSpan2.TotalMinutes);
			ReturnedSaveFuncInfo val = res;
			val.AddReturnedValue(await Item.SaveAsync(((CacheBaseForm)this).system, true, (Priority)0, default(CancellationToken), true, false, ""), (ReturnedState)0, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\frmAttPersonLeave.cs", 183);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			res.AddReturnedValue(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\frmAttPersonLeave.cs", 188);
		}
		finally
		{
			if (res.HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<frmAttPersonLeave>(this, res, "پیغام سیستم", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\frmAttPersonLeave.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void rbnHourly_CheckedChanged(object sender, EventArgs e)
	{
		if (rbnHourly.Checked)
		{
			((Control)dateToDate).Enabled = false;
			((Control)dateFromDate).Enabled = false;
		}
		else
		{
			((Control)dateToDate).Enabled = true;
			((Control)dateFromDate).Enabled = true;
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1000: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_111d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1278: Unknown result type (might be due to invalid IL or missing references)
		//IL_129c: Unknown result type (might be due to invalid IL or missing references)
		//IL_136c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1376: Expected O, but got Unknown
		//IL_13a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ac: Expected O, but got Unknown
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d0: Expected O, but got Unknown
		//IL_14ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1561: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Expected O, but got Unknown
		//IL_15e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ca: Expected O, but got Unknown
		//IL_16f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1700: Expected O, but got Unknown
		//IL_1721: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Unknown result type (might be due to invalid IL or missing references)
		//IL_190b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1915: Expected O, but got Unknown
		//IL_1941: Unknown result type (might be due to invalid IL or missing references)
		//IL_194b: Expected O, but got Unknown
		//IL_196c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1add: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae7: Expected O, but got Unknown
		//IL_1b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1d: Expected O, but got Unknown
		//IL_1b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7d: Expected O, but got Unknown
		//IL_1d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9a: Expected O, but got Unknown
		//IL_1da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df0: Expected O, but got Unknown
		//IL_1e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e14: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmAttPersonLeave));
		panelEx1 = new PanelEx();
		groupBox2 = new GroupBox();
		BtnAccept = new ButtonX();
		label4 = new Label();
		txtReason = new TextBox();
		label5 = new Label();
		label9 = new Label();
		label6 = new Label();
		txtManagerMessage = new TextBox();
		label7 = new Label();
		panel4 = new Panel();
		rbtnRemoved = new RadioButton();
		rbnAccepted = new RadioButton();
		rbnPending = new RadioButton();
		rbnFailed = new RadioButton();
		panel1 = new Panel();
		rbnDaily = new RadioButton();
		rbnHourly = new RadioButton();
		panel2 = new Panel();
		RbnTashvighi = new RadioButton();
		RbnMission = new RadioButton();
		rbnBiHoghogh = new RadioButton();
		rbnEstehghaghi = new RadioButton();
		rbnEstelaji = new RadioButton();
		groupBox1 = new GroupBox();
		dateModifiedDate = new DateBoxFa();
		txtToHour = new TextBox();
		label1 = new Label();
		txtFromHour = new TextBox();
		dateRequestDate = new DateBoxFa();
		label2 = new Label();
		dateFromDate = new DateBoxFa();
		dateToDate = new DateBoxFa();
		label8 = new Label();
		label11 = new Label();
		((Control)panelEx1).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
		((Control)panel4).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)groupBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).AutoSize = true;
		panelEx1.CanvasColor = SystemColors.ActiveCaption;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)groupBox2);
		((Control)panelEx1).Controls.Add((Control)(object)groupBox1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(646, 550);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Panel)panelEx1).TabStop = true;
		((Control)groupBox2).Controls.Add((Control)(object)BtnAccept);
		((Control)groupBox2).Controls.Add((Control)(object)label4);
		((Control)groupBox2).Controls.Add((Control)(object)txtReason);
		((Control)groupBox2).Controls.Add((Control)(object)label5);
		((Control)groupBox2).Controls.Add((Control)(object)label9);
		((Control)groupBox2).Controls.Add((Control)(object)label6);
		((Control)groupBox2).Controls.Add((Control)(object)txtManagerMessage);
		((Control)groupBox2).Controls.Add((Control)(object)label7);
		((Control)groupBox2).Controls.Add((Control)(object)panel4);
		((Control)groupBox2).Controls.Add((Control)(object)panel1);
		((Control)groupBox2).Controls.Add((Control)(object)panel2);
		((Control)groupBox2).Location = new Point(10, 151);
		((Control)groupBox2).Margin = new Padding(2, 3, 2, 3);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Padding = new Padding(2, 3, 2, 3);
		((Control)groupBox2).Size = new Size(625, 394);
		((Control)groupBox2).TabIndex = 34;
		groupBox2.TabStop = false;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(10, 354);
		((Control)BtnAccept).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(134, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 33;
		((Control)BtnAccept).Text = "[ثبت] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)label4).AutoSize = true;
		((Control)label4).Location = new Point(525, 69);
		((Control)label4).Margin = new Padding(2, 0, 2, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(80, 13);
		((Control)label4).TabIndex = 7;
		((Control)label4).Text = "دوره مرخصی:";
		((Control)txtReason).Enabled = false;
		((Control)txtReason).Location = new Point(10, 134);
		((Control)txtReason).Margin = new Padding(2, 3, 2, 3);
		((TextBoxBase)txtReason).Multiline = true;
		((Control)txtReason).Name = "txtReason";
		((Control)txtReason).Size = new Size(511, 76);
		((Control)txtReason).TabIndex = 8;
		((Control)label5).AutoSize = true;
		((Control)label5).Location = new Point(525, 107);
		((Control)label5).Margin = new Padding(2, 0, 2, 0);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(94, 13);
		((Control)label5).TabIndex = 9;
		((Control)label5).Text = "وضعیت مرخصی:";
		((Control)label9).AutoSize = true;
		((Control)label9).Location = new Point(525, 222);
		((Control)label9).Margin = new Padding(2, 0, 2, 0);
		((Control)label9).Name = "label9";
		((Control)label9).Size = new Size(55, 13);
		((Control)label9).TabIndex = 23;
		((Control)label9).Text = "پیام مدیر:";
		((Control)label6).AutoSize = true;
		((Control)label6).Location = new Point(525, 31);
		((Control)label6).Margin = new Padding(2, 0, 2, 0);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(75, 13);
		((Control)label6).TabIndex = 12;
		((Control)label6).Text = "نوع مرخصی:";
		((Control)txtManagerMessage).Location = new Point(10, 216);
		((Control)txtManagerMessage).Margin = new Padding(2, 3, 2, 3);
		((TextBoxBase)txtManagerMessage).Multiline = true;
		((Control)txtManagerMessage).Name = "txtManagerMessage";
		((Control)txtManagerMessage).Size = new Size(511, 130);
		((Control)txtManagerMessage).TabIndex = 22;
		((Control)label7).AutoSize = true;
		((Control)label7).Location = new Point(525, 141);
		((Control)label7).Margin = new Padding(2, 0, 2, 0);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(88, 13);
		((Control)label7).TabIndex = 14;
		((Control)label7).Text = "دلیل درخواست:";
		((Control)panel4).Controls.Add((Control)(object)rbtnRemoved);
		((Control)panel4).Controls.Add((Control)(object)rbnAccepted);
		((Control)panel4).Controls.Add((Control)(object)rbnPending);
		((Control)panel4).Controls.Add((Control)(object)rbnFailed);
		((Control)panel4).Location = new Point(6, 97);
		((Control)panel4).Margin = new Padding(2, 3, 2, 3);
		((Control)panel4).Name = "panel4";
		((Control)panel4).Size = new Size(516, 32);
		((Control)panel4).TabIndex = 19;
		((Control)rbtnRemoved).AutoSize = true;
		((Control)rbtnRemoved).Cursor = Cursors.Hand;
		((Control)rbtnRemoved).Location = new Point(100, 6);
		((Control)rbtnRemoved).Margin = new Padding(2, 3, 2, 3);
		((Control)rbtnRemoved).Name = "rbtnRemoved";
		((Control)rbtnRemoved).Size = new Size(77, 17);
		((Control)rbtnRemoved).TabIndex = 18;
		rbtnRemoved.TabStop = true;
		((Control)rbtnRemoved).Text = "حذف شده";
		((ButtonBase)rbtnRemoved).UseVisualStyleBackColor = true;
		((Control)rbnAccepted).AutoSize = true;
		((Control)rbnAccepted).Cursor = Cursors.Hand;
		((Control)rbnAccepted).Location = new Point(200, 6);
		((Control)rbnAccepted).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnAccepted).Name = "rbnAccepted";
		((Control)rbnAccepted).Size = new Size(89, 17);
		((Control)rbnAccepted).TabIndex = 18;
		rbnAccepted.TabStop = true;
		((Control)rbnAccepted).Text = "پذیرفته شده";
		((ButtonBase)rbnAccepted).UseVisualStyleBackColor = true;
		((Control)rbnPending).AutoSize = true;
		((Control)rbnPending).Cursor = Cursors.Hand;
		((Control)rbnPending).Location = new Point(405, 6);
		((Control)rbnPending).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnPending).Name = "rbnPending";
		((Control)rbnPending).Size = new Size(94, 17);
		((Control)rbnPending).TabIndex = 1;
		rbnPending.TabStop = true;
		((Control)rbnPending).Text = "در انتظار تائید";
		((ButtonBase)rbnPending).UseVisualStyleBackColor = true;
		((Control)rbnFailed).AutoSize = true;
		((Control)rbnFailed).Cursor = Cursors.Hand;
		((Control)rbnFailed).Location = new Point(327, 6);
		((Control)rbnFailed).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnFailed).Name = "rbnFailed";
		((Control)rbnFailed).Size = new Size(63, 17);
		((Control)rbnFailed).TabIndex = 0;
		rbnFailed.TabStop = true;
		((Control)rbnFailed).Text = "رد شده";
		((ButtonBase)rbnFailed).UseVisualStyleBackColor = true;
		((Control)panel1).Controls.Add((Control)(object)rbnDaily);
		((Control)panel1).Controls.Add((Control)(object)rbnHourly);
		((Control)panel1).Location = new Point(5, 59);
		((Control)panel1).Margin = new Padding(2, 3, 2, 3);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(516, 32);
		((Control)panel1).TabIndex = 15;
		((Control)rbnDaily).AutoSize = true;
		((Control)rbnDaily).Location = new Point(443, 7);
		((Control)rbnDaily).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnDaily).Name = "rbnDaily";
		((Control)rbnDaily).Size = new Size(56, 17);
		((Control)rbnDaily).TabIndex = 1;
		rbnDaily.TabStop = true;
		((Control)rbnDaily).Text = "روزانه";
		((ButtonBase)rbnDaily).UseVisualStyleBackColor = true;
		((Control)rbnHourly).AutoSize = true;
		((Control)rbnHourly).Location = new Point(326, 7);
		((Control)rbnHourly).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnHourly).Name = "rbnHourly";
		((Control)rbnHourly).Size = new Size(64, 17);
		((Control)rbnHourly).TabIndex = 0;
		rbnHourly.TabStop = true;
		((Control)rbnHourly).Text = "ساعتی";
		((ButtonBase)rbnHourly).UseVisualStyleBackColor = true;
		rbnHourly.CheckedChanged += rbnHourly_CheckedChanged;
		((Control)panel2).Controls.Add((Control)(object)RbnTashvighi);
		((Control)panel2).Controls.Add((Control)(object)RbnMission);
		((Control)panel2).Controls.Add((Control)(object)rbnBiHoghogh);
		((Control)panel2).Controls.Add((Control)(object)rbnEstehghaghi);
		((Control)panel2).Controls.Add((Control)(object)rbnEstelaji);
		((Control)panel2).Location = new Point(6, 21);
		((Control)panel2).Margin = new Padding(2, 3, 2, 3);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(516, 32);
		((Control)panel2).TabIndex = 16;
		((Control)RbnTashvighi).AutoSize = true;
		((Control)RbnTashvighi).Location = new Point(219, 6);
		((Control)RbnTashvighi).Margin = new Padding(2, 3, 2, 3);
		((Control)RbnTashvighi).Name = "RbnTashvighi";
		((Control)RbnTashvighi).Size = new Size(70, 17);
		((Control)RbnTashvighi).TabIndex = 20;
		RbnTashvighi.TabStop = true;
		((Control)RbnTashvighi).Text = "تشویقی";
		((ButtonBase)RbnTashvighi).UseVisualStyleBackColor = true;
		((Control)RbnMission).AutoSize = true;
		((Control)RbnMission).Location = new Point(3, 6);
		((Control)RbnMission).Margin = new Padding(2, 3, 2, 3);
		((Control)RbnMission).Name = "RbnMission";
		((Control)RbnMission).Size = new Size(69, 17);
		((Control)RbnMission).TabIndex = 19;
		RbnMission.TabStop = true;
		((Control)RbnMission).Text = "ماموریت";
		((ButtonBase)RbnMission).UseVisualStyleBackColor = true;
		((Control)rbnBiHoghogh).AutoSize = true;
		((Control)rbnBiHoghogh).Location = new Point(93, 6);
		((Control)rbnBiHoghogh).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnBiHoghogh).Name = "rbnBiHoghogh";
		((Control)rbnBiHoghogh).Size = new Size(84, 17);
		((Control)rbnBiHoghogh).TabIndex = 18;
		rbnBiHoghogh.TabStop = true;
		((Control)rbnBiHoghogh).Text = "بدون حقوق";
		((ButtonBase)rbnBiHoghogh).UseVisualStyleBackColor = true;
		((Control)rbnEstehghaghi).AutoSize = true;
		((Control)rbnEstehghaghi).Location = new Point(418, 6);
		((Control)rbnEstehghaghi).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnEstehghaghi).Name = "rbnEstehghaghi";
		((Control)rbnEstehghaghi).Size = new Size(81, 17);
		((Control)rbnEstehghaghi).TabIndex = 1;
		rbnEstehghaghi.TabStop = true;
		((Control)rbnEstehghaghi).Text = "استحقاقی";
		((ButtonBase)rbnEstehghaghi).UseVisualStyleBackColor = true;
		((Control)rbnEstelaji).AutoSize = true;
		((Control)rbnEstelaji).Location = new Point(311, 6);
		((Control)rbnEstelaji).Margin = new Padding(2, 3, 2, 3);
		((Control)rbnEstelaji).Name = "rbnEstelaji";
		((Control)rbnEstelaji).Size = new Size(79, 17);
		((Control)rbnEstelaji).TabIndex = 0;
		rbnEstelaji.TabStop = true;
		((Control)rbnEstelaji).Text = "استعلاجی";
		((ButtonBase)rbnEstelaji).UseVisualStyleBackColor = true;
		((Control)groupBox1).Controls.Add((Control)(object)dateModifiedDate);
		((Control)groupBox1).Controls.Add((Control)(object)txtToHour);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Controls.Add((Control)(object)txtFromHour);
		((Control)groupBox1).Controls.Add((Control)(object)dateRequestDate);
		((Control)groupBox1).Controls.Add((Control)(object)label2);
		((Control)groupBox1).Controls.Add((Control)(object)dateFromDate);
		((Control)groupBox1).Controls.Add((Control)(object)dateToDate);
		((Control)groupBox1).Controls.Add((Control)(object)label8);
		((Control)groupBox1).Controls.Add((Control)(object)label11);
		((Control)groupBox1).Location = new Point(10, 5);
		((Control)groupBox1).Margin = new Padding(2, 3, 2, 3);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Padding = new Padding(2, 3, 2, 3);
		((Control)groupBox1).Size = new Size(625, 146);
		((Control)groupBox1).TabIndex = 33;
		groupBox1.TabStop = false;
		dateModifiedDate.AllowNull = false;
		dateModifiedDate.BackcolorTooltip = Color.Empty;
		((TextBoxX)dateModifiedDate).Border.Class = "TextBoxBorder";
		((TextBoxX)dateModifiedDate).Border.CornerType = (eCornerType)1;
		((Control)dateModifiedDate).Enabled = false;
		dateModifiedDate.EnterToTab = true;
		dateModifiedDate.EscToClose = true;
		dateModifiedDate.FarsyDateStyle = (ENFarsyDateStyle)0;
		dateModifiedDate.FocusBackColor = Color.Yellow;
		dateModifiedDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dateModifiedDate.FocusForeColor = Color.Blue;
		dateModifiedDate.FocusTextSelect = true;
		((Control)dateModifiedDate).Font = new Font("B Titr", 9.75f);
		((Control)dateModifiedDate).Location = new Point(8, 25);
		((Control)dateModifiedDate).Margin = new Padding(2, 3, 2, 3);
		dateModifiedDate.MessageEmptyShowDialog = false;
		dateModifiedDate.MessegeEmpty = "";
		dateModifiedDate.MessegeEmptyInFormRight = true;
		dateModifiedDate.MessegeEmptyShowInForm = false;
		dateModifiedDate.MiladiDate = new DateTime(2025, 6, 29, 11, 58, 33, 857);
		((Control)dateModifiedDate).Name = "dateModifiedDate";
		((TextBoxX)dateModifiedDate).PreventEnterBeep = true;
		((Control)dateModifiedDate).RightToLeft = (RightToLeft)0;
		dateModifiedDate.ShowNameOfDayOnToolTip = false;
		dateModifiedDate.ShowToolTipText = false;
		((Control)dateModifiedDate).Size = new Size(141, 27);
		((Control)dateModifiedDate).TabIndex = 21;
		((Control)dateModifiedDate).Text = "28/06/2025";
		((TextBox)dateModifiedDate).TextAlign = (HorizontalAlignment)2;
		dateModifiedDate.TootTipDelay = 0;
		((Control)txtToHour).Font = new Font("B Titr", 9.75f);
		((Control)txtToHour).Location = new Point(8, 75);
		((Control)txtToHour).Margin = new Padding(2, 3, 2, 3);
		((Control)txtToHour).Name = "txtToHour";
		((Control)txtToHour).Size = new Size(54, 27);
		((Control)txtToHour).TabIndex = 30;
		txtToHour.TextAlign = (HorizontalAlignment)2;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(525, 32);
		((Control)label1).Margin = new Padding(2, 0, 2, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(91, 13);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "تاریخ درخواست:";
		((Control)txtFromHour).Font = new Font("B Titr", 9.75f);
		((Control)txtFromHour).Location = new Point(376, 75);
		((Control)txtFromHour).Margin = new Padding(2, 3, 2, 3);
		((Control)txtFromHour).Name = "txtFromHour";
		((Control)txtFromHour).Size = new Size(54, 27);
		((Control)txtFromHour).TabIndex = 29;
		txtFromHour.TextAlign = (HorizontalAlignment)2;
		dateRequestDate.AllowNull = false;
		dateRequestDate.BackcolorTooltip = Color.Empty;
		((TextBoxX)dateRequestDate).Border.Class = "TextBoxBorder";
		((TextBoxX)dateRequestDate).Border.CornerType = (eCornerType)1;
		((Control)dateRequestDate).Enabled = false;
		dateRequestDate.EnterToTab = true;
		dateRequestDate.EscToClose = true;
		dateRequestDate.FarsyDateStyle = (ENFarsyDateStyle)0;
		dateRequestDate.FocusBackColor = Color.Yellow;
		dateRequestDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dateRequestDate.FocusForeColor = Color.Blue;
		dateRequestDate.FocusTextSelect = true;
		((Control)dateRequestDate).Font = new Font("B Titr", 9.75f);
		((Control)dateRequestDate).Location = new Point(376, 25);
		((Control)dateRequestDate).Margin = new Padding(2, 3, 2, 3);
		dateRequestDate.MessageEmptyShowDialog = false;
		dateRequestDate.MessegeEmpty = "";
		dateRequestDate.MessegeEmptyInFormRight = true;
		dateRequestDate.MessegeEmptyShowInForm = false;
		dateRequestDate.MiladiDate = new DateTime(2025, 6, 28, 18, 37, 20, 866);
		((Control)dateRequestDate).Name = "dateRequestDate";
		((TextBoxX)dateRequestDate).PreventEnterBeep = true;
		((Control)dateRequestDate).RightToLeft = (RightToLeft)0;
		dateRequestDate.ShowNameOfDayOnToolTip = false;
		dateRequestDate.ShowToolTipText = false;
		((Control)dateRequestDate).Size = new Size(145, 27);
		((Control)dateRequestDate).TabIndex = 2;
		((Control)dateRequestDate).Text = "28/06/2025";
		((TextBox)dateRequestDate).TextAlign = (HorizontalAlignment)2;
		dateRequestDate.TootTipDelay = 0;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(525, 82);
		((Control)label2).Margin = new Padding(2, 0, 2, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(48, 13);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "از تاریخ:";
		dateFromDate.AllowNull = false;
		dateFromDate.BackcolorTooltip = Color.Empty;
		((TextBoxX)dateFromDate).Border.Class = "TextBoxBorder";
		((TextBoxX)dateFromDate).Border.CornerType = (eCornerType)1;
		dateFromDate.EnterToTab = true;
		dateFromDate.EscToClose = true;
		dateFromDate.FarsyDateStyle = (ENFarsyDateStyle)0;
		dateFromDate.FocusBackColor = Color.Yellow;
		dateFromDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dateFromDate.FocusForeColor = Color.Blue;
		dateFromDate.FocusTextSelect = true;
		((Control)dateFromDate).Font = new Font("B Titr", 9.75f);
		((Control)dateFromDate).Location = new Point(439, 75);
		((Control)dateFromDate).Margin = new Padding(2, 3, 2, 3);
		dateFromDate.MessageEmptyShowDialog = false;
		dateFromDate.MessegeEmpty = "";
		dateFromDate.MessegeEmptyInFormRight = true;
		dateFromDate.MessegeEmptyShowInForm = false;
		dateFromDate.MiladiDate = new DateTime(2025, 6, 28, 21, 37, 36, 869);
		((Control)dateFromDate).Name = "dateFromDate";
		((TextBoxX)dateFromDate).PreventEnterBeep = true;
		((Control)dateFromDate).RightToLeft = (RightToLeft)0;
		dateFromDate.ShowNameOfDayOnToolTip = false;
		dateFromDate.ShowToolTipText = false;
		((Control)dateFromDate).Size = new Size(82, 27);
		((Control)dateFromDate).TabIndex = 28;
		((Control)dateFromDate).Text = "28/06/2025";
		((TextBox)dateFromDate).TextAlign = (HorizontalAlignment)2;
		dateFromDate.TootTipDelay = 0;
		dateToDate.AllowNull = false;
		dateToDate.BackcolorTooltip = Color.Empty;
		((TextBoxX)dateToDate).Border.Class = "TextBoxBorder";
		((TextBoxX)dateToDate).Border.CornerType = (eCornerType)1;
		dateToDate.EnterToTab = true;
		dateToDate.EscToClose = true;
		dateToDate.FarsyDateStyle = (ENFarsyDateStyle)0;
		dateToDate.FocusBackColor = Color.Yellow;
		dateToDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dateToDate.FocusForeColor = Color.Blue;
		dateToDate.FocusTextSelect = true;
		((Control)dateToDate).Font = new Font("B Titr", 9.75f);
		((Control)dateToDate).Location = new Point(67, 75);
		((Control)dateToDate).Margin = new Padding(2, 3, 2, 3);
		dateToDate.MessageEmptyShowDialog = false;
		dateToDate.MessegeEmpty = "";
		dateToDate.MessegeEmptyInFormRight = true;
		dateToDate.MessegeEmptyShowInForm = false;
		dateToDate.MiladiDate = new DateTime(2025, 6, 28, 23, 50, 43, 872);
		((Control)dateToDate).Name = "dateToDate";
		((TextBoxX)dateToDate).PreventEnterBeep = true;
		((Control)dateToDate).RightToLeft = (RightToLeft)0;
		dateToDate.ShowNameOfDayOnToolTip = false;
		dateToDate.ShowToolTipText = false;
		((Control)dateToDate).Size = new Size(82, 27);
		((Control)dateToDate).TabIndex = 26;
		((Control)dateToDate).Text = "28/06/2025";
		((TextBox)dateToDate).TextAlign = (HorizontalAlignment)2;
		dateToDate.TootTipDelay = 0;
		((Control)label8).AutoSize = true;
		((Control)label8).Location = new Point(150, 32);
		((Control)label8).Margin = new Padding(2, 0, 2, 0);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(131, 13);
		((Control)label8).TabIndex = 20;
		((Control)label8).Text = "تاریخ آخرین بروزرسانی:";
		((Control)label11).AutoSize = true;
		((Control)label11).Location = new Point(150, 82);
		((Control)label11).Margin = new Padding(2, 0, 2, 0);
		((Control)label11).Name = "label11";
		((Control)label11).Size = new Size(47, 13);
		((Control)label11).TabIndex = 25;
		((Control)label11).Text = "تا تاریخ:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ButtonHighlight;
		((Form)this).ClientSize = new Size(646, 550);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("B Titr", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(3, 6, 3, 6);
		((Control)this).MinimumSize = new Size(538, 525);
		((Control)this).Name = "frmAttPersonLeave";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmAttPersonLeave_FormClosed);
		((Form)this).Load += frmAttPersonLeave_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmAttPersonLeave_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)panel4).ResumeLayout(false);
		((Control)panel4).PerformLayout();
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
