using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.ProductAttrebutes;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class ProductAttributeBusiness : Serializable<ProductAttributeBusiness>, IProductAttribute, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private bool _isDisposed;

	private List<ProductAttributeValueBusiness> _detailList;

	public string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Slug { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

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

	public ProductAttributeBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.ProductAttributeBusiness;

	public IReadOnlyCollection<ProductAttributeValueBusiness> DetailList
	{
		get
		{
			if (_detailList == null)
			{
				_detailList = new List<ProductAttributeValueBusiness>();
			}
			_detailList.RemoveAll((ProductAttributeValueBusiness q) => q?.IsDisposed ?? true);
			return _detailList;
		}
	}

	public event Func<object, Guid, Task> OnItemAdd;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public ProductAttributeBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 3;
				ReturnedSaveFuncInfo result;
				try
				{
					sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductAttributeBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 62);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 63), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 63);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, dis.Token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 65);
							if (system != EnSystem.UserInterface || !ret.HasError)
							{
								ret.throwExceptionIfError();
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ProductAttributeRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 68);
								goto end_IL_0164;
							}
							result = ret;
						}
						goto end_IL_009f;
						end_IL_0164:;
					}
					goto end_IL_008b;
					end_IL_009f:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 72);
					throw;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 78), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 78);
				}
			}
			return ret;
		}
	}

	public static Task<ProductAttributeBusiness> GetAsync(string contextGuid, Guid attribGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRepository.GetAsync(attribGuid, token, tranName);
	}

	public static Task<ProductAttributeBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRepository.GetAsync(name, token, tranName);
	}

	public static Task<IEnumerable<ProductAttributeReportBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRepository.GetAllModelAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar || system == EnSystem.Transfer)
				{
					return ret;
				}
				IEnumerable<ProductAttributeReportBusiness> enumerable = (await GetAllAsync(ContextGuid, true, token, tranName))?.Where((ProductAttributeReportBusiness q) => q.Guid != Guid);
				if (string.IsNullOrEmpty(Name))
				{
					ret.AddError("لطفا نام خصوصیت را وارد نمایید".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 98);
				}
				if (string.IsNullOrEmpty(Slug))
				{
					ret.AddError("لطفا نامک خصوصیت را وارد نمایید".GetTranslate(), "Slug", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 99);
				}
				else
				{
					bool flag = Slug.IsPersianText();
					if (flag)
					{
						ret.AddError("برای نامک نمی توانید از کاراکترهای فارسی استفاده نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 103);
					}
					else if (!flag && Slug.Length > 28)
					{
						ret.AddError("برای نامک فقط باید از 28 کاراکتر استفاده نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 104);
					}
					if (!Slug.ToLower().StartsWith("pa_"))
					{
						Slug = "pa_" + Slug;
					}
				}
				if (Ordinal <= 0)
				{
					ret.AddError("لطفا اولویت نمایش را مشخص نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 107);
				}
				if (enumerable == null || !enumerable.Any())
				{
					return ret;
				}
				if (enumerable.Any((ProductAttributeReportBusiness w) => w.Name == Name))
				{
					ret.AddError("نام خصوصیت تکراری می باشد".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 109);
				}
				if (enumerable.Any((ProductAttributeReportBusiness w) => w.Slug == Slug))
				{
					ret.AddError("نامک خصوصیت تکراری می باشد".GetTranslate(), "Slug", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 110);
				}
				if (enumerable.Any((ProductAttributeReportBusiness q) => q.Ordinal == Ordinal) && system == EnSystem.UserInterface)
				{
					ret.AddError("اولویت نمایش".GetTranslate() + (" " + NumberManager.AdadToHoroof(Ordinal) + " ").GetTranslate() + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 111);
				}
				if (!Status && (DetailList?.Any() ?? false))
				{
					await ret.AddErrorAsync("با توجه به اینکه خصوصیت جاری حاوی ".GetTranslate() + $"{DetailList.Count()}".GetTranslate() + "ویژگی می باشد، لذا امکان حذف وجود ندارد".GetTranslate(), "Slug", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 113);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 117);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 118);
			}
			return ret;
		}
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (ProductAttributeValueBusiness item in _detailList?.ToList() ?? new List<ProductAttributeValueBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 135);
			}
		}
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs"))
		{
			try
			{
				if ((_detailList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (ProductAttributeValueBusiness item in _detailList?.ToList()?.Where((ProductAttributeValueBusiness p) => p == null || p == sender)?.ToList() ?? new List<ProductAttributeValueBusiness>())
					{
						item?.Dispose();
					}
				}
				_detailList?.RemoveAll((ProductAttributeValueBusiness p) => p == null || p == sender);
				if (EventManager.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 155);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task AddDetailAsync(ProductAttributeValueBusiness det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (det == null)
			{
				return Task.CompletedTask;
			}
			if (_detailList == null)
			{
				_detailList = new List<ProductAttributeValueBusiness>();
			}
			det.MasterGuid = Guid;
			det.OnDisposed += Det_OnDisposed;
			_detailList.Add(det);
			if (EventManager.OnItemAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnItemAdd, this, det.Guid, token, 60, "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 173);
		}
		return Task.CompletedTask;
	}

	public async Task AddDetailListAsync(List<ProductAttributeValueBusiness> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (ProductAttributeValueBusiness item in list)
			{
				await AddDetailAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 183);
		}
	}

	public Task RemoveDetailAsync(Guid detGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ProductAttributeValueBusiness productAttributeValueBusiness = _detailList?.SingleOrDefault((ProductAttributeValueBusiness q) => q.Guid == detGuid);
			if (productAttributeValueBusiness == null)
			{
				return Task.CompletedTask;
			}
			productAttributeValueBusiness.Dispose(disposing: true);
			_detailList.Remove(productAttributeValueBusiness);
			if (EventManager.OnItemAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnItemAdd, this, detGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 197);
		}
		return Task.CompletedTask;
	}

	public static Task<short> GetNextOrdinalAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRepository.GetNextOrdinalAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, List<ProductAttributeBusiness> lst, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (ProductAttributeBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 209);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, sendToServer: false, priority, awaitOnSemaphore: true, await4Send: false, 1, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 210);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 215);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 216);
		}
		return ret;
	}

	public static ProductAttributeBusiness GenerateNew(string contextGuid, string name = "", short ordinal = 0, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return new ProductAttributeBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Status = true,
				ChangeStatus = EntityChangeSt.New,
				Name = name,
				Ordinal = ordinal,
				Slug = "pa_" + name.WordPressFixSlugOrSku().Negasht().ToLower()
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNew", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeBusiness.cs", 241);
			throw;
		}
	}
}
