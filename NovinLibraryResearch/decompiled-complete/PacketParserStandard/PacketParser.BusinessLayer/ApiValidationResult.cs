using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class ApiValidationResult
{
	public ReturnSaveFuncInfoType Type { get; set; }

	public Dictionary<string, List<string>> Results { get; set; }
}
