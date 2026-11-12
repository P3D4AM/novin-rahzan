using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiWatermarkEditorForm : StiInplaceEditorForm
{
	private StiAdvancedWatermark watermark;

	private IStiDashboardWatermark dashboard;

	private StiAdvancedWatermark clonedWatermark;

	private IStiDesignerBase designer;

	private StiPopup majorIconsPopup;

	private StiPopup minorIconsPopup;

	private Panel panelMajorIcons;

	private Panel panelMinorIcons;

	private StiHintTextBox textBoxMajorSearch;

	private StiHintTextBox textBoxMinorSearch;

	private IContainer components;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiButtonPanelUX panelFooter;

	private StiLabelUX labelTextColor;

	private StiLabelUX labelTextAngle;

	private StiLabelUX labelText;

	private StiCheckBoxUX checkBoxImageAspectRatio;

	private StiCheckBoxUX checkBoxImageTiling;

	private StiCheckBoxUX checkBoxImageStretch;

	private StiLabelUX labelImageTransparency;

	private StiLabelUX labelImageMultipleFactor;

	private StiLabelUX labelImageAlignment;

	private StiButtonUX buttonImageRemove;

	private StiButtonUX buttonImageOpen;

	private StiPanelUX panelImage;

	private ToolTip toolTip1;

	private StiTrackBarUX trackBarImageTransparency;

	public StiColorBoxXF colorBoxTextColor;

	public StiButtonEdit textBoxText;

	private StiLabelUX labelTextFont;

	public StiButtonEdit textBoxFont;

	private StiTrackBarUX trackBarImageSize;

	private StiPanelUX panelVerAlignment;

	private StiOperationRadioButton radioButtonImageVertAlignTop;

	private StiOperationRadioButton radioButtonImageVertAlignCenter;

	private StiOperationRadioButton radioButtonImageVertAlignBottom;

	private StiPanelUX panelHorAlignment;

	private StiOperationRadioButton radioButtonImageHorAlignLeft;

	private StiOperationRadioButton radioButtonImageHorAlignCenter;

	private StiOperationRadioButton radioButtonImageHorAlignRight;

	private StiColorBoxXF colorBoxWeaveMajorColor;

	private StiPanelUX panelBoxWeaveMajorIcon;

	private StiOperationButton buttonMajorIconRemove;

	private StiOperationButton buttonMajorIcon;

	private StiLabelUX labelWeaveAngle;

	private StiTrackBarUX trackBarWeaveDistance;

	private StiLabelUX labelWeaveDistance;

	private StiLabelUX labelWeaveMajorIcon;

	private StiOffice2013GroupBox groupBoxWeaveMajor;

	private StiLabelUX labelWeaveMajorSize;

	private StiTrackBarUX trackBarWeaveMajorSize;

	private StiOffice2013GroupBox groupBoxWeaveMinor;

	private StiLabelUX labelWeaveMinorSize;

	private StiTrackBarUX trackBarWeaveMinorSize;

	private StiPanelUX panelBoxWeaveMinorIcon;

	private StiOperationButton buttonMinorIconRemove;

	private StiOperationButton buttonMinorIcon;

	private StiLabelUX labelWeaveMinorIcon;

	private StiColorBoxXF colorBoxWeaveMinorColor;

	private StiTrackBarUX trackBarWeaveAngle;

	private StiGroupPanelUX stiGroupLine2;

	private StiTrackBarUX trackBarTextAngle;

	private StiCheckBoxUX checkBoxImageEnabled;

	private StiCheckBoxUX checkBoxWeaveEnabled;

	private StiCheckBoxUX checkBoxTextEnabled;

	private StiLabelUX labelImageSizeHint;

	private StiLabelUX labelImageTransparencyHint;

	private StiLabelUX labelTextAngleHint;

	private StiLabelUX labelWeaveAngleHint;

	private StiLabelUX labelWeaveDistanceHint;

	private StiLabelUX labelWeaveMinorSizeHint;

	private StiLabelUX labelWeaveMajorSizeHint;

	private Timer timerMarker;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageText;

	private StiCloudTabulatorPage pageImage;

	private StiCloudTabulatorPage pageWeave;

	public override bool AllowResize { get; }

	private StiDesignerControl Designer => designer as StiDesignerControl;

	public StiWatermarkEditorForm(StiAdvancedWatermark watermark, IStiDashboardWatermark watermarkComponent)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			designer = (watermarkComponent.GetApp() as StiReport).Designer;
			this.watermark = watermark;
			dashboard = watermarkComponent;
			clonedWatermark = watermark.Clone() as StiAdvancedWatermark;
			Localize();
			DoScaling();
			LoadWatermarkOptions();
			TabControl.SelectedIndex = 0;
		}
	}

	public override void Localize()
	{
		((Control)this).Text = Loc.GetMain("Watermark");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelText).Text = Loc.GetMain("Text");
		((Control)labelTextAngle).Text = Loc.GetMain("Angle");
		((Control)labelTextColor).Text = Loc.GetMain("TextColor");
		((Control)labelTextFont).Text = Loc.GetMain("Font");
		((Control)labelImageAlignment).Text = Loc.GetMain("ImageAlignment");
		((Control)labelImageMultipleFactor).Text = Loc.GetMain("MultipleFactor");
		((Control)labelImageTransparency).Text = Loc.GetMain("ImageTransparency");
		((Control)labelWeaveMajorIcon).Text = Loc.GetMain("Icon");
		((Control)labelWeaveMinorIcon).Text = Loc.GetMain("Icon");
		((Control)labelWeaveMajorSize).Text = Loc.GetMain("Size");
		((Control)labelWeaveMinorSize).Text = Loc.GetMain("Size");
		((Control)labelWeaveDistance).Text = Loc.GetMain("Distance");
		((Control)labelWeaveAngle).Text = Loc.GetMain("Angle");
		((Control)groupBoxWeaveMajor).Text = Loc.Get("ChartRibbon", "AxesTicksMajor");
		((Control)groupBoxWeaveMinor).Text = Loc.Get("ChartRibbon", "AxesTicksMinor");
		((Control)checkBoxTextEnabled).Text = Loc.GetMain("Enabled");
		((Control)checkBoxImageEnabled).Text = Loc.GetMain("Enabled");
		((Control)checkBoxWeaveEnabled).Text = Loc.GetMain("Enabled");
		((Control)checkBoxImageAspectRatio).Text = Loc.GetMain("AspectRatio");
		((Control)checkBoxImageStretch).Text = Loc.GetMain("ImageStretch");
		((Control)checkBoxImageTiling).Text = Loc.GetMain("ImageTiling");
		toolTip1.SetToolTip((Control)(object)radioButtonImageHorAlignLeft, StiHint.CreateHint(Loc.GetEnum("StiHorAlignmentLeft")));
		toolTip1.SetToolTip((Control)(object)radioButtonImageHorAlignCenter, StiHint.CreateHint(Loc.GetEnum("StiHorAlignmentCenter")));
		toolTip1.SetToolTip((Control)(object)radioButtonImageHorAlignRight, StiHint.CreateHint(Loc.GetEnum("StiHorAlignmentRight")));
		toolTip1.SetToolTip((Control)(object)radioButtonImageVertAlignTop, StiHint.CreateHint(Loc.GetEnum("StiVertAlignmentTop")));
		toolTip1.SetToolTip((Control)(object)radioButtonImageVertAlignCenter, StiHint.CreateHint(Loc.GetEnum("StiVertAlignmentCenter")));
		toolTip1.SetToolTip((Control)(object)radioButtonImageVertAlignBottom, StiHint.CreateHint(Loc.GetEnum("StiVertAlignmentBottom")));
		((Control)pageText).Text = Loc.GetMain("Text");
		((Control)pageImage).Text = Loc.GetMain("Image");
		((Control)pageWeave).Text = Loc.Get("PropertyHatchStyle", "Weave");
	}

	private void DoScaling()
	{
		textBoxFont.Image = StiReportDesignImages.Editor.Font();
		textBoxText.Image = StiReportDesignImages.Actions.EditSmall();
		((ButtonBase)buttonImageOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonImageRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)radioButtonImageHorAlignLeft).Image = StiReportDesignImages.Editor.AlignLeft();
		((ButtonBase)radioButtonImageHorAlignCenter).Image = StiReportDesignImages.Editor.AlignCenter();
		((ButtonBase)radioButtonImageHorAlignRight).Image = StiReportDesignImages.Editor.AlignRight();
		((ButtonBase)radioButtonImageVertAlignTop).Image = StiReportDesignImages.Editor.AlignTop();
		((ButtonBase)radioButtonImageVertAlignCenter).Image = StiReportDesignImages.Editor.AlignMiddle();
		((ButtonBase)radioButtonImageVertAlignBottom).Image = StiReportDesignImages.Editor.AlignBottom();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxWeaveMajorIcon);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxWeaveMinorIcon);
		TabControl.TitleWidth = StiScale.I(150);
	}

	private void LoadWatermarkOptions()
	{
		base.LockRefresh = true;
		LoadWatermarkTextOptions();
		LoadWatermarkImageOptions();
		LoadWatermarkWeaveOptions();
		base.LockRefresh = false;
		CheckTextStates();
		CheckImageStates();
		CalculateWindowHeight();
	}

	private void LoadWatermarkTextOptions()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		int i;
		for (i = watermark.TextAngle; i > 359; i -= 360)
		{
		}
		for (; i < -359; i += 360)
		{
		}
		((CheckBox)checkBoxTextEnabled).Checked = watermark.TextEnabled;
		((TrackBar)trackBarTextAngle).Value = i;
		((Control)textBoxText).Text = watermark.Text;
		((Control)textBoxFont).Text = ((TypeConverter)new FontConverter()).ConvertToString((object?)watermark.TextFont);
		colorBoxTextColor.SelectedColor = watermark.TextColor;
	}

	private void LoadWatermarkImageOptions()
	{
		((CheckBox)checkBoxImageEnabled).Checked = watermark.ImageEnabled;
		((CheckBox)checkBoxImageAspectRatio).Checked = watermark.ImageAspectRatio;
		((CheckBox)checkBoxImageStretch).Checked = watermark.ImageStretch;
		((CheckBox)checkBoxImageTiling).Checked = watermark.ImageTiling;
		((TrackBar)trackBarImageSize).Value = (int)(watermark.ImageMultipleFactor * 10.0);
		((TrackBar)trackBarImageTransparency).Value = watermark.ImageTransparency;
		CheckAlignmentState();
	}

	private void LoadWatermarkWeaveOptions()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		((CheckBox)checkBoxWeaveEnabled).Checked = watermark.WeaveEnabled;
		majorIconsPopup = new StiPopup((Control)(object)GetMajorIconPanel());
		minorIconsPopup = new StiPopup((Control)(object)GetMinorIconPanel());
		((Control)panelBoxWeaveMajorIcon).Paint += (PaintEventHandler)delegate
		{
			DrawBoxMajorIcon();
		};
		((Control)panelBoxWeaveMinorIcon).Paint += (PaintEventHandler)delegate
		{
			DrawBoxMinorIcon();
		};
		colorBoxWeaveMajorColor.SelectedColor = watermark.WeaveMajorColor;
		colorBoxWeaveMinorColor.SelectedColor = watermark.WeaveMinorColor;
		((TrackBar)trackBarWeaveMajorSize).Value = watermark.WeaveMajorSize;
		((TrackBar)trackBarWeaveMinorSize).Value = watermark.WeaveMinorSize;
		((TrackBar)trackBarWeaveAngle).Value = watermark.WeaveAngle;
		((TrackBar)trackBarWeaveDistance).Value = watermark.WeaveDistance;
		CheckAlignmentState();
		CheckIconsState();
	}

	private void CheckIconsState()
	{
		StiOperationButton stiOperationButton = buttonMajorIconRemove;
		StiColorBoxXF stiColorBoxXF = colorBoxWeaveMajorColor;
		bool flag2 = (((Control)trackBarWeaveMajorSize).Enabled = watermark.WeaveMajorIcon.HasValue && ((CheckBox)checkBoxWeaveEnabled).Checked);
		bool visible = (((Control)stiColorBoxXF).Enabled = flag2);
		((Control)stiOperationButton).Visible = visible;
		StiOperationButton stiOperationButton2 = buttonMinorIconRemove;
		StiColorBoxXF stiColorBoxXF2 = colorBoxWeaveMinorColor;
		flag2 = (((Control)trackBarWeaveMinorSize).Enabled = watermark.WeaveMinorIcon.HasValue && ((CheckBox)checkBoxWeaveEnabled).Checked);
		visible = (((Control)stiColorBoxXF2).Enabled = flag2);
		((Control)stiOperationButton2).Visible = visible;
		StiTrackBarUX stiTrackBarUX = trackBarWeaveDistance;
		visible = (((Control)trackBarWeaveAngle).Enabled = (watermark.WeaveMajorIcon.HasValue || watermark.WeaveMinorIcon.HasValue) && ((CheckBox)checkBoxWeaveEnabled).Checked);
		((Control)stiTrackBarUX).Enabled = visible;
		StiOperationButton stiOperationButton3 = buttonMajorIcon;
		visible = (((Control)buttonMinorIcon).Enabled = ((CheckBox)checkBoxWeaveEnabled).Checked);
		((Control)stiOperationButton3).Enabled = visible;
	}

	private void CheckAlignmentState()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Invalid comparison between Unknown and I4
		ContentAlignment val = watermark.ImageAlignment;
		if (watermark.ImageBytes == null)
		{
			val = (ContentAlignment)0;
		}
		((RadioButton)radioButtonImageHorAlignLeft).Checked = (int)val == 1 || (int)val == 16 || (int)val == 256;
		((RadioButton)radioButtonImageHorAlignCenter).Checked = (int)val == 2 || (int)val == 32 || (int)val == 512;
		((RadioButton)radioButtonImageHorAlignRight).Checked = (int)val == 4 || (int)val == 64 || (int)val == 1024;
		((RadioButton)radioButtonImageVertAlignTop).Checked = (int)val == 1 || (int)val == 2 || (int)val == 4;
		((RadioButton)radioButtonImageVertAlignCenter).Checked = (int)val == 16 || (int)val == 32 || (int)val == 64;
		((RadioButton)radioButtonImageVertAlignBottom).Checked = (int)val == 256 || (int)val == 512 || (int)val == 1024;
	}

	public void RefreshElementView()
	{
		StiDesignerControl stiDesignerControl = Designer;
		if (stiDesignerControl != null)
		{
			((Control)stiDesignerControl).Refresh();
		}
		Designer?.Builder.Redraw();
	}

	internal void CheckImageStates()
	{
		StiButtonUX stiButtonUX = buttonImageRemove;
		StiCheckBoxUX stiCheckBoxUX = checkBoxImageAspectRatio;
		StiCheckBoxUX stiCheckBoxUX2 = checkBoxImageStretch;
		StiCheckBoxUX stiCheckBoxUX3 = checkBoxImageTiling;
		StiTrackBarUX stiTrackBarUX = trackBarImageSize;
		StiTrackBarUX stiTrackBarUX2 = trackBarImageTransparency;
		StiPanelUX stiPanelUX = panelHorAlignment;
		StiPanelUX stiPanelUX2 = panelVerAlignment;
		StiLabelUX stiLabelUX = labelImageAlignment;
		StiLabelUX stiLabelUX2 = labelImageMultipleFactor;
		bool flag2 = (((Control)labelImageTransparency).Enabled = watermark.ImageBytes != null && ((CheckBox)checkBoxImageEnabled).Checked);
		bool flag4 = (((Control)stiLabelUX2).Enabled = flag2);
		bool flag6 = (((Control)stiLabelUX).Enabled = flag4);
		bool flag8 = (((Control)stiPanelUX2).Enabled = flag6);
		bool flag10 = (((Control)stiPanelUX).Enabled = flag8);
		bool flag12 = (((Control)stiTrackBarUX2).Enabled = flag10);
		bool flag14 = (((Control)stiTrackBarUX).Enabled = flag12);
		bool flag16 = (((Control)stiCheckBoxUX3).Enabled = flag14);
		bool flag18 = (((Control)stiCheckBoxUX2).Enabled = flag16);
		bool enabled = (((Control)stiCheckBoxUX).Enabled = flag18);
		((Control)stiButtonUX).Enabled = enabled;
		((Control)buttonImageOpen).Enabled = ((CheckBox)checkBoxImageEnabled).Checked;
		CheckAlignmentState();
	}

	private void CheckTextStates()
	{
		StiLabelUX stiLabelUX = labelText;
		StiLabelUX stiLabelUX2 = labelTextAngle;
		StiLabelUX stiLabelUX3 = labelTextColor;
		StiLabelUX stiLabelUX4 = labelTextFont;
		StiTrackBarUX stiTrackBarUX = trackBarTextAngle;
		StiColorBoxXF stiColorBoxXF = colorBoxTextColor;
		bool flag2 = (((Control)textBoxFont).Enabled = !string.IsNullOrEmpty(((Control)textBoxText).Text) && ((CheckBox)checkBoxTextEnabled).Checked);
		bool flag4 = (((Control)stiColorBoxXF).Enabled = flag2);
		bool flag6 = (((Control)stiTrackBarUX).Enabled = flag4);
		bool flag8 = (((Control)stiLabelUX4).Enabled = flag6);
		bool flag10 = (((Control)stiLabelUX3).Enabled = flag8);
		bool enabled = (((Control)stiLabelUX2).Enabled = flag10);
		((Control)stiLabelUX).Enabled = enabled;
		((Control)textBoxText).Enabled = ((CheckBox)checkBoxTextEnabled).Checked;
	}

	private void DrawBoxMajorIcon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Graphics val = ((Control)panelBoxWeaveMajorIcon).CreateGraphics();
		val.TextRenderingHint = (TextRenderingHint)3;
		StiDrawing.FillRectangle(val, StiUX.Background, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxWeaveMajorIcon).Width - StiScale.I4, ((Control)panelBoxWeaveMajorIcon).Height - StiScale.I6));
		if (watermark.WeaveMajorIcon.HasValue)
		{
			SolidBrush val2 = new SolidBrush(watermark.WeaveMajorColor);
			try
			{
				Font val3 = new Font(fontFamilyIcons, 13f);
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val.DrawString(StiFontIconsHelper.GetContent(watermark.WeaveMajorIcon.GetValueOrDefault()), val3, (Brush)(object)val2, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxWeaveMajorIcon).Width, ((Control)panelBoxWeaveMajorIcon).Height), val4);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		StiDrawing.DrawRectangle(val, StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark, 0.0, 0.0, ((Control)panelBoxWeaveMajorIcon).Width - 1, ((Control)panelBoxWeaveMajorIcon).Height - 1);
	}

	private void DrawBoxMinorIcon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Graphics val = ((Control)panelBoxWeaveMinorIcon).CreateGraphics();
		val.TextRenderingHint = (TextRenderingHint)3;
		StiDrawing.FillRectangle(val, StiUX.Background, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxWeaveMinorIcon).Width - StiScale.I4, ((Control)panelBoxWeaveMinorIcon).Height - StiScale.I6));
		if (watermark.WeaveMinorIcon.HasValue)
		{
			SolidBrush val2 = new SolidBrush(watermark.WeaveMinorColor);
			try
			{
				Font val3 = new Font(fontFamilyIcons, 13f);
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val.DrawString(StiFontIconsHelper.GetContent(watermark.WeaveMinorIcon.GetValueOrDefault()), val3, (Brush)(object)val2, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxWeaveMinorIcon).Width, ((Control)panelBoxWeaveMinorIcon).Height), val4);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		StiDrawing.DrawRectangle(val, StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark, 0.0, 0.0, ((Control)panelBoxWeaveMinorIcon).Width - 1, ((Control)panelBoxWeaveMinorIcon).Height - 1);
	}

	private Panel GetMajorIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxMajorSearch = stiHintTextBox;
		((Control)textBoxMajorSearch).TextChanged += TextBoxMajorSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxMajorSearch);
		panelMajorIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelMajorIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildMajorIconPanel();
		return val;
	}

	private Panel GetMinorIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxMinorSearch = stiHintTextBox;
		((Control)textBoxMinorSearch).TextChanged += MinorSearchTextBox_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxMinorSearch);
		panelMinorIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelMinorIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildMinorIconPanel();
		return val;
	}

	private void BuildMajorIconPanel(string filterName = null)
	{
		((Control)panelMajorIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(watermark.WeaveMajorIcon, watermark.WeaveMajorColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelMajorIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontMajorIconSelectionControl_ValueChanged;
		((Control)panelMajorIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void BuildMinorIconPanel(string filterName = null)
	{
		((Control)panelMinorIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(watermark.WeaveMinorIcon, watermark.WeaveMinorColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelMinorIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontMinorIconSelectionControl_ValueChanged;
		((Control)panelMinorIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void CheckColorMajorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)majorIconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = watermark.WeaveMajorColor;
			}
		}
	}

	private void CheckColorMinorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)minorIconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = watermark.WeaveMinorColor;
			}
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		StiAdvancedWatermark dashboardWatermark = dashboard.DashboardWatermark;
		dashboard.DashboardWatermark = clonedWatermark;
		designer.UndoRedoSave(Loc.GetMain("Watermark"));
		dashboard.DashboardWatermark = dashboardWatermark;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		dashboard.DashboardWatermark = clonedWatermark;
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_watermarks_watermark_property.htm");
	}

	private void This_Activated(object sender, EventArgs e)
	{
		((Control)textBoxText).Focus();
		textBoxText.Select(0, 0);
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		timerMarker.Stop();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		StiDesignerControl obj = designer as StiDesignerControl;
		if (obj != null)
		{
			Form obj2 = ((Control)obj).FindForm();
			if (obj2 != null)
			{
				obj2.Activate();
			}
		}
	}

	private void ButtonSelectFont_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(watermark.TextFont, designer.Report);
		try
		{
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				watermark.TextFont = stiFontEditorForm.SelectedFont;
				RefreshElementView();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void TextBoxText_TextChanged(object sender, EventArgs e)
	{
		CheckTextStates();
		if (!base.LockRefresh)
		{
			watermark.Text = ((Control)textBoxText).Text;
			RefreshElementView();
		}
	}

	private void CheckBoxWatermarkImageStretch_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.ImageStretch = ((CheckBox)checkBoxImageStretch).Checked;
			RefreshElementView();
			if (((CheckBox)checkBoxImageStretch).Checked)
			{
				((CheckBox)checkBoxImageTiling).Checked = false;
			}
		}
	}

	private void CheckBoxWatermarkImageTiling_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.ImageTiling = ((CheckBox)checkBoxImageTiling).Checked;
			RefreshElementView();
			if (((CheckBox)checkBoxImageTiling).Checked)
			{
				((CheckBox)checkBoxImageStretch).Checked = false;
				((CheckBox)checkBoxImageAspectRatio).Checked = false;
			}
		}
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonImageOpen_Click(sender, e);
	}

	private void ButtonImageOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					watermark.ImageBytes = StiImageHelper.BytesFromFile(((FileDialog)val).FileName);
					RefreshElementView();
					CheckImageStates();
					((Control)panelImage).Invalidate();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonImageRemove_Click(object sender, EventArgs e)
	{
		watermark.ImageBytes = null;
		((Control)panelImage).Invalidate();
		RefreshElementView();
		CheckImageStates();
	}

	private void ButtonEditText_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxText).Text;
		text = StiExpressionPacker.PackExpression(text, designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(designer.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxText).Text = StiExpressionPacker.UnPackExpression(text, designer, useBraces: false);
		}
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, watermark.ImageBytes, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void TrackBarImageTransparency_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelImageTransparencyHint).Text = ((TrackBar)trackBarImageTransparency).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.ImageTransparency = ((TrackBar)trackBarImageTransparency).Value;
			RefreshElementView();
		}
	}

	private void ColorBoxTextColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.TextColor = colorBoxTextColor.SelectedColor;
			RefreshElementView();
		}
	}

	private void TextBoxFont_ButtonClick(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(watermark.TextFont, designer.Report);
		try
		{
			designer?.Report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)this);
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				watermark.TextFont = stiFontEditorForm.SelectedFont;
				((Control)textBoxFont).Text = ((TypeConverter)new FontConverter()).ConvertToString((object?)stiFontEditorForm.SelectedFont);
				RefreshElementView();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void TextBoxText_ButtonClick(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxText).Text;
		DialogResult val = StiTextEditor.EditText(designer.Report, ref text, StiCodeEditorMode.EditorText);
		if ((int)val == 1)
		{
			((Control)textBoxText).Text = StiExpressionPacker.UnPackExpression(text, designer, useBraces: false);
			((Control)textBoxText).Focus();
			textBoxText.Select(0, 0);
			RefreshElementView();
		}
	}

	private void CheckBoxImageAspectRatio_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.ImageAspectRatio = ((CheckBox)checkBoxImageAspectRatio).Checked;
			RefreshElementView();
			if (((CheckBox)checkBoxImageAspectRatio).Checked)
			{
				((CheckBox)checkBoxImageTiling).Checked = false;
			}
		}
	}

	private void TrackBarImageSize_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelImageSizeHint).Text = ((TrackBar)trackBarImageSize).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.ImageMultipleFactor = (float)((TrackBar)trackBarImageSize).Value / 10f;
			RefreshElementView();
		}
	}

	private void RadioButtonImageAlign_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (((RadioButton)radioButtonImageHorAlignLeft).Checked && ((RadioButton)radioButtonImageVertAlignTop).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)1;
			}
			else if (((RadioButton)radioButtonImageHorAlignCenter).Checked && ((RadioButton)radioButtonImageVertAlignTop).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)2;
			}
			else if (((RadioButton)radioButtonImageHorAlignRight).Checked && ((RadioButton)radioButtonImageVertAlignTop).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)4;
			}
			else if (((RadioButton)radioButtonImageHorAlignLeft).Checked && ((RadioButton)radioButtonImageVertAlignCenter).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)16;
			}
			else if (((RadioButton)radioButtonImageHorAlignCenter).Checked && ((RadioButton)radioButtonImageVertAlignCenter).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)32;
			}
			else if (((RadioButton)radioButtonImageHorAlignRight).Checked && ((RadioButton)radioButtonImageVertAlignCenter).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)64;
			}
			else if (((RadioButton)radioButtonImageHorAlignLeft).Checked && ((RadioButton)radioButtonImageVertAlignBottom).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)256;
			}
			else if (((RadioButton)radioButtonImageHorAlignCenter).Checked && ((RadioButton)radioButtonImageVertAlignBottom).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)512;
			}
			else if (((RadioButton)radioButtonImageHorAlignRight).Checked && ((RadioButton)radioButtonImageVertAlignBottom).Checked)
			{
				watermark.ImageAlignment = (ContentAlignment)1024;
			}
			RefreshElementView();
		}
	}

	private void ColorBoxWeaveMajorColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.WeaveMajorColor = colorBoxWeaveMajorColor.SelectedColor;
			BuildMajorIconPanel();
			DrawBoxMajorIcon();
			RefreshElementView();
		}
	}

	private void ColorBoxWeaveMinorColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.WeaveMinorColor = colorBoxWeaveMinorColor.SelectedColor;
			BuildMinorIconPanel();
			DrawBoxMinorIcon();
			RefreshElementView();
		}
	}

	private void TrackBarWeaveDistance_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelWeaveDistanceHint).Text = ((TrackBar)trackBarWeaveDistance).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.WeaveDistance = ((TrackBar)trackBarWeaveDistance).Value;
			RefreshElementView();
		}
	}

	private void TrackBarWeaveAngle_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelWeaveAngleHint).Text = ((TrackBar)trackBarWeaveAngle).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.WeaveAngle = ((TrackBar)trackBarWeaveAngle).Value;
			RefreshElementView();
		}
	}

	private void TrackBarTextAngle_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelTextAngleHint).Text = ((TrackBar)trackBarTextAngle).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.TextAngle = ((TrackBar)trackBarTextAngle).Value;
			RefreshElementView();
		}
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		pageText.ShowMarker = watermark.TextEnabled;
		pageImage.ShowMarker = watermark.ImageEnabled;
		pageWeave.ShowMarker = watermark.WeaveEnabled;
	}

	private void TrackBarWeaveMajorSize_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelWeaveMajorSizeHint).Text = ((TrackBar)trackBarWeaveMajorSize).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.WeaveMajorSize = ((TrackBar)trackBarWeaveMajorSize).Value;
			RefreshElementView();
		}
	}

	private void TrackBarWeaveMinorSize_ValueChanged(object sender, EventArgs e)
	{
		((Control)labelWeaveMinorSizeHint).Text = ((TrackBar)trackBarWeaveMinorSize).Value.ToString();
		if (!base.LockRefresh)
		{
			watermark.WeaveMinorSize = ((TrackBar)trackBarWeaveMinorSize).Value;
			RefreshElementView();
		}
	}

	private void ButtonMajorIconRemove_Click(object sender, EventArgs e)
	{
		watermark.WeaveMajorIcon = null;
		CheckIconsState();
		RefreshElementView();
	}

	private void ButtonMinorIconRemove_Click(object sender, EventArgs e)
	{
		watermark.WeaveMinorIcon = null;
		CheckIconsState();
		RefreshElementView();
	}

	private void ButtonMajorIcon_Click(object sender, EventArgs e)
	{
		CheckColorMajorIcons();
		((ToolStripDropDown)majorIconsPopup).Show(new Point(((Control)panelBoxWeaveMajorIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonMajorIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonMajorIcon).Height + StiScale.I2));
		((Control)textBoxMajorSearch).Focus();
	}

	private void ButtonMinorIcon_Click(object sender, EventArgs e)
	{
		CheckColorMinorIcons();
		((ToolStripDropDown)minorIconsPopup).Show(new Point(((Control)panelBoxWeaveMinorIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonMinorIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonMinorIcon).Height + StiScale.I2));
		((Control)textBoxMinorSearch).Focus();
	}

	private void FontMajorIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		watermark.WeaveMajorIcon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		DrawBoxMajorIcon();
		((Control)majorIconsPopup).Hide();
		CheckIconsState();
		RefreshElementView();
	}

	private void FontMinorIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		watermark.WeaveMinorIcon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		DrawBoxMinorIcon();
		((Control)minorIconsPopup).Hide();
		CheckIconsState();
		RefreshElementView();
	}

	private void TextBoxMajorSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildMajorIconPanel(filterName);
	}

	private void MinorSearchTextBox_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildMinorIconPanel(filterName);
	}

	private void CheckBoxTextEnabled_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.TextEnabled = ((CheckBox)checkBoxTextEnabled).Checked;
			RefreshElementView();
			CheckTextStates();
		}
	}

	private void CheckBoxImageEnabled_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.ImageEnabled = ((CheckBox)checkBoxImageEnabled).Checked;
			RefreshElementView();
			CheckImageStates();
		}
	}

	private void CheckBoxWeaveEnabled_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			watermark.WeaveEnabled = ((CheckBox)checkBoxWeaveEnabled).Checked;
			RefreshElementView();
			CheckIconsState();
		}
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					watermark.ImageBytes = StiImageHelper.TryBytesFromFile(item);
					CheckImageStates();
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Expected O, but got Unknown
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Expected O, but got Unknown
		//IL_0f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f93: Expected O, but got Unknown
		//IL_0fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Expected O, but got Unknown
		//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe9: Expected O, but got Unknown
		//IL_1070: Unknown result type (might be due to invalid IL or missing references)
		//IL_107a: Expected O, but got Unknown
		//IL_11eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f5: Expected O, but got Unknown
		//IL_1206: Unknown result type (might be due to invalid IL or missing references)
		//IL_1210: Expected O, but got Unknown
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1298: Expected O, but got Unknown
		//IL_12a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b3: Expected O, but got Unknown
		//IL_1545: Unknown result type (might be due to invalid IL or missing references)
		//IL_1602: Unknown result type (might be due to invalid IL or missing references)
		//IL_165a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1664: Expected O, but got Unknown
		//IL_1681: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2013: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_213b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2145: Expected O, but got Unknown
		//IL_2162: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2579: Unknown result type (might be due to invalid IL or missing references)
		//IL_2731: Unknown result type (might be due to invalid IL or missing references)
		//IL_284f: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd4: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiWatermarkEditorForm));
		toolTip1 = new ToolTip(components);
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panelVerAlignment = new StiPanelUX();
		radioButtonImageVertAlignTop = new StiOperationRadioButton();
		radioButtonImageVertAlignCenter = new StiOperationRadioButton();
		radioButtonImageVertAlignBottom = new StiOperationRadioButton();
		panelHorAlignment = new StiPanelUX();
		radioButtonImageHorAlignLeft = new StiOperationRadioButton();
		radioButtonImageHorAlignCenter = new StiOperationRadioButton();
		radioButtonImageHorAlignRight = new StiOperationRadioButton();
		trackBarImageSize = new StiTrackBarUX();
		trackBarImageTransparency = new StiTrackBarUX();
		labelImageAlignment = new StiLabelUX();
		labelImageMultipleFactor = new StiLabelUX();
		checkBoxImageAspectRatio = new StiCheckBoxUX();
		checkBoxImageTiling = new StiCheckBoxUX();
		checkBoxImageStretch = new StiCheckBoxUX();
		labelImageTransparency = new StiLabelUX();
		panelImage = new StiPanelUX();
		buttonImageOpen = new StiButtonUX();
		buttonImageRemove = new StiButtonUX();
		labelTextColor = new StiLabelUX();
		colorBoxTextColor = new StiColorBoxXF();
		textBoxFont = new StiButtonEdit();
		textBoxText = new StiButtonEdit();
		labelText = new StiLabelUX();
		labelTextFont = new StiLabelUX();
		labelTextAngle = new StiLabelUX();
		panelFooter = new StiButtonPanelUX();
		colorBoxWeaveMajorColor = new StiColorBoxXF();
		panelBoxWeaveMajorIcon = new StiPanelUX();
		buttonMajorIconRemove = new StiOperationButton();
		buttonMajorIcon = new StiOperationButton();
		labelWeaveAngle = new StiLabelUX();
		trackBarWeaveDistance = new StiTrackBarUX();
		labelWeaveDistance = new StiLabelUX();
		labelWeaveMajorIcon = new StiLabelUX();
		labelImageSizeHint = new StiLabelUX();
		labelImageTransparencyHint = new StiLabelUX();
		checkBoxImageEnabled = new StiCheckBoxUX();
		checkBoxWeaveEnabled = new StiCheckBoxUX();
		stiGroupLine2 = new StiGroupPanelUX();
		labelWeaveAngleHint = new StiLabelUX();
		labelWeaveDistanceHint = new StiLabelUX();
		trackBarWeaveAngle = new StiTrackBarUX();
		groupBoxWeaveMinor = new StiOffice2013GroupBox();
		labelWeaveMinorSizeHint = new StiLabelUX();
		labelWeaveMinorSize = new StiLabelUX();
		trackBarWeaveMinorSize = new StiTrackBarUX();
		panelBoxWeaveMinorIcon = new StiPanelUX();
		buttonMinorIconRemove = new StiOperationButton();
		buttonMinorIcon = new StiOperationButton();
		labelWeaveMinorIcon = new StiLabelUX();
		colorBoxWeaveMinorColor = new StiColorBoxXF();
		groupBoxWeaveMajor = new StiOffice2013GroupBox();
		labelWeaveMajorSizeHint = new StiLabelUX();
		labelWeaveMajorSize = new StiLabelUX();
		trackBarWeaveMajorSize = new StiTrackBarUX();
		labelTextAngleHint = new StiLabelUX();
		checkBoxTextEnabled = new StiCheckBoxUX();
		trackBarTextAngle = new StiTrackBarUX();
		timerMarker = new Timer(components);
		TabControl = new StiCloudTabulator();
		pageText = new StiCloudTabulatorPage();
		pageImage = new StiCloudTabulatorPage();
		pageWeave = new StiCloudTabulatorPage();
		((Control)panelVerAlignment).SuspendLayout();
		((Control)panelHorAlignment).SuspendLayout();
		((ISupportInitialize)trackBarImageSize).BeginInit();
		((ISupportInitialize)trackBarImageTransparency).BeginInit();
		((Control)panelImage).SuspendLayout();
		((Control)panelFooter).SuspendLayout();
		((Control)panelBoxWeaveMajorIcon).SuspendLayout();
		((ISupportInitialize)trackBarWeaveDistance).BeginInit();
		((Control)stiGroupLine2).SuspendLayout();
		((ISupportInitialize)trackBarWeaveAngle).BeginInit();
		((Control)groupBoxWeaveMinor).SuspendLayout();
		((ISupportInitialize)trackBarWeaveMinorSize).BeginInit();
		((Control)panelBoxWeaveMinorIcon).SuspendLayout();
		((Control)groupBoxWeaveMajor).SuspendLayout();
		((ISupportInitialize)trackBarWeaveMajorSize).BeginInit();
		((ISupportInitialize)trackBarTextAngle).BeginInit();
		((Control)TabControl).SuspendLayout();
		((Control)pageText).SuspendLayout();
		((Control)pageImage).SuspendLayout();
		((Control)pageWeave).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(429, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(509, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelVerAlignment).Controls.Add((Control)(object)radioButtonImageVertAlignTop);
		((Control)panelVerAlignment).Controls.Add((Control)(object)radioButtonImageVertAlignCenter);
		((Control)panelVerAlignment).Controls.Add((Control)(object)radioButtonImageVertAlignBottom);
		((Control)panelVerAlignment).Location = new Point(258, 136);
		((Control)panelVerAlignment).Margin = new Padding(2, 2, 2, 2);
		((Control)panelVerAlignment).Name = "panelVerAlignment";
		((Control)panelVerAlignment).Size = new Size(100, 26);
		((Control)panelVerAlignment).TabIndex = 3;
		((RadioButton)radioButtonImageVertAlignTop).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageVertAlignTop).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageVertAlignTop).Image = (Image)componentResourceManager.GetObject("radioButtonImageVertAlignTop.Image");
		((Control)radioButtonImageVertAlignTop).Location = new Point(0, 0);
		((Control)radioButtonImageVertAlignTop).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageVertAlignTop).Name = "radioButtonImageVertAlignTop";
		((Control)radioButtonImageVertAlignTop).Size = new Size(25, 26);
		((Control)radioButtonImageVertAlignTop).TabIndex = 0;
		((ButtonBase)radioButtonImageVertAlignTop).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageVertAlignTop).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((RadioButton)radioButtonImageVertAlignCenter).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageVertAlignCenter).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageVertAlignCenter).Image = (Image)componentResourceManager.GetObject("radioButtonImageVertAlignCenter.Image");
		((Control)radioButtonImageVertAlignCenter).Location = new Point(30, 0);
		((Control)radioButtonImageVertAlignCenter).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageVertAlignCenter).Name = "radioButtonImageVertAlignCenter";
		((Control)radioButtonImageVertAlignCenter).Size = new Size(25, 26);
		((Control)radioButtonImageVertAlignCenter).TabIndex = 1;
		((ButtonBase)radioButtonImageVertAlignCenter).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageVertAlignCenter).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((RadioButton)radioButtonImageVertAlignBottom).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageVertAlignBottom).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageVertAlignBottom).Image = (Image)componentResourceManager.GetObject("radioButtonImageVertAlignBottom.Image");
		((Control)radioButtonImageVertAlignBottom).Location = new Point(59, 0);
		((Control)radioButtonImageVertAlignBottom).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageVertAlignBottom).Name = "radioButtonImageVertAlignBottom";
		((Control)radioButtonImageVertAlignBottom).Size = new Size(25, 26);
		((Control)radioButtonImageVertAlignBottom).TabIndex = 2;
		((ButtonBase)radioButtonImageVertAlignBottom).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageVertAlignBottom).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((Control)panelHorAlignment).Controls.Add((Control)(object)radioButtonImageHorAlignLeft);
		((Control)panelHorAlignment).Controls.Add((Control)(object)radioButtonImageHorAlignCenter);
		((Control)panelHorAlignment).Controls.Add((Control)(object)radioButtonImageHorAlignRight);
		((Control)panelHorAlignment).Location = new Point(156, 136);
		((Control)panelHorAlignment).Margin = new Padding(2, 2, 2, 2);
		((Control)panelHorAlignment).Name = "panelHorAlignment";
		((Control)panelHorAlignment).Size = new Size(100, 26);
		((Control)panelHorAlignment).TabIndex = 2;
		((RadioButton)radioButtonImageHorAlignLeft).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageHorAlignLeft).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageHorAlignLeft).Image = (Image)componentResourceManager.GetObject("radioButtonImageHorAlignLeft.Image");
		((Control)radioButtonImageHorAlignLeft).Location = new Point(0, 0);
		((Control)radioButtonImageHorAlignLeft).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageHorAlignLeft).Name = "radioButtonImageHorAlignLeft";
		((Control)radioButtonImageHorAlignLeft).Size = new Size(25, 26);
		((Control)radioButtonImageHorAlignLeft).TabIndex = 0;
		((ButtonBase)radioButtonImageHorAlignLeft).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageHorAlignLeft).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((RadioButton)radioButtonImageHorAlignCenter).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageHorAlignCenter).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageHorAlignCenter).Image = (Image)componentResourceManager.GetObject("radioButtonImageHorAlignCenter.Image");
		((Control)radioButtonImageHorAlignCenter).Location = new Point(30, 0);
		((Control)radioButtonImageHorAlignCenter).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageHorAlignCenter).Name = "radioButtonImageHorAlignCenter";
		((Control)radioButtonImageHorAlignCenter).Size = new Size(25, 26);
		((Control)radioButtonImageHorAlignCenter).TabIndex = 1;
		((ButtonBase)radioButtonImageHorAlignCenter).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageHorAlignCenter).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((RadioButton)radioButtonImageHorAlignRight).Appearance = (Appearance)1;
		((ButtonBase)radioButtonImageHorAlignRight).FlatAppearance.BorderSize = 0;
		((ButtonBase)radioButtonImageHorAlignRight).Image = (Image)componentResourceManager.GetObject("radioButtonImageHorAlignRight.Image");
		((Control)radioButtonImageHorAlignRight).Location = new Point(59, 0);
		((Control)radioButtonImageHorAlignRight).Margin = new Padding(2, 2, 2, 2);
		((Control)radioButtonImageHorAlignRight).Name = "radioButtonImageHorAlignRight";
		((Control)radioButtonImageHorAlignRight).Size = new Size(25, 26);
		((Control)radioButtonImageHorAlignRight).TabIndex = 2;
		((ButtonBase)radioButtonImageHorAlignRight).UseVisualStyleBackColor = true;
		((RadioButton)radioButtonImageHorAlignRight).CheckedChanged += RadioButtonImageAlign_CheckedChanged;
		((Control)trackBarImageSize).AutoSize = false;
		((Control)trackBarImageSize).Location = new Point(156, 210);
		((Control)trackBarImageSize).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarImageSize).Maximum = 50;
		((TrackBar)trackBarImageSize).Minimum = 1;
		((Control)trackBarImageSize).Name = "trackBarImageSize";
		((Control)trackBarImageSize).Size = new Size(123, 14);
		((Control)trackBarImageSize).TabIndex = 7;
		((TrackBar)trackBarImageSize).TickStyle = (TickStyle)0;
		((TrackBar)trackBarImageSize).Value = 1;
		((TrackBar)trackBarImageSize).ValueChanged += TrackBarImageSize_ValueChanged;
		((Control)trackBarImageTransparency).AutoSize = false;
		((Control)trackBarImageTransparency).Location = new Point(156, 175);
		((Control)trackBarImageTransparency).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarImageTransparency).Maximum = 255;
		((Control)trackBarImageTransparency).Name = "trackBarImageTransparency";
		((Control)trackBarImageTransparency).Size = new Size(123, 14);
		((Control)trackBarImageTransparency).TabIndex = 6;
		((TrackBar)trackBarImageTransparency).TickStyle = (TickStyle)0;
		((TrackBar)trackBarImageTransparency).ValueChanged += TrackBarImageTransparency_ValueChanged;
		((Label)labelImageAlignment).AutoEllipsis = true;
		((Control)labelImageAlignment).Location = new Point(12, 138);
		((Control)labelImageAlignment).Name = "labelImageAlignment";
		((Control)labelImageAlignment).Size = new Size(139, 21);
		((Control)labelImageAlignment).TabIndex = 22;
		((Control)labelImageAlignment).Text = "Image Alignment";
		((Label)labelImageAlignment).TextAlign = (ContentAlignment)16;
		((Label)labelImageMultipleFactor).AutoEllipsis = true;
		((Control)labelImageMultipleFactor).Location = new Point(12, 209);
		((Control)labelImageMultipleFactor).Name = "labelImageMultipleFactor";
		((Control)labelImageMultipleFactor).Size = new Size(136, 15);
		((Control)labelImageMultipleFactor).TabIndex = 24;
		((Control)labelImageMultipleFactor).Text = "Image Multiple Factor";
		((Label)labelImageMultipleFactor).TextAlign = (ContentAlignment)16;
		((Control)checkBoxImageAspectRatio).AutoSize = true;
		((Control)checkBoxImageAspectRatio).BackColor = Color.Transparent;
		((Control)checkBoxImageAspectRatio).Location = new Point(156, 239);
		((Control)checkBoxImageAspectRatio).Name = "checkBoxImageAspectRatio";
		((Control)checkBoxImageAspectRatio).Size = new Size(87, 17);
		((Control)checkBoxImageAspectRatio).TabIndex = 8;
		((Control)checkBoxImageAspectRatio).Text = "Aspect Ratio";
		((ButtonBase)checkBoxImageAspectRatio).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxImageAspectRatio).CheckedChanged += CheckBoxImageAspectRatio_CheckedChanged;
		((Control)checkBoxImageTiling).AutoSize = true;
		((Control)checkBoxImageTiling).BackColor = Color.Transparent;
		((Control)checkBoxImageTiling).Location = new Point(156, 289);
		((Control)checkBoxImageTiling).Name = "checkBoxImageTiling";
		((Control)checkBoxImageTiling).Size = new Size(83, 17);
		((Control)checkBoxImageTiling).TabIndex = 10;
		((Control)checkBoxImageTiling).Text = "Image Tiling";
		((ButtonBase)checkBoxImageTiling).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxImageTiling).CheckedChanged += CheckBoxWatermarkImageTiling_CheckedChanged;
		((Control)checkBoxImageStretch).AutoSize = true;
		((Control)checkBoxImageStretch).BackColor = Color.Transparent;
		((Control)checkBoxImageStretch).Location = new Point(156, 264);
		((Control)checkBoxImageStretch).Name = "checkBoxImageStretch";
		((Control)checkBoxImageStretch).Size = new Size(92, 17);
		((Control)checkBoxImageStretch).TabIndex = 9;
		((Control)checkBoxImageStretch).Text = "Image Stretch";
		((ButtonBase)checkBoxImageStretch).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxImageStretch).CheckedChanged += CheckBoxWatermarkImageStretch_CheckedChanged;
		((Label)labelImageTransparency).AutoEllipsis = true;
		((Control)labelImageTransparency).Location = new Point(12, 175);
		((Control)labelImageTransparency).Name = "labelImageTransparency";
		((Control)labelImageTransparency).Size = new Size(139, 15);
		((Control)labelImageTransparency).TabIndex = 23;
		((Control)labelImageTransparency).Text = "Image Transparency";
		((Label)labelImageTransparency).TextAlign = (ContentAlignment)16;
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Controls.Add((Control)(object)buttonImageOpen);
		((Control)panelImage).Controls.Add((Control)(object)buttonImageRemove);
		((Control)panelImage).Location = new Point(12, 40);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(419, 85);
		((Control)panelImage).TabIndex = 1;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonImageOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageOpen).Image = (Image)componentResourceManager.GetObject("buttonImageOpen.Image");
		((Control)buttonImageOpen).Location = new Point(389, 6);
		((Control)buttonImageOpen).Name = "buttonImageOpen";
		((Control)buttonImageOpen).Size = new Size(24, 24);
		((Control)buttonImageOpen).TabIndex = 0;
		((ButtonBase)buttonImageOpen).UseVisualStyleBackColor = true;
		((Control)buttonImageOpen).Click += ButtonImageOpen_Click;
		((Control)buttonImageRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageRemove).Image = (Image)componentResourceManager.GetObject("buttonImageRemove.Image");
		((Control)buttonImageRemove).Location = new Point(389, 34);
		((Control)buttonImageRemove).Name = "buttonImageRemove";
		((Control)buttonImageRemove).Size = new Size(24, 24);
		((Control)buttonImageRemove).TabIndex = 1;
		((ButtonBase)buttonImageRemove).UseVisualStyleBackColor = true;
		((Control)buttonImageRemove).Click += ButtonImageRemove_Click;
		((Label)labelTextColor).AutoEllipsis = true;
		((Control)labelTextColor).Location = new Point(6, 109);
		((Control)labelTextColor).Name = "labelTextColor";
		((Control)labelTextColor).Size = new Size(136, 22);
		((Control)labelTextColor).TabIndex = 19;
		((Control)labelTextColor).Text = "Text Color";
		((Label)labelTextColor).TextAlign = (ContentAlignment)16;
		colorBoxTextColor.Flat = false;
		((Control)colorBoxTextColor).Location = new Point(156, 109);
		((Control)colorBoxTextColor).Name = "colorBoxTextColor";
		colorBoxTextColor.SelectedColor = Color.Transparent;
		colorBoxTextColor.ShowColorName = false;
		((Control)colorBoxTextColor).Size = new Size(123, 22);
		((Control)colorBoxTextColor).TabIndex = 3;
		colorBoxTextColor.SelectedColorChanged += ColorBoxTextColor_SelectedColorChanged;
		textBoxFont.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxFont.ButtonBitmap");
		textBoxFont.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxFont.ButtonIcon");
		((Control)textBoxFont).Location = new Point(156, 73);
		((Control)textBoxFont).Name = "textBoxFont";
		textBoxFont.ReadOnly = true;
		((Control)textBoxFont).Size = new Size(275, 21);
		((Control)textBoxFont).TabIndex = 2;
		textBoxFont.ButtonClick += TextBoxFont_ButtonClick;
		textBoxText.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxText.ButtonBitmap");
		textBoxText.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxText.ButtonIcon");
		((Control)textBoxText).Location = new Point(156, 40);
		((Control)textBoxText).Name = "textBoxText";
		((Control)textBoxText).Size = new Size(275, 21);
		((Control)textBoxText).TabIndex = 1;
		textBoxText.ButtonClick += TextBoxText_ButtonClick;
		((Control)textBoxText).TextChanged += TextBoxText_TextChanged;
		((Label)labelText).AutoEllipsis = true;
		((Control)labelText).Location = new Point(6, 40);
		((Control)labelText).Name = "labelText";
		((Control)labelText).Size = new Size(136, 22);
		((Control)labelText).TabIndex = 16;
		((Control)labelText).Text = "Text";
		((Label)labelText).TextAlign = (ContentAlignment)16;
		((Label)labelTextFont).AutoEllipsis = true;
		((Control)labelTextFont).Location = new Point(6, 73);
		((Control)labelTextFont).Name = "labelTextFont";
		((Control)labelTextFont).Size = new Size(136, 22);
		((Control)labelTextFont).TabIndex = 17;
		((Control)labelTextFont).Text = "Font";
		((Label)labelTextFont).TextAlign = (ContentAlignment)16;
		((Label)labelTextAngle).AutoEllipsis = true;
		((Control)labelTextAngle).Location = new Point(6, 147);
		((Control)labelTextAngle).Name = "labelTextAngle";
		((Control)labelTextAngle).Size = new Size(136, 15);
		((Control)labelTextAngle).TabIndex = 18;
		((Control)labelTextAngle).Text = "Angle";
		((Label)labelTextAngle).TextAlign = (ContentAlignment)16;
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 354);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(597, 46);
		((Control)panelFooter).TabIndex = 1;
		((Control)colorBoxWeaveMajorColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxWeaveMajorColor).Location = new Point(277, 41);
		((Control)colorBoxWeaveMajorColor).Margin = new Padding(2, 2, 2, 2);
		((Control)colorBoxWeaveMajorColor).Name = "colorBoxWeaveMajorColor";
		colorBoxWeaveMajorColor.SelectedColor = Color.Transparent;
		colorBoxWeaveMajorColor.ShowColorName = false;
		((Control)colorBoxWeaveMajorColor).Size = new Size(143, 25);
		((Control)colorBoxWeaveMajorColor).TabIndex = 2;
		colorBoxWeaveMajorColor.SelectedColorChanged += ColorBoxWeaveMajorColor_SelectedColorChanged;
		((Control)panelBoxWeaveMajorIcon).Controls.Add((Control)(object)buttonMajorIconRemove);
		((Control)panelBoxWeaveMajorIcon).Controls.Add((Control)(object)buttonMajorIcon);
		((Control)panelBoxWeaveMajorIcon).Location = new Point(145, 41);
		((Control)panelBoxWeaveMajorIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)panelBoxWeaveMajorIcon).Name = "panelBoxWeaveMajorIcon";
		((Control)panelBoxWeaveMajorIcon).Size = new Size(123, 25);
		((Control)panelBoxWeaveMajorIcon).TabIndex = 1;
		((Control)buttonMajorIconRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonMajorIconRemove).Image = (Image)componentResourceManager.GetObject("buttonMajorIconRemove.Image");
		((Control)buttonMajorIconRemove).Location = new Point(76, 1);
		((Control)buttonMajorIconRemove).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonMajorIconRemove).Name = "buttonMajorIconRemove";
		((Control)buttonMajorIconRemove).Size = new Size(24, 23);
		((Control)buttonMajorIconRemove).TabIndex = 2;
		((Control)buttonMajorIconRemove).TabStop = false;
		((ButtonBase)buttonMajorIconRemove).UseVisualStyleBackColor = true;
		((Control)buttonMajorIconRemove).Click += ButtonMajorIconRemove_Click;
		((Control)buttonMajorIcon).Anchor = (AnchorStyles)9;
		((Control)buttonMajorIcon).Location = new Point(99, 1);
		((Control)buttonMajorIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonMajorIcon).Name = "buttonMajorIcon";
		((Control)buttonMajorIcon).Size = new Size(23, 23);
		((Control)buttonMajorIcon).TabIndex = 1;
		((Control)buttonMajorIcon).TabStop = false;
		((ButtonBase)buttonMajorIcon).UseVisualStyleBackColor = true;
		((Control)buttonMajorIcon).Click += ButtonMajorIcon_Click;
		((Label)labelWeaveAngle).AutoEllipsis = true;
		((Control)labelWeaveAngle).Location = new Point(0, 46);
		((Control)labelWeaveAngle).Name = "labelWeaveAngle";
		((Control)labelWeaveAngle).Size = new Size(136, 15);
		((Control)labelWeaveAngle).TabIndex = 31;
		((Control)labelWeaveAngle).Text = "Angle";
		((Label)labelWeaveAngle).TextAlign = (ContentAlignment)16;
		((Control)trackBarWeaveDistance).AutoSize = false;
		((Control)trackBarWeaveDistance).Location = new Point(145, 13);
		((Control)trackBarWeaveDistance).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarWeaveDistance).Maximum = 200;
		((TrackBar)trackBarWeaveDistance).Minimum = 50;
		((Control)trackBarWeaveDistance).Name = "trackBarWeaveDistance";
		((Control)trackBarWeaveDistance).Size = new Size(123, 14);
		((Control)trackBarWeaveDistance).TabIndex = 1;
		((TrackBar)trackBarWeaveDistance).TickStyle = (TickStyle)0;
		((TrackBar)trackBarWeaveDistance).Value = 100;
		((TrackBar)trackBarWeaveDistance).ValueChanged += TrackBarWeaveDistance_ValueChanged;
		((Label)labelWeaveDistance).AutoEllipsis = true;
		((Control)labelWeaveDistance).Location = new Point(0, 12);
		((Control)labelWeaveDistance).Name = "labelWeaveDistance";
		((Control)labelWeaveDistance).Size = new Size(136, 15);
		((Control)labelWeaveDistance).TabIndex = 33;
		((Control)labelWeaveDistance).Text = "Distance";
		((Label)labelWeaveDistance).TextAlign = (ContentAlignment)16;
		((Label)labelWeaveMajorIcon).AutoEllipsis = true;
		((Control)labelWeaveMajorIcon).Location = new Point(0, 42);
		((Control)labelWeaveMajorIcon).Name = "labelWeaveMajorIcon";
		((Control)labelWeaveMajorIcon).Size = new Size(136, 22);
		((Control)labelWeaveMajorIcon).TabIndex = 34;
		((Control)labelWeaveMajorIcon).Text = "Icon";
		((Label)labelWeaveMajorIcon).TextAlign = (ContentAlignment)16;
		((Label)labelImageSizeHint).AutoEllipsis = true;
		((Control)labelImageSizeHint).Location = new Point(278, 209);
		((Control)labelImageSizeHint).Name = "labelImageSizeHint";
		((Control)labelImageSizeHint).Size = new Size(27, 15);
		((Control)labelImageSizeHint).TabIndex = 5;
		((Control)labelImageSizeHint).Text = "0";
		((Label)labelImageSizeHint).TextAlign = (ContentAlignment)64;
		((Label)labelImageSizeHint).UseCompatibleTextRendering = true;
		((Label)labelImageTransparencyHint).AutoEllipsis = true;
		((Control)labelImageTransparencyHint).Location = new Point(278, 175);
		((Control)labelImageTransparencyHint).Name = "labelImageTransparencyHint";
		((Control)labelImageTransparencyHint).Size = new Size(27, 15);
		((Control)labelImageTransparencyHint).TabIndex = 4;
		((Control)labelImageTransparencyHint).Text = "0";
		((Label)labelImageTransparencyHint).TextAlign = (ContentAlignment)64;
		((Label)labelImageTransparencyHint).UseCompatibleTextRendering = true;
		((Control)checkBoxImageEnabled).AutoSize = true;
		((Control)checkBoxImageEnabled).Location = new Point(156, 12);
		((Control)checkBoxImageEnabled).Name = "checkBoxImageEnabled";
		((Control)checkBoxImageEnabled).Size = new Size(65, 17);
		((Control)checkBoxImageEnabled).TabIndex = 0;
		((Control)checkBoxImageEnabled).Text = "Enabled";
		((ButtonBase)checkBoxImageEnabled).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxImageEnabled).CheckedChanged += CheckBoxImageEnabled_CheckedChanged;
		((Control)checkBoxWeaveEnabled).AutoSize = true;
		((Control)checkBoxWeaveEnabled).Location = new Point(156, 12);
		((Control)checkBoxWeaveEnabled).Name = "checkBoxWeaveEnabled";
		((Control)checkBoxWeaveEnabled).Size = new Size(65, 17);
		((Control)checkBoxWeaveEnabled).TabIndex = 0;
		((Control)checkBoxWeaveEnabled).Text = "Enabled";
		((ButtonBase)checkBoxWeaveEnabled).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxWeaveEnabled).CheckedChanged += CheckBoxWeaveEnabled_CheckedChanged;
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelWeaveAngleHint);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelWeaveDistanceHint);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelWeaveDistance);
		((Control)stiGroupLine2).Controls.Add((Control)(object)trackBarWeaveAngle);
		((Control)stiGroupLine2).Controls.Add((Control)(object)trackBarWeaveDistance);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelWeaveAngle);
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(11, 248);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(420, 71);
		((Control)stiGroupLine2).TabIndex = 3;
		((Label)labelWeaveAngleHint).AutoEllipsis = true;
		((Control)labelWeaveAngleHint).Location = new Point(267, 46);
		((Control)labelWeaveAngleHint).Name = "labelWeaveAngleHint";
		((Control)labelWeaveAngleHint).Size = new Size(27, 15);
		((Control)labelWeaveAngleHint).TabIndex = 2;
		((Control)labelWeaveAngleHint).Text = "0";
		((Label)labelWeaveAngleHint).TextAlign = (ContentAlignment)64;
		((Label)labelWeaveAngleHint).UseCompatibleTextRendering = true;
		((Label)labelWeaveDistanceHint).AutoEllipsis = true;
		((Control)labelWeaveDistanceHint).Location = new Point(267, 12);
		((Control)labelWeaveDistanceHint).Name = "labelWeaveDistanceHint";
		((Control)labelWeaveDistanceHint).Size = new Size(27, 15);
		((Control)labelWeaveDistanceHint).TabIndex = 0;
		((Control)labelWeaveDistanceHint).Text = "0";
		((Label)labelWeaveDistanceHint).TextAlign = (ContentAlignment)64;
		((Label)labelWeaveDistanceHint).UseCompatibleTextRendering = true;
		((Control)trackBarWeaveAngle).AutoSize = false;
		((Control)trackBarWeaveAngle).Location = new Point(145, 47);
		((Control)trackBarWeaveAngle).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarWeaveAngle).Maximum = 360;
		((Control)trackBarWeaveAngle).Name = "trackBarWeaveAngle";
		((Control)trackBarWeaveAngle).Size = new Size(123, 14);
		((Control)trackBarWeaveAngle).TabIndex = 3;
		((TrackBar)trackBarWeaveAngle).TickStyle = (TickStyle)0;
		((TrackBar)trackBarWeaveAngle).ValueChanged += TrackBarWeaveAngle_ValueChanged;
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)labelWeaveMinorSizeHint);
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)labelWeaveMinorSize);
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)trackBarWeaveMinorSize);
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)panelBoxWeaveMinorIcon);
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)labelWeaveMinorIcon);
		((Control)groupBoxWeaveMinor).Controls.Add((Control)(object)colorBoxWeaveMinorColor);
		groupBoxWeaveMinor.HeaderHeight = 24;
		((Control)groupBoxWeaveMinor).Location = new Point(11, 143);
		((Control)groupBoxWeaveMinor).Name = "groupBoxWeaveMinor";
		((Control)groupBoxWeaveMinor).Size = new Size(420, 103);
		((Control)groupBoxWeaveMinor).TabIndex = 2;
		((Control)groupBoxWeaveMinor).TabStop = false;
		((Control)groupBoxWeaveMinor).Text = "Minor";
		((Label)labelWeaveMinorSizeHint).AutoEllipsis = true;
		((Control)labelWeaveMinorSizeHint).Location = new Point(267, 79);
		((Control)labelWeaveMinorSizeHint).Name = "labelWeaveMinorSizeHint";
		((Control)labelWeaveMinorSizeHint).Size = new Size(27, 15);
		((Control)labelWeaveMinorSizeHint).TabIndex = 2;
		((Control)labelWeaveMinorSizeHint).Text = "0";
		((Label)labelWeaveMinorSizeHint).TextAlign = (ContentAlignment)64;
		((Label)labelWeaveMinorSizeHint).UseCompatibleTextRendering = true;
		((Label)labelWeaveMinorSize).AutoEllipsis = true;
		((Control)labelWeaveMinorSize).Location = new Point(0, 79);
		((Control)labelWeaveMinorSize).Name = "labelWeaveMinorSize";
		((Control)labelWeaveMinorSize).Size = new Size(136, 15);
		((Control)labelWeaveMinorSize).TabIndex = 36;
		((Control)labelWeaveMinorSize).Text = "Size";
		((Label)labelWeaveMinorSize).TextAlign = (ContentAlignment)16;
		((Control)trackBarWeaveMinorSize).AutoSize = false;
		((Control)trackBarWeaveMinorSize).Location = new Point(145, 80);
		((Control)trackBarWeaveMinorSize).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarWeaveMinorSize).Maximum = 30;
		((TrackBar)trackBarWeaveMinorSize).Minimum = 5;
		((Control)trackBarWeaveMinorSize).Name = "trackBarWeaveMinorSize";
		((Control)trackBarWeaveMinorSize).Size = new Size(123, 14);
		((Control)trackBarWeaveMinorSize).TabIndex = 3;
		((TrackBar)trackBarWeaveMinorSize).TickStyle = (TickStyle)0;
		((TrackBar)trackBarWeaveMinorSize).Value = 10;
		((TrackBar)trackBarWeaveMinorSize).ValueChanged += TrackBarWeaveMinorSize_ValueChanged;
		((Control)panelBoxWeaveMinorIcon).Controls.Add((Control)(object)buttonMinorIconRemove);
		((Control)panelBoxWeaveMinorIcon).Controls.Add((Control)(object)buttonMinorIcon);
		((Control)panelBoxWeaveMinorIcon).Location = new Point(145, 41);
		((Control)panelBoxWeaveMinorIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)panelBoxWeaveMinorIcon).Name = "panelBoxWeaveMinorIcon";
		((Control)panelBoxWeaveMinorIcon).Size = new Size(123, 25);
		((Control)panelBoxWeaveMinorIcon).TabIndex = 0;
		((Control)buttonMinorIconRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonMinorIconRemove).Image = (Image)componentResourceManager.GetObject("buttonMinorIconRemove.Image");
		((Control)buttonMinorIconRemove).Location = new Point(76, 1);
		((Control)buttonMinorIconRemove).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonMinorIconRemove).Name = "buttonMinorIconRemove";
		((Control)buttonMinorIconRemove).Size = new Size(24, 23);
		((Control)buttonMinorIconRemove).TabIndex = 2;
		((Control)buttonMinorIconRemove).TabStop = false;
		((ButtonBase)buttonMinorIconRemove).UseVisualStyleBackColor = true;
		((Control)buttonMinorIconRemove).Click += ButtonMinorIconRemove_Click;
		((Control)buttonMinorIcon).Anchor = (AnchorStyles)9;
		((Control)buttonMinorIcon).Location = new Point(99, 1);
		((Control)buttonMinorIcon).Margin = new Padding(1, 1, 1, 1);
		((Control)buttonMinorIcon).Name = "buttonMinorIcon";
		((Control)buttonMinorIcon).Size = new Size(23, 23);
		((Control)buttonMinorIcon).TabIndex = 1;
		((Control)buttonMinorIcon).TabStop = false;
		((ButtonBase)buttonMinorIcon).UseVisualStyleBackColor = true;
		((Control)buttonMinorIcon).Click += ButtonMinorIcon_Click;
		((Label)labelWeaveMinorIcon).AutoEllipsis = true;
		((Control)labelWeaveMinorIcon).Location = new Point(0, 42);
		((Control)labelWeaveMinorIcon).Name = "labelWeaveMinorIcon";
		((Control)labelWeaveMinorIcon).Size = new Size(136, 22);
		((Control)labelWeaveMinorIcon).TabIndex = 34;
		((Control)labelWeaveMinorIcon).Text = "Icon";
		((Label)labelWeaveMinorIcon).TextAlign = (ContentAlignment)16;
		((Control)colorBoxWeaveMinorColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxWeaveMinorColor).Location = new Point(277, 41);
		((Control)colorBoxWeaveMinorColor).Margin = new Padding(2, 2, 2, 2);
		((Control)colorBoxWeaveMinorColor).Name = "colorBoxWeaveMinorColor";
		colorBoxWeaveMinorColor.SelectedColor = Color.Transparent;
		colorBoxWeaveMinorColor.ShowColorName = false;
		((Control)colorBoxWeaveMinorColor).Size = new Size(143, 25);
		((Control)colorBoxWeaveMinorColor).TabIndex = 1;
		colorBoxWeaveMinorColor.SelectedColorChanged += ColorBoxWeaveMinorColor_SelectedColorChanged;
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)labelWeaveMajorSizeHint);
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)labelWeaveMajorSize);
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)trackBarWeaveMajorSize);
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)panelBoxWeaveMajorIcon);
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)labelWeaveMajorIcon);
		((Control)groupBoxWeaveMajor).Controls.Add((Control)(object)colorBoxWeaveMajorColor);
		groupBoxWeaveMajor.HeaderHeight = 24;
		((Control)groupBoxWeaveMajor).Location = new Point(11, 38);
		((Control)groupBoxWeaveMajor).Name = "groupBoxWeaveMajor";
		((Control)groupBoxWeaveMajor).Size = new Size(420, 100);
		((Control)groupBoxWeaveMajor).TabIndex = 1;
		((Control)groupBoxWeaveMajor).TabStop = false;
		((Control)groupBoxWeaveMajor).Text = "Major";
		((Label)labelWeaveMajorSizeHint).AutoEllipsis = true;
		((Control)labelWeaveMajorSizeHint).Location = new Point(267, 79);
		((Control)labelWeaveMajorSizeHint).Name = "labelWeaveMajorSizeHint";
		((Control)labelWeaveMajorSizeHint).Size = new Size(27, 15);
		((Control)labelWeaveMajorSizeHint).TabIndex = 0;
		((Control)labelWeaveMajorSizeHint).Text = "0";
		((Label)labelWeaveMajorSizeHint).TextAlign = (ContentAlignment)64;
		((Label)labelWeaveMajorSizeHint).UseCompatibleTextRendering = true;
		((Label)labelWeaveMajorSize).AutoEllipsis = true;
		((Control)labelWeaveMajorSize).Location = new Point(0, 79);
		((Control)labelWeaveMajorSize).Name = "labelWeaveMajorSize";
		((Control)labelWeaveMajorSize).Size = new Size(136, 15);
		((Control)labelWeaveMajorSize).TabIndex = 36;
		((Control)labelWeaveMajorSize).Text = "Size";
		((Label)labelWeaveMajorSize).TextAlign = (ContentAlignment)16;
		((Control)trackBarWeaveMajorSize).AutoSize = false;
		((Control)trackBarWeaveMajorSize).Location = new Point(145, 80);
		((Control)trackBarWeaveMajorSize).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarWeaveMajorSize).Maximum = 30;
		((TrackBar)trackBarWeaveMajorSize).Minimum = 5;
		((Control)trackBarWeaveMajorSize).Name = "trackBarWeaveMajorSize";
		((Control)trackBarWeaveMajorSize).Size = new Size(123, 14);
		((Control)trackBarWeaveMajorSize).TabIndex = 2;
		((TrackBar)trackBarWeaveMajorSize).TickStyle = (TickStyle)0;
		((TrackBar)trackBarWeaveMajorSize).Value = 10;
		((TrackBar)trackBarWeaveMajorSize).ValueChanged += TrackBarWeaveMajorSize_ValueChanged;
		((Label)labelTextAngleHint).AutoEllipsis = true;
		((Control)labelTextAngleHint).Location = new Point(278, 147);
		((Control)labelTextAngleHint).Name = "labelTextAngleHint";
		((Control)labelTextAngleHint).Size = new Size(27, 15);
		((Control)labelTextAngleHint).TabIndex = 4;
		((Control)labelTextAngleHint).Text = "0";
		((Label)labelTextAngleHint).TextAlign = (ContentAlignment)64;
		((Label)labelTextAngleHint).UseCompatibleTextRendering = true;
		((Control)checkBoxTextEnabled).AutoSize = true;
		((Control)checkBoxTextEnabled).Location = new Point(156, 12);
		((Control)checkBoxTextEnabled).Name = "checkBoxTextEnabled";
		((Control)checkBoxTextEnabled).Size = new Size(65, 17);
		((Control)checkBoxTextEnabled).TabIndex = 0;
		((Control)checkBoxTextEnabled).Text = "Enabled";
		((ButtonBase)checkBoxTextEnabled).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxTextEnabled).CheckedChanged += CheckBoxTextEnabled_CheckedChanged;
		((Control)trackBarTextAngle).AutoSize = false;
		((Control)trackBarTextAngle).Location = new Point(156, 148);
		((Control)trackBarTextAngle).Margin = new Padding(2, 2, 2, 2);
		((TrackBar)trackBarTextAngle).Maximum = 360;
		((Control)trackBarTextAngle).Name = "trackBarTextAngle";
		((Control)trackBarTextAngle).Size = new Size(123, 14);
		((Control)trackBarTextAngle).TabIndex = 5;
		((TrackBar)trackBarTextAngle).TickStyle = (TickStyle)0;
		((TrackBar)trackBarTextAngle).ValueChanged += TrackBarTextAngle_ValueChanged;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((Control)TabControl).Controls.Add((Control)(object)pageText);
		((Control)TabControl).Controls.Add((Control)(object)pageImage);
		((Control)TabControl).Controls.Add((Control)(object)pageWeave);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(153, 0, 0, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(597, 354);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = true;
		TabControl.TitlePosition = StiTabTitlePosition.LeftHorizontal;
		TabControl.TitleWidth = 150;
		TabControl.Win11Look = true;
		((Control)pageText).Controls.Add((Control)(object)labelTextAngleHint);
		((Control)pageText).Controls.Add((Control)(object)checkBoxTextEnabled);
		((Control)pageText).Controls.Add((Control)(object)trackBarTextAngle);
		((Control)pageText).Controls.Add((Control)(object)textBoxText);
		((Control)pageText).Controls.Add((Control)(object)labelTextColor);
		((Control)pageText).Controls.Add((Control)(object)labelTextAngle);
		((Control)pageText).Controls.Add((Control)(object)colorBoxTextColor);
		((Control)pageText).Controls.Add((Control)(object)labelTextFont);
		((Control)pageText).Controls.Add((Control)(object)textBoxFont);
		((Control)pageText).Controls.Add((Control)(object)labelText);
		pageText.Dock = (DockStyle)5;
		pageText.InvisibleVisible = false;
		((Control)pageText).Location = new Point(153, 0);
		((Control)pageText).Margin = new Padding(2, 2, 2, 2);
		((Control)pageText).Name = "pageText";
		((Control)pageText).Size = new Size(444, 354);
		((Control)pageText).TabIndex = 0;
		((Control)pageText).Text = "Text";
		((Control)pageText).Visible = false;
		((Control)pageImage).Controls.Add((Control)(object)labelImageSizeHint);
		((Control)pageImage).Controls.Add((Control)(object)checkBoxImageEnabled);
		((Control)pageImage).Controls.Add((Control)(object)labelImageTransparencyHint);
		((Control)pageImage).Controls.Add((Control)(object)panelImage);
		((Control)pageImage).Controls.Add((Control)(object)panelHorAlignment);
		((Control)pageImage).Controls.Add((Control)(object)labelImageTransparency);
		((Control)pageImage).Controls.Add((Control)(object)checkBoxImageAspectRatio);
		((Control)pageImage).Controls.Add((Control)(object)labelImageMultipleFactor);
		((Control)pageImage).Controls.Add((Control)(object)checkBoxImageTiling);
		((Control)pageImage).Controls.Add((Control)(object)trackBarImageTransparency);
		((Control)pageImage).Controls.Add((Control)(object)trackBarImageSize);
		((Control)pageImage).Controls.Add((Control)(object)panelVerAlignment);
		((Control)pageImage).Controls.Add((Control)(object)checkBoxImageStretch);
		((Control)pageImage).Controls.Add((Control)(object)labelImageAlignment);
		pageImage.Dock = (DockStyle)5;
		pageImage.InvisibleVisible = false;
		((Control)pageImage).Location = new Point(153, 0);
		((Control)pageImage).Margin = new Padding(2, 2, 2, 2);
		((Control)pageImage).Name = "pageImage";
		((Control)pageImage).Size = new Size(444, 354);
		((Control)pageImage).TabIndex = 4;
		((Control)pageImage).Text = "Image";
		((Control)pageImage).Visible = false;
		((Control)pageWeave).Controls.Add((Control)(object)checkBoxWeaveEnabled);
		((Control)pageWeave).Controls.Add((Control)(object)stiGroupLine2);
		((Control)pageWeave).Controls.Add((Control)(object)groupBoxWeaveMajor);
		((Control)pageWeave).Controls.Add((Control)(object)groupBoxWeaveMinor);
		pageWeave.Dock = (DockStyle)5;
		pageWeave.InvisibleVisible = true;
		((Control)pageWeave).Location = new Point(153, 0);
		((Control)pageWeave).Margin = new Padding(2, 2, 2, 2);
		((Control)pageWeave).Name = "pageWeave";
		((Control)pageWeave).Size = new Size(444, 354);
		((Control)pageWeave).TabIndex = 1;
		((Control)pageWeave).Text = "Weave";
		((Control)pageWeave).Visible = false;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(597, 400);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(3, 3, 3, 3);
		((Control)this).Name = "StiWatermarkEditorForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Watermark";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Activated += This_Activated;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Control)panelVerAlignment).ResumeLayout(false);
		((Control)panelHorAlignment).ResumeLayout(false);
		((ISupportInitialize)trackBarImageSize).EndInit();
		((ISupportInitialize)trackBarImageTransparency).EndInit();
		((Control)panelImage).ResumeLayout(false);
		((Control)panelFooter).ResumeLayout(false);
		((Control)panelBoxWeaveMajorIcon).ResumeLayout(false);
		((ISupportInitialize)trackBarWeaveDistance).EndInit();
		((Control)stiGroupLine2).ResumeLayout(false);
		((ISupportInitialize)trackBarWeaveAngle).EndInit();
		((Control)groupBoxWeaveMinor).ResumeLayout(false);
		((ISupportInitialize)trackBarWeaveMinorSize).EndInit();
		((Control)panelBoxWeaveMinorIcon).ResumeLayout(false);
		((Control)groupBoxWeaveMajor).ResumeLayout(false);
		((ISupportInitialize)trackBarWeaveMajorSize).EndInit();
		((ISupportInitialize)trackBarTextAngle).EndInit();
		((Control)TabControl).ResumeLayout(false);
		((Control)pageText).ResumeLayout(false);
		((Control)pageText).PerformLayout();
		((Control)pageImage).ResumeLayout(false);
		((Control)pageImage).PerformLayout();
		((Control)pageWeave).ResumeLayout(false);
		((Control)pageWeave).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
