using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Design.Interaction;

[ToolboxItem(false)]
public class StiInteractionDrilldownControl : UserControl
{
	private StiInteraction interaction;

	private List<StiPage> datas;

	private bool isBuildDrillDownPages;

	private IContainer components;

	private Panel panelTop;

	private CheckBox checkBoxDrillDownEnabled;

	private Panel panelBottom;

	private StiComboBox comboBoxDrillDownMode;

	private Label labelDrillDownMode;

	private StiOffice2013GroupBox groupBoxDrillDownPage;

	private StiOffice2013GroupBox groupBoxDrillDownReport;

	private Label labelDrillDownReportFileName;

	internal StiListBoxV2 listBoxPanels;

	internal StiButtonEdit textBoxDrillDownReportFileName;

	private StiOffice2013GroupBox groupGallery;

	private ScrollableControl listGallery;

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(75.0);

	internal string NoSelection => "[" + Loc.Get("Report", "NotAssigned") + "]";

	internal bool IsNotEmpty => checkBoxDrillDownEnabled.Checked;

	public event EventHandler GalleryDoubleClick;

	public StiInteractionDrilldownControl(StiInteraction interaction)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.interaction = interaction;
			DoScaling();
			checkBoxDrillDownEnabled.Checked = this.interaction.DrillDownEnabled;
			((Control)textBoxDrillDownReportFileName).Text = this.interaction.DrillDownReport;
			Build();
			Localize();
		}
	}

	private void InvokeGalleryDoubleClick()
	{
		if (this.GalleryDoubleClick != null)
		{
			this.GalleryDoubleClick(this, EventArgs.Empty);
		}
	}

	public void Localize()
	{
		((Control)checkBoxDrillDownEnabled).Text = StiLocalization.Get("PropertyMain", "DrillDownEnabled");
		((Control)groupBoxDrillDownPage).Text = StiLocalization.Get("PropertyMain", "DrillDownPage");
		((Control)groupBoxDrillDownReport).Text = StiLocalization.Get("PropertyMain", "DrillDownReport");
		((Control)labelDrillDownReportFileName).Text = Loc.GetCleaned("Cloud", "labelFileName");
		((Control)labelDrillDownMode).Text = Loc.GetCleaned("PropertyMain", "DrillDownMode");
		((Control)groupGallery).Text = StiLocalization.Get("Report", "Gallery");
		int num = ((ListControl)comboBoxDrillDownMode).SelectedIndex;
		((ComboBox)comboBoxDrillDownMode).Items.Clear();
		((ComboBox)comboBoxDrillDownMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDrillDownModeSinglePage"));
		((ComboBox)comboBoxDrillDownMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDrillDownModeMultiPage"));
		if (num == -1)
		{
			num = (int)interaction.DrillDownMode;
		}
		((ListControl)comboBoxDrillDownMode).SelectedIndex = num;
	}

	public void Save()
	{
		interaction.DrillDownEnabled = checkBoxDrillDownEnabled.Checked;
		interaction.DrillDownReport = ((Control)textBoxDrillDownReportFileName).Text;
		interaction.DrillDownMode = (StiDrillDownMode)((ListControl)comboBoxDrillDownMode).SelectedIndex;
		StiReport report = interaction.ParentComponent.Report;
		interaction.DrillDownPage = report.Pages[((ListBox)listBoxPanels).SelectedItem.ToString()];
	}

	internal void DoScaling()
	{
		textBoxDrillDownReportFileName.Image = StiReportDesignImages.Actions.EditSmall();
		((Control)comboBoxDrillDownMode).Width = ((Control)textBoxDrillDownReportFileName).Width;
	}

	private void Build()
	{
		if (isBuildDrillDownPages)
		{
			return;
		}
		isBuildDrillDownPages = true;
		((ListBox)listBoxPanels).Items.Clear();
		((ListBox)listBoxPanels).Items.Add((object)NoSelection);
		((ListControl)listBoxPanels).SelectedIndex = 0;
		datas = new List<StiPage>();
		StiReport report = interaction.ParentComponent.Report;
		foreach (StiPage page in report.Pages)
		{
			if (page != interaction.ParentComponent.Page)
			{
				datas.Add(page);
				((ListBox)listBoxPanels).Items.Add((object)page.Name);
				if (page == interaction.DrillDownPage)
				{
					((ListControl)listBoxPanels).SelectedIndex = ((ListBox)listBoxPanels).Items.Count - 1;
				}
			}
		}
	}

	private void BuildGallery()
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		List<StiResource> list = (from v in interaction.ParentComponent.Report.Dictionary.Resources.ToList()
			where (v.Type == StiResourceType.Report || v.Type == StiResourceType.ReportSnapshot) && v.Content != null
			select v).ToList();
		((Control)groupGallery).Visible = list.Any();
		((Control)listGallery).Controls.Clear();
		int num = ItemGalleryStep;
		int itemGalleryStep = ItemGalleryStep;
		string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)textBoxDrillDownReportFileName).Text);
		foreach (StiResource item in list)
		{
			try
			{
				StiImageButton button = CreateGalleryButton(item.Name, item, num, itemGalleryStep, ItemGallerySize);
				if (resourceNameFromHyperlink == item.Name)
				{
					((CheckBox)button).Checked = true;
					ImageGallery_Click(button, EventArgs.Empty);
				}
				((CheckBox)button).CheckedChanged += ImageGallery_Click;
				((CheckBox)button).DoubleClick += Button_DoubleClick;
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					if (!IsFormDestructed())
					{
						((Control)listGallery).Controls.Add((Control)(object)button);
					}
				});
				num += ItemGallerySize + ItemGalleryStep;
			}
			catch
			{
			}
		}
	}

	private void Button_DoubleClick(object sender, EventArgs e)
	{
		InvokeGalleryDoubleClick();
	}

	private bool IsFormDestructed()
	{
		if (!((Control)this).IsDisposed && !((Control)this).Disposing)
		{
			return !((Control)this).IsHandleCreated;
		}
		return true;
	}

	private StiImageButton CreateGalleryButton(string name, object tag, int x, int y, int thumbWidth)
	{
		StiImageButton stiImageButton = new StiImageButton();
		((Control)stiImageButton).Left = x;
		((Control)stiImageButton).Top = y;
		((Control)stiImageButton).Width = thumbWidth;
		((Control)stiImageButton).Height = thumbWidth;
		((Control)stiImageButton).Text = name;
		((Control)stiImageButton).Tag = tag;
		StiImageButton stiImageButton2 = stiImageButton;
		try
		{
			StiResource stiResource = tag as StiResource;
			StiReport stiReport = new StiReport();
			StiPage stiPage;
			if (stiResource.Type == StiResourceType.ReportSnapshot)
			{
				stiReport.LoadDocument(stiResource.Content);
				stiPage = stiReport.RenderedPages[0];
			}
			else
			{
				stiReport.Load(stiResource.Content);
				stiPage = stiReport.Pages[0];
			}
			((ButtonBase)stiImageButton2).Image = (Image)(object)stiPage.GetThumbnail(thumbWidth, thumbWidth);
		}
		catch
		{
		}
		return stiImageButton2;
	}

	private void textBoxDrillDownReportFileName_ButtonClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			StiSettings.Load();
			string text = StiOptions.Designer.ReportSaveLoadPath;
			if (string.IsNullOrEmpty(text))
			{
				text = StiSettings.GetStr("StiDesigner", "ReportSaveLoadPath", string.Empty);
			}
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				((FileDialog)val).InitialDirectory = text;
			}
			((FileDialog)val).Filter = StiSLService.GetReportFilters(StiSLActions.Load);
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen"));
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && !string.IsNullOrEmpty(((FileDialog)val).FileName))
			{
				((Control)textBoxDrillDownReportFileName).Text = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ImageGallery_Click(object sender, EventArgs e)
	{
		(from Control c in (IEnumerable)((Control)listGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked && ((Control)stiImageButton).Tag is StiResource stiResource)
		{
			((Control)textBoxDrillDownReportFileName).Text = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		BuildGallery();
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiInteractionDrilldownControl));
		panelTop = new Panel();
		checkBoxDrillDownEnabled = new CheckBox();
		panelBottom = new Panel();
		comboBoxDrillDownMode = new StiComboBox();
		labelDrillDownMode = new Label();
		groupBoxDrillDownPage = new StiOffice2013GroupBox();
		listBoxPanels = new StiListBoxV2();
		groupBoxDrillDownReport = new StiOffice2013GroupBox();
		labelDrillDownReportFileName = new Label();
		textBoxDrillDownReportFileName = new StiButtonEdit();
		groupGallery = new StiOffice2013GroupBox();
		listGallery = new ScrollableControl();
		((Control)panelTop).SuspendLayout();
		((Control)panelBottom).SuspendLayout();
		((Control)groupBoxDrillDownPage).SuspendLayout();
		((Control)groupBoxDrillDownReport).SuspendLayout();
		((Control)groupGallery).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelTop).Controls.Add((Control)(object)checkBoxDrillDownEnabled);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(400, 47);
		((Control)panelTop).TabIndex = 6;
		((Control)checkBoxDrillDownEnabled).Location = new Point(12, 12);
		((Control)checkBoxDrillDownEnabled).Name = "checkBoxDrillDownEnabled";
		((Control)checkBoxDrillDownEnabled).Size = new Size(373, 24);
		((Control)checkBoxDrillDownEnabled).TabIndex = 0;
		((Control)checkBoxDrillDownEnabled).Text = "Drill-Down Enabled";
		((ButtonBase)checkBoxDrillDownEnabled).UseVisualStyleBackColor = true;
		((Control)panelBottom).Controls.Add((Control)(object)comboBoxDrillDownMode);
		((Control)panelBottom).Controls.Add((Control)(object)labelDrillDownMode);
		((Control)panelBottom).Dock = (DockStyle)2;
		((Control)panelBottom).Location = new Point(0, 227);
		((Control)panelBottom).Name = "panelBottom";
		((Control)panelBottom).Size = new Size(400, 40);
		((Control)panelBottom).TabIndex = 7;
		comboBoxDrillDownMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxDrillDownMode).Location = new Point(154, 5);
		((Control)comboBoxDrillDownMode).Name = "comboBoxDrillDownMode";
		((Control)comboBoxDrillDownMode).Size = new Size(231, 21);
		((Control)comboBoxDrillDownMode).TabIndex = 1;
		((Control)labelDrillDownMode).ForeColor = Color.DimGray;
		((Control)labelDrillDownMode).Location = new Point(12, 5);
		((Control)labelDrillDownMode).Name = "labelDrillDownMode";
		((Control)labelDrillDownMode).Size = new Size(136, 21);
		((Control)labelDrillDownMode).TabIndex = 0;
		((Control)labelDrillDownMode).Text = "Drill-Down Mode";
		labelDrillDownMode.TextAlign = (ContentAlignment)16;
		((Control)groupBoxDrillDownPage).Controls.Add((Control)(object)listBoxPanels);
		((Control)groupBoxDrillDownPage).Dock = (DockStyle)5;
		groupBoxDrillDownPage.HeaderHeight = 24;
		((Control)groupBoxDrillDownPage).Location = new Point(0, 47);
		((Control)groupBoxDrillDownPage).Name = "groupBoxDrillDownPage";
		((Control)groupBoxDrillDownPage).Size = new Size(400, 108);
		((Control)groupBoxDrillDownPage).TabIndex = 8;
		((Control)groupBoxDrillDownPage).TabStop = false;
		((Control)groupBoxDrillDownPage).Text = "Drill-Down Page";
		((Control)listBoxPanels).Anchor = (AnchorStyles)15;
		((ListControl)listBoxPanels).FormattingEnabled = true;
		((Control)listBoxPanels).Location = new Point(12, 39);
		((Control)listBoxPanels).Name = "listBoxPanels";
		((Control)listBoxPanels).Size = new Size(373, 54);
		((Control)listBoxPanels).TabIndex = 0;
		((Control)groupBoxDrillDownReport).Controls.Add((Control)(object)labelDrillDownReportFileName);
		((Control)groupBoxDrillDownReport).Controls.Add((Control)(object)textBoxDrillDownReportFileName);
		((Control)groupBoxDrillDownReport).Dock = (DockStyle)2;
		groupBoxDrillDownReport.HeaderHeight = 24;
		((Control)groupBoxDrillDownReport).Location = new Point(0, 155);
		((Control)groupBoxDrillDownReport).Name = "groupBoxDrillDownReport";
		((Control)groupBoxDrillDownReport).Size = new Size(400, 72);
		((Control)groupBoxDrillDownReport).TabIndex = 9;
		((Control)groupBoxDrillDownReport).TabStop = false;
		((Control)groupBoxDrillDownReport).Text = "Drill-Down Report";
		((Control)labelDrillDownReportFileName).ForeColor = Color.DimGray;
		((Control)labelDrillDownReportFileName).Location = new Point(12, 39);
		((Control)labelDrillDownReportFileName).Name = "labelDrillDownReportFileName";
		((Control)labelDrillDownReportFileName).Size = new Size(136, 21);
		((Control)labelDrillDownReportFileName).TabIndex = 1;
		((Control)labelDrillDownReportFileName).Text = "File Name";
		labelDrillDownReportFileName.TextAlign = (ContentAlignment)16;
		((Control)textBoxDrillDownReportFileName).Anchor = (AnchorStyles)13;
		((Control)textBoxDrillDownReportFileName).BackColor = SystemColors.Window;
		((Control)textBoxDrillDownReportFileName).ForeColor = SystemColors.ControlText;
		textBoxDrillDownReportFileName.Image = (Image)componentResourceManager.GetObject("textBoxDrillDownReportFileName.Image");
		((Control)textBoxDrillDownReportFileName).Location = new Point(154, 39);
		((Control)textBoxDrillDownReportFileName).Name = "textBoxDrillDownReportFileName";
		((Control)textBoxDrillDownReportFileName).Size = new Size(231, 21);
		((Control)textBoxDrillDownReportFileName).TabIndex = 6;
		textBoxDrillDownReportFileName.ButtonClick += textBoxDrillDownReportFileName_ButtonClick;
		((Control)groupGallery).BackColor = Color.White;
		((Control)groupGallery).Controls.Add((Control)(object)listGallery);
		((Control)groupGallery).Dock = (DockStyle)2;
		groupGallery.HeaderHeight = 24;
		((Control)groupGallery).Location = new Point(0, 267);
		((Control)groupGallery).Name = "groupGallery";
		((Control)groupGallery).Size = new Size(400, 133);
		((Control)groupGallery).TabIndex = 61;
		((Control)groupGallery).TabStop = false;
		((Control)groupGallery).Text = "Gallery";
		((Control)listGallery).Anchor = (AnchorStyles)15;
		listGallery.AutoScroll = true;
		((Control)listGallery).BackColor = Color.White;
		((Control)listGallery).Location = new Point(4, 27);
		((Control)listGallery).Name = "listGallery";
		((Control)listGallery).Size = new Size(392, 101);
		((Control)listGallery).TabIndex = 59;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)groupBoxDrillDownPage);
		((Control)this).Controls.Add((Control)(object)groupBoxDrillDownReport);
		((Control)this).Controls.Add((Control)(object)panelBottom);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Controls.Add((Control)(object)groupGallery);
		((Control)this).Name = "StiInteractionDrilldownControl";
		((Control)this).Size = new Size(400, 400);
		((UserControl)this).Load += This_Load;
		((Control)panelTop).ResumeLayout(false);
		((Control)panelBottom).ResumeLayout(false);
		((Control)groupBoxDrillDownPage).ResumeLayout(false);
		((Control)groupBoxDrillDownReport).ResumeLayout(false);
		((Control)groupGallery).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
