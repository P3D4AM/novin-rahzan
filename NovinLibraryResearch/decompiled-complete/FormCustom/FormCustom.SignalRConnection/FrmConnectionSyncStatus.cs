using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache;
using EntityCache.Interfaces.Sync;
using FormCustom.SignalRConnection.Uc;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace FormCustom.SignalRConnection;

public class FrmConnectionSyncStatus : AccountingBaseForm
{
	private static ConcurrentDictionary<string, FrmConnectionSyncStatus> _instances = new ConcurrentDictionary<string, FrmConnectionSyncStatus>();

	private bool _isDisposed;

	private CancellationTokenSource _syncInfoToken;

	private IContainer components;

	private UcSyncStatus ucCustomer;

	private UcSyncStatus ucCustomerGroup;

	private UcSyncStatus ucCustomerTitle;

	private UcSyncStatus ucKol;

	private UcSyncStatus ucMoein;

	private UcSyncStatus ucTafsil;

	private UcSyncStatus ucProduct;

	private UcSyncStatus ucProductGroup;

	private UcSyncStatus ucProductVahed;

	private UcSyncStatus ucUser;

	private UcSyncStatus ucCheckM;

	private UcSyncStatus ucCheckSh;

	private UcSyncStatus ucAghsatM;

	private UcSyncStatus ucAghsatSh;

	private UcSyncStatus ucTafsilGroup;

	private UcSyncStatus ucLastPrice;

	private UcSyncStatus ucDepot;

	private UcSyncStatus ucDesc;

	private UcSyncStatus ucCallingLog;

	private UcSyncStatus ucFiles;

	private UcSyncStatus ucContactUs;

	private UcSyncStatus ucVisit;

	private UcSyncStatus ucTicket;

	private UcSyncStatus ucTicketDet;

	private UcSyncStatus ucReception;

	private UcSyncStatus ucPardakht;

	private UcSyncStatus ucOrder;

	private SplitContainer splitContainer1;

	private UcCacheStatus ucCacheStatus;

	private UcSyncStatus ucSerials;

	private UcSyncStatus ucAccounts;

	private UcSyncStatus ucProductPriceLevel;

	internal SuperTabControl TabControl1;

	internal SuperTabControlPanel SuperTabControlPanel3;

	internal PanelEx PanelEx3;

	internal SuperTabItem tabRecieveFromServer;

	internal SuperTabControlPanel SuperTabControlPanel2;

	internal PanelEx PanelEx2;

	internal SuperTabItem TP2;

	private UcSyncDoneResponse ucSyncDoneResponse1;

	public SendData2ServerAggregator SendData2ServerInstance { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	private Task SyncInstance_OnSyncDoneResponse(object sender, SyncDoneResponseEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncDoneResponse", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs"))
		{
			try
			{
				_syncInfoToken?.Cancel();
				_syncInfoToken = new CancellationTokenSource(60000);
				CancellationToken token = _syncInfoToken.Token;
				Task.Run(() => ucSyncDoneResponse1.InitAsync(base.ContextGuid, token));
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
			}
			return Task.CompletedTask;
		}
	}

	public static FrmConnectionSyncStatus GetInstance(string contextGuid, ENSource source)
	{
		try
		{
			if (_instances.TryGetValue(contextGuid, out var value))
			{
				if (value == null || value.IsDisposed)
				{
					value = new FrmConnectionSyncStatus(contextGuid, source);
					_instances.TryRemove(contextGuid, out var _);
					_instances.TryAdd(contextGuid, value);
				}
				return value;
			}
			value = new FrmConnectionSyncStatus(contextGuid, source);
			_instances.TryAdd(contextGuid, value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs", 57);
			return null;
		}
	}

	public void HandleSync()
	{
		try
		{
			if (ClsCache.GetCache(base.ContextGuid)?.Context?.Sync != null)
			{
				ClsCache.GetCache(base.ContextGuid).Context.Sync.OnSyncDoneResponse += SyncInstance_OnSyncDoneResponse;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleSync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs", 68);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				SendData2ServerInstance = null;
				if (ClsCache.GetCache(base.ContextGuid)?.Context?.Sync != null)
				{
					ClsCache.GetCache(base.ContextGuid).Context.Sync.OnSyncDoneResponse -= SyncInstance_OnSyncDoneResponse;
				}
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs", 84);
		}
	}

	private FrmConnectionSyncStatus(string contextGuid, ENSource source)
		: base(contextGuid)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		try
		{
			InitializeComponent();
			SendData2ServerInstance = new SendData2ServerAggregator(ucCustomer, ucCustomerGroup, ucCustomerTitle, ucKol, ucMoein, ucTafsil, ucTafsilGroup, ucProduct, ucProductGroup, ucProductVahed, ucDepot, null, ucUser, null, ucCheckSh, ucCheckM, ucAghsatSh, ucAghsatM, ucLastPrice, ucDesc, ucCallingLog, ucFiles, ucContactUs, ucVisit, ucTicket, ucTicketDet, ucReception, ucPardakht, ucOrder, ucCheckM, ucCheckSh, ucSerials, ucAccounts, ucProductPriceLevel);
			ucCacheStatus.ContextGuid = contextGuid;
			((Form)this).FormClosing += new FormClosingEventHandler(FrmConnectionSyncStatus_FormClosing);
			tabRecieveFromServer.Visible = source == ENSource.Baje || source == ENSource.Department;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs", 105);
		}
	}

	private void FrmConnectionSyncStatus_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			((CancelEventArgs)(object)e).Cancel = true;
			((Control)this).Hide();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmConnectionSyncStatus_FormClosing", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\FrmConnectionSyncStatus.cs", 115);
		}
	}

	public override void OnFormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void InitializeComponent()
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Expected O, but got Unknown
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Expected O, but got Unknown
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		//IL_0baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7a: Expected O, but got Unknown
		//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Expected O, but got Unknown
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_106c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1076: Expected O, but got Unknown
		//IL_109d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1167: Unknown result type (might be due to invalid IL or missing references)
		//IL_1171: Expected O, but got Unknown
		//IL_119b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1265: Unknown result type (might be due to invalid IL or missing references)
		//IL_126f: Expected O, but got Unknown
		//IL_1299: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Unknown result type (might be due to invalid IL or missing references)
		//IL_136d: Expected O, but got Unknown
		//IL_1397: Unknown result type (might be due to invalid IL or missing references)
		//IL_1461: Unknown result type (might be due to invalid IL or missing references)
		//IL_146b: Expected O, but got Unknown
		//IL_1495: Unknown result type (might be due to invalid IL or missing references)
		//IL_155f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1569: Expected O, but got Unknown
		//IL_1593: Unknown result type (might be due to invalid IL or missing references)
		//IL_165d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1667: Expected O, but got Unknown
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_175b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Expected O, but got Unknown
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_1863: Expected O, but got Unknown
		//IL_188d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1957: Unknown result type (might be due to invalid IL or missing references)
		//IL_1961: Expected O, but got Unknown
		//IL_198b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5f: Expected O, but got Unknown
		//IL_1a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5d: Expected O, but got Unknown
		//IL_1b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5b: Expected O, but got Unknown
		//IL_1c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2120: Unknown result type (might be due to invalid IL or missing references)
		//IL_212a: Expected O, but got Unknown
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_221e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2228: Expected O, but got Unknown
		//IL_2252: Unknown result type (might be due to invalid IL or missing references)
		//IL_231c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2326: Expected O, but got Unknown
		//IL_2350: Unknown result type (might be due to invalid IL or missing references)
		//IL_2427: Unknown result type (might be due to invalid IL or missing references)
		//IL_2551: Unknown result type (might be due to invalid IL or missing references)
		//IL_255b: Expected O, but got Unknown
		//IL_2577: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ca: Expected O, but got Unknown
		//IL_2607: Unknown result type (might be due to invalid IL or missing references)
		//IL_2611: Expected O, but got Unknown
		//IL_26a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_275e: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2982: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac2: Expected O, but got Unknown
		//IL_2ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b88: Expected O, but got Unknown
		//IL_2b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9e: Expected O, but got Unknown
		//IL_2ba3: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmConnectionSyncStatus));
		ucCustomer = new UcSyncStatus();
		ucCustomerGroup = new UcSyncStatus();
		ucCustomerTitle = new UcSyncStatus();
		ucKol = new UcSyncStatus();
		ucMoein = new UcSyncStatus();
		ucTafsil = new UcSyncStatus();
		ucProduct = new UcSyncStatus();
		ucProductGroup = new UcSyncStatus();
		ucProductVahed = new UcSyncStatus();
		ucUser = new UcSyncStatus();
		ucCheckM = new UcSyncStatus();
		ucCheckSh = new UcSyncStatus();
		ucAghsatM = new UcSyncStatus();
		ucAghsatSh = new UcSyncStatus();
		ucTafsilGroup = new UcSyncStatus();
		ucLastPrice = new UcSyncStatus();
		ucDepot = new UcSyncStatus();
		ucDesc = new UcSyncStatus();
		ucCallingLog = new UcSyncStatus();
		ucFiles = new UcSyncStatus();
		ucContactUs = new UcSyncStatus();
		ucVisit = new UcSyncStatus();
		ucTicket = new UcSyncStatus();
		ucTicketDet = new UcSyncStatus();
		ucReception = new UcSyncStatus();
		ucPardakht = new UcSyncStatus();
		ucOrder = new UcSyncStatus();
		splitContainer1 = new SplitContainer();
		ucProductPriceLevel = new UcSyncStatus();
		ucAccounts = new UcSyncStatus();
		ucSerials = new UcSyncStatus();
		ucCacheStatus = new UcCacheStatus();
		TabControl1 = new SuperTabControl();
		SuperTabControlPanel3 = new SuperTabControlPanel();
		PanelEx3 = new PanelEx();
		tabRecieveFromServer = new SuperTabItem();
		SuperTabControlPanel2 = new SuperTabControlPanel();
		PanelEx2 = new PanelEx();
		TP2 = new SuperTabItem();
		ucSyncDoneResponse1 = new UcSyncDoneResponse();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((ISupportInitialize)TabControl1).BeginInit();
		((Control)TabControl1).SuspendLayout();
		((Control)SuperTabControlPanel3).SuspendLayout();
		((Control)PanelEx3).SuspendLayout();
		((Control)SuperTabControlPanel2).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)ucCustomer).Anchor = (AnchorStyles)13;
		((UserControl)ucCustomer).BorderStyle = (BorderStyle)1;
		ucCustomer.Count = 0;
		ucCustomer.FinalStatus = null;
		((Control)ucCustomer).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCustomer.Group = SyncPart.None;
		((Control)ucCustomer).Location = new Point(6, 3);
		((Control)ucCustomer).Margin = new Padding(5);
		((Control)ucCustomer).Name = "ucCustomer";
		((Control)ucCustomer).RightToLeft = (RightToLeft)1;
		ucCustomer.ShortMessage = "";
		((Control)ucCustomer).Size = new Size(371, 40);
		ucCustomer.Status = SyncStatus.SyncNotStarted;
		((Control)ucCustomer).TabIndex = 26;
		ucCustomer.Title = "[اشخاص]";
		ucCustomer.TotalCount = 0;
		((Control)ucCustomerGroup).Anchor = (AnchorStyles)13;
		((UserControl)ucCustomerGroup).BorderStyle = (BorderStyle)1;
		ucCustomerGroup.Count = 0;
		ucCustomerGroup.FinalStatus = null;
		((Control)ucCustomerGroup).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCustomerGroup.Group = SyncPart.None;
		((Control)ucCustomerGroup).Location = new Point(5, 3);
		((Control)ucCustomerGroup).Margin = new Padding(5);
		((Control)ucCustomerGroup).Name = "ucCustomerGroup";
		((Control)ucCustomerGroup).RightToLeft = (RightToLeft)1;
		ucCustomerGroup.ShortMessage = "";
		((Control)ucCustomerGroup).Size = new Size(363, 40);
		ucCustomerGroup.Status = SyncStatus.SyncNotStarted;
		((Control)ucCustomerGroup).TabIndex = 25;
		ucCustomerGroup.Title = "[گروه اشخاص]";
		ucCustomerGroup.TotalCount = 0;
		((Control)ucCustomerTitle).Anchor = (AnchorStyles)13;
		((UserControl)ucCustomerTitle).BorderStyle = (BorderStyle)1;
		ucCustomerTitle.Count = 0;
		ucCustomerTitle.FinalStatus = null;
		((Control)ucCustomerTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCustomerTitle.Group = SyncPart.None;
		((Control)ucCustomerTitle).Location = new Point(6, 46);
		((Control)ucCustomerTitle).Margin = new Padding(5);
		((Control)ucCustomerTitle).Name = "ucCustomerTitle";
		((Control)ucCustomerTitle).RightToLeft = (RightToLeft)1;
		ucCustomerTitle.ShortMessage = "";
		((Control)ucCustomerTitle).Size = new Size(371, 40);
		ucCustomerTitle.Status = SyncStatus.SyncNotStarted;
		((Control)ucCustomerTitle).TabIndex = 24;
		ucCustomerTitle.Title = "[عناوین اشخاص]";
		ucCustomerTitle.TotalCount = 0;
		((Control)ucKol).Anchor = (AnchorStyles)13;
		((UserControl)ucKol).BorderStyle = (BorderStyle)1;
		ucKol.Count = 0;
		ucKol.FinalStatus = null;
		((Control)ucKol).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucKol.Group = SyncPart.None;
		((Control)ucKol).Location = new Point(6, 90);
		((Control)ucKol).Margin = new Padding(5);
		((Control)ucKol).Name = "ucKol";
		((Control)ucKol).RightToLeft = (RightToLeft)1;
		ucKol.ShortMessage = "";
		((Control)ucKol).Size = new Size(371, 40);
		ucKol.Status = SyncStatus.SyncNotStarted;
		((Control)ucKol).TabIndex = 23;
		ucKol.Title = "[حسابهای دفتر کل]";
		ucKol.TotalCount = 0;
		((Control)ucMoein).Anchor = (AnchorStyles)13;
		((UserControl)ucMoein).BorderStyle = (BorderStyle)1;
		ucMoein.Count = 0;
		ucMoein.FinalStatus = null;
		((Control)ucMoein).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucMoein.Group = SyncPart.None;
		((Control)ucMoein).Location = new Point(5, 90);
		((Control)ucMoein).Margin = new Padding(5);
		((Control)ucMoein).Name = "ucMoein";
		((Control)ucMoein).RightToLeft = (RightToLeft)1;
		ucMoein.ShortMessage = "";
		((Control)ucMoein).Size = new Size(363, 40);
		ucMoein.Status = SyncStatus.SyncNotStarted;
		((Control)ucMoein).TabIndex = 22;
		ucMoein.Title = "[حسابهای دفتر معین]";
		ucMoein.TotalCount = 0;
		((Control)ucTafsil).Anchor = (AnchorStyles)13;
		((UserControl)ucTafsil).BorderStyle = (BorderStyle)1;
		ucTafsil.Count = 0;
		ucTafsil.FinalStatus = null;
		((Control)ucTafsil).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucTafsil.Group = SyncPart.None;
		((Control)ucTafsil).Location = new Point(6, 134);
		((Control)ucTafsil).Margin = new Padding(5);
		((Control)ucTafsil).Name = "ucTafsil";
		((Control)ucTafsil).RightToLeft = (RightToLeft)1;
		ucTafsil.ShortMessage = "";
		((Control)ucTafsil).Size = new Size(371, 40);
		ucTafsil.Status = SyncStatus.SyncNotStarted;
		((Control)ucTafsil).TabIndex = 21;
		ucTafsil.Title = "[حسابهای تفصیلی]";
		ucTafsil.TotalCount = 0;
		((Control)ucProduct).Anchor = (AnchorStyles)13;
		((UserControl)ucProduct).BorderStyle = (BorderStyle)1;
		ucProduct.Count = 0;
		ucProduct.FinalStatus = null;
		((Control)ucProduct).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucProduct.Group = SyncPart.None;
		((Control)ucProduct).Location = new Point(5, 134);
		((Control)ucProduct).Margin = new Padding(5);
		((Control)ucProduct).Name = "ucProduct";
		((Control)ucProduct).RightToLeft = (RightToLeft)1;
		ucProduct.ShortMessage = "";
		((Control)ucProduct).Size = new Size(363, 40);
		ucProduct.Status = SyncStatus.SyncNotStarted;
		((Control)ucProduct).TabIndex = 20;
		ucProduct.Title = "[کالاها]";
		ucProduct.TotalCount = 0;
		((Control)ucProductGroup).Anchor = (AnchorStyles)13;
		((UserControl)ucProductGroup).BorderStyle = (BorderStyle)1;
		ucProductGroup.Count = 0;
		ucProductGroup.FinalStatus = null;
		((Control)ucProductGroup).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucProductGroup.Group = SyncPart.None;
		((Control)ucProductGroup).Location = new Point(6, 177);
		((Control)ucProductGroup).Margin = new Padding(5);
		((Control)ucProductGroup).Name = "ucProductGroup";
		((Control)ucProductGroup).RightToLeft = (RightToLeft)1;
		ucProductGroup.ShortMessage = "";
		((Control)ucProductGroup).Size = new Size(371, 40);
		ucProductGroup.Status = SyncStatus.SyncNotStarted;
		((Control)ucProductGroup).TabIndex = 19;
		ucProductGroup.Title = "[گروه کالاها]";
		ucProductGroup.TotalCount = 0;
		((Control)ucProductVahed).Anchor = (AnchorStyles)13;
		((UserControl)ucProductVahed).BorderStyle = (BorderStyle)1;
		ucProductVahed.Count = 0;
		ucProductVahed.FinalStatus = null;
		((Control)ucProductVahed).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucProductVahed.Group = SyncPart.None;
		((Control)ucProductVahed).Location = new Point(5, 177);
		((Control)ucProductVahed).Margin = new Padding(5);
		((Control)ucProductVahed).Name = "ucProductVahed";
		((Control)ucProductVahed).RightToLeft = (RightToLeft)1;
		ucProductVahed.ShortMessage = "";
		((Control)ucProductVahed).Size = new Size(363, 40);
		ucProductVahed.Status = SyncStatus.SyncNotStarted;
		((Control)ucProductVahed).TabIndex = 18;
		ucProductVahed.Title = "[واحد کالاها]";
		ucProductVahed.TotalCount = 0;
		((Control)ucUser).Anchor = (AnchorStyles)13;
		((UserControl)ucUser).BorderStyle = (BorderStyle)1;
		ucUser.Count = 0;
		ucUser.FinalStatus = null;
		((Control)ucUser).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucUser.Group = SyncPart.None;
		((Control)ucUser).Location = new Point(6, 264);
		((Control)ucUser).Margin = new Padding(5);
		((Control)ucUser).Name = "ucUser";
		((Control)ucUser).RightToLeft = (RightToLeft)1;
		ucUser.ShortMessage = "";
		((Control)ucUser).Size = new Size(371, 40);
		ucUser.Status = SyncStatus.SyncNotStarted;
		((Control)ucUser).TabIndex = 27;
		ucUser.Title = "[کاربران]";
		ucUser.TotalCount = 0;
		((Control)ucCheckM).Anchor = (AnchorStyles)13;
		((UserControl)ucCheckM).BorderStyle = (BorderStyle)1;
		ucCheckM.Count = 0;
		ucCheckM.FinalStatus = null;
		((Control)ucCheckM).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCheckM.Group = SyncPart.None;
		((Control)ucCheckM).Location = new Point(6, 308);
		((Control)ucCheckM).Margin = new Padding(5);
		((Control)ucCheckM).Name = "ucCheckM";
		((Control)ucCheckM).RightToLeft = (RightToLeft)1;
		ucCheckM.ShortMessage = "";
		((Control)ucCheckM).Size = new Size(371, 40);
		ucCheckM.Status = SyncStatus.SyncNotStarted;
		((Control)ucCheckM).TabIndex = 29;
		ucCheckM.Title = "[چک اشخاص]";
		ucCheckM.TotalCount = 0;
		((Control)ucCheckSh).Anchor = (AnchorStyles)13;
		((UserControl)ucCheckSh).BorderStyle = (BorderStyle)1;
		ucCheckSh.Count = 0;
		ucCheckSh.FinalStatus = null;
		((Control)ucCheckSh).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCheckSh.Group = SyncPart.None;
		((Control)ucCheckSh).Location = new Point(5, 308);
		((Control)ucCheckSh).Margin = new Padding(5);
		((Control)ucCheckSh).Name = "ucCheckSh";
		((Control)ucCheckSh).RightToLeft = (RightToLeft)1;
		ucCheckSh.ShortMessage = "";
		((Control)ucCheckSh).Size = new Size(363, 40);
		ucCheckSh.Status = SyncStatus.SyncNotStarted;
		((Control)ucCheckSh).TabIndex = 30;
		ucCheckSh.Title = "[چک شخصی]";
		ucCheckSh.TotalCount = 0;
		((Control)ucAghsatM).Anchor = (AnchorStyles)13;
		((UserControl)ucAghsatM).BorderStyle = (BorderStyle)1;
		ucAghsatM.Count = 0;
		ucAghsatM.FinalStatus = null;
		((Control)ucAghsatM).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucAghsatM.Group = SyncPart.None;
		((Control)ucAghsatM).Location = new Point(6, 353);
		((Control)ucAghsatM).Margin = new Padding(5);
		((Control)ucAghsatM).Name = "ucAghsatM";
		((Control)ucAghsatM).RightToLeft = (RightToLeft)1;
		ucAghsatM.ShortMessage = "";
		((Control)ucAghsatM).Size = new Size(371, 40);
		ucAghsatM.Status = SyncStatus.SyncNotStarted;
		((Control)ucAghsatM).TabIndex = 31;
		ucAghsatM.Title = "[اقساط مشتری]";
		ucAghsatM.TotalCount = 0;
		((Control)ucAghsatSh).Anchor = (AnchorStyles)13;
		((UserControl)ucAghsatSh).BorderStyle = (BorderStyle)1;
		ucAghsatSh.Count = 0;
		ucAghsatSh.FinalStatus = null;
		((Control)ucAghsatSh).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucAghsatSh.Group = SyncPart.None;
		((Control)ucAghsatSh).Location = new Point(5, 264);
		((Control)ucAghsatSh).Margin = new Padding(5);
		((Control)ucAghsatSh).Name = "ucAghsatSh";
		((Control)ucAghsatSh).RightToLeft = (RightToLeft)1;
		ucAghsatSh.ShortMessage = "";
		((Control)ucAghsatSh).Size = new Size(363, 40);
		ucAghsatSh.Status = SyncStatus.SyncNotStarted;
		((Control)ucAghsatSh).TabIndex = 32;
		ucAghsatSh.Title = "[اقساط شخصی]";
		ucAghsatSh.TotalCount = 0;
		((Control)ucTafsilGroup).Anchor = (AnchorStyles)13;
		((UserControl)ucTafsilGroup).BorderStyle = (BorderStyle)1;
		ucTafsilGroup.Count = 0;
		ucTafsilGroup.FinalStatus = null;
		((Control)ucTafsilGroup).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucTafsilGroup.Group = SyncPart.None;
		((Control)ucTafsilGroup).Location = new Point(5, 46);
		((Control)ucTafsilGroup).Margin = new Padding(5);
		((Control)ucTafsilGroup).Name = "ucTafsilGroup";
		((Control)ucTafsilGroup).RightToLeft = (RightToLeft)1;
		ucTafsilGroup.ShortMessage = "";
		((Control)ucTafsilGroup).Size = new Size(363, 40);
		ucTafsilGroup.Status = SyncStatus.SyncNotStarted;
		((Control)ucTafsilGroup).TabIndex = 33;
		ucTafsilGroup.Title = "[گروه های تفصیلی]";
		ucTafsilGroup.TotalCount = 0;
		((Control)ucLastPrice).Anchor = (AnchorStyles)13;
		((UserControl)ucLastPrice).BorderStyle = (BorderStyle)1;
		ucLastPrice.Count = 0;
		ucLastPrice.FinalStatus = null;
		((Control)ucLastPrice).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucLastPrice.Group = SyncPart.None;
		((Control)ucLastPrice).Location = new Point(6, 397);
		((Control)ucLastPrice).Margin = new Padding(5);
		((Control)ucLastPrice).Name = "ucLastPrice";
		((Control)ucLastPrice).RightToLeft = (RightToLeft)1;
		ucLastPrice.ShortMessage = "";
		((Control)ucLastPrice).Size = new Size(371, 40);
		ucLastPrice.Status = SyncStatus.SyncNotStarted;
		((Control)ucLastPrice).TabIndex = 34;
		ucLastPrice.Title = "[آخرین قیمت کالا]";
		ucLastPrice.TotalCount = 0;
		((Control)ucDepot).Anchor = (AnchorStyles)13;
		((UserControl)ucDepot).BorderStyle = (BorderStyle)1;
		ucDepot.Count = 0;
		ucDepot.FinalStatus = null;
		((Control)ucDepot).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucDepot.Group = SyncPart.None;
		((Control)ucDepot).Location = new Point(6, 221);
		((Control)ucDepot).Margin = new Padding(5);
		((Control)ucDepot).Name = "ucDepot";
		((Control)ucDepot).RightToLeft = (RightToLeft)1;
		ucDepot.ShortMessage = "";
		((Control)ucDepot).Size = new Size(371, 40);
		ucDepot.Status = SyncStatus.SyncNotStarted;
		((Control)ucDepot).TabIndex = 35;
		ucDepot.Title = "[انبارها]";
		ucDepot.TotalCount = 0;
		((Control)ucDesc).Anchor = (AnchorStyles)13;
		((UserControl)ucDesc).BorderStyle = (BorderStyle)1;
		ucDesc.Count = 0;
		ucDesc.FinalStatus = null;
		((Control)ucDesc).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucDesc.Group = SyncPart.None;
		((Control)ucDesc).Location = new Point(5, 353);
		((Control)ucDesc).Margin = new Padding(5);
		((Control)ucDesc).Name = "ucDesc";
		((Control)ucDesc).RightToLeft = (RightToLeft)1;
		ucDesc.ShortMessage = "";
		((Control)ucDesc).Size = new Size(363, 40);
		ucDesc.Status = SyncStatus.SyncNotStarted;
		((Control)ucDesc).TabIndex = 37;
		ucDesc.Title = "[شرح آماده]";
		ucDesc.TotalCount = 0;
		((Control)ucCallingLog).Anchor = (AnchorStyles)13;
		((UserControl)ucCallingLog).BorderStyle = (BorderStyle)1;
		ucCallingLog.Count = 0;
		ucCallingLog.FinalStatus = null;
		((Control)ucCallingLog).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucCallingLog.Group = SyncPart.None;
		((Control)ucCallingLog).Location = new Point(6, 440);
		((Control)ucCallingLog).Margin = new Padding(5);
		((Control)ucCallingLog).Name = "ucCallingLog";
		((Control)ucCallingLog).RightToLeft = (RightToLeft)1;
		ucCallingLog.ShortMessage = "";
		((Control)ucCallingLog).Size = new Size(371, 40);
		ucCallingLog.Status = SyncStatus.SyncNotStarted;
		((Control)ucCallingLog).TabIndex = 38;
		ucCallingLog.Title = "[تماس ها]";
		ucCallingLog.TotalCount = 0;
		((Control)ucFiles).Anchor = (AnchorStyles)13;
		((UserControl)ucFiles).BorderStyle = (BorderStyle)1;
		ucFiles.Count = 0;
		ucFiles.FinalStatus = null;
		((Control)ucFiles).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucFiles.Group = SyncPart.None;
		((Control)ucFiles).Location = new Point(5, 397);
		((Control)ucFiles).Margin = new Padding(5);
		((Control)ucFiles).Name = "ucFiles";
		((Control)ucFiles).RightToLeft = (RightToLeft)1;
		ucFiles.ShortMessage = "";
		((Control)ucFiles).Size = new Size(363, 40);
		ucFiles.Status = SyncStatus.SyncNotStarted;
		((Control)ucFiles).TabIndex = 39;
		ucFiles.Title = "[فایل ها]";
		ucFiles.TotalCount = 0;
		((Control)ucContactUs).Anchor = (AnchorStyles)13;
		((UserControl)ucContactUs).BorderStyle = (BorderStyle)1;
		ucContactUs.Count = 0;
		ucContactUs.FinalStatus = null;
		((Control)ucContactUs).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucContactUs.Group = SyncPart.None;
		((Control)ucContactUs).Location = new Point(6, 484);
		((Control)ucContactUs).Margin = new Padding(5);
		((Control)ucContactUs).Name = "ucContactUs";
		((Control)ucContactUs).RightToLeft = (RightToLeft)1;
		ucContactUs.ShortMessage = "";
		((Control)ucContactUs).Size = new Size(371, 40);
		ucContactUs.Status = SyncStatus.SyncNotStarted;
		((Control)ucContactUs).TabIndex = 40;
		ucContactUs.Title = "[تماس با ما]";
		ucContactUs.TotalCount = 0;
		((Control)ucVisit).Anchor = (AnchorStyles)13;
		((UserControl)ucVisit).BorderStyle = (BorderStyle)1;
		ucVisit.Count = 0;
		ucVisit.FinalStatus = null;
		((Control)ucVisit).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucVisit.Group = SyncPart.None;
		((Control)ucVisit).Location = new Point(5, 440);
		((Control)ucVisit).Margin = new Padding(5);
		((Control)ucVisit).Name = "ucVisit";
		((Control)ucVisit).RightToLeft = (RightToLeft)1;
		ucVisit.ShortMessage = "";
		((Control)ucVisit).Size = new Size(363, 40);
		ucVisit.Status = SyncStatus.SyncNotStarted;
		((Control)ucVisit).TabIndex = 41;
		ucVisit.Title = "[ویزیت]";
		ucVisit.TotalCount = 0;
		((Control)ucTicket).Anchor = (AnchorStyles)13;
		((UserControl)ucTicket).BorderStyle = (BorderStyle)1;
		ucTicket.Count = 0;
		ucTicket.FinalStatus = null;
		((Control)ucTicket).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucTicket.Group = SyncPart.None;
		((Control)ucTicket).Location = new Point(6, 852);
		((Control)ucTicket).Margin = new Padding(5);
		((Control)ucTicket).Name = "ucTicket";
		((Control)ucTicket).RightToLeft = (RightToLeft)1;
		ucTicket.ShortMessage = "";
		((Control)ucTicket).Size = new Size(288, 60);
		ucTicket.Status = SyncStatus.SyncNotStarted;
		((Control)ucTicket).TabIndex = 42;
		ucTicket.Title = "[تیکت ها]";
		ucTicket.TotalCount = 0;
		((Control)ucTicketDet).Anchor = (AnchorStyles)13;
		((UserControl)ucTicketDet).BorderStyle = (BorderStyle)1;
		ucTicketDet.Count = 0;
		ucTicketDet.FinalStatus = null;
		((Control)ucTicketDet).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucTicketDet.Group = SyncPart.None;
		((Control)ucTicketDet).Location = new Point(5, 484);
		((Control)ucTicketDet).Margin = new Padding(5);
		((Control)ucTicketDet).Name = "ucTicketDet";
		((Control)ucTicketDet).RightToLeft = (RightToLeft)1;
		ucTicketDet.ShortMessage = "";
		((Control)ucTicketDet).Size = new Size(363, 40);
		ucTicketDet.Status = SyncStatus.SyncNotStarted;
		((Control)ucTicketDet).TabIndex = 43;
		ucTicketDet.Title = "[جزئیات تیکت ها]";
		ucTicketDet.TotalCount = 0;
		((Control)ucReception).Anchor = (AnchorStyles)13;
		((UserControl)ucReception).BorderStyle = (BorderStyle)1;
		ucReception.Count = 0;
		ucReception.FinalStatus = null;
		((Control)ucReception).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucReception.Group = SyncPart.None;
		((Control)ucReception).Location = new Point(6, 925);
		((Control)ucReception).Margin = new Padding(5);
		((Control)ucReception).Name = "ucReception";
		((Control)ucReception).RightToLeft = (RightToLeft)1;
		ucReception.ShortMessage = "";
		((Control)ucReception).Size = new Size(252, 40);
		ucReception.Status = SyncStatus.SyncNotStarted;
		((Control)ucReception).TabIndex = 44;
		ucReception.Title = "[دریافت ها]";
		ucReception.TotalCount = 0;
		((Control)ucPardakht).Anchor = (AnchorStyles)13;
		((UserControl)ucPardakht).BorderStyle = (BorderStyle)1;
		ucPardakht.Count = 0;
		ucPardakht.FinalStatus = null;
		((Control)ucPardakht).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucPardakht.Group = SyncPart.None;
		((Control)ucPardakht).Location = new Point(5, 855);
		((Control)ucPardakht).Margin = new Padding(5);
		((Control)ucPardakht).Name = "ucPardakht";
		((Control)ucPardakht).RightToLeft = (RightToLeft)1;
		ucPardakht.ShortMessage = "";
		((Control)ucPardakht).Size = new Size(252, 40);
		ucPardakht.Status = SyncStatus.SyncNotStarted;
		((Control)ucPardakht).TabIndex = 45;
		ucPardakht.Title = "[پرداخت ها]";
		ucPardakht.TotalCount = 0;
		((Control)ucOrder).Anchor = (AnchorStyles)13;
		((UserControl)ucOrder).BorderStyle = (BorderStyle)1;
		ucOrder.Count = 0;
		ucOrder.FinalStatus = null;
		((Control)ucOrder).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucOrder.Group = SyncPart.None;
		((Control)ucOrder).Location = new Point(6, 994);
		((Control)ucOrder).Margin = new Padding(5);
		((Control)ucOrder).Name = "ucOrder";
		((Control)ucOrder).RightToLeft = (RightToLeft)1;
		ucOrder.ShortMessage = "";
		((Control)ucOrder).Size = new Size(252, 40);
		ucOrder.Status = SyncStatus.SyncNotStarted;
		((Control)ucOrder).TabIndex = 46;
		ucOrder.Title = "[فاکتور ها]";
		ucOrder.TotalCount = 0;
		((Control)splitContainer1).Anchor = (AnchorStyles)15;
		((Control)splitContainer1).Location = new Point(9, 72);
		((Control)splitContainer1).Margin = new Padding(5);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucPardakht);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucProductVahed);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucProduct);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucOrder);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucReception);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucTicketDet);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucMoein);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucVisit);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucCustomerGroup);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucFiles);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucDesc);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucCheckSh);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucAghsatSh);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucTafsilGroup);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)ucProductPriceLevel);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucAccounts);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucSerials);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucTicket);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucProductGroup);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucTafsil);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucKol);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucCustomerTitle);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucCustomer);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucUser);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucContactUs);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucCheckM);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucAghsatM);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucCallingLog);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucLastPrice);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)ucDepot);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(770, 528);
		splitContainer1.SplitterDistance = 377;
		splitContainer1.SplitterWidth = 6;
		((Control)splitContainer1).TabIndex = 47;
		((Control)ucProductPriceLevel).Anchor = (AnchorStyles)13;
		((UserControl)ucProductPriceLevel).BorderStyle = (BorderStyle)1;
		ucProductPriceLevel.Count = 0;
		ucProductPriceLevel.FinalStatus = null;
		((Control)ucProductPriceLevel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucProductPriceLevel.Group = SyncPart.None;
		((Control)ucProductPriceLevel).Location = new Point(5, 221);
		((Control)ucProductPriceLevel).Margin = new Padding(5);
		((Control)ucProductPriceLevel).Name = "ucProductPriceLevel";
		((Control)ucProductPriceLevel).RightToLeft = (RightToLeft)1;
		ucProductPriceLevel.ShortMessage = "";
		((Control)ucProductPriceLevel).Size = new Size(363, 40);
		ucProductPriceLevel.Status = SyncStatus.SyncNotStarted;
		((Control)ucProductPriceLevel).TabIndex = 35;
		ucProductPriceLevel.Title = "[سطح قیمت کالا]";
		ucProductPriceLevel.TotalCount = 0;
		((Control)ucAccounts).Anchor = (AnchorStyles)13;
		((UserControl)ucAccounts).BorderStyle = (BorderStyle)1;
		ucAccounts.Count = 0;
		ucAccounts.FinalStatus = null;
		((Control)ucAccounts).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucAccounts.Group = SyncPart.None;
		((Control)ucAccounts).Location = new Point(6, 989);
		((Control)ucAccounts).Margin = new Padding(5);
		((Control)ucAccounts).Name = "ucAccounts";
		((Control)ucAccounts).RightToLeft = (RightToLeft)1;
		ucAccounts.ShortMessage = "";
		((Control)ucAccounts).Size = new Size(288, 60);
		ucAccounts.Status = SyncStatus.SyncNotStarted;
		((Control)ucAccounts).TabIndex = 47;
		ucAccounts.Title = "[مانده حساب ها]";
		ucAccounts.TotalCount = 0;
		((Control)ucSerials).Anchor = (AnchorStyles)13;
		((UserControl)ucSerials).BorderStyle = (BorderStyle)1;
		ucSerials.Count = 0;
		ucSerials.FinalStatus = null;
		((Control)ucSerials).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucSerials.Group = SyncPart.None;
		((Control)ucSerials).Location = new Point(6, 920);
		((Control)ucSerials).Margin = new Padding(5);
		((Control)ucSerials).Name = "ucSerials";
		((Control)ucSerials).RightToLeft = (RightToLeft)1;
		ucSerials.ShortMessage = "";
		((Control)ucSerials).Size = new Size(288, 60);
		ucSerials.Status = SyncStatus.SyncNotStarted;
		((Control)ucSerials).TabIndex = 43;
		ucSerials.Title = "[سریال ها]";
		ucSerials.TotalCount = 0;
		((Control)ucCacheStatus).Anchor = (AnchorStyles)13;
		((Control)ucCacheStatus).BackColor = Color.Transparent;
		((UserControl)ucCacheStatus).BorderStyle = (BorderStyle)1;
		ucCacheStatus.ContextGuid = null;
		((Control)ucCacheStatus).Location = new Point(712, 5);
		((Control)ucCacheStatus).Margin = new Padding(3, 5, 3, 5);
		((Control)ucCacheStatus).Name = "ucCacheStatus";
		ucCacheStatus.ShowStatusMenu = false;
		((Control)ucCacheStatus).Size = new Size(59, 64);
		((Control)ucCacheStatus).TabIndex = 48;
		((Control)TabControl1).BackColor = Color.Transparent;
		TabControl1.ControlBox.CloseBox.Name = "";
		TabControl1.ControlBox.MenuBox.Name = "";
		TabControl1.ControlBox.Name = "";
		TabControl1.ControlBox.SubItems.AddRange(new BaseItem[2]
		{
			TabControl1.ControlBox.CloseBox,
			TabControl1.ControlBox.MenuBox
		});
		((Control)TabControl1).Controls.Add((Control)(object)SuperTabControlPanel3);
		((Control)TabControl1).Controls.Add((Control)(object)SuperTabControlPanel2);
		((Control)TabControl1).Dock = (DockStyle)5;
		((Control)TabControl1).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)TabControl1).Location = new Point(0, 0);
		((Control)TabControl1).Margin = new Padding(3, 5, 3, 5);
		((Control)TabControl1).Name = "TabControl1";
		TabControl1.ReorderTabsEnabled = true;
		((Control)TabControl1).RightToLeft = (RightToLeft)1;
		TabControl1.SelectedTabFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		TabControl1.SelectedTabIndex = 0;
		((Control)TabControl1).Size = new Size(784, 635);
		TabControl1.TabFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TabControl1).TabIndex = 95;
		TabControl1.Tabs.AddRange(new BaseItem[2] { TP2, tabRecieveFromServer });
		TabControl1.TabStyle = eSuperTabStyle.WinMediaPlayer12;
		((Control)TabControl1).Text = "SuperTabControl1";
		((Control)SuperTabControlPanel3).Controls.Add((Control)(object)PanelEx3);
		((Control)SuperTabControlPanel3).Dock = (DockStyle)5;
		((Control)SuperTabControlPanel3).Location = new Point(0, 30);
		((Control)SuperTabControlPanel3).Margin = new Padding(3, 5, 3, 5);
		((Control)SuperTabControlPanel3).Name = "SuperTabControlPanel3";
		((Control)SuperTabControlPanel3).Size = new Size(784, 605);
		((Control)SuperTabControlPanel3).TabIndex = 0;
		SuperTabControlPanel3.TabItem = tabRecieveFromServer;
		((Control)PanelEx3).Anchor = (AnchorStyles)15;
		PanelEx3.CanvasColor = SystemColors.Control;
		PanelEx3.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx3).Controls.Add((Control)(object)ucSyncDoneResponse1);
		PanelEx3.DisabledBackColor = Color.Empty;
		((Control)PanelEx3).Location = new Point(0, 0);
		((Control)PanelEx3).Margin = new Padding(3, 5, 3, 5);
		((Control)PanelEx3).Name = "PanelEx3";
		((Control)PanelEx3).Size = new Size(784, 605);
		PanelEx3.Style.Alignment = (StringAlignment)1;
		PanelEx3.Style.BackColor1.Color = Color.White;
		PanelEx3.Style.BackColor2.Color = Color.White;
		PanelEx3.Style.Border = eBorderType.SingleLine;
		PanelEx3.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx3.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx3.Style.GradientAngle = 90;
		((Control)PanelEx3).TabIndex = 2;
		tabRecieveFromServer.AttachedControl = (Control)(object)SuperTabControlPanel3;
		tabRecieveFromServer.GlobalItem = false;
		tabRecieveFromServer.Name = "tabRecieveFromServer";
		tabRecieveFromServer.Text = "[وضعیت دریافت از سرور]";
		((Control)SuperTabControlPanel2).Controls.Add((Control)(object)PanelEx2);
		((Control)SuperTabControlPanel2).Dock = (DockStyle)5;
		((Control)SuperTabControlPanel2).Location = new Point(0, 30);
		((Control)SuperTabControlPanel2).Margin = new Padding(3, 5, 3, 5);
		((Control)SuperTabControlPanel2).Name = "SuperTabControlPanel2";
		((Control)SuperTabControlPanel2).Size = new Size(784, 605);
		((Control)SuperTabControlPanel2).TabIndex = 0;
		SuperTabControlPanel2.TabItem = TP2;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx2).Controls.Add((Control)(object)ucCacheStatus);
		((Control)PanelEx2).Controls.Add((Control)(object)splitContainer1);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Dock = (DockStyle)5;
		((Control)PanelEx2).Location = new Point(0, 0);
		((Control)PanelEx2).Margin = new Padding(3, 5, 3, 5);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(784, 605);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.Color = Color.White;
		PanelEx2.Style.BackColor2.Color = Color.White;
		PanelEx2.Style.Border = eBorderType.SingleLine;
		PanelEx2.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx2.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 1;
		TP2.AttachedControl = (Control)(object)SuperTabControlPanel2;
		TP2.GlobalItem = false;
		TP2.Name = "TP2";
		TP2.Text = "[وضعیت ارسال به سرور]";
		((Control)ucSyncDoneResponse1).Dock = (DockStyle)5;
		((Control)ucSyncDoneResponse1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucSyncDoneResponse1).Location = new Point(0, 0);
		((Control)ucSyncDoneResponse1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucSyncDoneResponse1).Name = "ucSyncDoneResponse1";
		((Control)ucSyncDoneResponse1).RightToLeft = (RightToLeft)1;
		((Control)ucSyncDoneResponse1).Size = new Size(784, 605);
		((Control)ucSyncDoneResponse1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 635);
		((Control)this).Controls.Add((Control)(object)TabControl1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(6, 5, 6, 5);
		((Control)this).Name = "FrmConnectionSyncStatus";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[وضعیت سینک داده ها با سرور نوین پرداز]";
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((ISupportInitialize)TabControl1).EndInit();
		((Control)TabControl1).ResumeLayout(false);
		((Control)SuperTabControlPanel3).ResumeLayout(false);
		((Control)PanelEx3).ResumeLayout(false);
		((Control)SuperTabControlPanel2).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
