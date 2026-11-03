namespace PacketParser.Entities;

public enum DownLoadStatus : short
{
	Complate,
	NotFound,
	Damaged,
	CopyError,
	CancelDownLoad,
	ErrorOnOpration,
	FileExsistOnLocal
}
