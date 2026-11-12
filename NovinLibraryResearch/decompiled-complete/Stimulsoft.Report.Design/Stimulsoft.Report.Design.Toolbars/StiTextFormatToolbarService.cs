using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiTextFormatToolbarService : StiToolbarService
{
	private ButtonItem tbTextFormat;

	public override string ServiceName => StiLocalization.Get("PropertyMain", "TextFormat");

	public override int Position => 6;

	public override int DefaultDockLine => 3;

	public override bool DefaultVisible => false;

	private void RefreshDesigner(object sender, EventArgs e)
	{
		if (!base.ToolBar.Visible)
		{
			return;
		}
		StiFormatService stiFormatService = ((Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null);
		if (stiFormatService != null && !CheckPropertyEnabled(Report, "TextFormat"))
		{
			stiFormatService = null;
		}
		if (stiFormatService != null)
		{
			if (!tbTextFormat.Enabled)
			{
				tbTextFormat.Enabled = true;
			}
			{
				foreach (ButtonItem textFormatButton in Designer.TextFormatButtons)
				{
					textFormatButton.Enabled = true;
					if (textFormatButton.Tag.GetType() == stiFormatService.GetType())
					{
						textFormatButton.Checked = true;
					}
					else
					{
						textFormatButton.Checked = false;
					}
				}
				return;
			}
		}
		if (tbTextFormat.Enabled)
		{
			tbTextFormat.Enabled = false;
		}
		foreach (ButtonItem textFormatButton2 in Designer.TextFormatButtons)
		{
			if (textFormatButton2.Enabled)
			{
				textFormatButton2.Enabled = false;
			}
		}
	}

	private void tbTextFormat_Click(object sender, EventArgs e)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		try
		{
			Designer.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "TextFormat"), onlyCurrentPage: true);
			StiComponentsCollection selected = Designer.GetSelected();
			StiComponent stiComponent = null;
			foreach (StiComponent item in selected)
			{
				if (item is IStiTextFormat)
				{
					stiComponent = item;
					break;
				}
			}
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiComponent as IStiTextFormat, Designer);
			try
			{
				Designer?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				if ((int)((Form)stiTextFormatEditorForm).ShowDialog() == 1)
				{
					foreach (StiComponent item2 in selected)
					{
						if (item2 is IStiTextFormat)
						{
							((IStiTextFormat)item2).TextFormat = (StiFormatService)((IStiTextFormat)stiComponent).TextFormat.Clone();
						}
					}
					((Control)Designer).Refresh();
					Designer.InvokeSelectedObjectsChanged();
				}
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
			Designer.Builder.Redraw();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void tbFormatService_Click(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			StiFormatService stiFormatService = buttonItem.Tag as StiFormatService;
			Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "TextFormat"), onlyCurrentPage: true);
			Designer.Report.GetCurrentPage().SetTextFormat((StiFormatService)((ICloneable)stiFormatService).Clone());
			((Control)Designer).Refresh();
			Designer.InvokeSelectedObjectsChanged();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void LocalizeSuperToolTip()
	{
		if (Designer == null)
		{
			return;
		}
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (tbTextFormat != null)
			{
				SetSuperToolTip(tbTextFormat, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "TextFormat")), "TextFormat", globalMeasureGraphics);
			}
			ButtonItem item = Designer.TextFormatButtons[0];
			SetSuperToolTip(item, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "General")), "FormatGeneral", globalMeasureGraphics);
			ButtonItem item2 = Designer.TextFormatButtons[1];
			SetSuperToolTip(item2, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Number")), "FormatNumber", globalMeasureGraphics);
			ButtonItem item3 = Designer.TextFormatButtons[2];
			SetSuperToolTip(item3, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Currency")), "FormatCurrency", globalMeasureGraphics);
			ButtonItem item4 = Designer.TextFormatButtons[3];
			SetSuperToolTip(item4, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Date")), "FormatDate", globalMeasureGraphics);
			ButtonItem item5 = Designer.TextFormatButtons[4];
			SetSuperToolTip(item5, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Time")), "FormatTime", globalMeasureGraphics);
			ButtonItem item6 = Designer.TextFormatButtons[5];
			SetSuperToolTip(item6, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Percentage")), "FormatPercentage", globalMeasureGraphics);
			ButtonItem item7 = Designer.TextFormatButtons[6];
			SetSuperToolTip(item7, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Boolean")), "FormatBoolean", globalMeasureGraphics);
			ButtonItem item8 = Designer.TextFormatButtons[7];
			SetSuperToolTip(item8, StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Custom")), "FormatCustom", globalMeasureGraphics);
		}
		else
		{
			if (tbTextFormat != null)
			{
				tbTextFormat.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "TextFormat"));
			}
			Designer.TextFormatButtons[0].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "General"));
			Designer.TextFormatButtons[1].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Number"));
			Designer.TextFormatButtons[2].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Currency"));
			Designer.TextFormatButtons[3].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Date"));
			Designer.TextFormatButtons[4].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Time"));
			Designer.TextFormatButtons[5].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Percentage"));
			Designer.TextFormatButtons[6].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Boolean"));
			Designer.TextFormatButtons[7].Tooltip = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "Custom"));
		}
	}

	public void InvokeTextFormat()
	{
		tbTextFormat_Click(null, EventArgs.Empty);
	}

	public static StiTextFormatToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiTextFormatToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiTextFormatToolbarService)) as StiTextFormatToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiTextFormatToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		LocalizeSuperToolTip();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("FormFormatEditor", "TextFormat");
		}
		if (tbTextFormat != null)
		{
			tbTextFormat.Text = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "TextFormat"));
		}
		if (Designer.TextFormatButtons == null)
		{
			return;
		}
		foreach (ButtonItem textFormatButton in Designer.TextFormatButtons)
		{
			textFormatButton.Text = StiHint.CreateHint(((StiFormatService)textFormatButton.Tag).ServiceName);
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		((Control)base.ToolBar).Text = StiLocalization.Get("FormFormatEditor", "TextFormat");
		Add(ref tbTextFormat, StiReportDesignImages.Actions.TextFormat(), StiLocalization.Get("FormFormatEditor", "TextFormat"), tbTextFormat_Click);
		tbTextFormat.Name = "tbTextFormat";
		AddToolButtonSeparator();
		designer.TextFormatButtons.Clear();
		foreach (StiFormatService item in StiOptions.Services.Formats.Where((StiFormatService s) => s.ServiceEnabled))
		{
			ButtonItem button = null;
			Add(ref button, StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(item.GetType())), item.ServiceName, tbFormatService_Click);
			button.Name = item.GetType().ToString();
			button.Tag = item;
			designer.TextFormatButtons.Add(button);
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		LocalizeSuperToolTip();
		designer.RefreshDesigner += RefreshDesigner;
	}

	public override void Dispose()
	{
		foreach (ButtonItem textFormatButton in Designer.TextFormatButtons)
		{
			textFormatButton.Tag = null;
		}
		Designer.TextFormatButtons.Clear();
		base.Dispose();
		tbTextFormat = null;
	}
}
