using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Forms.Person;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCTakPerson : UserControl, IHasContext
{
	private bool _Enable;

	private string _CodeM = "";

	private string _CodeT = "";

	public int PID;

	private Person _cls;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private ButtonX BTNMore;

	private LabelX labelX1;

	private TextBoxX TxtcodePerson;

	private LabelX lblname;

	private LabelX labelX2;

	public string ContextGuid { get; private set; }

	public bool EnalbleControl
	{
		get
		{
			_Enable = ucPanel1.Expand;
			return _Enable;
		}
		set
		{
			try
			{
				_Enable = ucPanel1.Expand;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakPerson.cs", 92);
			}
		}
	}

	public void SetContextGuid(string contextGuid)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new InvalidOperationException("contextGuid is null or rmpty!!");
			}
			ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void SetFocus()
	{
		try
		{
			((Control)TxtcodePerson).Focus();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UCTakPerson()
	{
		InitializeComponent();
	}

	public UCTakPerson(string contextGuid)
	{
		ContextGuid = contextGuid;
		InitializeComponent();
		((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
		((Control)panelEx1).Top = 0;
	}

	private async Task LoadPersonDataAsync()
	{
		if (!(_CodeT == "") && !(_CodeT != Text.RemoveNoNumbers(_CodeT, "")))
		{
			string contextGuid = ContextGuid;
			_cls = await Person.GetByPersonIdAsync(contextGuid, await Person.CodeT2IdAsync(ContextGuid, _CodeT));
			((Control)TxtcodePerson).Text = _CodeT;
			LabelX obj = lblname;
			Person cls = _cls;
			((Control)obj).Text = ((cls != null) ? ((PersonBase)cls).FullName : null);
			UCTakPerson uCTakPerson = this;
			Person cls2 = _cls;
			uCTakPerson.PID = ((cls2 != null) ? ((PersonBase)cls2).Id : 0);
		}
	}

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "نام شخص" : "نام شخص-غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakPerson.cs", 72);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void UCTakPerson_Load(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		RefreshForm();
	}

	private async void BTNMore_Click_1(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)TxtcodePerson).Focus();
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid);
			FrmSelectPersonel frm = new FrmSelectPersonel(ContextGuid, isSelect: true, timeAttendanceSettings.FontProgram);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_CodeM = Text.RemoveNoNumbers(frm.selectedCodeM, "");
			_CodeT = Text.RemoveNoNumbers(frm.selectedCodeT, "");
			((Component)(object)frm).Dispose();
			((Form)frm).Close();
			await LoadPersonDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNMore_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakPerson.cs", 128);
		}
	}

	private void TxtNamePerson_Enter(object sender, EventArgs e)
	{
		((Control)TxtcodePerson).BackColor = Color.Yellow;
	}

	private void TxtNamePerson_Leave(object sender, EventArgs e)
	{
		((Control)TxtcodePerson).BackColor = Color.White;
	}

	private async void TxtcodePerson_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtcodePerson).Text = Text.RemoveNoNumbers(((Control)TxtcodePerson).Text, "");
			_CodeT = ((Control)TxtcodePerson).Text;
			await LoadPersonDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtcodePerson_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakPerson.cs", 152);
		}
	}

	public async Task SetCodeAsync(int PID)
	{
		try
		{
			_cls = await Person.GetByPersonIdAsync(ContextGuid, PID);
			((Control)TxtcodePerson).Text = Text.RemoveNoNumbers(((PersonBase)_cls).CodeTafsil.ToString(), "");
			((Control)lblname).Text = ((PersonBase)_cls).FullName;
			PID = ((PersonBase)_cls).Id;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
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
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		panelEx1 = new PanelEx();
		lblname = new LabelX();
		labelX2 = new LabelX();
		BTNMore = new ButtonX();
		labelX1 = new LabelX();
		TxtcodePerson = new TextBoxX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)lblname);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)BTNMore);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TxtcodePerson);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)panelEx1).Location = new Point(0, 31);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Padding = new Padding(3);
		((Control)panelEx1).Size = new Size(600, 33);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)lblname).Anchor = (AnchorStyles)13;
		((BaseItemControl)lblname).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblname).ForeColor = Color.DarkRed;
		((Control)lblname).Location = new Point(6, 6);
		((Control)lblname).Name = "lblname";
		((Control)lblname).RightToLeft = (RightToLeft)1;
		((Control)lblname).Size = new Size(177, 23);
		((Control)lblname).TabIndex = 7;
		((Control)lblname).Text = "----------";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(189, 6);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(94, 23);
		((Control)labelX2).TabIndex = 6;
		((Control)labelX2).Text = "[نام شخص]";
		((Control)BTNMore).AccessibleRole = (AccessibleRole)43;
		((Control)BTNMore).Anchor = (AnchorStyles)9;
		BTNMore.ColorTable = (eButtonColor)6;
		((Control)BTNMore).Location = new Point(299, 7);
		((Control)BTNMore).Name = "BTNMore";
		((Control)BTNMore).Size = new Size(30, 21);
		((PopupItemControl)BTNMore).Style = (eDotNetBarStyle)9;
		((Control)BTNMore).TabIndex = 5;
		((Control)BTNMore).Text = "...";
		((Control)BTNMore).Click += BTNMore_Click_1;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(446, 6);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(111, 23);
		((Control)labelX1).TabIndex = 4;
		((Control)labelX1).Text = "[کد تفصیلی]";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtcodePerson).Anchor = (AnchorStyles)9;
		TxtcodePerson.Border.Class = "TextBoxBorder";
		TxtcodePerson.Border.CornerType = (eCornerType)1;
		((Control)TxtcodePerson).Location = new Point(347, 7);
		((Control)TxtcodePerson).Name = "TxtcodePerson";
		((Control)TxtcodePerson).Size = new Size(78, 21);
		((Control)TxtcodePerson).TabIndex = 3;
		((Control)TxtcodePerson).TextChanged += TxtcodePerson_TextChanged;
		((Control)TxtcodePerson).Enter += TxtNamePerson_Enter;
		((Control)TxtcodePerson).Leave += TxtNamePerson_Leave;
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		((UserControl)ucPanel1).AutoSizeMode = (AutoSizeMode)0;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Margin = new Padding(0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 64);
		((Control)ucPanel1).TabIndex = 0;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).AutoSizeMode = (AutoSizeMode)0;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Margin = new Padding(0);
		((Control)this).MaximumSize = new Size(600, 64);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCTakPerson";
		((Control)this).Size = new Size(600, 64);
		((UserControl)this).Load += UCTakPerson_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
