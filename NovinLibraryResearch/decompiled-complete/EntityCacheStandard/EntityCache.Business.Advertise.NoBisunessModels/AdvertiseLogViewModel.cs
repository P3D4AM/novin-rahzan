using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Advertise.NoBisunessModels;

public class AdvertiseLogViewModel : IHasContext
{
	public Guid Guid { get; set; }

	public string Title { get; set; }

	public long SimCardNumber { get; set; }

	public string State { get; set; }

	public string City { get; set; }

	public string Region { get; set; }

	public string Category { get; set; }

	public string SubCategory1 { get; set; }

	public string SubCategory2 { get; set; }

	public string URL { get; set; }

	public string IP { get; set; }

	public int VisitCount { get; set; }

	public string DateSh { get; set; }

	public DateTime DateM { get; set; }

	public AdvStatusCode StatusCode { get; set; }

	public string StatusName => StatusCode.GetDisplay();

	public AdvertiseType AdvType { get; set; }

	public string TypeName => AdvType.GetDisplay();

	public string UpdateDesc { get; set; }

	public string ContextGuid { get; set; }
}
