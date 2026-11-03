using System;

namespace PacketParser.EventArgs;

public class CustomerCommitRequestEventArgs : BaseEventArgs
{
	public Guid Guid { get; set; }

	public string SmsCode { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static CustomerCommitRequestEventArgs FromString(Packet pack, string data)
	{
		CustomerCommitRequestEventArgs customerCommitRequestEventArgs = data.FromJson<CustomerCommitRequestEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CustomerCommitRequestEventArgs.cs", 16);
		customerCommitRequestEventArgs.Part = pack.Part;
		customerCommitRequestEventArgs.TotalParts = pack.TotalParts;
		customerCommitRequestEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return customerCommitRequestEventArgs;
	}
}
