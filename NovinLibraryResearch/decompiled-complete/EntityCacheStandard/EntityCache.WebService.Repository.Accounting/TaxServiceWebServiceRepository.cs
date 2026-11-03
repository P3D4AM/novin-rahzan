using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs.TaxService;
using PacketParser.Filters;
using PacketParser.Records.TaxService;
using PacketParser.Settings.Accounting;

namespace EntityCache.WebService.Repository.Accounting;

public class TaxServiceWebServiceRepository : IDisposable
{
	protected ClsCache Cache;

	private CancellationTokenSource _trackingTaxOrderToken;

	public bool IsDisposed { get; set; }

	public WebContextHolder Context => Cache.Context;

	public string ContextGuid => Context.Guid;

	public event Func<object, TaxServiceResponse, Task> OnTaxResponse;

	public TaxServiceWebServiceRepository(ClsCache cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.TaxLogin.OnLoginResponse += TaxServiceInstance_OnLoginResponse;
			Cache.Context.WebOrder.OnSendToTaxServiceResponse += TaxServiceInstance_OnInvoiceResult;
			Cache.Context.WebOrder.OnTaxServiceTrackResponse += TaxServiceInstance_OnInvoiceTrackedResult;
			Cache.Context.WebOrder.OnTaxEconomicCodeStateResponse += WebOrder_OnTaxEconomicCodeStateResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 37);
		}
	}

	private Task TaxServiceInstance_OnLoginResponse(object sender, TaxLoginEventArgs response)
	{
		try
		{
			if (string.IsNullOrEmpty(response?.Token))
			{
				if (this.OnTaxResponse != null)
				{
					TaxServiceResponse args = new TaxServiceResponse
					{
						Message = (string.IsNullOrEmpty(response?.Message) ? "خطا در برقراری ارتباط".GetTranslate() : response.Message),
						Title = "اتصال به سامانه مودیان".GetTranslate(),
						Type = EnNotification.Error
					};
					PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args, default(CancellationToken), 60, "TaxServiceInstance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
				}
				return Task.CompletedTask;
			}
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(response.TimeOutExpire);
			string text = $"{dateTimeOffset.Hour}:{dateTimeOffset.Minute}";
			if (this.OnTaxResponse != null)
			{
				TaxServiceResponse taxServiceResponse = new TaxServiceResponse();
				taxServiceResponse.Message = "ارتباط با موفقیت برقرار شد ".GetTranslate() + "\r\n " + "توکن در ساعت ".GetTranslate() + " " + (text + " ").GetTranslate() + " " + "منقضی خواهد شد".GetTranslate();
				taxServiceResponse.Title = "اتصال به سامانه مودیان".GetTranslate();
				taxServiceResponse.Type = EnNotification.Ok;
				TaxServiceResponse args2 = taxServiceResponse;
				PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args2, default(CancellationToken), 60, "TaxServiceInstance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
			}
			if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
			{
				_trackingTaxOrderToken?.Cancel();
				_trackingTaxOrderToken = new CancellationTokenSource(60000);
				CancellationToken taxToken = _trackingTaxOrderToken.Token;
				Task.Run(() => TrackingTaxOrdersAsync(taxToken));
				Task.Run((Func<Task?>)CheckEconomicCodeStateAsync);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TaxServiceInstance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 82);
		}
		return Task.CompletedTask;
	}

	private async Task TaxServiceInstance_OnInvoiceResult(object sender, TaxInvoiceResultEventArgs res)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			if (res != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await OrderBusiness.SetTaxResultAsync(ContextGuid, EnSystem.SignalR, res), ReturnedState.None, "TaxServiceInstance_OnInvoiceResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 91);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TaxServiceInstance_OnInvoiceResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 95);
			result.AddReturnedValue(ex, "", "TaxServiceInstance_OnInvoiceResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 96);
		}
		finally
		{
			if (result.HasError && this.OnTaxResponse != null)
			{
				TaxServiceResponse args = new TaxServiceResponse
				{
					Message = result.ErrorMessage,
					Title = "خطا در ارسال صورتحساب".GetTranslate(),
					Type = EnNotification.Error
				};
				PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args, default(CancellationToken), 60, "TaxServiceInstance_OnInvoiceResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
			}
		}
	}

	private async Task TrackingTaxOrdersAsync(CancellationToken token)
	{
		_ = 5;
		try
		{
			if (!((await LocalSettingBusiness.GetSettingAsync(ContextGuid))?.TaxService?.TaxServiceEnable).GetValueOrDefault())
			{
				return;
			}
			int delay = 120000;
			await Task.Delay(30000);
			while (!token.IsCancellationRequested)
			{
				OrderFilter filter = new OrderFilter
				{
					TaxResult = EnTaxResult.Sent,
					FactorType = Document.OrderSell,
					DateTo = DateTime.Now,
					DateFrom = DateTime.Now.AddDays(-700.0)
				};
				IEnumerable<OrderReportBusiness> enumerable = await OrderBusiness.GetAllAsync(ContextGuid, EnSystem.None, filter);
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
				}
				if (enumerable == null || !enumerable.Any())
				{
					await Task.Delay(delay);
					continue;
				}
				await OrderBusiness.TrackingTaxOrderAsync(ContextGuid, enumerable);
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
				}
				await Task.Delay(delay);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TrackingTaxOrdersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 141);
		}
	}

	private async Task TaxServiceInstance_OnInvoiceTrackedResult(object sender, List<TaxInvoiceTrackingEventArgs> res)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			if (!(res?.Any() ?? false))
			{
				if (this.OnTaxResponse != null)
				{
					TaxServiceResponse args = new TaxServiceResponse
					{
						Message = "صورت حساب (های) مورد نظر کماکان در حال بررسی می باشند. لطفا منتظر پاسخ از سمت سامانه مودیان بمانید".GetTranslate(),
						Title = "اتمام رهگیری صورت حساب ها".GetTranslate(),
						Type = EnNotification.Ok
					};
					PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args, default(CancellationToken), 60, "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
				}
				return;
			}
			if (this.OnTaxResponse != null)
			{
				TaxServiceResponse taxServiceResponse = new TaxServiceResponse();
				taxServiceResponse.Message = "تعداد".GetTranslate() + " " + $" {res.Count()} " + " " + "صورت حساب تعیین تکلیف شدند. جهت مشاهده نتیجه، به صفحه لیست فاکتورها مراجعه نمایید".GetTranslate();
				taxServiceResponse.Title = "اتمام رهگیری صورت حساب ها".GetTranslate();
				taxServiceResponse.Type = EnNotification.Ok;
				TaxServiceResponse args2 = taxServiceResponse;
				PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args2, default(CancellationToken), 60, "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
			returnedSaveFuncInfo.AddReturnedValue(await OrderBusiness.SetTaxResultAsync(ContextGuid, EnSystem.SignalR, res), ReturnedState.None, "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 173);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 177);
			result.AddReturnedValue(ex, "", "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 178);
		}
		finally
		{
			if (result.HasError && this.OnTaxResponse != null)
			{
				TaxServiceResponse args3 = new TaxServiceResponse
				{
					Message = result.ErrorMessage,
					Title = "خطا در بروزرسانی وضعیت صورتحساب ها".GetTranslate(),
					Type = EnNotification.Error
				};
				PacketParser.Utilities.RaiseEvent(this.OnTaxResponse, this, args3, default(CancellationToken), 60, "TaxServiceInstance_OnInvoiceTrackedResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs");
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Cache.Context.TaxLogin.OnLoginResponse -= TaxServiceInstance_OnLoginResponse;
				Cache.Context.WebOrder.OnSendToTaxServiceResponse -= TaxServiceInstance_OnInvoiceResult;
				Cache.Context.WebOrder.OnTaxServiceTrackResponse -= TaxServiceInstance_OnInvoiceTrackedResult;
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 207);
		}
	}

	private async Task CheckEconomicCodeStateAsync()
	{
		_ = 1;
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			if ((accGlobalSettings?.TaxService?.TaxServiceEnable).GetValueOrDefault())
			{
				Packet packetData = WebOrder.GenerateTaxEconomicCodeStateRequest(new TaxEconomicCodeRecord(accGlobalSettings.TaxService.ClientId));
				await ClsCache.GetCache(ContextGuid).SendData(packetData, Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckEconomicCodeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 219);
		}
	}

	private async Task WebOrder_OnTaxEconomicCodeStateResponse(object sender, TaxEconomicCodeRecord e)
	{
		try
		{
			await LocalSettingBusiness.SetTaxEconomicCodeRecordAsync(ContextGuid, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebOrder_OnTaxEconomicCodeStateResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TaxServiceWebServiceRepository.cs", 227);
		}
	}
}
