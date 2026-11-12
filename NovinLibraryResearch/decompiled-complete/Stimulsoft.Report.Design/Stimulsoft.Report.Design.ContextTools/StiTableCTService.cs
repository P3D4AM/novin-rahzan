using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTableCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 90;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableCTService)) as StiTableCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		if (designer.Report.IsPageDesigner)
		{
			return;
		}
		StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
		if (selectedComponentsOnPage == null || selectedComponentsOnPage.Count == 0)
		{
			return;
		}
		bool join = false;
		List<StiTablceCellType> list = new List<StiTablceCellType>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			if (selectedComponentsOnPage[i] is IStiTableCell stiTableCell)
			{
				if (stiTableCell.Merged)
				{
					join = true;
				}
				if (!list.Contains(stiTableCell.CellType))
				{
					list.Add(stiTableCell.CellType);
				}
				continue;
			}
			return;
		}
		if (designer.SelectedComponentsOnPage[0].Parent is StiTable)
		{
			base.Designer = designer;
			StiTable parent = (StiTable)designer.SelectedComponentsOnPage[0].Parent;
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuTable"),
				Image = StiReportDesignImages.Actions.EditTable()
			};
			StiTableCellJoinCTService stiTableCellJoinCTService = new StiTableCellJoinCTService();
			stiTableCellJoinCTService.CreateMenuItem(designer, buttonItem, parent, join);
			StiTableInsertColumnToLeftCTService stiTableInsertColumnToLeftCTService = new StiTableInsertColumnToLeftCTService();
			stiTableInsertColumnToLeftCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableInsertColumnToRightCTService stiTableInsertColumnToRightCTService = new StiTableInsertColumnToRightCTService();
			stiTableInsertColumnToRightCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableDeleteColumnCTService stiTableDeleteColumnCTService = new StiTableDeleteColumnCTService();
			stiTableDeleteColumnCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableSelectColumnCTService stiTableSelectColumnCTService = new StiTableSelectColumnCTService();
			stiTableSelectColumnCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableInsertRowAboveCTService stiTableInsertRowAboveCTService = new StiTableInsertRowAboveCTService();
			stiTableInsertRowAboveCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableInsertRowBelowCTService stiTableInsertRowBelowCTService = new StiTableInsertRowBelowCTService();
			stiTableInsertRowBelowCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableDeleteRowCTService stiTableDeleteRowCTService = new StiTableDeleteRowCTService();
			stiTableDeleteRowCTService.CreateMenuItem(designer, buttonItem, parent);
			StiTableSelectRowCTService stiTableSelectRowCTService = new StiTableSelectRowCTService();
			stiTableSelectRowCTService.CreateMenuItem(designer, buttonItem, parent);
			ButtonItem buttonItem2 = new ButtonItem
			{
				BeginGroup = true,
				Image = StiReportDesignImages.Components.Text(),
				Text = StiLocalization.Get("MainMenu", "menuConvertToText"),
				Tag = StiTablceCellType.Text
			};
			if (list.Contains(StiTablceCellType.Text))
			{
				buttonItem2.Checked = true;
			}
			buttonItem2.Click += Convert_Click;
			buttonItem.SubItems.Add(buttonItem2);
			ButtonItem buttonItem3 = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuConvertToImage"),
				Image = StiReportDesignImages.Components.Image(),
				Tag = StiTablceCellType.Image
			};
			if (list.Contains(StiTablceCellType.Image))
			{
				buttonItem3.Checked = true;
			}
			buttonItem3.Click += Convert_Click;
			buttonItem.SubItems.Add(buttonItem3);
			ButtonItem buttonItem4 = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuConvertToCheckBox"),
				Image = StiReportDesignImages.Components.CheckBox(),
				Tag = StiTablceCellType.CheckBox
			};
			if (list.Contains(StiTablceCellType.CheckBox))
			{
				buttonItem4.Checked = true;
			}
			buttonItem4.Click += Convert_Click;
			buttonItem.SubItems.Add(buttonItem4);
			ButtonItem buttonItem5 = new ButtonItem
			{
				Tag = StiTablceCellType.RichText,
				Text = StiLocalization.Get("MainMenu", "MenuConvertToRichText"),
				Image = StiReportDesignImages.Components.RichText()
			};
			if (list.Contains(StiTablceCellType.RichText))
			{
				buttonItem5.Checked = true;
			}
			buttonItem5.Click += Convert_Click;
			buttonItem.SubItems.Add(buttonItem5);
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}

	private void Convert_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave("ChangeCellType", onlyCurrentPage: true);
		StiTablceCellType cellType = (StiTablceCellType)((ButtonItem)sender).Tag;
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			if (selectedComponentsOnPage[i] is IStiTableCell stiTableCell)
			{
				stiTableCell.CellType = cellType;
			}
		}
		((Control)base.Designer).Refresh();
	}
}
