using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components.Design;

public class StiPageSetupForm : StiFormUX
{
	private bool isLessOptions = true;

	private bool textLock;

	private StiPage page;

	private PrinterSettings printerSetting = new PrinterSettings();

	private IStiDesignerBase designer;

	private Font watermarkFont;

	private StiBrush watermarkBrush;

	private bool isVariableGalleryActivated;

	private bool isResourceGalleryActivated;

	private byte[] watermarkImage;

	private string watermarkImageHyperlink;

	private IContainer components;

	private StiCloudTabulator tabControl;

	private StiCloudTabulatorPage pagePaper;

	private StiCloudTabulatorPage pageColumns;

	private StiCloudTabulatorPage pageWatermark;

	private StiButtonPanelUX footerPanel;

	private StiOffice2013GroupBox gbPaper;

	private StiOffice2013GroupBox gbOrientation;

	private StiComboBox cbSize;

	private StiTextBox tbHeight;

	private StiTextBox tbWidth;

	private StiOffice2013GroupBox gbPageMargins;

	private StiRadioButtonUX rbLandscape;

	private StiRadioButtonUX rbPortrait;

	private StiTextBox ebBottom;

	private StiTextBox ebTop;

	private StiTextBox ebLeft;

	private StiTextBox ebRight;

	private StiOffice2013GroupBox gbColumns;

	private StiTextBox textBoxColumnGaps;

	private StiTextBox textBoxColumnWidth;

	private StiNumericUpDown udColumns;

	private StiGroupPanelUX stiGroupLine1;

	private StiCheckBoxUX checkBoxRightToLeft;

	private Panel panelWatermarkControl;

	private StiOffice2013GroupBox gbText;

	private StiNumericUpDown numericUpDownWatermarkAngle;

	private TextBox textBoxWatermarkText;

	private StiWatermarkControl watermarkControl;

	private StiDotSeparatorVertical stiDotSeparatorVertical1;

	private StiBrushControl brushControl;

	private StiOffice2013GroupBox gbImage;

	private StiCheckBoxUX checkBoxWatermarkAspectRatio;

	private StiCheckBoxUX checkBoxWatermarkImageTiling;

	private StiCheckBoxUX checkBoxWatermarkImageStretch;

	private StiCheckBoxUX checkBoxWatermarkShowImageBehind;

	private StiGroupPanelUX stiGroupLine2;

	private StiTextBox textBoxWatermarkImageMultipleFactor;

	private StiComboBox comboBoxWatermarkImageAlignment;

	private Panel panelTextOptions2;

	private StiCheckBoxUX checkBoxWatermarkRightToLeft;

	private StiCheckBoxUX checkBoxWatermarkShowBehind;

	private StiGroupPanelUX watermarkSeparator1;

	private StiCheckBoxUX cbMirrorMargins;

	private StiComboBox comboBoxWatermarkEnabled;

	public StiButtonEdit textBoxWatermarkEnabledExpression;

	private Panel panelEnabled;

	private Panel panelImage;

	private Panel panelText;

	private StiGroupPanelUX stiGroupLine4;

	private Panel panelMoreOptions;

	private StiGroupPanelUX stiGroupLine5;

	private Panel panelImageOptions;

	private Panel panelTextOptions;

	private Panel panelSelectBrush;

	private ScrollableControl panelImageGallery;

	private ToolTip toolTip;

	private TrackBar trackBarWatermarkImageTransparency;

	private Timer timerMarker;

	private Panel panelElements;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX lbSize;

	private StiLabelUX lbHeight;

	private StiLabelUX lbWidth;

	private StiLabelUX lbBottom;

	private StiLabelUX lbTop;

	private StiLabelUX labelInfoUnit;

	private StiLabelUX lbLeft;

	private StiLabelUX lbRight;

	private StiLabelUX labelColumnGaps;

	private StiLabelUX labelColumnWidth;

	private StiLabelUX lbNumberColumns;

	private StiButtonUX buttonSelectFont;

	private StiLabelUX labelSelectColor;

	private StiLabelUX labelSelectFont;

	private StiLabelUX labelWatermarkAngle;

	private StiLabelUX labelWatermarkImageTransparency;

	private StiLabelUX labelWatermarkImageMultipleFactor;

	private StiLabelUX labelWatermarkImageAlignment;

	private StiLabelUX labelWatermarkEnabledExpression;

	private StiLabelUX labelWatermarkEnabled;

	private StiButtonUX buttonEditText;

	private StiButtonUX buttonImageRemove;

	private StiButtonUX buttonImageOpen;

	private StiButtonUX buttonMoreOptions;

	private StiLabelUX labelSpecification;

	public bool IsVariableFromGallery
	{
		get
		{
			string exp = GetVariableNameFromExpression();
			return GetVariables().FirstOrDefault((StiVariable v) => v.Name != null && v.Name == exp) != null;
		}
	}

	public bool IsResourceFromGallery
	{
		get
		{
			string resourceName = GetResourceNameFromHyperlink();
			return GetResources().FirstOrDefault((StiResource v) => v.Name != null && v.Name == resourceName) != null;
		}
	}

	public string Specification
	{
		get
		{
			return ((Control)labelSpecification).Text;
		}
		set
		{
			((Control)labelSpecification).Text = value;
			((Control)labelSpecification).Visible = !string.IsNullOrWhiteSpace(value);
		}
	}

	public byte[] WatermarkImage
	{
		get
		{
			return watermarkImage;
		}
		set
		{
			watermarkImage = value;
			watermarkControl.Image = StiImageConverter.BytesToImage(value);
			CheckStates();
			((Control)panelImage).Invalidate();
		}
	}

	public string WatermarkImageHyperlink
	{
		get
		{
			return watermarkImageHyperlink;
		}
		set
		{
			watermarkImageHyperlink = value;
			CheckStates();
			((Control)panelImage).Invalidate();
		}
	}

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(32.0);

	public StiPageSetupForm(StiPage page, IStiDesignerBase designer, int pageIndex)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Invalid comparison between Unknown and I4
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Invalid comparison between Unknown and I4
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected I4, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Invalid comparison between Unknown and I4
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Invalid comparison between Unknown and I4
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Invalid comparison between Unknown and I4
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Invalid comparison between Unknown and I4
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Invalid comparison between Unknown and I4
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Invalid comparison between Unknown and I4
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		((Form)this).ShowIcon = false;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.designer = designer;
		this.page = page;
		((RadioButton)rbPortrait).Checked = page.Orientation == StiPageOrientation.Portrait;
		((RadioButton)rbLandscape).Checked = page.Orientation == StiPageOrientation.Landscape;
		((Control)tbWidth).Text = page.PageWidth.ToString();
		((Control)tbHeight).Text = page.PageHeight.ToString();
		((Control)ebLeft).Text = page.Margins.Left.ToString();
		((Control)ebRight).Text = page.Margins.Right.ToString();
		((Control)ebTop).Text = page.Margins.Top.ToString();
		((Control)ebBottom).Text = page.Margins.Bottom.ToString();
		((CheckBox)cbMirrorMargins).Checked = page.MirrorMargins;
		udColumns.Value = page.Columns;
		((Control)textBoxColumnWidth).Text = page.ColumnWidth.ToString();
		((Control)textBoxColumnGaps).Text = page.ColumnGaps.ToString();
		((CheckBox)checkBoxRightToLeft).Checked = page.RightToLeft;
		((ComboBox)cbSize).Items.Clear();
		((Control)cbSize).Text = "Custom";
		Localize();
		DoScaling();
		float num;
		for (num = page.Watermark.Angle; num > 359f; num -= 360f)
		{
		}
		for (; num < -359f; num += 360f)
		{
		}
		numericUpDownWatermarkAngle.Value = (decimal)num;
		((Control)textBoxWatermarkText).Text = page.Watermark.Text;
		ref Font reference = ref watermarkFont;
		object obj = page.Watermark.Font.Clone();
		reference = (Font)((obj is Font) ? obj : null);
		watermarkBrush = page.Watermark.TextBrush.Clone() as StiBrush;
		watermarkControl.TextBrush = page.Watermark.TextBrush.Clone() as StiBrush;
		brushControl.Brush = watermarkBrush;
		((CheckBox)checkBoxWatermarkRightToLeft).Checked = page.Watermark.RightToLeft;
		((CheckBox)checkBoxWatermarkShowBehind).Checked = page.Watermark.ShowBehind;
		((CheckBox)checkBoxWatermarkShowImageBehind).Checked = page.Watermark.ShowImageBehind;
		((CheckBox)checkBoxWatermarkAspectRatio).Checked = page.Watermark.AspectRatio;
		((CheckBox)checkBoxWatermarkImageStretch).Checked = page.Watermark.ImageStretch;
		((CheckBox)checkBoxWatermarkImageTiling).Checked = page.Watermark.ImageTiling;
		((Control)textBoxWatermarkImageMultipleFactor).Text = page.Watermark.ImageMultipleFactor.ToString();
		trackBarWatermarkImageTransparency.Value = page.Watermark.ImageTransparency;
		((Control)watermarkControl).Font = watermarkFont;
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"));
		((ComboBox)comboBoxWatermarkImageAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"));
		ContentAlignment imageAlignment = page.Watermark.ImageAlignment;
		if ((int)imageAlignment <= 32)
		{
			switch (imageAlignment - 1)
			{
			default:
				if ((int)imageAlignment != 16)
				{
					if ((int)imageAlignment == 32)
					{
						((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter");
					}
				}
				else
				{
					((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft");
				}
				break;
			case 1:
				((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter");
				break;
			case 0:
				((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft");
				break;
			case 3:
				((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight");
				break;
			case 2:
				break;
			}
		}
		else if ((int)imageAlignment <= 256)
		{
			if ((int)imageAlignment != 64)
			{
				if ((int)imageAlignment == 256)
				{
					((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft");
				}
			}
			else
			{
				((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight");
			}
		}
		else if ((int)imageAlignment != 512)
		{
			if ((int)imageAlignment == 1024)
			{
				((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight");
			}
		}
		else
		{
			((Control)comboBoxWatermarkImageAlignment).Text = StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter");
		}
		if (!string.IsNullOrEmpty(this.page.Watermark.EnabledExpression))
		{
			((Control)textBoxWatermarkEnabledExpression).Text = this.page.Watermark.EnabledExpression;
			((ListControl)comboBoxWatermarkEnabled).SelectedIndex = 2;
		}
		else
		{
			((ListControl)comboBoxWatermarkEnabled).SelectedIndex = ((!this.page.Watermark.Enabled) ? 1 : 0);
		}
		ComboBoxWatermarkEnabled_SelectionChangeCommitted(null, null);
		WatermarkImage = page.Watermark.TakeImage();
		WatermarkImageHyperlink = page.Watermark.ImageHyperlink;
		if (page.Watermark.ExistImage())
		{
			WatermarkImage = page.Watermark.TakeImage();
		}
		else if (IsVariableFromGallery)
		{
			Specification = GetVariableNameFromExpression();
			isVariableGalleryActivated = true;
			WatermarkImage = GetVariableImage();
		}
		else if (IsResourceFromGallery)
		{
			Specification = GetResourceNameFromHyperlink();
			isResourceGalleryActivated = true;
			WatermarkImage = GetResourceImage();
		}
		if (!StiOptions.Designer.ShowWatermarkInPageSetup)
		{
			pageWatermark.Invisible = true;
		}
		tabControl.SelectedIndex = pageIndex;
		SetFormSize();
		if (StiUX.IsDark)
		{
			((Control)panelText).BackColor = StiUX.InputBackground;
			((Control)textBoxWatermarkText).BackColor = StiUX.InputBackground;
			((Control)textBoxWatermarkText).ForeColor = StiUX.InputForeground;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormPageSetup", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelWatermarkEnabled).Text = Loc.GetCleaned("PropertyMain", "Enabled");
		((Control)labelWatermarkEnabledExpression).Text = Loc.GetCleaned("PropertyMain", "Expression");
		((Control)gbPaper).Text = Loc.GetCleaned("FormPageSetup", "Paper");
		((Control)lbSize).Text = Loc.GetCleaned("FormPageSetup", "Size");
		((Control)lbWidth).Text = Loc.GetCleaned("FormPageSetup", "Width");
		((Control)lbHeight).Text = Loc.GetCleaned("FormPageSetup", "Height");
		((Control)gbOrientation).Text = Loc.GetCleaned("FormPageSetup", "Orientation");
		((Control)rbPortrait).Text = Loc.GetCleaned("FormPageSetup", "PageOrientationPortrait");
		((Control)rbLandscape).Text = Loc.GetCleaned("FormPageSetup", "PageOrientationLandscape");
		((Control)gbPageMargins).Text = Loc.GetCleaned("FormPageSetup", "Margins");
		((Control)lbLeft).Text = Loc.GetCleaned("FormPageSetup", "Left");
		((Control)lbRight).Text = Loc.GetCleaned("FormPageSetup", "Right");
		((Control)lbTop).Text = Loc.GetCleaned("FormPageSetup", "Top");
		((Control)lbBottom).Text = Loc.GetCleaned("FormPageSetup", "Bottom");
		((Control)cbMirrorMargins).Text = Loc.Get("PropertyMain", "MirrorMargins");
		((Control)gbColumns).Text = StiLocalization.Get("FormPageSetup", "Columns");
		((Control)lbNumberColumns).Text = Loc.GetCleaned("FormPageSetup", "NumberOfColumns");
		((Control)labelColumnWidth).Text = Loc.GetCleaned("FormPageSetup", "labelColumnWidth");
		((Control)labelColumnGaps).Text = Loc.GetCleaned("FormPageSetup", "labelColumnGaps");
		((Control)checkBoxRightToLeft).Text = Loc.Get("PropertyMain", "RightToLeft");
		((Control)gbText).Text = StiLocalization.Get("FormPageSetup", "labelText");
		((Control)labelWatermarkAngle).Text = Loc.GetCleaned("FormPageSetup", "labelAngle");
		((Control)labelSelectFont).Text = Loc.GetCleaned("FormPageSetup", "labelSelectFont");
		((Control)labelSelectColor).Text = Loc.GetCleaned("FormPageSetup", "labelSelectBrush");
		((Control)buttonSelectFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)checkBoxWatermarkRightToLeft).Text = StiLocalization.Get("PropertyMain", "RightToLeft");
		((Control)checkBoxWatermarkShowBehind).Text = StiLocalization.Get("PropertyMain", "ShowBehind");
		((Control)checkBoxWatermarkShowImageBehind).Text = StiLocalization.Get("PropertyMain", "ShowImageBehind");
		((Control)checkBoxWatermarkAspectRatio).Text = StiLocalization.Get("PropertyMain", "AspectRatio");
		((Control)checkBoxWatermarkImageStretch).Text = StiLocalization.Get("PropertyMain", "ImageStretch");
		((Control)checkBoxWatermarkImageTiling).Text = StiLocalization.Get("PropertyMain", "ImageTiling");
		((Control)labelWatermarkImageAlignment).Text = Loc.GetCleaned("FormPageSetup", "labelImageAlignment");
		((Control)labelWatermarkImageMultipleFactor).Text = Loc.GetCleaned("FormPageSetup", "labelMultipleFactor");
		((Control)labelWatermarkImageTransparency).Text = Loc.GetCleaned("FormPageSetup", "labelImageTransparency");
		((Control)pagePaper).Text = StiLocalization.Get("PropertyMain", "Paper");
		((Control)pageColumns).Text = StiLocalization.Get("PropertyMain", "Columns");
		((Control)pageWatermark).Text = StiLocalization.Get("PropertyMain", "Watermark");
		((Control)labelInfoUnit).Text = Loc.GetCleaned("FormPageSetup", "labelInfoUnit");
		((Control)gbColumns).Text = StiLocalization.Get("FormPageSetup", "groupColumns");
		((Control)gbImage).Text = StiLocalization.Get("FormPageSetup", "groupImage");
		((Control)gbPageMargins).Text = StiLocalization.Get("FormPageSetup", "groupMargins");
		((Control)gbOrientation).Text = StiLocalization.Get("FormPageSetup", "groupOrientation");
		((Control)gbPaper).Text = StiLocalization.Get("FormPageSetup", "groupPaper");
		((Control)gbText).Text = StiLocalization.Get("FormPageSetup", "groupText");
		((ComboBox)comboBoxWatermarkEnabled).Items.Clear();
		((ComboBox)comboBoxWatermarkEnabled).Items.Add((object)StiLocalization.Get("PropertyMain", "Enabled"));
		((ComboBox)comboBoxWatermarkEnabled).Items.Add((object)StiLocalization.Get("PropertyMain", "Disabled"));
		((ComboBox)comboBoxWatermarkEnabled).Items.Add((object)StiLocalization.Get("PropertyMain", "Expression"));
		((Control)buttonMoreOptions).Text = StiLocalization.Get("Buttons", "MoreOptions");
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = tabControl.DockPadding;
		int right = (tabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		((Control)watermarkControl).Size = new Size(StiScale.XXI(152.0), StiScale.YYI(212.0));
		pageColumns.Image = StiReportDesignImages.Tasks.PageOptionColumns(StiImageSize.Double);
		pagePaper.Image = StiReportDesignImages.Tasks.PageOptionPaper(StiImageSize.Double);
		pageWatermark.Image = StiReportDesignImages.Tasks.PageOptionWatermark(StiImageSize.Double);
		((ButtonBase)buttonEditText).Image = StiReportDesignImages.Actions.Edit();
		textBoxWatermarkEnabledExpression.Image = StiReportDesignImages.Actions.EditSmall();
		((ButtonBase)buttonImageOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonImageRemove).Image = StiReportDesignImages.Actions.Remove();
		StiTextBox stiTextBox = textBoxColumnWidth;
		right = (((Control)textBoxColumnGaps).Width = ((Control)udColumns).Width);
		((Control)stiTextBox).Width = right;
		((Control)buttonSelectFont).Left = ((Control)numericUpDownWatermarkAngle).Left;
		StiNumericUpDown stiNumericUpDown = numericUpDownWatermarkAngle;
		right = (((Control)buttonSelectFont).Width = brushControl.GeneralWidth);
		((Control)stiNumericUpDown).Width = right;
	}

	private void BuildGallery()
	{
		List<StiVariable> variables = GetVariables();
		List<StiResource> resources = GetResources();
		((Control)panelImageGallery).Visible = variables.Any() || resources.Any();
		((Control)panelImageGallery).Controls.Clear();
		MethodInvoker val = default(MethodInvoker);
		Task.Factory.StartNew(delegate
		{
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Expected O, but got Unknown
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
			//IL_02ae: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			string variableNameFromExpression = GetVariableNameFromExpression();
			string resourceNameFromHyperlink = GetResourceNameFromHyperlink();
			StiImageButton button2 = default(StiImageButton);
			foreach (StiVariable variable in variables)
			{
				try
				{
					ref StiImageButton reference = ref button2;
					StiPageSetupForm stiPageSetupForm = this;
					string name = variable.Name;
					object valueObject = variable.ValueObject;
					reference = stiPageSetupForm.CreateGalleryButton(name, (Image)((valueObject is Image) ? valueObject : null), variable, num, itemGalleryStep, ItemGallerySize);
					((CheckBox)button2).Checked = variableNameFromExpression == variable.Name;
					((CheckBox)button2).CheckedChanged += delegate(object sender, EventArgs e)
					{
						ImageGalleryClick(sender);
					};
					((CheckBox)button2).DoubleClick += This_OkClick;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button2, StiLocalization.Get("PropertyMain", "Variable") + ": " + variable.Name);
							((Control)panelImageGallery).Controls.Add((Control)(object)button2);
						}
					});
					num += ((Control)button2).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			foreach (StiResource resource in resources)
			{
				try
				{
					StiImageButton button = CreateGalleryButton(resource.Name, resource.Content, resource, num, itemGalleryStep, ItemGallerySize);
					((CheckBox)button).Checked = resourceNameFromHyperlink == resource.Name;
					((CheckBox)button).CheckedChanged += delegate(object sender, EventArgs e)
					{
						ImageGalleryClick(sender);
					};
					((CheckBox)button).DoubleClick += This_OkClick;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button, StiLocalization.Get("PropertyMain", "Resource") + ": " + resource.Name);
							((Control)panelImageGallery).Controls.Add((Control)(object)button);
						}
					});
					num += ((Control)button).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			StiPageSetupForm stiPageSetupForm2 = this;
			MethodInvoker obj3 = val;
			if (obj3 == null)
			{
				MethodInvoker val2 = delegate
				{
					if (!IsFormDestructed())
					{
						ScrollToActiveButton(panelImageGallery);
					}
				};
				MethodInvoker val3 = val2;
				val = val2;
				obj3 = val3;
			}
			((Control)stiPageSetupForm2).Invoke((Delegate)(object)obj3);
		});
	}

	private void ImageGalleryClick(object sender)
	{
		Specification = null;
		isResourceGalleryActivated = false;
		isVariableGalleryActivated = false;
		(from Control c in (IEnumerable)((Control)panelImageGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked)
		{
			WatermarkImage = StiImageConverter.ImageToBytes(((ButtonBase)stiImageButton).Image);
			if (((Control)stiImageButton).Tag is StiResource stiResource)
			{
				Specification = stiResource.Name;
				isResourceGalleryActivated = true;
				WatermarkImageHyperlink = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
			}
			if (((Control)stiImageButton).Tag is StiVariable stiVariable)
			{
				Specification = stiVariable.Name;
				isVariableGalleryActivated = true;
				WatermarkImageHyperlink = StiHyperlinkProcessor.CreateVariableName(stiVariable.Name);
			}
		}
	}

	private byte[] GetVariableImage()
	{
		return StiHyperlinkProcessor.GetBytes(page.Report, WatermarkImageHyperlink);
	}

	private byte[] GetResourceImage()
	{
		return StiHyperlinkProcessor.GetBytes(page.Report, WatermarkImageHyperlink);
	}

	private string GetVariableNameFromExpression()
	{
		return StiHyperlinkProcessor.GetVariableNameFromHyperlink(WatermarkImageHyperlink);
	}

	private string GetResourceNameFromHyperlink()
	{
		return StiHyperlinkProcessor.GetResourceNameFromHyperlink(WatermarkImageHyperlink);
	}

	private bool IsFormDestructed()
	{
		if (!((Control)this).IsDisposed && !((Control)this).Disposing)
		{
			return !((Control)this).IsHandleCreated;
		}
		return true;
	}

	private StiImageButton CreateGalleryButton(string name, byte[] image, object tag, int x, int y, int thumbWidth)
	{
		return CreateGalleryButton(name, StiImageConverter.TryBytesToImage(image), tag, x, y, thumbWidth);
	}

	private StiImageButton CreateGalleryButton(string name, Image image, object tag, int x, int y, int thumbWidth)
	{
		StiImageButton obj = new StiImageButton
		{
			ShowFooter = false
		};
		((ButtonBase)obj).Image = image;
		((Control)obj).Left = x;
		((Control)obj).Top = y;
		((Control)obj).Width = thumbWidth * 2;
		((Control)obj).Height = thumbWidth;
		((Control)obj).Text = name;
		((Control)obj).Tag = tag;
		return obj;
	}

	private void ScrollToActiveButton(ScrollableControl gallery)
	{
		Control val = ((IEnumerable)((Control)gallery).Controls).Cast<Control>().FirstOrDefault((Func<Control, bool>)((Control c) => c is StiImageButton && ((CheckBox)(StiImageButton)(object)c).Checked));
		if (val != null)
		{
			gallery.ScrollControlIntoView(val);
		}
	}

	private List<StiResource> GetResources()
	{
		return (from v in page.Report.Dictionary.Resources.ToList()
			where v.Type == StiResourceType.Image && v.Content != null
			select v).ToList();
	}

	private List<StiVariable> GetVariables()
	{
		return (from v in page.Report.Dictionary.Variables.ToList()
			where v.ValueObject is Image
			select v).ToList();
	}

	private void SetFormSize()
	{
		Screen val = Screen.FromControl((Control)(object)this);
		int val2 = ((Control)gbOrientation).Height + ((Control)gbPageMargins).Height + ((Control)gbPaper).Height + ((Control)footerPanel).Height + StiScale.I12;
		((Control)this).MaximumSize = new Size(val.WorkingArea.Width - 40, val.WorkingArea.Height - 40);
		if (isLessOptions)
		{
			int val3 = GetImageGalleryHeight() + StiScale.I(505);
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, Math.Max(val3, val2));
		}
		else
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, ((Control)panelEnabled).Height + ((Control)gbText).Height + ((Control)panelTextOptions).Height + ((Control)panelSelectBrush).Height + ((Control)panelTextOptions2).Height + ((Control)gbImage).Height + ((Control)panelImageOptions).Height + ((Control)panelMoreOptions).Height + GetImageGalleryHeight() + ((Control)footerPanel).Height + StiScale.I8);
		}
		((Control)this).Top = (Screen.FromControl((Control)(object)this).WorkingArea.Height - ((Control)this).Height) / 2;
	}

	private int GetImageGalleryHeight()
	{
		if (!((Control)panelImageGallery).Visible)
		{
			return 0;
		}
		return ((Control)panelImageGallery).Height;
	}

	internal void CheckStates()
	{
		((Control)buttonImageRemove).Enabled = WatermarkImage != null;
	}

	private void BuildListOfPageSizes()
	{
		Task.Factory.StartNew(delegate
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			int pageWidth = (int)Math.Round(page.Unit.ConvertToHInches(page.PageWidth), 0);
			int pageHeight = (int)Math.Round(page.Unit.ConvertToHInches(page.PageHeight), 0);
			textLock = true;
			string paperSize = string.Empty;
			PaperSizeCollection paperSizes = StiOptions.Print.CustomPaperSizes ?? printerSetting.PaperSizes;
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Expected O, but got Unknown
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				((ComboBox)cbSize).Items.Add((object)StiLocalization.Get("Wizards", "Custom"));
				int i = StiScale.I10;
				foreach (PaperSize item in paperSizes)
				{
					PaperSize val = item;
					if ((int)page.PaperSize == 0)
					{
						if (((Math.Abs(val.Width - pageWidth) < i && Math.Abs(val.Height - pageHeight) < i) || (Math.Abs(val.Height - pageWidth) < i && Math.Abs(val.Width - pageHeight) < i)) && paperSize.Length == 0)
						{
							paperSize = val.PaperName;
						}
					}
					else if (page.PaperSize == val.Kind)
					{
						paperSize = val.PaperName;
					}
					if ((int)val.Kind != 0)
					{
						((ComboBox)cbSize).Items.Add((object)val.PaperName);
					}
				}
				if ((int)page.PaperSize == 0)
				{
					paperSize = StiLocalization.Get("Wizards", "Custom");
				}
				((Control)cbSize).Text = paperSize;
				if (((Control)cbSize).Text.Length == 0)
				{
					((ListControl)cbSize).SelectedIndex = ((ComboBox)cbSize).Items.Count - 1;
				}
				textLock = false;
				((Control)cbSize).Enabled = true;
			});
		});
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		designer.UndoRedoSave(((Control)this).Text, onlyCurrentPage: true);
		page.Orientation = ((!((RadioButton)rbPortrait).Checked) ? StiPageOrientation.Landscape : StiPageOrientation.Portrait);
		try
		{
			page.PageWidth = double.Parse(((Control)tbWidth).Text);
			page.PageHeight = double.Parse(((Control)tbHeight).Text);
		}
		catch
		{
		}
		try
		{
			page.Margins = new StiMargins(double.Parse(((Control)ebLeft).Text), double.Parse(((Control)ebRight).Text), double.Parse(((Control)ebTop).Text), double.Parse(((Control)ebBottom).Text));
			page.MirrorMargins = ((CheckBox)cbMirrorMargins).Checked;
		}
		catch
		{
		}
		try
		{
			page.Columns = (int)udColumns.Value;
			page.ColumnWidth = double.Parse(((Control)textBoxColumnWidth).Text);
			page.ColumnGaps = double.Parse(((Control)textBoxColumnGaps).Text);
			page.RightToLeft = ((CheckBox)checkBoxRightToLeft).Checked;
		}
		catch
		{
		}
		try
		{
			double.Parse(((Control)textBoxWatermarkImageMultipleFactor).Text);
		}
		catch
		{
			((Control)textBoxWatermarkImageMultipleFactor).Text = page.Watermark.ImageMultipleFactor.ToString();
		}
		page.Watermark.ImageHyperlink = "";
		page.Watermark.ResetImage();
		if (isVariableGalleryActivated || isResourceGalleryActivated)
		{
			page.Watermark.ImageHyperlink = WatermarkImageHyperlink;
		}
		else
		{
			page.Watermark.PutImage(WatermarkImage);
		}
		page.Watermark.Text = ((Control)textBoxWatermarkText).Text;
		page.Watermark.Angle = (float)numericUpDownWatermarkAngle.Value;
		page.Watermark.Font = watermarkFont;
		page.Watermark.TextBrush = watermarkBrush;
		page.Watermark.RightToLeft = ((CheckBox)checkBoxWatermarkRightToLeft).Checked;
		page.Watermark.ShowBehind = ((CheckBox)checkBoxWatermarkShowBehind).Checked;
		page.Watermark.ShowImageBehind = ((CheckBox)checkBoxWatermarkShowImageBehind).Checked;
		page.Watermark.AspectRatio = ((CheckBox)checkBoxWatermarkAspectRatio).Checked;
		page.Watermark.ImageStretch = ((CheckBox)checkBoxWatermarkImageStretch).Checked;
		page.Watermark.ImageTiling = ((CheckBox)checkBoxWatermarkImageTiling).Checked;
		page.Watermark.ImageMultipleFactor = double.Parse(((Control)textBoxWatermarkImageMultipleFactor).Text);
		page.Watermark.ImageTransparency = trackBarWatermarkImageTransparency.Value;
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)512;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)256;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)1024;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)32;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)16;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)64;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)2;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)1;
		}
		if (((Control)comboBoxWatermarkImageAlignment).Text == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"))
		{
			page.Watermark.ImageAlignment = (ContentAlignment)4;
		}
		switch (((ListControl)comboBoxWatermarkEnabled).SelectedIndex)
		{
		case 0:
			page.Watermark.EnabledExpression = string.Empty;
			page.Watermark.Enabled = true;
			break;
		case 1:
			page.Watermark.EnabledExpression = string.Empty;
			page.Watermark.Enabled = false;
			break;
		case 2:
			page.Watermark.EnabledExpression = ((Control)textBoxWatermarkEnabledExpression).Text;
			page.Watermark.Enabled = true;
			break;
		}
		PaperSizeCollection val = StiOptions.Print.CustomPaperSizes ?? printerSetting.PaperSizes;
		if (((ListControl)cbSize).SelectedIndex == 0)
		{
			page.PaperSize = (PaperKind)0;
		}
		else
		{
			foreach (PaperSize item in val)
			{
				PaperSize val2 = item;
				if (val2.PaperName == ((Control)cbSize).Text)
				{
					page.PaperSize = val2.Kind;
				}
			}
		}
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_watermarks_watermark_property.htm");
	}

	private void This_Load(object sender, EventArgs e)
	{
		ScrollToActiveButton(panelImageGallery);
		BuildGallery();
		BuildListOfPageSizes();
	}

	private void This_Activated(object sender, EventArgs e)
	{
		if (tabControl.SelectedIndex == 2)
		{
			((Control)textBoxWatermarkText).Focus();
		}
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		timerMarker.Stop();
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		try
		{
			pageWatermark.ShowMarker = !string.IsNullOrWhiteSpace(((Control)textBoxWatermarkText).Text) || !string.IsNullOrWhiteSpace(WatermarkImageHyperlink) || WatermarkImage != null;
			pageColumns.ShowMarker = udColumns.Value > 0m;
		}
		catch
		{
		}
	}

	private void BrushControl_BrushChanged(object sender, EventArgs e)
	{
		watermarkBrush = brushControl.Brush;
		watermarkControl.TextBrush = brushControl.Brush;
	}

	private void ComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		if (((ListControl)cbSize).SelectedIndex == 0 || textLock)
		{
			return;
		}
		textLock = true;
		PaperSizeCollection val = StiOptions.Print.CustomPaperSizes ?? printerSetting.PaperSizes;
		PaperSize val2 = null;
		foreach (PaperSize item in val)
		{
			PaperSize val3 = item;
			if (val3.PaperName == ((Control)cbSize).Text)
			{
				val2 = val3;
				break;
			}
		}
		double value = val2.Width;
		double value2 = val2.Height;
		if (page.Unit is StiCentimetersUnit || page.Unit is StiMillimetersUnit)
		{
			value = PrinterUnitConvert.Convert(val2.Width, (PrinterUnit)1, (PrinterUnit)3);
			value2 = PrinterUnitConvert.Convert(val2.Height, (PrinterUnit)1, (PrinterUnit)3);
			if (page.Unit is StiCentimetersUnit)
			{
				value /= (double)StiScale.I10;
				value2 /= (double)StiScale.I10;
			}
		}
		else
		{
			value = Math.Round(page.ConvertFromHInches(page.Unit, value), 2);
			value2 = Math.Round(page.ConvertFromHInches(page.Unit, value2), 2);
		}
		if (((RadioButton)rbLandscape).Checked)
		{
			((Control)tbHeight).Text = value.ToString();
			((Control)tbWidth).Text = value2.ToString();
		}
		else
		{
			((Control)tbWidth).Text = value.ToString();
			((Control)tbHeight).Text = value2.ToString();
		}
		textLock = false;
	}

	private void RadioButtonPortrait_CheckedChanged(object sender, EventArgs e)
	{
		string text = ((Control)ebLeft).Text;
		string text2 = ((Control)ebRight).Text;
		string text3 = ((Control)ebTop).Text;
		string text4 = ((Control)ebBottom).Text;
		string text5 = ((Control)tbHeight).Text;
		((Control)tbHeight).Text = ((Control)tbWidth).Text;
		((Control)tbWidth).Text = text5;
		if (((RadioButton)rbLandscape).Checked)
		{
			((Control)ebLeft).Text = text3;
			((Control)ebRight).Text = text4;
			((Control)ebTop).Text = text2;
			((Control)ebBottom).Text = text;
		}
		else
		{
			((Control)ebLeft).Text = text4;
			((Control)ebRight).Text = text3;
			((Control)ebTop).Text = text;
			((Control)ebBottom).Text = text2;
		}
	}

	private void ButtonSelectFont_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(watermarkFont, page.Report);
		try
		{
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				((Control)watermarkControl).Font = stiFontEditorForm.SelectedFont;
				watermarkFont = stiFontEditorForm.SelectedFont;
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void ComboBoxWatermarkImageAlignment_SelectedValueChanged(object sender, EventArgs e)
	{
		watermarkControl.ImageAlignment = ((ComboBox)comboBoxWatermarkImageAlignment).SelectedItem.ToString();
		((Control)watermarkControl).Invalidate();
	}

	private void TextBoxWatermarkText_TextChanged(object sender, EventArgs e)
	{
		watermarkControl.TextValue = ((Control)textBoxWatermarkText).Text;
	}

	private void NumericUpDownWatermarkAngle_ValueChanged(object sender, EventArgs e)
	{
		watermarkControl.TextAngle = (double)numericUpDownWatermarkAngle.Value;
	}

	private void CheckBoxWatermarkShowBehind_CheckedChanged(object sender, EventArgs e)
	{
		watermarkControl.TextBehind = ((CheckBox)checkBoxWatermarkShowBehind).Checked;
	}

	private void CheckBoxWatermarkShowImageBehind_CheckedChanged(object sender, EventArgs e)
	{
		watermarkControl.ImageBehind = ((CheckBox)checkBoxWatermarkShowImageBehind).Checked;
	}

	private void CheckBoxWatermarkImageStretch_CheckedChanged(object sender, EventArgs e)
	{
		watermarkControl.ImageStretch = ((CheckBox)checkBoxWatermarkImageStretch).Checked;
	}

	private void CheckBoxWatermarkImageTiling_CheckedChanged(object sender, EventArgs e)
	{
		watermarkControl.ImageTiling = ((CheckBox)checkBoxWatermarkImageTiling).Checked;
	}

	private void BrushControl_ClientSizeChanged(object sender, EventArgs e)
	{
		((Control)brushControl).Width = ((Control)panelSelectBrush).Width - ((Control)brushControl).Left;
		((Control)panelSelectBrush).Height = ((Control)brushControl).Height + StiScale.XXI(20.0);
		SetFormSize();
	}

	private void ComboBoxWatermarkEnabled_SelectionChangeCommitted(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxWatermarkEnabled).SelectedIndex == 2)
		{
			((Control)labelWatermarkEnabledExpression).Visible = true;
			((Control)textBoxWatermarkEnabledExpression).Visible = true;
			((Control)panelEnabled).Height = StiScale.XXI(82.0);
		}
		else
		{
			((Control)labelWatermarkEnabledExpression).Visible = false;
			((Control)textBoxWatermarkEnabledExpression).Visible = false;
			((Control)panelEnabled).Height = StiScale.XXI(45.0);
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
					WatermarkImage = StiImageHelper.BytesFromFile(((FileDialog)val).FileName);
					WatermarkImageHyperlink = null;
					Specification = null;
					isVariableGalleryActivated = false;
					isResourceGalleryActivated = false;
					(from Control c in (IEnumerable)((Control)panelImageGallery).Controls
						where c is StiImageButton
						select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
					{
						((CheckBox)c).Checked = false;
					});
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
		WatermarkImage = null;
		WatermarkImageHyperlink = null;
		Specification = null;
		isVariableGalleryActivated = false;
		isResourceGalleryActivated = false;
		(from Control c in (IEnumerable)((Control)panelImageGallery).Controls
			where c is StiImageButton
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
	}

	private void PanelText_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelText, e.Graphics, drawDragDrop: false, drawBorder: true, isImage: false, drawBackground: false);
	}

	private void ButtonEditText_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxWatermarkText).Text;
		text = StiExpressionPacker.PackExpression(text, page.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(page.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxWatermarkText).Text = StiExpressionPacker.UnPackExpression(text, page.Report.Designer, useBraces: false);
		}
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, WatermarkImage, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void TextBoxWatermarkEnabledExpression_ButtonClick(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxWatermarkEnabledExpression).Text;
		text = StiExpressionPacker.PackExpression(text, page.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(page.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxWatermarkEnabledExpression).Text = StiExpressionPacker.UnPackExpression(text, page.Report.Designer, useBraces: false);
		}
	}

	private void ButtonMoreOptions_Click(object sender, EventArgs e)
	{
		((Control)panelEnabled).Visible = !((Control)panelEnabled).Visible;
		((Control)panelTextOptions).Visible = !((Control)panelTextOptions).Visible;
		((Control)panelSelectBrush).Visible = !((Control)panelSelectBrush).Visible;
		((Control)panelTextOptions2).Visible = !((Control)panelTextOptions2).Visible;
		((Control)panelImageOptions).Visible = !((Control)panelImageOptions).Visible;
		isLessOptions = !isLessOptions;
		((Control)buttonMoreOptions).Text = (isLessOptions ? StiLocalization.Get("Buttons", "MoreOptions") : StiLocalization.Get("Buttons", "LessOptions"));
		SetFormSize();
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl.SelectedTab == pageWatermark)
		{
			((Control)textBoxWatermarkText).Focus();
		}
	}

	private void ComboBoxWatermarkEnabled_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetFormSize();
	}

	private void TrackBarWatermarkImageTransparency_ValueChanged(object sender, EventArgs e)
	{
		watermarkControl.ImageTransparency = trackBarWatermarkImageTransparency.Value;
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
					WatermarkImage = StiImageHelper.TryBytesFromFile(item);
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
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1233: Unknown result type (might be due to invalid IL or missing references)
		//IL_123d: Expected O, but got Unknown
		//IL_1275: Unknown result type (might be due to invalid IL or missing references)
		//IL_170e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1718: Expected O, but got Unknown
		//IL_1750: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c2: Expected O, but got Unknown
		//IL_22cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d9: Expected O, but got Unknown
		//IL_22e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f0: Expected O, but got Unknown
		//IL_2325: Unknown result type (might be due to invalid IL or missing references)
		//IL_232f: Expected O, but got Unknown
		//IL_23b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c0: Expected O, but got Unknown
		//IL_247c: Unknown result type (might be due to invalid IL or missing references)
		//IL_281f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2840: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdd: Expected O, but got Unknown
		//IL_2cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d05: Expected O, but got Unknown
		//IL_2edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee7: Expected O, but got Unknown
		//IL_2ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f02: Expected O, but got Unknown
		//IL_31e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3462: Unknown result type (might be due to invalid IL or missing references)
		//IL_346c: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiPageSetupForm));
		toolTip = new ToolTip(components);
		timerMarker = new Timer(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tabControl = new StiCloudTabulator();
		pagePaper = new StiCloudTabulatorPage();
		gbPageMargins = new StiOffice2013GroupBox();
		cbMirrorMargins = new StiCheckBoxUX();
		ebBottom = new StiTextBox();
		lbBottom = new StiLabelUX();
		ebTop = new StiTextBox();
		lbTop = new StiLabelUX();
		ebLeft = new StiTextBox();
		labelInfoUnit = new StiLabelUX();
		lbLeft = new StiLabelUX();
		lbRight = new StiLabelUX();
		ebRight = new StiTextBox();
		gbOrientation = new StiOffice2013GroupBox();
		rbLandscape = new StiRadioButtonUX();
		rbPortrait = new StiRadioButtonUX();
		gbPaper = new StiOffice2013GroupBox();
		lbSize = new StiLabelUX();
		cbSize = new StiComboBox();
		tbHeight = new StiTextBox();
		lbHeight = new StiLabelUX();
		lbWidth = new StiLabelUX();
		tbWidth = new StiTextBox();
		pageColumns = new StiCloudTabulatorPage();
		checkBoxRightToLeft = new StiCheckBoxUX();
		stiGroupLine1 = new StiGroupPanelUX();
		gbColumns = new StiOffice2013GroupBox();
		textBoxColumnGaps = new StiTextBox();
		labelColumnGaps = new StiLabelUX();
		textBoxColumnWidth = new StiTextBox();
		labelColumnWidth = new StiLabelUX();
		lbNumberColumns = new StiLabelUX();
		udColumns = new StiNumericUpDown();
		pageWatermark = new StiCloudTabulatorPage();
		panelElements = new Panel();
		panelMoreOptions = new Panel();
		buttonMoreOptions = new StiButtonUX();
		stiGroupLine5 = new StiGroupPanelUX();
		panelImageOptions = new Panel();
		trackBarWatermarkImageTransparency = new TrackBar();
		comboBoxWatermarkImageAlignment = new StiComboBox();
		labelWatermarkImageAlignment = new StiLabelUX();
		labelWatermarkImageMultipleFactor = new StiLabelUX();
		checkBoxWatermarkAspectRatio = new StiCheckBoxUX();
		textBoxWatermarkImageMultipleFactor = new StiTextBox();
		checkBoxWatermarkImageTiling = new StiCheckBoxUX();
		checkBoxWatermarkImageStretch = new StiCheckBoxUX();
		labelWatermarkImageTransparency = new StiLabelUX();
		checkBoxWatermarkShowImageBehind = new StiCheckBoxUX();
		stiGroupLine2 = new StiGroupPanelUX();
		panelImageGallery = new ScrollableControl();
		gbImage = new StiOffice2013GroupBox();
		panelImage = new Panel();
		buttonImageOpen = new StiButtonUX();
		buttonImageRemove = new StiButtonUX();
		labelSpecification = new StiLabelUX();
		panelTextOptions2 = new Panel();
		checkBoxWatermarkRightToLeft = new StiCheckBoxUX();
		checkBoxWatermarkShowBehind = new StiCheckBoxUX();
		watermarkSeparator1 = new StiGroupPanelUX();
		panelSelectBrush = new Panel();
		stiGroupLine4 = new StiGroupPanelUX();
		brushControl = new StiBrushControl();
		labelSelectColor = new StiLabelUX();
		panelTextOptions = new Panel();
		numericUpDownWatermarkAngle = new StiNumericUpDown();
		buttonSelectFont = new StiButtonUX();
		labelWatermarkAngle = new StiLabelUX();
		labelSelectFont = new StiLabelUX();
		gbText = new StiOffice2013GroupBox();
		panelText = new Panel();
		buttonEditText = new StiButtonUX();
		textBoxWatermarkText = new TextBox();
		panelEnabled = new Panel();
		textBoxWatermarkEnabledExpression = new StiButtonEdit();
		comboBoxWatermarkEnabled = new StiComboBox();
		labelWatermarkEnabledExpression = new StiLabelUX();
		labelWatermarkEnabled = new StiLabelUX();
		panelWatermarkControl = new Panel();
		watermarkControl = new StiWatermarkControl();
		stiDotSeparatorVertical1 = new StiDotSeparatorVertical();
		footerPanel = new StiButtonPanelUX();
		((Control)tabControl).SuspendLayout();
		((Control)pagePaper).SuspendLayout();
		((Control)gbPageMargins).SuspendLayout();
		((Control)gbOrientation).SuspendLayout();
		((Control)gbPaper).SuspendLayout();
		((Control)pageColumns).SuspendLayout();
		((Control)gbColumns).SuspendLayout();
		((ISupportInitialize)udColumns).BeginInit();
		((Control)pageWatermark).SuspendLayout();
		((Control)panelElements).SuspendLayout();
		((Control)panelMoreOptions).SuspendLayout();
		((Control)panelImageOptions).SuspendLayout();
		((ISupportInitialize)trackBarWatermarkImageTransparency).BeginInit();
		((Control)gbImage).SuspendLayout();
		((Control)panelImage).SuspendLayout();
		((Control)panelTextOptions2).SuspendLayout();
		((Control)panelSelectBrush).SuspendLayout();
		((Control)panelTextOptions).SuspendLayout();
		((ISupportInitialize)numericUpDownWatermarkAngle).BeginInit();
		((Control)gbText).SuspendLayout();
		((Control)panelText).SuspendLayout();
		((Control)panelEnabled).SuspendLayout();
		((Control)panelWatermarkControl).SuspendLayout();
		((Control)footerPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(534, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 2;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(615, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 3;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tabControl).Controls.Add((Control)(object)pagePaper);
		((Control)tabControl).Controls.Add((Control)(object)pageColumns);
		((Control)tabControl).Controls.Add((Control)(object)pageWatermark);
		((Control)tabControl).Dock = (DockStyle)5;
		((Control)tabControl).Location = new Point(0, 0);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 0, 119, 0);
		tabControl.ScrollHeight = 0;
		tabControl.ScrollWidth = 100;
		((Control)tabControl).Size = new Size(702, 488);
		((Control)tabControl).TabIndex = 0;
		tabControl.TitleBackColor = SystemColors.Control;
		tabControl.TitleInLine = false;
		tabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		tabControl.TitleWidth = 110;
		tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pagePaper).Controls.Add((Control)(object)gbPageMargins);
		((Control)pagePaper).Controls.Add((Control)(object)gbOrientation);
		((Control)pagePaper).Controls.Add((Control)(object)gbPaper);
		pagePaper.Dock = (DockStyle)5;
		pagePaper.Image = (Image)componentResourceManager.GetObject("pagePaper.Image");
		pagePaper.InvisibleVisible = false;
		((Control)pagePaper).Location = new Point(0, 0);
		((Control)pagePaper).Name = "pagePaper";
		((Control)pagePaper).Padding = new Padding(0, 0, 1, 0);
		((Control)pagePaper).Size = new Size(583, 488);
		((Control)pagePaper).TabIndex = 0;
		((Control)pagePaper).Text = "Paper";
		((Control)pagePaper).Visible = false;
		((Control)gbPageMargins).Controls.Add((Control)(object)cbMirrorMargins);
		((Control)gbPageMargins).Controls.Add((Control)(object)ebBottom);
		((Control)gbPageMargins).Controls.Add((Control)(object)lbBottom);
		((Control)gbPageMargins).Controls.Add((Control)(object)ebTop);
		((Control)gbPageMargins).Controls.Add((Control)(object)lbTop);
		((Control)gbPageMargins).Controls.Add((Control)(object)ebLeft);
		((Control)gbPageMargins).Controls.Add((Control)(object)labelInfoUnit);
		((Control)gbPageMargins).Controls.Add((Control)(object)lbLeft);
		((Control)gbPageMargins).Controls.Add((Control)(object)lbRight);
		((Control)gbPageMargins).Controls.Add((Control)(object)ebRight);
		((Control)gbPageMargins).Dock = (DockStyle)1;
		gbPageMargins.HeaderHeight = 24;
		((Control)gbPageMargins).Location = new Point(0, 231);
		((Control)gbPageMargins).Name = "gbPageMargins";
		((Control)gbPageMargins).Size = new Size(582, 228);
		((Control)gbPageMargins).TabIndex = 2;
		((Control)gbPageMargins).TabStop = false;
		((Control)gbPageMargins).Text = "Page Margins";
		((Control)cbMirrorMargins).Location = new Point(109, 186);
		((Control)cbMirrorMargins).Name = "cbMirrorMargins";
		((Control)cbMirrorMargins).Size = new Size(240, 24);
		((Control)cbMirrorMargins).TabIndex = 4;
		((Control)cbMirrorMargins).Text = "Mirror Margins";
		((Control)ebBottom).Location = new Point(109, 157);
		((Control)ebBottom).Name = "ebBottom";
		((Control)ebBottom).Size = new Size(71, 21);
		((Control)ebBottom).TabIndex = 3;
		ebBottom.TextAlign = (HorizontalAlignment)1;
		((Control)lbBottom).Location = new Point(12, 157);
		((Control)lbBottom).Name = "lbBottom";
		((Control)lbBottom).Size = new Size(88, 20);
		((Control)lbBottom).TabIndex = 31;
		((Control)lbBottom).Text = "Bottom";
		((Label)lbBottom).TextAlign = (ContentAlignment)16;
		((Control)ebTop).Location = new Point(109, 125);
		((Control)ebTop).Name = "ebTop";
		((Control)ebTop).Size = new Size(71, 21);
		((Control)ebTop).TabIndex = 2;
		ebTop.TextAlign = (HorizontalAlignment)1;
		((Control)lbTop).Location = new Point(12, 125);
		((Control)lbTop).Name = "lbTop";
		((Control)lbTop).Size = new Size(88, 20);
		((Control)lbTop).TabIndex = 30;
		((Control)lbTop).Text = "Top";
		((Label)lbTop).TextAlign = (ContentAlignment)16;
		((Control)ebLeft).Location = new Point(109, 61);
		((Control)ebLeft).Name = "ebLeft";
		((Control)ebLeft).Size = new Size(71, 21);
		((Control)ebLeft).TabIndex = 0;
		ebLeft.TextAlign = (HorizontalAlignment)1;
		((Control)labelInfoUnit).Location = new Point(11, 36);
		((Control)labelInfoUnit).Name = "labelInfoUnit";
		((Control)labelInfoUnit).Size = new Size(369, 22);
		((Control)labelInfoUnit).TabIndex = 28;
		((Control)labelInfoUnit).Text = "Page margins are specified in the current units";
		((Label)labelInfoUnit).TextAlign = (ContentAlignment)16;
		((Control)lbLeft).Location = new Point(12, 61);
		((Control)lbLeft).Name = "lbLeft";
		((Control)lbLeft).Size = new Size(88, 20);
		((Control)lbLeft).TabIndex = 27;
		((Control)lbLeft).Text = "Left";
		((Label)lbLeft).TextAlign = (ContentAlignment)16;
		((Control)lbRight).Location = new Point(12, 93);
		((Control)lbRight).Name = "lbRight";
		((Control)lbRight).Size = new Size(88, 20);
		((Control)lbRight).TabIndex = 29;
		((Control)lbRight).Text = "Right";
		((Label)lbRight).TextAlign = (ContentAlignment)16;
		((Control)ebRight).Location = new Point(109, 93);
		((Control)ebRight).Name = "ebRight";
		((Control)ebRight).Size = new Size(71, 21);
		((Control)ebRight).TabIndex = 1;
		ebRight.TextAlign = (HorizontalAlignment)1;
		((Control)gbOrientation).Controls.Add((Control)(object)rbLandscape);
		((Control)gbOrientation).Controls.Add((Control)(object)rbPortrait);
		((Control)gbOrientation).Dock = (DockStyle)1;
		gbOrientation.HeaderHeight = 24;
		((Control)gbOrientation).Location = new Point(0, 134);
		((Control)gbOrientation).Name = "gbOrientation";
		((Control)gbOrientation).Size = new Size(582, 97);
		((Control)gbOrientation).TabIndex = 1;
		((Control)gbOrientation).TabStop = false;
		((Control)gbOrientation).Text = "Paper orientation";
		((Control)rbLandscape).Location = new Point(109, 61);
		((Control)rbLandscape).Name = "rbLandscape";
		((Control)rbLandscape).Size = new Size(353, 24);
		((Control)rbLandscape).TabIndex = 1;
		((Control)rbLandscape).Text = "Landscape";
		((Control)rbPortrait).Location = new Point(109, 35);
		((Control)rbPortrait).Name = "rbPortrait";
		((Control)rbPortrait).Size = new Size(353, 21);
		((Control)rbPortrait).TabIndex = 0;
		((Control)rbPortrait).Text = "Portrait";
		((RadioButton)rbPortrait).CheckedChanged += RadioButtonPortrait_CheckedChanged;
		((Control)gbPaper).Controls.Add((Control)(object)lbSize);
		((Control)gbPaper).Controls.Add((Control)(object)cbSize);
		((Control)gbPaper).Controls.Add((Control)(object)tbHeight);
		((Control)gbPaper).Controls.Add((Control)(object)lbHeight);
		((Control)gbPaper).Controls.Add((Control)(object)lbWidth);
		((Control)gbPaper).Controls.Add((Control)(object)tbWidth);
		((Control)gbPaper).Dock = (DockStyle)1;
		gbPaper.HeaderHeight = 24;
		((Control)gbPaper).Location = new Point(0, 0);
		((Control)gbPaper).Name = "gbPaper";
		((Control)gbPaper).Size = new Size(582, 134);
		((Control)gbPaper).TabIndex = 0;
		((Control)gbPaper).TabStop = false;
		((Control)gbPaper).Text = "Paper Size";
		((Control)lbSize).Location = new Point(12, 36);
		((Control)lbSize).Name = "lbSize";
		((Control)lbSize).Size = new Size(91, 21);
		((Control)lbSize).TabIndex = 9;
		((Control)lbSize).Text = "Size";
		((Label)lbSize).TextAlign = (ContentAlignment)16;
		cbSize.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbSize).Enabled = false;
		((Control)cbSize).Location = new Point(109, 36);
		((ComboBox)cbSize).MaxDropDownItems = 20;
		((Control)cbSize).Name = "cbSize";
		((Control)cbSize).Size = new Size(216, 21);
		((Control)cbSize).TabIndex = 0;
		((ComboBox)cbSize).SelectedIndexChanged += ComboBoxSize_SelectedIndexChanged;
		((Control)tbHeight).Location = new Point(109, 101);
		((Control)tbHeight).Name = "tbHeight";
		((Control)tbHeight).Size = new Size(96, 21);
		((Control)tbHeight).TabIndex = 2;
		tbHeight.TextAlign = (HorizontalAlignment)1;
		((Control)lbHeight).Location = new Point(12, 101);
		((Control)lbHeight).Name = "lbHeight";
		((Control)lbHeight).Size = new Size(91, 20);
		((Control)lbHeight).TabIndex = 11;
		((Control)lbHeight).Text = "Height";
		((Label)lbHeight).TextAlign = (ContentAlignment)16;
		((Control)lbWidth).Location = new Point(12, 69);
		((Control)lbWidth).Name = "lbWidth";
		((Control)lbWidth).Size = new Size(87, 20);
		((Control)lbWidth).TabIndex = 10;
		((Control)lbWidth).Text = "Width";
		((Label)lbWidth).TextAlign = (ContentAlignment)16;
		((Control)tbWidth).Location = new Point(109, 69);
		((Control)tbWidth).Name = "tbWidth";
		((Control)tbWidth).Size = new Size(96, 21);
		((Control)tbWidth).TabIndex = 1;
		tbWidth.TextAlign = (HorizontalAlignment)1;
		((Control)pageColumns).Controls.Add((Control)(object)checkBoxRightToLeft);
		((Control)pageColumns).Controls.Add((Control)(object)stiGroupLine1);
		((Control)pageColumns).Controls.Add((Control)(object)gbColumns);
		pageColumns.Dock = (DockStyle)5;
		pageColumns.Image = (Image)componentResourceManager.GetObject("pageColumns.Image");
		pageColumns.InvisibleVisible = false;
		((Control)pageColumns).Location = new Point(0, 0);
		((Control)pageColumns).Name = "pageColumns";
		((Control)pageColumns).Padding = new Padding(0, 0, 1, 0);
		((Control)pageColumns).Size = new Size(583, 488);
		((Control)pageColumns).TabIndex = 1;
		((Control)pageColumns).Text = "Columns";
		((Control)pageColumns).Visible = false;
		((Control)checkBoxRightToLeft).AutoSize = true;
		((Control)checkBoxRightToLeft).Location = new Point(191, 143);
		((Control)checkBoxRightToLeft).Name = "checkBoxRightToLeft";
		((Control)checkBoxRightToLeft).Size = new Size(84, 17);
		((Control)checkBoxRightToLeft).TabIndex = 4;
		((Control)checkBoxRightToLeft).Text = "Right to Left";
		((ButtonBase)checkBoxRightToLeft).UseVisualStyleBackColor = false;
		((Control)stiGroupLine1).Dock = (DockStyle)1;
		stiGroupLine1.LinePosition = StiLinePosition.Bottom;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 131);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(582, 3);
		((Control)stiGroupLine1).TabIndex = 3;
		((Control)gbColumns).Controls.Add((Control)(object)textBoxColumnGaps);
		((Control)gbColumns).Controls.Add((Control)(object)labelColumnGaps);
		((Control)gbColumns).Controls.Add((Control)(object)textBoxColumnWidth);
		((Control)gbColumns).Controls.Add((Control)(object)labelColumnWidth);
		((Control)gbColumns).Controls.Add((Control)(object)lbNumberColumns);
		((Control)gbColumns).Controls.Add((Control)(object)udColumns);
		((Control)gbColumns).Dock = (DockStyle)1;
		gbColumns.HeaderHeight = 24;
		((Control)gbColumns).Location = new Point(0, 0);
		((Control)gbColumns).Name = "gbColumns";
		((Control)gbColumns).Size = new Size(582, 131);
		((Control)gbColumns).TabIndex = 2;
		((Control)gbColumns).TabStop = false;
		((Control)gbColumns).Text = "Page columns";
		((Control)textBoxColumnGaps).Location = new Point(191, 100);
		((Control)textBoxColumnGaps).Name = "textBoxColumnGaps";
		((Control)textBoxColumnGaps).Size = new Size(104, 21);
		((Control)textBoxColumnGaps).TabIndex = 8;
		textBoxColumnGaps.TextAlign = (HorizontalAlignment)1;
		((Control)labelColumnGaps).Location = new Point(12, 100);
		((Control)labelColumnGaps).Name = "labelColumnGaps";
		((Control)labelColumnGaps).Size = new Size(174, 20);
		((Control)labelColumnGaps).TabIndex = 6;
		((Control)labelColumnGaps).Text = "Column Gaps";
		((Label)labelColumnGaps).TextAlign = (ContentAlignment)16;
		((Control)textBoxColumnWidth).Location = new Point(191, 68);
		((Control)textBoxColumnWidth).Name = "textBoxColumnWidth";
		((Control)textBoxColumnWidth).Size = new Size(104, 21);
		((Control)textBoxColumnWidth).TabIndex = 7;
		textBoxColumnWidth.TextAlign = (HorizontalAlignment)1;
		((Control)labelColumnWidth).Location = new Point(12, 68);
		((Control)labelColumnWidth).Name = "labelColumnWidth";
		((Control)labelColumnWidth).Size = new Size(174, 20);
		((Control)labelColumnWidth).TabIndex = 4;
		((Control)labelColumnWidth).Text = "Column Width";
		((Label)labelColumnWidth).TextAlign = (ContentAlignment)16;
		((Control)lbNumberColumns).Location = new Point(12, 36);
		((Control)lbNumberColumns).Name = "lbNumberColumns";
		((Control)lbNumberColumns).Size = new Size(174, 20);
		((Control)lbNumberColumns).TabIndex = 3;
		((Control)lbNumberColumns).Text = "Number of Columns";
		((Label)lbNumberColumns).TextAlign = (ContentAlignment)16;
		udColumns.DecimalPlaces = 0;
		((Control)udColumns).Location = new Point(191, 36);
		((Control)udColumns).Name = "udColumns";
		((Control)udColumns).Size = new Size(104, 21);
		((Control)udColumns).TabIndex = 5;
		((Control)pageWatermark).Controls.Add((Control)(object)panelElements);
		((Control)pageWatermark).Controls.Add((Control)(object)panelWatermarkControl);
		pageWatermark.Dock = (DockStyle)5;
		pageWatermark.Image = (Image)componentResourceManager.GetObject("pageWatermark.Image");
		pageWatermark.InvisibleVisible = true;
		((Control)pageWatermark).Location = new Point(0, 0);
		((Control)pageWatermark).Name = "pageWatermark";
		((Control)pageWatermark).Padding = new Padding(0, 0, 1, 0);
		((Control)pageWatermark).Size = new Size(583, 488);
		((Control)pageWatermark).TabIndex = 2;
		((Control)pageWatermark).Text = "Watermark";
		((Control)pageWatermark).Visible = false;
		((ScrollableControl)panelElements).AutoScroll = true;
		((Control)panelElements).Controls.Add((Control)(object)panelMoreOptions);
		((Control)panelElements).Controls.Add((Control)(object)panelImageOptions);
		((Control)panelElements).Controls.Add((Control)(object)panelImageGallery);
		((Control)panelElements).Controls.Add((Control)(object)gbImage);
		((Control)panelElements).Controls.Add((Control)(object)panelTextOptions2);
		((Control)panelElements).Controls.Add((Control)(object)panelSelectBrush);
		((Control)panelElements).Controls.Add((Control)(object)panelTextOptions);
		((Control)panelElements).Controls.Add((Control)(object)gbText);
		((Control)panelElements).Controls.Add((Control)(object)panelEnabled);
		((Control)panelElements).Dock = (DockStyle)5;
		((Control)panelElements).Location = new Point(180, 0);
		((Control)panelElements).Name = "panelElements";
		((Control)panelElements).Size = new Size(402, 488);
		((Control)panelElements).TabIndex = 9;
		((Control)panelMoreOptions).Controls.Add((Control)(object)buttonMoreOptions);
		((Control)panelMoreOptions).Controls.Add((Control)(object)stiGroupLine5);
		((Control)panelMoreOptions).Dock = (DockStyle)1;
		((Control)panelMoreOptions).Location = new Point(0, 808);
		((Control)panelMoreOptions).Name = "panelMoreOptions";
		((Control)panelMoreOptions).Size = new Size(385, 41);
		((Control)panelMoreOptions).TabIndex = 8;
		((Control)buttonMoreOptions).Anchor = (AnchorStyles)10;
		((Control)buttonMoreOptions).Location = new Point(252, 16);
		((Control)buttonMoreOptions).Name = "buttonMoreOptions";
		((Control)buttonMoreOptions).Size = new Size(123, 24);
		((Control)buttonMoreOptions).TabIndex = 31;
		((Control)buttonMoreOptions).Text = "More Options";
		((ButtonBase)buttonMoreOptions).UseVisualStyleBackColor = true;
		((Control)buttonMoreOptions).Click += ButtonMoreOptions_Click;
		((Control)stiGroupLine5).Anchor = (AnchorStyles)13;
		stiGroupLine5.LinePosition = StiLinePosition.Bottom;
		stiGroupLine5.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine5).Location = new Point(0, 2);
		((Control)stiGroupLine5).Name = "stiGroupLine5";
		((Control)stiGroupLine5).Size = new Size(389, 3);
		((Control)stiGroupLine5).TabIndex = 30;
		((Control)panelImageOptions).Controls.Add((Control)(object)trackBarWatermarkImageTransparency);
		((Control)panelImageOptions).Controls.Add((Control)(object)comboBoxWatermarkImageAlignment);
		((Control)panelImageOptions).Controls.Add((Control)(object)labelWatermarkImageAlignment);
		((Control)panelImageOptions).Controls.Add((Control)(object)labelWatermarkImageMultipleFactor);
		((Control)panelImageOptions).Controls.Add((Control)(object)checkBoxWatermarkAspectRatio);
		((Control)panelImageOptions).Controls.Add((Control)(object)textBoxWatermarkImageMultipleFactor);
		((Control)panelImageOptions).Controls.Add((Control)(object)checkBoxWatermarkImageTiling);
		((Control)panelImageOptions).Controls.Add((Control)(object)checkBoxWatermarkImageStretch);
		((Control)panelImageOptions).Controls.Add((Control)(object)labelWatermarkImageTransparency);
		((Control)panelImageOptions).Controls.Add((Control)(object)checkBoxWatermarkShowImageBehind);
		((Control)panelImageOptions).Controls.Add((Control)(object)stiGroupLine2);
		((Control)panelImageOptions).Dock = (DockStyle)1;
		((Control)panelImageOptions).Location = new Point(0, 601);
		((Control)panelImageOptions).Name = "panelImageOptions";
		((Control)panelImageOptions).Size = new Size(385, 207);
		((Control)panelImageOptions).TabIndex = 7;
		((Control)panelImageOptions).Visible = false;
		((Control)trackBarWatermarkImageTransparency).Anchor = (AnchorStyles)13;
		((Control)trackBarWatermarkImageTransparency).AutoSize = false;
		((Control)trackBarWatermarkImageTransparency).Location = new Point(133, 67);
		trackBarWatermarkImageTransparency.Maximum = 255;
		((Control)trackBarWatermarkImageTransparency).Name = "trackBarWatermarkImageTransparency";
		((Control)trackBarWatermarkImageTransparency).Size = new Size(247, 21);
		((Control)trackBarWatermarkImageTransparency).TabIndex = 2;
		trackBarWatermarkImageTransparency.TickStyle = (TickStyle)0;
		trackBarWatermarkImageTransparency.ValueChanged += TrackBarWatermarkImageTransparency_ValueChanged;
		((Control)comboBoxWatermarkImageAlignment).Anchor = (AnchorStyles)13;
		comboBoxWatermarkImageAlignment.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxWatermarkImageAlignment).Location = new Point(140, 2);
		((ComboBox)comboBoxWatermarkImageAlignment).MaxDropDownItems = 20;
		((Control)comboBoxWatermarkImageAlignment).Name = "comboBoxWatermarkImageAlignment";
		((Control)comboBoxWatermarkImageAlignment).Size = new Size(234, 21);
		((Control)comboBoxWatermarkImageAlignment).TabIndex = 0;
		((ListControl)comboBoxWatermarkImageAlignment).SelectedValueChanged += ComboBoxWatermarkImageAlignment_SelectedValueChanged;
		((Label)labelWatermarkImageAlignment).AutoEllipsis = true;
		((Control)labelWatermarkImageAlignment).Location = new Point(12, 2);
		((Control)labelWatermarkImageAlignment).Name = "labelWatermarkImageAlignment";
		((Control)labelWatermarkImageAlignment).Size = new Size(122, 21);
		((Control)labelWatermarkImageAlignment).TabIndex = 24;
		((Control)labelWatermarkImageAlignment).Text = "Image Alignment";
		((Label)labelWatermarkImageAlignment).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkImageAlignment).UseCompatibleTextRendering = true;
		((Label)labelWatermarkImageMultipleFactor).AutoEllipsis = true;
		((Control)labelWatermarkImageMultipleFactor).Location = new Point(12, 35);
		((Control)labelWatermarkImageMultipleFactor).Name = "labelWatermarkImageMultipleFactor";
		((Control)labelWatermarkImageMultipleFactor).Size = new Size(122, 20);
		((Control)labelWatermarkImageMultipleFactor).TabIndex = 25;
		((Control)labelWatermarkImageMultipleFactor).Text = "Image Multiple Factor";
		((Label)labelWatermarkImageMultipleFactor).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkImageMultipleFactor).UseCompatibleTextRendering = true;
		((Control)checkBoxWatermarkAspectRatio).AutoSize = true;
		((Control)checkBoxWatermarkAspectRatio).Location = new Point(140, 109);
		((Control)checkBoxWatermarkAspectRatio).Name = "checkBoxWatermarkAspectRatio";
		((Control)checkBoxWatermarkAspectRatio).Size = new Size(87, 18);
		((Control)checkBoxWatermarkAspectRatio).TabIndex = 3;
		((Control)checkBoxWatermarkAspectRatio).Text = "Aspect Ratio";
		((ButtonBase)checkBoxWatermarkAspectRatio).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkAspectRatio).UseVisualStyleBackColor = false;
		((Control)textBoxWatermarkImageMultipleFactor).Location = new Point(140, 35);
		((Control)textBoxWatermarkImageMultipleFactor).Name = "textBoxWatermarkImageMultipleFactor";
		((Control)textBoxWatermarkImageMultipleFactor).Size = new Size(104, 21);
		((Control)textBoxWatermarkImageMultipleFactor).TabIndex = 1;
		textBoxWatermarkImageMultipleFactor.TextAlign = (HorizontalAlignment)1;
		((Control)checkBoxWatermarkImageTiling).AutoSize = true;
		((Control)checkBoxWatermarkImageTiling).Location = new Point(140, 184);
		((Control)checkBoxWatermarkImageTiling).Name = "checkBoxWatermarkImageTiling";
		((Control)checkBoxWatermarkImageTiling).Size = new Size(85, 18);
		((Control)checkBoxWatermarkImageTiling).TabIndex = 6;
		((Control)checkBoxWatermarkImageTiling).Text = "Image Tiling";
		((ButtonBase)checkBoxWatermarkImageTiling).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkImageTiling).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxWatermarkImageTiling).CheckedChanged += CheckBoxWatermarkImageTiling_CheckedChanged;
		((Control)checkBoxWatermarkImageStretch).AutoSize = true;
		((Control)checkBoxWatermarkImageStretch).Location = new Point(140, 159);
		((Control)checkBoxWatermarkImageStretch).Name = "checkBoxWatermarkImageStretch";
		((Control)checkBoxWatermarkImageStretch).Size = new Size(94, 18);
		((Control)checkBoxWatermarkImageStretch).TabIndex = 5;
		((Control)checkBoxWatermarkImageStretch).Text = "Image Stretch";
		((ButtonBase)checkBoxWatermarkImageStretch).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkImageStretch).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxWatermarkImageStretch).CheckedChanged += CheckBoxWatermarkImageStretch_CheckedChanged;
		((Label)labelWatermarkImageTransparency).AutoEllipsis = true;
		((Control)labelWatermarkImageTransparency).Location = new Point(12, 67);
		((Control)labelWatermarkImageTransparency).Name = "labelWatermarkImageTransparency";
		((Control)labelWatermarkImageTransparency).Size = new Size(123, 21);
		((Control)labelWatermarkImageTransparency).TabIndex = 26;
		((Control)labelWatermarkImageTransparency).Text = "Image Transparency";
		((Label)labelWatermarkImageTransparency).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkImageTransparency).UseCompatibleTextRendering = true;
		((Control)checkBoxWatermarkShowImageBehind).AutoSize = true;
		((Control)checkBoxWatermarkShowImageBehind).Location = new Point(140, 134);
		((Control)checkBoxWatermarkShowImageBehind).Name = "checkBoxWatermarkShowImageBehind";
		((Control)checkBoxWatermarkShowImageBehind).Size = new Size(124, 18);
		((Control)checkBoxWatermarkShowImageBehind).TabIndex = 4;
		((Control)checkBoxWatermarkShowImageBehind).Text = "Show Image Behind";
		((ButtonBase)checkBoxWatermarkShowImageBehind).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkShowImageBehind).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxWatermarkShowImageBehind).CheckedChanged += CheckBoxWatermarkShowImageBehind_CheckedChanged;
		((Control)stiGroupLine2).Anchor = (AnchorStyles)13;
		stiGroupLine2.LinePosition = StiLinePosition.Bottom;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(2, 93);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(400, 5);
		((Control)stiGroupLine2).TabIndex = 28;
		panelImageGallery.AutoScroll = true;
		((Control)panelImageGallery).Dock = (DockStyle)1;
		((Control)panelImageGallery).Location = new Point(0, 545);
		((Control)panelImageGallery).Name = "panelImageGallery";
		((Control)panelImageGallery).Size = new Size(385, 56);
		((Control)panelImageGallery).TabIndex = 6;
		((Control)gbImage).Controls.Add((Control)(object)panelImage);
		((Control)gbImage).Dock = (DockStyle)1;
		gbImage.HeaderHeight = 24;
		((Control)gbImage).Location = new Point(0, 411);
		((Control)gbImage).Name = "gbImage";
		((Control)gbImage).Size = new Size(385, 134);
		((Control)gbImage).TabIndex = 5;
		((Control)gbImage).TabStop = false;
		((Control)gbImage).Text = "Watermark image";
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Anchor = (AnchorStyles)13;
		((Control)panelImage).Controls.Add((Control)(object)buttonImageOpen);
		((Control)panelImage).Controls.Add((Control)(object)buttonImageRemove);
		((Control)panelImage).Controls.Add((Control)(object)labelSpecification);
		((Control)panelImage).Location = new Point(12, 36);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(362, 86);
		((Control)panelImage).TabIndex = 0;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonImageOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageOpen).Image = (Image)componentResourceManager.GetObject("buttonImageOpen.Image");
		((Control)buttonImageOpen).Location = new Point(332, 6);
		((Control)buttonImageOpen).Name = "buttonImageOpen";
		((Control)buttonImageOpen).Size = new Size(24, 24);
		((Control)buttonImageOpen).TabIndex = 1;
		((ButtonBase)buttonImageOpen).UseVisualStyleBackColor = true;
		((Control)buttonImageOpen).Click += ButtonImageOpen_Click;
		((Control)buttonImageRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageRemove).Image = (Image)componentResourceManager.GetObject("buttonImageRemove.Image");
		((Control)buttonImageRemove).Location = new Point(332, 34);
		((Control)buttonImageRemove).Name = "buttonImageRemove";
		((Control)buttonImageRemove).Size = new Size(24, 24);
		((Control)buttonImageRemove).TabIndex = 2;
		((ButtonBase)buttonImageRemove).UseVisualStyleBackColor = true;
		((Control)buttonImageRemove).Click += ButtonImageRemove_Click;
		((Control)labelSpecification).Anchor = (AnchorStyles)6;
		((Control)labelSpecification).AutoSize = true;
		((Control)labelSpecification).BackColor = SystemColors.ControlDark;
		((Control)labelSpecification).Location = new Point(5, 63);
		((Control)labelSpecification).Name = "labelSpecification";
		((Control)labelSpecification).Padding = new Padding(3);
		((Control)labelSpecification).Size = new Size(74, 19);
		((Control)labelSpecification).TabIndex = 5;
		((Control)labelSpecification).Text = "Specification";
		((Control)labelSpecification).Visible = false;
		((Control)panelTextOptions2).Controls.Add((Control)(object)checkBoxWatermarkRightToLeft);
		((Control)panelTextOptions2).Controls.Add((Control)(object)checkBoxWatermarkShowBehind);
		((Control)panelTextOptions2).Controls.Add((Control)(object)watermarkSeparator1);
		((Control)panelTextOptions2).Dock = (DockStyle)1;
		((Control)panelTextOptions2).Location = new Point(0, 343);
		((Control)panelTextOptions2).Name = "panelTextOptions2";
		((Control)panelTextOptions2).Size = new Size(385, 68);
		((Control)panelTextOptions2).TabIndex = 4;
		((Control)panelTextOptions2).Visible = false;
		((Control)checkBoxWatermarkRightToLeft).AutoSize = true;
		((Control)checkBoxWatermarkRightToLeft).Location = new Point(139, 12);
		((Control)checkBoxWatermarkRightToLeft).Name = "checkBoxWatermarkRightToLeft";
		((Control)checkBoxWatermarkRightToLeft).Size = new Size(84, 18);
		((Control)checkBoxWatermarkRightToLeft).TabIndex = 0;
		((Control)checkBoxWatermarkRightToLeft).Text = "Right to Left";
		((ButtonBase)checkBoxWatermarkRightToLeft).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkRightToLeft).UseVisualStyleBackColor = false;
		((Control)checkBoxWatermarkShowBehind).AutoSize = true;
		((Control)checkBoxWatermarkShowBehind).Location = new Point(139, 37);
		((Control)checkBoxWatermarkShowBehind).Name = "checkBoxWatermarkShowBehind";
		((Control)checkBoxWatermarkShowBehind).Size = new Size(90, 18);
		((Control)checkBoxWatermarkShowBehind).TabIndex = 1;
		((Control)checkBoxWatermarkShowBehind).Text = "Show Behind";
		((ButtonBase)checkBoxWatermarkShowBehind).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxWatermarkShowBehind).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxWatermarkShowBehind).CheckedChanged += CheckBoxWatermarkShowBehind_CheckedChanged;
		((Control)watermarkSeparator1).Anchor = (AnchorStyles)13;
		watermarkSeparator1.LinePosition = StiLinePosition.Bottom;
		watermarkSeparator1.LineStyle = StiLineStyle.Dot;
		((Control)watermarkSeparator1).Location = new Point(0, 2);
		((Control)watermarkSeparator1).Name = "watermarkSeparator1";
		((Control)watermarkSeparator1).Size = new Size(400, 3);
		((Control)watermarkSeparator1).TabIndex = 30;
		((Control)panelSelectBrush).Controls.Add((Control)(object)stiGroupLine4);
		((Control)panelSelectBrush).Controls.Add((Control)(object)brushControl);
		((Control)panelSelectBrush).Controls.Add((Control)(object)labelSelectColor);
		((Control)panelSelectBrush).Dock = (DockStyle)1;
		((Control)panelSelectBrush).Location = new Point(0, 257);
		((Control)panelSelectBrush).Name = "panelSelectBrush";
		((Control)panelSelectBrush).Size = new Size(385, 86);
		((Control)panelSelectBrush).TabIndex = 3;
		((Control)panelSelectBrush).Visible = false;
		((Control)stiGroupLine4).Anchor = (AnchorStyles)13;
		stiGroupLine4.LinePosition = StiLinePosition.Bottom;
		stiGroupLine4.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine4).Location = new Point(0, 0);
		((Control)stiGroupLine4).Name = "stiGroupLine4";
		((Control)stiGroupLine4).Size = new Size(400, 3);
		((Control)stiGroupLine4).TabIndex = 31;
		((Control)brushControl).Anchor = (AnchorStyles)7;
		brushControl.Brush = null;
		((Control)brushControl).Location = new Point(139, 14);
		((Control)brushControl).Margin = new Padding(6);
		((Control)brushControl).Name = "brushControl";
		((Control)brushControl).Padding = new Padding(1);
		((Control)brushControl).Size = new Size(221, 69);
		((Control)brushControl).TabIndex = 0;
		brushControl.BrushChanged += BrushControl_BrushChanged;
		((Control)brushControl).ClientSizeChanged += BrushControl_ClientSizeChanged;
		((Label)labelSelectColor).AutoEllipsis = true;
		((Control)labelSelectColor).Location = new Point(12, 21);
		((Control)labelSelectColor).Name = "labelSelectColor";
		((Control)labelSelectColor).Size = new Size(122, 16);
		((Control)labelSelectColor).TabIndex = 25;
		((Control)labelSelectColor).Text = "Select Color";
		((Label)labelSelectColor).TextAlign = (ContentAlignment)16;
		((Label)labelSelectColor).UseCompatibleTextRendering = true;
		((Control)panelTextOptions).Controls.Add((Control)(object)numericUpDownWatermarkAngle);
		((Control)panelTextOptions).Controls.Add((Control)(object)buttonSelectFont);
		((Control)panelTextOptions).Controls.Add((Control)(object)labelWatermarkAngle);
		((Control)panelTextOptions).Controls.Add((Control)(object)labelSelectFont);
		((Control)panelTextOptions).Dock = (DockStyle)1;
		((Control)panelTextOptions).Location = new Point(0, 189);
		((Control)panelTextOptions).Name = "panelTextOptions";
		((Control)panelTextOptions).Size = new Size(385, 68);
		((Control)panelTextOptions).TabIndex = 2;
		((Control)panelTextOptions).Visible = false;
		numericUpDownWatermarkAngle.DecimalPlaces = 0;
		((Control)numericUpDownWatermarkAngle).Location = new Point(140, 1);
		((Control)numericUpDownWatermarkAngle).Name = "numericUpDownWatermarkAngle";
		((Control)numericUpDownWatermarkAngle).Size = new Size(123, 21);
		((Control)numericUpDownWatermarkAngle).TabIndex = 1;
		numericUpDownWatermarkAngle.ValueChanged += NumericUpDownWatermarkAngle_ValueChanged;
		((Control)buttonSelectFont).Location = new Point(139, 33);
		((Control)buttonSelectFont).Name = "buttonSelectFont";
		((Control)buttonSelectFont).Size = new Size(125, 23);
		((Control)buttonSelectFont).TabIndex = 2;
		((Control)buttonSelectFont).Text = "Font";
		((ButtonBase)buttonSelectFont).UseVisualStyleBackColor = true;
		((Control)buttonSelectFont).Click += ButtonSelectFont_Click;
		((Label)labelWatermarkAngle).AutoEllipsis = true;
		((Control)labelWatermarkAngle).Location = new Point(11, 1);
		((Control)labelWatermarkAngle).Name = "labelWatermarkAngle";
		((Control)labelWatermarkAngle).Size = new Size(124, 20);
		((Control)labelWatermarkAngle).TabIndex = 23;
		((Control)labelWatermarkAngle).Text = "Angle";
		((Label)labelWatermarkAngle).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkAngle).UseCompatibleTextRendering = true;
		((Label)labelSelectFont).AutoEllipsis = true;
		((Control)labelSelectFont).Location = new Point(12, 33);
		((Control)labelSelectFont).Name = "labelSelectFont";
		((Control)labelSelectFont).Size = new Size(124, 23);
		((Control)labelSelectFont).TabIndex = 22;
		((Control)labelSelectFont).Text = "Select Font";
		((Label)labelSelectFont).TextAlign = (ContentAlignment)16;
		((Label)labelSelectFont).UseCompatibleTextRendering = true;
		((Control)gbText).Controls.Add((Control)(object)panelText);
		((Control)gbText).Dock = (DockStyle)1;
		gbText.HeaderHeight = 24;
		((Control)gbText).Location = new Point(0, 80);
		((Control)gbText).Name = "gbText";
		((Control)gbText).Size = new Size(385, 109);
		((Control)gbText).TabIndex = 1;
		((Control)gbText).Text = "Watermark Text";
		((Control)panelText).AllowDrop = true;
		((Control)panelText).Anchor = (AnchorStyles)13;
		((Control)panelText).Controls.Add((Control)(object)buttonEditText);
		((Control)panelText).Controls.Add((Control)(object)textBoxWatermarkText);
		((Control)panelText).Location = new Point(12, 36);
		((Control)panelText).Name = "panelText";
		((Control)panelText).Padding = new Padding(4);
		((Control)panelText).Size = new Size(362, 61);
		((Control)panelText).TabIndex = 0;
		((Control)panelText).Paint += new PaintEventHandler(PanelText_Paint);
		((Control)buttonEditText).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditText).Image = (Image)componentResourceManager.GetObject("buttonEditText.Image");
		((Control)buttonEditText).Location = new Point(332, 5);
		((Control)buttonEditText).Name = "buttonEditText";
		((Control)buttonEditText).Size = new Size(24, 24);
		((Control)buttonEditText).TabIndex = 1;
		((ButtonBase)buttonEditText).UseVisualStyleBackColor = true;
		((Control)buttonEditText).Click += ButtonEditText_Click;
		textBoxWatermarkText.AcceptsReturn = true;
		((TextBoxBase)textBoxWatermarkText).BorderStyle = (BorderStyle)0;
		((Control)textBoxWatermarkText).Dock = (DockStyle)5;
		((Control)textBoxWatermarkText).ForeColor = SystemColors.ControlText;
		((Control)textBoxWatermarkText).Location = new Point(4, 4);
		((TextBoxBase)textBoxWatermarkText).Multiline = true;
		((Control)textBoxWatermarkText).Name = "textBoxWatermarkText";
		((Control)textBoxWatermarkText).Size = new Size(354, 53);
		((Control)textBoxWatermarkText).TabIndex = 0;
		((Control)textBoxWatermarkText).TextChanged += TextBoxWatermarkText_TextChanged;
		((Control)panelEnabled).Controls.Add((Control)(object)textBoxWatermarkEnabledExpression);
		((Control)panelEnabled).Controls.Add((Control)(object)comboBoxWatermarkEnabled);
		((Control)panelEnabled).Controls.Add((Control)(object)labelWatermarkEnabledExpression);
		((Control)panelEnabled).Controls.Add((Control)(object)labelWatermarkEnabled);
		((Control)panelEnabled).Dock = (DockStyle)1;
		((Control)panelEnabled).Location = new Point(0, 0);
		((Control)panelEnabled).Name = "panelEnabled";
		((Control)panelEnabled).Size = new Size(385, 80);
		((Control)panelEnabled).TabIndex = 0;
		((Control)panelEnabled).Visible = false;
		((Control)textBoxWatermarkEnabledExpression).Anchor = (AnchorStyles)13;
		textBoxWatermarkEnabledExpression.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxWatermarkEnabledExpression.ButtonBitmap");
		textBoxWatermarkEnabledExpression.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxWatermarkEnabledExpression.ButtonIcon");
		((Control)textBoxWatermarkEnabledExpression).Location = new Point(140, 45);
		((Control)textBoxWatermarkEnabledExpression).Name = "textBoxWatermarkEnabledExpression";
		((Control)textBoxWatermarkEnabledExpression).Size = new Size(234, 21);
		((Control)textBoxWatermarkEnabledExpression).TabIndex = 1;
		textBoxWatermarkEnabledExpression.ButtonClick += TextBoxWatermarkEnabledExpression_ButtonClick;
		comboBoxWatermarkEnabled.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxWatermarkEnabled).FormattingEnabled = true;
		((Control)comboBoxWatermarkEnabled).Location = new Point(140, 12);
		((Control)comboBoxWatermarkEnabled).Name = "comboBoxWatermarkEnabled";
		((Control)comboBoxWatermarkEnabled).Size = new Size(124, 21);
		((Control)comboBoxWatermarkEnabled).TabIndex = 0;
		((ComboBox)comboBoxWatermarkEnabled).SelectedIndexChanged += ComboBoxWatermarkEnabled_SelectedIndexChanged;
		((ComboBox)comboBoxWatermarkEnabled).SelectionChangeCommitted += ComboBoxWatermarkEnabled_SelectionChangeCommitted;
		((Label)labelWatermarkEnabledExpression).AutoEllipsis = true;
		((Control)labelWatermarkEnabledExpression).Location = new Point(12, 45);
		((Control)labelWatermarkEnabledExpression).Name = "labelWatermarkEnabledExpression";
		((Control)labelWatermarkEnabledExpression).Size = new Size(124, 24);
		((Control)labelWatermarkEnabledExpression).TabIndex = 36;
		((Control)labelWatermarkEnabledExpression).Text = "Expression";
		((Label)labelWatermarkEnabledExpression).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkEnabledExpression).UseCompatibleTextRendering = true;
		((Label)labelWatermarkEnabled).AutoEllipsis = true;
		((Control)labelWatermarkEnabled).Location = new Point(12, 12);
		((Control)labelWatermarkEnabled).Name = "labelWatermarkEnabled";
		((Control)labelWatermarkEnabled).Size = new Size(124, 22);
		((Control)labelWatermarkEnabled).TabIndex = 34;
		((Control)labelWatermarkEnabled).Text = "Enabled";
		((Label)labelWatermarkEnabled).TextAlign = (ContentAlignment)16;
		((Label)labelWatermarkEnabled).UseCompatibleTextRendering = true;
		((Control)panelWatermarkControl).Controls.Add((Control)(object)watermarkControl);
		((Control)panelWatermarkControl).Controls.Add((Control)(object)stiDotSeparatorVertical1);
		((Control)panelWatermarkControl).Dock = (DockStyle)3;
		((Control)panelWatermarkControl).Location = new Point(0, 0);
		((Control)panelWatermarkControl).Name = "panelWatermarkControl";
		((Control)panelWatermarkControl).Size = new Size(180, 488);
		((Control)panelWatermarkControl).TabIndex = 0;
		((Control)watermarkControl).BackColor = SystemColors.ButtonHighlight;
		((UserControl)watermarkControl).BorderStyle = (BorderStyle)1;
		watermarkControl.Image = null;
		watermarkControl.ImageAlignment = null;
		watermarkControl.ImageBehind = false;
		watermarkControl.ImageStretch = false;
		watermarkControl.ImageTiling = false;
		watermarkControl.ImageTransparency = 0;
		((Control)watermarkControl).Location = new Point(14, 7);
		((Control)watermarkControl).Margin = new Padding(6);
		((Control)watermarkControl).Name = "watermarkControl";
		((Control)watermarkControl).Size = new Size(152, 212);
		((Control)watermarkControl).TabIndex = 1;
		watermarkControl.TextAngle = 0.0;
		watermarkControl.TextBehind = false;
		watermarkControl.TextBrush = null;
		watermarkControl.TextValue = null;
		watermarkControl.WatermarkEnable = true;
		((Control)stiDotSeparatorVertical1).Dock = (DockStyle)4;
		((Control)stiDotSeparatorVertical1).Location = new Point(179, 0);
		((Control)stiDotSeparatorVertical1).MaximumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical1).MinimumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical1).Name = "stiDotSeparatorVertical1";
		((Control)stiDotSeparatorVertical1).Size = new Size(1, 488);
		((Control)stiDotSeparatorVertical1).TabIndex = 0;
		((Control)stiDotSeparatorVertical1).Text = "stiDotSeparatorVertical1";
		((Control)footerPanel).Controls.Add((Control)(object)btCancel);
		((Control)footerPanel).Controls.Add((Control)(object)btOk);
		((Control)footerPanel).Dock = (DockStyle)2;
		footerPanel.LinePosition = StiLinePosition.Top;
		footerPanel.LineStyle = StiLineStyle.Dot;
		((Control)footerPanel).Location = new Point(0, 488);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(702, 46);
		((Control)footerPanel).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(702, 534);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)footerPanel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPageSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Page Properties";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Activated += This_Activated;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).Load += This_Load;
		((Control)tabControl).ResumeLayout(false);
		((Control)pagePaper).ResumeLayout(false);
		((Control)gbPageMargins).ResumeLayout(false);
		((Control)gbOrientation).ResumeLayout(false);
		((Control)gbPaper).ResumeLayout(false);
		((Control)pageColumns).ResumeLayout(false);
		((Control)pageColumns).PerformLayout();
		((Control)gbColumns).ResumeLayout(false);
		((ISupportInitialize)udColumns).EndInit();
		((Control)pageWatermark).ResumeLayout(false);
		((Control)panelElements).ResumeLayout(false);
		((Control)panelMoreOptions).ResumeLayout(false);
		((Control)panelImageOptions).ResumeLayout(false);
		((Control)panelImageOptions).PerformLayout();
		((ISupportInitialize)trackBarWatermarkImageTransparency).EndInit();
		((Control)gbImage).ResumeLayout(false);
		((Control)panelImage).ResumeLayout(false);
		((Control)panelImage).PerformLayout();
		((Control)panelTextOptions2).ResumeLayout(false);
		((Control)panelTextOptions2).PerformLayout();
		((Control)panelSelectBrush).ResumeLayout(false);
		((Control)panelTextOptions).ResumeLayout(false);
		((ISupportInitialize)numericUpDownWatermarkAngle).EndInit();
		((Control)gbText).ResumeLayout(false);
		((Control)panelText).ResumeLayout(false);
		((Control)panelText).PerformLayout();
		((Control)panelEnabled).ResumeLayout(false);
		((Control)panelWatermarkControl).ResumeLayout(false);
		((Control)footerPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
