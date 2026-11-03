using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Customer;

public class CustomerMapper : IItemAsyncMapper<CusBusiness, WebCus>, IItemAsyncMapper<CusBusiness, CusBusiness>, IItemAsyncMapper<WebCus, CusBusiness>, IItemAsyncMapper<CusBusiness, CusCompactBusiness>, IItemAsyncMapper<CusCompactBusiness, CusCompactBusiness>, IItemAsyncMapper<WebCusCompact, CusCompactBusiness>, IItemAsyncMapper<CusCompactBusiness, WebCusCompact>, IItemAsyncMapper<CustomerMetaDataBusiness, WebCustomerMetaData>, IItemAsyncMapper<WebCustomerMetaData, CustomerMetaDataBusiness>
{
	public static CustomerMapper Instance { get; private set; } = new CustomerMapper();


	public async Task<WebCus> MapAsync(string contextGuid, CusBusiness source, WebCus destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			line = 5;
			await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, source.Guid, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs");
			if (destination == null)
			{
				destination = new WebCus();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Family = source.Family;
			line = 10;
			destination.Status = source.Status;
			WebCus webCus = destination;
			webCus.LongCode = await source.GetCodeAsync(EnSystem.Mapper, tranName, token);
			destination.Special = source.Special;
			destination.PicFace = source.PicFace;
			line = 15;
			destination.PicSign = source.PicSign;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.CodeMeli = source.CodeMeli;
			line = 20;
			destination.Company = source.Company;
			destination.Date = source.Date;
			destination.Description = source.Description;
			destination.EghtesadiNo = source.EghtesadiNo;
			line = 25;
			destination.Etebar = source.Etebar;
			destination.EtebarCheck = source.EtebarCheck;
			destination.IdPath = source.IdPath;
			destination.AreaGuid = source.AreaGuid;
			destination.IsBazaryab = source.IsBazaryab;
			line = 30;
			destination.IsSupervisor = source.IsSupervisor;
			destination.Job = source.Job;
			destination.Location = source.Location;
			destination.Password = source.Password;
			destination.RSTR9 = source.Email;
			line = 50;
			destination.SupervisorGuid = source.SupervisorGuid;
			destination.FullName = source.FullName;
			destination.User = source.User;
			destination.Zip = source.Zip;
			destination.CustType = source.CustType;
			line = 55;
			destination.TafsilLevel = source.TafsilLevel;
			destination.GroupGuid = source.GroupGuid;
			webCus = destination;
			webCus.VisitorGuid = await source.GetVisitorGuidAsync(null, token, tranName);
			destination.TitleGuid = source.TitleGuid;
			destination.ServerStatus = source.ServerStatus;
			line = 60;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			line = 65;
			destination.BPercent = source.BPercent;
			destination.InPlacePayment = source.InPlacePayment;
			destination.IsEtebari = source.IsEtebari;
			destination.TaxType = source.TaxType;
			destination.Version = new ClientConfig().ClientVersion;
			destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
			destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
			destination.RoleGuid = source.RoleGuid;
			destination.ClientVersion = 2;
			destination.WpRoleName = (await source.GetRoleAsync(token, tranName))?.WordPressName ?? "";
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"CusBusiness=>web//Line:{line}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 108);
			throw;
		}
		return destination;
	}

	public Task<List<WebCus>> MapListAsync(string contextGuid, IEnumerable<CusBusiness> source, List<WebCus> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<WebCus>>(null);
	}

	public async Task<CusBusiness> MapAsync(string contextGuid, CusBusiness source, CusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			line = 5;
			if (destination == null)
			{
				destination = new CusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			line = 10;
			destination.Name = source.Name;
			destination.Family = source.Family;
			destination.Status = source.Status;
			destination.IdPath = source.IdPath;
			line = 15;
			destination.PicFace = source.PicFace;
			destination.PicSign = source.PicSign;
			destination.Address1 = source.Address1;
			line = 20;
			destination.Address2 = source.Address2;
			destination.CodeMeli = source.CodeMeli;
			destination.Company = source.Company;
			destination.Date = source.Date;
			line = 25;
			destination.Description = source.Description;
			destination.EghtesadiNo = source.EghtesadiNo;
			destination.Etebar = source.Etebar;
			destination.EtebarCheck = source.EtebarCheck;
			line = 30;
			destination.AreaGuid = source.AreaGuid;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			destination.Job = source.Job;
			destination.Location = source.Location;
			line = 35;
			destination.Password = source.Password;
			destination.Email = source.Email;
			line = 50;
			destination.SupervisorGuid = source.SupervisorGuid;
			destination.FullName = source.FullName;
			destination.User = source.User;
			destination.Zip = source.Zip;
			destination.CustType = source.CustType;
			line = 55;
			destination.TafsilLevel = source.TafsilLevel;
			destination.GroupGuid = source.GroupGuid;
			CusBusiness cusBusiness = destination;
			cusBusiness.SetVisitorGuid(await source.GetVisitorGuidAsync(null, token, tranName));
			destination.TitleGuid = source.TitleGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			line = 60;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			line = 65;
			destination.BPercent = source.BPercent;
			destination.InPlacePayment = source.InPlacePayment;
			destination.IsEtebari = source.IsEtebari;
			destination.TaxType = source.TaxType;
			destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
			destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
			destination.RoleGuid = source.RoleGuid;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"CusBusiness=>CusBusiness//Line:{line}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 189);
			throw;
		}
		return destination;
	}

	public Task<List<CusBusiness>> MapListAsync(string contextGuid, IEnumerable<CusBusiness> source, List<CusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<CusBusiness>>(null);
	}

	public async Task<CusBusiness> MapAsync(string contextGuid, WebCus source, CusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			line = 5;
			if (destination == null)
			{
				destination = new CusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			line = 100;
			destination.Family = source.Family;
			line = 101;
			destination.IdPath = source.IdPath;
			destination.Status = source.Status;
			line = 102;
			ClsCache cache = ClsCache.GetCache(contextGuid);
			line = 103;
			if (cache.Source == ENSource.Accounting)
			{
				line = 104;
				TafsilBusiness tempTafsilByCode = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, source.LongCode, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs");
				line = 105;
				if (tempTafsilByCode != null && tempTafsilByCode.Guid != source.Guid)
				{
					line = 106;
					CusGroupBusiness cusGroupBusiness = await CusGroupBusiness.GetAsync(contextGuid, source.GroupGuid, token, tranName);
					line = 107;
					ReturnedSaveFuncInfoWithValue<long> nextCode = await TafsilBusiness.NextCodeInGroupAsync(contextGuid, EnSystem.None, cusGroupBusiness.TafsilGroupGuid, token, tranName);
					line = 108;
					nextCode.throwExceptionIfError();
					line = 109;
					await destination.SetCodeAsync(EnSystem.Mapper, nextCode.value, tranName, token);
					line = 110;
					WebErrorLog.Instance.StartLog("این یک خطا نیست\r\nدر مپ از WebCus به CusBusiness کد:\r\n" + $"{source.LongCode}\r\n" + "با حساب تفصیلی:\r\n" + $"Name:{tempTafsilByCode.Name}/Guid:{tempTafsilByCode.Guid}\r\n" + "تداخل داشت به همین دلیل کد حساب را تغییر دادم..\r\n" + $"NewCode:{nextCode.value}/CurrentCusGuid:{source.Guid}/CusName:{source.Name}/CusFamily:{source.Family}/RequstedCusCode:{source.LongCode}", "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 237);
				}
				else
				{
					line = 112;
					await destination.SetCodeAsync(EnSystem.Mapper, source.LongCode, tranName, token);
				}
				line = 113;
			}
			line = 114;
			line = 115;
			destination.PicFace = source.PicFace;
			line = 15;
			destination.PicSign = source.PicSign;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.CodeMeli = source.CodeMeli;
			destination.Company = source.Company;
			line = 20;
			destination.Date = source.Date;
			destination.Description = source.Description;
			destination.EghtesadiNo = source.EghtesadiNo;
			destination.Etebar = source.Etebar;
			line = 25;
			destination.EtebarCheck = source.EtebarCheck;
			if (source.AreaGuid == Guid.Empty)
			{
				source.AreaGuid = null;
			}
			destination.AreaGuid = source.AreaGuid;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			line = 30;
			destination.Job = source.Job;
			destination.Location = source.Location;
			destination.Password = source.Password;
			destination.Email = source.RSTR9;
			destination.SupervisorGuid = source.SupervisorGuid;
			destination.FullName = source.FullName;
			line = 50;
			destination.User = source.User;
			destination.Zip = source.Zip;
			destination.CustType = source.CustType;
			destination.TafsilLevel = source.TafsilLevel;
			destination.GroupGuid = source.GroupGuid;
			line = 55;
			destination.SetVisitorGuid(source.VisitorGuid);
			destination.TitleGuid = source.TitleGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			line = 60;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.BPercent = source.BPercent;
			destination.InPlacePayment = source.InPlacePayment;
			line = 65;
			destination.IsEtebari = source.IsEtebari;
			destination.Status = source.Status;
			destination.TaxType = source.TaxType;
			destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
			destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
			destination.RoleGuid = source.RoleGuid;
			new List<CustomerMetaDataBusiness>();
			IEnumerable<CustomerMetaDataBusiness> enumerable = ((source.Version >= 12) ? source.MetaDataList?.Select((WebMetaData p) => new CustomerMetaDataBusiness(contextGuid)
			{
				EntityGuid = source.Guid,
				EntityType = EntityPart.Customer,
				Key = p.Key,
				Value = p.Value,
				DataType = p.DataType,
				ContextGuid = contextGuid
			})?.ToList() : (await CusBusiness.GetMetaDataListAsync(contextGuid, source.Guid, token)));
			if (enumerable != null && enumerable.Any())
			{
				foreach (CustomerMetaDataBusiness item in enumerable)
				{
					destination.AddDet(item.MetaKey, item.Value);
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"WebCus=>business//Line:{line}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 330);
			throw;
		}
		return destination;
	}

	public Task<List<CusBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCus> source, List<CusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<CusBusiness>>(null);
	}

	public async Task<CusCompactBusiness> MapAsync(string contextGuid, CusBusiness source, CusCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			line = 5;
			if (destination == null)
			{
				destination = new CusCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			line = 10;
			destination.Family = source.Family;
			destination.Status = source.Status;
			CusCompactBusiness cusCompactBusiness = destination;
			cusCompactBusiness.Code = await source.GetCodeAsync(EnSystem.Mapper, tranName, token);
			destination.PicFace = source.PicFace;
			line = 15;
			destination.PicSign = source.PicSign;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.Description = source.Description;
			line = 20;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			destination.Zip = source.Zip;
			destination.GroupGuid = source.GroupGuid;
			cusCompactBusiness = destination;
			cusCompactBusiness.VisitorGuid = await source.GetVisitorGuidAsync(null, token, tranName);
			line = 250;
			destination.TitleGuid = source.TitleGuid;
			line = 251;
			destination.IsEtebari = source.IsEtebari;
			line = 2520;
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, source.Guid, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs");
			line = 2521;
			line = 253;
			destination.ServerStatus = source.ServerStatus;
			line = 254;
			destination.TafsilGroupGuid = tafsilBusiness?.TafsilGroupGuid ?? Guid.Empty;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"CusBusiness=>compact// #Line_{line}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 385);
			throw;
		}
		return destination;
	}

	public Task<List<CusCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<CusBusiness> source, List<CusCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<CusCompactBusiness>>(null);
	}

	public Task<CusCompactBusiness> MapAsync(string contextGuid, CusCompactBusiness source, CusCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			num = 5;
			if (destination == null)
			{
				destination = new CusCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			num = 10;
			destination.Family = source.Family;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.PicFace = source.PicFace;
			destination.PicSign = source.PicSign;
			num = 15;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.Description = source.Description;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			num = 20;
			destination.Zip = source.Zip;
			destination.GroupGuid = source.GroupGuid;
			destination.VisitorGuid = source.VisitorGuid;
			destination.TitleGuid = source.TitleGuid;
			num = 25;
			destination.IsEtebari = source.IsEtebari;
			destination.Account = source.Account;
			destination.ServerStatus = source.ServerStatus;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"CusCompactBusiness=>CusCompactBusiness//Line:{num}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 435);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CusCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<CusCompactBusiness> source, List<CusCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<CusCompactBusiness>>(null);
	}

	public Task<CusCompactBusiness> MapAsync(string contextGuid, WebCusCompact source, CusCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CusCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Family = source.Family;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.PicFace = source.PicFace;
			destination.PicSign = source.PicSign;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			destination.GroupGuid = source.GroupGuid;
			destination.VisitorGuid = source.VisitorGuid;
			destination.TitleGuid = source.TitleGuid;
			destination.IsEtebari = source.IsEtebari;
			destination.Account = source.Account;
			destination.ServerStatus = source.ServerStatus;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.Special = source.Special;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.Description = source.Description;
			destination.Zip = source.Zip;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 479);
			throw;
		}
	}

	public Task<List<CusCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCusCompact> source, List<CusCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebCusCompact> MapAsync(string contextGuid, CusCompactBusiness source, WebCusCompact destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCusCompact();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Family = source.Family;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.PicFace = source.PicFace;
			destination.PicSign = source.PicSign;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			destination.GroupGuid = source.GroupGuid;
			destination.VisitorGuid = source.VisitorGuid;
			destination.TitleGuid = source.TitleGuid;
			destination.IsEtebari = source.IsEtebari;
			destination.Account = source.Account;
			destination.ServerStatus = source.ServerStatus;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.Special = source.Special;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.Description = source.Description;
			destination.Zip = source.Zip;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 521);
			throw;
		}
	}

	public Task<List<WebCusCompact>> MapListAsync(string contextGuid, IEnumerable<CusCompactBusiness> source, List<WebCusCompact> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CustomerMetaDataBusiness> MapAsync(string contextGuid, WebCustomerMetaData source, CustomerMetaDataBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CustomerMetaDataBusiness(contextGuid);
			}
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.DataType = source.DataType;
			destination.ContextGuid = source.ContextGuid;
			destination.MetaKey = source.MetaKey;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 548);
			throw;
		}
	}

	public Task<List<CustomerMetaDataBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCustomerMetaData> source, List<CustomerMetaDataBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebCustomerMetaData> MapAsync(string contextGuid, CustomerMetaDataBusiness source, WebCustomerMetaData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCustomerMetaData();
			}
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.DataType = source.DataType;
			destination.ContextGuid = source.ContextGuid;
			destination.MetaKey = source.MetaKey;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerMapper.cs", 575);
			throw;
		}
	}

	public Task<List<WebCustomerMetaData>> MapListAsync(string contextGuid, IEnumerable<CustomerMetaDataBusiness> source, List<WebCustomerMetaData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
