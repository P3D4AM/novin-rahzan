using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class DualButton : BaseItem
{
	public class ExtraCommandSource : ICommandSource
	{
		private ICommand _૦;

		private object _૧;

		public ICommand Command
		{
			get
			{
				return _૦;
			}
			set
			{
				_૦ = value;
			}
		}

		public object CommandParameter
		{
			get
			{
				return _૧;
			}
			set
			{
				_૧ = value;
			}
		}

		public ExtraCommandSource(ICommand command, object commandParam)
		{
			_૦ = command;
			_૧ = commandParam;
		}
	}

	private Size __268E = new Size(38, 24);

	private GraphicsPath __268F;

	private GraphicsPath __2690;

	private string __2691 = string.Empty;

	private eDualButtonPart __24EA;

	private eDualButtonPart __2692;

	private ICommand __2693;

	private object __2694;

	private eDualButtonPart _ԅ;

	private Font _ݐ;

	public Size ButtonSize
	{
		get
		{
			return __268E;
		}
		set
		{
			if (value.IsEmpty)
			{
				throw new ArgumentException("Empty size is not valid size");
			}
			if (value.Width < 1 || value.Width < 1)
			{
				throw new ArgumentException("Width or Height cannot be less than 1");
			}
			if (value != __268E)
			{
				Size oldValue = __268E;
				__268E = value;
				OnButtonSizeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates second button part text.")]
	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue("")]
	public string Text2
	{
		get
		{
			return __2691;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (value != __2691)
			{
				string oldValue = __2691;
				__2691 = value;
				OnText2Changed(oldValue, value);
			}
		}
	}

	public eDualButtonPart MouseOverPart
	{
		get
		{
			return __24EA;
		}
		private set
		{
			if (value != __24EA)
			{
				eDualButtonPart oldValue = __24EA;
				__24EA = value;
				OnMouseOverPartChanged(oldValue, value);
			}
		}
	}

	public eDualButtonPart LeftMouseButtonDown
	{
		get
		{
			return __2692;
		}
		private set
		{
			if (value != __2692)
			{
				eDualButtonPart oldValue = __2692;
				__2692 = value;
				OnLeftMouseButtonDownChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the command assigned to the item.")]
	[Category("Commands")]
	[DefaultValue(null)]
	public virtual ICommand Command2
	{
		get
		{
			return __2693;
		}
		set
		{
			bool flag = false;
			if (__2693 != value)
			{
				flag = true;
			}
			if (__2693 != null)
			{
				CommandManager.UnRegisterCommandSource(this, __2693);
			}
			__2693 = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommand2Changed();
			}
		}
	}

	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	[Browsable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Category("Commands")]
	[DefaultValue(null)]
	public virtual object Command2Parameter
	{
		get
		{
			return __2694;
		}
		set
		{
			__2694 = value;
		}
	}

	[Description("Indicates selected part of button.")]
	[DefaultValue(eDualButtonPart.None)]
	[Category("Behavior")]
	public eDualButtonPart IsSelected
	{
		get
		{
			return _ԅ;
		}
		set
		{
			if (value != _ԅ)
			{
				eDualButtonPart ԅ = _ԅ;
				_ԅ = value;
				OnIsSelectedChanged(ԅ, value);
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the text font.")]
	[Category("Appearance")]
	public Font Font
	{
		get
		{
			return _ݐ;
		}
		set
		{
			if (value != _ݐ)
			{
				Font ݐ = _ݐ;
				_ݐ = value;
				OnFontChanged(ݐ, value);
			}
		}
	}

	public DualButton()
	{
		_2695(__268E);
	}

	protected override void Dispose(bool disposing)
	{
		if (__268F != null)
		{
			__268F.Dispose();
			__268F = null;
		}
		if (__2690 != null)
		{
			__2690.Dispose();
			__2690 = null;
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		DualButton dualButton = new DualButton();
		CopyToItem(dualButton);
		return dualButton;
	}

	protected override void CopyToItem(BaseItem objCopy)
	{
		DualButton dualButton = (DualButton)objCopy;
		dualButton.ButtonSize = __268E;
		dualButton.Text2 = __2691;
		base.CopyToItem(objCopy);
	}

	private void _2695(Size _2696)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		if (__268F != null)
		{
			__268F.Dispose();
		}
		if (__2690 != null)
		{
			__2690.Dispose();
		}
		__268F = new GraphicsPath();
		__268F.AddLines(new PointF[3]
		{
			new PointF(0f, _2696.Height - 3),
			new PointF(0f, 0f),
			new PointF(_2696.Width - 2, 0f)
		});
		__268F.CloseAllFigures();
		__2690 = new GraphicsPath();
		__2690.AddLines(new PointF[3]
		{
			new PointF(2f, _2696.Height - 1),
			new PointF(_2696.Width - 1, _2696.Height - 1),
			new PointF(_2696.Width - 1, 2f)
		});
		__2690.CloseAllFigures();
	}

	protected virtual void OnButtonSizeChanged(Size oldValue, Size newValue)
	{
		_2695(newValue);
	}

	public override void RecalcSize()
	{
		m_Rect.Size = __268E;
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		Graphics graphics = p.Graphics;
		Matrix transform = graphics.Transform;
		graphics.TranslateTransform((float)m_Rect.X, (float)m_Rect.Y, (MatrixOrder)1);
		LinearGradientColorTable ധ = null;
		LinearGradientColorTable _2698 = null;
		LinearGradientColorTable _2699 = null;
		Color color = Color.Empty;
		if (__268F != null)
		{
			Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = _2697(p, eDualButtonPart.Up);
			if (office2007ButtonItemStateColorTable != null)
			{
				if (office2007ButtonItemStateColorTable.Background != null)
				{
					_2698 = office2007ButtonItemStateColorTable.Background;
				}
				else
				{
					_2698 = office2007ButtonItemStateColorTable.TopBackground;
					_2699 = office2007ButtonItemStateColorTable.BottomBackground;
				}
				ധ = office2007ButtonItemStateColorTable.OuterBorder;
				color = office2007ButtonItemStateColorTable.Text;
			}
			_0EE5(graphics, __268F, ധ, _2698, _2699);
			Rectangle bounds = Rectangle.Round(__268F.GetBounds());
			bounds.Offset(0, -1);
			bounds.Width /= 2;
			TextDrawing.DrawString(graphics, Text, _ݐ ?? p.Font, color, bounds, eTextFormat.HorizontalCenter);
		}
		if (__2690 != null)
		{
			Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable2 = _2697(p, eDualButtonPart.Down);
			_2699 = null;
			if (office2007ButtonItemStateColorTable2 != null)
			{
				if (office2007ButtonItemStateColorTable2.Background != null)
				{
					_2698 = office2007ButtonItemStateColorTable2.Background;
				}
				else
				{
					_2698 = office2007ButtonItemStateColorTable2.TopBackground;
					_2699 = office2007ButtonItemStateColorTable2.BottomBackground;
				}
				ധ = office2007ButtonItemStateColorTable2.OuterBorder;
				color = office2007ButtonItemStateColorTable2.Text;
			}
			_0EE5(graphics, __2690, ധ, _2698, _2699);
			Rectangle bounds2 = Rectangle.Round(__2690.GetBounds());
			bounds2.Offset(bounds2.Width / 2, 1);
			bounds2.Width /= 2;
			TextDrawing.DrawString(graphics, Text2, _ݐ ?? p.Font, color, bounds2, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
		}
		graphics.Transform = transform;
	}

	private Office2007ButtonItemStateColorTable _2697(ItemPaintArgs _065C, eDualButtonPart _070B)
	{
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && _065C.ໝ is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)_065C.ໝ).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (__2692 == _070B)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (__24EA == _070B)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			if (IsSelected == _070B)
			{
				return office2007ButtonItemColorTable.Checked;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	private void _0EE5(Graphics _٥, GraphicsPath _1A6D, LinearGradientColorTable ധ, LinearGradientColorTable _2698, LinearGradientColorTable _2699)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (_2698 != null && !_2698.IsEmpty)
		{
			if (_2699 != null && !_2699.IsEmpty)
			{
				LinearGradientBrush val = new LinearGradientBrush(_1A6D.GetBounds(), _2698.Start, _2699.End, 90f);
				try
				{
					ColorBlend val2 = new ColorBlend(4);
					val2.Colors = new Color[4] { _2698.Start, _2698.End, _2699.Start, _2699.End };
					val2.Positions = new float[4] { 0f, 0.5f, 0.5f, 1f };
					val.InterpolationColors = val2;
					_٥.FillPath((Brush)(object)val, _1A6D);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				DisplayHelp.FillPath(_٥, _1A6D, _2698);
			}
		}
		if (ധ == null || ധ.IsEmpty)
		{
			return;
		}
		Brush val3 = DisplayHelp.CreateBrush(Rectangle.Round(_1A6D.GetBounds()), ധ);
		try
		{
			Pen val4 = new Pen(val3, 1f);
			try
			{
				_٥.DrawPath(val4, _1A6D);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			LeftMouseButtonDown = HitTest(objArg.X, objArg.Y);
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			if (LeftMouseButtonDown != 0 && MouseOverPart == LeftMouseButtonDown)
			{
				if (LeftMouseButtonDown == eDualButtonPart.Up)
				{
					ExecuteCommand();
				}
				else
				{
					ExecuteCommand2();
				}
			}
			LeftMouseButtonDown = eDualButtonPart.None;
		}
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		MouseOverPart = HitTest(objArg.X, objArg.Y);
		if (__2692 != 0 && __24EA != __2692)
		{
			LeftMouseButtonDown = eDualButtonPart.None;
		}
		base.InternalMouseMove(objArg);
	}

	public eDualButtonPart HitTest(Point clientPoint)
	{
		return HitTest(clientPoint.X, clientPoint.Y);
	}

	public eDualButtonPart HitTest(int x, int y)
	{
		Point point = new Point(x - m_Rect.X, y - m_Rect.Y);
		if (__268F.IsVisible(point))
		{
			return eDualButtonPart.Up;
		}
		if (__2690.IsVisible(point))
		{
			return eDualButtonPart.Down;
		}
		return eDualButtonPart.None;
	}

	public override void InternalMouseLeave()
	{
		MouseOverPart = eDualButtonPart.None;
		LeftMouseButtonDown = eDualButtonPart.None;
		base.InternalMouseLeave();
	}

	protected virtual void OnText2Changed(string oldValue, string newValue)
	{
		Refresh();
	}

	protected virtual void OnMouseOverPartChanged(eDualButtonPart oldValue, eDualButtonPart newValue)
	{
		Refresh();
	}

	protected virtual void OnLeftMouseButtonDownChanged(eDualButtonPart oldValue, eDualButtonPart newValue)
	{
		Refresh();
	}

	protected virtual void ExecuteCommand2()
	{
		if (__2693 != null)
		{
			CommandManager._૭(new ExtraCommandSource(__2693, __2694));
		}
	}

	internal void _269A()
	{
		ExecuteCommand2();
	}

	protected virtual void OnCommand2Changed()
	{
	}

	protected virtual void OnIsSelectedChanged(eDualButtonPart oldValue, eDualButtonPart newValue)
	{
	}

	protected virtual void OnFontChanged(Font oldValue, Font newValue)
	{
	}
}
