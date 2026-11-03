using System;
using System.Collections;
using System.Security.Cryptography;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Engine;

internal sealed class StiPostProcessProviderV1
{
	internal static void PostProcessPages(StiPagesCollection pages)
	{
		foreach (StiPage page in pages)
		{
			PostProcessPage(page, page.PageInfoV1.PageNumber == 1, page.PageInfoV1.PageNumber == ((page.PageInfoV1.TotalPageCount == -1) ? page.Report.TotalPageCount : page.PageInfoV1.TotalPageCount));
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
		StiPagesCollection stiPagesCollection = new StiPagesCollection(null);
		stiPagesCollection.Add(page);
		PostProcessPrimitives(stiPagesCollection);
	}

	internal static void PostProcessPrimitives(StiPagesCollection pages)
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		ArrayList arrayList = new ArrayList();
		foreach (StiPage page in pages)
		{
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
			stiComponentsCollection.AddRange(page.Components);
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item is StiCrossLinePrimitive stiCrossLinePrimitive)
				{
					hashtable2[stiCrossLinePrimitive.Guid] = stiCrossLinePrimitive;
					stiCrossLinePrimitive.Parent.Components.Remove(item);
				}
			}
			if (hashtable2.Count <= 0)
			{
				continue;
			}
			stiComponentsCollection = page.GetComponents();
			arrayList.Clear();
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
					ArrayList arrayList2 = hashtable[stiStartPointPrimitive.ReferenceToGuid] as ArrayList;
					if (arrayList2 == null)
					{
						arrayList2 = new ArrayList();
						hashtable[stiStartPointPrimitive.ReferenceToGuid] = arrayList2;
						if (page.Columns > 1)
						{
							arrayList2.Add(stiStartPointPrimitive);
						}
					}
					if (page.Columns < 2)
					{
						arrayList2.Add(stiStartPointPrimitive);
					}
				}
				else if (item2 is StiEndPointPrimitive stiEndPointPrimitive && !ProcessOneEndPoint(hashtable, hashtable2, page, pages, stiEndPointPrimitive))
				{
					arrayList.Add(stiEndPointPrimitive);
				}
			}
			foreach (StiEndPointPrimitive item3 in arrayList)
			{
				ProcessOneEndPoint(hashtable, hashtable2, page, pages, item3);
			}
		}
		foreach (StiPage page2 in pages)
		{
			RemoveAllPointPrimitives(page2);
		}
	}

	private static bool ProcessOneEndPoint(Hashtable startPointsHash, Hashtable lines, StiPage page, StiPagesCollection pages, StiEndPointPrimitive endPoint)
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
		int num = 0;
		foreach (StiStartPointPrimitive item in arrayList)
		{
			StiStartPointPrimitive nextStartPoint = null;
			if (num + 1 < arrayList.Count)
			{
				nextStartPoint = arrayList[num + 1] as StiStartPointPrimitive;
			}
			ProcessOnePrimitive(pages, page, crossLine, item, nextStartPoint, endPoint);
			num++;
		}
		return true;
	}

	private static void ProcessOnePrimitive(StiPagesCollection pages, StiPage page, StiCrossLinePrimitive crossLine, StiPrimitive startPoint, StiPrimitive nextStartPoint, StiPrimitive endPoint)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		PointD startPos = ((StiComponent)startPoint).ComponentToPage(new PointD(startPoint.Left, startPoint.Top));
		PointD endPos = ((StiComponent)endPoint).ComponentToPage(new PointD(endPoint.Left, endPoint.Top));
		int storedColumn = ((StiPointPrimitive)startPoint).StoredColumn;
		int storedColumn2 = ((StiPointPrimitive)endPoint).StoredColumn;
		storedColumn = Math.Max(1, storedColumn);
		storedColumn2 = Math.Max(1, storedColumn2);
		if (startPoint.Page != endPoint.Page || storedColumn != storedColumn2)
		{
			int num = pages.IndexOf(startPoint.Page);
			int num2 = pages.IndexOf(endPoint.Page);
			int num3 = -1;
			if (nextStartPoint != null)
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
					double topPageContentPosition = stiPage.PageInfoV1.TopPageContentPosition;
					double bottomPageContentPosition = stiPage.PageInfoV1.BottomPageContentPosition;
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
						AddPrimitive(crossLine, new PointD(num4 + x, y), new PointD(num4 + num6, bottomPageContentPosition), stiPage);
					}
					else if (i == num2 && storedColumn2 == j)
					{
						AddPrimitive(crossLine, new PointD(num4 + x, topPageContentPosition), new PointD(num4 + num6, y2), stiPage);
					}
					else
					{
						AddPrimitive(crossLine, new PointD(num4 + x, topPageContentPosition), new PointD(num4 + num6, bottomPageContentPosition), stiPage);
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

	internal static void AddPrimitive(StiCrossLinePrimitive crossLine, PointD startPos, PointD endPos, StiPage page)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		if (crossLine is StiRoundedRectanglePrimitive)
		{
			StiCrossLinePrimitive stiCrossLinePrimitive = ((StiService)crossLine).Clone() as StiCrossLinePrimitive;
			stiCrossLinePrimitive.Guid = null;
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
				StiVerticalLinePrimitive component = new StiVerticalLinePrimitive
				{
					Name = crossLine.Name + "_Left",
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				};
				page.Components.Add(component);
			}
			if (((StiRectanglePrimitive)crossLine).RightSide)
			{
				StiVerticalLinePrimitive component2 = new StiVerticalLinePrimitive
				{
					Name = crossLine.Name + "_Right",
					Left = ((PointD)(ref endPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = ((PointD)(ref endPos)).Y - ((PointD)(ref startPos)).Y,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				};
				page.Components.Add(component2);
			}
			if (((StiRectanglePrimitive)crossLine).TopSide)
			{
				StiHorizontalLinePrimitive component3 = new StiHorizontalLinePrimitive
				{
					Name = crossLine.Name + "_Top",
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref startPos)).Y,
					Height = crossLine.Page.Unit.ConvertFromHInches(1.0),
					Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				};
				page.Components.Add(component3);
			}
			if (((StiRectanglePrimitive)crossLine).BottomSide)
			{
				StiHorizontalLinePrimitive component4 = new StiHorizontalLinePrimitive
				{
					Name = crossLine.Name + "_Bottom",
					Left = ((PointD)(ref startPos)).X,
					Top = ((PointD)(ref endPos)).Y,
					Height = crossLine.Page.Unit.ConvertFromHInches(1.0),
					Width = ((PointD)(ref endPos)).X - ((PointD)(ref startPos)).X,
					Color = crossLine.Color,
					Style = crossLine.Style,
					Size = crossLine.Size
				};
				page.Components.Add(component4);
			}
		}
		else
		{
			StiCrossLinePrimitive stiCrossLinePrimitive2 = ((StiService)crossLine).Clone() as StiCrossLinePrimitive;
			stiCrossLinePrimitive2.Guid = null;
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
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
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
		page.ClearPage();
		if (isFirstPage || isLastPage)
		{
			StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
			bool flag = !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
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
				double num = page.Report.Unit.ConvertFromHInches(200.0);
				double num2 = page.Report.Unit.ConvertFromHInches(52.0);
				RectangleD rect = default(RectangleD);
				((RectangleD)(ref rect))._002Ector((page.Width - num) / 2.0, page.Height - num2, num, num2);
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
