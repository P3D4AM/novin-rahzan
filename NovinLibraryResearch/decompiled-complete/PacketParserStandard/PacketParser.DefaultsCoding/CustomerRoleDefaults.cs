using System;

namespace PacketParser.DefaultsCoding;

public class CustomerRoleDefaults
{
	public Guid Editor => Guid.Parse("0e35cd81-97a4-473f-b501-56e6654b1c64");

	public Guid Author => Guid.Parse("0a144fce-9d4a-4e9b-9778-d035effc6cbc");

	public Guid Contributor => Guid.Parse("6d482af5-99f4-4858-af7e-f82d8b47c19a");

	public Guid Subscriber => Guid.Parse("033ef192-878b-4af4-96dc-0f838730b34d");

	public Guid Customer => Guid.Parse("9e4891f8-377d-4b9c-a9c8-aabdc8d733f9");

	public Guid Administrator => Guid.Parse("c1fd895b-bcb4-4a66-9459-1b2a0faf9832");
}
