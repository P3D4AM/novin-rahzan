namespace Stimulsoft.Editor;

public interface IFmtExportEx : IFmtExport, IExport
{
	string LineTerminator { get; set; }
}
