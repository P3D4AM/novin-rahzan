using System;

namespace PacketParser.EventArgs;

public class ContactUsReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static ContactUsReadEventArgs FromString(Packet pack, string data)
	{
		ContactUsReadEventArgs contactUsReadEventArgs = data.FromJson<ContactUsReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ContactUs\\ContactUsReadEventArgs.cs", 15);
		contactUsReadEventArgs.Part = pack.Part;
		contactUsReadEventArgs.TotalParts = pack.TotalParts;
		contactUsReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return contactUsReadEventArgs;
	}
}
