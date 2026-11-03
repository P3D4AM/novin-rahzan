using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PayamakSettings;
using PayamakUi.Classes;
using PayamakUi.Forms.FRMOthers;
using PayamakUi.Forms.Reports;
using PayamakUi.Properties;
using PayamakUi.UC;
using PayamakUi.UC.Others;
using PayamakUi.UC.Templates;
using PayamakUi.UC.UCSMS;

namespace PayamakUi;

public class FrmPayamakMain : CacheBaseForm
{
	public enum Enuc
	{
		Inbox = 1,
		OutBox,
		SendItems,
		PhoneBook,
		Draft,
		BlackList,
		SendNew,
		BulkSent
	}

	private CancellationTokenSource TokenPaymak = new CancellationTokenSource();

	private CancellationTokenSource TokenLoadComponent = new CancellationTokenSource();

	private Enuc _type;

	private CLSMain _cls;

	private bool IsConnectionStringOk = true;

	private static CancellationTokenSource _checkInboxSemaphore;

	private IContainer components;

	private GalleryContainer GalleryContainer2;

	private LabelItem LabelItem1;

	private ControlContainerItem ControlContainerItem1;

	private ControlContainerItem controlContair;

	private UCOutBox UcOutBox1;

	private UCDraft UcDraft1;

	private Panel Panel1;

	private SideBarPanelItem SideBarPanelItem1;

	private ButtonItem ButtonItem1;

	private ButtonItem ButtonItem2;

	private ButtonItem ButtonItem3;

	private ButtonItem ButtonItem4;

	private ButtonItem ButtonItem5;

	private ButtonItem ButtonItem6;

	private ButtonItem ButtonItem7;

	private ButtonItem ButtonItem8;

	private ButtonItem ButtonItem9;

	private ButtonItem ButtonItem10;

	private ButtonItem ButtonItem11;

	private UCStatuseBar UcStatuseBar1;

	private UCTemplateList UcTemplateList1;

	private ControlContainerItem ControlContainerItem2;

	private RibbonControl RibbonControl1;

	private ButtonItem BTNBlackList;

	private RibbonPanel RibbonPanel1;

	private RibbonBar RibbonBar1;

	private ButtonItem BTNSendMessage;

	private ButtonItem BtnBulkSent;

	private RibbonPanel RibbonPanel5;

	private RibbonBar RibbonBar5;

	private ButtonItem BTNAbout;

	private RibbonPanel RibbonPanel4;

	private RibbonBar RibbonBar4;

	private ButtonItem BTNSettings;

	private RibbonTabItem RibbonTabItem1;

	private RibbonTabItem RibbonTabItem4;

	private RibbonTabItem RibbonTabItem5;

	private RibbonPanel RibbonPanel3;

	private RibbonTabItem RibbonTabItem3;

	private RibbonBar RibbonBar3;

	private ButtonItem BtnSentReport;

	private UCTele UcTele1;

	private UCsentList UCsent_List;

	private UCSend UCSendMessage;

	private TreeView treeView1;

	private ButtonItem btnSendGroupMessage;

	public FrmPayamakMain(string contextGuid, Enuc type)
		: base(contextGuid)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		FrmPayamakMain frmPayamakMain = this;
		_type = type;
		_cls = null;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMMain_FormClosed);
		((Form)this).FormClosing += new FormClosingEventHandler(FRMMain_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(FRMMain_KeyDown);
		((Control)this).KeyPress += new KeyPressEventHandler(FRMMain_KeyPress);
		((Form)this).Load += FRMMain_Load;
		try
		{
			InitializeComponent();
			CancellationToken token = new CancellationTokenSource(60000).Token;
			UcTele1.ContextGuid = contextGuid;
			UCsent_List.ContextGuid = contextGuid;
			UCSendMessage.ContextGuid = contextGuid;
			UcDraft1.ContextGuid = contextGuid;
			UcTemplateList1.ContextGuid = contextGuid;
			UcOutBox1.ContextGuid = contextGuid;
			UcStatuseBar1.ContextGuid = contextGuid;
			Task.Run(() => frmPayamakMain.UcStatuseBar1.InitAsync(contextGuid, token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 56);
		}
	}

	public FrmPayamakMain(string contextGuid, bool cnStrIsOk)
		: base(contextGuid)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		FrmPayamakMain frmPayamakMain = this;
		_cls = null;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMMain_FormClosed);
		((Form)this).FormClosing += new FormClosingEventHandler(FRMMain_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(FRMMain_KeyDown);
		((Control)this).KeyPress += new KeyPressEventHandler(FRMMain_KeyPress);
		((Form)this).Load += FRMMain_Load;
		try
		{
			IsConnectionStringOk = cnStrIsOk;
			InitializeComponent();
			CancellationToken token = new CancellationTokenSource(60000).Token;
			UcTele1.ContextGuid = contextGuid;
			UCsent_List.ContextGuid = contextGuid;
			UCSendMessage.ContextGuid = contextGuid;
			UcDraft1.ContextGuid = contextGuid;
			UcTemplateList1.ContextGuid = contextGuid;
			UcOutBox1.ContextGuid = contextGuid;
			UcStatuseBar1.ContextGuid = contextGuid;
			Task.Run(() => frmPayamakMain.UcStatuseBar1.InitAsync(contextGuid, token));
			HideAllAsync(null);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 82);
		}
	}

	public FrmPayamakMain(string contextGuid)
		: base(contextGuid)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		FrmPayamakMain frmPayamakMain = this;
		_cls = null;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMMain_FormClosed);
		((Form)this).FormClosing += new FormClosingEventHandler(FRMMain_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(FRMMain_KeyDown);
		((Control)this).KeyPress += new KeyPressEventHandler(FRMMain_KeyPress);
		((Form)this).Load += FRMMain_Load;
		try
		{
			InitializeComponent();
			CancellationToken token = new CancellationTokenSource(60000).Token;
			Task.Run(() => frmPayamakMain.CheckInboxOutboxSending(token));
			Task.Run(() => frmPayamakMain.UcStatuseBar1.InitAsync(contextGuid, token));
			UcTele1.ContextGuid = contextGuid;
			UCsent_List.ContextGuid = contextGuid;
			UCSendMessage.ContextGuid = contextGuid;
			UcDraft1.ContextGuid = contextGuid;
			UcTemplateList1.ContextGuid = contextGuid;
			UcOutBox1.ContextGuid = contextGuid;
			UcStatuseBar1.ContextGuid = contextGuid;
			HideAllAsync(null);
			((ToolStripItem)UcStatuseBar1.TssDate).Text = DateTime.Now.MiladiToShamsi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 113);
		}
	}

	private void FRMMain_FormClosed(object sender, FormClosedEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			((Form)this).FormClosed -= new FormClosedEventHandler(FRMMain_FormClosed);
			Process.StopEngine();
			Process.ExitPayamak();
			TokenPaymak?.Cancel();
			TokenPaymak.Dispose();
			TokenLoadComponent?.Cancel();
			TokenLoadComponent.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMMain_FormClosed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 146);
		}
	}

	private void FRMMain_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			((CancelEventArgs)(object)e).Cancel = true;
			((Control)this).Hide();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMMain_FormClosing", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 156);
		}
	}

	private async void FRMMain_KeyDown(object sender, KeyEventArgs e)
	{
		_ = 1;
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 112)
			{
				BTNSettings_Click(sender, (EventArgs)(object)e);
			}
			else if ((int)keyCode == 113)
			{
				if (!((Control)UCSendMessage).Visible)
				{
					BTNSendMessage_Click(sender, (EventArgs)(object)e);
				}
			}
			else if ((int)keyCode == 119)
			{
				FRMFilter fRMFilter = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTInbox);
				((Form)fRMFilter).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter).Dispose();
			}
			else if ((int)keyCode == 120)
			{
				FRMFilter fRMFilter2 = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTOutBox);
				((Form)fRMFilter2).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter2).Dispose();
			}
			else if ((int)keyCode == 121)
			{
				FRMFilter fRMFilter3 = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTSentBox);
				((Form)fRMFilter3).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter3).Dispose();
			}
			else if ((int)keyCode == 122)
			{
				await HideAllAsync(UcDraft1);
			}
			else if ((int)keyCode == 123)
			{
				BTNSettings_Click(sender, (EventArgs)(object)e);
			}
			else if ((int)keyCode == 27)
			{
				await HideAllAsync(null);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 204);
		}
	}

	private void FRMMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar) && ((Control)UcDraft1).Visible)
			{
				UcDraft1.Search_Press(e.KeyChar);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMMain_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 215);
		}
	}

	private async void FRMMain_Load(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await Font.GetFontProgramAsync(default(CancellationToken));
			((ToolStripItem)UcStatuseBar1.TssDate).Text = DateTime.Now.MiladiToCalture();
			switch (_type)
			{
			case Enuc.OutBox:
			{
				FRMFilter fRMFilter2 = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTOutBox);
				((Form)fRMFilter2).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter2).Dispose();
				break;
			}
			case Enuc.SendItems:
			{
				FRMFilter fRMFilter = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTSentBox);
				((Form)fRMFilter).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter).Dispose();
				break;
			}
			default:
				await HideAllAsync(null);
				break;
			}
			LanguageAndCalendar.Instance.ChangeLanguage(LanguageTypes.Farsi, "", translateUi: true, ENSource.PayamakUI);
			if (!IsConnectionStringOk)
			{
				BTNSettings_Click(sender, e);
				return;
			}
			Process.StartGSMEngine();
			await HideAllAsync(UCsent_List);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 279);
			ret.AddReturnedValue(ex, "", "FRMMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 280);
		}
		finally
		{
			if (ret.HasError)
			{
				string title = "PayamakMain.Program.Main";
				((Form)new FrmShowErrorMessage(base.ContextGuid, ret, title, (MessageBoxButtons)4, isAskQuestion: false, LanguageTypes.Farsi, "FRMMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs")).ShowDialog((IWin32Window)(object)(base.IsDisposed ? null : this));
			}
		}
	}

	private async Task HideAllAsync(object except)
	{
		try
		{
			TokenLoadComponent?.Cancel();
			TokenLoadComponent = new CancellationTokenSource(60000);
			CancellationToken tkn = TokenLoadComponent.Token;
			if (except != null)
			{
				((Control)Panel1).Controls.Add((Control)(UserControl)except);
				((Control)(UserControl)except).Dock = (DockStyle)5;
				((Control)(UserControl)except).Show();
				((Control)(UserControl)except).BringToFront();
				((Control)(UserControl)except).Visible = true;
				((Control)(UserControl)except).Focus();
			}
			if (except is UCSend)
			{
				await UCSendMessage.ResetFormAsync();
			}
			else
			{
				((Control)UCSendMessage).Hide();
				((Control)UCSendMessage).Visible = false;
			}
			if (except is UCsentList)
			{
				await UCsent_List.LoadDataAsync();
			}
			else
			{
				((Control)UCsent_List).Hide();
				((Control)UCsent_List).Visible = false;
			}
			if (except is UCDraft)
			{
				await UcDraft1.LoadDataAsync();
			}
			else
			{
				((Control)UcDraft1).Hide();
				((Control)UcDraft1).Visible = false;
			}
			if (except is UCTemplateList)
			{
				Task.Run(() => UcTemplateList1.LoadDataAsync(tkn));
			}
			else
			{
				((Control)UcTemplateList1).Hide();
				((Control)UcTemplateList1).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HideAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 342);
		}
	}

	private void BTNSettings_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			FrmSetting frmSetting = new FrmSetting(base.ContextGuid);
			((Form)frmSetting).ShowDialog((IWin32Window)(object)this);
			((Component)(object)frmSetting).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNSettings_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 356);
		}
	}

	private async void BTNSendMessage_Click(object sender, EventArgs e)
	{
		try
		{
			await HideAllAsync(UCSendMessage);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNSendMessage_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 362);
		}
	}

	public async Task CheckInboxOutboxSending(CancellationToken inputToken)
	{
		_ = 1;
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (base.IsDisposed || inputToken.IsCancellationRequested)
				{
					return;
				}
			}
			if (!base.IsDisposed && !inputToken.IsCancellationRequested)
			{
				_checkInboxSemaphore?.Cancel();
				_checkInboxSemaphore = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { inputToken });
				CancellationToken token = _checkInboxSemaphore.Token;
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(10000);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckInboxOutboxSending", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 417);
		}
	}

	private void btnExit_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private async void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		try
		{
			switch (e.Node.Name)
			{
			case "NdInbox":
			{
				FRMFilter fRMFilter3 = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTInbox);
				((Form)fRMFilter3).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter3).Dispose();
				break;
			}
			case "NdOutBox":
			{
				FRMFilter fRMFilter2 = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTOutBox);
				((Form)fRMFilter2).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter2).Dispose();
				break;
			}
			case "NdSend":
			{
				FRMFilter fRMFilter = new FRMFilter(base.ContextGuid, FRMFilter.ReportType.RPTSentBox);
				((Form)fRMFilter).ShowDialog((IWin32Window)(object)this);
				((Component)(object)fRMFilter).Dispose();
				break;
			}
			default:
				await HideAllAsync(null);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "treeView1_NodeMouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 461);
		}
	}

	private void buttonItem13_Click(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Form)new frmSendMassage(base.ContextGuid)).ShowDialog();
	}

	private async void BtnSentReport_Click(object sender, EventArgs e)
	{
		try
		{
			await HideAllAsync(UCsent_List);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSentReport_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 476);
		}
	}

	private void BTNAbout_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)new FRMAbout()).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNAbout_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FrmPayamakMain.cs", 487);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b1: Expected O, but got Unknown
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a89: Expected O, but got Unknown
		//IL_1c23: Unknown result type (might be due to invalid IL or missing references)
		TreeNode val = new TreeNode("دریافتی ها");
		TreeNode val2 = new TreeNode("در حال ارسال");
		TreeNode val3 = new TreeNode("ارسال شده ها");
		GalleryContainer2 = new GalleryContainer();
		LabelItem1 = new LabelItem();
		ControlContainerItem1 = new ControlContainerItem();
		controlContair = new ControlContainerItem();
		SideBarPanelItem1 = new SideBarPanelItem();
		ButtonItem1 = new ButtonItem();
		ButtonItem2 = new ButtonItem();
		ButtonItem3 = new ButtonItem();
		ButtonItem4 = new ButtonItem();
		ButtonItem5 = new ButtonItem();
		ButtonItem6 = new ButtonItem();
		ButtonItem7 = new ButtonItem();
		ButtonItem8 = new ButtonItem();
		ButtonItem9 = new ButtonItem();
		ButtonItem10 = new ButtonItem();
		ButtonItem11 = new ButtonItem();
		ControlContainerItem2 = new ControlContainerItem();
		RibbonControl1 = new RibbonControl();
		RibbonPanel3 = new RibbonPanel();
		RibbonBar3 = new RibbonBar();
		BtnSentReport = new ButtonItem();
		RibbonPanel1 = new RibbonPanel();
		RibbonBar1 = new RibbonBar();
		BTNSendMessage = new ButtonItem();
		BtnBulkSent = new ButtonItem();
		btnSendGroupMessage = new ButtonItem();
		RibbonPanel4 = new RibbonPanel();
		RibbonBar4 = new RibbonBar();
		BTNSettings = new ButtonItem();
		RibbonPanel5 = new RibbonPanel();
		RibbonBar5 = new RibbonBar();
		BTNAbout = new ButtonItem();
		RibbonTabItem1 = new RibbonTabItem();
		RibbonTabItem3 = new RibbonTabItem();
		RibbonTabItem4 = new RibbonTabItem();
		RibbonTabItem5 = new RibbonTabItem();
		Panel1 = new Panel();
		UcTele1 = new UCTele();
		UCsent_List = new UCsentList();
		UCSendMessage = new UCSend();
		UcDraft1 = new UCDraft();
		UcTemplateList1 = new UCTemplateList();
		BTNBlackList = new ButtonItem();
		treeView1 = new TreeView();
		UcStatuseBar1 = new UCStatuseBar();
		UcOutBox1 = new UCOutBox();
		RibbonControl1.SuspendLayout();
		((Control)RibbonPanel3).SuspendLayout();
		((Control)RibbonPanel1).SuspendLayout();
		((Control)RibbonPanel4).SuspendLayout();
		((Control)RibbonPanel5).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		GalleryContainer2.BackgroundStyle.Class = "RibbonFileMenuColumnTwoContainer";
		GalleryContainer2.BackgroundStyle.CornerType = eCornerType.Square;
		GalleryContainer2.EnableGalleryPopup = false;
		GalleryContainer2.LayoutOrientation = eOrientation.Vertical;
		GalleryContainer2.MinimumSize = new Size(180, 240);
		GalleryContainer2.MultiLine = false;
		GalleryContainer2.Name = "GalleryContainer2";
		GalleryContainer2.PopupUsesStandardScrollbars = false;
		GalleryContainer2.TitleMouseOverStyle.CornerType = eCornerType.Square;
		GalleryContainer2.TitleStyle.CornerType = eCornerType.Square;
		LabelItem1.BorderSide = eBorderSide.Bottom;
		LabelItem1.BorderType = eBorderType.Etched;
		LabelItem1.CanCustomize = false;
		LabelItem1.Name = "LabelItem1";
		ControlContainerItem1.AllowItemResize = true;
		ControlContainerItem1.MenuVisibility = eMenuVisibility.VisibleAlways;
		ControlContainerItem1.Name = "ControlContainerItem1";
		controlContair.AllowItemResize = true;
		controlContair.MenuVisibility = eMenuVisibility.VisibleAlways;
		controlContair.Name = "controlContair";
		SideBarPanelItem1.FontBold = true;
		SideBarPanelItem1.Name = "SideBarPanelItem1";
		SideBarPanelItem1.SubItems.AddRange(new BaseItem[11]
		{
			ButtonItem1, ButtonItem2, ButtonItem3, ButtonItem4, ButtonItem5, ButtonItem6, ButtonItem7, ButtonItem8, ButtonItem9, ButtonItem10,
			ButtonItem11
		});
		SideBarPanelItem1.Text = "دفتـــــــر چه تـــــلفن";
		ButtonItem1.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem1.FixedSize = new Size(200, 0);
		ButtonItem1.ImagePaddingHorizontal = 20;
		ButtonItem1.ImagePaddingVertical = 20;
		ButtonItem1.ImagePosition = eImagePosition.Right;
		ButtonItem1.Name = "ButtonItem1";
		ButtonItem1.Text = "دفتــــــرجه تــــــلفن";
		ButtonItem2.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem2.FixedSize = new Size(200, 0);
		ButtonItem2.ImagePaddingHorizontal = 20;
		ButtonItem2.ImagePaddingVertical = 20;
		ButtonItem2.ImagePosition = eImagePosition.Right;
		ButtonItem2.Name = "ButtonItem2";
		ButtonItem2.Text = "پیش نــویس پـــــــیام";
		ButtonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem3.FixedSize = new Size(200, 0);
		ButtonItem3.ImagePaddingHorizontal = 20;
		ButtonItem3.ImagePaddingVertical = 20;
		ButtonItem3.ImagePosition = eImagePosition.Right;
		ButtonItem3.Name = "ButtonItem3";
		ButtonItem3.Text = "الگــــو های موجـــود";
		ButtonItem4.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem4.FixedSize = new Size(200, 0);
		ButtonItem4.ImagePaddingHorizontal = 20;
		ButtonItem4.ImagePaddingVertical = 20;
		ButtonItem4.ImagePosition = eImagePosition.Right;
		ButtonItem4.Name = "ButtonItem4";
		ButtonItem4.Text = "ارســـــــــال شده ها";
		ButtonItem5.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem5.FixedSize = new Size(200, 0);
		ButtonItem5.ImagePaddingHorizontal = 20;
		ButtonItem5.ImagePaddingVertical = 20;
		ButtonItem5.ImagePosition = eImagePosition.Right;
		ButtonItem5.Name = "ButtonItem5";
		ButtonItem5.Text = "صــــــــــندوق خروجی";
		ButtonItem6.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem6.FixedSize = new Size(200, 0);
		ButtonItem6.ImagePaddingHorizontal = 20;
		ButtonItem6.ImagePaddingVertical = 20;
		ButtonItem6.ImagePosition = eImagePosition.Right;
		ButtonItem6.Name = "ButtonItem6";
		ButtonItem6.Text = "ثــــبت مخــــــاطبین";
		ButtonItem7.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem7.FixedSize = new Size(200, 0);
		ButtonItem7.ImagePaddingHorizontal = 20;
		ButtonItem7.ImagePaddingVertical = 20;
		ButtonItem7.ImagePosition = eImagePosition.Right;
		ButtonItem7.Name = "ButtonItem7";
		ButtonItem7.Text = "لیســــــت ســــــیاه";
		ButtonItem8.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem8.FixedSize = new Size(200, 0);
		ButtonItem8.ImagePaddingHorizontal = 20;
		ButtonItem8.ImagePaddingVertical = 20;
		ButtonItem8.ImagePosition = eImagePosition.Right;
		ButtonItem8.Name = "ButtonItem8";
		ButtonItem8.Text = "صـــــــــــندوق ورودی";
		ButtonItem9.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem9.FixedSize = new Size(200, 0);
		ButtonItem9.ImagePaddingHorizontal = 20;
		ButtonItem9.ImagePaddingVertical = 20;
		ButtonItem9.ImagePosition = eImagePosition.Right;
		ButtonItem9.Name = "ButtonItem9";
		ButtonItem9.Text = "نـوشتن پــــــیام جــدید";
		ButtonItem10.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem10.FixedSize = new Size(200, 0);
		ButtonItem10.ImagePaddingHorizontal = 20;
		ButtonItem10.ImagePaddingVertical = 20;
		ButtonItem10.ImagePosition = eImagePosition.Right;
		ButtonItem10.Name = "ButtonItem10";
		ButtonItem10.Text = "ارسال هوشمند";
		ButtonItem11.ButtonStyle = eButtonStyle.ImageAndText;
		ButtonItem11.ImagePaddingHorizontal = 20;
		ButtonItem11.ImagePaddingVertical = 20;
		ButtonItem11.ImagePosition = eImagePosition.Right;
		ButtonItem11.Name = "ButtonItem11";
		ButtonItem11.Text = "نظـــر ســــــنجی";
		ControlContainerItem2.AllowItemResize = true;
		ControlContainerItem2.MenuVisibility = eMenuVisibility.VisibleAlways;
		ControlContainerItem2.Name = "ControlContainerItem2";
		((Control)RibbonControl1).Anchor = (AnchorStyles)13;
		RibbonControl1.BackgroundStyle.CornerType = eCornerType.Square;
		RibbonControl1.CaptionVisible = true;
		((Control)RibbonControl1).Controls.Add((Control)(object)RibbonPanel1);
		((Control)RibbonControl1).Controls.Add((Control)(object)RibbonPanel3);
		((Control)RibbonControl1).Controls.Add((Control)(object)RibbonPanel4);
		((Control)RibbonControl1).Controls.Add((Control)(object)RibbonPanel5);
		RibbonControl1.Items.AddRange(new BaseItem[4] { RibbonTabItem1, RibbonTabItem3, RibbonTabItem4, RibbonTabItem5 });
		RibbonControl1.KeyTipsFont = new Font("Tahoma", 7f);
		((Control)RibbonControl1).Location = new Point(0, -28);
		((Control)RibbonControl1).Name = "RibbonControl1";
		((Control)RibbonControl1).Padding = new Padding(0, 0, 0, 3);
		((Control)RibbonControl1).RightToLeft = (RightToLeft)1;
		((Control)RibbonControl1).Size = new Size(859, 163);
		RibbonControl1.Style = eDotNetBarStyle.StyleManagerControlled;
		RibbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
		RibbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
		RibbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
		RibbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
		RibbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
		RibbonControl1.SystemText.QatDialogAddButton = "&Add >>";
		RibbonControl1.SystemText.QatDialogCancelButton = "Cancel";
		RibbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
		RibbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
		RibbonControl1.SystemText.QatDialogOkButton = "OK";
		RibbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
		RibbonControl1.SystemText.QatDialogRemoveButton = "&Remove";
		RibbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
		RibbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
		RibbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
		RibbonControl1.TabGroupHeight = 14;
		((Control)RibbonControl1).TabIndex = 25;
		((Control)RibbonControl1).Text = "RibbonControl1";
		RibbonPanel3.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonPanel3).Controls.Add((Control)(object)RibbonBar3);
		((Control)RibbonPanel3).Dock = (DockStyle)5;
		RibbonPanel3.Location = new Point(0, 56);
		((Control)RibbonPanel3).Name = "RibbonPanel3";
		((Control)RibbonPanel3).Padding = new Padding(3, 0, 3, 3);
		RibbonPanel3.Size = new Size(859, 104);
		RibbonPanel3.Style.CornerType = eCornerType.Square;
		RibbonPanel3.StyleMouseDown.CornerType = eCornerType.Square;
		RibbonPanel3.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)RibbonPanel3).TabIndex = 7;
		RibbonPanel3.Visible = false;
		RibbonBar3.AutoOverflowEnabled = true;
		RibbonBar3.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		RibbonBar3.BackgroundStyle.CornerType = eCornerType.Square;
		RibbonBar3.ContainerControlProcessDialogKey = true;
		((Control)RibbonBar3).Dock = (DockStyle)5;
		RibbonBar3.DragDropSupport = true;
		RibbonBar3.Items.AddRange(new BaseItem[1] { BtnSentReport });
		((Control)RibbonBar3).Location = new Point(3, 0);
		((Control)RibbonBar3).Name = "RibbonBar3";
		RibbonBar3.ShowToolTips = false;
		((Control)RibbonBar3).Size = new Size(853, 101);
		RibbonBar3.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonBar3).TabIndex = 1;
		RibbonBar3.TitleStyle.CornerType = eCornerType.Square;
		RibbonBar3.TitleStyleMouseOver.CornerType = eCornerType.Square;
		RibbonBar3.TitleVisible = false;
		BtnSentReport.Image = (Image)(object)Resources.bulk_report1;
		BtnSentReport.ImagePaddingHorizontal = 25;
		BtnSentReport.ImagePosition = eImagePosition.Top;
		BtnSentReport.Name = "BtnSentReport";
		BtnSentReport.SubItemsExpandWidth = 14;
		BtnSentReport.Text = "گزارش ارسال شده ها";
		BtnSentReport.Click += BtnSentReport_Click;
		RibbonPanel1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonPanel1).Controls.Add((Control)(object)RibbonBar1);
		((Control)RibbonPanel1).Dock = (DockStyle)5;
		RibbonPanel1.Location = new Point(0, 56);
		((Control)RibbonPanel1).Name = "RibbonPanel1";
		((Control)RibbonPanel1).Padding = new Padding(3, 0, 3, 3);
		RibbonPanel1.Size = new Size(859, 104);
		RibbonPanel1.Style.CornerType = eCornerType.Square;
		RibbonPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		RibbonPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)RibbonPanel1).TabIndex = 1;
		RibbonBar1.AutoOverflowEnabled = true;
		RibbonBar1.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		RibbonBar1.BackgroundStyle.CornerType = eCornerType.Square;
		RibbonBar1.ContainerControlProcessDialogKey = true;
		((Control)RibbonBar1).Dock = (DockStyle)5;
		RibbonBar1.DragDropSupport = true;
		RibbonBar1.Items.AddRange(new BaseItem[3] { BTNSendMessage, BtnBulkSent, btnSendGroupMessage });
		((Control)RibbonBar1).Location = new Point(3, 0);
		((Control)RibbonBar1).Name = "RibbonBar1";
		RibbonBar1.ShowToolTips = false;
		((Control)RibbonBar1).Size = new Size(853, 101);
		RibbonBar1.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonBar1).TabIndex = 0;
		RibbonBar1.TitleStyle.CornerType = eCornerType.Square;
		RibbonBar1.TitleStyleMouseOver.CornerType = eCornerType.Square;
		RibbonBar1.TitleVisible = false;
		BTNSendMessage.ImagePaddingHorizontal = 40;
		BTNSendMessage.ImagePosition = eImagePosition.Top;
		BTNSendMessage.Name = "BTNSendMessage";
		BTNSendMessage.SubItemsExpandWidth = 14;
		BTNSendMessage.Text = "ارسال پیام";
		BTNSendMessage.Click += BTNSendMessage_Click;
		BtnBulkSent.ImagePaddingHorizontal = 50;
		BtnBulkSent.ImagePosition = eImagePosition.Top;
		BtnBulkSent.Name = "BtnBulkSent";
		BtnBulkSent.SubItemsExpandWidth = 14;
		BtnBulkSent.Text = "ارسال انبوه";
		BtnBulkSent.Visible = false;
		BtnBulkSent.Click += BTNSendMessage_Click;
		btnSendGroupMessage.ImagePaddingHorizontal = 50;
		btnSendGroupMessage.ImagePosition = eImagePosition.Top;
		btnSendGroupMessage.Name = "btnSendGroupMessage";
		btnSendGroupMessage.SubItemsExpandWidth = 14;
		btnSendGroupMessage.Text = "ارسال پیامک گروهی";
		btnSendGroupMessage.Click += buttonItem13_Click;
		RibbonPanel4.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonPanel4).Controls.Add((Control)(object)RibbonBar4);
		((Control)RibbonPanel4).Dock = (DockStyle)5;
		RibbonPanel4.Location = new Point(0, 56);
		((Control)RibbonPanel4).Name = "RibbonPanel4";
		((Control)RibbonPanel4).Padding = new Padding(3, 0, 3, 3);
		RibbonPanel4.Size = new Size(859, 104);
		RibbonPanel4.Style.CornerType = eCornerType.Square;
		RibbonPanel4.StyleMouseDown.CornerType = eCornerType.Square;
		RibbonPanel4.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)RibbonPanel4).TabIndex = 4;
		RibbonPanel4.Visible = false;
		RibbonBar4.AutoOverflowEnabled = true;
		RibbonBar4.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		RibbonBar4.BackgroundStyle.CornerType = eCornerType.Square;
		RibbonBar4.ContainerControlProcessDialogKey = true;
		((Control)RibbonBar4).Dock = (DockStyle)5;
		RibbonBar4.DragDropSupport = true;
		RibbonBar4.Items.AddRange(new BaseItem[1] { BTNSettings });
		((Control)RibbonBar4).Location = new Point(3, 0);
		((Control)RibbonBar4).Name = "RibbonBar4";
		RibbonBar4.ShowToolTips = false;
		((Control)RibbonBar4).Size = new Size(853, 101);
		RibbonBar4.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonBar4).TabIndex = 2;
		RibbonBar4.TitleStyle.CornerType = eCornerType.Square;
		RibbonBar4.TitleStyleMouseOver.CornerType = eCornerType.Square;
		RibbonBar4.TitleVisible = false;
		BTNSettings.ImagePaddingHorizontal = 40;
		BTNSettings.ImagePosition = eImagePosition.Top;
		BTNSettings.Name = "BTNSettings";
		BTNSettings.SubItemsExpandWidth = 14;
		BTNSettings.Text = "تنظیمات";
		BTNSettings.Click += BTNSettings_Click;
		RibbonPanel5.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonPanel5).Controls.Add((Control)(object)RibbonBar5);
		((Control)RibbonPanel5).Dock = (DockStyle)5;
		RibbonPanel5.Location = new Point(0, 56);
		((Control)RibbonPanel5).Name = "RibbonPanel5";
		((Control)RibbonPanel5).Padding = new Padding(3, 0, 3, 3);
		RibbonPanel5.Size = new Size(859, 104);
		RibbonPanel5.Style.CornerType = eCornerType.Square;
		RibbonPanel5.StyleMouseDown.CornerType = eCornerType.Square;
		RibbonPanel5.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)RibbonPanel5).TabIndex = 5;
		RibbonPanel5.Visible = false;
		RibbonBar5.AutoOverflowEnabled = true;
		RibbonBar5.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		RibbonBar5.BackgroundStyle.CornerType = eCornerType.Square;
		RibbonBar5.ContainerControlProcessDialogKey = true;
		((Control)RibbonBar5).Dock = (DockStyle)5;
		RibbonBar5.DragDropSupport = true;
		RibbonBar5.Items.AddRange(new BaseItem[1] { BTNAbout });
		((Control)RibbonBar5).Location = new Point(3, 0);
		((Control)RibbonBar5).Name = "RibbonBar5";
		RibbonBar5.ShowToolTips = false;
		((Control)RibbonBar5).Size = new Size(853, 101);
		RibbonBar5.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)RibbonBar5).TabIndex = 2;
		RibbonBar5.TitleStyle.CornerType = eCornerType.Square;
		RibbonBar5.TitleStyleMouseOver.CornerType = eCornerType.Square;
		RibbonBar5.TitleVisible = false;
		BTNAbout.ImagePosition = eImagePosition.Top;
		BTNAbout.Name = "BTNAbout";
		BTNAbout.SubItemsExpandWidth = 14;
		BTNAbout.Text = "درباره ما";
		BTNAbout.Click += BTNAbout_Click;
		RibbonTabItem1.Checked = true;
		RibbonTabItem1.Name = "RibbonTabItem1";
		RibbonTabItem1.Panel = RibbonPanel1;
		RibbonTabItem1.Text = "پیــــامک (SMS)";
		RibbonTabItem3.Name = "RibbonTabItem3";
		RibbonTabItem3.Panel = RibbonPanel3;
		RibbonTabItem3.Text = "گزارشات پیــامک";
		RibbonTabItem4.Name = "RibbonTabItem4";
		RibbonTabItem4.Panel = RibbonPanel4;
		RibbonTabItem4.Text = "امکانات";
		RibbonTabItem5.Name = "RibbonTabItem5";
		RibbonTabItem5.Panel = RibbonPanel5;
		RibbonTabItem5.Text = "راهنما";
		((Control)Panel1).Anchor = (AnchorStyles)15;
		((Control)Panel1).BackgroundImageLayout = (ImageLayout)3;
		((Control)Panel1).Controls.Add((Control)(object)UcTele1);
		((Control)Panel1).Controls.Add((Control)(object)UCsent_List);
		((Control)Panel1).Controls.Add((Control)(object)UCSendMessage);
		((Control)Panel1).Controls.Add((Control)(object)UcDraft1);
		((Control)Panel1).Controls.Add((Control)(object)UcTemplateList1);
		((Control)Panel1).Location = new Point(0, 132);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(715, 475);
		((Control)Panel1).TabIndex = 16;
		((Control)UcTele1).BackColor = Color.Transparent;
		UcTele1.ContextGuid = null;
		((Control)UcTele1).Location = new Point(50, 146);
		((Control)UcTele1).Name = "UcTele1";
		((Control)UcTele1).Size = new Size(566, 133);
		((Control)UcTele1).TabIndex = 26;
		((Control)UcTele1).Visible = false;
		((Control)UCsent_List).BackColor = Color.Transparent;
		UCsent_List.ContextGuid = null;
		((Control)UCsent_List).Dock = (DockStyle)5;
		((Control)UCsent_List).Location = new Point(0, 0);
		((Control)UCsent_List).Name = "UCsent_List";
		((Control)UCsent_List).RightToLeft = (RightToLeft)1;
		((Control)UCsent_List).Size = new Size(715, 475);
		((Control)UCsent_List).TabIndex = 9;
		((Control)UCsent_List).Visible = false;
		((Control)UCSendMessage).BackColor = Color.Transparent;
		UCSendMessage.ContextGuid = null;
		((Control)UCSendMessage).Dock = (DockStyle)5;
		((Control)UCSendMessage).Location = new Point(0, 0);
		UCSendMessage.Message = "";
		UCSendMessage.Mobiles = "";
		((Control)UCSendMessage).Name = "UCSendMessage";
		UCSendMessage.Number_021 = 0;
		UCSendMessage.Number_4Num = 0;
		UCSendMessage.Number_Currect = 0;
		UCSendMessage.Number_HamraheAval = 0;
		UCSendMessage.Number_InCorrect = 0;
		UCSendMessage.Number_IranCell = 0;
		UCSendMessage.Number_Panel = 0;
		UCSendMessage.Number_Rightel = 0;
		UCSendMessage.Number_Tekrari = 0;
		UCSendMessage.Number_Total = 0;
		((Control)UCSendMessage).Size = new Size(715, 475);
		((Control)UCSendMessage).TabIndex = 16;
		((Control)UcDraft1).BackColor = Color.Transparent;
		UcDraft1.ContextGuid = null;
		((Control)UcDraft1).Dock = (DockStyle)5;
		((Control)UcDraft1).Location = new Point(0, 0);
		((Control)UcDraft1).Name = "UcDraft1";
		((Control)UcDraft1).Size = new Size(715, 475);
		((Control)UcDraft1).TabIndex = 15;
		((Control)UcDraft1).Visible = false;
		UcTemplateList1.ContextGuid = null;
		((Control)UcTemplateList1).Dock = (DockStyle)5;
		((Control)UcTemplateList1).Location = new Point(0, 0);
		((Control)UcTemplateList1).Name = "UcTemplateList1";
		((Control)UcTemplateList1).Size = new Size(715, 475);
		((Control)UcTemplateList1).TabIndex = 17;
		BTNBlackList.ImagePaddingHorizontal = 25;
		BTNBlackList.ImagePosition = eImagePosition.Top;
		BTNBlackList.Name = "BTNBlackList";
		BTNBlackList.SubItemsExpandWidth = 14;
		BTNBlackList.Text = "لیست سیاه";
		((Control)treeView1).Anchor = (AnchorStyles)11;
		((Control)treeView1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)treeView1).Location = new Point(714, 135);
		((Control)treeView1).Name = "treeView1";
		val.Name = "NdInbox";
		val.Text = "دریافتی ها";
		val2.Name = "NdOutBox";
		val2.Text = "در حال ارسال";
		val3.Name = "NdSend";
		val3.Text = "ارسال شده ها";
		treeView1.Nodes.AddRange((TreeNode[])(object)new TreeNode[3] { val, val2, val3 });
		treeView1.RightToLeftLayout = true;
		((Control)treeView1).Size = new Size(145, 472);
		((Control)treeView1).TabIndex = 26;
		treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
		((UserControl)UcStatuseBar1).BorderStyle = (BorderStyle)1;
		UcStatuseBar1.ContextGuid = null;
		UcStatuseBar1.DateM = new DateTime(2020, 12, 12, 16, 19, 8, 0);
		((Control)UcStatuseBar1).Dock = (DockStyle)2;
		((Control)UcStatuseBar1).Location = new Point(0, 606);
		((Control)UcStatuseBar1).Name = "UcStatuseBar1";
		((Control)UcStatuseBar1).Size = new Size(859, 24);
		((Control)UcStatuseBar1).TabIndex = 18;
		((Control)UcOutBox1).BackColor = Color.Transparent;
		UcOutBox1.ContextGuid = null;
		((Control)UcOutBox1).Dock = (DockStyle)5;
		((Control)UcOutBox1).Location = new Point(0, 0);
		((Control)UcOutBox1).Name = "UcOutBox1";
		((Control)UcOutBox1).RightToLeft = (RightToLeft)1;
		((Control)UcOutBox1).Size = new Size(715, 475);
		((Control)UcOutBox1).TabIndex = 8;
		((Control)UcOutBox1).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.InactiveCaptionText;
		((Form)this).ClientSize = new Size(859, 630);
		((Control)this).Controls.Add((Control)(object)treeView1);
		((Control)this).Controls.Add((Control)(object)RibbonControl1);
		((Control)this).Controls.Add((Control)(object)UcStatuseBar1);
		((Control)this).Controls.Add((Control)(object)Panel1);
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Control)this).MinimumSize = new Size(616, 469);
		((Control)this).Name = "FrmPayamakMain";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "نــرم افزارپـــیامک نوین پرداز";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FRMMain_Load;
		RibbonControl1.ResumeLayout(performLayout: false);
		((Control)RibbonControl1).PerformLayout();
		((Control)RibbonPanel3).ResumeLayout(false);
		((Control)RibbonPanel1).ResumeLayout(false);
		((Control)RibbonPanel4).ResumeLayout(false);
		((Control)RibbonPanel5).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
