using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiTextQuickButton : StiQuickButton
{
	private IStiText textComponent;

	private string componentContent;

	public override Image Image => StiReportDesignImages.QuickButtons.Text();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Text_Selected();

	public override string Name => StiLocalization.Get("Components", "StiText");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase designer = ((StiComponent)textComponent).Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
			IStiText stiText = textComponent;
			object tag = buttonItem.Tag;
			stiText.Text = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			((StiText)textComponent).Type = StiSystemTextType.DataColumn;
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
			IStiDesignerBase designer = ((StiComponent)textComponent).Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
			textComponent.Text = "{" + buttonItem.Tag?.ToString() + "}";
			((StiText)textComponent).Type = StiSystemTextType.DataColumn;
			designer.DesignerControl.TextAliasesHash.Clear();
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void TextFormatButton_Click(object sender, EventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IStiDesignerBase designer = ((StiComponent)textComponent).Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "TextFormat"), onlyCurrentPage: true);
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(textComponent as IStiTextFormat, designer?.DesignerControl as StiDesignerControl);
			try
			{
				designer?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				((Form)stiTextFormatEditorForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
			designer.Refresh();
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
			IStiDesignerBase designer = ((StiComponent)textComponent).Report.Designer;
			designer?.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditClearContents"), onlyCurrentPage: true);
			textComponent.Text = string.Empty;
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
		try
		{
			ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
			cmQuickButtons.SubItems.Clear();
			textComponent = component as IStiText;
			componentContent = textComponent.Text;
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("FormFormatEditor", "TextFormat"),
				Image = StiQuickButtonHelper.TextFormatImage
			};
			buttonItem.Click += TextFormatButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuEditClearContents"),
				Image = StiQuickButtonHelper.ClearContentImage
			};
			buttonItem2.Click += ClearContentsButton_Click;
			cmQuickButtons.SubItems.Add(buttonItem2);
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
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(treeNode) + "}";
	}
}
