using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors;

public class VisualUpDownButton : VisualButtonBase
{
	private bool __2336;

	private bool __2337;

	private bool __2338;

	private bool __2339;

	private Rectangle __233A = Rectangle.Empty;

	private Rectangle __233B = Rectangle.Empty;

	private EventHandler _233C;

	private EventHandler _233D;

	private Image __233E;

	private Image __233F;

	private int _ᢼ = 15;

	private eUpDownButtonAutoChange __2340;

	private VisualItem __2341;

	private bool __2342 = true;

	private bool __2343 = true;

	[DefaultValue(null)]
	public Image UpImage
	{
		get
		{
			return __233E;
		}
		set
		{
			if (__233E != value)
			{
				__233E = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(null)]
	public Image DownImage
	{
		get
		{
			return __233F;
		}
		set
		{
			if (__233F != value)
			{
				__233F = value;
				InvalidateArrange();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool MouseOverButtonUp
	{
		get
		{
			return __2336;
		}
		set
		{
			if (__2336 != value)
			{
				__2336 = value;
				InvalidateRender();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool MouseOverButtonDown
	{
		get
		{
			return __2337;
		}
		set
		{
			if (__2337 != value)
			{
				__2337 = value;
				InvalidateRender();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseDownButtonDown
	{
		get
		{
			return __2339;
		}
		set
		{
			if (__2339 != value)
			{
				__2339 = value;
				InvalidateRender();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseDownButtonUp
	{
		get
		{
			return __2338;
		}
		set
		{
			if (__2338 != value)
			{
				__2338 = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue(15)]
	public int ButtonWidth
	{
		get
		{
			return _ᢼ;
		}
		set
		{
			if (_ᢼ != value)
			{
				_ᢼ = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(eUpDownButtonAutoChange.None)]
	public eUpDownButtonAutoChange AutoChange
	{
		get
		{
			return __2340;
		}
		set
		{
			__2340 = value;
		}
	}

	[DefaultValue(null)]
	public VisualItem AutoChangeItem
	{
		get
		{
			return __2341;
		}
		set
		{
			__2341 = value;
		}
	}

	[DefaultValue(true)]
	public bool UpEnabled
	{
		get
		{
			return __2342;
		}
		set
		{
			if (__2342 != value)
			{
				__2342 = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue(true)]
	public bool DownEnabled
	{
		get
		{
			return __2343;
		}
		set
		{
			if (__2343 != value)
			{
				__2343 = value;
				InvalidateRender();
			}
		}
	}

	public event EventHandler UpClick
	{
		add
		{
			EventHandler eventHandler = _233C;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _233C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _233C;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _233C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler DownClick
	{
		add
		{
			EventHandler eventHandler = _233D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _233D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _233D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _233D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public VisualUpDownButton()
	{
		base.Focusable = false;
		base.ClickAutoRepeat = true;
	}

	public override void PerformLayout(PaintInfo pi)
	{
		int num = pi.AvailableSize.Height;
		if (num % 2 != 0)
		{
			num++;
		}
		Size size = new Size(Dpi.Width(_ᢼ), num);
		if (__233E != null && __233F != null)
		{
			size.Width = Math.Max(Dpi.Width(__233E.Width), Dpi.Width(__233F.Width));
		}
		base.Size = size;
		base.CalculatedSize = size;
		base.PerformLayout(pi);
	}

	protected override void OnPaint(PaintInfo p)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle renderBounds = base.RenderBounds;
		if (renderBounds.Width <= 0 || renderBounds.Height <= 0)
		{
			return;
		}
		Rectangle rectangle = new Rectangle(renderBounds.X, renderBounds.Y, renderBounds.Width, renderBounds.Height / 2);
		if (__233E != null)
		{
			graphics.DrawImage(__233E, rectangle.Location);
		}
		else
		{
			Office2007ButtonItemStateColorTable office2007StateColorTableButtonUp = GetOffice2007StateColorTableButtonUp(p);
			PaintButtonBackground(p, office2007StateColorTableButtonUp, rectangle);
			SolidBrush val = new SolidBrush(office2007StateColorTableButtonUp.Text);
			try
			{
				p.Graphics.FillPolygon((Brush)(object)val, _192F.ረ(rectangle, ePopupSide.Top));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		__233B = rectangle;
		rectangle = new Rectangle(renderBounds.X, rectangle.Bottom, renderBounds.Width, renderBounds.Height - rectangle.Height);
		if (__233F != null)
		{
			graphics.DrawImage(__233F, rectangle.Location);
		}
		else
		{
			Office2007ButtonItemStateColorTable office2007StateColorTableButtonDown = GetOffice2007StateColorTableButtonDown(p);
			PaintButtonBackground(p, office2007StateColorTableButtonDown, rectangle);
			SolidBrush val2 = new SolidBrush(office2007StateColorTableButtonDown.Text);
			try
			{
				p.Graphics.FillPolygon((Brush)(object)val2, _192F.ረ(rectangle, ePopupSide.Bottom));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		__233A = rectangle;
		base.OnPaint(p);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			if (__233A.Contains(e.X, e.Y))
			{
				MouseOverButtonDown = true;
				MouseOverButtonUp = false;
			}
			else if (__233B.Contains(e.X, e.Y))
			{
				MouseOverButtonUp = true;
				MouseOverButtonDown = false;
			}
			else
			{
				MouseOverButtonUp = false;
				MouseOverButtonDown = false;
			}
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && GetIsEnabled())
		{
			if (MouseOverButtonUp)
			{
				MouseDownButtonUp = true;
			}
			else if (MouseOverButtonDown)
			{
				MouseDownButtonDown = true;
			}
			_2344();
		}
		base.OnMouseDown(e);
	}

	private void _2344()
	{
		if (MouseOverButtonUp)
		{
			if (__2340 != 0 || __2341 != null)
			{
				_2346();
			}
			OnUpClick(new EventArgs());
		}
		else if (MouseOverButtonDown)
		{
			if (__2340 != 0 || __2341 != null)
			{
				_2345();
			}
			OnDownClick(new EventArgs());
		}
	}

	protected override void OnClickAutoRepeat()
	{
		_2344();
		base.OnClickAutoRepeat();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		MouseDownButtonDown = false;
		MouseDownButtonUp = false;
		base.OnMouseUp(e);
	}

	protected override void OnMouseLeave()
	{
		MouseOverButtonDown = false;
		MouseOverButtonUp = false;
		base.OnMouseLeave();
	}

	private void _2345()
	{
		VisualItem visualItem = _2347();
		if (visualItem != null)
		{
			VisualInputGroup visualInputGroup = null;
			if (visualItem.Parent is VisualInputGroup && !((VisualInputGroup)base.Parent).IsUserInput)
			{
				visualInputGroup = (VisualInputGroup)base.Parent;
				visualInputGroup.IsUserInput = true;
			}
			if (visualItem is VisualNumericInput)
			{
				((VisualNumericInput)visualItem).DecreaseValue();
			}
			else if (visualItem is VisualListInput)
			{
				((VisualListInput)visualItem).SelectNext();
			}
			if (visualInputGroup != null)
			{
				visualInputGroup.IsUserInput = false;
			}
		}
	}

	private void _2346()
	{
		VisualItem visualItem = _2347();
		if (visualItem != null)
		{
			VisualInputGroup visualInputGroup = null;
			if (visualItem.Parent is VisualInputGroup && !((VisualInputGroup)base.Parent).IsUserInput)
			{
				visualInputGroup = (VisualInputGroup)base.Parent;
				visualInputGroup.IsUserInput = true;
			}
			if (visualItem is VisualNumericInput)
			{
				((VisualNumericInput)visualItem).IncreaseValue();
			}
			else if (visualItem is VisualListInput)
			{
				((VisualListInput)visualItem).SelectPrevious();
			}
			if (visualInputGroup != null)
			{
				visualInputGroup.IsUserInput = false;
			}
		}
	}

	private VisualItem _2347()
	{
		if (__2341 != null)
		{
			return __2341;
		}
		if (__2340 == eUpDownButtonAutoChange.None || base.Parent == null || base.Parent == null)
		{
			return null;
		}
		VisualGroup parent = base.Parent;
		if (__2340 == eUpDownButtonAutoChange.FocusedItem)
		{
			if (parent.FocusedItem is VisualInputGroup)
			{
				VisualInputGroup visualInputGroup = parent.FocusedItem as VisualInputGroup;
				while (visualInputGroup.FocusedItem is VisualInputGroup)
				{
					visualInputGroup = visualInputGroup.FocusedItem as VisualInputGroup;
				}
				return visualInputGroup.FocusedItem;
			}
			if (!(parent.FocusedItem is VisualInputBase))
			{
				foreach (VisualItem item in parent.Items)
				{
					if (item is VisualInputBase && item.Enabled && item.Visible && item.Focusable)
					{
						return item;
					}
				}
			}
			return parent.FocusedItem;
		}
		int num = parent.Items.IndexOf(this);
		for (int num2 = num; num2 >= 0; num2--)
		{
			VisualItem visualItem2 = parent.Items[num2];
			if (visualItem2 is VisualInputBase && visualItem2.Visible)
			{
				return visualItem2;
			}
		}
		return null;
	}

	protected virtual void OnUpClick(EventArgs e)
	{
		if (_233C != null)
		{
			_233C(this, e);
		}
	}

	protected virtual void OnDownClick(EventArgs e)
	{
		if (_233D != null)
		{
			_233D(this, e);
		}
	}

	private bool ᶙ(PaintInfo _065C)
	{
		if (base.RenderDefaultBackground)
		{
			return true;
		}
		if ((!_065C.MouseOver && !MouseDownButtonUp && !MouseDownButtonDown && !MouseOverButtonUp && !MouseOverButtonDown) || !GetIsEnabled())
		{
			return false;
		}
		return true;
	}

	protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct, Rectangle r)
	{
		Graphics graphics = p.Graphics;
		if (ᶙ(p))
		{
			ᯊ.ᯓ(graphics, ct, r, RoundRectangleShapeDescriptor.RectangleShape);
		}
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTableButtonUp(PaintInfo p)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!GetIsEnabled(p) || !__2342)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (MouseDownButtonUp)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (MouseOverButtonUp)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTableButtonDown(PaintInfo p)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!GetIsEnabled(p) || !__2343)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (MouseDownButtonDown)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (MouseOverButtonDown)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}
}
