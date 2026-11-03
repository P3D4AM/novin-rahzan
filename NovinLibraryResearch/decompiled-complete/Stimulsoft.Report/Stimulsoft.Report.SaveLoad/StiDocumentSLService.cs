using System.IO;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.SaveLoad;

[StiServiceBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SLDocument.bmp")]
public abstract class StiDocumentSLService : StiSLService
{
	public abstract bool MultiplePages { get; }

	public abstract void Save(StiReport report, Stream stream);

	public abstract void Load(StiReport report, Stream stream);
}
