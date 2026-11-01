using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Device;

namespace TimeAttendanceDAL.Mapper;

public class HardwareCodeCusViewMapper : IItemAsyncMapper<HardwareCodeCUSView, Person_Hardwarecodes>
{
	public static HardwareCodeCusViewMapper Instance { get; } = new HardwareCodeCusViewMapper();


	public async Task<Person_Hardwarecodes> MapAsync(string contextGuid, HardwareCodeCUSView source, Person_Hardwarecodes destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return await Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new Person_Hardwarecodes();
			}
			destination.HardwareCodeCusView = source;
			destination.NameInDevice = source.FullName ?? ("user" + source.HardwareCode);
			destination.HardWareCode = source.HardwareCode;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return await Task.FromResult(destination);
	}

	public Task<List<Person_Hardwarecodes>> MapListAsync(string contextGuid, IEnumerable<HardwareCodeCUSView> source, List<Person_Hardwarecodes> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<HardwareCodeCUSView, Person_Hardwarecodes>(source, contextGuid, (IItemAsyncMapper<HardwareCodeCUSView, Person_Hardwarecodes>)this, destination, tranName, (short)20);
	}
}
