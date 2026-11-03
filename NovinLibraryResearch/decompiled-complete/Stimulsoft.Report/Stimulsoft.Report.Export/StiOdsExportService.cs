using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Export.Tools;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Ods.png")]
public class StiOdsExportService : StiExportService
{
	private class CellStyleData
	{
		public string BorderLeft;

		public string BorderRight;

		public string BorderTop;

		public string BorderBottom;

		public Color BackColor;

		public StiVertAlignment VertAlign;

		public string FontName;

		public float FontSize;

		public bool Bold;

		public bool Italic;

		public bool Underline;

		public Color FontColor;

		public StiTextHorAlignment HorAlign;

		public int Angle;

		public bool Wordwrap;

		public int DataStyle;

		public CellStyleData()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			BorderLeft = "none";
			BorderRight = "none";
			BorderTop = "none";
			BorderBottom = "none";
			BackColor = Color.Transparent;
			VertAlign = (StiVertAlignment)2;
			FontName = "Arial";
			FontSize = 6f;
			Bold = false;
			Italic = false;
			Underline = false;
			FontColor = Color.Black;
			HorAlign = (StiTextHorAlignment)0;
			Angle = 0;
			Wordwrap = false;
			DataStyle = -1;
		}
	}

	private class DataStyleData
	{
		public bool isNumeric;

		public bool isCurrency;

		public bool isPercent;

		public bool isDate;

		public bool isTime;

		public bool isDefaultFormat;

		public int DecimalDigits;

		public char DecimalComma;

		public int GroupDigits;

		public string CurrencySymbol;

		public bool CurrencyPositionBefore;

		public bool NegativeBraces;

		public string DateTimeFormatString;

		public string CurrencyPositivePattern;

		public string CurrencyNegativePattern;

		public DataStyleData()
		{
			isNumeric = false;
			isCurrency = false;
			isPercent = false;
			isDate = false;
			isTime = false;
			isDefaultFormat = false;
			DecimalDigits = 2;
			DecimalComma = ' ';
			GroupDigits = 0;
			CurrencySymbol = CultureInfo.InstalledUICulture.NumberFormat.CurrencySymbol;
			CurrencyPositionBefore = false;
			NegativeBraces = false;
			DateTimeFormatString = null;
			CurrencyPositivePattern = null;
			CurrencyNegativePattern = null;
		}
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiMatrix matrix;

	private StiImageCache imageCache;

	private ArrayList cellStyleList;

	private ArrayList dataStyleList;

	private ArrayList sheetNameList;

	private ArrayList matrixList;

	private ArrayList firstPageIndexList;

	private ArrayList minRowList;

	private ArrayList maxRowList;

	private ArrayList cellStyleTableList;

	private int xmlIndentation = 1;

	private float imageQuality = 0.75f;

	private float imageResolution = 96f;

	private CultureInfo currentCulture;

	public override string DefaultExtension => "ods";

	public override StiExportFormat ExportFormat => StiExportFormat.Ods;

	public override string GroupCategory => "Excel";

	public override int Position => 33;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeCalcFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportOds(report, stream, settings as StiOdsExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiOdsExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(FormComplete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "CalcFiles");
	}

	private static string DoubleToString(double number)
	{
		return Math.Round(number / 100.0, 4).ToString().Replace(",", ".") + "in";
	}

	private void FormComplete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (!e.DialogResult)
		{
			base.IsStopped = true;
			return;
		}
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
			StiOdsExportSettings stiOdsExportSettings = new StiOdsExportSettings();
			stiOdsExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
			stiOdsExportSettings.ImageQuality = (float)form["ImageQuality"];
			stiOdsExportSettings.ImageResolution = (float)form["Resolution"];
			StiOdsExportSettings settings = stiOdsExportSettings;
			StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
		}
		catch (Exception ex)
		{
			throw ex;
		}
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
		return "#" + color.ToArgb().ToString("X6").Substring(2);
	}

	private int GetCellStyleNumber(int indexRow, int indexColumn, int height, int width)
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
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
			cellStyleData.BorderTop = GetStringFromBorder(matrix.BordersX[indexRow, indexColumn]);
		}
		if (flag)
		{
			cellStyleData.BorderLeft = GetStringFromBorder(matrix.BordersY[indexRow, indexColumn]);
		}
		if (flag4)
		{
			cellStyleData.BorderBottom = GetStringFromBorder(matrix.BordersX[indexRow + height, indexColumn]);
		}
		if (flag2)
		{
			cellStyleData.BorderRight = GetStringFromBorder(matrix.BordersY[indexRow, indexColumn + width]);
		}
		if (matrix.Cells[indexRow, indexColumn] != null)
		{
			StiCell stiCell = matrix.Cells[indexRow, indexColumn];
			StiCellStyle cellStyle = stiCell.CellStyle;
			cellStyleData.FontName = cellStyle.Font.Name;
			cellStyleData.FontSize = cellStyle.Font.SizeInPoints;
			cellStyleData.Bold = cellStyle.Font.Bold;
			cellStyleData.Italic = cellStyle.Font.Italic;
			cellStyleData.Underline = cellStyle.Font.Underline;
			cellStyleData.FontColor = cellStyle.TextColor;
			cellStyleData.BackColor = cellStyle.Color;
			cellStyleData.VertAlign = cellStyle.VertAlignment;
			cellStyleData.HorAlign = cellStyle.HorAlignment;
			double a = 0.0;
			if (cellStyle.TextOptions != null)
			{
				a = cellStyle.TextOptions.Angle;
				cellStyleData.Wordwrap = cellStyle.TextOptions.WordWrap;
			}
			cellStyleData.Angle = (int)Math.Round(a);
			if (stiCell.Component != null)
			{
				if (stiCell.Component is StiText textComp)
				{
					cellStyleData.DataStyle = GetDataStyleNumber(textComp);
				}
				if (stiCell.Component is StiCheckBox stiCheckBox && !string.IsNullOrEmpty(stiCheckBox.ExcelDataValue))
				{
					cellStyleData.FontName = StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Name;
					cellStyleData.FontSize = StiOptions.Export.CheckBoxReplacementForExcelValue.Font.SizeInPoints;
					cellStyleData.Bold = StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Bold;
					cellStyleData.Italic = StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Italic;
					cellStyleData.Underline = StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Underline;
					cellStyleData.VertAlign = StiOptions.Export.CheckBoxReplacementForExcelValue.VertAlignment;
					cellStyleData.HorAlign = StiOptions.Export.CheckBoxReplacementForExcelValue.HorAlignment;
				}
			}
		}
		if (cellStyleList.Count > 0)
		{
			for (int k = 0; k < cellStyleList.Count; k++)
			{
				CellStyleData cellStyleData2 = (CellStyleData)cellStyleList[k];
				if (cellStyleData2.BorderLeft == cellStyleData.BorderLeft && cellStyleData2.BorderRight == cellStyleData.BorderRight && cellStyleData2.BorderTop == cellStyleData.BorderTop && cellStyleData2.BorderBottom == cellStyleData.BorderBottom && cellStyleData2.BackColor == cellStyleData.BackColor && cellStyleData2.VertAlign == cellStyleData.VertAlign && cellStyleData2.FontName == cellStyleData.FontName && cellStyleData2.FontSize == cellStyleData.FontSize && cellStyleData2.Bold == cellStyleData.Bold && cellStyleData2.Italic == cellStyleData.Italic && cellStyleData2.Underline == cellStyleData.Underline && cellStyleData2.FontColor == cellStyleData.FontColor && cellStyleData2.HorAlign == cellStyleData.HorAlign && cellStyleData2.Angle == cellStyleData.Angle && cellStyleData2.Wordwrap == cellStyleData.Wordwrap && cellStyleData2.DataStyle == cellStyleData.DataStyle)
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
		return DoubleToString(border.Size) + " solid " + GetColorString(border.Color);
	}

	private int GetDataStyleNumber(StiText textComp)
	{
		DataStyleData dataStyleData = new DataStyleData();
		string text = textComp.Format;
		bool flag = textComp?.HideZeros ?? false;
		int num = text?.IndexOf("|") ?? (-1);
		if (num != -1)
		{
			dataStyleData.CurrencyPositivePattern = StiExportUtils.GetPositivePattern(text[num + 1] - 65);
			dataStyleData.CurrencyNegativePattern = StiExportUtils.GetNegativePattern(text[num + 2] - 65);
			text = text.Substring(0, num);
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (text[0] == 'C')
			{
				dataStyleData.isCurrency = true;
			}
			if (text[0] == 'N')
			{
				dataStyleData.isNumeric = true;
			}
			if (text[0] == 'P')
			{
				dataStyleData.isPercent = true;
			}
			if (text[0] == 'D')
			{
				dataStyleData.isDate = true;
				if (text.Length > 1)
				{
					dataStyleData.DateTimeFormatString = text.Substring(1);
				}
			}
			if (text[0] == 'T')
			{
				dataStyleData.isTime = true;
				if (text.Length > 1)
				{
					dataStyleData.DateTimeFormatString = text.Substring(1);
				}
			}
			if (text.Length == 1)
			{
				dataStyleData.isDefaultFormat = true;
			}
			else if (dataStyleData.isCurrency || dataStyleData.isNumeric || dataStyleData.isPercent)
			{
				int i = 1;
				if (char.IsDigit(text[i]))
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (; i < text.Length && char.IsDigit(text[i]); i++)
					{
						stringBuilder.Append(text[i]);
					}
					dataStyleData.DecimalDigits = int.Parse(stringBuilder.ToString());
				}
				if (i < text.Length && text[i] == 'G')
				{
					i++;
					dataStyleData.GroupDigits = 3;
				}
				if (i < text.Length && text[i] == '(')
				{
					i++;
					dataStyleData.NegativeBraces = true;
				}
				if (i < text.Length && (text[i] == '.' || text[i] == ','))
				{
					dataStyleData.DecimalComma = text[i];
					i++;
				}
				if (i < text.Length && (text[i] == '+' || text[i] == '-'))
				{
					if (text[i] == '+')
					{
						dataStyleData.CurrencyPositionBefore = true;
					}
					i++;
					if (i < text.Length)
					{
						dataStyleData.CurrencySymbol = text.Substring(i);
					}
				}
			}
			if (textComp.HideZeros && !string.IsNullOrEmpty(text))
			{
				dataStyleData.isDefaultFormat = false;
			}
			if (dataStyleList.Count > 0)
			{
				for (int j = 0; j < dataStyleList.Count; j++)
				{
					DataStyleData dataStyleData2 = (DataStyleData)dataStyleList[j];
					if (dataStyleData2.isNumeric == dataStyleData.isNumeric && dataStyleData2.isCurrency == dataStyleData.isCurrency && dataStyleData2.isPercent == dataStyleData.isPercent && dataStyleData2.isDate == dataStyleData.isDate && dataStyleData2.isTime == dataStyleData.isTime && dataStyleData2.isDefaultFormat == dataStyleData.isDefaultFormat && dataStyleData2.DecimalDigits == dataStyleData.DecimalDigits && dataStyleData2.DecimalComma == dataStyleData.DecimalComma && dataStyleData2.GroupDigits == dataStyleData.GroupDigits && dataStyleData2.CurrencySymbol == dataStyleData.CurrencySymbol && dataStyleData2.CurrencyPositionBefore == dataStyleData.CurrencyPositionBefore && dataStyleData2.DateTimeFormatString == dataStyleData.DateTimeFormatString && dataStyleData2.CurrencyPositivePattern == dataStyleData.CurrencyPositivePattern && dataStyleData2.CurrencyNegativePattern == dataStyleData.CurrencyNegativePattern)
					{
						return j;
					}
				}
			}
			dataStyleList.Add(dataStyleData);
			return dataStyleList.Count - 1;
		}
		return -1;
	}

	private MemoryStream WriteMimetype()
	{
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.ASCII);
		streamWriter.Write("application/vnd.oasis.opendocument.spreadsheet");
		streamWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteMeta()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("office:document-meta");
		xmlTextWriter.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		xmlTextWriter.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
		xmlTextWriter.WriteAttributeString("office:version", "1.1");
		xmlTextWriter.WriteStartElement("office:meta");
		string value = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}";
		string reportVersion = StiExportUtils.GetReportVersion(report);
		xmlTextWriter.WriteElementString("meta:generator", reportVersion);
		xmlTextWriter.WriteElementString("meta:creation-date", value);
		xmlTextWriter.WriteElementString("dc:date", value);
		xmlTextWriter.WriteElementString("meta:editing-cycles", "1");
		xmlTextWriter.WriteElementString("meta:editing-duration", "PT0M0S");
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
		xmlTextWriter.WriteAttributeString("meta:cell-count", "1");
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
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("manifest:manifest");
		xmlTextWriter.WriteAttributeString("xmlns:manifest", "urn:oasis:names:tc:opendocument:xmlns:manifest:1.0");
		xmlTextWriter.WriteStartElement("manifest:file-entry");
		xmlTextWriter.WriteAttributeString("manifest:media-type", "application/vnd.oasis.opendocument.spreadsheet");
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
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
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
		xmlTextWriter.WriteAttributeString("config:name", "VisibleAreaTop");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleAreaLeft");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("-10107");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleAreaWidth");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("43208");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VisibleAreaHeight");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("22174");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item-map-indexed");
		xmlTextWriter.WriteAttributeString("config:name", "Views");
		xmlTextWriter.WriteStartElement("config:config-item-map-entry");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ViewId");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteString("View1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item-map-named");
		xmlTextWriter.WriteAttributeString("config:name", "Tables");
		xmlTextWriter.WriteStartElement("config:config-item-map-entry");
		xmlTextWriter.WriteAttributeString("config:name", "Sheet1");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CursorPositionX");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CursorPositionY");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HorizontalSplitMode");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VerticalSplitMode");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HorizontalSplitPosition");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "VerticalSplitPosition");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ActiveSplitRange");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("2");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PositionLeft");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PositionRight");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PositionTop");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PositionBottom");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ActiveTable");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteString("Sheet1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HorizontalScrollbarWidth");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("270");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ZoomType");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ZoomValue");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("100");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PageViewZoomValue");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("60");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowPageBreakPreview");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowZeroValues");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowNotes");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowGrid");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "GridColor");
		xmlTextWriter.WriteAttributeString("config:type", "long");
		xmlTextWriter.WriteString("12632256");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowPageBreaks");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HasColumnRowHeaders");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HasSheetTabs");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsOutlineSymbolsSet");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsSnapToRaster");
		xmlTextWriter.WriteAttributeString("config:type", "long");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterIsVisible");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterResolutionX");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1000");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterResolutionY");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1000");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterSubdivisionX");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterSubdivisionY");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsRasterAxisSynchronized");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item-set");
		xmlTextWriter.WriteAttributeString("config:name", "ooo:configuration-settings");
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowZeroValues");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowNotes");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowGrid");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "GridColor");
		xmlTextWriter.WriteAttributeString("config:type", "long");
		xmlTextWriter.WriteString("12632256");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ShowPageBreaks");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "LinkUpdateMode");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("3");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HasColumnRowHeaders");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "HasSheetTabs");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsOutlineSymbolsSet");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsSnapToRaster");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterIsVisible");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterResolutionX");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1000");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterResolutionY");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1000");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterSubdivisionX");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "RasterSubdivisionY");
		xmlTextWriter.WriteAttributeString("config:type", "int");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsRasterAxisSynchronized");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AutoCalculate");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrinterName");
		xmlTextWriter.WriteAttributeString("config:type", "string");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "PrinterSetup");
		xmlTextWriter.WriteAttributeString("config:type", "base64Binary");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "ApplyUserData");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "CharacterCompressionType");
		xmlTextWriter.WriteAttributeString("config:type", "short");
		xmlTextWriter.WriteString("0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "IsKernAsianPunctuation");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "SaveVersionOnClose");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "UpdateFromTemplate");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "AllowPrintJobCancel");
		xmlTextWriter.WriteAttributeString("config:type", "boolean");
		xmlTextWriter.WriteString("true");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("config:config-item");
		xmlTextWriter.WriteAttributeString("config:name", "LoadReadonly");
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
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
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
		xmlTextWriter.WriteAttributeString("xmlns:presentation", "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
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
		xmlTextWriter.WriteAttributeString("style:family", "table-cell");
		xmlTextWriter.WriteStartElement("style:table-cell-properties");
		xmlTextWriter.WriteAttributeString("style:decimal-places", "2");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("style:tab-stop-distance", "1.25cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("style:font-name", "Arial");
		xmlTextWriter.WriteAttributeString("fo:language", "ru");
		xmlTextWriter.WriteAttributeString("fo:country", "RU");
		xmlTextWriter.WriteAttributeString("style:font-name-asian", "Lucida Sans Unicode");
		xmlTextWriter.WriteAttributeString("style:language-asian", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-asian", "none");
		xmlTextWriter.WriteAttributeString("style:font-name-complex", "Tahoma");
		xmlTextWriter.WriteAttributeString("style:language-complex", "zxx");
		xmlTextWriter.WriteAttributeString("style:country-complex", "none");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("number:number-style");
		xmlTextWriter.WriteAttributeString("style:name", "N0");
		xmlTextWriter.WriteStartElement("number:number");
		xmlTextWriter.WriteAttributeString("number:min-integer-digits", "1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Default");
		xmlTextWriter.WriteAttributeString("style:family", "table-cell");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Result");
		xmlTextWriter.WriteAttributeString("style:family", "table-cell");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Default");
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
		xmlTextWriter.WriteAttributeString("style:text-underline-style", "solid");
		xmlTextWriter.WriteAttributeString("style:text-underline-width", "auto");
		xmlTextWriter.WriteAttributeString("style:text-underline-color", "font-color");
		xmlTextWriter.WriteAttributeString("fo:font-weight", "bold");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Heading");
		xmlTextWriter.WriteAttributeString("style:family", "table-cell");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Default");
		xmlTextWriter.WriteStartElement("style:table-cell-properties");
		xmlTextWriter.WriteAttributeString("style:text-align-source", "fix");
		xmlTextWriter.WriteAttributeString("style:repeat-content", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:text-align", "center");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:text-properties");
		xmlTextWriter.WriteAttributeString("fo:font-size", "16pt");
		xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
		xmlTextWriter.WriteAttributeString("fo:font-weight", "bold");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "Heading1");
		xmlTextWriter.WriteAttributeString("style:family", "table-cell");
		xmlTextWriter.WriteAttributeString("style:parent-style-name", "Heading");
		xmlTextWriter.WriteStartElement("style:table-cell-properties");
		xmlTextWriter.WriteAttributeString("style:rotation-angle", "90");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("office:automatic-styles");
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
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:header-style");
		xmlTextWriter.WriteStartElement("style:header-footer-properties");
		xmlTextWriter.WriteAttributeString("fo:min-height", "0.101cm");
		xmlTextWriter.WriteAttributeString("fo:margin-left", "0cm");
		xmlTextWriter.WriteAttributeString("fo:margin-right", "0cm");
		xmlTextWriter.WriteAttributeString("fo:margin-bottom", "0cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:footer-style");
		xmlTextWriter.WriteStartElement("style:header-footer-properties");
		xmlTextWriter.WriteAttributeString("fo:min-height", "0.101cm");
		xmlTextWriter.WriteAttributeString("fo:margin-left", "0cm");
		xmlTextWriter.WriteAttributeString("fo:margin-right", "0cm");
		xmlTextWriter.WriteAttributeString("fo:margin-top", "0cm");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("office:master-styles");
		xmlTextWriter.WriteStartElement("style:master-page");
		xmlTextWriter.WriteAttributeString("style:name", "Default");
		xmlTextWriter.WriteAttributeString("style:page-layout-name", "pm1");
		xmlTextWriter.WriteStartElement("style:header");
		xmlTextWriter.WriteAttributeString("style:display", "false");
		xmlTextWriter.WriteStartElement("text:p");
		xmlTextWriter.WriteStartElement("text:sheet-name");
		xmlTextWriter.WriteString("Sheet1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:header-left");
		xmlTextWriter.WriteAttributeString("style:display", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:footer");
		xmlTextWriter.WriteAttributeString("style:display", "false");
		xmlTextWriter.WriteStartElement("text:p");
		xmlTextWriter.WriteString("Page ");
		xmlTextWriter.WriteStartElement("text:page-number");
		xmlTextWriter.WriteString("1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:footer-left");
		xmlTextWriter.WriteAttributeString("style:display", "false");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteContent(StiReport report, StiPagesCollection allPages)
	{
		//IL_10fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1103: Invalid comparison between Unknown and I4
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_120c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1225: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Invalid comparison between Unknown and I4
		//IL_123f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Invalid comparison between Unknown and I4
		//IL_1259: Unknown result type (might be due to invalid IL or missing references)
		//IL_125f: Invalid comparison between Unknown and I4
		MemoryStream memoryStream = new StiCachedStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
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
		xmlTextWriter.WriteAttributeString("xmlns:presentation", "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
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
		for (int i = 0; i < allPages.Count; i++)
		{
			StiPagesCollection stiPagesCollection = new StiPagesCollection(report, allPages);
			stiPagesCollection.AddV2Internal(allPages[i]);
			int num = i;
			string excelSheetValue = allPages[i].ExcelSheetValue;
			while (i < allPages.Count - 1 && allPages[i + 1].ExcelSheetValue == excelSheetValue)
			{
				i++;
				stiPagesCollection.AddV2Internal(allPages[i]);
			}
			stiPagesCollection.CacheMode = report.RenderedPages.CacheMode;
			string text = stiPagesCollection[0].ExcelSheetValue;
			if (string.IsNullOrEmpty(text))
			{
				text = $"Page {sheetNameList.Count + 1}";
			}
			string text2 = string.Empty;
			int num2 = 1;
			matrix = new StiMatrix(stiPagesCollection, StiOptions.Export.OpenDocumentCalc.DivideBigCells, this);
			if (base.IsStopped)
			{
				return null;
			}
			int num3 = 0;
			do
			{
				firstPageIndexList.Add(num);
				matrixList.Add(matrix);
				int num4 = matrix.CoordY.Count - 1;
				if (num4 - num3 > StiOptions.Export.OpenDocumentCalc.MaximumSheetHeight)
				{
					num4 = num3 + StiOptions.Export.OpenDocumentCalc.MaximumSheetHeight;
				}
				else
				{
					matrix = null;
				}
				minRowList.Add(num3);
				maxRowList.Add(num4);
				num3 = num4;
				if (matrix != null || text2.Length > 0)
				{
					text2 = $" part{num2++}";
				}
				sheetNameList.Add(text + text2);
				if (base.IsStopped)
				{
					return null;
				}
			}
			while (matrix != null);
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		Hashtable hashtable4 = new Hashtable();
		for (int j = 0; j < matrixList.Count; j++)
		{
			matrix = (StiMatrix)matrixList[j];
			int num5 = (int)minRowList[j];
			int num6 = (int)maxRowList[j];
			bool[,] array = new bool[matrix.CoordY.Count, matrix.CoordX.Count];
			int[,] array2 = new int[matrix.CoordY.Count, matrix.CoordX.Count];
			for (int k = 0; k < matrix.CoordX.Count - 1; k++)
			{
				double num7 = (double)matrix.CoordX.GetByIndex(k + 1) - (double)matrix.CoordX.GetByIndex(k);
				if (!hashtable3.ContainsKey(num7))
				{
					int count = hashtable3.Count;
					hashtable3[num7] = count;
					hashtable4[count] = num7;
				}
			}
			for (int l = num5 + 1; l < num6 + 1; l++)
			{
				double num8 = (double)matrix.CoordY.GetByIndex(l) - (double)matrix.CoordY.GetByIndex(l - 1);
				if (!hashtable.ContainsKey(num8))
				{
					int count2 = hashtable.Count;
					hashtable[num8] = count2;
					hashtable2[count2] = num8;
				}
				for (int m = 1; m < matrix.CoordX.Count; m++)
				{
					StiCell stiCell = matrix.Cells[l - 1, m - 1];
					if (array[l, m])
					{
						continue;
					}
					if (stiCell != null)
					{
						for (int n = 0; n <= stiCell.Height; n++)
						{
							for (int num9 = 0; num9 <= stiCell.Width; num9++)
							{
								array[l + n, m + num9] = true;
							}
						}
						array2[l - 1, m - 1] = GetCellStyleNumber(l - 1, m - 1, stiCell.Height + 1, stiCell.Width + 1);
					}
					else
					{
						array2[l - 1, m - 1] = GetCellStyleNumber(l - 1, m - 1, 1, 1);
					}
				}
			}
			cellStyleTableList.Add(array2);
		}
		xmlTextWriter.WriteStartElement("office:scripts");
		xmlTextWriter.WriteEndElement();
		Hashtable hashtable5 = new Hashtable();
		foreach (CellStyleData cellStyle in cellStyleList)
		{
			hashtable5[cellStyle.FontName] = cellStyle.FontName;
		}
		xmlTextWriter.WriteStartElement("office:font-face-decls");
		foreach (DictionaryEntry item in hashtable5)
		{
			string value = (string)item.Value;
			xmlTextWriter.WriteStartElement("style:font-face");
			xmlTextWriter.WriteAttributeString("style:name", value);
			xmlTextWriter.WriteAttributeString("svg:font-family", value);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("office:automatic-styles");
		for (int num10 = 0; num10 < hashtable3.Count; num10++)
		{
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"co{num10 + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "table-column");
			xmlTextWriter.WriteStartElement("style:table-column-properties");
			xmlTextWriter.WriteAttributeString("fo:break-before", "auto");
			double number = (double)hashtable4[num10];
			xmlTextWriter.WriteAttributeString("style:column-width", DoubleToString(number));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		for (int num11 = 0; num11 < hashtable.Count; num11++)
		{
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"ro{num11 + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "table-row");
			xmlTextWriter.WriteStartElement("style:table-row-properties");
			double number2 = (double)hashtable2[num11];
			xmlTextWriter.WriteAttributeString("style:row-height", DoubleToString(number2));
			xmlTextWriter.WriteAttributeString("fo:break-before", "auto");
			xmlTextWriter.WriteAttributeString("style:use-optimal-row-height", "false");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "ta1");
		xmlTextWriter.WriteAttributeString("style:family", "table");
		xmlTextWriter.WriteAttributeString("style:master-page-name", "Default");
		xmlTextWriter.WriteStartElement("style:table-properties");
		xmlTextWriter.WriteAttributeString("table:display", "true");
		xmlTextWriter.WriteAttributeString("style:writing-mode", "lr-tb");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "gr1");
		xmlTextWriter.WriteAttributeString("style:family", "graphic");
		xmlTextWriter.WriteStartElement("style:graphic-properties");
		xmlTextWriter.WriteAttributeString("draw:stroke", "none");
		xmlTextWriter.WriteAttributeString("draw:fill", "none");
		xmlTextWriter.WriteAttributeString("draw:textarea-horizontal-align", "center");
		xmlTextWriter.WriteAttributeString("draw:textarea-vertical-align", "middle");
		xmlTextWriter.WriteAttributeString("draw:color-mode", "standard");
		xmlTextWriter.WriteAttributeString("draw:luminance", "0%");
		xmlTextWriter.WriteAttributeString("draw:contrast", "0%");
		xmlTextWriter.WriteAttributeString("draw:gamma", "100%");
		xmlTextWriter.WriteAttributeString("draw:red", "0%");
		xmlTextWriter.WriteAttributeString("draw:green", "0%");
		xmlTextWriter.WriteAttributeString("draw:blue", "0%");
		xmlTextWriter.WriteAttributeString("fo:clip", "rect(0cm 0cm 0cm 0cm)");
		xmlTextWriter.WriteAttributeString("draw:image-opacity", "100%");
		xmlTextWriter.WriteAttributeString("style:mirror", "none");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("style:style");
		xmlTextWriter.WriteAttributeString("style:name", "P1");
		xmlTextWriter.WriteAttributeString("style:family", "paragraph");
		xmlTextWriter.WriteStartElement("style:paragraph-properties");
		xmlTextWriter.WriteAttributeString("fo:text-align", "center");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		for (int num12 = 0; num12 < dataStyleList.Count; num12++)
		{
			DataStyleData dataStyleData = (DataStyleData)dataStyleList[num12];
			if (dataStyleData.isNumeric)
			{
				xmlTextWriter.WriteStartElement("number:number-style");
				if (dataStyleData.NegativeBraces)
				{
					xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}P0");
					xmlTextWriter.WriteAttributeString("style:volatile", "true");
				}
				else
				{
					xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
				}
				if (dataStyleData.DecimalComma != ' ')
				{
					xmlTextWriter.WriteAttributeString("number:language", (dataStyleData.DecimalComma == ',') ? "ru" : "en");
					xmlTextWriter.WriteAttributeString("number:country", (dataStyleData.DecimalComma == ',') ? "RU" : "US");
				}
				xmlTextWriter.WriteStartElement("number:number");
				xmlTextWriter.WriteAttributeString("number:decimal-places", dataStyleData.DecimalDigits.ToString());
				xmlTextWriter.WriteAttributeString("number:min-integer-digits", "1");
				xmlTextWriter.WriteAttributeString("number:grouping", (dataStyleData.GroupDigits == 3) ? "true" : "false");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				if (dataStyleData.NegativeBraces)
				{
					xmlTextWriter.WriteStartElement("number:number-style");
					xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
					if (dataStyleData.DecimalComma != ' ')
					{
						xmlTextWriter.WriteAttributeString("number:language", (dataStyleData.DecimalComma == ',') ? "ru" : "en");
						xmlTextWriter.WriteAttributeString("number:country", (dataStyleData.DecimalComma == ',') ? "RU" : "US");
					}
					xmlTextWriter.WriteElementString("number:text", "(");
					xmlTextWriter.WriteStartElement("number:number");
					xmlTextWriter.WriteAttributeString("number:decimal-places", dataStyleData.DecimalDigits.ToString());
					xmlTextWriter.WriteAttributeString("number:min-integer-digits", "1");
					xmlTextWriter.WriteAttributeString("number:grouping", (dataStyleData.GroupDigits == 3) ? "true" : "false");
					xmlTextWriter.WriteEndElement();
					xmlTextWriter.WriteElementString("number:text", ")");
					xmlTextWriter.WriteStartElement("style:map");
					xmlTextWriter.WriteAttributeString("style:condition", "value()>=0");
					xmlTextWriter.WriteAttributeString("style:apply-style-name", $"N{num12 + 1}P0");
					xmlTextWriter.WriteEndElement();
					xmlTextWriter.WriteEndElement();
				}
			}
			if (dataStyleData.isPercent)
			{
				xmlTextWriter.WriteStartElement("number:percentage-style");
				xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
				if (dataStyleData.DecimalComma != ' ')
				{
					xmlTextWriter.WriteAttributeString("number:language", (dataStyleData.DecimalComma == ',') ? "ru" : "en");
					xmlTextWriter.WriteAttributeString("number:country", (dataStyleData.DecimalComma == ',') ? "RU" : "US");
				}
				xmlTextWriter.WriteStartElement("number:number");
				xmlTextWriter.WriteAttributeString("number:decimal-places", dataStyleData.DecimalDigits.ToString());
				xmlTextWriter.WriteAttributeString("number:min-integer-digits", "1");
				if (dataStyleData.GroupDigits == 3)
				{
					xmlTextWriter.WriteAttributeString("number:grouping", "true");
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteElementString("number:text", "%");
				xmlTextWriter.WriteEndElement();
			}
			if (dataStyleData.isCurrency)
			{
				xmlTextWriter.WriteStartElement("number:currency-style");
				xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
				if (dataStyleData.DecimalComma != ' ')
				{
					xmlTextWriter.WriteAttributeString("number:language", (dataStyleData.DecimalComma == ',') ? "ru" : "en");
					xmlTextWriter.WriteAttributeString("number:country", (dataStyleData.DecimalComma == ',') ? "RU" : "US");
				}
				if (dataStyleData.CurrencyPositionBefore)
				{
					xmlTextWriter.WriteElementString("number:currency-symbol", dataStyleData.CurrencySymbol);
				}
				xmlTextWriter.WriteStartElement("number:number");
				xmlTextWriter.WriteAttributeString("number:decimal-places", dataStyleData.DecimalDigits.ToString());
				xmlTextWriter.WriteAttributeString("number:min-integer-digits", "1");
				xmlTextWriter.WriteAttributeString("number:grouping", (dataStyleData.GroupDigits == 3) ? "true" : "false");
				xmlTextWriter.WriteEndElement();
				if (!dataStyleData.CurrencyPositionBefore)
				{
					xmlTextWriter.WriteElementString("number:currency-symbol", dataStyleData.CurrencySymbol);
				}
				xmlTextWriter.WriteEndElement();
			}
			if (dataStyleData.isDate)
			{
				xmlTextWriter.WriteStartElement("number:date-style");
				xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
				WriteDateTimeFormatString(xmlTextWriter, dataStyleData.DateTimeFormatString, "d");
				xmlTextWriter.WriteEndElement();
			}
			if (dataStyleData.isTime)
			{
				xmlTextWriter.WriteStartElement("number:time-style");
				xmlTextWriter.WriteAttributeString("style:name", $"N{num12 + 1}");
				if (!string.IsNullOrEmpty(dataStyleData.DateTimeFormatString) && dataStyleData.DateTimeFormatString.IndexOf("h", StringComparison.Ordinal) != -1)
				{
					xmlTextWriter.WriteAttributeString("number:language", "en");
					xmlTextWriter.WriteAttributeString("number:country", "US");
				}
				WriteDateTimeFormatString(xmlTextWriter, dataStyleData.DateTimeFormatString, "t");
				xmlTextWriter.WriteEndElement();
			}
		}
		for (int num13 = 0; num13 < cellStyleList.Count; num13++)
		{
			CellStyleData cellStyleData2 = (CellStyleData)cellStyleList[num13];
			xmlTextWriter.WriteStartElement("style:style");
			xmlTextWriter.WriteAttributeString("style:name", $"ce{num13 + 1}");
			xmlTextWriter.WriteAttributeString("style:family", "table-cell");
			xmlTextWriter.WriteAttributeString("style:parent-style-name", "Default");
			if (cellStyleData2.DataStyle != -1)
			{
				xmlTextWriter.WriteAttributeString("style:data-style-name", $"N{cellStyleData2.DataStyle + 1}");
			}
			xmlTextWriter.WriteStartElement("style:table-cell-properties");
			if (cellStyleData2.BackColor != Color.Transparent)
			{
				xmlTextWriter.WriteAttributeString("fo:background-color", GetColorString(cellStyleData2.BackColor));
			}
			else
			{
				xmlTextWriter.WriteAttributeString("fo:background-color", "transparent");
			}
			if ((int)cellStyleData2.VertAlign == 1)
			{
				xmlTextWriter.WriteAttributeString("style:vertical-align", "middle");
			}
			if ((int)cellStyleData2.VertAlign == 0)
			{
				xmlTextWriter.WriteAttributeString("style:vertical-align", "top");
			}
			xmlTextWriter.WriteAttributeString("style:text-align-source", "fix");
			xmlTextWriter.WriteAttributeString("style:repeat-content", "false");
			if (cellStyleData2.Wordwrap)
			{
				xmlTextWriter.WriteAttributeString("fo:wrap-option", "wrap");
			}
			xmlTextWriter.WriteAttributeString("fo:padding", "0in");
			xmlTextWriter.WriteAttributeString("fo:border-left", cellStyleData2.BorderLeft);
			xmlTextWriter.WriteAttributeString("fo:border-right", cellStyleData2.BorderRight);
			xmlTextWriter.WriteAttributeString("fo:border-top", cellStyleData2.BorderTop);
			xmlTextWriter.WriteAttributeString("fo:border-bottom", cellStyleData2.BorderBottom);
			if (cellStyleData2.Angle != 0)
			{
				xmlTextWriter.WriteAttributeString("style:rotation-angle", $"{cellStyleData2.Angle}");
				xmlTextWriter.WriteAttributeString("style:rotation-align", "none");
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("style:paragraph-properties");
			if ((int)cellStyleData2.HorAlign == 0)
			{
				xmlTextWriter.WriteAttributeString("fo:text-align", "start");
			}
			if ((int)cellStyleData2.HorAlign == 1)
			{
				xmlTextWriter.WriteAttributeString("fo:text-align", "center");
			}
			if ((int)cellStyleData2.HorAlign == 2)
			{
				xmlTextWriter.WriteAttributeString("fo:text-align", "end");
			}
			if ((int)cellStyleData2.HorAlign == 3)
			{
				xmlTextWriter.WriteAttributeString("fo:text-align", "justify");
			}
			xmlTextWriter.WriteAttributeString("fo:margin-left", "0cm");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("style:text-properties");
			xmlTextWriter.WriteAttributeString("fo:color", GetColorString(cellStyleData2.FontColor));
			xmlTextWriter.WriteAttributeString("style:font-name", cellStyleData2.FontName);
			string value2 = $"{cellStyleData2.FontSize}pt".Replace(",", ".");
			xmlTextWriter.WriteAttributeString("fo:font-size", value2);
			xmlTextWriter.WriteAttributeString("fo:font-size-asian", value2);
			xmlTextWriter.WriteAttributeString("fo:font-size-complex", value2);
			if (cellStyleData2.Italic)
			{
				xmlTextWriter.WriteAttributeString("fo:font-style", "italic");
				xmlTextWriter.WriteAttributeString("fo:font-style-asian", "italic");
				xmlTextWriter.WriteAttributeString("fo:font-style-complex", "italic");
			}
			if (cellStyleData2.Underline)
			{
				xmlTextWriter.WriteAttributeString("style:text-underline-style", "solid");
				xmlTextWriter.WriteAttributeString("style:text-underline-width", "auto");
				xmlTextWriter.WriteAttributeString("style:text-underline-color", "font-color");
			}
			if (cellStyleData2.Bold)
			{
				xmlTextWriter.WriteAttributeString("fo:font-weight", "bold");
				xmlTextWriter.WriteAttributeString("fo:font-weight-asian", "bold");
				xmlTextWriter.WriteAttributeString("fo:font-weight-complex", "bold");
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("office:body");
		xmlTextWriter.WriteStartElement("office:spreadsheet");
		xmlTextWriter.WriteStartElement("table:calculation-settings");
		xmlTextWriter.WriteAttributeString("table:use-regular-expressions", "false");
		xmlTextWriter.WriteEndElement();
		base.CurrentPassNumber = (StiOptions.Export.OpenDocumentCalc.DivideSegmentPages ? 3 : 2);
		for (int num14 = 0; num14 < matrixList.Count; num14++)
		{
			WriteTableFromMatrix(xmlTextWriter, num14, hashtable, hashtable3);
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private void WriteDateTimeFormatString(XmlTextWriter writer, string formatString, string defaultFormatString)
	{
		if (string.IsNullOrEmpty(formatString))
		{
			formatString = defaultFormatString;
		}
		if (formatString.Length == 1)
		{
			DateTimeFormatInfo dateTimeFormat = currentCulture.DateTimeFormat;
			switch (formatString)
			{
			case "d":
				formatString = dateTimeFormat.ShortDatePattern;
				break;
			case "D":
				formatString = dateTimeFormat.LongDatePattern;
				break;
			case "f":
				formatString = dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern;
				break;
			case "F":
				formatString = dateTimeFormat.FullDateTimePattern;
				break;
			case "g":
				formatString = dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern;
				break;
			case "G":
				formatString = dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.LongTimePattern;
				break;
			case "m":
			case "M":
				formatString = dateTimeFormat.MonthDayPattern;
				break;
			case "r":
			case "R":
				formatString = dateTimeFormat.RFC1123Pattern;
				break;
			case "s":
				formatString = dateTimeFormat.SortableDateTimePattern;
				break;
			case "t":
				formatString = dateTimeFormat.ShortTimePattern;
				break;
			case "T":
				formatString = dateTimeFormat.LongTimePattern;
				break;
			case "u":
				formatString = dateTimeFormat.UniversalSortableDateTimePattern;
				break;
			case "U":
				formatString = dateTimeFormat.FullDateTimePattern;
				break;
			case "y":
			case "Y":
				formatString = dateTimeFormat.YearMonthPattern;
				break;
			}
			formatString = formatString.Replace("\"", "").Replace("'", "");
		}
		int num = 0;
		while (num < formatString.Length)
		{
			char c = formatString[num];
			int num2 = 1;
			while (num + 1 < formatString.Length && formatString[num + 1] == c)
			{
				num++;
				num2++;
			}
			num++;
			switch (c)
			{
			case 'd':
				if (num2 == 1)
				{
					writer.WriteStartElement("number:day");
					writer.WriteEndElement();
				}
				if (num2 == 2)
				{
					writer.WriteStartElement("number:day");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				if (num2 == 3)
				{
					writer.WriteStartElement("number:day-of-week");
					writer.WriteEndElement();
				}
				if (num2 == 4)
				{
					writer.WriteStartElement("number:day-of-week");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				break;
			case 'M':
				if (num2 == 1)
				{
					writer.WriteStartElement("number:month");
					writer.WriteEndElement();
				}
				if (num2 == 2)
				{
					writer.WriteStartElement("number:month");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				if (num2 == 3)
				{
					writer.WriteStartElement("number:month");
					writer.WriteAttributeString("number:textual", "true");
					writer.WriteEndElement();
				}
				if (num2 == 4)
				{
					writer.WriteStartElement("number:month");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteAttributeString("number:textual", "true");
					writer.WriteEndElement();
				}
				break;
			case 'y':
				if (num2 == 1 || num2 == 2)
				{
					writer.WriteStartElement("number:year");
					writer.WriteEndElement();
				}
				if (num2 == 3 || num2 == 4)
				{
					writer.WriteStartElement("number:year");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				break;
			case 's':
				if (num2 == 1)
				{
					writer.WriteStartElement("number:seconds");
					writer.WriteEndElement();
				}
				if (num2 == 2)
				{
					writer.WriteStartElement("number:seconds");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				break;
			case 'm':
				if (num2 == 1)
				{
					writer.WriteStartElement("number:minutes");
					writer.WriteEndElement();
				}
				if (num2 == 2)
				{
					writer.WriteStartElement("number:minutes");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				break;
			case 'H':
			case 'h':
				if (num2 == 1)
				{
					writer.WriteStartElement("number:hours");
					writer.WriteEndElement();
				}
				if (num2 == 2)
				{
					writer.WriteStartElement("number:hours");
					writer.WriteAttributeString("number:style", "long");
					writer.WriteEndElement();
				}
				break;
			default:
				writer.WriteElementString("number:text", c.ToString());
				break;
			}
		}
	}

	private void WriteTableFromMatrix(XmlTextWriter writer, int indexSheet, Hashtable rowHeightList, Hashtable colWidthList)
	{
		StiMatrix stiMatrix = (StiMatrix)matrixList[indexSheet];
		int num = (int)minRowList[indexSheet];
		int num2 = (int)maxRowList[indexSheet];
		int[,] array = (int[,])cellStyleTableList[indexSheet];
		string text = (string)sheetNameList[indexSheet];
		bool[,] array2 = new bool[stiMatrix.CoordY.Count, stiMatrix.CoordX.Count];
		RichTextBox val = null;
		writer.WriteStartElement("table:table");
		writer.WriteAttributeString("table:name", text);
		writer.WriteAttributeString("table:style-name", "ta1");
		writer.WriteAttributeString("table:print", "false");
		for (int i = 0; i < stiMatrix.CoordX.Count - 1; i++)
		{
			double num3 = (double)stiMatrix.CoordX.GetByIndex(i + 1) - (double)stiMatrix.CoordX.GetByIndex(i);
			int num4 = (int)colWidthList[num3];
			writer.WriteStartElement("table:table-column");
			writer.WriteAttributeString("table:style-name", $"co{num4 + 1}");
			writer.WriteAttributeString("table:default-cell-style-name", "Default");
			writer.WriteEndElement();
		}
		double num5 = Math.Max((float)stiMatrix.CoordY.Count / 200f, 1f);
		int num6 = 0;
		for (int j = num + 1; j < num2 + 1; j++)
		{
			int num7 = (int)((double)j / num5);
			if (num7 > num6)
			{
				num6 = num7;
				InvokeExporting(j, stiMatrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			double num8 = (double)stiMatrix.CoordY.GetByIndex(j) - (double)stiMatrix.CoordY.GetByIndex(j - 1);
			int num9 = (int)rowHeightList[num8];
			writer.WriteStartElement("table:table-row");
			writer.WriteAttributeString("table:style-name", $"ro{num9 + 1}");
			for (int k = 1; k < stiMatrix.CoordX.Count; k++)
			{
				StiCell stiCell = stiMatrix.Cells[j - 1, k - 1];
				if (!array2[j, k])
				{
					if (stiCell != null)
					{
						for (int l = 0; l <= stiCell.Height; l++)
						{
							for (int m = 0; m <= stiCell.Width; m++)
							{
								array2[j + l, k + m] = true;
							}
						}
						int num10 = array[j - 1, k - 1];
						int dataStyle = ((CellStyleData)cellStyleList[num10]).DataStyle;
						writer.WriteStartElement("table:table-cell");
						writer.WriteAttributeString("table:style-name", $"ce{num10 + 1}");
						if (stiCell.Width > 0 || stiCell.Height > 0)
						{
							writer.WriteAttributeString("table:number-columns-spanned", $"{stiCell.Width + 1}");
							writer.WriteAttributeString("table:number-rows-spanned", $"{stiCell.Height + 1}");
						}
						StiRichText stiRichText = stiCell.Component as StiRichText;
						StiText stiText = stiCell.Component as StiText;
						string text2 = stiCell.Text;
						if (stiRichText != null && stiRichText.RtfText != string.Empty)
						{
							if (val == null)
							{
								val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
							}
							stiRichText.GetPreparedText(val);
							text2 = ((Control)val).Text;
						}
						if (stiCell.Component is StiCheckBox stiCheckBox && !string.IsNullOrEmpty(stiCheckBox.ExcelDataValue))
						{
							text2 = stiCheckBox.ExcelDataValue;
						}
						if (!stiCell.Component.IsExportAsImage(StiExportFormat.Ods) && (!string.IsNullOrEmpty(text2) || (stiText != null && stiText.ExcelDataValue != null)))
						{
							bool flag = false;
							double result = 0.0;
							DateTime result2 = default(DateTime);
							DataStyleData dataStyleData = null;
							if (dataStyle != -1 && stiText != null && stiText.ExcelDataValue != null && stiText.ExcelDataValue != "-")
							{
								string excelDataValue = stiText.ExcelDataValue;
								string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
								string s = excelDataValue.Replace(".", ",").Replace(",", numberDecimalSeparator);
								dataStyleData = (DataStyleData)dataStyleList[dataStyle];
								flag = true;
								try
								{
									if (dataStyleData.isDate || dataStyleData.isTime)
									{
										if (!DateTime.TryParse(excelDataValue, currentCulture, DateTimeStyles.None, out result2))
										{
											flag = false;
										}
									}
									else if (!double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
									{
										flag = false;
									}
								}
								catch (FormatException)
								{
									flag = false;
								}
								if (!flag && text2 == null)
								{
									text2 = excelDataValue;
								}
							}
							if (flag)
							{
								if (dataStyleData.isDate)
								{
									writer.WriteAttributeString("office:value-type", "date");
									writer.WriteAttributeString("office:date-value", result2.ToString("yyyy-MM-dd"));
								}
								else if (dataStyleData.isTime)
								{
									writer.WriteAttributeString("office:value-type", "time");
									writer.WriteAttributeString("office:time-value", result2.ToString("PTHH\\Hmm\\Mss\\S"));
								}
								else
								{
									string value = result.ToString().Replace(',', '.');
									if (dataStyleData.isNumeric)
									{
										writer.WriteAttributeString("office:value-type", "float");
									}
									if (dataStyleData.isPercent)
									{
										writer.WriteAttributeString("office:value-type", "percentage");
									}
									if (dataStyleData.isCurrency)
									{
										writer.WriteAttributeString("office:value-type", "currency");
									}
									writer.WriteAttributeString("office:value", value);
								}
							}
							else
							{
								writer.WriteAttributeString("office:value-type", "string");
							}
							List<string> list = StiExportUtils.SplitString(text2, removeControl: true);
							for (int n = 0; n < list.Count; n++)
							{
								string text3 = list[n];
								writer.WriteStartElement("text:p");
								writer.WriteString(text3);
								writer.WriteEndElement();
							}
						}
						if (stiCell.Component.IsExportAsImage(StiExportFormat.Ods))
						{
							writer.WriteAttributeString("office:value-type", "string");
							if (stiCell.Component is IStiExportImage stiExportImage)
							{
								float zoom = imageResolution;
								Image image = stiExportImage.GetImage(ref zoom);
								try
								{
									if (image != null)
									{
										Image image2 = stiMatrix.GetRealImageData(stiCell, image) ?? image;
										int num11 = imageCache.AddImageInt(image2);
										double number = (double)stiMatrix.CoordX.GetByIndex(k + stiCell.Width) - (double)stiMatrix.CoordX.GetByIndex(k - 1);
										double number2 = (double)stiMatrix.CoordY.GetByIndex(j + stiCell.Height) - (double)stiMatrix.CoordY.GetByIndex(j - 1);
										string value2 = $"'{text}'.{GetColumnName(stiCell.Left + stiCell.Width + 1)}{stiCell.Top + stiCell.Height + 1 + 1}";
										writer.WriteStartElement("draw:frame");
										writer.WriteAttributeString("table:end-cell-address", value2);
										writer.WriteAttributeString("table:end-x", "0in");
										writer.WriteAttributeString("table:end-y", "0in");
										writer.WriteAttributeString("draw:z-index", "0");
										writer.WriteAttributeString("draw:name", $"Picture{num11 + 1}");
										writer.WriteAttributeString("draw:style-name", "gr1");
										writer.WriteAttributeString("draw:text-style-name", "P1");
										writer.WriteAttributeString("text:anchor-type", "paragraph");
										writer.WriteAttributeString("svg:x", DoubleToString(0.0));
										writer.WriteAttributeString("svg:y", DoubleToString(0.0));
										writer.WriteAttributeString("svg:width", DoubleToString(number));
										writer.WriteAttributeString("svg:height", DoubleToString(number2));
										writer.WriteStartElement("draw:image");
										writer.WriteAttributeString("xlink:href", $"Pictures/{num11 + 1:D5}.jpg");
										writer.WriteAttributeString("xlink:type", "simple");
										writer.WriteAttributeString("xlink:show", "embed");
										writer.WriteAttributeString("xlink:actuate", "onLoad");
										writer.WriteStartElement("text:p");
										writer.WriteEndElement();
										writer.WriteEndElement();
										writer.WriteEndElement();
									}
								}
								finally
								{
									((IDisposable)image)?.Dispose();
								}
							}
						}
						writer.WriteEndElement();
					}
					else
					{
						int num12 = array[j - 1, k - 1];
						writer.WriteStartElement("table:table-cell");
						writer.WriteAttributeString("table:style-name", $"ce{num12 + 1}");
						writer.WriteEndElement();
					}
				}
				else
				{
					writer.WriteStartElement("table:covered-table-cell");
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		((Component)(object)val)?.Dispose();
	}

	public void ExportOds(StiReport report, string fileName)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(fileName);
			fileStream = new FileStream(fileName, FileMode.Create);
			ExportOds(report, fileStream);
		}
		finally
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public void ExportOds(StiReport report, Stream stream)
	{
		ExportOds(report, stream, new StiOdsExportSettings());
	}

	public void ExportOds(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		ExportOds(report, stream, new StiOdsExportSettings
		{
			PageRange = pageRange
		});
	}

	public void ExportOds(StiReport report, Stream stream, StiOdsExportSettings settings)
	{
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Ods format");
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		StiPagesRange pageRange = settings.PageRange;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		xmlIndentation = 1;
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
		currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			imageCache = new StiImageCache(StiOptions.Export.OpenDocumentCalc.AllowImageComparer, ImageFormat.Jpeg, imageQuality);
			cellStyleList = new ArrayList();
			dataStyleList = new ArrayList();
			sheetNameList = new ArrayList();
			matrixList = new ArrayList();
			firstPageIndexList = new ArrayList();
			minRowList = new ArrayList();
			maxRowList = new ArrayList();
			cellStyleTableList = new ArrayList();
			base.CurrentPassNumber = 0;
			base.MaximumPassNumber = (StiOptions.Export.OpenDocumentCalc.DivideSegmentPages ? 4 : 3);
			StiPagesCollection stiPagesCollection = pageRange.GetSelectedPages(report.RenderedPages);
			if (StiOptions.Export.OpenDocumentCalc.DivideSegmentPages)
			{
				stiPagesCollection = StiSegmentPagesDivider.Divide(stiPagesCollection, this);
				base.CurrentPassNumber = 1;
			}
			if (base.IsStopped)
			{
				return;
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			StiZipWriter20 val = new StiZipWriter20();
			val.Begin(stream, true);
			val.AddFile("content.xml", WriteContent(report, stiPagesCollection));
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument") + " 2";
			val.AddFile("mimetype", WriteMimetype());
			val.AddFile("meta.xml", WriteMeta());
			val.AddFile("META-INF/manifest.xml", WriteManifest());
			val.AddFile("settings.xml", WriteSettings());
			val.AddFile("styles.xml", WriteStyles(stiPagesCollection));
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
			sheetNameList.Clear();
			sheetNameList = null;
			firstPageIndexList.Clear();
			firstPageIndexList = null;
			minRowList.Clear();
			minRowList = null;
			maxRowList.Clear();
			maxRowList = null;
			foreach (StiMatrix matrix in matrixList)
			{
				matrix?.Clear();
			}
			matrixList.Clear();
			matrixList = null;
			cellStyleTableList.Clear();
			cellStyleTableList = null;
			dataStyleList.Clear();
			dataStyleList = null;
			cellStyleList.Clear();
			cellStyleList = null;
			imageCache.Clear();
			imageCache = null;
			if (report.RenderedPages.CacheMode)
			{
				StiMatrix.GCCollect();
			}
		}
	}
}
