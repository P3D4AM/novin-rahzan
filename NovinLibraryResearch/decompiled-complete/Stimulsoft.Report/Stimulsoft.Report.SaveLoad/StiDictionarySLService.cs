using System.IO;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

[StiServiceBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SLDictionary.bmp")]
public abstract class StiDictionarySLService : StiSLService
{
	public abstract void Save(StiDictionary dictionary, Stream stream);

	public abstract void Load(StiDictionary dictionary, Stream stream);

	public abstract void Merge(StiDictionary dictionary, Stream stream);
}
