using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class DisplayMoreItem : PopupItem
{
	private bool m_MouseOver;

	private const int FIXED_SIZE = 14;

	private bool m_PopupSubItemsOnMenu;

	private eOrientation m_OldOrientation;

	private bool m_Localized;

	public static int FixedSize => (int)Math.Ceiling(14.0 * StiScale2.X);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool NeedRecalcSize
	{
		get
		{
			return base.NeedRecalcSize;
		}
		set
		{
			m_NeedRecalcSize = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseOver => m_MouseOver;

	public DisplayMoreItem()
	{
		GlobalItem = false;
		m_MouseOver = false;
		m_PopupSubItemsOnMenu = false;
		m_Tooltip = "More Buttons";
		m_SystemItem = true;
		CanCustomize = false;
		m_ShouldSerialize = false;
		using LocalizationManager localizationManager = new LocalizationManager(null);
		m_Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
	}

	public override BaseItem Copy()
	{
		DisplayMoreItem displayMoreItem = new DisplayMoreItem();
		CopyToItem(displayMoreItem);
		return displayMoreItem;
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (!m_Localized)
		{
			m_Localized = true;
			using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			m_Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
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
		else
		{
			if (base.IsThemed)
			{
				PaintThemed(pa);
				return;
			}
			if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				PaintOffice2003(pa);
				return;
			}
			PaintDotNet(pa);
		}
		Graphics graphics = pa.Graphics;
		if (m_Orientation == eOrientation.Vertical)
		{
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - 8;
			array[0].Y = m_Rect.Top + 4;
			array[1].X = array[0].X + 2;
			array[1].Y = array[0].Y + 2;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y++;
			array[1].Y++;
			array[2].Y++;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y += 3;
			array[1].Y += 3;
			array[2].Y += 3;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y++;
			array[1].Y++;
			array[2].Y++;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].X = m_Rect.Left + 7;
			array[0].Y = m_Rect.Top + 4;
			array[1].X = array[0].X - 3;
			array[1].Y = array[0].Y + 3;
			array[2].X = array[0].X;
			array[2].Y = array[0].Y + 6;
			graphics.FillPolygon(SystemBrushes.ControlText, array);
		}
		else
		{
			Point[] array2 = new Point[3];
			array2[0].X = m_Rect.Left + (m_Rect.Width - 7) / 2 - 1;
			array2[0].Y = m_Rect.Top + 4;
			array2[1].X = array2[0].X + 2;
			array2[1].Y = array2[0].Y + 2;
			array2[2].X = array2[0].X;
			array2[2].Y = array2[0].Y + 4;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X++;
			array2[1].X++;
			array2[2].X++;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X += 3;
			array2[1].X += 3;
			array2[2].X += 3;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X++;
			array2[1].X++;
			array2[2].X++;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X = m_Rect.Left + m_Rect.Width / 2 - 1;
			array2[0].Y = m_Rect.Bottom - 4;
			array2[1].X = array2[0].X - 2;
			array2[1].Y = array2[0].Y - 3;
			array2[2].X = array2[1].X + 5;
			array2[2].Y = array2[1].Y;
			graphics.FillPolygon(SystemBrushes.ControlText, array2);
		}
	}

	private void PaintDotNet(ItemPaintArgs pa)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (m_Expanded)
		{
			Rectangle rectangle = new Rectangle(m_Rect.Left + 2, m_Rect.Top + 2, m_Rect.Width - 2, m_Rect.Height - 2);
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				SolidBrush val = new SolidBrush(pa.Colors.ItemExpandedShadow);
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			rectangle.Offset(-2, -2);
			rectangle.Height += 2;
			if (pa.Colors.ItemExpandedBackground2.IsEmpty)
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemExpandedBackground), rectangle);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				((Brush)val2).Dispose();
			}
			NativeFunctions.DrawRectangle(graphics, new Pen(pa.Colors.MenuBorder, 1f), rectangle);
		}
		else if (m_MouseOver)
		{
			Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width - 2, m_Rect.Height);
			if (!pa.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, rectangle);
				((Brush)val3).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), rectangle);
			}
			NativeFunctions.DrawRectangle(graphics, new Pen(pa.Colors.ItemHotBorder), rectangle);
		}
	}

	private void PaintThemed(ItemPaintArgs pa)
	{
		ThemeRebar themeRebar = pa.ThemeRebar;
		ThemeRebarParts chevron = ThemeRebarParts.Chevron;
		ThemeRebarStates state = ThemeRebarStates.ChevronNormal;
		if (m_Expanded)
		{
			state = ThemeRebarStates.ChevronPressed;
		}
		else if (m_MouseOver)
		{
			state = ThemeRebarStates.ChevronHot;
		}
		themeRebar.DrawBackground(pa.Graphics, chevron, state, m_Rect);
	}

	private void PaintOffice(ItemPaintArgs pa)
	{
		Graphics graphics = pa.Graphics;
		if (m_Expanded)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2);
		}
		else if (m_MouseOver)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4);
		}
	}

	private void PaintOffice2003(ItemPaintArgs pa)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Expected O, but got Unknown
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Expected O, but got Unknown
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		Rectangle rect = m_Rect;
		GraphicsPath val = new GraphicsPath();
		if (Orientation == eOrientation.Vertical)
		{
			rect.Y += 2;
			rect.Height--;
			rect.X -= 2;
			rect.Width += 2;
			val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
			val.AddLine(rect.Right - 2, rect.Y + 2, rect.Right, rect.Y);
			val.AddLine(rect.Right, rect.Bottom - 2, rect.Right - 2, rect.Bottom);
			val.AddLine(rect.X + 2, rect.Bottom, rect.X, rect.Bottom - 2);
			val.CloseAllFigures();
		}
		else
		{
			rect.X += 2;
			rect.Width--;
			rect.Y -= 2;
			rect.Height += 3;
			val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
			val.AddLine(rect.X + 2, rect.Bottom - 2, rect.X, rect.Bottom);
			val.AddLine(rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 2);
			val.AddLine(rect.Right, rect.Y + 2, rect.Right - 2, rect.Y);
			val.CloseAllFigures();
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (Expanded)
		{
			LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val2, val);
			((Brush)val2).Dispose();
		}
		else if (m_MouseOver)
		{
			LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val3, val);
			((Brush)val3).Dispose();
		}
		else
		{
			LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.CustomizeBackground, pa.Colors.CustomizeBackground2, pa.Colors.CustomizeBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val4, val);
			((Brush)val4).Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		if (Orientation == eOrientation.Vertical)
		{
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 10 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 10 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 8 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 8 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Top + 7 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Top + 7 + 1);
			Pen val5 = new Pen(pa.Colors.CustomizeText, 1f);
			try
			{
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 10, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 10);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 8);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 8);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2, rect.Top + 7);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Top + 7);
				return;
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Bottom - 11 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Bottom - 11 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 8 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 8 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Top + 7 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Top + 7 + 1);
		Point[] array = new Point[3];
		array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
		array[0].Y = rect.Bottom - 5 + 1;
		array[1].X = array[0].X - 2;
		array[1].Y = array[0].Y - 3;
		array[2].X = array[1].X + 5;
		array[2].Y = array[1].Y;
		SolidBrush val6 = new SolidBrush(SystemColors.HighlightText);
		try
		{
			graphics.FillPolygon((Brush)(object)val6, array);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		Pen val7 = new Pen(pa.Colors.CustomizeText, 1f);
		try
		{
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 11, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 11);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 8);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 8);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2, rect.Top + 7);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Top + 7);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		array = new Point[3];
		array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2;
		array[0].Y = rect.Bottom - 5;
		array[1].X = array[0].X - 2;
		array[1].Y = array[0].Y - 3;
		array[2].X = array[1].X + 5;
		array[2].Y = array[1].Y;
		SolidBrush val8 = new SolidBrush(pa.Colors.CustomizeText);
		try
		{
			graphics.FillPolygon((Brush)(object)val8, array);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Height = FixedSize;
			}
			else
			{
				m_Rect.Width = FixedSize;
			}
			base.RecalcSize();
		}
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
		if ((int)objArg.Button == 1048576)
		{
			Expanded = !m_Expanded;
		}
	}

	public override void Popup(Point p)
	{
		Popup(p.X, p.Y);
	}

	public override void Popup(int x, int y)
	{
		if (m_PopupSubItemsOnMenu)
		{
			PopupMenu(x, y);
		}
		else
		{
			PopupBar(x, y);
		}
	}

	public override void PopupBar(int x, int y)
	{
		AddItems();
		m_NeedRecalcSize = false;
		base.PopupBar(x, y);
		m_NeedRecalcSize = false;
	}

	public override void PopupMenu(int x, int y)
	{
		AddItems();
		m_NeedRecalcSize = false;
		base.PopupMenu(x, y);
		m_NeedRecalcSize = false;
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		SubItems.Clear();
	}

	protected virtual void AddItems()
	{
		if (m_SubItems == null)
		{
			m_SubItems = new SubItemsCollection(this);
		}
		m_OldOrientation = Orientation;
		foreach (BaseItem subItem in m_Parent.SubItems)
		{
			if (subItem.Displayed || !subItem.Visible)
			{
				continue;
			}
			m_SubItems._Add(subItem);
			if (subItem is ImageItem { ImageSize: { Width: var width } } imageItem)
			{
				if (width > SubItemsImageSize.Width)
				{
					SubItemsImageSize = new Size(imageItem.ImageSize.Width, SubItemsImageSize.Height);
				}
				if (imageItem.ImageSize.Height > SubItemsImageSize.Height)
				{
					SubItemsImageSize = new Size(SubItemsImageSize.Width, imageItem.ImageSize.Height);
				}
			}
		}
		foreach (BaseItem subItem2 in m_SubItems)
		{
			object containerControl = subItem2.ContainerControl;
			m_Parent.SubItems.RemoveItem(subItem2);
			subItem2.SetOrientation(eOrientation.Horizontal);
			subItem2.SetParent(this);
			subItem2.ContainerControl = null;
			subItem2.OnContainerChanged(containerControl);
		}
		NeedRecalcSize = true;
	}

	protected virtual int GetReInsertIndex()
	{
		int num = m_Parent.SubItems.Count;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			if (m_Parent.SubItems[num2] is CustomizeItem)
			{
				num = num2;
				break;
			}
		}
		return num;
	}

	protected virtual void RemoveItems()
	{
		if (base.PopupControl is Bar)
		{
			((Bar)(object)base.PopupControl).ParentItem = null;
		}
		m_HotSubItem = null;
		int num = GetReInsertIndex();
		foreach (BaseItem subItem in m_SubItems)
		{
			object containerControl = subItem.ContainerControl;
			subItem.Orientation = m_OldOrientation;
			m_Parent.SubItems._Add(subItem, num);
			num++;
			subItem.SetParent(m_Parent);
			subItem.ContainerControl = null;
		}
		ArrayList arrayList = new ArrayList(m_SubItems.Count);
		m_SubItems.CopyTo(arrayList);
		while (m_SubItems.Count > 0)
		{
			m_SubItems.RemoveItem(m_SubItems[0]);
		}
		foreach (BaseItem item in arrayList)
		{
			item.Displayed = false;
		}
		base.OnExpandChange();
		BaseItem parent = m_Parent;
		Control val = null;
		if (parent != null)
		{
			object containerControl2 = parent.ContainerControl;
			val = (Control)((containerControl2 is Control) ? containerControl2 : null);
		}
		bool flag = true;
		if (val != null)
		{
			flag = !BarFunctions.InvokeRecalcLayout(val, invalidate: false);
		}
		if (flag && parent != null)
		{
			parent.RecalcSize();
			parent.Refresh();
		}
	}

	protected internal override void OnExpandChange()
	{
		if (!Expanded && m_SubItems != null && m_Parent != null)
		{
			RemoveItems();
		}
		else
		{
			base.OnExpandChange();
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
}
