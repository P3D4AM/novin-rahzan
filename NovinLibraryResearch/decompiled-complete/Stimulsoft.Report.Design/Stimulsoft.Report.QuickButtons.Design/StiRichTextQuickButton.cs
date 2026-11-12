using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiRichTextQuickButton : StiQuickButton
{
	private StiRichText textComponent;

	private string componentContent;

	public override Image Image => StiReportDesignImages.QuickButtons.Field();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Field_Selected();

	public override string Name => StiLocalization.Get("Components", "StiRichText");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase designer = textComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiRichText"), onlyCurrentPage: true);
			textComponent.ResetSourceProperties();
			object tag = buttonItem.Tag;
			string columnPathFromNode = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			textComponent.DataColumn = columnPathFromNode.Substring(1, columnPathFromNode.Length - 2);
			designer.DesignerControl.TextAliasesHash.Clear();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetBusinessObjectField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase designer = textComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
			textComponent.ResetSourceProperties();
			textComponent.DataColumn = buttonItem.Tag as string;
			designer.DesignerControl.TextAliasesHash.Clear();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
		cmQuickButtons.SubItems.Clear();
		textComponent = component as StiRichText;
		componentContent = "{" + textComponent.DataColumn + "}";
		StiDataBand dataBand = component.GetDataBand();
		if (dataBand != null && dataBand.DataSource != null)
		{
			StiQuickButtonHelper.BuildDataSource(cmQuickButtons, dataBand, componentContent, OnSetField);
		}
		if (dataBand != null && dataBand.BusinessObject != null)
		{
			StiQuickButtonHelper.BuildBusinessObject(cmQuickButtons, dataBand, componentContent, OnSetBusinessObjectField);
		}
		cmQuickButtons.PopupMenu(control.PointToScreen(pos));
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(treeNode, useRelationName: true) + "}";
	}
}
