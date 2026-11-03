using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Engine;

public class StiTableOfContentsV2Builder : StiDataBandV2Builder
{
	private static string dotsString;

	private static string DotsString
	{
		get
		{
			if (dotsString == null)
			{
				dotsString = new string('.', 300);
			}
			return dotsString;
		}
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiTableOfContents stiTableOfContents = masterComp as StiTableOfContents;
		if (stiTableOfContents.NewPageBefore)
		{
			stiTableOfContents.Report.Engine.NewPage();
		}
		StiComponent stiComponent = (stiTableOfContents.IsFirstInReport ? RenderPanelWithBookmarks(stiTableOfContents) : RenderTextWithNoInformation(stiTableOfContents));
		if (stiTableOfContents.NewPageAfter)
		{
			(stiComponent as StiContainer).Components.Add(new StiNewPageContainer());
		}
		return stiComponent;
	}

	private static StiComponent RenderTextWithNoInformation(StiComponent comp)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		StiTableOfContents stiTableOfContents = comp as StiTableOfContents;
		StiPanel stiPanel = new StiPanel();
		stiPanel.CanGrow = false;
		stiPanel.CanShrink = false;
		stiPanel.CanBreak = false;
		stiPanel.GrowToHeight = false;
		stiPanel.ClientRectangle = stiTableOfContents.ClientRectangle;
		stiPanel.Name = stiTableOfContents.Name;
		stiPanel.Page = stiTableOfContents.Page;
		StiBorder border = stiTableOfContents.Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiPanel.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = stiTableOfContents.Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiPanel.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		stiPanel.RightToLeft = stiTableOfContents.RightToLeft;
		StiPanel stiPanel2 = stiPanel;
		StiText stiText = new StiText
		{
			ClientRectangle = new RectangleD(0.0, 0.0, stiPanel2.Width, stiPanel2.Height),
			Name = stiTableOfContents.Name,
			Page = stiTableOfContents.Page,
			Printable = stiTableOfContents.Printable,
			HorAlignment = (StiTextHorAlignment)1,
			VertAlignment = (StiVertAlignment)1,
			RightToLeft = stiTableOfContents.RightToLeft
		};
		stiText.Text.Value = Loc.Get("Errors", "OneTableOfContentsAllowed");
		stiPanel2.Components.Add(stiText);
		return stiPanel2;
	}

	private static StiComponent RenderPanelWithBookmarks(StiComponent comp)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		StiTableOfContents stiTableOfContents = comp as StiTableOfContents;
		StiPanel stiPanel = new StiPanel();
		stiPanel.CanGrow = true;
		stiPanel.CanShrink = true;
		stiPanel.CanBreak = true;
		stiPanel.GrowToHeight = stiTableOfContents.GrowToHeight;
		stiPanel.ClientRectangle = stiTableOfContents.ClientRectangle;
		stiPanel.TagValue = stiTableOfContents;
		stiPanel.Name = stiTableOfContents.Name;
		stiPanel.Page = stiTableOfContents.Page;
		StiBorder border = stiTableOfContents.Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiPanel.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = stiTableOfContents.Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiPanel.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		StiPanel stiPanel2 = stiPanel;
		if (comp.Report.IsSecondPass)
		{
			double posX = 0.0;
			double posY = stiTableOfContents.Page.Unit.ConvertFromHInches(stiTableOfContents.Margins.Top);
			RenderPointers(1, "TOC", stiTableOfContents, stiPanel2, GetPointers(stiTableOfContents, comp), ref posX, ref posY);
		}
		if (stiPanel2.Components.Count > 0)
		{
			double val = stiPanel2.Components.Cast<StiComponent>().Max((StiComponent c) => c.Bottom);
			stiPanel2.Height = Math.Max(stiPanel2.Height, val) + stiTableOfContents.Page.Unit.ConvertFromHInches(stiTableOfContents.Margins.Bottom);
		}
		return stiPanel2;
	}

	private static void RenderPointers(int level, string baseName, StiTableOfContents masterComp, StiPanel resultComp, StiBookmarksCollection bookmarks, ref double posX, ref double posY)
	{
		if (bookmarks == null)
		{
			return;
		}
		StiBaseStyle style = GetStyle(level, masterComp);
		int num = 1;
		foreach (StiBookmark bookmark in bookmarks)
		{
			StiText stiText = null;
			string hyperlink = GetHyperlink(bookmark);
			if (!(bookmark.ParentComponent is StiReport))
			{
				stiText = new StiText
				{
					Brush = null,
					WordWrap = false,
					Top = posY,
					Width = FullSize(resultComp),
					Height = FullSize(resultComp),
					Page = resultComp.Page,
					Printable = masterComp.Printable,
					Name = $"{resultComp.Name}_Index_{baseName}_{num}",
					VertAlignment = (StiVertAlignment)1,
					TextQuality = StiTextQuality.Wysiwyg,
					TagValue = bookmark,
					HyperlinkValue = hyperlink,
					RightToLeft = masterComp.RightToLeft,
					Margins = new StiMargins(0.0, masterComp.Margins.Right, 0.0, 0.0)
				};
				style?.SetStyleToComponent(stiText);
				stiText.HorAlignment = (StiTextHorAlignment)2;
				stiText.Left = (masterComp.RightToLeft ? 0.0 : (resultComp.Width - FullSize(resultComp)));
				stiText.NewGuid();
				SetOffSides(masterComp.RightToLeft, setLeftOff: true, setRightOff: false, stiText);
				resultComp.Components.Add(stiText);
			}
			StiText stiText2 = new StiText
			{
				Brush = null,
				WordWrap = false,
				Top = posY,
				Height = FullSize(resultComp),
				Name = $"{resultComp.Name}_Name_{baseName}_{num}",
				VertAlignment = (StiVertAlignment)1,
				Page = resultComp.Page,
				Printable = masterComp.Printable,
				TextQuality = StiTextQuality.Wysiwyg,
				Guid = stiText?.Guid,
				RightToLeft = masterComp.RightToLeft,
				Margins = new StiMargins(masterComp.Margins.Left, 0.0, 0.0, 0.0)
			};
			if (stiText != null)
			{
				stiText2.HyperlinkValue = hyperlink;
			}
			stiText2.Text.Value = bookmark.Text;
			if (stiText != null)
			{
				stiText2.Left = (masterComp.RightToLeft ? stiText.Width : posX);
				stiText2.Width = (masterComp.RightToLeft ? (resultComp.Width - stiText.Right - posX) : (stiText.Left - stiText2.Left));
				stiText2.Text.Value += DotsString;
			}
			else
			{
				stiText2.Width = resultComp.Width;
			}
			stiText2.TextOptions.Trimming = (StringTrimming)0;
			style?.SetStyleToComponent(stiText2);
			float num2 = ((style is StiStyle) ? (style as StiStyle).LineSpacing : 1f);
			stiText2.HorAlignment = (StiTextHorAlignment)0;
			stiText2.Height = masterComp.Report.Unit.ConvertFromHInches((double)(stiText2.Font.GetHeight() * num2));
			if (stiText != null)
			{
				SetOffSides(masterComp.RightToLeft, setLeftOff: false, setRightOff: true, stiText2);
			}
			resultComp.Components.Insert(0, stiText2);
			if (stiText != null)
			{
				stiText.Height = stiText2.Height;
			}
			posY += stiText2.Height;
			if (bookmark.Bookmarks.Count > 0)
			{
				double num3 = posX;
				posX += Indent(masterComp);
				RenderPointers(level + 1, $"{baseName}_{num}", masterComp, resultComp, bookmark.Bookmarks, ref posX, ref posY);
				posX = num3;
			}
			num++;
		}
	}

	internal static void PostProcessTableOfContents(StiReport report)
	{
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		if (!report.GetComponents().Cast<StiComponent>().Any((StiComponent c) => c is StiTableOfContents && c.Enabled))
		{
			return;
		}
		List<StiComponent> list = report.GetRenderedComponents().ToList();
		StiTableOfContents masterComp = report.Pages.ToList().SelectMany((StiPage p) => p.GetComponents().ToList()).FirstOrDefault((StiComponent c) => c is StiTableOfContents) as StiTableOfContents;
		StiComponent stiComponent = list.FirstOrDefault((StiComponent c) => c.Name == masterComp?.Name);
		if (stiComponent != null)
		{
			PostProcessBookmarks(list, masterComp, stiComponent);
		}
		foreach (StiComponent item in list.Where((StiComponent c) => c.TagValue is StiBookmark))
		{
			StiBookmark bookmark = item.TagValue as StiBookmark;
			StiComponent stiComponent2 = (string.IsNullOrWhiteSpace(bookmark.ComponentGuid) ? null : list.FirstOrDefault((StiComponent c) => c.Guid == bookmark.ComponentGuid));
			StiText textIndex = item as StiText;
			if (stiComponent2 != null)
			{
				textIndex.Text.Value = report.Engine.PageNumbers.GetPageNumber(stiComponent2.Page).ToString();
			}
			textIndex.AutoWidth = true;
			double right = textIndex.Right;
			StiText stiText = textIndex;
			SizeD actualSize = textIndex.GetActualSize();
			stiText.Width = ((SizeD)(ref actualSize)).Width;
			if (!masterComp.RightToLeft)
			{
				textIndex.Left = right - textIndex.Width;
			}
			textIndex.AutoWidth = false;
			StiComponent stiComponent3 = list.FirstOrDefault((StiComponent c) => c.Guid == textIndex.Guid && c != textIndex);
			if (stiComponent3 != null)
			{
				if (masterComp.RightToLeft)
				{
					stiComponent3.Width = stiComponent3.Right - textIndex.Right;
					stiComponent3.Left = textIndex.Right;
				}
				else
				{
					stiComponent3.Width = textIndex.Left - stiComponent3.Left;
				}
			}
		}
	}

	public static void PostProcessBookmarks(List<StiComponent> comps, StiTableOfContents masterComp, StiComponent resultComp)
	{
		PostProcessBookmarks(comps, "TOC", resultComp, GetPointersForPostProcessing(masterComp, resultComp));
	}

	private static StiBookmarksCollection GetPointers(StiTableOfContents masterComp, StiComponent comp)
	{
		StiBookmark firstPassPointer = comp.Report.Engine.FirstPassPointer;
		if (string.IsNullOrWhiteSpace(masterComp.ReportPointer))
		{
			return firstPassPointer?.Bookmarks;
		}
		firstPassPointer.Text = StiReportParser.Parse(masterComp.ReportPointer, (IStiReportComponent)(object)masterComp);
		firstPassPointer.ParentComponent = masterComp.Report;
		return new StiBookmarksCollection { firstPassPointer };
	}

	private static StiBookmarksCollection GetPointersForPostProcessing(StiTableOfContents masterComp, StiComponent resultComp)
	{
		if (string.IsNullOrWhiteSpace(masterComp.ReportPointer))
		{
			return resultComp.Report.Pointer.Bookmarks;
		}
		return new StiBookmarksCollection { resultComp.Report.Pointer };
	}

	public static void PostProcessBookmarks(List<StiComponent> comps, string baseName, StiComponent panel, StiBookmarksCollection bookmarks)
	{
		int num = 1;
		foreach (StiBookmark bookmark in bookmarks)
		{
			string strIndex = $"{panel.Name}_Index_{baseName}_{num}";
			string strName = $"{panel.Name}_Name_{baseName}_{num}";
			StiComponent stiComponent = comps.FirstOrDefault((StiComponent c) => c.Name == strIndex);
			if (stiComponent != null)
			{
				stiComponent.TagValue = bookmark;
				stiComponent.HyperlinkValue = GetHyperlink(bookmark);
			}
			StiComponent stiComponent2 = comps.FirstOrDefault((StiComponent c) => c.Name == strName);
			if (stiComponent2 != null)
			{
				stiComponent2.HyperlinkValue = GetHyperlink(bookmark);
			}
			if (bookmark.Bookmarks.Count > 0)
			{
				PostProcessBookmarks(comps, $"{baseName}_{num}", panel, bookmark.Bookmarks);
			}
			num++;
		}
	}

	private static string GetHyperlink(StiBookmark bookmark)
	{
		return "###" + bookmark.Text + "#GUID#" + bookmark.ComponentGuid;
	}

	private static void SetOffSides(bool isRightToLeft, bool setLeftOff, bool setRightOff, StiText text)
	{
		if (isRightToLeft)
		{
			if (setLeftOff)
			{
				SetOffSide(text, (StiBorderSides)4);
			}
			if (setRightOff)
			{
				SetOffSide(text, (StiBorderSides)2);
			}
		}
		else
		{
			if (setLeftOff)
			{
				SetOffSide(text, (StiBorderSides)2);
			}
			if (setRightOff)
			{
				SetOffSide(text, (StiBorderSides)4);
			}
		}
	}

	private static void SetOffSide(StiText text, StiBorderSides side)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if ((text.Border.Side & side) > 0)
		{
			StiBorder border = text.Border;
			border.Side -= side;
		}
	}

	private static StiBaseStyle GetStyle(int level, StiTableOfContents masterComp)
	{
		List<StiStyle> stylesList = masterComp.GetStylesList();
		if (level >= 1 && level <= stylesList.Count)
		{
			return stylesList[level - 1];
		}
		if (stylesList.Count > 0)
		{
			return stylesList.Last();
		}
		return null;
	}

	private static double FullSize(StiComponent comp)
	{
		return comp.Report.Unit.ConvertFromHInches(20.0);
	}

	private static double Indent(StiTableOfContents table)
	{
		return table.Report.Unit.ConvertFromHInches((double)table.Indent);
	}
}
