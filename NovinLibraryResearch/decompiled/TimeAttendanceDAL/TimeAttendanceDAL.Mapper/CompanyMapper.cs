using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceDAL.Mapper;

public class CompanyMapper : IItemAsyncMapper<Company, WebAttCompany>
{
	public static CompanyMapper Instance { get; } = new CompanyMapper();


	public Task<WebAttCompany> MapAsync(string contextGuid, Company source, WebAttCompany destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult<WebAttCompany>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttCompany();
			}
			destination.Id = source.Id;
			destination.Code = source.Code;
			destination.Name = source.Name;
			destination.NameKarfarma = source.NameKarfarma;
			destination.Address = source.Address;
			destination.NerkhBimeh = source.NerkhBimeh;
			destination.NoPeyman = source.NoPeyman;
			destination.InsuranceBranch = source.InsuranceBranch;
			destination.NameWorkShop = source.NameWorkShop;
			destination.AddressWorkShop = source.AddressWorkShop;
			return Task.FromResult<WebAttCompany>(destination);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Task<List<WebAttCompany>> MapListAsync(string contextGuid, IEnumerable<Company> source, List<WebAttCompany> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<Company, WebAttCompany>(source, contextGuid, (IItemAsyncMapper<Company, WebAttCompany>)this, destination, tranName, (short)20);
	}
}
