using System;

namespace PacketParser.DefaultsCoding;

public class HesabDefault
{
	public Guid KosoratDebitTafsilGuid = Guid.Parse("4ae617b1-7936-4bf4-9738-2e212de2c19e");

	public Guid kosoratCreditTafsilGuid = Guid.Parse("c76103d2-a3cb-4689-807c-2283a1209f03");

	public Guid EzafatDebitTafsilGuid = Guid.Parse("cf2c4b2a-1f09-424e-84d4-163353276ed9");

	public Guid EzafatCreditTafsilGuid = Guid.Parse("5b9b6e5b-f281-4659-aae9-c23b0f91da70");

	public Guid KosoratDebitMoeinGuid = Guid.Parse("06a82074-2c81-495d-addf-12492aae54c3");

	public Guid kosoratCreditMoeinGuid = Guid.Parse("b80ebdc3-295c-43d0-af96-9920efe44b3c");

	public Guid EzafatDebitMoeinGuid = Guid.Parse("b80ebdc3-295c-43d0-af96-9920efe44b3c");

	public Guid EzafatCreditMoeinGuid = Guid.Parse("fe68514f-dbc3-4af4-b509-297f6d6d4625");
}
