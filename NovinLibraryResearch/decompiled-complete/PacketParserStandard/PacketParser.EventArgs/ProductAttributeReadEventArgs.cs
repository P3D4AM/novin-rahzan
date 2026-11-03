using System;

namespace PacketParser.EventArgs;

public class ProductAttributeReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static ProductAttributeReadEventArgs FromString(Packet pack, string data)
	{
		ProductAttributeReadEventArgs productAttributeReadEventArgs = data.FromJson<ProductAttributeReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Product\\ProductAttributeReadEventArgs.cs", 15);
		productAttributeReadEventArgs.Part = pack.Part;
		productAttributeReadEventArgs.TotalParts = pack.TotalParts;
		productAttributeReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return productAttributeReadEventArgs;
	}
}
