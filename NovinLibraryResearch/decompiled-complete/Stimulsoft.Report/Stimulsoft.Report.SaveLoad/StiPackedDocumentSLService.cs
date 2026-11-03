using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.SaveLoad;

public class StiPackedDocumentSLService : StiDocumentSLService
{
	public override Type ServiceType => typeof(StiDocumentSLService);

	public override bool MultiplePages => false;

	public override void Save(StiReport report, Stream stream)
	{
		byte[] array = StiGZipHelper.Pack(report.SaveDocumentToByteArray());
		stream.Write(array, 0, array.Length);
	}

	public override void Load(StiReport report, Stream stream)
	{
		if (report.ReportCacheMode != 0)
		{
			string text = null;
			try
			{
				text = StiReportCache.CreateNewCache();
				string path = Path.Combine(text, $"{StiGuidUtils.NewGuid()}.tmp");
				FileStream fileStream = new FileStream(path, FileMode.Create);
				StiGZipHelper.Unpack(stream, (Stream)fileStream);
				report.LoadDocument(fileStream);
				fileStream.Close();
				return;
			}
			finally
			{
				if (!string.IsNullOrEmpty(text))
				{
					StiReportCache.DeleteCache(text);
				}
			}
		}
		byte[] array = new byte[stream.Length];
		stream.Seek(0L, SeekOrigin.Begin);
		stream.Read(array, 0, array.Length);
		array = StiGZipHelper.Unpack(array);
		report.LoadDocument(array);
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PackedDocumentFiles");
	}
}
