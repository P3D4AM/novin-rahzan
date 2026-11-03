using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.Win.Images;

namespace Stimulsoft.Report.Win.Ribbon;

[ToolboxItem(false)]
public class StiRibbonDotMatrixViewUserControl : UserControl
{
	private StiDotMatrixViewerControl DotMatrixViewerControl;

	private IContainer components;

	private RibbonBar ribbonBarFile;

	private ButtonItem biPrint;

	private ButtonItem biOpen;

	private ButtonItem biSave;

	private ButtonItem biClose;

	private void Localize()
	{
		((BaseItem)biPrint).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Print"));
		((BaseItem)biOpen).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
		((BaseItem)biSave).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
		((BaseItem)biClose).Text = StiLocalization.Get("FormViewer", "Close");
		LocalizeToolTip();
	}

	private void LocalizeToolTip()
	{
		if (DotMatrixViewerControl != null)
		{
			Graphics g = ((Control)this).CreateGraphics();
			string url = "user-manual/viewing_reports_dot_matrix_viewer_toolbar.htm";
			SetToolTip((BaseItem)(object)biPrint, g, ((BaseItem)biPrint).Text, "Print", StiScale.I(140), headerVisible: true, url);
			SetToolTip((BaseItem)(object)biOpen, g, ((BaseItem)biOpen).Text, "Open", StiScale.I(264), headerVisible: true, url);
			SetToolTip((BaseItem)(object)biSave, g, ((BaseItem)biSave).Text, "Save", StiScale.I(200), headerVisible: true, url);
			SetToolTip((BaseItem)(object)biClose, g, ((BaseItem)biClose).Text, "CloseDotMatrix", StiScale.I(200), headerVisible: true, url);
		}
	}

	private void SetToolTip(BaseItem item, Graphics g, string header, string localizeName, int width, bool headerVisible, string url)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		if (item != null)
		{
			string text = StiLocalization.Get("HelpDesigner", "PressF1");
			SizeF sizeF = DotMatrixViewerControl.superToolTip.MeasureFooterText(g, text);
			sizeF.Width += StiScale.I(40);
			string text2 = StiLocalization.Get("HelpViewer", localizeName);
			SuperTooltipInfo val = new SuperTooltipInfo(header, text, text2, (Image)null, (Image)null, DotMatrixViewerControl.CurrentTooltipColor)
			{
				HeaderVisible = headerVisible,
				Hyperlink = url,
				BodyImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png")
			};
			if (sizeF.Width > (float)width)
			{
				width = (int)sizeF.Width;
			}
			int height = (int)DotMatrixViewerControl.superToolTip.MeasureBodyText(g, text2, width - StiScale.I(28)).Height + StiScale.I(52);
			val.CustomSize = new Size(width, height);
			DotMatrixViewerControl.superToolTip.SetSuperTooltip((IComponent)item, val);
		}
	}

	private void UpdateButton()
	{
		if (DotMatrixViewerControl != null)
		{
			DotMatrixViewerControl.tbPrint = biPrint;
			DotMatrixViewerControl.tbOpen = biOpen;
			DotMatrixViewerControl.tbSave = biSave;
			DotMatrixViewerControl.tbClose = biClose;
		}
	}

	private void InitEventsButton()
	{
		((BaseItem)biPrint).Click += biPrint_Click;
		((BaseItem)biOpen).Click += biOpen_Click;
		((BaseItem)biSave).Click += biSave_Click;
		((BaseItem)biClose).Click += biClose_Click;
	}

	private void UpdateControlsState()
	{
		if (DotMatrixViewerControl != null)
		{
			StiDotMatrixViewerConfigService stiDotMatrixViewerConfigService = StiOptions.Services.DotMatrixViewerConfig.FirstOrDefault();
			if (stiDotMatrixViewerConfigService != null)
			{
				StiPreviewSettings settings = ((DotMatrixViewerControl.CurrentReport != null) ? ((StiPreviewSettings)DotMatrixViewerControl.CurrentReport.PreviewSettings) : StiPreviewSettings.Default);
				((BaseItem)biPrint).Visible = GetPrintVisibility(settings, stiDotMatrixViewerConfigService);
				((BaseItem)biSave).Visible = GetSaveVisibility(settings, stiDotMatrixViewerConfigService);
				((BaseItem)biOpen).Visible = GetOpenVisibility(settings, stiDotMatrixViewerConfigService);
				((BaseItem)biClose).Visible = GetCloseVisibility(settings, stiDotMatrixViewerConfigService);
			}
		}
	}

	private bool GetPrintVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.PrintEnabled.HasValue)
		{
			return config.PrintEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Print) > StiPreviewSettings.None)
		{
			return DotMatrixViewerControl.ShowPrint;
		}
		return false;
	}

	private bool GetSaveVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.SaveEnabled.HasValue)
		{
			return config.SaveEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Save) > StiPreviewSettings.None)
		{
			return DotMatrixViewerControl.ShowSave;
		}
		return false;
	}

	private bool GetOpenVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.OpenEnabled.HasValue)
		{
			return config.OpenEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Open) > StiPreviewSettings.None)
		{
			return DotMatrixViewerControl.ShowOpen;
		}
		return false;
	}

	private bool GetCloseVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.CloseEnabled.HasValue)
		{
			return config.CloseEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Close) <= StiPreviewSettings.None)
		{
			return DotMatrixViewerControl.ShowCloseButton;
		}
		return true;
	}

	private void DoScaling()
	{
		biSave.Image = StiReportViewerImages.Actions.ReportSave((StiImageSize)2);
		biPrint.Image = StiReportViewerImages.Actions.ReportPrint((StiImageSize)2);
		biOpen.Image = StiReportViewerImages.Actions.ReportOpen((StiImageSize)2);
		if (!StiScaleUI.IsNoScaling)
		{
			biClose.Image = StiReportViewerImages.Actions.Close((StiImageSize)2);
		}
	}

	private void biClose_Click(object sender, EventArgs e)
	{
		if (DotMatrixViewerControl != null)
		{
			DotMatrixViewerControl.InvokeClose();
		}
	}

	private void biSave_Click(object sender, EventArgs e)
	{
		if (DotMatrixViewerControl != null)
		{
			DotMatrixViewerControl.InvokeClickSaveButton();
		}
	}

	private void biOpen_Click(object sender, EventArgs e)
	{
		if (DotMatrixViewerControl != null)
		{
			DotMatrixViewerControl.InvokeClickOpenButton();
		}
	}

	private void biPrint_Click(object sender, EventArgs e)
	{
		if (DotMatrixViewerControl != null)
		{
			DotMatrixViewerControl.InvokeClickPrintButton();
		}
	}

	public StiRibbonDotMatrixViewUserControl(StiDotMatrixViewerControl dotMatrixViewerControl)
	{
		InitializeComponent();
		DoScaling();
		if (!((Component)this).DesignMode)
		{
			DotMatrixViewerControl = dotMatrixViewerControl;
			Localize();
			InitEventsButton();
			UpdateControlsState();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDotMatrixViewUserControl));
		ribbonBarFile = new RibbonBar();
		biPrint = new ButtonItem();
		biOpen = new ButtonItem();
		biSave = new ButtonItem();
		biClose = new ButtonItem();
		((Control)this).SuspendLayout();
		ribbonBarFile.AutoOverflowEnabled = true;
		ribbonBarFile.Items.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)biPrint,
			(BaseItem)biOpen,
			(BaseItem)biSave,
			(BaseItem)biClose
		});
		((Control)ribbonBarFile).Location = new Point(3, 3);
		((ItemControl)ribbonBarFile).MenuFocus = false;
		((Control)ribbonBarFile).Name = "ribbonBarFile";
		((Control)ribbonBarFile).Size = new Size(226, 93);
		ribbonBarFile.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarFile).TabIndex = 0;
		((Control)ribbonBarFile).Text = "File";
		biPrint.Image = (Image)componentResourceManager.GetObject("biPrint.Image");
		biPrint.ImageListSizeSelection = (eButtonImageListSelection)(-1);
		biPrint.ImagePaddingHorizontal = 8;
		biPrint.ImagePosition = (eImagePosition)2;
		((BaseItem)biPrint).Name = "biPrint";
		biPrint.SubItemsExpandWidth = 14;
		((BaseItem)biPrint).Text = "Print";
		((BaseItem)biOpen).BeginGroup = true;
		biOpen.Image = (Image)componentResourceManager.GetObject("biOpen.Image");
		biOpen.ImageListSizeSelection = (eButtonImageListSelection)(-1);
		biOpen.ImagePaddingHorizontal = 8;
		biOpen.ImagePosition = (eImagePosition)2;
		((BaseItem)biOpen).Name = "biOpen";
		biOpen.SubItemsExpandWidth = 14;
		((BaseItem)biOpen).Text = "Open";
		biSave.Image = (Image)componentResourceManager.GetObject("biSave.Image");
		biSave.ImageListSizeSelection = (eButtonImageListSelection)(-1);
		biSave.ImagePaddingHorizontal = 8;
		biSave.ImagePosition = (eImagePosition)2;
		((BaseItem)biSave).Name = "biSave";
		biSave.SubItemsExpandWidth = 14;
		((BaseItem)biSave).Text = "Save";
		((BaseItem)biClose).BeginGroup = true;
		biClose.Image = (Image)componentResourceManager.GetObject("biClose.Image");
		biClose.ImageListSizeSelection = (eButtonImageListSelection)(-1);
		biClose.ImagePaddingHorizontal = 8;
		biClose.ImagePosition = (eImagePosition)2;
		((BaseItem)biClose).Name = "biClose";
		biClose.SubItemsExpandWidth = 14;
		((BaseItem)biClose).Text = "Close";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Gainsboro;
		((Control)this).Controls.Add((Control)(object)ribbonBarFile);
		((Control)this).Name = "StiRibbonDotMatrixViewUserControl";
		((Control)this).Size = new Size(968, 336);
		((Control)this).ResumeLayout(false);
	}
}
