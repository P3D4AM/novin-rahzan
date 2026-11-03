using System;
using System.Collections;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components;

public class StiContainerHelper
{
	public const string NotCheckSizeMark = "*NotCheckSizeMark*";

	public static void CheckSize(StiComponent component)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Invalid comparison between Unknown and I4
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Invalid comparison between Unknown and I4
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected I4, but got Unknown
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Invalid comparison between Unknown and I4
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Expected I4, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Expected I4, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected I4, but got Unknown
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		if (!(component is StiContainer stiContainer) || (stiContainer.TagValue is string && (string)stiContainer.TagValue == "*NotCheckSizeMark*"))
		{
			return;
		}
		Hashtable hashtable = null;
		if (StiOptions.Engine.AllowCacheForGetActualSize)
		{
			StiReport report = component.Report;
			if (report == null && stiContainer?.ContainerInfoV2?.ParentBand != null)
			{
				report = stiContainer.ContainerInfoV2.ParentBand.Report;
			}
			if (report == null && stiContainer?.Parent?.ContainerInfoV2?.ParentBand != null)
			{
				report = stiContainer.Parent.ContainerInfoV2.ParentBand.Report;
			}
			if (report?.Engine != null)
			{
				if (report.Engine.HashCheckSize == null)
				{
					report.Engine.HashCheckSize = new Hashtable();
				}
				if (report.Engine.HashCheckSize.ContainsKey(component))
				{
					return;
				}
				hashtable = report.Engine.HashCheckSize;
			}
		}
		if (component.Report != null && (int)component.Report.EngineVersion == 1)
		{
			lock (((ICollection)stiContainer.Components).SyncRoot)
			{
				foreach (StiComponent component2 in stiContainer.Components)
				{
					if (component2.IsEnabled)
					{
						CheckSize(component2);
					}
				}
			}
		}
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		bool flag = false;
		lock (((ICollection)stiContainer.Components).SyncRoot)
		{
			foreach (StiComponent component3 in stiContainer.Components)
			{
				if (component3.IsEnabled)
				{
					SizeD actualSize;
					if (component3 is StiContainer)
					{
						double height = component3.Height;
						if (StiOptions.Engine.UseCheckSizeForContinuedContainers)
						{
							CheckSize(component3);
						}
						bool needSecondPass = false;
						actualSize = (component3 as StiContainer).GetActualSize(isFirstPass: true, ref needSecondPass);
						if (needSecondPass)
						{
							double height2 = component3.Height;
							if (component3.Height != ((SizeD)(ref actualSize)).Height)
							{
								component3.Height = ((SizeD)(ref actualSize)).Height;
							}
							actualSize = (component3 as StiContainer).GetActualSize(isFirstPass: false, ref needSecondPass);
							if (component3.Height != height2)
							{
								component3.Height = height2;
							}
						}
						if (StiOptions.Engine.UseCheckSizeForContinuedContainers && component3.Height != height)
						{
							component3.Height = height;
						}
					}
					else
					{
						actualSize = component3.GetActualSize();
					}
					if (!(component3 is StiHorizontalLinePrimitive))
					{
						((SizeD)(ref actualSize)).Height = Math.Round(((SizeD)(ref actualSize)).Height, 2);
					}
					((SizeD)(ref actualSize)).Width = Math.Round(((SizeD)(ref actualSize)).Width, 2);
					hashtable3[component3] = actualSize;
					if (((SizeD)(ref actualSize)).Height != component3.Height)
					{
						flag = true;
					}
				}
				else
				{
					hashtable3[component3] = (object)new SizeD(0.0, 0.0);
				}
			}
		}
		if (flag)
		{
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
			if (stiContainer is StiPage)
			{
				Hashtable hashtable4 = new Hashtable();
				foreach (StiComponent component4 in stiContainer.Components)
				{
					StiStartPointPrimitive stiStartPointPrimitive = component4 as StiStartPointPrimitive;
					if (!string.IsNullOrWhiteSpace(stiStartPointPrimitive?.ReferenceToGuid))
					{
						hashtable4[stiStartPointPrimitive.ReferenceToGuid] = null;
					}
				}
				if (hashtable4.Count > 0)
				{
					foreach (StiComponent component5 in stiContainer.Components)
					{
						if (!(component5 is StiCrossLinePrimitive) || !hashtable4.ContainsKey(component5.Guid))
						{
							stiComponentsCollection.Add(component5);
						}
					}
				}
				else
				{
					stiComponentsCollection.AddRange(stiContainer.Components);
				}
			}
			else
			{
				stiComponentsCollection.AddRange(stiContainer.Components);
			}
			stiComponentsCollection.SortByTopPosition();
			int num = 0;
			lock (((ICollection)stiComponentsCollection).SyncRoot)
			{
				foreach (StiComponent item in stiComponentsCollection)
				{
					SizeD val = (SizeD)hashtable3[item];
					double num2 = ((SizeD)(ref val)).Height - item.Height;
					decimal num3 = Math.Round((decimal)item.Left, 2);
					decimal num4 = Math.Round((decimal)item.Right, 2);
					decimal num5 = Math.Round((decimal)item.Bottom, 2);
					if (num2 != 0.0)
					{
						double num6 = 0.0;
						if (hashtable2[item] != null)
						{
							num6 = (double)hashtable2[item];
						}
						for (int i = num + 1; i < stiComponentsCollection.Count; i++)
						{
							StiComponent stiComponent6 = stiComponentsCollection[i];
							decimal num7 = Math.Round((decimal)stiComponent6.Left, 2);
							decimal num8 = Math.Round((decimal)stiComponent6.Top, 2);
							if (((stiComponent6.ShiftMode & StiShiftMode.OnlyInWidthOfComponent) == 0 || (!(num3 > num7) && !(num4 <= num7))) && num5 <= num8)
							{
								double val2 = 0.0;
								if (hashtable2[stiComponent6] != null)
								{
									val2 = (double)hashtable2[stiComponent6];
								}
								if (num2 > 0.0 && (stiComponent6.ShiftMode & StiShiftMode.IncreasingSize) != 0)
								{
									hashtable2[stiComponent6] = Math.Max(num2 + num6, val2);
								}
								if (num2 < 0.0 && (stiComponent6.ShiftMode & StiShiftMode.DecreasingSize) != 0)
								{
									hashtable2[stiComponent6] = Math.Min(num2 + num6, val2);
								}
							}
						}
					}
					num++;
				}
			}
		}
		lock (((ICollection)stiContainer.Components).SyncRoot)
		{
			foreach (StiComponent component6 in stiContainer.Components)
			{
				SizeD val3 = (SizeD)hashtable3[component6];
				if (component6 is IStiAutoWidth { AutoWidth: not false } stiAutoWidth)
				{
					StiHorAlignment val4 = (StiHorAlignment)1;
					if (stiAutoWidth is IStiTextHorAlignment { HorAlignment: var horAlignment })
					{
						switch ((int)horAlignment)
						{
						case 0:
							val4 = (StiHorAlignment)1;
							break;
						case 1:
						case 3:
							val4 = (StiHorAlignment)2;
							break;
						case 2:
							val4 = (StiHorAlignment)3;
							break;
						}
					}
					if (stiAutoWidth is IStiHorAlignment { HorAlignment: var horAlignment2 })
					{
						switch (horAlignment2 - 1)
						{
						case 0:
							val4 = (StiHorAlignment)1;
							break;
						case 1:
							val4 = (StiHorAlignment)2;
							break;
						case 2:
							val4 = (StiHorAlignment)3;
							break;
						}
					}
					float num9 = 0f;
					if (component6 is StiText)
					{
						num9 = (component6 as StiText).Angle;
					}
					if (num9 == 90f || num9 == 270f)
					{
						double top = component6.Top;
						switch (val4 - 1)
						{
						case 0:
						{
							double height4 = ((SizeD)(ref val3)).Height;
							component6.Top = component6.Bottom - height4;
							component6.Height = height4;
							break;
						}
						case 1:
						{
							double height3 = ((SizeD)(ref val3)).Height;
							component6.Top += (component6.Height - height3) / 2.0;
							component6.Height = height3;
							break;
						}
						case 2:
							component6.Height = ((SizeD)(ref val3)).Height;
							break;
						}
						if (top >= 0.0 && component6.Top < 0.0)
						{
							double height5 = component6.Height;
							component6.Top = 0.0;
							component6.Height = height5;
						}
					}
					else
					{
						double left = component6.Left;
						switch (val4 - 1)
						{
						case 0:
							component6.Width = ((SizeD)(ref val3)).Width;
							break;
						case 1:
						{
							double width2 = ((SizeD)(ref val3)).Width;
							component6.Left += (component6.Width - width2) / 2.0;
							component6.Width = width2;
							break;
						}
						case 2:
						{
							double width = ((SizeD)(ref val3)).Width;
							component6.Left = component6.Right - width;
							component6.Width = width;
							break;
						}
						}
						if (left >= 0.0 && component6.Left < 0.0)
						{
							double width3 = component6.Width;
							component6.Left = 0.0;
							component6.Width = width3;
						}
					}
				}
				if (component6.CanGrow)
				{
					double num10 = Math.Max(((SizeD)(ref val3)).Width, component6.Width);
					if (component6.Width != num10)
					{
						component6.Width = num10;
					}
					num10 = Math.Max(((SizeD)(ref val3)).Height, component6.Height);
					if (component6.Height != num10)
					{
						component6.Height = num10;
					}
				}
				if (component6.CanShrink)
				{
					if (component.Report != null && (int)component.Report.EngineVersion == 1)
					{
						component6.Width = Math.Min(((SizeD)(ref val3)).Width, component6.Width);
					}
					else if (!(component6 is StiContainer) || !((StiContainer)component6).ParentComponentIsBand)
					{
						component6.Width = Math.Min(((SizeD)(ref val3)).Width, component6.Width);
					}
					component6.Height = Math.Min(((SizeD)(ref val3)).Height, component6.Height);
				}
				CheckContainerGrowToHeight(component6, force: false);
				if (((component6.ShiftMode & StiShiftMode.IncreasingSize) != 0 || (component6.ShiftMode & StiShiftMode.DecreasingSize) != 0) && hashtable2[component6] != null)
				{
					component6.Top += (double)hashtable2[component6];
				}
			}
		}
		if (stiContainer.Report != null && (int)stiContainer.Report.EngineVersion == 1)
		{
			if (stiContainer.ParentComponentIsBand)
			{
				bool needSecondPass2 = false;
				SizeD actualSize2 = stiContainer.GetActualSize(isFirstPass: true, ref needSecondPass2);
				stiContainer.Width = ((SizeD)(ref actualSize2)).Width;
				stiContainer.Height = ((SizeD)(ref actualSize2)).Height;
				if (needSecondPass2)
				{
					actualSize2 = stiContainer.GetActualSize(isFirstPass: false, ref needSecondPass2);
					stiContainer.Width = ((SizeD)(ref actualSize2)).Width;
					stiContainer.Height = ((SizeD)(ref actualSize2)).Height;
				}
				CheckContainerGrowToHeight(stiContainer, needSecondPass2);
			}
		}
		else
		{
			if (ComponentPlacedOnBand(stiContainer) || stiContainer.ParentComponentIsBand)
			{
				bool needSecondPass3 = false;
				SizeD actualSize3 = stiContainer.GetActualSize(isFirstPass: true, ref needSecondPass3);
				if (!stiContainer.ParentComponentIsBand || stiContainer.ParentComponentIsCrossBand)
				{
					stiContainer.Width = ((SizeD)(ref actualSize3)).Width;
				}
				if (stiContainer.Height != ((SizeD)(ref actualSize3)).Height)
				{
					stiContainer.Height = ((SizeD)(ref actualSize3)).Height;
				}
				if (needSecondPass3)
				{
					actualSize3 = stiContainer.GetActualSize(isFirstPass: false, ref needSecondPass3);
					if (!stiContainer.ParentComponentIsBand || stiContainer.ParentComponentIsCrossBand)
					{
						stiContainer.Width = ((SizeD)(ref actualSize3)).Width;
					}
					if (stiContainer.Height != ((SizeD)(ref actualSize3)).Height)
					{
						stiContainer.Height = ((SizeD)(ref actualSize3)).Height;
					}
				}
				CheckContainerGrowToHeight(stiContainer, needSecondPass3);
			}
			if (stiContainer is StiPage && (stiContainer as StiPage).UnlimitedHeight)
			{
				StiPage stiPage = stiContainer as StiPage;
				bool needSecondPass4 = false;
				SizeD actualSize4 = stiContainer.GetActualSize(isFirstPass: true, ref needSecondPass4);
				while (((SizeD)(ref actualSize4)).Height > stiPage.Height)
				{
					stiPage.SegmentPerHeight++;
				}
			}
		}
		if (hashtable != null)
		{
			hashtable[component] = null;
		}
	}

	private static bool ComponentPlacedOnBand(StiComponent component)
	{
		StiContainer parent = component.Parent;
		while (true)
		{
			if (parent is StiBand)
			{
				return true;
			}
			if (parent is StiPage)
			{
				return false;
			}
			if (parent == null)
			{
				return false;
			}
			if (parent.ParentComponentIsBand)
			{
				break;
			}
			parent = parent.Parent;
		}
		return true;
	}

	public static void CheckContainerGrowToHeight(StiComponent component, bool force)
	{
		if (!(component is StiContainer stiContainer) || (stiContainer.TagValue is string && (string)stiContainer.TagValue == "*NotCheckSizeMark*"))
		{
			return;
		}
		StiComponentsCollection components = stiContainer.Components;
		lock (((ICollection)components).SyncRoot)
		{
			int count = components.Count;
			for (int i = 0; i < count; i++)
			{
				StiComponent stiComponent = components[i];
				if (stiComponent.GrowToHeight && stiComponent.IsEnabled)
				{
					double num = stiContainer.Height - stiComponent.Top;
					if (num > stiComponent.Height || force)
					{
						stiComponent.Height = num;
						CheckContainerGrowToHeight(stiComponent, force);
					}
				}
			}
		}
	}
}
