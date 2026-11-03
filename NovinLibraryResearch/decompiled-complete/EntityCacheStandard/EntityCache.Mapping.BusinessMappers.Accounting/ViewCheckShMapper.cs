using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Mapping.BusinessMappers.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ViewCheckShMapper : IItemAsyncMapper<ViewCheckShBusiness, WebViewCheckSh>, IItemAsyncMapper<ViewCheckShBusiness, ViewCheckShBusiness>, IItemAsyncMapper<WebViewCheckSh, ViewCheckShBusiness>
{
	public static ViewCheckShMapper Instance { get; private set; } = new ViewCheckShMapper();


	public async Task<WebViewCheckSh> MapAsync(string contextGuid, ViewCheckShBusiness source, WebViewCheckSh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebViewCheckSh();
			}
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.ChequeStatus = source.ChequeStatus;
			destination.DateSarresid = source.DateSarresid;
			destination.BankName = source.BankName;
			destination.ST = source.ST;
			destination.CustomerFullName = source.CustomerFullName;
			destination.PardakhtDate = source.PardakhtDate;
			destination.GuidSanad = source.GuidSanad;
			destination.Aval2re = source.Aval2re;
			destination.DateSarresidM = source.DateSarresidM;
			destination.PardakhtDesc = source.PardakhtDesc;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.BankGuid = source.BankGuid;
			destination.A2reMoeinGuid = source.A2reMoeinGuid;
			destination.A2reTafsilGuid = source.A2reTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.Version = 1;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateUtc;
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				WebViewCheckSh webViewCheckSh = destination;
				webViewCheckSh.OperationList = await ChequeShOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<WebChequeShOperation>(), tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewCheckShMapper.cs", 66);
			throw;
		}
	}

	public Task<List<WebViewCheckSh>> MapListAsync(string contextGuid, IEnumerable<ViewCheckShBusiness> source, List<WebViewCheckSh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ViewCheckShBusiness> MapAsync(string contextGuid, ViewCheckShBusiness source, ViewCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ViewCheckShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.ChequeStatus = source.ChequeStatus;
			destination.BankName = source.BankName;
			destination.CustomerFullName = source.CustomerFullName;
			destination.PardakhtDate = source.PardakhtDate;
			destination.GuidSanad = source.GuidSanad;
			destination.Aval2re = source.Aval2re;
			destination.DateSarresidM = source.DateSarresidM;
			destination.PardakhtDesc = source.PardakhtDesc;
			destination.Modified = source.Modified;
			destination.IsChecked = source.IsChecked;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.BankGuid = source.BankGuid;
			destination.A2reMoeinGuid = source.A2reMoeinGuid;
			destination.A2reTafsilGuid = source.A2reTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateUtc;
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				ViewCheckShBusiness viewCheckShBusiness = destination;
				viewCheckShBusiness.OperationList = await ChequeShOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<ChequeShOperationBusiness>(), tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewCheckShMapper.cs", 119);
			throw;
		}
	}

	public Task<List<ViewCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewCheckShBusiness> source, List<ViewCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ViewCheckShBusiness> MapAsync(string contextGuid, WebViewCheckSh source, ViewCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ViewCheckShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.ChequeStatus = source.ChequeStatus;
			destination.BankName = source.BankName;
			destination.CustomerFullName = source.CustomerFullName;
			destination.PardakhtDate = source.PardakhtDate;
			destination.GuidSanad = source.GuidSanad;
			destination.Aval2re = source.Aval2re;
			destination.DateSarresidM = source.DateSarresidM;
			destination.PardakhtDesc = source.PardakhtDesc;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.BankGuid = source.BankGuid;
			destination.A2reMoeinGuid = source.A2reMoeinGuid;
			destination.A2reTafsilGuid = source.A2reTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateSarresidM.ToUniversalTime();
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				ViewCheckShBusiness viewCheckShBusiness = destination;
				viewCheckShBusiness.OperationList = await ChequeShOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<ChequeShOperationBusiness>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewCheckShMapper.cs", 171);
			throw;
		}
	}

	public Task<List<ViewCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<WebViewCheckSh> source, List<ViewCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
