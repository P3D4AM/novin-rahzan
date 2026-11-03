using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiXmlDictionarySLService : StiDictionarySLService
{
	public override Type ServiceType => typeof(StiDictionarySLService);

	public override void Save(StiDictionary dictionary, Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		StiReport report = dictionary.Report;
		dictionary.Report = null;
		val.Serialize((object)dictionary, stream, "StiDictionary");
		dictionary.Report = report;
	}

	public override void Load(StiDictionary dictionary, Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)dictionary, stream, "StiDictionary");
	}

	public override void Merge(StiDictionary dictionary, Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		StiDictionary stiDictionary = new StiDictionary(dictionary.Report);
		val.Deserialize((object)stiDictionary, stream, "StiDictionary");
		dictionary.Merge(stiDictionary);
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load | StiSLActions.Merge;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "DictionaryFiles");
	}
}
