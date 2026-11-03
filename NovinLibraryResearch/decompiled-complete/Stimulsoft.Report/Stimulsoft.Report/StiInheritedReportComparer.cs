using System.Collections;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

internal static class StiInheritedReportComparer
{
	internal static void Compare(StiReport report, StiReport masterReport)
	{
		CompareComponents(report, masterReport);
		CompareVariables(report, masterReport);
		CompareDataSources(report, masterReport);
		CompareRelations(report, masterReport);
		CompareBusinessObjects(report, masterReport);
		CompareDatabases(report, masterReport);
		CompareEvents(report, masterReport);
	}

	private static void CompareComponents(StiReport report, StiReport masterReport)
	{
		StiComponentsCollection components = report.GetComponents();
		StiComponentsCollection components2 = masterReport.GetComponents();
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiComponent item in components2)
		{
			hashtable2[item.Name] = item;
		}
		foreach (StiComponent item2 in components)
		{
			if (!item2.Inherited && !(item2 is StiPage))
			{
				continue;
			}
			if (!(hashtable2[item2.Name] is StiComponent stiComponent3))
			{
				if (!(item2 is StiPage))
				{
					item2.Parent.Components.Remove(item2);
				}
				else if (((StiPage)item2).Components.Count == 0)
				{
					report.Pages.Remove(item2 as StiPage);
				}
				continue;
			}
			StiComponent stiComponent4 = null;
			stiComponent4 = ((!(stiComponent3 is StiContainer stiContainer)) ? (stiComponent3.Clone(cloneProperties: true) as StiComponent) : (stiContainer.Clone(cloneProperties: true, cloneComponents: false) as StiComponent));
			hashtable[item2.Name] = stiComponent4;
			if (item2 is StiPage)
			{
				int index = report.Pages.IndexOf(item2 as StiPage);
				report.Pages.RemoveAt(index);
				report.Pages.Insert(index, stiComponent4 as StiPage);
			}
			else
			{
				int index2 = item2.Parent.Components.IndexOf(item2);
				item2.Parent.Components.RemoveAt(index2);
				item2.Parent.Components.Insert(index2, stiComponent4);
			}
			if (!(item2 is StiContainer stiContainer2))
			{
				continue;
			}
			foreach (StiComponent component in stiContainer2.Components)
			{
				((StiContainer)stiComponent4).Components.Add(component);
			}
		}
		foreach (StiComponent item3 in components2)
		{
			if (hashtable[item3.Name] != null)
			{
				continue;
			}
			StiComponent stiComponent6 = null;
			stiComponent6 = ((!(item3 is StiContainer stiContainer3)) ? (item3.Clone(cloneProperties: true) as StiComponent) : (stiContainer3.Clone(cloneProperties: true, cloneComponents: false) as StiComponent));
			if (item3 is StiPage)
			{
				report.Pages.Add(stiComponent6 as StiPage);
			}
			else
			{
				StiContainer stiContainer4 = hashtable[item3.Parent.Name] as StiContainer;
				int positionForComponent = GetPositionForComponent(item3, stiContainer4);
				if (positionForComponent >= 0 && positionForComponent < stiContainer4.Components.Count)
				{
					stiContainer4.Components.Insert(positionForComponent, stiComponent6);
				}
				else
				{
					stiContainer4.Components.Add(stiComponent6);
				}
			}
			hashtable[stiComponent6.Name] = stiComponent6;
		}
		components = report.GetComponents();
		foreach (StiComponent item4 in components)
		{
			hashtable[item4.Name] = item4;
		}
		foreach (StiComponent item5 in components)
		{
			if (item5 is IStiMasterComponent { MasterComponent: not null } stiMasterComponent)
			{
				string name = stiMasterComponent.MasterComponent.Name;
				if (hashtable[name] is StiComponent masterComponent)
				{
					stiMasterComponent.MasterComponent = masterComponent;
				}
			}
		}
		foreach (StiPage page in report.Pages)
		{
			page.Report = report;
			SetParent(page, page, page);
			page.SortByPriority();
		}
	}

	private static int GetPositionForComponent(StiComponent comp, StiContainer contParent)
	{
		StiContainer parent = comp.Parent;
		for (int num = parent.Components.IndexOf(comp); num > 0; num--)
		{
			StiComponent stiComponent = parent.Components[num - 1];
			int num2 = contParent.Components.IndexOf(stiComponent.Name);
			if (num2 != -1)
			{
				return num2 + 1;
			}
		}
		for (int num = parent.Components.IndexOf(comp); num < parent.Components.Count - 1; num++)
		{
			StiComponent stiComponent2 = parent.Components[num + 1];
			int num3 = contParent.Components.IndexOf(stiComponent2.Name);
			if (num3 != -1)
			{
				return num3;
			}
		}
		return 0;
	}

	private static void SetParent(StiComponent component, StiPage page, StiContainer parent)
	{
		component.Page = page;
		parent = component as StiContainer;
		if (parent == null)
		{
			return;
		}
		foreach (StiComponent component2 in parent.Components)
		{
			SetParent(component2, page, parent);
		}
	}

	private static void CompareVariables(StiReport report, StiReport masterReport)
	{
		StiVariablesCollection stiVariablesCollection = new StiVariablesCollection();
		foreach (StiVariable variable3 in report.Dictionary.Variables)
		{
			stiVariablesCollection.Add(variable3);
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiVariable variable4 in masterReport.Dictionary.Variables)
		{
			hashtable2[variable4.Name] = variable4;
		}
		foreach (StiVariable item in stiVariablesCollection)
		{
			if (item.Inherited)
			{
				if (!(hashtable2[item.Name] is StiVariable stiVariable3))
				{
					report.Dictionary.Variables.Remove(item);
					continue;
				}
				hashtable[item.Name] = item;
				StiVariable variable2 = stiVariable3.Clone() as StiVariable;
				int index = report.Dictionary.Variables.IndexOf(item);
				report.Dictionary.Variables.RemoveAt(index);
				report.Dictionary.Variables.Insert(index, variable2);
			}
		}
		foreach (StiVariable variable5 in masterReport.Dictionary.Variables)
		{
			if (hashtable[variable5.Name] == null)
			{
				StiVariable stiVariable5 = variable5.Clone() as StiVariable;
				int num = masterReport.Dictionary.Variables.IndexOf(variable5);
				if (num >= 0 && num < report.Dictionary.Variables.Count)
				{
					report.Dictionary.Variables.Insert(0, stiVariable5);
				}
				else
				{
					report.Dictionary.Variables.Add(stiVariable5);
				}
				hashtable[stiVariable5.Name] = stiVariable5;
			}
		}
	}

	private static void CompareDataSources(StiReport report, StiReport masterReport)
	{
		StiDataSourcesCollection stiDataSourcesCollection = new StiDataSourcesCollection(null);
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			stiDataSourcesCollection.Add(dataSource);
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiDataSource dataSource2 in masterReport.Dictionary.DataSources)
		{
			hashtable2[dataSource2.Name] = dataSource2;
		}
		foreach (StiDataSource item in stiDataSourcesCollection)
		{
			if (item.Inherited)
			{
				if (!(hashtable2[item.Name] is StiDataSource stiDataSource3))
				{
					report.Dictionary.DataSources.Remove(item);
					continue;
				}
				hashtable[item.Name] = item;
				StiDataSource stiDataSource4 = stiDataSource3.Clone() as StiDataSource;
				stiDataSource4.Dictionary = report.Dictionary;
				int index = report.Dictionary.DataSources.IndexOf(item);
				report.Dictionary.DataSources.RemoveAt(index);
				report.Dictionary.DataSources.Insert(index, stiDataSource4);
			}
		}
		foreach (StiDataSource dataSource3 in masterReport.Dictionary.DataSources)
		{
			if (hashtable[dataSource3.Name] == null)
			{
				StiDataSource stiDataSource6 = dataSource3.Clone() as StiDataSource;
				stiDataSource6.Dictionary = report.Dictionary;
				int num = masterReport.Dictionary.DataSources.IndexOf(dataSource3);
				if (num >= 0 && num < report.Dictionary.DataSources.Count)
				{
					report.Dictionary.DataSources.Insert(0, stiDataSource6);
				}
				else
				{
					report.Dictionary.DataSources.Add(stiDataSource6);
				}
				hashtable[stiDataSource6.Name] = stiDataSource6;
			}
		}
	}

	private static void CompareRelations(StiReport report, StiReport masterReport)
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			hashtable[dataSource.Name] = dataSource;
		}
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(null);
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			stiDataRelationsCollection.Add(relation);
		}
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		foreach (StiDataRelation relation2 in masterReport.Dictionary.Relations)
		{
			hashtable3[relation2.Name] = relation2;
		}
		foreach (StiDataRelation item in stiDataRelationsCollection)
		{
			if (!item.Inherited)
			{
				continue;
			}
			if (!(hashtable3[item.Name] is StiDataRelation stiDataRelation3))
			{
				report.Dictionary.Relations.Remove(item);
				continue;
			}
			hashtable2[item.Name] = item;
			StiDataRelation stiDataRelation4 = stiDataRelation3.Clone() as StiDataRelation;
			stiDataRelation4.Dictionary = report.Dictionary;
			if (stiDataRelation4.ParentSource != null)
			{
				stiDataRelation4.ParentSource = hashtable[stiDataRelation4.ParentSource.Name] as StiDataSource;
			}
			if (stiDataRelation4.ChildSource != null)
			{
				stiDataRelation4.ChildSource = hashtable[stiDataRelation4.ChildSource.Name] as StiDataSource;
			}
			int index = report.Dictionary.Relations.IndexOf(item);
			report.Dictionary.Relations.RemoveAt(index);
			report.Dictionary.Relations.Insert(index, stiDataRelation4);
		}
		foreach (StiDataRelation relation3 in masterReport.Dictionary.Relations)
		{
			if (hashtable2[relation3.Name] == null)
			{
				StiDataRelation stiDataRelation6 = relation3.Clone() as StiDataRelation;
				stiDataRelation6.Dictionary = report.Dictionary;
				stiDataRelation6.ParentSource = hashtable[stiDataRelation6.ParentSource.Name] as StiDataSource;
				stiDataRelation6.ChildSource = hashtable[stiDataRelation6.ChildSource.Name] as StiDataSource;
				int num = masterReport.Dictionary.Relations.IndexOf(relation3);
				if (num >= 0 && num < report.Dictionary.Relations.Count)
				{
					report.Dictionary.Relations.Insert(0, stiDataRelation6);
				}
				else
				{
					report.Dictionary.Relations.Add(stiDataRelation6);
				}
				hashtable2[stiDataRelation6.Name] = stiDataRelation6;
			}
		}
	}

	private static void CompareDatabases(StiReport report, StiReport masterReport)
	{
		StiDatabaseCollection stiDatabaseCollection = new StiDatabaseCollection();
		foreach (StiDatabase database in report.Dictionary.Databases)
		{
			stiDatabaseCollection.Add(database);
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiDatabase database2 in masterReport.Dictionary.Databases)
		{
			hashtable2[database2.Name] = database2;
		}
		foreach (StiDatabase item in stiDatabaseCollection)
		{
			if (item.Inherited)
			{
				if (!(hashtable2[item.Name] is StiDatabase stiDatabase3))
				{
					report.Dictionary.Databases.Remove(item);
					continue;
				}
				StiDatabase stiDatabase4 = ((StiService)stiDatabase3).Clone() as StiDatabase;
				hashtable[stiDatabase4.Name] = item;
				int index = report.Dictionary.Databases.IndexOf(item);
				report.Dictionary.Databases.RemoveAt(index);
				report.Dictionary.Databases.Insert(index, stiDatabase4);
			}
		}
		foreach (StiDatabase database3 in masterReport.Dictionary.Databases)
		{
			if (hashtable[database3.Name] == null)
			{
				StiDatabase stiDatabase6 = ((StiService)database3).Clone() as StiDatabase;
				int num = masterReport.Dictionary.Databases.IndexOf(database3);
				if (num >= 0 && num < report.Dictionary.Databases.Count)
				{
					report.Dictionary.Databases.Insert(0, stiDatabase6);
				}
				else
				{
					report.Dictionary.Databases.Add(stiDatabase6);
				}
				hashtable[stiDatabase6.Name] = stiDatabase6;
			}
		}
	}

	private static void CompareBusinessObjects(StiReport report, StiReport masterReport)
	{
		StiBusinessObjectsCollection stiBusinessObjectsCollection = new StiBusinessObjectsCollection(null, null);
		foreach (StiBusinessObject businessObject in report.Dictionary.BusinessObjects)
		{
			stiBusinessObjectsCollection.Add(businessObject);
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiBusinessObject businessObject2 in masterReport.Dictionary.BusinessObjects)
		{
			hashtable2[businessObject2.Name] = businessObject2;
		}
		foreach (StiBusinessObject item in stiBusinessObjectsCollection)
		{
			if (item.Inherited)
			{
				if (!(hashtable2[item.Name] is StiBusinessObject stiBusinessObject3))
				{
					report.Dictionary.BusinessObjects.Remove(item);
					continue;
				}
				hashtable[item.Name] = item;
				StiBusinessObject stiBusinessObject4 = stiBusinessObject3.Clone() as StiBusinessObject;
				stiBusinessObject4.Dictionary = report.Dictionary;
				int index = report.Dictionary.BusinessObjects.IndexOf(item);
				report.Dictionary.BusinessObjects.RemoveAt(index);
				report.Dictionary.BusinessObjects.Insert(index, stiBusinessObject4);
			}
		}
		foreach (StiBusinessObject businessObject3 in masterReport.Dictionary.BusinessObjects)
		{
			if (hashtable[businessObject3.Name] == null)
			{
				StiBusinessObject stiBusinessObject6 = businessObject3.Clone() as StiBusinessObject;
				stiBusinessObject6.Dictionary = report.Dictionary;
				int num = masterReport.Dictionary.BusinessObjects.IndexOf(businessObject3);
				if (num >= 0 && num < report.Dictionary.BusinessObjects.Count)
				{
					report.Dictionary.BusinessObjects.Insert(0, stiBusinessObject6);
				}
				else
				{
					report.Dictionary.BusinessObjects.Add(stiBusinessObject6);
				}
				hashtable[stiBusinessObject6.Name] = stiBusinessObject6;
			}
		}
	}

	private static void CompareEvents(StiReport report, StiReport masterReport)
	{
		if (masterReport.BeginRenderEvent.Script.Length > 0)
		{
			report.BeginRenderEvent.Script = masterReport.BeginRenderEvent.Script;
		}
		if (masterReport.EndRenderEvent.Script.Length > 0)
		{
			report.EndRenderEvent.Script = masterReport.EndRenderEvent.Script;
		}
		if (masterReport.RenderingEvent.Script.Length > 0)
		{
			report.RenderingEvent.Script = masterReport.RenderingEvent.Script;
		}
		if (masterReport.ExportingEvent.Script.Length > 0)
		{
			report.ExportingEvent.Script = masterReport.ExportingEvent.Script;
		}
		if (masterReport.ExportedEvent.Script.Length > 0)
		{
			report.ExportedEvent.Script = masterReport.ExportedEvent.Script;
		}
		if (masterReport.PrintingEvent.Script.Length > 0)
		{
			report.PrintingEvent.Script = masterReport.PrintingEvent.Script;
		}
		if (masterReport.PrintedEvent.Script.Length > 0)
		{
			report.PrintedEvent.Script = masterReport.PrintedEvent.Script;
		}
	}
}
