using System;
using System.Drawing;

namespace PacketParser.Attendance.InterFaces;

public interface ITaradodFullDet
{
	bool ReloadNextRow { get; set; }

	DateTime DateM { get; }

	Color ColorEk { get; set; }

	int DetailStatusTypeTaradod { get; set; }

	int KasreKarBeginShift { get; set; }

	int JarimeRoz { get; set; }

	int KasreKarEndShift { get; set; }

	int KasreKarMidShift { get; set; }

	int EzafeKarBeginShift { get; set; }

	int EzafeKarEndShift { get; set; }

	int EzafeKarMidShift { get; set; }
}
