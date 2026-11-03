using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiXmlDocumentSLService : StiDocumentSLService
{
	private static object lockObj = new object();

	private const int sizeBuffer = 32768;

	private byte[] buffer;

	private long startBuffer;

	private int lengthData;

	public override Type ServiceType => typeof(StiDocumentSLService);

	public override bool MultiplePages => false;

	public override void Save(StiReport report, Stream stream)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		StiReport report2 = report;
		if (report.RenderedPages.Count > 0)
		{
			report2 = report.RenderedPages[0].Report;
		}
		StiDocument stiDocument = new StiDocument(report);
		try
		{
			foreach (StiPage page in stiDocument.Pages)
			{
				page.Report = null;
				page.Document = stiDocument;
			}
			stiDocument.Resources = new StiResourcesCollection();
			foreach (StiResource resource in report.Dictionary.Resources)
			{
				if (resource.AvailableInTheViewer || resource.Type == StiResourceType.FontEot || resource.Type == StiResourceType.FontOtf || resource.Type == StiResourceType.FontTtc || resource.Type == StiResourceType.FontTtf || resource.Type == StiResourceType.FontWoff || resource.Type == StiResourceType.Map)
				{
					stiDocument.Resources.Add(resource);
				}
			}
			StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
			RegPropertyNames(val);
			try
			{
				val.Serializing += base.OnSaving;
				val.SortProperties = false;
				val.CheckSerializable = true;
				if ((!StiOptions.Engine.DocumentSavingOptimization && !report.RenderedPages.CacheMode) || stiDocument.Pages.Count < 1)
				{
					val.Serialize((object)stiDocument, stream, "StiDocument", (StiSerializeTypes)8);
					return;
				}
				long position = stream.Position;
				if (!SaveOptimized(stream, stiDocument, val))
				{
					stream.Seek(position, SeekOrigin.Begin);
					val.Serialize((object)stiDocument, stream, "StiDocument", (StiSerializeTypes)8);
				}
				buffer = null;
			}
			finally
			{
				val.Serializing -= base.OnSaving;
			}
		}
		finally
		{
			foreach (StiPage page2 in stiDocument.Pages)
			{
				page2.Report = report2;
				page2.Document = null;
			}
		}
	}

	public static void RegPropertyNames(StiSerializing sr)
	{
		sr.IsDocument = true;
		if (sr.StringToProperty.Count == 0)
		{
			lock (lockObj)
			{
				sr.AddStringProperty("Brush", "bh");
				sr.AddStringProperty("TextBrush", "tb");
				sr.AddStringProperty("Border", "br");
				sr.AddStringProperty("Dock", "d");
				sr.AddStringProperty("Font", "fn");
				sr.AddStringProperty("Text", "text");
				sr.AddStringProperty("Rectangle", "rc");
				sr.AddStringProperty("TextOptions", "to");
				sr.AddStringProperty("VertAlignment", "va");
				sr.AddStringProperty("HorAlignment", "ha");
				sr.AddStringProperty("TextFormat", "tf");
			}
		}
	}

	public override void Load(StiReport report, Stream stream)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter(allowLoadOptimization: true));
			RegPropertyNames(val);
			StiDocument stiDocument = new StiDocument(report);
			try
			{
				val.Deserializing += base.OnLoading;
				if (!StiOptions.Engine.DocumentLoadingOptimization && report.ReportCacheMode != StiReportCacheMode.On)
				{
					val.Deserialize((object)stiDocument, stream, "StiDocument");
				}
				else
				{
					long position = stream.Position;
					if (!LoadOptimized(stream, stiDocument, val))
					{
						stream.Seek(position, SeekOrigin.Begin);
						val.Deserialize((object)stiDocument, stream, "StiDocument");
					}
				}
				report.IsRendered = true;
				report.NeedsCompiling = false;
			}
			finally
			{
				val.Deserializing -= base.OnLoading;
			}
		}
		finally
		{
			foreach (StiPage renderedPage in report.RenderedPages)
			{
				renderedPage.Report = report;
				StiComponentsCollection components = renderedPage.GetComponents();
				foreach (StiComponent item in components)
				{
					item.Page = renderedPage;
				}
			}
		}
	}

	private bool LoadOptimized(Stream stream, StiDocument document, StiSerializing sr)
	{
		long num = FindIndexOfForward(stream, "<Pages isList=", 0L);
		if (num == -1)
		{
			return false;
		}
		List<long> list = new List<long>();
		long num2 = num;
		while (true)
		{
			long num3 = FindIndexOfForward(stream, "type=\"Page\" isKey=\"true\"", num2);
			if (num3 == -1)
			{
				break;
			}
			long num4 = FindIndexOfBackward("<", num3);
			if (num4 == -1)
			{
				return false;
			}
			string pageName = GetPageName(num4 + 1);
			if (pageName == null)
			{
				return false;
			}
			num4 = FindIndexOfForward(stream, ">", num4);
			if (num4 == -1)
			{
				return false;
			}
			num4++;
			long num5 = FindIndexOfForward(stream, pageName, num4);
			if (num5 == -1)
			{
				return false;
			}
			list.Add(num4);
			list.Add(num5);
			num2 = num5 + pageName.Length;
		}
		stream.Seek(num2, SeekOrigin.Begin);
		lengthData = stream.Read(buffer, 32768, 65536);
		startBuffer = num2;
		long num6 = FindIndexOfForward(stream, "</Pages>", num2 + 1);
		if (num6 == -1)
		{
			return false;
		}
		num6 += "</Pages>".Length;
		MemoryStream memoryStream = new MemoryStream();
		CopyStreamBlock(stream, 0L, memoryStream, -1L, (int)num);
		MemoryStream memoryStream2 = new MemoryStream();
		CopyStreamBlock(stream, num6, memoryStream2, -1L, (int)(stream.Length - num6));
		byte[] bytes = Encoding.UTF8.GetBytes("<Pages isList=\"true\" count=\"1\">\r\n<Page1 Ref=\"2\" type=\"Page\" isKey=\"true\">");
		byte[] bytes2 = Encoding.UTF8.GetBytes("</Page1>\r\n</Pages>");
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		StiPagesCollection stiPagesCollection = new StiPagesCollection(stiReport);
		if (document.Report != null && document.Report.ReportCacheMode != 0 && (document.Report.ReportCacheMode == StiReportCacheMode.On || list.Count > StiOptions.Engine.ReportCache.LimitForStartUsingCache * 2))
		{
			document.Report.ReportCachePath = StiReportCache.CreateNewCache();
			stiReport.ReportCachePath = document.Report.ReportCachePath;
			document.Pages.CacheMode = true;
			document.Pages.CanUseCacheMode = false;
			stiPagesCollection.CacheMode = true;
			stiPagesCollection.CanUseCacheMode = true;
			stiPagesCollection.CheckCacheL2();
		}
		List<StiRepositoryItems> list2 = new List<StiRepositoryItems>();
		List<Font> list3 = new List<Font>();
		List<string> list4 = new List<string>();
		List<string> list5 = new List<string>();
		List<StiBrush> list6 = new List<StiBrush>();
		List<StiBorder> list7 = new List<StiBorder>();
		for (int i = 0; i < list.Count; i += 2)
		{
			long num7 = list[i];
			long num8 = list[i + 1];
			MemoryStream memoryStream3 = new MemoryStream();
			memoryStream.WriteTo(memoryStream3);
			memoryStream3.Write(bytes, 0, bytes.Length);
			CopyStreamBlock(stream, num7, memoryStream3, -1L, (int)(num8 - num7));
			memoryStream3.Write(bytes2, 0, bytes2.Length);
			memoryStream2.WriteTo(memoryStream3);
			memoryStream3.Seek(0L, SeekOrigin.Begin);
			sr.Deserialize((object)document, (Stream)memoryStream3, "StiDocument");
			if (document.Pages.Count == 0)
			{
				return false;
			}
			stiPagesCollection.Add(document.Pages[0]);
			List<StiComponent> componentsList = document.Pages[0].GetComponentsList();
			for (int j = 0; j < componentsList.Count; j++)
			{
				StiComponent stiComponent = componentsList[j];
				if (stiComponent.IsPropertiesInitialized())
				{
					bool flag = true;
					foreach (StiRepositoryItems item in list2)
					{
						if (((StiService)stiComponent).Properties.Equals(item))
						{
							((StiService)stiComponent).Properties = item;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						list2.Add(((StiService)stiComponent).Properties);
					}
				}
				if (stiComponent.ComponentPlacement != null)
				{
					int num9 = list4.IndexOf(stiComponent.ComponentPlacement);
					if (num9 == -1)
					{
						list4.Add(stiComponent.ComponentPlacement);
					}
					else
					{
						stiComponent.ComponentPlacement = list4[num9];
					}
				}
				if (stiComponent.Guid != null)
				{
					int num10 = list5.IndexOf(stiComponent.Guid);
					if (num10 == -1)
					{
						list5.Add(stiComponent.Guid);
					}
					else
					{
						stiComponent.Guid = null;
						stiComponent.Guid = list5[num10];
					}
				}
				if (stiComponent is IStiBrush { Brush: not null } stiBrush)
				{
					int num11 = list6.IndexOf(stiBrush.Brush);
					if (num11 == -1)
					{
						list6.Add(stiBrush.Brush);
					}
					else
					{
						stiBrush.Brush = list6[num11];
					}
				}
				if (stiComponent is IStiTextBrush { TextBrush: not null } stiTextBrush)
				{
					int num12 = list6.IndexOf(stiTextBrush.TextBrush);
					if (num12 == -1)
					{
						list6.Add(stiTextBrush.TextBrush);
					}
					else
					{
						stiTextBrush.TextBrush = list6[num12];
					}
				}
				if (stiComponent is IStiFont { Font: not null } stiFont)
				{
					int num13 = list3.IndexOf(stiFont.Font);
					if (num13 == -1)
					{
						list3.Add(stiFont.Font);
					}
					else
					{
						stiFont.Font = list3[num13];
					}
				}
				if (stiComponent is IStiBorder stiBorder)
				{
					int num14 = list7.IndexOf(stiBorder.Border);
					if (num14 == -1)
					{
						list7.Add(stiBorder.Border);
					}
					else
					{
						stiBorder.Border = list7[num14];
					}
				}
				if (stiComponent.Interaction != null && stiComponent.Interaction.IsDefault)
				{
					stiComponent.Interaction = null;
				}
			}
		}
		MemoryStream memoryStream4 = new MemoryStream();
		memoryStream.WriteTo(memoryStream4);
		memoryStream2.WriteTo(memoryStream4);
		memoryStream4.Seek(0L, SeekOrigin.Begin);
		sr.Deserialize((object)document, (Stream)memoryStream4, "StiDocument");
		document.Pages.Clear();
		document.Pages.AddRange(stiPagesCollection);
		stiReport.ReportCachePath = string.Empty;
		stiReport.Dispose();
		return true;
	}

	private bool SaveOptimized(Stream stream, StiDocument document, StiSerializing sr)
	{
		StiPagesCollection renderedPages = document.Report.RenderedPages;
		try
		{
			StiPagesCollection stiPagesCollection = new StiPagesCollection(document.Report);
			document.Report.RenderedPages = stiPagesCollection;
			MemoryStream memoryStream = new MemoryStream();
			sr.Serialize((object)document, (Stream)memoryStream, "StiDocument", (StiSerializeTypes)8);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			long num = FindIndexOfForward(memoryStream, "<Pages isList=", 0L);
			if (num == -1)
			{
				return false;
			}
			MemoryStream memoryStream2 = new MemoryStream();
			CopyStreamBlock(memoryStream, 0L, memoryStream2, -1L, (int)num);
			long num2 = FindIndexOfForward(stream, "<", num + 1);
			if (num2 == -1)
			{
				return false;
			}
			MemoryStream memoryStream3 = new MemoryStream();
			CopyStreamBlock(memoryStream, num2, memoryStream3, -1L, (int)(memoryStream.Length - num2));
			memoryStream.Close();
			byte[] bytes = Encoding.UTF8.GetBytes($"<Pages isList=\"true\" count=\"{renderedPages.Count}\">\r\n  ");
			byte[] bytes2 = Encoding.UTF8.GetBytes("</Pages>\r\n  ");
			stiPagesCollection.Add(StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage);
			memoryStream2.WriteTo(stream);
			stream.Write(bytes, 0, bytes.Length);
			MemoryStream memoryStream4 = new MemoryStream();
			int num3 = CorrectRefs(memoryStream2, memoryStream4, 0L, (int)memoryStream2.Length, 0, 0);
			memoryStream4.Close();
			int baseNewRef = num3;
			for (int i = 0; i < renderedPages.Count; i++)
			{
				renderedPages.GetPage(renderedPages[i]);
				stiPagesCollection[0] = renderedPages[i];
				buffer = null;
				memoryStream = new MemoryStream();
				sr.Serialize((object)document, (Stream)memoryStream, "StiDocument", (StiSerializeTypes)8);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				num = FindIndexOfForward(memoryStream, "<Pages isList=", 0L);
				if (num == -1)
				{
					return false;
				}
				num = FindIndexOfForward(memoryStream, "<", num + 1);
				if (num == -1)
				{
					return false;
				}
				num2 = FindIndexOfForward(memoryStream, "</Pages>", num);
				if (num2 == -1)
				{
					return false;
				}
				stream.WriteByte(32);
				stream.WriteByte(32);
				baseNewRef = CorrectRefs(memoryStream, stream, num, num2, baseNewRef, num3);
				memoryStream.Close();
			}
			stream.Write(bytes2, 0, bytes2.Length);
			CorrectRefs(memoryStream3, stream, 0L, (int)memoryStream3.Length, baseNewRef, num3);
		}
		finally
		{
			document.Report.RenderedPages = renderedPages;
		}
		return true;
	}

	private string GetPageName(long position)
	{
		position += 32768 - startBuffer;
		StringBuilder stringBuilder = new StringBuilder();
		while (!char.IsWhiteSpace((char)buffer[position]))
		{
			stringBuilder.Append((char)buffer[position]);
			position++;
		}
		if (stringBuilder.Length == 0)
		{
			return null;
		}
		return $"</{stringBuilder}>";
	}

	private void CopyStreamBlock(Stream inputStream, long inputPosition, Stream outputStream, long outputPosition, int length)
	{
		inputStream.Seek(inputPosition, SeekOrigin.Begin);
		if (outputPosition != -1)
		{
			outputStream.Seek(outputPosition, SeekOrigin.Begin);
		}
		byte[] array = new byte[65536];
		while (length > 0)
		{
			int num = length;
			if (num > 65536)
			{
				num = 65536;
			}
			length -= num;
			num = inputStream.Read(array, 0, num);
			if (num > 0)
			{
				outputStream.Write(array, 0, num);
				continue;
			}
			break;
		}
	}

	private long FindIndexOfForward(Stream stream, string expr, long offset)
	{
		if (buffer == null)
		{
			buffer = new byte[98304];
			stream.Seek(0L, SeekOrigin.Begin);
			lengthData = stream.Read(buffer, 32768, 65536);
			startBuffer = 0L;
		}
		offset -= startBuffer;
		while (true)
		{
			int num = lengthData;
			if (num > 32768)
			{
				num = 32768;
			}
			byte b = (byte)expr[0];
			for (long num2 = offset; num2 < num; num2++)
			{
				if (buffer[32768 + num2] != b)
				{
					continue;
				}
				bool flag = true;
				for (int i = 1; i < expr.Length; i++)
				{
					if (buffer[32768 + num2 + i] != (byte)expr[i])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return startBuffer + num2;
				}
			}
			if (lengthData < 32768)
			{
				break;
			}
			Array.Copy(buffer, 32768, buffer, 0, 65536);
			lengthData -= 32768;
			startBuffer += 32768L;
			offset = 0L;
			if (lengthData == 32768)
			{
				lengthData = stream.Read(buffer, 65536, 32768);
				lengthData += 32768;
			}
		}
		return -1L;
	}

	private long FindIndexOfBackward(string expr, long offset)
	{
		if (buffer == null)
		{
			return -1L;
		}
		offset -= startBuffer;
		byte b = (byte)expr[0];
		long num = offset;
		if (startBuffer > 0)
		{
			num += 32768;
		}
		while (num >= 0)
		{
			if (buffer[32768 + offset] == b)
			{
				bool flag = true;
				for (int i = 1; i < expr.Length; i++)
				{
					if (buffer[32768 + offset + i] != (byte)expr[i])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return startBuffer + offset;
				}
			}
			num--;
			offset--;
		}
		return -1L;
	}

	private int CorrectRefs(MemoryStream input, Stream output, long startPosition, long endPosition, int baseNewRef, int baseOldRef)
	{
		input.Seek(startPosition, SeekOrigin.Begin);
		int num = 0;
		int num2 = 0;
		while (input.Position < endPosition)
		{
			int num3 = input.ReadByte();
			long position = input.Position;
			if (num3 == 82 && num2 == 32)
			{
				if (input.ReadByte() == 101 && input.ReadByte() == 102 && input.ReadByte() == 61 && input.ReadByte() == 34)
				{
					int refNum = GetRefNum(input);
					if (refNum != -1)
					{
						int num4 = refNum - baseOldRef + baseNewRef;
						if (num4 > num)
						{
							num = num4;
						}
						WriteStringToStream(output, $"Ref=\"{num4}\"");
						num2 = 0;
						continue;
					}
				}
				input.Seek(position, SeekOrigin.Begin);
			}
			else if (num3 == 105 && num2 == 32)
			{
				if (input.ReadByte() == 115 && input.ReadByte() == 82 && input.ReadByte() == 101 && input.ReadByte() == 102 && input.ReadByte() == 61 && input.ReadByte() == 34)
				{
					int refNum2 = GetRefNum(input);
					if (refNum2 != -1)
					{
						int num4 = refNum2 - baseOldRef + baseNewRef;
						if (num4 > num)
						{
							num = num4;
						}
						WriteStringToStream(output, $"isRef=\"{num4}\"");
						num2 = 0;
						continue;
					}
				}
				input.Seek(position, SeekOrigin.Begin);
			}
			output.WriteByte((byte)num3);
			num2 = num3;
		}
		return num;
	}

	private static void WriteStringToStream(Stream ms, string st)
	{
		foreach (char c in st)
		{
			ms.WriteByte((byte)c);
		}
	}

	private static int GetRefNum(MemoryStream ms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		while (ms.Position < ms.Length && char.IsDigit((char)(num = ms.ReadByte())))
		{
			stringBuilder.Append((char)num);
		}
		if (num == 34 && stringBuilder.Length > 0)
		{
			return int.Parse(stringBuilder.ToString());
		}
		return -1;
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "DocumentFiles");
	}
}
