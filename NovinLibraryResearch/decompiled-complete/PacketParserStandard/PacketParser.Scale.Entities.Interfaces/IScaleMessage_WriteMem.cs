using System;

namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleMessage_WriteMem
{
	string Barcode { get; set; }

	string Code { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; }

	DateTime ExpireDate { get; set; }

	int Id { get; set; }

	short MaxTryCount { get; set; }

	int MemNo { get; set; }

	string Name { get; set; }

	DateTime NextTry { get; set; }

	string Parameter1 { get; set; }

	string Parameter2 { get; set; }

	string Parameter3 { get; set; }

	string Parameter4 { get; set; }

	double Parsang { get; set; }

	int ScaleId { get; set; }

	short St { get; set; }

	short TryCount { get; set; }

	decimal UnitPrice { get; set; }

	bool Vazni { get; set; }
}
