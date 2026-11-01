using PacketParser.BusinessLayer;

namespace TimeAttendanceDAL.Dal.Taradod;

public class ImportTaradodResult
{
	public int CountKol { get; set; }

	public int CountCorrect { get; set; }

	public int CountInCorrect { get; set; }

	public string FileContent { get; set; } = "";


	public ReturnedSaveFuncInfo Result { get; set; } = new ReturnedSaveFuncInfo();

}
