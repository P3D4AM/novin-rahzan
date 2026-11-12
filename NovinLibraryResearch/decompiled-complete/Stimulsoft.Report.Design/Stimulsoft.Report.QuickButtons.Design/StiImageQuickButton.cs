using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiImageQuickButton : StiQuickButton
{
	private StiImage imageComponent;

	private string componentContent;

	public override Image Image => StiReportDesignImages.QuickButtons.Field();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Field_Selected();

	public override string Name => StiLocalization.Get("Components", "StiImage");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase designer = imageComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiImage"), onlyCurrentPage: true);
			imageComponent.ResetAllImageProperties();
			StiImage stiImage = imageComponent;
			object tag = buttonItem.Tag;
			stiImage.DataColumn = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			imageComponent.ResetImageToDraw();
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
			IStiDesignerBase designer = imageComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
			imageComponent.ResetAllImageProperties();
			imageComponent.DataColumn = "{" + buttonItem.Tag?.ToString() + "}";
			imageComponent.ResetImageToDraw();
			designer.DesignerControl.TextAliasesHash.Clear();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnClearContent(object sender, EventArgs e)
	{
		try
		{
			IStiDesignerBase designer = imageComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditClearContents"), onlyCurrentPage: true);
			imageComponent.ResetAllImageProperties();
			imageComponent.ResetImageToDraw();
			designer.DesignerControl.TextAliasesHash.Clear();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetResource(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			StiResource stiResource = buttonItem.Tag as StiResource;
			IStiDesignerBase designer = imageComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Resource"), onlyCurrentPage: true);
			imageComponent.ResetAllImageProperties();
			imageComponent.ImageURL.Value = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
			imageComponent.ResetImageToDraw();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetVariable(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			StiVariable stiVariable = buttonItem.Tag as StiVariable;
			IStiDesignerBase designer = imageComponent.Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Variable"), onlyCurrentPage: true);
			imageComponent.ResetAllImageProperties();
			imageComponent.ImageURL.Value = StiHyperlinkProcessor.CreateVariableName(stiVariable.Name);
			imageComponent.ResetImageToDraw();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		try
		{
			ButtonItem contextMenu = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
			contextMenu.SubItems.Clear();
			imageComponent = component as StiImage;
			componentContent = imageComponent.DataColumn;
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuEditClearContents"),
				Image = StiQuickButtonHelper.ClearContentImage
			};
			buttonItem.Click += OnClearContent;
			contextMenu.SubItems.Add(buttonItem);
			StiDataBand dataBand = component.GetDataBand();
			if (dataBand != null && dataBand.DataSource != null)
			{
				StiQuickButtonHelper.BuildDataSource(contextMenu, dataBand, componentContent, OnSetField);
			}
			if (dataBand != null && dataBand.BusinessObject != null)
			{
				StiQuickButtonHelper.BuildBusinessObject(contextMenu, dataBand, componentContent, OnSetBusinessObjectField);
			}
			int index = 0;
			List<StiResource> list = (from r in imageComponent.Report.Dictionary.Resources.ToList()
				where r.Type == StiResourceType.Image
				select r).ToList();
			list.ForEach(delegate(StiResource r)
			{
				ButtonItem buttonItem3 = new ButtonItem
				{
					BeginGroup = (index == 0),
					Text = r.Name,
					Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromResource(r)],
					Tag = r
				};
				buttonItem3.Click += OnSetResource;
				contextMenu.SubItems.Add(buttonItem3);
				index++;
			});
			index = 0;
			List<StiVariable> list2 = (from v in imageComponent.Report.Dictionary.Variables.ToList()
				where StiTypeFinder.FindType(v.Type, typeof(Image))
				select v).ToList();
			list2.ForEach(delegate(StiVariable v)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					BeginGroup = (index == 0),
					Text = v.Name,
					Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromVariable(v)],
					Tag = v
				};
				buttonItem2.Click += OnSetVariable;
				contextMenu.SubItems.Add(buttonItem2);
				index++;
			});
			contextMenu.PopupMenu(control.PointToScreen(pos));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return StiDataBuilder.GetColumnPathFromNode(treeNode, useRelationName: true);
	}
}
