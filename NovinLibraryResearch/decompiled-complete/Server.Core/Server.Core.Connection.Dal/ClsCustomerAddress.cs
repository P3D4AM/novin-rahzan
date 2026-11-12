using System;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs.Customer;
using PacketParser.LogPacket;

namespace Server.Core.Connection.Dal;

public class ClsCustomerAddress
{
	public static Task CustomerAddressInstance_OnSaveRequest(object arg1, CustomerAddressSaveRequestEventArgs arg2)
	{
		try
		{
			ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CustomerAddress, 0, isReceivedPacket: true);
			return new Task(delegate
			{
			});
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}
}
