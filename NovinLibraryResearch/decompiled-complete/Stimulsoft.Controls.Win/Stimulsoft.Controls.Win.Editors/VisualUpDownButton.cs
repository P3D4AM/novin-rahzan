using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualUpDownButton : VisualButtonBase
{
	private bool _MouseOverButtonUp;

	private bool _MouseOverButtonDown;

	private bool _MouseDownButtonUp;

	private bool _MouseDownButtonDown;

	private Rectangle _DownButtonRectangle = Rectangle.Empty;

	private Rectangle _UpButtonRectangle = Rectangle.Empty;

	private Image _UpImage;

	private Image _DownImage;

	private int _ButtonWidth = 15;

	private eUpDownButtonAutoChange _AutoChange;

	private VisualItem _AutoChangeItem;

	private bool _UpEnabled = true;

	private bool _DownEnabled = true;

	[DefaultValue(null)]
	public Image UpImage
	{
		get
		{
			return _UpImage;
		}
		set
		{
			if (_UpImage != value)
			{
				_UpImage = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(null)]
	public Image DownImage
	{
		get
		{
			return _DownImage;
		}
		set
		{
			if (_DownImage != value)
			{
				_DownImage = value;
				InvalidateArrange();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseOverButtonUp
	{
		get
		{
			return _MouseOverButtonUp;
		}
		set
		{
			if (_MouseOverButtonUp != value)
			{
				_MouseOverButtonUp = value;
				InvalidateRender();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseOverButtonDown
	{
		get
		{
			return _MouseOverButtonDown;
		}
		set
		{
			if (_MouseOverButtonDown != value)
			{
				_MouseOverButtonDown = value;
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
			return _MouseDownButtonDown;
		}
		set
		{
			if (_MouseDownButtonDown != value)
			{
				_MouseDownButtonDown = value;
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
			return _MouseDownButtonUp;
		}
		set
		{
			if (_MouseDownButtonUp != value)
			{
				_MouseDownButtonUp = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue(15)]
	public int ButtonWidth
	{
		get
		{
			return _ButtonWidth;
		}
		set
		{
			if (_ButtonWidth != value)
			{
				_ButtonWidth = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(eUpDownButtonAutoChange.None)]
	public eUpDownButtonAutoChange AutoChange
	{
		get
		{
			return _AutoChange;
		}
		set
		{
			_AutoChange = value;
		}
	}

	[DefaultValue(null)]
	public VisualItem AutoChangeItem
	{
		get
		{
			return _AutoChangeItem;
		}
		set
		{
			_AutoChangeItem = value;
		}
	}

	[DefaultValue(true)]
	public bool UpEnabled
	{
		get
		{
			return _UpEnabled;
		}
		set
		{
			if (_UpEnabled != value)
			{
				_UpEnabled = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue(true)]
	public bool DownEnabled
	{
		get
		{
			return _DownEnabled;
		}
		set
		{
			if (_DownEnabled != value)
			{
				_DownEnabled = value;
				InvalidateRender();
			}
		}
	}

	public event EventHandler UpClick;

	public event EventHandler DownClick;

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
		Size size = new Size(_ButtonWidth, num);
		if (_UpImage != null && _DownImage != null)
		{
			size.Width = Math.Max(_UpImage.Width, _DownImage.Width);
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
		if (_UpImage != null)
		{
			graphics.DrawImage(_UpImage, rectangle.Location);
		}
		else
		{
			Office2007ButtonItemStateColorTable office2007StateColorTableButtonUp = GetOffice2007StateColorTableButtonUp(p);
			PaintButtonBackground(p, office2007StateColorTableButtonUp, rectangle);
			SolidBrush val = new SolidBrush(office2007StateColorTableButtonUp.Text);
			try
			{
				p.Graphics.FillPolygon((Brush)(object)val, ButtonItemPainter.GetExpandPolygon(rectangle, ePopupSide.Top));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		_UpButtonRectangle = rectangle;
		rectangle = new Rectangle(renderBounds.X, rectangle.Bottom, renderBounds.Width, renderBounds.Height - rectangle.Height);
		if (_DownImage != null)
		{
			graphics.DrawImage(_DownImage, rectangle.Location);
		}
		else
		{
			Office2007ButtonItemStateColorTable office2007StateColorTableButtonDown = GetOffice2007StateColorTableButtonDown(p);
			PaintButtonBackground(p, office2007StateColorTableButtonDown, rectangle);
			SolidBrush val2 = new SolidBrush(office2007StateColorTableButtonDown.Text);
			try
			{
				p.Graphics.FillPolygon((Brush)(object)val2, ButtonItemPainter.GetExpandPolygon(rectangle, ePopupSide.Bottom));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		_DownButtonRectangle = rectangle;
		base.OnPaint(p);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			if (_DownButtonRectangle.Contains(e.X, e.Y))
			{
				MouseOverButtonDown = true;
				MouseOverButtonUp = false;
			}
			else if (_UpButtonRectangle.Contains(e.X, e.Y))
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
			ExecuteClickAction();
		}
		base.OnMouseDown(e);
	}

	private void ExecuteClickAction()
	{
		if (MouseOverButtonUp)
		{
			if (_AutoChange != 0 || _AutoChangeItem != null)
			{
				UpClickFocusedInputValue();
			}
			OnUpClick(new EventArgs());
		}
		else if (MouseOverButtonDown)
		{
			if (_AutoChange != 0 || _AutoChangeItem != null)
			{
				DownClickFocusedInputValue();
			}
			OnDownClick(new EventArgs());
		}
	}

	protected override void OnClickAutoRepeat()
	{
		ExecuteClickAction();
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

	private void DownClickFocusedInputValue()
	{
		VisualItem autoChangeItem = GetAutoChangeItem();
		if (autoChangeItem != null)
		{
			VisualInputGroup visualInputGroup = null;
			if (autoChangeItem.Parent is VisualInputGroup && !((VisualInputGroup)base.Parent).IsUserInput)
			{
				visualInputGroup = (VisualInputGroup)base.Parent;
				visualInputGroup.IsUserInput = true;
			}
			if (autoChangeItem is VisualNumericInput)
			{
				((VisualNumericInput)autoChangeItem).DecreaseValue();
			}
			else if (autoChangeItem is VisualListInput)
			{
				((VisualListInput)autoChangeItem).SelectNext();
			}
			if (visualInputGroup != null)
			{
				visualInputGroup.IsUserInput = false;
			}
		}
	}

	private void UpClickFocusedInputValue()
	{
		VisualItem autoChangeItem = GetAutoChangeItem();
		if (autoChangeItem != null)
		{
			VisualInputGroup visualInputGroup = null;
			if (autoChangeItem.Parent is VisualInputGroup && !((VisualInputGroup)base.Parent).IsUserInput)
			{
				visualInputGroup = (VisualInputGroup)base.Parent;
				visualInputGroup.IsUserInput = true;
			}
			if (autoChangeItem is VisualNumericInput)
			{
				((VisualNumericInput)autoChangeItem).IncreaseValue();
			}
			else if (autoChangeItem is VisualListInput)
			{
				((VisualListInput)autoChangeItem).SelectPrevious();
			}
			if (visualInputGroup != null)
			{
				visualInputGroup.IsUserInput = false;
			}
		}
	}

	private VisualItem GetAutoChangeItem()
	{
		if (_AutoChangeItem != null)
		{
			return _AutoChangeItem;
		}
		if (_AutoChange == eUpDownButtonAutoChange.None || base.Parent == null || base.Parent == null)
		{
			return null;
		}
		VisualGroup parent = base.Parent;
		if (_AutoChange == eUpDownButtonAutoChange.FocusedItem)
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
		if (this.UpClick != null)
		{
			this.UpClick(this, e);
		}
	}

	protected virtual void OnDownClick(EventArgs e)
	{
		if (this.DownClick != null)
		{
			this.DownClick(this, e);
		}
	}

	private bool RenderBackground(PaintInfo p)
	{
		if (base.RenderDefaultBackground)
		{
			return true;
		}
		if ((!p.MouseOver && !MouseDownButtonUp && !MouseDownButtonDown && !MouseOverButtonUp && !MouseOverButtonDown) || !GetIsEnabled())
		{
			return false;
		}
		return true;
	}

	protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct, Rectangle r)
	{
		Graphics graphics = p.Graphics;
		if (RenderBackground(p))
		{
			Office2007ButtonItemPainter.PaintBackground(graphics, ct, r, RoundRectangleShapeDescriptor.RectangleShape);
		}
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTableButtonUp(PaintInfo p)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!GetIsEnabled(p) || !_UpEnabled)
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
			if (!GetIsEnabled(p) || !_DownEnabled)
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
