using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

[StiToolbox(false)]
public class StiGroup : StiContainer
{
	[Browsable(false)]
	public sealed override Type ServiceType => null;

	public override object Clone()
	{
		StiGroup stiGroup = (StiGroup)MemberwiseClone();
		stiGroup.Components.Clear();
		foreach (StiComponent component in Components)
		{
			stiGroup.Components.Add((StiComponent)component.Clone());
		}
		return stiGroup;
	}

	public string ToString(string application)
	{
		StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		stiSerializing.Serialize(this, stringWriter, application, StiSerializeTypes.SerializeToDesigner);
		stringWriter.Close();
		return stringBuilder.ToString();
	}

	public static StiGroup CreateFromString(string text, string application)
	{
		StringReader stringReader = new StringReader(text);
		StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
		StiGroup stiGroup = new StiGroup();
		stiSerializing.Deserialize(stiGroup, stringReader, application);
		stringReader.Close();
		return stiGroup;
	}

	public static StiComponentsCollection GetSelectedComponents(bool isSelectedFinded, int level, StiContainer cont, StiComponentsCollection allComps, Hashtable lists)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component in cont.Components)
		{
			StiPointPrimitive stiPointPrimitive = component as StiPointPrimitive;
			bool flag = isSelectedFinded;
			bool flag2 = false;
			if (level == 0 && component.IsSelected)
			{
				stiComponentsCollection.Add(component);
				allComps.Add(component);
				flag = true;
			}
			if (level != 0)
			{
				allComps.Add(component);
				flag2 = true;
			}
			if (flag2 && stiPointPrimitive != null && stiPointPrimitive.ReferenceToGuid != null)
			{
				lists[stiPointPrimitive.ReferenceToGuid] = component;
			}
			if (component is StiContainer cont2)
			{
				int num = level;
				if (flag)
				{
					level++;
				}
				stiComponentsCollection.AddRange(GetSelectedComponents(flag, level, cont2, allComps, lists));
				level = num;
			}
		}
		return stiComponentsCollection;
	}

	public static StiGroup GetGroupFromPage(StiPage page)
	{
		StiPage stiPage = (StiPage)page.Clone();
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		Hashtable hashtable = new Hashtable();
		StiComponentsCollection selectedComponents = GetSelectedComponents(isSelectedFinded: false, 0, stiPage, stiComponentsCollection, hashtable);
		if (hashtable.Count > 0)
		{
			Hashtable hashtable2 = new Hashtable();
			foreach (StiComponent item in selectedComponents)
			{
				if (item is StiCrossLinePrimitive)
				{
					StiCrossLinePrimitive stiCrossLinePrimitive = item as StiCrossLinePrimitive;
					hashtable2[stiCrossLinePrimitive.Guid] = stiCrossLinePrimitive;
				}
			}
			foreach (StiComponent component2 in stiPage.Components)
			{
				if (component2 is StiCrossLinePrimitive stiCrossLinePrimitive2 && hashtable[stiCrossLinePrimitive2.Guid] != null && hashtable2[stiCrossLinePrimitive2.Guid] == null)
				{
					selectedComponents.Add(stiCrossLinePrimitive2);
					stiComponentsCollection.Add(stiCrossLinePrimitive2);
					hashtable2[stiCrossLinePrimitive2.Guid] = stiCrossLinePrimitive2;
				}
			}
		}
		foreach (StiComponent item2 in selectedComponents)
		{
			if (item2.Parent != item2.Page)
			{
				item2.DisplayRectangle = item2.ComponentToPage(item2.DisplayRectangle);
			}
		}
		StiGroup stiGroup = new StiGroup();
		StiHundredthsOfInchUnit newUnit = new StiHundredthsOfInchUnit();
		foreach (StiComponent item3 in selectedComponents)
		{
			if (item3 is StiContainer cont)
			{
				ResetSelection(cont);
			}
		}
		foreach (StiComponent item4 in stiComponentsCollection)
		{
			if (!(item4 is StiCrossLinePrimitive))
			{
				if (!stiComponentsCollection.Contains(item4.Parent))
				{
					item4.Linked = false;
				}
				item4.Page = null;
				item4.Parent = null;
				if (item4 is StiContainer)
				{
					((StiContainer)item4).Convert(page.Report.GetCurrentPage().Unit, newUnit, isReportSnapshot: false, convertComponents: false);
				}
				else
				{
					item4.Convert(page.Report.GetCurrentPage().Unit, newUnit);
				}
			}
		}
		foreach (StiComponent item5 in selectedComponents)
		{
			stiGroup.Components.Add(item5);
		}
		return stiGroup;
	}

	private static void ResetSelection(StiContainer cont)
	{
		foreach (StiComponent component in cont.Components)
		{
			component.Reset();
			if (component is StiContainer cont2)
			{
				ResetSelection(cont2);
			}
		}
	}

	private static List<StiComponent> GetAllComps(List<StiComponent> comps)
	{
		List<StiComponent> list = new List<StiComponent>();
		foreach (StiComponent comp in comps)
		{
			list.Add(comp);
			if (comp is StiContainer stiContainer)
			{
				list.AddRange(GetAllComps(stiContainer.Components.ToList()));
			}
		}
		return list;
	}

	public StiComponentsCollection InsertIntoPage(StiPage page)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		StiUnit oldUnit = new StiHundredthsOfInchUnit();
		page.ResetSelection();
		List<StiComponent> list = StiComponentsByPageTypeFiltrator.Filter(page, GetAllComps(Components.ToList()));
		foreach (StiComponent item in list)
		{
			IStiComponentGuid stiComponentGuid = item;
			if (stiComponentGuid == null || string.IsNullOrEmpty(stiComponentGuid.Guid))
			{
				continue;
			}
			string guid = stiComponentGuid.Guid;
			stiComponentGuid.NewGuid();
			foreach (StiComponent item2 in list)
			{
				if (item2 is IStiComponentGuidReference stiComponentGuidReference && stiComponentGuidReference.ReferenceToGuid == guid)
				{
					stiComponentGuidReference.ReferenceToGuid = stiComponentGuid.Guid;
				}
				if (item2 is StiCrossHeader stiCrossHeader && stiCrossHeader.TotalGuid == guid)
				{
					stiCrossHeader.TotalGuid = stiComponentGuid.Guid;
				}
			}
		}
		StiComponentsCollection stiComponentsCollection2 = page.Report.GetComponents();
		Hashtable hashtable = new Hashtable();
		foreach (StiComponent item3 in stiComponentsCollection2)
		{
			hashtable[item3.Name] = item3;
		}
		foreach (StiComponent item4 in list)
		{
			if (!(item4 is StiCrossLinePrimitive))
			{
				if (item4 is StiContainer)
				{
					((StiContainer)item4).Convert(oldUnit, page.Unit, isReportSnapshot: false, convertComponents: false);
				}
				else
				{
					item4.Convert(oldUnit, page.Unit);
				}
			}
			item4.Page = page;
			if (Components.IndexOf(item4) != -1)
			{
				if (!(item4 is StiPointPrimitive))
				{
					page.Components.Add(item4);
				}
				item4.Select();
			}
			if (hashtable[item4.Name] != null)
			{
				item4.Name = StiNameCreation.CreateName(page.Report, StiNameCreation.GenerateName(page.Report, item4.LocalizedName, item4.GetType()));
				if (item4 is StiGauge)
				{
					StiGaugeHelper.CheckGaugeName(item4 as StiGauge);
				}
			}
			if (item4 is StiContainer stiContainer)
			{
				stiContainer.Components.SetParent(stiContainer);
			}
			if (!(item4 is StiPointPrimitive))
			{
				stiComponentsCollection.Add(item4);
			}
		}
		return stiComponentsCollection;
	}

	public StiGroup()
	{
		Components.Clear();
	}
}
