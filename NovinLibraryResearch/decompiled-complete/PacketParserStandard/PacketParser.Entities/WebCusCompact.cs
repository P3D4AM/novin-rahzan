using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebCusCompact : ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public short IdGroup { get; set; }

	public int VisitorId { get; set; }

	public short Title { get; set; }

	public string Name { get; set; }

	public string Family { get; set; }

	public bool Status { get; set; }

	public long Code { get; set; }

	public long LongCode { get; set; }

	public decimal Account { get; set; }

	public bool Special { get; set; }

	public string PicFace { get; set; }

	public string PicSign { get; set; }

	public int TafsilId { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid VisitorGuid { get; set; }

	public Guid TitleGuid { get; set; }

	public bool IsBazaryab { get; set; }

	public bool IsSupervisor { get; set; }

	public bool IsEtebari { get; set; }

	public string Tell1 { get; set; }

	public string FullName
	{
		get
		{
			return Name + " " + Family;
		}
		set
		{
		}
	}

	public string Address1 { get; set; }

	public string Address2 { get; set; }

	public string Description { get; set; }

	public string Zip { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public Guid TafsilGroupGuid { get; set; }
}
