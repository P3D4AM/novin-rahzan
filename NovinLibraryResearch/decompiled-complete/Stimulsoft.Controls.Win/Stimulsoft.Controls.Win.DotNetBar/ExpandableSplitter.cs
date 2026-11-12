using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class ExpandableSplitter : Splitter, IMessageHandlerClient
{
	private int m_AnimationTime = 100;

	private Control m_ExpandableControl;

	private bool m_Expanded = true;

	private eSplitterStyle m_Style;

	private bool m_Expandable = true;

	private eShortcut m_Shortcut;

	private bool m_FilterInstalled;

	private bool m_MouseOver;

	private bool m_MouseDown;

	private bool m_Resized;

	private Point m_MouseDownPoint = Point.Empty;

	private ColorScheme m_ColorScheme;

	private eColorSchemePart m_BackColorSchemePart = eColorSchemePart.None;

	private Color m_BackColor2 = Color.Empty;

	private eColorSchemePart m_BackColor2SchemePart = eColorSchemePart.None;

	private int m_BackColorGradientAngle;

	private Color m_ExpandFillColor = Color.Empty;

	private eColorSchemePart m_ExpandFillColorSchemePart = eColorSchemePart.None;

	private Color m_ExpandLineColor = Color.Empty;

	private eColorSchemePart m_ExpandLineColorSchemePart = eColorSchemePart.None;

	private Color m_GripDarkColor = Color.Empty;

	private eColorSchemePart m_GripDarkColorSchemePart = eColorSchemePart.None;

	private Color m_GripLightColor = Color.Empty;

	private eColorSchemePart m_GripLightColorSchemePart = eColorSchemePart.None;

	private Color m_HotBackColor = Color.Empty;

	private eColorSchemePart m_HotBackColorSchemePart = eColorSchemePart.None;

	private Color m_HotBackColor2 = Color.Empty;

	private eColorSchemePart m_HotBackColor2SchemePart = eColorSchemePart.None;

	private int m_HotBackColorGradientAngle;

	private Color m_HotExpandFillColor = Color.Empty;

	private eColorSchemePart m_HotExpandFillColorSchemePart = eColorSchemePart.None;

	private Color m_HotExpandLineColor = Color.Empty;

	private eColorSchemePart m_HotExpandLineColorSchemePart = eColorSchemePart.None;

	private Color m_HotGripDarkColor = Color.Empty;

	private eColorSchemePart m_HotGripDarkColorSchemePart = eColorSchemePart.None;

	private Color m_HotGripLightColor = Color.Empty;

	private eColorSchemePart m_HotGripLightColorSchemePart = eColorSchemePart.None;

	private SplitterPainter m_Painter;

	private SplitterPaintInfo m_SplitterPaintInfo = new SplitterPaintInfo();

	private bool m_ExpandActionClick = true;

	private bool m_ExpandActionDoubleClick;

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(true)]
	[Description("Indicates whether expandable control ExpandableControl assigned to this splitter is expaned or not. Default value is true.")]
	public bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			if (m_Expanded != value)
			{
				SetExpanded(value, eEventSource.Code);
			}
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(true)]
	[Description("Indicates whether Click event is triggering expand/collapse of the splitter. Default value is true.")]
	public bool ExpandActionClick
	{
		get
		{
			return m_ExpandActionClick;
		}
		set
		{
			m_ExpandActionClick = value;
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(false)]
	[Description("Indicates whether DoubleClick event is triggering expand/collapse of the splitter. Default value is false.")]
	public bool ExpandActionDoubleClick
	{
		get
		{
			return m_ExpandActionDoubleClick;
		}
		set
		{
			m_ExpandActionDoubleClick = value;
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(true)]
	[Description("Indicates whether splitter will act as expandable splitter. Default value is true.")]
	public bool Expandable
	{
		get
		{
			return m_Expandable;
		}
		set
		{
			m_Expandable = value;
			((Control)this).Refresh();
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(null)]
	[Description("Indicates control that will be expanded/collapsed by the splitter.")]
	public Control ExpandableControl
	{
		get
		{
			return m_ExpandableControl;
		}
		set
		{
			if (value != this)
			{
				m_ExpandableControl = value;
				OnExpandableControlChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eSplitterStyle.Office2003)]
	[Description("Indicates visual style of the control.")]
	public eSplitterStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(100)]
	[Category("Expand")]
	[Description("Indicates animation time in milliseconds, default value is 100.")]
	public int AnimationTime
	{
		get
		{
			return m_AnimationTime;
		}
		set
		{
			if (m_AnimationTime >= 0)
			{
				m_AnimationTime = value;
			}
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(eShortcut.None)]
	[Description("Indicates shortcut key to expand/collapse splitter.")]
	public eShortcut Shortcut
	{
		get
		{
			return m_Shortcut;
		}
		set
		{
			m_Shortcut = value;
			if (m_Shortcut != 0 && ((Control)this).IsHandleCreated)
			{
				InstallIMessageHandlerClient();
			}
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Gets or sets the background color for UI element.")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	public Color BackColor
	{
		get
		{
			return GetColor(((Control)this).BackColor, m_BackColorSchemePart);
		}
		set
		{
			if (m_BackColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				BackColorSchemePart = eColorSchemePart.None;
			}
			((Control)this).BackColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart BackColorSchemePart
	{
		get
		{
			return m_BackColorSchemePart;
		}
		set
		{
			m_BackColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Gets or sets the target gradient background color for UI element.")]
	public Color BackColor2
	{
		get
		{
			return GetColor(m_BackColor2, m_BackColor2SchemePart);
		}
		set
		{
			if (m_BackColor2SchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				BackColor2SchemePart = eColorSchemePart.None;
			}
			m_BackColor2 = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart BackColor2SchemePart
	{
		get
		{
			return m_BackColor2SchemePart;
		}
		set
		{
			m_BackColor2SchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(0)]
	[Description("Gets or sets the background gradient angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			if (m_BackColorGradientAngle != value)
			{
				m_BackColorGradientAngle = value;
				OnStyleChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part fill color.")]
	public Color ExpandFillColor
	{
		get
		{
			return GetColor(m_ExpandFillColor, m_ExpandFillColorSchemePart);
		}
		set
		{
			if (m_ExpandFillColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				ExpandFillColorSchemePart = eColorSchemePart.None;
			}
			m_ExpandFillColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart ExpandFillColorSchemePart
	{
		get
		{
			return m_ExpandFillColorSchemePart;
		}
		set
		{
			m_ExpandFillColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part line color.")]
	public Color ExpandLineColor
	{
		get
		{
			return GetColor(m_ExpandLineColor, m_ExpandLineColorSchemePart);
		}
		set
		{
			if (m_ExpandLineColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				ExpandLineColorSchemePart = eColorSchemePart.None;
			}
			m_ExpandLineColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart ExpandLineColorSchemePart
	{
		get
		{
			return m_ExpandLineColorSchemePart;
		}
		set
		{
			m_ExpandLineColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part line color.")]
	public Color GripDarkColor
	{
		get
		{
			return GetColor(m_GripDarkColor, m_GripDarkColorSchemePart);
		}
		set
		{
			if (m_GripDarkColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				GripDarkColorSchemePart = eColorSchemePart.None;
			}
			m_GripDarkColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart GripDarkColorSchemePart
	{
		get
		{
			return m_GripDarkColorSchemePart;
		}
		set
		{
			m_GripDarkColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part line color.")]
	public Color GripLightColor
	{
		get
		{
			return GetColor(m_GripLightColor, m_GripLightColorSchemePart);
		}
		set
		{
			if (m_GripLightColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				GripLightColorSchemePart = eColorSchemePart.None;
			}
			m_GripLightColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart GripLightColorSchemePart
	{
		get
		{
			return m_GripLightColorSchemePart;
		}
		set
		{
			m_GripLightColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Gets or sets the background color for UI element when mouse is over the element.")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	public Color HotBackColor
	{
		get
		{
			return GetColor(m_HotBackColor, m_HotBackColorSchemePart);
		}
		set
		{
			if (m_HotBackColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotBackColorSchemePart = eColorSchemePart.None;
			}
			m_HotBackColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotBackColorSchemePart
	{
		get
		{
			return m_HotBackColorSchemePart;
		}
		set
		{
			m_HotBackColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Gets or sets the target gradient background color for UI element when mouse is over the element.")]
	public Color HotBackColor2
	{
		get
		{
			return GetColor(m_HotBackColor2, m_HotBackColor2SchemePart);
		}
		set
		{
			if (m_HotBackColor2SchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotBackColor2SchemePart = eColorSchemePart.None;
			}
			m_HotBackColor2 = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotBackColor2SchemePart
	{
		get
		{
			return m_HotBackColor2SchemePart;
		}
		set
		{
			m_HotBackColor2SchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(0)]
	[Description("Gets or sets the background gradient angle when mouse is over the element.")]
	public int HotBackColorGradientAngle
	{
		get
		{
			return m_HotBackColorGradientAngle;
		}
		set
		{
			if (m_HotBackColorGradientAngle != value)
			{
				m_HotBackColorGradientAngle = value;
				OnStyleChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part fill color when mouse is over the element.")]
	public Color HotExpandFillColor
	{
		get
		{
			return GetColor(m_HotExpandFillColor, m_HotExpandFillColorSchemePart);
		}
		set
		{
			if (m_HotExpandFillColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotExpandFillColorSchemePart = eColorSchemePart.None;
			}
			m_HotExpandFillColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotExpandFillColorSchemePart
	{
		get
		{
			return m_HotExpandFillColorSchemePart;
		}
		set
		{
			m_HotExpandFillColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part line color when mouse is over the element.")]
	public Color HotExpandLineColor
	{
		get
		{
			return GetColor(m_HotExpandLineColor, m_HotExpandLineColorSchemePart);
		}
		set
		{
			if (m_HotExpandLineColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotExpandLineColorSchemePart = eColorSchemePart.None;
			}
			m_HotExpandLineColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotExpandLineColorSchemePart
	{
		get
		{
			return m_HotExpandLineColorSchemePart;
		}
		set
		{
			m_HotExpandLineColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part line color when mouse is over the element.")]
	public Color HotGripDarkColor
	{
		get
		{
			return GetColor(m_HotGripDarkColor, m_HotGripDarkColorSchemePart);
		}
		set
		{
			if (m_HotGripDarkColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotGripDarkColorSchemePart = eColorSchemePart.None;
			}
			m_HotGripDarkColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotGripDarkColorSchemePart
	{
		get
		{
			return m_HotGripDarkColorSchemePart;
		}
		set
		{
			m_HotGripDarkColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates grip part light color when mouse is over the element.")]
	public Color HotGripLightColor
	{
		get
		{
			return GetColor(m_HotGripLightColor, m_HotGripLightColorSchemePart);
		}
		set
		{
			if (m_HotGripLightColorSchemePart != eColorSchemePart.None && !value.IsEmpty)
			{
				HotGripLightColorSchemePart = eColorSchemePart.None;
			}
			m_HotGripLightColor = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotGripLightColorSchemePart
	{
		get
		{
			return m_HotGripLightColorSchemePart;
		}
		set
		{
			m_HotGripLightColorSchemePart = value;
			OnStyleChanged();
		}
	}

	bool IMessageHandlerClient.IsModal
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return val.Modal;
			}
			return false;
		}
	}

	public event ExpandChangeEventHandler ExpandedChanging;

	public event ExpandChangeEventHandler ExpandedChanged;

	public ExpandableSplitter()
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		ApplyStyle(eSplitterStyle.Office2003);
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	private SplitterPainter GetPainter()
	{
		if (m_Painter == null)
		{
			switch (m_Style)
			{
			case eSplitterStyle.Office2003:
			case eSplitterStyle.Office2007:
				m_Painter = new SplitterOffice2003Painter();
				break;
			case eSplitterStyle.Mozilla:
				m_Painter = new SplitterMozillaPainter();
				break;
			}
		}
		return m_Painter;
	}

	private SplitterPaintInfo GetSplitterPaintInfo(Graphics g)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		m_SplitterPaintInfo.Graphics = g;
		m_SplitterPaintInfo.Colors = GetColors();
		m_SplitterPaintInfo.Expandable = m_Expandable;
		m_SplitterPaintInfo.Expanded = m_Expanded;
		m_SplitterPaintInfo.Dock = ((Control)this).Dock;
		m_SplitterPaintInfo.DisplayRectangle = ((Control)this).DisplayRectangle;
		return m_SplitterPaintInfo;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		GetPainter().Paint(GetSplitterPaintInfo(e.Graphics));
	}

	private void OnExpandedChanged()
	{
		if (ExpandableControl == null)
		{
			return;
		}
		if (Expanded)
		{
			if (!ExpandableControl.Visible)
			{
				if (AnimationTime == 0 || ((Component)this).DesignMode)
				{
					if (((Component)this).DesignMode)
					{
						TypeDescriptor.GetProperties(ExpandableControl)["Visible"].SetValue(ExpandableControl, true);
					}
					ExpandableControl.Visible = true;
				}
				else
				{
					Rectangle animationTarget = GetAnimationTarget(ExpandableControl, showControl: false);
					Rectangle bounds = ExpandableControl.Bounds;
					BarFunctions.AnimateControl(ExpandableControl, show: true, m_AnimationTime, animationTarget, bounds);
				}
			}
		}
		else if (ExpandableControl.Visible || !((Control)this).IsHandleCreated)
		{
			if (AnimationTime == 0 || ((Component)this).DesignMode || !((Control)this).IsHandleCreated)
			{
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(ExpandableControl)["Visible"].SetValue(ExpandableControl, false);
				}
				ExpandableControl.Visible = false;
			}
			else
			{
				Rectangle bounds2 = ExpandableControl.Bounds;
				Rectangle animationTarget2 = GetAnimationTarget(ExpandableControl, showControl: false);
				BarFunctions.AnimateControl(ExpandableControl, show: false, m_AnimationTime, bounds2, animationTarget2);
				ExpandableControl.Visible = false;
				ExpandableControl.Bounds = bounds2;
			}
		}
		((Control)this).Refresh();
	}

	private Rectangle GetAnimationTarget(Control c, bool showControl)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		Rectangle result = Rectangle.Empty;
		DockStyle controlDock = GetControlDock(c);
		if (!showControl)
		{
			if ((int)controlDock == 3)
			{
				result = new Rectangle(c.Left, c.Top, 0, c.Height);
			}
			else if ((int)controlDock == 4)
			{
				result = new Rectangle(c.Right, c.Top, 0, c.Height);
			}
			else if ((int)controlDock == 1)
			{
				result = new Rectangle(c.Left, c.Top, c.Width, 0);
			}
			else if ((int)controlDock == 2)
			{
				result = new Rectangle(c.Left, c.Bottom, c.Width, 0);
			}
		}
		else if ((int)controlDock == 3)
		{
			result = new Rectangle(c.Left, c.Top, c.Width, c.Height);
		}
		else if ((int)controlDock == 4)
		{
			result = new Rectangle(c.Left - c.Width, c.Top, c.Width, c.Height);
		}
		else if ((int)controlDock == 1)
		{
			result = new Rectangle(c.Left, c.Top, c.Width, c.Height);
		}
		else if ((int)controlDock == 2)
		{
			result = new Rectangle(c.Left, c.Top - c.Height, c.Width, c.Height);
		}
		return result;
	}

	private DockStyle GetControlDock(Control c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if ((int)c.Dock == 0)
		{
			if (c.Right >= ((Control)this).Left)
			{
				if (c.Left <= ((Control)this).Right)
				{
					if (c.Bottom >= ((Control)this).Top)
					{
						if (c.Top <= ((Control)this).Bottom)
						{
							return (DockStyle)3;
						}
						return (DockStyle)2;
					}
					return (DockStyle)1;
				}
				return (DockStyle)4;
			}
			return (DockStyle)3;
		}
		if ((int)c.Dock == 5)
		{
			return (DockStyle)3;
		}
		return c.Dock;
	}

	private void OnExpandableControlChanged()
	{
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		InstallIMessageHandlerClient();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Control)this).OnHandleDestroyed(e);
		UninstallIMessageHandlerClient();
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (Expandable && m_MouseOver && !m_Resized && m_ExpandActionClick)
		{
			SetExpanded(!Expanded, eEventSource.Mouse);
		}
		m_Resized = false;
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		if (Expandable && m_MouseOver && m_ExpandActionDoubleClick)
		{
			SetExpanded(!Expanded, eEventSource.Mouse);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (m_MouseDown)
		{
			((Splitter)this).OnMouseDown(e);
			return;
		}
		m_Resized = false;
		m_MouseDownPoint = new Point(e.X, e.Y);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		m_MouseDown = false;
		((Splitter)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		m_MouseOver = true;
		((Control)this).Refresh();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		m_MouseOver = false;
		((Control)this).Refresh();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		((Splitter)this).OnMouseMove(e);
		if (!((Control)this).DisplayRectangle.Contains(e.X, e.Y) && !m_MouseDown && (int)e.Button == 1048576)
		{
			m_MouseDown = true;
			m_Resized = true;
			((Control)this).OnMouseDown(new MouseEventArgs(e.Button, 1, m_MouseDownPoint.X, m_MouseDownPoint.Y, e.Delta));
		}
	}

	private void InvokeExpandedChanging(ExpandedChangeEventArgs e)
	{
		if (this.ExpandedChanging != null)
		{
			this.ExpandedChanging(this, e);
		}
	}

	private void InvokeExpandedChanged(ExpandedChangeEventArgs e)
	{
		if (this.ExpandedChanged != null)
		{
			this.ExpandedChanged(this, e);
		}
	}

	private void SetExpanded(bool expanded, eEventSource action)
	{
		ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(action, expanded);
		InvokeExpandedChanging(expandedChangeEventArgs);
		if (!expandedChangeEventArgs.Cancel)
		{
			m_Expanded = expanded;
			OnExpandedChanged();
			InvokeExpandedChanged(expandedChangeEventArgs);
		}
	}

	private void OnStyleChanged()
	{
		m_Painter = null;
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		((Control)this).BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		m_BackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandFillColor()
	{
		m_ExpandFillColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandLineColor()
	{
		m_ExpandLineColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGripDarkColor()
	{
		m_GripDarkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGripLightColor()
	{
		m_GripLightColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotBackColor()
	{
		m_HotBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotBackColor2()
	{
		m_HotBackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotExpandFillColor()
	{
		m_HotExpandFillColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotExpandLineColor()
	{
		m_HotExpandLineColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotGripDarkColor()
	{
		m_HotGripDarkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotGripLightColor()
	{
		m_HotGripLightColor = Color.Empty;
	}

	private Color GetColor(Color color, eColorSchemePart p)
	{
		if (p == eColorSchemePart.None || p == eColorSchemePart.Custom)
		{
			return color;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme == null)
		{
			return color;
		}
		return (Color)colorScheme.GetType().GetProperty(p.ToString()).GetValue(colorScheme, null);
	}

	private ColorScheme GetColorScheme()
	{
		if (Style == eSplitterStyle.Office2007 && GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		}
		return m_ColorScheme;
	}

	private SplitterColors GetColors()
	{
		SplitterColors splitterColors = new SplitterColors();
		if (m_MouseOver && !HotBackColor.IsEmpty)
		{
			splitterColors.BackColor = HotBackColor;
			splitterColors.BackColor2 = HotBackColor2;
			splitterColors.BackColorGradientAngle = HotBackColorGradientAngle;
			splitterColors.ExpandFillColor = HotExpandFillColor;
			splitterColors.ExpandLineColor = HotExpandLineColor;
			splitterColors.GripDarkColor = HotGripDarkColor;
			splitterColors.GripLightColor = HotGripLightColor;
		}
		else
		{
			splitterColors.BackColor = BackColor;
			splitterColors.BackColor2 = BackColor2;
			splitterColors.BackColorGradientAngle = BackColorGradientAngle;
			splitterColors.ExpandFillColor = ExpandFillColor;
			splitterColors.ExpandLineColor = ExpandLineColor;
			splitterColors.GripDarkColor = GripDarkColor;
			splitterColors.GripLightColor = GripLightColor;
		}
		return splitterColors;
	}

	public void ApplyStyle(eSplitterStyle style)
	{
		switch (style)
		{
		case eSplitterStyle.Office2003:
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
			BackColorSchemePart = eColorSchemePart.PanelBackground;
			BackColor2SchemePart = eColorSchemePart.PanelBorder;
			GripLightColorSchemePart = eColorSchemePart.BarBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemText;
			ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			ExpandLineColorSchemePart = eColorSchemePart.ItemText;
			HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
			HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
			break;
		case eSplitterStyle.Office2007:
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
			BackColorSchemePart = eColorSchemePart.PanelBackground;
			BackColor2SchemePart = eColorSchemePart.PanelBorder;
			GripLightColorSchemePart = eColorSchemePart.BarBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemText;
			ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			ExpandLineColorSchemePart = eColorSchemePart.ItemText;
			HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
			HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
			break;
		case eSplitterStyle.Mozilla:
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.VS2005);
			BackColorSchemePart = eColorSchemePart.None;
			BackColor = SystemColors.ControlLight;
			BackColor2SchemePart = eColorSchemePart.None;
			BackColor2 = Color.Empty;
			GripLightColorSchemePart = eColorSchemePart.MenuBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemPressedBorder;
			ExpandFillColorSchemePart = eColorSchemePart.ItemPressedBackground;
			ExpandLineColorSchemePart = eColorSchemePart.ItemPressedBorder;
			HotBackColorSchemePart = eColorSchemePart.ItemCheckedBackground;
			HotBackColor2 = Color.Empty;
			HotBackColor2SchemePart = eColorSchemePart.None;
			HotGripLightColorSchemePart = eColorSchemePart.MenuBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.ItemPressedBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemPressedBorder;
			break;
		}
	}

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected I4, but got Unknown
		int num = WinApi.ToInt(wParam);
		if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
		{
			int key = Control.ModifierKeys | num;
			if (ProcessShortcut((eShortcut)key))
			{
				return true;
			}
		}
		return false;
	}

	private bool ProcessShortcut(eShortcut key)
	{
		if (key == m_Shortcut && Expandable)
		{
			SetExpanded(!Expanded, eEventSource.Keyboard);
		}
		return false;
	}

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!((Component)this).DesignMode)
		{
			int num = WinApi.ToInt(wParam);
			if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
			{
				int key = Control.ModifierKeys | num;
				if (ProcessShortcut((eShortcut)key))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	private void InstallIMessageHandlerClient()
	{
		if (!m_FilterInstalled && !((Component)this).DesignMode && m_Shortcut != 0)
		{
			MessageHandler.RegisterMessageClient(this);
			m_FilterInstalled = true;
		}
	}

	private void UninstallIMessageHandlerClient()
	{
		if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
	}
}
