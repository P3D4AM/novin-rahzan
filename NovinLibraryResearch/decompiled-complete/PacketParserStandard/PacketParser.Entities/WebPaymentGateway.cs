using System;
using System.Text.Json.Serialization;

namespace PacketParser.Entities;

public class WebPaymentGateway
{
	[JsonIgnore]
	public static Guid Mellat = Guid.Parse("f17adfc5-a4c5-4456-97d1-18b2337d2861");

	[JsonIgnore]
	public static Guid Melli = Guid.Parse("bdf5b24e-e8d7-472d-bdd3-32a42e69af77");

	[JsonIgnore]
	public static Guid Zarinpal = Guid.Parse("a09ee8ee-c840-4215-9838-06d2c72e9e7d");

	[JsonIgnore]
	public static Guid SamanKish = Guid.Parse("a45e0670-82f0-4e93-8f55-38e6a3971a45");

	[JsonIgnore]
	public static Guid Sepehr = Guid.Parse("c271d98f-9fa5-4a37-aff5-f31282ab7870");

	[JsonIgnore]
	public static Guid Saman = Guid.Parse("e7dbad13-a782-44c4-8d96-fe1f3aeb772c");

	[JsonIgnore]
	public static Guid AmneZibal = Guid.Parse("e32f5cce-4506-4fbf-9d8e-6d410ed571df");

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public WebPaymentGatewayKeyMetadata webPaymentGatewayMetadata { get; set; }

	public byte[] Image { get; set; }

	public bool Status { get; set; }

	public short Ordinal { get; set; }

	public string WordPressPaymentMethodName { get; set; }
}
