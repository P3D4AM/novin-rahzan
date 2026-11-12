using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class DockSite : Control
{
	public class DockSiteAccessibleObject : ControlAccessibleObject
	{
		private DockSite m_Owner;

		public override AccessibleRole Role
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (m_Owner == null || ((Control)m_Owner).IsDisposed)
				{
					return (AccessibleRole)0;
				}
				return ((Control)m_Owner).AccessibleRole;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (m_Owner != null && !((Control)m_Owner).IsDisposed && ((Control)m_Owner).Parent != null)
				{
					return ((Control)m_Owner).Parent.RectangleToScreen(((Control)m_Owner).Bounds);
				}
				return Rectangle.Empty;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				if (m_Owner == null || ((Control)m_Owner).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				return (AccessibleStates)0;
			}
		}

		public DockSiteAccessibleObject(DockSite owner)
			: base((Control)(object)owner)
		{
			m_Owner = owner;
		}

		public override int GetChildCount()
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed)
			{
				return ((ArrangedElementCollection)((Control)m_Owner).Controls).Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed && ((ArrangedElementCollection)((Control)m_Owner).Controls).Count > 0 && ((ArrangedElementCollection)((Control)m_Owner).Controls).Count > iIndex)
			{
				return ((Control)m_Owner).Controls[iIndex].AccessibilityObject;
			}
			return null;
		}
	}

	private eOrientation m_DockOrientation;

	private bool m_Layingout;

	private bool m_OversizeLayout;

	private DotNetBarManager m_Owner;

	private bool m_NeedsLayout;

	private bool m_NeedsNormalize;

	private ThemeRebar m_ThemeRebar;

	private eBackgroundImagePosition m_BackgroundImagePosition;

	private byte m_BackgroundImageAlpha = byte.MaxValue;

	private Color m_BackColor2 = Color.Empty;

	private int m_BackColorGradientAngle = 90;

	private bool m_Disposed;

	private DocumentDockUIManager m_DocumentUIManager;

	private bool m_LayoutSuspended;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool _DesignerLoading;

	private int m_OriginalSize;

	private bool m_OptimizeLayoutRedraw = true;

	private DocumentDockContainer m_DocumentDockContainer;

	[DefaultValue(true)]
	[Description("Indicates whether painting is disabled on dock site while layout of bars is performed.")]
	public bool OptimizeLayoutRedraw
	{
		get
		{
			return m_OptimizeLayoutRedraw;
		}
		set
		{
			m_OptimizeLayoutRedraw = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DocumentDockContainer DocumentDockContainer
	{
		get
		{
			return m_DocumentDockContainer;
		}
		set
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			m_DocumentDockContainer = value;
			m_DocumentUIManager = GetDocumentUIManager();
			m_DocumentUIManager.RootDocumentDockContainer = m_DocumentDockContainer;
			if ((int)((Control)this).Dock != 5 && value != null)
			{
				m_DocumentDockContainer.RecordDocumentSize = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return m_BackgroundImagePosition;
		}
		set
		{
			if (m_BackgroundImagePosition != value)
			{
				m_BackgroundImagePosition = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(byte.MaxValue)]
	[Description("Specifies the transparency of background image.")]
	public byte BackgroundImageAlpha
	{
		get
		{
			return m_BackgroundImageAlpha;
		}
		set
		{
			if (m_BackgroundImageAlpha != value)
			{
				m_BackgroundImageAlpha = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates the target gradient background color.")]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return m_BackColor2;
		}
		set
		{
			m_BackColor2 = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates gradient fill angle.")]
	[Category("Style")]
	[DefaultValue(90)]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			if (value != m_BackColorGradientAngle)
			{
				m_BackColorGradientAngle = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	private ThemeRebar ThemeRebar
	{
		get
		{
			if (m_ThemeRebar == null)
			{
				m_ThemeRebar = new ThemeRebar((Control)(object)this);
			}
			return m_ThemeRebar;
		}
	}

	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			((Control)this).Dock = value;
			if ((int)value == 1 || (int)value == 2 || (int)value == 5)
			{
				if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
				{
					((Control)this).Height = 0;
				}
				m_DockOrientation = eOrientation.Horizontal;
			}
			else
			{
				if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
				{
					((Control)this).Width = 0;
				}
				m_DockOrientation = eOrientation.Vertical;
			}
		}
	}

	internal bool IsAnyControlVisible
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is Bar)
				{
					if (((Bar)(object)val).IsVisible)
					{
						return true;
					}
				}
				else if (val.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool NeedsLayout
	{
		get
		{
			return m_NeedsLayout;
		}
		set
		{
			m_NeedsLayout = value;
		}
	}

	private bool IsLayoutSuspended
	{
		get
		{
			if ((m_Owner != null && m_Owner.SuspendLayout) || m_LayoutSuspended || _DesignerLoading)
			{
				return true;
			}
			return false;
		}
	}

	internal int OriginalSize
	{
		get
		{
			return m_OriginalSize;
		}
		set
		{
			m_OriginalSize = value;
		}
	}

	internal eOrientation DockOrientation => m_DockOrientation;

	internal bool IsDocumentDock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((Control)this).Dock != 5)
			{
				return m_DocumentDockContainer != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public DotNetBarManager Owner => m_Owner;

	internal bool HasFixedBars
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is Bar { LockDockPosition: not false })
				{
					return true;
				}
			}
			return false;
		}
	}

	public DockSite(DockStyle DockSide)
		: this()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).Dock = DockSide;
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new DockSiteAccessibleObject(this);
	}

	public DockSite()
	{
		((Control)this).TabStop = false;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).IsAccessible = true;
		((Control)this).AccessibleRole = (AccessibleRole)9;
	}

	protected override void Dispose(bool disposing)
	{
		m_Owner = null;
		m_Disposed = true;
		((Control)this).Dispose(disposing);
	}

	public DocumentDockUIManager GetDocumentUIManager()
	{
		if (m_DocumentUIManager == null)
		{
			m_DocumentUIManager = new DocumentDockUIManager(this);
			m_DocumentUIManager.RootDocumentDockContainer = m_DocumentDockContainer;
		}
		return m_DocumentUIManager;
	}

	public void SaveLayout(XmlElement xmlBars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (IsDocumentDock || m_DocumentDockContainer != null)
		{
			GetDocumentUIManager().SerializeLayout(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is Bar bar && bar.Name != "" && bar.SaveLayoutChanges)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.SerializeLayout(xmlElement);
			}
		}
	}

	public void LoadLayout(XmlElement xmlBars)
	{
		m_LayoutSuspended = true;
		try
		{
			if (!IsDocumentDock && m_DocumentDockContainer == null)
			{
				foreach (XmlElement childNode in xmlBars.ChildNodes)
				{
					Bar bar = null;
					if (m_Owner.Bars.Contains(m_Owner.Bars[childNode.GetAttribute("name")]))
					{
						bar = m_Owner.Bars[childNode.GetAttribute("name")];
					}
					else
					{
						bar = new Bar();
						m_Owner.Bars.Add(bar);
					}
					bar.DeserializeLayout(childNode);
				}
				return;
			}
			GetDocumentUIManager().DeserializeLayout((XmlElement)xmlBars.FirstChild);
		}
		finally
		{
			m_LayoutSuspended = false;
			RecalcLayout();
		}
	}

	public void SuspendLayout()
	{
		m_LayoutSuspended = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout()
	{
		ResumeLayout(performLayout: true);
	}

	public void ResumeLayout(bool performLayout)
	{
		m_LayoutSuspended = false;
		((Control)this).ResumeLayout(performLayout);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackgroundImageAlpha()
	{
		return m_BackgroundImageAlpha != byte.MaxValue;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !m_BackColor2.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		m_BackColor2 = Color.Empty;
	}

	internal bool GetDesignMode()
	{
		return ((Component)this).DesignMode;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			AccessibleObject accessibilityObject = ((Control)this).AccessibilityObject;
			ControlAccessibleObject val = (ControlAccessibleObject)(object)((accessibilityObject is ControlAccessibleObject) ? accessibilityObject : null);
			if (val != null)
			{
				val.NotifyClients((AccessibleEvents)32770);
			}
		}
	}

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
		if (IsLayoutSuspended || (((Control)this).BackgroundImage != null && BackgroundImageAlpha == byte.MaxValue && (BackgroundImagePosition == eBackgroundImagePosition.Stretch || BackgroundImagePosition == eBackgroundImagePosition.Tile || (((Control)this).BackgroundImage.Width >= ((Control)this).Width && ((Control)this).BackgroundImage.Height >= ((Control)this).Height))))
		{
			return;
		}
		if (!m_BackColor2.IsEmpty && !((Control)this).BackColor.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			DisplayHelp.FillRectangle(pevent.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor, BackColor2, m_BackColorGradientAngle);
			return;
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar && ((Bar)(object)((Control)this).Controls[0]).ThemedBackground)
		{
			ThemeRebar themeRebar = ThemeRebar;
			themeRebar.DrawBackground(pevent.Graphics, ThemeRebarParts.Background, ThemeRebarStates.Normal, ((Control)this).ClientRectangle);
			return;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme != null && ((Control)this).BackColor != Color.Transparent)
		{
			DisplayHelp.FillRectangle(pevent.Graphics, ((Control)this).ClientRectangle, colorScheme.DockSiteBackColor, colorScheme.DockSiteBackColor2, colorScheme.DockSiteBackColorGradientAngle);
		}
		else
		{
			((Control)this).OnPaintBackground(pevent);
		}
	}

	private ColorScheme GetColorScheme()
	{
		if (m_Owner != null && m_Owner.UseGlobalColorScheme)
		{
			if ((m_Owner.Style == eDotNetBarStyle.Office2007 || m_Owner.Style == eDotNetBarStyle.StyleManagerControlled) && GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return m_Owner.ColorScheme;
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar && (((Bar)(object)((Control)this).Controls[0]).Style == eDotNetBarStyle.Office2003 || ((Bar)(object)((Control)this).Controls[0]).Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(((Bar)(object)((Control)this).Controls[0]).Style)))
		{
			Bar bar = ((Control)this).Controls[0] as Bar;
			if (bar.Style == eDotNetBarStyle.StyleManagerControlled && GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return bar.GetColorScheme();
		}
		return null;
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		if (!IsLayoutSuspended)
		{
			if (((Control)this).BackgroundImage != null)
			{
				BarFunctions.PaintBackgroundImage(pevent.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackgroundImage, m_BackgroundImagePosition, m_BackgroundImageAlpha);
			}
			else
			{
				((Control)this).OnPaint(pevent);
			}
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		RecalcLayout();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (m_ThemeRebar != null)
		{
			m_ThemeRebar.Dispose();
			m_ThemeRebar = null;
		}
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			m_ThemeRebar = null;
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (!(e.Control is IDockInfo))
		{
			((Control)this).Controls.Remove(e.Control);
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		UpdateBarsCollection();
	}

	internal void AddBar(Control objBar)
	{
		if (!(objBar is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Visible && (Owner == null || (Owner != null && !Owner.IsLoadingDefinition)))
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		dockInfo.DockOrientation = m_DockOrientation;
		Control val = null;
		IDockInfo dockInfo2 = null;
		bool flag = false;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			val = ((Control)this).Controls[i];
			if (val is IDockInfo dockInfo3 && ((dockInfo.DockLine <= dockInfo3.DockLine && dockInfo.DockOffset < dockInfo3.DockOffset) || dockInfo.DockLine < dockInfo3.DockLine))
			{
				((Control)this).Controls.Add(objBar);
				((Control)this).Controls.SetChildIndex(objBar, i);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			((Control)this).Controls.Add(objBar);
		}
		if (objBar is Bar && ((Bar)(object)objBar).LayoutType == eLayoutType.DockContainer)
		{
			Bar bar = objBar as Bar;
			if (m_DockOrientation == eOrientation.Horizontal && bar.ItemsContainer.MinHeight > 0)
			{
				((Bar)(object)objBar).SyncLineMinHeight();
			}
			else if (m_DockOrientation == eOrientation.Vertical && bar.ItemsContainer.MinWidth > 0)
			{
				((Bar)(object)objBar).SyncLineMinWidth();
			}
		}
		NormalizeBars();
		LayoutBars();
	}

	internal void AddBar(Control objBar, int iInsertAtPosition)
	{
		if (!(objBar is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Visible)
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		dockInfo.DockOrientation = m_DockOrientation;
		((Control)this).Controls.Add(objBar);
		((Control)this).Controls.SetChildIndex(objBar, iInsertAtPosition);
		if (objBar is Bar && ((Bar)(object)objBar).LayoutType == eLayoutType.DockContainer)
		{
			Bar bar = objBar as Bar;
			if (m_DockOrientation == eOrientation.Horizontal && bar.ItemsContainer.MinHeight > 0)
			{
				((Bar)(object)objBar).SyncLineMinHeight();
			}
			else if (m_DockOrientation == eOrientation.Vertical && bar.ItemsContainer.MinWidth > 0)
			{
				((Bar)(object)objBar).SyncLineMinWidth();
			}
		}
		NormalizeBars();
		LayoutBars();
	}

	internal void SetBarPosition(Control objBar, int iNewIndex)
	{
		if (((Control)this).Contains(objBar))
		{
			if (((Control)this).Controls.GetChildIndex(objBar) != iNewIndex)
			{
				((Control)this).Controls.SetChildIndex(objBar, iNewIndex);
			}
			LayoutBars();
		}
	}

	internal void SetBarPosition(Control objBar, int iNewIndex, bool bInNewLine)
	{
		if (!((Control)this).Contains(objBar))
		{
			return;
		}
		if (((Control)this).Controls.GetChildIndex(objBar) != iNewIndex)
		{
			((Control)this).Controls.SetChildIndex(objBar, iNewIndex);
		}
		if (bInNewLine)
		{
			iNewIndex = ((Control)this).Controls.IndexOf(objBar);
			if (iNewIndex >= 0)
			{
				if (iNewIndex > 0)
				{
					((IDockInfo)objBar).SetDockLine(((IDockInfo)((Control)this).Controls[iNewIndex - 1]).DockLine + 1);
				}
				else
				{
					((IDockInfo)objBar).SetDockLine(-1);
				}
				for (int i = iNewIndex + 1; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
				{
					((IDockInfo)((Control)this).Controls[i]).SetDockLine(((IDockInfo)((Control)this).Controls[i]).DockLine + 2);
				}
			}
		}
		LayoutBars();
	}

	internal void AdjustBarPosition(Control objBar)
	{
		if (!(objBar is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Controls.Contains(objBar))
		{
			throw new ArgumentException("Control is not part of this collection.");
		}
		Control val = null;
		IDockInfo dockInfo2 = null;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			val = ((Control)this).Controls[i];
			if (val == objBar || !(val is IDockInfo dockInfo3))
			{
				continue;
			}
			if (dockInfo.DockLine == dockInfo3.DockLine && dockInfo.DockOffset <= dockInfo3.DockOffset)
			{
				if (i > 0 && i > ((Control)this).Controls.GetChildIndex(objBar))
				{
					i--;
				}
				((Control)this).Controls.SetChildIndex(objBar, i);
				return;
			}
			if (dockInfo.DockLine < dockInfo3.DockLine)
			{
				if (i > 0)
				{
					i--;
				}
				((Control)this).Controls.SetChildIndex(objBar, i);
				return;
			}
		}
		((Control)this).Controls.SetChildIndex(objBar, ((ArrangedElementCollection)((Control)this).Controls).Count);
	}

	public void RecalcLayout()
	{
		LayoutBars();
	}

	private void NormalizeBars()
	{
		if (IsLayoutSuspended)
		{
			m_NeedsNormalize = true;
			return;
		}
		int num = -1;
		int num2 = -2;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is Bar bar)
			{
				if (bar.DockLine != num2)
				{
					num++;
					num2 = bar.DockLine;
					bar.SetDockLine(num);
				}
				else
				{
					bar.SetDockLine(num);
				}
			}
		}
		m_NeedsNormalize = false;
	}

	private Rectangle GetDockLayoutRectangle()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if ((int)((Control)this).Dock == 5 || m_DocumentDockContainer == null)
		{
			return clientRectangle;
		}
		int splitterSize = m_DocumentDockContainer.SplitterSize;
		if ((int)((Control)this).Dock == 3)
		{
			clientRectangle.Width -= splitterSize;
		}
		else if ((int)((Control)this).Dock == 4)
		{
			clientRectangle.Width -= splitterSize;
			clientRectangle.X += splitterSize;
		}
		else if ((int)((Control)this).Dock == 2)
		{
			clientRectangle.Height -= splitterSize;
			clientRectangle.Y += splitterSize;
		}
		else if ((int)((Control)this).Dock == 1)
		{
			clientRectangle.Height -= splitterSize;
		}
		return clientRectangle;
	}

	private void LayoutBars()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Invalid comparison between Unknown and I4
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Invalid comparison between Unknown and I4
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Invalid comparison between Unknown and I4
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Invalid comparison between Unknown and I4
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Invalid comparison between Unknown and I4
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Invalid comparison between Unknown and I4
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7b: Invalid comparison between Unknown and I4
		//IL_0f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9f: Invalid comparison between Unknown and I4
		//IL_0fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Invalid comparison between Unknown and I4
		if (m_Disposed || ((Control)this).IsDisposed)
		{
			return;
		}
		if (IsLayoutSuspended)
		{
			m_NeedsLayout = true;
			return;
		}
		if (m_NeedsNormalize)
		{
			NormalizeBars();
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4) && ((Control)this).Width == 0) || (((int)((Control)this).Dock == 1 || (int)((Control)this).Dock == 2) && ((Control)this).Height == 0)))
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null && (((Control)val).Width == 0 || ((Control)val).Height == 0))
		{
			return;
		}
		if (val != null && ((ContainerControl)val).ParentForm != null)
		{
			val = ((ContainerControl)val).ParentForm;
		}
		if ((val != null && (((Control)val).Width == 0 || ((Control)val).Height == 0)) || (val != null && (int)val.WindowState == 1))
		{
			return;
		}
		m_NeedsLayout = false;
		if (m_Layingout)
		{
			return;
		}
		m_Layingout = true;
		int num = -1;
		if ((int)((Control)this).Dock == 5 || m_DocumentDockContainer != null)
		{
			Rectangle bounds = ((Control)this).Bounds;
			if (((Control)this).Parent != null && ((Control)this).Parent.Visible && m_OptimizeLayoutRedraw)
			{
				if (((Control)this).Parent != null)
				{
					num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
				}
				if (((Control)this).Handle != IntPtr.Zero)
				{
					NativeFunctions.SendMessage(((Control)this).Handle, 11, 0, 0);
				}
			}
			if (m_DocumentDockContainer != null)
			{
				if ((int)((Control)this).Dock != 5)
				{
					m_DocumentDockContainer.RecordDocumentSize = true;
				}
				m_DocumentDockContainer.Layout(GetDockLayoutRectangle());
				if ((int)((Control)this).Dock != 5 && m_DocumentDockContainer.DisplayBounds.IsEmpty)
				{
					if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
					{
						((Control)this).Width = 0;
					}
					else
					{
						((Control)this).Height = 0;
					}
				}
			}
			m_Layingout = false;
			if (((Control)this).Parent != null && ((Control)this).Parent.Visible && m_OptimizeLayoutRedraw)
			{
				NativeFunctions.SendMessage(((Control)this).Handle, 11, 1, 0);
				((Control)this).Parent.Invalidate(((((Control)this).Width == 0 || ((Control)this).Height == 0) && bounds.Width > 0 && bounds.Height > 0) ? bounds : ((Control)this).Bounds, true);
				((Control)this).Parent.Update();
			}
			if (num != -1)
			{
				((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, num);
			}
			return;
		}
		IDockInfo dockInfo = null;
		Bar bar = null;
		Control val2 = null;
		bool flag = false;
		Rectangle bounds2 = ((Control)this).Bounds;
		if (((Control)this).Parent != null && ((Control)this).Parent.Visible && m_OptimizeLayoutRedraw)
		{
			if (((Control)this).Parent != null)
			{
				num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
			}
			NativeFunctions.SendMessage(((Control)this).Handle, 11, 0, 0);
		}
		int num2 = 0;
		int num3 = 0;
		int[] array = new int[256];
		int[] array2 = new int[256];
		int[] array3 = new int[256];
		int[] array4 = new int[256];
		int[] array5 = new int[256];
		ArrayList arrayList = null;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			bar = ((Control)this).Controls[i] as Bar;
			if ((bar != null && !bar.Visible) || bar == null || bar.LayoutType != eLayoutType.DockContainer || !bar.Stretch || bar.DockLine < 0)
			{
				continue;
			}
			array[bar.DockLine]++;
			array2[bar.DockLine]++;
			if (m_DockOrientation == eOrientation.Horizontal)
			{
				if (bar.SplitDockWidth == 0)
				{
					array5[bar.DockLine]++;
				}
				else
				{
					array4[bar.DockLine] += bar.SplitDockWidth;
				}
			}
			else if (bar.SplitDockHeight == 0)
			{
				array5[bar.DockLine]++;
			}
			else
			{
				array4[bar.DockLine] += bar.SplitDockHeight;
			}
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar && ((Bar)(object)((Control)this).Controls[0]).LayoutType == eLayoutType.Toolbar && (((Bar)(object)((Control)this).Controls[0]).Style == eDotNetBarStyle.Office2003 || ((Bar)(object)((Control)this).Controls[0]).Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(((Bar)(object)((Control)this).Controls[0]).Style)))
		{
			num2 = 2;
			num3 = 1;
		}
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		bool flag2 = false;
		int num7 = 0;
		int num8 = 0;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		do
		{
			num4 = 0;
			num5 = 0;
			num6 = 0;
			num7 = 0;
			num8 = 0;
			flag3 = false;
			arrayList = new ArrayList(((ArrangedElementCollection)((Control)this).Controls).Count);
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				dockInfo = ((Control)this).Controls[0] as IDockInfo;
				num8 = dockInfo.DockLine;
			}
			bool flag6 = false;
			for (int j = 0; j < ((ArrangedElementCollection)((Control)this).Controls).Count; j++)
			{
				val2 = ((Control)this).Controls[j];
				dockInfo = val2 as IDockInfo;
				bar = val2 as Bar;
				flag2 = false;
				if (dockInfo == null || !val2.Visible)
				{
					if (bar != null && dockInfo.DockLine > num8)
					{
						num8 = dockInfo.DockLine;
						if (!flag4)
						{
							num7++;
							flag5 = true;
						}
					}
					flag6 = true;
					continue;
				}
				flag4 = false;
				if (m_DockOrientation == eOrientation.Horizontal)
				{
					if (flag5)
					{
						num6 += num4 + num3;
						num5 = 0;
						num4 = 0;
						flag5 = false;
					}
					Size frameSize = Size.Empty;
					Size size = ((bar.LayoutType != 0) ? new Size(1, 1) : bar.GetAdjustedFullSize(dockInfo.MinimumDockSize(m_DockOrientation)));
					if (bar.Stretch || (j > 0 && ((num5 + val2.Width > ((Control)this).Width && size.Width + num5 > ((Control)this).Width) || dockInfo.DockLine > num8)))
					{
						if (bar.Stretch && bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array[bar.DockLine] > 0)
						{
							if (array[bar.DockLine] == 1)
							{
								bar.SplitDockWidth = 0;
							}
							frameSize = ((bar.SplitDockWidth != 0 && (array2[bar.DockLine] != 1 || array4[bar.DockLine] == ((Control)this).Width)) ? new Size(bar.SplitDockWidth, 1) : ((array2[bar.DockLine] != 1) ? new Size((((Control)this).Width - array4[bar.DockLine]) / ((array5[bar.DockLine] == 0) ? 1 : array5[bar.DockLine]), 1) : new Size(((Control)this).Width - num5, 1)));
							if (num8 != dockInfo.DockLine && array2[bar.DockLine] == array[bar.DockLine] && num5 > 0)
							{
								num8 = dockInfo.DockLine;
								num7++;
								num6 += num4 + num3;
								num5 = 0;
								num4 = 0;
								if (array[bar.DockLine] == 1 || frameSize.IsEmpty)
								{
									frameSize = new Size(((Control)this).Width, 1);
								}
							}
							array2[bar.DockLine]--;
							flag2 = true;
							arrayList.Add(bar);
						}
						else
						{
							frameSize = new Size(((Control)this).Width, 1);
						}
					}
					else
					{
						frameSize = new Size(((Control)this).Width - num5, 1);
					}
					bar.RecalcSize(frameSize);
					if (j > 0 && dockInfo.DockLine > num8 && num5 > 0)
					{
						if (flag2)
						{
							array[bar.DockLine] = 0;
							array2[bar.DockLine] = 0;
							flag3 = true;
							break;
						}
						flag = ((dockInfo.DockLine <= num8) ? true : false);
						num8 = dockInfo.DockLine;
						num7++;
						num6 += num4 + num3;
						num5 = 0;
						num4 = 0;
						if (flag)
						{
							dockInfo.DockOffset = 0;
						}
						if (dockInfo.DockOffset > 0)
						{
							num5 = ((dockInfo.DockOffset + val2.Width <= ((Control)this).Width) ? dockInfo.DockOffset : (((Control)this).Width - val2.Width));
							if (num5 < 0)
							{
								num5 = 0;
							}
						}
					}
					else
					{
						if (num8 != dockInfo.DockLine)
						{
							num8 = dockInfo.DockLine;
							if (flag6)
							{
								num7++;
							}
						}
						if (flag)
						{
							dockInfo.DockOffset = 0;
						}
						if (!dockInfo.Stretch)
						{
							if (dockInfo.DockOffset > num5 && dockInfo.DockOffset + val2.Width <= ((Control)this).Width)
							{
								num5 = dockInfo.DockOffset;
							}
							else if (dockInfo.DockOffset > num5 && ((Control)this).Width - val2.Width > num5)
							{
								num5 = ((Control)this).Width - val2.Width;
							}
						}
					}
					if (num5 + val2.Width > ((Control)this).Width)
					{
						if (bar != null)
						{
							bar.RecalcSize(new Size(((Control)this).Width - num5, ((Control)bar).Height));
						}
						else
						{
							val2.Width = ((Control)this).Width - num5;
						}
						dockInfo.DockOffset = 0;
					}
					val2.Location = new Point(num5, num6);
					num5 += val2.Width + num2;
					if (flag || dockInfo.DockLine != num7)
					{
						dockInfo.DockOffset = val2.Left;
					}
					if (val2.Height > num4)
					{
						num4 = val2.Height;
					}
					if (bar != null && bar.ItemsContainer.HeightInternal > array3[num7])
					{
						array3[num7] = bar.ItemsContainer.HeightInternal;
					}
					if (dockInfo.Stretch)
					{
						if (bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array2[bar.DockLine] > 0)
						{
							dockInfo.SetDockLine(num7);
						}
						else
						{
							dockInfo.SetDockLine(num7);
							num8 = dockInfo.DockLine;
							num7++;
							flag4 = true;
							num6 += num4 + num3;
							num5 = 0;
							num4 = 0;
						}
					}
					else
					{
						dockInfo.SetDockLine(num7);
					}
				}
				else
				{
					if (flag5)
					{
						num5 += num4 + num3;
						num6 = 0;
						num4 = 0;
						flag5 = false;
					}
					Size frameSize2 = Size.Empty;
					Size size = new Size(1, 1);
					if (bar.Stretch || (j > 0 && ((num6 + val2.Height > ((Control)this).Height && size.Height + num6 > ((Control)this).Height) || dockInfo.DockLine > num8)))
					{
						if (bar.Stretch && bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array[bar.DockLine] > 0)
						{
							if (array[bar.DockLine] == 1)
							{
								bar.SplitDockHeight = 0;
							}
							frameSize2 = ((bar.SplitDockHeight != 0 && (array2[bar.DockLine] != 1 || array4[bar.DockLine] == ((Control)this).Height)) ? new Size(1, bar.SplitDockHeight) : ((array2[bar.DockLine] != 1) ? new Size(1, (((Control)this).Height - array4[bar.DockLine]) / ((array5[bar.DockLine] == 0) ? 1 : array5[bar.DockLine])) : new Size(1, ((Control)this).Height - num6)));
							if (num8 != dockInfo.DockLine && array2[bar.DockLine] == array[bar.DockLine] && num6 > 0)
							{
								num8 = dockInfo.DockLine;
								num7++;
								num5 += num4 + num3;
								num6 = 0;
								num4 = 0;
								frameSize2 = new Size(1, ((Control)this).Height);
							}
							array2[bar.DockLine]--;
							flag2 = true;
							arrayList.Add(bar);
						}
						else
						{
							frameSize2 = new Size(1, ((Control)this).Height);
						}
					}
					else
					{
						frameSize2 = new Size(1, ((Control)this).Height - num6);
					}
					bar.RecalcSize(frameSize2);
					if (j > 0 && dockInfo.DockLine > num8 && num6 > 0)
					{
						if (flag2)
						{
							array[bar.DockLine] = 0;
							array2[bar.DockLine] = 0;
							flag3 = true;
							break;
						}
						flag = ((dockInfo.DockLine <= num8) ? true : false);
						num8 = dockInfo.DockLine;
						num7++;
						num5 += num4 + num3;
						num6 = 0;
						num4 = 0;
						if (flag)
						{
							dockInfo.DockOffset = 0;
						}
						if (dockInfo.DockOffset > 0)
						{
							num6 = ((dockInfo.DockOffset + val2.Height <= ((Control)this).Height) ? dockInfo.DockOffset : (((Control)this).Height - val2.Height));
							if (num6 < 0)
							{
								num6 = 0;
							}
						}
					}
					else
					{
						if (num8 != dockInfo.DockLine)
						{
							num8 = dockInfo.DockLine;
						}
						if (flag)
						{
							dockInfo.DockOffset = 0;
						}
						if (!dockInfo.Stretch)
						{
							if (dockInfo.DockOffset > num6 && dockInfo.DockOffset + val2.Height <= ((Control)this).Height)
							{
								num6 = dockInfo.DockOffset;
							}
							else if (dockInfo.DockOffset > num6 && ((Control)this).Height - val2.Height > num6)
							{
								num6 = ((Control)this).Height - val2.Height;
							}
						}
					}
					if (num6 + val2.Height > ((Control)this).Height)
					{
						if (bar != null)
						{
							bar.RecalcSize(new Size(((Control)bar).Width, ((Control)this).Height - num6));
						}
						else
						{
							val2.Height = ((Control)this).Height - num6;
						}
						dockInfo.DockOffset = 0;
					}
					val2.Location = new Point(num5, num6);
					num6 += val2.Height + num2;
					if (flag || dockInfo.DockLine != num7)
					{
						dockInfo.DockOffset = val2.Top;
					}
					if (val2.Width > num4)
					{
						num4 = val2.Width;
					}
					if (bar != null && bar.ItemsContainer.WidthInternal > array3[num7])
					{
						array3[num7] = bar.ItemsContainer.WidthInternal;
					}
					if (dockInfo.Stretch)
					{
						if (bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array2[bar.DockLine] > 0)
						{
							dockInfo.SetDockLine(num7);
						}
						else
						{
							dockInfo.SetDockLine(num7);
							num8 = dockInfo.DockLine;
							num7++;
							num5 += num4 + num3;
							num6 = 0;
							num4 = 0;
						}
					}
					else
					{
						dockInfo.SetDockLine(num7);
					}
				}
				flag6 = false;
			}
		}
		while (flag3);
		foreach (Bar item in arrayList)
		{
			if (m_DockOrientation == eOrientation.Horizontal)
			{
				if (item.ItemsContainer.HeightInternal != array3[item.DockLine])
				{
					item.ItemsContainer.MinHeight = array3[item.DockLine];
					Size frameSize3 = new Size(((Control)item).Width, 1);
					item.RecalcSize(frameSize3);
				}
			}
			else if (item.ItemsContainer.WidthInternal != array3[item.DockLine])
			{
				item.ItemsContainer.MinWidth = array3[item.DockLine];
				Size frameSize4 = new Size(1, ((Control)item).Height);
				item.RecalcSize(frameSize4);
			}
		}
		if (m_DockOrientation == eOrientation.Horizontal)
		{
			if (((Control)this).Height != num6 + num4)
			{
				((Control)this).Height = num6 + num4;
				if (((Control)this).Parent != null)
				{
					((Control)this).Parent.PerformLayout((Control)(object)this, "Height");
				}
				((Control)this).Invalidate();
			}
		}
		else if (((Control)this).Width != num5 + num4)
		{
			((Control)this).Width = num5 + num4;
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.PerformLayout((Control)(object)this, "Width");
			}
			((Control)this).Invalidate();
		}
		if (!m_OversizeLayout)
		{
			if (((int)((Control)this).Dock == 4 && ((Control)this).Location.X < 0) || ((int)((Control)this).Dock == 3 && ((Control)this).Parent != null && ((Control)this).Right > ((Control)this).Parent.Width) || ((int)((Control)this).Dock == 1 && ((Control)this).Parent != null && ((Control)this).Bottom > ((Control)this).Parent.Height) || ((int)((Control)this).Dock == 2 && ((Control)this).Location.Y < 0))
			{
				m_OversizeLayout = true;
				m_Layingout = false;
				EnforceClientMinSize();
				if (m_OversizeLayout)
				{
					m_OversizeLayout = false;
				}
			}
		}
		else
		{
			m_OversizeLayout = false;
		}
		if (((Control)this).Parent != null && ((Control)this).Parent.Visible && m_OptimizeLayoutRedraw)
		{
			NativeFunctions.SendMessage(((Control)this).Handle, 11, 1, 0);
			if (num != -1)
			{
				((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, num);
			}
			((Control)this).Parent.Invalidate(bounds2, true);
			((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
			((Control)this).Parent.Update();
		}
		m_Layingout = false;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		Control val = (Control)(object)((Control)this).FindForm();
		if (val != null)
		{
			val.Resize += ParentResized;
		}
	}

	private void ParentResized(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		Control parent = ((Control)this).Parent;
		if ((parent == null || (parent.Width != 0 && parent.Height != 0)) && (parent == null || !(parent is Form) || (int)((Form)parent).WindowState != 1))
		{
			EnforceClientMinSize();
		}
	}

	private void EnforceClientMinSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		if ((int)((Control)this).Dock == 5 || (int)((Control)this).Dock == 0 || m_DocumentDockContainer == null || ((Control)this).Width == 0 || ((Control)this).Height == 0 || (((Control)this).Parent != null && (((Control)this).Parent.Width == 0 || ((Control)this).Parent.Height == 0)))
		{
			return;
		}
		bool flag = (int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3;
		int num = (flag ? GetClientWidth() : GetClientHeight());
		int num2 = 32;
		if (m_Owner != null)
		{
			num2 = (flag ? m_Owner.MinimumClientSize.Width : m_Owner.MinimumClientSize.Height);
		}
		if (num > num2)
		{
			if (m_OriginalSize <= 0)
			{
				return;
			}
			int num3 = m_OriginalSize - (flag ? ((Control)this).Width : ((Control)this).Height);
			if (num - num3 < num2)
			{
				num3 -= num2 - (num - num3);
			}
			if (num3 > 0)
			{
				if (flag)
				{
					((Control)this).Width = ((Control)this).Width + num3;
				}
				else
				{
					((Control)this).Height = ((Control)this).Height + num3;
				}
			}
			if ((flag && ((Control)this).Width >= m_OriginalSize) || (!flag && ((Control)this).Height >= m_OriginalSize))
			{
				m_OriginalSize = 0;
			}
			return;
		}
		GetDocumentUIManager().RootDocumentDockContainer.RefreshMinimumSize();
		DocumentDockUIManager documentUIManager = GetDocumentUIManager();
		Size containerMinimumSize = documentUIManager.GetContainerMinimumSize(documentUIManager.RootDocumentDockContainer);
		int num4 = (flag ? containerMinimumSize.Width : containerMinimumSize.Height);
		int num5 = num2 - num;
		if ((flag && ((Control)this).Width - num5 < num4) || (!flag && ((Control)this).Height - num5 < num4))
		{
			num5 = (flag ? ((Control)this).Width : ((Control)this).Height) - num4;
		}
		if (num5 > 0)
		{
			if (m_OriginalSize == 0)
			{
				m_OriginalSize = (flag ? ((Control)this).Width : ((Control)this).Height);
			}
			if (flag)
			{
				((Control)this).Width = ((Control)this).Width - num5;
			}
			else
			{
				((Control)this).Height = ((Control)this).Height - num5;
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		((Control)this).OnResize(e);
		if (!m_Layingout)
		{
			Control parent = ((Control)this).Parent;
			if ((parent == null || (parent.Width != 0 && parent.Height != 0)) && (parent == null || !(parent is Form) || (int)((Form)parent).WindowState != 1) && parent != null && ((m_Owner != null && m_Owner.ParentForm != null && (int)m_Owner.ParentForm.WindowState != 1) || m_Owner == null || m_Owner.ParentForm == null))
			{
				((Control)this).Invalidate(((Control)this).Region, false);
				LayoutBars();
			}
		}
	}

	internal int GetClientWidth()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		if (((Control)this).Parent == null)
		{
			return 0;
		}
		Control parent = ((Control)this).Parent;
		int num = parent.ClientSize.Width;
		foreach (Control item in (ArrangedElementCollection)parent.Controls)
		{
			Control val = item;
			if (val.Visible && ((int)val.Dock == 3 || (int)val.Dock == 4))
			{
				num -= val.Width;
			}
		}
		return num;
	}

	internal int GetClientHeight()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		if (((Control)this).Parent == null)
		{
			return 0;
		}
		Control parent = ((Control)this).Parent;
		int num = parent.ClientSize.Height;
		foreach (Control item in (ArrangedElementCollection)parent.Controls)
		{
			Control val = item;
			if (val.Visible && ((int)val.Dock == 1 || (int)val.Dock == 2))
			{
				num -= val.Height;
			}
		}
		return num;
	}

	private int getDockLineWidth(int iStartPos, int iCurrentLine, bool bDirectionForward)
	{
		int num = 0;
		int num2 = 0;
		if (bDirectionForward)
		{
			for (num2 = iStartPos; num2 < ((ArrangedElementCollection)((Control)this).Controls).Count; num2++)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != iCurrentLine)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Width;
				}
			}
		}
		else
		{
			for (num2 = iStartPos; num2 >= 0; num2--)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != iCurrentLine)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Width;
				}
			}
		}
		return num;
	}

	private int getDockLineHeight(int iStartPos, int iCurrentLine, bool bDirectionForward)
	{
		int num = 0;
		int num2 = 0;
		if (bDirectionForward)
		{
			for (num2 = iStartPos; num2 < ((ArrangedElementCollection)((Control)this).Controls).Count; num2++)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != iCurrentLine)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Height;
				}
			}
		}
		else
		{
			for (num2 = iStartPos; num2 >= 0; num2--)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != iCurrentLine)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Height;
				}
			}
		}
		return num;
	}

	public DockSiteInfo GetDockSiteInfo(IDockInfo pDock, int x, int y)
	{
		if (m_DockOrientation == eOrientation.Horizontal)
		{
			return GetDockSiteInfoH(pDock, x, y);
		}
		return GetDockSiteInfoV(pDock, x, y);
	}

	private DockSiteInfo GetDockSiteInfoH(IDockInfo pDock, int x, int y)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Invalid comparison between Unknown and I4
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Invalid comparison between Unknown and I4
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Invalid comparison between Unknown and I4
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Invalid comparison between Unknown and I4
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		DockSiteInfo dockSiteInfo = new DockSiteInfo();
		Rectangle rectangle = new Rectangle(((Control)this).PointToScreen(new Point(0, 0)), ((Control)this).Size);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		Control val = (Control)((pDock is Control) ? pDock : null);
		Size size = pDock.PreferredDockSize(m_DockOrientation);
		Size size2 = pDock.MinimumDockSize(m_DockOrientation);
		if (((Control)this).Height == 0)
		{
			rectangle.Height = 10;
			if ((int)((Control)this).Dock == 2)
			{
				rectangle.Y -= 10;
			}
		}
		else if (((Control)this).Width == 0)
		{
			rectangle.Width = ((Control)this).TopLevelControl.Width;
			if ((int)((Control)this).Dock == 4)
			{
				rectangle.X -= 10;
			}
		}
		rectangle.Inflate(8, 8);
		if (!rectangle.Contains(x, y) || pDock == null)
		{
			return dockSiteInfo;
		}
		bool flag = pDock.DockedSite == this;
		int[] array = new int[255];
		int num = 0;
		int num2 = -10;
		bool flag2 = false;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2.Visible)
			{
				IDockInfo dockInfo = val2 as IDockInfo;
				array[dockInfo.DockLine]++;
				num = dockInfo.DockLine;
			}
		}
		int num3 = -1;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			Control val3 = ((Control)this).Controls[i];
			if (!val3.Visible)
			{
				continue;
			}
			IDockInfo dockInfo2 = val3 as IDockInfo;
			if (dockInfo2.LockDockPosition && val is Bar && !((Bar)(object)val3).CanUndock)
			{
				if ((int)((Control)this).Dock == 1)
				{
					num3 = dockInfo2.DockLine;
				}
				else if (num3 == -1)
				{
					num3 = dockInfo2.DockLine;
				}
			}
			if (pt.Y > val3.Top + 4 && ((pt.Y < val3.Top + size.Height - 4 && size.Height <= val3.Height) || (pt.Y < val3.Bottom - 4 && size.Height > val3.Height)))
			{
				if (dockInfo2.Stretch && val3 != val)
				{
					if (val3 is Bar && val is Bar && ((Bar)(object)val3).LayoutType == eLayoutType.DockContainer && ((Bar)(object)val).LayoutType == eLayoutType.DockContainer && ((Bar)(object)val3).CanTearOffTabs)
					{
						if (val3.Bounds.Contains(pt))
						{
							num2 = dockInfo2.DockLine;
							if (!((Bar)(object)val3).IsPositionOnDockTab(x, y))
							{
								break;
							}
							dockSiteInfo.DockLine = dockInfo2.DockLine;
							dockSiteInfo.DockOffset = dockInfo2.DockOffset;
							dockSiteInfo.InsertPosition = i;
							dockSiteInfo.objDockSite = this;
							dockSiteInfo.DockSide = ((Control)this).Dock;
							dockSiteInfo.TabDockContainer = val3 as Bar;
							return dockSiteInfo;
						}
						continue;
					}
					num2 = dockInfo2.DockLine - 1;
					break;
				}
				if (val3 != val && val is Bar && ((Bar)(object)val).Stretch)
				{
					int insertPosition = i;
					int num4 = i - 1;
					while (num4 >= 0 && ((IDockInfo)((Control)this).Controls[num4]).DockLine == pDock.DockLine)
					{
						insertPosition = num4;
						num4--;
					}
					dockSiteInfo.DockLine = pDock.DockLine;
					dockSiteInfo.DockOffset = pDock.DockOffset;
					dockSiteInfo.InsertPosition = insertPosition;
					dockSiteInfo.objDockSite = this;
					dockSiteInfo.DockSide = ((Control)this).Dock;
					dockSiteInfo.NewLine = true;
					return dockSiteInfo;
				}
				num2 = dockInfo2.DockLine;
				break;
			}
			if (val3 == val && pt.Y <= val3.Top + 4 && pt.Y > val3.Top && array[dockInfo2.DockLine] > 1)
			{
				int insertPosition2 = i;
				int num5 = i - 1;
				while (num5 >= 0 && ((IDockInfo)((Control)this).Controls[num5]).DockLine == pDock.DockLine)
				{
					insertPosition2 = num5;
					num5--;
				}
				dockSiteInfo.DockLine = pDock.DockLine;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition2;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.NewLine = true;
				return dockSiteInfo;
			}
			if ((int)((Control)this).Dock == 1 && val3 == val && pt.Y >= val3.Bottom - 4 && pt.Y < val3.Bottom - 1 && array[dockInfo2.DockLine] == 1 && pDock.DockLine != num)
			{
				int insertPosition3 = i;
				for (int j = i + 1; j < ((ArrangedElementCollection)((Control)this).Controls).Count && ((((IDockInfo)((Control)this).Controls[j]).DockLine == pDock.DockLine + 1 && (((IDockInfo)((Control)this).Controls[j]).DockOffset <= pDock.DockOffset || pDock.Stretch)) || ((IDockInfo)((Control)this).Controls[j]).DockLine == pDock.DockLine); j++)
				{
					insertPosition3 = j;
				}
				dockSiteInfo.DockLine = pDock.DockLine + 1;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition3;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num2 == -10)
		{
			if (pt.Y <= -5 && (!flag || pDock.DockLine != 0 || array[0] != 1))
			{
				num2 = -1;
			}
			else if (!flag)
			{
				num2 = ((pt.Y <= ((Control)this).Height) ? num : (num + 1));
			}
			else if (pt.Y > ((Control)this).Height + 4 && (!flag || pDock.DockLine != num || array[num] != 1))
			{
				num2 = num + 1;
			}
			if (num2 == -10)
			{
				dockSiteInfo.DockLine = pDock.DockLine;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = ((Control)this).Controls.GetChildIndex(val);
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num3 != -1)
		{
			if ((int)((Control)this).Dock == 1 && num2 <= num3)
			{
				num2 = num3 + 1;
			}
			else if ((int)((Control)this).Dock == 2 && num2 >= num3)
			{
				num2 = num3 - 1;
			}
		}
		int num6 = ((ArrangedElementCollection)((Control)this).Controls).Count;
		for (int k = 0; k < ((ArrangedElementCollection)((Control)this).Controls).Count; k++)
		{
			Control val4 = ((Control)this).Controls[k];
			if (val4.Visible)
			{
				IDockInfo dockInfo3 = val4 as IDockInfo;
				if (dockInfo3.DockLine >= num2)
				{
					num6 = k;
					break;
				}
			}
		}
		for (int l = num6; l < ((ArrangedElementCollection)((Control)this).Controls).Count; l++)
		{
			Control val5 = ((Control)this).Controls[l];
			if (!val5.Visible)
			{
				continue;
			}
			IDockInfo dockInfo4 = val5 as IDockInfo;
			if (dockInfo4.DockLine > num2)
			{
				dockSiteInfo.InsertPosition = l;
				if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
				{
					dockSiteInfo.InsertPosition = l - 1;
				}
				if (pt.X + size.Width > ((Control)this).Width)
				{
					dockSiteInfo.DockOffset = ((Control)this).Width - size.Width;
				}
				else
				{
					dockSiteInfo.DockOffset = pt.X;
				}
				dockSiteInfo.DockLine = num2;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.objDockSite = this;
				flag2 = true;
				break;
			}
			if (val5.Bounds.Contains(pt))
			{
				if (val5 != val)
				{
					if (pt.X >= val5.Left && pt.X <= val5.Left + 10)
					{
						dockSiteInfo.InsertPosition = l;
						if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
						{
							dockSiteInfo.InsertPosition = l - 1;
						}
						dockSiteInfo.DockOffset = 0;
						dockSiteInfo.DockLine = num2;
						dockSiteInfo.DockSide = ((Control)this).Dock;
						dockSiteInfo.objDockSite = this;
					}
					else
					{
						int dockLineWidth = getDockLineWidth(l, num2, bDirectionForward: false);
						if (dockLineWidth + size.Width >= ((Control)this).Width || pt.X < dockLineWidth)
						{
							dockSiteInfo.DockOffset = val5.Right;
						}
						else
						{
							dockSiteInfo.DockOffset = pt.X;
						}
						dockSiteInfo.InsertPosition = l + 1;
						if (pDock.Stretch)
						{
							dockSiteInfo.DockOffset = pDock.DockOffset;
						}
						if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
						{
							dockSiteInfo.InsertPosition = l;
						}
						dockSiteInfo.DockLine = num2;
						dockSiteInfo.DockSide = ((Control)this).Dock;
						dockSiteInfo.objDockSite = this;
					}
				}
				else
				{
					dockSiteInfo.InsertPosition = l;
					dockSiteInfo.DockLine = num2;
					dockSiteInfo.DockSide = ((Control)this).Dock;
					dockSiteInfo.objDockSite = this;
					int dockLineWidth2 = getDockLineWidth(l + 1, pDock.DockLine, bDirectionForward: true);
					if (dockLineWidth2 + size.Width + pt.X >= ((Control)this).Width)
					{
						dockSiteInfo.DockOffset = ((Control)this).Width - dockLineWidth2 - size.Width;
					}
					else
					{
						dockSiteInfo.DockOffset = pt.X;
					}
					if (dockSiteInfo.DockOffset < 0)
					{
						dockSiteInfo.DockOffset = 0;
					}
				}
				flag2 = true;
				break;
			}
			if (pt.X < val5.Left || (pt.Y < val5.Top && num3 < 0))
			{
				dockSiteInfo.InsertPosition = l;
				if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
				{
					dockSiteInfo.InsertPosition = l - 1;
				}
				if (pDock.Stretch)
				{
					dockSiteInfo.DockOffset = pDock.DockOffset;
				}
				else
				{
					dockSiteInfo.DockOffset = pt.X;
				}
				dockSiteInfo.DockLine = num2;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.objDockSite = this;
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			if (num2 >= 0)
			{
				if (flag)
				{
					dockSiteInfo.InsertPosition = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
				}
				else
				{
					dockSiteInfo.InsertPosition = ((ArrangedElementCollection)((Control)this).Controls).Count;
				}
			}
			else
			{
				dockSiteInfo.InsertPosition = 0;
			}
			if (pt.X + size.Width > ((Control)this).Width)
			{
				dockSiteInfo.DockOffset = ((Control)this).Width - size.Width;
			}
			else
			{
				dockSiteInfo.DockOffset = pt.X;
			}
			if (num2 >= 0)
			{
				dockSiteInfo.DockLine = num2;
			}
			else
			{
				dockSiteInfo.DockLine = -1;
			}
			dockSiteInfo.DockSide = ((Control)this).Dock;
			dockSiteInfo.objDockSite = this;
		}
		if (dockSiteInfo.DockOffset < 10)
		{
			dockSiteInfo.DockOffset = 0;
		}
		if (m_DockOrientation == eOrientation.Horizontal)
		{
			if (size.Width > ((Control)this).Width)
			{
				dockSiteInfo.DockedWidth = ((Control)this).Width;
			}
			else
			{
				dockSiteInfo.DockedWidth = size.Width;
			}
			dockSiteInfo.DockedHeight = size.Height;
		}
		else
		{
			if (size.Height > ((Control)this).Height)
			{
				dockSiteInfo.DockedHeight = ((Control)this).Height;
			}
			else
			{
				dockSiteInfo.DockedHeight = size.Height;
			}
			dockSiteInfo.DockedWidth = size.Width;
		}
		return dockSiteInfo;
	}

	private DockSiteInfo GetDockSiteInfoV(IDockInfo pDock, int x, int y)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Invalid comparison between Unknown and I4
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Invalid comparison between Unknown and I4
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Invalid comparison between Unknown and I4
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		DockSiteInfo dockSiteInfo = new DockSiteInfo();
		Rectangle rectangle = new Rectangle(((Control)this).PointToScreen(new Point(0, 0)), ((Control)this).Size);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		Control val = (Control)((pDock is Control) ? pDock : null);
		Size size = pDock.PreferredDockSize(m_DockOrientation);
		if (((Control)this).Height == 0)
		{
			rectangle.Height = 10;
			if ((int)((Control)this).Dock == 2)
			{
				rectangle.Y -= 10;
			}
		}
		else if (((Control)this).Width == 0)
		{
			rectangle.Width = 10;
			if ((int)((Control)this).Dock == 4)
			{
				rectangle.X -= 10;
			}
		}
		rectangle.Inflate(8, 8);
		if (!rectangle.Contains(x, y) || pDock == null)
		{
			return dockSiteInfo;
		}
		bool flag = pDock.DockedSite == this;
		int[] array = new int[255];
		int num = 0;
		int num2 = -10;
		bool flag2 = false;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2.Visible)
			{
				IDockInfo dockInfo = val2 as IDockInfo;
				array[dockInfo.DockLine]++;
				num = dockInfo.DockLine;
			}
		}
		int num3 = -1;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			Control val3 = ((Control)this).Controls[i];
			if (!val3.Visible)
			{
				continue;
			}
			IDockInfo dockInfo2 = val3 as IDockInfo;
			if (pt.X > val3.Left + 4 && ((pt.X < val3.Left + size.Width - 4 && size.Width <= val3.Width) || (pt.X < val3.Right - 4 && size.Width > val3.Width)))
			{
				if (dockInfo2.Stretch && val3 != val)
				{
					if (val3 is Bar && val is Bar && ((Bar)(object)val3).LayoutType == eLayoutType.DockContainer && ((Bar)(object)val).LayoutType == eLayoutType.DockContainer && ((Bar)(object)val3).CanTearOffTabs)
					{
						if (val3.Bounds.Contains(pt))
						{
							num2 = dockInfo2.DockLine;
							if (!((Bar)(object)val3).IsPositionOnDockTab(x, y))
							{
								break;
							}
							dockSiteInfo.DockLine = dockInfo2.DockLine;
							dockSiteInfo.DockOffset = dockInfo2.DockOffset;
							dockSiteInfo.InsertPosition = i;
							dockSiteInfo.objDockSite = this;
							dockSiteInfo.DockSide = ((Control)this).Dock;
							dockSiteInfo.TabDockContainer = val3 as Bar;
							return dockSiteInfo;
						}
						continue;
					}
					num2 = dockInfo2.DockLine - 1;
					break;
				}
				num2 = dockInfo2.DockLine;
				break;
			}
			if (val3 == val && pt.X <= val3.Left + 4 && pt.X > val3.Left && array[dockInfo2.DockLine] > 1)
			{
				int insertPosition = i;
				int num4 = i - 1;
				while (num4 >= 0 && ((IDockInfo)((Control)this).Controls[num4]).DockLine == pDock.DockLine)
				{
					insertPosition = num4;
					num4--;
				}
				dockSiteInfo.DockLine = pDock.DockLine;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.NewLine = true;
				return dockSiteInfo;
			}
			if ((int)((Control)this).Dock == 3 && val3 == val && pt.X >= val3.Right - 4 && pt.X < val3.Right && array[dockInfo2.DockLine] == 1 && pDock.DockLine != num)
			{
				int insertPosition2 = i;
				for (int j = i + 1; j < ((ArrangedElementCollection)((Control)this).Controls).Count && ((((IDockInfo)((Control)this).Controls[j]).DockLine == pDock.DockLine + 1 && (((IDockInfo)((Control)this).Controls[j]).DockOffset <= pDock.DockOffset || pDock.Stretch)) || ((IDockInfo)((Control)this).Controls[j]).DockLine == pDock.DockLine); j++)
				{
					insertPosition2 = j;
				}
				dockSiteInfo.DockLine = pDock.DockLine + 1;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition2;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num2 == -10)
		{
			if (pt.X <= -5 && (!flag || pDock.DockLine != 0 || array[0] != 1))
			{
				num2 = -1;
			}
			else if (!flag)
			{
				num2 = ((pt.X <= ((Control)this).Width) ? num : (num + 1));
			}
			else if (pt.X > ((Control)this).Width + 4 && (!flag || pDock.DockLine != num || array[num] != 1))
			{
				num2 = num + 1;
			}
			if (num2 == -10)
			{
				dockSiteInfo.DockLine = pDock.DockLine;
				dockSiteInfo.DockOffset = pDock.DockOffset;
				dockSiteInfo.InsertPosition = ((Control)this).Controls.GetChildIndex(val);
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num3 != -1)
		{
			if ((int)((Control)this).Dock == 1 && num2 <= num3)
			{
				num2 = num3 + 1;
			}
			else if ((int)((Control)this).Dock == 2 && num2 >= num3)
			{
				num2 = num3 - 1;
			}
		}
		int num5 = ((ArrangedElementCollection)((Control)this).Controls).Count;
		for (int k = 0; k < ((ArrangedElementCollection)((Control)this).Controls).Count; k++)
		{
			Control val4 = ((Control)this).Controls[k];
			if (val4.Visible)
			{
				IDockInfo dockInfo3 = val4 as IDockInfo;
				if (dockInfo3.DockLine >= num2)
				{
					num5 = k;
					break;
				}
			}
		}
		for (int l = num5; l < ((ArrangedElementCollection)((Control)this).Controls).Count; l++)
		{
			Control val5 = ((Control)this).Controls[l];
			if (!val5.Visible)
			{
				continue;
			}
			IDockInfo dockInfo4 = val5 as IDockInfo;
			if (dockInfo4.DockLine > num2)
			{
				dockSiteInfo.InsertPosition = l;
				if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
				{
					dockSiteInfo.InsertPosition = l - 1;
				}
				if (pt.Y + size.Height > ((Control)this).Height)
				{
					dockSiteInfo.DockOffset = ((Control)this).Height - size.Height;
				}
				else
				{
					dockSiteInfo.DockOffset = pt.Y;
				}
				dockSiteInfo.DockLine = num2;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.objDockSite = this;
				flag2 = true;
				break;
			}
			if (val5.Bounds.Contains(pt))
			{
				if (val5 != val)
				{
					if (pt.Y >= val5.Top && pt.Y <= val5.Top + 10)
					{
						dockSiteInfo.InsertPosition = l;
						if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
						{
							dockSiteInfo.InsertPosition = l - 1;
						}
						dockSiteInfo.DockOffset = 0;
						dockSiteInfo.DockLine = num2;
						dockSiteInfo.DockSide = ((Control)this).Dock;
						dockSiteInfo.objDockSite = this;
					}
					else
					{
						int dockLineHeight = getDockLineHeight(l, num2, bDirectionForward: false);
						if (dockLineHeight + size.Height >= ((Control)this).Height || pt.Y < dockLineHeight)
						{
							dockSiteInfo.DockOffset = val5.Bottom;
						}
						else
						{
							dockSiteInfo.DockOffset = pt.Y;
						}
						dockSiteInfo.InsertPosition = l + 1;
						if (pDock.Stretch)
						{
							dockSiteInfo.DockOffset = pDock.DockOffset;
						}
						if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
						{
							dockSiteInfo.InsertPosition = l;
						}
						dockSiteInfo.DockLine = num2;
						dockSiteInfo.DockSide = ((Control)this).Dock;
						dockSiteInfo.objDockSite = this;
					}
				}
				else
				{
					dockSiteInfo.InsertPosition = l;
					dockSiteInfo.DockLine = num2;
					dockSiteInfo.DockSide = ((Control)this).Dock;
					dockSiteInfo.objDockSite = this;
					int dockLineHeight2 = getDockLineHeight(l + 1, pDock.DockLine, bDirectionForward: true);
					if (dockLineHeight2 + size.Height + pt.Y >= ((Control)this).Height)
					{
						dockSiteInfo.DockOffset = ((Control)this).Height - dockLineHeight2 - size.Height;
					}
					else
					{
						dockSiteInfo.DockOffset = pt.Y;
					}
					if (dockSiteInfo.DockOffset < 0)
					{
						dockSiteInfo.DockOffset = 0;
					}
				}
				flag2 = true;
				break;
			}
			if (pt.Y < val5.Top || (pt.X < val5.Left && num3 < 0))
			{
				dockSiteInfo.InsertPosition = l;
				if (flag && l > 0 && ((Control)this).Controls.GetChildIndex(val) < l)
				{
					dockSiteInfo.InsertPosition = l - 1;
				}
				if (pDock.Stretch)
				{
					dockSiteInfo.DockOffset = pDock.DockOffset;
				}
				else
				{
					dockSiteInfo.DockOffset = pt.Y;
				}
				dockSiteInfo.DockLine = num2;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.objDockSite = this;
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			if (num2 >= 0)
			{
				if (flag)
				{
					dockSiteInfo.InsertPosition = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
				}
				else
				{
					dockSiteInfo.InsertPosition = ((ArrangedElementCollection)((Control)this).Controls).Count;
				}
			}
			else
			{
				dockSiteInfo.InsertPosition = 0;
			}
			if (pt.Y + size.Height > ((Control)this).Height)
			{
				dockSiteInfo.DockOffset = ((Control)this).Height - size.Height;
			}
			else
			{
				dockSiteInfo.DockOffset = pt.Y;
			}
			if (num2 >= 0)
			{
				dockSiteInfo.DockLine = num2;
			}
			else
			{
				dockSiteInfo.DockLine = -1;
			}
			dockSiteInfo.DockSide = ((Control)this).Dock;
			dockSiteInfo.objDockSite = this;
		}
		if (dockSiteInfo.DockOffset < 10)
		{
			dockSiteInfo.DockOffset = 0;
		}
		if (m_DockOrientation == eOrientation.Horizontal)
		{
			if (size.Width > ((Control)this).Width)
			{
				dockSiteInfo.DockedWidth = ((Control)this).Width;
			}
			else
			{
				dockSiteInfo.DockedWidth = size.Width;
			}
			dockSiteInfo.DockedHeight = size.Height;
		}
		else
		{
			if (size.Height > ((Control)this).Height)
			{
				dockSiteInfo.DockedHeight = ((Control)this).Height;
			}
			else
			{
				dockSiteInfo.DockedHeight = size.Height;
			}
			dockSiteInfo.DockedWidth = size.Width;
		}
		return dockSiteInfo;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Rectangle GetBarDockRectangle(Bar bar, ref DockSiteInfo pDockInfo)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected I4, but got Unknown
		//IL_0a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Invalid comparison between Unknown and I4
		if (pDockInfo.objDockSite == null)
		{
			return Rectangle.Empty;
		}
		if ((int)((Control)pDockInfo.objDockSite).Dock == 5 || m_DocumentDockContainer != null)
		{
			DocumentDockUIManager documentUIManager = GetDocumentUIManager();
			return documentUIManager.GetDockPreviewRectangle(bar, ref pDockInfo);
		}
		Rectangle result = Rectangle.Empty;
		if (pDockInfo.InsertPosition < 0 && pDockInfo.NewLine)
		{
			pDockInfo.DockLine = -1;
		}
		else if (pDockInfo.InsertPosition >= ((ArrangedElementCollection)((Control)this).Controls).Count && pDockInfo.NewLine)
		{
			pDockInfo.DockLine = 999;
		}
		if (pDockInfo.TabDockContainer != null)
		{
			result = ((Control)pDockInfo.TabDockContainer).Bounds;
			result.Location = ((Control)this).PointToScreen(result.Location);
		}
		else if (pDockInfo.DockLine == 999 || pDockInfo.DockLine == -1)
		{
			int num = -1;
			int num2 = -1;
			if (pDockInfo.FullSizeDock)
			{
				num = GetFullSizeIndex();
			}
			else if (pDockInfo.PartialSizeDock)
			{
				num2 = GetPartialSizeIndex();
			}
			DockStyle dock = ((Control)this).Dock;
			switch (dock - 1)
			{
			case 0:
			{
				result.Width = ((Control)this).ClientRectangle.Width;
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
				if (num >= 0)
				{
					result.Width += GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: true).Width;
					result.Width += GetSiteZOrderSize(m_Owner.RightDockSite, bFullSize: true).Width;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Width -= GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: false).Width;
					result.Width -= GetSiteZOrderSize(m_Owner.RightDockSite, bFullSize: false).Width;
					pDockInfo.DockSiteZOrderIndex = num2;
				}
				Point empty3 = Point.Empty;
				if (pDockInfo.DockLine == -1)
				{
					empty3 = ((Control)this).PointToScreen(((Control)this).ClientRectangle.Location);
				}
				else
				{
					empty3 = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom));
					empty3.Y++;
				}
				if (num >= 0)
				{
					empty3.X -= GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: true).Width;
				}
				else if (num2 >= 0)
				{
					empty3.X += GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: false).Width;
				}
				result.Location = empty3;
				break;
			}
			case 1:
			{
				result.Width = ((Control)this).ClientRectangle.Width;
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
				if (num >= 0)
				{
					result.Width += GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: true).Width;
					result.Width += GetSiteZOrderSize(m_Owner.RightDockSite, bFullSize: true).Width;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Width -= GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: false).Width;
					result.Width -= GetSiteZOrderSize(m_Owner.RightDockSite, bFullSize: false).Width;
					pDockInfo.DockSiteZOrderIndex = num2;
				}
				Point empty2 = Point.Empty;
				if (pDockInfo.DockLine == -1)
				{
					empty2 = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y - result.Height));
				}
				else
				{
					empty2 = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - result.Height));
					empty2.Y++;
				}
				if (num >= 0)
				{
					empty2.X -= GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: true).Width;
				}
				else if (num2 >= 0)
				{
					empty2.X += GetSiteZOrderSize(m_Owner.LeftDockSite, bFullSize: false).Width;
				}
				result.Location = empty2;
				break;
			}
			case 3:
			{
				result.Height = ((Control)this).ClientRectangle.Height;
				if (num >= 0)
				{
					result.Height += GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: true).Height;
					result.Height += GetSiteZOrderSize(m_Owner.BottomDockSite, bFullSize: true).Height;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Height -= GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: false).Height;
					result.Height -= GetSiteZOrderSize(m_Owner.BottomDockSite, bFullSize: false).Height;
					pDockInfo.DockSiteZOrderIndex = num2;
				}
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
				Point empty4 = Point.Empty;
				if (pDockInfo.DockLine == -1)
				{
					empty4 = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.X - result.Width, ((Control)this).ClientRectangle.Y));
				}
				else
				{
					empty4 = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.Right - result.Width, ((Control)this).ClientRectangle.Y));
					empty4.X--;
				}
				if (num >= 0)
				{
					empty4.Y -= GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: true).Height;
				}
				else if (num2 >= 0)
				{
					empty4.Y += GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: false).Height;
				}
				result.Location = empty4;
				break;
			}
			default:
			{
				result.Height = ((Control)this).ClientRectangle.Height;
				if (num >= 0)
				{
					result.Height += GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: true).Height;
					result.Height += GetSiteZOrderSize(m_Owner.BottomDockSite, bFullSize: true).Height;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Height -= GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: false).Height;
					result.Height -= GetSiteZOrderSize(m_Owner.BottomDockSite, bFullSize: false).Height;
					pDockInfo.DockSiteZOrderIndex = num2;
				}
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
				Point empty = Point.Empty;
				if (pDockInfo.DockLine == -1)
				{
					empty = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y));
				}
				else
				{
					empty = ((Control)this).PointToScreen(new Point(((Control)this).ClientRectangle.Right, ((Control)this).ClientRectangle.Y));
					empty.X++;
				}
				if (num >= 0)
				{
					empty.Y -= GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: true).Height;
				}
				else if (num2 >= 0)
				{
					empty.Y += GetSiteZOrderSize(m_Owner.TopDockSite, bFullSize: false).Height;
				}
				result.Location = empty;
				break;
			}
			}
		}
		else
		{
			Bar bar2 = null;
			if (pDockInfo.InsertPosition >= 0 && pDockInfo.InsertPosition < ((ArrangedElementCollection)((Control)this).Controls).Count)
			{
				bar2 = ((Control)this).Controls[pDockInfo.InsertPosition] as Bar;
				if (!pDockInfo.NewLine)
				{
					if (bar2.DockLine > pDockInfo.DockLine)
					{
						int num3 = pDockInfo.InsertPosition - 1;
						while (num3 >= 0)
						{
							if (((Control)this).Controls[num3] is Bar { LayoutType: eLayoutType.DockContainer } bar3 && bar3.DockLine == pDockInfo.DockLine)
							{
								bar2 = bar3;
								break;
							}
						}
					}
					else if (bar2.DockLine < pDockInfo.DockLine)
					{
						int num4 = pDockInfo.InsertPosition + 1;
						while (num4 < ((ArrangedElementCollection)((Control)this).Controls).Count)
						{
							if (((Control)this).Controls[num4] is Bar { LayoutType: eLayoutType.DockContainer } bar4 && bar4.DockLine == pDockInfo.DockLine)
							{
								bar2 = bar4;
								break;
							}
						}
					}
				}
			}
			else if (pDockInfo.InsertPosition < 0)
			{
				bar2 = ((Control)this).Controls[0] as Bar;
			}
			else if (pDockInfo.InsertPosition >= ((ArrangedElementCollection)((Control)this).Controls).Count)
			{
				bar2 = ((Control)this).Controls[((ArrangedElementCollection)((Control)this).Controls).Count - 1] as Bar;
			}
			for (int i = pDockInfo.InsertPosition + 1; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (bar != null && bar2.LayoutType == eLayoutType.DockContainer && bar2.Visible)
				{
					break;
				}
				bar2 = ((Control)this).Controls[i] as Bar;
			}
			if (bar2 == null)
			{
				return result;
			}
			DockStyle dock2 = ((Control)this).Dock;
			if (dock2 - 1 <= 1)
			{
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
				if (pDockInfo.NewLine || bar2 == bar)
				{
					result.Width = ((Control)bar2).Width;
					if (result.Width == 0)
					{
						result.Width = ((Control)this).Width;
					}
					Point location = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location;
				}
				else if (bar2.DockOffset > pDockInfo.DockOffset)
				{
					result.Height = ((Control)bar2).Height;
					result.Width = ((Control)bar2).Width / 2;
					Point location2 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location2;
				}
				else
				{
					result.Height = ((Control)bar2).Height;
					result.Width = ((Control)bar2).Width / 2;
					Point location3 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left + result.Width, ((Control)bar2).Top));
					result.Location = location3;
				}
			}
			else
			{
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
				if (pDockInfo.NewLine || bar2 == bar)
				{
					result.Height = ((Control)bar2).Height;
					Point location4 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location4;
				}
				else if (bar2.DockOffset < pDockInfo.DockOffset)
				{
					result.Width = ((Control)bar2).Width;
					result.Height = ((Control)bar2).Height / 2;
					Point location5 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top + result.Height));
					result.Location = location5;
				}
				else
				{
					result.Width = ((Control)bar2).Width;
					result.Height = ((Control)bar2).Height / 2;
					Point location6 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location6;
				}
			}
		}
		return result;
	}

	internal Size GetSiteZOrderSize(DockSite dockSite, bool bFullSize)
	{
		if (dockSite == null || ((Control)dockSite).Parent == null)
		{
			return Size.Empty;
		}
		Size result = Size.Empty;
		if (bFullSize)
		{
			if (((Control)dockSite).Parent.Controls.IndexOf((Control)(object)dockSite) > ((Control)this).Parent.Controls.IndexOf((Control)(object)this))
			{
				result = ((Control)dockSite).Size;
			}
		}
		else if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) > ((Control)dockSite).Parent.Controls.IndexOf((Control)(object)dockSite))
		{
			result = ((Control)dockSite).Size;
		}
		return result;
	}

	internal int GetPartialSizeIndex()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (((Control)this).Parent == null)
		{
			return -1;
		}
		int num = 1;
		if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
		{
			if (m_Owner == null || m_Owner.TopDockSite == null || m_Owner.BottomDockSite == null || ((Control)m_Owner.TopDockSite).Parent == null || ((Control)m_Owner.BottomDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Min(((Control)m_Owner.TopDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.TopDockSite), ((Control)m_Owner.BottomDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.BottomDockSite));
		}
		else
		{
			if (m_Owner == null || m_Owner.LeftDockSite == null || m_Owner.RightDockSite == null || ((Control)m_Owner.LeftDockSite).Parent == null || ((Control)m_Owner.RightDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Min(((Control)m_Owner.LeftDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.LeftDockSite), ((Control)m_Owner.RightDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.RightDockSite));
		}
		if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) < num)
		{
			return -1;
		}
		return num;
	}

	internal int GetFullSizeIndex()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (((Control)this).Parent == null)
		{
			return -1;
		}
		int num = -1;
		if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
		{
			if (m_Owner == null || m_Owner.TopDockSite == null || m_Owner.BottomDockSite == null || ((Control)m_Owner.TopDockSite).Parent == null || ((Control)m_Owner.BottomDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Max(((Control)m_Owner.TopDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.TopDockSite), ((Control)m_Owner.BottomDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.BottomDockSite));
		}
		else
		{
			if (m_Owner == null || m_Owner.LeftDockSite == null || m_Owner.RightDockSite == null || ((Control)m_Owner.LeftDockSite).Parent == null || ((Control)m_Owner.RightDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Max(((Control)m_Owner.LeftDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.LeftDockSite), ((Control)m_Owner.RightDockSite).Parent.Controls.IndexOf((Control)(object)m_Owner.RightDockSite));
		}
		if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) > num)
		{
			return -1;
		}
		return num;
	}

	internal void RemoveBar(Control objBar)
	{
		if (((Control)this).Contains(objBar))
		{
			((Control)this).Controls.Remove(objBar);
			LayoutBars();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (m_DocumentDockContainer != null)
		{
			m_DocumentUIManager.OnMouseMove(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (m_DocumentDockContainer != null)
		{
			m_DocumentUIManager.OnMouseLeave();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button == 2097152 && m_DocumentDockContainer == null)
		{
			((IOwnerBarSupport)m_Owner)?.BarContextMenu((Control)(object)this, e);
		}
		if (m_DocumentDockContainer != null)
		{
			m_DocumentUIManager.OnMouseDown(e);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (m_DocumentDockContainer != null)
		{
			m_DocumentUIManager.OnMouseUp(e);
		}
	}

	private void UpdateBarsCollection()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (m_Owner == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is Bar)
			{
				Bar bar = val as Bar;
				if (!m_Owner.Bars.Contains(bar))
				{
					m_Owner.Bars.Add(bar);
				}
			}
		}
	}

	protected override void OnDockChanged(EventArgs e)
	{
		((Control)this).OnDockChanged(e);
		if (IsDocumentDock)
		{
			UpdateBarsCollection();
		}
	}

	internal void SetOwner(DotNetBarManager owner)
	{
		m_Owner = owner;
		UpdateBarsCollection();
	}
}
