using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiNewForm : Form
{
	private StiDesignerControl designer;

	private Hashtable imagesTemplates = new Hashtable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Button btCancel;

	private Button btOk;

	private ImageList ilNew;

	private ListView lvReportItems;

	private StiGroupControl gcReportItems;

	private StiGroupControl gcElements;

	private Panel panel3;

	private ListView lvElements;

	private Panel pnElements;

	private StiGroupBox stiGroupBox1;

	private PictureBox pbReportPage;

	private ListViewItem listViewItemBlankReport;

	private ListViewItem listViewItemBlankDashboard;

	private ListViewItem listViewItemUsingReportWizard;

	private ListViewItem listViewItemFromReportTemplate;

	private ListViewItem listViewItemOpenExistingReport;

	public StiReport CreatedReport { get; private set; }

	public StiReport MasterReport { get; }

	public StiNewForm(StiReport masterReport)
		: this(null, masterReport)
	{
	}

	public StiNewForm(StiDesignerControl designer)
		: this(designer, null)
	{
	}

	private StiNewForm(StiDesignerControl designer, StiReport masterReport)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.designer = designer;
		MasterReport = masterReport;
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		if (!StiDashboardAssembly.IsAssemblyLoaded)
		{
			lvReportItems.Items.Remove(listViewItemBlankDashboard);
		}
		((Form)this).ShowIcon = false;
		DoScaling();
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("Wizards", "title");
		((Control)gcReportItems).Text = StiLocalization.Get("Wizards", "groupCreateNewReport");
		lvReportItems.Items[0].Text = StiLocalization.Get("Wizards", "BlankReport");
		lvReportItems.Items[1].Text = StiLocalization.Get("FormOptions", "BlankDashboard");
		lvReportItems.Items[2].Text = StiLocalization.Get("Wizards", "UsingReportWizard");
		lvReportItems.Items[3].Text = StiLocalization.Get("Wizards", "FromReportTemplate");
		lvReportItems.Items[4].Text = StiLocalization.Get("Wizards", "OpenExistingReport");
	}

	private void DoScaling()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (StiScaleUI.Factor == 1.0)
		{
			return;
		}
		ImageList val = new ImageList();
		val.ImageSize = StiScaleUI.I(ilNew.ImageSize);
		foreach (Image image2 in ilNew.Images)
		{
			Image image = image2;
			val.Images.Add(StiImageResizer.GetImage(image));
		}
		lvReportItems.LargeImageList = val;
		lvElements.LargeImageList = val;
	}

	private void DisposeImagesTemplates()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (Bitmap value in imagesTemplates.Values)
		{
			Bitmap val = value;
			((Image)val).Dispose();
		}
	}

	private void StartReportWizard()
	{
		try
		{
			bool flag = false;
			if (designer != null)
			{
				flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
			}
			if (!flag)
			{
				int num = lvElements.SelectedIndices[0];
				if (lvReportItems.SelectedIndices.Count <= 0)
				{
					return;
				}
				List<StiWizardService> list = StiDesignerOptions.Wizards.Where((StiWizardService s) => s.ServiceEnabled).ToList();
				if (list.Count <= num)
				{
					return;
				}
				StiWizardService stiWizardService = list[num];
				StiReport stiReport = null;
				bool flag2 = false;
				if (designer == null)
				{
					stiReport = ((MasterReport == null) ? stiWizardService.CreateReport(new StiReport()) : stiWizardService.CreateReport(MasterReport));
				}
				else
				{
					StiReport stiReport2 = designer.Report;
					if (stiReport2 == null)
					{
						stiReport2 = new StiReport();
						designer.Report = stiReport2;
						flag2 = true;
					}
					stiReport = stiWizardService.CreateReport(stiReport2);
				}
				if (stiReport != null)
				{
					CreatedReport = stiReport;
					if (designer != null)
					{
						StiActions.MainMenu.InvokeRegNewReport(designer, stiReport);
					}
					((Form)this).Close();
				}
				else if (flag2)
				{
					designer.Report.IsModified = false;
					StiActions.MainMenu.InvokeClose(designer);
				}
			}
			else
			{
				((Form)this).Close();
			}
		}
		finally
		{
			StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
		}
	}

	private void btCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		if (lvReportItems.SelectedIndices.Count > 0)
		{
			int num = lvReportItems.SelectedIndices[0];
			if (num == 2 || num == 3)
			{
				lvElements_DoubleClick(sender, e);
			}
			else
			{
				lvReportItems_DoubleClick(sender, e);
			}
		}
		else
		{
			lvReportItems_DoubleClick(sender, e);
		}
	}

	private void lvReportItems_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		if (lvReportItems.SelectedIndices.Count <= 0)
		{
			return;
		}
		int num = lvReportItems.SelectedIndices[0];
		switch (num)
		{
		case 0:
		case 1:
		case 4:
			((Control)pnElements).Visible = false;
			((Control)pnElements).Visible = false;
			((Control)pbReportPage).Visible = false;
			break;
		case 2:
			((Control)gcElements).Text = StiLocalization.Get("Wizards", "groupWizards");
			((Control)pnElements).Visible = true;
			((Control)pbReportPage).Visible = false;
			lvElements.Items.Clear();
			foreach (StiWizardService item in StiDesignerOptions.Wizards.Where((StiWizardService s) => s.ServiceEnabled))
			{
				lvElements.Items.Add(new ListViewItem(item.ServiceName, 2));
			}
			if (lvElements.Items.Count > 0)
			{
				lvElements.Items[0].Selected = true;
			}
			break;
		case 3:
		{
			((Control)gcElements).Text = StiLocalization.Get("Wizards", "groupTemplates");
			((Control)pnElements).Visible = true;
			((Control)pbReportPage).Visible = true;
			pbReportPage.Image = null;
			StiReportTemplatesService service = StiReportTemplatesService.GetService();
			if (service != null)
			{
				lvElements.Items.Clear();
				string[] reportsNames = service.GetReportsNames();
				if (reportsNames != null)
				{
					string[] array = reportsNames;
					foreach (string text in array)
					{
						lvElements.Items.Add(new ListViewItem(text, 4));
					}
				}
			}
			if (lvElements.Items.Count > 0)
			{
				lvElements.Items[0].Selected = true;
			}
			break;
		}
		default:
			if (num == 3)
			{
				((Control)pbReportPage).Visible = false;
				((Control)pnElements).Visible = false;
			}
			break;
		}
	}

	private void lvReportItems_DoubleClick(object sender, EventArgs e)
	{
		if (lvReportItems.SelectedIndices.Count <= 0)
		{
			return;
		}
		switch (lvReportItems.SelectedIndices[0])
		{
		case 0:
			if (designer == null)
			{
				CreatedReport = new StiReport();
			}
			else
			{
				StiActions.MainMenu.InvokeReportNewBlank(designer);
			}
			((Form)this).DialogResult = (DialogResult)1;
			break;
		case 1:
			if (designer == null)
			{
				CreatedReport = new StiReport();
			}
			else
			{
				StiActions.MainMenu.InvokeDashboardNewBlank(designer);
			}
			((Form)this).DialogResult = (DialogResult)1;
			break;
		case 2:
			StartReportWizard();
			break;
		case 4:
		{
			bool flag = false;
			if (designer != null)
			{
				flag = StiActions.MainMenu.InvokeReportOpen(designer);
			}
			else
			{
				CreatedReport = new StiReport();
				flag = StiReportOpenDialog.OpenReport(CreatedReport);
			}
			if (flag)
			{
				((Form)this).Close();
			}
			break;
		}
		case 3:
			break;
		}
	}

	private void lvElements_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lvElements.SelectedIndices.Count <= 0)
		{
			return;
		}
		int num = lvElements.SelectedIndices[0];
		if (lvReportItems.SelectedIndices.Count <= 0)
		{
			return;
		}
		int num2 = lvReportItems.SelectedIndices[0];
		if (num2 == 2 || num2 != 3)
		{
			return;
		}
		string text = lvElements.Items[num].Text;
		object? obj = imagesTemplates[text];
		Bitmap val = (Bitmap)((obj is Bitmap) ? obj : null);
		if (val == null)
		{
			StiReportTemplatesService service = StiReportTemplatesService.GetService();
			if (service != null)
			{
				StiReport reportTemplate = service.GetReportTemplate(text);
				if (reportTemplate != null)
				{
					StiPage stiPage = reportTemplate.Pages[0];
					val = stiPage.GetThumbnail(((Control)pbReportPage).Width, ((Control)pbReportPage).Height, isDesignTime: true);
					imagesTemplates[text] = val;
				}
			}
		}
		pbReportPage.Image = (Image)(object)val;
	}

	private void StiNewForm_Closed(object sender, EventArgs e)
	{
		DisposeImagesTemplates();
		StiFormSettings.Save((Form)(object)this);
	}

	private void lvElements_DoubleClick(object sender, EventArgs e)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (lvElements.SelectedIndices.Count <= 0)
		{
			return;
		}
		int num = lvElements.SelectedIndices[0];
		if (lvReportItems.SelectedIndices.Count <= 0)
		{
			return;
		}
		switch (lvReportItems.SelectedIndices[0])
		{
		case 2:
			StartReportWizard();
			break;
		case 3:
		{
			string text = lvElements.Items[num].Text;
			StiReportTemplatesService service = StiReportTemplatesService.GetService();
			if (service == null)
			{
				break;
			}
			StiReport reportTemplate = service.GetReportTemplate(text);
			if (reportTemplate != null)
			{
				CreatedReport = reportTemplate;
				if (designer != null)
				{
					StiActions.MainMenu.InvokeRegNewReport(designer, reportTemplate);
				}
				((Form)this).Close();
			}
			else
			{
				MessageBox.Show(StiLocalization.Get("DesignerFx", "CanNotLoadThisReportTemplate"));
			}
			break;
		}
		}
	}

	private void StiNewForm_Load(object sender, EventArgs e)
	{
		lvReportItems.Items[0].Selected = true;
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiNewForm));
		listViewItemBlankReport = new ListViewItem(new string[1] { "Blank Report" }, 0, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204));
		listViewItemBlankDashboard = new ListViewItem("Blank Dashboard", 1);
		listViewItemUsingReportWizard = new ListViewItem(new string[1] { "Using Report Wizard" }, 2, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204));
		listViewItemFromReportTemplate = new ListViewItem(new string[1] { "From Report Template" }, 3, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204));
		listViewItemOpenExistingReport = new ListViewItem(new string[1] { "Open Existing Report" }, 4, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204));
		ilNew = new ImageList(components);
		panel1 = new Panel();
		pnElements = new Panel();
		lvElements = new ListView();
		panel3 = new Panel();
		gcElements = new StiGroupControl();
		lvReportItems = new ListView();
		panel2 = new Panel();
		gcReportItems = new StiGroupControl();
		btCancel = new Button();
		btOk = new Button();
		stiGroupBox1 = new StiGroupBox();
		pbReportPage = new PictureBox();
		((Control)panel1).SuspendLayout();
		((Control)pnElements).SuspendLayout();
		((Control)stiGroupBox1).SuspendLayout();
		((ISupportInitialize)pbReportPage).BeginInit();
		((Control)this).SuspendLayout();
		ilNew.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilNew.ImageStream");
		ilNew.TransparentColor = Color.Magenta;
		ilNew.Images.SetKeyName(0, "ReportBlankNew.png");
		ilNew.Images.SetKeyName(1, "DashboardBlankNew.png");
		ilNew.Images.SetKeyName(2, "ReportNewWizard.png");
		ilNew.Images.SetKeyName(3, "ReportNewTemplate.png");
		ilNew.Images.SetKeyName(4, "ReportOpen.png");
		ilNew.Images.SetKeyName(5, "ReportNew.png");
		((Control)panel1).Anchor = (AnchorStyles)15;
		((Control)panel1).BackColor = SystemColors.Window;
		panel1.BorderStyle = (BorderStyle)2;
		((Control)panel1).Controls.Add((Control)(object)pnElements);
		((Control)panel1).Controls.Add((Control)(object)lvReportItems);
		((Control)panel1).Controls.Add((Control)(object)panel2);
		((Control)panel1).Controls.Add((Control)(object)gcReportItems);
		((Control)panel1).Location = new Point(4, 4);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(400, 334);
		((Control)panel1).TabIndex = 3;
		((Control)pnElements).Controls.Add((Control)(object)lvElements);
		((Control)pnElements).Controls.Add((Control)(object)panel3);
		((Control)pnElements).Controls.Add((Control)(object)gcElements);
		((Control)pnElements).Dock = (DockStyle)5;
		((Control)pnElements).Location = new Point(0, 104);
		((Control)pnElements).Name = "pnElements";
		((Control)pnElements).Size = new Size(396, 226);
		((Control)pnElements).TabIndex = 8;
		((Control)pnElements).Visible = false;
		lvElements.BorderStyle = (BorderStyle)0;
		((Control)lvElements).Dock = (DockStyle)5;
		lvElements.HideSelection = false;
		lvElements.LargeImageList = ilNew;
		((Control)lvElements).Location = new Point(0, 31);
		((Control)lvElements).Name = "lvElements";
		((Control)lvElements).Size = new Size(396, 195);
		((Control)lvElements).TabIndex = 9;
		lvElements.UseCompatibleStateImageBehavior = false;
		lvElements.SelectedIndexChanged += lvElements_SelectedIndexChanged;
		((Control)lvElements).DoubleClick += lvElements_DoubleClick;
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(0, 23);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(396, 8);
		((Control)panel3).TabIndex = 8;
		((Control)gcElements).Dock = (DockStyle)1;
		((Control)gcElements).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)gcElements).Location = new Point(0, 0);
		((Control)gcElements).Name = "gcElements";
		((Control)gcElements).Size = new Size(396, 23);
		((Control)gcElements).TabIndex = 7;
		((Control)gcElements).Text = "Create a New Report";
		lvReportItems.BorderStyle = (BorderStyle)0;
		((Control)lvReportItems).Dock = (DockStyle)1;
		lvReportItems.HideSelection = false;
		lvReportItems.Items.AddRange((ListViewItem[])(object)new ListViewItem[5] { listViewItemBlankReport, listViewItemBlankDashboard, listViewItemUsingReportWizard, listViewItemFromReportTemplate, listViewItemOpenExistingReport });
		lvReportItems.LargeImageList = ilNew;
		((Control)lvReportItems).Location = new Point(0, 31);
		lvReportItems.MultiSelect = false;
		((Control)lvReportItems).Name = "lvReportItems";
		((Control)lvReportItems).Size = new Size(396, 73);
		((Control)lvReportItems).TabIndex = 4;
		lvReportItems.UseCompatibleStateImageBehavior = false;
		lvReportItems.SelectedIndexChanged += lvReportItems_SelectedIndexChanged;
		((Control)lvReportItems).DoubleClick += lvReportItems_DoubleClick;
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 23);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(396, 8);
		((Control)panel2).TabIndex = 2;
		((Control)gcReportItems).Dock = (DockStyle)1;
		((Control)gcReportItems).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)gcReportItems).Location = new Point(0, 0);
		((Control)gcReportItems).Name = "gcReportItems";
		((Control)gcReportItems).Size = new Size(396, 23);
		((Control)gcReportItems).TabIndex = 3;
		((Control)gcReportItems).Text = "Create a New Report";
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(568, 346);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 5;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += btCancel_Click;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(488, 346);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 4;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)stiGroupBox1).Anchor = (AnchorStyles)11;
		((Control)stiGroupBox1).Controls.Add((Control)(object)pbReportPage);
		((Control)stiGroupBox1).Location = new Point(408, 4);
		((Control)stiGroupBox1).Name = "stiGroupBox1";
		stiGroupBox1.ResHeight = 0;
		((Control)stiGroupBox1).Size = new Size(244, 334);
		((Control)stiGroupBox1).TabIndex = 6;
		((GroupBox)stiGroupBox1).TabStop = false;
		((Control)pbReportPage).Anchor = (AnchorStyles)15;
		((Control)pbReportPage).Location = new Point(8, 8);
		((Control)pbReportPage).Name = "pbReportPage";
		((Control)pbReportPage).Size = new Size(230, 317);
		pbReportPage.TabIndex = 0;
		pbReportPage.TabStop = false;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(656, 376);
		((Control)this).Controls.Add((Control)(object)stiGroupBox1);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiNewForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "New";
		((Form)this).Closed += StiNewForm_Closed;
		((Form)this).Load += StiNewForm_Load;
		((Control)panel1).ResumeLayout(false);
		((Control)pnElements).ResumeLayout(false);
		((Control)stiGroupBox1).ResumeLayout(false);
		((ISupportInitialize)pbReportPage).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
