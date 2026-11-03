using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class OrderTtmsReport
{
	public Guid Guid { get; set; }

	public Document FactorType { get; set; }

	public decimal ProductTax { get; set; }

	public decimal ProductSumWithTax { get; set; }

	public decimal ProductSumWithoutTax { get; set; }

	public decimal ProductTotalSumWithTax => ProductSumWithTax + ProductTax;

	public decimal ServiceTax { get; set; }

	public decimal ServiceSumWithTax { get; set; }

	public decimal ServiceSumWithoutTax { get; set; }

	public decimal ServiceTotalSumWithTax => ServiceSumWithTax + ServiceTax;
}
