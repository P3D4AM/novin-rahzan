using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Export;

public class StiMatrixCacheManager
{
	private int matrixWidth;

	private int matrixHeight;

	private int segmentHeight;

	private StiMatrix matrix;

	private int amountOfProcessedPagesForStartGCCollect;

	private string cachePath;

	internal bool flagForceSaveSegment;

	internal StiMatrixCacheSegment[] segments;

	private List<int> quickAccessSegments;

	private int amountOfQuickAccessSegments = 3;

	public StiMatrixCacheSegment[] Segments
	{
		get
		{
			if (segments == null)
			{
				int num = matrixHeight / segmentHeight + ((matrixHeight % segmentHeight != 0) ? 1 : 0);
				segments = new StiMatrixCacheSegment[num];
			}
			return segments;
		}
	}

	public List<int> QuickAccessSegments
	{
		get
		{
			if (quickAccessSegments == null)
			{
				quickAccessSegments = new List<int>();
			}
			return quickAccessSegments;
		}
	}

	public int AmountOfQuickAccessSegments
	{
		get
		{
			return amountOfQuickAccessSegments;
		}
		set
		{
			amountOfQuickAccessSegments = value;
		}
	}

	public StiMatrixCacheSegment GetMatrixSegment(int index)
	{
		StiMatrixCacheSegment[] array = Segments;
		if (array[index] == null)
		{
			array[index] = new StiMatrixCacheSegment(segmentHeight);
		}
		if (!matrix.useCacheMode)
		{
			return array[index];
		}
		if (QuickAccessSegments.Contains(index))
		{
			return array[index];
		}
		quickAccessSegments.Add(index);
		if (array[index].IsSaved)
		{
			LoadSegment(array[index]);
			if (flagForceSaveSegment)
			{
				array[index].IsSaved = false;
			}
		}
		if (quickAccessSegments.Count > AmountOfQuickAccessSegments)
		{
			int num = quickAccessSegments[0];
			quickAccessSegments.RemoveAt(0);
			StiMatrixCacheSegment stiMatrixCacheSegment = array[num];
			if (!stiMatrixCacheSegment.IsSaved)
			{
				SaveSegment(stiMatrixCacheSegment);
				stiMatrixCacheSegment.IsSaved = true;
			}
			stiMatrixCacheSegment.Clear();
			if (StiOptions.Engine.ReportCache.AllowGCCollect)
			{
				amountOfProcessedPagesForStartGCCollect++;
				if (amountOfProcessedPagesForStartGCCollect >= StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect)
				{
					amountOfProcessedPagesForStartGCCollect = 0;
					StiMatrix.GCCollect();
				}
			}
		}
		return array[index];
	}

	public StiMatrixLineData GetMatrixLineData(int lineNumber)
	{
		int index = lineNumber / segmentHeight;
		int num = lineNumber % segmentHeight;
		StiMatrixCacheSegment matrixSegment = GetMatrixSegment(index);
		StiMatrixLineData[] lines = matrixSegment.Lines;
		if (lines[num] == null)
		{
			lines[num] = new StiMatrixLineData(matrixWidth);
		}
		return lines[num];
	}

	public void WriteSegment(Stream stream, StiMatrixCacheSegment segment)
	{
		int num = segment.Lines.Length;
		while (num > 0 && segment.Lines[num - 1] == null)
		{
			num--;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiMatrixSerializer");
			xmlTextWriter.WriteAttributeString("version", "1.02");
			xmlTextWriter.WriteAttributeString("type", "Net");
			xmlTextWriter.WriteAttributeString("application", "StiMatrixCache");
			xmlTextWriter.WriteStartElement("Lines");
			xmlTextWriter.WriteAttributeString("count", num.ToString());
			for (int i = 0; i < num; i++)
			{
				StiMatrixLineData line = segment.Lines[i];
				WriteMatrixLine(xmlTextWriter, line);
			}
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.Flush();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private void WriteMatrixLine(XmlTextWriter tw, StiMatrixLineData line)
	{
		tw.WriteStartElement("line");
		if (line != null)
		{
			StiCell[] cells = line.Cells;
			foreach (StiCell cell in cells)
			{
				WriteMatrixCell(tw, cell);
			}
			if (line.Bookmarks != null)
			{
				for (int j = 0; j < line.Bookmarks.Length; j++)
				{
					string text = line.Bookmarks[j];
					if (text != null)
					{
						int k;
						for (k = 0; j + k + 1 < line.Bookmarks.Length && line.Bookmarks[j + k + 1] == text; k++)
						{
						}
						tw.WriteStartElement("bkm");
						tw.WriteAttributeString("x", j.ToString());
						if (k > 0)
						{
							tw.WriteAttributeString("w", k.ToString());
						}
						tw.WriteAttributeString("t", text);
						tw.WriteEndElement();
						j += k;
					}
				}
			}
			if (line.BordersX != null)
			{
				int l;
				for (l = 0; l < line.BordersX.Length && line.BordersX[l] == 0; l++)
				{
				}
				int num = line.BordersX.Length - 1;
				while (num > l && line.BordersX[num] == 0)
				{
					num--;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (l < num)
				{
					for (int m = l; m <= num; m++)
					{
						if (line.BordersX[m] != 0)
						{
							stringBuilder.Append(line.BordersX[m]);
						}
						if (m != num)
						{
							stringBuilder.Append(",");
						}
					}
					tw.WriteStartElement("bordersX");
					tw.WriteAttributeString("x", l.ToString());
					tw.WriteAttributeString("v", stringBuilder.ToString());
					tw.WriteEndElement();
				}
			}
			if (line.BordersY != null)
			{
				int n;
				for (n = 0; n < line.BordersY.Length && line.BordersY[n] == 0; n++)
				{
				}
				int num2 = line.BordersY.Length - 1;
				while (num2 > n && line.BordersY[num2] == 0)
				{
					num2--;
				}
				if (n < num2)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					for (int num3 = n; num3 <= num2; num3++)
					{
						if (line.BordersY[num3] != 0)
						{
							stringBuilder2.Append(line.BordersY[num3]);
						}
						if (num3 != num2)
						{
							stringBuilder2.Append(",");
						}
					}
					tw.WriteStartElement("bordersY");
					tw.WriteAttributeString("x", n.ToString());
					tw.WriteAttributeString("v", stringBuilder2.ToString());
					tw.WriteEndElement();
				}
			}
			if (line.CellStyles != null)
			{
				int num4;
				for (num4 = 0; num4 < line.CellStyles.Length && line.CellStyles[num4] == 0; num4++)
				{
				}
				int num5 = line.CellStyles.Length - 1;
				while (num5 > num4 && line.CellStyles[num5] == 0)
				{
					num5--;
				}
				if (num4 < num5)
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					for (int num6 = num4; num6 <= num5; num6++)
					{
						if (line.CellStyles[num6] != 0)
						{
							stringBuilder3.Append(line.CellStyles[num6]);
						}
						if (num6 != num5)
						{
							stringBuilder3.Append(",");
						}
					}
					tw.WriteStartElement("cellstyles");
					tw.WriteAttributeString("x", num4.ToString());
					tw.WriteAttributeString("v", stringBuilder3.ToString());
					tw.WriteEndElement();
				}
			}
		}
		tw.WriteEndElement();
	}

	private void WriteMatrixCell(XmlTextWriter tw, StiCell cell)
	{
		if (cell == null)
		{
			return;
		}
		tw.WriteStartElement("cell");
		tw.WriteAttributeString("x", cell.Left.ToString());
		if (cell.Width > 0)
		{
			tw.WriteAttributeString("w", cell.Width.ToString());
		}
		if (cell.Height > 0)
		{
			tw.WriteAttributeString("h", cell.Height.ToString());
		}
		if (cell is StiCell2 stiCell)
		{
			tw.WriteAttributeString("pid", stiCell.PageId.ToString());
			tw.WriteAttributeString("cid", stiCell.ComponentId.ToString());
			if (stiCell.cellStyleId != -1)
			{
				tw.WriteAttributeString("sid", stiCell.cellStyleId.ToString());
			}
		}
		if (!string.IsNullOrEmpty(cell.Text))
		{
			tw.WriteAttributeString("t", cell.Text);
		}
		tw.WriteEndElement();
	}

	public void ReadSegment(Stream stream, StiMatrixCacheSegment segment)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(stream);
			if (xmlDocument.DocumentElement.Name == "StiMatrixSerializer" && xmlDocument.DocumentElement.ChildNodes.Count > 0 && xmlDocument.DocumentElement.ChildNodes[0].Name == "Lines")
			{
				XmlNode xmlNode = xmlDocument.DocumentElement.ChildNodes[0];
				for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlNode.ChildNodes[i];
					StiMatrixLineData stiMatrixLineData = segment.Lines[i];
					if (stiMatrixLineData == null)
					{
						segment.Lines[i] = new StiMatrixLineData(matrixWidth);
						stiMatrixLineData = segment.Lines[i];
					}
					foreach (XmlNode childNode in xmlNode2.ChildNodes)
					{
						switch (childNode.Name)
						{
						case "cell":
							ReadMatrixCell(childNode, stiMatrixLineData);
							break;
						case "bordersX":
							ReadMatrixBorders(childNode, stiMatrixLineData.BordersX);
							break;
						case "bordersY":
							ReadMatrixBorders(childNode, stiMatrixLineData.BordersY);
							break;
						case "bkm":
							ReadMatrixBookmark(childNode, stiMatrixLineData.Bookmarks);
							break;
						case "cellstyles":
							ReadMatrixCellStyles(childNode, stiMatrixLineData.CellStyles);
							break;
						}
					}
				}
			}
			xmlDocument = null;
			int num = Array.IndexOf(segments, segment);
			if (num == -1)
			{
				return;
			}
			for (int j = 0; j < segment.Lines.Length; j++)
			{
				StiMatrixLineData stiMatrixLineData2 = segment.Lines[j];
				if (stiMatrixLineData2 == null)
				{
					continue;
				}
				StiCell[] cells = stiMatrixLineData2.Cells;
				foreach (StiCell stiCell in cells)
				{
					if (stiCell != null)
					{
						stiCell.Top = num * segmentHeight + j;
					}
				}
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private void ReadMatrixCell(XmlNode node, StiMatrixLineData line)
	{
		StiCell2 stiCell = new StiCell2();
		stiCell.Matrix = matrix;
		XmlElement xmlElement = node as XmlElement;
		if (xmlElement.HasAttribute("x"))
		{
			stiCell.Left = Convert.ToInt32(node.Attributes["x"].Value);
		}
		if (xmlElement.HasAttribute("w"))
		{
			stiCell.Width = Convert.ToInt32(node.Attributes["w"].Value);
		}
		if (xmlElement.HasAttribute("h"))
		{
			stiCell.Height = Convert.ToInt32(node.Attributes["h"].Value);
		}
		if (xmlElement.HasAttribute("pid"))
		{
			stiCell.PageId = Convert.ToInt32(node.Attributes["pid"].Value);
		}
		if (xmlElement.HasAttribute("cid"))
		{
			stiCell.ComponentId = Convert.ToInt32(node.Attributes["cid"].Value);
		}
		if (xmlElement.HasAttribute("sid"))
		{
			stiCell.cellStyleId = Convert.ToInt32(node.Attributes["sid"].Value);
		}
		if (xmlElement.HasAttribute("t"))
		{
			stiCell.Text = Convert.ToString(node.Attributes["t"].Value);
		}
		line.Cells[stiCell.Left] = stiCell;
	}

	private void ReadMatrixBorders(XmlNode node, int[] borders)
	{
		XmlElement xmlElement = node as XmlElement;
		int num = 0;
		if (xmlElement.HasAttribute("x"))
		{
			num = Convert.ToInt32(node.Attributes["x"].Value);
		}
		string text = Convert.ToString(node.Attributes["v"].Value);
		string[] array = text.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length > 0)
			{
				borders[num + i] = Convert.ToInt32(array[i]);
			}
		}
	}

	private void ReadMatrixBookmark(XmlNode node, string[] bookmarks)
	{
		XmlElement xmlElement = node as XmlElement;
		int num = 0;
		if (xmlElement.HasAttribute("x"))
		{
			num = Convert.ToInt32(node.Attributes["x"].Value);
		}
		int num2 = 0;
		if (xmlElement.HasAttribute("w"))
		{
			num2 = Convert.ToInt32(node.Attributes["w"].Value);
		}
		string text = Convert.ToString(node.Attributes["t"].Value);
		for (int i = 0; i < num2; i++)
		{
			bookmarks[num + i] = text;
		}
	}

	private void ReadMatrixCellStyles(XmlNode node, int[] cellStyles)
	{
		XmlElement xmlElement = node as XmlElement;
		int num = 0;
		if (xmlElement.HasAttribute("x"))
		{
			num = Convert.ToInt32(node.Attributes["x"].Value);
		}
		string text = Convert.ToString(node.Attributes["v"].Value);
		string[] array = text.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length > 0)
			{
				cellStyles[num + i] = Convert.ToInt32(array[i]);
			}
		}
	}

	public void SaveSegment(StiMatrixCacheSegment segment)
	{
		string cacheSegmentName = StiMatrixCache.GetCacheSegmentName(cachePath, segment.CacheGuid);
		StiFileUtils.ProcessReadOnly(cacheSegmentName);
		using Stream stream = new FileStream(cacheSegmentName, FileMode.Create, FileAccess.Write);
		WriteSegment(stream, segment);
		stream.Flush();
		stream.Close();
	}

	public void LoadSegment(StiMatrixCacheSegment segment)
	{
		string cacheSegmentName = StiMatrixCache.GetCacheSegmentName(cachePath, segment.CacheGuid);
		if (File.Exists(cacheSegmentName))
		{
			using (Stream stream = new FileStream(cacheSegmentName, FileMode.Open, FileAccess.Read))
			{
				ReadSegment(stream, segment);
				stream.Flush();
				stream.Close();
			}
		}
	}

	public void Clear()
	{
		if (matrix != null && matrix.useCacheMode)
		{
			StiMatrixCache.DeleteCache(cachePath);
		}
		matrix = null;
		segments = null;
		if (quickAccessSegments != null)
		{
			quickAccessSegments.Clear();
		}
	}

	public StiMatrixCacheManager(StiMatrix matrix, int width, int height, int maxPageHeight)
	{
		this.matrix = matrix;
		matrixWidth = width;
		matrixHeight = height;
		segmentHeight = StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect * 3;
		if (maxPageHeight * 3 > segmentHeight)
		{
			segmentHeight = maxPageHeight * 3;
		}
		if (matrix.useCacheMode)
		{
			cachePath = StiMatrixCache.CreateNewCache();
		}
	}
}
