using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ProductInfoModel
{
	public Guid Guid { get; set; }

	public bool ScaleSt { get; set; }

	public decimal BPercent1 { get; set; }

	public decimal BPercent2 { get; set; }

	public decimal BPercent3 { get; set; }

	public decimal BPercent4 { get; set; }

	public decimal Buy1 { get; set; }

	public decimal Buy2 { get; set; }

	public decimal Buy3 { get; set; }

	public bool IsSerial { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public decimal V3Qt { get; set; }

	public decimal V4Qt { get; set; }

	public Guid? V3Guid { get; set; }

	public Guid? V4Guid { get; set; }

	public ProductMojodiSt Mojodi { get; set; }
}
