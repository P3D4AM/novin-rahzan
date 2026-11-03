using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class TafsilFilter
{
	public HesabType? HesabType { get; set; }

	public Guid? RelatedMoeinGuid { get; set; }

	public bool? Status { get; set; } = true;


	public string NameSearchString { get; set; } = "";


	public SearchLocation? NameSearchLocation { get; set; }

	public string CodeSearchString { get; set; } = "";


	public SearchLocation? CodeSearchLocation { get; set; }

	public IEnumerable<Guid> TafsilGroupGuids { get; set; }

	public Guid? TafsilGroupGuid { get; set; }

	public long? StartCode { get; set; }

	public long? EndCode { get; set; }

	public List<Guid> MoeinGuidList { get; set; }

	public EnTafsilLevel TafsilLevel { get; set; }
}
