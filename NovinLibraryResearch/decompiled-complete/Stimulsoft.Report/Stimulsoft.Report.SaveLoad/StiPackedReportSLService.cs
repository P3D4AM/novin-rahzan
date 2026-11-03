using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiPackedReportSLService : StiReportSLService
{
	public override Type ServiceType => typeof(StiReportSLService);

	public override void Save(StiReport report, Stream stream)
	{
		try
		{
			report.IsSerializing = true;
			byte[] array = StiGZipHelper.Pack(report.SaveToByteArray());
			stream.Write(array, 0, array.Length);
		}
		finally
		{
			report.IsSerializing = false;
		}
	}

	public override void Load(StiReport report, Stream stream)
	{
		try
		{
			report.Clear();
			report.IsSerializing = true;
			StiDataCollection dataStore = report.DataStore;
			byte[] array = new byte[stream.Length];
			stream.Seek(0L, SeekOrigin.Begin);
			stream.Read(array, 0, array.Length);
			array = StiGZipHelper.Unpack(array);
			report.Load(array);
			report.DataStore.Clear();
			report.DataStore.RegData(dataStore);
		}
		finally
		{
			report.IsSerializing = false;
		}
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PackedReportFiles");
	}
}
