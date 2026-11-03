using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

public class DocumentDockUIManager
{
	private DocumentDockContainer ၝ;

	private Cursor _0ACB;

	private DockSite ၼ;

	private Point ၽ = Point.Empty;

	private bool ၾ;

	private Form ၿ;

	private DocumentDockContainer ႀ;

	private DocumentBaseContainer ႁ;

	private int _1082 = 3;

	private bool _0E3B;

	public DocumentDockContainer RootDocumentDockContainer
	{
		get
		{
			return ၝ;
		}
		set
		{
			ၝ = value;
		}
	}

	public int SplitterWidth
	{
		get
		{
			return _1082;
		}
		set
		{
			_1082 = value;
		}
	}

	public DockSite Container
	{
		get
		{
			return ၼ;
		}
		set
		{
			ၼ = value;
		}
	}

	public bool IsResizingDocument => ႁ != null;

	public DocumentDockUIManager(DockSite container)
	{
		ၼ = container;
	}

	internal Rectangle _1083(Bar ච, ref DockSiteInfo _1084)
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
		DocumentBaseContainer documentFromBar = GetDocumentFromBar(_1084.MouseOverBar);
		Rectangle result = Rectangle.Empty;
		if ((int)((Control)ၼ).Dock != 5 && (_1084.DockLine == 999 || _1084.DockLine == -1))
		{
			return _1085(ච, ref _1084);
		}
		if (documentFromBar != null)
		{
			if (_1084.MouseOverDockSide == eDockSide.Left)
			{
				result = documentFromBar.DisplayBounds;
				result.Width /= 2;
			}
			else if (_1084.MouseOverDockSide == eDockSide.Right)
			{
				result = documentFromBar.DisplayBounds;
				result.X += result.Width / 2;
				result.Width /= 2;
			}
			else if (_1084.MouseOverDockSide == eDockSide.Top)
			{
				result = documentFromBar.DisplayBounds;
				result.Height /= 2;
			}
			else if (_1084.MouseOverDockSide == eDockSide.Bottom)
			{
				result = documentFromBar.DisplayBounds;
				result.Y += result.Height / 2;
				result.Height /= 2;
			}
			else if (_1084.MouseOverDockSide == eDockSide.Document)
			{
				result = documentFromBar.DisplayBounds;
			}
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
		}
		else if ((int)((Control)ၼ).Dock == 5)
		{
			result = ((Control)ၼ).ClientRectangle;
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
		}
		else if ((int)((Control)ၼ).Dock == 4)
		{
			result = ((Control)ၼ).ClientRectangle;
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
			if (result.Width == 0)
			{
				result.Width = ච.GetBarDockedSize(eOrientation.Vertical);
				result.X -= result.Width;
			}
		}
		else if ((int)((Control)ၼ).Dock == 3)
		{
			result = ((Control)ၼ).ClientRectangle;
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
			if (result.Width == 0)
			{
				result.Width = ච.GetBarDockedSize(eOrientation.Vertical);
			}
		}
		else if ((int)((Control)ၼ).Dock == 1)
		{
			result = ((Control)ၼ).ClientRectangle;
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
			if (result.Height == 0)
			{
				result.Height = ච.GetBarDockedSize(eOrientation.Horizontal);
			}
		}
		else if ((int)((Control)ၼ).Dock == 2)
		{
			result = ((Control)ၼ).ClientRectangle;
			result.Location = ((Control)ၼ).PointToScreen(result.Location);
			if (result.Height == 0)
			{
				result.Height = ච.GetBarDockedSize(eOrientation.Horizontal);
				result.Y -= result.Height;
			}
		}
		return result;
	}

	private Rectangle _1085(Bar ච, ref DockSiteInfo _1084)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected I4, but got Unknown
		Rectangle empty = Rectangle.Empty;
		int num = -1;
		int num2 = -1;
		if (_1084.FullSizeDock)
		{
			num = ၼ.ၸ();
		}
		else if (_1084.PartialSizeDock)
		{
			num2 = ၼ.ၷ();
		}
		DockStyle dock = ((Control)ၼ).Dock;
		switch (dock - 1)
		{
		case 0:
		{
			empty.Width = ((Control)ၼ).ClientRectangle.Width;
			empty.Height = ච.GetBarDockedSize(eOrientation.Horizontal);
			if (num >= 0)
			{
				empty.Width += ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: true).Width;
				empty.Width += ၼ._1074(ၼ.Owner.RightDockSite, ၶ: true).Width;
				_1084.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Width -= ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: false).Width;
				empty.Width -= ၼ._1074(ၼ.Owner.RightDockSite, ၶ: false).Width;
				_1084.DockSiteZOrderIndex = num2;
			}
			Point empty4 = Point.Empty;
			if (_1084.DockLine == -1)
			{
				empty4 = ((Control)ၼ).PointToScreen(((Control)ၼ).ClientRectangle.Location);
			}
			else
			{
				empty4 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.X, ((Control)ၼ).ClientRectangle.Bottom));
				empty4.Y++;
			}
			if (num >= 0)
			{
				empty4.X -= ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: true).Width;
			}
			else if (num2 >= 0)
			{
				empty4.X += ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: false).Width;
			}
			empty.Location = empty4;
			break;
		}
		case 1:
		{
			empty.Width = ((Control)ၼ).ClientRectangle.Width;
			empty.Height = ච.GetBarDockedSize(eOrientation.Horizontal);
			if (num >= 0)
			{
				empty.Width += ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: true).Width;
				empty.Width += ၼ._1074(ၼ.Owner.RightDockSite, ၶ: true).Width;
				_1084.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Width -= ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: false).Width;
				empty.Width -= ၼ._1074(ၼ.Owner.RightDockSite, ၶ: false).Width;
				_1084.DockSiteZOrderIndex = num2;
			}
			Point empty3 = Point.Empty;
			if (_1084.DockLine == -1)
			{
				empty3 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.X, ((Control)ၼ).ClientRectangle.Y - empty.Height));
			}
			else
			{
				empty3 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.X, ((Control)ၼ).ClientRectangle.Bottom - empty.Height));
				empty3.Y++;
			}
			if (num >= 0)
			{
				empty3.X -= ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: true).Width;
			}
			else if (num2 >= 0)
			{
				empty3.X += ၼ._1074(ၼ.Owner.LeftDockSite, ၶ: false).Width;
			}
			empty.Location = empty3;
			break;
		}
		case 3:
		{
			empty.Height = ((Control)ၼ).ClientRectangle.Height;
			if (num >= 0)
			{
				empty.Height += ၼ._1074(ၼ.Owner.TopDockSite, ၶ: true).Height;
				empty.Height += ၼ._1074(ၼ.Owner.BottomDockSite, ၶ: true).Height;
				_1084.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Height -= ၼ._1074(ၼ.Owner.TopDockSite, ၶ: false).Height;
				empty.Height -= ၼ._1074(ၼ.Owner.BottomDockSite, ၶ: false).Height;
				_1084.DockSiteZOrderIndex = num2;
			}
			empty.Width = ච.GetBarDockedSize(eOrientation.Vertical);
			Point empty5 = Point.Empty;
			if (_1084.DockLine == -1)
			{
				empty5 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.X - empty.Width, ((Control)ၼ).ClientRectangle.Y));
			}
			else
			{
				empty5 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.Right - empty.Width, ((Control)ၼ).ClientRectangle.Y));
				empty5.X--;
			}
			if (num >= 0)
			{
				empty5.Y -= ၼ._1074(ၼ.Owner.TopDockSite, ၶ: true).Height;
			}
			else if (num2 >= 0)
			{
				empty5.Y += ၼ._1074(ၼ.Owner.TopDockSite, ၶ: false).Height;
			}
			empty.Location = empty5;
			break;
		}
		default:
		{
			empty.Height = ((Control)ၼ).ClientRectangle.Height;
			if (num >= 0)
			{
				empty.Height += ၼ._1074(ၼ.Owner.TopDockSite, ၶ: true).Height;
				empty.Height += ၼ._1074(ၼ.Owner.BottomDockSite, ၶ: true).Height;
				_1084.DockSiteZOrderIndex = num;
			}
			else if (num2 >= 0)
			{
				empty.Height -= ၼ._1074(ၼ.Owner.TopDockSite, ၶ: false).Height;
				empty.Height -= ၼ._1074(ၼ.Owner.BottomDockSite, ၶ: false).Height;
				_1084.DockSiteZOrderIndex = num2;
			}
			empty.Width = ච.GetBarDockedSize(eOrientation.Vertical);
			Point empty2 = Point.Empty;
			if (_1084.DockLine == -1)
			{
				empty2 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.X, ((Control)ၼ).ClientRectangle.Y));
			}
			else
			{
				empty2 = ((Control)ၼ).PointToScreen(new Point(((Control)ၼ).ClientRectangle.Right, ((Control)ၼ).ClientRectangle.Y));
				empty2.X++;
			}
			if (num >= 0)
			{
				empty2.Y -= ၼ._1074(ၼ.Owner.TopDockSite, ၶ: true).Height;
			}
			else if (num2 >= 0)
			{
				empty2.Y += ၼ._1074(ၼ.Owner.TopDockSite, ၶ: false).Height;
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
		return _1086(bar, ၝ);
	}

	private DocumentBaseContainer _1086(Bar ච, DocumentDockContainer _1087)
	{
		foreach (DocumentBaseContainer document in _1087.Documents)
		{
			if (document is DocumentBarContainer && ((DocumentBarContainer)document).Bar == ච)
			{
				return document;
			}
			if (document is DocumentDockContainer)
			{
				DocumentBaseContainer documentBaseContainer2 = _1086(ච, document as DocumentDockContainer);
				if (documentBaseContainer2 != null)
				{
					return documentBaseContainer2;
				}
			}
		}
		return null;
	}

	private bool _1088(eDockSide _1089)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		if (((int)((Control)ၼ).Dock == 3 && _1089 == eDockSide.Left) || ((int)((Control)ၼ).Dock == 4 && _1089 == eDockSide.Left) || ((int)((Control)ၼ).Dock == 1 && _1089 == eDockSide.Top) || ((int)((Control)ၼ).Dock == 2 && _1089 == eDockSide.Top))
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
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Invalid comparison between Unknown and I4
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Invalid comparison between Unknown and I4
		if (dockSide == eDockSide.None && ((Control)barToDock).Parent == ၼ)
		{
			_႒(barToDock);
			return;
		}
		if (((Control)barToDock).Parent is DockSite && ((DockSite)(object)((Control)barToDock).Parent).DocumentDockContainer != null)
		{
			((DockSite)(object)((Control)barToDock).Parent).GetDocumentUIManager().UnDock(barToDock);
		}
		else if (((Control)barToDock).Parent is DockSite && ((Control)barToDock).Parent != ၼ)
		{
			((DockSite)(object)((Control)barToDock).Parent).ၹ((Control)(object)barToDock);
		}
		else if (((Control)barToDock).Parent != null && ((Control)barToDock).Parent != ၼ)
		{
			if (((Control)barToDock).Parent is ᆡ)
			{
				barToDock._0BF7();
				barToDock.మ(eBarState.Docked);
			}
			else
			{
				((Control)barToDock).Parent.Controls.Remove((Control)(object)barToDock);
			}
		}
		if (!_0E3B)
		{
			DocumentBarContainer documentBarContainer = _႔(barToDock);
			DocumentBaseContainer documentFromBar = GetDocumentFromBar(referenceBar);
			if (referenceBar == null || dockSide == eDockSide.None || referenceBar == barToDock || documentFromBar == null)
			{
				if ((int)((Control)ၼ).Dock == 5)
				{
					ၝ.Documents.Add(documentBarContainer);
				}
				else
				{
					eOrientation eOrientation2 = eOrientation.Horizontal;
					if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
					{
						eOrientation2 = eOrientation.Vertical;
					}
					if (ၝ.Orientation != eOrientation2)
					{
						if (ၝ.Documents.Count <= 1)
						{
							ၝ.Orientation = eOrientation2;
							if (_1088(dockSide))
							{
								ၝ.Documents.Insert(0, documentBarContainer);
							}
							else
							{
								ၝ.Documents.Add(documentBarContainer);
							}
						}
						else
						{
							DocumentBaseContainer[] array = new DocumentBaseContainer[ၝ.Documents.Count];
							ၝ.Documents._0CD0(array);
							ၝ.Documents.Clear();
							DocumentDockContainer documentDockContainer = new DocumentDockContainer(array, ၝ.Orientation);
							documentDockContainer._10C9(ၝ.DisplayBounds);
							ၝ.Orientation = eOrientation2;
							ၝ.Documents.Add(documentDockContainer);
							if (_1088(dockSide))
							{
								ၝ.Documents.Insert(0, documentBarContainer);
							}
							else
							{
								ၝ.Documents.Add(documentBarContainer);
							}
						}
					}
					else if (_1088(dockSide))
					{
						ၝ.Documents.Insert(0, documentBarContainer);
					}
					else
					{
						ၝ.Documents.Add(documentBarContainer);
					}
				}
			}
			else
			{
				DocumentDockContainer documentDockContainer2 = documentFromBar.Parent as DocumentDockContainer;
				documentFromBar._10C9(Rectangle.Empty);
				documentBarContainer._10C9(Rectangle.Empty);
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
			_108A(barToDock, _108C: false);
		}
		if (ၼ != null)
		{
			if (((Control)barToDock).Parent == null)
			{
				((Control)ၼ).Controls.Add((Control)(object)barToDock);
			}
			_႒(barToDock);
			ၼ.RecalcLayout();
		}
		else
		{
			_႒(barToDock);
		}
	}

	internal void _108A(Bar _108B, bool _108C)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Invalid comparison between Unknown and I4
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Invalid comparison between Unknown and I4
		if ((ၼ.Owner != null && ၼ.Owner.IsLoadingLayout) || (int)((Control)ၼ).Dock == 5)
		{
			return;
		}
		DocumentBaseContainer documentFromBar = GetDocumentFromBar(_108B);
		if (documentFromBar == null)
		{
			return;
		}
		if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
		{
			if (((Control)ၼ).Width != 0 && (documentFromBar.Parent != ၝ || (ၝ.Orientation != 0 && (!_108C || ၝ.Documents.Count != 1))))
			{
				return;
			}
			int num = _108B.GetBarDockedSize(eOrientation.Vertical);
			if (((Control)_108B).Width > 0 && ((Control)_108B).Width < ((Control)ၼ).Width)
			{
				num = ((Control)_108B).Width;
			}
			int num2 = _106A();
			if (_108B.Visible)
			{
				if (num > num2)
				{
					num = _108B.MinimumDockSize(eOrientation.Vertical).Width * 2;
				}
				if (num > num2)
				{
					num = _108B.MinimumDockSize(eOrientation.Vertical).Width;
				}
			}
			if (documentFromBar.LayoutBounds.Width == 0)
			{
				documentFromBar._10C9(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, num, documentFromBar.LayoutBounds.Height));
			}
			num += Dpi.Width(ၝ.SplitterSize);
			if (_108C)
			{
				if (_108B.Visible)
				{
					DockSite dockSite = ၼ;
					((Control)dockSite).Width = ((Control)dockSite).Width + num;
				}
				else if (((Control)ၼ).Width > 0)
				{
					DockSite dockSite2 = ၼ;
					((Control)dockSite2).Width = ((Control)dockSite2).Width - Math.Min(num, ((Control)ၼ).Width);
				}
			}
			else
			{
				DockSite dockSite3 = ၼ;
				((Control)dockSite3).Width = ((Control)dockSite3).Width + num;
			}
			((Control)ၼ).Invalidate();
		}
		else
		{
			if (((int)((Control)ၼ).Dock != 1 && (int)((Control)ၼ).Dock != 2) || (((Control)ၼ).Height != 0 && (documentFromBar.Parent != ၝ || (ၝ.Orientation != eOrientation.Vertical && (!_108C || ၝ.Documents.Count != 1)))))
			{
				return;
			}
			int num3 = _108B.GetBarDockedSize(eOrientation.Horizontal);
			if (((Control)_108B).Height > 0 && ((Control)_108B).Height < ((Control)ၼ).Height)
			{
				num3 = ((Control)_108B).Height;
			}
			int num4 = _106B();
			if (_108B.Visible)
			{
				if (num3 > num4)
				{
					num3 = _108B.MinimumDockSize(eOrientation.Horizontal).Height * 2;
				}
				if (num3 > num4)
				{
					num3 = _108B.MinimumDockSize(eOrientation.Horizontal).Height;
				}
			}
			if (documentFromBar.LayoutBounds.Height == 0)
			{
				documentFromBar._10C9(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentFromBar.LayoutBounds.Width, num3));
			}
			num3 += Dpi.Width(ၝ.SplitterSize);
			if (_108C)
			{
				if (_108B.Visible)
				{
					DockSite dockSite4 = ၼ;
					((Control)dockSite4).Height = ((Control)dockSite4).Height + num3;
				}
				else if (((Control)ၼ).Height > 0)
				{
					DockSite dockSite5 = ၼ;
					((Control)dockSite5).Height = ((Control)dockSite5).Height - Math.Min(num3, ((Control)ၼ).Height);
				}
			}
			else
			{
				DockSite dockSite6 = ၼ;
				((Control)dockSite6).Height = ((Control)dockSite6).Height + num3;
			}
			((Control)ၼ).Invalidate();
		}
	}

	private int _106A()
	{
		if (ၼ == null)
		{
			return 0;
		}
		return ၼ._106A();
	}

	private int _106B()
	{
		if (ၼ == null)
		{
			return 0;
		}
		return ၼ._106B();
	}

	public void UnDock(Bar barToUnDock)
	{
		UnDock(barToUnDock, removeFromContainer: true);
	}

	internal void _108D(Bar ච, DocumentBarContainer ႎ, DockSiteInfo _108F)
	{
		if (ච._୬ == null || !(ႎ.Parent is DocumentDockContainer))
		{
			return;
		}
		DocumentDockContainer documentDockContainer = (DocumentDockContainer)ႎ.Parent;
		DocumentBaseContainer documentBaseContainer = Ⴀ(documentDockContainer, documentDockContainer.Documents.IndexOf(ႎ));
		if (documentBaseContainer == null)
		{
			DocumentBaseContainer documentBaseContainer2 = Ⴂ(documentDockContainer, documentDockContainer.Documents.IndexOf(ႎ));
			if (documentBaseContainer2 != null)
			{
				Bar bar = null;
				if (documentBaseContainer2 is DocumentBarContainer)
				{
					bar = ((DocumentBarContainer)documentBaseContainer2).Bar;
					ච._୬.LastRelativeDockToBar = bar;
					bar._୬.LastRelativeDockToBar = ච;
					bar._୬.LastRelativeDocumentId = ႎ.Id;
				}
				ච._୬.LastRelativeDocumentId = documentBaseContainer2.Id;
				if (documentDockContainer.Orientation == eOrientation.Horizontal)
				{
					ච.ଙ = eDockSide.Right;
					if (bar != null)
					{
						bar.ଙ = eDockSide.Left;
					}
				}
				else
				{
					ච.ଙ = eDockSide.Bottom;
					if (bar != null)
					{
						bar.ଙ = eDockSide.Top;
					}
				}
			}
		}
		else
		{
			Bar bar2 = null;
			if (documentBaseContainer is DocumentBarContainer)
			{
				bar2 = ((DocumentBarContainer)documentBaseContainer).Bar;
				ච._୬.LastRelativeDockToBar = bar2;
				bar2._୬.LastRelativeDockToBar = ච;
				bar2._୬.LastRelativeDocumentId = ႎ.Id;
			}
			ච._୬.LastRelativeDocumentId = documentBaseContainer.Id;
			if (documentDockContainer.Orientation == eOrientation.Horizontal)
			{
				ච.ଙ = eDockSide.Left;
				if (bar2 != null)
				{
					bar2.ଙ = eDockSide.Right;
				}
			}
			else
			{
				ච.ଙ = eDockSide.Top;
				if (bar2 != null)
				{
					bar2.ଙ = eDockSide.Bottom;
				}
			}
		}
		if (_႐(ච) != 0)
		{
			ච._୬.LastDockSiteSide = _႐(ච);
		}
	}

	private eDockSide _႐(Bar ච)
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
		if (((Control)ච).Parent == null)
		{
			return eDockSide.None;
		}
		if ((int)((Control)ච).Parent.Dock == 3)
		{
			return eDockSide.Left;
		}
		if ((int)((Control)ච).Parent.Dock == 4)
		{
			return eDockSide.Right;
		}
		if ((int)((Control)ච).Parent.Dock == 1)
		{
			return eDockSide.Top;
		}
		if ((int)((Control)ච).Parent.Dock == 2)
		{
			return eDockSide.Bottom;
		}
		if ((int)((Control)ච).Parent.Dock == 5)
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
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Invalid comparison between Unknown and I4
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Invalid comparison between Unknown and I4
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Invalid comparison between Unknown and I4
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Invalid comparison between Unknown and I4
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Invalid comparison between Unknown and I4
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Invalid comparison between Unknown and I4
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Invalid comparison between Unknown and I4
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Invalid comparison between Unknown and I4
		DocumentBarContainer barDocumentContainer = ၝ.GetBarDocumentContainer(barToUnDock);
		if (barToUnDock.DockSide != 0)
		{
			barToUnDock.ଙ = barToUnDock.DockSide;
		}
		if (barDocumentContainer != null)
		{
			_108D(barToUnDock, barDocumentContainer, barToUnDock._୬);
		}
		int num = 0;
		if (barDocumentContainer != null && ၼ != null && (int)((Control)ၼ).Dock != 5)
		{
			if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
			{
				if (barDocumentContainer.Parent != null && barDocumentContainer.Parent == ၝ && ၝ.Orientation == eOrientation.Horizontal)
				{
					num = ((Control)barToUnDock).Width + Dpi.Width(ၝ.SplitterSize);
				}
			}
			else if (((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2) && barDocumentContainer.Parent != null && barDocumentContainer.Parent == ၝ && ၝ.Orientation == eOrientation.Vertical)
			{
				num = ((Control)barToUnDock).Height + Dpi.Width(ၝ.SplitterSize);
			}
		}
		if (barDocumentContainer == null)
		{
			if (removeFromContainer && ((Control)barToUnDock).Parent == ၼ)
			{
				((Control)ၼ).Controls.Remove((Control)(object)barToUnDock);
			}
			_႓(barToUnDock);
			return;
		}
		if (barDocumentContainer.Parent is DocumentDockContainer documentDockContainer)
		{
			documentDockContainer.Documents.Remove(barDocumentContainer);
			if (documentDockContainer != ၝ)
			{
				_႑(documentDockContainer);
			}
			else if (documentDockContainer.Documents.Count == 1 && documentDockContainer.Documents[0] is DocumentDockContainer)
			{
				_႑(documentDockContainer.Documents[0] as DocumentDockContainer);
			}
		}
		if (ၼ != null)
		{
			if (removeFromContainer)
			{
				((Control)ၼ).Controls.Remove((Control)(object)barToUnDock);
			}
			_႓(barToUnDock);
			bool flag = false;
			if (ၼ.Owner != null)
			{
				flag = ၼ.Owner.IsLoadingLayout;
			}
			if (!flag && !ၼ.ၐ)
			{
				if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
				{
					((Control)ၼ).Width = 0;
				}
				else if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
				{
					((Control)ၼ).Height = 0;
				}
			}
			else if (!flag && num != 0)
			{
				if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
				{
					DockSite dockSite = ၼ;
					((Control)dockSite).Width = ((Control)dockSite).Width - num;
				}
				else if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
				{
					DockSite dockSite2 = ၼ;
					((Control)dockSite2).Height = ((Control)dockSite2).Height - num;
				}
			}
			else
			{
				ၼ.RecalcLayout();
			}
		}
		else
		{
			_႓(barToUnDock);
		}
	}

	private void _႑(DocumentDockContainer ԟ)
	{
		while ((ԟ.Documents.Count == 0 && ԟ.Parent != null && ԟ.Parent is DocumentDockContainer) || (ԟ.Parent != null && ԟ.Parent is DocumentDockContainer && (((DocumentDockContainer)ԟ.Parent).Documents.Count == 1 || ԟ.Documents.Count == 1)))
		{
			if (ԟ.Parent != null && ԟ.Parent is DocumentDockContainer && ((DocumentDockContainer)ԟ.Parent).Documents.Count == 1)
			{
				DocumentDockContainer documentDockContainer = ԟ.Parent as DocumentDockContainer;
				documentDockContainer.Orientation = ԟ.Orientation;
				documentDockContainer.Documents.Remove(ԟ);
				DocumentBaseContainer[] array = new DocumentBaseContainer[ԟ.Documents.Count];
				ԟ.Documents._0CD0(array);
				documentDockContainer.Documents.AddRange(array);
				ԟ = documentDockContainer;
			}
			else if (ԟ.Parent != null && ԟ.Parent is DocumentDockContainer && ԟ.Documents.Count == 1)
			{
				DocumentDockContainer documentDockContainer2 = ԟ.Parent as DocumentDockContainer;
				int index = documentDockContainer2.Documents.IndexOf(ԟ);
				documentDockContainer2.Documents.Remove(ԟ);
				DocumentBaseContainer value = ԟ.Documents[0];
				documentDockContainer2.Documents.Insert(index, value);
				ԟ = documentDockContainer2;
			}
			else
			{
				DocumentDockContainer documentDockContainer3 = ԟ.Parent as DocumentDockContainer;
				documentDockContainer3.Documents.Remove(ԟ);
				ԟ = documentDockContainer3;
			}
		}
	}

	private void _႒(Bar ච)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (ၼ != null && ၼ.Owner != null && ၼ.Owner.ApplyDocumentBarStyle && (int)((Control)ၼ).Dock == 5)
		{
			BarFunctions.ApplyAutoDocumentBarStyle(ච);
		}
	}

	private void _႓(Bar ච)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (ၼ != null && ၼ.Owner != null && ၼ.Owner.ApplyDocumentBarStyle && (int)((Control)ၼ).Dock == 5)
		{
			BarFunctions.RestoreAutoDocumentBarStyle(ච);
		}
	}

	private DocumentBarContainer _႔(Bar ච)
	{
		return new DocumentBarContainer(ච);
	}

	private DocumentDockContainer _႕()
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
				if (_0ACB != (Cursor)null)
				{
					((Control)Container).Cursor = _0ACB;
				}
				_0ACB = null;
			}
			else
			{
				if (_0ACB == (Cursor)null)
				{
					_0ACB = ((Control)Container).Cursor;
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
			Point point = _109D(x, y);
			if (!point.IsEmpty)
			{
				if (!ၾ)
				{
					_109A(point.X, point.Y);
					return;
				}
				_109F(point.X, point.Y);
				ၽ = point;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Cursor GetCursor(int x, int y)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		Cursor result = null;
		DocumentDockContainer documentDockContainer = _႗(x, y);
		if (documentDockContainer != null)
		{
			if (documentDockContainer.Orientation == eOrientation.Horizontal)
			{
				result = Cursors.VSplit;
			}
			else if (documentDockContainer.Orientation == eOrientation.Vertical)
			{
				result = Cursors.HSplit;
			}
		}
		else if (ၼ != null && (int)((Control)ၼ).Dock != 5)
		{
			if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
			{
				result = Cursors.HSplit;
			}
			else if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
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
		ၽ = new Point(x, y);
		DocumentDockContainer documentDockContainer = _႗(x, y);
		if (documentDockContainer != null)
		{
			ႁ = _႖(documentDockContainer, x, y);
			if (ႁ != null)
			{
				ႀ = documentDockContainer;
				if (!ၾ)
				{
					_႙();
					_109A(x, y);
				}
			}
		}
		else if (ၼ != null && (int)((Control)ၼ).Dock != 5)
		{
			ႀ = ၝ;
			if (!ၾ)
			{
				_႙();
				_109A(x, y);
			}
		}
	}

	public void OnMouseUp(MouseEventArgs e)
	{
		if (ႀ != null && !ၽ.IsEmpty)
		{
			_109F(e.X, e.Y);
		}
		ၽ = Point.Empty;
		ႀ = null;
		ႁ = null;
		_109B();
	}

	public void OnMouseLeave()
	{
		if (_0ACB != (Cursor)null)
		{
			((Control)Container).Cursor = _0ACB;
		}
		_0ACB = null;
	}

	private DocumentBaseContainer _႖(DocumentDockContainer ԟ, int و, int ى)
	{
		DocumentBaseContainer result = null;
		eOrientation orientation = ԟ.Orientation;
		foreach (DocumentBaseContainer document in ԟ.Documents)
		{
			if ((orientation != 0 || document.DisplayBounds.X <= و || !document.Visible) && (orientation != eOrientation.Vertical || document.DisplayBounds.Y <= ى || !document.Visible))
			{
				if (document.Visible)
				{
					result = document;
				}
				continue;
			}
			break;
		}
		return result;
	}

	private DocumentDockContainer _႗(int و, int ى)
	{
		return _႘(ၝ, و, ى);
	}

	private DocumentDockContainer _႘(DocumentDockContainer ԟ, int و, int ى)
	{
		if (ԟ == null || !ԟ.DisplayBounds.Contains(و, ى) || !ԟ.Visible)
		{
			return null;
		}
		DocumentDockContainer documentDockContainer = null;
		foreach (DocumentBaseContainer document in ԟ.Documents)
		{
			if (document is DocumentDockContainer)
			{
				documentDockContainer = _႘((DocumentDockContainer)document, و, ى);
				if (documentDockContainer != null)
				{
					break;
				}
			}
		}
		if (documentDockContainer == null)
		{
			documentDockContainer = ԟ;
		}
		return documentDockContainer;
	}

	private void _႙()
	{
		if (ႀ != null && ၿ == null)
		{
			ၿ = BarFunctions.CreateOutlineForm();
		}
	}

	private void _109A(int و, int ى)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Invalid comparison between Unknown and I4
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Invalid comparison between Unknown and I4
		if (ၿ != null && ႀ != null)
		{
			Rectangle empty = Rectangle.Empty;
			int num = Dpi.Width(_1082);
			if ((ႀ.Orientation == eOrientation.Horizontal && ႁ != null) || (ႁ == null && ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)))
			{
				Point point = ((Control)ၼ).PointToScreen(new Point(و - num / 2, ႀ.DisplayBounds.Y));
				empty.X = point.X;
				empty.Y = point.Y;
				empty.Height = ႀ.DisplayBounds.Height;
				empty.Width = num;
			}
			else if ((ႀ.Orientation == eOrientation.Vertical && ႁ != null) || (ႁ == null && ((int)((Control)ၼ).Dock == 2 || (int)((Control)ၼ).Dock == 1)))
			{
				Point point2 = ((Control)ၼ).PointToScreen(new Point(ႀ.DisplayBounds.X, ى - num / 2));
				empty.X = point2.X;
				empty.Y = point2.Y;
				empty.Width = ႀ.DisplayBounds.Width;
				empty.Height = num;
			}
			_089F.SetWindowPos(((Control)ၿ).Handle, new IntPtr(0), empty.X, empty.Y, empty.Width, empty.Height, 80);
		}
	}

	private void _109B()
	{
		if (ၿ != null)
		{
			((Control)ၿ).Visible = false;
			((Component)(object)ၿ).Dispose();
			ၿ = null;
		}
	}

	internal Size _109C(DocumentDockContainer _1087)
	{
		Size empty = Size.Empty;
		foreach (DocumentBaseContainer document in _1087.Documents)
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
				size = _109C(document as DocumentDockContainer);
			}
			if (_1087.Orientation == eOrientation.Horizontal)
			{
				empty.Width += size.Width + Dpi.Width(_1087.SplitterSize);
				if (size.Height < empty.Height || empty.Height == 0)
				{
					empty.Height = size.Height;
				}
			}
			else
			{
				empty.Height += size.Height + Dpi.Width(_1087.SplitterSize);
				if (size.Width < empty.Width || empty.Width == 0)
				{
					empty.Width = size.Width;
				}
			}
		}
		if (empty.Width > 0 && _1087.Orientation == eOrientation.Horizontal)
		{
			empty.Width += Dpi.Width(_1087.SplitterSize);
		}
		else if (empty.Height > 0 && _1087.Orientation == eOrientation.Vertical)
		{
			empty.Height += Dpi.Width(_1087.SplitterSize);
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

	private Point _109D(int و, int ى)
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
		if (ႀ == null)
		{
			return Point.Empty;
		}
		Point result = new Point(و, ى);
		int num = و - ၽ.X;
		int num2 = ى - ၽ.Y;
		if (ႁ == null)
		{
			if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
			{
				if ((int)((Control)ၼ).Dock == 4)
				{
					num *= -1;
				}
				Size size = _109C(ႀ);
				if (ႀ.DisplayBounds.Width + num < size.Width)
				{
					result.X -= (ႀ.DisplayBounds.Width + num - size.Width) * (((int)((Control)ၼ).Dock != 4) ? 1 : (-1));
				}
				else
				{
					int num3 = _106A();
					int num4 = 32;
					if (ၼ.Owner != null)
					{
						num4 = ၼ.Owner.MinimumClientSize.Width;
					}
					if (num3 - num < num4)
					{
						result.X -= (num4 - (num3 - num)) * (((int)((Control)ၼ).Dock != 4) ? 1 : (-1));
					}
				}
				return result;
			}
			if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
			{
				if ((int)((Control)ၼ).Dock == 2)
				{
					num2 *= -1;
				}
				Size size2 = _109C(ႀ);
				if (ႀ.DisplayBounds.Height + num2 < size2.Height)
				{
					result.Y -= (ႀ.DisplayBounds.Height + num2 - size2.Height) * (((int)((Control)ၼ).Dock != 2) ? 1 : (-1));
				}
				else
				{
					int num5 = _106B();
					int num6 = 32;
					if (ၼ.Owner != null)
					{
						num6 = ၼ.Owner.MinimumClientSize.Height;
					}
					if (num5 - num2 < num6)
					{
						result.Y -= (num6 - (num5 - num2)) * (((int)((Control)ၼ).Dock != 2) ? 1 : (-1));
					}
				}
				return result;
			}
			return Point.Empty;
		}
		if (ႀ.Orientation == eOrientation.Horizontal)
		{
			if (ႁ.DisplayBounds.Width + num >= ႁ.MinimumSize.Width)
			{
				DocumentBaseContainer documentBaseContainer = Ⴀ(ႀ, ႀ.Documents.IndexOf(ႁ));
				if (documentBaseContainer != null && documentBaseContainer.DisplayBounds.Width - num < documentBaseContainer.MinimumSize.Width)
				{
					result.X += documentBaseContainer.DisplayBounds.Width - num - documentBaseContainer.MinimumSize.Width;
				}
			}
			else
			{
				result.X -= ႁ.DisplayBounds.Width + num - ႁ.MinimumSize.Width;
			}
		}
		else if (ႀ.Orientation == eOrientation.Vertical)
		{
			if (ႁ.DisplayBounds.Height + num2 >= ႁ.MinimumSize.Height)
			{
				DocumentBaseContainer documentBaseContainer2 = Ⴀ(ႀ, ႀ.Documents.IndexOf(ႁ));
				if (documentBaseContainer2 != null && documentBaseContainer2.DisplayBounds.Height - num2 < documentBaseContainer2.MinimumSize.Height)
				{
					result.Y += documentBaseContainer2.DisplayBounds.Height - num2 - documentBaseContainer2.MinimumSize.Height;
				}
			}
			else
			{
				result.Y -= ႁ.DisplayBounds.Height + num2 - ႁ.MinimumSize.Height;
			}
		}
		return result;
	}

	private void _109E(DocumentDockContainer ႎ)
	{
		foreach (DocumentBaseContainer document in ႎ.Documents)
		{
			document._10C9(Rectangle.Empty);
		}
	}

	private void _109F(int و, int ى)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Invalid comparison between Unknown and I4
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Invalid comparison between Unknown and I4
		if (ႀ == null)
		{
			return;
		}
		Point point = _109D(و, ى);
		int num = point.X - ၽ.X;
		int num2 = point.Y - ၽ.Y;
		if (ႁ == null)
		{
			ၼ.SuspendLayout();
			if ((int)((Control)ၼ).Dock == 3 && num != 0)
			{
				if (ၝ.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer = ၝ.Documents[ၝ.Documents.Count - 1];
					int num3 = documentBaseContainer.DisplayBounds.Width + num;
					if (num3 > documentBaseContainer.MinimumSize.Width)
					{
						documentBaseContainer._10C9(new Rectangle(documentBaseContainer.DisplayBounds.X, documentBaseContainer.DisplayBounds.Y, num3, documentBaseContainer.DisplayBounds.Height));
					}
					else
					{
						_109E(ၝ);
					}
				}
				((Control)ၼ).Width = ((Control)ၼ).Width + num;
				if (((Control)ၼ).Width <= _109C(ၝ).Width)
				{
					_109E(ၝ);
				}
			}
			else if ((int)((Control)ၼ).Dock == 4 && num != 0)
			{
				if (ၝ.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer2 = ၝ.Documents[0];
					int num4 = documentBaseContainer2.DisplayBounds.Width - num;
					if (num4 > documentBaseContainer2.MinimumSize.Width)
					{
						documentBaseContainer2._10C9(new Rectangle(documentBaseContainer2.DisplayBounds.X, documentBaseContainer2.DisplayBounds.Y, num4, documentBaseContainer2.DisplayBounds.Height));
					}
					else
					{
						_109E(ၝ);
					}
				}
				((Control)ၼ).Width = ((Control)ၼ).Width - num;
				if (((Control)ၼ).Width <= _109C(ၝ).Width)
				{
					_109E(ၝ);
				}
			}
			else if ((int)((Control)ၼ).Dock == 1 && num2 != 0)
			{
				if (ၝ.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer3 = ၝ.Documents[ၝ.Documents.Count - 1];
					int num5 = documentBaseContainer3.DisplayBounds.Height + num2;
					if (num5 > documentBaseContainer3.MinimumSize.Height)
					{
						documentBaseContainer3._10C9(new Rectangle(documentBaseContainer3.DisplayBounds.X, documentBaseContainer3.DisplayBounds.Y, documentBaseContainer3.DisplayBounds.Width, num5));
					}
					else
					{
						_109E(ၝ);
					}
				}
				((Control)ၼ).Height = ((Control)ၼ).Height + num2;
				if (((Control)ၼ).Height <= _109C(ၝ).Height)
				{
					_109E(ၝ);
				}
			}
			else if ((int)((Control)ၼ).Dock == 2 && num2 != 0)
			{
				if (ၝ.Documents.Count > 0)
				{
					DocumentBaseContainer documentBaseContainer4 = ၝ.Documents[0];
					int num6 = documentBaseContainer4.DisplayBounds.Height - num2;
					if (num6 > documentBaseContainer4.MinimumSize.Height)
					{
						documentBaseContainer4._10C9(new Rectangle(documentBaseContainer4.DisplayBounds.X, documentBaseContainer4.DisplayBounds.Y, documentBaseContainer4.DisplayBounds.Width, num6));
					}
					else
					{
						_109E(ၝ);
					}
				}
				((Control)ၼ).Height = ((Control)ၼ).Height - num2;
				if (((Control)ၼ).Height <= _109C(ၝ).Height)
				{
					_109E(ၝ);
				}
			}
			ၼ.ResumeLayout();
		}
		else if (ႀ.Orientation == eOrientation.Horizontal && num != 0)
		{
			ႁ.SetWidth(ႁ.DisplayBounds.Width + num);
		}
		else if (ႀ.Orientation == eOrientation.Vertical && num2 != 0)
		{
			ႁ.SetHeight(ႁ.DisplayBounds.Height + num2);
		}
		ၼ.RecalcLayout();
		if (ၼ.Owner != null)
		{
			((IOwner)ၼ.Owner).InvokeUserCustomize((object)ႁ, EventArgs.Empty);
		}
	}

	private DocumentBaseContainer Ⴀ(DocumentDockContainer ԟ, int Ⴁ)
	{
		int count = ԟ.Documents.Count;
		for (int i = Ⴁ + 1; i < count; i++)
		{
			if (ԟ.Documents[i].Visible)
			{
				return ԟ.Documents[i];
			}
		}
		return null;
	}

	private DocumentBaseContainer Ⴂ(DocumentDockContainer ԟ, int Ⴁ)
	{
		_ = ԟ.Documents.Count;
		for (int num = Ⴁ - 1; num >= 0; num--)
		{
			if (ԟ.Documents[num].Visible)
			{
				return ԟ.Documents[num];
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
		if (documentBarContainer.Parent == ၝ && ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4))
		{
			ၼ.SuspendLayout();
			int num = width;
			num = ((documentBarContainer.DisplayBounds.Width != 0) ? (num - documentBarContainer.DisplayBounds.Width) : (num - bar.GetBarDockedSize(eOrientation.Vertical)));
			if (num != 0)
			{
				DockSite dockSite = ၼ;
				((Control)dockSite).Width = ((Control)dockSite).Width + num;
				documentBarContainer.SetWidth(width);
			}
			ၼ.ResumeLayout(performLayout: true);
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
		ၼ.RecalcLayout();
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
		if (ၼ != null && ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2) && documentBarContainer.Parent == ၝ)
		{
			ၼ.SuspendLayout();
			int num = height;
			num = ((documentBarContainer.DisplayBounds.Height != 0) ? (num - documentBarContainer.DisplayBounds.Height) : (num - bar.GetBarDockedSize(eOrientation.Vertical)));
			DockSite dockSite = ၼ;
			((Control)dockSite).Height = ((Control)dockSite).Height + num;
			documentBarContainer.SetHeight(height);
			ၼ.ResumeLayout(performLayout: true);
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
		ၼ.RecalcLayout();
	}

	internal void Ⴃ(XmlElement ԟ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		XmlElement xmlElement = null;
		if ((int)((Control)ၼ).Dock == 5)
		{
			xmlElement = ԟ.OwnerDocument.CreateElement(_290D._290E);
		}
		else
		{
			xmlElement = ԟ.OwnerDocument.CreateElement(_290D._2911);
			xmlElement.SetAttribute(_290D._2913, XmlConvert.ToString(((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4) ? ((Control)ၼ).Width : ((Control)ၼ).Height));
			xmlElement.SetAttribute(_290D._2912, Enum.GetName(typeof(DockStyle), ((Control)ၼ).Dock));
		}
		ԟ.AppendChild(xmlElement);
		Ⴆ(xmlElement, ၝ, Ⴇ: true);
	}

	internal void ఫ(XmlElement ԟ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		XmlElement xmlElement = null;
		if ((int)((Control)ၼ).Dock == 5)
		{
			xmlElement = ԟ.OwnerDocument.CreateElement(_290D._290E);
		}
		else
		{
			xmlElement = ԟ.OwnerDocument.CreateElement(_290D._2911);
			xmlElement.SetAttribute(_290D._2913, XmlConvert.ToString(((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4) ? ((Control)ၼ).Width : ((Control)ၼ).Height));
			xmlElement.SetAttribute(_290D._2912, Enum.GetName(typeof(DockStyle), ((Control)ၼ).Dock));
			xmlElement.SetAttribute(_290D._2915, XmlConvert.ToString(ၼ.ၒ));
		}
		ԟ.AppendChild(xmlElement);
		Ⴆ(xmlElement, ၝ, Ⴇ: false);
	}

	private bool Ⴄ(DocumentDockContainer Ⴅ)
	{
		if (Ⴅ.Documents.Count == 0)
		{
			return false;
		}
		foreach (DocumentBaseContainer document in Ⴅ.Documents)
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

	private void Ⴆ(XmlElement ԟ, DocumentDockContainer Ⴅ, bool Ⴇ)
	{
		if (!Ⴄ(Ⴅ))
		{
			return;
		}
		XmlElement xmlElement = null;
		xmlElement = ԟ.OwnerDocument.CreateElement(_290D._290F);
		ԟ.AppendChild(xmlElement);
		xmlElement.SetAttribute(_290D.ᡥ, XmlConvert.ToString((int)Ⴅ.Orientation));
		xmlElement.SetAttribute(_290D.ܐ, Ⴅ.LayoutBounds.Width.ToString());
		xmlElement.SetAttribute(_290D._0711, Ⴅ.LayoutBounds.Height.ToString());
		foreach (DocumentBaseContainer document in Ⴅ.Documents)
		{
			if (document is DocumentDockContainer)
			{
				Ⴆ(xmlElement, (DocumentDockContainer)document, Ⴇ);
			}
			else if (document is DocumentBarContainer)
			{
				Ⴈ(xmlElement, (DocumentBarContainer)document, Ⴇ);
			}
		}
	}

	private void Ⴈ(XmlElement ԟ, DocumentBarContainer Ⴅ, bool Ⴇ)
	{
		if (Ⴅ.Bar != null && Ⴅ.Bar.SaveLayoutChanges)
		{
			XmlElement xmlElement = ԟ.OwnerDocument.CreateElement(_290D._2910);
			ԟ.AppendChild(xmlElement);
			xmlElement.SetAttribute(_290D.ܐ, Ⴅ.LayoutBounds.Width.ToString());
			xmlElement.SetAttribute(_290D._0711, Ⴅ.LayoutBounds.Height.ToString());
			XmlElement xmlElement2 = ԟ.OwnerDocument.CreateElement(_2918._0FD9);
			xmlElement.AppendChild(xmlElement2);
			if (Ⴇ)
			{
				Ⴅ.Bar.ణ(xmlElement2);
			}
			else
			{
				Ⴅ.Bar.ఫ(xmlElement2);
			}
		}
	}

	internal void బ(XmlElement ԟ)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Invalid comparison between Unknown and I4
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Invalid comparison between Unknown and I4
		if ((ԟ.Name != _290D._290E && ԟ.Name != _290D._2911) || _0E3B)
		{
			return;
		}
		_0E3B = true;
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		try
		{
			foreach (XmlElement childNode in ԟ.ChildNodes)
			{
				if (childNode.Name == _290D._290F)
				{
					itemSerializationContext.ItemXmlElement = childNode;
					Ⴋ(itemSerializationContext, ၝ, Ⴌ: false);
					break;
				}
			}
		}
		finally
		{
			_0E3B = false;
		}
		if (ԟ.Name == _290D._2911)
		{
			if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
			{
				((Control)ၼ).Width = XmlConvert.ToInt32(ԟ.GetAttribute(_290D._2913));
			}
			else if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
			{
				((Control)ၼ).Height = XmlConvert.ToInt32(ԟ.GetAttribute(_290D._2913));
			}
			if (ԟ.HasAttribute(_290D._2915))
			{
				ၼ.ၒ = XmlConvert.ToInt32(ԟ.GetAttribute(_290D._2915));
			}
		}
		ၼ.RecalcLayout();
	}

	internal void Ⴉ(ItemSerializationContext Ⴊ)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Invalid comparison between Unknown and I4
		XmlElement itemXmlElement = Ⴊ.ItemXmlElement;
		if ((itemXmlElement.Name != _290D._290E && itemXmlElement.Name != _290D._2911) || _0E3B)
		{
			return;
		}
		_0E3B = true;
		try
		{
			foreach (XmlElement childNode in itemXmlElement.ChildNodes)
			{
				if (childNode.Name == _290D._290F)
				{
					Ⴊ.ItemXmlElement = childNode;
					Ⴋ(Ⴊ, ၝ, Ⴌ: true);
					break;
				}
			}
			if (itemXmlElement.HasAttribute(_290D._2913))
			{
				if ((int)((Control)ၼ).Dock == 3 || (int)((Control)ၼ).Dock == 4)
				{
					((Control)ၼ).Width = XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D._2913));
				}
				else if ((int)((Control)ၼ).Dock == 1 || (int)((Control)ၼ).Dock == 2)
				{
					((Control)ၼ).Height = XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D._2913));
				}
			}
		}
		finally
		{
			_0E3B = false;
		}
		ၼ.RecalcLayout();
	}

	private void Ⴋ(ItemSerializationContext Ⴊ, DocumentDockContainer Ⴅ, bool Ⴌ)
	{
		XmlElement itemXmlElement = Ⴊ.ItemXmlElement;
		if (itemXmlElement.Name != _290D._290F)
		{
			return;
		}
		Ⴅ.Documents.Clear();
		Ⴅ.Orientation = (eOrientation)XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D.ᡥ));
		if (itemXmlElement.HasAttribute(_290D.ܐ) && itemXmlElement.HasAttribute(_290D._0711))
		{
			Ⴅ._10C9(new Rectangle(0, 0, XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D.ܐ)), XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D._0711))));
		}
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			bool flag = true;
			DocumentBaseContainer documentBaseContainer = _290D._2916(childNode.Name);
			Ⴊ.ItemXmlElement = childNode;
			if (documentBaseContainer is DocumentDockContainer)
			{
				Ⴋ(Ⴊ, (DocumentDockContainer)documentBaseContainer, Ⴌ);
			}
			else if (documentBaseContainer is DocumentBarContainer)
			{
				flag = Ⴍ(Ⴊ, (DocumentBarContainer)documentBaseContainer, Ⴌ);
			}
			if (flag)
			{
				Ⴅ.Documents.Add(documentBaseContainer);
			}
		}
	}

	private bool Ⴍ(ItemSerializationContext Ⴊ, DocumentBarContainer Ⴅ, bool Ⴌ)
	{
		XmlElement itemXmlElement = Ⴊ.ItemXmlElement;
		if (itemXmlElement.Name != _290D._2910)
		{
			return false;
		}
		Ⴅ._10C9(new Rectangle(0, 0, XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D.ܐ)), XmlConvert.ToInt32(itemXmlElement.GetAttribute(_290D._0711))));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == _2918._0FD9))
			{
				continue;
			}
			Ⴅ.Bar = ၼ.Owner.Bars[childNode.GetAttribute(_2918._0D01)];
			if (Ⴌ || (Ⴅ.Bar == null && childNode.HasAttribute(_2918.ᖸ) && XmlConvert.ToBoolean(childNode.GetAttribute(_2918.ᖸ))))
			{
				Ⴅ.Bar = new Bar();
				ၼ.Owner.Bars.Add(Ⴅ.Bar);
				if (Ⴌ)
				{
					Ⴅ.Bar.Deserialize(childNode, Ⴊ);
				}
				else
				{
					Ⴅ.Bar.బ(childNode);
				}
				if (Ⴅ.Bar.Items.Count == 0 && !Ⴌ)
				{
					ၼ.Owner.Bars.Remove(Ⴅ.Bar);
					return false;
				}
				return true;
			}
			if (Ⴅ.Bar != null)
			{
				Ⴅ.Bar.బ(childNode);
				if (!Ⴅ.Bar.ଇ)
				{
					if (Ⴅ.LayoutBounds.Height > 0 && (Ⴅ.Bar.DockSide == eDockSide.Top || Ⴅ.Bar.DockSide == eDockSide.Bottom))
					{
						((Control)Ⴅ.Bar).Height = Ⴅ.LayoutBounds.Height;
					}
					else if (Ⴅ.LayoutBounds.Width > 0 && (Ⴅ.Bar.DockSide == eDockSide.Left || Ⴅ.Bar.DockSide == eDockSide.Right))
					{
						((Control)Ⴅ.Bar).Width = Ⴅ.LayoutBounds.Width;
					}
				}
				break;
			}
			return false;
		}
		return true;
	}
}
