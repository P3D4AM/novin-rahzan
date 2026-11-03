namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvTokens : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long Number { get; set; }

	AdvertiseType Type { get; set; }

	string Token { get; set; }
}
