using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class CustomerRegister : IItemAsyncMapper<CustomerRegister, CustomerRegister>
{
	public Guid Guid { get; set; }

	public string UserName { get; set; }

	public string Email { get; set; }

	public long Mobile { get; set; }

	public string SentVerificationCode { get; set; }

	public string ReciveVerificationCode { get; set; }

	public string Ip { get; set; }

	public string ConnectionGuid { get; set; }

	public short DepId { get; set; }

	public ENSource Source { get; set; }

	public Task<CustomerRegister> MapAsync(string contextGuid, CustomerRegister source, CustomerRegister destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CustomerRegister();
			}
			source.Guid = source.Guid;
			source.UserName = source.UserName;
			source.Email = source.Email;
			source.Mobile = source.Mobile;
			source.SentVerificationCode = source.SentVerificationCode;
			source.ReciveVerificationCode = source.ReciveVerificationCode;
			source.ConnectionGuid = source.ConnectionGuid;
			source.DepId = source.DepId;
			source.Source = source.Source;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CustomerRegister.cs", 45);
		}
		return Task.FromResult(destination);
	}

	public Task<List<CustomerRegister>> MapListAsync(string contextGuid, IEnumerable<CustomerRegister> source, List<CustomerRegister> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
