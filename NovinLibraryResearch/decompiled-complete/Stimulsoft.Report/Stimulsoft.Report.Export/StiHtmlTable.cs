using System;
using System.Collections;
using System.Drawing;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public class StiHtmlTable
{
	public string BackImageUrl;

	public StiHtmlUnit Width;

	public StiBorder Border;

	public int BorderWidth;

	public int CellPadding;

	public int CellSpacing;

	public ArrayList Rows;

	public StiHorAlignment Align = (StiHorAlignment)1;

	public string Position;

	public const string MarginsKey = "padding";

	public const string PageBreakBeforeKey = "page-break-before";

	public const string VertAlignKey = "div:vertical-align";

	public const string HorAlignKey = "div:text-align";

	public const string WordwrapKey = "div:wordwrap";

	private static string wrongUrlSymbols = "\"&<";

	public static string StringToUrl(string input)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(input);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = bytes;
		foreach (byte b in array)
		{
			if (b <= 32 || b > 127 || wrongUrlSymbols.IndexOf((char)b) != -1)
			{
				if (b <= 32 || b > 127)
				{
					stringBuilder.Append($"%{b:x2}");
				}
				if (b == 34)
				{
					stringBuilder.Append("&quot;");
				}
				if (b == 38)
				{
					stringBuilder.Append("&amp;");
				}
				if (b == 60)
				{
					stringBuilder.Append("&lt;");
				}
			}
			else
			{
				stringBuilder.Append((char)b);
			}
		}
		return stringBuilder.ToString();
	}

	public void RenderControl(StiHtmlTextWriter writer, bool addPageBreaks)
	{
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Invalid comparison between Unknown and I4
		//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Invalid comparison between Unknown and I4
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Invalid comparison between Unknown and I4
		//IL_1000: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Invalid comparison between Unknown and I4
		//IL_1028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0d: Invalid comparison between Unknown and I4
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6d: Invalid comparison between Unknown and I4
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcd: Invalid comparison between Unknown and I4
		//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_101a: Invalid comparison between Unknown and I4
		//IL_1009: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1041: Invalid comparison between Unknown and I4
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_101d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Invalid comparison between Unknown and I4
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Invalid comparison between Unknown and I4
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Invalid comparison between Unknown and I4
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff2: Invalid comparison between Unknown and I4
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Invalid comparison between Unknown and I4
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Invalid comparison between Unknown and I4
		//IL_0f45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d76: Invalid comparison between Unknown and I4
		//IL_1a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1755: Unknown result type (might be due to invalid IL or missing references)
		//IL_175a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac5: Invalid comparison between Unknown and I4
		//IL_1b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b25: Invalid comparison between Unknown and I4
		//IL_1b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b85: Invalid comparison between Unknown and I4
		//IL_1bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd2: Invalid comparison between Unknown and I4
		//IL_1bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Invalid comparison between Unknown and I4
		//IL_1ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b39: Invalid comparison between Unknown and I4
		//IL_1b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b99: Invalid comparison between Unknown and I4
		//IL_1b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be6: Invalid comparison between Unknown and I4
		//IL_1bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Invalid comparison between Unknown and I4
		//IL_1bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18db: Invalid comparison between Unknown and I4
		//IL_1b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b06: Invalid comparison between Unknown and I4
		//IL_1af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b66: Invalid comparison between Unknown and I4
		//IL_1b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbe: Invalid comparison between Unknown and I4
		//IL_1bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1933: Unknown result type (might be due to invalid IL or missing references)
		//IL_1938: Unknown result type (might be due to invalid IL or missing references)
		//IL_193a: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Invalid comparison between Unknown and I4
		//IL_18dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e0: Invalid comparison between Unknown and I4
		//IL_1b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_193f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1942: Invalid comparison between Unknown and I4
		WriteTableBegin(writer, addPageBreaks, writePageBreak: false);
		foreach (StiHtmlTableRow row in Rows)
		{
			if (row.Style.ContainsKey("page-break-before"))
			{
				row.Style.Remove("page-break-before");
				WriteTableEnd(writer, addPageBreaks);
				writer.WriteLine();
				WriteTableBegin(writer, addPageBreaks, writePageBreak: true);
			}
			writer.WriteBeginTag("tr");
			if (StiOptions.Export.Html.UseExtendedStyle)
			{
				writer.WriteAttribute("class", "sBaseStyleFix");
			}
			if (!StiHtmlUnit.IsNullOrZero(row.Height))
			{
				row.Style["height"] = row.Height.ToString();
			}
			if (row.Style.Count > 0)
			{
				writer.Write(" style=\"");
				foreach (DictionaryEntry item in row.Style)
				{
					writer.WriteStyleAttribute((string)item.Key, (string)item.Value);
				}
				writer.Write("\"");
			}
			writer.WriteLine(">");
			writer.Indent++;
			foreach (StiHtmlTableCell cell in row.Cells)
			{
				writer.WriteBeginTag("td");
				if (!string.IsNullOrEmpty(cell.ToolTip))
				{
					writer.WriteAttribute("title", cell.ToolTip);
				}
				if (!string.IsNullOrEmpty(cell.CssClass))
				{
					writer.WriteAttribute("class", cell.CssClass);
				}
				if (!string.IsNullOrEmpty(cell.Id))
				{
					writer.WriteAttribute("id", cell.Id);
				}
				if (!string.IsNullOrEmpty(cell.Editable))
				{
					writer.WriteAttribute("editable", cell.Editable);
				}
				if (cell.ColumnSpan > 0)
				{
					writer.WriteAttribute("colspan", cell.ColumnSpan.ToString());
				}
				if (cell.RowSpan > 0)
				{
					writer.WriteAttribute("rowspan", cell.RowSpan.ToString());
				}
				if (!string.IsNullOrEmpty(cell.Interaction))
				{
					writer.WriteAttribute("interaction", cell.Interaction);
				}
				if (!string.IsNullOrEmpty(cell.Collapsed))
				{
					writer.WriteAttribute("collapsed", cell.Collapsed);
				}
				if (!string.IsNullOrEmpty(cell.SortDirection))
				{
					writer.WriteAttribute("sort", cell.SortDirection);
				}
				if (!string.IsNullOrEmpty(cell.DataBandSort))
				{
					writer.WriteAttribute("databandsort", cell.DataBandSort);
				}
				if (!string.IsNullOrEmpty(cell.PageGuid))
				{
					writer.WriteAttribute("pageguid", cell.PageGuid);
				}
				if (!string.IsNullOrEmpty(cell.PageIndex))
				{
					writer.WriteAttribute("pageindex", cell.PageIndex);
				}
				if (!string.IsNullOrEmpty(cell.ReportFile))
				{
					writer.WriteAttribute("reportfile", cell.ReportFile);
				}
				if (!string.IsNullOrEmpty(cell.ComponentIndex))
				{
					writer.WriteAttribute("compindex", cell.ComponentIndex);
				}
				if (!string.IsNullOrEmpty(cell.DrillDownMode))
				{
					writer.WriteAttribute("drilldownmode", cell.DrillDownMode);
				}
				if (!StiHtmlUnit.IsNullOrZero(cell.Height))
				{
					cell.Style["height"] = cell.Height.ToString();
				}
				if (!StiHtmlUnit.IsNullOrZero(cell.Width))
				{
					cell.Style["width"] = cell.Width.ToString();
				}
				object obj = null;
				string text = null;
				string text2 = null;
				if (cell.Style.Count > 0)
				{
					if (cell.Style.ContainsKey("padding"))
					{
						obj = cell.Style["padding"];
						cell.Style.Remove("padding");
					}
					if (cell.Style.ContainsKey("div:vertical-align") && cell.Style.ContainsKey("height"))
					{
						text = (string)cell.Style["div:vertical-align"];
						cell.Style.Remove("div:vertical-align");
					}
					if (cell.Style.ContainsKey("div:text-align") && cell.Style.ContainsKey("width"))
					{
						text2 = (string)cell.Style["div:text-align"];
						cell.Style.Remove("div:text-align");
					}
				}
				bool flag = false;
				if (cell.Style.ContainsKey("div:wordwrap") && cell.Style.ContainsKey("width"))
				{
					flag = true;
					cell.Style.Remove("div:wordwrap");
				}
				StiHtmlHyperlink stiHtmlHyperlink = null;
				StiHtmlImage stiHtmlImage = null;
				bool flag2 = false;
				if (cell.Controls.Count > 0)
				{
					stiHtmlHyperlink = cell.Controls[0] as StiHtmlHyperlink;
					stiHtmlImage = cell.Controls[0] as StiHtmlImage;
					foreach (object control in cell.Controls)
					{
						if (control is StiHtmlRoundedRectangle)
						{
							flag2 = true;
						}
					}
				}
				if (stiHtmlImage != null && !StiOptions.Export.Html.UseStrictTableCellSize)
				{
					cell.Style["line-height"] = "0";
				}
				if (flag2)
				{
					cell.Style["position"] = "relative";
					cell.Style["overflow"] = "visible";
				}
				if (cell.Style.Count > 0)
				{
					writer.Write(" style=\"");
					foreach (DictionaryEntry item2 in cell.Style)
					{
						writer.WriteStyleAttribute((string)item2.Key, (string)item2.Value);
					}
					if (StiOptions.Export.Html.UseStrictTableCellSizeV2 && obj != null)
					{
						writer.WriteStyleAttribute("padding", (string)obj);
					}
					writer.Write("\"");
				}
				writer.Write(">");
				if (flag2)
				{
					foreach (object control2 in cell.Controls)
					{
						if (control2 is StiHtmlRoundedRectangle stiHtmlRoundedRectangle)
						{
							string text3 = null;
							text3 = ((!stiHtmlRoundedRectangle.FitToCell) ? $"<div style=\"width:{stiHtmlRoundedRectangle.Width - stiHtmlRoundedRectangle.Size}px;height:{stiHtmlRoundedRectangle.Height - stiHtmlRoundedRectangle.Size}px;margin:-{stiHtmlRoundedRectangle.Size / 2.0}px;position:absolute;border:{stiHtmlRoundedRectangle.Size}px{stiHtmlRoundedRectangle.Style} {stiHtmlRoundedRectangle.Color};border-radius:{stiHtmlRoundedRectangle.Round}px;\"></div>" : string.Format("<div style=\"width:calc(100% - {0}px);height:calc(100% - {0}px);margin:-{1}px;position:absolute;border:{0}px{2} {3};border-radius:{4}px;\"></div>", stiHtmlRoundedRectangle.Size, stiHtmlRoundedRectangle.Size / 2.0, stiHtmlRoundedRectangle.Style, stiHtmlRoundedRectangle.Color, stiHtmlRoundedRectangle.Round));
							writer.Write(text3);
						}
					}
				}
				string text4 = null;
				bool flag3 = cell.Controls.Count > 0 || !string.IsNullOrEmpty(cell.Text);
				if (flag3 && StiOptions.Export.Html.UseStrictTableCellSize)
				{
					writer.Write("<div");
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						writer.WriteAttribute("class", "sBaseStyleFix");
						string text5 = cell.Style["Font"] as string;
						string text6 = cell.Style["text-decoration"] as string;
						string text7 = cell.Style["color"] as string;
						if (!string.IsNullOrEmpty(text5))
						{
							text4 = "Font:" + text5 + ";";
						}
						if (!string.IsNullOrEmpty(text6))
						{
							text4 = text4 + "text-decoration:" + text6 + ";";
						}
						if (!string.IsNullOrEmpty(text7))
						{
							text4 = text4 + "color:" + text7 + ";";
						}
					}
					if (cell.Style.ContainsKey("width") || cell.Style.ContainsKey("height") || text4 != null)
					{
						writer.Write(" style=\"");
						if (cell.Style.ContainsKey("width"))
						{
							writer.WriteStyleAttribute("width", (string)cell.Style["width"]);
						}
						if (cell.Style.ContainsKey("height"))
						{
							writer.WriteStyleAttribute("height", (string)cell.Style["height"]);
						}
						if (!string.IsNullOrEmpty(text4))
						{
							writer.Write(text4);
						}
						writer.Write("\"");
					}
					writer.Write(">");
					if (text != null || text2 != null)
					{
						writer.Write("<div ");
						if (StiOptions.Export.Html.UseExtendedStyle)
						{
							writer.Write("class=\"sBaseStyleFix\" ");
						}
						writer.Write("style=\"");
						writer.WriteStyleAttribute("display", "table-cell");
						if (text != null)
						{
							writer.WriteStyleAttribute("height", (string)cell.Style["height"]);
							writer.WriteStyleAttribute("vertical-align", text);
						}
						if (text2 != null)
						{
							writer.WriteStyleAttribute("width", (string)cell.Style["width"]);
							writer.WriteStyleAttribute("text-align", text2);
						}
						if (!string.IsNullOrEmpty(text4))
						{
							writer.Write(text4);
						}
						writer.Write("\">");
					}
					if (obj != null || flag)
					{
						writer.Write("<div ");
						if (StiOptions.Export.Html.UseExtendedStyle)
						{
							writer.Write("class=\"sBaseStyleFix\" ");
						}
						writer.Write("style=\"");
						if (obj != null)
						{
							writer.WriteStyleAttribute("margin", (string)obj);
						}
						if (flag)
						{
							if (obj != null)
							{
								try
								{
									string[] array = ((string)obj).Replace("px", "").Split(new char[1] { ' ' });
									cell.Width.Value -= double.Parse(array[1]) + double.Parse(array[3]);
								}
								catch
								{
								}
							}
							writer.WriteStyleAttribute("width", cell.Width.ToString());
						}
						if (!string.IsNullOrEmpty(text4))
						{
							writer.Write(text4);
						}
						writer.Write("\">");
					}
				}
				StiHorAlignment horAlignment;
				StiVertAlignment vertAlignment;
				if (stiHtmlImage != null)
				{
					if (stiHtmlImage.ImageComp != null && !stiHtmlImage.ImageComp.Stretch && stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.None)
					{
						writer.Write("<div style=\"");
						writer.WriteStyleAttribute("height", cell.Height.ToString());
						writer.WriteStyleAttribute("width", cell.Width.ToString());
						writer.WriteStyleAttribute("display", "table-cell");
						horAlignment = stiHtmlImage.ImageComp.HorAlignment;
						writer.WriteStyleAttribute("text-align", ((object)(StiHorAlignment)(ref horAlignment)).ToString().ToLower());
						vertAlignment = stiHtmlImage.ImageComp.VertAlignment;
						writer.WriteStyleAttribute("vertical-align", ((object)(StiVertAlignment)(ref vertAlignment)).ToString().ToLower().Replace("center", "middle"));
						writer.Write("\">");
						writer.Write("<div style=\"");
						writer.WriteStyleAttribute("max-height", cell.Height.ToString());
						writer.WriteStyleAttribute("max-width", cell.Width.ToString());
						writer.WriteStyleAttribute("overflow", "hidden");
						writer.Write("\">");
						writer.WriteBeginTag("img");
						if (!string.IsNullOrEmpty(stiHtmlImage.ToolTip))
						{
							writer.WriteAttribute("title", stiHtmlImage.ToolTip);
						}
						if (!string.IsNullOrEmpty(stiHtmlImage.ImageUrl))
						{
							writer.WriteAttribute("src", StringToUrl(stiHtmlImage.ImageUrl));
						}
						double num = 0.0;
						double num2 = 0.0;
						double num3 = stiHtmlImage.Width.Value * stiHtmlImage.ImageComp.MultipleFactor * (double)stiHtmlImage.Zoom;
						double num4 = stiHtmlImage.Height.Value * stiHtmlImage.ImageComp.MultipleFactor * (double)stiHtmlImage.Zoom;
						if (num3 > cell.Width.Value)
						{
							StiHorAlignment horAlignment2 = stiHtmlImage.ImageComp.HorAlignment;
							if ((int)horAlignment2 != 2)
							{
								if ((int)horAlignment2 == 3)
								{
									num = cell.Width.Value - num3;
								}
							}
							else
							{
								num = cell.Width.Value / 2.0 - num3 / 2.0;
							}
						}
						if (num4 > cell.Height.Value)
						{
							StiVertAlignment vertAlignment2 = stiHtmlImage.ImageComp.VertAlignment;
							if ((int)vertAlignment2 != 1)
							{
								if ((int)vertAlignment2 == 2)
								{
									num2 = cell.Height.Value - num4;
								}
							}
							else
							{
								num2 = cell.Height.Value / 2.0 - num4 / 2.0;
							}
						}
						writer.WriteAttribute("height", num4 + "px");
						writer.WriteAttribute("width", num3 + "px");
						writer.Write(" style=\"");
						writer.WriteStyleAttribute("border-width", StiHtmlUnit.ToPixelString(0));
						writer.WriteStyleAttribute("margin-top", StiHtmlUnit.ToPixelString((int)num2));
						writer.WriteStyleAttribute("margin-left", StiHtmlUnit.ToPixelString((int)num));
						writer.Write("\" /></div></div>");
					}
					else if (stiHtmlImage.ImageComp != null && (stiHtmlImage.ImageComp.AspectRatio || stiHtmlImage.ImageComp.ImageRotation != 0))
					{
						writer.Write("<div style=\"");
						writer.WriteStyleAttribute("width", cell.Width.ToString());
						writer.WriteStyleAttribute("height", cell.Height.ToString());
						StiHorAlignment val = stiHtmlImage.ImageComp.HorAlignment;
						StiVertAlignment val2 = stiHtmlImage.ImageComp.VertAlignment;
						switch (stiHtmlImage.ImageComp.ImageRotation)
						{
						case StiImageRotation.Rotate90CW:
							val2 = (((int)stiHtmlImage.ImageComp.HorAlignment == 1) ? ((StiVertAlignment)2) : (((int)stiHtmlImage.ImageComp.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)0)));
							val = (((int)stiHtmlImage.ImageComp.VertAlignment != 0) ? (((int)stiHtmlImage.ImageComp.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)3)) : ((StiHorAlignment)1));
							break;
						case StiImageRotation.Rotate90CCW:
							val2 = (((int)stiHtmlImage.ImageComp.HorAlignment == 1) ? ((StiVertAlignment)0) : (((int)stiHtmlImage.ImageComp.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)2)));
							val = (((int)stiHtmlImage.ImageComp.VertAlignment != 0) ? (((int)stiHtmlImage.ImageComp.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)1)) : ((StiHorAlignment)3));
							break;
						case StiImageRotation.Rotate180:
							if ((int)stiHtmlImage.ImageComp.HorAlignment == 1)
							{
								val = (StiHorAlignment)3;
							}
							else if ((int)stiHtmlImage.ImageComp.HorAlignment == 3)
							{
								val = (StiHorAlignment)1;
							}
							if ((int)stiHtmlImage.ImageComp.VertAlignment == 0)
							{
								val2 = (StiVertAlignment)2;
							}
							else if ((int)stiHtmlImage.ImageComp.VertAlignment == 2)
							{
								val2 = (StiVertAlignment)0;
							}
							break;
						case StiImageRotation.FlipHorizontal:
							if ((int)stiHtmlImage.ImageComp.HorAlignment == 1)
							{
								val = (StiHorAlignment)3;
							}
							else if ((int)stiHtmlImage.ImageComp.HorAlignment == 3)
							{
								val = (StiHorAlignment)1;
							}
							break;
						case StiImageRotation.FlipVertical:
							if ((int)stiHtmlImage.ImageComp.VertAlignment == 0)
							{
								val2 = (StiVertAlignment)2;
							}
							else if ((int)stiHtmlImage.ImageComp.VertAlignment == 2)
							{
								val2 = (StiVertAlignment)0;
							}
							break;
						}
						if (stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW || stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate90CW)
						{
							writer.Write("\"><div style=\"");
							writer.WriteStyleAttribute("width", cell.Height.ToString());
							writer.WriteStyleAttribute("height", cell.Width.ToString());
							double num5 = (cell.Width.Value - cell.Height.Value) / 2.0;
							writer.WriteStyleAttribute("position", "relative");
							writer.WriteStyleAttribute("left", StiHtmlUnit.NewUnit(num5).ToString());
							writer.WriteStyleAttribute("top", StiHtmlUnit.NewUnit(0.0 - num5).ToString());
							writer.WriteStyleAttribute("transform", "rotate(" + ((stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW) ? "-" : "") + "90deg)");
						}
						writer.WriteStyleAttribute("background-repeat", "no-repeat");
						writer.WriteStyleAttribute("background-position", ((object)(StiHorAlignment)(ref val)).ToString().ToLower() + " " + ((object)(StiVertAlignment)(ref val2)).ToString().ToLower());
						writer.WriteStyleAttribute("background-image", "url(" + StringToUrl(stiHtmlImage.ImageUrl) + ")");
						double num6 = stiHtmlImage.Width.Value * stiHtmlImage.ImageComp.MultipleFactor * (double)stiHtmlImage.Zoom;
						double num7 = stiHtmlImage.Height.Value * stiHtmlImage.ImageComp.MultipleFactor * (double)stiHtmlImage.Zoom;
						writer.WriteStyleAttribute("background-size", (!stiHtmlImage.ImageComp.Stretch) ? $"{num6}px {num7}px" : (stiHtmlImage.ImageComp.AspectRatio ? "contain" : "100% 100%"));
						if (stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.FlipHorizontal)
						{
							writer.WriteStyleAttribute("transform", "scaleX(-1)");
						}
						else if (stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.FlipVertical)
						{
							writer.WriteStyleAttribute("transform", "scaleY(-1)");
						}
						else if (stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate180)
						{
							writer.WriteStyleAttribute("transform", "scale(-1)");
						}
						if (stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW || stiHtmlImage.ImageComp.ImageRotation == StiImageRotation.Rotate90CW)
						{
							writer.Write("\"></div>");
						}
						else
						{
							writer.Write("\">");
						}
						writer.Write("</div>");
					}
					else
					{
						writer.WriteBeginTag("img");
						if (!string.IsNullOrEmpty(stiHtmlImage.ToolTip))
						{
							writer.WriteAttribute("title", stiHtmlImage.ToolTip);
						}
						if (!string.IsNullOrEmpty(stiHtmlImage.ImageUrl))
						{
							writer.WriteAttribute("src", StringToUrl(stiHtmlImage.ImageUrl));
						}
						writer.Write(" style=\"");
						StiHtmlUnit width = cell.Width;
						StiHtmlUnit height = cell.Height;
						if (stiHtmlImage != null && stiHtmlImage.UseImageSize && stiHtmlImage.Width != null && stiHtmlImage.Height != null)
						{
							double num8 = Math.Abs(width.Value / stiHtmlImage.Width.Value + height.Value / stiHtmlImage.Height.Value);
							if (num8 > 1.8 && num8 < 2.2)
							{
								width = stiHtmlImage.Width;
								height = stiHtmlImage.Height;
							}
						}
						if (stiHtmlImage.Margin == null)
						{
							writer.WriteStyleAttribute("height", height.ToString());
							writer.WriteStyleAttribute("width", width.ToString());
						}
						else
						{
							writer.WriteStyleAttribute("height", StiHtmlUnit.NewUnit(height.Value + Math.Abs(stiHtmlImage.Margin.Value * 2.0)).ToString());
							writer.WriteStyleAttribute("width", StiHtmlUnit.NewUnit(width.Value + Math.Abs(stiHtmlImage.Margin.Value * 2.0)).ToString());
							writer.WriteStyleAttribute("margin", stiHtmlImage.Margin.ToString());
						}
						writer.WriteStyleAttribute("border-width", StiHtmlUnit.ToPixelString(0));
						writer.Write("\" />");
					}
				}
				else if (stiHtmlHyperlink != null)
				{
					writer.WriteBeginTag("a");
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.OpenLinksTarget))
					{
						writer.WriteAttribute("target", stiHtmlHyperlink.OpenLinksTarget);
					}
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.ToolTip))
					{
						writer.WriteAttribute("title", stiHtmlHyperlink.ToolTip);
					}
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.CssClass))
					{
						writer.WriteAttribute("class", stiHtmlHyperlink.CssClass);
					}
					if (stiHtmlHyperlink.Attributes.ContainsKey("name"))
					{
						writer.WriteAttribute("name", (string)stiHtmlHyperlink.Attributes["name"]);
					}
					if (stiHtmlHyperlink.Attributes.ContainsKey("guid"))
					{
						writer.WriteAttribute("guid", (string)stiHtmlHyperlink.Attributes["guid"]);
					}
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.NavigateUrl))
					{
						writer.WriteAttribute("href", StringToUrl(stiHtmlHyperlink.NavigateUrl));
					}
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.ImageUrl))
					{
						stiHtmlHyperlink.Style["display"] = "block";
						stiHtmlHyperlink.Style["height"] = cell.Height.ToString();
						stiHtmlHyperlink.Style["width"] = cell.Width.ToString();
					}
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						stiHtmlHyperlink.Style["border"] = "0";
					}
					if (stiHtmlHyperlink.Style.Count > 0)
					{
						writer.Write(" style=\"");
						foreach (DictionaryEntry item3 in stiHtmlHyperlink.Style)
						{
							writer.WriteStyleAttribute((string)item3.Key, (string)item3.Value);
						}
						if (!string.IsNullOrEmpty(text4))
						{
							writer.Write(text4);
						}
						writer.Write("\"");
					}
					writer.Write(">");
					if (!string.IsNullOrEmpty(stiHtmlHyperlink.ImageUrl))
					{
						if (stiHtmlHyperlink.ImageComp != null && !stiHtmlHyperlink.ImageComp.Stretch && stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.None)
						{
							writer.Write("<div style=\"");
							writer.WriteStyleAttribute("height", cell.Height.ToString());
							writer.WriteStyleAttribute("width", cell.Width.ToString());
							writer.WriteStyleAttribute("display", "table-cell");
							horAlignment = stiHtmlHyperlink.ImageComp.HorAlignment;
							writer.WriteStyleAttribute("text-align", ((object)(StiHorAlignment)(ref horAlignment)).ToString().ToLower());
							vertAlignment = stiHtmlHyperlink.ImageComp.VertAlignment;
							writer.WriteStyleAttribute("vertical-align", ((object)(StiVertAlignment)(ref vertAlignment)).ToString().ToLower().Replace("center", "middle"));
							writer.Write("\">");
							writer.Write("<div style=\"");
							writer.WriteStyleAttribute("max-height", cell.Height.ToString());
							writer.WriteStyleAttribute("max-width", cell.Width.ToString());
							writer.WriteStyleAttribute("overflow", "hidden");
							writer.Write("\">");
							writer.WriteBeginTag("img");
							if (!string.IsNullOrEmpty(stiHtmlHyperlink.ToolTip))
							{
								writer.WriteAttribute("title", stiHtmlHyperlink.ToolTip);
							}
							if (!string.IsNullOrEmpty(stiHtmlHyperlink.ImageUrl))
							{
								writer.WriteAttribute("src", StringToUrl(stiHtmlHyperlink.ImageUrl));
							}
							double num9 = 0.0;
							double num10 = 0.0;
							double num11 = stiHtmlHyperlink.Width.Value * stiHtmlHyperlink.ImageComp.MultipleFactor * (double)stiHtmlHyperlink.Zoom;
							double num12 = stiHtmlHyperlink.Height.Value * stiHtmlHyperlink.ImageComp.MultipleFactor * (double)stiHtmlHyperlink.Zoom;
							if (num11 > cell.Width.Value)
							{
								StiHorAlignment horAlignment3 = stiHtmlHyperlink.ImageComp.HorAlignment;
								if ((int)horAlignment3 != 2)
								{
									if ((int)horAlignment3 == 3)
									{
										num9 = cell.Width.Value - num11;
									}
								}
								else
								{
									num9 = cell.Width.Value / 2.0 - num11 / 2.0;
								}
							}
							if (num12 > cell.Height.Value)
							{
								StiVertAlignment vertAlignment3 = stiHtmlHyperlink.ImageComp.VertAlignment;
								if ((int)vertAlignment3 != 1)
								{
									if ((int)vertAlignment3 == 2)
									{
										num10 = cell.Height.Value - num12;
									}
								}
								else
								{
									num10 = cell.Height.Value / 2.0 - num12 / 2.0;
								}
							}
							writer.WriteAttribute("height", num12 + "px");
							writer.WriteAttribute("width", num11 + "px");
							writer.Write(" style=\"");
							writer.WriteStyleAttribute("border-width", StiHtmlUnit.ToPixelString(0));
							writer.WriteStyleAttribute("margin-top", StiHtmlUnit.ToPixelString((int)num10));
							writer.WriteStyleAttribute("margin-left", StiHtmlUnit.ToPixelString((int)num9));
							writer.Write("\" /></div></div>");
						}
						else if (stiHtmlHyperlink.ImageComp != null && (stiHtmlHyperlink.ImageComp.AspectRatio || stiHtmlHyperlink.ImageComp.ImageRotation != 0))
						{
							writer.Write("<div style=\"");
							writer.WriteStyleAttribute("width", cell.Width.ToString());
							writer.WriteStyleAttribute("height", cell.Height.ToString());
							StiHorAlignment val3 = stiHtmlHyperlink.ImageComp.HorAlignment;
							StiVertAlignment val4 = stiHtmlHyperlink.ImageComp.VertAlignment;
							switch (stiHtmlHyperlink.ImageComp.ImageRotation)
							{
							case StiImageRotation.Rotate90CW:
								val4 = (((int)stiHtmlHyperlink.ImageComp.HorAlignment == 1) ? ((StiVertAlignment)2) : (((int)stiHtmlHyperlink.ImageComp.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)0)));
								val3 = (((int)stiHtmlHyperlink.ImageComp.VertAlignment != 0) ? (((int)stiHtmlHyperlink.ImageComp.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)3)) : ((StiHorAlignment)1));
								break;
							case StiImageRotation.Rotate90CCW:
								val4 = (((int)stiHtmlHyperlink.ImageComp.HorAlignment == 1) ? ((StiVertAlignment)0) : (((int)stiHtmlHyperlink.ImageComp.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)2)));
								val3 = (((int)stiHtmlHyperlink.ImageComp.VertAlignment != 0) ? (((int)stiHtmlHyperlink.ImageComp.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)1)) : ((StiHorAlignment)3));
								break;
							case StiImageRotation.Rotate180:
								if ((int)stiHtmlHyperlink.ImageComp.HorAlignment == 1)
								{
									val3 = (StiHorAlignment)3;
								}
								else if ((int)stiHtmlHyperlink.ImageComp.HorAlignment == 3)
								{
									val3 = (StiHorAlignment)1;
								}
								if ((int)stiHtmlHyperlink.ImageComp.VertAlignment == 0)
								{
									val4 = (StiVertAlignment)2;
								}
								else if ((int)stiHtmlHyperlink.ImageComp.VertAlignment == 2)
								{
									val4 = (StiVertAlignment)0;
								}
								break;
							case StiImageRotation.FlipHorizontal:
								if ((int)stiHtmlHyperlink.ImageComp.HorAlignment == 1)
								{
									val3 = (StiHorAlignment)3;
								}
								else if ((int)stiHtmlHyperlink.ImageComp.HorAlignment == 3)
								{
									val3 = (StiHorAlignment)1;
								}
								break;
							case StiImageRotation.FlipVertical:
								if ((int)stiHtmlHyperlink.ImageComp.VertAlignment == 0)
								{
									val4 = (StiVertAlignment)2;
								}
								else if ((int)stiHtmlHyperlink.ImageComp.VertAlignment == 2)
								{
									val4 = (StiVertAlignment)0;
								}
								break;
							}
							if (stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW || stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate90CW)
							{
								writer.Write("\"><div style=\"");
								writer.WriteStyleAttribute("width", cell.Height.ToString());
								writer.WriteStyleAttribute("height", cell.Width.ToString());
								double num13 = (cell.Width.Value - cell.Height.Value) / 2.0;
								writer.WriteStyleAttribute("position", "relative");
								writer.WriteStyleAttribute("left", StiHtmlUnit.NewUnit(num13).ToString());
								writer.WriteStyleAttribute("top", StiHtmlUnit.NewUnit(0.0 - num13).ToString());
								writer.WriteStyleAttribute("transform", "rotate(" + ((stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW) ? "-" : "") + "90deg)");
							}
							writer.WriteStyleAttribute("background-repeat", "no-repeat");
							writer.WriteStyleAttribute("background-position", ((object)(StiHorAlignment)(ref val3)).ToString().ToLower() + " " + ((object)(StiVertAlignment)(ref val4)).ToString().ToLower());
							writer.WriteStyleAttribute("background-image", "url(" + StringToUrl(stiHtmlHyperlink.ImageUrl) + ")");
							double num14 = stiHtmlHyperlink.Width.Value * stiHtmlHyperlink.ImageComp.MultipleFactor * (double)stiHtmlHyperlink.Zoom;
							double num15 = stiHtmlHyperlink.Height.Value * stiHtmlHyperlink.ImageComp.MultipleFactor * (double)stiHtmlHyperlink.Zoom;
							writer.WriteStyleAttribute("background-size", (!stiHtmlHyperlink.ImageComp.Stretch) ? $"{num14}px {num15}px" : (stiHtmlHyperlink.ImageComp.AspectRatio ? "contain" : "100% 100%"));
							if (stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.FlipHorizontal)
							{
								writer.WriteStyleAttribute("transform", "scaleX(-1)");
							}
							else if (stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.FlipVertical)
							{
								writer.WriteStyleAttribute("transform", "scaleY(-1)");
							}
							else if (stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate180)
							{
								writer.WriteStyleAttribute("transform", "scale(-1)");
							}
							if (stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate90CCW || stiHtmlHyperlink.ImageComp.ImageRotation == StiImageRotation.Rotate90CW)
							{
								writer.Write("\"></div>");
							}
							else
							{
								writer.Write("\">");
							}
							writer.Write("</div>");
						}
						else
						{
							writer.WriteBeginTag("img");
							if (!string.IsNullOrEmpty(stiHtmlHyperlink.ToolTip))
							{
								writer.WriteAttribute("title", stiHtmlHyperlink.ToolTip);
							}
							if (!string.IsNullOrEmpty(stiHtmlHyperlink.ImageUrl))
							{
								writer.WriteAttribute("src", StringToUrl(stiHtmlHyperlink.ImageUrl));
							}
							writer.Write(" style=\"");
							StiHtmlUnit width2 = cell.Width;
							StiHtmlUnit height2 = cell.Height;
							if (stiHtmlImage != null && stiHtmlImage.UseImageSize && stiHtmlImage.Width != null && stiHtmlImage.Height != null)
							{
								double num16 = Math.Abs(width2.Value / stiHtmlImage.Width.Value + height2.Value / stiHtmlImage.Height.Value);
								if (num16 > 1.8 && num16 < 2.2)
								{
									width2 = stiHtmlImage.Width;
									height2 = stiHtmlImage.Height;
								}
							}
							if (stiHtmlHyperlink.Margin == null)
							{
								writer.WriteStyleAttribute("height", height2.ToString());
								writer.WriteStyleAttribute("width", width2.ToString());
							}
							else
							{
								writer.WriteStyleAttribute("height", StiHtmlUnit.NewUnit(height2.Value + Math.Abs(stiHtmlHyperlink.Margin.Value * 2.0)).ToString());
								writer.WriteStyleAttribute("width", StiHtmlUnit.NewUnit(width2.Value + Math.Abs(stiHtmlHyperlink.Margin.Value * 2.0)).ToString());
								writer.WriteStyleAttribute("margin", stiHtmlHyperlink.Margin.ToString());
							}
							writer.WriteStyleAttribute("border-width", StiHtmlUnit.ToPixelString(0));
							writer.Write("\" />");
						}
					}
					writer.Write(stiHtmlHyperlink.Text ?? cell.Text);
					writer.WriteFullEndTag("a");
				}
				else
				{
					if (flag2)
					{
						writer.Write("<div style=\"display:table-cell;");
						if (cell.Style.ContainsKey("width"))
						{
							writer.WriteStyleAttribute("width", (string)cell.Style["width"]);
						}
						if (cell.Style.ContainsKey("height"))
						{
							writer.WriteStyleAttribute("height", (string)cell.Style["height"]);
						}
						if (text != null)
						{
							writer.WriteStyleAttribute("vertical-align", text);
						}
						if (text2 != null)
						{
							writer.WriteStyleAttribute("text-align", text2);
						}
						writer.Write("\">");
					}
					writer.Write(cell.Text);
					if (flag2)
					{
						writer.Write("</div>");
					}
				}
				if (flag3 && StiOptions.Export.Html.UseStrictTableCellSize)
				{
					if (obj != null || flag)
					{
						writer.Write("</div>");
					}
					if (text != null || text2 != null)
					{
						writer.Write("</div>");
					}
					writer.Write("</div>");
				}
				writer.WriteFullEndTag("td");
			}
			writer.WriteLine();
			writer.Indent--;
			writer.WriteFullEndTag("tr");
		}
		WriteTableEnd(writer, addPageBreaks);
	}

	private void WriteTableBegin(StiHtmlTextWriter writer, bool writePadding, bool writePageBreak)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Invalid comparison between Unknown and I4
		if (writePadding)
		{
			writer.WriteBeginTag("div");
			writer.WriteAttribute("class", "pagemargins");
			if (writePageBreak)
			{
				writer.Write(" style=\"");
				writer.WriteStyleAttribute("page-break-before", "always");
				writer.Write("\"");
			}
			writer.WriteLine(">");
			writer.Indent++;
		}
		writer.WriteBeginTag("table");
		writer.WriteAttribute("cellspacing", CellSpacing.ToString());
		writer.WriteAttribute("cellpadding", CellPadding.ToString());
		writer.WriteAttribute("border", "0");
		if ((int)Align != 1)
		{
			writer.WriteAttribute("align", ((int)Align == 2) ? "center" : "right");
		}
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			writer.WriteAttribute("class", "sBaseStyleFix");
		}
		writer.Write(" style=\"");
		writer.WriteStyleAttribute("border-width", StiHtmlUnit.ToPixelString(BorderWidth));
		writer.WriteStyleAttribute("width", Width.ToString());
		if (Border != null && Border.DropShadow)
		{
			Color color = StiBrush.ToColor(Border.ShadowBrush);
			writer.WriteStyleAttribute("box-shadow", $"{Border.ShadowSize}px {Border.ShadowSize}px 1px rgb({color.R},{color.G},{color.B})");
		}
		if (!string.IsNullOrEmpty(Position))
		{
			writer.WriteStyleAttribute("Position", Position);
		}
		if (!string.IsNullOrEmpty(BackImageUrl))
		{
			writer.WriteStyleAttribute("background-image", $"url('{StringToUrl(BackImageUrl)}')");
		}
		writer.WriteStyleAttribute("border-collapse", "collapse");
		writer.WriteStyleAttribute("white-space", "normal");
		writer.Write("\">");
		writer.WriteLine();
		writer.Indent++;
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			writer.WriteBeginTag("tbody");
			writer.WriteAttribute("class", "sBaseStyleFix");
			writer.WriteLine(">");
			writer.Indent++;
		}
	}

	private void WriteTableEnd(StiHtmlTextWriter writer, bool writePadding)
	{
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			writer.WriteLine();
			writer.Indent--;
			writer.WriteFullEndTag("tbody");
		}
		writer.WriteLine();
		writer.Indent--;
		writer.WriteFullEndTag("table");
		if (writePadding)
		{
			writer.WriteLine();
			writer.Indent--;
			writer.WriteFullEndTag("div");
		}
	}

	public StiHtmlTable()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Rows = new ArrayList();
	}
}
