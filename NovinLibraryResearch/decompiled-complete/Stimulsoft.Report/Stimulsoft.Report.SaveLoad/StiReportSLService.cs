using System.IO;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.SaveLoad;

[StiServiceBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SLReport.bmp")]
public abstract class StiReportSLService : StiSLService
{
	public abstract void Save(StiReport report, Stream stream);

	public abstract void Load(StiReport report, Stream stream);
}
