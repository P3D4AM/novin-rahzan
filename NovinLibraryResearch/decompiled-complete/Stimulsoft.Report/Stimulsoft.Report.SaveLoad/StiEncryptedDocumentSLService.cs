using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.SaveLoad;

public class StiEncryptedDocumentSLService : StiDocumentSLService
{
	public override Type ServiceType => typeof(StiDocumentSLService);

	public string Key { get; set; } = string.Empty;


	public override bool MultiplePages => false;

	public override void Save(StiReport report, Stream stream)
	{
		byte[] array = StiEncryption.Encrypt(StiGZipHelper.Pack(report.SaveDocumentToByteArray()), Key);
		byte[] array2 = new byte[array.Length + 3];
		array2[0] = 109;
		array2[1] = 100;
		array2[2] = 120;
		Array.Copy(array, 0, array2, 3, array.Length);
		stream.Write(array2, 0, array2.Length);
	}

	public override void Load(StiReport report, Stream stream)
	{
		byte[] array = new byte[stream.Length];
		stream.Seek(0L, SeekOrigin.Begin);
		stream.Read(array, 0, array.Length);
		if (array[0] != 109 || array[1] != 100 || array[2] != 120)
		{
			throw new Exception("This file is a not '.mdx' format.");
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
		report.LoadDocument(array2);
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "EncryptedDocumentFiles");
	}
}
