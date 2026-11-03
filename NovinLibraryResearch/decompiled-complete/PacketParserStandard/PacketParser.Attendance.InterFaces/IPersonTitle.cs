namespace PacketParser.Attendance.InterFaces;

public interface IPersonTitle
{
	short Id { get; }

	string Name { get; set; }

	bool St { get; set; }

	string Guid { get; set; }
}
