using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

public class SuperTabItemBaseDisplay : IDisposable
{
	private SuperTabItem _ᑚ;

	private Bitmap __1FD5;

	private Bitmap _ῖ;

	private Bitmap _ῗ;

	private Color _Ῐ;

	private SuperTabItemColorTable _Ῑ;

	private SuperTabPanelColorTable _Ὶ;

	private bool _Ί;

	private Image _ອ;

	internal SuperTabItemColorTable ΐ
	{
		get
		{
			return _Ῑ;
		}
		set
		{
			_Ῑ = value;
		}
	}

	internal SuperTabItem ᑸ => _ᑚ;

	internal SuperTabStripItem _1FD4 => _ᑚ.TabStripItem;

	internal ῲ έ => _ᑚ.TabStripItem.έ;

	public SuperTabItemBaseDisplay(SuperTabItem tabItem)
	{
		_ᑚ = tabItem;
		_1FDF();
	}

	public virtual void Paint(ItemPaintArgs p)
	{
		if (!ᑸ.Visible)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		if (!_1FD4.ᾰ(ᑸ, graphics))
		{
			SuperTabItemStateColorTable superTabItemStateColorTable = ὠ();
			GraphicsPath val = _1FDE();
			try
			{
				DrawTabItemBackground(graphics, val, superTabItemStateColorTable);
				DrawTabContent(graphics, superTabItemStateColorTable);
				DrawTabBorder(graphics, val, superTabItemStateColorTable);
				_1FD4.ᾱ(ᑸ, graphics);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	internal virtual Rectangle _1FDC()
	{
		return _ᑚ.DisplayRectangle;
	}

	internal Rectangle _1FDD()
	{
		return _1FD4.ᾲ(_ᑚ);
	}

	internal virtual GraphicsPath _1FDE()
	{
		return _1FD4.ᾳ(_ᑚ);
	}

	internal void _1FDF()
	{
		_Ῑ = ጦ.ጨ(ᑸ.TabStyle, ColorFactory.Empty);
		_Ὶ = ጦ.ጩ(ᑸ.TabStyle, ColorFactory.Empty);
	}

	internal virtual SuperTabItemStateColorTable ὠ()
	{
		eTabState ὡ = ῠ();
		return ὠ(ὡ);
	}

	private eTabState ῠ()
	{
		if (!ᑸ.Enabled)
		{
			return eTabState.Disabled;
		}
		if (ᑸ == _1FD4.Ὧ)
		{
			if (!ᑸ.IsSelected)
			{
				return eTabState.MouseOver;
			}
			return eTabState.SelectedMouseOver;
		}
		if (!ᑸ.IsSelected)
		{
			return eTabState.Default;
		}
		return eTabState.Selected;
	}

	internal virtual SuperTabItemStateColorTable ὠ(eTabState ὡ)
	{
		SuperTabItemStateColorTable superTabItemStateColorTable = ὢ(ὡ);
		if (superTabItemStateColorTable.SelectionMarker.IsEmpty && (ὡ == eTabState.Selected || ὡ == eTabState.SelectedMouseOver))
		{
			using SuperTabColorTable superTabColorTable = έ.ጧ();
			superTabItemStateColorTable.SelectionMarker = superTabColorTable.SelectionMarker;
		}
		return superTabItemStateColorTable;
	}

	internal SuperTabItemStateColorTable ὢ(eTabState ὡ)
	{
		SuperTabItemStateColorTable superTabItemStateColorTable = ῡ(ὡ, _Ῑ.Default);
		SuperTabItemStateColorTable superTabItemStateColorTable2 = ῡ(ὡ, ᑸ.TabColor.Default);
		superTabItemStateColorTable = (SuperTabItemStateColorTable)superTabItemStateColorTable.Clone();
		superTabItemStateColorTable2 = (SuperTabItemStateColorTable)superTabItemStateColorTable2.Clone();
		switch (ᑸ.TabAlignment)
		{
		case eTabStripAlignment.Left:
			ΰ(superTabItemStateColorTable, ῡ(ὡ, _Ῑ.Left));
			ΰ(superTabItemStateColorTable2, ῡ(ὡ, ᑸ.TabColor.Left));
			break;
		case eTabStripAlignment.Bottom:
			ΰ(superTabItemStateColorTable, ῡ(ὡ, _Ῑ.Bottom));
			ΰ(superTabItemStateColorTable2, ῡ(ὡ, ᑸ.TabColor.Bottom));
			break;
		case eTabStripAlignment.Right:
			ΰ(superTabItemStateColorTable, ῡ(ὡ, _Ῑ.Right));
			ΰ(superTabItemStateColorTable2, ῡ(ὡ, ᑸ.TabColor.Right));
			break;
		}
		ῢ(ὡ, superTabItemStateColorTable);
		ΰ(superTabItemStateColorTable, superTabItemStateColorTable2);
		return superTabItemStateColorTable;
	}

	private SuperTabItemStateColorTable ῡ(eTabState ὡ, SuperTabColorStates ጎ)
	{
		return ὡ switch
		{
			eTabState.MouseOver => ጎ.MouseOver, 
			eTabState.Selected => ጎ.Selected, 
			eTabState.SelectedMouseOver => ጎ.SelectedMouseOver, 
			eTabState.Disabled => ጎ.Disabled, 
			_ => ጎ.Normal, 
		};
	}

	internal virtual void ῢ(eTabState ὡ, SuperTabItemStateColorTable ና)
	{
		if (ᑸ.PredefinedColor != 0)
		{
			SuperTabItemColorTable superTabItemColorTable = ጦ.ፐ(ᑸ.PredefinedColor, ColorFactory.Empty);
			SuperTabItemStateColorTable superTabItemStateColorTable = superTabItemColorTable.Default.Normal;
			switch (ὡ)
			{
			case eTabState.SelectedMouseOver:
				superTabItemStateColorTable = superTabItemColorTable.Default.SelectedMouseOver;
				break;
			case eTabState.MouseOver:
				superTabItemStateColorTable = superTabItemColorTable.Default.MouseOver;
				break;
			case eTabState.Selected:
				superTabItemStateColorTable = superTabItemColorTable.Default.Selected;
				break;
			}
			ና.Text = superTabItemStateColorTable.Text;
			ና.InnerBorder = superTabItemStateColorTable.InnerBorder;
			ና.OuterBorder = superTabItemStateColorTable.OuterBorder;
			ና.Background = superTabItemStateColorTable.Background;
			ና.CloseMarker = superTabItemStateColorTable.CloseMarker;
			ና.SelectionMarker = superTabItemStateColorTable.SelectionMarker;
		}
	}

	private void ΰ(SuperTabItemStateColorTable ና, SuperTabItemStateColorTable ῤ)
	{
		if (!ῤ.Background.IsEmpty)
		{
			if (!ῤ.Background.IsEmpty)
			{
				if (ῤ.Background.Colors != null)
				{
					ና.Background.Colors = ῤ.Background.Colors;
				}
				if (ῤ.Background.Positions != null)
				{
					ና.Background.Positions = ῤ.Background.Positions;
				}
				if (ῤ.Background.AdaptiveGradient.HasValue)
				{
					ና.Background.AdaptiveGradient = ῤ.Background.AdaptiveGradient;
				}
			}
			ና.Background.GradientAngle = ῤ.Background.GradientAngle;
		}
		if (ῤ.InnerBorder != Color.Empty)
		{
			ና.InnerBorder = ῤ.InnerBorder;
		}
		if (ῤ.OuterBorder != Color.Empty)
		{
			ና.OuterBorder = ῤ.OuterBorder;
		}
		if (ῤ.Text != Color.Empty)
		{
			ና.Text = ῤ.Text;
		}
		if (ῤ.CloseMarker != Color.Empty)
		{
			ና.CloseMarker = ῤ.CloseMarker;
		}
		if (ῤ.SelectionMarker != Color.Empty)
		{
			ና.SelectionMarker = ῤ.SelectionMarker;
		}
	}

	internal virtual SuperTabPanelItemColorTable ጩ()
	{
		SuperTabPanelItemColorTable superTabPanelItemColorTable = (SuperTabPanelItemColorTable)_Ὶ.Default.Clone();
		if (ᑸ.AttachedControl is SuperTabControlPanel superTabControlPanel)
		{
			SuperTabPanelItemColorTable superTabPanelItemColorTable2 = (SuperTabPanelItemColorTable)superTabControlPanel.PanelColor.Default.Clone();
			switch (ᑸ.TabAlignment)
			{
			case eTabStripAlignment.Left:
				ῦ(superTabPanelItemColorTable, _Ὶ.Left);
				ῦ(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Left);
				break;
			case eTabStripAlignment.Bottom:
				ῦ(superTabPanelItemColorTable, _Ὶ.Bottom);
				ῦ(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Bottom);
				break;
			case eTabStripAlignment.Right:
				ῦ(superTabPanelItemColorTable, _Ὶ.Right);
				ῦ(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Right);
				break;
			}
			ῢ(superTabPanelItemColorTable);
			ῦ(superTabPanelItemColorTable, superTabPanelItemColorTable2);
		}
		return superTabPanelItemColorTable;
	}

	internal virtual void ῢ(SuperTabPanelItemColorTable ῥ)
	{
		if (ᑸ.PredefinedColor != 0)
		{
			SuperTabPanelItemColorTable superTabPanelItemColorTable = ጦ.ፕ(ᑸ.PredefinedColor, ColorFactory.Empty);
			ῥ.InnerBorder = superTabPanelItemColorTable.InnerBorder;
			ῥ.OuterBorder = superTabPanelItemColorTable.OuterBorder;
			ῥ.Background = superTabPanelItemColorTable.Background;
		}
	}

	private void ῦ(SuperTabPanelItemColorTable ና, SuperTabPanelItemColorTable ῤ)
	{
		if (!ῤ.Background.IsEmpty)
		{
			if (ῤ.Background.Colors != null)
			{
				ና.Background.Colors = ῤ.Background.Colors;
			}
			if (ῤ.Background.Positions != null)
			{
				ና.Background.Positions = ῤ.Background.Positions;
			}
			if (ῤ.Background.AdaptiveGradient.HasValue)
			{
				ና.Background.AdaptiveGradient = ῤ.Background.AdaptiveGradient;
			}
			ና.Background.GradientAngle = ῤ.Background.GradientAngle;
		}
		if (!ῤ.InnerBorder.IsEmpty)
		{
			ና.InnerBorder = ῤ.InnerBorder;
		}
		if (!ῤ.OuterBorder.IsEmpty)
		{
			ና.OuterBorder = ῤ.OuterBorder;
		}
	}

	protected virtual void DrawTabItemBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors)
	{
		Rectangle ल = Rectangle.Round(path.GetBounds());
		if (ल.Width > 0 && ल.Height > 0 && tabColors.Background.Colors != null)
		{
			if (tabColors.Background.Colors.Length == 1)
			{
				ῧ(g, path, tabColors, ल);
			}
			else
			{
				Ῡ(g, path, tabColors, ल);
			}
		}
	}

	internal void ῧ(Graphics _٥, GraphicsPath _1A6D, SuperTabItemStateColorTable Ῠ, Rectangle ल)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Brush brush = Ῠ.Background.GetBrush(ल);
		try
		{
			if (brush == null)
			{
				return;
			}
			_٥.FillPath(brush, _1A6D);
			Pen val = new Pen(Ῠ.Background.Colors[0]);
			try
			{
				int num = ((!_ᑚ.IsSelected) ? 1 : 3);
				for (int i = 0; i < num; i++)
				{
					switch (_ᑚ.TabAlignment)
					{
					case eTabStripAlignment.Top:
						_٥.DrawLine(val, ल.X, ल.Bottom + i, ल.Right, ल.Bottom + i);
						break;
					case eTabStripAlignment.Left:
						_٥.DrawLine(val, ल.Right + i, ल.Top, ल.Right + i, ल.Bottom);
						break;
					case eTabStripAlignment.Bottom:
						_٥.DrawLine(val, ल.X, ल.Top - i, ल.Right, ल.Top - i);
						break;
					case eTabStripAlignment.Right:
						_٥.DrawLine(val, ल.X - i, ल.Top, ल.X - i, ल.Bottom);
						break;
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
	}

	internal void Ῡ(Graphics _٥, GraphicsPath _1A6D, SuperTabItemStateColorTable Ῠ, Rectangle ल)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		int num = Ῠ.Background.GradientAngle;
		if (Ῠ.Background.AdaptiveGradient != false)
		{
			switch (_ᑚ.TabAlignment)
			{
			case eTabStripAlignment.Left:
				num -= 90;
				break;
			case eTabStripAlignment.Bottom:
				num += 180;
				break;
			case eTabStripAlignment.Right:
				num += 90;
				break;
			}
		}
		LinearGradientBrush val = (LinearGradientBrush)Ῠ.Background.GetBrush(ल, num);
		try
		{
			if (val != null)
			{
				val.WrapMode = (WrapMode)3;
				_٥.FillPath((Brush)(object)val, _1A6D);
				SmoothingMode smoothingMode = _٥.SmoothingMode;
				_٥.SmoothingMode = (SmoothingMode)3;
				int num2 = ((!_ᑚ.IsSelected) ? 1 : 3);
				switch (_ᑚ.TabAlignment)
				{
				case eTabStripAlignment.Top:
					_٥.FillRectangle((Brush)(object)val, ल.X, ल.Bottom, ल.Width, num2);
					break;
				case eTabStripAlignment.Left:
					_٥.FillRectangle((Brush)(object)val, ल.Right, ल.Top, num2, ल.Height);
					break;
				case eTabStripAlignment.Bottom:
					_٥.FillRectangle((Brush)(object)val, ल.X, ल.Top - num2 + 1, ल.Width, num2);
					break;
				case eTabStripAlignment.Right:
					_٥.FillRectangle((Brush)(object)val, ल.X - num2 + 1, ल.Top, num2, ल.Height);
					break;
				}
				_٥.SmoothingMode = smoothingMode;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void DrawTabBorder(Graphics g, GraphicsPath path, SuperTabItemStateColorTable ct)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		if (!ct.InnerBorder.IsEmpty)
		{
			Pen val = new Pen(ct.InnerBorder, 2f);
			try
			{
				Region clip = g.Clip;
				g.SetClip(path);
				g.DrawPath(val, path);
				g.DrawPath(val, path);
				g.DrawPath(val, path);
				g.DrawPath(val, path);
				g.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!ct.OuterBorder.IsEmpty)
		{
			Pen val2 = new Pen(ct.OuterBorder);
			try
			{
				g.DrawPath(val2, path);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	protected virtual void DrawTabContent(Graphics g, SuperTabItemStateColorTable colors)
	{
		if (!Ὺ(g))
		{
			DrawTabImage(g);
		}
		DrawTabText(g, colors);
		if (_1FD4.CloseButtonOnTabsVisible && ᑸ.CloseButtonVisible && (_1FD4.CloseButtonOnTabsAlwaysDisplayed || ᑸ.IsMouseOver))
		{
			DrawCloseButton(g, colors);
		}
	}

	private bool Ὺ(Graphics _٥)
	{
		if (!string.IsNullOrEmpty(ᑸ.SymbolRealized))
		{
			Font font = Symbols.GetFont(ᑸ.SymbolSize, ᑸ.SymbolSet);
			Color color = Ύ();
			Region clip = _٥.Clip;
			_٥.SetClip(ᑸ.ContentRectangle, (CombineMode)1);
			Rectangle bounds = _1FEE(_٥, ᑸ._1759);
			bounds.Height += 10;
			if (ᑸ.ᓙ && ((_1FD4.TabAlignment == eTabStripAlignment.Right && !_1FD4.RotateVerticalText) || (_1FD4.TabAlignment == eTabStripAlignment.Left && _1FD4.RotateVerticalText)))
			{
				bounds.X += _1FD4.TabHorizontalSpacing;
			}
			TextDrawing.DrawStringLegacy(_٥, ᑸ.SymbolRealized, font, color, bounds, eTextFormat.HorizontalCenter);
			_1FEF(_٥);
			_٥.Clip = clip;
			return true;
		}
		return false;
	}

	internal Color Ύ()
	{
		if (!ᑸ.SymbolColor.IsEmpty)
		{
			return ᑸ.SymbolColor;
		}
		eTabState ὡ = ῠ();
		SuperTabItemStateColorTable superTabItemStateColorTable = ᑸ.ὢ(ὡ);
		if (superTabItemStateColorTable.Text.IsEmpty)
		{
			return SystemColors.ControlDark;
		}
		return superTabItemStateColorTable.Text;
	}

	protected virtual void DrawTabImage(Graphics g)
	{
		ᇎ ᇎ2 = ᑸ.ᕂ();
		if (ᇎ2 == null)
		{
			return;
		}
		if (_Ί && !ᑸ._๗)
		{
			ໂ();
		}
		if (ᑸ._๗)
		{
			if (!_Ί)
			{
				ເ(ᇎ2.ᇑ);
			}
			ImageAnimator.UpdateFrames();
		}
		Region clip = g.Clip;
		g.SetClip(ᑸ.ContentRectangle, (CombineMode)1);
		Rectangle ल = ᑸ._1759;
		if (ᑸ.ᓙ)
		{
			int width = ल.Width;
			ल.Width = ल.Height;
			ल.Height = width;
		}
		Rectangle भ = _1FEE(g, ल);
		ᇎ2.ᇗ(g, भ);
		_1FEF(g);
		g.Clip = clip;
	}

	internal void ເ(Image ແ)
	{
		_Ί = true;
		_ອ = ແ;
		ImageAnimator.Animate(ແ, (EventHandler)ໃ);
	}

	internal void ໂ()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		if (_Ί)
		{
			_Ί = false;
			ImageAnimator.StopAnimate(_ອ, (EventHandler)ໃ);
			FrameDimension val = new FrameDimension(_ອ.FrameDimensionsList[0]);
			_ອ.SelectActiveFrame(val, 0);
		}
	}

	private void ໃ(object ໄ, EventArgs ؿ)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (_Ί && ᑸ.Visible && ᑸ.Parent != null)
		{
			ᑸ.Refresh();
			return;
		}
		_Ί = false;
		ImageAnimator.StopAnimate((Image)ໄ, (EventHandler)ໃ);
	}

	protected virtual void DrawTabText(Graphics g, SuperTabItemStateColorTable colors)
	{
		if (_1FD4.DisplaySelectedTextOnly && !ᑸ.IsSelected)
		{
			return;
		}
		Rectangle ल = ᑸ._175A;
		if (ल.Height <= 0 || ल.Width <= 0)
		{
			return;
		}
		Font tabFont = ᑸ.GetTabFont();
		if (!ᑸ.ᓙ)
		{
			if (ᑸ.TextMarkupBody != null)
			{
				ल = Ῥ(ल);
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(g, tabFont, colors.Text, ᑸ.IsRightToLeft);
				ᑸ.TextMarkupBody._0CC6(new Rectangle(ल.Location, ᑸ.TextMarkupBody.Bounds.Size), markupDrawContext);
				_1FED(g, ल, markupDrawContext);
			}
			else
			{
				ल = Ῥ(ल, out var ᔽ);
				TextDrawing.DrawString(g, ᑸ.Text, tabFont, colors.Text, ल, ᔽ);
			}
		}
		else
		{
			if (ᑸ.TextMarkupBody != null)
			{
				Rectangle bounds = ᑸ.TextMarkupBody.Bounds;
				ल = _1FEE(g, ल);
				ल = Ῥ(ल);
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(g, tabFont, colors.Text, ᑸ.IsRightToLeft);
				ᑸ.TextMarkupBody.Bounds = ल;
				_1FED(g, ल, markupDrawContext2);
				ᑸ.TextMarkupBody.Bounds = bounds;
			}
			else
			{
				ल = _1FEE(g, ल);
				ल = Ῥ(ल, out var ᔽ2);
				TextDrawing.DrawStringLegacy(g, ᑸ.Text, tabFont, colors.Text, ल, ᔽ2);
			}
			_1FEF(g);
		}
		if (έ.ῶ && ᘁ())
		{
			ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(_1FDD()));
		}
	}

	private Rectangle Ῥ(Rectangle ल, out eTextFormat ᔽ)
	{
		ᔽ = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		switch (ᑸ.TextAlignment.HasValue ? ᑸ.TextAlignment.Value : ᑸ.TabStripItem.TextAlignment)
		{
		case eItemAlignment.Center:
			ᔽ |= eTextFormat.HorizontalCenter;
			if (ᑸ.ᓙ)
			{
				int num = Dpi.Width(_1FD4.TabHorizontalSpacing);
				ल.X -= num;
				ल.Width += num;
			}
			break;
		case eItemAlignment.Far:
			ᔽ |= eTextFormat.Right;
			if (ᑸ.ᓙ)
			{
				ल.X -= Dpi.Width3;
				ल.Width -= Dpi.Width3;
			}
			break;
		}
		return ल;
	}

	private Rectangle Ῥ(Rectangle ल)
	{
		if (ल.Height > ᑸ.TextMarkupBody.Bounds.Height)
		{
			ल.Y += (ल.Height - ᑸ.TextMarkupBody.Bounds.Height) / 2;
		}
		if (ल.Width > ᑸ.TextMarkupBody.Bounds.Width)
		{
			switch (ᑸ.TextAlignment.HasValue ? ᑸ.TextAlignment.Value : ᑸ.TabStripItem.TextAlignment)
			{
			case eItemAlignment.Center:
				ल.X += (ल.Width - ᑸ.TextMarkupBody.Bounds.Width) / 2;
				break;
			case eItemAlignment.Far:
				ल.X += ल.Width - ᑸ.TextMarkupBody.Bounds.Width;
				break;
			}
		}
		return ल;
	}

	private void _1FED(Graphics _٥, Rectangle ल, MarkupDrawContext _0CC8)
	{
		Region clip = _٥.Clip;
		_٥.SetClip(ल, (CombineMode)0);
		ᑸ.TextMarkupBody.Render(_0CC8);
		_٥.Clip = clip;
	}

	private bool ᘁ()
	{
		if (ᑸ.IsSelected && !ᑸ.DesignMode)
		{
			return ((Control)ᑸ.Ὁ).ContainsFocus;
		}
		return false;
	}

	protected virtual void DrawCloseButton(Graphics g, SuperTabItemStateColorTable colors)
	{
		Rectangle _1369 = ᑸ._1369;
		if (ᑸ.ὃ || ᑸ.ὄ)
		{
			if (ᑸ.ὃ && ᑸ.ὄ)
			{
				if (_1FD4.TabCloseButtonPressed != null)
				{
					g.DrawImageUnscaled(_1FD4.TabCloseButtonPressed, _1369);
				}
				else
				{
					g.DrawImageUnscaled((Image)(object)GetCloseButtonPressedBitmap(g), _1369);
				}
			}
			else if (_1FD4.TabCloseButtonHot != null)
			{
				g.DrawImageUnscaled(_1FD4.TabCloseButtonHot, _1369);
			}
			else
			{
				g.DrawImageUnscaled((Image)(object)GetCloseButtonHotBitmap(g), _1369);
			}
		}
		else if (_1FD4.TabCloseButtonNormal != null)
		{
			g.DrawImageUnscaled(_1FD4.TabCloseButtonNormal, _1369);
		}
		else
		{
			g.DrawImageUnscaled((Image)(object)GetCloseButtonBitmap(g, colors), _1369);
		}
	}

	private Rectangle _1FEE(Graphics _٥, Rectangle ल)
	{
		if (ᑸ.ᓙ)
		{
			if (έ.ῳ == eTabStripAlignment.Left)
			{
				if (_1FD4.RotateVerticalText)
				{
					_٥.RotateTransform(90f);
					ल = new Rectangle(ल.Top, -ल.Right, ल.Height, ल.Width);
				}
				else
				{
					_٥.RotateTransform(-90f);
					ल = new Rectangle(-ल.Bottom, ल.X, ल.Height, ल.Width);
				}
			}
			else if (_1FD4.RotateVerticalText)
			{
				_٥.RotateTransform(-90f);
				ल = new Rectangle(-ल.Bottom, ल.X, ल.Height, ल.Width);
			}
			else
			{
				_٥.RotateTransform(90f);
				ल = new Rectangle(ल.Top, -ल.Right, ल.Height, ल.Width);
			}
		}
		return ल;
	}

	private void _1FEF(Graphics _٥)
	{
		if (ᑸ.ᓙ)
		{
			_٥.ResetTransform();
		}
	}

	protected virtual Rectangle GetFocusRectangle(Rectangle rText)
	{
		rText.Inflate(-3, -3);
		return rText;
	}

	public Bitmap GetCloseButtonBitmap(Graphics g, SuperTabItemStateColorTable colors)
	{
		if (_Ῐ != colors.CloseMarker || __1FD5 == null)
		{
			if (__1FD5 != null)
			{
				((Image)__1FD5).Dispose();
			}
			__1FD5 = CreateCloseButtonBitmap(g, colors);
			_Ῐ = colors.CloseMarker;
		}
		return __1FD5;
	}

	public virtual Bitmap CreateCloseButtonBitmap(Graphics g, SuperTabItemStateColorTable colors)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Pen val3 = new Pen(colors.CloseMarker);
			try
			{
				val2.DrawLine(val3, new Point(4, 4), new Point(10, 9));
				val2.DrawLine(val3, new Point(10, 4), new Point(4, 9));
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public Bitmap GetCloseButtonHotBitmap(Graphics g)
	{
		if (_ῖ == null)
		{
			_ῖ = CreateCloseButtonHotBitmap(g);
		}
		return _ῖ;
	}

	public virtual Bitmap CreateCloseButtonHotBitmap(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Rectangle rectangle = new Rectangle(0, 0, 15, 15);
			GraphicsPath val3 = _1FF0(rectangle, 4);
			try
			{
				LinearGradientBrush val4 = new LinearGradientBrush(rectangle, Color.White, Color.Pink, 90f);
				try
				{
					val2.FillPath((Brush)(object)val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				rectangle.Inflate(-1, -1);
				GraphicsPath val5 = _1FF0(rectangle, 6);
				try
				{
					LinearGradientBrush val6 = new LinearGradientBrush(rectangle, Color.Pink, Color.Crimson, 90f);
					try
					{
						val2.FillPath((Brush)(object)val6, val5);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val7 = new Pen(Color.White, 2f);
				try
				{
					val2.DrawLine(val7, new Point(4, 4), new Point(10, 10));
					val2.DrawLine(val7, new Point(10, 4), new Point(4, 10));
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				val2.DrawPath(Pens.DarkRed, val3);
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public Bitmap GetCloseButtonPressedBitmap(Graphics g)
	{
		if (_ῗ == null)
		{
			_ῗ = CreateCloseButtonPressedBitmap(g);
		}
		return _ῗ;
	}

	public virtual Bitmap CreateCloseButtonPressedBitmap(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Rectangle rectangle = new Rectangle(0, 0, 15, 15);
			GraphicsPath val3 = _1FF0(rectangle, 4);
			try
			{
				LinearGradientBrush val4 = new LinearGradientBrush(rectangle, Color.LightGray, Color.Red, 90f);
				try
				{
					val2.FillPath((Brush)(object)val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				rectangle.Inflate(-1, -1);
				GraphicsPath val5 = _1FF0(rectangle, 6);
				try
				{
					LinearGradientBrush val6 = new LinearGradientBrush(rectangle, Color.Red, Color.Maroon, 90f);
					try
					{
						val2.FillPath((Brush)(object)val6, val5);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val7 = new Pen(Color.LightGray, 2f);
				try
				{
					val2.DrawLine(val7, new Point(4, 4), new Point(10, 10));
					val2.DrawLine(val7, new Point(10, 4), new Point(4, 10));
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				val2.DrawPath(Pens.DarkRed, val3);
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private GraphicsPath _1FF0(Rectangle ल, int _1FF1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = new Rectangle(ल.Right - _1FF1, ल.Bottom - _1FF1, _1FF1, _1FF1);
		val.AddArc(rectangle, 0f, 90f);
		rectangle.X = ल.X;
		val.AddArc(rectangle, 90f, 90f);
		rectangle.Y = ल.Y;
		val.AddArc(rectangle, 180f, 90f);
		rectangle.X = ल.Right - _1FF1;
		val.AddArc(rectangle, 270f, 90f);
		val.CloseAllFigures();
		return val;
	}

	public void Dispose()
	{
		ໂ();
	}
}
