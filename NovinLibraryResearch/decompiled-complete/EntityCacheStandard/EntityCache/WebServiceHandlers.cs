using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.Note;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Business.Attendance;
using EntityCache.Business.Crm;
using EntityCache.Business.Customers;
using EntityCache.Business.Department;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.Products;
using EntityCache.Business.Report;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.WebService;
using EntityCache.WebService.Repository;
using EntityCache.WebService.Repository.Accounting;
using EntityCache.WebService.Repository.Files;
using EntityCache.WebService.Repository.Tickets;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using Serilog;

namespace EntityCache;

public class WebServiceHandlers : INpDisposable, IDisposable, IObserver
{
	internal readonly string imagePath;

	private HyperLinkWebServiceRepository hyperLink;

	private NewsWebServiceRepository news;

	private DepotWebServiceRepository depot;

	private ProductVahedWebServiceRepository productVahed;

	private ProductGroupWebServiceRepository productGroups;

	private CustomerTitleWebServiceRepository customerTitle;

	private CustomerGroupWebServiceRepository customerGroup;

	private FileInfoWebServiceRepository fileInfo;

	private DeliveryWebServiceRepository delivery;

	private GpsWebServiceRepository gps;

	private ClsCache _cache;

	private CancellationTokenSource _syncToken;

	public bool IsDisposed { get; private set; }

	public HesabTreeWebServiceRepository HesabTree { get; private set; }

	public ProductWebServiceRepository Products { get; private set; }

	public CustomerWebServiceRepository Customers { get; private set; }

	public SyncWebServiceRepository Sync { get; private set; }

	public ContactUsWebServiceRepository ContactUs { get; private set; }

	public UserInfoWebServiceRepository UserInfo { get; private set; }

	public ReceptionWebServiceRepository Reception { get; private set; }

	public PardakhtWebServiceRepository Pardakht { get; private set; }

	public OrderWebServiceRepository Order { get; private set; }

	public VisitWebServiceRepository visit { get; private set; }

	public ExecuteQueryWebServiceRepository ExecuteQuery { get; private set; }

	public AghsatAshkhasWebServiceRepository AghsatAshkhas { get; private set; }

	public AghsatMGroupWebServiceRepository AghsatMGroup { get; private set; }

	public AghsatShakhsiWebServiceRepository AghsatShakhsi { get; private set; }

	public CallingLogWebServiceRepository CallingLog { get; private set; }

	public ViewCheckMWebServiceRepository ViewCheckM { get; private set; }

	public ViewCheckShWebServiceRepository ViewCheckSh { get; private set; }

	public TafsilLevelWebServiceRepository TafsilLevel { get; private set; }

	public LastPriceWebServiceRepository LastPrice { get; private set; }

	public KolWebServiceRepository Kol { get; private set; }

	public MoeinWebServiceRepository Moein { get; private set; }

	public TafsilWebServiceRepository Tafsil { get; private set; }

	public CashierLogWebServiceRepository UserLog { get; private set; }

	public ChequeMOperationWebServiceRepository ChequeM { get; private set; }

	public ChequeShOperationWebServiceRepository ChequeSh { get; private set; }

	public SanadWebServiceRepository Sanad { get; private set; }

	public PrdAnbarGardaniWebServiceRepository AnbarGardani { get; private set; }

	public PrdSerialWebServiceRepository Serial { get; private set; }

	public CustomerRoleWebServiceRepository CustomerRole { get; private set; }

	public NoteWebServiceRepository Note { get; private set; }

	public DiscountCouponWebServiceRepository DiscountCoupon { get; private set; }

	public TaxWebServiceRepository Tax { get; private set; }

	public ProductPriceLevelWebServiceRepository ProductPrice { get; private set; }

	public SanadStatusReportWebServiceRepository SanadStatus { get; private set; }

	public SanadPrintWebServiceRepository SanadPrint { get; private set; }

	public PersonLeaveWebServiceRepository PersonLeave { get; private set; }

	public BiometricDataWebServiceRepository Biometric { get; private set; }

	public TicketWebServiceRepository Ticket { get; private set; }

	public TicketDetWebServiceRepository TicketDet { get; private set; }

	public TicketPartWebServiceRepository TicketPart { get; private set; }

	public DescWebServiceRepository Desc { get; private set; }

	public IntroductionMethodWebServiceRepository IntroductionMethod { get; private set; }

	public SyncEntityWebServiceRepository SyncEntity { get; private set; }

	public AccountWebServiceRepository Account { get; private set; }

	public TaxServiceWebServiceRepository TaxService { get; private set; }

	public string ImagePath => imagePath;

	public WebServiceHandlers(ClsCache cache, string imagePath)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs"))
		{
			try
			{
				_cache?.Subscribe(this);
				_cache = cache;
				if (!string.IsNullOrEmpty(imagePath) && !Directory.Exists(imagePath))
				{
					Directory.CreateDirectory(imagePath);
				}
				this.imagePath = imagePath;
				cache.BufferLoggedIn += Buffer_LoggedIn;
				Initialize(cache);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 122);
			}
		}
	}

	private async Task Buffer_LoggedIn(object sender, LoginResponseEventArgs e)
	{
		using (new JaegerMonitor("Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs"))
		{
			_ = 1;
			try
			{
				Log.Information("Buffer_LoggedIn called");
				await Task.Delay(1000);
				_syncToken?.Cancel();
				_syncToken = new CancellationTokenSource();
				CancellationTokenSource.CreateLinkedTokenSource(e.connection.cancellationToken, _cache.BufferCancellationToken, _syncToken.Token);
				CancellationToken token = _syncToken.Token;
				await Task.Delay(1000);
				if (!token.IsCancellationRequested && e.Status == EnLoginResult.Success)
				{
					Task.Run(() => SendData2Server(EnSystem.SignalR, token, _cache), token);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 145);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendData2Server(EnSystem system, CancellationToken token, ClsCache cache)
	{
		int line = 1;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			while (!IsDisposed && !token.IsCancellationRequested)
			{
				ReturnedSaveFuncInfo result;
				try
				{
					ret = new ReturnedSaveFuncInfo();
					cache?.SendData2ServerInstance?.Reset();
					ret.AddReturnedValue(await SendDataToServerAsync(system, token, cache), ReturnedState.Information, "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 162);
					line = 15;
					if (token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 164);
						goto IL_0280;
					}
					line = 16;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 169);
					result = ret;
					goto IL_0280;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 174);
					ret.AddReturnedValue(ex, "", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 175);
				}
				finally
				{
					await Task.Delay(60000);
				}
				result = null;
				continue;
				IL_0280:
				return result;
			}
		}
		catch (OperationCanceledException exception2)
		{
			ret.AddReturnedValue(exception2, "", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 183);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 186);
			ret.AddReturnedValue(ex2, "", "SendData2Server", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 187);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SendDataToServerAsync(EnSystem system, CancellationToken token, ClsCache cache)
	{
		string tranName = "";
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ILogger hesabYarLogger = LogCache.Instance.Value.GetHesabYarLogger();
			if (hesabYarLogger != null)
			{
				hesabYarLogger.Debug($"SendDataToServerAsync Start.line:{line}");
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(cache.ContextGuid);
			LocalSettingBusiness.Send2ServerAsync(cache.ContextGuid, setting, token);
			if (!cache.BufferEnable)
			{
				throw new Exception("cachec has not buffer!!");
			}
			IMasterSendData2Server masterUi = cache?.MasterSendData2ServerAggregator?.Master;
			masterUi?.ResetResults();
			if (token.IsCancellationRequested || IsDisposed)
			{
				ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 206);
			}
			line = 2;
			ILogger hesabYarLogger2 = LogCache.Instance.Value.GetHesabYarLogger();
			if (hesabYarLogger2 != null)
			{
				hesabYarLogger2.Debug($"SendDataToServerAsync.line:{line}");
			}
			while (_cache.BufferLoginState.Status != EnLoginResult.Success && !_cache.Initialized)
			{
				line = 3;
				if (token.IsCancellationRequested || IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 212);
				}
				line = 4;
				await Task.Delay(1000, token);
			}
			ILogger hesabYarLogger3 = LogCache.Instance.Value.GetHesabYarLogger();
			if (hesabYarLogger3 != null)
			{
				hesabYarLogger3.Debug($"SendDataToServerAsync.line:{line}");
			}
			if (masterUi != null)
			{
				masterUi.StartSync = DateTime.Now;
			}
			line = 8;
			if (cache.Source == ENSource.Android || cache.Source == ENSource.Android_HesabYar || cache.Source == ENSource.IOS_HesabYar)
			{
				Task.Run(() => GPSBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), token);
			}
			line = 90;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 222);
			}
			line = 910;
			ILogger hesabYarLogger4 = LogCache.Instance.Value.GetHesabYarLogger();
			if (hesabYarLogger4 != null)
			{
				hesabYarLogger4.Debug($"SendDataToServerAsync.line:{line}");
			}
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 225);
			}
			ReturnedSaveFuncInfo products7;
			if (cache.Configs.Any((EnCacheConfig p) => p == EnCacheConfig.Accounting))
			{
				await ret.AddReturnedValueAsync(await UserAccBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 229);
				await ret.AddReturnedValueAsync(await CashiersLogBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 232);
				line = 9300;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک گروه های تفصیلی در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger5 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger5 != null)
					{
						hesabYarLogger5.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 9301;
				ReturnedSaveFuncInfo resTafsilGroup = await TafsilGroupBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				line = 9302;
				await ret.AddReturnedValueAsync(resTafsilGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 244);
				line = 9303;
				if (masterUi != null)
				{
					ILogger hesabYarLogger6 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger6 != null)
					{
						hesabYarLogger6.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					await masterUi.Result.AddReturnedValueAsync(resTafsilGroup, ReturnedState.Information, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 250);
				}
				line = 9304;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 253);
				}
				line = 9305;
				resTafsilGroup.throwExceptionIfError();
				line = 9306;
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک گروه های تفصیلی در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger7 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger7 != null)
					{
						hesabYarLogger7.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 940;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک گروه اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger8 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger8 != null)
					{
						hesabYarLogger8.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				ReturnedSaveFuncInfo resCusGroup = await CusGroupBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5);
				await ret.AddReturnedValueAsync(resCusGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 269);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resCusGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 272);
					ILogger hesabYarLogger9 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger9 != null)
					{
						hesabYarLogger9.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 275);
				}
				resCusGroup.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک گروه اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
				}
				line = 960;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک عناوین اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger10 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger10 != null)
					{
						hesabYarLogger10.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 960001;
				ReturnedSaveFuncInfo resCusTitle = await CusTitleBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				line = 960002;
				await ret.AddReturnedValueAsync(resCusTitle, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 289);
				line = 960003;
				line = 960004;
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resCusTitle, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 292);
				}
				line = 960005;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 294);
				}
				resCusTitle.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک عناوین اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger11 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger11 != null)
					{
						hesabYarLogger11.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 960006;
				ReturnedSaveFuncInfo resCusRole = await CustomerRoleBusiness.ResendNotSentsAsync(cache.ContextGuid, EnSystem.SignalR, token, "", 10);
				line = 960007;
				await ret.AddReturnedValueAsync(resCusRole, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 305);
				line = 960008;
				line = 960009;
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resCusRole, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 310);
				}
				line = 960010;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 313);
				}
				resCusRole.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک نقش اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger12 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger12 != null)
					{
						hesabYarLogger12.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 960011;
				ReturnedSaveFuncInfo resNote = await NoteBusiness.ResendNotSentsAsync(cache.ContextGuid, EnSystem.SignalR, token, "", 10);
				line = 960012;
				await ret.AddReturnedValueAsync(resNote, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 324);
				line = 960013;
				line = 960014;
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resNote, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 327);
				}
				line = 960015;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 329);
				}
				resNote.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک یادداشت ها در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger13 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger13 != null)
					{
						hesabYarLogger13.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 970;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک حساب های کل در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo resKol = await KolBusiness.ResendNotSents(cache.ContextGuid, token, "", 5);
				await ret.AddReturnedValueAsync(resKol, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 340);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resKol, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 341);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 342);
				}
				resKol.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک حساب های کل در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger14 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger14 != null)
					{
						hesabYarLogger14.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 980;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک حساب های معین در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo resMoein = await MoeinBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				await ret.AddReturnedValueAsync(resMoein, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 353);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resMoein, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 354);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 355);
				}
				resMoein.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک حساب های معین در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger15 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger15 != null)
					{
						hesabYarLogger15.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 990;
				if (masterUi != null)
				{
					masterUi.Description = "آغاز سینک حساب های تفصیلی در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo resTafsil = await TafsilBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				await ret.AddReturnedValueAsync(resTafsil, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 366);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resTafsil, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 369);
					ILogger hesabYarLogger16 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger16 != null)
					{
						hesabYarLogger16.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 372);
				}
				resTafsil.throwExceptionIfError();
				if (masterUi != null)
				{
					masterUi.Description = "اتمام سینک حساب های تفصیلی در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger17 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger17 != null)
					{
						hesabYarLogger17.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 9500;
				if (masterUi != null)
				{
					line = 9501;
					masterUi.Description = "آغاز سینک اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
				}
				line = 9502;
				ReturnedSaveFuncInfo resCustomer = await CusBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5);
				line = 9503;
				await ret.AddReturnedValueAsync(resCustomer, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 388);
				line = 9504;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 390);
				}
				line = 9506;
				resCustomer.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(resCustomer, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 396);
					ILogger hesabYarLogger18 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger18 != null)
					{
						hesabYarLogger18.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					line = 9509;
					masterUi.Description = "اتمام سینک اشخاص در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger19 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger19 != null)
					{
						hesabYarLogger19.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					line = 9601;
					masterUi.Description = "آغاز سینک گروه کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger20 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger20 != null)
					{
						hesabYarLogger20.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 962;
				ReturnedSaveFuncInfo prdGroup = await PrdGroupBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				line = 9603;
				await ret.AddReturnedValueAsync(prdGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 408);
				if (!token.IsCancellationRequested)
				{
					prdGroup.throwExceptionIfError();
				}
				line = 9604;
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(prdGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 413);
					ILogger hesabYarLogger21 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger21 != null)
					{
						hesabYarLogger21.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					line = 9607;
					masterUi.Description = "اتمام سینک گروه کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger22 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger22 != null)
					{
						hesabYarLogger22.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک واحد کالا در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo PrdVahed = await PrdVahedBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				await ret.AddReturnedValueAsync(PrdVahed, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 421);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 422);
				}
				PrdVahed.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(PrdVahed, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 426);
					ILogger hesabYarLogger23 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger23 != null)
					{
						hesabYarLogger23.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک واحد کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger24 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger24 != null)
					{
						hesabYarLogger24.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک اقساط مشتری در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo accounts = await AccountBusiness.ResendNotSentsAsync(cache.ContextGuid, system, token, "", 10);
				await ret.AddReturnedValueAsync(accounts, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 434);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 435);
				}
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(accounts, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 437);
				}
				ReturnedSaveFuncInfo AghsatMGroup = await AghsatM_GroupBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 440);
				}
				await ret.AddReturnedValueAsync(AghsatMGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 441);
				AghsatMGroup.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(AghsatMGroup, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 445);
					ILogger hesabYarLogger25 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger25 != null)
					{
						hesabYarLogger25.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک گروه اقساط مشتری در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo AghsatM = await AghsatMBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 451);
				}
				await ret.AddReturnedValueAsync(AghsatM, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 452);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(AghsatM, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 453);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 454);
				}
				AghsatM.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(AghsatM, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 458);
					ILogger hesabYarLogger26 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger26 != null)
					{
						hesabYarLogger26.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک اقساط مشتری در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger27 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger27 != null)
					{
						hesabYarLogger27.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک اقساط شخصی در ساعت " + DateTime.Now.ToShortTimeString();
				}
				line = 10201;
				ReturnedSaveFuncInfo AghsatSh = await AghsatShBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 467);
				}
				line = 10202;
				await ret.AddReturnedValueAsync(AghsatSh, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 469);
				AghsatSh.throwExceptionIfError();
				line = 10203;
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(AghsatSh, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 474);
					ILogger hesabYarLogger28 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger28 != null)
					{
						hesabYarLogger28.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک اقساط شخصی در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger29 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger29 != null)
					{
						hesabYarLogger29.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک انبارها در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo depots = await DepotBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 482);
				}
				if (depots.HasError)
				{
					throw new Exception("فکر کنم ارسال انبارها ها به سرور تایم اوت داره\r\n" + depots.GetErrorMessage());
				}
				depots.throwExceptionIfError();
				line = 102110;
				await ret.AddReturnedValueAsync(depots, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 488);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(depots, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 491);
					ILogger hesabYarLogger30 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger30 != null)
					{
						hesabYarLogger30.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک انبارها در ساعت " + DateTime.Now.ToShortTimeString();
					masterUi.Description = "آغاز سینک مالیات کالا در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo tax = await TaxBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5);
				await ret.AddReturnedValueAsync(tax, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 498);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(tax, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 499);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 500);
				}
				tax.throwExceptionIfError();
				if (masterUi != null)
				{
					ILogger hesabYarLogger31 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger31 != null)
					{
						hesabYarLogger31.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک مالیات کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger32 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger32 != null)
					{
						hesabYarLogger32.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک مالیات کالا در ساعت " + DateTime.Now.ToShortTimeString();
				}
				ReturnedSaveFuncInfo priceLevel = await ProductPriceLevelBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5);
				await ret.AddReturnedValueAsync(priceLevel, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 511);
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(priceLevel, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 512);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 513);
				}
				priceLevel.throwExceptionIfError();
				if (masterUi != null)
				{
					ILogger hesabYarLogger33 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger33 != null)
					{
						hesabYarLogger33.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک سطح قیمت کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger34 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger34 != null)
					{
						hesabYarLogger34.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "آغاز سینک سطح قیمت کالا در ساعت " + DateTime.Now.ToShortTimeString();
				}
				products7 = await ProductBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 524);
				}
				line = 10211;
				await ret.AddReturnedValueAsync(products7, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 526);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 527);
				}
				products7.throwExceptionIfError();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 529);
				}
				ret.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(products7, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 533);
					ILogger hesabYarLogger35 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger35 != null)
					{
						hesabYarLogger35.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک کالا در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger36 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger36 != null)
					{
						hesabYarLogger36.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				line = 10213;
				if (products7.HasError)
				{
					throw new Exception("فکر کنم ارسال کالا ها به سرور تایم اوت داره\r\n" + $"TokenIsCancell:{token.IsCancellationRequested}\r\b" + products7.GetErrorMessage());
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await PrdSerialBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 545);
				}
				returnedSaveFuncInfo.throwExceptionIfError();
				if (masterUi != null)
				{
					await masterUi.Result.AddReturnedValueAsync(products7, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 549);
					ILogger hesabYarLogger37 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger37 != null)
					{
						hesabYarLogger37.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
					masterUi.Description = "اتمام سینک سریال کالاها در ساعت " + DateTime.Now.ToShortTimeString();
					ILogger hesabYarLogger38 = LogCache.Instance.Value.GetHesabYarLogger();
					if (hesabYarLogger38 != null)
					{
						hesabYarLogger38.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
					}
				}
				if (token.IsCancellationRequested || IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 554);
				}
				line = 1050;
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
				await returnedSaveFuncInfo2.AddReturnedValueAsync(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => OrderBusiness.ResendNotSentsAsync(cache.ContextGuid, token, tranName, 5), token), Task.Run(() => OrderBusiness.ResendTempsNotSentsAsync(cache.ContextGuid, token, tranName, 5), token), Task.Run(() => VisitBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token), Task.Run(() => CallingLogBusiness.ResendNotSentsAsync(cache.ContextGuid, token, tranName, 20), token), Task.Run(() => ChequeMOperationBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token), Task.Run(() => ChequeShOperationBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token), Task.Run(() => ContactUsBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token), Task.Run(() => SanadBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token), Task.Run(() => CouponDiscountBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 10), token), Task.Run(() => SanadPrintBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 10), token), Task.Run(() => TaxOrderLogBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), token), Task.Run(() => DescBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), token), Task.Run(() => PrdAnbarGardaniBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 20), token)), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 557);
				line = 140;
			}
			if (masterUi != null)
			{
				masterUi.Description = "آغاز سینک آخرین قیمت کالا در ساعت " + DateTime.Now.ToShortTimeString();
			}
			ReturnedSaveFuncInfo lastPrice = await LastPriceBusiness.ResendNotSentsAsync(cache.ContextGuid, EnSystem.SignalR, token, "", 20);
			line = 10211;
			await ret.AddReturnedValueAsync(lastPrice, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 578);
			ret.throwExceptionIfError();
			if (masterUi != null)
			{
				await masterUi.Result.AddReturnedValueAsync(lastPrice, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 581);
			}
			line = 10212;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 583);
			}
			line = 10213;
			if (masterUi != null)
			{
				masterUi.Description = "اتمام سینک آخرین قیمت کالا در ساعت " + DateTime.Now.ToShortTimeString();
			}
			line = 1030;
			ReturnedSaveFuncInfo checkM = await ViewCheckMBusiness.ResendNotSentAsync(cache.ContextGuid, token, "", 20);
			await ret.AddReturnedValueAsync(checkM, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 589);
			ret.throwExceptionIfError();
			if (masterUi != null)
			{
				await masterUi.Result.AddReturnedValueAsync(checkM, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 591);
			}
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 592);
			}
			ReturnedSaveFuncInfo checkSh = await ViewCheckShBusiness.ResendNotSentAsync(cache.ContextGuid, token, "", 20);
			await ret.AddReturnedValueAsync(checkSh, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 595);
			ret.throwExceptionIfError();
			line = 1695;
			if (masterUi != null)
			{
				await masterUi.Result.AddReturnedValueAsync(checkSh, ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 599);
			}
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 600);
			}
			if (masterUi != null)
			{
				masterUi.EndSync = DateTime.Now;
				ILogger hesabYarLogger39 = LogCache.Instance.Value.GetHesabYarLogger();
				if (hesabYarLogger39 != null)
				{
					hesabYarLogger39.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
				}
				ILogger hesabYarLogger40 = LogCache.Instance.Value.GetHesabYarLogger();
				if (hesabYarLogger40 != null)
				{
					hesabYarLogger40.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
				}
			}
			line = 1610;
			products7 = ret;
			await products7.AddReturnedValueAsync(await FileInfoBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 610);
			ret.throwExceptionIfError();
			line = 1611;
			if (cache.Configs.Any((EnCacheConfig p) => p == EnCacheConfig.Accounting))
			{
				line = 1612;
				products7 = ret;
				await products7.AddReturnedValueAsync(await SanadStatusReportBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 618);
				ret.throwExceptionIfError();
				line = 1613;
				products7 = ret;
				await products7.AddReturnedValueAsync(await VisitorAreaBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 622);
				ret.throwExceptionIfError();
			}
			if (masterUi != null)
			{
				ILogger hesabYarLogger41 = LogCache.Instance.Value.GetHesabYarLogger();
				if (hesabYarLogger41 != null)
				{
					hesabYarLogger41.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
				}
				ILogger hesabYarLogger42 = LogCache.Instance.Value.GetHesabYarLogger();
				if (hesabYarLogger42 != null)
				{
					hesabYarLogger42.Debug($"SendDataToServerAsync.line:{line},MasterUI:{masterUi.Description}");
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return ret.AddError(system, "WebServiceRepository has been disposed!", "IsDisposed", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 630);
			}
			line = 164;
			ret.throwExceptionIfError();
			products7 = ret;
			await products7.AddReturnedValueAsync(await LkInfoBusiness.ResendNotSentsAsync(_cache.ContextGuid, token, "", 20), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 633);
			if (cache.Configs.Any((EnCacheConfig p) => p == EnCacheConfig.Attendance))
			{
				products7 = ret;
				await products7.AddReturnedValueAsync(await AttDepartmentBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 637);
				products7 = ret;
				await products7.AddReturnedValueAsync(await AttPersonLeaveBusiness.ResendNotSentsAsync(cache.ContextGuid, token, "", 5), ReturnedState.None, "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 638);
			}
		}
		catch (OperationCanceledException exception)
		{
			await ret.AddReturnedValueAsync(exception, "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 641);
		}
		catch (Exception ex)
		{
			await ret.AddReturnedValueAsync(ex, "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 644);
			if (!ex.Message.Contains("token is Cancelled"))
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" ?? "", "SendDataToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 646);
			}
		}
		return ret;
	}

	internal void Initialize(ClsCache cache)
	{
		using (new JaegerMonitor("Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs"))
		{
			int num = 0;
			try
			{
				if (cache.Source == ENSource.Accounting || cache.Source == ENSource.Baje || cache.Source == ENSource.Android_HesabYar || cache.Source == ENSource.Android || cache.Source == ENSource.Android_SefareshGir || cache.Source == ENSource.Department || cache.Source == ENSource.WebSite)
				{
					num = 1;
					if (cache.CacheRepository.IHyperLink != null)
					{
						hyperLink = new HyperLinkWebServiceRepository(cache);
					}
					num = 2;
					if (cache.CacheRepository.ITafsil != null)
					{
						HesabTree = new HesabTreeWebServiceRepository(cache);
					}
					num = 300;
					if (cache.CacheRepository.INews != null)
					{
						news = new NewsWebServiceRepository(cache);
					}
					num = 4;
					if (cache.CacheRepository.IContact != null)
					{
						ContactUs = new ContactUsWebServiceRepository(cache);
					}
					num = 5;
					Sync = new SyncWebServiceRepository(cache);
					num = 6;
					if (cache.CacheRepository.IDepot != null)
					{
						depot = new DepotWebServiceRepository(cache);
					}
					num = 7;
					if (cache.CacheRepository.IProductGroup != null)
					{
						productGroups = new ProductGroupWebServiceRepository(cache);
					}
					num = 8;
					if (cache.CacheRepository.IProductVahed != null)
					{
						productVahed = new ProductVahedWebServiceRepository(cache);
					}
					num = 9;
					if (cache.CacheRepository.IProduct != null)
					{
						Products = new ProductWebServiceRepository(cache);
					}
					num = 10;
					if (cache.CacheRepository.ICustomerGroup != null)
					{
						customerGroup = new CustomerGroupWebServiceRepository(cache);
					}
					num = 11;
					if (cache.CacheRepository.ICustomerTitle != null)
					{
						customerTitle = new CustomerTitleWebServiceRepository(cache);
					}
					num = 12;
					if (cache.CacheRepository.ICustomer != null)
					{
						Customers = new CustomerWebServiceRepository(cache);
					}
					num = 13;
					if (cache.CacheRepository.IUserInformation != null)
					{
						UserInfo = new UserInfoWebServiceRepository(cache);
					}
					num = 14;
					if (cache.CacheRepository.IFileInfo != null)
					{
						fileInfo = new FileInfoWebServiceRepository(cache);
					}
					num = 15;
					if (cache.CacheRepository.IReception != null)
					{
						Reception = new ReceptionWebServiceRepository(cache);
					}
					num = 16;
					if (cache.CacheRepository.IPardakht != null)
					{
						Pardakht = new PardakhtWebServiceRepository(cache);
					}
					num = 17;
					if (cache.CacheRepository.IOrder != null)
					{
						Order = new OrderWebServiceRepository(cache);
					}
					num = 18;
					if (cache.CacheRepository.IGps != null)
					{
						gps = new GpsWebServiceRepository(cache);
					}
					num = 19;
					delivery = new DeliveryWebServiceRepository(cache);
					num = 20;
					if (cache.CacheRepository.IVisit != null)
					{
						visit = new VisitWebServiceRepository(cache);
					}
					num = 21;
					if (cache.CacheRepository.IExecuteQuery != null)
					{
						ExecuteQuery = new ExecuteQueryWebServiceRepository(cache);
					}
					num = 22;
					if (cache.CacheRepository.AghsatShRepository != null)
					{
						AghsatShakhsi = new AghsatShakhsiWebServiceRepository(cache);
					}
					num = 23;
					if (cache.CacheRepository.ICallingLog != null)
					{
						CallingLog = new CallingLogWebServiceRepository(cache);
					}
					num = 24;
					if (cache.CacheRepository.AghsatMRepository != null)
					{
						AghsatAshkhas = new AghsatAshkhasWebServiceRepository(cache);
					}
					num = 25;
					if (cache.CacheRepository.IViewCheckMRepository != null)
					{
						ViewCheckM = new ViewCheckMWebServiceRepository(cache);
					}
					num = 26;
					if (cache.CacheRepository.IViewCheckShRepository != null)
					{
						ViewCheckSh = new ViewCheckShWebServiceRepository(cache);
					}
					num = 27;
					if (cache.CacheRepository.ITafsilGroup != null)
					{
						TafsilLevel = new TafsilLevelWebServiceRepository(cache);
					}
					num = 28;
					if (cache.CacheRepository.LastPriceRepository != null)
					{
						LastPrice = new LastPriceWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ITafsil != null)
					{
						Tafsil = new TafsilWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IMoein != null)
					{
						Moein = new MoeinWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IKol != null)
					{
						Kol = new KolWebServiceRepository(cache);
					}
					if (cache.CacheRepository.CashiersLogRepository != null)
					{
						UserLog = new CashierLogWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ChequeMOperation != null)
					{
						ChequeM = new ChequeMOperationWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ChequeShOperation != null)
					{
						ChequeSh = new ChequeShOperationWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ISanad != null)
					{
						Sanad = new SanadWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IPrdAnbarGardani != null)
					{
						AnbarGardani = new PrdAnbarGardaniWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IPrdSerial != null)
					{
						Serial = new PrdSerialWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IntroductionMethod != null)
					{
						IntroductionMethod = new IntroductionMethodWebServiceRepository(cache);
					}
					if (cache.CacheRepository.Account != null)
					{
						Account = new AccountWebServiceRepository(cache);
					}
					if (cache.CacheRepository.CustomerRole != null)
					{
						CustomerRole = new CustomerRoleWebServiceRepository(cache);
					}
					if (cache.CacheRepository.NoteRepository != null)
					{
						Note = new NoteWebServiceRepository(cache);
					}
					if (cache.CacheRepository.CouponDiscount != null)
					{
						DiscountCoupon = new DiscountCouponWebServiceRepository(cache);
					}
					if (cache.CacheRepository.Tax != null)
					{
						Tax = new TaxWebServiceRepository(cache);
					}
					if (cache.CacheRepository.AghsatMGroupRepository != null)
					{
						AghsatMGroup = new AghsatMGroupWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ProductPriceLevel != null)
					{
						ProductPrice = new ProductPriceLevelWebServiceRepository(cache);
					}
					if (cache.CacheRepository.SanadStatusReport != null)
					{
						SanadStatus = new SanadStatusReportWebServiceRepository(cache);
					}
					if (cache.CacheRepository.ISanaDPrint != null)
					{
						SanadPrint = new SanadPrintWebServiceRepository(cache);
					}
					num = 2400;
					Desc = new DescWebServiceRepository(cache);
					SyncEntity = new SyncEntityWebServiceRepository(cache);
					TaxService = new TaxServiceWebServiceRepository(cache);
					num = 29;
					InitTickets(cache);
				}
				else if (cache.Source == ENSource.Advertise)
				{
					num = 30;
				}
				else if (cache.Source == ENSource.TimeAttendance)
				{
					num = 31;
					if (cache.CacheRepository.IUserInformation != null)
					{
						UserInfo = new UserInfoWebServiceRepository(cache);
					}
					if (cache.CacheRepository.IAttPersonLeave != null)
					{
						PersonLeave = new PersonLeaveWebServiceRepository(cache);
					}
					if (cache.CacheRepository.BioMetricData != null)
					{
						Biometric = new BiometricDataWebServiceRepository(cache);
					}
				}
				else if (cache.Source == ENSource.NPBackupService)
				{
					num = 300;
				}
				else if (cache.Source == ENSource.WebNP)
				{
					num = 32;
					InitTickets(cache);
					if (cache.CacheRepository.IUserInformation != null)
					{
						UserInfo = new UserInfoWebServiceRepository(cache);
					}
					num = 33;
					if (cache.CacheRepository.IFileInfo != null)
					{
						fileInfo = new FileInfoWebServiceRepository(cache);
					}
					num = 34;
				}
				else
				{
					WebErrorLog.Instance.StartLog($"برای این نوع تعریفی انجام نشده//source:{cache.Source}//initialization returned!", "", "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 766);
				}
				num = 35;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{num}//cache is null:{cache == null}", "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 769);
			}
		}
	}

	private void InitTickets(ClsCache cache)
	{
		Ticket = new TicketWebServiceRepository(cache);
		TicketDet = new TicketDetWebServiceRepository(cache);
		TicketPart = new TicketPartWebServiceRepository(cache);
	}

	public void Dispose()
	{
		int num = 0;
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_syncToken?.Cancel();
				_syncToken?.Dispose();
				_syncToken = null;
				hyperLink?.Dispose();
				hyperLink = null;
				HesabTree?.Dispose();
				HesabTree = null;
				news?.Dispose();
				news = null;
				depot?.Dispose();
				depot = null;
				productVahed?.Dispose();
				productVahed = null;
				productGroups?.Dispose();
				productGroups = null;
				Products?.Dispose();
				Products = null;
				customerTitle?.Dispose();
				customerTitle = null;
				customerGroup?.Dispose();
				customerGroup = null;
				Customers?.Dispose();
				Customers = null;
				Sync?.Dispose();
				Sync = null;
				ContactUs?.Dispose();
				ContactUs = null;
				UserInfo?.Dispose();
				UserInfo = null;
				fileInfo?.Dispose();
				fileInfo = null;
				fileInfo = null;
				Reception?.Dispose();
				Reception = null;
				Pardakht?.Dispose();
				Pardakht = null;
				Order?.Dispose();
				Order = null;
				delivery?.Dispose();
				delivery = null;
				gps?.Dispose();
				gps = null;
				visit?.Dispose();
				visit = null;
				ExecuteQuery?.Dispose();
				ExecuteQuery = null;
				AghsatAshkhas?.Dispose();
				AghsatAshkhas = null;
				AghsatMGroup?.Dispose();
				AghsatMGroup = null;
				AghsatShakhsi?.Dispose();
				AghsatShakhsi = null;
				CallingLog?.Dispose();
				CallingLog = null;
				ViewCheckM?.Dispose();
				ViewCheckM = null;
				ViewCheckSh?.Dispose();
				ViewCheckSh = null;
				TafsilLevel?.Dispose();
				TafsilLevel = null;
				LastPrice?.Dispose();
				LastPrice = null;
				Kol?.Dispose();
				Kol = null;
				Moein?.Dispose();
				Moein = null;
				Tafsil?.Dispose();
				Tafsil = null;
				UserLog?.Dispose();
				UserLog = null;
				Ticket?.Dispose();
				Ticket = null;
				TicketDet?.Dispose();
				TicketDet = null;
				TicketPart?.Dispose();
				TicketPart = null;
				ChequeM?.Dispose();
				ChequeM = null;
				ChequeSh?.Dispose();
				ChequeSh = null;
				Sanad?.Dispose();
				Sanad = null;
				AnbarGardani?.Dispose();
				AnbarGardani = null;
				Account?.Dispose();
				Account = null;
				TaxService?.Dispose();
				TaxService = null;
				DiscountCoupon?.Dispose();
				DiscountCoupon = null;
				Tax?.Dispose();
				Tax = null;
				ProductPrice?.Dispose();
				ProductPrice = null;
				SanadStatus?.Dispose();
				SanadStatus = null;
				_cache.BufferLoggedIn -= Buffer_LoggedIn;
				_cache?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $" The Error Line IS :{num}//_cache.Buffer is null:{_cache?.BufferEnable ?? false}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebServiceHandlers.cs", 936);
		}
	}
}
