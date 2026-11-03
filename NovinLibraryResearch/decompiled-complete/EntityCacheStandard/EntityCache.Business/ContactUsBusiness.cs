using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class ContactUsBusiness : BaseBusiness<ContactUsBusiness>, IContactUs, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ContactUsBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _adddress;

	private string _codeMeli;

	private string _description;

	private string _email;

	private string _family;

	private string _fax;

	private string _message;

	private string _mob;

	private string _mob2;

	private string _nahveAshnaee;

	private string _name;

	private string _ostan;

	private string _sabegheKari;

	private string _senf;

	private string _shahrestan;

	private string _shoghl;

	private string _subject;

	private string _tahsilat;

	private string _tel;

	private string _tel2;

	private string _webSite;

	private string _zip;

	private string _ip;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public int ReferencedId { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Address
	{
		get
		{
			return _adddress;
		}
		set
		{
			_adddress = value.FixString();
		}
	}

	public string CodeMeli
	{
		get
		{
			return _codeMeli;
		}
		set
		{
			_codeMeli = value.FixString();
		}
	}

	public DateTime? DateBirth { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value.FixString();
		}
	}

	public string Email
	{
		get
		{
			return _email;
		}
		set
		{
			_email = value.FixString();
		}
	}

	public string Family
	{
		get
		{
			return _family;
		}
		set
		{
			_family = value.FixString();
		}
	}

	public string Fax
	{
		get
		{
			return _fax;
		}
		set
		{
			_fax = value.FixString();
		}
	}

	public bool Jensiat { get; set; }

	public string JensiatString
	{
		get
		{
			if (!Jensiat)
			{
				return "خانم";
			}
			return "آقا";
		}
	}

	public string Message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value.FixString();
		}
	}

	public string Mob
	{
		get
		{
			return _mob;
		}
		set
		{
			_mob = value.FixString();
		}
	}

	public string Mob2
	{
		get
		{
			return _mob2;
		}
		set
		{
			_mob2 = value.FixString();
		}
	}

	public string NahveAshnaee
	{
		get
		{
			return _nahveAshnaee;
		}
		set
		{
			_nahveAshnaee = value.FixString();
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value.FixString();
		}
	}

	public string Ostan
	{
		get
		{
			return _ostan;
		}
		set
		{
			_ostan = value.FixString();
		}
	}

	public string SabegheKari
	{
		get
		{
			return _sabegheKari;
		}
		set
		{
			_sabegheKari = value.FixString();
		}
	}

	public string Senf
	{
		get
		{
			return _senf;
		}
		set
		{
			_senf = value.FixString();
		}
	}

	public string Shahrestan
	{
		get
		{
			return _shahrestan;
		}
		set
		{
			_shahrestan = value.FixString();
		}
	}

	public string Shoghl
	{
		get
		{
			return _shoghl;
		}
		set
		{
			_shoghl = value.FixString();
		}
	}

	public string Subject
	{
		get
		{
			return _subject;
		}
		set
		{
			_subject = value.FixString();
		}
	}

	public string Tahsilat
	{
		get
		{
			return _tahsilat;
		}
		set
		{
			_tahsilat = value.FixString();
		}
	}

	public string Tel
	{
		get
		{
			return _tel;
		}
		set
		{
			_tel = value.FixString();
		}
	}

	public string Tel2
	{
		get
		{
			return _tel2;
		}
		set
		{
			_tel2 = value.FixString();
		}
	}

	public TypeContact Type { get; set; }

	public string WebSite
	{
		get
		{
			return _webSite;
		}
		set
		{
			_webSite = value.FixString();
		}
	}

	public string Zip
	{
		get
		{
			return _zip;
		}
		set
		{
			_zip = value.FixString();
		}
	}

	public string Ip
	{
		get
		{
			return _ip;
		}
		set
		{
			_ip = value.FixString();
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ContactUsBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 112);
				}
				List<WebContactUs> list = new List<WebContactUs>();
				List<WebContactUs> list2 = list;
				list2.Add(await ContactUsMapper.Instance.MapAsync(ContextGuid, this, new WebContactUs(), "", token));
				List<Packet> paks = WebContactUs.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 114);
				}
				using ContactUsWaiter waiter = new ContactUsWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ContactUsWaiter"), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 117);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 119);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 122);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 123);
			}
			return ret;
		}
	}

	private Task<WebContactUs> toWebItemAsync(CancellationToken token)
	{
		return ContactUsMapper.Instance.MapAsync(ContextGuid, this, new WebContactUs(), "", token);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, Priority priority, bool sendToServer, bool awaitOnSemaphore = true, int timeout = 60000, short tryCount = 10, string tranName = "", bool await4Send = false)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ContactusBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 142);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 143), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 143);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IContact.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 145);
						if (!dis.Token.IsCancellationRequested)
						{
							goto end_IL_01a2;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 146);
					}
					goto end_IL_00de;
					end_IL_01a2:;
				}
				goto end_IL_00ca;
				end_IL_00de:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 149);
				goto end_IL_00ca;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 152);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 153);
				goto end_IL_00ca;
			}
			return result;
			end_IL_00ca:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 158), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 158);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 161);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 219), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 219);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IContact.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 221);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 225);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 226);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 231), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 231);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, Priority priority, IEnumerable<ContactUsBusiness> lst, bool sendtoServer, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (ContactUsBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 244);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, sendtoServer, awaitOnSemaphore: true, 60000, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 245);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 250);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 251);
		}
		return ret;
	}

	public static Task<IEnumerable<ContactUsBusiness>> GetAllAsync(string contextGuid, bool? status, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IContact.GetAllAsync(status, token, tranName);
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 267), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 267);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IContact.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 269);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 273);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 274);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 279), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 279);
			}
		}
		return ret;
	}

	public static Task<ContactUsBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IContact.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<ContactUsBusiness>> GetAllAsync(string contextGuid, TypeContact type, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IContact.GetAllAsync(type, status, token, tranName);
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IContact.DbCountAsync(status, token, tranName);
	}

	public ContactUsBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IContact;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ContactUsBusiness/SetSaveResultAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 321), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 321);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IContact.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 323);
						goto end_IL_0116;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 320);
					goto end_IL_007d;
					end_IL_0116:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 326);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 329);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 330);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 335), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 335);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<ContactUsBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IContact.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<ContactUsBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.ContactUs;
			try
			{
				return ret;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 414);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}, ClsCache.GetCache(contextGuid): {ClsCache.GetCache(contextGuid) == null}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 417);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ContactUsBusiness.cs", 418);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}
}
