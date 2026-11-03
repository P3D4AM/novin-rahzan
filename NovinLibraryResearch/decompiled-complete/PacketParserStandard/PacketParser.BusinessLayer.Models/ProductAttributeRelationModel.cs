using System;

namespace PacketParser.BusinessLayer.Models;

public class ProductAttributeRelationModel
{
	public Guid ProductGuid { get; set; }

	public Guid AttributeValueGuid { get; set; }

	public Guid AttributeGuid { get; set; }

	public string AttributeValue { get; set; }

	public byte[] DeleteImage { get; set; }
}
