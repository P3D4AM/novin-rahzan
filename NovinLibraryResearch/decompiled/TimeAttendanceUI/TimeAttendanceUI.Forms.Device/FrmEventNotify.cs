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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Device;

public class FrmEventNotify : FormExtention
{
	private EventArgsNotify _clsArgsNotify;

	private string frm_Text = "[ویرایش قرارداد]";

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX labelX2;

	internal DateBoxFa TxtDate;

	private LabelX labelX1;

	internal LabelX labelX8;

	private TextBoxX TxtTitle;

	internal LabelX labelX10;

	private TextBoxX TxtDesc;

	private TabItem tabItem1;

	private TabItem tabItem3;

	private TabItem tabItem2;

	private TabItem tabItem4;

	private MaskedTextBoxAdv TxtTime;

	private LabelX labelX3;

	private TextBoxX TxtNotiFyType;

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

	public FrmEventNotify(string contextGuid, int id)
		: base(contextGuid, true, true)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		frm_Text = "[ویرایش قرارداد]";
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(contextGuid, id, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotify.cs");
		FillForm();
	}

	private async Task LoadDataAsync(string contextGuid, int objId, CancellationToken token)
	{
		try
		{
			_clsArgsNotify = await EventArgsNotify.GetAsync(((CacheBaseForm)this).ContextGuid, objId, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FillForm()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((Control)TxtDate).Text = _clsArgsNotify.DateSh;
		((Control)TxtTime).Text = _clsArgsNotify.Time;
		((Control)TxtNotiFyType).Text = Utilities.GetDisplay<NotificationEnum>(_clsArgsNotify.NotificationEnum);
		((Control)TxtTitle).Text = _clsArgsNotify.Title;
		((Control)TxtDesc).Text = _clsArgsNotify.Message;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(AcceptEvent);
	}

	private void AcceptEvent(object sender, EventArgs e)
	{
		try
		{
			((FormExtention)this).ResetControlChanged();
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AcceptEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotify.cs", 80);
		}
	}

	private void FrmEventNotify_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmEventNotify_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotify.cs", 93);
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
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Expected O, but got Unknown
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Expected O, but got Unknown
		//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		TxtTime = new MaskedTextBoxAdv();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX1 = new LabelX();
		TxtDesc = new TextBoxX();
		labelX10 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		TxtNotiFyType = new TextBoxX();
		TxtTitle = new TextBoxX();
		TxtDate = new DateBoxFa();
		labelX8 = new LabelX();
		tabItem1 = new TabItem(components);
		tabItem3 = new TabItem(components);
		tabItem2 = new TabItem(components);
		tabItem4 = new TabItem(components);
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)TxtTime);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)TxtNotiFyType);
		((Control)panelEx1).Controls.Add((Control)(object)TxtTitle);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDate);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(526, 352);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		TxtTime.BackgroundStyle.Class = "TextBoxBorder";
		TxtTime.BackgroundStyle.CornerType = (eCornerType)1;
		TxtTime.ButtonClear.Visible = true;
		((Control)TxtTime).Enabled = false;
		((Control)TxtTime).Location = new Point(13, 12);
		((Control)TxtTime).Name = "TxtTime";
		((Control)TxtTime).Size = new Size(113, 26);
		((PopupItemControl)TxtTime).Style = (eDotNetBarStyle)9;
		((Control)TxtTime).TabIndex = 3;
		((Control)TxtTime).Text = "";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(11, 315);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 11;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو](Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(117, 315);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 10;
		((Control)BtnAccept).Text = "[ذخیره](F5)";
		((Control)BtnAccept).Click += AcceptEvent;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Enabled = false;
		((Control)labelX1).Location = new Point(444, 102);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(70, 23);
		((Control)labelX1).TabIndex = 8;
		((Control)labelX1).Text = "[Message] :";
		labelX1.TextAlignment = (StringAlignment)2;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Enabled = false;
		((Control)TxtDesc).Location = new Point(11, 102);
		((TextBoxBase)TxtDesc).MaxLength = 990000;
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((TextBox)TxtDesc).ScrollBars = (ScrollBars)3;
		((Control)TxtDesc).Size = new Size(427, 200);
		((Control)TxtDesc).TabIndex = 9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.Class = "";
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Enabled = false;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(444, 12);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(70, 26);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "[تاریخ]:";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Enabled = false;
		((Control)labelX3).Location = new Point(444, 41);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(70, 23);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "[نوع اعلان]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Enabled = false;
		((Control)labelX2).Location = new Point(444, 71);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(70, 23);
		((Control)labelX2).TabIndex = 6;
		((Control)labelX2).Text = "[عنوان] :";
		labelX2.TextAlignment = (StringAlignment)2;
		TxtNotiFyType.Border.Class = "TextBoxBorder";
		TxtNotiFyType.Border.CornerType = (eCornerType)1;
		((Control)TxtNotiFyType).Enabled = false;
		((Control)TxtNotiFyType).Location = new Point(12, 44);
		((TextBoxBase)TxtNotiFyType).MaxLength = 100;
		((Control)TxtNotiFyType).Name = "TxtNotiFyType";
		((Control)TxtNotiFyType).Size = new Size(426, 21);
		((Control)TxtNotiFyType).TabIndex = 5;
		TxtTitle.Border.Class = "TextBoxBorder";
		TxtTitle.Border.CornerType = (eCornerType)1;
		((Control)TxtTitle).Enabled = false;
		((Control)TxtTitle).Location = new Point(12, 71);
		((TextBoxBase)TxtTitle).MaxLength = 100;
		((Control)TxtTitle).Name = "TxtTitle";
		((Control)TxtTitle).Size = new Size(426, 21);
		((Control)TxtTitle).TabIndex = 7;
		TxtDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtDate).Border.Class = "";
		((TextBoxX)TxtDate).Border.CornerType = (eCornerType)1;
		((Control)TxtDate).Enabled = false;
		TxtDate.EnterToTab = true;
		TxtDate.EscToClose = true;
		TxtDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtDate.FocusBackColor = Color.Yellow;
		TxtDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtDate.FocusForeColor = Color.Blue;
		TxtDate.FocusTextSelect = true;
		((Control)TxtDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtDate).Location = new Point(301, 12);
		TxtDate.MessageEmptyShowDialog = false;
		TxtDate.MessegeEmpty = "";
		TxtDate.MessegeEmptyInFormRight = true;
		TxtDate.MessegeEmptyShowInForm = false;
		TxtDate.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TxtDate).Name = "TxtDate";
		((Control)TxtDate).RightToLeft = (RightToLeft)0;
		((Control)TxtDate).Size = new Size(136, 26);
		((Control)TxtDate).TabIndex = 1;
		((Control)TxtDate).Text = "1391/12/12";
		((TextBox)TxtDate).TextAlign = (HorizontalAlignment)2;
		TxtDate.TootTipDelay = 500;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Enabled = false;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(132, 12);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(74, 26);
		((Control)labelX8).TabIndex = 2;
		((Control)labelX8).Text = "[زمان] :";
		labelX8.TextAlignment = (StringAlignment)2;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		tabItem3.Name = "tabItem3";
		tabItem3.Text = "[اطلاعات استخدامی]";
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[اطلاعات حقوقی]";
		tabItem4.Name = "tabItem4";
		tabItem4.Text = "[مزایا و کسورات]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(526, 374);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmEventNotify";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[Contratct]*";
		((Form)this).Load += FrmEventNotify_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
