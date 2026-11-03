using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class CustomizeItem : PopupItem
{
	private bool ݡ;

	private string ჩ = "";

	private string ც = "";

	private string ძ;

	private bool _0DF2;

	private bool წ = true;

	private bool ჭ = true;

	private Size ხ = Size.Empty;

	[Browsable(false)]
	public bool IsMouseOver => ݡ;

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Determines whether the item is visible or hidden.")]
	[DevCoBrowsable(true)]
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
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Customize menu item is visible.")]
	[DevCoBrowsable(true)]
	public virtual bool CustomizeItemVisible
	{
		get
		{
			return წ;
		}
		set
		{
			წ = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
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

	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Browsable(false)]
	[DefaultValue(false)]
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

	[DevCoBrowsable(false)]
	[Browsable(false)]
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
		ݡ = false;
		m_SystemItem = true;
		CanCustomize = false;
		LoadResources();
		AutoCollapseOnClick = false;
		ხ = new Size(16, 16);
	}

	public override BaseItem Copy()
	{
		CustomizeItem customizeItem = new CustomizeItem();
		CopyToItem(customizeItem);
		customizeItem.CustomizeItemVisible = წ;
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
		if (!_0DF2)
		{
			if (GetOwner() != null)
			{
				_0DF2 = true;
			}
			using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			ძ = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemTooltip);
			Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemAddRemove);
			ჩ = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemCustomize);
			ც = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemReset);
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
			_0DF5(pa);
		}
		else if (base.IsThemed && !base.IsOnMenu)
		{
			_0DF4(pa);
		}
		else
		{
			if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				_0DF6(pa);
				return;
			}
			_0DF3(pa);
		}
		if (!BaseItem.IsOnPopup(this))
		{
			Point[] array = new Point[3];
			if (m_Orientation == eOrientation.Vertical)
			{
				array[0].X = m_Rect.Left + Dpi.Width7;
				array[0].Y = m_Rect.Top + Dpi.Height4;
				array[1].X = array[0].X - Dpi.Width3;
				array[1].Y = array[0].Y + Dpi.Height3;
				array[2].X = array[0].X;
				array[2].Y = array[0].Y + Dpi.Height6;
				pa.Graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
			else
			{
				array[0].X = m_Rect.Left + m_Rect.Width / 2 - Dpi.Width1;
				array[0].Y = m_Rect.Bottom - Dpi.Height4;
				array[1].X = array[0].X - Dpi.Width2;
				array[1].Y = array[0].Y - Dpi.Height3;
				array[2].X = array[1].X + Dpi.Width5;
				array[2].Y = array[1].Y;
				pa.Graphics.FillPolygon(SystemBrushes.ControlText, array);
			}
			if (DesignMode && Focused)
			{
				Rectangle rect = m_Rect;
				_1A6B._1A6C(pa.Graphics, rect, pa.Colors.ItemDesignTimeBorder);
			}
		}
	}

	private void _0DF4(ItemPaintArgs _0BD2)
	{
		_0A7B obj = _0BD2._0A7B;
		_0F6F _070B = _0F6F._0F70;
		_0F76 య = _0F76.ཊ;
		if (m_Expanded)
		{
			య = _0F76._0F78;
		}
		else if (ݡ)
		{
			య = _0F76._0F77;
		}
		obj._0EE5(_0BD2.Graphics, _070B, య, m_Rect);
		obj = null;
		if (BaseItem.IsOnPopup(this))
		{
			Font val = null;
			eTextFormat format = ಲ();
			val = GetFont();
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			rect.Width -= 6;
			TextDrawing.DrawString(_0BD2.Graphics, m_Text, val, SystemColors.ControlText, rect, format);
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - 8;
			array[0].Y = m_Rect.Top + m_Rect.Height / 2 + 3;
			array[1].X = array[0].X - 2;
			array[1].Y = array[0].Y - 3;
			array[2].X = array[1].X + 5;
			array[2].Y = array[1].Y;
			_0BD2.Graphics.FillPolygon(SystemBrushes.ControlText, array);
		}
	}

	private void _0DF3(ItemPaintArgs _0BD2)
	{
		Rectangle rect = m_Rect;
		Graphics graphics = _0BD2.Graphics;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Office2007 || effectiveStyle == eDotNetBarStyle.Windows7)
		{
			Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
			if (_0BD2.ໝ is Office2007Renderer)
			{
				Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)_0BD2.ໝ).ColorTable.ButtonItemColors[0];
				office2007ButtonItemStateColorTable = (Expanded ? office2007ButtonItemColorTable.Expanded : ((!ݡ) ? office2007ButtonItemColorTable.Default : office2007ButtonItemColorTable.MouseOver));
			}
			if (office2007ButtonItemStateColorTable != null)
			{
				ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, rect, RoundRectangleShapeDescriptor.RoundCorner2, ᯕ: false, ᯖ: true);
			}
		}
		else
		{
			if (Expanded)
			{
				rect = new Rectangle(m_Rect.Left + Dpi.Width2, m_Rect.Top + Dpi.Height2, m_Rect.Width - Dpi.Width2, m_Rect.Height - Dpi.Height2);
				graphics.FillRectangle(SystemBrushes.ControlDark, rect);
				rect.Offset(-Dpi.Width2, -Dpi.Height2);
				rect.Height += Dpi.Height2;
				DisplayHelp.FillRectangle(graphics, rect, _0BD2.Colors.ItemExpandedBackground, _0BD2.Colors.ItemExpandedBackground2, _0BD2.Colors.ItemExpandedBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, _0BD2.Colors.MenuBorder, rect);
			}
			else if (ݡ)
			{
				rect = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width - Dpi.Width2, m_Rect.Height);
				DisplayHelp.FillRectangle(graphics, rect, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, _0BD2.Colors.ItemHotBorder, rect);
			}
			if (!BaseItem.IsOnPopup(this))
			{
				return;
			}
			if (Expanded)
			{
				DisplayHelp.FillRectangle(graphics, m_Rect, _0BD2.Colors.ItemExpandedBackground, _0BD2.Colors.ItemExpandedBackground2, _0BD2.Colors.ItemExpandedBackgroundGradientAngle);
				DisplayHelp.DrawRectangle(graphics, _0BD2.Colors.MenuBorder, m_Rect);
			}
		}
		Font val = null;
		eTextFormat format = ಲ();
		val = GetFont();
		Rectangle rect2 = m_Rect;
		rect2.Inflate(-Dpi.Width1, -Dpi.Height1);
		rect2.Width -= Dpi.Width6;
		rect2.X += Dpi.Width4;
		TextDrawing.DrawString(graphics, m_Text, val, SystemColors.ControlText, rect2, format);
		Point[] array = new Point[3];
		array[0].X = m_Rect.Right - Dpi.Width6;
		array[0].Y = m_Rect.Top + m_Rect.Height / 2 + Dpi.Height3;
		array[1].X = array[0].X - Dpi.Width2;
		array[1].Y = array[0].Y - Dpi.Height3;
		array[2].X = array[1].X + Dpi.Width5;
		array[2].Y = array[1].Y;
		graphics.FillPolygon(SystemBrushes.ControlText, array);
	}

	private void _0DF6(ItemPaintArgs _0BD2)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Expected O, but got Unknown
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Expected O, but got Unknown
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Expected O, but got Unknown
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		if (BaseItem.IsOnPopup(this))
		{
			_0DF3(_0BD2);
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		Rectangle rect = m_Rect;
		GraphicsPath val = new GraphicsPath();
		if (Orientation == eOrientation.Vertical)
		{
			rect.Y += Dpi.Height2;
			rect.Height -= Dpi.Height1;
			rect.X -= Dpi.Width2;
			rect.Width += Dpi.Width2;
			if (_0BD2.RightToLeft)
			{
				val.AddLine(rect.Right, rect.Y, rect.Right - Dpi.Width2, rect.Y + Dpi.Height2);
				val.AddLine(rect.X + Dpi.Width2, rect.Y + Dpi.Height2, rect.X, rect.Y);
				val.AddLine(rect.X, rect.Bottom - Dpi.Height2, rect.X + Dpi.Width2, rect.Bottom);
				val.AddLine(rect.Right - Dpi.Width2, rect.Bottom, rect.Right, rect.Bottom - Dpi.Height2);
			}
			else
			{
				val.AddLine(rect.X, rect.Y, rect.X + Dpi.Width2, rect.Y + Dpi.Height2);
				val.AddLine(rect.Right - Dpi.Width2, rect.Y + Dpi.Height2, rect.Right, rect.Y);
				val.AddLine(rect.Right, rect.Bottom - Dpi.Height2, rect.Right - Dpi.Width2, rect.Bottom);
				val.AddLine(rect.X + Dpi.Width2, rect.Bottom, rect.X, rect.Bottom - Dpi.Height2);
			}
			val.CloseAllFigures();
		}
		else
		{
			rect.X += Dpi.Width2;
			rect.Width -= Dpi.Width1;
			rect.Y -= Dpi.Height2;
			rect.Height += Dpi.Height3;
			if (_0BD2.RightToLeft)
			{
				rect.X -= Dpi.Width2;
				val.AddLine(rect.Right, rect.Y, rect.Right - Dpi.Width2, rect.Y + Dpi.Height2);
				val.AddLine(rect.Right - Dpi.Width2, rect.Bottom - Dpi.Height2, rect.Right, rect.Bottom);
				val.AddLine(rect.X + Dpi.Width2, rect.Bottom, rect.X, rect.Bottom - Dpi.Height2);
				val.AddLine(rect.X, rect.Y + Dpi.Height2, rect.X + Dpi.Width2, rect.Y);
			}
			else
			{
				val.AddLine(rect.X, rect.Y, rect.X + Dpi.Width2, rect.Y + Dpi.Height2);
				val.AddLine(rect.X + Dpi.Width2, rect.Bottom - Dpi.Height2, rect.X, rect.Bottom);
				val.AddLine(rect.Right - Dpi.Width2, rect.Bottom, rect.Right, rect.Bottom - Dpi.Height2);
				val.AddLine(rect.Right, rect.Y + Dpi.Height2, rect.Right - Dpi.Width2, rect.Y);
			}
			val.CloseAllFigures();
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (Expanded)
		{
			DisplayHelp.FillPath(graphics, val, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
		}
		else if (ݡ)
		{
			DisplayHelp.FillPath(graphics, val, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
		}
		else
		{
			DisplayHelp.FillPath(graphics, val, _0BD2.Colors.CustomizeBackground, _0BD2.Colors.CustomizeBackground2, _0BD2.Colors.CustomizeBackgroundGradientAngle);
		}
		graphics.SmoothingMode = smoothingMode;
		if (Orientation == eOrientation.Vertical)
		{
			Point[] array = new Point[3];
			array[0].X = rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width3;
			array[0].Y = rect.Bottom - Dpi.Height3 + Dpi.Height1;
			array[1].X = array[0].X - Dpi.Width2;
			array[1].Y = array[0].Y - Dpi.Height3;
			array[2].X = array[1].X + Dpi.Width5;
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
			Pen val3 = new Pen(_0BD2.Colors.CustomizeText, 1f);
			try
			{
				graphics.DrawLine(val3, rect.Left + (m_Rect.Width - Dpi.Width4) / 2, rect.Bottom - Dpi.Height9, rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width4, rect.Bottom - Dpi.Height9);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			array = new Point[3];
			array[0].X = rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width2;
			array[0].Y = rect.Bottom - Dpi.Height3;
			array[1].X = array[0].X - Dpi.Width2;
			array[1].Y = array[0].Y - Dpi.Height3;
			array[2].X = array[1].X + Dpi.Width5;
			array[2].Y = array[1].Y;
			SolidBrush val4 = new SolidBrush(_0BD2.Colors.CustomizeText);
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
		array2[0].X = rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width3;
		array2[0].Y = rect.Bottom - Dpi.Height5 + Dpi.Height1;
		array2[1].X = array2[0].X - Dpi.Width2;
		array2[1].Y = array2[0].Y - Dpi.Height3;
		array2[2].X = array2[1].X + Dpi.Width5;
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
		Pen val6 = new Pen(_0BD2.Colors.CustomizeText, 1f);
		try
		{
			graphics.DrawLine(val6, rect.Left + (m_Rect.Width - Dpi.Width4) / 2, rect.Bottom - Dpi.Height11, rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width4, rect.Bottom - Dpi.Height11);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		array2 = new Point[3];
		array2[0].X = rect.Left + (m_Rect.Width - Dpi.Width4) / 2 + Dpi.Width2;
		array2[0].Y = rect.Bottom - Dpi.Height5;
		array2[1].X = array2[0].X - Dpi.Width2;
		array2[1].Y = array2[0].Y - Dpi.Height3;
		array2[2].X = array2[1].X + Dpi.Width5;
		array2[2].Y = array2[1].Y;
		SolidBrush val7 = new SolidBrush(_0BD2.Colors.CustomizeText);
		try
		{
			graphics.FillPolygon((Brush)(object)val7, array2);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
	}

	private void _0DF5(ItemPaintArgs _0BD2)
	{
		Graphics graphics = _0BD2.Graphics;
		if (Expanded)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2);
		}
		else if (ݡ)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4);
		}
		if (BaseItem.IsOnPopup(this))
		{
			Font val = null;
			eTextFormat format = ಲ();
			val = GetFont();
			Rectangle rect = m_Rect;
			rect.Inflate(-Dpi.Width1, -Dpi.Height1);
			rect.Width -= Dpi.Width6;
			if (Expanded)
			{
				rect.Offset(Dpi.Width1, Dpi.Height1);
				rect.Width -= Dpi.Width1;
				rect.Height -= Dpi.Height1;
			}
			TextDrawing.DrawString(graphics, m_Text, val, SystemColors.ControlText, rect, format);
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - Dpi.Width8;
			array[0].Y = m_Rect.Top + m_Rect.Height / 2 + Dpi.Height3;
			array[1].X = array[0].X - Dpi.Width2;
			array[1].Y = array[0].Y - Dpi.Height3;
			array[2].X = array[1].X + Dpi.Width5;
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
				m_Rect.Height = Dpi.Width14;
				m_Rect.Width = Dpi.Height22;
			}
			else
			{
				m_Rect.Width = Dpi.Width14;
				m_Rect.Height = Dpi.Height22;
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
			eTextFormat format = ಲ();
			if (m_Text != "")
			{
				size2 = TextDrawing.MeasureString(g, m_Text, val2, 512, format);
				size2.Width += Dpi.Width2;
			}
			if (size2.Height > size.Height)
			{
				m_Rect.Height = size2.Height + Dpi.Height4;
			}
			else
			{
				m_Rect.Height = size.Height + Dpi.Height4;
			}
			m_Rect.Width = size2.Width + Dpi.Width15;
		}
		base.RecalcSize();
	}

	protected virtual string GetTooltipText()
	{
		return ძ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		ݡ = true;
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
		ݡ = false;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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
			ჳ();
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

	private void ჯ(object _٠)
	{
		((ButtonItem)_٠).Expanded = !((ButtonItem)_٠).Expanded;
	}

	private void ჰ(object _٠, EventArgs ؿ)
	{
		BaseItem baseItem = ((BaseItem)_٠).Tag as BaseItem;
		bool globalItem = baseItem.GlobalItem;
		baseItem.GlobalItem = false;
		baseItem.SetVisibleDirect(!baseItem.Visible);
		if (baseItem is TextBoxItem || baseItem is ComboBoxItem || baseItem is ControlContainerItem || baseItem.ContainerControl is NavigationBar)
		{
			baseItem.OnVisibleChanged(baseItem.Visible);
		}
		baseItem.UserCustomized = true;
		baseItem.GlobalItem = globalItem;
		((BaseItem)_٠).Visible = !((BaseItem)_٠).Visible;
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
		((BaseItem)_٠).Refresh();
		if (GetOwner() is IOwner owner)
		{
			owner.InvokeUserCustomize(baseItem, new EventArgs());
			owner.InvokeEndUserCustomize(baseItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemVisibilityChanged));
		}
	}

	private void ჱ(object _٠, EventArgs ؿ)
	{
		if (GetOwner() is IOwner owner)
		{
			BaseItem.CollapseAll(this);
			owner.Customize();
		}
	}

	private void ჲ(object _٠, EventArgs ؿ)
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
			if (val == null || !BaseItem.IsHandleValid(val) || val is ᆓ)
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

	private void ჳ()
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
				if (ჭ && !ხ.IsEmpty && ((ButtonItem)baseItem2).ImageSize != ხ)
				{
					((ButtonItem)baseItem2).ImageFixedSize = ხ;
					((ButtonItem)baseItem2).UseSmallImage = true;
				}
			}
			baseItem2.Click += ჰ;
			baseItem2.Tag = subItem;
			SubItems.Add(baseItem2);
		}
		if (parent is GenericItemContainer && ((GenericItemContainer)parent)._0DBC != null)
		{
			BaseItem baseItem3 = ((GenericItemContainer)parent)._0DBC;
			foreach (BaseItem subItem2 in baseItem3.SubItems)
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
					baseItem2.Click += ჰ;
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
			buttonItem.Text = ც;
			buttonItem.SetIsOnCustomizeMenu(b: true);
			buttonItem.SetSystemItem(b: true);
			buttonItem.Orientation = eOrientation.Horizontal;
			buttonItem.Click += ჲ;
			SubItems.Add(buttonItem);
		}
		if (წ)
		{
			buttonItem = new ButtonItem();
			buttonItem.GlobalItem = false;
			if (owner == null || (owner != null && !owner.ShowResetButton))
			{
				buttonItem.BeginGroup = true;
			}
			buttonItem.Text = ჩ;
			buttonItem.SetIsOnCustomizeMenu(b: true);
			buttonItem.SetSystemItem(b: true);
			buttonItem.Click += ჱ;
			SubItems.Add(buttonItem);
		}
		m_NeedRecalcSize = false;
	}

	private eTextFormat ಲ()
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
		if (!წ)
		{
			itemXmlElement.SetAttribute("customizeitemvisible", XmlConvert.ToString(წ));
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("customizeitemvisible"))
		{
			წ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("customizeitemvisible"));
		}
	}
}
