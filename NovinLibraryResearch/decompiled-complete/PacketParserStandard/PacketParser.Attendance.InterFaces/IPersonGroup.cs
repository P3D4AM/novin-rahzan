namespace PacketParser.Attendance.InterFaces;

public interface IPersonGroup
{
	short Id { get; }

	int ParentId { get; set; }

	short SubGroups { get; set; }

	short Level { get; set; }

	string Code { get; set; }

	string Name { get; set; }

	bool St { get; set; }

	bool Check { get; set; }
}
