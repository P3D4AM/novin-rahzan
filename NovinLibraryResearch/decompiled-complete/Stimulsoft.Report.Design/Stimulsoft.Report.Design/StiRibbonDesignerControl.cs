using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Ribbon;
using Stimulsoft.Report.Design.Toolbars;

namespace Stimulsoft.Report.Design;

[ToolboxBitmap(typeof(StiRibbonDesignerControl), "Toolbox.StiRibbonDesignerControl.bmp")]
public sealed class StiRibbonDesignerControl : StiDesignerControl
{
	private bool showControlBox = true;

	private bool showHelp = true;

	private bool showLocalizationMenu = true;

	private bool showOfficeButton = true;

	private bool showTabHome = true;

	private bool showTabHomeBarClipboard = true;

	private bool showTabHomeBarFont = true;

	private bool showTabHomeBarAlignment = true;

	private bool showTabHomeBarBorders = true;

	private bool showTabHomeBarTextFormat = true;

	private bool showTabHomeBarStyle = true;

	private bool showTabPage = true;

	private bool showTabPageBarPageSetup = true;

	private bool showTabPageBarWatermarkText = true;

	private bool showTabPageBarWatermarkImage = true;

	private bool showTabLayout = true;

	private bool showTabLayoutBarArrange = true;

	private bool showTabLayoutBarDockStyle = true;

	private bool showTabView = true;

	private bool showTabViewBarViewOptions = true;

	private bool showMainMenuNew = true;

	private bool showMainMenuReportNew = true;

	private bool showMainMenuReportWizardNew = true;

	private bool showMainMenuPageNew = true;

	private bool showMainMenuFormNew = true;

	private bool showMainMenuReportOpen = true;

	private bool showMainMenuPageOpen = true;

	private bool showMainMenuReportSave = true;

	private bool showMainMenuSaveAs = true;

	private bool showMainMenuReportSaveAs = true;

	private bool showMainMenuPageSaveAs = true;

	private bool showMainMenuPageDelete = true;

	private bool showMainMenuReportPreview = true;

	private bool showMainMenuCheckForIssues = true;

	private bool showMainMenuReportSetup = true;

	private bool showMainMenuRecentFiles = true;

	private bool showMainMenuClose = true;

	private bool showMainMenuOptions = true;

	private bool showMainMenuExit = true;

	private bool isPreview;

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowCaption
	{
		get
		{
			return RibbonControl.CaptionVisible;
		}
		set
		{
			RibbonControl.CaptionVisible = value;
			RibbonControl.TabGroupsVisible = value;
			if (value)
			{
				((Control)RibbonControl).Height = (int)(153.0 * StiScale.Factor);
			}
			else
			{
				((Control)RibbonControl).Height = (int)(124.0 * StiScale.Factor);
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowControlBox
	{
		get
		{
			return showControlBox;
		}
		set
		{
			showControlBox = value;
			if (RibbonControl.RibbonStrip != null && RibbonControl.RibbonStrip.SystemCaptionItem != null)
			{
				RibbonControl.RibbonStrip.SystemCaptionItem.MinimizeVisible = value;
				RibbonControl.RibbonStrip.SystemCaptionItem.CloseVisible = value;
				RibbonControl.RibbonStrip.SystemCaptionItem.RestoreMaximizeVisible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowHelp
	{
		get
		{
			return showHelp;
		}
		set
		{
			showHelp = value;
			showLocalizationMenu = value;
			if (RibbonControl.Items.IndexOf("biHelp") != -1)
			{
				RibbonControl.Items["biHelp"].Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowLocalizationMenu
	{
		get
		{
			return showLocalizationMenu;
		}
		set
		{
			showLocalizationMenu = value;
			RibbonControl.Items["biSelectLanguage"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowOfficeButton
	{
		get
		{
			return showOfficeButton;
		}
		set
		{
			showOfficeButton = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			officeStartButton.Visible = value;
		}
	}

	public override RibbonControl RibbonControl
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is RibbonControl)
				{
					return val as RibbonControl;
				}
			}
			return null;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHome
	{
		get
		{
			return showTabHome;
		}
		set
		{
			showTabHome = value;
			bool flag = RibbonControl.SelectedRibbonTabItem == RibbonControl.Items["ribbonTabHome"];
			RibbonControl.Items["ribbonTabHome"].Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (object item in RibbonControl.Items)
			{
				if (item is RibbonTabItem { Visible: not false } ribbonTabItem)
				{
					ribbonTabItem.Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarClipboard
	{
		get
		{
			return showTabHomeBarClipboard;
		}
		set
		{
			showTabHomeBarClipboard = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarClipboard"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarFont
	{
		get
		{
			return showTabHomeBarFont;
		}
		set
		{
			showTabHomeBarFont = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarFont"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarAlignment
	{
		get
		{
			return showTabHomeBarAlignment;
		}
		set
		{
			showTabHomeBarAlignment = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarAlignment"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarBorders
	{
		get
		{
			return showTabHomeBarBorders;
		}
		set
		{
			showTabHomeBarBorders = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarBorders"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarTextFormat
	{
		get
		{
			return showTabHomeBarTextFormat;
		}
		set
		{
			showTabHomeBarTextFormat = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarTextFormat"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarStyle
	{
		get
		{
			return showTabHomeBarStyle;
		}
		set
		{
			showTabHomeBarStyle = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelHome"];
			val.Controls["ribbonBarStyle"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPage
	{
		get
		{
			return showTabPage;
		}
		set
		{
			showTabPage = value;
			bool flag = RibbonControl.SelectedRibbonTabItem == RibbonControl.Items["ribbonTabPage"];
			RibbonControl.Items["ribbonTabPage"].Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (ButtonItem item in RibbonControl.Items)
			{
				if (item is RibbonTabItem { Visible: not false } ribbonTabItem)
				{
					ribbonTabItem.Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPageBarPageSetup
	{
		get
		{
			return showTabPageBarPageSetup;
		}
		set
		{
			showTabPageBarPageSetup = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelPage"];
			val.Controls["ribbonBarPageSetup"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPageBarWatermarkText
	{
		get
		{
			return showTabPageBarWatermarkText;
		}
		set
		{
			showTabPageBarWatermarkText = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelPage"];
			val.Controls["ribbonBarWatermarkText"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPageBarWatermarkImage
	{
		get
		{
			return showTabPageBarWatermarkImage;
		}
		set
		{
			showTabPageBarWatermarkImage = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelPage"];
			val.Controls["ribbonBarWatermarkImage"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabLayout
	{
		get
		{
			return showTabLayout;
		}
		set
		{
			showTabLayout = value;
			bool flag = RibbonControl.SelectedRibbonTabItem == RibbonControl.Items["ribbonTabLayout"];
			RibbonControl.Items["ribbonTabLayout"].Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (RibbonTabItem item in RibbonControl.Items)
			{
				if (item.Visible)
				{
					item.Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabLayoutBarArrange
	{
		get
		{
			return showTabLayoutBarArrange;
		}
		set
		{
			showTabLayoutBarArrange = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelLayout"];
			val.Controls["ribbonBarArrange"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabLayoutBarDockStyle
	{
		get
		{
			return showTabLayoutBarDockStyle;
		}
		set
		{
			showTabLayoutBarDockStyle = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelLayout"];
			val.Controls["ribbonBarDockStyle"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabView
	{
		get
		{
			return showTabView;
		}
		set
		{
			showTabView = value;
			bool flag = RibbonControl.SelectedRibbonTabItem == RibbonControl.Items["ribbonTabView"];
			RibbonControl.Items["ribbonTabView"].Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (RibbonTabItem item in RibbonControl.Items)
			{
				if (item.Visible)
				{
					item.Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabViewBarViewOptions
	{
		get
		{
			return showTabViewBarViewOptions;
		}
		set
		{
			showTabViewBarViewOptions = value;
			Control val = ((Control)RibbonControl).Controls["ribbonPanelPage"];
			val.Controls["ribbonBarViewOptions"].Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuNew
	{
		get
		{
			return showMainMenuNew;
		}
		set
		{
			showMainMenuNew = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonNew.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportNew
	{
		get
		{
			return showMainMenuReportNew;
		}
		set
		{
			showMainMenuReportNew = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biNew"];
			BaseItem baseItem5 = baseItem4.SubItems["biReportNew"];
			baseItem5.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportWizardNew
	{
		get
		{
			return showMainMenuReportWizardNew;
		}
		set
		{
			showMainMenuReportWizardNew = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biNew"];
			BaseItem baseItem5 = baseItem4.SubItems["biReportWizardNew"];
			baseItem5.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuPageNew
	{
		get
		{
			return showMainMenuPageNew;
		}
		set
		{
			showMainMenuPageNew = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biNew"];
			BaseItem baseItem5 = baseItem4.SubItems["biPageNew"];
			baseItem5.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuFormNew
	{
		get
		{
			return showMainMenuFormNew;
		}
		set
		{
			showMainMenuFormNew = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biNew"];
			BaseItem baseItem5 = baseItem4.SubItems["biFormNew"];
			baseItem5.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportOpen
	{
		get
		{
			return showMainMenuReportOpen;
		}
		set
		{
			showMainMenuReportOpen = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonReportOpen.Visible = value;
			}
		}
	}

	[Obsolete("Property 'ShowMainMenuReportOpenFromGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowMainMenuReportOpenFromGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuPageOpen
	{
		get
		{
			return showMainMenuPageOpen;
		}
		set
		{
			showMainMenuPageOpen = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biPageOpen"];
			baseItem4.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSave
	{
		get
		{
			return showMainMenuReportSave;
		}
		set
		{
			showMainMenuReportSave = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonReportSave.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuSaveAs
	{
		get
		{
			return showMainMenuSaveAs;
		}
		set
		{
			showMainMenuSaveAs = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biSaveAs"];
			baseItem4.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSaveAs
	{
		get
		{
			return showMainMenuReportSaveAs;
		}
		set
		{
			showMainMenuReportSaveAs = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonReportSaveAs.Visible = value;
			}
		}
	}

	[Obsolete("Property 'ShowMainMenuReportSaveAsToGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowMainMenuReportSaveAsToGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuPageSaveAs
	{
		get
		{
			return showMainMenuPageSaveAs;
		}
		set
		{
			showMainMenuPageSaveAs = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			BaseItem baseItem4 = baseItem3.SubItems["biSaveAs"];
			BaseItem baseItem5 = baseItem4.SubItems["biPageSaveAs"];
			baseItem5.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuPageDelete
	{
		get
		{
			return showMainMenuPageDelete;
		}
		set
		{
			showMainMenuPageDelete = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["itemContainerMainMenu3"];
			if (baseItem3.SubItems.Contains("biPageDelete"))
			{
				BaseItem baseItem4 = baseItem3.SubItems["biPageDelete"];
				baseItem4.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportPreview
	{
		get
		{
			return showMainMenuReportPreview;
		}
		set
		{
			showMainMenuReportPreview = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonPreview.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuCheckForIssues
	{
		get
		{
			return showMainMenuCheckForIssues;
		}
		set
		{
			showMainMenuCheckForIssues = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				((Control)stiRibbonBackStageUserControl.buttonCheckIssue).Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSetup
	{
		get
		{
			return showMainMenuReportSetup;
		}
		set
		{
			showMainMenuReportSetup = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				((Control)stiRibbonBackStageUserControl.buttonReportOptions).Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuRecentFiles
	{
		get
		{
			return showMainMenuRecentFiles;
		}
		set
		{
			showMainMenuRecentFiles = value;
			BaseItem officeStartButton = GetOfficeStartButton();
			BaseItem baseItem = officeStartButton.SubItems["itemContainerMainMenu"];
			BaseItem baseItem2 = baseItem.SubItems["itemContainerMainMenu2"];
			BaseItem baseItem3 = baseItem2.SubItems["galleryRecentFiles"];
			baseItem3.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuClose
	{
		get
		{
			return showMainMenuClose;
		}
		set
		{
			showMainMenuClose = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonClose.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuOptions
	{
		get
		{
			return showMainMenuOptions;
		}
		set
		{
			showMainMenuOptions = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonOptions.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuExit
	{
		get
		{
			return showMainMenuExit;
		}
		set
		{
			showMainMenuExit = value;
			ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			if (stiRibbonBackStageUserControl != null)
			{
				stiRibbonBackStageUserControl.buttonHelp.Visible = value;
			}
		}
	}

	public override void ApplyStyle()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		base.ApplyStyle();
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is RibbonControl))
			{
				continue;
			}
			RibbonControl ribbonControl = val as RibbonControl;
			foreach (Control item2 in (ArrangedElementCollection)((Control)ribbonControl).Controls)
			{
				Control val2 = item2;
				val2.Refresh();
			}
		}
	}

	public RibbonTabItem CreateRibbonTab(string text)
	{
		return CreateRibbonTab(text, 4);
	}

	public RibbonTabItem CreateRibbonTab(string text, int index)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		RibbonTabItem ribbonTabItem = new RibbonTabItem();
		ribbonTabItem.Text = text;
		RibbonControl.Items.Insert(index, ribbonTabItem);
		RibbonPanel ribbonPanel = new RibbonPanel();
		((Control)ribbonPanel).Padding = new Padding(3, 0, 3, 3);
		((Control)ribbonPanel).Dock = (DockStyle)5;
		((Control)RibbonControl).Controls.Add((Control)(object)ribbonPanel);
		ribbonTabItem.Panel = ribbonPanel;
		return ribbonTabItem;
	}

	public RibbonBar CreateRibbonBar(RibbonTabItem ribbonTab, string text)
	{
		RibbonBar ribbonBar = new RibbonBar();
		((Control)ribbonBar).Text = text;
		((Control)ribbonTab.Panel).Controls.Add((Control)(object)ribbonBar);
		return ribbonBar;
	}

	private BaseItem GetOfficeStartButton()
	{
		return RibbonControl.Items["OfficeStartButton"];
	}

	internal void RibbonTabPreview_CheckedChanged(object sender, EventArgs e)
	{
		isPreview = ((RibbonTabItem)sender).Checked;
		ShowPreview();
		if (isPreview)
		{
			base.RibbonDesignerUserControl?.ResetShortcutOpenFile();
			base.DesignerPreviewControl.ViewerControl?.InitShortcuts();
		}
		else
		{
			base.RibbonDesignerUserControl?.InitShortcutOpenFile();
			base.DesignerPreviewControl.ViewerControl?.ResetShortcuts();
		}
	}

	internal void ShowPreview()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		StiElementActionHelper.Hide();
		if (IsViewerPresent && base.DesignerPreviewControl != null)
		{
			((Control)base.DesignerPreviewControl).Visible = false;
		}
		if (base.LockTabControlRefresh)
		{
			return;
		}
		if (isPreview)
		{
			if (StiDashboardHelper.IsDashboardPresent(base.Report))
			{
				ShowDashboardPreview();
			}
			else
			{
				ShowReportPreview();
			}
		}
		else
		{
			if (base.Report == null)
			{
				return;
			}
			base.Report.IsStopped = true;
			base.Report.IsRendering = false;
			base.Report.ResetSelection();
			base.Report.GetCurrentPage().Select();
			AddSelectedToPropertyGrid(updateSelectedObjects: true);
			foreach (Control item in (ArrangedElementCollection)((Control)TabControl).Controls[TabControl.SelectedTabIndex].Controls)
			{
				Control val = item;
				if (val is StiPageViewControl)
				{
					val.Focus();
					break;
				}
			}
			if (IsViewerPresent)
			{
				UpdateDesignerPreviewControl1(null, null);
			}
			if (StiDashboardHelper.IsDashboardPresent(base.Report))
			{
				Control val2 = ((((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0].Name == "dashboardViewerControl") ? ((Control)this).Controls[0] : null);
				if (val2 != null)
				{
					if (val2.Parent != null && val2.Parent.Controls.Contains(val2))
					{
						val2.Parent.Controls.Remove(val2);
					}
					((Component)(object)val2).Dispose();
				}
				RibbonControl?.ReleaseExpanded();
			}
			base.Report.CompiledReport = null;
			base.Report.IsStopped = true;
			base.DesignerPreviewControl.ClearOldReport();
		}
		((Control)this).ResumeLayout();
		Form obj = ((Control)this).FindForm();
		if (obj != null)
		{
			((Control)obj).Invalidate();
		}
		((Control)this).Refresh();
	}

	private void ShowDashboardPreview()
	{
		RibbonControl.StopExpanded();
		SaveReportBeforePreview();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		try
		{
			base.Report.ResetRenderedState();
			InvokeClearingCompilerResults();
			base.CheckHelper?.BuildChecks(base.Report);
			if (!isPreview)
			{
				return;
			}
			if (base.CheckHelper.ErrorsCount > 0)
			{
				InvokeShowingCompilerResults(base.Report.CompilerResults);
			}
			if (base.CheckHelper.ErrorsCount == 0)
			{
				if (base.Report.CompiledReport != null)
				{
					base.Report.CompiledReport.Key = StiKeyHelper.GenerateKey();
					StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(this);
					CreateDashboardViewer(base.Report.CompiledReport);
				}
				else if (base.Report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
				{
					StiReport stiReport = StiReportCopyHelper.Copy(base.Report);
					stiReport.Key = StiKeyHelper.GenerateKey();
					StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(this);
					CreateDashboardViewer(stiReport);
				}
			}
			if (base.CheckHelper.IsMessagesPresent && isPreview)
			{
				base.IsReportPreviewMode = true;
				StiCheckStatusEventArgs e = new StiCheckStatusEventArgs(base.CheckHelper.ErrorsCount, base.CheckHelper.WarningsCount, base.CheckHelper.InformationMessagesCount, base.CheckHelper.ReportRenderingMessagesCount);
				InvokeShowingReportCheckerResults(e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(IStiDesignerBase), ex);
			StiExceptionProvider.Show(ex);
		}
	}

	private void ShowReportPreview()
	{
		if (IsViewerPresent)
		{
			SaveReportBeforePreview();
			UpdateDesignerPreviewControl2(null, null);
			((Control)base.DesignerPreviewControl).SuspendLayout();
			((Control)base.DesignerPreviewControl).Dock = (DockStyle)0;
			((Control)base.DesignerPreviewControl).Anchor = (AnchorStyles)15;
			((Control)base.DesignerPreviewControl).TabIndex = 0;
			((Control)base.DesignerPreviewControl).SetBounds(0, ((Control)RibbonControl).Height, ((Control)this).Width, ((Control)this).Height - ((Control)RibbonControl).Height);
			base.DesignerPreviewControl.ViewerControl.ShowStatusBar = true;
			((Control)base.DesignerPreviewControl).BringToFront();
			((Control)base.DesignerPreviewControl).Visible = true;
			base.DesignerPreviewControl?.ViewerControl?.InitShortcuts();
			base.RibbonDesignerUserControl?.ResetShortcutOpenFile();
			((Control)base.DesignerPreviewControl).ResumeLayout();
		}
	}

	private void CreateDashboardViewer(StiReport report)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		UserControl viewerControl = StiDashboardViewerAssembly.GetViewerControl(report, TabControl.SelectedTabIndex);
		if (viewerControl == null)
		{
			MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), "Dashboard Viewer from the 'Stimulsoft.Dashboard.Viewer.dll' wasn't found!", "Designer");
			return;
		}
		((Control)viewerControl).Location = new Point(0, ((Control)RibbonControl).Height);
		((Control)viewerControl).Name = "dashboardViewerControl";
		((Control)viewerControl).Size = new Size(((Control)this).Width, ((Control)this).Height - ((Control)viewerControl).Top);
		((Control)viewerControl).Anchor = (AnchorStyles)15;
		((Control)viewerControl).TabIndex = 0;
		if (isPreview)
		{
			((Control)this).Controls.Add((Control)(object)viewerControl);
			((Control)viewerControl).BringToFront();
			((Control)viewerControl).Focus();
			Timer timer = new Timer();
			timer.Interval = 1500;
			timer.Tick += delegate
			{
				timer.Stop();
				((Control)viewerControl).Focus();
				((Component)(object)timer).Dispose();
			};
			timer.Start();
		}
	}

	public StiRibbonDesignerControl()
		: this(null)
	{
	}

	public StiRibbonDesignerControl(StiReport report)
		: base(report, isRibbonGuiMode: true)
	{
		base.ShowRibbonStatusBar = true;
		StiRibbonDesignerUserControl stiRibbonDesignerUserControl = new StiRibbonDesignerUserControl(this);
		((Control)this).Controls.Add((Control)(object)stiRibbonDesignerUserControl.RibbonControl);
		EnableRibbonBackState();
		StiRibbonGuiService service = StiRibbonGuiService.GetService();
		ShowHelp = service.ShowHelp;
		ShowLocalizationMenu = service.ShowLocalizationMenu;
		ShowOfficeButton = service.ShowOfficeButton;
		ShowTabHome = service.ShowTabHome;
		ShowTabHomeBarAlignment = service.ShowTabHomeBarAlignment;
		ShowTabHomeBarBorders = service.ShowTabHomeBarBorders;
		ShowTabHomeBarClipboard = service.ShowTabHomeBarClipboard;
		ShowTabHomeBarFont = service.ShowTabHomeBarFont;
		ShowTabHomeBarStyle = service.ShowTabHomeBarStyle;
		ShowTabHomeBarTextFormat = service.ShowTabHomeBarTextFormat;
		ShowTabLayout = service.ShowTabLayout;
		ShowTabLayoutBarArrange = service.ShowTabLayoutBarArrange;
		ShowTabPage = service.ShowTabPage;
		ShowTabPageBarPageSetup = service.ShowTabPageBarPageSetup;
		ShowTabViewBarViewOptions = service.ShowTabViewBarViewOptions;
		ShowMainMenuNew = service.ShowMainMenuNew;
		ShowMainMenuReportOpen = service.ShowMainMenuReportOpen;
		ShowMainMenuReportSave = service.ShowMainMenuReportSave;
		ShowMainMenuSaveAs = service.ShowMainMenuSaveAs;
		ShowMainMenuCheckForIssues = service.ShowMainMenuCheckForIssues;
		ShowMainMenuReportSetup = service.ShowMainMenuReportSetup;
		ShowMainMenuRecentFiles = service.ShowMainMenuRecentFiles;
		ShowMainMenuClose = service.ShowMainMenuClose;
		ShowMainMenuOptions = service.ShowMainMenuOptions;
		ShowMainMenuExit = service.ShowMainMenuExit;
		StiRibbonToolboxToolbarService stiRibbonToolboxToolbarService = StiRibbonToolboxToolbarService.GetService(this);
		if (stiRibbonToolboxToolbarService == null)
		{
			stiRibbonToolboxToolbarService = new StiRibbonToolboxToolbarService();
		}
		stiRibbonToolboxToolbarService.Init(this);
	}

	private void InitializeComponent()
	{
		((ISupportInitialize)ContextMenuBar).BeginInit();
		DockSiteTopBars.SuspendLayout();
		((ISupportInitialize)MainMenu).BeginInit();
		((Control)this).SuspendLayout();
		((Control)DockSiteBottom).Location = new Point(0, 354);
		((Control)DockSiteLeft).Location = new Point(0, 76);
		((Control)DockSiteLeft).Size = new Size(0, 278);
		((Control)DockSiteRight).Location = new Point(552, 76);
		((Control)DockSiteRight).Size = new Size(0, 278);
		((Control)DockSiteTop).Location = new Point(0, 76);
		((Control)DockSiteLeftBars).Location = new Point(0, 76);
		((Control)DockSiteLeftBars).Size = new Size(36, 278);
		((Control)DockSiteRightBars).Location = new Point(552, 76);
		((Control)DockSiteRightBars).Size = new Size(0, 278);
		((Control)DockSiteTopBars).Size = new Size(552, 76);
		((Control)DockSiteTopBars).Controls.SetChildIndex((Control)(object)MainMenu, 0);
		((Control)DockSiteBottomBars).Location = new Point(0, 354);
		((Control)DockSiteBottomBars).Size = new Size(552, 25);
		((Control)this).Name = "StiRibbonDesignerControl";
		((ISupportInitialize)ContextMenuBar).EndInit();
		DockSiteTopBars.ResumeLayout(performLayout: false);
		((ISupportInitialize)MainMenu).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
