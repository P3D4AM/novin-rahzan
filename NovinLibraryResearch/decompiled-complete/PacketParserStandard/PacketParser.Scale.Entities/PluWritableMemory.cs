namespace PacketParser.Scale.Entities;

public class PluWritableMemory
{
	public enum EntityType
	{
		Vazni,
		Tedadi,
		Shomareshi
	}

	public string Name { get; set; }

	public int Price { get; set; }

	public string ProductCode { get; set; }

	public string Barcode { get; set; }

	public EntityType Type { get; set; }
}
