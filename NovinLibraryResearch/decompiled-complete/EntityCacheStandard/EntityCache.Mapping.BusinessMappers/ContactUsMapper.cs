using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ContactUsMapper : IItemAsyncMapper<ContactUsBusiness, WebContactUs>, IItemAsyncMapper<ContactUsBusiness, ContactUsBusiness>, IItemAsyncMapper<WebContactUs, ContactUsBusiness>
{
	public static ContactUsMapper Instance { get; private set; } = new ContactUsMapper();


	public Task<WebContactUs> MapAsync(string contextGuid, ContactUsBusiness source, WebContactUs destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebContactUs();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Address = source.Address;
			destination.CodeMeli = source.CodeMeli;
			destination.DateBirth = source.DateBirth;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Fax = source.Fax;
			destination.Jensiat = source.Jensiat;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Mob2 = source.Mob2;
			destination.NahveAshnaee = source.NahveAshnaee;
			destination.Name = source.Name;
			destination.Ostan = source.Ostan;
			destination.SabegheKari = source.SabegheKari;
			destination.Senf = source.Senf;
			destination.Shahrestan = source.Shahrestan;
			destination.Shoghl = source.Shoghl;
			destination.Subject = source.Subject;
			destination.Tahsilat = source.Tahsilat;
			destination.Tel = source.Tel;
			destination.Tel2 = source.Tel2;
			destination.Type = source.Type;
			destination.WebSite = source.WebSite;
			destination.Zip = source.Zip;
			destination.Ip = source.Ip;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ContactUsMapper.cs", 65);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebContactUs>> MapListAsync(string contextGuid, IEnumerable<ContactUsBusiness> source, List<WebContactUs> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ContactUsBusiness> MapAsync(string contextGuid, ContactUsBusiness source, ContactUsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ContactUsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Address = source.Address;
			destination.CodeMeli = source.CodeMeli;
			destination.DateBirth = source.DateBirth;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Fax = source.Fax;
			destination.Jensiat = source.Jensiat;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Mob2 = source.Mob2;
			destination.NahveAshnaee = source.NahveAshnaee;
			destination.Name = source.Name;
			destination.Ostan = source.Ostan;
			destination.SabegheKari = source.SabegheKari;
			destination.Senf = source.Senf;
			destination.Shahrestan = source.Shahrestan;
			destination.Shoghl = source.Shoghl;
			destination.Subject = source.Subject;
			destination.Tahsilat = source.Tahsilat;
			destination.Tel = source.Tel;
			destination.Tel2 = source.Tel2;
			destination.Type = source.Type;
			destination.WebSite = source.WebSite;
			destination.Zip = source.Zip;
			destination.Ip = source.Ip;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ContactUsMapper.cs", 123);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ContactUsBusiness>> MapListAsync(string contextGuid, IEnumerable<ContactUsBusiness> source, List<ContactUsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ContactUsBusiness> MapAsync(string contextGuid, WebContactUs source, ContactUsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ContactUsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Address = source.Address;
			destination.CodeMeli = source.CodeMeli;
			destination.DateBirth = source.DateBirth;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Fax = source.Fax;
			destination.Jensiat = source.Jensiat;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Mob2 = source.Mob2;
			destination.NahveAshnaee = source.NahveAshnaee;
			destination.Name = source.Name;
			destination.Ostan = source.Ostan;
			destination.SabegheKari = source.SabegheKari;
			destination.Senf = source.Senf;
			destination.Shahrestan = source.Shahrestan;
			destination.Shoghl = source.Shoghl;
			destination.Subject = source.Subject;
			destination.Tahsilat = source.Tahsilat;
			destination.Tel = source.Tel;
			destination.Tel2 = source.Tel2;
			destination.Type = source.Type;
			destination.WebSite = source.WebSite;
			destination.Zip = source.Zip;
			destination.Ip = source.Ip;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ContactUsMapper.cs", 181);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ContactUsBusiness>> MapListAsync(string contextGuid, IEnumerable<WebContactUs> source, List<ContactUsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
