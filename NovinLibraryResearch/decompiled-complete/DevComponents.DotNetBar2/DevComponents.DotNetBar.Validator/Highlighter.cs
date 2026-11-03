using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

[ProvideProperty("HighlightOnFocus", typeof(Control))]
[ComVisible(false)]
[ToolboxItem(true)]
[ProvideProperty("HighlightColor", typeof(Control))]
[ToolboxBitmap(typeof(Highlighter), "Validator.Highlighter.ico")]
[Designer("DevComponents.DotNetBar.Design.HighlighterDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class Highlighter : Component, IErrorProvider, IExtenderProvider
{
	private Dictionary<Control, eHighlightColor> __2FF2 = new Dictionary<Control, eHighlightColor>();

	private Dictionary<Control, bool> __2FF3 = new Dictionary<Control, bool>();

	private Dictionary<TabControl, int> __2FF4 = new Dictionary<TabControl, int>();

	private Dictionary<SuperTabControl, int> __2FF5 = new Dictionary<SuperTabControl, int>();

	private Dictionary<TabControl, int> __2FF6 = new Dictionary<TabControl, int>();

	private Dictionary<Panel, int> __2FF7 = new Dictionary<Panel, int>();

	private eHighlightColor _ᚾ = eHighlightColor.Blue;

	private _2FFB __2FF8;

	private Control __2FF0;

	private Timer __2FF9;

	private Color[] __2FFA;

	internal Dictionary<Control, eHighlightColor> Highlights => __2FF2;

	[DefaultValue(eHighlightColor.Blue)]
	[Category("Appearance")]
	[Localizable(true)]
	[Description("Indicates the highlight focus color.")]
	public eHighlightColor FocusHighlightColor
	{
		get
		{
			return _ᚾ;
		}
		set
		{
			_ᚾ = value;
			if (__2FF8 != null)
			{
				__2FF8._2FFD = value;
				UpdateHighlighterRegion();
			}
		}
	}

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates container control highlighter is bound to. Should be set to parent form.")]
	public Control ContainerControl
	{
		get
		{
			return __2FF0;
		}
		set
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			if (base.DesignMode)
			{
				__2FF0 = value;
			}
			else
			{
				if (__2FF0 == value)
				{
					return;
				}
				if (__2FF0 != null)
				{
					__2FF0.SizeChanged -= ContainerControlSizeChanged;
					__2FF0.HandleCreated -= ContainerControlHandleCreated;
					if (__2FF8 != null && ((Control)__2FF8).Parent == __2FF0)
					{
						__2FF0.Controls.Remove((Control)(object)__2FF8);
					}
				}
				__2FF0 = value;
				if (__2FF0 != null)
				{
					if (__2FF8 == null)
					{
						__2FF8 = new _2FFB(__2FF2);
						__2FF8._2FFD = _ᚾ;
						((Control)__2FF8).Margin = new Padding(0);
						((Control)__2FF8).Padding = new Padding(0);
						__2FF8._2FFE = __2FFA;
						((Control)__2FF8).Visible = false;
					}
					__2FF0.SizeChanged += ContainerControlSizeChanged;
					__2FF0.HandleCreated += ContainerControlHandleCreated;
					__2FF0.Controls.Add((Control)(object)__2FF8);
					UpdateHighlightPanelBounds();
				}
			}
		}
	}

	[DefaultValue(null)]
	[Description("Array of colors used to render custom highlight color. Control expects 3 colors in array to be specified which define the highlight border.")]
	[Category("Appearance")]
	public Color[] CustomHighlightColors
	{
		get
		{
			return __2FFA;
		}
		set
		{
			__2FFA = value;
			if (__2FF8 != null)
			{
				__2FF8._2FFE = __2FFA;
				((Control)__2FF8).Invalidate();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (__2FF0 != null)
		{
			__2FF0.SizeChanged -= ContainerControlSizeChanged;
			__2FF0.HandleCreated -= ContainerControlHandleCreated;
		}
		if (__2FF8 != null && ((Control)__2FF8).Parent == null && !((Control)__2FF8).IsDisposed)
		{
			((Component)(object)__2FF8).Dispose();
			__2FF8 = null;
		}
		else
		{
			__2FF8 = null;
		}
		base.Dispose(disposing);
	}

	[DefaultValue(false)]
	[Description("Indicates whether control is highlighted when it receives input focus.")]
	[Localizable(true)]
	public bool GetHighlightOnFocus(Control c)
	{
		if (__2FF3.ContainsKey(c))
		{
			return __2FF3[c];
		}
		return false;
	}

	public void SetHighlightOnFocus(Control c, bool highlight)
	{
		if (c == null)
		{
			throw new NullReferenceException();
		}
		if (__2FF3.ContainsKey(c))
		{
			if (!highlight)
			{
				RemoveHighlightOnFocus(__2FF3, c);
			}
		}
		else if (highlight)
		{
			AddHighlightOnFocus(__2FF3, c);
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
		if (__2FF8 != null && __2FF8._2FFC == sender)
		{
			UpdateHighlighterRegion();
		}
	}

	private void ControlHighlightLeave(object sender, EventArgs e)
	{
		if (__2FF8 != null)
		{
			__2FF8._2FFC = null;
		}
		UpdateHighlighterRegion();
	}

	private void ControlHighlightEnter(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (__2FF8 != null)
		{
			if (!((Control)__2FF8).Visible)
			{
				((Control)__2FF8).Visible = true;
			}
			((Control)__2FF8).BringToFront();
			__2FF8._2FFC = (Control)sender;
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

	[Localizable(true)]
	[DefaultValue(eHighlightColor.None)]
	[Description("Indicates the highlight color that is applied to the control.")]
	public eHighlightColor GetHighlightColor(Control c)
	{
		if (__2FF2.ContainsKey(c))
		{
			return __2FF2[c];
		}
		return eHighlightColor.None;
	}

	public void SetHighlightColor(Control c, eHighlightColor highlightColor)
	{
		if (__2FF2.ContainsKey(c))
		{
			if (highlightColor == eHighlightColor.None)
			{
				RemoveHighlight(__2FF2, c);
				return;
			}
			_ = __2FF2[c];
			RemoveHighlight(__2FF2, c);
			AddHighlight(__2FF2, c, highlightColor);
		}
		else if (highlightColor != 0)
		{
			AddHighlight(__2FF2, c, highlightColor);
		}
	}

	private void AddHighlight(Dictionary<Control, eHighlightColor> highlights, Control c, eHighlightColor highlightColor)
	{
		highlights.Add(c, highlightColor);
		c.LocationChanged += ControlLocationChanged;
		c.SizeChanged += ControlSizeChanged;
		c.VisibleChanged += ControlVisibleChanged;
		if (__2FF8 != null)
		{
			if (!((Control)__2FF8).Visible)
			{
				((Control)__2FF8).Visible = true;
			}
			((Control)__2FF8).BringToFront();
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
			if (__2FF4.ContainsKey(tabControl))
			{
				__2FF4[tabControl] += 1;
				return;
			}
			__2FF4.Add(tabControl, 1);
			tabControl.SelectedTabChanged += TabControl1SelectedTabChanged;
			return;
		}
		if (GetParentControl(c, typeof(SuperTabControl)) is SuperTabControl superTabControl)
		{
			if (__2FF5.ContainsKey(superTabControl))
			{
				__2FF5[superTabControl] += 1;
				return;
			}
			__2FF5.Add(superTabControl, 1);
			superTabControl.SelectedTabChanged += SuperTabControl1SelectedTabChanged;
			return;
		}
		Control parentControl = GetParentControl(c, typeof(TabControl));
		TabControl val = (TabControl)(object)((parentControl is TabControl) ? parentControl : null);
		if (val != null)
		{
			if (__2FF6.ContainsKey(val))
			{
				__2FF6[val] += 1;
				return;
			}
			__2FF6.Add(val, 1);
			val.SelectedIndexChanged += WinFormsTabSelectedIndexChanged;
			return;
		}
		Control parentControl2 = GetParentControl(c, typeof(Panel));
		Panel val2 = (Panel)(object)((parentControl2 is Panel) ? parentControl2 : null);
		if (val2 != null)
		{
			if (__2FF7.ContainsKey(val2))
			{
				__2FF7[val2] += 1;
				return;
			}
			__2FF7.Add(val2, 1);
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
		if (__2FF8 != null)
		{
			__2FF8.ᱎ();
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
			if (__2FF4.ContainsKey(tabControl))
			{
				if (__2FF4[tabControl] == 1)
				{
					__2FF4.Remove(tabControl);
					tabControl.SelectedTabChanged -= TabControl1SelectedTabChanged;
				}
				else
				{
					__2FF4[tabControl] -= 1;
				}
			}
		}
		else if (GetParentControl(c, typeof(SuperTabControl)) is SuperTabControl superTabControl)
		{
			if (__2FF5.ContainsKey(superTabControl))
			{
				if (__2FF5[superTabControl] == 1)
				{
					__2FF5.Remove(superTabControl);
					superTabControl.SelectedTabChanged -= SuperTabControl1SelectedTabChanged;
				}
				else
				{
					__2FF5[superTabControl] -= 1;
				}
			}
		}
		else
		{
			Control parentControl = GetParentControl(c, typeof(TabControl));
			TabControl val = (TabControl)(object)((parentControl is TabControl) ? parentControl : null);
			if (val != null)
			{
				if (__2FF6.ContainsKey(val))
				{
					if (__2FF6[val] == 1)
					{
						__2FF6.Remove(val);
						val.SelectedIndexChanged -= WinFormsTabSelectedIndexChanged;
					}
					else
					{
						__2FF6[val] -= 1;
					}
				}
			}
			else
			{
				Control parentControl2 = GetParentControl(c, typeof(Panel));
				Panel val2 = (Panel)(object)((parentControl2 is Panel) ? parentControl2 : null);
				if (val2 != null && __2FF7.ContainsKey(val2))
				{
					if (__2FF7[val2] == 1)
					{
						__2FF7.Remove(val2);
						((Control)val2).LocationChanged -= ParentPanelLocationChanged;
						((Control)val2).SizeChanged -= ParentPanelResized;
					}
					else
					{
						__2FF7[val2] -= 1;
					}
				}
			}
		}
		UpdateHighlighterRegion();
	}

	private void ContainerControlHandleCreated(object sender, EventArgs e)
	{
		if (__2FF2.Count > 0 && __2FF8 != null && !((Control)__2FF8).Visible)
		{
			((Control)__2FF8).Visible = true;
		}
	}

	private void UpdateHighlightPanelBounds()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(0, 0, __2FF0.ClientRectangle.Width, __2FF0.ClientRectangle.Height);
		if (((Control)__2FF8).Parent is Form)
		{
			Control parent = ((Control)__2FF8).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			if (((Control)val).AutoSize)
			{
				int x = rectangle.X;
				Padding padding = ((Control)val).Padding;
				rectangle.X = x + ((Padding)(ref padding)).Left;
				int y = rectangle.Y;
				Padding padding2 = ((Control)val).Padding;
				rectangle.Y = y + ((Padding)(ref padding2)).Top;
				int width = rectangle.Width;
				Padding padding3 = ((Control)val).Padding;
				rectangle.Width = width - ((Padding)(ref padding3)).Horizontal;
				int height = rectangle.Height;
				Padding padding4 = ((Control)val).Padding;
				rectangle.Height = height - ((Padding)(ref padding4)).Vertical;
			}
		}
		if (((Control)__2FF8).Bounds.Equals((object?)rectangle))
		{
			__2FF8.ᱎ();
		}
		else
		{
			((Control)__2FF8).Bounds = rectangle;
		}
		((Control)__2FF8).BringToFront();
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
				if (__2FF9 == null)
				{
					__2FF9 = new Timer();
					__2FF9.Interval = 100;
					__2FF9.Tick += DelayTimerTick;
					__2FF9.Start();
				}
				return;
			}
		}
		UpdateHighlightPanelBounds();
	}

	private void DelayTimerTick(object sender, EventArgs e)
	{
		Timer val = __2FF9;
		__2FF9 = null;
		val.Tick -= DelayTimerTick;
		val.Stop();
		((Component)(object)val).Dispose();
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
