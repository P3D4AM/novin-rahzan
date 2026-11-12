using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiTableCellRichTextQuickButton : StiQuickButton
{
	private StiTableCellRichText richTextComponent;

	private string componentContent;

	private IStiDesignerBase designer;

	public override Image Image => StiReportDesignImages.QuickButtons.Field();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Field_Selected();

	public override string Name => StiLocalization.Get("Components", "StiRichText");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase stiDesignerBase = richTextComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("Components", "StiRichText"), onlyCurrentPage: true);
			object tag = buttonItem.Tag;
			string columnPathFromNode = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			richTextComponent.DataColumn = columnPathFromNode.Substring(1, columnPathFromNode.Length - 2);
			stiDesignerBase.DesignerControl.TextAliasesHash.Clear();
			stiDesignerBase.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		this.designer = designer;
		ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
		cmQuickButtons.SubItems.Clear();
		richTextComponent = component as StiTableCellRichText;
		componentContent = "{" + richTextComponent.DataColumn + "}";
		if (!designer.Report.IsPageDesigner)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				BeginGroup = false,
				Text = Loc.Get("MainMenu", "menuConvertToText"),
				Tag = StiTablceCellType.Text,
				Image = StiReportDesignImages.Components.Text()
			};
			buttonItem.Click += ConvertToButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem
			{
				BeginGroup = false,
				Text = Loc.Get("MainMenu", "menuConvertToImage"),
				Tag = StiTablceCellType.Image,
				Image = StiReportDesignImages.Components.Image()
			};
			buttonItem2.Click += ConvertToButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem2);
			ButtonItem buttonItem3 = new ButtonItem
			{
				BeginGroup = false,
				Text = Loc.Get("MainMenu", "menuConvertToCheckBox"),
				Tag = StiTablceCellType.CheckBox,
				Image = StiReportDesignImages.Components.CheckBox()
			};
			buttonItem3.Click += ConvertToButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem3);
		}
		StiDataBand dataBand = component.GetDataBand();
		if (dataBand != null && dataBand.DataSource != null)
		{
			StiQuickButtonHelper.BuildDataSource(cmQuickButtons, dataBand, componentContent, OnSetField);
		}
		cmQuickButtons.PopupMenu(control.PointToScreen(pos));
	}

	private void ConvertToButton_Click(object sender, EventArgs e)
	{
		designer.UndoRedoSave("ChangeCellType", onlyCurrentPage: true);
		richTextComponent.CellType = (StiTablceCellType)((ButtonItem)sender).Tag;
		designer.Refresh();
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(treeNode, useRelationName: true) + "}";
	}
}
