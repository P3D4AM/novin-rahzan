using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

public class StiTableHelper
{
	private IStiDesignerBase designer;

	private bool altKeyMode;

	private Hashtable storedLocations;

	private Point storedPosition = Point.Empty;

	private RectangleD storedSelectedRect = RectangleD.Empty;

	private double mouseX;

	private double mouseY;

	public bool IsControlKeyPressed => (Control.ModifierKeys & 0x20000) > 0;

	public static bool IsAllowUseInTableMode(StiComponent comp)
	{
		if (comp is StiPage)
		{
			return false;
		}
		if (comp.Parent is IStiCrossTab)
		{
			return false;
		}
		return StiRestrictionsHelper.IsAllowChangePosition(comp);
	}

	public static int GetSelectedCount(StiComponentsCollection comps)
	{
		if (comps == null)
		{
			return 0;
		}
		int num = 0;
		foreach (StiComponent comp in comps)
		{
			if (IsAllowUseInTableMode(comp))
			{
				num++;
			}
		}
		return num;
	}

	public static void GetSelectedLines(IStiDesignerBase designer, ref Hashtable xx, ref Hashtable yy, bool isPaint)
	{
		GetSelectedLines(designer, ref xx, ref yy, isPaint, isPaint);
	}

	public static void GetSelectedLines(IStiDesignerBase designer, ref Hashtable xx, ref Hashtable yy, bool isPaint, bool useZoom)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		RectangleD selectedRectangle = GetSelectedRectangle(designer, isPaint, useZoom);
		xx[((RectangleD)(ref selectedRectangle)).X] = ((RectangleD)(ref selectedRectangle)).X;
		yy[((RectangleD)(ref selectedRectangle)).Y] = ((RectangleD)(ref selectedRectangle)).Y;
		xx[((RectangleD)(ref selectedRectangle)).Right] = ((RectangleD)(ref selectedRectangle)).Right;
		yy[((RectangleD)(ref selectedRectangle)).Bottom] = ((RectangleD)(ref selectedRectangle)).Bottom;
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if (IsAllowUseInTableMode(item))
			{
				RectangleD paintRectangle = item.GetPaintRectangle(isPaint, useZoom);
				xx[((RectangleD)(ref paintRectangle)).X] = ((RectangleD)(ref paintRectangle)).X;
				yy[((RectangleD)(ref paintRectangle)).Y] = ((RectangleD)(ref paintRectangle)).Y;
				xx[((RectangleD)(ref paintRectangle)).Right] = ((RectangleD)(ref paintRectangle)).Right;
				yy[((RectangleD)(ref paintRectangle)).Bottom] = ((RectangleD)(ref paintRectangle)).Bottom;
			}
		}
	}

	public static bool IsTableMode(StiComponent component)
	{
		return IsTableMode(component.Page.Report.Designer);
	}

	public static bool IsTableMode(IStiDesignerBase designer)
	{
		if (designer == null)
		{
			return false;
		}
		return GetSelectedCount(designer.SelectedComponentsOnPage) > 1;
	}

	public static RectangleD GetSelectedRectangle(IStiDesignerBase designer)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return GetSelectedRectangle(designer, isPaintRect: false);
	}

	public static RectangleD GetSelectedRectangle(IStiDesignerBase designer, bool isPaintRect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return GetSelectedRectangle(designer, isPaintRect, designer.SelectedComponentsOnPage);
	}

	public static RectangleD GetSelectedRectangle(IStiDesignerBase designer, bool isPaintRect, bool useZoom)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return GetSelectedRectangle(designer, isPaintRect, useZoom, designer.SelectedComponentsOnPage);
	}

	public static RectangleD GetSelectedRectangle(IStiDesignerBase designer, bool isPaintRect, StiComponentsCollection comps)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return GetSelectedRectangle(designer, isPaintRect, isPaintRect, comps);
	}

	public static RectangleD GetSelectedRectangle(IStiDesignerBase designer, bool isPaintRect, bool useZoom, StiComponentsCollection comps)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		RectangleD result = RectangleD.Empty;
		foreach (StiComponent comp in comps)
		{
			if (IsAllowUseInTableMode(comp))
			{
				RectangleD paintRectangle = comp.GetPaintRectangle(isPaintRect, useZoom);
				result = (((RectangleD)(ref result)).IsEmpty ? paintRectangle : ((RectangleD)(ref result)).FitToRectangle(paintRectangle));
			}
		}
		return result;
	}

	public static double GetDistForResize(StiComponent comp)
	{
		return comp.Page.Unit.ConvertFromHInches(4.0);
	}

	private RectangleD GetRect(StiComponent comp)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return comp.GetPaintRectangle(convertToHInches: false, convertByZoom: false, docking: false);
	}

	private void SetRect(StiComponent comp, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		comp.SetPaintRectangle(rect);
	}

	private double ConvertValue(double value)
	{
		value = designer.Report.GetCurrentPage().Unit.ConvertFromHInches(value / designer.Report.GetCurrentPage().Zoom);
		value = StiAlignValue.AlignToGrid(value, designer.Report.GetCurrentPage().GridSize, designer.Report.Info.AlignToGrid && !altKeyMode);
		return value;
	}

	private double Align(double value)
	{
		if (altKeyMode)
		{
			return value;
		}
		return StiAlignValue.AlignToGrid(value, designer.Report.GetCurrentPage().GridSize, designer.Report.Info.AlignToGrid);
	}

	private decimal Round(double value)
	{
		if (designer.Report.GetCurrentPage() is IStiForm)
		{
			return Math.Round((decimal)value, 0);
		}
		if (designer.Report.GetCurrentPage() is IStiScreen)
		{
			return Math.Round((decimal)value, 0);
		}
		if (designer.Report.GetCurrentPage() is IStiDashboard)
		{
			return Math.Round((decimal)value, 0);
		}
		StiUnit unit = designer.Report.GetCurrentPage().Unit;
		if (unit is StiCentimetersUnit || unit is StiInchesUnit)
		{
			return Math.Round((decimal)value, 1);
		}
		return Math.Round((decimal)value, 0);
	}

	private bool IsContain(double value1, double value2, double dist)
	{
		if (value1 > value2 - dist)
		{
			return value1 < value2 + dist;
		}
		return false;
	}

	public void ProcessResizeColumns()
	{
		double num = ConvertValue(Cursor.Position.X);
		double num2 = ConvertValue(storedPosition.X);
		double offset = (num - num2) * StiScale.System;
		StiResizeType resizeType = StiResizeType.Resize;
		object[] selected = designer.Report.GetSelected();
		if (!designer.Report.IsPageDesigner)
		{
			bool flag = false;
			for (int i = 0; i < selected.Length; i++)
			{
				if (selected[i] is IStiTableCell)
				{
					flag = true;
					resizeType = StiResizeType.TwoResize;
					break;
				}
			}
			if (!flag)
			{
				resizeType = (IsControlKeyPressed ? StiResizeType.TwoResize : StiResizeType.Resize);
			}
		}
		ResizeColumns(mouseX, offset, resizeType);
	}

	public void ResizeColumns(double lineX, double offset, StiResizeType resizeType)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		double distForResize = GetDistForResize(designer.Report.GetCurrentPage());
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if ((!designer.Report.IsPageDesigner && item is IStiTableCell && (!(designer.SelectedComponentsOnPage[0] is StiTable stiTable) || stiTable.Components.Count + 1 != designer.SelectedComponentsOnPage.Count)) || !IsAllowUseInTableMode(item))
			{
				continue;
			}
			RectangleD clientRectangle = item.ClientRectangle;
			RestoreComponentLocation(item);
			try
			{
				RectangleD rect = GetRect(item);
				if (IsContain(((RectangleD)(ref rect)).Right, lineX, distForResize))
				{
					if (item.Width + offset < 0.0)
					{
						offset = 0.0 - item.Width;
					}
				}
				else if (((RectangleD)(ref rect)).Left < lineX && ((RectangleD)(ref rect)).Right > lineX && resizeType == StiResizeType.Resize)
				{
					if (item.Width + offset < 0.0)
					{
						offset = 0.0 - item.Width;
					}
				}
				else if (IsContain(((RectangleD)(ref rect)).Left, lineX, distForResize) && resizeType == StiResizeType.TwoResize && item.Width - offset < 0.0)
				{
					offset = item.Width;
				}
			}
			finally
			{
				item.ClientRectangle = clientRectangle;
			}
		}
		RestoreComponentsLocation();
		foreach (StiComponent item2 in designer.SelectedComponentsOnPage)
		{
			if ((!(item2 is IStiTableCell) || (designer.SelectedComponentsOnPage[0] is StiTable stiTable2 && stiTable2.Components.Count + 1 == designer.SelectedComponentsOnPage.Count)) && IsAllowUseInTableMode(item2))
			{
				RectangleD rect2 = GetRect(item2);
				if (IsContain(((RectangleD)(ref rect2)).Right, lineX, distForResize))
				{
					item2.Width += offset;
					item2.Width = Align(item2.Width);
				}
				else if (((RectangleD)(ref rect2)).Left >= lineX - distForResize && resizeType == StiResizeType.Resize)
				{
					item2.Left += offset;
					item2.Left = Align(item2.Left);
				}
				else if (((RectangleD)(ref rect2)).Left < lineX && ((RectangleD)(ref rect2)).Right > lineX && resizeType == StiResizeType.Resize)
				{
					item2.Width += offset;
					item2.Width = Align(item2.Width);
				}
				else if (IsContain(((RectangleD)(ref rect2)).Left, lineX, distForResize) && resizeType == StiResizeType.TwoResize)
				{
					item2.Left += offset;
					item2.Width -= offset;
					item2.Left = Align(item2.Left);
					item2.Width = Align(item2.Width);
				}
			}
		}
	}

	public void ProcessResizeRows()
	{
		double num = ConvertValue(Cursor.Position.Y);
		double num2 = ConvertValue(storedPosition.Y);
		double offset = (num - num2) * StiScale.System;
		StiResizeType resizeType = (IsControlKeyPressed ? StiResizeType.TwoResize : StiResizeType.Resize);
		ResizeRows(mouseY, offset, resizeType);
	}

	public void ResizeRows(double lineY, double offset, StiResizeType resizeType)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		double distForResize = GetDistForResize(designer.Report.GetCurrentPage());
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if ((!designer.Report.IsPageDesigner && item is IStiTableCell && (!(designer.SelectedComponentsOnPage[0] is StiTable stiTable) || stiTable.Components.Count + 1 != designer.SelectedComponentsOnPage.Count)) || !IsAllowUseInTableMode(item))
			{
				continue;
			}
			RectangleD clientRectangle = item.ClientRectangle;
			RestoreComponentLocation(item);
			try
			{
				RectangleD rect = GetRect(item);
				if (IsContain(((RectangleD)(ref rect)).Bottom, lineY, distForResize))
				{
					if (item.Height + offset < 0.0)
					{
						offset = 0.0 - item.Height;
					}
				}
				else if (((RectangleD)(ref rect)).Top < lineY && ((RectangleD)(ref rect)).Bottom > lineY && resizeType == StiResizeType.Resize)
				{
					if (item.Height + offset < 0.0)
					{
						offset = 0.0 - item.Height;
					}
				}
				else if (IsContain(((RectangleD)(ref rect)).Top, lineY, distForResize) && resizeType == StiResizeType.TwoResize && item.Height - offset < 0.0)
				{
					offset = item.Height;
				}
			}
			finally
			{
				item.ClientRectangle = clientRectangle;
			}
		}
		RestoreComponentsLocation();
		foreach (StiComponent item2 in designer.SelectedComponentsOnPage)
		{
			if ((!(item2 is IStiTableCell) || (designer.SelectedComponentsOnPage[0] is StiTable stiTable2 && stiTable2.Components.Count + 1 == designer.SelectedComponentsOnPage.Count)) && IsAllowUseInTableMode(item2))
			{
				RectangleD rect2 = GetRect(item2);
				if (IsContain(((RectangleD)(ref rect2)).Bottom, lineY, distForResize))
				{
					item2.Height += offset;
					item2.Height = Align(item2.Height);
				}
				else if (((RectangleD)(ref rect2)).Top >= lineY - distForResize && resizeType == StiResizeType.Resize)
				{
					item2.Top += offset;
					item2.Top = Align(item2.Top);
				}
				else if (((RectangleD)(ref rect2)).Top < lineY && ((RectangleD)(ref rect2)).Bottom > lineY && resizeType == StiResizeType.Resize)
				{
					item2.Height += offset;
					item2.Height = Align(item2.Height);
				}
				else if (IsContain(((RectangleD)(ref rect2)).Top, lineY, distForResize) && resizeType == StiResizeType.TwoResize)
				{
					item2.Top += offset;
					item2.Height -= offset;
					item2.Top = Align(item2.Top);
					item2.Height = Align(item2.Height);
				}
			}
		}
	}

	public void ProcessResizeTable(bool resizeHorizontally, bool resizeVertically)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected I4, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		RectangleD selectedRectangle = GetSelectedRectangle(designer);
		Point position = Cursor.Position;
		bool flag = false;
		bool flag2 = false;
		StiAction currentAction = designer.Report.Info.CurrentAction;
		if ((int)currentAction == 3 || currentAction - 7 <= 1)
		{
			flag = true;
		}
		StiAction currentAction2 = designer.Report.Info.CurrentAction;
		switch (currentAction2 - 5)
		{
		case 0:
		case 2:
		case 4:
			flag2 = true;
			break;
		}
		double num = ConvertValue(position.X);
		double num2 = ConvertValue(position.Y);
		double num3 = ConvertValue(storedPosition.X);
		double num4 = ConvertValue(storedPosition.Y);
		double num6;
		double num5;
		if (flag)
		{
			num5 = num3 - num;
			num6 = ((RectangleD)(ref selectedRectangle)).Right;
		}
		else
		{
			num5 = num - num3;
			num6 = ((RectangleD)(ref selectedRectangle)).X;
		}
		double num8;
		double num7;
		if (flag2)
		{
			num7 = num4 - num2;
			num8 = ((RectangleD)(ref selectedRectangle)).Bottom;
		}
		else
		{
			num7 = num2 - num4;
			num8 = ((RectangleD)(ref selectedRectangle)).Y;
		}
		double system = StiScale.System;
		num5 *= system;
		num7 *= system;
		double num9 = (((RectangleD)(ref storedSelectedRect)).Width + num5) / ((RectangleD)(ref storedSelectedRect)).Width;
		double num10 = (((RectangleD)(ref storedSelectedRect)).Height + num7) / ((RectangleD)(ref storedSelectedRect)).Height;
		if (num9 < 0.0 || double.IsNaN(num9) || double.IsInfinity(num9) || num10 < 0.0 || double.IsNaN(num10) || double.IsInfinity(num10))
		{
			return;
		}
		RestoreComponentsLocation();
		selectedRectangle = GetSelectedRectangle(designer);
		if (flag2)
		{
			if (num8 < ((RectangleD)(ref selectedRectangle)).Bottom)
			{
				num8 = ((RectangleD)(ref selectedRectangle)).Bottom;
			}
		}
		else if (num8 > ((RectangleD)(ref selectedRectangle)).Top)
		{
			num8 = ((RectangleD)(ref selectedRectangle)).Top;
		}
		if (flag)
		{
			if (num6 < ((RectangleD)(ref selectedRectangle)).Right)
			{
				num6 = ((RectangleD)(ref selectedRectangle)).Right;
			}
		}
		else if (num6 > ((RectangleD)(ref selectedRectangle)).Left)
		{
			num6 = ((RectangleD)(ref selectedRectangle)).Left;
		}
		ResizeTable(num6, num8, num9, num10, resizeHorizontally, resizeVertically, flag, flag2);
	}

	public void ResizeTable(double startX, double startY, double dpiX, double dpiY, bool resizeHorizontally, bool resizeVertically, bool invertX, bool invertY)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if ((!(item is IStiTableCell) || (designer.SelectedComponentsOnPage[0] is StiTable stiTable && stiTable.Components.Count + 1 == designer.SelectedComponentsOnPage.Count)) && IsAllowUseInTableMode(item))
			{
				RectangleD val = GetRect(item);
				RectangleD val2 = val;
				((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width * dpiX;
				((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height * dpiY;
				((RectangleD)(ref val)).X = (invertX ? (startX - (startX - ((RectangleD)(ref val)).X) * dpiX) : (startX + (((RectangleD)(ref val)).X - startX) * dpiX));
				((RectangleD)(ref val)).Y = (invertY ? (startY - (startY - ((RectangleD)(ref val)).Y) * dpiY) : (startY + (((RectangleD)(ref val)).Y - startY) * dpiY));
				val = ((RectangleD)(ref val)).Normalize();
				((RectangleD)(ref val))._002Ector(Align(((RectangleD)(ref val)).X), Align(((RectangleD)(ref val)).Y), Align(((RectangleD)(ref val)).Width), Align(((RectangleD)(ref val)).Height));
				hashtable[Round(((RectangleD)(ref val2)).Left)] = ((RectangleD)(ref val)).X;
				hashtable2[Round(((RectangleD)(ref val2)).Top)] = ((RectangleD)(ref val)).Y;
				hashtable[Round(((RectangleD)(ref val2)).Right)] = ((RectangleD)(ref val)).Right;
				hashtable2[Round(((RectangleD)(ref val2)).Bottom)] = ((RectangleD)(ref val)).Bottom;
			}
		}
		foreach (StiComponent item2 in designer.SelectedComponentsOnPage)
		{
			if ((item2 is IStiTableCell && (!(designer.SelectedComponentsOnPage[0] is StiTable stiTable2) || stiTable2.Components.Count + 1 != designer.SelectedComponentsOnPage.Count)) || !IsAllowUseInTableMode(item2))
			{
				continue;
			}
			RectangleD rect = GetRect(item2);
			object obj = hashtable[Round(((RectangleD)(ref rect)).Left)];
			object obj2 = hashtable2[Round(((RectangleD)(ref rect)).Top)];
			object obj3 = hashtable[Round(((RectangleD)(ref rect)).Right)];
			object obj4 = hashtable2[Round(((RectangleD)(ref rect)).Bottom)];
			if (obj != null && obj2 != null && obj3 != null && obj4 != null)
			{
				double num = (double)obj;
				double num2 = (double)obj2;
				double num3 = (double)obj3;
				double num4 = (double)obj4;
				if (!resizeHorizontally)
				{
					num = ((RectangleD)(ref rect)).Left;
					num3 = ((RectangleD)(ref rect)).Right;
				}
				if (!resizeVertically)
				{
					num2 = ((RectangleD)(ref rect)).Top;
					num4 = ((RectangleD)(ref rect)).Bottom;
				}
				SetRect(item2, new RectangleD(num, num2, num3 - num, num4 - num2));
			}
		}
	}

	public void SaveSelectedRectangle()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		storedSelectedRect = GetSelectedRectangle(designer);
	}

	public void SaveComponentsLocation()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (storedLocations == null)
		{
			storedLocations = new Hashtable();
		}
		else
		{
			storedLocations.Clear();
		}
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if (IsAllowUseInTableMode(item))
			{
				storedLocations[item] = item.ClientRectangle;
			}
		}
	}

	public void RestoreComponentsLocation()
	{
		if (storedLocations == null)
		{
			return;
		}
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if (IsAllowUseInTableMode(item))
			{
				RestoreComponentLocation(item);
			}
		}
	}

	public void RestoreComponentLocation(StiComponent comp)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (storedLocations[comp] is RectangleD)
		{
			comp.ClientRectangle = (RectangleD)storedLocations[comp];
		}
	}

	public void SaveCursorLocation(int eX, int eY)
	{
		storedPosition = Cursor.Position;
		mouseX = designer.XToPage(eX);
		mouseY = designer.YToPage(eY);
	}

	public void Dispose()
	{
		storedLocations?.Clear();
		storedLocations = null;
	}

	public StiTableHelper(IStiDesignerBase designer, bool altKeyMode)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		this.designer = designer;
		this.altKeyMode = altKeyMode;
	}
}
