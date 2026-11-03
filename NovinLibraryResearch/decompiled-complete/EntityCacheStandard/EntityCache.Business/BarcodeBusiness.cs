using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Business;

public class BarcodeBusiness : Serializable<BarcodeBusiness>, IBarcode, IHasContext, IHasChangeStatus
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _barCode;

	private string _name;

	private string _description;

	private Guid? _depotGuid;

	public string BarCode
	{
		get
		{
			return _barCode;
		}
		set
		{
			if (!string.IsNullOrEmpty(_barCode) && _barCode != value)
			{
				throw new Exception("Change barcode value is not valid!");
			}
			_barCode = value?.FixString() ?? "";
		}
	}

	public DateTime CreateDate { get; set; }

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
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
			_name = value?.FixString() ?? "";
		}
	}

	public decimal Price { get; set; }

	[Obsolete("", true)]
	public EnPriceType PriceType { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public decimal Quantity { get; set; }

	public BarcodePriceType Type { get; set; }

	public short Ordinal { get; set; }

	public Guid? DepotGuid
	{
		get
		{
			if (!((_depotGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _depotGuid;
			}
			return null;
		}
		set
		{
			_depotGuid = value;
		}
	}

	public Guid PrdGuid { get; set; }

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public BarcodeBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.DbCountAsync(status, token, tranName);
	}

	public static Task<IEnumerable<BarcodeBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.GetAllAsync(token, tranName);
	}

	public static async Task<IEnumerable<BarcodeBusiness>> GetAllAsync(string contextGuid, string Searchtext, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<BarcodeBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.GetAllAsync(token, tranName);
			if (string.IsNullOrEmpty(Searchtext))
			{
				return enumerable;
			}
			foreach (string item in Searchtext.SplitString())
			{
				enumerable = enumerable.Where((BarcodeBusiness p) => p.BarCode.Contains(item) || p.Name.Contains(item));
			}
			return enumerable;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 67);
			throw;
		}
	}

	public static Task<BarcodeBusiness> GetAsync(string contextGuid, string barcode, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.GetAsync(barcode, token, tranName);
	}

	public static Task<List<BarcodeBusiness>> GetByPrdGuidAsync(string contextGuid, List<Guid> prdGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.GetByPrdGuidAsync(prdGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, List<Guid> prdGuid, CancellationToken token, short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 83), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 83);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.RemoveRangeAsync(prdGuid, tryCount, tranName, token), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 85);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 89);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 90);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 95), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 95);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, string barcode, CancellationToken token, short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 107), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 107);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.RemoveAsync(barcode, tryCount, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 109);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 113);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 114);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 119), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 119);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem System, CancellationToken token, List<BarcodeBusiness> barcodes, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(barcodes?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (BarcodeBusiness barcode in barcodes)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await barcode.SaveAsync(System, token, awaitOnSemaphore, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 134);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(barcodes.Count(), current, barcode.BarCode, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 146);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 147);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(barcodes?.Count() ?? 0, barcodes?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static string CheckBarcode(string input)
	{
		using (new JaegerMonitor("CheckBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs"))
		{
			try
			{
				Regex regex = new Regex("[A-Z]");
				Regex regex2 = new Regex("[\\d]");
				char[] source = "/+-_. ".ToCharArray();
				for (int i = 0; i < input.Length; i++)
				{
					char value = input[i];
					if (!regex2.Match(value.ToString()).Success && !regex.Match(value.ToString()).Success && !source.Contains(value))
					{
						return "";
					}
				}
				return input;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 181);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = PrdGuid;
			if (PrdGuid == Guid.Empty)
			{
				ret.AddError(system, "کالایی به این بارکد اختصاص نیافته است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 190);
			}
			if (Quantity <= 0m)
			{
				ret.AddError(system, "تعداد بارکد صفر یا پایین تر از صفر است لطفا تعداد را مشخص کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 191);
			}
			if (DepotGuid == Guid.Empty)
			{
				ret.AddError(system, "انبار انتخاب نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 192);
			}
			if (BarCode.Length < 3)
			{
				ret.AddReturnedValue((system == EnSystem.UserInterface) ? ReturnedState.Error : ReturnedState.Warning, "بارکد حداقل باید شامل سه حرف باشد".GetTranslate() + "\r\n" + "بارکد وارد شده".GetTranslate() + ":" + BarCode, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 194);
			}
			if (system == EnSystem.UserInterface && (BarCode?.Length ?? 0) == 13 && !BarCode.IsValidEAN13Barcode())
			{
				ret.AddWarning("رقم انتهایی مجاز برای بارکد ".GetTranslate() + "EAN13\r\n" + $"{BarCode.Checksum_EAN13Barcode()}" + "میباشد.".GetTranslate() + "\r\n" + "بارکد وارد شده".GetTranslate() + ":" + BarCode + "\r\n" + "مقدار صحیح بارکد".GetTranslate() + ":" + $" {BarCode.Substring(0, 12)}{BarCode.Checksum_EAN13Barcode()} \r\n" + "https://novinhozor.ir/lastarticle/ean13-barcode", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 198);
			}
			if (CheckBarcode(BarCode) == "")
			{
				ret.AddReturnedValue((system == EnSystem.UserInterface) ? ReturnedState.Error : ReturnedState.Warning, "در بارکد از حروف غیر مجاز استفاده شده".GetTranslate() + "\r\n" + "بارکد".GetTranslate() + ":" + BarCode, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 204);
			}
			BarcodeBusiness barcodeBusiness = await GetAsync(ContextGuid, BarCode, token, tranName);
			if (ChangeStatus == EntityChangeSt.New && barcodeBusiness != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				EnSystem system2 = system;
				string translate = "بارکد".GetTranslate();
				string barCode = BarCode;
				string translate2 = " تکراری به کالای دیگری".GetTranslate();
				ProductCompactBusiness productCompactBusiness = await barcodeBusiness.GetProductAsync(system, tranName);
				returnedSaveFuncInfo.AddError(system2, translate + "(" + barCode + ")" + translate2 + "\r\n" + productCompactBusiness?.Name + "\r\n" + "تخصیص داده شده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 210);
			}
			if (system == EnSystem.UserInterface)
			{
				ProductCompactBusiness productCompactBusiness2 = await ProductCompactBusiness.GetAsync(ContextGuid, BarCode, system, true, tranName, null, token);
				if (productCompactBusiness2 != null && productCompactBusiness2.Guid != PrdGuid)
				{
					await ret.AddErrorAsync("بارکد".GetTranslate() + "(" + BarCode + ")" + "تکراری به کالای دیگری".GetTranslate() + "\r\n " + productCompactBusiness2.Name + "\r\n " + "تخصیص داده شده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 218);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 223);
			throw;
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("BarcodeBusiness Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 229);
			}
		}
		return ret;
	}

	private Task<ProductCompactBusiness> GetProductAsync(EnSystem system, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ProductCompactBusiness.GetAsync(ContextGuid, PrdGuid, system, null, tranName, token, true);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem System, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BarcodeBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 246), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 246);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(System, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 248);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IBarcode.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 250);
						}
						goto end_IL_0121;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 244);
					goto end_IL_007d;
					end_IL_0121:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 254);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 255);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 260), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 260);
			}
		}
		return ret;
	}

	public static Task<BarcodeBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? PrdGuid = null, string description = "", Guid? DepotGuid = null, string tranName = "")
	{
		try
		{
			BarcodeBusiness barcodeBusiness = new BarcodeBusiness(contextGuid)
			{
				Description = (description ?? ""),
				CreateDate = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				Quantity = 1m
			};
			if (PrdGuid.HasValue)
			{
				barcodeBusiness.PrdGuid = PrdGuid.Value;
			}
			if (DepotGuid.HasValue)
			{
				barcodeBusiness.DepotGuid = DepotGuid.Value;
			}
			barcodeBusiness.Price = 0m;
			barcodeBusiness.ChangeStatus = EntityChangeSt.New;
			return Task.FromResult(barcodeBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 283);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FillBarcodesAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "BarcodeBusiness/FillBarcodesAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 296);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 298), ReturnedState.None, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 298);
						}
						if (!res.HasError)
						{
							transactionHasBeenCreated = true;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.FillBarcodesAsync(token, tranName), ReturnedState.None, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 301);
							goto end_IL_010d;
						}
						result = res;
					}
					goto end_IL_0076;
					end_IL_010d:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 305);
				res.AddReturnedValue(ex, "", "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 306);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 311), ReturnedState.None, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 311);
			}
		}
		return res;
	}

	public static Task<IEnumerable<BarcodeModel>> GetAllConflictsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBarcode.GetAllConflictsAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ReOrdinalBarcodesAsync(string contextGuid, EnSystem system, IEnumerable<BarcodeModel> conflictList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (!(conflictList?.Any() ?? false))
				{
					return res;
				}
				List<IGrouping<Guid, BarcodeModel>> list = (from q in conflictList
					group q by q.PrdGuid)?.ToList();
				if (list == null || !list.Any())
				{
					return res;
				}
				List<BarcodeBusiness> lstBarcode = new List<BarcodeBusiness>();
				foreach (IGrouping<Guid, BarcodeModel> item in list)
				{
					if (item == null || !item.Any())
					{
						continue;
					}
					List<BarcodeBusiness> list2 = (await GetByPrdGuidAsync(contextGuid, new List<Guid> { item.Key }, token, tranName))?.OrderBy((BarcodeBusiness q) => q.Ordinal)?.ToList();
					if (list2 == null || !list2.Any())
					{
						return res;
					}
					short num = 1;
					foreach (BarcodeBusiness item2 in list2)
					{
						if (!string.IsNullOrEmpty(item2?.BarCode))
						{
							if (item2.Ordinal == num)
							{
								num++;
								continue;
							}
							item2.Ordinal = num;
							num++;
							lstBarcode.Add(item2);
						}
					}
				}
				if (!(lstBarcode?.Any() ?? false))
				{
					return res;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveRangeAsync(system, token, lstBarcode, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "ReOrdinalBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 349);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateServerStatusAsync(contextGuid, lstBarcode.Select((BarcodeBusiness q) => q.PrdGuid).Distinct().ToList(), ServerStatusForClient.None, token, tranName), ReturnedState.None, "ReOrdinalBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 350);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReOrdinalBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 354);
				res.AddReturnedValue(ex, "", "ReOrdinalBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 355);
			}
			return res;
		}
	}

	public void SetOrdinalChange(short value, List<BarcodeBusiness> otherBarcodes)
	{
		try
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (value < 0)
			{
				throw new ArgumentException($"value({value}) is not valid for SanadDetail.Ordinal");
			}
			short ordinal = Ordinal;
			Ordinal = value;
			if (otherBarcodes != null && otherBarcodes.Any())
			{
				BarcodeBusiness barcodeBusiness = otherBarcodes.FirstOrDefault((BarcodeBusiness q) => q.Ordinal == Ordinal && q.BarCode != BarCode);
				if (barcodeBusiness != null)
				{
					barcodeBusiness.Ordinal = ordinal;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\BarcodeBusiness.cs", 374);
		}
	}
}
