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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCrtShift1 : FormExtention
{
	private string _frmText;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private RadioButton RBNormalShift;

	private RadioButton RBShenavarShift;

	private RadioButton RBCircleShift;

	private NumericUpDown numCodeShift;

	internal TextBoxX TxtDesc;

	internal LabelX labelX7;

	private TextBoxX TxtName;

	private LabelX labelX1;

	private LabelX labelX5;

	internal LabelX lblDescShiftSelect;

	private PictureBox pictureBox1;

	private PictureBox pictureBox3;

	private PictureBox pictureBox2;

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

	public string StrShiftName => ((Control)TxtName).Text;

	public string StrShiftDesc => ((Control)TxtDesc).Text;

	public string StrShiftCode => ((Control)numCodeShift).Text;

	public TypeShiftEnumType TypeShift
	{
		get
		{
			if (!RBCircleShift.Checked)
			{
				if (RBShenavarShift.Checked)
				{
					return (TypeShiftEnumType)2;
				}
				return (TypeShiftEnumType)1;
			}
			return (TypeShiftEnumType)100;
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
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmCrtShift1(string contextGuid)
		: base(contextGuid, true, true)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_frmText = "[شیفت]";
			InitializeComponent();
			((Control)TxtDesc).Text = "-";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCrtShift1.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCrtShift1.cs", 61);
		}
	}

	public async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			string numCode = (await Shifts.NextCodeAsync(((CacheBaseForm)this).ContextGuid)).ToString();
			string text = Utilities.GetTranslate(_frmText);
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((Control)numCodeShift).Text = numCode;
						((Control)TxtName).Text = text + numCode;
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

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((BaseForm)this).NoCheckFormContentsChanged = true;
			((Control)base.statusStrip1).Visible = false;
			((Control)RBNormalShift).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftNormal;
			((Control)RBShenavarShift).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftShenavar;
			((Control)RBCircleShift).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftCircle;
			base.objsCanEmpty.Add((Control)(object)TxtDesc);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCrtShift1.cs", 106);
		}
	}

	private bool ValidObj()
	{
		try
		{
			if (((Control)TxtDesc).Text.Trim() == "")
			{
				((Control)TxtDesc).Text = "-";
			}
			if (((Control)TxtName).Text.Trim() != "")
			{
				return true;
			}
			Utility.MessageBoxShowError("[برای این نوع شیفت یک عنوان و شرحی انتخاب نمائید]", ((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidObj", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCrtShift1.cs", 135);
		}
		return false;
	}

	private void TxtDesc_Leave(object sender, EventArgs e)
	{
		ValidObj();
	}

	private async void RBNormalShift_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			switch (((Control)(RadioButton)sender).Name)
			{
			case "RBNormalShift":
				((Control)lblDescShiftSelect).Text = Utilities.GetTranslate("[بیشتر شرکت ها شیفت های کاری منظمی دارند به عنوان مثال ساعت کاری از ساعت 8 الی 16 می باشد و یا ساعت کاری به 2 نوبت تقسیم می شود صبح و بعد از ظهر . لذا تعریف شیفت مستلزم معین کردن حداقل دو زمان شروع کار(ورود)\u200c و پایان کار(خروج) است. هر شیفت  می\u200c تواند به یک، دو یا سه نوبت تقسیم شود. مثلا شروع نوبت اول کار پرسنل ساعت 8 پایان نوبت اول 14 و شروع نوبت دوم 16 و پایان نوبت دوم 20 باشد. به این نوع شیفت های کاری شیفت عادی می گوییم]");
				break;
			case "RBShenavarShift":
				((Control)lblDescShiftSelect).Text = Utilities.GetTranslate("[در برخی شرکت ها بعضی از کارکنان ممکن است ساعت شروع کار معینی نداشته باشند و ملاک آنها میزان کارکرد باشد لذا از شیفت های شناور استفاده می شود. بعنوان مثال یک شرکت به کارکنانش میگوید شما می بایست روزانه 8 ساعت کار کنید ساعت شروع و پایان آن مهم نیست بنابراین یکی می تواند از ساعت 8 الی 16 بیاید و دیگری از ساعت 10 الی 18. به این نوع شیفت های کاری شیفت شناور می گوییم.]");
				break;
			case "RBCircleShift":
				((Control)lblDescShiftSelect).Text = Utilities.GetTranslate("[در برخی شرکت ها تعدادی از کارکنان بصورت چرخشی در محل کار حضور دارند.(مانند شیفت نگهبانی). به عنوان مثال یک نگهبان ساعت 8 صبح تا ساعت 8 صبح روز بعد در محل کار حضور دارد و مسئولیت کار خود را به دیگری می سپارد و بعد از 48 ساعت استراحت دوباره شیفت کاری او شروع می شود. به این نوع شیفت های کاری شیفت چرخشی می گوییم.]");
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RBNormalShift_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmCrtShift1.cs", 166);
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
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Expected O, but got Unknown
		new ComponentResourceManager(typeof(FrmCrtShift1));
		PanelEx1 = new PanelEx();
		pictureBox3 = new PictureBox();
		pictureBox2 = new PictureBox();
		pictureBox1 = new PictureBox();
		numCodeShift = new NumericUpDown();
		TxtDesc = new TextBoxX();
		labelX7 = new LabelX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		groupPanel4 = new GroupPanel();
		lblDescShiftSelect = new LabelX();
		RBNormalShift = new RadioButton();
		RBCircleShift = new RadioButton();
		RBShenavarShift = new RadioButton();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox3).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)numCodeShift).BeginInit();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox3);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox2);
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)PanelEx1).Controls.Add((Control)(object)numCodeShift);
		((Control)PanelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX7);
		((Control)PanelEx1).Controls.Add((Control)(object)TxtName);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX1);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX5);
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 478);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		pictureBox3.Image = (Image)(object)Resources.FloatingShift;
		((Control)pictureBox3).Location = new Point(487, 327);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(52, 50);
		pictureBox3.TabIndex = 7;
		pictureBox3.TabStop = false;
		pictureBox2.Image = (Image)(object)Resources.RotationShift;
		((Control)pictureBox2).Location = new Point(487, 392);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(52, 50);
		pictureBox2.TabIndex = 7;
		pictureBox2.TabStop = false;
		pictureBox1.Image = (Image)(object)Resources.NormalShift;
		((Control)pictureBox1).Location = new Point(487, 256);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(52, 50);
		pictureBox1.TabIndex = 7;
		pictureBox1.TabStop = false;
		((Control)numCodeShift).Anchor = (AnchorStyles)9;
		((Control)numCodeShift).Location = new Point(314, 12);
		numCodeShift.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numCodeShift.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numCodeShift).Name = "numCodeShift";
		((Control)numCodeShift).Size = new Size(167, 31);
		((Control)numCodeShift).TabIndex = 1;
		((UpDownBase)numCodeShift).TextAlign = (HorizontalAlignment)2;
		numCodeShift.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TxtDesc).Anchor = (AnchorStyles)13;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(14, 86);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(467, 147);
		((Control)TxtDesc).TabIndex = 4;
		((Control)TxtDesc).Leave += TxtDesc_Leave;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).Location = new Point(488, 86);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(70, 26);
		((Control)labelX7).TabIndex = 3;
		((Control)labelX7).Text = "[شرح] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)TxtName).Anchor = (AnchorStyles)13;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(14, 49);
		((Control)TxtName).Name = "TxtName";
		((Control)TxtName).Size = new Size(467, 31);
		((Control)TxtName).TabIndex = 3;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(488, 46);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(83, 31);
		((Control)labelX1).TabIndex = 2;
		((Control)labelX1).Text = "[نام شیفت] * :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(488, 12);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(80, 31);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[کد شیفت] * :";
		labelX5.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)lblDescShiftSelect);
		((Control)groupPanel4).Controls.Add((Control)(object)RBNormalShift);
		((Control)groupPanel4).Controls.Add((Control)(object)RBCircleShift);
		((Control)groupPanel4).Controls.Add((Control)(object)RBShenavarShift);
		((Control)groupPanel4).Location = new Point(14, 239);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(467, 210);
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
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "[نوع شیفت را مشخص کنید]";
		((Control)lblDescShiftSelect).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblDescShiftSelect).BackgroundStyle.Class = "";
		((BaseItemControl)lblDescShiftSelect).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblDescShiftSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblDescShiftSelect).Location = new Point(19, -1);
		((Control)lblDescShiftSelect).Name = "lblDescShiftSelect";
		((Control)lblDescShiftSelect).Size = new Size(311, 164);
		((Control)lblDescShiftSelect).TabIndex = 7;
		((Control)lblDescShiftSelect).Text = "[بیشتر شرکت ها شیفت های کاری منظمی دارند به عنوان مثال ساعت کاری از ساعت 8 الی 16 می باشد و یا ساعت کاری به 2 نوبت تقسیم می شود صبح و بعد از ظهر . لذا تعریف شیفت مستلزم معین کردن حداقل دو زمان شروع کار(ورود)\u200c و پایان کار(خروج) است. هر شیفت  می\u200c تواند به یک، دو یا سه نوبت تقسیم شود. مثلا شروع نوبت اول کار پرسنل ساعت 8 پایان نوبت اول 14 و شروع نوبت دوم 16 و پایان نوبت دوم 20 باشد. به این نوع شیفت های کاری شیفت عادی می گوییم]";
		lblDescShiftSelect.WordWrap = true;
		RBNormalShift.Checked = true;
		((Control)RBNormalShift).Location = new Point(337, -1);
		((Control)RBNormalShift).Name = "RBNormalShift";
		((Control)RBNormalShift).Size = new Size(108, 27);
		((Control)RBNormalShift).TabIndex = 0;
		RBNormalShift.TabStop = true;
		((Control)RBNormalShift).Text = "شیفت عادی";
		((ButtonBase)RBNormalShift).UseVisualStyleBackColor = true;
		RBNormalShift.CheckedChanged += RBNormalShift_CheckedChanged;
		((Control)RBCircleShift).Location = new Point(337, 136);
		((Control)RBCircleShift).Name = "RBCircleShift";
		((Control)RBCircleShift).Size = new Size(108, 27);
		((Control)RBCircleShift).TabIndex = 2;
		((Control)RBCircleShift).Text = "[شیفت چرخشی]";
		((ButtonBase)RBCircleShift).UseVisualStyleBackColor = true;
		RBCircleShift.CheckedChanged += RBNormalShift_CheckedChanged;
		((Control)RBShenavarShift).Location = new Point(337, 70);
		((Control)RBShenavarShift).Name = "RBShenavarShift";
		((Control)RBShenavarShift).Size = new Size(108, 27);
		((Control)RBShenavarShift).TabIndex = 1;
		((Control)RBShenavarShift).Text = "[شیفت شناور]";
		((ButtonBase)RBShenavarShift).UseVisualStyleBackColor = true;
		RBShenavarShift.CheckedChanged += RBNormalShift_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCrtShift1";
		((Control)this).Text = "FRMCrtShift1";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox3).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)numCodeShift).EndInit();
		((Control)groupPanel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
