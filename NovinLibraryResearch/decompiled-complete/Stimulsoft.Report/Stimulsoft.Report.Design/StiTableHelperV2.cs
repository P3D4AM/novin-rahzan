using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

public class StiTableHelperV2
{
	private IStiDesignerBase designer;

	private bool altKeyMode;

	private Hashtable storedLocations;

	private Point storedPosition = Point.Empty;

	private double mouseX;

	private double mouseY;

	private RectangleD storedSelectedRect = RectangleD.Empty;

	public IStiTableCell SelectedCell { get; set; }

	public StiTable SelectedTable { get; set; }

	public bool IsSelectedCell => SelectedCell != null;

	public bool IsSelectedTable => SelectedTable != null;

	public bool IsAnySelected
	{
		get
		{
			if (SelectedTable == null)
			{
				return SelectedCell != null;
			}
			return true;
		}
	}

	public void ProcessResizeCell()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected I4, but got Unknown
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
		double num5 = ((!flag) ? (num - num3) : (num3 - num));
		double num6 = ((!flag2) ? (num2 - num4) : (num4 - num2));
		double system = StiScale.System;
		num5 *= system;
		num6 *= system;
		ResizeCell(num5, num6);
	}

	public void ResizeCell(double offsetX, double offsetY)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault((StiComponent c) => c is IStiTableCell);
		if (stiComponent != null && stiComponent.Parent is StiTable table)
		{
			RestoreComponentsLocation(table);
			ResizeBasicCell(stiComponent, offsetX, offsetY, designer.Report.Info.CurrentAction);
		}
	}

	private void ResizeBasicCell(StiComponent tableCell, double offsetX, double offsetY, StiAction action)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected I4, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		double distForResize = GetDistForResize(designer.Report.GetCurrentPage());
		StiTable comp = tableCell.Parent as StiTable;
		Dictionary<StiComponent, RectangleD> changedComponentList = new Dictionary<StiComponent, RectangleD>();
		Dictionary<StiComponent, RectangleD> changedComponentList2 = new Dictionary<StiComponent, RectangleD>();
		RectangleD rect = GetRect(comp);
		RectangleD rect2 = GetRect(tableCell);
		RectangleD val = rect2;
		bool flag = (decimal)((RectangleD)(ref rect)).Left == (decimal)((RectangleD)(ref rect2)).Left;
		bool flag2 = (decimal)((RectangleD)(ref rect)).Top == (decimal)((RectangleD)(ref rect2)).Top;
		bool flag3 = (decimal)((RectangleD)(ref rect)).Right == (decimal)((RectangleD)(ref rect2)).Right;
		bool flag4 = (decimal)((RectangleD)(ref rect)).Bottom == (decimal)((RectangleD)(ref rect2)).Bottom;
		switch (action - 3)
		{
		case 0:
			if (((RectangleD)(ref rect2)).X - offsetX > ((RectangleD)(ref rect2)).Right)
			{
				return;
			}
			if (!flag)
			{
				((RectangleD)(ref rect2)).X = ((RectangleD)(ref rect2)).X - offsetX;
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).X + distForResize, offsetX, (StiAction)3, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).X - distForResize, 0.0 - offsetX, (StiAction)4, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			break;
		case 4:
			if (((RectangleD)(ref rect2)).X - offsetX > ((RectangleD)(ref rect2)).Right || ((RectangleD)(ref rect2)).Y - offsetY > ((RectangleD)(ref rect2)).Bottom)
			{
				return;
			}
			if (!flag)
			{
				((RectangleD)(ref rect2)).X = ((RectangleD)(ref rect2)).X - offsetX;
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).X - distForResize, 0.0 - offsetX, (StiAction)4, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).X + distForResize, offsetX, (StiAction)3, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			if (!flag2)
			{
				((RectangleD)(ref rect2)).Y = ((RectangleD)(ref rect2)).Y - offsetY;
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Y - distForResize, 0.0 - offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Y + distForResize, offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			break;
		case 2:
			if (((RectangleD)(ref rect2)).Y - offsetY > ((RectangleD)(ref rect2)).Bottom)
			{
				return;
			}
			if (!flag2)
			{
				((RectangleD)(ref rect2)).Y = ((RectangleD)(ref rect2)).Y - offsetY;
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Y - distForResize, 0.0 - offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Y + distForResize, offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			break;
		case 6:
			if (((RectangleD)(ref rect2)).Y - offsetY > ((RectangleD)(ref rect2)).Bottom || ((RectangleD)(ref rect2)).Width + offsetX <= 0.0)
			{
				return;
			}
			if (!flag2)
			{
				((RectangleD)(ref rect2)).Y = ((RectangleD)(ref rect2)).Y - offsetY;
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Y - distForResize, 0.0 - offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Y + distForResize, offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			if (!flag3)
			{
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).Right - distForResize, offsetX, (StiAction)4, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).Right + distForResize, 0.0 - offsetX, (StiAction)3, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			break;
		case 1:
			if (((RectangleD)(ref rect2)).Width + offsetX <= 0.0)
			{
				return;
			}
			if (!flag3)
			{
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).Right - distForResize, offsetX, (StiAction)4, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).Right + distForResize, 0.0 - offsetX, (StiAction)3, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			break;
		case 7:
			if (((RectangleD)(ref rect2)).Width + offsetX <= 0.0 || ((RectangleD)(ref rect2)).Height + offsetY <= 0.0)
			{
				return;
			}
			if (!flag3)
			{
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).Right - distForResize, offsetX, (StiAction)4, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).Right + distForResize, 0.0 - offsetX, (StiAction)3, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			if (!flag4)
			{
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom - distForResize, offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom + distForResize, 0.0 - offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			break;
		case 3:
			if (((RectangleD)(ref rect2)).Height + offsetY <= 0.0)
			{
				return;
			}
			if (!flag4)
			{
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom - distForResize, offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom + distForResize, 0.0 - offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			break;
		case 5:
			if (((RectangleD)(ref rect2)).X - offsetX > ((RectangleD)(ref rect2)).Right || ((RectangleD)(ref rect2)).Height + offsetY <= 0.0)
			{
				return;
			}
			if (!flag)
			{
				((RectangleD)(ref rect2)).X = ((RectangleD)(ref rect2)).X - offsetX;
				((RectangleD)(ref rect2)).Width = ((RectangleD)(ref rect2)).Width + offsetX;
				if (!ResizeColumns(tableCell, ((RectangleD)(ref val)).X - distForResize, 0.0 - offsetX, (StiAction)4, ref changedComponentList) || !ResizeColumns(tableCell, ((RectangleD)(ref val)).X + distForResize, offsetX, (StiAction)3, ref changedComponentList))
				{
					return;
				}
				SetPaintRectangle(changedComponentList);
			}
			if (!flag4)
			{
				((RectangleD)(ref rect2)).Height = ((RectangleD)(ref rect2)).Height + offsetY;
				if (!ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom - distForResize, offsetY, (StiAction)6, ref changedComponentList2) || !ResizeRows(tableCell, ((RectangleD)(ref val)).Bottom + distForResize, 0.0 - offsetY, (StiAction)5, ref changedComponentList2))
				{
					return;
				}
				SetPaintRectangle(changedComponentList2);
			}
			break;
		}
		tableCell.SetPaintRectangle(rect2);
	}

	private void SetPaintRectangle(Dictionary<StiComponent, RectangleD> list)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiComponent key in list.Keys)
		{
			key.SetPaintRectangle(list[key]);
		}
	}

	private bool TryResizeCell(StiComponent tableCell, double offsetX, double offsetY, StiAction action, ref RectangleD rectangle)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return ResizeCell(tableCell, offsetX, offsetY, action, ref rectangle, applyResize: false);
	}

	private bool ResizeCell(StiComponent tableCell, double offsetX, double offsetY, StiAction action, ref RectangleD rectangle, bool applyResize = true)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected I4, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = GetRect(tableCell);
		double distForResize = GetDistForResize(designer.Report.GetCurrentPage());
		switch (action - 3)
		{
		case 0:
			if (((RectangleD)(ref rect)).X - offsetX > ((RectangleD)(ref rect)).Right - distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - offsetX;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			break;
		case 4:
			if (((RectangleD)(ref rect)).X - offsetX > ((RectangleD)(ref rect)).Right - distForResize || ((RectangleD)(ref rect)).Y - offsetY > ((RectangleD)(ref rect)).Bottom - distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - offsetX;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - offsetY;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		case 2:
			if (((RectangleD)(ref rect)).Y - offsetY > ((RectangleD)(ref rect)).Bottom - distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - offsetY;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		case 6:
			if (((RectangleD)(ref rect)).Y - offsetY > ((RectangleD)(ref rect)).Bottom - distForResize || ((RectangleD)(ref rect)).Width + offsetX <= distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - offsetY;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		case 1:
			if (((RectangleD)(ref rect)).Width + offsetX <= distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			break;
		case 7:
			if (((RectangleD)(ref rect)).Width + offsetX <= distForResize || ((RectangleD)(ref rect)).Height + offsetY <= distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		case 3:
			if (((RectangleD)(ref rect)).Height + offsetY <= distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		case 5:
			if (((RectangleD)(ref rect)).X - offsetX > ((RectangleD)(ref rect)).Right - distForResize || ((RectangleD)(ref rect)).Height + offsetY <= distForResize)
			{
				return false;
			}
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - offsetX;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + offsetX;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + offsetY;
			break;
		}
		rectangle = rect;
		if (applyResize)
		{
			tableCell.SetPaintRectangle(rect);
		}
		return true;
	}

	public bool ResizeColumns(StiComponent baseCell, double lineX, double offset, StiAction action, ref Dictionary<StiComponent, RectangleD> changedComponentList)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (offset != 0.0)
		{
			StiTable stiTable = baseCell.Parent as StiTable;
			double num = GetDistForResize(designer.Report.GetCurrentPage()) * 1.1;
			foreach (StiComponent component in stiTable.Components)
			{
				if (changedComponentList.Keys.Contains(component) || baseCell == component)
				{
					continue;
				}
				RectangleD rect = GetRect(component);
				if ((((RectangleD)(ref rect)).Left < lineX && ((RectangleD)(ref rect)).Left + num >= lineX) || (((RectangleD)(ref rect)).Right > lineX && ((RectangleD)(ref rect)).Right - num <= lineX))
				{
					RectangleD rectangle = default(RectangleD);
					if (!TryResizeCell(component, offset, 0.0, action, ref rectangle))
					{
						return false;
					}
					changedComponentList.Add(component, rectangle);
				}
			}
		}
		return true;
	}

	public bool ResizeRows(StiComponent baseCell, double lineY, double offset, StiAction action, ref Dictionary<StiComponent, RectangleD> changedComponentList)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (offset != 0.0)
		{
			StiTable stiTable = baseCell.Parent as StiTable;
			double num = GetDistForResize(designer.Report.GetCurrentPage()) * 1.1;
			foreach (StiComponent component in stiTable.Components)
			{
				if (changedComponentList.Keys.Contains(component) || baseCell == component)
				{
					continue;
				}
				RectangleD rect = GetRect(component);
				if ((((RectangleD)(ref rect)).Top < lineY && ((RectangleD)(ref rect)).Top + num >= lineY) || (((RectangleD)(ref rect)).Bottom > lineY && ((RectangleD)(ref rect)).Bottom - num <= lineY))
				{
					RectangleD rectangle = default(RectangleD);
					if (!TryResizeCell(component, 0.0, offset, action, ref rectangle))
					{
						return false;
					}
					changedComponentList.Add(component, rectangle);
				}
			}
		}
		return true;
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

	public void ProcessResizeTable(bool resizeHorizontally, bool resizeVertically)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected I4, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		StiTable selectedTable = SelectedTable;
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
		RestoreComponentsLocation(selectedTable);
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
		ResizeTable(selectedTable, num6, num8, num9, num10, resizeHorizontally, resizeVertically, flag, flag2);
	}

	public void ResizeTable(StiTable table, double startX, double startY, double dpiX, double dpiY, bool resizeHorizontally, bool resizeVertically, bool invertX, bool invertY)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiComponent component in table.Components)
		{
			if (IsAllowUseInTableMode(component))
			{
				RectangleD val = GetRect(component);
				RectangleD val2 = val;
				((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width * dpiX;
				((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height * dpiY;
				((RectangleD)(ref val)).X = (invertX ? (startX - (startX - ((RectangleD)(ref val)).X) * dpiX) : (startX + (((RectangleD)(ref val)).X - startX) * dpiX));
				((RectangleD)(ref val)).Y = (invertY ? (startY - (startY - ((RectangleD)(ref val)).Y) * dpiY) : (startY + (((RectangleD)(ref val)).Y - startY) * dpiY));
				val = ((RectangleD)(ref val)).Normalize();
				hashtable[Round(((RectangleD)(ref val2)).Left)] = ((RectangleD)(ref val)).X;
				hashtable2[Round(((RectangleD)(ref val2)).Top)] = ((RectangleD)(ref val)).Y;
				hashtable[Round(((RectangleD)(ref val2)).Right)] = ((RectangleD)(ref val)).Right;
				hashtable2[Round(((RectangleD)(ref val2)).Bottom)] = ((RectangleD)(ref val)).Bottom;
			}
		}
		foreach (StiComponent component2 in table.Components)
		{
			if (!IsAllowUseInTableMode(component2))
			{
				continue;
			}
			RectangleD rect = GetRect(component2);
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
				component2.SetPaintRectangle(new RectangleD(num, num2, num3 - num, num4 - num2));
			}
		}
	}

	public void RestoreComponentsLocation()
	{
		if (SelectedCell != null)
		{
			StiTable table = ((StiComponent)SelectedCell).Parent as StiTable;
			RestoreComponentsLocation(table);
		}
	}

	public void RestoreComponentsLocation(StiTable table)
	{
		if (storedLocations == null)
		{
			return;
		}
		foreach (StiComponent component in table.Components)
		{
			if (IsAllowUseInTableMode(component))
			{
				RestoreComponentLocation(component);
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

	private double ConvertValue(double value)
	{
		value = designer.Report.GetCurrentPage().Unit.ConvertFromHInches(value / designer.Report.GetCurrentPage().Zoom);
		value = StiAlignValue.AlignToGrid(value, designer.Report.GetCurrentPage().GridSize, designer.Report.Info.AlignToGrid && !altKeyMode);
		return value;
	}

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

	public void SaveComponentsLocation(StiTable table)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (table == null)
		{
			return;
		}
		if (storedLocations == null)
		{
			storedLocations = new Hashtable();
		}
		else
		{
			storedLocations.Clear();
		}
		foreach (StiComponent component in table.Components)
		{
			if (IsAllowUseInTableMode(component))
			{
				storedLocations[component] = component.ClientRectangle;
			}
		}
	}

	public void SaveCursorLocation(int eX, int eY)
	{
		storedPosition = Cursor.Position;
		mouseX = designer.XToPage(eX);
		mouseY = designer.YToPage(eY);
	}

	public void SaveSelectedRectangle()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		storedSelectedRect = GetSelectedRectangle(designer);
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

	public void Dispose()
	{
		SelectedCell = null;
		storedLocations?.Clear();
		storedLocations = null;
	}

	public StiTableHelperV2(IStiDesignerBase designer, bool altKeyMode)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		this.designer = designer;
		this.altKeyMode = altKeyMode;
	}
}
