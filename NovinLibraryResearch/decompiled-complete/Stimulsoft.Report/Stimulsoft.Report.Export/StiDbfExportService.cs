using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Dbf.png")]
public class StiDbfExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private Stream writer2;

	private string separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

	public static string[] copePageNames = new string[20]
	{
		"Default", "437  U.S. MS-DOS", "620  Mazovia (Polish) MS-DOS", "737  Greek MS-DOS (437G)", "850  International MS-DOS", "852  Eastern European MS-DOS", "861  Icelandic MS-DOS", "865  Nordic MS-DOS", "866  Russian MS-DOS", "895  Kamenicky (Czech) MS-DOS",
		"857  Turkish MS-DOS", "1250  EasternEuropean MS-DOS", "1251  Russian Windows", "1252  Windows ANSI", "1253  Greek Windows", "1254  Turkish Windows", "10000  Standard Macintosh", "10006  Greek Macintosh", "10007  Russian Macintosh", "10029  Eastern European Macintosh"
	};

	internal static int[,] codePageCodes = new int[20, 2]
	{
		{ 0, 0 },
		{ 1, 437 },
		{ 105, 620 },
		{ 106, 737 },
		{ 2, 850 },
		{ 100, 852 },
		{ 103, 861 },
		{ 102, 865 },
		{ 101, 866 },
		{ 104, 895 },
		{ 107, 857 },
		{ 200, 1250 },
		{ 201, 1251 },
		{ 3, 1252 },
		{ 203, 1253 },
		{ 202, 1254 },
		{ 4, 10000 },
		{ 152, 10006 },
		{ 150, 10007 },
		{ 151, 10029 }
	};

	public override string DefaultExtension => "dbf";

	public override StiExportFormat ExportFormat => StiExportFormat.Dbf;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeDbfFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportDbf(report, stream, settings as StiDataExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiDbfExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(Form_Complete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "DbfFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = GetFileName(report, sendEMail);
			}
			if (fileName == null)
			{
				return;
			}
			StiDbfCodePages codePage = (StiDbfCodePages)Enum.Parse(typeof(StiDbfCodePages), codePageCodes[(int)form["EncodingSelectedIndex"], 1].ToString());
			StiFileUtils.ProcessReadOnly(fileName);
			try
			{
				using FileStream stream = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiDataExportSettings stiDataExportSettings = new StiDataExportSettings();
				stiDataExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiDataExportSettings.CodePage = codePage;
				StiDataExportSettings settings = stiDataExportSettings;
				StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	private string PrepareData(string input)
	{
		StringBuilder stringBuilder = new StringBuilder(input);
		for (int i = 0; i <= stringBuilder.Length - 2 && stringBuilder[i] == '0' && stringBuilder[i + 1] != '.'; i++)
		{
			stringBuilder[i] = ' ';
		}
		return stringBuilder.ToString();
	}

	public void ExportDbf(StiReport report, string fileName, StiDbfCodePages codePage)
	{
		ExportDbf(report, fileName, StiPagesRange.All, codePage);
	}

	public void ExportDbf(StiReport report, string fileName, StiPagesRange pageRange, StiDbfCodePages codePage)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportDbf(report, fileStream, pageRange, codePage);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportDbf(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportDbf(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportDbf(StiReport report, Stream stream)
	{
		ExportDbf(report, stream, new StiDbfExportSettings());
	}

	public void ExportDbf(StiReport report, Stream stream, StiPagesRange pageRange, StiDbfCodePages codePage)
	{
		StiDbfExportSettings settings = new StiDbfExportSettings
		{
			PageRange = pageRange,
			CodePage = codePage
		};
		ExportDbf(report, stream, settings);
	}

	public void ExportDbf(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Dbf format");
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		StiDbfCodePages codePage = settings.CodePage;
		StiDataExportMode dataExportMode = settings.DataExportMode;
		int num = 0;
		for (int i = 0; i < codePageCodes.Length; i++)
		{
			if (codePageCodes[i, 1] == (int)codePage)
			{
				num = i;
				break;
			}
		}
		Encoder encoder = ((num == 0) ? Encoding.Default.GetEncoder() : Encoding.GetEncoding(codePageCodes[num, 1]).GetEncoder());
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: false, this);
		stiMatrix.ScanComponentsPlacement(optimize: false);
		if (base.IsStopped)
		{
			return;
		}
		stiMatrix.PrepareDocument(this, dataExportMode);
		int num2 = 0;
		for (int j = 0; j < stiMatrix.Fields.Length; j++)
		{
			int num3 = 0;
			for (int k = 0; k < stiMatrix.DataArrayLength; k++)
			{
				string text = stiMatrix.Fields[j].DataArray[k];
				if (text == null)
				{
					text = string.Empty;
				}
				int length = text.Length;
				if (length > num3)
				{
					num3 = length;
				}
			}
			if (num3 == 0)
			{
				num3 = 1;
			}
			if (stiMatrix.Fields[j].Info[0] == 0 && stiMatrix.Fields[j].Info[1] == 0)
			{
				stiMatrix.Fields[j].Info[1] = num3;
			}
			num2 += stiMatrix.Fields[j].Info[1];
			StringBuilder stringBuilder = new StringBuilder();
			if (stiMatrix.Fields[j].Info[0] == 3 || stiMatrix.Fields[j].Info[0] == 4)
			{
				int num4 = stiMatrix.Fields[j].Info[1] - stiMatrix.Fields[j].Info[2] - 1;
				if (num4 > 0)
				{
					stringBuilder.Append('0', num4);
				}
				stringBuilder.Append(".");
				stringBuilder.Append('0', stiMatrix.Fields[j].Info[2]);
			}
			if (stiMatrix.Fields[j].Info[0] == 1 || stiMatrix.Fields[j].Info[0] == 2)
			{
				stringBuilder.Append('0', stiMatrix.Fields[j].Info[1]);
			}
			if (stringBuilder.Length > 1)
			{
				stringBuilder.Append(";-" + stringBuilder.ToString().Substring(1, stringBuilder.Length - 1));
			}
			stiMatrix.Fields[j].FormatString = stringBuilder.ToString();
		}
		num2++;
		Hashtable hashtable = new Hashtable();
		for (int l = 0; l < stiMatrix.Fields.Length; l++)
		{
			string text2 = stiMatrix.Fields[l].Name.ToUpper(CultureInfo.InvariantCulture);
			string text3 = string.Empty;
			int num5 = 0;
			while (hashtable.Contains(text2 + text3))
			{
				num5++;
				text3 = num5.ToString();
				if (text2.Length + text3.Length > 10)
				{
					text2 = text2.Substring(0, 10 - text3.Length);
				}
			}
			text2 += text3;
			stiMatrix.Fields[l].Name = text2;
			hashtable.Add(text2, text2);
		}
		int num6 = 32 + stiMatrix.Fields.Length * 32 + 1;
		byte[] array = new byte[num6];
		array[0] = 3;
		DateTime now = DateTime.Now;
		array[1] = (byte)(now.Year - now.Year / 100 * 100);
		array[2] = (byte)now.Month;
		array[3] = (byte)now.Day;
		BitConverter.GetBytes(stiMatrix.DataArrayLength).CopyTo(array, 4);
		BitConverter.GetBytes((short)num6).CopyTo(array, 8);
		BitConverter.GetBytes((short)num2).CopyTo(array, 10);
		for (int m = 0; m < stiMatrix.Fields.Length; m++)
		{
			int num7 = 32 + m * 32;
			string text4 = stiMatrix.Fields[m].Name.ToUpper(CultureInfo.InvariantCulture);
			byte[] array2 = new byte[text4.Length];
			encoder.GetBytes(text4.ToCharArray(), 0, text4.Length, array2, 0, flush: true);
			array2.CopyTo(array, num7);
			array[num7 + 11] = 67;
			if (stiMatrix.Fields[m].Info[0] == 1 || stiMatrix.Fields[m].Info[0] == 2 || stiMatrix.Fields[m].Info[0] == 3 || stiMatrix.Fields[m].Info[0] == 4)
			{
				array[num7 + 11] = 78;
			}
			if (stiMatrix.Fields[m].Info[0] == 5)
			{
				array[num7 + 11] = 68;
			}
			if (stiMatrix.Fields[m].Info[0] == 6)
			{
				array[num7 + 11] = 76;
			}
			array[num7 + 16] = (byte)stiMatrix.Fields[m].Info[1];
			array[num7 + 17] = (byte)stiMatrix.Fields[m].Info[2];
		}
		array[29] = (byte)codePageCodes[num, 0];
		array[num6 - 1] = 13;
		writer2 = stream;
		writer2.Write(array, 0, num6);
		array = new byte[num2];
		base.CurrentPassNumber = 2;
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		for (int n = 0; n < stiMatrix.DataArrayLength; n++)
		{
			InvokeExporting(n, stiMatrix.DataArrayLength, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			int num8 = 1;
			for (int num9 = 0; num9 < stiMatrix.Fields.Length; num9++)
			{
				string text5 = stiMatrix.Fields[num9].DataArray[n];
				if (text5 == null)
				{
					text5 = string.Empty;
				}
				int num10 = stiMatrix.Fields[num9].Info[1];
				int num11 = stiMatrix.Fields[num9].Info[2];
				switch (stiMatrix.Fields[num9].Info[0])
				{
				case 0:
					if (text5.Length > num10)
					{
						text5 = text5.Remove(num10, text5.Length - num10);
					}
					if (text5.Length < num10)
					{
						text5 += new string(' ', num10 - text5.Length);
					}
					break;
				case 1:
				{
					int result6 = 0;
					if (int.TryParse(text5, out result6))
					{
						text5 = PrepareData(result6.ToString(stiMatrix.Fields[num9].FormatString));
						if (text5.Length > num10)
						{
							text5 = text5.Remove(0, text5.Length - num10);
						}
					}
					else
					{
						result6 = 0;
					}
					break;
				}
				case 2:
				{
					long result5 = 0L;
					if (long.TryParse(text5, out result5))
					{
						text5 = PrepareData(result5.ToString(stiMatrix.Fields[num9].FormatString));
						if (text5.Length > num10)
						{
							text5 = text5.Remove(0, text5.Length - num10);
						}
					}
					else
					{
						result5 = 0L;
					}
					break;
				}
				case 3:
				{
					float result2 = 0f;
					if (float.TryParse(text5.Replace(".", ",").Replace(",", separator), out result2))
					{
						text5 = PrepareData(result2.ToString(stiMatrix.Fields[num9].FormatString).Replace(separator, "."));
						if (text5.Length > num10)
						{
							text5 = text5.Remove(0, text5.Length - num10);
						}
					}
					else
					{
						result2 = 0f;
					}
					break;
				}
				case 4:
				{
					double result3 = 0.0;
					if (double.TryParse(text5.Replace(".", ",").Replace(",", separator), out result3))
					{
						text5 = PrepareData(result3.ToString(stiMatrix.Fields[num9].FormatString).Replace(separator, "."));
						if (text5.Length > num10)
						{
							text5 = text5.Remove(0, text5.Length - num10);
						}
					}
					else
					{
						result3 = 0.0;
					}
					break;
				}
				case 5:
				{
					DateTime result4;
					if (string.IsNullOrEmpty(text5) || string.IsNullOrEmpty(text5.TrimEnd(new char[0])))
					{
						text5 = "        ";
					}
					else if (DateTime.TryParse(text5, out result4))
					{
						text5 = PrepareData(result4.ToString("yyyyMMdd"));
					}
					else
					{
						result4 = DateTime.MinValue;
					}
					break;
				}
				case 6:
				{
					bool result = false;
					bool.TryParse(text5, out result);
					text5 = (result ? "T" : "F");
					break;
				}
				}
				byte[] array3 = new byte[text5.Length];
				encoder.GetBytes(text5.ToCharArray(), 0, text5.Length, array3, 0, flush: true);
				array3.CopyTo(array, num8);
				for (int num12 = text5.Length; num12 < stiMatrix.Fields[num9].Info[1]; num12++)
				{
					array[num8 + num12] = 32;
				}
				num8 += stiMatrix.Fields[num9].Info[1];
			}
			array[0] = 32;
			writer2.Write(array, 0, array.Length);
		}
		array[0] = 26;
		writer2.Write(array, 0, 1);
		writer2.Flush();
		encoder = null;
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
	}
}
