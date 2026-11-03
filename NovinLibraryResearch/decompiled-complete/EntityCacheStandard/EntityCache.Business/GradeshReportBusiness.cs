using System;
using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class GradeshReportBusiness : IHasIsChecked, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Date { get; set; }

	public string DateSh { get; set; }

	public Guid? AsGuid { get; set; }

	public short AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNO { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal Sood { get; set; }

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public string SanadDesc { get; set; }

	public string DescForDisplay { get; set; }

	public DateTime DateMoaser { get; set; }

	public string DateMoaserSh => DateMoaser.MiladiToCalture();

	public string RemForDisplay { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid UGuid { get; set; }

	public string Tashkhis { get; set; }

	public string TafsilItemName { get; set; }

	public long TafsilItemCode { get; set; }

	public string vahedForDisplay { get; set; }

	public bool IsChecked { get; set; }
}
