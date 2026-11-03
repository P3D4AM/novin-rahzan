using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class CustomerTempResponseEventArgs : BaseEventArgs
{
	public CustomerSaveResponse CustomerSaveResponse { get; set; }

	public Guid RequestedCustomerGuid { get; set; }
}
