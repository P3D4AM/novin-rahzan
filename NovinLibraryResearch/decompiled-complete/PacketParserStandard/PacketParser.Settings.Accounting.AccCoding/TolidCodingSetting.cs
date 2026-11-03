using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class TolidCodingSetting
{
	private Guid tolidExportMoeinGuid = Guid.Empty;

	private Guid tolidExportTafsilGuid = Guid.Empty;

	private Guid tolidMavadMoeinGuid = Guid.Empty;

	private Guid tolidMavadTafsilGuid = Guid.Empty;

	public Guid TolidExportMoeinGuid
	{
		get
		{
			if (tolidExportMoeinGuid == Guid.Empty)
			{
				tolidExportMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein42;
			}
			return tolidExportMoeinGuid;
		}
		set
		{
			tolidExportMoeinGuid = value;
		}
	}

	public Guid TolidExportTafsilGuid
	{
		get
		{
			if (tolidExportTafsilGuid == Guid.Empty)
			{
				tolidExportTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsilTolidMahsolGuid;
			}
			return tolidExportTafsilGuid;
		}
		set
		{
			tolidExportTafsilGuid = value;
		}
	}

	public Guid TolidMavadMoeinGuid
	{
		get
		{
			if (tolidMavadMoeinGuid == Guid.Empty)
			{
				tolidMavadMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein40;
			}
			return tolidMavadMoeinGuid;
		}
		set
		{
			tolidMavadMoeinGuid = value;
		}
	}

	public Guid TolidMavadTafsilGuid
	{
		get
		{
			if (tolidMavadTafsilGuid == Guid.Empty)
			{
				tolidMavadTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsilTolidMavadGuid;
			}
			return tolidMavadTafsilGuid;
		}
		set
		{
			tolidMavadTafsilGuid = value;
		}
	}
}
