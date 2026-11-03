using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Filters;
using EntityCache.Models.Accounting.Prd;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Records;
using Serilog;

namespace EntityCache.Business;

public class PrdAnbarRelationBusiness : Serializable<PrdAnbarRelationBusiness>, IHasContext, IPrdAnbarRelation, IHasModified
{
	private decimal amount;

	private decimal _min;

	private decimal _max;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public decimal Amount
	{
		get
		{
			return amount.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			amount = Math.Round(value, 3);
		}
	}

	public string ContextGuid { get; set; }

	public bool AmountIsValid => Amount != 0m;

	public string AmountForDisplay
	{
		get
		{
			return Amount.ToString(3);
		}
		set
		{
			Amount = value.ParseToDecimal(0m, 3);
		}
	}

	public Guid AnbarGuid { get; set; }

	public decimal Min
	{
		get
		{
			return _min.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_min = value;
		}
	}

	public decimal Max
	{
		get
		{
			if (_max < Min)
			{
				_max = Min;
			}
			_max = _max.ToString().ParseToDecimal(0m, 3);
			return _max;
		}
		set
		{
			_max = value;
		}
	}

	public Guid PrdGuid { get; set; }

	public string Name
	{
		get
		{
			try
			{
				DepotBusiness depotBusiness = DepotBusiness.Get(ContextGuid, AnbarGuid);
				if (depotBusiness != null && !depotBusiness.Status)
				{
					return null;
				}
				return DepotBusiness.Get(ContextGuid, AnbarGuid)?.Name ?? "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Name", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 54);
				return "";
			}
		}
	}

	[JsonIgnore]
	public bool IsDepotDeleted
	{
		get
		{
			try
			{
				DepotBusiness depotBusiness = DepotBusiness.Get(ContextGuid, AnbarGuid);
				return depotBusiness != null && !depotBusiness.Status;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "IsDepotDeleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 71);
				return false;
			}
		}
	}

	[JsonIgnore]
	public DateTime Modified { get; set; } = DateTime.Now;


	public PrdAnbarRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<PrdAnbarRelationBusiness> GetAsync(string contextGuid, EnSystem system, Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			IEnumerable<PrdAnbarRelationBusiness> enumerable = await GetAllAsync(contextGuid, system, prdGuid, dptGuid, token, tranName);
			if (enumerable != null)
			{
				return enumerable.SingleOrDefault();
			}
			return new PrdAnbarRelationBusiness(contextGuid)
			{
				PrdGuid = prdGuid,
				AnbarGuid = dptGuid
			};
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 91);
			throw;
		}
	}

	public static async Task<IEnumerable<PrdAnbarRelationBusiness>> GetAllAsync(string contextGuid, EnSystem system, Guid? prdGuid, Guid? dptGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetAnbarRelationsAsync(prdGuid, dptGuid, token, tranName), system, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 104);
			throw;
		}
	}

	public static async Task<IEnumerable<PrdAnbarRelationBusiness>> GetAllAsync(string contextGuid, EnSystem system, List<ProductsChanges> list, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<PrdAnbarRelationBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetAllAsync(list, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return await ApplyAccessAsync(contextGuid, enumerable, system, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 120);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdAnbarRelationBusiness> lst, Guid prdGuid, bool updateAmount, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst?.Any() ?? false)
			{
				foreach (PrdAnbarRelationBusiness item in lst)
				{
					item.PrdGuid = prdGuid;
					item.Modified = DateTime.Now;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(updateAmount, awaitOnSemaphore: true, tryCount, tranName, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 134);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 139);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 140);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool updateAmount, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdAnbarRelationBusiness/SaveAsync", 60000, token))
				{
					line = 1;
					if (!dis.Token.IsCancellationRequested)
					{
						line = 2;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 157), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 157);
						}
						line = 3;
						transactionHasBeenCreated = true;
						line = 40;
						token.ThrowIfCancellationRequested();
						line = 41;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarRelation.SaveAsync(this, updateAmount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 163);
						goto end_IL_0122;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 155);
					goto end_IL_0084;
					end_IL_0122:;
				}
				goto end_IL_0073;
				end_IL_0084:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 165);
				goto end_IL_0073;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex, $"#Line_{line}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 171);
				}
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 172);
				goto end_IL_0073;
			}
			return result;
			end_IL_0073:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 177), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 177);
			}
		}
		return ret;
	}

	private static async Task<IEnumerable<PrdAnbarRelationBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<PrdAnbarRelationBusiness> items, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (system == EnSystem.SignalR || system == EnSystem.None || system == EnSystem.Transfer)
			{
				return items;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (currentCashier == null)
			{
				return items;
			}
			if (!currentCashier.AllDepotsAccess)
			{
				items = items?.Where(delegate(PrdAnbarRelationBusiness p)
				{
					Guid anbarGuid = p.AnbarGuid;
					Guid? depotGuid = currentCashier.DepotGuid;
					return anbarGuid == depotGuid;
				})?.ToList();
			}
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 194);
			throw;
		}
	}

	public static async Task<IEnumerable<ProductOrderPointModel>> GetAllProductOrderpointsAsync(string contextGuid, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			IEnumerable<ProductOrderPointModel> ret = await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetAllProductOrderpointsAsync(token, tranName);
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			ProductCompactFilter filter = new ProductCompactFilter
			{
				LoadJustParents = false,
				Sort = (cashierBusiness?.Settings?.Products?.ProductSort).GetValueOrDefault()
			};
			IEnumerable<Guid> products = (await ProductCompactBusiness.GetAllAsync(contextGuid, system, filter, token, tranName))?.Select((ProductCompactBusiness p) => p.Guid) ?? new List<Guid>();
			IEnumerable<Guid> depots = (await DepotBusiness.GetAllAsync(contextGuid, system, null, tranName, token))?.Select((DepotBusiness p) => p.Guid) ?? new List<Guid>();
			return ret.Where((ProductOrderPointModel p) => products.Contains(p.ProductGuid) && depots.Contains(p.DepotGuid))?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllProductOrderpointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 216);
			throw;
		}
	}

	public static Task<decimal> GetNegativeAmountAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetNegativeAmountAsync(token, tranName);
	}

	public static Task<decimal> GetPositiveAmountAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetPositiveAmountAsync(token, tranName);
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[شماره فاکتور]"),
			new SmsTemplateRecord("[وضعیت]"),
			new SmsTemplateRecord("[نام کاربر]"),
			new SmsTemplateRecord("[تاریخ فاکتور]"),
			new SmsTemplateRecord("[کد کالا]"),
			new SmsTemplateRecord("[نام کالا]"),
			new SmsTemplateRecord("[موجودی]")
		};
	}

	public static async Task<string> FillSmsParametersAsync(string contextGuid, OrderBusiness order, List<PrdAnbarRelationBusiness> list, EnSystem system, string message, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		checked
		{
			try
			{
				if (!(list?.Any() ?? false) || order == null)
				{
					return "";
				}
				if (message.Contains("[شماره فاکتور]"))
				{
					string text = message;
					message = text.Replace("[شماره فاکتور]", (await order.GetId2Async(system, "", token)).ToString());
				}
				if (message.Contains("[وضعیت]"))
				{
					if (list.First().Amount < 0m)
					{
						message = message.Replace("[وضعیت]", "منفی");
					}
					else if (list.First().Amount == 0m)
					{
						message = message.Replace("[وضعیت]", "فاقد موجودی");
					}
					else if (list.First().Amount < list.First().Min)
					{
						message = message.Replace("[وضعیت]", "زیرنقطه سفارش");
					}
				}
				if (message.Contains("[نام کاربر]"))
				{
					message = message.Replace("[نام کاربر]", (await CashierBusiness.GetAsync(contextGuid, order.UserAccGuid, token))?.Name ?? "");
				}
				if (message.Contains("[تاریخ فاکتور]"))
				{
					message = message.Replace("[تاریخ فاکتور]", order.GetDate().MiladiToCalture());
				}
				string header = message[..message.IndexOf("[")];
				string footer = "";
				int num = message.LastIndexOf("]");
				if (num < message.Length - 1)
				{
					footer = message.Substring(num + 1);
				}
				string body = message;
				if (!string.IsNullOrEmpty(header))
				{
					body = body.Replace(header, "");
				}
				if (!string.IsNullOrEmpty(footer))
				{
					body = body.Replace(footer, "");
				}
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrEmpty(header))
				{
					sb.AppendLine(header.Replace("\r\n", ""));
				}
				foreach (PrdAnbarRelationBusiness item in list)
				{
					ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(contextGuid, item.PrdGuid, system, null, "", token, true);
					if (productCompactBusiness == null)
					{
						return "";
					}
					if (body.Contains("[کد کالا]"))
					{
						body = body.Replace("[کد کالا]", productCompactBusiness.CodeForDisplay.ToString());
					}
					if (body.Contains("[نام کالا]"))
					{
						body = body.Replace("[نام کالا]", productCompactBusiness.Name);
					}
					if (body.Contains("[موجودی]"))
					{
						body = body.Replace("[موجودی]", item.Amount.ToString().Replace(".", "/"));
					}
					sb.AppendLine(body);
					body = message;
					if (!string.IsNullOrEmpty(header))
					{
						body = body.Replace(header, "");
					}
					if (!string.IsNullOrEmpty(footer))
					{
						body = body.Replace(footer, "");
					}
				}
				if (!string.IsNullOrEmpty(footer))
				{
					sb.AppendLine(footer);
				}
				return sb.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\PrdAnbarRelationBusiness.cs", 288);
				throw;
			}
		}
	}
}
