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

public class ViewChechMMapper : IItemAsyncMapper<ViewCheckMBusiness, WebViewCheckM>, IItemAsyncMapper<ViewCheckMBusiness, ViewCheckMBusiness>, IItemAsyncMapper<WebViewCheckM, ViewCheckMBusiness>
{
	public static ViewChechMMapper Instance { get; private set; } = new ViewChechMMapper();


	public async Task<WebViewCheckM> MapAsync(string contextGuid, ViewCheckMBusiness source, WebViewCheckM destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebViewCheckM();
			}
			destination.Guid = source.Guid;
			destination.BankName = source.BankName;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.STName = source.STName;
			destination.DateSanad = source.DateSanad;
			destination.DateSanadM = source.DateSanadM;
			destination.DateCheck = source.DateCheck;
			destination.DateCheckM = source.DateCheckM;
			destination.DateBargashtORVosol = source.DateBargashtORVosol;
			destination.DateBargashtORVosolM = source.DateBargashtORVosolM;
			destination.DateToPlace = source.DateToPlace;
			destination.Place = source.Place;
			destination.Description = source.Description;
			destination.Number = source.Number;
			destination.GuidSanad = source.GuidSanad;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Personal = source.Personal;
			destination.HesabNum = source.HesabNum;
			destination.CustomerFullName = source.CustomerFullName;
			destination.Aval2re = source.Aval2re;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.VisitorGuid = source.VisitorGuid;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.PardazandeTafsilGuid = source.PardazandeTafsilGuid;
			destination.IsEnteghali = source.IsEnteghali;
			destination.Version = 1;
			destination.Sayadi_Name = source.Sayadi_Name;
			destination.Sayadi_Identity = source.Sayadi_Identity;
			destination.Sayadi_NationalCode = source.Sayadi_NationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateUtc;
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				WebViewCheckM webViewCheckM = destination;
				webViewCheckM.OperationList = await ChequeMOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<WebChequeMOperation>(), tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewChechMMapper.cs", 74);
			throw;
		}
		return destination;
	}

	public Task<List<WebViewCheckM>> MapListAsync(string contextGuid, IEnumerable<ViewCheckMBusiness> source, List<WebViewCheckM> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ViewCheckMBusiness> MapAsync(string contextGuid, ViewCheckMBusiness source, ViewCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ViewCheckMBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.BankName = source.BankName;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.DateSanadM = source.DateSanadM;
			destination.DateCheckM = source.DateCheckM;
			destination.DateBargashtORVosolM = source.DateBargashtORVosolM;
			destination.DateToPlace = source.DateToPlace;
			destination.Place = source.Place;
			destination.Description = source.Description;
			destination.Number = source.Number;
			destination.GuidSanad = source.GuidSanad;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Personal = source.Personal;
			destination.HesabNum = source.HesabNum;
			destination.Aval2re = source.Aval2re;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.VisitorGuid = source.VisitorGuid;
			destination.Modified = source.Modified;
			destination.PardazandeTafsilGuid = source.PardazandeTafsilGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.IsEnteghali = source.IsEnteghali;
			destination.Sayadi_Identity = source.Sayadi_Identity;
			destination.Sayadi_Name = source.Sayadi_Name;
			destination.Sayadi_NationalCode = source.Sayadi_NationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateUtc;
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				ViewCheckMBusiness viewCheckMBusiness = destination;
				viewCheckMBusiness.OperationList = await ChequeMOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<ChequeMOperationBusiness>(), tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewChechMMapper.cs", 134);
			throw;
		}
		return destination;
	}

	public Task<List<ViewCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewCheckMBusiness> source, List<ViewCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ViewCheckMBusiness> MapAsync(string contextGuid, WebViewCheckM source, ViewCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ViewCheckMBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.BankName = source.BankName;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.PardazandeTafsilGuid = source.PardazandeTafsilGuid;
			destination.DateSanadM = source.DateSanadM;
			destination.DateCheckM = source.DateCheckM;
			destination.DateBargashtORVosolM = source.DateBargashtORVosolM;
			destination.DateToPlace = source.DateToPlace;
			destination.Place = source.Place;
			destination.Description = source.Description;
			destination.Number = source.Number;
			destination.GuidSanad = source.GuidSanad;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Personal = source.Personal;
			destination.HesabNum = source.HesabNum;
			destination.Aval2re = source.Aval2re;
			destination.PoshtNomreh = source.PoshtNomreh;
			destination.VisitorGuid = source.VisitorGuid;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.IsEnteghali = source.IsEnteghali;
			destination.Sayadi_Identity = source.Sayadi_Identity;
			destination.Sayadi_Name = source.Sayadi_Name;
			destination.Sayadi_NationalCode = source.Sayadi_NationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.DateUtc = source.DateCheckM.ToUniversalTime();
			if ((source?.OperationList?.Any()).GetValueOrDefault())
			{
				ViewCheckMBusiness viewCheckMBusiness = destination;
				viewCheckMBusiness.OperationList = await ChequeMOperationMapper.Instance.MapListAsync(contextGuid, source.OperationList, new List<ChequeMOperationBusiness>(), tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ViewChechMMapper.cs", 193);
			throw;
		}
		return destination;
	}

	public Task<List<ViewCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<WebViewCheckM> source, List<ViewCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
