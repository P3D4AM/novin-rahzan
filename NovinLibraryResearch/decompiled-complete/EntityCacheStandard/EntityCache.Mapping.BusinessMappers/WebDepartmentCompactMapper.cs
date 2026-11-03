using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class WebDepartmentCompactMapper : IItemAsyncMapper<WebDepartment, WebDepartmentCompact>, IItemAsyncMapper<WebDepartmentCompact, WebDepartment>
{
	public Task<WebDepartmentCompact> MapAsync(string contextGuid, WebDepartment source, WebDepartmentCompact destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDepartmentCompact();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.Id = source.Id;
			destination.Name = source.Name;
			destination.URL = source.URL;
			destination.StrType = source.StrType;
			destination.AndroidCount = source.AndroidCount;
			destination.BajeCount = source.BajeCount;
			destination.PrintCount = source.PrintCount;
			destination.HaveBKWebService = source.HaveBKWebService;
			destination.DateExpire = source.DateExpire;
			destination.SefareshgirLevel = source.SefareshgirLevel;
			destination.User = source.User;
			destination.Description = source.Description;
			destination.GPS = source.GPS;
			destination.Guid = source.Guid;
			destination.Date = source.Date;
			destination.Mail = source.Mail;
			destination.Password = source.Pass;
			destination.RestApi = source.RestApi;
			destination.WebSiteWordPress = source.WebSiteWordPress;
			destination.CloudeAttendance = source.CloudeAttendance;
			destination.SyncsKolMoeinTafsil = source.SyncsKolMoeinTafsil;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\WebDepartmentCompactMapper.cs", 64);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebDepartment> MapAsync(string contextGuid, WebDepartmentCompact source, WebDepartment destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDepartment();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.Id = source.Id;
			destination.Name = source.Name;
			destination.URL = source.URL;
			destination.StrType = source.StrType;
			destination.AndroidCount = source.AndroidCount;
			destination.BajeCount = source.BajeCount;
			destination.PrintCount = source.PrintCount;
			destination.HaveBKWebService = source.HaveBKWebService;
			destination.DateExpire = source.DateExpire;
			destination.SefareshgirLevel = source.SefareshgirLevel;
			destination.User = source.User;
			destination.Description = source.Description;
			destination.GPS = source.GPS;
			destination.Mail = source.Mail;
			destination.Pass = source.Password;
			destination.CloudeAttendance = source.CloudeAttendance;
			destination.WebSiteWordPress = source.WebSiteWordPress;
			destination.RestApi = source.RestApi;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\WebDepartmentCompactMapper.cs", 115);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebDepartmentCompact>> MapListAsync(string contextGuid, IEnumerable<WebDepartment> source, List<WebDepartmentCompact> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDepartment>> MapListAsync(string contextGuid, IEnumerable<WebDepartmentCompact> source, List<WebDepartment> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
