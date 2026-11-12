using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiTableCellTextQuickButton : StiQuickButton
{
	private StiTableCell textComponent;

	private string componentContent;

	private IStiDesignerBase designer;

	public override Image Image => StiReportDesignImages.QuickButtons.Field();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Field_Selected();

	public override string Name => StiLocalization.Get("Components", "StiText");

	private void OnSetField(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			IStiDesignerBase stiDesignerBase = textComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
			StiTableCell stiTableCell = textComponent;
			object tag = buttonItem.Tag;
			stiTableCell.Text = GetColumnPathFromNode((TreeNode)((tag is TreeNode) ? tag : null));
			textComponent.Type = StiSystemTextType.DataColumn;
			stiDesignerBase.DesignerControl.TextAliasesHash.Clear();
			stiDesignerBase.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void TextFormatButton_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IStiDesignerBase stiDesignerBase = textComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "TextFormat"), onlyCurrentPage: true);
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(textComponent, stiDesignerBase?.DesignerControl as StiDesignerControl);
			try
			{
				stiDesignerBase?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				((Form)stiTextFormatEditorForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
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
			IStiDesignerBase stiDesignerBase = textComponent.Report.Designer;
			stiDesignerBase?.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditClearContents"), onlyCurrentPage: true);
			textComponent.Text = string.Empty;
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
		try
		{
			this.designer = designer;
			ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
			cmQuickButtons.SubItems.Clear();
			textComponent = component as StiTableCell;
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
			if (!designer.Report.IsPageDesigner)
			{
				ButtonItem buttonItem3 = new ButtonItem
				{
					BeginGroup = true,
					Text = Loc.Get("MainMenu", "menuConvertToImage"),
					Tag = StiTablceCellType.Image,
					Image = StiReportDesignImages.Components.Image()
				};
				buttonItem3.Click += ConvertToButton_Click;
				cmQuickButtons.SubItems.Add(buttonItem3);
				ButtonItem buttonItem4 = new ButtonItem
				{
					BeginGroup = false,
					Text = Loc.Get("MainMenu", "menuConvertToCheckBox"),
					Tag = StiTablceCellType.CheckBox,
					Image = StiReportDesignImages.Components.CheckBox()
				};
				buttonItem4.Click += ConvertToButton_Click;
				cmQuickButtons.SubItems.Add(buttonItem4);
				ButtonItem buttonItem5 = new ButtonItem
				{
					BeginGroup = false,
					Text = Loc.Get("MainMenu", "MenuConvertToRichText"),
					Tag = StiTablceCellType.RichText,
					Image = StiReportDesignImages.Components.RichText()
				};
				buttonItem5.Click += ConvertToButton_Click;
				cmQuickButtons.SubItems.Add(buttonItem5);
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
		textComponent.CellType = (StiTablceCellType)((ButtonItem)sender).Tag;
		designer.Refresh();
	}

	private string GetColumnPathFromNode(TreeNode treeNode)
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(treeNode) + "}";
	}
}
