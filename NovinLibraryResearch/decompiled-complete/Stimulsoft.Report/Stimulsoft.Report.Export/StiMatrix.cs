using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report.Export;

public class StiMatrix
{
	public enum StiTableLineInfo
	{
		Empty,
		Unknown,
		PageHeader,
		PageFooter,
		HeaderAP,
		FooterAP,
		HeaderD,
		FooterD,
		Data,
		Trash
	}

	internal class RoundedRectangleInfo
	{
		public Rectangle Rect;

		public StiRoundedRectanglePrimitive Primitive;
	}

	public class DataField
	{
		public string Name;

		public int[] Info;

		public string FormatString;

		public string[] DataArray;

		public bool readyName;

		public bool readyType;

		public DataField(int size)
		{
			Name = string.Empty;
			Info = new int[3];
			FormatString = string.Empty;
			DataArray = new string[size];
			readyName = false;
			readyType = false;
		}
	}

	internal StiCell[,] cells2;

	public StiMatrixCellsCollection Cells;

	internal StiBorderSide[,] bordersX2;

	internal StiBorderSide[,] bordersY2;

	public StiMatrixBorderSidesXCollection BordersX;

	public StiMatrixBorderSidesYCollection BordersY;

	internal StiCellStyle[,] cellStyles2;

	public StiMatrixCellStylesCollection CellStyles;

	internal string[,] bookmarks2;

	public StiMatrixBookmarksCollection Bookmarks;

	internal StiExportFormat exportFormat = StiExportFormat.Excel;

	private bool isHtmlService;

	private bool isHtmlOrExcelXmlService;

	private bool isHtmlPngMode;

	internal StiReport report;

	private StiPagesCollection pages;

	private bool addComponentWithInteractions;

	private bool replaceCheckboxes;

	private Hashtable hyperlinksToTag;

	internal Hashtable pointerToBookmark;

	private Hashtable pointerToTag;

	internal Dictionary<int, List<RoundedRectangleInfo>> RoundedRectanglesList;

	private List<KeyValuePair<StiComponent, double>> tempRoundedRectangleInfoList;

	private double maxCoordY;

	private double defaultLinePrimitiveWidth;

	internal bool useCacheMode;

	private StiMatrixCacheManager cacheManager;

	internal StiText additionalInfo;

	private bool[] coordXCheck;

	private bool[] coordYCheck;

	private int[] coordXNew;

	private int[] coordYNew;

	private int[] coordXPrim;

	private int[] coordYPrim;

	internal Hashtable imagesBaseRect;

	private Hashtable leftCached = new Hashtable();

	private Hashtable topCached = new Hashtable();

	private Hashtable xcHash = new Hashtable();

	private Hashtable ycHash = new Hashtable();

	private Hashtable tagSplitCache = new Hashtable();

	private Hashtable stylesCache = new Hashtable();

	private Hashtable fontsCache = new Hashtable();

	private ArrayList createdCells = new ArrayList();

	private List<StiBorderSide> borderSides;

	private const float maxRowHeight = 200f;

	private const int _defaultLinePrimitiveWidth = 1;

	internal const double htmlScaleX = 0.96;

	internal const double htmlScaleY = 0.956;

	private static readonly StiRectanglePrimitive staticRectanglePrimitive = new StiRectanglePrimitive();

	public DataField[] Fields;

	public int DataArrayLength;

	private ArrayList FieldsList;

	private int sizeX;

	private int sizeY;

	private Hashtable htName;

	private StiPage lastPage;

	private int lastPageId = -1;

	private StiComponentsCollection lastComps;

	private StiCell[,] CellsMap { get; set; }

	public double TotalWidth { get; set; }

	private double TotalHeight { get; set; }

	public ArrayList Styles { get; private set; } = new ArrayList();


	public SortedList CoordX { get; private set; } = new SortedList();


	public SortedList CoordY { get; private set; } = new SortedList();


	public StiTableLineInfo[] LinePlacement { get; private set; }

	public string[] ParentBandName { get; private set; }

	public List<int> HorizontalPageBreaks { get; private set; } = new List<int>();


	public int[,,] Interactions { get; }

	internal List<StiBorderSide> BorderSides => borderSides ?? (borderSides = new List<StiBorderSide>());

	private double Round(double value)
	{
		return Math.Round(value, MidpointRounding.AwayFromZero);
	}

	private void AddCoord(RectangleD rect)
	{
		AddCoord(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top + TotalHeight);
		AddCoord(((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom + TotalHeight);
	}

	private void AddCoord(double x, double y, bool convert = true)
	{
		if (y > maxCoordY)
		{
			maxCoordY = y;
		}
		if (convert && isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization)
		{
			x = Round(x * 0.96);
			y = Round(y * 0.956);
		}
		else
		{
			x = Round(x);
			y = Round(y);
		}
		if (!xcHash.ContainsKey(x))
		{
			CoordX[x] = x;
			xcHash[x] = null;
		}
		if (!ycHash.ContainsKey(y))
		{
			CoordY[y] = y;
			ycHash[y] = null;
		}
	}

	private int GetCoordXRounded(double x)
	{
		if (isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization)
		{
			return (int)Round(x * 0.96);
		}
		return (int)Round(x);
	}

	private int GetCoordYRounded(double y)
	{
		if (isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization)
		{
			return (int)Round(y * 0.956);
		}
		return (int)Round(y);
	}

	public void PrepareTable()
	{
		for (int i = 1; i < CoordY.Count; i++)
		{
			double value = (double)CoordY.GetByIndex(i) - (double)CoordY.GetByIndex(i - 1);
			value = Round(value);
			double num = 200.0;
			if (!(value > num))
			{
				continue;
			}
			if (value / num > 100.0)
			{
				num = (int)(value / 100.0);
				for (int j = 1; j < 100; j++)
				{
					double y = (double)CoordY.GetByIndex(i - 1) + num * (double)j;
					AddCoord(0.0, y, convert: false);
				}
				i += 99;
			}
			else
			{
				if (value < num * 2.0)
				{
					num = Round(value / 2.0);
				}
				double y2 = (double)CoordY.GetByIndex(i - 1) + num;
				AddCoord(0.0, y2, convert: false);
			}
		}
		for (int k = 1; k < CoordX.Count; k++)
		{
			double value2 = (double)CoordX.GetByIndex(k) - (double)CoordX.GetByIndex(k - 1);
			value2 = Round(value2);
			double num2 = 1900.0;
			if (value2 > num2)
			{
				if (value2 < num2 * 2.0)
				{
					num2 = Round(value2 / 2.0);
				}
				double x = (double)CoordX.GetByIndex(k - 1) + num2;
				AddCoord(x, 0.0, convert: false);
			}
		}
	}

	public Rectangle GetRange(RectangleD rect)
	{
		double num = ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.96 : 1.0);
		double num2 = ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.956 : 1.0);
		double num3 = Round(((RectangleD)(ref rect)).Left * num);
		double num4 = Round((((RectangleD)(ref rect)).Top + TotalHeight) * num2);
		double num5 = Round(((RectangleD)(ref rect)).Right * num);
		double num6 = Round((((RectangleD)(ref rect)).Bottom + TotalHeight) * num2);
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		object obj = leftCached[num3];
		if (obj == null)
		{
			num7 = CoordX.IndexOfValue(num3);
			leftCached[num3] = num7;
		}
		else
		{
			num7 = (int)obj;
		}
		obj = topCached[num4];
		if (obj == null)
		{
			num8 = CoordY.IndexOfValue(num4);
			topCached[num4] = num8;
		}
		else
		{
			num8 = (int)obj;
		}
		obj = leftCached[num5];
		if (obj == null)
		{
			num9 = CoordX.IndexOfValue(num5);
			leftCached[num5] = num9;
		}
		else
		{
			num9 = (int)obj;
		}
		obj = topCached[num6];
		if (obj == null)
		{
			num10 = CoordY.IndexOfValue(num6);
			topCached[num6] = num10;
		}
		else
		{
			num10 = (int)obj;
		}
		return new Rectangle(num7, num8, num9 - num7, num10 - num8);
	}

	public StiCellStyle GetStyleFromComponent(StiComponent component, int x, int y)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.White;
		if (component is IStiBrush stiBrush && !(component is StiShape))
		{
			color = StiBrush.ToColor(stiBrush.Brush);
		}
		if (component is StiShape)
		{
			if (component.Page != null)
			{
				color = StiBrush.ToColor(component.Page.Brush);
			}
			if (color.Equals((object?)Color.Transparent) && !isHtmlPngMode)
			{
				color = Color.White;
			}
		}
		if (component is StiRichText stiRichText)
		{
			color = stiRichText.BackColor;
		}
		if (component is StiPrimitive || component is StiBarCode)
		{
			color = Color.Transparent;
		}
		if (x != -1 && y != -1 && color.A == 0 && CellsMap[y, x] != null)
		{
			StiCell stiCell = CellsMap[y, x];
			color = stiCell.CellStyle.Color;
		}
		Color textColor = Color.Black;
		if (component is IStiTextBrush stiTextBrush)
		{
			textColor = StiBrush.ToColor(stiTextBrush.TextBrush);
		}
		Font val = null;
		val = (Font)((!(component is IStiFont stiFont)) ? ((object)new Font("Arial", 8f)) : ((object)stiFont.Font));
		StiTextHorAlignment horAlignment = (StiTextHorAlignment)0;
		if (component is IStiTextHorAlignment stiTextHorAlignment)
		{
			horAlignment = stiTextHorAlignment.HorAlignment;
		}
		StiVertAlignment vertAlignment = (StiVertAlignment)0;
		if (component is IStiVertAlignment stiVertAlignment)
		{
			vertAlignment = stiVertAlignment.VertAlignment;
		}
		StiBorderSide val2 = null;
		StiBorderSide borderR = null;
		StiBorderSide border = null;
		StiBorderSide borderB = null;
		if (component is IStiBorder stiBorder)
		{
			StiBorder border2 = stiBorder.Border;
			StiAdvancedBorder val3 = (StiAdvancedBorder)(object)((border2 is StiAdvancedBorder) ? border2 : null);
			if (val3 != (StiAdvancedBorder)null)
			{
				val2 = val3.LeftSide;
				borderR = val3.RightSide;
				border = val3.TopSide;
				borderB = val3.BottomSide;
			}
			else
			{
				val2 = new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style);
				if (stiBorder.Border.IsRightBorderSidePresent)
				{
					borderR = val2;
				}
				if (stiBorder.Border.IsTopBorderSidePresent)
				{
					border = val2;
				}
				if (stiBorder.Border.IsBottomBorderSidePresent)
				{
					borderB = val2;
				}
				if (!stiBorder.Border.IsLeftBorderSidePresent)
				{
					val2 = null;
				}
			}
		}
		StiTextOptions val4 = null;
		if (component is IStiTextOptions stiTextOptions)
		{
			val4 = stiTextOptions.TextOptions;
		}
		bool wordWrap = false;
		if (component is IStiText stiText && stiText.GetTextInternal() != null)
		{
			wordWrap = stiText.GetTextInternal().IndexOf("\n", StringComparison.InvariantCulture) != -1;
		}
		if (isHtmlOrExcelXmlService && val4 != null)
		{
			wordWrap = val4.WordWrap;
		}
		string format = null;
		StiText stiText2 = component as StiText;
		if (exportFormat == StiExportFormat.ExcelXml && stiText2 != null)
		{
			string text = stiText2.Format;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			string text2 = string.Empty;
			int num = 2;
			int num2 = 0;
			char c = '$';
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = stiText2?.HideZeros ?? false;
			string text3 = null;
			string text4 = null;
			int num3 = text?.IndexOf("|") ?? (-1);
			if (num3 != -1)
			{
				text3 = StiExportUtils.GetPositivePattern(text[num3 + 1] - 65);
				text4 = StiExportUtils.GetNegativePattern(text[num3 + 2] - 65);
				text = text.Substring(0, num3);
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (text[0] == 'C')
				{
					flag = true;
				}
				if (text[0] == 'N')
				{
					flag2 = true;
				}
				if (text[0] == 'P')
				{
					flag3 = true;
				}
				if ((flag || flag2 || flag3) && text.Length > 1)
				{
					int i = 1;
					if (char.IsDigit(text[i]))
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (; i < text.Length && char.IsDigit(text[i]); i++)
						{
							stringBuilder.Append(text[i]);
						}
						num = int.Parse(stringBuilder.ToString());
					}
					if (i < text.Length && text[i] == 'G')
					{
						i++;
						num2 = 3;
					}
					if (i < text.Length && text[i] == '(')
					{
						i++;
						flag5 = true;
					}
					if (i < text.Length && (text[i] == '.' || text[i] == ','))
					{
						i++;
					}
					if (i < text.Length && (text[i] == '+' || text[i] == '-'))
					{
						if (text[i] == '+')
						{
							flag4 = true;
						}
						i++;
						if (i < text.Length)
						{
							c = text[i];
						}
					}
				}
			}
			if (flag || flag2 || flag3)
			{
				if (num3 != -1)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					if (num2 > 1)
					{
						stringBuilder2.Append("#,");
						stringBuilder2.Append('#', num2 - 1);
					}
					stringBuilder2.Append('0');
					if (num > 0)
					{
						stringBuilder2.Append(".");
						stringBuilder2.Append('0', num);
					}
					string newValue = stringBuilder2.ToString();
					string text5 = text3.Replace("n", newValue).Replace("$", $"\"{c}\"");
					string text6 = text4.Replace("n", newValue).Replace("$", $"\"{c}\"");
					text2 = text5 + ";" + text6 + (flag6 ? ";" : "");
				}
				else
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					if (flag && flag4)
					{
						stringBuilder3.Append("\"");
						stringBuilder3.Append(c);
						stringBuilder3.Append("\"");
					}
					if (num2 > 1)
					{
						stringBuilder3.Append("#,");
						stringBuilder3.Append('#', num2 - 1);
					}
					stringBuilder3.Append('0');
					if (num > 0)
					{
						stringBuilder3.Append(".");
						stringBuilder3.Append('0', num);
					}
					if (flag && !flag4)
					{
						stringBuilder3.Append("\"");
						stringBuilder3.Append(c);
						stringBuilder3.Append("\"");
					}
					if (flag3)
					{
						stringBuilder3.Append("%");
					}
					text2 = stringBuilder3.ToString();
					string text7 = (flag5 ? "(" : "-") + text2 + (flag5 ? ")" : "");
					if (flag6)
					{
						text2 = text2 + ";" + text7 + ";";
					}
					else if (flag5)
					{
						text2 = text2 + ";" + text7;
					}
				}
			}
			format = text2;
		}
		string internalStyleName = null;
		if (!string.IsNullOrEmpty(component.ComponentStyle))
		{
			internalStyleName = component.ComponentStyle;
		}
		if (isHtmlOrExcelXmlService && !StiOptions.Export.Html.UseComponentStyleName)
		{
			internalStyleName = null;
		}
		double lineSpacing = stiText2?.LineSpacing ?? 1f;
		bool allowHtmlTags = isHtmlService && stiText2 != null && stiText2.CheckAllowHtmlTags();
		return StiCellStyle.GetStyleFromCache(color, textColor, val, horAlignment, vertAlignment, border, val2, borderR, borderB, val4, wordWrap, format, internalStyleName, lineSpacing, allowHtmlTags, stylesCache, Styles, fontsCache, null, simplyAdd: false);
	}

	private StiCellStyle GetStyle(StiCellStyle style)
	{
		foreach (StiCellStyle style2 in Styles)
		{
			if (style2.Equals(style))
			{
				return style2;
			}
		}
		Styles.Add(style);
		return style;
	}

	private void RenderComponent(StiComponent component, int indexComponent, bool exportData, bool isPage)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Invalid comparison between Unknown and I4
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Expected O, but got Unknown
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Invalid comparison between Unknown and I4
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Invalid comparison between Unknown and I4
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Invalid comparison between Unknown and I4
		//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Invalid comparison between Unknown and I4
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5e: Invalid comparison between Unknown and I4
		StiComponent component2 = component;
		ReplaceComponentForExport(ref component, replaceCheckboxes);
		RectangleD rect = component.Page.Unit.ConvertToHInches(component.DisplayRectangle);
		if (((RectangleD)(ref rect)).Height < 0.0)
		{
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height;
			((RectangleD)(ref rect)).Height = Math.Abs(((RectangleD)(ref rect)).Height);
		}
		if (!(component is StiLinePrimitive) && (((RectangleD)(ref rect)).Width == 0.0 || ((RectangleD)(ref rect)).Height == 0.0))
		{
			return;
		}
		if (component is StiLinePrimitive)
		{
			if (component is StiRectanglePrimitive)
			{
				((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - defaultLinePrimitiveWidth;
				((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - defaultLinePrimitiveWidth;
			}
			else
			{
				if (((RectangleD)(ref rect)).Height < 1.5)
				{
					((RectangleD)(ref rect)).Height = defaultLinePrimitiveWidth;
					((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - ((RectangleD)(ref rect)).Height;
				}
				if (((RectangleD)(ref rect)).Width < 1.5)
				{
					((RectangleD)(ref rect)).Width = defaultLinePrimitiveWidth;
					((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - ((RectangleD)(ref rect)).Width;
				}
			}
		}
		else
		{
			if (((RectangleD)(ref rect)).Height < 1.5 && ((RectangleD)(ref rect)).Height > 0.0 && GetCoordYRounded(((RectangleD)(ref rect)).Top) == GetCoordYRounded(((RectangleD)(ref rect)).Bottom))
			{
				((RectangleD)(ref rect)).Height = 1.0 / (double)GetCoordYRounded(1.0) + 0.01;
			}
			if (((RectangleD)(ref rect)).Width < 1.5 && ((RectangleD)(ref rect)).Width > 0.0 && GetCoordXRounded(((RectangleD)(ref rect)).Left) == GetCoordXRounded(((RectangleD)(ref rect)).Right))
			{
				((RectangleD)(ref rect)).Width = 1.0 / (double)GetCoordXRounded(1.0) + 0.01;
			}
		}
		Rectangle range = GetRange(rect);
		if (range.Left == -1)
		{
			return;
		}
		bool flag = true;
		string text = component.BookmarkValue as string;
		string text2 = component.TagValue as string;
		if (text == null)
		{
			if (!string.IsNullOrEmpty(text2) && hyperlinksToTag.ContainsKey(text2))
			{
				text = text2;
			}
			else if (!string.IsNullOrWhiteSpace((string)component.PointerValue) && !string.IsNullOrEmpty(component.Guid))
			{
				text = $"{component.PointerValue}#GUID#{component.Guid}";
			}
		}
		StiBorder val = ((component is IStiBorder stiBorder) ? stiBorder.Border : null);
		bool flag2 = !string.IsNullOrEmpty(text);
		bool flag3 = val != null && (int)val.Side > 0;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		StiText stiText = component as StiText;
		if (stiText != null)
		{
			flag4 = stiText.Text != null && stiText.GetTextInternal() != null && (stiText.GetTextInternal().Length > 0 || exportData);
			flag5 = stiText.Brush != null && StiBrush.ToColor(stiText.Brush).A != 0;
			flag6 = stiText.ExcelDataValue != null;
			flag7 = stiText.Indicator != null;
			flag = flag4 || flag5 || flag6 || flag3 || flag7 || stiText.Editable;
		}
		if (component is StiContainer stiContainer)
		{
			flag = (stiContainer.Brush != null && StiBrush.ToColor(stiContainer.Brush).A != 0) || (flag3 && !isPage);
		}
		if (flag2)
		{
			flag = true;
		}
		int left = range.Left;
		int right = range.Right;
		int top = range.Top;
		int bottom = range.Bottom;
		if (component is StiRectanglePrimitive)
		{
			flag = false;
			if (coordXPrim[left] == 0)
			{
				coordXPrim[left] = 1;
			}
			if (coordXPrim[right] == 0)
			{
				coordXPrim[right] = 1;
			}
			if (coordYPrim[top] == 0)
			{
				coordYPrim[top] = 1;
			}
			if (coordYPrim[bottom] == 0)
			{
				coordYPrim[bottom] = 1;
			}
		}
		bool flag8 = IsComponentHasInteraction(component);
		int num = 0;
		int num2 = 0;
		if (flag8)
		{
			flag = true;
			num = component.Report.RenderedPages.IndexOf(component.Page) + 1;
			num2 = component.Page.Components.IndexOf(component) + 1;
		}
		if (isHtmlService && StiOptions.Export.Html.AllowRoundedRectangles && component is StiRoundedRectanglePrimitive)
		{
			tempRoundedRectangleInfoList.Add(new KeyValuePair<StiComponent, double>(component, TotalHeight));
			flag3 = false;
		}
		if (flag)
		{
			StiCell stiCell = (useCacheMode ? new StiCell2(this) : new StiCell(exportFormat));
			createdCells.Add(stiCell);
			if (exportFormat != StiExportFormat.Csv && exportFormat != StiExportFormat.Dbf && exportFormat != StiExportFormat.Xml && exportFormat != StiExportFormat.Json)
			{
				stiCell.CellStyle = GetStyleFromComponent(component, left, top);
			}
			bool flag9 = true;
			if (exportFormat == StiExportFormat.ExcelXml)
			{
				flag9 = false;
				if (component is StiRichText)
				{
					flag9 = true;
				}
				if (component is StiCheckBox)
				{
					flag9 = true;
				}
				if (stiText != null && stiText.ExcelDataValue != null)
				{
					flag9 = true;
				}
				if (StiOptions.Export.Excel.AllowFreezePanes && (component.Locked || (component.TagValue != null && component.TagValue.ToString().Contains("excelfreezepanes"))))
				{
					flag9 = true;
				}
			}
			if (flag9)
			{
				if (useCacheMode)
				{
					SetCellComponent(stiCell, component2, indexComponent);
				}
				else
				{
					stiCell.Component = component2;
				}
			}
			if (flag9 && stiCell.ExportImage != null)
			{
				if (useCacheMode)
				{
					string key = $"{(stiCell as StiCell2).PageId}*{(stiCell as StiCell2).ComponentId}";
					imagesBaseRect[key] = (object)new RectangleD(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y + TotalHeight, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
				}
				else
				{
					imagesBaseRect[component] = (object)new RectangleD(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y + TotalHeight, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
				}
			}
			Cells[top, left] = stiCell;
			stiCell.Left = left;
			stiCell.Top = top;
			if (component is IStiText stiText2)
			{
				stiCell.Text = stiText2.Text;
				if (stiText != null && (stiText.TextQuality == StiTextQuality.Wysiwyg || (int)stiText.HorAlignment == 3) && !string.IsNullOrEmpty(stiCell.Text) && stiCell.Text.EndsWith(StiTextRenderer.StiForceWidthAlignTag))
				{
					stiCell.Text = stiCell.Text.Substring(0, stiCell.Text.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
				}
				if (stiText != null && stiText.TextOptions != null && stiText.TextOptions.LineLimit)
				{
					RectangleD rect2 = stiText.ConvertTextMargins(rect, convert: false);
					rect2 = stiText.ConvertTextBorders(rect2, convert: false);
					Bitmap val2 = new Bitmap(1, 1);
					Graphics val3 = Graphics.FromImage((Image)(object)val2);
					string text3 = stiCell.Text;
					string text4 = StiTextDrawing.CutLineLimit(ref text3, val3, StiFontUtils.ChangeFontSize(stiText.Font, stiText.Font.Size * (float)StiDpiHelper.GraphicsScale), rect2, stiText.TextOptions, stiText.TextQuality == StiTextQuality.Typographic, StiDpiHelper.IsWindows);
					stiCell.Text = text4;
				}
			}
			else
			{
				stiCell.Text = string.Empty;
			}
			stiCell.Width = range.Width - 1;
			stiCell.Height = range.Height - 1;
			for (int i = range.X; i < right; i++)
			{
				for (int j = range.Y; j < bottom; j++)
				{
					CellsMap[j, i] = stiCell;
					if (flag2)
					{
						Bookmarks[j, i] = text;
					}
					if (flag8)
					{
						Interactions[j, i, 0] = num;
						Interactions[j, i, 1] = num2;
					}
				}
			}
			if (component is StiVerticalLinePrimitive)
			{
				if (coordXPrim[stiCell.Left] == 0)
				{
					coordXPrim[stiCell.Left] = 1;
				}
			}
			else
			{
				coordXPrim[stiCell.Left] = -1;
			}
			if (component is StiHorizontalLinePrimitive)
			{
				if (coordYPrim[stiCell.Top] == 0)
				{
					coordYPrim[stiCell.Top] = 1;
				}
			}
			else
			{
				coordYPrim[stiCell.Top] = -1;
			}
		}
		if (!flag3)
		{
			return;
		}
		double size = val.Size;
		StiBorderSide val4 = new StiBorderSide(val.Color, size, val.Style);
		StiAdvancedBorder val5 = (StiAdvancedBorder)(object)((val is StiAdvancedBorder) ? val : null);
		StiRectanglePrimitive stiRectanglePrimitive = staticRectanglePrimitive;
		if (component is StiVerticalLinePrimitive)
		{
			range.X++;
		}
		if (component is StiHorizontalLinePrimitive)
		{
			range.Y++;
		}
		if (component is StiRectanglePrimitive)
		{
			range.X++;
			range.Y++;
			stiRectanglePrimitive = component as StiRectanglePrimitive;
		}
		left = range.Left;
		right = range.Right;
		top = range.Top;
		bottom = range.Bottom;
		if (val5 != (StiAdvancedBorder)null)
		{
			val4 = val5.TopSide;
		}
		if (val.IsTopBorderSidePresent && (int)val4.Style != 6 && stiRectanglePrimitive.TopSide)
		{
			for (int k = left; k < right; k++)
			{
				if (BordersX[top, k] != (StiBorderSide)null)
				{
					if (BordersX[top, k].Size <= size)
					{
						BordersX[top, k] = val4;
					}
				}
				else
				{
					BordersX[top, k] = val4;
				}
			}
		}
		if (val5 != (StiAdvancedBorder)null)
		{
			val4 = val5.BottomSide;
		}
		if (val.IsBottomBorderSidePresent && (int)val4.Style != 6 && stiRectanglePrimitive.BottomSide)
		{
			for (int l = left; l < right; l++)
			{
				if (BordersX[bottom, l] != (StiBorderSide)null)
				{
					if (BordersX[bottom, l].Size <= size)
					{
						BordersX[bottom, l] = val4;
					}
				}
				else
				{
					BordersX[bottom, l] = val4;
				}
			}
		}
		if (val5 != (StiAdvancedBorder)null)
		{
			val4 = val5.LeftSide;
		}
		if (val.IsLeftBorderSidePresent && (int)val4.Style != 6 && stiRectanglePrimitive.LeftSide)
		{
			for (int m = top; m < bottom; m++)
			{
				if (BordersY[m, left] != (StiBorderSide)null)
				{
					if (BordersY[m, left].Size <= size)
					{
						BordersY[m, left] = val4;
					}
				}
				else
				{
					BordersY[m, left] = val4;
				}
			}
		}
		if (val5 != (StiAdvancedBorder)null)
		{
			val4 = val5.RightSide;
		}
		if (!val.IsRightBorderSidePresent || (int)val4.Style == 6 || !stiRectanglePrimitive.RightSide)
		{
			return;
		}
		for (int n = top; n < bottom; n++)
		{
			if (BordersY[n, right] != (StiBorderSide)null)
			{
				if (BordersY[n, right].Size <= size)
				{
					BordersY[n, right] = val4;
				}
			}
			else
			{
				BordersY[n, right] = val4;
			}
		}
	}

	private Rectangle GetCellRectangle(int startX, int startY, StiCell cell)
	{
		int i = startX;
		int num = startX;
		int num2 = startY;
		for (; i <= cell.Width && CellsMap[startY + cell.Top, i + cell.Left] != cell; i++)
		{
		}
		if (i > cell.Width)
		{
			return Rectangle.Empty;
		}
		for (num = i; num <= cell.Width && CellsMap[startY + cell.Top, num + cell.Left] == cell; num++)
		{
		}
		if (i == num)
		{
			return Rectangle.Empty;
		}
		bool flag = false;
		num2 = startY + 1;
		while (num2 <= cell.Height && !flag)
		{
			if (i > 0 && CellsMap[cell.Top + num2, cell.Left + i - 1] == cell)
			{
				flag = true;
				break;
			}
			if (num <= cell.Width && CellsMap[num2 + cell.Top, cell.Left + num] == cell)
			{
				flag = true;
				break;
			}
			for (int j = i; j < num; j++)
			{
				if (CellsMap[num2 + cell.Top, j + cell.Left] != cell)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num2++;
			}
		}
		return new Rectangle(i, startY, num - i, num2 - startY);
	}

	private void CutRectangleFromCellsMap(Rectangle cellRect, StiCell cell)
	{
		for (int i = cellRect.Left; i < cellRect.Right; i++)
		{
			for (int j = cellRect.Top; j < cellRect.Bottom; j++)
			{
				CellsMap[cell.Top + j, cell.Left + i] = null;
			}
		}
	}

	internal bool IsComponentHasInteraction(StiComponent comp)
	{
		if (addComponentWithInteractions && comp.Interaction != null)
		{
			if (comp.Interaction.SortingEnabled && !string.IsNullOrWhiteSpace(comp.Interaction.SortingColumn))
			{
				return true;
			}
			if (comp.Interaction.DrillDownEnabled && (!string.IsNullOrEmpty(comp.Interaction.DrillDownPageGuid) || !string.IsNullOrEmpty(comp.Interaction.DrillDownReport)))
			{
				return true;
			}
			if (comp.Interaction is StiBandInteraction && ((StiBandInteraction)comp.Interaction).CollapsingEnabled)
			{
				return true;
			}
		}
		return false;
	}

	internal void ScanComponentsPlacement(bool optimize)
	{
		ScanComponentsPlacement(optimize, exportObjectFormatting: false);
	}

	internal void ScanComponentsPlacement(bool optimize, bool exportObjectFormatting)
	{
		LinePlacement = new StiTableLineInfo[CoordY.Count];
		ParentBandName = new string[CoordY.Count];
		int num = -1;
		for (int i = 0; i < CoordY.Count - 1; i++)
		{
			StiTableLineInfo stiTableLineInfo = LinePlacement[i];
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int num2 = 0;
			for (int j = 1; j < CoordX.Count; j++)
			{
				StiCell stiCell = Cells[i, j - 1];
				if (stiCell == null || stiCell.Component is StiCrossColumnTotal)
				{
					continue;
				}
				string text = stiCell.Component.ComponentPlacement;
				if (text == null)
				{
					text = string.Empty;
				}
				int num3 = text.IndexOf("Hd_HPnl");
				if (num3 != -1)
				{
					text = text.Substring(0, num3 + 7);
				}
				if (!flag)
				{
					if (text.StartsWith("d", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.Data;
					}
					else if (text.StartsWith("h.ap", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.HeaderAP;
					}
					else if (text.StartsWith("f.ap", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.FooterAP;
					}
					else if (text.StartsWith("h", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.HeaderD;
					}
					else if (text.StartsWith("f", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.FooterD;
					}
					else if (text.StartsWith("gh", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.HeaderD;
					}
					else if (text.StartsWith("gf", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.FooterD;
					}
					else if (text.StartsWith("ph", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.PageHeader;
						flag = true;
					}
					else if (text.StartsWith("pf", StringComparison.InvariantCulture))
					{
						stiTableLineInfo = StiTableLineInfo.PageFooter;
						flag = true;
					}
				}
				if (text != string.Empty && stiTableLineInfo == StiTableLineInfo.Empty)
				{
					stiTableLineInfo = StiTableLineInfo.Unknown;
				}
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(text + "\u001f");
				}
				if (stringBuilder.Length == 1 && stringBuilder[0] == '\u001f' && text != string.Empty)
				{
					stringBuilder.Insert(0, text);
				}
				if ((stiTableLineInfo == StiTableLineInfo.HeaderD || stiTableLineInfo == StiTableLineInfo.HeaderAP) && !string.IsNullOrWhiteSpace(stiCell.Text))
				{
					stringBuilder.Append(stiCell.Text);
				}
				int num4 = stiCell.Height;
				if (!exportObjectFormatting)
				{
					num4 = 0;
				}
				for (int k = 0; k <= num4; k++)
				{
					if (i + k > num)
					{
						LinePlacement[i + k] = stiTableLineInfo;
						ParentBandName[i + k] = stringBuilder.ToString();
					}
				}
				if (num4 > num2)
				{
					num2 = num4;
				}
			}
			LinePlacement[i] = stiTableLineInfo;
			if (i + num2 >= num)
			{
				num = i + num2;
			}
		}
		if (!optimize)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		string text2 = null;
		for (int l = 0; l < CoordY.Count - 1; l++)
		{
			if (LinePlacement[l] != StiTableLineInfo.HeaderAP)
			{
				continue;
			}
			string text3 = ParentBandName[l];
			int num5 = text3.IndexOf('\u001f');
			if (num5 != -1)
			{
				string text4 = text3.Substring(0, num5);
				if (text4 != text2)
				{
					text2 = text4;
					hashtable.Clear();
				}
			}
			if (hashtable.ContainsKey(text3))
			{
				LinePlacement[l] = StiTableLineInfo.Trash;
				string parentBandName = GetParentBandName(l);
				while (l + 1 < CoordY.Count - 1 && LinePlacement[l + 1] == StiTableLineInfo.HeaderAP && GetParentBandName(l + 1) == parentBandName)
				{
					l++;
					LinePlacement[l] = StiTableLineInfo.Trash;
				}
			}
			else
			{
				hashtable.Add(text3, text3);
				for (string parentBandName2 = GetParentBandName(l); l + 1 < CoordY.Count - 1 && LinePlacement[l + 1] == StiTableLineInfo.HeaderAP && GetParentBandName(l + 1) == parentBandName2; l++)
				{
				}
			}
		}
		Hashtable hashtable2 = new Hashtable();
		text2 = null;
		for (int num6 = CoordY.Count - 1 - 1; num6 >= 0; num6--)
		{
			if (LinePlacement[num6] == StiTableLineInfo.FooterAP)
			{
				string text5 = ParentBandName[num6];
				int num7 = text5.IndexOf('\u001f');
				if (num7 != -1)
				{
					string text6 = text5.Substring(0, num7);
					if (text6 != text2)
					{
						text2 = text6;
						hashtable2.Clear();
					}
				}
				if (hashtable2.ContainsKey(text5))
				{
					LinePlacement[num6] = StiTableLineInfo.Trash;
					string parentBandName3 = GetParentBandName(num6);
					while (num6 - 1 >= 0 && LinePlacement[num6 - 1] == StiTableLineInfo.FooterAP && GetParentBandName(num6 - 1) == parentBandName3)
					{
						num6--;
						LinePlacement[num6] = StiTableLineInfo.Trash;
					}
				}
				else
				{
					hashtable2.Add(text5, text5);
					string parentBandName4 = GetParentBandName(num6);
					while (num6 - 1 >= 0 && LinePlacement[num6 - 1] == StiTableLineInfo.FooterAP && GetParentBandName(num6 - 1) == parentBandName4)
					{
						num6--;
					}
				}
			}
		}
		for (int m = 0; m < CoordY.Count - 1; m++)
		{
			if (LinePlacement[m] == StiTableLineInfo.PageHeader || LinePlacement[m] == StiTableLineInfo.PageFooter || LinePlacement[m] == StiTableLineInfo.Trash)
			{
				int num8 = 0;
				while (m + num8 > 0 && LinePlacement[m + num8 - 1] == StiTableLineInfo.Empty)
				{
					num8--;
					LinePlacement[m + num8] = LinePlacement[m];
				}
				num8 = 0;
				while (m + num8 < CoordY.Count - 1 && LinePlacement[m + num8 + 1] == StiTableLineInfo.Empty)
				{
					num8++;
					LinePlacement[m + num8] = LinePlacement[m];
				}
			}
		}
	}

	private string GetParentBandName(int rowIndex)
	{
		string text = ParentBandName[rowIndex];
		int num = text.IndexOf('\u001f');
		if (num == -1)
		{
			return text;
		}
		return text.Substring(0, num);
	}

	private void ProcessIntersectedCells(ArrayList createdCells2)
	{
		if (createdCells2 == null)
		{
			return;
		}
		IList valueList = CoordX.GetValueList();
		IList valueList2 = CoordY.GetValueList();
		foreach (StiCell item in createdCells2)
		{
			if (item.Width == 0 && item.Height == 0)
			{
				continue;
			}
			int startX = 0;
			int startY = 0;
			Rectangle cellRectangle = GetCellRectangle(startX, startY, item);
			if (cellRectangle.Width == item.Width + 1 && cellRectangle.Height == item.Height + 1)
			{
				CutRectangleFromCellsMap(cellRectangle, item);
				continue;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i <= item.Height; i++)
			{
				if (coordYPrim[item.Top + i] == 1)
				{
					continue;
				}
				int num = 0;
				while (num <= item.Width)
				{
					if (coordXPrim[item.Left + num] == 1)
					{
						num++;
						continue;
					}
					cellRectangle = GetCellRectangle(num, i, item);
					if (cellRectangle.Width == 0)
					{
						num = item.Width + 1;
						continue;
					}
					CutRectangleFromCellsMap(cellRectangle, item);
					StiCell stiCell2 = item.Clone() as StiCell;
					stiCell2.Left = item.Left + cellRectangle.X;
					stiCell2.Top = item.Top + cellRectangle.Y;
					stiCell2.Width = cellRectangle.Width - 1;
					stiCell2.Height = cellRectangle.Height - 1;
					Cells[stiCell2.Top, stiCell2.Left] = stiCell2;
					arrayList.Add(stiCell2);
					num += cellRectangle.Width;
				}
			}
			if (arrayList.Count <= 0)
			{
				continue;
			}
			double num2 = 0.0;
			StiCell stiCell3 = null;
			foreach (StiCell item2 in arrayList)
			{
				double num3 = (double)valueList[item2.Left + item2.Width + 1] - (double)valueList[item2.Left];
				double num4 = (double)valueList2[item2.Top + item2.Height + 1] - (double)valueList2[item2.Top];
				if (num2 < num3 * num4)
				{
					num2 = num3 * num4;
					stiCell3 = item2;
				}
			}
			foreach (StiCell item3 in arrayList)
			{
				if (item3 != stiCell3)
				{
					item3.Text = string.Empty;
					item3.ExportImage = null;
				}
			}
		}
		createdCells2.Clear();
	}

	private void GetMaxRectFromCell(StiCell cell, int y, int x, bool[,] readyCells)
	{
		int num = cell.Left + cell.Width + 1;
		if (y == cell.Top)
		{
			bool flag = BordersX[y, x] != (StiBorderSide)null;
			for (int i = x + 1; i < cell.Left + cell.Width + 1; i++)
			{
				if (BordersX[y, i] != (StiBorderSide)null != flag)
				{
					num = i;
					break;
				}
			}
		}
		int num2 = cell.Top + cell.Height + 1;
		if (x == cell.Left)
		{
			bool flag2 = BordersY[y, x] != (StiBorderSide)null;
			for (int j = y + 1; j < cell.Top + cell.Height + 1; j++)
			{
				if (BordersY[j, x] != (StiBorderSide)null != flag2)
				{
					num2 = j;
					break;
				}
			}
		}
		if (num2 == cell.Top + cell.Height + 1)
		{
			bool flag3 = BordersX[num2, x] != (StiBorderSide)null;
			for (int k = x + 1; k < num; k++)
			{
				if (BordersX[num2, k] != (StiBorderSide)null != flag3)
				{
					num = k;
					break;
				}
			}
		}
		if (num == cell.Left + cell.Width + 1)
		{
			bool flag4 = BordersY[y, num] != (StiBorderSide)null;
			for (int l = y + 1; l < num2; l++)
			{
				if (BordersY[l, num] != (StiBorderSide)null != flag4)
				{
					num2 = l;
					break;
				}
			}
		}
		for (int m = y; m <= num2 - 1; m++)
		{
			for (int n = x; n <= num - 1; n++)
			{
				readyCells[m, n] = true;
			}
		}
		if (cell.Left != x || cell.Top != y || cell.Width != num - x - 1 || cell.Height != num2 - y - 1)
		{
			StiCell stiCell = cell.Clone() as StiCell;
			stiCell.Left = x;
			stiCell.Top = y;
			stiCell.Width = num - x - 1;
			stiCell.Height = num2 - y - 1;
			Cells[y, x] = stiCell;
		}
	}

	public string[] SplitTagWithCache(string inputString)
	{
		object obj = tagSplitCache[inputString];
		if (obj != null)
		{
			return (string[])obj;
		}
		string[] array = SplitTag(inputString);
		tagSplitCache[inputString] = array;
		return array;
	}

	public static string[] SplitTag(string inputString)
	{
		string text = inputString;
		if (!text.EndsWith(";", StringComparison.InvariantCulture))
		{
			text += ";";
		}
		ArrayList arrayList = new ArrayList();
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		bool flag = false;
		while (num < text.Length)
		{
			char c = text[num];
			num++;
			if (c == '"')
			{
				if (flag)
				{
					if (text[num] == '"')
					{
						stringBuilder.Append(c);
						num++;
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					flag = !flag;
				}
			}
			if ((c == ';' && !flag) || num >= text.Length)
			{
				string text2 = stringBuilder.ToString().Trim();
				if (text2.Length > 0)
				{
					arrayList.Add(text2);
				}
				stringBuilder = new StringBuilder();
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		string[] array = new string[arrayList.Count];
		arrayList.CopyTo(0, array, 0, arrayList.Count);
		return array;
	}

	public static string[] GetStringsFromTag(string tag, int startPosition)
	{
		ArrayList arrayList = new ArrayList();
		int i;
		for (i = startPosition; i < tag.Length && tag[i] != '"'; i++)
		{
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		while (i < tag.Length)
		{
			char c = tag[i];
			i++;
			if (c == '"')
			{
				if (!flag)
				{
					flag = true;
				}
				else if (i < tag.Length && tag[i] == '"')
				{
					stringBuilder.Append(c);
					i++;
				}
				else
				{
					arrayList.Add(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
					flag = false;
				}
			}
			else if (flag)
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length > 0)
		{
			arrayList.Add(stringBuilder.ToString());
		}
		string[] array = new string[arrayList.Count];
		arrayList.CopyTo(0, array, 0, arrayList.Count);
		return array;
	}

	private void CopyFieldsListToFields()
	{
		Fields = new DataField[FieldsList.Count];
		for (int i = 0; i < FieldsList.Count; i++)
		{
			Fields[i] = (DataField)FieldsList[i];
		}
	}

	public void PrepareDocument(StiExportService service, StiDataExportMode mode)
	{
		RichTextBox val = null;
		sizeX = CoordX.Count - 1;
		sizeY = CoordY.Count - 1;
		int num = 0;
		for (int i = 0; i < sizeY; i++)
		{
			if (((mode & StiDataExportMode.Data) > (StiDataExportMode)0 && LinePlacement[i] == StiTableLineInfo.Data) || ((mode & StiDataExportMode.Headers) > (StiDataExportMode)0 && (LinePlacement[i] == StiTableLineInfo.HeaderD || LinePlacement[i] == StiTableLineInfo.HeaderAP)) || ((mode & StiDataExportMode.Footers) > (StiDataExportMode)0 && (LinePlacement[i] == StiTableLineInfo.FooterD || LinePlacement[i] == StiTableLineInfo.FooterAP)) || mode == StiDataExportMode.AllBands)
			{
				num++;
			}
		}
		FieldsList = new ArrayList();
		for (int j = 0; j < sizeX; j++)
		{
			FieldsList.Add(new DataField(num));
		}
		CopyFieldsListToFields();
		for (int k = 0; k < sizeX; k++)
		{
			Fields[k].Name = "FIELD" + k.ToString("D4");
		}
		string value = string.Empty;
		bool flag = false;
		switch (service.ExportFormat)
		{
		case StiExportFormat.Csv:
			value = "csv";
			flag = StiOptions.Export.Csv.UseAliases;
			break;
		case StiExportFormat.Dbf:
			value = "dbf";
			flag = StiOptions.Export.Dbf.UseAliases;
			break;
		case StiExportFormat.Xml:
			value = "xml";
			flag = StiOptions.Export.Xml.UseAliases;
			break;
		case StiExportFormat.Json:
			value = "json";
			break;
		}
		int num2 = 0;
		htName = new Hashtable();
		for (int l = 0; l < sizeY; l++)
		{
			if (((mode & StiDataExportMode.Data) <= (StiDataExportMode)0 || LinePlacement[l] != StiTableLineInfo.Data) && ((mode & StiDataExportMode.Headers) <= (StiDataExportMode)0 || (LinePlacement[l] != StiTableLineInfo.HeaderD && LinePlacement[l] != StiTableLineInfo.HeaderAP)) && ((mode & StiDataExportMode.Footers) <= (StiDataExportMode)0 || (LinePlacement[l] != StiTableLineInfo.FooterD && LinePlacement[l] != StiTableLineInfo.FooterAP)) && mode != StiDataExportMode.AllBands)
			{
				continue;
			}
			for (int m = 0; m < sizeX; m++)
			{
				StiCell stiCell = Cells[l, m];
				if (stiCell != null)
				{
					string text = stiCell.Component.TagValue as string;
					string[] array = null;
					if (!string.IsNullOrEmpty(text))
					{
						array = SplitTagWithCache(text);
					}
					if (array != null)
					{
						for (int n = 0; n < array.Length; n++)
						{
							if (!array[n].ToLower().StartsWith("column", StringComparison.InvariantCulture))
							{
								continue;
							}
							string[] stringsFromTag = GetStringsFromTag(array[n], 6);
							if (stringsFromTag.Length > 1)
							{
								if (!htName.ContainsKey(stringsFromTag[0]))
								{
									int num3 = Fields.Length;
									FieldsList.Add(new DataField(num));
									CopyFieldsListToFields();
									Fields[num3].Name = stringsFromTag[0];
									Fields[num3].readyName = true;
									Fields[num3].readyType = true;
									htName[stringsFromTag[0]] = num3;
								}
								int num4 = (int)htName[stringsFromTag[0]];
								Fields[num4].DataArray[num2] = stringsFromTag[1];
							}
						}
					}
					if (stiCell.Component != null && (stiCell.Component is StiSimpleText || stiCell.Component is StiCheckBox))
					{
						if (!Fields[m].readyName)
						{
							string text2 = stiCell.Component.Name;
							if (flag && !string.IsNullOrEmpty(stiCell.Component.Alias))
							{
								text2 = stiCell.Component.Alias;
							}
							if (array != null)
							{
								for (int num5 = 0; num5 < array.Length; num5++)
								{
									string text3 = array[num5].Trim();
									if (text3.ToLower().StartsWith(value, StringComparison.InvariantCulture) || text3.ToLower().StartsWith("default", StringComparison.InvariantCulture))
									{
										int num6 = text3.IndexOf("\"", StringComparison.InvariantCulture);
										if (num6 == -1)
										{
											throw new ArgumentException("Name of DataColumn not found in property: " + text2);
										}
										num6++;
										int num7 = text3.IndexOf("\"", num6, StringComparison.InvariantCulture) - num6;
										if (num7 <= 0)
										{
											throw new ArgumentException("Name of DataColumn not found in property: " + text2);
										}
										text2 = text3.Substring(num6, num7);
										if (!text3.ToLower().StartsWith("default", StringComparison.InvariantCulture))
										{
											break;
										}
									}
								}
							}
							if (service.ExportFormat == StiExportFormat.Dbf && text2.Length > 10)
							{
								text2 = text2.Substring(0, 10);
							}
							Fields[m].Name = text2;
							Fields[m].readyName = true;
						}
						if (!Fields[m].readyType && array != null && (service.ExportFormat == StiExportFormat.Xml || service.ExportFormat == StiExportFormat.Json || service.ExportFormat == StiExportFormat.Dbf))
						{
							string[] array2 = array[0].Split(":".ToCharArray(), 3);
							array2[0] = array2[0].Trim().ToLower();
							if (array2[0].StartsWith("int", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 1;
								Fields[m].Info[1] = 15;
							}
							if (array2[0].StartsWith("long", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 2;
								Fields[m].Info[1] = 25;
							}
							if (array2[0].StartsWith("float", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 3;
								Fields[m].Info[1] = 15;
								Fields[m].Info[2] = 5;
							}
							if (array2[0].StartsWith("double", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 4;
								Fields[m].Info[1] = 20;
								Fields[m].Info[2] = 10;
							}
							if (array2[0].StartsWith("date", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 5;
								Fields[m].Info[1] = 8;
								Fields[m].Info[2] = 0;
							}
							if (array2[0].StartsWith("bool", StringComparison.InvariantCulture))
							{
								Fields[m].Info[0] = 6;
								Fields[m].Info[1] = 1;
								Fields[m].Info[2] = 0;
							}
							if (array2.Length > 1)
							{
								array2[1] = array2[1].Trim();
								int result = 0;
								if (int.TryParse(array2[1], out result))
								{
									if (result > 0)
									{
										Fields[m].Info[1] = result;
									}
								}
								else
								{
									result = 0;
								}
								if (array2.Length > 2)
								{
									array2[2] = array2[2].Trim();
									int result2 = 0;
									if (int.TryParse(array2[2], out result2))
									{
										if (result2 > 0 && result2 < result)
										{
											Fields[m].Info[2] = result2;
										}
									}
									else
									{
										result2 = 0;
									}
								}
							}
							Fields[m].readyType = true;
						}
					}
					StiRichText stiRichText = stiCell.Component as StiRichText;
					StiCheckBox stiCheckBox = stiCell.Component as StiCheckBox;
					string text4 = stiCell.Text;
					if (!string.IsNullOrWhiteSpace(text4) && stiCell.Component is StiText stiText && !string.IsNullOrWhiteSpace(stiText.Format) && stiText.Format != "g" && text4.IndexOf('\u00a0') != -1)
					{
						text4 = text4.Replace('\u00a0', ' ');
					}
					if (stiRichText != null && stiRichText.RtfText != string.Empty)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val);
						text4 = ((Control)val).Text;
					}
					if (text4 == null)
					{
						text4 = string.Empty;
					}
					if (stiCheckBox != null && stiCheckBox.CheckedValue != null)
					{
						bool flag2 = false;
						bool flag3 = false;
						if (stiCheckBox.CheckedValue is bool)
						{
							if ((bool)stiCheckBox.CheckedValue)
							{
								flag2 = true;
							}
							else
							{
								flag3 = true;
							}
						}
						if (stiCheckBox.CheckedValue is string)
						{
							if ((string)stiCheckBox.CheckedValue == "True")
							{
								flag2 = true;
							}
							else
							{
								flag3 = true;
							}
						}
						if (flag2)
						{
							text4 = StiOptions.Export.CheckBoxTextForTrue;
						}
						if (flag3)
						{
							text4 = StiOptions.Export.CheckBoxTextForFalse;
						}
					}
					if (!string.IsNullOrEmpty(text4))
					{
						if ((service.ExportFormat == StiExportFormat.Csv && StiOptions.Export.Csv.UseMultilineText) || service.ExportFormat == StiExportFormat.Xml || service.ExportFormat == StiExportFormat.Json)
						{
							text4 = text4.Replace("\r", "");
						}
						else
						{
							text4 = text4.Replace("\n", " ");
							text4 = text4.Replace("\r", "");
						}
						if (service.ExportFormat == StiExportFormat.Dbf && text4.Length > 254)
						{
							text4 = text4.Substring(0, 254);
						}
					}
					Fields[m].DataArray[num2] = text4;
				}
				else
				{
					Fields[m].DataArray[num2] = string.Empty;
				}
			}
			num2++;
		}
		((Component)(object)val)?.Dispose();
		for (int num8 = FieldsList.Count - 1; num8 >= 0; num8--)
		{
			if (!Fields[num8].readyName)
			{
				FieldsList.RemoveAt(num8);
			}
		}
		CopyFieldsListToFields();
		DataArrayLength = num;
	}

	public void CheckStylesNames()
	{
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < Styles.Count; i++)
		{
			StiCellStyle stiCellStyle = (StiCellStyle)Styles[i];
			string text = stiCellStyle.StyleName;
			string text2 = string.Empty;
			int num = 0;
			while (hashtable.Contains(text + text2))
			{
				num++;
				text2 = num.ToString();
			}
			if (text2 != string.Empty)
			{
				text = (stiCellStyle.StyleName = text + text2);
			}
			hashtable.Add(text, text);
		}
	}

	public Image GetRealImageData(StiCell cell, Image baseImage)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		if (cell == null || cell.Component == null || baseImage == null)
		{
			return null;
		}
		object obj = null;
		if (useCacheMode)
		{
			if (!(cell is StiCell2 stiCell))
			{
				return null;
			}
			string key = $"{stiCell.PageId}*{stiCell.ComponentId}";
			obj = imagesBaseRect[key];
		}
		else
		{
			obj = imagesBaseRect[cell.Component];
		}
		if (obj == null || !(obj is RectangleD val))
		{
			return null;
		}
		double num = ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.96 : 1.0);
		double num2 = ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.956 : 1.0);
		double num3 = Round(((RectangleD)(ref val)).Left * num);
		double num4 = Round(((RectangleD)(ref val)).Top * num2);
		double num5 = Round(((RectangleD)(ref val)).Right * num);
		double num6 = Round(((RectangleD)(ref val)).Bottom * num2);
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		if (leftCached[num3] == null)
		{
			num7 = CoordX.IndexOfValue(num3);
			leftCached[num3] = num7;
		}
		else
		{
			num7 = (int)leftCached[num3];
		}
		if (topCached[num4] == null)
		{
			num8 = CoordY.IndexOfValue(num4);
			topCached[num4] = num8;
		}
		else
		{
			num8 = (int)topCached[num4];
		}
		if (leftCached[num5] == null)
		{
			num9 = CoordX.IndexOfValue(num5);
			leftCached[num5] = num9;
		}
		else
		{
			num9 = (int)leftCached[num5];
		}
		if (topCached[num6] == null)
		{
			num10 = CoordY.IndexOfValue(num6);
			topCached[num6] = num10;
		}
		else
		{
			num10 = (int)topCached[num6];
		}
		if (num7 == -1 || num9 == -1 || num8 == -1 || num10 == -1)
		{
			return null;
		}
		if (num7 == cell.Left && num9 == cell.Left + cell.Width + 1 && num8 == cell.Top && num10 == cell.Top + cell.Height + 1)
		{
			return null;
		}
		int num11 = (int)(double)CoordX.GetByIndex(cell.Left);
		int num12 = (int)(double)CoordY.GetByIndex(cell.Top);
		int num13 = (int)(double)CoordX.GetByIndex(cell.Left + cell.Width + 1);
		int num14 = cell.Top + cell.Height + 1;
		if (num14 > CoordY.Count - 1)
		{
			num14 = CoordY.Count - 1;
		}
		int num15 = (int)(double)CoordY.GetByIndex(num14);
		double num16 = ((double)num11 - num3) / (num5 - num3);
		double num17 = ((double)num13 - num3) / (num5 - num3);
		double num18 = ((double)num12 - num4) / (num6 - num4);
		double num19 = ((double)num15 - num4) / (num6 - num4);
		int num20 = (int)((double)baseImage.Width * num16);
		int num21 = (int)Math.Round((double)baseImage.Width * num17 + 0.45);
		int num22 = (int)((double)baseImage.Height * num18);
		int num23 = (int)Math.Round((double)baseImage.Height * num19 + 0.45);
		Bitmap val2 = new Bitmap(num21 - num20, num23 - num22);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.PageUnit = (GraphicsUnit)2;
			val3.Clear(Color.FromArgb(1, 255, 255, 255));
			val3.DrawImage(baseImage, 0, 0, new Rectangle(num20, num22, num21 - num20, num23 - num22), (GraphicsUnit)2);
			return (Image)(object)val2;
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	private bool CheckComponentPlacement(StiComponent component, StiDataExportMode dataMode)
	{
		if (!StiOptions.Export.OptimizeDataOnlyMode)
		{
			return true;
		}
		if (dataMode == StiDataExportMode.AllBands)
		{
			return true;
		}
		string text = component.ComponentPlacement;
		if (text == null)
		{
			text = string.Empty;
		}
		int num = text.IndexOf("Hd_HPnl", StringComparison.InvariantCulture);
		if (num != -1)
		{
			text = text.Substring(0, num + 7);
		}
		StiTableLineInfo stiTableLineInfo = StiTableLineInfo.Unknown;
		if (text.StartsWith("d", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.Data;
		}
		else if (text.StartsWith("h.ap", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.HeaderD;
		}
		else if (text.StartsWith("f.ap", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.FooterD;
		}
		else if (text.StartsWith("h", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.HeaderD;
		}
		else if (text.StartsWith("f", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.FooterD;
		}
		else if (text.StartsWith("gh", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.HeaderD;
		}
		else if (text.StartsWith("gf", StringComparison.InvariantCulture))
		{
			stiTableLineInfo = StiTableLineInfo.FooterD;
		}
		if (((dataMode & StiDataExportMode.Data) > (StiDataExportMode)0 && stiTableLineInfo == StiTableLineInfo.Data) || ((dataMode & StiDataExportMode.Headers) > (StiDataExportMode)0 && stiTableLineInfo == StiTableLineInfo.HeaderD) || ((dataMode & StiDataExportMode.Footers) > (StiDataExportMode)0 && stiTableLineInfo == StiTableLineInfo.FooterD))
		{
			return true;
		}
		return false;
	}

	internal static void ReplaceComponentForExport(ref StiComponent component, bool replaceCheckboxes)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (!replaceCheckboxes)
		{
			return;
		}
		StiCheckBox stiCheckBox = component as StiCheckBox;
		bool flag = stiCheckBox != null;
		if (flag && (!(stiCheckBox.TextBrush is StiSolidBrush) || !StiBrush.ToColor(stiCheckBox.TextBrush).Equals((object?)stiCheckBox.ContourColor)))
		{
			flag = false;
		}
		if (!flag)
		{
			return;
		}
		StiText stiText = new StiText();
		stiText.Page = stiCheckBox.Page;
		stiText.ClientRectangle = stiCheckBox.ClientRectangle;
		stiText.Brush = stiCheckBox.Brush;
		stiText.TextBrush = stiCheckBox.TextBrush;
		stiText.Border = stiCheckBox.Border;
		stiText.HorAlignment = (StiTextHorAlignment)1;
		stiText.VertAlignment = (StiVertAlignment)1;
		double num = component.Report.Unit.ConvertToHInches(Math.Min(stiCheckBox.Width, stiCheckBox.Height));
		if (stiCheckBox.CheckedValue != null)
		{
			bool flag2 = false;
			string text = stiCheckBox.CheckedValue.ToString().Trim().ToLower(CultureInfo.InvariantCulture);
			string[] array = stiCheckBox.Values.Split('/', ';', ',');
			if (array != null && array.Length != 0)
			{
				string text2 = array[0].Trim().ToLower(CultureInfo.InvariantCulture);
				flag2 = text == text2;
			}
			char c = ' ';
			string text3 = null;
			switch (flag2 ? stiCheckBox.CheckStyleForTrue : stiCheckBox.CheckStyleForFalse)
			{
			case StiCheckStyle.Cross:
				c = '\uf0fb';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.Check:
				c = '\uf0fc';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.CrossRectangle:
				c = '\uf0fd';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.CheckRectangle:
				c = '\uf0fe';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.CrossCircle:
				c = '\uf056';
				text3 = "Wingdings 2";
				break;
			case StiCheckStyle.DotCircle:
				c = '\uf0a4';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.DotRectangle:
				c = '\uf0a9';
				text3 = "Wingdings 2";
				break;
			case StiCheckStyle.NoneCircle:
				c = '\uf0a1';
				text3 = "Wingdings";
				break;
			case StiCheckStyle.NoneRectangle:
				c = '\uf0a8';
				text3 = "Wingdings";
				break;
			}
			if (c != ' ')
			{
				stiText.Font = StiFontCollection.CreateFont(text3, (float)(num * 0.72), (FontStyle)0);
				stiText.Text = c.ToString();
			}
		}
		component = stiText;
	}

	public StiComponent GetCellComponent(StiCell cell2)
	{
		if (!(cell2 is StiCell2 { ComponentId: not -1 } stiCell))
		{
			return null;
		}
		if (stiCell.ComponentId == -2)
		{
			return additionalInfo;
		}
		if (lastPageId != stiCell.PageId)
		{
			lastPageId = stiCell.PageId;
			lastPage = pages[stiCell.PageId];
			pages.GetPage(lastPage);
			if (lastComps != null)
			{
				lastComps.Clear();
			}
			lastComps = lastPage.GetComponents();
		}
		StiComponent component = lastComps[stiCell.ComponentId];
		ReplaceComponentForExport(ref component, replaceCheckboxes);
		return component;
	}

	public void SetCellComponent(StiCell cell2, StiComponent component, int indexComponent)
	{
		if (!(cell2 is StiCell2 stiCell))
		{
			return;
		}
		if (additionalInfo != null && component == additionalInfo)
		{
			stiCell.ComponentId = -2;
			return;
		}
		if (lastPage != component.Page)
		{
			lastPage = component.Page;
			lastPageId = pages.IndexOf(lastPage);
			if (lastComps != null)
			{
				lastComps.Clear();
			}
			lastComps = lastPage.GetComponents();
		}
		stiCell.PageId = lastPageId;
		if (indexComponent >= 0)
		{
			stiCell.ComponentId = indexComponent;
		}
		else
		{
			stiCell.ComponentId = lastComps.IndexOf(component);
		}
	}

	public int GetBorderSideIndex(StiBorderSide side)
	{
		if (side == (StiBorderSide)null)
		{
			return 0;
		}
		if (BorderSides.Count > 0)
		{
			for (int i = 0; i < borderSides.Count; i++)
			{
				if (((object)borderSides[i]).Equals((object?)side))
				{
					return i + 1;
				}
			}
		}
		borderSides.Add(side);
		return borderSides.Count;
	}

	private void ClearCellsMapArea(double y1, double y2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect)).Y = y1;
		((RectangleD)(ref rect)).Height = y2 - y1;
		((RectangleD)(ref rect)).X = (double)CoordX.GetByIndex(0);
		((RectangleD)(ref rect)).Width = 0.0;
		double totalHeight = TotalHeight;
		TotalHeight = 0.0;
		Rectangle range = GetRange(rect);
		TotalHeight = totalHeight;
		for (int i = range.Y; i < range.Y + range.Height; i++)
		{
			for (int j = 0; j < CoordX.Count; j++)
			{
				CellsMap[i, j] = null;
			}
		}
	}

	internal static void GCCollect()
	{
		if (StiOptions.Engine.ReportCache.AllowGCCollect)
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}

	internal void AllowModification(bool flag)
	{
		cacheManager.flagForceSaveSegment = flag;
	}

	public void Clear()
	{
		cells2 = null;
		bordersX2 = null;
		bordersY2 = null;
		bookmarks2 = null;
		cellStyles2 = null;
		CellsMap = null;
		Styles = null;
		CoordX = null;
		CoordY = null;
		LinePlacement = null;
		ParentBandName = null;
		HorizontalPageBreaks = null;
		xcHash.Clear();
		xcHash = null;
		ycHash.Clear();
		ycHash = null;
		coordXCheck = null;
		coordYCheck = null;
		coordXNew = null;
		coordYNew = null;
		coordXPrim = null;
		coordYPrim = null;
		leftCached.Clear();
		leftCached = null;
		topCached.Clear();
		topCached = null;
		tagSplitCache.Clear();
		tagSplitCache = null;
		stylesCache.Clear();
		stylesCache = null;
		fontsCache.Clear();
		fontsCache = null;
		Fields = null;
		FieldsList = null;
		htName = null;
		report = null;
		pages = null;
		lastPage = null;
		if (lastComps != null)
		{
			lastComps.Clear();
		}
		if (borderSides != null)
		{
			borderSides.Clear();
		}
		borderSides = null;
		if (cacheManager != null)
		{
			cacheManager.Clear();
		}
	}

	public StiMatrix(StiPagesCollection pages, StiExportService service)
		: this(pages, checkForExcel: false, service, null)
	{
	}

	public StiMatrix(StiPagesCollection pages, bool checkForExcel, StiExportService service)
		: this(pages, checkForExcel, service, null)
	{
	}

	public StiMatrix(StiPagesCollection pages, StiExportService service, ArrayList styles)
		: this(pages, checkForExcel: false, service, styles)
	{
	}

	public StiMatrix(StiPagesCollection pages, bool checkForExcel, StiExportService service, ArrayList styles, StiDataExportMode dataMode = StiDataExportMode.AllBands, bool hasDividedPages = false)
	{
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Expected O, but got Unknown
		//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Expected O, but got Unknown
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf4: Expected O, but got Unknown
		//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c15: Expected O, but got Unknown
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_104f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1007: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Unknown result type (might be due to invalid IL or missing references)
		//IL_1428: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df8: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_1228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		report = pages.Report;
		this.pages = pages;
		exportFormat = service.ExportFormat;
		bool exportData = exportFormat == StiExportFormat.Dbf || exportFormat == StiExportFormat.Csv || exportFormat == StiExportFormat.Xml || exportFormat == StiExportFormat.Json;
		isHtmlService = service is StiHtmlExportService;
		isHtmlOrExcelXmlService = isHtmlService || service is StiExcelXmlExportService;
		isHtmlPngMode = service is StiHtmlExportService && (service as StiHtmlExportService).imageFormat == ImageFormat.Png;
		if (service is StiHtmlExportService)
		{
			addComponentWithInteractions = ((StiHtmlExportService)service).RenderWebInteractions;
		}
		if (service is StiExcel2007ExportService || service is StiExcelExportService)
		{
			replaceCheckboxes = !StiOptions.Export.Excel.RenderCheckBoxAsImage;
		}
		if (service is StiWord2007ExportService)
		{
			replaceCheckboxes = !StiOptions.Export.Word.RenderCheckBoxAsImage;
		}
		int num = -1;
		if (styles != null)
		{
			Styles = styles;
			num = styles.Count;
		}
		double[] array = new double[pages.Count + 1];
		double[] array2 = new double[pages.Count + 1];
		int num2 = 0;
		double num3 = 0.0;
		maxCoordY = 0.0;
		bool flag = pages.Report != null && pages.Report.IsPrinting;
		defaultLinePrimitiveWidth = 1.0 / ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.956 : 1.0);
		service.StatusString = StiLocalization.Get("Export", "ExportingCalculatingCoordinates");
		bool flag2 = true;
		if (service is StiRtfExportService)
		{
			flag2 = (service as StiRtfExportService).RemoveEmptySpaceAtBottom;
		}
		if (service is StiWord2007ExportService)
		{
			flag2 = (service as StiWord2007ExportService).RemoveEmptySpaceAtBottom;
		}
		if (service is StiOdtExportService)
		{
			flag2 = (service as StiOdtExportService).RemoveEmptySpaceAtBottom;
		}
		if (service is StiOdsExportService)
		{
			flag2 = StiOptions.Export.OpenDocumentCalc.RemoveEmptySpaceAtBottom;
		}
		if (service is StiExcelExportService)
		{
			flag2 = StiOptions.Export.Excel.RemoveEmptySpaceAtBottom;
		}
		if (service is StiExcel2007ExportService)
		{
			flag2 = StiOptions.Export.Excel.RemoveEmptySpaceAtBottom;
		}
		if (service is StiHtmlExportService)
		{
			flag2 = (service as StiHtmlExportService).RemoveEmptySpaceAtBottom;
		}
		hyperlinksToTag = new Hashtable();
		pointerToBookmark = new Hashtable();
		pointerToTag = new Hashtable();
		int num4 = 0;
		int num5 = 0;
		foreach (StiPage page in pages)
		{
			pages.GetPage(page);
			service.InvokeExporting(page, pages, service.CurrentPassNumber, service.MaximumPassNumber);
			if (service.IsStopped)
			{
				return;
			}
			int count = CoordY.Count;
			array[num2] = TotalHeight;
			array2[num2] = num3;
			num2++;
			RectangleD val = page.Unit.ConvertToHInches(page.ClientRectangle);
			RectangleD val2 = val;
			if (page.Border != null && (int)page.Border.Side != 0)
			{
				StiContainer stiContainer = new StiContainer
				{
					DisplayRectangle = page.ClientRectangle
				};
				AddCoord(page.Unit.ConvertToHInches(stiContainer.DisplayRectangle));
			}
			if (flag2 && !(service is StiHtmlExportService))
			{
				((RectangleD)(ref val)).Width = 0.0;
			}
			if (flag2 || (num4 == pages.Count - 1 && !(service is StiHtmlExportService)))
			{
				((RectangleD)(ref val)).Height = 0.0;
			}
			AddCoord(val);
			val = page.Unit.ConvertToHInches(page.ClientRectangle);
			foreach (StiComponent component in page.Components)
			{
				if (!component.Enabled || (flag && !component.Printable))
				{
					continue;
				}
				RectangleD rect = page.Unit.ConvertToHInches(component.DisplayRectangle);
				if (((RectangleD)(ref rect)).Height < 0.0)
				{
					((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height;
					((RectangleD)(ref rect)).Height = Math.Abs(((RectangleD)(ref rect)).Height);
				}
				bool flag3 = true;
				if (component is StiPointPrimitive)
				{
					flag3 = false;
				}
				if (((RectangleD)(ref rect)).Right < ((RectangleD)(ref val)).Left || ((RectangleD)(ref rect)).Left > ((RectangleD)(ref val)).Right || ((RectangleD)(ref rect)).Bottom < ((RectangleD)(ref val)).Top || ((RectangleD)(ref rect)).Top > ((RectangleD)(ref val)).Bottom)
				{
					flag3 = false;
				}
				if (StiOptions.Configuration.IsWeb && StiOptions.Engine.AllowInteractionInChartWithComponents && component.Name.EndsWith("Interaction#FX%"))
				{
					flag3 = false;
				}
				if (component is StiContainer && !string.IsNullOrWhiteSpace(component.Name) && component.Name.StartsWith("TAG##"))
				{
					flag3 = false;
				}
				if (!CheckComponentPlacement(component, dataMode))
				{
					flag3 = false;
				}
				if (!flag3)
				{
					continue;
				}
				if (component is StiLinePrimitive)
				{
					if (component is StiRectanglePrimitive)
					{
						AddCoord(rect);
						((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - defaultLinePrimitiveWidth;
						((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - defaultLinePrimitiveWidth;
					}
					else
					{
						if (((RectangleD)(ref rect)).Height < 1.5)
						{
							((RectangleD)(ref rect)).Height = defaultLinePrimitiveWidth;
							((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - ((RectangleD)(ref rect)).Height;
						}
						if (((RectangleD)(ref rect)).Width < 1.5)
						{
							((RectangleD)(ref rect)).Width = defaultLinePrimitiveWidth;
							((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - ((RectangleD)(ref rect)).Width;
						}
					}
				}
				else
				{
					if (((RectangleD)(ref rect)).Height < 1.5 && ((RectangleD)(ref rect)).Height > 0.0 && GetCoordYRounded(((RectangleD)(ref rect)).Top) == GetCoordYRounded(((RectangleD)(ref rect)).Bottom))
					{
						((RectangleD)(ref rect)).Height = 1.0 / (double)GetCoordYRounded(1.0) + 0.01;
					}
					if (((RectangleD)(ref rect)).Width < 1.5 && ((RectangleD)(ref rect)).Width > 0.0 && GetCoordXRounded(((RectangleD)(ref rect)).Left) == GetCoordXRounded(((RectangleD)(ref rect)).Right))
					{
						((RectangleD)(ref rect)).Width = 1.0 / (double)GetCoordXRounded(1.0) + 0.01;
					}
				}
				AddCoord(rect);
				if (component is StiText { ExceedMargins: not StiExceedMargins.None } stiText && !(stiText.Brush is StiEmptyBrush))
				{
					rect = page.Unit.ConvertToHInches(component.DisplayRectangle);
					if ((stiText.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + ((RectangleD)(ref rect)).Left;
						((RectangleD)(ref rect)).X = 0.0;
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + ((RectangleD)(ref rect)).Top;
						((RectangleD)(ref rect)).Y = 0.0;
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + (((RectangleD)(ref val2)).Width - ((RectangleD)(ref rect)).Right);
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + (((RectangleD)(ref val2)).Height - ((RectangleD)(ref rect)).Bottom);
					}
					AddCoord(rect);
				}
				if (component.HyperlinkValue != null)
				{
					string text = component.HyperlinkValue as string;
					if (!string.IsNullOrEmpty(text) && text.Length > 2 && text.StartsWith("##") && text[2] != '#')
					{
						hyperlinksToTag[text.Substring(2)] = null;
					}
				}
				if (component.PointerValue != null && !string.IsNullOrWhiteSpace((string)component.PointerValue) && !string.IsNullOrEmpty(component.Guid))
				{
					string key = $"{component.PointerValue}#GUID#{component.Guid}";
					string text2 = component.BookmarkValue as string;
					if (!string.IsNullOrWhiteSpace(text2))
					{
						pointerToBookmark[key] = text2;
					}
					string value = component.TagValue as string;
					if (text2 == null && !string.IsNullOrEmpty(value))
					{
						pointerToTag[key] = value;
					}
				}
			}
			if (hasDividedPages && maxCoordY > TotalHeight + ((RectangleD)(ref val)).Height)
			{
				maxCoordY = TotalHeight + ((RectangleD)(ref val)).Height;
			}
			TotalHeight = Round(maxCoordY);
			num3 = Round(maxCoordY * ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.956 : 1.0));
			TotalWidth = Math.Max(TotalWidth, page.Unit.ConvertToHInches(page.Width) * ((isHtmlService && StiOptions.Export.Html.PrintLayoutOptimization) ? 0.96 : 1.0));
			num4++;
			if (CoordY.Count - count > num5)
			{
				num5 = CoordY.Count - count;
			}
		}
		array[num2] = TotalHeight;
		foreach (DictionaryEntry item2 in pointerToTag)
		{
			if (hyperlinksToTag.Contains(item2.Value))
			{
				pointerToBookmark[item2.Key] = item2.Value;
			}
		}
		pointerToTag.Clear();
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag4 = StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) && StiDesignerAppStatus.IsRunning;
		bool flag5 = !(StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey) || flag4);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag5 = true;
		}
		if (!flag5)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag5 = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag5 = true;
			}
		}
		bool flag6 = false;
		if (flag5)
		{
			StiPage stiPage2 = ((pages.Count > 0) ? pages[0] : new StiPage(report));
			RectangleD rect2 = default(RectangleD);
			((RectangleD)(ref rect2))._002Ector(0.0, stiPage2.Unit.ConvertFromHInches(-20.0), stiPage2.Width, stiPage2.Unit.ConvertFromHInches(20.0));
			additionalInfo = new StiText(rect2, "Stimulsoft Reports - Trial");
			additionalInfo.HorAlignment = (StiTextHorAlignment)1;
			additionalInfo.VertAlignment = (StiVertAlignment)1;
			additionalInfo.Font = new Font("Arial", 12f, (FontStyle)1);
			additionalInfo.TextBrush = (StiBrush)new StiSolidBrush(Color.Red);
			additionalInfo.Brush = (StiBrush)new StiSolidBrush(Color.White);
			additionalInfo.Border = new StiBorder((StiBorderSides)15, Color.Black, 1.0, (StiPenStyle)0);
			additionalInfo.Page = stiPage2;
			flag6 = pages.Count > 0 && (exportFormat == StiExportFormat.Excel || exportFormat == StiExportFormat.ExcelXml || exportFormat == StiExportFormat.Odt || exportFormat == StiExportFormat.Ods || exportFormat == StiExportFormat.Rtf || exportFormat == StiExportFormat.RtfTable);
			if (flag6)
			{
				RectangleD val3 = stiPage2.Unit.ConvertToHInches(additionalInfo.DisplayRectangle);
				AddCoord(((RectangleD)(ref val3)).Left, ((RectangleD)(ref val3)).Top);
				AddCoord(((RectangleD)(ref val3)).Right, ((RectangleD)(ref val3)).Bottom);
			}
		}
		if (checkForExcel)
		{
			PrepareTable();
		}
		if (isHtmlOrExcelXmlService)
		{
			AddCoord(0.0, num3 + 1.0, convert: false);
		}
		for (int i = 0; i < CoordY.Count; i++)
		{
			topCached[CoordY.GetByIndex(i)] = i;
		}
		for (int j = 0; j < CoordX.Count; j++)
		{
			leftCached[CoordX.GetByIndex(j)] = j;
		}
		xcHash.Clear();
		ycHash.Clear();
		useCacheMode = StiOptions.Export.UseCacheModeForStiMatrix && pages.CacheMode;
		cacheManager = new StiMatrixCacheManager(this, CoordX.Count, CoordY.Count, num5);
		Cells = new StiMatrixCellsCollection(cacheManager, this);
		Bookmarks = new StiMatrixBookmarksCollection(cacheManager, this);
		BordersX = new StiMatrixBorderSidesXCollection(cacheManager, this);
		BordersY = new StiMatrixBorderSidesYCollection(cacheManager, this);
		CellStyles = new StiMatrixCellStylesCollection(cacheManager, this);
		if (!useCacheMode)
		{
			cells2 = new StiCell[CoordY.Count, CoordX.Count];
			bordersX2 = new StiBorderSide[CoordY.Count, CoordX.Count];
			bordersY2 = new StiBorderSide[CoordY.Count, CoordX.Count];
			bookmarks2 = new string[CoordY.Count, CoordX.Count];
		}
		CellsMap = new StiCell[CoordY.Count, CoordX.Count];
		coordXCheck = new bool[CoordX.Count];
		coordYCheck = new bool[CoordY.Count];
		coordXNew = new int[CoordX.Count];
		coordYNew = new int[CoordY.Count];
		coordXPrim = new int[CoordX.Count];
		coordYPrim = new int[CoordY.Count];
		imagesBaseRect = new Hashtable();
		num2 = 0;
		tempRoundedRectangleInfoList = new List<KeyValuePair<StiComponent, double>>();
		if (addComponentWithInteractions)
		{
			Interactions = new int[CoordY.Count, CoordX.Count, 2];
		}
		service.StatusString = StiLocalization.Get("Export", "ExportingFormatingObjects");
		ArrayList createdCells = null;
		num4 = 0;
		foreach (StiPage page2 in pages)
		{
			pages.GetPage(page2);
			service.InvokeExporting(page2, pages, service.CurrentPassNumber + 1, service.MaximumPassNumber);
			if (service.IsStopped)
			{
				return;
			}
			TotalHeight = array[num2++];
			if (page2.Border != null && (int)page2.Border.Side != 0)
			{
				RenderComponent(new StiContainer
				{
					DisplayRectangle = page2.ClientRectangle,
					Border = page2.Border,
					Brush = page2.Brush,
					Page = page2
				}, -1, exportData, isPage: true);
			}
			RectangleD val4 = page2.Unit.ConvertToHInches(page2.ClientRectangle);
			foreach (StiComponent component2 in page2.Components)
			{
				StiText stiText2 = component2 as StiText;
				if (!component2.Enabled || (flag && !component2.Printable) || stiText2 == null || stiText2.ExceedMargins == StiExceedMargins.None || stiText2.Brush is StiEmptyBrush)
				{
					continue;
				}
				RectangleD rect3 = page2.Unit.ConvertToHInches(component2.DisplayRectangle);
				if (((RectangleD)(ref rect3)).Height < 0.0)
				{
					((RectangleD)(ref rect3)).Y = ((RectangleD)(ref rect3)).Y + ((RectangleD)(ref rect3)).Height;
					((RectangleD)(ref rect3)).Height = Math.Abs(((RectangleD)(ref rect3)).Height);
				}
				bool flag7 = true;
				if (((RectangleD)(ref rect3)).Right < ((RectangleD)(ref val4)).Left || ((RectangleD)(ref rect3)).Left > ((RectangleD)(ref val4)).Right || ((RectangleD)(ref rect3)).Bottom < ((RectangleD)(ref val4)).Top || ((RectangleD)(ref rect3)).Top > ((RectangleD)(ref val4)).Bottom)
				{
					flag7 = false;
				}
				if (!CheckComponentPlacement(component2, dataMode))
				{
					flag7 = false;
				}
				if (flag7)
				{
					if ((stiText2.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect3)).Width = ((RectangleD)(ref rect3)).Width + ((RectangleD)(ref rect3)).Left;
						((RectangleD)(ref rect3)).X = 0.0;
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect3)).Height = ((RectangleD)(ref rect3)).Height + ((RectangleD)(ref rect3)).Top;
						((RectangleD)(ref rect3)).Y = 0.0;
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect3)).Width = ((RectangleD)(ref rect3)).Width + (((RectangleD)(ref val4)).Width - ((RectangleD)(ref rect3)).Right);
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
					{
						((RectangleD)(ref rect3)).Height = ((RectangleD)(ref rect3)).Height + (((RectangleD)(ref val4)).Height - ((RectangleD)(ref rect3)).Bottom);
					}
					RenderComponent(new StiText(page2.Unit.ConvertFromHInches(rect3))
					{
						Page = page2,
						Brush = stiText2.Brush
					}, -1, exportData, isPage: false);
				}
			}
			int num6 = 0;
			foreach (StiComponent component3 in page2.Components)
			{
				if (component3.Enabled && (!flag || component3.Printable))
				{
					RectangleD val5 = page2.Unit.ConvertToHInches(component3.DisplayRectangle);
					if (((RectangleD)(ref val5)).Height < 0.0)
					{
						((RectangleD)(ref val5)).Y = ((RectangleD)(ref val5)).Y + ((RectangleD)(ref val5)).Height;
						((RectangleD)(ref val5)).Height = Math.Abs(((RectangleD)(ref val5)).Height);
					}
					bool flag8 = true;
					if (component3 is StiPointPrimitive)
					{
						flag8 = false;
					}
					if (((RectangleD)(ref val5)).Right < ((RectangleD)(ref val4)).Left || ((RectangleD)(ref val5)).Left > ((RectangleD)(ref val4)).Right || ((RectangleD)(ref val5)).Bottom < ((RectangleD)(ref val4)).Top || ((RectangleD)(ref val5)).Top > ((RectangleD)(ref val4)).Bottom)
					{
						flag8 = false;
					}
					if (StiOptions.Configuration.IsWeb && StiOptions.Engine.AllowInteractionInChartWithComponents && component3.Name.EndsWith("Interaction#FX%"))
					{
						flag8 = false;
					}
					if (component3 is StiContainer && !string.IsNullOrWhiteSpace(component3.Name) && component3.Name.StartsWith("TAG##"))
					{
						flag8 = false;
					}
					if (!CheckComponentPlacement(component3, dataMode))
					{
						flag8 = false;
					}
					if (flag8)
					{
						RenderComponent(component3, num6, exportData, isPage: false);
					}
				}
				num6++;
			}
			if (page2.BookmarkValue != null)
			{
				string value2 = page2.BookmarkValue as string;
				if (!string.IsNullOrEmpty(value2))
				{
					Rectangle range = GetRange(page2.Unit.ConvertToHInches(page2.ClientRectangle));
					if (range.Left != -1)
					{
						if (range.Bottom == -1)
						{
							range.Height = CoordY.Count - 1 - range.Top;
						}
						for (int k = range.Y; k < range.Bottom; k++)
						{
							bool flag9 = false;
							for (int l = range.X; l < range.Right; l++)
							{
								if (CellsMap[k, l] != null && Bookmarks[k, l] == null)
								{
									Bookmarks[k, l] = value2;
									flag9 = true;
									break;
								}
							}
							if (flag9)
							{
								continue;
							}
							for (int m = range.X; m < range.Right; m++)
							{
								if (CellsMap[k, m] != null)
								{
									Bookmarks[k, m] = value2;
									break;
								}
							}
						}
					}
				}
			}
			if (flag5 && flag6 && num4 == 0 && additionalInfo != null)
			{
				RenderComponent(additionalInfo, -2, exportData, isPage: false);
			}
			if (useCacheMode)
			{
				ProcessIntersectedCells(createdCells);
				createdCells = this.createdCells;
				this.createdCells = new ArrayList();
				if (num4 > 0)
				{
					ClearCellsMapArea(array[num4 - 1], array[num4]);
				}
			}
			num4++;
		}
		if (useCacheMode)
		{
			ProcessIntersectedCells(createdCells);
			ProcessIntersectedCells(this.createdCells);
			ClearCellsMapArea(array[0], array[num4]);
			GCCollect();
		}
		else
		{
			ProcessIntersectedCells(this.createdCells);
		}
		int num7 = 0;
		for (int n = 0; n < CoordX.Count; n++)
		{
			coordXNew[n] = num7;
			if (n < CoordX.Count - 1 && (double)CoordX.GetByIndex(n + 1) - (double)CoordX.GetByIndex(n) < 1.1 && coordXPrim[n] == 1)
			{
				coordXCheck[n] = true;
				num7++;
			}
		}
		int num8 = 0;
		for (int num9 = 0; num9 < CoordY.Count; num9++)
		{
			coordYNew[num9] = num8;
			if (num9 < CoordY.Count - 1 && (double)CoordY.GetByIndex(num9 + 1) - (double)CoordY.GetByIndex(num9) < 1.1 && coordYPrim[num9] == 1)
			{
				coordYCheck[num9] = true;
				num8++;
			}
		}
		cacheManager.flagForceSaveSegment = true;
		if (true && (num8 > 0 || num7 > 0))
		{
			bool[,] array3 = new bool[CoordY.Count, CoordX.Count];
			for (int num10 = 0; num10 < CoordY.Count - 1; num10++)
			{
				for (int num11 = 0; num11 < CoordX.Count - 1; num11++)
				{
					int num12 = coordXNew[num11];
					int num13 = coordYNew[num10];
					StiCell stiCell = Cells[num10, num11];
					BordersX[num10 - num13, num11 - num12] = BordersX[num10, num11];
					BordersY[num10 - num13, num11 - num12] = BordersY[num10, num11];
					Bookmarks[num10 - num13, num11 - num12] = Bookmarks[num10, num11];
					if (addComponentWithInteractions)
					{
						Interactions[num10 - num13, num11 - num12, 0] = Interactions[num10, num11, 0];
						Interactions[num10 - num13, num11 - num12, 1] = Interactions[num10, num11, 1];
					}
					if (stiCell == null || array3[num10, num11])
					{
						continue;
					}
					for (int num14 = 0; num14 <= stiCell.Height; num14++)
					{
						for (int num15 = 0; num15 <= stiCell.Width; num15++)
						{
							array3[num10 + num14, num11 + num15] = true;
							if (num14 != 0 || num15 != 0)
							{
								Cells[num10 + num14, num11 + num15] = null;
							}
						}
					}
					if (coordXCheck[num11] || coordYCheck[num10])
					{
						Cells[num10, num11] = null;
						stiCell = null;
					}
					else
					{
						stiCell.Width -= coordXNew[num11 + stiCell.Width + 1] - num12;
						stiCell.Height -= coordYNew[num10 + stiCell.Height + 1] - num13;
						stiCell.Left -= num12;
						stiCell.Top -= num13;
					}
					if (num12 > 0 || num13 > 0)
					{
						Cells[num10 - num13, num11 - num12] = stiCell;
						Cells[num10, num11] = null;
						stiCell = null;
					}
				}
			}
			for (int num16 = 0; num16 < CoordX.Count - 1; num16++)
			{
				int num17 = coordXNew[num16];
				BordersX[CoordY.Count - 1 - num8, num16 - num17] = BordersX[CoordY.Count - 1, num16];
			}
			for (int num18 = 0; num18 < CoordY.Count - 1; num18++)
			{
				int num19 = coordYNew[num18];
				BordersY[num18 - num19, CoordX.Count - 1 - num7] = BordersY[num18, CoordX.Count - 1];
			}
			for (int num20 = CoordX.Count - 1 - 1; num20 >= 0; num20--)
			{
				if (coordXCheck[num20])
				{
					CoordX.RemoveAt(num20);
				}
			}
			for (int num21 = CoordY.Count - 1 - 1; num21 >= 0; num21--)
			{
				if (coordYCheck[num21])
				{
					CoordY.RemoveAt(num21);
				}
			}
		}
		leftCached.Clear();
		topCached.Clear();
		coordXNew = null;
		coordXCheck = null;
		coordXPrim = null;
		coordYNew = null;
		coordYCheck = null;
		coordYPrim = null;
		CellsMap = null;
		if (useCacheMode)
		{
			GCCollect();
		}
		if (isHtmlService)
		{
			bool[,] array4 = new bool[CoordY.Count, CoordX.Count];
			for (int num22 = 1; num22 < CoordY.Count - 1; num22++)
			{
				for (int num23 = 1; num23 < CoordX.Count; num23++)
				{
					if (array4[num22 - 1, num23 - 1])
					{
						continue;
					}
					StiCell stiCell2 = Cells[num22 - 1, num23 - 1];
					if (stiCell2 == null || !(stiCell2.Component is StiContainer) || stiCell2.CellStyle.Color.Equals((object?)Color.White) || stiCell2.CellStyle.Color.Equals((object?)Color.Transparent))
					{
						continue;
					}
					for (int num24 = 0; num24 <= stiCell2.Height; num24++)
					{
						for (int num25 = 0; num25 <= stiCell2.Width; num25++)
						{
							if (!array4[num22 - 1 + num24, num23 - 1 + num25])
							{
								GetMaxRectFromCell(stiCell2, num22 - 1 + num24, num23 - 1 + num25, array4);
							}
						}
					}
				}
			}
		}
		RoundedRectanglesList = new Dictionary<int, List<RoundedRectangleInfo>>();
		if (tempRoundedRectangleInfoList.Count > 0)
		{
			foreach (KeyValuePair<StiComponent, double> tempRoundedRectangleInfo in tempRoundedRectangleInfoList)
			{
				StiComponent key2 = tempRoundedRectangleInfo.Key;
				RectangleD rect4 = key2.Page.Unit.ConvertToHInches(key2.DisplayRectangle);
				if (((RectangleD)(ref rect4)).Height < 0.0)
				{
					((RectangleD)(ref rect4)).Y = ((RectangleD)(ref rect4)).Y + ((RectangleD)(ref rect4)).Height;
					((RectangleD)(ref rect4)).Height = Math.Abs(((RectangleD)(ref rect4)).Height);
				}
				TotalHeight = tempRoundedRectangleInfo.Value;
				Rectangle range2 = GetRange(rect4);
				if (range2.Left != -1)
				{
					if (!RoundedRectanglesList.TryGetValue(range2.Top, out var value3))
					{
						value3 = new List<RoundedRectangleInfo>();
						RoundedRectanglesList[range2.Top] = value3;
					}
					RoundedRectangleInfo item = new RoundedRectangleInfo
					{
						Rect = range2,
						Primitive = (key2 as StiRoundedRectanglePrimitive)
					};
					value3.Add(item);
				}
			}
			tempRoundedRectangleInfoList.Clear();
		}
		if (isHtmlOrExcelXmlService)
		{
			cellStyles2 = new StiCellStyle[CoordY.Count, CoordX.Count];
			bool[,] array5 = new bool[CoordY.Count, CoordX.Count];
			Font font = new Font("Arial", 8f);
			ArrayList styles2 = Styles;
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(Styles);
			if (num == -1)
			{
				Styles.Clear();
			}
			else
			{
				Styles.RemoveRange(num, Styles.Count - num);
			}
			stylesCache.Clear();
			ArrayList styles3 = new ArrayList();
			foreach (StiCellStyle style in Styles)
			{
				StiCellStyle.GetStyleFromCache(style.Color, style.TextColor, style.Font, style.HorAlignment, style.VertAlignment, style.Border, style.BorderL, style.BorderR, style.BorderB, style.TextOptions, style.WordWrap, style.Format, style.InternalStyleName, style.LineSpacing, style.AllowHtmlTags, stylesCache, styles3, fontsCache, style, simplyAdd: true);
			}
			for (int num26 = 1; num26 < CoordY.Count - 1; num26++)
			{
				for (int num27 = 1; num27 < CoordX.Count; num27++)
				{
					if (array5[num26 - 1, num27 - 1])
					{
						continue;
					}
					StiCell stiCell3 = Cells[num26 - 1, num27 - 1];
					if (stiCell3 != null)
					{
						for (int num28 = 0; num28 <= stiCell3.Height; num28++)
						{
							for (int num29 = 0; num29 <= stiCell3.Width; num29++)
							{
								array5[num26 - 1 + num28, num27 - 1 + num29] = true;
							}
						}
						bool flag10 = true;
						bool flag11 = true;
						for (int num30 = 0; num30 < stiCell3.Height + 1; num30++)
						{
							if (BordersY[stiCell3.Top + num30, stiCell3.Left] == (StiBorderSide)null)
							{
								flag10 = false;
							}
							if (BordersY[stiCell3.Top + num30, stiCell3.Left + stiCell3.Width + 1] == (StiBorderSide)null)
							{
								flag11 = false;
							}
						}
						bool flag12 = true;
						bool flag13 = true;
						for (int num31 = 0; num31 < stiCell3.Width + 1; num31++)
						{
							if (BordersX[stiCell3.Top, stiCell3.Left + num31] == (StiBorderSide)null)
							{
								flag12 = false;
							}
							if (BordersX[stiCell3.Top + stiCell3.Height + 1, stiCell3.Left + num31] == (StiBorderSide)null)
							{
								flag13 = false;
							}
						}
						Styles = arrayList;
						StiCellStyle cellStyle = stiCell3.CellStyle;
						Styles = styles2;
						StiCellStyle styleFromCache = StiCellStyle.GetStyleFromCache(cellStyle.Color, cellStyle.TextColor, cellStyle.Font, cellStyle.HorAlignment, cellStyle.VertAlignment, flag12 ? BordersX[stiCell3.Top, stiCell3.Left] : null, flag10 ? BordersY[stiCell3.Top, stiCell3.Left] : null, flag11 ? BordersY[stiCell3.Top, stiCell3.Left + stiCell3.Width + 1] : null, flag13 ? BordersX[stiCell3.Top + stiCell3.Height + 1, stiCell3.Left] : null, cellStyle.TextOptions, cellStyle.WordWrap, cellStyle.Format, cellStyle.InternalStyleName, cellStyle.LineSpacing, cellStyle.AllowHtmlTags, stylesCache, Styles, fontsCache, cellStyle, simplyAdd: false);
						CellStyles[num26 - 1, num27 - 1] = styleFromCache;
					}
					else
					{
						bool flag14 = true;
						bool flag15 = true;
						if (BordersY[num26 - 1, num27 - 1] == (StiBorderSide)null)
						{
							flag14 = false;
						}
						if (BordersY[num26 - 1, num27] == (StiBorderSide)null)
						{
							flag15 = false;
						}
						bool flag16 = true;
						bool flag17 = true;
						if (BordersX[num26 - 1, num27 - 1] == (StiBorderSide)null)
						{
							flag16 = false;
						}
						if (BordersX[num26, num27 - 1] == (StiBorderSide)null)
						{
							flag17 = false;
						}
						if (flag14 || flag15 || flag16 || flag17)
						{
							StiCellStyle styleFromCache2 = StiCellStyle.GetStyleFromCache(Color.Transparent, Color.Black, font, (StiTextHorAlignment)1, (StiVertAlignment)1, flag16 ? BordersX[num26 - 1, num27 - 1] : null, flag14 ? BordersY[num26 - 1, num27 - 1] : null, flag15 ? BordersY[num26 - 1, num27] : null, flag17 ? BordersX[num26, num27 - 1] : null, null, wordWrap: false, null, null, 1.0, allowHtmlTags: false, stylesCache, Styles, fontsCache, null, simplyAdd: false);
							CellStyles[num26 - 1, num27 - 1] = styleFromCache2;
						}
					}
				}
			}
			CheckStylesNames();
		}
		cacheManager.flagForceSaveSegment = false;
		if (pages.Count <= 1)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		for (int num32 = 1; num32 < pages.Count; num32++)
		{
			hashtable[array2[num32]] = null;
		}
		for (int num33 = 0; num33 < CoordY.Count; num33++)
		{
			double num34 = (double)CoordY.GetByIndex(num33);
			if (hashtable.ContainsKey(num34))
			{
				HorizontalPageBreaks.Add(num33);
			}
		}
	}
}
