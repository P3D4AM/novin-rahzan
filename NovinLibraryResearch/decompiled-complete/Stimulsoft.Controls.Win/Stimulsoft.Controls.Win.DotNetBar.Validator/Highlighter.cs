using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

[ToolboxBitmap(typeof(Highlighter), "Validator.Highlighter.ico")]
[ToolboxItem(false)]
[ProvideProperty("HighlightColor", typeof(Control))]
[ProvideProperty("HighlightOnFocus", typeof(Control))]
[ComVisible(false)]
public class Highlighter : Component, IExtenderProvider, IErrorProvider
{
	private Dictionary<Control, eHighlightColor> _Highlights = new Dictionary<Control, eHighlightColor>();

	private Dictionary<Control, bool> _HighlightOnFocus = new Dictionary<Control, bool>();

	private Dictionary<TabControl, int> _TabControl1 = new Dictionary<TabControl, int>();

	private Dictionary<SuperTabControl, int> _SuperTabControl1 = new Dictionary<SuperTabControl, int>();

	private Dictionary<TabControl, int> _TabControl2 = new Dictionary<TabControl, int>();

	private Dictionary<Panel, int> _ParentPanel = new Dictionary<Panel, int>();

	private eHighlightColor _FocusHighlightColor = eHighlightColor.Blue;

	private HighlightPanel _HighlightPanel;

	private Control _ContainerControl;

	private Timer _DelayTimer;

	private Color[] _CustomHighlightColors;

	internal Dictionary<Control, eHighlightColor> Highlights => _Highlights;

	[DefaultValue(eHighlightColor.Blue)]
	[Category("Appearance")]
	[Description("Indicates the highlight focus color.")]
	[Localizable(true)]
	public eHighlightColor FocusHighlightColor
	{
		get
		{
			return _FocusHighlightColor;
		}
		set
		{
			_FocusHighlightColor = value;
			if (_HighlightPanel != null)
			{
				_HighlightPanel.FocusHighlightColor = value;
				UpdateHighlighterRegion();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates container control highlighter is bound to. Should be set to parent form.")]
	[Category("Behavior")]
	public Control ContainerControl
	{
		get
		{
			return _ContainerControl;
		}
		set
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			if (base.DesignMode)
			{
				_ContainerControl = value;
			}
			else
			{
				if (_ContainerControl == value)
				{
					return;
				}
				if (_ContainerControl != null)
				{
					_ContainerControl.SizeChanged -= ContainerControlSizeChanged;
					if (_HighlightPanel != null && ((Control)_HighlightPanel).Parent == _ContainerControl)
					{
						_ContainerControl.Controls.Remove((Control)(object)_HighlightPanel);
					}
				}
				_ContainerControl = value;
				if (_ContainerControl != null)
				{
					if (_HighlightPanel == null)
					{
						_HighlightPanel = new HighlightPanel(_Highlights);
						_HighlightPanel.FocusHighlightColor = _FocusHighlightColor;
						((Control)_HighlightPanel).Margin = new Padding(0);
						((Control)_HighlightPanel).Padding = new Padding(0);
						_HighlightPanel.CustomHighlightColors = _CustomHighlightColors;
						((Control)_HighlightPanel).Visible = false;
					}
					_ContainerControl.SizeChanged += ContainerControlSizeChanged;
					_ContainerControl.Controls.Add((Control)(object)_HighlightPanel);
					UpdateHighlightPanelBounds();
				}
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Array of colors used to render custom highlight color. Control expects 3 colors in array to be specified which define the highlight border.")]
	public Color[] CustomHighlightColors
	{
		get
		{
			return _CustomHighlightColors;
		}
		set
		{
			_CustomHighlightColors = value;
			if (_HighlightPanel != null)
			{
				_HighlightPanel.CustomHighlightColors = _CustomHighlightColors;
				((Control)_HighlightPanel).Invalidate();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_HighlightPanel != null && ((Control)_HighlightPanel).Parent == null && !((Control)_HighlightPanel).IsDisposed)
		{
			((Component)(object)_HighlightPanel).Dispose();
			_HighlightPanel = null;
		}
		else
		{
			_HighlightPanel = null;
		}
		base.Dispose(disposing);
	}

	[DefaultValue(false)]
	[Localizable(true)]
	[Description("Indicates whether control is highlighted when it receives input focus.")]
	public bool GetHighlightOnFocus(Control c)
	{
		if (_HighlightOnFocus.ContainsKey(c))
		{
			return _HighlightOnFocus[c];
		}
		return false;
	}

	public void SetHighlightOnFocus(Control c, bool highlight)
	{
		if (c == null)
		{
			throw new NullReferenceException();
		}
		if (_HighlightOnFocus.ContainsKey(c))
		{
			if (!highlight)
			{
				RemoveHighlightOnFocus(_HighlightOnFocus, c);
			}
		}
		else if (highlight)
		{
			AddHighlightOnFocus(_HighlightOnFocus, c);
		}
	}

	private void AddHighlightOnFocus(Dictionary<Control, bool> highlightOnFocus, Control c)
	{
		c.Enter += ControlHighlightEnter;
		c.Leave += ControlHighlightLeave;
		c.VisibleChanged += ControlHighlightVisibleChanged;
		highlightOnFocus.Add(c, value: true);
	}

	private void ControlHighlightVisibleChanged(object sender, EventArgs e)
	{
		if (_HighlightPanel != null && _HighlightPanel.FocusHighlightControl == sender)
		{
			UpdateHighlighterRegion();
		}
	}

	private void ControlHighlightLeave(object sender, EventArgs e)
	{
		if (_HighlightPanel != null)
		{
			_HighlightPanel.FocusHighlightControl = null;
		}
		UpdateHighlighterRegion();
	}

	private void ControlHighlightEnter(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (_HighlightPanel != null)
		{
			if (!((Control)_HighlightPanel).Visible)
			{
				((Control)_HighlightPanel).Visible = true;
			}
			((Control)_HighlightPanel).BringToFront();
			_HighlightPanel.FocusHighlightControl = (Control)sender;
		}
		UpdateHighlighterRegion();
	}

	private void RemoveHighlightOnFocus(Dictionary<Control, bool> highlightOnFocus, Control c)
	{
		c.Enter -= ControlHighlightEnter;
		c.Leave -= ControlHighlightLeave;
		c.VisibleChanged -= ControlHighlightVisibleChanged;
		highlightOnFocus.Remove(c);
	}

	[DefaultValue(eHighlightColor.None)]
	[Localizable(true)]
	[Description("Indicates the highlight color that is applied to the control.")]
	public eHighlightColor GetHighlightColor(Control c)
	{
		if (_Highlights.ContainsKey(c))
		{
			return _Highlights[c];
		}
		return eHighlightColor.None;
	}

	public void SetHighlightColor(Control c, eHighlightColor highlightColor)
	{
		if (_Highlights.ContainsKey(c))
		{
			if (highlightColor == eHighlightColor.None)
			{
				RemoveHighlight(_Highlights, c);
				return;
			}
			eHighlightColor eHighlightColor2 = _Highlights[c];
			RemoveHighlight(_Highlights, c);
			AddHighlight(_Highlights, c, highlightColor);
		}
		else if (highlightColor != 0)
		{
			AddHighlight(_Highlights, c, highlightColor);
		}
	}

	private void AddHighlight(Dictionary<Control, eHighlightColor> highlights, Control c, eHighlightColor highlightColor)
	{
		highlights.Add(c, highlightColor);
		c.LocationChanged += ControlLocationChanged;
		c.SizeChanged += ControlSizeChanged;
		c.VisibleChanged += ControlVisibleChanged;
		if (_HighlightPanel != null)
		{
			if (!((Control)_HighlightPanel).Visible)
			{
				((Control)_HighlightPanel).Visible = true;
			}
			((Control)_HighlightPanel).BringToFront();
		}
		if (c.Parent == null)
		{
			c.ParentChanged += ControlParentChanged;
		}
		else
		{
			AddTabControlHandlers(c);
		}
		UpdateHighlighterRegion();
	}

	private void ControlParentChanged(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Control val = (Control)sender;
		val.ParentChanged -= ControlParentChanged;
		AddTabControlHandlers(val);
	}

	private void AddTabControlHandlers(Control c)
	{
		if (GetParentControl(c, typeof(TabControl)) is TabControl tabControl)
		{
			if (_TabControl1.ContainsKey(tabControl))
			{
				_TabControl1[tabControl] += 1;
				return;
			}
			_TabControl1.Add(tabControl, 1);
			tabControl.SelectedTabChanged += TabControl1SelectedTabChanged;
			return;
		}
		if (GetParentControl(c, typeof(SuperTabControl)) is SuperTabControl superTabControl)
		{
			if (_SuperTabControl1.ContainsKey(superTabControl))
			{
				_SuperTabControl1[superTabControl] += 1;
				return;
			}
			_SuperTabControl1.Add(superTabControl, 1);
			superTabControl.SelectedTabChanged += SuperTabControl1SelectedTabChanged;
			return;
		}
		Control parentControl = GetParentControl(c, typeof(TabControl));
		TabControl val = (TabControl)(object)((parentControl is TabControl) ? parentControl : null);
		if (val != null)
		{
			if (_TabControl2.ContainsKey(val))
			{
				_TabControl2[val] += 1;
				return;
			}
			_TabControl2.Add(val, 1);
			val.SelectedIndexChanged += WinFormsTabSelectedIndexChanged;
			return;
		}
		Control parentControl2 = GetParentControl(c, typeof(Panel));
		Panel val2 = (Panel)(object)((parentControl2 is Panel) ? parentControl2 : null);
		if (val2 != null)
		{
			if (_ParentPanel.ContainsKey(val2))
			{
				_ParentPanel[val2] += 1;
				return;
			}
			_ParentPanel.Add(val2, 1);
			((Control)val2).Resize += ParentPanelResized;
			((Control)val2).LocationChanged += ParentPanelLocationChanged;
		}
	}

	private void ParentPanelLocationChanged(object sender, EventArgs e)
	{
		UpdateHighlights();
	}

	private void ParentPanelResized(object sender, EventArgs e)
	{
		UpdateHighlights();
	}

	private void WinFormsTabSelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private void TabControl1SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private void SuperTabControl1SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private Control GetParentControl(Control c, Type parentType)
	{
		for (Control parent = c.Parent; parent != null; parent = parent.Parent)
		{
			if (parentType.IsAssignableFrom(((object)parent).GetType()))
			{
				return parent;
			}
		}
		return null;
	}

	private void ControlVisibleChanged(object sender, EventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private void ControlSizeChanged(object sender, EventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private void ControlLocationChanged(object sender, EventArgs e)
	{
		UpdateHighlighterRegion();
	}

	private void UpdateHighlighterRegion()
	{
		if (_HighlightPanel != null)
		{
			_HighlightPanel.UpdateRegion();
		}
	}

	public void UpdateHighlights()
	{
		UpdateHighlighterRegion();
	}

	private void RemoveHighlight(Dictionary<Control, eHighlightColor> highlights, Control c)
	{
		highlights.Remove(c);
		c.LocationChanged -= ControlLocationChanged;
		c.SizeChanged -= ControlSizeChanged;
		c.VisibleChanged -= ControlVisibleChanged;
		if (GetParentControl(c, typeof(TabControl)) is TabControl tabControl)
		{
			if (_TabControl1.ContainsKey(tabControl))
			{
				if (_TabControl1[tabControl] == 1)
				{
					_TabControl1.Remove(tabControl);
					tabControl.SelectedTabChanged -= TabControl1SelectedTabChanged;
				}
				else
				{
					_TabControl1[tabControl] -= 1;
				}
			}
		}
		else if (GetParentControl(c, typeof(SuperTabControl)) is SuperTabControl superTabControl)
		{
			if (_SuperTabControl1.ContainsKey(superTabControl))
			{
				if (_SuperTabControl1[superTabControl] == 1)
				{
					_SuperTabControl1.Remove(superTabControl);
					superTabControl.SelectedTabChanged -= SuperTabControl1SelectedTabChanged;
				}
				else
				{
					_SuperTabControl1[superTabControl] -= 1;
				}
			}
		}
		else
		{
			Control parentControl = GetParentControl(c, typeof(TabControl));
			TabControl val = (TabControl)(object)((parentControl is TabControl) ? parentControl : null);
			if (val != null)
			{
				if (_TabControl2.ContainsKey(val))
				{
					if (_TabControl2[val] == 1)
					{
						_TabControl2.Remove(val);
						val.SelectedIndexChanged -= WinFormsTabSelectedIndexChanged;
					}
					else
					{
						_TabControl2[val] -= 1;
					}
				}
			}
			else
			{
				Control parentControl2 = GetParentControl(c, typeof(Panel));
				Panel val2 = (Panel)(object)((parentControl2 is Panel) ? parentControl2 : null);
				if (val2 != null && _ParentPanel.ContainsKey(val2))
				{
					if (_ParentPanel[val2] == 1)
					{
						_ParentPanel.Remove(val2);
						((Control)val2).LocationChanged -= ParentPanelLocationChanged;
						((Control)val2).SizeChanged -= ParentPanelResized;
					}
					else
					{
						_ParentPanel[val2] -= 1;
					}
				}
			}
		}
		UpdateHighlighterRegion();
	}

	private void UpdateHighlightPanelBounds()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(0, 0, _ContainerControl.ClientRectangle.Width, _ContainerControl.ClientRectangle.Height);
		if (((Control)_HighlightPanel).Parent is Form)
		{
			Control parent = ((Control)_HighlightPanel).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			if (((Control)val).AutoSize)
			{
				int x = rectangle.X;
				Padding padding = ((Control)val).Padding;
				rectangle.X = x + ((Padding)(ref padding)).Left;
				int y = rectangle.Y;
				padding = ((Control)val).Padding;
				rectangle.Y = y + ((Padding)(ref padding)).Top;
				int width = rectangle.Width;
				padding = ((Control)val).Padding;
				rectangle.Width = width - ((Padding)(ref padding)).Horizontal;
				int height = rectangle.Height;
				padding = ((Control)val).Padding;
				rectangle.Height = height - ((Padding)(ref padding)).Vertical;
			}
		}
		if (((Control)_HighlightPanel).Bounds.Equals((object?)rectangle))
		{
			_HighlightPanel.UpdateRegion();
		}
		else
		{
			((Control)_HighlightPanel).Bounds = rectangle;
		}
		((Control)_HighlightPanel).BringToFront();
	}

	private void ContainerControlSizeChanged(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (!BarFunctions.IsVista)
		{
			Form val = (Form)((sender is Form) ? sender : null);
			if (val != null)
			{
				if (_DelayTimer == null)
				{
					_DelayTimer = new Timer();
					_DelayTimer.Interval = 100;
					_DelayTimer.Tick += DelayTimerTick;
					_DelayTimer.Start();
				}
				return;
			}
		}
		UpdateHighlightPanelBounds();
	}

	private void DelayTimerTick(object sender, EventArgs e)
	{
		Timer delayTimer = _DelayTimer;
		_DelayTimer = null;
		delayTimer.Tick -= DelayTimerTick;
		delayTimer.Stop();
		((Component)(object)delayTimer).Dispose();
		UpdateHighlightPanelBounds();
	}

	public bool CanExtend(object extendee)
	{
		return extendee is Control;
	}

	void IErrorProvider.SetError(Control control, string value)
	{
		SetHighlightColor(control, eHighlightColor.Red);
	}

	void IErrorProvider.ClearError(Control control)
	{
		SetHighlightColor(control, eHighlightColor.None);
	}
}
