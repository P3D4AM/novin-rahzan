using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiReportLayout : UserControl
{
	private IContainer components;

	private StiOffice2013GroupBox gbUnit;

	internal RadioButton rbMillimeters;

	internal RadioButton rbCentimetres;

	internal RadioButton rbHundredthsOfInch;

	internal RadioButton rbInch;

	private StiOffice2013GroupBox gbLanguage;

	internal RadioButton rbVB;

	internal RadioButton rbCSharp;

	private StiOffice2013GroupBox gbOrientation;

	internal RadioButton rbLandscape;

	internal RadioButton rbPortrait;

	private StiOffice2013GroupBox gbComponents;

	internal RadioButton rbTable;

	internal RadioButton rbDataBand;

	public StiReportLayout()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
	}

	private void Localize()
	{
		((Control)gbUnit).Text = StiLocalization.Get("PropertyMain", "Unit");
		((Control)gbLanguage).Text = StiLocalization.Get("PropertyMain", "Language");
		((Control)rbCentimetres).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
		((Control)rbHundredthsOfInch).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
		((Control)rbInch).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeInches");
		((Control)rbMillimeters).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
		((Control)gbOrientation).Text = StiLocalization.Get("FormPageSetup", "Orientation");
		((Control)rbPortrait).Text = StiLocalization.Get("FormPageSetup", "PageOrientationPortrait");
		((Control)rbLandscape).Text = StiLocalization.Get("FormPageSetup", "PageOrientationLandscape");
		((Control)gbComponents).Text = StiLocalization.Get("Components", "StiComponent");
		((Control)rbDataBand).Text = StiLocalization.Get("Components", "StiDataBand");
		((Control)rbTable).Text = StiLocalization.Get("Components", "StiTable");
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		gbUnit = new StiOffice2013GroupBox();
		rbMillimeters = new RadioButton();
		rbCentimetres = new RadioButton();
		rbHundredthsOfInch = new RadioButton();
		rbInch = new RadioButton();
		gbLanguage = new StiOffice2013GroupBox();
		rbVB = new RadioButton();
		rbCSharp = new RadioButton();
		gbOrientation = new StiOffice2013GroupBox();
		rbLandscape = new RadioButton();
		rbPortrait = new RadioButton();
		gbComponents = new StiOffice2013GroupBox();
		rbTable = new RadioButton();
		rbDataBand = new RadioButton();
		((Control)gbUnit).SuspendLayout();
		((Control)gbLanguage).SuspendLayout();
		((Control)gbOrientation).SuspendLayout();
		((Control)gbComponents).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)gbUnit).Controls.Add((Control)(object)rbMillimeters);
		((Control)gbUnit).Controls.Add((Control)(object)rbCentimetres);
		((Control)gbUnit).Controls.Add((Control)(object)rbHundredthsOfInch);
		((Control)gbUnit).Controls.Add((Control)(object)rbInch);
		gbUnit.HeaderHeight = 24;
		((Control)gbUnit).Location = new Point(544, 0);
		((Control)gbUnit).Margin = new Padding(6, 6, 6, 6);
		((Control)gbUnit).Name = "gbUnit";
		((Control)gbUnit).Padding = new Padding(0, 48, 0, 0);
		((Control)gbUnit).Size = new Size(536, 337);
		((Control)gbUnit).TabIndex = 2;
		((Control)gbUnit).TabStop = false;
		((Control)gbUnit).Text = "Unit";
		((Control)rbMillimeters).Anchor = (AnchorStyles)13;
		((Control)rbMillimeters).ForeColor = SystemColors.ControlText;
		((Control)rbMillimeters).Location = new Point(16, 190);
		((Control)rbMillimeters).Margin = new Padding(6, 6, 6, 6);
		((Control)rbMillimeters).Name = "rbMillimeters";
		((Control)rbMillimeters).Size = new Size(504, 46);
		((Control)rbMillimeters).TabIndex = 3;
		((Control)rbMillimeters).Text = "Millimeters";
		((Control)rbCentimetres).Anchor = (AnchorStyles)13;
		((Control)rbCentimetres).ForeColor = SystemColors.ControlText;
		((Control)rbCentimetres).Location = new Point(16, 144);
		((Control)rbCentimetres).Margin = new Padding(6, 6, 6, 6);
		((Control)rbCentimetres).Name = "rbCentimetres";
		((Control)rbCentimetres).Size = new Size(504, 46);
		((Control)rbCentimetres).TabIndex = 2;
		((Control)rbCentimetres).Text = "Centimetres";
		((Control)rbHundredthsOfInch).Anchor = (AnchorStyles)13;
		((Control)rbHundredthsOfInch).ForeColor = SystemColors.ControlText;
		((Control)rbHundredthsOfInch).Location = new Point(16, 98);
		((Control)rbHundredthsOfInch).Margin = new Padding(6, 6, 6, 6);
		((Control)rbHundredthsOfInch).Name = "rbHundredthsOfInch";
		((Control)rbHundredthsOfInch).Size = new Size(504, 46);
		((Control)rbHundredthsOfInch).TabIndex = 1;
		((Control)rbHundredthsOfInch).Text = "HundredthsOfInch";
		((Control)rbInch).Anchor = (AnchorStyles)13;
		rbInch.Checked = true;
		((Control)rbInch).ForeColor = SystemColors.ControlText;
		((Control)rbInch).Location = new Point(16, 52);
		((Control)rbInch).Margin = new Padding(6, 6, 6, 6);
		((Control)rbInch).Name = "rbInch";
		((Control)rbInch).Size = new Size(504, 46);
		((Control)rbInch).TabIndex = 0;
		rbInch.TabStop = true;
		((Control)rbInch).Text = "Inch";
		((Control)gbLanguage).Controls.Add((Control)(object)rbVB);
		((Control)gbLanguage).Controls.Add((Control)(object)rbCSharp);
		gbLanguage.HeaderHeight = 24;
		((Control)gbLanguage).Location = new Point(0, 163);
		((Control)gbLanguage).Margin = new Padding(6, 6, 6, 6);
		((Control)gbLanguage).Name = "gbLanguage";
		((Control)gbLanguage).Size = new Size(544, 173);
		((Control)gbLanguage).TabIndex = 1;
		((Control)gbLanguage).TabStop = false;
		((Control)gbLanguage).Text = "Language";
		((Control)rbVB).Anchor = (AnchorStyles)13;
		((Control)rbVB).ForeColor = SystemColors.ControlText;
		((Control)rbVB).Location = new Point(16, 100);
		((Control)rbVB).Margin = new Padding(6, 6, 6, 6);
		((Control)rbVB).Name = "rbVB";
		((Control)rbVB).Size = new Size(496, 46);
		((Control)rbVB).TabIndex = 1;
		((Control)rbVB).Text = "VB.Net";
		((Control)rbCSharp).Anchor = (AnchorStyles)13;
		rbCSharp.Checked = true;
		((Control)rbCSharp).ForeColor = SystemColors.ControlText;
		((Control)rbCSharp).Location = new Point(16, 58);
		((Control)rbCSharp).Margin = new Padding(6, 6, 6, 6);
		((Control)rbCSharp).Name = "rbCSharp";
		((Control)rbCSharp).Size = new Size(496, 46);
		((Control)rbCSharp).TabIndex = 0;
		rbCSharp.TabStop = true;
		((Control)rbCSharp).Text = "C#";
		((Control)gbOrientation).Controls.Add((Control)(object)rbLandscape);
		((Control)gbOrientation).Controls.Add((Control)(object)rbPortrait);
		gbOrientation.HeaderHeight = 24;
		((Control)gbOrientation).Location = new Point(0, 0);
		((Control)gbOrientation).Margin = new Padding(6, 6, 6, 6);
		((Control)gbOrientation).Name = "gbOrientation";
		((Control)gbOrientation).Padding = new Padding(0, 48, 0, 0);
		((Control)gbOrientation).Size = new Size(544, 163);
		((Control)gbOrientation).TabIndex = 0;
		((Control)gbOrientation).TabStop = false;
		((Control)gbOrientation).Text = "Orientation";
		((Control)rbLandscape).Anchor = (AnchorStyles)13;
		((Control)rbLandscape).ForeColor = SystemColors.ControlText;
		((Control)rbLandscape).Location = new Point(16, 98);
		((Control)rbLandscape).Margin = new Padding(6, 6, 6, 6);
		((Control)rbLandscape).Name = "rbLandscape";
		((Control)rbLandscape).Size = new Size(496, 46);
		((Control)rbLandscape).TabIndex = 1;
		((Control)rbLandscape).Text = "Landscape";
		((Control)rbPortrait).Anchor = (AnchorStyles)13;
		rbPortrait.Checked = true;
		((Control)rbPortrait).ForeColor = SystemColors.ControlText;
		((Control)rbPortrait).Location = new Point(16, 52);
		((Control)rbPortrait).Margin = new Padding(6, 6, 6, 6);
		((Control)rbPortrait).Name = "rbPortrait";
		((Control)rbPortrait).Size = new Size(496, 46);
		((Control)rbPortrait).TabIndex = 0;
		rbPortrait.TabStop = true;
		((Control)rbPortrait).Text = "Portrait";
		((Control)gbComponents).Controls.Add((Control)(object)rbTable);
		((Control)gbComponents).Controls.Add((Control)(object)rbDataBand);
		gbComponents.HeaderHeight = 24;
		((Control)gbComponents).Location = new Point(0, 337);
		((Control)gbComponents).Margin = new Padding(6, 6, 6, 6);
		((Control)gbComponents).Name = "gbComponents";
		((Control)gbComponents).Padding = new Padding(0, 48, 0, 0);
		((Control)gbComponents).Size = new Size(1080, 146);
		((Control)gbComponents).TabIndex = 3;
		((Control)gbComponents).TabStop = false;
		((Control)gbComponents).Text = "Component";
		((Control)rbTable).Anchor = (AnchorStyles)13;
		((Control)rbTable).ForeColor = SystemColors.ControlText;
		((Control)rbTable).Location = new Point(16, 94);
		((Control)rbTable).Margin = new Padding(6, 6, 6, 6);
		((Control)rbTable).Name = "rbTable";
		((Control)rbTable).Size = new Size(1032, 46);
		((Control)rbTable).TabIndex = 1;
		((Control)rbTable).Text = "Table";
		((Control)rbDataBand).Anchor = (AnchorStyles)13;
		rbDataBand.Checked = true;
		((Control)rbDataBand).ForeColor = SystemColors.ControlText;
		((Control)rbDataBand).Location = new Point(16, 48);
		((Control)rbDataBand).Margin = new Padding(6, 6, 6, 6);
		((Control)rbDataBand).Name = "rbDataBand";
		((Control)rbDataBand).Size = new Size(1032, 46);
		((Control)rbDataBand).TabIndex = 0;
		rbDataBand.TabStop = true;
		((Control)rbDataBand).Text = "DataBand";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)gbComponents);
		((Control)this).Controls.Add((Control)(object)gbOrientation);
		((Control)this).Controls.Add((Control)(object)gbLanguage);
		((Control)this).Controls.Add((Control)(object)gbUnit);
		((Control)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiReportLayout";
		((Control)this).Size = new Size(1080, 615);
		((Control)gbUnit).ResumeLayout(false);
		((Control)gbLanguage).ResumeLayout(false);
		((Control)gbOrientation).ResumeLayout(false);
		((Control)gbComponents).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
