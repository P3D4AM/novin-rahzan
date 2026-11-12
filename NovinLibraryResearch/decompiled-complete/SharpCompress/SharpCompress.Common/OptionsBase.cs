namespace SharpCompress.Common;

public class OptionsBase
{
	public bool LeaveStreamOpen { get; set; } = true;


	public ArchiveEncoding ArchiveEncoding { get; set; } = new ArchiveEncoding();

}
