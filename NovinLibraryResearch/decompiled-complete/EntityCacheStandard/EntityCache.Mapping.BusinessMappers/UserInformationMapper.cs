using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class UserInformationMapper : IItemAsyncMapper<UserInformationBusiness, WebUserInformation>, IItemAsyncMapper<UserInformationBusiness, UserInformationBusiness>, IItemAsyncMapper<WebUserInformation, UserInformationBusiness>
{
	public static UserInformationMapper Instance { get; private set; } = new UserInformationMapper();


	public async Task<WebUserInformation> MapAsync(string contextGuid, UserInformationBusiness source, WebUserInformation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebUserInformation();
			}
			destination.CacheApiURL = source.CacheApiURL;
			destination.DepId = source.DepId;
			destination.PaymentURL = source.PaymentURL;
			destination.UserName = source.UserName;
			destination.UserEmail = source.UserEmail;
			destination.UserMobile = source.UserMobile;
			destination.UserId = source.UserId;
			destination.CashierGuid = source.CashierGuid;
			destination.UserDepGuid = source.UserDepGuid;
			destination.UserRegisterDateM = source.UserRegisterDateM;
			destination.UserExpireDateM = source.UserExpireDateM;
			destination.RelativePath = source.RelativePath;
			destination.UserDepGuid = source.UserDepGuid;
			destination.GpsEnable = source.GpsEnable;
			destination.SiteUrl = source.SiteUrl;
			destination.SiteType = source.SiteType;
			destination.AndroidPrint = source.AndroidPrint;
			destination.SyncInfo = source.SyncInfo;
			destination.Source = source.Source;
			destination.LastSaved = source.LastSaved;
			destination.CustomerInfo = source.CustomerInfo;
			destination.Order = source.Order;
			destination.AndroidPrintStyles = source.AndroidPrintStyles;
			destination.IsHaveTicketting = source.IsHaveTicketting;
			destination.HesabInfo = source.HesabInfo;
			destination.DepartmentDescription = source.DepartmentDescription;
			destination.MetaDescription = source.MetaDescription;
			destination.MetaKeyword = source.MetaKeyword;
			destination.Script = source.Script;
			destination.ScriptCounter = source.ScriptCounter;
			destination.ScriptEnamad = source.ScriptEnamad;
			destination.ScriptFooter = source.ScriptFooter;
			destination.ScriptHeader = source.ScriptHeader;
			destination.LstSocialNetwork = source.LstSocialNetwork;
			destination.PaymentGatewayRelation = source.PaymentGatewayRelation;
			destination.CurrentServerTime = source.CurrentServerTime;
			destination.HashedPassword = source.HashedPassword;
			destination.LstMediaLinks = source.LstMediaLinks;
			destination.TafsilGuid = source.TafsilGuid;
			destination.DefaultCusGroup = source.DefaultCusGroup;
			destination.UserAccCompacts = source.UserAccCompacts;
			destination.SyncsKolMoeinTafsil = source.SyncsKolMoeinTafsil;
			destination.BajeSettings = source.BajeSettings;
			destination.HesabyarSettings = source.HesabyarSettings;
			if (source.SyncInfo != null)
			{
				WebUserInformation webUserInformation = destination;
				if (webUserInformation.SyncInfo == null)
				{
					webUserInformation.SyncInfo = new Sync();
				}
				await WebContextHolder.GetWebContextHolder(contextGuid).Sync.MapAsync(contextGuid, source.SyncInfo, destination.SyncInfo, tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\UserInformationMapper.cs", 82);
			throw;
		}
	}

	public Task<List<WebUserInformation>> MapListAsync(string contextGuid, IEnumerable<UserInformationBusiness> source, List<WebUserInformation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<UserInformationBusiness> MapAsync(string contextGuid, UserInformationBusiness source, UserInformationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination != null && destination.Version > source.Version)
			{
				return destination;
			}
			if (destination == null)
			{
				destination = new UserInformationBusiness(contextGuid);
			}
			destination.CacheApiURL = source.CacheApiURL;
			destination.DepId = source.DepId;
			destination.PaymentURL = source.PaymentURL;
			destination.UserName = source.UserName;
			destination.UserEmail = source.UserEmail;
			destination.UserMobile = source.UserMobile;
			destination.UserId = source.UserId;
			destination.UserAccGuid = source.UserAccGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.UserDepGuid = source.UserDepGuid;
			destination.UserRegisterDateM = source.UserRegisterDateM;
			destination.UserExpireDateM = source.UserExpireDateM;
			destination.RelativePath = source.RelativePath;
			destination.UserDepGuid = source.UserDepGuid;
			destination.GpsEnable = source.GpsEnable;
			destination.SiteUrl = source.SiteUrl;
			destination.SiteType = source.SiteType;
			destination.AndroidPrint = source.AndroidPrint;
			destination.SyncInfo = source.SyncInfo;
			destination.Source = source.Source;
			destination.LastSaved = source.LastSaved;
			destination.CustomerInfo = source.CustomerInfo;
			destination.Order = source.Order;
			destination.AndroidPrintStyles = source.AndroidPrintStyles;
			destination.IsHaveTicketting = source.IsHaveTicketting;
			destination.HesabInfo = source.HesabInfo;
			destination.DepartmentDescription = source.DepartmentDescription;
			destination.MetaDescription = source.MetaDescription;
			destination.MetaKeyword = source.MetaKeyword;
			destination.Script = source.Script;
			destination.ScriptCounter = source.ScriptCounter;
			destination.ScriptEnamad = source.ScriptEnamad;
			destination.ScriptFooter = source.ScriptFooter;
			destination.ScriptHeader = source.ScriptHeader;
			destination.LstSocialNetwork = source.LstSocialNetwork;
			destination.PaymentGatewayRelation = source.PaymentGatewayRelation;
			destination.CurrentServerTime = source.CurrentServerTime;
			destination.HashedPassword = source.HashedPassword;
			destination.LstMediaLinks = source.LstMediaLinks;
			destination.TafsilGuid = source.TafsilGuid;
			destination.DefaultCusGroup = source.DefaultCusGroup;
			destination.UserAccCompacts = source.UserAccCompacts;
			destination.SyncsKolMoeinTafsil = source.SyncsKolMoeinTafsil;
			destination.BajeSettings = source.BajeSettings;
			destination.HesabyarSettings = source.HesabyarSettings;
			destination.ContextGuid = contextGuid;
			if (source.SyncInfo != null)
			{
				destination.SyncInfo = destination.SyncInfo ?? new Sync();
				await WebContextHolder.GetWebContextHolder(contextGuid).Sync.MapAsync(contextGuid, source.SyncInfo, destination.SyncInfo, tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\UserInformationMapper.cs", 157);
			throw;
		}
	}

	public Task<List<UserInformationBusiness>> MapListAsync(string contextGuid, IEnumerable<UserInformationBusiness> source, List<UserInformationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<UserInformationBusiness> MapAsync(string contextGuid, WebUserInformation source, UserInformationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\UserInformationMapper.cs"))
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
					destination = new UserInformationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.CacheApiURL = source.CacheApiURL;
				destination.DepId = source.DepId;
				destination.PaymentURL = source.PaymentURL;
				destination.UserName = source.UserName;
				destination.UserEmail = source.UserEmail;
				destination.UserMobile = source.UserMobile;
				destination.UserId = source.UserId;
				destination.UserAccGuid = source.UserAccGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.UserDepGuid = source.UserDepGuid;
				destination.UserRegisterDateM = source.UserRegisterDateM;
				destination.UserExpireDateM = source.UserExpireDateM;
				destination.RelativePath = source.RelativePath;
				destination.UserDepGuid = source.UserDepGuid;
				destination.GpsEnable = source.GpsEnable;
				destination.SiteUrl = source.SiteUrl;
				destination.SiteType = source.SiteType;
				destination.AndroidPrint = source.AndroidPrint;
				destination.SyncInfo = source.SyncInfo;
				destination.LastSaved = source.LastSaved;
				destination.CustomerInfo = source.CustomerInfo;
				destination.Order = source.Order;
				destination.AndroidPrintStyles = source.AndroidPrintStyles;
				destination.IsHaveTicketting = source.IsHaveTicketting;
				destination.HesabInfo = source.HesabInfo;
				destination.DepartmentDescription = source.DepartmentDescription;
				destination.MetaDescription = source.MetaDescription;
				destination.MetaKeyword = source.MetaKeyword;
				destination.Script = source.Script;
				destination.ScriptCounter = source.ScriptCounter;
				destination.ScriptEnamad = source.ScriptEnamad;
				destination.ScriptFooter = source.ScriptFooter;
				destination.ScriptHeader = source.ScriptHeader;
				destination.LstSocialNetwork = source.LstSocialNetwork;
				destination.PaymentGatewayRelation = source.PaymentGatewayRelation;
				destination.CurrentServerTime = source.CurrentServerTime;
				destination.HashedPassword = source.HashedPassword;
				destination.LstMediaLinks = source.LstMediaLinks;
				destination.TafsilGuid = source.TafsilGuid;
				destination.DefaultCusGroup = source.DefaultCusGroup;
				destination.UserAccCompacts = source.UserAccCompacts;
				destination.Source = source.Source;
				destination.SyncsKolMoeinTafsil = source.SyncsKolMoeinTafsil;
				destination.BajeSettings = source.BajeSettings;
				destination.HesabyarSettings = source.HesabyarSettings;
				if (source.SyncInfo != null)
				{
					UserInformationBusiness userInformationBusiness = destination;
					if (userInformationBusiness.SyncInfo == null)
					{
						userInformationBusiness.SyncInfo = new Sync();
					}
					await WebContextHolder.GetWebContextHolder(contextGuid).Sync.MapAsync(contextGuid, source.SyncInfo, destination.SyncInfo, tranName);
				}
				return destination;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\UserInformationMapper.cs", 232);
				throw;
			}
		}
	}

	public Task<List<UserInformationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebUserInformation> source, List<UserInformationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
