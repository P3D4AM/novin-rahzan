using System;
using PacketParser.Entities;

namespace EntityCache.EventArguments;

public class BedehiTalabEventArgs
{
	public int SanadNo { get; private set; }

	public DateTime DateM { get; private set; }

	public Guid? TafsilGuid { get; private set; }

	public HesabType? HesabType { get; private set; }

	public BedehiTalabEventArgs(int sanadNo, DateTime date, Guid? tafsilGuid = null, HesabType? htype = null)
	{
		SanadNo = sanadNo;
		DateM = date;
		TafsilGuid = tafsilGuid;
		HesabType = htype;
	}
}
