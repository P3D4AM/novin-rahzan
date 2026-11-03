using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvertiseLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	string Content { get; set; }

	long SimCardNumber { get; set; }

	string State { get; set; }

	string City { get; set; }

	string Region { get; set; }

	decimal Price { get; set; }

	string Category { get; set; }

	string SubCategory1 { get; set; }

	string SubCategory2 { get; set; }

	string URL { get; set; }

	string IP { get; set; }

	string UpdateDesc { get; set; }

	int VisitCount { get; set; }

	string DateSh { get; set; }

	DateTime DateM { get; set; }

	string Adv { get; set; }

	Guid MasterVisitorGuid { get; set; }

	Guid SlaveVisitorGuid { get; set; }

	AdvStatusCode StatusCode { get; set; }

	AdvertiseType AdvType { get; set; }

	DateTime LastUpdate { get; set; }
}
