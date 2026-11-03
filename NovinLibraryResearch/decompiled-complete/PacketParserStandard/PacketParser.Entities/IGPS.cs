using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IGPS : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal Accuracy { get; set; }

	EnGpsActivity Activity { get; set; }

	decimal Altitude { get; set; }

	decimal Bearing { get; set; }

	string ColorHex { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	decimal Distance { get; set; }

	decimal FromLastPointDistance { get; set; }

	decimal FromStartPointDistance { get; set; }

	DateTime GPSTime { get; set; }

	string Icon { get; set; }

	decimal Latitude { get; set; }

	decimal Longitude { get; set; }

	decimal PointDurationSeconds { get; set; }

	string PointDurationTime { get; }

	int PreviewsID { get; set; }

	decimal Speed { get; set; }

	decimal Time { get; set; }

	int UId { get; set; }

	Guid UserAccGuid { get; set; }
}
