using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Mapper;

internal class SmsNetAccountMapper : IItemAsyncMapper<BaseSmsNetAccount, SMSNet_Account>
{
	public static SmsNetAccountMapper Instance { get; private set; } = new SmsNetAccountMapper();


	public Task<SMSNet_Account> MapAsync(string contextGuid, BaseSmsNetAccount source, SMSNet_Account destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SMSNet_Account();
			}
			destination.Type = source.Type;
			destination.GsmCrecit = source.GsmCrecit;
			destination.GsmUpdate = source.GsmUpdate;
			destination.GsmMaxSent = source.GsmMaxSent;
			destination.IsChecked = source.IsChecked;
			destination.St = source.St;
			destination.Description = source.Description;
			destination.Username = source.Username;
			destination.Id = source.Id;
			destination.Title = source.Title;
			destination.Password = source.Password;
			destination.MetaData = source.MetaData;
			destination.GSM_Connection = source.GSM_Connection;
			destination.GSM_On_Sendeng_Message = source.GSM_Connection;
			destination.GSMPDUMode = source.GSMPDUMode;
			destination.GSMNO = source.GSMNO;
			destination.GSMSignal = source.GSMSignal;
			destination.GSMSendedSmsCount = source.GSMSendedSmsCount;
			destination.Domain = source.Domain;
			destination.FullDomain = source.FullDomain;
			destination.SenderNumber = source.SenderNumber;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Mapper\\SmsNetAccountMapper.cs", 51);
			throw;
		}
	}

	public Task<List<SMSNet_Account>> MapListAsync(string contextGuid, IEnumerable<BaseSmsNetAccount> source, List<SMSNet_Account> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
