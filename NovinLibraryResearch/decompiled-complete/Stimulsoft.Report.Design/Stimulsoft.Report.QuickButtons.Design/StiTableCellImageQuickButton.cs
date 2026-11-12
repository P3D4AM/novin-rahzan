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

public class StiTableCellImageQuickButton : StiQuickButton
{
	private StiTableCellImage imageComponent;

	private string componentContent;

	private IStiDesignerBase designer;

	public override Image Image => StiReportDesignImages.QuickButtons.Field();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Field_Selected();

	public override string Name => StiLocalization.Get("Components", "StiImage");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase stiDesignerBase = imageComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("Components", "StiImage"), onlyCurrentPage: true);
			StiTableCellImage stiTableCellImage = imageComponent;
			object tag = buttonItem.Tag;
			stiTableCellImage.DataColumn = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			stiDesignerBase.DesignerControl.TextAliasesHash.Clear();
			stiDesignerBase.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ClearContentsButton_Click(object sender, EventArgs e)
	{
		try
		{
			IStiDesignerBase stiDesignerBase = imageComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditClearContents"), onlyCurrentPage: true);
			imageComponent.DataColumn = string.Empty;
			stiDesignerBase.DesignerControl.TextAliasesHash.Clear();
			stiDesignerBase.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return StiDataBuilder.GetColumnPathFromNode(treeNode);
	}

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		try
		{
			this.designer = designer;
			ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
			cmQuickButtons.SubItems.Clear();
			imageComponent = component as StiTableCellImage;
			componentContent = imageComponent.DataColumn;
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuEditClearContents"),
				Image = StiQuickButtonHelper.ClearContentImage
			};
			buttonItem.Click += ClearContentsButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem);
			if (!designer.Report.IsPageDesigner)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					BeginGroup = true,
					Text = Loc.Get("MainMenu", "menuConvertToText"),
					Tag = StiTablceCellType.Text,
					Image = StiReportDesignImages.Components.Text()
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
				ButtonItem buttonItem4 = new ButtonItem
				{
					BeginGroup = false,
					Text = Loc.Get("MainMenu", "MenuConvertToRichText"),
					Tag = StiTablceCellType.RichText,
					Image = StiReportDesignImages.Components.RichText()
				};
				buttonItem4.Click += ConvertToButton_Click;
				cmQuickButtons.SubItems.Add(buttonItem4);
			}
			StiDataBand dataBand = component.GetDataBand();
			if (dataBand != null && dataBand.DataSource != null)
			{
				StiQuickButtonHelper.BuildDataSource(cmQuickButtons, dataBand, componentContent, OnSetField);
			}
			cmQuickButtons.PopupMenu(control.PointToScreen(pos));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ConvertToButton_Click(object sender, EventArgs e)
	{
		designer.UndoRedoSave("ChangeCellType", onlyCurrentPage: true);
		imageComponent.CellType = (StiTablceCellType)((ButtonItem)sender).Tag;
		designer.Refresh();
	}
}
