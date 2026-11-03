using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.DockSiteDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class DockSite : Control
{
	public class DockSiteAccessibleObject : ControlAccessibleObject
	{
		private DockSite _0AC3;

		public override AccessibleRole Role
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
				{
					return (AccessibleRole)0;
				}
				return ((Control)_0AC3).AccessibleRole;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (_0AC3 != null && !((Control)_0AC3).IsDisposed && ((Control)_0AC3).Parent != null)
				{
					return ((Control)_0AC3).Parent.RectangleToScreen(((Control)_0AC3).Bounds);
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
				if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				return (AccessibleStates)0;
			}
		}

		public DockSiteAccessibleObject(DockSite owner)
			: base((Control)(object)owner)
		{
			_0AC3 = owner;
		}

		public override int GetChildCount()
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed)
			{
				return ((ArrangedElementCollection)((Control)_0AC3).Controls).Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed && ((ArrangedElementCollection)((Control)_0AC3).Controls).Count > 0 && ((ArrangedElementCollection)((Control)_0AC3).Controls).Count > iIndex)
			{
				return ((Control)_0AC3).Controls[iIndex].AccessibilityObject;
			}
			return null;
		}
	}

	private eOrientation ၕ;

	private bool _1056;

	private bool _1057;

	private DotNetBarManager _0AC3;

	private bool _1058;

	private bool _1059;

	private _0A7A ৱ;

	private eBackgroundImagePosition ސ;

	private byte ޑ = byte.MaxValue;

	private Color ތ = Color.Empty;

	private int ގ = 90;

	private bool ป;

	private DocumentDockUIManager ၚ;

	private bool ஹ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool _DesignerLoading;

	private int ၛ;

	private bool ၜ = true;

	private DocumentDockContainer ၝ;

	[DefaultValue(true)]
	[Description("Indicates whether painting is disabled on dock site while layout of bars is performed.")]
	public bool OptimizeLayoutRedraw
	{
		get
		{
			return ၜ;
		}
		set
		{
			ၜ = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DocumentDockContainer DocumentDockContainer
	{
		get
		{
			return ၝ;
		}
		set
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			ၝ = value;
			ၚ = GetDocumentUIManager();
			ၚ.RootDocumentDockContainer = ၝ;
			if ((int)((Control)this).Dock != 5 && value != null)
			{
				ၝ.RecordDocumentSize = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies background image position when container is larger than image.")]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return ސ;
		}
		set
		{
			if (ސ != value)
			{
				ސ = value;
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(byte.MaxValue)]
	[Description("Specifies the transparency of background image.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public byte BackgroundImageAlpha
	{
		get
		{
			return ޑ;
		}
		set
		{
			if (ޑ != value)
			{
				ޑ = value;
				((Control)this).Refresh();
			}
		}
	}

	[Description("Indicates the target gradient background color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return ތ;
		}
		set
		{
			ތ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(90)]
	[Description("Indicates gradient fill angle.")]
	[Category("Style")]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			if (value != ގ)
			{
				ގ = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	private bool _104F
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((Control)this).Dock != 5)
			{
				return ၝ == null;
			}
			return false;
		}
	}

	private _0A7A _0A7A
	{
		get
		{
			if (ৱ == null)
			{
				ৱ = new _0A7A((Control)(object)this);
			}
			return ৱ;
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
				ၕ = eOrientation.Horizontal;
			}
			else
			{
				if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
				{
					((Control)this).Width = 0;
				}
				ၕ = eOrientation.Vertical;
			}
		}
	}

	internal bool ၐ
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
					if (((Bar)(object)val).ଇ)
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool NeedsLayout
	{
		get
		{
			return _1058;
		}
		set
		{
			_1058 = value;
		}
	}

	private bool ၑ
	{
		get
		{
			if ((_0AC3 != null && _0AC3.SuspendLayout) || ஹ || _DesignerLoading)
			{
				return true;
			}
			return false;
		}
	}

	internal int ၒ
	{
		get
		{
			return ၛ;
		}
		set
		{
			ၛ = value;
		}
	}

	internal eOrientation ಅ => ၕ;

	internal bool ၓ
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((Control)this).Dock != 5)
			{
				return ၝ != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public DotNetBarManager Owner => _0AC3;

	internal bool ၔ
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
		_0AC3 = null;
		ป = true;
		((Control)this).Dispose(disposing);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((Control)this).ScaleControl(factor, specified);
	}

	public DocumentDockUIManager GetDocumentUIManager()
	{
		if (ၚ == null)
		{
			ၚ = new DocumentDockUIManager(this);
			ၚ.RootDocumentDockContainer = ၝ;
		}
		return ၚ;
	}

	public void SaveLayout(XmlElement xmlBars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (ၓ || ၝ != null)
		{
			GetDocumentUIManager().ఫ(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is Bar bar && bar.Name != "" && bar.SaveLayoutChanges)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.ఫ(xmlElement);
			}
		}
	}

	public void LoadLayout(XmlElement xmlBars)
	{
		ஹ = true;
		try
		{
			if (!ၓ && ၝ == null)
			{
				foreach (XmlElement childNode in xmlBars.ChildNodes)
				{
					Bar bar = null;
					if (_0AC3.Bars.Contains(_0AC3.Bars[childNode.GetAttribute("name")]))
					{
						bar = _0AC3.Bars[childNode.GetAttribute("name")];
					}
					else
					{
						bar = new Bar();
						_0AC3.Bars.Add(bar);
					}
					bar.బ(childNode);
				}
				return;
			}
			GetDocumentUIManager().బ((XmlElement)xmlBars.FirstChild);
		}
		finally
		{
			ஹ = false;
			RecalcLayout();
		}
	}

	public void SuspendLayout()
	{
		ஹ = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout()
	{
		ResumeLayout(performLayout: true);
	}

	public void ResumeLayout(bool performLayout)
	{
		ஹ = false;
		((Control)this).ResumeLayout(performLayout);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackgroundImageAlpha()
	{
		return ޑ != byte.MaxValue;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !ތ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBackColor2()
	{
		ތ = Color.Empty;
	}

	internal bool ఛ()
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
		if (ၑ || (((Control)this).BackgroundImage != null && BackgroundImageAlpha == byte.MaxValue && (BackgroundImagePosition == eBackgroundImagePosition.Stretch || BackgroundImagePosition == eBackgroundImagePosition.Tile || (((Control)this).BackgroundImage.Width >= ((Control)this).Width && ((Control)this).BackgroundImage.Height >= ((Control)this).Height))))
		{
			return;
		}
		if (!ތ.IsEmpty && !((Control)this).BackColor.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			DisplayHelp.FillRectangle(pevent.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor, BackColor2, ގ);
			return;
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar && ((Bar)(object)((Control)this).Controls[0])._0AFE)
		{
			_0A7A obj = _0A7A;
			obj._0EE5(pevent.Graphics, ཤ.ཥ, ཫ.ཊ, ((Control)this).ClientRectangle);
			return;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme != null && ((Control)this).BackColor != Color.Transparent)
		{
			if (_104F && StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				DisplayHelp.FillRectangle(pevent.Graphics, ((Control)this).ClientRectangle, colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
			}
			else
			{
				DisplayHelp.FillRectangle(pevent.Graphics, ((Control)this).ClientRectangle, colorScheme.DockSiteBackColor, colorScheme.DockSiteBackColor2, colorScheme.DockSiteBackColorGradientAngle);
			}
		}
		else
		{
			((Control)this).OnPaintBackground(pevent);
		}
	}

	private ColorScheme GetColorScheme()
	{
		if (_0AC3 != null && _0AC3.UseGlobalColorScheme)
		{
			if ((_0AC3.Style == eDotNetBarStyle.Office2007 || _0AC3.Style == eDotNetBarStyle.StyleManagerControlled) && GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return _0AC3.ColorScheme;
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
		if (!ၑ)
		{
			if (((Control)this).BackgroundImage != null)
			{
				BarFunctions._0982(pevent.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackgroundImage, ސ, ޑ);
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
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = null;
		}
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			ৱ = null;
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
		ၺ();
	}

	internal void _105E(Control _105F)
	{
		if (!(_105F is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Visible && (Owner == null || (Owner != null && !Owner.IsLoadingDefinition)))
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		dockInfo.DockOrientation = ၕ;
		Control val = null;
		IDockInfo dockInfo2 = null;
		bool flag = false;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			val = ((Control)this).Controls[i];
			if (val is IDockInfo dockInfo3 && ((dockInfo.DockLine <= dockInfo3.DockLine && dockInfo.DockOffset < dockInfo3.DockOffset) || dockInfo.DockLine < dockInfo3.DockLine))
			{
				((Control)this).Controls.Add(_105F);
				((Control)this).Controls.SetChildIndex(_105F, i);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			((Control)this).Controls.Add(_105F);
		}
		if (_105F is Bar && ((Bar)(object)_105F).LayoutType == eLayoutType.DockContainer)
		{
			Bar bar = _105F as Bar;
			if (ၕ == eOrientation.Horizontal && bar.ItemsContainer._0B03 > 0)
			{
				((Bar)(object)_105F).అ();
			}
			else if (ၕ == eOrientation.Vertical && bar.ItemsContainer.ර > 0)
			{
				((Bar)(object)_105F)._0C04();
			}
		}
		ၥ();
		_1067();
	}

	internal void _105E(Control _105F, int _1060)
	{
		if (!(_105F is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Visible)
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		dockInfo.DockOrientation = ၕ;
		((Control)this).Controls.Add(_105F);
		((Control)this).Controls.SetChildIndex(_105F, _1060);
		if (_105F is Bar && ((Bar)(object)_105F).LayoutType == eLayoutType.DockContainer)
		{
			Bar bar = _105F as Bar;
			if (ၕ == eOrientation.Horizontal && bar.ItemsContainer._0B03 > 0)
			{
				((Bar)(object)_105F).అ();
			}
			else if (ၕ == eOrientation.Vertical && bar.ItemsContainer.ර > 0)
			{
				((Bar)(object)_105F)._0C04();
			}
		}
		ၥ();
		_1067();
	}

	internal void ၡ(Control _105F, int _1062)
	{
		if (((Control)this).Contains(_105F))
		{
			if (((Control)this).Controls.GetChildIndex(_105F) != _1062)
			{
				((Control)this).Controls.SetChildIndex(_105F, _1062);
			}
			_1067();
		}
	}

	internal void ၡ(Control _105F, int _1062, bool _1063)
	{
		if (!((Control)this).Contains(_105F))
		{
			return;
		}
		if (((Control)this).Controls.GetChildIndex(_105F) != _1062)
		{
			((Control)this).Controls.SetChildIndex(_105F, _1062);
		}
		if (_1063)
		{
			_1062 = ((Control)this).Controls.IndexOf(_105F);
			if (_1062 >= 0)
			{
				if (_1062 > 0)
				{
					((IDockInfo)_105F).SetDockLine(((IDockInfo)((Control)this).Controls[_1062 - 1]).DockLine + 1);
				}
				else
				{
					((IDockInfo)_105F).SetDockLine(-1);
				}
				for (int i = _1062 + 1; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
				{
					((IDockInfo)((Control)this).Controls[i]).SetDockLine(((IDockInfo)((Control)this).Controls[i]).DockLine + 2);
				}
			}
		}
		_1067();
	}

	internal void _1064(Control _105F)
	{
		if (!(_105F is IDockInfo dockInfo))
		{
			throw new ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
		}
		if (!((Control)this).Controls.Contains(_105F))
		{
			throw new ArgumentException("Control is not part of this collection.");
		}
		Control val = null;
		IDockInfo dockInfo2 = null;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			val = ((Control)this).Controls[i];
			if (val == _105F || !(val is IDockInfo dockInfo3))
			{
				continue;
			}
			if (dockInfo.DockLine == dockInfo3.DockLine && dockInfo.DockOffset <= dockInfo3.DockOffset)
			{
				if (i > 0 && i > ((Control)this).Controls.GetChildIndex(_105F))
				{
					i--;
				}
				((Control)this).Controls.SetChildIndex(_105F, i);
				return;
			}
			if (dockInfo.DockLine < dockInfo3.DockLine)
			{
				if (i > 0)
				{
					i--;
				}
				((Control)this).Controls.SetChildIndex(_105F, i);
				return;
			}
		}
		((Control)this).Controls.SetChildIndex(_105F, ((ArrangedElementCollection)((Control)this).Controls).Count);
	}

	public void RecalcLayout()
	{
		_1067();
	}

	private void ၥ()
	{
		if (ၑ)
		{
			_1059 = true;
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
		_1059 = false;
	}

	private Rectangle ၦ()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if ((int)((Control)this).Dock == 5 || ၝ == null)
		{
			return clientRectangle;
		}
		int num = Dpi.Width(ၝ.SplitterSize);
		if ((int)((Control)this).Dock == 3)
		{
			clientRectangle.Width -= num;
		}
		else if ((int)((Control)this).Dock == 4)
		{
			clientRectangle.Width -= num;
			clientRectangle.X += num;
		}
		else if ((int)((Control)this).Dock == 2)
		{
			clientRectangle.Height -= num;
			clientRectangle.Y += num;
		}
		else if ((int)((Control)this).Dock == 1)
		{
			clientRectangle.Height -= num;
		}
		return clientRectangle;
	}

	private void _1067()
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
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Invalid comparison between Unknown and I4
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Invalid comparison between Unknown and I4
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Invalid comparison between Unknown and I4
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Invalid comparison between Unknown and I4
		//IL_1015: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Invalid comparison between Unknown and I4
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Invalid comparison between Unknown and I4
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Invalid comparison between Unknown and I4
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Invalid comparison between Unknown and I4
		if (ป || ((Control)this).IsDisposed)
		{
			return;
		}
		if (ၑ)
		{
			_1058 = true;
			return;
		}
		if (_1059)
		{
			ၥ();
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
		_1058 = false;
		if (_1056)
		{
			return;
		}
		_1056 = true;
		int num = -1;
		if ((int)((Control)this).Dock == 5 || ၝ != null)
		{
			Rectangle bounds = ((Control)this).Bounds;
			if (((Control)this).Parent != null && ((Control)this).Parent.Visible && ၜ)
			{
				if (((Control)this).Parent != null)
				{
					num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
				}
				_089F.SendMessage(((Control)this).Handle, 11, 0, 0);
			}
			if (ၝ != null)
			{
				if (_0AC3 != null)
				{
					ၝ.SplitterSize = _0AC3.DockSplitterSize;
					GetDocumentUIManager().SplitterWidth = _0AC3.DockSplitterSize;
				}
				if ((int)((Control)this).Dock != 5)
				{
					ၝ.RecordDocumentSize = true;
				}
				ၝ.Layout(ၦ());
				if ((int)((Control)this).Dock != 5 && ၝ.DisplayBounds.IsEmpty)
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
			_1056 = false;
			if (((Control)this).Parent != null && ((Control)this).Parent.Visible && ၜ)
			{
				_089F.SendMessage(((Control)this).Handle, 11, 1, 0);
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
		if (((Control)this).Parent != null && ((Control)this).Parent.Visible && ၜ)
		{
			if (((Control)this).Parent != null)
			{
				num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
			}
			_089F.SendMessage(((Control)this).Handle, 11, 0, 0);
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
			if (ၕ == eOrientation.Horizontal)
			{
				if (bar._0B02 == 0)
				{
					array5[bar.DockLine]++;
				}
				else
				{
					array4[bar.DockLine] += bar._0B02;
				}
			}
			else if (bar._0B01 == 0)
			{
				array5[bar.DockLine]++;
			}
			else
			{
				array4[bar.DockLine] += bar._0B01;
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
				if (ၕ == eOrientation.Horizontal)
				{
					if (flag5)
					{
						num6 += num4 + num3;
						num5 = 0;
						num4 = 0;
						flag5 = false;
					}
					Size _௦ = Size.Empty;
					Size size = ((bar.LayoutType != 0) ? new Size(1, 1) : bar._౮(dockInfo.MinimumDockSize(ၕ)));
					if (bar.Stretch || (j > 0 && ((num5 + val2.Width > ((Control)this).Width && size.Width + num5 > ((Control)this).Width) || dockInfo.DockLine > num8)))
					{
						if (bar.Stretch && bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array[bar.DockLine] > 0)
						{
							if (array[bar.DockLine] == 1)
							{
								bar._0B02 = 0;
							}
							_௦ = ((bar._0B02 != 0 && (array2[bar.DockLine] != 1 || array4[bar.DockLine] == ((Control)this).Width)) ? new Size(bar._0B02, 1) : ((array2[bar.DockLine] != 1) ? new Size((((Control)this).Width - array4[bar.DockLine]) / ((array5[bar.DockLine] == 0) ? 1 : array5[bar.DockLine]), 1) : new Size(((Control)this).Width - num5, 1)));
							if (num8 != dockInfo.DockLine && array2[bar.DockLine] == array[bar.DockLine] && num5 > 0)
							{
								num8 = dockInfo.DockLine;
								num7++;
								num6 += num4 + num3;
								num5 = 0;
								num4 = 0;
								if (array[bar.DockLine] == 1 || _௦.IsEmpty)
								{
									_௦ = new Size(((Control)this).Width, 1);
								}
							}
							array2[bar.DockLine]--;
							flag2 = true;
							arrayList.Add(bar);
						}
						else
						{
							_௦ = new Size(((Control)this).Width, 1);
						}
					}
					else
					{
						_௦ = new Size(((Control)this).Width - num5, 1);
					}
					bar._0BE5(_௦);
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
							bar._0BE5(new Size(((Control)this).Width - num5, ((Control)bar).Height));
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
					Size _௦2 = Size.Empty;
					Size size = new Size(1, 1);
					if (bar.Stretch || (j > 0 && ((num6 + val2.Height > ((Control)this).Height && size.Height + num6 > ((Control)this).Height) || dockInfo.DockLine > num8)))
					{
						if (bar.Stretch && bar.LayoutType == eLayoutType.DockContainer && bar.DockLine >= 0 && array[bar.DockLine] > 0)
						{
							if (array[bar.DockLine] == 1)
							{
								bar._0B01 = 0;
							}
							_௦2 = ((bar._0B01 != 0 && (array2[bar.DockLine] != 1 || array4[bar.DockLine] == ((Control)this).Height)) ? new Size(1, bar._0B01) : ((array2[bar.DockLine] != 1) ? new Size(1, (((Control)this).Height - array4[bar.DockLine]) / ((array5[bar.DockLine] == 0) ? 1 : array5[bar.DockLine])) : new Size(1, ((Control)this).Height - num6)));
							if (num8 != dockInfo.DockLine && array2[bar.DockLine] == array[bar.DockLine] && num6 > 0)
							{
								num8 = dockInfo.DockLine;
								num7++;
								num5 += num4 + num3;
								num6 = 0;
								num4 = 0;
								_௦2 = new Size(1, ((Control)this).Height);
							}
							array2[bar.DockLine]--;
							flag2 = true;
							arrayList.Add(bar);
						}
						else
						{
							_௦2 = new Size(1, ((Control)this).Height);
						}
					}
					else
					{
						_௦2 = new Size(1, ((Control)this).Height - num6);
					}
					bar._0BE5(_௦2);
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
							bar._0BE5(new Size(((Control)bar).Width, ((Control)this).Height - num6));
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
			if (ၕ == eOrientation.Horizontal)
			{
				if (item.ItemsContainer.HeightInternal != array3[item.DockLine])
				{
					item.ItemsContainer._0B03 = array3[item.DockLine];
					Size _௦3 = new Size(((Control)item).Width, 1);
					item._0BE5(_௦3);
				}
			}
			else if (item.ItemsContainer.WidthInternal != array3[item.DockLine])
			{
				item.ItemsContainer.ර = array3[item.DockLine];
				Size _௦4 = new Size(1, ((Control)item).Height);
				item._0BE5(_௦4);
			}
		}
		if (ၕ == eOrientation.Horizontal)
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
		if (!_1057)
		{
			if (((int)((Control)this).Dock == 4 && ((Control)this).Location.X < 0) || ((int)((Control)this).Dock == 3 && ((Control)this).Parent != null && ((Control)this).Right > ((Control)this).Parent.Width) || ((int)((Control)this).Dock == 1 && ((Control)this).Parent != null && ((Control)this).Bottom > ((Control)this).Parent.Height) || ((int)((Control)this).Dock == 2 && ((Control)this).Location.Y < 0))
			{
				_1057 = true;
				_1056 = false;
				_1069();
				if (_1057)
				{
					_1057 = false;
				}
			}
		}
		else
		{
			_1057 = false;
		}
		if (((Control)this).Parent != null && ((Control)this).Parent.Visible && ၜ)
		{
			_089F.SendMessage(((Control)this).Handle, 11, 1, 0);
			if (num != -1)
			{
				((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, num);
			}
			((Control)this).Parent.Invalidate(bounds2, true);
			((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
			((Control)this).Parent.Update();
		}
		_1056 = false;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		Control val = (Control)(object)((Control)this).FindForm();
		if (val != null)
		{
			val.Resize += _1068;
		}
	}

	private void _1068(object _٠, EventArgs ؿ)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		Control parent = ((Control)this).Parent;
		if ((parent == null || (parent.Width != 0 && parent.Height != 0)) && (parent == null || !(parent is Form) || (int)((Form)parent).WindowState != 1))
		{
			_1069();
		}
	}

	private void _1069()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		if ((int)((Control)this).Dock == 5 || (int)((Control)this).Dock == 0 || ၝ == null || ((Control)this).Width == 0 || ((Control)this).Height == 0 || (((Control)this).Parent != null && (((Control)this).Parent.Width == 0 || ((Control)this).Parent.Height == 0)))
		{
			return;
		}
		bool flag = (int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3;
		int num = (flag ? _106A() : _106B());
		int num2 = 32;
		if (_0AC3 != null)
		{
			num2 = (flag ? _0AC3.MinimumClientSize.Width : _0AC3.MinimumClientSize.Height);
		}
		if (num > num2)
		{
			if (ၛ <= 0)
			{
				return;
			}
			int num3 = ၛ - (flag ? ((Control)this).Width : ((Control)this).Height);
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
			if ((flag && ((Control)this).Width >= ၛ) || (!flag && ((Control)this).Height >= ၛ))
			{
				ၛ = 0;
			}
			return;
		}
		GetDocumentUIManager().RootDocumentDockContainer.Ⴝ();
		DocumentDockUIManager documentUIManager = GetDocumentUIManager();
		Size size = documentUIManager._109C(documentUIManager.RootDocumentDockContainer);
		int num4 = (flag ? size.Width : size.Height);
		int num5 = num2 - num;
		if ((flag && ((Control)this).Width - num5 < num4) || (!flag && ((Control)this).Height - num5 < num4))
		{
			num5 = (flag ? ((Control)this).Width : ((Control)this).Height) - num4;
		}
		if (num5 > 0)
		{
			if (ၛ == 0)
			{
				ၛ = (flag ? ((Control)this).Width : ((Control)this).Height);
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
		if (!_1056)
		{
			Control parent = ((Control)this).Parent;
			if ((parent == null || (parent.Width != 0 && parent.Height != 0)) && (parent == null || !(parent is Form) || (int)((Form)parent).WindowState != 1) && parent != null && ((_0AC3 != null && _0AC3.ParentForm != null && (int)_0AC3.ParentForm.WindowState != 1) || _0AC3 == null || _0AC3.ParentForm == null))
			{
				((Control)this).Invalidate(((Control)this).Region, false);
				_1067();
			}
		}
	}

	internal int _106A()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
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

	internal int _106B()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
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

	private int _106C(int _106D, int ၮ, bool ၯ)
	{
		int num = 0;
		int num2 = 0;
		if (ၯ)
		{
			for (num2 = _106D; num2 < ((ArrangedElementCollection)((Control)this).Controls).Count; num2++)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != ၮ)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Width;
				}
			}
		}
		else
		{
			for (num2 = _106D; num2 >= 0; num2--)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != ၮ)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Width;
				}
			}
		}
		return num;
	}

	private int ၰ(int _106D, int ၮ, bool ၯ)
	{
		int num = 0;
		int num2 = 0;
		if (ၯ)
		{
			for (num2 = _106D; num2 < ((ArrangedElementCollection)((Control)this).Controls).Count; num2++)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != ၮ)
					{
						break;
					}
					num += ((Control)this).Controls[num2].Height;
				}
			}
		}
		else
		{
			for (num2 = _106D; num2 >= 0; num2--)
			{
				if (((Control)this).Controls[num2].Visible)
				{
					IDockInfo dockInfo = ((Control)this).Controls[num2] as IDockInfo;
					if (dockInfo.DockLine != ၮ)
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
		if (ၕ == eOrientation.Horizontal)
		{
			return _1071(pDock, x, y);
		}
		return _1073(pDock, x, y);
	}

	private DockSiteInfo _1071(IDockInfo _1072, int و, int ى)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Invalid comparison between Unknown and I4
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Invalid comparison between Unknown and I4
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Invalid comparison between Unknown and I4
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		DockSiteInfo dockSiteInfo = new DockSiteInfo();
		Rectangle rectangle = new Rectangle(((Control)this).PointToScreen(new Point(0, 0)), ((Control)this).Size);
		Point pt = ((Control)this).PointToClient(new Point(و, ى));
		Control val = (Control)((_1072 is Control) ? _1072 : null);
		Size size = _1072.PreferredDockSize(ၕ);
		_1072.MinimumDockSize(ၕ);
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
		if (!rectangle.Contains(و, ى) || _1072 == null)
		{
			return dockSiteInfo;
		}
		bool flag = _1072.DockedSite == this;
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
							if (!((Bar)(object)val3).వ(و, ى))
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
					while (num4 >= 0 && ((IDockInfo)((Control)this).Controls[num4]).DockLine == _1072.DockLine)
					{
						insertPosition = num4;
						num4--;
					}
					dockSiteInfo.DockLine = _1072.DockLine;
					dockSiteInfo.DockOffset = _1072.DockOffset;
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
				while (num5 >= 0 && ((IDockInfo)((Control)this).Controls[num5]).DockLine == _1072.DockLine)
				{
					insertPosition2 = num5;
					num5--;
				}
				dockSiteInfo.DockLine = _1072.DockLine;
				dockSiteInfo.DockOffset = _1072.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition2;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.NewLine = true;
				return dockSiteInfo;
			}
			if ((int)((Control)this).Dock == 1 && val3 == val && pt.Y >= val3.Bottom - 4 && pt.Y < val3.Bottom - 1 && array[dockInfo2.DockLine] == 1 && _1072.DockLine != num)
			{
				int insertPosition3 = i;
				for (int j = i + 1; j < ((ArrangedElementCollection)((Control)this).Controls).Count && ((((IDockInfo)((Control)this).Controls[j]).DockLine == _1072.DockLine + 1 && (((IDockInfo)((Control)this).Controls[j]).DockOffset <= _1072.DockOffset || _1072.Stretch)) || ((IDockInfo)((Control)this).Controls[j]).DockLine == _1072.DockLine); j++)
				{
					insertPosition3 = j;
				}
				dockSiteInfo.DockLine = _1072.DockLine + 1;
				dockSiteInfo.DockOffset = _1072.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition3;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num2 == -10)
		{
			if (pt.Y <= -5 && (!flag || _1072.DockLine != 0 || array[0] != 1))
			{
				num2 = -1;
			}
			else if (!flag)
			{
				num2 = ((pt.Y <= ((Control)this).Height) ? num : (num + 1));
			}
			else if (pt.Y > ((Control)this).Height + 4 && (!flag || _1072.DockLine != num || array[num] != 1))
			{
				num2 = num + 1;
			}
			if (num2 == -10)
			{
				dockSiteInfo.DockLine = _1072.DockLine;
				dockSiteInfo.DockOffset = _1072.DockOffset;
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
						int num7 = _106C(l, num2, ၯ: false);
						if (num7 + size.Width >= ((Control)this).Width || pt.X < num7)
						{
							dockSiteInfo.DockOffset = val5.Right;
						}
						else
						{
							dockSiteInfo.DockOffset = pt.X;
						}
						dockSiteInfo.InsertPosition = l + 1;
						if (_1072.Stretch)
						{
							dockSiteInfo.DockOffset = _1072.DockOffset;
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
					int num8 = _106C(l + 1, _1072.DockLine, ၯ: true);
					if (num8 + size.Width + pt.X >= ((Control)this).Width)
					{
						dockSiteInfo.DockOffset = ((Control)this).Width - num8 - size.Width;
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
				if (_1072.Stretch)
				{
					dockSiteInfo.DockOffset = _1072.DockOffset;
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
		if (ၕ == eOrientation.Horizontal)
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

	private DockSiteInfo _1073(IDockInfo _1072, int و, int ى)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Invalid comparison between Unknown and I4
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Invalid comparison between Unknown and I4
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Invalid comparison between Unknown and I4
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		DockSiteInfo dockSiteInfo = new DockSiteInfo();
		Rectangle rectangle = new Rectangle(((Control)this).PointToScreen(new Point(0, 0)), ((Control)this).Size);
		Point pt = ((Control)this).PointToClient(new Point(و, ى));
		Control val = (Control)((_1072 is Control) ? _1072 : null);
		Size size = _1072.PreferredDockSize(ၕ);
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
		if (!rectangle.Contains(و, ى) || _1072 == null)
		{
			return dockSiteInfo;
		}
		bool flag = _1072.DockedSite == this;
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
							if (!((Bar)(object)val3).వ(و, ى))
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
				while (num4 >= 0 && ((IDockInfo)((Control)this).Controls[num4]).DockLine == _1072.DockLine)
				{
					insertPosition = num4;
					num4--;
				}
				dockSiteInfo.DockLine = _1072.DockLine;
				dockSiteInfo.DockOffset = _1072.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				dockSiteInfo.NewLine = true;
				return dockSiteInfo;
			}
			if ((int)((Control)this).Dock == 3 && val3 == val && pt.X >= val3.Right - 4 && pt.X < val3.Right && array[dockInfo2.DockLine] == 1 && _1072.DockLine != num)
			{
				int insertPosition2 = i;
				for (int j = i + 1; j < ((ArrangedElementCollection)((Control)this).Controls).Count && ((((IDockInfo)((Control)this).Controls[j]).DockLine == _1072.DockLine + 1 && (((IDockInfo)((Control)this).Controls[j]).DockOffset <= _1072.DockOffset || _1072.Stretch)) || ((IDockInfo)((Control)this).Controls[j]).DockLine == _1072.DockLine); j++)
				{
					insertPosition2 = j;
				}
				dockSiteInfo.DockLine = _1072.DockLine + 1;
				dockSiteInfo.DockOffset = _1072.DockOffset;
				dockSiteInfo.InsertPosition = insertPosition2;
				dockSiteInfo.objDockSite = this;
				dockSiteInfo.DockSide = ((Control)this).Dock;
				return dockSiteInfo;
			}
		}
		if (num2 == -10)
		{
			if (pt.X <= -5 && (!flag || _1072.DockLine != 0 || array[0] != 1))
			{
				num2 = -1;
			}
			else if (!flag)
			{
				num2 = ((pt.X <= ((Control)this).Width) ? num : (num + 1));
			}
			else if (pt.X > ((Control)this).Width + 4 && (!flag || _1072.DockLine != num || array[num] != 1))
			{
				num2 = num + 1;
			}
			if (num2 == -10)
			{
				dockSiteInfo.DockLine = _1072.DockLine;
				dockSiteInfo.DockOffset = _1072.DockOffset;
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
						int num6 = ၰ(l, num2, ၯ: false);
						if (num6 + size.Height >= ((Control)this).Height || pt.Y < num6)
						{
							dockSiteInfo.DockOffset = val5.Bottom;
						}
						else
						{
							dockSiteInfo.DockOffset = pt.Y;
						}
						dockSiteInfo.InsertPosition = l + 1;
						if (_1072.Stretch)
						{
							dockSiteInfo.DockOffset = _1072.DockOffset;
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
					int num7 = ၰ(l + 1, _1072.DockLine, ၯ: true);
					if (num7 + size.Height + pt.Y >= ((Control)this).Height)
					{
						dockSiteInfo.DockOffset = ((Control)this).Height - num7 - size.Height;
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
				if (_1072.Stretch)
				{
					dockSiteInfo.DockOffset = _1072.DockOffset;
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
		if (ၕ == eOrientation.Horizontal)
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
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Expected I4, but got Unknown
		if (pDockInfo.objDockSite == null)
		{
			return Rectangle.Empty;
		}
		if ((int)((Control)pDockInfo.objDockSite).Dock == 5 || ၝ != null)
		{
			DocumentDockUIManager documentUIManager = GetDocumentUIManager();
			return documentUIManager._1083(bar, ref pDockInfo);
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
				num = ၸ();
			}
			else if (pDockInfo.PartialSizeDock)
			{
				num2 = ၷ();
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
					result.Width += _1074(_0AC3.LeftDockSite, ၶ: true).Width;
					result.Width += _1074(_0AC3.RightDockSite, ၶ: true).Width;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Width -= _1074(_0AC3.LeftDockSite, ၶ: false).Width;
					result.Width -= _1074(_0AC3.RightDockSite, ၶ: false).Width;
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
					empty3.X -= _1074(_0AC3.LeftDockSite, ၶ: true).Width;
				}
				else if (num2 >= 0)
				{
					empty3.X += _1074(_0AC3.LeftDockSite, ၶ: false).Width;
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
					result.Width += _1074(_0AC3.LeftDockSite, ၶ: true).Width;
					result.Width += _1074(_0AC3.RightDockSite, ၶ: true).Width;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Width -= _1074(_0AC3.LeftDockSite, ၶ: false).Width;
					result.Width -= _1074(_0AC3.RightDockSite, ၶ: false).Width;
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
					empty2.X -= _1074(_0AC3.LeftDockSite, ၶ: true).Width;
				}
				else if (num2 >= 0)
				{
					empty2.X += _1074(_0AC3.LeftDockSite, ၶ: false).Width;
				}
				result.Location = empty2;
				break;
			}
			case 3:
			{
				result.Height = ((Control)this).ClientRectangle.Height;
				if (num >= 0)
				{
					result.Height += _1074(_0AC3.TopDockSite, ၶ: true).Height;
					result.Height += _1074(_0AC3.BottomDockSite, ၶ: true).Height;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Height -= _1074(_0AC3.TopDockSite, ၶ: false).Height;
					result.Height -= _1074(_0AC3.BottomDockSite, ၶ: false).Height;
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
					empty4.Y -= _1074(_0AC3.TopDockSite, ၶ: true).Height;
				}
				else if (num2 >= 0)
				{
					empty4.Y += _1074(_0AC3.TopDockSite, ၶ: false).Height;
				}
				result.Location = empty4;
				break;
			}
			default:
			{
				result.Height = ((Control)this).ClientRectangle.Height;
				if (num >= 0)
				{
					result.Height += _1074(_0AC3.TopDockSite, ၶ: true).Height;
					result.Height += _1074(_0AC3.BottomDockSite, ၶ: true).Height;
					pDockInfo.DockSiteZOrderIndex = num;
				}
				else if (num2 >= 0)
				{
					result.Height -= _1074(_0AC3.TopDockSite, ၶ: false).Height;
					result.Height -= _1074(_0AC3.BottomDockSite, ၶ: false).Height;
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
					empty.Y -= _1074(_0AC3.TopDockSite, ၶ: true).Height;
				}
				else if (num2 >= 0)
				{
					empty.Y += _1074(_0AC3.TopDockSite, ၶ: false).Height;
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
			DockStyle dock = ((Control)this).Dock;
			switch (dock - 1)
			{
			case 0:
			case 1:
				result.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
				if (pDockInfo.NewLine || bar2 == bar)
				{
					result.Width = ((Control)bar2).Width;
					if (result.Width == 0)
					{
						result.Width = ((Control)this).Width;
					}
					Point location4 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location4;
				}
				else if (bar2.DockOffset > pDockInfo.DockOffset)
				{
					result.Height = ((Control)bar2).Height;
					result.Width = ((Control)bar2).Width / 2;
					Point location5 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location5;
				}
				else
				{
					result.Height = ((Control)bar2).Height;
					result.Width = ((Control)bar2).Width / 2;
					Point location6 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left + result.Width, ((Control)bar2).Top));
					result.Location = location6;
				}
				break;
			default:
				result.Width = bar.GetBarDockedSize(eOrientation.Vertical);
				if (pDockInfo.NewLine || bar2 == bar)
				{
					result.Height = ((Control)bar2).Height;
					Point location = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location;
				}
				else if (bar2.DockOffset < pDockInfo.DockOffset)
				{
					result.Width = ((Control)bar2).Width;
					result.Height = ((Control)bar2).Height / 2;
					Point location2 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top + result.Height));
					result.Location = location2;
				}
				else
				{
					result.Width = ((Control)bar2).Width;
					result.Height = ((Control)bar2).Height / 2;
					Point location3 = ((Control)this).PointToScreen(new Point(((Control)bar2).Left, ((Control)bar2).Top));
					result.Location = location3;
				}
				break;
			}
		}
		return result;
	}

	internal Size _1074(DockSite ၵ, bool ၶ)
	{
		if (ၵ == null || ((Control)ၵ).Parent == null)
		{
			return Size.Empty;
		}
		Size result = Size.Empty;
		if (ၶ)
		{
			if (((Control)ၵ).Parent.Controls.IndexOf((Control)(object)ၵ) > ((Control)this).Parent.Controls.IndexOf((Control)(object)this))
			{
				result = ((Control)ၵ).Size;
			}
		}
		else if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) > ((Control)ၵ).Parent.Controls.IndexOf((Control)(object)ၵ))
		{
			result = ((Control)ၵ).Size;
		}
		return result;
	}

	internal int ၷ()
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
			if (_0AC3 == null || _0AC3.TopDockSite == null || _0AC3.BottomDockSite == null || ((Control)_0AC3.TopDockSite).Parent == null || ((Control)_0AC3.BottomDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Min(((Control)_0AC3.TopDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.TopDockSite), ((Control)_0AC3.BottomDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.BottomDockSite));
		}
		else
		{
			if (_0AC3 == null || _0AC3.LeftDockSite == null || _0AC3.RightDockSite == null || ((Control)_0AC3.LeftDockSite).Parent == null || ((Control)_0AC3.RightDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Min(((Control)_0AC3.LeftDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.LeftDockSite), ((Control)_0AC3.RightDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.RightDockSite));
		}
		if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) < num)
		{
			return -1;
		}
		return num;
	}

	internal int ၸ()
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
			if (_0AC3 == null || _0AC3.TopDockSite == null || _0AC3.BottomDockSite == null || ((Control)_0AC3.TopDockSite).Parent == null || ((Control)_0AC3.BottomDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Max(((Control)_0AC3.TopDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.TopDockSite), ((Control)_0AC3.BottomDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.BottomDockSite));
		}
		else
		{
			if (_0AC3 == null || _0AC3.LeftDockSite == null || _0AC3.RightDockSite == null || ((Control)_0AC3.LeftDockSite).Parent == null || ((Control)_0AC3.RightDockSite).Parent == null)
			{
				return -1;
			}
			num = Math.Max(((Control)_0AC3.LeftDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.LeftDockSite), ((Control)_0AC3.RightDockSite).Parent.Controls.IndexOf((Control)(object)_0AC3.RightDockSite));
		}
		if (((Control)this).Parent.Controls.IndexOf((Control)(object)this) > num)
		{
			return -1;
		}
		return num;
	}

	internal void ၹ(Control _105F)
	{
		if (((Control)this).Contains(_105F))
		{
			((Control)this).Controls.Remove(_105F);
			_1067();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (ၝ != null)
		{
			ၚ.OnMouseMove(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (ၝ != null)
		{
			ၚ.OnMouseLeave();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button == 2097152 && ၝ == null)
		{
			((IOwnerBarSupport)_0AC3)?.BarContextMenu((Control)(object)this, e);
		}
		if (ၝ != null)
		{
			ၚ.OnMouseDown(e);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (ၝ != null)
		{
			ၚ.OnMouseUp(e);
		}
	}

	private void ၺ()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (_0AC3 == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is Bar)
			{
				Bar bar = val as Bar;
				if (!_0AC3.Bars.Contains(bar))
				{
					_0AC3.Bars.Add(bar);
				}
			}
		}
	}

	protected override void OnDockChanged(EventArgs e)
	{
		((Control)this).OnDockChanged(e);
		if (ၓ)
		{
			ၺ();
		}
	}

	internal void ၻ(DotNetBarManager ॡ)
	{
		_0AC3 = ॡ;
		ၺ();
	}
}
