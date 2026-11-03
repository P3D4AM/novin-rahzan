using System;

namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleProduct
{
	string Barcode { get; set; }

	string CodeOnScale { get; }

	string Description { get; set; }

	short GroupId { get; set; }

	Guid Guid { get; set; }

	int Id { get; set; }

	bool IsValidEAN13Barcode { get; }

	bool IsWeight { get; set; }

	string Name { get; set; }

	decimal PriceOriginal { get; set; }

	bool St { get; set; }

	short VahedId { get; set; }

	string Code { get; set; }

	int RowVersion { get; set; }
}
