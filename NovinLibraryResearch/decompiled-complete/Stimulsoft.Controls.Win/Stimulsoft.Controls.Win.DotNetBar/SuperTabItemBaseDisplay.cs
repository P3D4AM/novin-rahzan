using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabItemBaseDisplay : IDisposable
{
	private SuperTabItem _TabItem;

	private Bitmap _CloseButtonBitmap;

	private Bitmap _CloseButtonHotBitmap;

	private Bitmap _CloseButtonPressedBitmap;

	private Color _CloseMarkerColor;

	private SuperTabItemColorTable _DefaultTabColorTable;

	private SuperTabPanelColorTable _DefaultPanelColorTable;

	private bool _CurrentlyAnimating;

	private Image _AnimateImage;

	internal SuperTabItemColorTable DefaultItemColorTable
	{
		get
		{
			return _DefaultTabColorTable;
		}
		set
		{
			_DefaultTabColorTable = value;
		}
	}

	internal SuperTabItem TabItem => _TabItem;

	internal SuperTabStripItem TabStripItem => _TabItem.TabStripItem;

	internal SuperTabStripBaseDisplay TabDisplay => _TabItem.TabStripItem.TabDisplay;

	public SuperTabItemBaseDisplay(SuperTabItem tabItem)
	{
		_TabItem = tabItem;
		SetDefaultColorTable();
	}

	public virtual void Paint(ItemPaintArgs p)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		if (!TabItem.Visible)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		if (!TabStripItem.OnPreRenderTabItem(TabItem, graphics))
		{
			SuperTabItemStateColorTable tabColorTable = GetTabColorTable();
			GraphicsPath val = TabItemPath();
			try
			{
				DrawTabItemBackground(graphics, val, tabColorTable);
				DrawTabContent(graphics, tabColorTable);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			TabStripItem.OnPostRenderTabItem(TabItem, graphics);
		}
		if (TabItem.EndGroup)
		{
			Pen val2 = new Pen(Color.White, 1f);
			try
			{
				graphics.DrawLine(val2, new PointF(TabItem.DisplayRectangle.X + StiScale2.XXI(10.0), TabItem.DisplayRectangle.Bottom), new PointF(TabItem.DisplayRectangle.Right - StiScale2.XXI(10.0), TabItem.DisplayRectangle.Bottom));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	internal virtual Rectangle ContentRectangle()
	{
		return _TabItem.DisplayRectangle;
	}

	internal Rectangle GetContentRectangle()
	{
		return TabStripItem.OnGetTabItemContentRectangle(_TabItem);
	}

	internal virtual GraphicsPath TabItemPath()
	{
		return TabStripItem.OnGetTabItemPath(_TabItem);
	}

	internal void SetDefaultColorTable()
	{
		_DefaultTabColorTable = SuperTabStyleColorFactory.GetItemColorTable(TabItem.TabStyle, ColorFactory.Empty);
		_DefaultPanelColorTable = SuperTabStyleColorFactory.GetPanelColorTable(TabItem.TabStyle, ColorFactory.Empty);
	}

	internal virtual SuperTabItemStateColorTable GetTabColorTable()
	{
		eTabState tabState = GetTabState();
		return GetTabColorTable(tabState);
	}

	private eTabState GetTabState()
	{
		if (!TabItem.Enabled)
		{
			return eTabState.Disabled;
		}
		if (TabItem == TabStripItem.HotTab)
		{
			if (!TabItem.IsSelected)
			{
				return eTabState.MouseOver;
			}
			return eTabState.SelectedMouseOver;
		}
		if (!TabItem.IsSelected)
		{
			return eTabState.Default;
		}
		return eTabState.Selected;
	}

	internal virtual SuperTabItemStateColorTable GetTabColorTable(eTabState tabState)
	{
		SuperTabItemStateColorTable stateColorTable = GetStateColorTable(tabState, _DefaultTabColorTable.Default);
		SuperTabItemStateColorTable stateColorTable2 = GetStateColorTable(tabState, TabItem.TabColor.Default);
		stateColorTable = (SuperTabItemStateColorTable)stateColorTable.Clone();
		stateColorTable2 = (SuperTabItemStateColorTable)stateColorTable2.Clone();
		switch (TabItem.TabAlignment)
		{
		case eTabStripAlignment.Left:
			ApplyTabColors(stateColorTable, GetStateColorTable(tabState, _DefaultTabColorTable.Left));
			ApplyTabColors(stateColorTable2, GetStateColorTable(tabState, TabItem.TabColor.Left));
			break;
		case eTabStripAlignment.Bottom:
			ApplyTabColors(stateColorTable, GetStateColorTable(tabState, _DefaultTabColorTable.Bottom));
			ApplyTabColors(stateColorTable2, GetStateColorTable(tabState, TabItem.TabColor.Bottom));
			break;
		case eTabStripAlignment.Right:
			ApplyTabColors(stateColorTable, GetStateColorTable(tabState, _DefaultTabColorTable.Right));
			ApplyTabColors(stateColorTable2, GetStateColorTable(tabState, TabItem.TabColor.Right));
			break;
		}
		ApplyPredefinedColor(tabState, stateColorTable);
		ApplyTabColors(stateColorTable, stateColorTable2);
		if (stateColorTable.SelectionMarker.IsEmpty && (tabState == eTabState.Selected || tabState == eTabState.SelectedMouseOver))
		{
			using SuperTabColorTable superTabColorTable = TabDisplay.GetColorTable();
			stateColorTable.SelectionMarker = superTabColorTable.SelectionMarker;
		}
		return stateColorTable;
	}

	private SuperTabItemStateColorTable GetStateColorTable(eTabState tabState, SuperTabColorStates cs)
	{
		return tabState switch
		{
			eTabState.MouseOver => cs.MouseOver, 
			eTabState.Selected => cs.Selected, 
			eTabState.SelectedMouseOver => cs.SelectedMouseOver, 
			eTabState.Disabled => cs.Disabled, 
			_ => cs.Normal, 
		};
	}

	internal virtual void ApplyPredefinedColor(eTabState tabState, SuperTabItemStateColorTable sct)
	{
		if (TabItem.PredefinedColor != 0)
		{
			SuperTabItemColorTable predefinedTabColors = SuperTabStyleColorFactory.GetPredefinedTabColors(TabItem.PredefinedColor, ColorFactory.Empty);
			SuperTabItemStateColorTable superTabItemStateColorTable = predefinedTabColors.Default.Normal;
			switch (tabState)
			{
			case eTabState.SelectedMouseOver:
				superTabItemStateColorTable = predefinedTabColors.Default.SelectedMouseOver;
				break;
			case eTabState.MouseOver:
				superTabItemStateColorTable = predefinedTabColors.Default.MouseOver;
				break;
			case eTabState.Selected:
				superTabItemStateColorTable = predefinedTabColors.Default.Selected;
				break;
			}
			sct.Text = superTabItemStateColorTable.Text;
			sct.InnerBorder = superTabItemStateColorTable.InnerBorder;
			sct.OuterBorder = superTabItemStateColorTable.OuterBorder;
			sct.Background = superTabItemStateColorTable.Background;
			sct.CloseMarker = superTabItemStateColorTable.CloseMarker;
			sct.SelectionMarker = superTabItemStateColorTable.SelectionMarker;
		}
	}

	private void ApplyTabColors(SuperTabItemStateColorTable sct, SuperTabItemStateColorTable tct)
	{
		if (!tct.Background.IsEmpty)
		{
			if (!tct.Background.IsEmpty)
			{
				if (tct.Background.Colors != null)
				{
					sct.Background.Colors = tct.Background.Colors;
				}
				if (tct.Background.Positions != null)
				{
					sct.Background.Positions = tct.Background.Positions;
				}
				if (tct.Background.AdaptiveGradient.HasValue)
				{
					sct.Background.AdaptiveGradient = tct.Background.AdaptiveGradient;
				}
			}
			sct.Background.GradientAngle = tct.Background.GradientAngle;
		}
		if (tct.InnerBorder != Color.Empty)
		{
			sct.InnerBorder = tct.InnerBorder;
		}
		if (tct.OuterBorder != Color.Empty)
		{
			sct.OuterBorder = tct.OuterBorder;
		}
		if (tct.Text != Color.Empty)
		{
			sct.Text = tct.Text;
		}
		if (tct.CloseMarker != Color.Empty)
		{
			sct.CloseMarker = tct.CloseMarker;
		}
		if (tct.SelectionMarker != Color.Empty)
		{
			sct.SelectionMarker = tct.SelectionMarker;
		}
	}

	internal virtual SuperTabPanelItemColorTable GetPanelColorTable()
	{
		SuperTabPanelItemColorTable superTabPanelItemColorTable = (SuperTabPanelItemColorTable)_DefaultPanelColorTable.Default.Clone();
		if (TabItem.AttachedControl is SuperTabControlPanel superTabControlPanel)
		{
			SuperTabPanelItemColorTable superTabPanelItemColorTable2 = (SuperTabPanelItemColorTable)superTabControlPanel.PanelColor.Default.Clone();
			switch (TabItem.TabAlignment)
			{
			case eTabStripAlignment.Left:
				ApplyPanelColors(superTabPanelItemColorTable, _DefaultPanelColorTable.Left);
				ApplyPanelColors(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Left);
				break;
			case eTabStripAlignment.Bottom:
				ApplyPanelColors(superTabPanelItemColorTable, _DefaultPanelColorTable.Bottom);
				ApplyPanelColors(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Bottom);
				break;
			case eTabStripAlignment.Right:
				ApplyPanelColors(superTabPanelItemColorTable, _DefaultPanelColorTable.Right);
				ApplyPanelColors(superTabPanelItemColorTable2, superTabControlPanel.PanelColor.Right);
				break;
			}
			ApplyPredefinedColor(superTabPanelItemColorTable);
			ApplyPanelColors(superTabPanelItemColorTable, superTabPanelItemColorTable2);
		}
		return superTabPanelItemColorTable;
	}

	internal virtual void ApplyPredefinedColor(SuperTabPanelItemColorTable pct)
	{
		if (TabItem.PredefinedColor != 0)
		{
			SuperTabPanelItemColorTable predefinedPanelColors = SuperTabStyleColorFactory.GetPredefinedPanelColors(TabItem.PredefinedColor, ColorFactory.Empty);
			pct.InnerBorder = predefinedPanelColors.InnerBorder;
			pct.OuterBorder = predefinedPanelColors.OuterBorder;
			pct.Background = predefinedPanelColors.Background;
		}
	}

	private void ApplyPanelColors(SuperTabPanelItemColorTable sct, SuperTabPanelItemColorTable tct)
	{
		if (!tct.Background.IsEmpty)
		{
			if (tct.Background.Colors != null)
			{
				sct.Background.Colors = tct.Background.Colors;
			}
			if (tct.Background.Positions != null)
			{
				sct.Background.Positions = tct.Background.Positions;
			}
			if (tct.Background.AdaptiveGradient.HasValue)
			{
				sct.Background.AdaptiveGradient = tct.Background.AdaptiveGradient;
			}
			sct.Background.GradientAngle = tct.Background.GradientAngle;
		}
		if (!tct.InnerBorder.IsEmpty)
		{
			sct.InnerBorder = tct.InnerBorder;
		}
		if (!tct.OuterBorder.IsEmpty)
		{
			sct.OuterBorder = tct.OuterBorder;
		}
	}

	protected virtual void DrawTabItemBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors)
	{
		Rectangle r = Rectangle.Round(path.GetBounds());
		if (r.Width > 0 && r.Height > 0 && tabColors.Background.Colors != null)
		{
			if (tabColors.Background.Colors.Length == 1)
			{
				DrawTabItemSolidBackground(g, path, tabColors, r);
			}
			else
			{
				DrawTabItemGradientBackground(g, path, tabColors, r);
			}
		}
	}

	internal void DrawTabItemSolidBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		Brush brush = tabColors.Background.GetBrush(r);
		try
		{
			if (brush == null)
			{
				return;
			}
			g.FillPath(brush, path);
			Pen val = new Pen(tabColors.Background.Colors[0]);
			try
			{
				int num = ((!_TabItem.IsSelected) ? 1 : 3);
				for (int i = 0; i < num; i++)
				{
					switch (_TabItem.TabAlignment)
					{
					case eTabStripAlignment.Top:
						g.DrawLine(val, r.X, r.Bottom + i, r.Right, r.Bottom + i);
						break;
					case eTabStripAlignment.Left:
						g.DrawLine(val, r.Right + i, r.Top, r.Right + i, r.Bottom);
						break;
					case eTabStripAlignment.Bottom:
						g.DrawLine(val, r.X, r.Top - i, r.Right, r.Top - i);
						break;
					case eTabStripAlignment.Right:
						g.DrawLine(val, r.X - i, r.Top, r.X - i, r.Bottom);
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

	internal void DrawTabItemGradientBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		int num = tabColors.Background.GradientAngle;
		if (tabColors.Background.AdaptiveGradient != false)
		{
			switch (_TabItem.TabAlignment)
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
		LinearGradientBrush val = (LinearGradientBrush)tabColors.Background.GetBrush(r, num);
		try
		{
			if (val != null)
			{
				val.WrapMode = (WrapMode)3;
				g.FillPath((Brush)(object)val, path);
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)3;
				int num2 = ((!_TabItem.IsSelected) ? 1 : 3);
				switch (_TabItem.TabAlignment)
				{
				case eTabStripAlignment.Top:
					g.FillRectangle((Brush)(object)val, r.X, r.Bottom, r.Width, num2);
					break;
				case eTabStripAlignment.Left:
					g.FillRectangle((Brush)(object)val, r.Right, r.Top, num2, r.Height);
					break;
				case eTabStripAlignment.Bottom:
					g.FillRectangle((Brush)(object)val, r.X, r.Top - num2 + 1, r.Width, num2);
					break;
				case eTabStripAlignment.Right:
					g.FillRectangle((Brush)(object)val, r.X - num2 + 1, r.Top, num2, r.Height);
					break;
				}
				g.SmoothingMode = smoothingMode;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void DrawTabBorder(Graphics g, GraphicsPath path, SuperTabItemStateColorTable ct)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
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

	protected virtual void DrawTabContent(Graphics g, SuperTabItemStateColorTable colors)
	{
		DrawTabImage(g);
		DrawTabText(g, colors);
		if (TabStripItem.CloseButtonOnTabsVisible && TabItem.CloseButtonVisible && (TabStripItem.CloseButtonOnTabsAlwaysDisplayed || TabItem.IsMouseOver))
		{
			DrawCloseButton(g, colors);
		}
	}

	protected virtual void DrawCloseButton(Graphics g, SuperTabItemStateColorTable colors)
	{
		Rectangle closeButtonBounds = TabItem.CloseButtonBounds;
		if (TabItem.CloseButtonMouseOver || TabItem.CloseButtonPressed)
		{
			if (TabItem.CloseButtonMouseOver && TabItem.CloseButtonPressed)
			{
				if (TabStripItem.TabCloseButtonPressed != null)
				{
					g.DrawImageUnscaled(TabStripItem.TabCloseButtonPressed, closeButtonBounds);
				}
				else
				{
					g.DrawImageUnscaled((Image)(object)GetCloseButtonPressedBitmap(g), closeButtonBounds);
				}
			}
			else if (TabStripItem.TabCloseButtonHot != null)
			{
				g.DrawImageUnscaled(TabStripItem.TabCloseButtonHot, closeButtonBounds);
			}
			else
			{
				g.DrawImageUnscaled((Image)(object)GetCloseButtonHotBitmap(g), closeButtonBounds);
			}
		}
		else if (TabStripItem.TabCloseButtonNormal != null)
		{
			g.DrawImageUnscaled(TabStripItem.TabCloseButtonNormal, closeButtonBounds);
		}
		else
		{
			g.DrawImageUnscaled((Image)(object)GetCloseButtonBitmap(g, colors), closeButtonBounds);
		}
	}

	protected virtual void DrawTabImage(Graphics g)
	{
		CompositeImage tabImage = TabItem.GetTabImage();
		if (tabImage == null)
		{
			return;
		}
		if (_CurrentlyAnimating && !TabItem.CanAnimateImage)
		{
			StopImageAnimation();
		}
		if (TabItem.CanAnimateImage)
		{
			if (!_CurrentlyAnimating)
			{
				StartImageAnimation(tabImage.Image);
			}
			ImageAnimator.UpdateFrames();
		}
		Region clip = g.Clip;
		g.SetClip(TabItem.ContentRectangle, (CombineMode)1);
		Rectangle rect = SetTransform(g, TabItem.ImageBounds);
		tabImage.DrawImage(g, rect);
		ResetTransform(g);
		g.Clip = clip;
	}

	internal void StartImageAnimation(Image image)
	{
		_CurrentlyAnimating = true;
		_AnimateImage = image;
		ImageAnimator.Animate(image, (EventHandler)OnFrameChanged);
	}

	internal void StopImageAnimation()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (_CurrentlyAnimating)
		{
			_CurrentlyAnimating = false;
			ImageAnimator.StopAnimate(_AnimateImage, (EventHandler)OnFrameChanged);
			FrameDimension val = new FrameDimension(_AnimateImage.FrameDimensionsList[0]);
			_AnimateImage.SelectActiveFrame(val, 0);
		}
	}

	private void OnFrameChanged(object o, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (_CurrentlyAnimating && TabItem.Visible && TabItem.Parent != null)
		{
			TabItem.Refresh();
			return;
		}
		_CurrentlyAnimating = false;
		ImageAnimator.StopAnimate((Image)o, (EventHandler)OnFrameChanged);
	}

	protected virtual void DrawTabText(Graphics g, SuperTabItemStateColorTable colors)
	{
		if (TabStripItem.DisplaySelectedTextOnly && !TabItem.IsSelected)
		{
			return;
		}
		Rectangle textBounds = TabItem.TextBounds;
		Font tabFont = TabItem.GetTabFont();
		if (!TabItem.IsVertical)
		{
			if (TabItem.TextMarkupBody != null)
			{
				textBounds = AlignText(textBounds);
				MarkupDrawContext d = new MarkupDrawContext(g, tabFont, colors.Text, TabItem.IsRightToLeft);
				TabItem.TextMarkupBody.Arrange(new Rectangle(textBounds.Location, TabItem.TextMarkupBody.Bounds.Size), d);
				RenderMarkup(g, textBounds, d);
			}
			else
			{
				textBounds = AlignText(textBounds, out var strFormat);
				TextDrawing.DrawString(g, TabItem.Text, tabFont, colors.Text, textBounds, strFormat);
			}
		}
		else
		{
			if (TabItem.TextMarkupBody != null)
			{
				Rectangle bounds = TabItem.TextMarkupBody.Bounds;
				textBounds = SetTransform(g, textBounds);
				textBounds = AlignText(textBounds);
				MarkupDrawContext d2 = new MarkupDrawContext(g, tabFont, colors.Text, TabItem.IsRightToLeft);
				TabItem.TextMarkupBody.Bounds = textBounds;
				RenderMarkup(g, textBounds, d2);
				TabItem.TextMarkupBody.Bounds = bounds;
			}
			else
			{
				textBounds = SetTransform(g, textBounds);
				textBounds = AlignText(textBounds, out var strFormat2);
				TextDrawing.DrawStringLegacy(g, TabItem.Text, tabFont, colors.Text, textBounds, strFormat2);
			}
			ResetTransform(g);
		}
		if (TabDisplay.ShowFocusRectangle && TabItem.IsSelected)
		{
			ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(TabItem.DisplayRectangle));
		}
	}

	private Rectangle AlignText(Rectangle r, out eTextFormat strFormat)
	{
		strFormat = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		switch (TabItem.TextAlignment.HasValue ? TabItem.TextAlignment.Value : TabItem.TabStripItem.TextAlignment)
		{
		case eItemAlignment.Center:
			strFormat |= eTextFormat.HorizontalCenter;
			if (TabItem.IsVertical)
			{
				r.X -= TabStripItem.TabHorizontalSpacing;
				r.Width += TabStripItem.TabHorizontalSpacing;
			}
			break;
		case eItemAlignment.Far:
			strFormat |= eTextFormat.Right;
			if (TabItem.IsVertical)
			{
				r.X -= 3;
				r.Width -= 3;
			}
			break;
		}
		return r;
	}

	private Rectangle AlignText(Rectangle r)
	{
		if (r.Height > TabItem.TextMarkupBody.Bounds.Height)
		{
			r.Y += (r.Height - TabItem.TextMarkupBody.Bounds.Height) / 2;
		}
		if (r.Width > TabItem.TextMarkupBody.Bounds.Width)
		{
			switch (TabItem.TextAlignment.HasValue ? TabItem.TextAlignment.Value : TabItem.TabStripItem.TextAlignment)
			{
			case eItemAlignment.Center:
				r.X += (r.Width - TabItem.TextMarkupBody.Bounds.Width) / 2;
				break;
			case eItemAlignment.Far:
				r.X += r.Width - TabItem.TextMarkupBody.Bounds.Width;
				break;
			}
		}
		return r;
	}

	private void RenderMarkup(Graphics g, Rectangle r, MarkupDrawContext d)
	{
		Region clip = g.Clip;
		g.SetClip(r, (CombineMode)0);
		TabItem.TextMarkupBody.Render(d);
		g.Clip = clip;
	}

	private Rectangle SetTransform(Graphics g, Rectangle r)
	{
		if (TabItem.IsVertical)
		{
			if (TabDisplay.Alignment == eTabStripAlignment.Left)
			{
				if (TabStripItem.RotateVerticalText)
				{
					g.RotateTransform(90f);
					r = new Rectangle(r.Top, -r.Right, r.Height, r.Width);
				}
				else
				{
					g.RotateTransform(-90f);
					r = new Rectangle(-r.Bottom, r.X, r.Height, r.Width);
				}
			}
			else if (TabStripItem.RotateVerticalText)
			{
				g.RotateTransform(-90f);
				r = new Rectangle(-r.Bottom, r.X, r.Height, r.Width);
			}
			else
			{
				g.RotateTransform(90f);
				r = new Rectangle(r.Top, -r.Right, r.Height, r.Width);
			}
		}
		return r;
	}

	private void ResetTransform(Graphics g)
	{
		if (TabItem.IsVertical)
		{
			g.ResetTransform();
		}
	}

	protected virtual Rectangle GetFocusRectangle(Rectangle rText)
	{
		rText.Inflate(-2, -2);
		return rText;
	}

	public Bitmap GetCloseButtonBitmap(Graphics g, SuperTabItemStateColorTable colors)
	{
		if (_CloseMarkerColor != colors.CloseMarker || _CloseButtonBitmap == null)
		{
			if (_CloseButtonBitmap != null)
			{
				((Image)_CloseButtonBitmap).Dispose();
			}
			_CloseButtonBitmap = CreateCloseButtonBitmap(g, colors);
			_CloseMarkerColor = colors.CloseMarker;
		}
		return _CloseButtonBitmap;
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
		if (_CloseButtonHotBitmap == null)
		{
			_CloseButtonHotBitmap = CreateCloseButtonHotBitmap(g);
		}
		return _CloseButtonHotBitmap;
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Rectangle rectangle = new Rectangle(0, 0, 15, 15);
			GraphicsPath closePath = GetClosePath(rectangle, 4);
			try
			{
				LinearGradientBrush val3 = new LinearGradientBrush(rectangle, Color.White, Color.Pink, 90f);
				try
				{
					val2.FillPath((Brush)(object)val3, closePath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				rectangle.Inflate(-1, -1);
				GraphicsPath closePath2 = GetClosePath(rectangle, 6);
				try
				{
					LinearGradientBrush val4 = new LinearGradientBrush(rectangle, Color.Pink, Color.Crimson, 90f);
					try
					{
						val2.FillPath((Brush)(object)val4, closePath2);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)closePath2)?.Dispose();
				}
				Pen val5 = new Pen(Color.White, 2f);
				try
				{
					val2.DrawLine(val5, new Point(4, 4), new Point(10, 10));
					val2.DrawLine(val5, new Point(10, 4), new Point(4, 10));
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				val2.DrawPath(Pens.DarkRed, closePath);
				return val;
			}
			finally
			{
				((IDisposable)closePath)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public Bitmap GetCloseButtonPressedBitmap(Graphics g)
	{
		if (_CloseButtonPressedBitmap == null)
		{
			_CloseButtonPressedBitmap = CreateCloseButtonPressedBitmap(g);
		}
		return _CloseButtonPressedBitmap;
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Rectangle rectangle = new Rectangle(0, 0, 15, 15);
			GraphicsPath closePath = GetClosePath(rectangle, 4);
			try
			{
				LinearGradientBrush val3 = new LinearGradientBrush(rectangle, Color.LightGray, Color.Red, 90f);
				try
				{
					val2.FillPath((Brush)(object)val3, closePath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				rectangle.Inflate(-1, -1);
				GraphicsPath closePath2 = GetClosePath(rectangle, 6);
				try
				{
					LinearGradientBrush val4 = new LinearGradientBrush(rectangle, Color.Red, Color.Maroon, 90f);
					try
					{
						val2.FillPath((Brush)(object)val4, closePath2);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)closePath2)?.Dispose();
				}
				Pen val5 = new Pen(Color.LightGray, 2f);
				try
				{
					val2.DrawLine(val5, new Point(4, 4), new Point(10, 10));
					val2.DrawLine(val5, new Point(10, 4), new Point(4, 10));
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				val2.DrawPath(Pens.DarkRed, closePath);
				return val;
			}
			finally
			{
				((IDisposable)closePath)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private GraphicsPath GetClosePath(Rectangle r, int rad)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = new Rectangle(r.Right - rad, r.Bottom - rad, rad, rad);
		val.AddArc(rectangle, 0f, 90f);
		rectangle.X = r.X;
		val.AddArc(rectangle, 90f, 90f);
		rectangle.Y = r.Y;
		val.AddArc(rectangle, 180f, 90f);
		rectangle.X = r.Right - rad;
		val.AddArc(rectangle, 270f, 90f);
		val.CloseAllFigures();
		return val;
	}

	public void Dispose()
	{
		StopImageAnimation();
	}
}
