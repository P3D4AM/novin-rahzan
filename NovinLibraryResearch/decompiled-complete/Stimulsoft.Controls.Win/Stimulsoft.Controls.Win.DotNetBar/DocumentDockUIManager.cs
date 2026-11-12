using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DocumentDockUIManager
{
	private DocumentDockContainer m_DocumentDockContainer;

	private Cursor m_ContainerCursor;

	private DockSite m_Container;

	private Point m_MouseDownPoint = Point.Empty;

	private bool m_ResizePreview;

	private Form m_Splitter;

	private DocumentDockContainer m_ResizeDockContainer;

	private DocumentBaseContainer m_ResizeDocument;

	private int m_SplitterWidth = 3;

	private bool m_LoadingLayout;

	public DocumentDockContainer RootDocumentDockContainer
	{
		get
		{
			return m_DocumentDockContainer;
		}
		set
		{
			m_DocumentDockContainer = value;
		}
	}

	public DockSite Container
	{
		get
		{
			return m_Container;
		}
		set
		{
			m_Container = value;
		}
	}

	public bool IsResizingDocument => m_ResizeDocument != null;

	public DocumentDockUIManager(DockSite container)
	{
		m_Container = container;
	}

	internal Rectangle GetDockPreviewRectangle(Bar bar, ref DockSiteInfo dockInfo)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Invalid comparison between Unknown and I4
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and I4
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Invalid comparison between Unknown and I4
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Invalid comparison between Unknown and I4
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Invalid comparison between Unknown and I4
		DocumentBaseContainer documentFromBar = GetDocumentFromBar(dockInfo.MouseOverBar);
		Rectangle result = Rectangle.Empty;
		if ((int)((Control)m_Container).Dock != 5 && (dockInfo.DockLine == 999 || dockInfo.DockLine == -1))
		{
			return GetEdgeDockPreviewRectangle(bar, ref dockInfo);
		}
		if (documentFromBar != null)
		{
			if (dockInfo.MouseOverDockSide == eDockSide.Left)
			{
				result = documentFromBar.DisplayBounds;
				result.Width /= 2;
			}
			else if (dockInfo.MouseOverDockSide == eDockSide.Right)
			{
				result = documentFromBar.DisplayBounds;
				result.X += result.Width / 2;
				result.Width /= 2;
			}
			else if (dockInfo.MouseOverDockSide == eDockSide.Top)
			{
				result = documentFromBar.DisplayBounds;
				result.Height /= 2;
			}
			else if (dockInfo.MouseOverDockSide == eDockSide.Bottom)
			{
				result = documentFromBar.DisplayBounds;
				result.Y += result.Height / 2;
				result.Height /= 2;
			}
			else if (dockInfo.MouseOverDockSide == eDockSide.Document)
			{
				result = documentFromBar.DisplayBounds;
			}
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
		}
		else if ((int)((Control)m_Container).Dock == 5)
		{
			result = ((Control)m_Container).ClientRectangle;
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
		}
		else if ((int)((Control)m_Container).Dock == 4)
		{
			result = ((Control)m_Container).ClientRectangle;
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
			if (result.Width == 0)
			{
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
				result.X -= result.Width;
			}
		}
		else if ((int)((Control)m_Container).Dock == 3)
		{
			result = ((Control)m_Container).ClientRectangle;
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
			if (result.Width == 0)
			{
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
			}
		}
		else if ((int)((Control)m_Container).Dock == 1)
		{
			result = ((Control)m_Container).ClientRectangle;
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
			if (result.Height == 0)
			{
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
			}
		}
		else if ((int)((Control)m_Container).Dock == 2)
		{
			result = ((Control)m_Container).ClientRectangle;
			result.Location = ((Control)m_Container).PointToScreen(result.Location);
			if (result.Height == 0)
			{
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
				result.Y -= result.Height;
			}
		}
		return result;
	}

	private Rectangle GetEdgeDockPreviewRectangle(Bar bar, ref DockSiteInfo dockInfo)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected I4, but got Unknown
		Rectangle empty = Rectangle.Empty;
		int num = -1;
		int num2 = -1;
		if (dockInfo.FullSizeDock)
		{
			num = m_Container.GetFullSizeIndex();
		}
		else if (dockInfo.PartialSizeDock)
		{
			num2 = m_Container.GetPartialSizeIndex();
		}
		DockStyle dock = ((Control)m_Container).Dock;
		switch (dock - 1)
		{
		case 0:
		{
			empty.Width = ((Control)m_Container).ClientRectangle.Width;
			empty.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
			if (num >= 0)
			{
				empty.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: true).Width;
				empty.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, bFullSize: true).Width;
				dockInfo.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: false).Width;
				empty.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, bFullSize: false).Width;
				dockInfo.DockSiteZOrderIndex = num2;
			}
			Point empty4 = Point.Empty;
			if (dockInfo.DockLine == -1)
			{
				empty4 = ((Control)m_Container).PointToScreen(((Control)m_Container).ClientRectangle.Location);
			}
			else
			{
				empty4 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.X, ((Control)m_Container).ClientRectangle.Bottom));
				empty4.Y++;
			}
			if (num >= 0)
			{
				empty4.X -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: true).Width;
			}
			else if (num2 >= 0)
			{
				empty4.X += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: false).Width;
			}
			empty.Location = empty4;
			break;
		}
		case 1:
		{
			empty.Width = ((Control)m_Container).ClientRectangle.Width;
			empty.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
			if (num >= 0)
			{
				empty.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: true).Width;
				empty.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, bFullSize: true).Width;
				dockInfo.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: false).Width;
				empty.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, bFullSize: false).Width;
				dockInfo.DockSiteZOrderIndex = num2;
			}
			Point empty3 = Point.Empty;
			if (dockInfo.DockLine == -1)
			{
				empty3 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.X, ((Control)m_Container).ClientRectangle.Y - empty.Height));
			}
			else
			{
				empty3 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.X, ((Control)m_Container).ClientRectangle.Bottom - empty.Height));
				empty3.Y++;
			}
			if (num >= 0)
			{
				empty3.X -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: true).Width;
			}
			else if (num2 >= 0)
			{
				empty3.X += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, bFullSize: false).Width;
			}
			empty.Location = empty3;
			break;
		}
		case 3:
		{
			empty.Height = ((Control)m_Container).ClientRectangle.Height;
			if (num >= 0)
			{
				empty.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: true).Height;
				empty.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, bFullSize: true).Height;
				dockInfo.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: false).Height;
				empty.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, bFullSize: false).Height;
				dockInfo.DockSiteZOrderIndex = num2;
			}
			empty.Width = bar.GetBarDockedSize(eOrientation.Vertical);
			Point empty5 = Point.Empty;
			if (dockInfo.DockLine == -1)
			{
				empty5 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.X - empty.Width, ((Control)m_Container).ClientRectangle.Y));
			}
			else
			{
				empty5 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.Right - empty.Width, ((Control)m_Container).ClientRectangle.Y));
				empty5.X--;
			}
			if (num >= 0)
			{
				empty5.Y -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: true).Height;
			}
			else if (num2 >= 0)
			{
				empty5.Y += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: false).Height;
			}
			empty.Location = empty5;
			break;
		}
		default:
		{
			empty.Height = ((Control)m_Container).ClientRectangle.Height;
			if (num >= 0)
			{
				empty.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: true).Height;
				empty.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, bFullSize: true).Height;
				dockInfo.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: false).Height;
				empty.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, bFullSize: false).Height;
				dockInfo.DockSiteZOrderIndex = num2;
			}
			empty.Width = bar.GetBarDockedSize(eOrientation.Vertical);
			Point empty2 = Point.Empty;
			if (dockInfo.DockLine == -1)
			{
				empty2 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.X, ((Control)m_Container).ClientRectangle.Y));
			}
			else
			{
				empty2 = ((Control)m_Container).PointToScreen(new Point(((Control)m_Container).ClientRectangle.Right, ((Control)m_Container).ClientRectangle.Y));
				empty2.X++;
			}
			if (num >= 0)
			{
				empty2.Y -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: true).Height;
			}
			else if (num2 >= 0)
			{
				empty2.Y += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, bFullSize: false).Height;
			}
			empty.Location = empty2;
			break;
		}
		}
		return empty;
	}

	public DocumentBaseContainer GetDocumentFromBar(Bar bar)
	{
		if (bar == null)
		{
			return null;
		}
		return GetDocumentFromBar(bar, m_DocumentDockContainer);
	}

	private DocumentBaseContainer GetDocumentFromBar(Bar bar, DocumentDockContainer dockContainer)
	{
		foreach (DocumentBaseContainer document in dockContainer.Documents)
		{
			if (document is DocumentBarContainer && ((DocumentBarContainer)document).Bar == bar)
			{
				return document;
			}
			if (document is DocumentDockContainer)
			{
				DocumentBaseContainer documentFromBar = GetDocumentFromBar(bar, document as DocumentDockContainer);
				if (documentFromBar != null)
				{
					return documentFromBar;
				}
			}
		}
		return null;
	}

	private bool IsAddedInFront(eDockSide dockSide)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		if (((int)((Control)m_Container).Dock == 3 && dockSide == eDockSide.Left) || ((int)((Control)m_Container).Dock == 4 && dockSide == eDockSide.Left) || ((int)((Control)m_Container).Dock == 1 && dockSide == eDockSide.Top) || ((int)((Control)m_Container).Dock == 2 && dockSide == eDockSide.Top))
		{
			return true;
		}
		return false;
	}

	public void Dock(Bar barToDock)
	{
		Dock(null, barToDock, eDockSide.None);
	}

	public void Dock(Bar referenceBar, Bar barToDock, eDockSide dockSide)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Invalid comparison between Unknown and I4
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Invalid comparison between Unknown and I4
		if (dockSide == eDockSide.None && ((Control)barToDock).Parent == m_Container)
		{
			ApplyBarStyle(barToDock);
			return;
		}
		if (((Control)barToDock).Parent is DockSite && ((DockSite)(object)((Control)barToDock).Parent).DocumentDockContainer != null)
		{
			((DockSite)(object)((Control)barToDock).Parent).GetDocumentUIManager().UnDock(barToDock);
		}
		else if (((Control)barToDock).Parent is DockSite && ((Control)barToDock).Parent != m_Container)
		{
			((DockSite)(object)((Control)barToDock).Parent).RemoveBar((Control)(object)barToDock);
		}
		else if (((Control)barToDock).Parent != null && ((Control)barToDock).Parent != m_Container)
		{
			if (((Control)barToDock).Parent is FloatingContainer)
			{
				barToDock.RemoveFromFloatingContainer();
				barToDock.SetBarState(eBarState.Docked);
			}
			else
			{
				((Control)barToDock).Parent.Controls.Remove((Control)(object)barToDock);
			}
		}
		if (!m_LoadingLayout)
		{
			DocumentBarContainer documentBarContainer = CreateDocumentBarContainer(barToDock);
			DocumentBaseContainer documentFromBar = GetDocumentFromBar(referenceBar);
			if (referenceBar == null || dockSide == eDockSide.None || referenceBar == barToDock || documentFromBar == null)
			{
				if ((int)((Control)m_Container).Dock == 5)
				{
					m_DocumentDockContainer.Documents.Add(documentBarContainer);
				}
				else
				{
					eOrientation eOrientation2 = eOrientation.Horizontal;
					if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
					{
						eOrientation2 = eOrientation.Vertical;
					}
					if (m_DocumentDockContainer.Orientation != eOrientation2)
					{
						if (m_DocumentDockContainer.Documents.Count <= 1)
						{
							m_DocumentDockContainer.Orientation = eOrientation2;
							if (IsAddedInFront(dockSide))
							{
								m_DocumentDockContainer.Documents.Insert(0, documentBarContainer);
							}
							else
							{
								m_DocumentDockContainer.Documents.Add(documentBarContainer);
							}
						}
						else
						{
							DocumentBaseContainer[] array = new DocumentBaseContainer[m_DocumentDockContainer.Documents.Count];
							m_DocumentDockContainer.Documents.CopyTo(array);
							m_DocumentDockContainer.Documents.Clear();
							DocumentDockContainer documentDockContainer = new DocumentDockContainer(array, m_DocumentDockContainer.Orientation);
							documentDockContainer.SetLayoutBounds(m_DocumentDockContainer.DisplayBounds);
							m_DocumentDockContainer.Orientation = eOrientation2;
							m_DocumentDockContainer.Documents.Add(documentDockContainer);
							if (IsAddedInFront(dockSide))
							{
								m_DocumentDockContainer.Documents.Insert(0, documentBarContainer);
							}
							else
							{
								m_DocumentDockContainer.Documents.Add(documentBarContainer);
							}
						}
					}
					else if (IsAddedInFront(dockSide))
					{
						m_DocumentDockContainer.Documents.Insert(0, documentBarContainer);
					}
					else
					{
						m_DocumentDockContainer.Documents.Add(documentBarContainer);
					}
				}
			}
			else
			{
				DocumentDockContainer documentDockContainer2 = documentFromBar.Parent as DocumentDockContainer;
				documentFromBar.SetLayoutBounds(Rectangle.Empty);
				documentBarContainer.SetLayoutBounds(Rectangle.Empty);
				if ((documentDockContainer2.Orientation == eOrientation.Horizontal && (dockSide == eDockSide.Left || dockSide == eDockSide.Right)) || (documentDockContainer2.Orientation == eOrientation.Vertical && (dockSide == eDockSide.Top || dockSide == eDockSide.Bottom)))
				{
					if (dockSide == eDockSide.Right || dockSide == eDockSide.Bottom)
					{
						documentDockContainer2.Documents.Insert(documentDockContainer2.Documents.IndexOf(documentFromBar) + 1, documentBarContainer);
					}
					else
					{
						documentDockContainer2.Documents.Insert(documentDockContainer2.Documents.IndexOf(documentFromBar), documentBarContainer);
					}
				}
				else if (documentDockContainer2.Documents.Count == 1)
				{
					if (documentDockContainer2.Orientation == eOrientation.Vertical)
					{
						documentDockContainer2.Orientation = eOrientation.Horizontal;
					}
					else
					{
						documentDockContainer2.Orientation = eOrientation.Vertical;
					}
					if (dockSide == eDockSide.Right || dockSide == eDockSide.Bottom)
					{
						documentDockContainer2.Documents.Insert(documentDockContainer2.Documents.IndexOf(documentFromBar) + 1, documentBarContainer);
					}
					else
					{
						documentDockContainer2.Documents.Insert(documentDockContainer2.Documents.IndexOf(documentFromBar), documentBarContainer);
					}
				}
				else
				{
					DocumentDockContainer documentDockContainer3 = new DocumentDockContainer();
					if (documentDockContainer2.Orientation == eOrientation.Horizontal)
					{
						documentDockContainer3.Orientation = eOrientation.Vertical;
					}
					else
					{
						documentDockContainer3.Orientation = eOrientation.Horizontal;
					}
					documentDockContainer2.Documents.Insert(documentDockContainer2.Documents.IndexOf(documentFromBar), documentDockContainer3);
					documentDockContainer2.Documents.Remove(documentFromBar);
					if (dockSide == eDockSide.Right || dockSide == eDockSide.Bottom)
					{
						documentDockContainer3.Documents.Add(documentFromBar);
						documentDockContainer3.Documents.Add(documentBarContainer);
					}
					else
					{
						documentDockContainer3.Documents.Add(documentBarContainer);
						documentDockContainer3.Documents.Add(documentFromBar);
					}
				}
			}
			AdjustContainerSize(barToDock, visibleChanged: false);
		}
		if (m_Container != null)
		{
			if (((Control)barToDock).Parent == null)
			{
				((Control)m_Container).Controls.Add((Control)(object)barToDock);
			}
			ApplyBarStyle(barToDock);
			m_Container.RecalcLayout();
		}
		else
		{
			ApplyBarStyle(barToDock);
		}
	}

	internal void AdjustContainerSize(Bar barToDock, bool visibleChanged)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Invalid comparison between Unknown and I4
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Invalid comparison between Unknown and I4
		if ((m_Container.Owner != null && m_Container.Owner.IsLoadingLayout) || (int)((Control)m_Container).Dock == 5)
		{
			return;
		}
		DocumentBaseContainer documentFromBar = GetDocumentFromBar(barToDock);
		if (documentFromBar == null)
		{
			return;
		}
		if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
		{
			if (((Control)m_Container).Width != 0 && (documentFromBar.Parent != m_DocumentDockContainer || (m_DocumentDockContainer.Orientation != 0 && (!visibleChanged || m_DocumentDockContainer.Documents.Count != 1))))
			{
				return;
			}
			int num = barToDock.GetBarDockedSize(eOrientation.Vertical);
			if (((Control)barToDock).Width > 0 && ((Control)barToDock).Width < ((Control)m_Container).Width)
			{
				num = ((Control)barToDock).Width;
			}
			int clientWidth = GetClientWidth();
			if (barToDock.Visible)
			{
				if (num > clientWidth)
				{
					num = barToDock.MinimumDockSize(eOrientation.Vertical).Width * 2;
				}
				if (num > clientWidth)
				{
					num = barToDock.MinimumDockSize(eOrientation.Vertical).Width;
				}
			}
			if (documentFromBar.LayoutBounds.Width == 0)
			{
				documentFromBar.SetLayoutBounds(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, num, documentFromBar.LayoutBounds.Height));
			}
			num += m_DocumentDockContainer.SplitterSize;
			if (visibleChanged)
			{
				if (barToDock.Visible)
				{
					DockSite container = m_Container;
					((Control)container).Width = ((Control)container).Width + num;
				}
				else if (((Control)m_Container).Width > 0)
				{
					DockSite container2 = m_Container;
					((Control)container2).Width = ((Control)container2).Width - Math.Min(num, ((Control)m_Container).Width);
				}
			}
			else
			{
				DockSite container3 = m_Container;
				((Control)container3).Width = ((Control)container3).Width + num;
			}
			((Control)m_Container).Invalidate();
		}
		else
		{
			if (((int)((Control)m_Container).Dock != 1 && (int)((Control)m_Container).Dock != 2) || (((Control)m_Container).Height != 0 && (documentFromBar.Parent != m_DocumentDockContainer || (m_DocumentDockContainer.Orientation != eOrientation.Vertical && (!visibleChanged || m_DocumentDockContainer.Documents.Count != 1)))))
			{
				return;
			}
			int num2 = barToDock.GetBarDockedSize(eOrientation.Horizontal);
			if (((Control)barToDock).Height > 0 && ((Control)barToDock).Height < ((Control)m_Container).Height)
			{
				num2 = ((Control)barToDock).Height;
			}
			int clientHeight = GetClientHeight();
			if (barToDock.Visible)
			{
				if (num2 > clientHeight)
				{
					num2 = barToDock.MinimumDockSize(eOrientation.Horizontal).Height * 2;
				}
				if (num2 > clientHeight)
				{
					num2 = barToDock.MinimumDockSize(eOrientation.Horizontal).Height;
				}
			}
			if (documentFromBar.LayoutBounds.Height == 0)
			{
				documentFromBar.SetLayoutBounds(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentFromBar.LayoutBounds.Width, num2));
			}
			num2 += m_DocumentDockContainer.SplitterSize;
			if (visibleChanged)
			{
				if (barToDock.Visible)
				{
					DockSite container4 = m_Container;
					((Control)container4).Height = ((Control)container4).Height + num2;
				}
				else if (((Control)m_Container).Height > 0)
				{
					DockSite container5 = m_Container;
					((Control)container5).Height = ((Control)container5).Height - Math.Min(num2, ((Control)m_Container).Height);
				}
			}
			else
			{
				DockSite container6 = m_Container;
				((Control)container6).Height = ((Control)container6).Height + num2;
			}
			((Control)m_Container).Invalidate();
		}
	}

	private int GetClientWidth()
	{
		if (m_Container == null)
		{
			return 0;
		}
		return m_Container.GetClientWidth();
	}

	private int GetClientHeight()
	{
		if (m_Container == null)
		{
			return 0;
		}
		return m_Container.GetClientHeight();
	}

	public void UnDock(Bar barToUnDock)
	{
		UnDock(barToUnDock, removeFromContainer: true);
	}

	internal void SaveLastDockInfo(Bar bar, DocumentBarContainer doc, DockSiteInfo info)
	{
		if (bar.m_LastDockSiteInfo == null || !(doc.Parent is DocumentDockContainer))
		{
			return;
		}
		DocumentDockContainer documentDockContainer = (DocumentDockContainer)doc.Parent;
		DocumentBaseContainer nextDocument = GetNextDocument(documentDockContainer, documentDockContainer.Documents.IndexOf(doc));
		if (nextDocument == null)
		{
			DocumentBaseContainer previousDocument = GetPreviousDocument(documentDockContainer, documentDockContainer.Documents.IndexOf(doc));
			if (previousDocument != null)
			{
				Bar bar2 = null;
				if (previousDocument is DocumentBarContainer)
				{
					bar2 = ((DocumentBarContainer)previousDocument).Bar;
					bar.m_LastDockSiteInfo.LastRelativeDockToBar = bar2;
					bar2.m_LastDockSiteInfo.LastRelativeDockToBar = bar;
					bar2.m_LastDockSiteInfo.LastRelativeDocumentId = doc.Id;
				}
				bar.m_LastDockSiteInfo.LastRelativeDocumentId = previousDocument.Id;
				if (documentDockContainer.Orientation == eOrientation.Horizontal)
				{
					bar.LastDockSide = eDockSide.Right;
					if (bar2 != null)
					{
						bar2.LastDockSide = eDockSide.Left;
					}
				}
				else
				{
					bar.LastDockSide = eDockSide.Bottom;
					if (bar2 != null)
					{
						bar2.LastDockSide = eDockSide.Top;
					}
				}
			}
		}
		else
		{
			Bar bar3 = null;
			if (nextDocument is DocumentBarContainer)
			{
				bar3 = ((DocumentBarContainer)nextDocument).Bar;
				bar.m_LastDockSiteInfo.LastRelativeDockToBar = bar3;
				bar3.m_LastDockSiteInfo.LastRelativeDockToBar = bar;
				bar3.m_LastDockSiteInfo.LastRelativeDocumentId = doc.Id;
			}
			bar.m_LastDockSiteInfo.LastRelativeDocumentId = nextDocument.Id;
			if (documentDockContainer.Orientation == eOrientation.Horizontal)
			{
				bar.LastDockSide = eDockSide.Left;
				if (bar3 != null)
				{
					bar3.LastDockSide = eDockSide.Right;
				}
			}
			else
			{
				bar.LastDockSide = eDockSide.Top;
				if (bar3 != null)
				{
					bar3.LastDockSide = eDockSide.Bottom;
				}
			}
		}
		if (GetDockSide(bar) != 0)
		{
			bar.m_LastDockSiteInfo.LastDockSiteSide = GetDockSide(bar);
		}
	}

	private eDockSide GetDockSide(Bar bar)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		if (((Control)bar).Parent == null)
		{
			return eDockSide.None;
		}
		if ((int)((Control)bar).Parent.Dock == 3)
		{
			return eDockSide.Left;
		}
		if ((int)((Control)bar).Parent.Dock == 4)
		{
			return eDockSide.Right;
		}
		if ((int)((Control)bar).Parent.Dock == 1)
		{
			return eDockSide.Top;
		}
		if ((int)((Control)bar).Parent.Dock == 2)
		{
			return eDockSide.Bottom;
		}
		if ((int)((Control)bar).Parent.Dock == 5)
		{
			return eDockSide.Document;
		}
		return eDockSide.None;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void UnDock(Bar barToUnDock, bool removeFromContainer)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Invalid comparison between Unknown and I4
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Invalid comparison between Unknown and I4
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Invalid comparison between Unknown and I4
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Invalid comparison between Unknown and I4
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Invalid comparison between Unknown and I4
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Invalid comparison between Unknown and I4
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Invalid comparison between Unknown and I4
		DocumentBarContainer barDocumentContainer = m_DocumentDockContainer.GetBarDocumentContainer(barToUnDock);
		if (barToUnDock.DockSide != 0)
		{
			barToUnDock.LastDockSide = barToUnDock.DockSide;
		}
		if (barDocumentContainer != null)
		{
			SaveLastDockInfo(barToUnDock, barDocumentContainer, barToUnDock.m_LastDockSiteInfo);
		}
		int num = 0;
		if (barDocumentContainer != null && m_Container != null && (int)((Control)m_Container).Dock != 5)
		{
			if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
			{
				if (barDocumentContainer.Parent != null && barDocumentContainer.Parent == m_DocumentDockContainer && m_DocumentDockContainer.Orientation == eOrientation.Horizontal)
				{
					num = ((Control)barToUnDock).Width + m_DocumentDockContainer.SplitterSize;
				}
			}
			else if (((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2) && barDocumentContainer.Parent != null && barDocumentContainer.Parent == m_DocumentDockContainer && m_DocumentDockContainer.Orientation == eOrientation.Vertical)
			{
				num = ((Control)barToUnDock).Height + m_DocumentDockContainer.SplitterSize;
			}
		}
		if (barDocumentContainer == null)
		{
			if (removeFromContainer && ((Control)barToUnDock).Parent == m_Container)
			{
				((Control)m_Container).Controls.Remove((Control)(object)barToUnDock);
			}
			RestoreBarStyle(barToUnDock);
			return;
		}
		if (barDocumentContainer.Parent is DocumentDockContainer documentDockContainer)
		{
			documentDockContainer.Documents.Remove(barDocumentContainer);
			if (documentDockContainer != m_DocumentDockContainer)
			{
				RemoveEmptyContainers(documentDockContainer);
			}
			else if (documentDockContainer.Documents.Count == 1 && documentDockContainer.Documents[0] is DocumentDockContainer)
			{
				RemoveEmptyContainers(documentDockContainer.Documents[0] as DocumentDockContainer);
			}
		}
		if (m_Container != null)
		{
			if (removeFromContainer)
			{
				((Control)m_Container).Controls.Remove((Control)(object)barToUnDock);
			}
			RestoreBarStyle(barToUnDock);
			bool flag = false;
			if (m_Container.Owner != null)
			{
				flag = m_Container.Owner.IsLoadingLayout;
			}
			if (!flag && !m_Container.IsAnyControlVisible)
			{
				if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
				{
					((Control)m_Container).Width = 0;
				}
				else if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
				{
					((Control)m_Container).Height = 0;
				}
			}
			else if (!flag && num != 0)
			{
				if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
				{
					DockSite container = m_Container;
					((Control)container).Width = ((Control)container).Width - num;
				}
				else if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
				{
					DockSite container2 = m_Container;
					((Control)container2).Height = ((Control)container2).Height - num;
				}
			}
			else
			{
				m_Container.RecalcLayout();
			}
		}
		else
		{
			RestoreBarStyle(barToUnDock);
		}
	}

	private void RemoveEmptyContainers(DocumentDockContainer parent)
	{
		while ((parent.Documents.Count == 0 && parent.Parent != null && parent.Parent is DocumentDockContainer) || (parent.Parent != null && parent.Parent is DocumentDockContainer && ((DocumentDockContainer)parent.Parent).Documents.Count == 1))
		{
			if (parent.Parent != null && parent.Parent is DocumentDockContainer && ((DocumentDockContainer)parent.Parent).Documents.Count == 1)
			{
				DocumentDockContainer documentDockContainer = parent.Parent as DocumentDockContainer;
				documentDockContainer.Documents.Remove(parent);
				documentDockContainer.Orientation = parent.Orientation;
				DocumentBaseContainer[] array = new DocumentBaseContainer[parent.Documents.Count];
				parent.Documents.CopyTo(array);
				documentDockContainer.Documents.AddRange(array);
				parent = documentDockContainer;
			}
			else
			{
				DocumentDockContainer documentDockContainer2 = parent.Parent as DocumentDockContainer;
				documentDockContainer2.Documents.Remove(parent);
				parent = documentDockContainer2;
			}
		}
	}

	private void ApplyBarStyle(Bar bar)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (m_Container != null && m_Container.Owner != null && m_Container.Owner.ApplyDocumentBarStyle && (int)((Control)m_Container).Dock == 5)
		{
			BarFunctions.ApplyAutoDocumentBarStyle(bar);
		}
	}

	private void RestoreBarStyle(Bar bar)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (m_Container != null && m_Container.Owner != null && m_Container.Owner.ApplyDocumentBarStyle && (int)((Control)m_Container).Dock == 5)
		{
			BarFunctions.RestoreAutoDocumentBarStyle(bar);
		}
	}

	private DocumentBarContainer CreateDocumentBarContainer(Bar bar)
	{
		return new DocumentBarContainer(bar);
	}

	private DocumentDockContainer CreateDocumentDockContainer()
	{
		return new DocumentDockContainer();
	}

	public void OnMouseMove(MouseEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		int x = e.X;
		int y = e.Y;
		if ((int)e.Button == 0)
		{
			Cursor cursor = GetCursor(x, y);
			if (cursor == (Cursor)null)
			{
				if (m_ContainerCursor != (Cursor)null)
				{
					((Control)Container).Cursor = m_ContainerCursor;
				}
				m_ContainerCursor = null;
			}
			else
			{
				if (m_ContainerCursor == (Cursor)null)
				{
					m_ContainerCursor = ((Control)Container).Cursor;
				}
				((Control)Container).Cursor = cursor;
			}
		}
		else
		{
			if ((int)e.Button != 1048576)
			{
				return;
			}
			Point resizeToPoint = GetResizeToPoint(x, y);
			if (!resizeToPoint.IsEmpty)
			{
				if (!m_ResizePreview)
				{
					SplitterMouseMove(resizeToPoint.X, resizeToPoint.Y);
					return;
				}
				ResizeTo(resizeToPoint.X, resizeToPoint.Y);
				m_MouseDownPoint = resizeToPoint;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Cursor GetCursor(int x, int y)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		Cursor result = null;
		DocumentDockContainer containerFromRoot = GetContainerFromRoot(x, y);
		if (containerFromRoot != null)
		{
			if (containerFromRoot.Orientation == eOrientation.Horizontal)
			{
				result = Cursors.VSplit;
			}
			else if (containerFromRoot.Orientation == eOrientation.Vertical)
			{
				result = Cursors.HSplit;
			}
		}
		else if (m_Container != null && (int)((Control)m_Container).Dock != 5)
		{
			if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
			{
				result = Cursors.HSplit;
			}
			else if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
			{
				result = Cursors.VSplit;
			}
		}
		return result;
	}

	public void OnMouseDown(MouseEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		int x = e.X;
		int y = e.Y;
		if ((int)e.Button != 1048576)
		{
			return;
		}
		m_MouseDownPoint = new Point(x, y);
		DocumentDockContainer containerFromRoot = GetContainerFromRoot(x, y);
		if (containerFromRoot != null)
		{
			m_ResizeDocument = GetClosestDocument(containerFromRoot, x, y);
			if (m_ResizeDocument != null)
			{
				m_ResizeDockContainer = containerFromRoot;
				if (!m_ResizePreview)
				{
					CreateSplitter();
					SplitterMouseMove(x, y);
				}
			}
		}
		else if (m_Container != null && (int)((Control)m_Container).Dock != 5)
		{
			m_ResizeDockContainer = m_DocumentDockContainer;
			if (!m_ResizePreview)
			{
				CreateSplitter();
				SplitterMouseMove(x, y);
			}
		}
	}

	public void OnMouseUp(MouseEventArgs e)
	{
		if (m_ResizeDockContainer != null && !m_MouseDownPoint.IsEmpty)
		{
			ResizeTo(e.X, e.Y);
		}
		m_MouseDownPoint = Point.Empty;
		m_ResizeDockContainer = null;
		m_ResizeDocument = null;
		DestroySplitter();
	}

	public void OnMouseLeave()
	{
		if (m_ContainerCursor != (Cursor)null)
		{
			((Control)Container).Cursor = m_ContainerCursor;
		}
		m_ContainerCursor = null;
	}

	private DocumentBaseContainer GetClosestDocument(DocumentDockContainer parent, int x, int y)
	{
		DocumentBaseContainer result = null;
		eOrientation orientation = parent.Orientation;
		foreach (DocumentBaseContainer document in parent.Documents)
		{
			if ((orientation == eOrientation.Horizontal && document.DisplayBounds.X > x && document.Visible) || (orientation == eOrientation.Vertical && document.DisplayBounds.Y > y && document.Visible))
			{
				break;
			}
			if (document.Visible)
			{
				result = document;
			}
		}
		return result;
	}

	private DocumentDockContainer GetContainerFromRoot(int x, int y)
	{
		return GetContainer(m_DocumentDockContainer, x, y);
	}

	private DocumentDockContainer GetContainer(DocumentDockContainer parent, int x, int y)
	{
		if (parent == null || !parent.DisplayBounds.Contains(x, y) || !parent.Visible)
		{
			return null;
		}
		DocumentDockContainer documentDockContainer = null;
		foreach (DocumentBaseContainer document in parent.Documents)
		{
			if (document is DocumentDockContainer)
			{
				documentDockContainer = GetContainer((DocumentDockContainer)document, x, y);
				if (documentDockContainer != null)
				{
					break;
				}
			}
		}
		if (documentDockContainer == null)
		{
			documentDockContainer = parent;
		}
		return documentDockContainer;
	}

	private void CreateSplitter()
	{
		if (m_ResizeDockContainer != null && m_Splitter == null)
		{
			m_Splitter = BarFunctions.CreateOutlineForm();
		}
	}

	private void SplitterMouseMove(int x, int y)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Invalid comparison between Unknown and I4
		if (m_Splitter != null && m_ResizeDockContainer != null)
		{
			Rectangle empty = Rectangle.Empty;
			if ((m_ResizeDockContainer.Orientation == eOrientation.Horizontal && m_ResizeDocument != null) || (m_ResizeDocument == null && ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)))
			{
				Point point = ((Control)m_Container).PointToScreen(new Point(x - m_SplitterWidth / 2, m_ResizeDockContainer.DisplayBounds.Y));
				empty.X = point.X;
				empty.Y = point.Y;
				empty.Height = m_ResizeDockContainer.DisplayBounds.Height;
				empty.Width = m_SplitterWidth;
			}
			else if ((m_ResizeDockContainer.Orientation == eOrientation.Vertical && m_ResizeDocument != null) || (m_ResizeDocument == null && ((int)((Control)m_Container).Dock == 2 || (int)((Control)m_Container).Dock == 1)))
			{
				Point point2 = ((Control)m_Container).PointToScreen(new Point(m_ResizeDockContainer.DisplayBounds.X, y - m_SplitterWidth / 2));
				empty.X = point2.X;
				empty.Y = point2.Y;
				empty.Width = m_ResizeDockContainer.DisplayBounds.Width;
				empty.Height = m_SplitterWidth;
			}
			NativeFunctions.SetWindowPos(((Control)m_Splitter).Handle, new IntPtr(0), empty.X, empty.Y, empty.Width, empty.Height, 80);
		}
	}

	private void DestroySplitter()
	{
		if (m_Splitter != null)
		{
			((Control)m_Splitter).Visible = false;
			((Component)(object)m_Splitter).Dispose();
			m_Splitter = null;
		}
	}

	internal Size GetContainerMinimumSize(DocumentDockContainer dockContainer)
	{
		Size empty = Size.Empty;
		foreach (DocumentBaseContainer document in dockContainer.Documents)
		{
			Size size = Size.Empty;
			if (document is DocumentBarContainer)
			{
				if (!((DocumentBarContainer)document).Visible)
				{
					continue;
				}
				size = document.MinimumSize;
			}
			else if (document is DocumentDockContainer)
			{
				size = GetContainerMinimumSize(document as DocumentDockContainer);
			}
			if (dockContainer.Orientation == eOrientation.Horizontal)
			{
				empty.Width += size.Width + dockContainer.SplitterSize;
				if (size.Height < empty.Height || empty.Height == 0)
				{
					empty.Height = size.Height;
				}
			}
			else
			{
				empty.Height += size.Height + dockContainer.SplitterSize;
				if (size.Width < empty.Width || empty.Width == 0)
				{
					empty.Width = size.Width;
				}
			}
		}
		if (empty.Width > 0 && dockContainer.Orientation == eOrientation.Horizontal)
		{
			empty.Width += dockContainer.SplitterSize;
		}
		else if (empty.Height > 0 && dockContainer.Orientation == eOrientation.Vertical)
		{
			empty.Height += dockContainer.SplitterSize;
		}
		if (empty.Width == 0)
		{
			empty.Width = 32;
		}
		if (empty.Height == 0)
		{
			empty.Height = 32;
		}
		return empty;
	}

	private Point GetResizeToPoint(int x, int y)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Invalid comparison between Unknown and I4
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Invalid comparison between Unknown and I4
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Invalid comparison between Unknown and I4
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Invalid comparison between Unknown and I4
		if (m_ResizeDockContainer == null)
		{
			return Point.Empty;
		}
		Point result = new Point(x, y);
		int num = x - m_MouseDownPoint.X;
		int num2 = y - m_MouseDownPoint.Y;
		if (m_ResizeDocument == null)
		{
			if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
			{
				if ((int)((Control)m_Container).Dock == 4)
				{
					num *= -1;
				}
				Size containerMinimumSize = GetContainerMinimumSize(m_ResizeDockContainer);
				if (m_ResizeDockContainer.DisplayBounds.Width + num < containerMinimumSize.Width)
				{
					result.X -= (m_ResizeDockContainer.DisplayBounds.Width + num - containerMinimumSize.Width) * (((int)((Control)m_Container).Dock != 4) ? 1 : (-1));
				}
				else
				{
					int clientWidth = GetClientWidth();
					int num3 = 32;
					if (m_Container.Owner != null)
					{
						num3 = m_Container.Owner.MinimumClientSize.Width;
					}
					if (clientWidth - num < num3)
					{
						result.X -= (num3 - (clientWidth - num)) * (((int)((Control)m_Container).Dock != 4) ? 1 : (-1));
					}
				}
				return result;
			}
			if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
			{
				if ((int)((Control)m_Container).Dock == 2)
				{
					num2 *= -1;
				}
				Size containerMinimumSize2 = GetContainerMinimumSize(m_ResizeDockContainer);
				if (m_ResizeDockContainer.DisplayBounds.Height + num2 < containerMinimumSize2.Height)
				{
					result.Y -= (m_ResizeDockContainer.DisplayBounds.Height + num2 - containerMinimumSize2.Height) * (((int)((Control)m_Container).Dock != 2) ? 1 : (-1));
				}
				else
				{
					int clientHeight = GetClientHeight();
					int num4 = 32;
					if (m_Container.Owner != null)
					{
						num4 = m_Container.Owner.MinimumClientSize.Height;
					}
					if (clientHeight - num2 < num4)
					{
						result.Y -= (num4 - (clientHeight - num2)) * (((int)((Control)m_Container).Dock != 2) ? 1 : (-1));
					}
				}
				return result;
			}
			return Point.Empty;
		}
		if (m_ResizeDockContainer.Orientation == eOrientation.Horizontal)
		{
			if (m_ResizeDocument.DisplayBounds.Width + num >= m_ResizeDocument.MinimumSize.Width)
			{
				DocumentBaseContainer nextDocument = GetNextDocument(m_ResizeDockContainer, m_ResizeDockContainer.Documents.IndexOf(m_ResizeDocument));
				if (nextDocument != null && nextDocument.DisplayBounds.Width - num < nextDocument.MinimumSize.Width)
				{
					result.X += nextDocument.DisplayBounds.Width - num - nextDocument.MinimumSize.Width;
				}
			}
			else
			{
				result.X -= m_ResizeDocument.DisplayBounds.Width + num - m_ResizeDocument.MinimumSize.Width;
			}
		}
		else if (m_ResizeDockContainer.Orientation == eOrientation.Vertical)
		{
			if (m_ResizeDocument.DisplayBounds.Height + num2 >= m_ResizeDocument.MinimumSize.Height)
			{
				DocumentBaseContainer nextDocument2 = GetNextDocument(m_ResizeDockContainer, m_ResizeDockContainer.Documents.IndexOf(m_ResizeDocument));
				if (nextDocument2 != null && nextDocument2.DisplayBounds.Height - num2 < nextDocument2.MinimumSize.Height)
				{
					result.Y += nextDocument2.DisplayBounds.Height - num2 - nextDocument2.MinimumSize.Height;
				}
			}
			else
			{
				result.Y -= m_ResizeDocument.DisplayBounds.Height + num2 - m_ResizeDocument.MinimumSize.Height;
			}
		}
		return result;
	}

	private void ResetLayoutBounds(DocumentDockContainer doc)
	{
		foreach (DocumentBaseContainer document in doc.Documents)
		{
			document.SetLayoutBounds(Rectangle.Empty);
		}
	}

	private void ResizeTo(int x, int y)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Invalid comparison between Unknown and I4
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Invalid comparison between Unknown and I4
		if (m_ResizeDockContainer == null)
		{
			return;
		}
		Point resizeToPoint = GetResizeToPoint(x, y);
		int num = resizeToPoint.X - m_MouseDownPoint.X;
		int num2 = resizeToPoint.Y - m_MouseDownPoint.Y;
		if (m_ResizeDocument == null)
		{
			m_Container.SuspendLayout();
			if ((int)((Control)m_Container).Dock == 3 && num != 0)
			{
				if (m_DocumentDockContainer.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer = m_DocumentDockContainer.Documents[m_DocumentDockContainer.Documents.Count - 1];
					int num3 = documentBaseContainer.DisplayBounds.Width + num;
					if (num3 > documentBaseContainer.MinimumSize.Width)
					{
						documentBaseContainer.SetLayoutBounds(new Rectangle(documentBaseContainer.DisplayBounds.X, documentBaseContainer.DisplayBounds.Y, num3, documentBaseContainer.DisplayBounds.Height));
					}
					else
					{
						ResetLayoutBounds(m_DocumentDockContainer);
					}
				}
				((Control)m_Container).Width = ((Control)m_Container).Width + num;
				if (((Control)m_Container).Width <= GetContainerMinimumSize(m_DocumentDockContainer).Width)
				{
					ResetLayoutBounds(m_DocumentDockContainer);
				}
			}
			else if ((int)((Control)m_Container).Dock == 4 && num != 0)
			{
				if (m_DocumentDockContainer.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer2 = m_DocumentDockContainer.Documents[0];
					int num4 = documentBaseContainer2.DisplayBounds.Width - num;
					if (num4 > documentBaseContainer2.MinimumSize.Width)
					{
						documentBaseContainer2.SetLayoutBounds(new Rectangle(documentBaseContainer2.DisplayBounds.X, documentBaseContainer2.DisplayBounds.Y, num4, documentBaseContainer2.DisplayBounds.Height));
					}
					else
					{
						ResetLayoutBounds(m_DocumentDockContainer);
					}
				}
				((Control)m_Container).Width = ((Control)m_Container).Width - num;
				if (((Control)m_Container).Width <= GetContainerMinimumSize(m_DocumentDockContainer).Width)
				{
					ResetLayoutBounds(m_DocumentDockContainer);
				}
			}
			else if ((int)((Control)m_Container).Dock == 1 && num2 != 0)
			{
				if (m_DocumentDockContainer.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer3 = m_DocumentDockContainer.Documents[m_DocumentDockContainer.Documents.Count - 1];
					int num5 = documentBaseContainer3.DisplayBounds.Height + num2;
					if (num5 > documentBaseContainer3.MinimumSize.Height)
					{
						documentBaseContainer3.SetLayoutBounds(new Rectangle(documentBaseContainer3.DisplayBounds.X, documentBaseContainer3.DisplayBounds.Y, documentBaseContainer3.DisplayBounds.Width, num5));
					}
					else
					{
						ResetLayoutBounds(m_DocumentDockContainer);
					}
				}
				((Control)m_Container).Height = ((Control)m_Container).Height + num2;
				if (((Control)m_Container).Height <= GetContainerMinimumSize(m_DocumentDockContainer).Height)
				{
					ResetLayoutBounds(m_DocumentDockContainer);
				}
			}
			else if ((int)((Control)m_Container).Dock == 2 && num2 != 0)
			{
				if (m_DocumentDockContainer.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer4 = m_DocumentDockContainer.Documents[0];
					int num6 = documentBaseContainer4.DisplayBounds.Height - num2;
					if (num6 > documentBaseContainer4.MinimumSize.Height)
					{
						documentBaseContainer4.SetLayoutBounds(new Rectangle(documentBaseContainer4.DisplayBounds.X, documentBaseContainer4.DisplayBounds.Y, documentBaseContainer4.DisplayBounds.Width, num6));
					}
					else
					{
						ResetLayoutBounds(m_DocumentDockContainer);
					}
				}
				((Control)m_Container).Height = ((Control)m_Container).Height - num2;
				if (((Control)m_Container).Height <= GetContainerMinimumSize(m_DocumentDockContainer).Height)
				{
					ResetLayoutBounds(m_DocumentDockContainer);
				}
			}
			m_Container.ResumeLayout();
		}
		else if (m_ResizeDockContainer.Orientation == eOrientation.Horizontal && num != 0)
		{
			m_ResizeDocument.SetWidth(m_ResizeDocument.DisplayBounds.Width + num);
		}
		else if (m_ResizeDockContainer.Orientation == eOrientation.Vertical && num2 != 0)
		{
			m_ResizeDocument.SetHeight(m_ResizeDocument.DisplayBounds.Height + num2);
		}
		m_Container.RecalcLayout();
		if (m_Container.Owner != null)
		{
			((IOwner)m_Container.Owner).InvokeUserCustomize((object)m_ResizeDocument, EventArgs.Empty);
		}
	}

	private DocumentBaseContainer GetNextDocument(DocumentDockContainer parent, int refIndex)
	{
		int count = parent.Documents.Count;
		for (int i = refIndex + 1; i < count; i++)
		{
			if (parent.Documents[i].Visible)
			{
				return parent.Documents[i];
			}
		}
		return null;
	}

	private DocumentBaseContainer GetPreviousDocument(DocumentDockContainer parent, int refIndex)
	{
		int count = parent.Documents.Count;
		for (int num = refIndex - 1; num >= 0; num--)
		{
			if (parent.Documents[num].Visible)
			{
				return parent.Documents[num];
			}
		}
		return null;
	}

	public void SetBarWidth(Bar bar, int width)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		if (!(GetDocumentFromBar(bar) is DocumentBarContainer documentBarContainer))
		{
			return;
		}
		if (documentBarContainer.Parent == m_DocumentDockContainer && ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4))
		{
			m_Container.SuspendLayout();
			int num = width;
			num = ((documentBarContainer.DisplayBounds.Width != 0) ? (num - documentBarContainer.DisplayBounds.Width) : (num - bar.GetBarDockedSize(eOrientation.Vertical)));
			if (num != 0)
			{
				DockSite container = m_Container;
				((Control)container).Width = ((Control)container).Width + num;
				documentBarContainer.SetWidth(width);
			}
			m_Container.ResumeLayout(performLayout: true);
		}
		else if (documentBarContainer.Parent is DocumentDockContainer && ((DocumentDockContainer)documentBarContainer.Parent).Orientation == eOrientation.Vertical)
		{
			((DocumentDockContainer)documentBarContainer.Parent).SetWidth(width);
		}
		else
		{
			if (width < documentBarContainer.MinimumSize.Width)
			{
				width = documentBarContainer.MinimumSize.Width;
			}
			documentBarContainer.SetWidth(width);
		}
		m_Container.RecalcLayout();
	}

	public void SetBarHeight(Bar bar, int height)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (!(GetDocumentFromBar(bar) is DocumentBarContainer documentBarContainer))
		{
			return;
		}
		if (m_Container != null && ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2) && documentBarContainer.Parent == m_DocumentDockContainer)
		{
			m_Container.SuspendLayout();
			int num = height;
			num = ((documentBarContainer.DisplayBounds.Height != 0) ? (num - documentBarContainer.DisplayBounds.Height) : (num - bar.GetBarDockedSize(eOrientation.Vertical)));
			DockSite container = m_Container;
			((Control)container).Height = ((Control)container).Height + num;
			documentBarContainer.SetHeight(height);
			m_Container.ResumeLayout(performLayout: true);
		}
		else if (documentBarContainer.Parent is DocumentDockContainer && ((DocumentDockContainer)documentBarContainer.Parent).Orientation == eOrientation.Horizontal)
		{
			((DocumentDockContainer)documentBarContainer.Parent).SetHeight(height);
		}
		else
		{
			if (height < documentBarContainer.MinimumSize.Height)
			{
				height = documentBarContainer.MinimumSize.Height;
			}
			documentBarContainer.SetHeight(height);
		}
		m_Container.RecalcLayout();
	}

	internal void SerializeDefinition(XmlElement parent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		XmlElement xmlElement = null;
		if ((int)((Control)m_Container).Dock == 5)
		{
			xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.Documents);
		}
		else
		{
			xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockSite);
			xmlElement.SetAttribute(DocumentSerializationXml.DockSiteSize, XmlConvert.ToString(((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4) ? Math.Ceiling((double)((Control)m_Container).Width / StiScale2.X) : Math.Ceiling((double)((Control)m_Container).Height / StiScale2.Y)));
			xmlElement.SetAttribute(DocumentSerializationXml.DockingSide, Enum.GetName(typeof(DockStyle), ((Control)m_Container).Dock));
		}
		parent.AppendChild(xmlElement);
		SerializeDocumentDockContainer(xmlElement, m_DocumentDockContainer, definitionSerialization: true);
	}

	internal void SerializeLayout(XmlElement parent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		XmlElement xmlElement = null;
		if ((int)((Control)m_Container).Dock == 5)
		{
			xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.Documents);
		}
		else
		{
			xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockSite);
			xmlElement.SetAttribute(DocumentSerializationXml.DockSiteSize, XmlConvert.ToString(((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4) ? Math.Ceiling((double)((Control)m_Container).Width / StiScale2.X) : Math.Ceiling((double)((Control)m_Container).Height / StiScale2.Y)));
			xmlElement.SetAttribute(DocumentSerializationXml.DockingSide, Enum.GetName(typeof(DockStyle), ((Control)m_Container).Dock));
			xmlElement.SetAttribute(DocumentSerializationXml.OriginalDockSiteSize, XmlConvert.ToString(m_Container.OriginalSize));
		}
		parent.AppendChild(xmlElement);
		SerializeDocumentDockContainer(xmlElement, m_DocumentDockContainer, definitionSerialization: false);
	}

	private bool SerializeDocument(DocumentDockContainer instance)
	{
		if (instance.Documents.Count == 0)
		{
			return false;
		}
		foreach (DocumentBaseContainer document in instance.Documents)
		{
			if (document is DocumentBarContainer documentBarContainer)
			{
				if (documentBarContainer.Bar != null && documentBarContainer.Bar.SaveLayoutChanges)
				{
					return true;
				}
				continue;
			}
			return true;
		}
		return false;
	}

	private void SerializeDocumentDockContainer(XmlElement parent, DocumentDockContainer instance, bool definitionSerialization)
	{
		if (!SerializeDocument(instance))
		{
			return;
		}
		XmlElement xmlElement = null;
		xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockContainer);
		parent.AppendChild(xmlElement);
		xmlElement.SetAttribute(DocumentSerializationXml.Orientation, XmlConvert.ToString((int)instance.Orientation));
		xmlElement.SetAttribute(DocumentSerializationXml.Width, Math.Ceiling((double)instance.LayoutBounds.Width / StiScale2.X).ToString());
		xmlElement.SetAttribute(DocumentSerializationXml.Height, Math.Ceiling((double)instance.LayoutBounds.Height / StiScale2.Y).ToString());
		foreach (DocumentBaseContainer document in instance.Documents)
		{
			if (document is DocumentDockContainer)
			{
				SerializeDocumentDockContainer(xmlElement, (DocumentDockContainer)document, definitionSerialization);
			}
			else if (document is DocumentBarContainer)
			{
				SerializeDocumentBarContainer(xmlElement, (DocumentBarContainer)document, definitionSerialization);
			}
		}
	}

	private void SerializeDocumentBarContainer(XmlElement parent, DocumentBarContainer instance, bool definitionSerialization)
	{
		if (instance.Bar != null && instance.Bar.SaveLayoutChanges)
		{
			XmlElement xmlElement = parent.OwnerDocument.CreateElement(DocumentSerializationXml.BarContainer);
			parent.AppendChild(xmlElement);
			xmlElement.SetAttribute(DocumentSerializationXml.Width, Math.Ceiling((double)instance.LayoutBounds.Width / StiScale2.X).ToString());
			xmlElement.SetAttribute(DocumentSerializationXml.Height, Math.Ceiling((double)instance.LayoutBounds.Height / StiScale2.Y).ToString());
			XmlElement xmlElement2 = parent.OwnerDocument.CreateElement(BarSerializationXml.Bar);
			xmlElement.AppendChild(xmlElement2);
			if (definitionSerialization)
			{
				instance.Bar.Serialize(xmlElement2);
			}
			else
			{
				instance.Bar.SerializeLayout(xmlElement2);
			}
		}
	}

	internal void DeserializeLayout(XmlElement parent)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Invalid comparison between Unknown and I4
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Invalid comparison between Unknown and I4
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		if ((parent.Name != DocumentSerializationXml.Documents && parent.Name != DocumentSerializationXml.DockSite) || m_LoadingLayout)
		{
			return;
		}
		m_LoadingLayout = true;
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		try
		{
			foreach (XmlElement childNode in parent.ChildNodes)
			{
				if (childNode.Name == DocumentSerializationXml.DockContainer)
				{
					itemSerializationContext.ItemXmlElement = childNode;
					DeserializeDocumentDockContainer(itemSerializationContext, m_DocumentDockContainer, deserializeDefinition: false);
					break;
				}
			}
		}
		finally
		{
			m_LoadingLayout = false;
		}
		if (parent.Name == DocumentSerializationXml.DockSite)
		{
			if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
			{
				((Control)m_Container).Width = StiScale2.XXI(XmlConvert.ToDouble(parent.GetAttribute(DocumentSerializationXml.DockSiteSize)));
			}
			else if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
			{
				((Control)m_Container).Height = StiScale2.YYI(XmlConvert.ToDouble(parent.GetAttribute(DocumentSerializationXml.DockSiteSize)));
			}
			if (parent.HasAttribute(DocumentSerializationXml.OriginalDockSiteSize))
			{
				m_Container.OriginalSize = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.OriginalDockSiteSize));
			}
		}
		m_Container.RecalcLayout();
	}

	internal void DeserializeDefinition(ItemSerializationContext context)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Invalid comparison between Unknown and I4
		XmlElement itemXmlElement = context.ItemXmlElement;
		if ((itemXmlElement.Name != DocumentSerializationXml.Documents && itemXmlElement.Name != DocumentSerializationXml.DockSite) || m_LoadingLayout)
		{
			return;
		}
		m_LoadingLayout = true;
		try
		{
			foreach (XmlElement childNode in itemXmlElement.ChildNodes)
			{
				if (childNode.Name == DocumentSerializationXml.DockContainer)
				{
					context.ItemXmlElement = childNode;
					DeserializeDocumentDockContainer(context, m_DocumentDockContainer, deserializeDefinition: true);
					break;
				}
			}
			if (itemXmlElement.HasAttribute(DocumentSerializationXml.DockSiteSize))
			{
				if ((int)((Control)m_Container).Dock == 3 || (int)((Control)m_Container).Dock == 4)
				{
					((Control)m_Container).Width = StiScale2.XXI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.DockSiteSize)));
				}
				else if ((int)((Control)m_Container).Dock == 1 || (int)((Control)m_Container).Dock == 2)
				{
					((Control)m_Container).Height = StiScale2.YYI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.DockSiteSize)));
				}
			}
		}
		finally
		{
			m_LoadingLayout = false;
		}
		m_Container.RecalcLayout();
	}

	private void DeserializeDocumentDockContainer(ItemSerializationContext context, DocumentDockContainer instance, bool deserializeDefinition)
	{
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.Name != DocumentSerializationXml.DockContainer)
		{
			return;
		}
		instance.Documents.Clear();
		instance.Orientation = (eOrientation)XmlConvert.ToInt32(itemXmlElement.GetAttribute(DocumentSerializationXml.Orientation));
		if (itemXmlElement.HasAttribute(DocumentSerializationXml.Width) && itemXmlElement.HasAttribute(DocumentSerializationXml.Height))
		{
			instance.SetLayoutBounds(new Rectangle(0, 0, StiScale2.XXI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.Width))), StiScale2.YYI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.Height)))));
		}
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			bool flag = true;
			DocumentBaseContainer documentBaseContainer = DocumentSerializationXml.CreateDocument(childNode.Name);
			context.ItemXmlElement = childNode;
			if (documentBaseContainer is DocumentDockContainer)
			{
				DeserializeDocumentDockContainer(context, (DocumentDockContainer)documentBaseContainer, deserializeDefinition);
			}
			else if (documentBaseContainer is DocumentBarContainer)
			{
				flag = DeserializeDocumentBarContainer(context, (DocumentBarContainer)documentBaseContainer, deserializeDefinition);
			}
			if (flag)
			{
				instance.Documents.Add(documentBaseContainer);
			}
		}
	}

	private bool DeserializeDocumentBarContainer(ItemSerializationContext context, DocumentBarContainer instance, bool deserializeDefinition)
	{
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.Name != DocumentSerializationXml.BarContainer)
		{
			return false;
		}
		instance.SetLayoutBounds(new Rectangle(0, 0, StiScale2.XXI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.Width))), StiScale2.YYI(XmlConvert.ToDouble(itemXmlElement.GetAttribute(DocumentSerializationXml.Height)))));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == BarSerializationXml.Bar))
			{
				continue;
			}
			instance.Bar = m_Container.Owner.Bars[childNode.GetAttribute(BarSerializationXml.Name)];
			if (deserializeDefinition || (instance.Bar == null && childNode.HasAttribute(BarSerializationXml.Custom) && XmlConvert.ToBoolean(childNode.GetAttribute(BarSerializationXml.Custom))))
			{
				instance.Bar = new Bar();
				m_Container.Owner.Bars.Add(instance.Bar);
				if (deserializeDefinition)
				{
					instance.Bar.Deserialize(childNode, context);
				}
				else
				{
					instance.Bar.DeserializeLayout(childNode);
				}
				if (instance.Bar.Items.Count == 0 && !deserializeDefinition)
				{
					m_Container.Owner.Bars.Remove(instance.Bar);
					return false;
				}
				return true;
			}
			if (instance.Bar != null)
			{
				instance.Bar.DeserializeLayout(childNode);
				if (!instance.Bar.IsVisible)
				{
					if (instance.LayoutBounds.Height > 0 && (instance.Bar.DockSide == eDockSide.Top || instance.Bar.DockSide == eDockSide.Bottom))
					{
						((Control)instance.Bar).Height = instance.LayoutBounds.Height;
					}
					else if (instance.LayoutBounds.Width > 0 && (instance.Bar.DockSide == eDockSide.Left || instance.Bar.DockSide == eDockSide.Right))
					{
						((Control)instance.Bar).Width = instance.LayoutBounds.Width;
					}
				}
				break;
			}
			return false;
		}
		return true;
	}
}
