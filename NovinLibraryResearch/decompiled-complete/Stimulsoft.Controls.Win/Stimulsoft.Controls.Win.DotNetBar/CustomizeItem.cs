using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class CustomizeItem : PopupItem
{
	private bool m_MouseOver;

	private const int FIXED_SIZE = 14;

	private string m_CustomizeStr = "";

	private string m_ResetStr = "";

	private string m_ThisToolTip;

	private bool m_Localized;

	private bool m_CustomizeItemVisible = true;

	private bool m_AutoSizeMenuImages = true;

	private Size m_MenuImageSize = Size.Empty;

	[Browsable(false)]
	public bool IsMouseOver => m_MouseOver;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether the item is visible or hidden.")]
	public override bool Visible
	{
		get
		{
			if (ContainerControl is Bar bar)
			{
				if (bar.BarState == eBarState.Docked || bar.BarState == eBarState.Popup)
				{
					return base.Visible;
				}
				return false;
			}
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Customize menu item is visible.")]
	[DefaultValue(true)]
	public virtual bool CustomizeItemVisible
	{
		get
		{
			return m_CustomizeItemVisible;
		}
		set
		{
			m_CustomizeItemVisible = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether item can be customized by user.")]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	public CustomizeItem()
	{
		GlobalItem = false;
		m_MouseOver = false;
		m_SystemItem = true;
		CanCustomize = false;
		LoadResources();
		AutoCollapseOnClick = false;
		m_MenuImageSize = new Size(16, 16);
	}

	public override BaseItem Copy()
	{
		CustomizeItem customizeItem = new CustomizeItem();
		CopyToItem(customizeItem);
		customizeItem.CustomizeItemVisible = m_CustomizeItemVisible;
		return customizeItem;
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		LoadResources();
	}

	protected override void OnTooltip(bool bShow)
	{
		LoadResources();
		base.OnTooltip(bShow);
	}

	protected virtual void LoadResources()
	{
		if (!m_Localized)
		{
			if (GetOwner() != null)
			{
				m_Localized = true;
			}
			using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			m_ThisToolTip = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemTooltip);
			Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemAddRemove);
			m_CustomizeStr = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemCustomize);
			m_ResetStr = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemReset);
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2000)
		{
			PaintOffice(pa);
		}
		else if (base.IsThemed && !base.IsOnMenu)
		{
			PaintThemed(pa);
		}
		else
		{
			if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				PaintOffice2003(pa);
				return;
			}
			PaintDotNet(pa);
		}
		if (!BaseItem.IsOnPopup(this))
		{
			Point[] array = new Point[3];
			if (m_Orientation == eOrientation.Vertical)
			{
				array[0].X = m_Rect.Left + 7;
				array[0].Y = m_Rect.Top + 4;
				array[1].X = array[0].X - 3;
				array[1].Y = array[0].Y + 3;
				array[2].X = array[0].X;
				array[2].Y = array[0].Y + 6;
				pa.Graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
			else
			{
				array[0].X = m_Rect.Left + m_Rect.Width / 2 - 1;
				array[0].Y = m_Rect.Bottom - 4;
				array[1].X = array[0].X - 2;
				array[1].Y = array[0].Y - 3;
				array[2].X = array[1].X + 5;
				array[2].Y = array[1].Y;
				pa.Graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
			if (DesignMode && Focused)
			{
				Rectangle rect = m_Rect;
				DesignTime.DrawDesignTimeSelection(pa.Graphics, rect, pa.Colors.ItemDesignTimeBorder);
			}
		}
	}

	private void PaintThemed(ItemPaintArgs pa)
	{
		ThemeToolbar themeToolbar = pa.ThemeToolbar;
		ThemeToolbarParts button = ThemeToolbarParts.Button;
		ThemeToolbarStates state = ThemeToolbarStates.Normal;
		if (m_Expanded)
		{
			state = ThemeToolbarStates.Pressed;
		}
		else if (m_MouseOver)
		{
			state = ThemeToolbarStates.Hot;
		}
		themeToolbar.DrawBackground(pa.Graphics, button, state, m_Rect);
		themeToolbar = null;
		if (BaseItem.IsOnPopup(this))
		{
			Font val = null;
			eTextFormat stringFormat = GetStringFormat();
			val = GetFont();
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			rect.Width -= 6;
			TextDrawing.DrawString(pa.Graphics, m_Text, val, SystemColors.ControlText, rect, stringFormat);
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - 8;
			array[0].Y = m_Rect.Top + m_Rect.Height / 2 + 3;
			array[1].X = array[0].X - 2;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + 5;
			array[2].Y = array[1].Y;
			pa.Graphics.FillPolygon(SystemBrushes.ControlText, array);
		}
	}

	private void PaintDotNet(ItemPaintArgs pa)
	{
		Rectangle rect = m_Rect;
		Graphics graphics = pa.Graphics;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Office2007 || effectiveStyle == eDotNetBarStyle.Windows7)
		{
			Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
			if (pa.Renderer is Office2007Renderer)
			{
				Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0];
				office2007ButtonItemStateColorTable = (Expanded ? office2007ButtonItemColorTable.Expanded : ((!m_MouseOver) ? office2007ButtonItemColorTable.Default : office2007ButtonItemColorTable.MouseOver));
			}
			if (office2007ButtonItemStateColorTable != null)
			{
				Office2007ButtonItemPainter.PaintBackground(graphics, office2007ButtonItemStateColorTable, rect, RoundRectangleShapeDescriptor.RoundCorner2, isDefault: false, paintBorder: true);
			}
		}
		else
		{
			if (Expanded)
			{
				rect = new Rectangle(m_Rect.Left + 2, m_Rect.Top + 2, m_Rect.Width - 2, m_Rect.Height - 2);
				graphics.FillRectangle(SystemBrushes.ControlDark, rect);
				rect.Offset(-2, -2);
				rect.Height += 2;
				DisplayHelp.FillRectangle(graphics, rect, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, pa.Colors.MenuBorder, rect);
			}
			else if (m_MouseOver)
			{
				rect = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width - 2, m_Rect.Height);
				DisplayHelp.FillRectangle(graphics, rect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, pa.Colors.ItemHotBorder, rect);
			}
			if (!BaseItem.IsOnPopup(this))
			{
				return;
			}
			if (Expanded)
			{
				DisplayHelp.FillRectangle(graphics, m_Rect, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, pa.Colors.MenuBorder, m_Rect);
			}
		}
		Font val = null;
		eTextFormat stringFormat = GetStringFormat();
		val = GetFont();
		Rectangle rect2 = m_Rect;
		rect2.Inflate(-1, -1);
		rect2.Width -= 6;
		rect2.X += 4;
		TextDrawing.DrawString(graphics, m_Text, val, SystemColors.ControlText, rect2, stringFormat);
		Point[] array = new Point[3];
		array[0].X = m_Rect.Right - 6;
		array[0].Y = m_Rect.Top + m_Rect.Height / 2 + 3;
		array[1].X = array[0].X - 2;
		array[1].Y = array[0].Y - 3;
		array[2].X = array[1].X + 5;
		array[2].Y = array[1].Y;
		graphics.FillPolygon(SystemBrushes.ControlText, array);
	}

	private void PaintOffice2003(ItemPaintArgs pa)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Expected O, but got Unknown
		if (BaseItem.IsOnPopup(this))
		{
			PaintDotNet(pa);
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle rect = m_Rect;
		GraphicsPath val = new GraphicsPath();
		if (Orientation == eOrientation.Vertical)
		{
			rect.Y += 2;
			rect.Height--;
			rect.X -= 2;
			rect.Width += 2;
			if (pa.RightToLeft)
			{
				val.AddLine(rect.Right, rect.Y, rect.Right - 2, rect.Y + 2);
				val.AddLine(rect.X + 2, rect.Y + 2, rect.X, rect.Y);
				val.AddLine(rect.X, rect.Bottom - 2, rect.X + 2, rect.Bottom);
				val.AddLine(rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 2);
			}
			else
			{
				val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
				val.AddLine(rect.Right - 2, rect.Y + 2, rect.Right, rect.Y);
				val.AddLine(rect.Right, rect.Bottom - 2, rect.Right - 2, rect.Bottom);
				val.AddLine(rect.X + 2, rect.Bottom, rect.X, rect.Bottom - 2);
			}
			val.CloseAllFigures();
		}
		else
		{
			rect.X += 2;
			rect.Width--;
			rect.Y -= 2;
			rect.Height += 3;
			if (pa.RightToLeft)
			{
				rect.X -= 2;
				val.AddLine(rect.Right, rect.Y, rect.Right - 2, rect.Y + 2);
				val.AddLine(rect.Right - 2, rect.Bottom - 2, rect.Right, rect.Bottom);
				val.AddLine(rect.X + 2, rect.Bottom, rect.X, rect.Bottom - 2);
				val.AddLine(rect.X, rect.Y + 2, rect.X + 2, rect.Y);
			}
			else
			{
				val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
				val.AddLine(rect.X + 2, rect.Bottom - 2, rect.X, rect.Bottom);
				val.AddLine(rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 2);
				val.AddLine(rect.Right, rect.Y + 2, rect.Right - 2, rect.Y);
			}
			val.CloseAllFigures();
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (Expanded)
		{
			DisplayHelp.FillPath(graphics, val, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
		}
		else if (m_MouseOver)
		{
			DisplayHelp.FillPath(graphics, val, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
		}
		else
		{
			DisplayHelp.FillPath(graphics, val, pa.Colors.CustomizeBackground, pa.Colors.CustomizeBackground2, pa.Colors.CustomizeBackgroundGradientAngle);
		}
		graphics.SmoothingMode = smoothingMode;
		if (Orientation == eOrientation.Vertical)
		{
			Point[] array = new Point[3];
			array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
			array[0].Y = rect.Bottom - 3 + 1;
			array[1].X = array[0].X - 2;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + 5;
			array[2].Y = array[1].Y;
			SolidBrush val2 = new SolidBrush(SystemColors.Window);
			try
			{
				graphics.FillPolygon((Brush)(object)val2, array);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(pa.Colors.CustomizeText, 1f);
			try
			{
				graphics.DrawLine(val3, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 9, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 9);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			array = new Point[3];
			array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2;
			array[0].Y = rect.Bottom - 3;
			array[1].X = array[0].X - 2;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + 5;
			array[2].Y = array[1].Y;
			SolidBrush val4 = new SolidBrush(pa.Colors.CustomizeText);
			try
			{
				graphics.FillPolygon((Brush)(object)val4, array);
				return;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		Point[] array2 = new Point[3];
		array2[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
		array2[0].Y = rect.Bottom - 5 + 1;
		array2[1].X = array2[0].X - 2;
		array2[1].Y = array2[0].Y - 3;
		array2[2].X = array2[1].X + 5;
		array2[2].Y = array2[1].Y;
		SolidBrush val5 = new SolidBrush(SystemColors.Window);
		try
		{
			graphics.FillPolygon((Brush)(object)val5, array2);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		Pen val6 = new Pen(pa.Colors.CustomizeText, 1f);
		try
		{
			graphics.DrawLine(val6, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 11, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 11);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		array2 = new Point[3];
		array2[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2;
		array2[0].Y = rect.Bottom - 5;
		array2[1].X = array2[0].X - 2;
		array2[1].Y = array2[0].Y - 3;
		array2[2].X = array2[1].X + 5;
		array2[2].Y = array2[1].Y;
		SolidBrush val7 = new SolidBrush(pa.Colors.CustomizeText);
		try
		{
			graphics.FillPolygon((Brush)(object)val7, array2);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
	}

	private void PaintOffice(ItemPaintArgs pa)
	{
		Graphics graphics = pa.Graphics;
		if (Expanded)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2);
		}
		else if (m_MouseOver)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4);
		}
		if (BaseItem.IsOnPopup(this))
		{
			Font val = null;
			eTextFormat stringFormat = GetStringFormat();
			val = GetFont();
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			rect.Width -= 6;
			if (Expanded)
			{
				rect.Offset(1, 1);
				rect.Width--;
				rect.Height--;
			}
			TextDrawing.DrawString(graphics, m_Text, val, SystemColors.ControlText, rect, stringFormat);
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - 8;
			array[0].Y = m_Rect.Top + m_Rect.Height / 2 + 3;
			array[1].X = array[0].X - 2;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + 5;
			array[2].Y = array[1].Y;
			graphics.FillPolygon(SystemBrushes.ControlText, array);
		}
	}

	protected virtual void SetCustomTooltip(string text)
	{
		Tooltip = text;
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (!BaseItem.IsOnPopup(this))
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Height = 14;
				m_Rect.Width = 22;
			}
			else
			{
				m_Rect.Width = 14;
				m_Rect.Height = 22;
			}
			m_BeginGroup = false;
			SetCustomTooltip(GetTooltipText());
		}
		else
		{
			SetCustomTooltip("");
			m_BeginGroup = true;
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (!BaseItem.IsHandleValid(val))
			{
				return;
			}
			Graphics g = Graphics.FromHwnd(val.Handle);
			Size size = Size.Empty;
			size = ((m_Parent == null) ? ImageSize : ((!(m_Parent is ImageItem imageItem)) ? ImageSize : new Size(imageItem.SubItemsImageSize.Width, imageItem.SubItemsImageSize.Height)));
			Font val2 = null;
			val2 = GetFont();
			Size size2 = Size.Empty;
			eTextFormat stringFormat = GetStringFormat();
			if (m_Text != "")
			{
				size2 = TextDrawing.MeasureString(g, m_Text, val2, 512, stringFormat);
				size2.Width += 2;
			}
			if (size2.Height > size.Height)
			{
				m_Rect.Height = size2.Height + 4;
			}
			else
			{
				m_Rect.Height = size.Height + 4;
			}
			m_Rect.Width = size2.Width + 10;
		}
		base.RecalcSize();
	}

	protected virtual string GetTooltipText()
	{
		return m_ThisToolTip;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		m_MouseOver = true;
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseHover()
	{
		base.InternalMouseHover();
		MouseHoverCustomize();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		m_MouseOver = false;
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576 && !DesignMode)
		{
			MouseDownAction();
		}
	}

	protected virtual void MouseDownAction()
	{
		Expanded = !m_Expanded;
	}

	protected virtual void MouseHoverCustomize()
	{
		if (!Expanded && BaseItem.IsOnPopup(this))
		{
			Expanded = true;
		}
	}

	protected internal override void OnExpandChange()
	{
		if (Expanded)
		{
			SetupCustomizeItem();
		}
		base.OnExpandChange();
		if (!Expanded)
		{
			ClearCustomizeItem();
		}
	}

	protected virtual void SetupCustomizeItem()
	{
		if (BaseItem.IsOnPopup(this))
		{
			AddCustomizeItems();
			PopupType = ePopupType.Menu;
			return;
		}
		SubItems.Clear();
		CustomizeItem customizeItem = new CustomizeItem();
		customizeItem.CustomizeItemVisible = CustomizeItemVisible;
		customizeItem.IsRightToLeft = IsRightToLeft;
		SubItems.Add(customizeItem);
		PopupType = ePopupType.ToolBar;
	}

	protected virtual void ClearCustomizeItem()
	{
		SubItems.Clear();
	}

	private void AddRemoveClick(object sender)
	{
		((ButtonItem)sender).Expanded = !((ButtonItem)sender).Expanded;
	}

	private void ShowHideClick(object sender, EventArgs e)
	{
		BaseItem baseItem = ((BaseItem)sender).Tag as BaseItem;
		bool globalItem = baseItem.GlobalItem;
		baseItem.GlobalItem = false;
		baseItem.SetVisibleDirect(!baseItem.Visible);
		if (baseItem is TextBoxItem || baseItem is ComboBoxItem || baseItem is ControlContainerItem || baseItem.ContainerControl is NavigationBar)
		{
			baseItem.OnVisibleChanged(baseItem.Visible);
		}
		baseItem.UserCustomized = true;
		baseItem.GlobalItem = globalItem;
		((BaseItem)sender).Visible = !((BaseItem)sender).Visible;
		if (baseItem.ContainerControl is Bar)
		{
			((Bar)baseItem.ContainerControl).RecalcLayout();
		}
		else if (baseItem.ContainerControl is MenuPanel)
		{
			((MenuPanel)baseItem.ContainerControl).RecalcSize();
		}
		else if (baseItem.ContainerControl is BarBaseControl)
		{
			((BarBaseControl)baseItem.ContainerControl).RecalcLayout();
		}
		((BaseItem)sender).Refresh();
		if (GetOwner() is IOwner owner)
		{
			owner.InvokeUserCustomize(baseItem, new EventArgs());
			owner.InvokeEndUserCustomize(baseItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemVisibilityChanged));
		}
	}

	private void CustomizeClick(object sender, EventArgs e)
	{
		if (GetOwner() is IOwner owner)
		{
			BaseItem.CollapseAll(this);
			owner.Customize();
		}
	}

	private void ResetClick(object sender, EventArgs e)
	{
		if (GetOwner() is IOwner owner)
		{
			BaseItem item = this;
			if (BaseItem.IsOnPopup(this) && Parent != null)
			{
				item = Parent;
			}
			BaseItem.CollapseAll(this);
			owner.InvokeResetDefinition(item, new EventArgs());
		}
	}

	public override void Refresh()
	{
		if (SuspendLayout)
		{
			return;
		}
		if ((EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)) && !BaseItem.IsOnPopup(this))
		{
			if ((!m_Visible && !base.IsOnCustomizeMenu) || !m_Displayed)
			{
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null || !BaseItem.IsHandleValid(val) || val is ItemsListBox)
			{
				return;
			}
			if (m_NeedRecalcSize)
			{
				RecalcSize();
				if (m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
			}
			Rectangle rect = m_Rect;
			rect.Inflate(2, 2);
			val.Invalidate(rect, true);
		}
		else
		{
			base.Refresh();
		}
	}

	private void AddCustomizeItems()
	{
		SubItems.Clear();
		BaseItem parent = Parent;
		while (parent != null && parent.SystemItem && (!parent.SystemItem || !(parent is GenericItemContainer)))
		{
			parent = parent.Parent;
		}
		if (parent == null)
		{
			return;
		}
		BaseItem baseItem2;
		foreach (BaseItem subItem in parent.SubItems)
		{
			if (subItem.SystemItem || !subItem.CanCustomize)
			{
				continue;
			}
			baseItem2 = subItem.Copy();
			baseItem2.GlobalItem = false;
			baseItem2.ClearClick();
			baseItem2.BeginGroup = false;
			baseItem2.Enabled = true;
			baseItem2.SubItems.Clear();
			baseItem2.Tooltip = "";
			baseItem2.SetIsOnCustomizeMenu(b: true);
			if (subItem is ButtonItem)
			{
				((ButtonItem)baseItem2).HotTrackingStyle = eHotTrackingStyle.Default;
				if (m_AutoSizeMenuImages && !m_MenuImageSize.IsEmpty && ((ButtonItem)baseItem2).ImageSize != m_MenuImageSize)
				{
					((ButtonItem)baseItem2).ImageFixedSize = m_MenuImageSize;
					((ButtonItem)baseItem2).UseSmallImage = true;
				}
			}
			baseItem2.Click += ShowHideClick;
			baseItem2.Tag = subItem;
			SubItems.Add(baseItem2);
		}
		if (parent is GenericItemContainer && ((GenericItemContainer)parent).MoreItems != null)
		{
			BaseItem moreItems = ((GenericItemContainer)parent).MoreItems;
			foreach (BaseItem subItem2 in moreItems.SubItems)
			{
				if (!subItem2.SystemItem)
				{
					baseItem2 = subItem2.Copy();
					baseItem2.GlobalItem = false;
					baseItem2.ClearClick();
					baseItem2.BeginGroup = false;
					baseItem2.Enabled = true;
					baseItem2.SubItems.Clear();
					baseItem2.Tooltip = "";
					baseItem2.SetIsOnCustomizeMenu(b: true);
					baseItem2.Click += ShowHideClick;
					baseItem2.Tag = subItem2;
					SubItems.Add(baseItem2);
				}
			}
		}
		baseItem2 = null;
		ButtonItem buttonItem = null;
		IOwner owner = GetOwner() as IOwner;
		if (owner != null && owner.ShowResetButton)
		{
			buttonItem = new ButtonItem();
			buttonItem.GlobalItem = false;
			buttonItem.BeginGroup = true;
			buttonItem.Text = m_ResetStr;
			buttonItem.SetIsOnCustomizeMenu(b: true);
			buttonItem.SetSystemItem(b: true);
			buttonItem.Orientation = eOrientation.Horizontal;
			buttonItem.Click += ResetClick;
			SubItems.Add(buttonItem);
		}
		if (m_CustomizeItemVisible)
		{
			buttonItem = new ButtonItem();
			buttonItem.GlobalItem = false;
			if (owner == null || (owner != null && !owner.ShowResetButton))
			{
				buttonItem.BeginGroup = true;
			}
			buttonItem.Text = m_CustomizeStr;
			buttonItem.SetIsOnCustomizeMenu(b: true);
			buttonItem.SetSystemItem(b: true);
			buttonItem.Click += CustomizeClick;
			SubItems.Add(buttonItem);
		}
		m_NeedRecalcSize = false;
	}

	private eTextFormat GetStringFormat()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 |= eTextFormat.SingleLine;
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	protected virtual Font GetFont()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			return val.Font;
		}
		return SystemFonts.DefaultFont;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (!m_CustomizeItemVisible)
		{
			itemXmlElement.SetAttribute("customizeitemvisible", XmlConvert.ToString(m_CustomizeItemVisible));
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("customizeitemvisible"))
		{
			m_CustomizeItemVisible = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("customizeitemvisible"));
		}
	}
}
