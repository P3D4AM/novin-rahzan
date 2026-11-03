using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class ItemPaintArgs
{
	public Graphics Graphics;

	public ColorScheme Colors;

	public Control ContainerControl;

	public eTextFormat ButtonStringFormat;

	public bool IsOnMenu;

	public bool IsOnPopupBar;

	public bool IsOnMenuBar;

	public bool IsOnRibbonBar;

	public bool IsOnNavigationBar;

	public Font Font;

	public IOwner Owner;

	public bool RightToLeft;

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7E _09F5;

	private _0A7F _09F6;

	private _0A80 _09F7;

	public bool DesignerSelection;

	public bool GlassEnabled;

	private BaseRenderer ਜ;

	public ButtonItemRendererEventArgs ButtonItemRendererEventArgs = new ButtonItemRendererEventArgs();

	public Rectangle ClipRectangle = Rectangle.Empty;

	public Rectangle ContainerClipRectangle = Rectangle.Empty;

	public bool ControlExpanded = true;

	internal bool ໟ;

	internal bool _0EE0;

	public bool IsFlatOffice2007Representation;

	public PaintEventArgs PaintEventArgs;

	internal bool _0EE1;

	internal List<ခ> _0EE2;

	public bool DragInProgress;

	public object ContextData;

	public object ContextData2;

	private bool __0EE3;

	internal BaseRenderer ໝ
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	internal _0A79 _0A79
	{
		get
		{
			if (ৰ == null)
			{
				if (ContainerControl is Bar)
				{
					ৰ = ((Bar)(object)ContainerControl)._0A79;
				}
				else if (ContainerControl is ል)
				{
					ৰ = ((ል)ContainerControl)._0A79;
				}
			}
			return ৰ;
		}
	}

	internal _0A7A _0A7A
	{
		get
		{
			if (ৱ == null)
			{
				if (ContainerControl is Bar)
				{
					ৱ = ((Bar)(object)ContainerControl)._0A7A;
				}
				else if (ContainerControl is ል)
				{
					ৱ = ((ል)ContainerControl)._0A7A;
				}
			}
			return ৱ;
		}
	}

	internal _0A7B _0A7B
	{
		get
		{
			if (_09F2 == null)
			{
				if (ContainerControl is Bar)
				{
					_09F2 = ((Bar)(object)ContainerControl)._0A7B;
				}
				else if (ContainerControl is ል)
				{
					_09F2 = ((ል)ContainerControl)._0A7B;
				}
			}
			return _09F2;
		}
	}

	internal _0A7C _0A7C
	{
		get
		{
			if (_09F3 == null)
			{
				if (ContainerControl is Bar)
				{
					_09F3 = ((Bar)(object)ContainerControl)._0A7C;
				}
				else if (ContainerControl is ል)
				{
					_09F3 = ((ል)ContainerControl)._0A7C;
				}
			}
			return _09F3;
		}
	}

	internal _0A7D _0A7D
	{
		get
		{
			if (_09F4 == null)
			{
				if (ContainerControl is Bar)
				{
					_09F4 = ((Bar)(object)ContainerControl)._0A7D;
				}
				else if (ContainerControl is ል)
				{
					_09F4 = ((ል)ContainerControl)._0A7D;
				}
			}
			return _09F4;
		}
	}

	internal _0A7E _0A7E
	{
		get
		{
			if (_09F5 == null && ContainerControl is ል)
			{
				_09F5 = ((ል)ContainerControl)._0A7E;
			}
			return _09F5;
		}
	}

	internal _0A7F _0A7F
	{
		get
		{
			if (_09F6 == null)
			{
				if (ContainerControl is Bar)
				{
					_09F6 = ((Bar)(object)ContainerControl)._0A7F;
				}
				else if (ContainerControl is ል)
				{
					_09F6 = ((ል)ContainerControl)._0A7F;
				}
			}
			return _09F6;
		}
	}

	internal _0A80 _0A80
	{
		get
		{
			if (_09F7 == null && ContainerControl is ል)
			{
				_09F7 = ((ል)ContainerControl)._0A80;
			}
			return _09F7;
		}
	}

	internal bool ໞ => __0EE3;

	public ItemPaintArgs(IOwner owner, Control control, Graphics g, ColorScheme scheme)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		Graphics = g;
		Colors = scheme;
		ContainerControl = control;
		Owner = owner;
		if (control != null)
		{
			RightToLeft = (int)control.RightToLeft == 1;
		}
		if (control is MenuPanel || ContainerControl is ᆓ)
		{
			IsOnMenu = true;
		}
		else if (control is Bar && ((Bar)(object)control).MenuBar)
		{
			IsOnMenuBar = true;
		}
		else if (control is Bar && ((Bar)(object)control).BarState == eBarState.Popup)
		{
			IsOnPopupBar = true;
		}
		else if (control is RibbonBar)
		{
			IsOnRibbonBar = true;
		}
		else if (control is NavigationBar)
		{
			IsOnNavigationBar = true;
		}
		else if (control is SuperTabControl && ((SuperTabControl)(object)control).TabStrip.ApplicationButton != null)
		{
			__0EE3 = true;
		}
		if (control != null)
		{
			Font = control.Font;
		}
		_0EE4();
	}

	private void _0EE4()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		if (ContainerControl is ItemControl && (Owner == null || !Owner.AlwaysDisplayKeyAccelerators))
		{
			eTextFormat2 |= eTextFormat.HidePrefix;
		}
		else if ((Owner == null || !Owner.AlwaysDisplayKeyAccelerators) && !_089F.ࢢ && !IsOnMenu)
		{
			Bar bar = ContainerControl as Bar;
			if ((ContainerControl == null || !ContainerControl.Focused) && (bar == null || !bar._09A9) && (bar == null || !bar.AlwaysDisplayKeyAccelerators) && !(ContainerControl is NavigationBar))
			{
				eTextFormat2 |= eTextFormat.HidePrefix;
			}
		}
		eTextFormat2 = ((!(ContainerControl is RibbonBar)) ? (eTextFormat2 | (eTextFormat.SingleLine | eTextFormat.VerticalCenter)) : (eTextFormat2 | eTextFormat.VerticalCenter));
		if (!TextDrawing.UseTextRenderer)
		{
			eTextFormat2 |= eTextFormat.EndEllipsis;
		}
		ButtonStringFormat = eTextFormat2;
	}
}
