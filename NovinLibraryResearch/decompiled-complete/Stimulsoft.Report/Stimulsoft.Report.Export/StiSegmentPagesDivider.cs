using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public sealed class StiSegmentPagesDivider
{
	public static StiPagesCollection Divide(StiPagesCollection pages)
	{
		return Divide(pages, null);
	}

	public static StiPagesCollection Divide(StiPagesCollection pages, StiExportService service)
	{
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected I4, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		foreach (StiPage page in pages)
		{
			if (page.SegmentPerWidth > 1 || page.SegmentPerHeight > 1)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return pages;
		}
		if (service != null)
		{
			service.StatusString = StiLocalization.Get("Report", "PreparingReport");
		}
		StiPagesCollection stiPagesCollection = new StiPagesCollection(pages.Report, pages);
		stiPagesCollection.CacheMode = pages.CacheMode;
		if (stiPagesCollection.CacheMode)
		{
			stiPagesCollection.CanUseCacheMode = true;
		}
		RectangleM val2 = default(RectangleM);
		foreach (StiPage page2 in pages)
		{
			pages.GetPage(page2);
			service?.InvokeExporting(page2, pages, service.CurrentPassNumber, service.MaximumPassNumber);
			if (page2.SegmentPerWidth > 1 || page2.SegmentPerHeight > 1)
			{
				List<StiComponent>[,] array = new List<StiComponent>[page2.SegmentPerHeight, page2.SegmentPerWidth];
				for (int i = 0; i < page2.SegmentPerHeight; i++)
				{
					for (int j = 0; j < page2.SegmentPerWidth; j++)
					{
						array[i, j] = new List<StiComponent>();
					}
				}
				List<int>[] array2 = new List<int>[page2.SegmentPerHeight];
				for (int k = 0; k < page2.SegmentPerHeight; k++)
				{
					array2[k] = new List<int>();
				}
				decimal num = (decimal)(page2.PageWidth - page2.Margins.Left - page2.Margins.Right);
				decimal num2 = (decimal)(page2.PageHeight - page2.Margins.Top - page2.Margins.Bottom);
				for (int l = 0; l < page2.Components.Count; l++)
				{
					StiComponent stiComponent = page2.Components[l];
					if (!stiComponent.Enabled)
					{
						continue;
					}
					int num3 = (int)((decimal)stiComponent.Top / num2);
					int num4 = (int)((decimal)stiComponent.Bottom / num2);
					if (num4 > array2.Length - 1)
					{
						num4 = array2.Length - 1;
					}
					for (int m = num3; m <= num4; m++)
					{
						array2[m].Add(l);
					}
					if (!(stiComponent is StiText { ExceedMargins: not StiExceedMargins.None } stiText))
					{
						continue;
					}
					int num5 = (int)((decimal)stiComponent.Left / num);
					int num6 = num5;
					int num7 = num5;
					int num8 = num3;
					int num9 = num3;
					if ((stiText.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
					{
						num6 = 0;
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
					{
						num7 = page2.SegmentPerWidth - 1;
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
					{
						num8 = 0;
					}
					if ((stiText.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
					{
						num9 = page2.SegmentPerHeight - 1;
					}
					if (num6 == num7 && num8 == num9)
					{
						continue;
					}
					for (int n = num8; n <= num9; n++)
					{
						for (int num10 = num6; num10 <= num7; num10++)
						{
							if (n != num3 || num10 != num5)
							{
								StiText stiText2 = ((StiService)stiText).Clone() as StiText;
								stiText2.Text = string.Empty;
								stiText2.Border.Side = (StiBorderSides)0;
								while ((decimal)stiText2.Left > num)
								{
									stiText2.Left -= (double)num;
								}
								while ((decimal)stiText2.Top > num2)
								{
									stiText2.Top -= (double)num2;
								}
								if (num10 != num5)
								{
									stiText2.ExceedMargins |= StiExceedMargins.Left | StiExceedMargins.Right;
								}
								if (n != num3)
								{
									stiText2.ExceedMargins |= StiExceedMargins.Top | StiExceedMargins.Bottom;
								}
								array[n, num10].Add(stiText2);
							}
						}
					}
				}
				StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
				stiComponentsCollection.AddRange(page2.Components);
				for (int num11 = 0; num11 < page2.SegmentPerHeight; num11++)
				{
					for (int num12 = 0; num12 < page2.SegmentPerWidth; num12++)
					{
						StiPage stiPage3 = page2.Clone(cloneProperties: false, cloneComponents: false) as StiPage;
						stiPage3.CacheGuid = StiGuidUtils.NewGuid();
						stiPage3.SegmentPerWidth = 1;
						stiPage3.SegmentPerHeight = 1;
						if (service is StiPdfExportService)
						{
							StiBorderSides val = (StiBorderSides)0;
							if (num12 > 0)
							{
								val = (StiBorderSides)(val | 2);
							}
							if (num12 < page2.SegmentPerWidth - 1)
							{
								val = (StiBorderSides)(val | 4);
							}
							if (num11 > 0)
							{
								val = (StiBorderSides)(val | 1);
							}
							if (num11 < page2.SegmentPerHeight - 1)
							{
								val = (StiBorderSides)(val | 8);
							}
							stiPage3.TagValue = $"Segments:{(int)val}";
						}
						((RectangleM)(ref val2))._002Ector((decimal)((double)num12 * stiPage3.Width), (decimal)((double)num11 * stiPage3.Height), (decimal)stiPage3.Width, (decimal)stiPage3.Height);
						RectangleD val3 = ((RectangleM)(ref val2)).ToRectangleD();
						stiPage3.Components.AddRange(array[num11, num12].ToArray());
						List<int> list = array2[num11];
						foreach (int item in list)
						{
							StiComponent stiComponent2 = stiComponentsCollection[item];
							if (!stiComponent2.Enabled)
							{
								continue;
							}
							if (((RectangleM)(ref val2)).Left <= (decimal)stiComponent2.Left && (decimal)stiComponent2.Left < ((RectangleM)(ref val2)).Right && ((RectangleM)(ref val2)).Top <= (decimal)stiComponent2.Top && (decimal)stiComponent2.Top < ((RectangleM)(ref val2)).Bottom)
							{
								bool flag2 = stiComponent2.Right > ((double)num12 + 1.5) * stiPage3.Width;
								StiComponent stiComponent3 = ((StiService)stiComponent2).Clone() as StiComponent;
								stiComponent3.Left -= ((RectangleD)(ref val3)).Left;
								stiComponent3.Top -= ((RectangleD)(ref val3)).Top;
								if (flag2)
								{
									stiComponent3.Width = stiPage3.Width * 1.5 - stiComponent3.Left;
								}
								stiPage3.Components.Add(stiComponent3);
							}
							else
							{
								if (!((decimal)stiComponent2.Left < ((RectangleM)(ref val2)).Right) || !((decimal)stiComponent2.Right > ((RectangleM)(ref val2)).Left) || !((decimal)stiComponent2.Top < ((RectangleM)(ref val2)).Bottom) || !((decimal)stiComponent2.Bottom > ((RectangleM)(ref val2)).Top))
								{
									continue;
								}
								bool flag3 = !(service is StiPdfExportService);
								StiComponent stiComponent4 = null;
								if (stiComponent2 is StiContainer)
								{
									stiComponent4 = ((StiService)stiComponent2).Clone() as StiContainer;
								}
								if (stiComponent2 is StiText)
								{
									StiText stiText3 = ((StiService)stiComponent2).Clone() as StiText;
									if (flag3)
									{
										stiText3.Text = string.Empty;
									}
									stiComponent4 = stiText3;
								}
								if (stiComponent2 is StiImage)
								{
									StiContainer stiContainer = new StiContainer();
									stiContainer.Border = (stiComponent2 as StiImage).Border;
									stiContainer.Brush = (stiComponent2 as StiImage).Brush;
									StiContainer stiContainer2 = stiContainer;
									stiComponent4 = stiContainer2;
								}
								if (stiComponent2 is StiCrossLinePrimitive || stiComponent2 is StiHorizontalLinePrimitive)
								{
									stiComponent4 = ((StiService)stiComponent2).Clone() as StiComponent;
								}
								if (stiComponent4 != null)
								{
									StiBorder val4 = new StiBorder();
									if (stiComponent4 is IStiBorder)
									{
										val4 = ((IStiBorder)stiComponent4).Border;
									}
									if ((decimal)stiComponent2.Left < ((RectangleM)(ref val2)).Left && flag3)
									{
										stiComponent4.Left = 0.0;
										StiBorder obj = val4;
										obj.Side = (StiBorderSides)(obj.Side & 0xD);
									}
									else
									{
										stiComponent4.Left -= ((RectangleD)(ref val3)).Left;
									}
									if ((decimal)stiComponent2.Right > ((RectangleM)(ref val2)).Right && flag3)
									{
										stiComponent4.Width = stiPage3.Width - stiComponent4.Left;
										StiBorder obj2 = val4;
										obj2.Side = (StiBorderSides)(obj2.Side & 0xB);
									}
									else
									{
										stiComponent4.Width = stiComponent2.Right - (((RectangleD)(ref val3)).Left + stiComponent4.Left);
									}
									if ((decimal)stiComponent2.Top < ((RectangleM)(ref val2)).Top && flag3)
									{
										stiComponent4.Top = 0.0;
										StiBorder obj3 = val4;
										obj3.Side = (StiBorderSides)(obj3.Side & 0xE);
									}
									else
									{
										stiComponent4.Top -= ((RectangleD)(ref val3)).Top;
									}
									if ((decimal)stiComponent2.Bottom > ((RectangleM)(ref val2)).Bottom && flag3)
									{
										stiComponent4.Height = stiPage3.Height - stiComponent4.Top;
										StiBorder obj4 = val4;
										obj4.Side = (StiBorderSides)(obj4.Side & 7);
									}
									else
									{
										stiComponent4.Height = stiComponent2.Bottom - (((RectangleD)(ref val3)).Top + stiComponent4.Top);
									}
									stiPage3.Components.Add(stiComponent4);
								}
							}
						}
						stiPagesCollection.AddV2Internal(stiPage3);
					}
				}
			}
			else
			{
				stiPagesCollection.CanUseCacheMode = false;
				stiPagesCollection.AddV2Internal(page2);
				stiPagesCollection.CanUseCacheMode = stiPagesCollection.CacheMode;
			}
		}
		stiPagesCollection.CanUseCacheMode = false;
		return stiPagesCollection;
	}
}
