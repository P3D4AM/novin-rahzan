using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Department;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Department;

public class LkInfoMapper : IItemAsyncMapper<LkInfoBusiness, WebLkInfo>, IItemAsyncMapper<LkInfoBusiness, LkInfoBusiness>, IItemAsyncMapper<WebLkInfo, LkInfoBusiness>, IItemAsyncMapper<WebLkInfo, LKInfo>, IItemAsyncMapper<LKInfo, WebLkInfo>
{
	public static LkInfoMapper Instance { get; private set; } = new LkInfoMapper();


	public Task<WebLkInfo> MapAsync(string contextGuid, LkInfoBusiness source, WebLkInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebLkInfo();
			}
			destination.Guid = source.Guid;
			destination.AccountingPackage = source.AccountingPackage;
			destination.TypeAccounting = source.TypeAccounting;
			destination.HozoorType = source.HozoorType;
			destination.LKSerial = source.LKSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.ExpireDate = source.ExpireDate;
			destination.StartDate = source.StartDate;
			destination.Description = source.Description;
			destination.Tolid = source.Tolid;
			destination.BarCode = source.BarCode;
			destination.Scale = source.Scale;
			destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
			destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
			destination.ScaleCount = source.ScaleCount;
			destination.MaxActivationDays = source.MaxActivationDays;
			destination.Net_Accounting = source.Net_Accounting;
			destination.Net_Hozor = source.Net_Hozor;
			destination.Net_Scales = source.Net_Scales;
			destination.CallerID = source.CallerID;
			destination.FailedDate = source.FailedDate;
			destination.FailedDescription = source.FailedDescription;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.ChangeStatus = source.ChangeStatus;
			destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
			destination.ContextGuid = contextGuid;
			destination.MetaData = source.MetaData;
			destination.SpecialFacilities = source.SpecialFacilities;
			destination.IsOffline = source.IsOffline;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Department\\LkInfoMapper.cs", 67);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<LkInfoBusiness> MapAsync(string contextGuid, LkInfoBusiness source, LkInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new LkInfoBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.AccountingPackage = source.AccountingPackage;
			destination.TypeAccounting = source.TypeAccounting;
			destination.HozoorType = source.HozoorType;
			destination.LKSerial = source.LKSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.ExpireDate = source.ExpireDate;
			destination.StartDate = source.StartDate;
			destination.Description = source.Description;
			destination.Tolid = source.Tolid;
			destination.BarCode = source.BarCode;
			destination.Scale = source.Scale;
			destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
			destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
			destination.ScaleCount = source.ScaleCount;
			destination.MaxActivationDays = source.MaxActivationDays;
			destination.Net_Accounting = source.Net_Accounting;
			destination.Net_Hozor = source.Net_Hozor;
			destination.Net_Scales = source.Net_Scales;
			destination.CallerID = source.CallerID;
			destination.FailedDate = source.FailedDate;
			destination.FailedDescription = source.FailedDescription;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.ChangeStatus = source.ChangeStatus;
			destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
			destination.ContextGuid = contextGuid;
			destination.MetaData = source.MetaData;
			destination.SpecialFacilities = source.SpecialFacilities;
			destination.IsOffline = source.IsOffline;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Department\\LkInfoMapper.cs", 121);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<LkInfoBusiness> MapAsync(string contextGuid, WebLkInfo source, LkInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new LkInfoBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.AccountingPackage = source.AccountingPackage;
			destination.TypeAccounting = source.TypeAccounting;
			destination.HozoorType = source.HozoorType;
			destination.LKSerial = source.LKSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.ExpireDate = source.ExpireDate;
			destination.StartDate = source.StartDate;
			destination.Description = source.Description;
			destination.Tolid = source.Tolid;
			destination.BarCode = source.BarCode;
			destination.Scale = source.Scale;
			destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
			destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
			destination.ScaleCount = source.ScaleCount;
			destination.MaxActivationDays = source.MaxActivationDays;
			destination.Net_Accounting = source.Net_Accounting;
			destination.Net_Hozor = source.Net_Hozor;
			destination.Net_Scales = source.Net_Scales;
			destination.CallerID = source.CallerID;
			destination.FailedDate = source.FailedDate;
			destination.FailedDescription = source.FailedDescription;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.ChangeStatus = source.ChangeStatus;
			destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
			destination.ContextGuid = contextGuid;
			destination.MetaData = source.MetaData;
			destination.SpecialFacilities = source.SpecialFacilities;
			destination.IsOffline = source.IsOffline;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Department\\LkInfoMapper.cs", 175);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebLkInfo> MapAsync(string contextGuid, LKInfo source, WebLkInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebLkInfo();
			}
			destination.Guid = source.Guid;
			destination.AccountingPackage = source.AccountingPackage;
			destination.TypeAccounting = source.TypeAccounting;
			destination.HozoorType = source.HozoorType;
			destination.LKSerial = source.LKSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.ExpireDate = source.ExpireDate;
			destination.StartDate = source.StartDate;
			destination.Description = source.Description;
			destination.Tolid = source.Tolid;
			destination.BarCode = source.BarCode;
			destination.Scale = source.Scale;
			destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
			destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
			destination.ScaleCount = source.ScaleCount;
			destination.MaxActivationDays = source.MaxActivationDays;
			destination.Net_Accounting = source.Net_Accounting;
			destination.Net_Hozor = source.Net_Hozor;
			destination.Net_Scales = source.Net_Scales;
			destination.CallerID = source.CallerID;
			destination.FailedDate = source.FailedDate;
			destination.FailedDescription = source.FailedDescription;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
			destination.ContextGuid = contextGuid;
			destination.MetaData = source.MetaData;
			destination.SpecialFacilities = source.SpecialFacilities;
			destination.IsOffline = source.IsOffline;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Department\\LkInfoMapper.cs", 223);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<LKInfo> MapAsync(string contextGuid, WebLkInfo source, LKInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new LKInfo();
			}
			destination.Guid = source.Guid;
			destination.AccountingPackage = source.AccountingPackage;
			destination.TypeAccounting = source.TypeAccounting;
			destination.HozoorType = source.HozoorType;
			destination.LKSerial = source.LKSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.ExpireDate = source.ExpireDate;
			destination.StartDate = source.StartDate;
			destination.Description = source.Description;
			destination.Tolid = source.Tolid;
			destination.BarCode = source.BarCode;
			destination.Scale = source.Scale;
			destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
			destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
			destination.ScaleCount = source.ScaleCount;
			destination.MaxActivationDays = source.MaxActivationDays;
			destination.Net_Accounting = source.Net_Accounting;
			destination.Net_Hozor = source.Net_Hozor;
			destination.Net_Scales = source.Net_Scales;
			destination.CallerID = source.CallerID;
			destination.FailedDate = source.FailedDate;
			destination.FailedDescription = source.FailedDescription;
			destination.Modified = source.Modified;
			destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
			destination.MetaData = source.MetaData;
			destination.SpecialFacilities = source.SpecialFacilities;
			destination.IsOffline = source.IsOffline;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Department\\LkInfoMapper.cs", 269);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebLkInfo>> MapListAsync(string contextGuid, IEnumerable<LkInfoBusiness> source, List<WebLkInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LkInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<LkInfoBusiness> source, List<LkInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LkInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<WebLkInfo> source, List<LkInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebLkInfo>> MapListAsync(string contextGuid, IEnumerable<LKInfo> source, List<WebLkInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LKInfo>> MapListAsync(string contextGuid, IEnumerable<WebLkInfo> source, List<LKInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
