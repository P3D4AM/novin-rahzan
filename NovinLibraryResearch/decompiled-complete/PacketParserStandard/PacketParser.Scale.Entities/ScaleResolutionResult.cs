using PacketParser.Entities;

namespace PacketParser.Scale.Entities;

public struct ScaleResolutionResult
{
	public decimal WeightZarib;

	public int Id { get; set; }

	public int ScaleNumber { get; set; }

	public EnReadWeightType ReadWeightType { get; set; }

	public string ReadWeightRequest { get; set; }

	public string ReadWeightRegex { get; set; }
}
