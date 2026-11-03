using System;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiCell : ICloneable
{
	public StiCellType CellType = StiCellType.Cell;

	public SizeD Size { get; set; }

	public bool IsChangeWidthForRightToLeft { get; set; }

	public bool IsNumeric { get; set; }

	public bool IsNegativeColor { get; set; }

	public bool IsImage { get; set; }

	public bool IsCrossSummary { get; set; }

	public StiCrossField Field { get; set; }

	public string Text { get; set; }

	public object HyperlinkValue { get; set; }

	public object ToolTipValue { get; set; }

	public object TagValue { get; set; }

	public StiCell ParentCell { get; set; }

	public object Value { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public int SummaryIndex { get; set; } = -1;


	public int Level { get; set; } = -1;


	public Dictionary<string, object> DrillDownParameters { get; set; }

	public string ParentGuid { get; set; }

	public string Guid { get; set; }

	public bool KeepMergedCellsTogether { get; set; }

	public bool FieldCloned { get; set; }

	public object Clone()
	{
		StiCell stiCell = MemberwiseClone() as StiCell;
		stiCell.ParentCell = stiCell;
		return stiCell;
	}

	public string GetComponentPlacement()
	{
		string text = ((Field is StiCrossColumn) ? (StiOptions.Export.OptimizeDataOnlyMode ? "d" : "h") : ((Field is StiCrossHeader) ? (StiOptions.Export.OptimizeDataOnlyMode ? "d" : "h") : ((!(Field is StiCrossTitle)) ? "d" : "h")));
		if (text.Length <= 0 || Field == null || Field.Parent == null)
		{
			return text;
		}
		return text + "." + Field.Parent.Name;
	}

	public override string ToString()
	{
		return Text + " type:" + CellType;
	}

	public StiCell()
		: this(string.Empty, 0m, null)
	{
	}

	public StiCell(string text, decimal value, StiCrossField field)
		: this(text, value, 1, 1, field)
	{
		IsNumeric = true;
	}

	public StiCell(string text, decimal value, int width, int height, StiCrossField field)
	{
		Text = text;
		Value = value;
		Width = width;
		Height = height;
		Field = field;
	}
}
