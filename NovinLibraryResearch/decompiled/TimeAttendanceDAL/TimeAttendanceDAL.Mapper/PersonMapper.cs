using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDAL.Mapper;

public class PersonMapper : IItemAsyncMapper<Person, WebAttPerson>
{
	public static PersonMapper Instance { get; } = new PersonMapper();


	public async Task<WebAttPerson> MapAsync(string contextGuid, Person source, WebAttPerson destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return await Task.FromResult<WebAttPerson>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttPerson();
			}
			destination.Id = ((PersonBase)source).Id;
			((WebAttPersonCompact)destination).Name = ((PersonBase)source).Name;
			((WebAttPersonCompact)destination).Family = ((PersonBase)source).Family;
			((WebAttPersonCompact)destination).DateMBirthday = ((PersonBase)source).DateMBirthday;
			destination.MTavalod = ((PersonBase)source).MTavalod;
			((WebAttPersonCompact)destination).FatherName = ((PersonBase)source).FatherName;
			((WebAttPersonCompact)destination).Mob1 = ((PersonBase)source).Mob1;
			destination.Mob2 = ((PersonBase)source).Mob2;
			((WebAttPersonCompact)destination).Address = ((PersonBase)source).Address;
			((WebAttPersonCompact)destination).CodeMeli = ((PersonBase)source).CodeMeli;
			((WebAttPersonCompact)destination).FullName = ((PersonBase)source).FullName;
			((WebAttPersonCompact)destination).Guid = ((PersonBase)source).Guid;
			((WebAttPersonCompact)destination).UserName = source.UserName;
			destination.HashPassword = source.HashPassword;
			destination.CodeMoein = ((PersonBase)source).CodeMoein;
			destination.CodeTafsil = ((PersonBase)source).CodeTafsil;
			((WebAttPersonCompact)destination).Address = ((PersonBase)source).Address;
			((WebAttPersonCompact)destination).DateMStartWork = ((PersonBase)source).DateMStartWork;
			destination.Jensiat = ((PersonBase)source).Jensiat;
			destination.Hid = ((PersonBase)source).Hid;
			destination.TitleId = ((PersonBase)source).TitleId;
			destination.GroupId = ((PersonBase)source).GroupId;
			destination.FullCode = ((PersonBase)source).FullCode;
			destination.Type = ((PersonBase)source).Type;
			((WebAttPersonCompact)destination).ZipCode = ((PersonBase)source).ZipCode;
			((WebAttPersonCompact)destination).Tel1 = ((PersonBase)source).Tel1;
			destination.Tel2 = ((PersonBase)source).Tel2;
			destination.St = ((PersonBase)source).St;
			destination.HType = ((PersonBase)source).HType;
			destination.PicPath = ((PersonBase)source).PicPath;
			((WebAttPersonCompact)destination).Description = ((PersonBase)source).Description;
			((WebAttPersonCompact)destination).SodorSh = ((PersonBase)source).SodorSh;
			((WebAttPersonCompact)destination).NoBimeh = ((PersonBase)source).NoBimeh;
			destination.DateMFinishW = ((PersonBase)source).DateMFinishW;
			destination.EisarId = ((PersonBase)source).EisarId;
			((WebAttPersonCompact)destination).Shsh = ((PersonBase)source).Shsh;
			((WebAttPersonCompact)destination).SerialSh = ((PersonBase)source).SerialSh;
			((WebAttPersonCompact)destination).MadrakTahsili = ((PersonBase)source).MadrakTahsili;
			WebAttPerson val = destination;
			val.SetGroupName(await source.GetGroupNameAsync(token));
			val = destination;
			val.HashedQueryString = await source.GenerateUrlQueryStringAsync(contextGuid, token);
			destination.DepartmentGuid = ((PersonBase)source).GuidDepartment;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return await Task.FromResult<WebAttPerson>(destination);
	}

	public Task<List<WebAttPerson>> MapListAsync(string contextGuid, IEnumerable<Person> source, List<WebAttPerson> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<Person, WebAttPerson>(source, contextGuid, (IItemAsyncMapper<Person, WebAttPerson>)this, destination, tranName, (short)20);
	}
}
