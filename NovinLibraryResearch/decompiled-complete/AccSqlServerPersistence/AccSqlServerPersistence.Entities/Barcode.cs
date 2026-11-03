using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Barcode")]
public class Barcode : IBarcode, IHasMetaData
{
	[Key]
	[Required]
	public string? BarCode { get; set; }

	public Guid PrdGuid { get; set; }

	public DateTime CreateDate { get; set; }

	public short Ordinal { get; set; }

	public Guid? DepotGuid { get; set; }

	[Required]
	public string? Description { get; set; }

	[Required]
	public string? Name { get; set; }

	public decimal Price { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public decimal Quantity { get; set; }

	public BarcodePriceType Type { get; set; }

	public string? MetaData { get; set; }

	public Product Products { get; set; }

	public Depot Depots { get; set; }
}
