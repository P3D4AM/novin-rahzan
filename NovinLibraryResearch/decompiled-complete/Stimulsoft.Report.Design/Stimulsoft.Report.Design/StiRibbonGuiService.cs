using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Design;

public class StiRibbonGuiService : StiDesignerInitService
{
	internal class StiApplicationMenuItem
	{
		public string Text { get; set; }

		public EventHandler Handler { get; set; }

		public int Index { get; set; }

		public Image Image { get; set; }

		public bool BeginGroup { get; set; }
	}

	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDesigner");

	public sealed override Type ServiceType => typeof(StiRibbonGuiService);

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowOfficeButton
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowOfficeButton;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowOfficeButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowLocalizationMenu
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowLocalizationMenu;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowLocalizationMenu = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelp
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowHelp;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowHelp = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHome
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHome;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHome = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarClipboard
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarClipboard;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarClipboard = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarFont
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarFont;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarFont = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarAlignment
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarAlignment;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarAlignment = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarBorders
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarBorders;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarBorders = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarTextFormat
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarTextFormat;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarTextFormat = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabHomeBarStyle
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabHomeBarStyle;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabHomeBarStyle = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabPage
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabPage;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabPage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabPageBarPageSetup
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabPageBarPageSetup;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabPageBarPageSetup = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowTabPageBarWatermarkImage property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabPageBarWatermarkText
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowTabPageBarWatermarkImage property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabPageBarWatermarkImage
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabLayout
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabLayout;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabLayout = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabLayoutBarArrange
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabLayoutBarArrange;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabLayoutBarArrange = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowTabLayoutBarDockStyle property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabLayoutBarDockStyle
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowTabView property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabView
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabViewBarViewOptions
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabViewBarViewOptions;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabViewBarViewOptions = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTabPreview
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowTabPreview;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowTabPreview = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuNew
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuNew;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuReportNew
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuReportNew;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuReportNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuReportWizardNew property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportWizardNew
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuPageNew property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageNew
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuFormNew property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuFormNew
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuReportOpen
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuReportOpen;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuReportOpen = value;
		}
	}

	[Obsolete("Property 'ShowMainMenuReportOpenFromGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuPageOpen property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageOpen
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuReportSave
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuReportSave;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuReportSave = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuSaveAs
	{
		get
		{
			return ShowMainMenuReportSaveAs;
		}
		set
		{
			ShowMainMenuReportSaveAs = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuReportSaveAs
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuReportSaveAs;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuReportSaveAs = value;
		}
	}

	[Obsolete("Property 'ShowMainMenuReportSaveAsToGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuPageSaveAs property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageSaveAs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuPageDelete property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageDelete
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("ShowMainMenuReportPreview property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportPreview
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuCheckForIssues
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuCheckForIssues;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuCheckForIssues = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuReportSetup
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuReportSetup;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuReportSetup = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuRecentFiles
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuRecentFiles;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuRecentFiles = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuClose
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuClose;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuClose = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuOptions
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuOptions;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuOptions = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMainMenuExit
	{
		get
		{
			return StiOptions.Designer.Ribbon.ShowMainMenuExit;
		}
		set
		{
			StiOptions.Designer.Ribbon.ShowMainMenuExit = value;
		}
	}

	internal List<StiApplicationMenuItem> ApplicationMenuItems { get; set; }

	public void AddApplicationMenuItem(string text, EventHandler handler)
	{
		AddApplicationMenuItem(text, handler, null);
	}

	public void AddApplicationMenuItem(string text, EventHandler handler, Image image)
	{
		AddApplicationMenuItem(text, handler, image, -1);
	}

	public void AddApplicationMenuItem(string text, EventHandler handler, Image image, int index)
	{
		AddApplicationMenuItem(text, handler, image, index, addSeparatorBefore: false);
	}

	public void AddApplicationMenuItem(string text, EventHandler handler, Image image, int index, bool addSeparatorBefore)
	{
		StiApplicationMenuItem item = new StiApplicationMenuItem
		{
			Text = text,
			Handler = handler,
			Image = image,
			Index = index,
			BeginGroup = addSeparatorBefore
		};
		if (ApplicationMenuItems == null)
		{
			ApplicationMenuItems = new List<StiApplicationMenuItem>();
		}
		ApplicationMenuItems.Add(item);
	}

	public int IndexOf(string text)
	{
		if (ApplicationMenuItems != null)
		{
			foreach (StiApplicationMenuItem applicationMenuItem in ApplicationMenuItems)
			{
				if (applicationMenuItem.Text == text)
				{
					return ApplicationMenuItems.IndexOf(applicationMenuItem);
				}
			}
		}
		return -1;
	}

	public static StiRibbonGuiService GetService()
	{
		return StiDesignerOptions.RibbonGui.FirstOrDefault();
	}

	public override void Init(StiDesignerControl designer)
	{
		if (!StiOptions.Configuration.IsWeb && StiOptions.Engine.FullTrust)
		{
			base.Init(designer);
		}
	}
}
