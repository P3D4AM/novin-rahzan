using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiEncryptedReportSLService : StiReportSLService
{
	public override Type ServiceType => typeof(StiReportSLService);

	public string Key { get; set; } = string.Empty;


	public override void Save(StiReport report, Stream stream)
	{
		try
		{
			report.IsSerializing = true;
			byte[] array = StiEncryption.Encrypt(StiGZipHelper.Pack(report.SaveToByteArray()), Key);
			byte[] array2 = new byte[array.Length + 3];
			array2[0] = 109;
			array2[1] = 114;
			array2[2] = 120;
			Array.Copy(array, 0, array2, 3, array.Length);
			stream.Write(array2, 0, array2.Length);
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
			if (array[0] != 109 || array[1] != 114 || array[2] != 120)
			{
				throw new Exception("This file is a not '.mrx' format.");
			}
			byte[] array2 = new byte[array.Length - 3];
			Array.Copy(array, 3, array2, 0, array.Length - 3);
			array2 = StiEncryption.Decrypt(array2, Key);
			try
			{
				array2 = StiGZipHelper.Unpack(array2);
			}
			catch
			{
				throw new Exception("File decryption error: wrong key.");
			}
			report.Load(array2);
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
		return StiLocalization.Get("FileFilters", "EncryptedReportFiles");
	}
}
