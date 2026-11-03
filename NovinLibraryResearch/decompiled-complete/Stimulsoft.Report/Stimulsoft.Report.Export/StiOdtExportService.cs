using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export.Tools;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Odt.png")]
public class StiOdtExportService : StiExportService
{
	private class CellStyleData
	{
		public string BorderLeft;

		public string BorderRight;

		public string BorderTop;

		public string BorderBottom;

		public Color BackColor;

		public StiVertAlignment VertAlign;

		public CellStyleData()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			BorderLeft = "none";
			BorderRight = "none";
			BorderTop = "none";
			BorderBottom = "none";
			BackColor = Color.Transparent;
			VertAlign = (StiVertAlignment)2;
		}
	}

	private class ParagraphStyleData
	{
		public string FontName;

		public float FontSize;

		public bool Bold;

		public bool Italic;

		public bool Underline;

		public Color FontColor;

		public StiTextHorAlignment HorAlign;

		public int Angle;

		public ParagraphStyleData()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			FontName = "Arial";
			FontSize = 6f;
			Bold = false;
			Italic = false;
			Underline = false;
			FontColor = Color.Black;
			HorAlign = (StiTextHorAlignment)0;
			Angle = 0;
		}
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiMatrix matrix;

	private bool removeEmptySpaceAtBottom = StiOptions.Export.OpenDocumentWriter.RemoveEmptySpaceAtBottom;

	private StiImageCache imageCache;

	private ArrayList cellStyleList;

	private ArrayList paragraphStyleList;

	private int xmlIndentation = 1;

	private float imageQuality = 0.75f;

	private float imageResolution = 96f;

	private bool usePageHeadersAndFooters;

	public override string DefaultExtension => "odt";

	public override StiExportFormat ExportFormat => StiExportFormat.Odt;

	public override string GroupCategory => "Word";

	public override int Position => 23;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeWriterFile");

	public override bool MultipleFiles => false;

	internal StiMatrix Matrix => matrix;

	internal bool RemoveEmptySpaceAtBottom => removeEmptySpaceAtBottom;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportOdt(report, stream, settings as StiOdtExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiOdtExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(form_Complete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	private void form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
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
			StiFileUtils.ProcessReadOnly(fileName);
			try
			{
				using FileStream stream = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiOdtExportSettings stiOdtExportSettings = new StiOdtExportSettings();
				stiOdtExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiOdtExportSettings.UsePageHeadersAndFooters = (bool)form["UsePageHeadersAndFooters"];
				stiOdtExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiOdtExportSettings.ImageResolution = (float)form["Resolution"];
				stiOdtExportSettings.RemoveEmptySpaceAtBottom = (bool)form["RemoveEmptySpaceAtBottom"];
				StiOdtExportSettings settings = stiOdtExportSettings;
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

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "WriterFiles");
	}

	private static string DoubleToString(double number)
	{
		return Math.Round(number / 100.0, 4).ToString().Replace(",", ".") + "in";
	}

	private static string GetColumnName(int column)
	{
		int num = column / 26;
		int num2 = column % 26;
		StringBuilder stringBuilder = new StringBuilder();
		if (num > 0)
		{
			stringBuilder.Append((char)(65 + num - 1));
		}
		stringBuilder.Append((char)(65 + num2));
		return stringBuilder.ToString();
	}

	private static string GetColorString(Color color)
	{
		return "#" + color.ToArgb().ToString("X8").Substring(2);
	}

	private int GetCellStyleNumber(int indexRow, int indexColumn, int height, int width)
	{
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Invalid comparison between Unknown and I4
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Invalid comparison between Unknown and I4
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		CellStyleData cellStyleData = new CellStyleData();
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < height; i++)
		{
			if (matrix.BordersY[indexRow + i, indexColumn] == (StiBorderSide)null)
			{
				flag = false;
			}
			if (matrix.BordersY[indexRow + i, indexColumn + width] == (StiBorderSide)null)
			{
				flag2 = false;
			}
		}
		bool flag3 = true;
		bool flag4 = true;
		for (int j = 0; j < width; j++)
		{
			if (matrix.BordersX[indexRow, indexColumn + j] == (StiBorderSide)null)
			{
				flag3 = false;
			}
			if (matrix.BordersX[indexRow + height, indexColumn + j] == (StiBorderSide)null)
			{
				flag4 = false;
			}
		}
		if (flag3)
		{
			cellStyleData.BorderTop = GetStringFromBorder(Matrix.BordersX[indexRow, indexColumn]);
		}
		if (flag)
		{
			cellStyleData.BorderLeft = GetStringFromBorder(Matrix.BordersY[indexRow, indexColumn]);
		}
		if (flag4)
		{
			cellStyleData.BorderBottom = GetStringFromBorder(Matrix.BordersX[indexRow + height, indexColumn]);
		}
		if (flag2)
		{
			cellStyleData.BorderRight = GetStringFromBorder(Matrix.BordersY[indexRow, indexColumn + width]);
		}
		if (Matrix.Cells[indexRow, indexColumn] != null)
		{
			StiCell stiCell = Matrix.Cells[indexRow, indexColumn];
			cellStyleData.BackColor = stiCell.CellStyle.Color;
			cellStyleData.VertAlign = stiCell.CellStyle.VertAlignment;
			if (stiCell.CellStyle.TextOptions != null)
			{
				float angle = stiCell.CellStyle.TextOptions.Angle;
				if ((angle > 45f && angle < 135f) || (angle > 225f && angle < 315f))
				{
					if ((int)stiCell.CellStyle.HorAlignment == 0)
					{
						cellStyleData.VertAlign = (StiVertAlignment)2;
					}
					if ((int)stiCell.CellStyle.HorAlignment == 1)
					{
						cellStyleData.VertAlign = (StiVertAlignment)1;
					}
					if ((int)stiCell.CellStyle.HorAlignment == 2)
					{
						cellStyleData.VertAlign = (StiVertAlignment)0;
					}
					if ((int)stiCell.CellStyle.HorAlignment == 3)
					{
						cellStyleData.VertAlign = (StiVertAlignment)2;
					}
				}
			}
		}
		if (cellStyleList.Count > 0)
		{
			for (int k = 0; k < cellStyleList.Count; k++)
			{
				CellStyleData cellStyleData2 = (CellStyleData)cellStyleList[k];
				if (cellStyleData2.BorderLeft == cellStyleData.BorderLeft && cellStyleData2.BorderRight == cellStyleData.BorderRight && cellStyleData2.BorderTop == cellStyleData.BorderTop && cellStyleData2.BorderBottom == cellStyleData.BorderBottom && cellStyleData2.BackColor == cellStyleData.BackColor && cellStyleData2.VertAlign == cellStyleData.VertAlign)
				{
					return k;
				}
			}
		}
		cellStyleList.Add(cellStyleData);
		return cellStyleList.Count - 1;
	}

	private static string GetStringFromBorder(StiBorderSide border)
	{
		return $"{DoubleToString(border.Size)} solid {GetColorString(border.Color)}";
	}

	private int GetParagraphStyleNumber(int indexRow, int indexColumn)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		StiCellStyle cellStyle = Matrix.Cells[indexRow, indexColumn].CellStyle;
		ParagraphStyleData paragraphStyleData = new ParagraphStyleData();
		paragraphStyleData.FontName = cellStyle.Font.Name;
		paragraphStyleData.FontSize = cellStyle.Font.SizeInPoints;
		paragraphStyleData.Bold = cellStyle.Font.Bold;
		paragraphStyleData.Italic = cellStyle.Font.Italic;
		paragraphStyleData.Underline = cellStyle.Font.Underline;
		paragraphStyleData.FontColor = cellStyle.TextColor;
		paragraphStyleData.HorAlign = cellStyle.HorAlignment;
		float num = 0f;
		if (cellStyle.TextOptions != null)
		{
			num = cellStyle.TextOptions.Angle;
		}
		if (num > 45f && num < 135f)
		{
			paragraphStyleData.Angle = 90;
		}
		if (num > 225f && num < 315f)
		{
			paragraphStyleData.Angle = 270;
		}
		if (paragraphStyleData.Angle != 0)
		{
			if ((int)cellStyle.VertAlignment == 0)
			{
				paragraphStyleData.HorAlign = (StiTextHorAlignment)0;
			}
			if ((int)cellStyle.VertAlignment == 1)
			{
				paragraphStyleData.HorAlign = (StiTextHorAlignment)1;
			}
			if ((int)cellStyle.VertAlignment == 2)
			{
				paragraphStyleData.HorAlign = (StiTextHorAlignment)2;
			}
		}
		if (paragraphStyleList.Count > 0)
		{
			for (int i = 0; i < paragraphStyleList.Count; i++)
			{
				ParagraphStyleData paragraphStyleData2 = (ParagraphStyleData)paragraphStyleList[i];
				if (paragraphStyleData2.FontName == paragraphStyleData.FontName && paragraphStyleData2.FontSize == paragraphStyleData.FontSize && paragraphStyleData2.Bold == paragraphStyleData.Bold && paragraphStyleData2.Italic == paragraphStyleData.Italic && paragraphStyleData2.Underline == paragraphStyleData.Underline && paragraphStyleData2.FontColor == paragraphStyleData.FontColor && paragraphStyleData2.HorAlign == paragraphStyleData.HorAlign && paragraphStyleData2.Angle == paragraphStyleData.Angle)
				{
					return i;
				}
			}
		}
		paragraphStyleList.Add(paragraphStyleData);
		return paragraphStyleList.Count - 1;
	}

	private MemoryStream WriteMimetype()
	{
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.ASCII);
		streamWriter.Write("application/vnd.oasis.opendocument.text");
		streamWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteMeta()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("office:document-meta");
		xmlTextWriter.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
		xmlTextWriter.WriteAttributeString("office:version", "1.1");
		xmlTextWriter.WriteStartElement("office:meta");
		string value = string.Format("{0}", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
		xmlTextWriter.WriteElementString("meta:generator", "Stimulsoft Reports");
		xmlTextWriter.WriteElementString("meta:creation-date", value);
		xmlTextWriter.WriteElementString("dc:date", value);
		xmlTextWriter.WriteElementString("meta:editing-cycles", "1");
		xmlTextWriter.WriteElementString("meta:editing-duration", "PT0M01S");
		xmlTextWriter.WriteStartElement("meta:user-defined");
		xmlTextWriter.WriteAttributeString("meta:name", "Info 1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("meta:user-defined");
		xmlTextWriter.WriteAttributeString("meta:name", "Info 2");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("meta:user-defined");
		xmlTextWriter.WriteAttributeString("meta:name", "Info 3");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("meta:user-defined");
		xmlTextWriter.WriteAttributeString("meta:name", "Info 4");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("meta:document-statistic");
		xmlTextWriter.WriteAttributeString("meta:table-count", "1");
		xmlTextWriter.WriteAttributeString("meta:image-count", "0");
		xmlTextWriter.WriteAttributeString("meta:object-count", "0");
		xmlTextWriter.WriteAttributeString("meta:page-count", "1");
		xmlTextWriter.WriteAttributeString("meta:paragraph-count", "1");
		xmlTextWriter.WriteAttributeString("meta:word-count", "1");
		xmlTextWriter.WriteAttributeString("meta:character-count", "1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteManifest()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("manifest:manifest");
		xmlTextWriter.WriteAttributeString("xmlns:manifest", "urn:oasis:names:tc:opendocument:xmlns:manifest:1.0");
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "application/vnd.oasis.opendocument.text");
		xmlTextWriter.WriteAttributeString("manifest:full-path", "/");
		xmlTextWriter.WriteEndElement();
		for (int i = 0; i < imageCache.ImagePackedStore.Count; i++)
		{
			xmlTextWriter.WriteStartElement("manifest:file-entry");
			xmlTextWriter.WriteAttributeString("manifest:media-type", "image/jpeg");
			xmlTextWriter.WriteAttributeString("manifest:full-path", $"Pictures/{i + 1:D5}.jpg");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "text/xml");
		xmlTextWriter.WriteAttributeString("manifest:full-path", "content.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "text/xml");
		xmlTextWriter.WriteAttributeString("manifest:full-path", "styles.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "text/xml");
		xmlTextWriter.WriteAttributeString("manifest:full-path", "meta.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "text/xml");
		xmlTextWriter.WriteAttributeString("manifest:full-path", "settings.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteImage(int number)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = (byte[])imageCache.ImagePackedStore[number];
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	private MemoryStream WriteSettings()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("office:document-settings");
		xmlTextWriter.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:config", "urn:oasis:names:tc:opendocument:xmlns:config:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
		xmlTextWriter.WriteAttributeString("office:version", "1.1");
		xmlTextWriter.WriteStartElement("office:settings");
		xmlTextWriter.WriteStartElement("config:config-item-set");
		xmlTextWriter.WriteAttributeString("config:name", "ooo:view-settings");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewAreaTop");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewAreaLeft");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("-10107");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewAreaWidth");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("43208");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewAreaHeight");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("22174");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowRedlineChanges");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "InBrowseMode");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item-map-indexed");
		xmlTextWriter.WriteAttributeString("config:name", "Views");
		xmlTextWriter.WriteStartElement("config:config-item-map-entry");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewId");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteString("view2");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewLeft");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("3002");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewTop");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("10435");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleLeft");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("-10107");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleTop");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleRight");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("33099");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleBottom");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("22172");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ZoomType");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ZoomFactor");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("100");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsSelectedFrame");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item-set");
		xmlTextWriter.WriteAttributeString("config:name", "ooo:configuration-settings");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AddParaTableSpacing");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintReversed");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "OutlineLevelYieldsNumbering");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "LinkUpdateMode");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintEmptyPages");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IgnoreFirstLineIndentInNumbering");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CharacterCompressionType");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintSingleJobs");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UpdateFromTemplate");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintPaperFromSetup");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AddFrameOffsets");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintLeftPages");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RedlineProtectionKey");
		xmlTextWriter.WriteAttributeString("config:type", "base64Binary");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintTables");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ChartAutoUpdate");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintControls");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrinterSetup");
		xmlTextWriter.WriteAttributeString("config:type", "base64Binary");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IgnoreTabsAndBlanksForLineCalculation");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintAnnotationMode");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "LoadReadonly");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AddParaSpacingToTableCells");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AddExternalLeading");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ApplyUserData");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "FieldAutoUpdate");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "SaveVersionOnClose");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "SaveGlobalDocumentLinks");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsKernAsianPunctuation");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AlignTabStopPosition");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ClipAsCharacterAnchoredWriterFlyFrames");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CurrentDatabaseDataSource");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "DoNotCaptureDrawObjsOnPage");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "TableRowKeep");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrinterName");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintFaxName");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ConsiderTextWrapOnObjPos");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UseOldPrinterMetrics");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintRightPages");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsLabelDocument");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UseFormerLineSpacing");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AddParaTableSpacingAtStart");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UseFormerTextWrapping");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "DoNotResetParaAttrsForNumFont");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintProspect");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintGraphics");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AllowPrintJobCancel");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CurrentDatabaseCommandType");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "DoNotJustifyLinesWithManualBreak");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UseFormerObjectPositioning");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrinterIndependentLayout");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteString("high-resolution");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UseOldNumbering");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintPageBackground");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CurrentDatabaseCommand");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintDrawings");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrintBlackFonts");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UnxForceZeroExtLeading");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteStyles(StiPagesCollection pages)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("office:document-styles");
		xmlTextWriter.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:math", "http://www.w3.org/1998/Math/MathML");
		xmlTextWriter.WriteAttributeString("xmlns:form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
		xmlTextWriter.WriteAttributeString("xmlns:ooow", "http://openoffice.org/2004/writer");
		xmlTextWriter.WriteAttributeString("xmlns:oooc", "http://openoffice.org/2004/calc");
		xmlTextWriter.WriteAttributeString("xmlns:dom", "http://www.w3.org/2001/xml-events");
		xmlTextWriter.WriteAttributeString("office:version", "1.1");
		xmlTextWriter.WriteStartElement("office:font-face-decls");
		xmlTextWriter.WriteStartElement("style:font-face");
		xmlTextWriter.WriteAttributeString("style:name", "Arial");
		xmlTextWriter.WriteAttributeString("svg:font-family", "Arial");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:font-face");
		xmlTextWriter.WriteAttributeString("style:name", "Tahoma");
		xmlTextWriter.WriteAttributeString("svg:font-family", "Tahoma");
		xmlTextWriter.WriteAttributeString("style:font-family-generic", "system");
		xmlTextWriter.WriteAttributeString("style:font-pitch", "variable");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("office:styles");
		xmlTextWriter.WriteStartElement("style:default-style");
		xmlTextWriter.WriteAttributeString("style:family", "graphic");
		xmlTextWriter.WriteStartElement("style:graphic-properties");
		xmlTextWriter.WriteAttributeString("draw:shadow-offset-x", "0.3cm");
		xmlTextWriter.WriteAttributeString("draw:shadow-offset-y", "0.3cm");
		xmlTextWriter.WriteAttributeString("draw:start-line-spacing-horizontal", "0.283cm");
		xmlTextWriter.WriteAttributeString("draw:start-line-spacing-vertical", "0.283cm");
		xmlTextWriter.WriteAttributeString("draw:end-line-spacing-horizontal", "0.283cm");
		xmlTextWriter.WriteAttributeString("draw:end-line-spacing-vertical", "0.283cm");
		xmlTextWriter.WriteAttributeString("style:flow-with-text", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("style:text-autospace", "ideograph-alpha");
		xmlTextWriter.WriteAttributeString("style:line-break", "strict");
		xmlTextWriter.WriteAttributeString("style:writing-mode", "lr-tb");
		xmlTextWriter.WriteAttributeString("style:font-independent-line-spacing", "false");
		xmlTextWriter.WriteStartElement("style:tab-stops");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:use-window-font-color", "true");
		xmlTextWriter.WriteAttributeString("fo:font-size", "10pt");
		xmlTextWriter.WriteAttributeString("style:letter-kerning", "true");
		xmlTextWriter.WriteAttributeString("style:font-size-asian", "12pt");
		xmlTextWriter.WriteAttributeString("style:language-asian", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-asian", "none");
		xmlTextWriter.WriteAttributeString("style:font-size-complex", "12pt");
		xmlTextWriter.WriteAttributeString("style:language-complex", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-complex", "none");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:default-style");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:hyphenation-ladder-count", "no-limit");
		xmlTextWriter.WriteAttributeString("style:text-autospace", "ideograph-alpha");
		xmlTextWriter.WriteAttributeString("style:punctuation-wrap", "hanging");
		xmlTextWriter.WriteAttributeString("style:line-break", "strict");
		xmlTextWriter.WriteAttributeString("style:tab-stop-distance", "1.251cm");
		xmlTextWriter.WriteAttributeString("style:writing-mode", "page");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:use-window-font-color", "true");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial");
		xmlTextWriter.WriteAttributeString("fo:font-size", "10pt");
		xmlTextWriter.WriteAttributeString("style:letter-kerning", "true");
		xmlTextWriter.WriteAttributeString("style:font-name-asian", "Lucida Sans Unicode");
		xmlTextWriter.WriteAttributeString("style:font-size-asian", "12pt");
		xmlTextWriter.WriteAttributeString("style:language-asian", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-asian", "none");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma");
		xmlTextWriter.WriteAttributeString("style:font-size-complex", "12pt");
		xmlTextWriter.WriteAttributeString("style:language-complex", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-complex", "none");
		xmlTextWriter.WriteAttributeString("fo:hyphenate", "false");
		xmlTextWriter.WriteAttributeString("fo:hyphenation-remain-char-count", "2");
		xmlTextWriter.WriteAttributeString("fo:hyphenation-push-char-count", "2");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:default-style");
		xmlTextWriter.WriteAttributeString("style:family", "table");
		xmlTextWriter.WriteStartElement("style:table-properties");
		xmlTextWriter.WriteAttributeString("table:border-model", "collapsing");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:default-style");
		xmlTextWriter.WriteAttributeString("style:family", "table-row");
		xmlTextWriter.WriteStartElement("style:table-row-properties");
		xmlTextWriter.WriteAttributeString("fo:keep-together", "auto");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Standard");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:class", "text");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Title");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Standard");
		xmlTextWriter.WriteAttributeString("style:next-style-name", "Text_20_body");
		xmlTextWriter.WriteAttributeString("style:class", "text");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:margin-top", "0.423cm");
		xmlTextWriter.WriteAttributeString("fo:margin-bottom", "0.212cm");
		xmlTextWriter.WriteAttributeString("fo:keep-with-next", "always");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial1");
		xmlTextWriter.WriteAttributeString("fo:font-size", "14pt");
		xmlTextWriter.WriteAttributeString("style:font-name-asian", "Lucida Sans Unicode");
		xmlTextWriter.WriteAttributeString("style:font-size-asian", "14pt");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma");
		xmlTextWriter.WriteAttributeString("style:font-size-complex", "14pt");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Text_20_body");
		xmlTextWriter.WriteAttributeString("style:display-name", "Text body");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Standard");
		xmlTextWriter.WriteAttributeString("style:class", "text");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:margin-top", "0cm");
		xmlTextWriter.WriteAttributeString("fo:margin-bottom", "0.212cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Subtitle");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Title");
		xmlTextWriter.WriteAttributeString("style:next-style-name", "Text_20_body");
		xmlTextWriter.WriteAttributeString("style:class", "chapter");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:text-align", "center");
		xmlTextWriter.WriteAttributeString("style:justify-single-word", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("fo:font-size", "14pt");
		xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
		xmlTextWriter.WriteAttributeString("style:font-size-asian", "14pt");
		xmlTextWriter.WriteAttributeString("style:font-style-asian", "italic");
		xmlTextWriter.WriteAttributeString("style:font-size-complex", "14pt");
		xmlTextWriter.WriteAttributeString("style:font-style-complex", "italic");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "List");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Text_20_body");
		xmlTextWriter.WriteAttributeString("style:class", "list");
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Caption");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Standard");
		xmlTextWriter.WriteAttributeString("style:class", "extra");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:margin-top", "0.212cm");
		xmlTextWriter.WriteAttributeString("fo:margin-bottom", "0.212cm");
		xmlTextWriter.WriteAttributeString("text:number-lines", "false");
		xmlTextWriter.WriteAttributeString("text:line-number", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial");
		xmlTextWriter.WriteAttributeString("fo:font-size", "10pt");
		xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
		xmlTextWriter.WriteAttributeString("style:font-size-asian", "12pt");
		xmlTextWriter.WriteAttributeString("style:font-style-asian", "italic");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma1");
		xmlTextWriter.WriteAttributeString("style:font-size-complex", "12pt");
		xmlTextWriter.WriteAttributeString("style:font-style-complex", "italic");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Index");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Standard");
		xmlTextWriter.WriteAttributeString("style:class", "index");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("text:number-lines", "false");
		xmlTextWriter.WriteAttributeString("text:line-number", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Table_20_Contents");
		xmlTextWriter.WriteAttributeString("style:display-name", "Table Contents");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Standard");
		xmlTextWriter.WriteAttributeString("style:class", "extra");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("text:number-lines", "false");
		xmlTextWriter.WriteAttributeString("text:line-number", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Graphics");
		xmlTextWriter.WriteAttributeString("style:family", "graphic");
		xmlTextWriter.WriteStartElement("style:graphic-properties");
		xmlTextWriter.WriteAttributeString("text:anchor-type", "paragraph");
		xmlTextWriter.WriteAttributeString("svg:x", "0cm");
		xmlTextWriter.WriteAttributeString("svg:y", "0cm");
		xmlTextWriter.WriteAttributeString("style:wrap", "dynamic");
		xmlTextWriter.WriteAttributeString("style:number-wrapped-paragraphs", "no-limit");
		xmlTextWriter.WriteAttributeString("style:wrap-contour", "false");
		xmlTextWriter.WriteAttributeString("style:vertical-pos", "top");
		xmlTextWriter.WriteAttributeString("style:vertical-rel", "paragraph");
		xmlTextWriter.WriteAttributeString("style:horizontal-pos", "center");
		xmlTextWriter.WriteAttributeString("style:horizontal-rel", "paragraph");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-style");
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "1");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "2");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "3");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "4");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "5");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "6");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "7");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "8");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "9");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:outline-level-style");
		xmlTextWriter.WriteAttributeString("text:level", "10");
		xmlTextWriter.WriteAttributeString("style:num-format", "");
		xmlTextWriter.WriteStartElement("style:list-level-properties");
		xmlTextWriter.WriteAttributeString("text:min-label-distance", "0.381cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("text:notes-configuration");
		xmlTextWriter.WriteAttributeString("text:note-class", "footnote");
		xmlTextWriter.WriteAttributeString("style:num-format", "1");
		xmlTextWriter.WriteAttributeString("text:start-value", "0");
		xmlTextWriter.WriteAttributeString("text:footnotes-position", "page");
		xmlTextWriter.WriteAttributeString("text:start-numbering-at", "document");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:notes-configuration");
		xmlTextWriter.WriteAttributeString("text:note-class", "endnote");
		xmlTextWriter.WriteAttributeString("style:num-format", "i");
		xmlTextWriter.WriteAttributeString("text:start-value", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:linenumbering-configuration");
		xmlTextWriter.WriteAttributeString("text:number-lines", "false");
		xmlTextWriter.WriteAttributeString("text:offset", "0.499cm");
		xmlTextWriter.WriteAttributeString("style:num-format", "1");
		xmlTextWriter.WriteAttributeString("text:number-position", "left");
		xmlTextWriter.WriteAttributeString("text:increment", "5");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		StiPage stiPage = pages[0];
		double number = stiPage.Unit.ConvertToHInches(stiPage.PageHeight * (double)stiPage.SegmentPerHeight);
		double number2 = stiPage.Unit.ConvertToHInches(stiPage.PageWidth * (double)stiPage.SegmentPerWidth);
		double number3 = stiPage.Unit.ConvertToHInches(stiPage.Margins.Left);
		double number4 = stiPage.Unit.ConvertToHInches(stiPage.Margins.Right);
		double number5 = stiPage.Unit.ConvertToHInches(stiPage.Margins.Top);
		double num = stiPage.Unit.ConvertToHInches(stiPage.Margins.Bottom) - 4.0;
		if (num < 0.0)
		{
			num = 0.0;
		}
		xmlTextWriter.WriteStartElement("office:automatic-styles");
		xmlTextWriter.WriteStartElement("style:page-layout");
		xmlTextWriter.WriteAttributeString("style:name", "pm1");
		xmlTextWriter.WriteStartElement("style:page-layout-properties");
		xmlTextWriter.WriteAttributeString("fo:page-width", DoubleToString(number2));
		xmlTextWriter.WriteAttributeString("fo:page-height", DoubleToString(number));
		xmlTextWriter.WriteAttributeString("style:num-format", "1");
		xmlTextWriter.WriteAttributeString("style:print-orientation", (stiPage.Orientation == StiPageOrientation.Portrait) ? "portrait" : "landscape");
		xmlTextWriter.WriteAttributeString("fo:margin-top", DoubleToString(number5));
		xmlTextWriter.WriteAttributeString("fo:margin-bottom", DoubleToString(num));
		xmlTextWriter.WriteAttributeString("fo:margin-left", DoubleToString(number3));
		xmlTextWriter.WriteAttributeString("fo:margin-right", DoubleToString(number4));
		xmlTextWriter.WriteAttributeString("style:writing-mode", "lr-tb");
		xmlTextWriter.WriteAttributeString("style:footnote-max-height", "0cm");
		xmlTextWriter.WriteStartElement("style:footnote-sep");
		xmlTextWriter.WriteAttributeString("style:width", "0.018cm");
		xmlTextWriter.WriteAttributeString("style:distance-before-sep", "0.101cm");
		xmlTextWriter.WriteAttributeString("style:distance-after-sep", "0.101cm");
		xmlTextWriter.WriteAttributeString("style:adjustment", "left");
		xmlTextWriter.WriteAttributeString("style:rel-width", "25%");
		xmlTextWriter.WriteAttributeString("style:color", "#000000");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:header-style");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:footer-style");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("office:master-styles");
		xmlTextWriter.WriteStartElement("style:master-page");
		xmlTextWriter.WriteAttributeString("style:name", "Standard");
		xmlTextWriter.WriteAttributeString("style:page-layout-name", "pm1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteContent()
	{
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Invalid comparison between Unknown and I4
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Invalid comparison between Unknown and I4
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Invalid comparison between Unknown and I4
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Invalid comparison between Unknown and I4
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Invalid comparison between Unknown and I4
		MemoryStream memoryStream = new StiCachedStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("office:document-content");
		xmlTextWriter.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:math", "http://www.w3.org/1998/Math/MathML");
		xmlTextWriter.WriteAttributeString("xmlns:form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
		xmlTextWriter.WriteAttributeString("xmlns:ooow", "http://openoffice.org/2004/writer");
		xmlTextWriter.WriteAttributeString("xmlns:oooc", "http://openoffice.org/2004/calc");
		xmlTextWriter.WriteAttributeString("xmlns:dom", "http://www.w3.org/2001/xml-events");
		xmlTextWriter.WriteAttributeString("xmlns:xforms", "http://www.w3.org/2002/xforms");
		xmlTextWriter.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
		xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
		xmlTextWriter.WriteAttributeString("office:version", "1.1");
		bool[,] array = new bool[Matrix.CoordY.Count, Matrix.CoordX.Count];
		int[,] array2 = new int[Matrix.CoordY.Count, Matrix.CoordX.Count];
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		int[] array3 = new int[Matrix.CoordY.Count];
		for (int i = 1; i < Matrix.CoordY.Count; i++)
		{
			double num = (double)Matrix.CoordY.GetByIndex(i) - (double)Matrix.CoordY.GetByIndex(i - 1);
			if (!hashtable.ContainsKey(num))
			{
				int count = hashtable.Count;
				hashtable[num] = count;
				hashtable2[count] = num;
			}
			array3[i - 1] = (int)hashtable[num];
			for (int j = 1; j < Matrix.CoordX.Count; j++)
			{
				StiCell stiCell = Matrix.Cells[i - 1, j - 1];
				if (array[i, j])
				{
					continue;
				}
				if (stiCell != null)
				{
					for (int k = 0; k <= stiCell.Height; k++)
					{
						for (int l = 0; l <= stiCell.Width; l++)
						{
							array[i + k, j + l] = true;
						}
					}
					array2[i - 1, j - 1] = GetCellStyleNumber(i - 1, j - 1, stiCell.Height + 1, stiCell.Width + 1);
					int paragraphStyleNumber = GetParagraphStyleNumber(i - 1, j - 1);
				}
				else
				{
					array2[i - 1, j - 1] = GetCellStyleNumber(i - 1, j - 1, 1, 1);
				}
			}
		}
		xmlTextWriter.WriteStartElement("office:scripts");
		xmlTextWriter.WriteEndElement();
		Hashtable hashtable3 = new Hashtable();
		foreach (ParagraphStyleData paragraphStyle in paragraphStyleList)
		{
			hashtable3[paragraphStyle.FontName] = paragraphStyle.FontName;
		}
		xmlTextWriter.WriteStartElement("office:font-face-decls");
		foreach (DictionaryEntry item in hashtable3)
		{
			string value = (string)item.Value;
			xmlTextWriter.WriteStartElement("style:font-face");
			xmlTextWriter.WriteAttributeString("style:name", value);
			xmlTextWriter.WriteAttributeString("svg:font-family", value);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("office:automatic-styles");
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Table1");
		xmlTextWriter.WriteAttributeString("style:family", "table");
		xmlTextWriter.WriteStartElement("style:table-properties");
		double number = (double)Matrix.CoordX.GetByIndex(Matrix.CoordX.Count - 1) - (double)Matrix.CoordX.GetByIndex(0);
		xmlTextWriter.WriteAttributeString("style:width", DoubleToString(number));
		xmlTextWriter.WriteAttributeString("table:align", "left");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		for (int m = 0; m < Matrix.CoordX.Count - 1; m++)
		{
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"Table1.{GetColumnName(m)}");
			xmlTextWriter.WriteAttributeString("style:family", "table-column");
			xmlTextWriter.WriteStartElement("style:table-column-properties");
			double number2 = (double)Matrix.CoordX.GetByIndex(m + 1) - (double)Matrix.CoordX.GetByIndex(m);
			xmlTextWriter.WriteAttributeString("style:column-width", DoubleToString(number2));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		for (int n = 0; n < hashtable.Count; n++)
		{
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"Table1.{n + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "table-row");
			xmlTextWriter.WriteStartElement("style:table-row-properties");
			double number3 = (double)hashtable2[n];
			xmlTextWriter.WriteAttributeString("style:row-height", DoubleToString(number3));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		for (int num2 = 0; num2 < cellStyleList.Count; num2++)
		{
			CellStyleData cellStyleData = (CellStyleData)cellStyleList[num2];
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"cell{num2 + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "table-cell");
			xmlTextWriter.WriteStartElement("style:table-cell-properties");
			if ((int)cellStyleData.VertAlign == 1)
			{
				xmlTextWriter.WriteAttributeString("style:vertical-align", "middle");
			}
			if ((int)cellStyleData.VertAlign == 2)
			{
				xmlTextWriter.WriteAttributeString("style:vertical-align", "bottom");
			}
			if (cellStyleData.BackColor != Color.Transparent)
			{
				xmlTextWriter.WriteAttributeString("fo:background-color", GetColorString(cellStyleData.BackColor));
			}
			else
			{
				xmlTextWriter.WriteAttributeString("fo:background-color", "transparent");
			}
			xmlTextWriter.WriteAttributeString("fo:padding", "0in");
			xmlTextWriter.WriteAttributeString("fo:border-left", cellStyleData.BorderLeft);
			xmlTextWriter.WriteAttributeString("fo:border-right", cellStyleData.BorderRight);
			xmlTextWriter.WriteAttributeString("fo:border-top", cellStyleData.BorderTop);
			xmlTextWriter.WriteAttributeString("fo:border-bottom", cellStyleData.BorderBottom);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		for (int num3 = 0; num3 < paragraphStyleList.Count; num3++)
		{
			ParagraphStyleData paragraphStyleData2 = (ParagraphStyleData)paragraphStyleList[num3];
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"par{num3 + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "paragraph");
			xmlTextWriter.WriteAttributeString("style:parent-style-name", "Table_20_Contents");
			if ((int)paragraphStyleData2.HorAlign != 0)
			{
				xmlTextWriter.WriteStartElement("style:paragraph-properties");
				if ((int)paragraphStyleData2.HorAlign == 1)
				{
					xmlTextWriter.WriteAttributeString("fo:text-align", "center");
				}
				if ((int)paragraphStyleData2.HorAlign == 2)
				{
					xmlTextWriter.WriteAttributeString("fo:text-align", "right");
				}
				if ((int)paragraphStyleData2.HorAlign == 3)
				{
					xmlTextWriter.WriteAttributeString("fo:text-align", "justify");
				}
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteStartElement("style:text-properties");
			xmlTextWriter.WriteAttributeString("fo:color", GetColorString(paragraphStyleData2.FontColor));
			xmlTextWriter.WriteAttributeString("style:font-name", paragraphStyleData2.FontName);
			string value2 = $"{paragraphStyleData2.FontSize}pt".Replace(",", ".");
			xmlTextWriter.WriteAttributeString("fo:font-size", value2);
			xmlTextWriter.WriteAttributeString("fo:font-size-asian", value2);
			xmlTextWriter.WriteAttributeString("fo:font-size-complex", value2);
			if (paragraphStyleData2.Italic)
			{
				xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
				xmlTextWriter.WriteAttributeString("fo:font-style-asian", "italic");
				xmlTextWriter.WriteAttributeString("fo:font-style-complex", "italic");
			}
			if (paragraphStyleData2.Underline)
			{
				xmlTextWriter.WriteAttributeString("style:text-underline-style", "solid");
				xmlTextWriter.WriteAttributeString("style:text-underline-width", "auto");
				xmlTextWriter.WriteAttributeString("style:text-underline-color", "font-color");
			}
			if (paragraphStyleData2.Bold)
			{
				xmlTextWriter.WriteAttributeString("fo:font-weight", "bold");
				xmlTextWriter.WriteAttributeString("fo:font-weight-asian", "bold");
				xmlTextWriter.WriteAttributeString("fo:font-weight-complex", "bold");
			}
			if (paragraphStyleData2.Angle != 0)
			{
				xmlTextWriter.WriteAttributeString("style:text-rotation-angle", $"{paragraphStyleData2.Angle}");
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "parEnd");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Table_20_Contents");
		xmlTextWriter.WriteStartElement("style:text-properties");
		string value3 = $"{1}pt".Replace(",", ".");
		xmlTextWriter.WriteAttributeString("fo:font-size", value3);
		xmlTextWriter.WriteAttributeString("fo:font-size-asian", value3);
		xmlTextWriter.WriteAttributeString("fo:font-size-complex", value3);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "fr1");
		xmlTextWriter.WriteAttributeString("style:family", "graphic");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Graphics");
		xmlTextWriter.WriteStartElement("style:graphic-properties");
		xmlTextWriter.WriteAttributeString("style:vertical-pos", "middle");
		xmlTextWriter.WriteAttributeString("style:vertical-rel", "paragraph");
		xmlTextWriter.WriteAttributeString("style:horizontal-pos", "center");
		xmlTextWriter.WriteAttributeString("style:horizontal-rel", "paragraph");
		xmlTextWriter.WriteAttributeString("fo:background-color", "transparent");
		xmlTextWriter.WriteAttributeString("style:background-transparency", "100%");
		xmlTextWriter.WriteAttributeString("style:shadow", "none");
		xmlTextWriter.WriteAttributeString("style:mirror", "none");
		xmlTextWriter.WriteAttributeString("fo:clip", "rect(0cm 0cm 0cm 0cm)");
		xmlTextWriter.WriteAttributeString("draw:luminance", "0%");
		xmlTextWriter.WriteAttributeString("draw:contrast", "0%");
		xmlTextWriter.WriteAttributeString("draw:red", "0%");
		xmlTextWriter.WriteAttributeString("draw:green", "0%");
		xmlTextWriter.WriteAttributeString("draw:blue", "0%");
		xmlTextWriter.WriteAttributeString("draw:gamma", "100%");
		xmlTextWriter.WriteAttributeString("draw:color-inversion", "false");
		xmlTextWriter.WriteAttributeString("draw:image-opacity", "100%");
		xmlTextWriter.WriteAttributeString("draw:color-mode", "standard");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("office:body");
		xmlTextWriter.WriteStartElement("office:text");
		xmlTextWriter.WriteStartElement("office:forms");
		xmlTextWriter.WriteAttributeString("form:automatic-focus", "false");
		xmlTextWriter.WriteAttributeString("form:apply-design-mode", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:sequence-decls");
		xmlTextWriter.WriteStartElement("text:sequence-decl");
		xmlTextWriter.WriteAttributeString("text:display-outline-level", "0");
		xmlTextWriter.WriteAttributeString("text:name", "Illustration");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:sequence-decl");
		xmlTextWriter.WriteAttributeString("text:display-outline-level", "0");
		xmlTextWriter.WriteAttributeString("text:name", "Table");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:sequence-decl");
		xmlTextWriter.WriteAttributeString("text:display-outline-level", "0");
		xmlTextWriter.WriteAttributeString("text:name", "Text");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:sequence-decl");
		xmlTextWriter.WriteAttributeString("text:display-outline-level", "0");
		xmlTextWriter.WriteAttributeString("text:name", "Drawing");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		array = new bool[Matrix.CoordY.Count, Matrix.CoordX.Count];
		xmlTextWriter.WriteStartElement("table:table");
		xmlTextWriter.WriteAttributeString("table:name", "Table1");
		xmlTextWriter.WriteAttributeString("table:style-name", "Table1");
		for (int num4 = 0; num4 < Matrix.CoordX.Count - 1; num4++)
		{
			xmlTextWriter.WriteStartElement("table:table-column");
			xmlTextWriter.WriteAttributeString("table:style-name", $"Table1.{GetColumnName(num4)}");
			xmlTextWriter.WriteEndElement();
		}
		double num5 = Math.Max((float)matrix.CoordY.Count / 200f, 1f);
		int num6 = 0;
		for (int num7 = 1; num7 < Matrix.CoordY.Count; num7++)
		{
			int num8 = (int)((double)num7 / num5);
			if (num8 > num6)
			{
				num6 = num8;
				InvokeExporting(num7, matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			xmlTextWriter.WriteStartElement("table:table-row");
			xmlTextWriter.WriteAttributeString("table:style-name", $"Table1.{array3[num7 - 1] + 1}");
			for (int num9 = 1; num9 < Matrix.CoordX.Count; num9++)
			{
				StiCell stiCell2 = Matrix.Cells[num7 - 1, num9 - 1];
				if (!array[num7, num9])
				{
					if (stiCell2 != null)
					{
						for (int num10 = 0; num10 <= stiCell2.Height; num10++)
						{
							for (int num11 = 0; num11 <= stiCell2.Width; num11++)
							{
								array[num7 + num10, num9 + num11] = true;
							}
						}
						int num12 = array2[num7 - 1, num9 - 1];
						int paragraphStyleNumber2 = GetParagraphStyleNumber(num7 - 1, num9 - 1);
						xmlTextWriter.WriteStartElement("table:table-cell");
						xmlTextWriter.WriteAttributeString("table:style-name", $"cell{num12 + 1}");
						if (stiCell2.Width > 0)
						{
							xmlTextWriter.WriteAttributeString("table:number-columns-spanned", $"{stiCell2.Width + 1}");
						}
						if (stiCell2.Height > 0)
						{
							xmlTextWriter.WriteAttributeString("table:number-rows-spanned", $"{stiCell2.Height + 1}");
						}
						xmlTextWriter.WriteAttributeString("office:value-type", "string");
						if (stiCell2.Component is StiText && !stiCell2.Component.IsExportAsImage(StiExportFormat.Odt) && stiCell2.Text != null)
						{
							string text = stiCell2.Text;
							if ((paragraphStyleList[paragraphStyleNumber2] as ParagraphStyleData).Angle != 0)
							{
								text = text.Replace('\n', ' ');
							}
							ArrayList arrayList = new ArrayList();
							string text2 = string.Empty;
							string text3 = text;
							for (int num13 = 0; num13 < text3.Length; num13++)
							{
								char c = text3[num13];
								if (char.IsControl(c))
								{
									switch (c)
									{
									case '\n':
										arrayList.Add(text2);
										text2 = string.Empty;
										continue;
									case '\t':
										break;
									default:
										continue;
									}
								}
								text2 += c;
							}
							if (text2 != string.Empty)
							{
								arrayList.Add(text2);
							}
							if (arrayList.Count == 0)
							{
								arrayList.Add(text2);
							}
							for (int num14 = 0; num14 < arrayList.Count; num14++)
							{
								string text4 = (string)arrayList[num14];
								xmlTextWriter.WriteStartElement("text:p");
								xmlTextWriter.WriteAttributeString("text:style-name", $"par{paragraphStyleNumber2 + 1}");
								xmlTextWriter.WriteString(text4);
								xmlTextWriter.WriteEndElement();
							}
						}
						if (stiCell2.Component.IsExportAsImage(StiExportFormat.Odt) && stiCell2.Component is IStiExportImage stiExportImage)
						{
							float zoom = imageResolution;
							Image image = stiExportImage.GetImage(ref zoom);
							try
							{
								if (image != null)
								{
									int num15 = imageCache.AddImageInt(image);
									double number4 = (double)Matrix.CoordX.GetByIndex(num9 + stiCell2.Width) - (double)Matrix.CoordX.GetByIndex(num9 - 1);
									double number5 = (double)Matrix.CoordY.GetByIndex(num7 + stiCell2.Height) - (double)Matrix.CoordY.GetByIndex(num7 - 1);
									xmlTextWriter.WriteStartElement("text:p");
									xmlTextWriter.WriteAttributeString("text:style-name", "Table_20_Contents");
									xmlTextWriter.WriteStartElement("draw:frame");
									xmlTextWriter.WriteAttributeString("draw:style-name", "fr1");
									xmlTextWriter.WriteAttributeString("draw:name", $"Picture{num15 + 1}");
									xmlTextWriter.WriteAttributeString("text:anchor-type", "paragraph");
									xmlTextWriter.WriteAttributeString("svg:width", DoubleToString(number4));
									xmlTextWriter.WriteAttributeString("svg:height", DoubleToString(number5));
									xmlTextWriter.WriteAttributeString("draw:z-index", "0");
									xmlTextWriter.WriteStartElement("draw:image");
									xmlTextWriter.WriteAttributeString("xlink:href", $"Pictures/{num15 + 1:D5}.jpg");
									xmlTextWriter.WriteAttributeString("xlink:type", "simple");
									xmlTextWriter.WriteAttributeString("xlink:show", "embed");
									xmlTextWriter.WriteAttributeString("xlink:actuate", "onLoad");
									xmlTextWriter.WriteEndElement();
									xmlTextWriter.WriteEndElement();
									xmlTextWriter.WriteEndElement();
								}
							}
							finally
							{
								((IDisposable)image)?.Dispose();
							}
						}
						xmlTextWriter.WriteEndElement();
					}
					else
					{
						int num16 = array2[num7 - 1, num9 - 1];
						xmlTextWriter.WriteStartElement("table:table-cell");
						xmlTextWriter.WriteAttributeString("table:style-name", $"cell{num16 + 1}");
						xmlTextWriter.WriteEndElement();
					}
				}
				else
				{
					xmlTextWriter.WriteStartElement("table:covered-table-cell");
					xmlTextWriter.WriteEndElement();
				}
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("text:p");
		xmlTextWriter.WriteAttributeString("text:style-name", "parEnd");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	public void ExportOdt(StiReport report, string fileName)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(fileName);
			fileStream = new FileStream(fileName, FileMode.Create);
			ExportOdt(report, fileStream);
		}
		finally
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public void ExportOdt(StiReport report, Stream stream)
	{
		StiOdtExportSettings settings = new StiOdtExportSettings();
		ExportOdt(report, stream, settings);
	}

	public void ExportOdt(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiOdtExportSettings stiOdtExportSettings = new StiOdtExportSettings();
		stiOdtExportSettings.PageRange = pageRange;
		ExportOdt(report, stream, stiOdtExportSettings);
	}

	public void ExportOdt(StiReport report, Stream stream, StiOdtExportSettings settings)
	{
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Odt format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		usePageHeadersAndFooters = settings.UsePageHeadersAndFooters;
		removeEmptySpaceAtBottom = settings.RemoveEmptySpaceAtBottom;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		xmlIndentation = -1;
		if (imageQuality < 0f)
		{
			imageQuality = 0f;
		}
		if (imageQuality > 1f)
		{
			imageQuality = 1f;
		}
		if (imageResolution < 10f)
		{
			imageResolution = 10f;
		}
		imageResolution /= 100f;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			imageCache = new StiImageCache(StiOptions.Export.OpenDocumentWriter.AllowImageComparer, ImageFormat.Jpeg, imageQuality);
			cellStyleList = new ArrayList();
			paragraphStyleList = new ArrayList();
			base.CurrentPassNumber = 0;
			base.MaximumPassNumber = 3 + (StiOptions.Export.OpenDocumentWriter.DivideSegmentPages ? 1 : 0);
			StiPagesCollection pages = pageRange.GetSelectedPages(report.RenderedPages);
			if (StiOptions.Export.OpenDocumentWriter.DivideSegmentPages)
			{
				pages = StiSegmentPagesDivider.Divide(pages, this);
				base.CurrentPassNumber++;
			}
			if (base.IsStopped)
			{
				return;
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			StiZipWriter20 val = new StiZipWriter20();
			val.Begin(stream, true);
			matrix = new StiMatrix(pages, checkForExcel: true, this);
			if (base.IsStopped)
			{
				return;
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			base.CurrentPassNumber = 2 + (StiOptions.Export.OpenDocumentWriter.DivideSegmentPages ? 1 : 0);
			val.AddFile("content.xml", WriteContent());
			val.AddFile("mimetype", WriteMimetype());
			val.AddFile("meta.xml", WriteMeta());
			val.AddFile("META-INF/manifest.xml", WriteManifest());
			val.AddFile("settings.xml", WriteSettings());
			val.AddFile("styles.xml", WriteStyles(pages));
			if (imageCache.ImagePackedStore.Count > 0)
			{
				for (int i = 0; i < imageCache.ImagePackedStore.Count; i++)
				{
					val.AddFile($"Pictures/{i + 1:D5}.jpg", WriteImage(i));
				}
			}
			val.End();
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			if (matrix != null)
			{
				matrix.Clear();
				matrix = null;
			}
			cellStyleList.Clear();
			cellStyleList = null;
			paragraphStyleList.Clear();
			paragraphStyleList = null;
			imageCache.Clear();
			imageCache = null;
			if (report.RenderedPages.CacheMode)
			{
				StiMatrix.GCCollect();
			}
		}
	}
}
