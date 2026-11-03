using System;
using System.Collections;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiDataBandV2Builder : StiBandV2Builder
{
	public void FindHeaders(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.Headers = new StiComponentsCollection();
		for (int num = masterDataBand.Parent.Components.IndexOf(masterDataBand) - 1; num >= 0; num--)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[num];
			if (!(stiComponent is StiGroupHeaderBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand))
			{
				if (!(stiComponent is StiHeaderBand))
				{
					break;
				}
				masterDataBand.DataBandInfoV2.Headers.Insert(0, stiComponent);
			}
		}
	}

	public void FindHierarchicalHeaders(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null || !(masterDataBand is StiHierarchicalBand stiHierarchicalBand) || stiHierarchicalBand.Headers.Trim().Length <= 0)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.HierarchicalHeaders = new StiComponentsCollection();
		string[] array = stiHierarchicalBand.Headers.Split(new char[1] { ';' });
		Hashtable hashtable = new Hashtable();
		lock (array.SyncRoot)
		{
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim();
				hashtable[text2] = text2;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.Headers).SyncRoot)
		{
			foreach (StiHeaderBand header in masterDataBand.DataBandInfoV2.Headers)
			{
				if (hashtable[header.Name] != null)
				{
					masterDataBand.DataBandInfoV2.HierarchicalHeaders.Add(header);
				}
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.HierarchicalHeaders).SyncRoot)
		{
			foreach (StiHeaderBand hierarchicalHeader in masterDataBand.DataBandInfoV2.HierarchicalHeaders)
			{
				if (masterDataBand.DataBandInfoV2.Headers.Contains(hierarchicalHeader))
				{
					masterDataBand.DataBandInfoV2.Headers.Remove(hierarchicalHeader, clearParent: false);
				}
			}
		}
	}

	public void FindFooters(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.FootersOnAllPages = new StiComponentsCollection();
		masterDataBand.DataBandInfoV2.FootersOnLastPage = new StiComponentsCollection();
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count; i++)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[i];
			if (!(stiComponent is StiGroupFooterBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand) && (!(stiComponent is StiTable) || (stiComponent as StiTable).IsConverted))
			{
				if (!(stiComponent is StiFooterBand))
				{
					break;
				}
				if (((StiFooterBand)stiComponent).PrintOnAllPages)
				{
					masterDataBand.DataBandInfoV2.FootersOnAllPages.Add(stiComponent);
				}
				else
				{
					masterDataBand.DataBandInfoV2.FootersOnLastPage.Add(stiComponent);
				}
			}
		}
	}

	public void FindHierarchicalFooters(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null || !(masterDataBand is StiHierarchicalBand stiHierarchicalBand) || stiHierarchicalBand.Footers.Trim().Length <= 0)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.HierarchicalFooters = new StiComponentsCollection();
		string[] array = stiHierarchicalBand.Footers.Split(new char[1] { ';' });
		Hashtable hashtable = new Hashtable();
		lock (array.SyncRoot)
		{
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim();
				hashtable[text2] = text2;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnAllPages).SyncRoot)
		{
			foreach (StiFooterBand footersOnAllPage in masterDataBand.DataBandInfoV2.FootersOnAllPages)
			{
				if (hashtable[footersOnAllPage.Name] != null)
				{
					masterDataBand.DataBandInfoV2.HierarchicalFooters.Add(footersOnAllPage);
				}
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnLastPage).SyncRoot)
		{
			foreach (StiFooterBand item in masterDataBand.DataBandInfoV2.FootersOnLastPage)
			{
				if (hashtable[item.Name] != null)
				{
					masterDataBand.DataBandInfoV2.HierarchicalFooters.Add(item);
				}
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.HierarchicalFooters).SyncRoot)
		{
			foreach (StiFooterBand hierarchicalFooter in masterDataBand.DataBandInfoV2.HierarchicalFooters)
			{
				if (masterDataBand.DataBandInfoV2.FootersOnAllPages.Contains(hierarchicalFooter))
				{
					masterDataBand.DataBandInfoV2.FootersOnAllPages.Remove(hierarchicalFooter, clearParent: false);
				}
				if (masterDataBand.DataBandInfoV2.FootersOnLastPage.Contains(hierarchicalFooter))
				{
					masterDataBand.DataBandInfoV2.FootersOnLastPage.Remove(hierarchicalFooter, clearParent: false);
				}
			}
		}
	}

	public void FindEmptyBands(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.EmptyBands = new StiComponentsCollection();
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count; i++)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[i];
			if (stiComponent is StiEmptyBand && stiComponent.Enabled)
			{
				masterDataBand.DataBandInfoV2.EmptyBands.Add(stiComponent);
			}
			if ((!(stiComponent is StiTable) || (stiComponent as StiTable).IsConverted) && (stiComponent is StiFooterBand || stiComponent is StiGroupFooterBand || stiComponent is StiHeaderBand || stiComponent is StiGroupHeaderBand || stiComponent is StiDataBand))
			{
				break;
			}
		}
	}

	public void FindGroupHeaders(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.GroupHeaders = new StiComponentsCollection();
		for (int num = masterDataBand.Parent.Components.IndexOf(masterDataBand) - 1; num >= 0; num--)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[num];
			if (!(stiComponent is StiHeaderBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand))
			{
				if (!(stiComponent is StiGroupHeaderBand))
				{
					break;
				}
				masterDataBand.DataBandInfoV2.GroupHeaders.Insert(0, stiComponent);
			}
		}
	}

	public void FindGroupFooters(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.GroupFooters = new StiComponentsCollection();
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count; i++)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[i];
			if (!(stiComponent is StiFooterBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand) && (!(stiComponent is StiTable) || (stiComponent as StiTable).IsConverted))
			{
				if (!(stiComponent is StiGroupFooterBand))
				{
					break;
				}
				masterDataBand.DataBandInfoV2.GroupFooters.Add(stiComponent);
			}
		}
	}

	public void FindDetailDataBands(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.DetailDataBands = new StiComponentsCollection();
		StiComponentsCollection components = masterDataBand.Page.GetComponents();
		StiDataBand stiDataBand = masterDataBand;
		if (masterDataBand.IsBusinessObjectEmpty)
		{
			lock (((ICollection)components).SyncRoot)
			{
				foreach (StiComponent item in components)
				{
					if (!(item is StiEmptyBand) && item != masterDataBand)
					{
						if (item is StiTable && !(item as StiTable).IsConverted && masterDataBand.Name == item.Name + "_DB")
						{
							stiDataBand = item as StiDataBand;
						}
						if (item is IStiMasterComponent stiMasterComponent && stiMasterComponent is StiDataBand && (stiMasterComponent.MasterComponent == masterDataBand || stiMasterComponent.MasterComponent == stiDataBand) && (((StiDataBand)stiMasterComponent).DataSource != stiDataBand.DataSource || (((StiDataBand)stiMasterComponent).DataSource == null && stiDataBand.DataSource == null)))
						{
							StiComponent parent = item.Parent;
							while (parent != null && !(parent is StiPage) && parent != masterDataBand)
							{
								parent = parent.Parent;
							}
							if (parent != masterDataBand)
							{
								masterDataBand.DataBandInfoV2.DetailDataBands.Add(item);
							}
						}
						if (item is StiSubReport { SubReportPage: not null } stiSubReport)
						{
							foreach (StiComponent component in stiSubReport.SubReportPage.GetComponents())
							{
								if (component is IStiMasterComponent stiMasterComponent2 && stiMasterComponent2 is StiDataBand && (stiMasterComponent2.MasterComponent == masterDataBand || stiMasterComponent2.MasterComponent == stiDataBand) && (((StiDataBand)stiMasterComponent2).DataSource != stiDataBand.DataSource || (((StiDataBand)stiMasterComponent2).DataSource == null && stiDataBand.DataSource == null)))
								{
									if (masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports == null)
									{
										masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports = new Hashtable();
									}
									masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports[component] = null;
								}
							}
						}
					}
				}
				return;
			}
		}
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		StiComponentsCollection stiComponentsCollection2 = new StiComponentsCollection();
		foreach (StiComponent item2 in components)
		{
			if (item2 is StiSubReport { SubReportPage: not null } stiSubReport2)
			{
				stiComponentsCollection.AddRange(stiSubReport2.SubReportPage.GetComponents());
				stiComponentsCollection2.AddRange(stiSubReport2.SubReportPage.GetComponents());
			}
			else
			{
				stiComponentsCollection.Add(item2);
			}
		}
		components = stiComponentsCollection;
		bool flag = false;
		Hashtable hashtable = new Hashtable();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item3 in components)
			{
				if (item3 is StiEmptyBand || item3 == masterDataBand || hashtable.ContainsKey(item3))
				{
					continue;
				}
				StiDataBand stiDataBand2 = item3 as StiDataBand;
				if (stiDataBand2 != null && !stiDataBand2.IsBusinessObjectEmpty)
				{
					if (masterDataBand.BusinessObject == stiDataBand2.BusinessObject.ParentBusinessObject)
					{
						StiComponent parent2 = item3.Parent;
						while (parent2 != null && !(parent2 is StiPage) && parent2 != masterDataBand)
						{
							parent2 = parent2.Parent;
						}
						if (parent2 != masterDataBand)
						{
							masterDataBand.DataBandInfoV2.DetailDataBands.Add(item3);
							if (stiComponentsCollection2.IndexOf(item3) != -1)
							{
								if (masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports == null)
								{
									masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports = new Hashtable();
								}
								masterDataBand.DataBandInfoV2.DetailDataBandsFromSubReports[item3] = true;
							}
						}
						flag = true;
					}
					if (flag && IsParentOrCurrentBO(stiDataBand2.BusinessObject.ParentBusinessObject, masterDataBand.BusinessObject))
					{
						break;
					}
					if (stiDataBand2.DataBandInfoV2.DetailDataBands != null)
					{
						foreach (object detailDataBand in stiDataBand2.DataBandInfoV2.DetailDataBands)
						{
							hashtable[detailDataBand] = null;
						}
					}
				}
				if (stiDataBand2 != null && stiDataBand2.IsBusinessObjectEmpty && stiDataBand2.CountData != 0 && stiDataBand2.MasterComponent == masterDataBand)
				{
					masterDataBand.DataBandInfoV2.DetailDataBands.Add(item3);
				}
			}
		}
	}

	private bool IsParentOrCurrentBO(StiBusinessObject currentBO, StiBusinessObject parentBO)
	{
		while (currentBO != null)
		{
			if (currentBO == parentBO)
			{
				return false;
			}
			currentBO = currentBO.ParentBusinessObject;
		}
		return true;
	}

	public void FindSubReports(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.SubReports = new StiComponentsCollection();
		StiComponentsCollection components = masterDataBand.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				if (item is StiSubReport)
				{
					masterDataBand.DataBandInfoV2.SubReports.Add(item);
				}
			}
		}
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count && masterDataBand.Parent.Components[i] is StiChildBand; i++)
		{
			StiChildBand stiChildBand = masterDataBand.Parent.Components[i] as StiChildBand;
			StiComponentsCollection components2 = stiChildBand.GetComponents();
			lock (((ICollection)components2).SyncRoot)
			{
				foreach (StiComponent item2 in components2)
				{
					if (item2 is StiSubReport)
					{
						masterDataBand.DataBandInfoV2.SubReports.Add(item2);
					}
				}
			}
		}
	}

	public void FindDetails(StiDataBand masterDataBand)
	{
		if (masterDataBand == null || masterDataBand.Parent == null || masterDataBand.DataBandInfoV2 == null)
		{
			return;
		}
		masterDataBand.DataBandInfoV2.Details = new StiComponentsCollection();
		StiComponentsCollection components = masterDataBand.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				if (item is IStiMasterComponent stiMasterComponent && !(item is StiSubReport) && (!(stiMasterComponent is StiDataBand) || ((StiDataBand)stiMasterComponent).DataSource != masterDataBand.DataSource || masterDataBand.DataSource == null))
				{
					masterDataBand.DataBandInfoV2.Details.Add(item);
				}
			}
		}
	}

	public void ResetHeaders(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.Headers = null;
	}

	public void ResetHierarchicalHeaders(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.HierarchicalHeaders = null;
	}

	public void ResetFooters(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.FootersOnAllPages = null;
		masterDataBand.DataBandInfoV2.FootersOnLastPage = null;
	}

	public void ResetHierarchicalFooters(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.HierarchicalFooters = null;
	}

	public void ResetEmptyBands(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.EmptyBands = null;
	}

	public void ResetGroupHeaders(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.GroupHeaders = null;
	}

	public void ResetGroupFooters(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.GroupFooters = null;
	}

	public void ResetDetailDataBands(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.DetailDataBands = null;
	}

	public void ResetDetails(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV2.Details = null;
	}

	public void AddKeepLevelAtLatestDataBand(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		if (masterDataBand.Report.Engine.ContainerForRender != null)
		{
			masterDataBand.Report.Engine.AddKeepLevelAtLatestDataBand();
		}
	}

	public void AddKeepLevel(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		if (masterDataBand.Report.Engine.ContainerForRender != null)
		{
			masterDataBand.Report.Engine.AddLevel();
		}
	}

	public void RemoveKeepLevel(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		if (masterDataBand.Report.Engine.ContainerForRender != null)
		{
			masterDataBand.Report.Engine.RemoveLevel();
		}
	}

	public void RemoveKeepGroupHeaders(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2 == null || masterDataBand.DataBandInfoV2.GroupHeaders == null)
		{
			return;
		}
		int num = 0;
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				if (masterDataBand.DataBandInfoV2.GroupHeaderResults[num] && groupHeader.KeepGroupHeaderTogether)
				{
					RemoveKeepLevel(masterDataBand);
				}
				num++;
			}
		}
	}

	public void RemoveKeepHeaders(StiDataBand masterDataBand, bool[] keepHeaders)
	{
		if (masterDataBand.DataBandInfoV2 == null || masterDataBand.DataBandInfoV2.Headers == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.Headers).SyncRoot)
		{
			for (int i = 0; i < masterDataBand.DataBandInfoV2.Headers.Count; i++)
			{
				StiHeaderBand stiHeaderBand = masterDataBand.DataBandInfoV2.Headers[i] as StiHeaderBand;
				if (stiHeaderBand.KeepHeaderTogether && (masterDataBand.Position == 0 || keepHeaders[i]))
				{
					RemoveKeepLevel(masterDataBand);
					keepHeaders[i] = false;
				}
			}
		}
	}

	public bool AllowKeepDetails(StiDataBand masterDataBand)
	{
		if (!AllowDetailDataBands(masterDataBand))
		{
			return false;
		}
		if (masterDataBand.KeepDetails == StiKeepDetails.None)
		{
			return false;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.DetailDataBands).SyncRoot)
		{
			foreach (StiComponent detailDataBand in masterDataBand.DataBandInfoV2.DetailDataBands)
			{
				if (detailDataBand.Parent.Name != masterDataBand.Parent.Name)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void AddKeepDetails(StiDataBand masterDataBand)
	{
		if (!AllowKeepDetails(masterDataBand))
		{
			return;
		}
		if (masterDataBand.KeepDetails == StiKeepDetails.KeepFirstDetailTogether || masterDataBand.KeepDetails == StiKeepDetails.KeepFirstRowTogether)
		{
			StiDataBand stiDataBand = masterDataBand.Report.Engine.KeepFirstDetailTogetherTablesList[masterDataBand] as StiDataBand;
			if (stiDataBand == null)
			{
				stiDataBand = masterDataBand;
			}
			masterDataBand.Report.Engine.KeepFirstDetailTogetherList[stiDataBand] = masterDataBand;
		}
		AddKeepLevel(masterDataBand);
	}

	public void RemoveKeepDetails(StiDataBand masterDataBand)
	{
		if (AllowKeepDetails(masterDataBand) && masterDataBand.KeepDetails == StiKeepDetails.KeepDetailsTogether)
		{
			RemoveKeepLevel(masterDataBand);
		}
		if (masterDataBand.MasterComponent != null && masterDataBand.Report.Engine.KeepFirstDetailTogetherList[masterDataBand.MasterComponent] != null)
		{
			RemoveKeepLevel(masterDataBand);
			masterDataBand.Report.Engine.KeepFirstDetailTogetherList[masterDataBand.MasterComponent] = null;
		}
		if (masterDataBand.Report.Engine.KeepFirstDetailTogetherList[masterDataBand] != null)
		{
			RemoveKeepLevel(masterDataBand);
			masterDataBand.Report.Engine.KeepFirstDetailTogetherList[masterDataBand] = null;
		}
	}

	public void RemoveKeepDetailsRow(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.DetailDataBands == null || masterDataBand.DataBandInfoV2.DetailDataBands.Count == 0)
		{
			return;
		}
		StiDataBand stiDataBand = masterDataBand;
		while (stiDataBand != null && stiDataBand.MasterComponent != null)
		{
			stiDataBand = stiDataBand.MasterComponent as StiDataBand;
			if (stiDataBand != null && stiDataBand.KeepDetails == StiKeepDetails.KeepFirstRowTogether && masterDataBand.Report.Engine.KeepFirstDetailTogetherList[stiDataBand] != null)
			{
				RemoveKeepLevel(stiDataBand);
				masterDataBand.Report.Engine.KeepFirstDetailTogetherList[stiDataBand] = null;
			}
		}
	}

	public void StartBands(StiDataBand masterDataBand, StiComponentsCollection bands)
	{
		lock (((ICollection)bands).SyncRoot)
		{
			foreach (StiBand band in bands)
			{
				StartBand(masterDataBand, band);
			}
		}
	}

	public void StartBand(StiDataBand masterDataBand, StiBand band)
	{
		if (band is IStiPrintOnAllPages { PrintOnAllPages: not false })
		{
			masterDataBand.Report.Engine.BandsOnAllPages.Add(masterDataBand, band);
		}
	}

	public void EndBands(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.BandsOnAllPages.Remove(masterDataBand);
	}

	public bool GetGroupHeaderResult(StiDataBand masterDataBand, StiGroupHeaderBand groupHeaderBand)
	{
		int num = 0;
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				if (groupHeader == groupHeaderBand)
				{
					return masterDataBand.DataBandInfoV2.GroupHeaderResults[num];
				}
				num++;
			}
		}
		return false;
	}

	public bool GetGroupFooterResult(StiDataBand masterDataBand, StiGroupHeaderBand groupHeaderBand)
	{
		int num = 0;
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				if (groupHeader == groupHeaderBand)
				{
					return masterDataBand.DataBandInfoV2.GroupFooterResults[num];
				}
				num++;
			}
		}
		return false;
	}

	public void LinkGroupHeadersAndGroupFooters(StiDataBand masterDataBand)
	{
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				groupHeader.GroupHeaderBandInfoV2.GroupFooter = null;
			}
		}
		for (int i = 0; i < masterDataBand.DataBandInfoV2.GroupFooters.Count; i++)
		{
			int num = masterDataBand.DataBandInfoV2.GroupHeaders.Count - i - 1;
			if (num >= 0)
			{
				((StiGroupFooterBand)masterDataBand.DataBandInfoV2.GroupFooters[i]).GroupFooterBandInfoV2.GroupHeader = (StiGroupHeaderBand)masterDataBand.DataBandInfoV2.GroupHeaders[num];
				((StiGroupHeaderBand)masterDataBand.DataBandInfoV2.GroupHeaders[num]).GroupHeaderBandInfoV2.GroupFooter = (StiGroupFooterBand)masterDataBand.DataBandInfoV2.GroupFooters[i];
			}
			else
			{
				((StiGroupFooterBand)masterDataBand.DataBandInfoV2.GroupFooters[i]).GroupFooterBandInfoV2.GroupHeader = null;
			}
		}
	}

	public void ResetLinkGroupHeadersAndGroupFooters(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.GroupHeaders != null)
		{
			lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
			{
				foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
				{
					groupHeader.GroupHeaderBandInfoV2.GroupFooter = null;
				}
			}
		}
		if (masterDataBand.DataBandInfoV2.GroupFooters == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupFooters).SyncRoot)
		{
			foreach (StiGroupFooterBand groupFooter in masterDataBand.DataBandInfoV2.GroupFooters)
			{
				groupFooter.GroupFooterBandInfoV2.GroupHeader = null;
			}
		}
	}

	public static void PrepareGroupResults(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.GroupHeaders == null || masterDataBand.DataBandInfoV2.GroupHeaders.Count == 0)
		{
			return;
		}
		int num = 0;
		int position = masterDataBand.Position;
		int count = masterDataBand.Count;
		if (masterDataBand.DataBandInfoV2.GroupHeaderCachedResults == null || masterDataBand.DataBandInfoV2.GroupHeaderCachedResults.Length != count)
		{
			masterDataBand.DataBandInfoV2.GroupHeaderCachedResults = new bool[count][];
			masterDataBand.DataBandInfoV2.GroupFooterCachedResults = new bool[count][];
		}
		bool flag = false;
		if (position < masterDataBand.DataBandInfoV2.GroupHeaderCachedResults.Length && masterDataBand.DataBandInfoV2.GroupHeaderCachedResults[position] == null)
		{
			masterDataBand.DataBandInfoV2.GroupHeaderCachedResults[position] = new bool[masterDataBand.DataBandInfoV2.GroupHeaders.Count];
			masterDataBand.DataBandInfoV2.GroupFooterCachedResults[position] = new bool[masterDataBand.DataBandInfoV2.GroupHeaders.Count];
			flag = true;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				if (position > 0)
				{
					if (flag)
					{
						if (!masterDataBand.IsBusinessObjectEmpty)
						{
							StiBusinessObject businessObject = masterDataBand.BusinessObject;
							businessObject.SetPrevValue();
							object currentConditionValue = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							businessObject.RestoreCurrentValue();
							object currentConditionValue2 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.DataBandInfoV2.GroupHeaderResults[num] = !object.Equals(currentConditionValue, currentConditionValue2);
						}
						else
						{
							masterDataBand.Position = position - 1;
							object currentConditionValue3 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.Position = position;
							object currentConditionValue4 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.DataBandInfoV2.GroupHeaderResults[num] = !object.Equals(currentConditionValue3, currentConditionValue4);
						}
						masterDataBand.DataBandInfoV2.GroupHeaderCachedResults[position][num] = masterDataBand.DataBandInfoV2.GroupHeaderResults[num];
					}
					else
					{
						masterDataBand.DataBandInfoV2.GroupHeaderResults[num] = masterDataBand.DataBandInfoV2.GroupHeaderCachedResults[position][num];
					}
				}
				else
				{
					masterDataBand.DataBandInfoV2.GroupHeaderResults[num] = true;
				}
				if (position < count - 1)
				{
					if (flag)
					{
						if (!masterDataBand.IsBusinessObjectEmpty)
						{
							StiBusinessObject businessObject2 = masterDataBand.BusinessObject;
							businessObject2.SetNextValue();
							object currentConditionValue5 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							businessObject2.RestoreCurrentValue();
							object currentConditionValue6 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.DataBandInfoV2.GroupFooterResults[num] = !object.Equals(currentConditionValue5, currentConditionValue6);
						}
						else
						{
							masterDataBand.Position = position + 1;
							object currentConditionValue7 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.Position = position;
							object currentConditionValue8 = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(groupHeader);
							masterDataBand.DataBandInfoV2.GroupFooterResults[num] = !object.Equals(currentConditionValue7, currentConditionValue8);
						}
						masterDataBand.DataBandInfoV2.GroupFooterCachedResults[position][num] = masterDataBand.DataBandInfoV2.GroupFooterResults[num];
					}
					else
					{
						masterDataBand.DataBandInfoV2.GroupFooterResults[num] = masterDataBand.DataBandInfoV2.GroupFooterCachedResults[position][num];
					}
				}
				else
				{
					masterDataBand.DataBandInfoV2.GroupFooterResults[num] = true;
				}
				num++;
			}
		}
		for (int i = 0; i < masterDataBand.DataBandInfoV2.GroupHeaderResults.Length; i++)
		{
			if (masterDataBand.DataBandInfoV2.GroupHeaderResults[i])
			{
				for (int j = i + 1; j < masterDataBand.DataBandInfoV2.GroupHeaderResults.Length; j++)
				{
					masterDataBand.DataBandInfoV2.GroupHeaderResults[j] = true;
				}
				break;
			}
		}
		for (int k = 0; k < masterDataBand.DataBandInfoV2.GroupFooterResults.Length; k++)
		{
			if (masterDataBand.DataBandInfoV2.GroupFooterResults[k])
			{
				for (int l = k + 1; l < masterDataBand.DataBandInfoV2.GroupFooterResults.Length; l++)
				{
					masterDataBand.DataBandInfoV2.GroupFooterResults[l] = true;
				}
				break;
			}
		}
	}

	public void RenderGroupHeaders(StiDataBand masterDataBand)
	{
		masterDataBand.ParentBookmark = masterDataBand.DataBandInfoV2.StoredParentBookmark;
		masterDataBand.ParentPointer = masterDataBand.DataBandInfoV2.StoredParentPointer;
		bool flag = false;
		int num = 0;
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				groupHeader.ParentBookmark = masterDataBand.ParentBookmark;
				bool flag2 = groupHeader.DoBookmark();
				if (groupHeader.ParentBookmark != groupHeader.CurrentBookmark)
				{
					masterDataBand.ParentBookmark = groupHeader.CurrentBookmark;
				}
				groupHeader.ParentPointer = masterDataBand.ParentPointer;
				groupHeader.DoPointer(!flag2);
				if (groupHeader.ParentPointer != groupHeader.CurrentPointer)
				{
					masterDataBand.ParentPointer = groupHeader.CurrentPointer;
				}
				if (masterDataBand.DataBandInfoV2.GroupHeaderResults[num])
				{
					for (int i = num + 1; i < masterDataBand.DataBandInfoV2.GroupHeaders.Count; i++)
					{
						masterDataBand.Report.Engine.RemoveBandFromPageBreakSkipList(masterDataBand.DataBandInfoV2.GroupHeaders[i] as IStiPageBreak);
					}
					if (masterDataBand.Report != null)
					{
						StiReport report = masterDataBand.Report;
						if (report.CacheTotals && report.CachedTotals != null)
						{
							report.CachedTotals[groupHeader] = null;
						}
					}
					groupHeader.GroupHeaderBandInfoV2.SkipKeepGroups = masterDataBand.Report.Engine.IsFirstDataBandOnPage && masterDataBand.ComponentType == StiComponentType.Master && groupHeader.Line == 1;
					if (!groupHeader.GroupHeaderBandInfoV2.SkipKeepGroups && groupHeader.KeepGroupTogether)
					{
						AddKeepLevel(masterDataBand);
					}
					if (groupHeader.KeepGroupHeaderTogether)
					{
						AddKeepLevel(masterDataBand);
					}
					masterDataBand.Line = 1;
					groupHeader.InvokeBeginRender();
					groupHeader.CollapsingIndex = groupHeader.Line;
					groupHeader.Report.GroupLine = groupHeader.Line;
					InvokeCollapsedEvent(groupHeader);
					RenderBand(masterDataBand, groupHeader);
					flag = true;
					if (IsCollapsed(groupHeader, isRendering: true))
					{
						groupHeader.GroupHeaderBandInfoV2.OldSilentMode = masterDataBand.Report.Engine.SilentMode;
						masterDataBand.Report.Engine.SilentMode = true;
						groupHeader.GroupHeaderBandInfoV2.SilentModeEnabled = true;
					}
				}
				num++;
			}
		}
		if (flag)
		{
			RenderColumns(masterDataBand);
		}
	}

	public void RenderGroupFooters(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.GroupHeaders == null)
		{
			return;
		}
		for (int num = masterDataBand.DataBandInfoV2.GroupHeaders.Count - 1; num >= 0; num--)
		{
			if (masterDataBand.DataBandInfoV2.GroupFooterResults[num])
			{
				StiGroupHeaderBand stiGroupHeaderBand = masterDataBand.DataBandInfoV2.GroupHeaders[num] as StiGroupHeaderBand;
				StiGroupFooterBand groupFooter = stiGroupHeaderBand.GroupHeaderBandInfoV2.GroupFooter;
				if (groupFooter != null)
				{
					bool keepGroupFooterTogether = groupFooter.KeepGroupFooterTogether;
					if (keepGroupFooterTogether)
					{
						AddKeepLevelAtLatestDataBand(masterDataBand);
					}
					if (stiGroupHeaderBand.GroupHeaderBandInfoV2.SilentModeEnabled && stiGroupHeaderBand.Interaction is StiBandInteraction && !((StiBandInteraction)stiGroupHeaderBand.Interaction).CollapseGroupFooter)
					{
						masterDataBand.Report.Engine.SilentMode = stiGroupHeaderBand.GroupHeaderBandInfoV2.OldSilentMode;
						stiGroupHeaderBand.GroupHeaderBandInfoV2.SilentModeEnabled = false;
					}
					RenderBand(masterDataBand, groupFooter);
					if (stiGroupHeaderBand.GroupHeaderBandInfoV2.SilentModeEnabled)
					{
						masterDataBand.Report.Engine.SilentMode = stiGroupHeaderBand.GroupHeaderBandInfoV2.OldSilentMode;
						stiGroupHeaderBand.GroupHeaderBandInfoV2.SilentModeEnabled = false;
					}
					if (keepGroupFooterTogether)
					{
						RemoveKeepLevel(masterDataBand);
					}
				}
				stiGroupHeaderBand.InvokeEndRender();
				if (!stiGroupHeaderBand.GroupHeaderBandInfoV2.SkipKeepGroups && stiGroupHeaderBand.KeepGroupTogether)
				{
					RemoveKeepLevel(masterDataBand);
				}
				masterDataBand.Report.Engine.PrintOnAllPagesIgnoreList.Remove(stiGroupHeaderBand);
				for (int i = num + 1; i < masterDataBand.DataBandInfoV2.GroupHeaders.Count; i++)
				{
					StiGroupHeaderBand key = masterDataBand.DataBandInfoV2.GroupHeaders[i] as StiGroupHeaderBand;
					masterDataBand.Report.Engine.PrintOnAllPagesIgnoreList.Remove(key);
				}
				stiGroupHeaderBand.Line++;
			}
		}
	}

	public static void SetDetails(StiDataBand masterDataBand)
	{
		if (!masterDataBand.IsDataSourceEmpty || !masterDataBand.IsBusinessObjectEmpty)
		{
			Hashtable hashtable = new Hashtable();
			if (masterDataBand.DataBandInfoV2.DetailDataBands != null)
			{
				lock (((ICollection)masterDataBand.DataBandInfoV2.DetailDataBands).SyncRoot)
				{
					foreach (StiComponent detailDataBand in masterDataBand.DataBandInfoV2.DetailDataBands)
					{
						masterDataBand.Report.Engine.RemoveBandFromPageBreakSkipList(detailDataBand as IStiPageBreak);
						StiDataHelper.SetData(detailDataBand, reinit: false);
						if (detailDataBand is StiDataBand && !((StiDataBand)detailDataBand).IsBusinessObjectEmpty)
						{
							StiBusinessObject businessObject = ((StiDataBand)detailDataBand).BusinessObject;
							hashtable[businessObject] = businessObject;
						}
					}
				}
			}
			if (!masterDataBand.IsBusinessObjectEmpty)
			{
				StiBusinessObject businessObject2 = masterDataBand.BusinessObject;
				foreach (StiBusinessObject businessObject3 in businessObject2.BusinessObjects)
				{
					if (hashtable[businessObject3] == null)
					{
						businessObject3.SetDetails();
					}
				}
			}
			if (masterDataBand.DataBandInfoV2.SubReports != null)
			{
				lock (((ICollection)masterDataBand.DataBandInfoV2.SubReports).SyncRoot)
				{
					foreach (StiSubReport subReport in masterDataBand.DataBandInfoV2.SubReports)
					{
						if (subReport.SubReportPage == null)
						{
							continue;
						}
						StiComponentsCollection components = subReport.SubReportPage.GetComponents();
						lock (((ICollection)components).SyncRoot)
						{
							foreach (StiComponent item in components)
							{
								if (!(item is StiDataBand stiDataBand) || stiDataBand.DataSource != masterDataBand.DataSource)
								{
									StiDataHelper.SetData(item, reinit: false);
								}
							}
						}
					}
				}
			}
			if (masterDataBand.DataBandInfoV2.Details != null)
			{
				lock (((ICollection)masterDataBand.DataBandInfoV2.Details).SyncRoot)
				{
					foreach (StiComponent detail in masterDataBand.DataBandInfoV2.Details)
					{
						StiDataHelper.SetData(detail, reinit: false);
					}
				}
			}
		}
		PrepareGroupResults(masterDataBand);
	}

	public void RenderDetailDataBands(StiDataBand masterDataBand)
	{
		if (!AllowDetailDataBands(masterDataBand))
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.DetailDataBands).SyncRoot)
		{
			foreach (StiComponent detailDataBand in masterDataBand.DataBandInfoV2.DetailDataBands)
			{
				if (detailDataBand.Enabled && IsAllow(masterDataBand, detailDataBand as StiDataBand))
				{
					detailDataBand.ParentBookmark = masterDataBand.CurrentBookmark;
					detailDataBand.ParentPointer = masterDataBand.CurrentPointer;
					if (detailDataBand is IStiRenderMaster stiRenderMaster)
					{
						stiRenderMaster.RenderMaster();
					}
					else
					{
						detailDataBand.Render();
					}
				}
			}
		}
	}

	private StiDataBand GetParentDataBand(StiContainer cont)
	{
		StiContainer parent = cont.Parent;
		while (parent != null && !(parent is StiPage))
		{
			if (parent is StiDataBand)
			{
				return parent as StiDataBand;
			}
			if (parent is StiChildBand)
			{
				StiBand master = (parent as StiChildBand).GetMaster();
				if (master is StiDataBand)
				{
					return master as StiDataBand;
				}
			}
			parent = parent.Parent;
		}
		return null;
	}

	private bool IsAllow(StiDataBand master, StiDataBand detail)
	{
		if (master.DataBandInfoV2.DetailDataBandsFromSubReports != null && master.DataBandInfoV2.DetailDataBandsFromSubReports[detail] != null)
		{
			return false;
		}
		if (master.Parent == detail.Parent)
		{
			return true;
		}
		StiDataBand parentDataBand = GetParentDataBand(master);
		StiDataBand parentDataBand2 = GetParentDataBand(detail);
		if (parentDataBand == parentDataBand2)
		{
			return parentDataBand == null;
		}
		return true;
	}

	public bool AllowDetailDataBands(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.DetailDataBands != null && masterDataBand.DataBandInfoV2.DetailDataBands.Count > 0)
		{
			return masterDataBand.Columns < 2;
		}
		return false;
	}

	public bool IsDenyDetailsOnFirstPage(StiDataBand masterDataBand)
	{
		if (masterDataBand.Report.Engine.IsFirstDataBandOnPage && masterDataBand.ComponentType == StiComponentType.Master)
		{
			return masterDataBand.Line == 1;
		}
		return false;
	}

	public static bool IsDetailDataSourcesEmpty(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.DetailDataBands == null || masterDataBand.DataBandInfoV2.DetailDataBands.Count == 0)
		{
			return false;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.DetailDataBands).SyncRoot)
		{
			foreach (StiComponent detailDataBand in masterDataBand.DataBandInfoV2.DetailDataBands)
			{
				if (detailDataBand is IStiDataSource && !((IStiDataSource)detailDataBand).IsEmpty)
				{
					return false;
				}
				if (detailDataBand is IStiBusinessObject && !((IStiBusinessObject)detailDataBand).IsEmpty)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool IsPrintIfDetailEmpty(StiDataBand masterDataBand)
	{
		if (masterDataBand.PrintIfDetailEmpty)
		{
			return true;
		}
		if (IsDetailDataSourcesEmpty(masterDataBand))
		{
			return false;
		}
		bool flag = false;
		bool flag2 = true;
		if (masterDataBand.DataBandInfoV2.DetailDataBands != null)
		{
			foreach (StiDataBand detailDataBand in masterDataBand.DataBandInfoV2.DetailDataBands)
			{
				flag2 = false;
				if (StiOptions.Engine.PrintIfDetailEmptyNesting)
				{
					detailDataBand.SaveState("CheckPrintIfDetailEmpty");
					detailDataBand.First();
					while (!detailDataBand.IsEof)
					{
						if (IsPrintIfDetailEmpty(detailDataBand))
						{
							flag = true;
							break;
						}
						detailDataBand.Next();
					}
					detailDataBand.RestoreState("CheckPrintIfDetailEmpty");
				}
				else if (IsPrintIfDetailEmpty(detailDataBand))
				{
					flag = true;
				}
				if (flag)
				{
					break;
				}
			}
		}
		return flag2 || flag;
	}

	public void RenderHeaders(StiDataBand masterDataBand, bool[] keepHeaders)
	{
		lock (((ICollection)masterDataBand.DataBandInfoV2.Headers).SyncRoot)
		{
			for (int i = 0; i < masterDataBand.DataBandInfoV2.Headers.Count; i++)
			{
				StiHeaderBand stiHeaderBand = masterDataBand.DataBandInfoV2.Headers[i] as StiHeaderBand;
				if (!masterDataBand.IsEmpty || stiHeaderBand.PrintIfEmpty)
				{
					if (!masterDataBand.IsEmpty && stiHeaderBand.KeepHeaderTogether)
					{
						AddKeepLevel(masterDataBand);
						keepHeaders[i] = true;
					}
					RenderBand(masterDataBand, stiHeaderBand);
				}
			}
		}
	}

	public void RenderHierarchicalHeaders(StiDataBand masterDataBand, bool allowIndent, int level)
	{
		if (masterDataBand.DataBandInfoV2.HierarchicalHeaders == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.HierarchicalHeaders).SyncRoot)
		{
			foreach (StiHeaderBand hierarchicalHeader in masterDataBand.DataBandInfoV2.HierarchicalHeaders)
			{
				if (masterDataBand.IsEmpty && !hierarchicalHeader.PrintIfEmpty)
				{
					continue;
				}
				if (!masterDataBand.IsEmpty && hierarchicalHeader.KeepHeaderTogether)
				{
					AddKeepLevel(masterDataBand);
				}
				try
				{
					if (allowIndent)
					{
						StiHierarchicalBandV2Builder.CreateIndention(masterDataBand as StiHierarchicalBand, hierarchicalHeader, level);
					}
					RenderBand(masterDataBand, hierarchicalHeader);
				}
				finally
				{
					if (allowIndent)
					{
						StiHierarchicalBandV2Builder.CreateIndention(masterDataBand as StiHierarchicalBand, hierarchicalHeader, -level);
					}
				}
			}
		}
	}

	public void AddFooterMarker(StiDataBand masterDataBand, StiFooterBand footerMaster)
	{
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		masterDataBand.Report.Engine.AddFooterMarker(footerMaster);
	}

	public void RenderMarkerFootersOnAllPages(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.FootersOnAllPages == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnAllPages).SyncRoot)
		{
			foreach (StiFooterBand footersOnAllPage in masterDataBand.DataBandInfoV2.FootersOnAllPages)
			{
				if (!masterDataBand.IsEmpty || footersOnAllPage.PrintIfEmpty)
				{
					AddFooterMarker(masterDataBand, footersOnAllPage);
				}
			}
		}
	}

	public void RenderFootersOnLastPage(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.FootersOnLastPage == null)
		{
			return;
		}
		try
		{
			if (masterDataBand is StiHierarchicalBand)
			{
				((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.FinalFooterCalculation = true;
			}
			lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnLastPage).SyncRoot)
			{
				foreach (StiFooterBand item in masterDataBand.DataBandInfoV2.FootersOnLastPage)
				{
					if (masterDataBand.IsEmpty && !item.PrintIfEmpty)
					{
						continue;
					}
					bool flag = !masterDataBand.IsEmpty && item.KeepFooterTogether;
					if (flag)
					{
						AddKeepLevelAtLatestDataBand(masterDataBand);
					}
					StiContainer containerForRender = masterDataBand.Report.Engine.ContainerForRender;
					StiFooterMarkerContainer stiFooterMarkerContainer = null;
					if (containerForRender != null && containerForRender.Components.Count > 0)
					{
						stiFooterMarkerContainer = containerForRender.Components[containerForRender.Components.Count - 1] as StiFooterMarkerContainer;
					}
					RenderBand(masterDataBand, item);
					StiComponent stiComponent = null;
					if (stiFooterMarkerContainer != null)
					{
						stiComponent = containerForRender.Components[containerForRender.Components.Count - 1];
					}
					if (flag)
					{
						RemoveKeepLevel(masterDataBand);
					}
					if (stiFooterMarkerContainer == null || stiFooterMarkerContainer.ContainerInfoV2.ParentBand == null)
					{
						continue;
					}
					int num = stiFooterMarkerContainer.ContainerInfoV2.ParentBand.Parent.Components.IndexOf(stiFooterMarkerContainer.ContainerInfoV2.ParentBand);
					int num2 = item.Parent.Components.IndexOf(item);
					if (num == -1 || num2 == -1 || num <= num2)
					{
						continue;
					}
					if (masterDataBand.Report.Engine.ContainerForRender == containerForRender)
					{
						containerForRender.Components.Remove(stiFooterMarkerContainer);
						containerForRender.Components.Add(stiFooterMarkerContainer);
						stiFooterMarkerContainer.Top = stiComponent.Bottom;
						continue;
					}
					StiContainer stiContainer = containerForRender.Components[containerForRender.Components.Count - 2] as StiContainer;
					StiContainer stiContainer2 = containerForRender.Components[containerForRender.Components.Count - 1] as StiContainer;
					if (stiContainer != null && stiContainer2 != null && stiContainer2.Name == "Breaked")
					{
						containerForRender.Components.Remove(stiContainer);
						containerForRender.Components.Add(stiContainer);
						stiContainer2.Top = stiContainer.Top;
						stiContainer.Top = stiContainer2.Bottom;
					}
				}
			}
		}
		finally
		{
			if (masterDataBand is StiHierarchicalBand)
			{
				((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.FinalFooterCalculation = false;
			}
		}
	}

	public void RenderFootersOnAllPages(StiDataBand masterDataBand)
	{
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnAllPages).SyncRoot)
		{
			foreach (StiFooterBand footersOnAllPage in masterDataBand.DataBandInfoV2.FootersOnAllPages)
			{
				if (!masterDataBand.IsEmpty || footersOnAllPage.PrintIfEmpty)
				{
					RenderBand(masterDataBand, footersOnAllPage);
				}
			}
		}
	}

	public void RenderHierarchicalFooters(StiDataBand masterDataBand, bool allowIndent, int level)
	{
		if (masterDataBand.DataBandInfoV2.HierarchicalFooters == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.HierarchicalFooters).SyncRoot)
		{
			foreach (StiFooterBand hierarchicalFooter in masterDataBand.DataBandInfoV2.HierarchicalFooters)
			{
				if (masterDataBand.IsEmpty && !hierarchicalFooter.PrintIfEmpty)
				{
					continue;
				}
				bool flag = !masterDataBand.IsEmpty && hierarchicalFooter.KeepFooterTogether;
				if (flag)
				{
					AddKeepLevelAtLatestDataBand(masterDataBand);
				}
				try
				{
					if (allowIndent)
					{
						StiHierarchicalBandV2Builder.CreateIndention(masterDataBand as StiHierarchicalBand, hierarchicalFooter, level);
					}
					RenderBand(masterDataBand, hierarchicalFooter);
				}
				finally
				{
					if (allowIndent)
					{
						StiHierarchicalBandV2Builder.CreateIndention(masterDataBand as StiHierarchicalBand, hierarchicalFooter, -level);
					}
				}
				if (flag)
				{
					RemoveKeepLevel(masterDataBand);
				}
			}
		}
	}

	public void RenderReportTitles(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.ReportTitles == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.ReportTitles).SyncRoot)
		{
			foreach (StiReportTitleBand reportTitle in masterDataBand.DataBandInfoV2.ReportTitles)
			{
				if (!masterDataBand.IsEmpty || reportTitle.PrintIfEmpty)
				{
					RenderBand(masterDataBand, reportTitle);
				}
			}
		}
	}

	public void RenderReportSummaries(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.ReportSummaries == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.ReportSummaries).SyncRoot)
		{
			foreach (StiReportSummaryBand reportSummary in masterDataBand.DataBandInfoV2.ReportSummaries)
			{
				if (!masterDataBand.IsEmpty || reportSummary.PrintIfEmpty)
				{
					bool flag = !masterDataBand.IsEmpty && reportSummary.KeepReportSummaryTogether;
					if (flag)
					{
						AddKeepLevelAtLatestDataBand(masterDataBand);
					}
					RenderBand(masterDataBand, reportSummary);
					if (flag)
					{
						RemoveKeepLevel(masterDataBand);
					}
				}
			}
		}
	}

	public bool CheckKeepReportSummaryTogether(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.ReportSummaries == null)
		{
			return false;
		}
		bool result = false;
		lock (((ICollection)masterDataBand.DataBandInfoV2.ReportSummaries).SyncRoot)
		{
			foreach (StiReportSummaryBand reportSummary in masterDataBand.DataBandInfoV2.ReportSummaries)
			{
				if (!masterDataBand.IsEmpty && reportSummary.KeepReportSummaryTogether)
				{
					result = true;
				}
			}
			return result;
		}
	}

	public void Block(StiDataBand masterDataBand)
	{
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiDynamicBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				groupHeader.Blocked = true;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupFooters).SyncRoot)
		{
			foreach (StiDynamicBand groupFooter in masterDataBand.DataBandInfoV2.GroupFooters)
			{
				groupFooter.Blocked = true;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.Headers).SyncRoot)
		{
			foreach (StiDynamicBand header in masterDataBand.DataBandInfoV2.Headers)
			{
				header.Blocked = true;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnAllPages).SyncRoot)
		{
			foreach (StiDynamicBand footersOnAllPage in masterDataBand.DataBandInfoV2.FootersOnAllPages)
			{
				footersOnAllPage.Blocked = true;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnLastPage).SyncRoot)
		{
			foreach (StiDynamicBand item in masterDataBand.DataBandInfoV2.FootersOnLastPage)
			{
				item.Blocked = true;
			}
		}
		if (masterDataBand.DataBandInfoV2.ReportSummaries != null)
		{
			lock (((ICollection)masterDataBand.DataBandInfoV2.ReportSummaries).SyncRoot)
			{
				foreach (StiReportSummaryBand reportSummary in masterDataBand.DataBandInfoV2.ReportSummaries)
				{
					reportSummary.Blocked = true;
				}
			}
		}
		masterDataBand.Blocked = true;
	}

	public void UnBlock(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.GroupHeaders == null || masterDataBand.DataBandInfoV2.GroupFooters == null || masterDataBand.DataBandInfoV2.Headers == null || masterDataBand.DataBandInfoV2.FootersOnAllPages == null || masterDataBand.DataBandInfoV2.FootersOnLastPage == null)
		{
			return;
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
		{
			foreach (StiDynamicBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
			{
				groupHeader.Blocked = false;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.GroupFooters).SyncRoot)
		{
			foreach (StiDynamicBand groupFooter in masterDataBand.DataBandInfoV2.GroupFooters)
			{
				groupFooter.Blocked = false;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.Headers).SyncRoot)
		{
			foreach (StiDynamicBand header in masterDataBand.DataBandInfoV2.Headers)
			{
				header.Blocked = false;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnAllPages).SyncRoot)
		{
			foreach (StiDynamicBand footersOnAllPage in masterDataBand.DataBandInfoV2.FootersOnAllPages)
			{
				footersOnAllPage.Blocked = false;
			}
		}
		lock (((ICollection)masterDataBand.DataBandInfoV2.FootersOnLastPage).SyncRoot)
		{
			foreach (StiDynamicBand item in masterDataBand.DataBandInfoV2.FootersOnLastPage)
			{
				item.Blocked = false;
			}
		}
		masterDataBand.Blocked = false;
	}

	public void CheckHierarchicalHeaders(StiDataBand masterDataBand)
	{
		if (!(masterDataBand is StiHierarchicalBand))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		if (!masterDataBand.IsDataSourceEmpty)
		{
			num = masterDataBand.DataSource.GetLevel();
			num2 = masterDataBand.DataSource.Position;
		}
		if (!masterDataBand.IsBusinessObjectEmpty)
		{
			num = masterDataBand.BusinessObject.GetLevel();
			num2 = masterDataBand.BusinessObject.Position;
		}
		if (num2 == 0)
		{
			for (int i = 0; i <= num; i++)
			{
				if (i != num)
				{
					((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = i;
				}
				RenderHierarchicalHeaders(masterDataBand, allowIndent: true, i);
				((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = -1;
			}
			return;
		}
		int num3 = 0;
		if (!masterDataBand.IsDataSourceEmpty)
		{
			masterDataBand.DataSource.Position--;
			num3 = masterDataBand.DataSource.GetLevel();
			masterDataBand.DataSource.Position++;
		}
		if (!masterDataBand.IsBusinessObjectEmpty)
		{
			masterDataBand.BusinessObject.SetPrevValue();
			num3 = masterDataBand.BusinessObject.GetLevel();
			masterDataBand.BusinessObject.RestoreCurrentValue();
		}
		if (num3 >= num)
		{
			return;
		}
		for (int num4 = num; num4 > num3; num4--)
		{
			if (num4 != num)
			{
				((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = num4;
			}
			RenderHierarchicalHeaders(masterDataBand, allowIndent: true, num4);
			((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = -1;
		}
		((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = -1;
	}

	public void CheckHierarchicalFooters(StiDataBand masterDataBand)
	{
		if (!(masterDataBand is StiHierarchicalBand))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (!masterDataBand.IsDataSourceEmpty)
		{
			num = masterDataBand.DataSource.Position;
			num2 = masterDataBand.DataSource.Count;
			num3 = masterDataBand.DataSource.GetLevel();
		}
		if (!masterDataBand.IsBusinessObjectEmpty)
		{
			num = masterDataBand.BusinessObject.Position;
			num2 = masterDataBand.BusinessObject.Count;
			num3 = masterDataBand.BusinessObject.GetLevel();
		}
		if (num == num2 - 1)
		{
			for (int num4 = num3; num4 >= 0; num4--)
			{
				if (num4 != num3)
				{
					((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = num4;
				}
				RenderHierarchicalFooters(masterDataBand, allowIndent: true, num4);
			}
			((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = -1;
			return;
		}
		int num5 = 0;
		if (!masterDataBand.IsDataSourceEmpty)
		{
			masterDataBand.DataSource.Position++;
			num5 = masterDataBand.DataSource.GetLevel();
			masterDataBand.DataSource.Position--;
		}
		if (!masterDataBand.IsBusinessObjectEmpty)
		{
			masterDataBand.BusinessObject.SetNextValue();
			num5 = masterDataBand.BusinessObject.GetLevel();
			masterDataBand.BusinessObject.RestoreCurrentValue();
		}
		if (num5 >= num3)
		{
			return;
		}
		for (int num6 = num3; num6 > num5; num6--)
		{
			if (num6 != num3)
			{
				((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = num6;
			}
			RenderHierarchicalFooters(masterDataBand, allowIndent: true, num6);
		}
		((StiHierarchicalBand)masterDataBand).HierarchicalBandInfoV2.SpecifiedLevel = -1;
	}

	public void RenderBand(StiDataBand masterDataBand, StiBand band)
	{
		RenderBand(masterDataBand, band, ignorePageBreaks: false, allowRenderingEvents: false);
	}

	public void RenderBand(StiDataBand masterDataBand, StiBand band, bool ignorePageBreaks, bool allowRenderingEvents)
	{
		RegisterEmptyBands(masterDataBand);
		if (!(band is StiGroupHeaderBand))
		{
			if (band != masterDataBand)
			{
				band.ParentBookmark = masterDataBand.ParentBookmark;
				band.ParentPointer = masterDataBand.ParentPointer;
			}
			bool flag = false;
			if (band.Enabled || StiOptions.Engine.ShowBookmarkToDisabledBand)
			{
				flag = band.DoBookmark();
			}
			band.DoPointer(!flag);
		}
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(band);
		if (masterDataBand.Report.Engine.ContainerForRender != null)
		{
			masterDataBand.Report.Engine.RenderBand(band, ignorePageBreaks, allowRenderingEvents);
		}
	}

	public void RenderColumns(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		masterDataBand.Report.Engine.ColumnsOnDataBand.RenderColumns(masterDataBand);
	}

	public void RegisterEmptyBands(StiDataBand masterDataBand)
	{
		masterDataBand.Report.Engine.EmptyBands.Register(masterDataBand.DataBandInfoV2.EmptyBands);
	}

	public static bool IsCollapsed(StiContainer masterDataBand, bool isRendering)
	{
		if (StiOptions.Engine.ForceDisableCollapsing)
		{
			return false;
		}
		if (masterDataBand.Interaction == null || (masterDataBand.Interaction is StiBandInteraction && !((StiBandInteraction)masterDataBand.Interaction).CollapsingEnabled))
		{
			return false;
		}
		bool result = masterDataBand.CollapsedValue is bool && (bool)masterDataBand.CollapsedValue;
		if (masterDataBand.Report.InteractionCollapsingStates == null)
		{
			return result;
		}
		Hashtable hashtable = null;
		if (masterDataBand.CollapsingTreePath != null)
		{
			hashtable = masterDataBand.Report.InteractionCollapsingStates[masterDataBand.CollapsingTreePath + masterDataBand.Name] as Hashtable;
		}
		if (hashtable == null)
		{
			hashtable = masterDataBand.Report.InteractionCollapsingStates[masterDataBand.Name] as Hashtable;
		}
		if (hashtable == null)
		{
			return result;
		}
		if (!(hashtable[masterDataBand.CollapsingIndex] is bool))
		{
			return result;
		}
		return (bool)hashtable[masterDataBand.CollapsingIndex];
	}

	public override void SetReportVariables(StiComponent masterComp)
	{
		StiDataBand stiDataBand = masterComp as StiDataBand;
		stiDataBand.Report.Line = stiDataBand.Line;
		stiDataBand.Report.LineThrough = stiDataBand.LineThrough;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiDataBand stiDataBand = masterComp as StiDataBand;
		FindHeaders(stiDataBand);
		FindHierarchicalHeaders(stiDataBand);
		FindFooters(stiDataBand);
		FindHierarchicalFooters(stiDataBand);
		FindEmptyBands(stiDataBand);
		FindDetailDataBands(stiDataBand);
		FindDetails(stiDataBand);
		FindSubReports(stiDataBand);
		FindGroupHeaders(stiDataBand);
		FindGroupFooters(stiDataBand);
		stiDataBand.DataBandInfoV2.GroupHeaderResults = new bool[stiDataBand.DataBandInfoV2.GroupHeaders.Count];
		stiDataBand.DataBandInfoV2.GroupFooterResults = new bool[stiDataBand.DataBandInfoV2.GroupHeaders.Count];
		StiFilterHelper.SetFilter(masterComp);
		if (!stiDataBand.IsBusinessObjectEmpty && ((stiDataBand.Sort != null && stiDataBand.Sort.Length != 0) || (stiDataBand.FilterOn && stiDataBand.FilterMethodHandler != null) || (masterComp.Report != null && masterComp.Report.CalculationMode == StiCalculationMode.Interpretation && stiDataBand.FilterOn && stiDataBand.Filters.Count > 0) || stiDataBand is StiHierarchicalBand))
		{
			stiDataBand.BusinessObject.OwnerBand = stiDataBand;
		}
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiDataBand stiDataBand = masterComp as StiDataBand;
		stiDataBand.DataBandInfoV2.Headers = null;
		stiDataBand.DataBandInfoV2.HierarchicalHeaders = null;
		stiDataBand.DataBandInfoV2.FootersOnAllPages = null;
		stiDataBand.DataBandInfoV2.FootersOnLastPage = null;
		stiDataBand.DataBandInfoV2.HierarchicalFooters = null;
		stiDataBand.DataBandInfoV2.EmptyBands = null;
		stiDataBand.DataBandInfoV2.FootersOnLastPage = null;
		stiDataBand.DataBandInfoV2.DetailDataBands = null;
		stiDataBand.DataBandInfoV2.SubReports = null;
		stiDataBand.DataBandInfoV2.ReportTitles = null;
		stiDataBand.DataBandInfoV2.ReportSummaries = null;
		stiDataBand.DataBandInfoV2.Headers = null;
		stiDataBand.DataBandInfoV2.GroupHeaders = null;
		stiDataBand.DataBandInfoV2.GroupFooters = null;
	}

	private static void InvokeCollapsedEvent(StiContainer masterDataBand)
	{
		StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
		if (masterDataBand is StiGroupHeaderBand)
		{
			((StiGroupHeaderBand)masterDataBand).InvokeGetCollapsed(stiValueEventArgs);
			((StiGroupHeaderBand)masterDataBand).CollapsedValue = stiValueEventArgs.Value;
		}
		else if (masterDataBand is StiDataBand)
		{
			((StiDataBand)masterDataBand).InvokeGetCollapsed(stiValueEventArgs);
			((StiDataBand)masterDataBand).CollapsedValue = stiValueEventArgs.Value;
		}
		if (StiOptions.Engine.FixFirstCollapsingBandState && stiValueEventArgs.Value != null && masterDataBand.Report.InteractionCollapsingStates == null)
		{
			masterDataBand.Report.InteractionCollapsingStates = new Hashtable();
			Hashtable hashtable = new Hashtable();
			string key = masterDataBand.Name;
			if (masterDataBand.CollapsingTreePath != null)
			{
				key = masterDataBand.CollapsingTreePath + masterDataBand.Name;
			}
			masterDataBand.Report.InteractionCollapsingStates[key] = hashtable;
			hashtable[masterDataBand.CollapsingIndex] = stiValueEventArgs.Value;
		}
	}

	public override StiComponent Render(StiComponent masterComp)
	{
		return null;
	}

	public virtual void RenderMaster(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV2.GroupHeaders == null || masterDataBand.DataBandInfoV2.GroupFooters == null || masterDataBand.DataBandInfoV2.Headers == null || masterDataBand.DataBandInfoV2.FootersOnAllPages == null || masterDataBand.DataBandInfoV2.FootersOnLastPage == null)
		{
			return;
		}
		bool isCrossBandsMode = masterDataBand.Report.Engine.IsCrossBandsMode;
		bool isCrossBandsMode2 = masterDataBand is StiCrossDataBand;
		masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
		masterDataBand.Report.Engine.IsCrossBandsMode = isCrossBandsMode2;
		masterDataBand.DataBandInfoV2.StoredParentBookmark = masterDataBand.ParentBookmark;
		masterDataBand.DataBandInfoV2.StoredParentPointer = masterDataBand.ParentPointer;
		masterDataBand.Report.Engine.IsDynamicBookmarksMode = true;
		masterDataBand.InvokeBeginRender();
		Block(masterDataBand);
		LinkGroupHeadersAndGroupFooters(masterDataBand);
		try
		{
			masterDataBand.Line = 1;
			if (masterDataBand.MasterComponent == null)
			{
				masterDataBand.LineThrough = 1;
			}
			lock (((ICollection)masterDataBand.DataBandInfoV2.GroupHeaders).SyncRoot)
			{
				foreach (StiGroupHeaderBand groupHeader in masterDataBand.DataBandInfoV2.GroupHeaders)
				{
					groupHeader.Line = 1;
				}
			}
			bool flag = masterDataBand.MasterComponent == null || !StiOptions.Engine.OptimizeDetailDataFiltering;
			if (masterDataBand.MasterComponent is StiDataBand { IsDataSourceEmpty: not false, IsBusinessObjectEmpty: not false })
			{
				flag = true;
			}
			if (!flag)
			{
				if (masterDataBand.Report.Engine.HashDataSourceReferencesCounter == null)
				{
					flag = true;
				}
				else
				{
					if (!masterDataBand.IsDataSourceEmpty)
					{
						object obj = masterDataBand.Report.Engine.HashDataSourceReferencesCounter[masterDataBand.DataSourceName];
						if (obj != null && (int)obj > 1)
						{
							flag = true;
						}
					}
					if (!masterDataBand.IsBusinessObjectEmpty)
					{
						object obj2 = masterDataBand.Report.Engine.HashDataSourceReferencesCounter[masterDataBand.BusinessObject.Name];
						if (obj2 != null && (int)obj2 > 1)
						{
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				StiDataHelper.SetData(masterDataBand, reinit: false);
			}
			if (!flag || masterDataBand.BusinessObject != null)
			{
				masterDataBand.First();
			}
			masterDataBand.Report.Engine.ColumnsOnDataBand.Enabled = false;
			RenderReportTitles(masterDataBand);
			bool[] keepHeaders = new bool[masterDataBand.DataBandInfoV2.Headers.Count];
			RenderHeaders(masterDataBand, keepHeaders);
			RenderFootersOnAllPages(masterDataBand);
			StartBands(masterDataBand, masterDataBand.DataBandInfoV2.Headers);
			StartBands(masterDataBand, masterDataBand.DataBandInfoV2.FootersOnAllPages);
			StartBands(masterDataBand, masterDataBand.DataBandInfoV2.GroupHeaders);
			StartBand(masterDataBand, masterDataBand);
			if (masterDataBand.DataBandInfoV2.GroupHeaders.Count == 0)
			{
				RenderColumns(masterDataBand);
			}
			if (masterDataBand.Interaction is StiBandInteraction && masterDataBand.DataBandInfoV2.DetailDataBands.Count == 0 && !(masterDataBand is StiHierarchicalBand))
			{
				((StiBandInteraction)masterDataBand.Interaction).CollapsingEnabled = false;
			}
			masterDataBand.First();
			int num = 0;
			object containerForRender = masterDataBand.Report.Engine.ContainerForRender;
			int num2 = 0;
			try
			{
				if (!string.IsNullOrWhiteSpace(masterDataBand.LimitRows) && masterDataBand.Columns < 2)
				{
					object value = StiParser.ParseTextValue("{" + masterDataBand.LimitRows + "}", masterDataBand, new StiParserParameters
					{
						ExecuteIfStoreToPrint = true
					});
					num2 = Convert.ToInt32(value);
				}
			}
			catch (Exception ex)
			{
				string text = $"Expression in LimitRows property of '{masterDataBand.Name}' can't be evaluated! {ex.Message}";
				StiLogService.Write(((object)masterDataBand).GetType(), text);
				StiLogService.Write(((object)masterDataBand).GetType(), ex.Message);
				masterDataBand.Report.WriteToReportRenderingMessages(text);
			}
			StiBusinessObject businessObject = masterDataBand.BusinessObject;
			while (!masterDataBand.IsEof)
			{
				if (num2 > 0)
				{
					if (containerForRender != masterDataBand.Report.Engine.ContainerForRender)
					{
						containerForRender = masterDataBand.Report.Engine.ContainerForRender;
						num = 1;
					}
					num++;
					if (num > num2)
					{
						masterDataBand.Report.Engine.NewDestination();
						num = 1;
					}
				}
				int num3 = (masterDataBand.CollapsingIndex = masterDataBand.Position);
				if (masterDataBand.Interaction != null && ((StiBandInteraction)masterDataBand.Interaction).CollapsingEnabled)
				{
					string text2 = null;
					StiDataBand stiDataBand2 = masterDataBand;
					while (stiDataBand2.MasterComponent != null && stiDataBand2.MasterComponent is StiDataBand)
					{
						stiDataBand2 = stiDataBand2.MasterComponent as StiDataBand;
						text2 = $"{stiDataBand2.Position}:{text2}";
					}
					if (text2 != null)
					{
						masterDataBand.CollapsingTreePath = text2;
					}
				}
				if (masterDataBand.Columns < 2)
				{
					masterDataBand.Report.Engine.ColumnsOnDataBand.Enabled = false;
				}
				RenderGroupHeaders(masterDataBand);
				bool flag2 = IsDenyDetailsOnFirstPage(masterDataBand);
				if (!flag2)
				{
					AddKeepDetails(masterDataBand);
				}
				masterDataBand.SetReportVariables();
				bool flag3 = false;
				if (IsPrintIfDetailEmpty(masterDataBand))
				{
					InvokeCollapsedEvent(masterDataBand);
					CheckHierarchicalHeaders(masterDataBand);
					RenderBand(masterDataBand, masterDataBand, ignorePageBreaks: false, allowRenderingEvents: true);
					flag3 = true;
				}
				if (flag3)
				{
					RemoveKeepHeaders(masterDataBand, keepHeaders);
				}
				bool silentMode = masterDataBand.Report.Engine.SilentMode;
				if (IsCollapsed(masterDataBand, isRendering: true))
				{
					masterDataBand.Report.Engine.SilentMode = true;
				}
				if (!flag2)
				{
					RemoveKeepDetailsRow(masterDataBand);
				}
				RenderDetailDataBands(masterDataBand);
				masterDataBand.Report.Engine.SilentMode = silentMode;
				if (flag3)
				{
					CheckHierarchicalFooters(masterDataBand);
				}
				if (!flag2)
				{
					RemoveKeepDetails(masterDataBand);
				}
				RemoveKeepGroupHeaders(masterDataBand);
				RenderGroupFooters(masterDataBand);
				if (businessObject != null && !businessObject.isEnumeratorCreated)
				{
					businessObject.SetDetails();
				}
				if (masterDataBand.Position < num3)
				{
					masterDataBand.Position = num3;
				}
				masterDataBand.Line++;
				masterDataBand.LineThrough++;
				masterDataBand.Next();
			}
			if (masterDataBand.Report.Engine.ColumnsOnDataBand.Enabled)
			{
				StiColumnsContainer columns = masterDataBand.Report.Engine.ColumnsOnDataBand.GetColumns();
				masterDataBand.Report.Engine.CheckBreakColumnsContainer(columns);
			}
			masterDataBand.Report.Engine.Threads.SelectThreadFromContainer(masterDataBand);
			RenderMarkerFootersOnAllPages(masterDataBand);
			RenderFootersOnLastPage(masterDataBand);
			if (CheckKeepReportSummaryTogether(masterDataBand))
			{
				RenderReportSummaries(masterDataBand);
				EndBands(masterDataBand);
			}
			else
			{
				EndBands(masterDataBand);
				RenderReportSummaries(masterDataBand);
			}
		}
		finally
		{
			ResetLinkGroupHeadersAndGroupFooters(masterDataBand);
			UnBlock(masterDataBand);
			masterDataBand.Report.Engine.IsCrossBandsMode = isCrossBandsMode;
			masterDataBand.Report.Engine.IsDynamicBookmarksMode = false;
		}
		masterDataBand.InvokeEndRender();
	}
}
