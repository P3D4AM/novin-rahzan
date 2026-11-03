using System;
using System.Collections;
using System.Security.Cryptography;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Engine;

internal sealed class StiPostProcessProviderV2
{
	private const string nullGuid = "nullGuid";

	internal static void PostProcessPages(StiPagesCollection pages)
	{
		int num = 0;
		int num2 = 0;
		foreach (StiPage page in pages)
		{
			num2 += page.SegmentPerWidth * page.SegmentPerHeight;
		}
		bool[] array = new bool[num2 + 1];
		bool[] array2 = new bool[num2 + 1];
		if (num2 > 0)
		{
			array[1] = true;
		}
		array2[num2] = true;
		StiPageNumberHelper stiPageNumberHelper = null;
		if (pages.Report != null && pages.Report.Engine != null && pages.Report.Engine.PageNumbers != null)
		{
			if (pages.Report.ReportPass == StiReportPass.Second)
			{
				stiPageNumberHelper = pages.Report.Engine.PageNumbers;
			}
			StiPageNumberHelper pageNumbers = pages.Report.Engine.PageNumbers;
			num = 1;
			int num3 = Math.Min(pages.Count, pageNumbers.PageNumbers.Count);
			for (int i = 0; i < num3; i++)
			{
				if (pageNumbers.PageNumbers[i].ResetPageNumber)
				{
					array[num] = true;
					array2[num - 1] = true;
				}
				num += pageNumbers.PageNumbers[i].SegmentPerWidth * pageNumbers.PageNumbers[i].SegmentPerHeight;
			}
		}
		num = 1;
		foreach (StiPage page2 in pages)
		{
			if (stiPageNumberHelper != null && stiPageNumberHelper.IsFinished)
			{
				num = stiPageNumberHelper.GetPageNumber(page2);
				num2 = stiPageNumberHelper.GetTotalPageCount(page2);
				PostProcessPage(page2, num == 1, num == num2);
			}
			else
			{
				PostProcessPage(page2, array[num], array2[num]);
			}
			num += page2.SegmentPerWidth * page2.SegmentPerHeight;
		}
		PostProcessPrimitives(pages);
	}

	private static void RemoveAllPointPrimitives(StiContainer container)
	{
		int num = 0;
		while (num < container.Components.Count)
		{
			if (container.Components[num] is StiPointPrimitive)
			{
				container.Components.RemoveAt(num);
				continue;
			}
			if (container.Components[num] is StiContainer container2)
			{
				RemoveAllPointPrimitives(container2);
			}
			num++;
		}
	}

	internal static void PostProcessPrimitives(StiPage page)
	{
		StiPagesCollection pages = new StiPagesCollection(null) { page };
		PostProcessPrimitives(pages);
	}

	internal static void PostProcessPrimitives(StiPagesCollection pages)
	{
		Hashtable startPointsHash = new Hashtable();
		Hashtable endPointsHash = new Hashtable();
		Hashtable lines = new Hashtable();
		ArrayList endPoints = new ArrayList();
		foreach (StiPage page in pages)
		{
			PostProcessPrimitivesInContainer(page, pages, ref startPointsHash, ref endPointsHash, ref lines, ref endPoints);
		}
		foreach (StiPage page2 in pages)
		{
			RemoveAllPointPrimitives(page2);
		}
	}

	internal static void PostProcessPrimitivesInContainer(StiContainer container)
	{
		Hashtable startPointsHash = new Hashtable();
		Hashtable endPointsHash = new Hashtable();
		Hashtable lines = new Hashtable();
		ArrayList endPoints = new ArrayList();
		PostProcessPrimitivesInContainer(container, null, ref startPointsHash, ref endPointsHash, ref lines, ref endPoints);
		RemoveAllPointPrimitives(container);
	}

	internal static void PostProcessPrimitivesInContainer(StiContainer container, StiPagesCollection pages, ref Hashtable startPointsHash, ref Hashtable endPointsHash, ref Hashtable lines, ref ArrayList endPoints)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		stiComponentsCollection.AddRange(container.Components);
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is StiCrossLinePrimitive { Guid: not null } stiCrossLinePrimitive && stiCrossLinePrimitive.Guid != "nullGuid")
			{
				lines[stiCrossLinePrimitive.Guid] = stiCrossLinePrimitive;
				stiCrossLinePrimitive.Parent.Components.Remove(item);
			}
		}
		if (lines.Count <= 0)
		{
			return;
		}
		stiComponentsCollection = container.GetComponents();
		endPoints.Clear();
		foreach (StiComponent item2 in stiComponentsCollection)
		{
			if (!(item2 is StiPrimitive))
			{
				continue;
			}
			if (item2 is StiStartPointPrimitive stiStartPointPrimitive)
			{
				if (stiStartPointPrimitive.ReferenceToGuid == null)
				{
					continue;
				}
				StiPanel stiPanel = container as StiPanel;
				ArrayList arrayList = startPointsHash[stiStartPointPrimitive.ReferenceToGuid] as ArrayList;
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					startPointsHash[stiStartPointPrimitive.ReferenceToGuid] = arrayList;
					if (stiPanel != null && stiPanel.Columns > 1)
					{
						arrayList.Add(stiStartPointPrimitive);
					}
				}
				if ((stiPanel != null && stiPanel.Columns < 2) || stiPanel == null)
				{
					arrayList.Add(stiStartPointPrimitive);
				}
			}
			else if (item2 is StiEndPointPrimitive stiEndPointPrimitive && !ProcessOneEndPoint(startPointsHash, lines, container, pages, stiEndPointPrimitive))
			{
				endPoints.Add(stiEndPointPrimitive);
			}
		}
		foreach (StiEndPointPrimitive endPoint in endPoints)
		{
			ProcessOneEndPoint(startPointsHash, lines, container, pages, endPoint);
		}
	}

	private static bool ProcessOneEndPoint(Hashtable startPointsHash, Hashtable lines, StiContainer page, StiPagesCollection pages, StiEndPointPrimitive endPoint)
	{
		if (endPoint.ReferenceToGuid == null)
		{
			return false;
		}
		if (!(startPointsHash[endPoint.ReferenceToGuid] is ArrayList { Count: not 0 } arrayList))
		{
			return false;
		}
		startPointsHash[endPoint.ReferenceToGuid] = null;
		StiStartPointPrimitive stiStartPointPrimitive = arrayList[0] as StiStartPointPrimitive;
		if (!(lines[stiStartPointPrimitive.ReferenceToGuid] is StiCrossLinePrimitive crossLine))
		{
			return true;
		}
		StiStartPointPrimitive startPoint = (StiStartPointPrimitive)arrayList[0];
		ProcessOnePrimitive(pages, page, crossLine, startPoint, null, endPoint);
		return true;
	}

	private static void ProcessOnePrimitive(StiPagesCollection pages, StiContainer page, StiCrossLinePrimitive crossLine, StiPrimitive startPoint, StiPrimitive nextStartPoint, StiPrimitive endPoint)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		PointD startPos = ((StiComponent)startPoint).ComponentToPage(new PointD(startPoint.Left, startPoint.Top));
		PointD endPos = ((StiComponent)endPoint).ComponentToPage(new PointD(endPoint.Left, endPoint.Top));
		int storedColumn = ((StiPointPrimitive)startPoint).StoredColumn;
		int storedColumn2 = ((StiPointPrimitive)endPoint).StoredColumn;
		storedColumn = Math.Max(1, storedColumn);
		storedColumn2 = Math.Max(1, storedColumn2);
		if (startPoint.Page != endPoint.Page || storedColumn != storedColumn2)
		{
			int num = pages?.IndexOf(startPoint.Page) ?? 1;
			int num2 = pages?.IndexOf(endPoint.Page) ?? 1;
			int num3 = -1;
			if (nextStartPoint != null && pages != null)
			{
				num3 = pages.IndexOf(nextStartPoint.Page);
			}
			for (int i = num; i <= num2; i++)
			{
				if (num != num2 && nextStartPoint != null && num3 == num2 && i == num2)
				{
					continue;
				}
				StiPage stiPage = pages[i];
				if (stiPage != startPoint.Page)
				{
					storedColumn = 1;
				}
				if (stiPage != endPoint.Page)
				{
					storedColumn2 = Math.Max(stiPage.Columns, storedColumn2);
				}
				double num4 = 0.0;
				for (int j = storedColumn; j <= storedColumn2; j++)
				{
					double positionFromTop = stiPage.PageInfoV2.PositionFromTop;
					double positionFromBottom = stiPage.PageInfoV2.PositionFromBottom;
					double num5 = stiPage.GetColumnWidth() + stiPage.ColumnGaps;
					double x = ((PointD)(ref startPos)).X;
					double y = ((PointD)(ref startPos)).Y;
					double num6 = ((PointD)(ref endPos)).X;
					double y2 = ((PointD)(ref endPos)).Y;
					if (storedColumn != storedColumn2)
					{
						for (num6 -= num5 * (double)Math.Min(storedColumn2 - storedColumn, stiPage.Columns); num6 < x; num6 += num5)
						{
							if (!(num5 > 0.0))
							{
								break;
							}
						}
					}
					if (i == num && storedColumn == j)
					{
						AddPrimitive(crossLine, new PointD(num4 + x, y), new PointD(num4 + num6, positionFromBottom), stiPage);
					}
					else if (i == num2 && storedColumn2 == j)
					{
						AddPrimitive(crossLine, new PointD(num4 + x, positionFromTop), new PointD(num4 + num6, y2), stiPage);
					}
					else
					{
						AddPrimitive(crossLine, new PointD(num4 + x, positionFromTop), new PointD(num4 + num6, positionFromBottom), stiPage);
					}
					num4 += num5;
				}
			}
		}
		else
		{
			AddPrimitive(crossLine, startPos, endPos, page);
		}
	}

	internal static void AddPrimitive(StiCrossLinePrimitive crossLine, PointD startPos, PointD endPos, StiContainer page)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		if (crossLine is StiRoundedRectanglePrimitive)
		{
			StiCrossLinePrimitive stiCrossLinePrimitive = ((StiService)crossLine).Clone() as StiCrossLinePrimitive;
			stiCrossLinePrimitive.Guid = "nullGuid";
			stiCrossLinePrimitive.Left = ((PointD)(ref startPos)).X;
			stiCrossLinePrimitive.Top = ((PointD)(ref startPos)).Y;
			stiCrossLinePrimitive.Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X;
			stiCrossLinePrimitive.Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y;
			page.Components.Add(stiCrossLinePrimitive);
		}
		else if (crossLine is StiRectanglePrimitive)
		{
			if (((StiRectanglePrimitive)crossLine).LeftSide)
			{
				page.Components.Add(new StiVerticalLinePrimitive
				{
					Name = crossLine.Name + "_Left",
					Guid = "nullGuid",
					Page = crossLine.Page,
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				});
			}
			if (((StiRectanglePrimitive)crossLine).RightSide)
			{
				page.Components.Add(new StiVerticalLinePrimitive
				{
					Name = crossLine.Name + "_Right",
					Guid = "nullGuid",
					Page = crossLine.Page,
					Left = ((PointD)(ref endPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				});
			}
			if (((StiRectanglePrimitive)crossLine).TopSide)
			{
				page.Components.Add(new StiHorizontalLinePrimitive
				{
					Name = crossLine.Name + "_Top",
					Page = crossLine.Page,
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = crossLine.Page.Unit.ConvertFromHInches(1.0),
					Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				});
			}
			if (((StiRectanglePrimitive)crossLine).BottomSide)
			{
				page.Components.Add(new StiHorizontalLinePrimitive
				{
					Name = crossLine.Name + "_Bottom",
					Page = crossLine.Page,
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref endPos)).Y,
					Height = crossLine.Page.Unit.ConvertFromHInches(1.0),
					Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				});
			}
		}
		else
		{
			StiCrossLinePrimitive stiCrossLinePrimitive2 = ((StiService)crossLine).Clone() as StiCrossLinePrimitive;
			stiCrossLinePrimitive2.Guid = "nullGuid";
			stiCrossLinePrimitive2.Left = ((PointD)(ref startPos)).X;
			stiCrossLinePrimitive2.Top = ((PointD)(ref startPos)).Y;
			stiCrossLinePrimitive2.Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X;
			stiCrossLinePrimitive2.Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y;
			page.Components.Add(stiCrossLinePrimitive2);
		}
	}

	internal static void CopyStyles(StiLinePrimitive dest, StiLinePrimitive source)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		dest.Style = source.Style;
		dest.Color = source.Color;
		dest.Size = source.Size;
	}

	internal static void PostProcessPage(StiPage page, bool isFirstPage, bool isLastPage)
	{
		PostProcessPage(page, isFirstPage, isLastPage, clearPage: true);
	}

	internal static void PostProcessPage(StiPage page, bool isFirstPage, bool isLastPage, bool clearPage)
	{
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		((StiService)page).PackService();
		PostProcessPrintOn(page, isFirstPage, isLastPage);
		StiComponentsCollection components = page.GetComponents();
		page.DockToContainer();
		foreach (StiComponent item in components)
		{
			item.DockStyle = StiDockStyle.None;
			item.Page = page;
			((StiService)item).PackService();
			if (item is StiText { ShrinkFontToFit: not false } stiText)
			{
				stiText.Font = stiText.GetActualFont(stiText.Text, stiText.ShrinkFontToFitMinimumSize);
			}
		}
		Hashtable hashtable = new Hashtable();
		foreach (StiComponent item2 in components)
		{
			if (item2 is IStiText && ((IStiText)item2).ProcessingDuplicates != 0)
			{
				hashtable[item2] = item2.Parent;
			}
			if (item2 is StiImage && ((StiImage)item2).ProcessingDuplicates != 0)
			{
				hashtable[item2] = item2.Parent;
			}
		}
		for (int num = components.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent3 = components[num];
			if (stiComponent3.TagValue is string && (string)stiComponent3.TagValue == "*CheckCanShrinkLaterMark*")
			{
				stiComponent3.CanShrink = true;
				SizeD actualSize = stiComponent3.GetActualSize();
				stiComponent3.Height = ((SizeD)(ref actualSize)).Height;
				stiComponent3.TagValue = null;
			}
		}
		if (clearPage)
		{
			page.ClearPage();
			if (isFirstPage || isLastPage)
			{
				StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
				bool flag = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
				if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
				{
					flag = true;
				}
				if (!flag)
				{
					try
					{
						using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
						using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
						rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
						flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
					}
					catch (Exception)
					{
						flag = true;
					}
				}
				if (flag && page.Report != null)
				{
					double num2 = page.Report.Unit.ConvertFromHInches(200.0);
					double num3 = page.Report.Unit.ConvertFromHInches(52.0);
					RectangleD rect = default(RectangleD);
					((RectangleD)(ref rect))._002Ector((page.Width - num2) / 2.0, page.Height - num3, num2, num3);
					StiImage stiImage = new StiImage(rect)
					{
						Stretch = true,
						Printable = false
					};
					stiImage.ImageBytes = StiExportUtils.GetAdditionalData2(192);
					stiImage.Name = StiNameValidator.CorrectName(StiGuidUtils.NewGuid().Substring(0, 8));
					page.Components.Add(stiImage);
				}
			}
		}
		StiPostProcessDuplicatesHelper.PostProcessDuplicates(components, hashtable);
		hashtable.Clear();
	}

	private static void PostProcessPrintOn(StiPage page, bool isFirstPage, bool isLastPage)
	{
		PostProcessPrintOn(page.Components, isFirstPage, isLastPage);
	}

	private static void PostProcessPrintOn(StiComponentsCollection comps, bool isFirstPage, bool isLastPage)
	{
		int num = 0;
		while (num < comps.Count)
		{
			StiComponent stiComponent = comps[num];
			if (!AllowPrintOn(stiComponent, isFirstPage, isLastPage))
			{
				comps.RemoveAt(num);
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				PostProcessPrintOn(stiContainer.Components, isFirstPage, isLastPage);
			}
			num++;
		}
	}

	private static bool AllowPrintOn(StiComponent comp, bool isFirstPage, bool isLastPage)
	{
		if (comp.PrintOn == StiPrintOnType.AllPages)
		{
			return true;
		}
		if ((comp.PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages && isFirstPage)
		{
			return false;
		}
		if (comp.PrintOn == StiPrintOnType.OnlyFirstAndLastPage)
		{
			if (isFirstPage || isLastPage)
			{
				return true;
			}
			return false;
		}
		if ((comp.PrintOn & StiPrintOnType.OnlyFirstPage) > StiPrintOnType.AllPages && !isFirstPage)
		{
			return false;
		}
		if ((comp.PrintOn & StiPrintOnType.OnlyLastPage) > StiPrintOnType.AllPages && !isLastPage)
		{
			return false;
		}
		if ((comp.PrintOn & StiPrintOnType.ExceptLastPage) > StiPrintOnType.AllPages && isLastPage)
		{
			return false;
		}
		return true;
	}
}
