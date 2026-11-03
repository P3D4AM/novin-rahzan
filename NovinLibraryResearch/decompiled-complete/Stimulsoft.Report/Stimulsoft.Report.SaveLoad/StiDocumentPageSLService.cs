using System.IO;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.SaveLoad;

[StiServiceBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SLPage.bmp")]
public abstract class StiDocumentPageSLService : StiSLService
{
	public abstract void Save(StiPage page, Stream stream);

	public abstract void Load(StiPage page, Stream stream);
}
