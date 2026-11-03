namespace PacketParser.Entities;

public interface ITelegramConfig
{
	string ApiKey { get; set; }

	string ContactInfo { get; set; }

	EnTelegramType contactType { get; set; }
}
