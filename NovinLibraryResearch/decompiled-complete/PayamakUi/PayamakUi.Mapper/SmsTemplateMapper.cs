using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Mapper;

public class SmsTemplateMapper : IItemAsyncMapper<BaseSmsTemplate, SMSTemplate>
{
	public static SmsTemplateMapper Instance { get; private set; } = new SmsTemplateMapper();


	public Task<SMSTemplate> MapAsync(string contextGuid, BaseSmsTemplate source, SMSTemplate destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SMSTemplate();
			}
			destination.Id = source.Id;
			destination.Title = source.Title;
			destination.Content = source.Content;
			destination.DateRegister = source.DateRegister;
			destination.GroupName = source.GroupName;
			destination.Count = source.Count;
			destination.TopSt = source.TopSt;
			destination.EndSt = source.EndSt;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Mapper\\SmsTemplateMapper.cs", 39);
			throw;
		}
	}

	public Task<List<SMSTemplate>> MapListAsync(string contextGuid, IEnumerable<BaseSmsTemplate> source, List<SMSTemplate> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
