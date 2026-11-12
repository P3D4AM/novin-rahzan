using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Tools;

namespace Stimulsoft.Report.Design;

public class StiInsertionComponents
{
	public static void InsertGroups(StiDesignerControl designer, StiGroup group)
	{
		InsertGroups(designer, group, alignToGrid: false);
	}

	public static void InsertGroups(StiDesignerControl designer, StiGroup group, bool alignToGrid)
	{
		List<StiComponent> components = group.Components.ToList();
		StiPage currentPage = designer.Report.GetCurrentPage();
		components = StiComponentsByPageTypeFiltrator.Filter(currentPage, components);
		foreach (StiComponent item in components)
		{
			item.Dockable = false;
			if (!item.Inherited)
			{
				continue;
			}
			item.Inherited = false;
			if (!(item is StiContainer))
			{
				continue;
			}
			foreach (StiComponent component in (item as StiContainer).GetComponents())
			{
				component.Inherited = false;
			}
		}
		group.InsertIntoPage(currentPage);
		Insert(designer, alignToGrid);
	}

	public static void InsertComponents(StiDesignerControl designer, StiComponentsCollection comps)
	{
		InsertComponents(designer, comps, alignToGrid: false);
	}

	public static void InsertComponents(StiDesignerControl designer, StiComponentsCollection comps, bool alignToGrid)
	{
		designer.Report.GetCurrentPage().ResetSelection();
		foreach (StiComponent comp in comps)
		{
			comp.Select();
			comp.Dockable = false;
		}
		designer.Report.GetCurrentPage().Components.AddRange(comps);
		Insert(designer, alignToGrid);
	}

	public static void Insert(StiDesignerControl designer)
	{
		Insert(designer, alignToGrid: false);
	}

	public static void Insert(StiDesignerControl designer, bool alignToGrid)
	{
		StiSelectToolService selectToolService = StiSelectToolService.GetSelectToolService(designer);
		if (selectToolService == null)
		{
			return;
		}
		designer.Report.Info.IsComponentsMoving = true;
		selectToolService.Action = StiAction.Move;
		selectToolService.InsertComponents = true;
		PointD currentCursorPos = designer.PageView.GetCurrentCursorPos();
		double num = 0.0;
		double num2 = 0.0;
		StiComponentsCollection selectedComponents = designer.Report.GetCurrentPage().GetSelectedComponents();
		if (selectedComponents.Count > 0)
		{
			num = selectedComponents[0].Left;
			num2 = selectedComponents[0].Top;
		}
		foreach (StiComponent item in selectedComponents)
		{
			if (item.IsSelected)
			{
				if (alignToGrid)
				{
					item.ClientRectangle = item.ClientRectangle.AlignToGrid(designer.Report.GetCurrentPage().GridSize, designer.Report.Info.AlignToGrid);
				}
				num = Math.Min(item.Left, num);
				num2 = Math.Min(item.Top, num2);
			}
		}
		foreach (StiComponent item2 in selectedComponents)
		{
			if (item2.IsSelected)
			{
				item2.Left -= num - currentCursorPos.X;
				item2.Top -= num2 - currentCursorPos.Y;
			}
		}
		designer.PageView.StartPos = Cursor.Position;
		StiSelectToolService selectToolService2 = StiSelectToolService.GetSelectToolService(designer);
		selectToolService2.RedrawAll = true;
		selectToolService2.AltKeyMode = selectToolService2.AltKey;
	}

	private StiInsertionComponents()
	{
	}
}
