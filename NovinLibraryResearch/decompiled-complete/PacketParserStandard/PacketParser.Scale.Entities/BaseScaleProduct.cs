using System;
using PacketParser.Scale.Entities.Interfaces;

namespace PacketParser.Scale.Entities;

public class BaseScaleProduct : IScaleProduct
{
	private string barcode;

	public string Barcode
	{
		get
		{
			return barcode;
		}
		set
		{
			value = value?.FixString()?.RemoveNoNumbers() ?? "";
			if (value.Length > 13)
			{
				throw new ArgumentException("value:" + value + " for barcode is not valid");
			}
			barcode = value;
		}
	}

	public string CodeOnScale => Id.ToString();

	public string Description { get; set; }

	public short GroupId { get; set; }

	public Guid Guid { get; set; }

	public int Id { get; set; }

	public bool IsValidEAN13Barcode
	{
		get
		{
			if (!string.IsNullOrEmpty(Barcode))
			{
				return Barcode.Length == 13;
			}
			return false;
		}
	}

	public bool IsWeight { get; set; }

	public string Name { get; set; }

	public decimal PriceOriginal { get; set; }

	public bool St { get; set; }

	public short VahedId { get; set; }

	public string Code { get; set; }

	public int RowVersion { get; set; }
}
