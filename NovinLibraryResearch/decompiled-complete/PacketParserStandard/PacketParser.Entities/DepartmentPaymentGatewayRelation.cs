using System;

namespace PacketParser.Entities;

public class DepartmentPaymentGatewayRelation
{
	public Guid GuidBank { get; set; } = Guid.Empty;


	public Guid Guid { get; set; }

	public WebPaymentGatewayValueMetadata PaymentGatewayValueMetadata { get; set; }
}
