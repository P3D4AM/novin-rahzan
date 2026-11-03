using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;

namespace Stimulsoft.Report.Engine;

public class StiDataBandV1Builder : StiBandV1Builder
{
	public void SecondPassBreak(StiDataBand masterDataBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		masterDataBand.DataBandInfoV1.IsFirstPassOfBreak = true;
		if (masterDataBand.DataBandInfoV1.BreakableComps != null && masterDataBand.DataBandInfoV1.BreakableComps.Count > 0)
		{
			masterDataBand.DataBandInfoV1.IsFirstPassOfBreak = false;
			StiComponentDivider.ProcessPreviousBreakedContainer(renderedComponent as StiContainer, masterDataBand.DataBandInfoV1.BreakableComps, ref freeSpace);
			masterDataBand.DataBandInfoV1.BreakableComps = null;
		}
	}

	public bool FirstPassBreak(StiDataBand masterDataBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		renderedComponent.Height += freeSpace;
		masterDataBand.DataBandInfoV1.BreakableComps = StiComponentDivider.BreakContainer(renderedComponent as StiContainer);
		if (renderedComponent.Parent is StiPage && !masterDataBand.DataBandInfoV1.IsFirstPassOfBreak && (renderedComponent as StiContainer).Components.Count == 0)
		{
			(renderedComponent as StiContainer).Components.AddRange(masterDataBand.DataBandInfoV1.BreakableComps);
			masterDataBand.DataBandInfoV1.BreakableComps = null;
			masterDataBand.DataBandInfoV1.IsFirstPassOfBreak = true;
			freeSpace = 0.0;
			return false;
		}
		return true;
	}

	public virtual StiComponentsCollection GetGroupHeaders(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		int num = masterDataBand.Parent.Components.IndexOf(masterDataBand) - 1;
		if (masterDataBand is StiTable && !masterDataBand.Enabled)
		{
			string value = masterDataBand.Name + "_";
			while (num >= 0 && masterDataBand.Parent.Components[num].Name.StartsWith(value))
			{
				num--;
			}
		}
		while (num >= 0)
		{
			StiComponent stiComponent = masterDataBand.Parent.Components[num];
			if (!(stiComponent is StiHeaderBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand))
			{
				if (!(stiComponent is StiGroupHeaderBand))
				{
					break;
				}
				stiComponentsCollection.Insert(0, stiComponent);
			}
			num--;
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetGroupFooters(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count; i++)
		{
			if (!(masterDataBand.Parent.Components[i] is StiFooterBand) && !(masterDataBand.Parent.Components[i] is StiChildBand) && !(masterDataBand.Parent.Components[i] is StiEmptyBand))
			{
				if (!(masterDataBand.Parent.Components[i] is StiGroupFooterBand))
				{
					break;
				}
				stiComponentsCollection.Add(masterDataBand.Parent.Components[i]);
			}
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetDetails(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		if (masterDataBand.Parent != null)
		{
			StiComponentsCollection components = masterDataBand.Parent.Components;
			foreach (StiComponent item in components)
			{
				if (!(item is StiEmptyBand) && item != masterDataBand && item is IStiMasterComponent && ((IStiMasterComponent)item).MasterComponent == masterDataBand)
				{
					stiComponentsCollection.Add(item);
				}
			}
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetDataDetails(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		if (masterDataBand.Parent != null)
		{
			StiComponentsCollection components = masterDataBand.Page.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item is IStiMasterComponent && ((IStiMasterComponent)item).MasterComponent == masterDataBand)
				{
					stiComponentsCollection.Add(item);
				}
			}
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetHeaders(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		for (int num = masterDataBand.Parent.Components.IndexOf(masterDataBand) - 1; num >= 0; num--)
		{
			if (!(masterDataBand.Parent.Components[num] is StiGroupHeaderBand) && !(masterDataBand.Parent.Components[num] is StiChildBand) && !(masterDataBand.Parent.Components[num] is StiEmptyBand))
			{
				if (!(masterDataBand.Parent.Components[num] is StiHeaderBand))
				{
					break;
				}
				stiComponentsCollection.Insert(0, masterDataBand.Parent.Components[num]);
			}
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetFooters(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		for (int i = masterDataBand.Parent.Components.IndexOf(masterDataBand) + 1; i < masterDataBand.Parent.Components.Count; i++)
		{
			if (!(masterDataBand.Parent.Components[i] is StiGroupFooterBand) && !(masterDataBand.Parent.Components[i] is StiChildBand) && !(masterDataBand.Parent.Components[i] is StiEmptyBand))
			{
				if (!(masterDataBand.Parent.Components[i] is StiFooterBand))
				{
					break;
				}
				stiComponentsCollection.Add(masterDataBand.Parent.Components[i]);
			}
		}
		return stiComponentsCollection;
	}

	public virtual StiComponentsCollection GetSubReports(StiDataBand masterDataBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component in masterDataBand.Components)
		{
			if (component is StiSubReport)
			{
				stiComponentsCollection.Add(component);
			}
		}
		return stiComponentsCollection;
	}

	internal bool GetSubReportRenderResult(StiDataBand masterDataBand)
	{
		foreach (StiComponent subReportsComponent in masterDataBand.DataBandInfoV1.SubReportsComponents)
		{
			if (!((StiSubReport)subReportsComponent).SubReportInfoV1.SubReportRenderResult)
			{
				return false;
			}
		}
		return true;
	}

	public void RemoveRenderedHeaders(StiDataBand masterDataBand, StiContainer outContainer)
	{
		foreach (StiComponent renderedHeader in masterDataBand.DataBandInfoV1.RenderedHeaders)
		{
			IncFreeSpace(masterDataBand, renderedHeader);
			outContainer.Components.Remove(renderedHeader);
		}
		foreach (StiHeaderBand headerComponent in masterDataBand.DataBandInfoV1.HeaderComponents)
		{
			headerComponent.Prepare();
		}
	}

	public virtual bool RenderHeaders(StiDataBand masterDataBand, StiContainer outContainer)
	{
		masterDataBand.DataBandInfoV1.RenderedHeaders.Clear();
		if (masterDataBand.DataBandInfoV1.HeaderComponents != null)
		{
			bool isEof = masterDataBand.IsEof;
			try
			{
				foreach (StiHeaderBand headerComponent in masterDataBand.DataBandInfoV1.HeaderComponents)
				{
					StiBrush brush = headerComponent.Brush;
					bool enabled = headerComponent.Enabled;
					try
					{
						headerComponent.InvokeBeforePrint(headerComponent, EventArgs.Empty);
						if (headerComponent.IsEnabled && (!headerComponent.IsRendered || headerComponent.PrintOnAllPages) && (!masterDataBand.IsEmpty || headerComponent.PrintIfEmpty))
						{
							if (headerComponent.StartNewPage && !headerComponent.HeaderBandInfoV1.StartNewPageProcessed)
							{
								double num = 100.0 * masterDataBand.DataBandInfoV1.FreeSpace / masterDataBand.Page.Height;
								if ((double)headerComponent.StartNewPageIfLessThan > num || headerComponent.StartNewPageIfLessThan == 100f)
								{
									headerComponent.HeaderBandInfoV1.StartNewPageProcessed = true;
									return false;
								}
							}
							if (!headerComponent.IsRendered && headerComponent.ResetPageNumber)
							{
								masterDataBand.Report.EngineV1.ResetPageNumber();
							}
							StiComponent renderedComponent = null;
							headerComponent.ParentBookmark = masterDataBand.CurrentBookmark;
							headerComponent.Render(ref renderedComponent, outContainer);
							if (renderedComponent != null)
							{
								masterDataBand.DataBandInfoV1.RenderedHeaders.Add(renderedComponent);
								if (headerComponent.PrintAtBottom)
								{
									AddBottomRenderedHeaders(masterDataBand, renderedComponent, headerComponent);
								}
								DecFreeSpace(masterDataBand, renderedComponent);
								double freeSpace = masterDataBand.DataBandInfoV1.FreeSpace;
								StiHeaderBandV1Builder.SecondPassBreak(headerComponent, renderedComponent, headerComponent, ref freeSpace);
								masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
								if (CheckFreeSpace(masterDataBand, outContainer))
								{
									if (headerComponent.HeaderBandInfoV1.ForceCanBreak)
									{
										double freeSpace2 = masterDataBand.DataBandInfoV1.FreeSpace;
										StiHeaderBandV1Builder.FirstPassBreak(headerComponent, renderedComponent, headerComponent, ref freeSpace2);
										masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
										headerComponent.Prepare();
									}
									else
									{
										if (masterDataBand.DataBandInfoV1.FirstRow && IsKeepHeaderTogether(masterDataBand) && !masterDataBand.DataBandInfoV1.LatestDataBandBreaked)
										{
											RemoveRenderedHeaders(masterDataBand, outContainer);
										}
										else
										{
											headerComponent.Prepare();
										}
										if (outContainer.Components.IndexOf(renderedComponent) != -1)
										{
											outContainer.Components.Remove(renderedComponent);
										}
									}
									return false;
								}
							}
						}
						headerComponent.InvokeAfterPrint(headerComponent, EventArgs.Empty);
					}
					finally
					{
						headerComponent.Brush = brush;
						headerComponent.Enabled = enabled;
					}
				}
			}
			finally
			{
				if (!(masterDataBand is StiCrossDataBand))
				{
					masterDataBand.IsEof = isEof;
				}
			}
		}
		return true;
	}

	internal bool IsKeepHeaderTogether(StiDataBand masterDataBand)
	{
		if (masterDataBand.KeepHeaderTogether)
		{
			return !masterDataBand.DataBandInfoV1.CrossTabExistOnDataBand;
		}
		return false;
	}

	public bool RenderFootersOnAllPagesFirst(StiDataBand masterDataBand, ref StiComponentsCollection footers, ref StiComponentsCollection parentFooters, StiContainer outContainer)
	{
		if (masterDataBand.DataBandInfoV1.FooterComponents != null)
		{
			bool isEof = masterDataBand.IsEof;
			try
			{
				foreach (StiFooterBand footerComponent in masterDataBand.DataBandInfoV1.FooterComponents)
				{
					StiBrush brush = footerComponent.Brush;
					bool enabled = footerComponent.Enabled;
					try
					{
						if (!footerComponent.PrintOnAllPages || (masterDataBand.IsEmpty && !footerComponent.PrintIfEmpty))
						{
							continue;
						}
						footerComponent.InvokeBeforePrint(footerComponent, EventArgs.Empty);
						if (footerComponent.IsEnabled)
						{
							StiComponent renderedComponent = null;
							footerComponent.ParentBookmark = masterDataBand.CurrentBookmark;
							footerComponent.Render(ref renderedComponent, outContainer);
							if (renderedComponent != null)
							{
								DecFreeSpace(masterDataBand, renderedComponent);
								footers.Add(renderedComponent);
								parentFooters.Add(footerComponent);
								if (CheckFreeSpace(masterDataBand, outContainer))
								{
									IncFreeSpace(masterDataBand, renderedComponent);
									footers.Remove(renderedComponent);
									outContainer.Components.Remove(renderedComponent);
									return false;
								}
							}
						}
						footerComponent.InvokeAfterPrint(footerComponent, EventArgs.Empty);
					}
					finally
					{
						footerComponent.Brush = brush;
						footerComponent.Enabled = enabled;
					}
				}
			}
			finally
			{
				if (!(masterDataBand is StiCrossDataBand))
				{
					masterDataBand.IsEof = isEof;
				}
			}
		}
		return true;
	}

	public void RenderFootersOnAllPagesSecond(StiDataBand masterDataBand, ref StiComponentsCollection footers, ref StiComponentsCollection parentFooters, StiContainer outContainer)
	{
		if (masterDataBand.DataBandInfoV1.FooterComponents == null)
		{
			return;
		}
		bool isEof = masterDataBand.IsEof;
		try
		{
			foreach (StiComponent footer in footers)
			{
				outContainer.Components.Remove(footer);
			}
			int num = 0;
			foreach (StiContainer footer2 in footers)
			{
				StiBrush brush = footer2.Brush;
				bool enabled = footer2.Enabled;
				try
				{
					StiFooterBand stiFooterBand = null;
					foreach (StiFooterBand footerComponent in masterDataBand.DataBandInfoV1.FooterComponents)
					{
						if (footerComponent.Name == footer2.Name)
						{
							stiFooterBand = footerComponent;
							break;
						}
					}
					StiComponent renderedComponent = null;
					footer2.ParentBookmark = masterDataBand.DataBandInfoV1.ResParentBookmark;
					if (stiFooterBand is StiColumnFooterBand)
					{
						renderedComponent = footer2;
						outContainer.Components.Add(renderedComponent);
					}
					else
					{
						footer2.InternalRender(ref renderedComponent, outContainer);
					}
					StiFooterBand stiFooterBand3 = parentFooters[num++] as StiFooterBand;
					if (stiFooterBand3.PrintAtBottom)
					{
						AddBottomRenderedFooters(masterDataBand, renderedComponent, stiFooterBand3);
					}
				}
				finally
				{
					footer2.Brush = brush;
					footer2.Enabled = enabled;
				}
			}
		}
		finally
		{
			if (!(masterDataBand is StiCrossDataBand))
			{
				masterDataBand.IsEof = isEof;
			}
		}
	}

	private void AddAllFootersToRemittedCollection(StiDataBand masterDataBand, int index, ref StiComponent outFooter, ref StiContainer outContainer)
	{
		for (int i = index; i < masterDataBand.DataBandInfoV1.FooterComponents.Count; i++)
		{
			StiFooterBand stiFooterBand = masterDataBand.DataBandInfoV1.FooterComponents[i] as StiFooterBand;
			if (!stiFooterBand.PrintOnAllPages && masterDataBand.IsEof && stiFooterBand.IsEnabled)
			{
				outFooter = null;
				stiFooterBand.Render(ref outFooter, outContainer);
				outContainer.Components.Remove(outFooter);
				masterDataBand.DataBandInfoV1.RemmitedCollection.Add(outFooter);
			}
		}
	}

	public bool RenderFootersSecond(StiDataBand masterDataBand, out ArrayList renderedFooters, StiContainer outContainer, ref bool newPageStarted, ref bool isBreaked)
	{
		renderedFooters = new ArrayList();
		bool result = true;
		if (masterDataBand.DataBandInfoV1.FooterComponents != null)
		{
			bool isEof = masterDataBand.IsEof;
			try
			{
				for (int i = 0; i < masterDataBand.DataBandInfoV1.FooterComponents.Count; i++)
				{
					StiFooterBand stiFooterBand = masterDataBand.DataBandInfoV1.FooterComponents[i] as StiFooterBand;
					StiBrush brush = stiFooterBand.Brush;
					bool enabled = stiFooterBand.Enabled;
					try
					{
						if (stiFooterBand.PrintOnAllPages || !masterDataBand.IsEof || (masterDataBand.IsEmpty && !stiFooterBand.PrintIfEmpty))
						{
							continue;
						}
						stiFooterBand.InvokeBeforePrint(stiFooterBand, EventArgs.Empty);
						if (stiFooterBand.IsEnabled)
						{
							StiComponent renderedComponent = null;
							stiFooterBand.ParentBookmark = masterDataBand.DataBandInfoV1.ResParentBookmark;
							stiFooterBand.Render(ref renderedComponent, outContainer);
							if (stiFooterBand.PrintAtBottom)
							{
								AddBottomRenderedFooters(masterDataBand, renderedComponent, stiFooterBand);
							}
							renderedFooters.Add(renderedComponent);
							DecFreeSpace(masterDataBand, renderedComponent);
							double freeSpace = masterDataBand.DataBandInfoV1.FreeSpace;
							SecondPassBreak(masterDataBand, renderedComponent, stiFooterBand, ref freeSpace);
							masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
							if (CheckFreeSpace(masterDataBand, outContainer))
							{
								if (stiFooterBand.FooterBandInfoV1.ForceCanBreak)
								{
									double height = renderedComponent.Height;
									double freeSpace2 = masterDataBand.DataBandInfoV1.FreeSpace;
									StiFooterBandV1Builder.FirstPassBreak(stiFooterBand, renderedComponent, stiFooterBand, ref freeSpace2);
									masterDataBand.DataBandInfoV1.FreeSpace = freeSpace2;
									StiContainer stiContainer = ((StiService)renderedComponent).Clone() as StiContainer;
									stiContainer.Components.Clear();
									stiContainer.Components.AddRange(stiFooterBand.FooterBandInfoV1.BreakableComps);
									renderedFooters.Remove(renderedComponent);
									renderedComponent = stiContainer;
									masterDataBand.DataBandInfoV1.RemmitedCollection.Add(renderedComponent);
									renderedComponent.Height = height - renderedComponent.Height;
									i++;
									isBreaked = true;
								}
								else
								{
									outContainer.Components.Remove(renderedComponent);
									renderedFooters.Remove(renderedComponent);
								}
								if (!(masterDataBand is StiCrossDataBand))
								{
									masterDataBand.IsEof = isEof;
								}
								AddAllFootersToRemittedCollection(masterDataBand, i, ref renderedComponent, ref outContainer);
								result = false;
								masterDataBand.IsRendered = false;
								return result;
							}
							if (stiFooterBand.StartNewPage)
							{
								double num = 100.0 * masterDataBand.DataBandInfoV1.FreeSpace / masterDataBand.Page.Height;
								if ((double)stiFooterBand.StartNewPageIfLessThan > num || stiFooterBand.StartNewPageIfLessThan == 100f)
								{
									newPageStarted = true;
									outContainer.Components.Remove(renderedComponent);
									renderedFooters.Remove(renderedComponent);
									if (!(masterDataBand is StiCrossDataBand))
									{
										masterDataBand.IsEof = isEof;
									}
									AddAllFootersToRemittedCollection(masterDataBand, i, ref renderedComponent, ref outContainer);
									return false;
								}
							}
						}
						stiFooterBand.InvokeAfterPrint(stiFooterBand, EventArgs.Empty);
					}
					finally
					{
						stiFooterBand.Brush = brush;
						stiFooterBand.Enabled = enabled;
					}
				}
			}
			finally
			{
				if (!(masterDataBand is StiCrossDataBand))
				{
					masterDataBand.IsEof = isEof;
				}
			}
		}
		return result;
	}

	internal void RemoveFromContainerAt(StiDataBand masterDataBand, StiContainer outContainer, int startIndex)
	{
		int count = outContainer.Components.Count;
		for (int i = startIndex; i < count; i++)
		{
			int index = outContainer.Components.Count - 1;
			IncFreeSpace(masterDataBand, outContainer.Components[index]);
			outContainer.Components.RemoveAt(index);
		}
	}

	internal void RemoveFromContainerAt(StiDataBand masterDataBand, StiContainer outContainer, int startIndex, int endIndex)
	{
		for (int num = endIndex - startIndex; num > 0; num--)
		{
			IncFreeSpace(masterDataBand, outContainer.Components[startIndex]);
			outContainer.Components.RemoveAt(startIndex);
		}
	}

	internal bool GetGroupHeaderResult(StiDataBand masterDataBand, StiGroupHeaderBand band)
	{
		if (masterDataBand.IsBof)
		{
			return true;
		}
		if (masterDataBand.Position > 0)
		{
			masterDataBand.Position--;
			object currentConditionValue = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			masterDataBand.Position++;
			object currentConditionValue2 = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			if (!object.Equals(currentConditionValue, currentConditionValue2))
			{
				return true;
			}
		}
		int num = masterDataBand.DataBandInfoV1.GroupHeaderComponents.IndexOf(band);
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			StiGroupHeaderBand stiGroupHeaderBand = masterDataBand.DataBandInfoV1.GroupHeaderComponents[num2] as StiGroupHeaderBand;
			if (stiGroupHeaderBand.IsEnabled && GetGroupHeaderResult(masterDataBand, stiGroupHeaderBand))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool RenderGroupHeaders(StiDataBand masterDataBand, StiContainer outContainer, ref bool startNewPage)
	{
		int count = outContainer.Components.Count;
		int num = -1;
		masterDataBand.ParentBookmark = masterDataBand.DataBandInfoV1.ResParentBookmark;
		foreach (StiGroupHeaderBand groupHeaderComponent in masterDataBand.DataBandInfoV1.GroupHeaderComponents)
		{
			if (!IsPrintIfDetailEmpty(masterDataBand))
			{
				continue;
			}
			if (groupHeaderComponent.IsEnabled)
			{
				groupHeaderComponent.ParentBookmark = masterDataBand.ParentBookmark;
				bool flag = groupHeaderComponent.DoBookmark();
				if (groupHeaderComponent.ParentBookmark != groupHeaderComponent.CurrentBookmark)
				{
					masterDataBand.ParentBookmark = groupHeaderComponent.CurrentBookmark;
				}
				groupHeaderComponent.ParentPointer = masterDataBand.ParentPointer;
				groupHeaderComponent.DoPointer(!flag);
				if (groupHeaderComponent.ParentPointer != groupHeaderComponent.CurrentPointer)
				{
					masterDataBand.ParentPointer = groupHeaderComponent.CurrentPointer;
				}
			}
			bool flag2 = GetGroupHeaderResult(masterDataBand, groupHeaderComponent);
			if (groupHeaderComponent.GroupHeaderBandInfoV1.LastPositionLineRendering == masterDataBand.Position && !IsKeepHeaderTogether(masterDataBand))
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (groupHeaderComponent.GroupHeaderBandInfoV1.Rerender)
			{
				groupHeaderComponent.IsRendered = true;
				flag2 = true;
				groupHeaderComponent.GroupHeaderBandInfoV1.Rerender = false;
			}
			if (!flag2)
			{
				continue;
			}
			if (masterDataBand.Report != null)
			{
				StiReport report = masterDataBand.Report;
				if (report.CacheTotals && report.CachedTotals != null)
				{
					report.CachedTotals[groupHeaderComponent] = null;
				}
			}
			if (groupHeaderComponent.StartNewPage && (!masterDataBand.DataBandInfoV1.FirstRowInPath || masterDataBand.DataBandInfoV1.ForceStartNewPage))
			{
				double num2 = 100.0 * masterDataBand.DataBandInfoV1.FreeSpace / masterDataBand.Page.Height;
				if ((double)groupHeaderComponent.StartNewPageIfLessThan > num2 || groupHeaderComponent.StartNewPageIfLessThan == 100f)
				{
					startNewPage = true;
					if (num != -1)
					{
						masterDataBand.DataBandInfoV1.StartGroupIndex = num;
					}
					return false;
				}
			}
			if (flag3)
			{
				groupHeaderComponent.InvokeBeginRender(masterDataBand, masterDataBand.Position);
				masterDataBand.DataBandInfoV1.StartLine = masterDataBand.DataBandInfoV1.RuntimeLine;
			}
			if (num == -1)
			{
				masterDataBand.Report.SaveState(masterDataBand.Name + "Group");
				num = outContainer.Components.Count;
			}
			StiComponent renderedComponent = null;
			StiBrush brush = groupHeaderComponent.Brush;
			bool enabled = groupHeaderComponent.Enabled;
			try
			{
				groupHeaderComponent.InvokeBeforePrint(groupHeaderComponent, EventArgs.Empty);
				if (groupHeaderComponent.IsEnabled)
				{
					groupHeaderComponent.Render(ref renderedComponent, outContainer);
					bool flag4 = true;
					if (renderedComponent != null)
					{
						DecFreeSpace(masterDataBand, renderedComponent);
						double freeSpace = masterDataBand.DataBandInfoV1.FreeSpace;
						StiGroupHeaderBandV1Builder.SecondPassBreak(groupHeaderComponent, renderedComponent, groupHeaderComponent, ref freeSpace);
						masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
						if (CheckFreeSpace(masterDataBand, outContainer))
						{
							if (groupHeaderComponent.GroupHeaderBandInfoV1.ForceCanBreak)
							{
								double freeSpace2 = masterDataBand.DataBandInfoV1.FreeSpace;
								StiGroupHeaderBandV1Builder.FirstPassBreak(groupHeaderComponent, renderedComponent, groupHeaderComponent, ref freeSpace2);
								masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
								groupHeaderComponent.Prepare();
							}
							else
							{
								flag4 = false;
								if (IsKeepHeaderTogether(masterDataBand) && !masterDataBand.DataBandInfoV1.LatestDataBandBreaked)
								{
									RemoveFromContainerAt(masterDataBand, outContainer, count);
									masterDataBand.Report.RestoreState(masterDataBand.Name + "Group");
									foreach (StiGroupHeaderBand groupHeaderComponent2 in masterDataBand.DataBandInfoV1.GroupHeaderComponents)
									{
										groupHeaderComponent2.Prepare();
									}
								}
								else
								{
									groupHeaderComponent.GroupHeaderBandInfoV1.Rerender = true;
									IncFreeSpace(masterDataBand, renderedComponent);
									outContainer.Components.Remove(renderedComponent);
									groupHeaderComponent.Prepare();
								}
								if (num != -1)
								{
									masterDataBand.DataBandInfoV1.StartGroupIndex = num;
								}
							}
							return false;
						}
					}
					if (flag4 && groupHeaderComponent.GroupHeaderBandInfoV1.GroupFooter != null && groupHeaderComponent.GroupHeaderBandInfoV1.GroupFooter.GroupFooterBandInfoV1.PrintAtBottomComponent != null)
					{
						RemoveBottomRenderedGroupFooters(masterDataBand, groupHeaderComponent.GroupHeaderBandInfoV1.GroupFooter.GroupFooterBandInfoV1.PrintAtBottomComponent, groupHeaderComponent.GroupHeaderBandInfoV1.GroupFooter);
					}
				}
				groupHeaderComponent.InvokeAfterPrint(groupHeaderComponent, EventArgs.Empty);
			}
			finally
			{
				groupHeaderComponent.Brush = brush;
				groupHeaderComponent.Enabled = enabled;
			}
		}
		if (num != -1)
		{
			masterDataBand.DataBandInfoV1.StartGroupIndex = num;
		}
		return true;
	}

	internal bool GetGroupFooterResult(StiDataBand masterDataBand, StiGroupFooterBand band)
	{
		return GetGroupFooterResult(masterDataBand, band.GroupFooterBandInfoV1.GroupHeader);
	}

	internal bool GetGroupFooterResult(StiDataBand masterDataBand, StiGroupHeaderBand band)
	{
		if (masterDataBand.Position >= masterDataBand.Count - 1)
		{
			return true;
		}
		masterDataBand.Position++;
		object currentConditionValue = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
		masterDataBand.Position--;
		object currentConditionValue2 = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
		if (!object.Equals(currentConditionValue, currentConditionValue2))
		{
			return true;
		}
		int num = masterDataBand.DataBandInfoV1.GroupHeaderComponents.IndexOf(band);
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			StiGroupHeaderBand band2 = masterDataBand.DataBandInfoV1.GroupHeaderComponents[num2] as StiGroupHeaderBand;
			if (GetGroupFooterResult(masterDataBand, band2))
			{
				return true;
			}
		}
		return false;
	}

	internal void RenderOneGroupFooter(StiDataBand masterDataBand, StiGroupFooterBand groupFooter, ref StiComponent outFooter, StiContainer outContainer)
	{
		StiBrush brush = groupFooter.Brush;
		bool enabled = groupFooter.Enabled;
		try
		{
			groupFooter.InvokeBeforePrint(groupFooter, EventArgs.Empty);
			if (groupFooter.IsEnabled)
			{
				groupFooter.ParentBookmark = masterDataBand.ParentBookmark;
				bool flag = groupFooter.DoBookmark();
				groupFooter.ParentPointer = masterDataBand.ParentPointer;
				groupFooter.DoPointer(!flag);
				groupFooter.Render(ref outFooter, outContainer);
			}
			groupFooter.InvokeAfterPrint(groupFooter, EventArgs.Empty);
		}
		finally
		{
			groupFooter.Brush = brush;
			groupFooter.Enabled = enabled;
		}
	}

	internal bool IsGroupFooterResult(StiDataBand masterDataBand)
	{
		for (int num = masterDataBand.DataBandInfoV1.GroupHeaderComponents.Count - 1; num >= 0; num--)
		{
			StiGroupHeaderBand stiGroupHeaderBand = masterDataBand.DataBandInfoV1.GroupHeaderComponents[num] as StiGroupHeaderBand;
			if (stiGroupHeaderBand.IsEnabled && IsPrintIfDetailEmpty(masterDataBand))
			{
				int position = masterDataBand.Position;
				if (masterDataBand.Position > 0)
				{
					masterDataBand.Position--;
				}
				bool groupFooterResult = GetGroupFooterResult(masterDataBand, stiGroupHeaderBand);
				masterDataBand.Position = position;
				if (groupFooterResult)
				{
					return true;
				}
			}
		}
		return false;
	}

	public virtual bool RenderGroupFooters(StiDataBand masterDataBand, ref int footerIndex, StiContainer outContainer, out StiComponentsCollection renderedGroupFooters)
	{
		renderedGroupFooters = new StiComponentsCollection();
		bool flag = true;
		for (int num = masterDataBand.DataBandInfoV1.GroupHeaderComponents.Count - 1; num >= 0; num--)
		{
			StiGroupHeaderBand stiGroupHeaderBand = masterDataBand.DataBandInfoV1.GroupHeaderComponents[num] as StiGroupHeaderBand;
			if (stiGroupHeaderBand.IsEnabled && IsPrintIfDetailEmpty(masterDataBand) && GetGroupFooterResult(masterDataBand, stiGroupHeaderBand))
			{
				stiGroupHeaderBand.InvokeEndRender();
				StiGroupFooterBand groupFooter = stiGroupHeaderBand.GroupHeaderBandInfoV1.GroupFooter;
				StiComponent outFooter = null;
				if (groupFooter != null)
				{
					groupFooter.ParentBookmark = stiGroupHeaderBand.CurrentBookmark;
					RenderOneGroupFooter(masterDataBand, groupFooter, ref outFooter, outContainer);
					if (outFooter != null)
					{
						DecFreeSpace(masterDataBand, outFooter);
					}
				}
				if (outFooter != null)
				{
					if (CheckFreeSpace(masterDataBand, outContainer) || !flag)
					{
						masterDataBand.DataBandInfoV1.RemmitedCollection.Add(outFooter);
						outContainer.Components.Remove(outFooter);
						flag = false;
					}
					else
					{
						renderedGroupFooters.Add(stiGroupHeaderBand);
					}
				}
			}
		}
		return flag;
	}

	public int GetGroupLinesCount(StiDataBand masterDataBand, StiGroupHeaderBand band)
	{
		int position = masterDataBand.Position;
		int num = 0;
		int num2 = 0;
		while (masterDataBand.Position > 0)
		{
			object currentConditionValue = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			masterDataBand.Position--;
			object currentConditionValue2 = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			if (!object.Equals(currentConditionValue, currentConditionValue2))
			{
				break;
			}
			num++;
		}
		masterDataBand.Position = position;
		while (masterDataBand.Position < masterDataBand.Count - 1)
		{
			object currentConditionValue3 = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			masterDataBand.Position++;
			object currentConditionValue4 = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(band);
			if (!object.Equals(currentConditionValue3, currentConditionValue4))
			{
				break;
			}
			num2++;
		}
		masterDataBand.Position = position;
		return num + num2 + 1;
	}

	public virtual void GroupsComparison(StiDataBand masterDataBand)
	{
		foreach (StiGroupHeaderBand groupHeaderComponent in masterDataBand.DataBandInfoV1.GroupHeaderComponents)
		{
			groupHeaderComponent.GroupHeaderBandInfoV1.GroupFooter = null;
		}
		for (int i = 0; i < masterDataBand.DataBandInfoV1.GroupFooterComponents.Count; i++)
		{
			int num = masterDataBand.DataBandInfoV1.GroupHeaderComponents.Count - i - 1;
			if (num >= 0)
			{
				((StiGroupFooterBand)masterDataBand.DataBandInfoV1.GroupFooterComponents[i]).GroupFooterBandInfoV1.GroupHeader = (StiGroupHeaderBand)masterDataBand.DataBandInfoV1.GroupHeaderComponents[num];
				((StiGroupHeaderBand)masterDataBand.DataBandInfoV1.GroupHeaderComponents[num]).GroupHeaderBandInfoV1.GroupFooter = (StiGroupFooterBand)masterDataBand.DataBandInfoV1.GroupFooterComponents[i];
			}
			else
			{
				((StiGroupFooterBand)masterDataBand.DataBandInfoV1.GroupFooterComponents[i]).GroupFooterBandInfoV1.GroupHeader = null;
			}
		}
	}

	public virtual void IncFreeSpace(StiDataBand masterDataBand, StiComponent component)
	{
		masterDataBand.DataBandInfoV1.FreeSpace += component.Height;
	}

	public virtual void DecFreeSpace(StiDataBand masterDataBand, StiComponent component)
	{
		masterDataBand.DataBandInfoV1.FreeSpace -= component.Height;
	}

	public virtual double GetFreeSpaceFromRectangle(RectangleD rect)
	{
		return ((RectangleD)(ref rect)).Height;
	}

	public virtual double GetFreeSpaceFromComponent(StiComponent component)
	{
		return component.Height;
	}

	public static void SetDetails(StiDataBand masterDataBand)
	{
		if (masterDataBand.IsDataSourceEmpty)
		{
			return;
		}
		if (masterDataBand.DataBandInfoV1.DetailDataComponents != null)
		{
			foreach (StiComponent detailDataComponent in masterDataBand.DataBandInfoV1.DetailDataComponents)
			{
				StiDataHelper.SetData(detailDataComponent, reinit: false);
				if (!(detailDataComponent is StiDataBand masterDataBand2))
				{
					continue;
				}
				StiDataBandV1Builder stiDataBandV1Builder = StiV1Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV1Builder;
				StiComponentsCollection groupHeaders = stiDataBandV1Builder.GetGroupHeaders(masterDataBand2);
				foreach (StiGroupHeaderBand item in groupHeaders)
				{
					item.GroupHeaderBandInfoV1.LastPositionLineRendering = -1;
					item.GroupHeaderBandInfoV1.LastPositionRendering = -1;
				}
			}
		}
		foreach (StiComponent component2 in masterDataBand.Components)
		{
			if (component2.IsCross && (!(component2 is StiCrossDataBand) || !((StiCrossDataBand)component2).ColumnMode))
			{
				StiDataHelper.SetData(component2, reinit: false);
			}
		}
		if (masterDataBand.DataBandInfoV1.SubReportsComponents == null)
		{
			return;
		}
		foreach (StiSubReport subReportsComponent in masterDataBand.DataBandInfoV1.SubReportsComponents)
		{
			if (subReportsComponent.SubReportPage == null)
			{
				continue;
			}
			StiComponentsCollection components = subReportsComponent.SubReportPage.GetComponents();
			foreach (StiComponent item2 in components)
			{
				StiDataHelper.SetData(item2, reinit: false);
			}
		}
	}

	[Browsable(false)]
	public static bool IsDetailDataSourcesEmpty(StiDataBand masterDataBand)
	{
		if (masterDataBand.DataBandInfoV1.DetailComponents == null || masterDataBand.DataBandInfoV1.DetailComponents.Count == 0)
		{
			return false;
		}
		foreach (StiComponent detailComponent in masterDataBand.DataBandInfoV1.DetailComponents)
		{
			if (detailComponent is IStiDataSource && !((IStiDataSource)detailComponent).IsEmpty)
			{
				return false;
			}
		}
		return true;
	}

	public void SetData(StiDataBand masterDataBand)
	{
		if (!(masterDataBand is StiCrossDataBand))
		{
			StiDataHelper.SetData(masterDataBand, reinit: false);
		}
		else if (!((StiCrossDataBand)masterDataBand).ColumnMode)
		{
			StiDataHelper.SetData(masterDataBand, reinit: false);
		}
		SetDataIsPrepared(masterDataBand, value: true);
	}

	public void ResetStartNewPageProcessed(StiDataBand masterDataBand)
	{
		foreach (StiDataBand detailComponent in masterDataBand.DataBandInfoV1.DetailComponents)
		{
			foreach (StiHeaderBand headerComponent in detailComponent.DataBandInfoV1.HeaderComponents)
			{
				headerComponent.HeaderBandInfoV1.StartNewPageProcessed = false;
			}
		}
	}

	public virtual void FirstDetail(StiDataBand masterDataBand)
	{
		masterDataBand.DataBandInfoV1.CurrentDetailComponent = null;
		for (int i = 0; i < masterDataBand.DataBandInfoV1.DetailComponents.Count; i++)
		{
			masterDataBand.DataBandInfoV1.CurrentDetailComponent = masterDataBand.DataBandInfoV1.DetailComponents[i];
			if (masterDataBand.DataBandInfoV1.CurrentDetailComponent is StiDataBand { Columns: >0, IsEnabled: false })
			{
				masterDataBand.DataBandInfoV1.CurrentDetailComponent = null;
				continue;
			}
			break;
		}
	}

	public virtual void NextDetail(StiDataBand masterDataBand)
	{
		int num = masterDataBand.DataBandInfoV1.DetailComponents.IndexOf(masterDataBand.DataBandInfoV1.CurrentDetailComponent) + 1;
		if (num == masterDataBand.DataBandInfoV1.DetailComponents.Count)
		{
			masterDataBand.DataBandInfoV1.CurrentDetailComponent = null;
		}
		else
		{
			masterDataBand.DataBandInfoV1.CurrentDetailComponent = masterDataBand.DataBandInfoV1.DetailComponents[num];
		}
	}

	public virtual void RenderDetails(StiDataBand masterDataBand, StiContainer outContainer)
	{
		if (masterDataBand.DataBandInfoV1.CurrentDetailComponent == null)
		{
			FirstDetail(masterDataBand);
			if (masterDataBand.DataBandInfoV1.CurrentDetailComponent != null)
			{
				masterDataBand.DataBandInfoV1.CurrentDetailComponent.Prepare();
			}
		}
		bool flag = true;
		if (masterDataBand.DataBandInfoV1.CurrentDetailComponent == null)
		{
			return;
		}
		while (true)
		{
			masterDataBand.DataBandInfoV1.CurrentDetailComponent.ParentBookmark = masterDataBand.CurrentBookmark;
			StiComponent currentDetailComponent = masterDataBand.DataBandInfoV1.CurrentDetailComponent;
			flag = masterDataBand.DataBandInfoV1.CurrentDetailComponent.Render(outContainer);
			masterDataBand.DataBandInfoV1.CurrentDetailComponent = currentDetailComponent;
			if (flag)
			{
				NextDetail(masterDataBand);
				if (masterDataBand.DataBandInfoV1.CurrentDetailComponent != null)
				{
					masterDataBand.DataBandInfoV1.CurrentDetailComponent.Prepare();
					continue;
				}
				break;
			}
			break;
		}
	}

	public bool IsPrintIfDetailEmpty(StiDataBand masterDataBand)
	{
		if (!masterDataBand.PrintIfDetailEmpty)
		{
			return !IsDetailDataSourcesEmpty(masterDataBand);
		}
		return true;
	}

	internal void AddBottomRenderedHeaders(StiDataBand masterDataBand, StiComponent header, StiHeaderBand parentHeader)
	{
		if (parentHeader.PrintAtBottom)
		{
			StiDataBand stiDataBand = masterDataBand;
			while (stiDataBand.MasterComponent != null && stiDataBand.MasterComponent is StiDataBand)
			{
				stiDataBand = stiDataBand.MasterComponent as StiDataBand;
			}
			StiContainer parent = stiDataBand.Parent;
			if (parent.ContainerInfoV1.BottomRenderedHeaders == null)
			{
				parent.ContainerInfoV1.BottomRenderedHeaders = new Hashtable();
				parent.ContainerInfoV1.BottomRenderedParentsHeaders = new StiComponentsCollection();
			}
			StiComponentsCollection stiComponentsCollection = parent.ContainerInfoV1.BottomRenderedHeaders[parentHeader] as StiComponentsCollection;
			if (stiComponentsCollection == null)
			{
				stiComponentsCollection = new StiComponentsCollection();
				parent.ContainerInfoV1.BottomRenderedHeaders[parentHeader] = stiComponentsCollection;
			}
			stiComponentsCollection.Add(header);
			if (parent.ContainerInfoV1.BottomRenderedParentsHeaders.IndexOf(parentHeader) == -1)
			{
				parent.ContainerInfoV1.BottomRenderedParentsHeaders.Insert(0, parentHeader);
			}
		}
	}

	internal void AddBottomRenderedFooters(StiDataBand masterDataBand, StiComponent footer, StiFooterBand parentFooter)
	{
		if (parentFooter.PrintAtBottom)
		{
			StiDataBand stiDataBand = masterDataBand;
			while (stiDataBand.MasterComponent != null && stiDataBand.MasterComponent is StiDataBand)
			{
				stiDataBand = stiDataBand.MasterComponent as StiDataBand;
			}
			StiContainer parent = stiDataBand.Parent;
			if (parent.ContainerInfoV1.BottomRenderedFooters == null)
			{
				parent.ContainerInfoV1.BottomRenderedFooters = new Hashtable();
				parent.ContainerInfoV1.BottomRenderedParentsFooters = new StiComponentsCollection();
			}
			StiComponentsCollection stiComponentsCollection = parent.ContainerInfoV1.BottomRenderedFooters[parentFooter] as StiComponentsCollection;
			if (stiComponentsCollection == null)
			{
				stiComponentsCollection = new StiComponentsCollection();
				parent.ContainerInfoV1.BottomRenderedFooters[parentFooter] = stiComponentsCollection;
			}
			stiComponentsCollection.Add(footer);
			if (parent.ContainerInfoV1.BottomRenderedParentsFooters.IndexOf(parentFooter) == -1)
			{
				parent.ContainerInfoV1.BottomRenderedParentsFooters.Insert(0, parentFooter);
			}
		}
	}

	internal void AddBottomRenderedDataBands(StiDataBand masterDataBand, StiComponent band, StiDataBand parentDataBand)
	{
		if (parentDataBand.PrintAtBottom)
		{
			StiContainer parent = parentDataBand.Parent;
			if (parent.ContainerInfoV1.BottomRenderedDataBands == null)
			{
				parent.ContainerInfoV1.BottomRenderedDataBands = new Hashtable();
				parent.ContainerInfoV1.BottomRenderedParentsDataBands = new StiComponentsCollection();
			}
			StiComponentsCollection stiComponentsCollection = parent.ContainerInfoV1.BottomRenderedDataBands[parentDataBand] as StiComponentsCollection;
			if (stiComponentsCollection == null)
			{
				stiComponentsCollection = new StiComponentsCollection();
				parent.ContainerInfoV1.BottomRenderedDataBands[parentDataBand] = stiComponentsCollection;
			}
			stiComponentsCollection.Add(band);
			if (parent.ContainerInfoV1.BottomRenderedParentsDataBands.IndexOf(parentDataBand) == -1)
			{
				parent.ContainerInfoV1.BottomRenderedParentsDataBands.Insert(0, parentDataBand);
			}
		}
	}

	internal void AddBottomRenderedGroupFooters(StiDataBand masterDataBand, StiComponent footer, StiGroupFooterBand parentFooter)
	{
		if (parentFooter.PrintAtBottom)
		{
			StiContainer parent = parentFooter.Parent;
			if (parent.ContainerInfoV1.BottomRenderedGroupFooters == null)
			{
				parent.ContainerInfoV1.BottomRenderedGroupFooters = new Hashtable();
				parent.ContainerInfoV1.BottomRenderedParentsGroupFooters = new StiComponentsCollection();
			}
			StiComponentsCollection stiComponentsCollection = parent.ContainerInfoV1.BottomRenderedGroupFooters[parentFooter] as StiComponentsCollection;
			if (stiComponentsCollection == null)
			{
				stiComponentsCollection = new StiComponentsCollection();
				parent.ContainerInfoV1.BottomRenderedGroupFooters[parentFooter] = stiComponentsCollection;
			}
			stiComponentsCollection.Add(footer);
			if (parent.ContainerInfoV1.BottomRenderedParentsGroupFooters.IndexOf(parentFooter) == -1)
			{
				parent.ContainerInfoV1.BottomRenderedParentsGroupFooters.Insert(0, parentFooter);
			}
		}
	}

	internal void RemoveBottomRenderedGroupFooters(StiDataBand masterDataBand, StiComponent footer, StiGroupFooterBand parentFooter)
	{
		StiContainer parent = parentFooter.Parent;
		if (parent.ContainerInfoV1.BottomRenderedGroupFooters != null && parent.ContainerInfoV1.BottomRenderedGroupFooters[parentFooter] is StiComponentsCollection stiComponentsCollection && stiComponentsCollection.Contains(footer))
		{
			stiComponentsCollection.Remove(footer);
		}
	}

	internal void SetLastDataBand(StiDataBand masterDataBand)
	{
		StiDataBand stiDataBand = masterDataBand;
		while (stiDataBand.MasterComponent != null && stiDataBand.MasterComponent is StiDataBand)
		{
			stiDataBand = stiDataBand.MasterComponent as StiDataBand;
		}
		StiContainer parent = stiDataBand.Parent;
		parent.ContainerInfoV1.LastDataBand = masterDataBand;
	}

	public virtual void MoveNext(StiDataBand masterDataBand)
	{
		ResetStartNewPageProcessed(masterDataBand);
		masterDataBand.Next();
		SetDetails(masterDataBand);
	}

	internal void ApplyStyle(StiDataBand masterDataBand, StiContainer cont)
	{
		if (masterDataBand.Report == null)
		{
			return;
		}
		if ((masterDataBand.LineThrough & 1) == 1)
		{
			if (!string.IsNullOrEmpty(masterDataBand.EvenStyle))
			{
				StiBaseStyle stiBaseStyle = masterDataBand.Report.Styles[masterDataBand.EvenStyle];
				if (stiBaseStyle != null)
				{
					stiBaseStyle.SetStyleToComponent(cont);
					StiBaseStyle style = StiBaseStyle.GetStyle(cont);
					cont.SetParentStylesToChilds(style);
				}
			}
		}
		else if (!string.IsNullOrEmpty(masterDataBand.OddStyle))
		{
			StiBaseStyle stiBaseStyle2 = masterDataBand.Report.Styles[masterDataBand.OddStyle];
			if (stiBaseStyle2 != null)
			{
				stiBaseStyle2.SetStyleToComponent(cont);
				StiBaseStyle style2 = StiBaseStyle.GetStyle(cont);
				cont.SetParentStylesToChilds(style2);
			}
		}
	}

	internal bool IsAcrossThenDown(StiDataBand masterDataBand)
	{
		if (masterDataBand.Columns > 1)
		{
			return masterDataBand.ColumnDirection == StiColumnDirection.AcrossThenDown;
		}
		return false;
	}

	internal bool IsCrossColumn(StiDataBand masterDataBand)
	{
		if (masterDataBand is StiCrossDataBand)
		{
			return ((StiCrossDataBand)masterDataBand).ColumnMode;
		}
		return false;
	}

	public virtual void SetDataIsPrepared(StiDataBand masterDataBand, bool value)
	{
		masterDataBand.DataBandInfoV1.DataIsPrepared = value;
	}

	public virtual bool GetDataIsPrepared(StiDataBand masterDataBand)
	{
		return masterDataBand.DataBandInfoV1.DataIsPrepared;
	}

	public virtual bool CheckFreeSpace(StiDataBand masterDataBand, StiContainer outContainer)
	{
		if (masterDataBand.DataBandInfoV1.FreeSpace < 0.0)
		{
			if (!masterDataBand.Page.UnlimitedHeight)
			{
				return true;
			}
			DecFreeSpace(masterDataBand, outContainer);
			outContainer.AddSize(leftToRightDirection: false);
			IncFreeSpace(masterDataBand, outContainer);
		}
		return false;
	}

	public virtual void SetReportVariables(StiDataBand masterDataBand)
	{
		masterDataBand.Report.LineThrough = masterDataBand.LineThrough;
		masterDataBand.Report.Line = masterDataBand.DataBandInfoV1.RuntimeLine - masterDataBand.DataBandInfoV1.StartLine + 1;
	}

	private void SaveStateColumnsAcrossThenDown(StiDataBand masterDataBand, string name)
	{
		if (masterDataBand.ColumnDirection == StiColumnDirection.AcrossThenDown && !IsCrossColumn(masterDataBand) && masterDataBand.Columns > 1)
		{
			masterDataBand.Report.SaveState(masterDataBand.Name + name);
		}
	}

	private void RestoreStateColumnsAcrossThenDown(StiDataBand masterDataBand, string name)
	{
		if (masterDataBand.ColumnDirection == StiColumnDirection.AcrossThenDown && !IsCrossColumn(masterDataBand) && masterDataBand.Columns > 1)
		{
			masterDataBand.Page.PageInfoV1.RestoreCurrentDetailComponent = false;
			masterDataBand.Report.RestoreState(masterDataBand.Name + name);
			masterDataBand.Page.PageInfoV1.RestoreCurrentDetailComponent = true;
		}
		else
		{
			masterDataBand.Report.RestoreState("DataBand" + masterDataBand.Name);
		}
	}

	internal bool IsDataBandInMasterBand(StiDataBand masterDataBand)
	{
		if (masterDataBand.IsCross)
		{
			return false;
		}
		for (StiContainer parent = masterDataBand.Parent; parent != null; parent = parent.Parent)
		{
			if (parent is StiDataBand)
			{
				return true;
			}
		}
		return false;
	}

	internal void CheckParentContainerBeforeRendering(StiDataBand masterDataBand, StiContainer outContainer)
	{
		bool flag = false;
		if (masterDataBand.Parent != null && masterDataBand.Parent.Parent is StiDataBand)
		{
			flag = true;
		}
		if (!(masterDataBand is StiCrossDataBand) && (flag || outContainer.CanGrow || outContainer.CanShrink) && masterDataBand.Columns < 2)
		{
			double num = outContainer.ComponentToPage(outContainer.Top);
			masterDataBand.DataBandInfoV1.ResHeightOfContainerBeforeRendering = outContainer.Height;
			outContainer.Height = outContainer.Page.Height - num;
		}
	}

	internal void CheckParentContainerAfterRendering(StiDataBand masterDataBand, StiContainer outContainer)
	{
		bool flag = false;
		if (masterDataBand.Parent != null && masterDataBand.Parent.Parent is StiDataBand)
		{
			flag = true;
		}
		if (masterDataBand is StiCrossDataBand || (!flag && !outContainer.CanGrow && !outContainer.CanShrink) || masterDataBand.Columns >= 2)
		{
			return;
		}
		double num = 0.0;
		foreach (StiComponent component in outContainer.Components)
		{
			num = Math.Max(component.Bottom, num);
		}
		if (num > masterDataBand.DataBandInfoV1.ResHeightOfContainerBeforeRendering && (outContainer.CanGrow || flag))
		{
			outContainer.Height = num;
			return;
		}
		if (num < masterDataBand.DataBandInfoV1.ResHeightOfContainerBeforeRendering && (outContainer.CanShrink || flag))
		{
			outContainer.Height = num;
			return;
		}
		outContainer.Height = masterDataBand.DataBandInfoV1.ResHeightOfContainerBeforeRendering;
		if (masterDataBand.Parent.GrowToHeight && outContainer.Components.Count > 0)
		{
			outContainer.Components[outContainer.Components.Count - 1].GrowToHeight = true;
		}
	}

	internal void SetParentGrowToHeightInContainer(StiDataBand masterDataBand, StiContainer outContainer)
	{
		if (!(masterDataBand is StiCrossDataBand) && masterDataBand.Columns < 2 && masterDataBand.Parent.GrowToHeight && outContainer.Components.Count > 0)
		{
			outContainer.Components[outContainer.Components.Count - 1].GrowToHeight = true;
		}
	}

	internal void MoveDataBandsToMasterDataband(StiDataBand masterDataBand, StiContainer outContainer)
	{
		outContainer.DockToContainer();
		foreach (StiComponent component in outContainer.Components)
		{
			component.DockStyle = StiDockStyle.None;
			component.Left += outContainer.Left;
			outContainer.Parent.Components.Add(component);
		}
		outContainer.Height = 0.0;
		outContainer.Components.Clear();
	}

	protected virtual void ProcessRenderedContainer(StiDataBand masterDataBand, StiContainer container)
	{
	}

	private void ProcessDataItemsOnDataBand(StiContainer container)
	{
		foreach (StiComponent component in container.Components)
		{
			if (component.IsCross)
			{
				component.Prepare();
			}
			if (component is StiDataBand && !(component is StiCrossDataBand))
			{
				component.Prepare();
			}
			if (component is StiContainer container2)
			{
				ProcessDataItemsOnDataBand(container2);
			}
		}
	}

	private StiContainer GetRenderContainer(StiDataBand masterDataBand, StiContainer outContainer)
	{
		if (IsAcrossThenDown(masterDataBand))
		{
			if (masterDataBand.DataBandInfoV1.ColumnIndex == 1)
			{
				masterDataBand.DataBandInfoV1.ParentColumnContainer = new StiContainer();
				masterDataBand.DataBandInfoV1.ParentColumnContainer.Width = masterDataBand.Width;
				masterDataBand.DataBandInfoV1.ParentColumnContainer.Height = masterDataBand.Height;
				masterDataBand.DataBandInfoV1.ParentColumnContainer.DockStyle = StiDockStyle.Top;
				outContainer.Components.Add(masterDataBand.DataBandInfoV1.ParentColumnContainer);
			}
			masterDataBand.Report.Column = masterDataBand.DataBandInfoV1.ColumnIndex;
			StiContainer stiContainer = new StiContainer();
			stiContainer.DockStyle = (masterDataBand.RightToLeft ? StiDockStyle.Right : StiDockStyle.Left);
			stiContainer.Width = masterDataBand.GetColumnWidth() + masterDataBand.ColumnGaps;
			stiContainer.Height = masterDataBand.Height * 2.0;
			masterDataBand.DataBandInfoV1.ParentColumnContainer.Components.Add(stiContainer);
			masterDataBand.DataBandInfoV1.ColumnIndex++;
			if (masterDataBand.DataBandInfoV1.ColumnIndex > masterDataBand.Columns)
			{
				masterDataBand.DataBandInfoV1.ColumnIndex = 1;
			}
			masterDataBand.DataBandInfoV1.FreeSpace = stiContainer.Height;
			return stiContainer;
		}
		return outContainer;
	}

	public virtual bool RenderItem(StiDataBand masterDataBand, out bool breaked, StiContainer outContainer)
	{
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		bool flag = masterDataBand.CanBreak;
		StiComponent renderedComponent = null;
		SaveStateColumnsAcrossThenDown(masterDataBand, "Columns");
		if (!masterDataBand.IsCross)
		{
			if (masterDataBand.DataBandInfoV1.CrossTabExistOnDataBand)
			{
				flag = true;
			}
			if (masterDataBand.DataBandInfoV1.SubReportsComponents.Count > 0)
			{
				flag = true;
			}
			ProcessDataItemsOnDataBand(masterDataBand);
		}
		if (GetSubReportRenderResult(masterDataBand))
		{
			foreach (StiComponent subReportsComponent in masterDataBand.DataBandInfoV1.SubReportsComponents)
			{
				subReportsComponent.Prepare();
			}
		}
		breaked = false;
		bool result = true;
		int count = outContainer.Components.Count;
		masterDataBand.DataBandInfoV1.SkipStartNewPage = true;
		if (!IsDetailDataSourcesEmpty(masterDataBand) || masterDataBand.PrintIfDetailEmpty)
		{
			masterDataBand.DataBandInfoV1.SkipStartNewPage = false;
			SetReportVariables(masterDataBand);
			masterDataBand.Report.SaveState("DataBand" + masterDataBand.Name);
			StiBrush brush = masterDataBand.Brush;
			bool enabled = masterDataBand.Enabled;
			masterDataBand.InvokeBeforePrint(masterDataBand, EventArgs.Empty);
			if (masterDataBand.CalcInvisible)
			{
				masterDataBand.InvokeRendering();
				masterDataBand.InvokeGroupRendering();
			}
			try
			{
				if (masterDataBand.Enabled)
				{
					masterDataBand.DoGetBookmark();
					if (!masterDataBand.CalcInvisible)
					{
						masterDataBand.InvokeRendering();
						masterDataBand.InvokeGroupRendering();
					}
					masterDataBand.RenderedCount++;
					masterDataBand.DoBookmark();
					masterDataBand.DoPointer();
					bool flag2 = true;
					if (!masterDataBand.IsRendered || masterDataBand.PrintOnAllPages || masterDataBand.DataBandInfoV1.DetailComponents.Count == 0)
					{
						masterDataBand.RenderContainer(ref renderedComponent, outContainer);
						ProcessRenderedContainer(masterDataBand, renderedComponent as StiContainer);
						if (masterDataBand.PrintAtBottom)
						{
							AddBottomRenderedDataBands(masterDataBand, renderedComponent, masterDataBand);
						}
						ApplyStyle(masterDataBand, renderedComponent as StiContainer);
						if (masterDataBand.DataBandInfoV1.ItemsActive)
						{
							masterDataBand.DataBandInfoV1.RenderedItems.Add(renderedComponent);
						}
						DecFreeSpace(masterDataBand, renderedComponent);
						flag2 = GetSubReportRenderResult(masterDataBand);
					}
					double freeSpace = masterDataBand.DataBandInfoV1.FreeSpace;
					SecondPassBreak(masterDataBand, renderedComponent, masterDataBand, ref freeSpace);
					masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
					bool flag3 = CheckFreeSpace(masterDataBand, outContainer);
					if (masterDataBand.Columns < 2 && !flag3 && masterDataBand.DataBandInfoV1.DetailComponents != null && masterDataBand.DataBandInfoV1.DetailComponents.Count > 0)
					{
						RenderDetails(masterDataBand, outContainer);
						masterDataBand.DataBandInfoV1.FreeSpace = GetFreeSpaceFromRectangle(masterDataBand.GetDockRegion(outContainer));
						if (masterDataBand.DataBandInfoV1.CurrentDetailComponent is IStiBreakable && ((IStiBreakable)masterDataBand.DataBandInfoV1.CurrentDetailComponent).CanBreak)
						{
							masterDataBand.DataBandInfoV1.LatestDataBandBreaked = true;
						}
						else
						{
							masterDataBand.DataBandInfoV1.LatestDataBandBreaked = false;
						}
						if (masterDataBand.DataBandInfoV1.CurrentDetailComponent != null)
						{
							result = false;
							bool flag4 = false;
							if (masterDataBand.DataBandInfoV1.CurrentDetailComponent is IStiBreakable stiBreakable)
							{
								flag4 = stiBreakable.CanBreak;
							}
							if (masterDataBand.KeepChildTogether && (!masterDataBand.DataBandInfoV1.FirstRow || masterDataBand.DataBandInfoV1.AlwaysKeepChildTogether) && !flag4)
							{
								masterDataBand.IsRendered = false;
								masterDataBand.DataBandInfoV1.CurrentDetailComponent = null;
								masterDataBand.Report.RestoreState("DataBand" + masterDataBand.Name);
								int count2 = outContainer.Components.Count;
								for (int i = 0; i < count2 - count; i++)
								{
									outContainer.Components.RemoveAt(count);
								}
								if (IsKeepHeaderTogether(masterDataBand) && masterDataBand.DataBandInfoV1.FirstRow && !masterDataBand.DataBandInfoV1.LatestDataBandBreaked)
								{
									RemoveRenderedHeaders(masterDataBand, outContainer);
								}
								if (masterDataBand.DataBandInfoV1.ItemsActive)
								{
									masterDataBand.DataBandInfoV1.RenderedItems.RemoveAt(masterDataBand.DataBandInfoV1.RenderedItems.Count - 1);
								}
								masterDataBand.DataBandInfoV1.BreakableComps = null;
							}
							breaked = true;
							return result;
						}
						masterDataBand.IsRendered = false;
					}
					if (flag3)
					{
						bool flag5 = false;
						if (flag || !(masterDataBand.DataBandInfoV1.LastRenderBreaked || flag5))
						{
							result = false;
							masterDataBand.IsRendered = false;
							breaked = true;
							masterDataBand.DataBandInfoV1.LastRenderBreaked = true;
							if (flag)
							{
								double freeSpace2 = masterDataBand.DataBandInfoV1.FreeSpace;
								if (!FirstPassBreak(masterDataBand, renderedComponent, masterDataBand, ref freeSpace2))
								{
									breaked = false;
									result = true;
								}
								masterDataBand.DataBandInfoV1.FreeSpace = freeSpace2;
								RestoreStateColumnsAcrossThenDown(masterDataBand, "Columns");
							}
							else
							{
								if (renderedComponent != null)
								{
									outContainer.Components.Remove(renderedComponent);
								}
								if (masterDataBand.DataBandInfoV1.ItemsActive)
								{
									masterDataBand.DataBandInfoV1.RenderedItems.RemoveAt(masterDataBand.DataBandInfoV1.RenderedItems.Count - 1);
								}
								IncFreeSpace(masterDataBand, renderedComponent);
								RestoreStateColumnsAcrossThenDown(masterDataBand, "Columns");
								if (masterDataBand.DataBandInfoV1.FirstRow && IsKeepHeaderTogether(masterDataBand) && !masterDataBand.DataBandInfoV1.LatestDataBandBreaked)
								{
									RemoveRenderedHeaders(masterDataBand, outContainer);
								}
							}
							return result;
						}
						double width = renderedComponent.Width;
						double height = renderedComponent.Height;
						StiContainer stiContainer = renderedComponent as StiContainer;
						renderedComponent.Height += masterDataBand.DataBandInfoV1.FreeSpace;
						foreach (StiComponent component in stiContainer.Components)
						{
							if (component.Bottom >= renderedComponent.Height)
							{
								component.Height -= component.Bottom - renderedComponent.Height;
								component.Height = Math.Max(0.0, component.Height);
							}
						}
						flag3 = false;
						masterDataBand.DataBandInfoV1.FreeSpace = 0.0;
					}
					if (!flag2)
					{
						result = false;
						breaked = true;
						return result;
					}
					if (!flag3)
					{
						masterDataBand.InvokeAfterPrint(this, EventArgs.Empty);
						masterDataBand.DataBandInfoV1.LastRenderBreaked = false;
						masterDataBand.DataBandInfoV1.LastComponent = renderedComponent;
					}
				}
				else
				{
					masterDataBand.InvokeAfterPrint(masterDataBand, EventArgs.Empty);
				}
			}
			finally
			{
				masterDataBand.Brush = brush;
				masterDataBand.Enabled = enabled;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public virtual bool InternalRender(StiDataBand masterDataBand, ref StiComponent renderedComponent, StiContainer outContainer, bool value)
	{
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		if (masterDataBand.Position == 0 && !masterDataBand.IsRendered)
		{
			SetDetails(masterDataBand);
		}
		if (masterDataBand.DataBandInfoV1.FirstCall && masterDataBand.ResetDataSource)
		{
			StiDataHelper.SetData(masterDataBand, reinit: true);
			SetDataIsPrepared(masterDataBand, value: true);
		}
		else if (!GetDataIsPrepared(masterDataBand))
		{
			SetData(masterDataBand);
		}
		masterDataBand.DataBandInfoV1.FirstCall = false;
		masterDataBand.DataBandInfoV1.FirstGroupOnPass = true;
		int position = masterDataBand.Position;
		int num = 0;
		if (masterDataBand.Columns > 1)
		{
			num = (masterDataBand.Count - position) / masterDataBand.Columns;
			if (num * masterDataBand.Columns + position < masterDataBand.Count)
			{
				num++;
			}
			if (masterDataBand.MinRowsInColumn > 0)
			{
				num = Math.Max(masterDataBand.MinRowsInColumn, num);
			}
		}
		masterDataBand.DataBandInfoV1.RenderedItems = new List<StiComponent>();
		masterDataBand.DataBandInfoV1.ResParentBookmark = masterDataBand.ParentBookmark;
		masterDataBand.CurrentBookmark = masterDataBand.ParentBookmark;
		masterDataBand.DataBandInfoV1.LastComponent = null;
		masterDataBand.DataBandInfoV1.FirstRowInPath = true;
		masterDataBand.DataBandInfoV1.ForceStartNewPage = false;
		bool flag = true;
		masterDataBand.DataBandInfoV1.FirstRow = true;
		foreach (StiGroupHeaderBand groupHeaderComponent in masterDataBand.DataBandInfoV1.GroupHeaderComponents)
		{
			if (groupHeaderComponent.PrintOnAllPages)
			{
				groupHeaderComponent.GroupHeaderBandInfoV1.Rerender = true;
			}
		}
		masterDataBand.DataBandInfoV1.FreeSpace = GetFreeSpaceFromRectangle(masterDataBand.GetDockRegion(outContainer));
		ArrayList arrayList = null;
		bool flag2 = masterDataBand.DataBandInfoV1.RemmitedCollection.Count > 0;
		foreach (StiComponent item in masterDataBand.DataBandInfoV1.RemmitedCollection)
		{
			masterDataBand.DataBandInfoV1.ForceStartNewPage = true;
			outContainer.Components.Add(item);
			bool flag3 = false;
			foreach (StiFooterBand footerComponent in masterDataBand.DataBandInfoV1.FooterComponents)
			{
				if (footerComponent.Name == item.Name && footerComponent.PrintAtBottom)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				foreach (StiGroupFooterBand groupFooterComponent in masterDataBand.DataBandInfoV1.GroupFooterComponents)
				{
					if (groupFooterComponent.Name == item.Name && groupFooterComponent.PrintAtBottom)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(item);
						flag3 = true;
						break;
					}
				}
			}
			if (flag3)
			{
				item.DockStyle = StiDockStyle.Bottom;
			}
			if (item.Dockable)
			{
				item.DisplayRectangle = outContainer.DockToContainer(item.DisplayRectangle);
			}
			DecFreeSpace(masterDataBand, item);
		}
		masterDataBand.DataBandInfoV1.RemmitedCollection.Clear();
		int count = outContainer.Components.Count;
		if (masterDataBand.IsEof && masterDataBand.Count > 0)
		{
			return true;
		}
		if (!RenderHeaders(masterDataBand, outContainer))
		{
			return false;
		}
		StiComponentsCollection footers = new StiComponentsCollection();
		StiComponentsCollection parentFooters = new StiComponentsCollection();
		int count2 = outContainer.Components.Count;
		if (!RenderFootersOnAllPagesFirst(masterDataBand, ref footers, ref parentFooters, outContainer))
		{
			return false;
		}
		int num2 = outContainer.Components.Count - count2;
		int footerIndex = 0;
		masterDataBand.DataBandInfoV1.StartGroupIndex = -1;
		masterDataBand.DataBandInfoV1.StartMasterIndex = outContainer.Components.Count;
		int count3 = outContainer.Components.Count;
		while (!masterDataBand.IsEof && !masterDataBand.IsEmpty)
		{
			flag2 = false;
			count3 = outContainer.Components.Count;
			masterDataBand.Report.SaveState(masterDataBand.Name + "Item");
			bool startNewPage = false;
			flag = RenderGroupHeaders(masterDataBand, outContainer, ref startNewPage);
			if (!flag)
			{
				break;
			}
			int count4 = outContainer.Components.Count;
			bool breaked = false;
			if (masterDataBand.Columns >= 2 && masterDataBand.ColumnDirection == StiColumnDirection.AcrossThenDown && masterDataBand.DataBandInfoV1.GroupHeaderComponents.Count > 0 && IsGroupFooterResult(masterDataBand))
			{
				masterDataBand.DataBandInfoV1.ColumnIndex = 1;
			}
			double freeSpace = masterDataBand.DataBandInfoV1.FreeSpace;
			StiContainer renderContainer = GetRenderContainer(masterDataBand, outContainer);
			flag = RenderItem(masterDataBand, out breaked, renderContainer);
			if (masterDataBand.Columns >= 2 && masterDataBand.ColumnDirection == StiColumnDirection.AcrossThenDown)
			{
				masterDataBand.DataBandInfoV1.FreeSpace = freeSpace;
				if (count4 < outContainer.Components.Count)
				{
					masterDataBand.DataBandInfoV1.FreeSpace -= outContainer.Components[count4].Height;
					if (masterDataBand.DataBandInfoV1.FreeSpace < 0.0)
					{
						flag = false;
						breaked = true;
						outContainer.Components.RemoveAt(count4);
						masterDataBand.Report.RestoreState(masterDataBand.Name + "Item");
					}
				}
				if (masterDataBand.RightToLeft)
				{
					renderContainer = renderContainer.Components[0] as StiContainer;
					foreach (StiComponent component2 in renderContainer.Components)
					{
						component2.Left += masterDataBand.ColumnGaps;
					}
				}
			}
			if (!flag)
			{
				if (masterDataBand.DataBandInfoV1.StartGroupIndex != -1 && masterDataBand.KeepGroupTogether && masterDataBand.DataBandInfoV1.StartGroupIndex != count4 && (!masterDataBand.DataBandInfoV1.FirstGroupOnPass || StiOptions.Engine.IgnoreFirstPassForGroup))
				{
					RemoveFromContainerAt(masterDataBand, outContainer, masterDataBand.DataBandInfoV1.StartGroupIndex);
					masterDataBand.Report.RestoreState(masterDataBand.Name + "Group");
					foreach (StiComponent subReportsComponent in masterDataBand.DataBandInfoV1.SubReportsComponents)
					{
						subReportsComponent.Prepare();
					}
					masterDataBand.DataBandInfoV1.BreakableComps = null;
				}
				else if (IsKeepHeaderTogether(masterDataBand) && count3 != count4 && !masterDataBand.DataBandInfoV1.LatestDataBandBreaked)
				{
					if (masterDataBand.DataBandInfoV1.GroupHeaderComponents.Count > 0)
					{
						RemoveFromContainerAt(masterDataBand, outContainer, count3);
						masterDataBand.Report.RestoreState(masterDataBand.Name + "Group");
					}
					foreach (StiComponent subReportsComponent2 in masterDataBand.DataBandInfoV1.SubReportsComponents)
					{
						subReportsComponent2.Prepare();
					}
					masterDataBand.DataBandInfoV1.BreakableComps = null;
				}
			}
			StiComponentsCollection renderedGroupFooters = null;
			if (flag)
			{
				footerIndex = outContainer.Components.Count;
				flag = RenderGroupFooters(masterDataBand, ref footerIndex, outContainer, out renderedGroupFooters);
				if (!flag)
				{
					if (masterDataBand.DataBandInfoV1.StartGroupIndex != -1 && masterDataBand.KeepGroupTogether && masterDataBand.DataBandInfoV1.StartGroupIndex != count4 && (!masterDataBand.DataBandInfoV1.FirstGroupOnPass || StiOptions.Engine.IgnoreFirstPassForGroup))
					{
						RemoveFromContainerAt(masterDataBand, outContainer, masterDataBand.DataBandInfoV1.StartGroupIndex);
						masterDataBand.Report.RestoreState(masterDataBand.Name + "Group");
						renderedGroupFooters = null;
						masterDataBand.DataBandInfoV1.RemmitedCollection.Clear();
						masterDataBand.DataBandInfoV1.BreakableComps = null;
					}
					else if (masterDataBand.KeepFooterTogether && !masterDataBand.DataBandInfoV1.FirstRowInPath)
					{
						RemoveFromContainerAt(masterDataBand, outContainer, count3);
						masterDataBand.Report.RestoreState(masterDataBand.Name + "Item");
						renderedGroupFooters = null;
						masterDataBand.DataBandInfoV1.RemmitedCollection.Clear();
						masterDataBand.DataBandInfoV1.BreakableComps = null;
					}
					else
					{
						MoveNext(masterDataBand);
					}
					breaked = true;
				}
			}
			if (renderedGroupFooters != null && renderedGroupFooters.Count > 0)
			{
				RemoveFromContainerAt(masterDataBand, outContainer, footerIndex);
				foreach (StiGroupHeaderBand item2 in renderedGroupFooters)
				{
					StiGroupFooterBand groupFooter = item2.GroupHeaderBandInfoV1.GroupFooter;
					StiComponent outFooter = null;
					if (groupFooter != null)
					{
						RenderOneGroupFooter(masterDataBand, groupFooter, ref outFooter, outContainer);
						if (outFooter != null)
						{
							DecFreeSpace(masterDataBand, outFooter);
						}
					}
					masterDataBand.DataBandInfoV1.FirstGroupOnPass = false;
					if (groupFooter != null && groupFooter.PrintAtBottom && outFooter != null)
					{
						AddBottomRenderedGroupFooters(masterDataBand, outFooter, groupFooter);
						groupFooter.GroupFooterBandInfoV1.PrintAtBottomComponent = outFooter;
					}
				}
			}
			if (masterDataBand.ColumnDirection == StiColumnDirection.DownThenAcross && masterDataBand.DataBandInfoV1.ItemsActive && masterDataBand.Position >= position + num - 1)
			{
				breaked = true;
			}
			if (flag)
			{
				MoveNext(masterDataBand);
				if (masterDataBand.IsEof)
				{
					breaked = true;
				}
				else if (masterDataBand.StartNewPage && !masterDataBand.DataBandInfoV1.SkipStartNewPage)
				{
					if (!masterDataBand.PrintIfDetailEmpty)
					{
						while (!masterDataBand.IsEof && IsDetailDataSourcesEmpty(masterDataBand))
						{
							MoveNext(masterDataBand);
						}
					}
					if (!masterDataBand.IsEof)
					{
						double num3 = 100.0 * masterDataBand.DataBandInfoV1.FreeSpace / masterDataBand.Page.Height;
						if ((double)masterDataBand.StartNewPageIfLessThan > num3 || masterDataBand.StartNewPageIfLessThan == 100f)
						{
							flag = false;
							breaked = true;
						}
					}
				}
				masterDataBand.DataBandInfoV1.FirstRow = false;
				masterDataBand.DataBandInfoV1.FirstRowInPath = false;
			}
			if (breaked)
			{
				break;
			}
		}
		RenderFootersOnAllPagesSecond(masterDataBand, ref footers, ref parentFooters, outContainer);
		int count5 = outContainer.Components.Count;
		ArrayList renderedFooters = null;
		if (masterDataBand.IsEof)
		{
			masterDataBand.InvokeEndRender();
		}
		bool flag4 = false;
		if (flag2)
		{
			flag4 = true;
		}
		if (!flag2)
		{
			bool newPageStarted = false;
			bool isBreaked = false;
			flag4 = RenderFootersSecond(masterDataBand, out renderedFooters, outContainer, ref newPageStarted, ref isBreaked);
			if (!flag4)
			{
				flag = false;
			}
			if (!flag4 && masterDataBand.KeepFooterTogether && !newPageStarted && !isBreaked && masterDataBand.DataBandInfoV1.FooterComponents.Count > 0)
			{
				foreach (StiComponent item3 in renderedFooters)
				{
					if (outContainer.Components.Contains(item3))
					{
						outContainer.Components.Remove(item3);
					}
				}
				RemoveFromContainerAt(masterDataBand, outContainer, count3 - num2, outContainer.Components.Count - num2);
				masterDataBand.Report.RestoreState(masterDataBand.Name + "Item");
				SetDetails(masterDataBand);
				masterDataBand.IsRendered = false;
				flag = false;
				masterDataBand.DataBandInfoV1.RemmitedCollection.Clear();
			}
		}
		if (masterDataBand.IsEof && masterDataBand.DataBandInfoV1.RemmitedCollection.Count == 0)
		{
			flag = true;
		}
		if (!flag4)
		{
			flag = false;
		}
		if (outContainer.Components.Count != count && arrayList != null)
		{
			foreach (StiComponent item4 in arrayList)
			{
				item4.DockStyle = StiDockStyle.Top;
			}
		}
		return flag;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiDataBand stiDataBand = masterComp as StiDataBand;
		stiDataBand.DataBandInfoV1.DataIsPrepared = false;
		if (stiDataBand.IsDataSourceEmpty)
		{
			stiDataBand.IsEof = false;
		}
		stiDataBand.DataBandInfoV1.FirstCall = true;
		stiDataBand.DataBandInfoV1.HeaderComponents = GetHeaders(stiDataBand);
		stiDataBand.DataBandInfoV1.FooterComponents = GetFooters(stiDataBand);
		stiDataBand.DataBandInfoV1.DetailComponents = GetDetails(stiDataBand);
		stiDataBand.DataBandInfoV1.DetailDataComponents = GetDataDetails(stiDataBand);
		stiDataBand.DataBandInfoV1.SubReportsComponents = GetSubReports(stiDataBand);
		stiDataBand.DataBandInfoV1.GroupHeaderComponents = GetGroupHeaders(stiDataBand);
		stiDataBand.DataBandInfoV1.GroupFooterComponents = GetGroupFooters(stiDataBand);
		StiFilterHelper.SetFilter(stiDataBand);
		GroupsComparison(stiDataBand);
		foreach (StiComponent headerComponent in stiDataBand.DataBandInfoV1.HeaderComponents)
		{
			headerComponent.Prepare();
		}
		foreach (StiComponent footerComponent in stiDataBand.DataBandInfoV1.FooterComponents)
		{
			footerComponent.Prepare();
		}
		foreach (StiGroupHeaderBand groupHeaderComponent in stiDataBand.DataBandInfoV1.GroupHeaderComponents)
		{
			groupHeaderComponent.Line = 0;
			groupHeaderComponent.Prepare();
		}
		foreach (StiComponent groupFooterComponent in stiDataBand.DataBandInfoV1.GroupFooterComponents)
		{
			if (groupFooterComponent.Parent != null)
			{
				groupFooterComponent.Prepare();
			}
		}
		stiDataBand.DataBandInfoV1.RemmitedCollection.Clear();
		if (!IsCrossColumn(stiDataBand))
		{
			stiDataBand.InvokeBeginRender();
		}
		stiDataBand.DataBandInfoV1.LastPositionRendering = -1;
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiDataBand stiDataBand = masterComp as StiDataBand;
		stiDataBand.DataBandInfoV1.HeaderComponents = null;
		stiDataBand.DataBandInfoV1.FooterComponents = null;
		stiDataBand.DataBandInfoV1.DetailComponents = null;
		stiDataBand.DataBandInfoV1.DetailDataComponents = null;
		stiDataBand.DataBandInfoV1.SubReportsComponents = null;
		stiDataBand.DataBandInfoV1.GroupHeaderComponents = null;
		stiDataBand.DataBandInfoV1.GroupFooterComponents = null;
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiDataBand stiDataBand = masterComp as StiDataBand;
		if (stiDataBand.Columns > 1 && stiDataBand.ColumnDirection == StiColumnDirection.DownThenAcross)
		{
			stiDataBand.Report.Column = 1;
		}
		bool flag = false;
		if (stiDataBand.Columns > 1)
		{
			stiDataBand.DataBandInfoV1.ItemsActive = true;
		}
		flag = InternalRender(stiDataBand, ref renderedComponent, outContainer, value: true);
		stiDataBand.DataBandInfoV1.ItemsActive = false;
		if (stiDataBand.Columns > 1 && stiDataBand.ColumnDirection == StiColumnDirection.DownThenAcross)
		{
			stiDataBand.DataBandInfoV1.FreeSpace = stiDataBand.Page.Height * (double)stiDataBand.Columns * 2.0;
			if (!stiDataBand.IsEof)
			{
				for (int i = 0; i < stiDataBand.Columns; i++)
				{
					stiDataBand.Report.Column = i + 1;
					foreach (StiComponent renderedItem in stiDataBand.DataBandInfoV1.RenderedItems)
					{
						StiContainer stiContainer = renderedItem as StiContainer;
						StiContainer renderContainer = StiContainerV1Builder.GetRenderContainer(outContainer, stiContainer);
						renderContainer.DockStyle = (stiDataBand.RightToLeft ? StiDockStyle.Right : StiDockStyle.Left);
						renderContainer.Width = stiDataBand.GetColumnWidth() + stiDataBand.ColumnGaps;
						if (i == 0)
						{
							StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
							foreach (StiComponent component in stiContainer.Components)
							{
								stiComponentsCollection.Add(component);
							}
							renderContainer.Components.AddRange(stiComponentsCollection);
							stiContainer.Components.Remove(stiComponentsCollection);
							renderContainer.Components.SetParent(renderContainer);
							stiContainer.Components.Add(renderContainer);
							renderContainer.Parent = stiContainer;
							renderContainer.Left = (stiDataBand.RightToLeft ? (stiDataBand.Width - stiDataBand.GetColumnWidth()) : 0.0);
							if (stiDataBand.RightToLeft)
							{
								renderContainer.OffsetLocation(stiDataBand.ColumnGaps, 0.0);
							}
						}
						else
						{
							int position = stiDataBand.Position;
							bool breaked = false;
							RenderItem(stiDataBand, out breaked, renderContainer);
							stiContainer.Components.Add(renderContainer);
							if (stiDataBand.RightToLeft && renderContainer.Components[0] is StiContainer)
							{
								(renderContainer.Components[0] as StiContainer).OffsetLocation(stiDataBand.ColumnGaps, 0.0);
							}
							renderContainer.Parent = stiContainer;
							MoveNext(stiDataBand);
							if (stiDataBand.IsEof)
							{
								stiDataBand.InvokeEndRender();
								return true;
							}
						}
					}
				}
			}
		}
		return flag;
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		StiDataBand stiDataBand = masterComp as StiDataBand;
		stiDataBand.DataBandInfoV1.ColumnIndex = 1;
		stiDataBand.DataBandInfoV1.CrossTabExistOnDataBand = false;
		foreach (StiComponent component in stiDataBand.Components)
		{
			if (component is IStiCrossTab)
			{
				stiDataBand.DataBandInfoV1.CrossTabExistOnDataBand = true;
				break;
			}
		}
		if (stiDataBand.StartNewPage && !stiDataBand.Page.PageInfoV1.IsFirstDataBandOnPage && stiDataBand.Parent is StiPage)
		{
			double freeSpaceFromRectangle = GetFreeSpaceFromRectangle(stiDataBand.GetDockRegion(outContainer));
			double num = 100.0 * freeSpaceFromRectangle / stiDataBand.Page.Height;
			if ((double)stiDataBand.StartNewPageIfLessThan > num || stiDataBand.StartNewPageIfLessThan == 100f)
			{
				return false;
			}
		}
		CheckParentContainerBeforeRendering(stiDataBand, outContainer);
		SetLastDataBand(stiDataBand);
		if (IsDataBandInMasterBand(stiDataBand))
		{
			outContainer.Height = 1000000000.0;
		}
		bool result = InternalRender(stiDataBand, ref renderedComponent, outContainer);
		CheckParentContainerAfterRendering(stiDataBand, outContainer);
		SetParentGrowToHeightInContainer(stiDataBand, outContainer);
		if (IsDataBandInMasterBand(stiDataBand) && stiDataBand.Parent.Parent is StiDataBand && ((StiDataBand)stiDataBand.Parent.Parent).KeepChildTogether)
		{
			MoveDataBandsToMasterDataband(stiDataBand, outContainer);
		}
		if (stiDataBand.MasterComponent == null)
		{
			stiDataBand.Page.PageInfoV1.IsFirstDataBandOnPage = false;
		}
		foreach (StiGroupFooterBand groupFooterComponent in stiDataBand.DataBandInfoV1.GroupFooterComponents)
		{
			groupFooterComponent.GroupFooterBandInfoV1.PrintAtBottomComponent = null;
		}
		stiDataBand.DataBandInfoV1.ParentColumnContainer = null;
		return result;
	}
}
