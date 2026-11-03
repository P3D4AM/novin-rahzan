using System;
using System.Collections;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Engine;

internal sealed class StiPostProcessDuplicatesHelper
{
	internal enum TypeOfDuplicates
	{
		Text,
		Image
	}

	private class StiMergeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((StiComponent)x).Top.CompareTo(((StiComponent)y).Top);
		}
	}

	internal static void PostProcessDuplicates(StiComponentsCollection comps, Hashtable parentCont)
	{
		PostProcessDuplicates(comps, parentCont, TypeOfDuplicates.Text);
		PostProcessDuplicates(comps, parentCont, TypeOfDuplicates.Image);
	}

	internal static void PostProcessDuplicates(StiComponentsCollection comps, Hashtable parentCont, TypeOfDuplicates typeOfDuplicates)
	{
		SortedList sortedList = new SortedList();
		bool flag = false;
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		lock (((ICollection)comps).SyncRoot)
		{
			foreach (StiComponent comp in comps)
			{
				if ((comp is IStiText && ((IStiText)comp).ProcessingDuplicates != 0 && typeOfDuplicates == TypeOfDuplicates.Text) || (comp is StiImage && ((StiImage)comp).ProcessingDuplicates != 0 && typeOfDuplicates == TypeOfDuplicates.Image))
				{
					flag = true;
					stiComponentsCollection.Add(comp);
					SortedList sortedList2 = sortedList[comp.Width] as SortedList;
					if (sortedList2 == null)
					{
						sortedList2 = new SortedList();
						sortedList[comp.Width] = sortedList2;
					}
					ArrayList arrayList = sortedList2[comp.Left] as ArrayList;
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						sortedList2[comp.Left] = arrayList;
					}
					arrayList.Add(comp);
				}
			}
		}
		if (!flag)
		{
			return;
		}
		foreach (double key in sortedList.Keys)
		{
			SortedList sortedList3 = sortedList[key] as SortedList;
			foreach (double key2 in sortedList3.Keys)
			{
				ArrayList arrayList2 = sortedList3[key2] as ArrayList;
				arrayList2.Sort(new StiMergeComparer());
				for (int i = 0; i < arrayList2.Count; i++)
				{
					StiComponent stiComponent2 = arrayList2[i] as StiComponent;
					StiContainer stiContainer = parentCont[stiComponent2] as StiContainer;
					double num3 = stiComponent2.Bottom;
					StiContainer stiContainer2;
					for (int num4 = i + 1; num4 < arrayList2.Count; stiContainer = stiContainer2)
					{
						StiComponent stiComponent3 = arrayList2[num4] as StiComponent;
						stiContainer2 = parentCont[stiComponent3] as StiContainer;
						StiProcessingDuplicatesType stiProcessingDuplicatesType = StiProcessingDuplicatesType.None;
						bool flag2 = false;
						if (typeOfDuplicates == TypeOfDuplicates.Text)
						{
							stiProcessingDuplicatesType = ((IStiText)stiComponent2).ProcessingDuplicates;
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnValueRemoveText)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.RemoveText;
							}
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnValueAndTagHide)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.BasedOnTagHide;
							}
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnValueAndTagMerge)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.BasedOnTagMerge;
							}
							flag2 = stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalHide || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalMerge || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalRemoveText || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueRemoveText || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagHide || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagMerge;
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueRemoveText)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.BasedOnTagRemoveText;
							}
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagHide)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.BasedOnTagHide;
							}
							if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagMerge)
							{
								stiProcessingDuplicatesType = StiProcessingDuplicatesType.BasedOnTagMerge;
							}
						}
						else
						{
							flag2 = ((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.GlobalHide || ((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.GlobalMerge || ((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.GlobalRemoveImage;
						}
						if (stiComponent3.Name == stiComponent2.Name || flag2)
						{
							if (Math.Round(stiContainer.Bottom, 2) != Math.Round(stiContainer2.Top, 2) || (!(stiContainer.Name == stiContainer2.Name || flag2) && !(stiContainer.Name == "Continued") && (stiContainer.Name == null || !stiContainer.Name.StartsWith("Continued_")) && !(stiContainer2.Name == "Breaked") && (stiContainer2.Name == null || !stiContainer2.Name.StartsWith("Breaked_"))))
							{
								break;
							}
							switch (typeOfDuplicates)
							{
							case TypeOfDuplicates.Text:
							{
								string text = ((IStiText)stiComponent3).GetTextInternal();
								string text2 = ((IStiText)stiComponent2).GetTextInternal();
								if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnTagHide || stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnTagMerge || stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnTagRemoveText)
								{
									text = ((stiComponent3.TagValue == null) ? string.Empty : stiComponent3.TagValue.ToString());
									text2 = ((stiComponent2.TagValue == null) ? string.Empty : stiComponent2.TagValue.ToString());
								}
								if (Math.Round(stiComponent3.Top, 2) >= Math.Round(num3, 2) && text2 == text)
								{
									if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.Merge || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalMerge || stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnTagMerge)
									{
										stiComponent2.Height += stiComponent3.Bottom - stiComponent2.Bottom;
									}
									if (stiProcessingDuplicatesType == StiProcessingDuplicatesType.RemoveText || stiProcessingDuplicatesType == StiProcessingDuplicatesType.GlobalRemoveText || stiProcessingDuplicatesType == StiProcessingDuplicatesType.BasedOnTagRemoveText)
									{
										((IStiText)stiComponent3).SetTextInternal(string.Empty);
										i++;
										num4++;
									}
									else
									{
										num3 += stiComponent3.Height;
										stiComponent3.Parent.Components.Remove(stiComponent3);
										arrayList2.RemoveAt(num4);
									}
									continue;
								}
								break;
							}
							case TypeOfDuplicates.Image:
								if (Math.Round(stiComponent3.Top, 2) >= Math.Round(num3, 2) && StiImageHelper.IsEqualImages(((StiImage)stiComponent3).TakeImageToDraw(), ((StiImage)stiComponent2).TakeImageToDraw()))
								{
									if (((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.Merge || ((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.GlobalMerge)
									{
										stiComponent2.Height += stiComponent3.Bottom - stiComponent2.Bottom;
									}
									if (((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.RemoveImage || ((StiImage)stiComponent2).ProcessingDuplicates == StiImageProcessingDuplicatesType.GlobalRemoveImage)
									{
										((StiImage)stiComponent3).ResetImageToDraw();
										i++;
										num4++;
									}
									else
									{
										num3 += stiComponent3.Height;
										stiComponent3.Parent.Components.Remove(stiComponent3);
										arrayList2.RemoveAt(num4);
									}
									continue;
								}
								break;
							default:
								continue;
							}
							break;
						}
						num4++;
					}
				}
			}
		}
	}
}
