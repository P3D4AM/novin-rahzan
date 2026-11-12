using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DualButton : BaseItem
{
	public class ExtraCommandSource : ICommandSource
	{
		private ICommand _Command;

		private object _CommandParameter;

		public ICommand Command
		{
			get
			{
				return _Command;
			}
			set
			{
				_Command = value;
			}
		}

		public object CommandParameter
		{
			get
			{
				return _CommandParameter;
			}
			set
			{
				_CommandParameter = value;
			}
		}

		public ExtraCommandSource(ICommand command, object commandParam)
		{
			_Command = command;
			_CommandParameter = commandParam;
		}
	}

	private Size _ButtonSize = new Size(38, 24);

	private GraphicsPath _TopPartPath;

	private GraphicsPath _BottomPartPath;

	private string _Text2 = string.Empty;

	private eDualButtonPart _MouseOverPart;

	private eDualButtonPart _LeftMouseButtonDown;

	private ICommand _Command2;

	private object _Command2Parameter;

	private eDualButtonPart _IsSelected;

	private Font _Font;

	public Size ButtonSize
	{
		get
		{
			return _ButtonSize;
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
			if (value != _ButtonSize)
			{
				Size buttonSize = _ButtonSize;
				_ButtonSize = value;
				OnButtonSizeChanged(buttonSize, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates second button part text.")]
	[Localizable(true)]
	public string Text2
	{
		get
		{
			return _Text2;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (value != _Text2)
			{
				string text = _Text2;
				_Text2 = value;
				OnText2Changed(text, value);
			}
		}
	}

	public eDualButtonPart MouseOverPart
	{
		get
		{
			return _MouseOverPart;
		}
		private set
		{
			if (value != _MouseOverPart)
			{
				eDualButtonPart mouseOverPart = _MouseOverPart;
				_MouseOverPart = value;
				OnMouseOverPartChanged(mouseOverPart, value);
			}
		}
	}

	public eDualButtonPart LeftMouseButtonDown
	{
		get
		{
			return _LeftMouseButtonDown;
		}
		private set
		{
			if (value != _LeftMouseButtonDown)
			{
				eDualButtonPart leftMouseButtonDown = _LeftMouseButtonDown;
				_LeftMouseButtonDown = value;
				OnLeftMouseButtonDownChanged(leftMouseButtonDown, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public virtual ICommand Command2
	{
		get
		{
			return _Command2;
		}
		set
		{
			bool flag = false;
			if (_Command2 != value)
			{
				flag = true;
			}
			if (_Command2 != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command2);
			}
			_Command2 = value;
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

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public virtual object Command2Parameter
	{
		get
		{
			return _Command2Parameter;
		}
		set
		{
			_Command2Parameter = value;
		}
	}

	[DefaultValue(eDualButtonPart.None)]
	[Category("Behavior")]
	[Description("Indicates selected part of button.")]
	public eDualButtonPart IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			if (value != _IsSelected)
			{
				eDualButtonPart isSelected = _IsSelected;
				_IsSelected = value;
				OnIsSelectedChanged(isSelected, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Gets or sets the text font.")]
	public Font Font
	{
		get
		{
			return _Font;
		}
		set
		{
			if (value != _Font)
			{
				Font font = _Font;
				_Font = value;
				OnFontChanged(font, value);
			}
		}
	}

	public DualButton()
	{
		RecreatePaths(_ButtonSize);
	}

	protected override void Dispose(bool disposing)
	{
		if (_TopPartPath != null)
		{
			_TopPartPath.Dispose();
			_TopPartPath = null;
		}
		if (_BottomPartPath != null)
		{
			_BottomPartPath.Dispose();
			_BottomPartPath = null;
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
		dualButton.ButtonSize = _ButtonSize;
		dualButton.Text2 = _Text2;
		base.CopyToItem(objCopy);
	}

	private void RecreatePaths(Size buttonSize)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		if (_TopPartPath != null)
		{
			_TopPartPath.Dispose();
		}
		if (_BottomPartPath != null)
		{
			_BottomPartPath.Dispose();
		}
		_TopPartPath = new GraphicsPath();
		_TopPartPath.AddLines(new PointF[3]
		{
			new PointF(0f, buttonSize.Height - 3),
			new PointF(0f, 0f),
			new PointF(buttonSize.Width - 2, 0f)
		});
		_TopPartPath.CloseAllFigures();
		_BottomPartPath = new GraphicsPath();
		_BottomPartPath.AddLines(new PointF[3]
		{
			new PointF(2f, buttonSize.Height - 1),
			new PointF(buttonSize.Width - 1, buttonSize.Height - 1),
			new PointF(buttonSize.Width - 1, 2f)
		});
		_BottomPartPath.CloseAllFigures();
	}

	protected virtual void OnButtonSizeChanged(Size oldValue, Size newValue)
	{
		RecreatePaths(newValue);
	}

	public override void RecalcSize()
	{
		m_Rect.Size = _ButtonSize;
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		Graphics graphics = p.Graphics;
		Matrix transform = graphics.Transform;
		graphics.TranslateTransform((float)m_Rect.X, (float)m_Rect.Y, (MatrixOrder)1);
		LinearGradientColorTable border = null;
		LinearGradientColorTable back = null;
		LinearGradientColorTable back2 = null;
		Color color = Color.Empty;
		if (_TopPartPath != null)
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable = GetOffice2007StateColorTable(p, eDualButtonPart.Up);
			if (office2007StateColorTable != null)
			{
				if (office2007StateColorTable.Background != null)
				{
					back = office2007StateColorTable.Background;
				}
				else
				{
					back = office2007StateColorTable.TopBackground;
					back2 = office2007StateColorTable.BottomBackground;
				}
				border = office2007StateColorTable.OuterBorder;
				color = office2007StateColorTable.Text;
			}
			DrawBackground(graphics, _TopPartPath, border, back, back2);
			Rectangle bounds = Rectangle.Round(_TopPartPath.GetBounds());
			bounds.Offset(0, -1);
			bounds.Width /= 2;
			TextDrawing.DrawString(graphics, Text, _Font ?? p.Font, color, bounds, eTextFormat.HorizontalCenter);
		}
		if (_BottomPartPath != null)
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable2 = GetOffice2007StateColorTable(p, eDualButtonPart.Down);
			back2 = null;
			if (office2007StateColorTable2 != null)
			{
				if (office2007StateColorTable2.Background != null)
				{
					back = office2007StateColorTable2.Background;
				}
				else
				{
					back = office2007StateColorTable2.TopBackground;
					back2 = office2007StateColorTable2.BottomBackground;
				}
				border = office2007StateColorTable2.OuterBorder;
				color = office2007StateColorTable2.Text;
			}
			DrawBackground(graphics, _BottomPartPath, border, back, back2);
			Rectangle bounds2 = Rectangle.Round(_BottomPartPath.GetBounds());
			bounds2.Offset(bounds2.Width / 2, 1);
			bounds2.Width /= 2;
			TextDrawing.DrawString(graphics, Text2, _Font ?? p.Font, color, bounds2, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
		}
		graphics.Transform = transform;
	}

	private Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(ItemPaintArgs p, eDualButtonPart part)
	{
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && p.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)p.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (_LeftMouseButtonDown == part)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (_MouseOverPart == part)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			if (IsSelected == part)
			{
				return office2007ButtonItemColorTable.Checked;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	private void DrawBackground(Graphics g, GraphicsPath path, LinearGradientColorTable border, LinearGradientColorTable back, LinearGradientColorTable back2)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (back != null && !back.IsEmpty)
		{
			if (back2 != null && !back2.IsEmpty)
			{
				LinearGradientBrush val = new LinearGradientBrush(path.GetBounds(), back.Start, back2.End, 90f);
				try
				{
					ColorBlend val2 = new ColorBlend(4);
					val2.Colors = new Color[4] { back.Start, back.End, back2.Start, back2.End };
					val2.Positions = new float[4] { 0f, 0.5f, 0.5f, 1f };
					val.InterpolationColors = val2;
					g.FillPath((Brush)(object)val, path);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				DisplayHelp.FillPath(g, path, back);
			}
		}
		if (border == null || border.IsEmpty)
		{
			return;
		}
		Brush val3 = DisplayHelp.CreateBrush(Rectangle.Round(path.GetBounds()), border);
		try
		{
			Pen val4 = new Pen(val3, 1f);
			try
			{
				g.DrawPath(val4, path);
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
		if (_LeftMouseButtonDown != 0 && _MouseOverPart != _LeftMouseButtonDown)
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
		if (_TopPartPath.IsVisible(point))
		{
			return eDualButtonPart.Up;
		}
		if (_BottomPartPath.IsVisible(point))
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
		if (_Command2 != null)
		{
			CommandManager.ExecuteCommand(new ExtraCommandSource(_Command2, _Command2Parameter));
		}
	}

	internal void ExecuteCommand2Internal()
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
