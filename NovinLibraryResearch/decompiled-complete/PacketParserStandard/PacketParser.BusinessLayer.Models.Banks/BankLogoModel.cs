using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Banks;

public class BankLogoModel
{
	public byte[] Image { get; set; }

	public EnBank BankType { get; set; }
}
