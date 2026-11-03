using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class SanadStatusReportErrorTimeModel
{
	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public long Id2 { get; set; }

	public int StatusId { get; set; }

	public string AsTypeForDisplay => AsType.GetDisplay().GetTranslate();

	public DateTime SanadDateM { get; set; }

	public string SanadDateSh => SanadDateM.MiladiToCalture();

	public string SanadTime => $"{SanadDateM.Hour}:{SanadDateM.Minute}";

	public DateTime ErrorDateM { get; set; }

	public string ErrorDateSh => ErrorDateM.MiladiToCalture();

	public string ErrorTime => $"{ErrorDateM.Hour}:{ErrorDateM.Minute}";

	public string DescriptionForDisplay => $"{AsTypeForDisplay} ({Id2})";
}
